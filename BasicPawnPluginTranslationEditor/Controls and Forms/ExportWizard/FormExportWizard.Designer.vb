<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormExportWizard
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
        Me.Panel_FooterControl = New System.Windows.Forms.Panel()
        Me.Button_WizBack = New System.Windows.Forms.Button()
        Me.Button_WizNext = New System.Windows.Forms.Button()
        Me.Button_WizCancel = New System.Windows.Forms.Button()
        Me.Panel_FooterDarkControl = New System.Windows.Forms.Panel()
        Me.Panel_Pages = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel_TopDarkControl = New System.Windows.Forms.Panel()
        Me.Label_WizDesc = New System.Windows.Forms.Label()
        Me.Label_WizTitle = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel_FooterControl.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel_FooterControl
        '
        Me.Panel_FooterControl.BackColor = System.Drawing.SystemColors.Control
        Me.Panel_FooterControl.Controls.Add(Me.Button_WizBack)
        Me.Panel_FooterControl.Controls.Add(Me.Button_WizNext)
        Me.Panel_FooterControl.Controls.Add(Me.Button_WizCancel)
        Me.Panel_FooterControl.Controls.Add(Me.Panel_FooterDarkControl)
        Me.Panel_FooterControl.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_FooterControl.Location = New System.Drawing.Point(0, 393)
        Me.Panel_FooterControl.Name = "Panel_FooterControl"
        Me.Panel_FooterControl.Size = New System.Drawing.Size(624, 48)
        Me.Panel_FooterControl.TabIndex = 0
        '
        'Button_WizBack
        '
        Me.Button_WizBack.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_WizBack.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_WizBack.Location = New System.Drawing.Point(375, 13)
        Me.Button_WizBack.Name = "Button_WizBack"
        Me.Button_WizBack.Size = New System.Drawing.Size(75, 23)
        Me.Button_WizBack.TabIndex = 3
        Me.Button_WizBack.Text = "< Back"
        Me.Button_WizBack.UseVisualStyleBackColor = True
        '
        'Button_WizNext
        '
        Me.Button_WizNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_WizNext.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_WizNext.Location = New System.Drawing.Point(456, 13)
        Me.Button_WizNext.Name = "Button_WizNext"
        Me.Button_WizNext.Size = New System.Drawing.Size(75, 23)
        Me.Button_WizNext.TabIndex = 2
        Me.Button_WizNext.Text = "Next >"
        Me.Button_WizNext.UseVisualStyleBackColor = True
        '
        'Button_WizCancel
        '
        Me.Button_WizCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_WizCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_WizCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_WizCancel.Location = New System.Drawing.Point(537, 13)
        Me.Button_WizCancel.Name = "Button_WizCancel"
        Me.Button_WizCancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_WizCancel.TabIndex = 1
        Me.Button_WizCancel.Text = "Cancel"
        Me.Button_WizCancel.UseVisualStyleBackColor = True
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
        'Panel_Pages
        '
        Me.Panel_Pages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_Pages.Location = New System.Drawing.Point(0, 64)
        Me.Panel_Pages.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_Pages.Name = "Panel_Pages"
        Me.Panel_Pages.Size = New System.Drawing.Size(624, 329)
        Me.Panel_Pages.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.PictureBox1)
        Me.Panel3.Controls.Add(Me.Panel_TopDarkControl)
        Me.Panel3.Controls.Add(Me.Label_WizDesc)
        Me.Panel3.Controls.Add(Me.Label_WizTitle)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(624, 64)
        Me.Panel3.TabIndex = 0
        '
        'Panel_TopDarkControl
        '
        Me.Panel_TopDarkControl.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel_TopDarkControl.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_TopDarkControl.Location = New System.Drawing.Point(0, 63)
        Me.Panel_TopDarkControl.Name = "Panel_TopDarkControl"
        Me.Panel_TopDarkControl.Size = New System.Drawing.Size(624, 1)
        Me.Panel_TopDarkControl.TabIndex = 2
        '
        'Label_WizDesc
        '
        Me.Label_WizDesc.AutoSize = True
        Me.Label_WizDesc.Location = New System.Drawing.Point(64, 32)
        Me.Label_WizDesc.Margin = New System.Windows.Forms.Padding(64, 3, 3, 0)
        Me.Label_WizDesc.Name = "Label_WizDesc"
        Me.Label_WizDesc.Size = New System.Drawing.Size(105, 13)
        Me.Label_WizDesc.TabIndex = 1
        Me.Label_WizDesc.Text = "Wizard Description"
        '
        'Label_WizTitle
        '
        Me.Label_WizTitle.AutoSize = True
        Me.Label_WizTitle.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_WizTitle.Location = New System.Drawing.Point(32, 16)
        Me.Label_WizTitle.Margin = New System.Windows.Forms.Padding(32, 16, 3, 0)
        Me.Label_WizTitle.Name = "Label_WizTitle"
        Me.Label_WizTitle.Size = New System.Drawing.Size(68, 13)
        Me.Label_WizTitle.TabIndex = 0
        Me.Label_WizTitle.Text = "Wizard Title"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = Global.BasicPawnPluginTranslationEditor.My.Resources.Resources.accessibilitycpl_325_48x48_32
        Me.PictureBox1.Location = New System.Drawing.Point(564, 9)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'FormExportWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(624, 441)
        Me.Controls.Add(Me.Panel_Pages)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel_FooterControl)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(640, 480)
        Me.Name = "FormExportWizard"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Export Translation Wizard"
        Me.Panel_FooterControl.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel_FooterControl As Windows.Forms.Panel
    Friend WithEvents Button_WizBack As Windows.Forms.Button
    Friend WithEvents Button_WizNext As Windows.Forms.Button
    Friend WithEvents Button_WizCancel As Windows.Forms.Button
    Friend WithEvents Panel_FooterDarkControl As Windows.Forms.Panel
    Friend WithEvents Panel_Pages As Windows.Forms.Panel
    Friend WithEvents Panel3 As Windows.Forms.Panel
    Friend WithEvents Label_WizDesc As Windows.Forms.Label
    Friend WithEvents Label_WizTitle As Windows.Forms.Label
    Friend WithEvents Panel_TopDarkControl As Windows.Forms.Panel
    Friend WithEvents PictureBox1 As PictureBox
End Class
