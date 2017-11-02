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


Imports System.ComponentModel
Imports System.Text
Imports System.Text.RegularExpressions

Public Class FormMain
    Public g_ClassSyntaxUpdater As ClassSyntaxUpdater
    Public g_ClassSyntaxTools As ClassSyntaxTools
    Public g_ClassAutocompleteUpdater As ClassAutocompleteUpdater
    Public g_ClassTextEditorTools As ClassTextEditorTools
    Public g_ClassLineState As ClassTextEditorTools.ClassLineState
    Public g_ClassCustomHighlighting As ClassTextEditorTools.ClassCustomHighlighting
    Public g_ClassPluginController As ClassPluginController
    Public g_ClassTabControl As ClassTabControl
#Disable Warning IDE1006 ' Naming Styles
    Public WithEvents g_ClassCrossAppComunication As ClassCrossAppComunication
#Enable Warning IDE1006 ' Naming Styles

    Public g_mUCAutocomplete As UCAutocomplete
    Public g_mUCInformationList As UCInformationList
    Public g_mUCObjectBrowser As UCObjectBrowser
    Public g_mUCProjectBrowser As UCProjectBrowser
    Public g_mFormToolTip As FormToolTip
    Public g_mFormDebugger As FormDebugger
    Public g_mFormOpenTabFromInstances As FormOpenTabFromInstances
    Public g_mUCStartPage As UCStartPage
    Public g_mUCTextMinimap As ClassTextMinimap

    Public g_cDarkTextEditorBackgroundColor As Color = Color.FromArgb(255, 26, 26, 26)
    Public g_cDarkFormDetailsBackgroundColor As Color = Color.FromArgb(255, 24, 24, 24)
    Public g_cDarkFormBackgroundColor As Color = Color.FromArgb(255, 48, 48, 48)
    Public g_cDarkFormMenuBackgroundColor As Color = Color.FromArgb(255, 64, 64, 64)

    Public Const COMMSG_SERVERNAME As String = "BasicPawnComServer-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_OPEN_FILE_BY_PID As String = "BasicPawnComServer-OpenFileByPID-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_REQUEST_TABS As String = "BasicPawnComServer-RequestTabs-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_REQUEST_TABS_ANSWER As String = "BasicPawnComServer-RequestTabsAnswer-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_CLOSE_TAB As String = "BasicPawnComServer-CloseTab-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_SHOW_PING_FLASH As String = "BasicPawnComServer-ShowPingFlash-04e3632f-5472-42c5-929a-c3e0c2b35324"

    Private g_mPingFlashPanel As ClassPanelAlpha
    Private g_bFormPostCreate As Boolean = False
    Private g_bFormPostLoad As Boolean = False
    Private g_bIgnoreComboBoxEvent As Boolean = False
    Private g_bIgnoreCheckedChangedEvent As Boolean = False
    Private g_sTabsClipboardIdentifier As String = ""
    Private g_mTabControlDragTab As ClassTabControl.SourceTabPage = Nothing
    Private g_mTabControlDragPoint As Point = Point.Empty
    Private g_iDefaultDetailsSplitterDistance As Integer = 150


#Region "GUI Stuff"
    Public Sub New()
        g_ClassCrossAppComunication = New ClassCrossAppComunication
        g_ClassCrossAppComunication.Hook(COMMSG_SERVERNAME)

        'Load Settings 
        ClassSettings.LoadSettings()

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
                If (Not ClassSettings.g_iSettingsAlwaysOpenNewInstance AndAlso Array.IndexOf(Environment.GetCommandLineArgs, "-newinstance") = -1) Then
                    Dim pBasicPawnProc As Process() = Process.GetProcessesByName(IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath))
                    If (pBasicPawnProc.Length > 0) Then
                        Dim lInstances As New List(Of Object()) '{ProcessId, StartTime.Ticks}

                        For Each pProcess As Process In pBasicPawnProc
                            Try
                                If (IO.Path.GetFullPath(pProcess.MainModule.FileName).ToLower <> IO.Path.GetFullPath(Application.ExecutablePath).ToLower) Then
                                    Continue For
                                End If

                                lInstances.Add(New Object() {pProcess.Id, pProcess.StartTime.Ticks})
                            Catch ex As Exception
                                'Ignore random exceptions
                            End Try
                        Next

                        'Takes care of the ticks sorting. If there are instances with the same ticks it should sort always the same.
                        lInstances.Sort(Function(x As Object(), y As Object())
                                            Return CLng(x(1)).CompareTo(CLng(y(1)))
                                        End Function)

                        If (lInstances.Count > 0) Then
                            Dim iMasterId As Integer = CInt(lInstances(0)(0))

                            If (iMasterId <> Process.GetCurrentProcess.Id) Then
                                Try
                                    Process.GetProcessById(iMasterId).WaitForInputIdle(5000)

                                    For i = 0 To lOpenFileList.Count - 1
                                        If (IO.Path.GetExtension(lOpenFileList(i)).ToLower <> UCProjectBrowser.ClassProjectControl.g_sProjectExtension) Then
                                            Dim mMsg As New ClassCrossAppComunication.ClassMessage(COMARG_OPEN_FILE_BY_PID, CStr(iMasterId), lOpenFileList(i))
                                            g_ClassCrossAppComunication.SendMessage(mMsg)
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
        ImageList_Details.Images.Clear()
        ImageList_Details.Images.Add("0", My.Resources.imageres_5332_16x16)
        ImageList_Details.Images.Add("1", My.Resources.imageres_5333_16x16)

        g_ClassSyntaxUpdater = New ClassSyntaxUpdater(Me)
        g_ClassSyntaxTools = New ClassSyntaxTools(Me)
        g_ClassAutocompleteUpdater = New ClassAutocompleteUpdater(Me)
        g_ClassTextEditorTools = New ClassTextEditorTools(Me)
        g_ClassLineState = New ClassTextEditorTools.ClassLineState(Me)
        g_ClassCustomHighlighting = New ClassTextEditorTools.ClassCustomHighlighting(Me)
        g_ClassPluginController = New ClassPluginController(Me)
        g_ClassTabControl = New ClassTabControl(Me)

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

        g_mPingFlashPanel = New ClassPanelAlpha With {
            .Name = "@KeepForeBackColor",
            .Parent = Me,
            .Dock = DockStyle.Fill,
            .m_TransparentBackColor = Color.FromKnownColor(KnownColor.RoyalBlue),
            .m_Opacity = 0
        }
        g_mPingFlashPanel.BringToFront()
        g_mPingFlashPanel.Visible = False

        g_bFormPostCreate = True
    End Sub

    Public Sub UpdateFormConfigText()
        ToolStripStatusLabel_CurrentConfig.Text = "Config: " & ClassConfigs.m_ActiveConfig.GetName
    End Sub

    Public Sub PrintInformation(sType As String, sMessage As String, Optional bClear As Boolean = False, Optional bShowInformationTab As Boolean = False, Optional bEnsureVisible As Boolean = False)
        ClassThread.ExecAsync(Me, Sub()
                                      If (g_mUCInformationList Is Nothing) Then
                                          Return
                                      End If

                                      If (bClear) Then
                                          g_mUCInformationList.ListBox_Information.Items.Clear()
                                      End If

                                      Dim iIndex = g_mUCInformationList.ListBox_Information.Items.Add(String.Format("{0} ({1}) {2}", sType, Now.ToString, sMessage))

                                      ToolStripStatusLabel_LastInformation.Text = sMessage

                                      If (bEnsureVisible) Then
                                          'Scroll to item
                                          g_mUCInformationList.ListBox_Information.TopIndex = iIndex
                                      End If

                                      If (bShowInformationTab) Then
                                          SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False

                                          If (SplitContainer_ToolboxSourceAndDetails.SplitterDistance > (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)) Then
                                              SplitContainer_ToolboxSourceAndDetails.SplitterDistance = (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)
                                          End If

                                          TabControl_Details.SelectTab(1)
                                          End If
                                  End Sub)
    End Sub

    Private Sub ContextMenuStrip_RightClick_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip_RightClick.Opening
        g_mUCAutocomplete.UpdateAutocomplete("")
        g_mUCAutocomplete.g_ClassToolTip.m_IntelliSenseFunction = ""
        g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
    End Sub
