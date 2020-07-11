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


Imports System.Drawing

Public Class ClassReportListBox
    Inherits ListBox

    Public Sub New()
        MyBase.New()

        Me.ItemHeight = 32
        Me.DrawMode = DrawMode.OwnerDrawVariable

        Me.SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        If (e.Index < 0) Then
            Return
        End If

        If (Me.Items.Count < 1 OrElse e.Index > Me.Items.Count - 1) Then
            Return
        End If

        Dim mItem = TryCast(Me.Items(e.Index), ClassReportItem)
        If (mItem Is Nothing) Then
            Return
        End If

        Const TEXT_X_OFFSET = 28
        Const TEXT_SPACE_OFFSET = 2
        Const ICON_LOC_OFFSET = 3
        Const ICON_SIZE = 16

        'e.DrawBackground()
        e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), e.Bounds)

        Dim mDateTextSize = TextRenderer.MeasureText(mItem.m_Date, New Font(Me.Font, FontStyle.Regular))

        TextRenderer.DrawText(e.Graphics, mItem.m_Title, New Font(Me.Font, FontStyle.Bold),
                              New Rectangle(e.Bounds.X + TEXT_X_OFFSET, e.Bounds.Y + TEXT_SPACE_OFFSET, e.Bounds.Width - mDateTextSize.Width - TEXT_X_OFFSET, e.Bounds.Height - TEXT_SPACE_OFFSET),
                              Me.ForeColor,
                              TextFormatFlags.EndEllipsis Or TextFormatFlags.Left Or TextFormatFlags.Top)

        TextRenderer.DrawText(e.Graphics, mItem.m_Text, New Font(Me.Font, FontStyle.Regular),
                              New Rectangle(e.Bounds.X + TEXT_X_OFFSET, e.Bounds.Y, e.Bounds.Width - mDateTextSize.Width - TEXT_X_OFFSET, e.Bounds.Height - TEXT_SPACE_OFFSET),
                              Me.ForeColor,
                              TextFormatFlags.EndEllipsis Or TextFormatFlags.Left Or TextFormatFlags.Bottom)

        TextRenderer.DrawText(e.Graphics, mItem.m_Date, New Font(Me.Font, FontStyle.Regular),
                              New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height),
                              Me.ForeColor,
                              TextFormatFlags.Right Or TextFormatFlags.VerticalCenter)

        e.Graphics.DrawImage(mItem.m_Image, e.Bounds.X + ICON_LOC_OFFSET, e.Bounds.Y + ICON_LOC_OFFSET, ICON_SIZE, ICON_SIZE)

        e.DrawFocusRectangle()

        MyBase.OnDrawItem(e)
    End Sub

    Class ClassReportItem
        Property m_Title As String
        Property m_Text As String
        Property m_Date As String
        Property m_Image As Image
        Property m_IReport As FormReportManager.ClassReports.ClassReportItems.IReportInterface

        Public Sub New(_Title As String, _Text As String, _Date As String, _Image As Image, _IReport As FormReportManager.ClassReports.ClassReportItems.IReportInterface)
            m_Title = _Title
            m_Text = _Text
            m_Date = _Date
            m_Image = _Image
            m_IReport = _IReport
        End Sub
    End Class
End Class
