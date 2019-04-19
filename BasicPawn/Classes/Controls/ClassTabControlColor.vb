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


Imports System.Runtime.InteropServices

Public Class ClassTabControlColor
    Inherits ClassTabControlFix

    'TODO: Find better way to double buffer tabs without high CPU usage when tabs are drawn outside the window.
    'Protected Overrides Sub OnInvalidated(e As InvalidateEventArgs)
    '    ClassTools.ClassForms.SetDoubleBuffering(Me, True)
    '    ClassTools.ClassForms.SetDoubleBufferingUnmanaged(Me, True)

    '    MyBase.OnInvalidated(e)
    'End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        If (Me.DrawMode = TabDrawMode.Normal) Then
            MyBase.OnDrawItem(e)
            Return
        End If

        Const TAB_HEIGHT_OFFSET = 2

        Dim mTabFont As Font
        Dim mTabBack As Brush
        Dim mTabFore As Brush

        Dim mTabRec As Rectangle = Me.GetTabRect(e.Index)

        'Paint tab-bar
        If (e.Index = Me.TabPages.Count - 1) Then
            e.Graphics.FillRectangle(New SolidBrush(ClassControlStyle.g_cDarkFormColor.mDarkBackground),
                                     New RectangleF(mTabRec.X + mTabRec.Width, mTabRec.Y - TAB_HEIGHT_OFFSET, Me.Width - (mTabRec.X + mTabRec.Width) + 5, mTabRec.Height + TAB_HEIGHT_OFFSET))

        ElseIf (Me.Multiline) Then
            'Check if tab-warp
            If (e.Index + 1 <= Me.TabPages.Count - 1) Then
                Dim mTabNextRec As Rectangle = Me.GetTabRect(e.Index + 1)
                If (mTabRec.Y <> mTabNextRec.Y) Then
                    e.Graphics.FillRectangle(New SolidBrush(ClassControlStyle.g_cDarkFormColor.mDarkBackground),
                                             New RectangleF(mTabRec.X + mTabRec.Width, mTabRec.Y - TAB_HEIGHT_OFFSET, Me.Width - (mTabRec.X + mTabRec.Width) + 5, mTabRec.Height + TAB_HEIGHT_OFFSET))
                End If
            End If
        End If

        If (e.Index = Me.SelectedIndex) Then
            mTabFont = e.Font 'New Font(e.Font, FontStyle.Bold)
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
        mTabRec = New Rectangle(mTabRec.X + 2, mTabRec.Y + 2, mTabRec.Width - 4, mTabRec.Height - 4)
        e.Graphics.DrawString(Me.TabPages(e.Index).Text, mTabFont, mTabFore, mTabRec, New StringFormat())
    End Sub

    'Mimic padding to minimize weird edge color effects.
    Property m_TabPageAdjustEnabled As Boolean = True
    Property m_TabPageAdjustRectangle As New RECT(-3, -1, 3, 3)

    Private Const TCM_FIRST As Integer = &H1300
    Private Const TCM_ADJUSTRECT As UInteger = (TCM_FIRST + 40)

    Public Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer

        Sub New(_Left As Integer, _Top As Integer, _Right As Integer, _Bottom As Integer)
            Left = _Left
            Top = _Top
            Right = _Right
            Bottom = _Bottom
        End Sub
    End Structure

    Protected Overrides Sub WndProc(ByRef m As Message)
        If (m_TabPageAdjustEnabled) Then
            If (m.Msg = TCM_ADJUSTRECT) Then
                Dim mRect As RECT = DirectCast(m.GetLParam(GetType(RECT)), RECT)

                mRect.Left += m_TabPageAdjustRectangle.Left
                mRect.Right += m_TabPageAdjustRectangle.Right
                mRect.Top += m_TabPageAdjustRectangle.Top
                mRect.Bottom += m_TabPageAdjustRectangle.Bottom

                Marshal.StructureToPtr(mRect, m.LParam, True)
            End If
        End If

        MyBase.WndProc(m)
    End Sub
End Class
