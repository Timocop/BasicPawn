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
Imports System.Text


Public Class FormMain
    Public g_ClassSyntaxUpdater As ClassSyntaxUpdater
    Public g_ClassSyntaxTools As ClassSyntaxTools
    Public g_ClassAutocompleteUpdater As ClassAutocompleteUpdater
    Public g_ClassTextEditorTools As ClassTextEditorTools
    Public g_ClassLineState As ClassTextEditorTools.ClassLineState
    Public g_ClassCustomHighlighting As ClassTextEditorTools.ClassCustomHighlighting
    Public g_ClassTabControl As ClassTabControl

    Public g_mSourceSyntaxSourceAnalysis As ClassSyntaxTools.ClassSyntaxSourceAnalysis

    Public g_mUCAutocomplete As UCAutocomplete
    Public g_mUCInformationList As UCInformationList
    Public g_mUCObjectBrowser As UCObjectBrowser
    Public g_mUCToolTip As UCToolTip
    Public g_mFormDebugger As FormDebugger

    Public g_cDarkTextEditorBackgroundColor As Color = Color.FromArgb(255, 26, 26, 26)
    Public g_cDarkFormDetailsBackgroundColor As Color = Color.FromArgb(255, 24, 24, 24)
    Public g_cDarkFormBackgroundColor As Color = Color.FromArgb(255, 48, 48, 48)
    Public g_cDarkFormMenuBackgroundColor As Color = Color.FromArgb(255, 64, 64, 64)

    Public Class STRUC_AUTOCOMPLETE
        Public sInfo As String
        Public sFile As String
        Public mType As ENUM_TYPE_FLAGS
        Public sFunctionName As String
        Public sFullFunctionName As String

        Enum ENUM_TYPE_FLAGS
            NONE = 0
            DEBUG = (1 << 0)
            DEFINE = (1 << 1)
            [ENUM] = (1 << 2)
            FUNCENUM = (1 << 3)
            FUNCTAG = (1 << 4)
            STOCK = (1 << 5)
            [STATIC] = (1 << 6)
            [CONST] = (1 << 7)
            [PUBLIC] = (1 << 8)
            NATIVE = (1 << 9)
            FORWARD = (1 << 10)
            TYPESET = (1 << 11)
            METHODMAP = (1 << 12)
            TYPEDEF = (1 << 13)
            VARIABLE = (1 << 14)
            PUBLICVAR = (1 << 15)
            [PROPERTY] = (1 << 16)
            [FUNCTION] = (1 << 17)
        End Enum

        Public Function ParseTypeFullNames(sStr As String) As ENUM_TYPE_FLAGS
            Return ParseTypeNames(sStr.Split(New String() {" "}, 0))
        End Function

        Public Function ParseTypeNames(sStr As String()) As ENUM_TYPE_FLAGS
            Dim mTypes As ENUM_TYPE_FLAGS = ENUM_TYPE_FLAGS.NONE

            For i = 0 To sStr.Length - 1
                Select Case (sStr(i))
                    Case "debug" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.DEBUG)
                    Case "define" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.DEFINE)
                    Case "enum" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.ENUM)
                    Case "funcenum" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FUNCENUM)
                    Case "functag" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FUNCTAG)
                    Case "stock" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.STOCK)
                    Case "static" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.STATIC)
                    Case "const" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.CONST)
                    Case "public" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PUBLIC)
                    Case "native" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.NATIVE)
                    Case "forward" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FORWARD)
                    Case "typeset" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.TYPESET)
                    Case "methodmap" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.METHODMAP)
                    Case "typedef" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.TYPEDEF)
                    Case "variable" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.VARIABLE)
                    Case "publicvar" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PUBLICVAR)
                    Case "property" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PROPERTY)
                    Case "function" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FUNCTION)
                End Select
            Next

            Return mTypes
        End Function

        Public Function GetTypeNames() As String()
            Dim lNames As New List(Of String)

            If ((mType And ENUM_TYPE_FLAGS.DEBUG) = ENUM_TYPE_FLAGS.DEBUG) Then lNames.Add("debug")
            If ((mType And ENUM_TYPE_FLAGS.DEFINE) = ENUM_TYPE_FLAGS.DEFINE) Then lNames.Add("define")
            If ((mType And ENUM_TYPE_FLAGS.ENUM) = ENUM_TYPE_FLAGS.ENUM) Then lNames.Add("enum")
            If ((mType And ENUM_TYPE_FLAGS.FUNCENUM) = ENUM_TYPE_FLAGS.FUNCENUM) Then lNames.Add("funcenum")
            If ((mType And ENUM_TYPE_FLAGS.FUNCTAG) = ENUM_TYPE_FLAGS.FUNCTAG) Then lNames.Add("functag")
            If ((mType And ENUM_TYPE_FLAGS.STOCK) = ENUM_TYPE_FLAGS.STOCK) Then lNames.Add("stock")
            If ((mType And ENUM_TYPE_FLAGS.STATIC) = ENUM_TYPE_FLAGS.STATIC) Then lNames.Add("static")
            If ((mType And ENUM_TYPE_FLAGS.CONST) = ENUM_TYPE_FLAGS.CONST) Then lNames.Add("const")
            If ((mType And ENUM_TYPE_FLAGS.PUBLIC) = ENUM_TYPE_FLAGS.PUBLIC) Then lNames.Add("public")
            If ((mType And ENUM_TYPE_FLAGS.NATIVE) = ENUM_TYPE_FLAGS.NATIVE) Then lNames.Add("native")
            If ((mType And ENUM_TYPE_FLAGS.FORWARD) = ENUM_TYPE_FLAGS.FORWARD) Then lNames.Add("forward")
            If ((mType And ENUM_TYPE_FLAGS.TYPESET) = ENUM_TYPE_FLAGS.TYPESET) Then lNames.Add("typeset")
            If ((mType And ENUM_TYPE_FLAGS.METHODMAP) = ENUM_TYPE_FLAGS.METHODMAP) Then lNames.Add("methodmap")
            If ((mType And ENUM_TYPE_FLAGS.TYPEDEF) = ENUM_TYPE_FLAGS.TYPEDEF) Then lNames.Add("typedef")
            If ((mType And ENUM_TYPE_FLAGS.VARIABLE) = ENUM_TYPE_FLAGS.VARIABLE) Then lNames.Add("variable")
            If ((mType And ENUM_TYPE_FLAGS.PUBLICVAR) = ENUM_TYPE_FLAGS.PUBLICVAR) Then lNames.Add("publicvar")
            If ((mType And ENUM_TYPE_FLAGS.PROPERTY) = ENUM_TYPE_FLAGS.PROPERTY) Then lNames.Add("property")
            If ((mType And ENUM_TYPE_FLAGS.FUNCTION) = ENUM_TYPE_FLAGS.FUNCTION) Then lNames.Add("function")

            Return lNames.ToArray
        End Function

        Public Function GetTypeFullNames() As String
            Return String.Join(" ", GetTypeNames())
        End Function
    End Class



