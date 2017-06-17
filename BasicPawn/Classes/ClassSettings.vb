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


Public Class ClassSettings
#Region "Settings"
    Enum ENUM_AUTOCOMPLETE_SYNTAX
        SP_MIX
        SP_1_6
        SP_1_7
    End Enum

    Public Shared g_iSettingsAutocompleteSyntax As ENUM_AUTOCOMPLETE_SYNTAX = ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX
    Public Shared g_iSettingsDefaultEditorFont As Font = New Font("Consolas", 9, FontStyle.Regular)
    Public Shared g_sSettingsDefaultEditorFont As String = New FontConverter().ConvertToInvariantString(g_iSettingsDefaultEditorFont)

    'Settings
    Public Shared g_sSettingsFile As String = IO.Path.Combine(Application.StartupPath, "settings.ini")
    'General
    Public Shared g_iSettingsAlwaysOpenNewInstance As Boolean = False
    Public Shared g_iSettingsAutoShowStartPage As Boolean = True
    'Text Editor
    Public Shared g_iSettingsTextEditorFont As Font = g_iSettingsDefaultEditorFont
    Public Shared g_iSettingsInvertColors As Boolean = False
    Public Shared g_iSettingsTabsToSpaces As Integer = 0
    'Syntax Highligting
    Public Shared g_iSettingsDoubleClickMark As Boolean = True
    Public Shared g_iSettingsAutoMark As Boolean = True
    'Autocomplete
    Public Shared g_iSettingsAlwaysLoadDefaultIncludes As Boolean = True
    Public Shared g_iSettingsEnableToolTip As Boolean = True
    Public Shared g_iSettingsToolTipMethodComments As Boolean = False
    Public Shared g_iSettingsToolTipAutocompleteComments As Boolean = True
    Public Shared g_iSettingsUseWindowsToolTip As Boolean = False
    Public Shared g_iSettingsUseWindowsToolTipAnimations As Boolean = True
    Public Shared g_iSettingsFullMethodAutocomplete As Boolean = False
    Public Shared g_iSettingsFullEnumAutocomplete As Boolean = False
    Public Shared g_iSettingsAutocompleteCaseSensitive As Boolean = True
    Public Shared g_iSettingsVarAutocompleteCurrentSourceOnly As Boolean = True
    Public Shared g_iSettingsVarAutocompleteShowObjectBrowser As Boolean = False
    Public Shared g_iSettingsSwitchTabToAutocomplete As Boolean = True
    'Debugger
    Public Shared g_iSettingsDebuggerCatchExceptions As Boolean = True
    Public Shared g_iSettingsDebuggerEntitiesEnableColoring As Boolean = True
    Public Shared g_iSettingsDebuggerEntitiesEnableAutoScroll As Boolean = True
