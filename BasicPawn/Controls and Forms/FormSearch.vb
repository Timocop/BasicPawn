'BasicPawn
'Copyright(C) 2020 TheTimocop

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

    Enum ENUM_SEARCH_TYPE
        TAB
        ALL_TABS
        ALL_INCLUDES
    End Enum

    Private Structure STRUC_SEARCH_RESULTS
        Dim sText As String
        Dim iLocation As Integer
        Dim iLength As Integer
        Dim iLine As Integer
        Dim sFile As String
        Dim sTabIdentifier As String
        Dim mMatch As Match
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

    ReadOnly Property m_SingleInstance As Boolean
        Get
            Return CheckBox_SingleInstance.Checked
        End Get
    End Property

    Property m_SearchText As String
        Get
            Return TextBox_Search.Text
        End Get
        Set(value As String)
            TextBox_Search.Text = value
        End Set
    End Property

    Property m_ReplaceText As String
        Get
            Return TextBox_Replace.Text
        End Get
        Set(value As String)
            TextBox_Replace.Text = value
        End Set
    End Property

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

    Private Sub Button_SearchPre_Click(sender As Object, e As EventArgs) Handles Button_SearchPre.Click
        RadioButton_DirectionUp.Checked = True
        Button_Search.PerformClick()
    End Sub

    Private Sub Button_SearchNext_Click(sender As Object, e As EventArgs) Handles Button_SearchNext.Click
        RadioButton_DirectionDown.Checked = True
        Button_Search.PerformClick()
    End Sub

    Private Sub Button_ReplacePre_Click(sender As Object, e As EventArgs) Handles Button_ReplacePre.Click
        RadioButton_DirectionUp.Checked = True
        Button_Replace.PerformClick()
    End Sub

    Private Sub Button_ReplaceNext_Click(sender As Object, e As EventArgs) Handles Button_ReplaceNext.Click
        RadioButton_DirectionDown.Checked = True
        Button_Replace.PerformClick()
    End Sub

    Private Sub Button_Search_Click(sender As Object, e As EventArgs) Handles Button_Search.Click
        Dim mResults As STRUC_SEARCH_RESULTS() = DoSearch(ENUM_SEARCH_TYPE.TAB)
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
        Dim mResults As STRUC_SEARCH_RESULTS() = DoSearch(ENUM_SEARCH_TYPE.TAB)
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
                        Dim sReplace As String

                        If (mResults(i).mMatch IsNot Nothing) Then
                            sReplace = mResults(i).mMatch.Result(TextBox_Replace.Text)
                        Else
                            sReplace = TextBox_Replace.Text
                        End If

                        g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Replace(mResults(i).iLocation, mResults(i).iLength, sReplace)

                        g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.InvalidateTextArea()

                        SetTextEditorSelection(g_mFormMain.g_ClassTabControl.m_ActiveTab, mResults(i).iLocation, sReplace.Length, True)
                        Return
                    End If
                Next
            Else
                For i = 0 To mResults.Length - 1
                    If (mResults(i).iLocation >= iOffset) Then
                        Dim sReplace As String

                        If (mResults(i).mMatch IsNot Nothing) Then
                            sReplace = mResults(i).mMatch.Result(TextBox_Replace.Text)
                        Else
                            sReplace = TextBox_Replace.Text
                        End If

                        g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Replace(mResults(i).iLocation, mResults(i).iLength, sReplace)

                        g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.InvalidateTextArea()

                        SetTextEditorSelection(g_mFormMain.g_ClassTabControl.m_ActiveTab, mResults(i).iLocation, sReplace.Length, False)
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
        Dim mResults As STRUC_SEARCH_RESULTS() = DoSearch(ENUM_SEARCH_TYPE.TAB)
        If (mResults Is Nothing) Then
            Return
        End If

        If (mResults.Length < 1) Then
            ToolStripStatusLabel_Status.Text = String.Format("{0} Items replaced!", 0)
            Return
        End If

        Dim iReplaceCount As Integer = 0
        Dim bSomethingSelected As Boolean = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected

        If (CheckBox_ReplaceInSelection.Checked AndAlso Not bSomethingSelected) Then
            MessageBox.Show("Nothing has been selected", "Unable to replace", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            ToolStripStatusLabel_Status.Text = String.Format("{0} Items replaced!", 0)
            Return
        End If

        Dim iOffset As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Caret.Offset

        Try
            g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.StartUndoGroup()

            For i = mResults.Length - 1 To 0 Step -1
                If (CheckBox_ReplaceInSelection.Checked) Then
                    If (Not bSomethingSelected) Then
                        Exit For
                    End If

                    Dim iSelectOffset As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Offset
                    Dim iSelectLength As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Length

                    If (mResults(i).iLocation < iSelectOffset) Then
                        Continue For
                    End If

                    If (mResults(i).iLocation > (iSelectOffset + iSelectLength)) Then
                        Continue For
                    End If

                    If ((mResults(i).iLocation + mResults(i).iLength) > (iSelectOffset + iSelectLength)) Then
                        Continue For
                    End If
                End If

                Dim sReplace As String

                If (mResults(i).mMatch IsNot Nothing) Then
                    sReplace = mResults(i).mMatch.Result(TextBox_Replace.Text)
                Else
                    sReplace = TextBox_Replace.Text
                End If

                g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Replace(mResults(i).iLocation, mResults(i).iLength, sReplace)

                iReplaceCount += 1
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        Finally
            g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.EndUndoGroup()

            g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.InvalidateTextArea()
        End Try

        ToolStripStatusLabel_Status.Text = String.Format("{0} Items replaced!", iReplaceCount)
    End Sub

    Private Sub Button_ListAll_Click(sender As Object, e As EventArgs) Handles Button_ListAll.Click
        ShowListOutput()

        Dim mResults As STRUC_SEARCH_RESULTS() = Nothing

        Select Case (True)
            Case RadioButton_ListTypeCurrent.Checked
                mResults = DoSearch(ENUM_SEARCH_TYPE.TAB)

            Case RadioButton_ListTypeTabs.Checked
                mResults = DoSearch(ENUM_SEARCH_TYPE.ALL_TABS)

            Case RadioButton_ListTypeIncludes.Checked
                mResults = DoSearch(ENUM_SEARCH_TYPE.ALL_INCLUDES)
        End Select

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
            Try
                ListView_Output.BeginUpdate()
                ListView_Output.Items.Clear()
                For Each mItem In mResults
                    Dim mListViewItemData As New ClassListViewItemData(New String() {
                                                                       IO.Path.GetFileName(mItem.sFile),
                                                                       CStr(mItem.iLine + 1),
                                                                       mItem.sText,
                                                                       mItem.sFile})

                    mListViewItemData.g_mData("Filename") = IO.Path.GetFileName(mItem.sFile)
                    mListViewItemData.g_mData("Text") = mItem.sText
                    mListViewItemData.g_mData("Line") = mItem.iLine
                    mListViewItemData.g_mData("File") = mItem.sFile
                    mListViewItemData.g_mData("Location") = mItem.iLocation
                    mListViewItemData.g_mData("Length") = mItem.iLength
                    mListViewItemData.g_mData("TabIdentifier") = mItem.sTabIdentifier
                    mListViewItemData.g_mData("Match") = mItem.mMatch

                    ListView_Output.Items.Add(mListViewItemData)
                Next

                ClassTools.ClassControls.ClassListView.AutoResizeColumns(ListView_Output)
            Finally
                ListView_Output.EndUpdate()
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub TextBox_Search_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox_Search.KeyDown
        Select Case (e.KeyCode)
            Case Keys.Enter
                e.Handled = True
                e.SuppressKeyPress = True

                Button_Search.PerformClick()

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

    Private Sub TextBox_Replace_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox_Replace.KeyDown
        Select Case (e.KeyCode)
            Case Keys.Enter
                e.Handled = True
                e.SuppressKeyPress = True

                Button_Replace.PerformClick()

            Case Keys.Up
                e.Handled = True
                e.SuppressKeyPress = True

                RadioButton_DirectionUp.Checked = True
                Button_Replace.PerformClick()

            Case Keys.Down
                e.Handled = True
                e.SuppressKeyPress = True

                RadioButton_DirectionDown.Checked = True
                Button_Replace.PerformClick()
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

            'Open match tab
            Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByIdentifier(sTabIdentifier)
            If (mTab Is Nothing) Then
                'If not, check if file exist and search for tab
                If (IO.File.Exists(sFile)) Then
                    mTab = g_mFormMain.g_ClassTabControl.GetTabByFile(sFile)

                    'If that also fails, just open the file
                    If (mTab Is Nothing) Then
                        mTab = g_mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(sFile)
                        mTab.SelectTab()
                    End If
                Else
                    MessageBox.Show("Could not show the result! The file does not exist!", "Unable to show result", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                    Return
                End If
            End If

            If (Not mTab.m_IsActive) Then
                mTab.SelectTab()
            End If

            SetTextEditorSelection(mTab, iLocation, iLength, False)
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

    Private Sub SetTextEditorSelection(mTab As ClassTabControl.ClassTab, iOffset As Integer, iLength As Integer, bCaretBeginPos As Boolean)
        If (iOffset + iLength > mTab.m_TextEditor.Document.TextLength) Then
            Return
        End If

        Dim iLineLenStart As Integer = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).LineNumber
        Dim iLineLenEnd As Integer = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset + iLength).LineNumber
        Dim iLineColumStart As Integer = iOffset - mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Offset
        Dim iLineColumEnd As Integer = iOffset + iLength - mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset + iLength).Offset

        Dim mStartLoc As New TextLocation(iLineColumStart, iLineLenStart)
        Dim mEndLoc As New TextLocation(iLineColumEnd, iLineLenEnd)

        mTab.m_TextEditor.ActiveTextAreaControl.Caret.Position = If(bCaretBeginPos, mStartLoc, mEndLoc)
        mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(mStartLoc, mEndLoc)
        mTab.m_TextEditor.ActiveTextAreaControl.CenterViewOn(If(bCaretBeginPos, mStartLoc.Line, mEndLoc.Line), 10)
    End Sub

    ''' <summary>
    ''' Get the full line string by index.
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <param name="iIndex"></param>
    ''' <returns></returns>
    Private Function GetFullLineByIndex(sSource As String, iIndex As Integer) As String
        Dim iTargetLine As Integer = GetLineByIndex(sSource, iIndex)
        Dim iLine As Integer = -1

        Using mSR As New IO.StringReader(sSource)
            While True
                Dim sLine As String = mSR.ReadLine
                If (sLine Is Nothing) Then
                    Exit While
                End If

                iLine += 1

                If (iLine = iTargetLine) Then
                    Return sLine
                End If
            End While
        End Using

        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the line from an index.
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <param name="iIndex"></param>
    ''' <returns></returns>
    Private Function GetLineByIndex(sSource As String, iIndex As Integer) As Integer
        Dim iCount As Integer = 0

        For i = 0 To sSource.Length - 1
            If (i >= iIndex) Then
                Exit For
            End If

            If (sSource(i) = vbLf) Then
                iCount += 1
            End If
        Next

        Return iCount
    End Function

    ''' <summary>
    ''' Gets the line begin index from an index.
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <param name="iIndex"></param>
    ''' <returns></returns>
    Private Function GetLineBeginIndexByIndex(sSource As String, iIndex As Integer) As Integer
        For i = iIndex To 0 Step -1
            If (sSource(i) = vbLf) Then
                Return Math.Min(i + 1, sSource.Length - 1)
            End If
        Next

        Return 0
    End Function

    ''' <summary>
    ''' Gets the line end index from an index.
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <param name="iIndex"></param>
    ''' <returns></returns>
    Private Function GetLineEndIndexByIndex(sSource As String, iIndex As Integer) As Integer
        For i = iIndex To sSource.Length - 1
            If (sSource(i) = vbLf) Then
                Return i
            End If
        Next

        Return sSource.Length - 1
    End Function

    ''' <summary>
    ''' Does the search and gets all results.
    ''' </summary>
    ''' <returns></returns>
    Private Function DoSearch(iSearchType As ENUM_SEARCH_TYPE) As STRUC_SEARCH_RESULTS()
        Dim sSearchText As String = TextBox_Search.Text

        If (String.IsNullOrEmpty(sSearchText) OrElse sSearchText.Trim.Length < 1) Then
            ToolStripStatusLabel_Status.Text = "Unable to search 'nothing'!"
            Return Nothing
        End If

        Dim mRegex As Regex
        Dim bNormalMode As Boolean = RadioButton_ModeNormal.Checked
        Dim bWholeWord As Boolean = CheckBox_WholeWord.Checked
        Dim bCastSensitive As Boolean = CheckBox_CaseSensitive.Checked
        Dim bMultiLine As Boolean = CheckBox_Multiline.Checked
        Dim bMergeLines As Boolean = CheckBox_ListMergeLines.Checked

        Try
            Select Case (bNormalMode)
                Case True
                    mRegex = New Regex(If(bWholeWord, "\b", "") & Regex.Escape(sSearchText) & If(bWholeWord, "\b", ""), If(bCastSensitive, RegexOptions.None, RegexOptions.IgnoreCase))
                Case Else
                    mRegex = New Regex(sSearchText, If(bCastSensitive, RegexOptions.None, RegexOptions.IgnoreCase) Or If(bMultiLine, RegexOptions.Multiline, RegexOptions.None))
            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
            ToolStripStatusLabel_Status.Text = "Search Error!"
            Return Nothing
        End Try

        Dim lResults As New List(Of STRUC_SEARCH_RESULTS)

        Select Case (iSearchType)
            Case ENUM_SEARCH_TYPE.TAB
                Dim sSource As String = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent

                For Each mMatch As Match In mRegex.Matches(sSource)
                    Dim mResult As New STRUC_SEARCH_RESULTS With {
                        .sText = GetFullLineByIndex(sSource, mMatch.Index),
                        .iLocation = mMatch.Index,
                        .iLength = mMatch.Length,
                        .iLine = GetLineByIndex(sSource, mMatch.Index),
                        .sFile = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File,
                        .sTabIdentifier = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Identifier,
                        .mMatch = If(bNormalMode, Nothing, mMatch)
                    }

                    If (bMergeLines) Then
                        If (lResults.Exists(Function(x As STRUC_SEARCH_RESULTS)
                                                Return (x.iLine = mResult.iLine AndAlso x.sFile = mResult.sFile AndAlso x.sTabIdentifier = mResult.sTabIdentifier)
                                            End Function)) Then
                            Continue For
                        End If

                        mResult.iLocation = GetLineBeginIndexByIndex(sSource, mMatch.Index)
                        mResult.iLength = GetFullLineByIndex(sSource, mMatch.Index).Length
                    End If

                    lResults.Add(mResult)
                Next

            Case ENUM_SEARCH_TYPE.ALL_TABS
                For i = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                    Dim sSource As String = g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.TextContent

                    For Each mMatch As Match In mRegex.Matches(sSource)
                        Dim mResult As New STRUC_SEARCH_RESULTS With {
                            .sText = GetFullLineByIndex(sSource, mMatch.Index),
                            .iLocation = mMatch.Index,
                            .iLength = mMatch.Length,
                            .iLine = GetLineByIndex(sSource, mMatch.Index),
                            .sFile = g_mFormMain.g_ClassTabControl.m_Tab(i).m_File,
                            .sTabIdentifier = g_mFormMain.g_ClassTabControl.m_Tab(i).m_Identifier,
                            .mMatch = If(bNormalMode, Nothing, mMatch)
                        }

                        If (bMergeLines) Then
                            If (lResults.Exists(Function(x As STRUC_SEARCH_RESULTS)
                                                    Return (x.iLine = mResult.iLine AndAlso x.sFile = mResult.sFile AndAlso x.sTabIdentifier = mResult.sTabIdentifier)
                                                End Function)) Then
                                Continue For
                            End If

                            mResult.iLocation = GetLineBeginIndexByIndex(sSource, mMatch.Index)
                            mResult.iLength = GetFullLineByIndex(sSource, mMatch.Index).Length
                        End If

                        lResults.Add(mResult)
                    Next
                Next

            Case ENUM_SEARCH_TYPE.ALL_INCLUDES
                Dim mIncludeFiles = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IncludesGroup.m_IncludeFiles.ToArray

                For Each mInclude In mIncludeFiles
                    If (Not IO.File.Exists(mInclude.Value)) Then
                        Continue For
                    End If

                    Dim sSource As String
                    Dim sTabIdentifier As String = ""

                    If (mInclude.Value.ToLower = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File.ToLower) Then
                        sSource = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent
                        sTabIdentifier = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Identifier
                    Else
                        sSource = IO.File.ReadAllText(mInclude.Value)
                    End If

                    For Each mMatch As Match In mRegex.Matches(sSource)
                        Dim mResult As New STRUC_SEARCH_RESULTS With {
                            .sText = GetFullLineByIndex(sSource, mMatch.Index),
                            .iLocation = mMatch.Index,
                            .iLength = mMatch.Length,
                            .iLine = GetLineByIndex(sSource, mMatch.Index),
                            .sFile = mInclude.Value,
                            .sTabIdentifier = sTabIdentifier,
                            .mMatch = If(bNormalMode, Nothing, mMatch)
                        }

                        If (bMergeLines) Then
                            If (lResults.Exists(Function(x As STRUC_SEARCH_RESULTS)
                                                    Return (x.iLine = mResult.iLine AndAlso x.sFile = mResult.sFile AndAlso x.sTabIdentifier = mResult.sTabIdentifier)
                                                End Function)) Then
                                Continue For
                            End If

                            mResult.iLocation = GetLineBeginIndexByIndex(sSource, mMatch.Index)
                            mResult.iLength = GetFullLineByIndex(sSource, mMatch.Index).Length
                        End If

                        lResults.Add(mResult)
                    Next
                Next
        End Select

        Return lResults.ToArray
    End Function

#Region "WindowInfo"
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
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "TransparencyValue", CStr(TrackBar_Transparency.Value)),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "ModeNormal", If(RadioButton_ModeNormal.Checked, "1", "0"))
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

                If (mIni.ReadKeyValue(Me.Name, "ModeNormal", "1") <> "0") Then
                    RadioButton_ModeNormal.Checked = True
                Else
                    RadioButton_ModeRegEx.Checked = True
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
#End Region

#Region "WndProc"
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
#End Region

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If (keyData = Keys.Escape) Then
            Me.Close()

            Return True
        End If

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
End Class