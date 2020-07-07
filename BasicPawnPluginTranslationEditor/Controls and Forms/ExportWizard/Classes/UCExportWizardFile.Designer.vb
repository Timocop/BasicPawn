<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCExportWizardFile
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox_File = New System.Windows.Forms.TextBox()
        Me.Button_Browse = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(64, 128)
        Me.Label1.Margin = New System.Windows.Forms.Padding(64, 128, 3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "File:"
        '
        'TextBox_File
        '
        Me.TextBox_File.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_File.BackColor = System.Drawing.Color.White
        Me.TextBox_File.Location = New System.Drawing.Point(64, 144)
        Me.TextBox_File.Margin = New System.Windows.Forms.Padding(64, 3, 3, 3)
        Me.TextBox_File.Name = "TextBox_File"
        Me.TextBox_File.ReadOnly = True
        Me.TextBox_File.Size = New System.Drawing.Size(431, 22)
        Me.TextBox_File.TabIndex = 1
        '
        'Button_Browse
        '
        Me.Button_Browse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Browse.Location = New System.Drawing.Point(501, 144)
        Me.Button_Browse.Margin = New System.Windows.Forms.Padding(3, 64, 64, 3)
        Me.Button_Browse.Name = "Button_Browse"
        Me.Button_Browse.Size = New System.Drawing.Size(75, 23)
        Me.Button_Browse.TabIndex = 6
        Me.Button_Browse.Text = "Browse..."
        Me.Button_Browse.UseVisualStyleBackColor = True
        '
        'UCExportWizardFileSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.Button_Browse)
        Me.Controls.Add(Me.TextBox_File)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UCExportWizardFileSettings"
        Me.Size = New System.Drawing.Size(640, 480)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents TextBox_File As Windows.Forms.TextBox
    Friend WithEvents Button_Browse As Windows.Forms.Button
End Class
