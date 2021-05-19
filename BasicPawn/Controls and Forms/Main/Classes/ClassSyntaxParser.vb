'BasicPawn
'Copyright(C) 2021 Externet

'This program Is free software: you can redistribute it And/Or modify
'it under the terms Of the GNU General Public License As published by
'the Free Software Foundation, either version 3 Of the License, Or
'(at your option) any later version.

'This program Is distributed In the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty Of
'MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License For more details.

'You should have received a copy Of the GNU General Public License
'along with this program. If Not, see < http: //www.gnu.org/licenses/>.

#Const SEARCH_EVERYWHERE = (DEBUG AndAlso False)
#Const PROFILE_PARSING = (DEBUG AndAlso False)
#Const DUMP_TO_FILE = (DEBUG AndAlso True)

Imports System.Text
Imports System.Text.RegularExpressions

Public Class ClassSyntaxParser
    Private g_mFormMain As FormMain
    Private g_mSyntaxParsingThreads As New ClassSyncList(Of KeyValuePair(Of String, Threading.Thread))

    Private g_lUpdateRequests As New ClassSyncList(Of STRUC_SYNTAX_PARSE_TAB_REQUEST)
    Private g_lAutocompleteCache As New ClassSyncList(Of STRUC_AUTOCOMPLETE_CACHE)

    Private _lock As New Object

    Class STRUC_SYNTAX_PARSE_TAB_REQUEST
        Public sTabIdentifier As String
        Public iOptionFlags As ENUM_PARSE_OPTIONS_FLAGS

        Sub New(_TabIdentifier As String, _OptionFlags As ENUM_PARSE_OPTIONS_FLAGS)
            sTabIdentifier = _TabIdentifier
            iOptionFlags = _OptionFlags
        End Sub
    End Class

    Class STRUC_AUTOCOMPLETE_CACHE
        Private g_sIdentifier As String
        Private g_iType As ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE
        Private g_lAutocompleteItems As New ClassSyncList(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

        Public Sub New(_Identifier As String, _Type As ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE, _AutocompleteItems As ClassSyntaxTools.STRUC_AUTOCOMPLETE())
            g_sIdentifier = _Identifier
            g_iType = _Type

            'Clone array
            For Each mItem In _AutocompleteItems
                g_lAutocompleteItems.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mItem))
            Next
        End Sub

        Public ReadOnly Property m_Identifier As String
            Get
                Return g_sIdentifier
            End Get
        End Property

        Public ReadOnly Property m_Type As ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE
            Get
                Return g_iType
            End Get
        End Property

        Public ReadOnly Property m_AutocompleteItems As ClassSyncList(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Get
                Return g_lAutocompleteItems
            End Get
        End Property
    End Class

    Public Event OnSyntaxParseStarted(iUpdateType As ENUM_PARSE_TYPE_FLAGS, sTabIdentifier As String, iOptionFlags As ENUM_PARSE_OPTIONS_FLAGS)
    Public Event OnSyntaxParseSuccess(iUpdateType As ENUM_PARSE_TYPE_FLAGS, sTabIdentifier As String, iOptionFlags As ENUM_PARSE_OPTIONS_FLAGS, iFullParseError As ENUM_PARSE_ERROR, iVarParseError As ENUM_PARSE_ERROR)
    Public Event OnSyntaxParseEnd()
    Public Event OnSyntaxParseAbort()

    Public Sub New(f As FormMain)
        g_mFormMain = f
    End Sub

    Enum ENUM_PARSE_ERROR
        INVALID_TAB
        INVALID_FILE
        [ERROR]
        UNCHANGED = 0
        CACHED
        UPDATED
    End Enum

    Enum ENUM_PARSE_TYPE_FLAGS
        ALL = -1
        FULL_PARSE = (1 << 0)
        VAR_PARSE = (1 << 1)
    End Enum

    Enum ENUM_PARSE_OPTIONS_FLAGS
        NOONE = 0
        FORCE_UPDATE = (1 << 0)
        FORCE_SCHEDULE = (1 << 1)
    End Enum

    ReadOnly Property m_UpdateRequests As ClassSyncList(Of STRUC_SYNTAX_PARSE_TAB_REQUEST)
        Get
            Return g_lUpdateRequests
        End Get
    End Property

    ReadOnly Property m_AutocompleteCache As ClassSyncList(Of STRUC_AUTOCOMPLETE_CACHE)
        Get
            Return g_lAutocompleteCache
        End Get
    End Property

    ''' <summary>
    ''' Starts the syntax parse thread.
    ''' </summary> 
    ''' <returns></returns>
    Public Function StartUpdate(iUpdateType As ENUM_PARSE_TYPE_FLAGS) As Boolean
        Return StartUpdate(iUpdateType, g_mFormMain.g_ClassTabControl.m_ActiveTab, ENUM_PARSE_OPTIONS_FLAGS.NOONE)
    End Function

    Public Function StartUpdate(iUpdateType As ENUM_PARSE_TYPE_FLAGS, mTab As ClassTabControl.ClassTab, iOptionFlags As ENUM_PARSE_OPTIONS_FLAGS) As Boolean
        Return StartUpdate(iUpdateType, mTab.m_Identifier, iOptionFlags)
    End Function

    Public Function StartUpdate(iUpdateType As ENUM_PARSE_TYPE_FLAGS, sTabIdentifier As String, iOptionFlags As ENUM_PARSE_OPTIONS_FLAGS) As Boolean
        If (String.IsNullOrEmpty(sTabIdentifier)) Then
            Return False
        End If

        If (IsThreadLimitReached() OrElse IsThreadProcessing(sTabIdentifier)) Then
            Return False
        End If

        Dim mSyntaxParsingThread As New Threading.Thread(Sub()
                                                             Try
                                                                 Try
                                                                     Dim iFullParseError As ENUM_PARSE_ERROR = ENUM_PARSE_ERROR.UNCHANGED
                                                                     Dim iVarParseError As ENUM_PARSE_ERROR = ENUM_PARSE_ERROR.UNCHANGED

                                                                     If ((iUpdateType And ENUM_PARSE_TYPE_FLAGS.FULL_PARSE) <> 0) Then
                                                                         RaiseEvent OnSyntaxParseStarted(iUpdateType, sTabIdentifier, iOptionFlags)
                                                                         iFullParseError = FullSyntaxParse_Thread(sTabIdentifier, iOptionFlags)
                                                                     End If

                                                                     If ((iUpdateType And ENUM_PARSE_TYPE_FLAGS.VAR_PARSE) <> 0) Then
                                                                         RaiseEvent OnSyntaxParseStarted(iUpdateType, sTabIdentifier, iOptionFlags)
                                                                         iVarParseError = VarSyntaxParse_Thread(sTabIdentifier, iOptionFlags)
                                                                     End If

                                                                     RaiseEvent OnSyntaxParseSuccess(iUpdateType, sTabIdentifier, iOptionFlags, iFullParseError, iVarParseError)
                                                                 Catch ex As Threading.ThreadAbortException
                                                                     Throw
                                                                 Catch ex As Exception
                                                                     ClassExceptionLog.WriteToLog(ex)
                                                                 End Try

                                                                 RaiseEvent OnSyntaxParseEnd()
                                                             Finally
                                                                 g_mSyntaxParsingThreads.RemoveAll(Function(x As KeyValuePair(Of String, Threading.Thread))
                                                                                                       Return x.Key = sTabIdentifier
                                                                                                   End Function)
                                                             End Try
                                                         End Sub) With {
            .Priority = Threading.ThreadPriority.Lowest,
            .IsBackground = True
        }
        g_mSyntaxParsingThreads.Add(New KeyValuePair(Of String, Threading.Thread)(sTabIdentifier, mSyntaxParsingThread))
        mSyntaxParsingThread.Start()

        Return True
    End Function

    ''' <summary>
    ''' Starts the syntax parse thread. Adds task to background thread if thread is already running.
    ''' </summary> 
    ''' <returns></returns>
    Public Function StartUpdateSchedule(iUpdateType As ENUM_PARSE_TYPE_FLAGS) As Boolean
        Return StartUpdateSchedule(iUpdateType, g_mFormMain.g_ClassTabControl.m_ActiveTab, ENUM_PARSE_OPTIONS_FLAGS.NOONE)
    End Function

    Public Function StartUpdateSchedule(iUpdateType As ENUM_PARSE_TYPE_FLAGS, mTab As ClassTabControl.ClassTab, iOptionFlags As ENUM_PARSE_OPTIONS_FLAGS) As Boolean
        Return StartUpdateSchedule(iUpdateType, mTab.m_Identifier, iOptionFlags)
    End Function

    Public Function StartUpdateSchedule(iUpdateType As ENUM_PARSE_TYPE_FLAGS, sTabIdentifier As String, iOptionFlags As ENUM_PARSE_OPTIONS_FLAGS) As Boolean
        If (String.IsNullOrEmpty(sTabIdentifier)) Then
            Return False
        End If

        If (StartUpdate(iUpdateType, sTabIdentifier, iOptionFlags)) Then
            If ((iUpdateType And ENUM_PARSE_TYPE_FLAGS.FULL_PARSE) <> 0) Then
                'Remove next tab request
                Dim mTabRequest = m_UpdateRequests.Find(Function(i As STRUC_SYNTAX_PARSE_TAB_REQUEST)
                                                            Return (i.sTabIdentifier = sTabIdentifier)
                                                        End Function)

                If (mTabRequest IsNot Nothing) Then
                    m_UpdateRequests.Remove(mTabRequest)
                End If
            End If

            Return True
        Else
            If ((iUpdateType And ENUM_PARSE_TYPE_FLAGS.FULL_PARSE) <> 0) Then
                Dim mTabRequest = m_UpdateRequests.Find(Function(i As STRUC_SYNTAX_PARSE_TAB_REQUEST)
                                                            Return (i.sTabIdentifier = sTabIdentifier AndAlso i.iOptionFlags = iOptionFlags)
                                                        End Function)

                'Add when no request has been found OR the option flags is enforcing it
                If ((iOptionFlags And ENUM_PARSE_OPTIONS_FLAGS.FORCE_SCHEDULE) <> 0 OrElse mTabRequest Is Nothing) Then
                    m_UpdateRequests.Add(New STRUC_SYNTAX_PARSE_TAB_REQUEST(sTabIdentifier, iOptionFlags))
                End If
            End If

            Return False
        End If
    End Function

    Public Function GetAliveThreadCount() As Integer
        Return g_mSyntaxParsingThreads.Count
    End Function

    Public Function IsThreadLimitReached() As Boolean
        Return (GetAliveThreadCount() > ClassSettings.GetMaxParsingThreads() - 1)
    End Function

    Public Function IsThreadProcessing(mTab As ClassTabControl.ClassTab) As Boolean
        Return IsThreadProcessing(mTab.m_Identifier)
    End Function

    Public Function IsThreadProcessing(sTabIdentifier As String) As Boolean
        Return g_mSyntaxParsingThreads.Exists(Function(x As KeyValuePair(Of String, Threading.Thread)) x.Key = sTabIdentifier)
    End Function

    ''' <summary>
    ''' Stops the syntax parse thread
    ''' </summary>
    Public Sub StopUpdate()
        RaiseEvent OnSyntaxParseAbort()

        For Each mKey In g_mSyntaxParsingThreads.ToArray
            ClassThread.Abort(mKey.Value)
        Next
    End Sub

    Private Function FullSyntaxParse_Thread(sTabIdentifier As String, iOptionFlags As ENUM_PARSE_OPTIONS_FLAGS) As ENUM_PARSE_ERROR
        Try
            Dim mRequestTab As ClassTabControl.ClassTab = ClassThread.ExecEx(Of ClassTabControl.ClassTab)(g_mFormMain, Function() g_mFormMain.g_ClassTabControl.GetTabByIdentifier(sTabIdentifier))
            If (mRequestTab Is Nothing) Then
                Return ENUM_PARSE_ERROR.INVALID_TAB
            End If

            Dim sActiveTabIdentifier As String = mRequestTab.m_Identifier
            Dim sRequestedSourceFile As String = mRequestTab.m_File
            Dim iRequestedLangauge As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = mRequestTab.m_Language
            Dim sRequestedSource As String = ClassThread.ExecEx(Of String)(mRequestTab, Function() mRequestTab.m_TextEditor.Document.TextContent)
            Dim mRequestedConfig As ClassConfigs.STRUC_CONFIG_ITEM = ClassThread.ExecEx(Of ClassConfigs.STRUC_CONFIG_ITEM)(mRequestTab, Function() mRequestTab.m_ActiveConfig)
            Dim mTabs As ClassTabControl.ClassTab() = ClassThread.ExecEx(Of ClassTabControl.ClassTab())(g_mFormMain, Function() g_mFormMain.g_ClassTabControl.GetAllTabs())

            If (String.IsNullOrEmpty(sRequestedSourceFile) OrElse Not IO.File.Exists(sRequestedSourceFile)) Then
                Return ENUM_PARSE_ERROR.INVALID_FILE
            End If

            Dim mIncludeWatch As New Stopwatch
            Dim mLanguageWatch As New Stopwatch
            Dim mPreWatch As New Stopwatch
            Dim mPostWatch As New Stopwatch
            Dim mFinalizeWatch As New Stopwatch
            Dim mApplyWatch As New Stopwatch

            Dim lNewAutocompleteList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Dim mParser As New ClassParser()

            Dim lIncludeFiles As New List(Of KeyValuePair(Of String, String))
            Dim lIncludeFilesFull As New List(Of KeyValuePair(Of String, String))

            mIncludeWatch.Start()
            For Each sInclude In GetIncludeFiles(mRequestedConfig, sRequestedSource, sRequestedSourceFile, sRequestedSourceFile)
                lIncludeFiles.Add(New KeyValuePair(Of String, String)(sTabIdentifier, sInclude))
            Next
            For Each sInclude In GetIncludeFiles(mRequestedConfig, sRequestedSource, sRequestedSourceFile, sRequestedSourceFile, True)
                lIncludeFilesFull.Add(New KeyValuePair(Of String, String)(sTabIdentifier, sInclude))
            Next

            'Find main tab of include tabs
            g_mFormMain.g_ClassTabControl.GetTabIncludesByReferences(mRequestTab, mTabs, lIncludeFiles, lIncludeFilesFull)

            'Save includes first, they wont be modified below this anyways
            mRequestTab.m_IncludesGroup.m_IncludeFiles.DoSync(
                Sub()
                    mRequestTab.m_IncludesGroup.m_IncludeFiles.Clear()
                    mRequestTab.m_IncludesGroup.m_IncludeFiles.AddRange(lIncludeFiles.ToArray)
                End Sub)

            mRequestTab.m_IncludesGroup.m_IncludeFilesFull.DoSync(
                Sub()
                    mRequestTab.m_IncludesGroup.m_IncludeFilesFull.Clear()
                    mRequestTab.m_IncludesGroup.m_IncludeFilesFull.AddRange(lIncludeFilesFull.ToArray)
                End Sub)
            mIncludeWatch.Stop()

            'Detect current mod type...
            mLanguageWatch.Start()
            If (mRequestedConfig.g_iLanguage = ClassConfigs.STRUC_CONFIG_ITEM.ENUM_LANGUAGE_DETECT_TYPE.AUTO_DETECT) Then
                Dim iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = CType(-1, ClassSyntaxTools.ENUM_LANGUAGE_TYPE)

                For i = 0 To lIncludeFiles.Count - 1
                    '... by includes
                    Select Case (IO.Path.GetFileName(lIncludeFiles(i).Value).ToLower)
                        Case "sourcemod.inc"
                            iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN
                            Exit For

                        Case "amxmodx.inc"
                            iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.AMXMODX
                            Exit For
                    End Select

                    '... by extension
                    Select Case (IO.Path.GetExtension(lIncludeFiles(i).Value).ToLower)
                        Case ".sp"
                            iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN
                            Exit For

                        Case ".sma"
                            iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.AMXMODX
                            Exit For

                        Case ".p", ".pwn"
                            iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.PAWN
                            Exit For
                    End Select
                Next

                If (iRequestedLangauge <> iLanguage) Then
                    Select Case (iLanguage)
                        Case ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("Auto-Detected language: SourcePawn ({0})", IO.Path.GetFileName(sRequestedSourceFile)))

                        Case ClassSyntaxTools.ENUM_LANGUAGE_TYPE.AMXMODX
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("Auto-Detected language: AMX Mod X ({0})", IO.Path.GetFileName(sRequestedSourceFile)))

                        Case ClassSyntaxTools.ENUM_LANGUAGE_TYPE.PAWN
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("Auto-Detected language: Pawn ({0})", IO.Path.GetFileName(sRequestedSourceFile)))

                    End Select
                End If

                If (iLanguage > -1) Then
                    iRequestedLangauge = iLanguage
                End If
            Else
                Select Case (mRequestedConfig.g_iLanguage)
                    Case ClassConfigs.STRUC_CONFIG_ITEM.ENUM_LANGUAGE_DETECT_TYPE.SOURCEPAWN
                        iRequestedLangauge = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN

                    Case ClassConfigs.STRUC_CONFIG_ITEM.ENUM_LANGUAGE_DETECT_TYPE.AMXMODX
                        iRequestedLangauge = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.AMXMODX
                End Select
            End If
            mLanguageWatch.Stop()

            'Set mod type
            mRequestTab.m_Language = iRequestedLangauge

            Dim sAutocompleteIdentifier As String = mRequestTab.m_AutocompleteGroup.GenerateAutocompleteIdentifier()

            If ((iOptionFlags And ENUM_PARSE_OPTIONS_FLAGS.FORCE_UPDATE) = 0) Then
                'Only update syntax parse if files have changed.
                If (True) Then
                    If (mRequestTab.m_AutocompleteGroup.CheckAutocompleteIdentifier(ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE.FULL, sAutocompleteIdentifier)) Then
                        Return ENUM_PARSE_ERROR.UNCHANGED
                    End If
                End If

                'Get autocomplete items from cache
                If (ClassSettings.g_iSettingsMaxParsingCache > 0) Then
                    Dim mCacheItem = m_AutocompleteCache.Find(Function(a As STRUC_AUTOCOMPLETE_CACHE)
                                                                  Return (a.m_Identifier = sAutocompleteIdentifier AndAlso a.m_Type = ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE.FULL)
                                                              End Function)

                    If (mCacheItem IsNot Nothing) Then
                        mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.DoSync(
                            Sub()
                                mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.Clear()

                                'Clone autocomplete items
                                For Each mItem In mCacheItem.m_AutocompleteItems.ToArray
                                    mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mItem))
                                Next
                            End Sub)

                        Return ENUM_PARSE_ERROR.CACHED
                    End If
                End If
            End If

            'Add debugger placeholder variables and methods
            lNewAutocompleteList.AddRange(ClassDebuggerTools.ClassDebuggerHelpers.GetDebuggerAutocomplete)

            'Add preprocessor stuff
            lNewAutocompleteList.AddRange(mParser.GetPreprocessorKeywords(lIncludeFilesFull.ToArray))

            'Add editor cmd stuff
            lNewAutocompleteList.AddRange(mParser.GetTextEditorCommands())

            'Add config defines
            lNewAutocompleteList.AddRange(mParser.GetConfigDefines(mRequestedConfig, iRequestedLangauge))

            'Parse everything. Methods etc.
            If (True) Then
                Dim sSourceList As New List(Of KeyValuePair(Of String, String)) '{sFile, sSource}

                mPreWatch.Start()
                Dim i As Integer
                For i = 0 To lIncludeFiles.Count - 1
                    mParser.ProcessFullSyntaxPre(g_mFormMain, sRequestedSource, sRequestedSourceFile, lIncludeFiles(i).Value, sSourceList, lNewAutocompleteList, iRequestedLangauge)
                Next
                mPreWatch.Stop()

                mPostWatch.Start()
                For i = 0 To sSourceList.Count - 1
                    mParser.ProcessFullSyntaxPost(g_mFormMain, sRequestedSource, sRequestedSourceFile, sSourceList(i).Key, sSourceList(i).Value, lNewAutocompleteList, iRequestedLangauge)
                Next
                mPostWatch.Stop()
            End If

            'Finalize
            mFinalizeWatch.Start()
            mParser.ProcessFullSyntaxFinalize(g_mFormMain, lNewAutocompleteList)
            mFinalizeWatch.Stop()

            'Save everything and update syntax 
            mApplyWatch.Start()
            mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.DoSync(
                Sub()
                    mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.RemoveAll(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = 0)
                    mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.AddRange(lNewAutocompleteList.ToArray)

                    mRequestTab.m_AutocompleteGroup.m_AutocompleteIdentifierItem(ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE.FULL) = sAutocompleteIdentifier

                    If (ClassSettings.g_iSettingsMaxParsingCache > 0) Then
                        m_AutocompleteCache.DoSync(
                            Sub()
                                m_AutocompleteCache.RemoveAll(Function(a As STRUC_AUTOCOMPLETE_CACHE)
                                                                  Return (a.m_Identifier = sAutocompleteIdentifier AndAlso a.m_Type = ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE.FULL)
                                                              End Function)

                                m_AutocompleteCache.Add(New STRUC_AUTOCOMPLETE_CACHE(sAutocompleteIdentifier, ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE.FULL, mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.ToArray))
                            End Sub)
                    End If

#If DUMP_TO_FILE Then
                    If (True) Then
                        Dim mSB As New StringBuilder

                        For Each mItem In lNewAutocompleteList.ToArray
                            mSB.AppendLine(mItem.m_Filename)

                            For Each sKey In mItem.m_Data.Keys
                                If (TypeOf mItem.m_Data(sKey) Is String()) Then
                                    mSB.AppendLine(vbTab & sKey & "=" & String.Join(", ", CType(mItem.m_Data(sKey), String())))
                                Else
                                    mSB.AppendLine(vbTab & sKey & "=" & mItem.m_Data(sKey).ToString)
                                End If
                            Next
                        Next

                        SyncLock _lock
                            Dim sDumpDir As String = IO.Path.Combine(Application.StartupPath, "DUMP")
                            IO.Directory.CreateDirectory(sDumpDir)
                            IO.File.WriteAllText(IO.Path.Combine(sDumpDir, "full.txt"), mSB.ToString)
                        End SyncLock
                    End If
#End If
                End Sub)

            CleanAutocompleteCache(ClassSettings.g_iSettingsMaxParsingCache)

            mApplyWatch.Stop()

#If PROFILE_PARSING Then
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_DEBUG, "Syntax parsing finished!")
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_DEBUG, "Times:")
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Includes: " & mIncludeWatch.Elapsed.ToString)
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Language: " & mLanguageWatch.Elapsed.ToString)
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Pre: " & mPreWatch.Elapsed.ToString)
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Post: " & mPostWatch.Elapsed.ToString)
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Finalize: " & mFinalizeWatch.Elapsed.ToString)
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Apply: " & mApplyWatch.Elapsed.ToString)
#End If

            Return ENUM_PARSE_ERROR.UPDATED
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Syntax parsing failed! " & ex.Message, False, False)
            ClassExceptionLog.WriteToLog(ex)
        End Try

        Return ENUM_PARSE_ERROR.ERROR
    End Function

    Private Function VarSyntaxParse_Thread(sTabIdentifier As String, iOptionFlags As ENUM_PARSE_OPTIONS_FLAGS) As ENUM_PARSE_ERROR
        Try
            Dim mRequestTab As ClassTabControl.ClassTab = ClassThread.ExecEx(Of ClassTabControl.ClassTab)(g_mFormMain, Function() g_mFormMain.g_ClassTabControl.GetTabByIdentifier(sTabIdentifier))
            If (mRequestTab Is Nothing) Then
                Return ENUM_PARSE_ERROR.INVALID_TAB
            End If

            Dim sRequestedSourceFile As String = mRequestTab.m_File
            Dim iRequestedLangauge As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = mRequestTab.m_Language
            Dim sRequestedSource As String = ClassThread.ExecEx(Of String)(mRequestTab, Function() mRequestTab.m_TextEditor.Document.TextContent)

            If (String.IsNullOrEmpty(sRequestedSourceFile) OrElse Not IO.File.Exists(sRequestedSourceFile)) Then
                Return ENUM_PARSE_ERROR.INVALID_FILE
            End If

            Dim lOldVarAutocompleteList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            For Each mItem In mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.ToArray
                lOldVarAutocompleteList.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mItem))
            Next

            'No autocomplete entries?
            If (lOldVarAutocompleteList.Count < 1) Then
                Return ENUM_PARSE_ERROR.UNCHANGED
            End If

            Dim mPreWatch As New Stopwatch
            Dim mFinalizeWatch As New Stopwatch
            Dim mApplyWatch As New Stopwatch

            Dim lNewVarAutocompleteList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Dim mParser As New ClassParser

            Dim sAutocompleteIdentifier As String = mRequestTab.m_AutocompleteGroup.GenerateAutocompleteIdentifier

            If ((iOptionFlags And ENUM_PARSE_OPTIONS_FLAGS.FORCE_UPDATE) = 0) Then
                'Only update syntax if files have changed.
                If (True) Then
                    If (mRequestTab.m_AutocompleteGroup.CheckAutocompleteIdentifier(ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE.VARIABLE, sAutocompleteIdentifier)) Then
                        Return ENUM_PARSE_ERROR.UNCHANGED
                    End If
                End If

                'Get autocomplete items from cache
                If (ClassSettings.g_iSettingsMaxParsingCache > 0) Then
                    Dim mCacheItem = m_AutocompleteCache.Find(Function(a As STRUC_AUTOCOMPLETE_CACHE)
                                                                  Return (a.m_Identifier = sAutocompleteIdentifier AndAlso a.m_Type = ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE.VARIABLE)
                                                              End Function)

                    If (mCacheItem IsNot Nothing) Then
                        mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.DoSync(
                            Sub()
                                mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.Clear()

                                'Clone autocomplete items
                                For Each mItem In mCacheItem.m_AutocompleteItems.ToArray
                                    mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mItem))
                                Next
                            End Sub)

                        Return ENUM_PARSE_ERROR.CACHED
                    End If
                End If
            End If

            'Parse variables and create methodmaps for variables
            If (True) Then
                mPreWatch.Start()

                If (ClassSettings.g_iSettingsAutocompleteVarParseType = ClassSettings.ENUM_VAR_PARSE_TYPE.TAB) Then
                    mParser.ProcessVarSyntaxPre(g_mFormMain, sRequestedSource, sRequestedSourceFile, sRequestedSourceFile, lNewVarAutocompleteList, lOldVarAutocompleteList, iRequestedLangauge)
                Else
                    Dim mIncludeFiles = mRequestTab.m_IncludesGroup.m_IncludeFiles.ToArray
                    For i = 0 To mIncludeFiles.Length - 1
                        Select Case (ClassSettings.g_iSettingsAutocompleteVarParseType)
                            Case ClassSettings.ENUM_VAR_PARSE_TYPE.TAB_AND_INC
                                Dim bValid As Boolean = False

                                If (sRequestedSourceFile.ToLower = mIncludeFiles(i).Value.ToLower) Then
                                    bValid = True
                                End If

                                Select Case (IO.Path.GetExtension(mIncludeFiles(i).Value).ToLower)
                                    Case ".sp", ".sma", ".p", ".pwn"
                                        bValid = True
                                End Select

                                If (bValid) Then
                                    mParser.ProcessVarSyntaxPre(g_mFormMain, sRequestedSource, sRequestedSourceFile, (mIncludeFiles(i).Value), lNewVarAutocompleteList, lOldVarAutocompleteList, iRequestedLangauge)
                                End If

                            Case Else
                                mParser.ProcessVarSyntaxPre(g_mFormMain, sRequestedSource, sRequestedSourceFile, (mIncludeFiles(i).Value), lNewVarAutocompleteList, lOldVarAutocompleteList, iRequestedLangauge)
                        End Select
                    Next
                End If
                mPreWatch.Stop()

                mFinalizeWatch.Start()
                mParser.ProcessVarSyntaxFinalize(g_mFormMain, sRequestedSource, sRequestedSourceFile, lNewVarAutocompleteList, lOldVarAutocompleteList, iRequestedLangauge)
                mFinalizeWatch.Stop()
            End If

            mApplyWatch.Start()
            mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.DoSync(
                Sub()
                    mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.RemoveAll(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0)
                    mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.AddRange(lNewVarAutocompleteList.ToArray)

                    mRequestTab.m_AutocompleteGroup.m_AutocompleteIdentifierItem(ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE.VARIABLE) = sAutocompleteIdentifier

                    If (ClassSettings.g_iSettingsMaxParsingCache > 0) Then
                        m_AutocompleteCache.DoSync(
                            Sub()
                                m_AutocompleteCache.RemoveAll(Function(a As STRUC_AUTOCOMPLETE_CACHE)
                                                                  Return (a.m_Identifier = sAutocompleteIdentifier AndAlso a.m_Type = ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE.VARIABLE)
                                                              End Function)

                                m_AutocompleteCache.Add(New STRUC_AUTOCOMPLETE_CACHE(sAutocompleteIdentifier, ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE.VARIABLE, mRequestTab.m_AutocompleteGroup.m_AutocompleteItems.ToArray))
                            End Sub)
                    End If

#If DUMP_TO_FILE Then
                    If (True) Then
                        Dim mSB As New StringBuilder

                        For Each mItem In lNewVarAutocompleteList.ToArray
                            mSB.AppendLine(mItem.m_Filename)

                            For Each sKey In mItem.m_Data.Keys
                                If (TypeOf mItem.m_Data(sKey) Is String()) Then
                                    mSB.AppendLine(vbTab & sKey & "=" & String.Join(", ", CType(mItem.m_Data(sKey), String())))
                                Else
                                    mSB.AppendLine(vbTab & sKey & "=" & mItem.m_Data(sKey).ToString)
                                End If
                            Next
                        Next

                        SyncLock _lock
                            Dim sDumpDir As String = IO.Path.Combine(Application.StartupPath, "DUMP")
                            IO.Directory.CreateDirectory(sDumpDir)
                            IO.File.WriteAllText(IO.Path.Combine(sDumpDir, "var.txt"), mSB.ToString)
                        End SyncLock
                    End If
#End If
                End Sub)

            CleanAutocompleteCache(ClassSettings.g_iSettingsMaxParsingCache)

            mApplyWatch.Stop()

#If PROFILE_PARSING Then
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_DEBUG, "Variable syntax parsing finished!")
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_DEBUG, "Times:")
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Pre: " & mPreWatch.Elapsed.ToString)
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Finalize: " & mFinalizeWatch.Elapsed.ToString)
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Apply: " & mApplyWatch.Elapsed.ToString)
#End If

            Return ENUM_PARSE_ERROR.UPDATED
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Variable syntax parsing failed! " & ex.Message, False, False)
            ClassExceptionLog.WriteToLog(ex)
        End Try

        Return ENUM_PARSE_ERROR.ERROR
    End Function

    Private Sub CleanAutocompleteCache(iMaxItems As Integer)
        m_AutocompleteCache.DoSync(
            Sub()
                While m_AutocompleteCache.Count > iMaxItems
                    m_AutocompleteCache.PopFirst()
                End While
            End Sub)
    End Sub

    ''' <summary>
    ''' Gets all include files from a file
    ''' </summary>
    ''' <param name="sPath"></param>
    ''' <returns>Array if include file paths</returns>
    Public Function GetIncludeFiles(mConfig As ClassConfigs.STRUC_CONFIG_ITEM, sActiveSource As String, sActiveSourceFile As String, sPath As String, Optional bFindAll As Boolean = False, Optional iMaxDirectoryDepth As Integer = 10) As String()
        Dim lList As New List(Of String)

        GetIncludeFilesRecursive(mConfig, sActiveSource, sActiveSourceFile, sPath, lList)

        If (bFindAll) Then
            While True
#If SEARCH_EVERYWHERE Then
                If (String.IsNullOrEmpty(sActiveSourceFile) OrElse Not IO.File.Exists(sActiveSourceFile)) Then 
                    Exit While
                End If
#End If

                'Check includes
                Dim sIncludePaths As String
                If (mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    If (String.IsNullOrEmpty(sActiveSourceFile) OrElse Not IO.File.Exists(sActiveSourceFile)) Then
                        Exit While
                    End If
                    sIncludePaths = IO.Path.Combine(IO.Path.GetDirectoryName(sActiveSourceFile), "include")
                Else
                    sIncludePaths = mConfig.g_sIncludeFolders
                End If

#If SEARCH_EVERYWHERE Then
                'Check compiler
                Dim sCompilerPath As String
                If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    If (String.IsNullOrEmpty(sActiveSourceFile) OrElse Not IO.File.Exists(sActiveSourceFile)) Then 
                        Exit While
                    End If
                    sCompilerPath = IO.Path.GetDirectoryName(sActiveSourceFile)
                ElseIf (Not String.IsNullOrEmpty(ClassSettings.g_sConfigCompilerPath) AndAlso IO.File.Exists(ClassSettings.g_sConfigCompilerPath)) Then
                    sCompilerPath = IO.Path.GetDirectoryName(ClassSettings.g_sConfigCompilerPath)
                Else
                    sCompilerPath = ""
                End If
#End If

#If SEARCH_EVERYWHERE Then
                GetIncludeFilesRecursiveAll(IO.Path.GetDirectoryName(sActiveSourceFile), lList, iMaxDirectoryDepth)
#End If

                For Each sInclude As String In sIncludePaths.Split(";"c)
                    If (Not IO.Directory.Exists(sInclude)) Then
                        Continue For
                    End If

                    GetIncludeFilesRecursiveAll(mConfig, sInclude, sInclude, lList, iMaxDirectoryDepth)
                Next

#If SEARCH_EVERYWHERE Then
                GetIncludeFilesRecursiveAll(sCompilerPath, lList, iMaxDirectoryDepth)
#End If

                Exit While
            End While
        End If

        Return lList.ToArray
    End Function

    Private Sub GetIncludeFilesRecursiveAll(mConfig As ClassConfigs.STRUC_CONFIG_ITEM, sBaseInclude As String, sInclude As String, ByRef lList As List(Of String), iMaxDirectoryDepth As Integer)
        Dim sFiles As String()
        Dim sDirectories As String()

        sFiles = IO.Directory.GetFiles(sInclude)
        sDirectories = IO.Directory.GetDirectories(sInclude)

        For Each i As String In sFiles
            If (Not IO.File.Exists(i)) Then
                Continue For
            End If

            If (lList.Contains(i.ToLower)) Then
                Continue For
            End If

            Select Case (IO.Path.GetExtension(i).ToLower)
                Case ".sp", ".sma", ".p", ".pwn", ".inc"
                    lList.Add(i.ToLower)
            End Select
        Next

        If (iMaxDirectoryDepth < 1) Then
            Return
        End If

        For Each i As String In sDirectories
            GetIncludeFilesRecursiveAll(mConfig, sBaseInclude, i, lList, iMaxDirectoryDepth - 1)
        Next
    End Sub

    Private Sub GetIncludeFilesRecursive(mConfig As ClassConfigs.STRUC_CONFIG_ITEM, sActiveSource As String, sActiveSourceFile As String, sPath As String, ByRef lList As List(Of String))
        Dim sSource As String

        If (sActiveSourceFile.ToLower = sPath.ToLower) Then
            If (lList.Contains(sPath.ToLower)) Then
                Return
            End If

            lList.Add(sPath.ToLower)

            sSource = sActiveSource
        Else
            If (Not IO.File.Exists(sPath)) Then
                Return
            End If

            If (lList.Contains(sPath.ToLower)) Then
                Return
            End If

            lList.Add(sPath.ToLower)

            sSource = IO.File.ReadAllText(sPath)
        End If

        Dim lPathList As New List(Of String)
        Dim sCurrentDir As String = IO.Path.GetDirectoryName(sPath)

        Dim sCorrectPath As String
        Dim sMatchValue As String

        While True
            'Check includes
            Dim sIncludePaths As String
            If (mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                If (String.IsNullOrEmpty(sActiveSourceFile) OrElse Not IO.File.Exists(sActiveSourceFile)) Then
                    Exit While
                End If
                sIncludePaths = IO.Path.Combine(IO.Path.GetDirectoryName(sActiveSourceFile), "include")
            Else
                sIncludePaths = mConfig.g_sIncludeFolders
            End If

            'Check compiler
            Dim sCompilerPath As String
            If (mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                If (String.IsNullOrEmpty(sActiveSourceFile) OrElse Not IO.File.Exists(sActiveSourceFile)) Then
                    Exit While
                End If
                sCompilerPath = IO.Path.GetDirectoryName(sActiveSourceFile)
            ElseIf (Not String.IsNullOrEmpty(mConfig.g_sCompilerPath) AndAlso IO.File.Exists(mConfig.g_sCompilerPath)) Then
                sCompilerPath = IO.Path.GetDirectoryName(mConfig.g_sCompilerPath)
            Else
                sCompilerPath = ""
            End If

            For Each sInclude As String In sIncludePaths.Split(";"c)
                If (ClassSettings.g_bSettingsAlwaysLoadDefaultIncludes) Then
                    If (sActiveSourceFile.ToLower = sPath.ToLower) Then
                        For Each sDefaultInc As String In New String() {"sourcemod"}
                            Select Case (True)
                                Case IO.File.Exists(IO.Path.Combine(sInclude, sDefaultInc))
                                    sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sInclude, sDefaultInc))
                                Case IO.File.Exists(IO.Path.Combine(sCurrentDir, sDefaultInc))
                                    sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sCurrentDir, sDefaultInc))
                                Case IO.File.Exists(IO.Path.Combine(sCompilerPath, sDefaultInc))
                                    sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sCompilerPath, sDefaultInc))

                                Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sInclude, sDefaultInc)))
                                    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sInclude, sDefaultInc)))

                                Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sCurrentDir, sDefaultInc)))
                                    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sCurrentDir, sDefaultInc)))

                                Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sCompilerPath, sDefaultInc)))
                                    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sCompilerPath, sDefaultInc)))

                                Case Else
                                    Continue For
                            End Select

                            If (Not IO.File.Exists(sCorrectPath)) Then
                                Continue For
                            End If

                            If (lPathList.Contains(sCorrectPath.ToLower)) Then
                                Continue For
                            End If

                            lPathList.Add(sCorrectPath.ToLower)
                        Next
                    End If
                End If

                Using mSR As New IO.StringReader(sSource)
                    While True
                        Dim sLine As String = mSR.ReadLine()
                        If (sLine Is Nothing) Then
                            Exit While
                        End If

                        If (Not sLine.Contains("#include") AndAlso Not sLine.Contains("#tryinclude")) Then
                            Continue While
                        End If

                        Dim mMatch As Match = ClassRegexLock.Match(sLine, "^\s*#(include|tryinclude)\s+(?<PathInc>.*?)\s*$")
                        If (Not mMatch.Groups("PathInc").Success) Then
                            Continue While
                        End If

                        'Remove comments
                        'TODO: Add better check for detecting comments. Cant use ClassSyntaxSourceAnalysis here since the language is not defined yet...
                        sMatchValue = mMatch.Groups("PathInc").Value
                        sMatchValue = ClassRegexLock.Replace(sMatchValue, "//(.*?)$", "")
                        sMatchValue = ClassRegexLock.Replace(sMatchValue, "\/\*(.*?)$", "")
                        sMatchValue = sMatchValue.Replace("/"c, "\"c).Trim

                        Select Case (True)
                            Case sMatchValue.StartsWith("<") AndAlso sMatchValue.EndsWith(">")
                                sMatchValue = sMatchValue.TrimStart("<"c).TrimEnd(">"c)

                                Select Case (True)
                                    Case IO.File.Exists(IO.Path.Combine(sInclude, sMatchValue))
                                        sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sInclude, sMatchValue))
                                    Case IO.File.Exists(IO.Path.Combine(sCompilerPath, sMatchValue))
                                        sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sCompilerPath, sMatchValue))

                                    Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sInclude, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sInclude, sMatchValue)))

                                    Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sCompilerPath, sMatchValue)))

                                    Case Else
                                        Continue While
                                End Select

                            Case Else
                                sMatchValue = sMatchValue.TrimStart(""""c).TrimEnd(""""c)

                                Select Case (True)
                                    Case sMatchValue.Length > 1 AndAlso sMatchValue(1) = ":"c AndAlso IO.File.Exists(sMatchValue)
                                        sCorrectPath = IO.Path.GetFullPath(sMatchValue)

                                    Case IO.File.Exists(IO.Path.Combine(sInclude, sMatchValue))
                                        sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sInclude, sMatchValue))
                                    Case IO.File.Exists(IO.Path.Combine(sCurrentDir, sMatchValue))
                                        sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sCurrentDir, sMatchValue))
                                    Case IO.File.Exists(IO.Path.Combine(sCompilerPath, sMatchValue))
                                        sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sCompilerPath, sMatchValue))

                                    Case IO.File.Exists(String.Format("{0}.sp", IO.Path.Combine(sInclude, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sp", IO.Path.Combine(sInclude, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.sma", IO.Path.Combine(sInclude, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sma", IO.Path.Combine(sInclude, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.p", IO.Path.Combine(sInclude, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.p", IO.Path.Combine(sInclude, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.pwn", IO.Path.Combine(sInclude, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.pwn", IO.Path.Combine(sInclude, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sInclude, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sInclude, sMatchValue)))

                                    Case IO.File.Exists(String.Format("{0}.sp", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sp", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.sma", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sma", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.p", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.p", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.pwn", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.pwn", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sCurrentDir, sMatchValue)))

                                    Case IO.File.Exists(String.Format("{0}.sp", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sp", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.sma", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sma", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.p", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.p", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.pwn", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.pwn", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sCompilerPath, sMatchValue)))

                                    Case Else
                                        Continue While
                                End Select

                        End Select

                        If (Not IO.File.Exists(sCorrectPath)) Then
                            Continue While
                        End If

                        If (lPathList.Contains(sCorrectPath.ToLower)) Then
                            Continue While
                        End If

                        lPathList.Add(sCorrectPath.ToLower)
                    End While
                End Using
            Next

            Exit While
        End While

        For i = 0 To lPathList.Count - 1
            GetIncludeFilesRecursive(mConfig, sActiveSource, sActiveSourceFile, lPathList(i), lList)
        Next
    End Sub

    Class ClassParser
        Private Class STRUC_AUTOCOMPLETE_PARSE_PRE_INFO
            Public sSource As String
            Public sSourceCode As String
            Public sActiveSource As String
            Public sActiveSourceFile As String
            Public sFile As String
            Public lSourceList As List(Of KeyValuePair(Of String, String))
            Public lNewAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Public iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE

            Sub New(_Source As String,
                    _SourceCode As String,
                    _ActiveSource As String,
                    _ActiveSourceFile As String,
                    _File As String,
                    ByRef _SourceList As List(Of KeyValuePair(Of String, String)),
                    ByRef _NewAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE),
                    _Language As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)

                sSource = _Source
                sSourceCode = _SourceCode
                sActiveSource = _ActiveSource
                sActiveSourceFile = _ActiveSourceFile
                sFile = _File
                lSourceList = _SourceList
                lNewAutocompleteList = _NewAutocompleteList
                iLanguage = _Language
            End Sub
        End Class

        Private Class STRUC_AUTOCOMPLETE_PARSE_POST_INFO
            Public sActiveSource As String
            Public sActiveSourceFile As String
            Public sFile As String
            Public sSource As String
            Public lNewAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Public iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE

            Sub New(_ActiveSource As String,
                            _ActiveSourceFile As String,
                            ByRef _File As String,
                            ByRef _Source As String,
                            ByRef _NewAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE),
                            _Language As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)

                sActiveSource = _ActiveSource
                sActiveSourceFile = _ActiveSourceFile
                sFile = _File
                sSource = _Source
                lNewAutocompleteList = _NewAutocompleteList
                iLanguage = _Language
            End Sub
        End Class

        Private Class STRUC_VARIABLE_PARSE_PRE_INFO
            Public sSource As String
            Public sActiveSource As String
            Public sActiveSourceFile As String
            Public sFile As String
            Public lNewVarAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Public lOldVarAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Public iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE

            Sub New(_Source As String,
                        _ActiveSource As String,
                        _ActiveSourceFile As String,
                        ByRef _File As String,
                        ByRef _NewVarAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE),
                        ByRef _OldVarAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE),
                        _Language As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)

                sSource = _Source
                sActiveSource = _ActiveSource
                sActiveSourceFile = _ActiveSourceFile
                sFile = _File
                lNewVarAutocompleteList = _NewVarAutocompleteList
                lOldVarAutocompleteList = _OldVarAutocompleteList
                iLanguage = _Language
            End Sub
        End Class

        Private Class STRUC_VARIABLE_PARSE_POST_INFO
            Public sActiveSource As String
            Public sActiveSourceFile As String
            Public lNewVarAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Public lOldVarAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Public iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE

            Sub New(_ActiveSource As String,
                            _ActiveSourceFile As String,
                            ByRef _NewVarAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE),
                            ByRef _OldVarAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE),
                            _Language As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
                sActiveSource = _ActiveSource
                sActiveSourceFile = _ActiveSourceFile
                lNewVarAutocompleteList = _NewVarAutocompleteList
                lOldVarAutocompleteList = _OldVarAutocompleteList
                iLanguage = _Language
            End Sub
        End Class

        Public Sub ProcessFullSyntaxPre(mFormMain As FormMain,
                                                sActiveSource As String,
                                                sActiveSourceFile As String,
                                                sFile As String,
                                                ByRef sSourceList As List(Of KeyValuePair(Of String, String)),
                                                ByRef lNewAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE),
                                                iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
            Dim sSource As String
            If (sActiveSourceFile.ToLower = sFile.ToLower) Then
                sSource = sActiveSource
            Else
                sSource = IO.File.ReadAllText(sFile)
            End If

            CleanUpNewLinesSource(sSource, iLanguage)
            CleanupFunctionSpacesSource(sSource, iLanguage)

            Dim mSourceCode As New StringBuilder(sSource.Length)
            If (True) Then
                Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, iLanguage)

                For i = 0 To sSource.Length - 1
                    mSourceCode.Append(If(mSourceAnalysis.m_InNonCode(i), " "c, sSource(i)))
                Next
            End If

            Dim mParseInfo As New STRUC_AUTOCOMPLETE_PARSE_PRE_INFO(sSource, mSourceCode.ToString, sActiveSource, sActiveSourceFile, sFile, sSourceList, lNewAutocompleteList, iLanguage)
            Dim mAutocompletePre As New ClassFullSyntaxPre(Me)

            mAutocompletePre.ParseStructs(mParseInfo)
            mAutocompletePre.ParseEnumStructs(mParseInfo)
            mAutocompletePre.ParseMethodmapEnums(mParseInfo)
            mAutocompletePre.ParseTypesetEnums(mParseInfo)
            mAutocompletePre.ParseTypedefEnums(mParseInfo)
            mAutocompletePre.ParseFunctagEnums(mParseInfo)
            mAutocompletePre.ParseFuncenumEnums(mParseInfo)
            mAutocompletePre.ParseEnums(mFormMain, mParseInfo)

            sSourceList.Add(New KeyValuePair(Of String, String)(sFile, sSource))
        End Sub

        Public Sub ProcessFullSyntaxPost(mFormMain As FormMain,
                                                    sActiveSource As String,
                                                    sActiveSourceFile As String,
                                                    ByRef sFile As String,
                                                    ByRef sSource As String,
                                                    ByRef lNewAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE),
                                                    iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)

            Dim mParseInfo As New STRUC_AUTOCOMPLETE_PARSE_POST_INFO(sActiveSource, sActiveSourceFile, sFile, sSource, lNewAutocompleteList, iLanguage)
            Dim mAutocompletePost As New ClassFullSyntaxPost(Me)

            mAutocompletePost.ParseDefines(mFormMain, mParseInfo)
            mAutocompletePost.ParsePublicVariables(mFormMain, mParseInfo)
            mAutocompletePost.ParseFuncenums(mFormMain, mParseInfo)
            mAutocompletePost.ParseTypesets(mFormMain, mParseInfo)
            mAutocompletePost.ParseTypedefs(mFormMain, mParseInfo)
            mAutocompletePost.ParseMethodsAndFunctags(mFormMain, mParseInfo)
            mAutocompletePost.ParseMethodmaps(mFormMain, mParseInfo)
            mAutocompletePost.ParseEnumStructs(mFormMain, mParseInfo)
        End Sub

        Public Sub ProcessFullSyntaxFinalize(mFormMain As FormMain,
                                                    lTmpAutoList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE))

            Dim mAutocompleteFinalize As New ClassFullSyntaxFinalize(Me)

            mAutocompleteFinalize.ProcessMethodmapParentMethods(mFormMain, lTmpAutoList)
            mAutocompleteFinalize.ProcessMethodmapParentMethodmaps(mFormMain, lTmpAutoList)

            mAutocompleteFinalize.GenerateMethodmapThis(mFormMain, lTmpAutoList)
            mAutocompleteFinalize.GenerateEnumStructThis(mFormMain, lTmpAutoList)
        End Sub

        Public Sub ProcessVarSyntaxPre(mFormMain As FormMain,
                                                sActiveSource As String,
                                                sActiveSourceFile As String,
                                                ByRef sFile As String,
                                                ByRef lNewVarAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE),
                                                ByRef lOldVarAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE),
                                                iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
            Dim sSource As String
            If (sActiveSourceFile.ToLower = sFile.ToLower) Then
                sSource = sActiveSource
            Else
                sSource = IO.File.ReadAllText(sFile)
            End If

            CleanUpNewLinesSource(sSource, iLanguage)

            Dim mParseInfo As New STRUC_VARIABLE_PARSE_PRE_INFO(sSource, sActiveSource, sActiveSourceFile, sFile, lNewVarAutocompleteList, lOldVarAutocompleteList, iLanguage)
            Dim mVariablePre As New ClassVarSyntaxPre(Me)

            mVariablePre.ParseVariables(mFormMain, mParseInfo)
        End Sub

        Public Sub ProcessVarSyntaxFinalize(mFormMain As FormMain,
                                            sActiveSource As String,
                                            sActiveSourceFile As String,
                                            ByRef lNewVarAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE),
                                            ByRef lOldVarAutocompleteList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE),
                                            iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)

            Dim mParseInfo As New STRUC_VARIABLE_PARSE_POST_INFO(sActiveSource, sActiveSourceFile, lNewVarAutocompleteList, lOldVarAutocompleteList, iLanguage)
            Dim mVariableFinalize As New ClassVarSyntaxFinalize(Me)

            mVariableFinalize.ParseFunctionArguments(mFormMain, mParseInfo)

            mVariableFinalize.GenerateMethodmapVariables(mFormMain, mParseInfo)
            mVariableFinalize.GenerateMethodmapMethods(mFormMain, mParseInfo)
            mVariableFinalize.GenerateMethodmapInlineMethods(mFormMain, mParseInfo)
            mVariableFinalize.GenerateMethodmapFields(mFormMain, mParseInfo)
            mVariableFinalize.GenerateEnumStructVariables(mFormMain, mParseInfo)
            mVariableFinalize.GenerateEnumStructMethods(mFormMain, mParseInfo)
            mVariableFinalize.GenerateEnumStructInlineMethods(mFormMain, mParseInfo)
            mVariableFinalize.GenerateEnumStructFields(mFormMain, mParseInfo)
        End Sub

        Private Class ClassFullSyntaxPre
            Public g_ClassParse As ClassParser

            Sub New(_ClassParse As ClassParser)
                g_ClassParse = _ClassParse
            End Sub

            ''' <summary>
            ''' Parse structs. Only names only currently.
            ''' </summary>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseStructs(mParseInfo As STRUC_AUTOCOMPLETE_PARSE_PRE_INFO)
                If (mParseInfo.sSource.Contains("struct")) Then
                    Dim mPossibleStructMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "^\s*\b(struct)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)", RegexOptions.Multiline)
                    Dim mAnchorStructMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "\b(struct)\b", RegexOptions.Multiline)

                    Dim mMatch As Match
                    Dim iAnchorIndex As Integer

                    For i = 0 To mPossibleStructMatches.Count - 1
                        mMatch = mPossibleStructMatches(i)

                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        iAnchorIndex = 0
                        For j = 0 To mAnchorStructMatches.Count - 1
                            'We dont need to check for non-code. sSourceCode is already filtered.
                            If (mAnchorStructMatches(j).Index > mMatch.Index - 1) Then
                                Exit For
                            End If

                            iAnchorIndex += 1
                        Next

                        Dim sStructName As String = mMatch.Groups("Name").Value

                        Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                    IO.Path.GetFileName(mParseInfo.sFile),
                                                                                    mParseInfo.sFile,
                                                                                    ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STRUCT,
                                                                                    sStructName,
                                                                                    sStructName,
                                                                                    "struct " & sStructName)

                        mAutocomplete.m_Data("StructAnchorName") = "struct"
                        mAutocomplete.m_Data("StructAnchorIndex") = iAnchorIndex
                        mAutocomplete.m_Data("StructAnchorFile") = mParseInfo.sFile

                        mAutocomplete.m_Data("StructName") = sStructName

#If DEBUG Then
                        mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get structs (names only)"
#End If

                        If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                            mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                        End If
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse enum structs as enums.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseEnumStructs(mParseInfo As STRUC_AUTOCOMPLETE_PARSE_PRE_INFO)
                If (mParseInfo.sSource.Contains("enum") AndAlso mParseInfo.sSource.Contains("struct")) Then
                    Dim mPossibleEnumStructMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "^\s*\b(enum)\b\s+\b(struct)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)", RegexOptions.Multiline)
                    Dim mAnchorEnumStructMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "\b(enum)\b", RegexOptions.Multiline)

                    Dim mMatch As Match
                    Dim iAnchorIndex As Integer

                    For i = 0 To mPossibleEnumStructMatches.Count - 1
                        mMatch = mPossibleEnumStructMatches(i)

                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        iAnchorIndex = 0
                        For j = 0 To mAnchorEnumStructMatches.Count - 1
                            'We dont need to check for non-code. sSourceCode is already filtered.
                            If (mAnchorEnumStructMatches(j).Index > mMatch.Index - 1) Then
                                Exit For
                            End If

                            iAnchorIndex += 1
                        Next

                        Dim sEnumStructName As String = mMatch.Groups("Name").Value

                        Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                    IO.Path.GetFileName(mParseInfo.sFile),
                                                                                    mParseInfo.sFile,
                                                                                    ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM,
                                                                                    sEnumStructName,
                                                                                    sEnumStructName,
                                                                                    "enum " & sEnumStructName)

                        mAutocomplete.m_Data("EnumAnchorName") = "enum"
                        mAutocomplete.m_Data("EnumAnchorIndex") = iAnchorIndex
                        mAutocomplete.m_Data("EnumAnchorFile") = mParseInfo.sFile

                        mAutocomplete.m_Data("EnumName") = sEnumStructName
                        mAutocomplete.m_Data("EnumIsEnumStruct") = True
                        mAutocomplete.m_Data("EnumHidden") = True

#If DEBUG Then
                        mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get enum structs (names only)"
#End If

                        If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                            mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                        End If
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse methodmaps names as enums.
            ''' SpurcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseMethodmapEnums(mParseInfo As STRUC_AUTOCOMPLETE_PARSE_PRE_INFO)
                If ((ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX) AndAlso
                            mParseInfo.sSource.Contains("methodmap")) Then
                    Dim mPossibleEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "^\s*\b(methodmap)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)", RegexOptions.Multiline)
                    Dim mAnchorEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "\b(methodmap)\b", RegexOptions.Multiline)

                    Dim mMatch As Match
                    Dim iAnchorIndex As Integer

                    For i = 0 To mPossibleEnumMatches.Count - 1
                        mMatch = mPossibleEnumMatches(i)

                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        iAnchorIndex = 0
                        For j = 0 To mAnchorEnumMatches.Count - 1
                            'We dont need to check for non-code. sSourceCode is already filtered.
                            If (mAnchorEnumMatches(j).Index > mMatch.Index - 1) Then
                                Exit For
                            End If

                            iAnchorIndex += 1
                        Next

                        Dim sEnumName As String = mMatch.Groups("Name").Value

                        Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                    IO.Path.GetFileName(mParseInfo.sFile),
                                                                                    mParseInfo.sFile,
                                                                                    ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM,
                                                                                    sEnumName,
                                                                                    sEnumName,
                                                                                    "enum " & sEnumName)

                        mAutocomplete.m_Data("EnumAnchorName") = "methodmap"
                        mAutocomplete.m_Data("EnumAnchorIndex") = iAnchorIndex
                        mAutocomplete.m_Data("EnumAnchorFile") = mParseInfo.sFile

                        mAutocomplete.m_Data("EnumName") = sEnumName
                        mAutocomplete.m_Data("EnumIsMethodmap") = True
                        mAutocomplete.m_Data("EnumHidden") = True

