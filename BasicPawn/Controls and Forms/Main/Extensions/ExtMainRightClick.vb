'BasicPawn
'Copyright(C) 2021 Externet

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
Imports ICSharpCode.TextEditor.Document

Partial Public Class FormMain
    Private Sub ToolStripMenuItem_Mark_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Mark.Click
        Dim mActiveTab = g_ClassTabControl.m_ActiveTab
        Dim sTextContent As String = mActiveTab.m_TextEditor.Document.TextContent
        Dim sWord As String = ""

        If (mActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
            Dim mSelection As ISelection = mActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0)

            sWord = mSelection.SelectedText
        Else
            sWord = g_ClassTextEditorTools.GetCaretWord(mActiveTab, False, False, False)
        End If

        Dim mWordLocations As New List(Of Point)
        If (Not mActiveTab.g_ClassMarkerHighlighting.FindWordLocations(sWord, sTextContent, mWordLocations)) Then
            mActiveTab.g_ClassMarkerHighlighting.RemoveHighlighting(ClassTabControl.ClassTab.ClassMarkerHighlighting.ENUM_MARKER_TYPE.STATIC_MARKER)
            Return
        End If

        mActiveTab.g_ClassMarkerHighlighting.UpdateHighlighting(mWordLocations, ClassTabControl.ClassTab.ClassMarkerHighlighting.ENUM_MARKER_TYPE.STATIC_MARKER)
    End Sub

    Private Sub ToolStripMenuItem_ListReferences_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ListReferences.Click
        FindReferences()
    End Sub

    Private Sub ToolStripMenuItem_FindDefinition_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FindDefinition.Click
        FindDefinition(False)
    End Sub

    Private Sub ToolStripMenuItem_PeekDefinition_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_PeekDefinition.Click
        FindDefinition(True)
    End Sub

    Private Sub FindReferences()
        Try
            Dim sWord As String = Nothing
            Dim mReferences As ClassTextEditorTools.STRUC_REFERENCE_ITEM() = Nothing
            Select Case (g_ClassTextEditorTools.FindReferences(sWord, True, mReferences))
                Case ClassTextEditorTools.ENUM_REFERENCE_ERROR_CODE.INVALID_FILE
                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find references of '{0}'! Could not get current source file!", sWord), False, True, True)
                    Return
                Case ClassTextEditorTools.ENUM_REFERENCE_ERROR_CODE.INVALID_INPUT
                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find references of '{0}'! Nothing valid selected!", sWord), False, True, True)
                    Return
                Case ClassTextEditorTools.ENUM_REFERENCE_ERROR_CODE.NO_RESULT
                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find references of '{0}'!", sWord), False, True, True)
                    Return
            End Select

            g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("Listing references of: {0}", sWord), False, True, True)

            For Each mItem In mReferences
                Dim sMsg = (vbTab & String.Format("{0}({1}): {2}", mItem.sFile, mItem.iLine, mItem.sLine))

                g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, sMsg, New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO(mItem.sFile, New Integer() {mItem.iLine}))
            Next

            g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("{0} reference{1} found!", mReferences.Length, If(mReferences.Length <> 1, "s", "")), False, True, True)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub FindDefinition(bForceNewTab As Boolean)
        Try
            Dim mTab = g_ClassTabControl.m_ActiveTab

            Dim sWord As String = Nothing
            Dim mDefinitions As ClassTextEditorTools.STRUC_DEFINITION_ITEM() = Nothing
            Select Case (g_ClassTextEditorTools.FindDefinition(mTab, sWord, mDefinitions))
                Case ClassTextEditorTools.ENUM_DEFINITION_ERROR_CODE.INVALID_FILE
                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'! Could not get current source file!", sWord), False, True, True)
                    Return
                Case ClassTextEditorTools.ENUM_DEFINITION_ERROR_CODE.INVALID_INPUT
                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'! Nothing valid selected!", sWord), False, True, True)
                    Return
                Case ClassTextEditorTools.ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'!", sWord), False, True, True)

                    'If the definition is not found - like variables - list references.
                    FindReferences()
                    Return
            End Select

            If (mDefinitions.Length > 0) Then
                Dim mDefinition = mDefinitions(0)

                'If not, check if file exist and search for tab
                If (IO.File.Exists(mDefinition.sFile)) Then
                    If (bForceNewTab) Then
                        mTab = Nothing
                    Else
                        mTab = g_ClassTabControl.GetTabByFile(mDefinition.sFile)
                    End If

                    'If that also fails, just open the file
                    If (mTab Is Nothing) Then
                        mTab = g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(mDefinition.sFile)
                        mTab.SelectTab()
                    End If

                    Dim iLineNum As Integer = ClassTools.ClassMath.ClampInt(mDefinition.iLine - 1, 0, mTab.m_TextEditor.Document.TotalNumberOfLines - 1)
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

                    For Each mDefinition In mDefinitions
                        g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("{0}({1}): {2}", mDefinition.sFile, mDefinition.iLine, mDefinition.mAutocomplete.m_FullFunctionString),
                                                      New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO(mDefinition.sFile, {mDefinition.iLine}))
                    Next

                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("{0} definition{1} found!", mDefinitions.Length, If(mDefinitions.Length <> 1, "s", "")), False, True, True)
                Else
                    g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'! Could not find file '{1}'!", sWord, mDefinition.sFile), False, True, True)
                End If
            Else
                g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'!", sWord), False, True, True)

                'If the definition is not found - like variables - list references.
                FindReferences()
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
