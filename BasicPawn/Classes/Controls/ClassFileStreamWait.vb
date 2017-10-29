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


Public Class ClassFileStreamWait

    ''' <summary>
    ''' Creates a IO.FileStream but waits until access to the file.
    ''' </summary>
    ''' <param name="sPath"></param>
    ''' <param name="iMode"></param>
    ''' <param name="iAccess"></param>
    ''' <param name="iTimeoutTrys"></param>
    ''' <returns></returns>
    Public Shared Function Create(sPath As String, iMode As IO.FileMode, iAccess As IO.FileAccess, Optional iTimeoutTrys As Integer = -1) As IO.FileStream
        Dim bInfinite As Boolean = (iTimeoutTrys < 0)

        While True
            Dim mSteam As IO.FileStream = Nothing

            Try
                mSteam = New IO.FileStream(sPath, iMode, iAccess)
                Return mSteam
            Catch ex As Exception
                If (mSteam IsNot Nothing) Then
                    mSteam.Dispose()
                    mSteam = Nothing
                End If

                Threading.Thread.Sleep(100)
            End Try

            If (Not bInfinite) Then
                iTimeoutTrys -= 1

                If (iTimeoutTrys < 0) Then
                    Throw New ArgumentException("File acccess timeout")
                End If
            End If
        End While

        'Should never reach
        Throw New ArgumentException("Unable to create IO.FileStream")
    End Function
End Class
