'BasicPawn
'Copyright(C) 2017 TheTimocop

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
    Private g_lBeginRequestFullUpdateTabs As New List(Of SourceTabPage)


    Public Sub New(f As FormMain)
        g_mFormMain = f

        RemoveHandler g_mFormMain.TabControl_SourceTabs.SelectedIndexChanged, AddressOf OnTabSelected
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
    End Sub

    ReadOnly Property m_ActiveTab As SourceTabPage
        Get
            Return TryCast(g_mFormMain.TabControl_SourceTabs.SelectedTab, SourceTabPage)
        End Get
    End Property

    ReadOnly Property m_ActiveTabIndex As Integer
        Get
            Return g_mFormMain.TabControl_SourceTabs.SelectedIndex
        End Get
    End Property

    ReadOnly Property m_Tab(iIndex As Integer) As SourceTabPage
        Get
            Return TryCast(g_mFormMain.TabControl_SourceTabs.TabPages(iIndex), SourceTabPage)
        End Get
    End Property

    ReadOnly Property m_TabsCount As Integer
        Get
            Return g_mFormMain.TabControl_SourceTabs.TabCount
        End Get
    End Property

    Public Function GetAllTabs() As SourceTabPage()
        Dim lTabs As New List(Of SourceTabPage)

        For i = 0 To m_TabsCount - 1
            lTabs.Add(m_Tab(i))
        Next

        Return lTabs.ToArray
    End Function

    Public Function AddTab(Optional bSelect As Boolean = False, Optional bShowTemplateWizard As Boolean = False, Optional bChanged As Boolean = False, Optional bDontRecycleTabs As Boolean = False) As SourceTabPage
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

            Dim mRecycleTab As SourceTabPage = Nothing

            'Recycle first unsaved tab
            If (Not bDontRecycleTabs AndAlso m_TabsCount = 1) Then
                If (m_Tab(0).m_IsUnsaved AndAlso Not m_Tab(0).m_Changed) Then
                    mRecycleTab = m_Tab(0)
                End If
            End If

            Dim mTabPage As New SourceTabPage(g_mFormMain) With {
                .m_Changed = False
            }

            While True
                For Each mConfig In ClassConfigs.GetConfigs(False)
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

            Return mTabPage
        Finally
            EndUpdate()
        End Try
    End Function

    Public Function RemoveTab(iIndex As Integer, bPrompSave As Boolean, Optional iSelectTabIndex As Integer = -1) As Boolean
        Try
            BeginUpdate()

            If (bPrompSave AndAlso PromptSaveTab(iIndex)) Then
                Return False
            End If

            Dim mTabPage = m_Tab(iIndex)
            mTabPage.Dispose()
            mTabPage = Nothing

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

    Public Sub SwapTabs(iFromIndex As Integer, iToIndex As Integer)
        Try
            BeginUpdate()

            g_bIgnoreOnTabSelected = True

            Dim mFrom As SourceTabPage = m_Tab(iFromIndex)
            g_mFormMain.TabControl_SourceTabs.TabPages.Remove(mFrom)
            g_mFormMain.TabControl_SourceTabs.TabPages.Insert(iToIndex, mFrom)

            g_bIgnoreOnTabSelected = False

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

                g_mFormMain.TabControl_SourceTabs.SelectTab(iIndex)

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

    Public Function GetTabByIdentifier(sIdentifier As String) As SourceTabPage
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
    Public Function OpenFileTab(iIndex As Integer, sFile As String, Optional bIgnoreSavePrompt As Boolean = False) As Boolean
        If (Not bIgnoreSavePrompt AndAlso PromptSaveTab(iIndex)) Then
            Return False
        End If

        If (String.IsNullOrEmpty(sFile) OrElse Not IO.File.Exists(sFile)) Then
            m_Tab(iIndex).m_ClassLineState.m_IgnoreUpdates = True

            m_Tab(iIndex).m_FileCachedWriteDate = Date.MaxValue
            m_Tab(iIndex).m_File = ""
            m_Tab(iIndex).m_TextEditor.Document.TextContent = ""

            m_Tab(iIndex).m_Changed = False
            m_Tab(iIndex).m_AutocompleteItems.Clear()
            m_Tab(iIndex).m_IncludeFiles.Clear()
            m_Tab(iIndex).m_FileCachedWriteDate = Now

            m_Tab(iIndex).m_ActiveConfig = Nothing

            m_Tab(iIndex).m_ClassLineState.m_IgnoreUpdates = False

            If (g_iBeginUpdateCount > 0) Then
                g_bBeginRequestFullUpdate = True
                g_lBeginRequestFullUpdateTabs.Add(m_Tab(iIndex))
            Else
                FullUpdate(m_Tab(iIndex))
            End If

            If (g_mFormMain.g_mUCStartPage.Visible) Then
                g_mFormMain.g_mUCStartPage.Hide()
            End If

            g_mFormMain.PrintInformation("[INFO]", "User created a new source file")
            Return False
        End If


        Dim sFileText As String = IO.File.ReadAllText(sFile)

        m_Tab(iIndex).m_ClassLineState.m_IgnoreUpdates = True

        m_Tab(iIndex).m_FileCachedWriteDate = Date.MaxValue
        m_Tab(iIndex).m_File = sFile
        m_Tab(iIndex).m_TextEditor.Document.TextContent = sFileText

        m_Tab(iIndex).m_Changed = False
        m_Tab(iIndex).m_AutocompleteItems.Clear()
        m_Tab(iIndex).m_IncludeFiles.Clear()
        m_Tab(iIndex).m_FileCachedWriteDate = m_Tab(iIndex).m_FileRealWriteDate

        Dim mKnownConfig = ClassConfigs.ClassKnownConfigs.m_KnownConfigByFile(sFile)
        If (mKnownConfig Is Nothing) Then
            While True
                For Each mConfig In ClassConfigs.GetConfigs(False)
                    If (mConfig.g_bAutoload) Then
                        m_Tab(iIndex).m_ActiveConfig = mConfig
                        Exit While
                    End If
                Next

                m_Tab(iIndex).m_ActiveConfig = Nothing
                Exit While
            End While
        Else
            m_Tab(iIndex).m_ActiveConfig = mKnownConfig
        End If

        m_Tab(iIndex).m_ClassLineState.m_IgnoreUpdates = False

        If (g_iBeginUpdateCount > 0) Then
            g_bBeginRequestFullUpdate = True
            g_lBeginRequestFullUpdateTabs.Add(m_Tab(iIndex))
        Else
            FullUpdate(m_Tab(iIndex))
        End If

        If (g_mFormMain.g_mUCStartPage.Visible) Then
            g_mFormMain.g_mUCStartPage.Hide()
        End If

        g_mFormMain.PrintInformation("[INFO]", "User opened a new file: " & sFile)
        Return True
    End Function

    ''' <summary>
    ''' Saves a source file
    ''' </summary>
    ''' <param name="iIndex"></param>
    ''' <param name="bSaveAs">Force to use a new file using SaveFileDialog</param>
    Public Sub SaveFileTab(iIndex As Integer, Optional bSaveAs As Boolean = False)
        If (bSaveAs OrElse m_Tab(iIndex).m_IsUnsaved OrElse m_Tab(iIndex).m_InvalidFile) Then
            Using i As New SaveFileDialog
                i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|AMX Mod X|*.sma|Pawn (Not fully supported)|*.pwn;*.p|All files|*.*"
                i.FileName = m_Tab(iIndex).m_File

                If (i.ShowDialog = DialogResult.OK) Then
                    m_Tab(iIndex).m_FileCachedWriteDate = Date.MaxValue
                    m_Tab(iIndex).m_File = i.FileName

                    m_Tab(iIndex).m_Changed = False
                    m_Tab(iIndex).m_ClassLineState.SaveStates(m_Tab(iIndex).m_TextEditor)
                    m_Tab(iIndex).m_TextEditor.Refresh()

                    g_mFormMain.PrintInformation("[INFO]", "User saved file to: " & m_Tab(iIndex).m_File)
                    IO.File.WriteAllText(m_Tab(iIndex).m_File, m_Tab(iIndex).m_TextEditor.Document.TextContent)

                    m_Tab(iIndex).m_FileCachedWriteDate = m_Tab(iIndex).m_FileRealWriteDate

                    g_mFormMain.g_mUCStartPage.g_mClassRecentItems.AddRecent(m_Tab(iIndex).m_File)
                    'g_mFormMain.ShowPingFlash()

                    g_mFormMain.g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, Nothing)
                End If
            End Using
        Else
            m_Tab(iIndex).m_FileCachedWriteDate = Date.MaxValue
            m_Tab(iIndex).m_Changed = False
            m_Tab(iIndex).m_ClassLineState.SaveStates(m_Tab(iIndex).m_TextEditor)
            m_Tab(iIndex).m_TextEditor.Refresh()

            g_mFormMain.PrintInformation("[INFO]", "User saved file to: " & m_Tab(iIndex).m_File)
            IO.File.WriteAllText(m_Tab(iIndex).m_File, m_Tab(iIndex).m_TextEditor.Document.TextContent)

            m_Tab(iIndex).m_FileCachedWriteDate = m_Tab(iIndex).m_FileRealWriteDate

            g_mFormMain.g_mUCStartPage.g_mClassRecentItems.AddRecent(m_Tab(iIndex).m_File)
            'g_mFormMain.ShowPingFlash()
        End If
    End Sub

    ''' <summary>
    ''' If the code has been changed it will prompt the user and saves the source. The user can abort the saving.
    ''' </summary>
    ''' <param name="iIndex"></param>
    ''' <param name="bAlwaysPrompt">If true, always show MessageBox even if the code didnt change</param>
    ''' <param name="bAlwaysYes">If true, ignores MessageBox prompt</param>
    ''' <returns>False if saved, otherwise canceled.</returns>
    Public Function PromptSaveTab(iIndex As Integer, Optional bAlwaysPrompt As Boolean = False, Optional bAlwaysYes As Boolean = False, Optional bAlwaysSaveUnsaved As Boolean = False) As Boolean
        Dim bIsUnsaved As Boolean = (m_Tab(iIndex).m_IsUnsaved OrElse m_Tab(iIndex).m_InvalidFile)

        If (bAlwaysPrompt OrElse m_Tab(iIndex).m_Changed OrElse (bAlwaysSaveUnsaved AndAlso bIsUnsaved)) Then
            'Continue
        Else
            Return False
        End If

        Select Case (If(bAlwaysYes, DialogResult.Yes, MessageBox.Show(String.Format("Do you want to save your work? ({0})", m_Tab(iIndex).m_Title), "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)))
            Case DialogResult.Yes
                If (bIsUnsaved) Then
                    Using i As New SaveFileDialog
                        i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|AMX Mod X|*.sma|Pawn (Not fully supported)|*.pwn;*.p|All files|*.*"
                        i.FileName = m_Tab(iIndex).m_File

                        If (i.ShowDialog = DialogResult.OK) Then
                            m_Tab(iIndex).m_FileCachedWriteDate = Date.MaxValue
                            m_Tab(iIndex).m_File = i.FileName

                            m_Tab(iIndex).m_Changed = False
                            m_Tab(iIndex).m_ClassLineState.SaveStates(m_Tab(iIndex).m_TextEditor)
                            m_Tab(iIndex).m_TextEditor.Refresh()

                            g_mFormMain.PrintInformation("[INFO]", "User saved file to: " & m_Tab(iIndex).m_File)
                            IO.File.WriteAllText(m_Tab(iIndex).m_File, m_Tab(iIndex).m_TextEditor.Document.TextContent)

                            m_Tab(iIndex).m_FileCachedWriteDate = m_Tab(iIndex).m_FileRealWriteDate

                            g_mFormMain.g_mUCStartPage.g_mClassRecentItems.AddRecent(m_Tab(iIndex).m_File)
                            'g_mFormMain.ShowPingFlash()

                            Return False
                        Else
                            Return True
                        End If
                    End Using
                Else
                    m_Tab(iIndex).m_FileCachedWriteDate = Date.MaxValue
                    m_Tab(iIndex).m_Changed = False
                    m_Tab(iIndex).m_ClassLineState.SaveStates(m_Tab(iIndex).m_TextEditor)
                    m_Tab(iIndex).m_TextEditor.Refresh()

                    g_mFormMain.PrintInformation("[INFO]", "User saved file to: " & m_Tab(iIndex).m_File)
                    IO.File.WriteAllText(m_Tab(iIndex).m_File, m_Tab(iIndex).m_TextEditor.Document.TextContent)

                    m_Tab(iIndex).m_FileCachedWriteDate = m_Tab(iIndex).m_FileRealWriteDate

                    g_mFormMain.g_mUCStartPage.g_mClassRecentItems.AddRecent(m_Tab(iIndex).m_File)
                    'g_mFormMain.ShowPingFlash()

                    Return False
                End If

            Case DialogResult.No
                Return False

            Case Else
                Return True

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
        For i = 0 To m_TabsCount - 1
            If (m_Tab(i).m_IsUnsaved OrElse m_Tab(i).m_InvalidFile) Then
                Continue For
            End If

            If (Not m_Tab(i).m_IsFileNewer) Then
                Continue For
            End If

            Select Case (MessageBox.Show(String.Format("'{0}' in tab '{1}' has been changed! Do you want to reload the tab?", m_Tab(i).m_File, i + 1), "File changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
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
    End Sub

    Public Function GetTabByCursorPoint() As SourceTabPage
        For i = 0 To g_mFormMain.TabControl_SourceTabs.TabPages.Count - 1
            If (g_mFormMain.TabControl_SourceTabs.GetTabRect(i).Contains(g_mFormMain.TabControl_SourceTabs.PointToClient(Cursor.Position))) Then
                Return DirectCast(g_mFormMain.TabControl_SourceTabs.TabPages(i), SourceTabPage)
            End If
        Next

        Return Nothing
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

    Public Sub FullUpdate(mTab As SourceTabPage)
        FullUpdate({mTab})
    End Sub

    Public Sub FullUpdate(mTabs As SourceTabPage())
        'Stop all threads
        'TODO: For some reason it locks the *.xshd file, need fix! 
        'FIX: Using 'UpdateSyntaxFile' in the UI thread seems to solve this problem... but why, im using |SyncLock|, |Using| etc.?!
        g_mFormMain.g_ClassSyntaxUpdater.StopThread()

        g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete("")
        g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.m_IntelliSenseFunction = ""
        g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()

        g_mFormMain.g_mUCTextMinimap.UpdateText(False)
        g_mFormMain.g_mUCTextMinimap.UpdatePosition(False, True, True)

        g_mFormMain.g_mUCObjectBrowser.StartUpdate()

        For i = 0 To mTabs.Length - 1
            g_mFormMain.g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, mTabs(i).m_Identifier)
        Next

        g_mFormMain.g_ClassSyntaxUpdater.StartThread()

        g_mFormMain.UpdateFormConfigText()
    End Sub

    Private Sub OnTabSelected(sender As Object, e As EventArgs)
        If (g_bIgnoreOnTabSelected) Then
            Return
        End If

        g_bIgnoreOnTabSelected = True
        SelectTab(m_ActiveTabIndex)
        g_bIgnoreOnTabSelected = False
    End Sub

    Public Class SourceTabPage
        Inherits TabPage

        Private g_mFormMain As FormMain

        Private g_sText As String = "Unnamed"
        Private g_bTextChanged As Boolean = False
        Private g_sIdentifier As String = Guid.NewGuid.ToString

        Private g_sFile As String = ""
        Private g_mAutocompleteItems As New ClassSyncList(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
        Private g_mIncludeFiles As New ClassSyncList(Of DictionaryEntry) '{sTabIdentifier-Ref, IncludeFile}
        Private g_mIncludeFilesFull As New ClassSyncList(Of DictionaryEntry) '{sTabIdentifier-Ref, IncludeFile}
        Private g_mActiveConfig As ClassConfigs.STRUC_CONFIG_ITEM = ClassConfigs.m_DefaultConfig
        Private g_iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN
        Private g_bHasReferenceIncludes As Boolean = False
        Private g_mSourceTextEditor As TextEditorControlEx
        Private g_bHandlersEnabled As Boolean = False
        Private g_mFileCachedWriteDate As Date

        Public Sub New(f As FormMain)
            g_mFormMain = f

            CreateTextEditor()

            ClassControlStyle.UpdateControls(Me)
        End Sub

        Public Sub New(f As FormMain, sText As String)
            g_mFormMain = f
            m_Title = sText

            CreateTextEditor()

            ClassControlStyle.UpdateControls(Me)
        End Sub

        Public Sub New(f As FormMain, sText As String, bChanged As Boolean)
            g_mFormMain = f
            m_Title = sText
            m_Changed = bChanged

            CreateTextEditor()

            ClassControlStyle.UpdateControls(Me)
        End Sub

        Private Sub AddHandlers()
            RemoveHandlers()

            AddHandler g_mSourceTextEditor.ProcessCmdKeyEvent, AddressOf TextEditorControl_Source_ProcessCmdKey
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.KeyPress, AddressOf TextEditorControl_Source_AutoCloseChars

            AddHandler g_mSourceTextEditor.TextChanged, AddressOf TextEditorControl_Source_TextChanged

            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MotherTextAreaControl.VScrollBar.ValueChanged, AddressOf TextEditorControl_Source_Scroll

            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.DragEnter, AddressOf TextEditorControl_Source_DragEnter
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.DragDrop, AddressOf TextEditorControl_Source_DragDrop
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.DragOver, AddressOf TextEditorControl_Source_DragOver

            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord

            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete

            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_Source_SwitchToAutocompleteTab

            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectionChanged, AddressOf TextEditorControl_Source_UpdateInfo
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged, AddressOf TextEditorControl_Source_UpdateInfo

            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Document.LineLengthChanged, AddressOf TextEditorControl_DetectLineLengthChange
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Document.LineCountChanged, AddressOf TextEditorControl_DetectLineCountChange

            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Document.FoldingManager.FoldingsChanged, AddressOf TextEditorControl_FoldingsChanged
        End Sub

        Private Sub RemoveHandlers()
            RemoveHandler g_mSourceTextEditor.ProcessCmdKeyEvent, AddressOf TextEditorControl_Source_ProcessCmdKey
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.KeyPress, AddressOf TextEditorControl_Source_AutoCloseChars

            RemoveHandler g_mSourceTextEditor.TextChanged, AddressOf TextEditorControl_Source_TextChanged

            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MotherTextAreaControl.VScrollBar.ValueChanged, AddressOf TextEditorControl_Source_Scroll

            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.DragEnter, AddressOf TextEditorControl_Source_DragEnter
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.DragDrop, AddressOf TextEditorControl_Source_DragDrop
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.DragOver, AddressOf TextEditorControl_Source_DragOver

            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord

            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete

            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_Source_SwitchToAutocompleteTab

            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectionChanged, AddressOf TextEditorControl_Source_UpdateInfo
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged, AddressOf TextEditorControl_Source_UpdateInfo

            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Document.LineLengthChanged, AddressOf TextEditorControl_DetectLineLengthChange
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Document.LineCountChanged, AddressOf TextEditorControl_DetectLineCountChange

            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Document.FoldingManager.FoldingsChanged, AddressOf TextEditorControl_FoldingsChanged
        End Sub

        Private Sub CreateTextEditor()
            g_mSourceTextEditor = New TextEditorControlEx
            g_mSourceTextEditor.SuspendLayout()

            g_mSourceTextEditor.ContextMenuStrip = g_mFormMain.ContextMenuStrip_RightClick
            g_mSourceTextEditor.IsIconBarVisible = True
            g_mSourceTextEditor.ShowTabs = True
            g_mSourceTextEditor.ShowVRuler = False
            g_mSourceTextEditor.HideMouseCursor = True
            g_mSourceTextEditor.Margin = New Padding(0)
            g_mSourceTextEditor.Padding = New Padding(0)

            g_mSourceTextEditor.Document.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
            g_mSourceTextEditor.Document.TextEditorProperties.IndentationSize = If(ClassSettings.g_iSettingsTabsToSpaces > 0, ClassSettings.g_iSettingsTabsToSpaces, 4)
            g_mSourceTextEditor.Document.TextEditorProperties.ConvertTabsToSpaces = (ClassSettings.g_iSettingsTabsToSpaces > 0)

            g_mSourceTextEditor.Parent = Me
            g_mSourceTextEditor.Dock = DockStyle.Fill

            g_mSourceTextEditor.Document.FoldingManager.FoldingStrategy = New VariXFolding()
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

        Public Sub SelectTab()
            g_mFormMain.g_ClassTabControl.SelectTab(m_Index)
        End Sub

        Public Sub SelectTab(iDelay As Integer)
            g_mFormMain.g_ClassTabControl.SelectTab(m_Identifier, iDelay)
        End Sub

        Public Function RemoveTab(bPrompSave As Boolean, Optional iSelectTabIndex As Integer = -1) As Boolean
            Return g_mFormMain.g_ClassTabControl.RemoveTab(m_Index, bPrompSave, iSelectTabIndex)
        End Function

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

        Public ReadOnly Property m_AutocompleteItems As ClassSyncList(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Get
                Return g_mAutocompleteItems
            End Get
        End Property

        Public ReadOnly Property m_IncludeFiles As ClassSyncList(Of DictionaryEntry)
            Get
                Return g_mIncludeFiles
            End Get
        End Property

        Public ReadOnly Property m_IncludeFilesFull As ClassSyncList(Of DictionaryEntry)
            Get
                Return g_mIncludeFilesFull
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
                    Text = g_sText

                    UpdateToolTip()
                End If
            End Set
        End Property

        Public ReadOnly Property m_ClassLineState As ClassTextEditorTools.ClassLineState
            Get
                Return g_mFormMain.g_ClassLineState
            End Get
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
                    Text = g_sText

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
                    Text = value
                End If
            End Set
        End Property

        Public Sub UpdateToolTip()
            Dim lInfo As New List(Of String)
            If (g_bTextChanged) Then
                lInfo.Add("Unsaved")
            End If
            If (g_bHasReferenceIncludes) Then
                lInfo.Add("Referenced in other tabs")
            End If

            Me.ToolTipText = g_sFile & If(lInfo.Count < 1, "", Environment.NewLine & String.Format("({0})", String.Join(", ", lInfo.ToArray)))
        End Sub


        Public Overrides Property Text As String
            Get
                Return If(g_bHasReferenceIncludes, "#", "") & g_sText & If(g_bTextChanged, "*"c, "")
            End Get
            Set(value As String)
                MyBase.Text = value
            End Set
        End Property

        Private Sub TextEditorControl_Source_Scroll(sender As Object, e As EventArgs)
            g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTipFormLocation()
            g_mFormMain.g_mUCTextMinimap.UpdatePosition(False, True, True)
        End Sub

#Region "Drag & Drop"
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
                    g_mFormMain.g_ClassTabControl.BeginUpdate()

                    For Each sFile As String In sFiles
                        If (Not IO.File.Exists(sFile)) Then
                            Continue For
                        End If

                        Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(sFile)
                        mTab.SelectTab(500)
                    Next
                Finally
                    g_mFormMain.g_ClassTabControl.EndUpdate()
                End Try
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub
#End Region

#Region "TextEditor Controls"
        Private Sub TextEditorControl_Source_ProcessCmdKey(ByRef bBlock As Boolean, ByRef iMsg As Message, iKeys As Keys)
            Try
                Select Case (iKeys)

                    'Duplicate Line/Word
                    Case Keys.Control Or Keys.D
                        bBlock = True

                        If (g_mSourceTextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                            Dim sText As String = g_mSourceTextEditor.ActiveTextAreaControl.SelectionManager.SelectedText
                            Dim iCaretOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset

                            g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iCaretOffset, sText)
                        Else
                            Dim iCaretOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset
                            Dim iLineOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iCaretOffset).Offset
                            Dim iLineLen As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iCaretOffset).Length

                            g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iLineOffset, g_mSourceTextEditor.ActiveTextAreaControl.Document.GetText(iLineOffset, iLineLen) & Environment.NewLine)
                        End If

                        g_mSourceTextEditor.Refresh()

                    'Paste Autocomplete
                    Case Keys.Control Or Keys.Enter
                        bBlock = True

                        g_mSourceTextEditor.Document.UndoStack.StartUndoGroup()

                        Dim iOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset
                        Dim iPosition As Integer = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                        Dim iLineOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iOffset).Offset
                        Dim iLineLen As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iOffset).Length
                        Dim iLineNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iOffset).LineNumber

                        Dim sCaretFunctionName As String = Regex.Match(g_mSourceTextEditor.ActiveTextAreaControl.Document.GetText(iLineOffset, iPosition), "(\b[a-zA-Z0-9_]+\b(\.|\:){0,1}(\b[a-zA-Z0-9_]+\b){0,1})$").Value

                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition - sCaretFunctionName.Length
                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Remove(iOffset - sCaretFunctionName.Length, sCaretFunctionName.Length)

                        iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column

                        Dim bIsLineEmpty = Regex.IsMatch(g_mSourceTextEditor.ActiveTextAreaControl.Document.GetText(iLineOffset, iLineLen - sCaretFunctionName.Length), "^\s*$")

                        Dim mAutocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE = g_mFormMain.g_mUCAutocomplete.GetSelectedItem()

                        If (mAutocomplete IsNot Nothing) Then
                            Select Case (True)
                                Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD
                                    If (bIsLineEmpty) Then
                                        Dim iLineOffsetNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Offset
                                        Dim iLineLenNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Length
                                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Remove(iLineOffsetNum, iLineLenNum)

                                        Dim sIndentation As String = ClassSettings.BuildIndentation(1, ClassSettings.ENUM_INDENTATION_TYPES.USE_SETTINGS)

                                        Dim sNewInput As String
                                        With New Text.StringBuilder
                                            .AppendLine("public " & mAutocomplete.m_FullFunctionString.Remove(0, "forward".Length).Trim)
                                            .AppendLine("{")
                                            .AppendLine(sIndentation)
                                            .AppendLine("}")
                                            sNewInput = .ToString
                                        End With

                                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iLineOffsetNum, sNewInput)

                                        iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Line = iLineNum + 2
                                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = sIndentation.Length
                                    Else
                                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sCaretFunctionName.Length, mAutocomplete.m_FunctionName)

                                        iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + mAutocomplete.m_FunctionName.Length
                                    End If

                                Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM,
                                         (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG,
                                         (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET,
                                         (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF
                                    If (bIsLineEmpty) Then
                                        Dim iLineOffsetNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Offset
                                        Dim iLineLenNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Length
                                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Remove(iLineOffsetNum, iLineLenNum)

                                        Dim sIndentation As String = ClassSettings.BuildIndentation(1, ClassSettings.ENUM_INDENTATION_TYPES.USE_SETTINGS)

                                        Dim sNewInput As String
                                        With New Text.StringBuilder
                                            .AppendLine(mAutocomplete.m_FunctionString.Trim)
                                            .AppendLine("{")
                                            .AppendLine(sIndentation)
                                            .AppendLine("}")
                                            sNewInput = .ToString
                                        End With

                                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iLineOffsetNum, sNewInput)

                                        iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Line = iLineNum + 2
                                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = sIndentation.Length
                                    Else
                                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sCaretFunctionName.Length, mAutocomplete.m_FunctionName)

                                        iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + mAutocomplete.m_FunctionName.Length
                                    End If


                                Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE,
                                            (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sCaretFunctionName.Length, mAutocomplete.m_FunctionString)

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + mAutocomplete.m_FunctionString.Length

                                Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM,
                                            (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STRUCT) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STRUCT
                                    If (ClassSettings.g_iSettingsFullEnumAutocomplete OrElse mAutocomplete.m_FunctionString.IndexOf("."c) < 0) Then
                                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sCaretFunctionName.Length, mAutocomplete.m_FunctionString.Replace("."c, ":"c))

                                        iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + mAutocomplete.m_FunctionString.Length
                                    Else
                                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sCaretFunctionName.Length, mAutocomplete.m_FunctionString.Remove(0, mAutocomplete.m_FunctionString.IndexOf("."c) + 1))

                                        iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + mAutocomplete.m_FunctionString.Remove(0, mAutocomplete.m_FunctionString.IndexOf("."c) + 1).Length
                                    End If

                                Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP,
                                        (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE,
                                        (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.OPERATOR) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.OPERATOR
                                    If (mAutocomplete.m_FunctionString.IndexOf("."c) > -1 AndAlso sCaretFunctionName.IndexOf("."c) > -1 AndAlso Not sCaretFunctionName.StartsWith(mAutocomplete.m_FunctionString)) Then
                                        Dim sParenthesis As String = ""
                                        If (mAutocomplete.m_FullFunctionString.Contains("("c) AndAlso mAutocomplete.m_FullFunctionString.Contains(")"c)) Then
                                            sParenthesis = "()"
                                        End If

                                        Dim sNewInput As String = String.Format("{0}.{1}{2}",
                                                                                sCaretFunctionName.Remove(sCaretFunctionName.LastIndexOf("."c), sCaretFunctionName.Length - sCaretFunctionName.LastIndexOf("."c)),
                                                                                mAutocomplete.m_FunctionString.Remove(0, mAutocomplete.m_FunctionString.IndexOf("."c) + 1),
                                                                                sParenthesis)
                                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sCaretFunctionName.Length, sNewInput)

                                        iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length + If(sParenthesis.Length > 0, -1, 0)
                                    Else
                                        Dim sParenthesis As String = ""
                                        If (mAutocomplete.m_FullFunctionString.Contains("("c) AndAlso mAutocomplete.m_FullFunctionString.Contains(")"c)) Then
                                            sParenthesis = "()"
                                        End If

                                        Dim sNewInput As String = String.Format("{0}{1}",
                                                                                mAutocomplete.m_FunctionString.Remove(0, mAutocomplete.m_FunctionString.IndexOf("."c) + 1),
                                                                                sParenthesis)

                                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sCaretFunctionName.Length, sNewInput)

                                        iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length + If(sParenthesis.Length > 0, -1, 0)
                                    End If

                                Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PREPROCESSOR) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PREPROCESSOR
                                    Dim iLineOffsetNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Offset
                                    Dim sNewInput As String = String.Format("#{0}", mAutocomplete.m_FunctionString)

                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Remove(iLineOffsetNum, iPosition)
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iLineOffsetNum, sNewInput)

                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = sNewInput.Length

                                Case Else
                                    If (ClassSettings.g_iSettingsFullMethodAutocomplete) Then
                                        Dim sNewInput As String = mAutocomplete.m_FullFunctionString.Remove(0, Regex.Match(mAutocomplete.m_FullFunctionString, "^(?<Useless>.*?)(\b[a-zA-Z0-9_]+\b)\s*(\()").Groups("Useless").Length)
                                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sCaretFunctionName.Length, sNewInput)

                                        iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length
                                    Else
                                        Dim sNewInput As String = mAutocomplete.m_FunctionString.Remove(0, Regex.Match(mAutocomplete.m_FunctionString, "^(?<Useless>.*?)(\b[a-zA-Z0-9_]+\b)\s*(\()").Groups("Useless").Length)
                                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sCaretFunctionName.Length, String.Format("{0}()", sNewInput))

                                        iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                        g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length + 1
                                    End If

                            End Select
                        End If

                        g_mSourceTextEditor.Document.UndoStack.EndUndoGroup()
                        g_mSourceTextEditor.Refresh()

                    'Autocomplete up
                    Case Keys.Control Or Keys.Up
                        If (g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.SelectedItems.Count < 1) Then
                            Return
                        End If

                        Dim iListViewCount As Integer = g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items.Count

                        Dim iNewIndex As Integer = g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.SelectedItems(0).Index - 1

                        If (iNewIndex > -1 AndAlso iNewIndex < iListViewCount) Then
                            g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items(iNewIndex).Selected = True
                            g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items(iNewIndex).EnsureVisible()
                        End If

                        bBlock = True

                    'Autocomplete Down
                    Case Keys.Control Or Keys.Down
                        If (g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.SelectedItems.Count < 1) Then
                            Return
                        End If

                        Dim iListViewCount As Integer = g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items.Count

                        Dim iNewIndex As Integer = g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.SelectedItems(0).Index + 1

                        If (iNewIndex > -1 AndAlso iNewIndex < iListViewCount) Then
                            g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items(iNewIndex).Selected = True
                            g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items(iNewIndex).EnsureVisible()
                        End If

                        bBlock = True
                End Select
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Private Sub TextEditorControl_Source_AutoCloseChars(sender As Object, e As KeyPressEventArgs)
            Static sLastAutoClosedKey As String = ""

            Dim bLastAutoCloseKeySet As Boolean = False

            If (ClassSettings.g_iSettingsAutoCloseStrings) Then
                Select Case True
                    Case (e.KeyChar = """"c)
                        Dim iOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Caret.Offset
                        If (iOffset > 0 AndAlso g_mSourceTextEditor.ActiveTextAreaControl.Document.GetText(iOffset - 1, 1) = ClassSyntaxTools.g_sEscapeCharacters(g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)) Then
                            Exit Select
                        End If

                        Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(g_mSourceTextEditor.ActiveTextAreaControl.Document.TextContent, g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)
                        If (mSourceAnalysis.m_InString(iOffset) OrElse mSourceAnalysis.m_InChar(iOffset)) Then
                            Exit Select
                        End If

                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, """")

                    Case (e.KeyChar = "'"c)
                        Dim iOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Caret.Offset
                        If (iOffset > 0 AndAlso g_mSourceTextEditor.ActiveTextAreaControl.Document.GetText(iOffset - 1, 1) = ClassSyntaxTools.g_sEscapeCharacters(g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)) Then
                            Exit Select
                        End If

                        Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(g_mSourceTextEditor.ActiveTextAreaControl.Document.TextContent, g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)
                        If (mSourceAnalysis.m_InString(iOffset) OrElse mSourceAnalysis.m_InChar(iOffset)) Then
                            Exit Select
                        End If

                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, "'")
                End Select
            End If

            If (ClassSettings.g_iSettingsAutoCloseBrackets) Then
                Select Case True
                    Case (sLastAutoClosedKey = "[" AndAlso e.KeyChar = "]"c)
                        Dim iOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Caret.Offset
                        Dim iLenght As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.TextLength
                        If (iOffset > iLenght - 1) Then
                            Exit Select
                        End If

                        If (g_mSourceTextEditor.ActiveTextAreaControl.Document.GetCharAt(iOffset) <> "]"c) Then
                            Exit Select
                        End If

                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Remove(iOffset, 1)

                    Case (sLastAutoClosedKey = "(" AndAlso e.KeyChar = ")"c)
                        Dim iOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Caret.Offset
                        Dim iLenght As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.TextLength
                        If (iOffset > iLenght - 1) Then
                            Exit Select
                        End If

                        If (g_mSourceTextEditor.ActiveTextAreaControl.Document.GetCharAt(iOffset) <> ")"c) Then
                            Exit Select
                        End If

                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Remove(iOffset, 1)

                    Case (sLastAutoClosedKey = "{" AndAlso e.KeyChar = "}"c)
                        Dim iOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Caret.Offset
                        Dim iLenght As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.TextLength
                        If (iOffset > iLenght - 1) Then
                            Exit Select
                        End If

                        If (g_mSourceTextEditor.ActiveTextAreaControl.Document.GetCharAt(iOffset) <> "}"c) Then
                            Exit Select
                        End If

                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Remove(iOffset, 1)

                    Case (e.KeyChar = "["c)
                        Dim iOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Caret.Offset
                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, "]")

                        sLastAutoClosedKey = "["c
                        bLastAutoCloseKeySet = True

                    Case (e.KeyChar = "("c)
                        Dim iOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Caret.Offset
                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, ")")

                        sLastAutoClosedKey = "("c
                        bLastAutoCloseKeySet = True

                    Case (e.KeyChar = "{"c)
                        Dim iOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Caret.Offset
                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, "}")

                        sLastAutoClosedKey = "{"c
                        bLastAutoCloseKeySet = True
                End Select
            End If

            If (Not bLastAutoCloseKeySet AndAlso sLastAutoClosedKey <> "") Then
                sLastAutoClosedKey = ""
            End If
        End Sub

        Private Sub TextEditorControl_Source_UpdateInfo(sender As Object, e As EventArgs)
            g_mFormMain.ToolStripStatusLabel_EditorLine.Text = "L: " & g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Line + 1
            g_mFormMain.ToolStripStatusLabel_EditorCollum.Text = "C: " & g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Column
            g_mFormMain.ToolStripStatusLabel_EditorSelectedCount.Text = "S: " & g_mSourceTextEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText.Length
        End Sub

        Private Sub TextEditorControl_DetectLineLengthChange(sender As Object, e As LineLengthChangeEventArgs)
            Try
                Dim iTotalLines As Integer = g_mSourceTextEditor.Document.TotalNumberOfLines

                If (e.LineSegment.IsDeleted OrElse e.LineSegment.Length < 0) Then
                    Return
                End If

                If (e.LineSegment.LineNumber > iTotalLines) Then
                    Return
                End If

                m_ClassLineState.m_LineState(g_mSourceTextEditor, e.LineSegment.LineNumber) = ClassTextEditorTools.ClassLineState.LineStateBookmark.ENUM_BOOKMARK_TYPE.CHANGED
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Private Sub TextEditorControl_DetectLineCountChange(sender As Object, e As LineCountChangeEventArgs)
            Try
                Dim iTotalLines As Integer = g_mSourceTextEditor.Document.TotalNumberOfLines

                If (e.LinesMoved > -1) Then
                    For i = 0 To e.LinesMoved
                        If (e.LineStart + i > iTotalLines) Then
                            Return
                        End If

                        m_ClassLineState.m_LineState(g_mSourceTextEditor, e.LineStart + i) = ClassTextEditorTools.ClassLineState.LineStateBookmark.ENUM_BOOKMARK_TYPE.CHANGED
                    Next
                End If
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Private Sub TextEditorControl_Source_UpdateAutocomplete(sender As Object, e As Object)
            Try
                Static iOldCaretPos As Integer = 0

                Dim iOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset

                If (iOldCaretPos = iOffset) Then
                    Return
                End If

                iOldCaretPos = iOffset

                Dim sFunctionName As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True, True, True)

                If (g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete(sFunctionName) < 1) Then
                    sFunctionName = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(False, False, False)

                    g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete(sFunctionName)
                Else
                    g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
                End If
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Private Sub TextEditorControl_Source_SwitchToAutocompleteTab(sender As Object, e As MouseEventArgs)
            Try
                If (Not ClassSettings.g_iSettingsSwitchTabToAutocomplete) Then
                    Return
                End If

                If (e.Button <> MouseButtons.Left) Then
                    Return
                End If

                If (g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items.Count < 1 OrElse
                            g_mFormMain.TabControl_Details.TabPages.IndexOf(g_mFormMain.TabControl_Details.SelectedTab) = 0) Then
                    Return
                End If

                g_mFormMain.TabControl_Details.SuspendLayout()
                g_mFormMain.TabControl_Details.Enabled = False
                g_mFormMain.TabControl_Details.SelectTab(0)
                g_mFormMain.TabControl_Details.Enabled = True
                g_mFormMain.TabControl_Details.ResumeLayout()
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Private Sub TextEditorControl_Source_TextChanged(sender As Object, e As EventArgs)
            Try
                g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Changed = True
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Private Sub TextEditorControl_Source_DoubleClickMarkWord(sender As Object, e As MouseEventArgs)
            Try
                If (Not ClassSettings.g_iSettingsDoubleClickMark) Then
                    Return
                End If

                g_mFormMain.g_ClassTextEditorTools.MarkSelectedWord()
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub
#End Region

