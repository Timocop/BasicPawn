'BasicPawn
'Copyright(C) 2017 TheTimocop

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


Public Class UCStartPageRecentItem
    Public g_mUCStartPage As UCStartPage

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

    ReadOnly Property m_IsProjectFile As Boolean
        Get
            Return (IO.Path.GetExtension(g_sRecentFile).ToLower = UCProjectBrowser.ClassProjectControl.g_sProjectExtension)
        End Get
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

    Public Sub OpenRecent()
        Try
            If (m_IsProjectFile) Then
                g_mUCStartPage.g_mFormMain.g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile = m_RecentFile
                g_mUCStartPage.g_mFormMain.g_mUCProjectBrowser.g_ClassProjectControl.LoadProject(False)
            Else
                Dim mTab = g_mUCStartPage.g_mFormMain.g_ClassTabControl.AddTab()
                mTab.OpenFileTab(m_RecentFile)
                mTab.SelectTab(500)
            End If

            g_mUCStartPage.g_mFormMain.g_ClassTabControl.RemoveUnsavedTabsLeft()

            g_mUCStartPage.Hide()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
