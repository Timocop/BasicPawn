'BasicPawn
'Copyright(C) 2018 TheTimocop

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

    Private g_mFtpSecureStorage As ClassSecureStorage
    Private g_mSettingsSecureStorage As ClassSecureStorage
    Private g_lFtpEntries As New List(Of STRUC_FTP_ENTRY_ITEM)

    Enum ENUM_FTP_PROTOCOL_TYPE
        FTP
        SFTP
    End Enum

    Structure STRUC_FTP_ENTRY_ITEM
        Dim sGUID As String
        Dim sHost As String
        Dim sDatabaseEntry As String
        Dim sSourceModPath As String
        Dim iProtocolType As ENUM_FTP_PROTOCOL_TYPE

        Sub New(_Host As String, _DatabaseEntry As String, _SourceModPath As String, _ProtocolType As ENUM_FTP_PROTOCOL_TYPE)
            sGUID = Guid.NewGuid.ToString
            sHost = _Host
            sDatabaseEntry = _DatabaseEntry
            sSourceModPath = _SourceModPath
            iProtocolType = _ProtocolType
        End Sub
    End Structure

    Public Sub New(mPluginAutoErrorReport As PluginAutoErrorReport)
        g_mPluginAutoErrorReport = mPluginAutoErrorReport

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        GroupBox_NewEntry.Visible = False
        ComboBox_Protocol.SelectedIndex = 0
        Me.AutoSize = True

        g_mFtpSecureStorage = New ClassSecureStorage("PluginAutoErrorReportFtpEntries")
        g_mSettingsSecureStorage = New ClassSecureStorage("PluginAutoErrorReportSettings")

        LoadSettings()
        FillListView()
    End Sub

    Private Sub FormSettings_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub CheckBox_MoreDetails_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_MoreDetails.CheckedChanged
        GroupBox_NewEntry.Visible = CheckBox_MoreDetails.Checked
    End Sub

    Private Sub FormSettings_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub Button_Close_Click(sender As Object, e As EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    Private Sub LinkLabel_Remove_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_Remove.LinkClicked
        Try
            If (ListView_FtpEntries.SelectedItems.Count < 1) Then
                Throw New ArgumentException("Nothing selected to remove")
            End If

            For i = 0 To ListView_FtpEntries.SelectedItems.Count - 1
                Dim mListViewItemData = TryCast(ListView_FtpEntries.SelectedItems(i), ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Throw New ArgumentException("Invalid type")
                End If

                Dim sGUID As String = CStr(mListViewItemData.g_mData("GUID"))

                g_lFtpEntries.RemoveAll(Function(x As STRUC_FTP_ENTRY_ITEM)
                                            Return x.sGUID = sGUID
                                        End Function)
            Next

            FillListView()
            SaveSettings()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button_AddEntry_Click(sender As Object, e As EventArgs) Handles Button_AddEntry.Click
        Try
            Dim sDatabaseEntry As String = ComboBox_DatabaseEntry.Text
            If (String.IsNullOrEmpty(sDatabaseEntry)) Then
                Throw New ArgumentException("Invalid database entry")
            End If

            Dim sHost As String = TextBox_Host.Text
            If (String.IsNullOrEmpty(sHost)) Then
                Throw New ArgumentException("Invalid host")
            End If

            Dim sSourceModPath As String = TextBox_SourceModPath.Text

            Dim iProtocol As ENUM_FTP_PROTOCOL_TYPE
            Select Case (ComboBox_Protocol.SelectedIndex)
                Case 0
                    iProtocol = ENUM_FTP_PROTOCOL_TYPE.FTP
                Case Else
                    iProtocol = ENUM_FTP_PROTOCOL_TYPE.SFTP
            End Select

            g_lFtpEntries.Add(New STRUC_FTP_ENTRY_ITEM(sHost, sDatabaseEntry, sSourceModPath, iProtocol))

            FillListView()
            SaveSettings()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ComboBox_DatabaseEntry_DropDown(sender As Object, e As EventArgs) Handles ComboBox_DatabaseEntry.DropDown
        Try
            Dim sLastSelected As String = ComboBox_DatabaseEntry.Text

            Try
                ComboBox_DatabaseEntry.BeginUpdate()
                ComboBox_DatabaseEntry.Items.Clear()

                For Each mItem In ClassDatabase.GetDatabaseItems
                    ComboBox_DatabaseEntry.Items.Add(mItem.m_Name)
                Next
            Finally
                ComboBox_DatabaseEntry.EndUpdate()
            End Try

            Dim iIndex As Integer = ComboBox_DatabaseEntry.Items.IndexOf(sLastSelected)
            If (iIndex > -1) Then
                ComboBox_DatabaseEntry.SelectedIndex = iIndex
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_SearchPath_Click(sender As Object, e As EventArgs) Handles Button_SearchPath.Click
        Try
            Dim sDatabaseEntry As String = ComboBox_DatabaseEntry.Text
            If (String.IsNullOrEmpty(sDatabaseEntry)) Then
                Throw New ArgumentException("Invalid database entry")
            End If

            Dim sHost As String = TextBox_Host.Text
            If (String.IsNullOrEmpty(sHost)) Then
                Throw New ArgumentException("Invalid host")
            End If

            Dim iProtocol As ENUM_FTP_PROTOCOL_TYPE
            Select Case (ComboBox_Protocol.SelectedIndex)
                Case 0
                    iProtocol = ENUM_FTP_PROTOCOL_TYPE.FTP
                Case Else
                    iProtocol = ENUM_FTP_PROTOCOL_TYPE.SFTP
            End Select

            Dim mDatabaseItem = ClassDatabase.FindDatabaseItemByName(sDatabaseEntry)
            If (mDatabaseItem Is Nothing) Then
                Throw New ArgumentException("Unable to find database entry")
            End If

            Dim sUsername As String = mDatabaseItem.m_Username
            Dim sPassword As String = mDatabaseItem.m_Password

            While True
                Using i As New FormFileDialogFTP(FormFileDialogFTP.ENUM_DIALOG_TYPE.OPEN_DIRECTORY, CType(iProtocol, FormFileDialogFTP.ENUM_FTP_PROTOCOL_TYPE), sHost, sUsername, sPassword, "")
                    If (i.ShowDialog = DialogResult.OK) Then
                        Dim sSourceModPath As String = i.m_CurrentPath

                        Dim bIsValid As Boolean = False

                        Using mFormProgress As New FormProgress()
                            mFormProgress.Text = "Checking SourceMod..."
                            mFormProgress.m_Progress = 0
                            mFormProgress.Show(Me)

                            bIsValid = IsValidSourceModPath(sSourceModPath, iProtocol, sHost, sUsername, sPassword)

                            mFormProgress.m_Progress = 100
                        End Using

                        If (bIsValid) Then
                            TextBox_SourceModPath.Text = sSourceModPath
                        Else
                            Select Case (MessageBox.Show(Me, "Could not find SourceMod! Please try another path", "Unable to find SourceMod", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
                                Case DialogResult.Retry
                                    Continue While
                            End Select
                        End If
                    Else
                        Return
                    End If
                End Using

                Exit While
            End While
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Function IsValidSourceModPath(sPath As String, iProtocolType As ENUM_FTP_PROTOCOL_TYPE, sHost As String, sUsername As String, sPassword As String) As Boolean
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
                Case ENUM_FTP_PROTOCOL_TYPE.FTP
                    g_mClassFTP = New ClassFTP(sHost, sUsername, sPassword)

                    For Each sSourceModPath As String In sRequiredPaths
                        sSourceModPath = IO.Path.Combine(sPath, sSourceModPath).Replace("\", "/")

                        If (Not g_mClassFTP.PathExist(sSourceModPath)) Then
                            Return False
                        End If
                    Next

                    Return True

                Case ENUM_FTP_PROTOCOL_TYPE.SFTP
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
        Catch ex As Exception
            'Nothing
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
            g_lFtpEntries.Clear()

            g_mFtpSecureStorage.Open()
            g_mSettingsSecureStorage.Open()

            'Load Servers
            Using mIni As New ClassIni(g_mFtpSecureStorage.m_String(System.Text.Encoding.Default))
                For Each sSection As String In mIni.GetSectionNames
                    Dim sHost As String = mIni.ReadKeyValue(sSection, "Host", Nothing)
                    Dim sDatabaseEntry As String = mIni.ReadKeyValue(sSection, "DatabaseEntry", Nothing)
                    Dim sSourceModPath As String = mIni.ReadKeyValue(sSection, "SourceModPath", Nothing)
                    Dim sProtocol As String = mIni.ReadKeyValue(sSection, "Protocol", "FTP")

                    If (String.IsNullOrEmpty(sHost) OrElse String.IsNullOrEmpty(sDatabaseEntry)) Then
                        Continue For
                    End If

                    Dim iProtocolType As ENUM_FTP_PROTOCOL_TYPE
                    Select Case (sProtocol)
                        Case "SFTP"
                            iProtocolType = ENUM_FTP_PROTOCOL_TYPE.SFTP
                        Case Else
                            iProtocolType = ENUM_FTP_PROTOCOL_TYPE.FTP
                    End Select

                    g_lFtpEntries.Add(New STRUC_FTP_ENTRY_ITEM(sHost, sDatabaseEntry, sSourceModPath, iProtocolType))
                Next
            End Using

            'Load Settings
            Using mIni As New ClassIni(g_mSettingsSecureStorage.m_String(System.Text.Encoding.Default))
                Dim iMaxFileSize As Integer = 0
                If (Integer.TryParse(mIni.ReadKeyValue("Settings", "MaxFileSize", "100"), iMaxFileSize)) Then
                    NumericUpDown_MaxFileSize.Value = ClassTools.ClassMath.ClampInt(iMaxFileSize, CInt(NumericUpDown_MaxFileSize.Minimum), CInt(NumericUpDown_MaxFileSize.Maximum))
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub SaveSettings()
        Try
            'Save Servers
            Using mIni As New ClassIni
                For Each mFtpItem In g_lFtpEntries.ToArray
                    Dim sSection As String = Guid.NewGuid.ToString

                    mIni.WriteKeyValue(sSection, "Host", mFtpItem.sHost)
                    mIni.WriteKeyValue(sSection, "DatabaseEntry", mFtpItem.sDatabaseEntry)
                    mIni.WriteKeyValue(sSection, "SourceModPath", mFtpItem.sSourceModPath)
                    mIni.WriteKeyValue(sSection, "Protocol", If(mFtpItem.iProtocolType = ENUM_FTP_PROTOCOL_TYPE.FTP, "FTP", "SFTP"))
                Next

                g_mFtpSecureStorage.m_String(System.Text.Encoding.Default) = mIni.ExportToString
            End Using

            'Save Settings
            Using mIni As New ClassIni
                mIni.WriteKeyValue("Settings", "MaxFileSize", CStr(NumericUpDown_MaxFileSize.Value))

                g_mSettingsSecureStorage.m_String(System.Text.Encoding.Default) = mIni.ExportToString
            End Using

            g_mFtpSecureStorage.Close()
            g_mSettingsSecureStorage.Close()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub FillListView()
        Dim lListViewItems As New List(Of ListViewItem)
        For Each mFtpItem In g_lFtpEntries.ToArray
            Dim mListViewItemData = New ClassListViewItemData(New String() {mFtpItem.sDatabaseEntry, mFtpItem.sHost, mFtpItem.sSourceModPath, If(mFtpItem.iProtocolType = ENUM_FTP_PROTOCOL_TYPE.FTP, "FTP", "SFTP")})
            mListViewItemData.g_mData("GUID") = mFtpItem.sGUID
            lListViewItems.Add(mListViewItemData)
        Next

        ListView_FtpEntries.BeginUpdate()
        ListView_FtpEntries.Items.Clear()
        ListView_FtpEntries.Items.AddRange(lListViewItems.ToArray)
        ClassTools.ClassControls.ClassListView.AutoResizeColumns(ListView_FtpEntries)
        ListView_FtpEntries.EndUpdate()
    End Sub

    Private Sub CleanUp()

    End Sub
End Class