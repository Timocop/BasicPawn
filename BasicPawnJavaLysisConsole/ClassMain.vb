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


Module ClassMain
    Sub Main()
        Try
            Dim sCmdArgs As String() = Environment.GetCommandLineArgs

            Dim sArgs As New List(Of String)
            For i = 1 To sCmdArgs.Length - 1
                sArgs.Add(sCmdArgs(i))
            Next

            lysis.Lysis.main(sArgs.ToArray)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Environment.Exit(-1)
        End Try
    End Sub
End Module
