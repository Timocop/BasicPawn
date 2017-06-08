Public Class ClassTabControlColor
    Inherits TabControl

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        If (Me.DrawMode = TabDrawMode.Normal) Then
            MyBase.OnDrawItem(e)
            Return
        End If

        Dim mTabFont As Font
        Dim mTabBack As Brush
        Dim mTabFore As Brush

        Dim mTabRec As Rectangle

        If e.Index = Me.SelectedIndex Then
            mTabFont = New Font(e.Font, FontStyle.Bold)
            mTabBack = New Drawing2D.LinearGradientBrush(e.Bounds, ClassControlStyle.g_cDarkControlColor.mDarkBackground, ClassControlStyle.g_cDarkControlColor.mDarkBackground, Drawing2D.LinearGradientMode.BackwardDiagonal)
            mTabFore = Brushes.White

            mTabRec = e.Bounds
            e.Graphics.FillRectangle(mTabBack, mTabRec)
        Else
            mTabFont = e.Font
            mTabBack = New Drawing2D.LinearGradientBrush(e.Bounds, ClassControlStyle.g_cDarkFormColor.mDarkBackground, ClassControlStyle.g_cDarkFormColor.mDarkBackground, Drawing2D.LinearGradientMode.BackwardDiagonal)
            mTabFore = Brushes.White

            mTabRec = e.Bounds
            mTabRec = New Rectangle(mTabRec.X - 2, mTabRec.Y - 2, mTabRec.Width + 4, mTabRec.Height + 4)
            e.Graphics.FillRectangle(mTabBack, mTabRec)
        End If

        mTabRec = e.Bounds
        mTabRec = New Rectangle(mTabRec.X + 2, mTabRec.Y + 2, mTabRec.Width - 2, mTabRec.Height - 2)
        e.Graphics.DrawString(Me.TabPages(e.Index).Text, mTabFont, mTabFore, mTabRec, New StringFormat())

        Dim r As Rectangle = Me.GetTabRect(Me.TabPages.Count - 1)
        e.Graphics.FillRectangle(New SolidBrush(ClassControlStyle.g_cDarkFormColor.mDarkBackground), New RectangleF(r.X + r.Width, r.Y - 5, Me.Width - (r.X + r.Width) + 5, r.Height + 5))
    End Sub
End Class
