<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormDebuggerCriticalPopup
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormDebuggerCriticalPopup))
        Me.Panel_FooterControl = New System.Windows.Forms.Panel()
        Me.Button_Continue = New System.Windows.Forms.Button()
        Me.Panel_FooterDarkControl = New System.Windows.Forms.Panel()
        Me.Button_Close = New System.Windows.Forms.Button()
        Me.Class_PanelQuality1 = New BasicPawn.ClassPanelQuality()
        Me.Label_Title = New System.Windows.Forms.Label()
        Me.TextBox_Text = New System.Windows.Forms.TextBox()
        Me.Panel_FooterControl.SuspendLayout()
        Me.Class_PanelQuality1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel_FooterControl
        '
        Me.Panel_FooterControl.BackColor = System.Drawing.SystemColors.Control
        Me.Panel_FooterControl.Controls.Add(Me.Button_Continue)
        Me.Panel_FooterControl.Controls.Add(Me.Panel_FooterDarkControl)
        Me.Panel_FooterControl.Controls.Add(Me.Button_Close)
        Me.Panel_FooterControl.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_FooterControl.Location = New System.Drawing.Point(0, 513)
        Me.Panel_FooterControl.Name = "Panel_FooterControl"
        Me.Panel_FooterControl.Size = New System.Drawing.Size(784, 48)
        Me.Panel_FooterControl.TabIndex = 6
        '
        'Button_Continue
        '
        Me.Button_Continue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Continue.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Continue.Location = New System.Drawing.Point(594, 13)
        Me.Button_Continue.Name = "Button_Continue"
        Me.Button_Continue.Size = New System.Drawing.Size(86, 23)
        Me.Button_Continue.TabIndex = 3
        Me.Button_Continue.Text = "Continue"
        Me.Button_Continue.UseVisualStyleBackColor = True
        '
        'Panel_FooterDarkControl
        '
        Me.Panel_FooterDarkControl.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel_FooterDarkControl.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_FooterDarkControl.Location = New System.Drawing.Point(0, 0)
        Me.Panel_FooterDarkControl.Name = "Panel_FooterDarkControl"
        Me.Panel_FooterDarkControl.Size = New System.Drawing.Size(784, 1)
        Me.Panel_FooterDarkControl.TabIndex = 2
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Close.Location = New System.Drawing.Point(686, 13)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(86, 23)
        Me.Button_Close.TabIndex = 0
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
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
        Me.Class_PanelQuality1.Size = New System.Drawing.Size(784, 48)
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
        Me.Label_Title.Size = New System.Drawing.Size(784, 48)
        Me.Label_Title.TabIndex = 2
        Me.Label_Title.Text = "Popup Title"
        Me.Label_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBox_Text
        '
        Me.TextBox_Text.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox_Text.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_Text.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.TextBox_Text.Location = New System.Drawing.Point(12, 54)
        Me.TextBox_Text.Multiline = True
        Me.TextBox_Text.Name = "TextBox_Text"
        Me.TextBox_Text.ReadOnly = True
        Me.TextBox_Text.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBox_Text.Size = New System.Drawing.Size(760, 453)
        Me.TextBox_Text.TabIndex = 7
        '
        'FormDebuggerCriticalPopup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.TextBox_Text)
        Me.Controls.Add(Me.Panel_FooterControl)
        Me.Controls.Add(Me.Class_PanelQuality1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(550, 400)
        Me.Name = "FormDebuggerCriticalPopup"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Title"
        Me.Panel_FooterControl.ResumeLayout(False)
        Me.Class_PanelQuality1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Class_PanelQuality1 As ClassPanelQuality
    Friend WithEvents Label_Title As Label
    Friend WithEvents Panel_FooterControl As Panel
    Friend WithEvents Button_Close As Button
    Friend WithEvents Panel_FooterDarkControl As Panel
    Friend WithEvents TextBox_Text As TextBox
    Friend WithEvents Button_Continue As Button
End Class
