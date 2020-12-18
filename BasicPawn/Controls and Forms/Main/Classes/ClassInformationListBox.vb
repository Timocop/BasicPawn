'BasicPawn
'Copyright(C) 2020 TheTimocop

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


Public Class ClassInformationListBox
    Inherits ListBox

    Enum ENUM_ICONS
        ICO_NONE
        ICO_DEBUG
        ICO_INFO
        ICO_WARNING
        ICO_ERROR
    End Enum
    Private g_mIcons([Enum].GetNames(GetType(ENUM_ICONS)).Length - 1) As Image

    Public Sub New()
        MyBase.New()

        g_mIcons(ENUM_ICONS.ICO_NONE) = Nothing
        g_mIcons(ENUM_ICONS.ICO_DEBUG) = My.Resources.user32_102_16x16
        g_mIcons(ENUM_ICONS.ICO_INFO) = My.Resources.user32_104_16x16
        g_mIcons(ENUM_ICONS.ICO_WARNING) = My.Resources.user32_101_16x16
        g_mIcons(ENUM_ICONS.ICO_ERROR) = My.Resources.user32_103_16x16

        Me.DrawMode = DrawMode.OwnerDrawVariable
        Me.ItemHeight = 16

        Me.HorizontalScrollbar = True
        Me.HorizontalExtent = 0

        Me.SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)

        'Only able to get DPI value when control handle is created
        Me.ItemHeight = CInt(16 * ClassTools.ClassForms.ScaleDPI())
    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        If (e.Index < 0) Then
            Return
        End If

        If (Me.Items.Count < 1 OrElse e.Index > Me.Items.Count - 1) Then
            Return
        End If

        Dim mItem = TryCast(Me.Items(e.Index), ClassInformationItem)
        If (mItem Is Nothing) Then
            Return
        End If

        Dim TEXT_MSG_OFFSET As Integer = CInt(32 * ClassTools.ClassForms.ScaleDPI())
        Dim ICON_SIZE As Integer = CInt(16 * ClassTools.ClassForms.ScaleDPI())

        'e.DrawBackground()

        If ((e.State And DrawItemState.Selected) = DrawItemState.Selected) Then
            If (ClassControlStyle.m_IsInvertedColors) Then
                'Darker Color.RoyalBlue. Orginal Color.RoyalBlue: Color.FromArgb(65, 105, 150) 
                e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(36, 59, 127)), e.Bounds)
            Else
                e.Graphics.FillRectangle(New SolidBrush(Color.LightBlue), e.Bounds)
            End If
        Else
            e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), e.Bounds)
        End If

        TextRenderer.DrawText(e.Graphics, mItem.GetDrawTextFull, Me.Font,
                              New Rectangle(e.Bounds.X + TEXT_MSG_OFFSET, e.Bounds.Y, e.Bounds.Width - TEXT_MSG_OFFSET, e.Bounds.Height),
                              Me.ForeColor,
                              TextFormatFlags.EndEllipsis Or TextFormatFlags.Left Or TextFormatFlags.VerticalCenter)

        'Primary type icon
        If (g_mIcons(mItem.m_Icon) IsNot Nothing) Then
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

            'Draw icon
            e.Graphics.DrawImage(g_mIcons(mItem.m_Icon), e.Bounds.X, e.Bounds.Y, ICON_SIZE, ICON_SIZE)

            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.Default
            e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.Default
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.Default
        End If

        'Secondary icon, mostly used for type of actions
        If (mItem.GetDrawIcon IsNot Nothing) Then
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

            'Draw icon
            e.Graphics.DrawImage(mItem.GetDrawIcon, e.Bounds.X + ICON_SIZE, e.Bounds.Y, ICON_SIZE, ICON_SIZE)

            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.Default
            e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.Default
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.Default
        End If

        e.DrawFocusRectangle()

        MyBase.OnDrawItem(e)
    End Sub

    Protected Overrides Sub OnMeasureItem(e As MeasureItemEventArgs)
        If (e.Index < 0) Then
            Return
        End If

        If (Me.Items.Count < 1 OrElse e.Index > Me.Items.Count - 1) Then
            Return
        End If

        Dim mItem = TryCast(Me.Items(e.Index), ClassInformationItem)
        If (mItem Is Nothing) Then
            Return
        End If

        Dim iTextWidth As Integer = CInt(TextRenderer.MeasureText(mItem.GetDrawTextFull, Me.Font).Width * 1.1)

        If (Me.HorizontalExtent < iTextWidth) Then
            Me.HorizontalExtent = iTextWidth
        End If

        MyBase.OnMeasureItem(e)
    End Sub

    Class ClassInformationItem
        Property m_Icon As ENUM_ICONS
        Property m_Time As Date
        Property m_Text As String

        Public Sub New(_Icon As ENUM_ICONS, _Time As Date, _Text As String)
            m_Icon = _Icon
            m_Time = _Time
            m_Text = _Text
        End Sub

        Overridable Function GetDrawText() As String
            Return m_Text
        End Function

        Overridable Function GetDrawTextFull() As String
            Return String.Format("[{0}] {1}", m_Time.ToString, GetDrawText())
        End Function

        Overridable Function GetDrawIcon() As Image
            Return Nothing
        End Function
    End Class
End Class
