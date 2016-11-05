<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCInformationList
    Inherits System.Windows.Forms.UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ListBox_Information = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'ListBox_Information
        '
        Me.ListBox_Information.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListBox_Information.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ListBox_Information.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox_Information.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox_Information.FormattingEnabled = True
        Me.ListBox_Information.HorizontalScrollbar = True
        Me.ListBox_Information.Location = New System.Drawing.Point(0, 0)
        Me.ListBox_Information.Name = "ListBox_Information"
        Me.ListBox_Information.Size = New System.Drawing.Size(532, 178)
        Me.ListBox_Information.TabIndex = 0
        '
        'UCInformationList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.ListBox_Information)
        Me.Name = "UCInformationList"
        Me.Size = New System.Drawing.Size(532, 178)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ListBox_Information As ListBox
End Class
