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


Imports System.Security.Cryptography
Imports System.Text

#Const CHECK_VERSION = True

Module ClassMain
    Sub Main()
        Try
            Console.ForegroundColor = ConsoleColor.Cyan
            Console.WriteLine("BasicPawn Update Release Helper")
            Console.WriteLine()
            Console.WriteLine("This helper will help you to generate new update packages for BasicPawn.")
            Console.WriteLine("Generated releases will be moved into '..\Update Depot'.")
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~")
            Console.ForegroundColor = ConsoleColor.Yellow
            Console.WriteLine("Hit any key to generate a new update release")
            Console.ForegroundColor = ConsoleColor.White
            Console.ReadKey()

            'Init and check everything
            Dim sExecutablePath As String = Reflection.Assembly.GetExecutingAssembly.Location
            Dim sCurrentDirectory As String = IO.Path.GetDirectoryName(sExecutablePath)

            Dim sThirdPartyBinPath As String = IO.Path.GetFullPath(IO.Path.Combine(sCurrentDirectory, "..\..\..\Third Party Binaries"))
            Dim sUpdateDepotPath As String = IO.Path.GetFullPath(IO.Path.Combine(sCurrentDirectory, "..\..\..\Update Depot"))
            Dim sReleasePath As String = IO.Path.GetFullPath(IO.Path.Combine(sCurrentDirectory, "..\..\..\BasicPawn\bin\Release"))
            Dim sRootPath As String = IO.Path.GetFullPath(IO.Path.Combine(sCurrentDirectory, "..\..\..\"))

            Dim sSevenZipPath As String = IO.Path.Combine(sThirdPartyBinPath, "7za.exe")
            Dim sSevenZipSfxPath As String = IO.Path.Combine(sThirdPartyBinPath, "7z_sfx")
            Dim sBasicPawnPath As String = IO.Path.GetFullPath(IO.Path.Combine(sReleasePath, "BasicPawn.exe"))
            Dim sLicensePath As String = IO.Path.GetFullPath(IO.Path.Combine(sRootPath, "GPLv3.txt"))
            Dim sThirdPartyLicensePath As String = IO.Path.GetFullPath(IO.Path.Combine(sRootPath, "Third Party Legal Notices.txt"))

            Dim sUpdateSfxPath As String = IO.Path.Combine(sUpdateDepotPath, "BasicPawnUpdateSFX.dat")
            Dim sUpdateZipPath As String = IO.Path.Combine(sUpdateDepotPath, "BasicPawn.zip")

            Dim sVersionPath As String = IO.Path.Combine(sUpdateDepotPath, "CurrentVersion.txt")
            Dim sHashPath As String = IO.Path.Combine(sUpdateDepotPath, "DataHash.txt")
            Dim sPrivateKeyPath As String = IO.Path.Combine(sUpdateDepotPath, "RSA Update Private Key.txt")
            Dim sPublicKeyPath As String = IO.Path.Combine(sUpdateDepotPath, "RSA Update Public Key.txt")

            'Check files
            If (Not IO.File.Exists(sSevenZipPath)) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("'7za.exe' not found!")
                Console.ReadKey()
                Return
            End If

            If (Not IO.File.Exists(sSevenZipSfxPath)) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("'7z_sfx' not found!")
                Console.ReadKey()
                Return
            End If

            If (Not IO.File.Exists(sBasicPawnPath)) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("'BasicPawn.exe' not found!")
                Console.ReadKey()
                Return
            End If

            If (Not IO.File.Exists(sLicensePath)) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("'GPLv3.txt' not found!")
                Console.ReadKey()
                Return
            End If

            If (Not IO.File.Exists(sThirdPartyLicensePath)) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("'Third Party Legal Notices.txt' not found!")
                Console.ReadKey()
                Return
            End If

            If (Not IO.File.Exists(sVersionPath)) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("'CurrentVersion.txt' not found!")
                Console.ReadKey()
                Return
            End If

            If (Not IO.File.Exists(sHashPath)) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("'DataHash.txt' not found!")
                Console.ReadKey()
                Return
            End If

            If (Not IO.File.Exists(sPrivateKeyPath)) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("'RSA Update Private Key.txt' not found!")
                Console.ReadKey()
                Return
            End If

            If (Not IO.File.Exists(sPublicKeyPath)) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("'RSA Update Public Key.txt' not found!")
                Console.ReadKey()
                Return
            End If

            'Check directorys
            If (Not IO.Directory.Exists(sThirdPartyBinPath)) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("'Third Party Binaries' not found!")
                Console.ReadKey()
                Return
            End If

            If (Not IO.Directory.Exists(sUpdateDepotPath)) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("'Update Depot' not found!")
                Console.ReadKey()
                Return
            End If

            If (Not IO.Directory.Exists(sReleasePath)) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("'BasicPawn\bin\Release' not found!")
                Console.ReadKey()
                Return
            End If

            'Check versions
            Dim sNewVersion As String = FileVersionInfo.GetVersionInfo(sBasicPawnPath).FileVersion
            Dim sOldVersion As String = IO.File.ReadAllText(sVersionPath)

            Console.WriteLine("New Version: " & sNewVersion)
            Console.WriteLine("Old Version: " & sOldVersion)
            Console.WriteLine("Checking versions...")

#If CHECK_VERSION Then
            If (New Version(sOldVersion) >= New Version(sNewVersion)) Then
                Console.WriteLine("No new version")
                Console.ReadKey()
                Return
            End If 
#End If

            'Copy files to tmp folder
            Dim sTmpDirectorySfx As String = IO.Path.Combine(sReleasePath, "TmpUpdateReleaseSfx")
            Dim sTmpDirectoryZip As String = IO.Path.Combine(sReleasePath, "TmpUpdateReleaseZip")
            Dim sTmpDirectoryZipSub As String = IO.Path.Combine(sTmpDirectoryZip, "BasicPawn")
            If (IO.Directory.Exists(sTmpDirectorySfx)) Then
                IO.Directory.Delete(sTmpDirectorySfx, True)
            End If
            If (IO.Directory.Exists(sTmpDirectoryZip)) Then
                IO.Directory.Delete(sTmpDirectoryZip, True)
            End If
            If (IO.Directory.Exists(sTmpDirectoryZipSub)) Then
                IO.Directory.Delete(sTmpDirectoryZipSub, True)
            End If

            Console.WriteLine(String.Format("Create sfx temp folder '{0}'...", sTmpDirectorySfx))
            IO.Directory.CreateDirectory(sTmpDirectorySfx)
            Console.WriteLine(String.Format("Create zip temp folder '{0}'...", sTmpDirectoryZip))
            IO.Directory.CreateDirectory(sTmpDirectoryZip)
            Console.WriteLine(String.Format("Create zip-sub temp folder '{0}'...", sTmpDirectoryZipSub))
            IO.Directory.CreateDirectory(sTmpDirectoryZipSub)

            Console.WriteLine("Copying files...")
            If (True) Then
                For Each sFile In IO.Directory.GetFiles(sReleasePath)
                    Select Case (IO.Path.GetExtension(sFile).ToLower)
                        Case ".exe", ".dll"
                            'Success
                        Case Else
                            Continue For
                    End Select

                    If (sFile.EndsWith("vshost.exe")) Then
                        Continue For
                    End If

                    IO.File.Copy(sFile, IO.Path.Combine(sTmpDirectorySfx, IO.Path.GetFileName(sFile)))
                Next

                IO.File.Copy(sLicensePath, IO.Path.Combine(sTmpDirectorySfx, IO.Path.GetFileName(sLicensePath)))
                IO.File.Copy(sThirdPartyLicensePath, IO.Path.Combine(sTmpDirectorySfx, IO.Path.GetFileName(sThirdPartyLicensePath)))
            End If

            If (True) Then
                For Each sFile In IO.Directory.GetFiles(sReleasePath)
                    Select Case (IO.Path.GetExtension(sFile).ToLower)
                        Case ".exe", ".dll"
                            'Success
                        Case Else
                            Continue For
                    End Select

                    If (sFile.EndsWith("vshost.exe")) Then
                        Continue For
                    End If

                    IO.File.Copy(sFile, IO.Path.Combine(sTmpDirectoryZipSub, IO.Path.GetFileName(sFile)))
                Next

                IO.File.Copy(sLicensePath, IO.Path.Combine(sTmpDirectoryZipSub, IO.Path.GetFileName(sLicensePath)))
                IO.File.Copy(sThirdPartyLicensePath, IO.Path.Combine(sTmpDirectoryZipSub, IO.Path.GetFileName(sThirdPartyLicensePath)))
            End If

            'Create SFX file
            Console.WriteLine("Creating 7-Zip SFX...")

            IO.File.Delete(sUpdateSfxPath)

            Using pProcess As New Process
                pProcess.StartInfo.FileName = sSevenZipPath
                pProcess.StartInfo.WorkingDirectory = sTmpDirectorySfx
                pProcess.StartInfo.Arguments = String.Format("a -sfx7z_sfx ""{0}""", sUpdateSfxPath)

                pProcess.Start()
                pProcess.WaitForExit()

                Console.WriteLine("Exit code: " & pProcess.ExitCode)

                If (pProcess.ExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Creating 7-Zip SFX failed")
                    Console.ReadKey()
                    Return
                End If

                If (Not IO.File.Exists(sUpdateSfxPath)) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Update SFX not found")
                    Console.ReadKey()
                    Return
                End If
            End Using

            'Create ZIP file
            Console.WriteLine("Creating 7-Zip ZIP...")

            IO.File.Delete(sUpdateZipPath)

            Using pProcess As New Process
                pProcess.StartInfo.FileName = sSevenZipPath
                pProcess.StartInfo.WorkingDirectory = sTmpDirectoryZip
                pProcess.StartInfo.Arguments = String.Format("a -tzip ""{0}""", sUpdateZipPath)

                pProcess.Start()
                pProcess.WaitForExit()

                Console.WriteLine("Exit code: " & pProcess.ExitCode)

                If (pProcess.ExitCode <> 0) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Creating 7-Zip ZIP failed")
                    Console.ReadKey()
                    Return
                End If

                If (Not IO.File.Exists(sUpdateZipPath)) Then
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("ZIP not found")
                    Console.ReadKey()
                    Return
                End If
            End Using

            Console.WriteLine("Removing temp folders...")
            IO.Directory.Delete(sTmpDirectorySfx, True)
            IO.Directory.Delete(sTmpDirectoryZip, True)

            'Setup SHA256 and RSA 
            Console.WriteLine("Creating SHA256 and RSA...")

            Dim sPrivateKey As String = IO.File.ReadAllText(sPrivateKeyPath)
            Dim sPublicKey As String = IO.File.ReadAllText(sPublicKeyPath)

            Dim sSfxHash As String = ClassHash.HashSHA256File(sUpdateSfxPath).ToLower
            Console.WriteLine(String.Format("Update SFX Hash: {0}" , sSfxHash))

            Dim sEnCryptSfxHash As String = ClassRSA.Encrypt(sSfxHash, sPrivateKey)
            Dim sDeCryptSfxHash As String = ClassRSA.Decrypt(sEnCryptSfxHash, sPublicKey)
            If (sSfxHash.ToLower <> sDeCryptSfxHash.ToLower) Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine("Hashes do not match!")
                Console.ReadKey()
                Return
            End If

            'Write version and encrypted hash to disk 
            Console.WriteLine("Writing hash and version files...")
            IO.File.WriteAllText(sHashPath, sEnCryptSfxHash)
            IO.File.WriteAllText(sVersionPath, sNewVersion)

            Console.ForegroundColor = ConsoleColor.Green
            Console.WriteLine("Finished!")
            Console.ReadKey()
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine("ERROR: " & ex.Message)
            Console.ReadKey()
        End Try
    End Sub

    Class ClassRSA
        Public Shared Sub GenerateKeys(ByRef r_sPrivateKeyXML As String, ByRef r_sPublicKeyXML As String, Optional iKeySize As Integer = 2048)
            Using mRSA As New RSACryptoServiceProvider(iKeySize)
                Try
                    r_sPrivateKeyXML = mRSA.ToXmlString(True)
                    r_sPublicKeyXML = mRSA.ToXmlString(False)
                Finally
                    mRSA.PersistKeyInCsp = False
                End Try
            End Using
        End Sub

        Public Shared Function Encrypt(sText As String, sKeyXML As String) As String
            Dim iData As Byte() = New UTF8Encoding(False).GetBytes(sText)

            Using mRSA As New RSACryptoServiceProvider()
                Try
                    mRSA.FromXmlString(sKeyXML)

                    Dim iEncryptedData As Byte()

                    If (mRSA.PublicOnly) Then
                        Dim mRSAParm = mRSA.ExportParameters(False)

                        Dim bigInteger As BigInteger = New BigInteger(iData)
                        Dim bigInteger2 As BigInteger = bigInteger.modPow(New BigInteger(mRSAParm.Exponent), New BigInteger(mRSAParm.Modulus))

                        iEncryptedData = bigInteger2.getBytes
                    Else
                        Dim mRSAParm = mRSA.ExportParameters(True)

                        Dim bigInteger As BigInteger = New BigInteger(iData)
                        Dim bigInteger2 As BigInteger = bigInteger.modPow(New BigInteger(mRSAParm.D), New BigInteger(mRSAParm.Modulus))

                        iEncryptedData = bigInteger2.getBytes
                    End If

                    Return Convert.ToBase64String(iEncryptedData)
                Finally
                    mRSA.PersistKeyInCsp = False
                End Try
            End Using
        End Function

        Public Shared Function Decrypt(sTextBase64 As String, sKeyXML As String) As String
            Dim iData As Byte() = Convert.FromBase64String(sTextBase64)

            Using mRSA As New RSACryptoServiceProvider()
                Try
                    mRSA.FromXmlString(sKeyXML)

                    Dim iDecryptedData As Byte()

                    If (mRSA.PublicOnly) Then
                        Dim mRSAParm = mRSA.ExportParameters(False)

                        Dim bigInteger As BigInteger = New BigInteger(iData)
                        Dim bigInteger2 As BigInteger = bigInteger.modPow(New BigInteger(mRSAParm.Exponent), New BigInteger(mRSAParm.Modulus))

                        iDecryptedData = bigInteger2.getBytes
                    Else
                        Dim mRSAParm = mRSA.ExportParameters(True)

                        Dim bigInteger As BigInteger = New BigInteger(iData)
                        Dim bigInteger2 As BigInteger = bigInteger.modPow(New BigInteger(mRSAParm.D), New BigInteger(mRSAParm.Modulus))

                        iDecryptedData = bigInteger2.getBytes
                    End If

                    Return New UTF8Encoding(False).GetString(iDecryptedData)
                Finally
                    mRSA.PersistKeyInCsp = False
                End Try
            End Using
        End Function
    End Class

    Class ClassHash
        Public Shared Function HashSHA256File(sFile As String) As String
            Dim iHash As Byte()
            With New StringBuilder
                Dim sTemp As String = ""

                Using mHash As New SHA256Managed()
                    Using mFS As New IO.FileStream(sFile, IO.FileMode.Open, IO.FileAccess.Read)
                        mHash.ComputeHash(mFS)
                    End Using

                    iHash = mHash.Hash

                    For ii As Integer = 0 To iHash.Length - 1
                        sTemp = Convert.ToString(iHash(ii), 16)
                        If (sTemp.Length = 1) Then
                            sTemp = "0" & sTemp
                        End If
                        .Append(sTemp)
                    Next

                    mHash.Clear()
                End Using

                Return .ToString
            End With
        End Function
    End Class
End Module
