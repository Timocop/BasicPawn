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
            Const URL_SMARTPAWN As String = "https://github.com/Timocop/SmartPawn-Obfuscator/releases/download/v1.18.1/SmartPawn.1.18.1.zip"

            Dim sCurlPath As String = ClassProcess.FindEnvironment("PATH", "curl.exe")

            Dim sRootName As String = "SMARTPAWN_BUILD"
            Dim sRootBuildDir As String = IO.Path.Combine(Environment.CurrentDirectory, sRootName)

            Console.ForegroundColor = ConsoleColor.Cyan
            Console.WriteLine("BasicPawn SmartPawn Helper")
            Console.WriteLine()
            Console.WriteLine("This helper will help you download and update SmartPawn Obfuscator.")
            Console.WriteLine("It will also move all required binaries into the '..\Third Party Binaries\SmartPawn' folder.")
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")

            'Check Requirements
            If (True) Then
                Dim bAbort As Boolean = False

                If (sCurlPath Is Nothing OrElse Not IO.File.Exists(sCurlPath)) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Curl not found! Please update Windows!")
                    Console.ForegroundColor = ConsoleColor.White

                    bAbort = True
                Else
                    Console.WriteLine("Curl path:                   " & sCurlPath.ToUpper)
                End If

                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")

                If (bAbort) Then
                    Console.ReadKey()
                    Return
                End If
            End If

            Console.ForegroundColor = ConsoleColor.Yellow
            Console.WriteLine("Hit any key to download SmartPawn")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadKey()

            Console.WriteLine("Creating build directory...")
            IO.Directory.CreateDirectory(sRootBuildDir)

            Dim sSmartPawnFile As String = IO.Path.Combine(sRootBuildDir, "SmartPawn\SmartPawn.exe")

            If (Not IO.File.Exists(sSmartPawnFile)) Then
                Console.WriteLine("Fetching SmartPawn...")

                Dim sTmpFile As String = IO.Path.Combine(sRootBuildDir, "tmp.zip")

                Dim iExitCode As Integer = ClassProcess.CurlDownloadFile(URL_SMARTPAWN, sTmpFile)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to download SmartPawn!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                Console.WriteLine("Extracting SmartPawn...")

                iExitCode = ClassProcess.PowershellUnzip(sTmpFile, sRootBuildDir)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to extract SmartPawn!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                IO.File.Delete(sTmpFile)

                Console.WriteLine("Moving SmartPawn...")

                Dim sSmartPawnDir As String = IO.Path.Combine(IO.Path.GetFullPath(IO.Path.Combine(Environment.CurrentDirectory, "..\..\..")), "Third Party Binaries\SmartPawn")

                If (IO.Directory.Exists(sSmartPawnDir)) Then
                    'Deleting file will complain about access denied? Huh?
                    IO.Directory.Move(sSmartPawnDir, IO.Path.Combine(IO.Path.GetTempPath, IO.Path.GetRandomFileName))
                End If

                Dim sSmartPawnFolder As String = IO.Path.GetDirectoryName(sSmartPawnFile)

                IO.Directory.Move(sSmartPawnFolder, sSmartPawnDir)

                Console.WriteLine("Done.")
            Else
                Console.WriteLine("SmartPawn already done.")
            End If

            Console.WriteLine("Removing workspace folder...")
            If (IO.Directory.Exists(sRootBuildDir)) Then
                IO.Directory.Move(sRootBuildDir, IO.Path.Combine(IO.Path.GetTempPath, IO.Path.GetRandomFileName))
            End If

            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("Finished!")
            Console.ReadKey()
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("ERROR: " & ex.Message)
            Console.ReadKey()
        End Try
    End Sub

    Class ClassProcess
        Public Shared Sub ExecuteProgram(sPath As String, sArguments As String, ByRef r_ExitCode As Integer, ByRef r_Output As String, ByRef r_Error As String)
            ExecuteProgram(sPath, sArguments, IO.Path.GetDirectoryName(sPath), r_ExitCode, r_Output, r_Error)
        End Sub

        Public Shared Sub ExecuteProgram(sPath As String, sArguments As String, sWorkingDirectory As String, ByRef r_ExitCode As Integer, ByRef r_Output As String, ByRef r_Error As String)
            ExecuteProgram(sPath, sArguments, sWorkingDirectory, Nothing, r_ExitCode, r_Output, r_Error)
        End Sub

        Public Shared Sub ExecuteProgram(sPath As String, sArguments As String, sWorkingDirectory As String, mEnvironmentVariables As Dictionary(Of String, String), ByRef r_ExitCode As Integer, ByRef r_Output As String, ByRef r_Error As String)
            r_ExitCode = 0
            r_Output = ""

            Using i As New Process
                i.StartInfo.FileName = sPath
                i.StartInfo.Arguments = sArguments
                i.StartInfo.WorkingDirectory = sWorkingDirectory

                i.StartInfo.UseShellExecute = False
                i.StartInfo.CreateNoWindow = True
                i.StartInfo.RedirectStandardOutput = True
                i.StartInfo.RedirectStandardError = True

                If (mEnvironmentVariables IsNot Nothing) Then
                    For Each mVar In mEnvironmentVariables
                        i.StartInfo.EnvironmentVariables(mVar.Key) = mVar.Value
                    Next
                End If

                i.Start()

                r_Output = i.StandardOutput.ReadToEnd
                r_Error = i.StandardError.ReadToEnd

                i.WaitForExit()

                r_ExitCode = i.ExitCode
            End Using
        End Sub

        Public Shared Function FindEnvironment(sVariable As String, sFilename As String) As String
            For Each sPath As String In Environment.GetEnvironmentVariable(sVariable).Split(IO.Path.PathSeparator)
                Dim sFullPath As String = IO.Path.Combine(sPath, sFilename)
                If (IO.File.Exists(sFullPath)) Then
                    Return sFullPath
                End If
            Next

            Return Nothing
        End Function

        Public Shared Function CurlDownloadFile(sUrl As String, sDestination As String) As Integer
            Dim iExitCode As Integer

            ExecuteProgram("curl.exe", String.Format("""{0}"" -L -o ""{1}""", sUrl, sDestination), iExitCode, Nothing, Nothing)

            Return iExitCode
        End Function

        Public Shared Function PowershellUnzip(sFile As String, sDestination As String) As Integer
            Dim iExitCode As Integer

            ExecuteProgram("powershell.exe", "-command ""Expand-Archive -Force '" & sFile & "' '" & sDestination & "'""", iExitCode, Nothing, Nothing)

            Return iExitCode
        End Function
    End Class
End Module
