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


Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports BasicPawn
Imports BasicPawnPluginInterface

Public Class PluginFTP
    Implements IPluginInterface

    Public g_mFormMain As FormMain

    Private g_ClassPlugin As ClassPlugin
    Private g_mUpdateThread As Threading.Thread

    Private Shared ReadOnly g_mSpportedVersion As New Version("0.734")

#Region "Unused"
    Public Sub OnPluginLoad(sDLLPath As String) Implements IPluginInterface.OnPluginLoad
        Throw New NotImplementedException()
    End Sub

    Public Sub OnSettingsChanged() Implements IPluginInterface.OnSettingsChanged
        Throw New NotImplementedException()
    End Sub

    Public Sub OnConfigChanged() Implements IPluginInterface.OnConfigChanged
        Throw New NotImplementedException()
    End Sub

    Public Sub OnEditorSyntaxUpdate() Implements IPluginInterface.OnEditorSyntaxUpdate
        Throw New NotImplementedException()
    End Sub

    Public Sub OnEditorSyntaxUpdateEnd() Implements IPluginInterface.OnEditorSyntaxUpdateEnd
        Throw New NotImplementedException()
    End Sub

    Public Sub OnSyntaxUpdate(iType As Integer, bForceFromMemory As Boolean) Implements IPluginInterface.OnSyntaxUpdate
        Throw New NotImplementedException()
    End Sub

    Public Sub OnSyntaxUpdateEnd(iType As Integer, bForceFromMemory As Boolean) Implements IPluginInterface.OnSyntaxUpdateEnd
        Throw New NotImplementedException()
    End Sub

    Public Sub OnFormColorUpdate() Implements IPluginInterface.OnFormColorUpdate
        Throw New NotImplementedException()
    End Sub

    Public Sub OnDebuggerStart(mFormDebugger As Object) Implements IPluginInterface.OnDebuggerStart
        Throw New NotImplementedException()
    End Sub

    Public Sub OnDebuggerRefresh(mFormDebugger As Object) Implements IPluginInterface.OnDebuggerRefresh
        Throw New NotImplementedException()
    End Sub

    Public Sub OnDebuggerEndPost(mFormDebugger As Object) Implements IPluginInterface.OnDebuggerEndPost
        Throw New NotImplementedException()
    End Sub

    Public Sub OnDebuggerDebugStart() Implements IPluginInterface.OnDebuggerDebugStart
        Throw New NotImplementedException()
    End Sub

    Public Sub OnDebuggerDebugPause() Implements IPluginInterface.OnDebuggerDebugPause
        Throw New NotImplementedException()
    End Sub

    Public Sub OnDebuggerDebugStop() Implements IPluginInterface.OnDebuggerDebugStop
        Throw New NotImplementedException()
    End Sub

    Public Function OnPluginEnd() As Boolean Implements IPluginInterface.OnPluginEnd
        Throw New NotImplementedException()
    End Function

    Public Function OnDebuggerEnd(mFormDebugger As Object) As Boolean Implements IPluginInterface.OnDebuggerEnd
        Throw New NotImplementedException()
    End Function
#End Region

    Public ReadOnly Property m_PluginInformation As IPluginInterface.STRUC_PLUGIN_INFORMATION Implements IPluginInterface.m_PluginInformation
        Get
            Return New IPluginInterface.STRUC_PLUGIN_INFORMATION("FTP Plugin",
                                                                 "Timocop",
                                                                 "Allows uploading files to servers over FTP.",
                                                                 ClassUpdate.GetCurrentVersion(),
                                                                 Nothing)
        End Get
    End Property

    Public Sub OnPluginStart(mFormMain As Object, bEnabled As Boolean) Implements IPluginInterface.OnPluginStart
        g_mFormMain = DirectCast(mFormMain, FormMain)

        If (bEnabled) Then
            OnPluginEnabled(Nothing)
        End If
    End Sub

    Public ReadOnly Property m_PluginEnabled As Boolean Implements IPluginInterface.m_PluginEnabled
        Get
            Return (g_ClassPlugin IsNot Nothing)
        End Get
    End Property

    Public Sub OnPluginEndPost() Implements IPluginInterface.OnPluginEndPost
        ClassThread.Abort(g_mUpdateThread)

        If (g_ClassPlugin IsNot Nothing) Then
            g_ClassPlugin.Dispose()
            g_ClassPlugin = Nothing
        End If
    End Sub

    Public Function OnPluginEnabled(ByRef sReason As String) As Boolean Implements IPluginInterface.OnPluginEnabled
