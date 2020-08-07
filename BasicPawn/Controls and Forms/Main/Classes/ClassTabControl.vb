'BasicPawn
'Copyright(C) 2020 TheTimocop

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


Imports System.Text.RegularExpressions
Imports ICSharpCode.TextEditor
Imports ICSharpCode.TextEditor.Document

Public Class ClassTabControl
    Implements IDisposable

    Private g_mFormMain As FormMain
    Private g_bIgnoreOnTabSelected As Boolean = False
    Private g_bDidInit As Boolean = False

    Private g_iControlDrawCoutner As Integer = 0

    Private WithEvents g_mTimer As Timer
    Private g_sSelectTabDelayIdentifier As String = ""

    Private g_iBeginUpdateCount As Integer = 0
    Private g_bBeginRequestSyntaxUpdate As Boolean = False
    Private g_bBeginRequestFullUpdate As Boolean = False
    Private g_lBeginRequestFullUpdateTabs As New List(Of ClassTab)

    Private Shared g_sFoldingsConfig As String = IO.Path.Combine(Application.StartupPath, "foldings.ini")

    Public Const DEFAULT_SELECT_TAB_DELAY = 100

    Public Event OnTabAdded(mTab As ClassTab)
    Public Event OnTabRemoved(mTab As ClassTab)
    Public Event OnTabOpen(mTab As ClassTab, sPath As String)
    Public Event OnTabSaved(mTab As ClassTab, sOldPath As String, sNewPath As String)
    Public Event OnTabFullUpdate(mTab As ClassTab())

    Public Event OnTextEditorTabDetailsAction(mTab As ClassTab, iDetailsTabIndex As Integer, bIsSpecialAction As Boolean, iKeys As Keys)
    Public Sub __OnTextEditorTabDetailsAction(mTab As ClassTab, iDetailsTabIndex As Integer, bIsSpecialAction As Boolean, iKeys As Keys)
        RaiseEvent OnTextEditorTabDetailsAction(mTab, iDetailsTabIndex, bIsSpecialAction, iKeys)
    End Sub

    Public Event OnTextEditorTabDetailsMove(mTab As ClassTab, iDetailsTabIndex As Integer, iDirection As Integer, iKeys As Keys)
    Public Sub __OnTextEditorTabDetailsMove(mTab As ClassTab, iDetailsTabIndex As Integer, iDirection As Integer, iKeys As Keys)
        RaiseEvent OnTextEditorTabDetailsMove(mTab, iDetailsTabIndex, iDirection, iKeys)
    End Sub

    Public Sub New(f As FormMain)
        g_mFormMain = f

        AddHandler g_mFormMain.TabControl_SourceTabs.SelectedIndexChanged, AddressOf OnTabSelected

        g_mTimer = New Timer
    End Sub

    Public Sub Init()
        If (g_bDidInit) Then
            Return
        End If

        g_bDidInit = True

        g_mFormMain.TabControl_SourceTabs.TabPages.Clear()
        AddTab(True)

        AddHandler g_mFormMain.g_ClassSyntaxParser.OnSyntaxParseSuccess, AddressOf OnTabSyntaxParseSuccess
    End Sub

    ReadOnly Property m_ActiveTab As ClassTab
        Get
            Return TryCast(g_mFormMain.TabControl_SourceTabs.SelectedTab, ClassTab)
        End Get
    End Property

    ReadOnly Property m_ActiveTabIndex As Integer
        Get
            Return g_mFormMain.TabControl_SourceTabs.SelectedIndex
        End Get
    End Property

    ReadOnly Property m_Tab(iIndex As Integer) As ClassTab
        Get
            Return TryCast(g_mFormMain.TabControl_SourceTabs.TabPages(iIndex), ClassTab)
        End Get
    End Property

    ReadOnly Property m_TabsCount As Integer
        Get
            Return g_mFormMain.TabControl_SourceTabs.TabCount
        End Get
    End Property

    ReadOnly Property m_IsInitalized As Boolean
        Get
            Return g_bDidInit
        End Get
    End Property

    Public Function GetAllTabs() As ClassTab()
        Dim lTabs As New List(Of ClassTab)

        For i = 0 To m_TabsCount - 1
            lTabs.Add(m_Tab(i))
        Next

        Return lTabs.ToArray
    End Function

    Public Function AddTab(Optional bSelect As Boolean = False, Optional bShowTemplateWizard As Boolean = False, Optional bChanged As Boolean = False, Optional bDontRecycleTabs As Boolean = False) As ClassTab
        Dim sTemplateSource As String = ""

        If (bShowTemplateWizard) Then
            Using i As New FormNewWizard()
                If (i.ShowDialog = DialogResult.OK) Then
                    sTemplateSource = i.m_PreviewTemplateSource
                End If
            End Using
        End If

        Try
            BeginUpdate()

            Dim mRecycleTab As ClassTab = Nothing

            'Recycle first unsaved tab
            If (Not bDontRecycleTabs AndAlso m_TabsCount = 1) Then
                If (m_Tab(0).m_IsUnsaved AndAlso Not m_Tab(0).m_Changed) Then
                    mRecycleTab = m_Tab(0)
                End If
            End If

            Dim mTabPage As New ClassTab(g_mFormMain) With {
                .m_Changed = False
            }

            While True
                For Each mConfig In ClassConfigs.GetConfigs()
                    If (mConfig.g_bAutoload) Then
                        mTabPage.m_ActiveConfig = mConfig
                        Exit While
                    End If
                Next

                mTabPage.m_ActiveConfig = Nothing
                Exit While
            End While

            If (bShowTemplateWizard) Then
                mTabPage.m_TextEditor.Document.TextContent = sTemplateSource
            End If

            If (mRecycleTab Is Nothing) Then
                g_mFormMain.TabControl_SourceTabs.TabPages.Add(mTabPage)
            Else
                g_mFormMain.TabControl_SourceTabs.TabPages.Remove(mRecycleTab)
                g_mFormMain.TabControl_SourceTabs.TabPages.Insert(0, mTabPage)
            End If

            If (bSelect OrElse m_TabsCount < 2) Then
                mTabPage.SelectTab()
            End If

            If (g_iBeginUpdateCount > 0) Then
                g_bBeginRequestSyntaxUpdate = True
            Else
                g_mFormMain.g_ClassSyntaxTools.g_ClassSyntaxHighlighting.UpdateTextEditorSyntax()
            End If

            If (m_TabsCount > 1 AndAlso g_mFormMain.g_mUCStartPage.Visible) Then
                g_mFormMain.g_mUCStartPage.Hide()
            End If

            RaiseEvent OnTabAdded(mTabPage)

            Return mTabPage
        Finally
            EndUpdate()
        End Try
    End Function

    Public Function RemoveTab(iIndex As Integer, bPrompSave As Boolean, Optional iSelectTabIndex As Integer = -1) As Boolean
        Try
            BeginUpdate()

            If (bPrompSave AndAlso Not PromptSaveTab(iIndex)) Then
                Return False
            End If

            RaiseEvent OnTabRemoved(m_Tab(iIndex))

            Try
                'We do not want to call SelectTab() when diposing tabs. We will do it afterwards aynways.
                g_bIgnoreOnTabSelected = True

                Dim mTabPage = m_Tab(iIndex)
                mTabPage.Dispose()
                mTabPage = Nothing
            Finally
                g_bIgnoreOnTabSelected = False
            End Try

            If (m_TabsCount < 1) Then
                AddTab()
            End If

            If (iSelectTabIndex > -1) Then
                If (iSelectTabIndex > m_TabsCount - 1) Then
                    SelectTab(m_TabsCount - 1)
                Else
                    SelectTab(iSelectTabIndex)
                End If
            Else
                If (iIndex > m_TabsCount - 1) Then
                    SelectTab(m_TabsCount - 1)
                Else
                    SelectTab(iIndex)
                End If
            End If

            If (m_TabsCount > 1 AndAlso g_mFormMain.g_mUCStartPage.Visible) Then
                g_mFormMain.g_mUCStartPage.Hide()
            End If

            Return True
        Finally
            EndUpdate()
        End Try
    End Function

    Public Function RemoveTabGotoLast(iIndex As Integer, bPrompSave As Boolean) As Boolean
        Dim mLastTab = GetNextTabByLastSelection(m_Tab(iIndex), -1)

        If (mLastTab Is Nothing) Then
            Return RemoveTab(iIndex, bPrompSave)
        Else
            Return RemoveTab(iIndex, bPrompSave, mLastTab.m_Index)
        End If
    End Function

    Public Sub SwapTabs(iFromIndex As Integer, iToIndex As Integer)
        Try
            BeginUpdate()

            Try
                g_bIgnoreOnTabSelected = True

                Dim mFrom As ClassTab = m_Tab(iFromIndex)
                g_mFormMain.TabControl_SourceTabs.TabPages.Remove(mFrom)
                g_mFormMain.TabControl_SourceTabs.TabPages.Insert(iToIndex, mFrom)
            Finally
                g_bIgnoreOnTabSelected = False
            End Try

            SelectTab(iToIndex)
        Finally
            EndUpdate()
        End Try
    End Sub

    Public Sub SelectTab(iIndex As Integer)
        Try
            BeginUpdate()

            For i = 0 To m_TabsCount - 1
                If (iIndex = i) Then
                    Continue For
                End If

                If (Not m_Tab(i).m_HandlersEnabled) Then
                    Continue For
                End If

                m_Tab(i).m_HandlersEnabled = False
            Next

            If (iIndex > -1) Then
                If (Not m_Tab(iIndex).m_HandlersEnabled) Then
                    m_Tab(iIndex).m_HandlersEnabled = True
                End If

                Try
                    g_bIgnoreOnTabSelected = True
                    g_mFormMain.TabControl_SourceTabs.SelectTab(iIndex)
                Finally
                    g_bIgnoreOnTabSelected = False
                End Try

                If (g_iBeginUpdateCount > 0) Then
                    g_bBeginRequestSyntaxUpdate = True
                Else
                    g_mFormMain.g_ClassSyntaxTools.g_ClassSyntaxHighlighting.UpdateTextEditorSyntax()
                End If

                If (g_iBeginUpdateCount > 0) Then
                    g_bBeginRequestFullUpdate = True
                    g_lBeginRequestFullUpdateTabs.Add(m_Tab(iIndex))
                Else
                    FullUpdate(m_Tab(iIndex))
                End If

                m_Tab(iIndex).UpdateLastViewTime()
            End If
        Finally
            EndUpdate()
        End Try
    End Sub

    Public Sub SelectTab(sIdentifier As String, iDelay As Integer)
        g_mTimer.Stop()

        If (GetTabIndexByIdentifier(sIdentifier) < 0) Then
            g_sSelectTabDelayIdentifier = ""
            Return
        End If

        g_sSelectTabDelayIdentifier = sIdentifier
        g_mTimer.Interval = iDelay
        g_mTimer.Start()
    End Sub

    Public Function GetTabByIdentifier(sIdentifier As String) As ClassTab
        If (String.IsNullOrEmpty(sIdentifier)) Then
            Return Nothing
        End If

        For i = 0 To m_TabsCount - 1
            If (m_Tab(i).m_Identifier = sIdentifier) Then
                Return m_Tab(i)
            End If
        Next

        Return Nothing
    End Function

    Public Function GetTabIndexByIdentifier(sIdentifier As String) As Integer
        If (String.IsNullOrEmpty(sIdentifier)) Then
            Return -1
        End If

        For i = 0 To m_TabsCount - 1
            If (m_Tab(i).m_Identifier = sIdentifier) Then
                Return i
            End If
        Next

        Return -1
    End Function

    Public Function GetTabByFile(sFile As String) As ClassTab
        If (String.IsNullOrEmpty(sFile)) Then
            Return Nothing
        End If

        For i = 0 To m_TabsCount - 1
            If (Not m_Tab(i).m_IsUnsaved AndAlso m_Tab(i).m_File.ToLower = sFile.ToLower) Then
                Return m_Tab(i)
            End If
        Next

        Return Nothing
    End Function

    Private Sub OnSwitchTabDelay(sender As Object, e As EventArgs) Handles g_mTimer.Tick
        g_mTimer.Stop()

        If (String.IsNullOrEmpty(g_sSelectTabDelayIdentifier)) Then
            Return
        End If

        Dim iIndex As Integer = GetTabIndexByIdentifier(g_sSelectTabDelayIdentifier)
        g_sSelectTabDelayIdentifier = ""

        If (iIndex < 0) Then
            Return
        End If

        SelectTab(iIndex)
    End Sub

    Public Sub RenewTab(iIndex As Integer, Optional bIgnoreSavePrompt As Boolean = False)
        OpenFileTab(iIndex, Nothing, bIgnoreSavePrompt)
    End Sub

    ''' <summary>
    ''' Opens a new source file
    ''' </summary>
    ''' <param name="iIndex"></param>
    ''' <param name="sFile"></param>
    ''' <param name="bIgnoreSavePrompt">If true, the new file will be opened without prompting to save the changed source</param>
    ''' <returns></returns>
    Public Function OpenFileTab(iIndex As Integer, sFile As String, Optional bIgnoreSavePrompt As Boolean = False, Optional bKeepView As Boolean = True) As Boolean
        If (Not bIgnoreSavePrompt AndAlso Not PromptSaveTab(iIndex)) Then
            Return False
        End If

        If (String.IsNullOrEmpty(sFile) OrElse Not IO.File.Exists(sFile)) Then
            m_Tab(iIndex).g_ClassLineState.BeginIgnore()

            m_Tab(iIndex).m_FileCachedWriteDate = Date.MaxValue
            m_Tab(iIndex).m_File = ""
            m_Tab(iIndex).m_TextEditor.Document.TextContent = ""

            m_Tab(iIndex).m_Changed = False
            m_Tab(iIndex).m_AutocompleteGroup.m_AutocompleteItems.Clear()
            m_Tab(iIndex).m_IncludesGroup.m_IncludeFiles.Clear()
            m_Tab(iIndex).m_FileCachedWriteDate = Now

            m_Tab(iIndex).g_ClassFoldings.ClearSavedFoldStates()
            m_Tab(iIndex).g_ClassLineState.ClearStates()

            m_Tab(iIndex).m_ActiveConfig = Nothing

            m_Tab(iIndex).g_ClassLineState.EndIgnore()

            If (g_iBeginUpdateCount > 0) Then
                g_bBeginRequestFullUpdate = True
                g_lBeginRequestFullUpdateTabs.Add(m_Tab(iIndex))
            Else
                FullUpdate(m_Tab(iIndex))
            End If

            If (g_mFormMain.g_mUCStartPage.Visible) Then
                g_mFormMain.g_mUCStartPage.Hide()
            End If

            RaiseEvent OnTabOpen(m_Tab(iIndex), m_Tab(iIndex).m_File)

            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User created a new source file")
            Return False
        End If


        Dim sFileText As String = IO.File.ReadAllText(sFile)

        'Try to keep current view
        Dim iCaretLine = m_Tab(iIndex).m_TextEditor.ActiveTextAreaControl.Caret.Line
        Dim iCaretColumn = m_Tab(iIndex).m_TextEditor.ActiveTextAreaControl.Caret.Column

        m_Tab(iIndex).g_ClassLineState.BeginIgnore()

        m_Tab(iIndex).m_FileCachedWriteDate = Date.MaxValue
        m_Tab(iIndex).m_File = sFile
        m_Tab(iIndex).m_TextEditor.Document.TextContent = sFileText

        m_Tab(iIndex).m_Changed = False
        m_Tab(iIndex).m_AutocompleteGroup.m_AutocompleteItems.Clear()
        m_Tab(iIndex).m_IncludesGroup.m_IncludeFiles.Clear()
        m_Tab(iIndex).m_FileCachedWriteDate = m_Tab(iIndex).m_FileRealWriteDate

        m_Tab(iIndex).g_ClassFoldings.ClearSavedFoldStates()
        m_Tab(iIndex).g_ClassLineState.ClearStates()

        m_Tab(iIndex).m_ActiveConfig = ClassConfigs.FindOptimalConfigForFile(sFile, False, Nothing)

        m_Tab(iIndex).g_ClassLineState.EndIgnore()

        If (g_iBeginUpdateCount > 0) Then
            g_bBeginRequestFullUpdate = True
            g_lBeginRequestFullUpdateTabs.Add(m_Tab(iIndex))
        Else
            FullUpdate(m_Tab(iIndex))
        End If

        If (g_mFormMain.g_mUCStartPage.Visible) Then
            g_mFormMain.g_mUCStartPage.Hide()
        End If

        If (bKeepView) Then
            Dim mCaretPos = m_Tab(iIndex).m_TextEditor.ActiveTextAreaControl.Caret.ValidatePosition(New TextLocation(iCaretColumn, iCaretLine))
            m_Tab(iIndex).m_TextEditor.ActiveTextAreaControl.Caret.Position = mCaretPos
            m_Tab(iIndex).m_TextEditor.ActiveTextAreaControl.Caret.UpdateCaretPosition()

            m_Tab(iIndex).m_TextEditor.ActiveTextAreaControl.CenterViewOn(mCaretPos.Line, 10)
        End If

        RaiseEvent OnTabOpen(m_Tab(iIndex), m_Tab(iIndex).m_File)

        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User opened a new file: " & sFile, New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(sFile))
        Return True
    End Function

    ''' <summary>
    ''' Saves a source file
    ''' </summary>
    ''' <param name="iIndex"></param>
    ''' <param name="bSaveAs">Force to use a new file using SaveFileDialog</param>
    Public Function SaveFileTab(iIndex As Integer, Optional bSaveAs As Boolean = False) As Boolean
        If (bSaveAs OrElse m_Tab(iIndex).m_IsUnsaved OrElse m_Tab(iIndex).m_InvalidFile) Then
            Dim sOldFile As String = If(m_Tab(iIndex).m_IsUnsaved OrElse m_Tab(iIndex).m_InvalidFile, "", m_Tab(iIndex).m_File)

            Using i As New SaveFileDialog
                i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|AMX Mod X|*.sma|Pawn (Not fully supported)|*.pwn;*.p|All files|*.*"

                i.InitialDirectory = If(String.IsNullOrEmpty(m_Tab(iIndex).m_File), "", IO.Path.GetDirectoryName(m_Tab(iIndex).m_File))
                i.FileName = IO.Path.GetFileName(m_Tab(iIndex).m_File)

                If (i.ShowDialog = DialogResult.OK) Then
                    m_Tab(iIndex).m_FileCachedWriteDate = Date.MaxValue
                    m_Tab(iIndex).m_File = i.FileName

                    m_Tab(iIndex).m_Changed = False
                    m_Tab(iIndex).g_ClassLineState.SaveStates()

                    m_Tab(iIndex).m_TextEditor.InvalidateTextArea()

                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User saved file to: " & m_Tab(iIndex).m_File, New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(m_Tab(iIndex).m_File))

                    IO.File.WriteAllText(m_Tab(iIndex).m_File, m_Tab(iIndex).m_TextEditor.Document.TextContent)

                    m_Tab(iIndex).m_FileCachedWriteDate = m_Tab(iIndex).m_FileRealWriteDate

                    g_mFormMain.g_mUCStartPage.g_mClassRecentItems.AddRecent(m_Tab(iIndex).m_File)

                    g_mFormMain.g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL)

                    RaiseEvent OnTabSaved(m_Tab(iIndex), sOldFile, i.FileName)

                    Return True
                Else
                    Return False
                End If
            End Using
        Else
            m_Tab(iIndex).m_FileCachedWriteDate = Date.MaxValue
            m_Tab(iIndex).m_Changed = False
            m_Tab(iIndex).g_ClassLineState.SaveStates()

            m_Tab(iIndex).m_TextEditor.InvalidateTextArea()

            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User saved file to: " & m_Tab(iIndex).m_File, New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(m_Tab(iIndex).m_File))

            IO.File.WriteAllText(m_Tab(iIndex).m_File, m_Tab(iIndex).m_TextEditor.Document.TextContent)

            m_Tab(iIndex).m_FileCachedWriteDate = m_Tab(iIndex).m_FileRealWriteDate

            g_mFormMain.g_mUCStartPage.g_mClassRecentItems.AddRecent(m_Tab(iIndex).m_File)

            RaiseEvent OnTabSaved(m_Tab(iIndex), "", m_Tab(iIndex).m_File)

            Return True
        End If
    End Function

    ''' <summary>
    ''' If the code has been changed it will prompt the user and saves the source. The user can abort the saving.
    ''' </summary>
    ''' <param name="iIndex"></param>
    ''' <param name="bAlwaysPrompt">If true, always show MessageBox even if the code didnt change</param>
    ''' <param name="bAlwaysYes">If true, ignores MessageBox prompt</param>
    ''' <returns>True if saved or declined, otherwise canceled.</returns>
    Public Function PromptSaveTab(iIndex As Integer, Optional bAlwaysPrompt As Boolean = False, Optional bAlwaysYes As Boolean = False, Optional bAlwaysSaveUnsaved As Boolean = False) As Boolean
        Dim bIsUnsaved As Boolean = (m_Tab(iIndex).m_IsUnsaved OrElse m_Tab(iIndex).m_InvalidFile)

        If (bAlwaysPrompt OrElse m_Tab(iIndex).m_Changed OrElse (bAlwaysSaveUnsaved AndAlso bIsUnsaved)) Then
            'Continue
        Else
            Return True
        End If

        Select Case (If(bAlwaysYes, DialogResult.Yes, MessageBox.Show(String.Format("Do you want to save your work? '{0}' ({1})", m_Tab(iIndex).m_Title, iIndex), "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)))
            Case DialogResult.Yes
                Return SaveFileTab(iIndex, bIsUnsaved)

            Case DialogResult.No
                Return True

            Case Else
                Return False

        End Select
    End Function

    ''' <summary>
    ''' Removes all unsaved tabs from left to right. Stops at first saved tab encounter.
    ''' </summary>
    Public Sub RemoveUnsavedTabsLeft()
        Try
            BeginUpdate()

            While m_TabsCount > 0
                If (m_Tab(0).m_IsUnsaved AndAlso Not m_Tab(0).m_Changed) Then
                    Dim bLast As Boolean = (m_TabsCount = 1)

                    If (Not RemoveTab(0, True, m_ActiveTabIndex)) Then
                        Return
                    End If

                    If (bLast) Then
                        Return
                    End If
                Else
                    Return
                End If
            End While
        Finally
            EndUpdate()
        End Try
    End Sub

    ''' <summary>
    ''' Removes all tabs.
    ''' </summary>
    Public Sub RemoveAllTabs()
        Try
            BeginUpdate()

            While m_TabsCount > 0
                Dim bLast As Boolean = (m_TabsCount = 1)

                If (Not RemoveTab(m_TabsCount - 1, True, 0)) Then
                    Return
                End If

                If (bLast) Then
                    Return
                End If
            End While
        Finally
            EndUpdate()
        End Try
    End Sub

    Public Sub CheckFilesChangedPrompt()
        Try
            For i = 0 To m_TabsCount - 1
                If (m_Tab(i).m_IsUnsaved OrElse m_Tab(i).m_InvalidFile) Then
                    Continue For
                End If

                If (Not m_Tab(i).m_IsFileNewer) Then
                    Continue For
                End If

                Dim sMessage As New Text.StringBuilder
                sMessage.AppendFormat("'{0}' in tab '{1}' has been changed! Do you want to reload the tab?", m_Tab(i).m_File, i + 1)

                If (m_Tab(i).m_Changed) Then
                    sMessage.AppendLine()
                    sMessage.AppendLine()
                    sMessage.AppendFormat("The tab you are trying to reload has unsaved content and will be lost!")
                End If

                Select Case (MessageBox.Show(sMessage.ToString, "File changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    Case DialogResult.Yes
                        While True
                            If (Not OpenFileTab(i, m_Tab(i).m_File, True)) Then
                                Select Case (MessageBox.Show("Could not open file", "Error", MessageBoxButtons.RetryCancel))
                                    Case DialogResult.Retry
                                        Continue While
                                End Select
                            End If

                            Exit While
                        End While
                End Select


                m_Tab(i).m_FileCachedWriteDate = m_Tab(i).m_FileRealWriteDate
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Public Function GetNextTabByLastSelection(mTab As ClassTab, iDirection As Integer) As ClassTab
        Return GetNextTabByLastSelection(mTab.m_LastViewTime, iDirection)
    End Function

    Public Function GetNextTabByLastSelection(mTime As Date, iDirection As Integer) As ClassTab
        Dim mSortedTabs As New List(Of KeyValuePair(Of Long, Object))
        Dim mIdentifier As New Object

        For Each mTab In GetAllTabs()
            mSortedTabs.Add(New KeyValuePair(Of Long, Object)(mTab.m_LastViewTime.Ticks, mTab))
        Next

        mSortedTabs.Add(New KeyValuePair(Of Long, Object)(mTime.Ticks, mIdentifier))

        mSortedTabs.Sort(Function(x As KeyValuePair(Of Long, Object), y As KeyValuePair(Of Long, Object))
                             Return x.Key.CompareTo(y.Key)
                         End Function)

        For i = 0 To mSortedTabs.Count - 1
            If (mSortedTabs(i).Value Is mIdentifier) Then
                If (iDirection = 1) Then
                    For j = i + 1 To mSortedTabs.Count - 1
                        If (TypeOf mSortedTabs(j).Value IsNot ClassTab) Then
                            Continue For
                        End If

                        If (mSortedTabs(j).Key = mSortedTabs(i).Key) Then
                            Continue For
                        End If

                        Return DirectCast(mSortedTabs(j).Value, ClassTab)
                    Next
                Else
                    For j = i - 1 To 0 Step -1
                        If (TypeOf mSortedTabs(j).Value IsNot ClassTab) Then
                            Continue For
                        End If

                        If (mSortedTabs(j).Key = mSortedTabs(i).Key) Then
                            Continue For
                        End If

                        Return DirectCast(mSortedTabs(j).Value, ClassTab)
                    Next
                End If
            End If
        Next

        Return Nothing
    End Function

    Public Function GetTabByCursorPoint() As ClassTab
        For i = 0 To g_mFormMain.TabControl_SourceTabs.TabPages.Count - 1
            If (g_mFormMain.TabControl_SourceTabs.GetTabRect(i).Contains(g_mFormMain.TabControl_SourceTabs.PointToClient(Cursor.Position))) Then
                Return DirectCast(g_mFormMain.TabControl_SourceTabs.TabPages(i), ClassTab)
            End If
        Next

        Return Nothing
    End Function

    Public Function GetTabIncludesByReferences(mTab As ClassTab, mTabs As ClassTab()) As Boolean
        Return GetTabIncludesByReferences(mTab, mTabs, Nothing, Nothing)
    End Function

    Public Function GetTabIncludesByReferences(mTab As ClassTab, mTabs As ClassTab(), ByRef r_SharedIncludes As List(Of KeyValuePair(Of String, String)), ByRef r_SharedIncludesFull As List(Of KeyValuePair(Of String, String))) As Boolean
        Dim sTabFile As String = mTab.m_File

        Dim bRefIncludeAdded As Boolean = False

        Dim i As Integer
        For i = 0 To mTabs.Length - 1
            If (mTabs(i).m_IsUnsaved) Then
                Continue For
            End If

            If (mTabs(i).m_File.ToLower = sTabFile.ToLower) Then
                Continue For
            End If

            If (mTabs(i).m_IncludesGroup.m_IncludeFiles.Count < 1) Then
                Continue For
            End If

            Dim sOtherTabIdentifier As String = mTabs(i).m_Identifier
            Dim mIncludes = mTabs(i).m_IncludesGroup.m_IncludeFiles.ToArray
            Dim bIsMain As Boolean = False

            Dim j As Integer
            For j = 0 To mIncludes.Length - 1
                'Only check orginal includes, skip other ones
                If (mIncludes(j).Key <> sOtherTabIdentifier) Then
                    Continue For
                End If

                If (mIncludes(j).Value.ToLower <> sTabFile.ToLower) Then
                    Continue For
                End If

                bIsMain = True
                Exit For
            Next

            If (Not bIsMain) Then
                Continue For
            End If

            For j = 0 To mIncludes.Length - 1
                'Only check orginal includes, skip other ones
                If (mIncludes(j).Key <> sOtherTabIdentifier) Then
                    Continue For
                End If

                If (r_SharedIncludes IsNot Nothing AndAlso Not r_SharedIncludes.Exists(Function(x As KeyValuePair(Of String, String)) x.Value.ToLower = mIncludes(j).Value.ToLower)) Then
                    r_SharedIncludes.Add(New KeyValuePair(Of String, String)(sOtherTabIdentifier, mIncludes(j).Value))
                End If

                If (r_SharedIncludesFull IsNot Nothing AndAlso Not r_SharedIncludesFull.Exists(Function(x As KeyValuePair(Of String, String)) x.Value.ToLower = mIncludes(j).Value.ToLower)) Then
                    r_SharedIncludesFull.Add(New KeyValuePair(Of String, String)(sOtherTabIdentifier, mIncludes(j).Value))
                End If

                bRefIncludeAdded = True
            Next
        Next

        Return bRefIncludeAdded
    End Function

    Public Sub BeginUpdate()
        g_iBeginUpdateCount += 1
        ClassTools.ClassForms.SuspendDrawing(g_iControlDrawCoutner, g_mFormMain.SplitContainer_ToolboxAndEditor)
    End Sub

    Public Sub EndUpdate()
        Try
            If (g_iBeginUpdateCount > 0) Then
                g_iBeginUpdateCount -= 1

                If (g_bBeginRequestSyntaxUpdate AndAlso g_iBeginUpdateCount = 0) Then
                    g_bBeginRequestSyntaxUpdate = False

                    g_mFormMain.g_ClassSyntaxTools.g_ClassSyntaxHighlighting.UpdateTextEditorSyntax()
                End If

                If (g_bBeginRequestFullUpdate AndAlso g_iBeginUpdateCount = 0) Then
                    g_bBeginRequestFullUpdate = False

                    Dim mTabs = g_lBeginRequestFullUpdateTabs.ToArray
                    g_lBeginRequestFullUpdateTabs.Clear()

                    FullUpdate(mTabs)
                End If
            End If
        Finally
            ClassTools.ClassForms.ResumeDrawing(g_iControlDrawCoutner, g_mFormMain.SplitContainer_ToolboxAndEditor)
        End Try
    End Sub

    Public Sub FullUpdate(mTab As ClassTab)
        FullUpdate({mTab})
    End Sub

    Public Sub FullUpdate(mTabs As ClassTab())
        g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete("")
        g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip("")

        g_mFormMain.g_mUCTextMinimap.UpdateText(False)
        g_mFormMain.g_mUCTextMinimap.UpdatePosition(False, True, True)

        For i = 0 To mTabs.Length - 1
            If (mTabs(i).IsDisposed) Then
                Continue For
            End If

            mTabs(i).UpdateFoldings()
        Next

        RaiseEvent OnTabFullUpdate(mTabs)

        For i = 0 To mTabs.Length - 1
            If (mTabs(i).IsDisposed) Then
                Continue For
            End If

            g_mFormMain.g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL, mTabs(i), ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS.NOONE)
        Next

        g_mFormMain.g_ClassSyntaxUpdater.ResetDelays()

        g_mFormMain.UpdateFormConfigText()
    End Sub

    Public Sub CleanInvalidSavedFoldStates()
        Using mStream = ClassFileStreamWait.Create(g_sFoldingsConfig, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                Dim lRemoveItems As New List(Of ClassIni.STRUC_INI_CONTENT)

                For Each mItem In mIni.ReadEverything
                    If (String.IsNullOrEmpty(mItem.sSection) OrElse Not IO.File.Exists(mItem.sSection)) Then
                        lRemoveItems.Add(New ClassIni.STRUC_INI_CONTENT(mItem.sSection, mItem.sKey, Nothing))
                    End If
                Next

                mIni.WriteKeyValue(lRemoveItems.ToArray)
            End Using
        End Using
    End Sub

    Private Sub OnTabSelected(sender As Object, e As EventArgs)
        If (g_bIgnoreOnTabSelected) Then
            Return
        End If

        Try
            g_bIgnoreOnTabSelected = True
            SelectTab(m_ActiveTabIndex)
        Finally
            g_bIgnoreOnTabSelected = False
        End Try
    End Sub

    Private Sub OnTabSyntaxParseSuccess(iUpdateType As ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS, sTabIdentifier As String, iOptionFlags As ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS, iFullParseError As ClassSyntaxParser.ENUM_PARSE_ERROR, iVarParseError As ClassSyntaxParser.ENUM_PARSE_ERROR)
        Try
            ParseUpdateReference(iUpdateType, sTabIdentifier, iOptionFlags, iFullParseError, iVarParseError)
            ParseUpdateSyntax(iUpdateType, sTabIdentifier, iOptionFlags, iFullParseError, iVarParseError)
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ClassExceptionLog.WriteToLog(ex)
        End Try
    End Sub

    Private Sub ParseUpdateReference(iUpdateType As ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS, sTabIdentifier As String, iOptionFlags As ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS, iFullParseError As ClassSyntaxParser.ENUM_PARSE_ERROR, iVarParseError As ClassSyntaxParser.ENUM_PARSE_ERROR)
        Try
            Dim mRequestTab As ClassTab = ClassThread.ExecEx(Of ClassTab)(g_mFormMain, Function() g_mFormMain.g_ClassTabControl.GetTabByIdentifier(sTabIdentifier))
            If (mRequestTab Is Nothing) Then
                Return
            End If

            Dim mTabs As ClassTab() = ClassThread.ExecEx(Of ClassTab())(g_mFormMain, Function() g_mFormMain.g_ClassTabControl.GetAllTabs())

            Dim bHasReferences As Boolean = GetTabIncludesByReferences(mRequestTab, mTabs)

            ''mRequestTab.m_HasReferenceIncludes' Calls UI elements, invoke it.
            ClassThread.ExecAsync(g_mFormMain, Sub()
                                                   mRequestTab.m_HasReferenceIncludes = bHasReferences
                                               End Sub)
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ClassExceptionLog.WriteToLog(ex)
        End Try
    End Sub

    Private Sub ParseUpdateSyntax(iUpdateType As ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS, sTabIdentifier As String, iOptionFlags As ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS, iFullParseError As ClassSyntaxParser.ENUM_PARSE_ERROR, iVarParseError As ClassSyntaxParser.ENUM_PARSE_ERROR)
        Try
            Static sLastTabIndentifier As String = ""

            If ((iUpdateType And ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.FULL_PARSE) = 0) Then
                Return
            End If

            Dim sActiveTabIdentifier As String = ClassThread.ExecEx(Of String)(g_mFormMain, Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Identifier)
            If (sActiveTabIdentifier <> sTabIdentifier) Then
                Return
            End If

            Select Case (iFullParseError)
                Case ClassSyntaxParser.ENUM_PARSE_ERROR.UNCHANGED,
                        ClassSyntaxParser.ENUM_PARSE_ERROR.CACHED
                    'Do not update unchanged tabs more than once
                    If (sActiveTabIdentifier = sLastTabIndentifier) Then
                        Return
                    End If

                    sLastTabIndentifier = sActiveTabIdentifier

                Case ClassSyntaxParser.ENUM_PARSE_ERROR.UPDATED
                    sLastTabIndentifier = sActiveTabIdentifier

                Case Else
                    Return
            End Select

            ClassThread.ExecAsync(g_mFormMain, Sub()
                                                   g_mFormMain.g_ClassSyntaxTools.g_ClassSyntaxHighlighting.UpdateSyntax(ClassSyntaxTools.ENUM_SYNTAX_UPDATE_TYPE.AUTOCOMPLETE)
                                                   g_mFormMain.g_ClassSyntaxTools.g_ClassSyntaxHighlighting.UpdateTextEditorSyntax()
                                               End Sub)
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ClassExceptionLog.WriteToLog(ex)
        End Try
    End Sub

    Public Class ClassTab
        Inherits TabPage

        Private g_mFormMain As FormMain

        Private g_sText As String = "Unnamed"
        Private g_bTextChanged As Boolean = False
        Private g_sIdentifier As String = Guid.NewGuid.ToString
        Private g_sFile As String = ""

        Private g_mAutocompleteGroup As STRUC_AUTOCOMPLETE_GROUP
        Private g_mIncludesGroup As STRUC_INCLUDES_GROUP

        Private g_mActiveConfig As ClassConfigs.STRUC_CONFIG_ITEM = ClassConfigs.m_DefaultConfig
        Private g_iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN

        Private g_mSourceTextEditor As TextEditorControlEx
        Private g_bHasReferenceIncludes As Boolean = False
        Private g_bHandlersEnabled As Boolean = False
        Private g_mFileCachedWriteDate As Date
        Private g_mLastViewTime As Date

        Public g_ClassFoldings As ClassFoldings
        Public g_ClassLineState As ClassLineState
        Public g_ClassScopeHighlighting As ClassScopeHighlighting
        Public g_ClassMarkerHighlighting As ClassMarkerHighlighting
        Public g_ClassDragDropManager As ClassDragDropManager
        Public g_ClassTextEditorControlsManager As ClassTextEditorControlsManager

        Public Sub New(f As FormMain)
            g_mFormMain = f

            g_mAutocompleteGroup = New STRUC_AUTOCOMPLETE_GROUP(Me)
            g_mIncludesGroup = New STRUC_INCLUDES_GROUP(Me)
            g_ClassFoldings = New ClassFoldings(Me)
            g_ClassLineState = New ClassLineState(Me)
            g_ClassScopeHighlighting = New ClassScopeHighlighting(Me)
            g_ClassMarkerHighlighting = New ClassMarkerHighlighting(Me)
            g_ClassDragDropManager = New ClassDragDropManager(Me)
            g_ClassTextEditorControlsManager = New ClassTextEditorControlsManager(Me)

            CreateTextEditor()

            ClassControlStyle.UpdateControls(Me)
        End Sub

        Public Sub New(f As FormMain, sText As String)
            g_mFormMain = f

            g_mAutocompleteGroup = New STRUC_AUTOCOMPLETE_GROUP(Me)
            g_mIncludesGroup = New STRUC_INCLUDES_GROUP(Me)
            g_ClassFoldings = New ClassFoldings(Me)
            g_ClassLineState = New ClassLineState(Me)
            g_ClassScopeHighlighting = New ClassScopeHighlighting(Me)
            g_ClassMarkerHighlighting = New ClassMarkerHighlighting(Me)
            g_ClassDragDropManager = New ClassDragDropManager(Me)
            g_ClassTextEditorControlsManager = New ClassTextEditorControlsManager(Me)

            m_Title = sText

            CreateTextEditor()

            ClassControlStyle.UpdateControls(Me)
        End Sub

        Public Sub New(f As FormMain, sText As String, bChanged As Boolean)
            g_mFormMain = f

            g_mAutocompleteGroup = New STRUC_AUTOCOMPLETE_GROUP(Me)
            g_mIncludesGroup = New STRUC_INCLUDES_GROUP(Me)
            g_ClassFoldings = New ClassFoldings(Me)
            g_ClassLineState = New ClassLineState(Me)
            g_ClassScopeHighlighting = New ClassScopeHighlighting(Me)
            g_ClassMarkerHighlighting = New ClassMarkerHighlighting(Me)
            g_ClassDragDropManager = New ClassDragDropManager(Me)
            g_ClassTextEditorControlsManager = New ClassTextEditorControlsManager(Me)

            m_Title = sText
            m_Changed = bChanged

            CreateTextEditor()

            ClassControlStyle.UpdateControls(Me)
        End Sub

        Private Sub AddHandlers()
            RemoveHandlers()

            g_ClassDragDropManager.AddHandlers()
            g_ClassTextEditorControlsManager.AddHandlers()

            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MotherTextAreaControl.VScrollBar.ValueChanged, AddressOf TextEditorControl_Source_Scroll
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Document.FoldingManager.FoldingsChanged, AddressOf TextEditorControl_FoldingsChanged
        End Sub

        Private Sub RemoveHandlers()
            g_ClassDragDropManager.RemoveHandlers()
            g_ClassTextEditorControlsManager.RemoveHandlers()

            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MotherTextAreaControl.VScrollBar.ValueChanged, AddressOf TextEditorControl_Source_Scroll
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Document.FoldingManager.FoldingsChanged, AddressOf TextEditorControl_FoldingsChanged
        End Sub

        Private Sub CreateTextEditor()
            g_mSourceTextEditor = New TextEditorControlEx
            g_mSourceTextEditor.SuspendLayout()

            g_mSourceTextEditor.ContextMenuStrip = g_mFormMain.ContextMenuStrip_RightClick
            g_mSourceTextEditor.ShowMatchingBracket = False
            g_mSourceTextEditor.Margin = New Padding(0)
            g_mSourceTextEditor.Padding = New Padding(0)

            g_mSourceTextEditor.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
            g_mSourceTextEditor.TextEditorProperties.IndentationSize = If(ClassSettings.g_iSettingsTabsToSpaces > 0, ClassSettings.g_iSettingsTabsToSpaces, 4)
            g_mSourceTextEditor.TextEditorProperties.ConvertTabsToSpaces = (ClassSettings.g_iSettingsTabsToSpaces > 0)
            g_mSourceTextEditor.TextEditorProperties.ShowTabs = ClassSettings.g_bSettingsShowTabs ' TODO: Solve performance issues rendering tab arrows.
            g_mSourceTextEditor.TextEditorProperties.ShowVerticalRuler = ClassSettings.g_bSettingsShowVRuler
            g_mSourceTextEditor.m_CustomIconBarVisible = ClassSettings.g_bSettingsIconBar

            g_mSourceTextEditor.Parent = Me
            g_mSourceTextEditor.Dock = DockStyle.Fill

            g_mSourceTextEditor.Document.FoldingManager.FoldingStrategy = New ClassFoldings.ClassFoldingStrategy()
            g_mSourceTextEditor.Document.FoldingManager.UpdateFoldings(Nothing, Nothing)

            g_mSourceTextEditor.Visible = True
            g_mSourceTextEditor.ResumeLayout()
        End Sub

        Public Sub NewIndetifier()
            g_sIdentifier = Guid.NewGuid.ToString
        End Sub

        Public Function OpenFileTab(sFile As String, Optional bIgnoreSavePrompt As Boolean = False) As Boolean
            Return g_mFormMain.g_ClassTabControl.OpenFileTab(m_Index, sFile, bIgnoreSavePrompt)
        End Function

        Public Function SaveFileTab(bSaveAs As Boolean) As Boolean
            Return g_mFormMain.g_ClassTabControl.SaveFileTab(Me.m_Index, bSaveAs)
        End Function

        Public Sub SelectTab()
            g_mFormMain.g_ClassTabControl.SelectTab(m_Index)
        End Sub

        Public Sub SelectTab(iDelay As Integer)
            g_mFormMain.g_ClassTabControl.SelectTab(m_Identifier, iDelay)
        End Sub

        Public Function RemoveTab(bPrompSave As Boolean, Optional iSelectTabIndex As Integer = -1) As Boolean
            Return g_mFormMain.g_ClassTabControl.RemoveTab(m_Index, bPrompSave, iSelectTabIndex)
        End Function

        Public Function RemoveTabGotoLast(bPrompSave As Boolean) As Boolean
            Return g_mFormMain.g_ClassTabControl.RemoveTabGotoLast(m_Index, bPrompSave)
        End Function

        Public Sub UpdateFoldings()
            g_ClassFoldings.UpdateFoldings()
        End Sub

        Public Sub UpdateLastViewTime()
            g_mLastViewTime = Now
        End Sub

        Public Property m_HandlersEnabled As Boolean
            Get
                Return g_bHandlersEnabled
            End Get
            Set(value As Boolean)
                g_bHandlersEnabled = value

                If (g_bHandlersEnabled) Then
                    AddHandlers()
                Else
                    RemoveHandlers()
                End If
            End Set
        End Property

        Public Property m_File As String
            Get
                Return g_sFile
            End Get
            Set(value As String)
                g_sFile = value
                m_Title = IO.Path.GetFileName(value)

                UpdateToolTip()
            End Set
        End Property

        Public Property m_FileRealWriteDate As Date
            Get
                Return IO.File.GetLastWriteTime(g_sFile)
            End Get
            Set(value As Date)
                IO.File.SetLastWriteTime(g_sFile, value)
            End Set
        End Property

        Public Property m_FileCachedWriteDate As Date
            Get
                Return g_mFileCachedWriteDate
            End Get
            Set(value As Date)
                g_mFileCachedWriteDate = value
            End Set
        End Property

        Public Property m_LastViewTime As Date
            Get
                Return g_mLastViewTime
            End Get
            Set(value As Date)
                g_mLastViewTime = value
            End Set
        End Property

        Public ReadOnly Property m_IsFileNewer As Boolean
            Get
                Return (m_FileRealWriteDate > m_FileCachedWriteDate)
            End Get
        End Property

        Public ReadOnly Property m_IsUnsaved As Boolean
            Get
                Return String.IsNullOrEmpty(g_sFile)
            End Get
        End Property

        Public ReadOnly Property m_InvalidFile As Boolean
            Get
                Return (Not IO.File.Exists(g_sFile))
            End Get
        End Property

        Public ReadOnly Property m_IsActive As Boolean
            Get
                Return (g_mFormMain.g_ClassTabControl.m_ActiveTabIndex = m_Index)
            End Get
        End Property

        Public ReadOnly Property m_Identifier As String
            Get
                Return g_sIdentifier
            End Get
        End Property

        Public ReadOnly Property m_TextEditor As TextEditorControlEx
            Get
                Return g_mSourceTextEditor
            End Get
        End Property

        Public ReadOnly Property m_AutocompleteGroup As STRUC_AUTOCOMPLETE_GROUP
            Get
                Return g_mAutocompleteGroup
            End Get
        End Property

        Public ReadOnly Property m_IncludesGroup As STRUC_INCLUDES_GROUP
            Get
                Return g_mIncludesGroup
            End Get
        End Property

        Public Property m_ActiveConfig As ClassConfigs.STRUC_CONFIG_ITEM
            Get
                If (g_mActiveConfig IsNot Nothing AndAlso g_mActiveConfig.ConfigExist) Then
                    Return g_mActiveConfig
                Else
                    Return ClassConfigs.m_DefaultConfig
                End If
            End Get
            Set(value As ClassConfigs.STRUC_CONFIG_ITEM)
                If (value Is Nothing) Then
                    g_mActiveConfig = ClassConfigs.m_DefaultConfig
                Else
                    g_mActiveConfig = value
                End If
            End Set
        End Property

        Public Property m_Language As ClassSyntaxTools.ENUM_LANGUAGE_TYPE
            Get
                Return g_iLanguage
            End Get
            Set(value As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
                g_iLanguage = value
            End Set
        End Property

        Public Property m_HasReferenceIncludes As Boolean
            Get
                Return g_bHasReferenceIncludes
            End Get
            Set(value As Boolean)
                If (g_bHasReferenceIncludes <> value) Then
                    g_bHasReferenceIncludes = value

                    Me.Text = g_sText
                    UpdateToolTip()
                End If
            End Set
        End Property

        Public ReadOnly Property m_Index As Integer
            Get
                For i = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                    If (g_mFormMain.g_ClassTabControl.m_Tab(i) Is Me) Then
                        Return i
                    End If
                Next

                Return -1
            End Get
        End Property

        Public Property m_Changed As Boolean
            Get
                Return g_bTextChanged
            End Get
            Set(value As Boolean)
                If (g_bTextChanged <> value) Then
                    g_bTextChanged = value

                    Me.Text = g_sText
                    UpdateToolTip()
                End If
            End Set
        End Property

        Public Property m_Title As String
            Get
                Return g_sText
            End Get
            Set(value As String)
                If (g_sText <> value) Then
                    g_sText = value

                    Me.Text = value
                End If
            End Set
        End Property

        Public Sub UpdateToolTip()
            Dim sInfo As New Text.StringBuilder
            sInfo.AppendLine(g_sFile)

            If (m_Changed) Then
                sInfo.AppendLine(" - Content changed but not saved.")
            End If

            If (m_HasReferenceIncludes) Then
                sInfo.AppendLine(" - Has been referenced in other open tabs.")
            End If

            Me.ToolTipText = sInfo.ToString
        End Sub


        Public Overrides Property Text As String
            Get
                Return If(m_HasReferenceIncludes, "#", "") & g_sText & If(m_Changed, "*"c, "")
            End Get
            Set(value As String)
                MyBase.Text = value
            End Set
        End Property

        Private Sub TextEditorControl_Source_Scroll(sender As Object, e As EventArgs)
            g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTipFormLocation()
            g_mFormMain.g_mUCTextMinimap.UpdatePosition(False, True, True)
        End Sub

        Private Sub TextEditorControl_FoldingsChanged(sender As Object, e As EventArgs)
            g_mFormMain.g_mUCTextMinimap.UpdateText(True)

            If (ClassSettings.g_bSettingsRememberFoldings) Then
                g_ClassFoldings.SetSavedFoldStates()
            End If
        End Sub

        Public Class STRUC_AUTOCOMPLETE_GROUP
            Private g_ClassTab As ClassTab

            Private g_mAutocompleteItems As New ClassSyncList(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Private g_mAutocompleteIdentifier As New ClassSyncList(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER)

            Public Sub New(c As ClassTab)
                g_ClassTab = c
            End Sub

            ReadOnly Property m_AutocompleteItems As ClassSyncList(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
                Get
                    Return g_mAutocompleteItems
                End Get
            End Property

            ReadOnly Property m_AutocompleteIdentifier As ClassSyncList(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER)
                Get
                    Return g_mAutocompleteIdentifier
                End Get
            End Property

            Property m_AutocompleteIdentifierItem(iType As ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE) As String
                Get
                    Dim mIdentifier = m_AutocompleteIdentifier.Find(Function(a As ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER)
                                                                        Return (a.m_Type = iType)
                                                                    End Function)

                    If (mIdentifier Is Nothing) Then
                        Return Nothing
                    End If

                    Return mIdentifier.m_Identifier
                End Get
                Set(value As String)
                    m_AutocompleteIdentifier.RemoveAll(Function(a As ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER)
                                                           Return (a.m_Type = iType)
                                                       End Function)

                    m_AutocompleteIdentifier.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER(iType, value))
                End Set
            End Property

            Public Sub CopyToTab(mTab As ClassTab)
                mTab.m_AutocompleteGroup.m_AutocompleteItems.DoSync(
                    Sub()
                        mTab.m_AutocompleteGroup.m_AutocompleteItems.Clear()
                        For Each mItem In m_AutocompleteItems.ToArray
                            mTab.m_AutocompleteGroup.m_AutocompleteItems.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mItem))
                        Next
                    End Sub)
            End Sub

            Public Function GenerateAutocompleteIdentifier() As String
                Dim lIdentifierBuilder As New List(Of String)

                'Add tab information
                lIdentifierBuilder.Add(CStr(g_ClassTab.m_Language))
                lIdentifierBuilder.Add(g_ClassTab.m_ActiveConfig.GetName)
                lIdentifierBuilder.Add(g_ClassTab.m_TextEditor.Document.TextContent)

                For Each mInclude In g_ClassTab.m_IncludesGroup.m_IncludeFiles.ToArray
                    'Add file path
                    lIdentifierBuilder.Add(mInclude.Value)

                    If (Not IO.File.Exists(mInclude.Value)) Then
                        Continue For
                    End If

                    'Add file last write time
                    lIdentifierBuilder.Add(IO.File.GetLastWriteTime(mInclude.Value).ToString)
                Next

                Dim sIdentifier As String
                sIdentifier = String.Join("|", lIdentifierBuilder.ToArray)
                sIdentifier = ClassTools.ClassCrypto.ClassHash.SHA256StringHash(sIdentifier)

                Return sIdentifier
            End Function

            Public Function CheckAutocompleteIdentifier(iType As ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER.ENUM_AUTOCOMPLETE_TYPE, sAutocompleteIdentifier As String) As Boolean
                If (String.IsNullOrEmpty(sAutocompleteIdentifier)) Then
                    Return False
                End If

                If (m_AutocompleteIdentifier.Exists(Function(a As ClassSyntaxTools.STRUC_AUTOCOMPLETE_IDENTIFIER)
                                                        Return (a.m_Type = iType AndAlso a.m_Identifier = sAutocompleteIdentifier)
                                                    End Function)) Then
                    Return True
                End If

                Return False
            End Function
        End Class

        Public Class STRUC_INCLUDES_GROUP
            Private g_ClassTab As ClassTab

            Private g_mIncludeFiles As New ClassSyncList(Of KeyValuePair(Of String, String)) '{sTabIdentifier-Ref, IncludeFile}
            Private g_mIncludeFilesFull As New ClassSyncList(Of KeyValuePair(Of String, String)) '{sTabIdentifier-Ref, IncludeFile}

            Public Sub New(c As ClassTab)
                g_ClassTab = c
            End Sub

            ReadOnly Property m_IncludeFiles As ClassSyncList(Of KeyValuePair(Of String, String))
                Get
                    Return g_mIncludeFiles
                End Get
            End Property

            ReadOnly Property m_IncludeFilesFull As ClassSyncList(Of KeyValuePair(Of String, String))
                Get
                    Return g_mIncludeFilesFull
                End Get
            End Property
        End Class

        Public Class ClassDragDropManager
            Private g_ClassTab As ClassTab

            Public Sub New(c As ClassTab)
                g_ClassTab = c
            End Sub

            Public Sub AddHandlers()
                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.DragEnter, AddressOf TextEditorControl_Source_DragEnter
                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.DragDrop, AddressOf TextEditorControl_Source_DragDrop
                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.DragOver, AddressOf TextEditorControl_Source_DragOver
            End Sub

            Public Sub RemoveHandlers()
                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.DragEnter, AddressOf TextEditorControl_Source_DragEnter
                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.DragDrop, AddressOf TextEditorControl_Source_DragDrop
                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.DragOver, AddressOf TextEditorControl_Source_DragOver
            End Sub

            Private Sub TextEditorControl_Source_DragEnter(sender As Object, e As DragEventArgs)
                Try
                    If (Not e.Data.GetDataPresent(DataFormats.FileDrop)) Then
                        Return
                    End If

                    e.Effect = DragDropEffects.Copy
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            End Sub

            Private Sub TextEditorControl_Source_DragOver(sender As Object, e As DragEventArgs)
                Try
                    If (Not e.Data.GetDataPresent(DataFormats.FileDrop)) Then
                        Return
                    End If

                    e.Effect = DragDropEffects.Copy
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            End Sub

            Private Sub TextEditorControl_Source_DragDrop(sender As Object, e As DragEventArgs)
                Try
                    If (Not e.Data.GetDataPresent(DataFormats.FileDrop)) Then
                        Return
                    End If

                    Dim sFiles As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())

                    Try
                        g_ClassTab.g_mFormMain.g_ClassTabControl.BeginUpdate()

                        For Each sFile As String In sFiles
                            If (Not IO.File.Exists(sFile)) Then
                                Continue For
                            End If

                            Dim mTab = g_ClassTab.g_mFormMain.g_ClassTabControl.AddTab()
                            mTab.OpenFileTab(sFile)
                            mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)
                        Next
                    Finally
                        g_ClassTab.g_mFormMain.g_ClassTabControl.EndUpdate()
                    End Try
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            End Sub
        End Class

        Public Class ClassTextEditorControlsManager
            Private g_ClassTab As ClassTab

            Public Sub New(c As ClassTab)
                g_ClassTab = c
            End Sub

            Public Sub AddHandlers()
                AddHandler g_ClassTab.m_TextEditor.ProcessCmdKeyEvent, AddressOf TextEditorControl_Source_ProcessCmdKey
                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.KeyPress, AddressOf TextEditorControl_Source_AutoCloseChars

                AddHandler g_ClassTab.m_TextEditor.TextChanged, AddressOf TextEditorControl_Source_TextChanged

                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord

                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete

                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.KeyPress, AddressOf TextEditorControl_Source_SwitchToAutocompleteTabKeyPress
                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_Source_SwitchToAutocompleteTab
                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_MouseClick

                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseDown, AddressOf TextEditorControl_PeekDefinition_Pre
                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_PeekDefinition_Post

                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectionChanged, AddressOf TextEditorControl_Source_UpdateInfo
                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged, AddressOf TextEditorControl_Source_UpdateInfo

                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.Document.LineLengthChanged, AddressOf TextEditorControl_DetectLineLengthChange
                AddHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.Document.LineCountChanged, AddressOf TextEditorControl_DetectLineCountChange
            End Sub

            Public Sub RemoveHandlers()
                RemoveHandler g_ClassTab.m_TextEditor.ProcessCmdKeyEvent, AddressOf TextEditorControl_Source_ProcessCmdKey
                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.KeyPress, AddressOf TextEditorControl_Source_AutoCloseChars

                RemoveHandler g_ClassTab.m_TextEditor.TextChanged, AddressOf TextEditorControl_Source_TextChanged

                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord

                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete

                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.KeyPress, AddressOf TextEditorControl_Source_SwitchToAutocompleteTabKeyPress
                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_Source_SwitchToAutocompleteTab
                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_MouseClick

                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseDown, AddressOf TextEditorControl_PeekDefinition_Pre
                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_PeekDefinition_Post

                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectionChanged, AddressOf TextEditorControl_Source_UpdateInfo
                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged, AddressOf TextEditorControl_Source_UpdateInfo

                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.Document.LineLengthChanged, AddressOf TextEditorControl_DetectLineLengthChange
                RemoveHandler g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.Document.LineCountChanged, AddressOf TextEditorControl_DetectLineCountChange
            End Sub

            Private Sub TextEditorControl_Source_ProcessCmdKey(ByRef bBlock As Boolean, ByRef iMsg As Message, iKeys As Keys)
                Try
                    Select Case (iKeys)
                    'Details primary/secondary action
                        Case (Keys.Control Or Keys.Enter),
                                (Keys.Shift Or Keys.Control Or Keys.Enter)
                            bBlock = True

                            Dim bSpecialAction As Boolean = ((iKeys And Keys.Shift) <> 0)

                            g_ClassTab.g_mFormMain.g_ClassTabControl.__OnTextEditorTabDetailsAction(g_ClassTab, g_ClassTab.g_mFormMain.TabControl_Details.SelectedIndex, bSpecialAction, iKeys)

                    'Details navigation up
                        Case (Keys.Control Or Keys.Up),
                                (Keys.Control Or Keys.Down)
                            bBlock = True

                            Dim iDirection As Integer = If(iKeys = (Keys.Control Or Keys.Up), -1, 1)
                            g_ClassTab.g_mFormMain.g_ClassTabControl.__OnTextEditorTabDetailsMove(g_ClassTab, g_ClassTab.g_mFormMain.TabControl_Details.SelectedIndex, iDirection, iKeys)

                    'Details tab navigation
                        Case (Keys.Alt Or Keys.Control Or Keys.Left),
                               (Keys.Alt Or Keys.Control Or Keys.Right)
                            bBlock = True

                            Dim iNewIndex As Integer

                            If (iKeys = (Keys.Alt Or Keys.Control Or Keys.Left)) Then
                                iNewIndex = ClassTools.ClassMath.ClampInt(g_ClassTab.g_mFormMain.TabControl_Details.SelectedIndex - 1, 0, g_ClassTab.g_mFormMain.TabControl_Details.TabCount - 1)
                            Else
                                iNewIndex = ClassTools.ClassMath.ClampInt(g_ClassTab.g_mFormMain.TabControl_Details.SelectedIndex + 1, 0, g_ClassTab.g_mFormMain.TabControl_Details.TabCount - 1)
                            End If

                            If (g_ClassTab.g_mFormMain.TabControl_Details.SelectedIndex <> iNewIndex) Then
                                'Make control not auto-selected 
                                g_ClassTab.g_mFormMain.TabControl_Details.SelectTabNoFocus(iNewIndex)
                            End If

                    'Auto-Indent Brackets
                        Case Keys.Enter
                            If (Not ClassSettings.g_bSettingsAutoIndentBrackets) Then
                                Exit Select
                            End If

                            Dim iOffset As Integer = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset
                            Dim iLenght As Integer = g_ClassTab.m_TextEditor.Document.TextLength
                            If (iOffset - 1 < 0 OrElse iOffset > iLenght - 1) Then
                                Exit Select
                            End If

                            If (g_ClassTab.m_TextEditor.Document.GetCharAt(iOffset - 1) <> "{"c) Then
                                Exit Select
                            End If

                            bBlock = True

                            Try
                                g_ClassTab.m_TextEditor.Document.UndoStack.StartUndoGroup()

                                Dim bPushNewline As Boolean = False

                                If (g_ClassTab.m_TextEditor.Document.GetCharAt(iOffset) <> "}"c) Then
                                    Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(g_ClassTab.m_TextEditor.Document.TextContent, g_ClassTab.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                                    'Check if some end braces are missing. If so, add one.
                                    Dim iRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                                    Dim iBraceLevel As Integer = mSourceAnalysis.GetBraceLevel(mSourceAnalysis.m_MaxLength - 1, iRange)
                                    If (iRange = ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END) Then
                                        iBraceLevel -= 1
                                    End If

                                    If (iBraceLevel > 0) Then
                                        g_ClassTab.m_TextEditor.Document.Insert(iOffset, "}")
                                        bPushNewline = True
                                    End If
                                Else
                                    bPushNewline = True
                                End If

                                Call (New ICSharpCode.TextEditor.Actions.Return).Execute(g_ClassTab.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea)

                                If (bPushNewline) Then
                                    Dim mLastCaretLocation = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Position

                                    Call (New ICSharpCode.TextEditor.Actions.Return).Execute(g_ClassTab.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea)

                                    g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Position = mLastCaretLocation
                                End If

                                Call (New ICSharpCode.TextEditor.Actions.Tab).Execute(g_ClassTab.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea)
                            Finally
                                g_ClassTab.m_TextEditor.Document.UndoStack.EndUndoGroup()

                                g_ClassTab.m_TextEditor.InvalidateTextArea()
                            End Try
                    End Select
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            End Sub

            Private Sub TextEditorControl_Source_AutoCloseChars(sender As Object, e As KeyPressEventArgs)
                Static sLastAutoClosedKey As String = ""

                Dim bLastAutoCloseKeySet As Boolean = False

                If (ClassSettings.g_bSettingsAutoCloseStrings) Then
                    Select Case True
                        Case (sLastAutoClosedKey = """" AndAlso e.KeyChar = """"c)
                            Dim iOffset As Integer = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset
                            Dim iLenght As Integer = g_ClassTab.m_TextEditor.Document.TextLength
                            If (iOffset > iLenght - 1) Then
                                Exit Select
                            End If

                            If (g_ClassTab.m_TextEditor.Document.GetCharAt(iOffset) <> """"c) Then
                                Exit Select
                            End If

                            g_ClassTab.m_TextEditor.Document.Remove(iOffset, 1)

                        Case (sLastAutoClosedKey = "'" AndAlso e.KeyChar = "'"c)
                            Dim iOffset As Integer = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset
                            Dim iLenght As Integer = g_ClassTab.m_TextEditor.Document.TextLength
                            If (iOffset > iLenght - 1) Then
                                Exit Select
                            End If

                            If (g_ClassTab.m_TextEditor.Document.GetCharAt(iOffset) <> "'"c) Then
                                Exit Select
                            End If

                            g_ClassTab.m_TextEditor.Document.Remove(iOffset, 1)

                        Case (e.KeyChar = """"c)
                            Dim iOffset As Integer = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset
                            If (iOffset > 0 AndAlso g_ClassTab.m_TextEditor.Document.GetText(iOffset - 1, 1) = ClassSyntaxTools.g_sEscapeCharacters(g_ClassTab.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)) Then
                                Exit Select
                            End If

                            Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(g_ClassTab.m_TextEditor.Document.TextContent, g_ClassTab.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)
                            If (mSourceAnalysis.m_InString(iOffset) OrElse mSourceAnalysis.m_InChar(iOffset)) Then
                                Exit Select
                            End If

                            g_ClassTab.m_TextEditor.Document.Insert(iOffset, """")

                            sLastAutoClosedKey = """"c
                            bLastAutoCloseKeySet = True

                        Case (e.KeyChar = "'"c)
                            Dim iOffset As Integer = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset
                            If (iOffset > 0 AndAlso g_ClassTab.m_TextEditor.Document.GetText(iOffset - 1, 1) = ClassSyntaxTools.g_sEscapeCharacters(g_ClassTab.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)) Then
                                Exit Select
                            End If

                            Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(g_ClassTab.m_TextEditor.Document.TextContent, g_ClassTab.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)
                            If (mSourceAnalysis.m_InString(iOffset) OrElse mSourceAnalysis.m_InChar(iOffset)) Then
                                Exit Select
                            End If

                            g_ClassTab.m_TextEditor.Document.Insert(iOffset, "'")

                            sLastAutoClosedKey = "'"c
                            bLastAutoCloseKeySet = True
                    End Select
                End If

                If (ClassSettings.g_bSettingsAutoCloseBrackets) Then
                    Select Case True
                        Case (sLastAutoClosedKey = "[" AndAlso e.KeyChar = "]"c)
                            Dim iOffset As Integer = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset
                            Dim iLenght As Integer = g_ClassTab.m_TextEditor.Document.TextLength
                            If (iOffset > iLenght - 1) Then
                                Exit Select
                            End If

                            If (g_ClassTab.m_TextEditor.Document.GetCharAt(iOffset) <> "]"c) Then
                                Exit Select
                            End If

                            g_ClassTab.m_TextEditor.Document.Remove(iOffset, 1)

                        Case (sLastAutoClosedKey = "(" AndAlso e.KeyChar = ")"c)
                            Dim iOffset As Integer = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset
                            Dim iLenght As Integer = g_ClassTab.m_TextEditor.Document.TextLength
                            If (iOffset > iLenght - 1) Then
                                Exit Select
                            End If

                            If (g_ClassTab.m_TextEditor.Document.GetCharAt(iOffset) <> ")"c) Then
                                Exit Select
                            End If

                            g_ClassTab.m_TextEditor.Document.Remove(iOffset, 1)

                        Case (sLastAutoClosedKey = "{" AndAlso e.KeyChar = "}"c)
                            Dim iOffset As Integer = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset
                            Dim iLenght As Integer = g_ClassTab.m_TextEditor.Document.TextLength
                            If (iOffset > iLenght - 1) Then
                                Exit Select
                            End If

                            If (g_ClassTab.m_TextEditor.Document.GetCharAt(iOffset) <> "}"c) Then
                                Exit Select
                            End If

                            g_ClassTab.m_TextEditor.Document.Remove(iOffset, 1)

                        Case (e.KeyChar = "["c)
                            Dim iOffset As Integer = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset
                            g_ClassTab.m_TextEditor.Document.Insert(iOffset, "]")

                            sLastAutoClosedKey = "["c
                            bLastAutoCloseKeySet = True

                        Case (e.KeyChar = "("c)
                            Dim iOffset As Integer = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset
                            g_ClassTab.m_TextEditor.Document.Insert(iOffset, ")")

                            sLastAutoClosedKey = "("c
                            bLastAutoCloseKeySet = True

                        Case (e.KeyChar = "{"c)
                            Dim iOffset As Integer = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset
                            g_ClassTab.m_TextEditor.Document.Insert(iOffset, "}")

                            sLastAutoClosedKey = "{"c
                            bLastAutoCloseKeySet = True
                    End Select
                End If

                If (Not bLastAutoCloseKeySet AndAlso sLastAutoClosedKey <> "") Then
                    sLastAutoClosedKey = ""
                End If
            End Sub

            Private Sub TextEditorControl_Source_UpdateInfo(sender As Object, e As EventArgs)
                g_ClassTab.g_mFormMain.ToolStripStatusLabel_EditorLine.Text = "L: " & g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Line + 1
                g_ClassTab.g_mFormMain.ToolStripStatusLabel_EditorCollum.Text = "C: " & g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Column
                g_ClassTab.g_mFormMain.ToolStripStatusLabel_EditorSelectedCount.Text = "S: " & g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText.Length
            End Sub

            Private Sub TextEditorControl_DetectLineLengthChange(sender As Object, e As LineLengthChangeEventArgs)
                Try
                    Dim iTotalLines As Integer = g_ClassTab.m_TextEditor.Document.TotalNumberOfLines

                    If (e.LineSegment.IsDeleted OrElse e.LineSegment.Length < 0) Then
                        Return
                    End If

                    If (e.LineSegment.LineNumber > iTotalLines) Then
                        Return
                    End If

                    g_ClassTab.g_ClassLineState.m_LineState(e.LineSegment.LineNumber) = ClassTextEditorTools.ClassLineStateMark.ENUM_STATE.CHANGED
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            End Sub

            Private Sub TextEditorControl_DetectLineCountChange(sender As Object, e As LineCountChangeEventArgs)
                Try
                    Dim iTotalLines As Integer = g_ClassTab.m_TextEditor.Document.TotalNumberOfLines

                    If (e.LinesMoved > -1) Then
                        For i = 0 To e.LinesMoved
                            If (e.LineStart + i > iTotalLines) Then
                                Return
                            End If

                            g_ClassTab.g_ClassLineState.m_LineState(e.LineStart + i) = ClassTextEditorTools.ClassLineStateMark.ENUM_STATE.CHANGED
                        Next
                    End If
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            End Sub

            Private Sub TextEditorControl_Source_SwitchToAutocompleteTabKeyPress(sender As Object, e As KeyPressEventArgs)
                Try
                    SwitchToAutocompleteTab()
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            End Sub

            Private Sub TextEditorControl_Source_UpdateAutocomplete(sender As Object, e As Object)
                Try
                    Static iOldCaretPos As Integer = 0

                    Dim iOffset As Integer = g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset

                    If (iOldCaretPos = iOffset) Then
                        Return
                    End If

                    iOldCaretPos = iOffset

                    g_ClassTab.g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            End Sub

            Private Sub TextEditorControl_Source_SwitchToAutocompleteTab(sender As Object, e As MouseEventArgs)
                Try
                    If (e.Button <> MouseButtons.Left) Then
                        Return
                    End If

                    'Are we inside the editor? Excludes iconbar.
                    If (Not g_ClassTab.m_TextEditor.ActiveTextAreaControl.TextArea.TextView.DrawingPosition.Contains(e.Location)) Then
                        Return
                    End If

                    SwitchToAutocompleteTab()
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            End Sub

            Private Sub TextEditorControl_Source_TextChanged(sender As Object, e As EventArgs)
                Try
                    g_ClassTab.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Changed = True
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            End Sub

            Private Sub TextEditorControl_Source_DoubleClickMarkWord(sender As Object, e As MouseEventArgs)
                Try
                    If (Not ClassSettings.g_bSettingsDoubleClickMark) Then
                        Return
                    End If

                    Dim mActiveTab = g_ClassTab.g_mFormMain.g_ClassTabControl.m_ActiveTab
                    Dim sTextContent As String = mActiveTab.m_TextEditor.Document.TextContent
                    Dim sWord As String = ""

                    If (mActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                        Dim mSelection As ISelection = mActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0)

                        sWord = mSelection.SelectedText
                    Else
                        sWord = g_ClassTab.g_mFormMain.g_ClassTextEditorTools.GetCaretWord(mActiveTab, False, False, False)
                    End If

                    Dim mWordLocations As New List(Of Point)
                    If (Not mActiveTab.g_ClassMarkerHighlighting.FindWordLocations(sWord, sTextContent, mWordLocations)) Then
                        mActiveTab.g_ClassMarkerHighlighting.RemoveHighlighting(ClassTabControl.ClassTab.ClassMarkerHighlighting.ENUM_MARKER_TYPE.STATIC_MARKER)
                        Return
                    End If

                    mActiveTab.g_ClassMarkerHighlighting.UpdateHighlighting(mWordLocations, ClassTabControl.ClassTab.ClassMarkerHighlighting.ENUM_MARKER_TYPE.STATIC_MARKER)
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            End Sub

            Private Sub TextEditorControl_MouseClick(sender As Object, e As MouseEventArgs)
                Dim iOldIndex As Integer = g_ClassTab.m_Index
                Dim iNewIndex As Integer = 0

                Select Case (e.Button)
                    Case MouseButtons.XButton1
                        iNewIndex = iOldIndex - 1

                        If (iNewIndex < 0) Then
                            iNewIndex = g_ClassTab.g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                        End If

                    Case MouseButtons.XButton2
                        iNewIndex = iOldIndex + 1

                        If (iNewIndex > g_ClassTab.g_mFormMain.g_ClassTabControl.m_TabsCount - 1) Then
                            iNewIndex = 0
                        End If

                    Case Else
                        Return
                End Select

                If (iOldIndex <> iNewIndex) Then
                    g_ClassTab.g_mFormMain.g_ClassTabControl.SelectTab(iNewIndex)
                End If
            End Sub

            Private g_mLastMouseLoc As Point = Nothing
            Private g_mLastMouseTime As Date
            Private Sub TextEditorControl_PeekDefinition_Pre(sender As Object, e As MouseEventArgs)
                If (e.Button <> MouseButtons.Left) Then
                    Return
                End If

                g_mLastMouseLoc = New Point(e.X, e.Y)
                g_mLastMouseTime = Now
            End Sub

            Private Sub TextEditorControl_PeekDefinition_Post(sender As Object, e As MouseEventArgs)
                If (Control.ModifierKeys <> Keys.Control OrElse e.Button <> MouseButtons.Left) Then
                    Return
                End If

                If (g_ClassTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                    Return
                End If

                ' Validate if we actualy made a click, not a click-and-drag or something.
                Dim mLastClickDistance = New Point(e.X - g_mLastMouseLoc.X, e.Y - g_mLastMouseLoc.Y)
                If (Math.Abs(mLastClickDistance.X) > SystemInformation.DoubleClickSize.Width / 2 OrElse
                        Math.Abs(mLastClickDistance.Y) > SystemInformation.DoubleClickSize.Height / 2) Then
                    Return
                End If

                Dim iLastClickTime As Long = CLng((Now.Ticks - g_mLastMouseTime.Ticks) / 10000)
                If (iLastClickTime > SystemInformation.DoubleClickTime) Then
                    Return
                End If

                'Assume we moved the caret after the click
                g_ClassTab.g_mFormMain.ToolStripMenuItem_FindDefinition.PerformClick()
            End Sub

            Private Sub SwitchToAutocompleteTab()
                If (Not ClassSettings.g_bSettingsSwitchTabToAutocomplete) Then
                    Return
                End If

                If (g_ClassTab.g_mFormMain.TabControl_Details.SelectedTab Is g_ClassTab.g_mFormMain.TabPage_Autocomplete) Then
                    Return
                End If

                'Make control not auto-selected 
                g_ClassTab.g_mFormMain.TabControl_Details.SelectTabNoFocus(g_ClassTab.g_mFormMain.TabPage_Autocomplete)
            End Sub
        End Class

        Class ClassFoldings
            Private g_ClassTab As ClassTab

            Private g_mFoldingStates As New Dictionary(Of Integer, Boolean)
            Private g_bFoldingStatesLoaded As Boolean = False

            Sub New(c As ClassTab)
                g_ClassTab = c
            End Sub

            ReadOnly Property m_FoldingStates As Dictionary(Of Integer, Boolean)
                Get
                    Return g_mFoldingStates
                End Get
            End Property

            ReadOnly Property m_FoldingStatesLoaded As Boolean
                Get
                    Return g_bFoldingStatesLoaded
                End Get
            End Property

            Public Sub UpdateFoldings()
                'Read saved fold states when generating new foldings
                If (ClassSettings.g_bSettingsRememberFoldings) Then
                    If (Not m_FoldingStatesLoaded) Then
                        GetSavedFoldStates()
                    End If
                End If

                g_ClassTab.g_mSourceTextEditor.Document.FoldingManager.UpdateFoldings(Nothing, m_FoldingStates)
            End Sub

            Public Sub SetSavedFoldStates()
                If (String.IsNullOrEmpty(g_ClassTab.m_File)) Then
                    Return
                End If

                Const E_STATE = 0
                Const E_PSTATE = 1
                Const E_MAX = 1
                Dim mSavedStates As New Dictionary(Of Integer, Object()) '{bState, iAction, bChanged}

                Using mStream = ClassFileStreamWait.Create(g_sFoldingsConfig, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                    Using mIni As New ClassIni(mStream)
                        'Push what already exist into a list
                        For Each mItem In mIni.ReadEverything
                            If (mItem.sSection.ToLower <> g_ClassTab.m_File.ToLower) Then
                                Continue For
                            End If

                            Dim sOffset As String = mItem.sKey
                            Dim sState As String = mItem.sValue

                            Dim iOffset As Integer
                            If (Integer.TryParse(sOffset, iOffset)) Then
                                Dim mObj(E_MAX) As Object

                                mObj(E_STATE) = False
                                mObj(E_PSTATE) = (sState = "1")

                                mSavedStates(iOffset) = mObj
                            End If
                        Next

                        'Compare what changed
                        For Each mFold As FoldMarker In g_ClassTab.m_TextEditor.Document.FoldingManager.FoldMarker
                            Dim mObj(E_MAX) As Object

                            If (mSavedStates.ContainsKey(mFold.Offset)) Then
                                mObj = mSavedStates(mFold.Offset)

                                If (mFold.IsFolded) Then
                                    mObj(E_STATE) = True
                                Else
                                    mObj(E_STATE) = False
                                End If

                                mSavedStates(mFold.Offset) = mObj
                            Else
                                If (mFold.IsFolded) Then
                                    mObj(E_STATE) = True
                                    mObj(E_PSTATE) = False

                                    mSavedStates(mFold.Offset) = mObj
                                End If
                            End If
                        Next

                        Dim lChangedFolds As New List(Of ClassIni.STRUC_INI_CONTENT)

                        'Only write back what changed
                        For Each mItem In mSavedStates
                            Dim iOffset As Integer = mItem.Key
                            Dim bState As Boolean = CBool(mItem.Value(E_STATE))
                            Dim bPreState As Boolean = CBool(mItem.Value(E_PSTATE))

                            Select Case (bState)
                                Case True
                                    If (bState = bPreState) Then
                                        Continue For
                                    End If

                                    lChangedFolds.Add(New ClassIni.STRUC_INI_CONTENT(g_ClassTab.m_File.ToLower, CStr(iOffset), If(bState, "1", "0")))

                                Case Else
                                    If (bState = bPreState) Then
                                        Continue For
                                    End If

                                    lChangedFolds.Add(New ClassIni.STRUC_INI_CONTENT(g_ClassTab.m_File.ToLower, CStr(iOffset), Nothing))

                            End Select
                        Next

                        mIni.WriteKeyValue(lChangedFolds.ToArray)
                    End Using
                End Using
            End Sub

            Public Sub GetSavedFoldStates()
                If (String.IsNullOrEmpty(g_ClassTab.m_File)) Then
                    Return
                End If

                g_mFoldingStates.Clear()

                Using mStream = ClassFileStreamWait.Create(g_sFoldingsConfig, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                    Using mIni As New ClassIni(mStream)
                        For Each mItem In mIni.ReadEverything
                            If (mItem.sSection.ToLower <> g_ClassTab.m_File.ToLower) Then
                                Continue For
                            End If

                            Dim sOffset As String = mItem.sKey
                            Dim sState As String = mItem.sValue

                            Dim iOffset As Integer
                            If (Integer.TryParse(sOffset, iOffset)) Then
                                g_mFoldingStates(iOffset) = (sState = "1")
                            End If
                        Next
                    End Using
                End Using

                g_bFoldingStatesLoaded = True
            End Sub

            Public Sub ClearSavedFoldStates()
                g_mFoldingStates.Clear()
                g_bFoldingStatesLoaded = False
            End Sub

            Public Class ClassFoldingStrategy
                Implements IFoldingStrategy

                Public Function GenerateFoldMarkers(mDoc As IDocument, sFilename As String, mInfo As Object) As List(Of FoldMarker) Implements IFoldingStrategy.GenerateFoldMarkers
                    Dim mFoldStates = TryCast(mInfo, Dictionary(Of Integer, Boolean))

                    'Just create empty dictionary
                    If (mFoldStates Is Nothing) Then
                        mFoldStates = New Dictionary(Of Integer, Boolean)
                    End If

                    Dim mFolds As New List(Of FoldMarker)()

                    Dim iMaxLevels As Integer = 0
                    Dim i As Integer = 0
                    While True
                        i = mDoc.TextContent.IndexOf("{"c, i)
                        If (i < 0) Then
                            Exit While
                        End If

                        i += 1
                        iMaxLevels += 1
                    End While

                    If (iMaxLevels < 1) Then
                        Return mFolds
                    End If

                    Dim iLevels As Integer() = New Integer(iMaxLevels) {}
                    Dim iCurrentLevel As Integer = 0

                    For i = 0 To mDoc.TextContent.Length - 1
                        Select Case (mDoc.TextContent(i))
                            Case "{"c
                                iCurrentLevel += 1
                                If ((iCurrentLevel - 1) < 0) Then
                                    Continue For
                                End If

                                iLevels(iCurrentLevel - 1) = i
                            Case "}"c
                                iCurrentLevel -= 1
                                If (iCurrentLevel < 0) Then
                                    Continue For
                                End If

                                Dim iLineStart = mDoc.GetLineNumberForOffset(iLevels(iCurrentLevel))
                                Dim iColumStart = mDoc.GetLineSegment(iLineStart).Length
                                Dim iLineEnd = mDoc.GetLineNumberForOffset(i)
                                Dim iColumEnd = mDoc.GetLineSegment(iLineEnd).Length

                                If (iLineStart = iLineEnd) Then
                                    Continue For
                                End If

                                Dim mFold = New FoldMarker(mDoc, iLineStart, iColumStart, iLineEnd, iColumEnd)

                                If (mFoldStates.ContainsKey(mFold.Offset)) Then
                                    mFold.IsFolded = mFoldStates(mFold.Offset)
                                End If

                                mFolds.Add(mFold)
                        End Select
                    Next

                    Return mFolds
                End Function
            End Class
        End Class

        Class ClassLineState
            Private g_ClassTab As ClassTab

            Private g_iIgnoreCount As Integer = 0
            Private g_qLineStateHistory As New Queue(Of ClassTextEditorTools.ClassLineStateMark)

            Public Sub New(c As ClassTab)
                g_ClassTab = c
            End Sub

            Public Sub BeginIgnore()
                g_iIgnoreCount += 1
            End Sub

            Public Sub EndIgnore()
                g_iIgnoreCount -= 1

                If (g_iIgnoreCount < 0) Then
                    g_iIgnoreCount = 0
                End If
            End Sub

            ReadOnly Property m_LineStateMarks() As ClassTextEditorTools.ClassLineStateMark()
                Get
                    Dim lMarks As New List(Of ClassTextEditorTools.ClassLineStateMark)

                    For i = 0 To g_ClassTab.m_TextEditor.Document.BookmarkManager.Marks.Count - 1
                        If (TypeOf g_ClassTab.m_TextEditor.Document.BookmarkManager.Marks(i) IsNot ClassTextEditorTools.ClassLineStateMark) Then
                            Continue For
                        End If

                        lMarks.Add(DirectCast(g_ClassTab.m_TextEditor.Document.BookmarkManager.Marks(i), ClassTextEditorTools.ClassLineStateMark))
                    Next

                    Return lMarks.ToArray
                End Get
            End Property

            Property m_LineState(iIndex As Integer) As ClassTextEditorTools.ClassLineStateMark.ENUM_STATE
                Get
                    For Each mMark In g_ClassTab.m_TextEditor.Document.BookmarkManager.Marks
                        Dim mLineStateMark = TryCast(mMark, ClassTextEditorTools.ClassLineStateMark)
                        If (mLineStateMark Is Nothing OrElse mLineStateMark.Anchor.IsDeleted) Then
                            Continue For
                        End If

                        If (mLineStateMark.LineNumber <> iIndex) Then
                            Continue For
                        End If

                        Return mLineStateMark.m_Type
                    Next

                    Return ClassTextEditorTools.ClassLineStateMark.ENUM_STATE.NONE
                End Get
                Set(value As ClassTextEditorTools.ClassLineStateMark.ENUM_STATE)
                    If (g_iIgnoreCount > 0) Then
                        Return
                    End If

                    For i = g_ClassTab.m_TextEditor.Document.BookmarkManager.Marks.Count - 1 To 0 Step -1
                        Dim mLineStateMark = TryCast(g_ClassTab.m_TextEditor.Document.BookmarkManager.Marks(i), ClassTextEditorTools.ClassLineStateMark)
                        If (mLineStateMark Is Nothing OrElse mLineStateMark.Anchor.IsDeleted) Then
                            Continue For
                        End If

                        If (mLineStateMark.LineNumber <> iIndex) Then
                            Continue For
                        End If

                        Select Case (ClassSettings.g_iSettingsIconLineStateType)
                            Case ClassSettings.ENUM_LINE_STATE_TYPE.CHANGED_AND_SAVED
                                mLineStateMark.m_Type = value

                            Case ClassSettings.ENUM_LINE_STATE_TYPE.CHANGED
                                'Remove saved
                                If (mLineStateMark.m_Type = ClassTextEditorTools.ClassLineStateMark.ENUM_STATE.SAVED) Then
                                    g_ClassTab.m_TextEditor.Document.BookmarkManager.RemoveMark(mLineStateMark)
                                Else
                                    mLineStateMark.m_Type = value
                                End If

                            Case ClassSettings.ENUM_LINE_STATE_TYPE.NONE
                                g_ClassTab.m_TextEditor.Document.BookmarkManager.RemoveMark(mLineStateMark)
                        End Select

                        g_ClassTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, iIndex))
                        g_ClassTab.m_TextEditor.Document.CommitUpdate()

                        LimitStates()
                        Return
                    Next

                    If (ClassSettings.g_iSettingsIconLineStateType = ClassSettings.ENUM_LINE_STATE_TYPE.NONE) Then
                        Return
                    End If

                    Dim mNewMark As New ClassTextEditorTools.ClassLineStateMark(g_ClassTab.m_TextEditor.Document, New TextLocation(0, iIndex), ClassTextEditorTools.ClassLineStateMark.ENUM_STATE.CHANGED)
                    g_ClassTab.m_TextEditor.Document.BookmarkManager.AddMark(mNewMark)
                    g_qLineStateHistory.Enqueue(mNewMark)

                    g_ClassTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, iIndex))
                    g_ClassTab.m_TextEditor.Document.CommitUpdate()

                    LimitStates()
                End Set
            End Property

            ''' <summary>
            ''' Change all 'changed' states to 'saved'.
            ''' </summary>
            Public Sub SaveStates()
                For i = g_ClassTab.m_TextEditor.Document.BookmarkManager.Marks.Count - 1 To 0 Step -1
                    Dim mLineStateMark As ClassTextEditorTools.ClassLineStateMark = TryCast(g_ClassTab.m_TextEditor.Document.BookmarkManager.Marks(i), ClassTextEditorTools.ClassLineStateMark)
                    If (mLineStateMark Is Nothing) Then
                        Continue For
                    End If

                    Select Case (ClassSettings.g_iSettingsIconLineStateType)
                        Case ClassSettings.ENUM_LINE_STATE_TYPE.NONE
                            g_ClassTab.m_TextEditor.Document.BookmarkManager.RemoveMark(mLineStateMark)
                            g_ClassTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, mLineStateMark.LineNumber))

                        Case ClassSettings.ENUM_LINE_STATE_TYPE.CHANGED_AND_SAVED
                            If (mLineStateMark.m_Type = ClassTextEditorTools.ClassLineStateMark.ENUM_STATE.CHANGED) Then
                                mLineStateMark.m_Type = ClassTextEditorTools.ClassLineStateMark.ENUM_STATE.SAVED
                                g_ClassTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, mLineStateMark.LineNumber))
                            End If

                        Case ClassSettings.ENUM_LINE_STATE_TYPE.CHANGED
                            If (mLineStateMark.m_Type = ClassTextEditorTools.ClassLineStateMark.ENUM_STATE.CHANGED) Then
                                g_ClassTab.m_TextEditor.Document.BookmarkManager.RemoveMark(mLineStateMark)
                                g_ClassTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, mLineStateMark.LineNumber))
                            End If
                    End Select
                Next

                g_ClassTab.m_TextEditor.Document.CommitUpdate()
            End Sub

            ''' <summary>
            ''' Updates all line states by settings.
            ''' </summary> 
            Public Sub UpdateStates()
                For i = g_ClassTab.m_TextEditor.Document.BookmarkManager.Marks.Count - 1 To 0 Step -1
                    Dim mLineStateMark = TryCast(g_ClassTab.m_TextEditor.Document.BookmarkManager.Marks(i), ClassTextEditorTools.ClassLineStateMark)
                    If (mLineStateMark Is Nothing) Then
                        Continue For
                    End If

                    'Remove already invalid/removed marks
                    If (mLineStateMark.Anchor.IsDeleted) Then
                        g_ClassTab.m_TextEditor.Document.BookmarkManager.RemoveMark(mLineStateMark)
                        Continue For
                    End If

                    Select Case (ClassSettings.g_iSettingsIconLineStateType)
                        Case ClassSettings.ENUM_LINE_STATE_TYPE.CHANGED
                            If (mLineStateMark.m_Type = ClassTextEditorTools.ClassLineStateMark.ENUM_STATE.SAVED) Then
                                g_ClassTab.m_TextEditor.Document.BookmarkManager.RemoveMark(mLineStateMark)
                                g_ClassTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, mLineStateMark.LineNumber))
                            End If

                        Case ClassSettings.ENUM_LINE_STATE_TYPE.NONE
                            g_ClassTab.m_TextEditor.Document.BookmarkManager.RemoveMark(mLineStateMark)
                            g_ClassTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, mLineStateMark.LineNumber))

                    End Select
                Next

                g_ClassTab.m_TextEditor.Document.CommitUpdate()
            End Sub

            ''' <summary>
            ''' Clear all line states. Like on loading or new source.
            ''' </summary>
            Public Sub ClearStates()
                g_ClassTab.m_TextEditor.Document.BookmarkManager.RemoveMarks(Function(mBookmark As Bookmark) TypeOf mBookmark Is ClassTextEditorTools.ClassLineStateMark)
            End Sub

            ''' <summary>
            ''' Limits all line states by settings. To avoid overdrawing.
            ''' </summary> 
            Public Sub LimitStates()
                If (ClassSettings.g_iSettingsIconLineStateMax < 1) Then
                    Return
                End If

                While (g_qLineStateHistory.Count > ClassSettings.g_iSettingsIconLineStateMax)
                    Dim mLineStateMark = g_qLineStateHistory.Dequeue
                    If (mLineStateMark Is Nothing) Then
                        Continue While
                    End If

                    'Remove already invalid/removed marks
                    If (mLineStateMark.Anchor.IsDeleted) Then
                        g_ClassTab.m_TextEditor.Document.BookmarkManager.RemoveMark(mLineStateMark)
                        Continue While
                    End If

                    g_ClassTab.m_TextEditor.Document.BookmarkManager.RemoveMark(mLineStateMark)
                    g_ClassTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, mLineStateMark.LineNumber))
                End While

                g_ClassTab.m_TextEditor.Document.CommitUpdate()
            End Sub
        End Class

        Class ClassScopeHighlighting
            Private g_ClassTab As ClassTab

            Private g_mTextMarker As TextMarker = Nothing

            Public Sub New(c As ClassTab)
                g_ClassTab = c
            End Sub

            Public Sub UpdateHighlighting()
                Dim sText As String = g_ClassTab.m_TextEditor.Document.TextContent
                Dim iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = g_ClassTab.m_Language
                Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sText, iLanguage)

                Dim mOldTextMarker = g_mTextMarker
                Dim iCaretOffset = g_ClassTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset

                Dim mScopeLocation As Point
                If (Not FindScopeLocation(mSourceAnalysis, iCaretOffset, True, mScopeLocation)) Then
                    RemoveHighlighting()
                    Return
                End If

                UpdateHighlighting(mScopeLocation)
            End Sub

            Public Sub UpdateHighlighting(mScopeLocation As Point)
                Dim mOldTextMarker = g_mTextMarker
                Dim mColor = g_ClassTab.m_TextEditor.Document.HighlightingStrategy.GetColorFor("ScopeMarker").Color

                If (g_mTextMarker Is Nothing) Then
                    g_mTextMarker = New TextMarker(mScopeLocation.X, mScopeLocation.Y, TextMarkerType.SolidBlock, mColor)
                Else
                    Dim mOldLocation = New Point(g_mTextMarker.Offset, g_mTextMarker.Offset + g_mTextMarker.Length)

                    If (mScopeLocation.X <> mOldLocation.X OrElse mScopeLocation.Y <> mOldLocation.Y OrElse g_mTextMarker.Color <> mColor) Then
                        g_mTextMarker = New TextMarker(mScopeLocation.X, mScopeLocation.Y, TextMarkerType.SolidBlock, mColor)
                    End If
                End If

                If (g_mTextMarker Is Nothing) Then
                    Return
                End If

                g_ClassTab.m_TextEditor.Document.MarkerStrategy.RemoveAll(Function(x As TextMarker) x Is mOldTextMarker)
                g_ClassTab.m_TextEditor.Document.MarkerStrategy.AddMarker(g_mTextMarker)

                g_ClassTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.WholeTextArea))
                g_ClassTab.m_TextEditor.Document.CommitUpdate()
            End Sub

            Public Sub RemoveHighlighting()
                If (g_mTextMarker Is Nothing) Then
                    Return
                End If

                g_ClassTab.m_TextEditor.Document.MarkerStrategy.RemoveAll(Function(x As TextMarker) x Is g_mTextMarker)
                g_mTextMarker = Nothing

                g_ClassTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.WholeTextArea))
                g_ClassTab.m_TextEditor.Document.CommitUpdate()
            End Sub

            Public Function FindScopeLocation(mSourceAnalysis As ClassSyntaxTools.ClassSyntaxSourceAnalysis, iCaretOffset As Integer, bCaretAfter As Boolean, ByRef r_ScopeLocation As Point) As Boolean
                r_ScopeLocation = Nothing
                iCaretOffset = If(bCaretAfter, iCaretOffset - 1, iCaretOffset)

                Dim iSuccess As Integer = 0
                Dim iStartScope As Integer = 0
                Dim iEndScope As Integer = 0

                Dim iType As Integer = -1

                If (Not mSourceAnalysis.m_InRange(iCaretOffset) OrElse mSourceAnalysis.m_InNonCode(iCaretOffset)) Then
                    Return False
                End If

                Select Case (mSourceAnalysis.GetChar(iCaretOffset))
                    Case "{"c, "}"c
                        iType = 0

                    Case "("c, ")"c
                        iType = 1

                    Case "["c, "]"c
                        iType = 2

                    Case Else
                        Return False
                End Select

                If (True) Then
                    Dim iStartLevel As Integer
                    Select Case (iType)
                        Case 0
                            iStartLevel = mSourceAnalysis.GetBraceLevel(iCaretOffset, Nothing)
                        Case 1
                            iStartLevel = mSourceAnalysis.GetParenthesisLevel(iCaretOffset, Nothing)
                        Case 2
                            iStartLevel = mSourceAnalysis.GetBracketLevel(iCaretOffset, Nothing)
                        Case Else
                            Return False
                    End Select

                    If (iStartLevel = 0) Then
                        Return False
                    End If

                    For i = iCaretOffset To 0 Step -1
                        If (mSourceAnalysis.m_InNonCode(i)) Then
                            Continue For
                        End If

                        Dim iCurrentLevel As Integer
                        Select Case (iType)
                            Case 0
                                iCurrentLevel = mSourceAnalysis.GetBraceLevel(i, Nothing)
                            Case 1
                                iCurrentLevel = mSourceAnalysis.GetParenthesisLevel(i, Nothing)
                            Case 2
                                iCurrentLevel = mSourceAnalysis.GetBracketLevel(i, Nothing)
                            Case Else
                                Return False
                        End Select

                        If (iCurrentLevel = iStartLevel - 1) Then
                            iStartScope = (i + 1)
                            iSuccess += 1
                            Exit For
                        End If
                    Next
                End If

                If (True) Then
                    Dim iEndLevel As Integer
                    Select Case (iType)
                        Case 0
                            iEndLevel = mSourceAnalysis.GetBraceLevel(iCaretOffset, Nothing)
                        Case 1
                            iEndLevel = mSourceAnalysis.GetParenthesisLevel(iCaretOffset, Nothing)
                        Case 2
                            iEndLevel = mSourceAnalysis.GetBracketLevel(iCaretOffset, Nothing)
                        Case Else
                            Return False
                    End Select

                    If (iEndLevel = 0) Then
                        Return False
                    End If

                    For i = iCaretOffset To mSourceAnalysis.m_MaxLength 'Go over max length by 1
                        If (mSourceAnalysis.m_InNonCode(i)) Then
                            Continue For
                        End If

                        Dim iCurrentLevel As Integer
                        Select Case (iType)
                            Case 0
                                iCurrentLevel = mSourceAnalysis.GetBraceLevel(i, Nothing)
                            Case 1
                                iCurrentLevel = mSourceAnalysis.GetParenthesisLevel(i, Nothing)
                            Case 2
                                iCurrentLevel = mSourceAnalysis.GetBracketLevel(i, Nothing)
                            Case Else
                                Return False
                        End Select

                        If (iCurrentLevel = iEndLevel - 1) Then
                            iEndScope = i
                            iSuccess += 1
                            Exit For
                        End If
                    Next
                End If

                If (iSuccess = 2) Then
                    r_ScopeLocation = New Point(iStartScope, Math.Min(iEndScope - iStartScope, mSourceAnalysis.m_MaxLength - 1))
                    Return True
                Else
                    Return False
                End If
            End Function
        End Class

        Class ClassMarkerHighlighting
            Private g_ClassTab As ClassTab

            Private g_lTextMarkers As New List(Of IMarkers)

            Enum ENUM_MARKER_TYPE
                STATIC_MARKER
                CARET_MARKER
            End Enum

            Public Sub New(c As ClassTab)
                g_ClassTab = c
            End Sub

            Public Sub UpdateHighlighting(sWord As String, iType As ENUM_MARKER_TYPE)
                Dim sText As String = g_ClassTab.m_TextEditor.Document.TextContent

                Dim mWordLocations As New List(Of Point)
                If (Not FindWordLocations(sWord, sText, mWordLocations)) Then
                    RemoveHighlighting(iType)
                    Return
                End If

                UpdateHighlighting(mWordLocations, iType)
            End Sub

            Public Sub UpdateHighlighting(mWordLocations As List(Of Point), iType As ENUM_MARKER_TYPE)
                Dim mFrontColor As Color
                Dim mBackColor As Color

                Dim mStrategy = DirectCast(g_ClassTab.m_TextEditor.Document.HighlightingStrategy, DefaultHighlightingStrategy)

                Select Case (iType)
                    Case ENUM_MARKER_TYPE.STATIC_MARKER
                        mFrontColor = g_ClassTab.m_TextEditor.Document.HighlightingStrategy.GetColorFor("StaticWordMarker").Color
                        mBackColor = g_ClassTab.m_TextEditor.Document.HighlightingStrategy.GetColorFor("StaticWordMarker").BackgroundColor

                    Case Else
                        mFrontColor = g_ClassTab.m_TextEditor.Document.HighlightingStrategy.GetColorFor("CaretWordMarker").Color
                        mBackColor = g_ClassTab.m_TextEditor.Document.HighlightingStrategy.GetColorFor("CaretWordMarker").BackgroundColor
                End Select

                Dim mMarkers As New List(Of IMarkers)

                For Each mPoint As Point In mWordLocations.ToArray
                    Select Case (iType)
                        Case ENUM_MARKER_TYPE.STATIC_MARKER
                            mMarkers.Add(New IMarkers.ClassStaticMarker(mPoint.X, mPoint.Y, TextMarkerType.SolidBlock, mBackColor, mFrontColor))
                        Case Else
                            mMarkers.Add(New IMarkers.ClassCaretMarker(mPoint.X, mPoint.Y, TextMarkerType.SolidBlock, mBackColor, mFrontColor))
                    End Select
                Next

                If (mMarkers.Count < 1) Then
                    RemoveHighlighting(iType)
                    Return
                End If

                Select Case (iType)
                    Case ENUM_MARKER_TYPE.STATIC_MARKER
                        g_ClassTab.m_TextEditor.Document.MarkerStrategy.RemoveAll(Function(x As TextMarker) TypeOf x Is IMarkers.ClassStaticMarker)
                        g_lTextMarkers.RemoveAll(Function(x As IMarkers) TypeOf x Is IMarkers.ClassStaticMarker)

                    Case Else
                        g_ClassTab.m_TextEditor.Document.MarkerStrategy.RemoveAll(Function(x As TextMarker) TypeOf x Is IMarkers.ClassCaretMarker)
                        g_lTextMarkers.RemoveAll(Function(x As IMarkers) TypeOf x Is IMarkers.ClassCaretMarker)
                End Select

                For Each mItem In mMarkers.ToArray
                    g_ClassTab.m_TextEditor.Document.MarkerStrategy.InsertMarker(0, DirectCast(mItem, TextMarker)) 'Make it highest rendering priority
                    g_lTextMarkers.Add(mItem)
                Next

                g_ClassTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.WholeTextArea))
                g_ClassTab.m_TextEditor.Document.CommitUpdate()
            End Sub

            Public Sub RemoveHighlighting(iType As ENUM_MARKER_TYPE)
                If (g_lTextMarkers.Count < 1) Then
                    Return
                End If

                Select Case (iType)
                    Case ENUM_MARKER_TYPE.STATIC_MARKER
                        g_ClassTab.m_TextEditor.Document.MarkerStrategy.RemoveAll(Function(x As TextMarker) TypeOf x Is IMarkers.ClassStaticMarker)
                        g_lTextMarkers.RemoveAll(Function(x As IMarkers) TypeOf x Is IMarkers.ClassStaticMarker)

                    Case Else
                        g_ClassTab.m_TextEditor.Document.MarkerStrategy.RemoveAll(Function(x As TextMarker) TypeOf x Is IMarkers.ClassCaretMarker)
                        g_lTextMarkers.RemoveAll(Function(x As IMarkers) TypeOf x Is IMarkers.ClassCaretMarker)
                End Select

                g_ClassTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.WholeTextArea))
                g_ClassTab.m_TextEditor.Document.CommitUpdate()
            End Sub

            Public Function FindWordLocations(sWord As String, sText As String, ByRef r_WordLocations As List(Of Point)) As Boolean
                r_WordLocations = New List(Of Point)

                If (Not Regex.IsMatch(sWord, "^[a-zA-Z0-9_]+$")) Then
                    Return False
                End If

                For Each mMatch As Match In Regex.Matches(sText, String.Format("\b{0}\b", Regex.Escape(sWord)), RegexOptions.Multiline)
                    r_WordLocations.Add(New Point(mMatch.Index, mMatch.Length))
                Next

                Return (r_WordLocations.Count > 0)
            End Function

            Interface IMarkers
                Class ClassStaticMarker
                    Inherits TextMarker
                    Implements IMarkers

                    Public Sub New(offset As Integer, length As Integer, textMarkerType As TextMarkerType, color As Color)
                        MyBase.New(offset, length, textMarkerType, color)
                    End Sub

                    Public Sub New(offset As Integer, length As Integer, textMarkerType As TextMarkerType, color As Color, foreColor As Color)
                        MyBase.New(offset, length, textMarkerType, color, foreColor)
                    End Sub
                End Class

                Class ClassCaretMarker
                    Inherits TextMarker
                    Implements IMarkers

                    Public Sub New(offset As Integer, length As Integer, textMarkerType As TextMarkerType, color As Color)
                        MyBase.New(offset, length, textMarkerType, color)
                    End Sub

                    Public Sub New(offset As Integer, length As Integer, textMarkerType As TextMarkerType, color As Color, foreColor As Color)
                        MyBase.New(offset, length, textMarkerType, color, foreColor)
                    End Sub
                End Class
            End Interface
        End Class

        Protected Overrides Sub Dispose(disposing As Boolean)
            Try
                If (disposing) Then
                    RemoveHandlers()

                    If (g_mSourceTextEditor IsNot Nothing AndAlso Not g_mSourceTextEditor.IsDisposed) Then
                        g_mSourceTextEditor.Dispose()
                        g_mSourceTextEditor = Nothing
                    End If
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub
    End Class

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                RemoveHandler g_mFormMain.TabControl_SourceTabs.SelectedIndexChanged, AddressOf OnTabSelected

                If (g_mFormMain.g_ClassSyntaxParser IsNot Nothing) Then
                    RemoveHandler g_mFormMain.g_ClassSyntaxParser.OnSyntaxParseSuccess, AddressOf OnTabSyntaxParseSuccess
                End If

                If (g_mTimer IsNot Nothing) Then
                    g_mTimer.Dispose()
                    g_mTimer = Nothing
                End If
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class