#Region "GUI Stuff"
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_ClassSyntaxUpdater = New ClassSyntaxUpdater(Me)
        g_ClassSyntaxTools = New ClassSyntaxTools(Me)
        g_ClassAutocompleteUpdater = New ClassAutocompleteUpdater(Me)
        g_ClassTextEditorTools = New ClassTextEditorTools(Me)
        g_ClassLineState = New ClassTextEditorTools.ClassLineState(Me)
        g_ClassCustomHighlighting = New ClassTextEditorTools.ClassCustomHighlighting(Me)
        g_ClassTabControl = New ClassTabControl(Me)

        ' Load other Forms/Controls
        g_mUCAutocomplete = New UCAutocomplete(Me)
        g_mUCAutocomplete.Parent = TabPage_Autocomplete
        g_mUCAutocomplete.Dock = DockStyle.Fill
        g_mUCAutocomplete.Show()

        g_mUCInformationList = New UCInformationList(Me)
        g_mUCInformationList.Parent = TabPage_Information
        g_mUCInformationList.Dock = DockStyle.Fill
        g_mUCInformationList.Show()

        g_mUCObjectBrowser = New UCObjectBrowser(Me)
        g_mUCObjectBrowser.Parent = TabPage_ObjectBrowser
        g_mUCObjectBrowser.Dock = DockStyle.Fill
        g_mUCObjectBrowser.Show()

        g_mUCToolTip = New UCToolTip(Me)
        g_mUCToolTip.Parent = SplitContainer_ToolboxAndEditor.Panel2
        g_mUCToolTip.BringToFront()
        g_mUCToolTip.Hide()

        SplitContainer_ToolboxSourceAndDetails.SplitterDistance = SplitContainer_ToolboxSourceAndDetails.Height - 175
    End Sub

    Public Sub UpdateFormConfigText()
        ToolStripStatusLabel_CurrentConfig.Text = "Config: " & If(String.IsNullOrEmpty(ClassSettings.g_sConfigName), "Default", ClassSettings.g_sConfigName)
    End Sub

    Public Sub PrintInformation(sType As String, sMessage As String, Optional bClear As Boolean = False, Optional bShowInformationTab As Boolean = False, Optional iLatestNoDuplicateLines As Integer = 0)
        Me.BeginInvoke(
            Sub()
                If (g_mUCInformationList Is Nothing) Then
                    Return
                End If

                Dim bExist As Boolean = False

                If (iLatestNoDuplicateLines > 0) Then
                    For Each item As String In g_mUCInformationList.ListBox_Information.Items
                        If (iLatestNoDuplicateLines < 1) Then
                            Exit For
                        End If

                        If (item.StartsWith(sType) AndAlso item.EndsWith(sMessage)) Then
                            bExist = True
                            Exit For
                        End If

                        iLatestNoDuplicateLines -= 1
                    Next
                End If

                If (bClear) Then
                    g_mUCInformationList.ListBox_Information.Items.Clear()
                End If

                If (Not bExist) Then
                    g_mUCInformationList.ListBox_Information.Items.Insert(0, String.Format("{0} ({1}) {2}", sType, Now.ToString, sMessage))
                End If

                ToolStripStatusLabel_LastInformation.Text = sMessage

                If (bShowInformationTab) Then
                    SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False
                    SplitContainer_ToolboxSourceAndDetails.SplitterDistance = SplitContainer_ToolboxSourceAndDetails.Height - 200
                    TabControl_Details.SelectTab(1)
                End If
            End Sub)
    End Sub

