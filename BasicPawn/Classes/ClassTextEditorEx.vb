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


Imports ICSharpCode.TextEditor

Public Class TextEditorControlEx
    Inherits TextEditorControl

    Public Event ProcessCmdKeyEvent(ByRef bBlock As Boolean, ByRef iMsg As Message, iKeys As Keys)

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, e As Keys) As Boolean
        Dim bBLock As Boolean = False

        RaiseEvent ProcessCmdKeyEvent(bBLock, msg, e)

        If (bBLock) Then
            Return True
        End If

        Return MyBase.ProcessCmdKey(msg, e)
    End Function
End Class
