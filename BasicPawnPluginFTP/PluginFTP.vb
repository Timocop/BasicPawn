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


Imports BasicPawnPluginInterface

Public Class PluginFTP
    Implements IPluginInterfaceV10

    Public g_mFormMain As FormMain

    Private g_ClassPlugin As ClassPlugin

#Region "Unused"
    Public Sub OnPluginLoad(sDLLPath As String) Implements IPluginInterfaceV10.OnPluginLoad
        Throw New NotImplementedException()
    End Sub

    Public Sub OnSettingsChanged() Implements IPluginInterfaceV10.OnSettingsChanged
        Throw New NotImplementedException()
    End Sub

    Public Sub OnConfigChanged() Implements IPluginInterfaceV10.OnConfigChanged
        Throw New NotImplementedException()
    End Sub

    Public Sub OnEditorSyntaxUpdate() Implements IPluginInterfaceV10.OnEditorSyntaxUpdate
        Throw New NotImplementedException()
    End Sub

    Public Sub OnEditorSyntaxUpdateEnd() Implements IPluginInterfaceV10.OnEditorSyntaxUpdateEnd
        Throw New NotImplementedException()
    End Sub

    Public Sub OnSyntaxUpdate(iType As Integer, bForceFromMemory As Boolean) Implements IPluginInterfaceV10.OnSyntaxUpdate
        Throw New NotImplementedException()
    End Sub

    Public Sub OnSyntaxUpdateEnd(iType As Integer, bForceFromMemory As Boolean) Implements IPluginInterfaceV10.OnSyntaxUpdateEnd
        Throw New NotImplementedException()
    End Sub

    Public Sub OnFormColorUpdate() Implements IPluginInterfaceV10.OnFormColorUpdate
        Throw New NotImplementedException()
    End Sub

    Public Sub OnDebuggerStart(mFormDebugger As Object) Implements IPluginInterfaceV10.OnDebuggerStart
        Throw New NotImplementedException()
    End Sub

    Public Sub OnDebuggerRefresh(mFormDebugger As Object) Implements IPluginInterfaceV10.OnDebuggerRefresh
        Throw New NotImplementedException()
    End Sub

    Public Sub OnDebuggerEndPost(mFormDebugger As Object) Implements IPluginInterfaceV10.OnDebuggerEndPost
        Throw New NotImplementedException()
    End Sub

    Public Sub OnDebuggerDebugStart() Implements IPluginInterfaceV10.OnDebuggerDebugStart
        Throw New NotImplementedException()
    End Sub

    Public Sub OnDebuggerDebugPause() Implements IPluginInterfaceV10.OnDebuggerDebugPause
        Throw New NotImplementedException()
    End Sub

    Public Sub OnDebuggerDebugStop() Implements IPluginInterfaceV10.OnDebuggerDebugStop
        Throw New NotImplementedException()
    End Sub

    Public Function OnPluginEnd() As Boolean Implements IPluginInterfaceV10.OnPluginEnd
        Throw New NotImplementedException()
    End Function

    Public Function OnDebuggerEnd(mFormDebugger As Object) As Boolean Implements IPluginInterfaceV10.OnDebuggerEnd
        Throw New NotImplementedException()
    End Function