#If Not DEBUG Then
        If (New Version(Application.ProductVersion) < g_mSpportedVersion) Then
            sReason = String.Format("Unsupported BasicPawn version! Required is version v{0}.", g_mSpportedVersion.ToString)
            Return False
        End If
#End If

        If (g_ClassPlugin Is Nothing) Then
            g_ClassPlugin = New ClassPlugin(Me)
        End If

        If (Not ClassThread.IsValid(g_mUpdateThread)) Then
            g_mUpdateThread = New Threading.Thread(Sub()
                                                       Try
                                                           Threading.Thread.Sleep(10000)

                                                           Dim sCurrentVersion As String = ""
                                                           Dim sNewVersion As String = ""
                                                           If (ClassUpdate.CheckUpdateAvailable(sNewVersion, sCurrentVersion)) Then
                                                               Select Case (MessageBox.Show(String.Format("A new version is available! Do you want to download version v{0} now?", sNewVersion), m_PluginInformation.sName, MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                                                                   Case DialogResult.Yes
                                                                       Process.Start(ClassUpdate.g_sGithubDownloadURL)
                                                               End Select
                                                           End If
                                                       Catch ex As Threading.ThreadAbortException
                                                           Throw
                                                       Catch ex As Exception
                                                       End Try
                                                   End Sub) With {
                    .IsBackground = True,
                    .Priority = Threading.ThreadPriority.Lowest
                }
            g_mUpdateThread.Start()
        End If

        Return True
    End Function

    Public Function OnPluginDisabled(ByRef sReason As String) As Boolean Implements IPluginInterface.OnPluginDisabled
        If (g_ClassPlugin IsNot Nothing) Then
            g_ClassPlugin.Dispose()
            g_ClassPlugin = Nothing
        End If

        Return True
    End Function

    Class ClassUpdate
        Public Shared ReadOnly g_sGithubVersionURL As String = "https://github.com/Timocop/BasicPawn/raw/master/Plugin%20Releases/BasicPawnPluginFTPCurrentVersion.txt"
        Public Shared ReadOnly g_sGithubDownloadURL As String = "https://github.com/Timocop/BasicPawn/raw/master/Plugin%20Releases/BasicPawnPluginFTP.dll"

        Public Shared Function CheckUpdateAvailable() As Boolean
            Dim sNextVersion = ""
            Dim sCurrentVersion = ""
            Return CheckUpdateAvailable(sNextVersion, sCurrentVersion)
        End Function

        Public Shared Function CheckUpdateAvailable(ByRef r_sNextVersion As String, ByRef r_sCurrentVersion As String) As Boolean
            Dim sNextVersion As String = Regex.Match(GetNextVersion(), "[0-9\.]+").Value
            Dim sCurrentVersion As String = Regex.Match(GetCurrentVersion(), "[0-9\.]+").Value

            r_sNextVersion = sNextVersion
            r_sCurrentVersion = sCurrentVersion

            Return (New Version(sNextVersion) > New Version(sCurrentVersion))
        End Function

        Public Shared Function GetCurrentVersion() As String
            Return Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString
        End Function

        Public Shared Function GetNextVersion() As String
            If (String.IsNullOrEmpty(g_sGithubVersionURL)) Then
                Throw New ArgumentException("Version URL empty")
            End If

            Using mWC As New ClassWebClientEx
                Return mWC.DownloadString(g_sGithubVersionURL)
            End Using
        End Function
    End Class

    Class ClassPlugin
        Implements IDisposable

        Public g_mPluginFTP As PluginFTP

        Private g_mFtpMenuSplit As ToolStripSeparator
        Private g_mFtpMenuItem As ToolStripMenuItem
        Private g_mFtpCompileItem As ToolStripMenuItem
        Private g_mFtpCompileAllItem As ToolStripMenuItem

        Public Sub New(mPluginSample As PluginFTP)
            g_mPluginFTP = mPluginSample

            BuildFTPMenu()
        End Sub

        Private Sub BuildFTPMenu()
            g_mFtpMenuSplit = New ToolStripSeparator
            g_mFtpMenuItem = New ToolStripMenuItem("FTP File Upload", My.Resources.imageres_5340_16x16)
            g_mPluginFTP.g_mFormMain.ToolStripMenuItem_Tools.DropDownItems.Add(g_mFtpMenuSplit)
            g_mPluginFTP.g_mFormMain.ToolStripMenuItem_Tools.DropDownItems.Add(g_mFtpMenuItem)

            RemoveHandler g_mFtpMenuItem.Click, AddressOf OnMenuItemClick
            AddHandler g_mFtpMenuItem.Click, AddressOf OnMenuItemClick

            Dim iBuildIndex As Integer = g_mPluginFTP.g_mFormMain.MenuStrip_BasicPawn.Items.IndexOf(g_mPluginFTP.g_mFormMain.ToolStripMenuItem_Build)
            If (iBuildIndex > -1) Then
                g_mFtpCompileItem = New ToolStripMenuItem("Upload", My.Resources.imageres_5340_16x16)

                g_mPluginFTP.g_mFormMain.MenuStrip_BasicPawn.Items.Insert(iBuildIndex, g_mFtpCompileItem)

                RemoveHandler g_mFtpCompileItem.Click, AddressOf OnCompileItemClick
                AddHandler g_mFtpCompileItem.Click, AddressOf OnCompileItemClick
            End If

            Dim iBuildAllIndex As Integer = g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ContextMenuStrip_ProjectFiles.Items.IndexOf(g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ToolStripMenuItem_CompileAll)
            If (iBuildAllIndex > -1) Then
                g_mFtpCompileAllItem = New ToolStripMenuItem("Upload", My.Resources.imageres_5340_16x16)

                g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ContextMenuStrip_ProjectFiles.Items.Insert(iBuildAllIndex, g_mFtpCompileAllItem)

                RemoveHandler g_mFtpCompileAllItem.Click, AddressOf OnCompileAllItemClick
                AddHandler g_mFtpCompileAllItem.Click, AddressOf OnCompileAllItemClick

                RemoveHandler g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ContextMenuStrip_ProjectFiles.Opening, AddressOf ContextMenuStripProjectFilesOpening
                AddHandler g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ContextMenuStrip_ProjectFiles.Opening, AddressOf ContextMenuStripProjectFilesOpening
            End If

            'Update all FormMain controls, to change style for the newly created controls
            ClassControlStyle.UpdateControls(g_mPluginFTP.g_mFormMain)
        End Sub

        Private Sub OnMenuItemClick(sender As Object, e As EventArgs)
            Try
                Using i As New FormFTP(g_mPluginFTP, New String() {})
                    i.ShowDialog(g_mPluginFTP.g_mFormMain)
                End Using
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Private Sub OnCompileItemClick(sender As Object, e As EventArgs)
            Try
                Dim sSource As String = g_mPluginFTP.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent

                With New ClassDebuggerParser(g_mPluginFTP.g_mFormMain)
                    If (.HasDebugPlaceholder(sSource)) Then
                        .CleanupDebugPlaceholder(sSource, g_mPluginFTP.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)
                    End If
                End With

                Dim sSourceFile As String = Nothing
                If (Not g_mPluginFTP.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IsUnsaved AndAlso Not g_mPluginFTP.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                    sSourceFile = g_mPluginFTP.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File
                End If

                Dim sOutputFile As String = ""
                g_mPluginFTP.g_mFormMain.g_ClassTextEditorTools.CompileSource(False, sSource, sOutputFile, If(sSourceFile Is Nothing, Nothing, IO.Path.GetDirectoryName(sSourceFile)), Nothing, Nothing, sSourceFile)

                Using i As New FormFTP(g_mPluginFTP, sOutputFile)
                    i.ShowDialog(g_mPluginFTP.g_mFormMain)
                End Using
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Private Sub OnCompileAllItemClick(sender As Object, e As EventArgs)
            Try
                If (g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ListView_ProjectFiles.SelectedItems.Count < 1) Then
                    Return
                End If

                Dim lFiles As New List(Of String)

                For Each mListViewItem As ListViewItem In g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ListView_ProjectFiles.SelectedItems
                    Dim mListViewItemData = TryCast(mListViewItem, ClassListViewItemData)
                    If (mListViewItemData Is Nothing) Then
                        Continue For
                    End If

                    Dim mInfo = DirectCast(mListViewItemData.g_mData("Info"), UCProjectBrowser.ClassProjectControl.STRUC_PROJECT_FILE_INFO)

                    lFiles.Add(mInfo.sFile)
                Next

                Dim lCompiledFiles As New List(Of String)

                Using i As New FormMultiCompiler(g_mPluginFTP.g_mFormMain, lFiles.ToArray, False, False, lCompiledFiles)
                    i.ShowDialog(g_mPluginFTP.g_mFormMain)
                End Using

                Using i As New FormFTP(g_mPluginFTP, lCompiledFiles.ToArray)
                    i.ShowDialog(g_mPluginFTP.g_mFormMain)
                End Using
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Private Sub ContextMenuStripProjectFilesOpening(sender As Object, e As System.ComponentModel.CancelEventArgs)
            If (g_mFtpCompileAllItem Is Nothing OrElse g_mFtpCompileAllItem.IsDisposed) Then
                Return
            End If

            g_mFtpCompileAllItem.Enabled = (g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ListView_ProjectFiles.SelectedItems.Count > 0)
        End Sub


#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    RemoveHandler g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ContextMenuStrip_ProjectFiles.Opening, AddressOf ContextMenuStripProjectFilesOpening

                    If (g_mFtpMenuSplit IsNot Nothing AndAlso Not g_mFtpMenuSplit.IsDisposed) Then
                        g_mFtpMenuSplit.Dispose()
                        g_mFtpMenuSplit = Nothing
                    End If

                    If (g_mFtpMenuItem IsNot Nothing AndAlso Not g_mFtpMenuItem.IsDisposed) Then
                        RemoveHandler g_mFtpMenuItem.Click, AddressOf OnMenuItemClick

                        g_mFtpMenuItem.Dispose()
                        g_mFtpMenuItem = Nothing
                    End If

                    If (g_mFtpCompileItem IsNot Nothing AndAlso Not g_mFtpCompileItem.IsDisposed) Then
                        RemoveHandler g_mFtpCompileItem.Click, AddressOf OnCompileItemClick

                        g_mFtpCompileItem.Dispose()
                        g_mFtpCompileItem = Nothing
                    End If

                    If (g_mFtpCompileAllItem IsNot Nothing AndAlso Not g_mFtpCompileAllItem.IsDisposed) Then
                        RemoveHandler g_mFtpCompileAllItem.Click, AddressOf OnCompileAllItemClick

                        g_mFtpCompileAllItem.Dispose()
                        g_mFtpCompileAllItem = Nothing
                    End If
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
End Class
