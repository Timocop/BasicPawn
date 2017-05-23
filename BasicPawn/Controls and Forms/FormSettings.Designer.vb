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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSettings))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage_Settings = New System.Windows.Forms.TabPage()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.CheckBox_CatchExceptions = New System.Windows.Forms.CheckBox()
        Me.CheckBox_EntitiesEnableColor = New System.Windows.Forms.CheckBox()
        Me.CheckBox_EntitiesEnableShowNewEnts = New System.Windows.Forms.CheckBox()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CheckBox_SwitchTabToAutocomplete = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AlwaysLoadDefaultIncludes = New System.Windows.Forms.CheckBox()
        Me.Panel15 = New System.Windows.Forms.Panel()
        Me.CheckBox_VarAutocompleteShowObjectBrowser = New System.Windows.Forms.CheckBox()
        Me.CheckBox_CurrentSourceVarAutocomplete = New System.Windows.Forms.CheckBox()
        Me.CheckBox_OnScreenIntelliSense = New System.Windows.Forms.CheckBox()
        Me.CheckBox_FullAutcompleteMethods = New System.Windows.Forms.CheckBox()
        Me.CheckBox_CommentsMethodIntelliSense = New System.Windows.Forms.CheckBox()
        Me.CheckBox_CommentsAutocompleteIntelliSense = New System.Windows.Forms.CheckBox()
        Me.CheckBox_FullAutocompleteReTagging = New System.Windows.Forms.CheckBox()
        Me.CheckBox_WindowsToolTipPopup = New System.Windows.Forms.CheckBox()
        Me.CheckBox_CaseSensitive = New System.Windows.Forms.CheckBox()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CheckBox_DoubleClickMark = New System.Windows.Forms.CheckBox()
        Me.CheckBox_AutoMark = New System.Windows.Forms.CheckBox()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Button_Font = New System.Windows.Forms.Button()
        Me.Label_Font = New System.Windows.Forms.Label()
        Me.CheckBox_InvertedColors = New System.Windows.Forms.CheckBox()
        Me.Panel14 = New System.Windows.Forms.Panel()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.CheckBox_AlwaysNewInstance = New System.Windows.Forms.CheckBox()
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
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TextBox_SyntaxPath = New System.Windows.Forms.TextBox()
        Me.Button_SyntaxPath = New System.Windows.Forms.Button()
        Me.LinkLabel_SyntaxDefault = New System.Windows.Forms.LinkLabel()
        Me.Panel10 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel12 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox_GameFolder = New System.Windows.Forms.TextBox()
        Me.Button_GameFolder = New System.Windows.Forms.Button()
        Me.TextBox_SourceModFolder = New System.Windows.Forms.TextBox()
        Me.Button_SourceModFolder = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Panel9 = New System.Windows.Forms.Panel()
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
        Me.ListView_Plugins = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TabPage_Database = New System.Windows.Forms.TabPage()
        Me.Button_Refresh = New System.Windows.Forms.Button()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.ClassPictureBoxQuality2 = New BasicPawn.ClassPictureBoxQuality()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Button_AddDatabaseItem = New System.Windows.Forms.Button()
        Me.DatabaseViewer = New BasicPawn.ClassDatabaseViewer()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.ClassPictureBoxQuality1 = New BasicPawn.ClassPictureBoxQuality()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.Button_Apply = New System.Windows.Forms.Button()
        Me.CheckBox_AutoShowStartPage = New System.Windows.Forms.CheckBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage_Settings.SuspendLayout()
        Me.Panel8.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel14.SuspendLayout()
        Me.TabPage_Configs.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.Panel17.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox_ConfigSettings.SuspendLayout()
        Me.Panel11.SuspendLayout()
        Me.Panel10.SuspendLayout()
        Me.Panel9.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabPage_Plugins.SuspendLayout()
        Me.TabPage_Database.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        CType(Me.ClassPictureBoxQuality2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel3.SuspendLayout()
        CType(Me.ClassPictureBoxQuality1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.TabControl1.Size = New System.Drawing.Size(610, 750)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage_Settings
        '
        Me.TabPage_Settings.AutoScroll = True
        Me.TabPage_Settings.Controls.Add(Me.Panel8)
        Me.TabPage_Settings.Controls.Add(Me.Panel5)
        Me.TabPage_Settings.Controls.Add(Me.Panel6)
        Me.TabPage_Settings.Controls.Add(Me.Panel7)
        Me.TabPage_Settings.Controls.Add(Me.Panel14)
        Me.TabPage_Settings.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Settings.Name = "TabPage_Settings"
        Me.TabPage_Settings.Size = New System.Drawing.Size(602, 724)
        Me.TabPage_Settings.TabIndex = 0
        Me.TabPage_Settings.Text = "Settings"
        Me.TabPage_Settings.UseVisualStyleBackColor = True
        '
        'Panel8
        '
        Me.Panel8.AutoSize = True
        Me.Panel8.Controls.Add(Me.Label14)
        Me.Panel8.Controls.Add(Me.Panel4)
        Me.Panel8.Controls.Add(Me.CheckBox_CatchExceptions)
        Me.Panel8.Controls.Add(Me.CheckBox_EntitiesEnableColor)
        Me.Panel8.Controls.Add(Me.CheckBox_EntitiesEnableShowNewEnts)
        Me.Panel8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel8.Location = New System.Drawing.Point(0, 492)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(602, 92)
        Me.Panel8.TabIndex = 25
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label14.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(0, 1)
        Me.Label14.Margin = New System.Windows.Forms.Padding(3)
        Me.Label14.Name = "Label14"
        Me.Label14.Padding = New System.Windows.Forms.Padding(6, 3, 0, 0)
        Me.Label14.Size = New System.Drawing.Size(65, 16)
        Me.Label14.TabIndex = 18
        Me.Label14.Text = "Debugger"
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(602, 1)
        Me.Panel4.TabIndex = 10
        '
        'CheckBox_CatchExceptions
        '
        Me.CheckBox_CatchExceptions.AutoSize = True
        Me.CheckBox_CatchExceptions.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_CatchExceptions.Location = New System.Drawing.Point(6, 23)
        Me.CheckBox_CatchExceptions.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_CatchExceptions.Name = "CheckBox_CatchExceptions"
        Me.CheckBox_CatchExceptions.Size = New System.Drawing.Size(119, 18)
        Me.CheckBox_CatchExceptions.TabIndex = 19
        Me.CheckBox_CatchExceptions.Text = "Catch exceptions"
        Me.CheckBox_CatchExceptions.UseVisualStyleBackColor = True
        '
        'CheckBox_EntitiesEnableColor
        '
        Me.CheckBox_EntitiesEnableColor.AutoSize = True
        Me.CheckBox_EntitiesEnableColor.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_EntitiesEnableColor.Location = New System.Drawing.Point(6, 47)
        Me.CheckBox_EntitiesEnableColor.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_EntitiesEnableColor.Name = "CheckBox_EntitiesEnableColor"
        Me.CheckBox_EntitiesEnableColor.Size = New System.Drawing.Size(221, 18)
        Me.CheckBox_EntitiesEnableColor.TabIndex = 20
        Me.CheckBox_EntitiesEnableColor.Text = "Colorize created and deleted entities"
        Me.CheckBox_EntitiesEnableColor.UseVisualStyleBackColor = True
        '
        'CheckBox_EntitiesEnableShowNewEnts
        '
        Me.CheckBox_EntitiesEnableShowNewEnts.AutoSize = True
        Me.CheckBox_EntitiesEnableShowNewEnts.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_EntitiesEnableShowNewEnts.Location = New System.Drawing.Point(6, 71)
        Me.CheckBox_EntitiesEnableShowNewEnts.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_EntitiesEnableShowNewEnts.Name = "CheckBox_EntitiesEnableShowNewEnts"
        Me.CheckBox_EntitiesEnableShowNewEnts.Size = New System.Drawing.Size(211, 18)
        Me.CheckBox_EntitiesEnableShowNewEnts.TabIndex = 21
        Me.CheckBox_EntitiesEnableShowNewEnts.Text = "Automatically scroll to new entities"
        Me.CheckBox_EntitiesEnableShowNewEnts.UseVisualStyleBackColor = True
        '
        'Panel5
        '
        Me.Panel5.AutoSize = True
        Me.Panel5.Controls.Add(Me.Label1)
        Me.Panel5.Controls.Add(Me.CheckBox_SwitchTabToAutocomplete)
        Me.Panel5.Controls.Add(Me.CheckBox_AlwaysLoadDefaultIncludes)
        Me.Panel5.Controls.Add(Me.Panel15)
        Me.Panel5.Controls.Add(Me.CheckBox_VarAutocompleteShowObjectBrowser)
        Me.Panel5.Controls.Add(Me.CheckBox_CurrentSourceVarAutocomplete)
        Me.Panel5.Controls.Add(Me.CheckBox_OnScreenIntelliSense)
        Me.Panel5.Controls.Add(Me.CheckBox_FullAutcompleteMethods)
        Me.Panel5.Controls.Add(Me.CheckBox_CommentsMethodIntelliSense)
        Me.Panel5.Controls.Add(Me.CheckBox_CommentsAutocompleteIntelliSense)
        Me.Panel5.Controls.Add(Me.CheckBox_FullAutocompleteReTagging)
        Me.Panel5.Controls.Add(Me.CheckBox_WindowsToolTipPopup)
        Me.Panel5.Controls.Add(Me.CheckBox_CaseSensitive)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 208)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(602, 284)
        Me.Panel5.TabIndex = 22
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(0, 1)
        Me.Label1.Margin = New System.Windows.Forms.Padding(3)
        Me.Label1.Name = "Label1"
        Me.Label1.Padding = New System.Windows.Forms.Padding(6, 3, 0, 0)
        Me.Label1.Size = New System.Drawing.Size(161, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Autocomplete && IntelliSense"
        '
        'CheckBox_SwitchTabToAutocomplete
        '
        Me.CheckBox_SwitchTabToAutocomplete.AutoSize = True
        Me.CheckBox_SwitchTabToAutocomplete.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_SwitchTabToAutocomplete.Location = New System.Drawing.Point(6, 263)
        Me.CheckBox_SwitchTabToAutocomplete.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_SwitchTabToAutocomplete.Name = "CheckBox_SwitchTabToAutocomplete"
        Me.CheckBox_SwitchTabToAutocomplete.Size = New System.Drawing.Size(326, 18)
        Me.CheckBox_SwitchTabToAutocomplete.TabIndex = 20
        Me.CheckBox_SwitchTabToAutocomplete.Text = "Automatically switch tab to 'Autocomplete && IntelliSense'"
        Me.CheckBox_SwitchTabToAutocomplete.UseVisualStyleBackColor = True
        '
        'CheckBox_AlwaysLoadDefaultIncludes
        '
        Me.CheckBox_AlwaysLoadDefaultIncludes.AutoSize = True
        Me.CheckBox_AlwaysLoadDefaultIncludes.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AlwaysLoadDefaultIncludes.Location = New System.Drawing.Point(6, 23)
        Me.CheckBox_AlwaysLoadDefaultIncludes.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AlwaysLoadDefaultIncludes.Name = "CheckBox_AlwaysLoadDefaultIncludes"
        Me.CheckBox_AlwaysLoadDefaultIncludes.Size = New System.Drawing.Size(198, 18)
        Me.CheckBox_AlwaysLoadDefaultIncludes.TabIndex = 19
        Me.CheckBox_AlwaysLoadDefaultIncludes.Text = "Always load default include files"
        Me.CheckBox_AlwaysLoadDefaultIncludes.UseVisualStyleBackColor = True
        '
        'Panel15
        '
        Me.Panel15.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel15.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel15.Location = New System.Drawing.Point(0, 0)
        Me.Panel15.Name = "Panel15"
        Me.Panel15.Size = New System.Drawing.Size(602, 1)
        Me.Panel15.TabIndex = 10
        '
        'CheckBox_VarAutocompleteShowObjectBrowser
        '
        Me.CheckBox_VarAutocompleteShowObjectBrowser.AutoSize = True
        Me.CheckBox_VarAutocompleteShowObjectBrowser.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_VarAutocompleteShowObjectBrowser.Location = New System.Drawing.Point(6, 239)
        Me.CheckBox_VarAutocompleteShowObjectBrowser.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_VarAutocompleteShowObjectBrowser.Name = "CheckBox_VarAutocompleteShowObjectBrowser"
        Me.CheckBox_VarAutocompleteShowObjectBrowser.Size = New System.Drawing.Size(224, 18)
        Me.CheckBox_VarAutocompleteShowObjectBrowser.TabIndex = 18
        Me.CheckBox_VarAutocompleteShowObjectBrowser.Text = "Show variables in the Object Browser"
        Me.CheckBox_VarAutocompleteShowObjectBrowser.UseVisualStyleBackColor = True
        '
        'CheckBox_CurrentSourceVarAutocomplete
        '
        Me.CheckBox_CurrentSourceVarAutocomplete.AutoSize = True
        Me.CheckBox_CurrentSourceVarAutocomplete.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_CurrentSourceVarAutocomplete.Location = New System.Drawing.Point(6, 215)
        Me.CheckBox_CurrentSourceVarAutocomplete.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_CurrentSourceVarAutocomplete.Name = "CheckBox_CurrentSourceVarAutocomplete"
        Me.CheckBox_CurrentSourceVarAutocomplete.Size = New System.Drawing.Size(504, 18)
        Me.CheckBox_CurrentSourceVarAutocomplete.TabIndex = 17
        Me.CheckBox_CurrentSourceVarAutocomplete.Text = "Only search in the current opened source file for variables used for variable aut" &
    "ocompletion"
        Me.CheckBox_CurrentSourceVarAutocomplete.UseVisualStyleBackColor = True
        '
        'CheckBox_OnScreenIntelliSense
        '
        Me.CheckBox_OnScreenIntelliSense.AutoSize = True
        Me.CheckBox_OnScreenIntelliSense.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_OnScreenIntelliSense.Location = New System.Drawing.Point(6, 47)
        Me.CheckBox_OnScreenIntelliSense.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_OnScreenIntelliSense.Name = "CheckBox_OnScreenIntelliSense"
        Me.CheckBox_OnScreenIntelliSense.Size = New System.Drawing.Size(129, 18)
        Me.CheckBox_OnScreenIntelliSense.TabIndex = 2
        Me.CheckBox_OnScreenIntelliSense.Text = "Enable IntelliSense"
        Me.CheckBox_OnScreenIntelliSense.UseVisualStyleBackColor = True
        '
        'CheckBox_FullAutcompleteMethods
        '
        Me.CheckBox_FullAutcompleteMethods.AutoSize = True
        Me.CheckBox_FullAutcompleteMethods.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_FullAutcompleteMethods.Location = New System.Drawing.Point(6, 143)
        Me.CheckBox_FullAutcompleteMethods.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_FullAutcompleteMethods.Name = "CheckBox_FullAutcompleteMethods"
        Me.CheckBox_FullAutcompleteMethods.Size = New System.Drawing.Size(202, 18)
        Me.CheckBox_FullAutcompleteMethods.TabIndex = 3
        Me.CheckBox_FullAutcompleteMethods.Text = "Full autocompletion for methods"
        Me.CheckBox_FullAutcompleteMethods.UseVisualStyleBackColor = True
        '
        'CheckBox_CommentsMethodIntelliSense
        '
        Me.CheckBox_CommentsMethodIntelliSense.AutoSize = True
        Me.CheckBox_CommentsMethodIntelliSense.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_CommentsMethodIntelliSense.Location = New System.Drawing.Point(32, 71)
        Me.CheckBox_CommentsMethodIntelliSense.Margin = New System.Windows.Forms.Padding(32, 3, 3, 3)
        Me.CheckBox_CommentsMethodIntelliSense.Name = "CheckBox_CommentsMethodIntelliSense"
        Me.CheckBox_CommentsMethodIntelliSense.Size = New System.Drawing.Size(319, 18)
        Me.CheckBox_CommentsMethodIntelliSense.TabIndex = 6
        Me.CheckBox_CommentsMethodIntelliSense.Text = "Display comments for methods in on-screen IntelliSense"
        Me.CheckBox_CommentsMethodIntelliSense.UseVisualStyleBackColor = True
        '
        'CheckBox_CommentsAutocompleteIntelliSense
        '
        Me.CheckBox_CommentsAutocompleteIntelliSense.AutoSize = True
        Me.CheckBox_CommentsAutocompleteIntelliSense.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_CommentsAutocompleteIntelliSense.Location = New System.Drawing.Point(32, 95)
        Me.CheckBox_CommentsAutocompleteIntelliSense.Margin = New System.Windows.Forms.Padding(32, 3, 3, 3)
        Me.CheckBox_CommentsAutocompleteIntelliSense.Name = "CheckBox_CommentsAutocompleteIntelliSense"
        Me.CheckBox_CommentsAutocompleteIntelliSense.Size = New System.Drawing.Size(356, 18)
        Me.CheckBox_CommentsAutocompleteIntelliSense.TabIndex = 7
        Me.CheckBox_CommentsAutocompleteIntelliSense.Text = "Display comments for autocompletion in on-screen IntelliSense"
        Me.CheckBox_CommentsAutocompleteIntelliSense.UseVisualStyleBackColor = True
        '
        'CheckBox_FullAutocompleteReTagging
        '
        Me.CheckBox_FullAutocompleteReTagging.AutoSize = True
        Me.CheckBox_FullAutocompleteReTagging.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_FullAutocompleteReTagging.Location = New System.Drawing.Point(6, 167)
        Me.CheckBox_FullAutocompleteReTagging.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_FullAutocompleteReTagging.Name = "CheckBox_FullAutocompleteReTagging"
        Me.CheckBox_FullAutocompleteReTagging.Size = New System.Drawing.Size(415, 18)
        Me.CheckBox_FullAutocompleteReTagging.TabIndex = 8
        Me.CheckBox_FullAutocompleteReTagging.Text = "Full autocompletion for enums using re-tagging (SourcePawn <1.6 Syntax)"
        Me.CheckBox_FullAutocompleteReTagging.UseVisualStyleBackColor = True
        '
        'CheckBox_WindowsToolTipPopup
        '
        Me.CheckBox_WindowsToolTipPopup.AutoSize = True
        Me.CheckBox_WindowsToolTipPopup.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_WindowsToolTipPopup.Location = New System.Drawing.Point(32, 119)
        Me.CheckBox_WindowsToolTipPopup.Margin = New System.Windows.Forms.Padding(32, 3, 3, 3)
        Me.CheckBox_WindowsToolTipPopup.Name = "CheckBox_WindowsToolTipPopup"
        Me.CheckBox_WindowsToolTipPopup.Size = New System.Drawing.Size(127, 18)
        Me.CheckBox_WindowsToolTipPopup.TabIndex = 16
        Me.CheckBox_WindowsToolTipPopup.Text = "Use tooltip popup"
        Me.CheckBox_WindowsToolTipPopup.UseVisualStyleBackColor = True
        '
        'CheckBox_CaseSensitive
        '
        Me.CheckBox_CaseSensitive.AutoSize = True
        Me.CheckBox_CaseSensitive.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_CaseSensitive.Location = New System.Drawing.Point(6, 191)
        Me.CheckBox_CaseSensitive.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_CaseSensitive.Name = "CheckBox_CaseSensitive"
        Me.CheckBox_CaseSensitive.Size = New System.Drawing.Size(103, 18)
        Me.CheckBox_CaseSensitive.TabIndex = 15
        Me.CheckBox_CaseSensitive.Text = "Case sensitive"
        Me.CheckBox_CaseSensitive.UseVisualStyleBackColor = True
        '
        'Panel6
        '
        Me.Panel6.AutoSize = True
        Me.Panel6.Controls.Add(Me.Label7)
        Me.Panel6.Controls.Add(Me.Panel1)
        Me.Panel6.Controls.Add(Me.CheckBox_DoubleClickMark)
        Me.Panel6.Controls.Add(Me.CheckBox_AutoMark)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel6.Location = New System.Drawing.Point(0, 140)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(602, 68)
        Me.Panel6.TabIndex = 23
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(0, 1)
        Me.Label7.Margin = New System.Windows.Forms.Padding(3)
        Me.Label7.Name = "Label7"
        Me.Label7.Padding = New System.Windows.Forms.Padding(6, 3, 0, 0)
        Me.Label7.Size = New System.Drawing.Size(117, 16)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Syntax Highlighting"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(602, 1)
        Me.Panel1.TabIndex = 0
        '
        'CheckBox_DoubleClickMark
        '
        Me.CheckBox_DoubleClickMark.AutoSize = True
        Me.CheckBox_DoubleClickMark.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_DoubleClickMark.Location = New System.Drawing.Point(6, 23)
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
        Me.CheckBox_AutoMark.Location = New System.Drawing.Point(6, 47)
        Me.CheckBox_AutoMark.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AutoMark.Name = "CheckBox_AutoMark"
        Me.CheckBox_AutoMark.Size = New System.Drawing.Size(164, 18)
        Me.CheckBox_AutoMark.TabIndex = 17
        Me.CheckBox_AutoMark.Text = "Automatically mark words"
        Me.CheckBox_AutoMark.UseVisualStyleBackColor = True
        '
        'Panel7
        '
        Me.Panel7.AutoSize = True
        Me.Panel7.Controls.Add(Me.Label8)
        Me.Panel7.Controls.Add(Me.Panel3)
        Me.Panel7.Controls.Add(Me.Button_Font)
        Me.Panel7.Controls.Add(Me.Label_Font)
        Me.Panel7.Controls.Add(Me.CheckBox_InvertedColors)
        Me.Panel7.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel7.Location = New System.Drawing.Point(0, 67)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(602, 73)
        Me.Panel7.TabIndex = 24
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(0, 1)
        Me.Label8.Margin = New System.Windows.Forms.Padding(3)
        Me.Label8.Name = "Label8"
        Me.Label8.Padding = New System.Windows.Forms.Padding(6, 3, 0, 0)
        Me.Label8.Size = New System.Drawing.Size(68, 16)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Text Editor"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(602, 1)
        Me.Panel3.TabIndex = 9
        '
        'Button_Font
        '
        Me.Button_Font.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Font.Location = New System.Drawing.Point(6, 23)
        Me.Button_Font.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.Button_Font.Name = "Button_Font"
        Me.Button_Font.Size = New System.Drawing.Size(75, 23)
        Me.Button_Font.TabIndex = 11
        Me.Button_Font.Text = "Font"
        Me.Button_Font.UseVisualStyleBackColor = True
        '
        'Label_Font
        '
        Me.Label_Font.AutoSize = True
        Me.Label_Font.Location = New System.Drawing.Point(90, 28)
        Me.Label_Font.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
        Me.Label_Font.Name = "Label_Font"
        Me.Label_Font.Size = New System.Drawing.Size(73, 13)
        Me.Label_Font.TabIndex = 12
        Me.Label_Font.Text = "Current Font"
        '
        'CheckBox_InvertedColors
        '
        Me.CheckBox_InvertedColors.AutoSize = True
        Me.CheckBox_InvertedColors.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_InvertedColors.Location = New System.Drawing.Point(6, 52)
        Me.CheckBox_InvertedColors.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_InvertedColors.Name = "CheckBox_InvertedColors"
        Me.CheckBox_InvertedColors.Size = New System.Drawing.Size(108, 18)
        Me.CheckBox_InvertedColors.TabIndex = 13
        Me.CheckBox_InvertedColors.Text = "Inverted colors"
        Me.CheckBox_InvertedColors.UseVisualStyleBackColor = True
        '
        'Panel14
        '
        Me.Panel14.AutoSize = True
        Me.Panel14.Controls.Add(Me.CheckBox_AutoShowStartPage)
        Me.Panel14.Controls.Add(Me.Label16)
        Me.Panel14.Controls.Add(Me.CheckBox_AlwaysNewInstance)
        Me.Panel14.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel14.Location = New System.Drawing.Point(0, 0)
        Me.Panel14.Name = "Panel14"
        Me.Panel14.Size = New System.Drawing.Size(602, 67)
        Me.Panel14.TabIndex = 26
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label16.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(0, 0)
        Me.Label16.Margin = New System.Windows.Forms.Padding(3)
        Me.Label16.Name = "Label16"
        Me.Label16.Padding = New System.Windows.Forms.Padding(6, 3, 0, 0)
        Me.Label16.Size = New System.Drawing.Size(53, 16)
        Me.Label16.TabIndex = 18
        Me.Label16.Text = "General"
        '
        'CheckBox_AlwaysNewInstance
        '
        Me.CheckBox_AlwaysNewInstance.AutoSize = True
        Me.CheckBox_AlwaysNewInstance.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AlwaysNewInstance.Location = New System.Drawing.Point(6, 22)
        Me.CheckBox_AlwaysNewInstance.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AlwaysNewInstance.Name = "CheckBox_AlwaysNewInstance"
        Me.CheckBox_AlwaysNewInstance.Size = New System.Drawing.Size(248, 18)
        Me.CheckBox_AlwaysNewInstance.TabIndex = 19
        Me.CheckBox_AlwaysNewInstance.Text = "Always open new instance instead of tabs"
        Me.CheckBox_AlwaysNewInstance.UseVisualStyleBackColor = True
        '
        'TabPage_Configs
        '
        Me.TabPage_Configs.Controls.Add(Me.TableLayoutPanel1)
        Me.TabPage_Configs.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Configs.Name = "TabPage_Configs"
        Me.TabPage_Configs.Size = New System.Drawing.Size(602, 724)
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
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(602, 724)
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
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(452, 724)
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
        Me.Panel17.Size = New System.Drawing.Size(452, 100)
        Me.Panel17.TabIndex = 0
        '
        'Button_SaveConfig
        '
        Me.Button_SaveConfig.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_SaveConfig.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_SaveConfig.Location = New System.Drawing.Point(321, 74)
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
        Me.Button_ConfigRename.Location = New System.Drawing.Point(87, 44)
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
        Me.TextBox_ConfigName.Size = New System.Drawing.Size(446, 22)
        Me.TextBox_ConfigName.TabIndex = 2
        '
        'Button_ConfigCopy
        '
        Me.Button_ConfigCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ConfigCopy.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ConfigCopy.Location = New System.Drawing.Point(179, 44)
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
        Me.Button_ConfigRemove.Location = New System.Drawing.Point(363, 44)
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
        Me.Button_ConfigAdd.Location = New System.Drawing.Point(271, 44)
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
        Me.Panel2.Size = New System.Drawing.Size(452, 624)
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
        Me.GroupBox_ConfigSettings.Size = New System.Drawing.Size(452, 508)
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
        Me.Panel11.Controls.Add(Me.Label15)
        Me.Panel11.Controls.Add(Me.TextBox_SyntaxPath)
        Me.Panel11.Controls.Add(Me.Button_SyntaxPath)
        Me.Panel11.Controls.Add(Me.LinkLabel_SyntaxDefault)
        Me.Panel11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel11.Location = New System.Drawing.Point(3, 365)
        Me.Panel11.Name = "Panel11"
        Me.Panel11.Size = New System.Drawing.Size(446, 140)
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
        Me.Panel13.Size = New System.Drawing.Size(446, 1)
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
        Me.TextBox_Shell.Size = New System.Drawing.Size(440, 22)
        Me.TextBox_Shell.TabIndex = 17
        '
        'LinkLabel_ShowShellArguments
        '
        Me.LinkLabel_ShowShellArguments.AutoSize = True
        Me.LinkLabel_ShowShellArguments.Location = New System.Drawing.Point(3, 61)
        Me.LinkLabel_ShowShellArguments.Margin = New System.Windows.Forms.Padding(3, 0, 3, 6)
        Me.LinkLabel_ShowShellArguments.Name = "LinkLabel_ShowShellArguments"
        Me.LinkLabel_ShowShellArguments.Size = New System.Drawing.Size(136, 13)
        Me.LinkLabel_ShowShellArguments.TabIndex = 18
        Me.LinkLabel_ShowShellArguments.TabStop = True
        Me.LinkLabel_ShowShellArguments.Text = "Show all shell arguments"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(3, 80)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(162, 13)
        Me.Label15.TabIndex = 29
        Me.Label15.Text = "Custom syntax highlight path:"
        '
        'TextBox_SyntaxPath
        '
        Me.TextBox_SyntaxPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_SyntaxPath.BackColor = System.Drawing.Color.White
        Me.TextBox_SyntaxPath.Location = New System.Drawing.Point(3, 96)
        Me.TextBox_SyntaxPath.Name = "TextBox_SyntaxPath"
        Me.TextBox_SyntaxPath.ReadOnly = True
        Me.TextBox_SyntaxPath.Size = New System.Drawing.Size(403, 22)
        Me.TextBox_SyntaxPath.TabIndex = 30
        '
        'Button_SyntaxPath
        '
        Me.Button_SyntaxPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_SyntaxPath.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_SyntaxPath.Location = New System.Drawing.Point(412, 93)
        Me.Button_SyntaxPath.Name = "Button_SyntaxPath"
        Me.Button_SyntaxPath.Size = New System.Drawing.Size(31, 24)
        Me.Button_SyntaxPath.TabIndex = 28
        Me.Button_SyntaxPath.Text = "..."
        Me.Button_SyntaxPath.UseVisualStyleBackColor = True
        '
        'LinkLabel_SyntaxDefault
        '
        Me.LinkLabel_SyntaxDefault.AutoSize = True
        Me.LinkLabel_SyntaxDefault.Location = New System.Drawing.Point(3, 121)
        Me.LinkLabel_SyntaxDefault.Margin = New System.Windows.Forms.Padding(3, 0, 3, 6)
        Me.LinkLabel_SyntaxDefault.Name = "LinkLabel_SyntaxDefault"
        Me.LinkLabel_SyntaxDefault.Size = New System.Drawing.Size(148, 13)
        Me.LinkLabel_SyntaxDefault.TabIndex = 31
        Me.LinkLabel_SyntaxDefault.TabStop = True
        Me.LinkLabel_SyntaxDefault.Text = "Default syntax highlighting"
        '
        'Panel10
        '
        Me.Panel10.AutoSize = True
        Me.Panel10.Controls.Add(Me.Label9)
        Me.Panel10.Controls.Add(Me.Panel12)
        Me.Panel10.Controls.Add(Me.Label2)
        Me.Panel10.Controls.Add(Me.TextBox_GameFolder)
        Me.Panel10.Controls.Add(Me.Button_GameFolder)
        Me.Panel10.Controls.Add(Me.TextBox_SourceModFolder)
        Me.Panel10.Controls.Add(Me.Button_SourceModFolder)
        Me.Panel10.Controls.Add(Me.Label12)
        Me.Panel10.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel10.Location = New System.Drawing.Point(3, 263)
        Me.Panel10.Name = "Panel10"
        Me.Panel10.Size = New System.Drawing.Size(446, 102)
        Me.Panel10.TabIndex = 29
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
        Me.Panel12.Size = New System.Drawing.Size(446, 1)
        Me.Panel12.TabIndex = 27
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 13)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Game directory:"
        '
        'TextBox_GameFolder
        '
        Me.TextBox_GameFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_GameFolder.BackColor = System.Drawing.Color.White
        Me.TextBox_GameFolder.Location = New System.Drawing.Point(3, 36)
        Me.TextBox_GameFolder.Name = "TextBox_GameFolder"
        Me.TextBox_GameFolder.ReadOnly = True
        Me.TextBox_GameFolder.Size = New System.Drawing.Size(403, 22)
        Me.TextBox_GameFolder.TabIndex = 20
        '
        'Button_GameFolder
        '
        Me.Button_GameFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_GameFolder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_GameFolder.Location = New System.Drawing.Point(412, 33)
        Me.Button_GameFolder.Name = "Button_GameFolder"
        Me.Button_GameFolder.Size = New System.Drawing.Size(31, 24)
        Me.Button_GameFolder.TabIndex = 21
        Me.Button_GameFolder.Text = "..."
        Me.Button_GameFolder.UseVisualStyleBackColor = True
        '
        'TextBox_SourceModFolder
        '
        Me.TextBox_SourceModFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_SourceModFolder.BackColor = System.Drawing.Color.White
        Me.TextBox_SourceModFolder.Location = New System.Drawing.Point(3, 77)
        Me.TextBox_SourceModFolder.Name = "TextBox_SourceModFolder"
        Me.TextBox_SourceModFolder.ReadOnly = True
        Me.TextBox_SourceModFolder.Size = New System.Drawing.Size(403, 22)
        Me.TextBox_SourceModFolder.TabIndex = 25
        '
        'Button_SourceModFolder
        '
        Me.Button_SourceModFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_SourceModFolder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_SourceModFolder.Location = New System.Drawing.Point(412, 74)
        Me.Button_SourceModFolder.Name = "Button_SourceModFolder"
        Me.Button_SourceModFolder.Size = New System.Drawing.Size(31, 24)
        Me.Button_SourceModFolder.TabIndex = 26
        Me.Button_SourceModFolder.Text = "..."
        Me.Button_SourceModFolder.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(3, 61)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(117, 13)
        Me.Label12.TabIndex = 24
        Me.Label12.Text = "SourceMod directory:"
        '
        'Panel9
        '
        Me.Panel9.AutoSize = True
        Me.Panel9.Controls.Add(Me.Label11)
        Me.Panel9.Controls.Add(Me.Label3)
        Me.Panel9.Controls.Add(Me.RadioButton_ConfigSettingAutomatic)
        Me.Panel9.Controls.Add(Me.RadioButton_ConfigSettingManual)
        Me.Panel9.Controls.Add(Me.CheckBox_ConfigIsDefault)
        Me.Panel9.Controls.Add(Me.GroupBox1)
        Me.Panel9.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel9.Location = New System.Drawing.Point(3, 18)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(446, 245)
        Me.Panel9.TabIndex = 28
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
        Me.Label3.Size = New System.Drawing.Size(427, 28)
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
        Me.GroupBox1.Size = New System.Drawing.Size(440, 144)
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
        Me.TextBox_CompilerPath.ReadOnly = True
        Me.TextBox_CompilerPath.Size = New System.Drawing.Size(391, 22)
        Me.TextBox_CompilerPath.TabIndex = 8
        '
        'Button_Compiler
        '
        Me.Button_Compiler.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Compiler.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Compiler.Location = New System.Drawing.Point(403, 34)
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
        Me.Label5.Size = New System.Drawing.Size(96, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Include directory:"
        '
        'TextBox_IncludeFolder
        '
        Me.TextBox_IncludeFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_IncludeFolder.BackColor = System.Drawing.Color.White
        Me.TextBox_IncludeFolder.Location = New System.Drawing.Point(6, 75)
        Me.TextBox_IncludeFolder.Name = "TextBox_IncludeFolder"
        Me.TextBox_IncludeFolder.ReadOnly = True
        Me.TextBox_IncludeFolder.Size = New System.Drawing.Size(391, 22)
        Me.TextBox_IncludeFolder.TabIndex = 10
        '
        'Button_IncludeFolder
        '
        Me.Button_IncludeFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_IncludeFolder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_IncludeFolder.Location = New System.Drawing.Point(403, 73)
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
        Me.TextBox_OutputFolder.ReadOnly = True
        Me.TextBox_OutputFolder.Size = New System.Drawing.Size(391, 22)
        Me.TextBox_OutputFolder.TabIndex = 14
        '
        'Button_OutputFolder
        '
        Me.Button_OutputFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_OutputFolder.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_OutputFolder.Location = New System.Drawing.Point(403, 112)
        Me.Button_OutputFolder.Name = "Button_OutputFolder"
        Me.Button_OutputFolder.Size = New System.Drawing.Size(31, 24)
        Me.Button_OutputFolder.TabIndex = 15
        Me.Button_OutputFolder.Text = "..."
        Me.Button_OutputFolder.UseVisualStyleBackColor = True
        '
        'ListBox_Configs
        '
        Me.ListBox_Configs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox_Configs.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox_Configs.FormattingEnabled = True
        Me.ListBox_Configs.HorizontalScrollbar = True
        Me.ListBox_Configs.ItemHeight = 21
        Me.ListBox_Configs.Location = New System.Drawing.Point(3, 3)
        Me.ListBox_Configs.Name = "ListBox_Configs"
        Me.ListBox_Configs.Size = New System.Drawing.Size(144, 718)
        Me.ListBox_Configs.TabIndex = 0
        '
        'TabPage_Plugins
        '
        Me.TabPage_Plugins.Controls.Add(Me.ListView_Plugins)
        Me.TabPage_Plugins.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Plugins.Name = "TabPage_Plugins"
        Me.TabPage_Plugins.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Plugins.Size = New System.Drawing.Size(602, 724)
        Me.TabPage_Plugins.TabIndex = 2
        Me.TabPage_Plugins.Text = "Plugins"
        Me.TabPage_Plugins.UseVisualStyleBackColor = True
        '
        'ListView_Plugins
        '
        Me.ListView_Plugins.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6})
        Me.ListView_Plugins.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_Plugins.Location = New System.Drawing.Point(3, 3)
        Me.ListView_Plugins.Name = "ListView_Plugins"
        Me.ListView_Plugins.Size = New System.Drawing.Size(596, 718)
        Me.ListView_Plugins.TabIndex = 0
        Me.ListView_Plugins.UseCompatibleStateImageBehavior = False
        Me.ListView_Plugins.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "File"
        Me.ColumnHeader1.Width = 100
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Name"
        Me.ColumnHeader2.Width = 100
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Author"
        Me.ColumnHeader3.Width = 100
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Description"
        Me.ColumnHeader4.Width = 100
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Version"
        Me.ColumnHeader5.Width = 100
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "URL"
        Me.ColumnHeader6.Width = 100
        '
        'TabPage_Database
        '
        Me.TabPage_Database.Controls.Add(Me.Button_Refresh)
        Me.TabPage_Database.Controls.Add(Me.TableLayoutPanel4)
        Me.TabPage_Database.Controls.Add(Me.Button_AddDatabaseItem)
        Me.TabPage_Database.Controls.Add(Me.DatabaseViewer)
        Me.TabPage_Database.Controls.Add(Me.TableLayoutPanel3)
        Me.TabPage_Database.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Database.Name = "TabPage_Database"
        Me.TabPage_Database.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Database.Size = New System.Drawing.Size(602, 724)
        Me.TabPage_Database.TabIndex = 3
        Me.TabPage_Database.Text = "Database"
        Me.TabPage_Database.UseVisualStyleBackColor = True
        '
        'Button_Refresh
        '
        Me.Button_Refresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Refresh.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Refresh.Location = New System.Drawing.Point(496, 635)
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
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(590, 63)
        Me.TableLayoutPanel4.TabIndex = 5
        '
        'ClassPictureBoxQuality2
        '
        Me.ClassPictureBoxQuality2.Image = Global.BasicPawn.My.Resources.Resources.imageres_5381_64x64_32
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
        Me.Label18.Size = New System.Drawing.Size(516, 39)
        Me.Label18.TabIndex = 3
        Me.Label18.Text = resources.GetString("Label18.Text")
        '
        'Button_AddDatabaseItem
        '
        Me.Button_AddDatabaseItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_AddDatabaseItem.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_AddDatabaseItem.Location = New System.Drawing.Point(390, 635)
        Me.Button_AddDatabaseItem.Name = "Button_AddDatabaseItem"
        Me.Button_AddDatabaseItem.Size = New System.Drawing.Size(100, 23)
        Me.Button_AddDatabaseItem.TabIndex = 4
        Me.Button_AddDatabaseItem.Text = "Add"
        Me.Button_AddDatabaseItem.UseVisualStyleBackColor = True
        '
        'DatabaseViewer
        '
        Me.DatabaseViewer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DatabaseViewer.AutoScroll = True
        Me.DatabaseViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DatabaseViewer.Location = New System.Drawing.Point(6, 75)
        Me.DatabaseViewer.Name = "DatabaseViewer"
        Me.DatabaseViewer.Size = New System.Drawing.Size(590, 554)
        Me.DatabaseViewer.TabIndex = 3
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
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(6, 664)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(590, 54)
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
        Me.Label17.Size = New System.Drawing.Size(537, 26)
        Me.Label17.TabIndex = 3
        Me.Label17.Text = "Loaded BasicPawn plugins are able to read stored database entries. Make sure all " &
    "installed plugins are from a trustworthy publisher to prevent theft."
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Cancel.Location = New System.Drawing.Point(536, 768)
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
        Me.Button_Apply.Location = New System.Drawing.Point(444, 768)
        Me.Button_Apply.Name = "Button_Apply"
        Me.Button_Apply.Size = New System.Drawing.Size(86, 23)
        Me.Button_Apply.TabIndex = 2
        Me.Button_Apply.Text = "Apply"
        Me.Button_Apply.UseVisualStyleBackColor = True
        '
        'CheckBox_AutoShowStartPage
        '
        Me.CheckBox_AutoShowStartPage.AutoSize = True
        Me.CheckBox_AutoShowStartPage.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_AutoShowStartPage.Location = New System.Drawing.Point(6, 46)
        Me.CheckBox_AutoShowStartPage.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.CheckBox_AutoShowStartPage.Name = "CheckBox_AutoShowStartPage"
        Me.CheckBox_AutoShowStartPage.Size = New System.Drawing.Size(235, 18)
        Me.CheckBox_AutoShowStartPage.TabIndex = 20
        Me.CheckBox_AutoShowStartPage.Text = "Show StartPage when no file is opened"
        Me.CheckBox_AutoShowStartPage.UseVisualStyleBackColor = True
        '
        'FormSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Button_Cancel
        Me.ClientSize = New System.Drawing.Size(634, 803)
        Me.Controls.Add(Me.Button_Apply)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.TabControl1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(650, 500)
        Me.Name = "FormSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Settings"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_Settings.ResumeLayout(False)
        Me.TabPage_Settings.PerformLayout()
        Me.Panel8.ResumeLayout(False)
        Me.Panel8.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        Me.Panel14.ResumeLayout(False)
        Me.Panel14.PerformLayout()
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
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabPage_Plugins.ResumeLayout(False)
        Me.TabPage_Database.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        CType(Me.ClassPictureBoxQuality2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        CType(Me.ClassPictureBoxQuality1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage_Settings As TabPage
    Friend WithEvents TabPage_Configs As TabPage
    Friend WithEvents CheckBox_OnScreenIntelliSense As CheckBox
    Friend WithEvents Label1 As Label
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
    Friend WithEvents Label7 As Label
    Friend WithEvents CheckBox_CommentsMethodIntelliSense As CheckBox
    Friend WithEvents CheckBox_CommentsAutocompleteIntelliSense As CheckBox
    Friend WithEvents CheckBox_FullAutocompleteReTagging As CheckBox
    Friend WithEvents Label_Font As Label
    Friend WithEvents Button_Font As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents CheckBox_InvertedColors As CheckBox
    Friend WithEvents CheckBox_CaseSensitive As CheckBox
    Friend WithEvents CheckBox_WindowsToolTipPopup As CheckBox
    Friend WithEvents TextBox_Shell As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents LinkLabel_ShowShellArguments As LinkLabel
    Friend WithEvents CheckBox_AutoMark As CheckBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Button_GameFolder As Button
    Friend WithEvents TextBox_GameFolder As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Button_SourceModFolder As Button
    Friend WithEvents TextBox_SourceModFolder As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents CheckBox_EntitiesEnableColor As CheckBox
    Friend WithEvents CheckBox_CatchExceptions As CheckBox
    Friend WithEvents CheckBox_EntitiesEnableShowNewEnts As CheckBox
    Friend WithEvents Panel8 As Panel
    Friend WithEvents Panel7 As Panel
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Panel11 As Panel
    Friend WithEvents Panel10 As Panel
    Friend WithEvents Panel9 As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel13 As Panel
    Friend WithEvents Panel12 As Panel
    Friend WithEvents Button_SyntaxPath As Button
    Friend WithEvents TextBox_SyntaxPath As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents LinkLabel_SyntaxDefault As LinkLabel
    Friend WithEvents CheckBox_CurrentSourceVarAutocomplete As CheckBox
    Friend WithEvents TabPage_Plugins As TabPage
    Friend WithEvents ListView_Plugins As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ColumnHeader5 As ColumnHeader
    Friend WithEvents ColumnHeader6 As ColumnHeader
    Friend WithEvents Panel14 As Panel
    Friend WithEvents Panel15 As Panel
    Friend WithEvents Label16 As Label
    Friend WithEvents CheckBox_AlwaysNewInstance As CheckBox
    Friend WithEvents CheckBox_VarAutocompleteShowObjectBrowser As CheckBox
    Friend WithEvents CheckBox_AlwaysLoadDefaultIncludes As CheckBox
    Friend WithEvents CheckBox_SwitchTabToAutocomplete As CheckBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Panel17 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents CheckBox_ConfigIsDefault As CheckBox
    Friend WithEvents TabPage_Database As TabPage
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents ClassPictureBoxQuality1 As ClassPictureBoxQuality
    Friend WithEvents Label17 As Label
    Friend WithEvents ClassPictureBoxQuality2 As ClassPictureBoxQuality
    Friend WithEvents DatabaseViewer As ClassDatabaseViewer
    Friend WithEvents Button_AddDatabaseItem As Button
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents Label18 As Label
    Friend WithEvents Button_Refresh As Button
    Friend WithEvents CheckBox_AutoShowStartPage As CheckBox
End Class
