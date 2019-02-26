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


Partial Public Class FormMain
    Private Sub OnMessageReceive(mClassMessage As ClassCrossAppComunication.ClassMessage) Handles g_ClassCrossAppCom.OnMessageReceive
        Try
            'Just in case we get a message before the controls have been created
            If (Not g_bFormPostCreate) Then
                Return
            End If

            Select Case (mClassMessage.m_MessageName)
                Case COMARG_OPEN_FILE
                    Dim iProcessId As Integer = CInt(mClassMessage.m_Messages(0))
                    Dim sFile As String = mClassMessage.m_Messages(1)

                    If (iProcessId <> Process.GetCurrentProcess.Id) Then
                        Return
                    End If

                    'Avoid infinite-loop when calling itself
                    ClassThread.ExecAsync(Me, Sub()
                                                  Try
                                                      Dim mTab = g_ClassTabControl.AddTab()
                                                      mTab.OpenFileTab(sFile)
                                                      mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)
                                                  Catch ex As Exception
                                                      ClassExceptionLog.WriteToLogMessageBox(ex)
                                                  End Try

                                                  If (Me.WindowState = FormWindowState.Minimized) Then
                                                      ClassTools.ClassForms.FormWindowCommand(Me, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                                                  End If

                                                  Me.Activate()
                                              End Sub)

                Case COMARG_REQUEST_TABS
                    Dim sCallerIdentifier As String = mClassMessage.m_Messages(0)

                    Dim iProcessId As Integer = Process.GetCurrentProcess.Id
                    Dim sProcessName As String = Process.GetCurrentProcess.ProcessName

                    Using mIni As New ClassIni
                        Dim lContents As New List(Of ClassIni.STRUC_INI_CONTENT)

                        For i = 0 To g_ClassTabControl.m_TabsCount - 1
                            Dim sTabIdentifier As String = g_ClassTabControl.m_Tab(i).m_Identifier
                            Dim iTabIndex As Integer = g_ClassTabControl.m_Tab(i).m_Index
                            Dim sTabFile As String = g_ClassTabControl.m_Tab(i).m_File

                            lContents.Add(New ClassIni.STRUC_INI_CONTENT(sTabIdentifier, "Index", CStr(iTabIndex)))
                            lContents.Add(New ClassIni.STRUC_INI_CONTENT(sTabIdentifier, "File", sTabFile))
                        Next

                        mIni.WriteKeyValue(lContents.ToArray)

                        g_ClassCrossAppCom.SendMessage(New ClassCrossAppComunication.ClassMessage(COMARG_REQUEST_TABS_CALLBACK, CStr(iProcessId), sProcessName, sCallerIdentifier, mIni.ExportToString), False)
                    End Using

                Case COMARG_REQUEST_TABS_CALLBACK
                    Dim iProcessId As Integer = CInt(mClassMessage.m_Messages(0))
                    Dim sProcessName As String = mClassMessage.m_Messages(1)
                    Dim sCallerIdentifier As String = mClassMessage.m_Messages(2)
                    Dim sIniContent As String = mClassMessage.m_Messages(3)

                    'Ignore callback if form is not open
                    If (g_mFormInstanceManager Is Nothing OrElse g_mFormInstanceManager.IsDisposed) Then
                        Return
                    End If

                    Try
                        g_mFormInstanceManager.TreeViewColumns_Instances.m_TreeView.BeginUpdate()

                        Using mIni As New ClassIni(sIniContent)
                            For Each sSection As String In mIni.GetSectionNames
                                Dim sTabIdentifier As String = sSection
                                Dim iTabIndex As Integer = CInt(mIni.ReadKeyValue(sSection, "Index"))
                                Dim sTabFile As String = mIni.ReadKeyValue(sSection, "File")

                                g_mFormInstanceManager.AddTreeViewItem(sTabIdentifier, iTabIndex, sTabFile, sProcessName, iProcessId, sCallerIdentifier)
                            Next
                        End Using
                    Finally
                        g_mFormInstanceManager.TreeViewColumns_Instances.m_TreeView.EndUpdate()
                    End Try

                Case COMARG_CLOSE_TAB
                    Dim iProcessId As Integer = CInt(mClassMessage.m_Messages(0))
                    Dim sTabIdentifier As String = mClassMessage.m_Messages(1)
                    Dim sFile As String = mClassMessage.m_Messages(2)
                    Dim bCloseAppWhenEmpty As Boolean = CBool(mClassMessage.m_Messages(3))

                    If (iProcessId <> Process.GetCurrentProcess.Id) Then
                        Return
                    End If

                    'Avoid infinite-loop when calling itself
                    ClassThread.ExecAsync(Me, Sub()
                                                  Try
                                                      Dim mTab = g_ClassTabControl.GetTabByIdentifier(sTabIdentifier)
                                                      If (mTab Is Nothing) Then
                                                          Return
                                                      End If

                                                      If (Not String.IsNullOrEmpty(sFile) AndAlso sFile <> mTab.m_File) Then
                                                          Return
                                                      End If

                                                      mTab.RemoveTab(True, g_ClassTabControl.m_ActiveTabIndex)

                                                      If (bCloseAppWhenEmpty AndAlso g_ClassTabControl.m_TabsCount = 1 AndAlso g_ClassTabControl.m_ActiveTab.m_IsUnsaved) Then
                                                          Me.Close()
                                                      End If
                                                  Catch ex As Exception
                                                      ClassExceptionLog.WriteToLogMessageBox(ex)
                                                  End Try
                                              End Sub)

                Case COMARG_CLOSE_APP
                    Dim iProcessId As Integer = CInt(mClassMessage.m_Messages(0))

                    If (iProcessId <> Process.GetCurrentProcess.Id) Then
                        Return
                    End If

                    'Avoid infinite-loop when calling itself
                    ClassThread.ExecAsync(Me, Sub()
                                                  Try
                                                      Me.Close()
                                                  Catch ex As Exception
                                                      ClassExceptionLog.WriteToLogMessageBox(ex)
                                                  End Try
                                              End Sub)

                Case COMARG_ACTIVATE_FORM_OR_TAB
                    Dim iProcessId As Integer = CInt(mClassMessage.m_Messages(0))
                    Dim sTabIdentifier As String = mClassMessage.m_Messages(1)

                    If (iProcessId <> Process.GetCurrentProcess.Id) Then
                        Return
                    End If

                    'Avoid infinite-loop when calling itself
                    ClassThread.ExecAsync(Me, Sub()
                                                  If (Not String.IsNullOrEmpty(sTabIdentifier)) Then
                                                      Dim mTab = g_ClassTabControl.GetTabByIdentifier(sTabIdentifier)
                                                      If (mTab IsNot Nothing) Then
                                                          If (Not mTab.m_IsActive) Then
                                                              mTab.SelectTab()
                                                          End If
                                                      End If
                                                  End If

                                                  If (Me.WindowState = FormWindowState.Minimized) Then
                                                      ClassTools.ClassForms.FormWindowCommand(Me, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                                                  End If

                                                  Me.Activate()

                                                  'ShowPingFlash()
                                              End Sub)
            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
