Imports System.Text.RegularExpressions

Public Class FormSettings
    Public g_fFormMain As FormMain = Nothing
    Private g_sConfigFolder As String = Application.StartupPath & "\configs"
    Private g_sConfigFileExt As String = ".ini"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Size = New Size(0, 0)
    End Sub

    Private Sub Button_ConfigAdd_Click(sender As Object, e As EventArgs) Handles Button_ConfigAdd.Click
        Dim sCurrentConfigName As String = TextBox_ConfigName.Text

        If (String.IsNullOrEmpty(sCurrentConfigName) OrElse sCurrentConfigName.IndexOfAny(IO.Path.GetInvalidFileNameChars) > -1 OrElse sCurrentConfigName.IndexOfAny(IO.Path.GetInvalidPathChars) > -1) Then
            MessageBox.Show("Invalid config name!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim sConfigFile As String = IO.Path.Combine(g_sConfigFolder, sCurrentConfigName & g_sConfigFileExt)

        If (ListBox_Configs.Items.Contains(sCurrentConfigName) OrElse IO.File.Exists(sConfigFile)) Then
            MessageBox.Show("This config name is already used!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ListBox_Configs.Items.Add(sCurrentConfigName)
        IO.File.WriteAllText(sConfigFile, "")
    End Sub

    Private Sub Button_ConfigRemove_Click(sender As Object, e As EventArgs) Handles Button_ConfigRemove.Click
        Dim sCurrentConfigName As String = TextBox_ConfigName.Text

        If (sCurrentConfigName = "Default") Then
            Return
        End If

        If (String.IsNullOrEmpty(sCurrentConfigName)) Then
            MessageBox.Show("Invalid config name!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim sConfigFile As String = IO.Path.Combine(g_sConfigFolder, sCurrentConfigName & g_sConfigFileExt)

        ListBox_Configs.Items.Remove(sCurrentConfigName)
        IO.File.Delete(sConfigFile)
    End Sub

    ''' <summary>
    ''' Updates the configs in the ListBox
    ''' </summary>
    Private Sub UpdateList()
        ListBox_Configs.Items.Clear()
        ListBox_Configs.Items.Add("Default")

        If (Not IO.Directory.Exists(g_sConfigFolder)) Then
            IO.Directory.CreateDirectory(g_sConfigFolder)
        End If

        If (IO.Directory.Exists(g_sConfigFolder)) Then
            For Each sFile As String In IO.Directory.GetFiles(g_sConfigFolder)
                If (IO.Path.GetExtension(sFile) <> g_sConfigFileExt) Then
                    Continue For
                End If

                If (IO.Path.GetFileNameWithoutExtension(sFile) = "Default") Then
                    Continue For
                End If

                ListBox_Configs.Items.Add(IO.Path.GetFileNameWithoutExtension(sFile))
            Next
        End If
    End Sub

    Private Sub ListBox_Configs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox_Configs.SelectedIndexChanged
        If (ListBox_Configs.SelectedItems.Count < 1) Then
            GroupBox_ConfigSettings.Enabled = False
            Return
        End If

        Dim sCurrentConfigName As String = ListBox_Configs.SelectedItems(0)

        If (sCurrentConfigName = "Default") Then
            GroupBox_ConfigSettings.Enabled = False

            TextBox_ConfigName.Text = sCurrentConfigName
            RadioButton_ConfigSettingAutomatic.Checked = True
            TextBox_CompilerPath.Text = ""
            TextBox_IncludeFolder.Text = ""
            TextBox_OutputFolder.Text = ""
            TextBox_Shell.Text = ""

            Return
        End If

        GroupBox_ConfigSettings.Enabled = True

        TextBox_ConfigName.Text = sCurrentConfigName
        RadioButton_ConfigSettingAutomatic.Checked = True
        TextBox_CompilerPath.Text = ""
        TextBox_IncludeFolder.Text = ""
        TextBox_OutputFolder.Text = ""
        TextBox_Shell.Text = ""

        Dim sConfigFile As String = IO.Path.Combine(g_sConfigFolder, sCurrentConfigName & g_sConfigFileExt)

        If (Not IO.File.Exists(sConfigFile)) Then
            MessageBox.Show("Current config not found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim iniFile As New ClassIniFile(sConfigFile)
            RadioButton_ConfigSettingAutomatic.Checked = True
            RadioButton_ConfigSettingManual.Checked = (iniFile.ReadKeyValue("Config", "Type", "0") <> "0")
            TextBox_CompilerPath.Text = iniFile.ReadKeyValue("Config", "CompilerPath", "")
            TextBox_IncludeFolder.Text = iniFile.ReadKeyValue("Config", "IncludeDirectory", "")
            TextBox_OutputFolder.Text = iniFile.ReadKeyValue("Config", "OutputDirectory", "")
            TextBox_Shell.Text = iniFile.ReadKeyValue("Config", "ExecuteShell", "")
            TextBox_GameFolder.Text = iniFile.ReadKeyValue("Config", "DebugGameDirectory", "")
            TextBox_SourceModFolder.Text = iniFile.ReadKeyValue("Config", "DebugSourceModDirectory", "")
        Catch ex As Exception
            ClassExceptionLogManagement.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_Compiler_Click(sender As Object, e As EventArgs) Handles Button_Compiler.Click
        Using i As New OpenFileDialog
            i.Filter = "SourcePawn Compiler|spcomp.exe|Executables|*.exe"
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
                i.SelectedPath = TextBox_IncludeFolder.Text
            End If
            If (i.ShowDialog = DialogResult.OK) Then
                TextBox_IncludeFolder.Text = i.SelectedPath
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

            Dim sCurrentConfigName As String = ListBox_Configs.SelectedItems(0)

            If (String.IsNullOrEmpty(sCurrentConfigName) OrElse sCurrentConfigName = "Default") Then
                Return
            End If

            Dim sConfigFile As String = IO.Path.Combine(g_sConfigFolder, sCurrentConfigName & g_sConfigFileExt)

            Dim iniFile As New ClassIniFile(sConfigFile)
            iniFile.WriteKeyValue("Config", "Type", If(RadioButton_ConfigSettingManual.Checked, "1", "0"))
            iniFile.WriteKeyValue("Config", "CompilerPath", TextBox_CompilerPath.Text)
            iniFile.WriteKeyValue("Config", "IncludeDirectory", TextBox_IncludeFolder.Text)
            iniFile.WriteKeyValue("Config", "OutputDirectory", TextBox_OutputFolder.Text)
            iniFile.WriteKeyValue("Config", "ExecuteShell", TextBox_Shell.Text)
            iniFile.WriteKeyValue("Config", "DebugGameDirectory", TextBox_GameFolder.Text)
            iniFile.WriteKeyValue("Config", "DebugSourceModDirectory", TextBox_SourceModFolder.Text)

            MessageBox.Show("Config saved!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            ClassExceptionLogManagement.WriteToLogMessageBox(ex)
        End Try


    End Sub

    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub














#Region "Load/Save"
    Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If (g_fFormMain IsNot FormMain) Then
            Return
        End If

        'List all configs
        UpdateList()

        'Get all settings
        ClassSettings.LoadSettings()



        CheckBox_OnScreenIntelliSense.Checked = ClassSettings.g_iSettingsEnableToolTip
        CheckBox_CommentsMethodIntelliSense.Checked = ClassSettings.g_iSettingsToolTipMethodComments
        CheckBox_CommentsAutocompleteIntelliSense.Checked = ClassSettings.g_iSettingsToolTipAutocompleteComments
        CheckBox_WindowsToolTipPopup.Checked = ClassSettings.g_iSettingsUseWindowsToolTip
        CheckBox_FullAutcompleteMethods.Checked = ClassSettings.g_iSettingsFullMethodAutocomplete
        CheckBox_FullAutocompleteReTagging.Checked = ClassSettings.g_iSettingsFullEnumAutocomplete
        CheckBox_DetectMethodmapVariables.Checked = ClassSettings.g_iSettingsDetectMethodmapInNames
        CheckBox_CaseSensitive.Checked = ClassSettings.g_iSettingsAutocompleteCaseSensitive

        CheckBox_DoubleClickMark.Checked = ClassSettings.g_iSettingsDoubleClickMark
        CheckBox_AutoMark.Checked = ClassSettings.g_iSettingsAutoMark

        Label_Font.Text = New FontConverter().ConvertToInvariantString(ClassSettings.g_iSettingsTextEditorFont)
        CheckBox_InvertedColors.Checked = ClassSettings.g_iSettingsInvertColors

        CheckBox_CatchExceptions.Checked = ClassSettings.g_iSettingsDebuggerCatchExceptions
        CheckBox_EntitiesEnableColor.Checked = ClassSettings.g_iSettingsDebuggerEntitiesEnableAutoScroll
        CheckBox_EntitiesEnableShowNewEnts.Checked = ClassSettings.g_iSettingsDebuggerEntitiesEnableAutoScroll


        'Get current config
        Dim sCurrentConfigName As String = ClassSettings.g_sConfigName

        If (String.IsNullOrEmpty(sCurrentConfigName)) Then
            Dim i As Integer = ListBox_Configs.FindStringExact("Default")
            If (i > -1) Then
                ListBox_Configs.SetSelected(i, True)
            End If
        Else
            If (Not IO.File.Exists(IO.Path.Combine(g_sConfigFolder, sCurrentConfigName & g_sConfigFileExt))) Then
                MessageBox.Show("Current config not found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                TextBox_ConfigName.Text = sCurrentConfigName

                Dim i As Integer = ListBox_Configs.FindStringExact(sCurrentConfigName)
                If (i > -1) Then
                    ListBox_Configs.SetSelected(i, True)
                End If
            End If
        End If

    End Sub

    Private Sub Button_Apply_Click(sender As Object, e As EventArgs) Handles Button_Apply.Click
        If (ListBox_Configs.SelectedItems.Count > 0) Then
            Dim sCurrentConfigName As String = ListBox_Configs.SelectedItems(0)

            If (String.IsNullOrEmpty(sCurrentConfigName) OrElse sCurrentConfigName = "Default") Then
                ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC
                ClassSettings.g_sConfigName = ""
            Else
                ClassSettings.g_iConfigCompilingType = If(RadioButton_ConfigSettingAutomatic.Checked, ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC, ClassSettings.ENUM_COMPILING_TYPE.CONFIG)
                ClassSettings.g_sConfigName = sCurrentConfigName
                ClassSettings.g_sConfigCompilerPath = TextBox_CompilerPath.Text
                ClassSettings.g_sConfigOpenSourcePawnIncludeFolder = TextBox_IncludeFolder.Text
                ClassSettings.g_sConfigPluginOutputFolder = TextBox_OutputFolder.Text
                ClassSettings.g_sConfigExecuteShell = TextBox_Shell.Text
                ClassSettings.g_sConfigDebugGameFolder = TextBox_GameFolder.Text
                ClassSettings.g_sConfigDebugSourceModFolder = TextBox_SourceModFolder.Text
            End If
        End If



        ClassSettings.g_iSettingsEnableToolTip = CheckBox_OnScreenIntelliSense.Checked
        ClassSettings.g_iSettingsToolTipMethodComments = CheckBox_CommentsMethodIntelliSense.Checked
        ClassSettings.g_iSettingsToolTipAutocompleteComments = CheckBox_CommentsAutocompleteIntelliSense.Checked
        ClassSettings.g_iSettingsUseWindowsToolTip = CheckBox_WindowsToolTipPopup.Checked
        ClassSettings.g_iSettingsFullMethodAutocomplete = CheckBox_FullAutcompleteMethods.Checked
        ClassSettings.g_iSettingsFullEnumAutocomplete = CheckBox_FullAutocompleteReTagging.Checked
        ClassSettings.g_iSettingsDetectMethodmapInNames = CheckBox_DetectMethodmapVariables.Checked
        ClassSettings.g_iSettingsAutocompleteCaseSensitive = CheckBox_CaseSensitive.Checked

        ClassSettings.g_iSettingsDoubleClickMark = CheckBox_DoubleClickMark.Checked
        ClassSettings.g_iSettingsAutoMark = CheckBox_AutoMark.Checked

        ClassSettings.g_iSettingsTextEditorFont = New FontConverter().ConvertFromInvariantString(Label_Font.Text)
        ClassSettings.g_iSettingsInvertColors = CheckBox_InvertedColors.Checked

        ClassSettings.g_iSettingsDebuggerCatchExceptions = CheckBox_CatchExceptions.Checked
        ClassSettings.g_iSettingsDebuggerEntitiesEnableColoring = CheckBox_EntitiesEnableColor.Checked
        ClassSettings.g_iSettingsDebuggerEntitiesEnableAutoScroll = CheckBox_EntitiesEnableShowNewEnts.Checked



        ClassSettings.SaveSettings()
        Me.Close()
    End Sub
#End Region





    Private Sub Button_ConfigCopy_Click(sender As Object, e As EventArgs) Handles Button_ConfigCopy.Click
        If (ListBox_Configs.SelectedItems.Count < 1) Then
            Return
        End If

        Dim sCurrentConfigName As String = ListBox_Configs.SelectedItems(0)

        If (String.IsNullOrEmpty(sCurrentConfigName) OrElse sCurrentConfigName = "Default") Then
            Return
        End If

        Dim sConfigFile As String = IO.Path.Combine(g_sConfigFolder, sCurrentConfigName & g_sConfigFileExt)

        Dim iCounter As Integer = 1
        While True
            Dim sFileName As String = sCurrentConfigName & "(" & iCounter & ")"
            Dim sNewFileConfig As String = IO.Path.Combine(g_sConfigFolder, sFileName & g_sConfigFileExt)

            If (IO.File.Exists(sNewFileConfig)) Then
                iCounter += 1
                Continue While
            End If

            IO.File.Copy(sConfigFile, sNewFileConfig)
            ListBox_Configs.Items.Add(sFileName)

            Dim i As Integer = ListBox_Configs.FindStringExact(sFileName)
            If (i > -1) Then
                ListBox_Configs.SetSelected(i, True)
            End If
            Exit While
        End While
    End Sub

    Private Sub Button_ConfigRename_Click(sender As Object, e As EventArgs) Handles Button_ConfigRename.Click
        If (ListBox_Configs.SelectedItems.Count < 1) Then
            Return
        End If

        Dim sCurrentConfigName As String = ListBox_Configs.SelectedItems(0)

        If (String.IsNullOrEmpty(sCurrentConfigName) OrElse sCurrentConfigName = "Default") Then
            Return
        End If

        Dim sNewConfigName As String = TextBox_ConfigName.Text

        If (String.IsNullOrEmpty(sNewConfigName) OrElse sNewConfigName = "Default" OrElse sNewConfigName.IndexOfAny(IO.Path.GetInvalidFileNameChars) > -1 OrElse sNewConfigName.IndexOfAny(IO.Path.GetInvalidPathChars) > -1) Then
            MessageBox.Show("Invalid config name!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim sConfigFile As String = IO.Path.Combine(g_sConfigFolder, sCurrentConfigName & g_sConfigFileExt)
        Dim sNewConfigFile As String = IO.Path.Combine(g_sConfigFolder, sNewConfigName & g_sConfigFileExt)

        If (ListBox_Configs.Items.Contains(sNewConfigName) OrElse IO.File.Exists(sNewConfigFile)) Then
            MessageBox.Show("This config name is already used!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ListBox_Configs.Items.Remove(sCurrentConfigName)

        ListBox_Configs.Items.Add(sNewConfigName)
        IO.File.Move(sConfigFile, sNewConfigFile)

        Dim i As Integer = ListBox_Configs.FindStringExact(sNewConfigName)
        If (i > -1) Then
            ListBox_Configs.SetSelected(i, True)
        End If
    End Sub

    Private Sub Button_Font_Click(sender As Object, e As EventArgs) Handles Button_Font.Click
        Using i As New FontDialog()
            i.Font = New FontConverter().ConvertFromInvariantString(Label_Font.Text)
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

            For Each iItem In ClassSettings.GetShellArguments
                SB.AppendLine(String.Format("{0} - {1}", iItem.g_sMarker, iItem.g_sArgumentName))
            Next

            MessageBox.Show(SB.ToString, "Information", MessageBoxButtons.OK)
        Catch ex As Exception
            ClassExceptionLogManagement.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class