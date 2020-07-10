<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormOnlineTranslator
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormOnlineTranslator))
        Me.Panel_Footer = New System.Windows.Forms.Panel()
        Me.LinkLabel_Limits = New System.Windows.Forms.LinkLabel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button_Translate = New System.Windows.Forms.Button()
        Me.Panel_FooterDark = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBox_TranslateFrom = New System.Windows.Forms.ComboBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.TextBox_TranslateFrom = New System.Windows.Forms.TextBox()
        Me.TextBox_TranslateTo = New System.Windows.Forms.TextBox()
        Me.ComboBox_TranslateTo = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel_Footer.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel_Footer
        '
        Me.Panel_Footer.BackColor = System.Drawing.SystemColors.Control
        Me.Panel_Footer.Controls.Add(Me.LinkLabel_Limits)
        Me.Panel_Footer.Controls.Add(Me.Label1)
        Me.Panel_Footer.Controls.Add(Me.Button_Translate)
        Me.Panel_Footer.Controls.Add(Me.Panel_FooterDark)
        Me.Panel_Footer.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_Footer.Location = New System.Drawing.Point(0, 393)
        Me.Panel_Footer.Name = "Panel_Footer"
        Me.Panel_Footer.Size = New System.Drawing.Size(624, 48)
        Me.Panel_Footer.TabIndex = 0
        '
        'LinkLabel_Limits
        '
        Me.LinkLabel_Limits.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel_Limits.AutoSize = True
        Me.LinkLabel_Limits.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel_Limits.Location = New System.Drawing.Point(447, 18)
        Me.LinkLabel_Limits.Name = "LinkLabel_Limits"
        Me.LinkLabel_Limits.Size = New System.Drawing.Size(84, 13)
        Me.LinkLabel_Limits.TabIndex = 2
        Me.LinkLabel_Limits.TabStop = True
        Me.LinkLabel_Limits.Text = "See Limitations"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(162, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Powered by Google Translator"
        '
        'Button_Translate
        '
        Me.Button_Translate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Translate.Location = New System.Drawing.Point(537, 13)
        Me.Button_Translate.Name = "Button_Translate"
        Me.Button_Translate.Size = New System.Drawing.Size(75, 23)
        Me.Button_Translate.TabIndex = 1
        Me.Button_Translate.Text = "Translate"
        Me.Button_Translate.UseVisualStyleBackColor = True
        '
        'Panel_FooterDark
        '
        Me.Panel_FooterDark.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel_FooterDark.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_FooterDark.Location = New System.Drawing.Point(0, 0)
        Me.Panel_FooterDark.Name = "Panel_FooterDark"
        Me.Panel_FooterDark.Size = New System.Drawing.Size(624, 1)
        Me.Panel_FooterDark.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 16)
        Me.Label2.Margin = New System.Windows.Forms.Padding(16, 16, 3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Translate from"
        '
        'ComboBox_TranslateFrom
        '
        Me.ComboBox_TranslateFrom.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_TranslateFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_TranslateFrom.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox_TranslateFrom.FormattingEnabled = True
        Me.ComboBox_TranslateFrom.Location = New System.Drawing.Point(102, 13)
        Me.ComboBox_TranslateFrom.Margin = New System.Windows.Forms.Padding(3, 3, 16, 3)
        Me.ComboBox_TranslateFrom.Name = "ComboBox_TranslateFrom"
        Me.ComboBox_TranslateFrom.Size = New System.Drawing.Size(506, 21)
        Me.ComboBox_TranslateFrom.TabIndex = 2
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.TextBox_TranslateFrom)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ComboBox_TranslateFrom)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TextBox_TranslateTo)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ComboBox_TranslateTo)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label3)
        Me.SplitContainer1.Size = New System.Drawing.Size(624, 393)
        Me.SplitContainer1.SplitterDistance = 190
        Me.SplitContainer1.TabIndex = 3
        '
        'TextBox_TranslateFrom
        '
        Me.TextBox_TranslateFrom.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_TranslateFrom.Location = New System.Drawing.Point(16, 46)
        Me.TextBox_TranslateFrom.Margin = New System.Windows.Forms.Padding(16, 9, 16, 16)
        Me.TextBox_TranslateFrom.MaxLength = 1024
        Me.TextBox_TranslateFrom.Multiline = True
        Me.TextBox_TranslateFrom.Name = "TextBox_TranslateFrom"
        Me.TextBox_TranslateFrom.Size = New System.Drawing.Size(592, 128)
        Me.TextBox_TranslateFrom.TabIndex = 3
        '
        'TextBox_TranslateTo
        '
        Me.TextBox_TranslateTo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_TranslateTo.Location = New System.Drawing.Point(16, 46)
        Me.TextBox_TranslateTo.Margin = New System.Windows.Forms.Padding(16, 9, 16, 16)
        Me.TextBox_TranslateTo.Multiline = True
        Me.TextBox_TranslateTo.Name = "TextBox_TranslateTo"
        Me.TextBox_TranslateTo.Size = New System.Drawing.Size(592, 135)
        Me.TextBox_TranslateTo.TabIndex = 6
        '
        'ComboBox_TranslateTo
        '
        Me.ComboBox_TranslateTo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_TranslateTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_TranslateTo.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox_TranslateTo.FormattingEnabled = True
        Me.ComboBox_TranslateTo.Location = New System.Drawing.Point(102, 13)
        Me.ComboBox_TranslateTo.Margin = New System.Windows.Forms.Padding(3, 3, 16, 3)
        Me.ComboBox_TranslateTo.Name = "ComboBox_TranslateTo"
        Me.ComboBox_TranslateTo.Size = New System.Drawing.Size(506, 21)
        Me.ComboBox_TranslateTo.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(16, 16)
        Me.Label3.Margin = New System.Windows.Forms.Padding(16, 16, 3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Translate to"
        '
        'FormOnlineTranslator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(624, 441)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Panel_Footer)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(640, 480)
        Me.Name = "FormOnlineTranslator"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Online Translator"
        Me.Panel_Footer.ResumeLayout(False)
        Me.Panel_Footer.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel_Footer As Panel
    Friend WithEvents Panel_FooterDark As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents Button_Translate As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents ComboBox_TranslateFrom As ComboBox
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents TextBox_TranslateFrom As TextBox
    Friend WithEvents TextBox_TranslateTo As TextBox
    Friend WithEvents ComboBox_TranslateTo As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents LinkLabel_Limits As LinkLabel
End Class
