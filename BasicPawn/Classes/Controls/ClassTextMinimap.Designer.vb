<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ClassTextMinimap
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.RichTextBoxEx_Minimap = New BasicPawn.ClassRichTextBoxFix()
        Me.SuspendLayout()
        '
        'RichTextBoxEx_Minimap
        '
        Me.RichTextBoxEx_Minimap.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBoxEx_Minimap.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.RichTextBoxEx_Minimap.DetectUrls = False
        Me.RichTextBoxEx_Minimap.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBoxEx_Minimap.Location = New System.Drawing.Point(0, 0)
        Me.RichTextBoxEx_Minimap.m_AllowZoom = False
        Me.RichTextBoxEx_Minimap.m_SelectionEnabled = False
        Me.RichTextBoxEx_Minimap.Name = "RichTextBoxEx_Minimap"
        Me.RichTextBoxEx_Minimap.ReadOnly = True
        Me.RichTextBoxEx_Minimap.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None
        Me.RichTextBoxEx_Minimap.ShortcutsEnabled = False
        Me.RichTextBoxEx_Minimap.Size = New System.Drawing.Size(150, 549)
        Me.RichTextBoxEx_Minimap.TabIndex = 0
        Me.RichTextBoxEx_Minimap.Text = ""
        Me.RichTextBoxEx_Minimap.WordWrap = False
        '
        'ClassTextMinimap
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.Controls.Add(Me.RichTextBoxEx_Minimap)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "ClassTextMinimap"
        Me.Size = New System.Drawing.Size(150, 549)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RichTextBoxEx_Minimap As ClassRichTextBoxFix
End Class
