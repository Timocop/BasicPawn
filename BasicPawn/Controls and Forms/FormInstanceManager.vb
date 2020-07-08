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


Public Class FormInstanceManager
    Private g_mFormMain As FormMain

    Private g_sCallerIdentifier As String = Guid.NewGuid.ToString
    Private g_mShowDelayThread As Threading.Thread

    Const ICON_INSTANCE = 0
    Const ICON_FILE = 1

    Structure STRUC_TABINFO_ITEM
        Dim sTabIndentifier As String
        Dim sTabIndex As Integer
        Dim sTabFile As String
        Dim iProcessID As Integer
    End Structure

    Public Sub New(f As FormMain)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        ClassControlStyle.SetNameFlag(Panel_FooterControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)

        g_mFormMain = f

        ImageList_Instances.Images.Clear()
        ImageList_Instances.Images.Add(CStr(ICON_INSTANCE), My.Resources.imageres_5364_16x16)
        ImageList_Instances.Images.Add(CStr(ICON_FILE), My.Resources.imageres_5306_16x16)

        TreeViewColumns_Instances.m_Columns.Add("Tab", 100)
        TreeViewColumns_Instances.m_Columns.Add("File", 400)
        TreeViewColumns_Instances.m_TreeView.ImageList = ImageList_Instances
        TreeViewColumns_Instances.m_TreeView.ShowNodeToolTips = True
        TreeViewColumns_Instances.m_TreeView.CheckBoxes = True

        AddHandler TreeViewColumns_Instances.m_TreeView.NodeMouseDoubleClick, AddressOf TreeViewColumns_Instances_NodeMouseDoubleClick
        AddHandler TreeViewColumns_Instances.m_TreeView.NodeMouseClick, AddressOf TreeViewColumns_Instances_NodeMouseClick

        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)
    End Sub

    Public Sub AddTreeViewItem(sTabIdentifier As String, iTabIndex As Integer, sTabFile As String, sProcessName As String, iProcessId As Integer, Optional sCallerIdentifier As String = Nothing)
        If (sCallerIdentifier IsNot Nothing AndAlso sCallerIdentifier <> g_sCallerIdentifier) Then
            Return
        End If

        If (String.IsNullOrEmpty(sTabFile) OrElse Not IO.File.Exists(sTabFile)) Then
            Return
        End If

        Dim mTreeNode = FindOrCreateInstanceNode(iProcessId)
        mTreeNode.Nodes.Add(New ClassTreeNodeFile(iProcessId, sTabIdentifier, iTabIndex, sTabFile))

        If (Not mTreeNode.IsExpanded) Then
            mTreeNode.Expand()
        End If
    End Sub

    Public Sub RefreshList()
        g_sCallerIdentifier = Guid.NewGuid.ToString

        TreeViewColumns_Instances.m_TreeView.Nodes.Clear()

        Dim iPID As Integer = Process.GetCurrentProcess.Id
        Dim sProcessName As String = Process.GetCurrentProcess.ProcessName

        'Send request to other BasicPawn instances
        g_mFormMain.g_ClassCrossAppCom.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_REQUEST_TABS, g_sCallerIdentifier), False)
    End Sub

    Private Function FindOrCreateInstanceNode(iProcessId As Integer) As ClassTreeNodeInstance
        Dim mTreeNode As ClassTreeNodeInstance

        For Each i As TreeNode In TreeViewColumns_Instances.m_TreeView.Nodes
            mTreeNode = TryCast(i, ClassTreeNodeInstance)
            If (mTreeNode Is Nothing) Then
                Continue For
            End If

            If (mTreeNode.m_ProcessId <> iProcessId) Then
                Continue For
            End If

            Return mTreeNode
        Next

        mTreeNode = New ClassTreeNodeInstance(iProcessId)
        TreeViewColumns_Instances.m_TreeView.Nodes.Add(mTreeNode)
        Return mTreeNode
    End Function

    Private Function FindCheckedAllNodes(mNodeCollection As TreeNodeCollection) As TreeNode()
        Dim l As New List(Of TreeNode)

        For Each mTreeNode As TreeNode In mNodeCollection
            If (mTreeNode.Checked) Then
                l.Add(mTreeNode)
            End If

            l.AddRange(FindCheckedAllNodes(mTreeNode.Nodes))
        Next

        Return l.ToArray
    End Function

    Private Function FindCheckedInstanceNodes(mNodeCollection As TreeNodeCollection) As ClassTreeNodeInstance()
        Dim l As New List(Of ClassTreeNodeInstance)

        For Each i As TreeNode In mNodeCollection
            Dim mTreeNode = TryCast(i, ClassTreeNodeInstance)
            If (mTreeNode IsNot Nothing) Then
                If (mTreeNode.Checked) Then
                    l.Add(mTreeNode)
                End If
            End If

            l.AddRange(FindCheckedInstanceNodes(i.Nodes))
        Next

        Return l.ToArray
    End Function

    Private Function FindCheckedFileNodes(mNodeCollection As TreeNodeCollection) As ClassTreeNodeFile()
        Dim l As New List(Of ClassTreeNodeFile)

        For Each i As TreeNode In mNodeCollection
            Dim mTreeNode = TryCast(i, ClassTreeNodeFile)
            If (mTreeNode IsNot Nothing) Then
                If (mTreeNode.Checked) Then
                    l.Add(mTreeNode)
                End If
            End If

            l.AddRange(FindCheckedFileNodes(i.Nodes))
        Next

        Return l.ToArray
    End Function

    Private Sub FormOpenTabFromInstances_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RefreshList()

        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub LinkLabel_Refresh_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_Refresh.LinkClicked
        RefreshList()
    End Sub

    Private Sub TreeViewColumns_Instances_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs)
        'Fixes TreeView glichty focus with ContextMenuStrips
        Select Case (e.Button)
            Case MouseButtons.Right
                TreeViewColumns_Instances.m_TreeView.SelectedNode = e.Node
                ContextMenuStrip_Instances.Show(TreeViewColumns_Instances.m_TreeView, e.Location)

        End Select
    End Sub

    Private Sub TreeViewColumns_Instances_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs)
        If (TreeViewColumns_Instances.m_TreeView.SelectedNode Is Nothing) Then
            Return
        End If

        Dim mTreeNode As ClassTreeNodeFile = TryCast(TreeViewColumns_Instances.m_TreeView.SelectedNode, ClassTreeNodeFile)
        If (mTreeNode Is Nothing) Then
            Return
        End If

        Dim iProcessId As Integer = mTreeNode.m_ProcessId
        Dim sTabIdentifier As String = mTreeNode.m_TabIndentifier

        ClassThread.Abort(g_mShowDelayThread)

        g_mShowDelayThread = New Threading.Thread(Sub()
                                                      Try
                                                          Threading.Thread.Sleep(500)
                                                          g_mFormMain.g_ClassCrossAppCom.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_ACTIVATE_FORM_OR_TAB, CStr(iProcessId), sTabIdentifier), False)
                                                      Catch ex As Threading.ThreadAbortException
                                                          Throw
                                                      Catch ex As Exception
                                                      End Try
                                                  End Sub) With {
            .IsBackground = True
        }
        g_mShowDelayThread.Start()
    End Sub

    Private Sub ContextMenuStrip_Instances_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Instances.Opening
        ClassControlStyle.UpdateControls(ContextMenuStrip_Instances)

        ToolStripMenuItem_CheckAll.Enabled = (TreeViewColumns_Instances.m_TreeView.SelectedNode IsNot Nothing AndAlso
                                                                TypeOf TreeViewColumns_Instances.m_TreeView.SelectedNode Is ClassTreeNodeInstance)
        ToolStripMenuItem_UncheckAll.Enabled = (FindCheckedAllNodes(TreeViewColumns_Instances.m_TreeView.Nodes).Length > 0)

        ToolStripMenuItem_CopyChecked.Enabled = (FindCheckedFileNodes(TreeViewColumns_Instances.m_TreeView.Nodes).Length > 0 AndAlso
                                                                TreeViewColumns_Instances.m_TreeView.SelectedNode IsNot Nothing AndAlso
                                                                TypeOf TreeViewColumns_Instances.m_TreeView.SelectedNode Is ClassTreeNodeInstance)
        ToolStripMenuItem_MoveChecked.Enabled = (FindCheckedFileNodes(TreeViewColumns_Instances.m_TreeView.Nodes).Length > 0 AndAlso
                                                                TreeViewColumns_Instances.m_TreeView.SelectedNode IsNot Nothing AndAlso
                                                                TypeOf TreeViewColumns_Instances.m_TreeView.SelectedNode Is ClassTreeNodeInstance)
        ToolStripMenuItem_PopoutChecked.Enabled = (FindCheckedFileNodes(TreeViewColumns_Instances.m_TreeView.Nodes).Length > 0)
        ToolStripMenuItem_CloseChecked.Enabled = (FindCheckedFileNodes(TreeViewColumns_Instances.m_TreeView.Nodes).Length > 0)

        ToolStripMenuItem_CloseInstChecked.Enabled = (FindCheckedInstanceNodes(TreeViewColumns_Instances.m_TreeView.Nodes).Length > 0)
    End Sub

    Private Sub ToolStripMenuItem_Refresh_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Refresh.Click
        RefreshList()
    End Sub

    Private Sub ToolStripMenuItem_CheckAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CheckAll.Click
        If (TreeViewColumns_Instances.m_TreeView.SelectedNode Is Nothing) Then
            Return
        End If

        Dim mTreeNode As ClassTreeNodeInstance = TryCast(TreeViewColumns_Instances.m_TreeView.SelectedNode, ClassTreeNodeInstance)
        If (mTreeNode Is Nothing) Then
            Return
        End If

        For Each i As TreeNode In mTreeNode.Nodes
            If (Not i.Checked) Then
                i.Checked = True
            End If
        Next
    End Sub

    Private Sub ToolStripMenuItem_UncheckAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_UncheckAll.Click
        For Each i As TreeNode In FindCheckedAllNodes(TreeViewColumns_Instances.m_TreeView.Nodes)
            If (i.Checked) Then
                i.Checked = False
            End If
        Next
    End Sub

    Private Sub ToolStripMenuItem_CopyChecked_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CopyChecked.Click
        Try
            If (TreeViewColumns_Instances.m_TreeView.SelectedNode Is Nothing) Then
                Return
            End If

            Dim mSelectedNode = TryCast(TreeViewColumns_Instances.m_TreeView.SelectedNode, ClassTreeNodeInstance)
            If (mSelectedNode Is Nothing) Then
                Return
            End If

            For Each mFileNode In FindCheckedFileNodes(TreeViewColumns_Instances.m_TreeView.Nodes)
                If (String.IsNullOrEmpty(mFileNode.m_TabFile)) Then
                    MessageBox.Show(String.Format("Invalid file from process id {0} tab index {1}", mFileNode.m_ProcessId, mFileNode.m_TabIndex - 1), "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                If (Not IO.File.Exists(mFileNode.m_TabFile)) Then
                    MessageBox.Show(String.Format("'{0}' does not exist!", mFileNode.m_TabFile), "File does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                g_mFormMain.g_ClassCrossAppCom.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_OPEN_FILE, CStr(mSelectedNode.m_ProcessId), mFileNode.m_TabFile), False)
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_MoveChecked_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_MoveChecked.Click
        Try
            If (TreeViewColumns_Instances.m_TreeView.SelectedNode Is Nothing) Then
                Return
            End If

            Dim mSelectedNode = TryCast(TreeViewColumns_Instances.m_TreeView.SelectedNode, ClassTreeNodeInstance)
            If (mSelectedNode Is Nothing) Then
                Return
            End If

            For Each mFileNode In FindCheckedFileNodes(TreeViewColumns_Instances.m_TreeView.Nodes)
                If (String.IsNullOrEmpty(mFileNode.m_TabFile)) Then
                    MessageBox.Show(String.Format("Invalid file from process id {0} tab index {1}", mFileNode.m_ProcessId, mFileNode.m_TabIndex - 1), "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                If (Not IO.File.Exists(mFileNode.m_TabFile)) Then
                    MessageBox.Show(String.Format("'{0}' does not exist!", mFileNode.m_TabFile), "File does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                g_mFormMain.g_ClassCrossAppCom.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_OPEN_FILE, CStr(mSelectedNode.m_ProcessId), mFileNode.m_TabFile), False)
                g_mFormMain.g_ClassCrossAppCom.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_CLOSE_TAB, CStr(mFileNode.m_ProcessId), mFileNode.m_TabIndentifier, "", CStr(False)), False)
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_PopoutChecked_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_PopoutChecked.Click
        Try
            Dim lFiles As New List(Of String)

            For Each mFileNode In FindCheckedFileNodes(TreeViewColumns_Instances.m_TreeView.Nodes)
                If (String.IsNullOrEmpty(mFileNode.m_TabFile)) Then
                    MessageBox.Show(String.Format("Invalid file from process id {0} tab index {1}", mFileNode.m_ProcessId, mFileNode.m_TabIndex - 1), "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                If (Not IO.File.Exists(mFileNode.m_TabFile)) Then
                    MessageBox.Show(String.Format("'{0}' does not exist!", mFileNode.m_TabFile), "File does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                lFiles.Add(String.Format("""{0}""", mFileNode.m_TabFile))
            Next

            If (lFiles.Count < 1) Then
                Return
            End If

            Process.Start(Application.ExecutablePath, String.Join(" ", {"-newinstance", String.Join(" ", lFiles.ToArray)}))
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_CloseChecked_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CloseChecked.Click
        Try
            For Each mFileNode In FindCheckedFileNodes(TreeViewColumns_Instances.m_TreeView.Nodes)
                If (String.IsNullOrEmpty(mFileNode.m_TabFile)) Then
                    MessageBox.Show(String.Format("Invalid file from process id {0} tab index {1}", mFileNode.m_ProcessId, mFileNode.m_TabIndex - 1), "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                If (Not IO.File.Exists(mFileNode.m_TabFile)) Then
                    MessageBox.Show(String.Format("'{0}' does not exist!", mFileNode.m_TabFile), "File does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                g_mFormMain.g_ClassCrossAppCom.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_CLOSE_TAB, CStr(mFileNode.m_ProcessId), mFileNode.m_TabIndentifier, "", CStr(False)), False)
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_CloseInstChecked_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CloseInstChecked.Click
        Try
            For Each mInstanceNode In FindCheckedInstanceNodes(TreeViewColumns_Instances.m_TreeView.Nodes)
                If (mInstanceNode.m_ProcessId = Process.GetCurrentProcess.Id) Then
                    MessageBox.Show("Can not close itself", "Unable to close", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                g_mFormMain.g_ClassCrossAppCom.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_CLOSE_APP, CStr(mInstanceNode.m_ProcessId)), False)
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub



    Private Sub Button_Close_Click(sender As Object, e As EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    Private Sub FormOpenTabFromInstances_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub CleanUp()
        ClassThread.Abort(g_mShowDelayThread)

        If (TreeViewColumns_Instances IsNot Nothing) Then
            RemoveHandler TreeViewColumns_Instances.m_TreeView.NodeMouseDoubleClick, AddressOf TreeViewColumns_Instances_NodeMouseDoubleClick
            RemoveHandler TreeViewColumns_Instances.m_TreeView.NodeMouseClick, AddressOf TreeViewColumns_Instances_NodeMouseClick
        End If
    End Sub

    Class ClassTreeNodeInstance
        Inherits TreeNode

        Property m_ProcessId As Integer

        Public Sub New(_ProcessId As Integer)
            MyBase.New()

            Me.Text = String.Format("BasicPawn Id: {0}{1}", _ProcessId, If(Process.GetCurrentProcess.Id = _ProcessId, " (Current)", ""))
            Me.ImageKey = CStr(ICON_INSTANCE)
            Me.SelectedImageKey = CStr(ICON_INSTANCE)

            m_ProcessId = _ProcessId
        End Sub
    End Class

    Class ClassTreeNodeFile
        Inherits TreeNode

        Property m_ProcessId As Integer
        Property m_TabIndentifier As String
        Property m_TabIndex As Integer
        Property m_TabFile As String

        Public Sub New(_ProcessId As Integer, _TabIndentifier As String, _TabIndex As Integer, _TabFile As String)
            MyBase.New()

            Me.Text = CStr(_TabIndex)
            Me.Tag = New String() {
                 IO.Path.GetFileName(_TabFile)
            }

            Me.ToolTipText = _TabFile
            Me.ImageKey = CStr(ICON_FILE)
            Me.SelectedImageKey = CStr(ICON_FILE)

            m_ProcessId = _ProcessId
            m_TabIndentifier = _TabIndentifier
            m_TabIndex = _TabIndex
            m_TabFile = _TabFile
        End Sub
    End Class
End Class