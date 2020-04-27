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


Public Class ClassFileStreamWait

    ''' <summary>
    ''' Creates a IO.FileStream but waits until access to the file.
    ''' </summary>
    ''' <param name="sPath"></param>
    ''' <param name="iMode"></param>
    ''' <param name="iAccess"></param>
    ''' <param name="iTimeout"></param>
    ''' <returns></returns>
    Public Shared Function Create(sPath As String, iMode As IO.FileMode, iAccess As IO.FileAccess, Optional iTimeout As Integer = -1) As IO.FileStream
        Dim mTimeoutWatcher As New Stopwatch
        mTimeoutWatcher.Start()

        While True
            Dim mSteam As IO.FileStream = Nothing

            Try
                mSteam = New IO.FileStream(sPath, iMode, iAccess)
                Return mSteam
            Catch ex As IO.DriveNotFoundException
                Throw
            Catch ex As IO.FileNotFoundException
                Throw
            Catch ex As IO.DirectoryNotFoundException
                Throw
            Catch ex As IO.PathTooLongException
                Throw
            Catch ex As Exception
                If (mSteam IsNot Nothing) Then
                    mSteam.Dispose()
                    mSteam = Nothing
                End If

                If (iTimeout > -1 AndAlso mTimeoutWatcher.ElapsedMilliseconds > iTimeout) Then
                    Throw
                End If

                Threading.Thread.Sleep(100)
            End Try
        End While

        'Should never reach
        Throw New ArgumentException("Unable to create IO.FileStream")
    End Function
End Class
