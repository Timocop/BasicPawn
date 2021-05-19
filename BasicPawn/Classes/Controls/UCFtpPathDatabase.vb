'BasicPawn
'Copyright(C) 2021 Externet

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


Public Class UCFtpPathDatabase
    Private g_lFtpEntries As New List(Of STRUC_FTP_ENTRY_ITEM)

    Event OnEntryAdded(mFtpItem As STRUC_FTP_ENTRY_ITEM, ByRef r_bHandeled As Boolean)

    Enum ENUM_FTP_PROTOCOL_TYPE
        FTP
        SFTP
    End Enum

    Class STRUC_FTP_ENTRY_ITEM
        Public sGUID As String
        Public sHost As String
        Public sDatabaseEntry As String
        Public sDestinationPath As String
        Public iProtocolType As ENUM_FTP_PROTOCOL_TYPE

        Sub New(_Host As String, _DatabaseEntry As String, _DestinationPath As String, _ProtocolType As ENUM_FTP_PROTOCOL_TYPE)
            sGUID = Guid.NewGuid.ToString
            sHost = _Host
            sDatabaseEntry = _DatabaseEntry
            sDestinationPath = _DestinationPath
            iProtocolType = _ProtocolType
        End Sub
    End Class

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        GroupBox_NewEntry.Visible = False
        ComboBox_Protocol.SelectedIndex = 0
    End Sub

    Public ReadOnly Property m_FtpEntries As List(Of STRUC_FTP_ENTRY_ITEM)
        Get
            Return g_lFtpEntries
        End Get
    End Property

    Public Function GetSelectedEntry() As STRUC_FTP_ENTRY_ITEM
        If (ListView_FtpEntries.SelectedItems.Count < 1) Then
            Return Nothing
        End If

        Dim mListViewItemData = TryCast(ListView_FtpEntries.SelectedItems(0), ClassListViewItemData)
        If (mListViewItemData Is Nothing) Then
            Return Nothing
        End If

        Dim sGUID As String = CStr(mListViewItemData.g_mData("GUID"))

        For Each mFtpItem In m_FtpEntries.ToArray
            If (mFtpItem.sGUID = sGUID) Then
                Return mFtpItem
            End If
        Next

        Return Nothing
    End Function

    Private Sub CheckBox_MoreDetails_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_MoreDetails.CheckedChanged
        GroupBox_NewEntry.Visible = CheckBox_MoreDetails.Checked
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

            RefreshListView()
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

            Dim sDestinationPath As String = TextBox_DestinationPath.Text

            Dim iProtocol As ENUM_FTP_PROTOCOL_TYPE
            Select Case (ComboBox_Protocol.SelectedIndex)
                Case 0
                    iProtocol = ENUM_FTP_PROTOCOL_TYPE.FTP
                Case Else
                    iProtocol = ENUM_FTP_PROTOCOL_TYPE.SFTP
            End Select

            Dim bHandeled As Boolean = False
            RaiseEvent OnEntryAdded(New STRUC_FTP_ENTRY_ITEM(sHost, sDatabaseEntry, sDestinationPath, iProtocol), bHandeled)
            If (bHandeled) Then
                Return
            End If

            g_lFtpEntries.Add(New STRUC_FTP_ENTRY_ITEM(sHost, sDatabaseEntry, sDestinationPath, iProtocol))

            RefreshListView()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Public Sub LoadSettings(mIni As ClassIni)
        Try
            g_lFtpEntries.Clear()

            For Each sSection As String In mIni.GetSectionNames
                Dim sHost As String = mIni.ReadKeyValue(sSection, "Host", Nothing)
                Dim sDatabaseEntry As String = mIni.ReadKeyValue(sSection, "DatabaseEntry", Nothing)
                Dim sDestinationPath As String = mIni.ReadKeyValue(sSection, "DestinationPath", Nothing)
                Dim sProtocol As String = mIni.ReadKeyValue(sSection, "Protocol", "FTP")

                If (String.IsNullOrEmpty(sHost) OrElse String.IsNullOrEmpty(sDatabaseEntry) OrElse String.IsNullOrEmpty(sDestinationPath)) Then
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
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Public Sub SaveSettings(mIni As ClassIni)
        Try
            For Each mFtpItem In g_lFtpEntries.ToArray
                Dim sSection As String = Guid.NewGuid.ToString

                mIni.WriteKeyValue(sSection, "Host", mFtpItem.sHost)
                mIni.WriteKeyValue(sSection, "DatabaseEntry", mFtpItem.sDatabaseEntry)
                mIni.WriteKeyValue(sSection, "DestinationPath", mFtpItem.sDestinationPath)
                mIni.WriteKeyValue(sSection, "Protocol", If(mFtpItem.iProtocolType = ENUM_FTP_PROTOCOL_TYPE.FTP, "FTP", "SFTP"))
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Public Sub RefreshListView()
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

End Class
