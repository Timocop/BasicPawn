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


Public Class ClassSettings

#Region "Settings"
    Enum ENUM_ENFORCE_SYNTAX
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

    Public Shared g_iSettingsEnforceSyntax As ENUM_ENFORCE_SYNTAX = ENUM_ENFORCE_SYNTAX.SP_MIX
    Public Shared ReadOnly g_iSettingsDefaultEditorFont As Font = New Font("Consolas", 9, FontStyle.Regular)
    Public Shared ReadOnly g_sSettingsDefaultEditorFont As String = New FontConverter().ConvertToInvariantString(g_iSettingsDefaultEditorFont)

    'Settings
    Public Shared ReadOnly g_sSettingsFile As String = IO.Path.Combine(Application.StartupPath, "settings.ini")
    Public Shared ReadOnly g_sWindowInfoFile As String = IO.Path.Combine(Application.StartupPath, "windowinfo.ini")
    'General
    Public Shared g_bSettingsInvertColors As Boolean = False
    Public Shared g_bSettingsAlwaysOpenNewInstance As Boolean = False
    Public Shared g_bSettingsAutoShowStartPage As Boolean = True
    Public Shared g_bSettingsAutoOpenProjectFiles As Boolean = False
    Public Shared g_bSettingsAssociateSourcePawn As Boolean = False
    Public Shared g_bSettingsAssociateAmxModX As Boolean = False
    Public Shared g_bSettingsAssociateIncludes As Boolean = False
    Public Shared g_bSettingsAutoHoverScroll As Boolean = False
    Public Shared g_iSettingsThreadUpdateRate As Integer = 500
    Public Shared g_bSettingsTabCloseGotoPrevious As Boolean = True
    Public Shared g_bSettingsAutoSaveSource As Boolean = False
    Public Shared g_bSettingsAutoSaveSourceTemp As Boolean = False
    'Text Editor
    Public Shared g_iSettingsTextEditorFont As Font = g_iSettingsDefaultEditorFont
    Public Shared g_iSettingsTabsToSpaces As Integer = 0
    Public Shared g_sSettingsSyntaxHighlightingPath As String = ""
    Public Shared g_bSettingsRememberFoldings As Boolean = True
    Public Shared g_bSettingsIconBar As Boolean = True
    Public Shared g_iSettingsIconLineStateMax As Integer = 1000
    Public Shared g_iSettingsIconLineStateType As ENUM_LINE_STATE_TYPE = ENUM_LINE_STATE_TYPE.CHANGED
    Public Shared g_bSettingsShowTabs As Boolean = False
    Public Shared g_bSettingsShowVRuler As Boolean = False
    'Syntax Highligting
    Public Shared g_bSettingsDoubleClickMark As Boolean = True
    Public Shared g_bSettingsAutoMark As Boolean = True
    Public Shared g_bSettingsPublicAsDefineColor As Boolean = True
    Public Shared g_bSettingsHighlightCurrentScope As Boolean = True
    'Autocomplete
    Public Shared g_bSettingsAlwaysLoadDefaultIncludes As Boolean = True
    Public Shared g_bSettingsEnableToolTip As Boolean = True
    Public Shared g_bSettingsToolTipMethodComments As Boolean = False
    Public Shared g_bSettingsToolTipAutocompleteComments As Boolean = True
    Public Shared g_bSettingsUseWindowsToolTip As Boolean = False
    Public Shared g_bSettingsUseWindowsToolTipAnimations As Boolean = True
    Public Shared g_bSettingsUseWindowsToolTipNewlineMethods As Boolean = True
    Public Shared g_bSettingsUseWindowsToolTipDisplayTop As Boolean = True
    Public Shared g_bSettingsFullMethodAutocomplete As Boolean = False
    Public Shared g_bSettingsFullEnumAutocomplete As Boolean = False
    Public Shared g_bSettingsAutocompleteCaseSensitive As Boolean = False
    Public Shared g_iSettingsAutocompleteVarParseType As ENUM_VAR_PARSE_TYPE = ENUM_VAR_PARSE_TYPE.TAB_AND_INC
    Public Shared g_bSettingsObjectBrowserShowVariables As Boolean = False
    Public Shared g_bSettingsSwitchTabToAutocomplete As Boolean = True
    Public Shared g_bSettingsOnlyUpdateSyntaxWhenFocused As Boolean = True
    Public Shared g_bSettingsAutoCloseBrackets As Boolean = True
    Public Shared g_bSettingsAutoCloseStrings As Boolean = True
    Public Shared g_bSettingsAutoIndentBrackets As Boolean = True
    Public Shared g_iSettingsMaxParsingThreads As Integer = 0
    Public Shared g_iSettingsMaxParsingCache As Integer = 64
