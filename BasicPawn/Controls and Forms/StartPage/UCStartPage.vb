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


Public Class UCStartPage
    Public g_mFormMain As FormMain
    Public g_mClassRecentItems As New ClassRecentItems(Me)


    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        TableLayoutPanel1.Name &= "@KeepForeBackColor"
        Panel_FooterDarkControl.Name &= "@FooterDarkControl"
        Panel_FooterDarkControl2.Name &= "@FooterDarkControl"
        Panel_FooterDarkControl3.Name &= "@FooterDarkControl"

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

                    .Parent = TabPage_RecentProjects
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

                    .Parent = TabPage_RecentFiles
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

        g_mFormMain.PrintInformation("[INFO]", "User created a new source file")

        Me.Hide()
    End Sub

    Private Sub LinkLabel_NewTemplate_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_NewTemplate.LinkClicked
        g_mFormMain.g_ClassTabControl.AddTab(True, True, False, False)

        g_mFormMain.PrintInformation("[INFO]", "User created a new source file")

        Me.Hide()
    End Sub

    Private Sub LinkLabel_OpenNew_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_OpenNew.LinkClicked
        Try
            Dim mRecentItems = g_mClassRecentItems.GetRecentItems
            Dim bSomethingSelected As Boolean = False
            Dim bAppendFiles As Boolean = False

            For i = mRecentItems.Length - 1 To 0 Step -1
                If (Not mRecentItems(i).m_Open) Then
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
                        If (Not mRecentItems(i).m_Open) Then
                            Continue For
                        End If

                        If (mRecentItems(i).m_IsProjectFile) Then
                            g_mFormMain.g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile = mRecentItems(i).m_RecentFile
                            g_mFormMain.g_mUCProjectBrowser.g_ClassProjectControl.LoadProject(bAppendFiles, ClassSettings.g_iSettingsAutoOpenProjectFiles)
                            bAppendFiles = True
                        Else
                            Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
                            mTab.OpenFileTab(mRecentItems(i).m_RecentFile)
                            mTab.SelectTab(500)
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
            If (Not mRecentItems(i).m_Open) Then
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
                    If (Not mRecentItems(i).m_Open) Then
                        Continue For
                    End If

                    If (mRecentItems(i).m_IsProjectFile) Then
                        g_mFormMain.g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile = mRecentItems(i).m_RecentFile
                        g_mFormMain.g_mUCProjectBrowser.g_ClassProjectControl.LoadProject(bAppendFiles, ClassSettings.g_iSettingsAutoOpenProjectFiles)
                        bAppendFiles = True
                    Else
                        Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(mRecentItems(i).m_RecentFile)
                        mTab.SelectTab(500)
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

        Public Function GetRecentItems() As UCStartPageRecentItem()
            Dim lRecentItems As New List(Of UCStartPageRecentItem)

            For Each c As Control In g_mUCStartPage.TabPage_RecentFiles.Controls
                If (TypeOf c Is UCStartPageRecentItem) Then
                    lRecentItems.Add(DirectCast(c, UCStartPageRecentItem))
                End If
            Next

            For Each c As Control In g_mUCStartPage.TabPage_RecentProjects.Controls
                If (TypeOf c Is UCStartPageRecentItem) Then
                    lRecentItems.Add(DirectCast(c, UCStartPageRecentItem))
                End If
            Next

            Return lRecentItems.ToArray
        End Function

        Public Function GetAllItems() As Control()
            Dim lItems As New List(Of Control)

            For Each c As Control In g_mUCStartPage.TabPage_RecentFiles.Controls
                lItems.Add(c)
            Next

            For Each c As Control In g_mUCStartPage.TabPage_RecentProjects.Controls
                lItems.Add(c)
            Next

            Return lItems.ToArray
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
            Dim lRecentFiles As New List(Of Object()) ' {sFile, iDateTick}

            For Each sFile As String In sFiles
                If (Not IO.File.Exists(sFile)) Then
                    Continue For
                End If

                Dim dDate As Date = IO.File.GetLastWriteTime(sFile)

                lRecentFiles.Add(New Object() {sFile.ToLower, dDate.Ticks})
            Next

            lRecentFiles.Sort(Function(x As Object(), y As Object())
                                  Return -CLng(x(1)).CompareTo(CLng(y(1)))
                              End Function)

            Dim lRecentFilesSorted As New List(Of String)
            For Each iItem In lRecentFiles
                lRecentFilesSorted.Add(CStr(iItem(0)))
            Next

            Return lRecentFilesSorted.ToArray
        End Function

        Public Sub ClearRecentItems()
            g_mUCStartPage.TabPage_RecentFiles.SuspendLayout()
            g_mUCStartPage.TabPage_RecentProjects.SuspendLayout()

            Dim mRecentItems = GetAllItems()
            For i = mRecentItems.Length - 1 To 0 Step -1
                mRecentItems(i).Dispose()
            Next

            g_mUCStartPage.TabPage_RecentFiles.ResumeLayout()
            g_mUCStartPage.TabPage_RecentProjects.ResumeLayout()
        End Sub

        Public Sub RefreshRecentItems()
            g_mUCStartPage.TabPage_RecentFiles.SuspendLayout()
            g_mUCStartPage.TabPage_RecentProjects.SuspendLayout()

            ClearRecentItems()

            Dim sRecentFilesSorted As String() = SortFilesByDate(GetRecentFiles())

            Dim iLabelFlags_ProjectsToday As Integer = (1 << 0)
            Dim iLabelFlags_FilesToday As Integer = (1 << 1)
            Dim iLabelFlags_ProjectsYesterday As Integer = (1 << 2)
            Dim iLabelFlags_FilesYesterday As Integer = (1 << 3)
            Dim iLabelFlags_ProjectsWeek As Integer = (1 << 4)
            Dim iLabelFlags_FilesWeek As Integer = (1 << 5)
            Dim iLabelFlags_ProjectsMonth As Integer = (1 << 6)
            Dim iLabelFlags_FilesMonth As Integer = (1 << 7)
            Dim iLabelFlags_ProjectsOther As Integer = (1 << 8)
            Dim iLabelFlags_FilesOther As Integer = (1 << 9)


            Dim iLabelFlags As Integer = 0

            For Each sFile As String In sRecentFilesSorted
                Dim mDate As Date = IO.File.GetLastWriteTime(sFile)
                Dim bProjectFile As Boolean = (IO.Path.GetExtension(sFile).ToLower = UCProjectBrowser.ClassProjectControl.g_sProjectExtension)

                Select Case (True)
                    Case ((Now - New TimeSpan(1, 0, 0, 0)) < mDate)
                        If (bProjectFile) Then
                            If (Not CBool(iLabelFlags And iLabelFlags_ProjectsToday)) Then
                                iLabelFlags = (iLabelFlags Or iLabelFlags_ProjectsToday)
                                CreateLastModifiedLabel("Today", g_mUCStartPage.TabPage_RecentProjects)
                            End If
                        Else
                            If (Not CBool(iLabelFlags And iLabelFlags_FilesToday)) Then
                                iLabelFlags = (iLabelFlags Or iLabelFlags_FilesToday)
                                CreateLastModifiedLabel("Today", g_mUCStartPage.TabPage_RecentFiles)
                            End If
                        End If

                    Case ((Now - New TimeSpan(2, 0, 0, 0)) < mDate)
                        If (bProjectFile) Then
                            If (Not CBool(iLabelFlags And iLabelFlags_ProjectsYesterday)) Then
                                iLabelFlags = (iLabelFlags Or iLabelFlags_ProjectsYesterday)
                                CreateLastModifiedLabel("Yesterday", g_mUCStartPage.TabPage_RecentProjects)
                            End If
                        Else
                            If (Not CBool(iLabelFlags And iLabelFlags_FilesYesterday)) Then
                                iLabelFlags = (iLabelFlags Or iLabelFlags_FilesYesterday)
                                CreateLastModifiedLabel("Yesterday", g_mUCStartPage.TabPage_RecentFiles)
                            End If
                        End If

                    Case ((Now - New TimeSpan(7, 0, 0, 0)) < mDate)
                        If (bProjectFile) Then
                            If (Not CBool(iLabelFlags And iLabelFlags_ProjectsWeek)) Then
                                iLabelFlags = (iLabelFlags Or iLabelFlags_ProjectsWeek)
                                CreateLastModifiedLabel("This Week", g_mUCStartPage.TabPage_RecentProjects)
                            End If
                        Else
                            If (Not CBool(iLabelFlags And iLabelFlags_FilesWeek)) Then
                                iLabelFlags = (iLabelFlags Or iLabelFlags_FilesWeek)
                                CreateLastModifiedLabel("This Week", g_mUCStartPage.TabPage_RecentFiles)
                            End If
                        End If

                    Case ((Now - New TimeSpan(31, 0, 0, 0)) < mDate)
                        If (bProjectFile) Then
                            If (Not CBool(iLabelFlags And iLabelFlags_ProjectsMonth)) Then
                                iLabelFlags = (iLabelFlags Or iLabelFlags_ProjectsMonth)
                                CreateLastModifiedLabel("This Month", g_mUCStartPage.TabPage_RecentProjects)
                            End If
                        Else
                            If (Not CBool(iLabelFlags And iLabelFlags_FilesMonth)) Then
                                iLabelFlags = (iLabelFlags Or iLabelFlags_FilesMonth)
                                CreateLastModifiedLabel("This Month", g_mUCStartPage.TabPage_RecentFiles)
                            End If
                        End If

                    Case Else
                        If (bProjectFile) Then
                            If (Not CBool(iLabelFlags And iLabelFlags_ProjectsOther)) Then
                                iLabelFlags = (iLabelFlags Or iLabelFlags_ProjectsOther)
                                CreateLastModifiedLabel("Last Time", g_mUCStartPage.TabPage_RecentProjects)
                            End If
                        Else
                            If (Not CBool(iLabelFlags And iLabelFlags_FilesOther)) Then
                                iLabelFlags = (iLabelFlags Or iLabelFlags_FilesOther)
                                CreateLastModifiedLabel("Last Time", g_mUCStartPage.TabPage_RecentFiles)
                            End If
                        End If
                End Select

                With New UCStartPageRecentItem(g_mUCStartPage, sFile)
                    .SuspendLayout()

                    If (bProjectFile) Then
                        .Parent = g_mUCStartPage.TabPage_RecentProjects
                    Else
                        .Parent = g_mUCStartPage.TabPage_RecentFiles
                    End If

                    .Dock = DockStyle.Top
                    .BringToFront()
                    .Show()

                    .ResumeLayout()
                End With
            Next

            g_mUCStartPage.TabPage_RecentFiles.ResumeLayout()
            g_mUCStartPage.TabPage_RecentProjects.ResumeLayout()
        End Sub

        Private Sub CreateLastModifiedLabel(sText As String, mParent As Control)
            With New Label
                .SuspendLayout()

                .Text = sText
                .Font = New Font(.Font.FontFamily, 12, FontStyle.Bold)
                .Name &= "@SetForeColorRoyalBlue"
                .AutoSize = True

                .Parent = mParent
                .Dock = DockStyle.Top
                .BringToFront()
                .Show()

                .ResumeLayout()
            End With
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
