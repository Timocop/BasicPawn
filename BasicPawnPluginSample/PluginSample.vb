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


Imports BasicPawn
Imports BasicPawnPluginInterface
Imports System.Windows.Forms
Imports System.Drawing

Public Class PluginSample
    Implements IPluginInterface

    Private g_mFormMain As FormMain
    Private g_ClassTestToolbox As ClassTestToolbox


    Public ReadOnly Property m_PluginInformation As IPluginInterface.STRUC_PLUGIN_INFORMATION Implements IPluginInterface.m_PluginInformation
        Get
            Return New IPluginInterface.STRUC_PLUGIN_INFORMATION("Sample Plugin", "Timocop", "A simple sample plugin", "0.0", Nothing)
        End Get
    End Property

    Public Sub OnPluginEndPost() Implements IPluginInterface.OnPluginEndPost
        If (g_ClassTestToolbox IsNot Nothing) Then
            g_ClassTestToolbox.Dispose()
            g_ClassTestToolbox = Nothing
        End If
    End Sub

    Public Sub OnPluginLoad(sDLLPath As String) Implements IPluginInterface.OnPluginLoad
        Throw New NotImplementedException()
    End Sub

    Public Sub OnPluginStart(mFormMain As Object) Implements IPluginInterface.OnPluginStart
        g_mFormMain = DirectCast(mFormMain, FormMain)

        g_ClassTestToolbox = New ClassTestToolbox(Me)
    End Sub

    Public Function OnPluginEnd() As Boolean Implements IPluginInterface.OnPluginEnd
        Throw New NotImplementedException()
    End Function

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

    Public Function OnDebuggerEnd(mFormDebugger As Object) As Boolean Implements IPluginInterface.OnDebuggerEnd
        Throw New NotImplementedException()
    End Function

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

    Public Sub OnDebuggerRefresh(mFormDebugger As Object) Implements IPluginInterface.OnDebuggerRefresh
        Throw New NotImplementedException()
    End Sub

    Class ClassTestToolbox
        Implements IDisposable

        Private g_mPluginSample As PluginSample

        Private mToolboxPage As TabPage
        Private mTestButton As Button
        Private g_mAboutMenuItem As ToolStripMenuItem

        Public Sub New(mPluginSample As PluginSample)
            g_mPluginSample = mPluginSample

            mToolboxPage = New TabPage("Test Toolbox")
            mTestButton = New Button
            g_mAboutMenuItem = New ToolStripMenuItem("About Sample Plugin", BasicPawn.My.Resources.imageres_5314_16x16_32)

            BuildAboutMenu()
            BuildToolbox()
        End Sub

        Private Sub BuildAboutMenu()
            g_mPluginSample.g_mFormMain.MenuStrip_BasicPawn.Items.Add(g_mAboutMenuItem)

            RemoveHandler g_mAboutMenuItem.Click, AddressOf OnMenuItemClick
            AddHandler g_mAboutMenuItem.Click, AddressOf OnMenuItemClick
        End Sub

        Private Sub BuildToolbox()
            mToolboxPage.BackColor = Color.White

            g_mPluginSample.g_mFormMain.TabControl_Toolbox.TabPages.Add(mToolboxPage)

            mTestButton.Parent = mToolboxPage
            mTestButton.Dock = DockStyle.Top
            mTestButton.Text = "Show Messagebox"
            mTestButton.UseVisualStyleBackColor = True

            RemoveHandler mTestButton.Click, AddressOf OnButtonClick
            AddHandler mTestButton.Click, AddressOf OnButtonClick
        End Sub

        Private Sub OnButtonClick(sender As Object, e As EventArgs)
            MsgBox("Hello World!")
        End Sub

        Private Sub OnMenuItemClick(sender As Object, e As EventArgs)
            Using i As New FormAbout(g_mPluginSample)
                i.ShowDialog()
            End Using
        End Sub



#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    RemoveHandler mTestButton.Click, AddressOf OnButtonClick
                    RemoveHandler g_mAboutMenuItem.Click, AddressOf OnMenuItemClick

                    If (g_mAboutMenuItem IsNot Nothing AndAlso Not g_mAboutMenuItem.IsDisposed) Then
                        g_mAboutMenuItem.Dispose()
                        g_mAboutMenuItem = Nothing
                    End If

                    If (mTestButton IsNot Nothing AndAlso Not mTestButton.IsDisposed) Then
                        mTestButton.Dispose()
                        mTestButton = Nothing
                    End If

                    If (mToolboxPage IsNot Nothing AndAlso Not mToolboxPage.IsDisposed) Then
                        mToolboxPage.Dispose()
                        mToolboxPage = Nothing
                    End If
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
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
