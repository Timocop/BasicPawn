<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormUpdate
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If (disposing) Then
                If (g_mUpdateThread IsNot Nothing AndAlso g_mUpdateThread.IsAlive) Then
                    g_mUpdateThread.Abort()
                    g_mUpdateThread.Join()
                    g_mUpdateThread = Nothing
                End If
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
        Me.ClassPictureBoxQuality2 = New BasicPawn.ClassPictureBoxQuality()
        Me.ClassPictureBoxQuality1 = New BasicPawn.ClassPictureBoxQuality()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel_FooterControl.SuspendLayout()
        CType(Me.ClassPictureBoxQuality2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ClassPictureBoxQuality1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Update
        '
        Me.Button_Update.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Update.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Update.Location = New System.Drawing.Point(152, 13)
        Me.Button_Update.Name = "Button_Update"
        Me.Button_Update.Size = New System.Drawing.Size(128, 23)
        Me.Button_Update.TabIndex = 0
        Me.Button_Update.Text = "Update and Exit"
        Me.Button_Update.UseVisualStyleBackColor = True
        '
        'Panel_FooterControl
        '
        Me.Panel_FooterControl.BackColor = System.Drawing.SystemColors.Control
        Me.Panel_FooterControl.Controls.Add(Me.LinkLabel_ManualUpdate)
        Me.Panel_FooterControl.Controls.Add(Me.Panel_FooterDarkControl)
        Me.Panel_FooterControl.Controls.Add(Me.Button_Close)
        Me.Panel_FooterControl.Controls.Add(Me.Button_Update)
        Me.Panel_FooterControl.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_FooterControl.Location = New System.Drawing.Point(0, 163)
        Me.Panel_FooterControl.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_FooterControl.Name = "Panel_FooterControl"
        Me.Panel_FooterControl.Size = New System.Drawing.Size(384, 48)
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
        Me.Panel_FooterDarkControl.Size = New System.Drawing.Size(384, 1)
        Me.Panel_FooterDarkControl.TabIndex = 1
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.Location = New System.Drawing.Point(286, 13)
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
        Me.Label_Status.Location = New System.Drawing.Point(82, 51)
        Me.Label_Status.Name = "Label_Status"
        Me.Label_Status.Size = New System.Drawing.Size(290, 47)
        Me.Label_Status.TabIndex = 6
        Me.Label_Status.Text = "Status: Unknown"
        Me.Label_Status.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.Label_Status.Visible = False
        '
        'ProgressBar_Status
        '
        Me.ProgressBar_Status.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar_Status.Location = New System.Drawing.Point(85, 101)
        Me.ProgressBar_Status.Name = "ProgressBar_Status"
        Me.ProgressBar_Status.Size = New System.Drawing.Size(287, 10)
        Me.ProgressBar_Status.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.ProgressBar_Status.TabIndex = 5
        Me.ProgressBar_Status.Visible = False
        '
        'ClassPictureBoxQuality2
        '
        Me.ClassPictureBoxQuality2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ClassPictureBoxQuality2.Image = Global.BasicPawn.My.Resources.Resources.Bmp_Warn
        Me.ClassPictureBoxQuality2.Location = New System.Drawing.Point(52, 128)
        Me.ClassPictureBoxQuality2.m_HighQuality = True
        Me.ClassPictureBoxQuality2.Name = "ClassPictureBoxQuality2"
        Me.ClassPictureBoxQuality2.Size = New System.Drawing.Size(24, 24)
        Me.ClassPictureBoxQuality2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.ClassPictureBoxQuality2.TabIndex = 3
        Me.ClassPictureBoxQuality2.TabStop = False
        '
        'ClassPictureBoxQuality1
        '
        Me.ClassPictureBoxQuality1.Image = Global.BasicPawn.My.Resources.Resources.Bmp_Network
        Me.ClassPictureBoxQuality1.Location = New System.Drawing.Point(12, 12)
        Me.ClassPictureBoxQuality1.m_HighQuality = True
        Me.ClassPictureBoxQuality1.Name = "ClassPictureBoxQuality1"
        Me.ClassPictureBoxQuality1.Size = New System.Drawing.Size(64, 64)
        Me.ClassPictureBoxQuality1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.ClassPictureBoxQuality1.TabIndex = 2
        Me.ClassPictureBoxQuality1.TabStop = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.Location = New System.Drawing.Point(82, 128)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(290, 32)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "All BasicPawn instances will be closed and all your unsaved work will be lost!"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(82, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(290, 39)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "A new BasicPawn update is available!" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Do you want to update now?"
        '
        'FormUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(384, 211)
        Me.Controls.Add(Me.Label_Status)
        Me.Controls.Add(Me.Panel_FooterControl)
        Me.Controls.Add(Me.ProgressBar_Status)
        Me.Controls.Add(Me.ClassPictureBoxQuality2)
        Me.Controls.Add(Me.ClassPictureBoxQuality1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormUpdate"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BasicPawn Update"
        Me.Panel_FooterControl.ResumeLayout(False)
        Me.Panel_FooterControl.PerformLayout()
        CType(Me.ClassPictureBoxQuality2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ClassPictureBoxQuality1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Button_Update As Button
    Friend WithEvents Panel_FooterControl As Panel
    Friend WithEvents Panel_FooterDarkControl As Panel
    Friend WithEvents Button_Close As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents ClassPictureBoxQuality1 As ClassPictureBoxQuality
    Friend WithEvents ClassPictureBoxQuality2 As ClassPictureBoxQuality
    Friend WithEvents ProgressBar_Status As ProgressBar
    Friend WithEvents Label_Status As Label
    Friend WithEvents LinkLabel_ManualUpdate As LinkLabel
End Class
