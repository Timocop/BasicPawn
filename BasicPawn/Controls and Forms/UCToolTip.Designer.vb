<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCToolTip
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
        Me.TextEditorControl_ToolTip = New BasicPawn.TextEditorControlEx()
        Me.SuspendLayout()
        '
        'TextEditorControl_ToolTip
        '
        Me.TextEditorControl_ToolTip.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextEditorControl_ToolTip.EnableFolding = False
        Me.TextEditorControl_ToolTip.IsReadOnly = False
        Me.TextEditorControl_ToolTip.Location = New System.Drawing.Point(0, 0)
        Me.TextEditorControl_ToolTip.Name = "TextEditorControl_ToolTip"
        Me.TextEditorControl_ToolTip.ShowLineNumbers = False
        Me.TextEditorControl_ToolTip.ShowMatchingBracket = False
        Me.TextEditorControl_ToolTip.ShowVRuler = False
        Me.TextEditorControl_ToolTip.Size = New System.Drawing.Size(687, 270)
        Me.TextEditorControl_ToolTip.TabIndex = 0
        '
        'UCToolTip
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.TextEditorControl_ToolTip)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UCToolTip"
        Me.Size = New System.Drawing.Size(687, 270)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TextEditorControl_ToolTip As TextEditorControlEx
End Class
