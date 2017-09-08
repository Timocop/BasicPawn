<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormToolTip
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
        Me.components = New System.ComponentModel.Container()
        Me.Timer_Move = New System.Windows.Forms.Timer(Me.components)
        Me.TextEditorControl_ToolTip = New BasicPawn.TextEditorControlEx()
        Me.SuspendLayout()
        '
        'Timer_Move
        '
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
        Me.TextEditorControl_ToolTip.Size = New System.Drawing.Size(447, 129)
        Me.TextEditorControl_ToolTip.TabIndex = 1
        '
        'FormToolTip
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(447, 129)
        Me.ControlBox = False
        Me.Controls.Add(Me.TextEditorControl_ToolTip)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormToolTip"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TextEditorControl_ToolTip As TextEditorControlEx
    Friend WithEvents Timer_Move As Timer
End Class
