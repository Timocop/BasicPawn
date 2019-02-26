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


Partial Public Class FormMain
    Private Sub ToolStripMenuItem_ViewToolbox_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ViewToolbox.CheckedChanged
        If (g_bIgnoreCheckedChangedEvent) Then
            Return
        End If

        UpdateViews()
        SaveViews()
    End Sub

    Private Sub ToolStripMenuItem_ViewDetails_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ViewDetails.CheckedChanged
        If (g_bIgnoreCheckedChangedEvent) Then
            Return
        End If

        UpdateViews()
        SaveViews()
    End Sub

    Private Sub ToolStripMenuItem_ViewMinimap_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ViewMinimap.CheckedChanged
        If (g_bIgnoreCheckedChangedEvent) Then
            Return
        End If

        UpdateViews()
        SaveViews()
    End Sub

    Private Sub ToolStripMenuItem_ViewProgressAni_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ViewProgressAni.CheckedChanged
        If (g_bIgnoreCheckedChangedEvent) Then
            Return
        End If

        UpdateViews()
        SaveViews()
    End Sub
End Class
