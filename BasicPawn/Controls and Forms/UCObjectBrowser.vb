'BasicPawn
'Copyright(C) 2016 TheTimocop

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

Public Class UCObjectBrowser
    Private g_mFormMain As FormMain

    Public Sub New(f As FormMain)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        g_mFormMain = f
    End Sub

    Private g_lObjectsItems As New List(Of STRUC_OBJECTS_ITEM)

    Private g_sSourceMainFileExt As String() = {".sp", ".sma", ".pwn", ".p", "BasicPawn.exe"}

    Class STRUC_OBJECTS_ITEM
        Public g_sFile As String
        Public g_sType As String
        Public g_sName As String
        Public g_sNodeKey As String

        Public Sub New(sFile As String, sType As String, sName As String, sNodeKey As String)
            g_sFile = sFile
            g_sType = sType
            g_sName = sName
            g_sNodeKey = sNodeKey
        End Sub
    End Class

    Public Sub UpdateTreeView()
        Dim lExitObjectsKeys As New List(Of String)

        TreeView_ObjectBrowser.BeginUpdate()

        ' Add tree view nodes
        g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.ForEach(
            Sub(iItem As FormMain.STRUC_AUTOCOMPLETE)
                If ((iItem.mType And FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) Then
                    Return
                End If

                Dim treeNodesFiles As TreeNode()
                Dim treeNodesTypes As TreeNode()
                Dim treeNodesNames As TreeNode()
                Dim treeNodeFile As TreeNode
                Dim treeNodeType As TreeNode
                Dim treeNodeName As TreeNode
                Dim sFileKey As String = iItem.sFile.ToLower
                Dim sTypeKey As String = String.Format("{0}|{1}", iItem.sFile.ToLower, iItem.GetTypeFullNames.ToLower)
                Dim sNameKey As String = String.Format("{0}|{1}|{2}", iItem.sFile.ToLower, iItem.GetTypeFullNames.ToLower, iItem.sFunctionName.ToLower)

                ' Add file if not exist
                treeNodesFiles = TreeView_ObjectBrowser.Nodes.Find(sFileKey, False)
                If (treeNodesFiles.Length > 0) Then
                    treeNodeFile = treeNodesFiles(0)
                Else
                    ' Add SP to top
                    If (Array.Exists(g_sSourceMainFileExt, Function(j As String) iItem.sFile.ToLower.EndsWith(j.ToLower))) Then
                        treeNodeFile = TreeView_ObjectBrowser.Nodes.Insert(0, sFileKey, iItem.sFile.ToLower)
                    Else
                        treeNodeFile = TreeView_ObjectBrowser.Nodes.Add(sFileKey, iItem.sFile.ToLower)
                    End If
                End If

                ' Add types if not exist
                treeNodesTypes = treeNodeFile.Nodes.Find(sTypeKey, False)
                If (treeNodesTypes.Length > 0) Then
                    treeNodeType = treeNodesTypes(0)
                Else
                    treeNodeType = treeNodeFile.Nodes.Add(sTypeKey, If(String.IsNullOrEmpty(iItem.GetTypeFullNames), "private", iItem.GetTypeFullNames.ToLower))
                End If

                ' Add name if not exist
                treeNodesNames = treeNodeType.Nodes.Find(sNameKey, False)
                If (treeNodesNames.Length > 0) Then
                    treeNodeName = treeNodesNames(0)
                Else
                    g_lObjectsItems.Add(New STRUC_OBJECTS_ITEM(iItem.sFile, iItem.GetTypeFullNames, iItem.sFunctionName, sNameKey))

                    treeNodeName = treeNodeType.Nodes.Add(sNameKey, iItem.sFunctionName)

                    ' Make SP files normal, others italic for better view
                    If (Array.Exists(g_sSourceMainFileExt, Function(j As String) iItem.sFile.ToLower.EndsWith(j.ToLower))) Then
                        treeNodeFile.NodeFont = New Font(TreeView_ObjectBrowser.Font, FontStyle.Regular)
                        treeNodeFile.ExpandAll()
                    Else
                        treeNodeFile.NodeFont = New Font(TreeView_ObjectBrowser.Font, FontStyle.Italic)
                    End If

                End If

                lExitObjectsKeys.Add(sNameKey)
            End Sub)

        ' Remove invalid/unused nodes
        g_lObjectsItems.RemoveAll(Function(iItem As STRUC_OBJECTS_ITEM)
                                      Dim itemKey As String = lExitObjectsKeys.Find(Function(sKey As String) sKey = iItem.g_sNodeKey)
                                      If (itemKey Is Nothing) Then
                                          For Each treeNode As TreeNode In TreeView_ObjectBrowser.Nodes.Find(iItem.g_sNodeKey, True)
                                              TreeView_ObjectBrowser.Nodes.Remove(treeNode)
                                          Next

                                          Return True
                                      Else
                                          Return False
                                      End If
                                  End Function)

        ' Cleanup unused tree view nodes
        Dim firstNodes As TreeNodeCollection = TreeView_ObjectBrowser.Nodes
        For i = firstNodes.Count - 1 To 0 Step -1
            Dim secondNodes As TreeNodeCollection = firstNodes(i).Nodes
            For j = secondNodes.Count - 1 To 0 Step -1
                If (secondNodes(j).Nodes.Count < 1) Then
                    TreeView_ObjectBrowser.Nodes.Remove(secondNodes(j))
                End If
            Next

            If (firstNodes(i).Nodes.Count < 1) Then
                TreeView_ObjectBrowser.Nodes.Remove(firstNodes(i))
            End If
        Next

        TreeView_ObjectBrowser.EndUpdate()
    End Sub

    Private Sub TextboxWatermark_Search_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles TextboxWatermark_Search.PreviewKeyDown
        If (e.KeyCode <> Keys.Enter) Then
            Return
        End If

        Dim sSearchText As String = TextboxWatermark_Search.Text
        Dim objectItem = g_lObjectsItems.Find(Function(objectsItem As STRUC_OBJECTS_ITEM) objectsItem.g_sFile.ToLower.Contains(sSearchText.ToLower) OrElse objectsItem.g_sName.ToLower.Contains(sSearchText.ToLower))
        If (objectItem Is Nothing) Then
            Return
        End If

        Dim treeNode As TreeNode() = TreeView_ObjectBrowser.Nodes.Find(objectItem.g_sNodeKey, True)
        If (Not treeNode.Length > 0) Then
            Return
        End If

        TreeView_ObjectBrowser.SelectedNode = treeNode(0)
        TreeView_ObjectBrowser.SelectedNode.EnsureVisible()
    End Sub

    Private Sub ContextMenuStrip_ObjectBrowser_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_ObjectBrowser.Opening
        If (TreeView_ObjectBrowser.SelectedNode Is Nothing) Then
            Return
        End If

        ToolStripMenuItem_OpenFile.Enabled = (TreeView_ObjectBrowser.SelectedNode.Level = 0)
        ToolStripMenuItem_ListReferences.Enabled = (TreeView_ObjectBrowser.SelectedNode.Level = 2 AndAlso System.Text.RegularExpressions.Regex.IsMatch(TreeView_ObjectBrowser.SelectedNode.Text, "^[a-zA-Z0-9_]+$"))
        ToolStripMenuItem_Copy.Enabled = (TreeView_ObjectBrowser.SelectedNode.Level = 2 OrElse TreeView_ObjectBrowser.SelectedNode.Level = 0)
    End Sub

    Private Sub ToolStripMenuItem_OpenFile_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OpenFile.Click
        Try
            If (TreeView_ObjectBrowser.SelectedNode Is Nothing OrElse TreeView_ObjectBrowser.SelectedNode.Level <> 0) Then
                Return
            End If

            For Each sPath As String In g_mFormMain.g_ClassAutocompleteUpdater.GetIncludeFiles(ClassSettings.g_sConfigOpenedSourceFile)
                If (IO.Path.GetFileName(sPath).ToLower <> TreeView_ObjectBrowser.SelectedNode.Text.ToLower) Then
                    Continue For
                End If

                Process.Start(Application.ExecutablePath, String.Format("""{0}""", sPath))
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ListReferences_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ListReferences.Click
        Try
            If (TreeView_ObjectBrowser.SelectedNode Is Nothing OrElse TreeView_ObjectBrowser.SelectedNode.Level <> 2) Then
                Return
            End If

            g_mFormMain.g_ClassTextEditorTools.ListReferences(TreeView_ObjectBrowser.SelectedNode.Text)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Copy_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Copy.Click
        Try
            If (TreeView_ObjectBrowser.SelectedNode Is Nothing) Then
                Return
            End If

            If (TreeView_ObjectBrowser.SelectedNode.Level <> 2 AndAlso TreeView_ObjectBrowser.SelectedNode.Level <> 0) Then
                Return
            End If

            My.Computer.Clipboard.SetText(TreeView_ObjectBrowser.SelectedNode.Text)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub TreeView_ObjectBrowser_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TreeView_ObjectBrowser.MouseDoubleClick
        Try
            If (TreeView_ObjectBrowser.SelectedNode Is Nothing OrElse TreeView_ObjectBrowser.SelectedNode.Level <> 2) Then
                Return
            End If

            g_mFormMain.g_ClassTextEditorTools.ListReferences(TreeView_ObjectBrowser.SelectedNode.Text)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
