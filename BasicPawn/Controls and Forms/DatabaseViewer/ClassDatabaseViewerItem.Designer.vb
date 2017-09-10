<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ClassDatabaseViewerItem
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ClassDatabaseViewerItem))
        Me.Button_Remove = New BasicPawn.ClassButtonSmallDelete()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label_Name = New System.Windows.Forms.Label()
        Me.ClassPictureBoxQuality1 = New BasicPawn.ClassPictureBoxQuality()
        Me.Label_Username = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.Button_Remove, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.ClassPictureBoxQuality1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_Remove
        '
        Me.Button_Remove.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Button_Remove.BackColor = System.Drawing.Color.Transparent
        Me.Button_Remove.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button_Remove.Image = CType(resources.GetObject("Button_Remove.Image"), System.Drawing.Image)
        Me.Button_Remove.Location = New System.Drawing.Point(628, 7)
        Me.Button_Remove.Margin = New System.Windows.Forms.Padding(6, 3, 6, 3)
        Me.Button_Remove.MaximumSize = New System.Drawing.Size(16, 16)
        Me.Button_Remove.MinimumSize = New System.Drawing.Size(16, 16)
        Me.Button_Remove.Name = "Button_Remove"
        Me.Button_Remove.Size = New System.Drawing.Size(16, 16)
        Me.Button_Remove.TabIndex = 0
        Me.Button_Remove.TabStop = False
        Me.ToolTip1.SetToolTip(Me.Button_Remove, "Remove database entry")
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.Label_Name, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Button_Remove, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ClassPictureBoxQuality1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label_Username, 2, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(650, 31)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'Label_Name
        '
        Me.Label_Name.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_Name.AutoSize = True
        Me.Label_Name.Location = New System.Drawing.Point(37, 9)
        Me.Label_Name.Name = "Label_Name"
        Me.Label_Name.Size = New System.Drawing.Size(288, 13)
        Me.Label_Name.TabIndex = 3
        Me.Label_Name.Text = "Name"
        '
        'ClassPictureBoxQuality1
        '
        Me.ClassPictureBoxQuality1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ClassPictureBoxQuality1.Image = Global.BasicPawn.My.Resources.Resources.imageres_5360_16x16
        Me.ClassPictureBoxQuality1.Location = New System.Drawing.Point(9, 7)
        Me.ClassPictureBoxQuality1.m_HighQuality = True
        Me.ClassPictureBoxQuality1.Margin = New System.Windows.Forms.Padding(9, 3, 9, 3)
        Me.ClassPictureBoxQuality1.Name = "ClassPictureBoxQuality1"
        Me.ClassPictureBoxQuality1.Size = New System.Drawing.Size(16, 16)
        Me.ClassPictureBoxQuality1.TabIndex = 1
        Me.ClassPictureBoxQuality1.TabStop = False
        '
        'Label_Username
        '
        Me.Label_Username.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label_Username.AutoSize = True
        Me.Label_Username.Location = New System.Drawing.Point(331, 9)
        Me.Label_Username.Name = "Label_Username"
        Me.Label_Username.Size = New System.Drawing.Size(288, 13)
        Me.Label_Username.TabIndex = 2
        Me.Label_Username.Text = "Username"
        '
        'ClassDatabaseViewerItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "ClassDatabaseViewerItem"
        Me.Size = New System.Drawing.Size(650, 31)
        CType(Me.Button_Remove, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.ClassPictureBoxQuality1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Button_Remove As ClassButtonSmallDelete
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents ClassPictureBoxQuality1 As ClassPictureBoxQuality
    Friend WithEvents Label_Name As Label
    Friend WithEvents Label_Username As Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
