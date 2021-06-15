<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCFtpPathDatabase
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
        Me.TableLayoutPanel_Controls = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox_NewEntry = New System.Windows.Forms.GroupBox()
        Me.TextBox_Host = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ComboBox_Protocol = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Button_SearchPath = New System.Windows.Forms.Button()
        Me.TextBox_DestinationPath = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBox_DatabaseEntry = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button_AddEntry = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LinkLabel_RemoveItem = New System.Windows.Forms.LinkLabel()
        Me.CheckBox_MoreDetails = New System.Windows.Forms.CheckBox()
        Me.ListView_FtpEntries = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TableLayoutPanel_Controls.SuspendLayout()
        Me.GroupBox_NewEntry.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel_Controls
        '
        Me.TableLayoutPanel_Controls.ColumnCount = 1
        Me.TableLayoutPanel_Controls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Controls.Controls.Add(Me.GroupBox_NewEntry, 0, 1)
        Me.TableLayoutPanel_Controls.Controls.Add(Me.Panel2, 0, 0)
        Me.TableLayoutPanel_Controls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_Controls.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel_Controls.Name = "TableLayoutPanel_Controls"
        Me.TableLayoutPanel_Controls.RowCount = 2
        Me.TableLayoutPanel_Controls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Controls.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel_Controls.Size = New System.Drawing.Size(581, 504)
        Me.TableLayoutPanel_Controls.TabIndex = 3
        '
        'GroupBox_NewEntry
        '
        Me.GroupBox_NewEntry.Controls.Add(Me.TextBox_Host)
        Me.GroupBox_NewEntry.Controls.Add(Me.Label5)
        Me.GroupBox_NewEntry.Controls.Add(Me.ComboBox_Protocol)
        Me.GroupBox_NewEntry.Controls.Add(Me.Label4)
        Me.GroupBox_NewEntry.Controls.Add(Me.Button_SearchPath)
        Me.GroupBox_NewEntry.Controls.Add(Me.TextBox_DestinationPath)
        Me.GroupBox_NewEntry.Controls.Add(Me.Label2)
        Me.GroupBox_NewEntry.Controls.Add(Me.ComboBox_DatabaseEntry)
        Me.GroupBox_NewEntry.Controls.Add(Me.Label1)
        Me.GroupBox_NewEntry.Controls.Add(Me.Button_AddEntry)
        Me.GroupBox_NewEntry.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox_NewEntry.Location = New System.Drawing.Point(12, 331)
        Me.GroupBox_NewEntry.Margin = New System.Windows.Forms.Padding(12, 8, 12, 8)
        Me.GroupBox_NewEntry.Name = "GroupBox_NewEntry"
        Me.GroupBox_NewEntry.Size = New System.Drawing.Size(557, 165)
        Me.GroupBox_NewEntry.TabIndex = 0
        Me.GroupBox_NewEntry.TabStop = False
        Me.GroupBox_NewEntry.Text = "Add new entry"
        '
        'TextBox_Host
        '
        Me.TextBox_Host.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_Host.Location = New System.Drawing.Point(125, 75)
        Me.TextBox_Host.Name = "TextBox_Host"
        Me.TextBox_Host.Size = New System.Drawing.Size(426, 20)
        Me.TextBox_Host.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 78)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(32, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Host:"
        '
        'ComboBox_Protocol
        '
        Me.ComboBox_Protocol.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_Protocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Protocol.FormattingEnabled = True
        Me.ComboBox_Protocol.Items.AddRange(New Object() {"FTP (Not recommended - Unsecure)", "SFTP"})
        Me.ComboBox_Protocol.Location = New System.Drawing.Point(125, 48)
        Me.ComboBox_Protocol.Name = "ComboBox_Protocol"
        Me.ComboBox_Protocol.Size = New System.Drawing.Size(426, 21)
        Me.ComboBox_Protocol.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 51)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(49, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Protocol:"
        '
        'Button_SearchPath
        '
        Me.Button_SearchPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_SearchPath.Location = New System.Drawing.Point(519, 103)
        Me.Button_SearchPath.Name = "Button_SearchPath"
        Me.Button_SearchPath.Size = New System.Drawing.Size(32, 23)
        Me.Button_SearchPath.TabIndex = 5
        Me.Button_SearchPath.Text = "..."
        Me.Button_SearchPath.UseVisualStyleBackColor = True
        '
        'TextBox_DestinationPath
        '
        Me.TextBox_DestinationPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_DestinationPath.Location = New System.Drawing.Point(125, 103)
        Me.TextBox_DestinationPath.Name = "TextBox_DestinationPath"
        Me.TextBox_DestinationPath.Size = New System.Drawing.Size(388, 20)
        Me.TextBox_DestinationPath.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 106)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Destination path:"
        '
        'ComboBox_DatabaseEntry
        '
        Me.ComboBox_DatabaseEntry.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_DatabaseEntry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_DatabaseEntry.FormattingEnabled = True
        Me.ComboBox_DatabaseEntry.Location = New System.Drawing.Point(125, 21)
        Me.ComboBox_DatabaseEntry.Name = "ComboBox_DatabaseEntry"
        Me.ComboBox_DatabaseEntry.Size = New System.Drawing.Size(426, 21)
        Me.ComboBox_DatabaseEntry.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Database entry:"
        '
        'Button_AddEntry
        '
        Me.Button_AddEntry.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_AddEntry.Location = New System.Drawing.Point(476, 132)
        Me.Button_AddEntry.Name = "Button_AddEntry"
        Me.Button_AddEntry.Size = New System.Drawing.Size(75, 23)
        Me.Button_AddEntry.TabIndex = 0
        Me.Button_AddEntry.Text = "Add"
        Me.Button_AddEntry.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.LinkLabel_RemoveItem)
        Me.Panel2.Controls.Add(Me.CheckBox_MoreDetails)
        Me.Panel2.Controls.Add(Me.ListView_FtpEntries)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(581, 323)
        Me.Panel2.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 6)
        Me.Label3.Margin = New System.Windows.Forms.Padding(12, 6, 3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Selected a entry:"
        '
        'LinkLabel_RemoveItem
        '
        Me.LinkLabel_RemoveItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel_RemoveItem.AutoSize = True
        Me.LinkLabel_RemoveItem.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel_RemoveItem.Location = New System.Drawing.Point(522, 304)
        Me.LinkLabel_RemoveItem.Name = "LinkLabel_RemoveItem"
        Me.LinkLabel_RemoveItem.Size = New System.Drawing.Size(47, 13)
        Me.LinkLabel_RemoveItem.TabIndex = 3
        Me.LinkLabel_RemoveItem.TabStop = True
        Me.LinkLabel_RemoveItem.Text = "Remove"
        '
        'CheckBox_MoreDetails
        '
        Me.CheckBox_MoreDetails.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CheckBox_MoreDetails.AutoSize = True
        Me.CheckBox_MoreDetails.Location = New System.Drawing.Point(12, 303)
        Me.CheckBox_MoreDetails.Name = "CheckBox_MoreDetails"
        Me.CheckBox_MoreDetails.Size = New System.Drawing.Size(59, 17)
        Me.CheckBox_MoreDetails.TabIndex = 2
        Me.CheckBox_MoreDetails.Text = "More..."
        Me.CheckBox_MoreDetails.UseVisualStyleBackColor = True
        '
        'ListView_FtpEntries
        '
        Me.ListView_FtpEntries.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_FtpEntries.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader4, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.ListView_FtpEntries.FullRowSelect = True
        Me.ListView_FtpEntries.HideSelection = False
        Me.ListView_FtpEntries.Location = New System.Drawing.Point(12, 22)
        Me.ListView_FtpEntries.Margin = New System.Windows.Forms.Padding(12, 3, 12, 3)
        Me.ListView_FtpEntries.Name = "ListView_FtpEntries"
        Me.ListView_FtpEntries.Size = New System.Drawing.Size(557, 275)
        Me.ListView_FtpEntries.TabIndex = 1
        Me.ListView_FtpEntries.UseCompatibleStateImageBehavior = False
        Me.ListView_FtpEntries.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Database Entry"
        Me.ColumnHeader1.Width = 97
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Host"
        Me.ColumnHeader4.Width = 101
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Destination Path"
        Me.ColumnHeader2.Width = 155
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Protocol"
        Me.ColumnHeader3.Width = 56
        '
        'UCFtpPathDatabase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.TableLayoutPanel_Controls)
        Me.Name = "UCFtpPathDatabase"
        Me.Size = New System.Drawing.Size(581, 504)
        Me.TableLayoutPanel_Controls.ResumeLayout(False)
        Me.GroupBox_NewEntry.ResumeLayout(False)
        Me.GroupBox_NewEntry.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel_Controls As TableLayoutPanel
    Friend WithEvents GroupBox_NewEntry As GroupBox
    Friend WithEvents TextBox_Host As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents ComboBox_Protocol As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Button_SearchPath As Button
    Friend WithEvents TextBox_DestinationPath As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents ComboBox_DatabaseEntry As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Button_AddEntry As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents LinkLabel_RemoveItem As LinkLabel
    Friend WithEvents CheckBox_MoreDetails As CheckBox
    Friend WithEvents ListView_FtpEntries As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents ColumnHeader3 As ColumnHeader
End Class
