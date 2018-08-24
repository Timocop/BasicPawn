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

Public Class FormSearch
    Private g_mFormMain As FormMain
    Private g_sSearchText As String

    Private g_mExpandedSize As Size = Me.Size
    Private g_mCollapsedSize As Size = Me.Size

    Private Structure STRUC_SEARCH_RESULTS
        Dim iLocation As Integer
        Dim iLength As Integer
        Dim sFile As String
        Dim sTabIdentifier As String
    End Structure

    Public Sub New(f As FormMain, Optional sSearchText As String = "")
        g_mFormMain = f
        g_sSearchText = sSearchText

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)
    End Sub

    Private Sub SearchForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox_Search.Text = g_sSearchText

        ClassControlStyle.UpdateControls(Me)

        'Save orginal size
        g_mExpandedSize = Me.Size

        'Auto shrink form once
        Me.AutoSize = True
        Me.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Dim tmpSize = Me.Size
        Me.AutoSize = False
        Me.AutoSizeMode = AutoSizeMode.GrowOnly
        Me.Size = tmpSize
        Me.MinimumSize = tmpSize

        'Save collapsed size
        g_mCollapsedSize = Me.Size
    End Sub

    Private Sub SetTextEditorSelection(mTab As ClassTabControl.SourceTabPage, iOffset As Integer, iLength As Integer, bCaretBeginPos As Boolean)
        If (iOffset + iLength > mTab.m_TextEditor.Document.TextLength) Then
            Return
        End If

        Dim iLineLenStart As Integer = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).LineNumber
        Dim iLineLenEnd As Integer = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset + iLength).LineNumber
        Dim iLineColumStart As Integer = iOffset - mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Offset
        Dim iLineColumEnd As Integer = iOffset + iLength - mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset + iLength).Offset

        Dim mLocStart As New TextLocation(iLineColumStart, iLineLenStart)
        Dim mLocEnd As New TextLocation(iLineColumEnd, iLineLenEnd)

        mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(mLocStart, mLocEnd)
        mTab.m_TextEditor.ActiveTextAreaControl.Caret.Position = If(bCaretBeginPos, mLocStart, mLocEnd)
    End Sub

    Private Function GetFullLineByOffset(mTab As ClassTabControl.SourceTabPage, iOffset As Integer) As String
        If (iOffset > mTab.m_TextEditor.Document.TextLength) Then
            Return Nothing
        End If

        Dim iLineOffset As Integer = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Offset
        Dim iLineLength As Integer = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Length

        If (iLineOffset + iLineLength > mTab.m_TextEditor.Document.TextLength) Then
            Return Nothing
        End If

        Return mTab.m_TextEditor.Document.GetText(iLineOffset, iLineLength)
    End Function

    ''' <summary>
    ''' Does the search and gets all results.
    ''' </summary>
    ''' <returns></returns>
    Private Function DoSearch(bAllOpenTabs As Boolean) As STRUC_SEARCH_RESULTS()
        If (String.IsNullOrEmpty(TextBox_Search.Text) OrElse TextBox_Search.Text.Trim.Length < 1) Then
            ToolStripStatusLabel_Status.Text = "Unable to search 'nothing'!"
            Return Nothing
        End If

        Dim mRegex As Regex
        Try
            Select Case (RadioButton_ModeNormal.Checked)
                Case True
                    mRegex = New Regex(If(CheckBox_WholeWord.Checked, "\b", "") & Regex.Escape(TextBox_Search.Text) & If(CheckBox_WholeWord.Checked, "\b", ""), If(CheckBox_CaseSensitive.Checked, RegexOptions.None, RegexOptions.IgnoreCase))
                Case Else
                    mRegex = New Regex(TextBox_Search.Text, If(CheckBox_CaseSensitive.Checked, RegexOptions.None, RegexOptions.IgnoreCase) Or If(CheckBox_Multiline.Checked, RegexOptions.Multiline, RegexOptions.None))
            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
            ToolStripStatusLabel_Status.Text = "Search Error!"
            Return Nothing
        End Try

        Dim lResults As New List(Of STRUC_SEARCH_RESULTS)

        If (bAllOpenTabs) Then
            For i = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                For Each mMatch As Match In mRegex.Matches(g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.TextContent)
                    lResults.Add(New STRUC_SEARCH_RESULTS With {
                        .iLocation = mMatch.Index,
                        .iLength = mMatch.Length,
                        .sFile = g_mFormMain.g_ClassTabControl.m_Tab(i).m_File,
                        .sTabIdentifier = g_mFormMain.g_ClassTabControl.m_Tab(i).m_Identifier
                    })
                Next
            Next
        Else
            For Each mMatch As Match In mRegex.Matches(g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent)
                lResults.Add(New STRUC_SEARCH_RESULTS With {
                    .iLocation = mMatch.Index,
                    .iLength = mMatch.Length,
                    .sFile = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File,
                    .sTabIdentifier = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Identifier
                })
            Next
        End If

        Return lResults.ToArray
    End Function

    Private Sub Button_Search_Click(sender As Object, e As EventArgs) Handles Button_Search.Click
        Dim mResults As STRUC_SEARCH_RESULTS() = DoSearch(False)
        If (mResults Is Nothing) Then
            Return
        End If

        ToolStripStatusLabel_Status.Text = String.Format("{0} Items found!", mResults.Length)

        If (mResults.Length < 1) Then
            Return
        End If

        Try
            Dim iOffset As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset

            If (RadioButton_DirectionUp.Checked) Then
                For i = mResults.Length - 1 To 0 Step -1
                    If (mResults(i).iLocation < iOffset) Then
                        SetTextEditorSelection(g_mFormMain.g_ClassTabControl.m_ActiveTab, mResults(i).iLocation, mResults(i).iLength, True)
                        Return
                    End If
                Next

                If (CheckBox_LoopSearch.Checked) Then
                    SetTextEditorSelection(g_mFormMain.g_ClassTabControl.m_ActiveTab, mResults(mResults.Length - 1).iLocation, mResults(mResults.Length - 1).iLength, False)

                    ToolStripStatusLabel_Status.Text &= " Looped!"
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
                    Return
                End If
            Else
                For i = 0 To mResults.Length - 1
                    If (mResults(i).iLocation >= iOffset) Then
                        SetTextEditorSelection(g_mFormMain.g_ClassTabControl.m_ActiveTab, mResults(i).iLocation, mResults(i).iLength, False)
                        Return
                    End If
                Next

                If (CheckBox_LoopSearch.Checked) Then
                    SetTextEditorSelection(g_mFormMain.g_ClassTabControl.m_ActiveTab, mResults(0).iLocation, mResults(0).iLength, False)

                    ToolStripStatusLabel_Status.Text &= " Looped!"
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
                    Return
                End If
            End If

            ToolStripStatusLabel_Status.Text &= " End reached!"
            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_Replace_Click(sender As Object, e As EventArgs) Handles Button_Replace.Click
        Dim mResults As STRUC_SEARCH_RESULTS() = DoSearch(False)
        If (mResults Is Nothing) Then
            Return
        End If

        ToolStripStatusLabel_Status.Text = String.Format("{0} Items found!", mResults.Length)

        If (mResults.Length < 1) Then
            Return
        End If

        Try
            Dim iOffset As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset

            If (RadioButton_DirectionUp.Checked) Then
                For i = mResults.Length - 1 To 0 Step -1
                    If (mResults(i).iLocation < iOffset - mResults(i).iLength) Then
                        g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Replace(mResults(i).iLocation, mResults(i).iLength, TextBox_Replace.Text)
                        g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Refresh()

                        SetTextEditorSelection(g_mFormMain.g_ClassTabControl.m_ActiveTab, mResults(i).iLocation, TextBox_Replace.Text.Length, True)
                        Return
                    End If
                Next
            Else
                For i = 0 To mResults.Length - 1
                    If (mResults(i).iLocation >= iOffset) Then
                        g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Replace(mResults(i).iLocation, mResults(i).iLength, TextBox_Replace.Text)
                        g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Refresh()

                        SetTextEditorSelection(g_mFormMain.g_ClassTabControl.m_ActiveTab, mResults(i).iLocation, TextBox_Replace.Text.Length, False)
                        Return
                    End If
                Next
            End If

            ToolStripStatusLabel_Status.Text &= " End reached!"
            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_ReplaceAll_Click(sender As Object, e As EventArgs) Handles Button_ReplaceAll.Click
        Dim mResults As STRUC_SEARCH_RESULTS() = DoSearch(False)
        If (mResults Is Nothing) Then
            Return
        End If

        ToolStripStatusLabel_Status.Text = String.Format("{0} Items replaced!", mResults.Length)

        If (mResults.Length < 1) Then
            Return
        End If

        Try
            Dim iOffset As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset

            g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.StartUndoGroup()

            For i = mResults.Length - 1 To 0 Step -1
                g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Replace(mResults(i).iLocation, mResults(i).iLength, TextBox_Replace.Text)
            Next

            g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.EndUndoGroup()
            g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Refresh()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_ListAll_Click(sender As Object, e As EventArgs) Handles Button_ListAll.Click
        ShowListOutput()

        Dim mResults As STRUC_SEARCH_RESULTS() = DoSearch(False)
        If (mResults Is Nothing) Then
            ListView_Output.Items.Clear()
            Return
        End If

        ToolStripStatusLabel_Status.Text = String.Format("{0} Items found!", mResults.Length)

        If (mResults.Length < 1) Then
            ListView_Output.Items.Clear()
            Return
        End If

        Try
            ListView_Output.BeginUpdate()
            ListView_Output.Items.Clear()
            For Each mItem In mResults
                Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByIdentifier(mItem.sTabIdentifier)
                If (mTab Is Nothing) Then
                    Continue For
                End If

                If (mItem.iLocation + mItem.iLength > mTab.m_TextEditor.Document.TextLength) Then
                    Continue For
                End If

                Dim sText As String = GetFullLineByOffset(mTab, mItem.iLocation)
                If (String.IsNullOrEmpty(sText)) Then
                    Continue For
                End If

                Dim iLine As Integer = mTab.m_TextEditor.Document.GetLineNumberForOffset(mItem.iLocation) + 1

                Dim mListViewItemData As New ClassListViewItemData(New String() {
                                                                   IO.Path.GetFileName(mItem.sFile),
                                                                   CStr(iLine),
                                                                   sText,
                                                                   mItem.sFile})

                mListViewItemData.g_mData("Filename") = IO.Path.GetFileName(mItem.sFile)
                mListViewItemData.g_mData("Line") = iLine
                mListViewItemData.g_mData("Text") = sText
                mListViewItemData.g_mData("File") = mItem.sFile
                mListViewItemData.g_mData("Location") = mItem.iLocation
                mListViewItemData.g_mData("Length") = mItem.iLength
                mListViewItemData.g_mData("TabIdentifier") = mItem.sTabIdentifier

                ListView_Output.Items.Add(mListViewItemData)
            Next

            ClassTools.ClassControls.ClassListView.AutoResizeColumns(ListView_Output)
            ListView_Output.EndUpdate()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_ListAllOpenTabs_Click(sender As Object, e As EventArgs) Handles Button_ListAllOpenTabs.Click
        ShowListOutput()

        Dim mResults As STRUC_SEARCH_RESULTS() = DoSearch(True)
        If (mResults Is Nothing) Then
            ListView_Output.Items.Clear()
            Return
        End If

        ToolStripStatusLabel_Status.Text = String.Format("{0} Items found!", mResults.Length)

        If (mResults.Length < 1) Then
            ListView_Output.Items.Clear()
            Return
        End If

        Try
            ListView_Output.BeginUpdate()
            ListView_Output.Items.Clear()
            For Each mItem In mResults
                Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByIdentifier(mItem.sTabIdentifier)
                If (mTab Is Nothing) Then
                    Continue For
                End If

                If (mItem.iLocation + mItem.iLength > mTab.m_TextEditor.Document.TextLength) Then
                    Continue For
                End If

                Dim sText As String = GetFullLineByOffset(mTab, mItem.iLocation)
                If (String.IsNullOrEmpty(sText)) Then
                    Continue For
                End If

                Dim iLine As Integer = mTab.m_TextEditor.Document.GetLineNumberForOffset(mItem.iLocation) + 1

                Dim mListViewItemData As New ClassListViewItemData(New String() {
                                                                   IO.Path.GetFileName(mItem.sFile),
                                                                   CStr(iLine),
                                                                   sText,
                                                                   mItem.sFile})

                mListViewItemData.g_mData("Filename") = IO.Path.GetFileName(mItem.sFile)
                mListViewItemData.g_mData("Line") = iLine
                mListViewItemData.g_mData("Text") = sText
                mListViewItemData.g_mData("File") = mItem.sFile
                mListViewItemData.g_mData("Location") = mItem.iLocation
                mListViewItemData.g_mData("Length") = mItem.iLength
                mListViewItemData.g_mData("TabIdentifier") = mItem.sTabIdentifier

                ListView_Output.Items.Add(mListViewItemData)
            Next

            ClassTools.ClassControls.ClassListView.AutoResizeColumns(ListView_Output)
            ListView_Output.EndUpdate()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ShowListOutput()
        ListView_Output.Visible = True

        If (g_mCollapsedSize = Me.Size) Then
            Me.Size = g_mExpandedSize
        End If
    End Sub

    Private Sub TextBox_Search_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox_Search.KeyDown
        Select Case (e.KeyCode)
            Case Keys.Up
                e.Handled = True
                e.SuppressKeyPress = True

                RadioButton_DirectionUp.Checked = True
                Button_Search.PerformClick()

            Case Keys.Down
                e.Handled = True
                e.SuppressKeyPress = True

                RadioButton_DirectionDown.Checked = True
                Button_Search.PerformClick()
        End Select
    End Sub

    Private Sub ListView_Output_Click(sender As Object, e As EventArgs) Handles ListView_Output.Click
        If (ListView_Output.SelectedItems.Count < 1) Then
            Return
        End If

        Try
            Dim mListViewItemData = TryCast(ListView_Output.SelectedItems(0), ClassListViewItemData)
            If (mListViewItemData Is Nothing) Then
                Return
            End If

            Dim sFile As String = CStr(mListViewItemData.g_mData("File"))
            Dim iLocation As Integer = CInt(mListViewItemData.g_mData("Location"))
            Dim iLength As Integer = CInt(mListViewItemData.g_mData("Length"))
            Dim sTabIdentifier As String = CStr(mListViewItemData.g_mData("TabIdentifier"))

            Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByIdentifier(sTabIdentifier)
            If (mTab Is Nothing) Then
                MessageBox.Show("Could not show the result! The tab has been closed!", "Unable to show result", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                Return
            End If

            SetTextEditorSelection(mTab, iLocation, iLength, False)

            If (Not mTab.m_IsActive) Then
                mTab.SelectTab()
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class