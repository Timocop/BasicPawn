'BasicPawn
'Copyright(C) 2020 TheTimocop

'This program Is free software: you can redistribute it And/Or modify
'it under the terms Of the GNU General Public License As published by
'the Free Software Foundation, either version 3 Of the License, Or
'(at your option) any later version.

'This program Is distributed In the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty Of
'MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License For more details.

'You should have received a copy Of the GNU General Public License
'along with this program. If Not, see < http: //www.gnu.org/licenses/>.


Public Class FormImportWizard
    Public Sub New(sFile As String, sFiles As String())

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ClassControlStyle.SetNameFlag(Panel_Footer, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER)
        ClassControlStyle.SetNameFlag(Panel_FooterDark, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)

        TextBox_File.Text = sFile

        If (sFiles.Length > 0) Then
            RadioButton_FileSingle.Checked = False
            RadioButton_FileMulti.Checked = True
        Else
            RadioButton_FileSingle.Checked = True
            RadioButton_FileMulti.Checked = False
        End If

        Try
            ListBox_AdditionalFiles.BeginUpdate()
            ListBox_AdditionalFiles.Items.Clear()
            ListBox_AdditionalFiles.Items.AddRange(sFiles)
        Finally
            ListBox_AdditionalFiles.EndUpdate()
        End Try
    End Sub

    ReadOnly Property m_IsPacked As Boolean
        Get
            Return RadioButton_FileSingle.Checked
        End Get
    End Property

    Private Sub FormImportWizard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub LinkLabel_Help_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_Help.LinkClicked
        Dim mHelpMessage As New Text.StringBuilder
        mHelpMessage.AppendLine("As of SourceMod 1.1, there is a new preferred method of shipping translations.")
        mHelpMessage.AppendLine("By default, the main translation file should only contain English phrases.")
        mHelpMessage.AppendLine("Additional translations are made in separate files, under a folder named after the ISO language code in languages.cfg.")
        mHelpMessage.AppendLine()
        mHelpMessage.AppendLine("Choose '" & RadioButton_FileMulti.Text & "' if you want to import a translation file with additional translation files.")
        mHelpMessage.AppendLine("If the translation file contains all phrases however, choose '" & RadioButton_FileSingle.Text & "' instead.")

        MessageBox.Show(mHelpMessage.ToString, "What should i choose?", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub RadioButton_FileSingle_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_FileSingle.CheckedChanged
        If (Not RadioButton_FileSingle.Checked) Then
            Return
        End If

        Label_AdditionalFiles.Visible = False
        ListBox_AdditionalFiles.Visible = False
    End Sub

    Private Sub RadioButton_FileMulti_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_FileMulti.CheckedChanged
        If (Not RadioButton_FileMulti.Checked) Then
            Return
        End If

        Label_AdditionalFiles.Visible = True
        ListBox_AdditionalFiles.Visible = True
    End Sub
End Class