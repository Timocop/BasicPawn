'BasicPawn
'Copyright(C) 2021 Externet

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
        CheckBox_AlwaysLoadDefaultIncludes.Checked = ClassSettings.g_bSettingsAlwaysLoadDefaultIncludes
        CheckBox_OnScreenIntelliSense.Checked = ClassSettings.g_bSettingsEnableToolTip
        CheckBox_CommentsMethodIntelliSense.Checked = ClassSettings.g_bSettingsToolTipMethodComments
        CheckBox_CommentsAutocompleteIntelliSense.Checked = ClassSettings.g_bSettingsToolTipAutocompleteComments
        CheckBox_WindowsToolTipPopup.Checked = ClassSettings.g_bSettingsUseWindowsToolTip
        CheckBox_WindowsToolTipAnimations.Checked = ClassSettings.g_bSettingsUseWindowsToolTipAnimations
        CheckBox_WindowsToolTipNewlineMethods.Checked = ClassSettings.g_bSettingsUseWindowsToolTipNewlineMethods
        CheckBox_WindowsToolTipDisplayTop.Checked = ClassSettings.g_bSettingsUseWindowsToolTipDisplayTop
        CheckBox_FullAutcompleteMethods.Checked = ClassSettings.g_bSettingsFullMethodAutocomplete
        CheckBox_FullAutocompleteReTagging.Checked = ClassSettings.g_bSettingsFullEnumAutocomplete
        CheckBox_CaseSensitive.Checked = ClassSettings.g_bSettingsAutocompleteCaseSensitive

        Select Case (ClassSettings.g_iSettingsAutocompleteVarParseType)
            Case ClassSettings.ENUM_VAR_PARSE_TYPE.ALL
                RadioButton_VarParseAll.Checked = True
            Case ClassSettings.ENUM_VAR_PARSE_TYPE.TAB_AND_INC
                RadioButton_VarParseTabInc.Checked = True
            Case Else
                RadioButton_VarParseTab.Checked = True
        End Select

        CheckBox_VarAutocompleteShowObjectBrowser.Checked = ClassSettings.g_bSettingsObjectBrowserShowVariables
        CheckBox_SwitchTabToAutocomplete.Checked = ClassSettings.g_bSettingsSwitchTabToAutocomplete
        CheckBox_OnlyUpdateSyntaxWhenFocused.Checked = ClassSettings.g_bSettingsOnlyUpdateSyntaxWhenFocused
        CheckBox_AutoCloseBrackets.Checked = ClassSettings.g_bSettingsAutoCloseBrackets
        CheckBox_AutoCloseStrings.Checked = ClassSettings.g_bSettingsAutoCloseStrings
        CheckBox_AutoIndentBrackets.Checked = ClassSettings.g_bSettingsAutoIndentBrackets
        NumericUpDown_MaxParseThreads.Value = ClassSettings.g_iSettingsMaxParsingThreads
        NumericUpDown_MaxParseCache.Value = ClassSettings.g_iSettingsMaxParsingCache
    End Sub

    Private Sub Apply_AutocompleteIntelliSense()
        ClassSettings.g_bSettingsAlwaysLoadDefaultIncludes = CheckBox_AlwaysLoadDefaultIncludes.Checked
        ClassSettings.g_bSettingsEnableToolTip = CheckBox_OnScreenIntelliSense.Checked
        ClassSettings.g_bSettingsToolTipMethodComments = CheckBox_CommentsMethodIntelliSense.Checked
        ClassSettings.g_bSettingsToolTipAutocompleteComments = CheckBox_CommentsAutocompleteIntelliSense.Checked
        ClassSettings.g_bSettingsUseWindowsToolTip = CheckBox_WindowsToolTipPopup.Checked
        ClassSettings.g_bSettingsUseWindowsToolTipAnimations = CheckBox_WindowsToolTipAnimations.Checked
        ClassSettings.g_bSettingsUseWindowsToolTipNewlineMethods = CheckBox_WindowsToolTipNewlineMethods.Checked
        ClassSettings.g_bSettingsUseWindowsToolTipDisplayTop = CheckBox_WindowsToolTipDisplayTop.Checked
        ClassSettings.g_bSettingsFullMethodAutocomplete = CheckBox_FullAutcompleteMethods.Checked
        ClassSettings.g_bSettingsFullEnumAutocomplete = CheckBox_FullAutocompleteReTagging.Checked
        ClassSettings.g_bSettingsAutocompleteCaseSensitive = CheckBox_CaseSensitive.Checked

        Select Case (True)
            Case RadioButton_VarParseAll.Checked
                ClassSettings.g_iSettingsAutocompleteVarParseType = ClassSettings.ENUM_VAR_PARSE_TYPE.ALL
            Case RadioButton_VarParseTabInc.Checked
                ClassSettings.g_iSettingsAutocompleteVarParseType = ClassSettings.ENUM_VAR_PARSE_TYPE.TAB_AND_INC
            Case Else
                ClassSettings.g_iSettingsAutocompleteVarParseType = ClassSettings.ENUM_VAR_PARSE_TYPE.TAB
        End Select

        ClassSettings.g_bSettingsObjectBrowserShowVariables = CheckBox_VarAutocompleteShowObjectBrowser.Checked
        ClassSettings.g_bSettingsSwitchTabToAutocomplete = CheckBox_SwitchTabToAutocomplete.Checked
        ClassSettings.g_bSettingsOnlyUpdateSyntaxWhenFocused = CheckBox_OnlyUpdateSyntaxWhenFocused.Checked
        ClassSettings.g_bSettingsAutoCloseBrackets = CheckBox_AutoCloseBrackets.Checked
        ClassSettings.g_bSettingsAutoCloseStrings = CheckBox_AutoCloseStrings.Checked
        ClassSettings.g_bSettingsAutoIndentBrackets = CheckBox_AutoIndentBrackets.Checked
        ClassSettings.g_iSettingsMaxParsingThreads = CInt(NumericUpDown_MaxParseThreads.Value)
        ClassSettings.g_iSettingsMaxParsingCache = CInt(NumericUpDown_MaxParseCache.Value)
    End Sub
End Class
