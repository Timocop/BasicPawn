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


Imports ICSharpCode.TextEditor

Partial Public Class FormMain
    Private Sub ToolStripMenuItem_Mark_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Mark.Click
        g_ClassTextEditorTools.MarkSelectedWord()
    End Sub

    Private Sub ToolStripMenuItem_ListReferences_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ListReferences.Click
        Try
            g_ClassTextEditorTools.ListReferences(Nothing, True)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FindDefinition_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FindDefinition.Click
        Try
            Dim sWord = g_ClassTextEditorTools.GetCaretWord(True, True, True)
            If (String.IsNullOrEmpty(sWord)) Then
                g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Could not find definition! Nothing valid selected!", False, True, True)
                Return
            End If

            Dim mTab = g_ClassTabControl.m_ActiveTab

            Dim mDefinitions As ClassTextEditorTools.STRUC_DEFINITION_ITEM() = Nothing
            If (Not g_ClassTextEditorTools.FindDefinition(mTab, sWord, mDefinitions)) Then
                g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'!", sWord), False, True, True)
                Return
            End If


            If (mDefinitions.Length = 1) Then
                Dim mDefinition = mDefinitions(0)

                'If not, check if file exist and search for tab
                If (IO.File.Exists(mDefinition.sFile)) Then
                    mTab = g_ClassTabControl.GetTabByFile(mDefinition.sFile)

                    'If that also fails, just open the file
                    If (mTab Is Nothing) Then
                        mTab = g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(mDefinition.sFile)
                        mTab.SelectTab()
                    End If

                    Dim iLineNum As Integer = ClassTools.ClassMath.ClampInt(0, mTab.m_TextEditor.Document.TotalNumberOfLines - 1, mDefinition.iLine - 1)
                    Dim iLineLen As Integer = mTab.m_TextEditor.Document.GetLineSegment(iLineNum).Length

                    Dim mStartLoc As New TextLocation(0, iLineNum)
                    Dim mEndLoc As New TextLocation(iLineLen, iLineNum)

                    mTab.m_TextEditor.ActiveTextAreaControl.Caret.Position = mStartLoc
                    mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.ClearSelection()
                    mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(mStartLoc, mEndLoc)
                    mTab.m_TextEditor.ActiveTextAreaControl.CenterViewOn(iLineNum, 10)

                    If (Not mTab.m_IsActive) Then
                        mTab.SelectTab()
                    End If

                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("Listing definitions of '{0}':", sWord))
                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("{0}({1}): {2}", mDefinition.sFile, mDefinition.iLine, mDefinition.mAutocomplete.m_FullFunctionString),
                                                      New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO(mDefinition.sFile, {mDefinition.iLine}))
                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("{0} definition found!", 1), False, True, True)
                Else
                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'! Could not find file!", sWord), False, True, True)
                End If
            Else
                g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("Listing definitions of '{0}':", sWord))

                For Each mDefinition In mDefinitions
                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("{0}({1}): {2}", mDefinition.sFile, mDefinition.iLine, mDefinition.mAutocomplete.m_FullFunctionString),
                                                      New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO(mDefinition.sFile, {mDefinition.iLine}))
                Next
                g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("{0} definition{1} found!", mDefinitions.Length, If(mDefinitions.Length > 1, "s", "")), False, True, True)
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Cut_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Cut.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_Copy_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Copy.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_Paste_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Paste.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_Delete_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Delete.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_SelectAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_SelectAll.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_OutlineExpandAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OutlineExpandAll.Click
        For Each iItem In g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.FoldingManager.FoldMarker
            iItem.IsFolded = False
        Next

        g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty)
    End Sub

    Private Sub ToolStripMenuItem_OutlineToggleAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OutlineToggleAll.Click
        Call (New ICSharpCode.TextEditor.Actions.ToggleAllFoldings).Execute(g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea)
    End Sub

    Private Sub ToolStripMenuItem_OutlineCollapseAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OutlineCollapseAll.Click
        For Each iItem In g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.FoldingManager.FoldMarker
            iItem.IsFolded = True
        Next

        g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty)
    End Sub

    Private Sub ToolStripMenuItem_Comment_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Comment.Click
        Call (New ICSharpCode.TextEditor.Actions.ToggleComment).Execute(g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea)
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointInsert.Click
        Call (New ClassDebuggerTools.ClassDebuggerEntries.ClassBreakpointEntry).TextEditorInsertAtCaret(Me)
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointRemove.Click
        Call (New ClassDebuggerTools.ClassDebuggerEntries.ClassBreakpointEntry).TextEditorRemoveAtCaret(Me)
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointRemoveAll.Click
        Call (New ClassDebuggerTools.ClassDebuggerEntries.ClassBreakpointEntry).TextEditorRemoveAll(Me)
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherInsert.Click
        Call (New ClassDebuggerTools.ClassDebuggerEntries.ClassWatcherEntry).TextEditorInsertAtCaret(Me)
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherRemove.Click
        Call (New ClassDebuggerTools.ClassDebuggerEntries.ClassWatcherEntry).TextEditorRemoveAtCaret(Me)
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherRemoveAll.Click
        Call (New ClassDebuggerTools.ClassDebuggerEntries.ClassWatcherEntry).TextEditorRemoveAll(Me)
    End Sub

    Private Sub ToolStripMenuItem_DebuggerAssertInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerAssertInsert.Click
        Call (New ClassDebuggerTools.ClassDebuggerEntries.ClassAssetEntry).TextEditorInsertAtCaret(Me)
    End Sub

    Private Sub ToolStripMenuItem_DebuggerAssertRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerAssertRemove.Click
        Call (New ClassDebuggerTools.ClassDebuggerEntries.ClassAssetEntry).TextEditorRemoveAtCaret(Me)
    End Sub

    Private Sub ToolStripMenuItem_DebuggerAssertRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerAssertRemoveAll.Click
        Call (New ClassDebuggerTools.ClassDebuggerEntries.ClassAssetEntry).TextEditorRemoveAll(Me)
    End Sub
End Class