#End Region

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Display Mono/Wine version
        Dim sWineVersion As String = ClassTools.ClassOperatingSystem.GetWineVersion()
        Dim bMonoRuntime As Boolean = ClassTools.ClassOperatingSystem.IsMonoRuntime()
        ToolStripStatusLabel_AppVersion.Text = String.Format("v.{0} {1} {2}",
                                                             Application.ProductVersion,
                                                             If(sWineVersion IsNot Nothing, "| Running on Wine " & sWineVersion, ""),
                                                             If(bMonoRuntime, "| Running on Mono (Unsupported!)", "")).Trim

        'Some control init
        g_bIgnoreComboBoxEvent = True
        ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndex = 0
        g_bIgnoreComboBoxEvent = False

        'Load default configs
        For Each mConfig As ClassConfigs.STRUC_CONFIG_ITEM In ClassConfigs.GetConfigs(False)
            If (mConfig.g_bAutoload) Then
                ClassConfigs.m_ActiveConfig = mConfig
                UpdateFormConfigText()
                Exit For
            End If
        Next

        'Clean tabs
        g_ClassTabControl.Init()

        'Hide StartPage when disabled in settings
        If (Not ClassSettings.g_iSettingsAutoShowStartPage AndAlso g_mUCStartPage.Visible) Then
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
                If (IO.Path.GetExtension(lOpenFileList(i)).ToLower = UCProjectBrowser.ClassProjectControl.g_sProjectExtension) Then
                    g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile = lOpenFileList(i)
                    g_mUCProjectBrowser.g_ClassProjectControl.LoadProject(bAppendFiles)
                    bAppendFiles = True
                End If
            Next

            'Open all files here
            For i = 0 To lOpenFileList.Count - 1
                If (IO.Path.GetExtension(lOpenFileList(i)).ToLower <> UCProjectBrowser.ClassProjectControl.g_sProjectExtension) Then
                    Dim mTab = g_ClassTabControl.AddTab()
                    mTab.OpenFileTab(lOpenFileList(i), True)
                End If
            Next

        End If

        'Update Autocomplete 
        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, Nothing)
        For j = 0 To g_ClassTabControl.m_TabsCount - 1
            g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, g_ClassTabControl.m_Tab(j).m_Identifier)
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

        Dim mCheckUpdasteThread As New Threading.Thread(Sub()
                                                            Try
#If DEBUG Then
                                                                ClassThread.ExecAsync(Me, Sub()
                                                                                              ToolStripMenuItem_NewUpdate.Visible = True
                                                                                          End Sub)
#End If
                                                                Threading.Thread.Sleep(10000)

                                                                If (ClassUpdate.CheckUpdateAvailable()) Then
                                                                    ClassThread.ExecAsync(Me, Sub()
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
            If (g_ClassTabControl.PromptSaveTab(i)) Then
                e.Cancel = True
            End If
        Next

        'Save projects
        If (g_mUCProjectBrowser.g_ClassProjectControl.PrompSaveProject()) Then
            e.Cancel = True
        End If

        'Close debugger 
        If (g_mFormDebugger IsNot Nothing AndAlso Not g_mFormDebugger.IsDisposed) Then
            g_mFormDebugger.Close()
        End If

        If (g_mFormDebugger IsNot Nothing AndAlso Not g_mFormDebugger.IsDisposed) Then
            MessageBox.Show("You can not close BasicPawn while debugging!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            e.Cancel = True
        End If

        'Save window info
        ClassSettings.SaveWindowInfo(Me)
        SaveViews()
    End Sub


#Region "ContextMenuStrip_RightClick"
    Private Sub ToolStripMenuItem_Mark_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Mark.Click
        g_ClassTextEditorTools.MarkSelectedWord()
    End Sub

    Private Sub ToolStripMenuItem_Cut_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Cut.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_Copy_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Copy.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_Paste_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Paste.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_Delete_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Delete.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_SelectAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_SelectAll.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_OutlineExpandAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OutlineExpandAll.Click
        For Each iItem In g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.FoldingManager.FoldMarker
            iItem.IsFolded = False
        Next

        g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty)
    End Sub

    Private Sub ToolStripMenuItem_OutlineToggleAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OutlineToggleAll.Click
        With New ICSharpCode.TextEditor.Actions.ToggleAllFoldings
            .Execute(g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea)
        End With
    End Sub

    Private Sub ToolStripMenuItem_OutlineCollapseAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OutlineCollapseAll.Click
        For Each iItem In g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.FoldingManager.FoldMarker
            iItem.IsFolded = True
        Next

        g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty)
    End Sub

    Private Sub ToolStripMenuItem_Comment_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Comment.Click
        With New ICSharpCode.TextEditor.Actions.ToggleComment
            .Execute(g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea)
        End With
    End Sub
#End Region

#Region "MenuStrip_BasicPawn"

#Region "MenuStrip_File"
    Private Sub ToolStripMenuItem_FileNew_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileNew.Click
        g_ClassTabControl.AddTab(True, False, False, True)

        PrintInformation("[INFO]", "User created a new source file")
    End Sub

    Private Sub ToolStripMenuItem_FileNewWizard_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileNewWizard.Click
        g_ClassTabControl.AddTab(True, True, False, True)

        PrintInformation("[INFO]", "User created a new source file")
    End Sub

    Private Sub ToolStripMenuItem_FileProjectSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileProjectSave.Click
        Try
            Using i As New SaveFileDialog
                i.Filter = String.Format("BasicPawn Project|*{0}", UCProjectBrowser.ClassProjectControl.g_sProjectExtension)
                i.FileName = g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile

                If (i.ShowDialog = DialogResult.OK) Then
                    g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile = i.FileName
                Else
                    Return
                End If
            End Using

            g_mUCProjectBrowser.g_ClassProjectControl.SaveProject()

            g_mUCStartPage.g_mClassRecentItems.AddRecent(g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile)
            ShowPingFlash()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileProjectLoad_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileProjectLoad.Click
        Try

            Using i As New OpenFileDialog
                i.Filter = String.Format("BasicPawn Project|*{0}", UCProjectBrowser.ClassProjectControl.g_sProjectExtension)
                i.FileName = g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile
                i.Multiselect = True

                If (i.ShowDialog = DialogResult.OK) Then
                    Dim bAppendFiles As Boolean = False

                    g_ClassTabControl.RemoveAllTabs()

                    For Each sProjectFile As String In i.FileNames
                        g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile = sProjectFile
                        g_mUCProjectBrowser.g_ClassProjectControl.LoadProject(bAppendFiles)
                        bAppendFiles = True
                    Next
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileProjectClose_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileProjectClose.Click
        Try
            g_mUCProjectBrowser.g_ClassProjectControl.CloseProject()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileCloseAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileCloseAll.Click
        Try
            g_ClassTabControl.RemoveAllTabs()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileOpen_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileOpen.Click
        Try
            Using i As New OpenFileDialog
                i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|Pawn (Not fully supported)|*.pwn;*.p|AMX Mod X|*.sma|All files|*.*"
                i.FileName = g_ClassTabControl.m_ActiveTab.m_File
                i.Multiselect = True

                If (i.ShowDialog = DialogResult.OK) Then
                    For Each sFile As String In i.FileNames
                        Dim mTab = g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(sFile)
                        mTab.SelectTab(500)
                    Next
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSave.Click
        g_ClassTabControl.SaveFileTab(g_ClassTabControl.m_ActiveTabIndex)
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAll.Click
        For i = g_ClassTabControl.m_TabsCount - 1 To 0 Step -1
            g_ClassTabControl.SaveFileTab(i)
        Next
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAs.Click
        g_ClassTabControl.SaveFileTab(g_ClassTabControl.m_ActiveTabIndex, True)
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAsTemp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAsTemp.Click
        Dim sTempFile As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
        IO.File.WriteAllText(sTempFile, "")

        g_ClassTabControl.m_ActiveTab.m_File = sTempFile
        g_ClassTabControl.SaveFileTab(g_ClassTabControl.m_ActiveTabIndex)

        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, Nothing)
    End Sub

    Private Sub ToolStripMenuItem_FileSavePacked_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSavePacked.Click
        Try
            Dim sTempFile As String = ""
            Dim sPreSource As String = g_ClassTextEditorTools.GetCompilerPreProcessCode(Nothing, True, True, sTempFile)
            If (String.IsNullOrEmpty(sPreSource)) Then
                MessageBox.Show("Could not export packed source. See information tab for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return
            End If

            If (String.IsNullOrEmpty(sTempFile)) Then
                Throw New ArgumentException("Last Pre-Process source invalid")
            End If

            With New ClassDebuggerRunner.ClassPreProcess(Nothing)
                .FixPreProcessFiles(sPreSource)
            End With

            'Replace the temp source file with the currently opened one
            sPreSource = Regex.Replace(sPreSource,
                                       String.Format("^\s*\#file ""{0}""\s*$", Regex.Escape(sTempFile)),
                                       String.Format("#file ""{0}""", g_ClassTabControl.m_ActiveTab.m_File),
                                       RegexOptions.IgnoreCase Or RegexOptions.Multiline)

            Using i As New SaveFileDialog
                i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|Pawn (Not fully supported)|*.pwn;*.p|AMX Mod X|*.sma|All files|*.*"
                i.FileName = IO.Path.Combine(IO.Path.GetDirectoryName(g_ClassTabControl.m_ActiveTab.m_File), IO.Path.GetFileNameWithoutExtension(g_ClassTabControl.m_ActiveTab.m_File) & ".packed" & IO.Path.GetExtension(g_ClassTabControl.m_ActiveTab.m_File))

                If (i.ShowDialog = DialogResult.OK) Then
                    IO.File.WriteAllText(i.FileName, sPreSource)
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileLoadTabs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileLoadTabs.Click
        g_mFormOpenTabFromInstances = New FormOpenTabFromInstances(Me)
        g_mFormOpenTabFromInstances.ShowDialog(Me)
    End Sub

    Private Sub ToolStripMenuItem_FileStartPage_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileStartPage.Click
        g_mUCStartPage.Show()
    End Sub

    Private Sub ToolStripMenuItem_FileOpenFolder_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileOpenFolder.Click
        Try
            If (g_ClassTabControl.m_ActiveTab.m_IsUnsaved OrElse g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                MessageBox.Show("Could not open current folder. Source file does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Process.Start("explorer.exe", String.Format("/select,""{0}""", g_ClassTabControl.m_ActiveTab.m_File))
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileExit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileExit.Click
        Me.Close()
    End Sub
#End Region

#Region "MenuStrip_View"
    Private Sub ToolStripMenuItem_ViewToolbox_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ViewToolbox.CheckedChanged
        If (g_bIgnoreCheckedChangedEvent) Then
            Return
        End If

        UpdateViews()
        SaveViews()
    End Sub

    Private Sub ToolStripMenuItem_ViewDetails_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ViewDetails.CheckedChanged
        If (g_bIgnoreCheckedChangedEvent) Then
            Return
        End If

        UpdateViews()
        SaveViews()
    End Sub

    Private Sub ToolStripMenuItem_ViewMinimap_CheckedChanged(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ViewMinimap.CheckedChanged
        If (g_bIgnoreCheckedChangedEvent) Then
            Return
        End If

        UpdateViews()
        SaveViews()
    End Sub
#End Region

#Region "MenuStrip_Tools"
    Private Sub ToolStripMenuItem_ToolsSettingsAndConfigs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsSettingsAndConfigs.Click
        Using i As New FormSettings(Me)
            If (i.ShowDialog(Me) = DialogResult.OK) Then
                UpdateFormConfigText()

                g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, Nothing)
                For j = 0 To g_ClassTabControl.m_TabsCount - 1
                    g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, g_ClassTabControl.m_Tab(j).m_Identifier)
                Next

                For j = 0 To g_ClassTabControl.m_TabsCount - 1
                    g_ClassTabControl.m_Tab(j).m_TextEditor.Document.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
                    g_ClassTabControl.m_Tab(j).m_TextEditor.Document.TextEditorProperties.IndentationSize = If(ClassSettings.g_iSettingsTabsToSpaces > 0, ClassSettings.g_iSettingsTabsToSpaces, 4)
                    g_ClassTabControl.m_Tab(j).m_TextEditor.Document.TextEditorProperties.ConvertTabsToSpaces = (ClassSettings.g_iSettingsTabsToSpaces > 0)
                    g_ClassTabControl.m_Tab(j).m_TextEditor.Refresh()
                Next

                g_ClassSyntaxTools.UpdateFormColors()
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_ToolsFormatCode_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsFormatCode.Click
        Try
            If (Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                MessageBox.Show("Nothing selected to format!", "Unable to format", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim lRealSourceLines As New List(Of String)
            Using mSR As New IO.StringReader(g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent)
                Dim sLine As String
                While True
                    sLine = mSR.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    lRealSourceLines.Add(sLine)
                End While
            End Using

            Dim sFormatedSource As String = g_ClassSyntaxTools.FormatCode(g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent, ClassSettings.ENUM_INDENTATION_TYPES.USE_SETTINGS, g_ClassTabControl.m_ActiveTab.m_Language)
            Dim lFormatedSourceLines As New List(Of String)
            Using mSR As New IO.StringReader(sFormatedSource)
                Dim sLine As String
                While True
                    sLine = mSR.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    lFormatedSourceLines.Add(sLine)
                End While
            End Using

            If (lRealSourceLines.Count <> lFormatedSourceLines.Count) Then
                Throw New ArgumentException("Formated number of lines are not equal with document number of lines")
            End If

            Try
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.StartUndoGroup()

                For i = lFormatedSourceLines.Count - 1 To 0 Step -1
                    Dim mLineSeg = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineSegment(i)
                    Dim sLine As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetText(mLineSeg.Offset, mLineSeg.Length)

                    If (Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset) AndAlso
                            Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset + mLineSeg.Length)) Then
                        Continue For
                    End If

                    If (sLine = lFormatedSourceLines(i)) Then
                        Continue For
                    End If

                    g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Remove(mLineSeg.Offset, mLineSeg.Length)
                    g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Insert(mLineSeg.Offset, lFormatedSourceLines(i))
                Next
            Finally
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.EndUndoGroup()
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Refresh()
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ToolsConvertTabsSpaces_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsConvertTabsSpaces.Click
        If (ClassSettings.g_iSettingsTabsToSpaces > 0) Then
            With New ICSharpCode.TextEditor.Actions.ConvertLeadingTabsToSpaces
                .Execute(g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea)
            End With
        Else
            With New ICSharpCode.TextEditor.Actions.ConvertLeadingSpacesToTabs
                .Execute(g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea)
            End With
        End If
    End Sub

    Private Sub ToolStripMenuItem_ToolsSearchReplace_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsSearchReplace.Click
        If (g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
            g_ClassTextEditorTools.ShowSearchAndReplace(g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SelectedText)
        Else
            g_ClassTextEditorTools.ShowSearchAndReplace("")
        End If
    End Sub

    Private Sub ToolStripMenuItem_ToolsShowInformation_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsShowInformation.Click
        SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False

        If (SplitContainer_ToolboxSourceAndDetails.SplitterDistance > (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)) Then
            SplitContainer_ToolboxSourceAndDetails.SplitterDistance = (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)
        End If

        TabControl_Details.SelectTab(1)
    End Sub

    Private Sub ToolStripMenuItem_ToolsClearInformationLog_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsClearInformationLog.Click
        PrintInformation("[INFO]", "Information log cleaned!", True, True)
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocompleteUpdate_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocompleteUpdate.Click
        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, Nothing)
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocompleteUpdateAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocompleteUpdateAll.Click
        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, Nothing)
        For j = 0 To g_ClassTabControl.m_TabsCount - 1
            g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, g_ClassTabControl.m_Tab(j).m_Identifier)
        Next
    End Sub

    Private Sub ToolStripComboBox_ToolsAutocompleteSyntax_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndexChanged
        If (g_bIgnoreComboBoxEvent) Then
            Return
        End If

        Select Case (ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndex)
            Case 0
                ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX
            Case 1
                ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6
            Case 2
                ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7
        End Select

        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, Nothing)
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocompleteShowAutocomplete_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocompleteShowAutocomplete.Click
        SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False

        If (SplitContainer_ToolboxSourceAndDetails.SplitterDistance > (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)) Then
            SplitContainer_ToolboxSourceAndDetails.SplitterDistance = (SplitContainer_ToolboxSourceAndDetails.Height - g_iDefaultDetailsSplitterDistance)
        End If

        TabControl_Details.SelectTab(0)
    End Sub

    Private Sub ToolStripMenuItem_ListReferences_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ListReferences.Click
        g_ClassTextEditorTools.ListReferences()
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocomplete_DropDownOpening(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocomplete.DropDownOpening
        Dim sLanguage As String

        Select Case (g_ClassTabControl.m_ActiveTab.m_Language)
            Case ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN
                sLanguage = "SourcePawn"
            Case ClassSyntaxTools.ENUM_LANGUAGE_TYPE.AMXMODX
                sLanguage = "AMX Mod X"
            Case ClassSyntaxTools.ENUM_LANGUAGE_TYPE.PAWN
                sLanguage = "Pawn"
            Case Else
                sLanguage = "Unknown"
        End Select


        ToolStripMenuItem_ToolsAutocompleteCurrentMod.Text = String.Format("Current langauge: {0}", sLanguage)
    End Sub
#End Region

#Region "MenuStrip_Build"
    Private Sub ToolStripMenuItem_Build_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Build.Click
        Try
            Dim sSource As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent

            With New ClassDebuggerParser(Me)
                If (.HasDebugPlaceholder(sSource)) Then
                    .CleanupDebugPlaceholder(sSource, g_ClassTabControl.m_ActiveTab.m_Language)
                End If
            End With

            Dim sSourceFile As String = Nothing
            If (Not g_ClassTabControl.m_ActiveTab.m_IsUnsaved AndAlso Not g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                sSourceFile = g_ClassTabControl.m_ActiveTab.m_File
            End If

            Dim sOutputFile As String = ""
            g_ClassTextEditorTools.CompileSource(False, sSource, sOutputFile, If(sSourceFile Is Nothing, Nothing, IO.Path.GetDirectoryName(sSourceFile)), Nothing, Nothing, sSourceFile)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
#End Region

#Region "MenuStrip_Test"
    Private Sub ToolStripMenuItem_Test_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Test.Click
        Try
            Dim sSource As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent

            With New ClassDebuggerParser(Me)
                If (.HasDebugPlaceholder(sSource)) Then
                    .CleanupDebugPlaceholder(sSource, g_ClassTabControl.m_ActiveTab.m_Language)
                End If
            End With

            Dim sSourceFile As String = Nothing
            If (Not g_ClassTabControl.m_ActiveTab.m_IsUnsaved AndAlso Not g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                sSourceFile = g_ClassTabControl.m_ActiveTab.m_File
            End If

            Dim sOutputFile As String = ""
            g_ClassTextEditorTools.CompileSource(True, sSource, sOutputFile, If(sSourceFile Is Nothing, Nothing, IO.Path.GetDirectoryName(sSourceFile)), Nothing, Nothing, sSourceFile)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
#End Region

#Region "MenuStrip_Debug"
    Private Sub ToolStripMenuItem_Debug_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Debug.Click
        Try
            If (g_mFormDebugger Is Nothing OrElse g_mFormDebugger.IsDisposed) Then
                Try
                    g_mFormDebugger = New FormDebugger(Me, g_ClassTabControl.m_ActiveTab)
                    g_mFormDebugger.Show()
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)

                    If (g_mFormDebugger IsNot Nothing AndAlso Not g_mFormDebugger.IsDisposed) Then
                        g_mFormDebugger.Dispose()
                        g_mFormDebugger = Nothing
                    End If
                End Try
            Else
                If (g_mFormDebugger.WindowState = FormWindowState.Minimized) Then
                    ClassTools.ClassForms.FormWindowCommand(g_mFormDebugger, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                End If

                g_mFormDebugger.Activate()
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
#End Region

#Region "MenuStrip_Shell"
    Private Sub ToolStripMenuItem_Shell_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Shell.Click
        Try
            Dim sShell As String = ClassConfigs.m_ActiveConfig.g_sExecuteShell

            For Each mArg In ClassSettings.GetShellArguments(Me, Nothing)
                sShell = sShell.Replace(mArg.g_sMarker, mArg.g_sArgument)
            Next

            Try
                If (String.IsNullOrEmpty(sShell)) Then
                    Throw New ArgumentException("Shell is empty")
                End If

                Shell(sShell, AppWinStyle.NormalFocus)
            Catch ex As Exception
                MessageBox.Show(ex.Message & Environment.NewLine & Environment.NewLine & sShell, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
#End Region

#Region "MenuStrip_Help"
    Private Sub ToolStripMenuItem_HelpAbout_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_HelpAbout.Click
        Dim SB As New StringBuilder
        SB.AppendLine(String.Format("{0} v.{1}", Application.ProductName, Application.ProductVersion))
        SB.AppendLine("Created by Externet (aka Timocop)")
        SB.AppendLine()
        SB.AppendLine("Source and Releases")
        SB.AppendLine("     https://github.com/Timocop/BasicPawn")
        SB.AppendLine()
        SB.AppendLine("Third-Party tools:")
        SB.AppendLine("     SharpDevelop - TextEditor (LGPL-2.1)")
        SB.AppendLine()
        SB.AppendLine("         Authors:")
        SB.AppendLine("         Daniel Grunwald and SharpDevelop Community")
        SB.AppendLine("         https://github.com/icsharpcode/SharpDevelop")
        SB.AppendLine()
        SB.AppendLine("     SSH.NET (MIT)")
        SB.AppendLine()
        SB.AppendLine("         Authors:")
        SB.AppendLine("         Gert Driesen and Community")
        SB.AppendLine("         https://github.com/sshnet/SSH.NET")
        SB.AppendLine()
        SB.AppendLine("     BigInteger (Copyright (c) 2002 Chew Keong TAN)")
        SB.AppendLine()
        SB.AppendLine("         Authors:")
        SB.AppendLine("         Chew Keong TAN")
        SB.AppendLine("         https://www.codeproject.com/Articles/2728/C-BigInteger-Class")
        MessageBox.Show(SB.ToString, "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub ToolStripMenuItem_HelpCheckUpdates_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_HelpCheckUpdates.Click
        With New FormUpdate
            .ShowDialog(Me)
        End With
    End Sub

    Private Sub ToolStripMenuItem_HelpGithub_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_HelpGithub.Click
        Try
            Process.Start("https://github.com/Timocop/BasicPawn")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
#End Region

#Region "MenuStrip_Undo"
    Private Sub ToolStripMenuItem_Undo_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Undo.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.Undo()
    End Sub
#End Region

#Region "MenuStrip_Redo"
    Private Sub ToolStripMenuItem_Redo_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Redo.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.Redo()
    End Sub
#End Region

#Region "MenuStrip_NewUpdate"
    Private Sub ToolStripMenuItem_NewUpdate_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_NewUpdate.Click
        With New FormUpdate
            .ShowDialog(Me)
        End With
    End Sub
#End Region


    Private Sub ToolStripStatusLabel_CurrentConfig_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel_CurrentConfig.Click
        Using i As New FormSettings(Me)
            i.TabControl1.SelectTab(1)
            If (i.ShowDialog(Me) = DialogResult.OK) Then
                UpdateFormConfigText()

                g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, Nothing)
                For j = 0 To g_ClassTabControl.m_TabsCount - 1
                    g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL, g_ClassTabControl.m_Tab(j).m_Identifier)
                Next

                For j = 0 To g_ClassTabControl.m_TabsCount - 1
                    g_ClassTabControl.m_Tab(j).m_TextEditor.Document.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
                    g_ClassTabControl.m_Tab(j).m_TextEditor.Document.TextEditorProperties.IndentationSize = If(ClassSettings.g_iSettingsTabsToSpaces > 0, ClassSettings.g_iSettingsTabsToSpaces, 4)
                    g_ClassTabControl.m_Tab(j).m_TextEditor.Document.TextEditorProperties.ConvertTabsToSpaces = (ClassSettings.g_iSettingsTabsToSpaces > 0)
                    g_ClassTabControl.m_Tab(j).m_TextEditor.Refresh()
                Next

                g_ClassSyntaxTools.UpdateFormColors()
            End If
        End Using
    End Sub

#End Region

#Region "ContextMenuStrip_Tabs"
    Private Sub ContextMenuStrip_Tabs_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip_Tabs.Opening
        Dim mTab As ClassTabControl.SourceTabPage = g_ClassTabControl.GetTabByIdentifier(g_sTabsClipboardIdentifier)

        ToolStripMenuItem_Tabs_Insert.Enabled = (mTab IsNot Nothing)
        ToolStripMenuItem_Tabs_Insert.Text = If(mTab IsNot Nothing, String.Format("Insert ({0})", mTab.m_Title), "Insert")

        Dim mPointTab = g_ClassTabControl.GetTabByCursorPoint()
        If (mPointTab Is Nothing) Then
            Return
        End If

        Dim iPointTabIndex = TabControl_SourceTabs.TabPages.IndexOf(mPointTab)
        If (iPointTabIndex < 0 OrElse iPointTabIndex = g_ClassTabControl.m_ActiveTabIndex) Then
            Return
        End If

        g_ClassTabControl.SelectTab(iPointTabIndex)
    End Sub

    Private Sub ToolStripMenuItem_Tabs_Close_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_Close.Click
        g_ClassTabControl.RemoveTab(g_ClassTabControl.m_ActiveTabIndex, True)
    End Sub

    Private Sub ToolStripMenuItem_Tabs_CloseAllButThis_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_CloseAllButThis.Click
        Dim iActiveIndex As Integer = g_ClassTabControl.m_ActiveTabIndex

        For i = g_ClassTabControl.m_TabsCount - 1 To 0 Step -1
            If (iActiveIndex = i) Then
                Continue For
            End If

            If (Not g_ClassTabControl.RemoveTab(i, True, iActiveIndex)) Then
                Return
            End If
        Next
    End Sub

    Private Sub ToolStripMenuItem_Tabs_CloseAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_CloseAll.Click
        g_ClassTabControl.RemoveAllTabs()
    End Sub

    Private Sub ToolStripMenuItem_Tabs_Cut_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_Cut.Click
        g_sTabsClipboardIdentifier = g_ClassTabControl.m_ActiveTab.m_Identifier
    End Sub

    Private Sub ToolStripMenuItem_Tabs_Insert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_Insert.Click
        Dim iFromIndex As Integer = g_ClassTabControl.GetTabIndexByIdentifier(g_sTabsClipboardIdentifier)
        If (iFromIndex < 0) Then
            Return
        End If

        Dim iToIndex As Integer = g_ClassTabControl.m_ActiveTab.m_Index

        g_ClassTabControl.SwapTabs(iFromIndex, iToIndex)
    End Sub

    Private Sub ToolStripMenuItem_Tabs_OpenFolder_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_OpenFolder.Click
        Try
            If (g_ClassTabControl.m_ActiveTab.m_IsUnsaved OrElse g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                MessageBox.Show("Could not open current folder. Source file does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Process.Start("explorer.exe", String.Format("/select,""{0}""", g_ClassTabControl.m_ActiveTab.m_File))
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Tabs_Popout_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Tabs_Popout.Click
        Try
            If (g_ClassTabControl.m_ActiveTab.m_IsUnsaved OrElse g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                MessageBox.Show("Could not popout tab. Source file does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim sFile As String = g_ClassTabControl.m_ActiveTab.m_File

            If (g_ClassTabControl.m_ActiveTab.RemoveTab(True)) Then
                Process.Start(Application.ExecutablePath, String.Join(" ", {"-newinstance", String.Format("""{0}""", sFile)}))
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
#End Region

    Private Sub ToolStripMenuItem_DebuggerBreakpointInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointInsert.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorInsertBreakpointAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointRemove.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorRemoveBreakpointAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointRemoveAll.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorRemoveAllBreakpoints()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherInsert.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorInsertWatcherAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherRemove.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorRemoveWatcherAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherRemoveAll.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorRemoveAllWatchers()
        End With
    End Sub

    Private Sub ToolStripMenuItem_TabClose_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabClose.Click
        g_ClassTabControl.m_ActiveTab.RemoveTab(True)
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

    Private Sub ToolStripMenuItem_TabOpenInstance_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabOpenInstance.Click
        g_mFormOpenTabFromInstances = New FormOpenTabFromInstances(Me)
        g_mFormOpenTabFromInstances.ShowDialog(Me)
    End Sub

    Private Sub OnMessageReceive(mClassMessage As ClassCrossAppComunication.ClassMessage) Handles g_ClassCrossAppComunication.OnMessageReceive
        Try
            'Just in case we get a message before the controls have been created
            If (Not g_bFormPostCreate) Then
                Return
            End If

            Select Case (mClassMessage.m_MessageName)
                Case COMARG_OPEN_FILE_BY_PID
                    Dim iPID As Integer = CInt(mClassMessage.m_Messages(0))
                    Dim sFile As String = mClassMessage.m_Messages(1)

                    If (iPID <> Process.GetCurrentProcess.Id) Then
                        Return
                    End If

                    ClassThread.ExecAsync(Me, Sub()
                                                  Dim mTab = g_ClassTabControl.AddTab()
                                                  mTab.OpenFileTab(sFile)
                                                  mTab.SelectTab(500)

                                                  If (Me.WindowState = FormWindowState.Minimized) Then
                                                      ClassTools.ClassForms.FormWindowCommand(Me, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                                                  End If

                                                  Me.Activate()
                                              End Sub)

                Case COMARG_REQUEST_TABS
                    Dim sCallerIdentifier As String = mClassMessage.m_Messages(0)

                    For i = 0 To g_ClassTabControl.m_TabsCount - 1
                        Dim iPID As Integer = Process.GetCurrentProcess.Id
                        Dim sProcessName As String = Process.GetCurrentProcess.ProcessName
                        Dim sTabIdentifier As String = g_ClassTabControl.m_Tab(i).m_Identifier
                        Dim iTabIndex As Integer = g_ClassTabControl.m_Tab(i).m_Index
                        Dim sTabFile As String = g_ClassTabControl.m_Tab(i).m_File

                        g_ClassCrossAppComunication.SendMessage(New ClassCrossAppComunication.ClassMessage(COMARG_REQUEST_TABS_ANSWER, CStr(iPID), sProcessName, sTabIdentifier, CStr(iTabIndex), sTabFile, sCallerIdentifier), False)
                    Next

                Case COMARG_REQUEST_TABS_ANSWER
                    Dim iPID As Integer = CInt(mClassMessage.m_Messages(0))
                    Dim sProcessName As String = mClassMessage.m_Messages(1)
                    Dim sTabIdentifier As String = mClassMessage.m_Messages(2)
                    Dim iTabIndex As Integer = CInt(mClassMessage.m_Messages(3))
                    Dim sTabFile As String = mClassMessage.m_Messages(4)
                    Dim sCallerIdentifier As String = mClassMessage.m_Messages(5)

                    If (g_mFormOpenTabFromInstances IsNot Nothing AndAlso Not g_mFormOpenTabFromInstances.IsDisposed) Then
                        g_mFormOpenTabFromInstances.AddListViewItem(sTabIdentifier, iTabIndex, sTabFile, sProcessName, iPID, sCallerIdentifier)
                    End If

                Case COMARG_CLOSE_TAB
                    Dim iPID As Integer = CInt(mClassMessage.m_Messages(0))
                    Dim sTabIdentifier As String = mClassMessage.m_Messages(1)
                    Dim sFile As String = mClassMessage.m_Messages(2)
                    Dim bCloseAppWhenEmpty As Boolean = CBool(mClassMessage.m_Messages(3))

                    If (iPID <> Process.GetCurrentProcess.Id) Then
                        Return
                    End If

                    ClassThread.ExecAsync(Me, Sub()
                                                  Dim mTab = g_ClassTabControl.GetTabByIdentifier(sTabIdentifier)
                                                  If (mTab Is Nothing) Then
                                                      Return
                                                  End If

                                                  If (Not String.IsNullOrEmpty(sFile) AndAlso sFile <> mTab.m_File) Then
                                                      Return
                                                  End If

                                                  mTab.RemoveTab(True, g_ClassTabControl.m_ActiveTabIndex)

                                                  If (bCloseAppWhenEmpty AndAlso g_ClassTabControl.m_TabsCount = 1 AndAlso g_ClassTabControl.m_ActiveTab.m_IsUnsaved) Then
                                                      Me.Close()
                                                  End If
                                              End Sub)

                Case COMARG_SHOW_PING_FLASH
                    Dim iPID As Integer = CInt(mClassMessage.m_Messages(0))
                    Dim sTabIdentifier As String = mClassMessage.m_Messages(1)

                    If (iPID <> Process.GetCurrentProcess.Id) Then
                        Return
                    End If

                    ClassThread.ExecAsync(Me, Sub()
                                                  If (Not String.IsNullOrEmpty(sTabIdentifier)) Then
                                                      Dim mTab = g_ClassTabControl.GetTabByIdentifier(sTabIdentifier)
                                                      If (mTab IsNot Nothing) Then
                                                          If (Not mTab.m_IsActive) Then
                                                              mTab.SelectTab()
                                                          End If
                                                      End If
                                                  End If

                                                  If (Me.WindowState = FormWindowState.Minimized) Then
                                                      ClassTools.ClassForms.FormWindowCommand(Me, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                                                  End If

                                                  Me.Activate()

                                                  ShowPingFlash()
                                              End Sub)
            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Public Sub SaveViews()
        If (String.IsNullOrEmpty(Me.Name)) Then
            Return
        End If

        Using mStream = ClassFileStreamWait.Create(ClassSettings.g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                mIni.WriteKeyValue(Me.Name, "ViewToolbox", If(ToolStripMenuItem_ViewToolbox.Checked, "1", "0"))
                mIni.WriteKeyValue(Me.Name, "ViewDetails", If(ToolStripMenuItem_ViewDetails.Checked, "1", "0"))
                mIni.WriteKeyValue(Me.Name, "ViewMinimap", If(ToolStripMenuItem_ViewMinimap.Checked, "1", "0"))
                mIni.WriteKeyValue(Me.Name, "ToolboxSize", CStr(SplitContainer_ToolboxAndEditor.SplitterDistance))
                mIni.WriteKeyValue(Me.Name, "DetailsSize", CStr(SplitContainer_ToolboxSourceAndDetails.Height - SplitContainer_ToolboxSourceAndDetails.SplitterDistance))
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
    End Sub

    Public Sub ShowPingFlash()
        'Wine doesnt support alpha rendering
        If (ClassTools.ClassOperatingSystem.GetWineVersion() IsNot Nothing) Then
            Return
        End If

        g_mPingFlashPanel.m_Opacity = 50
        g_mPingFlashPanel.Visible = True

        Timer_PingFlash.Start()
    End Sub

    Private Sub Timer_PingFlash_Tick(sender As Object, e As EventArgs) Handles Timer_PingFlash.Tick
        If (g_mPingFlashPanel Is Nothing OrElse g_mPingFlashPanel.IsDisposed) Then
            Return
        End If

        g_mPingFlashPanel.m_Opacity -= 10

        If (g_mPingFlashPanel.m_Opacity > 0) Then
            Return
        End If

        g_mPingFlashPanel.Visible = False
        Timer_PingFlash.Stop()
    End Sub

    Private Sub FormMain_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub CleanUp()
        g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnPluginEndPost())

        If (g_mFormOpenTabFromInstances IsNot Nothing AndAlso Not g_mFormOpenTabFromInstances.IsDisposed) Then
            g_mFormOpenTabFromInstances.Close()
            g_mFormOpenTabFromInstances.Dispose()
            g_mFormOpenTabFromInstances = Nothing
        End If

        g_ClassSyntaxUpdater.StopThread()
        g_ClassAutocompleteUpdater.StopUpdate()
        g_mUCObjectBrowser.StopUpdate()

        For i = 0 To ClassSyntaxTools.g_SyntaxFiles.Length - 1
            If (Not String.IsNullOrEmpty(ClassSyntaxTools.g_SyntaxFiles(i).sFile) AndAlso IO.File.Exists(ClassSyntaxTools.g_SyntaxFiles(i).sFile)) Then
                IO.File.Delete(ClassSyntaxTools.g_SyntaxFiles(i).sFile)
            End If

            If (Not String.IsNullOrEmpty(ClassSyntaxTools.g_SyntaxFiles(i).sFolder) AndAlso IO.Directory.Exists(ClassSyntaxTools.g_SyntaxFiles(i).sFolder)) Then
                Try
                    'Still errors...
                    IO.Directory.Delete(ClassSyntaxTools.g_SyntaxFiles(i).sFolder, True)
                Catch ex As Exception
                End Try
            End If
        Next

        If (g_ClassCrossAppComunication IsNot Nothing) Then
            g_ClassCrossAppComunication.Dispose()
            g_ClassCrossAppComunication = Nothing
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

#Region "Drag & Drop TabControl Tabs"
    Private Sub TabControl_SourceTabs_MouseDown(sender As Object, e As MouseEventArgs) Handles TabControl_SourceTabs.MouseDown
        g_mTabControlDragTab = g_ClassTabControl.GetTabByCursorPoint()
    End Sub

    Private Sub TabControl_SourceTabs_MouseUp(sender As Object, e As MouseEventArgs) Handles TabControl_SourceTabs.MouseUp
        g_mTabControlDragTab = Nothing
    End Sub

    Private Sub TabControl_SourceTabs_MouseMove(sender As Object, e As MouseEventArgs) Handles TabControl_SourceTabs.MouseMove
        Try
            If (g_mTabControlDragTab Is Nothing OrElse e.Button <> MouseButtons.Left) Then
                Return
            End If

            TabControl_SourceTabs.DoDragDrop(g_mTabControlDragTab, DragDropEffects.Move)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub TabControl_SourceTabs_DragOver(sender As Object, e As DragEventArgs) Handles TabControl_SourceTabs.DragOver
        Try
            If (g_mTabControlDragPoint = Cursor.Position) Then
                Return
            End If

            Dim mDragTab = DirectCast(e.Data.GetData(GetType(ClassTabControl.SourceTabPage)), ClassTabControl.SourceTabPage)
            If (mDragTab Is Nothing) Then
                Return
            End If

            Dim mPointTab = g_ClassTabControl.GetTabByCursorPoint()
            If (mPointTab Is Nothing) Then
                Return
            End If

            If (mDragTab IsNot g_mTabControlDragTab) Then
                Return
            End If

            If (mDragTab Is mPointTab) Then
                Return
            End If

            Dim iDragIndex = TabControl_SourceTabs.TabPages.IndexOf(mDragTab)
            Dim iPointIndex = TabControl_SourceTabs.TabPages.IndexOf(mPointTab)
            If (iDragIndex < 0 OrElse iPointIndex < 0) Then
                Return
            End If

            e.Effect = DragDropEffects.Move
            g_ClassTabControl.SwapTabs(iDragIndex, iPointIndex)

            g_mTabControlDragPoint = Cursor.Position
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
#End Region
End Class
