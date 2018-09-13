'BasicPawn
'Copyright(C) 2018 TheTimocop

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

Public Class FormDebugger
    Public g_mFormMain As FormMain
    Public g_sDebugTabIdentifier As String = ""

    Public g_ClassDebuggerParser As ClassDebuggerParser
    Public g_ClassDebuggerRunnerEngine As ClassDebuggerParser.ClassRunnerEngine
    Public g_ClassDebuggerRunner As ClassDebuggerRunner
    Public g_ClassDebuggerSettings As ClassDebuggerSettings

    Public g_sLastPreProcessSourceFile As String = ""
    Public g_iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN

    Public g_bTextEditorEnableClickSelect As Boolean = True
    Public g_bListViewEnableClickSelect As Boolean = True

    Private g_bPostLoad As Boolean = False

    Public Sub New(f As FormMain, mDebugTab As ClassTabControl.SourceTabPage)
        Me.New(f, mDebugTab.m_Identifier)
    End Sub

    Public Sub New(f As FormMain, sDebugTabIdentifier As String)
        g_mFormMain = f
        g_sDebugTabIdentifier = sDebugTabIdentifier

        If (g_mFormMain.g_ClassTabControl.GetTabIndexByIdentifier(g_sDebugTabIdentifier) < 0) Then
            Throw New ArgumentException("Tab does not exist")
        End If

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ToolStripStatusLabel_DebugState.Name &= "@KeepBackColor"
        ToolStripStatusLabel_NoConnection.Name &= "@KeepBackColor"
        StatusStrip_BPDebugger.Name &= "@NoCustomRenderer"

        g_ClassDebuggerParser = New ClassDebuggerParser(g_mFormMain)
        g_ClassDebuggerRunnerEngine = New ClassDebuggerParser.ClassRunnerEngine
        g_ClassDebuggerRunner = New ClassDebuggerRunner(Me)
        g_ClassDebuggerSettings = New ClassDebuggerSettings(Me)

        TextEditorControlEx_DebuggerSource.IsReadOnly = True
        RichTextBox_DisasmSource.ReadOnly = True

        RichTextBox_DisasmSource.Font = TextEditorControlEx_DebuggerSource.Font

        AddHandler TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.PositionChanged, AddressOf TextEditorControlEx_DebuggerSource_CaretPositionChanged

        AddHandler TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.TextArea.SelectionManager.SelectionChanged, AddressOf TextEditorControlEx_DebuggerSource_CaretPositionChanged_DisplayInfos
        AddHandler TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.TextArea.Caret.PositionChanged, AddressOf TextEditorControlEx_DebuggerSource_CaretPositionChanged_DisplayInfos
    End Sub

    Private Sub FormDebugger_Load(sender As Object, e As EventArgs) Handles Me.Load
        If (Not RefreshSource()) Then
            MessageBox.Show("Could not open debugger. See information tab for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Dispose()
            Return
        End If

        g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnDebuggerStart(Me))

        'Load window info
        ClassSettings.LoadWindowInfo(Me)

        g_bPostLoad = True
    End Sub

    Private Function RefreshSource() As Boolean
        Try
            Using mFormProgress As New FormProgress
                mFormProgress.Text = "BasicPawn Debugger - Generating source..."
                If (g_bPostLoad) Then
                    mFormProgress.Show(Me)
                Else
                    mFormProgress.Show(g_mFormMain)
                End If
                mFormProgress.m_Progress = 0


                If (g_mFormMain.g_ClassTabControl.GetTabIndexByIdentifier(g_sDebugTabIdentifier) < 0) Then
                    Throw New ArgumentException("Tab does not exist")
                End If

                g_ClassDebuggerRunner.UpdateSourceFromTab()
                Me.Text = String.Format("BasicPawn Debugger ({0})", If(String.IsNullOrEmpty(g_ClassDebuggerRunner.m_CurrentSourceFile), "Unnamed", IO.Path.GetFileName(g_ClassDebuggerRunner.m_CurrentSourceFile)))


                mFormProgress.m_Progress = 10


                Dim iCompilerType As ClassTextEditorTools.ENUM_COMPILER_TYPE = ClassTextEditorTools.ENUM_COMPILER_TYPE.UNKNOWN

                'Create Pre-Process source
                Dim sPreSource As String = g_mFormMain.g_ClassTextEditorTools.GetCompilerPreProcessCode(g_ClassDebuggerRunner.m_CurrentSource,
                                                                                                        True,
                                                                                                        True,
                                                                                                        g_sLastPreProcessSourceFile,
                                                                                                        g_ClassDebuggerRunner.m_CurrentConfig,
                                                                                                        Nothing,
                                                                                                        Nothing,
                                                                                                        Nothing,
                                                                                                        If(String.IsNullOrEmpty(g_ClassDebuggerRunner.m_CurrentSourceFile), "Unnamed", g_ClassDebuggerRunner.m_CurrentSourceFile),
                                                                                                        True,
                                                                                                        Nothing,
                                                                                                        iCompilerType)
                If (String.IsNullOrEmpty(sPreSource)) Then
                    Throw New ArgumentException("Invalid source")
                End If


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
                g_ClassDebuggerRunner.g_ClassPreProcess.FixPreProcessFilePaths(sPreSource, g_sLastPreProcessSourceFile, If(String.IsNullOrEmpty(g_ClassDebuggerRunner.m_CurrentSourceFile), "Unnamed", g_ClassDebuggerRunner.m_CurrentSourceFile))
                g_ClassDebuggerRunner.g_ClassPreProcess.AnalysisSourceLines(sPreSource)
                g_ClassDebuggerParser.UpdateBreakpoints(sPreSource, False, g_iLanguage)
                g_ClassDebuggerParser.UpdateWatchers(sPreSource, False, g_iLanguage)


                mFormProgress.m_Progress = 40


                'Create DIASM code
                Dim sAsmLstSource As String = sPreSource
                With New ClassDebuggerParser(g_mFormMain)
                    .CleanupDebugPlaceholder(sAsmLstSource, g_iLanguage)
                End With

                iCompilerType = ClassTextEditorTools.ENUM_COMPILER_TYPE.UNKNOWN

                Dim sAsmSource As String = g_mFormMain.g_ClassTextEditorTools.GetCompilerAssemblyCode(True,
                                                                                                      sAsmLstSource,
                                                                                                      Nothing,
                                                                                                      g_ClassDebuggerRunner.m_CurrentConfig,
                                                                                                      IO.Path.GetDirectoryName(g_sLastPreProcessSourceFile),
                                                                                                      Nothing,
                                                                                                      Nothing,
                                                                                                      Nothing,
                                                                                                      True,
                                                                                                      Nothing,
                                                                                                      iCompilerType)
                If (String.IsNullOrEmpty(sAsmSource)) Then
                    Throw New ArgumentException("Invalid source")
                End If

                If (iCompilerType <> ClassTextEditorTools.ENUM_COMPILER_TYPE.SOURCEPAWN) Then
                    Throw New ArgumentException("Unsupported compiler")
                End If


                mFormProgress.m_Progress = 60


                TextEditorControlEx_DebuggerSource.Document.TextContent = sPreSource
                RichTextBox_DisasmSource.Text = sAsmSource

                TextEditorControlEx_DebuggerSource.Refresh()
                RichTextBox_DisasmSource.Refresh()

                g_mFormMain.g_ClassSyntaxTools.g_ClassSyntaxHighlighting.UpdateTextEditorSyntax()

                TextEditorControlEx_DebuggerSource.Document.MarkerStrategy.RemoveAll(Function() True)


                mFormProgress.m_Progress = 80


                'Add breakpoints
                ListView_Breakpoints.BeginUpdate()
                ListView_Breakpoints.Items.Clear()
                For Each mBreakpointItem In g_ClassDebuggerParser.g_lBreakpointList
                    Dim mListViewItemData As New ClassListViewItemData(New String() {mBreakpointItem.iLine.ToString, mBreakpointItem.sArguments, ""})
                    mListViewItemData.g_mData("GUID") = mBreakpointItem.sGUID

                    With ListView_Breakpoints.Items.Add(mListViewItemData)
                        .Checked = True
                    End With

                    Dim marker As New ICSharpCode.TextEditor.Document.TextMarker(mBreakpointItem.iOffset, mBreakpointItem.iTotalLength, ICSharpCode.TextEditor.Document.TextMarkerType.Underlined, Color.DarkOrange)
                    TextEditorControlEx_DebuggerSource.Document.MarkerStrategy.AddMarker(marker)
                Next
                ListView_Breakpoints.EndUpdate()

                'Add watchers
                ListView_Watchers.BeginUpdate()
                ListView_Watchers.Items.Clear()
                For Each mWatcherItem In g_ClassDebuggerParser.g_lWatcherList
                    Dim mListViewItemData As New ClassListViewItemData(New String() {mWatcherItem.iLine.ToString, mWatcherItem.sArguments, "", "0"})
                    mListViewItemData.g_mData("GUID") = mWatcherItem.sGUID

                    ListView_Watchers.Items.Add(mListViewItemData)

                    Dim marker As New ICSharpCode.TextEditor.Document.TextMarker(mWatcherItem.iOffset, mWatcherItem.iTotalLength, ICSharpCode.TextEditor.Document.TextMarkerType.Underlined, Color.DarkOrange)
                    TextEditorControlEx_DebuggerSource.Document.MarkerStrategy.AddMarker(marker)
                Next
                ListView_Watchers.EndUpdate()

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

            For Each mItem In g_ClassDebuggerParser.g_lBreakpointList
                If (mItem.sGUID <> sGUID) Then
                    Continue For
                End If

                Dim startLocation As New ICSharpCode.TextEditor.TextLocation(mItem.iIndex, mItem.iLine - 1)
                Dim endLocation As New ICSharpCode.TextEditor.TextLocation(mItem.iIndex + mItem.iTotalLength, mItem.iLine - 1)

                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.Position = startLocation
                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.SelectionManager.SetSelection(startLocation, endLocation)

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

            For Each item In g_ClassDebuggerParser.g_lWatcherList
                If (item.sGUID <> sGUID) Then
                    Continue For
                End If

                Dim startLocation As New ICSharpCode.TextEditor.TextLocation(item.iIndex, item.iLine - 1)
                Dim endLocation As New ICSharpCode.TextEditor.TextLocation(item.iIndex + item.iTotalLength, item.iLine - 1)

                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.Position = startLocation
                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.SelectionManager.SetSelection(startLocation, endLocation)

                Exit For
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub FormDebugger_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'Save window info
        ClassSettings.SaveWindowInfo(Me)

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

                    If (IO.Path.GetExtension(sFile).ToLower <> ClassDebuggerParser.g_sDebuggerFilesExt.ToLower) Then
                        Continue For
                    End If

                    Exit While
                Next

                Return
            End While
        ElseIf (Not bNoErrorPrompt) Then
            MessageBox.Show("Invalid game directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Select Case (If(bNoPrompt, DialogResult.Yes, MessageBox.Show("Cleanup BasicPawn Debugger temporary files?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question)))
            Case DialogResult.Yes
                If (Not String.IsNullOrEmpty(g_ClassDebuggerRunner.m_ServerFolder) AndAlso IO.Directory.Exists(g_ClassDebuggerRunner.m_ServerFolder)) Then
                    For Each sFile As String In IO.Directory.GetFiles(g_ClassDebuggerRunner.m_ServerFolder)
                        If (Not IO.File.Exists(sFile)) Then
                            Continue For
                        End If

                        If (IO.Path.GetExtension(sFile).ToLower <> ClassDebuggerParser.g_sDebuggerFilesExt.ToLower) Then
                            Continue For
                        End If

                        Try
                            IO.File.Delete(sFile)
                        Catch ex As Exception
                            ClassExceptionLog.WriteToLogMessageBox(ex)
                        End Try
                    Next
                ElseIf (Not bNoErrorPrompt) Then
                    MessageBox.Show("Invalid game directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            If (String.IsNullOrEmpty(g_ClassDebuggerRunner.g_mActiveBreakpointValue.sGUID)) Then
                MessageBox.Show("There is currently no active breakpoint!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Using i As New FormDebuggerBreakpointSetValue
                If (g_ClassDebuggerRunner.g_mActiveBreakpointValue.bReturnCustomValue) Then
                    Select Case (g_ClassDebuggerRunner.g_mActiveBreakpointValue.mValueType)
                        Case ClassDebuggerRunner.ENUM_BREAKPOINT_VALUE_TYPE.INTEGER
                            i.RadioButton_TypeInteger.Checked = True

                            Dim iInt As Decimal
                            If (Not Decimal.TryParse(g_ClassDebuggerRunner.g_mActiveBreakpointValue.sIntegerValue, iInt)) Then
                                iInt = 0
                            End If

                            i.NumericUpDown_BreakpointValue.Value = iInt
                        Case Else
                            i.RadioButton_TypeFloatingPoint.Checked = True

                            Dim iFloat As Decimal
                            If (Not Decimal.TryParse(g_ClassDebuggerRunner.g_mActiveBreakpointValue.sFloatValue.Replace(".", ","), iFloat)) Then
                                iFloat = 0
                            End If

                            i.NumericUpDown_BreakpointValue.Value = iFloat
                    End Select
                Else
                    Dim iOrgInt As Decimal
                    Dim iOrgFloat As Decimal
                    If (Not Decimal.TryParse(g_ClassDebuggerRunner.g_mActiveBreakpointValue.sOrginalIntegerValue, iOrgInt)) Then
                        iOrgInt = 0
                    End If
                    If (Not Decimal.TryParse(g_ClassDebuggerRunner.g_mActiveBreakpointValue.sOrginalFloatValue.Replace(".", ","), iOrgFloat)) Then
                        iOrgFloat = 0.0D
                    End If

                    If (iOrgInt <> 0 AndAlso iOrgFloat = 0.0) Then
                        i.RadioButton_TypeInteger.Checked = True
                        i.NumericUpDown_BreakpointValue.Value = CDec(g_ClassDebuggerRunner.g_mActiveBreakpointValue.sOrginalIntegerValue)
                    ElseIf (iOrgInt = 0 AndAlso iOrgFloat = 0.0) Then
                        i.RadioButton_TypeInteger.Checked = True
                        i.NumericUpDown_BreakpointValue.Value = CDec(g_ClassDebuggerRunner.g_mActiveBreakpointValue.sOrginalIntegerValue)
                    Else
                        i.RadioButton_TypeFloatingPoint.Checked = True
                        i.NumericUpDown_BreakpointValue.Value = CDec(g_ClassDebuggerRunner.g_mActiveBreakpointValue.sOrginalFloatValue.Replace(".", ","))
                    End If
                End If

                Select Case (i.ShowDialog(Me))
                    Case DialogResult.OK
                        g_ClassDebuggerRunner.g_mActiveBreakpointValue.bReturnCustomValue = True

                        Select Case (True)
                            Case i.RadioButton_TypeInteger.Checked
                                g_ClassDebuggerRunner.g_mActiveBreakpointValue.mValueType = ClassDebuggerRunner.ENUM_BREAKPOINT_VALUE_TYPE.INTEGER
                                g_ClassDebuggerRunner.g_mActiveBreakpointValue.sIntegerValue = Math.Round(i.NumericUpDown_BreakpointValue.Value).ToString
                                g_ClassDebuggerRunner.g_mActiveBreakpointValue.sFloatValue = "0.0"
                            Case Else
                                g_ClassDebuggerRunner.g_mActiveBreakpointValue.mValueType = ClassDebuggerRunner.ENUM_BREAKPOINT_VALUE_TYPE.FLOAT
                                g_ClassDebuggerRunner.g_mActiveBreakpointValue.sIntegerValue = "0"
                                g_ClassDebuggerRunner.g_mActiveBreakpointValue.sFloatValue = i.NumericUpDown_BreakpointValue.Value.ToString.Replace(",", ".")
                        End Select

                        'Update values in the ListView
                        g_ClassDebuggerRunner.UpdateBreakpointListView()

                    Case DialogResult.Abort
                        g_ClassDebuggerRunner.g_mActiveBreakpointValue.bReturnCustomValue = False

                        'Update values in the ListView
                        g_ClassDebuggerRunner.UpdateBreakpointListView()
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
            For Each info In g_ClassDebuggerParser.g_lBreakpointList
                Dim iOffset As Integer = info.iOffset
                Dim iLength As Integer = info.iLength
                Dim sGUID As String = info.sGUID

                If (iCaretOffset >= iOffset AndAlso iCaretOffset <= (iOffset + iLength)) Then
                    'ListView_Breakpoints.Select()
                    TabControl1.SelectTab(0)

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
                    ListView_Breakpoints.EndUpdate()

                    Exit For
                End If
            Next
        End If

        'Mark watchers
        If (True) Then
            For Each info In g_ClassDebuggerParser.g_lWatcherList
                Dim iOffset As Integer = info.iOffset
                Dim iLength As Integer = info.iLength
                Dim sGUID As String = info.sGUID

                If (iCaretOffset >= iOffset AndAlso iCaretOffset <= (iOffset + iLength)) Then
                    'ListView_Watchers.Select()
                    TabControl1.SelectTab(1)

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
                    ListView_Watchers.EndUpdate()

                    Exit For
                End If
            Next
        End If

        g_bListViewEnableClickSelect = True
        g_bTextEditorEnableClickSelect = True
    End Sub

#Region "ToolStrip Settings"
    Class ClassDebuggerSettings
        Private g_mFormDebugger As FormDebugger
        Public g_bIgnoreSaveing As Boolean = False

        Public Sub New(f As FormDebugger)
            g_mFormDebugger = f
        End Sub

        Public Sub SaveSettings()
            If (g_bIgnoreSaveing) Then
                Return
            End If

            ClassSettings.g_iSettingsDebuggerCatchExceptions = g_mFormDebugger.ToolStripMenuItem_SettingsCatchExceptions.Checked
            ClassSettings.g_iSettingsDebuggerEntitiesEnableColoring = g_mFormDebugger.ToolStripMenuItem_EntitiesEnableColor.Checked
            ClassSettings.g_iSettingsDebuggerEntitiesEnableAutoScroll = g_mFormDebugger.ToolStripMenuItem_EntitiesEnableShowNewEnts.Checked
        End Sub

        Public Sub LoadSettings()
            g_bIgnoreSaveing = True

            g_mFormDebugger.ToolStripMenuItem_SettingsCatchExceptions.Checked = ClassSettings.g_iSettingsDebuggerCatchExceptions
            g_mFormDebugger.ToolStripMenuItem_EntitiesEnableColor.Checked = ClassSettings.g_iSettingsDebuggerEntitiesEnableColoring
            g_mFormDebugger.ToolStripMenuItem_EntitiesEnableShowNewEnts.Checked = ClassSettings.g_iSettingsDebuggerEntitiesEnableAutoScroll

            g_bIgnoreSaveing = False
        End Sub
    End Class

    Private Sub ToolStripMenuItem_SettingsCatchExceptions_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_SettingsCatchExceptions.CheckedChanged
        g_ClassDebuggerSettings.SaveSettings()
    End Sub

    Private Sub ToolStripMenuItem_EntitiesEnableColor_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_EntitiesEnableColor.CheckedChanged
        g_ClassDebuggerSettings.SaveSettings()
    End Sub

    Private Sub ToolStripMenuItem_EntitiesEnableShowNewEnts_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_EntitiesEnableShowNewEnts.CheckedChanged
        g_ClassDebuggerSettings.SaveSettings()
    End Sub

    Private Sub ToolStripMenuItem_Tools_DropDownOpened(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tools.DropDownOpened
        g_ClassDebuggerSettings.LoadSettings()
    End Sub
#End Region

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