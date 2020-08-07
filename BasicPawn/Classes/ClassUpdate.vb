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


Imports System.Net
Imports System.Security.Authentication
Imports System.Text.RegularExpressions

Public Class ClassUpdate
    Public Shared ReadOnly g_sRSAPublicKeyXML As String = "<RSAKeyValue><Modulus>vhkaxwuw08ufJcXdcCGvXjeF/UTpQzIvfjo+DqUDT6OyrCB5u86t536wSDJawFeMPR9JicrY7eiT8Jy9O7zsu0y3+aaR7nBNw9h7DIGFLsgASKHR5PD2uW1dh3ZilkLCk+eKwEER91MyYm5fEciudrwZbsHRhjsMsHRvuyu231U=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"

    Class STRUC_UPDATE_LOCATIONS
        Public sLocationInfo As String
        Public sVersionUrl As String
        Public sDataHashUrl As String
        Public sDataUrl As String
        Public sUserAgent As String

        Public Sub New(_LocationInfo As String, _VersionUrl As String, _DataHashUrl As String, _DataUrl As String, _UserAgent As String)
            sLocationInfo = _LocationInfo
            sVersionUrl = _VersionUrl
            sDataHashUrl = _DataHashUrl
            sDataUrl = _DataUrl
            sUserAgent = _UserAgent
        End Sub
    End Class

    Public Shared g_mUpdateLocations As STRUC_UPDATE_LOCATIONS() = {
        New STRUC_UPDATE_LOCATIONS("github.com",
                                   "https://raw.githubusercontent.com/Timocop/BasicPawn/master/Update%20Depot/CurrentVersion.txt",
                                   "https://raw.githubusercontent.com/Timocop/BasicPawn/master/Update%20Depot/DataHash.txt",
                                   "https://raw.githubusercontent.com/Timocop/BasicPawn/master/Update%20Depot/BasicPawnUpdateSFX.dat",
                                   String.Format("BasicPawn/{0} (compatible; Windows NT)", Application.ProductVersion)),
        New STRUC_UPDATE_LOCATIONS("getbasicpawn.spdns.org",
                                   "http://getbasicpawn.spdns.org/basicpawn_update/CurrentVersion.txt",
                                   "http://getbasicpawn.spdns.org/basicpawn_update/DataHash.txt",
                                   "http://getbasicpawn.spdns.org/basicpawn_update/BasicPawnUpdateSFX.dat",
                                   String.Format("BasicPawn/{0} (compatible; Windows NT)", Application.ProductVersion))
    }

    Public Shared Sub InstallUpdate()
#If Not DEBUG Then
        If (Not CheckUpdateAvailable(Nothing)) Then
            Return
        End If
#End If

#If DEBUG Then
        IO.Directory.CreateDirectory(IO.Path.Combine(Application.StartupPath, "UpdateTest"))
        Dim sDataPath As String = IO.Path.Combine(Application.StartupPath, "UpdateTest\BasicPawnUpdateSFX.exe")
#Else
        Dim sDataPath As String = IO.Path.Combine(Application.StartupPath, "BasicPawnUpdateSFX.exe")
#End If
        Dim sHashEncrypted As String = ""
        Dim sHash As String = ""
        Dim sDataHash As String = ""

        IO.File.Delete(sDataPath)

        Dim bSuccess As Boolean = False

        For Each mItem In g_mUpdateLocations
            Try
                'Test if server files are available
                Using mWC As New ClassWebClientEx
                    If (Not String.IsNullOrEmpty(mItem.sUserAgent)) Then
                        mWC.Headers("User-Agent") = mItem.sUserAgent
                    End If

                    If (True) Then
                        sHashEncrypted = mWC.DownloadString(mItem.sVersionUrl)

                        If (String.IsNullOrEmpty(sHashEncrypted)) Then
                            Throw New ArgumentException("Invalid version")
                        End If
                    End If

                    If (True) Then
                        sHashEncrypted = mWC.DownloadString(mItem.sDataHashUrl)

                        If (String.IsNullOrEmpty(sHashEncrypted)) Then
                            Throw New ArgumentException("Invalid hash")
                        End If

                        sHash = ClassTools.ClassCrypto.ClassRSA.Decrypt(sHashEncrypted, g_sRSAPublicKeyXML)
                    End If

                    If (True) Then
                        IO.File.Delete(sDataPath)

                        mWC.DownloadFile(mItem.sDataUrl, sDataPath)

                        If (Not IO.File.Exists(sDataPath)) Then
                            Throw New ArgumentException("Files does not exist")
                        End If

                        sDataHash = ClassTools.ClassCrypto.ClassHash.SHA256FileHash(sDataPath)
                    End If
                End Using

                If (sHash.ToLower <> sDataHash.ToLower) Then
                    Throw New ArgumentException("Hash does not match")
                End If

                bSuccess = True
                Exit For
            Catch ex As Exception
            End Try
        Next

        If (Not bSuccess) Then
            Throw New ArgumentException("Unable to find update files")
        End If

