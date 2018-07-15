<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormReportDetails
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormReportDetails))
        Me.Label_ExceptionName = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label_FileName = New System.Windows.Forms.Label()
        Me.Label_Date = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ListView_StackTrace = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label_Warning = New System.Windows.Forms.Label()
        Me.PictureBox_Warning = New System.Windows.Forms.PictureBox()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Button_Close = New System.Windows.Forms.Button()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureBox_Warning, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label_ExceptionName
        '
        Me.Label_ExceptionName.AutoSize = True
        Me.Label_ExceptionName.BackColor = System.Drawing.Color.White
        Me.Label_ExceptionName.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_ExceptionName.Location = New System.Drawing.Point(62, 9)
        Me.Label_ExceptionName.Name = "Label_ExceptionName"
        Me.Label_ExceptionName.Size = New System.Drawing.Size(108, 17)
        Me.Label_ExceptionName.TabIndex = 2
        Me.Label_ExceptionName.Text = "Exception Name"
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.White
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(8, 8)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 1
        Me.PictureBox2.TabStop = False
        '
        'Label_FileName
        '
        Me.Label_FileName.AutoSize = True
        Me.Label_FileName.BackColor = System.Drawing.Color.White
        Me.Label_FileName.Location = New System.Drawing.Point(62, 26)
        Me.Label_FileName.Name = "Label_FileName"
        Me.Label_FileName.Size = New System.Drawing.Size(57, 13)
        Me.Label_FileName.TabIndex = 3
        Me.Label_FileName.Text = "File Name"
        '
        'Label_Date
        '
        Me.Label_Date.AutoSize = True
        Me.Label_Date.BackColor = System.Drawing.Color.White
        Me.Label_Date.Location = New System.Drawing.Point(62, 39)
        Me.Label_Date.Name = "Label_Date"
        Me.Label_Date.Size = New System.Drawing.Size(31, 13)
        Me.Label_Date.TabIndex = 4
        Me.Label_Date.Text = "Date"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.Controls.Add(Me.Label_Date)
        Me.Panel1.Controls.Add(Me.Label_ExceptionName)
        Me.Panel1.Controls.Add(Me.Label_FileName)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(455, 64)
        Me.Panel1.TabIndex = 5
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 63)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(455, 1)
        Me.Panel2.TabIndex = 5
        '
        'ListView_StackTrace
        '
        Me.ListView_StackTrace.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListView_StackTrace.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.ListView_StackTrace.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ListView_StackTrace.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView_StackTrace.FullRowSelect = True
        Me.ListView_StackTrace.Location = New System.Drawing.Point(0, 64)
        Me.ListView_StackTrace.MultiSelect = False
        Me.ListView_StackTrace.Name = "ListView_StackTrace"
        Me.ListView_StackTrace.Size = New System.Drawing.Size(455, 194)
        Me.ListView_StackTrace.TabIndex = 6
        Me.ListView_StackTrace.UseCompatibleStateImageBehavior = False
        Me.ListView_StackTrace.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "#"
        Me.ColumnHeader1.Width = 25
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Line"
        Me.ColumnHeader2.Width = 50
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "File"
        Me.ColumnHeader3.Width = 250
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Name"
        Me.ColumnHeader4.Width = 100
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Label_Warning)
        Me.Panel3.Controls.Add(Me.PictureBox_Warning)
        Me.Panel3.Controls.Add(Me.Panel4)
        Me.Panel3.Controls.Add(Me.Button_Close)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 258)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(455, 48)
        Me.Panel3.TabIndex = 7
        '
        'Label_Warning
        '
        Me.Label_Warning.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label_Warning.AutoSize = True
        Me.Label_Warning.Location = New System.Drawing.Point(34, 16)
        Me.Label_Warning.Name = "Label_Warning"
        Me.Label_Warning.Size = New System.Drawing.Size(52, 13)
        Me.Label_Warning.TabIndex = 3
        Me.Label_Warning.Text = "Warning"
        Me.Label_Warning.Visible = False
        '
        'PictureBox_Warning
        '
        Me.PictureBox_Warning.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox_Warning.Image = Global.BasicPawnPluginAutoErrorReport.My.Resources.Resources.user32_101_16x16_32
        Me.PictureBox_Warning.Location = New System.Drawing.Point(12, 13)
        Me.PictureBox_Warning.Name = "PictureBox_Warning"
        Me.PictureBox_Warning.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox_Warning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox_Warning.TabIndex = 2
        Me.PictureBox_Warning.TabStop = False
        Me.PictureBox_Warning.Visible = False
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(455, 1)
        Me.Panel4.TabIndex = 1
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Close.Location = New System.Drawing.Point(368, 13)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(75, 23)
        Me.Button_Close.TabIndex = 0
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'FormReportDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(455, 306)
        Me.Controls.Add(Me.ListView_StackTrace)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormReportDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Exception Name"
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.PictureBox_Warning, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox2 As Windows.Forms.PictureBox
    Friend WithEvents Label_ExceptionName As Windows.Forms.Label
    Friend WithEvents Label_FileName As Windows.Forms.Label
    Friend WithEvents Label_Date As Windows.Forms.Label
    Friend WithEvents Panel1 As Windows.Forms.Panel
    Friend WithEvents Panel2 As Windows.Forms.Panel
    Friend WithEvents ListView_StackTrace As Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As Windows.Forms.ColumnHeader
    Friend WithEvents Panel3 As Windows.Forms.Panel
    Friend WithEvents Panel4 As Windows.Forms.Panel
    Friend WithEvents Button_Close As Windows.Forms.Button
    Friend WithEvents ColumnHeader4 As Windows.Forms.ColumnHeader
    Friend WithEvents Label_Warning As Windows.Forms.Label
    Friend WithEvents PictureBox_Warning As Windows.Forms.PictureBox
End Class
