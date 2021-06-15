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


Public Class UCProjectBrowser
    Public g_mFormMain As FormMain
    Public g_ClassProjectControl As ClassProjectControl

    Private g_lClipboardFiles As New List(Of ClassProjectControl.STRUC_PROJECT_PATH_INFO)
    Private g_mSelectedItemsQueue As New Queue(Of ListViewItem)
    Private g_bLoaded As Boolean = False

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ImageList_ProjectBrowser.ImageSize = New Size(ClassTools.ClassForms.ScaleDPI(16), ClassTools.ClassForms.ScaleDPI(16))
        ImageList_ProjectBrowser.Images.Clear()
        ImageList_ProjectBrowser.Images.Add("0", My.Resources.Ico_Rtf)
        ImageList_ProjectBrowser.Images.Add("1", My.Resources.Ico_Folder)

        g_ClassProjectControl = New ClassProjectControl(Me)

        ClassTools.ClassForms.SetDoubleBuffering(ListView_ProjectFiles, True)
    End Sub

    Private Sub UCProjectBrowser_Load(sender As Object, e As EventArgs) Handles Me.Load
        g_bLoaded = True
    End Sub

    Class ClassProjectControl
        Private g_mUCProjectBrowser As UCProjectBrowser

        Private g_sProjectFile As String = ""
        Private g_sExplorerPath As String = ""
        Private g_bProjectChanged As Boolean = False

        Public Shared ReadOnly g_sProjectExtension As String = ".bpproj"

        Class STRUC_PROJECT_PATH_INFO
            Private g_mUCProjectBrowser As UCProjectBrowser

            Enum ENUM_PATH_TYPE
                FILE
                DIRECTORY
            End Enum

            Private g_sPath As String = ""
            Private g_iPathType As ENUM_PATH_TYPE

            Public Sub New(_UCProjectBrowser As UCProjectBrowser, _Path As String, _PathType As ENUM_PATH_TYPE)
                g_mUCProjectBrowser = _UCProjectBrowser
                g_sPath = _Path
                g_iPathType = _PathType
            End Sub

            ReadOnly Property m_PathType As ENUM_PATH_TYPE
                Get
                    Return g_iPathType
                End Get
            End Property

            ReadOnly Property m_Path As String
                Get
                    Return g_sPath
                End Get
            End Property
        End Class

        Public Sub New(f As UCProjectBrowser)
            g_mUCProjectBrowser = f
        End Sub

        ReadOnly Property m_ProjectFile As String
            Get
                Return g_sProjectFile
            End Get
        End Property

        ReadOnly Property m_ProjectOpened As Boolean
            Get
                Return (Not String.IsNullOrEmpty(g_sProjectFile))
            End Get
        End Property

        ReadOnly Property m_ProjectDirectory As String
            Get
                If (Not m_ProjectOpened) Then
                    Throw New ArgumentException("No project open")
                End If

                Return IO.Path.GetDirectoryName(g_sProjectFile)
            End Get
        End Property

        ReadOnly Property m_ProjectChanged As Boolean
            Get
                Return g_bProjectChanged
            End Get
        End Property

        Property m_ExplorerPath As String
            Get
                Return g_sExplorerPath
            End Get
            Set(value As String)
                Dim sPath As String = value

                If (String.IsNullOrEmpty(sPath)) Then
                    sPath = m_ProjectDirectory
                End If

                If (IO.Path.IsPathRooted(sPath)) Then
                    If (Not sPath.ToLower.StartsWith(m_ProjectDirectory.ToLower)) Then
                        sPath = m_ProjectDirectory
                    End If
                Else
                    sPath = IO.Path.Combine(m_ProjectDirectory, sPath)
                End If

                g_sExplorerPath = sPath

                g_mUCProjectBrowser.ToolStripTextBox_MenuProjectPath.Text = g_sExplorerPath
                g_mUCProjectBrowser.ToolStripTextBox_MenuProjectPath.Select(g_mUCProjectBrowser.ToolStripTextBox_MenuProjectPath.Text.Length - 1, 0)

                g_mUCProjectBrowser.ToolStripTextBox_MenuProjectPath.ToolTipText = g_sExplorerPath
            End Set
        End Property

        Public Sub GoToExplorer(sPath As String)
            If (String.IsNullOrEmpty(sPath)) Then
                Throw New ArgumentException("Path empty")
            End If

            Select Case (True)
                Case IO.File.Exists(sPath)
                    m_ExplorerPath = IO.Path.GetFullPath(IO.Path.GetDirectoryName(sPath))

                Case IO.Directory.Exists(sPath)
                    m_ExplorerPath = IO.Path.GetFullPath(sPath)

                Case Else
                    Throw New IO.DirectoryNotFoundException("Invalid path")
            End Select

            RefreshExplorer()
        End Sub

        Public Sub RefreshExplorer()
            g_mUCProjectBrowser.ListView_ProjectFiles.Items.Clear()

            If (String.IsNullOrEmpty(m_ExplorerPath) OrElse Not IO.Directory.Exists(m_ExplorerPath)) Then
                Throw New IO.DirectoryNotFoundException("Directory not found")
            End If

            Try
                g_mUCProjectBrowser.ListView_ProjectFiles.BeginUpdate()

                For Each sDirectory As String In IO.Directory.GetDirectories(m_ExplorerPath)
                    If (Not IO.Directory.Exists(sDirectory)) Then
                        Continue For
                    End If

                    Dim sName As String = New IO.DirectoryInfo(sDirectory).Name

                    Dim mItem As New ClassListViewItemData(sName, "1")

                    mItem.g_mData("Info") = New STRUC_PROJECT_PATH_INFO(g_mUCProjectBrowser, sDirectory, STRUC_PROJECT_PATH_INFO.ENUM_PATH_TYPE.DIRECTORY)

                    g_mUCProjectBrowser.ListView_ProjectFiles.Items.Add(mItem)
                Next

                For Each sFile As String In IO.Directory.GetFiles(m_ExplorerPath)
                    If (Not IO.File.Exists(sFile)) Then
                        Continue For
                    End If

                    Select Case (IO.Path.GetExtension(sFile))
                        Case ".sp", ".sma", ".p", ".pwn", ".inc"
                            'Allow

                        Case Else
                            Continue For
                    End Select

                    Dim sName As String = New IO.FileInfo(sFile).Name

                    Dim mItem As New ClassListViewItemData(sName, "0")

                    mItem.g_mData("Info") = New STRUC_PROJECT_PATH_INFO(g_mUCProjectBrowser, sFile, STRUC_PROJECT_PATH_INFO.ENUM_PATH_TYPE.FILE)

                    g_mUCProjectBrowser.ListView_ProjectFiles.Items.Add(mItem)
                Next
            Finally
                g_mUCProjectBrowser.ListView_ProjectFiles.EndUpdate()
            End Try
        End Sub

        Public Sub HomeExplorer()
            GoToExplorer(m_ProjectDirectory)
        End Sub

        Public Sub GoUpExplorer()
            If (String.IsNullOrEmpty(m_ExplorerPath) OrElse Not IO.Directory.Exists(m_ExplorerPath)) Then
                Throw New IO.DirectoryNotFoundException("Directory not found")
            End If

            GoToExplorer(IO.Path.GetFullPath(IO.Path.Combine(m_ExplorerPath, "..")))
        End Sub

        Public Sub UpdateStatusLabel()
            Dim sNewTabPageText As String = g_mUCProjectBrowser.g_mFormMain.TabPage_ProjectBrowser.Text.TrimEnd("*"c) & If(m_ProjectChanged, "*", "")
            If (g_mUCProjectBrowser.g_mFormMain.TabPage_ProjectBrowser.Text <> sNewTabPageText) Then
                g_mUCProjectBrowser.g_mFormMain.TabPage_ProjectBrowser.Text = sNewTabPageText
            End If

            If (m_ProjectOpened) Then
                g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Text = String.Format("Project: {0}{1}", IO.Path.GetFileNameWithoutExtension(m_ProjectFile), If(m_ProjectChanged, "*", ""))
                g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.ToolTipText = m_ProjectFile
                g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Visible = True
            Else
                If (g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Visible) Then
                    g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Visible = False
                    g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Text = ""
                    g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.ToolTipText = ""
                End If
            End If
        End Sub

        Public Sub SetProjectChanged(bChanged As Boolean)
            g_bProjectChanged = bChanged

            UpdateStatusLabel()
        End Sub

        Public Sub BeginUpdate()
            g_mUCProjectBrowser.ListView_ProjectFiles.BeginUpdate()
        End Sub

        Public Sub EndUpdate()
            g_mUCProjectBrowser.ListView_ProjectFiles.EndUpdate()
        End Sub

        Public Function GetPathsCount(Optional bSelectedOnly As Boolean = False) As Integer
            If (bSelectedOnly) Then
                Return g_mUCProjectBrowser.ListView_ProjectFiles.SelectedItems.Count
            Else
                Return g_mUCProjectBrowser.ListView_ProjectFiles.Items.Count
            End If
        End Function

        Public Function GetPathsInfo(Optional bSelectedOnly As Boolean = False) As STRUC_PROJECT_PATH_INFO()
            Dim lFileList As New List(Of STRUC_PROJECT_PATH_INFO)

            Dim mItems As IList
            If (bSelectedOnly) Then
                mItems = g_mUCProjectBrowser.ListView_ProjectFiles.SelectedItems
            Else
                mItems = g_mUCProjectBrowser.ListView_ProjectFiles.Items
            End If

            For Each mListViewItem As ListViewItem In mItems
                Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), STRUC_PROJECT_PATH_INFO)

                lFileList.Add(mInfo)
            Next

            Return lFileList.ToArray
        End Function

        Public Function PrompSaveProject(Optional bAlwaysPrompt As Boolean = False, Optional bAlwaysYes As Boolean = False) As Boolean
            If (Not bAlwaysPrompt AndAlso Not g_bProjectChanged) Then
                Return False
            End If

            Select Case (If(bAlwaysYes, DialogResult.Yes, MessageBox.Show("Do you want to save your project?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)))
                Case DialogResult.Yes
                    If (Not m_ProjectOpened) Then
                        Using i As New SaveFileDialog
                            i.Filter = String.Format("BasicPawn Project|*{0}", UCProjectBrowser.ClassProjectControl.g_sProjectExtension)

                            i.InitialDirectory = If(String.IsNullOrEmpty(m_ProjectFile), "", IO.Path.GetDirectoryName(m_ProjectFile))
                            i.FileName = IO.Path.GetFileName(m_ProjectFile)

                            If (i.ShowDialog = DialogResult.OK) Then
                                SaveProject(i.FileName)

                                g_mUCProjectBrowser.g_mFormMain.g_mUCStartPage.g_mClassRecentItems.AddRecent(m_ProjectFile)

                                Return False
                            Else
                                Return True
                            End If
                        End Using
                    Else
                        SaveProject()

                        g_mUCProjectBrowser.g_mFormMain.g_mUCStartPage.g_mClassRecentItems.AddRecent(m_ProjectFile)

                        Return False
                    End If

                Case DialogResult.No
                    Return False

                Case Else
                    Return True

            End Select
        End Function

        Public Sub SaveProject()
            SaveProject(g_sProjectFile)
        End Sub

        Public Sub SaveProject(sProjectFile As String)
            If (String.IsNullOrEmpty(sProjectFile)) Then
                Throw New ArgumentException("Project file path empty")
            End If

            g_mUCProjectBrowser.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User saved project file: " & sProjectFile, New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(sProjectFile))

            IO.File.WriteAllText(sProjectFile, "")

            Using mStream = ClassFileStreamWait.Create(sProjectFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT)

                    'TODO: Add settings

                    mIni.WriteKeyValue(lContent.ToArray)
                End Using
            End Using

            g_sProjectFile = sProjectFile
            m_ExplorerPath = ""
            RefreshExplorer()
            SetProjectChanged(False)
        End Sub

        Public Sub LoadProject(sProjectFile As String, bAppend As Boolean, bOpenProjectFiles As Boolean)
            If (String.IsNullOrEmpty(sProjectFile) OrElse Not IO.File.Exists(sProjectFile)) Then
                Throw New ArgumentException(String.Format("Project file '{0}' not found", sProjectFile))
            End If

            If (Not bAppend) Then
                If (Not CloseProject(False)) Then
                    Return
                End If
            End If

            g_mUCProjectBrowser.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User loaded project file: " & sProjectFile, New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(sProjectFile))

            Dim lProjectFiles As New List(Of String)
            Dim bDidAppend As Boolean = (GetPathsCount() > 0)

            'Read absolute paths
            Using mStream = ClassFileStreamWait.Create(sProjectFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    Try
                        BeginUpdate()

                        'TODO: Load project settings

                    Finally
                        EndUpdate()
                    End Try
                End Using
            End Using

            g_sProjectFile = sProjectFile
            m_ExplorerPath = ""
            RefreshExplorer()
            SetProjectChanged(bDidAppend)

            If (bOpenProjectFiles) Then
                Try
                    g_mUCProjectBrowser.g_mFormMain.g_ClassTabControl.BeginUpdate()

                    For Each sFile As String In lProjectFiles
                        Try
                            If (Not IO.File.Exists(sFile)) Then
                                Select Case (MessageBox.Show(String.Format("File '{0}' does not exist", sFile), "Unable to open file", MessageBoxButtons.OKCancel, MessageBoxIcon.Error))
                                    Case DialogResult.Cancel
                                        Exit For

                                    Case Else
                                        Continue For
                                End Select
                            End If

                            Dim mTab = g_mUCProjectBrowser.g_mFormMain.g_ClassTabControl.AddTab()
                            mTab.OpenFileTab(sFile)
                        Catch ex As Exception
                            ClassExceptionLog.WriteToLogMessageBox(ex)
                        End Try
                    Next
                Finally
                    g_mUCProjectBrowser.g_mFormMain.g_ClassTabControl.EndUpdate()
                End Try
            End If
        End Sub

        Public Function CloseProject(bForce As Boolean) As Boolean
            If (Not bForce AndAlso m_ProjectChanged) Then
                Select Case (MessageBox.Show("Project has unsaved changes! Do you want to continue and discard all changes?", "Unsaved Project changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    Case DialogResult.No
                        Return False
                End Select
            End If

            g_sProjectFile = ""
            g_mUCProjectBrowser.ListView_ProjectFiles.Items.Clear()

            SetProjectChanged(False)

            Return True
        End Function

        Public Function GetRecursiveFilesFromProject(iMaxDirectoryDepth As Integer) As String()
            If (Not m_ProjectOpened OrElse Not IO.File.Exists(m_ProjectFile)) Then
                Return Nothing
            End If

            Dim lList As New List(Of String)
            GetRecursiveFiles(m_ProjectDirectory, lList, iMaxDirectoryDepth)

            Return lList.ToArray
        End Function

        Private Sub GetRecursiveFiles(sDirectory As String, ByRef lList As List(Of String), iMaxDirectoryDepth As Integer)
            Dim sFiles As String() = IO.Directory.GetFiles(sDirectory)
            Dim sDirectories As String() = IO.Directory.GetDirectories(sDirectory)

            For Each i As String In sFiles
                If (Not IO.File.Exists(i)) Then
                    Continue For
                End If

                If (lList.Contains(i.ToLower)) Then
                    Continue For
                End If

                Select Case (IO.Path.GetExtension(i).ToLower)
                    Case ".sp", ".sma", ".p", ".pwn", ".inc"
                        lList.Add(i.ToLower)
                End Select
            Next

            If (iMaxDirectoryDepth < 1) Then
                Return
            End If

            For Each i As String In sDirectories
                GetRecursiveFiles(i, lList, iMaxDirectoryDepth - 1)
            Next
        End Sub
    End Class

    Private Sub ToolStripMenuItem_MenuProjectLoad_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_MenuProjectLoad.Click
        g_mFormMain.ToolStripMenuItem_FileProjectLoad.PerformClick()
    End Sub

    Private Sub ToolStripMenuItem_MenuProjectSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_MenuProjectSave.Click
        g_mFormMain.ToolStripMenuItem_FileProjectSave.PerformClick()
    End Sub

    Private Sub ToolStripMenuItem_Open_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Open.Click
        Try
            If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
                Return
            End If

            Try
                g_mFormMain.g_ClassTabControl.BeginUpdate()

                For Each mListViewItem As ListViewItem In ListView_ProjectFiles.SelectedItems
                    Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
                    If (mListViewItemData Is Nothing) Then
                        Continue For
                    End If

                    Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_PATH_INFO)
                    If (Not mInfo.m_PathType = ClassProjectControl.STRUC_PROJECT_PATH_INFO.ENUM_PATH_TYPE.FILE) Then
                        Continue For
                    End If

                    If (Not IO.File.Exists(mInfo.m_Path)) Then
                        Throw New ArgumentException(String.Format("File '{0}' does not exist", mInfo.m_Path))
                    End If

                    Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByFile(mInfo.m_Path)
                    If (mTab IsNot Nothing) Then
                        mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)
                    Else
                        mTab = g_mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(mInfo.m_Path)
                        mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)
                    End If
                Next

                g_mFormMain.g_ClassTabControl.RemoveUnsavedTabsLeft()
            Finally
                g_mFormMain.g_ClassTabControl.EndUpdate()
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ProjectLoad_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ProjectLoad.Click
        g_mFormMain.ToolStripMenuItem_FileProjectLoad.PerformClick()
    End Sub

    Private Sub ToolStripMenuItem_ProjectSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ProjectSave.Click
        g_mFormMain.ToolStripMenuItem_FileProjectSave.PerformClick()
    End Sub

    Private Sub ToolStripMenuItem_ProjectSaveAs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ProjectSaveAs.Click
        g_mFormMain.ToolStripMenuItem_FileProjectSaveAs.PerformClick()
    End Sub

    Private Sub ToolStripMenuItem_ProjectClose_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ProjectClose.Click
        g_mFormMain.ToolStripMenuItem_FileProjectClose.PerformClick()
    End Sub

    Private Sub ToolStripMenuItem_Cut_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Cut.Click
        'Try
        '    If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
        '        Return
        '    End If

        '    g_lClipboardFiles.Clear()

        '    Try
        '        g_ClassProjectControl.BeginUpdate()

        '        For i = ListView_ProjectFiles.SelectedItems.Count - 1 To 0 Step -1
        '            Dim mListViewItemData = TryCast(ListView_ProjectFiles.SelectedItems(i), ClassListViewItemData)
        '            If (mListViewItemData Is Nothing) Then
        '                Continue For
        '            End If

        '            Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)
        '            Dim iIndex As Integer = ListView_ProjectFiles.SelectedItems(i).Index

        '            mInfo.sGUID = Nothing

        '            g_lClipboardFiles.Add(mInfo)

        '            g_ClassProjectControl.RemoveFileAt(iIndex)
        '        Next
        '    Finally
        '        g_ClassProjectControl.EndUpdate()
        '    End Try

        '    g_lClipboardFiles.Reverse()
        'Catch ex As Exception
        '    ClassExceptionLog.WriteToLogMessageBox(ex)
        'End Try
    End Sub

    Private Sub ToolStripMenuItem_Copy_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Copy.Click
        'Try
        '    If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
        '        Return
        '    End If

        '    g_lClipboardFiles.Clear()

        '    For i = ListView_ProjectFiles.SelectedItems.Count - 1 To 0 Step -1
        '        Dim mListViewItemData = TryCast(ListView_ProjectFiles.SelectedItems(i), ClassListViewItemData)
        '        If (mListViewItemData Is Nothing) Then
        '            Continue For
        '        End If

        '        Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)

        '        mInfo.sGUID = Nothing

        '        g_lClipboardFiles.Add(mInfo)
        '    Next

        '    g_lClipboardFiles.Reverse()
        'Catch ex As Exception
        '    ClassExceptionLog.WriteToLogMessageBox(ex)
        'End Try
    End Sub

    Private Sub ToolStripMenuItem_Paste_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Paste.Click
        'Try
        '    If (ListView_ProjectFiles.SelectedItems.Count > 0) Then
        '        Dim iIndex As Integer = ListView_ProjectFiles.SelectedItems(0).Index

        '        Try
        '            g_ClassProjectControl.BeginUpdate()

        '            For i = g_lClipboardFiles.Count - 1 To 0 Step -1
        '                g_ClassProjectControl.InsertFile(iIndex, g_lClipboardFiles(i))
        '            Next
        '        Finally
        '            g_ClassProjectControl.EndUpdate()
        '        End Try
        '    Else
        '        Try
        '            g_ClassProjectControl.BeginUpdate()

        '            For i = 0 To g_lClipboardFiles.Count - 1
        '                g_ClassProjectControl.AddFile(g_lClipboardFiles(i))
        '            Next
        '        Finally
        '            g_ClassProjectControl.EndUpdate()
        '        End Try
        '    End If

        '    g_lClipboardFiles.Clear()
        'Catch ex As Exception
        '    ClassExceptionLog.WriteToLogMessageBox(ex)
        'End Try
    End Sub

    Private Sub ListView_ProjectFiles_DoubleClick(sender As Object, e As EventArgs) Handles ListView_ProjectFiles.DoubleClick
        Try
            Dim mFileInfos = g_ClassProjectControl.GetPathsInfo(True)
            If (mFileInfos.Length < 1) Then
                Return
            End If

            Dim mFileInfo = mFileInfos(0)

            Select Case (mFileInfo.m_PathType)
                Case ClassProjectControl.STRUC_PROJECT_PATH_INFO.ENUM_PATH_TYPE.FILE
                    If (Not IO.File.Exists(mFileInfo.m_Path)) Then
                        Throw New ArgumentException(String.Format("File '{0}' does not exist", mFileInfo.m_Path))
                    End If

                    Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByFile(mFileInfo.m_Path)
                    If (mTab IsNot Nothing) Then
                        mTab.SelectTab()
                    Else
                        mTab = g_mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(mFileInfo.m_Path)
                        mTab.SelectTab()
                    End If

                    g_mFormMain.g_ClassTabControl.RemoveUnsavedTabsLeft()

                Case ClassProjectControl.STRUC_PROJECT_PATH_INFO.ENUM_PATH_TYPE.DIRECTORY
                    g_ClassProjectControl.GoToExplorer(mFileInfo.m_Path)
            End Select

        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ContextMenuStrip_ProjectFiles_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_ProjectFiles.Opening
        ToolStripMenuItem_Open.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)

        ToolStripMenuItem_CompileAll.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
        ToolStripMenuItem_TestAll.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)

        ToolStripMenuItem_Cut.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
        ToolStripMenuItem_Copy.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
        ToolStripMenuItem_Paste.Enabled = (g_lClipboardFiles.Count > 0)
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

        'Deselect everything
        For i = 0 To ListView_ProjectFiles.Items.Count - 1
            ListView_ProjectFiles.Items(i).Selected = False
        Next

        For i = 0 To ListView_ProjectFiles.Items.Count - 1
            If (Not ListView_ProjectFiles.Items(i).Text.ToLower.Contains(sSearchText.ToLower)) Then
                Continue For
            End If

            ListView_ProjectFiles.Items(i).Selected = True
            ListView_ProjectFiles.Items(i).EnsureVisible()
        Next
    End Sub

    Private Sub ToolStripMenuItem_CompileAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CompileAll.Click
        Try
            Dim lFiles As New List(Of String)

            For Each mItem In g_ClassProjectControl.GetPathsInfo(True)
                If (Not mItem.m_PathType = ClassProjectControl.STRUC_PROJECT_PATH_INFO.ENUM_PATH_TYPE.FILE) Then
                    Continue For
                End If

                lFiles.Add(mItem.m_Path)
            Next

            Using i As New FormMultiCompiler(g_mFormMain, lFiles.ToArray, False, False)
                i.ShowDialog(g_mFormMain)
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_TestAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TestAll.Click
        Try
            Dim lFiles As New List(Of String)

            For Each mFileInfo In g_ClassProjectControl.GetPathsInfo(True)
                If (Not mFileInfo.m_PathType = ClassProjectControl.STRUC_PROJECT_PATH_INFO.ENUM_PATH_TYPE.FILE) Then
                    Continue For
                End If

                lFiles.Add(mFileInfo.m_Path)
            Next

            Using i As New FormMultiCompiler(g_mFormMain, lFiles.ToArray, True, False)
                i.ShowDialog(g_mFormMain)
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ListView_ProjectFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_ProjectFiles.SelectedIndexChanged
        UpdateListViewColors()
    End Sub

    Private Sub ListView_ProjectFiles_Invalidated(sender As Object, e As InvalidateEventArgs) Handles ListView_ProjectFiles.Invalidated
        Static bIgnoreEvent As Boolean = False
        Static mLastBackColor As Color = Color.White
        Static mLastForeColor As Color = Color.Black

        If (Not g_bLoaded OrElse bIgnoreEvent) Then
            Return
        End If

        If (ListView_ProjectFiles.BackColor <> mLastBackColor OrElse ListView_ProjectFiles.ForeColor <> mLastForeColor) Then
            mLastBackColor = ListView_ProjectFiles.BackColor
            mLastForeColor = ListView_ProjectFiles.ForeColor

            bIgnoreEvent = True
            UpdateListViewColors()
            bIgnoreEvent = False
        End If
    End Sub

    Private Sub UpdateListViewColors()
        If (g_mSelectedItemsQueue.Count < 1 AndAlso ListView_ProjectFiles.Items.Count < 1) Then
            Return
        End If

        Try
            ListView_ProjectFiles.SuspendLayout()

            While (g_mSelectedItemsQueue.Count > 0)
                Dim mItem = g_mSelectedItemsQueue.Dequeue

                'Reset to parent color
                mItem.ForeColor = Color.Empty
                mItem.BackColor = Color.Empty
            End While

            If (ListView_ProjectFiles.SelectedIndices.Count < 1) Then
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

            For Each i As Integer In ListView_ProjectFiles.SelectedIndices
                If (ListView_ProjectFiles.Items(i).ForeColor <> mForeColor OrElse
                         ListView_ProjectFiles.Items(i).BackColor <> mBackColor) Then
                    ListView_ProjectFiles.Items(i).ForeColor = mForeColor
                    ListView_ProjectFiles.Items(i).BackColor = mBackColor

                    g_mSelectedItemsQueue.Enqueue(ListView_ProjectFiles.Items(i))
                End If
            Next
        Finally
            ListView_ProjectFiles.ResumeLayout()
        End Try
    End Sub

    Private Sub ToolStripMenuItem_MenuProjectHome_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_MenuProjectHome.Click
        Try
            If (Not g_ClassProjectControl.m_ProjectOpened) Then
                Return
            End If

            g_ClassProjectControl.HomeExplorer()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_MenuProjectDirUp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_MenuProjectDirUp.Click
        Try
            If (Not g_ClassProjectControl.m_ProjectOpened) Then
                Return
            End If

            g_ClassProjectControl.GoUpExplorer()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_MenuProjectRefresh_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_MenuProjectRefresh.Click
        Try
            If (Not g_ClassProjectControl.m_ProjectOpened) Then
                Return
            End If

            g_ClassProjectControl.RefreshExplorer()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

#Region "File Drag & Drop"
    Private Sub ListView_ProjectFiles_DragEnter(sender As Object, e As DragEventArgs) Handles ListView_ProjectFiles.DragEnter
        Try
            If (Not e.Data.GetDataPresent(DataFormats.FileDrop)) Then
                Return
            End If

            e.Effect = DragDropEffects.Copy
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ListView_ProjectFiles_DragOver(sender As Object, e As DragEventArgs) Handles ListView_ProjectFiles.DragOver
        Try
            If (Not e.Data.GetDataPresent(DataFormats.FileDrop)) Then
                Return
            End If

            e.Effect = DragDropEffects.Copy
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ListView_ProjectFiles_DragDrop(sender As Object, e As DragEventArgs) Handles ListView_ProjectFiles.DragDrop
        'Try
        '    If (Not e.Data.GetDataPresent(DataFormats.FileDrop)) Then
        '        Return
        '    End If

        '    Dim sFiles As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
        '    If (sFiles Is Nothing) Then
        '        Return
        '    End If

        '    Try
        '        g_ClassProjectControl.BeginUpdate()

        '        For Each sFile As String In sFiles
        '            If (Not IO.File.Exists(sFile)) Then
        '                Continue For
        '            End If

        '            If (g_ClassProjectControl.IsFileInProject(sFile, False)) Then
        '                Continue For
        '            End If

        '            g_ClassProjectControl.AddFile(New ClassProjectControl.STRUC_PROJECT_FILE_INFO With {
        '                .sGUID = Nothing,
        '                .sFile = sFile,
        '                .sPackedData = Nothing
        '            })
        '        Next
        '    Finally
        '        g_ClassProjectControl.EndUpdate()
        '    End Try
        'Catch ex As Exception
        '    ClassExceptionLog.WriteToLogMessageBox(ex)
        'End Try
    End Sub
#End Region
End Class
