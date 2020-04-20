<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCAutocomplete
    Inherits System.Windows.Forms.UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ListBox_Autocomplete = New BasicPawn.ClassAutocompleteListBox()
        Me.ContextMenuStrip_Autocomplete = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_FindDefinition = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_PeekDefinition = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.Panel_IntelliSense = New System.Windows.Forms.Panel()
        Me.TextEditorControlEx_IntelliSense = New BasicPawn.TextEditorControlEx()
        Me.Label_IntelliSense = New System.Windows.Forms.Label()
        Me.Panel_Autocomplete = New System.Windows.Forms.Panel()
        Me.TextEditorControlEx_Autocomplete = New BasicPawn.TextEditorControlEx()
        Me.Label_Autocomplete = New System.Windows.Forms.Label()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ContextMenuStrip_Autocomplete.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.Panel_IntelliSense.SuspendLayout()
        Me.Panel_Autocomplete.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 0
        Me.ToolTip1.ReshowDelay = 100
        Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.ToolTip1.UseAnimation = False
        Me.ToolTip1.UseFading = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ListBox_Autocomplete)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(799, 161)
        Me.SplitContainer1.SplitterDistance = 376
        Me.SplitContainer1.TabIndex = 1
        '
        'ListBox_Autocomplete
        '
        Me.ListBox_Autocomplete.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListBox_Autocomplete.ContextMenuStrip = Me.ContextMenuStrip_Autocomplete
        Me.ListBox_Autocomplete.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox_Autocomplete.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ListBox_Autocomplete.FormattingEnabled = True
        Me.ListBox_Autocomplete.ItemHeight = 16
        Me.ListBox_Autocomplete.Location = New System.Drawing.Point(0, 0)
        Me.ListBox_Autocomplete.Name = "ListBox_Autocomplete"
        Me.ListBox_Autocomplete.Size = New System.Drawing.Size(376, 161)
        Me.ListBox_Autocomplete.TabIndex = 0
        '
        'ContextMenuStrip_Autocomplete
        '
        Me.ContextMenuStrip_Autocomplete.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_FindDefinition, Me.ToolStripMenuItem_PeekDefinition})
        Me.ContextMenuStrip_Autocomplete.Name = "ContextMenuStrip_Autocomplete"
        Me.ContextMenuStrip_Autocomplete.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_Autocomplete.Size = New System.Drawing.Size(154, 48)
        '
        'ToolStripMenuItem_FindDefinition
        '
        Me.ToolStripMenuItem_FindDefinition.Image = Global.BasicPawn.My.Resources.Resources.imageres_5357_16x16
        Me.ToolStripMenuItem_FindDefinition.Name = "ToolStripMenuItem_FindDefinition"
        Me.ToolStripMenuItem_FindDefinition.Size = New System.Drawing.Size(153, 22)
        Me.ToolStripMenuItem_FindDefinition.Text = "Find definition"
        '
        'ToolStripMenuItem_PeekDefinition
        '
        Me.ToolStripMenuItem_PeekDefinition.Name = "ToolStripMenuItem_PeekDefinition"
        Me.ToolStripMenuItem_PeekDefinition.Size = New System.Drawing.Size(153, 22)
        Me.ToolStripMenuItem_PeekDefinition.Text = "Peek definition"
        '
        'SplitContainer2
        '
        Me.SplitContainer2.BackColor = System.Drawing.Color.White
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.Panel_IntelliSense)
        Me.SplitContainer2.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.Panel_Autocomplete)
        Me.SplitContainer2.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SplitContainer2.Size = New System.Drawing.Size(419, 161)
        Me.SplitContainer2.SplitterDistance = 77
        Me.SplitContainer2.TabIndex = 1
        '
        'Panel_IntelliSense
        '
        Me.Panel_IntelliSense.Controls.Add(Me.TextEditorControlEx_IntelliSense)
        Me.Panel_IntelliSense.Controls.Add(Me.Label_IntelliSense)
        Me.Panel_IntelliSense.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_IntelliSense.Location = New System.Drawing.Point(0, 0)
        Me.Panel_IntelliSense.Name = "Panel_IntelliSense"
        Me.Panel_IntelliSense.Size = New System.Drawing.Size(419, 77)
        Me.Panel_IntelliSense.TabIndex = 3
        '
        'TextEditorControlEx_IntelliSense
        '
        Me.TextEditorControlEx_IntelliSense.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextEditorControlEx_IntelliSense.EnableFolding = False
        Me.TextEditorControlEx_IntelliSense.IsReadOnly = False
        Me.TextEditorControlEx_IntelliSense.Location = New System.Drawing.Point(0, 13)
        Me.TextEditorControlEx_IntelliSense.Margin = New System.Windows.Forms.Padding(0)
        Me.TextEditorControlEx_IntelliSense.Name = "TextEditorControlEx_IntelliSense"
        Me.TextEditorControlEx_IntelliSense.ShowLineNumbers = False
        Me.TextEditorControlEx_IntelliSense.ShowMatchingBracket = False
        Me.TextEditorControlEx_IntelliSense.ShowVRuler = False
        Me.TextEditorControlEx_IntelliSense.Size = New System.Drawing.Size(419, 64)
        Me.TextEditorControlEx_IntelliSense.TabIndex = 5
        Me.TextEditorControlEx_IntelliSense.Text = "IntelliSense"
        '
        'Label_IntelliSense
        '
        Me.Label_IntelliSense.AutoSize = True
        Me.Label_IntelliSense.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label_IntelliSense.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_IntelliSense.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Label_IntelliSense.Location = New System.Drawing.Point(0, 0)
        Me.Label_IntelliSense.Name = "Label_IntelliSense"
        Me.Label_IntelliSense.Size = New System.Drawing.Size(66, 13)
        Me.Label_IntelliSense.TabIndex = 2
        Me.Label_IntelliSense.Text = "IntelliSense"
        '
        'Panel_Autocomplete
        '
        Me.Panel_Autocomplete.Controls.Add(Me.TextEditorControlEx_Autocomplete)
        Me.Panel_Autocomplete.Controls.Add(Me.Label_Autocomplete)
        Me.Panel_Autocomplete.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_Autocomplete.Location = New System.Drawing.Point(0, 0)
        Me.Panel_Autocomplete.Name = "Panel_Autocomplete"
        Me.Panel_Autocomplete.Size = New System.Drawing.Size(419, 80)
        Me.Panel_Autocomplete.TabIndex = 4
        '
        'TextEditorControlEx_Autocomplete
        '
        Me.TextEditorControlEx_Autocomplete.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextEditorControlEx_Autocomplete.EnableFolding = False
        Me.TextEditorControlEx_Autocomplete.IsReadOnly = False
        Me.TextEditorControlEx_Autocomplete.Location = New System.Drawing.Point(0, 13)
        Me.TextEditorControlEx_Autocomplete.Margin = New System.Windows.Forms.Padding(0)
        Me.TextEditorControlEx_Autocomplete.Name = "TextEditorControlEx_Autocomplete"
        Me.TextEditorControlEx_Autocomplete.ShowLineNumbers = False
        Me.TextEditorControlEx_Autocomplete.ShowMatchingBracket = False
        Me.TextEditorControlEx_Autocomplete.ShowVRuler = False
        Me.TextEditorControlEx_Autocomplete.Size = New System.Drawing.Size(419, 67)
        Me.TextEditorControlEx_Autocomplete.TabIndex = 4
        Me.TextEditorControlEx_Autocomplete.Text = "Autocomplete"
        '
        'Label_Autocomplete
        '
        Me.Label_Autocomplete.AutoSize = True
        Me.Label_Autocomplete.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label_Autocomplete.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Autocomplete.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Label_Autocomplete.Location = New System.Drawing.Point(0, 0)
        Me.Label_Autocomplete.Name = "Label_Autocomplete"
        Me.Label_Autocomplete.Size = New System.Drawing.Size(79, 13)
        Me.Label_Autocomplete.TabIndex = 3
        Me.Label_Autocomplete.Text = "Autocomplete"
        '
        'UCAutocomplete
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.SplitContainer1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimumSize = New System.Drawing.Size(2, 100)
        Me.Name = "UCAutocomplete"
        Me.Size = New System.Drawing.Size(799, 161)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ContextMenuStrip_Autocomplete.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.Panel_IntelliSense.ResumeLayout(False)
        Me.Panel_IntelliSense.PerformLayout()
        Me.Panel_Autocomplete.ResumeLayout(False)
        Me.Panel_Autocomplete.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents Panel_IntelliSense As Panel
    Friend WithEvents Label_IntelliSense As Label
    Friend WithEvents Panel_Autocomplete As Panel
    Friend WithEvents Label_Autocomplete As Label
    Friend WithEvents TextEditorControlEx_Autocomplete As TextEditorControlEx
    Friend WithEvents TextEditorControlEx_IntelliSense As TextEditorControlEx
    Friend WithEvents ListBox_Autocomplete As ClassAutocompleteListBox
    Friend WithEvents ContextMenuStrip_Autocomplete As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_FindDefinition As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_PeekDefinition As ToolStripMenuItem
End Class
