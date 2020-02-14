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


Imports System.Runtime.InteropServices

Partial Public Class FormMain
    Implements IMessageFilter

    Private g_bInit As Boolean = False

    Class ClassWin32
        <DllImport("user32.dll")>
        Public Shared Function WindowFromPoint(ByVal pt As Point) As IntPtr
        End Function
        <DllImport("user32.dll")>
        Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wp As IntPtr, ByVal lp As IntPtr) As IntPtr
        End Function
    End Class

    Public Sub InitializeFilter()
        If (g_bInit) Then
            Return
        End If

        g_bInit = True

        Application.AddMessageFilter(Me)
    End Sub

    Private Const WM_MOUSEWHEEL As Integer = &H20A
    Public Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
        If (Not ClassSettings.g_iSettingsAutoHoverScroll) Then
            Return False
        End If

        Select Case (m.Msg)
            Case WM_MOUSEWHEEL
                Dim hWnd As IntPtr = ClassWin32.WindowFromPoint(Cursor.Position)

                If hWnd <> IntPtr.Zero AndAlso hWnd <> m.HWnd AndAlso Control.FromHandle(hWnd) IsNot Nothing Then
                    ClassWin32.SendMessage(hWnd, m.Msg, m.WParam, m.LParam)
                    Return True
                End If
        End Select

        Return False
    End Function
End Class
