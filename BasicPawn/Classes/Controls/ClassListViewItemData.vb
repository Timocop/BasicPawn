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


Public Class ClassListViewItemData
    Inherits ListViewItem

    Public g_mData As New Dictionary(Of String, Object)

    Public Sub New()
        MyBase.New
    End Sub

    Public Sub New(items() As String)
        MyBase.New(items)
    End Sub

    Public Sub New(group As ListViewGroup)
        MyBase.New(group)
    End Sub
    Public Sub New(text As String)
        MyBase.New(text)
    End Sub
    Public Sub New(text As String, imageIndex As Integer)
        MyBase.New(text, imageIndex)
    End Sub

    Public Sub New(items() As String, imageIndex As Integer)
        MyBase.New(items, imageIndex)
    End Sub

    Public Sub New(subItems() As ListViewSubItem, imageIndex As Integer)
        MyBase.New(subItems, imageIndex)
    End Sub

    Public Sub New(items() As String, imageKey As String)
        MyBase.New(items, imageKey)
    End Sub

    Public Sub New(text As String, group As ListViewGroup)
        MyBase.New(text, group)
    End Sub

    Public Sub New(items() As String, group As ListViewGroup)
        MyBase.New(items, group)
    End Sub

    Public Sub New(text As String, imageKey As String)
        MyBase.New(text, imageKey)
    End Sub

    Public Sub New(subItems() As ListViewSubItem, imageKey As String)
        MyBase.New(subItems, imageKey)
    End Sub

    Public Sub New(subItems() As ListViewSubItem, imageKey As String, group As ListViewGroup)
        MyBase.New(subItems, imageKey, group)
    End Sub

    Public Sub New(items() As String, imageKey As String, group As ListViewGroup)
        MyBase.New(items, imageKey, group)
    End Sub

    Public Sub New(text As String, imageIndex As Integer, group As ListViewGroup)
        MyBase.New(text, imageIndex, group)
    End Sub

    Public Sub New(items() As String, imageIndex As Integer, group As ListViewGroup)
        MyBase.New(items, imageIndex, group)
    End Sub

    Public Sub New(text As String, imageKey As String, group As ListViewGroup)
        MyBase.New(text, imageKey, group)
    End Sub

    Public Sub New(subItems() As ListViewSubItem, imageIndex As Integer, group As ListViewGroup)
        MyBase.New(subItems, imageIndex, group)
    End Sub

    Public Sub New(items() As String, imageKey As String, foreColor As Color, backColor As Color, font As Font)
        MyBase.New(items, imageKey, foreColor, backColor, font)
    End Sub

    Public Sub New(items() As String, imageIndex As Integer, foreColor As Color, backColor As Color, font As Font)
        MyBase.New(items, imageIndex, foreColor, backColor, font)
    End Sub

    Public Sub New(items() As String, imageIndex As Integer, foreColor As Color, backColor As Color, font As Font, group As ListViewGroup)
        MyBase.New(items, imageIndex, foreColor, backColor, font, group)
    End Sub

    Public Sub New(items() As String, imageKey As String, foreColor As Color, backColor As Color, font As Font, group As ListViewGroup)
        MyBase.New(items, imageKey, foreColor, backColor, font, group)
    End Sub
End Class
