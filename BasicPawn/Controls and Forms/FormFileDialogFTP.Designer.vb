<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormFileDialogFTP
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container()
        Me.Button_Apply = New System.Windows.Forms.Button()
        Me.TextBox_Filename = New System.Windows.Forms.TextBox()
        Me.ListView_FTP = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ImageList_FTP = New System.Windows.Forms.ImageList(Me.components)
        Me.TextBox_Path = New System.Windows.Forms.TextBox()
        Me.Button_Refresh = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Button_Apply
        '
        Me.Button_Apply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Apply.Location = New System.Drawing.Point(537, 406)
        Me.Button_Apply.Name = "Button_Apply"
        Me.Button_Apply.Size = New System.Drawing.Size(75, 23)
        Me.Button_Apply.TabIndex = 0
        Me.Button_Apply.Text = "Save"
        Me.Button_Apply.UseVisualStyleBackColor = True
        '
        'TextBox_Filename
        '
        Me.TextBox_Filename.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_Filename.Location = New System.Drawing.Point(12, 408)
        Me.TextBox_Filename.Name = "TextBox_Filename"
        Me.TextBox_Filename.Size = New System.Drawing.Size(519, 22)
        Me.TextBox_Filename.TabIndex = 1
        '
        'ListView_FTP
        '
        Me.ListView_FTP.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_FTP.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.ListView_FTP.FullRowSelect = True
        Me.ListView_FTP.Location = New System.Drawing.Point(12, 40)
        Me.ListView_FTP.MultiSelect = False
        Me.ListView_FTP.Name = "ListView_FTP"
        Me.ListView_FTP.Size = New System.Drawing.Size(600, 360)
        Me.ListView_FTP.SmallImageList = Me.ImageList_FTP
        Me.ListView_FTP.TabIndex = 2
        Me.ListView_FTP.UseCompatibleStateImageBehavior = False
        Me.ListView_FTP.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Size"
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Changed"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Permissions"
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Owner"
        '
        'ImageList_FTP
        '
        Me.ImageList_FTP.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList_FTP.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList_FTP.TransparentColor = System.Drawing.Color.Transparent
        '
        'TextBox_Path
        '
        Me.TextBox_Path.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_Path.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_Path.Location = New System.Drawing.Point(12, 12)
        Me.TextBox_Path.Name = "TextBox_Path"
        Me.TextBox_Path.Size = New System.Drawing.Size(519, 22)
        Me.TextBox_Path.TabIndex = 3
        '
        'Button_Refresh
        '
        Me.Button_Refresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Refresh.Location = New System.Drawing.Point(537, 12)
        Me.Button_Refresh.Name = "Button_Refresh"
        Me.Button_Refresh.Size = New System.Drawing.Size(75, 23)
        Me.Button_Refresh.TabIndex = 4
        Me.Button_Refresh.Text = "Refresh"
        Me.Button_Refresh.UseVisualStyleBackColor = True
        '
        'FormFileDialogFTP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 441)
        Me.Controls.Add(Me.Button_Refresh)
        Me.Controls.Add(Me.TextBox_Path)
        Me.Controls.Add(Me.ListView_FTP)
        Me.Controls.Add(Me.TextBox_Filename)
        Me.Controls.Add(Me.Button_Apply)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormFileDialogFTP"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Save to..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button_Apply As Button
    Friend WithEvents TextBox_Filename As TextBox
    Friend WithEvents ListView_FTP As ListView
    Friend WithEvents ImageList_FTP As ImageList
    Friend WithEvents TextBox_Path As TextBox
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ColumnHeader5 As ColumnHeader
    Friend WithEvents Button_Refresh As Button
End Class
