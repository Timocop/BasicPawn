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



Public Class FormSettings
    Private g_mFormMain As FormMain

    Private g_lRestoreConfigs As New List(Of ClassConfigs.STRUC_CONFIG_ITEM)
    Private g_bRestoreConfigs As Boolean = False
    Private g_bIgnoreChange As Boolean = False

    Public Sub New(f As FormMain)
        g_bIgnoreChange = True

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormMain = f

        Me.Size = Me.MinimumSize

        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)

        g_bIgnoreChange = False
    End Sub

    Private Sub Button_ConfigAdd_Click(sender As Object, e As EventArgs) Handles Button_ConfigAdd.Click
        Dim sNewName As String = TextBox_ConfigName.Text

        If (String.IsNullOrEmpty(sNewName) OrElse sNewName = ClassConfigs.m_DefaultConfig.GetName OrElse sNewName.IndexOfAny(IO.Path.GetInvalidFileNameChars) > -1 OrElse sNewName.IndexOfAny(IO.Path.GetInvalidPathChars) > -1) Then
            MessageBox.Show("Invalid config name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If (ClassConfigs.LoadConfig(sNewName) IsNot Nothing) Then
            MessageBox.Show("This config name is already used!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ListBox_Configs.Items.Remove(sNewName)
        ListBox_Configs.Items.Add(sNewName)
        ClassConfigs.SaveConfig(New ClassConfigs.STRUC_CONFIG_ITEM(sNewName))

        MarkChanged()

        g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As BasicPawnPluginInterface.IPluginInterface) j.OnConfigChanged())
    End Sub

    Private Sub Button_ConfigRemove_Click(sender As Object, e As EventArgs) Handles Button_ConfigRemove.Click
        Try
            Dim sName As String = TextBox_ConfigName.Text

            If (ClassConfigs.RemoveConfig(sName)) Then
                ListBox_Configs.Items.Remove(sName)

                MarkChanged()

                g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As BasicPawnPluginInterface.IPluginInterface) j.OnConfigChanged())
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Updates the configs in the ListBox
    ''' </summary>
    Private Sub UpdateList()
        ListBox_Configs.BeginUpdate()
        ListBox_Configs.Items.Clear()

        For Each mConfig As ClassConfigs.STRUC_CONFIG_ITEM In ClassConfigs.GetConfigs(True)
            ListBox_Configs.Items.Add(mConfig.GetName)
        Next

        ListBox_Configs.EndUpdate()
    End Sub

    Private Sub ListBox_Configs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox_Configs.SelectedIndexChanged
        Try
            If (ListBox_Configs.SelectedItems.Count < 1) Then
                GroupBox_ConfigSettings.Enabled = False
                Return
            End If

            Dim sName As String = ListBox_Configs.SelectedItems(0).ToString

            If (sName = ClassConfigs.m_DefaultConfig.GetName) Then
                g_bIgnoreChange = True

                Button_SaveConfig.Enabled = False
                GroupBox_ConfigSettings.Enabled = False
                GroupBox_ConfigSettings.Visible = False

                'General
                TextBox_ConfigName.Text = sName
                RadioButton_ConfigSettingAutomatic.Checked = True
                TextBox_CompilerPath.Text = ""
                TextBox_IncludeFolder.Text = ""
                TextBox_OutputFolder.Text = ""
                CheckBox_ConfigIsDefault.Checked = False
                'Debugging
                TextBox_GameFolder.Text = ""
                TextBox_SourceModFolder.Text = ""
                'Misc
                TextBox_Shell.Text = ""
                TextBox_SyntaxPath.Text = ""

                g_bIgnoreChange = False

                'ResetChanged()
                Return
            End If

            g_bIgnoreChange = True
            Button_SaveConfig.Enabled = True
            GroupBox_ConfigSettings.Enabled = True
            GroupBox_ConfigSettings.Visible = True

            'General
            TextBox_ConfigName.Text = sName
            RadioButton_ConfigSettingAutomatic.Checked = True
            TextBox_CompilerPath.Text = ""
            TextBox_IncludeFolder.Text = ""
            TextBox_OutputFolder.Text = ""
            CheckBox_ConfigIsDefault.Checked = False
            'Debugging
            TextBox_GameFolder.Text = ""
            TextBox_SourceModFolder.Text = ""
            'Misc
            TextBox_Shell.Text = ""
            TextBox_SyntaxPath.Text = ""

            g_bIgnoreChange = False

            Dim mConfig As ClassConfigs.STRUC_CONFIG_ITEM = ClassConfigs.LoadConfig(sName)

            If (mConfig Is Nothing) Then
                MessageBox.Show("Current config not found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            g_bIgnoreChange = True
            'General
            RadioButton_ConfigSettingAutomatic.Checked = True
            RadioButton_ConfigSettingManual.Checked = (mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.CONFIG)
            TextBox_CompilerPath.Text = mConfig.g_sCompilerPath
            TextBox_IncludeFolder.Text = mConfig.g_sIncludeFolders
            TextBox_OutputFolder.Text = mConfig.g_sOutputFolder
            CheckBox_ConfigIsDefault.Checked = mConfig.g_bAutoload
            'Debugging
            TextBox_GameFolder.Text = mConfig.g_sDebugGameFolder
            TextBox_SourceModFolder.Text = mConfig.g_sDebugSourceModFolder
            'Misc
            TextBox_Shell.Text = mConfig.g_sExecuteShell
            TextBox_SyntaxPath.Text = mConfig.g_sSyntaxHighlightingPath
            g_bIgnoreChange = False

            'ResetChanged()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_Compiler_Click(sender As Object, e As EventArgs) Handles Button_Compiler.Click
        Using i As New OpenFileDialog
            i.Filter = "SourcePawn Compiler|spcomp.exe|AMX Mod X Compiler|amxxpc.exe|Small Compiler|sc.exe|Pawn Compiler|pawncc.exe|Executables|*.exe"
            i.FileName = TextBox_CompilerPath.Text

            If (i.ShowDialog = DialogResult.OK) Then
                TextBox_CompilerPath.Text = i.FileName
            End If
        End Using
    End Sub

    Private Sub Button_OutputFolder_Click(sender As Object, e As EventArgs) Handles Button_OutputFolder.Click
        Using i As New FolderBrowserDialog
            If (String.IsNullOrEmpty(TextBox_OutputFolder.Text) AndAlso Not String.IsNullOrEmpty(TextBox_CompilerPath.Text) AndAlso IO.File.Exists(TextBox_CompilerPath.Text)) Then
                i.SelectedPath = IO.Path.GetDirectoryName(TextBox_CompilerPath.Text)
            Else
                i.SelectedPath = TextBox_OutputFolder.Text
            End If

            If (i.ShowDialog = DialogResult.OK) Then
                TextBox_OutputFolder.Text = i.SelectedPath
            End If
        End Using
    End Sub

    Private Sub Button_IncludeFolder_Click(sender As Object, e As EventArgs) Handles Button_IncludeFolder.Click
        Using i As New FolderBrowserDialog
            If (String.IsNullOrEmpty(TextBox_IncludeFolder.Text) AndAlso Not String.IsNullOrEmpty(TextBox_CompilerPath.Text) AndAlso IO.File.Exists(TextBox_CompilerPath.Text)) Then
                i.SelectedPath = IO.Path.GetDirectoryName(TextBox_CompilerPath.Text)
            Else
                i.SelectedPath = TextBox_IncludeFolder.Text.Split(";"c)(0)
            End If

            If (i.ShowDialog = DialogResult.OK) Then
                If (String.IsNullOrEmpty(TextBox_IncludeFolder.Text)) Then
                    TextBox_IncludeFolder.Text = i.SelectedPath
                Else
                    Select Case MessageBox.Show("Replace already existing include paths with this one? Otherwise the selected path will be addded to other already existing include paths.", "Replace or add include paths", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        Case DialogResult.Yes
                            TextBox_IncludeFolder.Text = i.SelectedPath
                        Case Else
                            TextBox_IncludeFolder.Text &= ";"c & i.SelectedPath
                    End Select
                End If
            End If
        End Using
    End Sub

    Private Sub Button_GameFolder_Click(sender As Object, e As EventArgs) Handles Button_GameFolder.Click
        Using i As New FolderBrowserDialog
            If (i.ShowDialog = DialogResult.OK) Then
                Dim sGameConfig As String = IO.Path.Combine(i.SelectedPath, "gameinfo.txt")

                If (Not IO.File.Exists(sGameConfig)) Then
                    MessageBox.Show("Invalid game directory! Game info not found!", "Invalid game directory", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                TextBox_GameFolder.Text = i.SelectedPath
            End If
        End Using
    End Sub

    Private Sub Button_SourceModFolder_Click(sender As Object, e As EventArgs) Handles Button_SourceModFolder.Click
        Using i As New FolderBrowserDialog
            If (i.ShowDialog = DialogResult.OK) Then
                Dim sSourceModBin As String = IO.Path.Combine(i.SelectedPath, "bin\sourcemod_mm.dll")

                If (Not IO.File.Exists(sSourceModBin)) Then
                    MessageBox.Show("Invalid SourceMod directory! SourceMod not found!", "Invalid SourceMod directory", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                TextBox_SourceModFolder.Text = i.SelectedPath
            End If
        End Using
    End Sub

    Private Sub Button_SaveConfig_Click(sender As Object, e As EventArgs) Handles Button_SaveConfig.Click
        Try
            If (ListBox_Configs.SelectedItems.Count < 1) Then
                Return
            End If

            Dim sName As String = ListBox_Configs.SelectedItems(0).ToString

            Dim mConfig As ClassConfigs.STRUC_CONFIG_ITEM = ClassConfigs.LoadConfig(sName)
            If (mConfig Is Nothing) Then
                MessageBox.Show("Current config not found or default config!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If (CheckBox_ConfigIsDefault.Checked) Then
                For Each mTmpConfig As ClassConfigs.STRUC_CONFIG_ITEM In ClassConfigs.GetConfigs(False)
                    If (mTmpConfig.g_bAutoload) Then
                        mTmpConfig.g_bAutoload = False
                        mTmpConfig.SaveConfig()
                    End If
                Next
            End If

            ClassConfigs.SaveConfig(New ClassConfigs.STRUC_CONFIG_ITEM(sName,
                                                                        If(RadioButton_ConfigSettingManual.Checked, ClassSettings.ENUM_COMPILING_TYPE.CONFIG, ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC),
                                                                        TextBox_IncludeFolder.Text,
                                                                        TextBox_CompilerPath.Text,
                                                                        TextBox_OutputFolder.Text,
                                                                        CheckBox_ConfigIsDefault.Checked,
                                                                        TextBox_GameFolder.Text,
                                                                        TextBox_SourceModFolder.Text,
                                                                        TextBox_Shell.Text,
                                                                        TextBox_SyntaxPath.Text))

            g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As BasicPawnPluginInterface.IPluginInterface) j.OnConfigChanged())
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub














#Region "Load/Save"
    Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'List all configs
        UpdateList()

        'Get all settings
        ClassSettings.LoadSettings()

        'General
        CheckBox_AlwaysNewInstance.Checked = ClassSettings.g_iSettingsAlwaysOpenNewInstance
        CheckBox_AutoShowStartPage.Checked = ClassSettings.g_iSettingsAutoShowStartPage
        'Text Editor
        Label_Font.Text = New FontConverter().ConvertToInvariantString(ClassSettings.g_iSettingsTextEditorFont)
        CheckBox_InvertedColors.Checked = ClassSettings.g_iSettingsInvertColors
        'Syntax Highligting
        CheckBox_DoubleClickMark.Checked = ClassSettings.g_iSettingsDoubleClickMark
        CheckBox_AutoMark.Checked = ClassSettings.g_iSettingsAutoMark
        'Autocomplete
        CheckBox_AlwaysLoadDefaultIncludes.Checked = ClassSettings.g_iSettingsAlwaysLoadDefaultIncludes
        CheckBox_OnScreenIntelliSense.Checked = ClassSettings.g_iSettingsEnableToolTip
        CheckBox_CommentsMethodIntelliSense.Checked = ClassSettings.g_iSettingsToolTipMethodComments
        CheckBox_CommentsAutocompleteIntelliSense.Checked = ClassSettings.g_iSettingsToolTipAutocompleteComments
        CheckBox_WindowsToolTipPopup.Checked = ClassSettings.g_iSettingsUseWindowsToolTip
        CheckBox_WindowsToolTipAnimations.Checked = ClassSettings.g_iSettingsUseWindowsToolTipAnimations
        CheckBox_FullAutcompleteMethods.Checked = ClassSettings.g_iSettingsFullMethodAutocomplete
        CheckBox_FullAutocompleteReTagging.Checked = ClassSettings.g_iSettingsFullEnumAutocomplete
        CheckBox_CaseSensitive.Checked = ClassSettings.g_iSettingsAutocompleteCaseSensitive
        CheckBox_CurrentSourceVarAutocomplete.Checked = ClassSettings.g_iSettingsVarAutocompleteCurrentSourceOnly
        CheckBox_VarAutocompleteShowObjectBrowser.Checked = ClassSettings.g_iSettingsVarAutocompleteShowObjectBrowser
        CheckBox_SwitchTabToAutocomplete.Checked = ClassSettings.g_iSettingsSwitchTabToAutocomplete
        'Debugger
        CheckBox_CatchExceptions.Checked = ClassSettings.g_iSettingsDebuggerCatchExceptions
        CheckBox_EntitiesEnableColor.Checked = ClassSettings.g_iSettingsDebuggerEntitiesEnableAutoScroll
        CheckBox_EntitiesEnableShowNewEnts.Checked = ClassSettings.g_iSettingsDebuggerEntitiesEnableAutoScroll

        'Get restore-point configs 
        For Each mConfig As ClassConfigs.STRUC_CONFIG_ITEM In ClassConfigs.GetConfigs(False)
            g_lRestoreConfigs.Add(mConfig)
        Next
        g_bRestoreConfigs = True

        'Get current config 
        TextBox_ConfigName.Text = ClassConfigs.m_ActiveConfig.GetName

        Dim i As Integer = ListBox_Configs.FindStringExact(ClassConfigs.m_ActiveConfig.GetName)
        If (i > -1) Then
            ListBox_Configs.SetSelected(i, True)
        End If

        If (Not ClassConfigs.m_ActiveConfig.ConfigExist AndAlso Not ClassConfigs.m_ActiveConfig.IsDefault) Then
            MessageBox.Show("Current config not found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If


        'List plugins
        Dim lListViewItems As New List(Of ListViewItem)
        For Each pluginInfo In g_mFormMain.g_ClassPluginController.m_Plugins
            lListViewItems.Add(New ListViewItem(New String() {
                                                    IO.Path.GetFileName(pluginInfo.sFile),
                                                    pluginInfo.mPluginInformation.sName,
                                                    pluginInfo.mPluginInformation.sAuthor,
                                                    pluginInfo.mPluginInformation.sDescription,
                                                    pluginInfo.mPluginInformation.sVersion,
                                                    pluginInfo.mPluginInformation.sURL
                                                }))
        Next
        ListView_Plugins.Items.Clear()
        ListView_Plugins.Items.AddRange(lListViewItems.ToArray)
        ListView_Plugins.AutoResizeColumns(If(ListView_Plugins.Items.Count > 0, ColumnHeaderAutoResizeStyle.ColumnContent, ColumnHeaderAutoResizeStyle.HeaderSize))

        'Fill DatabaseViewer
        DatabaseViewer.FillFromDatabase()

        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub Button_Apply_Click(sender As Object, e As EventArgs) Handles Button_Apply.Click
        g_bRestoreConfigs = False

        If (ListBox_Configs.SelectedItems.Count > 0) Then
            Dim sName As String = ListBox_Configs.SelectedItems(0).ToString

            ClassConfigs.m_ActiveConfig = ClassConfigs.LoadConfig(sName)
        End If

        'General
        ClassSettings.g_iSettingsAlwaysOpenNewInstance = CheckBox_AlwaysNewInstance.Checked
        ClassSettings.g_iSettingsAutoShowStartPage = CheckBox_AutoShowStartPage.Checked
        'Text Editor
        ClassSettings.g_iSettingsTextEditorFont = CType(New FontConverter().ConvertFromInvariantString(Label_Font.Text), Font)
        ClassSettings.g_iSettingsInvertColors = CheckBox_InvertedColors.Checked
        'Syntax Highligting
        ClassSettings.g_iSettingsDoubleClickMark = CheckBox_DoubleClickMark.Checked
        ClassSettings.g_iSettingsAutoMark = CheckBox_AutoMark.Checked
        'Autocomplete
        ClassSettings.g_iSettingsAlwaysLoadDefaultIncludes = CheckBox_AlwaysLoadDefaultIncludes.Checked
        ClassSettings.g_iSettingsEnableToolTip = CheckBox_OnScreenIntelliSense.Checked
        ClassSettings.g_iSettingsToolTipMethodComments = CheckBox_CommentsMethodIntelliSense.Checked
        ClassSettings.g_iSettingsToolTipAutocompleteComments = CheckBox_CommentsAutocompleteIntelliSense.Checked
        ClassSettings.g_iSettingsUseWindowsToolTip = CheckBox_WindowsToolTipPopup.Checked
        ClassSettings.g_iSettingsUseWindowsToolTipAnimations = CheckBox_WindowsToolTipAnimations.Checked
        ClassSettings.g_iSettingsFullMethodAutocomplete = CheckBox_FullAutcompleteMethods.Checked
        ClassSettings.g_iSettingsFullEnumAutocomplete = CheckBox_FullAutocompleteReTagging.Checked
        ClassSettings.g_iSettingsAutocompleteCaseSensitive = CheckBox_CaseSensitive.Checked
        ClassSettings.g_iSettingsVarAutocompleteCurrentSourceOnly = CheckBox_CurrentSourceVarAutocomplete.Checked
        ClassSettings.g_iSettingsVarAutocompleteShowObjectBrowser = CheckBox_VarAutocompleteShowObjectBrowser.Checked
        ClassSettings.g_iSettingsSwitchTabToAutocomplete = CheckBox_SwitchTabToAutocomplete.Checked
        'Debugger
        ClassSettings.g_iSettingsDebuggerCatchExceptions = CheckBox_CatchExceptions.Checked
        ClassSettings.g_iSettingsDebuggerEntitiesEnableColoring = CheckBox_EntitiesEnableColor.Checked
        ClassSettings.g_iSettingsDebuggerEntitiesEnableAutoScroll = CheckBox_EntitiesEnableShowNewEnts.Checked

        ClassSettings.SaveSettings()


        g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As BasicPawnPluginInterface.IPluginInterface) j.OnSettingsChanged())

        Me.Close()
    End Sub
#End Region





    Private Sub Button_ConfigCopy_Click(sender As Object, e As EventArgs) Handles Button_ConfigCopy.Click
        If (ListBox_Configs.SelectedItems.Count < 1) Then
            Return
        End If

        Dim sName As String = ListBox_Configs.SelectedItems(0).ToString

        Dim mConfig As ClassConfigs.STRUC_CONFIG_ITEM = ClassConfigs.LoadConfig(sName)
        If (mConfig Is Nothing) Then
            MessageBox.Show("Current config not found or default config!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        mConfig.SetName(String.Format("{0} {1}", mConfig.GetName, Guid.NewGuid.ToString))

        If (Not mConfig.SaveConfig) Then
            MessageBox.Show("Failed to save copy!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ListBox_Configs.Items.Add(mConfig.GetName)

        Dim i As Integer = ListBox_Configs.FindStringExact(mConfig.GetName)
        If (i > -1) Then
            ListBox_Configs.SetSelected(i, True)
        End If

        MarkChanged()

        g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As BasicPawnPluginInterface.IPluginInterface) j.OnConfigChanged())
    End Sub

    Private Sub Button_ConfigRename_Click(sender As Object, e As EventArgs) Handles Button_ConfigRename.Click
        If (ListBox_Configs.SelectedItems.Count < 1) Then
            Return
        End If

        Dim sName As String = ListBox_Configs.SelectedItems(0).ToString
        Dim sNewName As String = TextBox_ConfigName.Text

        If (String.IsNullOrEmpty(sNewName) OrElse ClassConfigs.m_DefaultConfig.GetName = sNewName OrElse sNewName.IndexOfAny(IO.Path.GetInvalidFileNameChars) > -1 OrElse sNewName.IndexOfAny(IO.Path.GetInvalidPathChars) > -1) Then
            MessageBox.Show("Invalid config name!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim mConfig As ClassConfigs.STRUC_CONFIG_ITEM = ClassConfigs.LoadConfig(sName)
        If (mConfig Is Nothing) Then
            MessageBox.Show("Current config not found or default config!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim mNewConfig As ClassConfigs.STRUC_CONFIG_ITEM = ClassConfigs.LoadConfig(sNewName)
        If (mNewConfig IsNot Nothing) Then
            Select Case (MessageBox.Show("This config name is already used! Overwrite config?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                Case DialogResult.No
                    Return
            End Select
        End If

        mConfig.RemoveConfig()
        mConfig.SetName(sNewName)
        mConfig.SaveConfig()

        ListBox_Configs.Items.Remove(sName)
        ListBox_Configs.Items.Remove(sNewName)
        ListBox_Configs.Items.Add(sNewName)

        Dim i As Integer = ListBox_Configs.FindStringExact(sNewName)
        If (i > -1) Then
            ListBox_Configs.SetSelected(i, True)
        End If

        MarkChanged()

        g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As BasicPawnPluginInterface.IPluginInterface) j.OnConfigChanged())
    End Sub

    Private Sub Button_Font_Click(sender As Object, e As EventArgs) Handles Button_Font.Click
        Using i As New FontDialog()
            i.Font = CType(New FontConverter().ConvertFromInvariantString(Label_Font.Text), Font)

            If (i.ShowDialog = DialogResult.OK) Then
                Label_Font.Text = New FontConverter().ConvertToInvariantString(i.Font)
            End If
        End Using
    End Sub

    Private Sub LinkLabel_ShowShellArguments_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_ShowShellArguments.LinkClicked
        Try
            Dim SB As New Text.StringBuilder
            SB.AppendLine("All available shell arguments:")
            SB.AppendLine()

            For Each iItem In ClassSettings.GetShellArguments(g_mFormMain)
                SB.AppendLine(String.Format("{0} - {1}", iItem.g_sMarker, iItem.g_sArgumentName))
            Next

            MessageBox.Show(SB.ToString, "Information", MessageBoxButtons.OK)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_SyntaxPath_Click(sender As Object, e As EventArgs) Handles Button_SyntaxPath.Click
        Using i As New OpenFileDialog
            i.Filter = "Syntax highlighting XSHD file|*.xml"
            i.FileName = TextBox_SyntaxPath.Text

            If (i.ShowDialog = DialogResult.OK) Then
                TextBox_SyntaxPath.Text = i.FileName
            End If
        End Using
    End Sub

    Private Sub LinkLabel_SyntaxDefault_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_SyntaxDefault.LinkClicked
        TextBox_SyntaxPath.Text = ""
    End Sub

    Private Sub FormSettings_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If (Not g_bRestoreConfigs) Then
            Return
        End If

        For Each mConfig As ClassConfigs.STRUC_CONFIG_ITEM In ClassConfigs.GetConfigs(False)
            mConfig.RemoveConfig()
        Next

        For Each mConfig As ClassConfigs.STRUC_CONFIG_ITEM In g_lRestoreConfigs
            mConfig.SaveConfig()
        Next
    End Sub

    Private Sub RadioButton_ConfigSettingAutomatic_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_ConfigSettingAutomatic.CheckedChanged
        MarkChanged()
    End Sub

    Private Sub RadioButton_ConfigSettingManual_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_ConfigSettingManual.CheckedChanged
        MarkChanged()
    End Sub

    Public Sub MarkChanged()
        If (Not g_bIgnoreChange AndAlso Not TabPage_Configs.Text.EndsWith("*"c)) Then
            TabPage_Configs.Text = TabPage_Configs.Text & "*"
            TabControl1.Refresh()
        End If
    End Sub

    Public Sub ResetChanged()
        If (TabPage_Configs.Text.EndsWith("*"c)) Then
            TabPage_Configs.Text = TabPage_Configs.Text.Trim("*"c)
            TabControl1.Refresh()
        End If
    End Sub

    Private Sub TextBox_CompilerPath_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CompilerPath.TextChanged
        MarkChanged()
    End Sub

    Private Sub TextBox_IncludeFolder_TextChanged(sender As Object, e As EventArgs) Handles TextBox_IncludeFolder.TextChanged
        MarkChanged()
    End Sub

    Private Sub TextBox_OutputFolder_TextChanged(sender As Object, e As EventArgs) Handles TextBox_OutputFolder.TextChanged
        MarkChanged()
    End Sub

    Private Sub CheckBox_ConfigIsDefault_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_ConfigIsDefault.CheckedChanged
        MarkChanged()
    End Sub

    Private Sub TextBox_GameFolder_TextChanged(sender As Object, e As EventArgs) Handles TextBox_GameFolder.TextChanged
        MarkChanged()
    End Sub

    Private Sub TextBox_SourceModFolder_TextChanged(sender As Object, e As EventArgs) Handles TextBox_SourceModFolder.TextChanged
        MarkChanged()
    End Sub

    Private Sub TextBox_Shell_TextChanged(sender As Object, e As EventArgs) Handles TextBox_Shell.TextChanged
        MarkChanged()
    End Sub

    Private Sub TextBox_SyntaxPath_TextChanged(sender As Object, e As EventArgs) Handles TextBox_SyntaxPath.TextChanged
        MarkChanged()
    End Sub

    Private Sub Button_AddDatabaseItem_Click(sender As Object, e As EventArgs) Handles Button_AddDatabaseItem.Click
        Using i As New FormDatabaseInput()
            If (i.ShowDialog = DialogResult.OK) Then
                DatabaseViewer.AddItem(i.m_Name, i.m_Username)

                Dim iItem As New ClassDatabase.STRUC_DATABASE_ITEM(i.m_Name, i.m_Username, i.m_Password)
                iItem.Save()
            End If
        End Using
    End Sub

    Private Sub Button_Refresh_Click(sender As Object, e As EventArgs) Handles Button_Refresh.Click
        DatabaseViewer.FillFromDatabase()
    End Sub
End Class