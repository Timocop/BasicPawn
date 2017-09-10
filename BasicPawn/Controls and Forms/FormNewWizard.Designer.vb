<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormNewWizard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormNewWizard))
        Me.TreeView_Explorer = New System.Windows.Forms.TreeView()
        Me.ContextMenuStrip_TreeView = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_OpenDir = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_DelTemplate = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList_TreeView = New System.Windows.Forms.ImageList(Me.components)
        Me.RichTextBox_Preview = New System.Windows.Forms.RichTextBox()
        Me.Button_Apply = New System.Windows.Forms.Button()
        Me.ListView_Properties = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuStrip_Properties = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripComboBox_SetProperty = New System.Windows.Forms.ToolStripComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LinkLabel_CreateDefault = New System.Windows.Forms.LinkLabel()
        Me.ContextMenuStrip_TreeView.SuspendLayout()
        Me.ContextMenuStrip_Properties.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TreeView_Explorer
        '
        Me.TreeView_Explorer.ContextMenuStrip = Me.ContextMenuStrip_TreeView
        Me.TreeView_Explorer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeView_Explorer.ImageIndex = 0
        Me.TreeView_Explorer.ImageList = Me.ImageList_TreeView
        Me.TreeView_Explorer.Location = New System.Drawing.Point(0, 13)
        Me.TreeView_Explorer.Margin = New System.Windows.Forms.Padding(0)
        Me.TreeView_Explorer.Name = "TreeView_Explorer"
        Me.TreeView_Explorer.SelectedImageIndex = 0
        Me.TreeView_Explorer.Size = New System.Drawing.Size(222, 495)
        Me.TreeView_Explorer.TabIndex = 0
        '
        'ContextMenuStrip_TreeView
        '
        Me.ContextMenuStrip_TreeView.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_OpenDir, Me.ToolStripSeparator1, Me.ToolStripMenuItem_DelTemplate})
        Me.ContextMenuStrip_TreeView.Name = "ContextMenuStrip_TreeView"
        Me.ContextMenuStrip_TreeView.ShowImageMargin = False
        Me.ContextMenuStrip_TreeView.Size = New System.Drawing.Size(157, 54)
        '
        'ToolStripMenuItem_OpenDir
        '
        Me.ToolStripMenuItem_OpenDir.Name = "ToolStripMenuItem_OpenDir"
        Me.ToolStripMenuItem_OpenDir.Size = New System.Drawing.Size(156, 22)
        Me.ToolStripMenuItem_OpenDir.Text = "Open Folder"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(153, 6)
        '
        'ToolStripMenuItem_DelTemplate
        '
        Me.ToolStripMenuItem_DelTemplate.Name = "ToolStripMenuItem_DelTemplate"
        Me.ToolStripMenuItem_DelTemplate.Size = New System.Drawing.Size(156, 22)
        Me.ToolStripMenuItem_DelTemplate.Text = "Move to Recycle bin"
        '
        'ImageList_TreeView
        '
        Me.ImageList_TreeView.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList_TreeView.ImageSize = New System.Drawing.Size(24, 24)
        Me.ImageList_TreeView.TransparentColor = System.Drawing.Color.Transparent
        '
        'RichTextBox_Preview
        '
        Me.RichTextBox_Preview.BackColor = System.Drawing.SystemColors.Window
        Me.RichTextBox_Preview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBox_Preview.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox_Preview.Location = New System.Drawing.Point(222, 13)
        Me.RichTextBox_Preview.Margin = New System.Windows.Forms.Padding(0)
        Me.RichTextBox_Preview.Name = "RichTextBox_Preview"
        Me.RichTextBox_Preview.ReadOnly = True
        Me.RichTextBox_Preview.Size = New System.Drawing.Size(527, 495)
        Me.RichTextBox_Preview.TabIndex = 1
        Me.RichTextBox_Preview.Text = ""
        Me.RichTextBox_Preview.WordWrap = False
        '
        'Button_Apply
        '
        Me.Button_Apply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Apply.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button_Apply.Location = New System.Drawing.Point(886, 526)
        Me.Button_Apply.Name = "Button_Apply"
        Me.Button_Apply.Size = New System.Drawing.Size(86, 23)
        Me.Button_Apply.TabIndex = 2
        Me.Button_Apply.Text = "Apply"
        Me.Button_Apply.UseVisualStyleBackColor = True
        '
        'ListView_Properties
        '
        Me.ListView_Properties.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.ListView_Properties.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_Properties.FullRowSelect = True
        Me.ListView_Properties.Location = New System.Drawing.Point(749, 13)
        Me.ListView_Properties.Margin = New System.Windows.Forms.Padding(0)
        Me.ListView_Properties.MultiSelect = False
        Me.ListView_Properties.Name = "ListView_Properties"
        Me.ListView_Properties.Size = New System.Drawing.Size(211, 495)
        Me.ListView_Properties.TabIndex = 3
        Me.ListView_Properties.UseCompatibleStateImageBehavior = False
        Me.ListView_Properties.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 100
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Value"
        Me.ColumnHeader2.Width = 100
        '
        'ContextMenuStrip_Properties
        '
        Me.ContextMenuStrip_Properties.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripComboBox_SetProperty})
        Me.ContextMenuStrip_Properties.Name = "ContextMenuStrip_Properties"
        Me.ContextMenuStrip_Properties.ShowImageMargin = False
        Me.ContextMenuStrip_Properties.ShowItemToolTips = False
        Me.ContextMenuStrip_Properties.Size = New System.Drawing.Size(236, 27)
        '
        'ToolStripComboBox_SetProperty
        '
        Me.ToolStripComboBox_SetProperty.DropDownWidth = 200
        Me.ToolStripComboBox_SetProperty.Margin = New System.Windows.Forms.Padding(0)
        Me.ToolStripComboBox_SetProperty.Name = "ToolStripComboBox_SetProperty"
        Me.ToolStripComboBox_SetProperty.Size = New System.Drawing.Size(200, 23)
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Template Explorer"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(225, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Preview"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.22917!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.875!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ListView_Properties, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TreeView_Explorer, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.RichTextBox_Preview, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 2, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(960, 508)
        Me.TableLayoutPanel1.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(752, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(59, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Properties"
        '
        'LinkLabel_CreateDefault
        '
        Me.LinkLabel_CreateDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel_CreateDefault.AutoSize = True
        Me.LinkLabel_CreateDefault.Location = New System.Drawing.Point(12, 531)
        Me.LinkLabel_CreateDefault.Name = "LinkLabel_CreateDefault"
        Me.LinkLabel_CreateDefault.Size = New System.Drawing.Size(133, 13)
        Me.LinkLabel_CreateDefault.TabIndex = 7
        Me.LinkLabel_CreateDefault.TabStop = True
        Me.LinkLabel_CreateDefault.Text = "Create default templates"
        '
        'FormNewWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(984, 561)
        Me.Controls.Add(Me.LinkLabel_CreateDefault)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.Button_Apply)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "FormNewWizard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "New Wizard"
        Me.ContextMenuStrip_TreeView.ResumeLayout(False)
        Me.ContextMenuStrip_Properties.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TreeView_Explorer As TreeView
    Friend WithEvents RichTextBox_Preview As RichTextBox
    Friend WithEvents Button_Apply As Button
    Friend WithEvents ListView_Properties As ListView
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Label3 As Label
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ContextMenuStrip_Properties As ContextMenuStrip
    Friend WithEvents ToolStripComboBox_SetProperty As ToolStripComboBox
    Friend WithEvents ImageList_TreeView As ImageList
    Friend WithEvents LinkLabel_CreateDefault As LinkLabel
    Friend WithEvents ContextMenuStrip_TreeView As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_OpenDir As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_DelTemplate As ToolStripMenuItem
End Class
