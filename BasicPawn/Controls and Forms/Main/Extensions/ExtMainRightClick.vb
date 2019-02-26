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


Partial Public Class FormMain
    Private Sub ToolStripMenuItem_Mark_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Mark.Click
        g_ClassTextEditorTools.MarkSelectedWord()
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
        With New ICSharpCode.TextEditor.Actions.ToggleAllFoldings
            .Execute(g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea)
        End With
    End Sub

    Private Sub ToolStripMenuItem_OutlineCollapseAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OutlineCollapseAll.Click
        For Each iItem In g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.FoldingManager.FoldMarker
            iItem.IsFolded = True
        Next

        g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty)
    End Sub

    Private Sub ToolStripMenuItem_Comment_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Comment.Click
        With New ICSharpCode.TextEditor.Actions.ToggleComment
            .Execute(g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea)
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointInsert.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorInsertAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointRemove.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorRemoveAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointRemoveAll.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorRemoveAll()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherInsert.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorInsertAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherRemove.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorRemoveAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherRemoveAll.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorRemoveAll()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerAssertInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerAssertInsert.Click
        With New ClassDebuggerParser.ClassAsserts(Me)
            .TextEditorInsertAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerAssertRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerAssertRemove.Click
        With New ClassDebuggerParser.ClassAsserts(Me)
            .TextEditorRemoveAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerAssertRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerAssertRemoveAll.Click
        With New ClassDebuggerParser.ClassAsserts(Me)
            .TextEditorRemoveAll()
        End With
    End Sub
End Class
