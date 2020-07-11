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



Public Class UCExportWizardFile
    Private g_mFormExportWizard As FormExportWizard

    Public Sub New(mFormExportWizard As FormExportWizard)
        g_mFormExportWizard = mFormExportWizard

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        AddHandler g_mFormExportWizard.OnNavigationPage, AddressOf OnNavigationPage

        UpdateControls()
    End Sub

    Property m_ExportFile As String
        Get
            Return g_mFormExportWizard.m_ExportFile
        End Get
        Set(value As String)
            g_mFormExportWizard.m_ExportFile = value

            UpdateControls()
        End Set
    End Property

    Private Sub Button_Browse_Click(sender As Object, e As EventArgs) Handles Button_Browse.Click
        Try
            Using i As New SaveFileDialog
                i.InitialDirectory = If(String.IsNullOrEmpty(m_ExportFile), "", IO.Path.GetDirectoryName(m_ExportFile))
                i.FileName = IO.Path.GetFileName(m_ExportFile)
                i.Filter = "Translation files|*.phrases.txt; *.txt|All files|*.*"

                If (i.ShowDialog = DialogResult.OK) Then
                    m_ExportFile = i.FileName
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub OnNavigationPage(iDirection As Integer, iPage As FormExportWizard.ENUM_PAGES, ByRef bHandeled As Boolean)
        'Make sure its this page
        If (iPage <> FormExportWizard.ENUM_PAGES.FILE) Then
            Return
        End If

        'Only if we go forward
        If (iDirection <> 1) Then
            Return
        End If

        If (String.IsNullOrEmpty(m_ExportFile)) Then
            MessageBox.Show("No file to export selected!", "No file selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            bHandeled = True
            Return
        End If
    End Sub

    Private Sub UCExportWizardFileSettings_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If (Me.Visible = False) Then
            Return
        End If

        g_mFormExportWizard.SetPageTitle("File to Export", "Specify the name of the file you want to export")
        UpdateControls()
    End Sub

    Private Sub UpdateControls()
        If (TextBox_File.Text <> m_ExportFile) Then
            TextBox_File.Text = m_ExportFile
        End If
    End Sub

    Private Sub CleanUp()
        If (g_mFormExportWizard IsNot Nothing) Then
            RemoveHandler g_mFormExportWizard.OnNavigationPage, AddressOf OnNavigationPage
        End If
    End Sub
End Class
