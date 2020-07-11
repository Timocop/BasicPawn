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



Public Class FormFTP
    Private g_mPluginFTP As PluginFTP

    Private g_mPluginConfig As ClassPluginController.ClassPluginConfig

    Private g_mUploadThread As Threading.Thread
    Private g_mFormProgress As FormProgress
    Private g_mUCFtpDatabase As UCFtpPathDatabase
    Private g_bDatabaseLoaded As Boolean = False

    Public Sub New(mPluginFTP As PluginFTP, sFile As String)
        Me.New(mPluginFTP, New String() {sFile})
    End Sub


    Public Sub New(mPluginFTP As PluginFTP, sFiles As String())
        g_mPluginFTP = mPluginFTP

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        g_mUCFtpDatabase = New UCFtpPathDatabase With {
            .Parent = Panel_FtpDatabase,
            .Dock = DockStyle.Fill
        }
        g_mUCFtpDatabase.Show()

        TextBox_UploadFile.Multiline = True
        TextBox_UploadFile.Text = String.Join(";"c, sFiles)

        Me.AutoSize = True

        g_mPluginConfig = New ClassPluginController.ClassPluginConfig("PluginFtpEntries")
    End Sub

    Private Sub FormFTP_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClassControlStyle.UpdateControls(Me)

        LoadSettings()
    End Sub

    Private Sub FormFTP_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveSettings()
    End Sub

    Private Sub FormFTP_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub Button_SearchUploadFile_Click(sender As Object, e As EventArgs) Handles Button_SearchUploadFile.Click
        Using i As New OpenFileDialog
            i.Multiselect = True

            If (i.ShowDialog = DialogResult.OK) Then
                TextBox_UploadFile.Text = String.Join(";"c, i.FileNames)
            End If
        End Using
    End Sub

    Private Sub Button_Browse_Click(sender As Object, e As EventArgs) Handles Button_Browse.Click
        Try
            Dim mFtpItem = g_mUCFtpDatabase.GetSelectedEntry
            If (mFtpItem Is Nothing) Then
                Throw New ArgumentException("Unable to find entry")
            End If

            Dim mDatabaseItem = ClassDatabase.FindDatabaseItemByName(mFtpItem.sDatabaseEntry)
            If (mDatabaseItem Is Nothing) Then
                Throw New ArgumentException("Unable to find database entry")
            End If

            Using i As New FormFileDialogFTP(FormFileDialogFTP.ENUM_DIALOG_TYPE.OPEN_DIRECTORY, CType(mFtpItem.iProtocolType, FormFileDialogFTP.ENUM_FTP_PROTOCOL_TYPE), mFtpItem.sHost, mDatabaseItem.m_Username, mDatabaseItem.m_Password, "")
                i.ShowDialog()
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_Upload_Click(sender As Object, e As EventArgs) Handles Button_Upload.Click
        If (ClassThread.IsValid(g_mUploadThread)) Then
            Return
        End If

        Try
            Dim mFtpItem = g_mUCFtpDatabase.GetSelectedEntry
            If (mFtpItem Is Nothing) Then
                Throw New ArgumentException("Unable to find entry")
            End If

            Dim mDatabaseItem = ClassDatabase.FindDatabaseItemByName(mFtpItem.sDatabaseEntry)
            If (mDatabaseItem Is Nothing) Then
                Throw New ArgumentException("Unable to find database entry")
            End If

            Dim sFiles As String() = TextBox_UploadFile.Text.Split(";"c)
            Dim iProtocolType As UCFtpPathDatabase.ENUM_FTP_PROTOCOL_TYPE = mFtpItem.iProtocolType
            Dim sHost As String = mFtpItem.sHost
            Dim sDestinationPath As String = mFtpItem.sDestinationPath
            Dim sUsername As String = mDatabaseItem.m_Username
            Dim sPassword As String = mDatabaseItem.m_Password

            g_mUploadThread = New Threading.Thread(
                        Sub()
                            Dim bCanceled As Boolean = False

                            Try
                                ClassThread.ExecEx(Of Object)(Me, Sub() TableLayoutPanel_Controls.Enabled = False)

                                Try

                                    Dim mFtpClient As Object = Nothing
                                    Try
                                        Select Case (iProtocolType)
                                            Case UCFtpPathDatabase.ENUM_FTP_PROTOCOL_TYPE.FTP
                                                mFtpClient = New ClassFTP(sHost, sUsername, sPassword)
                                            Case Else
                                                mFtpClient = New Renci.SshNet.SftpClient(sHost, sUsername, sPassword)
                                        End Select

                                        Dim i As Integer
                                        For i = 0 To sFiles.Length - 1
                                            If (Not IO.File.Exists(sFiles(i))) Then
                                                g_mPluginFTP.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not upload '{0}' because the file does not exist!", sFiles(i)))
                                                Continue For
                                            End If

                                            g_mPluginFTP.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO,
                                                                                                           String.Format("Uploading file '{0}' ({1}/{2}) to '{3}/{4}' using {5}...",
                                                                                                                         sFiles(i), i + 1,
                                                                                                                         sFiles.Length,
                                                                                                                         sHost.TrimEnd("/"c),
                                                                                                                         sDestinationPath.TrimStart("/"c),
                                                                                                                         If(iProtocolType = UCFtpPathDatabase.ENUM_FTP_PROTOCOL_TYPE.FTP, "FTP", "SFTP")),
                                                                                                           New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(sFiles(i)))

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

                                            Select Case (True)
                                                Case (TypeOf mFtpClient Is ClassFTP)
                                                    Dim mClient = DirectCast(mFtpClient, ClassFTP)

                                                    mClient.UploadFile(sFiles(i), sDestinationFile, mUploadAction)

                                                Case (TypeOf mFtpClient Is Renci.SshNet.SftpClient)
                                                    Dim mClient = DirectCast(mFtpClient, Renci.SshNet.SftpClient)

                                                    If (Not mClient.IsConnected) Then
                                                        mClient.Connect()
                                                    End If

                                                    Using mStream As New IO.FileStream(sFiles(i), IO.FileMode.Open, IO.FileAccess.Read)
                                                        mClient.UploadFile(mStream, sDestinationFile, mUploadAction)
                                                    End Using
                                                Case Else
                                                    Throw New ArgumentException("Invalid FTP client")
                                            End Select

                                            g_mPluginFTP.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Upload Success!")
                                        Next
                                    Finally
                                        If (TypeOf mFtpClient Is Renci.SshNet.SftpClient) Then
                                            Dim mClient = DirectCast(mFtpClient, Renci.SshNet.SftpClient)

                                            mClient.Dispose()
                                        End If

                                        mFtpClient = Nothing
                                    End Try
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
                                    g_mPluginFTP.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Upload canceled!")

                                    ClassThread.ExecAsync(Me, Sub() Me.Close())
                                End If

                                Throw
                            Catch ex As Exception
                                g_mPluginFTP.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Upload failed!")

                                ClassExceptionLog.WriteToLogMessageBox(ex)

                                ClassThread.ExecAsync(Me, Sub() TableLayoutPanel_Controls.Enabled = True)
                            End Try
                        End Sub) With {
                        .IsBackground = True,
                        .Priority = Threading.ThreadPriority.Lowest
                    }
            g_mUploadThread.Start()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Private Sub LoadSettings()
        Try
            g_mPluginConfig.LoadConfig()

            g_mUCFtpDatabase.LoadSettings(g_mPluginConfig)
            g_mUCFtpDatabase.RefreshListView()

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

            g_mPluginConfig.ParseFromString("")
            g_mUCFtpDatabase.SaveSettings(g_mPluginConfig)

            g_mPluginConfig.SaveConfig()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub CleanUp()
        ClassThread.Abort(g_mUploadThread)

        If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
            g_mFormProgress.Dispose()
            g_mFormProgress = Nothing
        End If
    End Sub
End Class