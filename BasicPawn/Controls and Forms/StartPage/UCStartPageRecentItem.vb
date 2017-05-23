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

        ClassControlStyle.UpdateControls(Me)
    End Sub

    Property m_RecentFile As String
        Get
            Return g_sRecentFile
        End Get
        Set(value As String)
            g_sRecentFile = value

            ToolTip1.SetToolTip(Label_DateAndFile, value)

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
        With New Text.StringBuilder
            '.AppendLine(m_RecentDate.ToString)
            .AppendLine(String.Format("{0} - {1}", m_RecentDate.ToString, IO.Path.GetFileName(g_sRecentFile)))
            .AppendLine(IO.Path.GetFullPath(g_sRecentFile))

            Label_DateAndFile.Text = .ToString
        End With
    End Sub

    Private Sub ClassButtonSmallDelete_RemoveFromRecent_Click(sender As Object, e As EventArgs) Handles ClassButtonSmallDelete_RemoveFromRecent.Click
        Try
            g_mUCStartPage.g_mClassRecentItems.RemoveRecent(m_RecentFile)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try

        Me.Dispose()
    End Sub

    Private Sub Label_DateAndFile_Click(sender As Object, e As EventArgs) Handles Label_DateAndFile.Click
        Try
            g_mUCStartPage.g_mFormMain.g_ClassTabControl.AddTab(True)
            g_mUCStartPage.g_mFormMain.g_ClassTabControl.OpenFileTab(g_mUCStartPage.g_mFormMain.g_ClassTabControl.m_TabsCount - 1, m_RecentFile)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
