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
