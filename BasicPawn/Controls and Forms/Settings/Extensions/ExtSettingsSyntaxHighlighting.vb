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
    Private Sub Load_SyntaxHighlighting()
        CheckBox_DoubleClickMark.Checked = ClassSettings.g_bSettingsDoubleClickMark
        CheckBox_AutoMark.Checked = ClassSettings.g_bSettingsAutoMark
        CheckBox_PublicAsDefineColor.Checked = ClassSettings.g_bSettingsPublicAsDefineColor
        CheckBox_HighlightScope.Checked = ClassSettings.g_bSettingsHighlightCurrentScope
    End Sub

    Private Sub Apply_SyntaxHighlighting()
        ClassSettings.g_bSettingsDoubleClickMark = CheckBox_DoubleClickMark.Checked
        ClassSettings.g_bSettingsAutoMark = CheckBox_AutoMark.Checked
        ClassSettings.g_bSettingsPublicAsDefineColor = CheckBox_PublicAsDefineColor.Checked
        ClassSettings.g_bSettingsHighlightCurrentScope = CheckBox_HighlightScope.Checked
    End Sub

    Private Sub FormClosing_SyntaxHighlighting()
        'Check for outdated syntax highlight files 
        If (True) Then
            Dim mCurrentVersion As New Version
            Dim mSyntaxVersion As New Version
            If (g_mFormMain.g_ClassSyntaxTools.g_ClassSyntaxHighlighting.CheckSyntaxVersion(mCurrentVersion, mSyntaxVersion)) Then
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "The custom syntax highlighting file you are using seems to be out-of-date and probably unable to load!")
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("Your version is v{0} but version v{1} is required.", mSyntaxVersion.ToString, mCurrentVersion.ToString))
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Click here to go to the GitHub syntax styles download page.",
                                                                  New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN("https://github.com/Timocop/BasicPawn/tree/master/Custom%20Syntax%20Styles"),
                                                                  False, True, True)
            End If
        End If
    End Sub
End Class
