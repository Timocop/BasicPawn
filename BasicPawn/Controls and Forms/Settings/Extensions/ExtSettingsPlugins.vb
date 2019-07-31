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
    Private Sub Init_Plugins()
        ImageList_Plugins.Images.Clear()
        ImageList_Plugins.Images.Add(CStr(ENUM_PLUGIN_IMAGE_STATE.ENABLED), My.Resources.netshell_1610_16x16)
        ImageList_Plugins.Images.Add(CStr(ENUM_PLUGIN_IMAGE_STATE.DISABLED), My.Resources.netshell_1608_16x16)
    End Sub

    Private Sub ToolStripMenuItem_PluginsRefresh_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_PluginsRefresh.Click
        UpdatePluginsListView()
    End Sub

    Private Sub ContextMenuStrip_Plugins_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Plugins.Opening
        ToolStripMenuItem_OpenUrl.Enabled = (ListView_Plugins.SelectedItems.Count > 0)
        ToolStripMenuItem_PluginsEnable.Enabled = (ListView_Plugins.SelectedItems.Count > 0)
        ToolStripMenuItem_PluginsDisable.Enabled = (ListView_Plugins.SelectedItems.Count > 0)
    End Sub

    Private Sub ToolStripMenuItem_OpenUrl_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OpenUrl.Click
        Try
            For Each mItem As ListViewItem In ListView_Plugins.SelectedItems
                Dim sURL As String = mItem.SubItems(5).Text

                If (String.IsNullOrEmpty(sURL) OrElse Not sURL.StartsWith("http")) Then
                    MessageBox.Show("Unable to open URL", "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                Process.Start(sURL)
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_PluginsEnable_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_PluginsEnable.Click
        Try
            For Each mItem As ListViewItem In ListView_Plugins.SelectedItems
                SetPluginState(mItem.SubItems(0).Text, True)
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try

        UpdatePluginsListView()
    End Sub

    Private Sub ToolStripMenuItem_PluginsDisable_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_PluginsDisable.Click
        Try
            For Each mItem As ListViewItem In ListView_Plugins.SelectedItems
                SetPluginState(mItem.SubItems(0).Text, False)
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try

        UpdatePluginsListView()
    End Sub

    Private Sub UpdatePluginsListView()
        Try
            Dim lListViewItems As New List(Of ListViewItem)

            For Each mPlugin In g_mFormMain.g_ClassPluginController.m_Plugins
                Try
                    Dim iPluginState As ENUM_PLUGIN_IMAGE_STATE = If(mPlugin.mPluginInterface.m_PluginEnabled, ENUM_PLUGIN_IMAGE_STATE.ENABLED, ENUM_PLUGIN_IMAGE_STATE.DISABLED)

                    If (mPlugin.mPluginInformation Is Nothing) Then
                        lListViewItems.Add(New ListViewItem(New String() {
                                                            IO.Path.GetFileName(mPlugin.sFile),
                                                            "-",
                                                            "-",
                                                            "-",
                                                            "-",
                                                            "-",
                                                            ""
                                                       }, CStr(iPluginState)))
                    Else
                        lListViewItems.Add(New ListViewItem(New String() {
                                                                    IO.Path.GetFileName(mPlugin.sFile),
                                                                    mPlugin.mPluginInformation.sName,
                                                                    mPlugin.mPluginInformation.sAuthor,
                                                                    mPlugin.mPluginInformation.sDescription,
                                                                    mPlugin.mPluginInformation.sVersion,
                                                                    mPlugin.mPluginInformation.sURL,
                                                                    ""
                                                               }, CStr(iPluginState)))
                    End If
                Catch ex As Exception
                    lListViewItems.Add(New ListViewItem(New String() {
                                                                    IO.Path.GetFileName(mPlugin.sFile),
                                                                    "-",
                                                                    "-",
                                                                    "-",
                                                                    "-",
                                                                    "-",
                                                                    ex.Message
                                                               }, CStr(ENUM_PLUGIN_IMAGE_STATE.ENABLED)))

                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            Next

            For Each mPlugin In g_mFormMain.g_ClassPluginController.m_FailPlugins
                If (mPlugin.mPluginInformation Is Nothing) Then
                    lListViewItems.Add(New ListViewItem(New String() {
                                                            IO.Path.GetFileName(mPlugin.sFile),
                                                            "-",
                                                            "-",
                                                            "-",
                                                            "-",
                                                            "-",
                                                            mPlugin.mException.Message
                                                       }, CStr(ENUM_PLUGIN_IMAGE_STATE.DISABLED)))
                Else
                    lListViewItems.Add(New ListViewItem(New String() {
                                                                    IO.Path.GetFileName(mPlugin.sFile),
                                                                    mPlugin.mPluginInformation.sName,
                                                                    mPlugin.mPluginInformation.sAuthor,
                                                                    mPlugin.mPluginInformation.sDescription,
                                                                    mPlugin.mPluginInformation.sVersion,
                                                                    mPlugin.mPluginInformation.sURL,
                                                                    mPlugin.mException.Message
                                                               }, CStr(ENUM_PLUGIN_IMAGE_STATE.DISABLED)))
                End If
            Next

            ListView_Plugins.BeginUpdate()
            ListView_Plugins.Items.Clear()
            ListView_Plugins.Items.AddRange(lListViewItems.ToArray)
            ClassTools.ClassControls.ClassListView.AutoResizeColumns(ListView_Plugins)
            ListView_Plugins.EndUpdate()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub SetPluginState(sFilename As String, bEnable As Boolean)
        Try
            For Each mPlugin In g_mFormMain.g_ClassPluginController.m_Plugins
                If (IO.Path.GetFileName(mPlugin.sFile).ToLower <> sFilename.ToLower) Then
                    Continue For
                End If

                While True
                    Dim bSuccess As Boolean = False
                    Dim sReason As String = ""

                    If (bEnable) Then
                        bSuccess = mPlugin.mPluginInterface.OnPluginEnabled(sReason)
                    Else
                        bSuccess = mPlugin.mPluginInterface.OnPluginDisabled(sReason)
                    End If

                    If (bSuccess) Then
                        g_mFormMain.g_ClassPluginController.m_PluginEnabledByConfig(mPlugin) = bEnable
                    Else
                        If (String.IsNullOrEmpty(sReason)) Then
                            sReason = "Unknown"
                        End If

                        With New Text.StringBuilder
                            .AppendFormat("Could not change plugin state of plugin '{0}' with reason:", mPlugin.sFile).AppendLine()
                            .AppendLine()
                            .AppendLine(sReason)

                            Select Case (MessageBox.Show(.ToString, "Chould not change plugin state", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
                                Case DialogResult.Retry
                                    Continue While
                            End Select
                        End With
                    End If

                    Exit While
                End While
            Next

            For Each mPlugin In g_mFormMain.g_ClassPluginController.m_FailPlugins
                If (IO.Path.GetFileName(mPlugin.sFile).ToLower <> sFilename.ToLower) Then
                    Continue For
                End If

                MessageBox.Show("Unable to change plugin state. Plugin failed to load.", "Chould not change plugin state", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit For
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub LinkLabel_MorePlugins_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_MorePlugins.LinkClicked
        Try
            Process.Start("https://github.com/Timocop/BasicPawn/tree/master/Plugin%20Releases")
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
