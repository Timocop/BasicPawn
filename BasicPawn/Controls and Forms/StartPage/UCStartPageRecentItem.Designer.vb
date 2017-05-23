<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCStartPageRecentItem
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UCStartPageRecentItem))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ClassButtonSmallDelete_RemoveFromRecent = New BasicPawn.ClassButtonSmallDelete()
        Me.CheckBox_Open = New System.Windows.Forms.CheckBox()
        Me.Label_DateAndFile = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.ClassButtonSmallDelete_RemoveFromRecent, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ClassButtonSmallDelete_RemoveFromRecent, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.CheckBox_Open, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label_DateAndFile, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(754, 36)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'ClassButtonSmallDelete_RemoveFromRecent
        '
        Me.ClassButtonSmallDelete_RemoveFromRecent.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ClassButtonSmallDelete_RemoveFromRecent.BackColor = System.Drawing.Color.Transparent
        Me.ClassButtonSmallDelete_RemoveFromRecent.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ClassButtonSmallDelete_RemoveFromRecent.Image = CType(resources.GetObject("ClassButtonSmallDelete_RemoveFromRecent.Image"), System.Drawing.Image)
        Me.ClassButtonSmallDelete_RemoveFromRecent.Location = New System.Drawing.Point(730, 10)
        Me.ClassButtonSmallDelete_RemoveFromRecent.MaximumSize = New System.Drawing.Size(16, 16)
        Me.ClassButtonSmallDelete_RemoveFromRecent.MinimumSize = New System.Drawing.Size(16, 16)
        Me.ClassButtonSmallDelete_RemoveFromRecent.Name = "ClassButtonSmallDelete_RemoveFromRecent"
        Me.ClassButtonSmallDelete_RemoveFromRecent.Size = New System.Drawing.Size(16, 16)
        Me.ClassButtonSmallDelete_RemoveFromRecent.TabIndex = 0
        Me.ClassButtonSmallDelete_RemoveFromRecent.TabStop = False
        Me.ToolTip1.SetToolTip(Me.ClassButtonSmallDelete_RemoveFromRecent, "Remove from recent files")
        '
        'CheckBox_Open
        '
        Me.CheckBox_Open.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.CheckBox_Open.Cursor = System.Windows.Forms.Cursors.Hand
        Me.CheckBox_Open.Location = New System.Drawing.Point(8, 11)
        Me.CheckBox_Open.Name = "CheckBox_Open"
        Me.CheckBox_Open.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox_Open.TabIndex = 1
        Me.CheckBox_Open.UseVisualStyleBackColor = True
        '
        'Label_DateAndFile
        '
        Me.Label_DateAndFile.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Label_DateAndFile.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_DateAndFile.Font = New System.Drawing.Font("Segoe UI Semibold", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_DateAndFile.Location = New System.Drawing.Point(32, 0)
        Me.Label_DateAndFile.Margin = New System.Windows.Forms.Padding(0)
        Me.Label_DateAndFile.Name = "Label_DateAndFile"
        Me.Label_DateAndFile.Size = New System.Drawing.Size(690, 36)
        Me.Label_DateAndFile.TabIndex = 2
        Me.Label_DateAndFile.Text = "File.txt" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "C:\File.txt"
        Me.Label_DateAndFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'UCStartPageRecentItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UCStartPageRecentItem"
        Me.Size = New System.Drawing.Size(754, 36)
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.ClassButtonSmallDelete_RemoveFromRecent, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents ClassButtonSmallDelete_RemoveFromRecent As ClassButtonSmallDelete
    Friend WithEvents CheckBox_Open As CheckBox
    Friend WithEvents Label_DateAndFile As Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
