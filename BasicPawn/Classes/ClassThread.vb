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


Public Class ClassThread
    ''' <summary>
    ''' Checks if a thread is still valid
    ''' </summary>
    ''' <param name="mThread"></param>
    ''' <returns>True if valid, false otherwise</returns>
    Public Shared Function IsValid(mThread As Threading.Thread) As Boolean
        If (mThread Is Nothing) Then
            Return False
        End If

        If (Not mThread.IsAlive) Then
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' Aborts a thread
    ''' </summary>
    ''' <param name="mThread"></param>
    ''' <param name="bWait">Wait until the application thread is finished</param>
    Public Shared Sub Abort(ByRef mThread As Threading.Thread, Optional bWait As Boolean = True)
        If (Not IsValid(mThread)) Then
            Return
        End If

        mThread.Abort()

        If (bWait) Then
            mThread.Join()
            mThread = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Invokes a thread-safe delegate.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="mControl"></param>
    ''' <param name="mDelegate"></param>
    ''' <param name="mParam"></param>
    ''' <returns></returns>
    Public Shared Function Exec(Of T)(mControl As Control, mDelegate As [Delegate], ParamArray mParam() As Object) As T
        Return DirectCast(mControl.Invoke(mDelegate, mParam), T)
    End Function

    ''' <summary>
    ''' Invokes a thread-safe delegate with 'Control.InvokeRequired' check.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="mControl"></param>
    ''' <param name="mDelegate"></param>
    ''' <param name="mParam"></param>
    ''' <returns></returns>
    Public Shared Function ExecEx(Of T)(mControl As Control, mDelegate As [Delegate], ParamArray mParam() As Object) As T
        If (mControl.InvokeRequired) Then
            Return DirectCast(mControl.Invoke(mDelegate, mParam), T)
        Else
            Return DirectCast(mDelegate.DynamicInvoke(mParam), T)
        End If
    End Function

    ''' <summary>
    ''' Invokes a async thread-safe delegate.
    ''' </summary>
    ''' <param name="mControl"></param>
    ''' <param name="mDelegate"></param>
    ''' <param name="mParam"></param>
    Public Shared Function ExecAsync(mControl As Control, mDelegate As [Delegate], ParamArray mParam() As Object) As IAsyncResult
        Return mControl.BeginInvoke(mDelegate, mParam)
    End Function

    ''' <summary>
    ''' Waits for a async invoke and returns the value.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="mControl"></param>
    ''' <param name="mAsyncResult"></param>
    ''' <returns></returns>
    Public Shared Function ExecAsyncEnd(Of T)(mControl As Control, mAsyncResult As IAsyncResult) As T
        Return DirectCast(mControl.EndInvoke(mAsyncResult), T)
    End Function
End Class
