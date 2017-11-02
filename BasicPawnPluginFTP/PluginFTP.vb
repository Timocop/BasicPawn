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
Imports BasicPawnPluginInterface

Public Class PluginFTP
    Implements IPluginInterface

    Public g_mFormMain As FormMain

    Private g_ClassPlugin As ClassPlugin

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
                                                                 "1.0",
                                                                 Nothing)
        End Get
    End Property

    Public Sub OnPluginStart(mFormMain As Object, bEnabled As Boolean) Implements IPluginInterface.OnPluginStart
        g_mFormMain = DirectCast(mFormMain, FormMain)

        If (bEnabled) Then
            g_ClassPlugin = New ClassPlugin(Me)
        End If
    End Sub

    Public ReadOnly Property m_PluginEnabled As Boolean Implements IPluginInterface.m_PluginEnabled
        Get
            Return (g_ClassPlugin IsNot Nothing)
        End Get
    End Property

    Public Sub OnPluginEndPost() Implements IPluginInterface.OnPluginEndPost
        If (g_ClassPlugin IsNot Nothing) Then
            g_ClassPlugin.Dispose()
            g_ClassPlugin = Nothing
        End If
    End Sub

    Public Function OnPluginEnabled(ByRef sReason As String) As Boolean Implements IPluginInterface.OnPluginEnabled
        If (g_ClassPlugin Is Nothing) Then
            g_ClassPlugin = New ClassPlugin(Me)
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

    Class ClassPlugin
        Implements IDisposable

        Public g_mPluginFTP As PluginFTP

        Private g_mFtpMenuSplit As ToolStripSeparator
        Private g_mFtpMenuItem As ToolStripMenuItem
        Private g_mFtpCompileItem As ToolStripMenuItem

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
        End Sub

        Private Sub OnMenuItemClick(sender As Object, e As EventArgs)
            Try
                Using i As New FormFTP(g_mPluginFTP, "")
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


#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    RemoveHandler g_mFtpMenuItem.Click, AddressOf OnMenuItemClick
                    RemoveHandler g_mFtpCompileItem.Click, AddressOf OnCompileItemClick

                    If (g_mFtpMenuSplit IsNot Nothing AndAlso Not g_mFtpMenuSplit.IsDisposed) Then
                        g_mFtpMenuSplit.Dispose()
                        g_mFtpMenuSplit = Nothing
                    End If

                    If (g_mFtpMenuItem IsNot Nothing AndAlso Not g_mFtpMenuItem.IsDisposed) Then
                        g_mFtpMenuItem.Dispose()
                        g_mFtpMenuItem = Nothing
                    End If

                    If (g_mFtpCompileItem IsNot Nothing AndAlso Not g_mFtpCompileItem.IsDisposed) Then
                        g_mFtpCompileItem.Dispose()
                        g_mFtpCompileItem = Nothing
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