#If DEBUG Then
                        mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get methodmap enums"
#End If

                        If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                            mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                        End If
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse typeset names as enums.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseTypesetEnums(mParseInfo As STRUC_AUTOCOMPLETE_PARSE_PRE_INFO)
                If ((ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX) AndAlso
                            mParseInfo.sSource.Contains("typeset")) Then
                    Dim mPossibleEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "^\s*\b(typeset)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)", RegexOptions.Multiline)
                    Dim mAnchorEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "\b(typeset)\b", RegexOptions.Multiline)

                    Dim mMatch As Match
                    Dim iAnchorIndex As Integer

                    For i = 0 To mPossibleEnumMatches.Count - 1
                        mMatch = mPossibleEnumMatches(i)

                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        iAnchorIndex = 0
                        For j = 0 To mAnchorEnumMatches.Count - 1
                            'We dont need to check for non-code. sSourceCode is already filtered.
                            If (mAnchorEnumMatches(j).Index > mMatch.Index - 1) Then
                                Exit For
                            End If

                            iAnchorIndex += 1
                        Next

                        Dim sEnumName As String = mMatch.Groups("Name").Value

                        Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                    IO.Path.GetFileName(mParseInfo.sFile),
                                                                                    mParseInfo.sFile,
                                                                                    ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM,
                                                                                    sEnumName,
                                                                                    sEnumName,
                                                                                    "enum " & sEnumName)

                        mAutocomplete.m_Data("EnumAnchorName") = "typeset"
                        mAutocomplete.m_Data("EnumAnchorIndex") = iAnchorIndex
                        mAutocomplete.m_Data("EnumAnchorFile") = mParseInfo.sFile

                        mAutocomplete.m_Data("EnumName") = sEnumName
                        mAutocomplete.m_Data("EnumIsTypeset") = True
                        mAutocomplete.m_Data("EnumHidden") = True

#If DEBUG Then
                        mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get typeset enums"
