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


Partial Public Class FormSettings
    Private Sub Button_AddDatabaseItem_Click(sender As Object, e As EventArgs) Handles Button_AddDatabaseItem.Click
        Using i As New FormDatabaseInput()
            If (i.ShowDialog(Me) = DialogResult.OK) Then
                DatabaseListBox_Database.BeginUpdate()
                DatabaseListBox_Database.RemoveItemByName(i.m_Name)
                DatabaseListBox_Database.Items.Add(New ClassDatabaseListBox.ClassDatabaseItem(i.m_Name, i.m_Username))
                DatabaseListBox_Database.EndUpdate()

                Dim iItem As New ClassDatabase.STRUC_DATABASE_ITEM(i.m_Name, i.m_Username, i.m_Password)
                iItem.Save()
            End If
        End Using
    End Sub

    Private Sub Button_Refresh_Click(sender As Object, e As EventArgs) Handles Button_Refresh.Click
        DatabaseListBox_Database.FillFromDatabase()
    End Sub
End Class
