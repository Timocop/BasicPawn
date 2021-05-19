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


Imports System.ComponentModel

Partial Public Class FormMain
    Private Sub ContextMenuStrip_Tabs_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip_Tabs.Opening
        Dim mTab As ClassTabControl.ClassTab = g_ClassTabControl.GetTabByIdentifier(g_sTabsClipboardIdentifier)

        ToolStripMenuItem_Tabs_Insert.Enabled = (mTab IsNot Nothing)
        ToolStripMenuItem_Tabs_Insert.Text = If(mTab IsNot Nothing, String.Format("Insert '{0}'", mTab.m_Title), "Insert")

        Dim mPointTab = g_ClassTabControl.GetTabByCursorPoint()
        If (mPointTab Is Nothing) Then
            Return
        End If

        Dim iPointTabIndex = TabControl_SourceTabs.TabPages.IndexOf(mPointTab)
        If (iPointTabIndex < 0 OrElse iPointTabIndex = g_ClassTabControl.m_ActiveTabIndex) Then
            Return
        End If

        g_ClassTabControl.SelectTab(iPointTabIndex)
    End Sub

    Private Sub ToolStripMenuItem_Tabs_Close_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_Close.Click
        Try
            g_ClassTabControl.RemoveTabGotoLast(g_ClassTabControl.m_ActiveTabIndex, True)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Tabs_CloseAllButThis_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_CloseAllButThis.Click
        Try
            Dim iActiveIndex As Integer = g_ClassTabControl.m_ActiveTabIndex

            Try
                g_ClassTabControl.BeginUpdate()

                For i = g_ClassTabControl.m_TabsCount - 1 To 0 Step -1
                    If (iActiveIndex = i) Then
                        Continue For
                    End If

                    If (Not g_ClassTabControl.RemoveTab(i, True, iActiveIndex)) Then
                        Return
                    End If
                Next
            Finally
                g_ClassTabControl.EndUpdate()
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Tabs_CloseAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_CloseAll.Click
        Try
            g_ClassTabControl.RemoveAllTabs()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Tabs_Cut_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_Cut.Click
        g_sTabsClipboardIdentifier = g_ClassTabControl.m_ActiveTab.m_Identifier
    End Sub

    Private Sub ToolStripMenuItem_Tabs_Insert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_Insert.Click
        Dim iFromIndex As Integer = g_ClassTabControl.GetTabIndexByIdentifier(g_sTabsClipboardIdentifier)
        If (iFromIndex < 0) Then
            Return
        End If

        Dim iToIndex As Integer = g_ClassTabControl.m_ActiveTab.m_Index

        g_ClassTabControl.SwapTabs(iFromIndex, iToIndex)
    End Sub

    Private Sub ToolStripMenuItem_Tabs_OpenFolder_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_OpenFolder.Click
        Try
            If (g_ClassTabControl.m_ActiveTab.m_IsUnsaved OrElse g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                MessageBox.Show("Could not open current folder. Source file does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Process.Start("explorer.exe", String.Format("/select,""{0}""", g_ClassTabControl.m_ActiveTab.m_File))
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Tabs_Popout_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_Popout.Click
        Try
            If (g_ClassTabControl.m_ActiveTab.m_IsUnsaved OrElse g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                MessageBox.Show("Could not popout tab. Source file does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim sFile As String = g_ClassTabControl.m_ActiveTab.m_File
            Dim bSuccess As Boolean = False

            If (ClassSettings.g_bSettingsTabCloseGotoPrevious) Then
                bSuccess = g_ClassTabControl.m_ActiveTab.RemoveTabGotoLast(True)
            Else
                bSuccess = g_ClassTabControl.m_ActiveTab.RemoveTab(True)
            End If

            If (bSuccess) Then
                Process.Start(Application.ExecutablePath, String.Join(" ", {"-newinstance", String.Format("""{0}""", sFile)}))
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