#Region "TextEditor Folding Code"

        ''' <summary>
        ''' The class to generate the foldings, it implements ICSharpCode.TextEditor.Document.IFoldingStrategy
        ''' </summary>
        Private Class VariXFolding
            Implements IFoldingStrategy
            ''' <summary>
            ''' Generates the foldings for our document.
            ''' </summary>
            ''' <param name="document">The current document.</param>
            ''' <param name="fileName">The filename of the document.</param>
            ''' <param name="parseInformation">Extra parse information, not used in this sample.</param>
            ''' <returns>A list of FoldMarkers.</returns>
            Public Function GenerateFoldMarkers(document As IDocument, fileName As String, parseInformation As Object) As List(Of FoldMarker) Implements IFoldingStrategy.GenerateFoldMarkers
                Dim mFolds As New List(Of FoldMarker)()

                Dim iMaxLevels As Integer = 0
                Dim i As Integer = 0
                While True
                    i = document.TextContent.IndexOf("{"c, i)
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

                For i = 0 To document.TextContent.Length - 1
                    Select Case (document.TextContent(i))
                        Case ("{"c)
                            iCurrentLevel += 1
                            If ((iCurrentLevel - 1) < 0) Then
                                Continue For
                            End If

                            iLevels(iCurrentLevel - 1) = If(i > 0, i - 1, i)
                        Case ("}"c)
                            iCurrentLevel -= 1
                            If (iCurrentLevel < 0) Then
                                Continue For
                            End If

                            Dim iLineStart = document.GetLineNumberForOffset(iLevels(iCurrentLevel))
                            Dim iColumStart = document.GetLineSegment(iLineStart).Length
                            Dim iLineEnd = document.GetLineNumberForOffset(i)
                            Dim iColumEnd = document.GetLineSegment(iLineEnd).Length

                            If (iLineStart = iLineEnd) Then
                                Continue For
                            End If

                            mFolds.Add(New FoldMarker(document, iLineStart, iColumStart, iLineEnd, iColumEnd))
                    End Select
                Next

                Return mFolds
            End Function
        End Class

        Private Sub TextEditorControl_FoldingsChanged(sender As Object, e As EventArgs)
            g_mFormMain.g_mUCTextMinimap.UpdateText(True)
        End Sub
#End Region

    End Class

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                RemoveHandler g_mFormMain.TabControl_SourceTabs.SelectedIndexChanged, AddressOf OnTabSelected

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