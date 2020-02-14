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

Public Class ClassRichTextBoxFix
    Inherits RichTextBox

    Class ClassWin32
        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As IntPtr) As Integer
        End Function
    End Class

    Private g_bAllowZoom As Boolean = False
    Private g_bSelectionEnabled As Boolean = True

    Property m_AllowZoom As Boolean
        Get
            Return g_bAllowZoom
        End Get
        Set(value As Boolean)
            g_bAllowZoom = value
        End Set
    End Property

    Property m_SelectionEnabled As Boolean
        Get
            Return g_bSelectionEnabled
        End Get
        Set(value As Boolean)
            g_bSelectionEnabled = value
        End Set
    End Property

    'm_AllowZoom
    Const WM_MOUSEWHEEL As Integer = &H20A
    Const WM_VSCROLL As Integer = &H115
    Const WM_HSCROLL As Integer = &H114
    Const EM_SETZOOM As Integer = &H4E1

    'm_SelectionEnabled
    Const WM_SETFOCUS As Integer = &H7
    Const WM_KILLFOCUS As Integer = &H8

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case (m.Msg)
            Case WM_MOUSEWHEEL, WM_VSCROLL, WM_HSCROLL
                MyBase.WndProc(m)

                If ((Control.ModifierKeys And Keys.Control) <> 0) Then
                    ClassWin32.SendMessage(Me.Handle, EM_SETZOOM, IntPtr.Zero, IntPtr.Zero)
                End If

            Case WM_SETFOCUS
                If (Not g_bSelectionEnabled) Then
                    m.Msg = WM_KILLFOCUS
                End If

                MyBase.WndProc(m)

            Case Else
                MyBase.WndProc(m)
        End Select
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)

        'WINE BUG: Text color keeps resetting when text changes. Re-apply color on text change.		
        Me.BackColor = Me.BackColor
        Me.ForeColor = Me.ForeColor
    End Sub
End Class
