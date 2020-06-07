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


Imports System.Text.RegularExpressions
Imports BasicPawn
Imports ICSharpCode.TextEditor
Imports ICSharpCode.TextEditor.Document

''' <summary>
''' Usefull tools for the TextEditor
''' </summary>
Public Class ClassTextEditorTools
    Private g_mFormMain As FormMain

    Class STRUC_REFERENCE_ITEM
        Public sFile As String
        Public iLine As Integer
        Public sLine As String

        Sub New(_File As String, _Line As Integer, _strLine As String)
            sFile = _File
            iLine = _Line
            sLine = _strLine
        End Sub
    End Class

    Class STRUC_DEFINITION_ITEM
        Public sFile As String
        Public iLine As Integer
        Public mAutocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE

        Sub New(_File As String, _Line As Integer, _Autocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            sFile = _File
            iLine = _Line
            mAutocomplete = _Autocomplete
        End Sub
    End Class

    Enum ENUM_REFERENCE_ERROR_CODE
        INVALID_FILE = -2
        INVALID_INPUT = -1
        NO_RESULT = 0
        NO_ERROR = 1
    End Enum

    Enum ENUM_DEFINITION_ERROR_CODE
        INVALID_FILE = -2
        INVALID_INPUT = -1
        NO_RESULT = 0
        NO_ERROR = 1
    End Enum

    Public Sub New(f As FormMain)
        g_mFormMain = f
    End Sub

    ''' <summary>
    ''' Finds all references of a word from current opened source and all include files.
    ''' </summary>
    ''' <param name="sText">Word to search, otherwise if empty or |Nothing| it will get the word under the caret.</param>
    ''' <param name="bIgnoreNonCode">If true, ignores all matches in comments and strings.</param>
    Public Function FindReferences(ByRef sText As String, bIgnoreNonCode As Boolean, ByRef mReferences As STRUC_REFERENCE_ITEM()) As ENUM_REFERENCE_ERROR_CODE
        Dim mTab = g_mFormMain.g_ClassTabControl.m_ActiveTab

        Return FindReferences(mTab, sText, bIgnoreNonCode, mReferences)
    End Function

    Public Function FindReferences(mTab As ClassTabControl.ClassTab, ByRef sText As String, bIgnoreNonCode As Boolean, ByRef mReferences As STRUC_REFERENCE_ITEM()) As ENUM_REFERENCE_ERROR_CODE
        If (String.IsNullOrEmpty(sText)) Then
            sText = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(mTab, False, False, False)
        End If

        If (String.IsNullOrEmpty(sText)) Then
            Return ENUM_REFERENCE_ERROR_CODE.INVALID_INPUT
        End If

        If (mTab.m_IsUnsaved OrElse mTab.m_InvalidFile) Then
            Return ENUM_REFERENCE_ERROR_CODE.INVALID_FILE
        End If

        Dim mIncludeFiles = mTab.m_IncludesGroup.m_IncludeFiles.ToArray

        Dim lRefList As New List(Of STRUC_REFERENCE_ITEM)

        For Each mInclude In mIncludeFiles
            If (Not IO.File.Exists(mInclude.Value)) Then
                Continue For
            End If

            Dim sSource As String

            If (mInclude.Value.ToLower = mTab.m_File.ToLower) Then
                sSource = mTab.m_TextEditor.Document.TextContent
            Else
                sSource = IO.File.ReadAllText(mInclude.Value)
            End If

            Dim mSourceAnalysis As ClassSyntaxTools.ClassSyntaxSourceAnalysis

            If (bIgnoreNonCode) Then
                mSourceAnalysis = New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, mTab.m_Language)
            Else
                mSourceAnalysis = Nothing
            End If

            Dim iLine As Integer = 0
            Using mSR As New IO.StringReader(sSource)
                While True
                    Dim sLine As String = mSR.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    iLine += 1

                    If (Not sLine.Contains(sText)) Then
                        Continue While
                    End If

                    For Each mMatch As Match In Regex.Matches(sLine, String.Format("\b{0}\b", Regex.Escape(sText)))
                        If (mSourceAnalysis IsNot Nothing) Then
                            'Get index from line and from match to check if its inside non-code area
                            Dim iIndex = mSourceAnalysis.GetIndexFromLine(iLine - 1)
                            If (iIndex < 0 OrElse Not mSourceAnalysis.m_InRange(iIndex + mMatch.Index) OrElse mSourceAnalysis.m_InNonCode(iIndex + mMatch.Index)) Then
                                Continue For
                            End If
                        End If

                        lRefList.Add(New STRUC_REFERENCE_ITEM(mInclude.Value, iLine, sLine.Trim))
                    Next
                End While
            End Using
        Next

        mReferences = lRefList.ToArray
        Return ENUM_REFERENCE_ERROR_CODE.NO_ERROR
    End Function

    Public Function FindDefinition(ByRef sText As String, ByRef mDefinitions As STRUC_DEFINITION_ITEM()) As ENUM_DEFINITION_ERROR_CODE
        Dim mTab = g_mFormMain.g_ClassTabControl.m_ActiveTab

        Return FindDefinition(mTab, sText, mDefinitions)
    End Function

    Public Function FindDefinition(mTab As ClassTabControl.ClassTab, ByRef sText As String, ByRef mDefinitions As STRUC_DEFINITION_ITEM()) As ENUM_DEFINITION_ERROR_CODE
        If (String.IsNullOrEmpty(sText)) Then
            sText = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(mTab, True, True, True)
        End If

        If (String.IsNullOrEmpty(sText)) Then
            Return ENUM_DEFINITION_ERROR_CODE.INVALID_INPUT
        End If

        If (mTab.m_IsUnsaved OrElse mTab.m_InvalidFile) Then
            Return ENUM_DEFINITION_ERROR_CODE.INVALID_FILE
        End If

        Dim sFunctionNames As String() = sText.Split("."c)
        Dim iCurrentScopeIndex As Integer = -1

        'If 'this' keyword find out the type 
        If (sFunctionNames.Length = 2 AndAlso sFunctionNames(0) = "this") Then
            Dim sTextContent As String = mTab.m_TextEditor.Document.TextContent
            Dim iCaretOffset As Integer = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset
            Dim iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = mTab.m_Language
            Dim iBraceList As Integer()() = ClassSyntaxTools.ClassSyntaxHelpers.GetExpressionBetweenCharacters(sTextContent, "{"c, "}"c, 1, iLanguage, True)

            'Get current scope index
            For i = 0 To iBraceList.Length - 1
                If (iCaretOffset < iBraceList(i)(0) OrElse iCaretOffset > iBraceList(i)(1)) Then
                    Continue For
                End If

                iCurrentScopeIndex = i
                Exit For
            Next
        End If

        Dim sFile As String = mTab.m_File
        Dim lFoundAutocomplete As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
        Dim mAutocompleteArray As ClassSyntaxTools.STRUC_AUTOCOMPLETE() = mTab.m_AutocompleteGroup.m_AutocompleteItems.ToArray

        If (lFoundAutocomplete.Count < 1) Then
            For i = 0 To mAutocompleteArray.Length - 1
                If (mAutocompleteArray(i).m_Data.ContainsKey("EnumHidden")) Then
                    Continue For
                End If

                'Ignore constructors just go straight to methodmap
                If ((mAutocompleteArray(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> 0) Then
                    If (CBool(mAutocompleteArray(i).m_Data("MethodmapMethodIsConstructor"))) Then
                        Continue For
                    End If
                End If

                If (mAutocompleteArray(i).m_Data.ContainsKey("IsThis")) Then
                    If (iCurrentScopeIndex < 0) Then
                        Continue For
                    End If

                    If ((mAutocompleteArray(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> 0) Then
                        Dim iScopeIndex As Integer = CInt(mAutocompleteArray(i).m_Data("@MethodmapScopeIndex"))
                        Dim sScopeFile As String = CStr(mAutocompleteArray(i).m_Data("@MethodmapScopeFile"))

                        If (iScopeIndex <> iCurrentScopeIndex) Then
                            Continue For
                        End If

                        If (sScopeFile.ToLower <> sFile.ToLower) Then
                            Continue For
                        End If
                    End If

                    If ((mAutocompleteArray(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT) <> 0) Then
                        Dim iScopeIndex As Integer = CInt(mAutocompleteArray(i).m_Data("@EnumStructScopeIndex"))
                        Dim sScopeFile As String = CStr(mAutocompleteArray(i).m_Data("@EnumStructScopeFile"))

                        If (iScopeIndex <> iCurrentScopeIndex) Then
                            Continue For
                        End If

                        If (sScopeFile.ToLower <> sFile.ToLower) Then
                            Continue For
                        End If
                    End If
                End If

                If (mAutocompleteArray(i).m_FunctionString.Equals(sText)) Then
                    lFoundAutocomplete.Add(mAutocompleteArray(i))
                End If
            Next
        End If

        'Find enums, methodmaps, struct enums etc.
        If (lFoundAutocomplete.Count < 1) Then
            For i = 0 To mAutocompleteArray.Length - 1
                If ((mAutocompleteArray(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) = 0) Then
                    Continue For
                End If

                If (mAutocompleteArray(i).m_FunctionName.Equals(sText)) Then
                    lFoundAutocomplete.Add(mAutocompleteArray(i))
                End If
            Next
        End If

        Dim lDefinitions As New List(Of STRUC_DEFINITION_ITEM)
        For Each mAutocomplete In lFoundAutocomplete
            Dim mDefinition As STRUC_DEFINITION_ITEM = Nothing

            If (FindDefinition(mTab, mAutocomplete, mDefinition) = ENUM_DEFINITION_ERROR_CODE.NO_ERROR) Then
                lDefinitions.Add(mDefinition)
            End If
        Next

        mDefinitions = lDefinitions.ToArray
        Return ENUM_DEFINITION_ERROR_CODE.NO_ERROR
    End Function

    Public Function FindDefinition(mAutocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE, ByRef mDefinition As STRUC_DEFINITION_ITEM) As ENUM_DEFINITION_ERROR_CODE
        Dim mTab = g_mFormMain.g_ClassTabControl.m_ActiveTab

        Return FindDefinition(mTab, mAutocomplete, mDefinition)
    End Function

    Public Function FindDefinition(mTab As ClassTabControl.ClassTab, mAutocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE, ByRef mDefinition As STRUC_DEFINITION_ITEM) As ENUM_DEFINITION_ERROR_CODE
        If (mAutocomplete Is Nothing) Then
            Return ENUM_DEFINITION_ERROR_CODE.INVALID_INPUT
        End If

        Dim sAnchorName As String
        Dim iAnchorIndex As Integer
        Dim sAnchorFile As String

        Select Case (True)
            Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE) <> 0
                If (Not mAutocomplete.m_Data.ContainsKey("DefineAnchorName")) Then
                    Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                End If

                sAnchorName = CStr(mAutocomplete.m_Data("DefineAnchorName"))
                iAnchorIndex = CInt(mAutocomplete.m_Data("DefineAnchorIndex"))
                sAnchorFile = CStr(mAutocomplete.m_Data("DefineAnchorFile"))

            Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) <> 0
                If (Not mAutocomplete.m_Data.ContainsKey("EnumAnchorName")) Then
                    Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                End If

                sAnchorName = CStr(mAutocomplete.m_Data("EnumAnchorName"))
                iAnchorIndex = CInt(mAutocomplete.m_Data("EnumAnchorIndex"))
                sAnchorFile = CStr(mAutocomplete.m_Data("EnumAnchorFile"))

            Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STRUCT) <> 0
                If (Not mAutocomplete.m_Data.ContainsKey("StructAnchorName")) Then
                    Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                End If

                sAnchorName = CStr(mAutocomplete.m_Data("StructAnchorName"))
                iAnchorIndex = CInt(mAutocomplete.m_Data("StructAnchorIndex"))
                sAnchorFile = CStr(mAutocomplete.m_Data("StructAnchorFile"))

            Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR) <> 0
                If (Not mAutocomplete.m_Data.ContainsKey("PublicAnchorName")) Then
                    Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                End If

                sAnchorName = CStr(mAutocomplete.m_Data("PublicAnchorName"))
                iAnchorIndex = CInt(mAutocomplete.m_Data("PublicAnchorIndex"))
                sAnchorFile = CStr(mAutocomplete.m_Data("PublicAnchorFile"))

            Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) <> 0
                If (Not mAutocomplete.m_Data.ContainsKey("FuncenumAnchorName")) Then
                    Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                End If

                sAnchorName = CStr(mAutocomplete.m_Data("FuncenumAnchorName"))
                iAnchorIndex = CInt(mAutocomplete.m_Data("FuncenumAnchorIndex"))
                sAnchorFile = CStr(mAutocomplete.m_Data("FuncenumAnchorFile"))

            Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) <> 0
                If (Not mAutocomplete.m_Data.ContainsKey("TypesetAnchorName")) Then
                    Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                End If

                sAnchorName = CStr(mAutocomplete.m_Data("TypesetAnchorName"))
                iAnchorIndex = CInt(mAutocomplete.m_Data("TypesetAnchorIndex"))
                sAnchorFile = CStr(mAutocomplete.m_Data("TypesetAnchorFile"))

            Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) <> 0
                If (Not mAutocomplete.m_Data.ContainsKey("TypedefAnchorName")) Then
                    Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                End If

                sAnchorName = CStr(mAutocomplete.m_Data("TypedefAnchorName"))
                iAnchorIndex = CInt(mAutocomplete.m_Data("TypedefAnchorIndex"))
                sAnchorFile = CStr(mAutocomplete.m_Data("TypedefAnchorFile"))

            Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) <> 0
                If (Not mAutocomplete.m_Data.ContainsKey("FunctagAnchorName")) Then
                    Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                End If

                sAnchorName = CStr(mAutocomplete.m_Data("FunctagAnchorName"))
                iAnchorIndex = CInt(mAutocomplete.m_Data("FunctagAnchorIndex"))
                sAnchorFile = CStr(mAutocomplete.m_Data("FunctagAnchorFile"))

            Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHOD) <> 0
                If (Not mAutocomplete.m_Data.ContainsKey("MethodAnchorName")) Then
                    Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                End If

                sAnchorName = CStr(mAutocomplete.m_Data("MethodAnchorName"))
                iAnchorIndex = CInt(mAutocomplete.m_Data("MethodAnchorIndex"))
                sAnchorFile = CStr(mAutocomplete.m_Data("MethodAnchorFile"))

            Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> 0,
                            (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0 AndAlso mAutocomplete.m_Data.ContainsKey("VariableMethodmapName")
                If (Not mAutocomplete.m_Data.ContainsKey("MethodmapAnchorName")) Then
                    Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                End If

                sAnchorName = CStr(mAutocomplete.m_Data("MethodmapAnchorName"))
                iAnchorIndex = CInt(mAutocomplete.m_Data("MethodmapAnchorIndex"))
                sAnchorFile = CStr(mAutocomplete.m_Data("MethodmapAnchorFile"))

            Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT) <> 0,
                            (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0 AndAlso mAutocomplete.m_Data.ContainsKey("VariableEnumStructName")
                If (Not mAutocomplete.m_Data.ContainsKey("EnumStructAnchorName")) Then
                    Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                End If

                sAnchorName = CStr(mAutocomplete.m_Data("EnumStructAnchorName"))
                iAnchorIndex = CInt(mAutocomplete.m_Data("EnumStructAnchorIndex"))
                sAnchorFile = CStr(mAutocomplete.m_Data("EnumStructAnchorFile"))

            Case Else
                Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
        End Select

        If (Not IO.File.Exists(sAnchorFile)) Then
            Return ENUM_DEFINITION_ERROR_CODE.INVALID_FILE
        End If

        Dim sSource As String

        If (sAnchorFile.ToLower = mTab.m_File.ToLower) Then
            sSource = mTab.m_TextEditor.Document.TextContent
        Else
            sSource = IO.File.ReadAllText(sAnchorFile)
        End If

        If (iAnchorIndex < 0) Then
            Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
        End If

        Dim mAnchorMatches = Regex.Matches(sSource, String.Format("\b({0})\b", Regex.Escape(sAnchorName)))
        If (mAnchorMatches.Count < 0 OrElse iAnchorIndex > mAnchorMatches.Count - 1) Then
            Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
        End If

        Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, mTab.m_Language)

        Dim iOffset As Integer = 0
        Dim iLine As Integer = 1
        Dim iSearchAnchorIndex As Integer = 0
        Dim bSuccess As Boolean = False

        For i = 0 To mAnchorMatches.Count - 1
            'We should ignore anchors in non-code parts.
            If (mSourceAnalysis.m_InNonCode(mAnchorMatches(i).Index)) Then
                Continue For
            End If

            If (iSearchAnchorIndex > iAnchorIndex) Then
                Exit For
            End If

            iSearchAnchorIndex += 1
            iOffset = mAnchorMatches(i).Index
            bSuccess = True
        Next

        'If we didnt find anything, abort.
        If (Not bSuccess) Then
            Return ENUM_DEFINITION_ERROR_CODE.NO_RESULT
        End If

        For i = 0 To iOffset
            If (sSource(i) = vbLf(0)) Then
                iLine += 1
            End If
        Next

        mDefinition = New STRUC_DEFINITION_ITEM(sAnchorFile, iLine, mAutocomplete)
        Return ENUM_DEFINITION_ERROR_CODE.NO_ERROR
    End Function

    Public Sub FormatCode(mTab As ClassTabControl.ClassTab)
        Dim iChangedLines As Integer = 0

        Dim lRealSourceLines As New List(Of String)
        Using mSR As New IO.StringReader(mTab.m_TextEditor.Document.TextContent)
            Dim sLine As String
            While True
                sLine = mSR.ReadLine
                If (sLine Is Nothing) Then
                    Exit While
                End If

                lRealSourceLines.Add(sLine)
            End While
        End Using

        Dim sFormatedSource As String = ClassSyntaxTools.ClassSyntaxHelpers.FormatCodeIndentation(mTab.m_TextEditor.Document.TextContent, ClassSyntaxTools.ClassSyntaxHelpers.ENUM_INDENTATION_TYPES.USE_SETTINGS, mTab.m_Language)
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
            mTab.m_TextEditor.Document.UndoStack.StartUndoGroup()

            For i = lFormatedSourceLines.Count - 1 To 0 Step -1
                Dim mLineSeg = mTab.m_TextEditor.Document.GetLineSegment(i)
                Dim sLine As String = mTab.m_TextEditor.Document.GetText(mLineSeg.Offset, mLineSeg.Length)

                If (mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                    If (Not mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset) AndAlso
                            Not mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset + mLineSeg.Length)) Then
                        Continue For
                    End If
                End If

                If (sLine = lFormatedSourceLines(i)) Then
                    Continue For
                End If

                mTab.m_TextEditor.Document.Remove(mLineSeg.Offset, mLineSeg.Length)
                mTab.m_TextEditor.Document.Insert(mLineSeg.Offset, lFormatedSourceLines(i))

                iChangedLines += 1
            Next
        Finally
            mTab.m_TextEditor.Document.UndoStack.EndUndoGroup()

            mTab.m_TextEditor.InvalidateTextArea()
        End Try

        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("{0} lines of code reindented!", iChangedLines), False, True, True)
    End Sub

    Public Sub FormatCodeTrim(mTab As ClassTabControl.ClassTab)
        Dim iChangedLines As Integer = 0

        Dim lRealSourceLines As New List(Of String)
        Using mSR As New IO.StringReader(mTab.m_TextEditor.Document.TextContent)
            Dim sLine As String
            While True
                sLine = mSR.ReadLine
                If (sLine Is Nothing) Then
                    Exit While
                End If

                lRealSourceLines.Add(sLine)
            End While
        End Using

        Dim sFormatedSource As String = ClassSyntaxTools.ClassSyntaxHelpers.FormatCodeTrimEnd(mTab.m_TextEditor.Document.TextContent)
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
            mTab.m_TextEditor.Document.UndoStack.StartUndoGroup()

            For i = lFormatedSourceLines.Count - 1 To 0 Step -1
                Dim mLineSeg = mTab.m_TextEditor.Document.GetLineSegment(i)
                Dim sLine As String = mTab.m_TextEditor.Document.GetText(mLineSeg.Offset, mLineSeg.Length)

                If (mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                    If (Not mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset) AndAlso
                            Not mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset + mLineSeg.Length)) Then
                        Continue For
                    End If
                End If

                If (sLine = lFormatedSourceLines(i)) Then
                    Continue For
                End If

                mTab.m_TextEditor.Document.Remove(mLineSeg.Offset, mLineSeg.Length)
                mTab.m_TextEditor.Document.Insert(mLineSeg.Offset, lFormatedSourceLines(i))

                iChangedLines += 1
            Next
        Finally
            mTab.m_TextEditor.Document.UndoStack.EndUndoGroup()

            mTab.m_TextEditor.InvalidateTextArea()
        End Try

        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("{0} lines with ending whitespace trimmed!", iChangedLines), False, True, True)
    End Sub

    ''' <summary>
    ''' Opens a 'Search and Replace' form
    ''' </summary>
    ''' <param name="sSearchText"></param>
    Public Sub ShowSearchAndReplace(sSearchText As String)
        'Check single instance
        For Each c As Form In Application.OpenForms
            Dim f = TryCast(c, FormSearch)
            If (f Is Nothing) Then
                Continue For
            End If

            If (Not f.m_SingleInstance) Then
                Continue For
            End If

            f.m_SearchText = sSearchText
            f.Activate()
            Return
        Next

        'Count forms
        Dim iFormCount As Integer = 0
        For Each f As Form In Application.OpenForms
            If (TypeOf f Is FormSearch) Then
                iFormCount += 1
            End If
        Next

        If (iFormCount > 100) Then
            MessageBox.Show("Too many 'Search & Replace' windows open!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim i As New FormSearch(g_mFormMain, sSearchText)
            i.Show(g_mFormMain)
        End If
    End Sub

    ''' <summary>
    ''' Gets the caret word in the text editor
    ''' </summary>
    ''' <param name="bIncludeMethodmaps">If true, includes methodmaps (e.g Methodmap.Name)</param>
    ''' <param name="bIncludeMethodNames">If true, includes method names. Skips arguments. (e.g method().Name -> Method.Name)</param>
    ''' <returns></returns>
    Public Function GetCaretWord(mTab As ClassTabControl.ClassTab, bIncludeMethodmaps As Boolean, bIncludeMethodNames As Boolean, bIncludeArrayNames As Boolean) As String
        Dim iOffset As Integer = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset
        Dim iPosition As Integer = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
        Dim iLineOffset As Integer = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Offset
        Dim iLineLen As Integer = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Length

        Dim sWordLeft As String = ""
        Dim sWordRight As String = ""

        If (bIncludeMethodmaps OrElse bIncludeMethodNames OrElse bIncludeArrayNames) Then
            Dim bIsMethod As Boolean = Regex.Match(mTab.m_TextEditor.Document.GetText(iLineOffset, iPosition), "(\))(\.)(\b[a-zA-Z0-9_]+\b){0,1}$").Success
            Dim bIsArray As Boolean = Regex.Match(mTab.m_TextEditor.Document.GetText(iLineOffset, iPosition), "(\])(\.)(\b[a-zA-Z0-9_]+\b){0,1}$").Success

            If ((bIncludeMethodNames AndAlso bIsMethod) OrElse (bIncludeArrayNames AndAlso bIsArray)) Then
                Dim sSource As String = mTab.m_TextEditor.Document.GetText(0, iOffset)
                If (sSource.Length > 0) Then
                    Dim mSourceBuilder As New Text.StringBuilder(sSource.Length)
                    Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, mTab.m_Language)
                    Dim iLastParenthesisLevel As Integer = mSourceAnalysis.GetParenthesisLevel(sSource.Length - 1, Nothing)

                    For i = sSource.Length - 1 To 0 Step -1
                        If (mSourceAnalysis.GetBraceLevel(i, Nothing) < 1) Then
                            Exit For
                        End If

                        If (mSourceAnalysis.m_InNonCode(i)) Then
                            Continue For
                        End If

                        If (bIncludeMethodNames AndAlso bIsMethod) Then
                            If (mSourceAnalysis.GetParenthesisLevel(i, Nothing) <> iLastParenthesisLevel) Then
                                Continue For
                            End If

                        ElseIf (bIncludeArrayNames AndAlso bIsArray) Then
                            If (mSourceAnalysis.GetBracketLevel(i, Nothing) > 0) Then
                                Continue For
                            End If
                        Else
                            Throw New ArgumentException("Unknown action")
                        End If


                        mSourceBuilder.Append(sSource(i))
                    Next

                    Dim sFilteredSource As String = StrReverse(mSourceBuilder.ToString)

                    sWordLeft = Regex.Match(sFilteredSource, "((\b[a-zA-Z0-9_]+\b)(\.){0,1}(\b[a-zA-Z0-9_]+\b){0,1})$").Value
                    If (sWordLeft.Contains("."c)) Then
                        'If the left contains already 2 parts of a methodmap, then only get the name on the right side.
                        sWordRight = Regex.Match(mTab.m_TextEditor.Document.GetText(iLineOffset + iPosition, iLineLen - iPosition), "^(\b[a-zA-Z0-9_]+\b)").Value
                    Else
                        sWordRight = Regex.Match(mTab.m_TextEditor.Document.GetText(iLineOffset + iPosition, iLineLen - iPosition), "^((\b[a-zA-Z0-9_]+\b){0,1}(\.){0,1}(\b[a-zA-Z0-9_]+\b))").Value
                    End If
                End If

            Else
                sWordLeft = Regex.Match(mTab.m_TextEditor.Document.GetText(iLineOffset, iPosition), "((\b[a-zA-Z0-9_]+\b)(\.){0,1}(\b[a-zA-Z0-9_]+\b){0,1})$").Value
                If (sWordLeft.Contains("."c)) Then
                    'If the left contains already 2 parts of a methodmap, then only get the name on the right side.
                    sWordRight = Regex.Match(mTab.m_TextEditor.Document.GetText(iLineOffset + iPosition, iLineLen - iPosition), "^(\b[a-zA-Z0-9_]+\b)").Value
                Else
                    sWordRight = Regex.Match(mTab.m_TextEditor.Document.GetText(iLineOffset + iPosition, iLineLen - iPosition), "^((\b[a-zA-Z0-9_]+\b){0,1}(\.){0,1}(\b[a-zA-Z0-9_]+\b))").Value
                End If
            End If
        Else
            sWordLeft = Regex.Match(mTab.m_TextEditor.Document.GetText(iLineOffset, iPosition), "(\b[a-zA-Z0-9_]+\b)$").Value
            sWordRight = Regex.Match(mTab.m_TextEditor.Document.GetText(iLineOffset + iPosition, iLineLen - iPosition), "^(\b[a-zA-Z0-9_]+\b)").Value
        End If

        Return (sWordLeft & sWordRight)
    End Function

    Enum ENUM_COMPILER_TYPE
        UNKNOWN
        SOURCEPAWN
        AMXX
        AMX
    End Enum

    ''' <summary>
    ''' Compiles the source.
    ''' </summary>
    ''' <param name="bTesting">Just creates a temporary file and removes it after compile.</param>
    ''' <param name="sSource">The source to compile. Use |Nothing| to get the source from the active tab.</param>
    ''' <param name="sOutputFile">The output file. This may change the extenstion if you using different compilers (e.g *.smx, *.amx, *.amxx). And extension is still required! Use |Nothing| to get the output by active tab.</param>
    ''' <param name="sWorkingDirectory">Sets the compiler working directory. Use |Nothing| to use the working directioy from the active tab.</param>
    ''' <param name="sCompilerPath">The compiler path. Use |Nothing| to get the source from the active tab.</param>
    ''' <param name="sIncludePaths">The include paths seperated by ';'. Use |Nothing| to get the source from the active tab.</param>
    ''' <param name="sEmulateSourceFile">Replaces the printed temporary path.</param>
    ''' <param name="bUseCustomCompilerOptions">If true it will use the configs custom compiler options, false otherwise.</param>
    ''' <param name="sCompilerOutput">The compiler output.</param>
    ''' <param name="iCompilerType"></param>
    ''' <returns>True on success, false otherwise.</returns>
    Public Function CompileSource(mTab As ClassTabControl.ClassTab,
                                  sFile As String,
                                  sSource As String,
                                  bTesting As Boolean,
                                  bBuildEvents As Boolean,
                                  ByRef sOutputFile As String,
                                  Optional mConfig As ClassConfigs.STRUC_CONFIG_ITEM = Nothing,
                                  Optional sWorkingDirectory As String = Nothing,
                                  Optional sCompilerPath As String = Nothing,
                                  Optional sCompilerSearchPath As String = Nothing,
                                  Optional sIncludePaths As String = Nothing,
                                  Optional sIncludeSearchPath As String = Nothing,
                                  Optional sEmulateSourceFile As String = Nothing,
                                  Optional bUseCustomCompilerOptions As Boolean = True,
                                  ByRef Optional sCompilerOutput As String = Nothing,
                                  ByRef Optional iCompilerType As ENUM_COMPILER_TYPE = ENUM_COMPILER_TYPE.UNKNOWN) As Boolean
        Try
            If (mTab Is Nothing) Then
                mTab = g_mFormMain.g_ClassTabControl.m_ActiveTab
            End If

            If (sSource Is Nothing) Then
                sSource = mTab.m_TextEditor.Document.TextContent
            End If

            If (mConfig Is Nothing) Then
                mConfig = mTab.m_ActiveConfig
            End If

            If (Not String.IsNullOrEmpty(sEmulateSourceFile)) Then
                With New Text.StringBuilder
                    .AppendFormat("#file ""{0}""", sEmulateSourceFile).AppendLine()
                    .AppendLine("#line 0")
                    .AppendLine(sSource)

                    sSource = .ToString
                End With
            End If

            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Compiling source started!", False, True, True)

            Dim iExitCode As Integer = 0
            Dim sOutput As String = ""
            Dim sLines As String()

            'Check compiler
            If (sCompilerPath Is Nothing) Then
                If (mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    Dim sFilePath As String = sFile

                    If (String.IsNullOrEmpty(sFilePath)) Then
                        If (mTab.m_Index < 0) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Could not get current source file!", False, False, True)
                            Return False
                        End If

                        If (mTab.m_IsUnsaved AndAlso
                                    Not g_mFormMain.g_ClassTabControl.PromptSaveTab(mTab.m_Index, False, True, True)) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Could not get current source file!", False, False, True)
                            Return False
                        End If

                        If (mTab.m_IsUnsaved) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Could not get current source file!", False, False, True)
                            Return False
                        End If

                        sFilePath = mTab.m_File
                    End If

                    Dim sFileDirectory As String = IO.Path.GetDirectoryName(sFilePath)
                    If (Not String.IsNullOrEmpty(sCompilerSearchPath)) Then
                        sFileDirectory = sCompilerSearchPath
                    End If

                    While True
                        'SourcePawn
                        sCompilerPath = IO.Path.Combine(sFileDirectory, "spcomp.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'AMX Mod X
                        sCompilerPath = IO.Path.Combine(sFileDirectory, "amxxpc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'Small
                        sCompilerPath = IO.Path.Combine(sFileDirectory, "sc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'Pawn
                        sCompilerPath = IO.Path.Combine(sFileDirectory, "pawncc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Compiler can not be found!", False, False, True)
                        Return False
                    End While
                Else
                    sCompilerPath = mConfig.g_sCompilerPath
                    If (Not IO.File.Exists(sCompilerPath)) Then
                        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Compiler can not be found!", False, False, True)
                        Return False
                    End If
                End If
            Else
                If (Not IO.File.Exists(sCompilerPath)) Then
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Compiler can not be found!", False, False, True)
                    Return False
                End If
            End If

            'Check include path
            If (sIncludePaths Is Nothing) Then
                If (mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    Dim sFilePath As String = sFile

                    If (String.IsNullOrEmpty(sFilePath)) Then
                        If (mTab.m_Index < 0) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Could not get current source file!", False, False, True)
                            Return False
                        End If

                        If (mTab.m_IsUnsaved AndAlso
                                        Not g_mFormMain.g_ClassTabControl.PromptSaveTab(mTab.m_Index, False, True, True)) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Could not get current source file!", False, False, True)
                            Return False
                        End If

                        If (mTab.m_IsUnsaved) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Could not get current source file!", False, False, True)
                            Return False
                        End If

                        sFilePath = mTab.m_File
                    End If

                    Dim sFileDirectory As String = IO.Path.GetDirectoryName(sFilePath)
                    If (Not String.IsNullOrEmpty(sIncludeSearchPath)) Then
                        sFileDirectory = sIncludeSearchPath
                    End If

                    sIncludePaths = IO.Path.Combine(sFileDirectory, "include")
                    If (Not IO.Directory.Exists(sIncludePaths)) Then
                        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Include path can not be found!", False, False, True)
                        Return False
                    End If
                Else
                    sIncludePaths = mConfig.g_sIncludeFolders
                    For Each sInclude As String In sIncludePaths.Split(";"c)
                        If (Not IO.Directory.Exists(sInclude)) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Include path can not be found!", False, False, True)
                            Return False
                        End If
                    Next
                End If
            Else
                For Each sInclude As String In sIncludePaths.Split(";"c)
                    If (Not IO.Directory.Exists(sInclude)) Then
                        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Include path can not be found!", False, False, True)
                        Return False
                    End If
                Next
            End If

            'Set output path
            If (Not bTesting) Then
                If (String.IsNullOrEmpty(sOutputFile)) Then
                    Dim sFilePath As String = sFile

                    If (String.IsNullOrEmpty(sFilePath)) Then
                        If (mTab.m_Index < 0) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Could not get current source file!", False, False, True)
                            Return False
                        End If

                        If (mTab.m_IsUnsaved AndAlso
                                    Not g_mFormMain.g_ClassTabControl.PromptSaveTab(mTab.m_Index, False, True, True)) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Could not get current source file!", False, False, True)
                            Return False
                        End If

                        If (mTab.m_IsUnsaved) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Could not get current source file!", False, False, True)
                            Return False
                        End If

                        sFilePath = mTab.m_File
                    End If

                    If (mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                        Dim sOutputDir As String = IO.Path.Combine(IO.Path.GetDirectoryName(sFilePath), "compiled")

                        If (Not IO.Directory.Exists(sOutputDir)) Then
                            IO.Directory.CreateDirectory(sOutputDir)
                        End If

                        sOutputFile = IO.Path.Combine(sOutputDir, String.Format("{0}.unk", IO.Path.GetFileNameWithoutExtension(sFilePath)))

                    Else
                        If (Not IO.Directory.Exists(mConfig.g_sOutputFolder)) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Invalid output directory!", False, False, True)
                            Return False
                        End If
                        sOutputFile = IO.Path.Combine(mConfig.g_sOutputFolder, String.Format("{0}.unk", IO.Path.GetFileNameWithoutExtension(sFilePath)))
                    End If
                End If
            End If

            Dim sTmpOutputFile As String = IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString)

            Dim TmpSourceFile As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
            IO.File.WriteAllText(TmpSourceFile, sSource)

            Dim lIncludeList As New List(Of String)
            For Each sInclude As String In sIncludePaths.Split(";"c)
                lIncludeList.Add("-i""" & sInclude & """")
            Next

            'Get compiler type first.
            'Normaly every compiler should print help without any process arguments, but AMX Mod X compiler wants input...
            'Use an unfinished argument to force help.
            'TODO: Should detect compiler type using file info instead? This is a bad hack.
            ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, "-", iExitCode, sOutput)

            iCompilerType = ENUM_COMPILER_TYPE.UNKNOWN

            sLines = sOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
            If (sLines.Length > 0) Then
                Select Case (True)
                    Case Regex.IsMatch(sLines(0), "\b(SourcePawn)\b \b(Compiler)\b", RegexOptions.IgnoreCase)
                        iCompilerType = ENUM_COMPILER_TYPE.SOURCEPAWN

                    Case Regex.IsMatch(sLines(0), "\b(AMX)\b \b(Mod)\b \b(X)\b", RegexOptions.IgnoreCase)
                        iCompilerType = ENUM_COMPILER_TYPE.AMXX

                    'Old AMX Mod still uses Small compiler
                    Case Regex.IsMatch(sLines(0), "\b(Pawn)\b \b(compiler)\b", RegexOptions.IgnoreCase), Regex.IsMatch(sLines(0), "\b(Small)\b \b(compiler)\b", RegexOptions.IgnoreCase)
                        iCompilerType = ENUM_COMPILER_TYPE.AMX

                End Select
            End If

            'Build arguments 
            Dim lArguments As New List(Of String) From {
               String.Format("""{0}"" {1} -o""{2}""", TmpSourceFile, String.Join(" ", lIncludeList.ToArray), sTmpOutputFile)
            }

            If (bUseCustomCompilerOptions) Then
                Select Case (iCompilerType)
                    Case ENUM_COMPILER_TYPE.SOURCEPAWN
                        lArguments.Add(mConfig.g_mCompilerOptionsSP.BuildCommandline)

                    Case ENUM_COMPILER_TYPE.AMXX
                        lArguments.Add(mConfig.g_mCompilerOptionsAMXX.BuildCommandline)

                End Select
            End If

            Dim mBuildEvents As ClassTextEditorTools.ClassCompilerBuildEvents = Nothing

            If (bBuildEvents) Then
                mBuildEvents = New ClassTextEditorTools.ClassCompilerBuildEvents(mConfig, mTab.m_File, mConfig.g_sIncludeFolders, mConfig.g_sCompilerPath, mConfig.g_sOutputFolder, bTesting, sWorkingDirectory)
            End If

            'Execute pre-build events
            If (mBuildEvents IsNot Nothing AndAlso Not String.IsNullOrEmpty(mBuildEvents.m_PreBuildCmd)) Then
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Executing pre-build event...", False, False, True)

                Dim iBuildEventExitCode As Integer = -1
                Dim sBuildEventOutput As String = ""

                mBuildEvents.ExecPre(iBuildEventExitCode, sBuildEventOutput)

                Dim sOutputLines = sBuildEventOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
                For i = 0 To sOutputLines.Length - 1
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & sOutputLines(i), False, False, True)
                Next

                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Completed pre-build event with exit code: " & iBuildEventExitCode, False, False, True)
            End If

            'Compile 
            If (String.IsNullOrEmpty(sWorkingDirectory) OrElse Not IO.Directory.Exists(sWorkingDirectory)) Then
                ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, String.Join(" "c, lArguments.ToArray), iExitCode, sOutput)
            Else
                ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, String.Join(" "c, lArguments.ToArray), sWorkingDirectory, iExitCode, sOutput)
            End If

            sCompilerOutput = sOutput

            IO.File.Delete(TmpSourceFile)

            sLines = sOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
            For i = 0 To sLines.Length - 1
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & sLines(i), g_mFormMain.g_mUCInformationList.ParseFromCompilerOutput(mTab.m_File, sLines(i)))
            Next

            sCompilerOutput = String.Join(Environment.NewLine, sLines)

            'The AMX Mod X compiler seem to overwrite the *.unk extension, just find them using the unique GUID filename.
            Dim sOutputMatches As String() = IO.Directory.GetFiles(IO.Path.GetDirectoryName(sTmpOutputFile), String.Format("{0}.*", IO.Path.GetFileNameWithoutExtension(sTmpOutputFile)), IO.SearchOption.TopDirectoryOnly)
            If (sOutputMatches.Length = 1) Then
                sTmpOutputFile = sOutputMatches(0)
            Else
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Compiling failed! Compiled output file can not be found!", False, False, True)
                Return False
            End If

            If (bTesting) Then
                IO.File.Delete(sTmpOutputFile)
            Else
                Select Case (iCompilerType)
                    Case ENUM_COMPILER_TYPE.SOURCEPAWN
                        Dim sNewOutputFile As String = IO.Path.ChangeExtension(sOutputFile, ".smx")
                        IO.File.Delete(sNewOutputFile)
                        IO.File.Move(sTmpOutputFile, sNewOutputFile)
                        sOutputFile = sNewOutputFile

                    Case ENUM_COMPILER_TYPE.AMXX
                        Dim sNewOutputFile As String = IO.Path.ChangeExtension(sOutputFile, ".amxx")
                        IO.File.Delete(sNewOutputFile)
                        IO.File.Move(sTmpOutputFile, sNewOutputFile)
                        sOutputFile = sNewOutputFile

                    Case ENUM_COMPILER_TYPE.AMX
                        Dim sNewOutputFile As String = IO.Path.ChangeExtension(sOutputFile, ".amx")
                        IO.File.Delete(sNewOutputFile)
                        IO.File.Move(sTmpOutputFile, sNewOutputFile)
                        sOutputFile = sNewOutputFile

                    Case Else
                        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_WARNING, vbTab & "Unsupported compiler!")

                        Dim sNewOutputFile As String = IO.Path.ChangeExtension(sOutputFile, ".bin")
                        IO.File.Delete(sNewOutputFile)
                        IO.File.Move(sTmpOutputFile, sNewOutputFile)
                        sOutputFile = sNewOutputFile

                End Select

                If (Not bTesting) Then
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("Saved compiled source: {0}", sOutputFile), New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(sOutputFile))
                End If
            End If

            'Execute post-build events
            If (mBuildEvents IsNot Nothing AndAlso Not String.IsNullOrEmpty(mBuildEvents.m_PostBuildCmd)) Then
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Executing post-build event...", False, False, True)

                Dim iBuildEventExitCode As Integer = -1
                Dim sBuildEventOutput As String = ""

                mBuildEvents.ExecPost(iBuildEventExitCode, sBuildEventOutput)

                Dim sOutputLines = sBuildEventOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
                For i = 0 To sOutputLines.Length - 1
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & sOutputLines(i), False, False, True)
                Next

                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Completed post-build event with exit code: " & iBuildEventExitCode, False, False, True)
            End If

            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Compiling source finished!", False, False, True)
            Return True
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try

        Return False
    End Function

    ''' <summary>
    ''' Generates pre-process source. Cleaned up, defines resolved etc.
    ''' </summary>
    ''' <param name="sSource">The source to compile. Use |Nothing| to get the source from the active tab.</param>
    ''' <param name="bCleanUpSourcemodDuplicate">Removes duplicated sourcemod includes.</param>
    ''' <param name="bCleanupForCompile">Removes pre-processor entries which hinders compiling.</param>
    ''' <param name="sTempOutputFile">The last used temporary file. Note: This is only the path, the file will be removed!</param>
    ''' <param name="sWorkingDirectory">Sets the compiler working directory. Use |Nothing| to use the working directioy from the active tab.</param>
    ''' <param name="sCompilerPath">The compiler path. Use |Nothing| to get the source from the active tab.</param>
    ''' <param name="sIncludePaths">The include paths seperated by ';'. Use |Nothing| to get the source from the active tab.</param>
    ''' <param name="sEmulateSourceFile">Replaces the printed temporary path.</param>
    ''' <param name="bUseCustomCompilerOptions">If true it will use the configs custom compiler options, false otherwise.</param>
    ''' <param name="sCompilerOutput">The compiler output.</param>
    ''' <param name="iCompilerType"></param>
    ''' <returns>New source on success, |Nothing| in otherwise.</returns>
    Public Function GetCompilerPreProcessCode(mTab As ClassTabControl.ClassTab,
                                              sFile As String,
                                              sSource As String,
                                              bCleanUpSourcemodDuplicate As Boolean,
                                              bCleanupForCompile As Boolean,
                                              ByRef sTempOutputFile As String,
                                              Optional mConfig As ClassConfigs.STRUC_CONFIG_ITEM = Nothing,
                                              Optional sWorkingDirectory As String = Nothing,
                                              Optional sCompilerPath As String = Nothing,
                                              Optional sCompilerSearchPath As String = Nothing,
                                              Optional sIncludePaths As String = Nothing,
                                              Optional sIncludeSearchPath As String = Nothing,
                                              Optional sEmulateSourceFile As String = Nothing,
                                              Optional bUseCustomCompilerOptions As Boolean = True,
                                              ByRef Optional sCompilerOutput As String = Nothing,
                                              ByRef Optional iCompilerType As ENUM_COMPILER_TYPE = ENUM_COMPILER_TYPE.UNKNOWN) As String
        Try
            If (mTab Is Nothing) Then
                mTab = g_mFormMain.g_ClassTabControl.m_ActiveTab
            End If

            If (sSource Is Nothing) Then
                sSource = mTab.m_TextEditor.Document.TextContent
            End If

            If (mConfig Is Nothing) Then
                mConfig = mTab.m_ActiveConfig
            End If

            If (Not String.IsNullOrEmpty(sEmulateSourceFile)) Then
                With New Text.StringBuilder
                    .AppendFormat("#file ""{0}""", sEmulateSourceFile).AppendLine()
                    .AppendLine("#line 0")
                    .AppendLine(sSource)

                    sSource = .ToString
                End With
            End If

            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Pre-Processing source started!", False, True, True)

            Dim sMarkStart As String = Guid.NewGuid.ToString
            Dim sMarkEnd As String = Guid.NewGuid.ToString

            Dim sOutputFile As String = ""

            Dim iExitCode As Integer = 0
            Dim sOutput As String = ""
            Dim sLines As String()

            'Check compiler
            If (sCompilerPath Is Nothing) Then
                If (mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    Dim sFilePath As String = sFile

                    If (String.IsNullOrEmpty(sFilePath)) Then
                        If (mTab.m_Index < 0) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Could not get current source file!", False, False, True)
                            Return Nothing
                        End If

                        If (mTab.m_IsUnsaved AndAlso
                                    Not g_mFormMain.g_ClassTabControl.PromptSaveTab(mTab.m_Index, False, True, True)) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Could not get current source file!", False, False, True)
                            Return Nothing
                        End If

                        If (mTab.m_IsUnsaved) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Could not get current source file!", False, False, True)
                            Return Nothing
                        End If

                        sFilePath = mTab.m_File
                    End If

                    Dim sFileDirectory As String = IO.Path.GetDirectoryName(sFilePath)
                    If (Not String.IsNullOrEmpty(sCompilerSearchPath)) Then
                        sFileDirectory = sCompilerSearchPath
                    End If

                    While True
                        'SourcePawn
                        sCompilerPath = IO.Path.Combine(sFileDirectory, "spcomp.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'AMX Mod X
                        sCompilerPath = IO.Path.Combine(sFileDirectory, "amxxpc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'Small
                        sCompilerPath = IO.Path.Combine(sFileDirectory, "sc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'Pawn
                        sCompilerPath = IO.Path.Combine(sFileDirectory, "pawncc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Compiler can not be found!", False, False, True)
                        Return Nothing
                    End While
                Else
                    sCompilerPath = mConfig.g_sCompilerPath
                    If (Not IO.File.Exists(sCompilerPath)) Then
                        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Compiler can not be found!", False, False, True)
                        Return Nothing
                    End If
                End If
            Else
                If (Not IO.File.Exists(sCompilerPath)) Then
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Compiler can not be found!", False, False, True)
                    Return Nothing
                End If
            End If

            'Check include path
            If (sIncludePaths Is Nothing) Then
                If (mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    Dim sFilePath As String = sFile

                    If (String.IsNullOrEmpty(sFilePath)) Then
                        If (mTab.m_Index < 0) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Could not get current source file!", False, False, True)
                            Return Nothing
                        End If

                        If (mTab.m_IsUnsaved AndAlso
                                        Not g_mFormMain.g_ClassTabControl.PromptSaveTab(mTab.m_Index, False, True, True)) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Could not get current source file!", False, False, True)
                            Return Nothing
                        End If

                        If (mTab.m_IsUnsaved) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Could not get current source file!", False, False, True)
                            Return Nothing
                        End If

                        sFilePath = mTab.m_File
                    End If

                    Dim sFileDirectory As String = IO.Path.GetDirectoryName(sFilePath)
                    If (Not String.IsNullOrEmpty(sIncludeSearchPath)) Then
                        sFileDirectory = sIncludeSearchPath
                    End If

                    sIncludePaths = IO.Path.Combine(sFileDirectory, "include")
                    If (Not IO.Directory.Exists(sIncludePaths)) Then
                        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Include path can not be found!", False, False, True)
                        Return Nothing
                    End If
                Else
                    sIncludePaths = mConfig.g_sIncludeFolders
                    For Each sInclude As String In sIncludePaths.Split(";"c)
                        If (Not IO.Directory.Exists(sInclude)) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Include path can not be found!", False, False, True)
                            Return Nothing
                        End If
                    Next
                End If
            Else
                If (Not IO.Directory.Exists(sIncludePaths)) Then
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Include path can not be found!", False, False, True)
                    Return Nothing
                End If
            End If


            Dim sTmpSource As String = sSource
            Dim sTmpSourcePath As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))

            sTempOutputFile = sTmpSourcePath

            If (bCleanUpSourcemodDuplicate) Then
                '#file pushes the lines +1 in the main source, add #line 0 to make them even again
                With New Text.StringBuilder
                    .AppendLine("#file " & sMarkStart)
                    .AppendLine("#line 0")
                    .AppendLine(sTmpSource)
                    .AppendLine("#file " & sMarkEnd)

                    sTmpSource = .ToString
                End With
            End If

            IO.File.WriteAllText(sTmpSourcePath, sTmpSource)

            sOutputFile = String.Format("{0}.lst", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))

            Dim lIncludeList As New List(Of String)
            For Each sInclude As String In sIncludePaths.Split(";"c)
                lIncludeList.Add("-i""" & sInclude & """")
            Next

            'Get compiler type first.
            'Normaly every compiler should print help without any process arguments, but AMX Mod X compiler wants input...
            'Use an unfinished argument to force help.
            'TODO: Should detect compiler type using file info instead? This is a bad hack.
            ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, "-", iExitCode, sOutput)

            iCompilerType = ENUM_COMPILER_TYPE.UNKNOWN

            sLines = sOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
            If (sLines.Length > 0) Then
                Select Case (True)
                    Case Regex.IsMatch(sLines(0), "\b(SourcePawn)\b \b(Compiler)\b", RegexOptions.IgnoreCase)
                        iCompilerType = ENUM_COMPILER_TYPE.SOURCEPAWN

                    Case Regex.IsMatch(sLines(0), "\b(AMX)\b \b(Mod)\b \b(X)\b", RegexOptions.IgnoreCase)
                        iCompilerType = ENUM_COMPILER_TYPE.AMXX

                            'Old AMX Mod still uses Small compiler
                    Case Regex.IsMatch(sLines(0), "\b(Pawn)\b \b(compiler)\b", RegexOptions.IgnoreCase), Regex.IsMatch(sLines(0), "\b(Small)\b \b(compiler)\b", RegexOptions.IgnoreCase)
                        iCompilerType = ENUM_COMPILER_TYPE.AMX

                End Select
            End If

            'Build arguments 
            Dim lArguments As New List(Of String) From {
               String.Format("""{0}"" -l {1} -o""{2}""", sTmpSourcePath, String.Join(" ", lIncludeList.ToArray), sOutputFile)
            }

            If (bUseCustomCompilerOptions) Then
                Select Case (iCompilerType)
                    Case ENUM_COMPILER_TYPE.SOURCEPAWN
                        lArguments.Add(mConfig.g_mCompilerOptionsSP.BuildCommandline)

                    Case ENUM_COMPILER_TYPE.AMXX
                        lArguments.Add(mConfig.g_mCompilerOptionsAMXX.BuildCommandline)

                End Select
            End If

            'Compile 
            If (String.IsNullOrEmpty(sWorkingDirectory) OrElse Not IO.Directory.Exists(sWorkingDirectory)) Then
                ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, String.Join(" "c, lArguments.ToArray), iExitCode, sOutput)
            Else
                ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, String.Join(" "c, lArguments.ToArray), sWorkingDirectory, iExitCode, sOutput)
            End If

            sCompilerOutput = sOutput

            IO.File.Delete(sTmpSourcePath)

            sLines = sOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
            For i = 0 To sLines.Length - 1
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & sLines(i), g_mFormMain.g_mUCInformationList.ParseFromCompilerOutput(mTab.m_File, sLines(i)))
            Next

            sCompilerOutput = String.Join(Environment.NewLine, sLines)

            If (String.IsNullOrEmpty(sOutputFile) OrElse Not IO.File.Exists(sOutputFile)) Then
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "Pre-Processing failed! Could not get Pre-Processed source file!", False, False, True)
                Return Nothing
            End If

            Dim sOutputSource As String = IO.File.ReadAllText(sOutputFile)

            IO.File.Delete(sOutputFile)


            Dim sList As New List(Of String)
            Dim bRecord As Boolean = False

            Dim sOutputLine As String
            Using mSR As New IO.StringReader(sOutputSource)
                While True
                    sOutputLine = mSR.ReadLine()
                    If (sOutputLine Is Nothing) Then
                        Exit While
                    End If

                    If (bCleanUpSourcemodDuplicate) Then
                        If (Not bRecord) Then
                            If (sOutputLine.Contains("#file " & sMarkStart)) Then
                                bRecord = True
                                Continue While
                            Else
                                If (sList.Count > 0) Then
                                    sList.Clear()
                                End If
                                Continue While
                            End If
                        Else
                            'Remove invalid lines
                            If (sOutputLine.Contains("#line 0")) Then
                                Continue While
                            End If

                            If (sOutputLine.Contains("#file " & sMarkEnd)) Then
                                Exit While
                            End If
                        End If
                    End If

                    sList.Add(sOutputLine)
                End While
            End Using

            Dim sNewSource = String.Join(Environment.NewLine, sList.ToArray)

            If (bCleanupForCompile) Then
                sNewSource = Regex.Replace(sNewSource, "^\s*#\b(endinput)\b", "", RegexOptions.Multiline)
            End If

            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Pre-Processing source finished!", False, False, True)
            Return sNewSource
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Compiles new assembly from source.
    ''' </summary>
    ''' <param name="bTesting">Just creates a temporary file and removes it after compile.</param>
    ''' <param name="sSource">The source to compile. Use |Nothing| to get the source from the active tab.</param>
    ''' <param name="sOutputFile">The output file. An extension is still required! (default is *.asm)</param>
    ''' <param name="sWorkingDirectory">Sets the compiler working directory. Use |Nothing| to use the working directioy from the active tab.</param>
    ''' <param name="sCompilerPath">The compiler path. Use |Nothing| to get the source from the active tab.</param>
    ''' <param name="sIncludePaths">The include paths seperated by ';'. Use |Nothing| to get the source from the active tab.</param>
    ''' <param name="sEmulateSourceFile">Replaces the printed temporary path.</param>
    ''' <param name="bUseCustomCompilerOptions">If true it will use the configs custom compiler options, false otherwise.</param>
    ''' <param name="sCompilerOutput">The compiler output.</param>
    ''' <param name="iCompilerType"></param>
    ''' <returns>New assembly on success, |Nothing| otherwise.</returns>
    Public Function GetCompilerAssemblyCode(mTab As ClassTabControl.ClassTab,
                                            sFile As String,
                                            sSource As String,
                                            bTesting As Boolean,
                                            ByRef sOutputFile As String,
                                            Optional mConfig As ClassConfigs.STRUC_CONFIG_ITEM = Nothing,
                                            Optional sWorkingDirectory As String = Nothing,
                                            Optional sCompilerPath As String = Nothing,
                                            Optional sCompilerSearchPath As String = Nothing,
                                            Optional sIncludePaths As String = Nothing,
                                            Optional sIncludeSearchPath As String = Nothing,
                                            Optional sEmulateSourceFile As String = Nothing,
                                            Optional bUseCustomCompilerOptions As Boolean = True,
                                            ByRef Optional sCompilerOutput As String = Nothing,
                                            ByRef Optional iCompilerType As ENUM_COMPILER_TYPE = ENUM_COMPILER_TYPE.UNKNOWN) As String
        Try
            If (mTab IsNot Nothing) Then
                mTab = g_mFormMain.g_ClassTabControl.m_ActiveTab
            End If

            If (sSource Is Nothing) Then
                sSource = mTab.m_TextEditor.Document.TextContent
            End If

            If (mConfig Is Nothing) Then
                mConfig = mTab.m_ActiveConfig
            End If

            If (Not String.IsNullOrEmpty(sEmulateSourceFile)) Then
                With New Text.StringBuilder
                    .AppendFormat("#file ""{0}""", sEmulateSourceFile).AppendLine()
                    .AppendLine("#line 0")
                    .AppendLine(sSource)

                    sSource = .ToString
                End With
            End If

            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "DIASM source started!", False, True, True)

            Dim iExitCode As Integer = 0
            Dim sOutput As String = ""
            Dim sLines As String()

            'Check compiler
            If (sCompilerPath Is Nothing) Then
                If (mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    Dim sFilePath As String = sFile

                    If (String.IsNullOrEmpty(sFilePath)) Then
                        If (mTab.m_IsUnsaved AndAlso
                                    Not g_mFormMain.g_ClassTabControl.PromptSaveTab(g_mFormMain.g_ClassTabControl.m_ActiveTabIndex, False, True, True)) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "DIASM failed! Could not get current source file!", False, False, True)
                            Return Nothing
                        End If

                        If (mTab.m_IsUnsaved) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "DIASM failed! Could not get current source file!", False, False, True)
                            Return Nothing
                        End If

                        sFilePath = mTab.m_File
                    End If

                    Dim sFileDirectory As String = IO.Path.GetDirectoryName(sFilePath)
                    If (Not String.IsNullOrEmpty(sCompilerSearchPath)) Then
                        sFileDirectory = sCompilerSearchPath
                    End If

                    While True
                        'SourcePawn
                        sCompilerPath = IO.Path.Combine(sFileDirectory, "spcomp.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'AMX Mod X
                        sCompilerPath = IO.Path.Combine(sFileDirectory, "amxxpc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'Small
                        sCompilerPath = IO.Path.Combine(sFileDirectory, "sc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'Pawn
                        sCompilerPath = IO.Path.Combine(sFileDirectory, "pawncc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "DIASM failed! Compiler can not be found!", False, False, True)
                        Return Nothing
                    End While
                Else
                    sCompilerPath = mConfig.g_sCompilerPath
                    If (Not IO.File.Exists(sCompilerPath)) Then
                        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "DIASM failed! Compiler can not be found!", False, False, True)
                        Return Nothing
                    End If
                End If
            Else
                If (Not IO.File.Exists(sCompilerPath)) Then
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "DIASM failed! Compiler can not be found!", False, False, True)
                    Return Nothing
                End If
            End If

            'Check include path
            If (sIncludePaths Is Nothing) Then
                If (mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    Dim sFilePath As String = sFile

                    If (String.IsNullOrEmpty(sFilePath)) Then
                        If (mTab.m_Index < 0) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "DIASM failed! Could not get current source file!", False, False, True)
                            Return Nothing
                        End If

                        If (mTab.m_IsUnsaved AndAlso
                                    Not g_mFormMain.g_ClassTabControl.PromptSaveTab(mTab.m_Index, False, True, True)) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "DIASM failed! Could not get current source file!", False, False, True)
                            Return Nothing
                        End If

                        If (mTab.m_IsUnsaved) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "DIASM failed! Could not get current source file!", False, False, True)
                            Return Nothing
                        End If

                        sFilePath = mTab.m_File
                    End If

                    Dim sFileDirectory As String = IO.Path.GetDirectoryName(sFilePath)
                    If (Not String.IsNullOrEmpty(sIncludeSearchPath)) Then
                        sFileDirectory = sIncludeSearchPath
                    End If

                    sIncludePaths = IO.Path.Combine(sFileDirectory, "include")
                    If (Not IO.Directory.Exists(sIncludePaths)) Then
                        g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "DIASM failed! Include path can not be found!", False, False, True)
                        Return Nothing
                    End If
                Else
                    sIncludePaths = mConfig.g_sIncludeFolders
                    For Each sInclude As String In sIncludePaths.Split(";"c)
                        If (Not IO.Directory.Exists(sInclude)) Then
                            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "DIASM failed! Include path can not be found!", False, False, True)
                            Return Nothing
                        End If
                    Next
                End If
            Else
                If (Not IO.Directory.Exists(sIncludePaths)) Then
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "DIASM failed! Include path can not be found!", False, False, True)
                    Return Nothing
                End If
            End If

            'Set output path
            If (bTesting) Then
                sOutputFile = String.Format("{0}.asm", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
            Else
                If (String.IsNullOrEmpty(sOutputFile)) Then
                    Throw New ArgumentException("Invalid output file")
                End If
            End If

            Dim TmpSourceFile As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
            IO.File.WriteAllText(TmpSourceFile, sSource)

            Dim lIncludeList As New List(Of String)
            For Each sInclude As String In sIncludePaths.Split(";"c)
                lIncludeList.Add("-i""" & sInclude & """")
            Next

            'Get compiler type first.
            'Normaly every compiler should print help without any process arguments, but AMX Mod X compiler wants input...
            'Use an unfinished argument to force help.
            'TODO: Should detect compiler type using file info instead? This is a bad hack.
            ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, "-", iExitCode, sOutput)

            iCompilerType = ENUM_COMPILER_TYPE.UNKNOWN

            sLines = sOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
            If (sLines.Length > 0) Then
                Select Case (True)
                    Case Regex.IsMatch(sLines(0), "\b(SourcePawn)\b \b(Compiler)\b", RegexOptions.IgnoreCase)
                        iCompilerType = ENUM_COMPILER_TYPE.SOURCEPAWN

                    Case Regex.IsMatch(sLines(0), "\b(AMX)\b \b(Mod)\b \b(X)\b", RegexOptions.IgnoreCase)
                        iCompilerType = ENUM_COMPILER_TYPE.AMXX

                            'Old AMX Mod still uses Small compiler
                    Case Regex.IsMatch(sLines(0), "\b(Pawn)\b \b(compiler)\b", RegexOptions.IgnoreCase), Regex.IsMatch(sLines(0), "\b(Small)\b \b(compiler)\b", RegexOptions.IgnoreCase)
                        iCompilerType = ENUM_COMPILER_TYPE.AMX

                End Select
            End If

            'Build arguments 
            Dim lArguments As New List(Of String) From {
                String.Format("""{0}"" -a {1} -o""{2}""", TmpSourceFile, String.Join(" ", lIncludeList.ToArray), sOutputFile)
            }

            If (bUseCustomCompilerOptions) Then
                Select Case (iCompilerType)
                    Case ENUM_COMPILER_TYPE.SOURCEPAWN
                        lArguments.Add(mConfig.g_mCompilerOptionsSP.BuildCommandline)

                    Case ENUM_COMPILER_TYPE.AMXX
                        lArguments.Add(mConfig.g_mCompilerOptionsAMXX.BuildCommandline)

                End Select
            End If

            'Compile 
            If (String.IsNullOrEmpty(sWorkingDirectory) OrElse Not IO.Directory.Exists(sWorkingDirectory)) Then
                ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, String.Join(" "c, lArguments.ToArray), iExitCode, sOutput)
            Else
                ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, String.Join(" "c, lArguments.ToArray), sWorkingDirectory, iExitCode, sOutput)
            End If

            sCompilerOutput = sOutput

            sLines = sOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
            For i = 0 To sLines.Length - 1
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & sLines(i), g_mFormMain.g_mUCInformationList.ParseFromCompilerOutput(mTab.m_File, sLines(i)))
            Next

            sCompilerOutput = String.Join(Environment.NewLine, sLines)

            Dim sAssemblySource As String = IO.File.ReadAllText(sOutputFile)

            IO.File.Delete(TmpSourceFile)

            If (bTesting) Then
                IO.File.Delete(sOutputFile)
            End If

            If (Not bTesting) Then
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("Saved DIASM source: {0}", sOutputFile), New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN(sOutputFile))
            End If

            g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "DIASM source finished!", False, False, True)
            Return sAssemblySource
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Class for custom color highlighting.
    ''' </summary>
    Public Class ClassCustomHighlighting
        Implements IDisposable

        Private g_mFormMain As FormMain
        Private Const g_sColorTextName As String = "Highlighting"

        Class STRUC_HIGHLIGHT_ITEM
            Public sIdentifier As String
            Public sWord As String
            Public mToolStripItem As ToolStripMenuItem
        End Class

        Private g_lHighlightItemList As New List(Of STRUC_HIGHLIGHT_ITEM)

        ReadOnly Property m_HightlightItems As STRUC_HIGHLIGHT_ITEM()
            Get
                Return g_lHighlightItemList.ToArray
            End Get
        End Property

        Public Sub New(f As FormMain)
            g_mFormMain = f
        End Sub

        ''' <summary>
        ''' Adds a highlight item. If exist, it will be ignored.
        ''' The highlight list will automatically allocated.
        ''' </summary>
        ''' <param name="iIndex"></param>
        Public Sub Add(iIndex As Integer)
            AllocateList(iIndex)

            If (g_lHighlightItemList(iIndex) IsNot Nothing) Then
                Return
            End If

            Dim sIdentifier = Guid.NewGuid.ToString

            Dim mMenuItem As ToolStripMenuItem = DirectCast(g_mFormMain.ToolStripMenuItem_HightlightCustom.DropDownItems.Add(String.Format("{0} {1}", g_sColorTextName, iIndex)), ToolStripMenuItem)
            mMenuItem.Name = sIdentifier
            mMenuItem.BackColor = Color.White

            RemoveHandler mMenuItem.Click, AddressOf OnClick
            AddHandler mMenuItem.Click, AddressOf OnClick

            g_lHighlightItemList(iIndex) = New STRUC_HIGHLIGHT_ITEM With {
                .sIdentifier = sIdentifier,
                .sWord = "",
                .mToolStripItem = mMenuItem
            }

        End Sub

        ''' <summary>
        ''' Cleans the highlight list, events and disposes all ToolStrip controls.
        ''' </summary>
        Public Sub Clear()
            For i = 0 To g_lHighlightItemList.Count - 1
                If (g_lHighlightItemList(i) Is Nothing) Then
                    Continue For
                End If

                'Remove Handlers
                If (g_lHighlightItemList(i).mToolStripItem IsNot Nothing) Then
                    RemoveHandler g_lHighlightItemList(i).mToolStripItem.Click, AddressOf OnClick
                End If

                'Remove Controls
                If (g_lHighlightItemList(i).mToolStripItem IsNot Nothing AndAlso Not g_lHighlightItemList(i).mToolStripItem.IsDisposed) Then
                    g_lHighlightItemList(i).mToolStripItem.Dispose()
                    g_lHighlightItemList(i).mToolStripItem = Nothing
                End If
            Next

            g_lHighlightItemList.Clear()
        End Sub

        Public Function AllocateList(iSize As Integer) As Boolean
            Dim bAllocated As Boolean = False

            While (iSize > g_lHighlightItemList.Count - 1)
                bAllocated = True
                g_lHighlightItemList.Add(Nothing)
            End While

            Return bAllocated
        End Function

        Private Sub OnClick(sender As Object, e As EventArgs)
            Try
                Dim toolStripItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
                If (toolStripItem Is Nothing) Then
                    Throw New ArgumentException("Invalid ToolStripMenuItem")
                End If

                Dim sIdentifier As String = toolStripItem.Name
                If (String.IsNullOrEmpty(sIdentifier)) Then
                    Throw New ArgumentException("Invalid ToolStripMenuItem name")
                End If

                Dim iIndex As Integer = -1
                Dim highlightItem As STRUC_HIGHLIGHT_ITEM = Nothing

                For i = 0 To g_lHighlightItemList.Count - 1
                    If (g_lHighlightItemList(i) Is Nothing) Then
                        Continue For
                    End If

                    If (g_lHighlightItemList(i).sIdentifier <> sIdentifier) Then
                        Continue For
                    End If

                    iIndex = i
                    highlightItem = g_lHighlightItemList(i)
                Next

                If (highlightItem Is Nothing OrElse iIndex < 0) Then
                    Throw New ArgumentException("No matching ToolStripMenuItem found")
                End If

                highlightItem.sWord = ""

                Dim mActiveTextEditor As TextEditorControlEx = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor

                If (mActiveTextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                    Dim m_CurrentSelection As ISelection = mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0)

                    If (Regex.IsMatch(m_CurrentSelection.SelectedText, "^[a-zA-Z0-9_]+$")) Then
                        highlightItem.sWord = m_CurrentSelection.SelectedText
                    End If
                Else
                    Dim sWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(g_mFormMain.g_ClassTabControl.m_ActiveTab, False, False, False)

                    If (Not String.IsNullOrEmpty(sWord)) Then
                        highlightItem.sWord = sWord
                    End If
                End If

                If (String.IsNullOrEmpty(highlightItem.sWord)) Then
                    toolStripItem.Text = String.Format("{0} {1}", g_sColorTextName, iIndex)
                Else
                    toolStripItem.Text = String.Format("{0} {1} {2}", g_sColorTextName, iIndex, "(Visible)")
                End If

                g_mFormMain.g_ClassSyntaxTools.g_ClassSyntaxHighlighting.UpdateSyntax(ClassSyntaxTools.ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD_CUSTOM)
                g_mFormMain.g_ClassSyntaxTools.g_ClassSyntaxHighlighting.UpdateTextEditorSyntax()
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    Clear()
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class


    Class ClassBookmarkMark
        Inherits Bookmark

        ReadOnly Property m_Id As String
        ReadOnly Property m_Name As String
        ReadOnly Property m_CreatedTimestamp As Date

        Public Sub New(iDocument As IDocument, mTextLocation As TextLocation, sName As String)
            MyBase.New(Nothing, mTextLocation, True)
            Me.m_Id = Guid.NewGuid.ToString
            Me.m_Name = sName
            Me.m_CreatedTimestamp = Now
        End Sub

        Public Sub New(iDocument As IDocument, mTextLocation As TextLocation, mCreateTimestamp As Date, sName As String)
            Me.New(Nothing, mTextLocation, sName)
            Me.m_CreatedTimestamp = mCreateTimestamp
        End Sub

        Public Overrides ReadOnly Property CanToggle As Boolean
            Get
                Return False
            End Get
        End Property

        Protected Overrides Sub OnIsEnabledChanged(e As EventArgs)
            Me.IsEnabled = True
        End Sub

        ''' <summary>
        ''' Lets override the bookmarks drawing, so we can show our custom coloring!
        ''' The bookmark is the only real icon we can change.
        ''' </summary>
        ''' <param name="mIconBarMargin"></param>
        ''' <param name="mGraphics"></param>
        ''' <param name="mPoint"></param>
        Public Overrides Sub Draw(mIconBarMargin As IconBarMargin, mGraphics As Graphics, mPoint As Point)
            Dim mRect As New Rectangle(1,
                                    mPoint.Y + 1,
                                    mIconBarMargin.DrawingPosition.Width - 2,
                                    mIconBarMargin.TextArea.TextView.FontHeight - 2)

            mGraphics.DrawImage(My.Resources.Unpin_16x16_32, mRect)
        End Sub
    End Class

    Class ClassLineStateMark
        Inherits Bookmark

        Enum ENUM_STATE
            NONE
            CHANGED
            SAVED
        End Enum

        Property m_Type As ENUM_STATE
        ReadOnly Property m_Id As String

        Public Sub New(iDocument As IDocument, textLocation As TextLocation, iType As ENUM_STATE)
            MyBase.New(iDocument, textLocation, True)
            Me.m_Type = iType
            Me.m_Id = Guid.NewGuid.ToString
        End Sub

        Public Overrides ReadOnly Property CanToggle As Boolean
            Get
                Return False
            End Get
        End Property

        Protected Overrides Sub OnIsEnabledChanged(e As EventArgs)
            Me.IsEnabled = True
        End Sub

        ''' <summary>
        ''' Lets override the bookmarks drawing, so we can show our custom coloring!
        ''' The bookmark is the only real icon we can change.
        ''' </summary>
        ''' <param name="mIconBarMargin"></param>
        ''' <param name="mGraphics"></param>
        ''' <param name="mPoint"></param>
        Public Overrides Sub Draw(mIconBarMargin As IconBarMargin, mGraphics As Graphics, mPoint As Point)
            Dim iFontHeight As Integer = CInt(mIconBarMargin.TextArea.TextView.FontHeight / 32)
            Dim mRect As New Rectangle(CInt(mIconBarMargin.DrawingPosition.Width / 1.25),
                                        mPoint.Y + iFontHeight,
                                        mIconBarMargin.DrawingPosition.Width - 4,
                                        mIconBarMargin.TextArea.TextView.FontHeight - iFontHeight * 2)

            Dim iColor As Color = If(m_Type = ENUM_STATE.CHANGED, Color.Orange, Color.Green)

            Using mBrush As New Drawing2D.LinearGradientBrush(New Point(mRect.Left, mRect.Top), New Point(mRect.Right, mRect.Bottom), iColor, iColor)
                mGraphics.FillRectangle(mBrush, mRect)
            End Using
        End Sub
    End Class

    ''' <summary>
    ''' Class for text editor custom commands.
    ''' </summary>
    Class ClassTestEditorCommands
        Private g_mFormMain As FormMain

        Private Shared g_lCommands As New ClassSyncList(Of ITextEditorCommand)

        Public Sub New(f As FormMain)
            g_mFormMain = f
        End Sub

        Shared ReadOnly Property m_Commands As ClassSyncList(Of ITextEditorCommand)
            Get
                Return g_lCommands
            End Get
        End Property

        Public Function AddCommand(mCmd As ITextEditorCommand) As Boolean
            For Each mItem In g_lCommands
                If (mItem.m_Command.ToLower = mCmd.m_Command.ToLower) Then
                    Return False
                End If
            Next

            g_lCommands.Add(mCmd)
            Return True
        End Function

        Public Interface ITextEditorCommand
            ReadOnly Property m_Command As String

            Sub Execute(sArg As String)
        End Interface
    End Class

    Class ClassCompilerBuildEvents
        Public Class STRUC_BUILD_MACRO
            Public sMacro As String = ""
            Public sDescription As String = ""
            Public sInput As String = ""

            Public Sub New(_Macro As String, _Description As String, _Input As String)
                sMacro = _Macro
                sDescription = _Description
                sInput = _Input
            End Sub
        End Class

        ReadOnly Property m_BuildMacros As New List(Of STRUC_BUILD_MACRO)

        ReadOnly Property m_File As String
        ReadOnly Property m_IncludePath As String
        ReadOnly Property m_CompilerPath As String
        ReadOnly Property m_OutputPath As String
        ReadOnly Property m_Testing As Boolean
        ReadOnly Property m_WorkingDirectory As String

        ReadOnly Property m_PreBuildCmd As String
        ReadOnly Property m_PostBuildCmd As String

        Sub New(mConfig As ClassConfigs.STRUC_CONFIG_ITEM, _File As String, _IncludePath As String, _CompilerPath As String, _OutputPath As String, _Testing As Boolean, _WorkingDirectory As String)
            Me.New(mConfig.g_sBuildEventPreCmd, mConfig.g_sBuildEventPostCmd, _File, _IncludePath, _CompilerPath, _OutputPath, _Testing, _WorkingDirectory)
        End Sub

        Sub New(_BuildEventPreCmd As String, _BuildEventPostCmd As String, _File As String, _IncludePath As String, _CompilerPath As String, _OutputPath As String, _Testing As Boolean, _WorkingDirectory As String)
            m_PreBuildCmd = _BuildEventPreCmd
            m_PostBuildCmd = _BuildEventPostCmd

            m_File = _File
            m_IncludePath = _IncludePath
            m_CompilerPath = _CompilerPath
            m_OutputPath = _OutputPath
            m_Testing = _Testing
            m_WorkingDirectory = _WorkingDirectory

            RebuildMacros()
        End Sub

        Public Sub RebuildMacros()
            m_BuildMacros.Clear()

            m_BuildMacros.Add(New STRUC_BUILD_MACRO("bp_source_file", "Source file", m_File))
            m_BuildMacros.Add(New STRUC_BUILD_MACRO("bp_source_filename", "Source filename", If(String.IsNullOrEmpty(m_File), "", IO.Path.GetFileNameWithoutExtension(m_File))))
            m_BuildMacros.Add(New STRUC_BUILD_MACRO("bp_source_fileext", "Source file extension", If(String.IsNullOrEmpty(m_File), "", IO.Path.GetExtension(m_File))))
            m_BuildMacros.Add(New STRUC_BUILD_MACRO("bp_source_directory", "Source directory", If(String.IsNullOrEmpty(m_File), "", IO.Path.GetDirectoryName(m_File))))

            m_BuildMacros.Add(New STRUC_BUILD_MACRO("bp_conf_includes", "Include folders (seperated by ';')", m_IncludePath))
            m_BuildMacros.Add(New STRUC_BUILD_MACRO("bp_conf_compiler", "Compiler path", m_CompilerPath))
            m_BuildMacros.Add(New STRUC_BUILD_MACRO("bp_conf_output", "Output path", m_OutputPath))

            m_BuildMacros.Add(New STRUC_BUILD_MACRO("bp_startdir", "BasicPawn startup directory", Application.StartupPath))
            m_BuildMacros.Add(New STRUC_BUILD_MACRO("bp_testing", "True if testing only, false if building binary", If(m_Testing, "true", "false")))
        End Sub

        Public Sub ExecPre(ByRef r_iExitCode As Integer, ByRef r_sOutput As String)
            Dim sBatFile As String = BuildBatch(m_PreBuildCmd)

            BuildProcess(sBatFile, r_iExitCode, r_sOutput)

            IO.File.Delete(sBatFile)
        End Sub

        Public Sub ExecPost(ByRef r_iExitCode As Integer, ByRef r_sOutput As String)
            Dim sBatFile As String = BuildBatch(m_PostBuildCmd)

            BuildProcess(sBatFile, r_iExitCode, r_sOutput)

            IO.File.Delete(sBatFile)
        End Sub

        Private Function BuildBatch(sCmd As String) As String
            Dim sBatFile As String = IO.Path.Combine(IO.Path.GetTempPath(), Guid.NewGuid.ToString & ".bat")

            'Make command line output cleaner by adding '@echo off'
            'The user can turn it back on with '@echo on'
            Dim mCmd As New Text.StringBuilder
            mCmd.AppendLine("@echo off")
            mCmd.AppendLine(sCmd)

            IO.File.WriteAllText(sBatFile, mCmd.ToString)

            Return sBatFile
        End Function

        Private Sub BuildProcess(sBatFile As String, ByRef r_iExitCode As Integer, ByRef r_sOutput As String)
            r_iExitCode = -1
            r_sOutput = ""

            Dim mMacros As New Dictionary(Of String, String)

            For Each mMacro In m_BuildMacros
                mMacros(mMacro.sMacro) = mMacro.sInput
            Next

            ClassTools.ClassProcess.ExecuteProgram("cmd.exe", String.Format("/c ""{0}""", sBatFile), m_WorkingDirectory, mMacros, r_iExitCode, r_sOutput)
        End Sub
    End Class
End Class
