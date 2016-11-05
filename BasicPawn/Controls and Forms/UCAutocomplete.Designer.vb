<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCAutocomplete
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
        Me.ListView_AutocompleteList = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.RichTextBox_IntelliSense = New System.Windows.Forms.RichTextBox()
        Me.Label_IntelliSense = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.RichTextBox_Autocomplete = New System.Windows.Forms.RichTextBox()
        Me.Label_Autocomplete = New System.Windows.Forms.Label()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListView_AutocompleteList
        '
        Me.ListView_AutocompleteList.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListView_AutocompleteList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader4, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.ListView_AutocompleteList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_AutocompleteList.FullRowSelect = True
        Me.ListView_AutocompleteList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.ListView_AutocompleteList.HideSelection = False
        Me.ListView_AutocompleteList.Location = New System.Drawing.Point(0, 0)
        Me.ListView_AutocompleteList.MultiSelect = False
        Me.ListView_AutocompleteList.Name = "ListView_AutocompleteList"
        Me.ListView_AutocompleteList.Size = New System.Drawing.Size(376, 161)
        Me.ListView_AutocompleteList.TabIndex = 0
        Me.ListView_AutocompleteList.UseCompatibleStateImageBehavior = False
        Me.ListView_AutocompleteList.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "File"
        Me.ColumnHeader1.Width = 137
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Type"
        Me.ColumnHeader4.Width = 89
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Name"
        Me.ColumnHeader2.Width = 169
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Full Name"
        Me.ColumnHeader3.Width = 398
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
        Me.SplitContainer1.Panel1.Controls.Add(Me.ListView_AutocompleteList)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(799, 161)
        Me.SplitContainer1.SplitterDistance = 376
        Me.SplitContainer1.TabIndex = 1
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
        Me.SplitContainer2.Panel1.Controls.Add(Me.Panel2)
        Me.SplitContainer2.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.Panel1)
        Me.SplitContainer2.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SplitContainer2.Size = New System.Drawing.Size(419, 161)
        Me.SplitContainer2.SplitterDistance = 77
        Me.SplitContainer2.TabIndex = 1
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.RichTextBox_IntelliSense)
        Me.Panel2.Controls.Add(Me.Label_IntelliSense)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(419, 77)
        Me.Panel2.TabIndex = 3
        '
        'RichTextBox_IntelliSense
        '
        Me.RichTextBox_IntelliSense.BackColor = System.Drawing.Color.White
        Me.RichTextBox_IntelliSense.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox_IntelliSense.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.RichTextBox_IntelliSense.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBox_IntelliSense.Location = New System.Drawing.Point(0, 13)
        Me.RichTextBox_IntelliSense.Name = "RichTextBox_IntelliSense"
        Me.RichTextBox_IntelliSense.ReadOnly = True
        Me.RichTextBox_IntelliSense.Size = New System.Drawing.Size(419, 64)
        Me.RichTextBox_IntelliSense.TabIndex = 1
        Me.RichTextBox_IntelliSense.Text = ""
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
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.RichTextBox_Autocomplete)
        Me.Panel1.Controls.Add(Me.Label_Autocomplete)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(419, 80)
        Me.Panel1.TabIndex = 4
        '
        'RichTextBox_Autocomplete
        '
        Me.RichTextBox_Autocomplete.BackColor = System.Drawing.Color.White
        Me.RichTextBox_Autocomplete.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox_Autocomplete.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.RichTextBox_Autocomplete.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBox_Autocomplete.Location = New System.Drawing.Point(0, 13)
        Me.RichTextBox_Autocomplete.Name = "RichTextBox_Autocomplete"
        Me.RichTextBox_Autocomplete.ReadOnly = True
        Me.RichTextBox_Autocomplete.Size = New System.Drawing.Size(419, 67)
        Me.RichTextBox_Autocomplete.TabIndex = 0
        Me.RichTextBox_Autocomplete.Text = ""
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
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimumSize = New System.Drawing.Size(2, 100)
        Me.Name = "UCAutocomplete"
        Me.Size = New System.Drawing.Size(799, 161)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ListView_AutocompleteList As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents Panel2 As Panel
    Friend WithEvents RichTextBox_IntelliSense As RichTextBox
    Friend WithEvents Label_IntelliSense As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents RichTextBox_Autocomplete As RichTextBox
    Friend WithEvents Label_Autocomplete As Label
End Class
