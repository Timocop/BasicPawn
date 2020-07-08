<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCExportWizardFinalize
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ListBox_Config = New System.Windows.Forms.ListBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(65, 32)
        Me.Label1.Margin = New System.Windows.Forms.Padding(32, 32, 3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(330, 39)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "You have successfully completed the Translation Export Wizard." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "You have specif" &
    "ied the following settings:"
        '
        'ListBox_Config
        '
        Me.ListBox_Config.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox_Config.FormattingEnabled = True
        Me.ListBox_Config.HorizontalScrollbar = True
        Me.ListBox_Config.Location = New System.Drawing.Point(64, 87)
        Me.ListBox_Config.Margin = New System.Windows.Forms.Padding(64, 16, 64, 16)
        Me.ListBox_Config.Name = "ListBox_Config"
        Me.ListBox_Config.Size = New System.Drawing.Size(512, 303)
        Me.ListBox_Config.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(65, 403)
        Me.Label3.Margin = New System.Windows.Forms.Padding(64, 16, 3, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(142, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Click Finish to export files."
        '
        'UCExportWizardFinalize
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ListBox_Config)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UCExportWizardFinalize"
        Me.Size = New System.Drawing.Size(640, 480)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents ListBox_Config As Windows.Forms.ListBox
    Friend WithEvents Label3 As Windows.Forms.Label
End Class
