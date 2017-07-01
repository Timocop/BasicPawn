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


Imports System.Text.RegularExpressions

Public Class ClassUpdate
    Private Shared g_sRSAPublicKeyXML As String = "<RSAKeyValue><Modulus>vhkaxwuw08ufJcXdcCGvXjeF/UTpQzIvfjo+DqUDT6OyrCB5u86t536wSDJawFeMPR9JicrY7eiT8Jy9O7zsu0y3+aaR7nBNw9h7DIGFLsgASKHR5PD2uW1dh3ZilkLCk+eKwEER91MyYm5fEciudrwZbsHRhjsMsHRvuyu231U=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"

    Private Shared g_sGithubVersionURL As String = ""
    Private Shared g_sGithubHashURL As String = ""
    Private Shared g_sGithubDataURL As String = ""

    Public Shared Sub InstallUpdate()
        If (String.IsNullOrEmpty(g_sGithubHashURL)) Then
            Throw New ArgumentException("Hash URL empty")
        End If

        If (String.IsNullOrEmpty(g_sGithubDataURL)) Then
            Throw New ArgumentException("Data URL empty")
        End If

        If (Not CheckUpdateAvailable()) Then
            Return
        End If

        Dim sDataPath As String = IO.Path.Combine(Application.StartupPath, "BasicPawnUpdateSFX.exe")
        Dim sHashEncrypted As String = ""
        Dim sHash As String = ""
        Dim sDataHash As String = ""

        IO.File.Delete(sDataPath)

        Using mWC As New ClassWebClientEx
            sHashEncrypted = mWC.DownloadString(g_sGithubHashURL)

            If (String.IsNullOrEmpty(sHashEncrypted)) Then
                Throw New ArgumentException("Invalid hash")
            End If

            sHash = ClassTools.ClassCrypto.ClassRSA.Decrypt(sHashEncrypted, g_sRSAPublicKeyXML)
        End Using

        Using mWC As New ClassWebClientEx
            mWC.DownloadFile(g_sGithubDataURL, sDataPath)

            If (Not IO.File.Exists(sDataPath)) Then
                Throw New ArgumentException("Files does not exist")
            End If

            sDataHash = ClassTools.ClassCrypto.ClassHash.HashSHA256File(sDataPath)
        End Using

        If (sHash.ToLower <> sDataHash.ToLower) Then
            Throw New ArgumentException("Hash does not match")
        End If

        For Each pProcess As Process In Process.GetProcessesByName("BasicPawn.exe")
            Try
                If (pProcess.HasExited OrElse pProcess.Id = Process.GetCurrentProcess.Id) Then
                    Continue For
                End If

                If (Not IO.Path.GetFullPath(pProcess.MainModule.FileName).ToLower.StartsWith(Application.StartupPath.ToLower)) Then
                    Continue For
                End If

                pProcess.Kill()
                pProcess.WaitForExit()
            Catch ex As Exception
            End Try
        Next

        Process.Start(sDataPath)
        Process.GetCurrentProcess.Kill()
        End
    End Sub

    Public Shared Function CheckUpdateAvailable() As Boolean
        Dim sNextVersion As String = Regex.Match(GetNextVersion(), "[0-9\.]").Value
        Dim sCurrentVersion As String = Regex.Match(GetCurrentVerison(), "[0-9\.]").Value

        Return (New Version(sNextVersion) > New Version(sCurrentVersion))
    End Function

    Public Shared Function GetCurrentVerison() As String
        Return Application.ProductVersion
    End Function

    Public Shared Function GetNextVersion() As String
        If (String.IsNullOrEmpty(g_sGithubVersionURL)) Then
            Throw New ArgumentException("Version URL empty")
        End If

        Using mWC As New ClassWebClientEx
            Return mWC.DownloadString(g_sGithubVersionURL)
        End Using
    End Function
End Class
