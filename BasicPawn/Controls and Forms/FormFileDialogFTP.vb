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


Public Class FormFileDialogFTP
    Private g_mClassFTP As ClassFTP = Nothing
    Private g_mClassSFTP As Renci.SshNet.SftpClient = Nothing

    Private g_iDialogType As ENUM_DIALOG_TYPE = ENUM_DIALOG_TYPE.OPEN_DIRECTORY
    Private g_iProtocolType As ENUM_FTP_PROTOCOL_TYPE = ENUM_FTP_PROTOCOL_TYPE.FTP

    Private g_mRefreshThread As Threading.Thread

    Enum ENUM_FTP_PROTOCOL_TYPE
        FTP
        SFTP
    End Enum

    Enum ENUM_DIALOG_TYPE
        OPEN_FILE
        SAVE_FILE
        OPEN_DIRECTORY
    End Enum

    Public Sub New(iDialogType As ENUM_DIALOG_TYPE, iProtocolType As ENUM_FTP_PROTOCOL_TYPE, sHost As String, sUsername As String, sPassword As String, sPath As String)
        g_iDialogType = iDialogType
        g_iProtocolType = iProtocolType

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ImageList_FTP.Images.Clear()
        ImageList_FTP.Images.Add("0", My.Resources.Ico_Folder)
        ImageList_FTP.Images.Add("1", My.Resources.Ico_Rtf)

        Select Case (iDialogType)
            Case ENUM_DIALOG_TYPE.OPEN_FILE
                Me.Text = "Open file..."
                Button_Apply.Text = "Open"
                TextBox_Filename.Visible = True

            Case ENUM_DIALOG_TYPE.SAVE_FILE
                Me.Text = "Save file..."
                Button_Apply.Text = "Save"
                TextBox_Filename.Visible = True

            Case ENUM_DIALOG_TYPE.OPEN_DIRECTORY
                Me.Text = "Browse..."
                Button_Apply.Text = "Open"
                TextBox_Filename.Visible = False

            Case Else
                Throw New ArgumentException("Unknown dialog type")
        End Select

        Select Case (iProtocolType)
            Case ENUM_FTP_PROTOCOL_TYPE.FTP
                g_mClassFTP = New ClassFTP(sHost, sUsername, sPassword)

            Case ENUM_FTP_PROTOCOL_TYPE.SFTP
                g_mClassSFTP = New Renci.SshNet.SftpClient(sHost, sUsername, sPassword)

            Case Else
                Throw New ArgumentException("Unknown connection type")
        End Select

        Me.Text &= If(iProtocolType = ENUM_FTP_PROTOCOL_TYPE.FTP, " (Over FTP)", " (Over SFTP)")

        If (String.IsNullOrEmpty(sPath)) Then
            TextBox_Path.Text = "/"
        Else
            TextBox_Path.Text = sPath
        End If
    End Sub

    Private Sub FormFileDialogFTP_Load(sender As Object, e As EventArgs) Handles Me.Load
        RefreshListView()

        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub Button_Apply_Click(sender As Object, e As EventArgs) Handles Button_Apply.Click
        Select Case (g_iDialogType)
            Case ENUM_DIALOG_TYPE.OPEN_FILE, ENUM_DIALOG_TYPE.SAVE_FILE
                If (String.IsNullOrEmpty(m_SelectedFilename)) Then
                    MessageBox.Show("No file selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
        End Select

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button_Refresh_Click(sender As Object, e As EventArgs) Handles Button_Refresh.Click
        RefreshListView()
    End Sub

    Private Sub FormFileDialogFTP_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub ListView_FTP_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView_FTP.MouseDoubleClick
        If (ListView_FTP.SelectedItems.Count < 1) Then
            Return
        End If

        If (TypeOf ListView_FTP.SelectedItems(0) IsNot ClassListViewItemData) Then
            Return
        End If

        Dim mListViewItemData As ClassListViewItemData = DirectCast(ListView_FTP.SelectedItems(0), ClassListViewItemData)
        Dim bIsDirectory As Boolean = CBool(mListViewItemData.g_mData("IsDirectory"))
        Dim sName As String = CStr(mListViewItemData.g_mData("Name"))

        If (Not bIsDirectory) Then
            Return
        End If

        Dim sNewPath As String = ""

        If (sName = "..") Then
            Dim iIndex As Integer = m_CurrentPath.LastIndexOf("/"c)
            If (iIndex > -1) Then
                sNewPath = m_CurrentPath.Substring(0, iIndex)
            End If
        Else
            sNewPath = m_CurrentPath.TrimEnd("/"c) & "/" & sName
        End If

        m_CurrentPath = sNewPath

        RefreshListView()
    End Sub

    Private Sub ListView_FTP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_FTP.SelectedIndexChanged
        If (ListView_FTP.SelectedItems.Count < 1) Then
            Return
        End If

        If (TypeOf ListView_FTP.SelectedItems(0) IsNot ClassListViewItemData) Then
            Return
        End If

        Dim mListViewItemData As ClassListViewItemData = DirectCast(ListView_FTP.SelectedItems(0), ClassListViewItemData)
        Dim bIsDirectory As Boolean = CBool(mListViewItemData.g_mData("IsDirectory"))
        Dim sName As String = CStr(mListViewItemData.g_mData("Name"))

        If (bIsDirectory) Then
            m_SelectedFilename = ""
            Return
        End If

        m_SelectedFilename = sName
    End Sub

    Property m_CurrentPath As String
        Get
            Dim value As String = TextBox_Path.Text

            If (String.IsNullOrEmpty(value)) Then
                value = "/"
            End If

            Return value
        End Get
        Set(value As String)
            If (String.IsNullOrEmpty(value)) Then
                value = "/"
            End If

            TextBox_Path.Text = value
        End Set
    End Property

    Property m_SelectedFilename As String
        Get
            Return TextBox_Filename.Text
        End Get
        Set(value As String)
            TextBox_Filename.Text = value
        End Set
    End Property

    Private Sub RefreshListView()
        If (ClassThread.IsValid(g_mRefreshThread)) Then
            Return
        End If

        g_mRefreshThread = New Threading.Thread(Sub()
                                                    Try
                                                        ClassThread.ExecEx(Of Object)(Me, Sub() Me.Enabled = False)

                                                        Dim sPath As String = ClassThread.ExecEx(Of String)(Me, Function() m_CurrentPath)

                                                        Dim lListViewDirectoryItems As New List(Of ClassListViewItemData)
                                                        Dim lListViewFileItems As New List(Of ClassListViewItemData)
                                                        Dim mListViewItemData As ClassListViewItemData

                                                        mListViewItemData = New ClassListViewItemData(New String() {"..", "", "", "", ""}, "0")
                                                        mListViewItemData.g_mData("Name") = ".."
                                                        mListViewItemData.g_mData("IsDirectory") = True
                                                        lListViewDirectoryItems.Add(mListViewItemData)

                                                        Select Case (g_iProtocolType)
                                                            Case ENUM_FTP_PROTOCOL_TYPE.FTP
                                                                If (g_mClassFTP Is Nothing) Then
                                                                    Throw New ArgumentException("FTP is NULL")
                                                                End If

                                                                For Each mItem In g_mClassFTP.GetDirectoryEntries(sPath)
                                                                    Select Case (mItem.sName)
                                                                        Case ".", ".."
                                                                            Continue For
                                                                    End Select

                                                                    If (mItem.bIsDirectory) Then
                                                                        mListViewItemData = New ClassListViewItemData(New String() {mItem.sName, ClassTools.ClassStrings.FormatBytes(mItem.iSize), mItem.dModified.ToString, mItem.sPermissions, mItem.sOwner}, "0")
                                                                        mListViewItemData.g_mData("Name") = mItem.sName
                                                                        mListViewItemData.g_mData("IsDirectory") = True
                                                                        lListViewDirectoryItems.Add(mListViewItemData)
                                                                    Else
                                                                        mListViewItemData = New ClassListViewItemData(New String() {mItem.sName, ClassTools.ClassStrings.FormatBytes(mItem.iSize), mItem.dModified.ToString, mItem.sPermissions, mItem.sOwner}, "1")
                                                                        mListViewItemData.g_mData("Name") = mItem.sName
                                                                        mListViewItemData.g_mData("IsDirectory") = False
                                                                        lListViewFileItems.Add(mListViewItemData)
                                                                    End If
                                                                Next

                                                            Case Else
                                                                If (g_mClassSFTP Is Nothing) Then
                                                                    Throw New ArgumentException("SFTP is NULL")
                                                                End If

                                                                If (Not g_mClassSFTP.IsConnected) Then
                                                                    g_mClassSFTP.Connect()
                                                                End If

                                                                For Each mItem In g_mClassSFTP.ListDirectory(sPath)
                                                                    Select Case (mItem.Name)
                                                                        Case ".", ".."
                                                                            Continue For
                                                                    End Select

                                                                    Dim mFileAttributes As New Text.StringBuilder
                                                                    mFileAttributes.Append(If(mItem.IsDirectory, "d", "-"))
                                                                    mFileAttributes.Append(If(mItem.OwnerCanRead, "r", "-"))
                                                                    mFileAttributes.Append(If(mItem.OwnerCanWrite, "w", "-"))
                                                                    mFileAttributes.Append(If(mItem.OwnerCanExecute, "x", "-"))
                                                                    mFileAttributes.Append(If(mItem.GroupCanRead, "r", "-"))
                                                                    mFileAttributes.Append(If(mItem.GroupCanWrite, "w", "-"))
                                                                    mFileAttributes.Append(If(mItem.GroupCanExecute, "x", "-"))
                                                                    mFileAttributes.Append(If(mItem.OthersCanRead, "r", "-"))
                                                                    mFileAttributes.Append(If(mItem.OthersCanWrite, "w", "-"))
                                                                    mFileAttributes.Append(If(mItem.OthersCanExecute, "x", "-"))

                                                                    If (mItem.IsDirectory) Then
                                                                        mListViewItemData = New ClassListViewItemData(New String() {mItem.Name, ClassTools.ClassStrings.FormatBytes(mItem.Length), mItem.LastWriteTime.ToString, mFileAttributes.ToString, CStr(mItem.UserId)}, "0")
                                                                        mListViewItemData.g_mData("Name") = mItem.Name
                                                                        mListViewItemData.g_mData("IsDirectory") = True
                                                                        lListViewDirectoryItems.Add(mListViewItemData)
                                                                    Else
                                                                        mListViewItemData = New ClassListViewItemData(New String() {mItem.Name, ClassTools.ClassStrings.FormatBytes(mItem.Length), mItem.LastWriteTime.ToString, mFileAttributes.ToString, CStr(mItem.UserId)}, "1")
                                                                        mListViewItemData.g_mData("Name") = mItem.Name
                                                                        mListViewItemData.g_mData("IsDirectory") = False
                                                                        lListViewFileItems.Add(mListViewItemData)
                                                                    End If
                                                                Next
                                                        End Select

                                                        lListViewDirectoryItems.Sort(Function(x As ClassListViewItemData, y As ClassListViewItemData)
                                                                                         Return CStr(x.g_mData("Name")).CompareTo(CStr(y.g_mData("Name")))
                                                                                     End Function)

                                                        lListViewFileItems.Sort(Function(x As ClassListViewItemData, y As ClassListViewItemData)
                                                                                    Return CStr(x.g_mData("Name")).CompareTo(CStr(y.g_mData("Name")))
                                                                                End Function)


                                                        ClassThread.ExecAsync(ListView_FTP, Sub()
                                                                                                ListView_FTP.BeginUpdate()
                                                                                                ListView_FTP.Items.Clear()
                                                                                                ListView_FTP.Items.AddRange(lListViewDirectoryItems.ToArray)
                                                                                                ListView_FTP.Items.AddRange(lListViewFileItems.ToArray)
                                                                                                ClassTools.ClassControls.ClassListView.AutoResizeColumns(ListView_FTP)
                                                                                                ListView_FTP.EndUpdate()

                                                                                                Me.Enabled = True
                                                                                            End Sub)
                                                    Catch ex As Threading.ThreadAbortException
                                                        Throw
                                                    Catch ex As Exception
                                                        ClassExceptionLog.WriteToLogMessageBox(ex)

                                                        ClassThread.ExecAsync(Me, Sub() Me.Enabled = True)
                                                    End Try
                                                End Sub) With {
            .IsBackground = True,
            .Priority = Threading.ThreadPriority.Lowest
        }
        g_mRefreshThread.Start()
    End Sub

    Private Sub CleanUp()
        ClassThread.Abort(g_mRefreshThread)

        If (g_mClassSFTP IsNot Nothing) Then
            g_mClassSFTP.Dispose()
            g_mClassSFTP = Nothing
        End If
    End Sub
End Class