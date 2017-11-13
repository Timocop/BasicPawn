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
    Public Shared ReadOnly g_iSettingsDefaultEditorFont As Font = New Font("Consolas", 9, FontStyle.Regular)
    Public Shared ReadOnly g_sSettingsDefaultEditorFont As String = New FontConverter().ConvertToInvariantString(g_iSettingsDefaultEditorFont)

    'Settings
    Public Shared ReadOnly g_sSettingsFile As String = IO.Path.Combine(Application.StartupPath, "settings.ini")
    Public Shared ReadOnly g_sWindowInfoFile As String = IO.Path.Combine(Application.StartupPath, "windowinfo.ini")
    'General
    Public Shared g_iSettingsAlwaysOpenNewInstance As Boolean = False
    Public Shared g_iSettingsAutoShowStartPage As Boolean = True
    Public Shared g_iSettingsAssociateSourcePawn As Boolean = False
    Public Shared g_iSettingsAssociateAmxModX As Boolean = False
    Public Shared g_iSettingsAssociateIncludes As Boolean = False
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
    Public Shared g_iSettingsUseWindowsToolTipNewlineMethods As Boolean = True
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
        Using mStream = ClassFileStreamWait.Create(g_sSettingsFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                'Settings
                mIni.WriteKeyValue("Editor", "AlwaysOpenNewInstance", If(g_iSettingsAlwaysOpenNewInstance, "1", "0"))
                mIni.WriteKeyValue("Editor", "AutoShowStartPage", If(g_iSettingsAutoShowStartPage, "1", "0"))
                mIni.WriteKeyValue("Editor", "AssociateSourcePawn", If(g_iSettingsAssociateSourcePawn, "1", "0"))
                mIni.WriteKeyValue("Editor", "AssociateAmxModX", If(g_iSettingsAssociateAmxModX, "1", "0"))
                mIni.WriteKeyValue("Editor", "AssociateIncludes", If(g_iSettingsAssociateIncludes, "1", "0"))
                'Text Editor
                mIni.WriteKeyValue("Editor", "TextEditorFont", New FontConverter().ConvertToInvariantString(g_iSettingsTextEditorFont))
                mIni.WriteKeyValue("Editor", "TextEditorInvertColors", If(g_iSettingsInvertColors, "1", "0"))
                mIni.WriteKeyValue("Editor", "TextEditorTabsToSpaces", CStr(g_iSettingsTabsToSpaces))
                'Syntax Highligting
                mIni.WriteKeyValue("Editor", "DoubleClickMark", If(g_iSettingsDoubleClickMark, "1", "0"))
                mIni.WriteKeyValue("Editor", "AutoMark", If(g_iSettingsAutoMark, "1", "0"))
                'Autocomplete
                mIni.WriteKeyValue("Editor", "AlwaysLoadDefaultIncludes", If(g_iSettingsAlwaysLoadDefaultIncludes, "1", "0"))
                mIni.WriteKeyValue("Editor", "AutocompleteToolTip", If(g_iSettingsEnableToolTip, "1", "0"))
                mIni.WriteKeyValue("Editor", "ToolTipMethodComments", If(g_iSettingsToolTipMethodComments, "1", "0"))
                mIni.WriteKeyValue("Editor", "ToolTipAutocompleteComments", If(g_iSettingsToolTipAutocompleteComments, "1", "0"))
                mIni.WriteKeyValue("Editor", "UseWindowsToolTip", If(g_iSettingsUseWindowsToolTip, "1", "0"))
                mIni.WriteKeyValue("Editor", "UseWindowsToolTipAnimations", If(g_iSettingsUseWindowsToolTipAnimations, "1", "0"))
                mIni.WriteKeyValue("Editor", "UseWindowsToolTipNewlineMethods", If(g_iSettingsUseWindowsToolTipNewlineMethods, "1", "0"))
                mIni.WriteKeyValue("Editor", "FullMethodAutocomplete", If(g_iSettingsFullMethodAutocomplete, "1", "0"))
                mIni.WriteKeyValue("Editor", "FullEnumAutocomplete", If(g_iSettingsFullEnumAutocomplete, "1", "0"))
                mIni.WriteKeyValue("Editor", "AutocompleteCaseSensitive", If(g_iSettingsAutocompleteCaseSensitive, "1", "0"))
                mIni.WriteKeyValue("Editor", "VarAutocompleteCurrentSourceOnly", If(g_iSettingsVarAutocompleteCurrentSourceOnly, "1", "0"))
                mIni.WriteKeyValue("Editor", "VarAutocompleteShowObjectBrowser", If(g_iSettingsVarAutocompleteShowObjectBrowser, "1", "0"))
                mIni.WriteKeyValue("Editor", "SwitchTabToAutocomplete", If(g_iSettingsSwitchTabToAutocomplete, "1", "0"))
                'Debugger
                mIni.WriteKeyValue("Debugger", "CatchExceptions", If(g_iSettingsDebuggerCatchExceptions, "1", "0"))
                mIni.WriteKeyValue("Debugger", "EntitiesColoring", If(g_iSettingsDebuggerEntitiesEnableColoring, "1", "0"))
                mIni.WriteKeyValue("Debugger", "EntitiesAutoScroll", If(g_iSettingsDebuggerEntitiesEnableAutoScroll, "1", "0"))

                SetRegistryKeys()
            End Using
        End Using
    End Sub

    Public Shared Sub LoadSettings()
        Try
            Using mStream = ClassFileStreamWait.Create(g_sSettingsFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    'Settings
                    g_iSettingsAlwaysOpenNewInstance = (mIni.ReadKeyValue("Editor", "AlwaysOpenNewInstance", "0") <> "0")
                    g_iSettingsAutoShowStartPage = (mIni.ReadKeyValue("Editor", "AutoShowStartPage", "1") <> "0")
                    g_iSettingsAssociateSourcePawn = (mIni.ReadKeyValue("Editor", "AssociateSourcePawn", "0") <> "0")
                    g_iSettingsAssociateAmxModX = (mIni.ReadKeyValue("Editor", "AssociateAmxModX", "0") <> "0")
                    g_iSettingsAssociateIncludes = (mIni.ReadKeyValue("Editor", "AssociateIncludes", "0") <> "0")
                    'Text Editor
                    Dim mFont As Font = CType(New FontConverter().ConvertFromInvariantString(mIni.ReadKeyValue("Editor", "TextEditorFont", g_sSettingsDefaultEditorFont)), Font)
                    If (mFont IsNot Nothing AndAlso mFont.Size < 256) Then
                        g_iSettingsTextEditorFont = mFont
                    Else
                        g_iSettingsTextEditorFont = g_iSettingsDefaultEditorFont
                    End If
                    g_iSettingsInvertColors = (mIni.ReadKeyValue("Editor", "TextEditorInvertColors", "0") <> "0")
                    Dim iTabsToSpaces As Integer = 0
                    If (Integer.TryParse(mIni.ReadKeyValue("Editor", "TextEditorTabsToSpaces", "0"), iTabsToSpaces)) Then
                        iTabsToSpaces = If(iTabsToSpaces < 0, 0, iTabsToSpaces)
                        iTabsToSpaces = If(iTabsToSpaces > 100, 100, iTabsToSpaces)
                        g_iSettingsTabsToSpaces = iTabsToSpaces
                    End If
                    'Syntax Highligting
                    g_iSettingsDoubleClickMark = (mIni.ReadKeyValue("Editor", "DoubleClickMark", "1") <> "0")
                    g_iSettingsAutoMark = (mIni.ReadKeyValue("Editor", "AutoMark", "1") <> "0")
                    'Autocomplete
                    g_iSettingsAlwaysLoadDefaultIncludes = (mIni.ReadKeyValue("Editor", "AlwaysLoadDefaultIncludes", "1") <> "0")
                    g_iSettingsEnableToolTip = (mIni.ReadKeyValue("Editor", "AutocompleteToolTip", "1") <> "0")
                    g_iSettingsToolTipMethodComments = (mIni.ReadKeyValue("Editor", "ToolTipMethodComments", "0") <> "0")
                    g_iSettingsToolTipAutocompleteComments = (mIni.ReadKeyValue("Editor", "ToolTipAutocompleteComments", "1") <> "0")
                    g_iSettingsUseWindowsToolTip = (mIni.ReadKeyValue("Editor", "UseWindowsToolTip", "0") <> "0")
                    g_iSettingsUseWindowsToolTipAnimations = (mIni.ReadKeyValue("Editor", "UseWindowsToolTipAnimations", "1") <> "0")
                    g_iSettingsUseWindowsToolTipNewlineMethods = (mIni.ReadKeyValue("Editor", "UseWindowsToolTipNewlineMethods", "1") <> "0")
                    g_iSettingsFullMethodAutocomplete = (mIni.ReadKeyValue("Editor", "FullMethodAutocomplete", "0") <> "0")
                    g_iSettingsFullEnumAutocomplete = (mIni.ReadKeyValue("Editor", "FullEnumAutocomplete", "0") <> "0")
                    g_iSettingsAutocompleteCaseSensitive = (mIni.ReadKeyValue("Editor", "AutocompleteCaseSensitive", "1") <> "0")
                    g_iSettingsVarAutocompleteCurrentSourceOnly = (mIni.ReadKeyValue("Editor", "VarAutocompleteCurrentSourceOnly", "1") <> "0")
                    g_iSettingsVarAutocompleteShowObjectBrowser = (mIni.ReadKeyValue("Editor", "VarAutocompleteShowObjectBrowser", "0") <> "0")
                    g_iSettingsSwitchTabToAutocomplete = (mIni.ReadKeyValue("Editor", "SwitchTabToAutocomplete", "1") <> "0")
                    'Debugger
                    g_iSettingsDebuggerCatchExceptions = (mIni.ReadKeyValue("Debugger", "CatchExceptions", "1") <> "0")
                    g_iSettingsDebuggerEntitiesEnableColoring = (mIni.ReadKeyValue("Debugger", "EntitiesColoring", "1") <> "0")
                    g_iSettingsDebuggerEntitiesEnableAutoScroll = (mIni.ReadKeyValue("Debugger", "EntitiesAutoScroll", "1") <> "0")

                    SetRegistryKeys()
                End Using
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Public Shared Sub SaveWindowInfo(mForm As Form)
        If (String.IsNullOrEmpty(mForm.Name)) Then
            Return
        End If

        Using mStream = ClassFileStreamWait.Create(g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                mIni.WriteKeyValue(mForm.Name, "X", CStr(mForm.Location.X))
                mIni.WriteKeyValue(mForm.Name, "Y", CStr(mForm.Location.Y))
                mIni.WriteKeyValue(mForm.Name, "Maximized", If(mForm.WindowState = FormWindowState.Maximized, "1", "0"))
                mIni.WriteKeyValue(mForm.Name, "Width", CStr(mForm.Width))
                mIni.WriteKeyValue(mForm.Name, "Height", CStr(mForm.Height))
            End Using
        End Using
    End Sub

    Public Shared Sub LoadWindowInfo(mForm As Form)
        If (String.IsNullOrEmpty(mForm.Name)) Then
            Return
        End If

        Dim tmpStr As String
        Dim tmpInt As Integer

        Using mStream = ClassFileStreamWait.Create(g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                tmpStr = mIni.ReadKeyValue(mForm.Name, "X", Nothing)
                If (tmpStr IsNot Nothing AndAlso Integer.TryParse(tmpStr, tmpInt)) Then
                    mForm.Location = New Point(tmpInt, mForm.Location.Y)
                End If

                tmpStr = mIni.ReadKeyValue(mForm.Name, "Y", Nothing)
                If (tmpStr IsNot Nothing AndAlso Integer.TryParse(tmpStr, tmpInt)) Then
                    mForm.Location = New Point(mForm.Location.X, tmpInt)
                End If

                tmpStr = mIni.ReadKeyValue(mForm.Name, "Maximized", Nothing)
                If (tmpStr IsNot Nothing AndAlso tmpStr = "1") Then
                    mForm.WindowState = FormWindowState.Maximized
                End If

                tmpStr = mIni.ReadKeyValue(mForm.Name, "Width", Nothing)
                If (mForm.WindowState = FormWindowState.Normal AndAlso tmpStr IsNot Nothing AndAlso Integer.TryParse(tmpStr, tmpInt)) Then
                    mForm.Width = tmpInt
                End If

                tmpStr = mIni.ReadKeyValue(mForm.Name, "Height", Nothing)
                If (mForm.WindowState = FormWindowState.Normal AndAlso tmpStr IsNot Nothing AndAlso Integer.TryParse(tmpStr, tmpInt)) Then
                    mForm.Height = tmpInt
                End If
            End Using
        End Using
    End Sub

    Private Shared Sub SetRegistryKeys()
        If (g_iSettingsAssociateSourcePawn) Then
            ClassTools.ClassRegistry.SetAssociation("BasicPawn.SourcePawn", ".sp", String.Format("""{0}"" ""%1""", Application.ExecutablePath), Application.ExecutablePath, Application.ExecutablePath)
        Else
            ClassTools.ClassRegistry.RemoveAssociation("BasicPawn.SourcePawn")
        End If

        If (g_iSettingsAssociateAmxModX) Then
            ClassTools.ClassRegistry.SetAssociation("BasicPawn.AmxModX", ".sma", String.Format("""{0}"" ""%1""", Application.ExecutablePath), Application.ExecutablePath, Application.ExecutablePath)
        Else
            ClassTools.ClassRegistry.RemoveAssociation("BasicPawn.AmxModX")
        End If

        If (g_iSettingsAssociateIncludes) Then
            ClassTools.ClassRegistry.SetAssociation("BasicPawn.Includes", ".inc", String.Format("""{0}"" ""%1""", Application.ExecutablePath), Application.ExecutablePath, Application.ExecutablePath)
        Else
            ClassTools.ClassRegistry.RemoveAssociation("BasicPawn.Includes")
        End If

        ClassTools.ClassRegistry.SetAssociation("BasicPawn.Project", UCProjectBrowser.ClassProjectControl.g_sProjectExtension, String.Format("""{0}"" ""%1""", Application.ExecutablePath), Application.ExecutablePath, Application.ExecutablePath)
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

    Enum ENUM_INDENTATION_TYPES
        USE_SETTINGS
        TABS
        SPACES
    End Enum

    Public Shared Function BuildIndentation(iLenght As Integer, iIndentationType As ENUM_INDENTATION_TYPES) As String
        Select Case (iIndentationType)
            Case ENUM_INDENTATION_TYPES.USE_SETTINGS
                If (g_iSettingsTabsToSpaces > 0) Then
                    Return New Text.StringBuilder().Insert(0, New String(" "c, g_iSettingsTabsToSpaces), iLenght).ToString
                Else
                    Return New Text.StringBuilder().Insert(0, vbTab, iLenght).ToString
                End If

            Case ENUM_INDENTATION_TYPES.TABS
                Return New Text.StringBuilder().Insert(0, vbTab, iLenght).ToString

            Case ENUM_INDENTATION_TYPES.SPACES
                Return New Text.StringBuilder().Insert(0, New String(" "c, g_iSettingsTabsToSpaces), iLenght).ToString

            Case Else
                Throw New ArgumentException("Invalid indentation type")

        End Select
    End Function

    ''' <summary>
    ''' Gets all available shell arguments
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetShellArguments(mFormMain As FormMain, sFile As String) As STRUC_SHELL_ARGUMENT_ITEM()
        'TODO: Add more shell arguments
        Dim sShellList As New List(Of STRUC_SHELL_ARGUMENT_ITEM)

        If (String.IsNullOrEmpty(sFile)) Then
            sFile = mFormMain.g_ClassTabControl.m_ActiveTab.m_File
        End If

        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%input%", "Current opened source file", sFile))
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%inputfilename%", "Current opened source filename", If(String.IsNullOrEmpty(sFile), "", IO.Path.GetFileNameWithoutExtension(sFile))))
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%inputfolder%", "Current opened source file folder", If(String.IsNullOrEmpty(sFile), "", IO.Path.GetDirectoryName(sFile))))
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%includes%", "Include folders", mFormMain.g_ClassTabControl.m_ActiveTab.m_ActiveConfig.g_sIncludeFolders))
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%compiler%", "Compiler path", mFormMain.g_ClassTabControl.m_ActiveTab.m_ActiveConfig.g_sCompilerPath))
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%output%", "Output folder", mFormMain.g_ClassTabControl.m_ActiveTab.m_ActiveConfig.g_sOutputFolder))
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%currentdir%", "BasicPawn statup folder", Application.StartupPath))

        Return sShellList.ToArray
    End Function
End Class
