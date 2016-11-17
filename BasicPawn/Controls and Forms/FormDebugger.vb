'BasicPawn
'Copyright(C) 2016 TheTimocop

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


Imports System.ComponentModel
Imports System.Text.RegularExpressions

Public Class FormDebugger
    Public g_mFormMain As FormMain
    Public g_ClassDebuggerParser As ClassDebuggerParser
    Public g_ClassDebuggerRunnerEngine As ClassDebuggerParser.ClassRunnerEngine
    Public g_ClassDebuggerRunner As ClassDebuggerRunner
    Public g_ClassDebuggerSettings As ClassDebuggerSettings

    Public g_sLastPreProcessSourceFile As String = ""

    Public g_bTextEditorEnableClickSelect As Boolean = True
    Public g_bListViewEnableClickSelect As Boolean = True

    Public Sub New(f As FormMain)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormMain = f

        g_ClassDebuggerParser = New ClassDebuggerParser(g_mFormMain)
        g_ClassDebuggerRunnerEngine = New ClassDebuggerParser.ClassRunnerEngine
        g_ClassDebuggerRunner = New ClassDebuggerRunner(Me)
        g_ClassDebuggerSettings = New ClassDebuggerSettings(Me)

        TextEditorControlEx_DebuggerSource.IsReadOnly = True
        TextEditorControlEx_DebuggerDiasm.IsReadOnly = True

        AddHandler TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.PositionChanged, AddressOf TextEditorControlEx_DebuggerSource_CaretPositionChanged

        AddHandler TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.TextArea.SelectionManager.SelectionChanged, AddressOf TextEditorControlEx_DebuggerSource_CaretPositionChanged_DisplayInfos
        AddHandler TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.TextArea.Caret.PositionChanged, AddressOf TextEditorControlEx_DebuggerSource_CaretPositionChanged_DisplayInfos
    End Sub

    Private Sub FormDebugger_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            'Create Pre-Process source
            Dim sLstSource As String = g_mFormMain.g_ClassTextEditorTools.GetCompilerPreProcessCode(True, True, g_sLastPreProcessSourceFile)
            If (String.IsNullOrEmpty(sLstSource)) Then
                MessageBox.Show("Could not open debugger. See information tab for more informations.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()

                Return
            End If

            If (String.IsNullOrEmpty(g_sLastPreProcessSourceFile)) Then
                Throw New ArgumentException("Last Pre-Process source invlaid")
            End If


            g_ClassDebuggerRunner.g_ClassPreProcess.FixPreProcessFiles(sLstSource)
            'Replace the temp source file with the currently opened one
            sLstSource = Regex.Replace(sLstSource,
                                       String.Format("^\s*\#file ""{0}""\s*$", Regex.Escape(g_sLastPreProcessSourceFile)),
                                       String.Format("#file ""{0}""", g_ClassDebuggerRunner.m_sCurrentSourceFile),
                                       RegexOptions.IgnoreCase Or RegexOptions.Multiline)

            g_ClassDebuggerRunner.g_ClassPreProcess.AnalysisSourceLines(sLstSource)
            g_ClassDebuggerParser.UpdateBreakpoints(sLstSource, False)
            g_ClassDebuggerParser.UpdateWatchers(sLstSource, False)


            'Create DIASM code
            Dim sAsmLstSource As String = sLstSource
            With New ClassDebuggerParser(g_mFormMain)
                .CleanupDebugPlaceholder(sAsmLstSource)
            End With
            Dim sAsmSource As String = g_mFormMain.g_ClassTextEditorTools.GetCompilerAssemblyCode(True, sAsmLstSource, Nothing)
            If (String.IsNullOrEmpty(sAsmSource)) Then
                MessageBox.Show("Could not open debugger. See information tab for more informations.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()

                Return
            End If



            TextEditorControlEx_DebuggerSource.Document.TextContent = sLstSource
            TextEditorControlEx_DebuggerDiasm.Document.TextContent = sAsmSource

            TextEditorControlEx_DebuggerSource.Refresh()
            TextEditorControlEx_DebuggerDiasm.Refresh()

            g_mFormMain.g_ClassSyntaxTools.UpdateTextEditorSyntax()

            'Add breakpoints
            ListView_Breakpoints.BeginUpdate()
            For Each breakpointItem In g_ClassDebuggerParser.g_lBreakpointList
                With ListView_Breakpoints.Items.Add(New ListViewItem(New String() {breakpointItem.iLine.ToString, breakpointItem.sArguments, "", breakpointItem.sGUID}))
                    .Checked = True
                End With

                Dim marker As New ICSharpCode.TextEditor.Document.TextMarker(breakpointItem.iOffset, breakpointItem.iTotalLenght, ICSharpCode.TextEditor.Document.TextMarkerType.Underlined, Color.DarkOrange)
                TextEditorControlEx_DebuggerSource.Document.MarkerStrategy.AddMarker(marker)
            Next
            ListView_Breakpoints.EndUpdate()

            'Add watchers
            ListView_Watchers.BeginUpdate()
            For Each breakpointItem In g_ClassDebuggerParser.g_lWatcherList
                ListView_Watchers.Items.Add(New ListViewItem(New String() {breakpointItem.iLine.ToString, breakpointItem.sArguments, "", "0", breakpointItem.sGUID}))

                Dim marker As New ICSharpCode.TextEditor.Document.TextMarker(breakpointItem.iOffset, breakpointItem.iTotalLenght, ICSharpCode.TextEditor.Document.TextMarkerType.Underlined, Color.DarkOrange)
                TextEditorControlEx_DebuggerSource.Document.MarkerStrategy.AddMarker(marker)
            Next
            ListView_Watchers.EndUpdate()

            'Add entities
            ListView_Entities.BeginUpdate()
            For i = 0 To 2048 - 1
                ListView_Entities.Items.Add(New ListViewItem(New String() {i.ToString, "", "", ""}))
            Next
            ListView_Entities.EndUpdate()

            ClassControlStyle.UpdateControls(Me)

            g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As BasicPawnPluginInterface.PluginInterface) j.OnDebuggerStart(Me))
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
            Me.Dispose()
        End Try
    End Sub

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

    Private Sub ToolStripMenuItem_ToolsCleanupTemp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsCleanupTemp.Click
        Try
            If (g_ClassDebuggerRunner.m_DebuggingState <> ClassDebuggerRunner.ENUM_DEBUGGING_STATE.STOPPED) Then
                MessageBox.Show("You can't clean temporary debugger files while the debugger or multiple debuggers are running!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

            Dim sGUID As String = ListView_Breakpoints.SelectedItems(0).SubItems(3).Text

            For Each item In g_ClassDebuggerParser.g_lBreakpointList
                If (item.sGUID <> sGUID) Then
                    Continue For
                End If

                Dim startLocation As New ICSharpCode.TextEditor.TextLocation(item.iIndex, item.iLine - 1)
                Dim endLocation As New ICSharpCode.TextEditor.TextLocation(item.iIndex + item.iTotalLenght, item.iLine - 1)

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

            Dim sGUID As String = ListView_Watchers.SelectedItems(0).SubItems(4).Text

            For Each item In g_ClassDebuggerParser.g_lWatcherList
                If (item.sGUID <> sGUID) Then
                    Continue For
                End If

                Dim startLocation As New ICSharpCode.TextEditor.TextLocation(item.iIndex, item.iLine - 1)
                Dim endLocation As New ICSharpCode.TextEditor.TextLocation(item.iIndex + item.iTotalLenght, item.iLine - 1)

                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.Position = startLocation
                TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.SelectionManager.SetSelection(startLocation, endLocation)

                Exit For
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub FormDebugger_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If (g_ClassDebuggerRunner.m_DebuggingState <> ClassDebuggerRunner.ENUM_DEBUGGING_STATE.STOPPED) Then
            MessageBox.Show("You need to stop the debugger first before closing!", "Debugger active!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
        If (Not String.IsNullOrEmpty(g_ClassDebuggerRunner.m_sGameFolder) AndAlso IO.Directory.Exists(g_ClassDebuggerRunner.m_sGameFolder)) Then
            While True
                For Each sFile As String In IO.Directory.GetFiles(g_ClassDebuggerRunner.m_sGameFolder)
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
                If (Not String.IsNullOrEmpty(g_ClassDebuggerRunner.m_sGameFolder) AndAlso IO.Directory.Exists(g_ClassDebuggerRunner.m_sGameFolder)) Then
                    For Each sFile As String In IO.Directory.GetFiles(g_ClassDebuggerRunner.m_sGameFolder)
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
        Dim sGUID As String = e.Item.SubItems(3).Text
        g_ClassDebuggerRunner.m_IgnoreBreakpointGUID(sGUID) = Not e.Item.Checked
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

                Select Case (i.ShowDialog)
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
            Dim sFile As String = "Unknown"
            If (iDebugLine > -1 AndAlso iDebugLine < g_ClassDebuggerRunner.g_mSourceLinesInfo.Length) Then
                Dim info = g_ClassDebuggerRunner.g_mSourceLinesInfo(iDebugLine)
                iRealLine = info.iRealLine
                sFile = IO.Path.GetFileName(info.sFile)

                If (String.IsNullOrEmpty(sFile)) Then
                    sFile = IO.Path.GetFileName(g_ClassDebuggerRunner.m_sCurrentSourceFile)
                End If
            End If

            ToolStripStatusLabel_EditorDebugLing.Text = String.Format("DL: {0}", iDebugLine + 1)
            ToolStripStatusLabel_EditorLine.Text = String.Format("L: {0} ({1})", iRealLine, sFile)
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
                Dim iLenght As Integer = info.iLenght
                Dim sGUID As String = info.sGUID

                If (iCaretOffset >= iOffset AndAlso iCaretOffset <= (iOffset + iLenght)) Then
                    'ListView_Breakpoints.Select()
                    TabControl1.SelectTab(0)

                    ListView_Breakpoints.BeginUpdate()
                    For i = 0 To ListView_Breakpoints.Items.Count - 1
                        ListView_Breakpoints.Items(i).Selected = False
                    Next

                    For i = 0 To ListView_Breakpoints.Items.Count - 1
                        If (ListView_Breakpoints.Items(i).SubItems(3).Text = sGUID) Then
                            ListView_Breakpoints.Items(i).Selected = True
                            ListView_Breakpoints.Items(i).EnsureVisible()
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
                Dim iLenght As Integer = info.iLenght
                Dim sGUID As String = info.sGUID

                If (iCaretOffset >= iOffset AndAlso iCaretOffset <= (iOffset + iLenght)) Then
                    'ListView_Watchers.Select()
                    TabControl1.SelectTab(1)

                    ListView_Watchers.BeginUpdate()
                    For i = 0 To ListView_Watchers.Items.Count - 1
                        ListView_Watchers.Items(i).Selected = False
                    Next

                    For i = 0 To ListView_Watchers.Items.Count - 1
                        If (ListView_Watchers.Items(i).SubItems(4).Text = sGUID) Then
                            ListView_Watchers.Items(i).Selected = True
                            ListView_Watchers.Items(i).EnsureVisible()
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

    Private Sub FormDebugger_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As BasicPawnPluginInterface.PluginInterface)
                                                               If (Not j.OnDebuggerEnd(Me)) Then
                                                                   e.Cancel = True
                                                               End If
                                                           End Sub)
    End Sub
#End Region
End Class