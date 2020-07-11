<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTranslationEditor
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormTranslationEditor))
        Me.MenuStrip_TranslationEditor = New System.Windows.Forms.MenuStrip()
        Me.ImageList_Translation = New System.Windows.Forms.ImageList(Me.components)
        Me.ContextMenuStrip_Translation = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_CopyText = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_CopyFormat = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_File = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_New = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Import = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_ImportRecent = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_Export = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Edit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_GroupAdd = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ShowMissing = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TransAdd = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TransEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_TransRemove = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_CopyLang = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_GroupRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_GroupRemove = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip_TranslationEditor.SuspendLayout()
        Me.ContextMenuStrip_Translation.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip_TranslationEditor
        '
        Me.MenuStrip_TranslationEditor.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_File, Me.ToolStripMenuItem_Edit})
        Me.MenuStrip_TranslationEditor.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip_TranslationEditor.Name = "MenuStrip_TranslationEditor"
        Me.MenuStrip_TranslationEditor.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.MenuStrip_TranslationEditor.Size = New System.Drawing.Size(624, 24)
        Me.MenuStrip_TranslationEditor.TabIndex = 0
        '
        'ImageList_Translation
        '
        Me.ImageList_Translation.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList_Translation.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList_Translation.TransparentColor = System.Drawing.Color.Transparent
        '
        'ContextMenuStrip_Translation
        '
        Me.ContextMenuStrip_Translation.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_TransAdd, Me.ToolStripMenuItem_TransEdit, Me.ToolStripMenuItem_TransRemove, Me.ToolStripSeparator4, Me.ToolStripMenuItem_CopyLang, Me.ToolStripMenuItem_CopyFormat, Me.ToolStripMenuItem_CopyText, Me.ToolStripSeparator2, Me.ToolStripMenuItem_GroupRename, Me.ToolStripMenuItem_GroupRemove})
        Me.ContextMenuStrip_Translation.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip_Translation.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_Translation.Size = New System.Drawing.Size(192, 214)
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(188, 6)
        '
        'ToolStripMenuItem_CopyText
        '
        Me.ToolStripMenuItem_CopyText.Name = "ToolStripMenuItem_CopyText"
        Me.ToolStripMenuItem_CopyText.Size = New System.Drawing.Size(191, 22)
        Me.ToolStripMenuItem_CopyText.Text = "Copy Text"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(188, 6)
        '
        'ToolStripMenuItem_CopyFormat
        '
        Me.ToolStripMenuItem_CopyFormat.Name = "ToolStripMenuItem_CopyFormat"
        Me.ToolStripMenuItem_CopyFormat.Size = New System.Drawing.Size(191, 22)
        Me.ToolStripMenuItem_CopyFormat.Text = "Copy Format"
        '
        'ToolStripMenuItem_File
        '
        Me.ToolStripMenuItem_File.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_New, Me.ToolStripMenuItem_Import, Me.ToolStripMenuItem_ImportRecent, Me.ToolStripSeparator1, Me.ToolStripMenuItem_Export})
        Me.ToolStripMenuItem_File.Image = Global.BasicPawnPluginTranslationEditor.My.Resources.Resources.imageres_5306_16x16_32
        Me.ToolStripMenuItem_File.Name = "ToolStripMenuItem_File"
        Me.ToolStripMenuItem_File.Size = New System.Drawing.Size(53, 20)
        Me.ToolStripMenuItem_File.Text = "File"
        '
        'ToolStripMenuItem_New
        '
        Me.ToolStripMenuItem_New.Image = CType(resources.GetObject("ToolStripMenuItem_New.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_New.Name = "ToolStripMenuItem_New"
        Me.ToolStripMenuItem_New.Size = New System.Drawing.Size(158, 22)
        Me.ToolStripMenuItem_New.Text = "New"
        '
        'ToolStripMenuItem_Import
        '
        Me.ToolStripMenuItem_Import.Image = CType(resources.GetObject("ToolStripMenuItem_Import.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Import.Name = "ToolStripMenuItem_Import"
        Me.ToolStripMenuItem_Import.Size = New System.Drawing.Size(158, 22)
        Me.ToolStripMenuItem_Import.Text = "Import"
        '
        'ToolStripMenuItem_ImportRecent
        '
        Me.ToolStripMenuItem_ImportRecent.Name = "ToolStripMenuItem_ImportRecent"
        Me.ToolStripMenuItem_ImportRecent.Size = New System.Drawing.Size(158, 22)
        Me.ToolStripMenuItem_ImportRecent.Text = "Import Recent..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(155, 6)
        '
        'ToolStripMenuItem_Export
        '
        Me.ToolStripMenuItem_Export.Image = Global.BasicPawnPluginTranslationEditor.My.Resources.Resources.shell32_16761_16x16_32
        Me.ToolStripMenuItem_Export.Name = "ToolStripMenuItem_Export"
        Me.ToolStripMenuItem_Export.Size = New System.Drawing.Size(158, 22)
        Me.ToolStripMenuItem_Export.Text = "Export"
        '
        'ToolStripMenuItem_Edit
        '
        Me.ToolStripMenuItem_Edit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_GroupAdd, Me.ToolStripSeparator3, Me.ToolStripMenuItem_ShowMissing})
        Me.ToolStripMenuItem_Edit.Image = CType(resources.GetObject("ToolStripMenuItem_Edit.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_Edit.Name = "ToolStripMenuItem_Edit"
        Me.ToolStripMenuItem_Edit.Size = New System.Drawing.Size(55, 20)
        Me.ToolStripMenuItem_Edit.Text = "Edit"
        '
        'ToolStripMenuItem_GroupAdd
        '
        Me.ToolStripMenuItem_GroupAdd.Image = CType(resources.GetObject("ToolStripMenuItem_GroupAdd.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_GroupAdd.Name = "ToolStripMenuItem_GroupAdd"
        Me.ToolStripMenuItem_GroupAdd.Size = New System.Drawing.Size(190, 22)
        Me.ToolStripMenuItem_GroupAdd.Text = "Add Phrase Group"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(187, 6)
        '
        'ToolStripMenuItem_ShowMissing
        '
        Me.ToolStripMenuItem_ShowMissing.Checked = True
        Me.ToolStripMenuItem_ShowMissing.CheckOnClick = True
        Me.ToolStripMenuItem_ShowMissing.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ToolStripMenuItem_ShowMissing.Name = "ToolStripMenuItem_ShowMissing"
        Me.ToolStripMenuItem_ShowMissing.Size = New System.Drawing.Size(190, 22)
        Me.ToolStripMenuItem_ShowMissing.Text = "Show Missing Phrases"
        '
        'ToolStripMenuItem_TransAdd
        '
        Me.ToolStripMenuItem_TransAdd.Image = CType(resources.GetObject("ToolStripMenuItem_TransAdd.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_TransAdd.Name = "ToolStripMenuItem_TransAdd"
        Me.ToolStripMenuItem_TransAdd.Size = New System.Drawing.Size(191, 22)
        Me.ToolStripMenuItem_TransAdd.Text = "Add"
        '
        'ToolStripMenuItem_TransEdit
        '
        Me.ToolStripMenuItem_TransEdit.Image = Global.BasicPawnPluginTranslationEditor.My.Resources.Resources.imageres_5350_16x16_32
        Me.ToolStripMenuItem_TransEdit.Name = "ToolStripMenuItem_TransEdit"
        Me.ToolStripMenuItem_TransEdit.Size = New System.Drawing.Size(191, 22)
        Me.ToolStripMenuItem_TransEdit.Text = "Edit"
        '
        'ToolStripMenuItem_TransRemove
        '
        Me.ToolStripMenuItem_TransRemove.Image = Global.BasicPawnPluginTranslationEditor.My.Resources.Resources.shell32_261_16x16_32
        Me.ToolStripMenuItem_TransRemove.Name = "ToolStripMenuItem_TransRemove"
        Me.ToolStripMenuItem_TransRemove.Size = New System.Drawing.Size(191, 22)
        Me.ToolStripMenuItem_TransRemove.Text = "Remove"
        '
        'ToolStripMenuItem_CopyLang
        '
        Me.ToolStripMenuItem_CopyLang.Image = Global.BasicPawnPluginTranslationEditor.My.Resources.Resources.imageres_5306_16x16_32
        Me.ToolStripMenuItem_CopyLang.Name = "ToolStripMenuItem_CopyLang"
        Me.ToolStripMenuItem_CopyLang.Size = New System.Drawing.Size(191, 22)
        Me.ToolStripMenuItem_CopyLang.Text = "Copy Language"
        '
        'ToolStripMenuItem_GroupRename
        '
        Me.ToolStripMenuItem_GroupRename.Image = CType(resources.GetObject("ToolStripMenuItem_GroupRename.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem_GroupRename.Name = "ToolStripMenuItem_GroupRename"
        Me.ToolStripMenuItem_GroupRename.Size = New System.Drawing.Size(191, 22)
        Me.ToolStripMenuItem_GroupRename.Text = "Rename Phrase Group"
        '
        'ToolStripMenuItem_GroupRemove
        '
        Me.ToolStripMenuItem_GroupRemove.Image = Global.BasicPawnPluginTranslationEditor.My.Resources.Resources.imageres_5337_16x16_32
        Me.ToolStripMenuItem_GroupRemove.Name = "ToolStripMenuItem_GroupRemove"
        Me.ToolStripMenuItem_GroupRemove.Size = New System.Drawing.Size(191, 22)
        Me.ToolStripMenuItem_GroupRemove.Text = "Remove Phrase Group"
        '
        'FormTranslationEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(624, 441)
        Me.Controls.Add(Me.MenuStrip_TranslationEditor)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip_TranslationEditor
        Me.MinimumSize = New System.Drawing.Size(320, 240)
        Me.Name = "FormTranslationEditor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Translation Editor"
        Me.MenuStrip_TranslationEditor.ResumeLayout(False)
        Me.MenuStrip_TranslationEditor.PerformLayout()
        Me.ContextMenuStrip_Translation.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip_TranslationEditor As Windows.Forms.MenuStrip
    Friend WithEvents ToolStripMenuItem_File As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Import As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_ImportRecent As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImageList_Translation As Windows.Forms.ImageList
    Friend WithEvents ToolStripMenuItem_Export As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_New As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ContextMenuStrip_Translation As Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_TransEdit As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_TransAdd As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_TransRemove As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_GroupRename As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_GroupRemove As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Edit As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_GroupAdd As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_ShowMissing As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_CopyLang As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_CopyFormat As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_CopyText As ToolStripMenuItem
End Class
