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


Imports System.ComponentModel

Partial Public Class FormMain
    Private Sub ToolStripStatusLabel_CurrentConfig_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel_CurrentConfig.Click
        ContextMenuStrip_Config.Show(Cursor.Position)
    End Sub

    Private Sub ContextMenuStrip_Config_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip_Config.Opening
        ClassControlStyle.UpdateControls(ContextMenuStrip_Config)
    End Sub

    Private Sub ToolStripMenuItem_EditConfigActiveTab_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_EditConfigActiveTab.Click
        Using i As New FormSettings(Me, FormSettings.ENUM_CONFIG_TYPE.ACTIVE)
            i.TabControl1.SelectTab(i.TabPage_Configs)

            If (i.ShowDialog(Me) = DialogResult.OK) Then
                i.ApplySettings()
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_EditConfigAllTabs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_EditConfigAllTabs.Click
        Using i As New FormSettings(Me, FormSettings.ENUM_CONFIG_TYPE.ALL)
            i.TabControl1.SelectTab(i.TabPage_Configs)

            If (i.ShowDialog(Me) = DialogResult.OK) Then
                i.ApplySettings()
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_FindOptimalConfigActiveTab_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FindOptimalConfigActiveTab.Click
        Dim mTab = g_ClassTabControl.m_ActiveTab
        If (mTab.m_IsUnsaved OrElse mTab.m_InvalidFile) Then
            Return
        End If

        Dim i As ClassConfigs.ENUM_OPTIMAL_CONFIG
        Dim mConfig = ClassConfigs.FindOptimalConfigForFile(mTab.m_File, True, i)

        'Only change config if we found one.
        If (i = ClassConfigs.ENUM_OPTIMAL_CONFIG.NONE) Then
            g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_WARNING, String.Format("No optimal config found for tab '{0} ({1})'", mTab.m_Title, mTab.m_Index), False, True, True)
        Else
            mTab.m_ActiveConfig = mConfig

            g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL, "", ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS.FORCE_UPDATE)

            g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("Optimal config found for tab '{0} ({1})': {2}", mTab.m_Title, mTab.m_Index, mConfig.GetName), False, True, True)
        End If

        UpdateFormConfigText()
    End Sub

    Private Sub ToolStripMenuItem_FindOptimalConfigAllTabs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FindOptimalConfigAllTabs.Click
        For Each mTab In g_ClassTabControl.GetAllTabs
            If (mTab.m_IsUnsaved OrElse mTab.m_InvalidFile) Then
                Continue For
            End If

            Dim i As ClassConfigs.ENUM_OPTIMAL_CONFIG
            Dim mConfig = ClassConfigs.FindOptimalConfigForFile(mTab.m_File, True, i)

            'Only change config if we found one.
            If (i = ClassConfigs.ENUM_OPTIMAL_CONFIG.NONE) Then
                g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_WARNING, String.Format("No optimal config found for tab '{0} ({1})'", mTab.m_Title, mTab.m_Index), False, True, True)
            Else
                mTab.m_ActiveConfig = mConfig

                g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL, mTab, ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS.FORCE_UPDATE)

                g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("Optimal config found for tab '{0} ({1})': {2}", mTab.m_Title, mTab.m_Index, mConfig.GetName), False, True, True)
            End If
        Next

        UpdateFormConfigText()
    End Sub
End Class