#End Region


    Enum ENUM_COMPILING_TYPE
        AUTOMATIC
        CONFIG
    End Enum

    Public Shared Sub SaveSettings()
        Dim initFile As New ClassIniFile(g_sSettingsFile)

        'Settings
        initFile.WriteKeyValue("Editor", "AlwaysOpenNewInstance", If(g_iSettingsAlwaysOpenNewInstance, "1", "0"))
        initFile.WriteKeyValue("Editor", "AutoShowStartPage", If(g_iSettingsAutoShowStartPage, "1", "0"))
        'Text Editor
        initFile.WriteKeyValue("Editor", "TextEditorFont", New FontConverter().ConvertToInvariantString(g_iSettingsTextEditorFont))
        initFile.WriteKeyValue("Editor", "TextEditorInvertColors", If(g_iSettingsInvertColors, "1", "0"))
        initFile.WriteKeyValue("Editor", "TextEditorTabsToSpaces", CStr(g_iSettingsTabsToSpaces))
        'Syntax Highligting
        initFile.WriteKeyValue("Editor", "DoubleClickMark", If(g_iSettingsDoubleClickMark, "1", "0"))
        initFile.WriteKeyValue("Editor", "AutoMark", If(g_iSettingsAutoMark, "1", "0"))
        'Autocomplete
        initFile.WriteKeyValue("Editor", "AlwaysLoadDefaultIncludes", If(g_iSettingsAlwaysLoadDefaultIncludes, "1", "0"))
        initFile.WriteKeyValue("Editor", "AutocompleteToolTip", If(g_iSettingsEnableToolTip, "1", "0"))
        initFile.WriteKeyValue("Editor", "ToolTipMethodComments", If(g_iSettingsToolTipMethodComments, "1", "0"))
        initFile.WriteKeyValue("Editor", "ToolTipAutocompleteComments", If(g_iSettingsToolTipAutocompleteComments, "1", "0"))
        initFile.WriteKeyValue("Editor", "UseWindowsToolTip", If(g_iSettingsUseWindowsToolTip, "1", "0"))
        initFile.WriteKeyValue("Editor", "UseWindowsToolTipAnimations", If(g_iSettingsUseWindowsToolTipAnimations, "1", "0"))
        initFile.WriteKeyValue("Editor", "FullMethodAutocomplete", If(g_iSettingsFullMethodAutocomplete, "1", "0"))
        initFile.WriteKeyValue("Editor", "FullEnumAutocomplete", If(g_iSettingsFullEnumAutocomplete, "1", "0"))
        initFile.WriteKeyValue("Editor", "AutocompleteCaseSensitive", If(g_iSettingsAutocompleteCaseSensitive, "1", "0"))
        initFile.WriteKeyValue("Editor", "VarAutocompleteCurrentSourceOnly", If(g_iSettingsVarAutocompleteCurrentSourceOnly, "1", "0"))
        initFile.WriteKeyValue("Editor", "VarAutocompleteShowObjectBrowser", If(g_iSettingsVarAutocompleteShowObjectBrowser, "1", "0"))
        initFile.WriteKeyValue("Editor", "SwitchTabToAutocomplete", If(g_iSettingsSwitchTabToAutocomplete, "1", "0"))
        'Debugger
        initFile.WriteKeyValue("Debugger", "CatchExceptions", If(g_iSettingsDebuggerCatchExceptions, "1", "0"))
        initFile.WriteKeyValue("Debugger", "EntitiesColoring", If(g_iSettingsDebuggerEntitiesEnableColoring, "1", "0"))
        initFile.WriteKeyValue("Debugger", "EntitiesAutoScroll", If(g_iSettingsDebuggerEntitiesEnableAutoScroll, "1", "0"))

    End Sub

    Public Shared Sub LoadSettings()
        Try
            Dim initFile As New ClassIniFile(g_sSettingsFile)

            'Settings
            g_iSettingsAlwaysOpenNewInstance = (initFile.ReadKeyValue("Editor", "AlwaysOpenNewInstance", "0") <> "0")
            g_iSettingsAutoShowStartPage = (initFile.ReadKeyValue("Editor", "AutoShowStartPage", "1") <> "0")
            'Text Editor
            Dim editorFont As Font = CType(New FontConverter().ConvertFromInvariantString(initFile.ReadKeyValue("Editor", "TextEditorFont", g_sSettingsDefaultEditorFont)), Font)
            If (editorFont IsNot Nothing AndAlso editorFont.Size < 256) Then
                g_iSettingsTextEditorFont = editorFont
            Else
                g_iSettingsTextEditorFont = g_iSettingsDefaultEditorFont
            End If
            g_iSettingsInvertColors = (initFile.ReadKeyValue("Editor", "TextEditorInvertColors", "0") <> "0")
            Dim iTabsToSpaces As Integer = 0
            If (Integer.TryParse(initFile.ReadKeyValue("Editor", "TextEditorTabsToSpaces", "0"), iTabsToSpaces)) Then
                iTabsToSpaces = If(iTabsToSpaces < 0, 0, iTabsToSpaces)
                iTabsToSpaces = If(iTabsToSpaces > 100, 100, iTabsToSpaces)
                g_iSettingsTabsToSpaces = iTabsToSpaces
            End If
            'Syntax Highligting
            g_iSettingsDoubleClickMark = (initFile.ReadKeyValue("Editor", "DoubleClickMark", "1") <> "0")
            g_iSettingsAutoMark = (initFile.ReadKeyValue("Editor", "AutoMark", "1") <> "0")
            'Autocomplete
            g_iSettingsAlwaysLoadDefaultIncludes = (initFile.ReadKeyValue("Editor", "AlwaysLoadDefaultIncludes", "1") <> "0")
            g_iSettingsEnableToolTip = (initFile.ReadKeyValue("Editor", "AutocompleteToolTip", "1") <> "0")
            g_iSettingsToolTipMethodComments = (initFile.ReadKeyValue("Editor", "ToolTipMethodComments", "0") <> "0")
            g_iSettingsToolTipAutocompleteComments = (initFile.ReadKeyValue("Editor", "ToolTipAutocompleteComments", "1") <> "0")
            g_iSettingsUseWindowsToolTip = (initFile.ReadKeyValue("Editor", "UseWindowsToolTip", "0") <> "0")
            g_iSettingsUseWindowsToolTipAnimations = (initFile.ReadKeyValue("Editor", "UseWindowsToolTipAnimations", "1") <> "0")
            g_iSettingsFullMethodAutocomplete = (initFile.ReadKeyValue("Editor", "FullMethodAutocomplete", "0") <> "0")
            g_iSettingsFullEnumAutocomplete = (initFile.ReadKeyValue("Editor", "FullEnumAutocomplete", "0") <> "0")
            g_iSettingsAutocompleteCaseSensitive = (initFile.ReadKeyValue("Editor", "AutocompleteCaseSensitive", "1") <> "0")
            g_iSettingsVarAutocompleteCurrentSourceOnly = (initFile.ReadKeyValue("Editor", "VarAutocompleteCurrentSourceOnly", "1") <> "0")
            g_iSettingsVarAutocompleteShowObjectBrowser = (initFile.ReadKeyValue("Editor", "VarAutocompleteShowObjectBrowser", "0") <> "0")
            g_iSettingsSwitchTabToAutocomplete = (initFile.ReadKeyValue("Editor", "SwitchTabToAutocomplete", "1") <> "0")
            'Debugger
            g_iSettingsDebuggerCatchExceptions = (initFile.ReadKeyValue("Debugger", "CatchExceptions", "1") <> "0")
            g_iSettingsDebuggerEntitiesEnableColoring = (initFile.ReadKeyValue("Debugger", "EntitiesColoring", "1") <> "0")
            g_iSettingsDebuggerEntitiesEnableAutoScroll = (initFile.ReadKeyValue("Debugger", "EntitiesAutoScroll", "1") <> "0")
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Public Class STRUC_SHELL_ARGUMENT_ITEM
        Public g_sMarker As String = ""
        Public g_sArgumentName As String = ""
        Public g_sArgument As String = ""

        Public Sub New(sMarker As String, sArgName As String, sArgument As String)
            g_sMarker = sMarker
            g_sArgumentName = sArgName
            g_sArgument = sArgument
        End Sub
    End Class

    Public Shared Function ConvertSpaces(iLenght As Integer) As String
        If (g_iSettingsTabsToSpaces > 0) Then
            Return New Text.StringBuilder().Insert(0, New String(" "c, g_iSettingsTabsToSpaces), iLenght).ToString
        Else
            Return New Text.StringBuilder().Insert(0, vbTab, iLenght).ToString
        End If
    End Function

    ''' <summary>
    ''' Gets all available shell arguments
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetShellArguments(mFormMain As FormMain) As STRUC_SHELL_ARGUMENT_ITEM()
        'TODO: Add more shell arguments
        Dim sShellList As New List(Of STRUC_SHELL_ARGUMENT_ITEM)

        Dim sFile As String = mFormMain.g_ClassTabControl.m_ActiveTab.m_File

        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%input%", "Current opened source file", sFile))
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%inputfilename%", "Current opened source filename", If(String.IsNullOrEmpty(sFile), "", IO.Path.GetFileNameWithoutExtension(sFile))))
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%inputfolder%", "Current opened source file folder", If(String.IsNullOrEmpty(sFile), "", IO.Path.GetDirectoryName(sFile))))
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%includes%", "Include folders", ClassConfigs.m_ActiveConfig.g_sIncludeFolders))
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%compiler%", "Compiler path", ClassConfigs.m_ActiveConfig.g_sCompilerPath))
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%output%", "Output folder", ClassConfigs.m_ActiveConfig.g_sOutputFolder))
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%currentdir%", "BasicPawn statup folder", Application.StartupPath))

        Return sShellList.ToArray
    End Function
End Class
