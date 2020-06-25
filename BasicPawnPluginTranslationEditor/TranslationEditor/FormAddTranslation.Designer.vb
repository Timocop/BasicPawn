<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormAddTranslation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormAddTranslation))
        Me.Button_Apply = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox_TranslationName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBox_Language = New System.Windows.Forms.ComboBox()
        Me.LinkLabel_LangCustom = New System.Windows.Forms.LinkLabel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox_Format = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox_Text = New System.Windows.Forms.TextBox()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Apply
        '
        Me.Button_Apply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Apply.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button_Apply.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Apply.Location = New System.Drawing.Point(366, 13)
        Me.Button_Apply.Name = "Button_Apply"
        Me.Button_Apply.Size = New System.Drawing.Size(86, 23)
        Me.Button_Apply.TabIndex = 0
        Me.Button_Apply.Text = "Apply"
        Me.Button_Apply.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.Controls.Add(Me.Button_Cancel)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.Button_Apply)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 273)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(464, 48)
        Me.Panel1.TabIndex = 1
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Cancel.Location = New System.Drawing.Point(274, 13)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(86, 23)
        Me.Button_Cancel.TabIndex = 2
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(464, 1)
        Me.Panel2.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(36, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Name"
        '
        'TextBox_TranslationName
        '
        Me.TextBox_TranslationName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_TranslationName.BackColor = System.Drawing.Color.White
        Me.TextBox_TranslationName.Location = New System.Drawing.Point(12, 25)
        Me.TextBox_TranslationName.Name = "TextBox_TranslationName"
        Me.TextBox_TranslationName.ReadOnly = True
        Me.TextBox_TranslationName.Size = New System.Drawing.Size(440, 22)
        Me.TextBox_TranslationName.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 59)
        Me.Label2.Margin = New System.Windows.Forms.Padding(3, 9, 3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Language"
        '
        'ComboBox_Language
        '
        Me.ComboBox_Language.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_Language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Language.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBox_Language.FormattingEnabled = True
        Me.ComboBox_Language.Location = New System.Drawing.Point(12, 75)
        Me.ComboBox_Language.Name = "ComboBox_Language"
        Me.ComboBox_Language.Size = New System.Drawing.Size(440, 21)
        Me.ComboBox_Language.TabIndex = 5
        '
        'LinkLabel_LangCustom
        '
        Me.LinkLabel_LangCustom.AutoSize = True
        Me.LinkLabel_LangCustom.Location = New System.Drawing.Point(12, 99)
        Me.LinkLabel_LangCustom.Name = "LinkLabel_LangCustom"
        Me.LinkLabel_LangCustom.Size = New System.Drawing.Size(98, 13)
        Me.LinkLabel_LangCustom.TabIndex = 6
        Me.LinkLabel_LangCustom.TabStop = True
        Me.LinkLabel_LangCustom.Text = "Custom language"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 121)
        Me.Label3.Margin = New System.Windows.Forms.Padding(3, 9, 3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Format"
        '
        'TextBox_Format
        '
        Me.TextBox_Format.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_Format.BackColor = System.Drawing.Color.White
        Me.TextBox_Format.Location = New System.Drawing.Point(12, 137)
        Me.TextBox_Format.Name = "TextBox_Format"
        Me.TextBox_Format.Size = New System.Drawing.Size(440, 22)
        Me.TextBox_Format.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 171)
        Me.Label4.Margin = New System.Windows.Forms.Padding(3, 9, 3, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(27, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Text"
        '
        'TextBox_Text
        '
        Me.TextBox_Text.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_Text.BackColor = System.Drawing.Color.White
        Me.TextBox_Text.Location = New System.Drawing.Point(12, 187)
        Me.TextBox_Text.Margin = New System.Windows.Forms.Padding(3, 3, 3, 9)
        Me.TextBox_Text.Multiline = True
        Me.TextBox_Text.Name = "TextBox_Text"
        Me.TextBox_Text.Size = New System.Drawing.Size(440, 74)
        Me.TextBox_Text.TabIndex = 10
        '
        'FormAddTranslation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(464, 321)
        Me.Controls.Add(Me.TextBox_Text)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TextBox_Format)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.LinkLabel_LangCustom)
        Me.Controls.Add(Me.ComboBox_Language)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBox_TranslationName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(480, 360)
        Me.Name = "FormAddTranslation"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Translation"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button_Apply As Windows.Forms.Button
    Friend WithEvents Panel1 As Windows.Forms.Panel
    Friend WithEvents Panel2 As Windows.Forms.Panel
    Friend WithEvents Button_Cancel As Windows.Forms.Button
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents TextBox_TranslationName As Windows.Forms.TextBox
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents ComboBox_Language As Windows.Forms.ComboBox
    Friend WithEvents LinkLabel_LangCustom As Windows.Forms.LinkLabel
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents TextBox_Format As Windows.Forms.TextBox
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents TextBox_Text As Windows.Forms.TextBox
End Class
