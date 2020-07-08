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


Imports System.Windows.Forms
Imports BasicPawn

Public Class FormSettings
    Private g_mPluginAutoErrorReport As PluginAutoErrorReport

    Private g_mPluginConfigFtp As ClassPluginController.ClassPluginConfig
    Private g_mPluginConfigSettings As ClassPluginController.ClassPluginConfig

    Private g_mUCFtpDatabase As UCFtpPathDatabase
    Private g_bDatabaseLoaded As Boolean = False

    Public Sub New(mPluginAutoErrorReport As PluginAutoErrorReport)
        g_mPluginAutoErrorReport = mPluginAutoErrorReport

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        g_mUCFtpDatabase = New UCFtpPathDatabase With {
            .Parent = Panel_FtpDatabase,
            .Dock = DockStyle.Fill
        }
        g_mUCFtpDatabase.Show()

        AddHandler g_mUCFtpDatabase.OnEntryAdded, AddressOf OnFtpDatabaseEntryAdded

        Me.AutoSize = True

        g_mPluginConfigFtp = New ClassPluginController.ClassPluginConfig("PluginAutoErrorReportFtpEntries")
        g_mPluginConfigSettings = New ClassPluginController.ClassPluginConfig("PluginAutoErrorReportSettings")

        LoadSettings()
    End Sub

    Private Sub FormSettings_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClassControlStyle.UpdateControls(Me)

        LoadSettings()
    End Sub

    Private Sub FormSettings_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveSettings()
    End Sub

    Private Sub FormSettings_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub Button_Close_Click(sender As Object, e As EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    Private Sub OnFtpDatabaseEntryAdded(mFtpItem As UCFtpPathDatabase.STRUC_FTP_ENTRY_ITEM, ByRef r_bHandeled As Boolean)
        Try
            r_bHandeled = True

            Dim mDatabaseItem = ClassDatabase.FindDatabaseItemByName(mFtpItem.sDatabaseEntry)
            If (mDatabaseItem Is Nothing) Then
                Throw New ArgumentException("Unable to find database entry")
            End If

            Dim sUsername As String = mDatabaseItem.m_Username
            Dim sPassword As String = mDatabaseItem.m_Password

            While True
                Dim bIsValid As Boolean = False

                Using mFormProgress As New FormProgress()
                    mFormProgress.Text = "Checking SourceMod..."
                    mFormProgress.m_Progress = 0
                    mFormProgress.Show(Me)

                    bIsValid = IsValidSourceModPath(mFtpItem.sDestinationPath, mFtpItem.iProtocolType, mFtpItem.sHost, sUsername, sPassword)

                    mFormProgress.m_Progress = 100
                End Using

                If (bIsValid) Then
                    r_bHandeled = False
                Else
                    Select Case (MessageBox.Show(Me, "Could not find SourceMod! Please try another destination path", "Unable to find SourceMod", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
                        Case DialogResult.Retry
                            Continue While
                    End Select
                End If

                Exit While
            End While
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Function IsValidSourceModPath(sPath As String, iProtocolType As UCFtpPathDatabase.ENUM_FTP_PROTOCOL_TYPE, sHost As String, sUsername As String, sPassword As String) As Boolean
        Dim g_mClassFTP As ClassFTP = Nothing
        Dim g_mClassSFTP As Renci.SshNet.SftpClient = Nothing

        Dim sRequiredPaths As String() = {
            "bin",
            "configs",
            "data",
            "extensions",
            "gamedata",
            "logs",
            "plugins"
        }

        Try
            Select Case (iProtocolType)
                Case UCFtpPathDatabase.ENUM_FTP_PROTOCOL_TYPE.FTP
                    g_mClassFTP = New ClassFTP(sHost, sUsername, sPassword)

                    For Each sSourceModPath As String In sRequiredPaths
                        sSourceModPath = IO.Path.Combine(sPath, sSourceModPath).Replace("\", "/")

                        If (Not g_mClassFTP.PathExist(sSourceModPath)) Then
                            Return False
                        End If
                    Next

                    Return True

                Case UCFtpPathDatabase.ENUM_FTP_PROTOCOL_TYPE.SFTP
                    g_mClassSFTP = New Renci.SshNet.SftpClient(sHost, sUsername, sPassword)

                    If (Not g_mClassSFTP.IsConnected) Then
                        g_mClassSFTP.Connect()
                    End If

                    For Each sSourceModPath As String In sRequiredPaths
                        sSourceModPath = IO.Path.Combine(sPath, sSourceModPath).Replace("\", "/")

                        If (Not g_mClassSFTP.Exists(sSourceModPath)) Then
                            Return False
                        End If
                    Next

                    Return True

                Case Else
                    Throw New ArgumentException("Unknown connection type")
            End Select
        Finally
            If (g_mClassSFTP IsNot Nothing) Then
                g_mClassSFTP.Dispose()
                g_mClassSFTP = Nothing
            End If
        End Try

        Return False
    End Function

    Private Sub LoadSettings()
        Try
            g_mPluginConfigFtp.LoadConfig()
            g_mPluginConfigSettings.LoadConfig()

            'Load Servers 
            g_mUCFtpDatabase.LoadSettings(g_mPluginConfigFtp)
            g_mUCFtpDatabase.RefreshListView()

            'Load Settings 
            Dim iMaxFileSize As Integer = 0
            If (Integer.TryParse(g_mPluginConfigSettings.ReadKeyValue("Settings", "MaxFileSize", "100"), iMaxFileSize)) Then
                NumericUpDown_MaxFileSize.Value = ClassTools.ClassMath.ClampInt(iMaxFileSize, CInt(NumericUpDown_MaxFileSize.Minimum), CInt(NumericUpDown_MaxFileSize.Maximum))
            End If

            g_bDatabaseLoaded = True
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub SaveSettings()
        Try
            If (Not g_bDatabaseLoaded) Then
                Return
            End If

            'Save Servers 
            g_mPluginConfigFtp.ParseFromString("")
            g_mUCFtpDatabase.SaveSettings(g_mPluginConfigFtp)

            'Save Settings
            g_mPluginConfigSettings.ParseFromString("")
            g_mPluginConfigSettings.WriteKeyValue("Settings", "MaxFileSize", CStr(NumericUpDown_MaxFileSize.Value))

            g_mPluginConfigFtp.SaveConfig()
            g_mPluginConfigSettings.SaveConfig()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub CleanUp()
        If (g_mUCFtpDatabase IsNot Nothing) Then
            RemoveHandler g_mUCFtpDatabase.OnEntryAdded, AddressOf OnFtpDatabaseEntryAdded
        End If
    End Sub
End Class