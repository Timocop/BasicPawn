<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormDebuggerException
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormDebuggerException))
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label_ExceptionInfo = New System.Windows.Forms.Label()
        Me.Panel_FooterControl = New System.Windows.Forms.Panel()
        Me.Button_Continue = New System.Windows.Forms.Button()
        Me.Button_ViewLog = New System.Windows.Forms.Button()
        Me.Panel_FooterDarkControl = New System.Windows.Forms.Panel()
        Me.Button_Close = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ListView_StackTrace = New System.Windows.Forms.ListView()
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label_Date = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Class_PictureBoxQuality2 = New BasicPawn.ClassPictureBoxQuality()
        Me.Class_PanelQuality1 = New BasicPawn.ClassPanelQuality()
        Me.Label_Title = New System.Windows.Forms.Label()
        Me.Panel_FooterControl.SuspendLayout()
        CType(Me.Class_PictureBoxQuality2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Class_PanelQuality1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.Location = New System.Drawing.Point(12, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(510, 43)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "The debugger caught an exception from the plugin you're currently debugging."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel1.Location = New System.Drawing.Point(12, 97)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 3, 3, 16)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(510, 1)
        Me.Panel1.TabIndex = 4
        '
        'Label_ExceptionInfo
        '
        Me.Label_ExceptionInfo.AutoSize = True
        Me.Label_ExceptionInfo.Location = New System.Drawing.Point(73, 114)
        Me.Label_ExceptionInfo.Margin = New System.Windows.Forms.Padding(64, 0, 3, 0)
        Me.Label_ExceptionInfo.Name = "Label_ExceptionInfo"
        Me.Label_ExceptionInfo.Size = New System.Drawing.Size(114, 13)
        Me.Label_ExceptionInfo.TabIndex = 5
        Me.Label_ExceptionInfo.Text = "Exception: Unknown"
        '
        'Panel_FooterControl
        '
        Me.Panel_FooterControl.BackColor = System.Drawing.SystemColors.Control
        Me.Panel_FooterControl.Controls.Add(Me.Button_Continue)
        Me.Panel_FooterControl.Controls.Add(Me.Button_ViewLog)
        Me.Panel_FooterControl.Controls.Add(Me.Panel_FooterDarkControl)
        Me.Panel_FooterControl.Controls.Add(Me.Button_Close)
        Me.Panel_FooterControl.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_FooterControl.Location = New System.Drawing.Point(0, 313)
        Me.Panel_FooterControl.Name = "Panel_FooterControl"
        Me.Panel_FooterControl.Size = New System.Drawing.Size(534, 48)
        Me.Panel_FooterControl.TabIndex = 6
        '
        'Button_Continue
        '
        Me.Button_Continue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Continue.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Continue.Location = New System.Drawing.Point(344, 13)
        Me.Button_Continue.Name = "Button_Continue"
        Me.Button_Continue.Size = New System.Drawing.Size(86, 23)
        Me.Button_Continue.TabIndex = 4
        Me.Button_Continue.Text = "Continue"
        Me.Button_Continue.UseVisualStyleBackColor = True
        '
        'Button_ViewLog
        '
        Me.Button_ViewLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button_ViewLog.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_ViewLog.Location = New System.Drawing.Point(12, 13)
        Me.Button_ViewLog.Name = "Button_ViewLog"
        Me.Button_ViewLog.Size = New System.Drawing.Size(86, 23)
        Me.Button_ViewLog.TabIndex = 3
        Me.Button_ViewLog.Text = "View Log"
        Me.Button_ViewLog.UseVisualStyleBackColor = True
        '
        'Panel_FooterDarkControl
        '
        Me.Panel_FooterDarkControl.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel_FooterDarkControl.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_FooterDarkControl.Location = New System.Drawing.Point(0, 0)
        Me.Panel_FooterDarkControl.Name = "Panel_FooterDarkControl"
        Me.Panel_FooterDarkControl.Size = New System.Drawing.Size(534, 1)
        Me.Panel_FooterDarkControl.TabIndex = 2
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Close.Location = New System.Drawing.Point(436, 13)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(86, 23)
        Me.Button_Close.TabIndex = 0
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 165)
        Me.Label3.Margin = New System.Windows.Forms.Padding(64, 9, 3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(66, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Stack-Trace:"
        '
        'ListView_StackTrace
        '
        Me.ListView_StackTrace.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_StackTrace.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5, Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.ListView_StackTrace.FullRowSelect = True
        Me.ListView_StackTrace.Location = New System.Drawing.Point(12, 187)
        Me.ListView_StackTrace.Margin = New System.Windows.Forms.Padding(3, 9, 3, 3)
        Me.ListView_StackTrace.Name = "ListView_StackTrace"
        Me.ListView_StackTrace.Size = New System.Drawing.Size(510, 120)
        Me.ListView_StackTrace.TabIndex = 8
        Me.ListView_StackTrace.UseCompatibleStateImageBehavior = False
        Me.ListView_StackTrace.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "#"
        Me.ColumnHeader5.Width = 20
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Debug-Line"
        Me.ColumnHeader1.Width = 75
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Line"
        Me.ColumnHeader2.Width = 35
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
        'Label_Date
        '
        Me.Label_Date.AutoSize = True
        Me.Label_Date.Location = New System.Drawing.Point(73, 127)
        Me.Label_Date.Margin = New System.Windows.Forms.Padding(64, 0, 3, 0)
        Me.Label_Date.Name = "Label_Date"
        Me.Label_Date.Size = New System.Drawing.Size(88, 13)
        Me.Label_Date.TabIndex = 9
        Me.Label_Date.Text = "Date: Unknown"
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel4.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel4.Location = New System.Drawing.Point(12, 156)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(3, 16, 3, 16)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(510, 1)
        Me.Panel4.TabIndex = 5
        '
        'Class_PictureBoxQuality2
        '
        Me.Class_PictureBoxQuality2.BackColor = System.Drawing.Color.Transparent
        Me.Class_PictureBoxQuality2.m_HighQuality = True
        Me.Class_PictureBoxQuality2.Image = Global.BasicPawn.My.Resources.Resources.Bmp_Stop
        Me.Class_PictureBoxQuality2.Location = New System.Drawing.Point(34, 109)
        Me.Class_PictureBoxQuality2.Margin = New System.Windows.Forms.Padding(3, 12, 3, 3)
        Me.Class_PictureBoxQuality2.Name = "Class_PictureBoxQuality2"
        Me.Class_PictureBoxQuality2.Size = New System.Drawing.Size(36, 36)
        Me.Class_PictureBoxQuality2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.Class_PictureBoxQuality2.TabIndex = 1
        Me.Class_PictureBoxQuality2.TabStop = False
        '
        'Class_PanelQuality1
        '
        Me.Class_PanelQuality1.BackgroundImage = Global.BasicPawn.My.Resources.Resources.BasicPawnRedTop
        Me.Class_PanelQuality1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Class_PanelQuality1.Controls.Add(Me.Label_Title)
        Me.Class_PanelQuality1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Class_PanelQuality1.m_HighQuality = False
        Me.Class_PanelQuality1.Location = New System.Drawing.Point(0, 0)
        Me.Class_PanelQuality1.Name = "Class_PanelQuality1"
        Me.Class_PanelQuality1.Size = New System.Drawing.Size(534, 48)
        Me.Class_PanelQuality1.TabIndex = 2
        '
        'Label_Title
        '
        Me.Label_Title.BackColor = System.Drawing.Color.Transparent
        Me.Label_Title.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label_Title.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_Title.ForeColor = System.Drawing.Color.Black
        Me.Label_Title.Location = New System.Drawing.Point(0, 0)
        Me.Label_Title.Name = "Label_Title"
        Me.Label_Title.Padding = New System.Windows.Forms.Padding(12, 0, 0, 0)
        Me.Label_Title.Size = New System.Drawing.Size(534, 48)
        Me.Label_Title.TabIndex = 2
        Me.Label_Title.Text = "The debugger caught an exception!"
        Me.Label_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FormDebuggerException
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(534, 361)
        Me.Controls.Add(Me.Label_Date)
        Me.Controls.Add(Me.Class_PictureBoxQuality2)
        Me.Controls.Add(Me.ListView_StackTrace)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Panel_FooterControl)
        Me.Controls.Add(Me.Label_ExceptionInfo)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Class_PanelQuality1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(550, 400)
        Me.Name = "FormDebuggerException"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Plugin Exception"
        Me.Panel_FooterControl.ResumeLayout(False)
        CType(Me.Class_PictureBoxQuality2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Class_PanelQuality1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Class_PictureBoxQuality2 As ClassPictureBoxQuality
    Friend WithEvents Class_PanelQuality1 As ClassPanelQuality
    Friend WithEvents Label_Title As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label_ExceptionInfo As Label
    Friend WithEvents Panel_FooterControl As Panel
    Friend WithEvents Button_Close As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents ListView_StackTrace As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents Panel_FooterDarkControl As Panel
    Friend WithEvents Label_Date As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ColumnHeader5 As ColumnHeader
    Friend WithEvents Button_ViewLog As Button
    Friend WithEvents Button_Continue As Button
End Class
