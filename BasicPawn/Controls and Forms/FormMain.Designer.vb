<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If (disposing) Then
                CleanUp()
            End If

            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormMain))
        Me.ContextMenuStrip_RightClick = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_Mark = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ListReferences = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_Cut = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Copy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Paste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Delete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_SelectAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_Debugger = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_DebuggerBreakpoints = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_DebuggerBreakpointInsert = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_DebuggerBreakpointRemove = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_DebuggerBreakpointRemoveAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_DebuggerWatchers = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_DebuggerWatcherInsert = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_DebuggerWatcherRemove = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_DebuggerWatcherRemoveAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HightlightCustom = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Comment = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_Outline = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_OutlineToggleAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_OutlineCollapseAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_OutlineExpandAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip_BasicPawn = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem_File = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileNew = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileLoadTabs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileCloseAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_FileSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileSaveAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileSaveAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileSaveAsTemp = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_FileStartPage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileProjectLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileProjectSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileProjectClose = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_FileSavePacked = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_FileOpenFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_FileExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Tools = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsSettingsAndConfigs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ToolsFormatCode = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsConvertTabsSpaces = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsSearchReplace = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsAutocomplete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripComboBox_ToolsAutocompleteSyntax = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripMenuItem_ToolsAutocompleteCurrentMod = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ToolsAutocompleteShowAutocomplete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ToolsShowInformation = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsClearInformationLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Undo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Redo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Build = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Test = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Debug = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Shell = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Help = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpSpecialControls = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompleteUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompleteDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompletePaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpSpecialControlsMoveSelected = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpSpecialControlsCopySelected = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpSpecialControlsDupLine = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpSpecialControlsCommentLines = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_HelpCheckUpdates = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpGithub = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_HelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_NewUpdate = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TabClose = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TabMoveRight = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TabMoveLeft = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TabOpenInstance = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer_ToolboxSourceAndDetails = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer_ToolboxAndEditor = New System.Windows.Forms.SplitContainer()
        Me.TabControl_Toolbox = New BasicPawn.ClassTabControlColor()
        Me.TabPage_ObjectBrowser = New System.Windows.Forms.TabPage()
        Me.TabPage_ProjectBrowser = New System.Windows.Forms.TabPage()
        Me.TabControl_SourceTabs = New BasicPawn.ClassTabControlColor()
        Me.ContextMenuStrip_Tabs = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_Tabs_Close = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Tabs_CloseAllButThis = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Tabs_CloseAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_Tabs_OpenFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Tabs_Popout = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabControl_Details = New BasicPawn.ClassTabControlColor()
        Me.TabPage_Autocomplete = New System.Windows.Forms.TabPage()
        Me.TabPage_Information = New System.Windows.Forms.TabPage()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.StatusStrip_BasicPawn = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel_EditorLine = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_EditorCollum = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_EditorSelectedCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_CurrentConfig = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_Project = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_LastInformation = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar_Autocomplete = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripStatusLabel_AppVersion = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Timer_PingFlash = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip_RightClick.SuspendLayout()
        Me.MenuStrip_BasicPawn.SuspendLayout()
        Me.SplitContainer_ToolboxSourceAndDetails.Panel1.SuspendLayout()
        Me.SplitContainer_ToolboxSourceAndDetails.Panel2.SuspendLayout()
        Me.SplitContainer_ToolboxSourceAndDetails.SuspendLayout()
        Me.SplitContainer_ToolboxAndEditor.Panel1.SuspendLayout()
        Me.SplitContainer_ToolboxAndEditor.Panel2.SuspendLayout()
        Me.SplitContainer_ToolboxAndEditor.SuspendLayout()
        Me.TabControl_Toolbox.SuspendLayout()
        Me.TabControl_SourceTabs.SuspendLayout()
        Me.ContextMenuStrip_Tabs.SuspendLayout()
        Me.TabControl_Details.SuspendLayout()
        Me.StatusStrip_BasicPawn.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip_RightClick
        '
        Me.ContextMenuStrip_RightClick.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_Mark, Me.ToolStripMenuItem_ListReferences, Me.ToolStripSeparator6, Me.ToolStripMenuItem_Cut, Me.ToolStripMenuItem_Copy, Me.ToolStripMenuItem_Paste, Me.ToolStripMenuItem_Delete, Me.ToolStripMenuItem_SelectAll, Me.ToolStripSeparator1, Me.ToolStripMenuItem_Debugger, Me.ToolStripMenuItem_HightlightCustom, Me.ToolStripMenuItem_Comment, Me.ToolStripSeparator11, Me.ToolStripMenuItem_Outline})
        Me.ContextMenuStrip_RightClick.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip_RightClick.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_RightClick.Size = New System.Drawing.Size(187, 264)
        '
        'ToolStripMenuItem_Mark
        '
        Me.ToolStripMenuItem_Mark.Image = CType(resources.GetObject("ToolStripMenuItem_Mark.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Mark.Name = "ToolStripMenuItem_Mark"
        Me.ToolStripMenuItem_Mark.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem_Mark.Text = "Mark"
        '
        'ToolStripMenuItem_ListReferences
        '
        Me.ToolStripMenuItem_ListReferences.Image = CType(resources.GetObject("ToolStripMenuItem_ListReferences.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ListReferences.Name = "ToolStripMenuItem_ListReferences"
        Me.ToolStripMenuItem_ListReferences.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem_ListReferences.Text = "List references"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(183, 6)
        '
        'ToolStripMenuItem_Cut
        '
        Me.ToolStripMenuItem_Cut.Image = CType(resources.GetObject("ToolStripMenuItem_Cut.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Cut.Name = "ToolStripMenuItem_Cut"
        Me.ToolStripMenuItem_Cut.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem_Cut.Text = "Cut"
        '
        'ToolStripMenuItem_Copy
        '
        Me.ToolStripMenuItem_Copy.Image = CType(resources.GetObject("ToolStripMenuItem_Copy.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Copy.Name = "ToolStripMenuItem_Copy"
        Me.ToolStripMenuItem_Copy.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem_Copy.Text = "Copy"
        '
        'ToolStripMenuItem_Paste
        '
        Me.ToolStripMenuItem_Paste.Image = CType(resources.GetObject("ToolStripMenuItem_Paste.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Paste.Name = "ToolStripMenuItem_Paste"
        Me.ToolStripMenuItem_Paste.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem_Paste.Text = "Paste"
        '
        'ToolStripMenuItem_Delete
        '
        Me.ToolStripMenuItem_Delete.Image = Global.BasicPawn.My.Resources.Resources.imageres_5337_16x16
        Me.ToolStripMenuItem_Delete.Name = "ToolStripMenuItem_Delete"
        Me.ToolStripMenuItem_Delete.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem_Delete.Text = "Delete"
        '
        'ToolStripMenuItem_SelectAll
        '
        Me.ToolStripMenuItem_SelectAll.Image = Global.BasicPawn.My.Resources.Resources.imageres_5312_16x16
        Me.ToolStripMenuItem_SelectAll.Name = "ToolStripMenuItem_SelectAll"
        Me.ToolStripMenuItem_SelectAll.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem_SelectAll.Text = "Select all"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(183, 6)
        '
        'ToolStripMenuItem_Debugger
        '
        Me.ToolStripMenuItem_Debugger.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_DebuggerBreakpoints, Me.ToolStripMenuItem_DebuggerWatchers})
        Me.ToolStripMenuItem_Debugger.Image = CType(resources.GetObject("ToolStripMenuItem_Debugger.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Debugger.Name = "ToolStripMenuItem_Debugger"
        Me.ToolStripMenuItem_Debugger.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem_Debugger.Text = "Debugging"
        '
        'ToolStripMenuItem_DebuggerBreakpoints
        '
        Me.ToolStripMenuItem_DebuggerBreakpoints.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_DebuggerBreakpointInsert, Me.ToolStripMenuItem_DebuggerBreakpointRemove, Me.ToolStripMenuItem_DebuggerBreakpointRemoveAll})
        Me.ToolStripMenuItem_DebuggerBreakpoints.Name = "ToolStripMenuItem_DebuggerBreakpoints"
        Me.ToolStripMenuItem_DebuggerBreakpoints.Size = New System.Drawing.Size(136, 22)
        Me.ToolStripMenuItem_DebuggerBreakpoints.Text = "Breakpoints"
        '
        'ToolStripMenuItem_DebuggerBreakpointInsert
        '
        Me.ToolStripMenuItem_DebuggerBreakpointInsert.Name = "ToolStripMenuItem_DebuggerBreakpointInsert"
        Me.ToolStripMenuItem_DebuggerBreakpointInsert.Size = New System.Drawing.Size(197, 22)
        Me.ToolStripMenuItem_DebuggerBreakpointInsert.Text = "Insert breakpoint"
        '
        'ToolStripMenuItem_DebuggerBreakpointRemove
        '
        Me.ToolStripMenuItem_DebuggerBreakpointRemove.Name = "ToolStripMenuItem_DebuggerBreakpointRemove"
        Me.ToolStripMenuItem_DebuggerBreakpointRemove.Size = New System.Drawing.Size(197, 22)
        Me.ToolStripMenuItem_DebuggerBreakpointRemove.Text = "Remove breakpoint"
        '
        'ToolStripMenuItem_DebuggerBreakpointRemoveAll
        '
        Me.ToolStripMenuItem_DebuggerBreakpointRemoveAll.Name = "ToolStripMenuItem_DebuggerBreakpointRemoveAll"
        Me.ToolStripMenuItem_DebuggerBreakpointRemoveAll.Size = New System.Drawing.Size(197, 22)
        Me.ToolStripMenuItem_DebuggerBreakpointRemoveAll.Text = "Remove all breakpoints"
        '
        'ToolStripMenuItem_DebuggerWatchers
        '
        Me.ToolStripMenuItem_DebuggerWatchers.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_DebuggerWatcherInsert, Me.ToolStripMenuItem_DebuggerWatcherRemove, Me.ToolStripMenuItem_DebuggerWatcherRemoveAll})
        Me.ToolStripMenuItem_DebuggerWatchers.Name = "ToolStripMenuItem_DebuggerWatchers"
        Me.ToolStripMenuItem_DebuggerWatchers.Size = New System.Drawing.Size(136, 22)
        Me.ToolStripMenuItem_DebuggerWatchers.Text = "Watchers"
        '
        'ToolStripMenuItem_DebuggerWatcherInsert
        '
        Me.ToolStripMenuItem_DebuggerWatcherInsert.Name = "ToolStripMenuItem_DebuggerWatcherInsert"
        Me.ToolStripMenuItem_DebuggerWatcherInsert.Size = New System.Drawing.Size(182, 22)
        Me.ToolStripMenuItem_DebuggerWatcherInsert.Text = "Insert watcher"
        '
        'ToolStripMenuItem_DebuggerWatcherRemove
        '
        Me.ToolStripMenuItem_DebuggerWatcherRemove.Name = "ToolStripMenuItem_DebuggerWatcherRemove"
        Me.ToolStripMenuItem_DebuggerWatcherRemove.Size = New System.Drawing.Size(182, 22)
        Me.ToolStripMenuItem_DebuggerWatcherRemove.Text = "Remove watcher"
        '
        'ToolStripMenuItem_DebuggerWatcherRemoveAll
        '
        Me.ToolStripMenuItem_DebuggerWatcherRemoveAll.Name = "ToolStripMenuItem_DebuggerWatcherRemoveAll"
        Me.ToolStripMenuItem_DebuggerWatcherRemoveAll.Size = New System.Drawing.Size(182, 22)
        Me.ToolStripMenuItem_DebuggerWatcherRemoveAll.Text = "Remove all watchers"
        '
        'ToolStripMenuItem_HightlightCustom
        '
        Me.ToolStripMenuItem_HightlightCustom.Image = Global.BasicPawn.My.Resources.Resources.imageres_5313_16x16
        Me.ToolStripMenuItem_HightlightCustom.Name = "ToolStripMenuItem_HightlightCustom"
        Me.ToolStripMenuItem_HightlightCustom.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem_HightlightCustom.Text = "Highlight color"
        '
        'ToolStripMenuItem_Comment
        '
        Me.ToolStripMenuItem_Comment.Image = Global.BasicPawn.My.Resources.Resources.imageres_5348_16x16
        Me.ToolStripMenuItem_Comment.Name = "ToolStripMenuItem_Comment"
        Me.ToolStripMenuItem_Comment.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem_Comment.Text = "Comment line in/out"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(183, 6)
        '
        'ToolStripMenuItem_Outline
        '
        Me.ToolStripMenuItem_Outline.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_OutlineToggleAll, Me.ToolStripSeparator13, Me.ToolStripMenuItem_OutlineCollapseAll, Me.ToolStripMenuItem_OutlineExpandAll})
        Me.ToolStripMenuItem_Outline.Image = Global.BasicPawn.My.Resources.Resources.imageres_5302_16x16
        Me.ToolStripMenuItem_Outline.Name = "ToolStripMenuItem_Outline"
        Me.ToolStripMenuItem_Outline.Size = New System.Drawing.Size(186, 22)
        Me.ToolStripMenuItem_Outline.Text = "Outlining"
        '
        'ToolStripMenuItem_OutlineToggleAll
        '
        Me.ToolStripMenuItem_OutlineToggleAll.Name = "ToolStripMenuItem_OutlineToggleAll"
        Me.ToolStripMenuItem_OutlineToggleAll.Size = New System.Drawing.Size(134, 22)
        Me.ToolStripMenuItem_OutlineToggleAll.Text = "Toggle all"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(131, 6)
        '
        'ToolStripMenuItem_OutlineCollapseAll
        '
        Me.ToolStripMenuItem_OutlineCollapseAll.Name = "ToolStripMenuItem_OutlineCollapseAll"
        Me.ToolStripMenuItem_OutlineCollapseAll.Size = New System.Drawing.Size(134, 22)
        Me.ToolStripMenuItem_OutlineCollapseAll.Text = "Collapse all"
        '
        'ToolStripMenuItem_OutlineExpandAll
        '
        Me.ToolStripMenuItem_OutlineExpandAll.Name = "ToolStripMenuItem_OutlineExpandAll"
        Me.ToolStripMenuItem_OutlineExpandAll.Size = New System.Drawing.Size(134, 22)
        Me.ToolStripMenuItem_OutlineExpandAll.Text = "Expand all"
        '
        'MenuStrip_BasicPawn
        '
        Me.MenuStrip_BasicPawn.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_File, Me.ToolStripMenuItem_Tools, Me.ToolStripMenuItem_Undo, Me.ToolStripMenuItem_Redo, Me.ToolStripMenuItem_Build, Me.ToolStripMenuItem_Test, Me.ToolStripMenuItem_Debug, Me.ToolStripMenuItem_Shell, Me.ToolStripMenuItem_Help, Me.ToolStripMenuItem_NewUpdate, Me.ToolStripMenuItem_TabClose, Me.ToolStripMenuItem_TabMoveRight, Me.ToolStripMenuItem_TabMoveLeft, Me.ToolStripMenuItem_TabOpenInstance})
        Me.MenuStrip_BasicPawn.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip_BasicPawn.Name = "MenuStrip_BasicPawn"
        Me.MenuStrip_BasicPawn.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.MenuStrip_BasicPawn.ShowItemToolTips = True
        Me.MenuStrip_BasicPawn.Size = New System.Drawing.Size(1008, 24)
        Me.MenuStrip_BasicPawn.TabIndex = 2
        Me.MenuStrip_BasicPawn.Text = "MenuStrip1"
        '
        'ToolStripMenuItem_File
        '
        Me.ToolStripMenuItem_File.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_FileNew, Me.ToolStripMenuItem_FileOpen, Me.ToolStripMenuItem_FileLoadTabs, Me.ToolStripMenuItem_FileCloseAll, Me.ToolStripSeparator9, Me.ToolStripMenuItem_FileSave, Me.ToolStripMenuItem_FileSaveAll, Me.ToolStripMenuItem_FileSaveAs, Me.ToolStripMenuItem_FileSaveAsTemp, Me.ToolStripSeparator7, Me.ToolStripMenuItem_FileStartPage, Me.ToolStripMenuItem_FileProjectLoad, Me.ToolStripMenuItem_FileProjectSave, Me.ToolStripMenuItem_FileProjectClose, Me.ToolStripSeparator10, Me.ToolStripMenuItem_FileSavePacked, Me.ToolStripSeparator8, Me.ToolStripMenuItem_FileOpenFolder, Me.ToolStripSeparator2, Me.ToolStripMenuItem_FileExit})
        Me.ToolStripMenuItem_File.Image = CType(resources.GetObject("ToolStripMenuItem_File.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_File.Name = "ToolStripMenuItem_File"
        Me.ToolStripMenuItem_File.Size = New System.Drawing.Size(53, 20)
        Me.ToolStripMenuItem_File.Text = "&File"
        '
        'ToolStripMenuItem_FileNew
        '
        Me.ToolStripMenuItem_FileNew.Image = CType(resources.GetObject("ToolStripMenuItem_FileNew.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_FileNew.Name = "ToolStripMenuItem_FileNew"
        Me.ToolStripMenuItem_FileNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileNew.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileNew.Text = "&New"
        Me.ToolStripMenuItem_FileNew.ToolTipText = "Creates a new tab"
        '
        'ToolStripMenuItem_FileOpen
        '
        Me.ToolStripMenuItem_FileOpen.Image = CType(resources.GetObject("ToolStripMenuItem_FileOpen.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_FileOpen.Name = "ToolStripMenuItem_FileOpen"
        Me.ToolStripMenuItem_FileOpen.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileOpen.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileOpen.Text = "&Open"
        Me.ToolStripMenuItem_FileOpen.ToolTipText = "Open a existing file"
        '
        'ToolStripMenuItem_FileLoadTabs
        '
        Me.ToolStripMenuItem_FileLoadTabs.Image = Global.BasicPawn.My.Resources.Resources.imageres_5333_16x16
        Me.ToolStripMenuItem_FileLoadTabs.Name = "ToolStripMenuItem_FileLoadTabs"
        Me.ToolStripMenuItem_FileLoadTabs.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileLoadTabs.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileLoadTabs.Text = "Open from other instances"
        Me.ToolStripMenuItem_FileLoadTabs.ToolTipText = "Open files from other running BasicPawn instances"
        '
        'ToolStripMenuItem_FileCloseAll
        '
        Me.ToolStripMenuItem_FileCloseAll.Image = Global.BasicPawn.My.Resources.Resources.imageres_5318_16x16
        Me.ToolStripMenuItem_FileCloseAll.Name = "ToolStripMenuItem_FileCloseAll"
        Me.ToolStripMenuItem_FileCloseAll.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileCloseAll.Text = "Close all"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(287, 6)
        '
        'ToolStripMenuItem_FileSave
        '
        Me.ToolStripMenuItem_FileSave.Image = CType(resources.GetObject("ToolStripMenuItem_FileSave.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_FileSave.Name = "ToolStripMenuItem_FileSave"
        Me.ToolStripMenuItem_FileSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileSave.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileSave.Text = "&Save"
        Me.ToolStripMenuItem_FileSave.ToolTipText = "Saves the current into a file"
        '
        'ToolStripMenuItem_FileSaveAll
        '
        Me.ToolStripMenuItem_FileSaveAll.Image = Global.BasicPawn.My.Resources.Resources.imageres_5303_16x16
        Me.ToolStripMenuItem_FileSaveAll.Name = "ToolStripMenuItem_FileSaveAll"
        Me.ToolStripMenuItem_FileSaveAll.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileSaveAll.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileSaveAll.Text = "Save all"
        Me.ToolStripMenuItem_FileSaveAll.ToolTipText = "Saves all open tabs into a file"
        '
        'ToolStripMenuItem_FileSaveAs
        '
        Me.ToolStripMenuItem_FileSaveAs.Image = CType(resources.GetObject("ToolStripMenuItem_FileSaveAs.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_FileSaveAs.Name = "ToolStripMenuItem_FileSaveAs"
        Me.ToolStripMenuItem_FileSaveAs.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileSaveAs.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileSaveAs.Text = "Save &as..."
        Me.ToolStripMenuItem_FileSaveAs.ToolTipText = "Saves the current into a file"
        '
        'ToolStripMenuItem_FileSaveAsTemp
        '
        Me.ToolStripMenuItem_FileSaveAsTemp.Image = Global.BasicPawn.My.Resources.Resources.imageres_5303_16x16
        Me.ToolStripMenuItem_FileSaveAsTemp.Name = "ToolStripMenuItem_FileSaveAsTemp"
        Me.ToolStripMenuItem_FileSaveAsTemp.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileSaveAsTemp.Text = "Save as &temporary"
        Me.ToolStripMenuItem_FileSaveAsTemp.ToolTipText = "Saves the current tab into the temporary folder"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(287, 6)
        '
        'ToolStripMenuItem_FileStartPage
        '
        Me.ToolStripMenuItem_FileStartPage.Image = Global.BasicPawn.My.Resources.Resources.imageres_5364_16x16
        Me.ToolStripMenuItem_FileStartPage.Name = "ToolStripMenuItem_FileStartPage"
        Me.ToolStripMenuItem_FileStartPage.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileStartPage.Text = "StartPage"
        '
        'ToolStripMenuItem_FileProjectLoad
        '
        Me.ToolStripMenuItem_FileProjectLoad.Image = Global.BasicPawn.My.Resources.Resources.imageres_5339_16x16
        Me.ToolStripMenuItem_FileProjectLoad.Name = "ToolStripMenuItem_FileProjectLoad"
        Me.ToolStripMenuItem_FileProjectLoad.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileProjectLoad.Text = "Load Project"
        '
        'ToolStripMenuItem_FileProjectSave
        '
        Me.ToolStripMenuItem_FileProjectSave.Image = Global.BasicPawn.My.Resources.Resources.imageres_5304_16x16
        Me.ToolStripMenuItem_FileProjectSave.Name = "ToolStripMenuItem_FileProjectSave"
        Me.ToolStripMenuItem_FileProjectSave.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileProjectSave.Text = "Save Project as..."
        '
        'ToolStripMenuItem_FileProjectClose
        '
        Me.ToolStripMenuItem_FileProjectClose.Image = Global.BasicPawn.My.Resources.Resources.imageres_5320_16x16
        Me.ToolStripMenuItem_FileProjectClose.Name = "ToolStripMenuItem_FileProjectClose"
        Me.ToolStripMenuItem_FileProjectClose.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileProjectClose.Text = "Close Project"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(287, 6)
        '
        'ToolStripMenuItem_FileSavePacked
        '
        Me.ToolStripMenuItem_FileSavePacked.Image = Global.BasicPawn.My.Resources.Resources.imageres_5303_16x16
        Me.ToolStripMenuItem_FileSavePacked.Name = "ToolStripMenuItem_FileSavePacked"
        Me.ToolStripMenuItem_FileSavePacked.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileSavePacked.Text = "&Export Packed"
        Me.ToolStripMenuItem_FileSavePacked.ToolTipText = "Packs all includes into one file and resolves all defines"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(287, 6)
        '
        'ToolStripMenuItem_FileOpenFolder
        '
        Me.ToolStripMenuItem_FileOpenFolder.Image = Global.BasicPawn.My.Resources.Resources.imageres_5304_16x16
        Me.ToolStripMenuItem_FileOpenFolder.Name = "ToolStripMenuItem_FileOpenFolder"
        Me.ToolStripMenuItem_FileOpenFolder.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileOpenFolder.Text = "Open current folder"
        Me.ToolStripMenuItem_FileOpenFolder.ToolTipText = "Opens the current tabs folder"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(287, 6)
        '
        'ToolStripMenuItem_FileExit
        '
        Me.ToolStripMenuItem_FileExit.Image = CType(resources.GetObject("ToolStripMenuItem_FileExit.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_FileExit.Name = "ToolStripMenuItem_FileExit"
        Me.ToolStripMenuItem_FileExit.Size = New System.Drawing.Size(290, 22)
        Me.ToolStripMenuItem_FileExit.Text = "Exit"
        Me.ToolStripMenuItem_FileExit.ToolTipText = "Quits BasicPawn"
        '
        'ToolStripMenuItem_Tools
        '
        Me.ToolStripMenuItem_Tools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ToolsSettingsAndConfigs, Me.ToolStripSeparator3, Me.ToolStripMenuItem_ToolsFormatCode, Me.ToolStripMenuItem_ToolsConvertTabsSpaces, Me.ToolStripMenuItem_ToolsSearchReplace, Me.ToolStripMenuItem_ToolsAutocomplete, Me.ToolStripSeparator4, Me.ToolStripMenuItem_ToolsShowInformation, Me.ToolStripMenuItem_ToolsClearInformationLog})
        Me.ToolStripMenuItem_Tools.Image = CType(resources.GetObject("ToolStripMenuItem_Tools.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Tools.Name = "ToolStripMenuItem_Tools"
        Me.ToolStripMenuItem_Tools.Size = New System.Drawing.Size(63, 20)
        Me.ToolStripMenuItem_Tools.Text = "T&ools"
        '
        'ToolStripMenuItem_ToolsSettingsAndConfigs
        '
        Me.ToolStripMenuItem_ToolsSettingsAndConfigs.Image = CType(resources.GetObject("ToolStripMenuItem_ToolsSettingsAndConfigs.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ToolsSettingsAndConfigs.Name = "ToolStripMenuItem_ToolsSettingsAndConfigs"
        Me.ToolStripMenuItem_ToolsSettingsAndConfigs.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem_ToolsSettingsAndConfigs.Text = "Settings && Configs"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(222, 6)
        '
        'ToolStripMenuItem_ToolsFormatCode
        '
        Me.ToolStripMenuItem_ToolsFormatCode.Image = CType(resources.GetObject("ToolStripMenuItem_ToolsFormatCode.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ToolsFormatCode.Name = "ToolStripMenuItem_ToolsFormatCode"
        Me.ToolStripMenuItem_ToolsFormatCode.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem_ToolsFormatCode.Text = "Format Code"
        '
        'ToolStripMenuItem_ToolsConvertTabsSpaces
        '
        Me.ToolStripMenuItem_ToolsConvertTabsSpaces.Image = Global.BasicPawn.My.Resources.Resources.imageres_5357_16x16
        Me.ToolStripMenuItem_ToolsConvertTabsSpaces.Name = "ToolStripMenuItem_ToolsConvertTabsSpaces"
        Me.ToolStripMenuItem_ToolsConvertTabsSpaces.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem_ToolsConvertTabsSpaces.Text = "Convert Tabs && Spaces"
        '
        'ToolStripMenuItem_ToolsSearchReplace
        '
        Me.ToolStripMenuItem_ToolsSearchReplace.Image = CType(resources.GetObject("ToolStripMenuItem_ToolsSearchReplace.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ToolsSearchReplace.Name = "ToolStripMenuItem_ToolsSearchReplace"
        Me.ToolStripMenuItem_ToolsSearchReplace.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_ToolsSearchReplace.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem_ToolsSearchReplace.Text = "Search && Replace"
        '
        'ToolStripMenuItem_ToolsAutocomplete
        '
        Me.ToolStripMenuItem_ToolsAutocomplete.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ToolsAutocompleteUpdate, Me.ToolStripComboBox_ToolsAutocompleteSyntax, Me.ToolStripMenuItem_ToolsAutocompleteCurrentMod, Me.ToolStripSeparator5, Me.ToolStripMenuItem_ToolsAutocompleteShowAutocomplete})
        Me.ToolStripMenuItem_ToolsAutocomplete.Image = CType(resources.GetObject("ToolStripMenuItem_ToolsAutocomplete.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ToolsAutocomplete.Name = "ToolStripMenuItem_ToolsAutocomplete"
        Me.ToolStripMenuItem_ToolsAutocomplete.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem_ToolsAutocomplete.Text = "Autocomplete && IntelliSense"
        '
        'ToolStripMenuItem_ToolsAutocompleteUpdate
        '
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate.Image = CType(resources.GetObject("ToolStripMenuItem_ToolsAutocompleteUpdate.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate.Name = "ToolStripMenuItem_ToolsAutocompleteUpdate"
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate.Size = New System.Drawing.Size(360, 22)
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate.Text = "Update"
        '
        'ToolStripComboBox_ToolsAutocompleteSyntax
        '
        Me.ToolStripComboBox_ToolsAutocompleteSyntax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBox_ToolsAutocompleteSyntax.DropDownWidth = 200
        Me.ToolStripComboBox_ToolsAutocompleteSyntax.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ToolStripComboBox_ToolsAutocompleteSyntax.Items.AddRange(New Object() {"Parse mixed syntax", "Parse SourcePawn <1.6, AMX Mod X and Pawn syntax", "Parse SourcePawn >1.7 syntax only"})
        Me.ToolStripComboBox_ToolsAutocompleteSyntax.Name = "ToolStripComboBox_ToolsAutocompleteSyntax"
        Me.ToolStripComboBox_ToolsAutocompleteSyntax.Size = New System.Drawing.Size(300, 23)
        '
        'ToolStripMenuItem_ToolsAutocompleteCurrentMod
        '
        Me.ToolStripMenuItem_ToolsAutocompleteCurrentMod.Enabled = False
        Me.ToolStripMenuItem_ToolsAutocompleteCurrentMod.Name = "ToolStripMenuItem_ToolsAutocompleteCurrentMod"
        Me.ToolStripMenuItem_ToolsAutocompleteCurrentMod.Size = New System.Drawing.Size(360, 22)
        Me.ToolStripMenuItem_ToolsAutocompleteCurrentMod.Text = "Current Mod: Unknown"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(357, 6)
        '
        'ToolStripMenuItem_ToolsAutocompleteShowAutocomplete
        '
        Me.ToolStripMenuItem_ToolsAutocompleteShowAutocomplete.Image = CType(resources.GetObject("ToolStripMenuItem_ToolsAutocompleteShowAutocomplete.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ToolsAutocompleteShowAutocomplete.Name = "ToolStripMenuItem_ToolsAutocompleteShowAutocomplete"
        Me.ToolStripMenuItem_ToolsAutocompleteShowAutocomplete.Size = New System.Drawing.Size(360, 22)
        Me.ToolStripMenuItem_ToolsAutocompleteShowAutocomplete.Text = "Show Autocomplete && IntelliSense"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(222, 6)
        '
        'ToolStripMenuItem_ToolsShowInformation
        '
        Me.ToolStripMenuItem_ToolsShowInformation.Image = CType(resources.GetObject("ToolStripMenuItem_ToolsShowInformation.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ToolsShowInformation.Name = "ToolStripMenuItem_ToolsShowInformation"
        Me.ToolStripMenuItem_ToolsShowInformation.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem_ToolsShowInformation.Text = "Show Information"
        '
        'ToolStripMenuItem_ToolsClearInformationLog
        '
        Me.ToolStripMenuItem_ToolsClearInformationLog.Image = CType(resources.GetObject("ToolStripMenuItem_ToolsClearInformationLog.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ToolsClearInformationLog.Name = "ToolStripMenuItem_ToolsClearInformationLog"
        Me.ToolStripMenuItem_ToolsClearInformationLog.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem_ToolsClearInformationLog.Text = "Clear Information"
        '
        'ToolStripMenuItem_Undo
        '
        Me.ToolStripMenuItem_Undo.Image = CType(resources.GetObject("ToolStripMenuItem_Undo.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Undo.Name = "ToolStripMenuItem_Undo"
        Me.ToolStripMenuItem_Undo.Size = New System.Drawing.Size(64, 20)
        Me.ToolStripMenuItem_Undo.Text = "&Undo"
        '
        'ToolStripMenuItem_Redo
        '
        Me.ToolStripMenuItem_Redo.Image = CType(resources.GetObject("ToolStripMenuItem_Redo.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Redo.Name = "ToolStripMenuItem_Redo"
        Me.ToolStripMenuItem_Redo.Size = New System.Drawing.Size(62, 20)
        Me.ToolStripMenuItem_Redo.Text = "&Redo"
        '
        'ToolStripMenuItem_Build
        '
        Me.ToolStripMenuItem_Build.Image = CType(resources.GetObject("ToolStripMenuItem_Build.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Build.Name = "ToolStripMenuItem_Build"
        Me.ToolStripMenuItem_Build.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.ToolStripMenuItem_Build.Size = New System.Drawing.Size(62, 20)
        Me.ToolStripMenuItem_Build.Text = "&Build"
        '
        'ToolStripMenuItem_Test
        '
        Me.ToolStripMenuItem_Test.Image = CType(resources.GetObject("ToolStripMenuItem_Test.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Test.Name = "ToolStripMenuItem_Test"
        Me.ToolStripMenuItem_Test.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.ToolStripMenuItem_Test.Size = New System.Drawing.Size(56, 20)
        Me.ToolStripMenuItem_Test.Text = "&Test"
        '
        'ToolStripMenuItem_Debug
        '
        Me.ToolStripMenuItem_Debug.Image = CType(resources.GetObject("ToolStripMenuItem_Debug.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Debug.Name = "ToolStripMenuItem_Debug"
        Me.ToolStripMenuItem_Debug.ShortcutKeys = System.Windows.Forms.Keys.F6
        Me.ToolStripMenuItem_Debug.Size = New System.Drawing.Size(114, 20)
        Me.ToolStripMenuItem_Debug.Text = "Start &Debugger"
        '
        'ToolStripMenuItem_Shell
        '
        Me.ToolStripMenuItem_Shell.Image = CType(resources.GetObject("ToolStripMenuItem_Shell.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Shell.Name = "ToolStripMenuItem_Shell"
        Me.ToolStripMenuItem_Shell.ShortcutKeys = System.Windows.Forms.Keys.F7
        Me.ToolStripMenuItem_Shell.Size = New System.Drawing.Size(60, 20)
        Me.ToolStripMenuItem_Shell.Text = "&Shell"
        '
        'ToolStripMenuItem_Help
        '
        Me.ToolStripMenuItem_Help.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_HelpSpecialControls, Me.ToolStripSeparator15, Me.ToolStripMenuItem_HelpCheckUpdates, Me.ToolStripMenuItem_HelpGithub, Me.ToolStripSeparator14, Me.ToolStripMenuItem_HelpAbout})
        Me.ToolStripMenuItem_Help.Image = CType(resources.GetObject("ToolStripMenuItem_Help.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Help.Name = "ToolStripMenuItem_Help"
        Me.ToolStripMenuItem_Help.Size = New System.Drawing.Size(60, 20)
        Me.ToolStripMenuItem_Help.Text = "&Help"
        '
        'ToolStripMenuItem_HelpSpecialControls
        '
        Me.ToolStripMenuItem_HelpSpecialControls.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_HelpSpecialControlsAutocompleteUp, Me.ToolStripMenuItem_HelpSpecialControlsAutocompleteDown, Me.ToolStripMenuItem_HelpSpecialControlsAutocompletePaste, Me.ToolStripMenuItem_HelpSpecialControlsMoveSelected, Me.ToolStripMenuItem_HelpSpecialControlsCopySelected, Me.ToolStripMenuItem_HelpSpecialControlsDupLine, Me.ToolStripMenuItem_HelpSpecialControlsCommentLines})
        Me.ToolStripMenuItem_HelpSpecialControls.Image = CType(resources.GetObject("ToolStripMenuItem_HelpSpecialControls.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_HelpSpecialControls.Name = "ToolStripMenuItem_HelpSpecialControls"
        Me.ToolStripMenuItem_HelpSpecialControls.Size = New System.Drawing.Size(170, 22)
        Me.ToolStripMenuItem_HelpSpecialControls.Text = "Special Controls"
        '
        'ToolStripMenuItem_HelpSpecialControlsAutocompleteUp
        '
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompleteUp.Name = "ToolStripMenuItem_HelpSpecialControlsAutocompleteUp"
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompleteUp.ShortcutKeyDisplayString = "CTRL+UP"
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompleteUp.Size = New System.Drawing.Size(274, 22)
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompleteUp.Text = "Autocomplete Up"
        '
        'ToolStripMenuItem_HelpSpecialControlsAutocompleteDown
        '
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompleteDown.Name = "ToolStripMenuItem_HelpSpecialControlsAutocompleteDown"
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompleteDown.ShortcutKeyDisplayString = "CTRL+DOWN"
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompleteDown.Size = New System.Drawing.Size(274, 22)
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompleteDown.Text = "Autocomplete Down"
        '
        'ToolStripMenuItem_HelpSpecialControlsAutocompletePaste
        '
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompletePaste.Name = "ToolStripMenuItem_HelpSpecialControlsAutocompletePaste"
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompletePaste.ShortcutKeyDisplayString = "CTRL+ENTER"
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompletePaste.Size = New System.Drawing.Size(274, 22)
        Me.ToolStripMenuItem_HelpSpecialControlsAutocompletePaste.Text = "Autocomplete Paste"
        '
        'ToolStripMenuItem_HelpSpecialControlsMoveSelected
        '
        Me.ToolStripMenuItem_HelpSpecialControlsMoveSelected.Name = "ToolStripMenuItem_HelpSpecialControlsMoveSelected"
        Me.ToolStripMenuItem_HelpSpecialControlsMoveSelected.ShortcutKeyDisplayString = "DRAG"
        Me.ToolStripMenuItem_HelpSpecialControlsMoveSelected.Size = New System.Drawing.Size(274, 22)
        Me.ToolStripMenuItem_HelpSpecialControlsMoveSelected.Text = "Move Selected"
        '
        'ToolStripMenuItem_HelpSpecialControlsCopySelected
        '
        Me.ToolStripMenuItem_HelpSpecialControlsCopySelected.Name = "ToolStripMenuItem_HelpSpecialControlsCopySelected"
        Me.ToolStripMenuItem_HelpSpecialControlsCopySelected.ShortcutKeyDisplayString = "CTRL+DRAG"
        Me.ToolStripMenuItem_HelpSpecialControlsCopySelected.Size = New System.Drawing.Size(274, 22)
        Me.ToolStripMenuItem_HelpSpecialControlsCopySelected.Text = "Copy Selected"
        '
        'ToolStripMenuItem_HelpSpecialControlsDupLine
        '
        Me.ToolStripMenuItem_HelpSpecialControlsDupLine.Name = "ToolStripMenuItem_HelpSpecialControlsDupLine"
        Me.ToolStripMenuItem_HelpSpecialControlsDupLine.ShortcutKeyDisplayString = "CTRL+D"
        Me.ToolStripMenuItem_HelpSpecialControlsDupLine.Size = New System.Drawing.Size(274, 22)
        Me.ToolStripMenuItem_HelpSpecialControlsDupLine.Text = "Duplicate Line/Selected"
        '
        'ToolStripMenuItem_HelpSpecialControlsCommentLines
        '
        Me.ToolStripMenuItem_HelpSpecialControlsCommentLines.Name = "ToolStripMenuItem_HelpSpecialControlsCommentLines"
        Me.ToolStripMenuItem_HelpSpecialControlsCommentLines.ShortcutKeyDisplayString = "CTRL+Num /"
        Me.ToolStripMenuItem_HelpSpecialControlsCommentLines.Size = New System.Drawing.Size(274, 22)
        Me.ToolStripMenuItem_HelpSpecialControlsCommentLines.Text = "Comment Lines In/Out"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(167, 6)
        '
        'ToolStripMenuItem_HelpCheckUpdates
        '
        Me.ToolStripMenuItem_HelpCheckUpdates.Image = Global.BasicPawn.My.Resources.Resources.imageres_5332_16x16
        Me.ToolStripMenuItem_HelpCheckUpdates.Name = "ToolStripMenuItem_HelpCheckUpdates"
        Me.ToolStripMenuItem_HelpCheckUpdates.Size = New System.Drawing.Size(170, 22)
        Me.ToolStripMenuItem_HelpCheckUpdates.Text = "Check for updates"
        '
        'ToolStripMenuItem_HelpGithub
        '
        Me.ToolStripMenuItem_HelpGithub.Image = Global.BasicPawn.My.Resources.Resources.imageres_5316_16x16
        Me.ToolStripMenuItem_HelpGithub.Name = "ToolStripMenuItem_HelpGithub"
        Me.ToolStripMenuItem_HelpGithub.Size = New System.Drawing.Size(170, 22)
        Me.ToolStripMenuItem_HelpGithub.Text = "View on Github"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(167, 6)
        '
        'ToolStripMenuItem_HelpAbout
        '
        Me.ToolStripMenuItem_HelpAbout.Image = CType(resources.GetObject("ToolStripMenuItem_HelpAbout.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_HelpAbout.Name = "ToolStripMenuItem_HelpAbout"
        Me.ToolStripMenuItem_HelpAbout.Size = New System.Drawing.Size(170, 22)
        Me.ToolStripMenuItem_HelpAbout.Text = "About"
        '
        'ToolStripMenuItem_NewUpdate
        '
        Me.ToolStripMenuItem_NewUpdate.Image = CType(resources.GetObject("ToolStripMenuItem_NewUpdate.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_NewUpdate.Name = "ToolStripMenuItem_NewUpdate"
        Me.ToolStripMenuItem_NewUpdate.Size = New System.Drawing.Size(151, 20)
        Me.ToolStripMenuItem_NewUpdate.Text = "New update available!"
        Me.ToolStripMenuItem_NewUpdate.Visible = False
        '
        'ToolStripMenuItem_TabClose
        '
        Me.ToolStripMenuItem_TabClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripMenuItem_TabClose.AutoToolTip = True
        Me.ToolStripMenuItem_TabClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripMenuItem_TabClose.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripMenuItem_TabClose.Name = "ToolStripMenuItem_TabClose"
        Me.ToolStripMenuItem_TabClose.Padding = New System.Windows.Forms.Padding(0)
        Me.ToolStripMenuItem_TabClose.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_TabClose.ShowShortcutKeys = False
        Me.ToolStripMenuItem_TabClose.Size = New System.Drawing.Size(19, 20)
        Me.ToolStripMenuItem_TabClose.Text = "X"
        Me.ToolStripMenuItem_TabClose.ToolTipText = "Close current tab"
        '
        'ToolStripMenuItem_TabMoveRight
        '
        Me.ToolStripMenuItem_TabMoveRight.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripMenuItem_TabMoveRight.AutoToolTip = True
        Me.ToolStripMenuItem_TabMoveRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripMenuItem_TabMoveRight.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripMenuItem_TabMoveRight.Name = "ToolStripMenuItem_TabMoveRight"
        Me.ToolStripMenuItem_TabMoveRight.Padding = New System.Windows.Forms.Padding(0)
        Me.ToolStripMenuItem_TabMoveRight.Size = New System.Drawing.Size(19, 20)
        Me.ToolStripMenuItem_TabMoveRight.Text = ">"
        Me.ToolStripMenuItem_TabMoveRight.ToolTipText = "Move current tab right"
        '
        'ToolStripMenuItem_TabMoveLeft
        '
        Me.ToolStripMenuItem_TabMoveLeft.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripMenuItem_TabMoveLeft.AutoToolTip = True
        Me.ToolStripMenuItem_TabMoveLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripMenuItem_TabMoveLeft.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripMenuItem_TabMoveLeft.Name = "ToolStripMenuItem_TabMoveLeft"
        Me.ToolStripMenuItem_TabMoveLeft.Padding = New System.Windows.Forms.Padding(0)
        Me.ToolStripMenuItem_TabMoveLeft.Size = New System.Drawing.Size(19, 20)
        Me.ToolStripMenuItem_TabMoveLeft.Text = "<"
        Me.ToolStripMenuItem_TabMoveLeft.ToolTipText = "Move current tab left"
        '
        'ToolStripMenuItem_TabOpenInstance
        '
        Me.ToolStripMenuItem_TabOpenInstance.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripMenuItem_TabOpenInstance.AutoToolTip = True
        Me.ToolStripMenuItem_TabOpenInstance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripMenuItem_TabOpenInstance.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripMenuItem_TabOpenInstance.Name = "ToolStripMenuItem_TabOpenInstance"
        Me.ToolStripMenuItem_TabOpenInstance.Padding = New System.Windows.Forms.Padding(0)
        Me.ToolStripMenuItem_TabOpenInstance.Size = New System.Drawing.Size(18, 20)
        Me.ToolStripMenuItem_TabOpenInstance.Text = "v"
        Me.ToolStripMenuItem_TabOpenInstance.ToolTipText = "Open from Instances"
        '
        'SplitContainer_ToolboxSourceAndDetails
        '
        Me.SplitContainer_ToolboxSourceAndDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer_ToolboxSourceAndDetails.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer_ToolboxSourceAndDetails.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer_ToolboxSourceAndDetails.Name = "SplitContainer_ToolboxSourceAndDetails"
        Me.SplitContainer_ToolboxSourceAndDetails.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer_ToolboxSourceAndDetails.Panel1
        '
        Me.SplitContainer_ToolboxSourceAndDetails.Panel1.Controls.Add(Me.SplitContainer_ToolboxAndEditor)
        Me.SplitContainer_ToolboxSourceAndDetails.Panel1MinSize = 16
        '
        'SplitContainer_ToolboxSourceAndDetails.Panel2
        '
        Me.SplitContainer_ToolboxSourceAndDetails.Panel2.Controls.Add(Me.TabControl_Details)
        Me.SplitContainer_ToolboxSourceAndDetails.Panel2MinSize = 16
        Me.SplitContainer_ToolboxSourceAndDetails.Size = New System.Drawing.Size(1008, 683)
        Me.SplitContainer_ToolboxSourceAndDetails.SplitterDistance = 500
        Me.SplitContainer_ToolboxSourceAndDetails.TabIndex = 3
        '
        'SplitContainer_ToolboxAndEditor
        '
        Me.SplitContainer_ToolboxAndEditor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer_ToolboxAndEditor.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer_ToolboxAndEditor.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer_ToolboxAndEditor.Name = "SplitContainer_ToolboxAndEditor"
        '
        'SplitContainer_ToolboxAndEditor.Panel1
        '
        Me.SplitContainer_ToolboxAndEditor.Panel1.Controls.Add(Me.TabControl_Toolbox)
        '
        'SplitContainer_ToolboxAndEditor.Panel2
        '
        Me.SplitContainer_ToolboxAndEditor.Panel2.Controls.Add(Me.TabControl_SourceTabs)
        Me.SplitContainer_ToolboxAndEditor.Size = New System.Drawing.Size(1008, 500)
        Me.SplitContainer_ToolboxAndEditor.SplitterDistance = 200
        Me.SplitContainer_ToolboxAndEditor.TabIndex = 1
        '
        'TabControl_Toolbox
        '
        Me.TabControl_Toolbox.Controls.Add(Me.TabPage_ObjectBrowser)
        Me.TabControl_Toolbox.Controls.Add(Me.TabPage_ProjectBrowser)
        Me.TabControl_Toolbox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl_Toolbox.Location = New System.Drawing.Point(0, 0)
        Me.TabControl_Toolbox.Margin = New System.Windows.Forms.Padding(0)
        Me.TabControl_Toolbox.Name = "TabControl_Toolbox"
        Me.TabControl_Toolbox.Padding = New System.Drawing.Point(0, 0)
        Me.TabControl_Toolbox.SelectedIndex = 0
        Me.TabControl_Toolbox.Size = New System.Drawing.Size(200, 500)
        Me.TabControl_Toolbox.TabIndex = 0
        '
        'TabPage_ObjectBrowser
        '
        Me.TabPage_ObjectBrowser.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_ObjectBrowser.Margin = New System.Windows.Forms.Padding(0)
        Me.TabPage_ObjectBrowser.Name = "TabPage_ObjectBrowser"
        Me.TabPage_ObjectBrowser.Size = New System.Drawing.Size(192, 474)
        Me.TabPage_ObjectBrowser.TabIndex = 0
        Me.TabPage_ObjectBrowser.Text = "Object Browser"
        Me.TabPage_ObjectBrowser.UseVisualStyleBackColor = True
        '
        'TabPage_ProjectBrowser
        '
        Me.TabPage_ProjectBrowser.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_ProjectBrowser.Name = "TabPage_ProjectBrowser"
        Me.TabPage_ProjectBrowser.Size = New System.Drawing.Size(192, 474)
        Me.TabPage_ProjectBrowser.TabIndex = 1
        Me.TabPage_ProjectBrowser.Text = "Project Browser"
        Me.TabPage_ProjectBrowser.UseVisualStyleBackColor = True
        '
        'TabControl_SourceTabs
        '
        Me.TabControl_SourceTabs.ContextMenuStrip = Me.ContextMenuStrip_Tabs
        Me.TabControl_SourceTabs.Controls.Add(Me.TabPage1)
        Me.TabControl_SourceTabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl_SourceTabs.Location = New System.Drawing.Point(0, 0)
        Me.TabControl_SourceTabs.Margin = New System.Windows.Forms.Padding(0)
        Me.TabControl_SourceTabs.Name = "TabControl_SourceTabs"
        Me.TabControl_SourceTabs.Padding = New System.Drawing.Point(0, 0)
        Me.TabControl_SourceTabs.SelectedIndex = 0
        Me.TabControl_SourceTabs.ShowToolTips = True
        Me.TabControl_SourceTabs.Size = New System.Drawing.Size(804, 500)
        Me.TabControl_SourceTabs.TabIndex = 1
        '
        'ContextMenuStrip_Tabs
        '
        Me.ContextMenuStrip_Tabs.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_Tabs_Close, Me.ToolStripMenuItem_Tabs_CloseAllButThis, Me.ToolStripMenuItem_Tabs_CloseAll, Me.ToolStripSeparator12, Me.ToolStripMenuItem_Tabs_OpenFolder, Me.ToolStripMenuItem_Tabs_Popout})
        Me.ContextMenuStrip_Tabs.Name = "ContextMenuStrip_Tabs"
        Me.ContextMenuStrip_Tabs.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_Tabs.Size = New System.Drawing.Size(179, 142)
        '
        'ToolStripMenuItem_Tabs_Close
        '
        Me.ToolStripMenuItem_Tabs_Close.Image = Global.BasicPawn.My.Resources.Resources.imageres_5337_16x16
        Me.ToolStripMenuItem_Tabs_Close.Name = "ToolStripMenuItem_Tabs_Close"
        Me.ToolStripMenuItem_Tabs_Close.Size = New System.Drawing.Size(178, 22)
        Me.ToolStripMenuItem_Tabs_Close.Text = "Close"
        '
        'ToolStripMenuItem_Tabs_CloseAllButThis
        '
        Me.ToolStripMenuItem_Tabs_CloseAllButThis.Name = "ToolStripMenuItem_Tabs_CloseAllButThis"
        Me.ToolStripMenuItem_Tabs_CloseAllButThis.Size = New System.Drawing.Size(178, 22)
        Me.ToolStripMenuItem_Tabs_CloseAllButThis.Text = "Close all but this"
        '
        'ToolStripMenuItem_Tabs_CloseAll
        '
        Me.ToolStripMenuItem_Tabs_CloseAll.Name = "ToolStripMenuItem_Tabs_CloseAll"
        Me.ToolStripMenuItem_Tabs_CloseAll.Size = New System.Drawing.Size(178, 22)
        Me.ToolStripMenuItem_Tabs_CloseAll.Text = "Close all"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(175, 6)
        '
        'ToolStripMenuItem_Tabs_OpenFolder
        '
        Me.ToolStripMenuItem_Tabs_OpenFolder.Image = Global.BasicPawn.My.Resources.Resources.imageres_5304_16x16
        Me.ToolStripMenuItem_Tabs_OpenFolder.Name = "ToolStripMenuItem_Tabs_OpenFolder"
        Me.ToolStripMenuItem_Tabs_OpenFolder.Size = New System.Drawing.Size(178, 22)
        Me.ToolStripMenuItem_Tabs_OpenFolder.Text = "Open current folder"
        '
        'ToolStripMenuItem_Tabs_Popout
        '
        Me.ToolStripMenuItem_Tabs_Popout.Image = Global.BasicPawn.My.Resources.Resources.imageres_5333_16x16
        Me.ToolStripMenuItem_Tabs_Popout.Name = "ToolStripMenuItem_Tabs_Popout"
        Me.ToolStripMenuItem_Tabs_Popout.Size = New System.Drawing.Size(178, 22)
        Me.ToolStripMenuItem_Tabs_Popout.Text = "Popout"
        '
        'TabPage1
        '
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(0)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(796, 474)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "New*"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabControl_Details
        '
        Me.TabControl_Details.Controls.Add(Me.TabPage_Autocomplete)
        Me.TabControl_Details.Controls.Add(Me.TabPage_Information)
        Me.TabControl_Details.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl_Details.ImageList = Me.ImageList1
        Me.TabControl_Details.Location = New System.Drawing.Point(0, 0)
        Me.TabControl_Details.Name = "TabControl_Details"
        Me.TabControl_Details.SelectedIndex = 0
        Me.TabControl_Details.Size = New System.Drawing.Size(1008, 179)
        Me.TabControl_Details.TabIndex = 0
        '
        'TabPage_Autocomplete
        '
        Me.TabPage_Autocomplete.ImageIndex = 1
        Me.TabPage_Autocomplete.Location = New System.Drawing.Point(4, 23)
        Me.TabPage_Autocomplete.Name = "TabPage_Autocomplete"
        Me.TabPage_Autocomplete.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Autocomplete.Size = New System.Drawing.Size(1000, 152)
        Me.TabPage_Autocomplete.TabIndex = 0
        Me.TabPage_Autocomplete.Text = "Autocomplete & IntelliSense"
        Me.TabPage_Autocomplete.UseVisualStyleBackColor = True
        '
        'TabPage_Information
        '
        Me.TabPage_Information.ImageIndex = 0
        Me.TabPage_Information.Location = New System.Drawing.Point(4, 23)
        Me.TabPage_Information.Name = "TabPage_Information"
        Me.TabPage_Information.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Information.Size = New System.Drawing.Size(1000, 152)
        Me.TabPage_Information.TabIndex = 1
        Me.TabPage_Information.Text = "Information"
        Me.TabPage_Information.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "imageres_5332_16x16-32.png")
        Me.ImageList1.Images.SetKeyName(1, "imageres_5333_16x16-32.png")
        '
        'StatusStrip_BasicPawn
        '
        Me.StatusStrip_BasicPawn.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel_EditorLine, Me.ToolStripStatusLabel_EditorCollum, Me.ToolStripStatusLabel_EditorSelectedCount, Me.ToolStripStatusLabel_CurrentConfig, Me.ToolStripStatusLabel_Project, Me.ToolStripStatusLabel_LastInformation, Me.ToolStripProgressBar_Autocomplete, Me.ToolStripStatusLabel_AppVersion})
        Me.StatusStrip_BasicPawn.Location = New System.Drawing.Point(0, 707)
        Me.StatusStrip_BasicPawn.Name = "StatusStrip_BasicPawn"
        Me.StatusStrip_BasicPawn.ShowItemToolTips = True
        Me.StatusStrip_BasicPawn.Size = New System.Drawing.Size(1008, 22)
        Me.StatusStrip_BasicPawn.TabIndex = 4
        Me.StatusStrip_BasicPawn.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel_EditorLine
        '
        Me.ToolStripStatusLabel_EditorLine.Name = "ToolStripStatusLabel_EditorLine"
        Me.ToolStripStatusLabel_EditorLine.Size = New System.Drawing.Size(25, 17)
        Me.ToolStripStatusLabel_EditorLine.Text = "L: 0"
        Me.ToolStripStatusLabel_EditorLine.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        '
        'ToolStripStatusLabel_EditorCollum
        '
        Me.ToolStripStatusLabel_EditorCollum.Name = "ToolStripStatusLabel_EditorCollum"
        Me.ToolStripStatusLabel_EditorCollum.Size = New System.Drawing.Size(27, 17)
        Me.ToolStripStatusLabel_EditorCollum.Text = "C: 0"
        '
        'ToolStripStatusLabel_EditorSelectedCount
        '
        Me.ToolStripStatusLabel_EditorSelectedCount.Name = "ToolStripStatusLabel_EditorSelectedCount"
        Me.ToolStripStatusLabel_EditorSelectedCount.Size = New System.Drawing.Size(25, 17)
        Me.ToolStripStatusLabel_EditorSelectedCount.Text = "S: 0"
        '
        'ToolStripStatusLabel_CurrentConfig
        '
        Me.ToolStripStatusLabel_CurrentConfig.ActiveLinkColor = System.Drawing.SystemColors.Highlight
        Me.ToolStripStatusLabel_CurrentConfig.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.ToolStripStatusLabel_CurrentConfig.IsLink = True
        Me.ToolStripStatusLabel_CurrentConfig.LinkColor = System.Drawing.SystemColors.HotTrack
        Me.ToolStripStatusLabel_CurrentConfig.Name = "ToolStripStatusLabel_CurrentConfig"
        Me.ToolStripStatusLabel_CurrentConfig.Size = New System.Drawing.Size(87, 17)
        Me.ToolStripStatusLabel_CurrentConfig.Text = "Config: Default"
        Me.ToolStripStatusLabel_CurrentConfig.VisitedLinkColor = System.Drawing.SystemColors.HotTrack
        '
        'ToolStripStatusLabel_Project
        '
        Me.ToolStripStatusLabel_Project.Name = "ToolStripStatusLabel_Project"
        Me.ToolStripStatusLabel_Project.Size = New System.Drawing.Size(79, 17)
        Me.ToolStripStatusLabel_Project.Text = "Project: None"
        Me.ToolStripStatusLabel_Project.Visible = False
        '
        'ToolStripStatusLabel_LastInformation
        '
        Me.ToolStripStatusLabel_LastInformation.Name = "ToolStripStatusLabel_LastInformation"
        Me.ToolStripStatusLabel_LastInformation.Size = New System.Drawing.Size(696, 17)
        Me.ToolStripStatusLabel_LastInformation.Spring = True
        Me.ToolStripStatusLabel_LastInformation.Text = "Last Info: No information"
        '
        'ToolStripProgressBar_Autocomplete
        '
        Me.ToolStripProgressBar_Autocomplete.Name = "ToolStripProgressBar_Autocomplete"
        Me.ToolStripProgressBar_Autocomplete.Size = New System.Drawing.Size(100, 16)
        '
        'ToolStripStatusLabel_AppVersion
        '
        Me.ToolStripStatusLabel_AppVersion.Name = "ToolStripStatusLabel_AppVersion"
        Me.ToolStripStatusLabel_AppVersion.Size = New System.Drawing.Size(31, 17)
        Me.ToolStripStatusLabel_AppVersion.Text = "v.0.0"
        '
        'Timer_PingFlash
        '
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1008, 729)
        Me.Controls.Add(Me.SplitContainer_ToolboxSourceAndDetails)
        Me.Controls.Add(Me.MenuStrip_BasicPawn)
        Me.Controls.Add(Me.StatusStrip_BasicPawn)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip_BasicPawn
        Me.MinimumSize = New System.Drawing.Size(640, 480)
        Me.Name = "FormMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BasicPawn"
        Me.ContextMenuStrip_RightClick.ResumeLayout(False)
        Me.MenuStrip_BasicPawn.ResumeLayout(False)
        Me.MenuStrip_BasicPawn.PerformLayout()
        Me.SplitContainer_ToolboxSourceAndDetails.Panel1.ResumeLayout(False)
        Me.SplitContainer_ToolboxSourceAndDetails.Panel2.ResumeLayout(False)
        Me.SplitContainer_ToolboxSourceAndDetails.ResumeLayout(False)
        Me.SplitContainer_ToolboxAndEditor.Panel1.ResumeLayout(False)
        Me.SplitContainer_ToolboxAndEditor.Panel2.ResumeLayout(False)
        Me.SplitContainer_ToolboxAndEditor.ResumeLayout(False)
        Me.TabControl_Toolbox.ResumeLayout(False)
        Me.TabControl_SourceTabs.ResumeLayout(False)
        Me.ContextMenuStrip_Tabs.ResumeLayout(False)
        Me.TabControl_Details.ResumeLayout(False)
        Me.StatusStrip_BasicPawn.ResumeLayout(False)
        Me.StatusStrip_BasicPawn.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ContextMenuStrip_RightClick As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_Mark As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_Paste As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Copy As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Cut As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_File As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Tools As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Build As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileOpen As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileSave As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileSaveAs As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Test As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileExit As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Help As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_ToolsAutocomplete As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ToolsAutocompleteUpdate As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ToolsAutocompleteShowAutocomplete As ToolStripMenuItem
    Friend WithEvents SplitContainer_ToolboxSourceAndDetails As SplitContainer
    Friend WithEvents TabPage_Autocomplete As TabPage
    Friend WithEvents TabPage_Information As TabPage
    Friend WithEvents ToolStripMenuItem_ToolsShowInformation As ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabel_EditorLine As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel_EditorCollum As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem_Undo As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Redo As ToolStripMenuItem
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents ToolStripMenuItem_FileNew As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ToolsSettingsAndConfigs As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripStatusLabel_CurrentConfig As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem_HelpSpecialControls As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpSpecialControlsAutocompleteUp As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpSpecialControlsAutocompleteDown As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpSpecialControlsAutocompletePaste As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpSpecialControlsMoveSelected As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpSpecialControlsCopySelected As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpAbout As ToolStripMenuItem
    Friend WithEvents ToolStripComboBox_ToolsAutocompleteSyntax As ToolStripComboBox
    Friend WithEvents ToolStripStatusLabel_LastInformation As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel_EditorSelectedCount As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem_ToolsFormatCode As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ListReferences As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ToolsClearInformationLog As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Shell As ToolStripMenuItem
    Friend WithEvents SplitContainer_ToolboxAndEditor As SplitContainer
    Friend WithEvents TabPage_ObjectBrowser As TabPage
    Friend WithEvents ToolStripMenuItem_ToolsSearchReplace As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpSpecialControlsDupLine As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_NewUpdate As ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabel_AppVersion As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem_Debug As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Debugger As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerBreakpoints As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerBreakpointInsert As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerBreakpointRemove As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerBreakpointRemoveAll As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_DebuggerWatchers As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerWatcherInsert As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerWatcherRemove As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerWatcherRemoveAll As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileSaveAsTemp As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HightlightCustom As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileOpenFolder As ToolStripMenuItem
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents ToolStripMenuItem_TabClose As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_TabMoveRight As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_TabMoveLeft As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileSaveAll As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_FileLoadTabs As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents Timer_PingFlash As Timer
    Friend WithEvents ToolStripMenuItem_TabOpenInstance As ToolStripMenuItem
    Friend WithEvents TabControl_SourceTabs As ClassTabControlColor
    Public WithEvents TabControl_Toolbox As ClassTabControlColor
    Public WithEvents TextEditorControl_Source As TextEditorControlEx
    Public WithEvents MenuStrip_BasicPawn As MenuStrip
    Public WithEvents TabControl_Details As ClassTabControlColor
    Public WithEvents StatusStrip_BasicPawn As StatusStrip
    Friend WithEvents ToolStripSeparator10 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_FileSavePacked As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpSpecialControlsCommentLines As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileStartPage As ToolStripMenuItem
    Friend WithEvents ToolStripProgressBar_Autocomplete As ToolStripProgressBar
    Friend WithEvents ToolStripMenuItem_Delete As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_SelectAll As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_Outline As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_OutlineCollapseAll As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_OutlineExpandAll As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileProjectSave As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileProjectLoad As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileCloseAll As ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabel_Project As ToolStripStatusLabel
    Friend WithEvents TabPage_ProjectBrowser As TabPage
    Friend WithEvents ToolStripMenuItem_FileProjectClose As ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_Tabs As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_Tabs_Close As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Tabs_CloseAllButThis As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Tabs_CloseAll As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator12 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_Tabs_OpenFolder As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Tabs_Popout As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpGithub As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Comment As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_OutlineToggleAll As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator13 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_ToolsConvertTabsSpaces As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator15 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_HelpCheckUpdates As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator14 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_ToolsAutocompleteCurrentMod As ToolStripMenuItem
End Class
