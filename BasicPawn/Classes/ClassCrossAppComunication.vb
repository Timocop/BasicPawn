'BasicPawn
'Copyright(C) 2016 TheTimocop

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

Public Class ClassCrossAppComunication
    Inherits NativeWindow
    Implements IDisposable

    Private g_mReceiveForm As New ClassReceiverForm
    Private g_sReceiveServerName As String = ""
    Private g_bEnforceEncoding As Boolean = True
    Private g_iEncodeKeyBytes As Byte() = New Byte() {2, 4, 8, 16, 32, 64, 128}

    Public Event OnMessageReceive(mClassMessage As ClassMessage)

    Private Class ClassReceiverForm
        Inherits Form

        Public g_bAllowDisponse As Boolean = False

        Protected Overrides Sub Dispose(disposing As Boolean)
            If (Not g_bAllowDisponse) Then
                Return
            End If

            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Sub OnVisibleChanged(e As EventArgs)
            MyBase.OnVisibleChanged(e)
            Me.Visible = False
        End Sub
    End Class

    Private Class ClassWndSearcher
        Public Shared Function SearchForWindow(sWndClass As String, sWndTitle As String) As IntPtr()
            Dim mSearchData As New ClassSearchData() With {
                                                    .sWndClass = sWndClass,
                                                    .sWndTitle = sWndTitle}
            EnumWindows(New EnumWindowsProc(AddressOf EnumProc), mSearchData)
            Return mSearchData.hWndList.ToArray
        End Function

        Public Shared Function EnumProc(hWnd As IntPtr, ByRef mSearchData As ClassSearchData) As Boolean
            Dim SB As New Text.StringBuilder(1024)
            GetClassName(hWnd, SB, SB.Capacity)

            If (String.IsNullOrEmpty(mSearchData.sWndClass) OrElse SB.ToString() = mSearchData.sWndClass) Then
                SB = New Text.StringBuilder(1024)
                GetWindowText(hWnd, SB, SB.Capacity)

                If (String.IsNullOrEmpty(mSearchData.sWndTitle) OrElse SB.ToString() = mSearchData.sWndTitle) Then
                    mSearchData.hWndList.Add(hWnd)
                    'Uncomment to only send to one instance
                    'Return False
                End If
            End If

            Return True
        End Function

        Public Class ClassSearchData
            Public sWndClass As String
            Public sWndTitle As String
            Public hWndList As New List(Of IntPtr)
        End Class

        Private Delegate Function EnumWindowsProc(hWnd As IntPtr, ByRef data As ClassSearchData) As Boolean

        <DllImport("user32.dll")>
        Private Shared Function EnumWindows(mEnumWindowsProc As EnumWindowsProc, ByRef mSearchData As ClassSearchData) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Public Shared Function GetClassName(hWnd As IntPtr, sbClassname As System.Text.StringBuilder, iMaxCount As Integer) As Integer
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
        Public Shared Function GetWindowText(hWnd As IntPtr, sbTitle As System.Text.StringBuilder, iMaxCount As Integer) As Integer
        End Function
    End Class

    Private Class ClassWin32
        Public Const WM_CLOSE As Integer = 16
        Public Const BN_CLICKED As Integer = 245
        Public Const WM_COPYDATA As Integer = &H4A

        Public Structure CopyDataStruct
            Public dwData As IntPtr
            Public cbData As Integer
            Public lpData As IntPtr

            Public Sub Free()
                If Me.lpData <> IntPtr.Zero Then
                    LocalFree(Me.lpData)
                    Me.lpData = IntPtr.Zero
                End If
            End Sub
        End Structure

        Public Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr

        <DllImport("User32.dll")>
        Public Shared Function SendMessage(hWnd As Integer, Msg As Integer, wParam As Integer, <MarshalAs(UnmanagedType.LPStr)> lParam As String) As Integer
        End Function

        <DllImport("User32.dll")>
        Public Shared Function SendMessage(hWnd As Integer, Msg As Integer, wParam As Integer, lParam As Integer) As Integer
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Public Shared Function SendMessage(hWnd As Integer, msg As Integer, wParam As Integer, lParam As IntPtr) As Integer
        End Function

        <DllImport("user32.dll")>
        Public Shared Function SendMessage(hWnd As IntPtr, Msg As Integer, wParam As IntPtr, ByRef lParam As CopyDataStruct) As Integer
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)>
        Public Shared Function LocalAlloc(flag As Integer, size As Integer) As IntPtr
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)>
        Public Shared Function LocalFree(p As IntPtr) As IntPtr
        End Function

        Public Sub New()
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class


    Public Property EncodeKeyBytes As Byte()
        Get
            Return g_iEncodeKeyBytes
        End Get
        Set(value As Byte())
            g_iEncodeKeyBytes = value
        End Set
    End Property

    Public Property EnforceEncoding As Boolean
        Get
            Return g_bEnforceEncoding
        End Get
        Set(value As Boolean)
            g_bEnforceEncoding = value
        End Set
    End Property


    Public Sub New()
        g_sReceiveServerName = ""
        g_mReceiveForm.Text = g_sReceiveServerName
    End Sub

    Public Sub Hook()
        Me.ReleaseHandle()

        g_mReceiveForm.Text = g_sReceiveServerName

        Me.AssignHandle(g_mReceiveForm.Handle)
    End Sub

    Public Sub Hook(sReceiveServerName As String)
        Me.ReleaseHandle()

        g_sReceiveServerName = sReceiveServerName
        g_mReceiveForm.Text = sReceiveServerName

        Me.AssignHandle(g_mReceiveForm.Handle)
    End Sub

    Public Sub Unhook()
        Me.ReleaseHandle()
        g_mReceiveForm.Text = ""
    End Sub

    Public Class ClassMessage
        Private g_sMessageName As String = ""
        Private g_iSenderPID As Integer = -1
        Private g_sMessages As String() = New String() {}

        Private g_sFormatedMessageName As String = ""
        Private g_sFormatedSenderPID As String = ""
        Private g_sFormatedMessages As String() = New String() {}

        Private g_sMessageSeperator As Char = "|"c



        Public Sub New(sFormatedMessage As String)
            FormatedMessage = sFormatedMessage
        End Sub
        Public Sub New(sMessageName As String, ParamArray sMessages As String())
            MessageName = sMessageName
            SenderPID = Process.GetCurrentProcess.Id
            Messages = sMessages
        End Sub

        Public Property MessageName As String
            Get
                Return g_sMessageName
            End Get
            Set(value As String)
                g_sMessageName = value

                g_sFormatedMessageName = g_sMessageName
                g_sFormatedMessageName = Convert.ToBase64String(Text.Encoding.Default.GetBytes(g_sFormatedMessageName))
            End Set
        End Property

        Public Property SenderPID As Integer
            Get
                Return g_iSenderPID
            End Get
            Set(value As Integer)
                g_iSenderPID = value

                g_sFormatedSenderPID = CStr(g_iSenderPID)
                g_sFormatedSenderPID = Convert.ToBase64String(Text.Encoding.Default.GetBytes(g_sFormatedSenderPID))
            End Set
        End Property

        Public Property Messages As String()
            Get
                Return g_sMessages
            End Get
            Set(value As String())
                g_sMessages = value

                g_sFormatedMessages = CType(g_sMessages.Clone, String())
                For i = 0 To g_sFormatedMessages.Length - 1
                    g_sFormatedMessages(i) = Convert.ToBase64String(Text.Encoding.Default.GetBytes(g_sFormatedMessages(i)))
                Next
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the encoded message. Everything empty on error.
        ''' </summary>
        ''' <returns></returns>
        Public Property FormatedMessage As String
            Get
                With New Text.StringBuilder
                    .Append(g_sFormatedMessageName)
                    .Append(g_sMessageSeperator)
                    .Append(g_sFormatedSenderPID)
                    .Append(g_sMessageSeperator)
                    .Append(String.Join(g_sMessageSeperator, g_sFormatedMessages))

                    Return .ToString
                End With
            End Get
            Set(value As String)
                Try
                    MessageName = ""
                    SenderPID = -1
                    Messages = New String() {}

                    Dim sSplitted As String() = value.Split(g_sMessageSeperator)

                    Dim lMessagesList As New List(Of String)

                    For i = 0 To sSplitted.Length - 1
                        Select Case (i)
                            Case 0
                                MessageName = Text.Encoding.Default.GetString(Convert.FromBase64String(sSplitted(i)))
                            Case 1
                                SenderPID = CInt(Text.Encoding.Default.GetString(Convert.FromBase64String(sSplitted(i))))
                            Case Else
                                lMessagesList.Add(Text.Encoding.Default.GetString(Convert.FromBase64String(sSplitted(i))))
                        End Select
                    Next

                    Messages = lMessagesList.ToArray
                Catch ex As Exception
                    MessageName = ""
                    SenderPID = -1
                    Messages = New String() {}
                End Try
            End Set
        End Property
    End Class

    ''' <summary>
    ''' Sends a message to all instances with the same server name.
    ''' </summary>
    ''' <param name="mClassMessage"></param>
    ''' <param name="bNotMe"></param>
    Public Sub SendMessage(mClassMessage As ClassMessage, Optional bNotMe As Boolean = True)
        If (String.IsNullOrEmpty(g_sReceiveServerName)) Then
            Throw New ArgumentException("Server name is null")
        End If

        For Each hWnd In ClassWndSearcher.SearchForWindow(Nothing, g_sReceiveServerName)
            If (bNotMe AndAlso hWnd = g_mReceiveForm.Handle) Then
                Continue For
            End If

            SendMessageEx(hWnd, mClassMessage.FormatedMessage)
        Next
    End Sub

    ''' <summary>
    ''' Sends a message to all instances with the same server name.
    ''' </summary>
    ''' <param name="sServerName"></param>
    ''' <param name="mClassMessage"></param>
    ''' <param name="bNotMe"></param>
    Public Sub SendMessage(sServerName As String, mClassMessage As ClassMessage, Optional bNotMe As Boolean = True)
        If (String.IsNullOrEmpty(sServerName)) Then
            Throw New ArgumentException("Server name is null")
        End If

        For Each hWnd In ClassWndSearcher.SearchForWindow(Nothing, sServerName)
            If (bNotMe AndAlso hWnd = g_mReceiveForm.Handle) Then
                Continue For
            End If

            SendMessageEx(hWnd, mClassMessage.FormatedMessage)
        Next
    End Sub

    Private Sub SendMessageEx(hTargetWinHndl As IntPtr, sMessage As String)
        Dim mCopyDataStruct As New ClassWin32.CopyDataStruct()
        Try
            Dim sFormatedMessage As String = sMessage
            If (g_bEnforceEncoding) Then
                sFormatedMessage = XorDeEncrypt(sFormatedMessage, g_iEncodeKeyBytes)
            End If

            mCopyDataStruct.cbData = (sFormatedMessage.Length + 1) * 2
            mCopyDataStruct.lpData = ClassWin32.LocalAlloc(&H40, mCopyDataStruct.cbData)
            Marshal.Copy(sFormatedMessage.ToCharArray(), 0, mCopyDataStruct.lpData, sFormatedMessage.Length)
            mCopyDataStruct.dwData = New IntPtr(1)
            ClassWin32.SendMessage(hTargetWinHndl, ClassWin32.WM_COPYDATA, IntPtr.Zero, mCopyDataStruct)
        Finally
            mCopyDataStruct.Free()
        End Try
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case ClassWin32.WM_COPYDATA
                Dim mCopyDataStruct As ClassWin32.CopyDataStruct = DirectCast(Marshal.PtrToStructure(m.LParam, GetType(ClassWin32.CopyDataStruct)), ClassWin32.CopyDataStruct)
                Dim sFormatedMessage As String = Marshal.PtrToStringUni(mCopyDataStruct.lpData)

                If (g_bEnforceEncoding) Then
                    sFormatedMessage = XorDeEncrypt(sFormatedMessage, g_iEncodeKeyBytes)
                End If

                RaiseEvent OnMessageReceive(New ClassMessage(sFormatedMessage))
            Case Else
                MyBase.WndProc(m)
        End Select
    End Sub

    Private Function XorDeEncrypt(ByRef sTest As String, ByRef iKey As Byte()) As String
        Dim SB As New Text.StringBuilder
        For i As Integer = 0 To sTest.Length - 1
            SB.Append(ChrW(iKey(i Mod iKey.Length) Xor AscW(sTest.Substring(i, 1))))
        Next
        Return SB.ToString
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' Dient zur Erkennung redundanter Aufrufe.

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: verwalteten Zustand (verwaltete Objekte) entsorgen.
                If (g_mReceiveForm IsNot Nothing AndAlso Not g_mReceiveForm.IsDisposed) Then
                    g_mReceiveForm.g_bAllowDisponse = True
                    g_mReceiveForm.Dispose()
                    g_mReceiveForm = Nothing
                End If

                Me.DestroyHandle()
                Me.ReleaseHandle()
            End If

            ' TODO: nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalize() weiter unten überschreiben.
            ' TODO: große Felder auf Null setzen.
        End If
        disposedValue = True
    End Sub

    ' TODO: Finalize() nur überschreiben, wenn Dispose(disposing As Boolean) weiter oben Code zur Bereinigung nicht verwalteter Ressourcen enthält.
    'Protected Overrides Sub Finalize()
    '    ' Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(disposing As Boolean) weiter oben ein.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' Dieser Code wird von Visual Basic hinzugefügt, um das Dispose-Muster richtig zu implementieren.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(disposing As Boolean) weiter oben ein.
        Dispose(True)
        ' TODO: Auskommentierung der folgenden Zeile aufheben, wenn Finalize() oben überschrieben wird.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class