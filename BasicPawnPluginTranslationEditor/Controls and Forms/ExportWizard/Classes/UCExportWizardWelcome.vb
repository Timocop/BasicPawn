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


Public Class UCExportWizardWelcome
    Private g_mFormExportWizard As FormExportWizard

    Public Sub New(mFormExportWizard As FormExportWizard)
        g_mFormExportWizard = mFormExportWizard

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub UCExportWizardWelcome_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If (Me.Visible = False) Then
            Return
        End If

        g_mFormExportWizard.SetPageTitle("Welcome to the Translation Export Wizard", "")
    End Sub
End Class
