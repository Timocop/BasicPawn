Public Class UCStartPage
    Public g_mFormMain As FormMain
    Public g_mClassRecentItems As New ClassRecentItems(Me)


    Public Sub New(mFormMain As FormMain)
        g_mFormMain = mFormMain

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

        Label_NoProjectsFound.Visible = False
        Timer_DelayUpdate.Start()
    End Sub

    Private Sub Timer_DelayUpdate_Tick(sender As Object, e As EventArgs) Handles Timer_DelayUpdate.Tick
        Try
            Timer_DelayUpdate.Stop()

            g_mClassRecentItems.RefreshRecentItems()
            Label_NoProjectsFound.Visible = (g_mClassRecentItems.GetRecentFiles.Length < 1)

            ClassControlStyle.UpdateControls(Me)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub LinkLabel_Close_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_Close.LinkClicked
        Me.Hide()
    End Sub

    Private Sub LinkLabel_OpenNew_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_OpenNew.LinkClicked
        Dim mRecentItems = g_mClassRecentItems.GetRecentItems
        Dim bSomethingSelected As Boolean = False

        'Close all tabs
        While g_mFormMain.g_ClassTabControl.m_TabsCount > 0
            Dim bLast As Boolean = (g_mFormMain.g_ClassTabControl.m_TabsCount = 1)

            If (Not g_mFormMain.g_ClassTabControl.RemoveTab(0, True)) Then
                Return
            End If

            If (bLast) Then
                Exit While
            End If
        End While

        For i = mRecentItems.Length - 1 To 0 Step -1
            Try
                If (Not mRecentItems(i).m_Open) Then
                    Continue For
                End If

                bSomethingSelected = True

                g_mFormMain.g_ClassTabControl.AddTab(True)
                g_mFormMain.g_ClassTabControl.OpenFileTab(g_mFormMain.g_ClassTabControl.m_TabsCount - 1, mRecentItems(i).m_RecentFile)

            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        Next

        If (bSomethingSelected) Then
            'Close new unsaved and unchanged tabs
            While g_mFormMain.g_ClassTabControl.m_TabsCount > 0
                If (String.IsNullOrEmpty(g_mFormMain.g_ClassTabControl.m_Tab(0).m_File) AndAlso Not g_mFormMain.g_ClassTabControl.m_Tab(0).m_Changed) Then
                    Dim bLast As Boolean = (g_mFormMain.g_ClassTabControl.m_TabsCount = 1)

                    If (Not g_mFormMain.g_ClassTabControl.RemoveTab(0, True)) Then
                        Exit While
                    End If

                    If (bLast) Then
                        Exit While
                    End If
                Else
                    Exit While
                End If
            End While
        End If

        If (Not bSomethingSelected) Then
            MessageBox.Show("No file selected to open!", "Could not open file", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub LinkLabel_Open_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_Open.LinkClicked
        Dim mRecentItems = g_mClassRecentItems.GetRecentItems
        Dim bSomethingSelected As Boolean = False

        For i = mRecentItems.Length - 1 To 0 Step -1
            Try
                If (Not mRecentItems(i).m_Open) Then
                    Continue For
                End If

                bSomethingSelected = True

                g_mFormMain.g_ClassTabControl.AddTab(True)
                g_mFormMain.g_ClassTabControl.OpenFileTab(g_mFormMain.g_ClassTabControl.m_TabsCount - 1, mRecentItems(i).m_RecentFile)

            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        Next

        If (bSomethingSelected) Then
            'Close new unsaved and unchanged tabs
            While g_mFormMain.g_ClassTabControl.m_TabsCount > 0
                If (String.IsNullOrEmpty(g_mFormMain.g_ClassTabControl.m_Tab(0).m_File) AndAlso Not g_mFormMain.g_ClassTabControl.m_Tab(0).m_Changed) Then
                    Dim bLast As Boolean = (g_mFormMain.g_ClassTabControl.m_TabsCount = 1)

                    If (Not g_mFormMain.g_ClassTabControl.RemoveTab(0, True)) Then
                        Exit While
                    End If

                    If (bLast) Then
                        Exit While
                    End If
                Else
                    Exit While
                End If
            End While
        End If

        If (Not bSomethingSelected) Then
            MessageBox.Show("No file selected to open!", "Could not open file", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
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

            Dim mIni As New ClassIniFile(m_RecentIni)

            For Each iItem In mIni.ReadEverything
                If (iItem.sSection <> RECENT_SECTION) Then
                    Continue For
                End If

                If (iItem.sValue.ToLower <> sFile.ToLower) Then
                    Continue For
                End If

                mIni.WriteKeyValue(iItem.sSection, iItem.sKey)
            Next
        End Sub

        Public Sub AddRecent(sFile As String)
            Dim mIni As New ClassIniFile(m_RecentIni)

            If (IO.File.Exists(m_RecentIni)) Then
                For Each iItem In mIni.ReadEverything
                    If (iItem.sSection <> RECENT_SECTION) Then
                        Continue For
                    End If

                    If (iItem.sValue.ToLower <> sFile.ToLower) Then
                        Continue For
                    End If

                    mIni.WriteKeyValue(iItem.sSection, iItem.sKey)
                Next
            End If

            mIni.WriteKeyValue(RECENT_SECTION, Guid.NewGuid.ToString, sFile)
        End Sub

        Public Function GetRecentItems() As UCStartPageRecentItem()
            Dim lRecentItems As New List(Of UCStartPageRecentItem)

            For Each c In g_mUCStartPage.Panel_RecentFiles.Controls
                If (TypeOf c Is UCStartPageRecentItem) Then
                    lRecentItems.Add(DirectCast(c, UCStartPageRecentItem))
                End If
            Next

            Return lRecentItems.ToArray
        End Function

        Public Function GetRecentFiles() As String()
            If (Not IO.File.Exists(m_RecentIni)) Then
                Return {}
            End If

            Dim lRecentFiles As New List(Of String)

            Dim mIni As New ClassIniFile(m_RecentIni)
            For Each iItem In mIni.ReadEverything
                If (iItem.sSection <> RECENT_SECTION) Then
                    Continue For
                End If

                If (Not IO.File.Exists(iItem.sValue.ToLower)) Then
                    'Remove invalid entries from ini
                    mIni.WriteKeyValue(RECENT_SECTION, iItem.sKey, Nothing)
                    Continue For
                End If

                If (lRecentFiles.Contains(iItem.sValue.ToLower)) Then
                    Continue For
                End If

                lRecentFiles.Add(iItem.sValue.ToLower)
            Next

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
            g_mUCStartPage.Panel_RecentFiles.SuspendLayout()

            Dim mRecentItems = GetRecentItems()
            For i = mRecentItems.Length - 1 To 0 Step -1
                mRecentItems(i).Dispose()
            Next

            g_mUCStartPage.Panel_RecentFiles.ResumeLayout()
        End Sub

        Public Sub RefreshRecentItems()
            ClearRecentItems()

            g_mUCStartPage.Panel_RecentFiles.SuspendLayout()

            Dim sRecentFiles As String() = GetRecentFiles()
            sRecentFiles = SortFilesByDate(sRecentFiles)

            For Each sFile As String In sRecentFiles
                With New UCStartPageRecentItem(g_mUCStartPage, sFile)
                    .Parent = g_mUCStartPage.Panel_RecentFiles
                    .Dock = DockStyle.Top
                    .BringToFront()
                    .Show()
                End With
            Next

            g_mUCStartPage.Panel_RecentFiles.ResumeLayout()
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
