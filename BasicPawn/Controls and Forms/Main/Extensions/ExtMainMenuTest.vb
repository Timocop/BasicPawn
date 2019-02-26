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
    Private Sub ToolStripMenuItem_Test_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Test.Click
        Try
            Dim sSource As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent

            With New ClassDebuggerParser(Me)
                If (.HasDebugPlaceholder(sSource)) Then
                    .CleanupDebugPlaceholder(sSource, g_ClassTabControl.m_ActiveTab.m_Language)
                End If
            End With

            Dim sSourceFile As String = Nothing
            If (Not g_ClassTabControl.m_ActiveTab.m_IsUnsaved AndAlso Not g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                sSourceFile = g_ClassTabControl.m_ActiveTab.m_File
            End If

            Dim sOutputFile As String = ""
            g_ClassTextEditorTools.CompileSource(True, sSource, sOutputFile, Nothing, If(sSourceFile Is Nothing, Nothing, IO.Path.GetDirectoryName(sSourceFile)), Nothing, Nothing, sSourceFile)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
