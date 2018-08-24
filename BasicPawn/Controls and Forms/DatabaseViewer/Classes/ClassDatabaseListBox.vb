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


Public Class ClassDatabaseListBox
    Inherits ListBox

    Public Sub New()
        MyBase.New()

        Me.ItemHeight = 32
        Me.DrawMode = DrawMode.OwnerDrawVariable

        Me.SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub

    Private Sub ClassDatabaseListBox_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        Dim mCurPos = Me.PointToClient(Cursor.Position)
        Dim iIndex = Me.IndexFromPoint(mCurPos)
        If (iIndex < 0) Then
            Return
        End If

        If (Me.Items.Count < 1 OrElse iIndex > Me.Items.Count - 1) Then
            Return
        End If

        Dim mButtonRect = GetButtonRectangle(iIndex)

        If (mButtonRect.Contains(mCurPos)) Then
            Me.Cursor = Cursors.Hand
        Else
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub ClassDatabaseListBox_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub ClassDatabaseListBox_Click(sender As Object, e As EventArgs) Handles Me.Click
        Dim mCurPos = Me.PointToClient(Cursor.Position)
        Dim iIndex = Me.IndexFromPoint(mCurPos)
        If (iIndex < 0) Then
            Return
        End If

        If (Me.Items.Count < 1 OrElse iIndex > Me.Items.Count - 1) Then
            Return
        End If

        Dim mButtonRect = GetButtonRectangle(iIndex)

        If (mButtonRect.Contains(mCurPos)) Then
            OnButtonClick(iIndex)
        End If
    End Sub

    Private Sub OnButtonClick(iIndex As Integer)
        Dim mItem = TryCast(Me.Items(iIndex), ClassDatabaseItem)
        If (mItem Is Nothing) Then
            Return
        End If

        For Each i In ClassDatabase.GetDatabaseItems
            If (i.m_Name <> mItem.m_Name OrElse Not i.HasUserAccess) Then
                Continue For
            End If

            i.Remove()
        Next

        Me.Items.RemoveAt(iIndex)
    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        If (e.Index < 0) Then
            Return
        End If

        If (Me.Items.Count < 1 OrElse e.Index > Me.Items.Count - 1) Then
            Return
        End If

        Dim mItem = TryCast(Me.Items(e.Index), ClassDatabaseItem)
        If (mItem Is Nothing) Then
            Return
        End If

        Const TEXT_NAME_OFFSET = 32
        Const ICON_LOC_OFFSET = 8
        Const ICON_SIZE = 16

        'e.DrawBackground()
        e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), e.Bounds)

        TextRenderer.DrawText(e.Graphics, mItem.m_Name, Me.Font,
                              New Rectangle(e.Bounds.X + TEXT_NAME_OFFSET, e.Bounds.Y, e.Bounds.Width - TEXT_NAME_OFFSET, e.Bounds.Height),
                              Me.ForeColor,
                              TextFormatFlags.EndEllipsis Or TextFormatFlags.Left Or TextFormatFlags.VerticalCenter)

        TextRenderer.DrawText(e.Graphics, mItem.m_Username, Me.Font,
                              New Rectangle(e.Bounds.X + TEXT_NAME_OFFSET, e.Bounds.Y, e.Bounds.Width - TEXT_NAME_OFFSET, e.Bounds.Height),
                              Me.ForeColor,
                              TextFormatFlags.EndEllipsis Or TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.HorizontalCenter)

        'Draw icon
        e.Graphics.DrawImage(My.Resources.imageres_5360_16x16, e.Bounds.X + ICON_LOC_OFFSET, e.Bounds.Y + ICON_LOC_OFFSET, ICON_SIZE, ICON_SIZE)

        'Draw remove button
        e.Graphics.DrawImage(My.Resources.Bmp_ButtonDeleteDefault, GetButtonRectangle(e.Bounds))

        e.DrawFocusRectangle()

        MyBase.OnDrawItem(e)
    End Sub

    Private Function GetButtonRectangle(i As Integer) As Rectangle
        Dim j = Me.GetItemRectangle(i)
        Return GetButtonRectangle(j)
    End Function

    Private Function GetButtonRectangle(i As Rectangle) As Rectangle
        Const BUTTON_SIZE = 16
        Const BUTTON_OFFSET = 24

        Return New Rectangle(i.X + i.Width - BUTTON_OFFSET, i.Y + i.Height - BUTTON_OFFSET, BUTTON_SIZE, BUTTON_SIZE)
    End Function

    Public Sub FillFromDatabase()
        Me.BeginUpdate()
        Me.Items.Clear()

        For Each iItem In ClassDatabase.GetDatabaseItems
            Me.Items.Add(New ClassDatabaseItem(iItem.m_Name, iItem.m_Username))
        Next

        Me.EndUpdate()
    End Sub

    Public Sub RemoveItemByName(sName As String)
        Me.BeginUpdate()

        For i = Me.Items.Count - 1 To 0 Step -1
            Dim mItem = TryCast(Me.Items(i), ClassDatabaseItem)
            If (mItem Is Nothing) Then
                Continue For
            End If

            If (mItem.m_Name <> sName) Then
                Continue For
            End If

            Me.Items.RemoveAt(i)
        Next

        Me.EndUpdate()
    End Sub

    Class ClassDatabaseItem
        Property m_Name As String
        Property m_Username As String

        Public Sub New(_Name As String, _Username As String)
            m_Name = _Name
            m_Username = _Username
        End Sub
    End Class
End Class
