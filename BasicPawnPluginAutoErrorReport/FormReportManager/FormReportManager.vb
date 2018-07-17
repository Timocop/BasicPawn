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


Imports System.Drawing
Imports System.Windows.Forms
Imports BasicPawn

Public Class FormReportManager
    Private g_mFetchReportsThread As Threading.Thread

    Private g_mFtpSecureStorage As ClassSecureStorage
    Private g_mSettingsSecureStorage As ClassSecureStorage

    Public g_mPluginAutoErrorReport As PluginAutoErrorReport

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
        g_mFtpSecureStorage = New ClassSecureStorage("PluginAutoErrorReportFtpEntries")
        g_mSettingsSecureStorage = New ClassSecureStorage("PluginAutoErrorReportSettings")
    End Sub

    Private Sub FormReportManager_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClassControlStyle.UpdateControls(Me)

        FetchReports()
    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub GetReportsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GetReportsToolStripMenuItem.Click
        FetchReports()
    End Sub

    Private Sub CloseReportWindowsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseReportWindowsToolStripMenuItem.Click
        For Each mItem In GetItems()
            Dim mUCReportExceptionItem = TryCast(mItem, UCReportExceptionItem)
            If (mUCReportExceptionItem Is Nothing) Then
                Continue For
            End If

            mUCReportExceptionItem.CloseReportForm()
        Next
    End Sub

    Public Sub FetchReports()
        If (ClassThread.IsValid(g_mFetchReportsThread)) Then
            Return
        End If

        g_mFetchReportsThread = New Threading.Thread(Sub()
                                                         Try
                                                             ClassThread.ExecAsync(Me, Sub()
                                                                                           ToolStripProgressBar_Progress.Visible = True
                                                                                           ToolStripStatusLabel_Progress.Visible = True
                                                                                           ToolStripSplitButton_ProgressAbort.Visible = True
                                                                                           ToolStripStatusLabel_Progress.Text = "Starting thread..."
                                                                                       End Sub)

                                                             Dim lReportItems As New List(Of Object()) '{sTitle, sMessage, mImage}
                                                             Dim lReportExceptionItems As New List(Of ClassDebuggerParser.STRUC_SM_EXCEPTION)
                                                             Dim lFtpEntries As New List(Of STRUC_FTP_ENTRY_ITEM)

                                                             Dim iMaxFileBytes As Long = (100 * 1024 * 1024)
                                                             Dim bFilesTooBig As Boolean = False

                                                             g_mFtpSecureStorage.Open()
                                                             g_mSettingsSecureStorage.Open()

                                                             ClassThread.ExecAsync(Me, Sub() ToolStripStatusLabel_Progress.Text = "Loading servers...")

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

                                                                     lFtpEntries.Add(New STRUC_FTP_ENTRY_ITEM(sHost, sDatabaseEntry, sSourceModPath, iProtocolType))
                                                                 Next
                                                             End Using

                                                             ClassThread.ExecAsync(Me, Sub() ToolStripStatusLabel_Progress.Text = "Loading settings...")

                                                             'Load Settings
                                                             Using mIni As New ClassIni(g_mSettingsSecureStorage.m_String(System.Text.Encoding.Default))
                                                                 Dim iMaxFileSize As Integer = 0
                                                                 If (Integer.TryParse(mIni.ReadKeyValue("Settings", "MaxFileSize", "100"), iMaxFileSize)) Then
                                                                     iMaxFileBytes = (iMaxFileSize * 1024 * 1024)
                                                                 End If
                                                             End Using

                                                             ClassThread.ExecAsync(Me, Sub() ToolStripStatusLabel_Progress.Text = "Fetching reports...")

                                                             'Fetch Reports
                                                             For Each mFtpItem In lFtpEntries
                                                                 Dim g_mClassFTP As ClassFTP = Nothing
                                                                 Dim g_mClassSFTP As Renci.SshNet.SftpClient = Nothing

                                                                 Dim sLogDirectory As String = IO.Path.Combine(mFtpItem.sSourceModPath, "logs").Replace("\", "/")

                                                                 Try
                                                                     Dim mDatabaseItem = ClassDatabase.FindDatabaseItemByName(mFtpItem.sDatabaseEntry)
                                                                     If (mDatabaseItem Is Nothing) Then
                                                                         Throw New ArgumentException(String.Format("Unable to find database entry: {0}", mFtpItem.sDatabaseEntry))
                                                                     End If

                                                                     Select Case (mFtpItem.iProtocolType)
                                                                         Case ENUM_FTP_PROTOCOL_TYPE.FTP
                                                                             g_mClassFTP = New ClassFTP(mFtpItem.sHost, mDatabaseItem.m_Username, mDatabaseItem.m_Password)

                                                                             If (Not g_mClassFTP.PathExist(sLogDirectory)) Then
                                                                                 Throw New ArgumentException(String.Format("Could not find SourceMod 'logs' directory on: {0}/{1}", mFtpItem.sHost.TrimEnd("/"c), sLogDirectory.TrimStart("/"c)))
                                                                             End If

                                                                             For Each mItem In g_mClassFTP.GetDirectoryEntries(sLogDirectory)
                                                                                 Select Case (mItem.sName)
                                                                                     Case ".", ".."
                                                                                         Continue For
                                                                                 End Select

                                                                                 If (mItem.bIsDirectory) Then
                                                                                     Continue For
                                                                                 End If

                                                                                 Dim sFileExt As String = IO.Path.GetExtension(mItem.sName)
                                                                                 Dim sFileFullName As String = IO.Path.GetFileName(mItem.sName)

                                                                                 If (sFileExt.ToLower <> ".log" OrElse Not sFileFullName.ToLower.StartsWith("errors_")) Then
                                                                                     Continue For
                                                                                 End If

                                                                                 If (mItem.iSize > iMaxFileBytes) Then
                                                                                     bFilesTooBig = True
                                                                                     Continue For
                                                                                 End If

                                                                                 Dim sTmpFile As String = IO.Path.GetTempFileName
                                                                                 Try
                                                                                     g_mClassFTP.DownloadFile(mItem.sFullName, sTmpFile)

                                                                                     With New ClassDebuggerParser(Nothing)
                                                                                         lReportExceptionItems.AddRange(.ReadSourceModLogExceptions(IO.File.ReadAllLines(sTmpFile)))
                                                                                     End With
                                                                                 Finally
                                                                                     IO.File.Delete(sTmpFile)
                                                                                 End Try
                                                                             Next


                                                                         Case ENUM_FTP_PROTOCOL_TYPE.SFTP
                                                                             g_mClassSFTP = New Renci.SshNet.SftpClient(mFtpItem.sHost, mDatabaseItem.m_Username, mDatabaseItem.m_Password)

                                                                             If (Not g_mClassSFTP.IsConnected) Then
                                                                                 g_mClassSFTP.Connect()
                                                                             End If

                                                                             If (Not g_mClassSFTP.Exists(sLogDirectory)) Then
                                                                                 Throw New ArgumentException(String.Format("Could not find SourceMod 'logs' directory on: {0}/{1}", mFtpItem.sHost.TrimEnd("/"c), sLogDirectory.TrimStart("/"c)))
                                                                             End If

                                                                             For Each mItem In g_mClassSFTP.ListDirectory(sLogDirectory)
                                                                                 Select Case (mItem.Name)
                                                                                     Case ".", ".."
                                                                                         Continue For
                                                                                 End Select

                                                                                 If (mItem.IsDirectory) Then
                                                                                     Continue For
                                                                                 End If

                                                                                 Dim sFileExt As String = IO.Path.GetExtension(mItem.Name)
                                                                                 Dim sFileFullName As String = IO.Path.GetFileName(mItem.Name)

                                                                                 If (sFileExt.ToLower <> ".log" OrElse Not sFileFullName.ToLower.StartsWith("errors_")) Then
                                                                                     Continue For
                                                                                 End If

                                                                                 If (mItem.Length > iMaxFileBytes) Then
                                                                                     bFilesTooBig = True
                                                                                     Continue For
                                                                                 End If

                                                                                 Dim sTmpFile As String = IO.Path.GetTempFileName
                                                                                 Try
                                                                                     Using mStream As New IO.FileStream(sTmpFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                                                                                         g_mClassSFTP.DownloadFile(mItem.FullName, mStream)
                                                                                     End Using

                                                                                     With New ClassDebuggerParser(Nothing)
                                                                                         lReportExceptionItems.AddRange(.ReadSourceModLogExceptions(IO.File.ReadAllLines(sTmpFile)))
                                                                                     End With
                                                                                 Finally
                                                                                     IO.File.Delete(sTmpFile)
                                                                                 End Try
                                                                             Next

                                                                         Case Else
                                                                             Throw New ArgumentException("Unknown connection type")
                                                                     End Select
                                                                 Catch ex As Threading.ThreadAbortException
                                                                     Throw
                                                                 Catch ex As Exception
                                                                     lReportItems.Add({"Error", ex.Message, My.Resources.user32_103_16x16_32})
                                                                 Finally
                                                                     If (g_mClassSFTP IsNot Nothing) Then
                                                                         g_mClassSFTP.Dispose()
                                                                         g_mClassSFTP = Nothing
                                                                     End If
                                                                 End Try
                                                             Next

                                                             If (bFilesTooBig) Then
                                                                 lReportItems.Add({"Unable to fetch some reports", String.Format("Some reports are too big too fetch. (max. {0} MB)", iMaxFileBytes / 1024 / 1024), My.Resources.user32_101_16x16_32})
                                                             End If

                                                             If (lReportExceptionItems.Count < 1) Then
                                                                 lReportItems.Add({"No error reports found", "Congratulations! No error reports have been found!", My.Resources.ieframe_36866_16x16_32})
                                                             End If

                                                             ClassThread.ExecAsync(Me, Sub() ToolStripStatusLabel_Progress.Text = "Removing duplicated reports...")

                                                             'Remove duplicates
                                                             Dim lTmpList As New List(Of ClassDebuggerParser.STRUC_SM_EXCEPTION)
                                                             For Each mItem In lReportExceptionItems.ToArray
                                                                 If (lTmpList.Exists(Function(x As ClassDebuggerParser.STRUC_SM_EXCEPTION)
                                                                                         If (x.sExceptionInfo <> mItem.sExceptionInfo) Then
                                                                                             Return False
                                                                                         End If

                                                                                         If (x.sBlamingFile <> mItem.sBlamingFile) Then
                                                                                             Return False
                                                                                         End If

                                                                                         If (x.dLogDate <> mItem.dLogDate) Then
                                                                                             Return False
                                                                                         End If

                                                                                         If (x.mStackTraces.Length <> mItem.mStackTraces.Length) Then
                                                                                             Return False
                                                                                         End If

                                                                                         For i = 0 To x.mStackTraces.Length - 1
                                                                                             If (x.mStackTraces(i).sFunctionName <> mItem.mStackTraces(i).sFunctionName) Then
                                                                                                 Return False
                                                                                             End If

                                                                                             If (x.mStackTraces(i).iLine <> mItem.mStackTraces(i).iLine) Then
                                                                                                 Return False
                                                                                             End If

                                                                                             If (x.mStackTraces(i).sFileName <> mItem.mStackTraces(i).sFileName) Then
                                                                                                 Return False
                                                                                             End If

                                                                                             If (x.mStackTraces(i).bNativeFault <> mItem.mStackTraces(i).bNativeFault) Then
                                                                                                 Return False
                                                                                             End If
                                                                                         Next

                                                                                         Return True
                                                                                     End Function)) Then
                                                                     Continue For
                                                                 End If

                                                                 lTmpList.Add(mItem)
                                                             Next
                                                             lReportExceptionItems.Clear()
                                                             lReportExceptionItems.AddRange(lTmpList)

                                                             ClassThread.ExecAsync(Me, Sub() ToolStripStatusLabel_Progress.Text = "Sorting reports...")

                                                             'Sort all reports. Top = Newer.
                                                             lReportExceptionItems.Sort(Function(x As ClassDebuggerParser.STRUC_SM_EXCEPTION, y As ClassDebuggerParser.STRUC_SM_EXCEPTION)
                                                                                            Return -(x.dLogDate.CompareTo(y.dLogDate))
                                                                                        End Function)

                                                             ClassThread.ExecAsync(Me, Sub()
                                                                                           ToolStripStatusLabel_Progress.Text = "Listing reports..."

                                                                                           UcReportList_Reports.SuspendLayout()

                                                                                           CleanReports()

                                                                                           Const iDisplayLimit = 500
                                                                                           Dim iLimit As Integer = iDisplayLimit

                                                                                           For Each mItem In lReportExceptionItems
                                                                                               iLimit -= 1

                                                                                               If (iLimit < 0) Then
                                                                                                   lReportItems.Add({"Unable to display more reports", String.Format("You can not display more than {0} reports at once", iDisplayLimit), My.Resources.user32_101_16x16_32})
                                                                                                   Exit For
                                                                                               End If

                                                                                               With New UCReportExceptionItem(Me, mItem)
                                                                                                   .SuspendLayout()

                                                                                                   .Parent = UcReportList_Reports
                                                                                                   .Dock = DockStyle.Top
                                                                                                   .BringToFront()
                                                                                                   .Show()

                                                                                                   .ResumeLayout()
                                                                                               End With
                                                                                           Next

                                                                                           For Each mItem In lReportItems
                                                                                               With New UCReportItem(Me, CStr(mItem(0)), CStr(mItem(1)), "", CType(mItem(2), Image))
                                                                                                   .SuspendLayout()

                                                                                                   .Parent = UcReportList_Reports
                                                                                                   .Dock = DockStyle.Top
                                                                                                   .SendToBack()
                                                                                                   .Show()

                                                                                                   .ResumeLayout()
                                                                                               End With
                                                                                           Next

                                                                                           UcReportList_Reports.ResumeLayout()
                                                                                       End Sub)

                                                             ClassThread.ExecAsync(Me, Sub()
                                                                                           ToolStripProgressBar_Progress.Visible = False
                                                                                           ToolStripStatusLabel_Progress.Visible = False
                                                                                           ToolStripSplitButton_ProgressAbort.Visible = False
                                                                                       End Sub)
                                                         Catch ex As Threading.ThreadAbortException
                                                             Throw
                                                         Catch ex As Exception
                                                             ClassExceptionLog.WriteToLogMessageBox(ex)

                                                             ClassThread.ExecAsync(Me, Sub()
                                                                                           ToolStripProgressBar_Progress.Visible = False
                                                                                           ToolStripStatusLabel_Progress.Visible = False
                                                                                           ToolStripSplitButton_ProgressAbort.Visible = False
                                                                                       End Sub)
                                                         End Try
                                                     End Sub) With {
            .IsBackground = True
        }
        g_mFetchReportsThread.Start()
    End Sub

    Public Sub CleanReports()
        Me.SuspendLayout()

        For Each mControl As UCReportItem In GetItems()
            mControl.Dispose()
        Next

        Me.ResumeLayout()
    End Sub

    Public Function GetItems() As UCReportItem()
        Dim lItemList As New List(Of UCReportItem)

        For Each mControl As Control In UcReportList_Reports.Controls
            If (TypeOf mControl Is UCReportItem) Then
                lItemList.Add(DirectCast(mControl, UCReportItem))
            End If
        Next

        Return lItemList.ToArray
    End Function

    Private Sub ToolStripSplitButton_ProgressAbort_ButtonClick(sender As Object, e As EventArgs) Handles ToolStripSplitButton_ProgressAbort.ButtonClick
        ClassThread.Abort(g_mFetchReportsThread)

        ToolStripProgressBar_Progress.Visible = False
        ToolStripStatusLabel_Progress.Visible = False
        ToolStripSplitButton_ProgressAbort.Visible = False
    End Sub

    Private Sub FormReportManager_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub CleanUp()
        ClassThread.Abort(g_mFetchReportsThread)
    End Sub
End Class