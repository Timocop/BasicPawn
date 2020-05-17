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

Partial Public Class FormMain
    Private Sub ToolStripMenuItem_ToolsSettingsAndConfigs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsSettingsAndConfigs.Click
        Using i As New FormSettings(Me, FormSettings.ENUM_CONFIG_TYPE.ACTIVE)
            If (i.ShowDialog(Me) = DialogResult.OK) Then
                i.ApplySettings()
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_ToolsFormatCodeIndent_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsFormatCodeIndent.Click
        Try
            g_ClassTextEditorTools.FormatCode(g_ClassTabControl.m_ActiveTab)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ToolsFormatCodeTrim_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsFormatCodeTrim.Click
        Try
            g_ClassTextEditorTools.FormatCodeTrim(g_ClassTabControl.m_ActiveTab)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ToolsConvertToSettings_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsConvertToSettings.Click
        Try
            If (Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                MessageBox.Show("Nothing selected to format!", "Unable to format", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim lRealSourceLines As New List(Of String)
            Using mSR As New IO.StringReader(g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent)
                Dim sLine As String
                While True
                    sLine = mSR.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    lRealSourceLines.Add(sLine)
                End While
            End Using

            Dim sFormatedSource As String = ClassSyntaxTools.ClassSyntaxHelpers.FormatCodeConvert(g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent, ClassSyntaxTools.ClassSyntaxHelpers.ENUM_INDENTATION_TYPES.USE_SETTINGS, -1)
            Dim lFormatedSourceLines As New List(Of String)
            Using mSR As New IO.StringReader(sFormatedSource)
                Dim sLine As String
                While True
                    sLine = mSR.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    lFormatedSourceLines.Add(sLine)
                End While
            End Using

            If (lRealSourceLines.Count <> lFormatedSourceLines.Count) Then
                Throw New ArgumentException("Formated number of lines are not equal with document number of lines")
            End If

            Try
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.StartUndoGroup()

                For i = lFormatedSourceLines.Count - 1 To 0 Step -1
                    Dim mLineSeg = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineSegment(i)
                    Dim sLine As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetText(mLineSeg.Offset, mLineSeg.Length)

                    If (Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset) AndAlso
                            Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset + mLineSeg.Length)) Then
                        Continue For
                    End If

                    If (sLine = lFormatedSourceLines(i)) Then
                        Continue For
                    End If

                    g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Remove(mLineSeg.Offset, mLineSeg.Length)
                    g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Insert(mLineSeg.Offset, lFormatedSourceLines(i))
                Next
            Finally
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.EndUndoGroup()

                g_ClassTabControl.m_ActiveTab.m_TextEditor.InvalidateTextArea()
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ToolsConvertToTab_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsConvertToTab.Click
        Try
            If (Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                MessageBox.Show("Nothing selected to format!", "Unable to format", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim lRealSourceLines As New List(Of String)
            Using mSR As New IO.StringReader(g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent)
                Dim sLine As String
                While True
                    sLine = mSR.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    lRealSourceLines.Add(sLine)
                End While
            End Using

            Dim iSpaceLength As Integer
            If (Not Integer.TryParse(Regex.Match(ToolStripTextBox_ToolsConvertSpaceSize.Text, "[0-9]+").Value, iSpaceLength) OrElse iSpaceLength < 1) Then
                Throw New ArgumentException("Invalid space size")
            End If

            Dim sFormatedSource As String = ClassSyntaxTools.ClassSyntaxHelpers.FormatCodeConvert(g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent, ClassSyntaxTools.ClassSyntaxHelpers.ENUM_INDENTATION_TYPES.TABS, iSpaceLength)
            Dim lFormatedSourceLines As New List(Of String)
            Using mSR As New IO.StringReader(sFormatedSource)
                Dim sLine As String
                While True
                    sLine = mSR.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    lFormatedSourceLines.Add(sLine)
                End While
            End Using

            If (lRealSourceLines.Count <> lFormatedSourceLines.Count) Then
                Throw New ArgumentException("Formated number of lines are not equal with document number of lines")
            End If

            Try
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.StartUndoGroup()

                For i = lFormatedSourceLines.Count - 1 To 0 Step -1
                    Dim mLineSeg = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineSegment(i)
                    Dim sLine As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetText(mLineSeg.Offset, mLineSeg.Length)

                    If (Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset) AndAlso
                            Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset + mLineSeg.Length)) Then
                        Continue For
                    End If

                    If (sLine = lFormatedSourceLines(i)) Then
                        Continue For
                    End If

                    g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Remove(mLineSeg.Offset, mLineSeg.Length)
                    g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Insert(mLineSeg.Offset, lFormatedSourceLines(i))
                Next
            Finally
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.EndUndoGroup()

                g_ClassTabControl.m_ActiveTab.m_TextEditor.InvalidateTextArea()
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ToolsConvertToSpace_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsConvertToSpace.Click
        Try
            If (Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                MessageBox.Show("Nothing selected to format!", "Unable to format", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim lRealSourceLines As New List(Of String)
            Using mSR As New IO.StringReader(g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent)
                Dim sLine As String
                While True
                    sLine = mSR.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    lRealSourceLines.Add(sLine)
                End While
            End Using

            Dim iSpaceLength As Integer
            If (Not Integer.TryParse(Regex.Match(ToolStripTextBox_ToolsConvertSpaceSize.Text, "[0-9]+").Value, iSpaceLength) OrElse iSpaceLength < 1) Then
                Throw New ArgumentException("Invalid space size")
            End If

            Dim sFormatedSource As String = ClassSyntaxTools.ClassSyntaxHelpers.FormatCodeConvert(g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent, ClassSyntaxTools.ClassSyntaxHelpers.ENUM_INDENTATION_TYPES.SPACES, iSpaceLength)
            Dim lFormatedSourceLines As New List(Of String)
            Using mSR As New IO.StringReader(sFormatedSource)
                Dim sLine As String
                While True
                    sLine = mSR.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    lFormatedSourceLines.Add(sLine)
                End While
            End Using

            If (lRealSourceLines.Count <> lFormatedSourceLines.Count) Then
                Throw New ArgumentException("Formated number of lines are not equal with document number of lines")
            End If

            Try
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.StartUndoGroup()

                For i = lFormatedSourceLines.Count - 1 To 0 Step -1
                    Dim mLineSeg = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineSegment(i)
                    Dim sLine As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetText(mLineSeg.Offset, mLineSeg.Length)

                    If (Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset) AndAlso
                            Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset + mLineSeg.Length)) Then
                        Continue For
                    End If

                    If (sLine = lFormatedSourceLines(i)) Then
                        Continue For
                    End If

                    g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Remove(mLineSeg.Offset, mLineSeg.Length)
                    g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Insert(mLineSeg.Offset, lFormatedSourceLines(i))
                Next
            Finally
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.EndUndoGroup()

                g_ClassTabControl.m_ActiveTab.m_TextEditor.InvalidateTextArea()
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ToolsSearchReplace_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsSearchReplace.Click
        If (g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
            g_ClassTextEditorTools.ShowSearchAndReplace(g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SelectedText)
        Else
            g_ClassTextEditorTools.ShowSearchAndReplace("")
        End If
    End Sub

    Private Sub ToolStripMenuItem_ToolsShowInformation_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsShowInformation.Click
        If (ToolStripMenuItem_ViewDetails.Checked) Then
            SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False

            If (SplitContainer_ToolboxSourceAndDetails.SplitterDistance > (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)) Then
                SplitContainer_ToolboxSourceAndDetails.SplitterDistance = (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)
            End If

            If (TabControl_Details.SelectedTab IsNot TabPage_Information) Then
                TabControl_Details.SelectTab(TabPage_Information)
            End If
        End If
    End Sub

    Private Sub ToolStripMenuItem_ToolsClearInformationLog_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsClearInformationLog.Click
        g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Information log cleaned!", True, True)
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocompleteUpdate_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocompleteUpdate.Click
        Dim mActiveTab = g_ClassTabControl.m_ActiveTab
        If (mActiveTab.m_IsUnsaved OrElse mActiveTab.m_InvalidFile) Then
            MessageBox.Show("Invalid source file. Please save your source first.", "Unable to update", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL, mActiveTab.m_Identifier, ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS.FORCE_UPDATE)
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocompleteUpdateAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocompleteUpdateAll.Click
        g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL, g_ClassTabControl.m_ActiveTab, ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS.FORCE_UPDATE)
        For j = 0 To g_ClassTabControl.m_TabsCount - 1
            g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL, g_ClassTabControl.m_Tab(j), ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS.FORCE_UPDATE)
        Next
    End Sub

    Private Sub ToolStripComboBox_ToolsAutocompleteSyntax_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndexChanged
        If (g_bIgnoreComboBoxEvent) Then
            Return
        End If

        Select Case (ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndex)
            Case 0
                ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_MIX
            Case 1
                ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_6
            Case 2
                ClassSettings.g_iSettingsEnforceSyntax = ClassSettings.ENUM_ENFORCE_SYNTAX.SP_1_7
        End Select

        g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL, g_ClassTabControl.m_ActiveTab, ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS.FORCE_UPDATE)
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocompleteShowAutocomplete_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocompleteShowAutocomplete.Click
        If (ToolStripMenuItem_ViewDetails.Checked) Then
            SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False

            If (SplitContainer_ToolboxSourceAndDetails.SplitterDistance > (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)) Then
                SplitContainer_ToolboxSourceAndDetails.SplitterDistance = (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)
            End If

            If (TabControl_Details.SelectedTab IsNot TabPage_Autocomplete) Then
                TabControl_Details.SelectTab(TabPage_Autocomplete)
            End If
        End If
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocomplete_DropDownOpening(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocomplete.DropDownOpening
        Dim sLanguage As String

        Select Case (g_ClassTabControl.m_ActiveTab.m_Language)
            Case ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN
                sLanguage = "SourcePawn"
            Case ClassSyntaxTools.ENUM_LANGUAGE_TYPE.AMXMODX
                sLanguage = "AMX Mod X"
            Case ClassSyntaxTools.ENUM_LANGUAGE_TYPE.PAWN
                sLanguage = "Pawn"
            Case Else
                sLanguage = "Unknown"
        End Select


        ToolStripMenuItem_ToolsAutocompleteCurrentMod.Text = String.Format("Current language: {0}", sLanguage)
    End Sub

    Private Sub ToolStripMenuItem_BookmarksAdd_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_BookmarksAdd.Click
        g_mUCBookmarkDetails.ToolStripMenuItem_AddBookmark.PerformClick()
    End Sub

    Private Sub ToolStripMenuItem_BookmarksRemoveLines_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_BookmarksRemoveLines.Click
        Try
            Dim mTab = g_ClassTabControl.m_ActiveTab
            If (mTab.m_IsUnsaved) Then
                Return
            End If

            Dim iCaretLine = mTab.m_TextEditor.ActiveTextAreaControl.Caret.Line

            For Each mBookmark In g_mUCBookmarkDetails.g_ClassBookmarks.GetBookmarks(mTab.m_File)
                If (mBookmark.iLine <> iCaretLine) Then
                    Continue For
                End If

                g_mUCBookmarkDetails.g_ClassBookmarks.RemoveBookmark(mBookmark)
                g_mUCBookmarkDetails.g_ClassBookmarks.RefreshBookmarks()

                g_mUCBookmarkDetails.RefreshBookmarkList()
                g_mUCBookmarkDetails.RefreshBookmarkIconBar()
                Exit For
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_BookmarksShow_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_BookmarksShow.Click
        If (ToolStripMenuItem_ViewDetails.Checked) Then
            SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False

            If (SplitContainer_ToolboxSourceAndDetails.SplitterDistance > (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)) Then
                SplitContainer_ToolboxSourceAndDetails.SplitterDistance = (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)
            End If

            If (TabControl_Details.SelectedTab IsNot TabPage_Bookmarks) Then
                TabControl_Details.SelectTab(TabPage_Bookmarks)
            End If
        End If
    End Sub
End Class
