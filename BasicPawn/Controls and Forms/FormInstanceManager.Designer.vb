<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormInstanceManager
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormInstanceManager))
        Me.Panel_FooterControl = New System.Windows.Forms.Panel()
        Me.Button_Close = New System.Windows.Forms.Button()
        Me.Panel_FooterDarkControl = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LinkLabel_Refresh = New System.Windows.Forms.LinkLabel()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList_Instances = New System.Windows.Forms.ImageList(Me.components)
        Me.ContextMenuStrip_Instances = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_Refresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_UncheckAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_CopyChecked = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_MoveChecked = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_CloseChecked = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_CloseInstChecked = New System.Windows.Forms.ToolStripMenuItem()
        Me.TreeViewColumns_Instances = New BasicPawn.ClassTreeViewColumns()
        Me.ToolStripMenuItem_PopoutChecked = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel_FooterControl.SuspendLayout()
        Me.ContextMenuStrip_Instances.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel_FooterControl
        '
        Me.Panel_FooterControl.BackColor = System.Drawing.SystemColors.Control
        Me.Panel_FooterControl.Controls.Add(Me.Button_Close)
        Me.Panel_FooterControl.Controls.Add(Me.Panel_FooterDarkControl)
        Me.Panel_FooterControl.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_FooterControl.Location = New System.Drawing.Point(0, 393)
        Me.Panel_FooterControl.Name = "Panel_FooterControl"
        Me.Panel_FooterControl.Size = New System.Drawing.Size(624, 48)
        Me.Panel_FooterControl.TabIndex = 0
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.Location = New System.Drawing.Point(526, 13)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(86, 23)
        Me.Button_Close.TabIndex = 1
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'Panel_FooterDarkControl
        '
        Me.Panel_FooterDarkControl.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel_FooterDarkControl.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_FooterDarkControl.Location = New System.Drawing.Point(0, 0)
        Me.Panel_FooterDarkControl.Name = "Panel_FooterDarkControl"
        Me.Panel_FooterDarkControl.Size = New System.Drawing.Size(624, 1)
        Me.Panel_FooterDarkControl.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(600, 36)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Manage files from all open BasicPawn instances."
        '
        'LinkLabel_Refresh
        '
        Me.LinkLabel_Refresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel_Refresh.AutoSize = True
        Me.LinkLabel_Refresh.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel_Refresh.Location = New System.Drawing.Point(566, 374)
        Me.LinkLabel_Refresh.Margin = New System.Windows.Forms.Padding(3)
        Me.LinkLabel_Refresh.Name = "LinkLabel_Refresh"
        Me.LinkLabel_Refresh.Size = New System.Drawing.Size(46, 13)
        Me.LinkLabel_Refresh.TabIndex = 5
        Me.LinkLabel_Refresh.TabStop = True
        Me.LinkLabel_Refresh.Text = "Refresh"
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.DisplayIndex = 0
        Me.ColumnHeader1.Text = "Details"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.DisplayIndex = 1
        Me.ColumnHeader2.Text = "Files"
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Tab"
        Me.ColumnHeader3.Width = 100
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "File"
        Me.ColumnHeader4.Width = 400
        '
        'ImageList_Instances
        '
        Me.ImageList_Instances.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList_Instances.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList_Instances.TransparentColor = System.Drawing.Color.Transparent
        '
        'ContextMenuStrip_Instances
        '
        Me.ContextMenuStrip_Instances.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_Refresh, Me.ToolStripMenuItem_UncheckAll, Me.ToolStripSeparator1, Me.ToolStripMenuItem_CopyChecked, Me.ToolStripMenuItem_MoveChecked, Me.ToolStripMenuItem_PopoutChecked, Me.ToolStripMenuItem_CloseChecked, Me.ToolStripSeparator2, Me.ToolStripMenuItem_CloseInstChecked})
        Me.ContextMenuStrip_Instances.Name = "ContextMenuStrip_Instances"
        Me.ContextMenuStrip_Instances.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_Instances.Size = New System.Drawing.Size(207, 192)
        '
        'ToolStripMenuItem_Refresh
        '
        Me.ToolStripMenuItem_Refresh.Image = Global.BasicPawn.My.Resources.Resources.shell32_16739_16x16_32
        Me.ToolStripMenuItem_Refresh.Name = "ToolStripMenuItem_Refresh"
        Me.ToolStripMenuItem_Refresh.Size = New System.Drawing.Size(206, 22)
        Me.ToolStripMenuItem_Refresh.Text = "Refresh"
        '
        'ToolStripMenuItem_UncheckAll
        '
        Me.ToolStripMenuItem_UncheckAll.Name = "ToolStripMenuItem_UncheckAll"
        Me.ToolStripMenuItem_UncheckAll.Size = New System.Drawing.Size(206, 22)
        Me.ToolStripMenuItem_UncheckAll.Text = "Uncheck all"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(203, 6)
        '
        'ToolStripMenuItem_CopyChecked
        '
        Me.ToolStripMenuItem_CopyChecked.Image = Global.BasicPawn.My.Resources.Resources.imageres_5301_16x16
        Me.ToolStripMenuItem_CopyChecked.Name = "ToolStripMenuItem_CopyChecked"
        Me.ToolStripMenuItem_CopyChecked.Size = New System.Drawing.Size(206, 22)
        Me.ToolStripMenuItem_CopyChecked.Text = "Copy checked items to..."
        '
        'ToolStripMenuItem_MoveChecked
        '
        Me.ToolStripMenuItem_MoveChecked.Image = Global.BasicPawn.My.Resources.Resources.shell32_16762_16x16_32
        Me.ToolStripMenuItem_MoveChecked.Name = "ToolStripMenuItem_MoveChecked"
        Me.ToolStripMenuItem_MoveChecked.Size = New System.Drawing.Size(206, 22)
        Me.ToolStripMenuItem_MoveChecked.Text = "Move checked items to..."
        '
        'ToolStripMenuItem_CloseChecked
        '
        Me.ToolStripMenuItem_CloseChecked.Image = Global.BasicPawn.My.Resources.Resources.imageres_5337_16x16
        Me.ToolStripMenuItem_CloseChecked.Name = "ToolStripMenuItem_CloseChecked"
        Me.ToolStripMenuItem_CloseChecked.Size = New System.Drawing.Size(206, 22)
        Me.ToolStripMenuItem_CloseChecked.Text = "Close checked items"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(203, 6)
        '
        'ToolStripMenuItem_CloseInstChecked
        '
        Me.ToolStripMenuItem_CloseInstChecked.Image = Global.BasicPawn.My.Resources.Resources.imageres_5337_16x16
        Me.ToolStripMenuItem_CloseInstChecked.Name = "ToolStripMenuItem_CloseInstChecked"
        Me.ToolStripMenuItem_CloseInstChecked.Size = New System.Drawing.Size(206, 22)
        Me.ToolStripMenuItem_CloseInstChecked.Text = "Close checked instances"
        '
        'TreeViewColumns_Instances
        '
        Me.TreeViewColumns_Instances.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TreeViewColumns_Instances.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TreeViewColumns_Instances.Location = New System.Drawing.Point(12, 48)
        Me.TreeViewColumns_Instances.m_GridView = False
        Me.TreeViewColumns_Instances.Name = "TreeViewColumns_Instances"
        Me.TreeViewColumns_Instances.Size = New System.Drawing.Size(600, 320)
        Me.TreeViewColumns_Instances.TabIndex = 6
        '
        'ToolStripMenuItem_PopoutChecked
        '
        Me.ToolStripMenuItem_PopoutChecked.Image = Global.BasicPawn.My.Resources.Resources.imageres_5333_16x16
        Me.ToolStripMenuItem_PopoutChecked.Name = "ToolStripMenuItem_PopoutChecked"
        Me.ToolStripMenuItem_PopoutChecked.Size = New System.Drawing.Size(206, 22)
        Me.ToolStripMenuItem_PopoutChecked.Text = "Popout checked items"
        '
        'FormInstanceManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(624, 441)
        Me.Controls.Add(Me.TreeViewColumns_Instances)
        Me.Controls.Add(Me.LinkLabel_Refresh)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel_FooterControl)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Black
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormInstanceManager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Instance Manager"
        Me.TopMost = True
        Me.Panel_FooterControl.ResumeLayout(False)
        Me.ContextMenuStrip_Instances.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel_FooterControl As Panel
    Friend WithEvents Panel_FooterDarkControl As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents LinkLabel_Refresh As LinkLabel
    Friend WithEvents Button_Close As Button
    Friend WithEvents TreeViewColumns_Instances As ClassTreeViewColumns
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ImageList_Instances As ImageList
    Friend WithEvents ContextMenuStrip_Instances As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_UncheckAll As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_CopyChecked As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_MoveChecked As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_CloseChecked As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_CloseInstChecked As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Refresh As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_PopoutChecked As ToolStripMenuItem
End Class
