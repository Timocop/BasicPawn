'BasicPawn
'Copyright(C) 2021 Externet

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
        Dim iSize As Integer = ClassTools.ClassForms.ScaleDPI(16)

        Me.Image = My.Resources.Bmp_ButtonDeleteDefault
        Me.Size = New Size(iSize, iSize)
        Me.MinimumSize = New Size(iSize, iSize)
        Me.MaximumSize = New Size(iSize, iSize)

        Me.BackColor = Color.Transparent
        Me.SizeMode = PictureBoxSizeMode.Zoom
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
