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


Imports System.Drawing
Imports System.Text
Imports System.Text.RegularExpressions

Public Class FormReportManager
    Public Shared ReadOnly g_sAcceleratorServer As String = "https://crash.limetech.org/"

    Private g_mPluginConfigFtp As ClassPluginController.ClassPluginConfig
    Private g_mPluginConfigSettings As ClassPluginController.ClassPluginConfig

    Private g_mClassTreeViewColumns As ClassTreeViewColumns

    Private g_mClassReports As ClassReports
    Private g_mClassLogs As ClassLogs

    Public g_mPluginAutoErrorReport As PluginAutoErrorReport

    Public g_sGetReportsOrginalText As String = ""
    Public g_sGetReportsOrginalImage As Image
    Public g_sGetLogsOrginalText As String = ""
    Public g_sGetLogsOrginalImage As Image

    Const ICON_FILE = 0
    Const ICON_WARN = 1
    Const ICON_ERROR = 2
    Const ICON_ARROW = 3

    Const ERROR_NOERROR = 0
    Const ERROR_MSGONLY = 1
    Const ERROR_TOOBIG = 2

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
        g_mClassTreeViewColumns = New ClassTreeViewColumns
        g_mClassTreeViewColumns.m_TreeView.ImageList = ImageList_Logs
        g_mClassTreeViewColumns.m_Columns.Add("File", 400)
        g_mClassTreeViewColumns.m_Columns.Add("Size", 100)
        g_mClassTreeViewColumns.m_Columns.Add("Date", 100)
        g_mClassTreeViewColumns.Dock = DockStyle.Fill
        g_mClassTreeViewColumns.Parent = TabPage_Logs

        AddHandler g_mClassTreeViewColumns.m_TreeView.DoubleClick, AddressOf ClassTreeViewColumns_DoubleClick

        ImageList_Logs.Images.Clear()
        ImageList_Logs.Images.Add(CStr(ICON_FILE), My.Resources.imageres_5304_16x16_32)
        ImageList_Logs.Images.Add(CStr(ICON_WARN), My.Resources.user32_101_16x16_32)
        ImageList_Logs.Images.Add(CStr(ICON_ERROR), My.Resources.user32_103_16x16_32)
        ImageList_Logs.Images.Add(CStr(ICON_ARROW), My.Resources.netshell_1607_16x16_32)

        g_sGetReportsOrginalText = ToolStripMenuItem_GetReports.Text
        g_sGetReportsOrginalImage = ToolStripMenuItem_GetReports.Image
        g_sGetLogsOrginalText = ToolStripMenuItem_GetLogs.Text
        g_sGetLogsOrginalImage = ToolStripMenuItem_GetLogs.Image

        'Does not write only read
        g_mPluginConfigFtp = New ClassPluginController.ClassPluginConfig("PluginAutoErrorReportFtpEntries")
        g_mPluginConfigSettings = New ClassPluginController.ClassPluginConfig("PluginAutoErrorReportSettings")

        g_mClassReports = New ClassReports(Me)
        g_mClassLogs = New ClassLogs(Me)
    End Sub

    Private Sub FormReportManager_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClassControlStyle.UpdateControls(Me)

        Try
            g_mClassReports.FetchReports()
            g_mClassLogs.FetchLogs()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub GetReportsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_GetReports.Click
        Try
            If (g_mClassReports.IsFetchingReports) Then
                g_mClassReports.AbortFetching()
            Else
                g_mClassReports.FetchReports()
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub


    Private Sub ToolStripMenuItem_GetLogs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_GetLogs.Click
        Try
            If (g_mClassLogs.IsFetchingLogs) Then
                g_mClassLogs.AbortFetching()
            Else
                g_mClassLogs.FetchLogs()
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub CloseReportWindowsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CloseReportWindows.Click
        g_mClassReports.CloseReportForms()
    End Sub

    Private Sub ReportListBox_Reports_DoubleClick(sender As Object, e As EventArgs) Handles ReportListBox_Reports.DoubleClick
        If (ReportListBox_Reports.SelectedItems.Count < 1) Then
            Return
        End If

        Dim mReportItem = TryCast(ReportListBox_Reports.SelectedItems(0), ClassReportListBox.ClassReportItem)
        If (mReportItem Is Nothing) Then
            Return
        End If

        If (mReportItem.m_IReport Is Nothing) Then
            Return
        End If

        mReportItem.m_IReport.OnClicked(Me)
    End Sub

    Private Sub ClassTreeViewColumns_DoubleClick(sender As Object, e As EventArgs)
        Try
            If (g_mClassTreeViewColumns.m_TreeView.SelectedNode Is Nothing) Then
                Return
            End If

            Dim mTreeNodeData = TryCast(g_mClassTreeViewColumns.m_TreeView.SelectedNode, ClassTreeNodeData)
            If (mTreeNodeData Is Nothing) Then
                Return
            End If

            Dim sTitle As String = CStr(mTreeNodeData.g_mData("Title"))
            Dim sRemoteFile As String = CStr(mTreeNodeData.g_mData("RemoteFile"))
            Dim sLocalFile As String = CStr(mTreeNodeData.g_mData("LocalFile"))
            Dim iDate As Date = New Date(CLng(mTreeNodeData.g_mData("DateTick")))
            Dim iSize As Long = CLng(mTreeNodeData.g_mData("Size"))
            Dim iErrorIndex As Integer = CInt(mTreeNodeData.g_mData("ErrorIndex"))

            Select Case (iErrorIndex)
                Case ERROR_NOERROR
                    If (String.IsNullOrEmpty(sLocalFile) OrElse Not IO.File.Exists(sLocalFile)) Then
                        Throw New ArgumentException("Unable to open. Could not find local file.")
                    End If

                    Process.Start("notepad.exe", sLocalFile)

                Case ERROR_MSGONLY
                    Return

                Case ERROR_TOOBIG
                    Throw New ArgumentException("Unable to open. This file is too big to fetch.")

            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Class ClassReports
        Implements IDisposable

        Public g_mFormReportManager As FormReportManager

        Private g_mFetchReportsThread As Threading.Thread

        Public Sub New(mFormReportManager As FormReportManager)
            g_mFormReportManager = mFormReportManager
        End Sub

        Public Sub FetchReports()
            If (ClassThread.IsValid(g_mFetchReportsThread)) Then
                Return
            End If

            'Put here because file race condition
            g_mFormReportManager.g_mPluginConfigFtp.LoadConfig()
            g_mFormReportManager.g_mPluginConfigSettings.LoadConfig()

            Dim sConfigFtpContent As String = g_mFormReportManager.g_mPluginConfigFtp.ExportToString
            Dim sConfigSettingsContent As String = g_mFormReportManager.g_mPluginConfigSettings.ExportToString

            g_mFetchReportsThread = New Threading.Thread(Sub()
                                                             Try
                                                                 ClassThread.ExecAsync(g_mFormReportManager, Sub()
                                                                                                                 g_mFormReportManager.ToolStripMenuItem_GetReports.Text = "Abort fetching reports"
                                                                                                                 g_mFormReportManager.ToolStripMenuItem_GetReports.Image = My.Resources.imageres_5337_16x16_32
                                                                                                             End Sub)

                                                                 Const C_REPORTITEMS_TITLE = "Title"
                                                                 Const C_REPORTITEMS_MESSAGE = "Message"
                                                                 Const C_REPORTITEMS_IMAGE = "Image"

                                                                 Dim lReportItems As New List(Of Dictionary(Of String, Object)) 'See keys: C_REPORTITEMS_*
                                                                 Dim lReportExceptionItems As New List(Of ClassReportItems.IReportInterface)
                                                                 Dim lFtpEntries As New List(Of STRUC_FTP_ENTRY_ITEM)

                                                                 Dim iMaxFileBytes As Long = (100 * 1024 * 1024)
                                                                 Dim bFilesTooBig As Boolean = False

                                                                 'Load Servers  
                                                                 Using mIni As New ClassIni(sConfigFtpContent)
                                                                     For Each sSection As String In mIni.GetSectionNames
                                                                         Dim sHost As String = mIni.ReadKeyValue(sSection, "Host", Nothing)
                                                                         Dim sDatabaseEntry As String = mIni.ReadKeyValue(sSection, "DatabaseEntry", Nothing)
                                                                         Dim sSourceModPath As String = mIni.ReadKeyValue(sSection, "DestinationPath", Nothing)
                                                                         Dim sProtocol As String = mIni.ReadKeyValue(sSection, "Protocol", "FTP")

                                                                         If (String.IsNullOrEmpty(sHost) OrElse String.IsNullOrEmpty(sDatabaseEntry) OrElse String.IsNullOrEmpty(sSourceModPath)) Then
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

                                                                 'Load Settings 
                                                                 Using mIni As New ClassIni(sConfigSettingsContent)
                                                                     Dim iMaxFileSize As Integer = 0
                                                                     If (Integer.TryParse(mIni.ReadKeyValue("Settings", "MaxFileSize", "100"), iMaxFileSize)) Then
                                                                         iMaxFileBytes = (iMaxFileSize * 1024 * 1024)
                                                                     End If
                                                                 End Using

                                                                 Dim mFtpCacheDic As New Dictionary(Of String, Object)
                                                                 Dim mClassFTP As ClassFTP = Nothing
                                                                 Dim mClassSFTP As Renci.SshNet.SftpClient = Nothing

                                                                 Try
                                                                     'Fetch Reports
                                                                     For Each mFtpItem In lFtpEntries
                                                                         Dim sLogDirectory As String = IO.Path.Combine(mFtpItem.sSourceModPath, "logs").Replace("\", "/")

                                                                         Try
                                                                             Dim mDatabaseItem = ClassDatabase.FindDatabaseItemByName(mFtpItem.sDatabaseEntry)
                                                                             If (mDatabaseItem Is Nothing) Then
                                                                                 Throw New ArgumentException(String.Format("Unable to find database entry: {0}", mFtpItem.sDatabaseEntry))
                                                                             End If

                                                                             Dim sSafeFtpCacheDicKey As String = ClassTools.ClassStrings.ToSafeKey(mFtpItem.sHost & vbLf & mDatabaseItem.m_Username)

                                                                             Select Case (mFtpItem.iProtocolType)
                                                                                 Case ENUM_FTP_PROTOCOL_TYPE.FTP
                                                                                     If (mFtpCacheDic.ContainsKey(sSafeFtpCacheDicKey)) Then
                                                                                         mClassFTP = DirectCast(mFtpCacheDic(sSafeFtpCacheDicKey), ClassFTP)
                                                                                     Else
                                                                                         mClassFTP = New ClassFTP(mFtpItem.sHost, mDatabaseItem.m_Username, mDatabaseItem.m_Password)
                                                                                         mFtpCacheDic(sSafeFtpCacheDicKey) = mClassFTP
                                                                                     End If

                                                                                     If (Not mClassFTP.PathExist(sLogDirectory)) Then
                                                                                         Throw New ArgumentException(String.Format("Could not find SourceMod 'logs' directory on: {0}/{1}", mFtpItem.sHost.TrimEnd("/"c), sLogDirectory.TrimStart("/"c)))
                                                                                     End If

                                                                                     For Each mItem In mClassFTP.GetDirectoryEntries(sLogDirectory)
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
                                                                                             mClassFTP.DownloadFile(mItem.sFullName, sTmpFile)

                                                                                             For Each mModule In ClassReportItems.GetModules
                                                                                                 lReportExceptionItems.AddRange(mModule.ParseFromFile(sTmpFile))
                                                                                             Next
                                                                                         Finally
                                                                                             IO.File.Delete(sTmpFile)
                                                                                         End Try
                                                                                     Next


                                                                                 Case ENUM_FTP_PROTOCOL_TYPE.SFTP
                                                                                     If (mFtpCacheDic.ContainsKey(sSafeFtpCacheDicKey)) Then
                                                                                         mClassSFTP = DirectCast(mFtpCacheDic(sSafeFtpCacheDicKey), Renci.SshNet.SftpClient)
                                                                                     Else
                                                                                         mClassSFTP = New Renci.SshNet.SftpClient(mFtpItem.sHost, mDatabaseItem.m_Username, mDatabaseItem.m_Password)
                                                                                         mFtpCacheDic(sSafeFtpCacheDicKey) = mClassSFTP
                                                                                     End If

                                                                                     If (Not mClassSFTP.IsConnected) Then
                                                                                         mClassSFTP.Connect()
                                                                                     End If

                                                                                     If (Not mClassSFTP.Exists(sLogDirectory)) Then
                                                                                         Throw New ArgumentException(String.Format("Could not find SourceMod 'logs' directory on: {0}/{1}", mFtpItem.sHost.TrimEnd("/"c), sLogDirectory.TrimStart("/"c)))
                                                                                     End If

                                                                                     For Each mItem In mClassSFTP.ListDirectory(sLogDirectory)
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
                                                                                                 mClassSFTP.DownloadFile(mItem.FullName, mStream)
                                                                                             End Using

                                                                                             For Each mModule In ClassReportItems.GetModules
                                                                                                 lReportExceptionItems.AddRange(mModule.ParseFromFile(sTmpFile))
                                                                                             Next
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
                                                                             Dim mReportItemsDic As New Dictionary(Of String, Object)
                                                                             mReportItemsDic(C_REPORTITEMS_TITLE) = "Error"
                                                                             mReportItemsDic(C_REPORTITEMS_MESSAGE) = ex.Message
                                                                             mReportItemsDic(C_REPORTITEMS_IMAGE) = My.Resources.user32_103_16x16_32

                                                                             lReportItems.Add(mReportItemsDic)
                                                                         End Try
                                                                     Next
                                                                 Finally
                                                                     For Each mItem In mFtpCacheDic
                                                                         Select Case (True)
                                                                             Case TypeOf mItem.Value Is ClassFTP
                                                                                 'Nothing to dispose

                                                                             Case TypeOf mItem.Value Is Renci.SshNet.SftpClient
                                                                                 mClassSFTP = DirectCast(mItem.Value, Renci.SshNet.SftpClient)
                                                                                 If (mClassSFTP IsNot Nothing) Then
                                                                                     mClassSFTP.Dispose()
                                                                                     mClassSFTP = Nothing
                                                                                 End If
                                                                         End Select
                                                                     Next

                                                                     mFtpCacheDic = Nothing
                                                                 End Try

                                                                 If (bFilesTooBig) Then
                                                                     Dim mReportItemsDic As New Dictionary(Of String, Object)
                                                                     mReportItemsDic(C_REPORTITEMS_TITLE) = "Unable to fetch some reports"
                                                                     mReportItemsDic(C_REPORTITEMS_MESSAGE) = String.Format("Some reports are too big to fetch. (max. {0} MB)", iMaxFileBytes / 1024 / 1024)
                                                                     mReportItemsDic(C_REPORTITEMS_IMAGE) = My.Resources.user32_101_16x16_32

                                                                     lReportItems.Add(mReportItemsDic)
                                                                 End If

                                                                 If (lReportExceptionItems.Count < 1) Then
                                                                     Dim mReportItemsDic As New Dictionary(Of String, Object)
                                                                     mReportItemsDic(C_REPORTITEMS_TITLE) = "No error reports found"
                                                                     mReportItemsDic(C_REPORTITEMS_MESSAGE) = "Congratulations! No error reports have been found!"
                                                                     mReportItemsDic(C_REPORTITEMS_IMAGE) = My.Resources.ieframe_36866_16x16_32

                                                                     lReportItems.Add(mReportItemsDic)
                                                                 End If

                                                                 'Remove duplicates
                                                                 Dim lTmpList As New List(Of ClassReportItems.IReportInterface)
                                                                 For Each mItem In lReportExceptionItems.ToArray
                                                                     If (lTmpList.Exists(Function(x As ClassReportItems.IReportInterface)
                                                                                             Return x.Equal(mItem)
                                                                                         End Function)) Then
                                                                         Continue For
                                                                     End If

                                                                     lTmpList.Add(mItem)
                                                                 Next
                                                                 lReportExceptionItems.Clear()
                                                                 lReportExceptionItems.AddRange(lTmpList)

                                                                 'Sort all reports. Top = Newer.
                                                                 lReportExceptionItems.Sort(Function(x As ClassReportItems.IReportInterface, y As ClassReportItems.IReportInterface)
                                                                                                Return -(x.LogDate.CompareTo(y.LogDate))
                                                                                            End Function)

                                                                 ClassThread.ExecAsync(g_mFormReportManager, Sub()
                                                                                                                 Try
                                                                                                                     g_mFormReportManager.ReportListBox_Reports.BeginUpdate()
                                                                                                                     g_mFormReportManager.ReportListBox_Reports.Items.Clear()

                                                                                                                     For Each mItem In lReportExceptionItems
                                                                                                                         g_mFormReportManager.ReportListBox_Reports.Items.Add(mItem.ToReport)
                                                                                                                     Next

                                                                                                                     For Each mReportItemsDic In lReportItems
                                                                                                                         Dim sTitle As String = CStr(mReportItemsDic(C_REPORTITEMS_TITLE))
                                                                                                                         Dim sMessage As String = CStr(mReportItemsDic(C_REPORTITEMS_MESSAGE))
                                                                                                                         Dim mImage As Image = CType(mReportItemsDic(C_REPORTITEMS_IMAGE), Image)

                                                                                                                         g_mFormReportManager.ReportListBox_Reports.Items.Add(New ClassReportListBox.ClassReportItem(sTitle, sMessage, "", mImage, Nothing))
                                                                                                                     Next
                                                                                                                 Finally
                                                                                                                     g_mFormReportManager.ReportListBox_Reports.EndUpdate()
                                                                                                                 End Try
                                                                                                             End Sub)

                                                                 ClassThread.ExecAsync(g_mFormReportManager, Sub()
                                                                                                                 g_mFormReportManager.ToolStripMenuItem_GetReports.Text = g_mFormReportManager.g_sGetReportsOrginalText
                                                                                                                 g_mFormReportManager.ToolStripMenuItem_GetReports.Image = g_mFormReportManager.g_sGetReportsOrginalImage
                                                                                                             End Sub)
                                                             Catch ex As Threading.ThreadAbortException
                                                                 Throw
                                                             Catch ex As Exception
                                                                 ClassExceptionLog.WriteToLogMessageBox(ex)

                                                                 ClassThread.ExecAsync(g_mFormReportManager, Sub()
                                                                                                                 g_mFormReportManager.ToolStripMenuItem_GetReports.Text = g_mFormReportManager.g_sGetReportsOrginalText
                                                                                                                 g_mFormReportManager.ToolStripMenuItem_GetReports.Image = g_mFormReportManager.g_sGetReportsOrginalImage
                                                                                                             End Sub)
                                                             End Try
                                                         End Sub) With {
                .IsBackground = True
            }
            g_mFetchReportsThread.Start()
        End Sub

        Public Function IsFetchingReports() As Boolean
            Return ClassThread.IsValid(g_mFetchReportsThread)
        End Function

        Public Sub AbortFetching()
            ClassThread.Abort(g_mFetchReportsThread)

            g_mFormReportManager.ToolStripMenuItem_GetReports.Text = g_mFormReportManager.g_sGetReportsOrginalText
            g_mFormReportManager.ToolStripMenuItem_GetReports.Image = g_mFormReportManager.g_sGetReportsOrginalImage
        End Sub

        Public Sub CloseReportForms()
            Dim lForms As New List(Of Form)

            For Each mForm As Form In Application.OpenForms
                If (TypeOf mForm Is FormReportDetails) Then
                    lForms.Add(mForm)
                End If
            Next

            For Each mForm In lForms
                mForm.Close()
            Next
        End Sub

        Class ClassReportItems
            Interface IReaderInterface
                Function ParseFromFile(sText As String) As IReportInterface()
            End Interface

            Interface IReportInterface
                Function ToReport() As ClassReportListBox.ClassReportItem

                Function Equal(other As IReportInterface) As Boolean

                Function LogDate() As Date

                Function ToString() As String

                Sub OnClicked(mFormReportManager As Object)
            End Interface

            Public Shared Function GetModules() As IReaderInterface()
                Return {
                    New ClassExceptionReader,
                    New ClassAcceleratorReader
                }
            End Function

            Class ClassExceptionReader
                Implements IReaderInterface

                Public Function ParseFromFile(sFile As String) As IReportInterface() Implements IReaderInterface.ParseFromFile
                    Dim mItems As New List(Of IReportInterface)

                    For Each mItem In ClassDebuggerTools.ClassDebuggerHelpers.ReadSourceModLogExceptions(IO.File.ReadAllLines(sFile))
                        mItems.Add(New ClassItem(mItem))
                    Next

                    Return mItems.ToArray
                End Function

                Class ClassItem
                    Implements IReportInterface

                    Private g_mException As ClassDebuggerTools.STRUC_SM_EXCEPTION

                    Public Sub New(_Exception As ClassDebuggerTools.STRUC_SM_EXCEPTION)
                        g_mException = _Exception
                    End Sub

                    Public Function ToReport() As ClassReportListBox.ClassReportItem Implements IReportInterface.ToReport
                        Dim sTitle = g_mException.sExceptionInfo
                        Dim sText = g_mException.sBlamingFile

                        Dim sDate As String
                        If (g_mException.dLogDate.ToShortDateString = Now.ToShortDateString) Then
                            sDate = g_mException.dLogDate.ToShortTimeString
                        Else
                            sDate = g_mException.dLogDate.ToLongDateString
                        End If

                        Return New ClassReportListBox.ClassReportItem(sTitle, sText, sDate, My.Resources.ieframe_36883_16x16_32, Me)
                    End Function

                    Public Function Equal(other As IReportInterface) As Boolean Implements IReportInterface.Equal
                        Return ToString.ToLower = other.ToString.ToLower
                    End Function

                    Public Function LogDate() As Date Implements IReportInterface.LogDate
                        Return g_mException.dLogDate
                    End Function

                    Public Function IReportInterface_ToString() As String Implements IReportInterface.ToString
                        Dim sException As New StringBuilder

                        sException.AppendFormat(String.Format("Exception Info: {0}", g_mException.sExceptionInfo)).AppendLine()
                        sException.AppendFormat(String.Format("Blaming File: {0}", g_mException.sBlamingFile)).AppendLine()
                        sException.AppendFormat(String.Format("Date: {0}", g_mException.dLogDate.ToString)).AppendLine()
                        sException.AppendFormat(String.Format("Stack Traces: {0}", CStr(g_mException.mStackTraces.Length))).AppendLine()

                        For i = 0 To g_mException.mStackTraces.Length - 1
                            sException.AppendFormat("[{0}] Function Name: {1}", i, g_mException.mStackTraces(i).sFunctionName).AppendLine()
                            sException.AppendFormat("[{0}] Line: {1}", i, CStr(g_mException.mStackTraces(i).iLine)).AppendLine()
                            sException.AppendFormat("[{0}] Filename: {1}", i, g_mException.mStackTraces(i).sFileName).AppendLine()
                            sException.AppendFormat("[{0}] Is Native: {1}", i, If(g_mException.mStackTraces(i).bNativeFault, "true", "false")).AppendLine()
                        Next

                        Return sException.ToString
                    End Function

                    Public Sub OnClicked(_FormReportManager As Object) Implements IReportInterface.OnClicked
                        Dim mFormReportManager = TryCast(_FormReportManager, FormReportManager)
                        If (mFormReportManager Is Nothing) Then
                            Return
                        End If

                        Dim iCount = 0
                        For Each mForm As Form In Application.OpenForms
                            If (TypeOf mForm Is FormReportDetails) Then
                                iCount += 1
                            End If
                        Next

                        If (iCount > 100) Then
                            MessageBox.Show("Too many 'Report' windows open!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Else
                            Call (New FormReportDetails(mFormReportManager, g_mException)).Show()
                        End If
                    End Sub
                End Class
            End Class

            Class ClassAcceleratorReader
                Implements IReaderInterface

                Public Structure STRUC_ACC_CRASH
                    Public sCrashId As String
                    Public dLogDate As Date
                End Structure

                Public Function ParseFromFile(sFile As String) As IReportInterface() Implements IReaderInterface.ParseFromFile
                    Dim mItems As New List(Of IReportInterface)

                    For Each mItem In ReadSourceModAcceleratorCrash(IO.File.ReadAllLines(sFile))
                        mItems.Add(New ClassItem(mItem))
                    Next

                    Return mItems.ToArray
                End Function

                Private Function ReadSourceModAcceleratorCrash(sLogLines As String()) As STRUC_ACC_CRASH()
                    Dim lAccCrashes As New List(Of STRUC_ACC_CRASH)

                    For i = 0 To sLogLines.Length - 1
                        'Ignore empty lines
                        If (sLogLines(i).Trim.Length < 1) Then
                            Continue For
                        End If

                        Dim mCrashInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+ \- [0-9]+\:[0-9]+\:[0-9]+)\: \[CRASH\] Accelerator uploaded crash dump\:\s+Crash ID\:\s+(?<CrashId>[-a-zA-Z0-9]+)$")
                        If (mCrashInfo.Success) Then
                            Dim sDate As String = mCrashInfo.Groups("Date").Value
                            Dim sCrashId As String = mCrashInfo.Groups("CrashId").Value

                            Dim dDate As Date
                            If (Not Date.TryParseExact(sDate.Trim, "MM/dd/yyyy - HH:mm:ss", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, dDate)) Then
                                Continue For
                            End If

                            lAccCrashes.Add(New STRUC_ACC_CRASH With {
                                .sCrashId = sCrashId,
                                .dLogDate = dDate
                            })
                            Continue For
                        End If
                    Next

                    Return lAccCrashes.ToArray
                End Function

                Class ClassItem
                    Implements IReportInterface

                    Private g_mCrashId As STRUC_ACC_CRASH

                    Public Sub New(_CrashId As STRUC_ACC_CRASH)
                        g_mCrashId = _CrashId
                    End Sub

                    Public Function ToReport() As ClassReportListBox.ClassReportItem Implements IReportInterface.ToReport
                        Dim sDate As String
                        If (g_mCrashId.dLogDate.ToShortDateString = Now.ToShortDateString) Then
                            sDate = g_mCrashId.dLogDate.ToShortTimeString
                        Else
                            sDate = g_mCrashId.dLogDate.ToLongDateString
                        End If

                        Return New ClassReportListBox.ClassReportItem("Server Crash", String.Format("Accelerator uploaded a crash dump: {0}", g_mCrashId.sCrashId.ToUpper), sDate, My.Resources.ieframe_36883_16x16_32, Me)
                    End Function

                    Public Function Equal(other As IReportInterface) As Boolean Implements IReportInterface.Equal
                        Return ToString.ToLower = other.ToString.ToLower
                    End Function

                    Public Function LogDate() As Date Implements IReportInterface.LogDate
                        Return g_mCrashId.dLogDate
                    End Function

                    Public Function IReportInterface_ToString() As String Implements IReportInterface.ToString
                        Dim sException As New StringBuilder

                        sException.AppendFormat("Crash Id: {0}", g_mCrashId.sCrashId).AppendLine()
                        sException.AppendFormat("Date: {0}", g_mCrashId.dLogDate.ToString).AppendLine()

                        Return sException.ToString
                    End Function

                    Public Sub OnClicked(_FormReportManager As Object) Implements IReportInterface.OnClicked
                        Dim mFormReportManager = TryCast(_FormReportManager, FormReportManager)
                        If (mFormReportManager Is Nothing) Then
                            Return
                        End If

                        Dim sMessage As New StringBuilder
                        sMessage.AppendLine("Oh noes! Seems your server crashed at some point!")
                        sMessage.AppendFormat("Accelerator uploaded a crash dump: Crash Id: {0}", g_mCrashId.sCrashId.ToUpper).AppendLine.AppendLine()
                        sMessage.AppendLine("Do you want to lookup the crash dump?")
                        sMessage.AppendLine(" - Choose 'Yes' to open it in your browser.")
                        sMessage.AppendLine(" - Choose 'No' to copy the crash id to your clipboard.")
                        sMessage.AppendLine(" - Choose 'Cancel' to do nothing.")

                        Select Case (MessageBox.Show(sMessage.ToString, "Server Crash Dump", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                            Case DialogResult.Yes
                                Try
                                    Process.Start(String.Format("{0}/{1}", g_sAcceleratorServer.TrimEnd("/"c), g_mCrashId.sCrashId.Replace("-", "").ToLower))
                                Catch ex As Exception
                                End Try
                            Case DialogResult.No
                                My.Computer.Clipboard.SetText(g_mCrashId.sCrashId, TextDataFormat.Text)
                        End Select
                    End Sub
                End Class
            End Class
        End Class

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    AbortFetching()
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class

    Class ClassLogs
        Implements IDisposable

        Public g_mFormReportManager As FormReportManager

        Private g_mFetchLogsThread As Threading.Thread
        Private g_lFilesCleanup As New List(Of String)

        Public Sub New(mFormReportManager As FormReportManager)
            g_mFormReportManager = mFormReportManager
        End Sub

        Public Sub FetchLogs()
            If (ClassThread.IsValid(g_mFetchLogsThread)) Then
                Return
            End If

            'Cleanup old files
            For Each sFile As String In g_lFilesCleanup
                IO.File.Delete(sFile)
            Next
            g_lFilesCleanup.Clear()

            'Put here because file race condition
            g_mFormReportManager.g_mPluginConfigFtp.LoadConfig()
            g_mFormReportManager.g_mPluginConfigSettings.LoadConfig()

            Dim sConfigFtpContent As String = g_mFormReportManager.g_mPluginConfigFtp.ExportToString
            Dim sConfigSettingsContent As String = g_mFormReportManager.g_mPluginConfigSettings.ExportToString

            'Start new thread
            g_mFetchLogsThread = New Threading.Thread(Sub()
                                                          Try
                                                              ClassThread.ExecAsync(g_mFormReportManager, Sub()
                                                                                                              g_mFormReportManager.ToolStripMenuItem_GetLogs.Text = "Abort fetching logs"
                                                                                                              g_mFormReportManager.ToolStripMenuItem_GetLogs.Image = My.Resources.imageres_5337_16x16_32
                                                                                                          End Sub)

                                                              Const C_REPORTITEMS_TITLE = "Title"
                                                              Const C_REPORTITEMS_REMOTEFILE = "RemoteFile"
                                                              Const C_REPORTITEMS_LOCALFILE = "LocalFile"
                                                              Const C_REPORTITEMS_DATETICK = "DateTick"
                                                              Const C_REPORTITEMS_SIZE = "Size"
                                                              Const C_REPORTITEMS_ERRORINDEX = "ErrorIndex" 'See ERROR_* constants

                                                              Dim lReportItems As New List(Of Dictionary(Of String, Object)) 'See keys: C_REPORTITEMS_*
                                                              Dim lFtpEntries As New List(Of STRUC_FTP_ENTRY_ITEM)

                                                              Dim iMaxFileBytes As Long = (100 * 1024 * 1024)
                                                              Dim bFilesTooBig As Boolean = False

                                                              'Load Servers 
                                                              Using mIni As New ClassIni(sConfigFtpContent)
                                                                  For Each sSection As String In mIni.GetSectionNames
                                                                      Dim sHost As String = mIni.ReadKeyValue(sSection, "Host", Nothing)
                                                                      Dim sDatabaseEntry As String = mIni.ReadKeyValue(sSection, "DatabaseEntry", Nothing)
                                                                      Dim sSourceModPath As String = mIni.ReadKeyValue(sSection, "DestinationPath", Nothing)
                                                                      Dim sProtocol As String = mIni.ReadKeyValue(sSection, "Protocol", "FTP")

                                                                      If (String.IsNullOrEmpty(sHost) OrElse String.IsNullOrEmpty(sDatabaseEntry) OrElse String.IsNullOrEmpty(sSourceModPath)) Then
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

                                                              'Load Settings 
                                                              Using mIni As New ClassIni(sConfigSettingsContent)
                                                                  Dim iMaxFileSize As Integer = 0
                                                                  If (Integer.TryParse(mIni.ReadKeyValue("Settings", "MaxFileSize", "100"), iMaxFileSize)) Then
                                                                      iMaxFileBytes = (iMaxFileSize * 1024 * 1024)
                                                                  End If
                                                              End Using

                                                              Dim mFtpCacheDic As New Dictionary(Of String, Object)
                                                              Dim mClassFTP As ClassFTP = Nothing
                                                              Dim mClassSFTP As Renci.SshNet.SftpClient = Nothing
                                                              Try
                                                                  'Fetch Logs
                                                                  For Each mFtpItem In lFtpEntries
                                                                      Try
                                                                          Dim sLogDirectory As String = IO.Path.Combine(mFtpItem.sSourceModPath, "logs").Replace("\", "/")

                                                                          Dim mDatabaseItem = ClassDatabase.FindDatabaseItemByName(mFtpItem.sDatabaseEntry)
                                                                          If (mDatabaseItem Is Nothing) Then
                                                                              Throw New ArgumentException(String.Format("Unable to find database entry: {0}", mFtpItem.sDatabaseEntry))
                                                                          End If

                                                                          Dim sSafeFtpCacheDicKey As String = ClassTools.ClassStrings.ToSafeKey(mFtpItem.sHost & vbLf & mDatabaseItem.m_Username)

                                                                          Select Case (mFtpItem.iProtocolType)
                                                                              Case ENUM_FTP_PROTOCOL_TYPE.FTP
                                                                                  If (mFtpCacheDic.ContainsKey(sSafeFtpCacheDicKey)) Then
                                                                                      mClassFTP = DirectCast(mFtpCacheDic(sSafeFtpCacheDicKey), ClassFTP)
                                                                                  Else
                                                                                      mClassFTP = New ClassFTP(mFtpItem.sHost, mDatabaseItem.m_Username, mDatabaseItem.m_Password)
                                                                                      mFtpCacheDic(sSafeFtpCacheDicKey) = mClassFTP
                                                                                  End If

                                                                                  If (Not mClassFTP.PathExist(sLogDirectory)) Then
                                                                                      Throw New ArgumentException(String.Format("Could not find SourceMod 'logs' directory on: {0}/{1}", mFtpItem.sHost.TrimEnd("/"c), sLogDirectory.TrimStart("/"c)))
                                                                                  End If

                                                                                  For Each mItem In mClassFTP.GetDirectoryEntries(sLogDirectory)
                                                                                      Select Case (mItem.sName)
                                                                                          Case ".", ".."
                                                                                              Continue For
                                                                                      End Select

                                                                                      If (mItem.bIsDirectory) Then
                                                                                          Continue For
                                                                                      End If

                                                                                      Dim sFileExt As String = IO.Path.GetExtension(mItem.sName)
                                                                                      Dim sFileFullName As String = IO.Path.GetFileName(mItem.sName)

                                                                                      If (sFileExt.ToLower <> ".log" AndAlso sFileExt.ToLower <> ".txt") Then
                                                                                          Continue For
                                                                                      End If

                                                                                      If (mItem.iSize > iMaxFileBytes) Then
                                                                                          bFilesTooBig = True

                                                                                          Dim mReportItemDic As New Dictionary(Of String, Object)
                                                                                          mReportItemDic(C_REPORTITEMS_TITLE) = mItem.sName
                                                                                          mReportItemDic(C_REPORTITEMS_REMOTEFILE) = mFtpItem.sHost.TrimEnd("\"c) & mItem.sFullName.TrimStart("\"c)
                                                                                          mReportItemDic(C_REPORTITEMS_LOCALFILE) = ""
                                                                                          mReportItemDic(C_REPORTITEMS_DATETICK) = mItem.dModified.Ticks
                                                                                          mReportItemDic(C_REPORTITEMS_SIZE) = mItem.iSize
                                                                                          mReportItemDic(C_REPORTITEMS_ERRORINDEX) = ERROR_TOOBIG

                                                                                          lReportItems.Add(mReportItemDic)
                                                                                          Continue For
                                                                                      End If

                                                                                      Dim sTmpFile As String = IO.Path.GetTempFileName
                                                                                      g_lFilesCleanup.Add(sTmpFile)
                                                                                      mClassFTP.DownloadFile(mItem.sFullName, sTmpFile)
                                                                                      ApplyNewlineFix(sTmpFile)

                                                                                      If (True) Then
                                                                                          Dim mReportItemDic As New Dictionary(Of String, Object)
                                                                                          mReportItemDic(C_REPORTITEMS_TITLE) = mItem.sName
                                                                                          mReportItemDic(C_REPORTITEMS_REMOTEFILE) = mFtpItem.sHost.TrimEnd("\"c) & mItem.sFullName.TrimStart("\"c)
                                                                                          mReportItemDic(C_REPORTITEMS_LOCALFILE) = sTmpFile
                                                                                          mReportItemDic(C_REPORTITEMS_DATETICK) = mItem.dModified.Ticks
                                                                                          mReportItemDic(C_REPORTITEMS_SIZE) = mItem.iSize
                                                                                          mReportItemDic(C_REPORTITEMS_ERRORINDEX) = ERROR_NOERROR

                                                                                          lReportItems.Add(mReportItemDic)
                                                                                      End If
                                                                                  Next


                                                                              Case ENUM_FTP_PROTOCOL_TYPE.SFTP
                                                                                  If (mFtpCacheDic.ContainsKey(sSafeFtpCacheDicKey)) Then
                                                                                      mClassSFTP = DirectCast(mFtpCacheDic(sSafeFtpCacheDicKey), Renci.SshNet.SftpClient)
                                                                                  Else
                                                                                      mClassSFTP = New Renci.SshNet.SftpClient(mFtpItem.sHost, mDatabaseItem.m_Username, mDatabaseItem.m_Password)
                                                                                      mFtpCacheDic(sSafeFtpCacheDicKey) = mClassSFTP
                                                                                  End If

                                                                                  If (Not mClassSFTP.IsConnected) Then
                                                                                      mClassSFTP.Connect()
                                                                                  End If

                                                                                  If (Not mClassSFTP.Exists(sLogDirectory)) Then
                                                                                      Throw New ArgumentException(String.Format("Could not find SourceMod 'logs' directory on: {0}/{1}", mFtpItem.sHost.TrimEnd("/"c), sLogDirectory.TrimStart("/"c)))
                                                                                  End If

                                                                                  For Each mItem In mClassSFTP.ListDirectory(sLogDirectory)
                                                                                      Select Case (mItem.Name)
                                                                                          Case ".", ".."
                                                                                              Continue For
                                                                                      End Select

                                                                                      If (mItem.IsDirectory) Then
                                                                                          Continue For
                                                                                      End If

                                                                                      Dim sFileExt As String = IO.Path.GetExtension(mItem.Name)
                                                                                      Dim sFileFullName As String = IO.Path.GetFileName(mItem.Name)

                                                                                      If (sFileExt.ToLower <> ".log" AndAlso sFileExt.ToLower <> ".txt") Then
                                                                                          Continue For
                                                                                      End If

                                                                                      If (mItem.Length > iMaxFileBytes) Then
                                                                                          bFilesTooBig = True

                                                                                          Dim mReportItemDic As New Dictionary(Of String, Object)
                                                                                          mReportItemDic(C_REPORTITEMS_TITLE) = mItem.Name
                                                                                          mReportItemDic(C_REPORTITEMS_REMOTEFILE) = mFtpItem.sHost.TrimEnd("\"c) & mItem.FullName.TrimStart("\"c)
                                                                                          mReportItemDic(C_REPORTITEMS_LOCALFILE) = ""
                                                                                          mReportItemDic(C_REPORTITEMS_DATETICK) = mItem.Attributes.LastWriteTime.Ticks
                                                                                          mReportItemDic(C_REPORTITEMS_SIZE) = mItem.Length
                                                                                          mReportItemDic(C_REPORTITEMS_ERRORINDEX) = ERROR_TOOBIG

                                                                                          lReportItems.Add(mReportItemDic)
                                                                                          Continue For
                                                                                      End If

                                                                                      Dim sTmpFile As String = IO.Path.GetTempFileName
                                                                                      g_lFilesCleanup.Add(sTmpFile)
                                                                                      Using mStream As New IO.FileStream(sTmpFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                                                                                          mClassSFTP.DownloadFile(mItem.FullName, mStream)
                                                                                      End Using
                                                                                      ApplyNewlineFix(sTmpFile)

                                                                                      If (True) Then
                                                                                          Dim mReportItemDic As New Dictionary(Of String, Object)
                                                                                          mReportItemDic(C_REPORTITEMS_TITLE) = mItem.Name
                                                                                          mReportItemDic(C_REPORTITEMS_REMOTEFILE) = mFtpItem.sHost.TrimEnd("\"c) & mItem.FullName.TrimStart("\"c)
                                                                                          mReportItemDic(C_REPORTITEMS_LOCALFILE) = sTmpFile
                                                                                          mReportItemDic(C_REPORTITEMS_DATETICK) = mItem.Attributes.LastWriteTime.Ticks
                                                                                          mReportItemDic(C_REPORTITEMS_SIZE) = mItem.Length
                                                                                          mReportItemDic(C_REPORTITEMS_ERRORINDEX) = ERROR_NOERROR

                                                                                          lReportItems.Add(mReportItemDic)
                                                                                      End If
                                                                                  Next

                                                                                  If (bFilesTooBig) Then
                                                                                      Dim mReportItemDic As New Dictionary(Of String, Object)
                                                                                      mReportItemDic(C_REPORTITEMS_TITLE) = String.Format("Unable to fetch some log files. Some log files are too big to fetch. (max. {0} MB)", iMaxFileBytes / 1024 / 1024)
                                                                                      mReportItemDic(C_REPORTITEMS_REMOTEFILE) = ""
                                                                                      mReportItemDic(C_REPORTITEMS_LOCALFILE) = ""
                                                                                      mReportItemDic(C_REPORTITEMS_DATETICK) = 0
                                                                                      mReportItemDic(C_REPORTITEMS_SIZE) = 0
                                                                                      mReportItemDic(C_REPORTITEMS_ERRORINDEX) = ERROR_MSGONLY

                                                                                      lReportItems.Add(mReportItemDic)
                                                                                  End If

                                                                              Case Else
                                                                                  Throw New ArgumentException("Unknown connection type")
                                                                          End Select
                                                                      Catch ex As Threading.ThreadAbortException
                                                                          Throw
                                                                      Catch ex As Exception
                                                                          Dim mReportItemDic As New Dictionary(Of String, Object)
                                                                          mReportItemDic(C_REPORTITEMS_TITLE) = "Error: " & ex.Message
                                                                          mReportItemDic(C_REPORTITEMS_REMOTEFILE) = ""
                                                                          mReportItemDic(C_REPORTITEMS_LOCALFILE) = ""
                                                                          mReportItemDic(C_REPORTITEMS_DATETICK) = 0
                                                                          mReportItemDic(C_REPORTITEMS_SIZE) = 0
                                                                          mReportItemDic(C_REPORTITEMS_ERRORINDEX) = ERROR_MSGONLY

                                                                          lReportItems.Add(mReportItemDic)
                                                                      End Try
                                                                  Next
                                                              Catch ex As Exception
                                                                  For Each mItem In mFtpCacheDic
                                                                      Select Case (True)
                                                                          Case TypeOf mItem.Value Is ClassFTP
                                                                                 'Nothing to dispose

                                                                          Case TypeOf mItem.Value Is Renci.SshNet.SftpClient
                                                                              mClassSFTP = DirectCast(mItem.Value, Renci.SshNet.SftpClient)
                                                                              If (mClassSFTP IsNot Nothing) Then
                                                                                  mClassSFTP.Dispose()
                                                                                  mClassSFTP = Nothing
                                                                              End If
                                                                      End Select
                                                                  Next

                                                                  mFtpCacheDic = Nothing
                                                              End Try

                                                              ClassThread.ExecAsync(g_mFormReportManager, Sub()
                                                                                                              Try
                                                                                                                  g_mFormReportManager.g_mClassTreeViewColumns.m_TreeView.BeginUpdate()
                                                                                                                  g_mFormReportManager.g_mClassTreeViewColumns.m_TreeView.Nodes.Clear()

                                                                                                                  For Each mItem In lReportItems
                                                                                                                      Dim sTitle As String = CStr(mItem(C_REPORTITEMS_TITLE))
                                                                                                                      Dim sRemoteFile As String = CStr(mItem(C_REPORTITEMS_REMOTEFILE))
                                                                                                                      Dim sLocalFile As String = CStr(mItem(C_REPORTITEMS_LOCALFILE))
                                                                                                                      Dim iDate As Date = New Date(CLng(mItem(C_REPORTITEMS_DATETICK)))
                                                                                                                      Dim iSize As Long = CLng(mItem(C_REPORTITEMS_SIZE))
                                                                                                                      Dim iErrorIndex As Integer = CInt(mItem(C_REPORTITEMS_ERRORINDEX))

                                                                                                                      Dim iImageIndex As Integer = ICON_FILE
                                                                                                                      Select Case (iErrorIndex)
                                                                                                                          Case ERROR_MSGONLY
                                                                                                                              iImageIndex = ICON_WARN

                                                                                                                          Case ERROR_TOOBIG
                                                                                                                              iImageIndex = ICON_ERROR

                                                                                                                      End Select

                                                                                                                      Select Case (iErrorIndex)
                                                                                                                          Case ERROR_MSGONLY
                                                                                                                              Dim sRootNodeName As String = "Information"

                                                                                                                              Dim mRootNodes As TreeNodeCollection = g_mFormReportManager.g_mClassTreeViewColumns.m_TreeView.Nodes
                                                                                                                              Dim mFileNodes As TreeNodeCollection

                                                                                                                              If (Not mRootNodes.ContainsKey(sRootNodeName)) Then
                                                                                                                                  mRootNodes.Add(New TreeNode(sRootNodeName, ICON_ARROW, ICON_ARROW) With {
                                                                                                                                        .Name = sRootNodeName
                                                                                                                                  })
                                                                                                                              End If

                                                                                                                              mFileNodes = mRootNodes(sRootNodeName).Nodes

                                                                                                                              mFileNodes.Add(New TreeNode(sTitle, iImageIndex, iImageIndex))
                                                                                                                          Case Else

                                                                                                                              'Check if the remote path is present, we need that
                                                                                                                              If (String.IsNullOrEmpty(sRemoteFile)) Then
                                                                                                                                  Continue For
                                                                                                                              End If

                                                                                                                              Dim sRootNodeName As String = IO.Path.GetDirectoryName(sRemoteFile)

                                                                                                                              Dim mRootNodes As TreeNodeCollection = g_mFormReportManager.g_mClassTreeViewColumns.m_TreeView.Nodes
                                                                                                                              Dim mFileNodes As TreeNodeCollection

                                                                                                                              If (Not mRootNodes.ContainsKey(sRootNodeName)) Then
                                                                                                                                  mRootNodes.Add(New TreeNode(sRootNodeName, ICON_ARROW, ICON_ARROW) With {
                                                                                                                                    .Name = sRootNodeName
                                                                                                                              })
                                                                                                                              End If

                                                                                                                              mFileNodes = mRootNodes(sRootNodeName).Nodes

                                                                                                                              Dim mNode As New ClassTreeNodeData(sTitle, iImageIndex, iImageIndex) With {
                                                                                                                                    .Tag = New String() {ClassTools.ClassStrings.FormatBytes(iSize), iDate.ToString}
                                                                                                                              }

                                                                                                                              mNode.g_mData("Title") = sTitle
                                                                                                                              mNode.g_mData("RemoteFile") = sRemoteFile
                                                                                                                              mNode.g_mData("LocalFile") = sLocalFile
                                                                                                                              mNode.g_mData("DateTick") = iDate.Ticks
                                                                                                                              mNode.g_mData("Size") = iSize
                                                                                                                              mNode.g_mData("ErrorIndex") = iErrorIndex

                                                                                                                              mFileNodes.Add(mNode)
                                                                                                                      End Select
                                                                                                                  Next

                                                                                                                  g_mFormReportManager.g_mClassTreeViewColumns.m_TreeView.Sort()
                                                                                                              Finally
                                                                                                                  g_mFormReportManager.g_mClassTreeViewColumns.m_TreeView.EndUpdate()
                                                                                                                  g_mFormReportManager.g_mClassTreeViewColumns.m_TreeView.ExpandAll()
                                                                                                              End Try
                                                                                                          End Sub)

                                                              ClassThread.ExecAsync(g_mFormReportManager, Sub()
                                                                                                              g_mFormReportManager.ToolStripMenuItem_GetLogs.Text = g_mFormReportManager.g_sGetLogsOrginalText
                                                                                                              g_mFormReportManager.ToolStripMenuItem_GetLogs.Image = g_mFormReportManager.g_sGetLogsOrginalImage
                                                                                                          End Sub)
                                                          Catch ex As Threading.ThreadAbortException
                                                              Throw
                                                          Catch ex As Exception
                                                              ClassExceptionLog.WriteToLogMessageBox(ex)

                                                              ClassThread.ExecAsync(g_mFormReportManager, Sub()
                                                                                                              g_mFormReportManager.ToolStripMenuItem_GetLogs.Text = g_mFormReportManager.g_sGetLogsOrginalText
                                                                                                              g_mFormReportManager.ToolStripMenuItem_GetLogs.Image = g_mFormReportManager.g_sGetLogsOrginalImage
                                                                                                          End Sub)
                                                          End Try
                                                      End Sub) With {
                .IsBackground = True
            }
            g_mFetchLogsThread.Start()
        End Sub

        'Unix newline fix for notepad or other editors that doesnt support it
        Private Sub ApplyNewlineFix(sPath As String)
            Dim sText As String = IO.File.ReadAllText(sPath)

            sText = String.Join(Environment.NewLine, sText.Split(New String() {vbNewLine, vbLf}, 0))

            IO.File.WriteAllText(sPath, sText)
        End Sub

        Public Function IsFetchingLogs() As Boolean
            Return ClassThread.IsValid(g_mFetchLogsThread)
        End Function

        Public Sub AbortFetching()
            ClassThread.Abort(g_mFetchLogsThread)

            g_mFormReportManager.ToolStripMenuItem_GetLogs.Text = g_mFormReportManager.g_sGetLogsOrginalText
            g_mFormReportManager.ToolStripMenuItem_GetLogs.Image = g_mFormReportManager.g_sGetLogsOrginalImage
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    AbortFetching()

                    'Cleanup old files
                    For Each sFile As String In g_lFilesCleanup
                        IO.File.Delete(sFile)
                    Next
                    g_lFilesCleanup.Clear()
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

    Private Sub FormReportManager_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub CleanUp()

        If (g_mClassTreeViewColumns IsNot Nothing) Then
            RemoveHandler g_mClassTreeViewColumns.m_TreeView.DoubleClick, AddressOf ClassTreeViewColumns_DoubleClick
        End If

        If (g_mClassTreeViewColumns IsNot Nothing AndAlso Not g_mClassTreeViewColumns.IsDisposed) Then
            g_mClassTreeViewColumns.Dispose()
            g_mClassTreeViewColumns = Nothing
        End If

        If (g_mClassReports IsNot Nothing) Then
            g_mClassReports.CloseReportForms()

            g_mClassReports.Dispose()
            g_mClassReports = Nothing
        End If

        If (g_mClassLogs IsNot Nothing) Then
            g_mClassLogs.Dispose()
            g_mClassLogs = Nothing
        End If
    End Sub
End Class