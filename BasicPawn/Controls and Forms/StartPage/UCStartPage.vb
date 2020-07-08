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


Public Class UCStartPage
    Public g_mFormMain As FormMain
    Public g_mClassRecentItems As New ClassRecentItems(Me)


    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        ClassControlStyle.SetNameFlag(TableLayoutPanel1, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_KEEP_COLOR)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl2, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl3, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)

        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)
    End Sub

    Private Sub UCStartPage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub UCStartPage_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        g_mFormMain.MenuStrip_BasicPawn.Visible = Not Me.Visible
        g_mFormMain.StatusStrip_BasicPawn.Visible = Not Me.Visible
        g_mFormMain.SplitContainer_ToolboxSourceAndDetails.Visible = Not Me.Visible

        g_mClassRecentItems.ClearRecentItems()

        If (Not Me.Visible) Then
            Timer_DelayUpdate.Stop()
            Return
        End If

        ClassControlStyle.UpdateControls(Me)

        Timer_DelayUpdate.Start()
    End Sub

    Private Sub Timer_DelayUpdate_Tick(sender As Object, e As EventArgs) Handles Timer_DelayUpdate.Tick
        Try
            Timer_DelayUpdate.Stop()

            g_mClassRecentItems.RefreshRecentItems()

            Dim bProjectsFound As Boolean = False
            Dim bFilesFound As Boolean = False

            For Each iItem In g_mClassRecentItems.GetRecentItems
                If (iItem.m_IsProjectFile) Then
                    bProjectsFound = True
                Else
                    bFilesFound = True
                End If
            Next

            If (Not bProjectsFound) Then
                With New Label
                    .SuspendLayout()

                    .Text = "No recent projects found!"
                    .Font = New Font(Me.Font.FontFamily, 12, FontStyle.Regular)
                    .AutoSize = False
                    .TextAlign = ContentAlignment.MiddleCenter

                    .Parent = RecentListBox_Projects
                    .Dock = DockStyle.Fill
                    .Show()

                    .ResumeLayout()
                End With
            End If

            If (Not bFilesFound) Then
                With New Label
                    .SuspendLayout()

                    .Text = "No recent files found!"
                    .Font = New Font(Me.Font.FontFamily, 12, FontStyle.Regular)
                    .AutoSize = False
                    .TextAlign = ContentAlignment.MiddleCenter

                    .Parent = RecentListBox_Files
                    .Dock = DockStyle.Fill
                    .Show()

                    .ResumeLayout()
                End With
            End If

            ClassControlStyle.UpdateControls(Me)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub LinkLabel_New_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_New.LinkClicked
        g_mFormMain.g_ClassTabControl.AddTab(True, False, False, False)

        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User created a new source file")

        Me.Hide()
    End Sub

    Private Sub LinkLabel_NewTemplate_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_NewTemplate.LinkClicked
        g_mFormMain.g_ClassTabControl.AddTab(True, True, False, False)

        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User created a new source file")

        Me.Hide()
    End Sub

    Private Sub LinkLabel_OpenNew_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_OpenNew.LinkClicked
        Try
            Dim mRecentItems = g_mClassRecentItems.GetRecentItems
            Dim bSomethingSelected As Boolean = False
            Dim bAppendFiles As Boolean = False

            For i = mRecentItems.Length - 1 To 0 Step -1
                If (Not mRecentItems(i).m_Checked) Then
                    Continue For
                End If

                bSomethingSelected = True
                Exit For
            Next

            If (Not bSomethingSelected) Then
                MessageBox.Show("No file selected to open!", "Could not open file", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Try
                g_mFormMain.g_ClassTabControl.BeginUpdate()

                'Close all
                g_mFormMain.g_ClassTabControl.RemoveAllTabs()

                For i = mRecentItems.Length - 1 To 0 Step -1
                    Try
                        If (Not mRecentItems(i).m_Checked) Then
                            Continue For
                        End If

                        If (mRecentItems(i).m_IsProjectFile) Then
                            g_mFormMain.g_mUCProjectBrowser.g_ClassProjectControl.LoadProject(mRecentItems(i).m_RecentFile, bAppendFiles, ClassSettings.g_bSettingsAutoOpenProjectFiles)
                            bAppendFiles = True
                        Else
                            Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
                            mTab.OpenFileTab(mRecentItems(i).m_RecentFile)
                            mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)
                        End If
                    Catch ex As Exception
                        ClassExceptionLog.WriteToLogMessageBox(ex)
                    End Try
                Next

                g_mFormMain.g_ClassTabControl.RemoveUnsavedTabsLeft()
                Me.Hide()
            Finally
                g_mFormMain.g_ClassTabControl.EndUpdate()
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub LinkLabel_Open_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_Open.LinkClicked
        Dim mRecentItems = g_mClassRecentItems.GetRecentItems
        Dim bSomethingSelected As Boolean = False
        Dim bAppendFiles As Boolean = False

        For i = mRecentItems.Length - 1 To 0 Step -1
            If (Not mRecentItems(i).m_Checked) Then
                Continue For
            End If

            bSomethingSelected = True
            Exit For
        Next

        If (Not bSomethingSelected) Then
            MessageBox.Show("No file selected to open!", "Could not open file", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            g_mFormMain.g_ClassTabControl.BeginUpdate()

            For i = mRecentItems.Length - 1 To 0 Step -1
                Try
                    If (Not mRecentItems(i).m_Checked) Then
                        Continue For
                    End If

                    If (mRecentItems(i).m_IsProjectFile) Then
                        g_mFormMain.g_mUCProjectBrowser.g_ClassProjectControl.LoadProject(mRecentItems(i).m_RecentFile, bAppendFiles, ClassSettings.g_bSettingsAutoOpenProjectFiles)
                        bAppendFiles = True
                    Else
                        Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(mRecentItems(i).m_RecentFile)
                        mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)
                    End If
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            Next

            g_mFormMain.g_ClassTabControl.RemoveUnsavedTabsLeft()
            Me.Hide()
        Finally
            g_mFormMain.g_ClassTabControl.EndUpdate()
        End Try
    End Sub


    Private Sub RecentListBox_Files_OnButtonClick(iIndex As Integer) Handles RecentListBox_Files.OnButtonClick
        Try
            Dim mItem = TryCast(RecentListBox_Files.Items(iIndex), ClassRecentListBox.ClassRecentItem)
            If (mItem Is Nothing) Then
                Return
            End If

            g_mClassRecentItems.RemoveRecent(mItem.m_RecentFile)
            RecentListBox_Files.Items.RemoveAt(iIndex)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub RecentListBox_Projects_OnButtonClick(iIndex As Integer) Handles RecentListBox_Projects.OnButtonClick
        Try
            Dim mItem = TryCast(RecentListBox_Projects.Items(iIndex), ClassRecentListBox.ClassRecentItem)
            If (mItem Is Nothing) Then
                Return
            End If

            g_mClassRecentItems.RemoveRecent(mItem.m_RecentFile)
            RecentListBox_Projects.Items.RemoveAt(iIndex)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub


    Private Sub RecentListBox_Files_OnItemDoubleClick(iIndex As Integer) Handles RecentListBox_Files.OnItemDoubleClick
        Try
            Dim mItem = TryCast(RecentListBox_Files.Items(iIndex), ClassRecentListBox.ClassRecentItem)
            If (mItem Is Nothing) Then
                Return
            End If

            Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
            mTab.OpenFileTab(mItem.m_RecentFile)
            mTab.SelectTab()

            g_mFormMain.g_ClassTabControl.RemoveUnsavedTabsLeft()

            Me.Hide()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub RecentListBox_Projects_OnItemDoubleClick(iIndex As Integer) Handles RecentListBox_Projects.OnItemDoubleClick
        Try
            Dim mItem = TryCast(RecentListBox_Projects.Items(iIndex), ClassRecentListBox.ClassRecentItem)
            If (mItem Is Nothing) Then
                Return
            End If

            g_mFormMain.g_mUCProjectBrowser.g_ClassProjectControl.LoadProject(mItem.m_RecentFile, False, ClassSettings.g_bSettingsAutoOpenProjectFiles)

            g_mFormMain.g_ClassTabControl.RemoveUnsavedTabsLeft()

            Me.Hide()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ButtonSmall_Close_Click(sender As Object, e As EventArgs) Handles ButtonSmall_Close.Click
        Me.Hide()
    End Sub

    Class ClassRecentItems
        Private g_mUCStartPage As UCStartPage
        Private Const RECENT_SECTION = "Recent"

        Public Sub New(f As UCStartPage)
            g_mUCStartPage = f
        End Sub

        ReadOnly Property m_RecentIni As String
            Get
                Return IO.Path.Combine(Application.StartupPath, "recent.ini")
            End Get
        End Property

        Public Sub RemoveRecent(sFile As String)
            If (Not IO.File.Exists(m_RecentIni)) Then
                Return
            End If

            Using mStream = ClassFileStreamWait.Create(m_RecentIni, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT)

                    For Each iItem In mIni.ReadEverything
                        If (iItem.sSection <> RECENT_SECTION) Then
                            Continue For
                        End If

                        If (iItem.sValue.ToLower <> sFile.ToLower) Then
                            Continue For
                        End If

                        lContent.Add(New ClassIni.STRUC_INI_CONTENT(iItem.sSection, iItem.sKey, Nothing))
                    Next

                    mIni.WriteKeyValue(lContent.ToArray)
                End Using
            End Using
        End Sub

        Public Sub AddRecent(sFile As String)
            Using mStream = ClassFileStreamWait.Create(m_RecentIni, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT)

                    If (IO.File.Exists(m_RecentIni)) Then
                        For Each iItem In mIni.ReadEverything
                            If (iItem.sSection <> RECENT_SECTION) Then
                                Continue For
                            End If

                            If (iItem.sValue.ToLower <> sFile.ToLower) Then
                                Continue For
                            End If

                            lContent.Add(New ClassIni.STRUC_INI_CONTENT(iItem.sSection, iItem.sKey, Nothing))
                        Next
                    End If

                    lContent.Add(New ClassIni.STRUC_INI_CONTENT(RECENT_SECTION, Guid.NewGuid.ToString, sFile))

                    mIni.WriteKeyValue(lContent.ToArray)
                End Using
            End Using
        End Sub

        Public Function GetRecentItems() As ClassRecentListBox.ClassRecentItem()
            Dim lRecentItems As New List(Of ClassRecentListBox.ClassRecentItem)

            For Each i As Object In g_mUCStartPage.RecentListBox_Files.Items
                If (TypeOf i Is ClassRecentListBox.ClassRecentItem) Then
                    lRecentItems.Add(DirectCast(i, ClassRecentListBox.ClassRecentItem))
                End If
            Next

            For Each i As Object In g_mUCStartPage.RecentListBox_Projects.Items
                If (TypeOf i Is ClassRecentListBox.ClassRecentItem) Then
                    lRecentItems.Add(DirectCast(i, ClassRecentListBox.ClassRecentItem))
                End If
            Next

            Return lRecentItems.ToArray
        End Function

        Public Function GetRecentFiles() As String()
            If (Not IO.File.Exists(m_RecentIni)) Then
                Return {}
            End If

            Dim lRecentFiles As New List(Of String)

            Using mStream = ClassFileStreamWait.Create(m_RecentIni, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT)

                    For Each iItem In mIni.ReadEverything
                        If (iItem.sSection <> RECENT_SECTION) Then
                            Continue For
                        End If

                        If (Not IO.File.Exists(iItem.sValue.ToLower)) Then
                            'Remove invalid entries from ini
                            lContent.Add(New ClassIni.STRUC_INI_CONTENT(RECENT_SECTION, iItem.sKey, Nothing))
                            Continue For
                        End If

                        If (lRecentFiles.Contains(iItem.sValue.ToLower)) Then
                            Continue For
                        End If

                        lRecentFiles.Add(iItem.sValue.ToLower)
                    Next

                    mIni.WriteKeyValue(lContent.ToArray)
                End Using
            End Using

            Return lRecentFiles.ToArray
        End Function

        Public Function SortFilesByDate(sFiles As String()) As String()
            Const C_RECENTFILES_FILE = "File"
            Const C_RECENTFILES_DATETICK = "DateTick"

            Dim lRecentFiles As New List(Of Dictionary(Of String, Object)) 'See keys: C_RECENTFILES_*

            For Each sFile As String In sFiles
                If (Not IO.File.Exists(sFile)) Then
                    Continue For
                End If

                Dim dDate As Date = IO.File.GetLastWriteTime(sFile)

                Dim mRecentFilesDic As New Dictionary(Of String, Object)
                mRecentFilesDic(C_RECENTFILES_FILE) = sFile.ToLower
                mRecentFilesDic(C_RECENTFILES_DATETICK) = dDate.Ticks

                lRecentFiles.Add(mRecentFilesDic)
            Next

            lRecentFiles.Sort(Function(x As Dictionary(Of String, Object), y As Dictionary(Of String, Object))
                                  Return -CLng(x(C_RECENTFILES_DATETICK)).CompareTo(CLng(y(C_RECENTFILES_DATETICK)))
                              End Function)

            Dim lRecentFilesSorted As New List(Of String)

            For Each mItem In lRecentFiles
                Dim sFile As String = CStr(mItem(C_RECENTFILES_FILE))

                lRecentFilesSorted.Add(sFile)
            Next

            Return lRecentFilesSorted.ToArray
        End Function

        Public Sub ClearRecentItems()
            g_mUCStartPage.RecentListBox_Files.Items.Clear()
            g_mUCStartPage.RecentListBox_Projects.Items.Clear()
        End Sub

        Public Sub RefreshRecentItems()
            Try
                g_mUCStartPage.RecentListBox_Files.BeginUpdate()
                g_mUCStartPage.RecentListBox_Projects.BeginUpdate()

                g_mUCStartPage.RecentListBox_Files.Items.Clear()
                g_mUCStartPage.RecentListBox_Projects.Items.Clear()

                Dim sRecentFilesSorted As String() = SortFilesByDate(GetRecentFiles())

                Const LAST_PROJECTS_TODAY As Integer = (1 << 0)
                Const LAST_PROJECTS_YESTERDAY As Integer = (1 << 1)
                Const LAST_PROJECTS_WEEK As Integer = (1 << 2)
                Const LAST_PROJECTS_MONTH As Integer = (1 << 3)
                Const LAST_PROJECTS_OTHER As Integer = (1 << 4)
                Const LAST_FILES_TODAY As Integer = (1 << 5)
                Const LIST_FILES_YESTERDAY As Integer = (1 << 6)
                Const LAST_FILES_WEEK As Integer = (1 << 7)
                Const LAST_FILES_MONTH As Integer = (1 << 8)
                Const LAST_FILES_OTHER As Integer = (1 << 9)


                Dim iLastFlags As Integer = 0

                For Each sFile As String In sRecentFilesSorted
                    Dim mDate As Date = IO.File.GetLastWriteTime(sFile)
                    Dim bProjectFile As Boolean = (IO.Path.GetExtension(sFile).ToLower = UCProjectBrowser.ClassProjectControl.g_sProjectExtension)

                    Select Case (True)
                        Case ((Now - New TimeSpan(1, 0, 0, 0)) < mDate)
                            If (bProjectFile) Then
                                If ((iLastFlags And LAST_PROJECTS_TODAY) = 0) Then
                                    iLastFlags = (iLastFlags Or LAST_PROJECTS_TODAY)
                                    g_mUCStartPage.RecentListBox_Projects.Items.Add(New ClassRecentListBox.ClassTitleItem("Today"))
                                End If
                            Else
                                If ((iLastFlags And LAST_FILES_TODAY) = 0) Then
                                    iLastFlags = (iLastFlags Or LAST_FILES_TODAY)
                                    g_mUCStartPage.RecentListBox_Files.Items.Add(New ClassRecentListBox.ClassTitleItem("Today"))
                                End If
                            End If

                        Case ((Now - New TimeSpan(2, 0, 0, 0)) < mDate)
                            If (bProjectFile) Then
                                If ((iLastFlags And LAST_PROJECTS_YESTERDAY) = 0) Then
                                    iLastFlags = (iLastFlags Or LAST_PROJECTS_YESTERDAY)
                                    g_mUCStartPage.RecentListBox_Projects.Items.Add(New ClassRecentListBox.ClassTitleItem("Yesterday"))
                                End If
                            Else
                                If ((iLastFlags And LIST_FILES_YESTERDAY) = 0) Then
                                    iLastFlags = (iLastFlags Or LIST_FILES_YESTERDAY)
                                    g_mUCStartPage.RecentListBox_Files.Items.Add(New ClassRecentListBox.ClassTitleItem("Yesterday"))
                                End If
                            End If

                        Case ((Now - New TimeSpan(7, 0, 0, 0)) < mDate)
                            If (bProjectFile) Then
                                If ((iLastFlags And LAST_PROJECTS_WEEK) = 0) Then
                                    iLastFlags = (iLastFlags Or LAST_PROJECTS_WEEK)
                                    g_mUCStartPage.RecentListBox_Projects.Items.Add(New ClassRecentListBox.ClassTitleItem("This Week"))
                                End If
                            Else
                                If ((iLastFlags And LAST_FILES_WEEK) = 0) Then
                                    iLastFlags = (iLastFlags Or LAST_FILES_WEEK)
                                    g_mUCStartPage.RecentListBox_Files.Items.Add(New ClassRecentListBox.ClassTitleItem("This Week"))
                                End If
                            End If

                        Case ((Now - New TimeSpan(31, 0, 0, 0)) < mDate)
                            If (bProjectFile) Then
                                If ((iLastFlags And LAST_PROJECTS_MONTH) = 0) Then
                                    iLastFlags = (iLastFlags Or LAST_PROJECTS_MONTH)
                                    g_mUCStartPage.RecentListBox_Projects.Items.Add(New ClassRecentListBox.ClassTitleItem("This Month"))
                                End If
                            Else
                                If ((iLastFlags And LAST_FILES_MONTH) = 0) Then
                                    iLastFlags = (iLastFlags Or LAST_FILES_MONTH)
                                    g_mUCStartPage.RecentListBox_Files.Items.Add(New ClassRecentListBox.ClassTitleItem("This Month"))
                                End If
                            End If

                        Case Else
                            If (bProjectFile) Then
                                If ((iLastFlags And LAST_PROJECTS_OTHER) = 0) Then
                                    iLastFlags = (iLastFlags Or LAST_PROJECTS_OTHER)
                                    g_mUCStartPage.RecentListBox_Projects.Items.Add(New ClassRecentListBox.ClassTitleItem("Last Time"))
                                End If
                            Else
                                If ((iLastFlags And LAST_FILES_OTHER) = 0) Then
                                    iLastFlags = (iLastFlags Or LAST_FILES_OTHER)
                                    g_mUCStartPage.RecentListBox_Files.Items.Add(New ClassRecentListBox.ClassTitleItem("Last Time"))
                                End If
                            End If
                    End Select

                    If (bProjectFile) Then
                        g_mUCStartPage.RecentListBox_Projects.Items.Add(New ClassRecentListBox.ClassRecentItem(sFile))
                    Else
                        g_mUCStartPage.RecentListBox_Files.Items.Add(New ClassRecentListBox.ClassRecentItem(sFile))
                    End If
                Next
            Finally
                g_mUCStartPage.RecentListBox_Files.EndUpdate()
                g_mUCStartPage.RecentListBox_Projects.EndUpdate()
            End Try
        End Sub
    End Class


    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor

        MyBase.OnPaint(e)
    End Sub

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor

        MyBase.OnPaintBackground(e)
    End Sub
End Class
