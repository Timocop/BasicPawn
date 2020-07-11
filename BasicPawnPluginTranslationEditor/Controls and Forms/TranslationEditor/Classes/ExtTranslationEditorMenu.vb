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


Partial Public Class FormTranslationEditor
    Private Sub ToolStripMenuItem_TransAdd_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TransAdd.Click
        Try
            Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
            If (mSelectedNode Is Nothing) Then
                Return
            End If

            If (mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.NAME AndAlso
                    mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.MISSING_ITEM) Then
                Return
            End If

            Dim sName As String = mSelectedNode.m_Name
            Dim sLanguage As String = mSelectedNode.m_Language

            'If its an missing item go to parent
            If (mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.MISSING_ITEM) Then
                mSelectedNode = TryCast(mSelectedNode.Parent, ClassTranslationManager.ClassTranslationTreeNode)
                If (mSelectedNode Is Nothing) Then
                    Return
                End If
            End If

            Using i As New FormAddTranslation(sName, sLanguage, "", "", FormAddTranslation.ENUM_DIALOG_TYPE.ADD, g_ClassTranslationManager.GetKnownLangauges())
                If (i.ShowDialog(Me) = DialogResult.OK) Then
                    'Remove missing nodes and also check if translation with this language already exist.
                    If (True) Then
                        Dim mMissingTranslationNode As New List(Of ClassTranslationManager.ClassTranslationTreeNode)

                        For Each mNode In mSelectedNode.Nodes
                            Dim mLoopNode = TryCast(mNode, ClassTranslationManager.ClassTranslationTreeNode)
                            If (mLoopNode Is Nothing) Then
                                Continue For
                            End If

                            If (mLoopNode.m_Language = i.m_Langauge) Then
                                If (mLoopNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.MISSING_ITEM) Then
                                    mMissingTranslationNode.Add(mLoopNode)
                                Else
                                    Throw New ArgumentException("A translation with this language already exist!")
                                End If

                            End If
                        Next

                        For Each mNode In mMissingTranslationNode
                            mSelectedNode.Nodes.Remove(mNode)
                        Next
                    End If

                    If (i.m_Langauge = "en") Then
                        mSelectedNode.Nodes.Add(New ClassTranslationManager.ClassTranslationTreeNode(i.m_Name, i.m_Langauge, i.m_Format, i.m_Text, ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER))
                    Else
                        mSelectedNode.Nodes.Add(New ClassTranslationManager.ClassTranslationTreeNode(i.m_Name, i.m_Langauge, i.m_Format, i.m_Text, ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE))
                    End If

                    mSelectedNode.TreeView.Sort()

                    g_ClassTranslationManager.m_Changed = True
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_TransEdit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TransEdit.Click
        Try
            Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
            If (mSelectedNode Is Nothing) Then
                Return
            End If

            If (mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER AndAlso
                    mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE) Then
                Return
            End If

            Using i As New FormAddTranslation(mSelectedNode.m_Name, mSelectedNode.m_Language, mSelectedNode.m_Format, mSelectedNode.m_Text, FormAddTranslation.ENUM_DIALOG_TYPE.EDIT, g_ClassTranslationManager.GetKnownLangauges())
                If (i.ShowDialog(Me) = DialogResult.OK) Then
                    If (i.m_Langauge = "en") Then
                        mSelectedNode.SetInfo(mSelectedNode.m_Name, mSelectedNode.m_Language, i.m_Format, i.m_Text, ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER)
                    Else
                        mSelectedNode.SetInfo(mSelectedNode.m_Name, mSelectedNode.m_Language, i.m_Format, i.m_Text, ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE)
                    End If

                    g_ClassTranslationManager.m_Changed = True
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_TransRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TransRemove.Click
        Try
            Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
            If (mSelectedNode Is Nothing) Then
                Return
            End If

            If (mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER AndAlso
                    mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE) Then
                Return
            End If

            If (MessageBox.Show(String.Format("Remove language '{0}' from phrase group '{1}'?", mSelectedNode.m_Language, mSelectedNode.m_Name), "Remove phrase language", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No) Then
                Return
            End If

            mSelectedNode.Remove()

            g_ClassTranslationManager.TreeViewFillMissing(Not ToolStripMenuItem_ShowMissing.Checked)

            g_ClassTranslationManager.m_Changed = True
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_CopyLang_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CopyLang.Click
        Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
        If (mSelectedNode Is Nothing) Then
            Return
        End If

        If (mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER AndAlso
                mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE) Then
            Return
        End If

        My.Computer.Clipboard.SetText(mSelectedNode.m_Language)
    End Sub

    Private Sub ToolStripMenuItem_CopyFormat_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CopyFormat.Click
        Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
        If (mSelectedNode Is Nothing) Then
            Return
        End If

        If (mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER AndAlso
                mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE) Then
            Return
        End If

        My.Computer.Clipboard.SetText(mSelectedNode.m_Format)
    End Sub

    Private Sub ToolStripMenuItem_CopyText_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CopyText.Click
        Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
        If (mSelectedNode Is Nothing) Then
            Return
        End If

        If (mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER AndAlso
                mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE) Then
            Return
        End If

        My.Computer.Clipboard.SetText(mSelectedNode.m_Text)
    End Sub

    Private Sub ToolStripMenuItem_GroupAdd_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_GroupAdd.Click
        Try
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
            g_TreeViewColumns.m_TreeView.Nodes.Add(New ClassTranslationManager.ClassTranslationTreeNode(sNewName))

            g_ClassTranslationManager.TreeViewFillMissing(Not ToolStripMenuItem_ShowMissing.Checked)

            g_ClassTranslationManager.m_Changed = True
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_GroupRename_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_GroupRename.Click
        Try
            Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
            If (mSelectedNode Is Nothing) Then
                Return
            End If

            If (mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.NAME) Then
                Return
            End If

            Dim sNewName As String = InputBox("", "Rename Group", Nothing)
            If (String.IsNullOrEmpty(sNewName)) Then
                Return
            End If

            For Each mNode In g_TreeViewColumns.m_TreeView.Nodes
                Dim mLoopNode = TryCast(mNode, ClassTranslationManager.ClassTranslationTreeNode)
                If (mLoopNode Is Nothing) Then
                    Continue For
                End If

                If (mLoopNode.m_Name = sNewName) Then
                    Throw New ArgumentException("A group with this name already exist!")
                End If
            Next

            mSelectedNode.SetInfo(sNewName, Nothing, Nothing, Nothing, ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.NAME)

            g_ClassTranslationManager.m_Changed = True
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_GroupRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_GroupRemove.Click
        Try
            Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
            If (mSelectedNode Is Nothing) Then
                Return
            End If

            If (mSelectedNode.m_NodeType <> ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.NAME) Then
                Return
            End If

            If (MessageBox.Show(String.Format("Remove phrase group '{0}'?", mSelectedNode.m_Name), "Remove phrase group", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No) Then
                Return
            End If

            mSelectedNode.Remove()

            g_ClassTranslationManager.TreeViewFillMissing(Not ToolStripMenuItem_ShowMissing.Checked)

            g_ClassTranslationManager.m_Changed = True
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ShowMissing_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ShowMissing.Click
        g_ClassTranslationManager.TreeViewFillMissing(Not ToolStripMenuItem_ShowMissing.Checked)
    End Sub

    Private Sub ContextMenuStrip_Translation_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Translation.Opening
        ClassControlStyle.UpdateControls(ContextMenuStrip_Translation)

        Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
        If (mSelectedNode Is Nothing) Then
            Return
        End If

        ToolStripMenuItem_TransAdd.Enabled = (mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.NAME OrElse
                                                    mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.MISSING_ITEM)
        ToolStripMenuItem_TransEdit.Enabled = (mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER OrElse
                                                    mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE)
        ToolStripMenuItem_TransRemove.Enabled = (mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER OrElse
                                                    mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE)

        ToolStripMenuItem_CopyLang.Enabled = (mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER OrElse
                                                    mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE)
        ToolStripMenuItem_CopyFormat.Enabled = (mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER OrElse
                                                    mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE)
        ToolStripMenuItem_CopyText.Enabled = (mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER OrElse
                                                    mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE)

        ToolStripMenuItem_GroupRename.Enabled = (mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.NAME)
        ToolStripMenuItem_GroupRemove.Enabled = (mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.NAME)
    End Sub

End Class
