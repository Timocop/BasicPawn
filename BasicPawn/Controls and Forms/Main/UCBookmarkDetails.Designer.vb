<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UCBookmarkDetails
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
        Me.components = New System.ComponentModel.Container()
        Me.ListView_Bookmarks = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuStrip_Bookmarks = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_Goto = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_AddBookmark = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_EditBookmark = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_RemoveBookmark = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_RefreshBookmark = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_LocalBookmarksOnly = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList_Bookmarks = New System.Windows.Forms.ImageList(Me.components)
        Me.ContextMenuStrip_Bookmarks.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListView_Bookmarks
        '
        Me.ListView_Bookmarks.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListView_Bookmarks.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.ListView_Bookmarks.ContextMenuStrip = Me.ContextMenuStrip_Bookmarks
        Me.ListView_Bookmarks.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_Bookmarks.FullRowSelect = True
        Me.ListView_Bookmarks.Location = New System.Drawing.Point(0, 0)
        Me.ListView_Bookmarks.Name = "ListView_Bookmarks"
        Me.ListView_Bookmarks.Size = New System.Drawing.Size(600, 200)
        Me.ListView_Bookmarks.SmallImageList = Me.ImageList_Bookmarks
        Me.ListView_Bookmarks.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView_Bookmarks.TabIndex = 0
        Me.ListView_Bookmarks.UseCompatibleStateImageBehavior = False
        Me.ListView_Bookmarks.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 125
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "File"
        Me.ColumnHeader2.Width = 360
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Line"
        Me.ColumnHeader3.Width = 50
        '
        'ContextMenuStrip_Bookmarks
        '
        Me.ContextMenuStrip_Bookmarks.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_Goto, Me.ToolStripSeparator3, Me.ToolStripMenuItem_AddBookmark, Me.ToolStripMenuItem_EditBookmark, Me.ToolStripMenuItem_RemoveBookmark, Me.ToolStripSeparator1, Me.ToolStripMenuItem_RefreshBookmark, Me.ToolStripSeparator2, Me.ToolStripMenuItem_LocalBookmarksOnly})
        Me.ContextMenuStrip_Bookmarks.Name = "ContextMenuStrip_Bookmarks"
        Me.ContextMenuStrip_Bookmarks.Size = New System.Drawing.Size(220, 154)
        '
        'ToolStripMenuItem_Goto
        '
        Me.ToolStripMenuItem_Goto.Image = Global.BasicPawn.My.Resources.Resources.imageres_5302_16x16
        Me.ToolStripMenuItem_Goto.Name = "ToolStripMenuItem_Goto"
        Me.ToolStripMenuItem_Goto.Size = New System.Drawing.Size(219, 22)
        Me.ToolStripMenuItem_Goto.Text = "Goto line"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(216, 6)
        '
        'ToolStripMenuItem_AddBookmark
        '
        Me.ToolStripMenuItem_AddBookmark.Image = Global.BasicPawn.My.Resources.Resources.imageres_5376_16x16
        Me.ToolStripMenuItem_AddBookmark.Name = "ToolStripMenuItem_AddBookmark"
        Me.ToolStripMenuItem_AddBookmark.ShortcutKeyDisplayString = "Ctrl+K"
        Me.ToolStripMenuItem_AddBookmark.Size = New System.Drawing.Size(219, 22)
        Me.ToolStripMenuItem_AddBookmark.Text = "Add bookmark"
        '
        'ToolStripMenuItem_EditBookmark
        '
        Me.ToolStripMenuItem_EditBookmark.Name = "ToolStripMenuItem_EditBookmark"
        Me.ToolStripMenuItem_EditBookmark.Size = New System.Drawing.Size(219, 22)
        Me.ToolStripMenuItem_EditBookmark.Text = "Edit bookmark"
        '
        'ToolStripMenuItem_RemoveBookmark
        '
        Me.ToolStripMenuItem_RemoveBookmark.Name = "ToolStripMenuItem_RemoveBookmark"
        Me.ToolStripMenuItem_RemoveBookmark.ShortcutKeyDisplayString = ""
        Me.ToolStripMenuItem_RemoveBookmark.Size = New System.Drawing.Size(219, 22)
        Me.ToolStripMenuItem_RemoveBookmark.Text = "Remove bookmark"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(216, 6)
        '
        'ToolStripMenuItem_RefreshBookmark
        '
        Me.ToolStripMenuItem_RefreshBookmark.Image = Global.BasicPawn.My.Resources.Resources.shell32_16739_16x16
        Me.ToolStripMenuItem_RefreshBookmark.Name = "ToolStripMenuItem_RefreshBookmark"
        Me.ToolStripMenuItem_RefreshBookmark.Size = New System.Drawing.Size(219, 22)
        Me.ToolStripMenuItem_RefreshBookmark.Text = "Refresh"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(216, 6)
        '
        'ToolStripMenuItem_LocalBookmarksOnly
        '
        Me.ToolStripMenuItem_LocalBookmarksOnly.CheckOnClick = True
        Me.ToolStripMenuItem_LocalBookmarksOnly.Name = "ToolStripMenuItem_LocalBookmarksOnly"
        Me.ToolStripMenuItem_LocalBookmarksOnly.Size = New System.Drawing.Size(219, 22)
        Me.ToolStripMenuItem_LocalBookmarksOnly.Text = "Show local bookmarks only"
        '
        'ImageList_Bookmarks
        '
        Me.ImageList_Bookmarks.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList_Bookmarks.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList_Bookmarks.TransparentColor = System.Drawing.Color.Transparent
        '
        'UCBookmarkDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.ListView_Bookmarks)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UCBookmarkDetails"
        Me.Size = New System.Drawing.Size(600, 200)
        Me.ContextMenuStrip_Bookmarks.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ListView_Bookmarks As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ImageList_Bookmarks As ImageList
    Friend WithEvents ContextMenuStrip_Bookmarks As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_AddBookmark As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_EditBookmark As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_RemoveBookmark As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_RefreshBookmark As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_LocalBookmarksOnly As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Goto As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
End Class
