<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormDebuggerAssertSetAction
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormDebuggerAssertSetAction))
        Me.Panel_FooterControl = New System.Windows.Forms.Panel()
        Me.Button_Ignore = New System.Windows.Forms.Button()
        Me.Button_Error = New System.Windows.Forms.Button()
        Me.Button_Fail = New System.Windows.Forms.Button()
        Me.Panel_FooterDarkControl = New System.Windows.Forms.Panel()
        Me.ClassPictureBoxQuality1 = New BasicPawn.ClassPictureBoxQuality()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel_FooterControl.SuspendLayout()
        CType(Me.ClassPictureBoxQuality1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel_FooterControl
        '
        Me.Panel_FooterControl.BackColor = System.Drawing.SystemColors.Control
        Me.Panel_FooterControl.Controls.Add(Me.Button_Ignore)
        Me.Panel_FooterControl.Controls.Add(Me.Button_Error)
        Me.Panel_FooterControl.Controls.Add(Me.Button_Fail)
        Me.Panel_FooterControl.Controls.Add(Me.Panel_FooterDarkControl)
        Me.Panel_FooterControl.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_FooterControl.Location = New System.Drawing.Point(0, 113)
        Me.Panel_FooterControl.Name = "Panel_FooterControl"
        Me.Panel_FooterControl.Size = New System.Drawing.Size(364, 48)
        Me.Panel_FooterControl.TabIndex = 0
        '
        'Button_Ignore
        '
        Me.Button_Ignore.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Ignore.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Ignore.Location = New System.Drawing.Point(82, 13)
        Me.Button_Ignore.Name = "Button_Ignore"
        Me.Button_Ignore.Size = New System.Drawing.Size(86, 23)
        Me.Button_Ignore.TabIndex = 3
        Me.Button_Ignore.Text = "Ignore"
        Me.ToolTip1.SetToolTip(Me.Button_Ignore, "Ignores assert and continues current callback.")
        Me.Button_Ignore.UseVisualStyleBackColor = True
        '
        'Button_Error
        '
        Me.Button_Error.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Error.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Error.Location = New System.Drawing.Point(174, 13)
        Me.Button_Error.Name = "Button_Error"
        Me.Button_Error.Size = New System.Drawing.Size(86, 23)
        Me.Button_Error.TabIndex = 2
        Me.Button_Error.Text = "Error"
        Me.ToolTip1.SetToolTip(Me.Button_Error, "Aborts the current callback.")
        Me.Button_Error.UseVisualStyleBackColor = True
        '
        'Button_Fail
        '
        Me.Button_Fail.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Fail.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Fail.Location = New System.Drawing.Point(266, 13)
        Me.Button_Fail.Name = "Button_Fail"
        Me.Button_Fail.Size = New System.Drawing.Size(86, 23)
        Me.Button_Fail.TabIndex = 1
        Me.Button_Fail.Text = "Fail"
        Me.ToolTip1.SetToolTip(Me.Button_Fail, "Causes the plugin to enter a failed state. The plugin will be paused until it is " &
        "unloaded or reloaded.")
        Me.Button_Fail.UseVisualStyleBackColor = True
        '
        'Panel_FooterDarkControl
        '
        Me.Panel_FooterDarkControl.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel_FooterDarkControl.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_FooterDarkControl.Location = New System.Drawing.Point(0, 0)
        Me.Panel_FooterDarkControl.Name = "Panel_FooterDarkControl"
        Me.Panel_FooterDarkControl.Size = New System.Drawing.Size(364, 1)
        Me.Panel_FooterDarkControl.TabIndex = 0
        '
        'ClassPictureBoxQuality1
        '
        Me.ClassPictureBoxQuality1.Image = Global.BasicPawn.My.Resources.Resources.user32_101_48x48_32
        Me.ClassPictureBoxQuality1.Location = New System.Drawing.Point(12, 12)
        Me.ClassPictureBoxQuality1.m_HighQuality = True
        Me.ClassPictureBoxQuality1.Name = "ClassPictureBoxQuality1"
        Me.ClassPictureBoxQuality1.Size = New System.Drawing.Size(48, 48)
        Me.ClassPictureBoxQuality1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.ClassPictureBoxQuality1.TabIndex = 1
        Me.ClassPictureBoxQuality1.TabStop = False
        '
        'Label1
        '
        Me.Label1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(66, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Padding = New System.Windows.Forms.Padding(3)
        Me.Label1.Size = New System.Drawing.Size(286, 48)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Select a action for the active assert:"
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 30000
        Me.ToolTip1.InitialDelay = 500
        Me.ToolTip1.ReshowDelay = 100
        '
        'FormDebuggerAssertSetAction
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(364, 161)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ClassPictureBoxQuality1)
        Me.Controls.Add(Me.Panel_FooterControl)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormDebuggerAssertSetAction"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Set Action..."
        Me.Panel_FooterControl.ResumeLayout(False)
        CType(Me.ClassPictureBoxQuality1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel_FooterControl As Panel
    Friend WithEvents Panel_FooterDarkControl As Panel
    Friend WithEvents ClassPictureBoxQuality1 As ClassPictureBoxQuality
    Friend WithEvents Button_Ignore As Button
    Friend WithEvents Button_Error As Button
    Friend WithEvents Button_Fail As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
