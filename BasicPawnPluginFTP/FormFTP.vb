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


Imports System.Windows.Forms
Imports BasicPawn

Public Class FormFTP
    Private g_mPluginFTP As PluginFTP

    Private g_mSecureStorage As ClassSecureStorage
    Private g_lFtpEntries As New List(Of STRUC_FTP_ENTRY_ITEM)

    Private g_mUploadThread As Threading.Thread
    Private g_mFormProgress As FormProgress

    Enum ENUM_FTP_PROTOCOL_TYPE
        FTP
        SFTP
    End Enum

    Structure STRUC_FTP_ENTRY_ITEM
        Dim sGUID As String
        Dim sHost As String
        Dim sDatabaseEntry As String
        Dim sDestinationPath As String
        Dim iProtocolType As ENUM_FTP_PROTOCOL_TYPE

        Sub New(_Host As String, _DatabaseEntry As String, _DestinationPath As String, _ProtocolType As ENUM_FTP_PROTOCOL_TYPE)
            sGUID = Guid.NewGuid.ToString
            sHost = _Host
            sDatabaseEntry = _DatabaseEntry
            sDestinationPath = _DestinationPath
            iProtocolType = _ProtocolType
        End Sub
    End Structure

    Public Sub New(mPluginFTP As PluginFTP, sFile As String)
        Me.New(mPluginFTP, New String() {sFile})
    End Sub


    Public Sub New(mPluginFTP As PluginFTP, sFiles As String())
        g_mPluginFTP = mPluginFTP

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        TextBox_UploadFile.Multiline = True
        TextBox_UploadFile.Text = String.Join(";"c, sFiles)
        GroupBox_NewEntry.Visible = False
        ComboBox_Protocol.SelectedIndex = 0
        Me.AutoSize = True

        g_mSecureStorage = New ClassSecureStorage("PluginFtpEntries")

        LoadData()
        FillListView()
    End Sub

    Private Sub FormFTP_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub Button_SearchUploadFile_Click(sender As Object, e As EventArgs) Handles Button_SearchUploadFile.Click
        Using i As New OpenFileDialog
            i.Multiselect = True

            If (i.ShowDialog = DialogResult.OK) Then
                TextBox_UploadFile.Text = String.Join(";"c, i.FileNames)
            End If
        End Using
    End Sub

    Private Sub ListView_FtpEntries_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_FtpEntries.SelectedIndexChanged
        Button_Upload.Enabled = (ListView_FtpEntries.SelectedItems.Count > 0)
        Button_Browse.Enabled = (ListView_FtpEntries.SelectedItems.Count > 0)
    End Sub

    Private Sub ListView_FtpEntries_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView_FtpEntries.MouseDoubleClick
        Button_Upload.PerformClick()
    End Sub

    Private Sub LinkLabel_RemoveItem_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_RemoveItem.LinkClicked
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
            SaveData()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CheckBox_MoreDetails_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_MoreDetails.CheckedChanged
        GroupBox_NewEntry.Visible = CheckBox_MoreDetails.Checked
    End Sub

    Private Sub ComboBox_DatabaseEntry_DropDown(sender As Object, e As EventArgs) Handles ComboBox_DatabaseEntry.DropDown
        Try
            Dim sLastSelected As String = ComboBox_DatabaseEntry.Text

            ComboBox_DatabaseEntry.BeginUpdate()
            ComboBox_DatabaseEntry.Items.Clear()

            For Each mItem In ClassDatabase.GetDatabaseItems
                ComboBox_DatabaseEntry.Items.Add(mItem.m_Name)
            Next
            ComboBox_DatabaseEntry.EndUpdate()

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

            Using i As New FormFileDialogFTP(FormFileDialogFTP.ENUM_DIALOG_TYPE.OPEN_DIRECTORY, CType(iProtocol, FormFileDialogFTP.ENUM_FTP_PROTOCOL_TYPE), sHost, sUsername, sPassword, "")
                If (i.ShowDialog = DialogResult.OK) Then
                    TextBox_DestinationPath.Text = i.m_CurrentPath
                Else
                    Return
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_Browse_Click(sender As Object, e As EventArgs) Handles Button_Browse.Click
        Try
            If (ListView_FtpEntries.SelectedItems.Count < 1) Then
                Return
            End If

            Dim mListViewItemData = TryCast(ListView_FtpEntries.SelectedItems(0), ClassListViewItemData)
            If (mListViewItemData Is Nothing) Then
                Return
            End If

            Dim sGUID As String = CStr(mListViewItemData.g_mData("GUID"))

            For Each mFtpItem In g_lFtpEntries.ToArray
                If (mFtpItem.sGUID = sGUID) Then
                    Dim mDatabaseItem = ClassDatabase.FindDatabaseItemByName(mFtpItem.sDatabaseEntry)
                    If (mDatabaseItem Is Nothing) Then
                        Throw New ArgumentException("Unable to find database entry")
                    End If

                    Using i As New FormFileDialogFTP(FormFileDialogFTP.ENUM_DIALOG_TYPE.OPEN_DIRECTORY, CType(mFtpItem.iProtocolType, FormFileDialogFTP.ENUM_FTP_PROTOCOL_TYPE), mFtpItem.sHost, mDatabaseItem.m_Username, mDatabaseItem.m_Password, "")
                        i.ShowDialog()
                    End Using

                    Exit For
                End If
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_Upload_Click(sender As Object, e As EventArgs) Handles Button_Upload.Click
        If (ClassThread.IsValid(g_mUploadThread)) Then
            Return
        End If

        Try
            If (ListView_FtpEntries.SelectedItems.Count < 1) Then
                Return
            End If

            Dim mListViewItemData = TryCast(ListView_FtpEntries.SelectedItems(0), ClassListViewItemData)
            If (mListViewItemData Is Nothing) Then
                Return
            End If

            Dim sGUID As String = CStr(mListViewItemData.g_mData("GUID"))

            For Each mFtpItem In g_lFtpEntries.ToArray
                If (mFtpItem.sGUID = sGUID) Then
                    Dim mDatabaseItem = ClassDatabase.FindDatabaseItemByName(mFtpItem.sDatabaseEntry)
                    If (mDatabaseItem Is Nothing) Then
                        Throw New ArgumentException("Unable to find database entry")
                    End If

                    Dim sFiles As String() = TextBox_UploadFile.Text.Split(";"c)
                    Dim iProtocolType As ENUM_FTP_PROTOCOL_TYPE = mFtpItem.iProtocolType
                    Dim sHost As String = mFtpItem.sHost
                    Dim sDestinationPath As String = mFtpItem.sDestinationPath
                    Dim sUsername As String = mDatabaseItem.m_Username
                    Dim sPassword As String = mDatabaseItem.m_Password

                    g_mUploadThread = New Threading.Thread(Sub()
                                                               Dim bCanceled As Boolean = False

                                                               Try
                                                                   ClassThread.ExecEx(Of Object)(Me, Sub() TableLayoutPanel_Controls.Enabled = False)

                                                                   Try

                                                                       Dim i As Integer
                                                                       For i = 0 To sFiles.Length - 1
                                                                           If (Not IO.File.Exists(sFiles(i))) Then
                                                                               g_mPluginFTP.g_mFormMain.PrintInformation("[ERRO]", String.Format("Could not upload '{0}' because the file does not exist!", sFiles(i)))
                                                                               Continue For
                                                                           End If

                                                                           g_mPluginFTP.g_mFormMain.PrintInformation("[INFO]", String.Format("Uploading file '{0}' ({1}/{2}) to '{3}/{4}' using {5}...", sFiles(i), i + 1, sFiles.Length, sHost.TrimEnd("/"c), sDestinationPath.TrimStart("/"c),
                                                                                                                                       If(iProtocolType = ENUM_FTP_PROTOCOL_TYPE.FTP, "FTP", "SFTP")))

                                                                           Dim sFilename As String = IO.Path.GetFileName(sFiles(i))
                                                                           Dim sDestinationFile As String = (sDestinationPath.TrimEnd("/"c) & "/" & sFilename)
                                                                           Dim iFileLength As ULong = CULng(New IO.FileInfo(sFiles(i)).Length)

                                                                           ClassThread.ExecEx(Of Object)(Me, Sub()
                                                                                                                 If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                                                                                                                     g_mFormProgress.Dispose()
                                                                                                                     g_mFormProgress = Nothing
                                                                                                                 End If

                                                                                                                 g_mFormProgress = New FormProgress With {
                                                                                                                     .Text = String.Format("Uploading file ({0}/{1})...", i + 1, sFiles.Length),
                                                                                                                     .m_Progress = 0
                                                                                                                 }
                                                                                                                 g_mFormProgress.m_CloseAction = (Function()
                                                                                                                                                      'Ewww quite dirty
                                                                                                                                                      g_mFormProgress.Text = "Canceling..."
                                                                                                                                                      bCanceled = True
                                                                                                                                                      ClassThread.Abort(g_mUploadThread, False)
                                                                                                                                                      Return True
                                                                                                                                                  End Function)
                                                                                                                 g_mFormProgress.Show(Me)
                                                                                                             End Sub)

                                                                           Dim mUploadAction As Action(Of ULong) = (Sub(iBytesUplaoded As ULong)
                                                                                                                        Dim iProgress As Integer = ClassTools.ClassMath.ClampInt(CInt((iBytesUplaoded / iFileLength) * 100), 0, 100)

                                                                                                                        ClassThread.ExecAsync(Me, Sub()
                                                                                                                                                      If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                                                                                                                                                          If (g_mFormProgress.m_Progress <> iProgress) Then
                                                                                                                                                              g_mFormProgress.m_Progress = iProgress
                                                                                                                                                          End If
                                                                                                                                                      End If
                                                                                                                                                  End Sub)
                                                                                                                    End Sub)

                                                                           Select Case (iProtocolType)
                                                                               Case ENUM_FTP_PROTOCOL_TYPE.FTP
                                                                                   Dim mClassFTP As New ClassFTP(sHost, sUsername, sPassword)

                                                                                   mClassFTP.UploadFile(sFiles(i), sDestinationFile, mUploadAction)
                                                                               Case Else
                                                                                   Using mClassSFTP As New Renci.SshNet.SftpClient(sHost, sUsername, sPassword)
                                                                                       mClassSFTP.Connect()

                                                                                       Using mStream As New IO.FileStream(sFiles(i), IO.FileMode.Open, IO.FileAccess.Read)
                                                                                           mClassSFTP.UploadFile(mStream, sDestinationFile, mUploadAction)
                                                                                       End Using
                                                                                   End Using
                                                                           End Select

                                                                           g_mPluginFTP.g_mFormMain.PrintInformation("[INFO]", "Upload Success!")
                                                                       Next
                                                                   Finally
                                                                       ClassThread.ExecAsync(Me, Sub()
                                                                                                     If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                                                                                                         g_mFormProgress.Dispose()
                                                                                                         g_mFormProgress = Nothing
                                                                                                     End If
                                                                                                 End Sub)
                                                                   End Try

                                                                   ClassThread.ExecAsync(Me, Sub() Me.Close())
                                                               Catch ex As Threading.ThreadAbortException
                                                                   If (bCanceled) Then
                                                                       g_mPluginFTP.g_mFormMain.PrintInformation("[ERRO]", "Upload canceled!")

                                                                       ClassThread.ExecAsync(Me, Sub() Me.Close())
                                                                   End If

                                                                   Throw
                                                               Catch ex As Exception
                                                                   g_mPluginFTP.g_mFormMain.PrintInformation("[ERRO]", "Upload failed!")

                                                                   ClassExceptionLog.WriteToLogMessageBox(ex)

                                                                   ClassThread.ExecAsync(Me, Sub() TableLayoutPanel_Controls.Enabled = True)
                                                               End Try
                                                           End Sub) With {
                        .IsBackground = True,
                        .Priority = Threading.ThreadPriority.Lowest
                    }
                    g_mUploadThread.Start()

                    Exit For
                End If
            Next


        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
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

            Dim sDestinationPath As String = TextBox_DestinationPath.Text

            Dim iProtocol As ENUM_FTP_PROTOCOL_TYPE
            Select Case (ComboBox_Protocol.SelectedIndex)
                Case 0
                    iProtocol = ENUM_FTP_PROTOCOL_TYPE.FTP
                Case Else
                    iProtocol = ENUM_FTP_PROTOCOL_TYPE.SFTP
            End Select

            g_lFtpEntries.Add(New STRUC_FTP_ENTRY_ITEM(sHost, sDatabaseEntry, sDestinationPath, iProtocol))

            FillListView()
            SaveData()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Private Sub FormFTP_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub LoadData()
        Try
            g_lFtpEntries.Clear()

            g_mSecureStorage.Open()

            Using mIni As New ClassIni(g_mSecureStorage.m_String(System.Text.Encoding.Default))
                For Each sSection As String In mIni.GetSectionNames
                    Dim sHost As String = mIni.ReadKeyValue(sSection, "Host", Nothing)
                    Dim sDatabaseEntry As String = mIni.ReadKeyValue(sSection, "DatabaseEntry", Nothing)
                    Dim sDestinationPath As String = mIni.ReadKeyValue(sSection, "DestinationPath", Nothing)
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

                    g_lFtpEntries.Add(New STRUC_FTP_ENTRY_ITEM(sHost, sDatabaseEntry, sDestinationPath, iProtocolType))
                Next
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub SaveData()
        Try
            Using mIni As New ClassIni
                For Each mFtpItem In g_lFtpEntries.ToArray
                    Dim sSection As String = Guid.NewGuid.ToString

                    mIni.WriteKeyValue(sSection, "Host", mFtpItem.sHost)
                    mIni.WriteKeyValue(sSection, "DatabaseEntry", mFtpItem.sDatabaseEntry)
                    mIni.WriteKeyValue(sSection, "DestinationPath", mFtpItem.sDestinationPath)
                    mIni.WriteKeyValue(sSection, "Protocol", If(mFtpItem.iProtocolType = ENUM_FTP_PROTOCOL_TYPE.FTP, "FTP", "SFTP"))
                Next

                g_mSecureStorage.m_String(System.Text.Encoding.Default) = mIni.ExportToString
            End Using

            g_mSecureStorage.Close()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub FillListView()
        Dim lListViewItems As New List(Of ListViewItem)
        For Each mFtpItem In g_lFtpEntries.ToArray
            Dim mListViewItemData = New ClassListViewItemData(New String() {mFtpItem.sDatabaseEntry, mFtpItem.sHost, mFtpItem.sDestinationPath, If(mFtpItem.iProtocolType = ENUM_FTP_PROTOCOL_TYPE.FTP, "FTP", "SFTP")})
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
        ClassThread.Abort(g_mUploadThread)

        If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
            g_mFormProgress.Dispose()
            g_mFormProgress = Nothing
        End If
    End Sub
End Class