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



Imports ICSharpCode.TextEditor

Public Class FormDebugger
    Public g_mFormMain As FormMain

    Public g_ClassDebuggerParser As ClassDebuggerTools
    Public g_ClassDebuggerEntries As ClassDebuggerTools.ClassDebuggerEntries
    Public g_ClassDebuggerRunnerEngine As ClassDebuggerTools.ClassRunnerEngine
    Public g_ClassDebuggerRunner As ClassDebuggerRunner

    Public g_sLastPreProcessSourceFile As String = ""
    Public g_iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN

    Public g_bTextEditorEnableClickSelect As Boolean = True
    Public g_bListViewEnableClickSelect As Boolean = True

    Private g_bPostLoad As Boolean = False
    Private g_bIgnoreCheckedChangedEvent As Boolean = False

    Public Sub New(f As FormMain, mDebugTab As ClassTabControl.ClassTab)
        Me.New(f, mDebugTab.m_Identifier)
    End Sub

    Public Sub New(f As FormMain, sDebugTabIdentifier As String)
        g_mFormMain = f

        If (g_mFormMain.g_ClassTabControl.GetTabIndexByIdentifier(sDebugTabIdentifier) < 0) Then
            Throw New ArgumentException("Tab does not exist")
        End If

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        ClassControlStyle.SetNameFlag(ToolStripStatusLabel_DebugState, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_KEEP_BACKCOLOR)
        ClassControlStyle.SetNameFlag(ToolStripStatusLabel_NoConnection, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_KEEP_BACKCOLOR)
        ClassControlStyle.SetNameFlag(StatusStrip_BPDebugger, ClassControlStyle.ENUM_STYLE_FLAGS.MENU_SYSTEMRENDER)

        g_ClassDebuggerParser = New ClassDebuggerTools(Me)
        g_ClassDebuggerEntries = New ClassDebuggerTools.ClassDebuggerEntries
        g_ClassDebuggerRunnerEngine = New ClassDebuggerTools.ClassRunnerEngine
        g_ClassDebuggerRunner = New ClassDebuggerRunner(Me, sDebugTabIdentifier)

        TextEditorControlEx_DebuggerSource.IsReadOnly = True
        RichTextBox_DisasmSource.ReadOnly = True

        RichTextBox_DisasmSource.Font = TextEditorControlEx_DebuggerSource.Font

        AddHandler TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.PositionChanged, AddressOf TextEditorControlEx_DebuggerSource_CaretPositionChanged

        AddHandler TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.TextArea.SelectionManager.SelectionChanged, AddressOf TextEditorControlEx_DebuggerSource_CaretPositionChanged_DisplayInfos
        AddHandler TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.TextArea.Caret.PositionChanged, AddressOf TextEditorControlEx_DebuggerSource_CaretPositionChanged_DisplayInfos
    End Sub

    Private Sub FormDebugger_Load(sender As Object, e As EventArgs) Handles Me.Load
        PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Starting debugger...", False, True)

        If (Not RefreshSource()) Then
            MessageBox.Show("Could not open debugger. See information tab for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Dispose()
            Return
        End If

        g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnDebuggerStart(Me))

        'Load window info
        ClassSettings.LoadWindowInfo(Me)
        LoadViews()

        g_bPostLoad = True
    End Sub

    Public Sub PrintInformation(iIcon As ClassInformationListBox.ENUM_ICONS, sText As String, Optional bClear As Boolean = False, Optional bEnsureVisible As Boolean = False)
        ClassThread.ExecAsync(Me, Sub()
                                      If (bClear) Then
                                          ListBox_Information.Items.Clear()
                                      End If

                                      'Tabs are not displayed correctly with TextRenderer. Replace them with spaces
                                      sText = sText.Replace(vbTab, New String(" "c, 4))

                                      Dim mItem As New ClassInformationListBox.ClassInformationItem(iIcon, Now, sText)

                                      Dim iIndex = ListBox_Information.Items.Add(mItem)

                                      If (bEnsureVisible) Then
                                          'Scroll to item
                                          ListBox_Information.TopIndex = iIndex
                                      End If
                                  End Sub)
    End Sub

    Private Function RefreshSource() As Boolean
        Try
            PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Refreshing source...", False, True)

            Using mFormProgress As New FormProgress
                mFormProgress.Text = "BasicPawn Debugger - Generating source..."
                If (g_bPostLoad) Then
                    mFormProgress.Show(Me)
                Else
                    mFormProgress.Show(g_mFormMain)
                End If
                mFormProgress.m_Progress = 0

                g_ClassDebuggerRunner.UpdateSourceFromTab()

                Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByIdentifier(g_ClassDebuggerRunner.m_DebugTabIdentifier)
                If (mTab Is Nothing) Then
                    Throw New ArgumentException("Tab does not exist")
                End If

                Me.Text = String.Format("BasicPawn Debugger ({0})", IO.Path.GetFileName(g_ClassDebuggerRunner.m_CurrentSourceFile))


                mFormProgress.m_Progress = 10


                Dim iCompilerType As ClassTextEditorTools.ENUM_COMPILER_TYPE = ClassTextEditorTools.ENUM_COMPILER_TYPE.UNKNOWN
                Dim sCompilerOutput As String = ""

                'Create Pre-Process source
                Dim sPreSource As String = g_mFormMain.g_ClassTextEditorTools.GetCompilerPreProcessCode(mTab,
                                                                                                        g_ClassDebuggerRunner.m_CurrentSourceFile,
                                                                                                        g_ClassDebuggerRunner.m_CurrentSource,
                                                                                                        True,
                                                                                                        True,
                                                                                                        g_sLastPreProcessSourceFile,
                                                                                                        g_ClassDebuggerRunner.m_CurrentConfig,
                                                                                                        Nothing,
                                                                                                        Nothing,
                                                                                                        Nothing,
                                                                                                        Nothing,
                                                                                                        Nothing,
                                                                                                        g_ClassDebuggerRunner.m_CurrentSourceFile,
                                                                                                        True,
                                                                                                        sCompilerOutput,
                                                                                                        iCompilerType)

                If (String.IsNullOrEmpty(sPreSource)) Then
                    Throw New ArgumentException("Compiler failure")
                End If

                PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Pre-Processing source output:", False, False)
                For Each sLine In sCompilerOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
                    PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & sLine, False, False)
                Next
                PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, New String("~"c, 50), False, True)


                mFormProgress.m_Progress = 20


                Select Case (iCompilerType)
                    Case ClassTextEditorTools.ENUM_COMPILER_TYPE.SOURCEPAWN
                        g_iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN

                    Case ClassTextEditorTools.ENUM_COMPILER_TYPE.AMXX
                        g_iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.AMXMODX

                    Case ClassTextEditorTools.ENUM_COMPILER_TYPE.AMX
                        g_iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.PAWN

                    Case ClassTextEditorTools.ENUM_COMPILER_TYPE.UNKNOWN
                        Throw New ArgumentException("Unsupported compiler")
                End Select

                'TODO: Add AMX Mod X support
                If (iCompilerType <> ClassTextEditorTools.ENUM_COMPILER_TYPE.SOURCEPAWN) Then
                    Throw New ArgumentException("Unsupported compiler. SourceMod required.")
                End If

                If (String.IsNullOrEmpty(g_sLastPreProcessSourceFile)) Then
                    Throw New ArgumentException("Last Pre-Process source invalid")
                End If


                g_ClassDebuggerRunner.g_ClassPreProcess.FixPreProcessFiles(sPreSource)
                g_ClassDebuggerRunner.g_ClassPreProcess.FixPreProcessFilePaths(sPreSource, g_sLastPreProcessSourceFile, g_ClassDebuggerRunner.m_CurrentSourceFile)
                g_ClassDebuggerRunner.g_ClassPreProcess.AnalysisSourceLines(sPreSource)
                g_ClassDebuggerEntries.UpdateBreakpoints(sPreSource, False, g_iLanguage)
                g_ClassDebuggerEntries.UpdateWatchers(sPreSource, False, g_iLanguage)
                g_ClassDebuggerEntries.UpdateAsserts(sPreSource, False, g_iLanguage)


                mFormProgress.m_Progress = 40


                'Create DIASM code
                Dim sAsmLstSource As String = sPreSource

                ClassDebuggerTools.ClassDebuggerHelpers.CleanupDebugPlaceholder(sAsmLstSource, g_iLanguage)

                iCompilerType = ClassTextEditorTools.ENUM_COMPILER_TYPE.UNKNOWN
                sCompilerOutput = ""


                Dim sAsmSource As String = g_mFormMain.g_ClassTextEditorTools.GetCompilerAssemblyCode(mTab,
                                                                                                      g_sLastPreProcessSourceFile,
                                                                                                      sAsmLstSource,
                                                                                                      True,
                                                                                                      Nothing,
                                                                                                      g_ClassDebuggerRunner.m_CurrentConfig,
                                                                                                      IO.Path.GetDirectoryName(g_sLastPreProcessSourceFile),
                                                                                                      Nothing,
                                                                                                      IO.Path.GetDirectoryName(g_ClassDebuggerRunner.m_CurrentSourceFile),
                                                                                                      Nothing,
                                                                                                      IO.Path.GetDirectoryName(g_ClassDebuggerRunner.m_CurrentSourceFile),
                                                                                                      Nothing,
                                                                                                      True,
                                                                                                      sCompilerOutput,
                                                                                                      iCompilerType)

                If (String.IsNullOrEmpty(sAsmSource)) Then
                    Throw New ArgumentException("Compiler failure")
                End If

                If (iCompilerType <> ClassTextEditorTools.ENUM_COMPILER_TYPE.SOURCEPAWN) Then
                    Throw New ArgumentException("Unsupported compiler")
                End If

                PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "DIASM source output:", False, False)
                For Each sLine In sCompilerOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
                    PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & sLine, False, False)
                Next
                PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, New String("~"c, 50), False, True)


                mFormProgress.m_Progress = 60

                If (True) Then
                    Dim iCaretLine = TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.Line
                    Dim iCaretColumn = TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.Column

                    TextEditorControlEx_DebuggerSource.Document.TextContent = sPreSource
                    RichTextBox_DisasmSource.Text = sAsmSource

                    'Restore old care position
                    Dim mCaretPos = TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.ValidatePosition(New TextLocation(iCaretColumn, iCaretLine))
                    TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.Position = mCaretPos
                    TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.UpdateCaretPosition()

                    TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.CenterViewOn(mCaretPos.Line, 10)
                End If

                TextEditorControlEx_DebuggerSource.InvalidateTextArea()
                RichTextBox_DisasmSource.Invalidate()

                g_mFormMain.g_ClassSyntaxTools.g_ClassSyntaxHighlighting.UpdateTextEditorSyntax()

                TextEditorControlEx_DebuggerSource.Document.MarkerStrategy.RemoveAll(Function() True)


                mFormProgress.m_Progress = 80


                'Add breakpoints 
                Try
                    ListView_Breakpoints.BeginUpdate()
                    ListView_Breakpoints.Items.Clear()

                    For Each mBreakpointItem In g_ClassDebuggerEntries.g_lBreakpointList
                        Dim mListViewItemData As New ClassListViewItemData(New String() {mBreakpointItem.iLine.ToString, mBreakpointItem.sArguments, ""})
                        mListViewItemData.g_mData("GUID") = mBreakpointItem.sGUID

                        With ListView_Breakpoints.Items.Add(mListViewItemData)
                            .Checked = True
                        End With

                        Dim marker As New ICSharpCode.TextEditor.Document.TextMarker(mBreakpointItem.iOffset, mBreakpointItem.iTotalLength, ICSharpCode.TextEditor.Document.TextMarkerType.Underlined, Color.DarkOrange)
                        TextEditorControlEx_DebuggerSource.Document.MarkerStrategy.AddMarker(marker)
                    Next
                Finally
                    ListView_Breakpoints.EndUpdate()
                End Try

                'Add watchers 
                Try
                    ListView_Watchers.BeginUpdate()
                    ListView_Watchers.Items.Clear()

                    For Each mWatcherItem In g_ClassDebuggerEntries.g_lWatcherList
                        Dim mListViewItemData As New ClassListViewItemData(New String() {mWatcherItem.iLine.ToString, mWatcherItem.sArguments, "", "0"})
                        mListViewItemData.g_mData("GUID") = mWatcherItem.sGUID

                        ListView_Watchers.Items.Add(mListViewItemData)

                        Dim marker As New ICSharpCode.TextEditor.Document.TextMarker(mWatcherItem.iOffset, mWatcherItem.iTotalLength, ICSharpCode.TextEditor.Document.TextMarkerType.Underlined, Color.DarkOrange)
                        TextEditorControlEx_DebuggerSource.Document.MarkerStrategy.AddMarker(marker)
                    Next
                Finally
                    ListView_Watchers.EndUpdate()
                End Try

                'Add asserts 
                Try
                    ListView_Asserts.BeginUpdate()
                    ListView_Asserts.Items.Clear()

                    For Each mAssertItem In g_ClassDebuggerEntries.g_lAssertList
                        Dim mListViewItemData As New ClassListViewItemData(New String() {mAssertItem.iLine.ToString, mAssertItem.sArguments, "", ""})
                        mListViewItemData.g_mData("GUID") = mAssertItem.sGUID

                        ListView_Asserts.Items.Add(mListViewItemData)

                        Dim marker As New ICSharpCode.TextEditor.Document.TextMarker(mAssertItem.iOffset, mAssertItem.iTotalLength, ICSharpCode.TextEditor.Document.TextMarkerType.Underlined, Color.DarkOrange)
                        TextEditorControlEx_DebuggerSource.Document.MarkerStrategy.AddMarker(marker)
                    Next
                Finally
                    ListView_Asserts.EndUpdate()
                End Try

                'Add entities
                ListView_Entities.BeginUpdate()
                ListView_Entities.Items.Clear()
                For i = 0 To 2048 - 1
                    Dim mListViewItemData As New ClassListViewItemData(New String() {i.ToString, "", ""})
                    mListViewItemData.g_mData("Index") = i
                    mListViewItemData.g_mData("EntityRef") = 0
                    mListViewItemData.g_mData("Classname") = ""
                    mListViewItemData.g_mData("Ticks") = Nothing

                    ListView_Entities.Items.Add(mListViewItemData)
                Next
                ListView_Entities.EndUpdate()


                mFormProgress.m_Progress = 100


                ClassControlStyle.UpdateControls(Me)

                g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnDebuggerRefresh(Me))
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
            Return False
        End Try

        Return True
    End Function

    Private Sub ToolStripMenuItemFile_Exit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemFile_Exit.Click
        Me.Close()
    End Sub

    Private Sub ToolStripMenuItem_DebugStart_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebugStart.Click
        Select Case (g_ClassDebuggerRunner.m_DebuggingState)
            Case ClassDebuggerRunner.ENUM_DEBUGGING_STATE.PAUSED
                g_ClassDebuggerRunner.ContinueDebugging()
            Case Else
                g_ClassDebuggerRunner.StartDebugging()
        End Select
    End Sub

    Private Sub ToolStripMenuItem_DebugPause_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebugPause.Click
        g_ClassDebuggerRunner.PauseDebugging()
    End Sub

    Private Sub ToolStripMenuItem_DebugStop_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebugStop.Click
        g_ClassDebuggerRunner.StopDebugging()
    End Sub

    Private Sub ToolStripMenuItem_DebugRefresh_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebugRefresh.Click
        Dim bWasRunning As Boolean = (g_ClassDebuggerRunner.m_DebuggingState <> ClassDebuggerRunner.ENUM_DEBUGGING_STATE.STOPPED)

        If (bWasRunning) Then
            If (Not g_ClassDebuggerRunner.StopDebugging) Then
                Return
            End If

            g_ClassDebuggerRunner.m_SuspendGame = True
        End If

        If (Not RefreshSource()) Then
            MessageBox.Show("Could not refresh source. See information tab for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        If (bWasRunning) Then
            If (Not g_ClassDebuggerRunner.StartDebugging()) Then
                g_ClassDebuggerRunner.m_SuspendGame = False
            End If
        End If
    End Sub

    Private Sub ToolStripMenuItem_ToolsCleanupTemp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsCleanupTemp.Click
        Try
            If (g_ClassDebuggerRunner.m_DebuggingState <> ClassDebuggerRunner.ENUM_DEBUGGING_STATE.STOPPED) Then
                MessageBox.Show("You can not clean temporary debugger files while the debugger or multiple debuggers are running!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            CleanupDebuggerTempFiles(True, False)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

#Region "Highlight ListView Pleaceholders"
    Private Sub ListView_Breakpoints_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_Breakpoints.SelectedIndexChanged
        MarkSelectedBreakpoint()
    End Sub

    Private Sub ListView_Breakpoints_MouseClick(sender As Object, e As MouseEventArgs) Handles ListView_Breakpoints.MouseClick
        MarkSelectedBreakpoint()
    End Sub

    Private Sub ListView_Watchers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_Watchers.SelectedIndexChanged
        MarkSelectedWatcher()
    End Sub

    Private Sub ListView_Watchers_MouseClick(sender As Object, e As MouseEventArgs) Handles ListView_Watchers.MouseClick
        MarkSelectedWatcher()
    End Sub

    Private Sub ListView_Asserts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_Asserts.SelectedIndexChanged
        MarkSelectedAssert()
    End Sub

    Private Sub ListView_Asserts_MouseClick(sender As Object, e As MouseEventArgs) Handles ListView_Asserts.MouseClick
        MarkSelectedAssert()
    End Sub
#End Region

    ''' <summary>
    ''' Mark selected debugger breakpoints using the ListView
    ''' </summary>
    Public Sub MarkSelectedBreakpoint()
        Try
            If (Not g_bListViewEnableClickSelect) Then
                Return
            End If

            If (ListView_Breakpoints.SelectedItems.Count < 1) Then
                Return
            End If

            Dim mListViewItemData = TryCast(ListView_Breakpoints.SelectedItems(0), ClassListViewItemData)
            If (mListViewItemData Is Nothing) Then
                Return
            End If

            Dim sGUID As String = CStr(mListViewItemData.g_mData("GUID"))

            For Each mItem In g_ClassDebuggerEntries.g_lBreakpointList
                If (mItem.sGUID <> sGUID) Then
                    Continue For
                End If

                Dim iLine As Integer = mItem.iLine - 1

                Dim mStartLoc As New ICSharpCode.TextEditor.TextLocation(mItem.iIndex, iLine)
                Dim mEndLoc As New ICSharpCode.TextEditor.TextLocation(mItem.iIndex + mItem.iTotalLength, iLine)

                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.Position = mStartLoc
                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.SelectionManager.ClearSelection()
                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.SelectionManager.SetSelection(mStartLoc, mEndLoc)
                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.CenterViewOn(iLine, 10)

                Exit For
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Mark selected debugger watchers using the ListView
    ''' </summary>
    Public Sub MarkSelectedWatcher()
        Try
            If (Not g_bListViewEnableClickSelect) Then
                Return
            End If

            If (ListView_Watchers.SelectedItems.Count < 1) Then
                Return
            End If

            Dim mListViewItemData = TryCast(ListView_Watchers.SelectedItems(0), ClassListViewItemData)
            If (mListViewItemData Is Nothing) Then
                Return
            End If

            Dim sGUID As String = CStr(mListViewItemData.g_mData("GUID"))

            For Each item In g_ClassDebuggerEntries.g_lWatcherList
                If (item.sGUID <> sGUID) Then
                    Continue For
                End If

                Dim iLine As Integer = item.iLine - 1

                Dim mStartLoc As New ICSharpCode.TextEditor.TextLocation(item.iIndex, iLine)
                Dim mEndLoc As New ICSharpCode.TextEditor.TextLocation(item.iIndex + item.iTotalLength, iLine)

                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.Position = mStartLoc
                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.SelectionManager.ClearSelection()
                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.SelectionManager.SetSelection(mStartLoc, mEndLoc)
                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.CenterViewOn(iLine, 10)

                Exit For
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Mark selected debugger watchers using the ListView
    ''' </summary>
    Public Sub MarkSelectedAssert()
        Try
            If (Not g_bListViewEnableClickSelect) Then
                Return
            End If

            If (ListView_Asserts.SelectedItems.Count < 1) Then
                Return
            End If

            Dim mListViewItemData = TryCast(ListView_Asserts.SelectedItems(0), ClassListViewItemData)
            If (mListViewItemData Is Nothing) Then
                Return
            End If

            Dim sGUID As String = CStr(mListViewItemData.g_mData("GUID"))

            For Each item In g_ClassDebuggerEntries.g_lAssertList
                If (item.sGUID <> sGUID) Then
                    Continue For
                End If

                Dim iLine As Integer = item.iLine - 1

                Dim mStartLoc As New ICSharpCode.TextEditor.TextLocation(item.iIndex, iLine)
                Dim mEndLoc As New ICSharpCode.TextEditor.TextLocation(item.iIndex + item.iTotalLength, iLine)

                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.Position = mStartLoc
                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.SelectionManager.ClearSelection()
                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.SelectionManager.SetSelection(mStartLoc, mEndLoc)
                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.CenterViewOn(iLine, 10)

                Exit For
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub FormDebugger_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'Save window info
        ClassSettings.SaveWindowInfo(Me)
        SaveViews()

        g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM)
                                                               If (Not j.mPluginInterface.OnDebuggerEnd(Me)) Then
                                                                   e.Cancel = True
                                                               End If
                                                           End Sub)

        If (Not g_ClassDebuggerRunner.StopDebugging()) Then
            e.Cancel = True
            Return
        End If

        'CleanupDebuggerTempFiles(False, True)
    End Sub

    ''' <summary>
    ''' Clean all *.bpdebug files in the game diretory
    ''' </summary>
    ''' <param name="bNoPrompt">If true, all files will be cleaned without any question asked.</param>
    ''' <param name="bNoErrorPrompt">If true, no error messagebox will be shown on error.</param>
    Public Sub CleanupDebuggerTempFiles(bNoPrompt As Boolean, bNoErrorPrompt As Boolean)
        'Make sure there are debugger files, otherwise dont cleanup
        If (Not String.IsNullOrEmpty(g_ClassDebuggerRunner.m_ServerFolder) AndAlso IO.Directory.Exists(g_ClassDebuggerRunner.m_ServerFolder)) Then
            While True
                For Each sFile As String In IO.Directory.GetFiles(g_ClassDebuggerRunner.m_ServerFolder)
                    If (Not IO.File.Exists(sFile)) Then
                        Continue For
                    End If

                    If (IO.Path.GetExtension(sFile).ToLower <> ClassDebuggerTools.g_sDebuggerFilesExt.ToLower) Then
                        Continue For
                    End If

                    Exit While
                Next

                Return
            End While
        ElseIf (Not bNoErrorPrompt) Then
            MessageBox.Show("Invalid server directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Select Case (If(bNoPrompt, DialogResult.Yes, MessageBox.Show("Cleanup BasicPawn Debugger temporary files?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question)))
            Case DialogResult.Yes
                If (Not String.IsNullOrEmpty(g_ClassDebuggerRunner.m_ServerFolder) AndAlso IO.Directory.Exists(g_ClassDebuggerRunner.m_ServerFolder)) Then
                    For Each sFile As String In IO.Directory.GetFiles(g_ClassDebuggerRunner.m_ServerFolder)
                        If (Not IO.File.Exists(sFile)) Then
                            Continue For
                        End If

                        If (IO.Path.GetExtension(sFile).ToLower <> ClassDebuggerTools.g_sDebuggerFilesExt.ToLower) Then
                            Continue For
                        End If

                        Try
                            IO.File.Delete(sFile)
                        Catch ex As Exception
                            ClassExceptionLog.WriteToLogMessageBox(ex)
                        End Try
                    Next
                ElseIf (Not bNoErrorPrompt) Then
                    MessageBox.Show("Invalid server directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
        End Select
    End Sub

#Region "Enable/Disable Breakpoints"
    Private Sub ListView_Breakpoints_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListView_Breakpoints.ItemChecked
        Dim mListViewItemData = TryCast(e.Item, ClassListViewItemData)
        If (mListViewItemData Is Nothing) Then
            Return
        End If

        Dim sGUID As String = CStr(mListViewItemData.g_mData("GUID"))

        g_ClassDebuggerRunner.m_IgnoreBreakpointGUID(sGUID) = (Not e.Item.Checked)
    End Sub

    Private Sub ToolStripMenuItem_BreakpointsEnableAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_BreakpointsEnableAll.Click
        For i = 0 To ListView_Breakpoints.Items.Count - 1
            ListView_Breakpoints.Items(i).Checked = True
        Next
    End Sub

    Private Sub ToolStripMenuItem_BreakpointsDisableAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_BreakpointsDisableAll.Click
        For i = 0 To ListView_Breakpoints.Items.Count - 1
            ListView_Breakpoints.Items(i).Checked = False
        Next
    End Sub
#End Region

    Private Sub ToolStripMenuItem_BreakpointsSetValues_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_BreakpointsSetValues.Click
        Try
            If (String.IsNullOrEmpty(g_ClassDebuggerRunner.g_mActiveBreakpointInfo.sGUID)) Then
                MessageBox.Show("There is currently no active breakpoint!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Using i As New FormDebuggerBreakpointSetValue
                If (g_ClassDebuggerRunner.g_mActiveBreakpointInfo.bReturnCustomValue) Then
                    Select Case (g_ClassDebuggerRunner.g_mActiveBreakpointInfo.mValueType)
                        Case ClassDebuggerRunner.ENUM_BREAKPOINT_VALUE_TYPE.INTEGER
                            i.RadioButton_TypeInteger.Checked = True

                            Dim iInt As Decimal
                            If (Not Decimal.TryParse(g_ClassDebuggerRunner.g_mActiveBreakpointInfo.sIntegerValue, iInt)) Then
                                iInt = 0
                            End If

                            i.NumericUpDown_BreakpointValue.Value = iInt
                        Case Else
                            i.RadioButton_TypeFloatingPoint.Checked = True

                            Dim iFloat As Decimal
                            If (Not Decimal.TryParse(g_ClassDebuggerRunner.g_mActiveBreakpointInfo.sFloatValue.Replace(".", ","), iFloat)) Then
                                iFloat = 0
                            End If

                            i.NumericUpDown_BreakpointValue.Value = iFloat
                    End Select
                Else
                    Dim iOrgInt As Decimal
                    Dim iOrgFloat As Decimal
                    If (Not Decimal.TryParse(g_ClassDebuggerRunner.g_mActiveBreakpointInfo.sOrginalIntegerValue, iOrgInt)) Then
                        iOrgInt = 0
                    End If
                    If (Not Decimal.TryParse(g_ClassDebuggerRunner.g_mActiveBreakpointInfo.sOrginalFloatValue.Replace(".", ","), iOrgFloat)) Then
                        iOrgFloat = 0.0D
                    End If

                    If (iOrgInt <> 0 AndAlso iOrgFloat = 0.0) Then
                        i.RadioButton_TypeInteger.Checked = True
                        i.NumericUpDown_BreakpointValue.Value = CDec(g_ClassDebuggerRunner.g_mActiveBreakpointInfo.sOrginalIntegerValue)
                    ElseIf (iOrgInt = 0 AndAlso iOrgFloat = 0.0) Then
                        i.RadioButton_TypeInteger.Checked = True
                        i.NumericUpDown_BreakpointValue.Value = CDec(g_ClassDebuggerRunner.g_mActiveBreakpointInfo.sOrginalIntegerValue)
                    Else
                        i.RadioButton_TypeFloatingPoint.Checked = True
                        i.NumericUpDown_BreakpointValue.Value = CDec(g_ClassDebuggerRunner.g_mActiveBreakpointInfo.sOrginalFloatValue.Replace(".", ","))
                    End If
                End If

                Select Case (i.ShowDialog(Me))
                    Case DialogResult.OK
                        g_ClassDebuggerRunner.g_mActiveBreakpointInfo.bReturnCustomValue = True

                        Select Case (True)
                            Case i.RadioButton_TypeInteger.Checked
                                g_ClassDebuggerRunner.g_mActiveBreakpointInfo.mValueType = ClassDebuggerRunner.ENUM_BREAKPOINT_VALUE_TYPE.INTEGER
                                g_ClassDebuggerRunner.g_mActiveBreakpointInfo.sIntegerValue = Math.Round(i.NumericUpDown_BreakpointValue.Value).ToString
                                g_ClassDebuggerRunner.g_mActiveBreakpointInfo.sFloatValue = "0.0"
                            Case Else
                                g_ClassDebuggerRunner.g_mActiveBreakpointInfo.mValueType = ClassDebuggerRunner.ENUM_BREAKPOINT_VALUE_TYPE.FLOAT
                                g_ClassDebuggerRunner.g_mActiveBreakpointInfo.sIntegerValue = "0"
                                g_ClassDebuggerRunner.g_mActiveBreakpointInfo.sFloatValue = i.NumericUpDown_BreakpointValue.Value.ToString.Replace(",", ".")
                        End Select

                        'Update values in the ListView
                        g_ClassDebuggerRunner.UpdateListViewInfoItems()

                    Case DialogResult.Abort
                        g_ClassDebuggerRunner.g_mActiveBreakpointInfo.bReturnCustomValue = False

                        'Update values in the ListView
                        g_ClassDebuggerRunner.UpdateListViewInfoItems()
                End Select
            End Using
        Catch ex As Exception
            'TODO: Add better handle read support.
            '      For some reason saving a handle as float, it becomes massive. The int doesnt.
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_AssertAction_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_AssertAction.Click
        Try
            If (String.IsNullOrEmpty(g_ClassDebuggerRunner.g_mActiveAssertInfo.sGUID)) Then
                MessageBox.Show("There is currently no active assert!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Using i As New FormDebuggerAssertSetAction
                Select Case (i.ShowDialog(Me))
                    Case DialogResult.OK
                        Select Case (i.m_Action)
                            Case FormDebuggerAssertSetAction.ENUM_ACTION.FAIL
                                g_ClassDebuggerRunner.g_mActiveAssertInfo.iActionType = ClassDebuggerRunner.ENUM_ASSERT_ACTION_TYPE.FAIL
                            Case FormDebuggerAssertSetAction.ENUM_ACTION.ERROR
                                g_ClassDebuggerRunner.g_mActiveAssertInfo.iActionType = ClassDebuggerRunner.ENUM_ASSERT_ACTION_TYPE.ERROR
                            Case Else
                                g_ClassDebuggerRunner.g_mActiveAssertInfo.iActionType = ClassDebuggerRunner.ENUM_ASSERT_ACTION_TYPE.IGNORE
                        End Select

                        'Update values in the ListView
                        g_ClassDebuggerRunner.UpdateListViewInfoItems()
                End Select
            End Using
        Catch ex As Exception
            'TODO: Add better handle read support.
            '      For some reason saving a handle as float, it becomes massive. The int doesnt.
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub TextEditorControlEx_DebuggerSource_CaretPositionChanged_DisplayInfos(sender As Object, e As EventArgs)
        Try
            Dim iDebugLine As Integer = TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.TextArea.Caret.Position.Line
            Dim iRealLine As Integer = -1
            Dim sFilename As String = "Unknown"
            If (iDebugLine > -1 AndAlso iDebugLine < g_ClassDebuggerRunner.g_mSourceLinesInfo.Length) Then
                Dim info = g_ClassDebuggerRunner.g_mSourceLinesInfo(iDebugLine)
                iRealLine = info.iRealLine
                sFilename = IO.Path.GetFileName(info.sFile)

                If (String.IsNullOrEmpty(sFilename)) Then
                    If (String.IsNullOrEmpty(g_ClassDebuggerRunner.m_CurrentSourceFile)) Then
                        sFilename = "Unknown"
                    Else
                        sFilename = IO.Path.GetFileName(g_ClassDebuggerRunner.m_CurrentSourceFile)
                    End If
                End If
            End If

            ToolStripStatusLabel_EditorDebugLing.Text = String.Format("DL: {0}", iDebugLine + 1)
            ToolStripStatusLabel_EditorLine.Text = String.Format("L: {0} ({1})", iRealLine, sFilename)
            ToolStripStatusLabel_EditorCollum.Text = String.Format("C: {0}", TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.TextArea.Caret.Column)
            ToolStripStatusLabel_EditorSelected.Text = String.Format("S: {0}", TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText.Length)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub TextEditorControlEx_DebuggerSource_CaretPositionChanged(sender As Object, e As EventArgs)
        If (Not g_bTextEditorEnableClickSelect) Then
            Return
        End If

        If (TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
            Return
        End If

        g_bTextEditorEnableClickSelect = False
        g_bListViewEnableClickSelect = False

        Dim iCaretOffset As Integer = TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.Offset

        'Mark breakpoints
        If (True) Then
            For Each info In g_ClassDebuggerEntries.g_lBreakpointList
                Dim iOffset As Integer = info.iOffset
                Dim iLength As Integer = info.iLength
                Dim sGUID As String = info.sGUID

                If (iCaretOffset >= iOffset AndAlso iCaretOffset <= (iOffset + iLength)) Then
                    TabControl1.SelectTab(TabPage_Breakpoints)

                    Try
                        ListView_Breakpoints.BeginUpdate()

                        For i = 0 To ListView_Breakpoints.Items.Count - 1
                            ListView_Breakpoints.Items(i).Selected = False
                        Next

                        For i = 0 To ListView_Breakpoints.Items.Count - 1
                            Dim mListViewItemData = TryCast(ListView_Breakpoints.Items(i), ClassListViewItemData)
                            If (mListViewItemData Is Nothing) Then
                                Continue For
                            End If

                            If (CStr(mListViewItemData.g_mData("GUID")) = sGUID) Then
                                mListViewItemData.Selected = True
                                mListViewItemData.EnsureVisible()
                                Exit For
                            End If
                        Next
                    Finally
                        ListView_Breakpoints.EndUpdate()
                    End Try

                    Exit For
                End If
            Next
        End If

        'Mark watchers
        If (True) Then
            For Each info In g_ClassDebuggerEntries.g_lWatcherList
                Dim iOffset As Integer = info.iOffset
                Dim iLength As Integer = info.iLength
                Dim sGUID As String = info.sGUID

                If (iCaretOffset >= iOffset AndAlso iCaretOffset <= (iOffset + iLength)) Then
                    TabControl1.SelectTab(TabPage_Watchers)

                    Try
                        ListView_Watchers.BeginUpdate()

                        For i = 0 To ListView_Watchers.Items.Count - 1
                            ListView_Watchers.Items(i).Selected = False
                        Next

                        For i = 0 To ListView_Watchers.Items.Count - 1
                            Dim mListViewItemData = TryCast(ListView_Watchers.Items(i), ClassListViewItemData)
                            If (mListViewItemData Is Nothing) Then
                                Continue For
                            End If

                            If (CStr(mListViewItemData.g_mData("GUID")) = sGUID) Then
                                mListViewItemData.Selected = True
                                mListViewItemData.EnsureVisible()
                                Exit For
                            End If
                        Next
                    Finally
                        ListView_Watchers.EndUpdate()
                    End Try

                    Exit For
                End If
            Next
        End If

        'Mark asserts
        If (True) Then
            For Each info In g_ClassDebuggerEntries.g_lAssertList
                Dim iOffset As Integer = info.iOffset
                Dim iLength As Integer = info.iLength
                Dim sGUID As String = info.sGUID

                If (iCaretOffset >= iOffset AndAlso iCaretOffset <= (iOffset + iLength)) Then
                    TabControl1.SelectTab(TabPage_Asserts)

                    Try
                        ListView_Asserts.BeginUpdate()

                        For i = 0 To ListView_Asserts.Items.Count - 1
                            ListView_Asserts.Items(i).Selected = False
                        Next

                        For i = 0 To ListView_Asserts.Items.Count - 1
                            Dim mListViewItemData = TryCast(ListView_Asserts.Items(i), ClassListViewItemData)
                            If (mListViewItemData Is Nothing) Then
                                Continue For
                            End If

                            If (CStr(mListViewItemData.g_mData("GUID")) = sGUID) Then
                                mListViewItemData.Selected = True
                                mListViewItemData.EnsureVisible()
                                Exit For
                            End If
                        Next
                    Finally
                        ListView_Asserts.EndUpdate()
                    End Try

                    Exit For
                End If
            Next
        End If

        g_bListViewEnableClickSelect = True
        g_bTextEditorEnableClickSelect = True
    End Sub

#Region "ToolStrip Settings"
    Private Sub ToolStripMenuItem_SettingsCatchExceptions_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_SettingsCatchExceptions.CheckedChanged
        If (g_bIgnoreCheckedChangedEvent) Then
            Return
        End If

        SaveViews()
        UpdateViews()
    End Sub

    Private Sub ToolStripMenuItem_EntitiesEnableColor_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_EntitiesEnableColor.CheckedChanged
        If (g_bIgnoreCheckedChangedEvent) Then
            Return
        End If

        SaveViews()
        UpdateViews()
    End Sub

    Private Sub ToolStripMenuItem_EntitiesEnableShowNewEnts_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_EntitiesEnableShowNewEnts.CheckedChanged
        If (g_bIgnoreCheckedChangedEvent) Then
            Return
        End If

        SaveViews()
        UpdateViews()
    End Sub
#End Region

    Public Sub SaveViews()
        If (String.IsNullOrEmpty(Me.Name)) Then
            Return
        End If

        Using mStream = ClassFileStreamWait.Create(ClassSettings.g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT) From {
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "SettingsCatchExceptions", If(ToolStripMenuItem_SettingsCatchExceptions.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "EntitiesEnableColor", If(ToolStripMenuItem_EntitiesEnableColor.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "EntitiesEnableShowNewEnts", If(ToolStripMenuItem_EntitiesEnableShowNewEnts.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "EditorDistance", CStr(SplitContainer1.SplitterDistance)),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "InfotmationDistance", CStr(SplitContainer2.Height - SplitContainer2.SplitterDistance))
                }

                mIni.WriteKeyValue(lContent.ToArray)
            End Using
        End Using
    End Sub

    Public Sub LoadViews()
        If (String.IsNullOrEmpty(Me.Name)) Then
            Return
        End If

        Dim tmpStr As String
        Dim tmpInt As Integer

        Using mStream = ClassFileStreamWait.Create(ClassSettings.g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                g_bIgnoreCheckedChangedEvent = True
                ToolStripMenuItem_SettingsCatchExceptions.Checked = (mIni.ReadKeyValue(Me.Name, "SettingsCatchExceptions", "1") <> "0")
                ToolStripMenuItem_EntitiesEnableColor.Checked = (mIni.ReadKeyValue(Me.Name, "EntitiesEnableColor", "1") <> "0")
                ToolStripMenuItem_EntitiesEnableShowNewEnts.Checked = (mIni.ReadKeyValue(Me.Name, "EntitiesEnableShowNewEnts", "1") <> "0")
                g_bIgnoreCheckedChangedEvent = False

                tmpStr = mIni.ReadKeyValue(Me.Name, "EditorDistance", Nothing)
                If (tmpStr IsNot Nothing AndAlso Integer.TryParse(tmpStr, tmpInt) AndAlso tmpInt > -1) Then
                    SplitContainer1.SplitterDistance = tmpInt
                End If

                tmpStr = mIni.ReadKeyValue(Me.Name, "InfotmationDistance", Nothing)
                If (tmpStr IsNot Nothing AndAlso Integer.TryParse(tmpStr, tmpInt) AndAlso (SplitContainer2.Height - tmpInt) > -1) Then
                    SplitContainer2.SplitterDistance = (SplitContainer2.Height - tmpInt)
                End If
            End Using
        End Using

        UpdateViews()
    End Sub

    Public Sub UpdateViews()
        g_ClassDebuggerRunner.g_ClassSettings.m_SettingsCatchExceptions = ToolStripMenuItem_SettingsCatchExceptions.Checked
        g_ClassDebuggerRunner.g_ClassSettings.m_EntitiesEnableColor = ToolStripMenuItem_EntitiesEnableColor.Checked
        g_ClassDebuggerRunner.g_ClassSettings.m_EntitiesEnableShowNewEnts = ToolStripMenuItem_EntitiesEnableShowNewEnts.Checked
    End Sub

    Private Sub FormDebugger_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub CleanUp()
        g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnDebuggerEndPost(Me))

        RemoveHandler TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.PositionChanged, AddressOf TextEditorControlEx_DebuggerSource_CaretPositionChanged

        RemoveHandler TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.TextArea.SelectionManager.SelectionChanged, AddressOf TextEditorControlEx_DebuggerSource_CaretPositionChanged_DisplayInfos
        RemoveHandler TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.TextArea.Caret.PositionChanged, AddressOf TextEditorControlEx_DebuggerSource_CaretPositionChanged_DisplayInfos

        If (g_ClassDebuggerRunner IsNot Nothing) Then
            g_ClassDebuggerRunner.Dispose()
            g_ClassDebuggerRunner = Nothing
        End If
    End Sub

    Private Sub Timer_ConnectionCheck_Tick(sender As Object, e As EventArgs) Handles Timer_ConnectionCheck.Tick
        If (g_ClassDebuggerRunner.m_DebuggingState <> ClassDebuggerRunner.ENUM_DEBUGGING_STATE.STARTED) Then
            Return
        End If

        g_ClassDebuggerRunner.SetDebuggerStatusConnection(True)
    End Sub
End Class