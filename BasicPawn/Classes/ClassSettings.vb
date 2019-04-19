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


Public Class ClassSettings

#Region "Settings"
    Enum ENUM_AUTOCOMPLETE_SYNTAX
        SP_MIX
        SP_1_6
        SP_1_7
    End Enum

    Enum ENUM_VAR_PARSE_TYPE
        ALL
        TAB_AND_INC
        TAB
    End Enum

    Enum ENUM_LINE_STATE_TYPE
        NONE
        CHANGED_AND_SAVED
        CHANGED
    End Enum

    Public Shared g_iSettingsAutocompleteSyntax As ENUM_AUTOCOMPLETE_SYNTAX = ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX
    Public Shared ReadOnly g_iSettingsDefaultEditorFont As Font = New Font("Consolas", 9, FontStyle.Regular)
    Public Shared ReadOnly g_sSettingsDefaultEditorFont As String = New FontConverter().ConvertToInvariantString(g_iSettingsDefaultEditorFont)

    'Settings
    Public Shared ReadOnly g_sSettingsFile As String = IO.Path.Combine(Application.StartupPath, "settings.ini")
    Public Shared ReadOnly g_sWindowInfoFile As String = IO.Path.Combine(Application.StartupPath, "windowinfo.ini")
    'General
    Public Shared g_iSettingsInvertColors As Boolean = False
    Public Shared g_iSettingsAlwaysOpenNewInstance As Boolean = False
    Public Shared g_iSettingsAutoShowStartPage As Boolean = True
    Public Shared g_iSettingsAutoOpenProjectFiles As Boolean = False
    Public Shared g_iSettingsAssociateSourcePawn As Boolean = False
    Public Shared g_iSettingsAssociateAmxModX As Boolean = False
    Public Shared g_iSettingsAssociateIncludes As Boolean = False
    Public Shared g_iSettingsAutoHoverScroll As Boolean = False
    Public Shared g_iSettingsThreadUpdateRate As Integer = 500
    'Text Editor
    Public Shared g_iSettingsTextEditorFont As Font = g_iSettingsDefaultEditorFont
    Public Shared g_iSettingsTabsToSpaces As Integer = 0
    Public Shared g_sSettingsSyntaxHighlightingPath As String = ""
    Public Shared g_bSettingsRememberFoldings As Boolean = True
    Public Shared g_bSettingsIconBar As Boolean = True
    Public Shared g_iSettingsIconLineStateMax As Integer = 1000
    Public Shared g_iSettingsIconLineStateType As ENUM_LINE_STATE_TYPE = ENUM_LINE_STATE_TYPE.CHANGED
    'Syntax Highligting
    Public Shared g_iSettingsDoubleClickMark As Boolean = True
    Public Shared g_iSettingsAutoMark As Boolean = True
    Public Shared g_iSettingsPublicAsDefineColor As Boolean = True
    'Autocomplete
    Public Shared g_iSettingsAlwaysLoadDefaultIncludes As Boolean = True
    Public Shared g_iSettingsEnableToolTip As Boolean = True
    Public Shared g_iSettingsToolTipMethodComments As Boolean = False
    Public Shared g_iSettingsToolTipAutocompleteComments As Boolean = True
    Public Shared g_iSettingsUseWindowsToolTip As Boolean = False
    Public Shared g_iSettingsUseWindowsToolTipAnimations As Boolean = True
    Public Shared g_iSettingsUseWindowsToolTipNewlineMethods As Boolean = True
    Public Shared g_iSettingsUseWindowsToolTipDisplayTop As Boolean = True
    Public Shared g_iSettingsFullMethodAutocomplete As Boolean = False
    Public Shared g_iSettingsFullEnumAutocomplete As Boolean = False
    Public Shared g_iSettingsAutocompleteCaseSensitive As Boolean = False
    Public Shared g_iSettingsAutocompleteVarParseType As ENUM_VAR_PARSE_TYPE = ENUM_VAR_PARSE_TYPE.TAB_AND_INC
    Public Shared g_iSettingsObjectBrowserShowVariables As Boolean = False
    Public Shared g_iSettingsSwitchTabToAutocomplete As Boolean = True
    Public Shared g_iSettingsOnlyUpdateSyntaxWhenFocused As Boolean = True
    Public Shared g_iSettingsAutoCloseBrackets As Boolean = True
    Public Shared g_iSettingsAutoCloseStrings As Boolean = True
    Public Shared g_iSettingsAutoIndentBrackets As Boolean = True
