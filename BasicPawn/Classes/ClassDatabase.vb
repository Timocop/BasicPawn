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


Imports System.Security.Principal
Imports System.Text

Public Class ClassDatabase
    Private Shared g_sDatabasePath As String = IO.Path.Combine(Application.StartupPath, "database.ini")

    Class STRUC_DATABASE_ITEM
        Private g_sName As String
        Private g_sUserSid As String
        Private g_iUsername As Byte()
        Private g_iPassword As Byte()

        Public Sub New(sName As String, sUsername As String, sPassword As String)
            Me.New(sName, WindowsIdentity.GetCurrent.User.Value, sUsername, sPassword)
        End Sub

        Public Sub New(sName As String, sUserSid As String, sUsername As String, sPassword As String)
            g_sName = sName
            g_sUserSid = sUserSid

            m_Username = sUsername
            m_Password = sPassword
        End Sub

        Public ReadOnly Property m_Name As String
            Get
                Return g_sName
            End Get
        End Property

        Public ReadOnly Property m_UserSid As String
            Get
                Return g_sUserSid
            End Get
        End Property

        Public Property m_Username As String
            Get
                Dim i As Byte() = ClassSecureStorage.Decrypt(g_iUsername)
                Return Encoding.UTF8.GetString(i)
            End Get
            Set(value As String)
                Dim i As Byte() = Encoding.UTF8.GetBytes(value)
                g_iUsername = ClassSecureStorage.Encrypt(i)
            End Set
        End Property

        Public Property m_Password As String
            Get
                Dim i As Byte() = ClassSecureStorage.Decrypt(g_iPassword)
                Return Encoding.UTF8.GetString(i)
            End Get
            Set(value As String)
                Dim i As Byte() = Encoding.UTF8.GetBytes(value)
                g_iPassword = ClassSecureStorage.Encrypt(i)
            End Set
        End Property



        Public Sub Save()
            Dim sCryptUsername As String = m_Username
            Dim sCryptPassword As String = m_Password

            sCryptUsername = ClassSecureStorage.Encrypt(sCryptUsername, Encoding.UTF8)
            sCryptPassword = ClassSecureStorage.Encrypt(sCryptPassword, Encoding.UTF8)

            sCryptUsername = ClassTools.ClassCrypto.Base.ToBase64(sCryptUsername)
            sCryptPassword = ClassTools.ClassCrypto.Base.ToBase64(sCryptPassword)

            Dim mIniFile As New ClassIniFile(g_sDatabasePath)
            mIniFile.WriteKeyValue(g_sName, "UserSid", g_sUserSid)
            mIniFile.WriteKeyValue(g_sName, "Username", sCryptUsername)
            mIniFile.WriteKeyValue(g_sName, "Password", sCryptPassword)
        End Sub

        Public Sub Remove()
            If (Not IO.File.Exists(g_sDatabasePath)) Then
                Return
            End If

            Dim mIniFile As New ClassIniFile(g_sDatabasePath)
            mIniFile.WriteKeyValue(g_sName, "UserSid")
            mIniFile.WriteKeyValue(g_sName, "Username")
            mIniFile.WriteKeyValue(g_sName, "Password")
        End Sub
    End Class

    Public Function GetDatabaseItems() As STRUC_DATABASE_ITEM()
        If (Not IO.File.Exists(g_sDatabasePath)) Then
            Return Nothing
        End If

        Dim lDatabaseItems As New List(Of STRUC_DATABASE_ITEM)

        Dim mIniFile As New ClassIniFile(g_sDatabasePath)

        For Each sSection As String In mIniFile.GetSectionNames
            Dim sCryptUserSid As String = mIniFile.ReadKeyValue(sSection, "UserSid")
            Dim sCryptUsername As String = mIniFile.ReadKeyValue(sSection, "Username")
            Dim sCryptPassword As String = mIniFile.ReadKeyValue(sSection, "Password")

            If (String.IsNullOrEmpty(sCryptUserSid) OrElse String.IsNullOrEmpty(sCryptUsername) OrElse String.IsNullOrEmpty(sCryptPassword)) Then
                Continue For
            End If

            If (sCryptUserSid.ToLower <> WindowsIdentity.GetCurrent.User.Value.ToLower) Then
                Continue For
            End If

            Try
                sCryptUsername = ClassTools.ClassCrypto.Base.FromBase64(sCryptUsername)
                sCryptPassword = ClassTools.ClassCrypto.Base.FromBase64(sCryptPassword)

                sCryptUsername = ClassSecureStorage.Decrypt(sCryptUsername, Encoding.UTF8)
                sCryptPassword = ClassSecureStorage.Decrypt(sCryptPassword, Encoding.UTF8)

                lDatabaseItems.Add(New STRUC_DATABASE_ITEM(sSection, sCryptUserSid, sCryptUsername, sCryptPassword))
            Catch ex As Exception
                'Ignore invalid base64
            End Try
        Next

        Return lDatabaseItems.ToArray
    End Function
End Class