#End Region

#Region "Syntax Stuff"


    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ToolStripStatusLabel_AppVersion.Text = String.Format("v.{0}", Application.ProductVersion)

        'Some control init
        ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndex = 0

        'Load Settings 
        ClassSettings.LoadSettings()

        g_ClassTabControl.Init()

        'Load source files via Arguments
        Dim sArgs As String() = Environment.GetCommandLineArgs
        For i = 1 To sArgs.Length - 1
            If (i = 1) Then
                g_ClassTabControl.RemoveTab(0, False)
            End If

            g_ClassTabControl.AddTab(False)
            g_ClassTabControl.OpenFileTab(g_ClassTabControl.m_TabsCount - 1, sArgs(i), True)
        Next

        'Update Autocomplete
        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

        'UpdateTextEditorControl1Colors()
        g_ClassSyntaxTools.UpdateFormColors()

        g_ClassSyntaxUpdater.StartThread()
    End Sub

#End Region


#Region "Open/Save/Dialog"
    Private Sub FormMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        For i = 0 To g_ClassTabControl.m_TabsCount - 1
            If (g_ClassTabControl.PromptSaveTab(i)) Then
                e.Cancel = True
            End If
        Next
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip_RightClick.Opening
        g_mUCAutocomplete.UpdateAutocomplete("")
        g_mUCAutocomplete.g_ClassToolTip.CurrentMethod = ""
        g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
    End Sub
#End Region

#Region "ContextMenuStrip"
    Private Sub ToolStripMenuItem_Mark_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Mark.Click
        g_ClassTextEditorTools.MarkSelectedWord()
    End Sub

    Private Sub ToolStripMenuItem_Cut_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Cut.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_Copy_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Copy.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_Paste_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Paste.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e)
    End Sub
#End Region

#Region "MenuStrip"

#Region "MenuStrip_File"
    Private Sub ToolStripMenuItem_FileNew_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileNew.Click
        g_ClassTabControl.AddTab(False, True, True)

        PrintInformation("[INFO]", "User created a new source file")
    End Sub

    Private Sub ToolStripMenuItem_FileOpen_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileOpen.Click
        Using i As New OpenFileDialog
            i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|Pawn (Not fully supported)|*.pwn;*.p|AMX Mod X|*.sma|All files|*.*"
            i.FileName = g_ClassTabControl.m_ActiveTab.m_File
            i.Multiselect = True

            If (i.ShowDialog = DialogResult.OK) Then
                For Each sFile As String In i.FileNames
                    g_ClassTabControl.AddTab(False)
                    g_ClassTabControl.OpenFileTab(g_ClassTabControl.m_TabsCount - 1, sFile)
                Next
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_FileSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSave.Click
        g_ClassTabControl.SaveFileTab(g_ClassTabControl.m_ActiveTabIndex)
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAll.Click
        For i = 0 To g_ClassTabControl.m_TabsCount - 1
            g_ClassTabControl.SaveFileTab(i)
        Next
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAs.Click
        g_ClassTabControl.SaveFileTab(g_ClassTabControl.m_ActiveTabIndex, True)
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAsTemp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAsTemp.Click
        Dim sTempFile As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
        IO.File.WriteAllText(sTempFile, "")

        g_ClassTabControl.m_ActiveTab.m_File = sTempFile
        g_ClassTabControl.SaveFileTab(g_ClassTabControl.m_ActiveTabIndex)
    End Sub


    Private Sub ToolStripMenuItem_FileExit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileExit.Click
        Me.Close()
    End Sub