#End Region

    Enum ENUM_COMPILING_TYPE
        AUTOMATIC
        CONFIG
    End Enum

    Public Shared Sub SaveSettings()
        Using mStream = ClassFileStreamWait.Create(g_sSettingsFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT)

                'Settings
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorInvertColors", If(g_iSettingsInvertColors, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AlwaysOpenNewInstance", If(g_iSettingsAlwaysOpenNewInstance, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoShowStartPage", If(g_iSettingsAutoShowStartPage, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoOpenProjectFiles", If(g_iSettingsAutoOpenProjectFiles, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AssociateSourcePawn", If(g_iSettingsAssociateSourcePawn, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AssociateAmxModX", If(g_iSettingsAssociateAmxModX, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AssociateIncludes", If(g_iSettingsAssociateIncludes, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoHoverScroll", If(g_iSettingsAutoHoverScroll, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorThreadUpdateRate", CStr(g_iSettingsThreadUpdateRate)))
                'Text Editor
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorFont", New FontConverter().ConvertToInvariantString(g_iSettingsTextEditorFont)))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorTabsToSpaces", CStr(g_iSettingsTabsToSpaces)))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorSyntaxHighlightingPath", g_sSettingsSyntaxHighlightingPath))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorRememberFoldings", If(g_bSettingsRememberFoldings, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorIconBar", If(g_bSettingsIconBar, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorIconBarLineStateMax", CStr(g_iSettingsIconLineStateMax)))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorIconBarLineStateType", CStr(g_iSettingsIconLineStateType)))
                'Syntax Highligting
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "DoubleClickMark", If(g_iSettingsDoubleClickMark, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoMark", If(g_iSettingsAutoMark, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "PublicAsDefineColor", If(g_iSettingsPublicAsDefineColor, "1", "0")))
                'Autocomplete
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AlwaysLoadDefaultIncludes", If(g_iSettingsAlwaysLoadDefaultIncludes, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutocompleteToolTip", If(g_iSettingsEnableToolTip, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "ToolTipMethodComments", If(g_iSettingsToolTipMethodComments, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "ToolTipAutocompleteComments", If(g_iSettingsToolTipAutocompleteComments, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "UseWindowsToolTip", If(g_iSettingsUseWindowsToolTip, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "UseWindowsToolTipAnimations", If(g_iSettingsUseWindowsToolTipAnimations, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "UseWindowsToolTipNewlineMethods", If(g_iSettingsUseWindowsToolTipNewlineMethods, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "UseWindowsToolTipDisplayTop", If(g_iSettingsUseWindowsToolTipDisplayTop, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "FullMethodAutocomplete", If(g_iSettingsFullMethodAutocomplete, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "FullEnumAutocomplete", If(g_iSettingsFullEnumAutocomplete, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutocompleteCaseSensitive", If(g_iSettingsAutocompleteCaseSensitive, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutocompleteVarParseType", CStr(g_iSettingsAutocompleteVarParseType)))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "ObjectBrowserShowVariables", If(g_iSettingsObjectBrowserShowVariables, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "SwitchTabToAutocomplete", If(g_iSettingsSwitchTabToAutocomplete, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "OnlyUpdateSyntaxWhenFocused", If(g_iSettingsOnlyUpdateSyntaxWhenFocused, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoCloseBrackets", If(g_iSettingsAutoCloseBrackets, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoCloseStrings", If(g_iSettingsAutoCloseStrings, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoIndentBrackets", If(g_iSettingsAutoIndentBrackets, "1", "0")))

                mIni.WriteKeyValue(lContent.ToArray)

                SetRegistryKeys()
            End Using
        End Using
    End Sub

    Public Shared Sub LoadSettings()
        Try
            Dim tmpInt As Integer

            Using mStream = ClassFileStreamWait.Create(g_sSettingsFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    'Settings 
                    g_iSettingsInvertColors = (mIni.ReadKeyValue("Editor", "TextEditorInvertColors", "0") <> "0")
                    g_iSettingsAlwaysOpenNewInstance = (mIni.ReadKeyValue("Editor", "AlwaysOpenNewInstance", "0") <> "0")
                    g_iSettingsAutoShowStartPage = (mIni.ReadKeyValue("Editor", "AutoShowStartPage", "1") <> "0")
                    g_iSettingsAutoOpenProjectFiles = (mIni.ReadKeyValue("Editor", "AutoOpenProjectFiles", "0") <> "0")
                    g_iSettingsAssociateSourcePawn = (mIni.ReadKeyValue("Editor", "AssociateSourcePawn", "0") <> "0")
                    g_iSettingsAssociateAmxModX = (mIni.ReadKeyValue("Editor", "AssociateAmxModX", "0") <> "0")
                    g_iSettingsAssociateIncludes = (mIni.ReadKeyValue("Editor", "AssociateIncludes", "0") <> "0")
                    g_iSettingsAutoHoverScroll = (mIni.ReadKeyValue("Editor", "AutoHoverScroll", "1") <> "0")

                    If (Integer.TryParse(mIni.ReadKeyValue("Editor", "TextEditorThreadUpdateRate", "500"), tmpInt)) Then
                        g_iSettingsThreadUpdateRate = ClassTools.ClassMath.ClampInt(tmpInt, 100, 2500)
                    End If

                    'Text Editor
                    Dim mFont As Font = CType(New FontConverter().ConvertFromInvariantString(mIni.ReadKeyValue("Editor", "TextEditorFont", g_sSettingsDefaultEditorFont)), Font)
                    If (mFont IsNot Nothing AndAlso mFont.Size < 256) Then
                        g_iSettingsTextEditorFont = mFont
                    Else
                        g_iSettingsTextEditorFont = g_iSettingsDefaultEditorFont
                    End If

                    If (Integer.TryParse(mIni.ReadKeyValue("Editor", "TextEditorTabsToSpaces", "0"), tmpInt)) Then
                        g_iSettingsTabsToSpaces = ClassTools.ClassMath.ClampInt(tmpInt, 0, 100)
                    End If

                    g_sSettingsSyntaxHighlightingPath = mIni.ReadKeyValue("Editor", "TextEditorSyntaxHighlightingPath", "")
                    g_bSettingsRememberFoldings = (mIni.ReadKeyValue("Editor", "TextEditorRememberFoldings", "1") <> "0")

                    g_bSettingsIconBar = (mIni.ReadKeyValue("Editor", "TextEditorIconBar", "1") <> "0")

                    If (Integer.TryParse(mIni.ReadKeyValue("Editor", "TextEditorIconBarLineStateMax", "1000"), tmpInt)) Then
                        g_iSettingsIconLineStateMax = ClassTools.ClassMath.ClampInt(tmpInt, 0, 99999)
                    End If

                    If (Integer.TryParse(mIni.ReadKeyValue("Editor", "TextEditorIconBarLineStateType", CStr(ENUM_LINE_STATE_TYPE.CHANGED)), tmpInt)) Then
                        g_iSettingsIconLineStateType = CType(ClassTools.ClassMath.ClampInt(tmpInt, 0, [Enum].GetNames(GetType(ENUM_LINE_STATE_TYPE)).Length - 1), ENUM_LINE_STATE_TYPE)
                    End If

                    'Syntax Highligting
                    g_iSettingsDoubleClickMark = (mIni.ReadKeyValue("Editor", "DoubleClickMark", "1") <> "0")
                    g_iSettingsAutoMark = (mIni.ReadKeyValue("Editor", "AutoMark", "1") <> "0")
                    g_iSettingsPublicAsDefineColor = (mIni.ReadKeyValue("Editor", "PublicAsDefineColor", "1") <> "0")
                    'Autocomplete
                    g_iSettingsAlwaysLoadDefaultIncludes = (mIni.ReadKeyValue("Editor", "AlwaysLoadDefaultIncludes", "1") <> "0")
                    g_iSettingsEnableToolTip = (mIni.ReadKeyValue("Editor", "AutocompleteToolTip", "1") <> "0")
                    g_iSettingsToolTipMethodComments = (mIni.ReadKeyValue("Editor", "ToolTipMethodComments", "0") <> "0")
                    g_iSettingsToolTipAutocompleteComments = (mIni.ReadKeyValue("Editor", "ToolTipAutocompleteComments", "1") <> "0")
                    g_iSettingsUseWindowsToolTip = (mIni.ReadKeyValue("Editor", "UseWindowsToolTip", "0") <> "0")
                    g_iSettingsUseWindowsToolTipAnimations = (mIni.ReadKeyValue("Editor", "UseWindowsToolTipAnimations", "1") <> "0")
                    g_iSettingsUseWindowsToolTipNewlineMethods = (mIni.ReadKeyValue("Editor", "UseWindowsToolTipNewlineMethods", "1") <> "0")
                    g_iSettingsUseWindowsToolTipDisplayTop = (mIni.ReadKeyValue("Editor", "UseWindowsToolTipDisplayTop", "1") <> "0")
                    g_iSettingsFullMethodAutocomplete = (mIni.ReadKeyValue("Editor", "FullMethodAutocomplete", "0") <> "0")
                    g_iSettingsFullEnumAutocomplete = (mIni.ReadKeyValue("Editor", "FullEnumAutocomplete", "0") <> "0")
                    g_iSettingsAutocompleteCaseSensitive = (mIni.ReadKeyValue("Editor", "AutocompleteCaseSensitive", "0") <> "0")

                    If (Integer.TryParse(mIni.ReadKeyValue("Editor", "AutocompleteVarParseType", CStr(ENUM_VAR_PARSE_TYPE.TAB_AND_INC)), tmpInt)) Then
                        g_iSettingsAutocompleteVarParseType = CType(ClassTools.ClassMath.ClampInt(tmpInt, 0, [Enum].GetNames(GetType(ENUM_VAR_PARSE_TYPE)).Length - 1), ENUM_VAR_PARSE_TYPE)
                    End If

                    g_iSettingsObjectBrowserShowVariables = (mIni.ReadKeyValue("Editor", "ObjectBrowserShowVariables", "0") <> "0")
                    g_iSettingsSwitchTabToAutocomplete = (mIni.ReadKeyValue("Editor", "SwitchTabToAutocomplete", "1") <> "0")
                    g_iSettingsOnlyUpdateSyntaxWhenFocused = (mIni.ReadKeyValue("Editor", "OnlyUpdateSyntaxWhenFocused", "1") <> "0")
                    g_iSettingsAutoCloseBrackets = (mIni.ReadKeyValue("Editor", "AutoCloseBrackets", "1") <> "0")
                    g_iSettingsAutoCloseStrings = (mIni.ReadKeyValue("Editor", "AutoCloseStrings", "1") <> "0")
                    g_iSettingsAutoIndentBrackets = (mIni.ReadKeyValue("Editor", "AutoIndentBrackets", "1") <> "0")

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

        'Dont save wrong bounds when window is minimized
        If (mForm.WindowState = FormWindowState.Minimized) Then
            Return
        End If

        Using mStream = ClassFileStreamWait.Create(g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT) From {
                    New ClassIni.STRUC_INI_CONTENT(mForm.Name, "X", CStr(mForm.Location.X)),
                    New ClassIni.STRUC_INI_CONTENT(mForm.Name, "Y", CStr(mForm.Location.Y)),
                    New ClassIni.STRUC_INI_CONTENT(mForm.Name, "Maximized", If(mForm.WindowState = FormWindowState.Maximized, "1", "0")),
                    New ClassIni.STRUC_INI_CONTENT(mForm.Name, "Width", CStr(mForm.Width)),
                    New ClassIni.STRUC_INI_CONTENT(mForm.Name, "Height", CStr(mForm.Height))
                }

                mIni.WriteKeyValue(lContent.ToArray)
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

                'Clamp form bounds by current screen bounds
                Dim mScreen As Screen = Screen.FromControl(mForm)

                If (mScreen IsNot Nothing) Then
                    If (mForm.Location.X < mScreen.Bounds.X) Then
                        mForm.Location = New Point(mScreen.Bounds.X, mForm.Location.Y)
                    End If
                    If (mForm.Location.Y < mScreen.Bounds.Y) Then
                        mForm.Location = New Point(mForm.Location.X, mScreen.Bounds.Y)
                    End If
                    If (mForm.WindowState = FormWindowState.Normal AndAlso mForm.Location.X + mForm.Width > mScreen.Bounds.Width) Then
                        mForm.Width = mScreen.Bounds.Width - mForm.Location.X
                    End If
                    If (mForm.WindowState = FormWindowState.Normal AndAlso mForm.Location.Y + mForm.Height > mScreen.Bounds.Height) Then
                        mForm.Height = mScreen.Bounds.Height - mForm.Location.Y
                    End If
                End If
            End Using
        End Using
    End Sub

    Private Shared Sub SetRegistryKeys()
        If (g_iSettingsAssociateSourcePawn) Then
            ClassTools.ClassRegistry.SetAssociation("BasicPawn.SourcePawn", ".sp", String.Format("""{0}"" ""%1""", Application.ExecutablePath), Application.ExecutablePath, Application.ExecutablePath, ClassTools.ClassRegistry.ENUM_SELECTION_MODEL.PLAYER)
        Else
            ClassTools.ClassRegistry.RemoveAssociation("BasicPawn.SourcePawn")
        End If

        If (g_iSettingsAssociateAmxModX) Then
            ClassTools.ClassRegistry.SetAssociation("BasicPawn.AmxModX", ".sma", String.Format("""{0}"" ""%1""", Application.ExecutablePath), Application.ExecutablePath, Application.ExecutablePath, ClassTools.ClassRegistry.ENUM_SELECTION_MODEL.PLAYER)
        Else
            ClassTools.ClassRegistry.RemoveAssociation("BasicPawn.AmxModX")
        End If

        If (g_iSettingsAssociateIncludes) Then
            ClassTools.ClassRegistry.SetAssociation("BasicPawn.Includes", ".inc", String.Format("""{0}"" ""%1""", Application.ExecutablePath), Application.ExecutablePath, Application.ExecutablePath, ClassTools.ClassRegistry.ENUM_SELECTION_MODEL.PLAYER)
        Else
            ClassTools.ClassRegistry.RemoveAssociation("BasicPawn.Includes")
        End If

        ClassTools.ClassRegistry.SetAssociation("BasicPawn.Project", UCProjectBrowser.ClassProjectControl.g_sProjectExtension, String.Format("""{0}"" ""%1""", Application.ExecutablePath), Application.ExecutablePath, Application.ExecutablePath, ClassTools.ClassRegistry.ENUM_SELECTION_MODEL.SINGLE)
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

    Public Shared Function BuildIndentation(iLength As Integer, iIndentationType As ENUM_INDENTATION_TYPES) As String
        Select Case (iIndentationType)
            Case ENUM_INDENTATION_TYPES.USE_SETTINGS
                If (g_iSettingsTabsToSpaces > 0) Then
                    Return New Text.StringBuilder().Insert(0, New String(" "c, g_iSettingsTabsToSpaces), iLength).ToString
                Else
                    Return New Text.StringBuilder().Insert(0, vbTab, iLength).ToString
                End If

            Case ENUM_INDENTATION_TYPES.TABS
                Return New Text.StringBuilder().Insert(0, vbTab, iLength).ToString

            Case ENUM_INDENTATION_TYPES.SPACES
                Return New Text.StringBuilder().Insert(0, New String(" "c, g_iSettingsTabsToSpaces), iLength).ToString

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
        sShellList.Add(New STRUC_SHELL_ARGUMENT_ITEM("%currentdir%", "BasicPawn startup folder", Application.StartupPath))

        Return sShellList.ToArray
    End Function
End Class
