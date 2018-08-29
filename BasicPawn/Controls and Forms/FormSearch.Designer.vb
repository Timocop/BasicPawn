<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormSearch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSearch))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox_Search = New System.Windows.Forms.TextBox()
        Me.TextBox_Replace = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button_Search = New System.Windows.Forms.Button()
        Me.Button_Replace = New System.Windows.Forms.Button()
        Me.CheckBox_WholeWord = New System.Windows.Forms.CheckBox()
        Me.CheckBox_CaseSensitive = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CheckBox_LoopSearch = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Multiline = New System.Windows.Forms.CheckBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.RadioButton_ModeRegEx = New System.Windows.Forms.RadioButton()
        Me.RadioButton_ModeNormal = New System.Windows.Forms.RadioButton()
        Me.Button_ReplaceAll = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.RadioButton_DirectionDown = New System.Windows.Forms.RadioButton()
        Me.RadioButton_DirectionUp = New System.Windows.Forms.RadioButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel_Status = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Button_ListAll = New System.Windows.Forms.Button()
        Me.ListView_Output = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button_ListAllOpenTabs = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.CheckBox_Transparency = New System.Windows.Forms.CheckBox()
        Me.RadioButton_TransparencyInactive = New System.Windows.Forms.RadioButton()
        Me.RadioButton_TransparencyAlways = New System.Windows.Forms.RadioButton()
        Me.TrackBar_Transparency = New System.Windows.Forms.TrackBar()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.TrackBar_Transparency, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Search:"
        '
        'TextBox_Search
        '
        Me.TextBox_Search.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_Search.Location = New System.Drawing.Point(68, 12)
        Me.TextBox_Search.Name = "TextBox_Search"
        Me.TextBox_Search.Size = New System.Drawing.Size(317, 22)
        Me.TextBox_Search.TabIndex = 1
        '
        'TextBox_Replace
        '
        Me.TextBox_Replace.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_Replace.Location = New System.Drawing.Point(68, 40)
        Me.TextBox_Replace.Name = "TextBox_Replace"
        Me.TextBox_Replace.Size = New System.Drawing.Size(317, 22)
        Me.TextBox_Replace.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Replace:"
        '
        'Button_Search
        '
        Me.Button_Search.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Search.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Search.Location = New System.Drawing.Point(391, 12)
        Me.Button_Search.Name = "Button_Search"
        Me.Button_Search.Size = New System.Drawing.Size(116, 23)
        Me.Button_Search.TabIndex = 4
        Me.Button_Search.Text = "Search"
        Me.ToolTip1.SetToolTip(Me.Button_Search, "Use the keyboard shortcuts UP/DOWN in the 'Search' textbox to search with directi" &
        "on.")
        Me.Button_Search.UseVisualStyleBackColor = True
        '
        'Button_Replace
        '
        Me.Button_Replace.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Replace.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Replace.Location = New System.Drawing.Point(391, 41)
        Me.Button_Replace.Name = "Button_Replace"
        Me.Button_Replace.Size = New System.Drawing.Size(116, 23)
        Me.Button_Replace.TabIndex = 5
        Me.Button_Replace.Text = "Replace"
        Me.Button_Replace.UseVisualStyleBackColor = True
        '
        'CheckBox_WholeWord
        '
        Me.CheckBox_WholeWord.AutoSize = True
        Me.CheckBox_WholeWord.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_WholeWord.Location = New System.Drawing.Point(6, 19)
        Me.CheckBox_WholeWord.Name = "CheckBox_WholeWord"
        Me.CheckBox_WholeWord.Size = New System.Drawing.Size(133, 18)
        Me.CheckBox_WholeWord.TabIndex = 6
        Me.CheckBox_WholeWord.Text = "Match Whole Word"
        Me.CheckBox_WholeWord.UseVisualStyleBackColor = True
        '
        'CheckBox_CaseSensitive
        '
        Me.CheckBox_CaseSensitive.AutoSize = True
        Me.CheckBox_CaseSensitive.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_CaseSensitive.Location = New System.Drawing.Point(6, 42)
        Me.CheckBox_CaseSensitive.Name = "CheckBox_CaseSensitive"
        Me.CheckBox_CaseSensitive.Size = New System.Drawing.Size(104, 18)
        Me.CheckBox_CaseSensitive.TabIndex = 7
        Me.CheckBox_CaseSensitive.Text = "Case Sensitive"
        Me.CheckBox_CaseSensitive.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CheckBox_LoopSearch)
        Me.GroupBox1.Controls.Add(Me.CheckBox_Multiline)
        Me.GroupBox1.Controls.Add(Me.CheckBox_WholeWord)
        Me.GroupBox1.Controls.Add(Me.CheckBox_CaseSensitive)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 68)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(136, 152)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Settings"
        '
        'CheckBox_LoopSearch
        '
        Me.CheckBox_LoopSearch.AutoSize = True
        Me.CheckBox_LoopSearch.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_LoopSearch.Location = New System.Drawing.Point(6, 89)
        Me.CheckBox_LoopSearch.Name = "CheckBox_LoopSearch"
        Me.CheckBox_LoopSearch.Size = New System.Drawing.Size(95, 18)
        Me.CheckBox_LoopSearch.TabIndex = 9
        Me.CheckBox_LoopSearch.Text = "Loop Search"
        Me.CheckBox_LoopSearch.UseVisualStyleBackColor = True
        '
        'CheckBox_Multiline
        '
        Me.CheckBox_Multiline.AutoSize = True
        Me.CheckBox_Multiline.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_Multiline.Location = New System.Drawing.Point(6, 65)
        Me.CheckBox_Multiline.Name = "CheckBox_Multiline"
        Me.CheckBox_Multiline.Size = New System.Drawing.Size(118, 18)
        Me.CheckBox_Multiline.TabIndex = 8
        Me.CheckBox_Multiline.Text = "Multiline (RegEx)"
        Me.CheckBox_Multiline.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.RadioButton_ModeRegEx)
        Me.GroupBox2.Controls.Add(Me.RadioButton_ModeNormal)
        Me.GroupBox2.Location = New System.Drawing.Point(154, 68)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(137, 73)
        Me.GroupBox2.TabIndex = 9
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Mode"
        '
        'RadioButton_ModeRegEx
        '
        Me.RadioButton_ModeRegEx.AutoSize = True
        Me.RadioButton_ModeRegEx.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RadioButton_ModeRegEx.Location = New System.Drawing.Point(6, 42)
        Me.RadioButton_ModeRegEx.Name = "RadioButton_ModeRegEx"
        Me.RadioButton_ModeRegEx.Size = New System.Drawing.Size(134, 18)
        Me.RadioButton_ModeRegEx.TabIndex = 11
        Me.RadioButton_ModeRegEx.Text = "Regular Expressions"
        Me.RadioButton_ModeRegEx.UseVisualStyleBackColor = True
        '
        'RadioButton_ModeNormal
        '
        Me.RadioButton_ModeNormal.AutoSize = True
        Me.RadioButton_ModeNormal.Checked = True
        Me.RadioButton_ModeNormal.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RadioButton_ModeNormal.Location = New System.Drawing.Point(6, 19)
        Me.RadioButton_ModeNormal.Name = "RadioButton_ModeNormal"
        Me.RadioButton_ModeNormal.Size = New System.Drawing.Size(68, 18)
        Me.RadioButton_ModeNormal.TabIndex = 10
        Me.RadioButton_ModeNormal.TabStop = True
        Me.RadioButton_ModeNormal.Text = "Normal"
        Me.RadioButton_ModeNormal.UseVisualStyleBackColor = True
        '
        'Button_ReplaceAll
        '
        Me.Button_ReplaceAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ReplaceAll.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ReplaceAll.Location = New System.Drawing.Point(391, 70)
        Me.Button_ReplaceAll.Name = "Button_ReplaceAll"
        Me.Button_ReplaceAll.Size = New System.Drawing.Size(116, 23)
        Me.Button_ReplaceAll.TabIndex = 10
        Me.Button_ReplaceAll.Text = "Replace All"
        Me.Button_ReplaceAll.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.RadioButton_DirectionDown)
        Me.GroupBox3.Controls.Add(Me.RadioButton_DirectionUp)
        Me.GroupBox3.Location = New System.Drawing.Point(297, 68)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(90, 73)
        Me.GroupBox3.TabIndex = 11
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Direction"
        '
        'RadioButton_DirectionDown
        '
        Me.RadioButton_DirectionDown.AutoSize = True
        Me.RadioButton_DirectionDown.Checked = True
        Me.RadioButton_DirectionDown.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RadioButton_DirectionDown.Location = New System.Drawing.Point(6, 42)
        Me.RadioButton_DirectionDown.Name = "RadioButton_DirectionDown"
        Me.RadioButton_DirectionDown.Size = New System.Drawing.Size(62, 18)
        Me.RadioButton_DirectionDown.TabIndex = 1
        Me.RadioButton_DirectionDown.TabStop = True
        Me.RadioButton_DirectionDown.Text = "Down"
        Me.RadioButton_DirectionDown.UseVisualStyleBackColor = True
        '
        'RadioButton_DirectionUp
        '
        Me.RadioButton_DirectionUp.AutoSize = True
        Me.RadioButton_DirectionUp.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RadioButton_DirectionUp.Location = New System.Drawing.Point(6, 19)
        Me.RadioButton_DirectionUp.Name = "RadioButton_DirectionUp"
        Me.RadioButton_DirectionUp.Size = New System.Drawing.Size(46, 18)
        Me.RadioButton_DirectionUp.TabIndex = 0
        Me.RadioButton_DirectionUp.TabStop = True
        Me.RadioButton_DirectionUp.Text = "Up"
        Me.RadioButton_DirectionUp.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel_Status})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 440)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(519, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 12
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel_Status
        '
        Me.ToolStripStatusLabel_Status.Name = "ToolStripStatusLabel_Status"
        Me.ToolStripStatusLabel_Status.Size = New System.Drawing.Size(190, 17)
        Me.ToolStripStatusLabel_Status.Text = "Click the 'Search' button to search."
        '
        'Button_ListAll
        '
        Me.Button_ListAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ListAll.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ListAll.Location = New System.Drawing.Point(391, 99)
        Me.Button_ListAll.Name = "Button_ListAll"
        Me.Button_ListAll.Size = New System.Drawing.Size(116, 23)
        Me.Button_ListAll.TabIndex = 13
        Me.Button_ListAll.Text = "List All"
        Me.Button_ListAll.UseVisualStyleBackColor = True
        '
        'ListView_Output
        '
        Me.ListView_Output.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.ListView_Output.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_Output.FullRowSelect = True
        Me.ListView_Output.HideSelection = False
        Me.ListView_Output.Location = New System.Drawing.Point(3, 253)
        Me.ListView_Output.MultiSelect = False
        Me.ListView_Output.Name = "ListView_Output"
        Me.ListView_Output.Size = New System.Drawing.Size(513, 184)
        Me.ListView_Output.TabIndex = 14
        Me.ListView_Output.UseCompatibleStateImageBehavior = False
        Me.ListView_Output.View = System.Windows.Forms.View.Details
        Me.ListView_Output.Visible = False
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "File"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Line"
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Text"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Path"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ListView_Output, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(519, 440)
        Me.TableLayoutPanel1.TabIndex = 15
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.GroupBox4)
        Me.Panel1.Controls.Add(Me.Button_ListAllOpenTabs)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.TextBox_Search)
        Me.Panel1.Controls.Add(Me.Button_ListAll)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.TextBox_Replace)
        Me.Panel1.Controls.Add(Me.GroupBox3)
        Me.Panel1.Controls.Add(Me.Button_Search)
        Me.Panel1.Controls.Add(Me.Button_ReplaceAll)
        Me.Panel1.Controls.Add(Me.Button_Replace)
        Me.Panel1.Controls.Add(Me.GroupBox2)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(519, 250)
        Me.Panel1.TabIndex = 0
        '
        'Button_ListAllOpenTabs
        '
        Me.Button_ListAllOpenTabs.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ListAllOpenTabs.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ListAllOpenTabs.Location = New System.Drawing.Point(391, 128)
        Me.Button_ListAllOpenTabs.Name = "Button_ListAllOpenTabs"
        Me.Button_ListAllOpenTabs.Size = New System.Drawing.Size(116, 36)
        Me.Button_ListAllOpenTabs.TabIndex = 14
        Me.Button_ListAllOpenTabs.Text = "List All in All open tabs"
        Me.Button_ListAllOpenTabs.UseVisualStyleBackColor = True
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 25000
        Me.ToolTip1.InitialDelay = 500
        Me.ToolTip1.ReshowDelay = 100
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.TrackBar_Transparency)
        Me.GroupBox4.Controls.Add(Me.RadioButton_TransparencyAlways)
        Me.GroupBox4.Controls.Add(Me.RadioButton_TransparencyInactive)
        Me.GroupBox4.Controls.Add(Me.CheckBox_Transparency)
        Me.GroupBox4.Location = New System.Drawing.Point(154, 147)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(231, 73)
        Me.GroupBox4.TabIndex = 15
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "                                "
        '
        'CheckBox_Transparency
        '
        Me.CheckBox_Transparency.AutoSize = True
        Me.CheckBox_Transparency.Checked = True
        Me.CheckBox_Transparency.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckBox_Transparency.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_Transparency.Location = New System.Drawing.Point(6, 0)
        Me.CheckBox_Transparency.Name = "CheckBox_Transparency"
        Me.CheckBox_Transparency.Size = New System.Drawing.Size(98, 18)
        Me.CheckBox_Transparency.TabIndex = 0
        Me.CheckBox_Transparency.Text = "Transparency"
        Me.CheckBox_Transparency.UseVisualStyleBackColor = True
        '
        'RadioButton_TransparencyInactive
        '
        Me.RadioButton_TransparencyInactive.AutoSize = True
        Me.RadioButton_TransparencyInactive.Checked = True
        Me.RadioButton_TransparencyInactive.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RadioButton_TransparencyInactive.Location = New System.Drawing.Point(6, 24)
        Me.RadioButton_TransparencyInactive.Name = "RadioButton_TransparencyInactive"
        Me.RadioButton_TransparencyInactive.Size = New System.Drawing.Size(104, 18)
        Me.RadioButton_TransparencyInactive.TabIndex = 1
        Me.RadioButton_TransparencyInactive.TabStop = True
        Me.RadioButton_TransparencyInactive.Text = "When inactive"
        Me.RadioButton_TransparencyInactive.UseVisualStyleBackColor = True
        '
        'RadioButton_TransparencyAlways
        '
        Me.RadioButton_TransparencyAlways.AutoSize = True
        Me.RadioButton_TransparencyAlways.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RadioButton_TransparencyAlways.Location = New System.Drawing.Point(6, 48)
        Me.RadioButton_TransparencyAlways.Name = "RadioButton_TransparencyAlways"
        Me.RadioButton_TransparencyAlways.Size = New System.Drawing.Size(66, 18)
        Me.RadioButton_TransparencyAlways.TabIndex = 2
        Me.RadioButton_TransparencyAlways.Text = "Always"
        Me.RadioButton_TransparencyAlways.UseVisualStyleBackColor = True
        '
        'TrackBar_Transparency
        '
        Me.TrackBar_Transparency.AutoSize = False
        Me.TrackBar_Transparency.LargeChange = 1
        Me.TrackBar_Transparency.Location = New System.Drawing.Point(116, 33)
        Me.TrackBar_Transparency.Maximum = 5
        Me.TrackBar_Transparency.Minimum = 1
        Me.TrackBar_Transparency.Name = "TrackBar_Transparency"
        Me.TrackBar_Transparency.Size = New System.Drawing.Size(109, 16)
        Me.TrackBar_Transparency.TabIndex = 3
        Me.TrackBar_Transparency.TickStyle = System.Windows.Forms.TickStyle.None
        Me.TrackBar_Transparency.Value = 4
        '
        'FormSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(519, 462)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FormSearch"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Search & Replace"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.TrackBar_Transparency, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox_Search As TextBox
    Friend WithEvents TextBox_Replace As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Button_Search As Button
    Friend WithEvents Button_Replace As Button
    Friend WithEvents CheckBox_WholeWord As CheckBox
    Friend WithEvents CheckBox_CaseSensitive As CheckBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents RadioButton_ModeRegEx As RadioButton
    Friend WithEvents RadioButton_ModeNormal As RadioButton
    Friend WithEvents Button_ReplaceAll As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents RadioButton_DirectionDown As RadioButton
    Friend WithEvents RadioButton_DirectionUp As RadioButton
    Friend WithEvents CheckBox_Multiline As CheckBox
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel_Status As ToolStripStatusLabel
    Friend WithEvents CheckBox_LoopSearch As CheckBox
    Friend WithEvents Button_ListAll As Button
    Friend WithEvents ListView_Output As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents Button_ListAllOpenTabs As Button
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents CheckBox_Transparency As CheckBox
    Friend WithEvents RadioButton_TransparencyAlways As RadioButton
    Friend WithEvents RadioButton_TransparencyInactive As RadioButton
    Friend WithEvents TrackBar_Transparency As TrackBar
End Class
