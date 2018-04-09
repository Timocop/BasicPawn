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


Public Class ClassButtonSmallDelete
    Inherits PictureBox

    Public Sub New()
        Me.Image = My.Resources.Bmp_ButtonDeleteDefault
        Me.Size = New Size(16, 16)
        Me.MinimumSize = New Size(16, 16)
        Me.MaximumSize = New Size(16, 16)

        Me.BackColor = Color.Transparent
    End Sub

    Protected Overrides Sub OnMouseDown(mevent As MouseEventArgs)
        Me.Image = My.Resources.Bmp_ButtonDeletePressed
        MyBase.OnMouseDown(mevent)
    End Sub

    Protected Overrides Sub OnMouseUp(mevent As MouseEventArgs)
        Me.Image = My.Resources.Bmp_ButtonDeleteHover
        MyBase.OnMouseUp(mevent)
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        Me.Image = My.Resources.Bmp_ButtonDeleteHover
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        Me.Image = My.Resources.Bmp_ButtonDeleteDefault
        MyBase.OnMouseLeave(e)
    End Sub

End Class
