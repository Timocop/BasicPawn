<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormFilesMessageBox
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormFilesMessageBox))
        Me.Label_Message = New System.Windows.Forms.Label()
        Me.Panel_FooterControl = New System.Windows.Forms.Panel()
        Me.Button_Apply = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.Panel_FooterDarkControl = New System.Windows.Forms.Panel()
        Me.ListBox_Files = New System.Windows.Forms.ListBox()
        Me.Label_Question = New System.Windows.Forms.Label()
        Me.Panel_FooterControl.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label_Message
        '
        Me.Label_Message.AutoSize = True
        Me.Label_Message.Location = New System.Drawing.Point(25, 25)
        Me.Label_Message.Margin = New System.Windows.Forms.Padding(16, 16, 3, 0)
        Me.Label_Message.Name = "Label_Message"
        Me.Label_Message.Size = New System.Drawing.Size(52, 13)
        Me.Label_Message.TabIndex = 0
        Me.Label_Message.Text = "Message"
        '
        'Panel_FooterControl
        '
        Me.Panel_FooterControl.BackColor = System.Drawing.SystemColors.Control
        Me.Panel_FooterControl.Controls.Add(Me.Button_Apply)
        Me.Panel_FooterControl.Controls.Add(Me.Button_Cancel)
        Me.Panel_FooterControl.Controls.Add(Me.Panel_FooterDarkControl)
        Me.Panel_FooterControl.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_FooterControl.Location = New System.Drawing.Point(0, 232)
        Me.Panel_FooterControl.Name = "Panel_FooterControl"
        Me.Panel_FooterControl.Size = New System.Drawing.Size(471, 48)
        Me.Panel_FooterControl.TabIndex = 1
        '
        'Button_Apply
        '
        Me.Button_Apply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Apply.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button_Apply.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Apply.Location = New System.Drawing.Point(303, 13)
        Me.Button_Apply.Name = "Button_Apply"
        Me.Button_Apply.Size = New System.Drawing.Size(75, 23)
        Me.Button_Apply.TabIndex = 2
        Me.Button_Apply.Text = "OK"
        Me.Button_Apply.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Cancel.Location = New System.Drawing.Point(384, 13)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 1
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Panel_FooterDarkControl
        '
        Me.Panel_FooterDarkControl.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel_FooterDarkControl.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_FooterDarkControl.Location = New System.Drawing.Point(0, 0)
        Me.Panel_FooterDarkControl.Name = "Panel_FooterDarkControl"
        Me.Panel_FooterDarkControl.Size = New System.Drawing.Size(471, 1)
        Me.Panel_FooterDarkControl.TabIndex = 0
        '
        'ListBox_Files
        '
        Me.ListBox_Files.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox_Files.FormattingEnabled = True
        Me.ListBox_Files.HorizontalScrollbar = True
        Me.ListBox_Files.Location = New System.Drawing.Point(25, 54)
        Me.ListBox_Files.Margin = New System.Windows.Forms.Padding(16, 16, 16, 3)
        Me.ListBox_Files.Name = "ListBox_Files"
        Me.ListBox_Files.Size = New System.Drawing.Size(421, 121)
        Me.ListBox_Files.TabIndex = 2
        '
        'Label_Question
        '
        Me.Label_Question.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label_Question.AutoSize = True
        Me.Label_Question.Location = New System.Drawing.Point(25, 194)
        Me.Label_Question.Margin = New System.Windows.Forms.Padding(16, 16, 3, 0)
        Me.Label_Question.Name = "Label_Question"
        Me.Label_Question.Size = New System.Drawing.Size(59, 13)
        Me.Label_Question.TabIndex = 3
        Me.Label_Question.Text = "Question?"
        '
        'FormFilesMessageBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(471, 280)
        Me.Controls.Add(Me.Label_Question)
        Me.Controls.Add(Me.ListBox_Files)
        Me.Controls.Add(Me.Panel_FooterControl)
        Me.Controls.Add(Me.Label_Message)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormFilesMessageBox"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Title"
        Me.Panel_FooterControl.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label_Message As Label
    Friend WithEvents Panel_FooterControl As Panel
    Friend WithEvents Panel_FooterDarkControl As Panel
    Friend WithEvents ListBox_Files As ListBox
    Friend WithEvents Label_Question As Label
    Friend WithEvents Button_Apply As Button
    Friend WithEvents Button_Cancel As Button
End Class
