﻿'BasicPawn
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

Public Class ClassExceptionLog
    Public Shared ReadOnly g_sLogName As String = IO.Path.Combine(Application.StartupPath, "application_error.log")

    Public Shared Sub WriteToLog(ex As Exception)
        With New Text.StringBuilder
            .AppendFormat("[{0}]", Now.ToString).AppendLine()
            .AppendLine(ex.ToString)

            IO.File.AppendAllText(g_sLogName, .ToString)
        End With
    End Sub

    Public Shared Sub WriteToLogMessageBox(ex As Exception)
        WriteToLog(ex)
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Shared Function GetDebugStackTrace(sText As String) As String
#If DEBUG Then
        Dim mStackTrace As New StackTrace(True)
        If (mStackTrace.FrameCount < 1) Then
            Return ""
        End If

        Dim sFile As String = mStackTrace.GetFrame(1).GetFileName
        Dim iLine As Integer = mStackTrace.GetFrame(1).GetFileLineNumber

        Return String.Format("{0}({1}): {2}", sFile, iLine, sText)
#Else
        Throw New ArgumentException("Only available in debug mode")
#End If
    End Function
End Class
