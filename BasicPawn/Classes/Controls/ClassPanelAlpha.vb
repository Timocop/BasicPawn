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


Public Class ClassPanelAlpha
    Inherits Panel

    Private g_mOpacity As Integer = 50
    Public g_mTransparentBackColor As Color

    Public Sub New()
        g_mTransparentBackColor = Color.Red
        m_Opacity = 50
        Me.BackColor = Color.Transparent

        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If Me.Parent IsNot Nothing AndAlso m_Opacity > 0 Then
            Dim lControls As New SortedList(Of Integer, Control)

            Using mBitmap = New Bitmap(Me.Parent.Width, Me.Parent.Height)
                For Each c As Control In Me.Parent.Controls
                    If (Me.Parent.Controls.GetChildIndex(c) > Me.Parent.Controls.GetChildIndex(Me) AndAlso c.Bounds.IntersectsWith(Me.Bounds)) Then
                        lControls.Add(-Me.Parent.Controls.GetChildIndex(c), c)
                    End If
                Next

                For Each i In lControls
                    If (Not i.Value.Visible) Then
                        Continue For
                    End If

                    i.Value.DrawToBitmap(mBitmap, i.Value.Bounds)
                Next

                e.Graphics.DrawImage(mBitmap, -Left, -Top)
                Using b = New SolidBrush(Color.FromArgb(Me.g_mOpacity, Me.g_mTransparentBackColor))
                    e.Graphics.FillRectangle(b, Me.ClientRectangle)
                End Using
            End Using
        End If
    End Sub

    Public Property m_Opacity As Integer
        Get
            Return g_mOpacity
        End Get
        Set(value As Integer)
            value = Math.Max(value, 0)
            value = Math.Min(value, 100)

            g_mOpacity = value
            Me.Invalidate()
        End Set
    End Property

    Public Property m_TransparentBackColor As Color
        Get
            Return g_mTransparentBackColor
        End Get
        Set
            g_mTransparentBackColor = Value
            Me.Invalidate()
        End Set
    End Property

    Public Overrides Property BackColor As Color
        Get
            Return Color.Transparent
        End Get
        Set
            MyBase.BackColor = Color.Transparent
        End Set
    End Property
End Class
