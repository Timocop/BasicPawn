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


Public Class ClassRecentListBox
    Inherits ListBox

    Public Event OnItemClick(iIndex As Integer)
    Public Event OnItemDoubleClick(iIndex As Integer)
    Public Event OnButtonClick(iIndex As Integer)
    Public Event OnCheckBoxClick(iIndex As Integer)

    Public Sub New()
        MyBase.New()

        Me.ItemHeight = 32
        Me.DrawMode = DrawMode.OwnerDrawVariable

        Me.SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub


    Private Sub ClassRecentListBox_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        Dim mCurPos = Me.PointToClient(Cursor.Position)
        Dim iIndex = Me.IndexFromPoint(mCurPos)
        If (iIndex < 0) Then
            Return
        End If

        If (Me.Items.Count < 1 OrElse iIndex > Me.Items.Count - 1) Then
            Return
        End If

        If (TypeOf Me.Items(iIndex) IsNot ClassRecentItem) Then
            Return
        End If

        Dim mButtonRect = GetButtonRectangle(iIndex)
        Dim mCheckBoxRect = GetCheckBoxRectangle(iIndex)

        If (mButtonRect.Contains(mCurPos) OrElse mCheckBoxRect.Contains(mCurPos)) Then
            Me.Cursor = Cursors.Hand
        Else
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub ClassRecentListBox_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub ClassRecentListBox_Click(sender As Object, e As EventArgs) Handles Me.Click
        Dim mCurPos = Me.PointToClient(Cursor.Position)
        Dim iIndex = Me.IndexFromPoint(mCurPos)
        If (iIndex < 0) Then
            Return
        End If

        If (Me.Items.Count < 1 OrElse iIndex > Me.Items.Count - 1) Then
            Return
        End If

        If (TypeOf Me.Items(iIndex) IsNot ClassRecentItem) Then
            Return
        End If

        Dim mButtonRect = GetButtonRectangle(iIndex)
        Dim mCheckBoxRect = GetCheckBoxRectangle(iIndex)

        Select Case (True)
            Case mButtonRect.Contains(mCurPos)
                RaiseEvent OnButtonClick(iIndex)

            Case mCheckBoxRect.Contains(mCurPos)
                RaiseEvent OnCheckBoxClick(iIndex)

                Dim mItem = DirectCast(Me.Items(iIndex), ClassRecentItem)
                mItem.m_Checked = Not mItem.m_Checked

                Me.Invalidate(mCheckBoxRect)

            Case Else
                RaiseEvent OnItemClick(iIndex)
        End Select
    End Sub

    Private Sub ClassRecentListBox_DoubleClick(sender As Object, e As EventArgs) Handles Me.DoubleClick
        Dim mCurPos = Me.PointToClient(Cursor.Position)
        Dim iIndex = Me.IndexFromPoint(mCurPos)
        If (iIndex < 0) Then
            Return
        End If

        If (Me.Items.Count < 1 OrElse iIndex > Me.Items.Count - 1) Then
            Return
        End If

        If (TypeOf Me.Items(iIndex) IsNot ClassRecentItem) Then
            Return
        End If

        Dim mButtonRect = GetButtonRectangle(iIndex)
        Dim mCheckBoxRect = GetCheckBoxRectangle(iIndex)

        Select Case (True)
            Case mButtonRect.Contains(mCurPos)
            Case mCheckBoxRect.Contains(mCurPos)
                'Nothing

            Case Else
                RaiseEvent OnItemDoubleClick(iIndex)
        End Select
    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        If (e.Index < 0) Then
            Return
        End If

        If (Me.Items.Count < 1 OrElse e.Index > Me.Items.Count - 1) Then
            Return
        End If

        Select Case (True)
            Case (TypeOf Me.Items(e.Index) Is ClassRecentItem)
                Dim mItem = TryCast(Me.Items(e.Index), ClassRecentItem)
                If (mItem Is Nothing) Then
                    Return
                End If

                Const TEXT_X_OFFSET = 28
                Const TEXT_SPACE_OFFSET = 2

                e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), e.Bounds)

                TextRenderer.DrawText(e.Graphics, mItem.m_Title, New Font(Me.Font, FontStyle.Bold),
                                      New Rectangle(e.Bounds.X + TEXT_X_OFFSET, e.Bounds.Y + TEXT_SPACE_OFFSET, e.Bounds.Width - TEXT_X_OFFSET, e.Bounds.Height - TEXT_SPACE_OFFSET),
                                      Me.ForeColor,
                                      TextFormatFlags.EndEllipsis Or TextFormatFlags.Left Or TextFormatFlags.Top)

                TextRenderer.DrawText(e.Graphics, mItem.m_Path, New Font(Me.Font, FontStyle.Regular),
                                      New Rectangle(e.Bounds.X + TEXT_X_OFFSET, e.Bounds.Y, e.Bounds.Width - TEXT_X_OFFSET, e.Bounds.Height - TEXT_SPACE_OFFSET),
                                      Me.ForeColor,
                                      TextFormatFlags.EndEllipsis Or TextFormatFlags.Left Or TextFormatFlags.Bottom)

                'Draw checkbox
                Dim mCheckBoxRect = GetCheckBoxRectangle(e.Bounds)
                CheckBoxRenderer.DrawCheckBox(e.Graphics, New Point(mCheckBoxRect.X, mCheckBoxRect.Y), If(mItem.m_Checked, VisualStyles.CheckBoxState.CheckedNormal, VisualStyles.CheckBoxState.UncheckedNormal))

                'Draw remove button
                e.Graphics.DrawImage(My.Resources.Bmp_ButtonDeleteDefault, GetButtonRectangle(e.Bounds))

                e.DrawFocusRectangle()

                MyBase.OnDrawItem(e)

            Case (TypeOf Me.Items(e.Index) Is ClassTitleItem)
                Dim mItem = TryCast(Me.Items(e.Index), ClassTitleItem)
                If (mItem Is Nothing) Then
                    Return
                End If

                Const TEXT_NAME_OFFSET = 2

                e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), e.Bounds)

                TextRenderer.DrawText(e.Graphics, mItem.m_Title, New Font(Me.Font.FontFamily, 12, FontStyle.Bold),
                              New Rectangle(e.Bounds.X + TEXT_NAME_OFFSET, e.Bounds.Y, e.Bounds.Width - TEXT_NAME_OFFSET, e.Bounds.Height),
                              Color.RoyalBlue,
                              TextFormatFlags.EndEllipsis Or TextFormatFlags.Left Or TextFormatFlags.VerticalCenter)

                MyBase.OnDrawItem(e)

            Case Else
                Return

        End Select
    End Sub

    Private Function GetButtonRectangle(i As Integer) As Rectangle
        Return GetButtonRectangle(Me.GetItemRectangle(i))
    End Function

    Private Function GetButtonRectangle(i As Rectangle) As Rectangle
        Const BUTTON_SIZE = 16
        Const BUTTON_OFFSET = 24

        Return New Rectangle(i.X + i.Width - BUTTON_OFFSET, i.Y + i.Height - BUTTON_OFFSET, BUTTON_SIZE, BUTTON_SIZE)
    End Function

    Private Function GetCheckBoxRectangle(i As Integer) As Rectangle
        Return GetCheckBoxRectangle(Me.GetItemRectangle(i))
    End Function

    Private Function GetCheckBoxRectangle(i As Rectangle) As Rectangle
        Const CHECKBOX_SIZE = 16
        Const CHECKBOX_OFFSET = 8

        Return New Rectangle(i.X + CHECKBOX_OFFSET, i.Y + CHECKBOX_OFFSET, CHECKBOX_SIZE, CHECKBOX_SIZE)
    End Function

    Class ClassRecentItem
        Property m_RecentFile As String = ""
        Property m_RecentDate As Date = Date.MinValue

        Property m_Title As String = ""
        Property m_Path As String = ""

        Property m_Checked As Boolean = False

        ReadOnly Property m_IsProjectFile As Boolean
            Get
                Return (IO.Path.GetExtension(m_RecentFile).ToLower = UCProjectBrowser.ClassProjectControl.g_sProjectExtension)
            End Get
        End Property

        Public Sub New(_File As String)
            Try
                m_RecentFile = _File
                m_RecentDate = IO.File.GetLastWriteTime(_File)

                m_Title = String.Format("{0} - {1}", m_RecentDate.ToString, IO.Path.GetFileName(m_RecentFile))
                m_Path = IO.Path.GetFullPath(m_RecentFile)
            Catch ex As Exception
            End Try
        End Sub
    End Class

    Class ClassTitleItem
        Property m_Title As String

        Public Sub New(_Title As String)
            m_Title = _Title
        End Sub
    End Class
End Class
