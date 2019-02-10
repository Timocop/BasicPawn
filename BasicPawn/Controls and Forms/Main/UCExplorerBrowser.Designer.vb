<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UCExplorerBrowser
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.ListView_ExplorerFiles = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuStrip_ExplorerBrowser = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_OpenFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_Filter = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList_ExplorerBrowser = New System.Windows.Forms.ImageList(Me.components)
        Me.MenuStrip_ExplorerBrowser = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem_DirectoryUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Home = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Refresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripTextBox_Path = New System.Windows.Forms.ToolStripTextBox()
        Me.TextboxWatermark_Search = New BasicPawn.ClassTextboxWatermark()
        Me.ContextMenuStrip_ExplorerBrowser.SuspendLayout()
        Me.MenuStrip_ExplorerBrowser.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListView_ExplorerFiles
        '
        Me.ListView_ExplorerFiles.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListView_ExplorerFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.ListView_ExplorerFiles.ContextMenuStrip = Me.ContextMenuStrip_ExplorerBrowser
        Me.ListView_ExplorerFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_ExplorerFiles.HideSelection = False
        Me.ListView_ExplorerFiles.Location = New System.Drawing.Point(0, 46)
        Me.ListView_ExplorerFiles.Name = "ListView_ExplorerFiles"
        Me.ListView_ExplorerFiles.Size = New System.Drawing.Size(269, 610)
        Me.ListView_ExplorerFiles.SmallImageList = Me.ImageList_ExplorerBrowser
        Me.ListView_ExplorerFiles.TabIndex = 1
        Me.ListView_ExplorerFiles.UseCompatibleStateImageBehavior = False
        Me.ListView_ExplorerFiles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 150
        '
        'ContextMenuStrip_ExplorerBrowser
        '
        Me.ContextMenuStrip_ExplorerBrowser.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_OpenFile, Me.ToolStripSeparator1, Me.ToolStripMenuItem_Filter})
        Me.ContextMenuStrip_ExplorerBrowser.Name = "ContextMenuStrip_ExplorerBrowser"
        Me.ContextMenuStrip_ExplorerBrowser.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_ExplorerBrowser.Size = New System.Drawing.Size(214, 54)
        '
        'ToolStripMenuItem_OpenFile
        '
        Me.ToolStripMenuItem_OpenFile.Image = Global.BasicPawn.My.Resources.Resources.imageres_5338_16x16
        Me.ToolStripMenuItem_OpenFile.Name = "ToolStripMenuItem_OpenFile"
        Me.ToolStripMenuItem_OpenFile.Size = New System.Drawing.Size(213, 22)
        Me.ToolStripMenuItem_OpenFile.Text = "Open"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(210, 6)
        '
        'ToolStripMenuItem_Filter
        '
        Me.ToolStripMenuItem_Filter.CheckOnClick = True
        Me.ToolStripMenuItem_Filter.Name = "ToolStripMenuItem_Filter"
        Me.ToolStripMenuItem_Filter.Size = New System.Drawing.Size(213, 22)
        Me.ToolStripMenuItem_Filter.Text = "Filter sources and includes"
        '
        'ImageList_ExplorerBrowser
        '
        Me.ImageList_ExplorerBrowser.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList_ExplorerBrowser.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList_ExplorerBrowser.TransparentColor = System.Drawing.Color.Transparent
        '
        'MenuStrip_ExplorerBrowser
        '
        Me.MenuStrip_ExplorerBrowser.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_DirectoryUp, Me.ToolStripMenuItem_Home, Me.ToolStripMenuItem_Refresh, Me.ToolStripTextBox_Path})
        Me.MenuStrip_ExplorerBrowser.Location = New System.Drawing.Point(0, 22)
        Me.MenuStrip_ExplorerBrowser.Name = "MenuStrip_ExplorerBrowser"
        Me.MenuStrip_ExplorerBrowser.Padding = New System.Windows.Forms.Padding(0)
        Me.MenuStrip_ExplorerBrowser.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.MenuStrip_ExplorerBrowser.ShowItemToolTips = True
        Me.MenuStrip_ExplorerBrowser.Size = New System.Drawing.Size(269, 24)
        Me.MenuStrip_ExplorerBrowser.TabIndex = 2
        Me.MenuStrip_ExplorerBrowser.Text = "MenuStrip1"
        '
        'ToolStripMenuItem_DirectoryUp
        '
        Me.ToolStripMenuItem_DirectoryUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripMenuItem_DirectoryUp.Image = Global.BasicPawn.My.Resources.Resources.imageres_5339_16x16
        Me.ToolStripMenuItem_DirectoryUp.Name = "ToolStripMenuItem_DirectoryUp"
        Me.ToolStripMenuItem_DirectoryUp.Size = New System.Drawing.Size(28, 24)
        Me.ToolStripMenuItem_DirectoryUp.Text = "Directory up"
        Me.ToolStripMenuItem_DirectoryUp.ToolTipText = "Directory up"
        '
        'ToolStripMenuItem_Home
        '
        Me.ToolStripMenuItem_Home.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripMenuItem_Home.Image = Global.BasicPawn.My.Resources.Resources.imageres_5304_16x16
        Me.ToolStripMenuItem_Home.Name = "ToolStripMenuItem_Home"
        Me.ToolStripMenuItem_Home.Size = New System.Drawing.Size(28, 24)
        Me.ToolStripMenuItem_Home.Text = "Home"
        Me.ToolStripMenuItem_Home.ToolTipText = "Home"
        '
        'ToolStripMenuItem_Refresh
        '
        Me.ToolStripMenuItem_Refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripMenuItem_Refresh.Image = Global.BasicPawn.My.Resources.Resources.shell32_16739_16x16
        Me.ToolStripMenuItem_Refresh.Name = "ToolStripMenuItem_Refresh"
        Me.ToolStripMenuItem_Refresh.Size = New System.Drawing.Size(28, 24)
        Me.ToolStripMenuItem_Refresh.Text = "Refresh"
        Me.ToolStripMenuItem_Refresh.ToolTipText = "Refresh"
        '
        'ToolStripTextBox_Path
        '
        Me.ToolStripTextBox_Path.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripTextBox_Path.Name = "ToolStripTextBox_Path"
        Me.ToolStripTextBox_Path.ReadOnly = True
        Me.ToolStripTextBox_Path.Size = New System.Drawing.Size(100, 24)
        '
        'TextboxWatermark_Search
        '
        Me.TextboxWatermark_Search.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextboxWatermark_Search.Location = New System.Drawing.Point(0, 0)
        Me.TextboxWatermark_Search.m_WatermarkText = "Search..."
        Me.TextboxWatermark_Search.Margin = New System.Windows.Forms.Padding(0)
        Me.TextboxWatermark_Search.Name = "TextboxWatermark_Search"
        Me.TextboxWatermark_Search.Size = New System.Drawing.Size(269, 22)
        Me.TextboxWatermark_Search.TabIndex = 0
        '
        'UCExplorerBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.ListView_ExplorerFiles)
        Me.Controls.Add(Me.MenuStrip_ExplorerBrowser)
        Me.Controls.Add(Me.TextboxWatermark_Search)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UCExplorerBrowser"
        Me.Size = New System.Drawing.Size(269, 656)
        Me.ContextMenuStrip_ExplorerBrowser.ResumeLayout(False)
        Me.MenuStrip_ExplorerBrowser.ResumeLayout(False)
        Me.MenuStrip_ExplorerBrowser.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextboxWatermark_Search As ClassTextboxWatermark
    Friend WithEvents MenuStrip_ExplorerBrowser As MenuStrip
    Friend WithEvents ToolStripMenuItem_Home As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_DirectoryUp As ToolStripMenuItem
    Friend WithEvents ImageList_ExplorerBrowser As ImageList
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ToolStripMenuItem_Refresh As ToolStripMenuItem
    Friend WithEvents ToolStripTextBox_Path As ToolStripTextBox
    Friend WithEvents ContextMenuStrip_ExplorerBrowser As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_OpenFile As ToolStripMenuItem
    Public WithEvents ListView_ExplorerFiles As ListView
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_Filter As ToolStripMenuItem
End Class
