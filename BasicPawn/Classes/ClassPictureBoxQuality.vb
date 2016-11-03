Public Class ClassPictureBoxQuality
    Inherits PictureBox

    Private g_iHighQuality As Boolean = False

    Public Property HighQuality() As Boolean
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
