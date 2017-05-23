<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCStartPage
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel_BasicPawnTitle = New System.Windows.Forms.Panel()
        Me.LinkLabel_Open = New System.Windows.Forms.LinkLabel()
        Me.LinkLabel_Close = New System.Windows.Forms.LinkLabel()
        Me.Panel_FooterDarkControl = New System.Windows.Forms.Panel()
        Me.ClassPictureBoxQuality1 = New BasicPawn.ClassPictureBoxQuality()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel_RecentFiles = New System.Windows.Forms.Panel()
        Me.Panel_FooterDarkControl2 = New System.Windows.Forms.Panel()
        Me.Panel_FooterDarkControl3 = New System.Windows.Forms.Panel()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Timer_DelayUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.Label_NoProjectsFound = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.Panel_BasicPawnTitle.SuspendLayout()
        CType(Me.ClassPictureBoxQuality1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel_RecentFiles.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.BackColor = System.Drawing.Color.Transparent
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel_FooterDarkControl2, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel_FooterDarkControl3, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 86.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 86.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(800, 600)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.TableLayoutPanel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 86)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(800, 428)
        Me.Panel1.TabIndex = 0
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Panel_BasicPawnTitle, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Panel_RecentFiles, 0, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(800, 428)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'Panel_BasicPawnTitle
        '
        Me.Panel_BasicPawnTitle.Controls.Add(Me.LinkLabel_Open)
        Me.Panel_BasicPawnTitle.Controls.Add(Me.LinkLabel_Close)
        Me.Panel_BasicPawnTitle.Controls.Add(Me.Panel_FooterDarkControl)
        Me.Panel_BasicPawnTitle.Controls.Add(Me.ClassPictureBoxQuality1)
        Me.Panel_BasicPawnTitle.Controls.Add(Me.Label2)
        Me.Panel_BasicPawnTitle.Controls.Add(Me.Label1)
        Me.Panel_BasicPawnTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_BasicPawnTitle.Location = New System.Drawing.Point(0, 0)
        Me.Panel_BasicPawnTitle.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_BasicPawnTitle.Name = "Panel_BasicPawnTitle"
        Me.Panel_BasicPawnTitle.Size = New System.Drawing.Size(800, 64)
        Me.Panel_BasicPawnTitle.TabIndex = 0
        '
        'LinkLabel_Open
        '
        Me.LinkLabel_Open.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel_Open.AutoSize = True
        Me.LinkLabel_Open.Location = New System.Drawing.Point(668, 47)
        Me.LinkLabel_Open.Name = "LinkLabel_Open"
        Me.LinkLabel_Open.Size = New System.Drawing.Size(36, 13)
        Me.LinkLabel_Open.TabIndex = 6
        Me.LinkLabel_Open.TabStop = True
        Me.LinkLabel_Open.Text = "Open"
        '
        'LinkLabel_Close
        '
        Me.LinkLabel_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel_Close.AutoSize = True
        Me.LinkLabel_Close.Location = New System.Drawing.Point(710, 47)
        Me.LinkLabel_Close.Name = "LinkLabel_Close"
        Me.LinkLabel_Close.Size = New System.Drawing.Size(87, 13)
        Me.LinkLabel_Close.TabIndex = 5
        Me.LinkLabel_Close.TabStop = True
        Me.LinkLabel_Close.Text = "Close StartPage"
        '
        'Panel_FooterDarkControl
        '
        Me.Panel_FooterDarkControl.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel_FooterDarkControl.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_FooterDarkControl.Location = New System.Drawing.Point(0, 63)
        Me.Panel_FooterDarkControl.Name = "Panel_FooterDarkControl"
        Me.Panel_FooterDarkControl.Size = New System.Drawing.Size(800, 1)
        Me.Panel_FooterDarkControl.TabIndex = 3
        '
        'ClassPictureBoxQuality1
        '
        Me.ClassPictureBoxQuality1.Image = Global.BasicPawn.My.Resources.Resources.BasicPawn_NoText_PNGx64
        Me.ClassPictureBoxQuality1.Location = New System.Drawing.Point(3, 3)
        Me.ClassPictureBoxQuality1.m_HighQuality = True
        Me.ClassPictureBoxQuality1.Name = "ClassPictureBoxQuality1"
        Me.ClassPictureBoxQuality1.Size = New System.Drawing.Size(42, 42)
        Me.ClassPictureBoxQuality1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.ClassPictureBoxQuality1.TabIndex = 1
        Me.ClassPictureBoxQuality1.TabStop = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 48)
        Me.Label2.Margin = New System.Windows.Forms.Padding(3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Recent Projects"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(51, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(254, 42)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "BasicPawn - StartPage"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel_RecentFiles
        '
        Me.Panel_RecentFiles.AutoScroll = True
        Me.Panel_RecentFiles.Controls.Add(Me.Label_NoProjectsFound)
        Me.Panel_RecentFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_RecentFiles.Location = New System.Drawing.Point(0, 64)
        Me.Panel_RecentFiles.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_RecentFiles.Name = "Panel_RecentFiles"
        Me.Panel_RecentFiles.Size = New System.Drawing.Size(800, 364)
        Me.Panel_RecentFiles.TabIndex = 1
        '
        'Panel_FooterDarkControl2
        '
        Me.Panel_FooterDarkControl2.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel_FooterDarkControl2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_FooterDarkControl2.Location = New System.Drawing.Point(0, 514)
        Me.Panel_FooterDarkControl2.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_FooterDarkControl2.Name = "Panel_FooterDarkControl2"
        Me.Panel_FooterDarkControl2.Size = New System.Drawing.Size(800, 1)
        Me.Panel_FooterDarkControl2.TabIndex = 1
        '
        'Panel_FooterDarkControl3
        '
        Me.Panel_FooterDarkControl3.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel_FooterDarkControl3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_FooterDarkControl3.Location = New System.Drawing.Point(0, 85)
        Me.Panel_FooterDarkControl3.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_FooterDarkControl3.Name = "Panel_FooterDarkControl3"
        Me.Panel_FooterDarkControl3.Size = New System.Drawing.Size(800, 1)
        Me.Panel_FooterDarkControl3.TabIndex = 2
        '
        'Timer_DelayUpdate
        '
        Me.Timer_DelayUpdate.Interval = 1000
        '
        'Label_NoProjectsFound
        '
        Me.Label_NoProjectsFound.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_NoProjectsFound.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_NoProjectsFound.Location = New System.Drawing.Point(0, 0)
        Me.Label_NoProjectsFound.Name = "Label_NoProjectsFound"
        Me.Label_NoProjectsFound.Size = New System.Drawing.Size(800, 364)
        Me.Label_NoProjectsFound.TabIndex = 0
        Me.Label_NoProjectsFound.Text = "No recent projects found!"
        Me.Label_NoProjectsFound.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UCStartPage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackgroundImage = Global.BasicPawn.My.Resources.Resources.Bmp_Design
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UCStartPage"
        Me.Size = New System.Drawing.Size(800, 600)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.Panel_BasicPawnTitle.ResumeLayout(False)
        Me.Panel_BasicPawnTitle.PerformLayout()
        CType(Me.ClassPictureBoxQuality1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel_RecentFiles.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Panel_BasicPawnTitle As Panel
    Friend WithEvents Panel_RecentFiles As Panel
    Friend WithEvents ClassPictureBoxQuality1 As ClassPictureBoxQuality
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel_FooterDarkControl As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Panel_FooterDarkControl2 As Panel
    Friend WithEvents Panel_FooterDarkControl3 As Panel
    Friend WithEvents Timer_DelayUpdate As Timer
    Friend WithEvents LinkLabel_Close As LinkLabel
    Friend WithEvents LinkLabel_Open As LinkLabel
    Friend WithEvents Label_NoProjectsFound As Label
End Class
