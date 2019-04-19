'BasicPawn
'Copyright(C) 2018 TheTimocop

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


Partial Public Class FormSettings
    Private Sub Load_TextEditor()
        Label_Font.Text = New FontConverter().ConvertToInvariantString(ClassSettings.g_iSettingsTextEditorFont)
        CheckBox_TabsToSpace.Checked = (ClassSettings.g_iSettingsTabsToSpaces > 0)
        NumericUpDown_TabsToSpaces.Value = If(ClassSettings.g_iSettingsTabsToSpaces > 0, ClassSettings.g_iSettingsTabsToSpaces, 4)
        TextBox_CustomSyntax.Text = ClassSettings.g_sSettingsSyntaxHighlightingPath
        CheckBox_RememberFolds.Checked = ClassSettings.g_bSettingsRememberFoldings
        CheckBox_IconBar.Checked = ClassSettings.g_bSettingsIconBar

        Select Case (ClassSettings.g_iSettingsIconLineStateType)
            Case ClassSettings.ENUM_LINE_STATE_TYPE.NONE
                RadioButton_LineStateNone.Checked = True
            Case ClassSettings.ENUM_LINE_STATE_TYPE.CHANGED_AND_SAVED
                RadioButton_LineStateChangedSaved.Checked = True
            Case Else
                RadioButton_LineStateChanged.Checked = True
        End Select

        NumericUpDown_LineStateCount.Value = ClassSettings.g_iSettingsIconLineStateMax
    End Sub

    Private Sub Apply_TextEditor()
        ClassSettings.g_iSettingsTextEditorFont = CType(New FontConverter().ConvertFromInvariantString(Label_Font.Text), Font)
        ClassSettings.g_iSettingsTabsToSpaces = CInt(If(CheckBox_TabsToSpace.Checked, NumericUpDown_TabsToSpaces.Value, 0))
        ClassSettings.g_sSettingsSyntaxHighlightingPath = TextBox_CustomSyntax.Text
        ClassSettings.g_bSettingsRememberFoldings = CheckBox_RememberFolds.Checked
        ClassSettings.g_bSettingsIconBar = CheckBox_IconBar.Checked

        Select Case (True)
            Case RadioButton_LineStateNone.Checked
                ClassSettings.g_iSettingsIconLineStateType = ClassSettings.ENUM_LINE_STATE_TYPE.NONE
            Case RadioButton_LineStateChangedSaved.Checked
                ClassSettings.g_iSettingsIconLineStateType = ClassSettings.ENUM_LINE_STATE_TYPE.CHANGED_AND_SAVED
            Case Else
                ClassSettings.g_iSettingsIconLineStateType = ClassSettings.ENUM_LINE_STATE_TYPE.CHANGED
        End Select

        ClassSettings.g_iSettingsIconLineStateMax = CInt(NumericUpDown_LineStateCount.Value)
    End Sub

    Private Sub Button_Font_Click(sender As Object, e As EventArgs) Handles Button_Font.Click
        Using i As New FontDialog()
            i.Font = CType(New FontConverter().ConvertFromInvariantString(Label_Font.Text), Font)

            If (i.ShowDialog = DialogResult.OK) Then
                Label_Font.Text = New FontConverter().ConvertToInvariantString(i.Font)
            End If
        End Using
    End Sub

    Private Sub Button_CustomSyntax_Click(sender As Object, e As EventArgs) Handles Button_CustomSyntax.Click
        Try
            Using i As New OpenFileDialog
                i.Filter = "Syntax highlighting XSHD file|*.xml"

                i.InitialDirectory = If(String.IsNullOrEmpty(TextBox_CustomSyntax.Text), "", IO.Path.GetDirectoryName(TextBox_CustomSyntax.Text))
                i.FileName = IO.Path.GetFileName(TextBox_CustomSyntax.Text)

                If (i.ShowDialog = DialogResult.OK) Then
                    TextBox_CustomSyntax.Text = i.FileName
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub LinkLabel_DefaultSyntax_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_DefaultSyntax.LinkClicked
        TextBox_CustomSyntax.Text = ""
    End Sub

    Private Sub LinkLabel_MoreStyles_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_MoreStyles.LinkClicked
        Try
            Process.Start("https://github.com/Timocop/BasicPawn/tree/master/Custom%20Syntax%20Styles")
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
