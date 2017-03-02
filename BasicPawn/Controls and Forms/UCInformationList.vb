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


Imports System.Text.RegularExpressions
Imports ICSharpCode.TextEditor

Public Class UCInformationList
    Private g_mFormMain As FormMain

    Public Sub New(f As FormMain)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormMain = f
    End Sub

    Private Sub ListBox_Information_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox_Information.SelectedIndexChanged
        If (ListBox_Information.SelectedItems.Count < 1) Then
            Return
        End If

        If (String.IsNullOrEmpty(g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File) OrElse Not IO.File.Exists(g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File)) Then
            Return
        End If

        Dim reMatch As Match = Regex.Match(ListBox_Information.SelectedItems(0).ToString, String.Format("\b{0}\b\((?<Line>[0-9]+)\)\s\:", Regex.Escape(g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File)), RegexOptions.IgnoreCase)
        If (reMatch.Success) Then
            Dim iLineNum As Integer = CInt(reMatch.Groups("Line").Value) - 1
            If (iLineNum < 0 OrElse iLineNum > g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TotalNumberOfLines - 1) Then
                Return
            End If

            g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Caret.Line = iLineNum
            g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.ClearSelection()

            Dim iLineLen As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineSegment(iLineNum).Length

            Dim iStart As New TextLocation(0, iLineNum)
            Dim iEnd As New TextLocation(iLineLen, iLineNum)

            g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(iStart, iEnd)
        End If
    End Sub

    Private Sub OpenInNotepadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenInNotepadToolStripMenuItem.Click
        Try
            Dim lInfos As New List(Of String)

            For Each sLine As String In ListBox_Information.Items
                lInfos.Add(sLine)
            Next

            Dim sTempFile As String = IO.Path.GetTempFileName
            IO.File.WriteAllLines(sTempFile, lInfos.ToArray)

            Process.Start("notepad.exe", sTempFile)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub CopyAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyAllToolStripMenuItem.Click
        Try
            Dim sbInfos As New Text.StringBuilder

            For Each sLine As String In ListBox_Information.Items
                sbInfos.AppendLine(sLine)
            Next

            My.Computer.Clipboard.SetText(sbInfos.ToString, TextDataFormat.Text)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
