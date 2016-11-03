'BasicPawn
'Copyright(C) 2016 TheTimocop

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

Public Class ClassExceptionLogManagement
    Private Shared g_sLogName As String = IO.Path.Combine(Application.StartupPath, "application_error.log")

    Public Shared Sub WriteToLog(ex As Exception)
        Dim SB As New Text.StringBuilder
        SB.AppendLine(String.Format("[{0}]", Now.ToString))
        SB.AppendLine(ex.ToString)

        IO.File.AppendAllText(g_sLogName, SB.ToString)
    End Sub

    Public Shared Sub WriteToLogMessageBox(ex As Exception)
        WriteToLog(ex)
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
End Class
