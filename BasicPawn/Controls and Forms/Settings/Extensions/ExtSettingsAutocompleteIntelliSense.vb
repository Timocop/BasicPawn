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
    Private Sub Load_AutocompleteIntelliSense()
        CheckBox_AlwaysLoadDefaultIncludes.Checked = ClassSettings.g_iSettingsAlwaysLoadDefaultIncludes
        CheckBox_OnScreenIntelliSense.Checked = ClassSettings.g_iSettingsEnableToolTip
        CheckBox_CommentsMethodIntelliSense.Checked = ClassSettings.g_iSettingsToolTipMethodComments
        CheckBox_CommentsAutocompleteIntelliSense.Checked = ClassSettings.g_iSettingsToolTipAutocompleteComments
        CheckBox_WindowsToolTipPopup.Checked = ClassSettings.g_iSettingsUseWindowsToolTip
        CheckBox_WindowsToolTipAnimations.Checked = ClassSettings.g_iSettingsUseWindowsToolTipAnimations
        CheckBox_WindowsToolTipNewlineMethods.Checked = ClassSettings.g_iSettingsUseWindowsToolTipNewlineMethods
        CheckBox_WindowsToolTipDisplayTop.Checked = ClassSettings.g_iSettingsUseWindowsToolTipDisplayTop
        CheckBox_FullAutcompleteMethods.Checked = ClassSettings.g_iSettingsFullMethodAutocomplete
        CheckBox_FullAutocompleteReTagging.Checked = ClassSettings.g_iSettingsFullEnumAutocomplete
        CheckBox_CaseSensitive.Checked = ClassSettings.g_iSettingsAutocompleteCaseSensitive

        Select Case (ClassSettings.g_iSettingsAutocompleteVarParseType)
            Case ClassSettings.ENUM_VAR_PARSE_TYPE.ALL
                RadioButton_VarParseAll.Checked = True
            Case ClassSettings.ENUM_VAR_PARSE_TYPE.TAB_AND_INC
                RadioButton_VarParseTabInc.Checked = True
            Case Else
                RadioButton_VarParseTab.Checked = True
        End Select

        CheckBox_VarAutocompleteShowObjectBrowser.Checked = ClassSettings.g_iSettingsObjectBrowserShowVariables
        CheckBox_SwitchTabToAutocomplete.Checked = ClassSettings.g_iSettingsSwitchTabToAutocomplete
        CheckBox_OnlyUpdateSyntaxWhenFocused.Checked = ClassSettings.g_iSettingsOnlyUpdateSyntaxWhenFocused
        CheckBox_AutoCloseBrackets.Checked = ClassSettings.g_iSettingsAutoCloseBrackets
        CheckBox_AutoCloseStrings.Checked = ClassSettings.g_iSettingsAutoCloseStrings
        CheckBox_AutoIndentBrackets.Checked = ClassSettings.g_iSettingsAutoIndentBrackets
        NumericUpDown_MaxParseThreads.Value = ClassSettings.g_iSettingsMaxParsingThreads
        NumericUpDown_MaxParseCache.Value = ClassSettings.g_iSettingsMaxParsingCache
    End Sub

    Private Sub Apply_AutocompleteIntelliSense()
        ClassSettings.g_iSettingsAlwaysLoadDefaultIncludes = CheckBox_AlwaysLoadDefaultIncludes.Checked
        ClassSettings.g_iSettingsEnableToolTip = CheckBox_OnScreenIntelliSense.Checked
        ClassSettings.g_iSettingsToolTipMethodComments = CheckBox_CommentsMethodIntelliSense.Checked
        ClassSettings.g_iSettingsToolTipAutocompleteComments = CheckBox_CommentsAutocompleteIntelliSense.Checked
        ClassSettings.g_iSettingsUseWindowsToolTip = CheckBox_WindowsToolTipPopup.Checked
        ClassSettings.g_iSettingsUseWindowsToolTipAnimations = CheckBox_WindowsToolTipAnimations.Checked
        ClassSettings.g_iSettingsUseWindowsToolTipNewlineMethods = CheckBox_WindowsToolTipNewlineMethods.Checked
        ClassSettings.g_iSettingsUseWindowsToolTipDisplayTop = CheckBox_WindowsToolTipDisplayTop.Checked
        ClassSettings.g_iSettingsFullMethodAutocomplete = CheckBox_FullAutcompleteMethods.Checked
        ClassSettings.g_iSettingsFullEnumAutocomplete = CheckBox_FullAutocompleteReTagging.Checked
        ClassSettings.g_iSettingsAutocompleteCaseSensitive = CheckBox_CaseSensitive.Checked

        Select Case (True)
            Case RadioButton_VarParseAll.Checked
                ClassSettings.g_iSettingsAutocompleteVarParseType = ClassSettings.ENUM_VAR_PARSE_TYPE.ALL
            Case RadioButton_VarParseTabInc.Checked
                ClassSettings.g_iSettingsAutocompleteVarParseType = ClassSettings.ENUM_VAR_PARSE_TYPE.TAB_AND_INC
            Case Else
                ClassSettings.g_iSettingsAutocompleteVarParseType = ClassSettings.ENUM_VAR_PARSE_TYPE.TAB
        End Select

        ClassSettings.g_iSettingsObjectBrowserShowVariables = CheckBox_VarAutocompleteShowObjectBrowser.Checked
        ClassSettings.g_iSettingsSwitchTabToAutocomplete = CheckBox_SwitchTabToAutocomplete.Checked
        ClassSettings.g_iSettingsOnlyUpdateSyntaxWhenFocused = CheckBox_OnlyUpdateSyntaxWhenFocused.Checked
        ClassSettings.g_iSettingsAutoCloseBrackets = CheckBox_AutoCloseBrackets.Checked
        ClassSettings.g_iSettingsAutoCloseStrings = CheckBox_AutoCloseStrings.Checked
        ClassSettings.g_iSettingsAutoIndentBrackets = CheckBox_AutoIndentBrackets.Checked
        ClassSettings.g_iSettingsMaxParsingThreads = CInt(NumericUpDown_MaxParseThreads.Value)
        ClassSettings.g_iSettingsMaxParsingCache = CInt(NumericUpDown_MaxParseCache.Value)
    End Sub
End Class
