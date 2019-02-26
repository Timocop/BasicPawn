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
    Private Sub ToolStripMenuItem_Debug_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Debug.Click
        Try
            If (g_mFormDebugger Is Nothing OrElse g_mFormDebugger.IsDisposed) Then
                Try
                    g_mFormDebugger = New FormDebugger(Me, g_ClassTabControl.m_ActiveTab)
                    g_mFormDebugger.Show()
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)

                    If (g_mFormDebugger IsNot Nothing AndAlso Not g_mFormDebugger.IsDisposed) Then
                        g_mFormDebugger.Dispose()
                        g_mFormDebugger = Nothing
                    End If
                End Try
            Else
                If (g_mFormDebugger.WindowState = FormWindowState.Minimized) Then
                    ClassTools.ClassForms.FormWindowCommand(g_mFormDebugger, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                End If

                g_mFormDebugger.Activate()
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