#End Region

#Region "MenuStrip_Tools"
    Private Sub ToolStripMenuItem_ToolsSettingsAndConfigs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsSettingsAndConfigs.Click
        Using i As New FormSettings(Me)
            If (i.ShowDialog() = DialogResult.OK) Then
                UpdateFormConfigText()

                g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

                g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Refresh()

                g_ClassSyntaxTools.UpdateFormColors()
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_ToolsFormatCode_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsFormatCode.Click
        Try
            Dim sSource As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent

            sSource = g_ClassSyntaxTools.FormatCode(sSource)

            g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.StartUndoGroup()
            g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Remove(0, g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextLength)
            g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Insert(0, sSource)
            g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.EndUndoGroup()
            g_ClassTabControl.m_ActiveTab.m_TextEditor.Refresh()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ToolsSearchReplace_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsSearchReplace.Click
        If (g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
            g_ClassTextEditorTools.ShowSearchAndReplace(g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SelectedText)
        Else
            g_ClassTextEditorTools.ShowSearchAndReplace("")
        End If
    End Sub

    Private Sub ToolStripMenuItem_ToolsShowInformation_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsShowInformation.Click
        SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False
        SplitContainer_ToolboxSourceAndDetails.SplitterDistance = SplitContainer_ToolboxSourceAndDetails.Height - 200
        TabControl_Details.SelectTab(1)
    End Sub

    Private Sub ToolStripMenuItem_ToolsClearInformationLog_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsClearInformationLog.Click
        PrintInformation("[INFO]", "Information log cleaned!", True)
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocompleteUpdate_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocompleteUpdate.Click
        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)
    End Sub

    Private Sub ToolStripComboBox_ToolsAutocompleteSyntax_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndexChanged
        Select Case (ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndex)
            Case 0
                ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX
            Case 1
                ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6
            Case 2
                ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7
        End Select

        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocompleteShowAutocomplete_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocompleteShowAutocomplete.Click
        SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False
        SplitContainer_ToolboxSourceAndDetails.SplitterDistance = SplitContainer_ToolboxSourceAndDetails.Height - 200
        TabControl_Details.SelectTab(0)
    End Sub

    Private Sub ToolStripMenuItem_ListReferences_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ListReferences.Click
        g_ClassTextEditorTools.ListReferences()
    End Sub
#End Region

