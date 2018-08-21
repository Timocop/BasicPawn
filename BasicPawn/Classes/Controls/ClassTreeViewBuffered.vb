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


Imports System.Runtime.InteropServices

Public Class ClassTreeViewBuffered
    Inherits TreeView

    Class ClassNatives
        Public Const TVM_SETEXTENDEDSTYLE As Integer = &H1100 + 44
        Public Const TVS_EX_DOUBLEBUFFER As Integer = &H4

        <DllImport("user32.dll")>
        Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        End Function
    End Class

    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        ClassNatives.SendMessage(Me.Handle, ClassNatives.TVM_SETEXTENDEDSTYLE, New IntPtr(ClassNatives.TVS_EX_DOUBLEBUFFER), New IntPtr(ClassNatives.TVS_EX_DOUBLEBUFFER))
        MyBase.OnHandleCreated(e)
    End Sub
End Class
