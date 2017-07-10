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
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuStrip_ProjectFiles = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_Open = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ProjectSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_Cut = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Copy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Paste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_AddTab = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_AddNewTabs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_AddAllTabs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_AddFiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_Exlcude = New System.Windows.Forms.ToolStripMenuItem()
        Me.TextboxWatermark_Search = New BasicPawn.ClassTextboxWatermark()
        Me.ToolStripMenuItem_CompileAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_TestAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_ProjectFiles.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListView_ProjectFiles
        '
        Me.ListView_ProjectFiles.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListView_ProjectFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.ListView_ProjectFiles.ContextMenuStrip = Me.ContextMenuStrip_ProjectFiles
        Me.ListView_ProjectFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_ProjectFiles.HideSelection = False
        Me.ListView_ProjectFiles.Location = New System.Drawing.Point(0, 22)
        Me.ListView_ProjectFiles.Name = "ListView_ProjectFiles"
        Me.ListView_ProjectFiles.Size = New System.Drawing.Size(276, 471)
        Me.ListView_ProjectFiles.TabIndex = 0
        Me.ListView_ProjectFiles.UseCompatibleStateImageBehavior = False
        Me.ListView_ProjectFiles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "File"
        Me.ColumnHeader1.Width = 250
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Path"
        Me.ColumnHeader2.Width = 500
        '
        'ContextMenuStrip_ProjectFiles
        '
        Me.ContextMenuStrip_ProjectFiles.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_Open, Me.ToolStripMenuItem_ProjectSave, Me.ToolStripSeparator1, Me.ToolStripMenuItem_Cut, Me.ToolStripMenuItem_Copy, Me.ToolStripMenuItem_Paste, Me.ToolStripSeparator4, Me.ToolStripMenuItem_CompileAll, Me.ToolStripMenuItem_TestAll, Me.ToolStripSeparator3, Me.ToolStripMenuItem_AddTab, Me.ToolStripMenuItem_AddNewTabs, Me.ToolStripMenuItem_AddAllTabs, Me.ToolStripMenuItem_AddFiles, Me.ToolStripSeparator2, Me.ToolStripMenuItem_Exlcude})
        Me.ContextMenuStrip_ProjectFiles.Name = "ContextMenuStrip_ProjectFiles"
        Me.ContextMenuStrip_ProjectFiles.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_ProjectFiles.Size = New System.Drawing.Size(184, 314)
        '
        'ToolStripMenuItem_Open
        '
        Me.ToolStripMenuItem_Open.Image = Global.BasicPawn.My.Resources.Resources.imageres_5339_16x16_32
        Me.ToolStripMenuItem_Open.Name = "ToolStripMenuItem_Open"
        Me.ToolStripMenuItem_Open.Size = New System.Drawing.Size(183, 22)
        Me.ToolStripMenuItem_Open.Text = "Open"
        '
        'ToolStripMenuItem_ProjectSave
        '
        Me.ToolStripMenuItem_ProjectSave.Image = Global.BasicPawn.My.Resources.Resources.imageres_5303_16x16_32
        Me.ToolStripMenuItem_ProjectSave.Name = "ToolStripMenuItem_ProjectSave"
        Me.ToolStripMenuItem_ProjectSave.Size = New System.Drawing.Size(183, 22)
        Me.ToolStripMenuItem_ProjectSave.Text = "Save Project"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(180, 6)
        '
        'ToolStripMenuItem_Cut
        '
        Me.ToolStripMenuItem_Cut.Image = Global.BasicPawn.My.Resources.Resources.imageres_5314_16x16_32
        Me.ToolStripMenuItem_Cut.Name = "ToolStripMenuItem_Cut"
        Me.ToolStripMenuItem_Cut.Size = New System.Drawing.Size(183, 22)
        Me.ToolStripMenuItem_Cut.Text = "Cut"
        '
        'ToolStripMenuItem_Copy
        '
        Me.ToolStripMenuItem_Copy.Image = Global.BasicPawn.My.Resources.Resources.imageres_5350_16x16_32
        Me.ToolStripMenuItem_Copy.Name = "ToolStripMenuItem_Copy"
        Me.ToolStripMenuItem_Copy.Size = New System.Drawing.Size(183, 22)
        Me.ToolStripMenuItem_Copy.Text = "Copy"
        '
        'ToolStripMenuItem_Paste
        '
        Me.ToolStripMenuItem_Paste.Image = Global.BasicPawn.My.Resources.Resources.imageres_5301_16x16_32
        Me.ToolStripMenuItem_Paste.Name = "ToolStripMenuItem_Paste"
        Me.ToolStripMenuItem_Paste.Size = New System.Drawing.Size(183, 22)
        Me.ToolStripMenuItem_Paste.Text = "Paste"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(180, 6)
        '
        'ToolStripMenuItem_AddTab
        '
        Me.ToolStripMenuItem_AddTab.Image = Global.BasicPawn.My.Resources.Resources.imageres_5367_16x16_32
        Me.ToolStripMenuItem_AddTab.Name = "ToolStripMenuItem_AddTab"
        Me.ToolStripMenuItem_AddTab.Size = New System.Drawing.Size(183, 22)
        Me.ToolStripMenuItem_AddTab.Text = "Add this tab"
        Me.ToolStripMenuItem_AddTab.ToolTipText = "Adds the current tab to the project"
        '
        'ToolStripMenuItem_AddNewTabs
        '
        Me.ToolStripMenuItem_AddNewTabs.Name = "ToolStripMenuItem_AddNewTabs"
        Me.ToolStripMenuItem_AddNewTabs.Size = New System.Drawing.Size(183, 22)
        Me.ToolStripMenuItem_AddNewTabs.Text = "Add new tabs"
        Me.ToolStripMenuItem_AddNewTabs.ToolTipText = "Adds all new tabs to the project."
        '
        'ToolStripMenuItem_AddAllTabs
        '
        Me.ToolStripMenuItem_AddAllTabs.Name = "ToolStripMenuItem_AddAllTabs"
        Me.ToolStripMenuItem_AddAllTabs.Size = New System.Drawing.Size(183, 22)
        Me.ToolStripMenuItem_AddAllTabs.Text = "Add all tabs"
        Me.ToolStripMenuItem_AddAllTabs.ToolTipText = "Adds all tabs to the project."
        '
        'ToolStripMenuItem_AddFiles
        '
        Me.ToolStripMenuItem_AddFiles.Name = "ToolStripMenuItem_AddFiles"
        Me.ToolStripMenuItem_AddFiles.Size = New System.Drawing.Size(183, 22)
        Me.ToolStripMenuItem_AddFiles.Text = "Add files..."
        Me.ToolStripMenuItem_AddFiles.ToolTipText = "Adds custom files to the project."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(180, 6)
        '
        'ToolStripMenuItem_Exlcude
        '
        Me.ToolStripMenuItem_Exlcude.Image = Global.BasicPawn.My.Resources.Resources.imageres_5337_16x16_32
        Me.ToolStripMenuItem_Exlcude.Name = "ToolStripMenuItem_Exlcude"
        Me.ToolStripMenuItem_Exlcude.Size = New System.Drawing.Size(183, 22)
        Me.ToolStripMenuItem_Exlcude.Text = "Exclude from project"
        Me.ToolStripMenuItem_Exlcude.ToolTipText = "Exclude files from the project."
        '
        'TextboxWatermark_Search
        '
        Me.TextboxWatermark_Search.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextboxWatermark_Search.Location = New System.Drawing.Point(0, 0)
        Me.TextboxWatermark_Search.m_sWatermarkText = "Search..."
        Me.TextboxWatermark_Search.Name = "TextboxWatermark_Search"
        Me.TextboxWatermark_Search.Size = New System.Drawing.Size(276, 22)
        Me.TextboxWatermark_Search.TabIndex = 1
        Me.TextboxWatermark_Search.Text = "Search..."
        '
        'ToolStripMenuItem_CompileAll
        '
        Me.ToolStripMenuItem_CompileAll.Image = Global.BasicPawn.My.Resources.Resources.imageres_5341_16x16_32
        Me.ToolStripMenuItem_CompileAll.Name = "ToolStripMenuItem_CompileAll"
        Me.ToolStripMenuItem_CompileAll.Size = New System.Drawing.Size(183, 22)
        Me.ToolStripMenuItem_CompileAll.Text = "Compile all"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(180, 6)
        '
        'ToolStripMenuItem_TestAll
        '
        Me.ToolStripMenuItem_TestAll.Image = Global.BasicPawn.My.Resources.Resources.imageres_5342_16x16_32
        Me.ToolStripMenuItem_TestAll.Name = "ToolStripMenuItem_TestAll"
        Me.ToolStripMenuItem_TestAll.Size = New System.Drawing.Size(183, 22)
        Me.ToolStripMenuItem_TestAll.Text = "Test all"
        '
        'UCProjectBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.ListView_ProjectFiles)
        Me.Controls.Add(Me.TextboxWatermark_Search)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UCProjectBrowser"
        Me.Size = New System.Drawing.Size(276, 493)
        Me.ContextMenuStrip_ProjectFiles.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ListView_ProjectFiles As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ContextMenuStrip_ProjectFiles As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_Open As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_Cut As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Copy As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Paste As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_Exlcude As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_AddTab As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_AddAllTabs As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_AddFiles As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_AddNewTabs As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ProjectSave As ToolStripMenuItem
    Friend WithEvents TextboxWatermark_Search As ClassTextboxWatermark
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_CompileAll As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_TestAll As ToolStripMenuItem
End Class
