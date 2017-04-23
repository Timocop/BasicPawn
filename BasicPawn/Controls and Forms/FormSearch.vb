'BasicPawn
'Copyright(C) 2017 TheTimocop

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

        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)
    End Sub

    Private Structure STRUC_SEARCH_RESULTS
        Dim iLocation As Integer
        Dim iLenght As Integer
    End Structure

    Private Function GetTextEditorText() As String
        Return g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent
    End Function

    Private Function GetTextEditorCaretOffset() As Integer
        Return g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset
    End Function

    Private Sub SetTextEditorSelection(iOffset As Integer, iLenght As Integer, bCaretBeginPos As Boolean)
        If (iOffset + iLenght > g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextLength) Then
            Return
        End If

        Dim iLineLenStart As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).LineNumber
        Dim iLineLenEnd As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset + iLenght).LineNumber
        Dim iLineColumStart As Integer = iOffset - g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Offset
        Dim iLineColumEnd As Integer = iOffset + iLenght - g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset + iLenght).Offset

        Dim iTLStart As New TextLocation(iLineColumStart, iLineLenStart)
        Dim iTLEnd As New TextLocation(iLineColumEnd, iLineLenEnd)

        g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(iTLStart, iTLEnd)
        g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Caret.Position = If(bCaretBeginPos, iTLStart, iTLEnd)
    End Sub

    Private Function GetFullLineByOffset(iOffset As Integer) As String
        If (iOffset > g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextLength) Then
            Return Nothing
        End If

        Dim iLineOffset As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Offset
        Dim iLineLenght As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Length

        If (iLineOffset + iLineLenght > g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextLength) Then
            Return Nothing
        End If

        Return g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetText(iLineOffset, iLineLenght)
    End Function

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

        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub Button_Search_Click(sender As Object, e As EventArgs) Handles Button_Search.Click
        Dim iStrucArray As STRUC_SEARCH_RESULTS() = DoSearch()
        If (iStrucArray Is Nothing) Then
            Return
        End If

        ToolStripStatusLabel1.Text = String.Format("{0} Items found!", iStrucArray.Length)

        If (iStrucArray.Length < 1) Then
            Return
        End If

        Try
            Dim iOffset As Integer = GetTextEditorCaretOffset()

            If (RadioButton_DirectionUp.Checked) Then
                For i = iStrucArray.Length - 1 To 0 Step -1
                    If (iStrucArray(i).iLocation < iOffset) Then
                        SetTextEditorSelection(iStrucArray(i).iLocation, iStrucArray(i).iLenght, True)
                        Return
                    End If
                Next

                If (CheckBox_LoopSearch.Checked) Then
                    SetTextEditorSelection(iStrucArray(iStrucArray.Length - 1).iLocation, iStrucArray(iStrucArray.Length - 1).iLenght, False)

                    ToolStripStatusLabel1.Text &= " Looped!"
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
                    Return
                End If
            Else
                For i = 0 To iStrucArray.Length - 1
                    If (iStrucArray(i).iLocation >= iOffset) Then
                        SetTextEditorSelection(iStrucArray(i).iLocation, iStrucArray(i).iLenght, False)
                        Return
                    End If
                Next

                If (CheckBox_LoopSearch.Checked) Then
                    SetTextEditorSelection(iStrucArray(0).iLocation, iStrucArray(0).iLenght, False)

                    ToolStripStatusLabel1.Text &= " Looped!"
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
                    Return
                End If
            End If

            ToolStripStatusLabel1.Text &= " End reached!"
            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_Replace_Click(sender As Object, e As EventArgs) Handles Button_Replace.Click
        Dim iStrucArray As STRUC_SEARCH_RESULTS() = DoSearch()
        If (iStrucArray Is Nothing) Then
            Return
        End If

        ToolStripStatusLabel1.Text = String.Format("{0} Items found!", iStrucArray.Length)

        If (iStrucArray.Length < 1) Then
            Return
        End If

        Try
            Dim iOffset As Integer = GetTextEditorCaretOffset()

            If (RadioButton_DirectionUp.Checked) Then
                For i = iStrucArray.Length - 1 To 0 Step -1
                    If (iStrucArray(i).iLocation < iOffset - iStrucArray(i).iLenght) Then
                        g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Replace(iStrucArray(i).iLocation, iStrucArray(i).iLenght, TextBox_Replace.Text)

                        SetTextEditorSelection(iStrucArray(i).iLocation, TextBox_Replace.Text.Length, True)
                        Return
                    End If
                Next
            Else
                For i = 0 To iStrucArray.Length - 1
                    If (iStrucArray(i).iLocation >= iOffset) Then
                        g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Replace(iStrucArray(i).iLocation, iStrucArray(i).iLenght, TextBox_Replace.Text)

                        SetTextEditorSelection(iStrucArray(i).iLocation, TextBox_Replace.Text.Length, False)
                        Return
                    End If
                Next
            End If

            ToolStripStatusLabel1.Text &= " End reached!"
            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_ReplaceAll_Click(sender As Object, e As EventArgs) Handles Button_ReplaceAll.Click
        Dim iStrucArray As STRUC_SEARCH_RESULTS() = DoSearch()
        If (iStrucArray Is Nothing) Then
            Return
        End If

        ToolStripStatusLabel1.Text = String.Format("{0} Items replaced!", iStrucArray.Length)

        If (iStrucArray.Length < 1) Then
            Return
        End If

        Try
            Dim iOffset As Integer = GetTextEditorCaretOffset()

            g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.StartUndoGroup()

            For i = iStrucArray.Length - 1 To 0 Step -1
                g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Replace(iStrucArray(i).iLocation, iStrucArray(i).iLenght, TextBox_Replace.Text)
            Next

            g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.EndUndoGroup()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_ListAll_Click(sender As Object, e As EventArgs) Handles Button_ListAll.Click
        ListView_Output.Visible = True

        Dim iStrucArray As STRUC_SEARCH_RESULTS() = DoSearch()
        If (iStrucArray Is Nothing) Then
            Return
        End If

        ToolStripStatusLabel1.Text = String.Format("{0} Items found!", iStrucArray.Length)

        If (iStrucArray.Length < 1) Then
            Return
        End If

        Try
            ListView_Output.BeginUpdate()
            ListView_Output.Items.Clear()
            SetTextEditorSelection(iStrucArray(iStrucArray.Length - 1).iLocation, iStrucArray(iStrucArray.Length - 1).iLenght, False)
            For Each i In iStrucArray
                If (i.iLocation + i.iLenght > g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextLength) Then
                    Continue For
                End If

                Dim sTest As String = GetFullLineByOffset(i.iLocation)
                If (String.IsNullOrEmpty(sTest)) Then
                    Continue For
                End If

                Dim iLine As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineNumberForOffset(i.iLocation) + 1

                ListView_Output.Items.Add(New ListViewItem(New String() {CStr(iLine), sTest, CStr(i.iLocation), CStr(i.iLenght)}))
            Next

            ListView_Output.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            ListView_Output.EndUpdate()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub TextBox_Search_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox_Search.KeyUp
        Select Case (e.KeyCode)
            Case Keys.Up
                e.Handled = True

                RadioButton_DirectionUp.Checked = True
                Button_Search.PerformClick()

            Case Keys.Down
                e.Handled = True
                RadioButton_DirectionDown.Checked = True
                Button_Search.PerformClick()
        End Select
    End Sub

    Private Sub ListView_Output_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_Output.SelectedIndexChanged
        If (ListView_Output.SelectedItems.Count < 1) Then
            Return
        End If

        Try
            Dim iLocation As Integer = CInt(ListView_Output.SelectedItems(0).SubItems(2).Text)
            Dim iLenght As Integer = CInt(ListView_Output.SelectedItems(0).SubItems(3).Text)

            SetTextEditorSelection(iLocation, iLenght, False)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class