#End If

                        If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                            mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                        End If
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse typedef names as enums.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseTypedefEnums(mParseInfo As STRUC_AUTOCOMPLETE_PARSE_PRE_INFO)
                If ((ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX) AndAlso
                            mParseInfo.sSource.Contains("typedef")) Then
                    Dim mPossibleEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "^\s*\b(typedef)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)", RegexOptions.Multiline)
                    Dim mAnchorEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "\b(typedef)\b", RegexOptions.Multiline)

                    Dim mMatch As Match
                    Dim iAnchorIndex As Integer

                    For i = 0 To mPossibleEnumMatches.Count - 1
                        mMatch = mPossibleEnumMatches(i)

                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        iAnchorIndex = 0
                        For j = 0 To mAnchorEnumMatches.Count - 1
                            'We dont need to check for non-code. sSourceCode is already filtered.
                            If (mAnchorEnumMatches(j).Index > mMatch.Index - 1) Then
                                Exit For
                            End If

                            iAnchorIndex += 1
                        Next

                        Dim sEnumName As String = mMatch.Groups("Name").Value

                        Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                    IO.Path.GetFileName(mParseInfo.sFile),
                                                                                    mParseInfo.sFile,
                                                                                    ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM,
                                                                                    sEnumName,
                                                                                    sEnumName,
                                                                                    "enum " & sEnumName)

                        mAutocomplete.m_Data("EnumAnchorName") = "typedef"
                        mAutocomplete.m_Data("EnumAnchorIndex") = iAnchorIndex
                        mAutocomplete.m_Data("EnumAnchorFile") = mParseInfo.sFile

                        mAutocomplete.m_Data("EnumName") = sEnumName
                        mAutocomplete.m_Data("EnumIsTypedef") = True
                        mAutocomplete.m_Data("EnumHidden") = True

#If DEBUG Then
                        mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get typedef enums"
#End If

                        If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                            mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                        End If
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse functag names as enums.
            ''' SourcePawn -1.6 only.
            ''' </summary>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseFunctagEnums(mParseInfo As STRUC_AUTOCOMPLETE_PARSE_PRE_INFO)
                If ((ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_6 OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX) AndAlso
                            mParseInfo.sSource.Contains("functag")) Then
                    Dim mPossibleEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "^\s*\b(functag)\b\s+\b(public)\b\s+(?<Tag>\b[a-zA-Z0-9_]+\b\s+|\b[a-zA-Z0-9_]+\b\:\s*|)(?<Name>\b[a-zA-Z0-9_]+\b)\s*\(", RegexOptions.Multiline)
                    Dim mAnchorEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "\b(functag)\b", RegexOptions.Multiline)

                    Dim mMatch As Match
                    Dim iAnchorIndex As Integer

                    For i = 0 To mPossibleEnumMatches.Count - 1
                        mMatch = mPossibleEnumMatches(i)

                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        iAnchorIndex = 0
                        For j = 0 To mAnchorEnumMatches.Count - 1
                            'We dont need to check for non-code. sSourceCode is already filtered.
                            If (mAnchorEnumMatches(j).Index > mMatch.Index - 1) Then
                                Exit For
                            End If

                            iAnchorIndex += 1
                        Next

                        Dim sEnumName As String = mMatch.Groups("Name").Value

                        Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                    IO.Path.GetFileName(mParseInfo.sFile),
                                                                                    mParseInfo.sFile,
                                                                                    ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM,
                                                                                    sEnumName,
                                                                                    sEnumName,
                                                                                    "enum " & sEnumName)

                        mAutocomplete.m_Data("EnumAnchorName") = "functag"
                        mAutocomplete.m_Data("EnumAnchorIndex") = iAnchorIndex
                        mAutocomplete.m_Data("EnumAnchorFile") = mParseInfo.sFile

                        mAutocomplete.m_Data("EnumName") = sEnumName
                        mAutocomplete.m_Data("EnumIsFunctag") = True
                        mAutocomplete.m_Data("EnumHidden") = True

#If DEBUG Then
                        mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get functag enums"
#End If

                        If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                            mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                        End If
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse funcenum names as enums.
            ''' SourcePawn -1.6 only.
            ''' </summary>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseFuncenumEnums(mParseInfo As STRUC_AUTOCOMPLETE_PARSE_PRE_INFO)
                If ((ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_6 OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX) AndAlso
                            mParseInfo.sSource.Contains("funcenum")) Then
                    Dim mPossibleEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "^\s*\b(funcenum)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)", RegexOptions.Multiline)
                    Dim mAnchorEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "\b(funcenum)\b", RegexOptions.Multiline)

                    Dim mMatch As Match
                    Dim iAnchorIndex As Integer

                    For i = 0 To mPossibleEnumMatches.Count - 1
                        mMatch = mPossibleEnumMatches(i)

                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        iAnchorIndex = 0
                        For j = 0 To mAnchorEnumMatches.Count - 1
                            'We dont need to check for non-code. sSourceCode is already filtered.
                            If (mAnchorEnumMatches(j).Index > mMatch.Index - 1) Then
                                Exit For
                            End If

                            iAnchorIndex += 1
                        Next

                        Dim sEnumName As String = mMatch.Groups("Name").Value

                        Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                    IO.Path.GetFileName(mParseInfo.sFile),
                                                                                    mParseInfo.sFile,
                                                                                    ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM,
                                                                                    sEnumName,
                                                                                    sEnumName,
                                                                                    "enum " & sEnumName)

                        mAutocomplete.m_Data("EnumAnchorName") = "funcenum"
                        mAutocomplete.m_Data("EnumAnchorIndex") = iAnchorIndex
                        mAutocomplete.m_Data("EnumAnchorFile") = mParseInfo.sFile

                        mAutocomplete.m_Data("EnumName") = sEnumName
                        mAutocomplete.m_Data("EnumIsFuncenum") = True
                        mAutocomplete.m_Data("EnumHidden") = True

#If DEBUG Then
                        mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get funcenum enums"
#End If

                        If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                            mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                        End If
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse enums and its entries.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseEnums(mFormMain As FormMain, mParseInfo As STRUC_AUTOCOMPLETE_PARSE_PRE_INFO)
                If (mParseInfo.sSource.Contains("enum")) Then

                    Dim mPossibleEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "^\s*\b(enum)\b\s*((?<Tag>\b[a-zA-Z0-9_]+\b)(\:)(?<Name>\b[a-zA-Z0-9_]+\b)\s*(\(.*?\)){0,1}|(?<Name>\b[a-zA-Z0-9_]+\b)(\:){0,1}\s*(\(.*?\)){0,1}|\(.*?\)|)\s*(?<BraceStart>\{)", RegexOptions.Multiline)
                    Dim mAnchorEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSourceCode, "\b(enum)\b", RegexOptions.Multiline)
                    Dim iBraceList As Integer()() = ClassSyntaxTools.ClassSyntaxHelpers.GetExpressionBetweenCharacters(mParseInfo.sSourceCode, "{"c, "}"c, 1, mParseInfo.iLanguage, True)


                    Dim mMatch As Match
                    Dim mMatch2 As Match

                    Dim sEnumName As String

                    Dim bIsValid As Boolean
                    Dim sEnumSource As String
                    Dim iBraceIndex As Integer
                    Dim iAnchorIndex As Integer

                    Dim mEnumSourceAnalysis As ClassSyntaxTools.ClassSyntaxSourceAnalysis

                    Dim mEnumBuilder As StringBuilder
                    Dim lEnumSplitList As List(Of String)

                    Dim sLine As String

                    Dim iTargetEnumSplitListIndex As Integer


                    For i = 0 To mPossibleEnumMatches.Count - 1
                        mMatch = mPossibleEnumMatches(i)
                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        sEnumName = mMatch.Groups("Name").Value
                        If (String.IsNullOrEmpty(sEnumName.Trim)) Then
                            sEnumName = "Enum"
                        End If

                        bIsValid = False
                        sEnumSource = ""
                        iBraceIndex = mMatch.Groups("BraceStart").Index
                        For ii = 0 To iBraceList.Length - 1
                            If (iBraceIndex = iBraceList(ii)(0)) Then
                                sEnumSource = mParseInfo.sSourceCode.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                                bIsValid = True
                                Exit For
                            End If
                        Next
                        If (Not bIsValid) Then
                            Continue For
                        End If

                        iAnchorIndex = 0
                        For j = 0 To mAnchorEnumMatches.Count - 1
                            'We dont need to check for non-code. sSourceCode is already filtered.
                            If (mAnchorEnumMatches(j).Index > mMatch.Index - 1) Then
                                Exit For
                            End If

                            iAnchorIndex += 1
                        Next

                        mEnumSourceAnalysis = New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sEnumSource, mParseInfo.iLanguage)

                        mEnumBuilder = New StringBuilder
                        lEnumSplitList = New List(Of String)

                        For ii = 0 To sEnumSource.Length - 1
                            Select Case (sEnumSource(ii))
                                Case ","c
                                    If (mEnumSourceAnalysis.GetParenthesisLevel(ii, Nothing) > 0 OrElse mEnumSourceAnalysis.GetBracketLevel(ii, Nothing) > 0 OrElse mEnumSourceAnalysis.GetBraceLevel(ii, Nothing) > 0 OrElse mEnumSourceAnalysis.m_InNonCode(ii)) Then
                                        Exit Select
                                    End If

                                    If (mEnumBuilder.Length < 1) Then
                                        Exit Select
                                    End If

                                    sLine = mEnumBuilder.ToString
                                    sLine = ClassRegexLock.Replace(sLine.Trim, "\s+", " ")

                                    lEnumSplitList.Add(sLine)
                                    mEnumBuilder = New StringBuilder
                                Case Else
                                    If (mEnumSourceAnalysis.m_InNonCode(ii)) Then
                                        Exit Select
                                    End If

                                    mEnumBuilder.Append(sEnumSource(ii))
                            End Select
                        Next

                        If (mEnumBuilder.Length > 0) Then
                            sLine = mEnumBuilder.ToString
                            sLine = ClassRegexLock.Replace(sLine.Trim, "\s+", " ")

                            If (Not String.IsNullOrEmpty(sLine)) Then
                                lEnumSplitList.Add(sLine)
                                mEnumBuilder = New StringBuilder
                            End If
                        End If


                        Dim lEnumCommentArray As String() = New String(lEnumSplitList.Count - 1) {}
                        For j = 0 To lEnumCommentArray.Length - 1
                            lEnumCommentArray(j) = ""
                        Next

                        Dim sEnumSourceLines As String() = sEnumSource.Split(New String() {vbNewLine, vbLf}, 0)
                        For ii = 0 To sEnumSourceLines.Length - 1
                            iTargetEnumSplitListIndex = -1
                            For iii = 0 To lEnumSplitList.Count - 1
                                If (sEnumSourceLines(ii).Contains(lEnumSplitList(iii)) AndAlso ClassRegexLock.IsMatch(sEnumSourceLines(ii), String.Format("\b{0}\b", Regex.Escape(lEnumSplitList(iii))))) Then
                                    iTargetEnumSplitListIndex = iii
                                    Exit For
                                End If
                            Next
                            If (iTargetEnumSplitListIndex < 0) Then
                                Continue For
                            End If

                            While True
                                mMatch2 = ClassRegexLock.Match(sEnumSourceLines(ii), "/\*(.*?)\*/\s*$")
                                If (mMatch2.Success) Then
                                    lEnumCommentArray(iTargetEnumSplitListIndex) = mMatch2.Value
                                    Exit While
                                End If
                                If (ii > 1) Then
                                    mMatch2 = ClassRegexLock.Match(sEnumSourceLines(ii - 1), "^\s*(?<Comment>//(.*?)$)")
                                    If (mMatch2.Success) Then
                                        lEnumCommentArray(iTargetEnumSplitListIndex) = mMatch2.Groups("Comment").Value
                                        Exit While
                                    End If
                                End If

                                lEnumCommentArray(iTargetEnumSplitListIndex) = ""
                                Exit While
                            End While
                        Next

                        If (True) Then
                            Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                        IO.Path.GetFileName(mParseInfo.sFile),
                                                                                        mParseInfo.sFile,
                                                                                        ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM,
                                                                                        sEnumName,
                                                                                        sEnumName,
                                                                                        "enum " & sEnumName)

                            mAutocomplete.m_Data("EnumAnchorName") = "enum"
                            mAutocomplete.m_Data("EnumAnchorIndex") = iAnchorIndex
                            mAutocomplete.m_Data("EnumAnchorFile") = mParseInfo.sFile

                            mAutocomplete.m_Data("EnumName") = sEnumName

#If DEBUG Then
                            mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get enums"
#End If

                            If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                            End If
                        End If

                        For ii = 0 To lEnumSplitList.Count - 1
                            Dim sEnumFull As String = lEnumSplitList(ii)
                            Dim sEnumComment As String = lEnumCommentArray(ii)

                            mMatch2 = ClassRegexLock.Match(sEnumFull, "^\s*(?<Tag>\b[a-zA-Z0-9_]+\b:)*(?<Name>\b[a-zA-Z0-9_]+\b)")
                            If (Not mMatch2.Groups("Name").Success) Then
                                Continue For
                            End If

                            Dim sEnumVarName As String = mMatch2.Groups("Name").Value

                            Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(sEnumComment,
                                                                                        IO.Path.GetFileName(mParseInfo.sFile),
                                                                                        mParseInfo.sFile,
                                                                                        ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM,
                                                                                        sEnumVarName,
                                                                                        String.Format("{0}.{1}", sEnumName, sEnumVarName),
                                                                                        String.Format("enum {0} {1}", sEnumName, sEnumFull))

                            mAutocomplete.m_Data("EnumAnchorName") = "enum"
                            mAutocomplete.m_Data("EnumAnchorIndex") = iAnchorIndex
                            mAutocomplete.m_Data("EnumAnchorFile") = mParseInfo.sFile

                            mAutocomplete.m_Data("EnumName") = sEnumName
                            mAutocomplete.m_Data("EnumIsChild") = True
                            mAutocomplete.m_Data("EnumChildName") = sEnumVarName

#If DEBUG Then
                            mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get enums"
#End If

                            If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                            End If
                        Next
                    Next
                End If
            End Sub
        End Class

        Private Class ClassFullSyntaxPost
            Public g_ClassParse As ClassParser

            Sub New(_ClassParse As ClassParser)
                g_ClassParse = _ClassParse
            End Sub

            ''' <summary>
            ''' Parse defines.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseDefines(mFormMain As FormMain, mParseInfo As STRUC_AUTOCOMPLETE_PARSE_POST_INFO)
                If (mParseInfo.sSource.Contains("#define")) Then
                    Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(mParseInfo.sSource, mParseInfo.iLanguage)

                    Dim mMatch As Match

                    Dim sFullDefine As String
                    Dim sFullName As String
                    Dim sName As String
                    Dim iAnchorIndex As Integer
                    Dim sAnchorName As String

                    Dim sLines As String() = mParseInfo.sSource.Split(New String() {vbNewLine, vbLf}, 0)
                    For i = 0 To sLines.Length - 1
                        If (Not sLines(i).Contains("#define")) Then
                            Continue For
                        End If

                        mMatch = ClassRegexLock.Match(sLines(i), "(?<FullDefine>^\s*#define\s+(?<Name>\b[a-zA-Z0-9_]+\b))")
                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        Dim iIndex = mSourceAnalysis.GetIndexFromLine(i)
                        If (mSourceAnalysis.m_InNonCode(mMatch.Index + iIndex)) Then
                            Continue For
                        End If

                        sFullName = ""
                        sName = mMatch.Groups("Name").Value
                        sFullDefine = mMatch.Groups("FullDefine").Value

                        sFullDefine = ClassRegexLock.Match(sLines(i), String.Format("{0}(.*?)$", Regex.Escape(sFullDefine))).Value
                        sFullName = sFullDefine

                        sFullName = sFullName.Replace(vbTab, " ")

                        sAnchorName = "define"
                        iAnchorIndex = 0
                        For ii = 0 To i - 1
                            If (Not sLines(ii).Contains(sAnchorName)) Then
                                Continue For
                            End If

                            For Each mAnchorMatch As Match In ClassRegexLock.Matches(sLines(ii), String.Format("\b({0})\b", sAnchorName))
                                Dim iAnchorOffset = mSourceAnalysis.GetIndexFromLine(ii) + mAnchorMatch.Index

                                If (mSourceAnalysis.m_InNonCode(iAnchorOffset)) Then
                                    Continue For
                                End If

                                iAnchorIndex += 1
                            Next
                        Next

                        Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                    IO.Path.GetFileName(mParseInfo.sFile),
                                                                                    mParseInfo.sFile,
                                                                                    ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE,
                                                                                    sName,
                                                                                    sName,
                                                                                    sFullName)

                        mAutocomplete.m_Data("DefineAnchorName") = sAnchorName
                        mAutocomplete.m_Data("DefineAnchorIndex") = iAnchorIndex
                        mAutocomplete.m_Data("DefineAnchorFile") = mParseInfo.sFile

                        mAutocomplete.m_Data("DefineName") = sName

#If DEBUG Then
                        mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get Defines"
#End If

                        If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                            mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                        End If
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse public variables.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub ParsePublicVariables(mFormMain As FormMain, mParseInfo As STRUC_AUTOCOMPLETE_PARSE_POST_INFO)
                If (mParseInfo.sSource.Contains("public")) Then
                    Dim sRegExTypePattern As String = g_ClassParse.GetTypeNamesToPattern(mParseInfo.lNewAutocompleteList)
                    Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(mParseInfo.sSource, mParseInfo.iLanguage)

                    Dim mMatch As Match

                    Dim iBracketList As Integer()()
                    Dim sFullName As String
                    Dim sName As String
                    Dim sComment As String
                    Dim sTypes As String
                    Dim sTag As String
                    Dim sOther As String
                    Dim IsFunction As Boolean
                    Dim IsInvalidName As Boolean
                    Dim IsArray As Boolean
                    Dim iAnchorIndex As Integer
                    Dim sAnchorName As String

                    Dim sLines As String() = mParseInfo.sSource.Split(New String() {vbNewLine, vbLf}, 0)
                    For i = 0 To sLines.Length - 1
                        If (Not sLines(i).Contains("public")) Then
                            Continue For
                        End If

                        'SP 1.7 +Tags
                        mMatch = ClassRegexLock.Match(sLines(i), String.Format("^\s*(?<Types>public(\b[a-zA-Z0-9_ ]+\b)*)\s+(?<Tag>\b{0}\b(\[\s*\])*\s)\s*(?<Name>\b[a-zA-Z0-9_]+\b)(?<Other>(.*?))$", sRegExTypePattern))
                        If (Not mMatch.Success) Then
                            If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                                Continue For
                            End If

                            'SP 1.6 +Tags
                            mMatch = ClassRegexLock.Match(sLines(i), String.Format("^\s*(?<Types>public(\b[a-zA-Z0-9_ ]+\b)*)\s+(?<Tag>\b{0}\b\:)(?<Name>\b[a-zA-Z0-9_]+\b)(?<Other>(.*?))$", sRegExTypePattern, "{0}"))
                            If (Not mMatch.Success) Then
                                'SP 1.6 -Tags
                                mMatch = ClassRegexLock.Match(sLines(i), String.Format("^\s*(?<Types>public(\b[a-zA-Z0-9_ ]+\b)*)\s+(?<!<Tag>\b{0}\b\:)(?<Name>\b[a-zA-Z0-9_]+\b)(?<Other>(.*?))$", sRegExTypePattern, "{0}"))
                                If (Not mMatch.Success) Then
                                    Continue For
                                End If
                            End If
                        Else
                            If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_6) Then
                                Continue For
                            End If
                        End If

                        Dim iIndex = mSourceAnalysis.GetIndexFromLine(i)
                        If (iIndex < 0 OrElse mSourceAnalysis.GetBraceLevel(mMatch.Index + iIndex, Nothing) > 0 OrElse mSourceAnalysis.m_InNonCode(mMatch.Index + iIndex)) Then
                            Continue For
                        End If

                        sFullName = ""
                        sName = mMatch.Groups("Name").Value
                        sComment = ClassRegexLock.Match(mMatch.Groups("Other").Value, "/\*(.*?)\*/\s*$").Value
                        sTypes = mMatch.Groups("Types").Value
                        sTag = mMatch.Groups("Tag").Value
                        sOther = mMatch.Groups("Other").Value.Trim
                        IsFunction = sOther.StartsWith("(")
                        IsArray = sOther.StartsWith("[")
                        IsInvalidName = sOther.StartsWith(":")

                        If (IsFunction OrElse IsInvalidName) Then
                            Continue For
                        End If

                        Dim sArrayDim As String = ""
                        If (IsArray) Then
                            iBracketList = ClassSyntaxTools.ClassSyntaxHelpers.GetExpressionBetweenCharacters(sOther, "["c, "]"c, 1, mParseInfo.iLanguage, True)
                            For j = 0 To iBracketList.Length - 1
                                sArrayDim &= sOther.Substring(iBracketList(j)(0), iBracketList(j)(1) - iBracketList(j)(0) + 1)
                            Next
                        End If

                        sFullName = String.Format("{0} {1}{2}{3}", sTypes, sTag, sName, sArrayDim)
                        sFullName = sFullName.Replace(vbTab, " "c)

                        sAnchorName = "public"
                        iAnchorIndex = 0
                        For ii = 0 To i - 1
                            If (Not sLines(ii).Contains(sAnchorName)) Then
                                Continue For
                            End If

                            For Each mAnchorMatch As Match In ClassRegexLock.Matches(sLines(ii), String.Format("\b({0})\b", sAnchorName))
                                Dim iAnchorOffset = mSourceAnalysis.GetIndexFromLine(ii) + mAnchorMatch.Index

                                If (mSourceAnalysis.m_InNonCode(iAnchorOffset)) Then
                                    Continue For
                                End If

                                iAnchorIndex += 1
                            Next
                        Next

                        Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(sComment,
                                                                                        IO.Path.GetFileName(mParseInfo.sFile),
                                                                                        mParseInfo.sFile,
                                                                                        ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR,
                                                                                        sName,
                                                                                        sName,
                                                                                        sFullName)

                        mAutocomplete.m_Data("PublicAnchorName") = sAnchorName
                        mAutocomplete.m_Data("PublicAnchorIndex") = iAnchorIndex
                        mAutocomplete.m_Data("PublicAnchorFile") = mParseInfo.sFile

                        mAutocomplete.m_Data("PublicvarName") = sName
                        mAutocomplete.m_Data("PublicvarTypes") = sTypes
                        mAutocomplete.m_Data("PublicvarTag") = sTag
                        mAutocomplete.m_Data("PublicvarArrayDim") = sArrayDim

#If DEBUG Then
                        mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get public global variables"
