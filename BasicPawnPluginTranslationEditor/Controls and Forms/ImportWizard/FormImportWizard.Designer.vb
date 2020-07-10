<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormImportWizard
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
        Me.Panel_Footer = New System.Windows.Forms.Panel()
        Me.Button_Import = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.Panel_FooterDark = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RadioButton_FileSingle = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox_File = New System.Windows.Forms.TextBox()
        Me.RadioButton_FileMulti = New System.Windows.Forms.RadioButton()
        Me.Label_AdditionalFiles = New System.Windows.Forms.Label()
        Me.ListBox_AdditionalFiles = New System.Windows.Forms.ListBox()
        Me.LinkLabel_Help = New System.Windows.Forms.LinkLabel()
        Me.Panel_Footer.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel_Footer
        '
        Me.Panel_Footer.BackColor = System.Drawing.SystemColors.Control
        Me.Panel_Footer.Controls.Add(Me.Button_Import)
        Me.Panel_Footer.Controls.Add(Me.Button_Cancel)
        Me.Panel_Footer.Controls.Add(Me.Panel_FooterDark)
        Me.Panel_Footer.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_Footer.Location = New System.Drawing.Point(0, 393)
        Me.Panel_Footer.Name = "Panel_Footer"
        Me.Panel_Footer.Size = New System.Drawing.Size(624, 48)
        Me.Panel_Footer.TabIndex = 0
        '
        'Button_Import
        '
        Me.Button_Import.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Import.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button_Import.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Import.Location = New System.Drawing.Point(456, 13)
        Me.Button_Import.Name = "Button_Import"
        Me.Button_Import.Size = New System.Drawing.Size(75, 23)
        Me.Button_Import.TabIndex = 2
        Me.Button_Import.Text = "Import"
        Me.Button_Import.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Cancel.Location = New System.Drawing.Point(537, 13)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 1
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
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
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 25)
        Me.Label1.Margin = New System.Windows.Forms.Padding(16, 16, 3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "File to Import:"
        '
        'RadioButton_FileSingle
        '
        Me.RadioButton_FileSingle.AutoSize = True
        Me.RadioButton_FileSingle.Checked = True
        Me.RadioButton_FileSingle.Location = New System.Drawing.Point(41, 101)
        Me.RadioButton_FileSingle.Margin = New System.Windows.Forms.Padding(32, 6, 3, 3)
        Me.RadioButton_FileSingle.Name = "RadioButton_FileSingle"
        Me.RadioButton_FileSingle.Size = New System.Drawing.Size(148, 17)
        Me.RadioButton_FileSingle.TabIndex = 2
        Me.RadioButton_FileSingle.TabStop = True
        Me.RadioButton_FileSingle.Text = "Import selected file only"
        Me.RadioButton_FileSingle.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(25, 82)
        Me.Label2.Margin = New System.Windows.Forms.Padding(16, 16, 3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(286, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Choose a method you want to import translation files:"
        '
        'TextBox_File
        '
        Me.TextBox_File.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_File.BackColor = System.Drawing.Color.White
        Me.TextBox_File.Location = New System.Drawing.Point(25, 41)
        Me.TextBox_File.Margin = New System.Windows.Forms.Padding(16, 3, 16, 3)
        Me.TextBox_File.Name = "TextBox_File"
        Me.TextBox_File.ReadOnly = True
        Me.TextBox_File.Size = New System.Drawing.Size(574, 22)
        Me.TextBox_File.TabIndex = 4
        '
        'RadioButton_FileMulti
        '
        Me.RadioButton_FileMulti.AutoSize = True
        Me.RadioButton_FileMulti.Location = New System.Drawing.Point(41, 124)
        Me.RadioButton_FileMulti.Margin = New System.Windows.Forms.Padding(32, 3, 3, 3)
        Me.RadioButton_FileMulti.Name = "RadioButton_FileMulti"
        Me.RadioButton_FileMulti.Size = New System.Drawing.Size(285, 17)
        Me.RadioButton_FileMulti.TabIndex = 5
        Me.RadioButton_FileMulti.Text = "Import selected file and additional translation files"
        Me.RadioButton_FileMulti.UseVisualStyleBackColor = True
        '
        'Label_AdditionalFiles
        '
        Me.Label_AdditionalFiles.AutoSize = True
        Me.Label_AdditionalFiles.Location = New System.Drawing.Point(25, 160)
        Me.Label_AdditionalFiles.Margin = New System.Windows.Forms.Padding(16, 16, 3, 0)
        Me.Label_AdditionalFiles.Name = "Label_AdditionalFiles"
        Me.Label_AdditionalFiles.Size = New System.Drawing.Size(258, 13)
        Me.Label_AdditionalFiles.TabIndex = 6
        Me.Label_AdditionalFiles.Text = "Additional translation files that will be imported:"
        '
        'ListBox_AdditionalFiles
        '
        Me.ListBox_AdditionalFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox_AdditionalFiles.FormattingEnabled = True
        Me.ListBox_AdditionalFiles.HorizontalScrollbar = True
        Me.ListBox_AdditionalFiles.Location = New System.Drawing.Point(25, 176)
        Me.ListBox_AdditionalFiles.Margin = New System.Windows.Forms.Padding(16, 3, 16, 16)
        Me.ListBox_AdditionalFiles.Name = "ListBox_AdditionalFiles"
        Me.ListBox_AdditionalFiles.Size = New System.Drawing.Size(574, 199)
        Me.ListBox_AdditionalFiles.TabIndex = 7
        '
        'LinkLabel_Help
        '
        Me.LinkLabel_Help.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel_Help.AutoSize = True
        Me.LinkLabel_Help.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel_Help.Location = New System.Drawing.Point(474, 128)
        Me.LinkLabel_Help.Name = "LinkLabel_Help"
        Me.LinkLabel_Help.Size = New System.Drawing.Size(125, 13)
        Me.LinkLabel_Help.TabIndex = 8
        Me.LinkLabel_Help.TabStop = True
        Me.LinkLabel_Help.Text = "What should i choose?"
        '
        'FormImportWizard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(624, 441)
        Me.Controls.Add(Me.LinkLabel_Help)
        Me.Controls.Add(Me.ListBox_AdditionalFiles)
        Me.Controls.Add(Me.Label_AdditionalFiles)
        Me.Controls.Add(Me.RadioButton_FileMulti)
        Me.Controls.Add(Me.TextBox_File)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.RadioButton_FileSingle)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel_Footer)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormImportWizard"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import Wizard"
        Me.Panel_Footer.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel_Footer As Panel
    Friend WithEvents Button_Import As Button
    Friend WithEvents Button_Cancel As Button
    Friend WithEvents Panel_FooterDark As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents RadioButton_FileSingle As RadioButton
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox_File As TextBox
    Friend WithEvents RadioButton_FileMulti As RadioButton
    Friend WithEvents Label_AdditionalFiles As Label
    Friend WithEvents ListBox_AdditionalFiles As ListBox
    Friend WithEvents LinkLabel_Help As LinkLabel
End Class
