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



Public Class UCExplorerBrowser
    Private g_mFormMain As FormMain
    Private g_sExplorerPath As String = ""

    Private g_bIgnoreCheckedChangedEvent As Boolean = False
    Private g_mSelectedItemsQueue As New Queue(Of ListViewItem)
    Private g_bLoaded As Boolean = False

    Property m_ExplorerPath As String
        Get
            Return g_sExplorerPath
        End Get
        Set(value As String)
            g_sExplorerPath = value

            TextBox_Path.Text = g_sExplorerPath
            TextBox_Path.Select(TextBox_Path.Text.Length - 1, 0)
        End Set
    End Property

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ImageList_ExplorerBrowser.ImageSize = New Size(ClassTools.ClassForms.ScaleDPI(16), ClassTools.ClassForms.ScaleDPI(16))
        ImageList_ExplorerBrowser.Images.Clear()
        ImageList_ExplorerBrowser.Images.Add("0", My.Resources.Ico_Rtf)
        ImageList_ExplorerBrowser.Images.Add("1", My.Resources.Ico_Folder)

        LoadViews()

        ClassTools.ClassForms.SetDoubleBuffering(ListView_ExplorerFiles, True)
    End Sub

    Private Sub UCExplorerBrowser_Load(sender As Object, e As EventArgs) Handles Me.Load
        g_bLoaded = True
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
        For i = 0 To ListView_ExplorerFiles.Items.Count - 1
            ListView_ExplorerFiles.Items(i).Selected = False
        Next

        For i = 0 To ListView_ExplorerFiles.Items.Count - 1
            If (Not ListView_ExplorerFiles.Items(i).Text.ToLower.Contains(sSearchText.ToLower)) Then
                Continue For
            End If

            ListView_ExplorerFiles.Items(i).Selected = True
            ListView_ExplorerFiles.Items(i).EnsureVisible()
        Next
    End Sub

    Private Sub ToolStripMenuItem_Home_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Home.Click
        Try
            HomeExplorer()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_DirectoryUp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DirectoryUp.Click
        Try
            GoUpExplorer()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Refresh_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Refresh.Click
        Try
            RefreshExplorer()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ContextMenuStrip_ExplorerBrowser_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_ExplorerBrowser.Opening
        ToolStripMenuItem_OpenFile.Enabled = (ListView_ExplorerFiles.SelectedItems.Count > 0)
    End Sub

    Private Sub ListView_ExplorerFiles_DoubleClick(sender As Object, e As EventArgs) Handles ListView_ExplorerFiles.DoubleClick
        Try
            If (ListView_ExplorerFiles.SelectedItems.Count < 1) Then
                Return
            End If

            Dim mListViewItemData = TryCast(ListView_ExplorerFiles.SelectedItems(0), ClassListViewItemData)
            If (mListViewItemData Is Nothing) Then
                Return
            End If

            Dim sPath As String = CStr(mListViewItemData.g_mData("Path"))
            If (String.IsNullOrEmpty(sPath)) Then
                Throw New IO.DirectoryNotFoundException("Invalid path")
            End If

            Select Case (True)
                Case IO.File.Exists(sPath)
                    Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByFile(sPath)
                    If (mTab IsNot Nothing) Then
                        mTab.SelectTab()
                    Else
                        mTab = g_mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(sPath)
                        mTab.SelectTab()
                    End If

                Case IO.Directory.Exists(sPath)
                    GoToExplorer(sPath)

                Case Else
                    Throw New IO.DirectoryNotFoundException("Invalid path")
            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_OpenFile_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OpenFile.Click
        Try
            If (ListView_ExplorerFiles.SelectedItems.Count < 1) Then
                Return
            End If

            Try
                g_mFormMain.g_ClassTabControl.BeginUpdate()

                For Each mItem In ListView_ExplorerFiles.SelectedItems
                    Dim mListViewItemData = TryCast(mItem, ClassListViewItemData)
                    If (mListViewItemData Is Nothing) Then
                        Return
                    End If

                    Dim sPath As String = CStr(mListViewItemData.g_mData("Path"))
                    If (String.IsNullOrEmpty(sPath)) Then
                        Throw New IO.DirectoryNotFoundException("Invalid path")
                    End If

                    Select Case (True)
                        Case IO.File.Exists(sPath)
                            Dim mTab = g_mFormMain.g_ClassTabControl.GetTabByFile(sPath)
                            If (mTab IsNot Nothing) Then
                                mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)
                            Else
                                mTab = g_mFormMain.g_ClassTabControl.AddTab()
                                mTab.OpenFileTab(sPath)
                                mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)
                            End If

                        Case IO.Directory.Exists(sPath)
                            GoToExplorer(sPath)

                        Case Else
                            Throw New IO.DirectoryNotFoundException("Invalid path")
                    End Select
                Next
            Finally
                g_mFormMain.g_ClassTabControl.EndUpdate()
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Filter_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Filter.CheckedChanged
        If (g_bIgnoreCheckedChangedEvent) Then
            Return
        End If

        Try
            RefreshExplorer()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try

        SaveViews()
    End Sub

    Private Sub ListView_ExplorerFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_ExplorerFiles.SelectedIndexChanged
        UpdateListViewColors()
    End Sub

    Private Sub ListView_ExplorerFiles_Invalidated(sender As Object, e As InvalidateEventArgs) Handles ListView_ExplorerFiles.Invalidated
        Static bIgnoreEvent As Boolean = False
        Static mLastBackColor As Color = Color.White
        Static mLastForeColor As Color = Color.Black

        If (Not g_bLoaded OrElse bIgnoreEvent) Then
            Return
        End If

        If (ListView_ExplorerFiles.BackColor <> mLastBackColor OrElse ListView_ExplorerFiles.ForeColor <> mLastForeColor) Then
            mLastBackColor = ListView_ExplorerFiles.BackColor
            mLastForeColor = ListView_ExplorerFiles.ForeColor

            bIgnoreEvent = True
            UpdateListViewColors()
            bIgnoreEvent = False
        End If
    End Sub

    Private Sub UpdateListViewColors()
        If (g_mSelectedItemsQueue.Count < 1 AndAlso ListView_ExplorerFiles.Items.Count < 1) Then
            Return
        End If

        Try
            ListView_ExplorerFiles.SuspendLayout()

            While (g_mSelectedItemsQueue.Count > 0)
                Dim mItem = g_mSelectedItemsQueue.Dequeue

                'Reset to parent color
                mItem.ForeColor = Color.Empty
                mItem.BackColor = Color.Empty
            End While

            If (ListView_ExplorerFiles.SelectedIndices.Count < 1) Then
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

            For Each i As Integer In ListView_ExplorerFiles.SelectedIndices
                If (ListView_ExplorerFiles.Items(i).ForeColor <> mForeColor OrElse
                         ListView_ExplorerFiles.Items(i).BackColor <> mBackColor) Then
                    ListView_ExplorerFiles.Items(i).ForeColor = mForeColor
                    ListView_ExplorerFiles.Items(i).BackColor = mBackColor

                    g_mSelectedItemsQueue.Enqueue(ListView_ExplorerFiles.Items(i))
                End If
            Next
        Finally
            ListView_ExplorerFiles.ResumeLayout()
        End Try
    End Sub

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
        ListView_ExplorerFiles.Items.Clear()

        If (String.IsNullOrEmpty(m_ExplorerPath) OrElse Not IO.Directory.Exists(m_ExplorerPath)) Then
            Throw New IO.DirectoryNotFoundException("Directory not found")
        End If

        Dim bFilter As Boolean = ToolStripMenuItem_Filter.Checked

        Try
            ListView_ExplorerFiles.BeginUpdate()

            For Each sDirectory As String In IO.Directory.GetDirectories(m_ExplorerPath)
                If (Not IO.Directory.Exists(sDirectory)) Then
                    Continue For
                End If

                Dim sName As String = New IO.DirectoryInfo(sDirectory).Name

                Dim mItem As New ClassListViewItemData(sName, "1")

                mItem.g_mData("Path") = sDirectory

                ListView_ExplorerFiles.Items.Add(mItem)
            Next

            For Each sFile As String In IO.Directory.GetFiles(m_ExplorerPath)
                If (Not IO.File.Exists(sFile)) Then
                    Continue For
                End If

                If (bFilter) Then
                    Select Case (IO.Path.GetExtension(sFile))
                        Case ".sp", ".sma", ".p", ".pwn", ".inc"
                            'Allow

                        Case Else
                            Continue For
                    End Select
                End If

                Dim sName As String = New IO.FileInfo(sFile).Name

                Dim mItem As New ClassListViewItemData(sName, "0")

                mItem.g_mData("Path") = sFile

                ListView_ExplorerFiles.Items.Add(mItem)
            Next
        Finally
            ListView_ExplorerFiles.EndUpdate()
        End Try
    End Sub

    Public Sub HomeExplorer()
        Dim mTab = g_mFormMain.g_ClassTabControl.m_ActiveTab
        If (mTab.m_IsUnsaved OrElse mTab.m_InvalidFile) Then
            Throw New IO.FileNotFoundException("Could not find file")
        End If

        GoToExplorer(mTab.m_File)
    End Sub

    Public Sub GoUpExplorer()
        If (String.IsNullOrEmpty(m_ExplorerPath) OrElse Not IO.Directory.Exists(m_ExplorerPath)) Then
            Throw New IO.DirectoryNotFoundException("Directory not found")
        End If

        GoToExplorer(IO.Path.GetFullPath(IO.Path.Combine(m_ExplorerPath, "..")))
    End Sub


    Public Sub SaveViews()
        If (String.IsNullOrEmpty(Me.Name)) Then
            Return
        End If

        Using mStream = ClassFileStreamWait.Create(ClassSettings.g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT) From {
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "FilterSourcesIncludes", If(ToolStripMenuItem_Filter.Checked, "1", "0"))
                }

                mIni.WriteKeyValue(lContent.ToArray)
            End Using
        End Using
    End Sub

    Public Sub LoadViews()
        If (String.IsNullOrEmpty(Me.Name)) Then
            Return
        End If

        Using mStream = ClassFileStreamWait.Create(ClassSettings.g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                g_bIgnoreCheckedChangedEvent = True
                ToolStripMenuItem_Filter.Checked = (mIni.ReadKeyValue(Me.Name, "FilterSourcesIncludes", "1") <> "0")
                g_bIgnoreCheckedChangedEvent = False
            End Using
        End Using
    End Sub
End Class
