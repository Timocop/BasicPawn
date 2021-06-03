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


Imports System.Windows.Forms
Imports BasicPawn
Imports BasicPawnPluginInterface

Public Class PluginLysisDecompiler
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

        Private g_mPluginLysisDecompiler As PluginLysisDecompiler

        Private g_mDecompilerMenuSplit As ToolStripSeparator
        Private g_mDecompilerMenuItem As ToolStripMenuItem

        Private g_mDecompileThread As Threading.Thread = Nothing
        Private g_mFormProgress As FormProgress = Nothing
        Private g_mDecompileProcess As Process = Nothing

        Public Sub New(mPluginTranslationEditor As PluginLysisDecompiler)
            g_mPluginLysisDecompiler = mPluginTranslationEditor

            BuildLysisDecompilerMenu()
        End Sub

        Private Sub BuildLysisDecompilerMenu()
            g_mDecompilerMenuSplit = New ToolStripSeparator
            g_mDecompilerMenuItem = New ToolStripMenuItem("Decompiler (Powered by Lysis)", My.Resources.imageres_5330_16x16_32)
            g_mPluginLysisDecompiler.g_mFormMain.ToolStripMenuItem_Tools.DropDownItems.Add(g_mDecompilerMenuSplit)
            g_mPluginLysisDecompiler.g_mFormMain.ToolStripMenuItem_Tools.DropDownItems.Add(g_mDecompilerMenuItem)

            RemoveHandler g_mDecompilerMenuItem.Click, AddressOf OnMenuItemClick
            AddHandler g_mDecompilerMenuItem.Click, AddressOf OnMenuItemClick

            'Update all FormMain controls, to change style for the newly created controls 
            ClassControlStyle.UpdateControls(g_mDecompilerMenuSplit)
            ClassControlStyle.UpdateControls(g_mDecompilerMenuItem)
        End Sub

        Private Sub OnMenuItemClick(sender As Object, e As EventArgs)
            Try
                If (ClassThread.IsValid(g_mDecompileThread)) Then
                    Return
                End If

                Dim sFiles As String() = New String() {}

                Using i As New OpenFileDialog
                    i.Filter = "All supported formats|*.smx;*.amxx|SourceMod Plugin|*.smx|AMX Mod X Plugin|*.amxx"
                    i.Multiselect = True

                    If (i.ShowDialog = DialogResult.OK) Then
                        sFiles = i.FileNames
                    End If
                End Using

                If (sFiles.Length < 1) Then
                    Return
                End If

                g_mDecompileThread = New Threading.Thread(Sub()
                                                              Try
                                                                  For i = 0 To sFiles.Length - 1
                                                                      Try
                                                                          Dim iLinesProcessed As Integer = 0
                                                                          Dim sFilename As String = IO.Path.GetFileName(sFiles(i))

                                                                          ClassThread.ExecEx(Of Object)(g_mPluginLysisDecompiler.g_mFormMain,
                                                                                                    Sub()
                                                                                                        If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                                                                                                            g_mFormProgress.Dispose()
                                                                                                            g_mFormProgress = Nothing
                                                                                                        End If

                                                                                                        g_mFormProgress = New FormProgress With {
                                                                                                                                                  .Text = String.Format("Decompiling {0}...", sFilename),
                                                                                                                                                  .m_Progress = 0,
                                                                                                                                                  .m_ProgressMax = 10
                                                                                                                                              }
                                                                                                        g_mFormProgress.Size = New Drawing.Size(CInt(g_mFormProgress.Size.Width * 1.25), g_mFormProgress.Size.Height)

                                                                                                        g_mFormProgress.m_CloseAction = (Function()
                                                                                                                                             'Ewww quite dirty
                                                                                                                                             g_mFormProgress.Text = "Canceling..."

                                                                                                                                             Try
                                                                                                                                                 If (g_mDecompileProcess IsNot Nothing AndAlso Not g_mDecompileProcess.HasExited) Then
                                                                                                                                                     g_mDecompileProcess.Kill()
                                                                                                                                                 End If
                                                                                                                                             Catch ex As Exception
                                                                                                                                             End Try

                                                                                                                                             ClassThread.Abort(g_mDecompileThread, False)
                                                                                                                                             Return True
                                                                                                                                         End Function)
                                                                                                        g_mFormProgress.Show(g_mPluginLysisDecompiler.g_mFormMain)
                                                                                                    End Sub)

                                                                          Dim sDecompilerCmd As String = IO.Path.Combine(Application.StartupPath, "plugins\BasicPawnPluginLysisDecompiler\LysisDecompiler.exe")
                                                                          If (Not IO.File.Exists(sDecompilerCmd)) Then
                                                                              Throw New ArgumentException("Lysis decompiler console does not exist")
                                                                          End If

                                                                          Dim iExitCode As Integer = 0
                                                                          Dim sOutput As New Text.StringBuilder

                                                                          Try
                                                                              g_mDecompileProcess = New Process
                                                                              g_mDecompileProcess.StartInfo.FileName = sDecompilerCmd
                                                                              g_mDecompileProcess.StartInfo.Arguments = String.Format("""{0}""", sFiles(i))

                                                                              g_mDecompileProcess.StartInfo.UseShellExecute = False
                                                                              g_mDecompileProcess.StartInfo.CreateNoWindow = True
                                                                              g_mDecompileProcess.StartInfo.RedirectStandardOutput = True

                                                                              g_mDecompileProcess.Start()

                                                                              While True
                                                                                  Dim sLine = g_mDecompileProcess.StandardOutput.ReadLine
                                                                                  If (sLine Is Nothing) Then
                                                                                      Exit While
                                                                                  End If

                                                                                  sOutput.AppendLine(sLine)
                                                                                  iLinesProcessed += 1

                                                                                  If ((iLinesProcessed Mod 100) = 0) Then

                                                                                      ClassThread.ExecAsync(g_mPluginLysisDecompiler.g_mFormMain,
                                                                                                            Sub()
                                                                                                                If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                                                                                                                    g_mFormProgress.Text = String.Format("Decompiling {0}... ({1} lines processed)", sFilename, iLinesProcessed)

                                                                                                                    Dim iProgress = (g_mFormProgress.m_Progress + 1)
                                                                                                                    If (iProgress > g_mFormProgress.m_ProgressMax) Then
                                                                                                                        iProgress = 0

                                                                                                                        g_mFormProgress.m_ProgressMax *= 10
                                                                                                                    End If

                                                                                                                    g_mFormProgress.m_Progress = iProgress
                                                                                                                End If
                                                                                                            End Sub)
                                                                                  End If
                                                                              End While

                                                                              g_mDecompileProcess.WaitForExit()

                                                                              iExitCode = g_mDecompileProcess.ExitCode
                                                                          Finally
                                                                              If (g_mDecompileProcess IsNot Nothing) Then
                                                                                  Try
                                                                                      If (g_mDecompileProcess IsNot Nothing AndAlso Not g_mDecompileProcess.HasExited) Then
                                                                                          g_mDecompileProcess.Kill()
                                                                                      End If
                                                                                  Catch ex As Exception
                                                                                  End Try

                                                                                  g_mDecompileProcess.Dispose()
                                                                                  g_mDecompileProcess = Nothing
                                                                              End If
                                                                          End Try

                                                                          If (iExitCode <> 0) Then
                                                                              Throw New ArgumentException("Decompilation failed")
                                                                          End If

                                                                          If (iLinesProcessed > UInt16.MaxValue) Then
                                                                              If (MessageBox.Show(String.Format("A total of {0} lines have been decompiled but you might run out of memory if you show that much decompiled code in the editor." & Environment.NewLine & Environment.NewLine & "Are you sure you want to load the decompiled results into the editor?", iLinesProcessed), "Decompiler Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.No) Then
                                                                                  Continue For
                                                                              End If
                                                                          End If

                                                                          Dim sTempFile As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
                                                                          IO.File.WriteAllText(sTempFile, sOutput.ToString)

                                                                          ClassThread.ExecAsync(g_mPluginLysisDecompiler.g_mFormMain,
                                                                                                Sub()
                                                                                                    Dim mTab = g_mPluginLysisDecompiler.g_mFormMain.g_ClassTabControl.AddTab()
                                                                                                    mTab.OpenFileTab(sTempFile)
                                                                                                    mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)

                                                                                                    g_mPluginLysisDecompiler.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User opened decompiled source")
                                                                                                End Sub)

                                                                      Finally
                                                                          ClassThread.ExecAsync(g_mPluginLysisDecompiler.g_mFormMain,
                                                                                            Sub()
                                                                                                If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                                                                                                    g_mFormProgress.Dispose()
                                                                                                    g_mFormProgress = Nothing
                                                                                                End If
                                                                                            End Sub)
                                                                      End Try
                                                                  Next

                                                              Catch ex As Threading.ThreadAbortException
                                                                  Throw
                                                              Catch ex As Exception
                                                                  ClassExceptionLog.WriteToLogMessageBox(ex)
                                                              End Try
                                                          End Sub)
                g_mDecompileThread.IsBackground = True
                g_mDecompileThread.Start()
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
                    Try
                        If (g_mDecompileProcess IsNot Nothing AndAlso Not g_mDecompileProcess.HasExited) Then
                            g_mDecompileProcess.Kill()
                        End If
                    Catch ex As Exception
                    End Try

                    ClassThread.Abort(g_mDecompileThread)

                    If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                        g_mFormProgress.Dispose()
                        g_mFormProgress = Nothing
                    End If

                    If (g_mDecompilerMenuItem IsNot Nothing) Then
                        RemoveHandler g_mDecompilerMenuItem.Click, AddressOf OnMenuItemClick
                    End If

                    If (g_mDecompilerMenuSplit IsNot Nothing AndAlso Not g_mDecompilerMenuSplit.IsDisposed) Then
                        g_mDecompilerMenuSplit.Dispose()
                        g_mDecompilerMenuSplit = Nothing
                    End If

                    If (g_mDecompilerMenuItem IsNot Nothing AndAlso Not g_mDecompilerMenuItem.IsDisposed) Then
                        g_mDecompilerMenuItem.Dispose()
                        g_mDecompilerMenuItem = Nothing
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
