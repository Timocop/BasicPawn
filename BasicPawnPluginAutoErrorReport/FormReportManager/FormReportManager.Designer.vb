﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormReportManager))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem_File = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_GetReports = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_GetLogs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_CloseReportWindows = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList_Logs = New System.Windows.Forms.ImageList(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.TabControl1 = New BasicPawn.ClassTabControlColor()
        Me.TabPage_Reports = New System.Windows.Forms.TabPage()
        Me.ReportListBox_Reports = New BasicPawnPluginAutoErrorReport.ClassReportListBox()
        Me.TabPage_Logs = New System.Windows.Forms.TabPage()
        Me.MenuStrip1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage_Reports.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_File, Me.ToolStripMenuItem_GetReports, Me.ToolStripMenuItem_GetLogs, Me.ToolStripMenuItem_CloseReportWindows})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.MenuStrip1.Size = New System.Drawing.Size(736, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ToolStripMenuItem_File
        '
        Me.ToolStripMenuItem_File.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CloseToolStripMenuItem})
        Me.ToolStripMenuItem_File.Image = Global.BasicPawnPluginAutoErrorReport.My.Resources.Resources.imageres_5306_16x16_32
        Me.ToolStripMenuItem_File.Name = "ToolStripMenuItem_File"
        Me.ToolStripMenuItem_File.Size = New System.Drawing.Size(53, 20)
        Me.ToolStripMenuItem_File.Text = "&File"
        '
        'CloseToolStripMenuItem
        '
        Me.CloseToolStripMenuItem.Image = Global.BasicPawnPluginAutoErrorReport.My.Resources.Resources.imageres_5337_16x16_32
        Me.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem"
        Me.CloseToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.CloseToolStripMenuItem.Text = "&Close"
        '
        'ToolStripMenuItem_GetReports
        '
        Me.ToolStripMenuItem_GetReports.Image = Global.BasicPawnPluginAutoErrorReport.My.Resources.Resources.miguiresource_500_16x16_32
        Me.ToolStripMenuItem_GetReports.Name = "ToolStripMenuItem_GetReports"
        Me.ToolStripMenuItem_GetReports.Size = New System.Drawing.Size(104, 20)
        Me.ToolStripMenuItem_GetReports.Text = "Fetch &reports"
        '
        'ToolStripMenuItem_GetLogs
        '
        Me.ToolStripMenuItem_GetLogs.Image = CType(resources.GetObject("ToolStripMenuItem_GetLogs.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_GetLogs.Name = "ToolStripMenuItem_GetLogs"
        Me.ToolStripMenuItem_GetLogs.Size = New System.Drawing.Size(89, 20)
        Me.ToolStripMenuItem_GetLogs.Text = "Fetch &logs"
        '
        'ToolStripMenuItem_CloseReportWindows
        '
        Me.ToolStripMenuItem_CloseReportWindows.Image = CType(resources.GetObject("ToolStripMenuItem_CloseReportWindows.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_CloseReportWindows.Name = "ToolStripMenuItem_CloseReportWindows"
        Me.ToolStripMenuItem_CloseReportWindows.Size = New System.Drawing.Size(149, 20)
        Me.ToolStripMenuItem_CloseReportWindows.Text = "Close report windows"
        '
        'ImageList_Logs
        '
        Me.ImageList_Logs.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList_Logs.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList_Logs.TransparentColor = System.Drawing.Color.Transparent
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 508)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(736, 22)
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage_Reports)
        Me.TabControl1.Controls.Add(Me.TabPage_Logs)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 24)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(736, 484)
        Me.TabControl1.TabIndex = 3
        '
        'TabPage_Reports
        '
        Me.TabPage_Reports.Controls.Add(Me.ReportListBox_Reports)
        Me.TabPage_Reports.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Reports.Name = "TabPage_Reports"
        Me.TabPage_Reports.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Reports.Size = New System.Drawing.Size(728, 458)
        Me.TabPage_Reports.TabIndex = 0
        Me.TabPage_Reports.Text = "Reports"
        Me.TabPage_Reports.UseVisualStyleBackColor = True
        '
        'ReportListBox_Reports
        '
        Me.ReportListBox_Reports.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ReportListBox_Reports.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ReportListBox_Reports.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ReportListBox_Reports.FormattingEnabled = True
        Me.ReportListBox_Reports.ItemHeight = 32
        Me.ReportListBox_Reports.Location = New System.Drawing.Point(3, 3)
        Me.ReportListBox_Reports.Name = "ReportListBox_Reports"
        Me.ReportListBox_Reports.Size = New System.Drawing.Size(722, 452)
        Me.ReportListBox_Reports.TabIndex = 0
        '
        'TabPage_Logs
        '
        Me.TabPage_Logs.Location = New System.Drawing.Point(4, 22)
        Me.TabPage_Logs.Name = "TabPage_Logs"
        Me.TabPage_Logs.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage_Logs.Size = New System.Drawing.Size(728, 458)
        Me.TabPage_Logs.TabIndex = 1
        Me.TabPage_Logs.Text = "Logs"
        Me.TabPage_Logs.UseVisualStyleBackColor = True
        '
        'FormReportManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(736, 530)
        Me.Controls.Add(Me.TabControl1)
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
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage_Reports.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As Windows.Forms.MenuStrip
    Friend WithEvents ToolStripMenuItem_File As Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_GetReports As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_CloseReportWindows As Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabControl1 As BasicPawn.ClassTabControlColor
    Friend WithEvents TabPage_Reports As Windows.Forms.TabPage
    Friend WithEvents TabPage_Logs As Windows.Forms.TabPage
    Friend WithEvents ToolStripMenuItem_GetLogs As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImageList_Logs As Windows.Forms.ImageList
    Friend WithEvents StatusStrip1 As Windows.Forms.StatusStrip
    Friend WithEvents ReportListBox_Reports As ClassReportListBox
End Class
