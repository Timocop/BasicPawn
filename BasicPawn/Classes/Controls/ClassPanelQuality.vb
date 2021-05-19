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


Public Class ClassPanelQuality
    Inherits Panel

    Private g_iHighQuality As Boolean = False

    Public Property m_HighQuality() As Boolean
        Get
            Return g_iHighQuality
        End Get
        Set(value As Boolean)
            g_iHighQuality = value
        End Set
    End Property

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If (g_iHighQuality) Then
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        Else
            'Save UI thread CPU ressources
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
            e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        End If

        MyBase.OnPaint(e)
    End Sub

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        If (g_iHighQuality) Then
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        Else
            'Save UI thread CPU ressources
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed
            e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighSpeed
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        End If

        MyBase.OnPaintBackground(e)
    End Sub
End Class
