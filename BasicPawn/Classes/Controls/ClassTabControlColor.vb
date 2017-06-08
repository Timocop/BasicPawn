'BasicPawn
'Copyright(C) 2017 TheTimocop

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


Public Class ClassTabControlColor
    Inherits TabControl

    Protected Overrides Sub OnInvalidated(e As InvalidateEventArgs)
        MyBase.OnInvalidated(e)

        ClassTools.ClassForms.SetDoubleBuffering(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanaged(Me, True)
    End Sub

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
