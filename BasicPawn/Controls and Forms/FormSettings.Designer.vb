<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormSettings
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSettings))
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.Button_Apply = New System.Windows.Forms.Button()
        Me.ContextMenuStrip_Plugins = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_PluginsRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_OpenUrl = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_PluginsEnable = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_PluginsDisable = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabControl1 = New BasicPawn.ClassTabControlColor()
        Me.TabPage_Settings = New System.Windows.Forms.TabPage()
        Me.ClassTabControlColor1 = New BasicPawn.ClassTabControlColor()
        Me.TabPage_General = New System.Windows.Forms.TabPage()
        Me.CheckBox_AutoHoverScroll = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AutoOpenProjectFiles = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AlwaysNewInstance = New System.Windows.Forms.CheckBox()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.CheckBox_AutoShowStartPage = New System.Windows.Forms.CheckBox()
        Me.Button_ViewErrorLog = New System.Windows.Forms.Button()
        Me.CheckBox_AssociateSourcePawn = New System.Windows.Forms.CheckBox()
        Me.Button_ClearErrorLog = New System.Windows.Forms.Button()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.CheckBox_AssociateAmxMod = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AssociateIncludes = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AssociateBasicPawnProject = New System.Windows.Forms.CheckBox()
        Me.TabPage_Editor = New System.Windows.Forms.TabPage()
        Me.CheckBox_RememberFolds = New System.Windows.Forms.CheckBox()
        Me.LinkLabel_MoreStyles = New System.Windows.Forms.LinkLabel()
        Me.Button_Font = New System.Windows.Forms.Button()
        Me.LinkLabel_DefaultSyntax = New System.Windows.Forms.LinkLabel()
        Me.CheckBox_InvertedColors = New System.Windows.Forms.CheckBox()
        Me.Button_CustomSyntax = New System.Windows.Forms.Button()
        Me.Label_Font = New System.Windows.Forms.Label()
        Me.TextBox_CustomSyntax = New System.Windows.Forms.TextBox()
        Me.CheckBox_TabsToSpace = New System.Windows.Forms.CheckBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.NumericUpDown_TabsToSpaces = New System.Windows.Forms.NumericUpDown()
        Me.TabPage_Syntax = New System.Windows.Forms.TabPage()
        Me.CheckBox_DoubleClickMark = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AutoMark = New System.Windows.Forms.CheckBox()
        Me.TabPage_Autocomplete = New System.Windows.Forms.TabPage()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.RadioButton_VarParseTab = New System.Windows.Forms.RadioButton()
        Me.RadioButton_VarParseTabInc = New System.Windows.Forms.RadioButton()
        Me.RadioButton_VarParseAll = New System.Windows.Forms.RadioButton()
        Me.CheckBox_AutoIndentBrackets = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AlwaysLoadDefaultIncludes = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AutoCloseStrings = New System.Windows.Forms.CheckBox()
        Me.CheckBox_CaseSensitive = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AutoCloseBrackets = New System.Windows.Forms.CheckBox()
        Me.CheckBox_WindowsToolTipPopup = New System.Windows.Forms.CheckBox()
        Me.CheckBox_OnlyUpdateSyntaxWhenFocused = New System.Windows.Forms.CheckBox()
        Me.CheckBox_FullAutocompleteReTagging = New System.Windows.Forms.CheckBox()
        Me.CheckBox_WindowsToolTipNewlineMethods = New System.Windows.Forms.CheckBox()
        Me.CheckBox_CommentsAutocompleteIntelliSense = New System.Windows.Forms.CheckBox()
        Me.CheckBox_WindowsToolTipAnimations = New System.Windows.Forms.CheckBox()
        Me.CheckBox_CommentsMethodIntelliSense = New System.Windows.Forms.CheckBox()
        Me.CheckBox_FullAutcompleteMethods = New System.Windows.Forms.CheckBox()
        Me.CheckBox_SwitchTabToAutocomplete = New System.Windows.Forms.CheckBox()
        Me.CheckBox_OnScreenIntelliSense = New System.Windows.Forms.CheckBox()
        Me.CheckBox_VarAutocompleteShowObjectBrowser = New System.Windows.Forms.CheckBox()
        Me.TabPage_Configs = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel17 = New System.Windows.Forms.Panel()
        Me.Button_SaveConfig = New System.Windows.Forms.Button()
        Me.Label_ConfigName = New System.Windows.Forms.Label()
        Me.Button_ConfigRename = New System.Windows.Forms.Button()
        Me.TextBox_ConfigName = New System.Windows.Forms.TextBox()
        Me.Button_ConfigCopy = New System.Windows.Forms.Button()
        Me.Button_ConfigRemove = New System.Windows.Forms.Button()
        Me.Button_ConfigAdd = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.GroupBox_ConfigSettings = New System.Windows.Forms.GroupBox()
        Me.Panel11 = New System.Windows.Forms.Panel()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Panel13 = New System.Windows.Forms.Panel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TextBox_Shell = New System.Windows.Forms.TextBox()
        Me.LinkLabel_ShowShellArguments = New System.Windows.Forms.LinkLabel()
        Me.Panel10 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox_ClientFolder = New System.Windows.Forms.TextBox()
        Me.Button_ClientFolder = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel12 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox_ServerFolder = New System.Windows.Forms.TextBox()
        Me.Button_ServerFolder = New System.Windows.Forms.Button()
        Me.TextBox_SourceModFolder = New System.Windows.Forms.TextBox()
        Me.Button_SourceModFolder = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Panel9 = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.ListView_KnownFiles = New System.Windows.Forms.ListView()
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Button_KnownFileAdd = New System.Windows.Forms.Button()
        Me.Button_KnownFileRemove = New System.Windows.Forms.Button()
        Me.TextBox_AutoAssignPaths = New System.Windows.Forms.TextBox()
        Me.Button_AutoAssignPaths = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.TabControl2 = New BasicPawn.ClassTabControlColor()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.TextBoxEx_CODefineConstantsSP = New BasicPawn.ClassTextboxWatermark()
        Me.TextBoxEx_COIgnoredWarningsSP = New BasicPawn.ClassTextboxWatermark()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.ComboBox_COTreatWarningsAsErrorsSP = New System.Windows.Forms.ComboBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.ComboBox_COVerbosityLevelSP = New System.Windows.Forms.ComboBox()
        Me.ComboBox_COOptimizationLevelSP = New System.Windows.Forms.ComboBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.ComboBox_COSymbolicInformationAMXX = New System.Windows.Forms.ComboBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.TextBoxEx_CODefineConstantsAMXX = New BasicPawn.ClassTextboxWatermark()
        Me.TextBoxEx_COIgnoredWarningsAMXX = New BasicPawn.ClassTextboxWatermark()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.ComboBox_COTreatWarningsAsErrorsAMXX = New System.Windows.Forms.ComboBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.ComboBox_COVerbosityLevelAMXX = New System.Windows.Forms.ComboBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.ComboBox_Language = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.RadioButton_ConfigSettingAutomatic = New System.Windows.Forms.RadioButton()
        Me.RadioButton_ConfigSettingManual = New System.Windows.Forms.RadioButton()
        Me.CheckBox_ConfigIsDefault = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox_CompilerPath = New System.Windows.Forms.TextBox()
        Me.Button_Compiler = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBox_IncludeFolder = New System.Windows.Forms.TextBox()
        Me.Button_IncludeFolder = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TextBox_OutputFolder = New System.Windows.Forms.TextBox()
        Me.Button_OutputFolder = New System.Windows.Forms.Button()
        Me.ListBox_Configs = New System.Windows.Forms.ListBox()
        Me.TabPage_Plugins = New System.Windows.Forms.TabPage()
        Me.LinkLabel_MorePlugins = New System.Windows.Forms.LinkLabel()
        Me.ListView_Plugins = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabPage_Database = New System.Windows.Forms.TabPage()
        Me.DatabaseListBox_Database = New BasicPawn.ClassDatabaseListBox()
        Me.Button_Refresh = New System.Windows.Forms.Button()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.ClassPictureBoxQuality2 = New BasicPawn.ClassPictureBoxQuality()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Button_AddDatabaseItem = New System.Windows.Forms.Button()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.ClassPictureBoxQuality1 = New BasicPawn.ClassPictureBoxQuality()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.CheckBox_WindowsToolTipDisplayTop = New System.Windows.Forms.CheckBox()
        Me.ContextMenuStrip_Plugins.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage_Settings.SuspendLayout()
        Me.ClassTabControlColor1.SuspendLayout()
        Me.TabPage_General.SuspendLayout()
        Me.TabPage_Editor.SuspendLayout()
        CType(Me.NumericUpDown_TabsToSpaces, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage_Syntax.SuspendLayout()
        Me.TabPage_Autocomplete.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.TabPage_Configs.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.Panel17.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox_ConfigSettings.SuspendLayout()
        Me.Panel11.SuspendLayout()
        Me.Panel10.SuspendLayout()
        Me.Panel9.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage_Plugins.SuspendLayout()
        Me.TabPage_Database.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        CType(Me.ClassPictureBoxQuality2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel3.SuspendLayout()
        CType(Me.ClassPictureBoxQuality1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Cancel.Location = New System.Drawing.Point(686, 526)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(86, 23)
        Me.Button_Cancel.TabIndex = 1
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Button_Apply
        '
        Me.Button_Apply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Apply.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button_Apply.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Apply.Location = New System.Drawing.Point(594, 526)
        Me.Button_Apply.Name = "Button_Apply"
        Me.Button_Apply.Size = New System.Drawing.Size(86, 23)
        Me.Button_Apply.TabIndex = 2
        Me.Button_Apply.Text = "Apply"
        Me.Button_Apply.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip_Plugins
        '
        Me.ContextMenuStrip_Plugins.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_PluginsRefresh, Me.ToolStripSeparator1, Me.ToolStripMenuItem_OpenUrl, Me.ToolStripSeparator2, Me.ToolStripMenuItem_PluginsEnable, Me.ToolStripMenuItem_PluginsDisable})
        Me.ContextMenuStrip_Plugins.Name = "ContextMenuStrip_Plugins"
        Me.ContextMenuStrip_Plugins.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_Plugins.Size = New System.Drawing.Size(128, 104)
        '
        'ToolStripMenuItem_PluginsRefresh
        '
        Me.ToolStripMenuItem_PluginsRefresh.Image = Global.BasicPawn.My.Resources.Resources.shell32_16739_16x16_32
        Me.ToolStripMenuItem_PluginsRefresh.Name = "ToolStripMenuItem_PluginsRefresh"
        Me.ToolStripMenuItem_PluginsRefresh.Size = New System.Drawing.Size(127, 22)
        Me.ToolStripMenuItem_PluginsRefresh.Text = "Refresh"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(124, 6)
        '
        'ToolStripMenuItem_OpenUrl
        '
        Me.ToolStripMenuItem_OpenUrl.Image = Global.BasicPawn.My.Resources.Resources.imageres_5316_16x16
        Me.ToolStripMenuItem_OpenUrl.Name = "ToolStripMenuItem_OpenUrl"
        Me.ToolStripMenuItem_OpenUrl.Size = New System.Drawing.Size(127, 22)
        Me.ToolStripMenuItem_OpenUrl.Text = "Open URL"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(124, 6)
        '
        'ToolStripMenuItem_PluginsEnable
        '
        Me.ToolStripMenuItem_PluginsEnable.Name = "ToolStripMenuItem_PluginsEnable"
        Me.ToolStripMenuItem_PluginsEnable.Size = New System.Drawing.Size(127, 22)
        Me.ToolStripMenuItem_PluginsEnable.Text = "Enable"
        '
        'ToolStripMenuItem_PluginsDisable
        '
        Me.ToolStripMenuItem_PluginsDisable.Name = "ToolStripMenuItem_PluginsDisable"
        Me.ToolStripMenuItem_PluginsDisable.Size = New System.Drawing.Size(127, 22)
        Me.ToolStripMenuItem_PluginsDisable.Text = "Disable"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage_Settings)
        Me.TabControl1.Controls.Add(Me.TabPage_Configs)
        Me.TabControl1.Controls.Add(Me.TabPage_Plugins)
        Me.TabControl1.Controls.Add(Me.TabPage_Database)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(760, 508)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage_Settings
        '
        Me.TabPage_Settings.Controls.Add(Me.ClassTabControlColor1)
        Me.TabPage_Settings.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Settings.Name = "TabPage_Settings"
        Me.TabPage_Settings.Size = New System.Drawing.Size(752, 482)
        Me.TabPage_Settings.TabIndex = 0
        Me.TabPage_Settings.Text = "Settings"
        Me.TabPage_Settings.UseVisualStyleBackColor = True
        '
        'ClassTabControlColor1
        '
        Me.ClassTabControlColor1.Controls.Add(Me.TabPage_General)
        Me.ClassTabControlColor1.Controls.Add(Me.TabPage_Editor)
        Me.ClassTabControlColor1.Controls.Add(Me.TabPage_Syntax)
        Me.ClassTabControlColor1.Controls.Add(Me.TabPage_Autocomplete)
        Me.ClassTabControlColor1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ClassTabControlColor1.Location = New System.Drawing.Point(0, 0)
        Me.ClassTabControlColor1.Multiline = True
        Me.ClassTabControlColor1.Name = "ClassTabControlColor1"
        Me.ClassTabControlColor1.SelectedIndex = 0
        Me.ClassTabControlColor1.Size = New System.Drawing.Size(752, 482)
        Me.ClassTabControlColor1.TabIndex = 27
        '
        'TabPage_General
        '
        Me.TabPage_General.AutoScroll = True
        Me.TabPage_General.Controls.Add(Me.CheckBox_AutoHoverScroll)
        Me.TabPage_General.Controls.Add(Me.CheckBox_AutoOpenProjectFiles)
        Me.TabPage_General.Controls.Add(Me.CheckBox_AlwaysNewInstance)
        Me.TabPage_General.Controls.Add(Me.Label30)
        Me.TabPage_General.Controls.Add(Me.CheckBox_AutoShowStartPage)
        Me.TabPage_General.Controls.Add(Me.Button_ViewErrorLog)
        Me.TabPage_General.Controls.Add(Me.CheckBox_AssociateSourcePawn)
        Me.TabPage_General.Controls.Add(Me.Button_ClearErrorLog)
        Me.TabPage_General.Controls.Add(Me.Label19)
        Me.TabPage_General.Controls.Add(Me.CheckBox_AssociateAmxMod)
        Me.TabPage_General.Controls.Add(Me.CheckBox_AssociateIncludes)
        Me.TabPage_General.Controls.Add(Me.CheckBox_AssociateBasicPawnProject)
        Me.TabPage_General.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_General.Name = "TabPage_General"
        Me.TabPage_General.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_General.Size = New System.Drawing.Size(744, 456)
        Me.TabPage_General.TabIndex = 0
        Me.TabPage_General.Text = "General"
        Me.TabPage_General.UseVisualStyleBackColor = True
        '
        'CheckBox_AutoHoverScroll
        '
        Me.CheckBox_AutoHoverScroll.AutoSize = True
        Me.CheckBox_AutoHoverScroll.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AutoHoverScroll.Location = New System.Drawing.Point(6, 75)
        Me.CheckBox_AutoHoverScroll.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AutoHoverScroll.Name = "CheckBox_AutoHoverScroll"
        Me.CheckBox_AutoHoverScroll.Size = New System.Drawing.Size(191, 18)
        Me.CheckBox_AutoHoverScroll.TabIndex = 30
        Me.CheckBox_AutoHoverScroll.Text = "Scroll controls on mouse hover"
        Me.CheckBox_AutoHoverScroll.UseVisualStyleBackColor = True
        '
        'CheckBox_AutoOpenProjectFiles
        '
        Me.CheckBox_AutoOpenProjectFiles.AutoSize = True
        Me.CheckBox_AutoOpenProjectFiles.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AutoOpenProjectFiles.Location = New System.Drawing.Point(6, 51)
        Me.CheckBox_AutoOpenProjectFiles.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AutoOpenProjectFiles.Name = "CheckBox_AutoOpenProjectFiles"
        Me.CheckBox_AutoOpenProjectFiles.Size = New System.Drawing.Size(336, 18)
        Me.CheckBox_AutoOpenProjectFiles.TabIndex = 29
        Me.CheckBox_AutoOpenProjectFiles.Text = "Automatically open all project files when opening a project"
        Me.CheckBox_AutoOpenProjectFiles.UseVisualStyleBackColor = True
        '
        'CheckBox_AlwaysNewInstance
        '
        Me.CheckBox_AlwaysNewInstance.AutoSize = True
        Me.CheckBox_AlwaysNewInstance.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AlwaysNewInstance.Location = New System.Drawing.Point(6, 3)
        Me.CheckBox_AlwaysNewInstance.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AlwaysNewInstance.Name = "CheckBox_AlwaysNewInstance"
        Me.CheckBox_AlwaysNewInstance.Size = New System.Drawing.Size(248, 18)
        Me.CheckBox_AlwaysNewInstance.TabIndex = 19
        Me.CheckBox_AlwaysNewInstance.Text = "Always open new instance instead of tabs"
        Me.CheckBox_AlwaysNewInstance.UseVisualStyleBackColor = True
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(6, 219)
        Me.Label30.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(147, 13)
        Me.Label30.TabIndex = 28
        Me.Label30.Text = "Application exception logs:"
        '
        'CheckBox_AutoShowStartPage
        '
        Me.CheckBox_AutoShowStartPage.AutoSize = True
        Me.CheckBox_AutoShowStartPage.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AutoShowStartPage.Location = New System.Drawing.Point(6, 27)
        Me.CheckBox_AutoShowStartPage.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AutoShowStartPage.Name = "CheckBox_AutoShowStartPage"
        Me.CheckBox_AutoShowStartPage.Size = New System.Drawing.Size(235, 18)
        Me.CheckBox_AutoShowStartPage.TabIndex = 20
        Me.CheckBox_AutoShowStartPage.Text = "Show StartPage when no file is opened"
        Me.CheckBox_AutoShowStartPage.UseVisualStyleBackColor = True
        '
        'Button_ViewErrorLog
        '
        Me.Button_ViewErrorLog.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ViewErrorLog.Location = New System.Drawing.Point(315, 214)
        Me.Button_ViewErrorLog.Name = "Button_ViewErrorLog"
        Me.Button_ViewErrorLog.Size = New System.Drawing.Size(150, 23)
        Me.Button_ViewErrorLog.TabIndex = 27
        Me.Button_ViewErrorLog.Text = "View log"
        Me.Button_ViewErrorLog.UseVisualStyleBackColor = True
        '
        'CheckBox_AssociateSourcePawn
        '
        Me.CheckBox_AssociateSourcePawn.AutoSize = True
        Me.CheckBox_AssociateSourcePawn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AssociateSourcePawn.Location = New System.Drawing.Point(32, 118)
        Me.CheckBox_AssociateSourcePawn.Margin = New System.Windows.Forms.Padding(32, 3, 3, 3)
        Me.CheckBox_AssociateSourcePawn.Name = "CheckBox_AssociateSourcePawn"
        Me.CheckBox_AssociateSourcePawn.Size = New System.Drawing.Size(148, 18)
        Me.CheckBox_AssociateSourcePawn.TabIndex = 21
        Me.CheckBox_AssociateSourcePawn.Text = "SourcePawn files (*.sp)"
        Me.CheckBox_AssociateSourcePawn.UseVisualStyleBackColor = True
        '
        'Button_ClearErrorLog
        '
        Me.Button_ClearErrorLog.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ClearErrorLog.Location = New System.Drawing.Point(159, 214)
        Me.Button_ClearErrorLog.Name = "Button_ClearErrorLog"
        Me.Button_ClearErrorLog.Size = New System.Drawing.Size(150, 23)
        Me.Button_ClearErrorLog.TabIndex = 26
        Me.Button_ClearErrorLog.Text = "Clear log (Empty)"
        Me.Button_ClearErrorLog.UseVisualStyleBackColor = True
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(6, 99)
        Me.Label19.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(141, 13)
        Me.Label19.TabIndex = 22
        Me.Label19.Text = "Associate BasicPawn with:"
        '
        'CheckBox_AssociateAmxMod
        '
        Me.CheckBox_AssociateAmxMod.AutoSize = True
        Me.CheckBox_AssociateAmxMod.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AssociateAmxMod.Location = New System.Drawing.Point(32, 142)
        Me.CheckBox_AssociateAmxMod.Margin = New System.Windows.Forms.Padding(32, 3, 3, 3)
        Me.CheckBox_AssociateAmxMod.Name = "CheckBox_AssociateAmxMod"
        Me.CheckBox_AssociateAmxMod.Size = New System.Drawing.Size(152, 18)
        Me.CheckBox_AssociateAmxMod.TabIndex = 25
        Me.CheckBox_AssociateAmxMod.Text = "AMX Mod X files (*.sma)"
        Me.CheckBox_AssociateAmxMod.UseVisualStyleBackColor = True
        '
        'CheckBox_AssociateIncludes
        '
        Me.CheckBox_AssociateIncludes.AutoSize = True
        Me.CheckBox_AssociateIncludes.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AssociateIncludes.Location = New System.Drawing.Point(32, 166)
        Me.CheckBox_AssociateIncludes.Margin = New System.Windows.Forms.Padding(32, 3, 3, 3)
        Me.CheckBox_AssociateIncludes.Name = "CheckBox_AssociateIncludes"
        Me.CheckBox_AssociateIncludes.Size = New System.Drawing.Size(126, 18)
        Me.CheckBox_AssociateIncludes.TabIndex = 23
        Me.CheckBox_AssociateIncludes.Text = "Include files (*.inc)"
        Me.CheckBox_AssociateIncludes.UseVisualStyleBackColor = True
        '
        'CheckBox_AssociateBasicPawnProject
        '
        Me.CheckBox_AssociateBasicPawnProject.AutoSize = True
        Me.CheckBox_AssociateBasicPawnProject.Checked = True
        Me.CheckBox_AssociateBasicPawnProject.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox_AssociateBasicPawnProject.Enabled = False
        Me.CheckBox_AssociateBasicPawnProject.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AssociateBasicPawnProject.Location = New System.Drawing.Point(32, 190)
        Me.CheckBox_AssociateBasicPawnProject.Margin = New System.Windows.Forms.Padding(32, 3, 3, 3)
        Me.CheckBox_AssociateBasicPawnProject.Name = "CheckBox_AssociateBasicPawnProject"
        Me.CheckBox_AssociateBasicPawnProject.Size = New System.Drawing.Size(200, 18)
        Me.CheckBox_AssociateBasicPawnProject.TabIndex = 24
        Me.CheckBox_AssociateBasicPawnProject.Text = "BasicPawn Project files (*.bpproj)"
        Me.CheckBox_AssociateBasicPawnProject.UseVisualStyleBackColor = True
        '
        'TabPage_Editor
        '
        Me.TabPage_Editor.AutoScroll = True
        Me.TabPage_Editor.Controls.Add(Me.CheckBox_RememberFolds)
        Me.TabPage_Editor.Controls.Add(Me.LinkLabel_MoreStyles)
        Me.TabPage_Editor.Controls.Add(Me.Button_Font)
        Me.TabPage_Editor.Controls.Add(Me.LinkLabel_DefaultSyntax)
        Me.TabPage_Editor.Controls.Add(Me.CheckBox_InvertedColors)
        Me.TabPage_Editor.Controls.Add(Me.Button_CustomSyntax)
        Me.TabPage_Editor.Controls.Add(Me.Label_Font)
        Me.TabPage_Editor.Controls.Add(Me.TextBox_CustomSyntax)
        Me.TabPage_Editor.Controls.Add(Me.CheckBox_TabsToSpace)
        Me.TabPage_Editor.Controls.Add(Me.Label32)
        Me.TabPage_Editor.Controls.Add(Me.NumericUpDown_TabsToSpaces)
        Me.TabPage_Editor.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Editor.Name = "TabPage_Editor"
        Me.TabPage_Editor.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Editor.Size = New System.Drawing.Size(744, 456)
        Me.TabPage_Editor.TabIndex = 1
        Me.TabPage_Editor.Text = "TextEditor"
        Me.TabPage_Editor.UseVisualStyleBackColor = True
        '
        'CheckBox_RememberFolds
        '
        Me.CheckBox_RememberFolds.AutoSize = True
        Me.CheckBox_RememberFolds.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_RememberFolds.Location = New System.Drawing.Point(6, 140)
        Me.CheckBox_RememberFolds.Name = "CheckBox_RememberFolds"
        Me.CheckBox_RememberFolds.Size = New System.Drawing.Size(132, 18)
        Me.CheckBox_RememberFolds.TabIndex = 23
        Me.CheckBox_RememberFolds.Text = "Remember foldings"
        Me.CheckBox_RememberFolds.UseVisualStyleBackColor = True
        '
        'LinkLabel_MoreStyles
        '
        Me.LinkLabel_MoreStyles.AutoSize = True
        Me.LinkLabel_MoreStyles.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel_MoreStyles.Location = New System.Drawing.Point(163, 121)
        Me.LinkLabel_MoreStyles.Margin = New System.Windows.Forms.Padding(6, 0, 3, 3)
        Me.LinkLabel_MoreStyles.Name = "LinkLabel_MoreStyles"
        Me.LinkLabel_MoreStyles.Size = New System.Drawing.Size(120, 13)
        Me.LinkLabel_MoreStyles.TabIndex = 22
        Me.LinkLabel_MoreStyles.TabStop = True
        Me.LinkLabel_MoreStyles.Text = "Get more syntax styles"
        '
        'Button_Font
        '
        Me.Button_Font.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Font.Location = New System.Drawing.Point(6, 3)
        Me.Button_Font.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.Button_Font.Name = "Button_Font"
        Me.Button_Font.Size = New System.Drawing.Size(75, 23)
        Me.Button_Font.TabIndex = 11
        Me.Button_Font.Text = "Font"
        Me.Button_Font.UseVisualStyleBackColor = True
        '
        'LinkLabel_DefaultSyntax
        '
        Me.LinkLabel_DefaultSyntax.AutoSize = True
        Me.LinkLabel_DefaultSyntax.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel_DefaultSyntax.Location = New System.Drawing.Point(6, 121)
        Me.LinkLabel_DefaultSyntax.Margin = New System.Windows.Forms.Padding(6, 0, 3, 3)
        Me.LinkLabel_DefaultSyntax.Name = "LinkLabel_DefaultSyntax"
        Me.LinkLabel_DefaultSyntax.Size = New System.Drawing.Size(148, 13)
        Me.LinkLabel_DefaultSyntax.TabIndex = 21
        Me.LinkLabel_DefaultSyntax.TabStop = True
        Me.LinkLabel_DefaultSyntax.Text = "Default syntax highlighting"
        '
        'CheckBox_InvertedColors
        '
        Me.CheckBox_InvertedColors.AutoSize = True
        Me.CheckBox_InvertedColors.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_InvertedColors.Location = New System.Drawing.Point(6, 32)
        Me.CheckBox_InvertedColors.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_InvertedColors.Name = "CheckBox_InvertedColors"
        Me.CheckBox_InvertedColors.Size = New System.Drawing.Size(134, 18)
        Me.CheckBox_InvertedColors.TabIndex = 13
        Me.CheckBox_InvertedColors.Text = "High contrast mode"
        Me.CheckBox_InvertedColors.UseVisualStyleBackColor = True
        '
        'Button_CustomSyntax
        '
        Me.Button_CustomSyntax.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_CustomSyntax.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_CustomSyntax.Location = New System.Drawing.Point(703, 96)
        Me.Button_CustomSyntax.Name = "Button_CustomSyntax"
        Me.Button_CustomSyntax.Size = New System.Drawing.Size(32, 23)
        Me.Button_CustomSyntax.TabIndex = 20
        Me.Button_CustomSyntax.Text = "..."
        Me.Button_CustomSyntax.UseVisualStyleBackColor = True
        '
        'Label_Font
        '
        Me.Label_Font.AutoSize = True
        Me.Label_Font.Location = New System.Drawing.Point(90, 8)
        Me.Label_Font.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
        Me.Label_Font.Name = "Label_Font"
        Me.Label_Font.Size = New System.Drawing.Size(73, 13)
        Me.Label_Font.TabIndex = 12
        Me.Label_Font.Text = "Current Font"
        '
        'TextBox_CustomSyntax
        '
        Me.TextBox_CustomSyntax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_CustomSyntax.Location = New System.Drawing.Point(6, 96)
        Me.TextBox_CustomSyntax.Name = "TextBox_CustomSyntax"
        Me.TextBox_CustomSyntax.Size = New System.Drawing.Size(691, 22)
        Me.TextBox_CustomSyntax.TabIndex = 19
        '
        'CheckBox_TabsToSpace
        '
        Me.CheckBox_TabsToSpace.AutoSize = True
        Me.CheckBox_TabsToSpace.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_TabsToSpace.Location = New System.Drawing.Point(6, 56)
        Me.CheckBox_TabsToSpace.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_TabsToSpace.Name = "CheckBox_TabsToSpace"
        Me.CheckBox_TabsToSpace.Size = New System.Drawing.Size(151, 18)
        Me.CheckBox_TabsToSpace.TabIndex = 14
        Me.CheckBox_TabsToSpace.Text = "Convert tabs to spaces:"
        Me.CheckBox_TabsToSpace.UseVisualStyleBackColor = True
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(6, 80)
        Me.Label32.Margin = New System.Windows.Forms.Padding(6, 3, 3, 0)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(162, 13)
        Me.Label32.TabIndex = 18
        Me.Label32.Text = "Custom syntax highlight path:"
        '
        'NumericUpDown_TabsToSpaces
        '
        Me.NumericUpDown_TabsToSpaces.Location = New System.Drawing.Point(163, 52)
        Me.NumericUpDown_TabsToSpaces.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown_TabsToSpaces.Name = "NumericUpDown_TabsToSpaces"
        Me.NumericUpDown_TabsToSpaces.Size = New System.Drawing.Size(42, 22)
        Me.NumericUpDown_TabsToSpaces.TabIndex = 15
        Me.NumericUpDown_TabsToSpaces.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'TabPage_Syntax
        '
        Me.TabPage_Syntax.AutoScroll = True
        Me.TabPage_Syntax.Controls.Add(Me.CheckBox_DoubleClickMark)
        Me.TabPage_Syntax.Controls.Add(Me.CheckBox_AutoMark)
        Me.TabPage_Syntax.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Syntax.Name = "TabPage_Syntax"
        Me.TabPage_Syntax.Size = New System.Drawing.Size(744, 456)
        Me.TabPage_Syntax.TabIndex = 2
        Me.TabPage_Syntax.Text = "Syntax Highlighting"
        Me.TabPage_Syntax.UseVisualStyleBackColor = True
        '
        'CheckBox_DoubleClickMark
        '
        Me.CheckBox_DoubleClickMark.AutoSize = True
        Me.CheckBox_DoubleClickMark.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_DoubleClickMark.Location = New System.Drawing.Point(6, 3)
        Me.CheckBox_DoubleClickMark.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_DoubleClickMark.Name = "CheckBox_DoubleClickMark"
        Me.CheckBox_DoubleClickMark.Size = New System.Drawing.Size(190, 18)
        Me.CheckBox_DoubleClickMark.TabIndex = 5
        Me.CheckBox_DoubleClickMark.Text = "Mark words using double click"
        Me.CheckBox_DoubleClickMark.UseVisualStyleBackColor = True
        '
        'CheckBox_AutoMark
        '
        Me.CheckBox_AutoMark.AutoSize = True
        Me.CheckBox_AutoMark.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AutoMark.Location = New System.Drawing.Point(6, 27)
        Me.CheckBox_AutoMark.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AutoMark.Name = "CheckBox_AutoMark"
        Me.CheckBox_AutoMark.Size = New System.Drawing.Size(164, 18)
        Me.CheckBox_AutoMark.TabIndex = 17
        Me.CheckBox_AutoMark.Text = "Automatically mark words"
        Me.CheckBox_AutoMark.UseVisualStyleBackColor = True
        '
        'TabPage_Autocomplete
        '
        Me.TabPage_Autocomplete.AutoScroll = True
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_WindowsToolTipDisplayTop)
        Me.TabPage_Autocomplete.Controls.Add(Me.GroupBox4)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_AutoIndentBrackets)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_AlwaysLoadDefaultIncludes)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_AutoCloseStrings)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_CaseSensitive)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_AutoCloseBrackets)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_WindowsToolTipPopup)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_OnlyUpdateSyntaxWhenFocused)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_FullAutocompleteReTagging)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_WindowsToolTipNewlineMethods)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_CommentsAutocompleteIntelliSense)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_WindowsToolTipAnimations)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_CommentsMethodIntelliSense)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_FullAutcompleteMethods)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_SwitchTabToAutocomplete)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_OnScreenIntelliSense)
        Me.TabPage_Autocomplete.Controls.Add(Me.CheckBox_VarAutocompleteShowObjectBrowser)
        Me.TabPage_Autocomplete.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Autocomplete.Name = "TabPage_Autocomplete"
        Me.TabPage_Autocomplete.Size = New System.Drawing.Size(744, 456)
        Me.TabPage_Autocomplete.TabIndex = 3
        Me.TabPage_Autocomplete.Text = "Autocomplete & IntelliSense"
        Me.TabPage_Autocomplete.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.RadioButton_VarParseTab)
        Me.GroupBox4.Controls.Add(Me.RadioButton_VarParseTabInc)
        Me.GroupBox4.Controls.Add(Me.RadioButton_VarParseAll)
        Me.GroupBox4.Location = New System.Drawing.Point(6, 411)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(459, 92)
        Me.GroupBox4.TabIndex = 27
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Variable parsing behaviour"
        '
        'RadioButton_VarParseTab
        '
        Me.RadioButton_VarParseTab.AutoSize = True
        Me.RadioButton_VarParseTab.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RadioButton_VarParseTab.Location = New System.Drawing.Point(6, 67)
        Me.RadioButton_VarParseTab.Name = "RadioButton_VarParseTab"
        Me.RadioButton_VarParseTab.Size = New System.Drawing.Size(192, 18)
        Me.RadioButton_VarParseTab.TabIndex = 2
        Me.RadioButton_VarParseTab.Text = "Parse from active tab only (Fast)"
        Me.RadioButton_VarParseTab.UseVisualStyleBackColor = True
        '
        'RadioButton_VarParseTabInc
        '
        Me.RadioButton_VarParseTabInc.AutoSize = True
        Me.RadioButton_VarParseTabInc.Checked = True
        Me.RadioButton_VarParseTabInc.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RadioButton_VarParseTabInc.Location = New System.Drawing.Point(6, 44)
        Me.RadioButton_VarParseTabInc.Name = "RadioButton_VarParseTabInc"
        Me.RadioButton_VarParseTabInc.Size = New System.Drawing.Size(407, 18)
        Me.RadioButton_VarParseTabInc.TabIndex = 1
        Me.RadioButton_VarParseTabInc.TabStop = True
        Me.RadioButton_VarParseTabInc.Text = "Parse from active tab and all source includes (*.sp, * .sma, *.p, *.pwn) only"
        Me.RadioButton_VarParseTabInc.UseVisualStyleBackColor = True
        '
        'RadioButton_VarParseAll
        '
        Me.RadioButton_VarParseAll.AutoSize = True
        Me.RadioButton_VarParseAll.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RadioButton_VarParseAll.Location = New System.Drawing.Point(6, 21)
        Me.RadioButton_VarParseAll.Name = "RadioButton_VarParseAll"
        Me.RadioButton_VarParseAll.Size = New System.Drawing.Size(180, 18)
        Me.RadioButton_VarParseAll.TabIndex = 0
        Me.RadioButton_VarParseAll.Text = "Parse from all includes (Slow)"
        Me.RadioButton_VarParseAll.UseVisualStyleBackColor = True
        '
        'CheckBox_AutoIndentBrackets
        '
        Me.CheckBox_AutoIndentBrackets.AutoSize = True
        Me.CheckBox_AutoIndentBrackets.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AutoIndentBrackets.Location = New System.Drawing.Point(6, 387)
        Me.CheckBox_AutoIndentBrackets.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AutoIndentBrackets.Name = "CheckBox_AutoIndentBrackets"
        Me.CheckBox_AutoIndentBrackets.Size = New System.Drawing.Size(236, 18)
        Me.CheckBox_AutoIndentBrackets.TabIndex = 26
        Me.CheckBox_AutoIndentBrackets.Text = "Automatically indent brackets on return"
        Me.CheckBox_AutoIndentBrackets.UseVisualStyleBackColor = True
        '
        'CheckBox_AlwaysLoadDefaultIncludes
        '
        Me.CheckBox_AlwaysLoadDefaultIncludes.AutoSize = True
        Me.CheckBox_AlwaysLoadDefaultIncludes.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AlwaysLoadDefaultIncludes.Location = New System.Drawing.Point(6, 3)
        Me.CheckBox_AlwaysLoadDefaultIncludes.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AlwaysLoadDefaultIncludes.Name = "CheckBox_AlwaysLoadDefaultIncludes"
        Me.CheckBox_AlwaysLoadDefaultIncludes.Size = New System.Drawing.Size(198, 18)
        Me.CheckBox_AlwaysLoadDefaultIncludes.TabIndex = 19
        Me.CheckBox_AlwaysLoadDefaultIncludes.Text = "Always load default include files"
        Me.CheckBox_AlwaysLoadDefaultIncludes.UseVisualStyleBackColor = True
        '
        'CheckBox_AutoCloseStrings
        '
        Me.CheckBox_AutoCloseStrings.AutoSize = True
        Me.CheckBox_AutoCloseStrings.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AutoCloseStrings.Location = New System.Drawing.Point(6, 363)
        Me.CheckBox_AutoCloseStrings.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AutoCloseStrings.Name = "CheckBox_AutoCloseStrings"
        Me.CheckBox_AutoCloseStrings.Size = New System.Drawing.Size(239, 18)
        Me.CheckBox_AutoCloseStrings.TabIndex = 25
        Me.CheckBox_AutoCloseStrings.Text = "Automatically close strings while writing"
        Me.CheckBox_AutoCloseStrings.UseVisualStyleBackColor = True
        '
        'CheckBox_CaseSensitive
        '
        Me.CheckBox_CaseSensitive.AutoSize = True
        Me.CheckBox_CaseSensitive.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_CaseSensitive.Location = New System.Drawing.Point(6, 243)
        Me.CheckBox_CaseSensitive.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_CaseSensitive.Name = "CheckBox_CaseSensitive"
        Me.CheckBox_CaseSensitive.Size = New System.Drawing.Size(103, 18)
        Me.CheckBox_CaseSensitive.TabIndex = 15
        Me.CheckBox_CaseSensitive.Text = "Case sensitive"
        Me.CheckBox_CaseSensitive.UseVisualStyleBackColor = True
        '
        'CheckBox_AutoCloseBrackets
        '
        Me.CheckBox_AutoCloseBrackets.AutoSize = True
        Me.CheckBox_AutoCloseBrackets.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AutoCloseBrackets.Location = New System.Drawing.Point(6, 339)
        Me.CheckBox_AutoCloseBrackets.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AutoCloseBrackets.Name = "CheckBox_AutoCloseBrackets"
        Me.CheckBox_AutoCloseBrackets.Size = New System.Drawing.Size(247, 18)
        Me.CheckBox_AutoCloseBrackets.TabIndex = 24
        Me.CheckBox_AutoCloseBrackets.Text = "Automatically close brackets while writing"
        Me.CheckBox_AutoCloseBrackets.UseVisualStyleBackColor = True
        '
        'CheckBox_WindowsToolTipPopup
        '
        Me.CheckBox_WindowsToolTipPopup.AutoSize = True
        Me.CheckBox_WindowsToolTipPopup.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_WindowsToolTipPopup.Location = New System.Drawing.Point(32, 99)
        Me.CheckBox_WindowsToolTipPopup.Margin = New System.Windows.Forms.Padding(32, 3, 3, 3)
        Me.CheckBox_WindowsToolTipPopup.Name = "CheckBox_WindowsToolTipPopup"
        Me.CheckBox_WindowsToolTipPopup.Size = New System.Drawing.Size(127, 18)
        Me.CheckBox_WindowsToolTipPopup.TabIndex = 16
        Me.CheckBox_WindowsToolTipPopup.Text = "Use tooltip popup"
        Me.CheckBox_WindowsToolTipPopup.UseVisualStyleBackColor = True
        '
        'CheckBox_OnlyUpdateSyntaxWhenFocused
        '
        Me.CheckBox_OnlyUpdateSyntaxWhenFocused.AutoSize = True
        Me.CheckBox_OnlyUpdateSyntaxWhenFocused.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_OnlyUpdateSyntaxWhenFocused.Location = New System.Drawing.Point(6, 315)
        Me.CheckBox_OnlyUpdateSyntaxWhenFocused.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_OnlyUpdateSyntaxWhenFocused.Name = "CheckBox_OnlyUpdateSyntaxWhenFocused"
        Me.CheckBox_OnlyUpdateSyntaxWhenFocused.Size = New System.Drawing.Size(283, 18)
        Me.CheckBox_OnlyUpdateSyntaxWhenFocused.TabIndex = 23
        Me.CheckBox_OnlyUpdateSyntaxWhenFocused.Text = "Only update syntax when the window is focused"
        Me.CheckBox_OnlyUpdateSyntaxWhenFocused.UseVisualStyleBackColor = True
        '
        'CheckBox_FullAutocompleteReTagging
        '
        Me.CheckBox_FullAutocompleteReTagging.AutoSize = True
        Me.CheckBox_FullAutocompleteReTagging.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_FullAutocompleteReTagging.Location = New System.Drawing.Point(6, 219)
        Me.CheckBox_FullAutocompleteReTagging.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_FullAutocompleteReTagging.Name = "CheckBox_FullAutocompleteReTagging"
        Me.CheckBox_FullAutocompleteReTagging.Size = New System.Drawing.Size(415, 18)
        Me.CheckBox_FullAutocompleteReTagging.TabIndex = 8
        Me.CheckBox_FullAutocompleteReTagging.Text = "Full autocompletion for enums using re-tagging (SourcePawn <1.6 Syntax)"
        Me.CheckBox_FullAutocompleteReTagging.UseVisualStyleBackColor = True
        '
        'CheckBox_WindowsToolTipNewlineMethods
        '
        Me.CheckBox_WindowsToolTipNewlineMethods.AutoSize = True
        Me.CheckBox_WindowsToolTipNewlineMethods.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_WindowsToolTipNewlineMethods.Location = New System.Drawing.Point(64, 147)
        Me.CheckBox_WindowsToolTipNewlineMethods.Margin = New System.Windows.Forms.Padding(64, 3, 3, 3)
        Me.CheckBox_WindowsToolTipNewlineMethods.Name = "CheckBox_WindowsToolTipNewlineMethods"
        Me.CheckBox_WindowsToolTipNewlineMethods.Size = New System.Drawing.Size(175, 18)
        Me.CheckBox_WindowsToolTipNewlineMethods.TabIndex = 22
        Me.CheckBox_WindowsToolTipNewlineMethods.Text = "Newline method arguments"
        Me.CheckBox_WindowsToolTipNewlineMethods.UseVisualStyleBackColor = True
        '
        'CheckBox_CommentsAutocompleteIntelliSense
        '
        Me.CheckBox_CommentsAutocompleteIntelliSense.AutoSize = True
        Me.CheckBox_CommentsAutocompleteIntelliSense.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_CommentsAutocompleteIntelliSense.Location = New System.Drawing.Point(32, 75)
        Me.CheckBox_CommentsAutocompleteIntelliSense.Margin = New System.Windows.Forms.Padding(32, 3, 3, 3)
        Me.CheckBox_CommentsAutocompleteIntelliSense.Name = "CheckBox_CommentsAutocompleteIntelliSense"
        Me.CheckBox_CommentsAutocompleteIntelliSense.Size = New System.Drawing.Size(356, 18)
        Me.CheckBox_CommentsAutocompleteIntelliSense.TabIndex = 7
        Me.CheckBox_CommentsAutocompleteIntelliSense.Text = "Display comments for autocompletion in on-screen IntelliSense"
        Me.CheckBox_CommentsAutocompleteIntelliSense.UseVisualStyleBackColor = True
        '
        'CheckBox_WindowsToolTipAnimations
        '
        Me.CheckBox_WindowsToolTipAnimations.AutoSize = True
        Me.CheckBox_WindowsToolTipAnimations.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_WindowsToolTipAnimations.Location = New System.Drawing.Point(64, 123)
        Me.CheckBox_WindowsToolTipAnimations.Margin = New System.Windows.Forms.Padding(64, 3, 3, 3)
        Me.CheckBox_WindowsToolTipAnimations.Name = "CheckBox_WindowsToolTipAnimations"
        Me.CheckBox_WindowsToolTipAnimations.Size = New System.Drawing.Size(204, 18)
        Me.CheckBox_WindowsToolTipAnimations.TabIndex = 21
        Me.CheckBox_WindowsToolTipAnimations.Text = "Smooth tooltip popup movement"
        Me.CheckBox_WindowsToolTipAnimations.UseVisualStyleBackColor = True
        '
        'CheckBox_CommentsMethodIntelliSense
        '
        Me.CheckBox_CommentsMethodIntelliSense.AutoSize = True
        Me.CheckBox_CommentsMethodIntelliSense.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_CommentsMethodIntelliSense.Location = New System.Drawing.Point(32, 51)
        Me.CheckBox_CommentsMethodIntelliSense.Margin = New System.Windows.Forms.Padding(32, 3, 3, 3)
        Me.CheckBox_CommentsMethodIntelliSense.Name = "CheckBox_CommentsMethodIntelliSense"
        Me.CheckBox_CommentsMethodIntelliSense.Size = New System.Drawing.Size(319, 18)
        Me.CheckBox_CommentsMethodIntelliSense.TabIndex = 6
        Me.CheckBox_CommentsMethodIntelliSense.Text = "Display comments for methods in on-screen IntelliSense"
        Me.CheckBox_CommentsMethodIntelliSense.UseVisualStyleBackColor = True
        '
        'CheckBox_FullAutcompleteMethods
        '
        Me.CheckBox_FullAutcompleteMethods.AutoSize = True
        Me.CheckBox_FullAutcompleteMethods.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_FullAutcompleteMethods.Location = New System.Drawing.Point(6, 195)
        Me.CheckBox_FullAutcompleteMethods.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_FullAutcompleteMethods.Name = "CheckBox_FullAutcompleteMethods"
        Me.CheckBox_FullAutcompleteMethods.Size = New System.Drawing.Size(202, 18)
        Me.CheckBox_FullAutcompleteMethods.TabIndex = 3
        Me.CheckBox_FullAutcompleteMethods.Text = "Full autocompletion for methods"
        Me.CheckBox_FullAutcompleteMethods.UseVisualStyleBackColor = True
        '
        'CheckBox_SwitchTabToAutocomplete
        '
        Me.CheckBox_SwitchTabToAutocomplete.AutoSize = True
        Me.CheckBox_SwitchTabToAutocomplete.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_SwitchTabToAutocomplete.Location = New System.Drawing.Point(6, 291)
        Me.CheckBox_SwitchTabToAutocomplete.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_SwitchTabToAutocomplete.Name = "CheckBox_SwitchTabToAutocomplete"
        Me.CheckBox_SwitchTabToAutocomplete.Size = New System.Drawing.Size(326, 18)
        Me.CheckBox_SwitchTabToAutocomplete.TabIndex = 20
        Me.CheckBox_SwitchTabToAutocomplete.Text = "Automatically switch tab to 'Autocomplete && IntelliSense'"
        Me.CheckBox_SwitchTabToAutocomplete.UseVisualStyleBackColor = True
        '
        'CheckBox_OnScreenIntelliSense
        '
        Me.CheckBox_OnScreenIntelliSense.AutoSize = True
        Me.CheckBox_OnScreenIntelliSense.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_OnScreenIntelliSense.Location = New System.Drawing.Point(6, 27)
        Me.CheckBox_OnScreenIntelliSense.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_OnScreenIntelliSense.Name = "CheckBox_OnScreenIntelliSense"
        Me.CheckBox_OnScreenIntelliSense.Size = New System.Drawing.Size(129, 18)
        Me.CheckBox_OnScreenIntelliSense.TabIndex = 2
        Me.CheckBox_OnScreenIntelliSense.Text = "Enable IntelliSense"
        Me.CheckBox_OnScreenIntelliSense.UseVisualStyleBackColor = True
        '
        'CheckBox_VarAutocompleteShowObjectBrowser
        '
        Me.CheckBox_VarAutocompleteShowObjectBrowser.AutoSize = True
        Me.CheckBox_VarAutocompleteShowObjectBrowser.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_VarAutocompleteShowObjectBrowser.Location = New System.Drawing.Point(6, 267)
        Me.CheckBox_VarAutocompleteShowObjectBrowser.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_VarAutocompleteShowObjectBrowser.Name = "CheckBox_VarAutocompleteShowObjectBrowser"
        Me.CheckBox_VarAutocompleteShowObjectBrowser.Size = New System.Drawing.Size(224, 18)
        Me.CheckBox_VarAutocompleteShowObjectBrowser.TabIndex = 18
        Me.CheckBox_VarAutocompleteShowObjectBrowser.Text = "Show variables in the Object Browser"
        Me.CheckBox_VarAutocompleteShowObjectBrowser.UseVisualStyleBackColor = True
        '
        'TabPage_Configs
        '
        Me.TabPage_Configs.Controls.Add(Me.TableLayoutPanel1)
        Me.TabPage_Configs.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Configs.Name = "TabPage_Configs"
        Me.TabPage_Configs.Size = New System.Drawing.Size(752, 482)
        Me.TabPage_Configs.TabIndex = 1
        Me.TabPage_Configs.Text = "Configs"
        Me.TabPage_Configs.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ListBox_Configs, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(752, 482)
        Me.TableLayoutPanel1.TabIndex = 10
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Panel17, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Panel2, 0, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(150, 0)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(602, 482)
        Me.TableLayoutPanel2.TabIndex = 8
        '
        'Panel17
        '
        Me.Panel17.Controls.Add(Me.Button_SaveConfig)
        Me.Panel17.Controls.Add(Me.Label_ConfigName)
        Me.Panel17.Controls.Add(Me.Button_ConfigRename)
        Me.Panel17.Controls.Add(Me.TextBox_ConfigName)
        Me.Panel17.Controls.Add(Me.Button_ConfigCopy)
        Me.Panel17.Controls.Add(Me.Button_ConfigRemove)
        Me.Panel17.Controls.Add(Me.Button_ConfigAdd)
        Me.Panel17.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel17.Location = New System.Drawing.Point(0, 0)
        Me.Panel17.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel17.Name = "Panel17"
        Me.Panel17.Size = New System.Drawing.Size(602, 100)
        Me.Panel17.TabIndex = 0
        '
        'Button_SaveConfig
        '
        Me.Button_SaveConfig.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_SaveConfig.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_SaveConfig.Location = New System.Drawing.Point(471, 74)
        Me.Button_SaveConfig.Name = "Button_SaveConfig"
        Me.Button_SaveConfig.Size = New System.Drawing.Size(128, 23)
        Me.Button_SaveConfig.TabIndex = 12
        Me.Button_SaveConfig.Text = "Save Settings"
        Me.Button_SaveConfig.UseVisualStyleBackColor = True
        '
        'Label_ConfigName
        '
        Me.Label_ConfigName.AutoSize = True
        Me.Label_ConfigName.Location = New System.Drawing.Point(3, 0)
        Me.Label_ConfigName.Name = "Label_ConfigName"
        Me.Label_ConfigName.Size = New System.Drawing.Size(77, 13)
        Me.Label_ConfigName.TabIndex = 1
        Me.Label_ConfigName.Text = "Config Name:"
        '
        'Button_ConfigRename
        '
        Me.Button_ConfigRename.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ConfigRename.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ConfigRename.Location = New System.Drawing.Point(237, 44)
        Me.Button_ConfigRename.Name = "Button_ConfigRename"
        Me.Button_ConfigRename.Size = New System.Drawing.Size(86, 23)
        Me.Button_ConfigRename.TabIndex = 9
        Me.Button_ConfigRename.Text = "Rename"
        Me.Button_ConfigRename.UseVisualStyleBackColor = True
        '
        'TextBox_ConfigName
        '
        Me.TextBox_ConfigName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_ConfigName.Location = New System.Drawing.Point(3, 16)
        Me.TextBox_ConfigName.Name = "TextBox_ConfigName"
        Me.TextBox_ConfigName.Size = New System.Drawing.Size(596, 22)
        Me.TextBox_ConfigName.TabIndex = 2
        '
        'Button_ConfigCopy
        '
        Me.Button_ConfigCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ConfigCopy.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ConfigCopy.Location = New System.Drawing.Point(329, 44)
        Me.Button_ConfigCopy.Name = "Button_ConfigCopy"
        Me.Button_ConfigCopy.Size = New System.Drawing.Size(86, 23)
        Me.Button_ConfigCopy.TabIndex = 8
        Me.Button_ConfigCopy.Text = "Copy"
        Me.Button_ConfigCopy.UseVisualStyleBackColor = True
        '
        'Button_ConfigRemove
        '
        Me.Button_ConfigRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ConfigRemove.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ConfigRemove.Location = New System.Drawing.Point(513, 44)
        Me.Button_ConfigRemove.Name = "Button_ConfigRemove"
        Me.Button_ConfigRemove.Size = New System.Drawing.Size(86, 23)
        Me.Button_ConfigRemove.TabIndex = 4
        Me.Button_ConfigRemove.Text = "Remove"
        Me.Button_ConfigRemove.UseVisualStyleBackColor = True
        '
        'Button_ConfigAdd
        '
        Me.Button_ConfigAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ConfigAdd.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ConfigAdd.Location = New System.Drawing.Point(421, 44)
        Me.Button_ConfigAdd.Name = "Button_ConfigAdd"
        Me.Button_ConfigAdd.Size = New System.Drawing.Size(86, 23)
        Me.Button_ConfigAdd.TabIndex = 5
        Me.Button_ConfigAdd.Text = "Add"
        Me.Button_ConfigAdd.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.AutoScroll = True
        Me.Panel2.Controls.Add(Me.GroupBox_ConfigSettings)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 100)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(602, 382)
        Me.Panel2.TabIndex = 1
        '
        'GroupBox_ConfigSettings
        '
        Me.GroupBox_ConfigSettings.AutoSize = True
        Me.GroupBox_ConfigSettings.Controls.Add(Me.Panel11)
        Me.GroupBox_ConfigSettings.Controls.Add(Me.Panel10)
        Me.GroupBox_ConfigSettings.Controls.Add(Me.Panel9)
        Me.GroupBox_ConfigSettings.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox_ConfigSettings.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox_ConfigSettings.Margin = New System.Windows.Forms.Padding(0)
        Me.GroupBox_ConfigSettings.Name = "GroupBox_ConfigSettings"
        Me.GroupBox_ConfigSettings.Size = New System.Drawing.Size(585, 950)
        Me.GroupBox_ConfigSettings.TabIndex = 7
        Me.GroupBox_ConfigSettings.TabStop = False
        Me.GroupBox_ConfigSettings.Text = "Config Settings"
        '
        'Panel11
        '
        Me.Panel11.AutoSize = True
        Me.Panel11.Controls.Add(Me.Label13)
        Me.Panel11.Controls.Add(Me.Panel13)
        Me.Panel11.Controls.Add(Me.Label10)
        Me.Panel11.Controls.Add(Me.TextBox_Shell)
        Me.Panel11.Controls.Add(Me.LinkLabel_ShowShellArguments)
        Me.Panel11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel11.Location = New System.Drawing.Point(3, 867)
        Me.Panel11.Name = "Panel11"
        Me.Panel11.Size = New System.Drawing.Size(579, 80)
        Me.Panel11.TabIndex = 30
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label13.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(0, 1)
        Me.Label13.Margin = New System.Windows.Forms.Padding(3)
        Me.Label13.Name = "Label13"
        Me.Label13.Padding = New System.Windows.Forms.Padding(6, 3, 0, 0)
        Me.Label13.Size = New System.Drawing.Size(37, 16)
        Me.Label13.TabIndex = 27
        Me.Label13.Text = "Misc"
        '
        'Panel13
        '
        Me.Panel13.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel13.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel13.Location = New System.Drawing.Point(0, 0)
        Me.Panel13.Name = "Panel13"
        Me.Panel13.Size = New System.Drawing.Size(579, 1)
        Me.Panel13.TabIndex = 28
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(3, 20)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(77, 13)
        Me.Label10.TabIndex = 16
        Me.Label10.Text = "Execute Shell:"
        '
        'TextBox_Shell
        '
        Me.TextBox_Shell.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_Shell.BackColor = System.Drawing.Color.White
        Me.TextBox_Shell.Location = New System.Drawing.Point(3, 36)
        Me.TextBox_Shell.Name = "TextBox_Shell"
        Me.TextBox_Shell.Size = New System.Drawing.Size(573, 22)
        Me.TextBox_Shell.TabIndex = 17
        '
        'LinkLabel_ShowShellArguments
        '
        Me.LinkLabel_ShowShellArguments.AutoSize = True
        Me.LinkLabel_ShowShellArguments.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel_ShowShellArguments.Location = New System.Drawing.Point(3, 61)
        Me.LinkLabel_ShowShellArguments.Margin = New System.Windows.Forms.Padding(3, 0, 3, 6)
        Me.LinkLabel_ShowShellArguments.Name = "LinkLabel_ShowShellArguments"
        Me.LinkLabel_ShowShellArguments.Size = New System.Drawing.Size(136, 13)
        Me.LinkLabel_ShowShellArguments.TabIndex = 18
        Me.LinkLabel_ShowShellArguments.TabStop = True
        Me.LinkLabel_ShowShellArguments.Text = "Show all shell arguments"
        '
        'Panel10
        '
        Me.Panel10.AutoSize = True
        Me.Panel10.Controls.Add(Me.Label1)
        Me.Panel10.Controls.Add(Me.TextBox_ClientFolder)
        Me.Panel10.Controls.Add(Me.Button_ClientFolder)
        Me.Panel10.Controls.Add(Me.Label9)
        Me.Panel10.Controls.Add(Me.Panel12)
        Me.Panel10.Controls.Add(Me.Label2)
        Me.Panel10.Controls.Add(Me.TextBox_ServerFolder)
        Me.Panel10.Controls.Add(Me.Button_ServerFolder)
        Me.Panel10.Controls.Add(Me.TextBox_SourceModFolder)
        Me.Panel10.Controls.Add(Me.Button_SourceModFolder)
        Me.Panel10.Controls.Add(Me.Label12)
        Me.Panel10.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel10.Location = New System.Drawing.Point(3, 724)
        Me.Panel10.Name = "Panel10"
        Me.Panel10.Size = New System.Drawing.Size(579, 143)
        Me.Panel10.TabIndex = 29
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 28
        Me.Label1.Text = "Client directory:"
        '
        'TextBox_ClientFolder
        '
        Me.TextBox_ClientFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_ClientFolder.BackColor = System.Drawing.Color.White
        Me.TextBox_ClientFolder.Location = New System.Drawing.Point(3, 36)
        Me.TextBox_ClientFolder.Name = "TextBox_ClientFolder"
        Me.TextBox_ClientFolder.Size = New System.Drawing.Size(536, 22)
        Me.TextBox_ClientFolder.TabIndex = 29
        '
        'Button_ClientFolder
        '
        Me.Button_ClientFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ClientFolder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ClientFolder.Location = New System.Drawing.Point(545, 34)
        Me.Button_ClientFolder.Name = "Button_ClientFolder"
        Me.Button_ClientFolder.Size = New System.Drawing.Size(31, 24)
        Me.Button_ClientFolder.TabIndex = 30
        Me.Button_ClientFolder.Text = "..."
        Me.Button_ClientFolder.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(0, 1)
        Me.Label9.Margin = New System.Windows.Forms.Padding(3)
        Me.Label9.Name = "Label9"
        Me.Label9.Padding = New System.Windows.Forms.Padding(6, 3, 0, 0)
        Me.Label9.Size = New System.Drawing.Size(72, 16)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "Debugging"
        '
        'Panel12
        '
        Me.Panel12.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel12.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel12.Location = New System.Drawing.Point(0, 0)
        Me.Panel12.Name = "Panel12"
        Me.Panel12.Size = New System.Drawing.Size(579, 1)
        Me.Panel12.TabIndex = 27
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 13)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Server directory:"
        '
        'TextBox_ServerFolder
        '
        Me.TextBox_ServerFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_ServerFolder.BackColor = System.Drawing.Color.White
        Me.TextBox_ServerFolder.Location = New System.Drawing.Point(3, 77)
        Me.TextBox_ServerFolder.Name = "TextBox_ServerFolder"
        Me.TextBox_ServerFolder.Size = New System.Drawing.Size(536, 22)
        Me.TextBox_ServerFolder.TabIndex = 20
        '
        'Button_ServerFolder
        '
        Me.Button_ServerFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ServerFolder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ServerFolder.Location = New System.Drawing.Point(545, 75)
        Me.Button_ServerFolder.Name = "Button_ServerFolder"
        Me.Button_ServerFolder.Size = New System.Drawing.Size(31, 24)
        Me.Button_ServerFolder.TabIndex = 21
        Me.Button_ServerFolder.Text = "..."
        Me.Button_ServerFolder.UseVisualStyleBackColor = True
        '
        'TextBox_SourceModFolder
        '
        Me.TextBox_SourceModFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_SourceModFolder.BackColor = System.Drawing.Color.White
        Me.TextBox_SourceModFolder.Location = New System.Drawing.Point(3, 118)
        Me.TextBox_SourceModFolder.Name = "TextBox_SourceModFolder"
        Me.TextBox_SourceModFolder.Size = New System.Drawing.Size(536, 22)
        Me.TextBox_SourceModFolder.TabIndex = 25
        '
        'Button_SourceModFolder
        '
        Me.Button_SourceModFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_SourceModFolder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_SourceModFolder.Location = New System.Drawing.Point(545, 116)
        Me.Button_SourceModFolder.Name = "Button_SourceModFolder"
        Me.Button_SourceModFolder.Size = New System.Drawing.Size(31, 24)
        Me.Button_SourceModFolder.TabIndex = 26
        Me.Button_SourceModFolder.Text = "..."
        Me.Button_SourceModFolder.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(3, 102)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(117, 13)
        Me.Label12.TabIndex = 24
        Me.Label12.Text = "SourceMod directory:"
        '
        'Panel9
        '
        Me.Panel9.AutoSize = True
        Me.Panel9.Controls.Add(Me.Label7)
        Me.Panel9.Controls.Add(Me.GroupBox3)
        Me.Panel9.Controls.Add(Me.TextBox_AutoAssignPaths)
        Me.Panel9.Controls.Add(Me.Button_AutoAssignPaths)
        Me.Panel9.Controls.Add(Me.GroupBox2)
        Me.Panel9.Controls.Add(Me.Label20)
        Me.Panel9.Controls.Add(Me.ComboBox_Language)
        Me.Panel9.Controls.Add(Me.Label11)
        Me.Panel9.Controls.Add(Me.Label3)
        Me.Panel9.Controls.Add(Me.RadioButton_ConfigSettingAutomatic)
        Me.Panel9.Controls.Add(Me.RadioButton_ConfigSettingManual)
        Me.Panel9.Controls.Add(Me.CheckBox_ConfigIsDefault)
        Me.Panel9.Controls.Add(Me.GroupBox1)
        Me.Panel9.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel9.Location = New System.Drawing.Point(3, 18)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(579, 706)
        Me.Panel9.TabIndex = 28
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 245)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(197, 13)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Default config paths (Seperate by ';'):"
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.ListView_KnownFiles)
        Me.GroupBox3.Controls.Add(Me.Button_KnownFileAdd)
        Me.GroupBox3.Controls.Add(Me.Button_KnownFileRemove)
        Me.GroupBox3.Location = New System.Drawing.Point(3, 513)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(573, 190)
        Me.GroupBox3.TabIndex = 29
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Known Files"
        '
        'ListView_KnownFiles
        '
        Me.ListView_KnownFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_KnownFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader8})
        Me.ListView_KnownFiles.Location = New System.Drawing.Point(6, 21)
        Me.ListView_KnownFiles.Name = "ListView_KnownFiles"
        Me.ListView_KnownFiles.Size = New System.Drawing.Size(561, 134)
        Me.ListView_KnownFiles.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView_KnownFiles.TabIndex = 3
        Me.ListView_KnownFiles.UseCompatibleStateImageBehavior = False
        Me.ListView_KnownFiles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "File"
        '
        'Button_KnownFileAdd
        '
        Me.Button_KnownFileAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_KnownFileAdd.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_KnownFileAdd.Location = New System.Drawing.Point(389, 161)
        Me.Button_KnownFileAdd.Name = "Button_KnownFileAdd"
        Me.Button_KnownFileAdd.Size = New System.Drawing.Size(86, 23)
        Me.Button_KnownFileAdd.TabIndex = 2
        Me.Button_KnownFileAdd.Text = "Add"
        Me.Button_KnownFileAdd.UseVisualStyleBackColor = True
        '
        'Button_KnownFileRemove
        '
        Me.Button_KnownFileRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_KnownFileRemove.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_KnownFileRemove.Location = New System.Drawing.Point(481, 161)
        Me.Button_KnownFileRemove.Name = "Button_KnownFileRemove"
        Me.Button_KnownFileRemove.Size = New System.Drawing.Size(86, 23)
        Me.Button_KnownFileRemove.TabIndex = 1
        Me.Button_KnownFileRemove.Text = "Remove"
        Me.Button_KnownFileRemove.UseVisualStyleBackColor = True
        '
        'TextBox_AutoAssignPaths
        '
        Me.TextBox_AutoAssignPaths.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_AutoAssignPaths.BackColor = System.Drawing.Color.White
        Me.TextBox_AutoAssignPaths.Location = New System.Drawing.Point(6, 261)
        Me.TextBox_AutoAssignPaths.Name = "TextBox_AutoAssignPaths"
        Me.TextBox_AutoAssignPaths.Size = New System.Drawing.Size(530, 22)
        Me.TextBox_AutoAssignPaths.TabIndex = 17
        '
        'Button_AutoAssignPaths
        '
        Me.Button_AutoAssignPaths.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_AutoAssignPaths.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_AutoAssignPaths.Location = New System.Drawing.Point(542, 259)
        Me.Button_AutoAssignPaths.Name = "Button_AutoAssignPaths"
        Me.Button_AutoAssignPaths.Size = New System.Drawing.Size(31, 24)
        Me.Button_AutoAssignPaths.TabIndex = 18
        Me.Button_AutoAssignPaths.Text = "..."
        Me.Button_AutoAssignPaths.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.TabControl2)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 316)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(573, 191)
        Me.GroupBox2.TabIndex = 28
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Compiler Options"
        '
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.TabPage1)
        Me.TabControl2.Controls.Add(Me.TabPage2)
        Me.TabControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl2.Location = New System.Drawing.Point(3, 18)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(567, 170)
        Me.TabControl2.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label25)
        Me.TabPage1.Controls.Add(Me.TextBoxEx_CODefineConstantsSP)
        Me.TabPage1.Controls.Add(Me.TextBoxEx_COIgnoredWarningsSP)
        Me.TabPage1.Controls.Add(Me.Label24)
        Me.TabPage1.Controls.Add(Me.Label23)
        Me.TabPage1.Controls.Add(Me.ComboBox_COTreatWarningsAsErrorsSP)
        Me.TabPage1.Controls.Add(Me.Label22)
        Me.TabPage1.Controls.Add(Me.ComboBox_COVerbosityLevelSP)
        Me.TabPage1.Controls.Add(Me.ComboBox_COOptimizationLevelSP)
        Me.TabPage1.Controls.Add(Me.Label21)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(559, 144)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "SourcePawn"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(6, 118)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(175, 13)
        Me.Label25.TabIndex = 9
        Me.Label25.Text = "Define constants (Separate by ';')"
        '
        'TextBoxEx_CODefineConstantsSP
        '
        Me.TextBoxEx_CODefineConstantsSP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxEx_CODefineConstantsSP.Location = New System.Drawing.Point(198, 115)
        Me.TextBoxEx_CODefineConstantsSP.m_WatermarkText = "sym=val;sym2=val..."
        Me.TextBoxEx_CODefineConstantsSP.Name = "TextBoxEx_CODefineConstantsSP"
        Me.TextBoxEx_CODefineConstantsSP.Size = New System.Drawing.Size(355, 22)
        Me.TextBoxEx_CODefineConstantsSP.TabIndex = 8
        '
        'TextBoxEx_COIgnoredWarningsSP
        '
        Me.TextBoxEx_COIgnoredWarningsSP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxEx_COIgnoredWarningsSP.Location = New System.Drawing.Point(198, 87)
        Me.TextBoxEx_COIgnoredWarningsSP.m_WatermarkText = "100;101..."
        Me.TextBoxEx_COIgnoredWarningsSP.Name = "TextBoxEx_COIgnoredWarningsSP"
        Me.TextBoxEx_COIgnoredWarningsSP.Size = New System.Drawing.Size(355, 22)
        Me.TextBoxEx_COIgnoredWarningsSP.TabIndex = 7
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(6, 90)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(180, 13)
        Me.Label24.TabIndex = 6
        Me.Label24.Text = "Ignored warnings (Separate by ';')"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(6, 63)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(129, 13)
        Me.Label23.TabIndex = 5
        Me.Label23.Text = "Treat warnings as errors"
        '
        'ComboBox_COTreatWarningsAsErrorsSP
        '
        Me.ComboBox_COTreatWarningsAsErrorsSP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_COTreatWarningsAsErrorsSP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_COTreatWarningsAsErrorsSP.FormattingEnabled = True
        Me.ComboBox_COTreatWarningsAsErrorsSP.Location = New System.Drawing.Point(198, 60)
        Me.ComboBox_COTreatWarningsAsErrorsSP.Name = "ComboBox_COTreatWarningsAsErrorsSP"
        Me.ComboBox_COTreatWarningsAsErrorsSP.Size = New System.Drawing.Size(355, 21)
        Me.ComboBox_COTreatWarningsAsErrorsSP.TabIndex = 4
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(6, 36)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(80, 13)
        Me.Label22.TabIndex = 3
        Me.Label22.Text = "Verbosity level"
        '
        'ComboBox_COVerbosityLevelSP
        '
        Me.ComboBox_COVerbosityLevelSP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_COVerbosityLevelSP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_COVerbosityLevelSP.FormattingEnabled = True
        Me.ComboBox_COVerbosityLevelSP.Location = New System.Drawing.Point(198, 33)
        Me.ComboBox_COVerbosityLevelSP.Name = "ComboBox_COVerbosityLevelSP"
        Me.ComboBox_COVerbosityLevelSP.Size = New System.Drawing.Size(355, 21)
        Me.ComboBox_COVerbosityLevelSP.TabIndex = 2
        '
        'ComboBox_COOptimizationLevelSP
        '
        Me.ComboBox_COOptimizationLevelSP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_COOptimizationLevelSP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_COOptimizationLevelSP.FormattingEnabled = True
        Me.ComboBox_COOptimizationLevelSP.Location = New System.Drawing.Point(198, 6)
        Me.ComboBox_COOptimizationLevelSP.Name = "ComboBox_COOptimizationLevelSP"
        Me.ComboBox_COOptimizationLevelSP.Size = New System.Drawing.Size(355, 21)
        Me.ComboBox_COOptimizationLevelSP.TabIndex = 1
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(6, 9)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(100, 13)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "Optimization level"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Label31)
        Me.TabPage2.Controls.Add(Me.ComboBox_COSymbolicInformationAMXX)
        Me.TabPage2.Controls.Add(Me.Label26)
        Me.TabPage2.Controls.Add(Me.TextBoxEx_CODefineConstantsAMXX)
        Me.TabPage2.Controls.Add(Me.TextBoxEx_COIgnoredWarningsAMXX)
        Me.TabPage2.Controls.Add(Me.Label27)
        Me.TabPage2.Controls.Add(Me.Label28)
        Me.TabPage2.Controls.Add(Me.ComboBox_COTreatWarningsAsErrorsAMXX)
        Me.TabPage2.Controls.Add(Me.Label29)
        Me.TabPage2.Controls.Add(Me.ComboBox_COVerbosityLevelAMXX)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(559, 146)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "AMX Mod X"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(6, 9)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(116, 13)
        Me.Label31.TabIndex = 21
        Me.Label31.Text = "Symbolic information"
        '
        'ComboBox_COSymbolicInformationAMXX
        '
        Me.ComboBox_COSymbolicInformationAMXX.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_COSymbolicInformationAMXX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_COSymbolicInformationAMXX.FormattingEnabled = True
        Me.ComboBox_COSymbolicInformationAMXX.Location = New System.Drawing.Point(198, 6)
        Me.ComboBox_COSymbolicInformationAMXX.Name = "ComboBox_COSymbolicInformationAMXX"
        Me.ComboBox_COSymbolicInformationAMXX.Size = New System.Drawing.Size(355, 21)
        Me.ComboBox_COSymbolicInformationAMXX.TabIndex = 20
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(6, 118)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(175, 13)
        Me.Label26.TabIndex = 19
        Me.Label26.Text = "Define constants (Separate by ';')"
        '
        'TextBoxEx_CODefineConstantsAMXX
        '
        Me.TextBoxEx_CODefineConstantsAMXX.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxEx_CODefineConstantsAMXX.Location = New System.Drawing.Point(198, 115)
        Me.TextBoxEx_CODefineConstantsAMXX.m_WatermarkText = "sym=val;sym2=val..."
        Me.TextBoxEx_CODefineConstantsAMXX.Name = "TextBoxEx_CODefineConstantsAMXX"
        Me.TextBoxEx_CODefineConstantsAMXX.Size = New System.Drawing.Size(355, 22)
        Me.TextBoxEx_CODefineConstantsAMXX.TabIndex = 18
        '
        'TextBoxEx_COIgnoredWarningsAMXX
        '
        Me.TextBoxEx_COIgnoredWarningsAMXX.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxEx_COIgnoredWarningsAMXX.Location = New System.Drawing.Point(198, 87)
        Me.TextBoxEx_COIgnoredWarningsAMXX.m_WatermarkText = "100;101..."
        Me.TextBoxEx_COIgnoredWarningsAMXX.Name = "TextBoxEx_COIgnoredWarningsAMXX"
        Me.TextBoxEx_COIgnoredWarningsAMXX.Size = New System.Drawing.Size(355, 22)
        Me.TextBoxEx_COIgnoredWarningsAMXX.TabIndex = 17
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(6, 90)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(180, 13)
        Me.Label27.TabIndex = 16
        Me.Label27.Text = "Ignored warnings (Separate by ';')"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(6, 63)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(129, 13)
        Me.Label28.TabIndex = 15
        Me.Label28.Text = "Treat warnings as errors"
        '
        'ComboBox_COTreatWarningsAsErrorsAMXX
        '
        Me.ComboBox_COTreatWarningsAsErrorsAMXX.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_COTreatWarningsAsErrorsAMXX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_COTreatWarningsAsErrorsAMXX.FormattingEnabled = True
        Me.ComboBox_COTreatWarningsAsErrorsAMXX.Location = New System.Drawing.Point(198, 60)
        Me.ComboBox_COTreatWarningsAsErrorsAMXX.Name = "ComboBox_COTreatWarningsAsErrorsAMXX"
        Me.ComboBox_COTreatWarningsAsErrorsAMXX.Size = New System.Drawing.Size(355, 21)
        Me.ComboBox_COTreatWarningsAsErrorsAMXX.TabIndex = 14
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(6, 36)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(80, 13)
        Me.Label29.TabIndex = 13
        Me.Label29.Text = "Verbosity level"
        '
        'ComboBox_COVerbosityLevelAMXX
        '
        Me.ComboBox_COVerbosityLevelAMXX.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_COVerbosityLevelAMXX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_COVerbosityLevelAMXX.FormattingEnabled = True
        Me.ComboBox_COVerbosityLevelAMXX.Location = New System.Drawing.Point(198, 33)
        Me.ComboBox_COVerbosityLevelAMXX.Name = "ComboBox_COVerbosityLevelAMXX"
        Me.ComboBox_COVerbosityLevelAMXX.Size = New System.Drawing.Size(355, 21)
        Me.ComboBox_COVerbosityLevelAMXX.TabIndex = 12
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(3, 292)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(61, 13)
        Me.Label20.TabIndex = 27
        Me.Label20.Text = "Language:"
        '
        'ComboBox_Language
        '
        Me.ComboBox_Language.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Language.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox_Language.FormattingEnabled = True
        Me.ComboBox_Language.Location = New System.Drawing.Point(361, 289)
        Me.ComboBox_Language.Name = "ComboBox_Language"
        Me.ComboBox_Language.Size = New System.Drawing.Size(212, 21)
        Me.ComboBox_Language.TabIndex = 26
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label11.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(0, 0)
        Me.Label11.Margin = New System.Windows.Forms.Padding(3)
        Me.Label11.Name = "Label11"
        Me.Label11.Padding = New System.Windows.Forms.Padding(6, 3, 0, 0)
        Me.Label11.Size = New System.Drawing.Size(53, 16)
        Me.Label11.TabIndex = 23
        Me.Label11.Text = "General"
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.Location = New System.Drawing.Point(16, 43)
        Me.Label3.Margin = New System.Windows.Forms.Padding(16, 0, 3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(560, 28)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Automatically detect compiler path and include folder from currently opened sourc" &
    "e file."
        '
        'RadioButton_ConfigSettingAutomatic
        '
        Me.RadioButton_ConfigSettingAutomatic.AutoSize = True
        Me.RadioButton_ConfigSettingAutomatic.Checked = True
        Me.RadioButton_ConfigSettingAutomatic.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RadioButton_ConfigSettingAutomatic.Location = New System.Drawing.Point(6, 22)
        Me.RadioButton_ConfigSettingAutomatic.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.RadioButton_ConfigSettingAutomatic.Name = "RadioButton_ConfigSettingAutomatic"
        Me.RadioButton_ConfigSettingAutomatic.Size = New System.Drawing.Size(83, 18)
        Me.RadioButton_ConfigSettingAutomatic.TabIndex = 0
        Me.RadioButton_ConfigSettingAutomatic.TabStop = True
        Me.RadioButton_ConfigSettingAutomatic.Text = "Automatic"
        Me.RadioButton_ConfigSettingAutomatic.UseVisualStyleBackColor = True
        '
        'RadioButton_ConfigSettingManual
        '
        Me.RadioButton_ConfigSettingManual.AutoSize = True
        Me.RadioButton_ConfigSettingManual.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RadioButton_ConfigSettingManual.Location = New System.Drawing.Point(6, 74)
        Me.RadioButton_ConfigSettingManual.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.RadioButton_ConfigSettingManual.Name = "RadioButton_ConfigSettingManual"
        Me.RadioButton_ConfigSettingManual.Size = New System.Drawing.Size(70, 18)
        Me.RadioButton_ConfigSettingManual.TabIndex = 1
        Me.RadioButton_ConfigSettingManual.Text = "Manual"
        '
        'CheckBox_ConfigIsDefault
        '
        Me.CheckBox_ConfigIsDefault.AutoSize = True
        Me.CheckBox_ConfigIsDefault.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_ConfigIsDefault.Location = New System.Drawing.Point(6, 224)
        Me.CheckBox_ConfigIsDefault.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_ConfigIsDefault.Name = "CheckBox_ConfigIsDefault"
        Me.CheckBox_ConfigIsDefault.Size = New System.Drawing.Size(160, 18)
        Me.CheckBox_ConfigIsDefault.TabIndex = 25
        Me.CheckBox_ConfigIsDefault.Text = "Set this config as default"
        Me.CheckBox_ConfigIsDefault.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.TextBox_CompilerPath)
        Me.GroupBox1.Controls.Add(Me.Button_Compiler)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.TextBox_IncludeFolder)
        Me.GroupBox1.Controls.Add(Me.Button_IncludeFolder)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.TextBox_OutputFolder)
        Me.GroupBox1.Controls.Add(Me.Button_OutputFolder)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 74)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(573, 144)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "                    "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(83, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Compiler path:"
        '
        'TextBox_CompilerPath
        '
        Me.TextBox_CompilerPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_CompilerPath.BackColor = System.Drawing.Color.White
        Me.TextBox_CompilerPath.Location = New System.Drawing.Point(6, 36)
        Me.TextBox_CompilerPath.Name = "TextBox_CompilerPath"
        Me.TextBox_CompilerPath.Size = New System.Drawing.Size(524, 22)
        Me.TextBox_CompilerPath.TabIndex = 8
        '
        'Button_Compiler
        '
        Me.Button_Compiler.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Compiler.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Compiler.Location = New System.Drawing.Point(536, 34)
        Me.Button_Compiler.Name = "Button_Compiler"
        Me.Button_Compiler.Size = New System.Drawing.Size(31, 24)
        Me.Button_Compiler.TabIndex = 8
        Me.Button_Compiler.Text = "..."
        Me.Button_Compiler.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 59)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(177, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Include directory (Separate by ';'):"
        '
        'TextBox_IncludeFolder
        '
        Me.TextBox_IncludeFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_IncludeFolder.BackColor = System.Drawing.Color.White
        Me.TextBox_IncludeFolder.Location = New System.Drawing.Point(6, 75)
        Me.TextBox_IncludeFolder.Name = "TextBox_IncludeFolder"
        Me.TextBox_IncludeFolder.Size = New System.Drawing.Size(524, 22)
        Me.TextBox_IncludeFolder.TabIndex = 10
        '
        'Button_IncludeFolder
        '
        Me.Button_IncludeFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_IncludeFolder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_IncludeFolder.Location = New System.Drawing.Point(536, 73)
        Me.Button_IncludeFolder.Name = "Button_IncludeFolder"
        Me.Button_IncludeFolder.Size = New System.Drawing.Size(31, 24)
        Me.Button_IncludeFolder.TabIndex = 11
        Me.Button_IncludeFolder.Text = "..."
        Me.Button_IncludeFolder.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 98)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(96, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Output directory:"
        '
        'TextBox_OutputFolder
        '
        Me.TextBox_OutputFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_OutputFolder.BackColor = System.Drawing.Color.White
        Me.TextBox_OutputFolder.Location = New System.Drawing.Point(6, 114)
        Me.TextBox_OutputFolder.Name = "TextBox_OutputFolder"
        Me.TextBox_OutputFolder.Size = New System.Drawing.Size(524, 22)
        Me.TextBox_OutputFolder.TabIndex = 14
        '
        'Button_OutputFolder
        '
        Me.Button_OutputFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_OutputFolder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_OutputFolder.Location = New System.Drawing.Point(536, 112)
        Me.Button_OutputFolder.Name = "Button_OutputFolder"
        Me.Button_OutputFolder.Size = New System.Drawing.Size(31, 24)
        Me.Button_OutputFolder.TabIndex = 15
        Me.Button_OutputFolder.Text = "..."
        Me.Button_OutputFolder.UseVisualStyleBackColor = True
        '
        'ListBox_Configs
        '
        Me.ListBox_Configs.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ListBox_Configs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox_Configs.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox_Configs.FormattingEnabled = True
        Me.ListBox_Configs.HorizontalScrollbar = True
        Me.ListBox_Configs.ItemHeight = 21
        Me.ListBox_Configs.Location = New System.Drawing.Point(3, 3)
        Me.ListBox_Configs.Name = "ListBox_Configs"
        Me.ListBox_Configs.Size = New System.Drawing.Size(144, 476)
        Me.ListBox_Configs.TabIndex = 0
        '
        'TabPage_Plugins
        '
        Me.TabPage_Plugins.Controls.Add(Me.LinkLabel_MorePlugins)
        Me.TabPage_Plugins.Controls.Add(Me.ListView_Plugins)
        Me.TabPage_Plugins.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Plugins.Name = "TabPage_Plugins"
        Me.TabPage_Plugins.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Plugins.Size = New System.Drawing.Size(752, 482)
        Me.TabPage_Plugins.TabIndex = 2
        Me.TabPage_Plugins.Text = "Plugins"
        Me.TabPage_Plugins.UseVisualStyleBackColor = True
        '
        'LinkLabel_MorePlugins
        '
        Me.LinkLabel_MorePlugins.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel_MorePlugins.AutoSize = True
        Me.LinkLabel_MorePlugins.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel_MorePlugins.Location = New System.Drawing.Point(6, 461)
        Me.LinkLabel_MorePlugins.Margin = New System.Windows.Forms.Padding(3)
        Me.LinkLabel_MorePlugins.Name = "LinkLabel_MorePlugins"
        Me.LinkLabel_MorePlugins.Size = New System.Drawing.Size(105, 13)
        Me.LinkLabel_MorePlugins.TabIndex = 1
        Me.LinkLabel_MorePlugins.TabStop = True
        Me.LinkLabel_MorePlugins.Text = "Get more plugins..."
        '
        'ListView_Plugins
        '
        Me.ListView_Plugins.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_Plugins.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7})
        Me.ListView_Plugins.ContextMenuStrip = Me.ContextMenuStrip_Plugins
        Me.ListView_Plugins.Location = New System.Drawing.Point(6, 6)
        Me.ListView_Plugins.Name = "ListView_Plugins"
        Me.ListView_Plugins.Size = New System.Drawing.Size(740, 449)
        Me.ListView_Plugins.TabIndex = 0
        Me.ListView_Plugins.UseCompatibleStateImageBehavior = False
        Me.ListView_Plugins.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "File"
        Me.ColumnHeader1.Width = 75
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Name"
        Me.ColumnHeader2.Width = 75
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Author"
        Me.ColumnHeader3.Width = 75
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Description"
        Me.ColumnHeader4.Width = 75
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Version"
        Me.ColumnHeader5.Width = 75
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "URL"
        Me.ColumnHeader6.Width = 75
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Enabled"
        Me.ColumnHeader7.Width = 75
        '
        'TabPage_Database
        '
        Me.TabPage_Database.Controls.Add(Me.DatabaseListBox_Database)
        Me.TabPage_Database.Controls.Add(Me.Button_Refresh)
        Me.TabPage_Database.Controls.Add(Me.TableLayoutPanel4)
        Me.TabPage_Database.Controls.Add(Me.Button_AddDatabaseItem)
        Me.TabPage_Database.Controls.Add(Me.TableLayoutPanel3)
        Me.TabPage_Database.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Database.Name = "TabPage_Database"
        Me.TabPage_Database.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Database.Size = New System.Drawing.Size(752, 482)
        Me.TabPage_Database.TabIndex = 3
        Me.TabPage_Database.Text = "Database"
        Me.TabPage_Database.UseVisualStyleBackColor = True
        '
        'DatabaseListBox_Database
        '
        Me.DatabaseListBox_Database.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DatabaseListBox_Database.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.DatabaseListBox_Database.FormattingEnabled = True
        Me.DatabaseListBox_Database.ItemHeight = 32
        Me.DatabaseListBox_Database.Location = New System.Drawing.Point(6, 75)
        Me.DatabaseListBox_Database.Name = "DatabaseListBox_Database"
        Me.DatabaseListBox_Database.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.DatabaseListBox_Database.Size = New System.Drawing.Size(740, 313)
        Me.DatabaseListBox_Database.TabIndex = 7
        '
        'Button_Refresh
        '
        Me.Button_Refresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Refresh.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Refresh.Location = New System.Drawing.Point(646, 394)
        Me.Button_Refresh.Name = "Button_Refresh"
        Me.Button_Refresh.Size = New System.Drawing.Size(100, 23)
        Me.Button_Refresh.TabIndex = 6
        Me.Button_Refresh.Text = "Refresh"
        Me.Button_Refresh.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel4.ColumnCount = 2
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.ClassPictureBoxQuality2, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.Label18, 1, 0)
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(6, 6)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 1
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(740, 63)
        Me.TableLayoutPanel4.TabIndex = 5
        '
        'ClassPictureBoxQuality2
        '
        Me.ClassPictureBoxQuality2.Image = Global.BasicPawn.My.Resources.Resources.Bmp_DriveEncryption
        Me.ClassPictureBoxQuality2.Location = New System.Drawing.Point(3, 3)
        Me.ClassPictureBoxQuality2.m_HighQuality = True
        Me.ClassPictureBoxQuality2.Name = "ClassPictureBoxQuality2"
        Me.ClassPictureBoxQuality2.Size = New System.Drawing.Size(48, 48)
        Me.ClassPictureBoxQuality2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.ClassPictureBoxQuality2.TabIndex = 2
        Me.ClassPictureBoxQuality2.TabStop = False
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(60, 6)
        Me.Label18.Margin = New System.Windows.Forms.Padding(6)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(664, 26)
        Me.Label18.TabIndex = 3
        Me.Label18.Text = resources.GetString("Label18.Text")
        '
        'Button_AddDatabaseItem
        '
        Me.Button_AddDatabaseItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_AddDatabaseItem.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_AddDatabaseItem.Location = New System.Drawing.Point(540, 394)
        Me.Button_AddDatabaseItem.Name = "Button_AddDatabaseItem"
        Me.Button_AddDatabaseItem.Size = New System.Drawing.Size(100, 23)
        Me.Button_AddDatabaseItem.TabIndex = 4
        Me.Button_AddDatabaseItem.Text = "Add"
        Me.Button_AddDatabaseItem.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.ClassPictureBoxQuality1, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.Label17, 1, 0)
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(6, 420)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(740, 54)
        Me.TableLayoutPanel3.TabIndex = 1
        '
        'ClassPictureBoxQuality1
        '
        Me.ClassPictureBoxQuality1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClassPictureBoxQuality1.Image = Global.BasicPawn.My.Resources.Resources.Bmp_ShieldWarn
        Me.ClassPictureBoxQuality1.Location = New System.Drawing.Point(3, 3)
        Me.ClassPictureBoxQuality1.m_HighQuality = True
        Me.ClassPictureBoxQuality1.Name = "ClassPictureBoxQuality1"
        Me.ClassPictureBoxQuality1.Size = New System.Drawing.Size(32, 32)
        Me.ClassPictureBoxQuality1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.ClassPictureBoxQuality1.TabIndex = 2
        Me.ClassPictureBoxQuality1.TabStop = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(44, 6)
        Me.Label17.Margin = New System.Windows.Forms.Padding(6)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(688, 26)
        Me.Label17.TabIndex = 3
        Me.Label17.Text = "Loaded BasicPawn plugins are able to read stored database entries. Make sure all " &
    "installed plugins are from a trustworthy publisher to prevent theft."
        '
        'CheckBox_WindowsToolTipDisplayTop
        '
        Me.CheckBox_WindowsToolTipDisplayTop.AutoSize = True
        Me.CheckBox_WindowsToolTipDisplayTop.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_WindowsToolTipDisplayTop.Location = New System.Drawing.Point(64, 171)
        Me.CheckBox_WindowsToolTipDisplayTop.Margin = New System.Windows.Forms.Padding(64, 3, 3, 3)
        Me.CheckBox_WindowsToolTipDisplayTop.Name = "CheckBox_WindowsToolTipDisplayTop"
        Me.CheckBox_WindowsToolTipDisplayTop.Size = New System.Drawing.Size(210, 18)
        Me.CheckBox_WindowsToolTipDisplayTop.TabIndex = 28
        Me.CheckBox_WindowsToolTipDisplayTop.Text = "Display tooltip above caret instead"
        Me.CheckBox_WindowsToolTipDisplayTop.UseVisualStyleBackColor = True
        '
        'FormSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Button_Cancel
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.Button_Apply)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.TabControl1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(640, 480)
        Me.Name = "FormSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Settings"
        Me.ContextMenuStrip_Plugins.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_Settings.ResumeLayout(False)
        Me.ClassTabControlColor1.ResumeLayout(False)
        Me.TabPage_General.ResumeLayout(False)
        Me.TabPage_General.PerformLayout()
        Me.TabPage_Editor.ResumeLayout(False)
        Me.TabPage_Editor.PerformLayout()
        CType(Me.NumericUpDown_TabsToSpaces, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage_Syntax.ResumeLayout(False)
        Me.TabPage_Syntax.PerformLayout()
        Me.TabPage_Autocomplete.ResumeLayout(False)
        Me.TabPage_Autocomplete.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.TabPage_Configs.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.Panel17.ResumeLayout(False)
        Me.Panel17.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.GroupBox_ConfigSettings.ResumeLayout(False)
        Me.GroupBox_ConfigSettings.PerformLayout()
        Me.Panel11.ResumeLayout(False)
        Me.Panel11.PerformLayout()
        Me.Panel10.ResumeLayout(False)
        Me.Panel10.PerformLayout()
        Me.Panel9.ResumeLayout(False)
        Me.Panel9.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.TabControl2.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabPage_Plugins.ResumeLayout(False)
        Me.TabPage_Plugins.PerformLayout()
        Me.TabPage_Database.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        CType(Me.ClassPictureBoxQuality2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        CType(Me.ClassPictureBoxQuality1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As ClassTabControlColor
    Friend WithEvents TabPage_Settings As TabPage
    Friend WithEvents TabPage_Configs As TabPage
    Friend WithEvents CheckBox_OnScreenIntelliSense As CheckBox
    Friend WithEvents Button_Cancel As Button
    Friend WithEvents Button_Apply As Button
    Friend WithEvents Button_ConfigAdd As Button
    Friend WithEvents Button_ConfigRemove As Button
    Friend WithEvents TextBox_ConfigName As TextBox
    Friend WithEvents Label_ConfigName As Label
    Friend WithEvents ListBox_Configs As ListBox
    Friend WithEvents GroupBox_ConfigSettings As GroupBox
    Friend WithEvents RadioButton_ConfigSettingManual As RadioButton
    Friend WithEvents RadioButton_ConfigSettingAutomatic As RadioButton
    Friend WithEvents TextBox_IncludeFolder As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBox_CompilerPath As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Button_SaveConfig As Button
    Friend WithEvents Button_IncludeFolder As Button
    Friend WithEvents Button_Compiler As Button
    Friend WithEvents Button_OutputFolder As Button
    Friend WithEvents TextBox_OutputFolder As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents CheckBox_FullAutcompleteMethods As CheckBox
    Friend WithEvents Button_ConfigCopy As Button
    Friend WithEvents Button_ConfigRename As Button
    Friend WithEvents CheckBox_DoubleClickMark As CheckBox
    Friend WithEvents CheckBox_CommentsMethodIntelliSense As CheckBox
    Friend WithEvents CheckBox_CommentsAutocompleteIntelliSense As CheckBox
    Friend WithEvents CheckBox_FullAutocompleteReTagging As CheckBox
    Friend WithEvents Label_Font As Label
    Friend WithEvents Button_Font As Button
    Friend WithEvents CheckBox_InvertedColors As CheckBox
    Friend WithEvents CheckBox_CaseSensitive As CheckBox
    Friend WithEvents CheckBox_WindowsToolTipPopup As CheckBox
    Friend WithEvents TextBox_Shell As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents LinkLabel_ShowShellArguments As LinkLabel
    Friend WithEvents CheckBox_AutoMark As CheckBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Button_ServerFolder As Button
    Friend WithEvents TextBox_ServerFolder As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Button_SourceModFolder As Button
    Friend WithEvents TextBox_SourceModFolder As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Panel11 As Panel
    Friend WithEvents Panel10 As Panel
    Friend WithEvents Panel9 As Panel
    Friend WithEvents Panel13 As Panel
    Friend WithEvents Panel12 As Panel
    Friend WithEvents TabPage_Plugins As TabPage
    Friend WithEvents ListView_Plugins As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ColumnHeader5 As ColumnHeader
    Friend WithEvents ColumnHeader6 As ColumnHeader
    Friend WithEvents CheckBox_AlwaysNewInstance As CheckBox
    Friend WithEvents CheckBox_VarAutocompleteShowObjectBrowser As CheckBox
    Friend WithEvents CheckBox_AlwaysLoadDefaultIncludes As CheckBox
    Friend WithEvents CheckBox_SwitchTabToAutocomplete As CheckBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Panel17 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents TabPage_Database As TabPage
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents ClassPictureBoxQuality1 As ClassPictureBoxQuality
    Friend WithEvents Label17 As Label
    Friend WithEvents ClassPictureBoxQuality2 As ClassPictureBoxQuality
    Friend WithEvents Button_AddDatabaseItem As Button
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents Label18 As Label
    Friend WithEvents Button_Refresh As Button
    Friend WithEvents CheckBox_AutoShowStartPage As CheckBox
    Friend WithEvents CheckBox_WindowsToolTipAnimations As CheckBox
    Friend WithEvents NumericUpDown_TabsToSpaces As NumericUpDown
    Friend WithEvents CheckBox_TabsToSpace As CheckBox
    Friend WithEvents Label19 As Label
    Friend WithEvents CheckBox_AssociateSourcePawn As CheckBox
    Friend WithEvents CheckBox_AssociateIncludes As CheckBox
    Friend WithEvents CheckBox_AssociateBasicPawnProject As CheckBox
    Friend WithEvents CheckBox_AssociateAmxMod As CheckBox
    Friend WithEvents ContextMenuStrip_Plugins As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_PluginsRefresh As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_PluginsEnable As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_PluginsDisable As ToolStripMenuItem
    Friend WithEvents ColumnHeader7 As ColumnHeader
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents TabControl2 As ClassTabControlColor
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Label21 As Label
    Friend WithEvents ComboBox_COOptimizationLevelSP As ComboBox
    Friend WithEvents ComboBox_COVerbosityLevelSP As ComboBox
    Friend WithEvents Label22 As Label
    Friend WithEvents Label23 As Label
    Friend WithEvents ComboBox_COTreatWarningsAsErrorsSP As ComboBox
    Friend WithEvents Label24 As Label
    Friend WithEvents TextBoxEx_COIgnoredWarningsSP As ClassTextboxWatermark
    Friend WithEvents TextBoxEx_CODefineConstantsSP As ClassTextboxWatermark
    Friend WithEvents Label25 As Label
    Friend WithEvents Label31 As Label
    Friend WithEvents ComboBox_COSymbolicInformationAMXX As ComboBox
    Friend WithEvents Label26 As Label
    Friend WithEvents TextBoxEx_CODefineConstantsAMXX As ClassTextboxWatermark
    Friend WithEvents TextBoxEx_COIgnoredWarningsAMXX As ClassTextboxWatermark
    Friend WithEvents Label27 As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents ComboBox_COTreatWarningsAsErrorsAMXX As ComboBox
    Friend WithEvents Label29 As Label
    Friend WithEvents ComboBox_COVerbosityLevelAMXX As ComboBox
    Friend WithEvents CheckBox_WindowsToolTipNewlineMethods As CheckBox
    Friend WithEvents Label30 As Label
    Friend WithEvents Button_ViewErrorLog As Button
    Friend WithEvents Button_ClearErrorLog As Button
    Friend WithEvents LinkLabel_MorePlugins As LinkLabel
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Button_KnownFileRemove As Button
    Friend WithEvents Button_KnownFileAdd As Button
    Friend WithEvents ListView_KnownFiles As ListView
    Friend WithEvents ColumnHeader8 As ColumnHeader
    Friend WithEvents Label32 As Label
    Friend WithEvents Button_CustomSyntax As Button
    Friend WithEvents TextBox_CustomSyntax As TextBox
    Friend WithEvents LinkLabel_DefaultSyntax As LinkLabel
    Friend WithEvents LinkLabel_MoreStyles As LinkLabel
    Friend WithEvents CheckBox_OnlyUpdateSyntaxWhenFocused As CheckBox
    Friend WithEvents CheckBox_AutoOpenProjectFiles As CheckBox
    Friend WithEvents CheckBox_AutoCloseBrackets As CheckBox
    Friend WithEvents CheckBox_AutoCloseStrings As CheckBox
    Friend WithEvents CheckBox_AutoIndentBrackets As CheckBox
    Friend WithEvents CheckBox_AutoHoverScroll As CheckBox
    Friend WithEvents DatabaseListBox_Database As ClassDatabaseListBox
    Friend WithEvents ClassTabControlColor1 As ClassTabControlColor
    Friend WithEvents TabPage_General As TabPage
    Friend WithEvents TabPage_Editor As TabPage
    Friend WithEvents TabPage_Syntax As TabPage
    Friend WithEvents TabPage_Autocomplete As TabPage
    Friend WithEvents CheckBox_RememberFolds As CheckBox
    Friend WithEvents ToolStripMenuItem_OpenUrl As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents RadioButton_VarParseTab As RadioButton
    Friend WithEvents RadioButton_VarParseTabInc As RadioButton
    Friend WithEvents RadioButton_VarParseAll As RadioButton
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox_ClientFolder As TextBox
    Friend WithEvents Button_ClientFolder As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents TextBox_AutoAssignPaths As TextBox
    Friend WithEvents Button_AutoAssignPaths As Button
    Friend WithEvents Label20 As Label
    Friend WithEvents ComboBox_Language As ComboBox
    Friend WithEvents CheckBox_ConfigIsDefault As CheckBox
    Friend WithEvents CheckBox_WindowsToolTipDisplayTop As CheckBox
End Class
