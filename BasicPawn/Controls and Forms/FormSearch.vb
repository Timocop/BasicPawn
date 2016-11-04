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

Public Class FormSearch
    Private g_mFormMain As FormMain
    Private g_sSearchText As String

    Public Sub New(f As FormMain, Optional sSearchText As String = "")

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormMain = f
        g_sSearchText = sSearchText
    End Sub

    Private Structure STRUC_SEARCH_RESULTS
        Dim iLocation As Integer
        Dim iLenght As Integer
    End Structure

    Private Function GetTextEditorText() As String
        Return g_mFormMain.TextEditorControl1.Document.TextContent
    End Function

    Private Function GetTextEditorCaretOffset() As Integer
        Return g_mFormMain.TextEditorControl1.ActiveTextAreaControl.Caret.Offset
    End Function

    Private Sub SetTextEditorSelection(iOffset As Integer, iLenght As Integer, bCaretBeginPos As Boolean)
        Dim iLineLenStart As Integer = g_mFormMain.TextEditorControl1.Document.GetLineSegmentForOffset(iOffset).LineNumber
        Dim iLineLenEnd As Integer = g_mFormMain.TextEditorControl1.Document.GetLineSegmentForOffset(iOffset + iLenght).LineNumber
        Dim iLineColumStart As Integer = iOffset - g_mFormMain.TextEditorControl1.Document.GetLineSegmentForOffset(iOffset).Offset
        Dim iLineColumEnd As Integer = iOffset + iLenght - g_mFormMain.TextEditorControl1.Document.GetLineSegmentForOffset(iOffset + iLenght).Offset

        Dim iTLStart As New TextLocation(iLineColumStart, iLineLenStart)
        Dim iTLEnd As New TextLocation(iLineColumEnd, iLineLenEnd)

        g_mFormMain.TextEditorControl1.ActiveTextAreaControl.SelectionManager.SetSelection(iTLStart, iTLEnd)
        g_mFormMain.TextEditorControl1.ActiveTextAreaControl.Caret.Position = If(bCaretBeginPos, iTLStart, iTLEnd)
    End Sub

    ''' <summary>
    ''' Does the search and gets all results.
    ''' </summary>
    ''' <returns></returns>
    Private Function DoSearch() As STRUC_SEARCH_RESULTS()
        Dim iRegEx As Regex
        Try
            Select Case (RadioButton_ModeNormal.Checked)
                Case True
                    iRegEx = New Regex(If(CheckBox_WholeWord.Checked, "\b", "") & Regex.Escape(TextBox_Search.Text) & If(CheckBox_WholeWord.Checked, "\b", ""), If(CheckBox_CaseSensitive.Checked, RegexOptions.None, RegexOptions.IgnoreCase))
                Case Else
                    iRegEx = New Regex(TextBox_Search.Text, If(CheckBox_CaseSensitive.Checked, RegexOptions.None, RegexOptions.IgnoreCase) Or If(CheckBox_Multiline.Checked, RegexOptions.Multiline, RegexOptions.None))
            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
            ToolStripStatusLabel1.Text = "Search Error!"
            Return Nothing
        End Try

        Dim lStrucList As New List(Of STRUC_SEARCH_RESULTS)

        Dim iStruc As STRUC_SEARCH_RESULTS
        For Each m As Match In iRegEx.Matches(GetTextEditorText)
            iStruc.iLocation = m.Index
            iStruc.iLenght = m.Length

            lStrucList.Add(iStruc)
        Next

        Return lStrucList.ToArray
    End Function

    Private Sub SearchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox_Search.Text = g_sSearchText
    End Sub

    Private Sub Button_Search_Click(sender As Object, e As EventArgs) Handles Button_Search.Click
        Dim iStrucArray As STRUC_SEARCH_RESULTS() = DoSearch()
        If (iStrucArray Is Nothing) Then
            Return
        End If

        ToolStripStatusLabel1.Text = iStrucArray.Length & " Items found!"

        If (iStrucArray.Length < 1) Then
            Return
        End If

        Dim iOffset As Integer = GetTextEditorCaretOffset()

        If (RadioButton_DirectionUp.Checked) Then
            For i = iStrucArray.Length - 1 To 0 Step -1
                If (iStrucArray(i).iLocation < iOffset) Then
                    SetTextEditorSelection(iStrucArray(i).iLocation, iStrucArray(i).iLenght, True)
                    Return
                End If
            Next
        Else
            For i = 0 To iStrucArray.Length - 1
                If (iStrucArray(i).iLocation >= iOffset) Then
                    SetTextEditorSelection(iStrucArray(i).iLocation, iStrucArray(i).iLenght, False)
                    Return
                End If
            Next
        End If

        ToolStripStatusLabel1.Text &= " End reached!"
        My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
    End Sub

    Private Sub Button_Replace_Click(sender As Object, e As EventArgs) Handles Button_Replace.Click
        Dim iStrucArray As STRUC_SEARCH_RESULTS() = DoSearch()
        If (iStrucArray Is Nothing) Then
            Return
        End If

        ToolStripStatusLabel1.Text = iStrucArray.Length & " Items found!"

        If (iStrucArray.Length < 1) Then
            Return
        End If

        Dim iOffset As Integer = GetTextEditorCaretOffset()

        If (RadioButton_DirectionUp.Checked) Then
            For i = iStrucArray.Length - 1 To 0 Step -1
                If (iStrucArray(i).iLocation < iOffset - iStrucArray(i).iLenght) Then
                    g_mFormMain.TextEditorControl1.Document.Replace(iStrucArray(i).iLocation, iStrucArray(i).iLenght, TextBox_Replace.Text)

                    SetTextEditorSelection(iStrucArray(i).iLocation, TextBox_Replace.Text.Length, True)
                    Return
                End If
            Next
        Else
            For i = 0 To iStrucArray.Length - 1
                If (iStrucArray(i).iLocation >= iOffset) Then
                    g_mFormMain.TextEditorControl1.Document.Replace(iStrucArray(i).iLocation, iStrucArray(i).iLenght, TextBox_Replace.Text)

                    SetTextEditorSelection(iStrucArray(i).iLocation, TextBox_Replace.Text.Length, False)
                    Return
                End If
            Next
        End If

        ToolStripStatusLabel1.Text &= " End reached!"
        My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
    End Sub

    Private Sub Button_ReplaceAll_Click(sender As Object, e As EventArgs) Handles Button_ReplaceAll.Click
        Dim iStrucArray As STRUC_SEARCH_RESULTS() = DoSearch()
        If (iStrucArray Is Nothing) Then
            Return
        End If

        ToolStripStatusLabel1.Text = iStrucArray.Length & " Items replaced!"

        If (iStrucArray.Length < 1) Then
            Return
        End If

        Dim iOffset As Integer = GetTextEditorCaretOffset()

        g_mFormMain.TextEditorControl1.Document.UndoStack.StartUndoGroup()

        For i = iStrucArray.Length - 1 To 0 Step -1
            g_mFormMain.TextEditorControl1.Document.Replace(iStrucArray(i).iLocation, iStrucArray(i).iLenght, TextBox_Replace.Text)
        Next

        g_mFormMain.TextEditorControl1.Document.UndoStack.EndUndoGroup()
    End Sub

    Private Sub TextBox_Search_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox_Search.KeyUp
        Select Case (e.KeyCode)
            Case Keys.Up
                RadioButton_DirectionUp.Checked = True
                Button_Search.PerformClick()

                e.Handled = True
            Case Keys.Down
                RadioButton_DirectionDown.Checked = True
                Button_Search.PerformClick()

                e.Handled = True
        End Select
    End Sub
End Class