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


Imports BasicPawn

Public Class UCReportExceptionItem
    Inherits UCReportItem

    Public g_mException As ClassDebuggerParser.STRUC_SM_EXCEPTION

    Private g_mFormReportDetails As FormReportDetails

    Public Sub New(mFormReportManager As FormReportManager, mException As ClassDebuggerParser.STRUC_SM_EXCEPTION)
        MyBase.New(mFormReportManager)

        g_mException = mException

        Label_Title.Text = mException.sExceptionInfo
        Label_File.Text = mException.sBlamingFile

        If (mException.dLogDate.ToShortDateString = Now.ToShortDateString) Then
            Label_Date.Text = mException.dLogDate.ToShortTimeString
        Else
            Label_Date.Text = mException.dLogDate.ToLongDateString
        End If

        PictureBox1.Image = My.Resources.ieframe_36883_16x16_32
    End Sub

    Private Sub FormButton_Click(sender As Object, e As EventArgs) Handles Me.OnButtonClick
        If (g_mFormReportDetails IsNot Nothing AndAlso Not g_mFormReportDetails.IsDisposed) Then
            g_mFormReportDetails.Activate()
        Else
            g_mFormReportDetails = New FormReportDetails(g_mFormReportManager, g_mException)
            g_mFormReportDetails.Show(Me)
        End If
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If (disposing) Then
            If (g_mFormReportDetails IsNot Nothing AndAlso Not g_mFormReportDetails.IsDisposed) Then
                g_mFormReportDetails.Dispose()
                g_mFormReportDetails = Nothing
            End If
        End If

        MyBase.Dispose(disposing)
    End Sub
End Class
