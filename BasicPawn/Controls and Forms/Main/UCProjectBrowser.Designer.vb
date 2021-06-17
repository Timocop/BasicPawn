<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCProjectBrowser
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ListView_ProjectFiles = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuStrip_ProjectFiles = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_Open = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ProjectLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ProjectSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ProjectSaveAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ProjectClose = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_Cut = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Copy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Paste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_CompileAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TestAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_Settings = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList_ProjectBrowser = New System.Windows.Forms.ImageList(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem_MenuProjectLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_MenuProjectSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_MenuProjectHome = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_MenuProjectDirUp = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_MenuProjectRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.TextboxWatermark_Search = New BasicPawn.ClassTextboxWatermark()
        Me.TextBox_ProjectPath = New System.Windows.Forms.TextBox()
        Me.ContextMenuStrip_ProjectFiles.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListView_ProjectFiles
        '
        Me.ListView_ProjectFiles.AllowDrop = True
        Me.ListView_ProjectFiles.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListView_ProjectFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.ListView_ProjectFiles.ContextMenuStrip = Me.ContextMenuStrip_ProjectFiles
        Me.ListView_ProjectFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_ProjectFiles.FullRowSelect = True
        Me.ListView_ProjectFiles.HideSelection = False
        Me.ListView_ProjectFiles.Location = New System.Drawing.Point(0, 68)
        Me.ListView_ProjectFiles.Margin = New System.Windows.Forms.Padding(0)
        Me.ListView_ProjectFiles.Name = "ListView_ProjectFiles"
        Me.ListView_ProjectFiles.ShowItemToolTips = True
        Me.ListView_ProjectFiles.Size = New System.Drawing.Size(276, 425)
        Me.ListView_ProjectFiles.SmallImageList = Me.ImageList_ProjectBrowser
        Me.ListView_ProjectFiles.TabIndex = 0
        Me.ListView_ProjectFiles.UseCompatibleStateImageBehavior = False
        Me.ListView_ProjectFiles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 150
        '
        'ContextMenuStrip_ProjectFiles
        '
        Me.ContextMenuStrip_ProjectFiles.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_Open, Me.ToolStripSeparator7, Me.ToolStripMenuItem_ProjectLoad, Me.ToolStripMenuItem_ProjectSave, Me.ToolStripMenuItem_ProjectSaveAs, Me.ToolStripMenuItem_ProjectClose, Me.ToolStripSeparator1, Me.ToolStripMenuItem_Cut, Me.ToolStripMenuItem_Copy, Me.ToolStripMenuItem_Paste, Me.ToolStripSeparator4, Me.ToolStripMenuItem_CompileAll, Me.ToolStripMenuItem_TestAll, Me.ToolStripSeparator2, Me.ToolStripMenuItem_Settings})
        Me.ContextMenuStrip_ProjectFiles.Name = "ContextMenuStrip_ProjectFiles"
        Me.ContextMenuStrip_ProjectFiles.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_ProjectFiles.Size = New System.Drawing.Size(162, 270)
        '
        'ToolStripMenuItem_Open
        '
        Me.ToolStripMenuItem_Open.Image = Global.BasicPawn.My.Resources.Resources.imageres_5338_16x16
        Me.ToolStripMenuItem_Open.Name = "ToolStripMenuItem_Open"
        Me.ToolStripMenuItem_Open.Size = New System.Drawing.Size(161, 22)
        Me.ToolStripMenuItem_Open.Text = "Open"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(158, 6)
        '
        'ToolStripMenuItem_ProjectLoad
        '
        Me.ToolStripMenuItem_ProjectLoad.Image = Global.BasicPawn.My.Resources.Resources.imageres_5339_16x16
        Me.ToolStripMenuItem_ProjectLoad.Name = "ToolStripMenuItem_ProjectLoad"
        Me.ToolStripMenuItem_ProjectLoad.Size = New System.Drawing.Size(161, 22)
        Me.ToolStripMenuItem_ProjectLoad.Text = "Load Project"
        '
        'ToolStripMenuItem_ProjectSave
        '
        Me.ToolStripMenuItem_ProjectSave.Image = Global.BasicPawn.My.Resources.Resources.imageres_5304_16x16
        Me.ToolStripMenuItem_ProjectSave.Name = "ToolStripMenuItem_ProjectSave"
        Me.ToolStripMenuItem_ProjectSave.Size = New System.Drawing.Size(161, 22)
        Me.ToolStripMenuItem_ProjectSave.Text = "Save Project"
        '
        'ToolStripMenuItem_ProjectSaveAs
        '
        Me.ToolStripMenuItem_ProjectSaveAs.Name = "ToolStripMenuItem_ProjectSaveAs"
        Me.ToolStripMenuItem_ProjectSaveAs.Size = New System.Drawing.Size(161, 22)
        Me.ToolStripMenuItem_ProjectSaveAs.Text = "Save Project as..."
        '
        'ToolStripMenuItem_ProjectClose
        '
        Me.ToolStripMenuItem_ProjectClose.Image = Global.BasicPawn.My.Resources.Resources.imageres_5320_16x16
        Me.ToolStripMenuItem_ProjectClose.Name = "ToolStripMenuItem_ProjectClose"
        Me.ToolStripMenuItem_ProjectClose.Size = New System.Drawing.Size(161, 22)
        Me.ToolStripMenuItem_ProjectClose.Text = "Close Project"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(158, 6)
        '
        'ToolStripMenuItem_Cut
        '
        Me.ToolStripMenuItem_Cut.Image = Global.BasicPawn.My.Resources.Resources.shell32_16762_16x16
        Me.ToolStripMenuItem_Cut.Name = "ToolStripMenuItem_Cut"
        Me.ToolStripMenuItem_Cut.Size = New System.Drawing.Size(161, 22)
        Me.ToolStripMenuItem_Cut.Text = "Cut"
        '
        'ToolStripMenuItem_Copy
        '
        Me.ToolStripMenuItem_Copy.Image = Global.BasicPawn.My.Resources.Resources.imageres_5350_16x16
        Me.ToolStripMenuItem_Copy.Name = "ToolStripMenuItem_Copy"
        Me.ToolStripMenuItem_Copy.Size = New System.Drawing.Size(161, 22)
        Me.ToolStripMenuItem_Copy.Text = "Copy"
        '
        'ToolStripMenuItem_Paste
        '
        Me.ToolStripMenuItem_Paste.Image = Global.BasicPawn.My.Resources.Resources.shell32_16763_16x16
        Me.ToolStripMenuItem_Paste.Name = "ToolStripMenuItem_Paste"
        Me.ToolStripMenuItem_Paste.Size = New System.Drawing.Size(161, 22)
        Me.ToolStripMenuItem_Paste.Text = "Paste"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(158, 6)
        '
        'ToolStripMenuItem_CompileAll
        '
        Me.ToolStripMenuItem_CompileAll.Image = Global.BasicPawn.My.Resources.Resources.imageres_5341_16x16
        Me.ToolStripMenuItem_CompileAll.Name = "ToolStripMenuItem_CompileAll"
        Me.ToolStripMenuItem_CompileAll.Size = New System.Drawing.Size(161, 22)
        Me.ToolStripMenuItem_CompileAll.Text = "Build"
        '
        'ToolStripMenuItem_TestAll
        '
        Me.ToolStripMenuItem_TestAll.Image = Global.BasicPawn.My.Resources.Resources.imageres_5342_16x16
        Me.ToolStripMenuItem_TestAll.Name = "ToolStripMenuItem_TestAll"
        Me.ToolStripMenuItem_TestAll.Size = New System.Drawing.Size(161, 22)
        Me.ToolStripMenuItem_TestAll.Text = "Test"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(158, 6)
        '
        'ToolStripMenuItem_Settings
        '
        Me.ToolStripMenuItem_Settings.Image = Global.BasicPawn.My.Resources.Resources.imageres_5364_16x16
        Me.ToolStripMenuItem_Settings.Name = "ToolStripMenuItem_Settings"
        Me.ToolStripMenuItem_Settings.Size = New System.Drawing.Size(161, 22)
        Me.ToolStripMenuItem_Settings.Text = "Project Settings"
        '
        'ImageList_ProjectBrowser
        '
        Me.ImageList_ProjectBrowser.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList_ProjectBrowser.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList_ProjectBrowser.TransparentColor = System.Drawing.Color.Transparent
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_MenuProjectLoad, Me.ToolStripMenuItem_MenuProjectSave, Me.ToolStripMenuItem1, Me.ToolStripMenuItem_MenuProjectHome, Me.ToolStripMenuItem_MenuProjectDirUp, Me.ToolStripMenuItem_MenuProjectRefresh})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 44)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(0)
        Me.MenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.MenuStrip1.ShowItemToolTips = True
        Me.MenuStrip1.Size = New System.Drawing.Size(276, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ToolStripMenuItem_MenuProjectLoad
        '
        Me.ToolStripMenuItem_MenuProjectLoad.AutoToolTip = True
        Me.ToolStripMenuItem_MenuProjectLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripMenuItem_MenuProjectLoad.Image = Global.BasicPawn.My.Resources.Resources.imageres_5339_16x16
        Me.ToolStripMenuItem_MenuProjectLoad.Name = "ToolStripMenuItem_MenuProjectLoad"
        Me.ToolStripMenuItem_MenuProjectLoad.Size = New System.Drawing.Size(28, 24)
        Me.ToolStripMenuItem_MenuProjectLoad.Text = "Load Project"
        Me.ToolStripMenuItem_MenuProjectLoad.ToolTipText = "Load Project"
        '
        'ToolStripMenuItem_MenuProjectSave
        '
        Me.ToolStripMenuItem_MenuProjectSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripMenuItem_MenuProjectSave.Image = Global.BasicPawn.My.Resources.Resources.imageres_5304_16x16
        Me.ToolStripMenuItem_MenuProjectSave.Name = "ToolStripMenuItem_MenuProjectSave"
        Me.ToolStripMenuItem_MenuProjectSave.Size = New System.Drawing.Size(28, 24)
        Me.ToolStripMenuItem_MenuProjectSave.Text = "Save Project"
        Me.ToolStripMenuItem_MenuProjectSave.ToolTipText = "Save Project"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripMenuItem1.Enabled = False
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Padding = New System.Windows.Forms.Padding(0)
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(14, 24)
        Me.ToolStripMenuItem1.Text = "|"
        '
        'ToolStripMenuItem_MenuProjectHome
        '
        Me.ToolStripMenuItem_MenuProjectHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripMenuItem_MenuProjectHome.Image = Global.BasicPawn.My.Resources.Resources.imageres_5303_16x16
        Me.ToolStripMenuItem_MenuProjectHome.Name = "ToolStripMenuItem_MenuProjectHome"
        Me.ToolStripMenuItem_MenuProjectHome.Size = New System.Drawing.Size(28, 24)
        Me.ToolStripMenuItem_MenuProjectHome.Text = "Explorer Home"
        Me.ToolStripMenuItem_MenuProjectHome.ToolTipText = "Explorer Home"
        '
        'ToolStripMenuItem_MenuProjectDirUp
        '
        Me.ToolStripMenuItem_MenuProjectDirUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripMenuItem_MenuProjectDirUp.Image = Global.BasicPawn.My.Resources.Resources.imageres_5339_16x16
        Me.ToolStripMenuItem_MenuProjectDirUp.Name = "ToolStripMenuItem_MenuProjectDirUp"
        Me.ToolStripMenuItem_MenuProjectDirUp.Size = New System.Drawing.Size(28, 24)
        Me.ToolStripMenuItem_MenuProjectDirUp.Text = "Explorer Directory Up"
        Me.ToolStripMenuItem_MenuProjectDirUp.ToolTipText = "Explorer Directory Up"
        '
        'ToolStripMenuItem_MenuProjectRefresh
        '
        Me.ToolStripMenuItem_MenuProjectRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripMenuItem_MenuProjectRefresh.Image = Global.BasicPawn.My.Resources.Resources.shell32_16739_16x16
        Me.ToolStripMenuItem_MenuProjectRefresh.Name = "ToolStripMenuItem_MenuProjectRefresh"
        Me.ToolStripMenuItem_MenuProjectRefresh.Size = New System.Drawing.Size(28, 24)
        Me.ToolStripMenuItem_MenuProjectRefresh.Text = "Explorer Refresh"
        Me.ToolStripMenuItem_MenuProjectRefresh.ToolTipText = "Explorer Refresh"
        '
        'TextboxWatermark_Search
        '
        Me.TextboxWatermark_Search.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextboxWatermark_Search.Location = New System.Drawing.Point(0, 0)
        Me.TextboxWatermark_Search.m_WatermarkText = "Search..."
        Me.TextboxWatermark_Search.Margin = New System.Windows.Forms.Padding(0)
        Me.TextboxWatermark_Search.Name = "TextboxWatermark_Search"
        Me.TextboxWatermark_Search.Size = New System.Drawing.Size(276, 22)
        Me.TextboxWatermark_Search.TabIndex = 1
        '
        'TextBox_ProjectPath
        '
        Me.TextBox_ProjectPath.BackColor = System.Drawing.Color.White
        Me.TextBox_ProjectPath.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBox_ProjectPath.Location = New System.Drawing.Point(0, 22)
        Me.TextBox_ProjectPath.Name = "TextBox_ProjectPath"
        Me.TextBox_ProjectPath.ReadOnly = True
        Me.TextBox_ProjectPath.Size = New System.Drawing.Size(276, 22)
        Me.TextBox_ProjectPath.TabIndex = 3
        '
        'UCProjectBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.ListView_ProjectFiles)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.TextBox_ProjectPath)
        Me.Controls.Add(Me.TextboxWatermark_Search)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UCProjectBrowser"
        Me.Size = New System.Drawing.Size(276, 493)
        Me.ContextMenuStrip_ProjectFiles.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents TextboxWatermark_Search As ClassTextboxWatermark
    Public WithEvents ContextMenuStrip_ProjectFiles As ContextMenuStrip
    Public WithEvents ToolStripMenuItem_Open As ToolStripMenuItem
    Public WithEvents ToolStripSeparator1 As ToolStripSeparator
    Public WithEvents ToolStripMenuItem_Cut As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Copy As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_Paste As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_ProjectSave As ToolStripMenuItem
    Public WithEvents ToolStripSeparator4 As ToolStripSeparator
    Public WithEvents ToolStripMenuItem_CompileAll As ToolStripMenuItem
    Public WithEvents ToolStripMenuItem_TestAll As ToolStripMenuItem
    Public WithEvents ListView_ProjectFiles As ListView
    Friend WithEvents ToolStripMenuItem_ProjectSaveAs As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ProjectClose As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_ProjectLoad As ToolStripMenuItem
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ToolStripMenuItem_MenuProjectLoad As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_MenuProjectSave As ToolStripMenuItem
    Public WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_Settings As ToolStripMenuItem
    Friend WithEvents ImageList_ProjectBrowser As ImageList
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_MenuProjectHome As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_MenuProjectDirUp As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_MenuProjectRefresh As ToolStripMenuItem
    Friend WithEvents TextBox_ProjectPath As TextBox
End Class
