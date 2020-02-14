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
        Me.TableLayoutPanel_Controls = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button_Browse = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.Button_Upload = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Button_SearchUploadFile = New System.Windows.Forms.Button()
        Me.TextBox_UploadFile = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Panel_FtpDatabase = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel_Controls.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel_Controls
        '
        Me.TableLayoutPanel_Controls.ColumnCount = 1
        Me.TableLayoutPanel_Controls.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel_Controls.Controls.Add(Me.Panel1, 0, 2)
        Me.TableLayoutPanel_Controls.Controls.Add(Me.Panel2, 0, 0)
        Me.TableLayoutPanel_Controls.Controls.Add(Me.Panel_FtpDatabase, 0, 1)
        Me.TableLayoutPanel_Controls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel_Controls.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel_Controls.Name = "TableLayoutPanel_Controls"
        Me.TableLayoutPanel_Controls.RowCount = 3
        Me.TableLayoutPanel_Controls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64.0!))
        Me.TableLayoutPanel_Controls.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
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
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(464, 64)
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
        'Panel_FtpDatabase
        '
        Me.Panel_FtpDatabase.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel_FtpDatabase.Location = New System.Drawing.Point(0, 64)
        Me.Panel_FtpDatabase.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel_FtpDatabase.Name = "Panel_FtpDatabase"
        Me.Panel_FtpDatabase.Size = New System.Drawing.Size(464, 309)
        Me.Panel_FtpDatabase.TabIndex = 4
        '
        'FormFTP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
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
        Me.TableLayoutPanel_Controls.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel_Controls As Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel1 As Windows.Forms.Panel
    Friend WithEvents Panel2 As Windows.Forms.Panel
    Friend WithEvents Button_Upload As Windows.Forms.Button
    Friend WithEvents Button_Cancel As Windows.Forms.Button
    Friend WithEvents Button_Browse As Windows.Forms.Button
    Friend WithEvents Button_SearchUploadFile As Windows.Forms.Button
    Friend WithEvents TextBox_UploadFile As Windows.Forms.TextBox
    Friend WithEvents Label6 As Windows.Forms.Label
    Friend WithEvents Panel_FtpDatabase As Windows.Forms.Panel
End Class
