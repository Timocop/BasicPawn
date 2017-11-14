<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormOpenTabFromInstances
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormOpenTabFromInstances))
        Me.Panel_FooterControl = New System.Windows.Forms.Panel()
        Me.Button_Open = New System.Windows.Forms.Button()
        Me.Button_Cancel = New System.Windows.Forms.Button()
        Me.Panel_FooterDarkControl = New System.Windows.Forms.Panel()
        Me.ListView_Instances = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CheckBox_NewInstance = New System.Windows.Forms.CheckBox()
        Me.CheckBox_CloseTabs = New System.Windows.Forms.CheckBox()
        Me.LinkLabel_Refresh = New System.Windows.Forms.LinkLabel()
        Me.Panel_FooterControl.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel_FooterControl
        '
        Me.Panel_FooterControl.BackColor = System.Drawing.SystemColors.Control
        Me.Panel_FooterControl.Controls.Add(Me.Button_Open)
        Me.Panel_FooterControl.Controls.Add(Me.Button_Cancel)
        Me.Panel_FooterControl.Controls.Add(Me.Panel_FooterDarkControl)
        Me.Panel_FooterControl.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel_FooterControl.Location = New System.Drawing.Point(0, 393)
        Me.Panel_FooterControl.Name = "Panel_FooterControl"
        Me.Panel_FooterControl.Size = New System.Drawing.Size(624, 48)
        Me.Panel_FooterControl.TabIndex = 0
        '
        'Button_Open
        '
        Me.Button_Open.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Open.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Open.Location = New System.Drawing.Point(434, 13)
        Me.Button_Open.Name = "Button_Open"
        Me.Button_Open.Size = New System.Drawing.Size(86, 23)
        Me.Button_Open.TabIndex = 2
        Me.Button_Open.Text = "Open"
        Me.Button_Open.UseVisualStyleBackColor = True
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button_Cancel.Location = New System.Drawing.Point(526, 13)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(86, 23)
        Me.Button_Cancel.TabIndex = 1
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Panel_FooterDarkControl
        '
        Me.Panel_FooterDarkControl.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel_FooterDarkControl.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel_FooterDarkControl.Location = New System.Drawing.Point(0, 0)
        Me.Panel_FooterDarkControl.Name = "Panel_FooterDarkControl"
        Me.Panel_FooterDarkControl.Size = New System.Drawing.Size(624, 1)
        Me.Panel_FooterDarkControl.TabIndex = 0
        '
        'ListView_Instances
        '
        Me.ListView_Instances.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_Instances.CheckBoxes = True
        Me.ListView_Instances.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.ListView_Instances.FullRowSelect = True
        Me.ListView_Instances.Location = New System.Drawing.Point(12, 48)
        Me.ListView_Instances.MultiSelect = False
        Me.ListView_Instances.Name = "ListView_Instances"
        Me.ListView_Instances.Size = New System.Drawing.Size(600, 293)
        Me.ListView_Instances.TabIndex = 1
        Me.ListView_Instances.UseCompatibleStateImageBehavior = False
        Me.ListView_Instances.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Tab"
        Me.ColumnHeader1.Width = 35
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Path"
        Me.ColumnHeader2.Width = 550
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(600, 36)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Select files from other running BasicPawn instances you want to open."
        '
        'CheckBox_NewInstance
        '
        Me.CheckBox_NewInstance.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CheckBox_NewInstance.AutoSize = True
        Me.CheckBox_NewInstance.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_NewInstance.Location = New System.Drawing.Point(12, 370)
        Me.CheckBox_NewInstance.Name = "CheckBox_NewInstance"
        Me.CheckBox_NewInstance.Size = New System.Drawing.Size(178, 18)
        Me.CheckBox_NewInstance.TabIndex = 3
        Me.CheckBox_NewInstance.Text = "Open files in a new instance"
        Me.CheckBox_NewInstance.UseVisualStyleBackColor = True
        '
        'CheckBox_CloseTabs
        '
        Me.CheckBox_CloseTabs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CheckBox_CloseTabs.AutoSize = True
        Me.CheckBox_CloseTabs.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.CheckBox_CloseTabs.Location = New System.Drawing.Point(12, 347)
        Me.CheckBox_CloseTabs.Name = "CheckBox_CloseTabs"
        Me.CheckBox_CloseTabs.Size = New System.Drawing.Size(220, 18)
        Me.CheckBox_CloseTabs.TabIndex = 4
        Me.CheckBox_CloseTabs.Text = "Close selected tabs in other instance"
        Me.CheckBox_CloseTabs.UseVisualStyleBackColor = True
        '
        'LinkLabel_Refresh
        '
        Me.LinkLabel_Refresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel_Refresh.AutoSize = True
        Me.LinkLabel_Refresh.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel_Refresh.Location = New System.Drawing.Point(566, 349)
        Me.LinkLabel_Refresh.Name = "LinkLabel_Refresh"
        Me.LinkLabel_Refresh.Size = New System.Drawing.Size(46, 13)
        Me.LinkLabel_Refresh.TabIndex = 5
        Me.LinkLabel_Refresh.TabStop = True
        Me.LinkLabel_Refresh.Text = "Refresh"
        '
        'FormOpenTabFromInstances
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(624, 441)
        Me.Controls.Add(Me.LinkLabel_Refresh)
        Me.Controls.Add(Me.CheckBox_CloseTabs)
        Me.Controls.Add(Me.CheckBox_NewInstance)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ListView_Instances)
        Me.Controls.Add(Me.Panel_FooterControl)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Black
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormOpenTabFromInstances"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Open tabs from Instances"
        Me.TopMost = True
        Me.Panel_FooterControl.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel_FooterControl As Panel
    Friend WithEvents Button_Cancel As Button
    Friend WithEvents Panel_FooterDarkControl As Panel
    Friend WithEvents Button_Open As Button
    Friend WithEvents ListView_Instances As ListView
    Friend WithEvents Label1 As Label
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents CheckBox_NewInstance As CheckBox
    Friend WithEvents CheckBox_CloseTabs As CheckBox
    Friend WithEvents LinkLabel_Refresh As LinkLabel
End Class
