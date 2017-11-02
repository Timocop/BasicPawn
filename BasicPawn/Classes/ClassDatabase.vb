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

#Const ENCRYPT_DATABASE = True

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
                Return Encoding.Unicode.GetString(i)
            End Get
            Set(value As String)
                Dim i As Byte() = Encoding.Unicode.GetBytes(value)
                g_iUsername = ClassSecureStorage.Encrypt(i)
            End Set
        End Property

        Public Property m_Password As String
            Get
                Dim i As Byte() = ClassSecureStorage.Decrypt(g_iPassword)
                Return Encoding.Unicode.GetString(i)
            End Get
            Set(value As String)
                Dim i As Byte() = Encoding.Unicode.GetBytes(value)
                g_iPassword = ClassSecureStorage.Encrypt(i)
            End Set
        End Property

        Public Function HasUserAccess() As Boolean
            Return g_sUserSid.ToLower = WindowsIdentity.GetCurrent.User.Value.ToLower
        End Function

        Public Sub Save()
            Dim sCryptUsername As String
            Dim sCryptPassword As String

#If ENCRYPT_DATABASE Then
            Dim iCryptUsername As Byte() = Encoding.Unicode.GetBytes(m_Username)
            Dim iCryptPassword As Byte() = Encoding.Unicode.GetBytes(m_Password)

            iCryptUsername = ClassSecureStorage.Encrypt(iCryptUsername)
            iCryptPassword = ClassSecureStorage.Encrypt(iCryptPassword)

            'Test
            ClassSecureStorage.Decrypt(iCryptUsername)
            ClassSecureStorage.Decrypt(iCryptPassword)

            sCryptUsername = ClassTools.ClassCrypto.ClassBase.ToBase64Ex(iCryptUsername)
            sCryptPassword = ClassTools.ClassCrypto.ClassBase.ToBase64Ex(iCryptPassword)
#Else
            Dim iCryptUsername As Byte() = Encoding.Unicode.GetBytes(m_Username)
            Dim iCryptPassword As Byte() = Encoding.Unicode.GetBytes(m_Password)

            sCryptUsername = ClassTools.ClassCrypto.Base.ToBase64Ex(iCryptUsername)
            sCryptPassword = ClassTools.ClassCrypto.Base.ToBase64Ex(iCryptPassword)
#End If


            Using mStream = ClassFileStreamWait.Create(g_sDatabasePath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    mIni.WriteKeyValue(g_sName, "UserSid", g_sUserSid)
                    mIni.WriteKeyValue(g_sName, "Username", sCryptUsername)
                    mIni.WriteKeyValue(g_sName, "Password", sCryptPassword)
                End Using
            End Using
        End Sub

        Public Sub Remove()
            If (Not IO.File.Exists(g_sDatabasePath)) Then
                Return
            End If

            Using mStream = ClassFileStreamWait.Create(g_sDatabasePath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    mIni.WriteKeyValue(g_sName, "UserSid")
                    mIni.WriteKeyValue(g_sName, "Username")
                    mIni.WriteKeyValue(g_sName, "Password")
                End Using
            End Using
        End Sub
    End Class

    Public Shared Function FindDatabaseItemByName(sName As String) As STRUC_DATABASE_ITEM
        For Each mItem In GetDatabaseItems()
            If (mItem.m_Name = sName) Then
                Return mItem
            End If
        Next

        Return Nothing
    End Function

    Public Shared Function GetDatabaseItems() As STRUC_DATABASE_ITEM()
        If (Not IO.File.Exists(g_sDatabasePath)) Then
            Return {}
        End If

        Dim lDatabaseItems As New List(Of STRUC_DATABASE_ITEM)

        Using mStream = ClassFileStreamWait.Create(g_sDatabasePath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                For Each sSection As String In mIni.GetSectionNames
                    Try
                        Dim sCryptUserSid As String = mIni.ReadKeyValue(sSection, "UserSid")
                        Dim sCryptUsername As String = mIni.ReadKeyValue(sSection, "Username")
                        Dim sCryptPassword As String = mIni.ReadKeyValue(sSection, "Password")

                        If (String.IsNullOrEmpty(sCryptUserSid) OrElse String.IsNullOrEmpty(sCryptUsername) OrElse String.IsNullOrEmpty(sCryptPassword)) Then
                            Continue For
                        End If

                        If (sCryptUserSid.ToLower <> WindowsIdentity.GetCurrent.User.Value.ToLower) Then
                            Continue For
                        End If


#If ENCRYPT_DATABASE Then
                        Dim iCryptUsername As Byte() = ClassTools.ClassCrypto.ClassBase.FromBase64Ex(sCryptUsername)
                        Dim iCryptPassword As Byte() = ClassTools.ClassCrypto.ClassBase.FromBase64Ex(sCryptPassword)

                        iCryptUsername = ClassSecureStorage.Decrypt(iCryptUsername)
                        iCryptPassword = ClassSecureStorage.Decrypt(iCryptPassword)

                        sCryptUsername = Encoding.Unicode.GetString(iCryptUsername)
                        sCryptPassword = Encoding.Unicode.GetString(iCryptPassword)
#Else
                Dim iCryptUsername As Byte() = ClassTools.ClassCrypto.Base.FromBase64Ex(sCryptUsername)
                Dim iCryptPassword As Byte() = ClassTools.ClassCrypto.Base.FromBase64Ex(sCryptPassword)

                sCryptUsername = Encoding.Unicode.GetString(iCryptUsername)
                sCryptPassword = Encoding.Unicode.GetString(iCryptPassword)
#End If

                        lDatabaseItems.Add(New STRUC_DATABASE_ITEM(sSection, sCryptUserSid, sCryptUsername, sCryptPassword))
                    Catch ex As Exception
                        'Ignore invalid base64
                    End Try
                Next
            End Using
        End Using

        Return lDatabaseItems.ToArray
    End Function

    Public Shared Function IsNameUsed(sName As String) As Boolean
        If (Not IO.File.Exists(g_sDatabasePath)) Then
            Return False
        End If

        Using mStream = ClassFileStreamWait.Create(g_sDatabasePath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                For Each sSection As String In mIni.GetSectionNames
                    If (sSection = sName) Then
                        Return True
                    End If
                Next
            End Using
        End Using

        Return False
    End Function
End Class
