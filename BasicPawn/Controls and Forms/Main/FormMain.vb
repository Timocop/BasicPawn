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


Imports System.ComponentModel

Public Class FormMain
    Public g_ClassTabControl As ClassTabControl
    Public g_ClassSyntaxUpdater As ClassBackgroundUpdater
    Public g_ClassSyntaxParser As ClassSyntaxParser
    Public g_ClassTextEditorTools As ClassTextEditorTools
    Public g_ClassCustomHighlighting As ClassTextEditorTools.ClassCustomHighlighting
    Public g_ClassTextEditorCommands As ClassTextEditorTools.ClassTestEditorCommands
    Public g_ClassPluginController As ClassPluginController
    Public g_ClassSyntaxTools As ClassSyntaxTools
    Public WithEvents g_ClassCrossAppCom As ClassCrossAppComunication
    Public WithEvents g_ClassCrossAppComPost As ClassCrossAppComunication

    Public g_mUCAutocomplete As UCAutocomplete
    Public g_mUCInformationList As UCInformationList
    Public g_mUCBookmarkDetails As UCBookmarkDetails
    Public g_mUCObjectBrowser As UCObjectBrowser
    Public g_mUCProjectBrowser As UCProjectBrowser
    Public g_mUCExplorerBrowser As UCExplorerBrowser
    Public g_mFormToolTip As FormToolTip
    Public g_mFormDebugger As FormDebugger
    Public g_mFormInstanceManager As FormInstanceManager
    Public g_mUCStartPage As UCStartPage
    Public g_mUCTextMinimap As ClassTextMinimap

    Public g_cDarkTextEditorBackgroundColor As Color = Color.FromArgb(255, 32, 32, 32)
    Public g_cDarkFormDetailsBackgroundColor As Color = Color.FromArgb(255, 24, 24, 24)
    Public g_cDarkFormBackgroundColor As Color = Color.FromArgb(255, 48, 48, 48)
    Public g_cDarkFormMenuBackgroundColor As Color = Color.FromArgb(255, 64, 64, 64)

    Public Const COMMSG_SERVERNAME As String = "BasicPawnComServer-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMMSG_SERVERNAMEPOST As String = "BasicPawnComServerPost-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_OPEN_FILE As String = "BasicPawnComServer-OpenFile-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_REQUEST_TABS As String = "BasicPawnComServer-RequestTabs-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_REQUEST_TABS_CALLBACK As String = "BasicPawnComServer-RequestTabsCallback-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_CLOSE_TAB As String = "BasicPawnComServer-CloseTab-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_CLOSE_APP As String = "BasicPawnComServer-CloseApp-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_ACTIVATE_FORM_OR_TAB As String = "BasicPawnComServer-ActivateFormOrTab-04e3632f-5472-42c5-929a-c3e0c2b35324"

    Public ReadOnly g_iDefaultDetailsSplitterDistance As Integer = 150

    Private g_bFormPostCreate As Boolean = False
    Private g_bFormPostLoad As Boolean = False
    Private g_bIgnoreComboBoxEvent As Boolean = False
    Private g_bIgnoreCheckedChangedEvent As Boolean = False
    Private g_sTabsClipboardIdentifier As String = ""
    Private g_mTabControlDragTab As ClassTabControl.ClassTab = Nothing
    Private g_mTabControlDragPoint As Point = Point.Empty

    Public Sub New()
        g_ClassCrossAppCom = New ClassCrossAppComunication
        g_ClassCrossAppCom.Hook(COMMSG_SERVERNAME)

        'Load Settings 
        ClassSettings.LoadSettings()
        ClassConfigs.ClassKnownConfigs.LoadKnownConfigs()

        'Load source files via Arguments 
        Dim lOpenFileList As New List(Of String)
        For i = 1 To Environment.GetCommandLineArgs.Length - 1
            If (IO.File.Exists(Environment.GetCommandLineArgs(i))) Then
                lOpenFileList.Add(Environment.GetCommandLineArgs(i))
            End If
        Next

        If (lOpenFileList.Count > 0) Then
            'Open all project files 
            Dim bOpenProject As Boolean = False
            For i = 0 To lOpenFileList.Count - 1
                If (IO.Path.GetExtension(lOpenFileList(i)).ToLower = UCProjectBrowser.ClassProjectControl.g_sProjectExtension) Then
                    bOpenProject = True
                    Exit For
                End If
            Next

            If (Not bOpenProject) Then
                'Open all files in the oldest BasicPawn instance
                If (Not ClassSettings.g_bSettingsAlwaysOpenNewInstance AndAlso Array.IndexOf(Environment.GetCommandLineArgs, "-newinstance") = -1) Then
                    Dim pBasicPawnProc As Process() = Process.GetProcessesByName(IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath))
                    If (pBasicPawnProc.Length > 0) Then
                        Const C_INSTANCES_PID = "Pid"
                        Const C_INSTANCES_TICKS = "Tick"

                        Dim lInstances As New List(Of Dictionary(Of String, Object)) 'See keys: C_INSTANCES_*

                        For Each pProcess As Process In pBasicPawnProc
                            Try
                                If (IO.Path.GetFullPath(pProcess.MainModule.FileName).ToLower <> IO.Path.GetFullPath(Application.ExecutablePath).ToLower) Then
                                    Continue For
                                End If

                                Dim mInstancesDic As New Dictionary(Of String, Object)
                                mInstancesDic(C_INSTANCES_PID) = pProcess.Id
                                mInstancesDic(C_INSTANCES_TICKS) = pProcess.StartTime.Ticks

                                lInstances.Add(mInstancesDic)
                            Catch ex As Exception
                                'Ignore random exceptions
                            End Try
                        Next

                        'Takes care of the ticks sorting. If there are instances with the same ticks it should sort always the same.
                        lInstances.Sort(Function(x As Dictionary(Of String, Object), y As Dictionary(Of String, Object))
                                            Return CLng(x(C_INSTANCES_TICKS)).CompareTo(CLng(y(C_INSTANCES_TICKS)))
                                        End Function)

                        If (lInstances.Count > 0) Then
                            Dim iMasterId As Integer = CInt(lInstances(0)(C_INSTANCES_PID))

                            If (iMasterId <> Process.GetCurrentProcess.Id) Then
                                Try
                                    Const iTimeout As Integer = 10000

                                    Process.GetProcessById(iMasterId).WaitForInputIdle(iTimeout)

                                    Dim mStopWatch As New Stopwatch
                                    mStopWatch.Start()

                                    'Check if a server is listening
                                    While True
                                        If (mStopWatch.Elapsed > New TimeSpan(0, 0, 0, 0, iTimeout)) Then
                                            Exit While
                                        End If

                                        If (g_ClassCrossAppCom.ServerExist() AndAlso g_ClassCrossAppCom.ServerExistEx(COMMSG_SERVERNAMEPOST)) Then
                                            Exit While
                                        End If

                                        Threading.Thread.Sleep(100)
                                    End While

                                    For i = 0 To lOpenFileList.Count - 1
                                        If (IO.Path.GetExtension(lOpenFileList(i)).ToLower <> UCProjectBrowser.ClassProjectControl.g_sProjectExtension) Then
                                            Dim mMsg As New ClassCrossAppComunication.ClassMessage(COMARG_OPEN_FILE, CStr(iMasterId), lOpenFileList(i))
                                            g_ClassCrossAppCom.SendMessage(mMsg)
                                        End If
                                    Next
                                Catch ex As Exception
                                    ClassExceptionLog.WriteToLogMessageBox(ex)
                                End Try

                                Me.WindowState = FormWindowState.Minimized
                                Me.ShowInTaskbar = False
                                Application.Exit()
                                End
                            End If
                        End If
                    End If
                End If
            End If
        End If



        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        InitializeFilter()

        ImageList_Details.Images.Clear()
        ImageList_Details.Images.Add("0", My.Resources.imageres_5333_16x16)
        ImageList_Details.Images.Add("1", My.Resources.imageres_5332_16x16)
        ImageList_Details.Images.Add("2", My.Resources.imageres_5354_16x16)

        TabPage_Autocomplete.ImageKey = "0"
        TabPage_Information.ImageKey = "1"
        TabPage_Bookmarks.ImageKey = "2"

        ' Some control init
        g_bIgnoreComboBoxEvent = True
        ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndex = 0
        g_bIgnoreComboBoxEvent = False

        g_ClassTabControl = New ClassTabControl(Me)
        g_ClassSyntaxUpdater = New ClassBackgroundUpdater(Me)
        g_ClassSyntaxParser = New ClassSyntaxParser(Me)
        g_ClassTextEditorTools = New ClassTextEditorTools(Me)
        g_ClassCustomHighlighting = New ClassTextEditorTools.ClassCustomHighlighting(Me)
        g_ClassTextEditorCommands = New ClassTextEditorTools.ClassTestEditorCommands(Me)
        g_ClassPluginController = New ClassPluginController(Me)
        g_ClassSyntaxTools = New ClassSyntaxTools(Me)

        ' Load other Forms/Controls
        g_mUCAutocomplete = New UCAutocomplete(Me) With {
            .Parent = TabPage_Autocomplete,
            .Dock = DockStyle.Fill
        }
        g_mUCAutocomplete.Show()

        g_mUCInformationList = New UCInformationList(Me) With {
            .Parent = TabPage_Information,
            .Dock = DockStyle.Fill
        }
        g_mUCInformationList.Show()

        g_mUCBookmarkDetails = New UCBookmarkDetails(Me) With {
            .Parent = TabPage_Bookmarks,
            .Dock = DockStyle.Fill
        }
        g_mUCBookmarkDetails.Show()

        g_mUCObjectBrowser = New UCObjectBrowser(Me) With {
            .Parent = TabPage_ObjectBrowser,
            .Dock = DockStyle.Fill
        }
        g_mUCObjectBrowser.Show()

        g_mUCProjectBrowser = New UCProjectBrowser(Me) With {
            .Parent = TabPage_ProjectBrowser,
            .Dock = DockStyle.Fill
        }
        g_mUCProjectBrowser.Show()

        g_mUCExplorerBrowser = New UCExplorerBrowser(Me) With {
            .Parent = TabPage_ExplorerBrowser,
            .Dock = DockStyle.Fill
        }
        g_mUCExplorerBrowser.Show()

        g_mFormToolTip = New FormToolTip(Me) With {
            .Owner = Me
        }
        g_mFormToolTip.Hide()

        g_mUCStartPage = New UCStartPage(Me) With {
            .Parent = Me,
            .Dock = DockStyle.Fill
        }
        g_mUCStartPage.BringToFront()
        g_mUCStartPage.Show()

        g_mUCTextMinimap = New ClassTextMinimap(Me) With {
            .Parent = SplitContainer_ToolboxAndEditor.Panel2,
            .Dock = DockStyle.Right
        }
        g_mUCTextMinimap.SendToBack()
        g_mUCTextMinimap.Show()

        SplitContainer_ToolboxSourceAndDetails.SplitterDistance = (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)

        g_bFormPostCreate = True

        g_ClassCrossAppComPost = New ClassCrossAppComunication
        g_ClassCrossAppComPost.Hook(COMMSG_SERVERNAMEPOST)
    End Sub

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Display Mono/Wine version
        Dim sWineVersion As String = ClassTools.ClassOperatingSystem.GetWineVersion()
        Dim bMonoRuntime As Boolean = ClassTools.ClassOperatingSystem.IsMonoRuntime()
        ToolStripStatusLabel_AppVersion.Text = String.Format("v.{0} {1} {2}",
                                                             Application.ProductVersion,
                                                             If(sWineVersion IsNot Nothing, "| Running on Wine " & sWineVersion, ""),
                                                             If(bMonoRuntime, "| Running on Mono (Unsupported!)", "")).Trim

        'Clean tabs
        g_ClassTabControl.Init()

        'Hide StartPage when disabled in settings
        If (Not ClassSettings.g_bSettingsAutoShowStartPage AndAlso g_mUCStartPage.Visible) Then
            g_mUCStartPage.Hide()
        End If

        'Load source files via Arguments 
        Dim lOpenFileList As New List(Of String)
        For i = 1 To Environment.GetCommandLineArgs.Length - 1
            If (IO.File.Exists(Environment.GetCommandLineArgs(i))) Then
                lOpenFileList.Add(Environment.GetCommandLineArgs(i))
            End If
        Next

        If (lOpenFileList.Count > 0) Then
            'Hide StartPage when files are going to be opened. Such as project and source files.
            If (g_mUCStartPage.Visible) Then
                g_mUCStartPage.Hide()
            End If

            'Open all project files 
            Dim bAppendFiles As Boolean = False
            For i = 0 To lOpenFileList.Count - 1
                Try
                    If (IO.Path.GetExtension(lOpenFileList(i)).ToLower = UCProjectBrowser.ClassProjectControl.g_sProjectExtension) Then
                        g_mUCProjectBrowser.g_ClassProjectControl.LoadProject(lOpenFileList(i), bAppendFiles, ClassSettings.g_bSettingsAutoOpenProjectFiles)
                        bAppendFiles = True
                    End If
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            Next

            'Open all files here
            Try
                g_ClassTabControl.BeginUpdate()

                For i = 0 To lOpenFileList.Count - 1
                    Try
                        If (IO.Path.GetExtension(lOpenFileList(i)).ToLower <> UCProjectBrowser.ClassProjectControl.g_sProjectExtension) Then
                            Dim mTab = g_ClassTabControl.AddTab()
                            mTab.OpenFileTab(lOpenFileList(i))
                        End If
                    Catch ex As Exception
                        ClassExceptionLog.WriteToLogMessageBox(ex)
                    End Try
                Next
            Finally
                g_ClassTabControl.EndUpdate()
            End Try
        End If

        'Update Autocomplete 
        g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL)
        For j = 0 To g_ClassTabControl.m_TabsCount - 1
            g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL, g_ClassTabControl.m_Tab(j), ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS.NOONE)
        Next

        'UpdateTextEditorControl1Colors()
        g_ClassSyntaxTools.UpdateFormColors()

        g_ClassSyntaxUpdater.StartThread()

        While True
            For i = 1 To Environment.GetCommandLineArgs.Length - 1
                If (Environment.GetCommandLineArgs(i).ToLower = "-safemode") Then
                    Exit While
                End If
            Next

            g_ClassPluginController.LoadPlugins(IO.Path.Combine(Application.StartupPath, "plugins"))

            Exit While
        End While

        g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnPluginStart(Me, g_ClassPluginController.m_PluginEnabledByConfig(j)))

        If (lOpenFileList.Count < 1) Then
            Dim mFormTip As FormTipOfTheDay = Nothing
            For Each mForm In Application.OpenForms
                If (TypeOf mForm Is FormTipOfTheDay) Then
                    mFormTip = DirectCast(mForm, FormTipOfTheDay)
                    Exit For
                End If
            Next

            If (mFormTip IsNot Nothing) Then
                mFormTip.Activate()
            Else
                Dim mTipForm As New FormTipOfTheDay
                If (mTipForm.m_DoNotShow) Then
                    mTipForm.Dispose()
                Else
                    mTipForm.Show(Me)
                End If
            End If
        End If

        Dim mCheckUpdasteThread As New Threading.Thread(Sub()
                                                            Try
                                                                Dim bForceUpdate As Boolean = False

#If DEBUG Then
                                                                bForceUpdate = True
                                                                Threading.Thread.Sleep(2500)
#Else
                                                                Threading.Thread.Sleep(10000)
#End If


                                                                If (bForceUpdate OrElse ClassUpdate.CheckUpdateAvailable(Nothing)) Then
                                                                    ClassThread.ExecAsync(Me, Sub()
                                                                                                  'Make update button visible
                                                                                                  ToolStripMenuItem_NewUpdate.Visible = True
                                                                                              End Sub)
                                                                End If
                                                            Catch ex As Threading.ThreadAbortException
                                                                Throw
                                                            Catch ex As Exception
                                                            End Try
                                                        End Sub) With {
            .IsBackground = True
        }
        mCheckUpdasteThread.Start()

        'Load last window info
        ClassSettings.LoadWindowInfo(Me)
        LoadViews()

        g_bFormPostLoad = True
    End Sub

    Private Sub FormMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM)
                                                   If (Not j.mPluginInterface.OnPluginEnd()) Then
                                                       e.Cancel = True
                                                   End If
                                               End Sub)

        'Save tabs
        For i = 0 To g_ClassTabControl.m_TabsCount - 1
            Try
                If (Not g_ClassTabControl.PromptSaveTab(i)) Then
                    e.Cancel = True
                End If
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        Next

        'Save projects
        Try
            If (g_mUCProjectBrowser.g_ClassProjectControl.PrompSaveProject()) Then
                e.Cancel = True
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try

        'Close debugger 
        If (g_mFormDebugger IsNot Nothing AndAlso Not g_mFormDebugger.IsDisposed) Then
            g_mFormDebugger.Close()
        End If

        If (g_mFormDebugger IsNot Nothing AndAlso Not g_mFormDebugger.IsDisposed) Then
            MessageBox.Show("You can not close BasicPawn while debugging!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            e.Cancel = True
        End If

        'Cleanup invalid foldings
        g_ClassTabControl.CleanInvalidSavedFoldStates()

        'Save window info
        ClassSettings.SaveWindowInfo(Me)
        SaveViews()
    End Sub

    Private Sub ToolStripMenuItem_TabClose_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabClose.Click
        Try
            If (ClassSettings.g_bSettingsTabCloseGotoPrevious) Then
                g_ClassTabControl.m_ActiveTab.RemoveTabGotoLast(True)
            Else
                g_ClassTabControl.m_ActiveTab.RemoveTab(True)
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_TabMoveRight_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabMoveRight.Click
        Dim iActiveIndex As Integer = g_ClassTabControl.m_ActiveTabIndex
        Dim iToIndex As Integer = iActiveIndex + 1

        If (iToIndex > g_ClassTabControl.m_TabsCount - 1) Then
            Return
        End If

        g_ClassTabControl.SwapTabs(iActiveIndex, iToIndex)
    End Sub

    Private Sub ToolStripMenuItem_TabMoveLeft_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabMoveLeft.Click
        Dim iActiveIndex As Integer = g_ClassTabControl.m_ActiveTabIndex
        Dim iToIndex As Integer = iActiveIndex - 1

        If (iToIndex < 0) Then
            Return
        End If

        g_ClassTabControl.SwapTabs(iActiveIndex, iToIndex)
    End Sub

    Private Sub ToolStripMenuItem_TabLastViewedRight_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabLastViewedRight.Click
        Dim mNextTab = g_ClassTabControl.GetNextTabByLastSelection(g_ClassTabControl.m_ActiveTab, 1)
        If (mNextTab Is Nothing) Then
            Return
        End If

        'Keep original time
        Dim mLastTime = mNextTab.m_LastViewTime
        mNextTab.SelectTab()
        mNextTab.m_LastViewTime = mLastTime
    End Sub

    Private Sub ToolStripMenuItem_TabLastViewedLeft_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabLastViewedLeft.Click
        Dim mNextTab = g_ClassTabControl.GetNextTabByLastSelection(g_ClassTabControl.m_ActiveTab, -1)
        If (mNextTab Is Nothing) Then
            Return
        End If

        'Keep original time
        Dim mLastTime = mNextTab.m_LastViewTime
        mNextTab.SelectTab()
        mNextTab.m_LastViewTime = mLastTime
    End Sub

    Private Sub ToolStripMenuItem_TabOpenInstance_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabOpenInstance.Click
        If (g_mFormInstanceManager IsNot Nothing AndAlso Not g_mFormInstanceManager.IsDisposed) Then
            Return
        End If

        g_mFormInstanceManager = New FormInstanceManager(Me)
        g_mFormInstanceManager.Show(Me)
    End Sub

    Public Sub UpdateFormConfigText()
        ToolStripStatusLabel_CurrentConfig.Text = "Config: " & g_ClassTabControl.m_ActiveTab.m_ActiveConfig.GetName
    End Sub

    Private Sub ContextMenuStrip_RightClick_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip_RightClick.Opening
        g_mUCAutocomplete.UpdateAutocomplete("")
        g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip("")

        ToolStripMenuItem_Outline.Enabled = (g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.FoldingManager.FoldMarker.Count > 0)
    End Sub

    Public Sub SaveViews()
        If (String.IsNullOrEmpty(Me.Name)) Then
            Return
        End If

        Using mStream = ClassFileStreamWait.Create(ClassSettings.g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT) From {
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "ViewToolbox", If(ToolStripMenuItem_ViewToolbox.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "ViewDetails", If(ToolStripMenuItem_ViewDetails.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "ViewMinimap", If(ToolStripMenuItem_ViewMinimap.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "ViewProgressAni", If(ToolStripMenuItem_ViewProgressAni.Checked, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "ToolboxSize", CStr(SplitContainer_ToolboxAndEditor.SplitterDistance)),
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "DetailsSize", CStr(SplitContainer_ToolboxSourceAndDetails.Height - SplitContainer_ToolboxSourceAndDetails.SplitterDistance))
                }

                mIni.WriteKeyValue(lContent.ToArray)
            End Using
        End Using
    End Sub

    Public Sub LoadViews()
        If (String.IsNullOrEmpty(Me.Name)) Then
            Return
        End If

        Dim tmpStr As String
        Dim tmpInt As Integer

        Using mStream = ClassFileStreamWait.Create(ClassSettings.g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                g_bIgnoreCheckedChangedEvent = True
                ToolStripMenuItem_ViewToolbox.Checked = (mIni.ReadKeyValue(Me.Name, "ViewToolbox", "1") <> "0")
                ToolStripMenuItem_ViewDetails.Checked = (mIni.ReadKeyValue(Me.Name, "ViewDetails", "1") <> "0")
                ToolStripMenuItem_ViewMinimap.Checked = (mIni.ReadKeyValue(Me.Name, "ViewMinimap", "1") <> "0")
                ToolStripMenuItem_ViewProgressAni.Checked = (mIni.ReadKeyValue(Me.Name, "ViewProgressAni", "1") <> "0")
                g_bIgnoreCheckedChangedEvent = False

                tmpStr = mIni.ReadKeyValue(Me.Name, "ToolboxSize", Nothing)
                If (tmpStr IsNot Nothing AndAlso Integer.TryParse(tmpStr, tmpInt) AndAlso tmpInt > -1) Then
                    SplitContainer_ToolboxAndEditor.SplitterDistance = tmpInt
                End If

                tmpStr = mIni.ReadKeyValue(Me.Name, "DetailsSize", Nothing)
                If (tmpStr IsNot Nothing AndAlso Integer.TryParse(tmpStr, tmpInt) AndAlso (SplitContainer_ToolboxSourceAndDetails.Height - tmpInt) > -1) Then
                    SplitContainer_ToolboxSourceAndDetails.SplitterDistance = (SplitContainer_ToolboxSourceAndDetails.Height - tmpInt)
                End If
            End Using
        End Using

        UpdateViews()
    End Sub

    Public Sub UpdateViews()
        SplitContainer_ToolboxAndEditor.Panel1Collapsed = (Not ToolStripMenuItem_ViewToolbox.Checked)
        SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = (Not ToolStripMenuItem_ViewDetails.Checked)
        g_mUCTextMinimap.Visible = (ToolStripMenuItem_ViewMinimap.Checked)
        ToolStripStatusLabel_AutocompleteProgress.Visible = If(ToolStripMenuItem_ViewProgressAni.Checked, ToolStripStatusLabel_AutocompleteProgress.Visible, False)
    End Sub

    Private Sub FormMain_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub CleanUp()
        g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnPluginEndPost())

        If (g_mFormInstanceManager IsNot Nothing AndAlso Not g_mFormInstanceManager.IsDisposed) Then
            g_mFormInstanceManager.Close()
            g_mFormInstanceManager.Dispose()
            g_mFormInstanceManager = Nothing
        End If

        g_ClassSyntaxUpdater.StopThread()
        g_ClassSyntaxParser.StopUpdate()
        g_mUCObjectBrowser.StopUpdate()

        If (g_ClassCrossAppCom IsNot Nothing) Then
            g_ClassCrossAppCom.Dispose()
            g_ClassCrossAppCom = Nothing
        End If

        If (g_ClassCrossAppComPost IsNot Nothing) Then
            g_ClassCrossAppComPost.Dispose()
            g_ClassCrossAppComPost = Nothing
        End If

        If (g_ClassCustomHighlighting IsNot Nothing) Then
            g_ClassCustomHighlighting.Dispose()
            g_ClassCustomHighlighting = Nothing
        End If

        If (g_ClassTabControl IsNot Nothing) Then
            g_ClassTabControl.Dispose()
            g_ClassTabControl = Nothing
        End If
    End Sub

    Private Sub Timer_CheckFiles_Tick(sender As Object, e As EventArgs) Handles Timer_CheckFiles.Tick
        Try
            Timer_CheckFiles.Stop()

            If (Not g_bFormPostLoad) Then
                Return
            End If

            g_ClassTabControl.CheckFilesChangedPrompt()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        Finally
            Timer_CheckFiles.Start()
        End Try
    End Sub

    Private Sub Timer_AutoSave_Tick(sender As Object, e As EventArgs) Handles Timer_AutoSave.Tick
        Try
            Timer_AutoSave.Stop()

            If (Not g_bFormPostLoad) Then
                Return
            End If

            If (Not ClassSettings.g_bSettingsAutoSaveSource AndAlso Not ClassSettings.g_bSettingsAutoSaveSourceTemp) Then
                Return
            End If

            Dim mActiveTab = g_ClassTabControl.m_ActiveTab

            If (mActiveTab.m_Changed) Then
                If (mActiveTab.m_IsUnsaved) Then
                    If (Not ClassSettings.g_bSettingsAutoSaveSourceTemp) Then
                        Return
                    End If

                    Dim sTempFile As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
                    IO.File.WriteAllText(sTempFile, "")

                    mActiveTab.m_File = sTempFile
                    mActiveTab.SaveFileTab(False)

                    g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL)
                Else
                    If (Not ClassSettings.g_bSettingsAutoSaveSource) Then
                        Return
                    End If

                    If (mActiveTab.m_InvalidFile) Then
                        Return
                    End If

                    mActiveTab.SaveFileTab(False)
                End If
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        Finally
            Timer_AutoSave.Start()
        End Try
    End Sub
    Private Sub Timer_SyntaxAnimation_Tick(sender As Object, e As EventArgs) Handles Timer_SyntaxAnimation.Tick
        Try
            Timer_SyntaxAnimation.Stop()

            If (Not g_bFormPostLoad) Then
                Return
            End If

            Dim iThreadCount = g_ClassSyntaxParser.GetAliveThreadCount()
            If (ToolStripMenuItem_ViewProgressAni.Checked AndAlso iThreadCount > 0) Then
                ToolStripStatusLabel_AutocompleteProgress.ToolTipText = String.Format("Parsing syntax {0}/{1}", iThreadCount, iThreadCount + g_ClassSyntaxParser.m_UpdateRequests.Count)
                ToolStripStatusLabel_AutocompleteProgress.Visible = True
            Else
                If (ToolStripStatusLabel_AutocompleteProgress.Visible) Then
                    ToolStripStatusLabel_AutocompleteProgress.ToolTipText = ""
                    ToolStripStatusLabel_AutocompleteProgress.Visible = False
                End If
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        Finally
            Timer_SyntaxAnimation.Start()
        End Try
    End Sub


    Private Sub FormMain_Move(sender As Object, e As EventArgs) Handles Me.Move
        Try
            If (Not g_bFormPostLoad) Then
                Return
            End If

            g_mUCAutocomplete.g_ClassToolTip.UpdateToolTipFormLocation()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub FormMain_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        Try
            If (Not g_bFormPostLoad) Then
                Return
            End If

            g_mUCAutocomplete.g_ClassToolTip.UpdateToolTipFormLocation()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef mMsg As Message, iKeyData As Keys) As Boolean
        ' Workaround since 'Keys.Enter' does not work on 'ShortcutKeys' property.
        If (iKeyData = (Keys.Control Or Keys.Alt Or Keys.Enter)) Then
            ToolStripMenuItem_EditInsertLineUp.PerformClick()
            Return False
        End If

        If (iKeyData = (Keys.Shift Or Keys.Control Or Keys.Alt Or Keys.Enter)) Then
            ToolStripMenuItem_EditInsertLineDown.PerformClick()
            Return False
        End If

        Return MyBase.ProcessCmdKey(mMsg, iKeyData)
    End Function
End Class
