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


Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions

Public Class ClassFTP
    Private g_iHost As Byte()
    Private g_iUser As Byte()
    Private g_iPassword As Byte()

    Public Sub New(sHost As String, sUser As String, sPassword As String)
        m_Host = sHost
        m_User = sUser
        m_Password = sPassword
    End Sub

    Public Property m_Host As String
        Get
            Dim i As Byte() = ClassSecureStorage.Decrypt(g_iHost)
            Return Encoding.UTF8.GetString(i)
        End Get
        Set(value As String)
            Dim i As Byte() = Encoding.UTF8.GetBytes(value)
            g_iHost = ClassSecureStorage.Encrypt(i)
        End Set
    End Property

    Public Property m_User As String
        Get
            Dim i As Byte() = ClassSecureStorage.Decrypt(g_iUser)
            Return Encoding.UTF8.GetString(i)
        End Get
        Set(value As String)
            Dim i As Byte() = Encoding.UTF8.GetBytes(value)
            g_iUser = ClassSecureStorage.Encrypt(i)
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

    Public Sub DownloadFile(sRemoteFile As String, sLocalFile As String, Optional iBufferSize As Integer = 4 * 1024)
        Dim mRemoteURI As New Uri(String.Format("ftp://{0}/{1}", m_Host.TrimEnd("/"c), sRemoteFile.TrimStart("/"c)))

        Dim mFtpRequest = DirectCast(FtpWebRequest.Create(mRemoteURI), FtpWebRequest)
        mFtpRequest.Credentials = New NetworkCredential(m_User, m_Password)
        mFtpRequest.Method = WebRequestMethods.Ftp.DownloadFile

        Using mFtpResponse = DirectCast(mFtpRequest.GetResponse(), FtpWebResponse)
            Using mFtpStream = mFtpResponse.GetResponseStream()
                Using mFileStream As New FileStream(sLocalFile, FileMode.Create)
                    Dim iBuffer As Byte() = New Byte(iBufferSize) {}
                    Dim iBytesRead As Integer = 0

                    Do
                        iBytesRead = mFtpStream.Read(iBuffer, 0, iBufferSize)
                        mFileStream.Write(iBuffer, 0, iBytesRead)
                    Loop While iBytesRead > 0
                End Using
            End Using
        End Using
    End Sub

    Public Sub UploadFile(sLocalFile As String, sRemoteFile As String, Optional iBufferSize As Integer = 4 * 1024)
        Dim mRemoteURI As New Uri(String.Format("ftp://{0}/{1}", m_Host.TrimEnd("/"c), sRemoteFile.TrimStart("/"c)))

        Dim mFtpRequest = DirectCast(FtpWebRequest.Create(mRemoteURI), FtpWebRequest)
        mFtpRequest.Credentials = New NetworkCredential(m_User, m_Password)
        mFtpRequest.Method = WebRequestMethods.Ftp.UploadFile

        Using mFtpResponse = DirectCast(mFtpRequest.GetResponse(), FtpWebResponse)
            Using mFtpStream = mFtpResponse.GetResponseStream()
                Using mFileStream As New FileStream(sLocalFile, FileMode.Open)
                    Dim iBuffer As Byte() = New Byte(iBufferSize) {}
                    Dim iBytesRead As Integer = 0

                    Do
                        iBytesRead = mFileStream.Read(iBuffer, 0, iBufferSize)
                        mFtpStream.Write(iBuffer, 0, iBytesRead)
                    Loop While iBytesRead > 0
                End Using
            End Using
        End Using
    End Sub

    Public Sub DeleteFile(sRemoteFile As String)
        Dim mRemoteURI As New Uri(String.Format("ftp://{0}/{1}", m_Host.TrimEnd("/"c), sRemoteFile.TrimStart("/"c)))

        Dim mFtpRequest = DirectCast(FtpWebRequest.Create(mRemoteURI), FtpWebRequest)
        mFtpRequest.Credentials = New NetworkCredential(m_User, m_Password)
        mFtpRequest.Method = WebRequestMethods.Ftp.DeleteFile

        Using DirectCast(mFtpRequest.GetResponse(), FtpWebResponse)
        End Using
    End Sub

    Public Sub RenameFile(sRemoteFile As String, sTargetFileName As String)
        Dim mRemoteURI As New Uri(String.Format("ftp://{0}/{1}", m_Host.TrimEnd("/"c), sRemoteFile.TrimStart("/"c)))

        Dim mFtpRequest = DirectCast(FtpWebRequest.Create(mRemoteURI), FtpWebRequest)
        mFtpRequest.Credentials = New NetworkCredential(m_User, m_Password)
        mFtpRequest.Method = WebRequestMethods.Ftp.Rename
        mFtpRequest.RenameTo = sTargetFileName

        Using DirectCast(mFtpRequest.GetResponse(), FtpWebResponse)
        End Using
    End Sub

    Public Sub CreateDirectory(sDirectoryPath As String)
        Dim mRemoteURI As New Uri(String.Format("ftp://{0}/{1}", m_Host.TrimEnd("/"c), sDirectoryPath.TrimStart("/"c)))

        Dim mFtpRequest = DirectCast(FtpWebRequest.Create(mRemoteURI), FtpWebRequest)
        mFtpRequest.Credentials = New NetworkCredential(m_User, m_Password)
        mFtpRequest.Method = WebRequestMethods.Ftp.MakeDirectory

        Using DirectCast(mFtpRequest.GetResponse(), FtpWebResponse)
        End Using
    End Sub

    Public Structure STRUC_FTP_FILE_INFO
        Dim sPermissions As String
        Dim iNode As Integer
        Dim sOwner As String
        Dim sGroup As String
        Dim iSize As Long
        Dim dModified As Date
        Dim sName As String
        Dim bIsDirectory As Boolean
    End Structure

    Public Function GetDirectoryEntries(sDirectoryPath As String) As STRUC_FTP_FILE_INFO()
        Dim lEntries As New List(Of STRUC_FTP_FILE_INFO)

        Dim mRemoteURI As New Uri(String.Format("ftp://{0}/{1}", m_Host.TrimEnd("/"c), sDirectoryPath.TrimStart("/"c)))

        Dim mFtpRequest = DirectCast(FtpWebRequest.Create(mRemoteURI), FtpWebRequest)
        mFtpRequest.Credentials = New NetworkCredential(m_User, m_Password)
        mFtpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails

        Using mFtpResponse = DirectCast(mFtpRequest.GetResponse(), FtpWebResponse)
            Using mFtpStream = mFtpResponse.GetResponseStream()
                Using mFtpStreamReader As New StreamReader(mFtpStream)
                    Dim sPattern As String = "^(?<Permissions>[\w-]+)\s+(?<Node>\d+)\s+(?<Owner>[\w\d]+)\s+(?<Group>[\w\d]+)\s+(?<Size>\d+)\s+(?<Date>\w+\s+\d+\s+\d+|\w+\s+\d+\s+\d+:\d+)\s(?<Name>.+)$"
                    Dim mRegex As New Regex(sPattern)
                    Dim mCulture As CultureInfo = CultureInfo.GetCultureInfo("en-us")
                    Dim sDateTimeFormat As String() = New String() {"MMM dd HH:mm", "MMM dd H:mm", "MMM d HH:mm", "MMM d H:mm"}
                    Dim sDateFormats As String() = New String() {"MMM dd yyyy", "MMM d yyyy"}

                    While (Not mFtpStreamReader.EndOfStream)
                        Dim sLine As String = mFtpStreamReader.ReadLine()
                        Dim mMatch As Match = mRegex.Match(sLine)
                        If (Not mMatch.Success) Then
                            Continue While
                        End If

                        Dim sPermissions As String = mMatch.Groups("Permissions").Value
                        Dim iNode As Integer = Integer.Parse(mMatch.Groups("Node").Value, mCulture)
                        Dim sOwner As String = mMatch.Groups("Owner").Value
                        Dim sGroup As String = mMatch.Groups("Group").Value
                        Dim iSize As Long = Long.Parse(mMatch.Groups("Size").Value, mCulture)

                        Dim sDate As String = Regex.Replace(mMatch.Groups("Date").Value, "\s+", " ")
                        Dim dModified As Date = Date.ParseExact(sDate, If(sDate.Contains(":"), sDateTimeFormat, sDateFormats), mCulture, DateTimeStyles.None)
                        Dim sName As String = mMatch.Groups("Name").Value
                        Dim bIsDirectory As Boolean = sPermissions.StartsWith("d")

                        lEntries.Add(New STRUC_FTP_FILE_INFO With {
                            .sPermissions = sPermissions,
                            .iNode = iNode,
                            .sOwner = sOwner,
                            .sGroup = sGroup,
                            .iSize = iSize,
                            .dModified = dModified,
                            .sName = sName,
                            .bIsDirectory = bIsDirectory
                        })
                    End While
                End Using
            End Using
        End Using

        Return lEntries.ToArray
    End Function
End Class
