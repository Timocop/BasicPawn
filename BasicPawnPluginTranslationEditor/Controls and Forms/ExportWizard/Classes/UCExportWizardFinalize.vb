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



Public Class UCExportWizardFinalize
    Private g_mFormExportWizard As FormExportWizard

    Public Sub New(mFormExportWizard As FormExportWizard)
        g_mFormExportWizard = mFormExportWizard
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        UpdateControls()
    End Sub

    Private Sub UCExportWizardFinalize_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If (Not Me.Visible) Then
            Return
        End If

        g_mFormExportWizard.SetPageTitle("Completing the Translation Export Wizard", "")
        UpdateControls()
    End Sub

    Private Sub UpdateControls()
        Dim mConfigs As New List(Of KeyValuePair(Of String, String)) From {
            New KeyValuePair(Of String, String)("File to Export:", g_mFormExportWizard.m_ExportFile),
            New KeyValuePair(Of String, String)("Export Method:", If(g_mFormExportWizard.m_ExportIsPacked, "Export phrases to single translation file.", "Export phrases to multiple translation files."))
        }

        Try
            For Each sFile In g_mFormExportWizard.m_ExportFilesAdditional
                mConfigs.Add(New KeyValuePair(Of String, String)("Additional File:", sFile))
            Next

            For Each sFile In g_mFormExportWizard.m_ExportFilesRemoval
                mConfigs.Add(New KeyValuePair(Of String, String)("Remove File:", sFile))
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try

        Try
            ListBox_Config.BeginUpdate()
            ListBox_Config.Items.Clear()

            For Each mItem In mConfigs
                ListBox_Config.Items.Add(String.Format("{0,-32}{1}", mItem.Key, mItem.Value))
            Next
        Finally
            ListBox_Config.EndUpdate()
        End Try
    End Sub
End Class
