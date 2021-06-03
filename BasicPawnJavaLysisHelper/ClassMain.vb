
Imports System.Text.RegularExpressions

Module ClassMain
    Sub Main()
        Try
            Dim sJavaPath As String = ClassProcess.FindEnvironment("PATH", "java.exe")
            Dim sJavaCPath As String = ClassProcess.FindEnvironment("PATH", "javac.exe")
            Dim sGitPath As String = ClassProcess.FindEnvironment("PATH", "git.exe")
            Dim sCurlPath As String = ClassProcess.FindEnvironment("PATH", "curl.exe")
            Dim sJavaHome As String = Environment.GetEnvironmentVariable("JAVA_HOME")
            Dim sWindowsKitX64 As String = Nothing

            Dim sRootName As String = "IKVM_LYSIS_BUILD"
            Dim sRootBuildDir As String = IO.Path.Combine(Environment.CurrentDirectory.Substring(0, 3), sRootName)

            Console.ForegroundColor = ConsoleColor.Cyan
            Console.WriteLine("BasicPawn Java-Lysis Converter Helper")
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
            Console.WriteLine("Java available:              " & If(sJavaPath IsNot Nothing, "TRUE", "FALSE"))
            Console.WriteLine("Java Compiler available:     " & If(sJavaCPath IsNot Nothing, "TRUE", "FALSE"))
            Console.WriteLine("Git available:               " & If(sGitPath IsNot Nothing, "TRUE", "FALSE"))
            Console.WriteLine("Curl available:              " & If(sCurlPath IsNot Nothing, "TRUE", "FALSE"))
            Console.WriteLine("Java-Home avaiable:          " & If(sJavaHome IsNot Nothing, "TRUE", "FALSE"))

            'Check kWindows Kit 10
            If (True) Then
                Dim RegKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\WOW6432Node\Microsoft\Windows Kits\Installed Roots", False)
                If (RegKey IsNot Nothing) Then
                    Dim sPath As String = CStr(RegKey.GetValue("KitsRoot10", Nothing))

                    If (sPath IsNot Nothing AndAlso IO.Directory.Exists(sPath)) Then
                        sWindowsKitX64 = IO.Path.Combine(sPath, "bin\x64")
                    End If
                End If
            End If

            Console.WriteLine("Windows Kit 10 available:    " & If(sWindowsKitX64 IsNot Nothing, "TRUE", "FALSE"))
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")

            'Check Requirements
            If (True) Then
                Dim bAbort As Boolean = False
                If (sJavaPath Is Nothing) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Java 8 missing! Please install Java 8 (AdpotOpenJDK recommended)!")
                    Console.ForegroundColor = ConsoleColor.White

                    bAbort = True
                End If

                If (sJavaCPath Is Nothing) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Java Development Kit missing! Please install Java Development Kit (AdpotOpenJDK recommended)!")
                    Console.ForegroundColor = ConsoleColor.White

                    bAbort = True
                End If

                If (sGitPath Is Nothing) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Git missing! Install Git (from git-scm)")
                    Console.ForegroundColor = ConsoleColor.White

                    bAbort = True
                End If

                If (sCurlPath Is Nothing) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Curl not found! Please update Windows! (Windows 10 required!)")
                    Console.ForegroundColor = ConsoleColor.White

                    bAbort = True
                End If

                If (sJavaHome Is Nothing) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Java-Home not found! Please setup JAVA_HOME environment variable!")
                    Console.ForegroundColor = ConsoleColor.White

                    bAbort = True
                End If

                If (bAbort) Then
                    Console.ReadKey()
                    Return
                End If
            End If

            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine()
            Console.WriteLine("If custom backup '{0}' folder is available, please place it in '{1}' (e.g. '{2}') to speed up the process!", sRootName, Environment.CurrentDirectory.Substring(0, 3), sRootBuildDir)
            Console.WriteLine("If the build process is done, please backup '{0}' to avoid re-downloading and re-building files.", sRootBuildDir)
            Console.WriteLine()
            Console.ForegroundColor = ConsoleColor.White

            Console.ForegroundColor = ConsoleColor.Yellow
            Console.WriteLine("Working directory is: '{0}'", sRootBuildDir)
            Console.WriteLine("Hit any key to generate java-lysis for .NET")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadKey()

            'Check Java Version
            If (True) Then
                Console.WriteLine("Checking Java version...")

                Dim iExitCode As Integer = 0
                Dim sOutput As String = ""

                'https://stackoverflow.com/questions/10257720/executing-java-version-programmatically-places-the-output-in-standarderror-in
                ClassProcess.ExecuteProgram(sJavaPath, "-fullversion", iExitCode, Nothing, sOutput)

                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to get version! Failed with exitcode: " & iExitCode)
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                Dim mMatch As Match = Regex.Match(sOutput, "\""(?<Major>[0-9a-z_-]+)\.(?<Minor>[0-9a-z_-]+)\.(?<Build>[0-9a-z_-]+)\""\s*$", RegexOptions.Multiline)
                If (Not mMatch.Success) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to get version!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                If (mMatch.Groups("Major").Value <> "1" OrElse mMatch.Groups("Minor").Value <> "8") Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unsupported version! Java 8 required! Got: " & mMatch.Value)
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If
            End If

            Console.WriteLine("Setting JAVA_HOME variable...")
            Environment.SetEnvironmentVariable("JAVA1.8_HOME", sJavaHome)

            Console.WriteLine("Setting PATH variables...")
            Dim sPaths As New List(Of String)(Environment.GetEnvironmentVariable("PATH").Split(IO.Path.PathSeparator))
            sPaths.Add(sJavaHome)
            sPaths.Add(sWindowsKitX64)
            Environment.SetEnvironmentVariable("PATH", String.Join(";"c, sPaths.ToArray))

            Console.WriteLine("Validating Windows Kit 10...")
            If (Not IO.File.Exists(IO.Path.Combine(sWindowsKitX64, "rc.exe"))) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("Validation failed! Unable to find Microsoft Resource Compiler!")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadKey()
                Return
            End If

            Dim sNantDir As String = IO.Path.Combine(sRootBuildDir, "nant-0.92")
            Dim sIkvm8Dir As String = IO.Path.Combine(sRootBuildDir, "ikvm8")
            Dim sOpenJdkDir As String = IO.Path.Combine(sRootBuildDir, "openjdk-8u45-b14")
            Dim sSZLDir As String = IO.Path.Combine(sRootBuildDir, "szl")
            Dim sLysisDir As String = IO.Path.Combine(sRootBuildDir, "lysis-java")

            Console.WriteLine("Creating build directory...")
            IO.Directory.CreateDirectory(sRootBuildDir)

            If (Not IO.Directory.Exists(sNantDir)) Then
                Console.WriteLine("Fetching NAnt...")

                Dim sTmpFile As String = IO.Path.Combine(sRootBuildDir, "tmp.zip")

                Dim iExitCode As Integer = ClassProcess.CurlDownloadFile("https://deac-ams.dl.sourceforge.net/project/nant/nant/0.92/nant-0.92-bin.zip", sTmpFile)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to download NAnt!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                Console.WriteLine("Extracting NAnt...")

                iExitCode = ClassProcess.PowershellUnzip(sTmpFile, sRootBuildDir)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to extract NAnt!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                IO.File.Delete(sTmpFile)

                Console.WriteLine("Done.")
            End If

            If (Not IO.Directory.Exists(sIkvm8Dir)) Then
                Console.WriteLine("Fetching windward IKVM8...")

                Dim iExitCode As Integer
                ClassProcess.ExecuteProgram(sGitPath, "clone https://github.com/windward-studios/ikvm8.git", sRootBuildDir, iExitCode, Nothing, Nothing)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to clone repository!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                Console.WriteLine("Done.")
            End If

            If (Not IO.Directory.Exists(sOpenJdkDir)) Then
                Console.WriteLine("Fetching OpenJDK (This may take a while)...")

                Dim sTmpFile As String = IO.Path.Combine(sRootBuildDir, "tmp.zip")

                Dim iExitCode As Integer = ClassProcess.CurlDownloadFile("http://www.frijters.net/openjdk-8u45-b14-stripped.zip", sTmpFile)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to download OpenJDK!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                Console.WriteLine("Extracting OpenJDK...")

                iExitCode = ClassProcess.PowershellUnzip(sTmpFile, sRootBuildDir)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to extract OpenJDK!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                IO.File.Delete(sTmpFile)

                Console.WriteLine("Done.")
            End If

            If (Not IO.Directory.Exists(sSZLDir)) Then
                Console.WriteLine("Fetching SharpZipLib...")

                Dim sTmpFile As String = IO.Path.Combine(sRootBuildDir, "tmp.zip")

                Dim iExitCode As Integer = ClassProcess.CurlDownloadFile("https://github.com/icsharpcode/SharpZipLib/releases/download/v1.3.2/SharpZipLib.1.3.2.nupkg", sTmpFile)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to download SharpZipLib!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                Console.WriteLine("Extracting SharpZipLib...")

                iExitCode = ClassProcess.PowershellUnzip(sTmpFile, sSZLDir)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to extract SharpZipLib!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                IO.File.Delete(sTmpFile)

                Console.WriteLine("Moving Binaries...")
                For Each sFile As String In IO.Directory.GetFiles(IO.Path.Combine(sSZLDir, "lib\net45"), "*.dll")
                    Dim sFilename As String = IO.Path.GetFileName(sFile)

                    IO.File.Copy(sFile, IO.Path.Combine(sIkvm8Dir, "bin\" & sFilename))
                Next

                Console.WriteLine("Done.")
            End If

            If (Not IO.File.Exists(IO.Path.Combine(sIkvm8Dir, "bin\ICSharpCode.SharpZipLib.dll"))) Then
                Console.WriteLine("Moving SharpZipLib Binaries...")

                For Each sFile As String In IO.Directory.GetFiles(IO.Path.Combine(sSZLDir, "lib\net45"), "*.dll")
                    Dim sFilename As String = IO.Path.GetFileName(sFile)

                    IO.File.Copy(sFile, IO.Path.Combine(sIkvm8Dir, "bin\" & sFilename))
                Next

                Console.WriteLine("Done.")
            End If


            If (Not IO.File.Exists(IO.Path.Combine(sIkvm8Dir, "bin\IKVM.Runtime.JNI.dll"))) Then
                Console.WriteLine("Building IKVM Runtime (This may take a while)...")

                Dim iExitCode As Integer
                Dim sOutput As String = ""
                ClassProcess.ExecuteProgram(IO.Path.Combine(sNantDir, "bin\NAnt.exe"), Nothing, sIkvm8Dir, iExitCode, sOutput, Nothing)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(sOutput)
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                Console.WriteLine("Done.")
            End If

            Dim bRebuildLysis As Boolean = False

            If (Not IO.Directory.Exists(sLysisDir)) Then
                bRebuildLysis = True
            Else
                Console.ForegroundColor = ConsoleColor.Yellow
                Console.WriteLine("Java-Lysis is already available. Update and build? [y/n]")
                Console.ForegroundColor = ConsoleColor.White

                bRebuildLysis = (Console.ReadKey.KeyChar = "y"c)
                Console.WriteLine()
            End If

            If (bRebuildLysis) Then
                Console.WriteLine("Fetching Lysis-Java...")

                If (IO.Directory.Exists(sLysisDir)) Then
                    'Deleting file will complain about access denied? Huh?
                    IO.Directory.Move(sLysisDir, IO.Path.Combine(IO.Path.GetTempPath, IO.Path.GetRandomFileName))
                End If

                Dim iExitCode As Integer
                ClassProcess.ExecuteProgram(sGitPath, "clone https://github.com/DosMike/lysis-java.git", sRootBuildDir, iExitCode, Nothing, Nothing)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to clone repository!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                Console.WriteLine("Building Lysis-Java...")

                Dim sOutput As String = ""
                ClassProcess.ExecuteProgram(IO.Path.Combine(sLysisDir, "gradlew.bat"), "jar", sLysisDir, iExitCode, sOutput, Nothing)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(sOutput)
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                Dim sJarFiles As String() = IO.Directory.GetFiles(IO.Path.Combine(sLysisDir, "build\libs"), "lysis-java*.jar")
                If (sJarFiles.Count = 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to find lysis-java jar file!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                Dim sJarFile As String = sJarFiles(0)

                Console.WriteLine("Converting Lysis-Java to .NET runtime...")

                ClassProcess.ExecuteProgram(IO.Path.Combine(sIkvm8Dir, "bin\ikvmc.exe"), String.Format("-out:lysis-java.dll -assembly:lysis-java -target:library ""{0}""", sJarFile), IO.Path.Combine(sIkvm8Dir, "bin"), iExitCode, sOutput, Nothing)
                If (iExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Unable to clone repository!")
                    Console.ForegroundColor = ConsoleColor.White
                    Console.ReadKey()
                    Return
                End If

                Console.WriteLine("Done.")
            End If

            If (IO.File.Exists(IO.Path.Combine(sIkvm8Dir, "bin\lysis-java.dll"))) Then
                Console.WriteLine("Moving Lysis-Java .NET Binaries...")

                Dim sLysisJavaDir As String = IO.Path.Combine(IO.Path.GetFullPath(IO.Path.Combine(Environment.CurrentDirectory, "..\..\..")), "Third Party Binaries\Lysis-Java IKVM")

                IO.Directory.CreateDirectory(sLysisJavaDir)

                For Each sFile As String In IO.Directory.GetFiles(IO.Path.Combine(sIkvm8Dir, "bin"), "*.dll")
                    Dim sFilename As String = IO.Path.GetFileName(sFile)

                    IO.File.Copy(sFile, IO.Path.Combine(sLysisJavaDir, sFilename), True)
                Next

                Console.WriteLine("Done.")
            Else
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("Unable to find lysis-java.dll!")
                Console.ForegroundColor = ConsoleColor.White
                Console.ReadKey()
                Return
            End If

            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("Finished!")
            Console.WriteLine("Backup folder '{0}' to avoid re-downloading and re-building files.", sRootBuildDir)
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
