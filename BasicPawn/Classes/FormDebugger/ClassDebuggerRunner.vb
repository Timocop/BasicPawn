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


Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions

Public Class ClassDebuggerRunner
    Implements IDisposable

    Const SM_LOG_READ_LINES_MAX = 100

    Public g_mFormDebugger As FormDebugger

    Public g_ClassPreProcess As ClassPreProcess

    Private g_mFormDebuggerException As FormDebuggerException
    Private g_mFormDebuggerCriticalPopupException As FormDebuggerCriticalPopup
    Private g_mFormDebuggerCriticalPopupFatalException As FormDebuggerCriticalPopup

    Private g_mListViewEntitiesUpdaterThread As Threading.Thread
    Private Const g_iListViewEntitesUpdaterTime As Integer = 2500

    Private g_mRememberAction As FormDebuggerStop.ENUM_DIALOG_RESULT = FormDebuggerStop.ENUM_DIALOG_RESULT.DO_NOTHING
    Private g_bRememberAction As Boolean = False

    Enum ENUM_FILESYSTEMWATCHER_TYPES
        BREAKPOINTS
        WATCHERS
        ENTITIES
        EXCEPTIONS
        FATAL_EXCEPTIONS
        PING
    End Enum
    Private g_mFileSystemWatcherArray([Enum].GetNames(GetType(ENUM_FILESYSTEMWATCHER_TYPES)).Length - 1) As IO.FileSystemWatcher
    Private g_mFileSystemWatcherLock As New Object

    Enum ENUM_DEBUGGING_STATE
        STARTED
        PAUSED
        STOPPED
    End Enum
    Private g_mDebuggingState As ENUM_DEBUGGING_STATE = ENUM_DEBUGGING_STATE.STOPPED

    Private g_sPluginIdentity As String = ""

    Private g_bSuspendGame As Boolean = False

    Private g_sLatestDebuggerPlugin As String = ""
    Private g_sLatestDebuggerRunnerPlugin As String = ""

    Private g_sClientFolder As String = ""
    Private g_sServerFolder As String = ""
    Private g_sSourceModFolder As String = ""
    Private g_sCurrentSourceFile As String = ""
    Private g_sCurrentSource As String = ""
    Private g_mCurrentConfig As ClassConfigs.STRUC_CONFIG_ITEM

    Structure STURC_SOURCE_LINES_INFO_ITEM
        Dim iRealLine As Integer
        Dim sFile As String
    End Structure
    Public g_mSourceLinesInfo As STURC_SOURCE_LINES_INFO_ITEM()

    Enum ENUM_BREAKPOINT_VALUE_TYPE
        [INTEGER]
        FLOAT
    End Enum

    Structure STRUC_ACTIVE_BREAKPOINT_INFORMATION
        Dim sGUID As String
        Dim bReturnCustomValue As Boolean
        Dim mValueType As ENUM_BREAKPOINT_VALUE_TYPE
        Dim sIntegerValue As String
        Dim sFloatValue As String
        Dim sOrginalIntegerValue As String
        Dim sOrginalFloatValue As String
    End Structure
    Public g_mActiveBreakpointValue As STRUC_ACTIVE_BREAKPOINT_INFORMATION

    Public Sub New(f As FormDebugger)
        g_mFormDebugger = f

        UpdateSourceFromTab()

        g_ClassPreProcess = New ClassPreProcess(Me)
    End Sub

    Public ReadOnly Property m_ClientFolder As String
        Get
            Return g_sClientFolder
        End Get
    End Property

    Public ReadOnly Property m_ServerFolder As String
        Get
            Return g_sServerFolder
        End Get
    End Property

    Public ReadOnly Property m_SourceModFolder As String
        Get
            Return g_sSourceModFolder
        End Get
    End Property

    Public ReadOnly Property m_CurrentSourceFile As String
        Get
            Return g_sCurrentSourceFile
        End Get
    End Property

    Public ReadOnly Property m_CurrentSource As String
        Get
            Return g_sCurrentSource
        End Get
    End Property

    Public ReadOnly Property m_CurrentConfig As ClassConfigs.STRUC_CONFIG_ITEM
        Get
            Return g_mCurrentConfig
        End Get
    End Property

    Public Property m_PluginIdentity As String
        Get
            Return g_sPluginIdentity
        End Get
        Set(value As String)
            g_sPluginIdentity = value
        End Set
    End Property

    Public Property m_DebuggingState As ENUM_DEBUGGING_STATE
        Get
            Return g_mDebuggingState
        End Get
        Set(value As ENUM_DEBUGGING_STATE)
            g_mDebuggingState = value
        End Set
    End Property

    Public Property m_SuspendGame As Boolean
        Get
            Return g_bSuspendGame
        End Get
        Set(value As Boolean)
            If (g_bSuspendGame <> value) Then
                g_bSuspendGame = value

                Dim sClientParentFolder As String = IO.Directory.GetParent(m_ClientFolder).FullName
                Dim sServerParentFolder As String = IO.Directory.GetParent(m_ServerFolder).FullName

                For Each mProcess As Process In Process.GetProcesses
                    Try
                        If (mProcess.Id = Process.GetCurrentProcess.Id) Then
                            Continue For
                        End If

                        Dim sFullPath As String = IO.Path.GetFullPath(mProcess.MainModule.FileName)
                        If (sFullPath.ToLower = IO.Path.GetFullPath(Application.ExecutablePath).ToLower) Then
                            Continue For
                        End If

                        If ((sClientParentFolder IsNot Nothing AndAlso sClientParentFolder.Length > 3 AndAlso sFullPath.ToLower.StartsWith(sClientParentFolder.ToLower)) OrElse
                                    (sServerParentFolder IsNot Nothing AndAlso sServerParentFolder.Length > 3 AndAlso sFullPath.ToLower.StartsWith(sServerParentFolder.ToLower))) Then
                            If (value) Then
                                WinNative.SuspendProcess(mProcess)
                            Else
                                WinNative.ResumeProcess(mProcess)
                            End If
                        End If
                    Catch ex As Exception
                        'Ignore access denied from AVs and co.
                    End Try
                Next
            Else
                g_bSuspendGame = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' Enable/Disable breakpoints via I/O. So the Debugger Runner can read them.
    ''' </summary>
    ''' <param name="sGUID"></param>
    ''' <returns></returns>
    Public Property m_IgnoreBreakpointGUID(sGUID As String) As Boolean
        Get
            If (String.IsNullOrEmpty(sGUID)) Then
                Throw New ArgumentException("Guid empty")
            End If

            Dim sIgnoreExt As String = ClassDebuggerParser.g_sDebuggerBreakpointIgnoreExt
            Dim sFile As String = IO.Path.Combine(m_ServerFolder, sGUID & sIgnoreExt)

            Return IO.File.Exists(sFile)
        End Get
        Set(value As Boolean)
            If (String.IsNullOrEmpty(sGUID)) Then
                Throw New ArgumentException("Guid empty")
            End If

            Dim sIgnoreExt As String = ClassDebuggerParser.g_sDebuggerBreakpointIgnoreExt
            Dim sFile As String = IO.Path.Combine(m_ServerFolder, sGUID & sIgnoreExt)

            If (value) Then
                IO.File.WriteAllText(sFile, "")
            Else
                IO.File.Delete(sFile)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Updates the source from the debug tab. 
    ''' </summary>
    Public Sub UpdateSourceFromTab()
        Dim iIndex As Integer = g_mFormDebugger.g_mFormMain.g_ClassTabControl.GetTabIndexByIdentifier(g_mFormDebugger.g_sDebugTabIdentifier)
        If (iIndex < 0) Then
            Throw New ArgumentException("Tab does not exist")
        End If

        g_sCurrentSourceFile = g_mFormDebugger.g_mFormMain.g_ClassTabControl.m_Tab(iIndex).m_File
        g_sCurrentSource = g_mFormDebugger.g_mFormMain.g_ClassTabControl.m_Tab(iIndex).m_TextEditor.Document.TextContent
        g_sClientFolder = g_mFormDebugger.g_mFormMain.g_ClassTabControl.m_Tab(iIndex).m_ActiveConfig.g_sDebugClientFolder
        g_sServerFolder = g_mFormDebugger.g_mFormMain.g_ClassTabControl.m_Tab(iIndex).m_ActiveConfig.g_sDebugServerFolder
        g_sSourceModFolder = g_mFormDebugger.g_mFormMain.g_ClassTabControl.m_Tab(iIndex).m_ActiveConfig.g_sDebugSourceModFolder
        g_mCurrentConfig = g_mFormDebugger.g_mFormMain.g_ClassTabControl.m_Tab(iIndex).m_ActiveConfig
    End Sub

    ''' <summary>
    ''' Updates the current breakpoints in the breakpoint ListView
    ''' WARN: UI elements!
    ''' </summary>
    Public Sub UpdateBreakpointListView()
        If (String.IsNullOrEmpty(g_mActiveBreakpointValue.sGUID)) Then
            For i = 0 To g_mFormDebugger.ListView_Breakpoints.Items.Count - 1
                Dim mListViewItemData = TryCast(g_mFormDebugger.ListView_Breakpoints.Items(i), ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                If (ClassControlStyle.m_IsInvertedColors) Then
                    mListViewItemData.BackColor = ClassControlStyle.g_cDarkControlColor.mDarkBackground
                Else
                    mListViewItemData.BackColor = ClassControlStyle.g_cDarkControlColor.mLightBackground
                End If

                mListViewItemData.SubItems(2).Text = ""
            Next
        Else
            For i = 0 To g_mFormDebugger.ListView_Breakpoints.Items.Count - 1
                Dim mListViewItemData = TryCast(g_mFormDebugger.ListView_Breakpoints.Items(i), ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                If (g_mActiveBreakpointValue.sGUID = CStr(mListViewItemData.g_mData("GUID"))) Then
                    mListViewItemData.BackColor = Color.Red
                    mListViewItemData.Selected = True
                    mListViewItemData.Selected = False

                    'ListView_Breakpoints.Select()
                    g_mFormDebugger.TabControl1.SelectTab(0)

                    If (g_mActiveBreakpointValue.bReturnCustomValue) Then
                        mListViewItemData.SubItems(2).Text = String.Format("i:{0} | f:{1}", g_mActiveBreakpointValue.sIntegerValue, g_mActiveBreakpointValue.sFloatValue.Replace(",", "."))
                    Else
                        mListViewItemData.SubItems(2).Text = String.Format("i:{0} | f:{1}", g_mActiveBreakpointValue.sOrginalIntegerValue, g_mActiveBreakpointValue.sOrginalFloatValue.Replace(",", "."))
                    End If
                End If
            Next
        End If
    End Sub

    Public Class ClassPreProcess
        Private g_ClassDebuggerRunner As ClassDebuggerRunner

        Public Sub New(c As ClassDebuggerRunner)
            g_ClassDebuggerRunner = c
        End Sub

        ''' <summary>
        ''' Fixes some of the #file errors made by the compiler.
        ''' WARN: Pre-Process source only!
        ''' 
        '''         MyFunc()#file "MySource.sp"
        '''     should be
        '''         MyFunc()
        '''         #file "MySource.sp"
        ''' </summary>
        ''' <param name="sSource"></param>
        Public Sub FixPreProcessFiles(ByRef sSource As String)
            Dim mFileMatches As MatchCollection = Regex.Matches(sSource, "(?<IsNewline>^\s*){0,1}#file\s+(?<Path>.*?)$", RegexOptions.Multiline)
            Dim mSourceBuilder As New Text.StringBuilder(sSource)

            For i = mFileMatches.Count - 1 To 0 Step -1
                Dim sPath As String = mFileMatches(i).Groups("Path").Value.Trim.Trim(""""c)

                If (mFileMatches(i).Groups("IsNewline").Success) Then
                    mSourceBuilder = mSourceBuilder.Remove(mFileMatches(i).Index, mFileMatches(i).Value.Length)
                    mSourceBuilder = mSourceBuilder.Insert(mFileMatches(i).Index, String.Format("#file ""{0}""", sPath))
                Else
                    mSourceBuilder = mSourceBuilder.Remove(mFileMatches(i).Index, mFileMatches(i).Value.Length)
                    mSourceBuilder = mSourceBuilder.Insert(mFileMatches(i).Index, String.Format("{0}#file ""{1}""", Environment.NewLine, sPath))
                End If
            Next

            sSource = mSourceBuilder.ToString
        End Sub

        ''' <summary>
        ''' Analysis the source code to get all real lines and files.
        ''' WARN: Pre-Process source only!
        ''' </summary>
        ''' <param name="sSource"></param>
        Public Sub AnalysisSourceLines(sSource As String)
            Dim lineInfo As New List(Of STURC_SOURCE_LINES_INFO_ITEM)

            Dim iCurrentFakeLine As Integer = 0

            Dim iCurrentLine As Integer = 0
            Dim sCurrentFile As String = ""

            Dim sLine As String = ""
            Using mSR As New IO.StringReader(sSource)
                While True
                    iCurrentFakeLine += 1
                    iCurrentLine += 1

                    sLine = mSR.ReadLine
                    If (sLine Is Nothing) Then
                        lineInfo.Add(New STURC_SOURCE_LINES_INFO_ITEM() With {.iRealLine = iCurrentLine, .sFile = sCurrentFile})
                        Exit While
                    End If

                    Dim mLine As Match = Regex.Match(sLine, "#line\s+(?<Line>[0-9]+)")
                    If (mLine.Success) Then
                        iCurrentLine = CInt(mLine.Groups("Line").Value) - 1

                        lineInfo.Add(New STURC_SOURCE_LINES_INFO_ITEM() With {.iRealLine = iCurrentLine, .sFile = sCurrentFile})
                        Continue While
                    End If

                    Dim mFile As Match = Regex.Match(sLine, "#file\s+(?<Path>.*?)$")
                    If (mFile.Success) Then
                        sCurrentFile = mFile.Groups("Path").Value.Trim(" "c, """"c)
                        iCurrentLine -= 1

                        lineInfo.Add(New STURC_SOURCE_LINES_INFO_ITEM() With {.iRealLine = iCurrentLine, .sFile = sCurrentFile})
                        Continue While
                    End If

                    lineInfo.Add(New STURC_SOURCE_LINES_INFO_ITEM() With {.iRealLine = iCurrentLine, .sFile = sCurrentFile})
                End While
            End Using

            g_ClassDebuggerRunner.g_mSourceLinesInfo = lineInfo.ToArray
        End Sub

        ''' <summary>
        ''' Makes the Pre-Process source ready for compiling.
        ''' WARN: Pre-Process source only!
        ''' </summary>
        ''' <param name="sSource"></param>
        Public Sub FinishSource(ByRef sSource As String)
            If (String.IsNullOrEmpty(g_ClassDebuggerRunner.m_PluginIdentity)) Then
                Throw New ArgumentException("Plugin identity invalid")
            End If

            Dim sourceFinished As New Text.StringBuilder

            Dim sLine As String = ""
            Using mSR As New IO.StringReader(sSource)
                While True
                    sLine = mSR.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    If (Regex.IsMatch(sLine, "#line\s+(?<Line>[0-9]+)")) Then
                        sourceFinished.AppendLine("")
                        Continue While
                    End If

                    If (Regex.IsMatch(sLine, "#file\s+(?<Path>.*?)$")) Then
                        sourceFinished.AppendLine("")
                        Continue While
                    End If

                    sourceFinished.AppendLine(sLine)
                End While
            End Using

            sourceFinished.AppendLine(String.Format("#file ""{0}""", g_ClassDebuggerRunner.m_PluginIdentity))

            sSource = sourceFinished.ToString
        End Sub
    End Class

    ''' <summary>
    ''' Start the debugger.
    ''' Create all elements for debugging here.
    ''' </summary>
    Public Function StartDebugging() As Boolean
        Try
            If (m_DebuggingState <> ENUM_DEBUGGING_STATE.STOPPED) Then
                Return True
            End If

            g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Starting debugger..."
            g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Orange
            g_mFormDebugger.ToolStripStatusLabel_NoConnection.Visible = False

            'Set unique plugin identity
            m_PluginIdentity = Guid.NewGuid.ToString

            'Check game and sourcemod directorys 
            Dim sClientFolder As String = m_ClientFolder
            Dim sServerFolder As String = m_ServerFolder
            Dim sModFolder As String = m_SourceModFolder
            If (Not IO.Directory.Exists(sClientFolder)) Then
                Throw New ArgumentException("Invalid client directory")
            End If
            If (Not IO.Directory.Exists(sServerFolder)) Then
                Throw New ArgumentException("Invalid server directory")
            End If
            If (Not IO.Directory.Exists(sModFolder)) Then
                Throw New ArgumentException("Invalid SourceMod directory")
            End If

            'TODO: May add AMX Mod X support?
            Dim sClientInfo As String = IO.Path.Combine(sClientFolder, "gameinfo.txt")
            Dim sServerInfo As String = IO.Path.Combine(sServerFolder, "gameinfo.txt")
            Dim sSourceModBin As String = IO.Path.Combine(sModFolder, "bin\sourcemod_mm.dll")
            If (Not IO.File.Exists(sClientInfo)) Then
                Throw New ArgumentException("Invalid client directory")
            End If
            If (Not IO.File.Exists(sServerInfo)) Then
                Throw New ArgumentException("Invalid server directory")
            End If
            If (Not IO.File.Exists(sSourceModBin)) Then
                Throw New ArgumentException("Invalid SourceMod directory")
            End If


            g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Starting I/O communicator..."

            'Setup I/O events
            CreateFileSystemWatcher(sServerFolder)

            'Setup listview entities updater 
            ClassThread.Abort(g_mListViewEntitiesUpdaterThread)

            g_mListViewEntitiesUpdaterThread = New Threading.Thread(AddressOf ListViewEntitiesUpdaterThread) With {
                .Priority = Threading.ThreadPriority.Lowest,
                .IsBackground = True
            }
            g_mListViewEntitiesUpdaterThread.Start()


            g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Compiling plugin and BasicPawn modules..."

            'Export debugger cmd runner engine
            If (True) Then
                Dim iCompilerType As ClassTextEditorTools.ENUM_COMPILER_TYPE = ClassTextEditorTools.ENUM_COMPILER_TYPE.UNKNOWN

                Dim sSource As String = g_mFormDebugger.g_ClassDebuggerRunnerEngine.GenerateRunnerEngine(False)
                Dim sOutputFile As String = IO.Path.Combine(m_SourceModFolder, String.Format("plugins\BasicPawnDebugCmdRunEngine-{0}.unk", Guid.NewGuid.ToString))
                g_sLatestDebuggerRunnerPlugin = sOutputFile

                If (Not g_mFormDebugger.g_mFormMain.g_ClassTextEditorTools.CompileSource(False, sSource, sOutputFile, m_CurrentConfig, Nothing, Nothing, Nothing, Nothing, True, Nothing, iCompilerType)) Then
                    Throw New ArgumentException("Compiler failure! See information tab for more information. (BasicPawn Debug Cmd Runner Engine)")
                End If

                If (iCompilerType <> ClassTextEditorTools.ENUM_COMPILER_TYPE.SOURCEPAWN) Then
                    Throw New ArgumentException("Unsupported compiler")
                End If

                g_sLatestDebuggerRunnerPlugin = sOutputFile
            End If

            'Export main plugin source
            If (True) Then
                Dim iCompilerType As ClassTextEditorTools.ENUM_COMPILER_TYPE = ClassTextEditorTools.ENUM_COMPILER_TYPE.UNKNOWN

                Dim sSource As String = g_mFormDebugger.TextEditorControlEx_DebuggerSource.Document.TextContent
                Dim sOutputFile As String = IO.Path.Combine(m_SourceModFolder, String.Format("plugins\BasicPawnDebug-{0}.unk", Guid.NewGuid.ToString))
                g_sLatestDebuggerPlugin = sOutputFile

                g_mFormDebugger.g_ClassDebuggerParser.UpdateBreakpoints(sSource, True, g_mFormDebugger.g_iLanguage)
                With New ClassDebuggerParser.ClassBreakpoints(g_mFormDebugger.g_mFormMain)
                    .CompilerReady(sSource, g_mFormDebugger.g_ClassDebuggerParser, g_mFormDebugger.g_iLanguage)
                End With
                g_mFormDebugger.g_ClassDebuggerParser.UpdateBreakpoints(g_mFormDebugger.TextEditorControlEx_DebuggerSource.Document.TextContent, True, g_mFormDebugger.g_iLanguage)

                g_mFormDebugger.g_ClassDebuggerParser.UpdateWatchers(sSource, True, g_mFormDebugger.g_iLanguage)
                With New ClassDebuggerParser.ClassWatchers(g_mFormDebugger.g_mFormMain)
                    .CompilerReady(sSource, g_mFormDebugger.g_ClassDebuggerParser, g_mFormDebugger.g_iLanguage)
                End With
                g_mFormDebugger.g_ClassDebuggerParser.UpdateWatchers(g_mFormDebugger.TextEditorControlEx_DebuggerSource.Document.TextContent, True, g_mFormDebugger.g_iLanguage)

                g_ClassPreProcess.FinishSource(sSource)

                If (Not g_mFormDebugger.g_mFormMain.g_ClassTextEditorTools.CompileSource(False, sSource, sOutputFile, m_CurrentConfig, Nothing, Nothing, Nothing, Nothing, True, Nothing, iCompilerType)) Then
                    Throw New ArgumentException("Compiler failure! See information tab for more information. (BasicPawn Debug Main Plugin)")
                End If

                If (iCompilerType <> ClassTextEditorTools.ENUM_COMPILER_TYPE.SOURCEPAWN) Then
                    Throw New ArgumentException("Unsupported compiler")
                End If

                g_sLatestDebuggerPlugin = sOutputFile
            End If

            g_mFormDebugger.Timer_ConnectionCheck.Start()

            g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Debugger running! (Reload the map or enter 'sm plugins refresh' into the console to load all new plugins)"
            g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Green
            g_mFormDebugger.ToolStripStatusLabel_NoConnection.Visible = False

            'Make sure other async threads doenst fuckup everthing
            SyncLock g_mFileSystemWatcherLock
                g_mActiveBreakpointValue.sGUID = ""
                m_DebuggingState = ENUM_DEBUGGING_STATE.STARTED
                m_SuspendGame = False
            End SyncLock

            g_mFormDebugger.g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnDebuggerDebugStart())
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)

            StopDebugging(True)

            g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Error! " & ex.Message
            g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Red
            g_mFormDebugger.ToolStripStatusLabel_NoConnection.Visible = False

            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Stops the debugger.
    ''' Remove all elements for the debugging here.
    ''' </summary>
    Public Function StopDebugging(Optional bForceStop As Boolean = False) As Boolean
        Try
            If (Not bForceStop) Then
                If (m_DebuggingState = ENUM_DEBUGGING_STATE.STOPPED) Then
                    Return True
                End If

                If (Not g_bRememberAction) Then
                    Using i As New FormDebuggerStop
                        If (i.ShowDialog(g_mFormDebugger) = DialogResult.OK) Then
                            g_mRememberAction = i.m_DialogResult
                            g_bRememberAction = i.m_RememberAction
                        Else
                            Return False
                        End If
                    End Using
                End If

                Select Case (g_mRememberAction)
                    Case FormDebuggerStop.ENUM_DIALOG_RESULT.DO_NOTHING
                                'Do nothing? Yea lets do nothing here...

                    Case FormDebuggerStop.ENUM_DIALOG_RESULT.TERMINATE_GAME
                        If (Not String.IsNullOrEmpty(m_ClientFolder) AndAlso IO.Directory.Exists(m_ClientFolder)) Then
                            Dim sClientParentFolder As String = IO.Directory.GetParent(m_ClientFolder).FullName

                            For Each proc As Process In Process.GetProcesses
                                Try
                                    If (proc.Id = Process.GetCurrentProcess.Id) Then
                                        Continue For
                                    End If

                                    Dim sFullPath As String = IO.Path.GetFullPath(proc.MainModule.FileName)
                                    If (sFullPath.ToLower = Application.ExecutablePath.ToLower) Then
                                        Continue For
                                    End If

                                    If (sFullPath.ToLower.StartsWith(sClientParentFolder.ToLower)) Then
                                        proc.Kill()
                                    End If
                                Catch ex As Exception
                                    'Ignore access denied
                                End Try
                            Next
                        End If

                        If (Not String.IsNullOrEmpty(m_ServerFolder) AndAlso IO.Directory.Exists(m_ServerFolder)) Then
                            Dim sServerParentFolder As String = IO.Directory.GetParent(m_ServerFolder).FullName

                            For Each proc As Process In Process.GetProcesses
                                Try
                                    If (proc.Id = Process.GetCurrentProcess.Id) Then
                                        Continue For
                                    End If

                                    Dim sFullPath As String = IO.Path.GetFullPath(proc.MainModule.FileName)
                                    If (sFullPath.ToLower = Application.ExecutablePath.ToLower) Then
                                        Continue For
                                    End If

                                    If (sFullPath.ToLower.StartsWith(sServerParentFolder.ToLower)) Then
                                        proc.Kill()
                                    End If
                                Catch ex As Exception
                                    'Ignore access denied
                                End Try
                            Next
                        End If

                    Case FormDebuggerStop.ENUM_DIALOG_RESULT.RELOAD_MAP
                        If (Not String.IsNullOrEmpty(m_ServerFolder) AndAlso IO.Directory.Exists(m_ServerFolder)) Then
                            g_mFormDebugger.g_ClassDebuggerRunnerEngine.AcceptCommand(m_ServerFolder, "@reloadmap")
                        Else
                            MessageBox.Show("Could not send command! Game directory does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If

                    Case FormDebuggerStop.ENUM_DIALOG_RESULT.REFRESH_PLUGINS
                        If (Not String.IsNullOrEmpty(m_ServerFolder) AndAlso IO.Directory.Exists(m_ServerFolder)) Then
                            g_mFormDebugger.g_ClassDebuggerRunnerEngine.AcceptCommand(m_ServerFolder, "@refreshplugins")
                        Else
                            MessageBox.Show("Could not send command! Game directory does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If

                    Case FormDebuggerStop.ENUM_DIALOG_RESULT.RESTART_GAME
                        If (Not String.IsNullOrEmpty(m_ServerFolder) AndAlso IO.Directory.Exists(m_ServerFolder)) Then
                            g_mFormDebugger.g_ClassDebuggerRunnerEngine.AcceptCommand(m_ServerFolder, "_restart")
                        Else
                            MessageBox.Show("Could not send command! Game directory does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If

                    Case FormDebuggerStop.ENUM_DIALOG_RESULT.UNLOAD_PLUGIN
                        If (Not String.IsNullOrEmpty(m_ServerFolder) AndAlso IO.Directory.Exists(m_ServerFolder)) Then
                            With New Text.StringBuilder
                                .AppendLine(String.Format("sm plugins unload {0}", IO.Path.GetFileName(g_sLatestDebuggerPlugin)))
                                .AppendLine(String.Format("sm plugins unload {0}", IO.Path.GetFileName(g_sLatestDebuggerRunnerPlugin)))

                                g_mFormDebugger.g_ClassDebuggerRunnerEngine.AcceptCommand(m_ServerFolder, .ToString)
                            End With
                        Else
                            MessageBox.Show("Could not send command! Game directory does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                End Select
            End If

            'Remove I/O events
            RemoveFileSystemWatcher()

            'Remove entities updater thread
            ClassThread.Abort(g_mListViewEntitiesUpdaterThread)

            'Remove plugin
            If (Not String.IsNullOrEmpty(g_sLatestDebuggerPlugin) AndAlso IO.File.Exists(g_sLatestDebuggerPlugin)) Then
                IO.File.Delete(g_sLatestDebuggerPlugin)
                'ElseIf (Not String.IsNullOrEmpty(g_sLatestDebuggerPlugin)) Then
                '    MessageBox.Show(String.Format("Could not find '{0}'. Please remove it manualy!", g_sLatestDebuggerPlugin))
            End If

            'Remove cmd runner plugin
            If (Not String.IsNullOrEmpty(g_sLatestDebuggerRunnerPlugin) AndAlso IO.File.Exists(g_sLatestDebuggerRunnerPlugin)) Then
                IO.File.Delete(g_sLatestDebuggerRunnerPlugin)
                'ElseIf (Not String.IsNullOrEmpty(g_sLatestDebuggerRunnerPlugin)) Then
                '    MessageBox.Show(String.Format("Could not find '{0}'. Please remove it manualy!", g_sLatestDebuggerRunnerPlugin))
            End If

            g_sLatestDebuggerPlugin = ""
            g_sLatestDebuggerRunnerPlugin = ""

            'Reset everything
            'Make sure other async threads doenst fuckup everthing
            SyncLock g_mFileSystemWatcherLock
                g_mActiveBreakpointValue.sGUID = ""
                m_DebuggingState = ENUM_DEBUGGING_STATE.STOPPED
                m_SuspendGame = False
            End SyncLock

            UpdateBreakpointListView()

            g_mFormDebugger.Timer_ConnectionCheck.Stop()

            g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Debugger stopped!"
            g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Red
            g_mFormDebugger.ToolStripStatusLabel_NoConnection.Visible = False

            g_mFormDebugger.g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnDebuggerDebugStop())
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)

            g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Error! " & ex.Message
            g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Red
            g_mFormDebugger.ToolStripStatusLabel_NoConnection.Visible = False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Cintinues debugging if paused.
    ''' </summary>
    Public Sub ContinueDebugging()
        Try
            If (m_DebuggingState <> ENUM_DEBUGGING_STATE.PAUSED) Then
                Return
            End If

            If (Not String.IsNullOrEmpty(g_mActiveBreakpointValue.sGUID)) Then
                Dim sServerFolder As String = m_ServerFolder
                Dim sContinueFile As String = IO.Path.Combine(sServerFolder, g_mActiveBreakpointValue.sGUID & ClassDebuggerParser.g_sDebuggerBreakpointContinueExt.ToLower)
                Dim sContinueVarFile As String = IO.Path.Combine(sServerFolder, g_mActiveBreakpointValue.sGUID & ClassDebuggerParser.g_sDebuggerBreakpointContinueVarExt.ToLower)

                If (g_mActiveBreakpointValue.bReturnCustomValue) Then
                    Select Case (g_mActiveBreakpointValue.mValueType)
                        Case ENUM_BREAKPOINT_VALUE_TYPE.INTEGER
                            IO.File.WriteAllText(sContinueVarFile, "i:" & g_mActiveBreakpointValue.sIntegerValue)
                        Case Else
                            IO.File.WriteAllText(sContinueVarFile, "f:" & g_mActiveBreakpointValue.sFloatValue.Replace(",", "."))
                    End Select
                Else
                    IO.File.WriteAllText(sContinueFile, "")
                End If
            End If

            'Close any forms
            If (g_mFormDebuggerException IsNot Nothing AndAlso Not g_mFormDebuggerException.IsDisposed) Then
                g_mFormDebuggerException.Dispose()
                g_mFormDebuggerException = Nothing
            End If

            If (g_mFormDebuggerCriticalPopupException IsNot Nothing AndAlso Not g_mFormDebuggerCriticalPopupException.IsDisposed) Then
                g_mFormDebuggerCriticalPopupException.Dispose()
                g_mFormDebuggerCriticalPopupException = Nothing
            End If

            If (g_mFormDebuggerCriticalPopupFatalException IsNot Nothing AndAlso Not g_mFormDebuggerCriticalPopupFatalException.IsDisposed) Then
                g_mFormDebuggerCriticalPopupFatalException.Dispose()
                g_mFormDebuggerCriticalPopupFatalException = Nothing
            End If

            'Make sure other async threads doenst fuckup everthing
            SyncLock g_mFileSystemWatcherLock
                g_mActiveBreakpointValue.sGUID = ""

                m_DebuggingState = ENUM_DEBUGGING_STATE.STARTED
                m_SuspendGame = False
            End SyncLock

            UpdateBreakpointListView()

            g_mFormDebugger.Timer_ConnectionCheck.Start()

            g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Debugger running!"
            g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Green
            g_mFormDebugger.ToolStripStatusLabel_NoConnection.Visible = False

            g_mFormDebugger.g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnDebuggerDebugStart())
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)

            g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Error! " & ex.Message
            g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Red
            g_mFormDebugger.ToolStripStatusLabel_NoConnection.Visible = False
        End Try
    End Sub

    ''' <summary>
    ''' Pauses debugging.
    ''' Only suspends the game process.
    ''' </summary>
    Public Sub PauseDebugging()
        Try
            If (m_DebuggingState <> ENUM_DEBUGGING_STATE.STARTED) Then
                Return
            End If

            'Make sure other async threads doenst fuckup everthing
            SyncLock g_mFileSystemWatcherLock
                m_SuspendGame = True
                m_DebuggingState = ENUM_DEBUGGING_STATE.PAUSED
            End SyncLock

            UpdateBreakpointListView()

            g_mFormDebugger.Timer_ConnectionCheck.Stop()

            g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Debugger awaiting input..."
            g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Orange
            g_mFormDebugger.ToolStripStatusLabel_NoConnection.Visible = False

            g_mFormDebugger.g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnDebuggerDebugPause())
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)

            g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Error! " & ex.Message
            g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Red
            g_mFormDebugger.ToolStripStatusLabel_NoConnection.Visible = False
        End Try
    End Sub

    ''' <summary>
    ''' Those FileSystemWatchers are required to receive messages from SourceMod and plugins.
    ''' </summary>
    ''' <param name="sGameDir"></param>
    Private Sub CreateFileSystemWatcher(sGameDir As String)
        RemoveFileSystemWatcher()

        For i As ENUM_FILESYSTEMWATCHER_TYPES = 0 To CType(g_mFileSystemWatcherArray.Length - 1, ENUM_FILESYSTEMWATCHER_TYPES)
            g_mFileSystemWatcherArray(i) = New IO.FileSystemWatcher
            g_mFileSystemWatcherArray(i).BeginInit()
            g_mFileSystemWatcherArray(i).Path = sGameDir
            g_mFileSystemWatcherArray(i).IncludeSubdirectories = True
            g_mFileSystemWatcherArray(i).NotifyFilter = IO.NotifyFilters.Size Or IO.NotifyFilters.FileName Or IO.NotifyFilters.CreationTime
            g_mFileSystemWatcherArray(i).Filter = ""
            g_mFileSystemWatcherArray(i).EnableRaisingEvents = True
            g_mFileSystemWatcherArray(i).EndInit()

            Select Case (i)
                Case ENUM_FILESYSTEMWATCHER_TYPES.BREAKPOINTS
                    AddHandler g_mFileSystemWatcherArray(i).Created, AddressOf FSW_OnBreakpointDetected
                    AddHandler g_mFileSystemWatcherArray(i).Changed, AddressOf FSW_OnBreakpointDetected
                    AddHandler g_mFileSystemWatcherArray(i).Renamed, AddressOf FSW_OnBreakpointDetected

                Case ENUM_FILESYSTEMWATCHER_TYPES.WATCHERS
                    AddHandler g_mFileSystemWatcherArray(i).Created, AddressOf FSW_OnWatcherDetected
                    AddHandler g_mFileSystemWatcherArray(i).Changed, AddressOf FSW_OnWatcherDetected
                    AddHandler g_mFileSystemWatcherArray(i).Renamed, AddressOf FSW_OnWatcherDetected

                Case ENUM_FILESYSTEMWATCHER_TYPES.ENTITIES
                    AddHandler g_mFileSystemWatcherArray(i).Created, AddressOf FSW_OnEntitiesFetch
                    AddHandler g_mFileSystemWatcherArray(i).Changed, AddressOf FSW_OnEntitiesFetch
                    AddHandler g_mFileSystemWatcherArray(i).Renamed, AddressOf FSW_OnEntitiesFetch

                Case ENUM_FILESYSTEMWATCHER_TYPES.EXCEPTIONS
                    AddHandler g_mFileSystemWatcherArray(i).Created, AddressOf FSW_OnSourceModException
                    AddHandler g_mFileSystemWatcherArray(i).Changed, AddressOf FSW_OnSourceModException
                    AddHandler g_mFileSystemWatcherArray(i).Renamed, AddressOf FSW_OnSourceModException

                Case ENUM_FILESYSTEMWATCHER_TYPES.FATAL_EXCEPTIONS
                    AddHandler g_mFileSystemWatcherArray(i).Created, AddressOf FSW_OnSourceModFatalException
                    AddHandler g_mFileSystemWatcherArray(i).Changed, AddressOf FSW_OnSourceModFatalException
                    AddHandler g_mFileSystemWatcherArray(i).Renamed, AddressOf FSW_OnSourceModFatalException

                Case ENUM_FILESYSTEMWATCHER_TYPES.PING
                    AddHandler g_mFileSystemWatcherArray(i).Created, AddressOf FSW_OnPingFetch
                    AddHandler g_mFileSystemWatcherArray(i).Changed, AddressOf FSW_OnPingFetch
                    AddHandler g_mFileSystemWatcherArray(i).Renamed, AddressOf FSW_OnPingFetch

                Case Else
                    Throw New ArgumentException("Invalid FileSystemWatcher")
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Remove all FileSystemWatchers so we dont receive any messages from SourceMod and plugins.
    ''' </summary>
    Private Sub RemoveFileSystemWatcher()
        For i As ENUM_FILESYSTEMWATCHER_TYPES = 0 To CType(g_mFileSystemWatcherArray.Length - 1, ENUM_FILESYSTEMWATCHER_TYPES)
            If (g_mFileSystemWatcherArray(i) Is Nothing) Then
                Continue For
            End If

            RemoveHandler g_mFileSystemWatcherArray(i).Created, AddressOf FSW_OnBreakpointDetected
            RemoveHandler g_mFileSystemWatcherArray(i).Changed, AddressOf FSW_OnBreakpointDetected
            RemoveHandler g_mFileSystemWatcherArray(i).Renamed, AddressOf FSW_OnBreakpointDetected

            RemoveHandler g_mFileSystemWatcherArray(i).Created, AddressOf FSW_OnWatcherDetected
            RemoveHandler g_mFileSystemWatcherArray(i).Changed, AddressOf FSW_OnWatcherDetected
            RemoveHandler g_mFileSystemWatcherArray(i).Renamed, AddressOf FSW_OnWatcherDetected

            RemoveHandler g_mFileSystemWatcherArray(i).Created, AddressOf FSW_OnEntitiesFetch
            RemoveHandler g_mFileSystemWatcherArray(i).Changed, AddressOf FSW_OnEntitiesFetch
            RemoveHandler g_mFileSystemWatcherArray(i).Renamed, AddressOf FSW_OnEntitiesFetch

            RemoveHandler g_mFileSystemWatcherArray(i).Created, AddressOf FSW_OnSourceModException
            RemoveHandler g_mFileSystemWatcherArray(i).Changed, AddressOf FSW_OnSourceModException
            RemoveHandler g_mFileSystemWatcherArray(i).Renamed, AddressOf FSW_OnSourceModException

            RemoveHandler g_mFileSystemWatcherArray(i).Created, AddressOf FSW_OnSourceModFatalException
            RemoveHandler g_mFileSystemWatcherArray(i).Changed, AddressOf FSW_OnSourceModFatalException
            RemoveHandler g_mFileSystemWatcherArray(i).Renamed, AddressOf FSW_OnSourceModFatalException

            RemoveHandler g_mFileSystemWatcherArray(i).Created, AddressOf FSW_OnPingFetch
            RemoveHandler g_mFileSystemWatcherArray(i).Changed, AddressOf FSW_OnPingFetch
            RemoveHandler g_mFileSystemWatcherArray(i).Renamed, AddressOf FSW_OnPingFetch

            g_mFileSystemWatcherArray(i).Dispose()
            g_mFileSystemWatcherArray(i) = Nothing
        Next
    End Sub

    ''' <summary>
    ''' Handle received breakpoints from plugins.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FSW_OnBreakpointDetected(sender As Object, e As IO.FileSystemEventArgs)
        Try
            Dim sFile As String = e.FullPath
            If (Not IO.File.Exists(sFile)) Then
                Return
            End If

            Dim sFileExt As String = IO.Path.GetExtension(sFile)
            If (sFileExt.ToLower <> ClassDebuggerParser.g_sDebuggerFilesExt.ToLower OrElse Not sFile.ToLower.EndsWith(ClassDebuggerParser.g_sDebuggerBreakpointTriggerExt.ToLower)) Then
                Return
            End If


            Dim sGUID As String = IO.Path.GetFileName(sFile).ToLower.Replace(ClassDebuggerParser.g_sDebuggerBreakpointTriggerExt.ToLower, "")
            If (String.IsNullOrEmpty(sGUID) OrElse sGUID.Trim.Length = 0) Then
                Return
            End If

            If (Not g_mFormDebugger.g_ClassDebuggerParser.g_lBreakpointList.Exists(Function(i As ClassDebuggerParser.STRUC_DEBUGGER_ITEM) i.sGUID = sGUID)) Then
                Return
            End If

            Dim sLines As String() = New String() {}

            Dim mStopWatch As New Stopwatch
            mStopWatch.Start()
            While True
                Try
                    If (mStopWatch.ElapsedMilliseconds > 2500) Then
                        mStopWatch.Stop()
                        Return
                    End If

                    sLines = IO.File.ReadAllLines(sFile) 'Tools.StringReadLinesEnd(sFile, 3) 'IO.File.ReadAllLines(sFile)

                    Exit While
                Catch ex As Threading.ThreadAbortException
                    Throw
                Catch ex As Exception
                End Try
            End While
            mStopWatch.Stop()

            Try
                IO.File.Delete(sFile)
            Catch ex As Threading.ThreadAbortException
                Throw
            Catch ex As Exception
            End Try

            Dim sInteger As String
            Dim sFloat As String
            If (sLines.Length < 2) Then
                sInteger = "-1"
                sFloat = "-1.0"
            Else
                sInteger = sLines(0).Remove(0, "i:".Length)
                sFloat = sLines(1).Remove(0, "f:".Length)
            End If

            'Make sure other async threads doenst fuckup everthing
            SyncLock g_mFileSystemWatcherLock
                m_SuspendGame = True
                m_DebuggingState = ENUM_DEBUGGING_STATE.PAUSED

                g_mActiveBreakpointValue.sGUID = sGUID
                g_mActiveBreakpointValue.bReturnCustomValue = False
                g_mActiveBreakpointValue.mValueType = ENUM_BREAKPOINT_VALUE_TYPE.INTEGER
                g_mActiveBreakpointValue.sIntegerValue = sInteger
                g_mActiveBreakpointValue.sFloatValue = sFloat
                g_mActiveBreakpointValue.sOrginalIntegerValue = sInteger
                g_mActiveBreakpointValue.sOrginalFloatValue = sFloat


                'INFO: Dont use 'Invoke' it deadlocks on FileSystemWatcher.Dispose, use async 'BeginInvoke' instead.
                ClassThread.ExecAsync(g_mFormDebugger, Sub()
                                                           If (m_DebuggingState <> ENUM_DEBUGGING_STATE.PAUSED) Then
                                                               Return
                                                           End If

                                                           UpdateBreakpointListView()

                                                           g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Debugger awaiting input..."
                                                           g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Orange
                                                           g_mFormDebugger.ToolStripStatusLabel_NoConnection.Visible = False

                                                           If (g_mFormDebugger.WindowState = FormWindowState.Minimized) Then
                                                               ClassTools.ClassForms.FormWindowCommand(g_mFormDebugger, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                                                           End If
                                                           g_mFormDebugger.Activate()

                                                           g_mFormDebugger.g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnDebuggerDebugPause())
                                                       End Sub)
            End SyncLock
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As ObjectDisposedException
            'Filter unexpected disposes
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Handle received Watchers values from plugins.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FSW_OnWatcherDetected(sender As Object, e As IO.FileSystemEventArgs)
        Try
            Dim sFile As String = e.FullPath
            If (Not IO.File.Exists(sFile)) Then
                Return
            End If

            Dim sFileExt As String = IO.Path.GetExtension(sFile)
            If (sFileExt.ToLower <> ClassDebuggerParser.g_sDebuggerFilesExt.ToLower OrElse Not sFile.ToLower.EndsWith(ClassDebuggerParser.g_sDebuggerWatcherValueExt.ToLower)) Then
                Return
            End If


            Dim sGUID As String = IO.Path.GetFileName(sFile).ToLower.Replace(ClassDebuggerParser.g_sDebuggerWatcherValueExt.ToLower, "")
            If (String.IsNullOrEmpty(sGUID) OrElse sGUID.Trim.Length = 0) Then
                Return
            End If

            If (Not g_mFormDebugger.g_ClassDebuggerParser.g_lWatcherList.Exists(Function(i As ClassDebuggerParser.STRUC_DEBUGGER_ITEM) i.sGUID = sGUID)) Then
                Return
            End If

            Dim sLines As String() = New String() {}

            Dim mStopWatch As New Stopwatch
            mStopWatch.Start()
            While True
                Try
                    If (mStopWatch.ElapsedMilliseconds > 2500) Then
                        mStopWatch.Stop()
                        Return
                    End If

                    sLines = IO.File.ReadAllLines(sFile) 'Tools.StringReadLinesEnd(sFile, 3) 'IO.File.ReadAllLines(sFile)

                    Exit While
                Catch ex As Threading.ThreadAbortException
                    Throw
                Catch ex As Exception
                End Try
            End While
            mStopWatch.Stop()

            Try
                IO.File.Delete(sFile)
            Catch ex As Threading.ThreadAbortException
                Throw
            Catch ex As Exception
            End Try

            Dim sInteger As String
            Dim sFloat As String
            Dim sCount As String
            If (sLines.Length < 3) Then
                sInteger = "-1"
                sFloat = "-1.0"
                sCount = "-1"
            Else
                sInteger = sLines(0).Remove(0, "i:".Length)
                sFloat = sLines(1).Remove(0, "f:".Length)
                sCount = sLines(2).Remove(0, "c:".Length)
            End If

            'INFO: Dont use 'Invoke' it deadlocks on FileSystemWatcher.Dispose, use async 'BeginInvoke' instead.
            ClassThread.ExecAsync(g_mFormDebugger.ListView_Watchers, Sub()
                                                                         For i = 0 To g_mFormDebugger.ListView_Watchers.Items.Count - 1
                                                                             Dim mListViewItemData = TryCast(g_mFormDebugger.ListView_Watchers.Items(i), ClassListViewItemData)
                                                                             If (mListViewItemData Is Nothing) Then
                                                                                 Continue For
                                                                             End If

                                                                             If (CStr(mListViewItemData.g_mData("GUID")) = sGUID) Then
                                                                                 mListViewItemData.SubItems(2).Text = String.Format("i:{0} | f:{1}", sInteger, sFloat)
                                                                                 mListViewItemData.SubItems(3).Text = sCount
                                                                             End If
                                                                         Next
                                                                     End Sub)
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As ObjectDisposedException
            'Filter unexpected disposes
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Enum ENUM_ENTITY_ACTION
        UPDATE
        REMOVE
    End Enum

    ''' <summary>
    ''' Handle fetched entities from plugins.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FSW_OnEntitiesFetch(sender As Object, e As IO.FileSystemEventArgs)
        Try
            Dim sFile As String = e.FullPath
            If (Not IO.File.Exists(sFile)) Then
                Return
            End If

            Dim sFileExt As String = IO.Path.GetExtension(sFile)
            If (sFileExt.ToLower <> ClassDebuggerParser.g_sDebuggerFilesExt.ToLower OrElse Not sFile.ToLower.EndsWith(ClassDebuggerParser.ClassRunnerEngine.g_sDebuggerRunnerEntityFileExt.ToLower)) Then
                Return
            End If


            Dim sGUID As String = IO.Path.GetFileName(sFile).ToLower.Replace(ClassDebuggerParser.ClassRunnerEngine.g_sDebuggerRunnerEntityFileExt.ToLower, "")
            If (String.IsNullOrEmpty(sGUID) OrElse sGUID.Trim.Length = 0) Then
                Return
            End If

            If (g_mFormDebugger.g_ClassDebuggerRunnerEngine.g_sDebuggerRunnerGuid <> sGUID) Then
                Return
            End If

            Dim sLines As String() = New String() {}

            Dim mStopWatch As New Stopwatch
            mStopWatch.Start()
            While True
                Try
                    If (mStopWatch.ElapsedMilliseconds > 2500) Then
                        mStopWatch.Stop()
                        Return
                    End If

                    sLines = IO.File.ReadAllLines(sFile) 'Tools.StringReadLinesEnd(sFile, 3) 'IO.File.ReadAllLines(sFile)

                    Exit While
                Catch ex As Threading.ThreadAbortException
                    Throw
                Catch ex As Exception
                End Try
            End While
            mStopWatch.Stop()

            Try
                IO.File.Delete(sFile)
            Catch ex As Threading.ThreadAbortException
                Throw
            Catch ex As Exception
            End Try

            For i = 0 To sLines.Length - 1
                Dim mMatch As Match = Regex.Match(sLines(i), "^(?<Index>[0-9]+)\:(?<EntRef>[0-9]+|\-[0-9]+)\:(?<Action>[0-9]+)\:(?<Classname>.*?)$")
                If (Not mMatch.Success) Then
                    Continue For
                End If

                Dim iIndex As Integer = CInt(mMatch.Groups("Index").Value)
                Dim iEntRef As Integer = CInt(mMatch.Groups("EntRef").Value)
                Dim iAction As ENUM_ENTITY_ACTION = CType(mMatch.Groups("Action").Value, ENUM_ENTITY_ACTION)
                Dim sClassname As String = mMatch.Groups("Classname").Value
                Dim iDateTicks As Long = Date.Now.Ticks

                If (iIndex < 0 OrElse iIndex >= 2048) Then
                    Continue For
                End If

                Select Case (iAction)
                    Case ENUM_ENTITY_ACTION.UPDATE
                        ClassThread.ExecAsync(g_mFormDebugger.ListView_Entities, Sub()
                                                                                     Try
                                                                                         Dim mListViewItemData = TryCast(g_mFormDebugger.ListView_Entities.Items(iIndex), ClassListViewItemData)
                                                                                         If (mListViewItemData Is Nothing) Then
                                                                                             Return
                                                                                         End If

                                                                                         Dim sOldEntRef As String = CStr(mListViewItemData.g_mData("EntityRef"))
                                                                                         Dim bIsNewEnt As Boolean = True
                                                                                         If (Not String.IsNullOrEmpty(sOldEntRef)) Then
                                                                                             bIsNewEnt = (CInt(sOldEntRef) <> iEntRef)
                                                                                         End If

                                                                                         If (bIsNewEnt) Then
                                                                                             mListViewItemData.SubItems(1).Text = iEntRef.ToString
                                                                                             mListViewItemData.SubItems(2).Text = sClassname

                                                                                             mListViewItemData.g_mData("EntityRef") = iEntRef
                                                                                             mListViewItemData.g_mData("Classname") = sClassname
                                                                                             mListViewItemData.g_mData("Ticks") = iDateTicks

                                                                                             If (ClassSettings.g_iSettingsDebuggerEntitiesEnableColoring) Then
                                                                                                 mListViewItemData.BackColor = Color.Green
                                                                                             End If

                                                                                             If (ClassSettings.g_iSettingsDebuggerEntitiesEnableAutoScroll) Then
                                                                                                 mListViewItemData.Selected = True
                                                                                                 mListViewItemData.EnsureVisible()
                                                                                             End If
                                                                                         End If
                                                                                     Catch ex As Exception
                                                                                         'Ignore random minor errors
                                                                                     End Try
                                                                                 End Sub)
                    Case ENUM_ENTITY_ACTION.REMOVE
                        ClassThread.ExecAsync(g_mFormDebugger.ListView_Entities, Sub()
                                                                                     Try
                                                                                         Dim mListViewItemData = TryCast(g_mFormDebugger.ListView_Entities.Items(iIndex), ClassListViewItemData)
                                                                                         If (mListViewItemData Is Nothing) Then
                                                                                             Return
                                                                                         End If

                                                                                         Dim sOldEntRef As String = CStr(mListViewItemData.g_mData("EntityRef"))
                                                                                         Dim bIsNewEnt As Boolean = True
                                                                                         If (Not String.IsNullOrEmpty(sOldEntRef)) Then
                                                                                             bIsNewEnt = (CInt(sOldEntRef) <> iEntRef)
                                                                                         End If

                                                                                         If (bIsNewEnt) Then
                                                                                             mListViewItemData.SubItems(1).Text = "-1"
                                                                                             mListViewItemData.SubItems(2).Text = "-"

                                                                                             mListViewItemData.g_mData("EntityRef") = -1
                                                                                             mListViewItemData.g_mData("Classname") = "-"
                                                                                             mListViewItemData.g_mData("Ticks") = iDateTicks

                                                                                             If (ClassSettings.g_iSettingsDebuggerEntitiesEnableColoring) Then
                                                                                                 mListViewItemData.BackColor = Color.Red
                                                                                             End If
                                                                                         End If
                                                                                     Catch ex As Exception
                                                                                         'Ignore random minor errors
                                                                                     End Try
                                                                                 End Sub)
                End Select
            Next
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As ObjectDisposedException
            'Filter unexpected disposes
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Handle fetched pings from plugins.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FSW_OnPingFetch(sender As Object, e As IO.FileSystemEventArgs)
        Try
            Dim sFile As String = e.FullPath
            If (Not IO.File.Exists(sFile)) Then
                Return
            End If

            Dim sFileExt As String = IO.Path.GetExtension(sFile)
            If (sFileExt.ToLower <> ClassDebuggerParser.g_sDebuggerFilesExt.ToLower OrElse Not sFile.ToLower.EndsWith(ClassDebuggerParser.ClassRunnerEngine.g_sDebuggerRunnerPingExt.ToLower)) Then
                Return
            End If


            Dim sGUID As String = IO.Path.GetFileName(sFile).ToLower.Replace(ClassDebuggerParser.ClassRunnerEngine.g_sDebuggerRunnerPingExt.ToLower, "")
            If (String.IsNullOrEmpty(sGUID) OrElse sGUID.Trim.Length = 0) Then
                Return
            End If

            If (g_mFormDebugger.g_ClassDebuggerRunnerEngine.g_sDebuggerRunnerGuid <> sGUID) Then
                Return
            End If

            ClassThread.ExecAsync(g_mFormDebugger, Sub()
                                                       g_mFormDebugger.ToolStripStatusLabel_NoConnection.Visible = False

                                                       g_mFormDebugger.Timer_ConnectionCheck.Stop()
                                                       g_mFormDebugger.Timer_ConnectionCheck.Start()
                                                   End Sub)
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As ObjectDisposedException
            'Filter unexpected disposes
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Clear all entities ListView items coloring when times up.
    ''' Nessecary for the 'Process Explorer' like coloring.
    ''' </summary>
    Private Sub ListViewEntitiesUpdaterThread()
        Try
            While True
                Threading.Thread.Sleep(g_iListViewEntitesUpdaterTime)

                ClassThread.ExecAsync(g_mFormDebugger.ListView_Entities, Sub()
                                                                             For i = 0 To g_mFormDebugger.ListView_Entities.Items.Count - 1
                                                                                 Dim mListViewItemData = TryCast(g_mFormDebugger.ListView_Entities.Items(i), ClassListViewItemData)
                                                                                 If (mListViewItemData Is Nothing) Then
                                                                                     Return
                                                                                 End If

                                                                                 Dim mTicks As Object = mListViewItemData.g_mData("Ticks")
                                                                                 If (mTicks Is Nothing) Then
                                                                                     Continue For
                                                                                 End If

                                                                                 Dim mDate As New Date(CLng(mTicks))

                                                                                 If ((mDate + New TimeSpan(0, 0, 0, 0, g_iListViewEntitesUpdaterTime)) < Date.Now) Then
                                                                                     If (ClassControlStyle.m_IsInvertedColors) Then
                                                                                         mListViewItemData.BackColor = ClassControlStyle.g_cDarkControlColor.mDarkBackground
                                                                                     Else
                                                                                         mListViewItemData.BackColor = ClassControlStyle.g_cDarkControlColor.mLightBackground
                                                                                     End If

                                                                                     mListViewItemData.g_mData("Ticks") = Nothing
                                                                                 End If
                                                                             Next
                                                                         End Sub)
            End While
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Handle received SourceMod exceptions.
    ''' NOTE: There are 2 main parts here, filtered exceptions and non-filtered exceptions (unknown exceptions)
    '''       It will filter the exceptions first, to get all possible information (date, Line, File).
    '''       If that failes, then use them as unknown exceptions, which just shows the latest log entries.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FSW_OnSourceModException(sender As Object, e As IO.FileSystemEventArgs)
        Try
            Dim sFile As String = e.FullPath

            If (Not IO.File.Exists(sFile)) Then
                Return
            End If

            Dim sFileExt As String = IO.Path.GetExtension(sFile)
            Dim sFileFullName As String = IO.Path.GetFileName(sFile)
            Dim dDate As Date = Now

            If (Not ClassSettings.g_iSettingsDebuggerCatchExceptions OrElse sFileExt.ToLower <> ".log" OrElse Not sFileFullName.ToLower.StartsWith("errors_")) Then
                Return
            End If


            Dim sLines As String() = New String() {}

            'Make sure other async threads doenst fuckup everthing
            SyncLock g_mFileSystemWatcherLock
                Dim bWasSuspended As Boolean = m_SuspendGame

                Dim mStopWatch As New Stopwatch
                mStopWatch.Start()
                While True
                    Try
                        If (mStopWatch.ElapsedMilliseconds > 2500) Then
                            mStopWatch.Stop()
                            Return
                        End If

                        'TODO: Add better techneque, this has a high false positive rate. (Reads while SourceMod writes to the file)
                        'Check if SourceMod wrote to the file.
                        Dim fileChangeTime As Date = IO.File.GetLastWriteTime(sFile)
                        If (mStopWatch.ElapsedMilliseconds < 500 AndAlso (fileChangeTime + New TimeSpan(0, 0, 0, 0, 100)) > Date.Now) Then
                            Continue While
                        End If

                        'Make sure we suspend the game process first, otherwise we risk that SourceMod disables its logging because we used the file first
                        m_SuspendGame = True
                        sLines = ClassTools.ClassStrings.StringReadLinesEnd(sFile, SM_LOG_READ_LINES_MAX)
                        m_SuspendGame = bWasSuspended

                        Exit While
                    Catch ex As Threading.ThreadAbortException
                        Throw
                    Catch ex As Exception
                    End Try
                End While
                mStopWatch.Stop()
            End SyncLock


            If (sLines.Length > 0) Then
                Dim bFoundKnownExceptions As Boolean = False

                Dim sSMXFileName As String = IO.Path.GetFileName(g_sLatestDebuggerPlugin)

                Dim smExceptions = g_mFormDebugger.g_ClassDebuggerParser.ReadSourceModLogExceptions(sLines)
                Dim i As Integer
                For i = smExceptions.Length - 1 To 0 Step -1
                    Dim sBlameFile As String = smExceptions(i).sBlamingFile

                    If (smExceptions(i).dLogDate + New TimeSpan(0, 0, 0, 1) < dDate) Then
                        Continue For
                    End If

                    If (Not sBlameFile.ToLower.StartsWith(sSMXFileName.ToLower)) Then
                        Continue For
                    End If

                    If (g_mFormDebuggerException IsNot Nothing AndAlso Not g_mFormDebuggerException.IsDisposed) Then
                        Continue For
                    End If

                    'Make sure other async threads doenst fuckup everthing
                    SyncLock g_mFileSystemWatcherLock
                        bFoundKnownExceptions = True

                        m_SuspendGame = True
                        m_DebuggingState = ENUM_DEBUGGING_STATE.PAUSED

                        'INFO: Dont use 'Invoke' it deadlocks on FileSystemWatcher.Dispose, use async 'BeginInvoke' instead.
                        ClassThread.ExecAsync(g_mFormDebugger, Sub()
                                                                   If (g_mFormDebuggerException IsNot Nothing AndAlso Not g_mFormDebuggerException.IsDisposed) Then
                                                                       Return
                                                                   End If

                                                                   If (m_DebuggingState <> ENUM_DEBUGGING_STATE.PAUSED) Then
                                                                       Return
                                                                   End If

                                                                   UpdateBreakpointListView()

                                                                   g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Debugger awaiting input..."
                                                                   g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Orange


                                                                   If (g_mFormDebugger.WindowState = FormWindowState.Minimized) Then
                                                                       ClassTools.ClassForms.FormWindowCommand(g_mFormDebugger, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                                                                   End If
                                                                   g_mFormDebugger.Activate()

                                                                   g_mFormDebuggerException = New FormDebuggerException(g_mFormDebugger, sFile, smExceptions(i))
                                                                   g_mFormDebuggerException.Show(g_mFormDebugger)

                                                                   If (g_mFormDebuggerException.WindowState = FormWindowState.Minimized) Then
                                                                       ClassTools.ClassForms.FormWindowCommand(g_mFormDebuggerException, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                                                                   End If
                                                                   g_mFormDebuggerException.Activate()

                                                                   g_mFormDebugger.g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnDebuggerDebugPause())
                                                               End Sub)

                    End SyncLock

                    Exit For
                Next

                If (Not bFoundKnownExceptions) Then
                    If (g_mFormDebuggerException IsNot Nothing AndAlso Not g_mFormDebuggerException.IsDisposed) Then
                        Return
                    End If

                    If (g_mFormDebuggerCriticalPopupException IsNot Nothing AndAlso Not g_mFormDebuggerCriticalPopupException.IsDisposed) Then
                        Return
                    End If

                    'Make sure other async threads doenst fuckup everthing 
                    SyncLock g_mFileSystemWatcherLock
                        m_SuspendGame = True
                        m_DebuggingState = ENUM_DEBUGGING_STATE.PAUSED

                        'INFO: Dont use 'Invoke' it deadlocks on FileSystemWatcher.Dispose, use async 'BeginInvoke' instead.
                        ClassThread.ExecAsync(g_mFormDebugger, Sub()
                                                                   If (g_mFormDebuggerException IsNot Nothing AndAlso Not g_mFormDebuggerException.IsDisposed) Then
                                                                       Return
                                                                   End If

                                                                   If (g_mFormDebuggerCriticalPopupException IsNot Nothing AndAlso Not g_mFormDebuggerCriticalPopupException.IsDisposed) Then
                                                                       Return
                                                                   End If

                                                                   If (m_DebuggingState <> ENUM_DEBUGGING_STATE.PAUSED) Then
                                                                       Return
                                                                   End If

                                                                   UpdateBreakpointListView()

                                                                   g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Debugger awaiting input..."
                                                                   g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Orange

                                                                   If (g_mFormDebugger.WindowState = FormWindowState.Minimized) Then
                                                                       ClassTools.ClassForms.FormWindowCommand(g_mFormDebugger, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                                                                   End If
                                                                   g_mFormDebugger.Activate()

                                                                   g_mFormDebuggerCriticalPopupException = New FormDebuggerCriticalPopup(g_mFormDebugger, "Unknown SourceMod Exception", "The debugger caught unknown exceptions!", String.Join(Environment.NewLine, sLines))
                                                                   g_mFormDebuggerCriticalPopupException.Show(g_mFormDebugger)

                                                                   If (g_mFormDebuggerCriticalPopupException.WindowState = FormWindowState.Minimized) Then
                                                                       ClassTools.ClassForms.FormWindowCommand(g_mFormDebuggerCriticalPopupException, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                                                                   End If
                                                                   g_mFormDebuggerCriticalPopupException.Activate()

                                                                   g_mFormDebugger.g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnDebuggerDebugPause())
                                                               End Sub)
                    End SyncLock
                End If
            End If
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As ObjectDisposedException
            'Filter unexpected disposes
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Handle received SourceMod fatal exceptions.
    ''' Just show whole log, we dont care about filtering that, too much randomness.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub FSW_OnSourceModFatalException(sender As Object, e As IO.FileSystemEventArgs)
        Try
            Dim sFile As String = e.FullPath

            If (Not IO.File.Exists(sFile)) Then
                Return
            End If

            Dim sFileFullName As String = IO.Path.GetFileName(sFile)

            If (Not ClassSettings.g_iSettingsDebuggerCatchExceptions OrElse sFileFullName.ToLower <> "sourcemod_fatal.log") Then
                Return
            End If


            Dim sLines As String() = New String() {}


            'Make sure other async threads doenst fuckup everthing
            SyncLock g_mFileSystemWatcherLock
                Dim bWasSuspended As Boolean = m_SuspendGame

                Dim mStopWatch As New Stopwatch
                mStopWatch.Start()
                While True
                    Try
                        If (mStopWatch.ElapsedMilliseconds > 2500) Then
                            mStopWatch.Stop()
                            Return
                        End If

                        'TODO: Add better techneque, this has a high false positive rate. (Reads while SourceMod writes to the file)
                        'Check if SourceMod wrote to the file.
                        Dim fileChangeTime As Date = IO.File.GetLastWriteTime(sFile)
                        If (mStopWatch.ElapsedMilliseconds < 500 AndAlso (fileChangeTime + New TimeSpan(0, 0, 0, 0, 100)) > Date.Now) Then
                            Continue While
                        End If

                        'Make sure we suspend the game process first, otherwise we risk that SourceMod disables its logging because we used the file first
                        m_SuspendGame = True
                        sLines = ClassTools.ClassStrings.StringReadLinesEnd(sFile, SM_LOG_READ_LINES_MAX)
                        m_SuspendGame = bWasSuspended

                        Exit While
                    Catch ex As Threading.ThreadAbortException
                        Throw
                    Catch ex As Exception
                    End Try
                End While
                mStopWatch.Stop()
            End SyncLock


            If (sLines.Length > 0) Then
                If (g_mFormDebuggerCriticalPopupFatalException IsNot Nothing AndAlso Not g_mFormDebuggerCriticalPopupFatalException.IsDisposed) Then
                    Return
                End If

                'Make sure other async threads doenst fuckup everthing 
                SyncLock g_mFileSystemWatcherLock
                    m_SuspendGame = True
                    m_DebuggingState = ENUM_DEBUGGING_STATE.PAUSED

                    'INFO: Dont use 'Invoke' it deadlocks on FileSystemWatcher.Dispose, use async 'BeginInvoke' instead.
                    ClassThread.ExecAsync(g_mFormDebugger, Sub()
                                                               If (g_mFormDebuggerCriticalPopupFatalException IsNot Nothing AndAlso Not g_mFormDebuggerCriticalPopupFatalException.IsDisposed) Then
                                                                   Return
                                                               End If

                                                               If (m_DebuggingState <> ENUM_DEBUGGING_STATE.PAUSED) Then
                                                                   Return
                                                               End If

                                                               UpdateBreakpointListView()

                                                               g_mFormDebugger.ToolStripStatusLabel_DebugState.Text = "Status: Debugger awaiting input..."
                                                               g_mFormDebugger.ToolStripStatusLabel_DebugState.BackColor = Color.Orange

                                                               If (g_mFormDebugger.WindowState = FormWindowState.Minimized) Then
                                                                   ClassTools.ClassForms.FormWindowCommand(g_mFormDebugger, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                                                               End If
                                                               g_mFormDebugger.Activate()

                                                               g_mFormDebuggerCriticalPopupFatalException = New FormDebuggerCriticalPopup(g_mFormDebugger, "SourceMod Fatal Error", "The debugger caught fatal errors!", String.Join(Environment.NewLine, sLines))
                                                               g_mFormDebuggerCriticalPopupFatalException.Show(g_mFormDebugger)

                                                               If (g_mFormDebuggerCriticalPopupFatalException.WindowState = FormWindowState.Minimized) Then
                                                                   ClassTools.ClassForms.FormWindowCommand(g_mFormDebuggerCriticalPopupFatalException, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                                                               End If
                                                               g_mFormDebuggerCriticalPopupFatalException.Activate()

                                                               g_mFormDebugger.g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnDebuggerDebugPause())
                                                           End Sub)
                End SyncLock
            End If
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As ObjectDisposedException
            'Filter unexpected disposes
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub



    Class WinNative
        Enum ThreadAccess
            DIRECT_IMPERSONATION = &H200
            GET_CONTEXT = 8
            IMPERSONATE = &H100
            QUERY_INFORMATION = &H40
            SET_CONTEXT = &H10
            SET_INFORMATION = &H20
            SET_THREAD_TOKEN = &H80
            SUSPEND_RESUME = 2
            TERMINATE = 1
        End Enum

        <DllImport("kernel32.dll")>
        Private Shared Function OpenThread(dwDesiredAccess As ThreadAccess, bInheritHandle As Boolean, dwThreadId As Integer) As IntPtr
        End Function
        <DllImport("kernel32.dll")>
        Private Shared Function ResumeThread(hThread As IntPtr) As Integer
        End Function
        <DllImport("Kernel32.dll")>
        Private Shared Function SuspendThread(hThread As IntPtr) As Integer
        End Function
        <DllImport("kernel32.dll")>
        Private Shared Function CloseHandle(hHandle As IntPtr) As Boolean
        End Function

        Public Shared Function IsSuspended(pProcess As Process) As Boolean
            Dim mThreads As ProcessThreadCollection = pProcess.Threads
            If (mThreads.Count < 1) Then
                Return False
            End If

            Return (mThreads(0).ThreadState = ThreadState.Wait AndAlso mThreads(0).WaitReason = ThreadWaitReason.Suspended)
        End Function

        ''' <summary>
        ''' Suspend process.
        ''' </summary>
        ''' <param name="pProcess">The process class</param>
        Public Shared Sub SuspendProcess(pProcess As Process)
            Dim mThreads As ProcessThreadCollection = pProcess.Threads

            For i = 0 To mThreads.Count - 1
                Dim hThread As IntPtr = IntPtr.Zero
                Try
                    hThread = OpenThread(ThreadAccess.SUSPEND_RESUME, False, mThreads(i).Id)
                    If hThread <> IntPtr.Zero Then
                        SuspendThread(hThread)
                    End If
                Catch ex As Exception
                Finally
                    If hThread <> IntPtr.Zero Then
                        CloseHandle(hThread)
                    End If
                End Try
            Next
        End Sub

        ''' <summary>
        ''' Resumes process.
        ''' </summary>
        ''' <param name="pProcess">The process class</param> 
        Public Shared Sub ResumeProcess(pProcess As Process)
            Dim mThreads As ProcessThreadCollection = pProcess.Threads

            For i = 0 To mThreads.Count - 1
                Dim hThread As IntPtr = IntPtr.Zero
                Try
                    hThread = OpenThread(ThreadAccess.SUSPEND_RESUME, False, mThreads(i).Id)
                    If hThread <> IntPtr.Zero Then
                        Dim suspendCount As Integer = 0
                        Do
                            suspendCount = ResumeThread(hThread)
                        Loop While suspendCount > 0
                    End If
                Catch ex As Exception
                Finally
                    If hThread <> IntPtr.Zero Then
                        CloseHandle(hThread)
                    End If
                End Try
            Next
        End Sub
    End Class

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                RemoveFileSystemWatcher()

                ClassThread.Abort(g_mListViewEntitiesUpdaterThread)

                If (g_mFormDebuggerException IsNot Nothing AndAlso Not g_mFormDebuggerException.IsDisposed) Then
                    g_mFormDebuggerException.Dispose()
                    g_mFormDebuggerException = Nothing
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
