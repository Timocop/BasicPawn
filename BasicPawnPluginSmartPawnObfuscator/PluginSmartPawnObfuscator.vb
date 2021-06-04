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


Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports BasicPawn
Imports BasicPawnPluginInterface

Public Class PluginSmartPawnObfuscator
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

        Private g_mPluginSmartPawnObfuscator As PluginSmartPawnObfuscator

        Private g_mObfuscatorMenuSplit As ToolStripSeparator
        Private g_mObfuscatorMenuItem As ToolStripMenuItem
        Private g_mObfuscatorAllMenuItem As ToolStripMenuItem
        Private g_mObfuscatorPresetInfoMenuItem As ToolStripMenuItem
        Private g_mObfuscatorPresetMenuItem As ToolStripComboBox
        Private g_mObfuscatorPoweredInfoMenuItem As ToolStripMenuItem

        Private g_mObfuscatorThread As Threading.Thread = Nothing
        Private g_mFormProgress As FormProgress = Nothing
        Private g_mObfuscatorProcess As Process = Nothing

        Private Enum PRESET_ENUM
            MINIMUM
            NORMAL
            AGGRESSIVE
            MAXIMUM
            MAXIMUM_METADATA_OVERFLOW
            UNVERIFIABLE
            STEALTH
            OPTIMIZED
        End Enum
        Private g_iProtectionPreset As PRESET_ENUM = PRESET_ENUM.NORMAL

        Public Sub New(mPluginTranslationEditor As PluginSmartPawnObfuscator)
            g_mPluginSmartPawnObfuscator = mPluginTranslationEditor

            BuildLysisDecompilerMenu()
        End Sub

        Private Sub BuildLysisDecompilerMenu()
            g_mObfuscatorMenuSplit = New ToolStripSeparator
            g_mObfuscatorMenuItem = New ToolStripMenuItem("Obfuscate current", My.Resources.certmgr_449_16x16_32)
            g_mObfuscatorAllMenuItem = New ToolStripMenuItem("Obfuscate all")
            g_mObfuscatorPresetInfoMenuItem = New ToolStripMenuItem("Obfuscation presets:")
            g_mObfuscatorPresetMenuItem = New ToolStripComboBox()
            g_mObfuscatorPoweredInfoMenuItem = New ToolStripMenuItem("Powered by SmartPawn")
            g_mPluginSmartPawnObfuscator.g_mFormMain.ToolStripMenuItem_Build.DropDownItems.Add(g_mObfuscatorMenuSplit)
            g_mPluginSmartPawnObfuscator.g_mFormMain.ToolStripMenuItem_Build.DropDownItems.Add(g_mObfuscatorMenuItem)
            g_mPluginSmartPawnObfuscator.g_mFormMain.ToolStripMenuItem_Build.DropDownItems.Add(g_mObfuscatorAllMenuItem)
            g_mPluginSmartPawnObfuscator.g_mFormMain.ToolStripMenuItem_Build.DropDownItems.Add(g_mObfuscatorPresetInfoMenuItem)
            g_mPluginSmartPawnObfuscator.g_mFormMain.ToolStripMenuItem_Build.DropDownItems.Add(g_mObfuscatorPresetMenuItem)
            g_mPluginSmartPawnObfuscator.g_mFormMain.ToolStripMenuItem_Build.DropDownItems.Add(g_mObfuscatorPoweredInfoMenuItem)

            g_mObfuscatorPresetInfoMenuItem.Enabled = False
            g_mObfuscatorPoweredInfoMenuItem.Enabled = False
            g_mObfuscatorPresetMenuItem.DropDownStyle = ComboBoxStyle.DropDownList
            g_mObfuscatorPresetMenuItem.FlatStyle = FlatStyle.System
            g_mObfuscatorPresetMenuItem.ComboBox.Width = (g_mObfuscatorPresetMenuItem.ComboBox.Width * 2)

            g_mObfuscatorPresetMenuItem.Items.Clear()
            g_mObfuscatorPresetMenuItem.Items.Add("Minimum")
            g_mObfuscatorPresetMenuItem.Items.Add("Normal")
            g_mObfuscatorPresetMenuItem.Items.Add("Aggressive")
            g_mObfuscatorPresetMenuItem.Items.Add("Maximum")
            g_mObfuscatorPresetMenuItem.Items.Add("Maximum (Metadata Overflow)")
            g_mObfuscatorPresetMenuItem.Items.Add("Unverifiable")
            g_mObfuscatorPresetMenuItem.Items.Add("Stealth")
            g_mObfuscatorPresetMenuItem.Items.Add("Optimized")

            If ([Enum].GetNames(GetType(PRESET_ENUM)).Length <> g_mObfuscatorPresetMenuItem.Items.Count) Then
                Throw New ArgumentException("Invalid preset length")
            End If

            RemoveHandler g_mObfuscatorMenuItem.Click, AddressOf OnMenuItemClick
            RemoveHandler g_mObfuscatorAllMenuItem.Click, AddressOf OnAllMenuItemClick
            RemoveHandler g_mObfuscatorPresetMenuItem.SelectedIndexChanged, AddressOf OnPresetMenuItemClick
            AddHandler g_mObfuscatorMenuItem.Click, AddressOf OnMenuItemClick
            AddHandler g_mObfuscatorAllMenuItem.Click, AddressOf OnAllMenuItemClick
            AddHandler g_mObfuscatorPresetMenuItem.SelectedIndexChanged, AddressOf OnPresetMenuItemClick

            g_mObfuscatorPresetMenuItem.SelectedIndex = g_iProtectionPreset

            'Update all FormMain controls, to change style for the newly created controls 
            ClassControlStyle.UpdateControls(g_mObfuscatorMenuSplit)
            ClassControlStyle.UpdateControls(g_mObfuscatorMenuItem)
            ClassControlStyle.UpdateControls(g_mObfuscatorAllMenuItem)
            ClassControlStyle.UpdateControls(g_mObfuscatorPresetInfoMenuItem)
            ClassControlStyle.UpdateControls(g_mObfuscatorPresetMenuItem)
            ClassControlStyle.UpdateControls(g_mObfuscatorPoweredInfoMenuItem)
        End Sub

        Private Sub OnMenuItemClick(sender As Object, e As EventArgs)
            Try
                If (ClassThread.IsValid(g_mObfuscatorThread)) Then
                    Return
                End If

                Dim mTab = g_mPluginSmartPawnObfuscator.g_mFormMain.g_ClassTabControl.m_ActiveTab
                If (mTab.m_IsUnsaved OrElse mTab.m_InvalidFile) Then
                    Throw New ArgumentException("Please save your source first")
                End If

                StartObfuscation(New String() {mTab.m_File}, g_iProtectionPreset)
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Private Sub OnAllMenuItemClick(sender As Object, e As EventArgs)
            Try
                If (ClassThread.IsValid(g_mObfuscatorThread)) Then
                    Return
                End If

                Dim sFiles As New List(Of String)

                For Each mTab In g_mPluginSmartPawnObfuscator.g_mFormMain.g_ClassTabControl.GetAllTabs
                    If (mTab.m_IsUnsaved OrElse mTab.m_InvalidFile) Then
                        Continue For
                    End If

                    sFiles.Add(mTab.m_File)
                Next

                StartObfuscation(sFiles.ToArray, g_iProtectionPreset)
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Private Sub OnPresetMenuItemClick(sender As Object, e As EventArgs)
            g_iProtectionPreset = CType(g_mObfuscatorPresetMenuItem.SelectedIndex, PRESET_ENUM)
        End Sub

        Private Sub StartObfuscation(sFiles As String(), iPreset As PRESET_ENUM)
            If (sFiles.Length < 1) Then
                Return
            End If

            g_mObfuscatorThread = New Threading.Thread(Sub()
                                                           Try
                                                               For i = 0 To sFiles.Length - 1
                                                                   Try
                                                                       Dim sFilename As String = IO.Path.GetFileName(sFiles(i))

                                                                       ClassThread.ExecEx(Of Object)(g_mPluginSmartPawnObfuscator.g_mFormMain,
                                                                                                Sub()
                                                                                                    If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                                                                                                        g_mFormProgress.Dispose()
                                                                                                        g_mFormProgress = Nothing
                                                                                                    End If

                                                                                                    g_mFormProgress = New FormProgress With {
                                                                                                                                              .Text = String.Format("Obfuscating {0}...", sFilename),
                                                                                                                                              .m_Progress = 0,
                                                                                                                                              .m_ProgressMax = 10
                                                                                                                                          }
                                                                                                    g_mFormProgress.Size = New Drawing.Size(CInt(g_mFormProgress.Size.Width * 1.25), g_mFormProgress.Size.Height)

                                                                                                    g_mFormProgress.m_CloseAction = (Function()
                                                                                                                                         'Ewww quite dirty
                                                                                                                                         g_mFormProgress.Text = "Canceling..."

                                                                                                                                         Try
                                                                                                                                             If (g_mObfuscatorProcess IsNot Nothing AndAlso Not g_mObfuscatorProcess.HasExited) Then
                                                                                                                                                 g_mObfuscatorProcess.Kill()
                                                                                                                                             End If
                                                                                                                                         Catch ex As Exception
                                                                                                                                         End Try

                                                                                                                                         ClassThread.Abort(g_mObfuscatorThread, False)
                                                                                                                                         Return True
                                                                                                                                     End Function)
                                                                                                    g_mFormProgress.Show(g_mPluginSmartPawnObfuscator.g_mFormMain)
                                                                                                End Sub)

                                                                       Dim sSmartPawnCmd As String = IO.Path.Combine(Application.StartupPath, "plugins\BasicPawnPluginSmartPawnObfuscator\SmartPawn.exe")
                                                                       If (Not IO.File.Exists(sSmartPawnCmd)) Then
                                                                           Throw New ArgumentException("SmartPawn does not exist")
                                                                       End If

                                                                       Dim iExitCode As Integer = 0
                                                                       Dim sOutput As New Text.StringBuilder
                                                                       Dim sPreset As String = "0"

                                                                       Select Case (iPreset)
                                                                           Case PRESET_ENUM.MINIMUM
                                                                               sPreset = "1"
                                                                           Case PRESET_ENUM.NORMAL
                                                                               sPreset = "2"
                                                                           Case PRESET_ENUM.AGGRESSIVE
                                                                               sPreset = "3"
                                                                           Case PRESET_ENUM.MAXIMUM
                                                                               sPreset = "4"
                                                                           Case PRESET_ENUM.MAXIMUM_METADATA_OVERFLOW
                                                                               sPreset = "5"
                                                                           Case PRESET_ENUM.UNVERIFIABLE
                                                                               sPreset = "6"
                                                                           Case PRESET_ENUM.STEALTH
                                                                               sPreset = "s"
                                                                           Case PRESET_ENUM.OPTIMIZED
                                                                               sPreset = "o"
                                                                       End Select

                                                                       Try
                                                                           g_mObfuscatorProcess = New Process
                                                                           g_mObfuscatorProcess.StartInfo.FileName = sSmartPawnCmd
                                                                           g_mObfuscatorProcess.StartInfo.Arguments = String.Format("-c -t{0} -s""{1}""", sPreset, sFiles(i))

                                                                           g_mObfuscatorProcess.StartInfo.UseShellExecute = False
                                                                           g_mObfuscatorProcess.StartInfo.CreateNoWindow = True
                                                                           g_mObfuscatorProcess.StartInfo.RedirectStandardOutput = True

                                                                           g_mObfuscatorProcess.Start()

                                                                           'Wait for the first message, the console window should be visible by now.
                                                                           'TODO: Change this behavior in SMartPawn instead. This is a workaround.
                                                                           g_mObfuscatorProcess.StandardOutput.Peek()
                                                                           ClassNative.WindowHide(g_mObfuscatorProcess.MainWindowHandle)

                                                                           While True
                                                                               Dim sLine = g_mObfuscatorProcess.StandardOutput.ReadLine
                                                                               If (sLine Is Nothing) Then
                                                                                   Exit While
                                                                               End If

                                                                               sOutput.AppendLine(sLine)

                                                                               ClassThread.ExecAsync(g_mPluginSmartPawnObfuscator.g_mFormMain,
                                                                                                    Sub()
                                                                                                        If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                                                                                                            g_mFormProgress.Text = String.Format("Obfuscating {0}...", sFilename)

                                                                                                            Dim iProgress = (g_mFormProgress.m_Progress + 1)
                                                                                                            If (iProgress > g_mFormProgress.m_ProgressMax) Then
                                                                                                                iProgress = 0

                                                                                                                g_mFormProgress.m_ProgressMax *= 10
                                                                                                            End If

                                                                                                            g_mFormProgress.m_Progress = iProgress
                                                                                                        End If
                                                                                                    End Sub)
                                                                           End While

                                                                           g_mObfuscatorProcess.WaitForExit()

                                                                           iExitCode = g_mObfuscatorProcess.ExitCode
                                                                       Finally
                                                                           If (g_mObfuscatorProcess IsNot Nothing) Then
                                                                               Try
                                                                                   If (g_mObfuscatorProcess IsNot Nothing AndAlso Not g_mObfuscatorProcess.HasExited) Then
                                                                                       g_mObfuscatorProcess.Kill()
                                                                                   End If
                                                                               Catch ex As Exception
                                                                               End Try

                                                                               g_mObfuscatorProcess.Dispose()
                                                                               g_mObfuscatorProcess = Nothing
                                                                           End If
                                                                       End Try

                                                                       If (iExitCode <> 0) Then
                                                                           Throw New ArgumentException("Obfuscation failed")
                                                                       End If

                                                                       Dim bObfuscationSuccess As Boolean = False

                                                                       If (True) Then
                                                                           Dim sOutputLines = sOutput.ToString.Split(New String() {Environment.NewLine, vbLf}, 0)
                                                                           For j = 0 To sOutputLines.Length - 1
                                                                               If (sOutputLines(j).EndsWith("Obfuscation finished!")) Then
                                                                                   bObfuscationSuccess = True
                                                                                   Exit For
                                                                               End If
                                                                           Next
                                                                       End If


                                                                       ClassThread.ExecAsync(g_mPluginSmartPawnObfuscator.g_mFormMain,
                                                                                        Sub()
                                                                                            g_mPluginSmartPawnObfuscator.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Executing obfuscation...", False, True, True)

                                                                                            Dim sOutputLines = sOutput.ToString.Split(New String() {Environment.NewLine, vbLf}, 0)
                                                                                            For j = 0 To sOutputLines.Length - 1
                                                                                                If (sOutputLines(j).StartsWith("[-] ") OrElse sOutputLines(j).StartsWith("[!] ") OrElse sOutputLines(j).StartsWith("[X] ")) Then
                                                                                                    sOutputLines(j) = sOutputLines(j).Remove(0, 4)
                                                                                                End If

                                                                                                g_mPluginSmartPawnObfuscator.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & sOutputLines(j), False, False, True)
                                                                                            Next

                                                                                            g_mPluginSmartPawnObfuscator.g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Completed obfuscation!", False, False, True)
                                                                                        End Sub)

                                                                       If (Not bObfuscationSuccess AndAlso sFiles.Length > 1) Then
                                                                           With New Text.StringBuilder
                                                                               .AppendFormat("'{0}' was unable to be obfuscated!", sFilename).AppendLine()
                                                                               .AppendLine("See information tab for more information.")
                                                                               .AppendLine()
                                                                               .AppendLine("Do you want to to abort the obfuscation process?")

                                                                               Select Case (MessageBox.Show(.ToString, "Obfuscation failed", MessageBoxButtons.YesNo, MessageBoxIcon.Error))
                                                                                   Case DialogResult.Yes
                                                                                       Return
                                                                               End Select
                                                                           End With
                                                                       End If
                                                                   Finally
                                                                       ClassThread.ExecAsync(g_mPluginSmartPawnObfuscator.g_mFormMain,
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
            g_mObfuscatorThread.IsBackground = True
            g_mObfuscatorThread.Start()
        End Sub

        Class ClassNative
            <DllImport("user32.dll")>
            Private Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
            End Function

            Public Shared Sub WindowHide(hWnd As IntPtr)
                If (hWnd = IntPtr.Zero) Then
                    Return
                End If

                ShowWindow(hWnd, 0)
            End Sub
        End Class

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).  
                    Try
                        If (g_mObfuscatorProcess IsNot Nothing AndAlso Not g_mObfuscatorProcess.HasExited) Then
                            g_mObfuscatorProcess.Kill()
                        End If
                    Catch ex As Exception
                    End Try

                    ClassThread.Abort(g_mObfuscatorThread)

                    If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                        g_mFormProgress.Dispose()
                        g_mFormProgress = Nothing
                    End If

                    If (g_mObfuscatorMenuItem IsNot Nothing) Then
                        RemoveHandler g_mObfuscatorMenuItem.Click, AddressOf OnMenuItemClick
                    End If

                    If (g_mObfuscatorAllMenuItem IsNot Nothing) Then
                        RemoveHandler g_mObfuscatorAllMenuItem.Click, AddressOf OnAllMenuItemClick
                    End If

                    If (g_mObfuscatorPresetMenuItem IsNot Nothing) Then
                        RemoveHandler g_mObfuscatorPresetMenuItem.SelectedIndexChanged, AddressOf OnPresetMenuItemClick
                    End If

                    If (g_mObfuscatorMenuSplit IsNot Nothing AndAlso Not g_mObfuscatorMenuSplit.IsDisposed) Then
                        g_mObfuscatorMenuSplit.Dispose()
                        g_mObfuscatorMenuSplit = Nothing
                    End If

                    If (g_mObfuscatorMenuItem IsNot Nothing AndAlso Not g_mObfuscatorMenuItem.IsDisposed) Then
                        g_mObfuscatorMenuItem.Dispose()
                        g_mObfuscatorMenuItem = Nothing
                    End If

                    If (g_mObfuscatorAllMenuItem IsNot Nothing AndAlso Not g_mObfuscatorAllMenuItem.IsDisposed) Then
                        g_mObfuscatorAllMenuItem.Dispose()
                        g_mObfuscatorAllMenuItem = Nothing
                    End If

                    If (g_mObfuscatorPresetInfoMenuItem IsNot Nothing AndAlso Not g_mObfuscatorPresetInfoMenuItem.IsDisposed) Then
                        g_mObfuscatorPresetInfoMenuItem.Dispose()
                        g_mObfuscatorPresetInfoMenuItem = Nothing
                    End If

                    If (g_mObfuscatorPresetMenuItem IsNot Nothing AndAlso Not g_mObfuscatorPresetMenuItem.IsDisposed) Then
                        g_mObfuscatorPresetMenuItem.Dispose()
                        g_mObfuscatorPresetMenuItem = Nothing
                    End If

                    If (g_mObfuscatorPoweredInfoMenuItem IsNot Nothing AndAlso Not g_mObfuscatorPoweredInfoMenuItem.IsDisposed) Then
                        g_mObfuscatorPoweredInfoMenuItem.Dispose()
                        g_mObfuscatorPoweredInfoMenuItem = Nothing
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
