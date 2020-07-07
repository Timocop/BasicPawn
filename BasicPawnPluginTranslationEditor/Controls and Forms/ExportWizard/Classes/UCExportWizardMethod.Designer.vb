<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCExportWizardMethod
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
        Me.ListBox_AdditionalFiles = New System.Windows.Forms.ListBox()
        Me.RadioButton_StoreSingleFile = New System.Windows.Forms.RadioButton()
        Me.RadioButton_StoreMultiFiles = New System.Windows.Forms.RadioButton()
        Me.Label_AdditionalFiles = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ListBox_AdditionalFiles
        '
        Me.ListBox_AdditionalFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox_AdditionalFiles.FormattingEnabled = True
        Me.ListBox_AdditionalFiles.HorizontalScrollbar = True
        Me.ListBox_AdditionalFiles.Location = New System.Drawing.Point(64, 107)
        Me.ListBox_AdditionalFiles.Margin = New System.Windows.Forms.Padding(64, 3, 64, 64)
        Me.ListBox_AdditionalFiles.Name = "ListBox_AdditionalFiles"
        Me.ListBox_AdditionalFiles.Size = New System.Drawing.Size(512, 316)
        Me.ListBox_AdditionalFiles.TabIndex = 0
        '
        'RadioButton_StoreSingleFile
        '
        Me.RadioButton_StoreSingleFile.AutoSize = True
        Me.RadioButton_StoreSingleFile.Checked = True
        Me.RadioButton_StoreSingleFile.Location = New System.Drawing.Point(64, 32)
        Me.RadioButton_StoreSingleFile.Margin = New System.Windows.Forms.Padding(64, 32, 3, 3)
        Me.RadioButton_StoreSingleFile.Name = "RadioButton_StoreSingleFile"
        Me.RadioButton_StoreSingleFile.Size = New System.Drawing.Size(245, 17)
        Me.RadioButton_StoreSingleFile.TabIndex = 1
        Me.RadioButton_StoreSingleFile.TabStop = True
        Me.RadioButton_StoreSingleFile.Text = "Store translations as single translation file."
        Me.RadioButton_StoreSingleFile.UseVisualStyleBackColor = True
        '
        'RadioButton_StoreMultiFiles
        '
        Me.RadioButton_StoreMultiFiles.AutoSize = True
        Me.RadioButton_StoreMultiFiles.Location = New System.Drawing.Point(64, 55)
        Me.RadioButton_StoreMultiFiles.Margin = New System.Windows.Forms.Padding(64, 3, 3, 3)
        Me.RadioButton_StoreMultiFiles.Name = "RadioButton_StoreMultiFiles"
        Me.RadioButton_StoreMultiFiles.Size = New System.Drawing.Size(271, 17)
        Me.RadioButton_StoreMultiFiles.TabIndex = 2
        Me.RadioButton_StoreMultiFiles.Text = "Store translations into multiple translation files."
        Me.RadioButton_StoreMultiFiles.UseVisualStyleBackColor = True
        '
        'Label_AdditionalFiles
        '
        Me.Label_AdditionalFiles.AutoSize = True
        Me.Label_AdditionalFiles.Location = New System.Drawing.Point(64, 91)
        Me.Label_AdditionalFiles.Margin = New System.Windows.Forms.Padding(64, 16, 3, 0)
        Me.Label_AdditionalFiles.Name = "Label_AdditionalFiles"
        Me.Label_AdditionalFiles.Size = New System.Drawing.Size(166, 13)
        Me.Label_AdditionalFiles.TabIndex = 3
        Me.Label_AdditionalFiles.Text = "Additional files will be created:"
        '
        'UCExportWizardFileMode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.Label_AdditionalFiles)
        Me.Controls.Add(Me.RadioButton_StoreMultiFiles)
        Me.Controls.Add(Me.RadioButton_StoreSingleFile)
        Me.Controls.Add(Me.ListBox_AdditionalFiles)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UCExportWizardFileMode"
        Me.Size = New System.Drawing.Size(640, 480)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ListBox_AdditionalFiles As Windows.Forms.ListBox
    Friend WithEvents RadioButton_StoreSingleFile As Windows.Forms.RadioButton
    Friend WithEvents RadioButton_StoreMultiFiles As Windows.Forms.RadioButton
    Friend WithEvents Label_AdditionalFiles As Windows.Forms.Label
End Class
