Public Class UCStartPageRecentItem

    Private g_mUCStartPage As UCStartPage

    Private g_sRecentFile As String = ""
    Private g_dRecentDate As Date = Date.MinValue

    Public Sub New(f As UCStartPage, sFile As String)
        g_mUCStartPage = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        Try
            m_RecentFile = sFile
            m_RecentDate = IO.File.GetLastWriteTime(sFile)
        Catch ex As Exception
        End Try
    End Sub

    Property m_RecentFile As String
        Get
            Return g_sRecentFile
        End Get
        Set(value As String)
            g_sRecentFile = value

            ToolTip1.SetToolTip(Label_TitleName, value)
            ToolTip1.SetToolTip(Label_TitlePath, value)

            UpdateInfoText()
        End Set
    End Property

    Property m_RecentDate As Date
        Get
            Return g_dRecentDate
        End Get
        Set(value As Date)
            g_dRecentDate = value

            UpdateInfoText()
        End Set
    End Property

    Property m_Open As Boolean
        Get
            Return CheckBox_Open.Checked
        End Get
        Set(value As Boolean)
            CheckBox_Open.Checked = value
        End Set
    End Property

    Private Sub UpdateInfoText()
        Label_TitleName.Text = String.Format("{0} - {1}", m_RecentDate.ToString, IO.Path.GetFileName(g_sRecentFile))
        Label_TitlePath.Text = IO.Path.GetFullPath(g_sRecentFile)
    End Sub

    Private Sub ClassButtonSmallDelete_RemoveFromRecent_Click(sender As Object, e As EventArgs) Handles ClassButtonSmallDelete_RemoveFromRecent.Click
        Try
            g_mUCStartPage.g_mClassRecentItems.RemoveRecent(m_RecentFile)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try

        Me.Dispose()
    End Sub

    Private Sub Label_TitleName_Click(sender As Object, e As EventArgs) Handles Label_TitleName.Click
        OpenRecent()
    End Sub

    Private Sub Label_TitlePath_Click(sender As Object, e As EventArgs) Handles Label_TitlePath.Click
        OpenRecent()
    End Sub

    Private Sub OpenRecent()
        Try
            g_mUCStartPage.g_mFormMain.g_ClassTabControl.AddTab(True)
            g_mUCStartPage.g_mFormMain.g_ClassTabControl.OpenFileTab(g_mUCStartPage.g_mFormMain.g_ClassTabControl.m_TabsCount - 1, m_RecentFile)

            'Close new unsaved and unchanged tabs
            While g_mUCStartPage.g_mFormMain.g_ClassTabControl.m_TabsCount > 0
                If (String.IsNullOrEmpty(g_mUCStartPage.g_mFormMain.g_ClassTabControl.m_Tab(0).m_File) AndAlso Not g_mUCStartPage.g_mFormMain.g_ClassTabControl.m_Tab(0).m_Changed) Then
                    Dim bLast As Boolean = (g_mUCStartPage.g_mFormMain.g_ClassTabControl.m_TabsCount = 1)

                    If (Not g_mUCStartPage.g_mFormMain.g_ClassTabControl.RemoveTab(0, True)) Then
                        Exit While
                    End If

                    If (bLast) Then
                        Exit While
                    End If
                Else
                    Exit While
                End If
            End While

        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
