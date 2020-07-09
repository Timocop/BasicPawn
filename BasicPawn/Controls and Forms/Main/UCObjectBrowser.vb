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



Imports ICSharpCode.TextEditor

Public Class UCObjectBrowser
    Private g_mFormMain As FormMain
    Private g_mUpdateThread As Threading.Thread
    Private g_bUpdateThreadUIUpdateCount As Integer = 0

    Private g_mSelectedItemsQueue As New Queue(Of TreeNode)
    Private g_bLoaded As Boolean = False

    Private g_bUpdateRequests As Boolean = False

    Public Shared g_bWndProcBug As Boolean = False
    Private Shared g_sSourceMainFileExt As String() = {".sp", ".sma", ".pwn", ".p", ".src"}

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.  
        TreeView_ObjectBrowser.TreeViewNodeSorter = New ClassAutocompleteTreeNodeSorter

        AddHandler g_mFormMain.g_ClassSyntaxParser.OnSyntaxParseSuccess, AddressOf OnSyntaxParseSuccess

        ClassTools.ClassForms.SetDoubleBuffering(TreeView_ObjectBrowser, True)
    End Sub

    ReadOnly Property m_UpdateRequests As Boolean
        Get
            Return g_bUpdateRequests
        End Get
    End Property

    Private Sub UCObjectBrowser_Load(sender As Object, e As EventArgs) Handles Me.Load
        g_bLoaded = True
    End Sub

    ReadOnly Property m_IsUpdating As Boolean
        Get
            Return ClassThread.IsValid(g_mUpdateThread)
        End Get
    End Property

    Public Function StartUpdate() As Boolean
        If (ClassThread.IsValid(g_mUpdateThread)) Then
            Return False
        End If

        g_mUpdateThread = New Threading.Thread(Sub()
                                                   Try
                                                       UpdateTreeViewThread()
                                                   Catch ex As Threading.ThreadAbortException
                                                       Throw
                                                   Catch ex As Exception
                                                       ClassExceptionLog.WriteToLog(ex)
                                                   End Try
                                               End Sub) With {
            .Priority = Threading.ThreadPriority.Lowest,
            .IsBackground = True
        }
        g_mUpdateThread.Start()

        Return True
    End Function

    Public Function StartUpdateSchedule() As Boolean
        If (StartUpdate()) Then
            g_bUpdateRequests = False

            Return True
        Else
            g_bUpdateRequests = True

            Return False
        End If
    End Function

    Public Sub StopUpdate()
        ClassThread.Abort(g_mUpdateThread)
    End Sub

    Private Sub UpdateTreeViewThread()
        Try
            Dim bIsDisabled As Boolean = False
            Dim bWndProcBug As Boolean = g_bWndProcBug

            Try

                Dim mTab As ClassTabControl.ClassTab = ClassThread.ExecEx(Of ClassTabControl.ClassTab)(Me, Function() g_mFormMain.g_ClassTabControl.m_ActiveTab)
                If (mTab Is Nothing) Then
                    Return
                End If

                Dim bUnsavedFile As Boolean = mTab.m_IsUnsaved
                Dim lAutocompleteList As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)(mTab.m_AutocompleteGroup.m_AutocompleteItems.ToArray)

                'Remove invalid nodes 
                While True
                    If (Not bUnsavedFile AndAlso lAutocompleteList.Count < 1) Then
                        Exit While
                    End If

                    Dim mFileNodes As TreeNodeCollection = ClassThread.ExecEx(Of TreeNodeCollection)(Me, Function() TreeView_ObjectBrowser.Nodes)
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
                                    ClassThread.ExecEx(Of Object)(Me, Sub()
                                                                          DisableObjectBrowserOnce(bWndProcBug, bIsDisabled)

                                                                          mNameNodes(l).Remove()
                                                                      End Sub)
                                    Continue For
                                End If

                                Dim bOutdatedNode As Boolean = False
                                Dim mAutocomplete = lAutocompleteList.Find(Function(m As ClassSyntaxTools.STRUC_AUTOCOMPLETE) m.m_Filename = mTreeNodeAutocomplete.m_Autocomplete.m_Filename AndAlso
                                                                                                                                     m.m_Type = mTreeNodeAutocomplete.m_Autocomplete.m_Type AndAlso
                                                                                                                                     m.m_FunctionString = mTreeNodeAutocomplete.m_Autocomplete.m_FunctionString)

                                If (mAutocomplete IsNot Nothing) Then
                                    'Validate if the data changed
                                    If (mAutocomplete.m_Data.Count = mTreeNodeAutocomplete.m_Autocomplete.m_Data.Count) Then
                                        Dim val As Object = Nothing

                                        While True
                                            For Each mItem In mAutocomplete.m_Data
                                                If (mTreeNodeAutocomplete.m_Autocomplete.m_Data.TryGetValue(mItem.Key, val)) Then
                                                    'Simplefied, most items are int, bool and strings anways
                                                    If (mItem.Value.ToString <> val.ToString) Then
                                                        bOutdatedNode = True
                                                        Exit While
                                                    End If
                                                Else
                                                    bOutdatedNode = True
                                                    Exit While
                                                End If
                                            Next

                                            For Each mItem In mTreeNodeAutocomplete.m_Autocomplete.m_Data
                                                If (mAutocomplete.m_Data.TryGetValue(mItem.Key, val)) Then
                                                    'Simplefied, most items are int, bool and strings anways
                                                    If (mItem.Value.ToString <> val.ToString) Then
                                                        bOutdatedNode = True
                                                        Exit While
                                                    End If
                                                Else
                                                    bOutdatedNode = True
                                                    Exit While
                                                End If
                                            Next

                                            Exit While
                                        End While
                                    Else
                                        bOutdatedNode = True
                                    End If
                                Else
                                    bOutdatedNode = True
                                End If

                                If (bOutdatedNode) Then
                                    ClassThread.ExecEx(Of Object)(Me, Sub()
                                                                          DisableObjectBrowserOnce(bWndProcBug, bIsDisabled)

                                                                          mNameNodes(l).Remove()
                                                                      End Sub)
                                End If
                            Next

                            If (mTypeNodes(j).Nodes.Count < 1) Then
                                ClassThread.ExecEx(Of Object)(Me, Sub()
                                                                      DisableObjectBrowserOnce(bWndProcBug, bIsDisabled)

                                                                      mTypeNodes(j).Remove()
                                                                  End Sub)
                            End If
                        Next

                        If (mFileNodes(i).Nodes.Count < 1) Then
                            ClassThread.ExecEx(Of Object)(Me, Sub()
                                                                  DisableObjectBrowserOnce(bWndProcBug, bIsDisabled)

                                                                  mFileNodes(i).Remove()
                                                              End Sub)
                        End If
                    Next

                    Exit While
                End While

                While True
                    If (lAutocompleteList.Count < 1) Then
                        Exit While
                    End If

                    Dim mFileNodes As TreeNodeCollection = ClassThread.ExecEx(Of TreeNodeCollection)(Me, Function() TreeView_ObjectBrowser.Nodes)
                    Dim mTypeNodes As TreeNodeCollection
                    Dim mNameNodes As TreeNodeCollection

                    Dim i As Integer
                    For i = 0 To lAutocompleteList.Count - 1
                        If (Not ClassSettings.g_bSettingsObjectBrowserShowVariables) Then
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
                        Dim sSafeFilename As String = ClassTools.ClassStrings.ToSafeKey(sFilename)

                        If (Not mFileNodes.ContainsKey(sSafeFilename)) Then
                            ClassThread.ExecEx(Of Object)(Me, Sub()
                                                                  DisableObjectBrowserOnce(bWndProcBug, bIsDisabled)

                                                                  mFileNodes.Add(New TreeNode(sFilename) With {
                                                                        .Name = sSafeFilename,
                                                                        .Tag = If(bIsMainFile, "*", "") & sFilename,
                                                                        .NodeFont = New Font(TreeView_ObjectBrowser.Font, If(bIsMainFile, FontStyle.Regular, FontStyle.Italic))
                                                                    })
                                                              End Sub)
                        End If

                        mTypeNodes = mFileNodes(sSafeFilename).Nodes

                        Dim iTypes As Integer = mAutocomplete.m_Type
                        Dim sTypes As String = If(String.IsNullOrEmpty(mAutocomplete.GetTypeFullNames), "private", mAutocomplete.GetTypeFullNames.ToLower)
                        Dim sSafeType As String = ClassTools.ClassStrings.ToSafeKey(sTypes)

                        If (Not mTypeNodes.ContainsKey(sSafeType)) Then
                            ClassThread.ExecEx(Of Object)(Me, Sub()
                                                                  DisableObjectBrowserOnce(bWndProcBug, bIsDisabled)

                                                                  mTypeNodes.Add(New TreeNode(sTypes) With {
                                                                        .Name = sSafeType,
                                                                        .Tag = sTypes
                                                                    })
                                                              End Sub)
                        End If

                        mNameNodes = mTypeNodes(sSafeType).Nodes

                        Dim sName As String = mAutocomplete.m_FunctionString
                        Dim sSafeName As String = ClassTools.ClassStrings.ToSafeKey(sName)

                        If (Not mNameNodes.ContainsKey(sSafeName)) Then
                            ClassThread.ExecEx(Of Object)(Me, Sub()
                                                                  DisableObjectBrowserOnce(bWndProcBug, bIsDisabled)

                                                                  mNameNodes.Add(New ClassTreeNodeAutocomplete(sName, sSafeName, mAutocomplete) With {
                                                                        .Tag = sName
                                                                    })
                                                              End Sub)
                        End If
                    Next

                    Exit While
                End While

                lAutocompleteList = Nothing

            Catch ex As Threading.ThreadAbortException
                Throw
            Finally
                If (bIsDisabled) Then
                    ClassThread.ExecAsync(TreeView_ObjectBrowser, Sub() EnableObjectBrowserRender(bWndProcBug, True))
                End If
            End Try
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub DisableObjectBrowserOnce(bWndProcBug As Boolean, ByRef bIsDisabled As Boolean)
        If (bIsDisabled) Then
            Return
        End If

        bIsDisabled = True
        EnableObjectBrowserRender(bWndProcBug, False)
    End Sub

    Private Sub EnableObjectBrowserRender(bWndProcBug As Boolean, bEnable As Boolean)
        If (bEnable) Then
            If (bWndProcBug) Then
                If (g_bUpdateThreadUIUpdateCount > 0) Then
                    g_bUpdateThreadUIUpdateCount -= 1
                End If

                If (g_bUpdateThreadUIUpdateCount = 0) Then
                    TreeView_ObjectBrowser.Enabled = True
                    TreeView_ObjectBrowser.Visible = True
                End If
            Else
                If (g_bUpdateThreadUIUpdateCount > 0) Then
                    g_bUpdateThreadUIUpdateCount -= 1
                End If

                If (g_bUpdateThreadUIUpdateCount = 0) Then
                    TreeView_ObjectBrowser.Enabled = True
                    TreeView_ObjectBrowser.EndUpdate()
                End If
            End If
        Else
            If (bWndProcBug) Then
                If (g_bUpdateThreadUIUpdateCount = 0) Then
                    TreeView_ObjectBrowser.Visible = False
                    TreeView_ObjectBrowser.Enabled = False
                End If

                g_bUpdateThreadUIUpdateCount += 1
            Else
                If (g_bUpdateThreadUIUpdateCount = 0) Then
                    TreeView_ObjectBrowser.BeginUpdate()
                    TreeView_ObjectBrowser.Enabled = False
                End If

                g_bUpdateThreadUIUpdateCount += 1
            End If
        End If
    End Sub

    Private Sub OnSyntaxParseSuccess(iUpdateType As ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS, sTabIdentifier As String, iOptionFlags As ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS, iFullParseError As ClassSyntaxParser.ENUM_PARSE_ERROR, iVarParseError As ClassSyntaxParser.ENUM_PARSE_ERROR)
        Try
            Static sLastTabIndentifier As String = ""

            If ((iUpdateType And ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.FULL_PARSE) = 0) Then
                Return
            End If

            Dim sActiveTabIdentifier As String = ClassThread.ExecEx(Of String)(g_mFormMain, Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Identifier)
            If (sActiveTabIdentifier <> sTabIdentifier) Then
                Return
            End If

            Select Case (iFullParseError)
                Case ClassSyntaxParser.ENUM_PARSE_ERROR.UNCHANGED,
                        ClassSyntaxParser.ENUM_PARSE_ERROR.CACHED
                    'Do not update unchanged tabs more than once
                    If (sActiveTabIdentifier = sLastTabIndentifier) Then
                        Return
                    End If

                    sLastTabIndentifier = sActiveTabIdentifier

                Case ClassSyntaxParser.ENUM_PARSE_ERROR.UPDATED
                    sLastTabIndentifier = sActiveTabIdentifier

                Case Else
                    Return
            End Select

            ClassThread.ExecAsync(g_mFormMain, Sub()
                                                   g_mFormMain.g_mUCObjectBrowser.StartUpdateSchedule()
                                               End Sub)
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ClassExceptionLog.WriteToLog(ex)
        End Try
    End Sub

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

                For Each mInclude In g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IncludesGroup.m_IncludeFiles.ToArray
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

    Private Sub ToolStripMenuItem_OpenSources_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OpenSources.Click
        Try
            g_mFormMain.g_ClassTabControl.BeginUpdate()

            For Each mNode As TreeNode In TreeView_ObjectBrowser.Nodes
                If (Not Array.Exists(g_sSourceMainFileExt, Function(s As String) mNode.Text.ToLower.EndsWith(s.ToLower))) Then
                    Continue For
                End If

                For Each mInclude In g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IncludesGroup.m_IncludeFiles.ToArray
                    If (IO.Path.GetFileName(mInclude.Value).ToLower <> mNode.Text.ToLower) Then
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
            Next
        Finally
            g_mFormMain.g_ClassTabControl.EndUpdate()
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ListReferences_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ListReferences.Click
        Try
            If (TreeView_ObjectBrowser.SelectedNode Is Nothing OrElse TreeView_ObjectBrowser.SelectedNode.Level <> 2) Then
                Return
            End If

            Dim sWord As String = TreeView_ObjectBrowser.SelectedNode.Text
            Dim mReferences As ClassTextEditorTools.STRUC_REFERENCE_ITEM() = Nothing
            Select Case (g_mFormMain.g_ClassTextEditorTools.FindReferences(sWord, True, mReferences))
                Case ClassTextEditorTools.ENUM_REFERENCE_ERROR_CODE.INVALID_FILE
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find references of '{0}'! Could not get current source file!", sWord), False, True, True)
                    Return
                Case ClassTextEditorTools.ENUM_REFERENCE_ERROR_CODE.INVALID_INPUT
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find references of '{0}'! Nothing valid selected!", sWord), False, True, True)
                    Return
                Case ClassTextEditorTools.ENUM_REFERENCE_ERROR_CODE.NO_RESULT
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find references of '{0}'!", sWord), False, True, True)
                    Return
            End Select

            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("Listing references of: {0}", sWord), False, True, True)

            For Each mItem In mReferences
                Dim sMsg = (vbTab & String.Format("{0}({1}): {2}", mItem.sFile, mItem.iLine, mItem.sLine))

                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, sMsg, New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO(mItem.sFile, New Integer() {mItem.iLine}))
            Next

            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("{0} reference{1} found!", mReferences.Length, If(mReferences.Length <> 1, "s", "")), False, True, True)
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

            Dim mDefinition As ClassTextEditorTools.STRUC_DEFINITION_ITEM = Nothing
            Select Case (g_mFormMain.g_ClassTextEditorTools.FindDefinition(mAutocomplete.m_Autocomplete, mDefinition))
                Case ClassTextEditorTools.ENUM_DEFINITION_ERROR_CODE.INVALID_FILE
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'! Could not get current source file!", sWord), False, True, True)
                    Return
                Case ClassTextEditorTools.ENUM_DEFINITION_ERROR_CODE.INVALID_INPUT
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'! Nothing valid selected!", sWord), False, True, True)
                    Return
                Case ClassTextEditorTools.ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'!", sWord), False, True, True)
                    Return
            End Select

            If (mDefinition IsNot Nothing) Then
                'If not, check if file exist and search for tab
                If (IO.File.Exists(mDefinition.sFile)) Then
                    Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByFile(mDefinition.sFile)

                    'If that also fails, just open the file
                    If (mTab Is Nothing) Then
                        mTab = g_mFormMain.g_ClassTabControl.AddTab()
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

                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("Listing definitions of '{0}':", sWord))

                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("{0}({1}): {2}", mDefinition.sFile, mDefinition.iLine, mDefinition.mAutocomplete.m_FullFunctionString),
                                                          New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO(mDefinition.sFile, {mDefinition.iLine}))

                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("{0} definition found!", 1), False, True, True)
                Else
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'! Could not find file '{1}'!", sWord, mDefinition.sFile), False, True, True)
                End If
            Else
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'!", sWord), False, True, True)
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub TreeView_ObjectBrowser_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView_ObjectBrowser.AfterSelect
        UpdateListViewColors()
    End Sub

    Private Sub TreeView_ObjectBrowser_Invalidated(sender As Object, e As InvalidateEventArgs) Handles TreeView_ObjectBrowser.Invalidated
        Static bIgnoreEvent As Boolean = False
        Static mLastBackColor As Color = Color.White
        Static mLastForeColor As Color = Color.Black

        If (Not g_bLoaded OrElse bIgnoreEvent) Then
            Return
        End If

        If (TreeView_ObjectBrowser.BackColor <> mLastBackColor OrElse TreeView_ObjectBrowser.ForeColor <> mLastForeColor) Then
            mLastBackColor = TreeView_ObjectBrowser.BackColor
            mLastForeColor = TreeView_ObjectBrowser.ForeColor

            bIgnoreEvent = True
            UpdateListViewColors()
            bIgnoreEvent = False
        End If
    End Sub

    Private Sub UpdateListViewColors()
        If (g_mSelectedItemsQueue.Count < 1 AndAlso TreeView_ObjectBrowser.Nodes.Count < 1) Then
            Return
        End If

        Try
            TreeView_ObjectBrowser.SuspendLayout()

            While (g_mSelectedItemsQueue.Count > 0)
                Dim mItem = g_mSelectedItemsQueue.Dequeue

                'Reset to parent color
                mItem.ForeColor = Color.Empty
                mItem.BackColor = Color.Empty
            End While

            If (TreeView_ObjectBrowser.SelectedNode Is Nothing) Then
                Return
            End If

            Dim mForeColor As Color
            Dim mBackColor As Color
            If (ClassControlStyle.m_IsInvertedColors) Then
                'Darker Color.RoyalBlue. Orginal Color.RoyalBlue: Color.FromArgb(65, 105, 150) 
                mForeColor = Color.White
                mBackColor = Color.FromArgb(36, 59, 127)
            Else
                mForeColor = Color.Black
                mBackColor = Color.LightBlue
            End If

            If (TreeView_ObjectBrowser.SelectedNode.ForeColor <> mForeColor OrElse
                     TreeView_ObjectBrowser.SelectedNode.BackColor <> mBackColor) Then
                TreeView_ObjectBrowser.SelectedNode.ForeColor = mForeColor
                TreeView_ObjectBrowser.SelectedNode.BackColor = mBackColor

                g_mSelectedItemsQueue.Enqueue(TreeView_ObjectBrowser.SelectedNode)
            End If
        Finally
            TreeView_ObjectBrowser.ResumeLayout()
        End Try
    End Sub

    Private Sub CleanUp()
        If (g_mFormMain.g_ClassSyntaxParser IsNot Nothing) Then
            RemoveHandler g_mFormMain.g_ClassSyntaxParser.OnSyntaxParseSuccess, AddressOf OnSyntaxParseSuccess
        End If
    End Sub

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

    Class ClassTreeNodeAutocomplete
        Inherits TreeNode

        Public Sub New(sText As String, sKey As String, mAutocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Me.Text = sText
            Me.Name = sKey

            m_Autocomplete = mAutocomplete
        End Sub

        ReadOnly Property m_Autocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE
    End Class

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
