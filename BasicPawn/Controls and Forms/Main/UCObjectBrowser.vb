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

Public Class UCObjectBrowser
    Private g_mFormMain As FormMain
    Private g_mUpdateThread As Threading.Thread
    Private g_bUpdateThreadUIUpdateCount As Integer = 0

    Public Shared g_bWndProcBug As Boolean = False

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.  
        TreeView_ObjectBrowser.TreeViewNodeSorter = New ClassAutocompleteTreeNodeSorter
    End Sub

    Private Shared g_sSourceMainFileExt As String() = {".sp", ".sma", ".pwn", ".p", ".src"}

    Class ClassAutocompleteTreeNodeSorter
        Implements IComparer

        Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            Dim mTreeNodeX = DirectCast(x, TreeNode)
            Dim mTreeNodeY = DirectCast(y, TreeNode)

            If (TypeOf mTreeNodeX.Tag Is String AndAlso TypeOf mTreeNodeY.Tag Is String) Then
                Return CStr(mTreeNodeX.Tag).CompareTo(CStr(mTreeNodeY.Tag))
            End If

            Return 1
        End Function
    End Class

    ReadOnly Property m_IsUpdating As Boolean
        Get
            Return ClassThread.IsValid(g_mUpdateThread)
        End Get
    End Property

    Public Sub StartUpdate()
        If (ClassThread.IsValid(g_mUpdateThread)) Then
            Return
        End If

        g_mUpdateThread = New Threading.Thread(AddressOf UpdateTreeViewThread) With {
                .Priority = Threading.ThreadPriority.Lowest,
                .IsBackground = True
            }
        g_mUpdateThread.Start()
    End Sub

    Public Sub StopUpdate()
        ClassThread.Abort(g_mUpdateThread)
    End Sub

    Private Sub UpdateTreeViewThread()
        Try
            Dim bWndProcBug As Boolean = g_bWndProcBug

            Try
                If (bWndProcBug) Then
                    ClassThread.ExecEx(Of Object)(Me, Sub()
                                                          If (g_bUpdateThreadUIUpdateCount = 0) Then
                                                              TreeView_ObjectBrowser.Visible = False
                                                              TreeView_ObjectBrowser.Enabled = False
                                                          End If

                                                          g_bUpdateThreadUIUpdateCount += 1
                                                      End Sub)
                Else
                    ClassThread.ExecEx(Of Object)(Me, Sub()
                                                          If (g_bUpdateThreadUIUpdateCount = 0) Then
                                                              TreeView_ObjectBrowser.BeginUpdate()
                                                              TreeView_ObjectBrowser.Enabled = False
                                                          End If

                                                          g_bUpdateThreadUIUpdateCount += 1
                                                      End Sub)
                End If

                Dim mActiveTab As ClassTabControl.SourceTabPage = ClassThread.ExecEx(Of ClassTabControl.SourceTabPage)(Me, Function() g_mFormMain.g_ClassTabControl.m_ActiveTab)

                Dim lAutocompleteList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
                lAutocompleteList.AddRange(mActiveTab.m_AutocompleteItems.ToArray)

                If (True) Then
                    Dim mFileNodes As TreeNodeCollection = ClassThread.ExecEx(Of TreeNodeCollection)(TreeView_ObjectBrowser, Function() TreeView_ObjectBrowser.Nodes)
                    Dim mTypeNodes As TreeNodeCollection
                    Dim mNameNodes As TreeNodeCollection

                    Dim i As Integer
                    For i = 0 To lAutocompleteList.Count - 1
                        If (Not ClassSettings.g_iSettingsObjectBrowserShowVariables) Then
                            If ((lAutocompleteList(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0) Then
                                Continue For
                            End If
                        End If

                        'Dont display hidden enums
                        If (lAutocompleteList(i).m_Data.ContainsKey("EnumHidden")) Then
                            Continue For
                        End If

                        'Dont display 'this'
                        If (lAutocompleteList(i).m_Data.ContainsKey("IsThis")) Then
                            Continue For
                        End If

                        Dim mAutocomplete = lAutocompleteList(i)

                        'Add missing nodes
                        Dim sFilename As String = mAutocomplete.m_Filename
                        Dim bIsMainFile As Boolean = Array.Exists(g_sSourceMainFileExt, Function(s As String) sFilename.ToLower.EndsWith(s.ToLower))

                        If (Not mFileNodes.ContainsKey(sFilename)) Then
                            ClassThread.ExecEx(Of Object)(TreeView_ObjectBrowser, Sub()
                                                                                      mFileNodes.Add(New TreeNode(sFilename) With {
                                                                                          .Name = sFilename,
                                                                                          .Tag = If(bIsMainFile, "*", "") & sFilename,
                                                                                          .NodeFont = New Font(TreeView_ObjectBrowser.Font, If(bIsMainFile, FontStyle.Regular, FontStyle.Italic))
                                                                                      })
                                                                                  End Sub)
                        End If

                        mTypeNodes = mFileNodes(sFilename).Nodes

                        Dim iTypes As Integer = mAutocomplete.m_Type
                        Dim sTypes As String = If(String.IsNullOrEmpty(mAutocomplete.GetTypeFullNames), "private", mAutocomplete.GetTypeFullNames.ToLower)
                        If (Not mTypeNodes.ContainsKey(sTypes)) Then
                            ClassThread.ExecEx(Of Object)(TreeView_ObjectBrowser, Sub()
                                                                                      mTypeNodes.Add(New TreeNode(sTypes) With {
                                                                                          .Name = sTypes,
                                                                                          .Tag = sTypes
                                                                                      })
                                                                                  End Sub)
                        End If

                        mNameNodes = mTypeNodes(sTypes).Nodes

                        Dim sName As String = mAutocomplete.m_FunctionString
                        If (Not mNameNodes.ContainsKey(sName)) Then
                            ClassThread.ExecEx(Of Object)(TreeView_ObjectBrowser, Sub()
                                                                                      mNameNodes.Add(New ClassTreeNodeAutocomplete(sName, sName, mAutocomplete) With {
                                                                                          .Tag = sName
                                                                                      })
                                                                                  End Sub)
                        End If
                    Next
                End If


                'Remove invalid nodes 
                If (True) Then
                    Dim mFileNodes As TreeNodeCollection = ClassThread.ExecEx(Of TreeNodeCollection)(TreeView_ObjectBrowser, Function() TreeView_ObjectBrowser.Nodes)
                    Dim mTypeNodes As TreeNodeCollection
                    Dim mNameNodes As TreeNodeCollection

                    Dim i As Integer
                    For i = mFileNodes.Count - 1 To 0 Step -1
                        mTypeNodes = mFileNodes(i).Nodes

                        Dim j As Integer
                        For j = mTypeNodes.Count - 1 To 0 Step -1
                            mNameNodes = mTypeNodes(j).Nodes

                            Dim l As Integer
                            For l = mNameNodes.Count - 1 To 0 Step -1
                                Dim mTreeNodeAutocomplete As ClassTreeNodeAutocomplete = TryCast(mNameNodes(l), ClassTreeNodeAutocomplete)
                                If (mTreeNodeAutocomplete Is Nothing) Then
                                    ClassThread.ExecEx(Of Object)(TreeView_ObjectBrowser, Sub() mNameNodes(l).Remove())
                                    Continue For
                                End If

                                If (Not lAutocompleteList.Exists(Function(m As ClassSyntaxTools.STRUC_AUTOCOMPLETE) m.m_Filename = mTreeNodeAutocomplete.m_Autocomplete.m_Filename AndAlso m.m_Type = mTreeNodeAutocomplete.m_Autocomplete.m_Type AndAlso m.m_FunctionString = mTreeNodeAutocomplete.m_Autocomplete.m_FunctionString)) Then
                                    ClassThread.ExecEx(Of Object)(TreeView_ObjectBrowser, Sub() mNameNodes(l).Remove())
                                End If
                            Next

                            If (mTypeNodes(j).Nodes.Count < 1) Then
                                ClassThread.ExecEx(Of Object)(TreeView_ObjectBrowser, Sub() mTypeNodes(j).Remove())
                            End If
                        Next

                        If (mFileNodes(i).Nodes.Count < 1) Then
                            ClassThread.ExecEx(Of Object)(TreeView_ObjectBrowser, Sub() mFileNodes(i).Remove())
                        End If
                    Next
                End If

                lAutocompleteList = Nothing

            Catch ex As Threading.ThreadAbortException
                Throw
            Finally
                If (bWndProcBug) Then
                    ClassThread.ExecAsync(TreeView_ObjectBrowser, Sub()
                                                                      If (g_bUpdateThreadUIUpdateCount > 0) Then
                                                                          g_bUpdateThreadUIUpdateCount -= 1
                                                                      End If

                                                                      If (g_bUpdateThreadUIUpdateCount = 0) Then
                                                                          TreeView_ObjectBrowser.Enabled = True
                                                                          TreeView_ObjectBrowser.Visible = True
                                                                      End If
                                                                  End Sub)
                Else
                    ClassThread.ExecAsync(TreeView_ObjectBrowser, Sub()
                                                                      If (g_bUpdateThreadUIUpdateCount > 0) Then
                                                                          g_bUpdateThreadUIUpdateCount -= 1
                                                                      End If

                                                                      If (g_bUpdateThreadUIUpdateCount = 0) Then
                                                                          TreeView_ObjectBrowser.Enabled = True
                                                                          TreeView_ObjectBrowser.EndUpdate()
                                                                      End If
                                                                  End Sub)
                End If
            End Try
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Class ClassTreeNodeAutocomplete
        Inherits TreeNode

        Public Sub New(sText As String, sKey As String, mAutocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Me.Text = sText
            Me.Name = sKey

            m_Autocomplete = mAutocomplete
        End Sub

        ReadOnly Property m_Autocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE
    End Class

    Private Sub TextboxWatermark_Search_KeyDown(sender As Object, e As KeyEventArgs) Handles TextboxWatermark_Search.KeyDown
        If (e.KeyCode <> Keys.Enter) Then
            Return
        End If

        e.Handled = True
        e.SuppressKeyPress = True

        Dim sSearchText As String = TextboxWatermark_Search.Text
        If (String.IsNullOrEmpty(sSearchText)) Then
            Return
        End If

        Dim mTreeNodes As TreeNode() = GetAllTreeViewNodes(TreeView_ObjectBrowser)

        Dim iSelectedIndex As Integer = -1
        For i = 0 To mTreeNodes.Length - 1
            If (mTreeNodes(i).IsSelected) Then
                iSelectedIndex = i
                Exit For
            End If
        Next

        While True
            For i = iSelectedIndex + 1 To mTreeNodes.Length - 1
                If (mTreeNodes(i).Text.ToLower.Contains(sSearchText.ToLower)) Then
                    TreeView_ObjectBrowser.SelectedNode = mTreeNodes(i)
                    TreeView_ObjectBrowser.SelectedNode.EnsureVisible()
                    Exit While
                End If
            Next

            For i = 0 To mTreeNodes.Length - 1
                If (mTreeNodes(i).Text.ToLower.Contains(sSearchText.ToLower)) Then
                    TreeView_ObjectBrowser.SelectedNode = mTreeNodes(i)
                    TreeView_ObjectBrowser.SelectedNode.EnsureVisible()
                    Exit While
                End If
            Next

            Exit While
        End While
    End Sub

    Private Function GetAllTreeViewNodes(mTreeView As TreeView) As TreeNode()
        Dim lTreeNodes As New List(Of TreeNode)

        For Each mItem As TreeNode In mTreeView.Nodes
            lTreeNodes.AddRange(GetAllTreeViewNodes(mItem))
        Next

        Return lTreeNodes.ToArray
    End Function

    Private Function GetAllTreeViewNodes(mTreeNode As TreeNode) As TreeNode()
        Dim lTreeNodes As New List(Of TreeNode) From {
            mTreeNode
        }

        For Each mItem As TreeNode In mTreeNode.Nodes
            lTreeNodes.AddRange(GetAllTreeViewNodes(mItem))
        Next

        Return lTreeNodes.ToArray
    End Function

    Private Sub TreeView_ObjectBrowser_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeView_ObjectBrowser.NodeMouseClick
        Try
            'Fixes TreeView glichty focus with ContextMenuStrips
            Select Case (e.Button)
                Case MouseButtons.Right
                    TreeView_ObjectBrowser.SelectedNode = e.Node
                    ContextMenuStrip_ObjectBrowser.Show(TreeView_ObjectBrowser, e.Location)

            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub TreeView_ObjectBrowser_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeView_ObjectBrowser.NodeMouseDoubleClick
        Try
            Select Case (e.Button)
                Case MouseButtons.Left
                    If (e.Node.Level <> 2) Then
                        Return
                    End If

                    FindSelectedNodeDefinition()
            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ContextMenuStrip_ObjectBrowser_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_ObjectBrowser.Opening
        ClassControlStyle.UpdateControls(ContextMenuStrip_ObjectBrowser)

        If (TreeView_ObjectBrowser.SelectedNode Is Nothing) Then
            Return
        End If

        ToolStripMenuItem_OpenFile.Enabled = (TreeView_ObjectBrowser.SelectedNode.Level = 0)
        ToolStripMenuItem_ListReferences.Enabled = (TreeView_ObjectBrowser.SelectedNode.Level = 2 AndAlso System.Text.RegularExpressions.Regex.IsMatch(TreeView_ObjectBrowser.SelectedNode.Text, "^[a-zA-Z0-9_]+$"))
        ToolStripMenuItem_FindDefinition.Enabled = (TreeView_ObjectBrowser.SelectedNode.Level = 2)
        ToolStripMenuItem_Copy.Enabled = (TreeView_ObjectBrowser.SelectedNode.Level = 2 OrElse TreeView_ObjectBrowser.SelectedNode.Level = 0)
    End Sub

    Private Sub ToolStripMenuItem_OpenFile_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OpenFile.Click
        Try
            If (TreeView_ObjectBrowser.SelectedNode Is Nothing OrElse TreeView_ObjectBrowser.SelectedNode.Level <> 0) Then
                Return
            End If

            Try
                g_mFormMain.g_ClassTabControl.BeginUpdate()

                For Each mInclude In g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IncludeFiles.ToArray
                    If (IO.Path.GetFileName(mInclude.Value).ToLower <> TreeView_ObjectBrowser.SelectedNode.Text.ToLower) Then
                        Continue For
                    End If

                    Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByFile(mInclude.Value)
                    If (mTab IsNot Nothing) Then
                        mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)
                    Else
                        mTab = g_mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(mInclude.Value)
                        mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)
                    End If
                Next
            Finally
                g_mFormMain.g_ClassTabControl.EndUpdate()
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ListReferences_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ListReferences.Click
        Try
            If (TreeView_ObjectBrowser.SelectedNode Is Nothing OrElse TreeView_ObjectBrowser.SelectedNode.Level <> 2) Then
                Return
            End If

            g_mFormMain.g_ClassTextEditorTools.ListReferences(TreeView_ObjectBrowser.SelectedNode.Text, True)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FindDefinition_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FindDefinition.Click
        FindSelectedNodeDefinition()
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

    Private Sub ToolStripMenuItem_ExpandAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ExpandAll.Click
        TreeView_ObjectBrowser.ExpandAll()
    End Sub

    Private Sub ToolStripMenuItem_ExpandSources_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ExpandSources.Click
        For Each mNode As TreeNode In TreeView_ObjectBrowser.Nodes
            If (Not Array.Exists(g_sSourceMainFileExt, Function(s As String) mNode.Text.ToLower.EndsWith(s.ToLower))) Then
                Continue For
            End If

            mNode.ExpandAll()
        Next
    End Sub

    Private Sub ToolStripMenuItem_CollapseAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CollapseAll.Click
        TreeView_ObjectBrowser.CollapseAll()
    End Sub

    Private Sub FindSelectedNodeDefinition()
        Try
            If (TreeView_ObjectBrowser.SelectedNode Is Nothing OrElse TreeView_ObjectBrowser.SelectedNode.Level <> 2) Then
                Return
            End If

            Dim mAutocomplete = TryCast(TreeView_ObjectBrowser.SelectedNode, ClassTreeNodeAutocomplete)
            If (mAutocomplete Is Nothing) Then
                Return
            End If

            Dim sWord As String = mAutocomplete.m_Autocomplete.m_FunctionName

            Dim mDefinition As New KeyValuePair(Of String, Integer)
            If (Not g_mFormMain.g_ClassTextEditorTools.FindDefinition(mAutocomplete.m_Autocomplete, mDefinition)) Then
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'!", sWord), False, True, True)
                Return
            End If

            'If not, check if file exist and search for tab
            If (IO.File.Exists(mDefinition.Key)) Then
                Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByFile(mDefinition.Key)

                'If that also fails, just open the file
                If (mTab Is Nothing) Then
                    mTab = g_mFormMain.g_ClassTabControl.AddTab()
                    mTab.OpenFileTab(mDefinition.Key)
                    mTab.SelectTab()
                ElseIf (Not mTab.m_IsActive) Then
                    mTab.SelectTab()
                End If

                Dim iLineNum As Integer = ClassTools.ClassMath.ClampInt(0, mTab.m_TextEditor.Document.TotalNumberOfLines - 1, mDefinition.Value - 1)
                Dim iLineLen As Integer = mTab.m_TextEditor.Document.GetLineSegment(iLineNum).Length

                Dim iStart As New TextLocation(0, iLineNum)
                Dim iEnd As New TextLocation(iLineLen, iLineNum)

                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Position = iStart
                mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.ClearSelection()
                mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(iStart, iEnd)
                mTab.m_TextEditor.ActiveTextAreaControl.CenterViewOn(iLineNum, 10)

                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("Found definition of '{0}': {1}({2})", sWord, mDefinition.Key, mDefinition.Value),
                                                      New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO(mDefinition.Key, {mDefinition.Value}),
                                                      False, True, True)
            Else
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'! Could not find file!", sWord), False, True, True)
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Class ClassTreeViewFix
        Inherits TreeView

        Protected Overrides Sub WndProc(ByRef m As Message)
            Try
                MyBase.WndProc(m)
            Catch ex As Exception
                If (Not g_bWndProcBug) Then
                    g_bWndProcBug = True
                    ClassExceptionLog.WriteToLog(ex)
                End If
            End Try
        End Sub
    End Class
End Class
