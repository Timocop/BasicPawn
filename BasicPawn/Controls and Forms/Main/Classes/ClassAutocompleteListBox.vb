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


Public Class ClassAutocompleteListBox
    Inherits ListBox

    Public Sub New()
        MyBase.New()

        Me.ItemHeight = 16
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

        Dim mItem = TryCast(Me.Items(e.Index), ClassAutocompleteItem)
        If (mItem Is Nothing) Then
            Return
        End If

        Const TEXT_NAME_OFFSET = 24
        Const ICON_LOC_OFFSET = 2
        Const ICON_SIZE = 16

        'e.DrawBackground()
        e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), e.Bounds)

        TextRenderer.DrawText(e.Graphics, mItem.m_Types, Me.Font,
                              New Rectangle(e.Bounds.X + TEXT_NAME_OFFSET, e.Bounds.Y, e.Bounds.Width - TEXT_NAME_OFFSET, e.Bounds.Height),
                              Me.ForeColor,
                              TextFormatFlags.EndEllipsis Or TextFormatFlags.Left Or TextFormatFlags.VerticalCenter)

        TextRenderer.DrawText(e.Graphics, mItem.m_Method, Me.Font,
                              New Rectangle(e.Bounds.X + TEXT_NAME_OFFSET, e.Bounds.Y, e.Bounds.Width - TEXT_NAME_OFFSET, e.Bounds.Height),
                              Me.ForeColor,
                              TextFormatFlags.EndEllipsis Or TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.HorizontalCenter)

        'Draw icon
        e.Graphics.DrawImage(mItem.m_Icon, e.Bounds.X + ICON_LOC_OFFSET, e.Bounds.Y + ICON_LOC_OFFSET, ICON_SIZE, ICON_SIZE)

        e.DrawFocusRectangle()

        MyBase.OnDrawItem(e)
    End Sub

    Class ClassAutocompleteItem
        Property m_Icon As Image
        Property m_Types As String
        Property m_Method As String
        Property m_Autocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE

        Public Sub New(_Icon As Image, _Types As String, _Method As String, _Autocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            m_Icon = _Icon
            m_Types = _Types
            m_Method = _Method
            m_Autocomplete = _Autocomplete
        End Sub
    End Class
End Class
