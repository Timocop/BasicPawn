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

    Private g_lClipboardFiles As New List(Of ClassProjectControl.STRUC_PROJECT_FILE_INFO)
    Private g_mSelectedItemsQueue As New Queue(Of ListViewItem)
    Private g_bLoaded As Boolean = False

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_ClassProjectControl = New ClassProjectControl(Me)

        ClassTools.ClassForms.SetDoubleBuffering(ListView_ProjectFiles, True)
    End Sub

    Private Sub UCProjectBrowser_Load(sender As Object, e As EventArgs) Handles Me.Load
        g_bLoaded = True
    End Sub

    Class ClassProjectControl
        Private g_mUCProjectBrowser As UCProjectBrowser

        Private g_sProjectFile As String = ""
        Private g_bProjectChanged As Boolean = False
        Private g_bProjectRelativeEnabled As Boolean = True
        Private g_ProjectRelativeNoInclude As Boolean = True

        Public Shared ReadOnly g_sProjectExtension As String = ".bpproj"

        Structure STRUC_PROJECT_FILE_INFO
            Dim sGUID As String
            Dim sFile As String
            Dim sPackedData As String
            Dim bIsRelative As Boolean
        End Structure

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

        ReadOnly Property m_ProjectChanged As Boolean
            Get
                Return g_bProjectChanged
            End Get
        End Property

        Public Property m_ProjectRelativeEnabled As Boolean
            Get
                Return g_bProjectRelativeEnabled
            End Get
            Set(value As Boolean)
                If (g_bProjectRelativeEnabled = value) Then
                    Return
                End If

                g_bProjectRelativeEnabled = value
                SetProjectChanged(True)
            End Set
        End Property

        Public Property m_ProjectRelativeNoInclude As Boolean
            Get
                Return g_ProjectRelativeNoInclude
            End Get
            Set(value As Boolean)
                If (g_ProjectRelativeNoInclude = value) Then
                    Return
                End If

                g_ProjectRelativeNoInclude = value
                SetProjectChanged(True)
            End Set
        End Property

        Public Sub UpdateStatusLabel()
            Dim sNewTabPageText As String = g_mUCProjectBrowser.g_mFormMain.TabPage_ProjectBrowser.Text.TrimEnd("*"c) & If(m_ProjectChanged, "*", "")
            If (g_mUCProjectBrowser.g_mFormMain.TabPage_ProjectBrowser.Text <> sNewTabPageText) Then
                g_mUCProjectBrowser.g_mFormMain.TabPage_ProjectBrowser.Text = sNewTabPageText
            End If

            If (m_ProjectOpened) Then
                g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Text = String.Format("Project: {0}{1}", IO.Path.GetFileNameWithoutExtension(m_ProjectFile), If(m_ProjectChanged, "*", ""))
                g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.ToolTipText = m_ProjectFile
                g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Visible = True

                g_mUCProjectBrowser.ToolStripTextBox_MenuProjectPath.Text = m_ProjectFile
            Else
                If (g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Visible) Then
                    g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Visible = False
                    g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.Text = ""
                    g_mUCProjectBrowser.g_mFormMain.ToolStripStatusLabel_Project.ToolTipText = ""
                End If

                g_mUCProjectBrowser.ToolStripTextBox_MenuProjectPath.Text = ""
            End If
        End Sub

        Public Sub UpdateListViewInfo()
            For Each mListViewItem As ListViewItem In g_mUCProjectBrowser.ListView_ProjectFiles.Items
                Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), STRUC_PROJECT_FILE_INFO)

                mListViewItem.SubItems(0).Text = CreateTreeFilename(mInfo.sFile, mInfo.bIsRelative)
                mListViewItem.SubItems(1).Text = mInfo.sFile
                mListViewItem.SubItems(2).Text = If(String.IsNullOrEmpty(mInfo.sPackedData), "-", "Yes")
                mListViewItem.SubItems(3).Text = If(mInfo.bIsRelative, "Yes", "-")
                mListViewItem.ToolTipText = mInfo.sFile & If(String.IsNullOrEmpty(mInfo.sPackedData), "", Environment.NewLine & "(Is packed)") & If(mInfo.bIsRelative, Environment.NewLine & "(Is relative path)", "")
            Next
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

        Public Sub AddFile(mInfo As STRUC_PROJECT_FILE_INFO)
            Dim mListViewItemData As New ClassListViewItemData(New String() {
                                                               CreateTreeFilename(mInfo.sFile, mInfo.bIsRelative),
                                                               mInfo.sFile,
                                                               If(String.IsNullOrEmpty(mInfo.sPackedData), "-", "Yes"),
                                                               If(mInfo.bIsRelative, "Yes", "-")}) With {
                .ToolTipText = mInfo.sFile & If(String.IsNullOrEmpty(mInfo.sPackedData), "", Environment.NewLine & "(Is packed)") & If(mInfo.bIsRelative, Environment.NewLine & "(Is relative path)", "")
            }

            If (String.IsNullOrEmpty(mInfo.sGUID)) Then
                mInfo.sGUID = Guid.NewGuid.ToString
            End If

            mListViewItemData.g_mData("Info") = mInfo

            g_mUCProjectBrowser.ListView_ProjectFiles.Items.Add(mListViewItemData)

            If (Not mInfo.bIsRelative) Then
                SetProjectChanged(True)
            End If
        End Sub

        Public Sub InsertFile(iIndex As Integer, mInfo As STRUC_PROJECT_FILE_INFO)
            Dim mListViewItemData As New ClassListViewItemData(New String() {
                                                               CreateTreeFilename(mInfo.sFile, mInfo.bIsRelative),
                                                               mInfo.sFile,
                                                               If(String.IsNullOrEmpty(mInfo.sPackedData), "-", "Yes"),
                                                               If(mInfo.bIsRelative, "Yes", "-")}) With {
                .ToolTipText = mInfo.sFile & If(String.IsNullOrEmpty(mInfo.sPackedData), "", Environment.NewLine & "(Is packed)") & If(mInfo.bIsRelative, Environment.NewLine & "(Is relative path)", "")
            }

            If (String.IsNullOrEmpty(mInfo.sGUID)) Then
                mInfo.sGUID = Guid.NewGuid.ToString
            End If

            mListViewItemData.g_mData("Info") = mInfo

            g_mUCProjectBrowser.ListView_ProjectFiles.Items.Insert(iIndex, mListViewItemData)

            If (Not mInfo.bIsRelative) Then
                SetProjectChanged(True)
            End If
        End Sub

        Public Sub PackFileData(ByRef mInfo As STRUC_PROJECT_FILE_INFO)
            If (mInfo.bIsRelative) Then
                Return
            End If

            Dim sText As String = IO.File.ReadAllText(mInfo.sFile)

            mInfo.sPackedData = sText

            SetProjectChanged(True)
        End Sub

        Public Function ExtractFileDataDialog(mInfo As STRUC_PROJECT_FILE_INFO, ByRef r_sNewPath As String) As Boolean
            Using i As New SaveFileDialog
                i.InitialDirectory = If(String.IsNullOrEmpty(mInfo.sFile), "", IO.Path.GetDirectoryName(mInfo.sFile))
                i.FileName = IO.Path.GetFileName(mInfo.sFile)

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

        Public Sub RemoveFileAll(sFile As String, bRelativeOnly As Boolean)
            Try
                BeginUpdate()

                For i = g_mUCProjectBrowser.ListView_ProjectFiles.Items.Count - 1 To 0 Step -1
                    Dim mListViewItemData = TryCast(g_mUCProjectBrowser.ListView_ProjectFiles.Items(i), ClassListViewItemData)
                    If (mListViewItemData Is Nothing) Then
                        Continue For
                    End If

                    Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), STRUC_PROJECT_FILE_INFO)

                    If (bRelativeOnly) Then
                        If (Not mInfo.bIsRelative) Then
                            Continue For
                        End If
                    End If

                    If (mInfo.sFile.ToLower = sFile.ToLower) Then
                        g_mUCProjectBrowser.ListView_ProjectFiles.Items.RemoveAt(i)

                        If (Not mInfo.bIsRelative) Then
                            SetProjectChanged(True)
                        End If
                    End If
                Next
            Finally
                EndUpdate()
            End Try
        End Sub

        Public Sub RemoveFileAt(iIndex As Integer)
            g_mUCProjectBrowser.ListView_ProjectFiles.Items.RemoveAt(iIndex)

            SetProjectChanged(True)
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

        Public Function IsFileInProject(sFile As String, bRelativePathOnly As Boolean) As Boolean
            For Each mInfo As STRUC_PROJECT_FILE_INFO In GetFiles()
                If (bRelativePathOnly) Then
                    If (Not mInfo.bIsRelative) Then
                        Continue For
                    End If
                End If

                If (mInfo.sFile.ToLower = sFile.ToLower) Then
                    Return True
                End If
            Next

            Return False
        End Function

        Public Sub ClearFiles()
            g_mUCProjectBrowser.ListView_ProjectFiles.Items.Clear()

            SetProjectChanged(True)
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

                    lContent.Add(New ClassIni.STRUC_INI_CONTENT("ProjectOptions", "RelativeProject", If(m_ProjectRelativeEnabled, "1", "0")))
                    lContent.Add(New ClassIni.STRUC_INI_CONTENT("ProjectOptions", "RelativeNoIncludes", If(m_ProjectRelativeNoInclude, "1", "0")))

                    For Each mInfo As STRUC_PROJECT_FILE_INFO In GetFiles()
                        If (mInfo.bIsRelative) Then
                            Continue For
                        End If

                        lContent.Add(New ClassIni.STRUC_INI_CONTENT("Project", mInfo.sGUID, mInfo.sFile))

                        If (Not String.IsNullOrEmpty(mInfo.sPackedData)) Then
                            lContent.Add(New ClassIni.STRUC_INI_CONTENT("PackedData", mInfo.sGUID, ClassTools.ClassCrypto.ClassBase.ToBase64(mInfo.sPackedData, System.Text.Encoding.UTF8)))
                        End If
                    Next

                    mIni.WriteKeyValue(lContent.ToArray)
                End Using
            End Using

            g_sProjectFile = sProjectFile
            RefreshRelativeFiles()
            UpdateListViewInfo()
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
            Dim bDidAppend As Boolean = (GetFilesCount() > 0)

            'Read absolute paths
            Using mStream = ClassFileStreamWait.Create(sProjectFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    Try
                        BeginUpdate()

                        m_ProjectRelativeEnabled = If(mIni.ReadKeyValue("ProjectOptions", "RelativeProject", "1") <> "0", True, False)
                        m_ProjectRelativeNoInclude = If(mIni.ReadKeyValue("ProjectOptions", "RelativeNoIncludes", "1") <> "0", True, False)

                        For Each mItem In mIni.ReadEverything
                            Select Case (mItem.sSection)
                                Case "Project"
                                    Dim sPackedData As String = mIni.ReadKeyValue("PackedData", mItem.sKey, Nothing)
                                    If (Not String.IsNullOrEmpty(sPackedData)) Then
                                        sPackedData = ClassTools.ClassCrypto.ClassBase.FromBase64(sPackedData, System.Text.Encoding.UTF8)
                                    End If

                                    AddFile(New STRUC_PROJECT_FILE_INFO With {
                                        .sGUID = mItem.sKey,
                                        .sFile = mItem.sValue,
                                        .sPackedData = sPackedData,
                                        .bIsRelative = False
                                    })

                                    lProjectFiles.Add(mItem.sValue)

                                    g_mUCProjectBrowser.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Loaded project file: " & mItem.sValue, New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(mItem.sValue))
                            End Select
                        Next
                    Finally
                        EndUpdate()
                    End Try
                End Using
            End Using

            g_sProjectFile = sProjectFile
            RefreshRelativeFiles()
            UpdateListViewInfo()
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

        Private Function CreateTreeFilename(sFile As String, bIsRelative As Boolean) As String
            If (Not bIsRelative) Then
                Return IO.Path.GetFileName(sFile)
            End If

            Dim sProjectDirectory As String = IO.Path.GetDirectoryName(m_ProjectFile).TrimEnd("\"c) & "\"c
            If (sFile.ToLower.StartsWith(sProjectDirectory.ToLower)) Then
                sFile = sFile.Remove(0, sProjectDirectory.Length)
            Else
                Return IO.Path.GetFileName(sFile)
            End If

            Dim sFileSplit = sFile.Replace("/"c, "\"c).Split("\"c)
            For i = 0 To sFileSplit.Length - 2
                sFileSplit(i) = "."
            Next

            Return String.Join("\", sFileSplit)
        End Function

        Public Sub RefreshRelativeFiles()
            If (Not m_ProjectOpened OrElse Not IO.File.Exists(m_ProjectFile)) Then
                Throw New ArgumentException(String.Format("Project file '{0}' not found", m_ProjectFile))
            End If

            Dim sRecursiveFiles As String() = GetRecursiveFilesFromProject()
            Dim mProjectFiles = GetFiles()

            If (sRecursiveFiles Is Nothing) Then
                Throw New ArgumentException("Unable to search for files")
            End If

            Try
                BeginUpdate()

                If (m_ProjectRelativeEnabled) Then
                    For Each sFile As String In sRecursiveFiles
                        If (m_ProjectRelativeNoInclude) Then
                            Select Case (IO.Path.GetExtension(sFile).ToLower)
                                Case ".inc"
                                    Continue For
                            End Select
                        End If

                        If (IsFileInProject(sFile, True)) Then
                            Continue For
                        End If

                        AddFile(New STRUC_PROJECT_FILE_INFO With {
                              .sGUID = Nothing,
                              .sFile = sFile,
                              .sPackedData = Nothing,
                              .bIsRelative = True
                          })

                        g_mUCProjectBrowser.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & "Loaded relative project file: " & sFile, New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(sFile))
                    Next
                End If

                For Each mInfo In mProjectFiles
                    If (Not mInfo.bIsRelative) Then
                        Continue For
                    End If

                    If (Not m_ProjectRelativeEnabled) Then
                        RemoveFileAll(mInfo.sFile, True)
                        Continue For
                    End If

                    If (m_ProjectRelativeNoInclude) Then
                        Select Case (IO.Path.GetExtension(mInfo.sFile).ToLower)
                            Case ".inc"
                                RemoveFileAll(mInfo.sFile, True)
                                Continue For
                        End Select
                    End If

                    If (Not Array.Exists(sRecursiveFiles, Function(a As String) a.ToLower = mInfo.sFile.ToLower)) Then
                        RemoveFileAll(mInfo.sFile, True)
                        Continue For
                    End If
                Next
            Finally
                EndUpdate()
            End Try
        End Sub

        Public Function CloseProject(bForce As Boolean) As Boolean
            If (Not bForce AndAlso m_ProjectChanged) Then
                Select Case (MessageBox.Show("Project has unsaved changes! Do you want to continue and discard all changes?", "Unsaved Project changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    Case DialogResult.No
                        Return False
                End Select
            End If

            ClearFiles()
            g_sProjectFile = ""

            SetProjectChanged(False)

            Return True
        End Function

        Private Function GetRecursiveFilesFromProject() As String()
            If (Not m_ProjectOpened OrElse Not IO.File.Exists(m_ProjectFile)) Then
                Return Nothing
            End If

            Dim lList As New List(Of String)
            GetRecursiveFiles(IO.Path.GetDirectoryName(m_ProjectFile), lList, 10)

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
        Try
            If (ListView_ProjectFiles.SelectedItems.Count < 1) Then
                Return
            End If

            g_lClipboardFiles.Clear()

            Try
                g_ClassProjectControl.BeginUpdate()

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
            Finally
                g_ClassProjectControl.EndUpdate()
            End Try

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

                Try
                    g_ClassProjectControl.BeginUpdate()

                    For i = g_lClipboardFiles.Count - 1 To 0 Step -1
                        g_ClassProjectControl.InsertFile(iIndex, g_lClipboardFiles(i))
                    Next
                Finally
                    g_ClassProjectControl.EndUpdate()
                End Try
            Else
                Try
                    g_ClassProjectControl.BeginUpdate()

                    For i = 0 To g_lClipboardFiles.Count - 1
                        g_ClassProjectControl.AddFile(g_lClipboardFiles(i))
                    Next
                Finally
                    g_ClassProjectControl.EndUpdate()
                End Try
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

            Try
                g_ClassProjectControl.BeginUpdate()

                For i = ListView_ProjectFiles.SelectedItems.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = ListView_ProjectFiles.SelectedItems(i).Index

                    g_ClassProjectControl.RemoveFileAt(iIndex)
                Next
            Finally
                g_ClassProjectControl.EndUpdate()
            End Try
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

            If (g_ClassProjectControl.IsFileInProject(g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File, False)) Then
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

    Private Sub ToolStripMenuItem_AddAllTabs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_AddAllTabs.Click
        Try
            Try
                g_ClassProjectControl.BeginUpdate()

                For i = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                    If (g_mFormMain.g_ClassTabControl.m_Tab(i).m_IsUnsaved) Then
                        Continue For
                    End If

                    If (g_ClassProjectControl.IsFileInProject(g_mFormMain.g_ClassTabControl.m_Tab(i).m_File, False)) Then
                        Continue For
                    End If

                    g_ClassProjectControl.AddFile(New ClassProjectControl.STRUC_PROJECT_FILE_INFO With {
                        .sGUID = Nothing,
                        .sFile = g_mFormMain.g_ClassTabControl.m_Tab(i).m_File,
                        .sPackedData = Nothing
                    })
                Next
            Finally
                g_ClassProjectControl.EndUpdate()
            End Try
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
                    Try
                        g_ClassProjectControl.BeginUpdate()

                        For Each sFile As String In i.FileNames
                            If (g_ClassProjectControl.IsFileInProject(sFile, False)) Then
                                Continue For
                            End If

                            g_ClassProjectControl.AddFile(New ClassProjectControl.STRUC_PROJECT_FILE_INFO With {
                                .sGUID = Nothing,
                                .sFile = sFile,
                                .sPackedData = Nothing
                            })
                        Next
                    Finally
                        g_ClassProjectControl.EndUpdate()
                    End Try
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
            If (mInfo.bIsRelative) Then
                Continue For
            End If

            If (Not String.IsNullOrEmpty(mInfo.sPackedData)) Then
                bPackedSelected = True
                Exit For
            End If
        Next

        ToolStripMenuItem_Open.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)

        ToolStripMenuItem_CompileAll.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
        ToolStripMenuItem_TestAll.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)

        ToolStripMenuItem_PackFile.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
        ToolStripMenuItem_ExtractFile.Enabled = bPackedSelected
        ToolStripMenuItem_DeletePack.Enabled = bPackedSelected

        ToolStripMenuItem_Cut.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
        ToolStripMenuItem_Copy.Enabled = (ListView_ProjectFiles.SelectedItems.Count > 0)
        ToolStripMenuItem_Paste.Enabled = (g_lClipboardFiles.Count > 0)

        ToolStripMenuItem_AddTab.Enabled = (Not g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IsUnsaved)

        ToolStripMenuItem_RelativeEnable.Checked = g_ClassProjectControl.m_ProjectRelativeEnabled
        ToolStripMenuItem_RelativeNoIncludes.Checked = g_ClassProjectControl.m_ProjectRelativeNoInclude

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
                    Throw New ArgumentException(String.Format("File '{0}' does not exist", mInfo.sFile))
                End If

                If (mInfo.bIsRelative) Then
                    MessageBox.Show(String.Format("File '{0}' is relative and can not be packed!", mInfo.sFile), "Unable to pack", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Continue For
                End If

                g_ClassProjectControl.PackFileData(mInfo)

                mListViewItemData.g_mData("Info") = mInfo

                g_ClassProjectControl.SetProjectChanged(True)
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
                    MessageBox.Show(String.Format("File '{0}' is not packed!", mInfo.sFile), "Unable to extract", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Continue For
                End If

                If (mInfo.bIsRelative) Then
                    MessageBox.Show(String.Format("File '{0}' is relative and can not be extracted!", mInfo.sFile), "Unable to extract", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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

                                g_ClassProjectControl.SetProjectChanged(True)
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

                g_ClassProjectControl.SetProjectChanged(True)
            Next

            g_ClassProjectControl.UpdateListViewInfo()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_RelativeRefresh_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_RelativeRefresh.Click
        Try
            g_ClassProjectControl.RefreshRelativeFiles()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_RelativeEnable_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_RelativeEnable.Click
        Try
            g_ClassProjectControl.m_ProjectRelativeEnabled = ToolStripMenuItem_RelativeEnable.Checked
            g_ClassProjectControl.RefreshRelativeFiles()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_RelativeNoIncludes_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_RelativeNoIncludes.Click
        Try
            g_ClassProjectControl.m_ProjectRelativeNoInclude = ToolStripMenuItem_RelativeNoIncludes.Checked
            g_ClassProjectControl.RefreshRelativeFiles()
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

            Try
                g_ClassProjectControl.BeginUpdate()

                For Each sFile As String In sFiles
                    If (Not IO.File.Exists(sFile)) Then
                        Continue For
                    End If

                    If (g_ClassProjectControl.IsFileInProject(sFile, False)) Then
                        Continue For
                    End If

                    g_ClassProjectControl.AddFile(New ClassProjectControl.STRUC_PROJECT_FILE_INFO With {
                        .sGUID = Nothing,
                        .sFile = sFile,
                        .sPackedData = Nothing
                    })
                Next
            Finally
                g_ClassProjectControl.EndUpdate()
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
#End Region
End Class
