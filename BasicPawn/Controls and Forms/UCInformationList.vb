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

        Try
            Dim sSelectedItem As String = ListBox_Information.SelectedItems(0).ToString.Replace("/"c, "\"c)

            Dim bForceEnd As Boolean = False
            While True
                For i = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                    If (g_mFormMain.g_ClassTabControl.m_Tab(i).m_IsUnsaved OrElse g_mFormMain.g_ClassTabControl.m_Tab(i).m_InvalidFile) Then
                        Continue For
                    End If


                    Dim sRegexPath As String = Regex.Escape(g_mFormMain.g_ClassTabControl.m_Tab(i).m_File.Replace("/"c, "\"c))

                    Dim regMatch As Match = Regex.Match(sSelectedItem, String.Format("\b{0}\b\((?<Line>[0-9]+)\)\s\:", sRegexPath), RegexOptions.IgnoreCase)
                    If (regMatch.Success) Then
                        Dim iLineNum As Integer = CInt(regMatch.Groups("Line").Value) - 1
                        If (iLineNum < 0 OrElse iLineNum > g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.TotalNumberOfLines - 1) Then
                            Return
                        End If

                        g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.Caret.Line = iLineNum
                        g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.Caret.Column = 0
                        g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.SelectionManager.ClearSelection()

                        Dim iLineLen As Integer = g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.GetLineSegment(iLineNum).Length

                        Dim iStart As New TextLocation(0, iLineNum)
                        Dim iEnd As New TextLocation(iLineLen, iLineNum)

                        g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(iStart, iEnd)

                        If (g_mFormMain.g_ClassTabControl.m_ActiveTabIndex <> i) Then
                            g_mFormMain.g_ClassTabControl.SelectTab(i)
                        End If
                        Return
                    End If
                Next

                If (bForceEnd) Then
                    Exit While
                End If

                For Each mInclude As DictionaryEntry In g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IncludeFilesFull.ToArray
                    If (String.IsNullOrEmpty(CStr(mInclude.Value)) OrElse Not IO.File.Exists(CStr(mInclude.Value))) Then
                        Continue For
                    End If


                    Dim sRegexPath As String = Regex.Escape(CStr(mInclude.Value).Replace("/"c, "\"c))

                    Dim regMatch As Match = Regex.Match(sSelectedItem, String.Format("\b{0}\b\((?<Line>[0-9]+)\)\s\:", sRegexPath), RegexOptions.IgnoreCase)
                    If (Not regMatch.Success) Then
                        Continue For
                    End If

                    Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
                    mTab.OpenFileTab(CStr(mInclude.Value))
                    mTab.SelectTab()

                    bForceEnd = True
                    Continue While
                Next

                Exit While
            End While
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
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
