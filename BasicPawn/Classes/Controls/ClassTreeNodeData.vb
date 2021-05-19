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


Imports System.Runtime.Serialization

Public Class ClassTreeNodeData
    Inherits TreeNode

    Public g_mData As New Dictionary(Of String, Object)

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(text As String)
        MyBase.New(text)
    End Sub

    Public Sub New(text As String, children() As TreeNode)
        MyBase.New(text, children)
    End Sub

    Public Sub New(text As String, imageIndex As Integer, selectedImageIndex As Integer)
        MyBase.New(text, imageIndex, selectedImageIndex)
    End Sub

    Public Sub New(text As String, imageIndex As Integer, selectedImageIndex As Integer, children() As TreeNode)
        MyBase.New(text, imageIndex, selectedImageIndex, children)
    End Sub

    Protected Sub New(serializationInfo As SerializationInfo, context As StreamingContext)
        MyBase.New(serializationInfo, context)
    End Sub
End Class
