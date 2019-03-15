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


Public Class UCProjectBrowser
    Public g_mFormMain As FormMain
    Public g_ClassProjectControl As ClassProjectControl

    Private g_lClipboardFiles As New List(Of ClassProjectControl.STRUC_PROJECT_FILE_INFO)

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_ClassProjectControl = New ClassProjectControl(Me)
    End Sub

    Class ClassProjectControl
        Private g_mUCProjectBrowser As UCProjectBrowser

        Private g_sProjectFile As String = ""
        Private g_bProjectChanged As Boolean = False

        Public Shared g_sProjectExtension As String = ".bpproj"

        Structure STRUC_PROJECT_FILE_INFO
            Dim sGUID As String
            Dim sFile As String
            Dim sPackedData As String
        End Structure

        Public Sub New(f As UCProjectBrowser)
            g_mUCProjectBrowser = f
        End Sub

        Property m_ProjectFile As String
            Get
                Return g_sProjectFile
            End Get
            Set(value As String)
                If (g_sProjectFile.ToLower <> value.ToLower) Then
                    m_ProjectChanged = False
                End If

                g_sProjectFile = value

                If (Not m_ProjectOpened) Then
                    m_ProjectChanged = False
                End If

                UpdateStatusLabel()
            End Set
        End Property

        ReadOnly Property m_ProjectOpened As Boolean
            Get
                Return (Not String.IsNullOrEmpty(g_sProjectFile))
            End Get
        End Property

        Property m_ProjectChanged As Boolean
            Get
                Return g_bProjectChanged
            End Get
            Set(value As Boolean)
                g_bProjectChanged = value

                UpdateStatusLabel()
            End Set
        End Property

        Public Sub UpdateStatusLabel()
            g_mUCProjectBrowser.g_mFormMain.TabPage_ProjectBrowser.Text = g_mUCProjectBrowser.g_mFormMain.TabPage_ProjectBrowser.Text.TrimEnd("*"c) & If(m_ProjectChanged, "*", "")

            If (Not String.IsNullOrEmpty(g_sProjectFile)) Then
                g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Text = String.Format("Project: {0}{1}", IO.Path.GetFileNameWithoutExtension(g_sProjectFile), If(m_ProjectChanged, "*", ""))
                g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.ToolTipText = g_sProjectFile
                g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Visible = True
            Else
                g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Visible = False
                g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Text = ""
                g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.ToolTipText = ""
            End If
        End Sub

        Public Sub UpdateListViewInfo()
            For Each mListViewItem As ListViewItem In g_mUCProjectBrowser.ListView_ProjectFiles.Items
                Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), STRUC_PROJECT_FILE_INFO)

                mListViewItem.SubItems(0).Text = IO.Path.GetFileName(mInfo.sFile)
                mListViewItem.SubItems(1).Text = mInfo.sFile
                mListViewItem.SubItems(2).Text = If(String.IsNullOrEmpty(mInfo.sPackedData), "-", "Yes")
                mListViewItem.ToolTipText = mInfo.sFile & If(String.IsNullOrEmpty(mInfo.sPackedData), "", Environment.NewLine & "(Packed)")
            Next
        End Sub

        Public Sub AddFile(mInfo As STRUC_PROJECT_FILE_INFO)
            Dim mListViewItemData As New ClassListViewItemData(New String() {IO.Path.GetFileName(mInfo.sFile), mInfo.sFile, If(String.IsNullOrEmpty(mInfo.sPackedData), "-", "Yes")}) With {
                .ToolTipText = mInfo.sFile & If(String.IsNullOrEmpty(mInfo.sPackedData), "", Environment.NewLine & "(Packed)")
            }

            If (String.IsNullOrEmpty(mInfo.sGUID)) Then
                mInfo.sGUID = Guid.NewGuid.ToString
            End If

            mListViewItemData.g_mData("Info") = mInfo

            g_mUCProjectBrowser.ListView_ProjectFiles.Items.Add(mListViewItemData)

            m_ProjectChanged = True
        End Sub

        Public Sub InsertFile(iIndex As Integer, mInfo As STRUC_PROJECT_FILE_INFO)
            Dim mListViewItemData As New ClassListViewItemData(New String() {IO.Path.GetFileName(mInfo.sFile), mInfo.sFile, If(String.IsNullOrEmpty(mInfo.sPackedData), "-", "Yes")}) With {
                .ToolTipText = mInfo.sFile & If(String.IsNullOrEmpty(mInfo.sPackedData), "", Environment.NewLine & "(Packed)")
            }

            If (String.IsNullOrEmpty(mInfo.sGUID)) Then
                mInfo.sGUID = Guid.NewGuid.ToString
            End If

            mListViewItemData.g_mData("Info") = mInfo

            g_mUCProjectBrowser.ListView_ProjectFiles.Items.Insert(iIndex, mListViewItemData)

            m_ProjectChanged = True
        End Sub

        Public Sub PackFileData(ByRef mInfo As STRUC_PROJECT_FILE_INFO)
            Dim sText As String = IO.File.ReadAllText(mInfo.sFile)

            mInfo.sPackedData = sText

            m_ProjectChanged = True
        End Sub

        Public Function ExtractFileDataDialog(mInfo As STRUC_PROJECT_FILE_INFO, ByRef r_sNewPath As String) As Boolean
            Using i As New SaveFileDialog
                i.FileName = mInfo.sFile

                If (i.ShowDialog() = DialogResult.OK) Then
                    r_sNewPath = i.FileName

                    IO.File.WriteAllText(r_sNewPath, mInfo.sPackedData)
                    Return True
                End If
            End Using

            Return False
        End Function

        Public Sub ExtractFileData(mInfo As STRUC_PROJECT_FILE_INFO, sFile As String)
            IO.File.WriteAllText(sFile, mInfo.sPackedData)
        End Sub

        Public Sub RemoveFileAll(sFile As String)
            For i = g_mUCProjectBrowser.ListView_ProjectFiles.Items.Count - 1 To 0 Step -1
                Dim mListViewItemData = TryCast(g_mUCProjectBrowser.ListView_ProjectFiles.Items(i), ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), STRUC_PROJECT_FILE_INFO)

                If (mInfo.sFile.ToLower = sFile.ToLower) Then
                    g_mUCProjectBrowser.ListView_ProjectFiles.Items.RemoveAt(i)
                End If
            Next

            m_ProjectChanged = True
        End Sub

        Public Sub RemoveFileAt(iIndex As Integer)
            g_mUCProjectBrowser.ListView_ProjectFiles.Items.RemoveAt(iIndex)

            m_ProjectChanged = True
        End Sub

        Public Function GetFilesCount() As Integer
            Return g_mUCProjectBrowser.ListView_ProjectFiles.Items.Count
        End Function

        Public Function GetFiles() As STRUC_PROJECT_FILE_INFO()
            Dim lFileList As New List(Of STRUC_PROJECT_FILE_INFO)

            For Each mListViewItem As ListViewItem In g_mUCProjectBrowser.ListView_ProjectFiles.Items
                Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), STRUC_PROJECT_FILE_INFO)

                lFileList.Add(mInfo)
            Next

            Return lFileList.ToArray
        End Function

        Public Function IsFileInProject(sFile As String) As Boolean
            For Each mInfo As STRUC_PROJECT_FILE_INFO In GetFiles()
                If (mInfo.sFile.ToLower = sFile.ToLower) Then
                    Return True
                End If
            Next

            Return False
        End Function

        Public Sub ClearFiles()
            g_mUCProjectBrowser.ListView_ProjectFiles.Items.Clear()

            m_ProjectChanged = True
        End Sub

        Public Function PrompSaveProject(Optional bAlwaysPrompt As Boolean = False, Optional bAlwaysYes As Boolean = False) As Boolean
            If (Not bAlwaysPrompt AndAlso Not g_bProjectChanged) Then
                Return False
            End If

            Select Case (If(bAlwaysYes, DialogResult.Yes, MessageBox.Show("Do you want to save your project?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)))
                Case DialogResult.Yes
                    If (Not m_ProjectOpened) Then
                        Using i As New SaveFileDialog
                            i.Filter = String.Format("BasicPawn Project|*{0}", UCProjectBrowser.ClassProjectControl.g_sProjectExtension)
                            i.FileName = m_ProjectFile

                            If (i.ShowDialog = DialogResult.OK) Then
                                m_ProjectFile = i.FileName
                                SaveProject()

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
            If (String.IsNullOrEmpty(g_sProjectFile)) Then
                Throw New ArgumentException("Project file path empty")
            End If

            IO.File.WriteAllText(g_sProjectFile, "")

            Using mStream = ClassFileStreamWait.Create(g_sProjectFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT)

                    For Each mInfo As STRUC_PROJECT_FILE_INFO In GetFiles()
                        lContent.Add(New ClassIni.STRUC_INI_CONTENT("Project", mInfo.sGUID, mInfo.sFile))

                        If (Not String.IsNullOrEmpty(mInfo.sPackedData)) Then
                            lContent.Add(New ClassIni.STRUC_INI_CONTENT("PackedData", mInfo.sGUID, ClassTools.ClassCrypto.ClassBase.ToBase64(mInfo.sPackedData, System.Text.Encoding.UTF8)))
                        End If
                    Next

                    mIni.WriteKeyValue(lContent.ToArray)
                End Using
            End Using

            m_ProjectChanged = False
            UpdateListViewInfo()

            g_mUCProjectBrowser.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User saved project file: " & g_sProjectFile, New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(g_sProjectFile))
        End Sub

        Public Sub LoadProject(bAppend As Boolean, bOpenProjectFiles As Boolean)
            If (String.IsNullOrEmpty(g_sProjectFile) OrElse Not IO.File.Exists(g_sProjectFile)) Then
                Throw New ArgumentException("Project file not found")
            End If

            g_mUCProjectBrowser.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User loaded project file: " & g_sProjectFile, New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(g_sProjectFile))

            If (Not bAppend) Then
                ClearFiles()
            End If

            Dim lProjectFiles As New List(Of String)
            Dim bDidAppend As Boolean = GetFilesCount() > 0

            Using mStream = ClassFileStreamWait.Create(g_sProjectFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    For Each mItem In mIni.ReadEverything
                        If (mItem.sSection <> "Project") Then
                            Continue For
                        End If

                        Dim sPackedData As String = mIni.ReadKeyValue("PackedData", mItem.sKey, Nothing)
                        If (Not String.IsNullOrEmpty(sPackedData)) Then
                            sPackedData = ClassTools.ClassCrypto.ClassBase.FromBase64(sPackedData, System.Text.Encoding.UTF8)
                        End If

                        AddFile(New STRUC_PROJECT_FILE_INFO With {
                            .sGUID = mItem.sKey,
                            .sFile = mItem.sValue,
                            .sPackedData = sPackedData
                        })

                        lProjectFiles.Add(mItem.sValue)

                        g_mUCProjectBrowser.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Loaded project file: " & mItem.sValue, New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(mItem.sValue))
                    Next
                End Using
            End Using

            m_ProjectChanged = bDidAppend
            UpdateListViewInfo()

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

        Public Sub CloseProject()
            ClearFiles()
            m_ProjectFile = ""

            m_ProjectChanged = False
        End Sub
    End Class

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

                    Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)
                    If (Not IO.File.Exists(mInfo.sFile)) Then
                        Throw New ArgumentException(String.Format("File '{0}' does not exist", mInfo.sFile))
                    End If

                    Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByFile(mInfo.sFile)
                    If (mTab IsNot Nothing) Then
                        mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)
                    Else
                        mTab = g_mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(mInfo.sFile)
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

    Private Sub ToolStripMenuItem_ProjectSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ProjectSave.Click
        Try
            If (String.IsNullOrEmpty(g_ClassProjectControl.m_ProjectFile)) Then
                Using i As New SaveFileDialog
                    i.Filter = String.Format("BasicPawn Project|*{0}", ClassProjectControl.g_sProjectExtension)
                    i.FileName = g_ClassProjectControl.m_ProjectFile

                    If (i.ShowDialog = DialogResult.OK) Then
                        g_ClassProjectControl.m_ProjectFile = i.FileName
                    Else
                        Return
                    End If
                End Using
            End If

            g_ClassProjectControl.SaveProject()

            g_mFormMain.g_mUCStartPage.g_mClassRecentItems.AddRecent(g_ClassProjectControl.m_ProjectFile)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Cut_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Cut.Click
        Try
            If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
                Return
            End If

            g_lClipboardFiles.Clear()

            For i = ListView_ProjectFiles.SelectedItems.Count - 1 To 0 Step -1
                Dim mListViewItemData = TryCast(ListView_ProjectFiles.SelectedItems(i), ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)
                Dim iIndex As Integer = ListView_ProjectFiles.SelectedItems(i).Index

                mInfo.sGUID = Nothing

                g_lClipboardFiles.Add(mInfo)

                g_ClassProjectControl.RemoveFileAt(iIndex)
            Next

            g_lClipboardFiles.Reverse()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Copy_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Copy.Click
        Try
            If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
                Return
            End If

            g_lClipboardFiles.Clear()

            For i = ListView_ProjectFiles.SelectedItems.Count - 1 To 0 Step -1
                Dim mListViewItemData = TryCast(ListView_ProjectFiles.SelectedItems(i), ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)

                mInfo.sGUID = Nothing

                g_lClipboardFiles.Add(mInfo)
            Next

            g_lClipboardFiles.Reverse()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Paste_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Paste.Click
        Try
            If (ListView_ProjectFiles.SelectedItems.Count > 0) Then
                Dim iIndex As Integer = ListView_ProjectFiles.SelectedItems(0).Index

                For i = g_lClipboardFiles.Count - 1 To 0 Step -1
                    g_ClassProjectControl.InsertFile(iIndex, g_lClipboardFiles(i))
                Next
            Else
                For i = 0 To g_lClipboardFiles.Count - 1
                    g_ClassProjectControl.AddFile(g_lClipboardFiles(i))
                Next
            End If

            g_lClipboardFiles.Clear()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Exlcude_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Exlcude.Click
        Try
            If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
                Return
            End If

            For i = ListView_ProjectFiles.SelectedItems.Count - 1 To 0 Step -1
                Dim iIndex As Integer = ListView_ProjectFiles.SelectedItems(i).Index

                g_ClassProjectControl.RemoveFileAt(iIndex)
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ListView_ProjectFiles_DoubleClick(sender As Object, e As EventArgs) Handles ListView_ProjectFiles.DoubleClick
        Try
            If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
                Return
            End If

            Dim mListViewItemData = TryCast(ListView_ProjectFiles.SelectedItems(0), ClassListViewItemData)
            If (mListViewItemData Is Nothing) Then
                Return
            End If

            Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)
            If (Not IO.File.Exists(mInfo.sFile)) Then
                Throw New ArgumentException(String.Format("File '{0}' does not exist", mInfo.sFile))
            End If

            Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByFile(mInfo.sFile)
            If (mTab IsNot Nothing) Then
                mTab.SelectTab()
            Else
                mTab = g_mFormMain.g_ClassTabControl.AddTab()
                mTab.OpenFileTab(mInfo.sFile)
                mTab.SelectTab()
            End If

            g_mFormMain.g_ClassTabControl.RemoveUnsavedTabsLeft()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_AddTab_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_AddTab.Click
        Try
            If (g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IsUnsaved) Then
                Return
            End If

            g_ClassProjectControl.AddFile(New ClassProjectControl.STRUC_PROJECT_FILE_INFO With {
                    .sGUID = Nothing,
                    .sFile = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File,
                    .sPackedData = Nothing
                })
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_AddNewTabs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_AddNewTabs.Click
        Try
            For i = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                If (g_mFormMain.g_ClassTabControl.m_Tab(i).m_IsUnsaved) Then
                    Continue For
                End If

                If (g_ClassProjectControl.IsFileInProject(g_mFormMain.g_ClassTabControl.m_Tab(i).m_File)) Then
                    Continue For
                End If

                g_ClassProjectControl.AddFile(New ClassProjectControl.STRUC_PROJECT_FILE_INFO With {
                    .sGUID = Nothing,
                    .sFile = g_mFormMain.g_ClassTabControl.m_Tab(i).m_File,
                    .sPackedData = Nothing
                })
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_AddAllTabs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_AddAllTabs.Click
        Try
            For i = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                If (g_mFormMain.g_ClassTabControl.m_Tab(i).m_IsUnsaved) Then
                    Continue For
                End If

                g_ClassProjectControl.AddFile(New ClassProjectControl.STRUC_PROJECT_FILE_INFO With {
                    .sGUID = Nothing,
                    .sFile = g_mFormMain.g_ClassTabControl.m_Tab(i).m_File,
                    .sPackedData = Nothing
                })
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_AddFiles_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_AddFiles.Click
        Try
            Using i As New OpenFileDialog
                i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|AMX Mod X|*.sma|Pawn (Not fully supported)|*.pwn;*.p|All files|*.*"
                i.Multiselect = True

                If (i.ShowDialog = DialogResult.OK) Then
                    For Each sFile As String In i.FileNames
                        g_ClassProjectControl.AddFile(New ClassProjectControl.STRUC_PROJECT_FILE_INFO With {
                            .sGUID = Nothing,
                            .sFile = sFile,
                            .sPackedData = Nothing
                        })
                    Next
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ContextMenuStrip_ProjectFiles_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_ProjectFiles.Opening
        Dim bPackedSelected As Boolean = False

        For Each mListViewItem As ListViewItem In ListView_ProjectFiles.SelectedItems
            Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
            If (mListViewItemData Is Nothing) Then
                Continue For
            End If

            Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)

            If (Not String.IsNullOrEmpty(mInfo.sPackedData)) Then
                bPackedSelected = True
                Exit For
            End If
        Next

        ToolStripMenuItem_Open.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)

        ToolStripMenuItem_CompileAll.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
        ToolStripMenuItem_TestAll.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
        ToolStripMenuItem_ShellAll.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)

        ToolStripMenuItem_PackFile.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
        ToolStripMenuItem_ExtractFile.Enabled = bPackedSelected
        ToolStripMenuItem_DeletePack.Enabled = bPackedSelected

        ToolStripMenuItem_Cut.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
        ToolStripMenuItem_Copy.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
        ToolStripMenuItem_Paste.Enabled = (g_lClipboardFiles.Count > 0)

        ToolStripMenuItem_AddTab.Enabled = (Not g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IsUnsaved)

        ToolStripMenuItem_Exlcude.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
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
            Dim mListViewItemData = TryCast(ListView_ProjectFiles.Items(i), ClassListViewItemData)
            If (mListViewItemData Is Nothing) Then
                Return
            End If

            Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)

            If (IO.Path.GetFileName(mInfo.sFile).ToLower.Contains(sSearchText.ToLower)) Then
                ListView_ProjectFiles.Items(i).Selected = True
                ListView_ProjectFiles.Items(i).EnsureVisible()
            End If
        Next
    End Sub

    Private Sub ToolStripMenuItem_CompileAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CompileAll.Click
        Try
            If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
                Return
            End If

            Dim lFiles As New List(Of String)

            For Each mListViewItem As ListViewItem In ListView_ProjectFiles.SelectedItems
                Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)

                lFiles.Add(mInfo.sFile)
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
            If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
                Return
            End If

            Dim lFiles As New List(Of String)

            For Each mListViewItem As ListViewItem In ListView_ProjectFiles.SelectedItems
                Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)

                lFiles.Add(mInfo.sFile)
            Next

            Using i As New FormMultiCompiler(g_mFormMain, lFiles.ToArray, True, False)
                i.ShowDialog(g_mFormMain)
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ShellAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ShellAll.Click
        Try
            If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
                Return
            End If

            Dim lFiles As New List(Of String)

            For Each mListViewItem As ListViewItem In ListView_ProjectFiles.SelectedItems
                Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)

                lFiles.Add(mInfo.sFile)
            Next

            Using mFormProgress As New FormProgress
                mFormProgress.Text = "Executing shell..."
                mFormProgress.ProgressBar_Progress.Maximum = lFiles.Count
                mFormProgress.Show(Me)
                mFormProgress.m_Progress = 0

                For Each sFile In lFiles
                    Dim mConfig = ClassConfigs.FindOptimalConfigForFile(sFile, False, Nothing)
                    If (mConfig Is Nothing) Then
                        Throw New ArgumentException(String.Format("Could not find config for file '{0}'.{1}Make sure BasicPawn can find a config for this file using 'Known files' or 'Default config paths' in configs.", sFile, Environment.NewLine))
                    End If

                    Dim sShell As String = mConfig.g_sExecuteShell

                    For Each mArg In ClassSettings.GetShellArguments(g_mFormMain, sFile)
                        sShell = sShell.Replace(mArg.g_sMarker, mArg.g_sArgument)
                    Next

                    Try
                        If (String.IsNullOrEmpty(sShell)) Then
                            Throw New ArgumentException("Shell is empty")
                        End If

                        Shell(sShell, AppWinStyle.NormalFocus)
                    Catch ex As Exception
                        MessageBox.Show(ex.Message & Environment.NewLine & Environment.NewLine & sShell, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try

                    mFormProgress.m_Progress += 1
                Next
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_PackFile_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_PackFile.Click
        Try
            If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
                Return
            End If

            For Each mListViewItem As ListViewItem In ListView_ProjectFiles.SelectedItems
                Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)
                If (Not IO.File.Exists(mInfo.sFile)) Then
                    Throw New ArgumentException("File does not exist")
                End If

                g_ClassProjectControl.PackFileData(mInfo)

                mListViewItemData.g_mData("Info") = mInfo

                g_ClassProjectControl.m_ProjectChanged = True
            Next

            g_ClassProjectControl.UpdateListViewInfo()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ExtractFile_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ExtractFile.Click
        Try
            If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
                Return
            End If

            For Each mListViewItem As ListViewItem In ListView_ProjectFiles.SelectedItems
                Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)

                If (String.IsNullOrEmpty(mInfo.sPackedData)) Then
                    MessageBox.Show("This file is not packed!", "Unable to extract", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Continue For
                End If

                Dim sNewPath As String = Nothing
                If (Not g_ClassProjectControl.ExtractFileDataDialog(mInfo, sNewPath)) Then
                    Return
                End If

                If (mInfo.sFile.ToLower <> sNewPath.ToLower) Then
                    With New Text.StringBuilder
                        .AppendLine("Do you want to replace the path in the project file to the extracted path?")
                        .AppendLine()
                        .AppendFormat("'{0}'", mInfo.sFile).AppendLine()
                        .AppendLine("to")
                        .AppendFormat("'{0}'", sNewPath).AppendLine()

                        Select Case (MessageBox.Show(.ToString, "Replace path", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                            Case DialogResult.Yes
                                mInfo.sFile = sNewPath

                                mListViewItemData.g_mData("Info") = mInfo

                                g_ClassProjectControl.m_ProjectChanged = True
                        End Select
                    End With
                End If
            Next

            g_ClassProjectControl.UpdateListViewInfo()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_DeletePack_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DeletePack.Click
        Try
            If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
                Return
            End If

            For Each mListViewItem As ListViewItem In ListView_ProjectFiles.SelectedItems
                Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), ClassProjectControl.STRUC_PROJECT_FILE_INFO)

                If (String.IsNullOrEmpty(mInfo.sPackedData)) Then
                    MessageBox.Show("This file is not packed!", "Unable to remove", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Continue For
                End If

                mInfo.sPackedData = Nothing

                mListViewItemData.g_mData("Info") = mInfo

                g_ClassProjectControl.m_ProjectChanged = True
            Next

            g_ClassProjectControl.UpdateListViewInfo()
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
        Try
            If (Not e.Data.GetDataPresent(DataFormats.FileDrop)) Then
                Return
            End If

            Dim sFiles As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            If (sFiles Is Nothing) Then
                Return
            End If

            For Each sFile As String In sFiles
                If (Not IO.File.Exists(sFile)) Then
                    Continue For
                End If

                g_ClassProjectControl.AddFile(New ClassProjectControl.STRUC_PROJECT_FILE_INFO With {
                    .sGUID = Nothing,
                    .sFile = sFile,
                    .sPackedData = Nothing
                })
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
#End Region
End Class
