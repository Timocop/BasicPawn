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


Imports System.Security.Cryptography
Imports System.Security.Principal

Public Class ClassSecureStorage
    Private Shared g_iEntropy As Byte() = {110, 156, 222, 18, 129, 105, 11, 43, 235, 7}
    Private g_sName As String
    Private g_iBytes As Byte()

    Public ReadOnly Property m_StoragePath(sName As String) As String
        Get
            Return IO.Path.Combine(Application.StartupPath, String.Format("storage\{0}\{1}.dat", WindowsIdentity.GetCurrent.User.Value, sName))
        End Get
    End Property

    Public ReadOnly Property m_Name As String
        Get
            Return g_sName
        End Get
    End Property

    Public Property m_Bytes As Byte()
        Get
            Return g_iBytes
        End Get
        Set(value As Byte())
            g_iBytes = value
        End Set
    End Property

    Public Property m_String(i As Text.Encoding) As String
        Get
            Return i.GetString(m_Bytes)
        End Get
        Set(value As String)
            g_iBytes = i.GetBytes(value)
        End Set
    End Property

    Public Sub New(sName As String)
        g_sName = sName
    End Sub

    Public Function Open() As Boolean
        If (Not IO.File.Exists(m_StoragePath(m_Name))) Then
            Return False
        End If

        Dim iUnprotectedBytes As Byte() = IO.File.ReadAllBytes(m_StoragePath(m_Name))
        g_iBytes = Decrypt(iUnprotectedBytes)

        Return True
    End Function

    Public Sub Close()
        Dim iProtectedBytes As Byte() = Encrypt(g_iBytes)
        Dim sDirectory As String = IO.Path.GetDirectoryName(m_StoragePath(m_Name))

        If (Not IO.Directory.Exists(sDirectory)) Then
            IO.Directory.CreateDirectory(sDirectory)
        End If

        IO.File.WriteAllBytes(m_StoragePath(m_Name), iProtectedBytes)
    End Sub




    Public Shared Function Encrypt(iData As Byte()) As Byte()
        Return ProtectedData.Protect(iData, g_iEntropy, DataProtectionScope.CurrentUser)
    End Function

    Public Shared Function Encrypt(sText As String, i As Text.Encoding) As String
        Return i.GetString(ProtectedData.Protect(i.GetBytes(sText), g_iEntropy, DataProtectionScope.CurrentUser))
    End Function


    Public Shared Function Decrypt(iData As Byte()) As Byte()
        Return ProtectedData.Unprotect(iData, g_iEntropy, DataProtectionScope.CurrentUser)
    End Function

    Public Shared Function Decrypt(sText As String, i As Text.Encoding) As String
        Return i.GetString(ProtectedData.Unprotect(i.GetBytes(sText), g_iEntropy, DataProtectionScope.CurrentUser))
    End Function
End Class
