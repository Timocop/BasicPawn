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


Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml
Imports ICSharpCode.TextEditor.Document

Public Class ClassSyntaxTools
    Private g_mFormMain As FormMain

    Public g_ClassSyntaxHighlighting As ClassSyntaxHighlighting

    Public Shared g_sHighlightWord As String = ""
    Public Shared g_sCaretWord As String = ""

    Public Shared g_sSyntaxXML As String = My.Resources.SourcePawn_Syntax
    Public Shared g_sSyntaxDarkXML As String = My.Resources.SourcePawn_SyntaxDark
    Public Shared g_mSyntaxProvider As ClassSyntaxHighlighting.ClassBinarySyntaxModeFileProvider

    Public Shared ReadOnly g_sSyntaxHighlightCaretMarker As String = "<!-- [DO NOT EDIT | HIGHLIGHT CARET MARKER] -->"
    Public Shared ReadOnly g_sSyntaxHighlightWordMarker As String = "<!-- [DO NOT EDIT | HIGHLIGHT WORD MARKER] -->"
    Public Shared ReadOnly g_sSyntaxHighlightWordCustomMarker As String = "<!-- [DO NOT EDIT | HIGHLIGHT WORD CUSTOM MARKER] -->"
    Public Shared ReadOnly g_sSyntaxHighlightDefineMarker As String = "<!-- [DO NOT EDIT | DEFINE MARKER] -->"
    Public Shared ReadOnly g_sSyntaxHighlightEnumMarker As String = "<!-- [DO NOT EDIT | ENUM MARKER] -->"
    Public Shared ReadOnly g_sSyntaxHighlightEnum2Marker As String = "<!-- [DO NOT EDIT | ENUM2 MARKER] -->"
    Public Shared ReadOnly g_sSyntaxSourcePawnMarker As String = "SourcePawn-04e3632f-5472-42c5-929a-c3e0c2b35324"

    Public Shared ReadOnly g_mSyntaxRequiredVersion As New Version("1.2")

    Enum ENUM_LANGUAGE_TYPE
        SOURCEPAWN
        AMXMODX
        PAWN
    End Enum
    Public Shared g_sEscapeCharacters As String()

    Public Enum ENUM_SYNTAX_UPDATE_TYPE
        NONE
        CARET_WORD
        HIGHLIGHT_WORD
        HIGHLIGHT_WORD_CUSTOM
        AUTOCOMPLETE
    End Enum

    Shared Sub New()
        g_sEscapeCharacters = New String() {"\", "^", "\"}
    End Sub


    Public Sub New(f As FormMain)
        g_mFormMain = f

        g_ClassSyntaxHighlighting = New ClassSyntaxHighlighting(Me)

        'Check escape chars
        If (g_sEscapeCharacters.Length <> [Enum].GetNames(GetType(ENUM_LANGUAGE_TYPE)).Length) Then
            Throw New ArgumentException("g_sEscapeCharacters length")
        End If

        g_mSyntaxProvider = New ClassSyntaxHighlighting.ClassBinarySyntaxModeFileProvider()
        HighlightingManager.Manager.AddSyntaxModeFileProvider(g_mSyntaxProvider)
    End Sub

    Public Class STRUC_AUTOCOMPLETE
        Private g_sInfo As String
        Private g_sFilename As String
        Private g_sPath As String
        Private g_iType As ENUM_TYPE_FLAGS
        Private g_sFunctionName As String
        Private g_sFunctionString As String
        Private g_sFullFunctionString As String
        Private g_mData As New Dictionary(Of String, Object)

        Public Sub New(mAutocomplete As STRUC_AUTOCOMPLETE)
            Me.New(mAutocomplete.m_Info, mAutocomplete.m_Filename, mAutocomplete.m_Path, mAutocomplete.m_Type, mAutocomplete.m_FunctionName, mAutocomplete.m_FunctionString, mAutocomplete.m_FullFunctionString)

            For Each mItem In mAutocomplete.g_mData
                g_mData(mItem.Key) = mItem.Value
            Next
        End Sub

        Public Sub New(sInfo As String, sFilename As String, sPath As String, iType As ENUM_TYPE_FLAGS, sFunctionName As String, sFunctionString As String, sFullFunctionString As String)
            g_sInfo = sInfo
            g_sFilename = sFilename
            g_sPath = sPath
            g_iType = iType
            g_sFunctionName = sFunctionName
            g_sFunctionString = sFunctionString
            g_sFullFunctionString = sFullFunctionString
        End Sub

        Property m_Info As String
            Get
                Return g_sInfo
            End Get
            Set(value As String)
                g_sInfo = value
            End Set
        End Property

        Property m_Filename As String
            Get
                Return g_sFilename
            End Get
            Set(value As String)
                g_sFilename = value
            End Set
        End Property

        Property m_Path As String
            Get
                Return g_sPath
            End Get
            Set(value As String)
                g_sPath = value
            End Set
        End Property

        Property m_Type As ENUM_TYPE_FLAGS
            Get
                Return g_iType
            End Get
            Set(value As ENUM_TYPE_FLAGS)
                g_iType = value
            End Set
        End Property

        Property m_FunctionName As String
            Get
                Return g_sFunctionName
            End Get
            Set(value As String)
                g_sFunctionName = value
            End Set
        End Property

        Property m_FunctionString As String
            Get
                Return g_sFunctionString
            End Get
            Set(value As String)
                g_sFunctionString = value
            End Set
        End Property

        Property m_FullFunctionString As String
            Get
                Return g_sFullFunctionString
            End Get
            Set(value As String)
                g_sFullFunctionString = value
            End Set
        End Property

        ReadOnly Property m_Data As Dictionary(Of String, Object)
            Get
                Return g_mData
            End Get
        End Property

        Enum ENUM_TYPE_FLAGS
            NONE = 0
            DEBUG = (1 << 0)
            DEFINE = (1 << 1)
            [ENUM] = (1 << 2)
            FUNCENUM = (1 << 3)
            FUNCTAG = (1 << 4)
            STOCK = (1 << 5)
            [STATIC] = (1 << 6)
            [CONST] = (1 << 7)
            [PUBLIC] = (1 << 8)
            NATIVE = (1 << 9)
            METHOD = (1 << 10)
            FORWARD = (1 << 11)
            TYPESET = (1 << 12)
            METHODMAP = (1 << 13)
            TYPEDEF = (1 << 14)
            VARIABLE = (1 << 15)
            PUBLICVAR = (1 << 16)
            [PROPERTY] = (1 << 17)
            [FUNCTION] = (1 << 18)
            STRUCT = (1 << 19)
            PREPROCESSOR = (1 << 20)
            [OPERATOR] = (1 << 21)
            ENUM_STRUCT = (1 << 22)
            FIELD = (1 << 23)
            INLINE_METHOD = (1 << 24)
            COMMAND = (1 << 25)
        End Enum

        Public Shared Function ParseTypeFullNames(sStr As String) As ENUM_TYPE_FLAGS
            Return ParseTypeNames(sStr.Split(New String() {" "}, 0))
        End Function

        Public Shared Function ParseTypeNames(sStr As String()) As ENUM_TYPE_FLAGS
            Dim mTypes As ENUM_TYPE_FLAGS = ENUM_TYPE_FLAGS.NONE

            For i = 0 To sStr.Length - 1
                Select Case (sStr(i))
                    Case "debug" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.DEBUG)
                    Case "define" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.DEFINE)
                    Case "enum" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.ENUM)
                    Case "struct" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.STRUCT)
                    Case "enum-struct" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.ENUM_STRUCT)
                    Case "methodmap" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.METHODMAP)
                    Case "funcenum" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FUNCENUM)
                    Case "functag" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FUNCTAG)
                    Case "typeset" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.TYPESET)
                    Case "typedef" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.TYPEDEF)
                    Case "stock" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.STOCK)
                    Case "static" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.STATIC)
                    Case "const" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.CONST)
                    Case "public" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PUBLIC)
                    Case "native" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.NATIVE)
                    Case "forward" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FORWARD)
                    Case "method" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.METHOD)
                    Case "property" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PROPERTY)
                    Case "variable" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.VARIABLE)
                    Case "publicvar" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PUBLICVAR)
                    Case "function" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FUNCTION)
                    Case "preprocessor" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PREPROCESSOR)
                    Case "operator" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.OPERATOR)
                    Case "field" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FIELD)
                    Case "inline-method" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.INLINE_METHOD)
                    Case "command" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.COMMAND)
                End Select
            Next

            Return mTypes
        End Function

        Public Function GetTypeNames() As String()
            Dim lNames As New List(Of String)

            If ((g_iType And ENUM_TYPE_FLAGS.DEBUG) <> 0) Then lNames.Add("debug")
            If ((g_iType And ENUM_TYPE_FLAGS.DEFINE) <> 0) Then lNames.Add("define")
            If ((g_iType And ENUM_TYPE_FLAGS.ENUM) <> 0) Then lNames.Add("enum")
            If ((g_iType And ENUM_TYPE_FLAGS.STRUCT) <> 0) Then lNames.Add("struct")
            If ((g_iType And ENUM_TYPE_FLAGS.ENUM_STRUCT) <> 0) Then lNames.Add("enum-struct")
            If ((g_iType And ENUM_TYPE_FLAGS.METHODMAP) <> 0) Then lNames.Add("methodmap")
            If ((g_iType And ENUM_TYPE_FLAGS.FUNCENUM) <> 0) Then lNames.Add("funcenum")
            If ((g_iType And ENUM_TYPE_FLAGS.FUNCTAG) <> 0) Then lNames.Add("functag")
            If ((g_iType And ENUM_TYPE_FLAGS.TYPESET) <> 0) Then lNames.Add("typeset")
            If ((g_iType And ENUM_TYPE_FLAGS.TYPEDEF) <> 0) Then lNames.Add("typedef")
            If ((g_iType And ENUM_TYPE_FLAGS.STOCK) <> 0) Then lNames.Add("stock")
            If ((g_iType And ENUM_TYPE_FLAGS.STATIC) <> 0) Then lNames.Add("static")
            If ((g_iType And ENUM_TYPE_FLAGS.CONST) <> 0) Then lNames.Add("const")
            If ((g_iType And ENUM_TYPE_FLAGS.PUBLIC) <> 0) Then lNames.Add("public")
            If ((g_iType And ENUM_TYPE_FLAGS.NATIVE) <> 0) Then lNames.Add("native")
            If ((g_iType And ENUM_TYPE_FLAGS.FORWARD) <> 0) Then lNames.Add("forward")
            If ((g_iType And ENUM_TYPE_FLAGS.METHOD) <> 0) Then lNames.Add("method")
            If ((g_iType And ENUM_TYPE_FLAGS.PROPERTY) <> 0) Then lNames.Add("property")
            If ((g_iType And ENUM_TYPE_FLAGS.VARIABLE) <> 0) Then lNames.Add("variable")
            If ((g_iType And ENUM_TYPE_FLAGS.PUBLICVAR) <> 0) Then lNames.Add("publicvar")
            If ((g_iType And ENUM_TYPE_FLAGS.FUNCTION) <> 0) Then lNames.Add("function")
            If ((g_iType And ENUM_TYPE_FLAGS.PREPROCESSOR) <> 0) Then lNames.Add("preprocessor")
            If ((g_iType And ENUM_TYPE_FLAGS.OPERATOR) <> 0) Then lNames.Add("operator")
            If ((g_iType And ENUM_TYPE_FLAGS.FIELD) <> 0) Then lNames.Add("field")
            If ((g_iType And ENUM_TYPE_FLAGS.INLINE_METHOD) <> 0) Then lNames.Add("inline-method")
            If ((g_iType And ENUM_TYPE_FLAGS.COMMAND) <> 0) Then lNames.Add("command")

            Return lNames.ToArray
        End Function

        Public Function GetTypeFullNames() As String
            Return String.Join(" ", GetTypeNames())
        End Function
    End Class


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
        g_ClassSyntaxHighlighting.RefreshSyntax()

        For Each c As Form In Application.OpenForms
            ClassControlStyle.UpdateControls(c)
        Next

        g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnFormColorUpdate())
    End Sub

    ''' <summary>
    ''' Gets the expression between characters e.g if "()": MyFunc(MyArgs) => MyArgs
    ''' </summary>
    ''' <param name="sExpression"></param>
    ''' <param name="sCharOpen"></param>
    ''' <param name="sCharClose"></param>
    ''' <param name="iTargetEndScopeLevel"></param>
    ''' <param name="bInvalidCodeCheck">If true, it will ignore preprocessor directives and all non-code like strings, comments etc.</param>
    ''' <returns></returns>
    Public Function GetExpressionBetweenCharacters(sExpression As String, sCharOpen As Char, sCharClose As Char, iTargetEndScopeLevel As Integer, iLanguage As ENUM_LANGUAGE_TYPE, Optional bInvalidCodeCheck As Boolean = False) As Integer()()
        Dim iCurrentLevel As Integer = 0
        Dim sExpressionsList As New List(Of Integer())
        Dim bWasOpen As Boolean = False

        Dim iStartLoc As Integer = 0

        Dim mSourceAnalysis As ClassSyntaxSourceAnalysis = Nothing
        If (bInvalidCodeCheck) Then
            mSourceAnalysis = New ClassSyntaxSourceAnalysis(sExpression, iLanguage)
        End If

        For i = 0 To sExpression.Length - 1
            If (mSourceAnalysis IsNot Nothing AndAlso bInvalidCodeCheck) Then
                If (mSourceAnalysis.m_InNonCode(i)) Then
                    Continue For
                End If

                'Ignore possible malformed preprocessor directives.
                If (mSourceAnalysis.m_InPreprocessor(i)) Then
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
    ''' Automatically indents the source.
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <returns></returns>
    Public Function FormatCodeIndentation(sSource As String, iIndentationType As ClassSettings.ENUM_INDENTATION_TYPES, iLanguage As ENUM_LANGUAGE_TYPE) As String
        If (True) Then
            Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, iLanguage)
            Dim mSourceBuilder As New StringBuilder(sSource)

            'Trim, but skip comments
            For i = mSourceBuilder.Length - 1 - 1 To 0 Step -1
                Dim a As Byte
                If (i = 0) Then
                    a = 0
                ElseIf (mSourceBuilder(i) = vbLf(0)) Then
                    a = 1
                Else
                    Continue For
                End If

                If (mSourceAnalysis.m_InMultiComment(i) OrElse mSourceAnalysis.m_InSingleComment(i)) Then
                    Continue For
                End If

                For j = i + a To mSourceBuilder.Length - 1
                    If (mSourceAnalysis.m_InMultiComment(j) OrElse mSourceAnalysis.m_InSingleComment(j)) Then
                        Exit For
                    End If

                    If (mSourceBuilder(j) = vbCr OrElse mSourceBuilder(j) = vbLf OrElse Not Char.IsWhiteSpace(mSourceBuilder(j))) Then
                        mSourceBuilder = mSourceBuilder.Remove(i + a, j - (i + a))
                        Exit For
                    End If
                Next
            Next

            sSource = mSourceBuilder.ToString
        End If

        If (True) Then
            Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, iLanguage)
            Dim mSourceBuilder As New StringBuilder(sSource)

            Dim iBraceCount As Integer = 0
            Dim iBracedCount As Integer = 0

            For i = mSourceBuilder.Length - 1 - 1 To 0 Step -1
                Try
                    Select Case (mSourceBuilder(i))
                        Case "("c
                            If (Not mSourceAnalysis.m_InNonCode(i)) Then
                                iBracedCount -= 1
                            End If

                        Case ")"c
                            If (Not mSourceAnalysis.m_InNonCode(i)) Then
                                iBracedCount += 1
                            End If

                        Case "{"c
                            If (Not mSourceAnalysis.m_InNonCode(i)) Then
                                iBraceCount -= 1
                            End If

                        Case "}"c
                            If (Not mSourceAnalysis.m_InNonCode(i)) Then
                                iBraceCount += 1
                            End If

                        Case Else
                            If (i = 0 OrElse mSourceBuilder(i) = vbLf(0)) Then
                                Dim a As Byte
                                If (i = 0) Then
                                    a = 0
                                Else
                                    a = 1
                                End If

                                Dim bContinue As Boolean = False

                                'Dont indent comments
                                For j = i + a To mSourceBuilder.Length - 1
                                    If (mSourceAnalysis.m_InMultiComment(j) OrElse mSourceAnalysis.m_InSingleComment(j)) Then
                                        bContinue = True
                                        Exit For
                                    End If

                                    If (mSourceBuilder(j) = vbCr OrElse mSourceBuilder(j) = vbLf OrElse Not Char.IsWhiteSpace(mSourceBuilder(j))) Then
                                        Exit For
                                    End If
                                Next
                                If (bContinue) Then
                                    Continue For
                                End If

                                Dim iPreprocessorLevel As Integer = 0

                                'Dont indent preprocessor. Only after escape-newline.
                                For j = i + a To mSourceBuilder.Length - 1
                                    If (mSourceAnalysis.m_InPreprocessor(j)) Then
                                        If (mSourceBuilder(j) = "#"c) Then
                                            bContinue = True
                                            Exit For
                                        End If

                                        iPreprocessorLevel = 1
                                        Exit For
                                    End If

                                    If (mSourceBuilder(j) = vbCr OrElse mSourceBuilder(j) = vbLf OrElse Not Char.IsWhiteSpace(mSourceBuilder(j))) Then
                                        Exit For
                                    End If
                                Next
                                If (bContinue) Then
                                    Continue For
                                End If

                                'Finish up
                                Dim iBraceRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                                Dim iBraceLevel As Integer = mSourceAnalysis.GetBraceLevel(i + 1 + iBraceCount, iBraceRange)
                                Select Case (iBraceRange)
                                    Case ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.START, ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END
                                        iBraceLevel -= 1
                                End Select

                                'Add indentation 
                                Dim iIndentLength As Integer = iBraceLevel

                                If (iBracedCount > 0) Then
                                    iIndentLength += (iBracedCount + 1)
                                End If

                                If (iPreprocessorLevel > 0) Then
                                    iIndentLength = 1
                                End If

                                If (iIndentLength < 0) Then
                                    iIndentLength = 0
                                End If

                                mSourceBuilder = mSourceBuilder.Insert(i + 1, ClassSettings.BuildIndentation(iIndentLength, iIndentationType))

                                iBraceCount = 0
                            End If

                    End Select
                Catch ex As Exception
                    ' Ignore random errors
                End Try
            Next

            sSource = mSourceBuilder.ToString
        End If

        Return sSource
    End Function

    ''' <summary>
    ''' Trims all ending whitespace from the source.
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <returns></returns>
    Public Function FormatCodeTrimEnd(sSource As String) As String
        Dim mSourceBuilder As New StringBuilder

        Using mSR As New IO.StringReader(sSource)
            While True
                Dim sLine As String = mSR.ReadLine
                If (sLine Is Nothing) Then
                    Exit While
                End If

                mSourceBuilder.AppendLine(sLine.TrimEnd)
            End While
        End Using

        Return mSourceBuilder.ToString
    End Function

    ''' <summary>
    ''' Converts tabs to spaces or mirrored.
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <param name="iIndentationType"></param>
    ''' <param name="iLength"></param>
    ''' <returns></returns>
    Public Function FormatCodeConvert(sSource As String, iIndentationType As ClassSettings.ENUM_INDENTATION_TYPES, iLength As Integer) As String
        Dim mSourceBuilder As New StringBuilder

        Using mSR As New IO.StringReader(sSource)
            While True
                Dim sLine As String = mSR.ReadLine
                If (sLine Is Nothing) Then
                    Exit While
                End If

                If (iLength < 1) Then
                    iLength = 4
                End If

                Select Case (iIndentationType)
                    Case ClassSettings.ENUM_INDENTATION_TYPES.USE_SETTINGS
                        If (ClassSettings.g_iSettingsTabsToSpaces > 0) Then
                            sLine = sLine.Replace(vbTab, New String(" "c, ClassSettings.g_iSettingsTabsToSpaces))
                        Else
                            sLine = sLine.Replace(New String(" "c, 4), vbTab)
                        End If

                    Case ClassSettings.ENUM_INDENTATION_TYPES.TABS
                        sLine = sLine.Replace(New String(" "c, iLength), vbTab)

                    Case ClassSettings.ENUM_INDENTATION_TYPES.SPACES
                        sLine = sLine.Replace(vbTab, New String(" "c, iLength))

                    Case Else
                        Throw New ArgumentException("Invalid indentation type")

                End Select

                mSourceBuilder.AppendLine(sLine)
            End While
        End Using

        Return mSourceBuilder.ToString
    End Function

    ''' <summary>
    ''' Checks if the source requires new decls and returns the offset.
    ''' NOTE: High false positive rate.
    ''' Returns -1 if not found.
    ''' </summary>
    ''' <param name="sSource"></param>
    ''' <param name="bIgnoreChecks"></param>
    ''' <returns>The offset in the source, -1 if not found.</returns>
    Public Function HasNewDeclsPragma(sSource As String, iLanguage As ENUM_LANGUAGE_TYPE, Optional bIgnoreChecks As Boolean = True) As Integer
        'TODO: Add better check
        Dim sRegexPattern As String = "\#\b(pragma)\b(\s+|\s*(\\*)\s*)\b(newdecls)\b(\s+|\s*(\\*)\s*)\b(required)\b"

        If (bIgnoreChecks) Then
            For Each match As Match In Regex.Matches(sSource, sRegexPattern, RegexOptions.Multiline)
                Return match.Index
            Next
        Else
            Dim mSourceAnalysis As New ClassSyntaxSourceAnalysis(sSource, iLanguage)
            For Each match As Match In Regex.Matches(sSource, sRegexPattern, RegexOptions.Multiline)
                If (mSourceAnalysis.m_InNonCode(match.Index)) Then
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
        Static sBadNames As String() = New String() {
                "const",
                "static",
                "new",
                "decl",
                "if",
                "for",
                "else",
                "case",
                "switch",
                "default",
                "while",
                "do",
                "enum",
 _
                "stock",
                "public",
                "private",
                "forward",
                "native",
                "funcenum",
                "functag",
 _
                "methodmap",
                "property",
                "this",
                "typeset",
                "function",
                "typedef",
                "using",
 _
                "break",
                "continue",
                "goto",
                "return",
 _
                "true",
                "false",
                "null",
 _
                "delete",
                "sizeof",
                "typeof",
                "view_as"
            }

        Return Array.Exists(sBadNames, Function(s As String) s = sName)
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

        Enum ENUM_STATE_RANGE
            NONE
            START
            [END]
        End Enum

        Private g_iStateArray As Integer(,)
        Private g_iMaxLength As Integer = 0
        Private g_sCacheText As String = ""

        Public ReadOnly Property m_InRange(i As Integer) As Boolean
            Get
                Return (i > -1 AndAlso i < g_iStateArray.Length)
            End Get
        End Property

        ''' <summary>
        ''' Gets the max length
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property m_MaxLength() As Integer
            Get
                Return g_iMaxLength
            End Get
        End Property

        ''' <summary>
        ''' If char index is in single-comment
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property m_InSingleComment(i As Integer) As Boolean
            Get
                Return g_iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) > 0
            End Get
        End Property

        ''' <summary>
        ''' If char index is in multi-comment
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property m_InMultiComment(i As Integer) As Boolean
            Get
                Return g_iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) > 0
            End Get
        End Property

        ''' <summary>
        ''' If char index is in string
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property m_InString(i As Integer) As Boolean
            Get
                Return g_iStateArray(i, ENUM_STATE_TYPES.IN_STRING) > 0
            End Get
        End Property

        ''' <summary>
        ''' If char index is in char
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property m_InChar(i As Integer) As Boolean
            Get
                Return g_iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) > 0
            End Get
        End Property

        ''' <summary>
        ''' If char index is in Preprocessor (#define, #pragma etc.)
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property m_InPreprocessor(i As Integer) As Boolean
            Get
                Return g_iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) > 0
            End Get
        End Property

        ''' <summary>
        ''' It will return true if the char index is in a string, char, single- or multi-comment.
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public ReadOnly Property m_InNonCode(i As Integer) As Boolean
            Get
                Return (g_iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) > 0 OrElse
                            g_iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) > 0 OrElse
                            g_iStateArray(i, ENUM_STATE_TYPES.IN_STRING) > 0 OrElse
                            g_iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) > 0)
            End Get
        End Property

        ''' <summary>
        ''' Get current parenthesis "(" or ")" level
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public Function GetParenthesisLevel(i As Integer, ByRef iRange As ENUM_STATE_RANGE) As Integer
            iRange = ENUM_STATE_RANGE.NONE

            If (g_iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) > 0) Then
                Select Case (g_sCacheText(i))
                    Case "("c
                        iRange = ENUM_STATE_RANGE.START

                    Case ")"c
                        iRange = ENUM_STATE_RANGE.END
                End Select
            End If

            Return g_iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL)
        End Function

        ''' <summary>
        ''' Get current bracket "[" or "]" level
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public Function GetBracketLevel(i As Integer, ByRef iRange As ENUM_STATE_RANGE) As Integer
            iRange = ENUM_STATE_RANGE.NONE

            If (g_iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) > 0) Then
                Select Case (g_sCacheText(i))
                    Case "["c
                        iRange = ENUM_STATE_RANGE.START

                    Case "]"c
                        iRange = ENUM_STATE_RANGE.END
                End Select
            End If

            Return g_iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL)
        End Function

        ''' <summary>
        ''' Get current brace "{" or "}" level
        ''' </summary>
        ''' <param name="i">The char index</param>
        ''' <returns></returns>
        Public Function GetBraceLevel(i As Integer, ByRef iRange As ENUM_STATE_RANGE) As Integer
            iRange = ENUM_STATE_RANGE.NONE

            If (g_iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) > 0) Then
                Select Case (g_sCacheText(i))
                    Case "{"c
                        iRange = ENUM_STATE_RANGE.START

                    Case "}"c
                        iRange = ENUM_STATE_RANGE.END
                End Select
            End If

            Return g_iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL)
        End Function

        Public Function GetCachedText() As String
            Return g_sCacheText
        End Function

        Public Function GetIndexFromLine(iLine As Integer) As Integer
            If (iLine = 0) Then
                Return 0
            End If

            Dim iLineCount As Integer = 0
            For i = 0 To g_sCacheText.Length - 1
                Select Case (g_sCacheText(i))
                    Case vbLf(0)
                        iLineCount += 1

                        If (iLineCount >= iLine) Then
                            If (i + 1 > g_sCacheText.Length - 1) Then
                                'Get the length even if its longer than the g_sCacheTest length.
                                'g_iStateArray should have +1 more length.
                                Return g_sCacheText.Length
                            Else
                                Return i + 1
                            End If
                        End If
                End Select
            Next

            Return -1
        End Function

        ''' <summary>
        ''' Analizes source.
        ''' </summary>
        ''' <param name="sText"></param>
        ''' <param name="iLanguage"></param>
        ''' <param name="bIgnorePreprocessor">Doesnt analize preprocessor directives if true, false otherwise.</param>
        Public Sub New(sText As String, iLanguage As ENUM_LANGUAGE_TYPE, Optional bIgnorePreprocessor As Boolean = False)
            g_sCacheText = sText
            g_iMaxLength = sText.Length

            'Init g_iStateArray with +1 index.
            g_iStateArray = New Integer(sText.Length, [Enum].GetNames(GetType(ENUM_STATE_TYPES)).Length - 1) {}

            Dim iParenthesisLevel As Integer = 0 '() 
            Dim iBracketLevel As Integer = 0 '[] 
            Dim iBraceLevel As Integer = 0 '{} 
            Dim iParenthesisLevelPreSave As Integer = 0 '() 'Save/load before/after preprocessor
            Dim iBracketLevelPreSave As Integer = 0 '[] 'Save/load before/after preprocessor
            Dim iBraceLevelPreSave As Integer = 0 '{} 'Save/load before/after preprocessor
            Dim bInSingleComment As Integer = 0
            Dim bInMultiComment As Integer = 0
            Dim bInString As Integer = 0
            Dim bInChar As Integer = 0
            Dim bInPreprocessor As Integer = 0

            For i = 0 To sText.Length - 1
                g_iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) = iParenthesisLevel
                g_iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel
                g_iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel
                g_iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                g_iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = bInMultiComment
                g_iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = bInString
                g_iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) = bInChar
                g_iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = bInPreprocessor

                Select Case (sText(i))
                    Case "#"c
                        If (iParenthesisLevel > 0 OrElse iBracketLevel > 0 OrElse bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        If (bInPreprocessor < 1) Then
                            'Preprocessor directives can be malformed, reset level after we are done with it.
                            iParenthesisLevelPreSave = iParenthesisLevel
                            iBracketLevelPreSave = iBracketLevel
                            iBraceLevelPreSave = iBraceLevel
                        End If

                        bInPreprocessor = 1
                        g_iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = 1

                    Case "*"c
                        If (bInSingleComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        '/*
                        If (i > 0) Then
                            If (sText(i - 1) = "/"c AndAlso bInMultiComment < 1) Then
                                bInMultiComment = 1
                                g_iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = 1
                                g_iStateArray(i - 1, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = 1
                                Continue For
                            End If
                        End If

                        If (i + 1 < sText.Length - 1) Then
                            If (sText(i + 1) = "/"c AndAlso bInMultiComment > 0) Then
                                bInMultiComment = 0
                                g_iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = 1
                                g_iStateArray(i + 1, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = 1

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
                                g_iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                            End If
                        End If

                    Case "("c
                        If (bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        iParenthesisLevel += 1
                        g_iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) = iParenthesisLevel

                    Case ")"c
                        If (bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        g_iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) = iParenthesisLevel
                        iParenthesisLevel -= 1

                    Case "["c
                        If (bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        iBracketLevel += 1
                        g_iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel

                    Case "]"c
                        If (bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        g_iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel
                        iBracketLevel -= 1

                    Case "{"c
                        If (bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        iBraceLevel += 1
                        g_iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel

                    Case "}"c
                        If (bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        g_iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel
                        iBraceLevel -= 1

                    Case "'"c
                        If (bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0) Then
                            Continue For
                        End If

                        'ignore \'
                        Dim iEscapes As Integer = 0
                        For j = i - 1 To 0 Step -1
                            If (sText(j) <> ClassSyntaxTools.g_sEscapeCharacters(iLanguage)) Then
                                Exit For
                            End If

                            iEscapes += 1
                        Next

                        If ((iEscapes Mod 2) = 0) Then
                            bInChar = If(bInChar > 0, 0, 1)
                            g_iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) = 1
                        End If

                    Case """"c
                        If (bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                            Continue For
                        End If

                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        'ignore \"
                        Dim iEscapes As Integer = 0
                        For j = i - 1 To 0 Step -1
                            If (sText(j) <> ClassSyntaxTools.g_sEscapeCharacters(iLanguage)) Then
                                Exit For
                            End If

                            iEscapes += 1
                        Next

                        If ((iEscapes Mod 2) = 0) Then
                            bInString = If(bInString > 0, 0, 1)
                            g_iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = 1
                        End If

                    Case vbLf(0)
                        Dim bEscapeNewline As Boolean = False

                        'Detect escape newline
                        For j = i - 1 To 0 Step -1
                            If (sText(j) = vbLf(0)) Then
                                Exit For
                            End If

                            If (Char.IsWhiteSpace(sText(j))) Then
                                Continue For
                            End If

                            If (sText(j) = "\"c) Then
                                bEscapeNewline = True
                                Exit For
                            End If

                            Exit For
                        Next

                        If (bInSingleComment > 0) Then
                            bInSingleComment = 0
                            g_iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                        End If

                        If (bInPreprocessor > 0 AndAlso Not bEscapeNewline) Then
                            bInPreprocessor = 0
                            g_iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = bInPreprocessor

                            'Preprocessor directives can be malformed, reset level after we are done with it.
                            iParenthesisLevel = iParenthesisLevelPreSave
                            iBracketLevel = iBracketLevelPreSave
                            iBraceLevel = iBraceLevelPreSave
                        End If

                    Case Else
                        g_iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) = iParenthesisLevel
                        g_iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel
                        g_iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel
                        g_iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                        g_iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = bInMultiComment
                        g_iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = bInString
                        g_iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) = bInChar
                        g_iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = bInPreprocessor
                End Select
            Next
        End Sub
    End Class

    Class ClassSyntaxHighlighting
        Public g_ClassSyntaxTools As ClassSyntaxTools

        Private g_sCustomSyntaxText As String = ""
        Private _lock As Object = New Object()

        Enum ENUM_SYNTAX_EDITORS
            MAIN_TEXTEDITOR
            DEBUGGER_SOURCE_TEXTEDITOR
        End Enum

        Class STRUC_SYNTAX_ITEM
            Public sName As String

            Public mSyntaxMode As SyntaxMode
            Public sSyntaxText As String

            Public Sub New(_Name As String)
                sName = _Name

                mSyntaxMode = New SyntaxMode(String.Format("{0}.sp", Guid.NewGuid.ToString), sName, ".sp")
                sSyntaxText = ""
            End Sub

            Public Function ToXmlReader() As XmlTextReader
                Return New XmlTextReader(New IO.StringReader(sSyntaxText))
            End Function
        End Class
        Public Shared g_mTextEditorSyntaxItems As STRUC_SYNTAX_ITEM()

        Public Sub New(mClassSyntaxTools As ClassSyntaxTools)
            g_ClassSyntaxTools = mClassSyntaxTools

            g_mTextEditorSyntaxItems = {
                New STRUC_SYNTAX_ITEM("SourcePawn-MainTextEditor-" & Guid.NewGuid.ToString),
                New STRUC_SYNTAX_ITEM("SourcePawn-DebuggerSourceTextEditor-" & Guid.NewGuid.ToString)
            }

            If (g_mTextEditorSyntaxItems.Length <> [Enum].GetNames(GetType(ENUM_SYNTAX_EDITORS)).Length) Then
                Throw New ArgumentException("Invalid syntax item size")
            End If
        End Sub

        Public Sub RefreshCustomSyntax()
            If (Not String.IsNullOrEmpty(ClassSettings.g_sSettingsSyntaxHighlightingPath) AndAlso
                           IO.File.Exists(ClassSettings.g_sSettingsSyntaxHighlightingPath) AndAlso
                           IO.Path.GetExtension(ClassSettings.g_sSettingsSyntaxHighlightingPath).ToLower = ".xml") Then
                g_sCustomSyntaxText = IO.File.ReadAllText(ClassSettings.g_sSettingsSyntaxHighlightingPath)
            Else
                g_sCustomSyntaxText = ""
            End If
        End Sub

        Public Sub RenewSyntax()
            For i = 0 To g_mTextEditorSyntaxItems.Length - 1
                RenewSyntax(CType(i, ENUM_SYNTAX_EDITORS))
            Next
        End Sub

        Public Sub RenewSyntax(i As ENUM_SYNTAX_EDITORS)
            Try
                SyncLock _lock
                    Dim sModSyntaxXML As String
                    If (Not String.IsNullOrEmpty(g_sCustomSyntaxText)) Then
                        sModSyntaxXML = g_sCustomSyntaxText.Replace(g_sSyntaxSourcePawnMarker, g_mTextEditorSyntaxItems(i).sName)
                    Else
                        If (ClassSettings.g_iSettingsInvertColors) Then
                            sModSyntaxXML = g_sSyntaxDarkXML.Replace(g_sSyntaxSourcePawnMarker, g_mTextEditorSyntaxItems(i).sName)
                        Else
                            sModSyntaxXML = g_sSyntaxXML.Replace(g_sSyntaxSourcePawnMarker, g_mTextEditorSyntaxItems(i).sName)
                        End If
                    End If

                    'Cleanup
                    sModSyntaxXML = Regex.Replace(sModSyntaxXML, "^\s*", "", RegexOptions.Multiline)
                    sModSyntaxXML = Regex.Replace(sModSyntaxXML, "\s*$", "", RegexOptions.Multiline)

                    g_mTextEditorSyntaxItems(i).sSyntaxText = sModSyntaxXML
                End SyncLock
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Public Sub RefreshSyntax()
            RefreshCustomSyntax()

            UpdateSyntax()
            UpdateTextEditorSyntax()
        End Sub

        Public Sub UpdateSyntax()
            For j = 0 To [Enum].GetNames(GetType(ENUM_SYNTAX_UPDATE_TYPE)).Length - 1
                UpdateSyntax(CType(j, ENUM_SYNTAX_UPDATE_TYPE), j = 0)
            Next
        End Sub

        ''' <summary>
        ''' Checks the syntax version. If true, the syntax file requires an update, false otherwise.
        ''' </summary>
        ''' <param name="r_CurrentVersion"></param>
        ''' <param name="r_SyntaxVersion"></param>
        ''' <returns></returns>
        Public Function CheckSyntaxVersion(ByRef r_CurrentVersion As Version, ByRef r_SyntaxVersion As Version) As Boolean
            r_CurrentVersion = g_mSyntaxRequiredVersion
            r_SyntaxVersion = New Version(0, 0)

            Dim mVersion = GetSyntaxVersion()
            If (mVersion Is Nothing) Then
                Return False
            End If

            r_SyntaxVersion = mVersion

            If (mVersion = New Version(0, 0) OrElse g_mSyntaxRequiredVersion > mVersion) Then
                Return True
            End If

            Return False
        End Function

        Public Function GetSyntaxVersion() As Version
            If (Not String.IsNullOrEmpty(ClassSettings.g_sSettingsSyntaxHighlightingPath) AndAlso
                       IO.File.Exists(ClassSettings.g_sSettingsSyntaxHighlightingPath) AndAlso
                       IO.Path.GetExtension(ClassSettings.g_sSettingsSyntaxHighlightingPath).ToLower = ".xml") Then
                Dim sContent = IO.File.ReadAllText(ClassSettings.g_sSettingsSyntaxHighlightingPath)
                Dim mMatch As Match = Regex.Match(sContent, "\<\!\-\-\s*\<SyntaxVersion\>\s*(?<Version>[0-9\.]+)\s*\<\/SyntaxVersion\>\s*\-\-\>", RegexOptions.IgnoreCase Or RegexOptions.Multiline)

                If (mMatch.Groups("Version").Success) Then
                    Return New Version(mMatch.Groups("Version").Value)
                Else
                    Return New Version(0, 0)
                End If
            Else
                Return Nothing
            End If
        End Function

        Public Sub UpdateSyntax(iType As ENUM_SYNTAX_UPDATE_TYPE, Optional bRenew As Boolean = False)
            Try
                g_ClassSyntaxTools.g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnSyntaxUpdate(iType, bRenew))

                SyncLock _lock
                    For i = 0 To g_mTextEditorSyntaxItems.Length - 1
                        If (String.IsNullOrEmpty(g_mTextEditorSyntaxItems(i).sSyntaxText) OrElse bRenew) Then
                            RenewSyntax(CType(i, ENUM_SYNTAX_EDITORS))
                        End If

                        Select Case (i)
                            Case ENUM_SYNTAX_EDITORS.MAIN_TEXTEDITOR, ENUM_SYNTAX_EDITORS.DEBUGGER_SOURCE_TEXTEDITOR
                                Dim mXmlBuilder As New StringBuilder

                                Dim iHighlightCustomCount As Integer = 0

                                Dim mActiveTab As ClassTabControl.SourceTabPage = g_ClassSyntaxTools.g_mFormMain.g_ClassTabControl.m_ActiveTab
                                Dim mActiveAutocomplete As STRUC_AUTOCOMPLETE() = mActiveTab.m_AutocompleteItems.ToArray
                                Dim iLanguage As ENUM_LANGUAGE_TYPE = mActiveTab.m_Language

                                Using mSR As New IO.StringReader(g_mTextEditorSyntaxItems(i).sSyntaxText)
                                    Dim sLine As String

                                    While True
                                        sLine = mSR.ReadLine
                                        If (sLine Is Nothing) Then
                                            Exit While
                                        End If

                                        Select Case (iType)
                                            Case ENUM_SYNTAX_UPDATE_TYPE.CARET_WORD
                                                If (i = ENUM_SYNTAX_EDITORS.MAIN_TEXTEDITOR) Then
                                                    If (sLine.Contains(g_sSyntaxHighlightCaretMarker)) Then

                                                        mXmlBuilder.Append(g_sSyntaxHighlightCaretMarker)

                                                        If (Not String.IsNullOrEmpty(g_sCaretWord) AndAlso ClassSettings.g_iSettingsAutoMark) Then
                                                            mXmlBuilder.AppendFormat("<Key word=""{0}""/>", g_sCaretWord)
                                                        End If

                                                        mXmlBuilder.AppendLine()
                                                        Continue While
                                                    End If
                                                End If

                                            Case ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD
                                                If (i = ENUM_SYNTAX_EDITORS.MAIN_TEXTEDITOR) Then
                                                    If (sLine.Contains(g_sSyntaxHighlightWordMarker)) Then

                                                        mXmlBuilder.Append(g_sSyntaxHighlightWordMarker)

                                                        If (Not String.IsNullOrEmpty(g_sHighlightWord)) Then
                                                            mXmlBuilder.AppendFormat("<Key word=""{0}""/>", g_sHighlightWord)
                                                        End If

                                                        mXmlBuilder.AppendLine()
                                                        Continue While
                                                    End If
                                                End If

                                            Case ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD_CUSTOM
                                                If (i = ENUM_SYNTAX_EDITORS.MAIN_TEXTEDITOR) Then
                                                    If (sLine.Contains(g_sSyntaxHighlightWordCustomMarker)) Then
                                                        mXmlBuilder.Append(g_sSyntaxHighlightWordCustomMarker)

                                                        g_ClassSyntaxTools.g_mFormMain.g_ClassCustomHighlighting.Add(iHighlightCustomCount)
                                                        Dim menuItem = g_ClassSyntaxTools.g_mFormMain.g_ClassCustomHighlighting.m_HightlightItems()

                                                        If (menuItem(iHighlightCustomCount) IsNot Nothing AndAlso Not String.IsNullOrEmpty(menuItem(iHighlightCustomCount).sWord)) Then
                                                            mXmlBuilder.AppendFormat("<Key word=""{0}""/>", menuItem(iHighlightCustomCount).sWord)
                                                        End If

                                                        mXmlBuilder.AppendLine()
                                                        iHighlightCustomCount += 1
                                                        Continue While
                                                    End If
                                                End If

                                            Case ENUM_SYNTAX_UPDATE_TYPE.AUTOCOMPLETE
                                                If (i = ENUM_SYNTAX_EDITORS.MAIN_TEXTEDITOR OrElse i = ENUM_SYNTAX_EDITORS.DEBUGGER_SOURCE_TEXTEDITOR) Then
                                                    If (sLine.Contains(g_sSyntaxHighlightDefineMarker)) Then
                                                        mXmlBuilder.Append(g_sSyntaxHighlightDefineMarker)

                                                        For Each mAutocomplete In mActiveAutocomplete
                                                            Select Case (True)
                                                                Case (mAutocomplete.m_Type And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE) <> 0,
                                                                     (ClassSettings.g_iSettingsPublicAsDefineColor AndAlso (mAutocomplete.m_Type And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR) <> 0)
                                                                    mXmlBuilder.AppendFormat("<Key word=""{0}""/>", mAutocomplete.m_FunctionString)

                                                            End Select
                                                        Next

                                                        mXmlBuilder.AppendLine()
                                                        Continue While
                                                    End If

                                                    If (sLine.Contains(g_sSyntaxHighlightEnumMarker)) Then
                                                        mXmlBuilder.Append(g_sSyntaxHighlightEnumMarker)

                                                        Dim lExistList As New List(Of String)

                                                        For Each mAutocomplete In mActiveAutocomplete
                                                            Select Case (True)
                                                                Case (mAutocomplete.m_Type And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) <> 0
                                                                    Dim sEnumName As String() = mAutocomplete.m_FunctionString.Split("."c)
                                                                    Select Case (sEnumName.Length)
                                                                        Case 2
                                                                            If (Not lExistList.Contains(sEnumName(0))) Then
                                                                                lExistList.Add(sEnumName(0))
                                                                            End If

                                                                            mXmlBuilder.AppendFormat("<Key word=""{0}""/>", sEnumName(1))
                                                                    End Select

                                                                Case (mAutocomplete.m_Type And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> 0
                                                                    If (Not lExistList.Contains(mAutocomplete.m_FunctionString)) Then
                                                                        lExistList.Add(mAutocomplete.m_FunctionString)
                                                                    End If
                                                            End Select
                                                        Next

                                                        mXmlBuilder.AppendLine()
                                                        Continue While
                                                    End If

                                                    If (sLine.Contains(g_sSyntaxHighlightEnum2Marker)) Then
                                                        mXmlBuilder.Append(g_sSyntaxHighlightEnum2Marker)

                                                        Dim lExistList As New List(Of String)

                                                        For Each mAutocomplete In mActiveAutocomplete
                                                            Select Case (True)
                                                                Case (mAutocomplete.m_Type And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STRUCT) <> 0
                                                                    If (Not lExistList.Contains(mAutocomplete.m_FunctionString)) Then
                                                                        lExistList.Add(mAutocomplete.m_FunctionString)
                                                                    End If

                                                                Case (mAutocomplete.m_Type And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) <> 0
                                                                    Dim sEnumName As String() = mAutocomplete.m_FunctionString.Split("."c)
                                                                    Select Case (sEnumName.Length)
                                                                        Case 1, 2
                                                                            If (Not lExistList.Contains(sEnumName(0))) Then
                                                                                lExistList.Add(sEnumName(0))
                                                                            End If
                                                                    End Select

                                                                Case (mAutocomplete.m_Type And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> 0
                                                                    If (Not lExistList.Contains(mAutocomplete.m_FunctionString)) Then
                                                                        lExistList.Add(mAutocomplete.m_FunctionString)
                                                                    End If
                                                            End Select
                                                        Next

                                                        For Each s In lExistList
                                                            mXmlBuilder.AppendFormat("<Key word=""{0}""/>", s)
                                                        Next

                                                        mXmlBuilder.AppendLine()
                                                        Continue While
                                                    End If
                                                End If
                                        End Select

                                        If (Not String.IsNullOrEmpty(sLine.Trim)) Then
                                            mXmlBuilder.AppendLine(sLine.Trim)
                                        End If
                                    End While
                                End Using

                                Dim sFormatedString As String = mXmlBuilder.ToString

                                'Set escape characters
                                If (True) Then
                                    Dim mMatchColl As MatchCollection = Regex.Matches(sFormatedString, "\b(escapecharacter)\b\s*=\s*""(?<EscapeChar>(\^|\\))""")
                                    Dim mSyntaxBuilder As New StringBuilder(sFormatedString)

                                    For j = mMatchColl.Count - 1 To 0 Step -1
                                        If (Not mMatchColl(j).Success) Then
                                            Continue For
                                        End If

                                        Try
                                            Dim sEscapeChar As String = mMatchColl(j).Groups("EscapeChar").Value
                                            If (sEscapeChar = g_sEscapeCharacters(iLanguage)) Then
                                                Continue For
                                            End If

                                            Dim iEscapeCharIndex As Integer = mMatchColl(j).Groups("EscapeChar").Index

                                            mSyntaxBuilder = mSyntaxBuilder.Remove(iEscapeCharIndex, g_sEscapeCharacters(iLanguage).Length)
                                            mSyntaxBuilder = mSyntaxBuilder.Insert(iEscapeCharIndex, g_sEscapeCharacters(iLanguage))
                                        Catch : End Try
                                    Next

                                    sFormatedString = mSyntaxBuilder.ToString
                                End If

                                g_mTextEditorSyntaxItems(i).sSyntaxText = sFormatedString
                        End Select
                    Next
                End SyncLock

                g_ClassSyntaxTools.g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnSyntaxUpdateEnd(iType, bRenew))
            Catch ex As Threading.ThreadAbortException
                Throw
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Public Sub UpdateTextEditorSyntax()
            Try
                SyncLock _lock
                    g_ClassSyntaxTools.g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnEditorSyntaxUpdate())

                    For i = 0 To g_mTextEditorSyntaxItems.Length - 1
                        If (String.IsNullOrEmpty(g_mTextEditorSyntaxItems(i).sSyntaxText)) Then
                            RenewSyntax(CType(i, ENUM_SYNTAX_EDITORS))
                        End If
                    Next

                    HighlightingManager.Manager.ReloadSyntaxModes()

                    For i = 0 To g_mTextEditorSyntaxItems.Length - 1
                        Select Case (i)
                            Case ENUM_SYNTAX_EDITORS.MAIN_TEXTEDITOR
                                For j = 0 To g_ClassSyntaxTools.g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                                    If (j = g_ClassSyntaxTools.g_mFormMain.g_ClassTabControl.m_ActiveTabIndex) Then
                                        If (g_ClassSyntaxTools.g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.Document.HighlightingStrategy.Name <> g_mTextEditorSyntaxItems(i).sName) Then
                                            g_ClassSyntaxTools.g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.SetHighlighting(g_mTextEditorSyntaxItems(i).sName)
                                        End If
                                    Else
                                        If (g_ClassSyntaxTools.g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.Document.HighlightingStrategy.Name <> "Default") Then
                                            g_ClassSyntaxTools.g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.SetHighlighting("Default")
                                        End If
                                    End If
                                Next

                                If (g_ClassSyntaxTools.g_mFormMain.g_mFormToolTip.TextEditorControl_ToolTip.Document.HighlightingStrategy.Name <> g_mTextEditorSyntaxItems(i).sName) Then
                                    g_ClassSyntaxTools.g_mFormMain.g_mFormToolTip.TextEditorControl_ToolTip.SetHighlighting(g_mTextEditorSyntaxItems(i).sName)
                                    g_ClassSyntaxTools.g_mFormMain.g_mFormToolTip.TextEditorControl_ToolTip.Font = New Font(g_ClassSyntaxTools.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Font.FontFamily, 8, FontStyle.Regular)
                                End If

                                If (g_ClassSyntaxTools.g_mFormMain.g_mUCAutocomplete.TextEditorControlEx_IntelliSense.Document.HighlightingStrategy.Name <> g_mTextEditorSyntaxItems(i).sName) Then
                                    g_ClassSyntaxTools.g_mFormMain.g_mUCAutocomplete.TextEditorControlEx_IntelliSense.SetHighlighting(g_mTextEditorSyntaxItems(i).sName)
                                    g_ClassSyntaxTools.g_mFormMain.g_mUCAutocomplete.TextEditorControlEx_IntelliSense.Font = New Font(g_ClassSyntaxTools.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Font.FontFamily, 8, FontStyle.Regular)
                                End If

                                If (g_ClassSyntaxTools.g_mFormMain.g_mUCAutocomplete.TextEditorControlEx_Autocomplete.Document.HighlightingStrategy.Name <> g_mTextEditorSyntaxItems(i).sName) Then
                                    g_ClassSyntaxTools.g_mFormMain.g_mUCAutocomplete.TextEditorControlEx_Autocomplete.SetHighlighting(g_mTextEditorSyntaxItems(i).sName)
                                    g_ClassSyntaxTools.g_mFormMain.g_mUCAutocomplete.TextEditorControlEx_Autocomplete.Font = New Font(g_ClassSyntaxTools.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Font.FontFamily, 8, FontStyle.Regular)
                                End If

                            Case ENUM_SYNTAX_EDITORS.DEBUGGER_SOURCE_TEXTEDITOR
                                If (g_ClassSyntaxTools.g_mFormMain.g_mFormDebugger IsNot Nothing AndAlso Not g_ClassSyntaxTools.g_mFormMain.g_mFormDebugger.IsDisposed) Then
                                    If (g_ClassSyntaxTools.g_mFormMain.g_mFormDebugger.TextEditorControlEx_DebuggerSource.Document.HighlightingStrategy.Name <> g_mTextEditorSyntaxItems(i).sName) Then
                                        g_ClassSyntaxTools.g_mFormMain.g_mFormDebugger.TextEditorControlEx_DebuggerSource.SetHighlighting(g_mTextEditorSyntaxItems(i).sName)
                                    End If
                                End If

                        End Select
                    Next
                End SyncLock
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        Class ClassBinarySyntaxModeFileProvider
            Implements ISyntaxModeFileProvider

            Public ReadOnly Property SyntaxModes As ICollection(Of SyntaxMode) Implements ISyntaxModeFileProvider.SyntaxModes
                Get
                    Dim lSyntaxModes As New List(Of SyntaxMode)

                    For i = 0 To g_mTextEditorSyntaxItems.Length - 1
                        lSyntaxModes.Add(g_mTextEditorSyntaxItems(i).mSyntaxMode)
                    Next

                    Return lSyntaxModes.ToArray
                End Get
            End Property

            Public Sub UpdateSyntaxModeList() Implements ISyntaxModeFileProvider.UpdateSyntaxModeList
            End Sub

            Public Function GetSyntaxModeFile(mSyntaxMode As SyntaxMode) As XmlTextReader Implements ISyntaxModeFileProvider.GetSyntaxModeFile
                For i = 0 To g_mTextEditorSyntaxItems.Length - 1
                    If (mSyntaxMode.Name = g_mTextEditorSyntaxItems(i).sName) Then
                        Return g_mTextEditorSyntaxItems(i).ToXmlReader
                    End If
                Next

                Throw New ArgumentException("Syntax mode does not exist. Expected: " & mSyntaxMode.Name)
            End Function
        End Class
    End Class
End Class
