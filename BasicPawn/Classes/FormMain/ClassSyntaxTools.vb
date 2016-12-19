'BasicPawn
'Copyright(C) 2016 TheTimocop

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


Imports System.Text
Imports System.Text.RegularExpressions
Imports BasicPawn.FormMain
Imports ICSharpCode.TextEditor.Document

Public Class ClassSyntaxTools
    Private g_mFormMain As FormMain

    Public g_sStatementsArray As String() = {"if", "else", "for", "while", "do", "switch"}

    Public g_sHighlightWord As String = ""
    Public g_sCaretWord As String = ""

    Enum ENUM_SYNTAX_FILES
        MAIN_TEXTEDITOR
        DEBUGGER_TEXTEDITOR
    End Enum
    Structure STRUC_SYNTAX_FILES_ITEM
        Dim sFile As String
        Dim sFolder As String
        Dim sDefinition As String
    End Structure
    Public g_SyntaxFiles([Enum].GetNames(GetType(ENUM_SYNTAX_FILES)).Length - 1) As STRUC_SYNTAX_FILES_ITEM

    Public g_SyntaxXML As String = My.Resources.SourcePawn_Syntax

    Public sSyntax_HighlightCaretMarker As String = "<!-- [DO NOT EDIT | HIGHLIGHT CARET MARKER] -->"
    Public sSyntax_HighlightWordMarker As String = "<!-- [DO NOT EDIT | HIGHLIGHT WORD MARKER] -->"
    Public sSyntax_HighlightWordCustomMarker As String = "<!-- [DO NOT EDIT | HIGHLIGHT WORD CUSTOM MARKER] -->"
    Public sSyntax_HighlightDefineMarker As String = "<!-- [DO NOT EDIT | DEFINE MARKER] -->"
    Public sSyntax_HighlightEnumMarker As String = "<!-- [DO NOT EDIT | ENUM MARKER] -->"
    Public sSyntax_HighlightEnum2Marker As String = "<!-- [DO NOT EDIT | ENUM2 MARKER] -->"
    Public sSyntax_SourcePawnMarker As String = "SourcePawn-04e3632f-5472-42c5-929a-c3e0c2b35324"

    Public lAutocompleteList As New ClassSyncList(Of STRUC_AUTOCOMPLETE)

    Private _lock As Object = New Object()

    Public Enum ENUM_SYNTAX_UPDATE_TYPE
        NONE
        CARET_WORD
        HIGHLIGHT_WORD
        HIGHLIGHT_WORD_CUSTOM
        AUTOCOMPLETE
    End Enum

    Public Sub New(f As FormMain)
        g_mFormMain = f

        'Add syntax Files for TextEditor
        Dim sSyntaxWorkingDir As String = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, Guid.NewGuid.ToString)

        g_SyntaxFiles(ENUM_SYNTAX_FILES.MAIN_TEXTEDITOR) = New STRUC_SYNTAX_FILES_ITEM() With {
                                                                    .sFile = String.Format("{0}.xshd", IO.Path.Combine(sSyntaxWorkingDir, Guid.NewGuid.ToString)),
                                                                    .sFolder = sSyntaxWorkingDir,
                                                                    .sDefinition = "SourcePawn-MainTextEditor-" & Guid.NewGuid.ToString}

        g_SyntaxFiles(ENUM_SYNTAX_FILES.DEBUGGER_TEXTEDITOR) = New STRUC_SYNTAX_FILES_ITEM() With {
                                                                    .sFile = String.Format("{0}.xshd", IO.Path.Combine(sSyntaxWorkingDir, Guid.NewGuid.ToString)),
                                                                    .sFolder = sSyntaxWorkingDir,
                                                                    .sDefinition = "SourcePawn-DebugTextEditor-" & Guid.NewGuid.ToString}

        'Add all syntax files to the provider, only once
        For i = 0 To g_SyntaxFiles.Length - 1
            CreateSyntaxFile(CType(i, ENUM_SYNTAX_FILES))

            Dim synraxProvider As New FileSyntaxModeProvider(g_SyntaxFiles(i).sFolder)
            HighlightingManager.Manager.AddSyntaxModeFileProvider(synraxProvider)
        Next
    End Sub


    Public Class STRUC_FORM_COLORS_ITEM
        Public g_cControl As Control
        Public g_cBackColorOrg As Color
        Public g_cBackColorInv As Color
        Public g_cForeColorOrg As Color
        Public g_cForeColorInv As Color

        Public Sub New(cControl As Control, cBackColorOrg As Color, cBackColorInv As Color, cForeColorOrg As Color, cForeColorInv As Color)
            g_cControl = cControl
            g_cBackColorOrg = cBackColorOrg
            g_cBackColorInv = cBackColorInv
            g_cForeColorOrg = cForeColorOrg
            g_cForeColorInv = cForeColorInv
        End Sub
    End Class

    ''' <summary>
    ''' Updates the form colors and syntax.
    ''' </summary>
    Public Sub UpdateFormColors()
        UpdateSyntaxFile(ENUM_SYNTAX_UPDATE_TYPE.NONE, True) 'Just generate new files once, we dont need to create new files every type.
        UpdateSyntaxFile(ENUM_SYNTAX_UPDATE_TYPE.AUTOCOMPLETE)
        UpdateSyntaxFile(ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD)
        UpdateSyntaxFile(ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD_CUSTOM)
        UpdateSyntaxFile(ENUM_SYNTAX_UPDATE_TYPE.CARET_WORD)
        UpdateTextEditorSyntax()

        For Each c As Form In Application.OpenForms
            ClassControlStyle.UpdateControls(c)
        Next
    End Sub

    ''' <summary>
    ''' Checks if the syntax files exist. If not, they will be created.
    ''' </summary>
    Public Sub CreateSyntaxFile(i As ENUM_SYNTAX_FILES)
        Try
            SyncLock _lock
                If (Not IO.Directory.Exists(g_SyntaxFiles(i).sFolder)) Then
                    IO.Directory.CreateDirectory(g_SyntaxFiles(i).sFolder)
                End If

                Dim sModSyntaxXML As String
                If (Not String.IsNullOrEmpty(ClassSettings.g_sConfigSyntaxHighlightingPath) AndAlso
                        IO.File.Exists(ClassSettings.g_sConfigSyntaxHighlightingPath) AndAlso
                        IO.Path.GetExtension(ClassSettings.g_sConfigSyntaxHighlightingPath).ToLower = ".xml") Then

                    Dim sFileText As String = IO.File.ReadAllText(ClassSettings.g_sConfigSyntaxHighlightingPath)
                    sModSyntaxXML = sFileText.Replace(sSyntax_SourcePawnMarker, g_SyntaxFiles(i).sDefinition)
                Else
                    sModSyntaxXML = g_SyntaxXML.Replace(sSyntax_SourcePawnMarker, g_SyntaxFiles(i).sDefinition)
                End If

                IO.File.WriteAllText(g_SyntaxFiles(i).sFile, sModSyntaxXML)
            End SyncLock
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Updates the syntax file with new information (e.g highlights, defines, enums etc. from the includes and source).
    ''' This only changes the syntax file.
    ''' </summary>
    ''' <param name="iType"></param>
    ''' <param name="bForceFromMemory">If true, overwrites the syntax file from memory cache (factory new)</param>
    Public Sub UpdateSyntaxFile(iType As ENUM_SYNTAX_UPDATE_TYPE, Optional bForceFromMemory As Boolean = False)
        Try
            SyncLock _lock
                For i = 0 To g_SyntaxFiles.Length - 1
                    If (Not IO.File.Exists(g_SyntaxFiles(i).sFile) OrElse bForceFromMemory) Then
                        CreateSyntaxFile(CType(i, ENUM_SYNTAX_FILES))
                    End If

                    Select Case (i)
                        Case ENUM_SYNTAX_FILES.MAIN_TEXTEDITOR,
                                ENUM_SYNTAX_FILES.DEBUGGER_TEXTEDITOR
                            Dim SB As New StringBuilder

                            Dim iHighlightCustomCount As Integer = 0

                            Using SR As New IO.StreamReader(g_SyntaxFiles(i).sFile)
                                Dim sLine As String

                                While True
                                    sLine = SR.ReadLine
                                    If (sLine Is Nothing) Then
                                        Exit While
                                    End If

                                    Select Case (iType)
                                        Case ENUM_SYNTAX_UPDATE_TYPE.CARET_WORD
                                            If (i = ENUM_SYNTAX_FILES.MAIN_TEXTEDITOR AndAlso
                                                    sLine.Contains(sSyntax_HighlightCaretMarker)) Then

                                                SB.Append(sSyntax_HighlightCaretMarker)

                                                If (Not String.IsNullOrEmpty(g_sCaretWord) AndAlso ClassSettings.g_iSettingsAutoMark) Then
                                                    SB.Append(String.Format("<Key word=""{0}""/>", g_sCaretWord))
                                                End If

                                                SB.AppendLine()
                                                Continue While
                                            End If

                                        Case ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD
                                            If (i = ENUM_SYNTAX_FILES.MAIN_TEXTEDITOR AndAlso
                                                    sLine.Contains(sSyntax_HighlightWordMarker)) Then

                                                SB.Append(sSyntax_HighlightWordMarker)

                                                If (Not String.IsNullOrEmpty(g_sHighlightWord)) Then
                                                    SB.Append(String.Format("<Key word=""{0}""/>", g_sHighlightWord))
                                                End If

                                                SB.AppendLine()
                                                Continue While
                                            End If

                                        Case ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD_CUSTOM
                                            If (i = ENUM_SYNTAX_FILES.MAIN_TEXTEDITOR AndAlso
                                                    sLine.Contains(sSyntax_HighlightWordCustomMarker)) Then

                                                SB.Append(sSyntax_HighlightWordCustomMarker)

                                                g_mFormMain.g_ClassCustomHighlighting.Add(iHighlightCustomCount)
                                                Dim menuItem = g_mFormMain.g_ClassCustomHighlighting.m_HightlightItems()

                                                If (menuItem(iHighlightCustomCount) IsNot Nothing AndAlso Not String.IsNullOrEmpty(menuItem(iHighlightCustomCount).sWord)) Then
                                                    SB.Append(String.Format("<Key word=""{0}""/>", menuItem(iHighlightCustomCount).sWord))
                                                End If

                                                SB.AppendLine()
                                                iHighlightCustomCount += 1
                                                Continue While
                                            End If

                                        Case ENUM_SYNTAX_UPDATE_TYPE.AUTOCOMPLETE
                                            If (sLine.Contains(sSyntax_HighlightDefineMarker)) Then
                                                SB.Append(sSyntax_HighlightDefineMarker)

                                                For Each struc In lAutocompleteList
                                                    Select Case (True)
                                                        Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE,
                                                                    (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR
                                                            SB.Append(String.Format("<Key word=""{0}""/>", struc.sFunctionName))

                                                    End Select
                                                Next

                                                SB.AppendLine()
                                                Continue While
                                            End If

                                            If (sLine.Contains(sSyntax_HighlightEnumMarker)) Then
                                                SB.Append(sSyntax_HighlightEnumMarker)

                                                Dim lExistList As New List(Of String)

                                                For Each struc In lAutocompleteList
                                                    Select Case (True)
                                                        Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM
                                                            Dim sEnumName As String() = struc.sFunctionName.Split("."c)
                                                            If (sEnumName.Length = 2) Then
                                                                If (Not lExistList.Contains(sEnumName(0))) Then
                                                                    lExistList.Add(sEnumName(0))
                                                                End If

                                                                SB.Append(String.Format("<Key word=""{0}""/>", sEnumName(1)))
                                                            End If

                                                        Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP
                                                            If (Not lExistList.Contains(struc.sFunctionName)) Then
                                                                lExistList.Add(struc.sFunctionName)
                                                            End If
                                                    End Select
                                                Next

                                                SB.AppendLine()
                                                Continue While
                                            End If

                                            If (sLine.Contains(sSyntax_HighlightEnum2Marker)) Then
                                                SB.Append(sSyntax_HighlightEnum2Marker)

                                                Dim lExistList As New List(Of String)

                                                For Each struc In lAutocompleteList
                                                    Select Case (True)
                                                        Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM
                                                            Dim sEnumName As String() = struc.sFunctionName.Split("."c)
                                                            If (sEnumName.Length = 2) Then
                                                                If (Not lExistList.Contains(sEnumName(0))) Then
                                                                    lExistList.Add(sEnumName(0))
                                                                End If
                                                            End If

                                                        Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP
                                                            If (Not lExistList.Contains(struc.sFunctionName)) Then
                                                                lExistList.Add(struc.sFunctionName)
                                                            End If
                                                    End Select
                                                Next

                                                For Each s In lExistList
                                                    SB.Append(String.Format("<Key word=""{0}""/>", s))
                                                Next

                                                SB.AppendLine()
                                                Continue While
                                            End If
                                    End Select

                                    If (Not String.IsNullOrEmpty(sLine.Trim)) Then
                                        SB.AppendLine(sLine.Trim)
                                    End If
                                End While
                            End Using

                            Dim sFormatedString As String = SB.ToString

                            'Invert colors of the syntax file. But only for the default syntax file.
                            While (ClassSettings.g_iSettingsInvertColors)
                                If (Not String.IsNullOrEmpty(ClassSettings.g_sConfigSyntaxHighlightingPath) AndAlso
                                        IO.File.Exists(ClassSettings.g_sConfigSyntaxHighlightingPath) AndAlso
                                        IO.Path.GetExtension(ClassSettings.g_sConfigSyntaxHighlightingPath).ToLower = ".xml") Then
                                    Exit While
                                End If

                                Dim mMatchColl As MatchCollection = Regex.Matches(sFormatedString, "\b(color|bgcolor)\b\s*=\s*""(?<Color>[a-zA-Z]+)""")
                                For j = mMatchColl.Count - 1 To 0 Step -1
                                    If (Not mMatchColl(j).Success) Then
                                        Continue For
                                    End If

                                    Try
                                        Dim sColorName As String = mMatchColl(j).Groups("Color").Value
                                        Dim iColorNameIndex As Integer = mMatchColl(j).Groups("Color").Index
                                        Dim cConv As Color = ColorTranslator.FromHtml(sColorName)

                                        Dim cInvColor As Color = ClassControlStyle.InvertColor(cConv)

                                        If (cInvColor.R = 0 AndAlso cInvColor.G = 0 AndAlso cInvColor.B = 0) Then
                                            cInvColor = g_mFormMain.g_cDarkTextEditorBackgroundColor
                                        End If

                                        Dim sInvColor As String = ColorTranslator.ToHtml(cInvColor)

                                        sFormatedString = sFormatedString.Remove(iColorNameIndex, sColorName.Length)
                                        sFormatedString = sFormatedString.Insert(iColorNameIndex, sInvColor)
                                    Catch : End Try
                                Next

                                Exit While
                            End While

                            IO.File.WriteAllText(g_SyntaxFiles(i).sFile, sFormatedString)
                    End Select
                Next
            End SyncLock
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Updates the text editor syntax. Should be used after changing the syntax file.
    ''' </summary>
    Public Sub UpdateTextEditorSyntax()
        Try
            SyncLock _lock
                For i = 0 To g_SyntaxFiles.Length - 1
                    If (Not IO.File.Exists(g_SyntaxFiles(i).sFile)) Then
                        CreateSyntaxFile(CType(i, ENUM_SYNTAX_FILES))
                    End If
                Next

                HighlightingManager.Manager.ReloadSyntaxModes()

                For i = 0 To g_SyntaxFiles.Length - 1
                    Select Case (i)
                        Case ENUM_SYNTAX_FILES.MAIN_TEXTEDITOR
                            For j = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                                If (j = g_mFormMain.g_ClassTabControl.m_ActiveTabIndex) Then
                                    If (g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.Document.HighlightingStrategy.Name <> g_SyntaxFiles(i).sDefinition) Then
                                        g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.SetHighlighting(g_SyntaxFiles(i).sDefinition)
                                    End If
                                Else
                                    If (g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.Document.HighlightingStrategy.Name <> "Default") Then
                                        g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.SetHighlighting("Default")
                                    End If
                                End If
                            Next

                            g_mFormMain.g_mUCToolTip.TextEditorControl_ToolTip.SetHighlighting(g_SyntaxFiles(i).sDefinition)
                            g_mFormMain.g_mUCToolTip.TextEditorControl_ToolTip.Font = New Font(g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Font.FontFamily, 8, FontStyle.Regular)

                        Case ENUM_SYNTAX_FILES.DEBUGGER_TEXTEDITOR
                            If (g_mFormMain.g_mFormDebugger IsNot Nothing AndAlso Not g_mFormMain.g_mFormDebugger.IsDisposed) Then
                                g_mFormMain.g_mFormDebugger.TextEditorControlEx_DebuggerSource.SetHighlighting(g_SyntaxFiles(i).sDefinition)
                                g_mFormMain.g_mFormDebugger.TextEditorControlEx_DebuggerDiasm.SetHighlighting(g_SyntaxFiles(i).sDefinition)
                            End If
                    End Select
                Next
            End SyncLock
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Gets the expression between characters e.g if "()": MyFunc(MyArgs) => MyArgs
    ''' </summary>
    ''' <param name="sExpression"></param>
    ''' <param name="sCharOpen"></param>
    ''' <param name="sCharClose"></param>
    ''' <param name="iTargetEndScopeLevel"></param>
    ''' <param name="bInvalidCodeCheck">If true, it will ignore all Non-Code stuff like strings, comments etc.</param>
    ''' <returns></returns>
    Public Function GetExpressionBetweenCharacters(sExpression As String, sCharOpen As Char, sCharClose As Char, iTargetEndScopeLevel As Integer, Optional bInvalidCodeCheck As Boolean = False) As Integer()()
        Dim iCurrentLevel As Integer = 0
        Dim sExpressionsList As New List(Of Integer())
        Dim bWasOpen As Boolean = False

        Dim iStartLoc As Integer = 0

        Dim sourceAnalysis As ClassSyntaxSourceAnalysis = Nothing
        If (bInvalidCodeCheck) Then
            sourceAnalysis = New ClassSyntaxSourceAnalysis(sExpression)
        End If

        For i = 0 To sExpression.Length - 1
            If (sourceAnalysis IsNot Nothing AndAlso bInvalidCodeCheck) Then
                If (sourceAnalysis.InNonCode(i)) Then
                    Continue For
                End If
            End If

            If (sExpression(i) = sCharOpen) Then
                iCurrentLevel += 1
            End If

            If (Not bWasOpen AndAlso iCurrentLevel >= iTargetEndScopeLevel) Then
                iStartLoc = i
                bWasOpen = True
            End If

            If (sExpression(i) = sCharClose) Then
                iCurrentLevel -= 1
                If (iCurrentLevel <= 0) Then
                    iCurrentLevel = 0
                End If
            End If

            If (bWasOpen AndAlso iCurrentLevel < iTargetEndScopeLevel) Then
                sExpressionsList.Add({iStartLoc, i})
                bWasOpen = False
            End If
        Next

        Return sExpressionsList.ToArray
    End Function

    ''' <summary>
    ''' Automatic formats tabs in the code.
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <returns></returns>
    Public Function FormatCode(sSource As String) As String
        Dim SB As New StringBuilder
        Using SR As New IO.StringReader(sSource)
            While True
                Dim sLine As String = SR.ReadLine
                If (sLine Is Nothing) Then
                    Exit While
                End If

                SB.AppendLine(sLine.Trim)
            End While
        End Using
        sSource = SB.ToString

        Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

        Dim iBraceCount As Integer = 0
        Dim iBracedCount As Integer = 0

        For i = sSource.Length - 1 - 1 To 0 Step -1
            Try
                Select Case (sSource(i))
                    Case "("c
                        If (Not sourceAnalysis.InNonCode(i)) Then
                            iBracedCount -= 1
                        End If

                    Case ")"c
                        If (Not sourceAnalysis.InNonCode(i)) Then
                            iBracedCount += 1
                        End If

                    Case "{"c
                        If (Not sourceAnalysis.InNonCode(i)) Then
                            iBraceCount -= 1
                        End If

                    Case "}"c
                        If (Not sourceAnalysis.InNonCode(i)) Then
                            iBraceCount += 1
                        End If

                    Case vbLf(0)
                        'If (Not sourceAnalysis.InNonCode(i)) Then
                        sSource = sSource.Insert(i + 1, New String(vbTab(0), sourceAnalysis.GetBraceLevel(i + 1 + iBraceCount) + If(iBracedCount > 0, iBracedCount + 1, 0)))
                        'End If
                        iBraceCount = 0

                End Select
            Catch ex As Exception
                ' Ignore random errors
            End Try
        Next

        Return sSource
    End Function

    ''' <summary>
    ''' Checks if the source requires new decls and returns the offset.
    ''' NOTE: High false positive rate.
    ''' Returns -1 if not found.
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <param name="bIgnoreChecks"></param>
    ''' <returns>The offset in the source, -1 if not found.</returns>
    Public Function HasNewDeclsPragma(sSource As String, Optional bIgnoreChecks As Boolean = True) As Integer
        'TODO: Add better check
        Dim sRegexPattern As String = "\#\b(pragma)\b(\s+|\s*(\\*)\s*)\b(newdecls)\b(\s+|\s*(\\*)\s*)\b(required)\b"

        If (bIgnoreChecks) Then
            For Each match As Match In Regex.Matches(sSource, sRegexPattern, RegexOptions.Multiline)
                Return match.Index
            Next
        Else
            Dim sourceAnalysis As New ClassSyntaxSourceAnalysis(sSource)
            For Each match As Match In Regex.Matches(sSource, sRegexPattern, RegexOptions.Multiline)
                If (sourceAnalysis.InNonCode(match.Index)) Then
                    Continue For
                End If

                Return match.Index
            Next
        End If

        Return -1
    End Function

    ''' <summary>
    ''' Checks if the name is a forbidden name.
    ''' </summary>
    ''' <param name="sName"></param>
    ''' <returns></returns>
    Public Function IsForbiddenVariableName(sName As String) As Boolean
        Static sRegexBadName As String = Nothing
        If (sRegexBadName = Nothing) Then
            Dim lBadList As New List(Of String)
            lBadList.Add("const")
            lBadList.Add("new")
            lBadList.Add("decl")
            lBadList.Add("static")
            lBadList.Add("stock")
            lBadList.Add("public")
            lBadList.Add("private")
            lBadList.Add("if")
            lBadList.Add("for")
            lBadList.Add("else")
            lBadList.Add("case")
            lBadList.Add("switch")
            lBadList.Add("while")
            lBadList.Add("do")
            lBadList.Add("enum")

            lBadList.Add("true")
            lBadList.Add("false")
            lBadList.Add("null")

            lBadList.Add("delete")
            lBadList.Add("sizeof")
            sRegexBadName = String.Format("^({0})$", String.Join("|", lBadList.ToArray))
        End If

        Return Regex.IsMatch(sName, sRegexBadName)
    End Function

    Public Class ClassSyntaxSourceAnalysis
        Enum ENUM_STATE_TYPES
            PARENTHESIS_LEVEL
            BRACKET_LEVEL
            BRACE_LEVEL
            IN_SINGLE_COMMENT
            IN_MULTI_COMMENT
            IN_STRING
            IN_CHAR
            IN_PREPROCESSOR
        End Enum

        Private iStateArray As Integer(,)
        Private iMaxLenght As Integer = 0
        Private sCacheText As String = ""

        ''' <summary>
        ''' Gets the max lenght
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property GetMaxLenght() As Integer
            Get
                Return iMaxLenght
            End Get
        End Property

        ''' <summary>
        ''' If char index is in single-comment
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property InSingleComment(i As Integer) As Boolean
            Get
                Return iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) > 0
            End Get
        End Property

        ''' <summary>
        ''' If char index is in multi-comment
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property InMultiComment(i As Integer) As Boolean
            Get
                Return iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) > 0
            End Get
        End Property

        ''' <summary>
        ''' If char index is in string
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property InString(i As Integer) As Boolean
            Get
                Return iStateArray(i, ENUM_STATE_TYPES.IN_STRING) > 0
            End Get
        End Property

        ''' <summary>
        ''' If char index is in char
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property InChar(i As Integer) As Boolean
            Get
                Return iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) > 0
            End Get
        End Property

        ''' <summary>
        ''' If char index is in Preprocessor (#define, #pragma etc.)
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property InPreprocessor(i As Integer) As Boolean
            Get
                Return iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) > 0
            End Get
        End Property

        ''' <summary>
        ''' Get current parenthesis "(" or ")" level
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property GetParenthesisLevel(i As Integer) As Integer
            Get
                Return iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL)
            End Get
        End Property

        ''' <summary>
        ''' Get current parenthesis "[" or "]" level
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property GetBracketLevel(i As Integer) As Integer
            Get
                Return iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL)
            End Get
        End Property

        ''' <summary>
        ''' Get current parenthesis "{" or "}" level
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property GetBraceLevel(i As Integer) As Integer
            Get
                Return iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL)
            End Get
        End Property

        ''' <summary>
        ''' It will return true if the char index is in a string, char, single- or multi-comment.
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property InNonCode(i As Integer) As Boolean
            Get
                Return (iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) > 0 OrElse
                            iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) > 0 OrElse
                            iStateArray(i, ENUM_STATE_TYPES.IN_STRING) > 0 OrElse
                            iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) > 0)
            End Get
        End Property

        Public Function GetIndexFromLine(iLine As Integer) As Integer
            If (iLine = 0) Then
                Return 0
            End If

            Dim iLineCount As Integer = 0
            For i = 0 To sCacheText.Length - 1
                Select Case (sCacheText(i))
                    Case vbLf(0)
                        iLineCount += 1

                        If (iLineCount >= iLine) Then
                            Return If(i + 1 > sCacheText.Length - 1, -1, i + 1)
                        End If
                End Select
            Next

            Return -1
        End Function


        Public Sub New(ByRef sText As String, Optional bIgnorePreprocessor As Boolean = True)
            sCacheText = sText
            iMaxLenght = sText.Length

            iStateArray = New Integer(sText.Length, [Enum].GetNames(GetType(ENUM_STATE_TYPES)).Length - 1) {}

            Dim iParenthesisLevel As Integer = 0 '()
            Dim iBracketLevel As Integer = 0 '[]
            Dim iBraceLevel As Integer = 0 '{}
            Dim bInSingleComment As Integer = 0
            Dim bInMultiComment As Integer = 0
            Dim bInString As Integer = 0
            Dim bInChar As Integer = 0
            Dim bInPreprocessor As Integer = 0

            For i = 0 To sText.Length - 1
                iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) = iParenthesisLevel '0
                iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel '1
                iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel '2
                iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment '3
                iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = bInMultiComment '4
                iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = bInString '5
                iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) = bInChar '6
                iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = bInPreprocessor '7

                Select Case (sText(i))
                    Case "#"c
                        If (iParenthesisLevel > 0 OrElse iBracketLevel > 0 OrElse iBraceLevel > 0 OrElse bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        '/*
                        bInPreprocessor = 1
                        iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = 1
                    Case "*"c
                        If (bInSingleComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        '/*
                        If (i > 0) Then
                            If (sText(i - 1) = "/"c AndAlso bInMultiComment < 1) Then
                                bInMultiComment = 1
                                iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = 1
                                iStateArray(i - 1, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = 1
                                Continue For
                            End If
                        End If

                        If (i + 1 < sText.Length - 1) Then
                            If (sText(i + 1) = "/"c AndAlso bInMultiComment > 0) Then
                                bInMultiComment = 0
                                iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = 1
                                iStateArray(i + 1, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = 1

                                i += 1
                                Continue For
                            End If
                        End If
                    Case "/"c
                        If (bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        '//
                        If (i + 1 < sText.Length - 1) Then
                            If (sText(i + 1) = "/"c AndAlso bInSingleComment < 1) Then
                                bInSingleComment = 1
                                iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                            End If
                        End If
                    Case "("c
                        If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        iParenthesisLevel += 1
                        iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) = iParenthesisLevel
                    Case ")"c
                        If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        iParenthesisLevel -= 1
                        iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) = iParenthesisLevel
                    Case "["c
                        If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        iBracketLevel += 1
                        iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel
                    Case "]"c
                        If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        iBracketLevel -= 1
                        iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel
                    Case "{"c
                        If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        iBraceLevel += 1
                        iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel
                    Case "}"c
                        If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        iBraceLevel -= 1
                        iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel
                    Case "'"c
                        If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0) Then
                            Continue For
                        End If

                        'ignore \'
                        If (i > 1 AndAlso sText(i - 1) <> "\"c) Then
                            bInChar = If(bInChar > 0, 0, 1)
                            iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) = 1
                        ElseIf (i > 2 AndAlso sText(i - 1) = "\"c AndAlso sText(i - 2) = "\"c) Then
                            bInChar = If(bInChar > 0, 0, 1)
                            iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) = 1
                        End If
                    Case """"c
                        If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        'ignore \"
                        If (i > 1 AndAlso sText(i - 1) <> "\"c) Then
                            bInString = If(bInString > 0, 0, 1)
                            iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = 1
                        ElseIf (i > 2 AndAlso sText(i - 1) = "\"c AndAlso sText(i - 2) = "\"c) Then
                            bInString = If(bInString > 0, 0, 1)
                            iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = 1
                        End If
                    Case vbLf(0)
                        If (bInSingleComment > 0) Then
                            bInSingleComment = 0
                            iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                        End If

                        If (bInPreprocessor > 0) Then
                            bInPreprocessor = 0
                            iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = bInPreprocessor
                        End If
                    Case Else
                        iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) = iParenthesisLevel
                        iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel
                        iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel
                        iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                        iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = bInMultiComment
                        iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = bInString
                        iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) = bInChar
                        iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = bInPreprocessor
                End Select
            Next
        End Sub
    End Class

End Class
