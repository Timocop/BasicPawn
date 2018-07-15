<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class UCReportItem
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UCReportItem))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label_Title = New System.Windows.Forms.Label()
        Me.Label_File = New System.Windows.Forms.Label()
        Me.Label_Date = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(16, 16)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Label_Title
        '
        Me.Label_Title.AutoSize = True
        Me.Label_Title.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Title.Location = New System.Drawing.Point(25, 3)
        Me.Label_Title.Name = "Label_Title"
        Me.Label_Title.Size = New System.Drawing.Size(36, 17)
        Me.Label_Title.TabIndex = 1
        Me.Label_Title.Text = "Title"
        '
        'Label_File
        '
        Me.Label_File.AutoSize = True
        Me.Label_File.Location = New System.Drawing.Point(3, 22)
        Me.Label_File.Name = "Label_File"
        Me.Label_File.Size = New System.Drawing.Size(52, 13)
        Me.Label_File.TabIndex = 2
        Me.Label_File.Text = "Message"
        '
        'Label_Date
        '
        Me.Label_Date.Dock = System.Windows.Forms.DockStyle.Right
        Me.Label_Date.Location = New System.Drawing.Point(556, 0)
        Me.Label_Date.Name = "Label_Date"
        Me.Label_Date.Size = New System.Drawing.Size(159, 43)
        Me.Label_Date.TabIndex = 3
        Me.Label_Date.Text = "Date"
        Me.Label_Date.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'UCReportItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.Label_File)
        Me.Controls.Add(Me.Label_Title)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label_Date)
        Me.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UCReportItem"
        Me.Size = New System.Drawing.Size(715, 43)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PictureBox1 As Windows.Forms.PictureBox
    Friend WithEvents Label_Title As Windows.Forms.Label
    Friend WithEvents Label_File As Windows.Forms.Label
    Friend WithEvents Label_Date As Windows.Forms.Label
End Class