#End Region

    Public Shared Function GetMaxParsingThreads() As Integer
        If (g_iSettingsMaxParsingThreads < 1) Then
            Return Math.Max(1, CInt(Environment.ProcessorCount / 2))
        End If

        Return ClassTools.ClassMath.ClampInt(g_iSettingsMaxParsingThreads, 1, Environment.ProcessorCount)
    End Function

    Enum ENUM_COMPILING_TYPE
        AUTOMATIC
        CONFIG
    End Enum

    Public Shared Sub SaveSettings()
        Using mStream = ClassFileStreamWait.Create(g_sSettingsFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT)

                'Settings
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorInvertColors", If(g_bSettingsInvertColors, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AlwaysOpenNewInstance", If(g_bSettingsAlwaysOpenNewInstance, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoShowStartPage", If(g_bSettingsAutoShowStartPage, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoOpenProjectFiles", If(g_bSettingsAutoOpenProjectFiles, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AssociateSourcePawn", If(g_bSettingsAssociateSourcePawn, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AssociateAmxModX", If(g_bSettingsAssociateAmxModX, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AssociateIncludes", If(g_bSettingsAssociateIncludes, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoHoverScroll", If(g_bSettingsAutoHoverScroll, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorThreadUpdateRate", CStr(g_iSettingsThreadUpdateRate)))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TabCloseGotoPrevious", If(g_bSettingsTabCloseGotoPrevious, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoSaveSource", If(g_bSettingsAutoSaveSource, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoSaveSourceTemp", If(g_bSettingsAutoSaveSourceTemp, "1", "0")))
                'Text Editor
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorFont", New FontConverter().ConvertToInvariantString(g_iSettingsTextEditorFont)))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorTabsToSpaces", CStr(g_iSettingsTabsToSpaces)))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorSyntaxHighlightingPath", g_sSettingsSyntaxHighlightingPath))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorRememberFoldings", If(g_bSettingsRememberFoldings, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorIconBar", If(g_bSettingsIconBar, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorIconBarLineStateMax", CStr(g_iSettingsIconLineStateMax)))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorIconBarLineStateType", CStr(g_iSettingsIconLineStateType)))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorShowTabs", If(g_bSettingsShowTabs, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "TextEditorShowVRuler", If(g_bSettingsShowVRuler, "1", "0")))
                'Syntax Highligting
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "DoubleClickMark", If(g_bSettingsDoubleClickMark, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoMark", If(g_bSettingsAutoMark, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "PublicAsDefineColor", If(g_bSettingsPublicAsDefineColor, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "HighlightCurrentScope", If(g_bSettingsHighlightCurrentScope, "1", "0")))
                'Autocomplete
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AlwaysLoadDefaultIncludes", If(g_bSettingsAlwaysLoadDefaultIncludes, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutocompleteToolTip", If(g_bSettingsEnableToolTip, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "ToolTipMethodComments", If(g_bSettingsToolTipMethodComments, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "ToolTipAutocompleteComments", If(g_bSettingsToolTipAutocompleteComments, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "UseWindowsToolTip", If(g_bSettingsUseWindowsToolTip, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "UseWindowsToolTipAnimations", If(g_bSettingsUseWindowsToolTipAnimations, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "UseWindowsToolTipNewlineMethods", If(g_bSettingsUseWindowsToolTipNewlineMethods, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "UseWindowsToolTipDisplayTop", If(g_bSettingsUseWindowsToolTipDisplayTop, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "FullMethodAutocomplete", If(g_bSettingsFullMethodAutocomplete, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "FullEnumAutocomplete", If(g_bSettingsFullEnumAutocomplete, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutocompleteCaseSensitive", If(g_bSettingsAutocompleteCaseSensitive, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutocompleteVarParseType", CStr(g_iSettingsAutocompleteVarParseType)))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "ObjectBrowserShowVariables", If(g_bSettingsObjectBrowserShowVariables, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "SwitchTabToAutocomplete", If(g_bSettingsSwitchTabToAutocomplete, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "OnlyUpdateSyntaxWhenFocused", If(g_bSettingsOnlyUpdateSyntaxWhenFocused, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoCloseBrackets", If(g_bSettingsAutoCloseBrackets, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoCloseStrings", If(g_bSettingsAutoCloseStrings, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "AutoIndentBrackets", If(g_bSettingsAutoIndentBrackets, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "MaxSyntaxParsingThreads", CStr(g_iSettingsMaxParsingThreads)))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Editor", "MaxSyntaxParsingCache", CStr(g_iSettingsMaxParsingCache)))

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
                    g_bSettingsInvertColors = (mIni.ReadKeyValue("Editor", "TextEditorInvertColors", "0") <> "0")
                    g_bSettingsAlwaysOpenNewInstance = (mIni.ReadKeyValue("Editor", "AlwaysOpenNewInstance", "0") <> "0")
                    g_bSettingsAutoShowStartPage = (mIni.ReadKeyValue("Editor", "AutoShowStartPage", "1") <> "0")
                    g_bSettingsAutoOpenProjectFiles = (mIni.ReadKeyValue("Editor", "AutoOpenProjectFiles", "0") <> "0")
                    g_bSettingsAssociateSourcePawn = (mIni.ReadKeyValue("Editor", "AssociateSourcePawn", "0") <> "0")
                    g_bSettingsAssociateAmxModX = (mIni.ReadKeyValue("Editor", "AssociateAmxModX", "0") <> "0")
                    g_bSettingsAssociateIncludes = (mIni.ReadKeyValue("Editor", "AssociateIncludes", "0") <> "0")
                    g_bSettingsAutoHoverScroll = (mIni.ReadKeyValue("Editor", "AutoHoverScroll", "1") <> "0")

                    If (Integer.TryParse(mIni.ReadKeyValue("Editor", "TextEditorThreadUpdateRate", "500"), tmpInt)) Then
                        g_iSettingsThreadUpdateRate = ClassTools.ClassMath.ClampInt(tmpInt, 100, 2500)
                    End If

                    g_bSettingsTabCloseGotoPrevious = (mIni.ReadKeyValue("Editor", "TabCloseGotoPrevious", "1") <> "0")
                    g_bSettingsAutoSaveSource = (mIni.ReadKeyValue("Editor", "AutoSaveSource", "0") <> "0")
                    g_bSettingsAutoSaveSourceTemp = (mIni.ReadKeyValue("Editor", "AutoSaveSourceTemp", "0") <> "0")

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

                    g_bSettingsShowTabs = (mIni.ReadKeyValue("Editor", "TextEditorShowTabs", "0") <> "0")
                    g_bSettingsShowVRuler = (mIni.ReadKeyValue("Editor", "TextEditorShowVRuler", "0") <> "0")

                    'Syntax Highligting
                    g_bSettingsDoubleClickMark = (mIni.ReadKeyValue("Editor", "DoubleClickMark", "1") <> "0")
                    g_bSettingsAutoMark = (mIni.ReadKeyValue("Editor", "AutoMark", "1") <> "0")
                    g_bSettingsPublicAsDefineColor = (mIni.ReadKeyValue("Editor", "PublicAsDefineColor", "1") <> "0")
                    g_bSettingsHighlightCurrentScope = (mIni.ReadKeyValue("Editor", "HighlightCurrentScope", "1") <> "0")
                    'Autocomplete
                    g_bSettingsAlwaysLoadDefaultIncludes = (mIni.ReadKeyValue("Editor", "AlwaysLoadDefaultIncludes", "1") <> "0")
                    g_bSettingsEnableToolTip = (mIni.ReadKeyValue("Editor", "AutocompleteToolTip", "1") <> "0")
                    g_bSettingsToolTipMethodComments = (mIni.ReadKeyValue("Editor", "ToolTipMethodComments", "0") <> "0")
                    g_bSettingsToolTipAutocompleteComments = (mIni.ReadKeyValue("Editor", "ToolTipAutocompleteComments", "1") <> "0")
                    g_bSettingsUseWindowsToolTip = (mIni.ReadKeyValue("Editor", "UseWindowsToolTip", "0") <> "0")
                    g_bSettingsUseWindowsToolTipAnimations = (mIni.ReadKeyValue("Editor", "UseWindowsToolTipAnimations", "1") <> "0")
                    g_bSettingsUseWindowsToolTipNewlineMethods = (mIni.ReadKeyValue("Editor", "UseWindowsToolTipNewlineMethods", "1") <> "0")
                    g_bSettingsUseWindowsToolTipDisplayTop = (mIni.ReadKeyValue("Editor", "UseWindowsToolTipDisplayTop", "1") <> "0")
                    g_bSettingsFullMethodAutocomplete = (mIni.ReadKeyValue("Editor", "FullMethodAutocomplete", "0") <> "0")
                    g_bSettingsFullEnumAutocomplete = (mIni.ReadKeyValue("Editor", "FullEnumAutocomplete", "0") <> "0")
                    g_bSettingsAutocompleteCaseSensitive = (mIni.ReadKeyValue("Editor", "AutocompleteCaseSensitive", "0") <> "0")

                    If (Integer.TryParse(mIni.ReadKeyValue("Editor", "AutocompleteVarParseType", CStr(ENUM_VAR_PARSE_TYPE.TAB_AND_INC)), tmpInt)) Then
                        g_iSettingsAutocompleteVarParseType = CType(ClassTools.ClassMath.ClampInt(tmpInt, 0, [Enum].GetNames(GetType(ENUM_VAR_PARSE_TYPE)).Length - 1), ENUM_VAR_PARSE_TYPE)
                    End If

                    g_bSettingsObjectBrowserShowVariables = (mIni.ReadKeyValue("Editor", "ObjectBrowserShowVariables", "0") <> "0")
                    g_bSettingsSwitchTabToAutocomplete = (mIni.ReadKeyValue("Editor", "SwitchTabToAutocomplete", "1") <> "0")
                    g_bSettingsOnlyUpdateSyntaxWhenFocused = (mIni.ReadKeyValue("Editor", "OnlyUpdateSyntaxWhenFocused", "1") <> "0")
                    g_bSettingsAutoCloseBrackets = (mIni.ReadKeyValue("Editor", "AutoCloseBrackets", "1") <> "0")
                    g_bSettingsAutoCloseStrings = (mIni.ReadKeyValue("Editor", "AutoCloseStrings", "1") <> "0")
                    g_bSettingsAutoIndentBrackets = (mIni.ReadKeyValue("Editor", "AutoIndentBrackets", "1") <> "0")

                    If (Integer.TryParse(mIni.ReadKeyValue("Editor", "MaxSyntaxParsingThreads", "0"), tmpInt)) Then
                        g_iSettingsMaxParsingThreads = ClassTools.ClassMath.ClampInt(tmpInt, 0, Environment.ProcessorCount)
                    End If

                    If (Integer.TryParse(mIni.ReadKeyValue("Editor", "MaxSyntaxParsingCache", "64"), tmpInt)) Then
                        g_iSettingsMaxParsingCache = ClassTools.ClassMath.ClampInt(tmpInt, 0, 2048)
                    End If

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
        If (g_bSettingsAssociateSourcePawn) Then
            ClassTools.ClassRegistry.SetAssociation("BasicPawn.SourcePawn", ".sp", String.Format("""{0}"" ""%1""", Application.ExecutablePath), Application.ExecutablePath, Application.ExecutablePath, ClassTools.ClassRegistry.ENUM_SELECTION_MODEL.PLAYER)
        Else
            ClassTools.ClassRegistry.RemoveAssociation("BasicPawn.SourcePawn")
        End If

        If (g_bSettingsAssociateAmxModX) Then
            ClassTools.ClassRegistry.SetAssociation("BasicPawn.AmxModX", ".sma", String.Format("""{0}"" ""%1""", Application.ExecutablePath), Application.ExecutablePath, Application.ExecutablePath, ClassTools.ClassRegistry.ENUM_SELECTION_MODEL.PLAYER)
        Else
            ClassTools.ClassRegistry.RemoveAssociation("BasicPawn.AmxModX")
        End If

        If (g_bSettingsAssociateIncludes) Then
            ClassTools.ClassRegistry.SetAssociation("BasicPawn.Includes", ".inc", String.Format("""{0}"" ""%1""", Application.ExecutablePath), Application.ExecutablePath, Application.ExecutablePath, ClassTools.ClassRegistry.ENUM_SELECTION_MODEL.PLAYER)
        Else
            ClassTools.ClassRegistry.RemoveAssociation("BasicPawn.Includes")
        End If

        ClassTools.ClassRegistry.SetAssociation("BasicPawn.Project", UCProjectBrowser.ClassProjectControl.g_sProjectExtension, String.Format("""{0}"" ""%1""", Application.ExecutablePath), Application.ExecutablePath, Application.ExecutablePath, ClassTools.ClassRegistry.ENUM_SELECTION_MODEL.SINGLE)
    End Sub

End Class
