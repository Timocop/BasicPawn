<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormReportManager
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormReportManager))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetReportsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripProgressBar_Progress = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripStatusLabel_Progress = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripSplitButton_ProgressAbort = New System.Windows.Forms.ToolStripSplitButton()
        Me.UcReportList_Reports = New BasicPawnPluginAutoErrorReport.UCReportList()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.GetReportsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.MenuStrip1.Size = New System.Drawing.Size(736, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseToolStripMenuItem})
        Me.FileToolStripMenuItem.Image = Global.BasicPawnPluginAutoErrorReport.My.Resources.Resources.imageres_5306_16x16_32
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(53, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Image = Global.BasicPawnPluginAutoErrorReport.My.Resources.Resources.imageres_5337_16x16_32
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.CloseToolStripMenuItem.Text = "&Close"
        '
        'GetReportsToolStripMenuItem
        '
        Me.GetReportsToolStripMenuItem.Image = Global.BasicPawnPluginAutoErrorReport.My.Resources.Resources.miguiresource_500_16x16_32
        Me.GetReportsToolStripMenuItem.Name = "GetReportsToolStripMenuItem"
        Me.GetReportsToolStripMenuItem.Size = New System.Drawing.Size(104, 20)
        Me.GetReportsToolStripMenuItem.Text = "Fetch &reports"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripProgressBar_Progress, Me.ToolStripStatusLabel_Progress, Me.ToolStripSplitButton_ProgressAbort})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 508)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(736, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripProgressBar_Progress
        '
        Me.ToolStripProgressBar_Progress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripProgressBar_Progress.Name = "ToolStripProgressBar_Progress"
        Me.ToolStripProgressBar_Progress.Size = New System.Drawing.Size(100, 16)
        Me.ToolStripProgressBar_Progress.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.ToolStripProgressBar_Progress.Visible = False
        '
        'ToolStripStatusLabel_Progress
        '
        Me.ToolStripStatusLabel_Progress.Name = "ToolStripStatusLabel_Progress"
        Me.ToolStripStatusLabel_Progress.Size = New System.Drawing.Size(59, 17)
        Me.ToolStripStatusLabel_Progress.Text = "Loading..."
        Me.ToolStripStatusLabel_Progress.Visible = False
        '
        'ToolStripSplitButton_ProgressAbort
        '
        Me.ToolStripSplitButton_ProgressAbort.DropDownButtonWidth = 0
        Me.ToolStripSplitButton_ProgressAbort.Image = Global.BasicPawnPluginAutoErrorReport.My.Resources.Resources.imageres_5337_16x16_32
        Me.ToolStripSplitButton_ProgressAbort.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripSplitButton_ProgressAbort.Name = "ToolStripSplitButton_ProgressAbort"
        Me.ToolStripSplitButton_ProgressAbort.Size = New System.Drawing.Size(58, 20)
        Me.ToolStripSplitButton_ProgressAbort.Text = "Abort"
        Me.ToolStripSplitButton_ProgressAbort.Visible = False
        '
        'UcReportList_Reports
        '
        Me.UcReportList_Reports.AutoScroll = True
        Me.UcReportList_Reports.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UcReportList_Reports.Location = New System.Drawing.Point(0, 24)
        Me.UcReportList_Reports.Name = "UcReportList_Reports"
        Me.UcReportList_Reports.Size = New System.Drawing.Size(736, 484)
        Me.UcReportList_Reports.TabIndex = 1
        '
        'FormReportManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(736, 530)
        Me.Controls.Add(Me.UcReportList_Reports)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FormReportManager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Report Manager"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetReportsToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents UcReportList_Reports As UCReportList
    Friend WithEvents StatusStrip1 As Windows.Forms.StatusStrip
    Friend WithEvents ToolStripProgressBar_Progress As Windows.Forms.ToolStripProgressBar
    Friend WithEvents ToolStripStatusLabel_Progress As Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripSplitButton_ProgressAbort As Windows.Forms.ToolStripSplitButton
End Class
