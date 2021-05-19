'BasicPawn
'Copyright(C) 2021 Externet

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


Public Class ClassTreeViewColumns
    Public ReadOnly Property m_TreeView() As TreeView
        Get
            Return TreeView1
        End Get
    End Property

    Public ReadOnly Property m_Columns() As ListView.ColumnHeaderCollection
        Get
            Return ListView1.Columns
        End Get
    End Property

    Public Property m_GridView As Boolean

    Private Sub TreeView1_Click(sender As Object, e As EventArgs) Handles TreeView1.Click
        Dim mPoint As Point = TreeView1.PointToClient(Control.MousePosition)
        Dim mTreeNode As TreeNode = TreeView1.GetNodeAt(mPoint)
        If (mTreeNode Is Nothing) Then
            Return
        End If

        TreeView1.SelectedNode = mTreeNode
    End Sub

    Private Sub ListView1_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles ListView1.ColumnClick
        TreeView1.Focus()
    End Sub

    Private Sub ListView1_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles ListView1.ColumnWidthChanged
        TreeView1.Focus()
        TreeView1.Invalidate()
    End Sub

    Private Sub ListView1_ColumnWidthChanging(sender As Object, e As ColumnWidthChangingEventArgs) Handles ListView1.ColumnWidthChanging
        TreeView1.Focus()
        TreeView1.Invalidate()
    End Sub

    Private Sub TreeView1_DrawNode(sender As Object, e As DrawTreeNodeEventArgs) Handles TreeView1.DrawNode
        e.DrawDefault = True

        Dim mRect As Rectangle = e.Bounds
        Dim sColumnText As String
        Dim sSubList As String()
        Dim iFlags As TextFormatFlags

        If ((e.State And TreeNodeStates.Selected) <> 0) Then
            If ((e.State And TreeNodeStates.Focused) <> 0) Then
                e.Graphics.FillRectangle(SystemBrushes.Highlight, mRect)
            Else
                e.Graphics.FillRectangle(SystemBrushes.Control, mRect)
            End If
        Else
            e.Graphics.FillRectangle(New SolidBrush(TreeView1.BackColor), mRect)
        End If

        If (m_GridView) Then
            e.Graphics.DrawRectangle(SystemPens.Control, mRect)
        End If

        For i = 1 To ListView1.Columns.Count - 1
            mRect.Offset(ListView1.Columns(i - 1).Width, 0)
            mRect.Width = ListView1.Columns(i).Width

            If (m_GridView) Then
                e.Graphics.DrawRectangle(SystemPens.Control, mRect)
            End If

            sSubList = TryCast(e.Node.Tag, String())

            If (sSubList IsNot Nothing AndAlso i <= sSubList.Length) Then
                sColumnText = sSubList(i - 1)
            Else
                sColumnText = ""
            End If

            iFlags = TextFormatFlags.EndEllipsis

            Select Case (ListView1.Columns(i).TextAlign)
                Case HorizontalAlignment.Center
                    iFlags = iFlags Or TextFormatFlags.HorizontalCenter

                Case HorizontalAlignment.Left
                    iFlags = iFlags Or TextFormatFlags.Left

                Case HorizontalAlignment.Right
                    iFlags = iFlags Or TextFormatFlags.Right

            End Select

            mRect.Y += 1

            If ((e.State And TreeNodeStates.Selected) <> 0 AndAlso (e.State And TreeNodeStates.Focused) <> 0) Then
                TextRenderer.DrawText(e.Graphics, sColumnText, e.Node.NodeFont, mRect, SystemColors.HighlightText, iFlags)
            Else
                TextRenderer.DrawText(e.Graphics, sColumnText, e.Node.NodeFont, mRect, e.Node.ForeColor, e.Node.BackColor, iFlags)
            End If

            mRect.Y -= 1
        Next
    End Sub
End Class
