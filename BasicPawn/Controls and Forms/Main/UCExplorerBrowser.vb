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



Public Class UCExplorerBrowser
    Private g_mFormMain As FormMain
    Private g_sExplorerPath As String = ""

    Property m_ExplorerPath As String
        Get
            Return g_sExplorerPath
        End Get
        Set(value As String)
            g_sExplorerPath = value

            ToolStripTextBox_Path.Text = g_sExplorerPath
            ToolStripTextBox_Path.Select(ToolStripTextBox_Path.Text.Length - 1, 0)
        End Set
    End Property

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ImageList_ExplorerBrowser.Images.Clear()
        ImageList_ExplorerBrowser.Images.Add("0", My.Resources.Ico_Rtf)
        ImageList_ExplorerBrowser.Images.Add("1", My.Resources.Ico_Folder)
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

                        Return
                    End If

                    mTab = g_mFormMain.g_ClassTabControl.AddTab()
                    mTab.OpenFileTab(sPath)
                    mTab.SelectTab()

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
                                mTab.SelectTab(500)

                                Continue For
                            End If

                            mTab = g_mFormMain.g_ClassTabControl.AddTab()
                            mTab.OpenFileTab(sPath)
                            mTab.SelectTab(500)

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

            Dim sName As String = New IO.FileInfo(sFile).Name

            Dim mItem As New ClassListViewItemData(sName, "0")

            mItem.g_mData("Path") = sFile

            ListView_ExplorerFiles.Items.Add(mItem)
        Next

        ListView_ExplorerFiles.EndUpdate()
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
End Class