#If Not DEBUG Then
        For Each pProcess As Process In Process.GetProcessesByName(IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath))
            Try
                If (pProcess.HasExited OrElse pProcess.Id = Process.GetCurrentProcess.Id) Then
                    Continue For
                End If

                If (IO.Path.GetFullPath(pProcess.MainModule.FileName).ToLower <> IO.Path.GetFullPath(Application.ExecutablePath).ToLower) Then
                    Continue For
                End If

                pProcess.Kill()
                pProcess.WaitForExit()
            Catch ex As Exception
            End Try
        Next
#End If

        Dim sBatchFile As String = IO.Path.Combine(Application.StartupPath, "InstallUpdate.bat")

        Dim sUpdateBatch As New Text.StringBuilder
        sUpdateBatch.AppendLine("@echo off")
        sUpdateBatch.AppendFormat("start /w """" ""{0}"" -y", sDataPath).AppendLine() 'Run 7zip SFX and wait
        sUpdateBatch.AppendFormat("start """" ""{0}""", Application.ExecutablePath).AppendLine() 'Run BasicPawn but do not wait
        sUpdateBatch.AppendFormat("del ""{0}""", sBatchFile).AppendLine() 'KMS 

        IO.File.WriteAllText(sBatchFile, sUpdateBatch.ToString)

        Using i As New Process
            i.StartInfo.FileName = sBatchFile
            i.StartInfo.WorkingDirectory = IO.Path.GetDirectoryName(sBatchFile)

            i.StartInfo.UseShellExecute = False
            i.StartInfo.CreateNoWindow = True

            i.Start()
        End Using

#If Not DEBUG Then
        Process.GetCurrentProcess.Kill()
        End
#End If
    End Sub

    Public Shared Function CheckUpdateAvailable(ByRef r_sLocationInfo As String) As Boolean
        Dim sNextVersion = ""
        Dim sCurrentVersion = ""
        Return CheckUpdateAvailable(r_sLocationInfo, sNextVersion, sCurrentVersion)
    End Function

    Public Shared Function CheckUpdateAvailable(ByRef r_sLocationInfo As String, ByRef r_sNextVersion As String, ByRef r_sCurrentVersion As String) As Boolean
        Dim sNextVersion As String = Regex.Match(GetNextVersion(r_sLocationInfo), "[0-9\.]+").Value
        Dim sCurrentVersion As String = Regex.Match(GetCurrentVerison(), "[0-9\.]+").Value

        r_sNextVersion = sNextVersion
        r_sCurrentVersion = sCurrentVersion

        Return (New Version(sNextVersion) > New Version(sCurrentVersion))
    End Function

    Public Shared Function GetCurrentVerison() As String
        Return Application.ProductVersion
    End Function

    Public Shared Function GetNextVersion(ByRef r_sLocationInfo As String) As String
        r_sLocationInfo = Nothing

        SetTLS12()

        Dim sNextVersion As String = Nothing
        Dim sLocationInfo As String = Nothing

        For Each mItem In g_mUpdateLocations
            Try
                Using mWC As New ClassWebClientEx
                    If (Not String.IsNullOrEmpty(mItem.sUserAgent)) Then
                        mWC.Headers("User-Agent") = mItem.sUserAgent
                    End If

                    Dim sVersion = mWC.DownloadString(mItem.sVersionUrl)
                    If (String.IsNullOrEmpty(sVersion)) Then
                        Continue For
                    End If

                    If (Not String.IsNullOrEmpty(sNextVersion)) Then
                        If (New Version(sNextVersion) > New Version(sVersion)) Then
                            Continue For
                        End If
                    End If

                    sNextVersion = sVersion
                    sLocationInfo = mItem.sLocationInfo
                End Using
            Catch ex As Exception
            End Try
        Next

        If (String.IsNullOrEmpty(sNextVersion)) Then
            Throw New ArgumentException("Unable to find update files")
        End If

        r_sLocationInfo = sLocationInfo
        Return sNextVersion
    End Function

    Private Shared Sub SetTLS12()
        'https://stackoverflow.com/questions/43240611/net-framework-3-5-and-tls-1-2
        Const _Tls12 As SslProtocols = DirectCast(&HC00, SslProtocols)
        Const Tls12 As SecurityProtocolType = DirectCast(_Tls12, SecurityProtocolType)

        ServicePointManager.SecurityProtocol = Tls12
    End Sub
End Class
