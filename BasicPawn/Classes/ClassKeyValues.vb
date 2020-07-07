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


Imports System.Text
Imports System.Text.RegularExpressions

Public Class ClassKeyValues
    Implements IDisposable

    Private g_mStream As IO.Stream
    Private g_mStreamWriter As IO.StreamWriter
    Private g_mStreamReader As IO.StreamReader

    'https://en.wikipedia.org/wiki/Escape_sequences_in_C
    Private Shared g_sEscapes()() As String = New String()() {
        New String() {"\\", "\"},
        New String() {"\a", Chr(&H7)},
        New String() {"\b", Chr(&H8)},
        New String() {"\e", Chr(&H1B)},
        New String() {"\f", Chr(&HC)},
        New String() {"\n", Chr(&HA)},
        New String() {"\r", Chr(&HD)},
        New String() {"\t", Chr(&H9)},
        New String() {"\v", Chr(&HB)},
        New String() {"\'", Chr(&H27)},
        New String() {"\""", Chr(&H22)},
        New String() {"\?", Chr(&H3F)}
    }

    Class STRUC_KEYVALUES_SECTION
        Private g_sName As String
        Private g_mKeys As New List(Of KeyValuePair(Of String, String))
        Private g_mSections As ClassKeyValueList

        Private g_mParent As STRUC_KEYVALUES_SECTION

        Public Sub New(sName As String)
            g_sName = sName

            g_mSections = New ClassKeyValueList(Me)
        End Sub

        ReadOnly Property m_Name As String
            Get
                Return g_sName
            End Get
        End Property

        ReadOnly Property m_Keys As List(Of KeyValuePair(Of String, String))
            Get
                Return g_mKeys
            End Get
        End Property

        ReadOnly Property m_Sections As ClassKeyValueList
            Get
                Return g_mSections
            End Get
        End Property

        ReadOnly Property m_Root As STRUC_KEYVALUES_SECTION
            Get
                Dim mKeyValue = Me

                While True
                    If (mKeyValue.m_Parent Is Nothing) Then
                        Exit While
                    End If

                    mKeyValue = mKeyValue.m_Parent
                End While

                Return mKeyValue
            End Get
        End Property

        Property m_Parent As STRUC_KEYVALUES_SECTION
            Get
                Return g_mParent
            End Get
            Set(value As STRUC_KEYVALUES_SECTION)
                g_mParent = value
            End Set
        End Property

        Public Function FindSection(sName As String) As STRUC_KEYVALUES_SECTION
            For Each mItem In m_Sections
                If (mItem.g_sName IsNot Nothing AndAlso mItem.g_sName = sName) Then
                    Return mItem
                End If
            Next

            Return Nothing
        End Function

        Public Function FindSectionAll(sName As String) As STRUC_KEYVALUES_SECTION()
            Dim mSections As New List(Of STRUC_KEYVALUES_SECTION)

            For Each mItem In m_Sections
                If (mItem.g_sName IsNot Nothing AndAlso mItem.g_sName = sName) Then
                    mSections.Add(mItem)
                End If
            Next

            Return mSections.ToArray
        End Function

        Public Function GetRootSection() As STRUC_KEYVALUES_SECTION
            Dim mItem As STRUC_KEYVALUES_SECTION = Me

            While True
                If (mItem.m_Parent Is Nothing) Then
                    Exit While
                End If

                mItem = mItem.m_Parent
            End While

            Return mItem
        End Function

        Class ClassKeyValueList
            Implements IList(Of STRUC_KEYVALUES_SECTION), ICollection(Of STRUC_KEYVALUES_SECTION), IEnumerable(Of STRUC_KEYVALUES_SECTION)

            Private g_mList As List(Of STRUC_KEYVALUES_SECTION)
            Private g_mParent As STRUC_KEYVALUES_SECTION

            Public Sub New(mParent As STRUC_KEYVALUES_SECTION)
                g_mList = New List(Of STRUC_KEYVALUES_SECTION)
                g_mParent = mParent
            End Sub

            Public Sub New(mParent As STRUC_KEYVALUES_SECTION, mItems As STRUC_KEYVALUES_SECTION())
                Me.New(mParent)

                For Each mItem In mItems
                    Me.Add(mItem)
                Next
            End Sub

            Default Public Property Item(index As Integer) As STRUC_KEYVALUES_SECTION Implements IList(Of STRUC_KEYVALUES_SECTION).Item
                Get
                    Return g_mList(index)
                End Get
                Set(value As STRUC_KEYVALUES_SECTION)
                    g_mList(index) = value
                End Set
            End Property

            Public ReadOnly Property Count As Integer Implements ICollection(Of STRUC_KEYVALUES_SECTION).Count
                Get
                    Return g_mList.Count
                End Get
            End Property

            Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of STRUC_KEYVALUES_SECTION).IsReadOnly
                Get
                    Return False
                End Get
            End Property

            Public Sub Insert(index As Integer, item As STRUC_KEYVALUES_SECTION) Implements IList(Of STRUC_KEYVALUES_SECTION).Insert
                item.m_Parent = g_mParent
                g_mList.Insert(index, item)
            End Sub

            Public Sub RemoveAt(index As Integer) Implements IList(Of STRUC_KEYVALUES_SECTION).RemoveAt
                g_mList.RemoveAt(index)
            End Sub

            Public Sub Add(item As STRUC_KEYVALUES_SECTION) Implements ICollection(Of STRUC_KEYVALUES_SECTION).Add
                item.m_Parent = g_mParent
                g_mList.Add(item)
            End Sub

            Public Sub Clear() Implements ICollection(Of STRUC_KEYVALUES_SECTION).Clear
                g_mList.Clear()
            End Sub

            Public Sub CopyTo(array() As STRUC_KEYVALUES_SECTION, arrayIndex As Integer) Implements ICollection(Of STRUC_KEYVALUES_SECTION).CopyTo
                g_mList.CopyTo(array, arrayIndex)
            End Sub

            Public Function IndexOf(item As STRUC_KEYVALUES_SECTION) As Integer Implements IList(Of STRUC_KEYVALUES_SECTION).IndexOf
                Return g_mList.IndexOf(item)
            End Function

            Public Function Contains(item As STRUC_KEYVALUES_SECTION) As Boolean Implements ICollection(Of STRUC_KEYVALUES_SECTION).Contains
                Return g_mList.Contains(item)
            End Function

            Public Function Remove(item As STRUC_KEYVALUES_SECTION) As Boolean Implements ICollection(Of STRUC_KEYVALUES_SECTION).Remove
                Return g_mList.Remove(item)
            End Function

            Public Function GetEnumerator() As IEnumerator(Of STRUC_KEYVALUES_SECTION) Implements IEnumerable(Of STRUC_KEYVALUES_SECTION).GetEnumerator
                Return g_mList.GetEnumerator
            End Function

            Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Return g_mList.GetEnumerator
            End Function
        End Class
    End Class

    Public Sub New()
        Me.New(New IO.MemoryStream())
    End Sub

    Public Sub New(sContent As String)
        Me.New(New IO.MemoryStream())

        g_mStreamWriter.Write(sContent)
    End Sub

    Public Sub New(sFile As String, iMode As IO.FileMode)
        Me.New(New IO.FileStream(sFile, iMode, IO.FileAccess.ReadWrite))
    End Sub

    Public Sub New(mStream As IO.Stream)
        g_mStream = mStream

        If (g_mStream.CanWrite) Then
            g_mStreamWriter = New IO.StreamWriter(mStream)
            g_mStreamWriter.AutoFlush = True
        End If

        If (g_mStream.CanRead) Then
            g_mStreamReader = New IO.StreamReader(mStream)
        End If
    End Sub

    Public Function Deserialize(bUnescapeString As Boolean) As STRUC_KEYVALUES_SECTION
        g_mStreamReader.BaseStream.Seek(0, IO.SeekOrigin.Begin)

        Dim sContent As String = g_mStreamReader.ReadToEnd

        Dim mKeyValues As New STRUC_KEYVALUES_SECTION(Nothing)
        Dim mSections As New ClassSyncList(Of STRUC_KEYVALUES_SECTION)
        mSections.Add(mKeyValues)

        Dim iLastBraceLevel As Integer = 0
        Dim sTotalStrings As New StringBuilder

        Dim mKeyValueAnalysis As New ClassKeyValueAnalysis(sContent)
        For i = 0 To sContent.Length - 1
            If (mKeyValueAnalysis.GetBracketLevel(i, Nothing) > 0) Then
                Throw New ArgumentException(String.Format("Conditions not supported. Offset: {0}", i))
            End If

            If (mKeyValueAnalysis.m_InPreprocessor(i)) Then
                Throw New ArgumentException(String.Format("Recursive includes not supported. Offset: {0}", i))
            End If

            If (mKeyValueAnalysis.m_InSingleComment(i) OrElse mKeyValueAnalysis.m_InMultiComment(i)) Then
                Continue For
            End If

            Dim iScopeLevel As Integer = mKeyValueAnalysis.GetBraceLevel(i, Nothing)

            If (iScopeLevel <> iLastBraceLevel) Then
                If (sTotalStrings.Length > 0) Then
                    Dim mStrings As New List(Of String)

                    'Split found strings into array and also handle escapes.
                    If (True) Then
                        Dim mStringAnalysis As New ClassKeyValueAnalysis(sTotalStrings.ToString)

                        Dim bReading As Boolean = False
                        Dim sBuffer As String = ""
                        For j = 0 To mStringAnalysis.m_MaxLength - 1
                            If (mStringAnalysis.GetChar(j) = """"c) Then
                                If (Not mStringAnalysis.IsEscaped(j)) Then
                                    'Begin reading string
                                    If (Not bReading) Then
                                        bReading = True

                                        sBuffer = ""
                                        Continue For
                                    End If

                                    'End reading string
                                    If (bReading) Then
                                        bReading = False

                                        mStrings.Add(sBuffer)
                                        Continue For
                                    End If
                                End If
                            End If

                            If (bReading) Then
                                sBuffer &= mStringAnalysis.GetChar(j)
                            End If
                        Next
                    End If


                    Dim sKey As String = Nothing
                    Dim sValue As String = Nothing

                    Dim mSortedKeyValues As New List(Of KeyValuePair(Of String, String))

                    For j = 0 To mStrings.Count - 1
                        Select Case (j Mod 2)
                            Case 0
                                sKey = If(bUnescapeString, UnescapeString(mStrings(j)), mStrings(j))
                                sValue = Nothing

                            Case 1
                                sValue = If(bUnescapeString, UnescapeString(mStrings(j)), mStrings(j))

                                mSortedKeyValues.Add(New KeyValuePair(Of String, String)(sKey, sValue))
                        End Select
                    Next

                    If (sKey IsNot Nothing) Then
                        Dim mSection As STRUC_KEYVALUES_SECTION = mSections(mSections.Count - 1)

                        'Setting value to key
                        For Each mItem In mSortedKeyValues
                            mSection.m_Keys.Add(New KeyValuePair(Of String, String)(mItem.Key, mItem.Value))
                        Next

                        'Did keyvalues end without value? Must be section.
                        If (sValue Is Nothing) Then
                            If (iScopeLevel > iLastBraceLevel) Then
                                Dim mChildKeyValue As New STRUC_KEYVALUES_SECTION(sKey)

                                mSection.m_Sections.Add(mChildKeyValue)
                                mSections.Add(mChildKeyValue)
                            End If
                        End If
                    End If


                    sTotalStrings = New StringBuilder
                End If
            End If

            'Pop last section when we move back
            If (iScopeLevel < iLastBraceLevel) Then
                If (mSections.Count > 0) Then
                    mSections.PopLast()
                End If
            End If

            If (mKeyValueAnalysis.m_InString(i)) Then
                sTotalStrings.Append(mKeyValueAnalysis.GetChar(i))
            Else
                Dim sChr As Char = mKeyValueAnalysis.GetChar(i)
                Static sIgnoreChars As Char() = New Char() {""""c, "{"c, "}"c, "["c, "]"c}

                If (Not Char.IsWhiteSpace(sChr) AndAlso Array.IndexOf(sIgnoreChars, sChr) = -1) Then
                    Throw New ArgumentException(String.Format("Section/Key/Value must be string. Offset: {0}", i))
                End If
            End If

            iLastBraceLevel = iScopeLevel
        Next

        Return mKeyValues
    End Function

    Public Sub Serialize(mKeyValues As STRUC_KEYVALUES_SECTION, bEscapeStrings As Boolean)
        Dim mKeyValueBuilder As New StringBuilder

        SerializeSectionRecursive(mKeyValueBuilder, mKeyValues, 0, bEscapeStrings)

        ParseFromString(mKeyValueBuilder.ToString)
    End Sub

    Private Sub SerializeSectionRecursive(mKeyValueBuilder As StringBuilder, mKeyValues As STRUC_KEYVALUES_SECTION, iTabbing As Integer, bEscapeStrings As Boolean)
        If (mKeyValues.m_Name Is Nothing) Then
            For Each mSection In mKeyValues.m_Sections
                SerializeSectionRecursive(mKeyValueBuilder, mSection, iTabbing, bEscapeStrings)
            Next
        Else
            Dim sTabbing = New String(vbTab(0), iTabbing)
            Dim sKeyTabbing = New String(vbTab(0), iTabbing + 1)

            mKeyValueBuilder.AppendFormat("{0}""{1}""", sTabbing, If(bEscapeStrings, EscapeString(mKeyValues.m_Name), mKeyValues.m_Name)).AppendLine()
            mKeyValueBuilder.AppendFormat("{0}{1}", sTabbing, "{").AppendLine()

            For Each mKeys In mKeyValues.m_Keys
                mKeyValueBuilder.AppendFormat("{0}""{1}""{2}""{3}""", sKeyTabbing, If(bEscapeStrings, EscapeString(mKeys.Key), mKeys.Key), vbTab(0), If(bEscapeStrings, EscapeString(mKeys.Value), mKeys.Value)).AppendLine()
            Next

            For Each mSection In mKeyValues.m_Sections
                SerializeSectionRecursive(mKeyValueBuilder, mSection, iTabbing + 1, bEscapeStrings)
            Next

            mKeyValueBuilder.AppendFormat("{0}{1}", sTabbing, "}").AppendLine()
        End If
    End Sub

    Public Shared Function UnescapeString(sText As String) As String
        For i = sText.Length - 1 To 0 Step -1
            If (sText(i) <> "\"c) Then
                Continue For
            End If

            Dim iEscCount = 0
            For j = i - 1 To 0 Step -1
                If (sText(j) <> "\"c) Then
                    Exit For
                End If

                iEscCount += 1
            Next

            'Has been escaped?
            If ((iEscCount Mod 2) <> 0) Then
                Continue For
            End If

            For j = g_sEscapes.Length - 1 To 0 Step -1
                Dim iEscIndex = (i + g_sEscapes(j)(0).Length - 1)
                If (iEscIndex > sText.Length - 1) Then
                    Continue For
                End If

                Dim sTextEsc As String = sText.Substring(i, g_sEscapes(j)(0).Length)
                If (g_sEscapes(j)(0) <> sTextEsc) Then
                    Continue For
                End If

                sText = sText.Remove(i, sTextEsc.Length)
                sText = sText.Insert(i, g_sEscapes(j)(1))
            Next
        Next

        Return sText
    End Function

    Public Shared Function EscapeString(sText As String) As String
        For i = 0 To g_sEscapes.Length - 1
            sText = sText.Replace(g_sEscapes(i)(1), g_sEscapes(i)(0))
        Next

        Return sText
    End Function

    ''' <summary>
    ''' Exports the IO.Stream content to file.
    ''' </summary>
    ''' <param name="sFile"></param>
    Public Sub ExportToFile(sFile As String)
        g_mStream.Seek(0, IO.SeekOrigin.Begin)

        Using mFile As New IO.FileStream(sFile, IO.FileMode.OpenOrCreate, IO.FileAccess.Write)
            mFile.SetLength(0)

            CopyStream(g_mStream, mFile, 1024 * 8)
        End Using
    End Sub

    ''' <summary>
    ''' Exports the IO.Stream content to string.
    ''' </summary>
    ''' <returns></returns>
    Public Function ExportToString() As String
        g_mStream.Seek(0, IO.SeekOrigin.Begin)
        Return g_mStreamReader.ReadToEnd
    End Function

    ''' <summary>
    ''' Parses the file content to IO.Stream.
    ''' </summary>
    ''' <param name="sFile"></param>
    Public Sub ParseFromFile(sFile As String)
        g_mStream.Seek(0, IO.SeekOrigin.Begin)
        g_mStream.SetLength(0)

        Using mFile As New IO.FileStream(sFile, IO.FileMode.Open, IO.FileAccess.Read)
            CopyStream(mFile, g_mStream, 1024 * 8)
        End Using
    End Sub

    ''' <summary>
    ''' Parses the string content to IO.Stream.
    ''' </summary>
    ''' <param name="sText"></param>
    Public Sub ParseFromString(sText As String)
        g_mStreamWriter.BaseStream.Seek(0, IO.SeekOrigin.Begin)
        g_mStreamWriter.BaseStream.SetLength(0)
        g_mStreamWriter.Write(sText)
    End Sub

    Private Sub CopyStream(mInput As IO.Stream, mOutput As IO.Stream, iBufferSize As Integer)
        Dim iBuffer As Byte() = New Byte(iBufferSize - 1) {}
        Dim iBytesRead As Integer = 0

        Do
            iBytesRead = mInput.Read(iBuffer, 0, iBuffer.Length)
            mOutput.Write(iBuffer, 0, iBytesRead)
        Loop While iBytesRead > 0
    End Sub

    Public Class ClassKeyValueAnalysis
        Enum ENUM_STATE_TYPES
            BRACKET_LEVEL
            BRACE_LEVEL
            IN_SINGLE_COMMENT
            IN_MULTI_COMMENT
            IN_STRING
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
        Private g_mLineIndexes As New Dictionary(Of Integer, Integer)
        Private g_sEscapeChar As Char = "\"c

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
                            g_iStateArray(i, ENUM_STATE_TYPES.IN_STRING) > 0)
            End Get
        End Property

        Property m_EscapeChar As Char
            Get
                Return g_sEscapeChar
            End Get
            Set(value As Char)
                g_sEscapeChar = value
            End Set
        End Property

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

        Public Function GetChar(iIndex As Integer) As Char
            Return g_sCacheText(iIndex)
        End Function

        Public Function GetIndexFromLine(iLine As Integer) As Integer
            If (iLine = 0) Then
                Return 0
            End If

            For Each mItem In g_mLineIndexes
                If (mItem.Key + 1 >= iLine) Then
                    If (mItem.Value + 1 > g_sCacheText.Length - 1) Then
                        'Get the length even if its longer than the g_sCacheTest length.
                        'g_iStateArray should have +1 more length.
                        Return g_sCacheText.Length
                    Else
                        Return mItem.Value + 1
                    End If
                End If
            Next

            Return -1
        End Function

        Public Function IsEscaped(iIndex As Integer) As Boolean
            Dim iEscapes As Integer = 0
            For j = iIndex - 1 To 0 Step -1
                If (g_sCacheText(j) <> g_sEscapeChar) Then
                    Exit For
                End If

                iEscapes += 1
            Next

            If ((iEscapes Mod 2) = 0) Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Sub New(sText As String)
            g_sCacheText = sText
            g_iMaxLength = sText.Length

            'Init g_iStateArray with +1 index.
            g_iStateArray = New Integer(sText.Length, [Enum].GetNames(GetType(ENUM_STATE_TYPES)).Length - 1) {}

            Dim iBracketLevel As Integer = 0 '[] 
            Dim iBraceLevel As Integer = 0 '{} 
            Dim bInSingleComment As Integer = 0
            Dim bInMultiComment As Integer = 0
            Dim bInString As Integer = 0
            Dim bInChar As Integer = 0
            Dim bInPreprocessor As Integer = 0
            Dim iBracketLevelPreSave As Integer = 0 '[] 'Save/load before/after preprocessor
            Dim iBraceLevelPreSave As Integer = 0 '{} 'Save/load before/after preprocessor
            Dim bInSingleCommentPreSave As Integer = 0
            Dim bInMultiCommentPreSave As Integer = 0
            Dim bInStringPreSave As Integer = 0

            Dim iLine As Integer = 0

            For i = 0 To sText.Length - 1
                g_iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel
                g_iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel
                g_iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                g_iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = bInMultiComment
                g_iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = bInString
                g_iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = bInPreprocessor

                Select Case (sText(i))
                    Case "#"c
                        If (iBracketLevel > 0 OrElse bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        If (bInPreprocessor < 1) Then
                            'Preprocessor directives can be malformed, reset level after we are done with it. 
                            iBracketLevelPreSave = iBracketLevel
                            iBraceLevelPreSave = iBraceLevel
                            bInSingleCommentPreSave = bInSingleComment
                            bInMultiCommentPreSave = bInMultiComment
                            bInStringPreSave = bInString
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

                    Case "["c
                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        iBracketLevel += 1
                        g_iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel

                    Case "]"c
                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        g_iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel
                        iBracketLevel -= 1

                    Case "{"c
                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        iBraceLevel += 1
                        g_iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel

                    Case "}"c
                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        g_iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel
                        iBraceLevel -= 1

                    Case """"c
                        If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInChar > 0) Then
                            Continue For
                        End If

                        'ignore \"
                        Dim iEscapes As Integer = 0
                        For j = i - 1 To 0 Step -1
                            If (sText(j) <> g_sEscapeChar) Then
                                Exit For
                            End If

                            iEscapes += 1
                        Next

                        If ((iEscapes Mod 2) = 0) Then
                            bInString = If(bInString > 0, 0, 1)
                            g_iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = 1
                        End If

                    Case vbLf(0)
                        Dim bIsMulitLine As Boolean = False

                        'Do not reset pre-save while in multi-comment or multi-string.
                        If (bInMultiComment > 0) Then
                            bIsMulitLine = True
                        End If

                        If (bInString > 0) Then
                            bIsMulitLine = True
                        End If

                        If (bInSingleComment > 0) Then
                            bInSingleComment = 0
                            g_iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                        End If

                        If (bInPreprocessor > 0 AndAlso Not bIsMulitLine) Then
                            bInPreprocessor = 0
                            g_iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = bInPreprocessor

                            'Preprocessor directives can be malformed, reset level after we are done with it. 
                            iBracketLevel = iBracketLevelPreSave
                            iBraceLevel = iBraceLevelPreSave
                            bInSingleComment = bInSingleCommentPreSave
                            bInMultiComment = bInMultiCommentPreSave
                            bInString = bInStringPreSave
                        End If

                        g_mLineIndexes(iLine) = i

                        iLine += 1

                    Case Else
                        g_iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel
                        g_iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel
                        g_iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                        g_iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = bInMultiComment
                        g_iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = bInString
                        g_iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = bInPreprocessor
                End Select
            Next
        End Sub
    End Class

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                If (g_mStreamWriter IsNot Nothing) Then
                    g_mStreamWriter.Dispose()
                    g_mStreamWriter = Nothing
                End If

                If (g_mStreamReader IsNot Nothing) Then
                    g_mStreamReader.Dispose()
                    g_mStreamReader = Nothing
                End If

                If (g_mStream IsNot Nothing) Then
                    g_mStream.Dispose()
                    g_mStream = Nothing
                End If
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
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
