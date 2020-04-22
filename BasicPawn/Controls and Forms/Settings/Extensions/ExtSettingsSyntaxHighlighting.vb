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
        CheckBox_DoubleClickMark.Checked = ClassSettings.g_iSettingsDoubleClickMark
        CheckBox_AutoMark.Checked = ClassSettings.g_iSettingsAutoMark
        CheckBox_PublicAsDefineColor.Checked = ClassSettings.g_iSettingsPublicAsDefineColor
        CheckBox_HighlightScope.Checked = ClassSettings.g_iSettingsHighlightCurrentScope
    End Sub

    Private Sub Apply_SyntaxHighlighting()
        ClassSettings.g_iSettingsDoubleClickMark = CheckBox_DoubleClickMark.Checked
        ClassSettings.g_iSettingsAutoMark = CheckBox_AutoMark.Checked
        ClassSettings.g_iSettingsPublicAsDefineColor = CheckBox_PublicAsDefineColor.Checked
        ClassSettings.g_iSettingsHighlightCurrentScope = CheckBox_HighlightScope.Checked
    End Sub
End Class
