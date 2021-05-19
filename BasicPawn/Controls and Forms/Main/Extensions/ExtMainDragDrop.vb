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


Partial Public Class FormMain
    Private Sub TabControl_SourceTabs_MouseDown(sender As Object, e As MouseEventArgs) Handles TabControl_SourceTabs.MouseDown
        g_mTabControlDragTab = g_ClassTabControl.GetTabByCursorPoint()
    End Sub

    Private Sub TabControl_SourceTabs_MouseUp(sender As Object, e As MouseEventArgs) Handles TabControl_SourceTabs.MouseUp
        g_mTabControlDragTab = Nothing
    End Sub

    Private Sub TabControl_SourceTabs_MouseMove(sender As Object, e As MouseEventArgs) Handles TabControl_SourceTabs.MouseMove
        Try
            If (g_mTabControlDragTab Is Nothing OrElse e.Button <> MouseButtons.Left) Then
                Return
            End If

            TabControl_SourceTabs.DoDragDrop(g_mTabControlDragTab, DragDropEffects.Move)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub TabControl_SourceTabs_DragOver(sender As Object, e As DragEventArgs) Handles TabControl_SourceTabs.DragOver
        Try
            If (g_mTabControlDragPoint = Cursor.Position) Then
                Return
            End If

            Dim mDragTab = DirectCast(e.Data.GetData(GetType(ClassTabControl.ClassTab)), ClassTabControl.ClassTab)
            If (mDragTab Is Nothing) Then
                Return
            End If

            Dim mPointTab = g_ClassTabControl.GetTabByCursorPoint()
            If (mPointTab Is Nothing) Then
                Return
            End If

            If (mDragTab IsNot g_mTabControlDragTab) Then
                Return
            End If

            If (mDragTab Is mPointTab) Then
                Return
            End If

            Dim iDragIndex = TabControl_SourceTabs.TabPages.IndexOf(mDragTab)
            Dim iPointIndex = TabControl_SourceTabs.TabPages.IndexOf(mPointTab)
            If (iDragIndex < 0 OrElse iPointIndex < 0) Then
                Return
            End If

            e.Effect = DragDropEffects.Move
            g_ClassTabControl.SwapTabs(iDragIndex, iPointIndex)

            g_mTabControlDragPoint = Cursor.Position
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub TabControl_SourceTabs_MouseClick(sender As Object, e As MouseEventArgs) Handles TabControl_SourceTabs.MouseClick
        Try
            If (e.Button <> MouseButtons.Middle) Then
                Return
            End If

            Dim mTab = g_ClassTabControl.GetTabByCursorPoint()
            If (mTab Is Nothing) Then
                Return
            End If

            If (ClassSettings.g_bSettingsTabCloseGotoPrevious) Then
                mTab.RemoveTabGotoLast(True)
            Else
                mTab.RemoveTab(True)
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
