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

    Private g_bIgnoreEvent As Boolean = False
    Private g_bIsFormFocused As Boolean = True
    Private g_bIsFormClosing As Boolean = False

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

        'Load last window info
        ClassSettings.LoadWindowInfo(Me)
        LoadViews()

        'Keep size
        Me.Size = g_mCollapsedSize
    End Sub

    Private Sub FormSearch_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        g_bIsFormClosing = True

        'Save window info
        ClassSettings.SaveWindowInfo(Me)
        SaveViews()
    End Sub

    Private Sub CheckBox_Transparency_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_Transparency.CheckedChanged
        If (g_bIgnoreEvent) Then
            Return
        End If

        UpdateViews()
    End Sub

    Private Sub RadioButton_TransparencyInactive_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_TransparencyInactive.CheckedChanged
        If (g_bIgnoreEvent) Then
            Return
        End If

        UpdateViews()
    End Sub

    Private Sub RadioButton_TransparencyAlways_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_TransparencyAlways.CheckedChanged
        If (g_bIgnoreEvent) Then
            Return
        End If

        UpdateViews()
    End Sub

    Private Sub TrackBar_Transparency_Scroll(sender As Object, e As EventArgs) Handles TrackBar_Transparency.Scroll
        If (g_bIgnoreEvent) Then
            Return
        End If

        UpdateViews()
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

    Private Function GetTextEditorSelection(mTab As ClassTabControl.SourceTabPage, ByRef r_iOffset As Integer, ByRef r_iLength As Integer) As Boolean
        If (Not mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
            Return False
        End If

        For Each i In mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection
            r_iOffset = i.Offset
            r_iLength = i.Length

            Return True
        Next

        Return False
    End Function

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

    Public Sub SaveViews()
        If (String.IsNullOrEmpty(Me.Name)) Then
            Return
        End If

        Using mStream = ClassFileStreamWait.Create(ClassSettings.g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT) From {
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "WholeWord", If(CheckBox_WholeWord.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "CaseSensitive", If(CheckBox_CaseSensitive.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "Multiline", If(CheckBox_Multiline.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "LoopSearch", If(CheckBox_LoopSearch.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "Transparency", If(CheckBox_Transparency.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "TransparencyInactive", If(RadioButton_TransparencyInactive.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "TransparencyValue", CStr(TrackBar_Transparency.Value))
                }

                mIni.WriteKeyValue(lContent.ToArray)
            End Using
        End Using
    End Sub

    Public Sub LoadViews()
        If (String.IsNullOrEmpty(Me.Name)) Then
            Return
        End If

        Dim tmpStr As String
        Dim tmpInt As Integer

        Using mStream = ClassFileStreamWait.Create(ClassSettings.g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                g_bIgnoreEvent = True
                CheckBox_WholeWord.Checked = (mIni.ReadKeyValue(Me.Name, "WholeWord", "0") <> "0")
                CheckBox_CaseSensitive.Checked = (mIni.ReadKeyValue(Me.Name, "CaseSensitive", "0") <> "0")
                CheckBox_Multiline.Checked = (mIni.ReadKeyValue(Me.Name, "Multiline", "0") <> "0")
                CheckBox_LoopSearch.Checked = (mIni.ReadKeyValue(Me.Name, "LoopSearch", "0") <> "0")
                CheckBox_Transparency.Checked = (mIni.ReadKeyValue(Me.Name, "Transparency", "1") <> "0")

                If (mIni.ReadKeyValue(Me.Name, "TransparencyInactive", "1") <> "0") Then
                    RadioButton_TransparencyInactive.Checked = True
                    RadioButton_TransparencyAlways.Checked = False
                Else
                    RadioButton_TransparencyInactive.Checked = False
                    RadioButton_TransparencyAlways.Checked = True
                End If

                tmpStr = mIni.ReadKeyValue(Me.Name, "TransparencyValue", Nothing)
                If (tmpStr IsNot Nothing AndAlso Integer.TryParse(tmpStr, tmpInt)) Then
                    TrackBar_Transparency.Value = ClassTools.ClassMath.ClampInt(tmpInt, TrackBar_Transparency.Minimum, TrackBar_Transparency.Maximum)
                End If

                g_bIgnoreEvent = False
            End Using
        End Using

        UpdateViews()
    End Sub

    Public Sub UpdateViews()
        If (CheckBox_Transparency.Checked) Then
            If (RadioButton_TransparencyInactive.Checked) Then
                If (g_bIsFormFocused) Then
                    Me.Opacity = 1
                Else
                    Select Case (TrackBar_Transparency.Value)
                        Case 1
                            Me.Opacity = 0.2
                        Case 2
                            Me.Opacity = 0.4
                        Case 3
                            Me.Opacity = 0.6
                        Case 4
                            Me.Opacity = 0.8
                        Case Else
                            Me.Opacity = 1
                    End Select
                End If
            Else
                Select Case (TrackBar_Transparency.Value)
                    Case 1
                        Me.Opacity = 0.2
                    Case 2
                        Me.Opacity = 0.4
                    Case 3
                        Me.Opacity = 0.6
                    Case 4
                        Me.Opacity = 0.8
                    Case Else
                        Me.Opacity = 1
                End Select
            End If
        Else
            Me.Opacity = 1
        End If
    End Sub

    Const WM_SETFOCUS = &H7
    Const WM_KILLFOCUS = &H8
    Const WM_ACTIVATE = &H6

    Const WA_INACTIVE = 0
    Const WA_ACTIVE = 1
    Const WA_CLICKACTIVE = 2

    Protected Overrides Sub WndProc(ByRef m As Message)
        Static bIgnoreEvent As Boolean = False

        MyBase.WndProc(m)

        If (bIgnoreEvent OrElse g_bIsFormClosing) Then
            Return
        End If

        bIgnoreEvent = True

        Select Case (m.Msg)
            Case WM_ACTIVATE
                If (m.WParam <> New IntPtr(WA_INACTIVE)) Then
                    g_bIsFormFocused = True

                    UpdateViews()
                Else
                    g_bIsFormFocused = False

                    UpdateViews()
                End If

            Case WM_SETFOCUS
                g_bIsFormFocused = True

                UpdateViews()

            Case WM_KILLFOCUS
                g_bIsFormFocused = False

                UpdateViews()
        End Select

        bIgnoreEvent = False
    End Sub
End Class