#End If

                        If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                            mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                        End If
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse funcenums and its entries.
            ''' SourcePawn -1.6 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseFuncenums(mFormMain As FormMain, mParseInfo As STRUC_AUTOCOMPLETE_PARSE_POST_INFO)
                If ((ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_6 OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX) AndAlso
                            mParseInfo.sSource.Contains("funcenum")) Then
                    Dim mPossibleEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, "^\s*\b(funcenum)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)", RegexOptions.Multiline)
                    Dim mAnchorEnumMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, "\b(funcenum)\b", RegexOptions.Multiline)
                    Dim iBraceList As Integer()() = ClassSyntaxTools.ClassSyntaxHelpers.GetExpressionBetweenCharacters(mParseInfo.sSource, "{"c, "}"c, 1, mParseInfo.iLanguage, True)
                    Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(mParseInfo.sSource, mParseInfo.iLanguage)

                    Dim mMatch As Match

                    Dim sEnumName As String

                    Dim bIsValid As Boolean
                    Dim sEnumSource As String
                    Dim iBraceIndex As Integer
                    Dim iAnchorIndex As Integer

                    Dim mEnumBuilder As StringBuilder
                    Dim lEnumSplitList As List(Of String)

                    Dim mEnumSourceAnalysis As ClassSyntaxTools.ClassSyntaxSourceAnalysis

                    Dim sLine As String
                    Dim iInvalidLen As Integer

                    Dim sComment As String
                    Dim lEnumCommentArray As String()
                    Dim sEnumSourceLines As String()

                    Dim iLine As Integer
                    Dim bCommentStart As Boolean
                    Dim mRegMatch2 As Match

                    For i = 0 To mPossibleEnumMatches.Count - 1
                        mMatch = mPossibleEnumMatches(i)
                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        sEnumName = mMatch.Groups("Name").Value

                        bIsValid = False
                        sEnumSource = ""
                        iBraceIndex = mMatch.Groups("BraceStart").Index
                        For ii = 0 To iBraceList.Length - 1
                            If (iBraceIndex = iBraceList(ii)(0)) Then
                                sEnumSource = mParseInfo.sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                                bIsValid = True
                                Exit For
                            End If
                        Next
                        If (Not bIsValid) Then
                            Continue For
                        End If

                        iAnchorIndex = 0
                        For j = 0 To mAnchorEnumMatches.Count - 1
                            If (mSourceAnalysis.m_InNonCode(mAnchorEnumMatches(j).Index)) Then
                                Continue For
                            End If

                            If (mAnchorEnumMatches(j).Index > mMatch.Index - 1) Then
                                Exit For
                            End If

                            iAnchorIndex += 1
                        Next

                        mEnumBuilder = New StringBuilder
                        lEnumSplitList = New List(Of String)

                        mEnumSourceAnalysis = New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sEnumSource, mParseInfo.iLanguage)

                        For ii = 0 To sEnumSource.Length - 1
                            Select Case (sEnumSource(ii))
                                Case ","c
                                    If (mEnumSourceAnalysis.GetParenthesisLevel(ii, Nothing) > 0 OrElse mEnumSourceAnalysis.GetBracketLevel(ii, Nothing) > 0 OrElse mEnumSourceAnalysis.GetBraceLevel(ii, Nothing) > 0 OrElse mEnumSourceAnalysis.m_InNonCode(ii)) Then
                                        Exit Select
                                    End If

                                    If (mEnumBuilder.Length < 1) Then
                                        Exit Select
                                    End If

                                    sLine = mEnumBuilder.ToString
                                    sLine = ClassRegexLock.Replace(sLine.Trim, "\s+", " ")

                                    iInvalidLen = ClassRegexLock.Match(sLine, "^(.*?)(?<End>\b[a-zA-Z0-9_]+\b)").Groups("End").Index
                                    If (iInvalidLen > 0) Then
                                        sLine = sLine.Remove(0, iInvalidLen)
                                    End If

                                    lEnumSplitList.Add(sLine)
                                    mEnumBuilder = New StringBuilder
                            End Select

                            If (Not mEnumSourceAnalysis.m_InSingleComment(ii) AndAlso Not mEnumSourceAnalysis.m_InMultiComment(ii)) Then
                                mEnumBuilder.Append(sEnumSource(ii))
                            End If
                        Next

                        If (mEnumBuilder.Length > 0) Then
                            sLine = mEnumBuilder.ToString
                            sLine = ClassRegexLock.Replace(sLine.Trim, "\s+", " ")

                            iInvalidLen = ClassRegexLock.Match(sLine, "^(.*?)(?<End>\b[a-zA-Z0-9_]+\b)").Groups("End").Index
                            If (iInvalidLen > 0) Then
                                sLine = sLine.Remove(0, iInvalidLen)

                                If (Not String.IsNullOrEmpty(sLine)) Then
                                    lEnumSplitList.Add(sLine)
                                    mEnumBuilder = New StringBuilder
                                End If
                            End If
                        End If

                        sComment = ""
                        lEnumCommentArray = New String(lEnumSplitList.Count - 1) {}
                        For j = 0 To lEnumCommentArray.Length - 1
                            lEnumCommentArray(j) = ""
                        Next

                        sEnumSourceLines = sEnumSource.Split(New String() {vbNewLine, vbLf}, 0)

                        For ii = 0 To lEnumSplitList.Count - 1
                            iLine = 0
                            For iii = 0 To sEnumSourceLines.Length - 1
                                If (sEnumSourceLines(iii).Contains(lEnumSplitList(ii))) Then
                                    iLine = iii
                                    Exit For
                                End If
                            Next

                            bCommentStart = False
                            sComment = ""
                            For iii = iLine - 1 To 0 Step -1
                                mRegMatch2 = ClassRegexLock.Match(sEnumSourceLines(iii), "^\s*(?<Start>//(.*?))$")
                                If (Not bCommentStart AndAlso mRegMatch2.Success) Then
                                    sComment = mRegMatch2.Groups("Start").Value & Environment.NewLine & sComment
                                    Continue For
                                End If

                                mRegMatch2 = ClassRegexLock.Match(sEnumSourceLines(iii), "(?<Start>(/\*+))|(?<End>\*+/|(?<Pragma>)^\s*#pragma)")

                                If (Not bCommentStart AndAlso Not mRegMatch2.Groups("End").Success) Then
                                    Exit For
                                End If

                                If (Not mRegMatch2.Groups("Pragma").Success) Then
                                    bCommentStart = True
                                End If

                                Select Case (True)
                                    Case mRegMatch2.Groups("End").Success AndAlso Not mRegMatch2.Groups("Pragma").Success
                                        sComment = sEnumSourceLines(iii).Substring(0, mRegMatch2.Groups("End").Index + 2) & Environment.NewLine & sComment
                                    Case mRegMatch2.Groups("Start").Success
                                        sComment = sEnumSourceLines(iii).Substring(mRegMatch2.Groups("Start").Index, sEnumSourceLines(iii).Length - mRegMatch2.Groups("Start").Index) & Environment.NewLine & sComment
                                    Case Else
                                        sComment = sEnumSourceLines(iii) & Environment.NewLine & sComment
                                End Select

                                If (bCommentStart AndAlso mRegMatch2.Groups("Start").Success) Then
                                    Exit For
                                End If
                            Next

                            lEnumCommentArray(ii) = sComment
                        Next

                        For ii = 0 To lEnumSplitList.Count - 1
                            Dim sEnumFull As String = lEnumSplitList(ii)
                            Dim sEnumComment As String = lEnumCommentArray(ii)

                            sEnumComment = ClassRegexLock.Replace(sEnumComment, "^\s+", "", RegexOptions.Multiline)

                            Dim regMatch As Match = ClassRegexLock.Match(sEnumFull, "^\s*(?<Tag>\b[a-zA-Z0-9_]+\b:)*(?<Name>\b[a-zA-Z0-9_]+\b)")
                            If (Not regMatch.Groups("Name").Success) Then
                                Continue For
                            End If

                            Dim sEnumVarName As String = regMatch.Groups("Name").Value

                            Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(sEnumComment,
                                                                                        IO.Path.GetFileName(mParseInfo.sFile),
                                                                                        mParseInfo.sFile,
                                                                                        ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM,
                                                                                        sEnumName,
                                                                                        "public " & (ClassRegexLock.Replace(sEnumFull, "\s*\b(public)\b\s*\(", sEnumName & "(")),
                                                                                        String.Format("funcenum {0} {1}", sEnumName, sEnumFull))

                            mAutocomplete.m_Data("FuncenumAnchorName") = "funcenum"
                            mAutocomplete.m_Data("FuncenumAnchorIndex") = iAnchorIndex
                            mAutocomplete.m_Data("FuncenumAnchorFile") = mParseInfo.sFile

                            mAutocomplete.m_Data("FuncenumName") = sEnumName
                            mAutocomplete.m_Data("FuncenumVarName") = sEnumVarName
                            mAutocomplete.m_Data("FuncenumFull") = sEnumFull

#If DEBUG Then
                            mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get funcenums"
#End If

                            If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                            End If
                        Next
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse typesets and its entries.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseTypesets(mFormMain As FormMain, mParseInfo As STRUC_AUTOCOMPLETE_PARSE_POST_INFO)
                If ((ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX) AndAlso
                            mParseInfo.sSource.Contains("typeset")) Then
                    Dim sRegExTypePattern As String = g_ClassParse.GetTypeNamesToPattern(mParseInfo.lNewAutocompleteList)

                    Dim mPossibleTypesetMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, "^\s*\b(typeset)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)", RegexOptions.Multiline)
                    Dim mAnchorTypesetMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, "\b(typeset)\b", RegexOptions.Multiline)
                    Dim iBraceList As Integer()() = ClassSyntaxTools.ClassSyntaxHelpers.GetExpressionBetweenCharacters(mParseInfo.sSource, "{"c, "}"c, 1, mParseInfo.iLanguage, True)
                    Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(mParseInfo.sSource, mParseInfo.iLanguage)

                    Dim mMatch As Match

                    Dim bIsValid As Boolean
                    Dim sTypesetSource As String
                    Dim iBraceIndex As Integer
                    Dim iAnchorIndex As Integer

                    Dim sName As String

                    For i = 0 To mPossibleTypesetMatches.Count - 1
                        mMatch = mPossibleTypesetMatches(i)
                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        bIsValid = False
                        sTypesetSource = ""
                        iBraceIndex = mMatch.Groups("BraceStart").Index
                        For ii = 0 To iBraceList.Length - 1
                            If (iBraceIndex = iBraceList(ii)(0)) Then
                                sTypesetSource = mParseInfo.sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                                bIsValid = True
                                Exit For
                            End If
                        Next
                        If (Not bIsValid) Then
                            Continue For
                        End If

                        iAnchorIndex = 0
                        For j = 0 To mAnchorTypesetMatches.Count - 1
                            If (mSourceAnalysis.m_InNonCode(mAnchorTypesetMatches(j).Index)) Then
                                Continue For
                            End If

                            If (mAnchorTypesetMatches(j).Index > mMatch.Index - 1) Then
                                Exit For
                            End If

                            iAnchorIndex += 1
                        Next

                        sName = mMatch.Groups("Name").Value

                        Dim mTypesetSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sTypesetSource, mParseInfo.iLanguage)
                        Dim iTypesetBraceList As Integer()() = ClassSyntaxTools.ClassSyntaxHelpers.GetExpressionBetweenCharacters(sTypesetSource, "("c, ")"c, 1, mParseInfo.iLanguage, True)

                        Dim mMethodMatches As MatchCollection = ClassRegexLock.Matches(sTypesetSource, String.Format("^\s*(?<Type>\b(function)\b)\s+(?<Tag>\b({0})\b)\s*(?<BraceStart>\()", sRegExTypePattern), RegexOptions.Multiline)

                        Dim SB As StringBuilder

                        For ii = 0 To mMethodMatches.Count - 1
                            If (mTypesetSourceAnalysis.m_InNonCode(mMethodMatches(ii).Groups("Type").Index)) Then
                                Continue For
                            End If

                            SB = New StringBuilder
                            For iii = mMethodMatches(ii).Groups("Type").Index - 1 To 0 Step -1
                                Select Case (sTypesetSource(iii))
                                    Case " "c, vbTab(0), vbLf(0), vbCr(0)
                                        SB.Append(sTypesetSource(iii))
                                    Case Else
                                        If (Not mTypesetSourceAnalysis.m_InMultiComment(iii) AndAlso Not mTypesetSourceAnalysis.m_InSingleComment(iii) AndAlso Not mTypesetSourceAnalysis.m_InPreprocessor(iii)) Then
                                            Exit For
                                        End If

                                        SB.Append(sTypesetSource(iii))
                                End Select
                            Next

                            Dim sComment As String = StrReverse(SB.ToString)
                            sComment = ClassRegexLock.Replace(sComment, "^\s+", "", RegexOptions.Multiline)
                            sComment = ClassRegexLock.Replace(sComment, "\s+$", "", RegexOptions.Multiline)
                            Dim sType As String = mMethodMatches(ii).Groups("Type").Value
                            Dim sTag As String = mMethodMatches(ii).Groups("Tag").Value.Trim
                            Dim iBraceStart As Integer = mMethodMatches(ii).Groups("BraceStart").Index
                            Dim sBraceString As String = Nothing

                            For iii = 0 To iTypesetBraceList.Length - 1
                                If (iBraceStart = iTypesetBraceList(iii)(0)) Then
                                    sBraceString = sTypesetSource.Substring(iTypesetBraceList(iii)(0), iTypesetBraceList(iii)(1) - iTypesetBraceList(iii)(0) + 1)
                                    Exit For
                                End If
                            Next

                            If (String.IsNullOrEmpty(sBraceString)) Then
                                Continue For
                            End If

                            Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(sComment,
                                                                                        IO.Path.GetFileName(mParseInfo.sFile),
                                                                                        mParseInfo.sFile,
                                                                                        ClassSyntaxTools.STRUC_AUTOCOMPLETE.ParseTypeFullNames(sType) Or ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET,
                                                                                        sName,
                                                                                        String.Format("public {0} {1}{2}", sTag, sName, sBraceString),
                                                                                        String.Format("typeset {0} {1} {2}{3}", sName, sType, sTag, sBraceString))

                            mAutocomplete.m_Data("TypesetAnchorName") = "typeset"
                            mAutocomplete.m_Data("TypesetAnchorIndex") = iAnchorIndex
                            mAutocomplete.m_Data("TypesetAnchorFile") = mParseInfo.sFile

                            mAutocomplete.m_Data("TypesetType") = sType
                            mAutocomplete.m_Data("TypesetTag") = sTag
                            mAutocomplete.m_Data("TypesetName") = sName
                            mAutocomplete.m_Data("TypesetArguments") = sBraceString

#If DEBUG Then
                            mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get typesets"
#End If

                            If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                            End If
                        Next
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse typedefs and its entries.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseTypedefs(mFormMain As FormMain, mParseInfo As STRUC_AUTOCOMPLETE_PARSE_POST_INFO)
                If ((ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX) AndAlso
                            mParseInfo.sSource.Contains("typedef")) Then
                    Dim sRegExTypePattern As String = g_ClassParse.GetTypeNamesToPattern(mParseInfo.lNewAutocompleteList)

                    Dim mPossibleTypedefMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, String.Format("^\s*\b(typedef)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s+=\s+\b(function)\b\s+(?<Tag>\b({0})\b)\s*(?<BraceStart>\()", sRegExTypePattern), RegexOptions.Multiline)
                    Dim mAnchorTypedefMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, "\b(typedef)\b", RegexOptions.Multiline)
                    Dim iBraceList As Integer()() = ClassSyntaxTools.ClassSyntaxHelpers.GetExpressionBetweenCharacters(mParseInfo.sSource, "("c, ")"c, 1, mParseInfo.iLanguage, True)
                    Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(mParseInfo.sSource, mParseInfo.iLanguage)

                    Dim mMatch As Match
                    Dim bIsValid As Boolean
                    Dim sTypedefSource As String
                    Dim iBraceIndex As Integer
                    Dim iAnchorIndex As Integer

                    For i = 0 To mPossibleTypedefMatches.Count - 1
                        mMatch = mPossibleTypedefMatches(i)
                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        If (mSourceAnalysis.m_InNonCode(mMatch.Index)) Then
                            Continue For
                        End If

                        bIsValid = False
                        sTypedefSource = ""
                        iBraceIndex = mMatch.Groups("BraceStart").Index
                        For ii = 0 To iBraceList.Length - 1
                            If (iBraceIndex = iBraceList(ii)(0)) Then
                                sTypedefSource = mParseInfo.sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                                bIsValid = True
                                Exit For
                            End If
                        Next
                        If (Not bIsValid) Then
                            Continue For
                        End If

                        If (String.IsNullOrEmpty(sTypedefSource)) Then
                            Continue For
                        End If

                        sTypedefSource = sTypedefSource.Trim

                        iAnchorIndex = 0
                        For j = 0 To mAnchorTypedefMatches.Count - 1
                            If (mSourceAnalysis.m_InNonCode(mAnchorTypedefMatches(j).Index)) Then
                                Continue For
                            End If

                            If (mAnchorTypedefMatches(j).Index > mMatch.Index - 1) Then
                                Exit For
                            End If

                            iAnchorIndex += 1
                        Next

                        Dim sName As String = mMatch.Groups("Name").Value
                        Dim sTag As String = mMatch.Groups("Tag").Value

                        Dim SB As New StringBuilder
                        For iii = mMatch.Index - 1 To 0 Step -1
                            Select Case (mParseInfo.sSource(iii))
                                Case " "c, vbTab(0), vbLf(0), vbCr(0)
                                    SB.Append(mParseInfo.sSource(iii))
                                Case Else
                                    If (Not mSourceAnalysis.m_InMultiComment(iii) AndAlso Not mSourceAnalysis.m_InSingleComment(iii) AndAlso Not mSourceAnalysis.m_InPreprocessor(iii)) Then
                                        Exit For
                                    End If

                                    SB.Append(mParseInfo.sSource(iii))
                            End Select
                        Next

                        If (True) Then
                            Dim sComment As String = StrReverse(SB.ToString)
                            sComment = ClassRegexLock.Replace(sComment, "^\s+", "", RegexOptions.Multiline)
                            sComment = ClassRegexLock.Replace(sComment, "\s+$", "", RegexOptions.Multiline)

                            Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(sComment,
                                                                                        IO.Path.GetFileName(mParseInfo.sFile),
                                                                                        mParseInfo.sFile,
                                                                                        ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF,
                                                                                        sName,
                                                                                        String.Format("public {0} {1}({2})", sTag, sName, sTypedefSource),
                                                                                        String.Format("typedef {0} = function {1} ({2})", sName, sTag, sTypedefSource))

                            mAutocomplete.m_Data("TypedefAnchorName") = "typedef"
                            mAutocomplete.m_Data("TypedefAnchorIndex") = iAnchorIndex
                            mAutocomplete.m_Data("TypedefAnchorFile") = mParseInfo.sFile

                            mAutocomplete.m_Data("TypedefTag") = sTag
                            mAutocomplete.m_Data("TypedefName") = sName
                            mAutocomplete.m_Data("TypedefArguments") = String.Format("({0})", sTypedefSource)

#If DEBUG Then
                            mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get typedefs"
#End If

                            If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                            End If
                        End If
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse methods and functags.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseMethodsAndFunctags(mFormMain As FormMain, mParseInfo As STRUC_AUTOCOMPLETE_PARSE_POST_INFO)
                Dim sRegExTypePattern As String = g_ClassParse.GetTypeNamesToPattern(mParseInfo.lNewAutocompleteList)
                Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(mParseInfo.sSource, mParseInfo.iLanguage)

                Dim iBraceList As Integer()()
                Dim sBraceText As String
                Dim mMatch As Match
                Dim iAnchorIndex As Integer
                Dim sAnchorName As String

                Dim sTypes As String()
                Dim sTag As String
                Dim sName As String
                Dim sFullname As String
                Dim sComment As String
                Dim iTypes As ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS

                Dim mFuncTagMatch As Match

                Dim bCommentStart As Boolean
                Dim mRegMatch2 As Match

                'Remove any array brackets from method types for regex match
                Dim mRegexSource As New Text.StringBuilder(mParseInfo.sSource.Length)
                For j = 0 To mParseInfo.sSource.Length - 1
                    If (mSourceAnalysis.GetBraceLevel(j, Nothing) < 1 AndAlso mSourceAnalysis.GetParenthesisLevel(j, Nothing) < 1) Then
                        If (mSourceAnalysis.GetBracketLevel(j, Nothing) > 0) Then
                            mRegexSource.Append(" "c)
                            Continue For
                        End If
                    End If

                    mRegexSource.Append(mParseInfo.sSource(j))
                Next

                Dim sLines As String() = mRegexSource.ToString.Split(New String() {vbNewLine, vbLf}, 0)

                'If (mSourceAnalysis.m_MaxLength - 1 > 0) Then
                '    Dim iLeftBraceRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                '    Dim iLastBraceLevel As Integer = mSourceAnalysis.GetBraceLevel(mSourceAnalysis.m_MaxLength - 1, iLeftBraceRange)
                '    If (iLastBraceLevel > 0 AndAlso iLeftBraceRange <> ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END) Then
                '        mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Uneven brace level! May lead to syntax parser failures! [LV:{0}] ({1})", iLastBraceLevel, IO.Path.GetFileName(mParseInfo.sFile)), False, False)
                '    End If
                'End If

                For i = 0 To sLines.Length - 1
                    If ((ClassTools.ClassStrings.WordCount(sLines(i), "("c) + ClassTools.ClassStrings.WordCount(sLines(i), ")"c)) Mod 2 <> 0) Then
                        Continue For
                    End If

                    iBraceList = ClassSyntaxTools.ClassSyntaxHelpers.GetExpressionBetweenCharacters(sLines(i), "("c, ")"c, 1, mParseInfo.iLanguage, True)
                    If (iBraceList.Length < 1) Then
                        Continue For
                    End If

                    Dim iIndex = mSourceAnalysis.GetIndexFromLine(i)
                    If (iIndex < 0 OrElse mSourceAnalysis.GetBraceLevel(iBraceList(0)(0) + iIndex, Nothing) > 0 OrElse mSourceAnalysis.m_InNonCode(iBraceList(0)(0) + iIndex)) Then
                        Continue For
                    End If

                    sBraceText = sLines(i).Substring(iBraceList(0)(0), iBraceList(0)(1) - iBraceList(0)(0) + 1)

                    'TODO: Optimize further, this has a big impact on parsing performance
                    Dim sFuncFilter As String = String.Format("({0}|{1}|{2})",
                                                              String.Format("(?<SP17>^\s*(?<Types>[a-zA-Z0-9_ ]*)(?<Tag>\b{0}\b\s)\s*(?<Name>\b[a-zA-Z0-9_]+\b)\s*{1}(?<IsFunc>\s*;){2})", sRegExTypePattern, Regex.Escape(sBraceText), "{0,1}"),       'SP 1.7 +Tags
                                                              String.Format("(?<SP16T>^\s*(?<Types>[a-zA-Z0-9_ ]*)(?<Tag>\b{0}\b\:)(?<Name>\b[a-zA-Z0-9_]+\b)\s*{1}(?<IsFunc>\s*;){2})", sRegExTypePattern, Regex.Escape(sBraceText), "{0,1}"),         'SP 1.6 +Tags
                                                              String.Format("(?<SP16>^\s*(?<Types>[a-zA-Z0-9_ ]*)(?<!<Tag>\b{0}\b\:)(?<Name>\b[a-zA-Z0-9_]+\b)\s*{1}(?<IsFunc>\s*;){2})", sRegExTypePattern, Regex.Escape(sBraceText), "{0,1}"))        'SP 1.6 -Tags

                    mMatch = ClassRegexLock.Match(sLines(i), sFuncFilter)
                    If (Not mMatch.Groups("SP17").Success) Then
                        If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                            Continue For
                        End If

                        If (Not mMatch.Groups("SP16T").Success) Then
                            If (Not mMatch.Groups("SP16").Success) Then
                                Continue For
                            End If
                        End If
                    Else
                        If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_6) Then
                            Continue For
                        End If
                    End If

                    iIndex = mSourceAnalysis.GetIndexFromLine(i)
                    If (iIndex < 0 OrElse mSourceAnalysis.GetBraceLevel(mMatch.Index + iIndex, Nothing) > 0 OrElse mSourceAnalysis.m_InNonCode(mMatch.Index + iIndex)) Then
                        Continue For
                    End If

                    sTypes = mMatch.Groups("Types").Value.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                    sTag = mMatch.Groups("Tag").Value
                    sName = mMatch.Groups("Name").Value
                    sFullname = mMatch.Groups("Types").Value & sTag & sName & sBraceText
                    sComment = ""
                    iTypes = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ParseTypeNames(sTypes)

                    If (ClassSyntaxTools.ClassSyntaxHelpers.IsForbiddenVariableName(sName)) Then
                        Continue For
                    End If

                    If (sTypes.Length < 1 AndAlso mMatch.Groups("IsFunc").Success) Then
                        Continue For
                    End If

                    Dim bIsFunctag As Boolean = False

                    If ((iTypes And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) <> 0) Then
                        While True
                            mFuncTagMatch = ClassRegexLock.Match(sFullname, "(?<Name>\b[a-zA-Z0-9_]+\b)\s+(?<Tag>\b[a-zA-Z0-9_]+\:\b)*\b(public)\b\s*\(")
                            If (mFuncTagMatch.Success) Then
                                sTypes = New String() {"functag"}
                                sTag = mFuncTagMatch.Groups("Tag").Value
                                sName = mFuncTagMatch.Groups("Name").Value
                                sFullname = sTag & sName & sBraceText

                                bIsFunctag = True
                                Exit While
                            End If

                            mFuncTagMatch = ClassRegexLock.Match(sFullname, "\b(public)\b\s+(?<Tag>\b[a-zA-Z0-9_]+\:\b)*(?<Name>\b[a-zA-Z0-9_]+\b)\s*\(")
                            If (mFuncTagMatch.Success) Then
                                sTypes = New String() {"functag"}
                                sTag = mFuncTagMatch.Groups("Tag").Value
                                sName = mFuncTagMatch.Groups("Name").Value
                                sFullname = sTag & sName & sBraceText

                                bIsFunctag = True
                                Exit While
                            End If

                            Continue For
                        End While
                    End If

                    bCommentStart = False
                    For ii = i - 1 To 0 Step -1
                        mRegMatch2 = ClassRegexLock.Match(sLines(ii), "(?<Start>(/\*+))|(?<End>\*+/|(?<Pragma>)^\s*#pragma)")

                        If (Not bCommentStart AndAlso Not mRegMatch2.Groups("End").Success) Then
                            Exit For
                        End If

                        If (Not mRegMatch2.Groups("Pragma").Success) Then
                            bCommentStart = True
                        End If

                        Select Case (True)
                            Case mRegMatch2.Groups("End").Success AndAlso Not mRegMatch2.Groups("Pragma").Success
                                sComment = sLines(ii).Substring(0, mRegMatch2.Groups("End").Index + 2) & Environment.NewLine & sComment
                            Case mRegMatch2.Groups("Start").Success
                                sComment = sLines(ii).Substring(mRegMatch2.Groups("Start").Index, sLines(ii).Length - mRegMatch2.Groups("Start").Index) & Environment.NewLine & sComment
                            Case Else
                                sComment = sLines(ii) & Environment.NewLine & sComment
                        End Select

                        If (bCommentStart AndAlso mRegMatch2.Groups("Start").Success) Then
                            Exit For
                        End If
                    Next

                    sComment = ClassRegexLock.Replace(sComment, "^\s+", "", RegexOptions.Multiline)

                    If (bIsFunctag) Then
                        sAnchorName = "functag"
                        iAnchorIndex = 0
                        For ii = 0 To i - 1
                            If (Not sLines(ii).Contains(sAnchorName)) Then
                                Continue For
                            End If

                            For Each mAnchorMatch As Match In ClassRegexLock.Matches(sLines(ii), String.Format("\b({0})\b", sAnchorName))
                                Dim iAnchorOffset = mSourceAnalysis.GetIndexFromLine(ii) + mAnchorMatch.Index

                                If (mSourceAnalysis.m_InNonCode(iAnchorOffset)) Then
                                    Continue For
                                End If

                                iAnchorIndex += 1
                            Next
                        Next

                        Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(sComment,
                                                                                    IO.Path.GetFileName(mParseInfo.sFile),
                                                                                    mParseInfo.sFile,
                                                                                    ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG,
                                                                                    sName,
                                                                                    String.Format("public {0}", sFullname),
                                                                                    String.Format("functag {0}", sFullname))

                        mAutocomplete.m_Data("FunctagAnchorName") = sAnchorName
                        mAutocomplete.m_Data("FunctagAnchorIndex") = iAnchorIndex
                        mAutocomplete.m_Data("FunctagAnchorFile") = mParseInfo.sFile

                        mAutocomplete.m_Data("FunctagName") = sName
                        mAutocomplete.m_Data("FunctagTag") = sTag.Trim
                        mAutocomplete.m_Data("FunctagArguments") = sBraceText

#If DEBUG Then
                        mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get methods/functags"
#End If

                        If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                            mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                        End If
                    Else
                        'Filter out non-valid method types that 'ParseTypeNames()' may could parse
                        If (iTypes <> ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.NONE) Then
                            Select Case (True)
                                Case (iTypes And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLIC) <> 0
                                    sAnchorName = "public"

                                Case (iTypes And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.NATIVE) <> 0
                                    sAnchorName = "native"

                                Case (iTypes And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STOCK) <> 0
                                    sAnchorName = "stock"

                                Case (iTypes And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STATIC) <> 0
                                    sAnchorName = "static"

                                Case (iTypes And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) <> 0
                                    sAnchorName = "forward"

                                Case Else
                                    Continue For
                            End Select
                        Else
                            sAnchorName = sName
                        End If

                        iAnchorIndex = 0
                        For ii = 0 To i - 1
                            If (Not sLines(ii).Contains(sAnchorName)) Then
                                Continue For
                            End If

                            For Each mAnchorMatch As Match In ClassRegexLock.Matches(sLines(ii), String.Format("\b({0})\b", Regex.Escape(sAnchorName)))
                                Dim iAnchorOffset = mSourceAnalysis.GetIndexFromLine(ii) + mAnchorMatch.Index

                                If (mSourceAnalysis.m_InNonCode(iAnchorOffset)) Then
                                    Continue For
                                End If

                                iAnchorIndex += 1
                            Next
                        Next

                        Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(sComment,
                                                                                    IO.Path.GetFileName(mParseInfo.sFile),
                                                                                    mParseInfo.sFile,
                                                                                    ClassSyntaxTools.STRUC_AUTOCOMPLETE.ParseTypeNames(sTypes) Or ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHOD,
                                                                                    sName,
                                                                                    sName,
                                                                                    sFullname)

                        mAutocomplete.m_Data("MethodAnchorName") = sAnchorName
                        mAutocomplete.m_Data("MethodAnchorIndex") = iAnchorIndex
                        mAutocomplete.m_Data("MethodAnchorFile") = mParseInfo.sFile

                        mAutocomplete.m_Data("MethodName") = sName
                        mAutocomplete.m_Data("MethodType") = String.Join(" ", sTypes)
                        mAutocomplete.m_Data("MethodTag") = sTag.Trim
                        mAutocomplete.m_Data("MethodArguments") = sBraceText

#If DEBUG Then
                        mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get methods/functags"
#End If

                        If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                            mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                        End If
                    End If
                Next
            End Sub

            ''' <summary>
            ''' Parse methodmaps and its entries.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseMethodmaps(mFormMain As FormMain, mParseInfo As STRUC_AUTOCOMPLETE_PARSE_POST_INFO)
                If ((ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX) AndAlso
                            mParseInfo.sSource.Contains("methodmap")) Then
                    Dim sRegExTypePattern As String = g_ClassParse.GetTypeNamesToPattern(mParseInfo.lNewAutocompleteList)

                    Dim mPossibleMethodmapMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, "^\s*\b(methodmap)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)(?<ParentingName>\s+\b[a-zA-Z0-9_]+\b){0,1}(?<FullParent>\s*\<\s*(?<Parent>\b[a-zA-Z0-9_]+\b)){0,1}\s*(?<BraceStart>\{)", RegexOptions.Multiline)
                    Dim mAnchorMethodmapMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, "\b(methodmap)\b", RegexOptions.Multiline)
                    Dim mAnchorPublicMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, "\b(public)\b", RegexOptions.Multiline)
                    Dim mAnchorPropertyMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, "\b(property)\b", RegexOptions.Multiline)
                    Dim iBraceList As Integer()() = ClassSyntaxTools.ClassSyntaxHelpers.GetExpressionBetweenCharacters(mParseInfo.sSource, "{"c, "}"c, 1, mParseInfo.iLanguage, True)
                    Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(mParseInfo.sSource, mParseInfo.iLanguage)

                    Dim mMatch As Match

                    Dim bIsValid As Boolean
                    Dim sMethodmapSource As String
                    Dim iBraceIndex As Integer
                    Dim iAnchorIndex As Integer
                    Dim sAnchorName As String

                    Dim sMethodMapName As String
                    Dim sMethodMapHasParent As Boolean
                    Dim sMethodMapParentName As String
                    Dim sMethodMapFullParentName As String
                    Dim sMethodMapParentingName As String

                    Dim iScopeIndex As Integer = -1

                    For i = 0 To mPossibleMethodmapMatches.Count - 1
                        mMatch = mPossibleMethodmapMatches(i)
                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        bIsValid = False
                        sMethodmapSource = ""
                        iBraceIndex = mMatch.Groups("BraceStart").Index
                        For ii = 0 To iBraceList.Length - 1
                            If (iBraceIndex = iBraceList(ii)(0)) Then
                                sMethodmapSource = mParseInfo.sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                                iScopeIndex = ii

                                bIsValid = True
                                Exit For
                            End If
                        Next
                        If (Not bIsValid) Then
                            Continue For
                        End If

                        sMethodMapName = mMatch.Groups("Name").Value
                        sMethodMapHasParent = mMatch.Groups("Parent").Success
                        sMethodMapParentName = mMatch.Groups("Parent").Value
                        sMethodMapFullParentName = mMatch.Groups("FullParent").Value
                        sMethodMapParentingName = mMatch.Groups("ParentingName").Value

                        If (True) Then
                            sAnchorName = "methodmap"
                            iAnchorIndex = 0
                            For j = 0 To mAnchorMethodmapMatches.Count - 1
                                If (mSourceAnalysis.m_InNonCode(mAnchorMethodmapMatches(j).Index)) Then
                                    Continue For
                                End If

                                If (mAnchorMethodmapMatches(j).Index > mMatch.Index - 1) Then
                                    Exit For
                                End If

                                iAnchorIndex += 1
                            Next

                            Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                        IO.Path.GetFileName(mParseInfo.sFile),
                                                                                        mParseInfo.sFile,
                                                                                        ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP,
                                                                                        sMethodMapName,
                                                                                        sMethodMapName,
                                                                                        "methodmap " & sMethodMapName & sMethodMapFullParentName)

                            'Root information. Childs should have always same information as root entry.
                            mAutocomplete.m_Data("@MethodmapScopeIndex") = iScopeIndex
                            mAutocomplete.m_Data("@MethodmapScopeFile") = mParseInfo.sFile

                            mAutocomplete.m_Data("MethodmapName") = sMethodMapName
                            mAutocomplete.m_Data("MethodmapParentName") = sMethodMapParentName
                            mAutocomplete.m_Data("MethodmapParentingName") = sMethodMapParentingName

                            mAutocomplete.m_Data("MethodmapAnchorName") = sAnchorName
                            mAutocomplete.m_Data("MethodmapAnchorIndex") = iAnchorIndex
                            mAutocomplete.m_Data("MethodmapAnchorFile") = mParseInfo.sFile

                            mAutocomplete.m_Data("MethodmapMethodType") = ""
                            mAutocomplete.m_Data("MethodmapMethodTag") = ""
                            mAutocomplete.m_Data("MethodmapMethodName") = ""
                            mAutocomplete.m_Data("MethodmapMethodArguments") = ""
                            mAutocomplete.m_Data("MethodmapMethodIsConstructor") = False

#If DEBUG Then
                            mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get methodmaps"
#End If

                            If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                            End If
                        End If

                        Dim mMethodmapSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sMethodmapSource, mParseInfo.iLanguage)
                        Dim iMethodmapBraceList As Integer()() = ClassSyntaxTools.ClassSyntaxHelpers.GetExpressionBetweenCharacters(sMethodmapSource, "("c, ")"c, 1, mParseInfo.iLanguage, True)

                        'Remove any array brackets from method types for regex match
                        Dim mRegexSource As New Text.StringBuilder(sMethodmapSource.Length)
                        For j = 0 To sMethodmapSource.Length - 1
                            If (mMethodmapSourceAnalysis.GetBraceLevel(j, Nothing) < 1 AndAlso mMethodmapSourceAnalysis.GetParenthesisLevel(j, Nothing) < 1) Then
                                If (mMethodmapSourceAnalysis.GetBracketLevel(j, Nothing) > 0) Then
                                    mRegexSource.Append(" "c)
                                    Continue For
                                End If
                            End If

                            mRegexSource.Append(sMethodmapSource(j))
                        Next

                        Dim mMethodMatches As MatchCollection = ClassRegexLock.Matches(mRegexSource.ToString,
                                                                              String.Format("^\s*(?<Type>\b(property|public\s+(static\s*){2}(native\s*){4}|public)\b)\s+((?<Tag>\b({0})\b\s)\s*(?<Name>\b[a-zA-Z0-9_]+\b)|(?<Constructor>\b{1}\b)|(?<Name>\b[a-zA-Z0-9_]+\b))\s*(?<BraceStart>\(){3}", sRegExTypePattern, sMethodMapName, "{0,1}", "{0,1}", "{0,1}"),
                                                                              RegexOptions.Multiline)

                        Dim SB As StringBuilder

                        For ii = 0 To mMethodMatches.Count - 1
                            If (mMethodmapSourceAnalysis.m_InNonCode(mMethodMatches(ii).Groups("Type").Index)) Then
                                Continue For
                            End If

                            SB = New StringBuilder
                            For iii = mMethodMatches(ii).Groups("Type").Index - 1 To 0 Step -1
                                Select Case (sMethodmapSource(iii))
                                    Case " "c, vbTab(0), vbLf(0), vbCr(0)
                                        SB.Append(sMethodmapSource(iii))
                                    Case Else
                                        If (Not mMethodmapSourceAnalysis.m_InMultiComment(iii) AndAlso Not mMethodmapSourceAnalysis.m_InSingleComment(iii) AndAlso Not mMethodmapSourceAnalysis.m_InPreprocessor(iii)) Then
                                            Exit For
                                        End If

                                        SB.Append(sMethodmapSource(iii))
                                End Select
                            Next

                            Dim sComment As String = StrReverse(SB.ToString)
                            sComment = ClassRegexLock.Replace(sComment, "^\s+", "", RegexOptions.Multiline)
                            sComment = ClassRegexLock.Replace(sComment, "\s+$", "", RegexOptions.Multiline)
                            Dim sType As String = mMethodMatches(ii).Groups("Type").Value
                            Dim sTag As String = mMethodMatches(ii).Groups("Tag").Value.Trim
                            Dim sName As String = mMethodMatches(ii).Groups("Name").Value

                            If (sName = "get" OrElse sName = "set") Then
                                Continue For
                            End If

                            If (sType = "property") Then
                                sAnchorName = "property"
                                iAnchorIndex = 0
                                For j = 0 To mAnchorPropertyMatches.Count - 1
                                    If (mSourceAnalysis.m_InNonCode(mAnchorPropertyMatches(j).Index)) Then
                                        Continue For
                                    End If

                                    If (mAnchorPropertyMatches(j).Index > mMatch.Index - 1) Then
                                        Exit For
                                    End If

                                    iAnchorIndex += 1
                                Next

                                Dim mSubAnchorPropertyMatches As MatchCollection = ClassRegexLock.Matches(mRegexSource.ToString, String.Format("\b({0})\b", sAnchorName), RegexOptions.Multiline)
                                For j = 0 To mSubAnchorPropertyMatches.Count - 1
                                    If (mMethodmapSourceAnalysis.m_InNonCode(mSubAnchorPropertyMatches(j).Index)) Then
                                        Continue For
                                    End If

                                    If (mSubAnchorPropertyMatches(j).Index > mMethodMatches(ii).Index - 1) Then
                                        Exit For
                                    End If

                                    iAnchorIndex += 1
                                Next


                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(sComment,
                                                                                            IO.Path.GetFileName(mParseInfo.sFile),
                                                                                            mParseInfo.sFile,
                                                                                            ClassSyntaxTools.STRUC_AUTOCOMPLETE.ParseTypeFullNames(sType) Or ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PROPERTY Or ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP,
                                                                                            sName,
                                                                                            String.Format("{0}.{1}", sMethodMapName, sName),
                                                                                            String.Format("methodmap {0} {1}{2}{3} = {4} {5}", sType, sMethodMapName, sMethodMapParentingName, If(sMethodMapHasParent, " < " & sMethodMapParentName, ""), sTag, sName))

                                'Root information. Childs should have always same information as root entry.
                                mAutocomplete.m_Data("@MethodmapScopeIndex") = iScopeIndex
                                mAutocomplete.m_Data("@MethodmapScopeFile") = mParseInfo.sFile

                                mAutocomplete.m_Data("MethodmapName") = sMethodMapName
                                mAutocomplete.m_Data("MethodmapParentName") = sMethodMapParentName
                                mAutocomplete.m_Data("MethodmapParentingName") = sMethodMapParentingName

                                mAutocomplete.m_Data("MethodmapAnchorName") = sAnchorName
                                mAutocomplete.m_Data("MethodmapAnchorIndex") = iAnchorIndex
                                mAutocomplete.m_Data("MethodmapAnchorFile") = mParseInfo.sFile

                                mAutocomplete.m_Data("MethodmapMethodType") = sType
                                mAutocomplete.m_Data("MethodmapMethodTag") = sTag
                                mAutocomplete.m_Data("MethodmapMethodName") = sName
                                mAutocomplete.m_Data("MethodmapMethodArguments") = ""
                                mAutocomplete.m_Data("MethodmapMethodIsConstructor") = False

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get methodmaps"
#End If

                                If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                    mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                                End If
                            Else
                                sAnchorName = "public"
                                iAnchorIndex = 0
                                For j = 0 To mAnchorPublicMatches.Count - 1
                                    If (mSourceAnalysis.m_InNonCode(mAnchorPublicMatches(j).Index)) Then
                                        Continue For
                                    End If

                                    If (mAnchorPublicMatches(j).Index > mMatch.Index - 1) Then
                                        Exit For
                                    End If

                                    iAnchorIndex += 1
                                Next

                                Dim mSubAnchorPublicMatches As MatchCollection = ClassRegexLock.Matches(mRegexSource.ToString, String.Format("\b({0})\b", sAnchorName), RegexOptions.Multiline)
                                For j = 0 To mSubAnchorPublicMatches.Count - 1
                                    If (mMethodmapSourceAnalysis.m_InNonCode(mSubAnchorPublicMatches(j).Index)) Then
                                        Continue For
                                    End If

                                    If (mSubAnchorPublicMatches(j).Index > mMethodMatches(ii).Index - 1) Then
                                        Exit For
                                    End If

                                    iAnchorIndex += 1
                                Next


                                Dim bIsConstructor As Boolean = mMethodMatches(ii).Groups("Constructor").Success
                                Dim iBraceStart As Integer = mMethodMatches(ii).Groups("BraceStart").Index
                                Dim sBraceString As String = Nothing

                                For iii = 0 To iMethodmapBraceList.Length - 1
                                    If (iBraceStart = iMethodmapBraceList(iii)(0)) Then
                                        sBraceString = sMethodmapSource.Substring(iMethodmapBraceList(iii)(0), iMethodmapBraceList(iii)(1) - iMethodmapBraceList(iii)(0) + 1)
                                        Exit For
                                    End If
                                Next

                                If (String.IsNullOrEmpty(sBraceString)) Then
                                    Continue For
                                End If

                                If (bIsConstructor) Then
                                    Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(sComment,
                                                                                                IO.Path.GetFileName(mParseInfo.sFile),
                                                                                                mParseInfo.sFile,
                                                                                                ClassSyntaxTools.STRUC_AUTOCOMPLETE.ParseTypeFullNames(sType) Or ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP,
                                                                                                sMethodMapName,
                                                                                                sMethodMapName,
                                                                                                String.Format("methodmap {0}{1} = {2}{3}", sMethodMapName, If(sMethodMapHasParent, " < " & sMethodMapParentName, ""), sMethodMapName, sBraceString))

                                    'Root information. Childs should have always same information as root entry.
                                    mAutocomplete.m_Data("@MethodmapScopeIndex") = iScopeIndex
                                    mAutocomplete.m_Data("@MethodmapScopeFile") = mParseInfo.sFile

                                    mAutocomplete.m_Data("MethodmapName") = sMethodMapName
                                    mAutocomplete.m_Data("MethodmapParentName") = sMethodMapParentName
                                    mAutocomplete.m_Data("MethodmapParentingName") = sMethodMapParentingName

                                    mAutocomplete.m_Data("MethodmapAnchorName") = sAnchorName
                                    mAutocomplete.m_Data("MethodmapAnchorIndex") = iAnchorIndex
                                    mAutocomplete.m_Data("MethodmapAnchorFile") = mParseInfo.sFile

                                    mAutocomplete.m_Data("MethodmapMethodType") = sType
                                    mAutocomplete.m_Data("MethodmapMethodTag") = sTag
                                    mAutocomplete.m_Data("MethodmapMethodName") = sName
                                    mAutocomplete.m_Data("MethodmapMethodArguments") = sBraceString
                                    mAutocomplete.m_Data("MethodmapMethodIsConstructor") = True

#If DEBUG Then
                                    mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get methodmaps"
#End If

                                    'Remove all single methodmaps and replace them with the constructor, the enum version needs to stay for autocompletion.
                                    'mParseInfo.lNewAutocompleteList.RemoveAll(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)

                                    If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                        mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                                    End If
                                Else
                                    Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(sComment,
                                                                                                IO.Path.GetFileName(mParseInfo.sFile),
                                                                                                mParseInfo.sFile,
                                                                                                ClassSyntaxTools.STRUC_AUTOCOMPLETE.ParseTypeFullNames(sType) Or ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.INLINE_METHOD Or ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP,
                                                                                                sName,
                                                                                                String.Format("{0}.{1}", sMethodMapName, sName),
                                                                                                String.Format("methodmap {0} {1}{2} = {3} {4}{5}", sType, sMethodMapName, If(sMethodMapHasParent, " < " & sMethodMapParentName, ""), sTag, sName, sBraceString))

                                    'Root information. Childs should have always same information as root entry.
                                    mAutocomplete.m_Data("@MethodmapScopeIndex") = iScopeIndex
                                    mAutocomplete.m_Data("@MethodmapScopeFile") = mParseInfo.sFile

                                    mAutocomplete.m_Data("InlineMethodName") = sName
                                    mAutocomplete.m_Data("InlineMethodType") = sType
                                    mAutocomplete.m_Data("InlineMethodTag") = sTag
                                    mAutocomplete.m_Data("InlineMethodArguments") = sBraceString

                                    mAutocomplete.m_Data("MethodmapName") = sMethodMapName
                                    mAutocomplete.m_Data("MethodmapParentName") = sMethodMapParentName
                                    mAutocomplete.m_Data("MethodmapParentingName") = sMethodMapParentingName

                                    mAutocomplete.m_Data("MethodmapAnchorName") = sAnchorName
                                    mAutocomplete.m_Data("MethodmapAnchorIndex") = iAnchorIndex
                                    mAutocomplete.m_Data("MethodmapAnchorFile") = mParseInfo.sFile

                                    mAutocomplete.m_Data("MethodmapMethodType") = sType
                                    mAutocomplete.m_Data("MethodmapMethodTag") = sTag
                                    mAutocomplete.m_Data("MethodmapMethodName") = sName
                                    mAutocomplete.m_Data("MethodmapMethodArguments") = sBraceString
                                    mAutocomplete.m_Data("MethodmapMethodIsConstructor") = False

#If DEBUG Then
                                    mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get methodmaps"
#End If

                                    If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                        mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                                    End If
                                End If
                            End If
                        Next
                    Next
                End If
            End Sub

            ''' <summary>
            ''' Parse enum structs and its entries.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseEnumStructs(mFormMain As FormMain, mParseInfo As STRUC_AUTOCOMPLETE_PARSE_POST_INFO)
                If ((ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX) AndAlso
                            mParseInfo.sSource.Contains("enum") AndAlso mParseInfo.sSource.Contains("struct")) Then
                    Dim sRegExTypePattern As String = g_ClassParse.GetTypeNamesToPattern(mParseInfo.lNewAutocompleteList)

                    Dim mPossibleEnumStructMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, "^\s*\b(enum)\b\s+\b(struct)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)", RegexOptions.Multiline)
                    Dim mAnchorEnumStructMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, "\b(enum)\b", RegexOptions.Multiline)
                    Dim iBraceList As Integer()() = ClassSyntaxTools.ClassSyntaxHelpers.GetExpressionBetweenCharacters(mParseInfo.sSource, "{"c, "}"c, 1, mParseInfo.iLanguage, True)
                    Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(mParseInfo.sSource, mParseInfo.iLanguage)

                    Dim mMatch As Match

                    Dim bIsValid As Boolean
                    Dim sEnumStructSource As String
                    Dim iBraceIndex As Integer
                    Dim iAnchorIndex As Integer
                    Dim sAnchorName As String

                    Dim sEnumStructName As String

                    Dim iScopeIndex As Integer = -1

                    For i = 0 To mPossibleEnumStructMatches.Count - 1
                        mMatch = mPossibleEnumStructMatches(i)
                        If (Not mMatch.Success) Then
                            Continue For
                        End If

                        bIsValid = False
                        sEnumStructSource = ""
                        iBraceIndex = mMatch.Groups("BraceStart").Index
                        For ii = 0 To iBraceList.Length - 1
                            If (iBraceIndex = iBraceList(ii)(0)) Then
                                sEnumStructSource = mParseInfo.sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                                iScopeIndex = ii

                                bIsValid = True
                                Exit For
                            End If
                        Next
                        If (Not bIsValid) Then
                            Continue For
                        End If

                        sEnumStructName = mMatch.Groups("Name").Value

                        If (True) Then
                            sAnchorName = "enum"
                            iAnchorIndex = 0
                            For j = 0 To mAnchorEnumStructMatches.Count - 1
                                If (mSourceAnalysis.m_InNonCode(mAnchorEnumStructMatches(j).Index)) Then
                                    Continue For
                                End If

                                If (mAnchorEnumStructMatches(j).Index > mMatch.Index - 1) Then
                                    Exit For
                                End If

                                iAnchorIndex += 1
                            Next

                            Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                        IO.Path.GetFileName(mParseInfo.sFile),
                                                                                        mParseInfo.sFile,
                                                                                        ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT,
                                                                                        sEnumStructName,
                                                                                        sEnumStructName,
                                                                                        "enum struct " & sEnumStructName)

                            'Root information. Childs should have always same information as root entry.
                            mAutocomplete.m_Data("@EnumStructScopeIndex") = iScopeIndex
                            mAutocomplete.m_Data("@EnumStructScopeFile") = mParseInfo.sFile

                            mAutocomplete.m_Data("EnumStructAnchorName") = sAnchorName
                            mAutocomplete.m_Data("EnumStructAnchorIndex") = iAnchorIndex
                            mAutocomplete.m_Data("EnumStructAnchorFile") = mParseInfo.sFile

                            mAutocomplete.m_Data("EnumStructName") = sEnumStructName
                            mAutocomplete.m_Data("EnumStructFieldTag") = ""
                            mAutocomplete.m_Data("EnumStructFieldName") = ""
                            mAutocomplete.m_Data("EnumStructMethodTag") = ""
                            mAutocomplete.m_Data("EnumStructMethodName") = ""
                            mAutocomplete.m_Data("EnumStructMethodArguments") = ""

#If DEBUG Then
                            mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get enum structs"
#End If

                            If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                            End If
                        End If

                        Dim mEnumStructSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sEnumStructSource, mParseInfo.iLanguage)
                        Dim iEnumStructBraceList As Integer()() = ClassSyntaxTools.ClassSyntaxHelpers.GetExpressionBetweenCharacters(sEnumStructSource, "("c, ")"c, 1, mParseInfo.iLanguage, True)

                        'Get fields
                        If (True) Then
                            Dim SB As New StringBuilder

                            'Remove everthing we dont need
                            For j = 0 To sEnumStructSource.Length - 1
                                If (mEnumStructSourceAnalysis.m_InNonCode(j) OrElse mEnumStructSourceAnalysis.m_InPreprocessor(j)) Then
                                    SB.Append(" ")
                                    Continue For
                                End If

                                Dim jj As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                                If ((mEnumStructSourceAnalysis.GetBraceLevel(j, jj) > 0 AndAlso jj = ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.NONE) OrElse
                                            (mEnumStructSourceAnalysis.GetBracketLevel(j, jj) > 0 AndAlso jj = ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.NONE) OrElse
                                            (mEnumStructSourceAnalysis.GetParenthesisLevel(j, jj) > 0 AndAlso jj = ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.NONE)) Then
                                    SB.Append(" ")
                                    Continue For
                                End If

                                SB.Append(sEnumStructSource(j))
                            Next

                            Dim mFieldMatches As MatchCollection = ClassRegexLock.Matches(SB.ToString,
                                                                              String.Format("^\s*(?<Tag>\b({0})\b\s)\s*(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<IsMethod>\(){1}", sRegExTypePattern, "{0,1}"),
                                                                              RegexOptions.Multiline)


                            For ii = 0 To mFieldMatches.Count - 1
                                If (mFieldMatches(ii).Groups("IsMethod").Success) Then
                                    Continue For
                                End If

                                Dim sTag As String = mFieldMatches(ii).Groups("Tag").Value.Trim
                                Dim sName As String = mFieldMatches(ii).Groups("Name").Value

                                'Just go to the struct instead
                                sAnchorName = sName
                                iAnchorIndex = 0
                                Dim mAnchorFieldMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, String.Format("\b({0})\b", Regex.Escape(sAnchorName)), RegexOptions.Multiline)
                                For j = 0 To mAnchorFieldMatches.Count - 1
                                    If (mSourceAnalysis.m_InNonCode(mAnchorFieldMatches(j).Index)) Then
                                        Continue For
                                    End If

                                    If (mAnchorFieldMatches(j).Index > mMatch.Index - 1) Then
                                        Exit For
                                    End If

                                    iAnchorIndex += 1
                                Next

                                Dim mSubAnchorFieldMatches As MatchCollection = ClassRegexLock.Matches(sEnumStructSource, String.Format("\b({0})\b", Regex.Escape(sAnchorName)), RegexOptions.Multiline)
                                For j = 0 To mSubAnchorFieldMatches.Count - 1
                                    If (mEnumStructSourceAnalysis.m_InNonCode(mSubAnchorFieldMatches(j).Index)) Then
                                        Continue For
                                    End If

                                    If (mSubAnchorFieldMatches(j).Index > mFieldMatches(ii).Index - 1) Then
                                        Exit For
                                    End If

                                    iAnchorIndex += 1
                                Next


                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                                 IO.Path.GetFileName(mParseInfo.sFile),
                                                                                                 mParseInfo.sFile,
                                                                                                 ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FIELD Or ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT,
                                                                                                 sName,
                                                                                                 String.Format("{0}.{1}", sEnumStructName, sName),
                                                                                                 String.Format("enum struct {0} = {1} {2}", sEnumStructName, sTag, sName))

                                'Root information. Childs should have always same information as root entry.
                                mAutocomplete.m_Data("@EnumStructScopeIndex") = iScopeIndex
                                mAutocomplete.m_Data("@EnumStructScopeFile") = mParseInfo.sFile

                                mAutocomplete.m_Data("EnumStructAnchorName") = sAnchorName
                                mAutocomplete.m_Data("EnumStructAnchorIndex") = iAnchorIndex
                                mAutocomplete.m_Data("EnumStructAnchorFile") = mParseInfo.sFile

                                mAutocomplete.m_Data("EnumStructName") = sEnumStructName
                                mAutocomplete.m_Data("EnumStructFieldTag") = sTag
                                mAutocomplete.m_Data("EnumStructFieldName") = sName
                                mAutocomplete.m_Data("EnumStructMethodTag") = ""
                                mAutocomplete.m_Data("EnumStructMethodName") = ""
                                mAutocomplete.m_Data("EnumStructMethodArguments") = ""

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get enum structs"
#End If

                                If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                    mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                                End If
                            Next
                        End If

                        'Get methods
                        If (True) Then
                            'Remove any array brackets from method types for regex match
                            Dim mRegexSource As New Text.StringBuilder(sEnumStructSource.Length)
                            For j = 0 To sEnumStructSource.Length - 1
                                If (mEnumStructSourceAnalysis.GetBraceLevel(j, Nothing) < 1 AndAlso mEnumStructSourceAnalysis.GetParenthesisLevel(j, Nothing) < 1) Then
                                    If (mEnumStructSourceAnalysis.GetBracketLevel(j, Nothing) > 0) Then
                                        mRegexSource.Append(" "c)
                                        Continue For
                                    End If
                                End If

                                mRegexSource.Append(sEnumStructSource(j))
                            Next

                            Dim mMethodMatches As MatchCollection = ClassRegexLock.Matches(mRegexSource.ToString,
                                                                             String.Format("^\s*(?<Type>)(?<Tag>\b({0})\b)\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\()", sRegExTypePattern),
                                                                             RegexOptions.Multiline)

                            Dim SB As StringBuilder

                            For ii = 0 To mMethodMatches.Count - 1
                                If (mEnumStructSourceAnalysis.m_InNonCode(mMethodMatches(ii).Groups("Type").Index)) Then
                                    Continue For
                                End If

                                SB = New StringBuilder
                                For iii = mMethodMatches(ii).Groups("Type").Index - 1 To 0 Step -1
                                    Select Case (sEnumStructSource(iii))
                                        Case " "c, vbTab(0), vbLf(0), vbCr(0)
                                            SB.Append(sEnumStructSource(iii))
                                        Case Else
                                            If (Not mEnumStructSourceAnalysis.m_InMultiComment(iii) AndAlso Not mEnumStructSourceAnalysis.m_InSingleComment(iii) AndAlso Not mEnumStructSourceAnalysis.m_InPreprocessor(iii)) Then
                                                Exit For
                                            End If

                                            SB.Append(sEnumStructSource(iii))
                                    End Select
                                Next

                                Dim sComment As String = StrReverse(SB.ToString)
                                sComment = ClassRegexLock.Replace(sComment, "^\s+", "", RegexOptions.Multiline)
                                sComment = ClassRegexLock.Replace(sComment, "\s+$", "", RegexOptions.Multiline)
                                Dim sTag As String = mMethodMatches(ii).Groups("Tag").Value.Trim
                                Dim sName As String = mMethodMatches(ii).Groups("Name").Value

                                Dim iBraceStart As Integer = mMethodMatches(ii).Groups("BraceStart").Index
                                Dim sBraceString As String = Nothing

                                For iii = 0 To iEnumStructBraceList.Length - 1
                                    If (iBraceStart = iEnumStructBraceList(iii)(0)) Then
                                        sBraceString = sEnumStructSource.Substring(iEnumStructBraceList(iii)(0), iEnumStructBraceList(iii)(1) - iEnumStructBraceList(iii)(0) + 1)
                                        Exit For
                                    End If
                                Next

                                If (String.IsNullOrEmpty(sBraceString)) Then
                                    Continue For
                                End If


                                sAnchorName = sName
                                iAnchorIndex = 0
                                Dim mAnchorMethodMatches As MatchCollection = ClassRegexLock.Matches(mParseInfo.sSource, String.Format("\b({0})\b", Regex.Escape(sAnchorName)), RegexOptions.Multiline)
                                For j = 0 To mAnchorMethodMatches.Count - 1
                                    If (mSourceAnalysis.m_InNonCode(mAnchorMethodMatches(j).Index)) Then
                                        Continue For
                                    End If

                                    If (mAnchorMethodMatches(j).Index > mMatch.Index - 1) Then
                                        Exit For
                                    End If

                                    iAnchorIndex += 1
                                Next

                                Dim mSubAnchorMethodMatches As MatchCollection = ClassRegexLock.Matches(mRegexSource.ToString, String.Format("\b({0})\b", Regex.Escape(sAnchorName)), RegexOptions.Multiline)
                                For j = 0 To mSubAnchorMethodMatches.Count - 1
                                    If (mEnumStructSourceAnalysis.m_InNonCode(mSubAnchorMethodMatches(j).Index)) Then
                                        Continue For
                                    End If

                                    If (mSubAnchorMethodMatches(j).Index > mMethodMatches(ii).Index - 1) Then
                                        Exit For
                                    End If

                                    iAnchorIndex += 1
                                Next


                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(sComment,
                                                                                                    IO.Path.GetFileName(mParseInfo.sFile),
                                                                                                    mParseInfo.sFile,
                                                                                                    ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.INLINE_METHOD Or ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT,
                                                                                                    sName,
                                                                                                    String.Format("{0}.{1}", sEnumStructName, sName),
                                                                                                    String.Format("enum struct {0} = {1} {2}{3}", sEnumStructName, sTag, sName, sBraceString))

                                mAutocomplete.m_Data("InlineMethodName") = sName
                                mAutocomplete.m_Data("InlineMethodType") = ""
                                mAutocomplete.m_Data("InlineMethodTag") = sTag
                                mAutocomplete.m_Data("InlineMethodArguments") = sBraceString

                                'Root information. Childs should have always same information as root entry.
                                mAutocomplete.m_Data("@EnumStructScopeIndex") = iScopeIndex
                                mAutocomplete.m_Data("@EnumStructScopeFile") = mParseInfo.sFile

                                mAutocomplete.m_Data("EnumStructAnchorName") = sAnchorName
                                mAutocomplete.m_Data("EnumStructAnchorIndex") = iAnchorIndex
                                mAutocomplete.m_Data("EnumStructAnchorFile") = mParseInfo.sFile

                                mAutocomplete.m_Data("EnumStructName") = sEnumStructName
                                mAutocomplete.m_Data("EnumStructFieldTag") = ""
                                mAutocomplete.m_Data("EnumStructFieldName") = ""
                                mAutocomplete.m_Data("EnumStructMethodTag") = sTag
                                mAutocomplete.m_Data("EnumStructMethodName") = sName
                                mAutocomplete.m_Data("EnumStructMethodArguments") = sBraceString

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Get enum structs"
#End If

                                If (Not mParseInfo.lNewAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                    mParseInfo.lNewAutocompleteList.Add(mAutocomplete)
                                End If
                            Next
                        End If

                    Next
                End If
            End Sub
        End Class

        Private Class ClassFullSyntaxFinalize
            Public g_ClassParse As ClassParser

            Sub New(_ClassParse As ClassParser)
                g_ClassParse = _ClassParse
            End Sub

            ''' <summary>
            ''' Combines methodmap methods from parents.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="lTmpAutoList"></param>
            Public Sub ProcessMethodmapParentMethods(mFormMain As FormMain, lTmpAutoList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE))
                'Merging:
                '     methodmap Test1 : Handle {...}
                '     methodmap Test2 : Test1 {...}     (All Test1 methods etc. will be added to Test2, because its a child of Test1)
                '   to
                '       Test1.Close
                '       Test2.Close 

                Dim lTmpAutoAddList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

                For i = lTmpAutoList.Count - 1 To 0 Step -1
                    If ((lTmpAutoList(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = 0 OrElse
                                    Not lTmpAutoList(i).m_FunctionString.Contains("."c)) Then
                        Continue For
                    End If

                    Dim sMethodmapName As String = CStr(lTmpAutoList(i).m_Data("MethodmapName"))
                    Dim sMethodmapParentName As String = CStr(lTmpAutoList(i).m_Data("MethodmapParentName"))
                    If (String.IsNullOrEmpty(sMethodmapName) OrElse String.IsNullOrEmpty(sMethodmapParentName)) Then
                        Continue For
                    End If

                    Dim iOldNextParent As String = sMethodmapParentName

                    While (Not String.IsNullOrEmpty(iOldNextParent))
                        Dim sNextParent As String = ""

                        For ii = 0 To lTmpAutoList.Count - 1
                            If ((lTmpAutoList(ii).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = 0 OrElse
                                            Not lTmpAutoList(ii).m_FunctionString.Contains("."c)) Then
                                Continue For
                            End If

                            'TODO: Dont use yet, make methodmap parsing more efficent first
                            'If ((lTmpAutoList(ii).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STATIC) = 0) Then
                            '    Continue For
                            'End If

                            Dim sParentMethodmapName As String = CStr(lTmpAutoList(ii).m_Data("MethodmapName"))
                            Dim sParentMethodmapParentName As String = CStr(lTmpAutoList(ii).m_Data("MethodmapParentName"))
                            Dim sParentMethodmapMethodName As String = CStr(lTmpAutoList(ii).m_Data("MethodmapMethodName"))
                            If (String.IsNullOrEmpty(sParentMethodmapName) OrElse String.IsNullOrEmpty(sParentMethodmapMethodName)) Then
                                Continue For
                            End If

                            If (iOldNextParent <> sParentMethodmapName) Then
                                Continue For
                            End If

                            sNextParent = sParentMethodmapParentName

                            Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(lTmpAutoList(ii).m_Info,
                                                                                            lTmpAutoList(ii).m_Filename,
                                                                                            lTmpAutoList(ii).m_Path,
                                                                                            lTmpAutoList(ii).m_Type,
                                                                                            sParentMethodmapMethodName,
                                                                                            String.Format("{0}.{1}", sMethodmapName, sParentMethodmapMethodName),
                                                                                            lTmpAutoList(ii).m_FullFunctionString)

                            For Each mData In lTmpAutoList(ii).m_Data
                                mAutocomplete.m_Data(mData.Key) = mData.Value
                            Next

                            'Keep root information
                            For Each mData In lTmpAutoList(i).m_Data
                                If (mData.Key.StartsWith("@"c)) Then
                                    mAutocomplete.m_Data(mData.Key) = mData.Value
                                End If
                            Next

                            mAutocomplete.m_Data("VariableMethodmapName") = sParentMethodmapName
                            mAutocomplete.m_Data("VariableMethodmapMethod") = sParentMethodmapMethodName

#If DEBUG Then
                            mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Merges all methods to methodmaps"
#End If

                            If (Not lTmpAutoList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString) AndAlso
                                        Not lTmpAutoAddList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                lTmpAutoAddList.Add(mAutocomplete)
                            End If
                        Next

                        iOldNextParent = sNextParent
                    End While
                Next

                lTmpAutoList.AddRange(lTmpAutoAddList.ToArray)
            End Sub

            ''' <summary>
            ''' Combines methodmap from mathodmap parents.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="lTmpAutoList"></param>
            Public Sub ProcessMethodmapParentMethodmaps(mFormMain As FormMain, lTmpAutoList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE))
                'Merging:
                '       methodmap Test1 : Handle = Test2 Method() {...}
                '   to
                '       Method.Close 
                Dim lTmpAutoAddList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

                For i = lTmpAutoList.Count - 1 To 0 Step -1
                    If ((lTmpAutoList(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = 0 OrElse
                                    Not lTmpAutoList(i).m_FunctionString.Contains("."c)) Then
                        Continue For
                    End If

                    Dim sMethodmapMethodName As String = CStr(lTmpAutoList(i).m_Data("MethodmapMethodName"))
                    Dim sMethodmapMethodTag As String = CStr(lTmpAutoList(i).m_Data("MethodmapMethodTag"))
                    If (String.IsNullOrEmpty(sMethodmapMethodName) OrElse String.IsNullOrEmpty(sMethodmapMethodTag)) Then
                        Continue For
                    End If

                    Dim iOldNextParent As String = sMethodmapMethodTag

                    While (Not String.IsNullOrEmpty(iOldNextParent))
                        Dim sNextParent As String = ""

                        For ii = 0 To lTmpAutoList.Count - 1
                            If ((lTmpAutoList(ii).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = 0 OrElse
                                            Not lTmpAutoList(ii).m_FunctionString.Contains("."c)) Then
                                Continue For
                            End If

                            'TODO: Dont use yet, make methodmap parsing more efficent first
                            'If ((lTmpAutoList(ii).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STATIC) = 0) Then
                            '    Continue For
                            'End If

                            Dim sParentMethodmapName As String = CStr(lTmpAutoList(ii).m_Data("MethodmapName"))
                            Dim sParentMethodmapParentName As String = CStr(lTmpAutoList(ii).m_Data("MethodmapParentName"))
                            Dim sParentMethodmapMethodName As String = CStr(lTmpAutoList(ii).m_Data("MethodmapMethodName"))
                            If (String.IsNullOrEmpty(sParentMethodmapName) OrElse String.IsNullOrEmpty(sParentMethodmapMethodName)) Then
                                Continue For
                            End If

                            If (iOldNextParent <> sParentMethodmapName) Then
                                Continue For
                            End If

                            sNextParent = sParentMethodmapParentName

                            Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(lTmpAutoList(ii).m_Info,
                                                                                            lTmpAutoList(ii).m_Filename,
                                                                                            lTmpAutoList(ii).m_Path,
                                                                                            lTmpAutoList(ii).m_Type,
                                                                                            sParentMethodmapMethodName,
                                                                                            String.Format("{0}.{1}", sMethodmapMethodName, sParentMethodmapMethodName),
                                                                                            lTmpAutoList(ii).m_FullFunctionString)

                            For Each mData In lTmpAutoList(ii).m_Data
                                mAutocomplete.m_Data(mData.Key) = mData.Value
                            Next

                            'Keep root info
                            For Each mData In lTmpAutoList(i).m_Data
                                If (mData.Key.StartsWith("@"c)) Then
                                    mAutocomplete.m_Data(mData.Key) = mData.Value
                                End If
                            Next

                            mAutocomplete.m_Data("VariableMethodmapName") = sMethodmapMethodName
                            mAutocomplete.m_Data("VariableMethodmapMethod") = sParentMethodmapMethodName

#If DEBUG Then
                            mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Merges all methodmaps with parent methodmaps"
#End If

                            If (Not lTmpAutoList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString) AndAlso
                                        Not lTmpAutoAddList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                lTmpAutoAddList.Add(mAutocomplete)
                            End If
                        Next

                        iOldNextParent = sNextParent
                    End While
                Next

                lTmpAutoList.AddRange(lTmpAutoAddList.ToArray)
            End Sub

            ''' <summary>
            ''' Generates combined methodmap 'this' keywords.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="lTmpAutoList"></param>
            Public Sub GenerateMethodmapThis(mFormMain As FormMain, lTmpAutoList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE))
                'Merging:
                '     methodmap Test1 : Handle {...}
                '   to
                '       this.Close
                Dim lTmpAutoAddList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

                For i = lTmpAutoList.Count - 1 To 0 Step -1
                    If ((lTmpAutoList(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = 0 OrElse
                                    Not lTmpAutoList(i).m_FunctionString.Contains("."c)) Then
                        Continue For
                    End If

                    Dim sMethodmapMethodName As String = CStr(lTmpAutoList(i).m_Data("MethodmapMethodName"))
                    Dim sMethodmapMethodTag As String = CStr(lTmpAutoList(i).m_Data("MethodmapMethodTag"))
                    If (String.IsNullOrEmpty(sMethodmapMethodName) OrElse String.IsNullOrEmpty(sMethodmapMethodTag)) Then
                        Continue For
                    End If

                    Dim iMethodmapScopeIndex As Integer = CInt(lTmpAutoList(i).m_Data("@MethodmapScopeIndex"))
                    Dim sMethodmapScopeFile As String = CStr(lTmpAutoList(i).m_Data("@MethodmapScopeFile"))

                    Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(lTmpAutoList(i).m_Info,
                                                                                      lTmpAutoList(i).m_Filename,
                                                                                      lTmpAutoList(i).m_Path,
                                                                                      lTmpAutoList(i).m_Type,
                                                                                      sMethodmapMethodName,
                                                                                      String.Format("this.{0}", sMethodmapMethodName),
                                                                                      lTmpAutoList(i).m_FullFunctionString)

                    For Each mData In lTmpAutoList(i).m_Data
                        mAutocomplete.m_Data(mData.Key) = mData.Value
                    Next

                    mAutocomplete.m_Data("IsThis") = True

#If DEBUG Then
                    mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Generates combined methodmap 'this' keywords"
#End If

                    If (Not lTmpAutoList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString AndAlso
                                                                                                            CInt(x.m_Data("@MethodmapScopeIndex")) = iMethodmapScopeIndex AndAlso
                                                                                                            CStr(x.m_Data("@MethodmapScopeFile")) = sMethodmapScopeFile) AndAlso
                                Not lTmpAutoAddList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString AndAlso
                                                                                                                        CInt(x.m_Data("@MethodmapScopeIndex")) = iMethodmapScopeIndex AndAlso
                                                                                                                        CStr(x.m_Data("@MethodmapScopeFile")) = sMethodmapScopeFile)) Then
                        lTmpAutoAddList.Add(mAutocomplete)
                    End If
                Next

                lTmpAutoList.AddRange(lTmpAutoAddList.ToArray)
            End Sub

            ''' <summary>
            ''' Generates combined enum struct 'this' keywords.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="lTmpAutoList"></param>
            Public Sub GenerateEnumStructThis(mFormMain As FormMain, lTmpAutoList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE))
                'Merging:
                '     enum struct Test1 { int Value; }
                '   to
                '       this.Value
                Dim lTmpAutoAddList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

                For i = lTmpAutoList.Count - 1 To 0 Step -1
                    If ((lTmpAutoList(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT) = 0 OrElse
                                            Not lTmpAutoList(i).m_FunctionString.Contains("."c)) Then
                        Continue For
                    End If

                    Dim sEnumStructName As String = CStr(lTmpAutoList(i).m_Data("EnumStructName"))
                    Dim sEnumStructFieldName As String = CStr(lTmpAutoList(i).m_Data("EnumStructFieldName"))
                    Dim sEnumStructMethodName As String = CStr(lTmpAutoList(i).m_Data("EnumStructMethodName"))
                    Dim sEnumStructTargetName As String
                    Dim bIsField As Boolean = False

                    If ((lTmpAutoList(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FIELD) <> 0) Then
                        sEnumStructTargetName = sEnumStructFieldName
                        bIsField = True
                    Else
                        sEnumStructTargetName = sEnumStructMethodName
                        bIsField = False
                    End If

                    If (String.IsNullOrEmpty(sEnumStructTargetName)) Then
                        Throw New ArgumentException("Invalid enum struct field/method name")
                    End If

                    Dim iEnumStructScopeIndex As Integer = CInt(lTmpAutoList(i).m_Data("@EnumStructScopeIndex"))
                    Dim sEnumStructScopeFile As String = CStr(lTmpAutoList(i).m_Data("@EnumStructScopeFile"))

                    Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(lTmpAutoList(i).m_Info,
                                                                                      lTmpAutoList(i).m_Filename,
                                                                                      lTmpAutoList(i).m_Path,
                                                                                      lTmpAutoList(i).m_Type,
                                                                                      sEnumStructTargetName,
                                                                                      String.Format("this.{0}", sEnumStructTargetName),
                                                                                      lTmpAutoList(i).m_FullFunctionString)

                    For Each mData In lTmpAutoList(i).m_Data
                        mAutocomplete.m_Data(mData.Key) = mData.Value
                    Next

                    mAutocomplete.m_Data("IsThis") = True

#If DEBUG Then
                    mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Generates combined enum struct 'this' keywords"
#End If

                    If (Not lTmpAutoList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString AndAlso
                                                                                                            CInt(x.m_Data("@EnumStructScopeIndex")) = iEnumStructScopeIndex AndAlso
                                                                                                            CStr(x.m_Data("@EnumStructScopeFile")) = sEnumStructScopeFile) AndAlso
                                Not lTmpAutoAddList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString AndAlso
                                                                                                                        CInt(x.m_Data("@EnumStructScopeIndex")) = iEnumStructScopeIndex AndAlso
                                                                                                                        CStr(x.m_Data("@EnumStructScopeFile")) = sEnumStructScopeFile)) Then
                        lTmpAutoAddList.Add(mAutocomplete)
                    End If
                Next

                lTmpAutoList.AddRange(lTmpAutoAddList.ToArray)
            End Sub
        End Class

        Private Class ClassVarSyntaxPre
            Public g_ClassParse As ClassParser

            Sub New(_ClassParse As ClassParser)
                g_ClassParse = _ClassParse
            End Sub

            ''' <summary>
            ''' Parse variables.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseVariables(mFormMain As FormMain, mParseInfo As STRUC_VARIABLE_PARSE_PRE_INFO)
                If (True) Then
                    Dim sRegExTypePattern As String = g_ClassParse.GetTypeNamesToPattern(mParseInfo.lOldVarAutocompleteList)

                    Dim sStatementsVar As String = "\b(for)\b"
                    Dim sInitTypesPattern As String = "\b(new|decl|static|const)\b" '"public" is already taken care off
                    Dim sOldStyleVarPattern As String = String.Format("(?<Init>{0}\s+)(?<IsConst>\b(const)\b\s+){2}((?<Tag>{1})\:\s*(?<Var>\b[a-zA-Z0-9_]+\b)|(?<Var>\b[a-zA-Z0-9_]+\b))\s*((?<End>$)|(?<IsFunc>\()|(?<More>\W))", sInitTypesPattern, sRegExTypePattern, "{0,1}")
                    Dim sNewStyleVarPattern As String = String.Format("(?<IsConst>\b(const)\b\s+){1}(?<Tag>{0})\s+(?<Var>\b[a-zA-Z0-9_]+\b)\s*((?<End>$)|(?<IsFunc>\()|(?<More>\W))", sRegExTypePattern, "{0,1}")

                    Dim mSourceList As New List(Of String) From {
                        mParseInfo.sSource
                    }

                    'Get variables from statements
                    If (True) Then
                        Dim mStatementMatches = ClassRegexLock.Matches(mParseInfo.sSource, String.Format("(?<Name>{0})\s*(?<Start>\()", sStatementsVar))
                        Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(mParseInfo.sSource, mParseInfo.iLanguage)
                        Dim mSourceBuilder As StringBuilder

                        For Each mMatch As Match In mStatementMatches
                            mSourceBuilder = New StringBuilder

                            Dim iStartIndex As Integer = mMatch.Groups("Start").Index
                            Dim iStartLevel As Integer = mSourceAnalysis.GetParenthesisLevel(iStartIndex, Nothing)
                            Dim bFinished As Boolean = False

                            For i = iStartIndex To mParseInfo.sSource.Length - 1
                                If (mSourceAnalysis.m_InNonCode(i)) Then
                                    Continue For
                                End If

                                Dim iParentRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                                Dim iParentLevel = mSourceAnalysis.GetParenthesisLevel(i, iParentRange)

                                If (iParentLevel < iStartLevel) Then
                                    bFinished = True
                                    Exit For
                                End If

                                If (iParentLevel = iStartLevel) Then
                                    If (iParentRange <> ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.NONE) Then
                                        Continue For
                                    End If
                                End If

                                If (iParentLevel > iStartLevel) Then
                                    Select Case (iParentRange)
                                        Case ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.START
                                            mSourceBuilder.Append("(")

                                        Case ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END
                                            mSourceBuilder.Append(")")
                                    End Select

                                    Continue For
                                End If

                                mSourceBuilder.Append(mParseInfo.sSource(i))
                            Next

                            If (bFinished) Then
                                If (mSourceBuilder.Length > 0) Then
                                    mSourceList.Add(mSourceBuilder.ToString)
                                End If
                            End If
                        Next
                    End If

                    Dim lCommaLinesList As New List(Of String)

                    For j = 0 To mSourceList.Count - 1
                        Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(mSourceList(j), mParseInfo.iLanguage)
                        Dim mCodeBuilder As New StringBuilder

                        'This is the easiest and yet stupiest idea to resolve multi-decls i've ever come up with...
                        For i = 0 To mSourceList(j).Length - 1
                            If (i <> mSourceList(j).Length - 1) Then
                                If (mSourceAnalysis.m_InNonCode(i)) Then
                                    Continue For
                                End If

                                If (mSourceAnalysis.GetBracketLevel(i, Nothing) > 0) Then
                                    Continue For
                                End If

                                Dim iParentRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                                If (mSourceAnalysis.GetParenthesisLevel(i, iParentRange) > 0) Then
                                    Select Case (iParentRange)
                                        Case ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.START
                                            mCodeBuilder.Append("(")

                                        Case ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END
                                            mCodeBuilder.Append(")")
                                    End Select

                                    Continue For
                                End If
                            End If

                            If (mSourceList(j)(i) = ","c OrElse mSourceList(j)(i) = "="c OrElse i = mSourceList(j).Length - 1) Then
                                Dim sLine As String = mCodeBuilder.ToString.Trim

                                If (Not String.IsNullOrEmpty(sLine)) Then
                                    lCommaLinesList.Add(sLine)
                                End If

                                mCodeBuilder = New StringBuilder

                                'To make sure the assignment doesnt get parsed as variable
                                If (mSourceList(j)(i) = "="c) Then
                                    mCodeBuilder.Append(mSourceList(j)(i))
                                End If

                                Continue For
                            End If

                            mCodeBuilder.Append(mSourceList(j)(i))
                        Next
                    Next

                    '0: Not yet found
                    '1: Old style
                    '2: New style
                    Dim iLastInitStyle As Byte = 0
                    Dim iLastInitIndex As Integer = 0
                    Dim sLastTag As String = Nothing

                    For Each sLine As String In lCommaLinesList
                        Select Case (iLastInitStyle)
                            Case 1 'Old Style
                                If (ClassSettings.g_iSettingsEnforceSyntax <> ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX AndAlso ClassSettings.g_iSettingsEnforceSyntax <> ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_6) Then
                                    Exit Select
                                End If

                                Dim mMatch As Match = ClassRegexLock.Match(sLine, String.Format("^((?<Tag>{0})\:\s*)*(?<Var>\b[a-zA-Z_][a-zA-Z0-9_]*\b)\s*((?<End>$)|(?<IsFunc>\()|(?<IsMethodmap>\.)|(?<IsTag>\:)|(?<More>\W))", sRegExTypePattern))
                                Dim sTag As String = mMatch.Groups("Tag").Value.Trim
                                Dim sVar As String = mMatch.Groups("Var").Value.Trim

                                If (mMatch.Groups("IsFunc").Success OrElse mMatch.Groups("IsMethodmap").Success OrElse mMatch.Groups("IsTag").Success OrElse Not mMatch.Groups("Var").Success) Then
                                    Exit Select
                                End If

                                If (ClassSyntaxTools.ClassSyntaxHelpers.IsForbiddenVariableName(sVar)) Then
                                    Exit Select
                                End If

                                If (ClassRegexLock.IsMatch(sVar, sRegExTypePattern)) Then
                                    Exit Select
                                End If

                                If (mParseInfo.lOldVarAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE)
                                                                                  If ((x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FIELD) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PREPROCESSOR) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.OPERATOR) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.COMMAND) <> 0) Then
                                                                                      Return False
                                                                                  End If

                                                                                  If (Not x.m_FunctionString.Contains(sVar)) Then
                                                                                      Return False
                                                                                  End If

                                                                                  Return ClassRegexLock.IsMatch(x.m_FunctionString, String.Format("\b{0}\b", Regex.Escape(sVar)))
                                                                              End Function)) Then
                                    Exit Select
                                End If

                                If (String.IsNullOrEmpty(sTag)) Then
                                    sTag = "int"
                                End If

                                Dim sTmpFile As String = IO.Path.GetFileName(mParseInfo.sFile)
                                Dim mItem As ClassSyntaxTools.STRUC_AUTOCOMPLETE = mParseInfo.lNewVarAutocompleteList.Find(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Filename.ToLower = sTmpFile.ToLower AndAlso x.m_FunctionString = sVar AndAlso (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0)
                                If (mItem Is Nothing) Then
                                    Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                                IO.Path.GetFileName(mParseInfo.sFile),
                                                                                                mParseInfo.sFile,
                                                                                                ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE,
                                                                                                sVar,
                                                                                                sVar,
                                                                                                String.Format("{0} > {1}", sVar, sTag))

                                    mAutocomplete.m_Data("VariableName") = sVar
                                    mAutocomplete.m_Data("VariableTags") = New String() {sTag}

#If DEBUG Then
                                    mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Parse variables"
#End If

                                    mParseInfo.lNewVarAutocompleteList.Add(mAutocomplete)
                                Else
                                    If (Not ClassRegexLock.IsMatch(mItem.m_FullFunctionString, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                        mItem.m_FullFunctionString = String.Format("{0}|{1}", mItem.m_FullFunctionString, sTag)
                                    End If

                                    If (mItem.m_Data.ContainsKey("VariableTags")) Then
                                        Dim sItemTags As String() = CType(mItem.m_Data("VariableTags"), String())

                                        If (Array.IndexOf(sItemTags, sTag) = -1) Then
                                            With New List(Of String)
                                                .AddRange(sItemTags)
                                                .Add(sTag)
                                                mItem.m_Data("VariableTags") = .ToArray
                                            End With
                                        End If
                                    Else
                                        Throw New ArgumentException("VariableTags invalid")
                                    End If
                                End If

                            Case 2 'New Style
                                If (ClassSettings.g_iSettingsEnforceSyntax <> ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX AndAlso ClassSettings.g_iSettingsEnforceSyntax <> ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                                    Exit Select
                                End If

                                Dim mMatch As Match = ClassRegexLock.Match(sLine, String.Format("^(?<Tag>{0}\s+)*(?<Var>\b[a-zA-Z_][a-zA-Z0-9_]*\b)\s*((?<End>$)|(?<IsFunc>\()|(?<IsMethodmap>\.)|(?<IsTag>\:)|(?<More>\W))", sRegExTypePattern))
                                Dim sTag As String = mMatch.Groups("Tag").Value.Trim
                                Dim sVar As String = mMatch.Groups("Var").Value.Trim

                                If (mMatch.Groups("IsFunc").Success OrElse mMatch.Groups("IsMethodmap").Success OrElse mMatch.Groups("IsTag").Success OrElse Not mMatch.Groups("Var").Success) Then
                                    Exit Select
                                End If

                                If (ClassSyntaxTools.ClassSyntaxHelpers.IsForbiddenVariableName(sVar)) Then
                                    Exit Select
                                End If

                                If (ClassRegexLock.IsMatch(sVar, sRegExTypePattern)) Then
                                    Exit Select
                                End If

                                If (mParseInfo.lOldVarAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE)
                                                                                  If ((x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FIELD) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PREPROCESSOR) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.OPERATOR) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.COMMAND) <> 0) Then
                                                                                      Return False
                                                                                  End If

                                                                                  If (Not x.m_FunctionString.Contains(sVar)) Then
                                                                                      Return False
                                                                                  End If

                                                                                  Return ClassRegexLock.IsMatch(x.m_FunctionString, String.Format("\b{0}\b", Regex.Escape(sVar)))
                                                                              End Function)) Then
                                    Exit Select
                                End If

                                If (String.IsNullOrEmpty(sTag)) Then
                                    If (String.IsNullOrEmpty(sLastTag)) Then
                                        sTag = "int"
                                    Else
                                        sTag = sLastTag
                                    End If
                                End If

                                Dim sTmpFile As String = IO.Path.GetFileName(mParseInfo.sFile)
                                Dim mItem As ClassSyntaxTools.STRUC_AUTOCOMPLETE = mParseInfo.lNewVarAutocompleteList.Find(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Filename.ToLower = sTmpFile.ToLower AndAlso x.m_FunctionString = sVar AndAlso (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0)
                                If (mItem Is Nothing) Then
                                    Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                                IO.Path.GetFileName(mParseInfo.sFile),
                                                                                                mParseInfo.sFile,
                                                                                                ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE,
                                                                                                sVar,
                                                                                                sVar,
                                                                                                String.Format("{0} > {1}", sVar, sTag))

                                    mAutocomplete.m_Data("VariableName") = sVar
                                    mAutocomplete.m_Data("VariableTags") = New String() {sTag}

#If DEBUG Then
                                    mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Parse variables"
#End If

                                    mParseInfo.lNewVarAutocompleteList.Add(mAutocomplete)
                                Else
                                    If (Not ClassRegexLock.IsMatch(mItem.m_FullFunctionString, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                        mItem.m_FullFunctionString = String.Format("{0}|{1}", mItem.m_FullFunctionString, sTag)
                                    End If

                                    If (mItem.m_Data.ContainsKey("VariableTags")) Then
                                        Dim sItemTags As String() = CType(mItem.m_Data("VariableTags"), String())

                                        If (Array.IndexOf(sItemTags, sTag) = -1) Then
                                            With New List(Of String)
                                                .AddRange(sItemTags)
                                                .Add(sTag)
                                                mItem.m_Data("VariableTags") = .ToArray
                                            End With
                                        End If
                                    Else
                                        Throw New ArgumentException("VariableTags invalid")
                                    End If
                                End If

                        End Select

                        iLastInitIndex = 0
                        If (sLine.Contains(";"c)) Then
                            iLastInitStyle = 0
                        End If


                        If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_6) Then
                            For Each mMatch As Match In ClassRegexLock.Matches(sLine, sOldStyleVarPattern)
                                Dim iIndex As Integer = mMatch.Groups("Var").Index
                                Dim sInit As String = mMatch.Groups("Init").Value.Trim
                                Dim sTag As String = mMatch.Groups("Tag").Value.Trim
                                Dim sVar As String = mMatch.Groups("Var").Value.Trim
                                Dim bIsConst As Boolean = mMatch.Groups("IsConst").Success
                                Dim bIsFunc As Boolean = mMatch.Groups("IsFunc").Success

                                If (iLastInitIndex < iIndex) Then
                                    iLastInitIndex = iIndex
                                    iLastInitStyle = 1 'Old style
                                End If

                                sLastTag = Nothing

                                If (bIsFunc) Then
                                    Continue For
                                End If

                                If (ClassSyntaxTools.ClassSyntaxHelpers.IsForbiddenVariableName(sVar)) Then
                                    Continue For
                                End If

                                If (ClassRegexLock.IsMatch(sVar, sRegExTypePattern)) Then
                                    Continue For
                                End If

                                If (mParseInfo.lOldVarAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE)
                                                                                  If ((x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FIELD) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PREPROCESSOR) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.OPERATOR) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.COMMAND) <> 0) Then
                                                                                      Return False
                                                                                  End If

                                                                                  If (Not x.m_FunctionString.Contains(sVar)) Then
                                                                                      Return False
                                                                                  End If

                                                                                  Return ClassRegexLock.IsMatch(x.m_FunctionString, String.Format("\b{0}\b", Regex.Escape(sVar)))
                                                                              End Function)) Then
                                    Continue For
                                End If

                                If (String.IsNullOrEmpty(sTag)) Then
                                    sTag = "int"
                                End If

                                Dim sTmpFile As String = IO.Path.GetFileName(mParseInfo.sFile)
                                Dim mItem As ClassSyntaxTools.STRUC_AUTOCOMPLETE = mParseInfo.lNewVarAutocompleteList.Find(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Filename.ToLower = sTmpFile.ToLower AndAlso x.m_FunctionString = sVar AndAlso (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0)
                                If (mItem Is Nothing) Then
                                    Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                                IO.Path.GetFileName(mParseInfo.sFile),
                                                                                                mParseInfo.sFile,
                                                                                                ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE,
                                                                                                sVar,
                                                                                                sVar,
                                                                                                String.Format("{0} > {1}", sVar, sTag)) 'String.Format("{0} > {1}", sVar, If(bIsConst, "const:", "") & sTag)

                                    mAutocomplete.m_Data("VariableName") = sVar
                                    mAutocomplete.m_Data("VariableTags") = New String() {sTag}

#If DEBUG Then
                                    mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Parse variables"
#End If

                                    mParseInfo.lNewVarAutocompleteList.Add(mAutocomplete)
                                Else
                                    If (Not ClassRegexLock.IsMatch(mItem.m_FullFunctionString, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                        mItem.m_FullFunctionString = String.Format("{0}|{1}", mItem.m_FullFunctionString, sTag)
                                    End If
                                    'If (bIsConst AndAlso Not ClassRegexLock.IsMatch(mItem.m_FullFunctionName, String.Format("\b{0}\b", "const"))) Then
                                    '    mItem.m_FullFunctionName = String.Format("{0}:{1}", "const", mItem.m_FullFunctionName)
                                    'End If

                                    If (mItem.m_Data.ContainsKey("VariableTags")) Then
                                        Dim sItemTags As String() = CType(mItem.m_Data("VariableTags"), String())

                                        If (Array.IndexOf(sItemTags, sTag) = -1) Then
                                            With New List(Of String)
                                                .AddRange(sItemTags)
                                                .Add(sTag)
                                                mItem.m_Data("VariableTags") = .ToArray
                                            End With
                                        End If
                                    Else
                                        Throw New ArgumentException("VariableTags invalid")
                                    End If
                                End If
                            Next
                        End If

                        If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                            For Each mMatch As Match In ClassRegexLock.Matches(sLine, sNewStyleVarPattern)
                                Dim iIndex As Integer = mMatch.Groups("Var").Index
                                Dim sInit As String = mMatch.Groups("Init").Value.Trim
                                Dim sTag As String = mMatch.Groups("Tag").Value.Trim
                                Dim sVar As String = mMatch.Groups("Var").Value.Trim
                                Dim bIsConst As Boolean = mMatch.Groups("IsConst").Success
                                Dim bIsFunc As Boolean = mMatch.Groups("IsFunc").Success

                                If (iLastInitIndex < iIndex) Then
                                    iLastInitIndex = iIndex
                                    iLastInitStyle = 2 'New style
                                End If

                                sLastTag = Nothing

                                If (bIsFunc) Then
                                    Continue For
                                End If

                                If (String.IsNullOrEmpty(sTag)) Then
                                    Continue For
                                End If

                                If (ClassSyntaxTools.ClassSyntaxHelpers.IsForbiddenVariableName(sVar)) Then
                                    Continue For
                                End If

                                If (ClassRegexLock.IsMatch(sVar, sRegExTypePattern)) Then
                                    Continue For
                                End If

                                If (mParseInfo.lOldVarAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE)
                                                                                  If ((x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FIELD) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PREPROCESSOR) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.OPERATOR) <> 0 OrElse
                                                                                            (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.COMMAND) <> 0) Then
                                                                                      Return False
                                                                                  End If

                                                                                  If (Not x.m_FunctionString.Contains(sVar)) Then
                                                                                      Return False
                                                                                  End If

                                                                                  Return ClassRegexLock.IsMatch(x.m_FunctionString, String.Format("\b{0}\b", Regex.Escape(sVar)))
                                                                              End Function)) Then
                                    Continue For
                                End If

                                sLastTag = sTag

                                Dim sTmpFile As String = IO.Path.GetFileName(mParseInfo.sFile)
                                Dim mItem As ClassSyntaxTools.STRUC_AUTOCOMPLETE = mParseInfo.lNewVarAutocompleteList.Find(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Filename.ToLower = sTmpFile.ToLower AndAlso x.m_FunctionString = sVar AndAlso (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0)
                                If (mItem Is Nothing) Then
                                    Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                                IO.Path.GetFileName(mParseInfo.sFile),
                                                                                                mParseInfo.sFile,
                                                                                                ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE,
                                                                                                sVar,
                                                                                                sVar,
                                                                                                String.Format("{0} > {1}", sVar, sTag)) 'String.Format("{0} > {1}", sVar, If(bIsConst, "const:", "") & sTag)

                                    mAutocomplete.m_Data("VariableName") = sVar
                                    mAutocomplete.m_Data("VariableTags") = New String() {sTag}

#If DEBUG Then
                                    mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Parse variables"
#End If

                                    mParseInfo.lNewVarAutocompleteList.Add(mAutocomplete)
                                Else
                                    If (Not ClassRegexLock.IsMatch(mItem.m_FullFunctionString, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                        mItem.m_FullFunctionString = String.Format("{0}|{1}", mItem.m_FullFunctionString, sTag)
                                    End If
                                    'If (bIsConst AndAlso Not ClassRegexLock.IsMatch(mItem.m_FullFunctionName, String.Format("\b{0}\b", "const"))) Then
                                    '    mItem.m_FullFunctionName = String.Format("{0}:{1}", "const", mItem.m_FullFunctionName)
                                    'End If

                                    If (mItem.m_Data.ContainsKey("VariableTags")) Then
                                        Dim sItemTags As String() = CType(mItem.m_Data("VariableTags"), String())

                                        If (Array.IndexOf(sItemTags, sTag) = -1) Then
                                            With New List(Of String)
                                                .AddRange(sItemTags)
                                                .Add(sTag)
                                                mItem.m_Data("VariableTags") = .ToArray
                                            End With
                                        End If
                                    Else
                                        Throw New ArgumentException("VariableTags invalid")
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            End Sub
        End Class

        Private Class ClassVarSyntaxPost
            Public g_ClassParse As ClassParser

            Sub New(_ClassParse As ClassParser)
                g_ClassParse = _ClassParse
            End Sub


        End Class

        Private Class ClassVarSyntaxFinalize
            Public g_ClassParse As ClassParser

            Private Structure STRUC_PARSE_ARGUMENT_ITEM
                Dim sArgument As String
                Dim sFile As String
            End Structure

            Sub New(_ClassParse As ClassParser)
                g_ClassParse = _ClassParse
            End Sub

            ''' <summary>
            ''' Parse function argument variables.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub ParseFunctionArguments(mFormMain As FormMain, mParseInfo As STRUC_VARIABLE_PARSE_POST_INFO)
                Dim sRegExTypePattern As String = g_ClassParse.GetTypeNamesToPattern(mParseInfo.lOldVarAutocompleteList)

                Dim lArgList As New List(Of STRUC_PARSE_ARGUMENT_ITEM)
                Dim mCodeBuilder As New StringBuilder

                For Each mItem In mParseInfo.lOldVarAutocompleteList
                    If ((mItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0) Then
                        Continue For
                    End If

                    Select Case (ClassSettings.g_iSettingsAutocompleteVarParseType)
                        Case ClassSettings.ENUM_VAR_PARSE_TYPE.TAB_AND_INC
                            Dim bValid As Boolean = False

                            If (String.IsNullOrEmpty(mParseInfo.sActiveSourceFile) OrElse mParseInfo.sActiveSourceFile.ToLower = mItem.m_Path.ToLower) Then
                                bValid = True
                            End If

                            Select Case (IO.Path.GetExtension(mItem.m_Path).ToLower)
                                Case ".sp", ".sma", ".p", ".pwn"
                                    bValid = True
                            End Select

                            If (Not bValid) Then
                                Continue For
                            End If

                        Case ClassSettings.ENUM_VAR_PARSE_TYPE.TAB
                            If (Not String.IsNullOrEmpty(mParseInfo.sActiveSourceFile) AndAlso mParseInfo.sActiveSourceFile.ToLower <> mItem.m_Path.ToLower) Then
                                Continue For
                            End If
                    End Select


                    Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(mItem.m_FullFunctionString, mParseInfo.iLanguage)
                    mCodeBuilder = New StringBuilder

                    Dim bNeedSave As Boolean = False
                    'This is the easiest and yet stupiest idea to resolve multi-decls i've ever come up with...
                    For i = 0 To mItem.m_FullFunctionString.Length - 1
                        If (bNeedSave OrElse i = mItem.m_FullFunctionString.Length - 1) Then
                            bNeedSave = False

                            Dim sLine As String = mCodeBuilder.ToString.Trim
                            sLine = ClassRegexLock.Replace(sLine, "(\=)+(.*$)", "")
                            sLine = ClassRegexLock.Replace(sLine, Regex.Escape("&"), "")
                            sLine = ClassRegexLock.Replace(sLine, Regex.Escape("@"), "")

                            If (Not String.IsNullOrEmpty(sLine)) Then
                                lArgList.Add(New STRUC_PARSE_ARGUMENT_ITEM With {.sArgument = sLine, .sFile = mItem.m_Filename})
                            End If

                            mCodeBuilder = New StringBuilder
                        End If

                        If (mSourceAnalysis.m_InNonCode(i)) Then
                            Continue For
                        End If

                        If (mSourceAnalysis.GetBracketLevel(i, Nothing) > 0 OrElse
                                    mSourceAnalysis.GetBraceLevel(i, Nothing) > 0) Then
                            Continue For
                        End If

                        Dim iParentRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                        If (mSourceAnalysis.GetParenthesisLevel(i, iParentRange) < 1 OrElse
                                    iParentRange <> ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.NONE) Then
                            Continue For
                        End If

                        If (mItem.m_FullFunctionString(i) = ","c) Then
                            bNeedSave = True
                            Continue For
                        Else
                            mCodeBuilder.Append(mItem.m_FullFunctionString(i))
                        End If
                    Next
                Next

                For Each mArg As STRUC_PARSE_ARGUMENT_ITEM In lArgList
                    'Old style
                    If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_6) Then
                        Dim mMatch As Match = ClassRegexLock.Match(mArg.sArgument, String.Format("(?<OneSevenTag>\b{0}\b\s+)*((?<Tag>\b{1}\b)\:\s*)*(?<Var>\b[a-zA-Z_][a-zA-Z0-9_]*\b)$", sRegExTypePattern, sRegExTypePattern))
                        Dim sTag As String = If(String.IsNullOrEmpty(mMatch.Groups("Tag").Value), "int", mMatch.Groups("Tag").Value).Trim
                        Dim sVar As String = mMatch.Groups("Var").Value.Trim
                        Dim bIsOneSeven As Boolean = mMatch.Groups("OneSevenTag").Success

                        If (Not bIsOneSeven AndAlso mMatch.Success AndAlso mMatch.Groups("Var").Success AndAlso Not ClassSyntaxTools.ClassSyntaxHelpers.IsForbiddenVariableName(sVar)) Then
                            Dim sTmpFile As String = IO.Path.GetFileName(mArg.sFile)
                            Dim mItem As ClassSyntaxTools.STRUC_AUTOCOMPLETE = mParseInfo.lNewVarAutocompleteList.Find(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Filename.ToLower = sTmpFile.ToLower AndAlso x.m_FunctionString = sVar AndAlso (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0)
                            If (mItem Is Nothing) Then
                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                            IO.Path.GetFileName(mArg.sFile),
                                                                                            mArg.sFile,
                                                                                            ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE,
                                                                                            sVar,
                                                                                            sVar,
                                                                                            String.Format("{0} > {1}", sVar, sTag))

                                mAutocomplete.m_Data("VariableName") = sVar
                                mAutocomplete.m_Data("VariableTags") = New String() {sTag}

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Parse function argument variables"
#End If

                                mParseInfo.lNewVarAutocompleteList.Add(mAutocomplete)
                            Else
                                If (Not ClassRegexLock.IsMatch(mItem.m_FullFunctionString, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                    mItem.m_FullFunctionString = String.Format("{0}|{1}", mItem.m_FullFunctionString, sTag)
                                End If

                                If (mItem.m_Data.ContainsKey("VariableTags")) Then
                                    Dim sItemTags As String() = CType(mItem.m_Data("VariableTags"), String())

                                    If (Array.IndexOf(sItemTags, sTag) = -1) Then
                                        With New List(Of String)
                                            .AddRange(sItemTags)
                                            .Add(sTag)
                                            mItem.m_Data("VariableTags") = .ToArray
                                        End With
                                    End If
                                Else
                                    Throw New ArgumentException("VariableTags invalid")
                                End If
                            End If
                        End If
                    End If

                    'New style
                    If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                        Dim mMatch As Match = ClassRegexLock.Match(mArg.sArgument, String.Format("(?<Tag>\b{0}\b)\s+(?<Var>\b[a-zA-Z_][a-zA-Z0-9_]*\b)$", sRegExTypePattern))
                        Dim sTag As String = If(String.IsNullOrEmpty(mMatch.Groups("Tag").Value), "int", mMatch.Groups("Tag").Value).Trim
                        Dim sVar As String = mMatch.Groups("Var").Value.Trim

                        If (mMatch.Success AndAlso mMatch.Groups("Var").Success AndAlso Not ClassSyntaxTools.ClassSyntaxHelpers.IsForbiddenVariableName(sVar)) Then
                            Dim sTmpFile As String = IO.Path.GetFileName(mArg.sFile)
                            Dim mItem As ClassSyntaxTools.STRUC_AUTOCOMPLETE = mParseInfo.lNewVarAutocompleteList.Find(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Filename.ToLower = sTmpFile.ToLower AndAlso x.m_FunctionString = sVar AndAlso (x.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0)
                            If (mItem Is Nothing) Then
                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE("",
                                                                                            IO.Path.GetFileName(mArg.sFile),
                                                                                            mArg.sFile,
                                                                                            ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE,
                                                                                            sVar,
                                                                                            sVar,
                                                                                            String.Format("{0} > {1}", sVar, sTag))

                                mAutocomplete.m_Data("VariableName") = sVar
                                mAutocomplete.m_Data("VariableTags") = New String() {sTag}

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Parse function argument variables"
#End If

                                mParseInfo.lNewVarAutocompleteList.Add(mAutocomplete)
                            Else
                                If (Not ClassRegexLock.IsMatch(mItem.m_FullFunctionString, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                    mItem.m_FullFunctionString = String.Format("{0}|{1}", mItem.m_FullFunctionString, sTag)
                                End If

                                If (mItem.m_Data.ContainsKey("VariableTags")) Then
                                    Dim sItemTags As String() = CType(mItem.m_Data("VariableTags"), String())

                                    If (Array.IndexOf(sItemTags, sTag) = -1) Then
                                        With New List(Of String)
                                            .AddRange(sItemTags)
                                            .Add(sTag)
                                            mItem.m_Data("VariableTags") = .ToArray
                                        End With
                                    End If
                                Else
                                    Throw New ArgumentException("VariableTags invalid")
                                End If
                            End If
                        End If
                    End If
                Next
            End Sub

            ''' <summary>
            ''' Generates combined methodmap variables.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub GenerateMethodmapVariables(mFormMain As FormMain, mParseInfo As STRUC_VARIABLE_PARSE_POST_INFO)
                If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                    Dim lVarMethodmapList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

                    For Each mVariableItem In mParseInfo.lNewVarAutocompleteList
                        If ((mVariableItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = 0) Then
                            Continue For
                        End If

                        Dim sVariableName As String = CStr(mVariableItem.m_Data("VariableName"))
                        Dim sVariableTags As String() = CType(mVariableItem.m_Data("VariableTags"), String())
                        If (String.IsNullOrEmpty(sVariableName)) Then
                            Continue For
                        End If

                        Dim lSkipTags As New List(Of String)
                        Dim lTargetTags As New Stack(Of String)
                        For Each sTag In sVariableTags
                            lTargetTags.Push(sTag)
                        Next

                        While (lTargetTags.Count <> 0)
                            Dim sTargetTag As String = lTargetTags.Pop

                            If (lSkipTags.Contains(sTargetTag)) Then
                                Continue While
                            End If

                            For Each mMethodmapItem In mParseInfo.lOldVarAutocompleteList
                                If ((mMethodmapItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = 0 OrElse
                                        Not mMethodmapItem.m_FunctionString.Contains("."c)) Then
                                    Continue For
                                End If

                                'TODO: Dont use yet, make methodmap parsing more efficent first
                                'If ((mMethodmapItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STATIC) <> 0) Then
                                '    Continue For
                                'End If

                                Dim sMethodmapName As String = CStr(mMethodmapItem.m_Data("MethodmapName"))
                                Dim sMethodmapMethodName As String = CStr(mMethodmapItem.m_Data("MethodmapMethodName"))
                                Dim sMethodmapParentName As String = CStr(mMethodmapItem.m_Data("MethodmapParentName"))
                                If (String.IsNullOrEmpty(sMethodmapName) OrElse String.IsNullOrEmpty(sMethodmapMethodName)) Then
                                    Continue For
                                End If

                                If (sTargetTag <> sMethodmapName) Then
                                    Continue For
                                End If

                                If (Not String.IsNullOrEmpty(sMethodmapParentName)) Then
                                    lTargetTags.Push(sMethodmapParentName)
                                    lSkipTags.Add(sTargetTag)
                                End If

                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mMethodmapItem.m_Info,
                                                                                        mVariableItem.m_Filename,
                                                                                        mVariableItem.m_Path,
                                                                                        (ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE Or mMethodmapItem.m_Type) And Not ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP,
                                                                                        sMethodmapMethodName,
                                                                                        String.Format("{0}.{1}", sVariableName, sMethodmapMethodName),
                                                                                        mMethodmapItem.m_FullFunctionString)

                                mAutocomplete.m_Data("VariableName") = sVariableName
                                mAutocomplete.m_Data("VariableTags") = sVariableTags

                                For Each mData In mMethodmapItem.m_Data
                                    mAutocomplete.m_Data(mData.Key) = mData.Value
                                Next

                                'Keep root information
                                For Each mData In mVariableItem.m_Data
                                    If (mData.Key.StartsWith("@"c)) Then
                                        mAutocomplete.m_Data(mData.Key) = mData.Value
                                    End If
                                Next

                                mAutocomplete.m_Data("VariableMethodmapName") = sVariableName
                                mAutocomplete.m_Data("VariableMethodmapMethod") = sMethodmapMethodName

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Make methodmaps using variables"
#End If

                                If (Not mParseInfo.lNewVarAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString AndAlso
                                                                                      CStr(x.m_Data("MethodmapName")) = sMethodmapName AndAlso
                                                                                      CStr(x.m_Data("MethodmapMethodName")) = sMethodmapMethodName) AndAlso
                                        Not lVarMethodmapList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString AndAlso
                                                                                      CStr(x.m_Data("MethodmapName")) = sMethodmapName AndAlso
                                                                                      CStr(x.m_Data("MethodmapMethodName")) = sMethodmapMethodName)) Then
                                    lVarMethodmapList.Add(mAutocomplete)
                                End If
                            Next
                        End While
                    Next

                    mParseInfo.lNewVarAutocompleteList.AddRange(lVarMethodmapList.ToArray)
                End If
            End Sub

            ''' <summary>
            ''' Generates combined methodmap methods.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub GenerateMethodmapMethods(mFormMain As FormMain, mParseInfo As STRUC_VARIABLE_PARSE_POST_INFO)
                If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                    Dim lVarMethodmapList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

                    For Each mMethodItem In mParseInfo.lOldVarAutocompleteList
                        If ((mMethodItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0) Then
                            Continue For
                        End If

                        If ((mMethodItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHOD) = 0) Then
                            Continue For
                        End If

                        Dim sMethodName As String = CStr(mMethodItem.m_Data("MethodName"))
                        Dim sMethodTag As String = CStr(mMethodItem.m_Data("MethodTag"))
                        If (String.IsNullOrEmpty(sMethodName)) Then
                            Continue For
                        End If

                        Dim lSkipTags As New List(Of String)
                        Dim lTargetTags As New Stack(Of String)
                        lTargetTags.Push(sMethodTag)

                        While (lTargetTags.Count <> 0)
                            Dim sTargetTag As String = lTargetTags.Pop

                            If (lSkipTags.Contains(sTargetTag)) Then
                                Continue While
                            End If

                            For Each mMethodmapItem In mParseInfo.lOldVarAutocompleteList
                                If ((mMethodmapItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = 0 OrElse
                                            Not mMethodmapItem.m_FunctionString.Contains("."c)) Then
                                    Continue For
                                End If

                                'TODO: Dont use yet, make methodmap parsing more efficent first
                                'If ((mMethodmapItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STATIC) <> 0) Then
                                '    Continue For
                                'End If

                                Dim sMethodmapName As String = CStr(mMethodmapItem.m_Data("MethodmapName"))
                                Dim sMethodmapMethodName As String = CStr(mMethodmapItem.m_Data("MethodmapMethodName"))
                                Dim sMethodmapParentName As String = CStr(mMethodmapItem.m_Data("MethodmapParentName"))
                                If (String.IsNullOrEmpty(sMethodmapName) OrElse String.IsNullOrEmpty(sMethodmapMethodName)) Then
                                    Continue For
                                End If

                                If (sTargetTag <> sMethodmapName) Then
                                    Continue For
                                End If

                                If (Not String.IsNullOrEmpty(sMethodmapParentName)) Then
                                    lTargetTags.Push(sMethodmapParentName)
                                    lSkipTags.Add(sTargetTag)
                                End If

                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mMethodmapItem.m_Info,
                                                                                            mMethodItem.m_Filename,
                                                                                            mMethodItem.m_Path,
                                                                                            (ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE Or mMethodmapItem.m_Type) And Not ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP,
                                                                                            sMethodmapMethodName,
                                                                                            String.Format("{0}.{1}", sMethodName, sMethodmapMethodName),
                                                                                            mMethodmapItem.m_FullFunctionString)

                                mAutocomplete.m_Data("VariableName") = sMethodName
                                mAutocomplete.m_Data("VariableTags") = New String() {sMethodTag}

                                For Each mData In mMethodmapItem.m_Data
                                    mAutocomplete.m_Data(mData.Key) = mData.Value
                                Next

                                'Keep root information
                                For Each mData In mMethodItem.m_Data
                                    If (mData.Key.StartsWith("@"c)) Then
                                        mAutocomplete.m_Data(mData.Key) = mData.Value
                                    End If
                                Next

                                mAutocomplete.m_Data("VariableMethodmapName") = sMethodName
                                mAutocomplete.m_Data("VariableMethodmapMethod") = sMethodmapMethodName

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Make methodmaps using methods"
#End If

                                If (Not mParseInfo.lNewVarAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString) AndAlso
                                            Not lVarMethodmapList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                    lVarMethodmapList.Add(mAutocomplete)
                                End If
                            Next
                        End While
                    Next

                    mParseInfo.lNewVarAutocompleteList.AddRange(lVarMethodmapList.ToArray)
                End If
            End Sub

            ''' <summary>
            ''' Generates combined methodmap methods.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub GenerateMethodmapInlineMethods(mFormMain As FormMain, mParseInfo As STRUC_VARIABLE_PARSE_POST_INFO)
                If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                    Dim lVarMethodmapList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

                    For Each mInlineMethodItem In mParseInfo.lOldVarAutocompleteList
                        If ((mInlineMethodItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0) Then
                            Continue For
                        End If

                        'Dont add our own inline methods again.
                        If ((mInlineMethodItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> 0) Then
                            Continue For
                        End If

                        If ((mInlineMethodItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.INLINE_METHOD) = 0) Then
                            Continue For
                        End If

                        Dim sInlineMethodName As String = CStr(mInlineMethodItem.m_Data("InlineMethodName"))
                        Dim sInlineMethodTag As String = CStr(mInlineMethodItem.m_Data("InlineMethodTag"))
                        If (String.IsNullOrEmpty(sInlineMethodName)) Then
                            Continue For
                        End If

                        Dim lSkipTags As New List(Of String)
                        Dim lTargetTags As New Stack(Of String)
                        lTargetTags.Push(sInlineMethodTag)

                        While (lTargetTags.Count <> 0)
                            Dim sTargetTag As String = lTargetTags.Pop

                            If (lSkipTags.Contains(sTargetTag)) Then
                                Continue While
                            End If

                            For Each mMethodmapItem In mParseInfo.lOldVarAutocompleteList
                                If ((mMethodmapItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = 0 OrElse
                                            Not mMethodmapItem.m_FunctionString.Contains("."c)) Then
                                    Continue For
                                End If

                                'TODO: Dont use yet, make methodmap parsing more efficent first
                                'If ((mMethodmapItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STATIC) <> 0) Then
                                '    Continue For
                                'End If

                                Dim sMethodmapName As String = CStr(mMethodmapItem.m_Data("MethodmapName"))
                                Dim sMethodmapMethodName As String = CStr(mMethodmapItem.m_Data("MethodmapMethodName"))
                                Dim sMethodmapParentName As String = CStr(mMethodmapItem.m_Data("MethodmapParentName"))
                                If (String.IsNullOrEmpty(sMethodmapName) OrElse String.IsNullOrEmpty(sMethodmapMethodName)) Then
                                    Continue For
                                End If

                                If (sTargetTag <> sMethodmapName) Then
                                    Continue For
                                End If

                                If (Not String.IsNullOrEmpty(sMethodmapParentName)) Then
                                    lTargetTags.Push(sMethodmapParentName)
                                    lSkipTags.Add(sTargetTag)
                                End If

                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mMethodmapItem.m_Info,
                                                                                            mInlineMethodItem.m_Filename,
                                                                                            mInlineMethodItem.m_Path,
                                                                                            (ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE Or mMethodmapItem.m_Type) And Not ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP,
                                                                                            sMethodmapMethodName,
                                                                                            String.Format("{0}.{1}", sInlineMethodName, sMethodmapMethodName),
                                                                                            mMethodmapItem.m_FullFunctionString)

                                mAutocomplete.m_Data("VariableName") = sInlineMethodName
                                mAutocomplete.m_Data("VariableTags") = New String() {sInlineMethodTag}

                                For Each mData In mMethodmapItem.m_Data
                                    mAutocomplete.m_Data(mData.Key) = mData.Value
                                Next

                                'Keep root information
                                For Each mData In mInlineMethodItem.m_Data
                                    If (mData.Key.StartsWith("@"c)) Then
                                        mAutocomplete.m_Data(mData.Key) = mData.Value
                                    End If
                                Next

                                mAutocomplete.m_Data("VariableMethodmapName") = sInlineMethodName
                                mAutocomplete.m_Data("VariableMethodmapMethod") = sMethodmapMethodName

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Make methodmaps using inline-methods"
#End If

                                If (Not mParseInfo.lNewVarAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString) AndAlso
                                            Not lVarMethodmapList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                    lVarMethodmapList.Add(mAutocomplete)
                                End If
                            Next
                        End While
                    Next

                    mParseInfo.lNewVarAutocompleteList.AddRange(lVarMethodmapList.ToArray)
                End If
            End Sub

            ''' <summary>
            ''' Generates combined methodmap methods.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub GenerateMethodmapFields(mFormMain As FormMain, mParseInfo As STRUC_VARIABLE_PARSE_POST_INFO)
                If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                    Dim lVarMethodmapList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

                    For Each mFieldItem In mParseInfo.lOldVarAutocompleteList
                        If ((mFieldItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0) Then
                            Continue For
                        End If

                        If ((mFieldItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FIELD) = 0) Then
                            Continue For
                        End If

                        Dim sFieldName As String = CStr(mFieldItem.m_Data("EnumStructFieldName"))
                        Dim sFieldTag As String = CStr(mFieldItem.m_Data("EnumStructFieldTag"))
                        If (String.IsNullOrEmpty(sFieldName)) Then
                            Continue For
                        End If

                        Dim lSkipTags As New List(Of String)
                        Dim lTargetTags As New Stack(Of String)
                        lTargetTags.Push(sFieldTag)

                        While (lTargetTags.Count <> 0)
                            Dim sTargetTag As String = lTargetTags.Pop

                            If (lSkipTags.Contains(sTargetTag)) Then
                                Continue While
                            End If

                            For Each mMethodmapItem In mParseInfo.lOldVarAutocompleteList
                                If ((mMethodmapItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = 0 OrElse
                                            Not mMethodmapItem.m_FunctionString.Contains("."c)) Then
                                    Continue For
                                End If

                                'TODO: Dont use yet, make methodmap parsing more efficent first
                                'If ((mMethodmapItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STATIC) <> 0) Then
                                '    Continue For
                                'End If

                                Dim sMethodmapName As String = CStr(mMethodmapItem.m_Data("MethodmapName"))
                                Dim sMethodmapMethodName As String = CStr(mMethodmapItem.m_Data("MethodmapMethodName"))
                                Dim sMethodmapParentName As String = CStr(mMethodmapItem.m_Data("MethodmapParentName"))
                                If (String.IsNullOrEmpty(sMethodmapName) OrElse String.IsNullOrEmpty(sMethodmapMethodName)) Then
                                    Continue For
                                End If

                                If (sTargetTag <> sMethodmapName) Then
                                    Continue For
                                End If

                                If (Not String.IsNullOrEmpty(sMethodmapParentName)) Then
                                    lTargetTags.Push(sMethodmapParentName)
                                    lSkipTags.Add(sTargetTag)
                                End If

                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mMethodmapItem.m_Info,
                                                                                            mFieldItem.m_Filename,
                                                                                            mFieldItem.m_Path,
                                                                                            (ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE Or mMethodmapItem.m_Type) And Not ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP,
                                                                                            sMethodmapMethodName,
                                                                                            String.Format("{0}.{1}", sFieldName, sMethodmapMethodName),
                                                                                            mMethodmapItem.m_FullFunctionString)

                                mAutocomplete.m_Data("VariableName") = sFieldName
                                mAutocomplete.m_Data("VariableTags") = New String() {sFieldTag}

                                For Each mData In mMethodmapItem.m_Data
                                    mAutocomplete.m_Data(mData.Key) = mData.Value
                                Next

                                'Keep root information
                                For Each mData In mFieldItem.m_Data
                                    If (mData.Key.StartsWith("@"c)) Then
                                        mAutocomplete.m_Data(mData.Key) = mData.Value
                                    End If
                                Next

                                mAutocomplete.m_Data("VariableMethodmapName") = sFieldName
                                mAutocomplete.m_Data("VariableMethodmapMethod") = sMethodmapMethodName

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Make methodmaps using fields"
#End If

                                If (Not mParseInfo.lNewVarAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString) AndAlso
                                            Not lVarMethodmapList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                    lVarMethodmapList.Add(mAutocomplete)
                                End If
                            Next
                        End While
                    Next

                    mParseInfo.lNewVarAutocompleteList.AddRange(lVarMethodmapList.ToArray)
                End If
            End Sub


            ''' <summary>
            ''' Generates combined methodmap variables.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub GenerateEnumStructVariables(mFormMain As FormMain, mParseInfo As STRUC_VARIABLE_PARSE_POST_INFO)
                If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                    Dim lVarEnumStructList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

                    For Each mVariableItem In mParseInfo.lNewVarAutocompleteList
                        If ((mVariableItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = 0) Then
                            Continue For
                        End If

                        Dim sVariableName As String = CStr(mVariableItem.m_Data("VariableName"))
                        Dim sVariableTags As String() = CType(mVariableItem.m_Data("VariableTags"), String())
                        If (String.IsNullOrEmpty(sVariableName)) Then
                            Continue For
                        End If

                        Dim lSkipTags As New List(Of String)
                        Dim lTargetTags As New Stack(Of String)
                        For Each sTag In sVariableTags
                            lTargetTags.Push(sTag)
                        Next

                        While (lTargetTags.Count <> 0)
                            Dim sTargetTag As String = lTargetTags.Pop

                            If (lSkipTags.Contains(sTargetTag)) Then
                                Continue While
                            End If

                            For Each mEnumStructItem In mParseInfo.lOldVarAutocompleteList
                                If ((mEnumStructItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT) = 0 OrElse
                                        Not mEnumStructItem.m_FunctionString.Contains("."c)) Then
                                    Continue For
                                End If

                                'TODO: Dont use yet, make enum struct parsing more efficent first
                                'If ((mEnumStructItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STATIC) <> 0) Then
                                '    Continue For
                                'End If

                                Dim sEnumStructName As String = CStr(mEnumStructItem.m_Data("EnumStructName"))
                                Dim sEnumStructFieldName As String = CStr(mEnumStructItem.m_Data("EnumStructFieldName"))
                                Dim sEnumStructMethodName As String = CStr(mEnumStructItem.m_Data("EnumStructMethodName"))
                                Dim sEnumStructTargetName As String
                                Dim bIsField As Boolean = False

                                If ((mEnumStructItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FIELD) <> 0) Then
                                    sEnumStructTargetName = sEnumStructFieldName
                                    bIsField = True
                                Else
                                    sEnumStructTargetName = sEnumStructMethodName
                                    bIsField = False
                                End If

                                If (String.IsNullOrEmpty(sEnumStructTargetName)) Then
                                    Throw New ArgumentException("Invalid enum struct field/method name")
                                End If

                                If (String.IsNullOrEmpty(sEnumStructName) OrElse String.IsNullOrEmpty(sEnumStructTargetName)) Then
                                    Continue For
                                End If

                                If (sTargetTag <> sEnumStructName) Then
                                    Continue For
                                End If

                                lSkipTags.Add(sTargetTag)

                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mEnumStructItem.m_Info,
                                                                                        mVariableItem.m_Filename,
                                                                                        mVariableItem.m_Path,
                                                                                        (ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE Or mEnumStructItem.m_Type) And Not ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT,
                                                                                        sEnumStructTargetName,
                                                                                        String.Format("{0}.{1}", sVariableName, sEnumStructTargetName),
                                                                                        mEnumStructItem.m_FullFunctionString)

                                mAutocomplete.m_Data("VariableName") = sVariableName
                                mAutocomplete.m_Data("VariableTags") = sVariableTags

                                For Each mData In mEnumStructItem.m_Data
                                    mAutocomplete.m_Data(mData.Key) = mData.Value
                                Next

                                'Keep root information
                                For Each mData In mVariableItem.m_Data
                                    If (mData.Key.StartsWith("@"c)) Then
                                        mAutocomplete.m_Data(mData.Key) = mData.Value
                                    End If
                                Next

                                mAutocomplete.m_Data("VariableEnumStructName") = sVariableName
                                mAutocomplete.m_Data("VariableEnumStructField") = If(bIsField, sEnumStructTargetName, "")
                                mAutocomplete.m_Data("VariableEnumStructMethod") = If(bIsField, "", sEnumStructTargetName)

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Make enum structs using variables"
#End If

                                If (Not mParseInfo.lNewVarAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString AndAlso
                                                                                      CStr(x.m_Data("EnumStructName")) = sEnumStructName AndAlso
                                                                                      CStr(x.m_Data("EnumStructFieldName")) = sEnumStructFieldName AndAlso
                                                                                      CStr(x.m_Data("EnumStructMethodName")) = sEnumStructMethodName) AndAlso
                                        Not lVarEnumStructList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString AndAlso
                                                                                      CStr(x.m_Data("EnumStructName")) = sEnumStructName AndAlso
                                                                                      CStr(x.m_Data("EnumStructFieldName")) = sEnumStructFieldName AndAlso
                                                                                      CStr(x.m_Data("EnumStructMethodName")) = sEnumStructMethodName)) Then
                                    lVarEnumStructList.Add(mAutocomplete)
                                End If
                            Next
                        End While
                    Next

                    mParseInfo.lNewVarAutocompleteList.AddRange(lVarEnumStructList.ToArray)
                End If
            End Sub

            ''' <summary>
            ''' Generates combined enum struct methods.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub GenerateEnumStructMethods(mFormMain As FormMain, mParseInfo As STRUC_VARIABLE_PARSE_POST_INFO)
                If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                    Dim lVarEnumStructList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

                    For Each mMethodItem In mParseInfo.lOldVarAutocompleteList
                        If ((mMethodItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0) Then
                            Continue For
                        End If

                        If ((mMethodItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHOD) = 0) Then
                            Continue For
                        End If

                        Dim sMethodName As String = CStr(mMethodItem.m_Data("MethodName"))
                        Dim sMethodTag As String = CStr(mMethodItem.m_Data("MethodTag"))
                        If (String.IsNullOrEmpty(sMethodName)) Then
                            Continue For
                        End If

                        Dim lSkipTags As New List(Of String)
                        Dim lTargetTags As New Stack(Of String)
                        lTargetTags.Push(sMethodTag)

                        While (lTargetTags.Count <> 0)
                            Dim sTargetTag As String = lTargetTags.Pop

                            If (lSkipTags.Contains(sTargetTag)) Then
                                Continue While
                            End If

                            For Each mEnumStructItem In mParseInfo.lOldVarAutocompleteList
                                If ((mEnumStructItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT) = 0 OrElse
                                            Not mEnumStructItem.m_FunctionString.Contains("."c)) Then
                                    Continue For
                                End If

                                'TODO: Dont use yet, make methodmap parsing more efficent first
                                'If ((mMethodmapItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STATIC) <> 0) Then
                                '    Continue For
                                'End If

                                Dim sEnumStructName As String = CStr(mEnumStructItem.m_Data("EnumStructName"))
                                Dim sEnumStructFieldName As String = CStr(mEnumStructItem.m_Data("EnumStructFieldName"))
                                Dim sEnumStructMethodName As String = CStr(mEnumStructItem.m_Data("EnumStructMethodName"))
                                Dim sEnumStructTargetName As String
                                Dim bIsField As Boolean = False

                                If ((mEnumStructItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FIELD) <> 0) Then
                                    sEnumStructTargetName = sEnumStructFieldName
                                    bIsField = True
                                Else
                                    sEnumStructTargetName = sEnumStructMethodName
                                    bIsField = False
                                End If

                                If (String.IsNullOrEmpty(sEnumStructTargetName)) Then
                                    Throw New ArgumentException("Invalid enum struct field/method name")
                                End If

                                If (String.IsNullOrEmpty(sEnumStructName) OrElse String.IsNullOrEmpty(sEnumStructTargetName)) Then
                                    Continue For
                                End If

                                If (sTargetTag <> sEnumStructName) Then
                                    Continue For
                                End If

                                lSkipTags.Add(sTargetTag)

                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mEnumStructItem.m_Info,
                                                                                            mMethodItem.m_Filename,
                                                                                            mMethodItem.m_Path,
                                                                                            (ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE Or mEnumStructItem.m_Type) And Not ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP,
                                                                                            sEnumStructTargetName,
                                                                                            String.Format("{0}.{1}", sMethodName, sEnumStructTargetName),
                                                                                            mEnumStructItem.m_FullFunctionString)

                                mAutocomplete.m_Data("VariableName") = sMethodName
                                mAutocomplete.m_Data("VariableTags") = New String() {sMethodTag}

                                For Each mData In mEnumStructItem.m_Data
                                    mAutocomplete.m_Data(mData.Key) = mData.Value
                                Next

                                'Keep root information
                                For Each mData In mMethodItem.m_Data
                                    If (mData.Key.StartsWith("@"c)) Then
                                        mAutocomplete.m_Data(mData.Key) = mData.Value
                                    End If
                                Next

                                mAutocomplete.m_Data("VariableEnumStructName") = sMethodName
                                mAutocomplete.m_Data("VariableEnumStructField") = If(bIsField, sEnumStructTargetName, "")
                                mAutocomplete.m_Data("VariableEnumStructMethod") = If(bIsField, "", sEnumStructTargetName)

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Make enum structs using methods"
#End If

                                If (Not mParseInfo.lNewVarAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString) AndAlso
                                            Not lVarEnumStructList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                    lVarEnumStructList.Add(mAutocomplete)
                                End If
                            Next
                        End While
                    Next

                    mParseInfo.lNewVarAutocompleteList.AddRange(lVarEnumStructList.ToArray)
                End If
            End Sub

            ''' <summary>
            ''' Generates combined methodmap methods.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub GenerateEnumStructInlineMethods(mFormMain As FormMain, mParseInfo As STRUC_VARIABLE_PARSE_POST_INFO)
                If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                    Dim lVarMethodmapList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

                    For Each mInlineMethodItem In mParseInfo.lOldVarAutocompleteList
                        If ((mInlineMethodItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0) Then
                            Continue For
                        End If

                        'Dont add our own inline methods again.
                        If ((mInlineMethodItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT) <> 0) Then
                            Continue For
                        End If

                        If ((mInlineMethodItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.INLINE_METHOD) = 0) Then
                            Continue For
                        End If

                        Dim sInlineMethodName As String = CStr(mInlineMethodItem.m_Data("InlineMethodName"))
                        Dim sInlineMethodTag As String = CStr(mInlineMethodItem.m_Data("InlineMethodTag"))
                        If (String.IsNullOrEmpty(sInlineMethodName)) Then
                            Continue For
                        End If

                        Dim lSkipTags As New List(Of String)
                        Dim lTargetTags As New Stack(Of String)
                        lTargetTags.Push(sInlineMethodTag)

                        While (lTargetTags.Count <> 0)
                            Dim sTargetTag As String = lTargetTags.Pop

                            If (lSkipTags.Contains(sTargetTag)) Then
                                Continue While
                            End If

                            For Each mEnumStructItem In mParseInfo.lOldVarAutocompleteList
                                If ((mEnumStructItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT) = 0 OrElse
                                            Not mEnumStructItem.m_FunctionString.Contains("."c)) Then
                                    Continue For
                                End If

                                'TODO: Dont use yet, make methodmap parsing more efficent first
                                'If ((mMethodmapItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STATIC) <> 0) Then
                                '    Continue For
                                'End If

                                Dim sEnumStructName As String = CStr(mEnumStructItem.m_Data("EnumStructName"))
                                Dim sEnumStructFieldName As String = CStr(mEnumStructItem.m_Data("EnumStructFieldName"))
                                Dim sEnumStructMethodName As String = CStr(mEnumStructItem.m_Data("EnumStructMethodName"))
                                Dim sEnumStructTargetName As String
                                Dim bIsField As Boolean = False

                                If ((mEnumStructItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FIELD) <> 0) Then
                                    sEnumStructTargetName = sEnumStructFieldName
                                    bIsField = True
                                Else
                                    sEnumStructTargetName = sEnumStructMethodName
                                    bIsField = False
                                End If

                                If (String.IsNullOrEmpty(sEnumStructTargetName)) Then
                                    Throw New ArgumentException("Invalid enum struct field/method name")
                                End If

                                If (String.IsNullOrEmpty(sEnumStructName) OrElse String.IsNullOrEmpty(sEnumStructTargetName)) Then
                                    Continue For
                                End If

                                If (sTargetTag <> sEnumStructName) Then
                                    Continue For
                                End If

                                lSkipTags.Add(sTargetTag)

                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mEnumStructItem.m_Info,
                                                                                            mInlineMethodItem.m_Filename,
                                                                                            mInlineMethodItem.m_Path,
                                                                                            (ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE Or mEnumStructItem.m_Type) And Not ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT,
                                                                                            sEnumStructTargetName,
                                                                                            String.Format("{0}.{1}", sInlineMethodName, sEnumStructTargetName),
                                                                                            mEnumStructItem.m_FullFunctionString)

                                mAutocomplete.m_Data("VariableName") = sInlineMethodName
                                mAutocomplete.m_Data("VariableTags") = New String() {sInlineMethodTag}

                                For Each mData In mEnumStructItem.m_Data
                                    mAutocomplete.m_Data(mData.Key) = mData.Value
                                Next

                                'Keep root information
                                For Each mData In mInlineMethodItem.m_Data
                                    If (mData.Key.StartsWith("@"c)) Then
                                        mAutocomplete.m_Data(mData.Key) = mData.Value
                                    End If
                                Next

                                mAutocomplete.m_Data("VariableEnumStructName") = sEnumStructTargetName
                                mAutocomplete.m_Data("VariableEnumStructField") = If(bIsField, sEnumStructTargetName, "")
                                mAutocomplete.m_Data("VariableEnumStructMethod") = If(bIsField, "", sEnumStructTargetName)

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Make enum structs using inline-methods"
#End If

                                If (Not mParseInfo.lNewVarAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString) AndAlso
                                            Not lVarMethodmapList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                    lVarMethodmapList.Add(mAutocomplete)
                                End If
                            Next
                        End While
                    Next

                    mParseInfo.lNewVarAutocompleteList.AddRange(lVarMethodmapList.ToArray)
                End If
            End Sub

            ''' <summary>
            ''' Generates combined methodmap methods.
            ''' SourcePawn +1.7 only.
            ''' </summary>
            ''' <param name="mFormMain"></param>
            ''' <param name="mParseInfo"></param>
            Public Sub GenerateEnumStructFields(mFormMain As FormMain, mParseInfo As STRUC_VARIABLE_PARSE_POST_INFO)
                If (ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7) Then
                    Dim lVarMethodmapList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

                    For Each mFieldItem In mParseInfo.lOldVarAutocompleteList
                        If ((mFieldItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0) Then
                            Continue For
                        End If

                        If ((mFieldItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FIELD) = 0) Then
                            Continue For
                        End If

                        Dim sFieldName As String = CStr(mFieldItem.m_Data("EnumStructFieldName"))
                        Dim sFieldTag As String = CStr(mFieldItem.m_Data("EnumStructFieldTag"))
                        If (String.IsNullOrEmpty(sFieldName)) Then
                            Continue For
                        End If

                        Dim lSkipTags As New List(Of String)
                        Dim lTargetTags As New Stack(Of String)
                        lTargetTags.Push(sFieldTag)

                        While (lTargetTags.Count <> 0)
                            Dim sTargetTag As String = lTargetTags.Pop

                            If (lSkipTags.Contains(sTargetTag)) Then
                                Continue While
                            End If

                            For Each mEnumStructItem In mParseInfo.lOldVarAutocompleteList
                                If ((mEnumStructItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT) = 0 OrElse
                                            Not mEnumStructItem.m_FunctionString.Contains("."c)) Then
                                    Continue For
                                End If

                                'TODO: Dont use yet, make methodmap parsing more efficent first
                                'If ((mMethodmapItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STATIC) <> 0) Then
                                '    Continue For
                                'End If

                                Dim sEnumStructName As String = CStr(mEnumStructItem.m_Data("EnumStructName"))
                                Dim sEnumStructFieldName As String = CStr(mEnumStructItem.m_Data("EnumStructFieldName"))
                                Dim sEnumStructMethodName As String = CStr(mEnumStructItem.m_Data("EnumStructMethodName"))
                                Dim sEnumStructTargetName As String
                                Dim bIsField As Boolean = False

                                If ((mEnumStructItem.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FIELD) <> 0) Then
                                    sEnumStructTargetName = sEnumStructFieldName
                                    bIsField = True
                                Else
                                    sEnumStructTargetName = sEnumStructMethodName
                                    bIsField = False
                                End If

                                If (String.IsNullOrEmpty(sEnumStructTargetName)) Then
                                    Throw New ArgumentException("Invalid enum struct field/method name")
                                End If

                                If (sTargetTag <> sEnumStructName) Then
                                    Continue For
                                End If

                                lSkipTags.Add(sTargetTag)

                                Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mEnumStructItem.m_Info,
                                                                                            mFieldItem.m_Filename,
                                                                                            mFieldItem.m_Path,
                                                                                            (ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE Or mEnumStructItem.m_Type) And Not ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT,
                                                                                            sEnumStructTargetName,
                                                                                            String.Format("{0}.{1}", sFieldName, sEnumStructTargetName),
                                                                                            mEnumStructItem.m_FullFunctionString)

                                mAutocomplete.m_Data("VariableName") = sFieldName
                                mAutocomplete.m_Data("VariableTags") = New String() {sFieldTag}

                                For Each mData In mEnumStructItem.m_Data
                                    mAutocomplete.m_Data(mData.Key) = mData.Value
                                Next

                                'Keep root information
                                For Each mData In mFieldItem.m_Data
                                    If (mData.Key.StartsWith("@"c)) Then
                                        mAutocomplete.m_Data(mData.Key) = mData.Value
                                    End If
                                Next

                                mAutocomplete.m_Data("VariableEnumStructName") = sEnumStructTargetName
                                mAutocomplete.m_Data("VariableEnumStructField") = If(bIsField, sEnumStructTargetName, "")
                                mAutocomplete.m_Data("VariableEnumStructMethod") = If(bIsField, "", sEnumStructTargetName)

#If DEBUG Then
                                mAutocomplete.m_Data("DataSet-" & ClassExceptionLog.GetDebugStackTrace("")) = "Make enum structs using fields"
#End If

                                If (Not mParseInfo.lNewVarAutocompleteList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString) AndAlso
                                            Not lVarMethodmapList.Exists(Function(x As ClassSyntaxTools.STRUC_AUTOCOMPLETE) x.m_Type = mAutocomplete.m_Type AndAlso x.m_FunctionString = mAutocomplete.m_FunctionString)) Then
                                    lVarMethodmapList.Add(mAutocomplete)
                                End If
                            Next
                        End While
                    Next

                    mParseInfo.lNewVarAutocompleteList.AddRange(lVarMethodmapList.ToArray)
                End If
            End Sub
        End Class

        Public Sub CleanupFunctionSpacesSource(ByRef sSource As String, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
            Dim lBraceLocList As New List(Of Integer())

            'Fix function spaces 
            Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, iLanguage)
            Dim mSourceBuilder As New StringBuilder(sSource)

            Dim iParentStart As Integer = 0
            Dim iParentLen As Integer = 0
            For i = 0 To mSourceBuilder.Length - 1
                Dim iParentRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                Dim iParentLevel As Integer = mSourceAnalysis.GetParenthesisLevel(i, iParentRange)
                Select Case (iParentRange)
                    Case ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.START
                        If (iParentLevel - 1 = 0) Then
                            iParentStart = i
                        End If

                    Case ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END
                        If (iParentLevel - 1 = 0) Then
                            iParentLen = i - iParentStart + 1
                            lBraceLocList.Add(New Integer() {iParentStart, iParentLen})
                        End If

                End Select
            Next

            For i = lBraceLocList.Count - 1 To 0 Step -1
                iParentStart = lBraceLocList(i)(0)
                iParentLen = lBraceLocList(i)(1)

                Dim sBraceString As String = mSourceBuilder.ToString(iParentStart, iParentLen)
                Dim mRegMatchCol As MatchCollection = ClassRegexLock.Matches(sBraceString, "\s+")

                For ii = mRegMatchCol.Count - 1 To 0 Step -1
                    Dim mMatch As Match = mRegMatchCol(ii)
                    If (Not mMatch.Success) Then
                        Continue For
                    End If

                    Dim iOffset As Integer = iParentStart + mMatch.Index
                    Dim bContinue As Boolean = False
                    If (mSourceAnalysis.m_InNonCode(iOffset)) Then
                        Continue For
                    End If

                    sBraceString = sBraceString.Remove(mMatch.Index, mMatch.Length)
                    sBraceString = sBraceString.Insert(mMatch.Index, " "c)
                Next

                mSourceBuilder = mSourceBuilder.Remove(iParentStart, iParentLen)
                mSourceBuilder = mSourceBuilder.Insert(iParentStart, sBraceString)
            Next

            sSource = mSourceBuilder.ToString
        End Sub

        Public Sub CleanUpNewLinesSource(ByRef sSource As String, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
            'Remove new lines e.g: MyStuff \
            '                      MyStuff2
            sSource = ClassRegexLock.Replace(sSource, "\\\s*\n\s*", "")

            If (True) Then
                'From:
                '   public
                '       static
                '           bla()
                'To:
                '   public static bla()
                Dim sTypes As String() = {"enum", "struct", "funcenum", "functag", "stock", "static", "const", "public", "native", "forward", "typeset", "methodmap", "typedef"}
                Dim mRegMatchCol As MatchCollection = ClassRegexLock.Matches(sSource, String.Format("^\s*(\b{0}\b)(?<Space>\s*\n\s*)", String.Join("\b|\b", sTypes)), RegexOptions.Multiline)
                Dim mSourceBuilder As New StringBuilder(sSource)

                For i = mRegMatchCol.Count - 1 To 0 Step -1
                    Dim mMatch As Match = mRegMatchCol(i)
                    If (Not mMatch.Groups("Space").Success) Then
                        Continue For
                    End If

                    mSourceBuilder = mSourceBuilder.Remove(mMatch.Groups("Space").Index, mMatch.Groups("Space").Length)
                    mSourceBuilder = mSourceBuilder.Insert(mMatch.Groups("Space").Index, " "c)
                Next

                sSource = mSourceBuilder.ToString
            End If

            If (True) Then
                Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, iLanguage)
                Dim mSourceBuilder As New StringBuilder(sSource)

                'Collapse new lines in statements with parenthesis e.g:
                'MyStuff(MyArg1,
                '        MyArg2)
                For i = mSourceBuilder.Length - 1 To 0 Step -1
                    Select Case (mSourceBuilder(i))
                        Case vbLf(0)
                            If (mSourceAnalysis.m_InNonCode(i)) Then
                                Exit Select
                            End If

                            If (mSourceAnalysis.GetParenthesisLevel(i, Nothing) > 0) Then
                                Select Case (True)
                                    Case i > 1 AndAlso mSourceBuilder(i - 1) = vbCr
                                        mSourceBuilder = mSourceBuilder.Remove(i - 1, 2)
                                        mSourceBuilder = mSourceBuilder.Insert(i - 1, New String(" "c, 2))
                                    Case Else
                                        mSourceBuilder = mSourceBuilder.Remove(i, 1)
                                        mSourceBuilder = mSourceBuilder.Insert(i, New String(" "c, 1))
                                End Select
                            End If

                        Case Else
                            'If we collapse statements etc., we need to remove single line comments!
                            'if(
                            '       //This my cause problems!
                            '   )
                            'Results in:
                            'if(//This my cause problems!)
                            'Just remove the comments
                            If (mSourceAnalysis.GetParenthesisLevel(i, Nothing) > 0 AndAlso mSourceAnalysis.m_InSingleComment(i)) Then
                                mSourceBuilder = mSourceBuilder.Remove(i, 1)
                                mSourceBuilder = mSourceBuilder.Insert(i, " "c)
                            End If

                    End Select
                Next

                sSource = mSourceBuilder.ToString
            End If
        End Sub

        Public Function GetTypeNamesToPattern(lTmpAutoList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)) As String
            Return String.Format("(\b{0}\b)", String.Join("\b|\b", GetTypeNames(lTmpAutoList)))
        End Function

        Public Function GetTypeNames(lTmpAutoList As List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)) As String()
            Dim lNames As New List(Of String) From {
                "void", 'For >1.7
                "int",
                "bool",
                "float",
                "char",
                "function",
                "object",
                "null_t",
                "nullfunc_t",
                "__nullable__",
 _
                "_", 'For <1.6
                "any",
                "bool",
                "Float",
                "String",
                "Function"
            }

            lTmpAutoList.ForEach(Sub(j As ClassSyntaxTools.STRUC_AUTOCOMPLETE)
                                     Dim sName As String = Nothing

                                     Select Case (True)
                                         Case ((j.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) <> 0)
                                             sName = CStr(j.m_Data("EnumName"))
                                         Case ((j.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STRUCT) <> 0)
                                             sName = CStr(j.m_Data("StructName"))
                                         Case ((j.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) <> 0)
                                             sName = CStr(j.m_Data("FuncenumName"))
                                         Case ((j.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) <> 0)
                                             sName = CStr(j.m_Data("FunctagName"))
                                         Case ((j.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) <> 0)
                                             sName = CStr(j.m_Data("TypedefName"))
                                         Case ((j.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) <> 0)
                                             sName = CStr(j.m_Data("TypesetName"))
                                         Case Else
                                             Return
                                     End Select

                                     If (String.IsNullOrEmpty(sName)) Then
                                         Return
                                     End If

                                     If (sName.Contains("."c)) Then
                                         Return
                                     End If

                                     If (lNames.Contains(sName)) Then
                                         Return
                                     End If

                                     lNames.Add(sName)
                                 End Sub)

            Return lNames.ToArray
        End Function

        Public Function GetPreprocessorKeywords(mIncludes As KeyValuePair(Of String, String)()) As ClassSyntaxTools.STRUC_AUTOCOMPLETE()
            Dim lTmpAutoList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

            Dim mDic As New Dictionary(Of String, String)
            Dim mDicOp As New Dictionary(Of String, String)
            Dim mDicConst As New Dictionary(Of String, String)

            'Pawn
            mDic("assert") = "#assert"
            mDic("define") = "#define <name>"
            mDic("emit") = "#emit <opcode>"
            mDic("endinput") = "#endinput"
            mDic("endscript") = "#endscript"
            mDic("error") = "#error <message>"
            mDic("warning") = "#warning <message>"
            mDic("file") = "#file <filepath>"
            mDic("line") = "#line <value>"
            mDic("section") = "#section <name>"
            mDic("include") = "#include <filename>"
            mDic("tryinclude") = "#tryinclude <filename>"
            mDic("undef") = "#undef <name>"

            mDic("pragma") = "#pragma <keyvalue> <value>"
            mDic("pragma align") = "#pragma align"
            mDic("pragma amxram") = "#pragma amxram <value>"
            mDic("pragma amxlimit") = "#pragma amxlimit <value>"
            mDic("pragma codepage") = "#pragma codepage <name/value>"
            mDic("pragma compress") = "#pragma compress <1/0>"
            mDic("pragma ctrlchar") = "#pragma ctrlchar <value>"
            mDic("pragma deprecated") = "#pragma deprecated <value>"
            mDic("pragma dynamic") = "#pragma dynamic <value>"
            mDic("pragma pack") = "#pragma pack <1/0>"
            mDic("pragma rational") = "#pragma rational <tagname(value)>"
            mDic("pragma semicolon") = "#pragma semicolon <1/0>"
            mDic("pragma tabsize") = "#pragma tabsize <value>"
            mDic("pragma unused") = "#pragma unused <symbol,...>"

            'AMX Mod X
            mDic("pragma library") = "#pragma library <library>"
            mDic("pragma reqlib") = "#pragma reqlib <library>"
            mDic("pragma loadlib") = "#pragma loadlib <library>"
            mDic("pragma reqclass") = "#pragma reqclass <class>"
            mDic("pragma defclasslib") = "#pragma defclasslib <class> <library>"
            mDic("pragma explib") = "#pragma explib <symbol> <symbol>" 'TODO: Wtf are those and what arguments? Never been used anywhere, only found in the compiler source.
            mDic("pragma expclass") = "#pragma expclass <symbol> <symbol>" 'TODO: Wtf are those and what arguments? Never been used anywhere, only found in the compiler source.

            'SourcePawn
            'NOTE: SourcePawn has alot of removed pragmas
            mDic("pragma newdecls") = "#pragma newdecls <required/optional>"
            mDic("pragma newdecls required") = "#pragma newdecls <required/optional>"
            mDic("pragma newdecls optional") = "#pragma newdecls <required/optional>"

            For i = 0 To mIncludes.Length - 1
                Dim sKey As String = String.Format("include <{0}>", IO.Path.GetFileNameWithoutExtension(mIncludes(i).Value))
                Dim sValue As String = String.Format("#include <{0}>", IO.Path.GetFileNameWithoutExtension(mIncludes(i).Value))
                mDic(sKey) = sValue
            Next

            'Operators
            mDicOp("char") = "<exp> char"
            mDicOp("defined") = "defined <symbol>"
            mDicOp("sizeof") = "sizeof <symbol>"
            mDicOp("state") = "state <symbol>"
            mDicOp("tagof") = "tagof <symbol>"
            mDicOp("cellsof") = "cellsof <symbol>"

            mDicOp("delete") = "delete <symbol>"
            mDicOp("view_as") = "view_as<type>(symbol)"

            'Constants 
            mDicConst("true") = "true"
            mDicConst("false") = "false"
            mDicConst("EOS") = "EOS"
            mDicConst("INVALID_FUNCTION") = "INVALID_FUNCTION"
            mDicConst("cellbits") = "cellbits"
            mDicConst("cellmax") = "cellmax"
            mDicConst("cellmin") = "cellmin"
            mDicConst("charbits") = "charbits"
            mDicConst("charmin") = "charmin"
            mDicConst("charmax") = "charmax"
            mDicConst("ucharmax") = "ucharmax"
            mDicConst("__Pawn") = "__Pawn"
            mDicConst("__LINE__") = "__LINE__"

            For Each mItem In mDic
                lTmpAutoList.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE("", "compiler.exe", "", ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PREPROCESSOR, mItem.Key, mItem.Key, mItem.Value))
            Next

            For Each mItem In mDicOp
                lTmpAutoList.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE("", "compiler.exe", "", ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.OPERATOR, mItem.Key, mItem.Key, mItem.Value))
            Next

            For Each mItem In mDicConst
                lTmpAutoList.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE("", "compiler.exe", "", ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.OPERATOR, mItem.Key, mItem.Key, mItem.Value))
            Next

            Return lTmpAutoList.ToArray
        End Function

        Public Function GetTextEditorCommands() As ClassSyntaxTools.STRUC_AUTOCOMPLETE()
            Dim lTmpAutoList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)

            For Each mItem In ClassTextEditorTools.ClassTestEditorCommands.m_Commands
                lTmpAutoList.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE("", "BasicPawn.exe", "", ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.COMMAND, mItem.m_Command, mItem.m_Command, mItem.m_Command))
            Next

            Return lTmpAutoList.ToArray
        End Function

        ''' <summary>
        ''' Gets config defined constants
        ''' </summary>
        ''' <param name="mConfig"></param>
        ''' <param name="iLanguage">Target language, -1 for all.</param>
        ''' <returns></returns>
        Public Function GetConfigDefines(mConfig As ClassConfigs.STRUC_CONFIG_ITEM, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE) As ClassSyntaxTools.STRUC_AUTOCOMPLETE()
            Dim lTmpAutoList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Dim lDefineList As New List(Of KeyValuePair(Of String, String))

            If (iLanguage = -1 OrElse
                    iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN) Then
                lDefineList.AddRange(mConfig.g_mCompilerOptionsSP.g_mDefineConstants)
            End If

            If (iLanguage = -1 OrElse
                    iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.AMXMODX OrElse
                    iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.PAWN) Then
                lDefineList.AddRange(mConfig.g_mCompilerOptionsAMXX.g_mDefineConstants)
            End If

            For Each mDefine In lDefineList
                lTmpAutoList.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE("", "BasicPawn.exe", "", ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE, mDefine.Key, mDefine.Key, String.Format("const {0}={1}", mDefine.Key, mDefine.Value)))
            Next

            Return lTmpAutoList.ToArray
        End Function

    End Class

    Class ClassRegexLock
        ' Regex is thread-safe and we need to limit how many threads make thread-safe calls simultaneously. Multiple threads waiting for the main thread to finish can cause massive lag and stutter.
        ' Instead of threads waiting for the main thread to finish - thus blocking the main thread - lets wait for each other thread instead and make only one thread wait for the main thread.
        ' While this is slower than without locking, the ui thread will not lag during the process.

        Private Shared ReadOnly _lock As New Object

        Public Shared Function Matches(input As String, pattern As String, Optional options As RegexOptions = RegexOptions.None) As MatchCollection
            SyncLock _lock
                Return Regex.Matches(input, pattern, options)
            End SyncLock
        End Function

        Public Shared Function Replace(input As String, pattern As String, replacement As String, Optional options As RegexOptions = RegexOptions.None) As String
            SyncLock _lock
                Return Regex.Replace(input, pattern, replacement, options)
            End SyncLock
        End Function

        Public Shared Function Split(input As String, pattern As String, Optional options As RegexOptions = RegexOptions.None) As String()
            SyncLock _lock
                Return Regex.Split(input, pattern, options)
            End SyncLock
        End Function

        Public Shared Function Match(input As String, pattern As String, Optional options As RegexOptions = RegexOptions.None) As Match
            SyncLock _lock
                Return Regex.Match(input, pattern, options)
            End SyncLock
        End Function

        Public Shared Function IsMatch(input As String, pattern As String, Optional options As RegexOptions = RegexOptions.None) As Boolean
            SyncLock _lock
                Return Regex.IsMatch(input, pattern, options)
            End SyncLock
        End Function
    End Class
End Class
