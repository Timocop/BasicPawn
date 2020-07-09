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


Partial Public Class FormSettings
    Private Sub Load_General()
        CheckBox_InvertedColors.Checked = ClassSettings.g_bSettingsInvertColors
        CheckBox_AlwaysNewInstance.Checked = ClassSettings.g_bSettingsAlwaysOpenNewInstance
        CheckBox_AutoShowStartPage.Checked = ClassSettings.g_bSettingsAutoShowStartPage
        CheckBox_AutoOpenProjectFiles.Checked = ClassSettings.g_bSettingsAutoOpenProjectFiles
        CheckBox_AssociateSourcePawn.Checked = ClassSettings.g_bSettingsAssociateSourcePawn
        CheckBox_AssociateAmxMod.Checked = ClassSettings.g_bSettingsAssociateAmxModX
        CheckBox_AssociateIncludes.Checked = ClassSettings.g_bSettingsAssociateIncludes
        CheckBox_AutoHoverScroll.Checked = ClassSettings.g_bSettingsAutoHoverScroll
        NumericUpDown_ThreadUpdateRate.Value = ClassSettings.g_iSettingsThreadUpdateRate
        CheckBox_TabCloseGoToPrevious.Checked = ClassSettings.g_bSettingsTabCloseGotoPrevious
        CheckBox_AutoSaveSource.Checked = ClassSettings.g_bSettingsAutoSaveSource
        CheckBox_AutoSaveSourceTemp.Checked = ClassSettings.g_bSettingsAutoSaveSourceTemp
    End Sub

    Private Sub Apply_General()
        ClassSettings.g_bSettingsInvertColors = CheckBox_InvertedColors.Checked
        ClassSettings.g_bSettingsAlwaysOpenNewInstance = CheckBox_AlwaysNewInstance.Checked
        ClassSettings.g_bSettingsAutoShowStartPage = CheckBox_AutoShowStartPage.Checked
        ClassSettings.g_bSettingsAutoOpenProjectFiles = CheckBox_AutoOpenProjectFiles.Checked
        ClassSettings.g_bSettingsAssociateSourcePawn = CheckBox_AssociateSourcePawn.Checked
        ClassSettings.g_bSettingsAssociateAmxModX = CheckBox_AssociateAmxMod.Checked
        ClassSettings.g_bSettingsAssociateIncludes = CheckBox_AssociateIncludes.Checked
        ClassSettings.g_bSettingsAutoHoverScroll = CheckBox_AutoHoverScroll.Checked
        ClassSettings.g_iSettingsThreadUpdateRate = CInt(NumericUpDown_ThreadUpdateRate.Value)
        ClassSettings.g_bSettingsTabCloseGotoPrevious = CheckBox_TabCloseGoToPrevious.Checked
        ClassSettings.g_bSettingsAutoSaveSource = CheckBox_AutoSaveSource.Checked
        ClassSettings.g_bSettingsAutoSaveSourceTemp = CheckBox_AutoSaveSourceTemp.Checked
    End Sub

    Private Sub Button_ClearErrorLog_Click(sender As Object, e As EventArgs) Handles Button_ClearErrorLog.Click
        Try
            If (IO.File.Exists(ClassExceptionLog.g_sLogName)) Then
                IO.File.Delete(ClassExceptionLog.g_sLogName)
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        Finally
            UpdateErrorLogSize()
        End Try
    End Sub

    Private Sub Button_ViewErrorLog_Click(sender As Object, e As EventArgs) Handles Button_ViewErrorLog.Click
        Try
            If (Not IO.File.Exists(ClassExceptionLog.g_sLogName)) Then
                MessageBox.Show("Log file does not exist", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Process.Start("notepad", String.Format("""{0}""", ClassExceptionLog.g_sLogName))
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        Finally
            UpdateErrorLogSize()
        End Try
    End Sub

    Private Sub UpdateErrorLogSize()
        Try
            Dim mLogInfo As New IO.FileInfo(ClassExceptionLog.g_sLogName)

            If (Not mLogInfo.Exists) Then
                Button_ClearErrorLog.Text = "Clear log (Empty)"
                Return
            End If

            Button_ClearErrorLog.Text = String.Format("Clear log ({0})", ClassTools.ClassStrings.FormatBytes(mLogInfo.Length))
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
