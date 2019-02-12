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
        Me.components = New System.ComponentModel.Container()
        Me.ListBox_Information = New System.Windows.Forms.ListBox()
        Me.ContextMenuStrip_Information = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_OpenExplorer = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_GotoLine = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_OpenNotepad = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_OpenNotepadAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_OpenNotepadAllFull = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_OpenNotepadAllMin = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_OpenNotepadSelected = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_OpenNotepadSelectedFull = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_OpenNotepadSelectedMin = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Copy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_CopyAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_CopyAllFull = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_CopyAllMin = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_CopySelected = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_CopySelectedFull = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_CopySelectedMin = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip_Information.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListBox_Information
        '
        Me.ListBox_Information.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListBox_Information.ContextMenuStrip = Me.ContextMenuStrip_Information
        Me.ListBox_Information.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox_Information.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox_Information.FormattingEnabled = True
        Me.ListBox_Information.HorizontalScrollbar = True
        Me.ListBox_Information.Location = New System.Drawing.Point(0, 0)
        Me.ListBox_Information.Name = "ListBox_Information"
        Me.ListBox_Information.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.ListBox_Information.Size = New System.Drawing.Size(532, 178)
        Me.ListBox_Information.TabIndex = 0
        '
        'ContextMenuStrip_Information
        '
        Me.ContextMenuStrip_Information.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_OpenExplorer, Me.ToolStripMenuItem_GotoLine, Me.ToolStripSeparator1, Me.ToolStripMenuItem_OpenNotepad, Me.ToolStripMenuItem_Copy})
        Me.ContextMenuStrip_Information.Name = "ContextMenuStrip_Information"
        Me.ContextMenuStrip_Information.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_Information.Size = New System.Drawing.Size(166, 98)
        '
        'ToolStripMenuItem_OpenExplorer
        '
        Me.ToolStripMenuItem_OpenExplorer.Image = Global.BasicPawn.My.Resources.Resources.imageres_5304_16x16
        Me.ToolStripMenuItem_OpenExplorer.Name = "ToolStripMenuItem_OpenExplorer"
        Me.ToolStripMenuItem_OpenExplorer.Size = New System.Drawing.Size(165, 22)
        Me.ToolStripMenuItem_OpenExplorer.Text = "Open in Explorer"
        '
        'ToolStripMenuItem_GotoLine
        '
        Me.ToolStripMenuItem_GotoLine.Image = Global.BasicPawn.My.Resources.Resources.imageres_5302_16x16
        Me.ToolStripMenuItem_GotoLine.Name = "ToolStripMenuItem_GotoLine"
        Me.ToolStripMenuItem_GotoLine.Size = New System.Drawing.Size(165, 22)
        Me.ToolStripMenuItem_GotoLine.Text = "Goto line"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(162, 6)
        '
        'ToolStripMenuItem_OpenNotepad
        '
        Me.ToolStripMenuItem_OpenNotepad.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_OpenNotepadAll, Me.ToolStripMenuItem_OpenNotepadSelected})
        Me.ToolStripMenuItem_OpenNotepad.Image = Global.BasicPawn.My.Resources.Resources.imageres_5338_16x16
        Me.ToolStripMenuItem_OpenNotepad.Name = "ToolStripMenuItem_OpenNotepad"
        Me.ToolStripMenuItem_OpenNotepad.Size = New System.Drawing.Size(165, 22)
        Me.ToolStripMenuItem_OpenNotepad.Text = "Open in Notepad"
        '
        'ToolStripMenuItem_OpenNotepadAll
        '
        Me.ToolStripMenuItem_OpenNotepadAll.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_OpenNotepadAllFull, Me.ToolStripMenuItem_OpenNotepadAllMin})
        Me.ToolStripMenuItem_OpenNotepadAll.Name = "ToolStripMenuItem_OpenNotepadAll"
        Me.ToolStripMenuItem_OpenNotepadAll.Size = New System.Drawing.Size(118, 22)
        Me.ToolStripMenuItem_OpenNotepadAll.Text = "All"
        '
        'ToolStripMenuItem_OpenNotepadAllFull
        '
        Me.ToolStripMenuItem_OpenNotepadAllFull.Name = "ToolStripMenuItem_OpenNotepadAllFull"
        Me.ToolStripMenuItem_OpenNotepadAllFull.Size = New System.Drawing.Size(118, 22)
        Me.ToolStripMenuItem_OpenNotepadAllFull.Text = "Full"
        '
        'ToolStripMenuItem_OpenNotepadAllMin
        '
        Me.ToolStripMenuItem_OpenNotepadAllMin.Name = "ToolStripMenuItem_OpenNotepadAllMin"
        Me.ToolStripMenuItem_OpenNotepadAllMin.Size = New System.Drawing.Size(118, 22)
        Me.ToolStripMenuItem_OpenNotepadAllMin.Text = "Minimal"
        '
        'ToolStripMenuItem_OpenNotepadSelected
        '
        Me.ToolStripMenuItem_OpenNotepadSelected.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_OpenNotepadSelectedFull, Me.ToolStripMenuItem_OpenNotepadSelectedMin})
        Me.ToolStripMenuItem_OpenNotepadSelected.Name = "ToolStripMenuItem_OpenNotepadSelected"
        Me.ToolStripMenuItem_OpenNotepadSelected.Size = New System.Drawing.Size(118, 22)
        Me.ToolStripMenuItem_OpenNotepadSelected.Text = "Selected"
        '
        'ToolStripMenuItem_OpenNotepadSelectedFull
        '
        Me.ToolStripMenuItem_OpenNotepadSelectedFull.Name = "ToolStripMenuItem_OpenNotepadSelectedFull"
        Me.ToolStripMenuItem_OpenNotepadSelectedFull.Size = New System.Drawing.Size(118, 22)
        Me.ToolStripMenuItem_OpenNotepadSelectedFull.Text = "Full"
        '
        'ToolStripMenuItem_OpenNotepadSelectedMin
        '
        Me.ToolStripMenuItem_OpenNotepadSelectedMin.Name = "ToolStripMenuItem_OpenNotepadSelectedMin"
        Me.ToolStripMenuItem_OpenNotepadSelectedMin.Size = New System.Drawing.Size(118, 22)
        Me.ToolStripMenuItem_OpenNotepadSelectedMin.Text = "Minimal"
        '
        'ToolStripMenuItem_Copy
        '
        Me.ToolStripMenuItem_Copy.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_CopyAll, Me.ToolStripMenuItem_CopySelected})
        Me.ToolStripMenuItem_Copy.Image = Global.BasicPawn.My.Resources.Resources.imageres_5306_16x16
        Me.ToolStripMenuItem_Copy.Name = "ToolStripMenuItem_Copy"
        Me.ToolStripMenuItem_Copy.Size = New System.Drawing.Size(165, 22)
        Me.ToolStripMenuItem_Copy.Text = "Copy"
        '
        'ToolStripMenuItem_CopyAll
        '
        Me.ToolStripMenuItem_CopyAll.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_CopyAllFull, Me.ToolStripMenuItem_CopyAllMin})
        Me.ToolStripMenuItem_CopyAll.Name = "ToolStripMenuItem_CopyAll"
        Me.ToolStripMenuItem_CopyAll.Size = New System.Drawing.Size(118, 22)
        Me.ToolStripMenuItem_CopyAll.Text = "All"
        '
        'ToolStripMenuItem_CopyAllFull
        '
        Me.ToolStripMenuItem_CopyAllFull.Name = "ToolStripMenuItem_CopyAllFull"
        Me.ToolStripMenuItem_CopyAllFull.Size = New System.Drawing.Size(118, 22)
        Me.ToolStripMenuItem_CopyAllFull.Text = "Full"
        '
        'ToolStripMenuItem_CopyAllMin
        '
        Me.ToolStripMenuItem_CopyAllMin.Name = "ToolStripMenuItem_CopyAllMin"
        Me.ToolStripMenuItem_CopyAllMin.Size = New System.Drawing.Size(118, 22)
        Me.ToolStripMenuItem_CopyAllMin.Text = "Minimal"
        '
        'ToolStripMenuItem_CopySelected
        '
        Me.ToolStripMenuItem_CopySelected.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_CopySelectedFull, Me.ToolStripMenuItem_CopySelectedMin})
        Me.ToolStripMenuItem_CopySelected.Name = "ToolStripMenuItem_CopySelected"
        Me.ToolStripMenuItem_CopySelected.Size = New System.Drawing.Size(118, 22)
        Me.ToolStripMenuItem_CopySelected.Text = "Selected"
        '
        'ToolStripMenuItem_CopySelectedFull
        '
        Me.ToolStripMenuItem_CopySelectedFull.Name = "ToolStripMenuItem_CopySelectedFull"
        Me.ToolStripMenuItem_CopySelectedFull.Size = New System.Drawing.Size(118, 22)
        Me.ToolStripMenuItem_CopySelectedFull.Text = "Full"
        '
        'ToolStripMenuItem_CopySelectedMin
        '
        Me.ToolStripMenuItem_CopySelectedMin.Name = "ToolStripMenuItem_CopySelectedMin"
        Me.ToolStripMenuItem_CopySelectedMin.Size = New System.Drawing.Size(118, 22)
        Me.ToolStripMenuItem_CopySelectedMin.Text = "Minimal"
        '
        'UCInformationList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.ListBox_Information)
        Me.Name = "UCInformationList"
        Me.Size = New System.Drawing.Size(532, 178)
        Me.ContextMenuStrip_Information.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ListBox_Information As ListBox
    Friend WithEvents ContextMenuStrip_Information As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_OpenNotepad As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Copy As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_OpenExplorer As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_GotoLine As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_OpenNotepadAll As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_OpenNotepadAllFull As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_OpenNotepadAllMin As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_OpenNotepadSelected As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_OpenNotepadSelectedFull As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_OpenNotepadSelectedMin As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_CopyAll As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_CopyAllFull As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_CopyAllMin As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_CopySelected As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_CopySelectedFull As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_CopySelectedMin As ToolStripMenuItem
End Class
