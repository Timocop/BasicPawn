<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormFTP
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormFTP))
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
        Me.ListView_FtpEntries = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TableLayoutPanel_Controls = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button_Browse = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.Button_Upload = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Button_SearchUploadFile = New System.Windows.Forms.Button()
        Me.TextBox_UploadFile = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LinkLabel_RemoveItem = New System.Windows.Forms.LinkLabel()
        Me.CheckBox_MoreDetails = New System.Windows.Forms.CheckBox()
        Me.GroupBox_NewEntry.SuspendLayout()
        Me.TableLayoutPanel_Controls.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
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
        Me.GroupBox_NewEntry.Location = New System.Drawing.Point(12, 200)
        Me.GroupBox_NewEntry.Margin = New System.Windows.Forms.Padding(12, 8, 12, 8)
        Me.GroupBox_NewEntry.Name = "GroupBox_NewEntry"
        Me.GroupBox_NewEntry.Size = New System.Drawing.Size(440, 165)
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
        Me.TextBox_Host.Size = New System.Drawing.Size(309, 22)
        Me.TextBox_Host.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 78)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Host:"
        '
        'ComboBox_Protocol
        '
        Me.ComboBox_Protocol.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_Protocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Protocol.FormattingEnabled = True
        Me.ComboBox_Protocol.Items.AddRange(New Object() {"FTP (Not reommended - Unsecure)", "SFTP"})
        Me.ComboBox_Protocol.Location = New System.Drawing.Point(125, 48)
        Me.ComboBox_Protocol.Name = "ComboBox_Protocol"
        Me.ComboBox_Protocol.Size = New System.Drawing.Size(309, 21)
        Me.ComboBox_Protocol.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 51)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Protocol:"
        '
        'Button_SearchPath
        '
        Me.Button_SearchPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_SearchPath.Location = New System.Drawing.Point(402, 103)
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
        Me.TextBox_DestinationPath.Size = New System.Drawing.Size(271, 22)
        Me.TextBox_DestinationPath.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 106)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 13)
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
        Me.ComboBox_DatabaseEntry.Size = New System.Drawing.Size(309, 21)
        Me.ComboBox_DatabaseEntry.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Database entry:"
        '
        'Button_AddEntry
        '
        Me.Button_AddEntry.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_AddEntry.Location = New System.Drawing.Point(365, 132)
        Me.Button_AddEntry.Name = "Button_AddEntry"
        Me.Button_AddEntry.Size = New System.Drawing.Size(75, 23)
        Me.Button_AddEntry.TabIndex = 0
        Me.Button_AddEntry.Text = "Add"
        Me.Button_AddEntry.UseVisualStyleBackColor = True
        '
        'ListView_FtpEntries
        '
        Me.ListView_FtpEntries.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_FtpEntries.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader4, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.ListView_FtpEntries.FullRowSelect = True
        Me.ListView_FtpEntries.HideSelection = False
        Me.ListView_FtpEntries.Location = New System.Drawing.Point(12, 66)
        Me.ListView_FtpEntries.Name = "ListView_FtpEntries"
        Me.ListView_FtpEntries.Size = New System.Drawing.Size(440, 100)
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
        Me.ColumnHeader2.Text = "Upload Path"
        Me.ColumnHeader2.Width = 155
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Protocol"
        Me.ColumnHeader3.Width = 56
        '
        'TableLayoutPanel_Controls
        '
        Me.TableLayoutPanel_Controls.ColumnCount = 1
        Me.TableLayoutPanel_Controls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Controls.Controls.Add(Me.GroupBox_NewEntry, 0, 1)
        Me.TableLayoutPanel_Controls.Controls.Add(Me.Panel1, 0, 2)
        Me.TableLayoutPanel_Controls.Controls.Add(Me.Panel2, 0, 0)
        Me.TableLayoutPanel_Controls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_Controls.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel_Controls.Name = "TableLayoutPanel_Controls"
        Me.TableLayoutPanel_Controls.RowCount = 3
        Me.TableLayoutPanel_Controls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Controls.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel_Controls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48.0!))
        Me.TableLayoutPanel_Controls.Size = New System.Drawing.Size(464, 421)
        Me.TableLayoutPanel_Controls.TabIndex = 2
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button_Browse)
        Me.Panel1.Controls.Add(Me.Button_Cancel)
        Me.Panel1.Controls.Add(Me.Button_Upload)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 373)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(464, 48)
        Me.Panel1.TabIndex = 2
        '
        'Button_Browse
        '
        Me.Button_Browse.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_Browse.Enabled = False
        Me.Button_Browse.Location = New System.Drawing.Point(12, 13)
        Me.Button_Browse.Name = "Button_Browse"
        Me.Button_Browse.Size = New System.Drawing.Size(75, 23)
        Me.Button_Browse.TabIndex = 2
        Me.Button_Browse.Text = "Browse"
        Me.Button_Browse.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.Location = New System.Drawing.Point(377, 13)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Button_Cancel.TabIndex = 1
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Button_Upload
        '
        Me.Button_Upload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Upload.Enabled = False
        Me.Button_Upload.Location = New System.Drawing.Point(296, 13)
        Me.Button_Upload.Name = "Button_Upload"
        Me.Button_Upload.Size = New System.Drawing.Size(75, 23)
        Me.Button_Upload.TabIndex = 0
        Me.Button_Upload.Text = "Upload"
        Me.Button_Upload.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Button_SearchUploadFile)
        Me.Panel2.Controls.Add(Me.TextBox_UploadFile)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.LinkLabel_RemoveItem)
        Me.Panel2.Controls.Add(Me.CheckBox_MoreDetails)
        Me.Panel2.Controls.Add(Me.ListView_FtpEntries)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(464, 192)
        Me.Panel2.TabIndex = 3
        '
        'Button_SearchUploadFile
        '
        Me.Button_SearchUploadFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_SearchUploadFile.Location = New System.Drawing.Point(420, 25)
        Me.Button_SearchUploadFile.Name = "Button_SearchUploadFile"
        Me.Button_SearchUploadFile.Size = New System.Drawing.Size(32, 23)
        Me.Button_SearchUploadFile.TabIndex = 10
        Me.Button_SearchUploadFile.Text = "..."
        Me.Button_SearchUploadFile.UseVisualStyleBackColor = True
        '
        'TextBox_UploadFile
        '
        Me.TextBox_UploadFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_UploadFile.Location = New System.Drawing.Point(12, 25)
        Me.TextBox_UploadFile.Name = "TextBox_UploadFile"
        Me.TextBox_UploadFile.Size = New System.Drawing.Size(402, 22)
        Me.TextBox_UploadFile.TabIndex = 6
        Me.TextBox_UploadFile.WordWrap = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 9)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(152, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Upload files (seperate by ';'):"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 50)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(91, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Selected a entry:"
        '
        'LinkLabel_RemoveItem
        '
        Me.LinkLabel_RemoveItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel_RemoveItem.AutoSize = True
        Me.LinkLabel_RemoveItem.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel_RemoveItem.Location = New System.Drawing.Point(405, 173)
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
        Me.CheckBox_MoreDetails.Location = New System.Drawing.Point(12, 172)
        Me.CheckBox_MoreDetails.Name = "CheckBox_MoreDetails"
        Me.CheckBox_MoreDetails.Size = New System.Drawing.Size(62, 17)
        Me.CheckBox_MoreDetails.TabIndex = 2
        Me.CheckBox_MoreDetails.Text = "More..."
        Me.CheckBox_MoreDetails.UseVisualStyleBackColor = True
        '
        'FormFTP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(464, 421)
        Me.Controls.Add(Me.TableLayoutPanel_Controls)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(480, 420)
        Me.Name = "FormFTP"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Upload to FTP"
        Me.GroupBox_NewEntry.ResumeLayout(False)
        Me.GroupBox_NewEntry.PerformLayout()
        Me.TableLayoutPanel_Controls.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox_NewEntry As Windows.Forms.GroupBox
    Friend WithEvents ListView_FtpEntries As Windows.Forms.ListView
    Friend WithEvents TableLayoutPanel_Controls As Windows.Forms.TableLayoutPanel
    Friend WithEvents ColumnHeader1 As Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As Windows.Forms.ColumnHeader
    Friend WithEvents Panel1 As Windows.Forms.Panel
    Friend WithEvents Panel2 As Windows.Forms.Panel
    Friend WithEvents Button_Upload As Windows.Forms.Button
    Friend WithEvents CheckBox_MoreDetails As Windows.Forms.CheckBox
    Friend WithEvents Button_SearchPath As Windows.Forms.Button
    Friend WithEvents TextBox_DestinationPath As Windows.Forms.TextBox
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents ComboBox_DatabaseEntry As Windows.Forms.ComboBox
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents Button_AddEntry As Windows.Forms.Button
    Friend WithEvents Button_Cancel As Windows.Forms.Button
    Friend WithEvents LinkLabel_RemoveItem As Windows.Forms.LinkLabel
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents ComboBox_Protocol As Windows.Forms.ComboBox
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents ColumnHeader3 As Windows.Forms.ColumnHeader
    Friend WithEvents TextBox_Host As Windows.Forms.TextBox
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents ColumnHeader4 As Windows.Forms.ColumnHeader
    Friend WithEvents Button_Browse As Windows.Forms.Button
    Friend WithEvents Button_SearchUploadFile As Windows.Forms.Button
    Friend WithEvents TextBox_UploadFile As Windows.Forms.TextBox
    Friend WithEvents Label6 As Windows.Forms.Label
End Class
