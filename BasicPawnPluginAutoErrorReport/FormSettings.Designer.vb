<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormSettings))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ListView_FtpEntries = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox_NewEntry = New System.Windows.Forms.GroupBox()
        Me.TextBox_Host = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ComboBox_Protocol = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Button_SearchPath = New System.Windows.Forms.Button()
        Me.TextBox_SourceModPath = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ComboBox_DatabaseEntry = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Button_AddEntry = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.LinkLabel_Remove = New System.Windows.Forms.LinkLabel()
        Me.CheckBox_MoreDetails = New System.Windows.Forms.CheckBox()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.NumericUpDown_MaxFileSize = New System.Windows.Forms.NumericUpDown()
        Me.Label_MaxFileSize = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Button_Apply = New System.Windows.Forms.Button()
        Me.Button_Close = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox_NewEntry.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.NumericUpDown_MaxFileSize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel5.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Servers && Location"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "General"
        '
        'ListView_FtpEntries
        '
        Me.ListView_FtpEntries.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_FtpEntries.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.ListView_FtpEntries.Location = New System.Drawing.Point(9, 22)
        Me.ListView_FtpEntries.Margin = New System.Windows.Forms.Padding(0)
        Me.ListView_FtpEntries.Name = "ListView_FtpEntries"
        Me.ListView_FtpEntries.Size = New System.Drawing.Size(446, 63)
        Me.ListView_FtpEntries.TabIndex = 2
        Me.ListView_FtpEntries.UseCompatibleStateImageBehavior = False
        Me.ListView_FtpEntries.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Database Entry"
        Me.ColumnHeader1.Width = 100
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Host"
        Me.ColumnHeader2.Width = 100
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "SourceMod Path"
        Me.ColumnHeader3.Width = 150
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Protocol"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox_NewEntry, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel3, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel4, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel5, 0, 3)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(464, 421)
        Me.TableLayoutPanel1.TabIndex = 3
        '
        'GroupBox_NewEntry
        '
        Me.GroupBox_NewEntry.Controls.Add(Me.TextBox_Host)
        Me.GroupBox_NewEntry.Controls.Add(Me.Label5)
        Me.GroupBox_NewEntry.Controls.Add(Me.ComboBox_Protocol)
        Me.GroupBox_NewEntry.Controls.Add(Me.Label4)
        Me.GroupBox_NewEntry.Controls.Add(Me.Button_SearchPath)
        Me.GroupBox_NewEntry.Controls.Add(Me.TextBox_SourceModPath)
        Me.GroupBox_NewEntry.Controls.Add(Me.Label3)
        Me.GroupBox_NewEntry.Controls.Add(Me.ComboBox_DatabaseEntry)
        Me.GroupBox_NewEntry.Controls.Add(Me.Label6)
        Me.GroupBox_NewEntry.Controls.Add(Me.Button_AddEntry)
        Me.GroupBox_NewEntry.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox_NewEntry.Location = New System.Drawing.Point(12, 117)
        Me.GroupBox_NewEntry.Margin = New System.Windows.Forms.Padding(12, 8, 12, 8)
        Me.GroupBox_NewEntry.Name = "GroupBox_NewEntry"
        Me.GroupBox_NewEntry.Size = New System.Drawing.Size(440, 184)
        Me.GroupBox_NewEntry.TabIndex = 0
        Me.GroupBox_NewEntry.TabStop = False
        Me.GroupBox_NewEntry.Text = "Add new entry"
        '
        'TextBox_Host
        '
        Me.TextBox_Host.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_Host.Location = New System.Drawing.Point(133, 75)
        Me.TextBox_Host.Name = "TextBox_Host"
        Me.TextBox_Host.Size = New System.Drawing.Size(301, 22)
        Me.TextBox_Host.TabIndex = 19
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 78)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "Host:"
        '
        'ComboBox_Protocol
        '
        Me.ComboBox_Protocol.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_Protocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Protocol.FormattingEnabled = True
        Me.ComboBox_Protocol.Items.AddRange(New Object() {"FTP (Not reommended - Unsecure)", "SFTP"})
        Me.ComboBox_Protocol.Location = New System.Drawing.Point(133, 48)
        Me.ComboBox_Protocol.Name = "ComboBox_Protocol"
        Me.ComboBox_Protocol.Size = New System.Drawing.Size(301, 21)
        Me.ComboBox_Protocol.TabIndex = 17
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 51)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Protocol:"
        '
        'Button_SearchPath
        '
        Me.Button_SearchPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_SearchPath.Location = New System.Drawing.Point(402, 103)
        Me.Button_SearchPath.Name = "Button_SearchPath"
        Me.Button_SearchPath.Size = New System.Drawing.Size(32, 23)
        Me.Button_SearchPath.TabIndex = 15
        Me.Button_SearchPath.Text = "..."
        Me.Button_SearchPath.UseVisualStyleBackColor = True
        '
        'TextBox_SourceModPath
        '
        Me.TextBox_SourceModPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_SourceModPath.Location = New System.Drawing.Point(133, 103)
        Me.TextBox_SourceModPath.Name = "TextBox_SourceModPath"
        Me.TextBox_SourceModPath.Size = New System.Drawing.Size(261, 22)
        Me.TextBox_SourceModPath.TabIndex = 14
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 106)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "SourceMod path:"
        '
        'ComboBox_DatabaseEntry
        '
        Me.ComboBox_DatabaseEntry.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox_DatabaseEntry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_DatabaseEntry.FormattingEnabled = True
        Me.ComboBox_DatabaseEntry.Location = New System.Drawing.Point(133, 21)
        Me.ComboBox_DatabaseEntry.Name = "ComboBox_DatabaseEntry"
        Me.ComboBox_DatabaseEntry.Size = New System.Drawing.Size(301, 21)
        Me.ComboBox_DatabaseEntry.TabIndex = 12
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(87, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Database entry:"
        '
        'Button_AddEntry
        '
        Me.Button_AddEntry.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_AddEntry.Location = New System.Drawing.Point(359, 158)
        Me.Button_AddEntry.Name = "Button_AddEntry"
        Me.Button_AddEntry.Size = New System.Drawing.Size(75, 23)
        Me.Button_AddEntry.TabIndex = 10
        Me.Button_AddEntry.Text = "Add"
        Me.Button_AddEntry.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.LinkLabel_Remove)
        Me.Panel3.Controls.Add(Me.ListView_FtpEntries)
        Me.Panel3.Controls.Add(Me.CheckBox_MoreDetails)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(464, 109)
        Me.Panel3.TabIndex = 5
        '
        'LinkLabel_Remove
        '
        Me.LinkLabel_Remove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel_Remove.AutoSize = True
        Me.LinkLabel_Remove.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel_Remove.Location = New System.Drawing.Point(405, 90)
        Me.LinkLabel_Remove.Name = "LinkLabel_Remove"
        Me.LinkLabel_Remove.Size = New System.Drawing.Size(47, 13)
        Me.LinkLabel_Remove.TabIndex = 1
        Me.LinkLabel_Remove.TabStop = True
        Me.LinkLabel_Remove.Text = "Remove"
        Me.LinkLabel_Remove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CheckBox_MoreDetails
        '
        Me.CheckBox_MoreDetails.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CheckBox_MoreDetails.AutoSize = True
        Me.CheckBox_MoreDetails.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_MoreDetails.Location = New System.Drawing.Point(12, 88)
        Me.CheckBox_MoreDetails.Name = "CheckBox_MoreDetails"
        Me.CheckBox_MoreDetails.Size = New System.Drawing.Size(68, 18)
        Me.CheckBox_MoreDetails.TabIndex = 0
        Me.CheckBox_MoreDetails.Text = "More..."
        Me.CheckBox_MoreDetails.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.NumericUpDown_MaxFileSize)
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Controls.Add(Me.Label_MaxFileSize)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 309)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(464, 64)
        Me.Panel4.TabIndex = 6
        '
        'NumericUpDown_MaxFileSize
        '
        Me.NumericUpDown_MaxFileSize.Location = New System.Drawing.Point(145, 23)
        Me.NumericUpDown_MaxFileSize.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.NumericUpDown_MaxFileSize.Name = "NumericUpDown_MaxFileSize"
        Me.NumericUpDown_MaxFileSize.Size = New System.Drawing.Size(120, 22)
        Me.NumericUpDown_MaxFileSize.TabIndex = 7
        Me.NumericUpDown_MaxFileSize.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'Label_MaxFileSize
        '
        Me.Label_MaxFileSize.AutoSize = True
        Me.Label_MaxFileSize.Location = New System.Drawing.Point(12, 25)
        Me.Label_MaxFileSize.Margin = New System.Windows.Forms.Padding(3, 12, 3, 0)
        Me.Label_MaxFileSize.Name = "Label_MaxFileSize"
        Me.Label_MaxFileSize.Size = New System.Drawing.Size(98, 13)
        Me.Label_MaxFileSize.TabIndex = 6
        Me.Label_MaxFileSize.Text = "Max file size (MB):"
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.Button_Apply)
        Me.Panel5.Controls.Add(Me.Button_Close)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel5.Location = New System.Drawing.Point(0, 373)
        Me.Panel5.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(464, 48)
        Me.Panel5.TabIndex = 7
        '
        'Button_Apply
        '
        Me.Button_Apply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Apply.Location = New System.Drawing.Point(296, 13)
        Me.Button_Apply.Name = "Button_Apply"
        Me.Button_Apply.Size = New System.Drawing.Size(75, 23)
        Me.Button_Apply.TabIndex = 5
        Me.Button_Apply.Text = "Apply"
        Me.Button_Apply.UseVisualStyleBackColor = True
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.Location = New System.Drawing.Point(377, 13)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(75, 23)
        Me.Button_Close.TabIndex = 4
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'FormSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(464, 421)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(480, 460)
        Me.Name = "FormSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Automatic Error Reporting Settings"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox_NewEntry.ResumeLayout(False)
        Me.GroupBox_NewEntry.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        CType(Me.NumericUpDown_MaxFileSize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel5.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents ListView_FtpEntries As Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As Windows.Forms.ColumnHeader
    Friend WithEvents TableLayoutPanel1 As Windows.Forms.TableLayoutPanel
    Friend WithEvents CheckBox_MoreDetails As Windows.Forms.CheckBox
    Friend WithEvents LinkLabel_Remove As Windows.Forms.LinkLabel
    Friend WithEvents GroupBox_NewEntry As Windows.Forms.GroupBox
    Friend WithEvents TextBox_Host As Windows.Forms.TextBox
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents ComboBox_Protocol As Windows.Forms.ComboBox
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents Button_SearchPath As Windows.Forms.Button
    Friend WithEvents TextBox_SourceModPath As Windows.Forms.TextBox
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents ComboBox_DatabaseEntry As Windows.Forms.ComboBox
    Friend WithEvents Label6 As Windows.Forms.Label
    Friend WithEvents Button_AddEntry As Windows.Forms.Button
    Friend WithEvents Button_Close As Windows.Forms.Button
    Friend WithEvents Button_Apply As Windows.Forms.Button
    Friend WithEvents Label_MaxFileSize As Windows.Forms.Label
    Friend WithEvents NumericUpDown_MaxFileSize As Windows.Forms.NumericUpDown
    Friend WithEvents Panel3 As Windows.Forms.Panel
    Friend WithEvents Panel4 As Windows.Forms.Panel
    Friend WithEvents Panel5 As Windows.Forms.Panel
End Class
