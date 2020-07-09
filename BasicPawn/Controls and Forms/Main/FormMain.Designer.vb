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
        Me.ToolStripSeparator24 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ListReferences = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FindDefinition = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_PeekDefinition = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.ToolStripMenuItem_DebuggerAsserts = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_DebuggerAssertInsert = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_DebuggerAssertRemove = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_DebuggerAssertRemoveAll = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.ToolStripMenuItem_FileNewWizard = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileLoadTabs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileClose = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.ToolStripMenuItem_FileProjectSaveAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FileProjectClose = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_FileSavePacked = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_FileOpenFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_FileExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Edit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditUndo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditRedo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator22 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_EditCut = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditSelectAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator23 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditDupLine = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditLineUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditLineDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditInsertLineUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditInsertLineDown = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_View = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ViewToolbox = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ViewDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ViewMinimap = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ViewProgressAni = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Tools = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsSettingsAndConfigs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ToolsFormatCode = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsFormatCodeIndent = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsFormatCodeTrim = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsConvertTabsSpaces = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsConvertToSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ToolsConvertToTab = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsConvertToSpace = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripTextBox_ToolsConvertSpaceSize = New System.Windows.Forms.ToolStripTextBox()
        Me.ToolStripMenuItem_ToolsSearchReplace = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsAutocomplete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsAutocompleteUpdateAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripComboBox_ToolsAutocompleteSyntax = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripMenuItem_ToolsAutocompleteCurrentMod = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ToolsAutocompleteShowAutocomplete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Bookmarks = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_BookmarksAdd = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_BookmarksRemoveLines = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator21 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_BookmarksShow = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ToolsShowInformation = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ToolsClearInformationLog = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Undo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Redo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Build = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_BuildCurrent = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_BuildAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Test = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TestCurrent = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TestAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator20 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_TestDebug = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Help = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpControls = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpControlsTabNav = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpControlsDetailsNav = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpControlsDetailsPrimAction = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpControlsDetailsSecAction = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpControlsMoveSelected = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpControlsCopySelected = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_HelpCheckUpdates = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpGithub = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ShowTips = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_HelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TabClose = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TabMoveRight = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TabMoveLeft = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TabLastViewedRight = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TabLastViewedLeft = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TabOpenInstance = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_NewUpdate = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer_ToolboxSourceAndDetails = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer_ToolboxAndEditor = New System.Windows.Forms.SplitContainer()
        Me.ContextMenuStrip_Tabs = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_Tabs_Close = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Tabs_CloseAllButThis = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Tabs_CloseAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_Tabs_Cut = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Tabs_Insert = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_Tabs_OpenFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Tabs_Popout = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList_Details = New System.Windows.Forms.ImageList(Me.components)
        Me.StatusStrip_BasicPawn = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel_EditorLine = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_EditorCollum = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_EditorSelectedCount = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_CurrentConfig = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_Project = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_Spacer = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_AutocompleteProgress = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_AppVersion = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Timer_PingFlash = New System.Windows.Forms.Timer(Me.components)
        Me.Timer_CheckFiles = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip_Config = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_EditConfigActiveTab = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditConfigAllTabs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_FindOptimalConfigActiveTab = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_FindOptimalConfigAllTabs = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer_SyntaxAnimation = New System.Windows.Forms.Timer(Me.components)
        Me.TabControl_Toolbox = New BasicPawn.ClassTabControlColor()
        Me.TabPage_ObjectBrowser = New System.Windows.Forms.TabPage()
        Me.TabPage_ProjectBrowser = New System.Windows.Forms.TabPage()
        Me.TabPage_ExplorerBrowser = New System.Windows.Forms.TabPage()
        Me.TabControl_SourceTabs = New BasicPawn.ClassTabControlColor()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabControl_Details = New BasicPawn.ClassTabControlColor()
        Me.TabPage_Autocomplete = New System.Windows.Forms.TabPage()
        Me.TabPage_Information = New System.Windows.Forms.TabPage()
        Me.TabPage_Bookmarks = New System.Windows.Forms.TabPage()
        Me.Timer_AutoSave = New System.Windows.Forms.Timer(Me.components)
        Me.ContextMenuStrip_RightClick.SuspendLayout()
        Me.MenuStrip_BasicPawn.SuspendLayout()
        Me.SplitContainer_ToolboxSourceAndDetails.Panel1.SuspendLayout()
        Me.SplitContainer_ToolboxSourceAndDetails.Panel2.SuspendLayout()
        Me.SplitContainer_ToolboxSourceAndDetails.SuspendLayout()
        Me.SplitContainer_ToolboxAndEditor.Panel1.SuspendLayout()
        Me.SplitContainer_ToolboxAndEditor.Panel2.SuspendLayout()
        Me.SplitContainer_ToolboxAndEditor.SuspendLayout()
        Me.ContextMenuStrip_Tabs.SuspendLayout()
        Me.StatusStrip_BasicPawn.SuspendLayout()
        Me.ContextMenuStrip_Config.SuspendLayout()
        Me.TabControl_Toolbox.SuspendLayout()
        Me.TabControl_SourceTabs.SuspendLayout()
        Me.TabControl_Details.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip_RightClick
        '
        Me.ContextMenuStrip_RightClick.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_Mark, Me.ToolStripSeparator24, Me.ToolStripMenuItem_ListReferences, Me.ToolStripMenuItem_FindDefinition, Me.ToolStripMenuItem_PeekDefinition, Me.ToolStripSeparator6, Me.ToolStripMenuItem_Cut, Me.ToolStripMenuItem_Copy, Me.ToolStripMenuItem_Paste, Me.ToolStripMenuItem_Delete, Me.ToolStripMenuItem_SelectAll, Me.ToolStripSeparator1, Me.ToolStripMenuItem_Debugger, Me.ToolStripMenuItem_HightlightCustom, Me.ToolStripMenuItem_Comment, Me.ToolStripSeparator11, Me.ToolStripMenuItem_Outline})
        Me.ContextMenuStrip_RightClick.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip_RightClick.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_RightClick.Size = New System.Drawing.Size(256, 314)
        '
        'ToolStripMenuItem_Mark
        '
        Me.ToolStripMenuItem_Mark.Image = CType(resources.GetObject("ToolStripMenuItem_Mark.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Mark.Name = "ToolStripMenuItem_Mark"
        Me.ToolStripMenuItem_Mark.Size = New System.Drawing.Size(255, 22)
        Me.ToolStripMenuItem_Mark.Text = "Mark"
        '
        'ToolStripSeparator24
        '
        Me.ToolStripSeparator24.Name = "ToolStripSeparator24"
        Me.ToolStripSeparator24.Size = New System.Drawing.Size(252, 6)
        '
        'ToolStripMenuItem_ListReferences
        '
        Me.ToolStripMenuItem_ListReferences.Image = CType(resources.GetObject("ToolStripMenuItem_ListReferences.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ListReferences.Name = "ToolStripMenuItem_ListReferences"
        Me.ToolStripMenuItem_ListReferences.ShortcutKeyDisplayString = "Ctrl+L"
        Me.ToolStripMenuItem_ListReferences.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_ListReferences.Size = New System.Drawing.Size(255, 22)
        Me.ToolStripMenuItem_ListReferences.Text = "List references"
        '
        'ToolStripMenuItem_FindDefinition
        '
        Me.ToolStripMenuItem_FindDefinition.Image = Global.BasicPawn.My.Resources.Resources.imageres_5357_16x16
        Me.ToolStripMenuItem_FindDefinition.Name = "ToolStripMenuItem_FindDefinition"
        Me.ToolStripMenuItem_FindDefinition.ShortcutKeyDisplayString = "Ctrl+Shift+L"
        Me.ToolStripMenuItem_FindDefinition.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FindDefinition.Size = New System.Drawing.Size(255, 22)
        Me.ToolStripMenuItem_FindDefinition.Text = "Find definition"
        '
        'ToolStripMenuItem_PeekDefinition
        '
        Me.ToolStripMenuItem_PeekDefinition.Name = "ToolStripMenuItem_PeekDefinition"
        Me.ToolStripMenuItem_PeekDefinition.ShortcutKeyDisplayString = "Ctrl+Alt+L"
        Me.ToolStripMenuItem_PeekDefinition.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_PeekDefinition.Size = New System.Drawing.Size(255, 22)
        Me.ToolStripMenuItem_PeekDefinition.Text = "Peek definition"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(252, 6)
        '
        'ToolStripMenuItem_Cut
        '
        Me.ToolStripMenuItem_Cut.Image = Global.BasicPawn.My.Resources.Resources.shell32_16762_16x16
        Me.ToolStripMenuItem_Cut.Name = "ToolStripMenuItem_Cut"
        Me.ToolStripMenuItem_Cut.ShortcutKeyDisplayString = "Ctrl+X"
        Me.ToolStripMenuItem_Cut.Size = New System.Drawing.Size(255, 22)
        Me.ToolStripMenuItem_Cut.Text = "Cut"
        '
        'ToolStripMenuItem_Copy
        '
        Me.ToolStripMenuItem_Copy.Image = CType(resources.GetObject("ToolStripMenuItem_Copy.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Copy.Name = "ToolStripMenuItem_Copy"
        Me.ToolStripMenuItem_Copy.ShortcutKeyDisplayString = "Ctrl+C"
        Me.ToolStripMenuItem_Copy.Size = New System.Drawing.Size(255, 22)
        Me.ToolStripMenuItem_Copy.Text = "Copy"
        '
        'ToolStripMenuItem_Paste
        '
        Me.ToolStripMenuItem_Paste.Image = Global.BasicPawn.My.Resources.Resources.shell32_16763_16x16
        Me.ToolStripMenuItem_Paste.Name = "ToolStripMenuItem_Paste"
        Me.ToolStripMenuItem_Paste.ShortcutKeyDisplayString = "Ctrl+V"
        Me.ToolStripMenuItem_Paste.Size = New System.Drawing.Size(255, 22)
        Me.ToolStripMenuItem_Paste.Text = "Paste"
        '
        'ToolStripMenuItem_Delete
        '
        Me.ToolStripMenuItem_Delete.Image = Global.BasicPawn.My.Resources.Resources.imageres_5337_16x16
        Me.ToolStripMenuItem_Delete.Name = "ToolStripMenuItem_Delete"
        Me.ToolStripMenuItem_Delete.ShortcutKeyDisplayString = "Del"
        Me.ToolStripMenuItem_Delete.Size = New System.Drawing.Size(255, 22)
        Me.ToolStripMenuItem_Delete.Text = "Delete"
        '
        'ToolStripMenuItem_SelectAll
        '
        Me.ToolStripMenuItem_SelectAll.Image = Global.BasicPawn.My.Resources.Resources.imageres_5312_16x16
        Me.ToolStripMenuItem_SelectAll.Name = "ToolStripMenuItem_SelectAll"
        Me.ToolStripMenuItem_SelectAll.ShortcutKeyDisplayString = "Ctrl+A"
        Me.ToolStripMenuItem_SelectAll.Size = New System.Drawing.Size(255, 22)
        Me.ToolStripMenuItem_SelectAll.Text = "Select all"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(252, 6)
        '
        'ToolStripMenuItem_Debugger
        '
        Me.ToolStripMenuItem_Debugger.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_DebuggerBreakpoints, Me.ToolStripMenuItem_DebuggerWatchers, Me.ToolStripMenuItem_DebuggerAsserts})
        Me.ToolStripMenuItem_Debugger.Image = CType(resources.GetObject("ToolStripMenuItem_Debugger.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Debugger.Name = "ToolStripMenuItem_Debugger"
        Me.ToolStripMenuItem_Debugger.Size = New System.Drawing.Size(255, 22)
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
        'ToolStripMenuItem_DebuggerAsserts
        '
        Me.ToolStripMenuItem_DebuggerAsserts.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_DebuggerAssertInsert, Me.ToolStripMenuItem_DebuggerAssertRemove, Me.ToolStripMenuItem_DebuggerAssertRemoveAll})
        Me.ToolStripMenuItem_DebuggerAsserts.Name = "ToolStripMenuItem_DebuggerAsserts"
        Me.ToolStripMenuItem_DebuggerAsserts.Size = New System.Drawing.Size(136, 22)
        Me.ToolStripMenuItem_DebuggerAsserts.Text = "Asserts"
        '
        'ToolStripMenuItem_DebuggerAssertInsert
        '
        Me.ToolStripMenuItem_DebuggerAssertInsert.Name = "ToolStripMenuItem_DebuggerAssertInsert"
        Me.ToolStripMenuItem_DebuggerAssertInsert.Size = New System.Drawing.Size(170, 22)
        Me.ToolStripMenuItem_DebuggerAssertInsert.Text = "Insert assert"
        '
        'ToolStripMenuItem_DebuggerAssertRemove
        '
        Me.ToolStripMenuItem_DebuggerAssertRemove.Name = "ToolStripMenuItem_DebuggerAssertRemove"
        Me.ToolStripMenuItem_DebuggerAssertRemove.Size = New System.Drawing.Size(170, 22)
        Me.ToolStripMenuItem_DebuggerAssertRemove.Text = "Remove assert"
        '
        'ToolStripMenuItem_DebuggerAssertRemoveAll
        '
        Me.ToolStripMenuItem_DebuggerAssertRemoveAll.Name = "ToolStripMenuItem_DebuggerAssertRemoveAll"
        Me.ToolStripMenuItem_DebuggerAssertRemoveAll.Size = New System.Drawing.Size(170, 22)
        Me.ToolStripMenuItem_DebuggerAssertRemoveAll.Text = "Remove all asserts"
        '
        'ToolStripMenuItem_HightlightCustom
        '
        Me.ToolStripMenuItem_HightlightCustom.Image = Global.BasicPawn.My.Resources.Resources.imageres_5313_16x16
        Me.ToolStripMenuItem_HightlightCustom.Name = "ToolStripMenuItem_HightlightCustom"
        Me.ToolStripMenuItem_HightlightCustom.Size = New System.Drawing.Size(255, 22)
        Me.ToolStripMenuItem_HightlightCustom.Text = "Highlight color"
        '
        'ToolStripMenuItem_Comment
        '
        Me.ToolStripMenuItem_Comment.Image = Global.BasicPawn.My.Resources.Resources.imageres_5348_16x16
        Me.ToolStripMenuItem_Comment.Name = "ToolStripMenuItem_Comment"
        Me.ToolStripMenuItem_Comment.ShortcutKeyDisplayString = "Ctrl+Num /"
        Me.ToolStripMenuItem_Comment.Size = New System.Drawing.Size(255, 22)
        Me.ToolStripMenuItem_Comment.Text = "Comment line in/out"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(252, 6)
        '
        'ToolStripMenuItem_Outline
        '
        Me.ToolStripMenuItem_Outline.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_OutlineToggleAll, Me.ToolStripSeparator13, Me.ToolStripMenuItem_OutlineCollapseAll, Me.ToolStripMenuItem_OutlineExpandAll})
        Me.ToolStripMenuItem_Outline.Image = Global.BasicPawn.My.Resources.Resources.imageres_5302_16x16
        Me.ToolStripMenuItem_Outline.Name = "ToolStripMenuItem_Outline"
        Me.ToolStripMenuItem_Outline.Size = New System.Drawing.Size(255, 22)
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
        Me.MenuStrip_BasicPawn.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_File, Me.ToolStripMenuItem_Edit, Me.ToolStripMenuItem_View, Me.ToolStripMenuItem_Tools, Me.ToolStripMenuItem_Undo, Me.ToolStripMenuItem_Redo, Me.ToolStripMenuItem_Build, Me.ToolStripMenuItem_Test, Me.ToolStripMenuItem_Help, Me.ToolStripMenuItem_TabClose, Me.ToolStripMenuItem_TabMoveRight, Me.ToolStripMenuItem_TabMoveLeft, Me.ToolStripMenuItem_TabLastViewedRight, Me.ToolStripMenuItem_TabLastViewedLeft, Me.ToolStripMenuItem_TabOpenInstance, Me.ToolStripMenuItem_NewUpdate})
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
        Me.ToolStripMenuItem_File.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_FileNew, Me.ToolStripMenuItem_FileNewWizard, Me.ToolStripMenuItem_FileOpen, Me.ToolStripMenuItem_FileLoadTabs, Me.ToolStripMenuItem_FileClose, Me.ToolStripMenuItem_FileCloseAll, Me.ToolStripSeparator9, Me.ToolStripMenuItem_FileSave, Me.ToolStripMenuItem_FileSaveAll, Me.ToolStripMenuItem_FileSaveAs, Me.ToolStripMenuItem_FileSaveAsTemp, Me.ToolStripSeparator7, Me.ToolStripMenuItem_FileStartPage, Me.ToolStripMenuItem_FileProjectLoad, Me.ToolStripMenuItem_FileProjectSave, Me.ToolStripMenuItem_FileProjectSaveAs, Me.ToolStripMenuItem_FileProjectClose, Me.ToolStripSeparator10, Me.ToolStripMenuItem_FileSavePacked, Me.ToolStripSeparator8, Me.ToolStripMenuItem_FileOpenFolder, Me.ToolStripSeparator2, Me.ToolStripMenuItem_FileExit})
        Me.ToolStripMenuItem_File.Image = CType(resources.GetObject("ToolStripMenuItem_File.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_File.Name = "ToolStripMenuItem_File"
        Me.ToolStripMenuItem_File.Size = New System.Drawing.Size(53, 20)
        Me.ToolStripMenuItem_File.Text = "&File"
        '
        'ToolStripMenuItem_FileNew
        '
        Me.ToolStripMenuItem_FileNew.Image = CType(resources.GetObject("ToolStripMenuItem_FileNew.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_FileNew.Name = "ToolStripMenuItem_FileNew"
        Me.ToolStripMenuItem_FileNew.ShortcutKeyDisplayString = "Ctrl+N"
        Me.ToolStripMenuItem_FileNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileNew.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileNew.Text = "&New"
        Me.ToolStripMenuItem_FileNew.ToolTipText = "Creates a new tab"
        '
        'ToolStripMenuItem_FileNewWizard
        '
        Me.ToolStripMenuItem_FileNewWizard.Image = Global.BasicPawn.My.Resources.Resources.imageres_5350_16x16
        Me.ToolStripMenuItem_FileNewWizard.Name = "ToolStripMenuItem_FileNewWizard"
        Me.ToolStripMenuItem_FileNewWizard.ShortcutKeyDisplayString = "Ctrl+Shift+N"
        Me.ToolStripMenuItem_FileNewWizard.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileNewWizard.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileNewWizard.Text = "New using templates"
        '
        'ToolStripMenuItem_FileOpen
        '
        Me.ToolStripMenuItem_FileOpen.Image = CType(resources.GetObject("ToolStripMenuItem_FileOpen.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_FileOpen.Name = "ToolStripMenuItem_FileOpen"
        Me.ToolStripMenuItem_FileOpen.ShortcutKeyDisplayString = "Ctrl+O"
        Me.ToolStripMenuItem_FileOpen.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileOpen.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileOpen.Text = "&Open"
        Me.ToolStripMenuItem_FileOpen.ToolTipText = "Open a existing file"
        '
        'ToolStripMenuItem_FileLoadTabs
        '
        Me.ToolStripMenuItem_FileLoadTabs.Image = Global.BasicPawn.My.Resources.Resources.imageres_5333_16x16
        Me.ToolStripMenuItem_FileLoadTabs.Name = "ToolStripMenuItem_FileLoadTabs"
        Me.ToolStripMenuItem_FileLoadTabs.ShortcutKeyDisplayString = "Ctrl+Shift+O"
        Me.ToolStripMenuItem_FileLoadTabs.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileLoadTabs.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileLoadTabs.Text = "Instance Manager"
        Me.ToolStripMenuItem_FileLoadTabs.ToolTipText = "Manage files from running BasicPawn instances"
        '
        'ToolStripMenuItem_FileClose
        '
        Me.ToolStripMenuItem_FileClose.Image = Global.BasicPawn.My.Resources.Resources.imageres_5337_16x16
        Me.ToolStripMenuItem_FileClose.Name = "ToolStripMenuItem_FileClose"
        Me.ToolStripMenuItem_FileClose.ShortcutKeyDisplayString = "Ctrl+W"
        Me.ToolStripMenuItem_FileClose.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileClose.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileClose.Text = "Close"
        '
        'ToolStripMenuItem_FileCloseAll
        '
        Me.ToolStripMenuItem_FileCloseAll.Image = Global.BasicPawn.My.Resources.Resources.imageres_5318_16x16
        Me.ToolStripMenuItem_FileCloseAll.Name = "ToolStripMenuItem_FileCloseAll"
        Me.ToolStripMenuItem_FileCloseAll.ShortcutKeyDisplayString = "Ctrl+Shift+W"
        Me.ToolStripMenuItem_FileCloseAll.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.W), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileCloseAll.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileCloseAll.Text = "Close all"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(257, 6)
        '
        'ToolStripMenuItem_FileSave
        '
        Me.ToolStripMenuItem_FileSave.Image = Global.BasicPawn.My.Resources.Resources.shell32_16761_16x16
        Me.ToolStripMenuItem_FileSave.Name = "ToolStripMenuItem_FileSave"
        Me.ToolStripMenuItem_FileSave.ShortcutKeyDisplayString = "Ctrl+S"
        Me.ToolStripMenuItem_FileSave.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileSave.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileSave.Text = "&Save"
        Me.ToolStripMenuItem_FileSave.ToolTipText = "Saves the current into a file"
        '
        'ToolStripMenuItem_FileSaveAll
        '
        Me.ToolStripMenuItem_FileSaveAll.Name = "ToolStripMenuItem_FileSaveAll"
        Me.ToolStripMenuItem_FileSaveAll.ShortcutKeyDisplayString = "Ctrl+Shift+S"
        Me.ToolStripMenuItem_FileSaveAll.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileSaveAll.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileSaveAll.Text = "Save all"
        Me.ToolStripMenuItem_FileSaveAll.ToolTipText = "Saves all open tabs into a file"
        '
        'ToolStripMenuItem_FileSaveAs
        '
        Me.ToolStripMenuItem_FileSaveAs.Name = "ToolStripMenuItem_FileSaveAs"
        Me.ToolStripMenuItem_FileSaveAs.ShortcutKeyDisplayString = "Ctrl+Alt+S"
        Me.ToolStripMenuItem_FileSaveAs.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
            Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_FileSaveAs.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileSaveAs.Text = "Save &as..."
        Me.ToolStripMenuItem_FileSaveAs.ToolTipText = "Saves the current into a file"
        '
        'ToolStripMenuItem_FileSaveAsTemp
        '
        Me.ToolStripMenuItem_FileSaveAsTemp.Name = "ToolStripMenuItem_FileSaveAsTemp"
        Me.ToolStripMenuItem_FileSaveAsTemp.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileSaveAsTemp.Text = "Save as &temporary"
        Me.ToolStripMenuItem_FileSaveAsTemp.ToolTipText = "Saves the current tab into the temporary folder"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(257, 6)
        '
        'ToolStripMenuItem_FileStartPage
        '
        Me.ToolStripMenuItem_FileStartPage.Image = Global.BasicPawn.My.Resources.Resources.imageres_5364_16x16
        Me.ToolStripMenuItem_FileStartPage.Name = "ToolStripMenuItem_FileStartPage"
        Me.ToolStripMenuItem_FileStartPage.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileStartPage.Text = "StartPage"
        '
        'ToolStripMenuItem_FileProjectLoad
        '
        Me.ToolStripMenuItem_FileProjectLoad.Image = Global.BasicPawn.My.Resources.Resources.imageres_5339_16x16
        Me.ToolStripMenuItem_FileProjectLoad.Name = "ToolStripMenuItem_FileProjectLoad"
        Me.ToolStripMenuItem_FileProjectLoad.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileProjectLoad.Text = "Load Project"
        '
        'ToolStripMenuItem_FileProjectSave
        '
        Me.ToolStripMenuItem_FileProjectSave.Image = Global.BasicPawn.My.Resources.Resources.imageres_5304_16x16
        Me.ToolStripMenuItem_FileProjectSave.Name = "ToolStripMenuItem_FileProjectSave"
        Me.ToolStripMenuItem_FileProjectSave.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileProjectSave.Text = "Save Project"
        '
        'ToolStripMenuItem_FileProjectSaveAs
        '
        Me.ToolStripMenuItem_FileProjectSaveAs.Name = "ToolStripMenuItem_FileProjectSaveAs"
        Me.ToolStripMenuItem_FileProjectSaveAs.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileProjectSaveAs.Text = "Save Project as..."
        '
        'ToolStripMenuItem_FileProjectClose
        '
        Me.ToolStripMenuItem_FileProjectClose.Image = Global.BasicPawn.My.Resources.Resources.imageres_5320_16x16
        Me.ToolStripMenuItem_FileProjectClose.Name = "ToolStripMenuItem_FileProjectClose"
        Me.ToolStripMenuItem_FileProjectClose.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileProjectClose.Text = "Close Project"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(257, 6)
        '
        'ToolStripMenuItem_FileSavePacked
        '
        Me.ToolStripMenuItem_FileSavePacked.Image = Global.BasicPawn.My.Resources.Resources.imageres_5303_16x16
        Me.ToolStripMenuItem_FileSavePacked.Name = "ToolStripMenuItem_FileSavePacked"
        Me.ToolStripMenuItem_FileSavePacked.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileSavePacked.Text = "&Export Packed"
        Me.ToolStripMenuItem_FileSavePacked.ToolTipText = "Packs all includes into one file and resolves all defines"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(257, 6)
        '
        'ToolStripMenuItem_FileOpenFolder
        '
        Me.ToolStripMenuItem_FileOpenFolder.Image = Global.BasicPawn.My.Resources.Resources.imageres_5304_16x16
        Me.ToolStripMenuItem_FileOpenFolder.Name = "ToolStripMenuItem_FileOpenFolder"
        Me.ToolStripMenuItem_FileOpenFolder.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileOpenFolder.Text = "Open current folder"
        Me.ToolStripMenuItem_FileOpenFolder.ToolTipText = "Opens the current tabs folder"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(257, 6)
        '
        'ToolStripMenuItem_FileExit
        '
        Me.ToolStripMenuItem_FileExit.Image = CType(resources.GetObject("ToolStripMenuItem_FileExit.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_FileExit.Name = "ToolStripMenuItem_FileExit"
        Me.ToolStripMenuItem_FileExit.Size = New System.Drawing.Size(260, 22)
        Me.ToolStripMenuItem_FileExit.Text = "Exit"
        Me.ToolStripMenuItem_FileExit.ToolTipText = "Quits BasicPawn"
        '
        'ToolStripMenuItem_Edit
        '
        Me.ToolStripMenuItem_Edit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_EditUndo, Me.ToolStripMenuItem_EditRedo, Me.ToolStripSeparator22, Me.ToolStripMenuItem_EditCut, Me.ToolStripMenuItem_EditCopy, Me.ToolStripMenuItem_EditPaste, Me.ToolStripMenuItem_EditDelete, Me.ToolStripMenuItem_EditSelectAll, Me.ToolStripSeparator23, Me.ToolStripMenuItem2})
        Me.ToolStripMenuItem_Edit.Image = Global.BasicPawn.My.Resources.Resources.imageres_5350_16x16
        Me.ToolStripMenuItem_Edit.Name = "ToolStripMenuItem_Edit"
        Me.ToolStripMenuItem_Edit.Size = New System.Drawing.Size(55, 20)
        Me.ToolStripMenuItem_Edit.Text = "Edit"
        '
        'ToolStripMenuItem_EditUndo
        '
        Me.ToolStripMenuItem_EditUndo.Image = Global.BasicPawn.My.Resources.Resources.imageres_5315_16x16
        Me.ToolStripMenuItem_EditUndo.Name = "ToolStripMenuItem_EditUndo"
        Me.ToolStripMenuItem_EditUndo.ShortcutKeyDisplayString = "Ctrl+Z"
        Me.ToolStripMenuItem_EditUndo.Size = New System.Drawing.Size(162, 22)
        Me.ToolStripMenuItem_EditUndo.Text = "Undo"
        '
        'ToolStripMenuItem_EditRedo
        '
        Me.ToolStripMenuItem_EditRedo.Image = Global.BasicPawn.My.Resources.Resources.imageres_5311_16x16
        Me.ToolStripMenuItem_EditRedo.Name = "ToolStripMenuItem_EditRedo"
        Me.ToolStripMenuItem_EditRedo.ShortcutKeyDisplayString = "Ctrl+Y"
        Me.ToolStripMenuItem_EditRedo.Size = New System.Drawing.Size(162, 22)
        Me.ToolStripMenuItem_EditRedo.Text = "Redo"
        '
        'ToolStripSeparator22
        '
        Me.ToolStripSeparator22.Name = "ToolStripSeparator22"
        Me.ToolStripSeparator22.Size = New System.Drawing.Size(159, 6)
        '
        'ToolStripMenuItem_EditCut
        '
        Me.ToolStripMenuItem_EditCut.Image = Global.BasicPawn.My.Resources.Resources.shell32_16762_16x16
        Me.ToolStripMenuItem_EditCut.Name = "ToolStripMenuItem_EditCut"
        Me.ToolStripMenuItem_EditCut.ShortcutKeyDisplayString = "Ctrl+X"
        Me.ToolStripMenuItem_EditCut.Size = New System.Drawing.Size(162, 22)
        Me.ToolStripMenuItem_EditCut.Text = "Cut"
        '
        'ToolStripMenuItem_EditCopy
        '
        Me.ToolStripMenuItem_EditCopy.Image = Global.BasicPawn.My.Resources.Resources.imageres_5306_16x16
        Me.ToolStripMenuItem_EditCopy.Name = "ToolStripMenuItem_EditCopy"
        Me.ToolStripMenuItem_EditCopy.ShortcutKeyDisplayString = "Ctrl+C"
        Me.ToolStripMenuItem_EditCopy.Size = New System.Drawing.Size(162, 22)
        Me.ToolStripMenuItem_EditCopy.Text = "Copy"
        '
        'ToolStripMenuItem_EditPaste
        '
        Me.ToolStripMenuItem_EditPaste.Image = Global.BasicPawn.My.Resources.Resources.shell32_16763_16x16
        Me.ToolStripMenuItem_EditPaste.Name = "ToolStripMenuItem_EditPaste"
        Me.ToolStripMenuItem_EditPaste.ShortcutKeyDisplayString = "Ctrl+V"
        Me.ToolStripMenuItem_EditPaste.Size = New System.Drawing.Size(162, 22)
        Me.ToolStripMenuItem_EditPaste.Text = "Paste"
        '
        'ToolStripMenuItem_EditDelete
        '
        Me.ToolStripMenuItem_EditDelete.Image = Global.BasicPawn.My.Resources.Resources.imageres_5337_16x16
        Me.ToolStripMenuItem_EditDelete.Name = "ToolStripMenuItem_EditDelete"
        Me.ToolStripMenuItem_EditDelete.ShortcutKeyDisplayString = "Del"
        Me.ToolStripMenuItem_EditDelete.Size = New System.Drawing.Size(162, 22)
        Me.ToolStripMenuItem_EditDelete.Text = "Delete"
        '
        'ToolStripMenuItem_EditSelectAll
        '
        Me.ToolStripMenuItem_EditSelectAll.Image = Global.BasicPawn.My.Resources.Resources.imageres_5312_16x16
        Me.ToolStripMenuItem_EditSelectAll.Name = "ToolStripMenuItem_EditSelectAll"
        Me.ToolStripMenuItem_EditSelectAll.ShortcutKeyDisplayString = "Ctrl+A"
        Me.ToolStripMenuItem_EditSelectAll.Size = New System.Drawing.Size(162, 22)
        Me.ToolStripMenuItem_EditSelectAll.Text = "Select all"
        '
        'ToolStripSeparator23
        '
        Me.ToolStripSeparator23.Name = "ToolStripSeparator23"
        Me.ToolStripSeparator23.Size = New System.Drawing.Size(159, 6)
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_EditDupLine, Me.ToolStripMenuItem_EditLineUp, Me.ToolStripMenuItem_EditLineDown, Me.ToolStripMenuItem_EditInsertLineUp, Me.ToolStripMenuItem_EditInsertLineDown})
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(162, 22)
        Me.ToolStripMenuItem2.Text = "Line Operations"
        '
        'ToolStripMenuItem_EditDupLine
        '
        Me.ToolStripMenuItem_EditDupLine.Name = "ToolStripMenuItem_EditDupLine"
        Me.ToolStripMenuItem_EditDupLine.ShortcutKeyDisplayString = "Ctrl+D"
        Me.ToolStripMenuItem_EditDupLine.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_EditDupLine.Size = New System.Drawing.Size(384, 22)
        Me.ToolStripMenuItem_EditDupLine.Text = "Duplicate Line/Selected"
        '
        'ToolStripMenuItem_EditLineUp
        '
        Me.ToolStripMenuItem_EditLineUp.Name = "ToolStripMenuItem_EditLineUp"
        Me.ToolStripMenuItem_EditLineUp.ShortcutKeyDisplayString = "Ctrl+Shift+Up"
        Me.ToolStripMenuItem_EditLineUp.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.Up), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_EditLineUp.Size = New System.Drawing.Size(384, 22)
        Me.ToolStripMenuItem_EditLineUp.Text = "Move Line Up"
        '
        'ToolStripMenuItem_EditLineDown
        '
        Me.ToolStripMenuItem_EditLineDown.Name = "ToolStripMenuItem_EditLineDown"
        Me.ToolStripMenuItem_EditLineDown.ShortcutKeyDisplayString = "Ctrl+Shift+Down"
        Me.ToolStripMenuItem_EditLineDown.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.Down), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_EditLineDown.Size = New System.Drawing.Size(384, 22)
        Me.ToolStripMenuItem_EditLineDown.Text = "Move Line Down"
        '
        'ToolStripMenuItem_EditInsertLineUp
        '
        Me.ToolStripMenuItem_EditInsertLineUp.Name = "ToolStripMenuItem_EditInsertLineUp"
        Me.ToolStripMenuItem_EditInsertLineUp.ShortcutKeyDisplayString = "Ctrl+Alt+Enter"
        Me.ToolStripMenuItem_EditInsertLineUp.Size = New System.Drawing.Size(384, 22)
        Me.ToolStripMenuItem_EditInsertLineUp.Text = "Insert Empty Line Above Current Line"
        '
        'ToolStripMenuItem_EditInsertLineDown
        '
        Me.ToolStripMenuItem_EditInsertLineDown.Name = "ToolStripMenuItem_EditInsertLineDown"
        Me.ToolStripMenuItem_EditInsertLineDown.ShortcutKeyDisplayString = "Shift+Ctrl+Alt+Enter"
        Me.ToolStripMenuItem_EditInsertLineDown.Size = New System.Drawing.Size(384, 22)
        Me.ToolStripMenuItem_EditInsertLineDown.Text = "Insert Empty Line Below Current Line"
        '
        'ToolStripMenuItem_View
        '
        Me.ToolStripMenuItem_View.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ViewToolbox, Me.ToolStripMenuItem_ViewDetails, Me.ToolStripMenuItem_ViewMinimap, Me.ToolStripSeparator18, Me.ToolStripMenuItem_ViewProgressAni})
        Me.ToolStripMenuItem_View.Image = Global.BasicPawn.My.Resources.Resources.imageres_5321_16x16
        Me.ToolStripMenuItem_View.Name = "ToolStripMenuItem_View"
        Me.ToolStripMenuItem_View.Size = New System.Drawing.Size(60, 20)
        Me.ToolStripMenuItem_View.Text = "&View"
        '
        'ToolStripMenuItem_ViewToolbox
        '
        Me.ToolStripMenuItem_ViewToolbox.CheckOnClick = True
        Me.ToolStripMenuItem_ViewToolbox.Name = "ToolStripMenuItem_ViewToolbox"
        Me.ToolStripMenuItem_ViewToolbox.Size = New System.Drawing.Size(208, 22)
        Me.ToolStripMenuItem_ViewToolbox.Text = "Toolbox"
        '
        'ToolStripMenuItem_ViewDetails
        '
        Me.ToolStripMenuItem_ViewDetails.CheckOnClick = True
        Me.ToolStripMenuItem_ViewDetails.Name = "ToolStripMenuItem_ViewDetails"
        Me.ToolStripMenuItem_ViewDetails.Size = New System.Drawing.Size(208, 22)
        Me.ToolStripMenuItem_ViewDetails.Text = "Details"
        '
        'ToolStripMenuItem_ViewMinimap
        '
        Me.ToolStripMenuItem_ViewMinimap.CheckOnClick = True
        Me.ToolStripMenuItem_ViewMinimap.Name = "ToolStripMenuItem_ViewMinimap"
        Me.ToolStripMenuItem_ViewMinimap.Size = New System.Drawing.Size(208, 22)
        Me.ToolStripMenuItem_ViewMinimap.Text = "Document Minimap"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(205, 6)
        '
        'ToolStripMenuItem_ViewProgressAni
        '
        Me.ToolStripMenuItem_ViewProgressAni.CheckOnClick = True
        Me.ToolStripMenuItem_ViewProgressAni.Name = "ToolStripMenuItem_ViewProgressAni"
        Me.ToolStripMenuItem_ViewProgressAni.Size = New System.Drawing.Size(208, 22)
        Me.ToolStripMenuItem_ViewProgressAni.Text = "Syntax parsing animation"
        '
        'ToolStripMenuItem_Tools
        '
        Me.ToolStripMenuItem_Tools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ToolsSettingsAndConfigs, Me.ToolStripSeparator3, Me.ToolStripMenuItem_ToolsFormatCode, Me.ToolStripMenuItem_ToolsConvertTabsSpaces, Me.ToolStripMenuItem_ToolsSearchReplace, Me.ToolStripMenuItem_ToolsAutocomplete, Me.ToolStripMenuItem_Bookmarks, Me.ToolStripSeparator4, Me.ToolStripMenuItem_ToolsShowInformation, Me.ToolStripMenuItem_ToolsClearInformationLog})
        Me.ToolStripMenuItem_Tools.Image = CType(resources.GetObject("ToolStripMenuItem_Tools.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Tools.Name = "ToolStripMenuItem_Tools"
        Me.ToolStripMenuItem_Tools.Size = New System.Drawing.Size(62, 20)
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
        Me.ToolStripMenuItem_ToolsFormatCode.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ToolsFormatCodeIndent, Me.ToolStripMenuItem_ToolsFormatCodeTrim})
        Me.ToolStripMenuItem_ToolsFormatCode.Image = CType(resources.GetObject("ToolStripMenuItem_ToolsFormatCode.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ToolsFormatCode.Name = "ToolStripMenuItem_ToolsFormatCode"
        Me.ToolStripMenuItem_ToolsFormatCode.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem_ToolsFormatCode.Text = "Format Code"
        '
        'ToolStripMenuItem_ToolsFormatCodeIndent
        '
        Me.ToolStripMenuItem_ToolsFormatCodeIndent.Name = "ToolStripMenuItem_ToolsFormatCodeIndent"
        Me.ToolStripMenuItem_ToolsFormatCodeIndent.ShortcutKeyDisplayString = "Ctrl+R"
        Me.ToolStripMenuItem_ToolsFormatCodeIndent.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_ToolsFormatCodeIndent.Size = New System.Drawing.Size(239, 22)
        Me.ToolStripMenuItem_ToolsFormatCodeIndent.Text = "Reindent Code"
        '
        'ToolStripMenuItem_ToolsFormatCodeTrim
        '
        Me.ToolStripMenuItem_ToolsFormatCodeTrim.Name = "ToolStripMenuItem_ToolsFormatCodeTrim"
        Me.ToolStripMenuItem_ToolsFormatCodeTrim.ShortcutKeyDisplayString = "Ctrl+T"
        Me.ToolStripMenuItem_ToolsFormatCodeTrim.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.T), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_ToolsFormatCodeTrim.Size = New System.Drawing.Size(239, 22)
        Me.ToolStripMenuItem_ToolsFormatCodeTrim.Text = "Trim ending whitespace"
        '
        'ToolStripMenuItem_ToolsConvertTabsSpaces
        '
        Me.ToolStripMenuItem_ToolsConvertTabsSpaces.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ToolsConvertToSettings, Me.ToolStripSeparator17, Me.ToolStripMenuItem_ToolsConvertToTab, Me.ToolStripMenuItem_ToolsConvertToSpace, Me.ToolStripMenuItem1, Me.ToolStripTextBox_ToolsConvertSpaceSize})
        Me.ToolStripMenuItem_ToolsConvertTabsSpaces.Image = Global.BasicPawn.My.Resources.Resources.imageres_5357_16x16
        Me.ToolStripMenuItem_ToolsConvertTabsSpaces.Name = "ToolStripMenuItem_ToolsConvertTabsSpaces"
        Me.ToolStripMenuItem_ToolsConvertTabsSpaces.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem_ToolsConvertTabsSpaces.Text = "Convert Tabs && Spaces"
        '
        'ToolStripMenuItem_ToolsConvertToSettings
        '
        Me.ToolStripMenuItem_ToolsConvertToSettings.Name = "ToolStripMenuItem_ToolsConvertToSettings"
        Me.ToolStripMenuItem_ToolsConvertToSettings.Size = New System.Drawing.Size(177, 22)
        Me.ToolStripMenuItem_ToolsConvertToSettings.Text = "Convert by Settings"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(174, 6)
        '
        'ToolStripMenuItem_ToolsConvertToTab
        '
        Me.ToolStripMenuItem_ToolsConvertToTab.Name = "ToolStripMenuItem_ToolsConvertToTab"
        Me.ToolStripMenuItem_ToolsConvertToTab.Size = New System.Drawing.Size(177, 22)
        Me.ToolStripMenuItem_ToolsConvertToTab.Text = "Convert to Tabs"
        '
        'ToolStripMenuItem_ToolsConvertToSpace
        '
        Me.ToolStripMenuItem_ToolsConvertToSpace.Name = "ToolStripMenuItem_ToolsConvertToSpace"
        Me.ToolStripMenuItem_ToolsConvertToSpace.Size = New System.Drawing.Size(177, 22)
        Me.ToolStripMenuItem_ToolsConvertToSpace.Text = "Convert to Spaces"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Enabled = False
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(177, 22)
        Me.ToolStripMenuItem1.Text = "Space size:"
        '
        'ToolStripTextBox_ToolsConvertSpaceSize
        '
        Me.ToolStripTextBox_ToolsConvertSpaceSize.MaxLength = 2
        Me.ToolStripTextBox_ToolsConvertSpaceSize.Name = "ToolStripTextBox_ToolsConvertSpaceSize"
        Me.ToolStripTextBox_ToolsConvertSpaceSize.Size = New System.Drawing.Size(100, 23)
        Me.ToolStripTextBox_ToolsConvertSpaceSize.Text = "4"
        '
        'ToolStripMenuItem_ToolsSearchReplace
        '
        Me.ToolStripMenuItem_ToolsSearchReplace.Image = CType(resources.GetObject("ToolStripMenuItem_ToolsSearchReplace.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ToolsSearchReplace.Name = "ToolStripMenuItem_ToolsSearchReplace"
        Me.ToolStripMenuItem_ToolsSearchReplace.ShortcutKeyDisplayString = "Ctrl+F"
        Me.ToolStripMenuItem_ToolsSearchReplace.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_ToolsSearchReplace.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem_ToolsSearchReplace.Text = "Search && Replace"
        '
        'ToolStripMenuItem_ToolsAutocomplete
        '
        Me.ToolStripMenuItem_ToolsAutocomplete.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_ToolsAutocompleteUpdate, Me.ToolStripMenuItem_ToolsAutocompleteUpdateAll, Me.ToolStripComboBox_ToolsAutocompleteSyntax, Me.ToolStripMenuItem_ToolsAutocompleteCurrentMod, Me.ToolStripSeparator5, Me.ToolStripMenuItem_ToolsAutocompleteShowAutocomplete})
        Me.ToolStripMenuItem_ToolsAutocomplete.Image = CType(resources.GetObject("ToolStripMenuItem_ToolsAutocomplete.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ToolsAutocomplete.Name = "ToolStripMenuItem_ToolsAutocomplete"
        Me.ToolStripMenuItem_ToolsAutocomplete.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem_ToolsAutocomplete.Text = "Autocomplete && IntelliSense"
        '
        'ToolStripMenuItem_ToolsAutocompleteUpdate
        '
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate.Image = CType(resources.GetObject("ToolStripMenuItem_ToolsAutocompleteUpdate.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate.Name = "ToolStripMenuItem_ToolsAutocompleteUpdate"
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate.ShortcutKeyDisplayString = "F5"
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate.Size = New System.Drawing.Size(360, 22)
        Me.ToolStripMenuItem_ToolsAutocompleteUpdate.Text = "Update"
        '
        'ToolStripMenuItem_ToolsAutocompleteUpdateAll
        '
        Me.ToolStripMenuItem_ToolsAutocompleteUpdateAll.Name = "ToolStripMenuItem_ToolsAutocompleteUpdateAll"
        Me.ToolStripMenuItem_ToolsAutocompleteUpdateAll.ShortcutKeyDisplayString = "Shift+F5"
        Me.ToolStripMenuItem_ToolsAutocompleteUpdateAll.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.F5), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_ToolsAutocompleteUpdateAll.Size = New System.Drawing.Size(360, 22)
        Me.ToolStripMenuItem_ToolsAutocompleteUpdateAll.Text = "Update all tabs"
        '
        'ToolStripComboBox_ToolsAutocompleteSyntax
        '
        Me.ToolStripComboBox_ToolsAutocompleteSyntax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBox_ToolsAutocompleteSyntax.DropDownWidth = 200
        Me.ToolStripComboBox_ToolsAutocompleteSyntax.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ToolStripComboBox_ToolsAutocompleteSyntax.Items.AddRange(New Object() {"Parse mixed syntax", "Parse SourcePawn, AMX Mod X and Pawn syntax", "Parse SourcePawn transitional syntax only"})
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
        'ToolStripMenuItem_Bookmarks
        '
        Me.ToolStripMenuItem_Bookmarks.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_BookmarksAdd, Me.ToolStripMenuItem_BookmarksRemoveLines, Me.ToolStripSeparator21, Me.ToolStripMenuItem_BookmarksShow})
        Me.ToolStripMenuItem_Bookmarks.Image = Global.BasicPawn.My.Resources.Resources.imageres_5354_16x16
        Me.ToolStripMenuItem_Bookmarks.Name = "ToolStripMenuItem_Bookmarks"
        Me.ToolStripMenuItem_Bookmarks.Size = New System.Drawing.Size(225, 22)
        Me.ToolStripMenuItem_Bookmarks.Text = "Bookmarks"
        '
        'ToolStripMenuItem_BookmarksAdd
        '
        Me.ToolStripMenuItem_BookmarksAdd.Image = Global.BasicPawn.My.Resources.Resources.imageres_5376_16x16
        Me.ToolStripMenuItem_BookmarksAdd.Name = "ToolStripMenuItem_BookmarksAdd"
        Me.ToolStripMenuItem_BookmarksAdd.ShortcutKeyDisplayString = "Ctrl+K"
        Me.ToolStripMenuItem_BookmarksAdd.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.K), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_BookmarksAdd.Size = New System.Drawing.Size(289, 22)
        Me.ToolStripMenuItem_BookmarksAdd.Text = "Add bookmark"
        '
        'ToolStripMenuItem_BookmarksRemoveLines
        '
        Me.ToolStripMenuItem_BookmarksRemoveLines.Name = "ToolStripMenuItem_BookmarksRemoveLines"
        Me.ToolStripMenuItem_BookmarksRemoveLines.ShortcutKeyDisplayString = "Ctrl+Shift+K"
        Me.ToolStripMenuItem_BookmarksRemoveLines.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
            Or System.Windows.Forms.Keys.K), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_BookmarksRemoveLines.Size = New System.Drawing.Size(289, 22)
        Me.ToolStripMenuItem_BookmarksRemoveLines.Text = "Remove bookmark at caret"
        '
        'ToolStripSeparator21
        '
        Me.ToolStripSeparator21.Name = "ToolStripSeparator21"
        Me.ToolStripSeparator21.Size = New System.Drawing.Size(286, 6)
        '
        'ToolStripMenuItem_BookmarksShow
        '
        Me.ToolStripMenuItem_BookmarksShow.Image = Global.BasicPawn.My.Resources.Resources.imageres_5350_16x16
        Me.ToolStripMenuItem_BookmarksShow.Name = "ToolStripMenuItem_BookmarksShow"
        Me.ToolStripMenuItem_BookmarksShow.Size = New System.Drawing.Size(289, 22)
        Me.ToolStripMenuItem_BookmarksShow.Text = "Show Bookmarks"
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
        Me.ToolStripMenuItem_Build.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_BuildCurrent, Me.ToolStripMenuItem_BuildAll})
        Me.ToolStripMenuItem_Build.Image = CType(resources.GetObject("ToolStripMenuItem_Build.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Build.Name = "ToolStripMenuItem_Build"
        Me.ToolStripMenuItem_Build.Size = New System.Drawing.Size(62, 20)
        Me.ToolStripMenuItem_Build.Text = "&Build"
        '
        'ToolStripMenuItem_BuildCurrent
        '
        Me.ToolStripMenuItem_BuildCurrent.Image = Global.BasicPawn.My.Resources.Resources.imageres_5343_16x16
        Me.ToolStripMenuItem_BuildCurrent.Name = "ToolStripMenuItem_BuildCurrent"
        Me.ToolStripMenuItem_BuildCurrent.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.ToolStripMenuItem_BuildCurrent.Size = New System.Drawing.Size(167, 22)
        Me.ToolStripMenuItem_BuildCurrent.Text = "Build current"
        '
        'ToolStripMenuItem_BuildAll
        '
        Me.ToolStripMenuItem_BuildAll.Name = "ToolStripMenuItem_BuildAll"
        Me.ToolStripMenuItem_BuildAll.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.F3), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_BuildAll.Size = New System.Drawing.Size(167, 22)
        Me.ToolStripMenuItem_BuildAll.Text = "Build all"
        '
        'ToolStripMenuItem_Test
        '
        Me.ToolStripMenuItem_Test.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_TestCurrent, Me.ToolStripMenuItem_TestAll, Me.ToolStripSeparator20, Me.ToolStripMenuItem_TestDebug})
        Me.ToolStripMenuItem_Test.Image = CType(resources.GetObject("ToolStripMenuItem_Test.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Test.Name = "ToolStripMenuItem_Test"
        Me.ToolStripMenuItem_Test.Size = New System.Drawing.Size(55, 20)
        Me.ToolStripMenuItem_Test.Text = "&Test"
        '
        'ToolStripMenuItem_TestCurrent
        '
        Me.ToolStripMenuItem_TestCurrent.Image = Global.BasicPawn.My.Resources.Resources.imageres_5343_16x16
        Me.ToolStripMenuItem_TestCurrent.Name = "ToolStripMenuItem_TestCurrent"
        Me.ToolStripMenuItem_TestCurrent.ShortcutKeys = System.Windows.Forms.Keys.F4
        Me.ToolStripMenuItem_TestCurrent.Size = New System.Drawing.Size(160, 22)
        Me.ToolStripMenuItem_TestCurrent.Text = "Test current"
        '
        'ToolStripMenuItem_TestAll
        '
        Me.ToolStripMenuItem_TestAll.Name = "ToolStripMenuItem_TestAll"
        Me.ToolStripMenuItem_TestAll.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.ToolStripMenuItem_TestAll.Size = New System.Drawing.Size(160, 22)
        Me.ToolStripMenuItem_TestAll.Text = "Test all"
        '
        'ToolStripSeparator20
        '
        Me.ToolStripSeparator20.Name = "ToolStripSeparator20"
        Me.ToolStripSeparator20.Size = New System.Drawing.Size(157, 6)
        '
        'ToolStripMenuItem_TestDebug
        '
        Me.ToolStripMenuItem_TestDebug.Image = Global.BasicPawn.My.Resources.Resources.imageres_5362_16x16
        Me.ToolStripMenuItem_TestDebug.Name = "ToolStripMenuItem_TestDebug"
        Me.ToolStripMenuItem_TestDebug.Size = New System.Drawing.Size(160, 22)
        Me.ToolStripMenuItem_TestDebug.Text = "Debug"
        '
        'ToolStripMenuItem_Help
        '
        Me.ToolStripMenuItem_Help.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_HelpControls, Me.ToolStripSeparator15, Me.ToolStripMenuItem_HelpCheckUpdates, Me.ToolStripMenuItem_HelpGithub, Me.ToolStripSeparator14, Me.ToolStripMenuItem_ShowTips, Me.ToolStripMenuItem_HelpAbout})
        Me.ToolStripMenuItem_Help.Image = CType(resources.GetObject("ToolStripMenuItem_Help.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Help.Name = "ToolStripMenuItem_Help"
        Me.ToolStripMenuItem_Help.Size = New System.Drawing.Size(60, 20)
        Me.ToolStripMenuItem_Help.Text = "&Help"
        '
        'ToolStripMenuItem_HelpControls
        '
        Me.ToolStripMenuItem_HelpControls.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_HelpControlsTabNav, Me.ToolStripMenuItem_HelpControlsDetailsNav, Me.ToolStripMenuItem_HelpControlsDetailsPrimAction, Me.ToolStripMenuItem_HelpControlsDetailsSecAction, Me.ToolStripMenuItem_HelpControlsMoveSelected, Me.ToolStripMenuItem_HelpControlsCopySelected})
        Me.ToolStripMenuItem_HelpControls.Image = CType(resources.GetObject("ToolStripMenuItem_HelpControls.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_HelpControls.Name = "ToolStripMenuItem_HelpControls"
        Me.ToolStripMenuItem_HelpControls.Size = New System.Drawing.Size(170, 22)
        Me.ToolStripMenuItem_HelpControls.Text = "Special Controls"
        '
        'ToolStripMenuItem_HelpControlsTabNav
        '
        Me.ToolStripMenuItem_HelpControlsTabNav.Name = "ToolStripMenuItem_HelpControlsTabNav"
        Me.ToolStripMenuItem_HelpControlsTabNav.ShortcutKeyDisplayString = "Ctrl+PageUp; Ctrl+PageDown"
        Me.ToolStripMenuItem_HelpControlsTabNav.Size = New System.Drawing.Size(464, 22)
        Me.ToolStripMenuItem_HelpControlsTabNav.Text = "Tab Navigation"
        '
        'ToolStripMenuItem_HelpControlsDetailsNav
        '
        Me.ToolStripMenuItem_HelpControlsDetailsNav.Name = "ToolStripMenuItem_HelpControlsDetailsNav"
        Me.ToolStripMenuItem_HelpControlsDetailsNav.ShortcutKeyDisplayString = "Ctrl+Up; Ctrl+Down; Ctrl+Alt+Left; Ctrl+Alt+Right"
        Me.ToolStripMenuItem_HelpControlsDetailsNav.Size = New System.Drawing.Size(464, 22)
        Me.ToolStripMenuItem_HelpControlsDetailsNav.Text = "Details Tab Navigation"
        '
        'ToolStripMenuItem_HelpControlsDetailsPrimAction
        '
        Me.ToolStripMenuItem_HelpControlsDetailsPrimAction.Name = "ToolStripMenuItem_HelpControlsDetailsPrimAction"
        Me.ToolStripMenuItem_HelpControlsDetailsPrimAction.ShortcutKeyDisplayString = "Ctrl+Enter"
        Me.ToolStripMenuItem_HelpControlsDetailsPrimAction.Size = New System.Drawing.Size(464, 22)
        Me.ToolStripMenuItem_HelpControlsDetailsPrimAction.Text = "Details Tab Primary Action"
        '
        'ToolStripMenuItem_HelpControlsDetailsSecAction
        '
        Me.ToolStripMenuItem_HelpControlsDetailsSecAction.Name = "ToolStripMenuItem_HelpControlsDetailsSecAction"
        Me.ToolStripMenuItem_HelpControlsDetailsSecAction.ShortcutKeyDisplayString = "Ctrl+Shift+Enter"
        Me.ToolStripMenuItem_HelpControlsDetailsSecAction.Size = New System.Drawing.Size(464, 22)
        Me.ToolStripMenuItem_HelpControlsDetailsSecAction.Text = "Details Tab Secondary Action"
        '
        'ToolStripMenuItem_HelpControlsMoveSelected
        '
        Me.ToolStripMenuItem_HelpControlsMoveSelected.Name = "ToolStripMenuItem_HelpControlsMoveSelected"
        Me.ToolStripMenuItem_HelpControlsMoveSelected.ShortcutKeyDisplayString = "Drag"
        Me.ToolStripMenuItem_HelpControlsMoveSelected.Size = New System.Drawing.Size(464, 22)
        Me.ToolStripMenuItem_HelpControlsMoveSelected.Text = "Move Selected"
        '
        'ToolStripMenuItem_HelpControlsCopySelected
        '
        Me.ToolStripMenuItem_HelpControlsCopySelected.Name = "ToolStripMenuItem_HelpControlsCopySelected"
        Me.ToolStripMenuItem_HelpControlsCopySelected.ShortcutKeyDisplayString = "Ctrl+Drag"
        Me.ToolStripMenuItem_HelpControlsCopySelected.Size = New System.Drawing.Size(464, 22)
        Me.ToolStripMenuItem_HelpControlsCopySelected.Text = "Copy Selected"
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
        'ToolStripMenuItem_ShowTips
        '
        Me.ToolStripMenuItem_ShowTips.Image = Global.BasicPawn.My.Resources.Resources.user32_102_16x16
        Me.ToolStripMenuItem_ShowTips.Name = "ToolStripMenuItem_ShowTips"
        Me.ToolStripMenuItem_ShowTips.Size = New System.Drawing.Size(170, 22)
        Me.ToolStripMenuItem_ShowTips.Text = "Show Tips"
        '
        'ToolStripMenuItem_HelpAbout
        '
        Me.ToolStripMenuItem_HelpAbout.Image = CType(resources.GetObject("ToolStripMenuItem_HelpAbout.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_HelpAbout.Name = "ToolStripMenuItem_HelpAbout"
        Me.ToolStripMenuItem_HelpAbout.Size = New System.Drawing.Size(170, 22)
        Me.ToolStripMenuItem_HelpAbout.Text = "About"
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
        Me.ToolStripMenuItem_TabClose.Size = New System.Drawing.Size(18, 20)
        Me.ToolStripMenuItem_TabClose.Text = "x"
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
        'ToolStripMenuItem_TabLastViewedRight
        '
        Me.ToolStripMenuItem_TabLastViewedRight.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripMenuItem_TabLastViewedRight.AutoToolTip = True
        Me.ToolStripMenuItem_TabLastViewedRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripMenuItem_TabLastViewedRight.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripMenuItem_TabLastViewedRight.Name = "ToolStripMenuItem_TabLastViewedRight"
        Me.ToolStripMenuItem_TabLastViewedRight.Padding = New System.Windows.Forms.Padding(0)
        Me.ToolStripMenuItem_TabLastViewedRight.Size = New System.Drawing.Size(27, 20)
        Me.ToolStripMenuItem_TabLastViewedRight.Text = ">>"
        Me.ToolStripMenuItem_TabLastViewedRight.ToolTipText = "Navigate to next selected tab"
        '
        'ToolStripMenuItem_TabLastViewedLeft
        '
        Me.ToolStripMenuItem_TabLastViewedLeft.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripMenuItem_TabLastViewedLeft.AutoToolTip = True
        Me.ToolStripMenuItem_TabLastViewedLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripMenuItem_TabLastViewedLeft.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripMenuItem_TabLastViewedLeft.Name = "ToolStripMenuItem_TabLastViewedLeft"
        Me.ToolStripMenuItem_TabLastViewedLeft.Padding = New System.Windows.Forms.Padding(0)
        Me.ToolStripMenuItem_TabLastViewedLeft.Size = New System.Drawing.Size(27, 20)
        Me.ToolStripMenuItem_TabLastViewedLeft.Text = "<<"
        Me.ToolStripMenuItem_TabLastViewedLeft.ToolTipText = "Navigate to previous selected tab"
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
        Me.ToolStripMenuItem_TabOpenInstance.ToolTipText = "Instance Manager"
        '
        'ToolStripMenuItem_NewUpdate
        '
        Me.ToolStripMenuItem_NewUpdate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripMenuItem_NewUpdate.Image = Global.BasicPawn.My.Resources.Resources.shell32_16739_16x16
        Me.ToolStripMenuItem_NewUpdate.Name = "ToolStripMenuItem_NewUpdate"
        Me.ToolStripMenuItem_NewUpdate.Size = New System.Drawing.Size(99, 20)
        Me.ToolStripMenuItem_NewUpdate.Text = "Update now"
        Me.ToolStripMenuItem_NewUpdate.ToolTipText = "A new BasicPawn version is available"
        Me.ToolStripMenuItem_NewUpdate.Visible = False
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
        'ContextMenuStrip_Tabs
        '
        Me.ContextMenuStrip_Tabs.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_Tabs_Close, Me.ToolStripMenuItem_Tabs_CloseAllButThis, Me.ToolStripMenuItem_Tabs_CloseAll, Me.ToolStripSeparator16, Me.ToolStripMenuItem_Tabs_Cut, Me.ToolStripMenuItem_Tabs_Insert, Me.ToolStripSeparator12, Me.ToolStripMenuItem_Tabs_OpenFolder, Me.ToolStripMenuItem_Tabs_Popout})
        Me.ContextMenuStrip_Tabs.Name = "ContextMenuStrip_Tabs"
        Me.ContextMenuStrip_Tabs.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_Tabs.Size = New System.Drawing.Size(179, 170)
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
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(175, 6)
        '
        'ToolStripMenuItem_Tabs_Cut
        '
        Me.ToolStripMenuItem_Tabs_Cut.Image = Global.BasicPawn.My.Resources.Resources.shell32_16762_16x16
        Me.ToolStripMenuItem_Tabs_Cut.Name = "ToolStripMenuItem_Tabs_Cut"
        Me.ToolStripMenuItem_Tabs_Cut.Size = New System.Drawing.Size(178, 22)
        Me.ToolStripMenuItem_Tabs_Cut.Text = "Cut"
        '
        'ToolStripMenuItem_Tabs_Insert
        '
        Me.ToolStripMenuItem_Tabs_Insert.Image = Global.BasicPawn.My.Resources.Resources.shell32_16763_16x16
        Me.ToolStripMenuItem_Tabs_Insert.Name = "ToolStripMenuItem_Tabs_Insert"
        Me.ToolStripMenuItem_Tabs_Insert.Size = New System.Drawing.Size(178, 22)
        Me.ToolStripMenuItem_Tabs_Insert.Text = "Insert"
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
        'ImageList_Details
        '
        Me.ImageList_Details.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList_Details.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList_Details.TransparentColor = System.Drawing.Color.Transparent
        '
        'StatusStrip_BasicPawn
        '
        Me.StatusStrip_BasicPawn.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel_EditorLine, Me.ToolStripStatusLabel_EditorCollum, Me.ToolStripStatusLabel_EditorSelectedCount, Me.ToolStripStatusLabel_CurrentConfig, Me.ToolStripStatusLabel_Project, Me.ToolStripStatusLabel_Spacer, Me.ToolStripStatusLabel_AutocompleteProgress, Me.ToolStripStatusLabel_AppVersion})
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
        'ToolStripStatusLabel_Spacer
        '
        Me.ToolStripStatusLabel_Spacer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None
        Me.ToolStripStatusLabel_Spacer.Name = "ToolStripStatusLabel_Spacer"
        Me.ToolStripStatusLabel_Spacer.Size = New System.Drawing.Size(798, 17)
        Me.ToolStripStatusLabel_Spacer.Spring = True
        '
        'ToolStripStatusLabel_AutocompleteProgress
        '
        Me.ToolStripStatusLabel_AutocompleteProgress.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripStatusLabel_AutocompleteProgress.Image = Global.BasicPawn.My.Resources.Resources.aero_busy
        Me.ToolStripStatusLabel_AutocompleteProgress.Name = "ToolStripStatusLabel_AutocompleteProgress"
        Me.ToolStripStatusLabel_AutocompleteProgress.Size = New System.Drawing.Size(16, 17)
        Me.ToolStripStatusLabel_AutocompleteProgress.Visible = False
        '
        'ToolStripStatusLabel_AppVersion
        '
        Me.ToolStripStatusLabel_AppVersion.Name = "ToolStripStatusLabel_AppVersion"
        Me.ToolStripStatusLabel_AppVersion.Size = New System.Drawing.Size(31, 17)
        Me.ToolStripStatusLabel_AppVersion.Text = "v.0.0"
        '
        'Timer_CheckFiles
        '
        Me.Timer_CheckFiles.Enabled = True
        Me.Timer_CheckFiles.Interval = 2500
        '
        'ContextMenuStrip_Config
        '
        Me.ContextMenuStrip_Config.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_EditConfigActiveTab, Me.ToolStripMenuItem_EditConfigAllTabs, Me.ToolStripSeparator19, Me.ToolStripMenuItem_FindOptimalConfigActiveTab, Me.ToolStripMenuItem_FindOptimalConfigAllTabs})
        Me.ContextMenuStrip_Config.Name = "ContextMenuStrip_Config"
        Me.ContextMenuStrip_Config.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_Config.Size = New System.Drawing.Size(251, 98)
        '
        'ToolStripMenuItem_EditConfigActiveTab
        '
        Me.ToolStripMenuItem_EditConfigActiveTab.Image = Global.BasicPawn.My.Resources.Resources.imageres_5364_16x16
        Me.ToolStripMenuItem_EditConfigActiveTab.Name = "ToolStripMenuItem_EditConfigActiveTab"
        Me.ToolStripMenuItem_EditConfigActiveTab.Size = New System.Drawing.Size(250, 22)
        Me.ToolStripMenuItem_EditConfigActiveTab.Text = "Change config for active tab..."
        '
        'ToolStripMenuItem_EditConfigAllTabs
        '
        Me.ToolStripMenuItem_EditConfigAllTabs.Name = "ToolStripMenuItem_EditConfigAllTabs"
        Me.ToolStripMenuItem_EditConfigAllTabs.Size = New System.Drawing.Size(250, 22)
        Me.ToolStripMenuItem_EditConfigAllTabs.Text = "Change config for all tabs..."
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(247, 6)
        '
        'ToolStripMenuItem_FindOptimalConfigActiveTab
        '
        Me.ToolStripMenuItem_FindOptimalConfigActiveTab.Image = Global.BasicPawn.My.Resources.Resources.imageres_5332_16x16
        Me.ToolStripMenuItem_FindOptimalConfigActiveTab.Name = "ToolStripMenuItem_FindOptimalConfigActiveTab"
        Me.ToolStripMenuItem_FindOptimalConfigActiveTab.Size = New System.Drawing.Size(250, 22)
        Me.ToolStripMenuItem_FindOptimalConfigActiveTab.Text = "Find optimal config for active tab"
        '
        'ToolStripMenuItem_FindOptimalConfigAllTabs
        '
        Me.ToolStripMenuItem_FindOptimalConfigAllTabs.Name = "ToolStripMenuItem_FindOptimalConfigAllTabs"
        Me.ToolStripMenuItem_FindOptimalConfigAllTabs.Size = New System.Drawing.Size(250, 22)
        Me.ToolStripMenuItem_FindOptimalConfigAllTabs.Text = "Find optimal config for all tabs"
        '
        'Timer_SyntaxAnimation
        '
        Me.Timer_SyntaxAnimation.Enabled = True
        Me.Timer_SyntaxAnimation.Interval = 1000
        '
        'TabControl_Toolbox
        '
        Me.TabControl_Toolbox.Controls.Add(Me.TabPage_ObjectBrowser)
        Me.TabControl_Toolbox.Controls.Add(Me.TabPage_ProjectBrowser)
        Me.TabControl_Toolbox.Controls.Add(Me.TabPage_ExplorerBrowser)
        Me.TabControl_Toolbox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl_Toolbox.Location = New System.Drawing.Point(0, 0)
        Me.TabControl_Toolbox.m_TabPageAdjustEnabled = True
        Me.TabControl_Toolbox.Margin = New System.Windows.Forms.Padding(0)
        Me.TabControl_Toolbox.Multiline = True
        Me.TabControl_Toolbox.Name = "TabControl_Toolbox"
        Me.TabControl_Toolbox.Padding = New System.Drawing.Point(0, 0)
        Me.TabControl_Toolbox.SelectedIndex = 0
        Me.TabControl_Toolbox.Size = New System.Drawing.Size(200, 500)
        Me.TabControl_Toolbox.TabIndex = 0
        '
        'TabPage_ObjectBrowser
        '
        Me.TabPage_ObjectBrowser.Location = New System.Drawing.Point(1, 21)
        Me.TabPage_ObjectBrowser.Margin = New System.Windows.Forms.Padding(0)
        Me.TabPage_ObjectBrowser.Name = "TabPage_ObjectBrowser"
        Me.TabPage_ObjectBrowser.Size = New System.Drawing.Size(196, 476)
        Me.TabPage_ObjectBrowser.TabIndex = 0
        Me.TabPage_ObjectBrowser.Text = "Object Browser"
        Me.TabPage_ObjectBrowser.UseVisualStyleBackColor = True
        '
        'TabPage_ProjectBrowser
        '
        Me.TabPage_ProjectBrowser.Location = New System.Drawing.Point(1, 21)
        Me.TabPage_ProjectBrowser.Name = "TabPage_ProjectBrowser"
        Me.TabPage_ProjectBrowser.Size = New System.Drawing.Size(196, 476)
        Me.TabPage_ProjectBrowser.TabIndex = 1
        Me.TabPage_ProjectBrowser.Text = "Project"
        Me.TabPage_ProjectBrowser.UseVisualStyleBackColor = True
        '
        'TabPage_ExplorerBrowser
        '
        Me.TabPage_ExplorerBrowser.Location = New System.Drawing.Point(1, 21)
        Me.TabPage_ExplorerBrowser.Name = "TabPage_ExplorerBrowser"
        Me.TabPage_ExplorerBrowser.Size = New System.Drawing.Size(196, 476)
        Me.TabPage_ExplorerBrowser.TabIndex = 2
        Me.TabPage_ExplorerBrowser.Text = "Explorer"
        Me.TabPage_ExplorerBrowser.UseVisualStyleBackColor = True
        '
        'TabControl_SourceTabs
        '
        Me.TabControl_SourceTabs.AllowDrop = True
        Me.TabControl_SourceTabs.ContextMenuStrip = Me.ContextMenuStrip_Tabs
        Me.TabControl_SourceTabs.Controls.Add(Me.TabPage1)
        Me.TabControl_SourceTabs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl_SourceTabs.Location = New System.Drawing.Point(0, 0)
        Me.TabControl_SourceTabs.m_TabPageAdjustEnabled = True
        Me.TabControl_SourceTabs.Margin = New System.Windows.Forms.Padding(0)
        Me.TabControl_SourceTabs.Name = "TabControl_SourceTabs"
        Me.TabControl_SourceTabs.Padding = New System.Drawing.Point(0, 0)
        Me.TabControl_SourceTabs.SelectedIndex = 0
        Me.TabControl_SourceTabs.ShowToolTips = True
        Me.TabControl_SourceTabs.Size = New System.Drawing.Size(804, 500)
        Me.TabControl_SourceTabs.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Location = New System.Drawing.Point(1, 21)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(0)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(800, 476)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "New*"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabControl_Details
        '
        Me.TabControl_Details.Controls.Add(Me.TabPage_Autocomplete)
        Me.TabControl_Details.Controls.Add(Me.TabPage_Information)
        Me.TabControl_Details.Controls.Add(Me.TabPage_Bookmarks)
        Me.TabControl_Details.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl_Details.ImageList = Me.ImageList_Details
        Me.TabControl_Details.Location = New System.Drawing.Point(0, 0)
        Me.TabControl_Details.m_TabPageAdjustEnabled = True
        Me.TabControl_Details.Name = "TabControl_Details"
        Me.TabControl_Details.SelectedIndex = 0
        Me.TabControl_Details.Size = New System.Drawing.Size(1008, 179)
        Me.TabControl_Details.TabIndex = 0
        '
        'TabPage_Autocomplete
        '
        Me.TabPage_Autocomplete.Location = New System.Drawing.Point(1, 22)
        Me.TabPage_Autocomplete.Name = "TabPage_Autocomplete"
        Me.TabPage_Autocomplete.Size = New System.Drawing.Size(1004, 154)
        Me.TabPage_Autocomplete.TabIndex = 0
        Me.TabPage_Autocomplete.Text = "Autocomplete & IntelliSense"
        Me.TabPage_Autocomplete.UseVisualStyleBackColor = True
        '
        'TabPage_Information
        '
        Me.TabPage_Information.Location = New System.Drawing.Point(1, 22)
        Me.TabPage_Information.Name = "TabPage_Information"
        Me.TabPage_Information.Size = New System.Drawing.Size(1004, 154)
        Me.TabPage_Information.TabIndex = 1
        Me.TabPage_Information.Text = "Information"
        Me.TabPage_Information.UseVisualStyleBackColor = True
        '
        'TabPage_Bookmarks
        '
        Me.TabPage_Bookmarks.Location = New System.Drawing.Point(1, 22)
        Me.TabPage_Bookmarks.Name = "TabPage_Bookmarks"
        Me.TabPage_Bookmarks.Size = New System.Drawing.Size(1004, 154)
        Me.TabPage_Bookmarks.TabIndex = 2
        Me.TabPage_Bookmarks.Text = "Bookmarks"
        Me.TabPage_Bookmarks.UseVisualStyleBackColor = True
        '
        'Timer_AutoSave
        '
        Me.Timer_AutoSave.Enabled = True
        Me.Timer_AutoSave.Interval = 7500
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
        Me.ContextMenuStrip_Tabs.ResumeLayout(False)
        Me.StatusStrip_BasicPawn.ResumeLayout(False)
        Me.StatusStrip_BasicPawn.PerformLayout()
        Me.ContextMenuStrip_Config.ResumeLayout(False)
        Me.TabControl_Toolbox.ResumeLayout(False)
        Me.TabControl_SourceTabs.ResumeLayout(False)
        Me.TabControl_Details.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStripMenuItem_FileOpen As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileSave As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileSaveAs As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileExit As ToolStripMenuItem
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
    Friend WithEvents ImageList_Details As ImageList
    Friend WithEvents ToolStripMenuItem_FileNew As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ToolsSettingsAndConfigs As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripStatusLabel_CurrentConfig As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem_HelpControls As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpControlsDetailsNav As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpControlsDetailsPrimAction As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpControlsMoveSelected As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpControlsCopySelected As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpAbout As ToolStripMenuItem
    Friend WithEvents ToolStripComboBox_ToolsAutocompleteSyntax As ToolStripComboBox
    Friend WithEvents ToolStripStatusLabel_EditorSelectedCount As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem_ToolsFormatCode As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ToolsClearInformationLog As ToolStripMenuItem
    Friend WithEvents SplitContainer_ToolboxAndEditor As SplitContainer
    Friend WithEvents TabPage_ObjectBrowser As TabPage
    Friend WithEvents ToolStripMenuItem_ToolsSearchReplace As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripStatusLabel_AppVersion As ToolStripStatusLabel
    Friend WithEvents ToolStripMenuItem_DebuggerBreakpoints As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerBreakpointInsert As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerBreakpointRemove As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerBreakpointRemoveAll As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerWatchers As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerWatcherInsert As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerWatcherRemove As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerWatcherRemoveAll As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileSaveAsTemp As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileOpenFolder As ToolStripMenuItem
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents ToolStripMenuItem_FileSaveAll As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_FileLoadTabs As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents Timer_PingFlash As Timer
    Friend WithEvents TabControl_SourceTabs As ClassTabControlColor
    Public WithEvents TabControl_Toolbox As ClassTabControlColor
    Public WithEvents TextEditorControl_Source As TextEditorControlEx
    Public WithEvents MenuStrip_BasicPawn As MenuStrip
    Public WithEvents TabControl_Details As ClassTabControlColor
    Public WithEvents StatusStrip_BasicPawn As StatusStrip
    Friend WithEvents ToolStripSeparator10 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_FileSavePacked As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileStartPage As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_OutlineCollapseAll As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_OutlineExpandAll As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileProjectSaveAs As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileProjectLoad As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileCloseAll As ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabel_Project As ToolStripStatusLabel
    Friend WithEvents TabPage_ProjectBrowser As TabPage
    Friend WithEvents ToolStripMenuItem_FileProjectClose As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpGithub As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_OutlineToggleAll As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator13 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_ToolsConvertTabsSpaces As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator15 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_HelpCheckUpdates As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator14 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_ToolsAutocompleteCurrentMod As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileNewWizard As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ToolsAutocompleteUpdateAll As ToolStripMenuItem
    Friend WithEvents Timer_CheckFiles As Timer
    Friend WithEvents ToolStripMenuItem_ViewToolbox As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ViewDetails As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ViewMinimap As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_File As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Tools As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Build As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Test As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Help As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Undo As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Redo As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_NewUpdate As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_TabClose As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_TabMoveRight As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_TabMoveLeft As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_TabOpenInstance As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_View As ToolStripMenuItem
    Public WithEvents ContextMenuStrip_RightClick As ContextMenuStrip
    Public WithEvents ContextMenuStrip_Tabs As ContextMenuStrip
    Public WithEvents ToolStripMenuItem_Mark As ToolStripMenuItem
    Public WithEvents ToolStripSeparator1 As ToolStripSeparator
    Public WithEvents ToolStripMenuItem_Paste As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Copy As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Cut As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_ListReferences As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Debugger As ToolStripMenuItem
    Public WithEvents ToolStripSeparator6 As ToolStripSeparator
    Public WithEvents ToolStripMenuItem_HightlightCustom As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Delete As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_SelectAll As ToolStripMenuItem
    Public WithEvents ToolStripSeparator11 As ToolStripSeparator
    Public WithEvents ToolStripMenuItem_Outline As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Tabs_Close As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Tabs_CloseAllButThis As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Tabs_CloseAll As ToolStripMenuItem
    Public WithEvents ToolStripSeparator12 As ToolStripSeparator
    Public WithEvents ToolStripMenuItem_Tabs_OpenFolder As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Tabs_Popout As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Comment As ToolStripMenuItem
    Public WithEvents ToolStripSeparator16 As ToolStripSeparator
    Public WithEvents ToolStripMenuItem_Tabs_Cut As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Tabs_Insert As ToolStripMenuItem
    Public WithEvents ContextMenuStrip_Config As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_EditConfigActiveTab As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditConfigAllTabs As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ToolsFormatCodeIndent As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ToolsFormatCodeTrim As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ToolsConvertToSettings As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator17 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_ToolsConvertToTab As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ToolsConvertToSpace As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripTextBox_ToolsConvertSpaceSize As ToolStripTextBox
    Friend WithEvents ToolStripStatusLabel_AutocompleteProgress As ToolStripStatusLabel
    Friend WithEvents ToolStripSeparator18 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_ViewProgressAni As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerAsserts As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerAssertInsert As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerAssertRemove As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DebuggerAssertRemoveAll As ToolStripMenuItem
    Friend WithEvents TabPage_ExplorerBrowser As TabPage
    Friend WithEvents ToolStripSeparator19 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_FindOptimalConfigActiveTab As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FindOptimalConfigAllTabs As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpControlsDetailsSecAction As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_BuildCurrent As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_BuildAll As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_TestCurrent As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_TestAll As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator20 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_TestDebug As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FileClose As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_HelpControlsTabNav As ToolStripMenuItem
    Friend WithEvents TabPage_Bookmarks As TabPage
    Friend WithEvents ToolStripMenuItem_Bookmarks As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_BookmarksAdd As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator21 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_BookmarksShow As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Edit As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditUndo As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditRedo As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator22 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_EditCut As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditCopy As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditPaste As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditDelete As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditSelectAll As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator23 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_BookmarksRemoveLines As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_FindDefinition As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_PeekDefinition As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator24 As ToolStripSeparator
    Friend WithEvents Timer_SyntaxAnimation As Timer
    Friend WithEvents ToolStripMenuItem_FileProjectSave As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditDupLine As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditLineUp As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditLineDown As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditInsertLineUp As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditInsertLineDown As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ShowTips As ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabel_Spacer As ToolStripStatusLabel
    Public WithEvents ToolStripMenuItem_TabLastViewedRight As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_TabLastViewedLeft As ToolStripMenuItem
    Friend WithEvents Timer_AutoSave As Timer
End Class
