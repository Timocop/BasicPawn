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
Imports ICSharpCode.TextEditor.Document

Public Class UCBookmarkDetails
    Private g_mFormMain As FormMain

    Private g_bLoaded As Boolean = False

    Public g_ClassBookmarks As ClassBookmarks
    Public g_ClassActions As ClassActions

    Property m_ShowLocalTabsOnly As Boolean

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ImageList_Bookmarks.Images.Clear()
        ImageList_Bookmarks.Images.Add("0", My.Resources.Pin_16x16_32)
        ImageList_Bookmarks.Images.Add("1", My.Resources.Unpin_16x16_32)


        g_ClassBookmarks = New ClassBookmarks(Me)
        g_ClassBookmarks.RefreshBookmarks()

        g_ClassActions = New ClassActions(Me)

        AddHandler g_ClassBookmarks.OnBookmarksUpdated, AddressOf OnBookmarksUpdated
        AddHandler g_mFormMain.g_ClassTabControl.OnTabFullUpdate, AddressOf OnTabFullUpdate
        AddHandler g_mFormMain.g_ClassSyntaxUpdater.OnSyntaxUpdate, AddressOf OnSyntaxUpdate

        'Set double buffering to avoid annonying flickers
        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
    End Sub

    Private Sub UCBookmarkDetails_Load(sender As Object, e As EventArgs) Handles Me.Load
        g_bLoaded = True
    End Sub

    Private Sub ToolStripMenuItem_Goto_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Goto.Click
        BookmarkGotoSelected()
    End Sub

    Private Sub ToolStripMenuItem_AddBookmark_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_AddBookmark.Click
        Try
            Dim mTab = g_mFormMain.g_ClassTabControl.m_ActiveTab
            If (mTab.m_IsUnsaved) Then
                MessageBox.Show("Invalid file path.", "Unable to add bookmark", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim iCaretLine As Integer = mTab.m_TextEditor.ActiveTextAreaControl.Caret.Line

            Dim mInputTitle As New Text.StringBuilder
            mInputTitle.AppendFormat("Creating a bookmark at line {0}.", iCaretLine + 1).AppendLine()
            mInputTitle.AppendLine("Enter a name for the new bookmark:")

            Dim sName As String = InputBox(mInputTitle.ToString, "Create Bookmark", "Bookmark-" & iCaretLine + 1)
            If (String.IsNullOrEmpty(sName)) Then
                Return
            End If

            g_ClassBookmarks.AddBookmark(New ClassBookmarks.STRUC_BOOKMARK(mTab.m_File, iCaretLine, sName))
            g_ClassBookmarks.RefreshBookmarks()

            RefreshBookmarkList()
            RefreshBookmarkIconBar()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_EditBookmark_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_EditBookmark.Click
        Try
            If (ListView_Bookmarks.SelectedItems.Count < 1) Then
                Return
            End If

            Dim mLvItem = TryCast(ListView_Bookmarks.SelectedItems(0), ClassListViewItemData)
            If (mLvItem Is Nothing) Then
                Return
            End If

            Dim sName As String = CStr(mLvItem.g_mData("Name"))
            Dim sFile As String = CStr(mLvItem.g_mData("File"))
            Dim iLine As Integer = CInt(mLvItem.g_mData("Line"))

            Dim mInputTitle As New Text.StringBuilder
            mInputTitle.AppendFormat("Editing a bookmark at line {0}.", iLine + 1).AppendLine()
            mInputTitle.AppendLine("Enter a name for the existing bookmark:")

            Dim sNewName As String = InputBox(mInputTitle.ToString, "Edit Bookmark", sName)
            If (String.IsNullOrEmpty(sNewName)) Then
                Return
            End If

            g_ClassBookmarks.AddBookmark(New ClassBookmarks.STRUC_BOOKMARK(sFile, iLine, sNewName))
            g_ClassBookmarks.RefreshBookmarks()

            RefreshBookmarkList()
            RefreshBookmarkIconBar()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_RemoveBookmark_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_RemoveBookmark.Click
        Try
            If (ListView_Bookmarks.SelectedItems.Count < 1) Then
                Return
            End If

            Dim lBookmarks As New List(Of ClassBookmarks.STRUC_BOOKMARK)

            For Each mItem In ListView_Bookmarks.SelectedItems
                Dim mLvItem = TryCast(mItem, ClassListViewItemData)
                If (mLvItem Is Nothing) Then
                    Continue For
                End If

                Dim sFile As String = CStr(mLvItem.g_mData("File"))
                Dim iLine As Integer = CInt(mLvItem.g_mData("Line"))

                lBookmarks.Add(New ClassBookmarks.STRUC_BOOKMARK(sFile, iLine))
            Next

            g_ClassBookmarks.RemoveBookmark(lBookmarks.ToArray)
            g_ClassBookmarks.RefreshBookmarks()

            RefreshBookmarkList()
            RefreshBookmarkIconBar()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_RefreshBookmark_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_RefreshBookmark.Click
        Try
            g_ClassBookmarks.RefreshBookmarks()

            RefreshBookmarkList()
            RefreshBookmarkIconBar()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ContextMenuStrip_Bookmarks_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Bookmarks.Opening
        ToolStripMenuItem_EditBookmark.Enabled = (ListView_Bookmarks.SelectedItems.Count > 0)
        ToolStripMenuItem_RemoveBookmark.Enabled = (ListView_Bookmarks.SelectedItems.Count > 0)

        ToolStripMenuItem_LocalBookmarksOnly.Checked = m_ShowLocalTabsOnly
    End Sub

    Private Sub ToolStripMenuItem_LocalBookmarksOnly_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_LocalBookmarksOnly.CheckedChanged
        If (m_ShowLocalTabsOnly = ToolStripMenuItem_LocalBookmarksOnly.Checked) Then
            Return
        End If

        m_ShowLocalTabsOnly = ToolStripMenuItem_LocalBookmarksOnly.Checked

        g_ClassBookmarks.RefreshBookmarks()

        RefreshBookmarkList()
        RefreshBookmarkIconBar()
    End Sub

    Private Sub UCBookmarkDetails_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If (Not Me.Visible) Then
            Return
        End If

        RefreshBookmarkList()
        RefreshBookmarkIconBar()
    End Sub

    Private Sub ListView_Bookmarks_DoubleClick(sender As Object, e As EventArgs) Handles ListView_Bookmarks.DoubleClick
        BookmarkGotoSelected()
    End Sub

    Private Sub ListView_Bookmarks_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_Bookmarks.SelectedIndexChanged
        UpdateListViewColors()
    End Sub

    Private Sub ListView_Bookmarks_Invalidated(sender As Object, e As InvalidateEventArgs) Handles ListView_Bookmarks.Invalidated
        Static bIgnoreEvent As Boolean = False
        Static mLastBackColor As Color = Color.White
        Static mLastForeColor As Color = Color.Black

        If (Not g_bLoaded OrElse bIgnoreEvent) Then
            Return
        End If

        If (ListView_Bookmarks.BackColor <> mLastBackColor OrElse ListView_Bookmarks.ForeColor <> mLastForeColor) Then
            mLastBackColor = ListView_Bookmarks.BackColor
            mLastForeColor = ListView_Bookmarks.ForeColor

            bIgnoreEvent = True
            UpdateListViewColors()
            bIgnoreEvent = False
        End If
    End Sub

    Private Sub UpdateListViewColors()
        If (ListView_Bookmarks.Items.Count < 1) Then
            Return
        End If

        Try
            ListView_Bookmarks.SuspendLayout()

            For i = 0 To ListView_Bookmarks.Items.Count - 1
                If (ListView_Bookmarks.Items(i).ForeColor <> ListView_Bookmarks.ForeColor OrElse
                            ListView_Bookmarks.Items(i).BackColor <> ListView_Bookmarks.BackColor) Then
                    ListView_Bookmarks.Items(i).ForeColor = ListView_Bookmarks.ForeColor
                    ListView_Bookmarks.Items(i).BackColor = ListView_Bookmarks.BackColor
                End If
            Next

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

            For Each i As Integer In ListView_Bookmarks.SelectedIndices
                If (ListView_Bookmarks.Items(i).ForeColor <> mForeColor OrElse
                         ListView_Bookmarks.Items(i).BackColor <> mBackColor) Then
                    ListView_Bookmarks.Items(i).ForeColor = mForeColor
                    ListView_Bookmarks.Items(i).BackColor = mBackColor
                End If
            Next
        Finally
            ListView_Bookmarks.ResumeLayout()
        End Try
    End Sub


    Public Sub RefreshBookmarkIconBar()
        RefreshBookmarkIconBar(g_mFormMain.g_ClassTabControl.m_ActiveTab)
    End Sub

    Public Sub RefreshBookmarkIconBar(mTab As ClassTabControl.SourceTabPage)
        Dim lExistBookmarks As New List(Of ClassTextEditorTools.ClassBookmarkMark)
        Dim lNewBookmarks As New List(Of ClassBookmarks.STRUC_BOOKMARK)
        Dim lAddBookmarks As New List(Of ClassBookmarks.STRUC_BOOKMARK)

        If (mTab.m_IsUnsaved) Then
            mTab.m_TextEditor.Document.BookmarkManager.RemoveMarks(Function(x As Bookmark) TypeOf x Is ClassTextEditorTools.ClassBookmarkMark)
            Return
        End If

        For Each mBookmark In mTab.m_TextEditor.Document.BookmarkManager.Marks
            If (TypeOf mBookmark Is ClassTextEditorTools.ClassBookmarkMark) Then
                lExistBookmarks.Add(DirectCast(mBookmark, ClassTextEditorTools.ClassBookmarkMark))
            End If
        Next

        'Use mTab.m_File, since we dont want other includes in our current iconbar
        For Each mBookmark In g_ClassBookmarks.GetBookmarks(mTab.m_File)
            If (lAddBookmarks.Exists(Function(x As ClassBookmarks.STRUC_BOOKMARK)
                                         Return (mBookmark.iLine = x.iLine)
                                     End Function)) Then
                Continue For
            End If

            lNewBookmarks.Add(mBookmark)

            If (lExistBookmarks.Exists(Function(x As ClassTextEditorTools.ClassBookmarkMark)
                                           Return (mBookmark.iLine = x.LineNumber)
                                       End Function)) Then
                Continue For
            End If

            lAddBookmarks.Add(mBookmark)
        Next

        'Remove non-existent bookmarks
        Dim mRemoveBookmarks = lExistBookmarks.FindAll(Function(x As ClassTextEditorTools.ClassBookmarkMark)
                                                           Return Not lNewBookmarks.Exists(Function(y As ClassBookmarks.STRUC_BOOKMARK)
                                                                                               Return (x.LineNumber = y.iLine)
                                                                                           End Function)
                                                       End Function)

        For i = 0 To mRemoveBookmarks.Count - 1
            mTab.m_TextEditor.Document.BookmarkManager.RemoveMark(mRemoveBookmarks(i))
            mTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, mRemoveBookmarks(i).LineNumber))
        Next

        'Add new items
        For i = 0 To lAddBookmarks.Count - 1
            mTab.m_TextEditor.Document.BookmarkManager.AddMark(New ClassTextEditorTools.ClassBookmarkMark(Nothing, New TextLocation(0, lAddBookmarks(i).iLine), lAddBookmarks(i).sName))
            mTab.m_TextEditor.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, lAddBookmarks(i).iLine))
        Next

        mTab.m_TextEditor.Document.CommitUpdate()
    End Sub

    Public Sub RefreshBookmarkList()
        Dim lLvDataExistItems As New List(Of ClassListViewItemData)
        Dim lLvDataNewItems As New List(Of ClassBookmarks.STRUC_BOOKMARK)
        Dim lLvDataAddItems As New List(Of ClassBookmarks.STRUC_BOOKMARK)

        For Each mLvItem As ListViewItem In ListView_Bookmarks.Items
            If (TypeOf mLvItem Is ClassListViewItemData) Then
                lLvDataExistItems.Add(DirectCast(mLvItem, ClassListViewItemData))
            End If
        Next

        Dim mTabs As ClassTabControl.SourceTabPage()

        If (m_ShowLocalTabsOnly) Then
            mTabs = {g_mFormMain.g_ClassTabControl.m_ActiveTab}
        Else
            mTabs = g_mFormMain.g_ClassTabControl.GetAllTabs
        End If

        For Each mTab In mTabs
            If (mTab.m_IsUnsaved) Then
                Continue For
            End If

            For Each mBookmark In g_ClassBookmarks.GetBookmarks(mTab)
                If (lLvDataAddItems.Exists(Function(x As ClassBookmarks.STRUC_BOOKMARK)
                                               Return (mBookmark.sName = x.sName AndAlso mBookmark.sFile = x.sFile AndAlso mBookmark.iLine = x.iLine)
                                           End Function)) Then
                    Continue For
                End If

                lLvDataNewItems.Add(mBookmark)

                If (lLvDataExistItems.Exists(Function(x As ClassListViewItemData)
                                                 Dim sNameX As String = CStr(x.g_mData("Name"))
                                                 Dim sFileX As String = CStr(x.g_mData("File"))
                                                 Dim iLineX As Integer = CInt(x.g_mData("Line"))

                                                 Return (mBookmark.sName = sNameX AndAlso mBookmark.sFile = sFileX AndAlso mBookmark.iLine = iLineX)
                                             End Function)) Then
                    Continue For
                End If

                lLvDataAddItems.Add(mBookmark)
            Next
        Next

        'Remove non-existent from old
        Dim mLvDatRemove = lLvDataExistItems.FindAll(Function(x As ClassListViewItemData)
                                                         Dim sNameX As String = CStr(x.g_mData("Name"))
                                                         Dim sFileX As String = CStr(x.g_mData("File"))
                                                         Dim iLineX As Integer = CInt(x.g_mData("Line"))

                                                         Return Not lLvDataNewItems.Exists(Function(y As ClassBookmarks.STRUC_BOOKMARK)
                                                                                               Return (sNameX = y.sName AndAlso sFileX = y.sFile AndAlso iLineX = y.iLine)
                                                                                           End Function)

                                                     End Function)

        If (mLvDatRemove.Count > 0 OrElse lLvDataAddItems.Count > 0) Then
            Try
                ListView_Bookmarks.BeginUpdate()

                For i = 0 To mLvDatRemove.Count - 1
                    ListView_Bookmarks.Items.Remove(mLvDatRemove(i))
                Next

                'Add new items
                For i = 0 To lLvDataAddItems.Count - 1
                    Dim mLvItem = New ClassListViewItemData(New String() {lLvDataAddItems(i).sName, lLvDataAddItems(i).sFile, CStr(lLvDataAddItems(i).iLine + 1)}, "1")
                    mLvItem.g_mData("Name") = lLvDataAddItems(i).sName
                    mLvItem.g_mData("File") = lLvDataAddItems(i).sFile
                    mLvItem.g_mData("Line") = lLvDataAddItems(i).iLine
                    ListView_Bookmarks.Items.Add(mLvItem)
                Next
            Finally
                ListView_Bookmarks.EndUpdate()
            End Try
        End If
    End Sub

    Private Sub OnBookmarksUpdated()
        'Make sure TabControl is initalized. We dont want to loop through tabs too early.
        If (Not g_mFormMain.g_ClassTabControl.m_IsInitalized) Then
            Return
        End If

        ClassThread.ExecAsync(Me, Sub()
                                      RefreshBookmarkList()
                                      RefreshBookmarkIconBar()
                                  End Sub)
    End Sub

    Private Sub OnTabFullUpdate(mTab As ClassTabControl.SourceTabPage())
        RefreshBookmarkList()
        RefreshBookmarkIconBar()
    End Sub

    Private Sub OnSyntaxUpdate(bIsFormMainFocused As Boolean, iCaretOffset As Integer, mCaretPos As Point)
        Static mBookmarkListUpdateDelay As New TimeSpan(0, 0, 30)
        Static mBookmarkIconUpdateDelay As New TimeSpan(0, 0, 5)

        Static dLastBookmarkListUpdateDelay As Date = (Now + mBookmarkListUpdateDelay)
        Static dLastBookmarkIconUpdateDelay As Date = (Now + mBookmarkIconUpdateDelay)

        If (bIsFormMainFocused AndAlso dLastBookmarkListUpdateDelay < Now) Then
            dLastBookmarkListUpdateDelay = (Now + mBookmarkListUpdateDelay)

            ClassThread.ExecAsync(g_mFormMain, Sub() RefreshBookmarkList())
        End If

        If (bIsFormMainFocused AndAlso dLastBookmarkIconUpdateDelay < Now) Then
            dLastBookmarkIconUpdateDelay = (Now + mBookmarkIconUpdateDelay)

            ClassThread.ExecAsync(g_mFormMain, Sub() RefreshBookmarkIconBar())
        End If
    End Sub

    Private Sub CleanUp()
        If (g_mFormMain.g_ClassSyntaxUpdater IsNot Nothing) Then
            RemoveHandler g_mFormMain.g_ClassSyntaxUpdater.OnSyntaxUpdate, AddressOf OnSyntaxUpdate
        End If

        If (g_mFormMain.g_ClassTabControl IsNot Nothing) Then
            RemoveHandler g_mFormMain.g_ClassTabControl.OnTabFullUpdate, AddressOf OnTabFullUpdate
        End If

        If (g_ClassBookmarks IsNot Nothing) Then
            RemoveHandler g_ClassBookmarks.OnBookmarksUpdated, AddressOf OnBookmarksUpdated
        End If

        If (g_ClassActions IsNot Nothing) Then
            g_ClassActions.Dispose()
            g_ClassActions = Nothing
        End If

        If (g_ClassBookmarks IsNot Nothing) Then
            g_ClassBookmarks.Dispose()
            g_ClassBookmarks = Nothing
        End If
    End Sub

    Private Sub BookmarkGotoSelected()
        Try
            If (ListView_Bookmarks.SelectedItems.Count < 1) Then
                Return
            End If

            Dim mLvItem = TryCast(ListView_Bookmarks.SelectedItems(0), ClassListViewItemData)
            If (mLvItem Is Nothing) Then
                Return
            End If

            Dim sName As String = CStr(mLvItem.g_mData("Name"))
            Dim sFile As String = CStr(mLvItem.g_mData("File"))
            Dim iLine As Integer = CInt(mLvItem.g_mData("Line"))

            If (String.IsNullOrEmpty(sFile)) Then
                MessageBox.Show("Invalid path", "Unable to open path", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If (iLine < 0) Then
                MessageBox.Show("Invalid line", "Unable to open path", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim bForceEnd As Boolean = False
            While True
                For i = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                    If (g_mFormMain.g_ClassTabControl.m_Tab(i).m_IsUnsaved OrElse g_mFormMain.g_ClassTabControl.m_Tab(i).m_InvalidFile) Then
                        Continue For
                    End If

                    Dim sTabFile As String = g_mFormMain.g_ClassTabControl.m_Tab(i).m_File.Replace("/"c, "\"c)

                    'Try to find using absolute and relative path
                    If (sTabFile.ToLower.EndsWith(sFile.ToLower)) Then
                        Dim iLineNum As Integer = ClassTools.ClassMath.ClampInt(0, g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.TotalNumberOfLines - 1, iLine)

                        g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.Caret.Line = iLineNum
                        g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.Caret.Column = 0
                        g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.SelectionManager.ClearSelection()

                        Dim iLineLen As Integer = g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.GetLineSegment(iLineNum).Length

                        Dim iStart As New TextLocation(0, iLineNum)
                        Dim iEnd As New TextLocation(iLineLen, iLineNum)

                        g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(iStart, iEnd)

                        If (g_mFormMain.g_ClassTabControl.m_ActiveTabIndex <> i) Then
                            g_mFormMain.g_ClassTabControl.SelectTab(i)
                        End If

                        Return
                    End If
                Next

                If (bForceEnd) Then
                    Exit While
                End If

                If (IO.File.Exists(sFile)) Then
                    Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
                    mTab.OpenFileTab(sFile)
                    mTab.SelectTab()

                    bForceEnd = True
                End If

                If (bForceEnd) Then
                    Continue While
                End If

                For Each mInclude In g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IncludeFilesFull.ToArray
                    If (String.IsNullOrEmpty(mInclude.Value) OrElse Not IO.File.Exists(mInclude.Value)) Then
                        Continue For
                    End If

                    Dim sIncFile As String = mInclude.Value.Replace("/"c, "\"c)

                    'Try to find using absolute and relative path
                    If (sIncFile.ToLower.EndsWith(sFile.ToLower)) Then
                        Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(mInclude.Value)
                        mTab.SelectTab()

                        bForceEnd = True
                        Continue While
                    End If
                Next

                Exit While
            End While

            MessageBox.Show(String.Format("Could not find path '{0}'!", sFile), "Unable to open path", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Class ClassBookmarks
        Implements IDisposable

        Private ReadOnly g_sBookmarkPath As String = IO.Path.Combine(Application.StartupPath, "bookmarks.ini")
        Private ReadOnly g_mBookmarkCache As New ClassSyncList(Of STRUC_BOOKMARK)
        Private g_mBookmarkWatcher As IO.FileSystemWatcher

        Private g_mUCBookmarkDetails As UCBookmarkDetails

        Public Event OnBookmarksUpdated()

        Public Sub New(f As UCBookmarkDetails)
            g_mUCBookmarkDetails = f

            g_mBookmarkWatcher = New IO.FileSystemWatcher
            g_mBookmarkWatcher.BeginInit()
            g_mBookmarkWatcher.Path = IO.Path.GetDirectoryName(g_sBookmarkPath)
            g_mBookmarkWatcher.Filter = IO.Path.GetFileName(g_sBookmarkPath)
            g_mBookmarkWatcher.NotifyFilter = IO.NotifyFilters.LastWrite Or IO.NotifyFilters.Size
            g_mBookmarkWatcher.IncludeSubdirectories = False
            g_mBookmarkWatcher.EnableRaisingEvents = True
            g_mBookmarkWatcher.EndInit()

            AddHandler g_mBookmarkWatcher.Changed, AddressOf OnBookmarkChanged
        End Sub

        Public Sub AddBookmark(mBookmark As STRUC_BOOKMARK)
            AddBookmark({mBookmark})
        End Sub

        Public Sub AddBookmark(mBookmark As STRUC_BOOKMARK())
            If (mBookmark.Length < 1) Then
                Return
            End If

            Using mStream = ClassFileStreamWait.Create(g_sBookmarkPath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT)

                    For i = 0 To mBookmark.Length - 1
                        lContent.Add(New ClassIni.STRUC_INI_CONTENT(mBookmark(i).sFile.ToLower, CStr(mBookmark(i).iLine), mBookmark(i).sName))
                    Next

                    mIni.WriteKeyValue(lContent.ToArray)
                End Using
            End Using
        End Sub

        Public Sub RemoveBookmark(mBookmark As STRUC_BOOKMARK)
            RemoveBookmark({mBookmark})
        End Sub

        Public Sub RemoveBookmark(mBookmark As STRUC_BOOKMARK())
            If (mBookmark.Length < 1) Then
                Return
            End If

            Using mStream = ClassFileStreamWait.Create(g_sBookmarkPath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT)

                    For i = 0 To mBookmark.Length - 1
                        lContent.Add(New ClassIni.STRUC_INI_CONTENT(mBookmark(i).sFile.ToLower, CStr(mBookmark(i).iLine), Nothing))
                    Next

                    mIni.WriteKeyValue(lContent.ToArray)
                End Using
            End Using
        End Sub

        Public Function GetBookmarks() As STRUC_BOOKMARK()
            Return g_mBookmarkCache.ToArray
        End Function

        Public Function GetBookmarks(sFile As String) As STRUC_BOOKMARK()
            Return g_mBookmarkCache.FindAll(Function(x As STRUC_BOOKMARK) x.sFile.ToLower = sFile.ToLower).ToArray
        End Function

        Public Function GetBookmarks(mTab As ClassTabControl.SourceTabPage) As STRUC_BOOKMARK()
            Dim lBookmarks As New List(Of STRUC_BOOKMARK)

            For Each mItem In mTab.m_IncludeFiles
                lBookmarks.AddRange(g_mBookmarkCache.FindAll(Function(x As STRUC_BOOKMARK) x.sFile.ToLower = mItem.Value.ToLower))
            Next

            Return lBookmarks.ToArray
        End Function

        Public Sub RefreshBookmarks()
            Using mStream = ClassFileStreamWait.Create(g_sBookmarkPath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    Dim mBookmarkDic As New Dictionary(Of String, STRUC_BOOKMARK)

                    For Each mItem In mIni.ReadEverything
                        Dim sFile As String = mItem.sSection.ToLower
                        Dim iLine As Integer = 0
                        Dim sName As String = mItem.sValue

                        If (Not Integer.TryParse(mItem.sKey, iLine) OrElse iLine < 0) Then
                            Continue For
                        End If

                        mBookmarkDic(sFile & "|" & iLine) = New STRUC_BOOKMARK(sFile, iLine, sName)
                    Next

                    g_mBookmarkCache.DoSync(Sub()
                                                g_mBookmarkCache.Clear()
                                                g_mBookmarkCache.AddRange(mBookmarkDic.Values)
                                            End Sub)
                End Using
            End Using

            RaiseEvent OnBookmarksUpdated()
        End Sub

        Private Sub OnBookmarkChanged(sender As Object, e As IO.FileSystemEventArgs)
            Try
                If (g_sBookmarkPath.ToLower <> e.FullPath.ToLower) Then
                    Return
                End If

                RefreshBookmarks()
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Class STRUC_BOOKMARK
            Public sFile As String
            Public iLine As Integer
            Public sName As String

            Sub New(_File As String, _Line As Integer)
                sFile = _File
                iLine = _Line
                sName = ""
            End Sub

            Sub New(_File As String, _Line As Integer, _Name As String)
                sFile = _File
                iLine = _Line
                sName = _Name
            End Sub
        End Class

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects). 
                    If (g_mBookmarkWatcher IsNot Nothing) Then
                        RemoveHandler g_mBookmarkWatcher.Changed, AddressOf OnBookmarkChanged
                    End If

                    If (g_mBookmarkWatcher IsNot Nothing) Then
                        g_mBookmarkWatcher.Dispose()
                        g_mBookmarkWatcher = Nothing
                    End If
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class

    Class ClassActions
        Implements IDisposable

        Private g_mUCBookmarkDetails As UCBookmarkDetails

        Sub New(f As UCBookmarkDetails)
            g_mUCBookmarkDetails = f

            AddHandler g_mUCBookmarkDetails.g_mFormMain.g_ClassTabControl.OnTextEditorTabDetailsAction, AddressOf OnTextEditorTabDetailsAction
            AddHandler g_mUCBookmarkDetails.g_mFormMain.g_ClassTabControl.OnTextEditorTabDetailsMove, AddressOf OnTextEditorTabDetailsMove
        End Sub

        Public Sub OnTextEditorTabDetailsAction(mTab As ClassTabControl.SourceTabPage, iDetailsTabIndex As Integer, bIsSpecialAction As Boolean, iKeys As Keys)
            If (iDetailsTabIndex <> g_mUCBookmarkDetails.g_mFormMain.TabPage_Bookmarks.TabIndex) Then
                Return
            End If

            g_mUCBookmarkDetails.BookmarkGotoSelected()
        End Sub

        Public Sub OnTextEditorTabDetailsMove(mTab As ClassTabControl.SourceTabPage, iDetailsTabIndex As Integer, iDirection As Integer, iKeys As Keys)
            'Check if the tab is actualy selected, if not, return
            If (iDetailsTabIndex <> g_mUCBookmarkDetails.g_mFormMain.TabPage_Bookmarks.TabIndex) Then
                Return
            End If

            If (iDirection = 0) Then
                Return
            End If

            Static iLastCount As Integer = 0
            If (g_mUCBookmarkDetails.ListView_Bookmarks.Items.Count < 1) Then
                Return
            End If

            Dim bCountChanged = (g_mUCBookmarkDetails.ListView_Bookmarks.Items.Count <> iLastCount)
            iLastCount = g_mUCBookmarkDetails.ListView_Bookmarks.Items.Count

            If (bCountChanged OrElse g_mUCBookmarkDetails.ListView_Bookmarks.SelectedItems.Count < 1) Then
                g_mUCBookmarkDetails.ListView_Bookmarks.SelectedIndices.Clear()
                g_mUCBookmarkDetails.ListView_Bookmarks.Items(g_mUCBookmarkDetails.ListView_Bookmarks.Items.Count - 1).Selected = True
                g_mUCBookmarkDetails.ListView_Bookmarks.Items(g_mUCBookmarkDetails.ListView_Bookmarks.Items.Count - 1).EnsureVisible()
            End If

            'What? Mmh...
            If (g_mUCBookmarkDetails.ListView_Bookmarks.SelectedItems.Count < 1) Then
                Return
            End If

            Dim iCount As Integer = g_mUCBookmarkDetails.ListView_Bookmarks.Items.Count
            Dim iNewIndex As Integer = g_mUCBookmarkDetails.ListView_Bookmarks.SelectedIndices(0) + iDirection

            If (iNewIndex > -1 AndAlso iNewIndex < iCount) Then
                g_mUCBookmarkDetails.ListView_Bookmarks.SelectedIndices.Clear()
                g_mUCBookmarkDetails.ListView_Bookmarks.Items(iNewIndex).Selected = True
                g_mUCBookmarkDetails.ListView_Bookmarks.Items(iNewIndex).EnsureVisible()
            End If
        End Sub


#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    If (g_mUCBookmarkDetails.g_mFormMain.g_ClassTabControl IsNot Nothing) Then
                        RemoveHandler g_mUCBookmarkDetails.g_mFormMain.g_ClassTabControl.OnTextEditorTabDetailsAction, AddressOf OnTextEditorTabDetailsAction
                        RemoveHandler g_mUCBookmarkDetails.g_mFormMain.g_ClassTabControl.OnTextEditorTabDetailsMove, AddressOf OnTextEditorTabDetailsMove
                    End If
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Class
