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


Imports System.Windows.Forms

Partial Public Class FormTranslationEditor
    Private Sub ToolStripMenuItem_TransAdd_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TransAdd.Click
        Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
        If (mSelectedNode Is Nothing) Then
            Return
        End If

        Using i As New FormAddTranslation(mSelectedNode.m_Name, mSelectedNode.m_Language, "", "", FormAddTranslation.ENUM_DIALOG_TYPE.ADD, g_ClassTranslationManager.GetKnownLangauges())
            If (i.ShowDialog(Me) = DialogResult.OK) Then
                MsgBox("OK")
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_TransEdit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TransEdit.Click
        Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
        If (mSelectedNode Is Nothing) Then
            Return
        End If

        If (mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER OrElse
                mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE) Then
            Return
        End If

        Using i As New FormAddTranslation(mSelectedNode.m_Name, mSelectedNode.m_Language, mSelectedNode.m_Format, mSelectedNode.m_Text, FormAddTranslation.ENUM_DIALOG_TYPE.EDIT, g_ClassTranslationManager.GetKnownLangauges())
            If (i.ShowDialog(Me) = DialogResult.OK) Then
                MsgBox("OK")
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_TransRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TransRemove.Click
        Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
        If (mSelectedNode Is Nothing) Then
            Return
        End If

        If (mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER OrElse
                mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE) Then
            Return
        End If

        Dim mKnownLangauges = g_ClassTranslationManager.GetKnownLangauges()

        Using i As New FormAddTranslation(mSelectedNode.m_Name, mSelectedNode.m_Language, mSelectedNode.m_Format, mSelectedNode.m_Text, FormAddTranslation.ENUM_DIALOG_TYPE.EDIT, g_ClassTranslationManager.GetKnownLangauges())
            If (i.ShowDialog(Me) = DialogResult.OK) Then
                MsgBox("OK")
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_GroupAdd_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_GroupAdd.Click
        Dim sNewName As String = InputBox("", "New Group", Nothing)
        If (String.IsNullOrEmpty(sNewName)) Then
            Return
        End If

        For Each mNode In g_TreeViewColumns.m_TreeView.Nodes
            Dim mLoopNode = TryCast(mNode, ClassTranslationManager.ClassTranslationTreeNode)
            If (mLoopNode Is Nothing) Then
                Continue For
            End If

            If (mLoopNode.m_Name = sNewName) Then
                MessageBox.Show("A group with this name already exist!", "Group already exist", MessageBoxButtons.OK)
                Return
            End If
        Next

        'Create new group and fill missing
        Dim mNewNode As New ClassTranslationManager.ClassTranslationTreeNode(sNewName)
        For Each mKnownLang In g_ClassTranslationManager.GetKnownLangauges()
            mNewNode.Nodes.Add(New ClassTranslationManager.ClassTranslationTreeNode(sNewName, mKnownLang.Key, "", "", ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.MISSING_ITEM))
        Next
        g_TreeViewColumns.m_TreeView.Nodes.Add(mNewNode)
    End Sub

    Private Sub ToolStripMenuItem_GroupRename_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_GroupRename.Click

    End Sub

    Private Sub ToolStripMenuItem_GroupRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_GroupRemove.Click

    End Sub

End Class
