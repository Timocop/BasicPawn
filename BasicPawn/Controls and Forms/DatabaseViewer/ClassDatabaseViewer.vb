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


Public Class ClassDatabaseViewer
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.AutoScroll = True
        Me.BorderStyle = BorderStyle.FixedSingle
    End Sub

    Public Sub AddItem(sName As String, sUsername As String)
        Me.SuspendLayout()

        RemoveItemByName(sName)

        Dim mDatabaseViewerItem As New ClassDatabaseViewerItem(sName, sUsername) With {
            .Parent = Me,
            .Dock = DockStyle.Top
        }
        mDatabaseViewerItem.BringToFront()
        mDatabaseViewerItem.Show()

        Me.ResumeLayout()
    End Sub

    Public Sub RemoveItemByName(sName As String)
        Me.SuspendLayout()

        For Each iItem In GetItems()
            If (iItem.m_Name = sName) Then
                iItem.Dispose()
            End If
        Next

        Me.ResumeLayout()
    End Sub

    Public Function GetItems() As ClassDatabaseViewerItem()
        Dim lItemList As New List(Of ClassDatabaseViewerItem)

        For Each mControl As Control In Me.Controls
            If (TypeOf mControl Is ClassDatabaseViewerItem) Then
                lItemList.Add(DirectCast(mControl, ClassDatabaseViewerItem))
            End If
        Next

        Return lItemList.ToArray
    End Function

    Public Sub FillFromDatabase()
        Me.SuspendLayout()

        For Each iItem In GetItems()
            iItem.Dispose()
        Next

        For Each iItem In ClassDatabase.GetDatabaseItems
            AddItem(iItem.m_Name, iItem.m_Username)
        Next

        Me.ResumeLayout()
    End Sub
End Class
