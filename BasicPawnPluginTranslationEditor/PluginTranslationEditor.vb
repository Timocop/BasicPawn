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


Imports BasicPawn
Imports BasicPawnPluginInterface
Imports System.Windows.Forms
Imports System.Drawing


Public Class PluginTranslationEditor
    Implements IPluginInterfaceV10

    Public g_mFormMain As FormMain
    Private g_ClassPlugin As ClassPlugin

#Region "Unused"
    Public Sub OnPluginLoad(sDLLPath As String) Implements IPluginInterfaceV10.OnPluginLoad
        Throw New NotImplementedException()
    End Sub

    Public Function OnPluginEnd() As Boolean Implements IPluginInterfaceV10.OnPluginEnd
        Throw New NotImplementedException()
    End Function

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

    Public Function OnDebuggerEnd(mFormDebugger As Object) As Boolean Implements IPluginInterfaceV10.OnDebuggerEnd
        Throw New NotImplementedException()
    End Function

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

    Public Sub OnDebuggerRefresh(mFormDebugger As Object) Implements IPluginInterfaceV10.OnDebuggerRefresh
        Throw New NotImplementedException()
    End Sub
#End Region

    Public Sub OnPluginStart(mFormMain As Object, bEnabled As Boolean) Implements IPluginInterfaceV10.OnPluginStart
        g_mFormMain = DirectCast(mFormMain, FormMain)

        If (bEnabled) Then
            g_ClassPlugin = New ClassPlugin(Me)
        End If
    End Sub

    Public Sub OnPluginEndPost() Implements IPluginInterfaceV10.OnPluginEndPost
        If (g_ClassPlugin IsNot Nothing) Then
            g_ClassPlugin.Dispose()
            g_ClassPlugin = Nothing
        End If
    End Sub

    Public ReadOnly Property m_PluginEnabled As Boolean Implements IPluginInterfaceV10.m_PluginEnabled
        Get
            Return (g_ClassPlugin IsNot Nothing)
        End Get
    End Property

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

        Private g_mPluginTranslationEditor As PluginTranslationEditor

        Private g_mFtpMenuSplit As ToolStripSeparator
        Private g_mFtpMenuItem As ToolStripMenuItem

        Private g_mFormTranslationEditor As FormTranslationEditor

        Public Sub New(mPluginTranslationEditor As PluginTranslationEditor)
            g_mPluginTranslationEditor = mPluginTranslationEditor

            BuildTranslationEditorMenu()
        End Sub

        Private Sub BuildTranslationEditorMenu()
            g_mFtpMenuSplit = New ToolStripSeparator
            g_mFtpMenuItem = New ToolStripMenuItem("Translation Editor", My.Resources.accessibilitycpl_325_16x16_32)
            g_mPluginTranslationEditor.g_mFormMain.ToolStripMenuItem_Tools.DropDownItems.Add(g_mFtpMenuSplit)
            g_mPluginTranslationEditor.g_mFormMain.ToolStripMenuItem_Tools.DropDownItems.Add(g_mFtpMenuItem)

            RemoveHandler g_mFtpMenuItem.Click, AddressOf OnMenuItemClick
            AddHandler g_mFtpMenuItem.Click, AddressOf OnMenuItemClick

            'Update all FormMain controls, to change style for the newly created controls 
            ClassControlStyle.UpdateControls(g_mFtpMenuSplit)
            ClassControlStyle.UpdateControls(g_mFtpMenuItem)
        End Sub

        Private Sub OnMenuItemClick(sender As Object, e As EventArgs)
            Try
                If (g_mFormTranslationEditor Is Nothing OrElse g_mFormTranslationEditor.IsDisposed) Then
                    g_mFormTranslationEditor = New FormTranslationEditor(g_mPluginTranslationEditor)
                    g_mFormTranslationEditor.Show()
                Else
                    If (g_mFormTranslationEditor.WindowState = FormWindowState.Minimized) Then
                        ClassTools.ClassForms.FormWindowCommand(g_mFormTranslationEditor, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                    End If

                    g_mFormTranslationEditor.Activate()
                End If
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
                    If (g_mFtpMenuItem IsNot Nothing) Then
                        RemoveHandler g_mFtpMenuItem.Click, AddressOf OnMenuItemClick
                    End If

                    If (g_mFtpMenuSplit IsNot Nothing AndAlso Not g_mFtpMenuSplit.IsDisposed) Then
                        g_mFtpMenuSplit.Dispose()
                        g_mFtpMenuSplit = Nothing
                    End If

                    If (g_mFtpMenuItem IsNot Nothing AndAlso Not g_mFtpMenuItem.IsDisposed) Then
                        g_mFtpMenuItem.Dispose()
                        g_mFtpMenuItem = Nothing
                    End If

                    If (g_mFormTranslationEditor IsNot Nothing AndAlso Not g_mFormTranslationEditor.IsDisposed) Then
                        g_mFormTranslationEditor.Dispose()
                        g_mFormTranslationEditor = Nothing
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