#Region "MenuStrip_Build"
    Private Sub ToolStripMenuItem_Build_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Build.Click
        With New ClassDebuggerParser(Me)
            If (.HasDebugPlaceholder(g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent)) Then
                Select Case (MessageBox.Show("All BasicPawn Debugger placeholders need to be removed before compiling the source. Remove all placeholder?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
                    Case DialogResult.OK
                        .CleanupDebugPlaceholder(Me)
                    Case Else
                        Return
                End Select
            End If
        End With

        g_ClassTextEditorTools.CompileSource(False)
    End Sub
#End Region

#Region "MenuStrip_Test"
    Private Sub ToolStripMenuItem_Test_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Test.Click
        With New ClassDebuggerParser(Me)
            If (.HasDebugPlaceholder(g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent)) Then
                Select Case (MessageBox.Show("All BasicPawn Debugger placeholders need to be removed before compiling the source. Remove all placeholder?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
                    Case DialogResult.OK
                        .CleanupDebugPlaceholder(Me)
                    Case Else
                        Return
                End Select
            End If
        End With

        g_ClassTextEditorTools.CompileSource(True)
    End Sub
#End Region

#Region "MenuStrip_Debug"
    Private Sub ToolStripMenuItem_Debug_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Debug.Click
        If (g_mFormDebugger Is Nothing OrElse g_mFormDebugger.IsDisposed) Then
            g_mFormDebugger = New FormDebugger(Me)
            g_mFormDebugger.Show()
        End If
    End Sub
#End Region

#Region "MenuStrip_Shell"
    Private Sub ToolStripMenuItem_Shell_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Shell.Click
        Try
            Dim sShell As String = ClassSettings.g_sConfigExecuteShell

            For Each shellModule In ClassSettings.GetShellArguments(Me)
                sShell = sShell.Replace(shellModule.g_sMarker, shellModule.g_sArgument)
            Next

            Try
                If (String.IsNullOrEmpty(sShell)) Then
                    Throw New ArgumentException("Shell is empty")
                End If

                Shell(sShell, AppWinStyle.NormalFocus)
            Catch ex As Exception
                MessageBox.Show(ex.Message & Environment.NewLine & Environment.NewLine & sShell, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
#End Region

#Region "MenuStrip_Help"
    Private Sub ToolStripMenuItem_HelpAbout_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_HelpAbout.Click
        Dim SB As New StringBuilder
        SB.AppendLine(String.Format("{0} v.{1}", Application.ProductName, Application.ProductVersion))
        SB.AppendLine("Created by Externet (aka Timocop)")
        SB.AppendLine()
        SB.AppendLine("Source and Releases")
        SB.AppendLine("     https://github.com/Timocop/BasicPawn")
        SB.AppendLine()
        SB.AppendLine("Third-Party tools:")
        SB.AppendLine("     SharpDevelop - TextEditor (LGPL-2.1)")
        SB.AppendLine()
        SB.AppendLine("     Authors:")
        SB.AppendLine("     Daniel Grunwald and SharpDevelop Community")
        SB.AppendLine("     https://github.com/icsharpcode/SharpDevelop")
        MessageBox.Show(SB.ToString, "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

#Region "MenuStrip_Undo"
    Private Sub ToolStripMenuItem_Undo_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Undo.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.Undo()
    End Sub
#End Region

#Region "MenuStrip_Redo"
    Private Sub ToolStripMenuItem_Redo_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Redo.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.Redo()
    End Sub
#End Region


    Private Sub ToolStripStatusLabel_CurrentConfig_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel_CurrentConfig.Click
        Using i As New FormSettings(Me)
            i.TabControl1.SelectTab(1)
            If (i.ShowDialog() = DialogResult.OK) Then
                UpdateFormConfigText()

                g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

                g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Refresh()

                g_ClassSyntaxTools.UpdateFormColors()
            End If
        End Using
    End Sub

#End Region


    Private Sub ToolStripMenuItem_DebuggerBreakpointInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointInsert.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorInsertBreakpointAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointRemove.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorRemoveBreakpointAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointRemoveAll.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorRemoveAllBreakpoints()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherInsert.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorInsertWatcherAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherRemove.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorRemoveWatcherAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherRemoveAll.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorRemoveAllWatchers()
        End With
    End Sub


    Private Sub FormMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If (g_mFormDebugger IsNot Nothing AndAlso Not g_mFormDebugger.IsDisposed) Then
            MessageBox.Show("You can't close BasicPawn while debugging!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            e.Cancel = True
        End If
    End Sub

    Private Sub ToolStripMenuItem_CheckUpdate_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CheckUpdate.Click
        Try
            Process.Start("https://github.com/Timocop/BasicPawn/releases")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileOpenFolder_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileOpenFolder.Click
        Try
            If (String.IsNullOrEmpty(g_ClassTabControl.m_ActiveTab.m_File) OrElse Not IO.File.Exists(g_ClassTabControl.m_ActiveTab.m_File)) Then
                MessageBox.Show("Can't open current folder. Source file can't be found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Process.Start("explorer.exe", "/select,""" & g_ClassTabControl.m_ActiveTab.m_File & """")
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_TabClose_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabClose.Click
        g_ClassTabControl.RemoveTab(g_ClassTabControl.m_ActiveTabIndex, True)
    End Sub

    Private Sub ToolStripMenuItem_TabMoveRight_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabMoveRight.Click
        Dim iActiveIndex As Integer = g_ClassTabControl.m_ActiveTabIndex
        Dim iToIndex As Integer = iActiveIndex + 1

        If (iToIndex > g_ClassTabControl.m_TabsCount - 1) Then
            Return
        End If

        g_ClassTabControl.SwapTabs(iActiveIndex, iToIndex)
    End Sub

    Private Sub ToolStripMenuItem_TabMoveLeft_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabMoveLeft.Click
        Dim iActiveIndex As Integer = g_ClassTabControl.m_ActiveTabIndex
        Dim iToIndex As Integer = iActiveIndex - 1

        If (iToIndex < 0) Then
            Return
        End If

        g_ClassTabControl.SwapTabs(iActiveIndex, iToIndex)
    End Sub
End Class