#End Region

    Public ReadOnly Property m_PluginEnabled As Boolean Implements IPluginInterfaceV10.m_PluginEnabled
        Get
            Return (g_ClassPlugin IsNot Nothing)
        End Get
    End Property

    Public Sub OnPluginStart(mFormMain As Object, bEnabled As Boolean) Implements IPluginInterfaceV10.OnPluginStart
        g_mFormMain = DirectCast(mFormMain, FormMain)

        If (bEnabled) Then
            OnPluginEnabled(Nothing)
        End If
    End Sub

    Public Sub OnPluginEndPost() Implements IPluginInterfaceV10.OnPluginEndPost
        If (g_ClassPlugin IsNot Nothing) Then
            g_ClassPlugin.Dispose()
            g_ClassPlugin = Nothing
        End If
    End Sub

    Public Function OnPluginEnabled(ByRef sReason As String) As Boolean Implements IPluginInterfaceV10.OnPluginEnabled
        If (g_ClassPlugin Is Nothing) Then
            g_ClassPlugin = New ClassPlugin(Me)
        End If

        Return True
    End Function

    Public Function OnPluginDisabled(ByRef sReason As String) As Boolean Implements IPluginInterfaceV10.OnPluginDisabled
        If (g_ClassPlugin IsNot Nothing) Then
            g_ClassPlugin.Dispose()
            g_ClassPlugin = Nothing
        End If

        Return True
    End Function

    Class ClassPlugin
        Implements IDisposable

        Public g_mPluginFTP As PluginFTP

        Private g_mFtpMenuSplit As ToolStripSeparator
        Private g_mFtpMenuBuildSplit As ToolStripSeparator
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

            If (True) Then
                g_mFtpMenuBuildSplit = New ToolStripSeparator
                g_mFtpCompileItem = New ToolStripMenuItem("Upload", My.Resources.imageres_5340_16x16)

                g_mPluginFTP.g_mFormMain.ToolStripMenuItem_Build.DropDownItems.Add(g_mFtpMenuBuildSplit)
                g_mPluginFTP.g_mFormMain.ToolStripMenuItem_Build.DropDownItems.Add(g_mFtpCompileItem)

                RemoveHandler g_mFtpCompileItem.Click, AddressOf OnCompileItemClick
                AddHandler g_mFtpCompileItem.Click, AddressOf OnCompileItemClick

                'Update all FormMain controls, to change style for the newly created controls
                ClassControlStyle.UpdateControls(g_mFtpMenuBuildSplit)
                ClassControlStyle.UpdateControls(g_mFtpCompileItem)
            End If

            Dim iBuildAllIndex As Integer = g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ContextMenuStrip_ProjectFiles.Items.IndexOf(g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ToolStripMenuItem_CompileAll)
            If (iBuildAllIndex > -1) Then
                g_mFtpCompileAllItem = New ToolStripMenuItem("Upload", My.Resources.imageres_5340_16x16)

                g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ContextMenuStrip_ProjectFiles.Items.Insert(iBuildAllIndex, g_mFtpCompileAllItem)

                RemoveHandler g_mFtpCompileAllItem.Click, AddressOf OnCompileAllItemClick
                AddHandler g_mFtpCompileAllItem.Click, AddressOf OnCompileAllItemClick

                RemoveHandler g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ContextMenuStrip_ProjectFiles.Opening, AddressOf ContextMenuStripProjectFilesOpening
                AddHandler g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ContextMenuStrip_ProjectFiles.Opening, AddressOf ContextMenuStripProjectFilesOpening

                'Update all FormMain controls, to change style for the newly created controls
                ClassControlStyle.UpdateControls(g_mFtpCompileAllItem)
            End If

            'Update all FormMain controls, to change style for the newly created controls
            ClassControlStyle.UpdateControls(g_mFtpMenuSplit)
            ClassControlStyle.UpdateControls(g_mFtpMenuItem)
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

                If (ClassDebuggerTools.ClassDebuggerHelpers.HasDebugPlaceholder(sSource)) Then
                    ClassDebuggerTools.ClassDebuggerHelpers.CleanupDebugPlaceholder(sSource, g_mPluginFTP.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)
                End If

                Dim sSourceFile As String = Nothing
                If (Not g_mPluginFTP.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IsUnsaved AndAlso Not g_mPluginFTP.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                    sSourceFile = g_mPluginFTP.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File
                End If

                Dim sOutputFile As String = ""
                g_mPluginFTP.g_mFormMain.g_ClassTextEditorTools.CompileSource(Nothing,
                                                                              Nothing,
                                                                              sSource,
                                                                              False,
                                                                              False,
                                                                              sOutputFile,
                                                                              Nothing,
                                                                              If(sSourceFile Is Nothing, Nothing, IO.Path.GetDirectoryName(sSourceFile)),
                                                                              Nothing,
                                                                              Nothing,
                                                                              Nothing,
                                                                              Nothing,
                                                                              sSourceFile)

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

                    'Remove Handlers
                    RemoveHandler g_mPluginFTP.g_mFormMain.g_mUCProjectBrowser.ContextMenuStrip_ProjectFiles.Opening, AddressOf ContextMenuStripProjectFilesOpening

                    If (g_mFtpMenuItem IsNot Nothing) Then
                        RemoveHandler g_mFtpMenuItem.Click, AddressOf OnMenuItemClick
                    End If

                    If (g_mFtpCompileItem IsNot Nothing) Then
                        RemoveHandler g_mFtpCompileItem.Click, AddressOf OnCompileItemClick
                    End If

                    If (g_mFtpCompileAllItem IsNot Nothing) Then
                        RemoveHandler g_mFtpCompileAllItem.Click, AddressOf OnCompileAllItemClick
                    End If

                    'Remove Controls
                    If (g_mFtpMenuSplit IsNot Nothing AndAlso Not g_mFtpMenuSplit.IsDisposed) Then
                        g_mFtpMenuSplit.Dispose()
                        g_mFtpMenuSplit = Nothing
                    End If

                    If (g_mFtpMenuBuildSplit IsNot Nothing AndAlso Not g_mFtpMenuBuildSplit.IsDisposed) Then
                        g_mFtpMenuBuildSplit.Dispose()
                        g_mFtpMenuBuildSplit = Nothing
                    End If

                    If (g_mFtpMenuItem IsNot Nothing AndAlso Not g_mFtpMenuItem.IsDisposed) Then
                        g_mFtpMenuItem.Dispose()
                        g_mFtpMenuItem = Nothing
                    End If

                    If (g_mFtpCompileItem IsNot Nothing AndAlso Not g_mFtpCompileItem.IsDisposed) Then
                        g_mFtpCompileItem.Dispose()
                        g_mFtpCompileItem = Nothing
                    End If

                    If (g_mFtpCompileAllItem IsNot Nothing AndAlso Not g_mFtpCompileAllItem.IsDisposed) Then
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
