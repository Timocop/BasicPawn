<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormUpdate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormUpdate))
        Me.Button_Update = New System.Windows.Forms.Button()
        Me.Panel_FooterControl = New System.Windows.Forms.Panel()
        Me.LinkLabel_ManualUpdate = New System.Windows.Forms.LinkLabel()
        Me.Panel_FooterDarkControl = New System.Windows.Forms.Panel()
        Me.Button_Close = New System.Windows.Forms.Button()
        Me.Label_Status = New System.Windows.Forms.Label()
        Me.ProgressBar_Status = New System.Windows.Forms.ProgressBar()
        Me.Label_WarnText = New System.Windows.Forms.Label()
        Me.Label_StatusTitle = New System.Windows.Forms.Label()
        Me.ClassPictureBoxQuality_WarnIcon = New BasicPawn.ClassPictureBoxQuality()
        Me.ClassPictureBoxQuality_TitleIcon = New BasicPawn.ClassPictureBoxQuality()
        Me.Panel_FooterControl.SuspendLayout()
        CType(Me.ClassPictureBoxQuality_WarnIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ClassPictureBoxQuality_TitleIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Update
        '
        Me.Button_Update.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Update.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Update.Location = New System.Drawing.Point(172, 13)
        Me.Button_Update.Name = "Button_Update"
        Me.Button_Update.Size = New System.Drawing.Size(128, 23)
        Me.Button_Update.TabIndex = 0
        Me.Button_Update.Text = "Update and Exit"
        Me.Button_Update.UseVisualStyleBackColor = True
        Me.Button_Update.Visible = False
        '
        'Panel_FooterControl
        '
        Me.Panel_FooterControl.BackColor = System.Drawing.SystemColors.Control
        Me.Panel_FooterControl.Controls.Add(Me.LinkLabel_ManualUpdate)
        Me.Panel_FooterControl.Controls.Add(Me.Panel_FooterDarkControl)
        Me.Panel_FooterControl.Controls.Add(Me.Button_Close)
        Me.Panel_FooterControl.Controls.Add(Me.Button_Update)
        Me.Panel_FooterControl.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_FooterControl.Location = New System.Drawing.Point(0, 193)
        Me.Panel_FooterControl.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_FooterControl.Name = "Panel_FooterControl"
        Me.Panel_FooterControl.Size = New System.Drawing.Size(404, 48)
        Me.Panel_FooterControl.TabIndex = 0
        '
        'LinkLabel_ManualUpdate
        '
        Me.LinkLabel_ManualUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel_ManualUpdate.AutoSize = True
        Me.LinkLabel_ManualUpdate.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel_ManualUpdate.Location = New System.Drawing.Point(12, 18)
        Me.LinkLabel_ManualUpdate.Name = "LinkLabel_ManualUpdate"
        Me.LinkLabel_ManualUpdate.Size = New System.Drawing.Size(83, 13)
        Me.LinkLabel_ManualUpdate.TabIndex = 3
        Me.LinkLabel_ManualUpdate.TabStop = True
        Me.LinkLabel_ManualUpdate.Text = "Show Releases"
        '
        'Panel_FooterDarkControl
        '
        Me.Panel_FooterDarkControl.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel_FooterDarkControl.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_FooterDarkControl.Location = New System.Drawing.Point(0, 0)
        Me.Panel_FooterDarkControl.Name = "Panel_FooterDarkControl"
        Me.Panel_FooterDarkControl.Size = New System.Drawing.Size(404, 1)
        Me.Panel_FooterDarkControl.TabIndex = 1
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.Location = New System.Drawing.Point(306, 13)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(86, 23)
        Me.Button_Close.TabIndex = 2
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'Label_Status
        '
        Me.Label_Status.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_Status.Location = New System.Drawing.Point(82, 76)
        Me.Label_Status.Name = "Label_Status"
        Me.Label_Status.Size = New System.Drawing.Size(310, 40)
        Me.Label_Status.TabIndex = 6
        Me.Label_Status.Text = "Status: Unknown"
        Me.Label_Status.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.Label_Status.Visible = False
        '
        'ProgressBar_Status
        '
        Me.ProgressBar_Status.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar_Status.Location = New System.Drawing.Point(85, 119)
        Me.ProgressBar_Status.Name = "ProgressBar_Status"
        Me.ProgressBar_Status.Size = New System.Drawing.Size(307, 10)
        Me.ProgressBar_Status.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.ProgressBar_Status.TabIndex = 5
        Me.ProgressBar_Status.Visible = False
        '
        'Label_WarnText
        '
        Me.Label_WarnText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_WarnText.Location = New System.Drawing.Point(82, 158)
        Me.Label_WarnText.Name = "Label_WarnText"
        Me.Label_WarnText.Size = New System.Drawing.Size(310, 32)
        Me.Label_WarnText.TabIndex = 1
        Me.Label_WarnText.Text = "All BasicPawn instances will be closed and all your unsaved work will be lost!"
        Me.Label_WarnText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label_WarnText.Visible = False
        '
        'Label_StatusTitle
        '
        Me.Label_StatusTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_StatusTitle.Location = New System.Drawing.Point(82, 12)
        Me.Label_StatusTitle.Name = "Label_StatusTitle"
        Me.Label_StatusTitle.Size = New System.Drawing.Size(310, 64)
        Me.Label_StatusTitle.TabIndex = 0
        Me.Label_StatusTitle.Text = "Checking..."
        '
        'ClassPictureBoxQuality_WarnIcon
        '
        Me.ClassPictureBoxQuality_WarnIcon.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ClassPictureBoxQuality_WarnIcon.Image = Global.BasicPawn.My.Resources.Resources.Bmp_Warn
        Me.ClassPictureBoxQuality_WarnIcon.Location = New System.Drawing.Point(52, 158)
        Me.ClassPictureBoxQuality_WarnIcon.m_HighQuality = True
        Me.ClassPictureBoxQuality_WarnIcon.Name = "ClassPictureBoxQuality_WarnIcon"
        Me.ClassPictureBoxQuality_WarnIcon.Size = New System.Drawing.Size(24, 24)
        Me.ClassPictureBoxQuality_WarnIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.ClassPictureBoxQuality_WarnIcon.TabIndex = 3
        Me.ClassPictureBoxQuality_WarnIcon.TabStop = False
        Me.ClassPictureBoxQuality_WarnIcon.Visible = False
        '
        'ClassPictureBoxQuality_TitleIcon
        '
        Me.ClassPictureBoxQuality_TitleIcon.Image = Global.BasicPawn.My.Resources.Resources.Bmp_Network
        Me.ClassPictureBoxQuality_TitleIcon.Location = New System.Drawing.Point(12, 12)
        Me.ClassPictureBoxQuality_TitleIcon.m_HighQuality = True
        Me.ClassPictureBoxQuality_TitleIcon.Name = "ClassPictureBoxQuality_TitleIcon"
        Me.ClassPictureBoxQuality_TitleIcon.Size = New System.Drawing.Size(64, 64)
        Me.ClassPictureBoxQuality_TitleIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.ClassPictureBoxQuality_TitleIcon.TabIndex = 2
        Me.ClassPictureBoxQuality_TitleIcon.TabStop = False
        '
        'FormUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(404, 241)
        Me.Controls.Add(Me.Label_Status)
        Me.Controls.Add(Me.Panel_FooterControl)
        Me.Controls.Add(Me.ProgressBar_Status)
        Me.Controls.Add(Me.ClassPictureBoxQuality_WarnIcon)
        Me.Controls.Add(Me.ClassPictureBoxQuality_TitleIcon)
        Me.Controls.Add(Me.Label_StatusTitle)
        Me.Controls.Add(Me.Label_WarnText)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormUpdate"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BasicPawn Update"
        Me.Panel_FooterControl.ResumeLayout(False)
        Me.Panel_FooterControl.PerformLayout()
        CType(Me.ClassPictureBoxQuality_WarnIcon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ClassPictureBoxQuality_TitleIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Button_Update As Button
    Friend WithEvents Panel_FooterControl As Panel
    Friend WithEvents Panel_FooterDarkControl As Panel
    Friend WithEvents Button_Close As Button
    Friend WithEvents Label_WarnText As Label
    Friend WithEvents Label_StatusTitle As Label
    Friend WithEvents ClassPictureBoxQuality_TitleIcon As ClassPictureBoxQuality
    Friend WithEvents ClassPictureBoxQuality_WarnIcon As ClassPictureBoxQuality
    Friend WithEvents ProgressBar_Status As ProgressBar
    Friend WithEvents Label_Status As Label
    Friend WithEvents LinkLabel_ManualUpdate As LinkLabel
End Class
