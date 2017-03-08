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

Public Class ClassAutocompleteUpdater
    Private g_mFormMain As FormMain
    Private g_mAutocompleteUpdaterThread As Threading.Thread

    Public g_bForceFullAutocompleteUpdate As Boolean = False
    Private _lock As New Object

    Public Event OnAutocompleteUpdateStarted(iUpdateType As ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS)
    Public Event OnAutocompleteUpdateEnd()
    Public Event OnAutocompleteUpdateAbort()

    Public Sub New(f As FormMain)
        g_mFormMain = f
    End Sub

    Enum ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS
        ALL = 0
        FULL_AUTOCOMPLETE = (1 << 0)
        VARIABLES_AUTOCOMPLETE = (1 << 1)
    End Enum

    ''' <summary>
    ''' Starts the autocomplete update thread
    ''' </summary>
    Public Function StartUpdate(iUpdateType As ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS) As Boolean
        If (g_mAutocompleteUpdaterThread Is Nothing OrElse Not g_mAutocompleteUpdaterThread.IsAlive) Then
            g_mAutocompleteUpdaterThread = New Threading.Thread(Sub()
                                                                    SyncLock _lock
                                                                        If (iUpdateType = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL OrElse
                                                                                    (iUpdateType And ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.FULL_AUTOCOMPLETE) = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.FULL_AUTOCOMPLETE) Then
                                                                            RaiseEvent OnAutocompleteUpdateStarted(iUpdateType)
                                                                            FullAutocompleteUpdate_Thread()
                                                                        End If

                                                                        If (iUpdateType = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL OrElse
                                                                                    (iUpdateType And ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.VARIABLES_AUTOCOMPLETE) = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.VARIABLES_AUTOCOMPLETE) Then
                                                                            RaiseEvent OnAutocompleteUpdateStarted(iUpdateType)
                                                                            VariableAutocompleteUpdate_Thread()
                                                                        End If
                                                                    End SyncLock

                                                                    RaiseEvent OnAutocompleteUpdateEnd()
                                                                End Sub)
            g_mAutocompleteUpdaterThread.Priority = Threading.ThreadPriority.Lowest
            g_mAutocompleteUpdaterThread.IsBackground = True
            g_mAutocompleteUpdaterThread.Start()

            If (iUpdateType = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL OrElse
                        (iUpdateType And ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.FULL_AUTOCOMPLETE) = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.FULL_AUTOCOMPLETE) Then
                g_bForceFullAutocompleteUpdate = False
            End If

            Return True
        Else
            If (Not g_bForceFullAutocompleteUpdate) Then
                g_mFormMain.PrintInformation("[INFO]", "Could not start autocomplete update thread, it's already running!", False, False, 15)
            End If

            If (iUpdateType = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL OrElse
                        (iUpdateType And ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.FULL_AUTOCOMPLETE) = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.FULL_AUTOCOMPLETE) Then
                g_bForceFullAutocompleteUpdate = True
            End If

            Return False
        End If
    End Function

    ''' <summary>
    ''' Stops the autocomplete update thread
    ''' </summary>
    Public Sub StopUpdate()
        RaiseEvent OnAutocompleteUpdateAbort

        If (g_mAutocompleteUpdaterThread IsNot Nothing AndAlso g_mAutocompleteUpdaterThread.IsAlive) Then
            'g_mFormMain.PrintInformation("[WARN]", "Autocomplete update canceled!")

            g_mAutocompleteUpdaterThread.Abort()
            g_mAutocompleteUpdaterThread.Join()
            g_mAutocompleteUpdaterThread = Nothing
        End If
    End Sub

    Private Sub FullAutocompleteUpdate_Thread()
        Try
            'g_mFormMain.PrintInformation("[INFO]", "Autocomplete update started...")

            Dim sActiveSourceFile As String = CStr(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File))
            Dim sActiveSource As String = CStr(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent))

            If (String.IsNullOrEmpty(sActiveSourceFile) OrElse Not IO.File.Exists(sActiveSourceFile)) Then
                g_mFormMain.PrintInformation("[ERRO]", "Autocomplete update failed! Could not get current source file!", False, False, 1)
                Return
            End If

            Dim lTmpAutocompleteList As New ClassSyncList(Of STRUC_AUTOCOMPLETE)

            'Add debugger placeholder variables and methods
            lTmpAutocompleteList.AddRange((New ClassDebuggerParser(g_mFormMain)).GetDebuggerAutocomplete)


            'Parse everything. Methods etc.
            If (True) Then
                Dim sSourceList As New ClassSyncList(Of String())
                Dim sFiles As String() = GetIncludeFiles(sActiveSource, sActiveSourceFile, sActiveSourceFile)

                For i = 0 To sFiles.Length - 1
                    ParseAutocomplete_Pre(sActiveSource, sActiveSourceFile, sFiles(i), sSourceList, lTmpAutocompleteList)
                Next

                Dim sRegExEnum As String = String.Format("(\b{0}\b)", String.Join("\b|\b", GetEnumNames(lTmpAutocompleteList)))
                For i = 0 To sSourceList.Count - 1
                    ParseAutocomplete_Post(sActiveSource, sActiveSourceFile, sSourceList(i)(0), sRegExEnum, sSourceList(i)(1), lTmpAutocompleteList)
                Next
            End If

            'Parse Methodmaps
            If (True) Then
                ParseAutocompleteMethodmap(lTmpAutocompleteList)
            End If

            'Save everything and update syntax
            g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.DoSync(
                Sub()
                    g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.RemoveAll(Function(j As STRUC_AUTOCOMPLETE) (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                    g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.AddRange(lTmpAutocompleteList)
                End Sub)

            g_mFormMain.BeginInvoke(Sub()
                                        'Dont move this outside of invoke! Results in "File is already in use!" when aborting the thread... for some reason...
                                        g_mFormMain.g_ClassSyntaxTools.UpdateSyntaxFile(ClassSyntaxTools.ENUM_SYNTAX_UPDATE_TYPE.AUTOCOMPLETE)
                                        g_mFormMain.g_ClassSyntaxTools.UpdateTextEditorSyntax()
                                        g_mFormMain.g_mUCObjectBrowser.StartUpdate()
                                    End Sub)

            'g_mFormMain.PrintInformation("[INFO]", "Autocomplete update finished!")
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            g_mFormMain.PrintInformation("[ERRO]", "Autocomplete update failed! " & ex.Message, False, False, 1)
            ClassExceptionLog.WriteToLog(ex)
        End Try
    End Sub

    Private Sub ParseAutocompleteMethodmap(lTmpAutoList As ClassSyncList(Of STRUC_AUTOCOMPLETE))
        Dim lAlreadyDidList As New List(Of String)

        Dim lTmpAutoAddList As New List(Of STRUC_AUTOCOMPLETE)

        For i = lTmpAutoList.Count - 1 To 0 Step -1
            If ((lTmpAutoList(i).mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP OrElse Not lTmpAutoList(i).sFullFunctionName.Contains("<"c) OrElse Not lTmpAutoList(i).sFunctionName.Contains("."c)) Then
                Continue For
            End If

            Dim mMatch As Match = Regex.Match(lTmpAutoList(i).sFullFunctionName, "(?<Name>\b[a-zA-Z0-9_]+\b)\s+\<\s+(?<Parent>\b[a-zA-Z0-9_]+\b)")
            If (Not mMatch.Success) Then
                Continue For
            End If

            Dim sName As String = mMatch.Groups("Name").Value
            Dim sParent As String = mMatch.Groups("Parent").Value

            If (lAlreadyDidList.Contains(sName)) Then
                Continue For
            End If

            lAlreadyDidList.Add(sName)

            Dim iOldNextParent As String = sParent

            While Not String.IsNullOrEmpty(iOldNextParent)
                Dim sNextParent As String = ""

                For ii = 0 To lTmpAutoList.Count - 1
                    If ((lTmpAutoList(ii).mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP OrElse Not lTmpAutoList(ii).sFunctionName.Contains("."c)) Then
                        Continue For
                    End If

                    Dim mParentMatch As Match = Regex.Match(lTmpAutoList(ii).sFullFunctionName, "(\b[a-zA-Z0-9_]+\b)\s+\<\s+(?<Parent>\b[a-zA-Z0-9_]+\b)")
                    Dim mParentMatch2 As Match = Regex.Match(lTmpAutoList(ii).sFunctionName, "(?<Name>^\b[a-zA-Z0-9_]+\b)\.")


                    Dim sParentName As String = mParentMatch2.Groups("Name").Value
                    Dim sParentParent As String = mParentMatch.Groups("Parent").Value

                    If (String.IsNullOrEmpty(sParentParent) OrElse iOldNextParent <> sParentName) Then
                        Continue For
                    End If

                    sNextParent = sParentParent

                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = lTmpAutoList(ii).sFile
                    struc.sFullFunctionName = lTmpAutoList(ii).sFullFunctionName
                    struc.sFunctionName = Regex.Replace(lTmpAutoList(ii).sFunctionName, "^\b[a-zA-Z0-9_]+\b\.", String.Format("{0}.", sName))
                    struc.sInfo = lTmpAutoList(ii).sInfo
                    struc.mType = lTmpAutoList(ii).mType

                    lTmpAutoAddList.Add(struc)
                Next

                iOldNextParent = sNextParent
            End While
        Next

        lTmpAutoList.AddRange(lTmpAutoAddList.ToArray)
    End Sub

    Private Function GetEnumNames(lTmpAutoList As ClassSyncList(Of STRUC_AUTOCOMPLETE)) As String()
        Dim lList As New List(Of String)

        'For >1.7
        lList.Add("void")
        lList.Add("int")
        lList.Add("bool")
        lList.Add("float")
        lList.Add("char")
        lList.Add("function")
        lList.Add("object")
        lList.Add("null_t")
        lList.Add("nullfunc_t")
        lList.Add("__nullable__")

        'For <1.6
        lList.Add("_")
        lList.Add("any")
        lList.Add("bool")
        lList.Add("Float")
        lList.Add("String")
        lList.Add("Function")

        'Special
        'lList.Add("EOS")
        'lList.Add("INVALID_FUNCTION")
        'lList.Add("cellbits")
        'lList.Add("cellmax")
        'lList.Add("cellmin")
        'lList.Add("charbits")
        'lList.Add("charmin")
        'lList.Add("charmax")
        'lList.Add("ucharmax")
        'lList.Add("__Pawn")
        'lList.Add("debug")

        lTmpAutoList.ForEach(Sub(j As STRUC_AUTOCOMPLETE)
                                 If ((j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) Then
                                     Return
                                 End If

                                 If (j.sFunctionName.Contains("."c)) Then
                                     Return
                                 End If

                                 If (lList.Contains(j.sFunctionName)) Then
                                     Return
                                 End If

                                 lList.Add(j.sFunctionName)
                             End Sub)

        Return lList.ToArray
    End Function

    Public Sub CleanUpNewLinesSource(ByRef sSource As String)
        'Remove new lines e.g: MyStuff \
        '                      MyStuff2
        sSource = Regex.Replace(sSource, "\\\s*\n\s*", "")

        If (True) Then
            'From:
            '   public
            '       static
            '           bla()
            'To:
            '   public static bla()
            Dim sTypes As String() = {"enum", "funcenum", "functag", "stock", "static", "const", "public", "native", "forward", "typeset", "methodmap", "typedef"}
            Dim mRegMatchCol As MatchCollection = Regex.Matches(sSource, String.Format("^\s*(\b{0}\b)(?<Space>\s*\n\s*)", String.Join("\b|\b", sTypes)), RegexOptions.Multiline)
            For i = mRegMatchCol.Count - 1 To 0 Step -1
                Dim mRegMatch As Match = mRegMatchCol(i)
                If (Not mRegMatch.Groups("Space").Success) Then
                    Continue For
                End If
                sSource = sSource.Remove(mRegMatch.Groups("Space").Index, mRegMatch.Groups("Space").Length)
                sSource = sSource.Insert(mRegMatch.Groups("Space").Index, " "c)
            Next
        End If

        If (True) Then
            Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

            'Filter new lines in statements with parenthesis e.g: MyStuff(MyArg1,
            '                                                               MyArg2)
            For i = 0 To sSource.Length - 1

                Select Case (sSource(i))
                    Case vbLf(0)
                        If (sourceAnalysis.InNonCode(i)) Then
                            Exit Select
                        End If

                        If (sourceAnalysis.GetParenthesisLevel(i) > 0) Then
                            Select Case (True)
                                Case i > 1 AndAlso sSource(i - 1) = vbCr
                                    sSource = sSource.Remove(i - 1, 2)
                                    sSource = sSource.Insert(i - 1, New String(" "c, 2))
                                Case Else
                                    sSource = sSource.Remove(i, 1)
                                    sSource = sSource.Insert(i, New String(" "c, 1))
                            End Select
                        End If

                    Case Else
                        'If we collapse statements etc., we need to remove single line comments!
                        'if(
                        '       //This my cause problems!
                        '   )
                        'Results in:
                        'if(//This my cause problems!)
                        'Just remove the comments
                        If (sourceAnalysis.GetParenthesisLevel(i) > 0 AndAlso sourceAnalysis.InSingleComment(i)) Then
                            sSource = sSource.Remove(i, 1)
                            sSource = sSource.Insert(i, " "c)
                        End If

                End Select
            Next
        End If
    End Sub

    Private Sub ParseAutocomplete_Pre(sActiveSource As String, sActiveSourceFile As String, sFile As String, ByRef sSourceList As ClassSyncList(Of String()), ByRef lTmpAutocompleteList As ClassSyncList(Of STRUC_AUTOCOMPLETE))
        Dim sSource As String
        If (sActiveSourceFile.ToLower = sFile.ToLower) Then
            sSource = sActiveSource
        Else
            sSource = IO.File.ReadAllText(sFile)
        End If

        CleanUpNewLinesSource(sSource)

        Dim lStringLocList As New List(Of Integer())
        Dim lBraceLocList As New List(Of Integer())

        If (True) Then
            'Filter new spaces 
            Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

            Dim iBraceLevel As Integer = 0
            Dim iStringStart As Integer = 0
            Dim iBraceStart As Integer = 0
            Dim bInString As Boolean = False
            For i = 0 To sSource.Length - 1
                Select Case (sSource(i))
                    Case "("c
                        If (sourceAnalysis.InNonCode(i)) Then
                            Continue For
                        End If

                        If (iBraceLevel = 0) Then
                            iBraceStart = i
                        End If
                        iBraceLevel += 1
                    Case ")"c
                        If (sourceAnalysis.InNonCode(i)) Then
                            Continue For
                        End If

                        iBraceLevel -= 1
                        If (iBraceLevel = 0) Then
                            Dim iBraceLen = i - iBraceStart + 1
                            lBraceLocList.Add(New Integer() {iBraceStart, iBraceLen})
                        End If
                    Case """"c
                        If (i > 1 AndAlso sSource(i - 1) <> "\"c) Then
                            If (Not bInString AndAlso sSource(i - 1) = "'"c) Then
                                Continue For
                            End If

                            Select Case (bInString)
                                Case True
                                    Dim iStringLen = i - iStringStart + 1
                                    lStringLocList.Add(New Integer() {iStringStart, iStringLen})
                                Case Else
                                    iStringStart = i
                            End Select

                            bInString = Not bInString
                        End If
                End Select
            Next
        End If

        'Fix function spaces
        If (True) Then
            For i = lBraceLocList.Count - 1 To 0 Step -1
                Dim iBraceStart As Integer = lBraceLocList(i)(0)
                Dim iBraceLen As Integer = lBraceLocList(i)(1)

                Dim sBraceString As String = sSource.Substring(iBraceStart, iBraceLen)
                Dim mRegMatchCol As MatchCollection = Regex.Matches(sBraceString, "\s+")

                For ii = mRegMatchCol.Count - 1 To 0 Step -1
                    Dim mRegMatch As Match = mRegMatchCol(ii)
                    If (Not mRegMatch.Success) Then
                        Continue For
                    End If

                    Dim iOffset As Integer = iBraceStart + mRegMatch.Index
                    Dim bContinue As Boolean = False
                    For iii = 0 To lStringLocList.Count - 1
                        If (iOffset > lStringLocList(iii)(0) AndAlso iOffset < (lStringLocList(iii)(0) + lStringLocList(iii)(1))) Then
                            bContinue = True
                            Exit For
                        End If
                    Next

                    If (bContinue) Then
                        Continue For
                    End If

                    sBraceString = sBraceString.Remove(mRegMatch.Index, mRegMatch.Length)
                    sBraceString = sBraceString.Insert(mRegMatch.Index, " "c)
                Next

                sSource = sSource.Remove(iBraceStart, iBraceLen)
                sSource = sSource.Insert(iBraceStart, sBraceString)
            Next
        End If


        'Get methodmap enums
        If ((ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("methodmap")) Then
            Dim SB_Source As New StringBuilder(sSource.Length)

            If (True) Then
                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                For i = 0 To sSource.Length - 1
                    SB_Source.Append(If(sourceAnalysis.InNonCode(i), " "c, sSource(i)))
                Next
            End If

            Dim mPossibleEnumMatches As MatchCollection = Regex.Matches(SB_Source.ToString, "^\s*\b(methodmap)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)", RegexOptions.Multiline) '^\s*\b(enum)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)

            Dim mRegMatch As Match

            For i = 0 To mPossibleEnumMatches.Count - 1
                mRegMatch = mPossibleEnumMatches(i)

                If (Not mRegMatch.Success) Then
                    Continue For
                End If

                Dim sEnumName As String = mRegMatch.Groups("Name").Value

                Dim struc As New STRUC_AUTOCOMPLETE
                struc.sFile = IO.Path.GetFileName(sFile)
                struc.sFullFunctionName = "enum " & sEnumName
                struc.sFunctionName = sEnumName
                struc.sInfo = ""
                struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM

                If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                    lTmpAutocompleteList.Add(struc)
                End If
            Next
        End If


        'Get typeset enums
        If ((ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("typeset")) Then
            Dim SB_Source As New StringBuilder(sSource.Length)

            If (True) Then
                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                For i = 0 To sSource.Length - 1
                    SB_Source.Append(If(sourceAnalysis.InNonCode(i), " "c, sSource(i)))
                Next
            End If

            Dim mPossibleEnumMatches As MatchCollection = Regex.Matches(SB_Source.ToString, "^\s*\b(typeset)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)", RegexOptions.Multiline) '^\s*\b(enum)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)

            Dim mRegMatch As Match

            For i = 0 To mPossibleEnumMatches.Count - 1
                mRegMatch = mPossibleEnumMatches(i)

                If (Not mRegMatch.Success) Then
                    Continue For
                End If

                Dim sEnumName As String = mRegMatch.Groups("Name").Value

                Dim struc As New STRUC_AUTOCOMPLETE
                struc.sFile = IO.Path.GetFileName(sFile)
                struc.sFullFunctionName = "enum " & sEnumName
                struc.sFunctionName = sEnumName
                struc.sInfo = ""
                struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM

                If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                    lTmpAutocompleteList.Add(struc)
                End If
            Next
        End If

        'Get typedef enums
        'If ((SettingsClass.g_CurrentAutocompleteSyntax = SettingsClass.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7 OrElse SettingsClass.g_CurrentAutocompleteSyntax = SettingsClass.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("typedef")) Then
        '    Dim SB_Source As New StringBuilder(sSource.Length)

        '    If (True) Then
        '        Dim iSynCR_Source As New ClassSyntaxTools.ClassSyntaxCharReader(sSource)

        '        For i = 0 To sSource.Length - 1
        '            SB_Source.Append(If(iSynCR_Source.InNonCode(i), " "c, sSource(i)))
        '        Next
        '    End If

        '    Dim mPossibleEnumMatches As MatchCollection = Regex.Matches(SB_Source.ToString, "^\s*\b(typedef)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)", RegexOptions.Multiline) '^\s*\b(enum)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)

        '    Dim mRegMatch As Match

        '    For i = 0 To mPossibleEnumMatches.Count - 1
        '        mRegMatch = mPossibleEnumMatches(i)

        '        If (Not mRegMatch.Success) Then
        '            Continue For
        '        End If

        '        Dim sEnumName As String = mRegMatch.Groups("Name").Value

        '        Dim struc As STRUC_AUTOCOMPLETE
        '        struc.sFile = IO.Path.GetFileName(sFile)
        '        struc.sFullFunctionname = "enum " & sEnumName
        '        struc.sFunctionName = sEnumName
        '        struc.sInfo = ""
        '        struc.sType = "enum"

        '        If (Not lTmpAutoList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.sType = struc.sType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
        '            lTmpAutoList.Add(struc)
        '        End If
        '    Next
        'End If

        'Get enums
        If (sSource.Contains("enum")) Then
            Dim SB_Source As New StringBuilder(sSource.Length)

            If (True) Then
                Dim sourceAnalysis2 As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                For i = 0 To sSource.Length - 1
                    SB_Source.Append(If(sourceAnalysis2.InNonCode(i), " "c, sSource(i)))
                Next
            End If

            Dim mPossibleEnumMatches As MatchCollection = Regex.Matches(SB_Source.ToString, "^\s*\b(enum)\b\s+((?<Name>\b[a-zA-Z0-9_]+\b)|\(.*?\)|)(\:){0,1}\s*(?<BraceStart>\{)", RegexOptions.Multiline) '^\s*\b(enum)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)
            Dim iBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(SB_Source.ToString, "{"c, "}"c, 1, True)


            Dim mRegMatch As Match

            Dim sEnumName As String

            Dim bIsValid As Boolean
            Dim sEnumSource As String
            Dim iBraceIndex As Integer

            Dim sourceAnalysis As ClassSyntaxTools.ClassSyntaxSourceAnalysis

            Dim SB As StringBuilder
            Dim lEnumSplitList As List(Of String)

            Dim sLine As String

            Dim iTargetEnumSplitListIndex As Integer

            Dim regMatch As Match

            For i = 0 To mPossibleEnumMatches.Count - 1
                mRegMatch = mPossibleEnumMatches(i)
                If (Not mRegMatch.Success) Then
                    Continue For
                End If

                sEnumName = mRegMatch.Groups("Name").Value
                If (String.IsNullOrEmpty(sEnumName.Trim)) Then
                    g_mFormMain.PrintInformation("[WARN]", String.Format("Failed to read name from enum because it has no name: Renamed to 'Enum' ({0})", IO.Path.GetFileName(sFile)), False, False, 15)
                    sEnumName = "Enum"
                End If

                bIsValid = False
                sEnumSource = ""
                iBraceIndex = mRegMatch.Groups("BraceStart").Index
                For ii = 0 To iBraceList.Length - 1
                    If (iBraceIndex = iBraceList(ii)(0)) Then
                        sEnumSource = SB_Source.ToString.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                        bIsValid = True
                        Exit For
                    End If
                Next
                If (Not bIsValid) Then
                    Continue For
                End If

                sourceAnalysis = New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sEnumSource)

                SB = New StringBuilder
                lEnumSplitList = New List(Of String)

                For ii = 0 To sEnumSource.Length - 1
                    Select Case (sEnumSource(ii))
                        Case ","c
                            If (sourceAnalysis.GetParenthesisLevel(ii) > 0 OrElse sourceAnalysis.GetBracketLevel(ii) > 0 OrElse sourceAnalysis.GetBraceLevel(ii) > 0 OrElse sourceAnalysis.InNonCode(ii)) Then
                                Exit Select
                            End If

                            If (SB.Length < 1) Then
                                Exit Select
                            End If

                            sLine = SB.ToString
                            sLine = Regex.Replace(sLine.Trim, "\s+", " ")

                            lEnumSplitList.Add(sLine)
                            SB.Length = 0
                        Case Else
                            If (sourceAnalysis.InNonCode(ii)) Then
                                Exit Select
                            End If

                            SB.Append(sEnumSource(ii))
                    End Select
                Next

                If (SB.Length > 0) Then
                    sLine = SB.ToString
                    sLine = Regex.Replace(sLine.Trim, "\s+", " ")

                    If (Not String.IsNullOrEmpty(sLine)) Then
                        lEnumSplitList.Add(sLine)
                        SB.Length = 0
                    End If
                End If


                Dim lEnumCommentArray As String() = New String(lEnumSplitList.Count - 1) {}

                Dim sEnumSourceLines As String() = sEnumSource.Split(New String() {vbNewLine, vbLf}, 0)
                For ii = 0 To sEnumSourceLines.Length - 1
                    iTargetEnumSplitListIndex = -1
                    For iii = 0 To lEnumSplitList.Count - 1
                        If (sEnumSourceLines(ii).Contains(lEnumSplitList(iii)) AndAlso Regex.IsMatch(sEnumSourceLines(ii), String.Format("\b{0}\b", Regex.Escape(lEnumSplitList(iii))))) Then
                            iTargetEnumSplitListIndex = iii
                            Exit For
                        End If
                    Next
                    If (iTargetEnumSplitListIndex < 0) Then
                        Continue For
                    End If

                    While True
                        regMatch = Regex.Match(sEnumSourceLines(ii), "/\*(.*?)\*/\s*$")
                        If (regMatch.Success) Then
                            lEnumCommentArray(iTargetEnumSplitListIndex) = regMatch.Value
                            Exit While
                        End If
                        If (ii > 1) Then
                            regMatch = Regex.Match(sEnumSourceLines(ii - 1), "^\s*(?<Comment>//(.*?)$)")
                            If (regMatch.Success) Then
                                lEnumCommentArray(iTargetEnumSplitListIndex) = regMatch.Groups("Comment").Value
                                Exit While
                            End If
                        End If

                        lEnumCommentArray(iTargetEnumSplitListIndex) = ""
                        Exit While
                    End While
                Next

                If (True) Then
                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = IO.Path.GetFileName(sFile)
                    struc.sFullFunctionName = "enum " & sEnumName
                    struc.sFunctionName = sEnumName
                    struc.sInfo = ""
                    struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM

                    If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                        lTmpAutocompleteList.Add(struc)
                    End If
                End If

                For ii = 0 To lEnumSplitList.Count - 1
                    Dim sEnumFull As String = lEnumSplitList(ii)
                    Dim sEnumComment As String = lEnumCommentArray(ii)

                    regMatch = Regex.Match(sEnumFull, "^\s*(?<Tag>\b[a-zA-Z0-9_]+\b:)*(?<Name>\b[a-zA-Z0-9_]+\b)")
                    If (Not regMatch.Groups("Name").Success) Then
                        g_mFormMain.PrintInformation("[WARN]", String.Format("Failed to resolve type 'Enum': enum {0} {1}", sEnumName, sEnumFull), False, False, 15)
                        Continue For
                    End If

                    Dim sEnumVarName As String = regMatch.Groups("Name").Value

                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = IO.Path.GetFileName(sFile)
                    struc.sFullFunctionName = String.Format("enum {0} {1}", sEnumName, sEnumFull)
                    struc.sFunctionName = String.Format("{0}.{1}", sEnumName, sEnumVarName)
                    struc.sInfo = sEnumComment
                    struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM

                    If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                        lTmpAutocompleteList.Add(struc)
                    End If
                Next
            Next
        End If

        sSourceList.Add(New String() {sFile, sSource})
    End Sub

    Private Sub ParseAutocomplete_Post(sActiveSource As String, sActiveSourceFile As String, ByRef sFile As String, ByRef sRegExEnum As String, ByRef sSource As String, ByRef lTmpAutocompleteList As ClassSyncList(Of STRUC_AUTOCOMPLETE))
        'Get Defines
        If (sSource.Contains("#define")) Then
            Dim sLine As String

            Dim mRegMatch As Match

            Dim sFullName As String
            Dim sName As String

            Dim iBraceList As Integer()()

            Dim sBraceText As String
            Dim sFullDefine As String

            Using SR As New IO.StringReader(sSource)
                While True
                    sLine = SR.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    If (Not sLine.Contains("#define")) Then
                        Continue While
                    End If

                    mRegMatch = Regex.Match(sLine, "^\s*#define\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<Arguments>\()*")
                    If (Not mRegMatch.Success) Then
                        Continue While
                    End If

                    sFullName = ""
                    sName = mRegMatch.Groups("Name").Value

                    If (mRegMatch.Groups("Arguments").Success) Then
                        iBraceList = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sLine, "("c, ")"c, 1, True)
                        If (iBraceList.Length < 1) Then
                            Continue While
                        End If

                        sBraceText = sLine.Substring(iBraceList(0)(0) + 1, iBraceList(0)(1) - iBraceList(0)(0))

                        sFullDefine = Regex.Match(sLine, String.Format("{0}{1}(.*?)$", Regex.Escape(mRegMatch.Value), Regex.Escape(sBraceText))).Value
                        sFullName = sFullDefine
                    Else
                        sFullDefine = Regex.Match(sLine, String.Format("{0}(.*?)$", Regex.Escape(mRegMatch.Value))).Value
                        sFullName = sFullDefine
                    End If

                    sFullName = sFullName.Replace(vbTab, " ")

                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = IO.Path.GetFileName(sFile)
                    struc.sFullFunctionName = sFullName
                    struc.sFunctionName = sName
                    struc.sInfo = ""
                    struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE

                    If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                        lTmpAutocompleteList.Add(struc)
                    End If
                End While
            End Using
        End If

        'Get public global variables
        If (sSource.Contains("public")) Then
            'Dim SB_Source As New StringBuilder(sSource.Length)

            'If (True) Then
            '    Dim iSynCR_Source As New ClassSyntaxTools.ClassSyntaxCharReader(sSource)

            '    For i = 0 To sSource.Length - 1
            '        SB_Source.Append(If(iSynCR_Source.InNonCode(i), " "c, sSource(i)))
            '    Next
            'End If


            Dim mRegMatch As Match

            Dim sFullName As String
            Dim sName As String
            Dim sComment As String

            Dim sLines As String() = sSource.Split(New String() {vbNewLine, vbLf}, 0)
            Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)
            For i = 0 To sLines.Length - 1
                If (Not sLines(i).Contains("public") OrElse Not sLines(i).Contains(";"c)) Then
                    Continue For
                End If

                Dim iIndex = sourceAnalysis.GetIndexFromLine(i)
                If (iIndex < 0 OrElse sourceAnalysis.GetBraceLevel(iIndex) > 0 OrElse sourceAnalysis.InNonCode(iIndex)) Then
                    Continue For
                End If

                'SP 1.7 +Tags
                mRegMatch = Regex.Match(sLines(i), String.Format("^\s*(?<Types>public(\b[a-zA-Z0-9_ ]+\b)*)\s+(?<Tag>\b{0}\b(\[\s*\])*\s)\s*(?<Name>\b[a-zA-Z0-9_]+\b)(?<Other>(.*?))$", sRegExEnum))
                If (Not mRegMatch.Success) Then
                    If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7) Then
                        Continue For
                    End If

                    'SP 1.6 +Tags
                    mRegMatch = Regex.Match(sLines(i), String.Format("^\s*(?<Types>public(\b[a-zA-Z0-9_ ]+\b)*)\s+(?<Tag>\b{0}\b\:)(?<Name>\b[a-zA-Z0-9_]+\b)(?<Other>(.*?))$", sRegExEnum))
                    If (Not mRegMatch.Success) Then
                        'SP 1.6 -Tags
                        mRegMatch = Regex.Match(sLines(i), String.Format("^\s*(?<Types>public(\b[a-zA-Z0-9_ ]+\b)*)\s+(?<!<Tag>\b{0}\b\:)(?<Name>\b[a-zA-Z0-9_]+\b)(?<Other>(.*?))$", sRegExEnum))
                        If (Not mRegMatch.Success) Then
                            Continue For
                        End If
                    End If
                Else
                    If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6) Then
                        Continue For
                    End If
                End If

                sFullName = ""
                sName = mRegMatch.Groups("Name").Value
                sComment = Regex.Match(mRegMatch.Groups("Other").Value, "/\*(.*?)\*/\s*$").Value

                sFullName = String.Format("{0} {1}{2}{3}", mRegMatch.Groups("Types").Value, mRegMatch.Groups("Tag").Value, mRegMatch.Groups("Name").Value, mRegMatch.Groups("Other").Value)
                sFullName = sFullName.Replace(vbTab, " "c)
                If (Not String.IsNullOrEmpty(sComment)) Then
                    sFullName = sFullName.Replace(sComment, "")
                End If

                Dim struc As New STRUC_AUTOCOMPLETE
                struc.sFile = IO.Path.GetFileName(sFile)
                struc.sFullFunctionName = sFullName
                struc.sFunctionName = sName
                struc.sInfo = sComment
                struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR

                If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                    lTmpAutocompleteList.Add(struc)
                End If
            Next
        End If

        'Get Functions
        If (True) Then
            Dim iBraceList As Integer()()
            Dim sBraceText As String
            Dim mRegMatch As Match

            Dim sTypes As String()
            Dim sTag As String
            Dim sName As String
            Dim sFull As String
            Dim sComment As String

            Dim mFuncTagMatch As Match

            Dim bCommentStart As Boolean
            Dim mRegMatch2 As Match

            Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)
            Dim sLines As String() = sSource.Split(New String() {vbNewLine, vbLf}, 0)

            If (sourceAnalysis.GetMaxLenght - 1 > 0) Then
                Dim iLastBraceLevel As Integer = sourceAnalysis.GetBraceLevel(sourceAnalysis.GetMaxLenght - 1)
                If (iLastBraceLevel > 0) Then
                    g_mFormMain.PrintInformation("[ERRO]", String.Format("Uneven brace level! May lead to syntax parser failures! [LV:{0}] ({1})", iLastBraceLevel, IO.Path.GetFileName(sFile)), False, False, 1)
                End If
            End If

            For i = 0 To sLines.Length - 1
                If ((ClassTools.ClassStrings.WordCount(sLines(i), "("c) + ClassTools.ClassStrings.WordCount(sLines(i), ")"c)) Mod 2 <> 0) Then
                    Continue For
                End If

                Dim iIndex = sourceAnalysis.GetIndexFromLine(i)
                If (iIndex < 0 OrElse sourceAnalysis.GetBraceLevel(iIndex) > 0 OrElse sourceAnalysis.InNonCode(iIndex)) Then
                    Continue For
                End If

                iBraceList = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sLines(i), "("c, ")"c, 1, True)
                If (iBraceList.Length < 1) Then
                    Continue For
                End If

                sBraceText = sLines(i).Substring(iBraceList(0)(0), iBraceList(0)(1) - iBraceList(0)(0) + 1)

                'SP 1.7 +Tags
                mRegMatch = Regex.Match(sLines(i), String.Format("^\s*(?<Types>[a-zA-Z0-9_ ]*)(?<Tag>\b{0}\b\s)\s*(?<Name>\b[a-zA-Z0-9_]+\b)\s*{1}(?<IsFunc>\s*;){2}", sRegExEnum, Regex.Escape(sBraceText), "{0,1}"))
                If (Not mRegMatch.Success) Then
                    If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7) Then
                        Continue For
                    End If

                    'SP 1.6 +Tags
                    mRegMatch = Regex.Match(sLines(i), String.Format("^\s*(?<Types>[a-zA-Z0-9_ ]*)(?<Tag>\b{0}\b\:)(?<Name>\b[a-zA-Z0-9_]+\b)\s*{1}(?<IsFunc>\s*;){2}", sRegExEnum, Regex.Escape(sBraceText), "{0,1}"))
                    If (Not mRegMatch.Success) Then
                        'SP 1.6 -Tags
                        mRegMatch = Regex.Match(sLines(i), String.Format("^\s*(?<Types>[a-zA-Z0-9_ ]*)(?<!<Tag>\b{0}\b\:)(?<Name>\b[a-zA-Z0-9_]+\b)\s*{1}(?<IsFunc>\s*;){2}", sRegExEnum, Regex.Escape(sBraceText), "{0,1}"))
                        If (Not mRegMatch.Success) Then
                            Continue For
                        End If
                    End If
                Else
                    If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6) Then
                        Continue For
                    End If
                End If

                sTypes = mRegMatch.Groups("Types").Value.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                sTag = mRegMatch.Groups("Tag").Value
                sName = mRegMatch.Groups("Name").Value
                sFull = mRegMatch.Groups("Types").Value & sTag & sName & sBraceText
                sComment = ""

                If (Regex.IsMatch(sName, String.Format("(\b{0}\b)", String.Join("\b|\b", g_mFormMain.g_ClassSyntaxTools.g_sStatementsArray)))) Then
                    Continue For
                End If

                If (sTypes.Length < 1 AndAlso mRegMatch.Groups("IsFunc").Success) Then
                    Continue For
                End If

                If (Array.IndexOf(sTypes, "functag") > -1) Then
                    While True
                        mFuncTagMatch = Regex.Match(sFull, "(?<Name>\b[a-zA-Z0-9_]+\b)\s+(?<Tag>\b[a-zA-Z0-9_]+\:\b)*\b(public)\b\s*\(")
                        If (mFuncTagMatch.Success) Then
                            sTypes = New String() {"functag"}
                            sTag = mFuncTagMatch.Groups("Tag").Value
                            sName = mFuncTagMatch.Groups("Name").Value
                            sFull = "functag " & sTag & sName & sBraceText
                            Exit While
                        End If
                        mFuncTagMatch = Regex.Match(sFull, "\b(public)\b\s+(?<Tag>\b[a-zA-Z0-9_]+\:\b)*(?<Name>\b[a-zA-Z0-9_]+\b)\s*\(")
                        If (mFuncTagMatch.Success) Then
                            sTypes = New String() {"functag"}
                            sTag = mFuncTagMatch.Groups("Tag").Value
                            sName = mFuncTagMatch.Groups("Name").Value
                            sFull = "functag " & sTag & sName & sBraceText
                            Exit While
                        End If

                        Exit While
                    End While
                End If

                bCommentStart = False
                For ii = i - 1 To 0 Step -1
                    mRegMatch2 = Regex.Match(sLines(ii), "(?<Start>(/\*+))|(?<End>\*+/|(?<Pragma>)^\s*#pragma)")

                    If (Not bCommentStart AndAlso Not mRegMatch2.Groups("End").Success) Then
                        Exit For
                    End If

                    If (Not mRegMatch2.Groups("Pragma").Success) Then
                        bCommentStart = True
                    End If

                    Select Case (True)
                        Case mRegMatch2.Groups("End").Success AndAlso Not mRegMatch2.Groups("Pragma").Success
                            sComment = sLines(ii).Substring(0, mRegMatch2.Groups("End").Index + 2) & Environment.NewLine & sComment
                        Case mRegMatch2.Groups("Start").Success
                            sComment = sLines(ii).Substring(mRegMatch2.Groups("Start").Index, sLines(ii).Length - mRegMatch2.Groups("Start").Index) & Environment.NewLine & sComment
                        Case Else
                            sComment = sLines(ii) & Environment.NewLine & sComment
                    End Select

                    If (bCommentStart AndAlso mRegMatch2.Groups("Start").Success) Then
                        Exit For
                    End If
                Next

                sComment = New Regex("^\s+", RegexOptions.Multiline).Replace(sComment, "")

                Dim struc As New STRUC_AUTOCOMPLETE
                struc.sFile = IO.Path.GetFileName(sFile)
                struc.sFullFunctionName = sFull
                struc.sFunctionName = sName
                struc.sInfo = sComment
                struc.mType = struc.ParseTypeNames(sTypes)

                If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                    lTmpAutocompleteList.Add(struc)
                End If
            Next
        End If

        'Get funcenums
        If ((ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6 OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("funcenum")) Then
            Dim mPossibleEnumMatches As MatchCollection = Regex.Matches(sSource, "^\s*\b(funcenum)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)", RegexOptions.Multiline)
            Dim iBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sSource, "{"c, "}"c, 1, True)

            Dim mRegMatch As Match

            Dim sEnumName As String

            Dim bIsValid As Boolean
            Dim sEnumSource As String
            Dim iBraceIndex As Integer

            Dim SB As StringBuilder
            Dim lEnumSplitList As List(Of String)

            Dim sourceAnalysis As ClassSyntaxTools.ClassSyntaxSourceAnalysis

            Dim sLine As String
            Dim iInvalidLen As Integer

            Dim sComment As String
            Dim lEnumCommentArray As String()
            Dim sEnumSourceLines As String()

            Dim iLine As Integer
            Dim bCommentStart As Boolean
            Dim mRegMatch2 As Match

            For i = 0 To mPossibleEnumMatches.Count - 1
                mRegMatch = mPossibleEnumMatches(i)
                If (Not mRegMatch.Success) Then
                    Continue For
                End If

                sEnumName = mRegMatch.Groups("Name").Value

                bIsValid = False
                sEnumSource = ""
                iBraceIndex = mRegMatch.Groups("BraceStart").Index
                For ii = 0 To iBraceList.Length - 1
                    If (iBraceIndex = iBraceList(ii)(0)) Then
                        sEnumSource = sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                        bIsValid = True
                        Exit For
                    End If
                Next
                If (Not bIsValid) Then
                    Continue For
                End If

                SB = New StringBuilder
                lEnumSplitList = New List(Of String)

                sourceAnalysis = New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sEnumSource)

                For ii = 0 To sEnumSource.Length - 1
                    Select Case (sEnumSource(ii))
                        Case ","c
                            If (sourceAnalysis.GetParenthesisLevel(ii) > 0 OrElse sourceAnalysis.GetBracketLevel(ii) > 0 OrElse sourceAnalysis.GetBraceLevel(ii) > 0 OrElse sourceAnalysis.InNonCode(ii)) Then
                                Exit Select
                            End If

                            If (SB.Length < 1) Then
                                Exit Select
                            End If

                            sLine = SB.ToString
                            sLine = Regex.Replace(sLine.Trim, "\s+", " ")

                            iInvalidLen = Regex.Match(sLine, "^(.*?)(?<End>\b[a-zA-Z0-9_]+\b)").Groups("End").Index
                            If (iInvalidLen > 0) Then
                                sLine = sLine.Remove(0, iInvalidLen)
                            End If

                            lEnumSplitList.Add(sLine)
                            SB.Length = 0
                    End Select

                    If (Not sourceAnalysis.InSingleComment(ii) AndAlso Not sourceAnalysis.InMultiComment(ii)) Then
                        SB.Append(sEnumSource(ii))
                    End If
                Next

                If (SB.Length > 0) Then
                    sLine = SB.ToString
                    sLine = Regex.Replace(sLine.Trim, "\s+", " ")

                    iInvalidLen = Regex.Match(sLine, "^(.*?)(?<End>\b[a-zA-Z0-9_]+\b)").Groups("End").Index
                    If (iInvalidLen > 0) Then
                        sLine = sLine.Remove(0, iInvalidLen)

                        If (Not String.IsNullOrEmpty(sLine)) Then
                            lEnumSplitList.Add(sLine)
                            SB.Length = 0
                        End If
                    End If
                End If

                sComment = ""
                lEnumCommentArray = New String(lEnumSplitList.Count - 1) {}
                sEnumSourceLines = sEnumSource.Split(New String() {vbNewLine, vbLf}, 0)

                For ii = 0 To lEnumSplitList.Count - 1
                    iLine = 0
                    For iii = 0 To sEnumSourceLines.Length - 1
                        If (sEnumSourceLines(iii).Contains(lEnumSplitList(ii))) Then
                            iLine = iii
                            Exit For
                        End If
                    Next

                    bCommentStart = False
                    sComment = ""
                    For iii = iLine - 1 To 0 Step -1
                        mRegMatch2 = Regex.Match(sEnumSourceLines(iii), "^\s*(?<Start>//(.*?))$")
                        If (Not bCommentStart AndAlso mRegMatch2.Success) Then
                            sComment = mRegMatch2.Groups("Start").Value & Environment.NewLine & sComment
                            Continue For
                        End If

                        mRegMatch2 = Regex.Match(sEnumSourceLines(iii), "(?<Start>(/\*+))|(?<End>\*+/|(?<Pragma>)^\s*#pragma)")

                        If (Not bCommentStart AndAlso Not mRegMatch2.Groups("End").Success) Then
                            Exit For
                        End If

                        If (Not mRegMatch2.Groups("Pragma").Success) Then
                            bCommentStart = True
                        End If

                        Select Case (True)
                            Case mRegMatch2.Groups("End").Success AndAlso Not mRegMatch2.Groups("Pragma").Success
                                sComment = sEnumSourceLines(iii).Substring(0, mRegMatch2.Groups("End").Index + 2) & Environment.NewLine & sComment
                            Case mRegMatch2.Groups("Start").Success
                                sComment = sEnumSourceLines(iii).Substring(mRegMatch2.Groups("Start").Index, sEnumSourceLines(iii).Length - mRegMatch2.Groups("Start").Index) & Environment.NewLine & sComment
                            Case Else
                                sComment = sEnumSourceLines(iii) & Environment.NewLine & sComment
                        End Select

                        If (bCommentStart AndAlso mRegMatch2.Groups("Start").Success) Then
                            Exit For
                        End If
                    Next

                    lEnumCommentArray(ii) = sComment
                Next

                For ii = 0 To lEnumSplitList.Count - 1
                    Dim sEnumFull As String = lEnumSplitList(ii)
                    Dim sEnumComment As String = lEnumCommentArray(ii)

                    sEnumComment = New Regex("^\s+", RegexOptions.Multiline).Replace(sEnumComment, "")

                    Dim regMatch As Match = Regex.Match(sEnumFull, "^\s*(?<Tag>\b[a-zA-Z0-9_]+\b:)*(?<Name>\b[a-zA-Z0-9_]+\b)")
                    If (Not regMatch.Groups("Name").Success) Then
                        g_mFormMain.PrintInformation("[WARN]", String.Format("Failed to resolve type 'Enum': enum {0} {1}", sEnumName, sEnumFull), False, False, 15)
                        Continue For
                    End If

                    Dim sEnumVarName As String = regMatch.Groups("Name").Value

                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = IO.Path.GetFileName(sFile)
                    struc.sFullFunctionName = String.Format("funcenum {0} {1}", sEnumName, sEnumFull)
                    struc.sFunctionName = "public " & (New Regex("\b(public)\b").Replace(sEnumFull, sEnumName, 1)) 'sEnumFull.Replace("public(", sEnumName & "(" )
                    struc.sInfo = sEnumComment
                    struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM

                    If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                        lTmpAutocompleteList.Add(struc)
                    End If
                Next
            Next
        End If

        'Get methodmaps
        If ((ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("methodmap")) Then
            Dim mPossibleMethodmapMatches As MatchCollection = Regex.Matches(sSource, "^\s*\b(methodmap)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)(?<ParentingName>\s+\b[a-zA-Z0-9_]+\b){0,1}(?<FullParent>\s*\<\s*(?<Parent>\b[a-zA-Z0-9_]+\b)){0,1}\s*(?<BraceStart>\{)", RegexOptions.Multiline)
            Dim iBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sSource, "{"c, "}"c, 1, True)

            Dim mRegMatch As Match

            Dim bIsValid As Boolean
            Dim sMethodmapSource As String
            Dim iBraceIndex As Integer

            Dim sMethodMapName As String
            Dim sMethodMapHasParent As Boolean
            Dim sMethodMapParentName As String
            Dim sMethodMapFullParentName As String
            Dim sMethodMapParentingName As String

            For i = 0 To mPossibleMethodmapMatches.Count - 1
                mRegMatch = mPossibleMethodmapMatches(i)
                If (Not mRegMatch.Success) Then
                    Continue For
                End If

                bIsValid = False
                sMethodmapSource = ""
                iBraceIndex = mRegMatch.Groups("BraceStart").Index
                For ii = 0 To iBraceList.Length - 1
                    If (iBraceIndex = iBraceList(ii)(0)) Then
                        sMethodmapSource = sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                        bIsValid = True
                        Exit For
                    End If
                Next
                If (Not bIsValid) Then
                    Continue For
                End If

                sMethodMapName = mRegMatch.Groups("Name").Value
                sMethodMapHasParent = mRegMatch.Groups("Parent").Success
                sMethodMapParentName = mRegMatch.Groups("Parent").Value
                sMethodMapFullParentName = mRegMatch.Groups("FullParent").Value
                sMethodMapParentingName = mRegMatch.Groups("ParentingName").Value

                If (True) Then
                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = IO.Path.GetFileName(sFile)
                    struc.sFullFunctionName = "methodmap " & sMethodMapName & sMethodMapFullParentName
                    struc.sFunctionName = sMethodMapName
                    struc.sInfo = ""
                    struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP

                    If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                        lTmpAutocompleteList.Add(struc)
                    End If
                End If

                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sMethodmapSource)
                Dim iMethodmapBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sMethodmapSource, "("c, ")"c, 1, True)

                Dim mMethodMatches As MatchCollection = Regex.Matches(sMethodmapSource,
                                                                      String.Format("^\s*(?<Type>\b(property|public\s+(static\s+){2}native|public)\b)\s+((?<Tag>\b({0})\b\s)\s*(?<Name>\b[a-zA-Z0-9_]+\b)|(?<Constructor>\b{1}\b)|(?<Name>\b[a-zA-Z0-9_]+\b))\s*(?<BraceStart>\(){3}", sRegExEnum, sMethodMapName, "{0,1}", "{0,1}"),
                                                                      RegexOptions.Multiline)

                Dim SB As StringBuilder

                For ii = 0 To mMethodMatches.Count - 1
                    If (sourceAnalysis.InNonCode(mMethodMatches(ii).Groups("Type").Index)) Then
                        Continue For
                    End If

                    SB = New StringBuilder
                    For iii = mMethodMatches(ii).Groups("Type").Index - 1 To 0 Step -1
                        Select Case (sMethodmapSource(iii))
                            Case " "c, vbTab(0), vbLf(0), vbCr(0)
                                SB.Append(sMethodmapSource(iii))
                            Case Else
                                If (Not sourceAnalysis.InMultiComment(iii) AndAlso Not sourceAnalysis.InSingleComment(iii) AndAlso Not sourceAnalysis.InPreprocessor(iii)) Then
                                    Exit For
                                End If

                                SB.Append(sMethodmapSource(iii))
                        End Select
                    Next

                    Dim sComment As String = StrReverse(SB.ToString)
                    sComment = Regex.Replace(sComment, "^\s+", "", RegexOptions.Multiline)
                    sComment = Regex.Replace(sComment, "\s+$", "", RegexOptions.Multiline)
                    Dim sType As String = mMethodMatches(ii).Groups("Type").Value
                    Dim sTag As String = mMethodMatches(ii).Groups("Tag").Value.Trim
                    Dim sName As String = mMethodMatches(ii).Groups("Name").Value

                    If (sType = "property") Then
                        Dim struc As New STRUC_AUTOCOMPLETE
                        struc.sFile = IO.Path.GetFileName(sFile)
                        struc.sFullFunctionName = String.Format("methodmap {0}{1}{2} {3} {4}", sMethodMapName, sMethodMapParentingName, If(sMethodMapHasParent, " < " & sMethodMapParentName, ""), sTag, sMethodMapName)
                        struc.sFunctionName = String.Format("{0}.{1}", sMethodMapName, sName)
                        struc.sInfo = sComment
                        struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP Or struc.ParseTypeFullNames(sType)

                        If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                            lTmpAutocompleteList.Add(struc)
                        End If
                    Else
                        Dim bIsConstructor As Boolean = mMethodMatches(ii).Groups("Constructor").Success
                        Dim iBraceStart As Integer = mMethodMatches(ii).Groups("BraceStart").Index
                        Dim sBraceString As String = Nothing

                        For iii = 0 To iMethodmapBraceList.Length - 1
                            If (iBraceStart = iMethodmapBraceList(iii)(0)) Then
                                sBraceString = sMethodmapSource.Substring(iMethodmapBraceList(iii)(0), iMethodmapBraceList(iii)(1) - iMethodmapBraceList(iii)(0) + 1)
                                Exit For
                            End If
                        Next

                        If (String.IsNullOrEmpty(sBraceString)) Then
                            Continue For
                        End If

                        If (bIsConstructor) Then
                            Dim struc As New STRUC_AUTOCOMPLETE
                            struc.sFile = IO.Path.GetFileName(sFile)
                            struc.sFullFunctionName = String.Format("methodmap {0}{1} {2}{3}", sMethodMapName, If(sMethodMapHasParent, " < " & sMethodMapParentName, ""), sMethodMapName, sBraceString)
                            struc.sFunctionName = String.Format("{0}.{1}", sMethodMapName, sMethodMapName)
                            struc.sInfo = sComment
                            struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP Or struc.ParseTypeFullNames(sType)

                            If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                                lTmpAutocompleteList.Add(struc)
                            End If
                        Else
                            Dim struc As New STRUC_AUTOCOMPLETE
                            struc.sFile = IO.Path.GetFileName(sFile)
                            struc.sFullFunctionName = String.Format("methodmap {0} {1} {2}{3} {4}{5}", sType, sTag, sMethodMapName, If(sMethodMapHasParent, " < " & sMethodMapParentName, ""), sName, sBraceString)
                            struc.sFunctionName = String.Format("{0}.{1}", sMethodMapName, sName)
                            struc.sInfo = sComment
                            struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP Or struc.ParseTypeFullNames(sType)

                            If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                                lTmpAutocompleteList.Add(struc)
                            End If
                        End If
                    End If
                Next
            Next
        End If

        'Get typesets
        If ((ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("typeset")) Then
            Dim mPossibleMethodmapMatches As MatchCollection = Regex.Matches(sSource, "^\s*\b(typeset)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)", RegexOptions.Multiline)
            Dim iBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sSource, "{"c, "}"c, 1, True)

            Dim mRegMatch As Match

            Dim bIsValid As Boolean
            Dim sMethodmapSource As String
            Dim iBraceIndex As Integer

            Dim sMethodMapName As String

            For i = 0 To mPossibleMethodmapMatches.Count - 1
                mRegMatch = mPossibleMethodmapMatches(i)
                If (Not mRegMatch.Success) Then
                    Continue For
                End If

                bIsValid = False
                sMethodmapSource = ""
                iBraceIndex = mRegMatch.Groups("BraceStart").Index
                For ii = 0 To iBraceList.Length - 1
                    If (iBraceIndex = iBraceList(ii)(0)) Then
                        sMethodmapSource = sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                        bIsValid = True
                        Exit For
                    End If
                Next
                If (Not bIsValid) Then
                    Continue For
                End If

                sMethodMapName = mRegMatch.Groups("Name").Value

                If (True) Then
                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = IO.Path.GetFileName(sFile)
                    struc.sFullFunctionName = "enum " & sMethodMapName
                    struc.sFunctionName = sMethodMapName
                    struc.sInfo = ""
                    struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM

                    If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                        lTmpAutocompleteList.Add(struc)
                    End If
                End If

                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sMethodmapSource)
                Dim iMethodmapBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sMethodmapSource, "("c, ")"c, 1, True)

                Dim mMethodMatches As MatchCollection = Regex.Matches(sMethodmapSource, String.Format("^\s*(?<Type>\b(function)\b)\s+(?<Tag>\b({0})\b)\s*(?<BraceStart>\()", sRegExEnum), RegexOptions.Multiline)

                Dim SB As StringBuilder

                For ii = 0 To mMethodMatches.Count - 1
                    If (sourceAnalysis.InNonCode(mMethodMatches(ii).Groups("Type").Index)) Then
                        Continue For
                    End If

                    SB = New StringBuilder
                    For iii = mMethodMatches(ii).Groups("Type").Index - 1 To 0 Step -1
                        Select Case (sMethodmapSource(iii))
                            Case " "c, vbTab(0), vbLf(0), vbCr(0)
                                SB.Append(sMethodmapSource(iii))
                            Case Else
                                If (Not sourceAnalysis.InMultiComment(iii) AndAlso Not sourceAnalysis.InSingleComment(iii) AndAlso Not sourceAnalysis.InPreprocessor(iii)) Then
                                    Exit For
                                End If

                                SB.Append(sMethodmapSource(iii))
                        End Select
                    Next

                    Dim sComment As String = StrReverse(SB.ToString)
                    sComment = Regex.Replace(sComment, "^\s+", "", RegexOptions.Multiline)
                    sComment = Regex.Replace(sComment, "\s+$", "", RegexOptions.Multiline)
                    Dim sType As String = mMethodMatches(ii).Groups("Type").Value
                    Dim sTag As String = mMethodMatches(ii).Groups("Tag").Value.Trim
                    Dim iBraceStart As Integer = mMethodMatches(ii).Groups("BraceStart").Index
                    Dim sBraceString As String = Nothing

                    For iii = 0 To iMethodmapBraceList.Length - 1
                        If (iBraceStart = iMethodmapBraceList(iii)(0)) Then
                            sBraceString = sMethodmapSource.Substring(iMethodmapBraceList(iii)(0), iMethodmapBraceList(iii)(1) - iMethodmapBraceList(iii)(0) + 1)
                            Exit For
                        End If
                    Next

                    If (String.IsNullOrEmpty(sBraceString)) Then
                        Continue For
                    End If

                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = IO.Path.GetFileName(sFile)
                    struc.sFullFunctionName = String.Format("typeset {0} {1} {2}{3}", sMethodMapName, sType, sTag, sBraceString)
                    struc.sFunctionName = String.Format("public {0} {1}{2}", sTag, sMethodMapName, sBraceString)
                    struc.sInfo = sComment
                    struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET Or struc.ParseTypeFullNames(sType)

                    If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                        lTmpAutocompleteList.Add(struc)
                    End If
                Next
            Next
        End If

        If ((ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("typedef")) Then
            Dim mPossibleMethodmapMatches As MatchCollection = Regex.Matches(sSource, String.Format("^\s*\b(typedef)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s+=\s+\b(function)\b\s+(?<Tag>\b({0})\b)\s*(?<BraceStart>\()", sRegExEnum), RegexOptions.Multiline)
            Dim iBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sSource, "("c, ")"c, 1, True)

            Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

            For i = 0 To mPossibleMethodmapMatches.Count - 1
                Dim mRegMatch As Match = mPossibleMethodmapMatches(i)
                If (Not mRegMatch.Success) Then
                    Continue For
                End If

                If (sourceAnalysis.InNonCode(mRegMatch.Index)) Then
                    Continue For
                End If

                Dim bIsValid As Boolean = False
                Dim sBraceString As String = ""
                Dim iBraceIndex As Integer = mRegMatch.Groups("BraceStart").Index
                For ii = 0 To iBraceList.Length - 1
                    If (iBraceIndex = iBraceList(ii)(0)) Then
                        sBraceString = sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                        bIsValid = True
                        Exit For
                    End If
                Next
                If (Not bIsValid) Then
                    Continue For
                End If

                If (String.IsNullOrEmpty(sBraceString)) Then
                    Continue For
                End If

                sBraceString = sBraceString.Trim

                Dim sName As String = mRegMatch.Groups("Name").Value
                Dim sTag As String = mRegMatch.Groups("Tag").Value

                Dim SB As New StringBuilder
                For iii = mRegMatch.Index - 1 To 0 Step -1
                    Select Case (sSource(iii))
                        Case " "c, vbTab(0), vbLf(0), vbCr(0)
                            SB.Append(sSource(iii))
                        Case Else
                            If (Not sourceAnalysis.InMultiComment(iii) AndAlso Not sourceAnalysis.InSingleComment(iii) AndAlso Not sourceAnalysis.InPreprocessor(iii)) Then
                                Exit For
                            End If

                            SB.Append(sSource(iii))
                    End Select
                Next

                Dim sComment As String = StrReverse(SB.ToString)
                sComment = Regex.Replace(sComment, "^\s+", "", RegexOptions.Multiline)
                sComment = Regex.Replace(sComment, "\s+$", "", RegexOptions.Multiline)


                Dim struc As New STRUC_AUTOCOMPLETE
                struc.sFile = IO.Path.GetFileName(sFile)
                struc.sFullFunctionName = String.Format("typedef {0} = function {1} ({2})", sName, sTag, sBraceString)
                struc.sFunctionName = String.Format("public {0} {1}({2})", sTag, sName, sBraceString)
                struc.sInfo = sComment
                struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF

                If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                    lTmpAutocompleteList.Add(struc)
                End If
            Next
        End If
    End Sub

    Private Sub VariableAutocompleteUpdate_Thread()
        Try
            Dim sSourceFile As String = CStr(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File))
            Dim sActiveSource As String = CStr(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent))

            'g_mFormMain.PrintInformation("[INFO]", "Variable autocomplete update started...")
            If (String.IsNullOrEmpty(sSourceFile) OrElse Not IO.File.Exists(sSourceFile)) Then
                'g_mFormMain.PrintInformation("[ERRO]", "Variable autocomplete update failed! Could not get current source file!")
                Return
            End If

            'No autocomplete entries?
            If (g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.Count < 1) Then
                Return
            End If


            Dim lTmpVarAutocompleteList As New ClassSyncList(Of STRUC_AUTOCOMPLETE)

            'Parse variables and create methodmaps for variables
            If (True) Then
                Dim sRegExEnumPattern As String = String.Format("(\b{0}\b)", String.Join("\b|\b", GetEnumNames(g_mFormMain.g_ClassSyntaxTools.lAutocompleteList)))

                If (ClassSettings.g_iSettingsVarAutocompleteCurrentSourceOnly) Then
                    ParseVariables_Pre(sActiveSource, sSourceFile, sSourceFile, sRegExEnumPattern, lTmpVarAutocompleteList, g_mFormMain.g_ClassSyntaxTools.lAutocompleteList)
                Else
                    Dim sFiles As String() = GetIncludeFiles(sActiveSource, sSourceFile, sSourceFile)
                    For i = 0 To sFiles.Length - 1
                        ParseVariables_Pre(sActiveSource, sSourceFile, sFiles(i), sRegExEnumPattern, lTmpVarAutocompleteList, g_mFormMain.g_ClassSyntaxTools.lAutocompleteList)
                    Next
                End If

                ParseVariables_Post(sActiveSource, sSourceFile, sRegExEnumPattern, lTmpVarAutocompleteList, g_mFormMain.g_ClassSyntaxTools.lAutocompleteList)
            End If

            g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.DoSync(
                Sub()
                    g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.RemoveAll(Function(j As STRUC_AUTOCOMPLETE) (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                    g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.AddRange(lTmpVarAutocompleteList.ToArray)
                End Sub)

            'g_mFormMain.PrintInformation("[INFO]", "Variable autocomplete update finished!")
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            g_mFormMain.PrintInformation("[ERRO]", "Variable autocomplete update failed! " & ex.Message, False, False, 1)
            ClassExceptionLog.WriteToLog(ex)
        End Try
    End Sub

    Private Structure STRUC_PARSE_ARGUMENT_ITEM
        Dim sArgument As String
        Dim sFile As String
    End Structure

    Private Sub ParseVariables_Pre(sActiveSource As String, sActiveSourceFile As String, ByRef sFile As String, ByRef sRegExEnumPattern As String, ByRef lTmpVarAutocompleteList As ClassSyncList(Of STRUC_AUTOCOMPLETE), ByRef lTmpAutocompleteList As ClassSyncList(Of STRUC_AUTOCOMPLETE))
        Dim sSource As String
        If (sActiveSourceFile.ToLower = sFile.ToLower) Then
            sSource = sActiveSource
        Else
            sSource = IO.File.ReadAllText(sFile)
        End If

        CleanUpNewLinesSource(sSource)

        'Parse variables
        If (True) Then
            Dim sInitTypesPattern As String = "\b(new|decl|static|const)\b" '"public" is already taken care off
            Dim sOldStyleVarPattern As String = String.Format("(?<Init>{0}\s+)(?<IsConst>\b(const)\b\s+){2}((?<Tag>{1})\:\s*(?<Var>\b[a-zA-Z0-9_]+\b)|(?<Var>\b[a-zA-Z0-9_]+\b))($|\W)", sInitTypesPattern, sRegExEnumPattern, "{0,1}")
            Dim sNewStyleVarPattern As String = String.Format("(?<IsConst>\b(const)\b\s+){1}(?<Tag>{0})\s+(?<Var>\b[a-zA-Z0-9_]+\b)($|\W)", sRegExEnumPattern, "{0,1}")

            Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)
            Dim lCommaLinesList As New List(Of String)
            Dim codeBuilder As New StringBuilder

            Dim bInvalidBracketLevel As Boolean = False
            Dim bInvalidParentLevel As Boolean = False
            'This is the easiest and yet stupiest idea to resolve multi-decls i've ever come up with...
            For i = 0 To sSource.Length - 1
                If (i <> sSource.Length - 1) Then
                    If (sourceAnalysis.InNonCode(i)) Then
                        Continue For
                    End If

                    If (sourceAnalysis.GetBracketLevel(i) > 0) Then
                        If (Not bInvalidBracketLevel) Then
                            bInvalidBracketLevel = True
                        End If
                        Continue For
                    ElseIf (bInvalidBracketLevel) Then
                        bInvalidBracketLevel = False
                        Continue For
                    End If

                    If (sourceAnalysis.GetParenthesisLevel(i) > 0) Then
                        If (Not bInvalidParentLevel) Then
                            bInvalidParentLevel = True
                            codeBuilder.Append("(")
                        End If
                        Continue For
                    ElseIf (bInvalidParentLevel) Then
                        bInvalidParentLevel = False
                        codeBuilder.Append(")")
                        Continue For
                    End If
                End If

                If (sSource(i) = ","c OrElse sSource(i) = "="c OrElse i = sSource.Length - 1) Then
                    Dim sLine As String = codeBuilder.ToString.Trim

                    If (Not String.IsNullOrEmpty(sLine)) Then
                        lCommaLinesList.Add(sLine)
                    End If

                    codeBuilder.Length = 0
                    Continue For
                End If

                codeBuilder.Append(sSource(i))
            Next

            '0: Not yet found
            '1: Old style
            '2: New style
            Dim iLastInitStyle As Byte = 0
            Dim iLastInitIndex As Integer = 0
            Dim sLastTag As String = Nothing

            For Each sLine As String In lCommaLinesList
                Select Case (iLastInitStyle)
                    Case 1 'Old Style
                        If (ClassSettings.g_iSettingsAutocompleteSyntax <> ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX AndAlso ClassSettings.g_iSettingsAutocompleteSyntax <> ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6) Then
                            Exit Select
                        End If

                        Dim mMatch As Match = Regex.Match(sLine, String.Format("^((?<Tag>{0})\:\s*)*(?<Var>\b[a-zA-Z_][a-zA-Z0-9_]*\b)((?<End>$)|(?<IsFunc>\()|(?<IsMethodmap>\.)|(?<IsTag>\:)|(?<More>\W))", sRegExEnumPattern))
                        Dim sTag As String = mMatch.Groups("Tag").Value.Trim
                        Dim sVar As String = mMatch.Groups("Var").Value.Trim

                        If (mMatch.Groups("IsFunc").Success OrElse mMatch.Groups("IsMethodmap").Success OrElse mMatch.Groups("IsTag").Success OrElse Not mMatch.Groups("Var").Success) Then
                            Exit Select
                        End If

                        If (g_mFormMain.g_ClassSyntaxTools.IsForbiddenVariableName(sVar)) Then
                            Exit Select
                        End If

                        If (Regex.IsMatch(sVar, sRegExEnumPattern)) Then
                            Exit Select
                        End If

                        If (lTmpAutocompleteList.Exists(Function(j As STRUC_AUTOCOMPLETE)
                                                            If ((j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) Then
                                                                Return False
                                                            End If

                                                            Return Regex.IsMatch(j.sFunctionName, String.Format("\b{0}\b", Regex.Escape(sVar)))
                                                        End Function)) Then
                            Exit Select
                        End If

                        If (String.IsNullOrEmpty(sTag)) Then
                            sTag = "int"
                        End If

                        Dim tmpFile As String = IO.Path.GetFileName(sFile)
                        Dim autoItem As STRUC_AUTOCOMPLETE = lTmpVarAutocompleteList.Find(Function(j As STRUC_AUTOCOMPLETE) j.sFile.ToLower = tmpFile.ToLower AndAlso j.sFunctionName = sVar AndAlso (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                        If (autoItem Is Nothing) Then
                            autoItem = New STRUC_AUTOCOMPLETE
                            autoItem.sFile = tmpFile.ToLower
                            autoItem.sFullFunctionName = String.Format("{0} > {1}", sVar, sTag)
                            autoItem.sFunctionName = sVar
                            autoItem.sInfo = ""
                            autoItem.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                            lTmpVarAutocompleteList.Add(autoItem)
                        Else
                            If (Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                autoItem.sFullFunctionName = String.Format("{0}|{1}", autoItem.sFullFunctionName, sTag)
                            End If
                        End If

                    Case 2 'New Style
                        If (ClassSettings.g_iSettingsAutocompleteSyntax <> ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX AndAlso ClassSettings.g_iSettingsAutocompleteSyntax <> ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7) Then
                            Exit Select
                        End If

                        Dim mMatch As Match = Regex.Match(sLine, String.Format("^(?<Tag>{0}\s+)*(?<Var>\b[a-zA-Z_][a-zA-Z0-9_]*\b)((?<End>$)|(?<IsFunc>\()|(?<IsMethodmap>\.)|(?<IsTag>\:)|(?<More>\W))", sRegExEnumPattern))
                        Dim sTag As String = mMatch.Groups("Tag").Value.Trim
                        Dim sVar As String = mMatch.Groups("Var").Value.Trim

                        If (mMatch.Groups("IsFunc").Success OrElse mMatch.Groups("IsMethodmap").Success OrElse mMatch.Groups("IsTag").Success OrElse Not mMatch.Groups("Var").Success) Then
                            Exit Select
                        End If

                        If (g_mFormMain.g_ClassSyntaxTools.IsForbiddenVariableName(sVar)) Then
                            Exit Select
                        End If

                        If (Regex.IsMatch(sVar, sRegExEnumPattern)) Then
                            Exit Select
                        End If

                        If (lTmpAutocompleteList.Exists(Function(j As STRUC_AUTOCOMPLETE)
                                                            If ((j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) Then
                                                                Return False
                                                            End If

                                                            Return Regex.IsMatch(j.sFunctionName, String.Format("\b{0}\b", Regex.Escape(sVar)))
                                                        End Function)) Then
                            Exit Select
                        End If

                        If (String.IsNullOrEmpty(sTag)) Then
                            If (String.IsNullOrEmpty(sLastTag)) Then
                                sTag = "int"
                            Else
                                sTag = sLastTag
                            End If
                        End If

                        Dim tmpFile As String = IO.Path.GetFileName(sFile)
                        Dim autoItem As STRUC_AUTOCOMPLETE = lTmpVarAutocompleteList.Find(Function(j As STRUC_AUTOCOMPLETE) j.sFile.ToLower = tmpFile.ToLower AndAlso j.sFunctionName = sVar AndAlso (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                        If (autoItem Is Nothing) Then
                            autoItem = New STRUC_AUTOCOMPLETE
                            autoItem.sFile = tmpFile.ToLower
                            autoItem.sFullFunctionName = String.Format("{0} > {1}", sVar, sTag)
                            autoItem.sFunctionName = sVar
                            autoItem.sInfo = ""
                            autoItem.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                            lTmpVarAutocompleteList.Add(autoItem)
                        Else
                            If (Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                autoItem.sFullFunctionName = String.Format("{0}|{1}", autoItem.sFullFunctionName, sTag)
                            End If
                        End If

                End Select

                iLastInitIndex = 0
                If (sLine.Contains(";"c)) Then
                    iLastInitStyle = 0
                End If


                If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6) Then
                    For Each mMatch As Match In Regex.Matches(sLine, sOldStyleVarPattern)
                        Dim iIndex As Integer = mMatch.Groups("Var").Index
                        Dim sInit As String = mMatch.Groups("Init").Value.Trim
                        Dim sTag As String = mMatch.Groups("Tag").Value.Trim
                        Dim sVar As String = mMatch.Groups("Var").Value.Trim
                        Dim bIsConst As Boolean = mMatch.Groups("IsConst").Success

                        If (iLastInitIndex < iIndex) Then
                            iLastInitIndex = iIndex
                            iLastInitStyle = 1 'Old style
                        End If

                        sLastTag = Nothing

                        If (g_mFormMain.g_ClassSyntaxTools.IsForbiddenVariableName(sVar)) Then
                            Continue For
                        End If

                        If (Regex.IsMatch(sVar, sRegExEnumPattern)) Then
                            Continue For
                        End If

                        If (lTmpAutocompleteList.Exists(Function(j As STRUC_AUTOCOMPLETE)
                                                            If ((j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) Then
                                                                Return False
                                                            End If

                                                            Return Regex.IsMatch(j.sFunctionName, String.Format("\b{0}\b", Regex.Escape(sVar)))
                                                        End Function)) Then
                            Continue For
                        End If

                        If (String.IsNullOrEmpty(sTag)) Then
                            sTag = "int"
                        End If

                        Dim tmpFile As String = IO.Path.GetFileName(sFile)
                        Dim autoItem As STRUC_AUTOCOMPLETE = lTmpVarAutocompleteList.Find(Function(j As STRUC_AUTOCOMPLETE) j.sFile.ToLower = tmpFile.ToLower AndAlso j.sFunctionName = sVar AndAlso (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                        If (autoItem Is Nothing) Then
                            autoItem = New STRUC_AUTOCOMPLETE
                            autoItem.sFile = tmpFile.ToLower
                            autoItem.sFullFunctionName = String.Format("{0} > {1}", sVar, If(bIsConst, "const:", "") & sTag)
                            autoItem.sFunctionName = sVar
                            autoItem.sInfo = ""
                            autoItem.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                            lTmpVarAutocompleteList.Add(autoItem)
                        Else
                            If (Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                autoItem.sFullFunctionName = String.Format("{0}|{1}", autoItem.sFullFunctionName, sTag)
                            End If
                            If (bIsConst AndAlso Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", "const"))) Then
                                autoItem.sFullFunctionName = String.Format("{0}:{1}", "const", autoItem.sFullFunctionName)
                            End If
                        End If
                    Next
                End If

                If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7) Then
                    For Each mMatch As Match In Regex.Matches(sLine, sNewStyleVarPattern)
                        Dim iIndex As Integer = mMatch.Groups("Var").Index
                        Dim sInit As String = mMatch.Groups("Init").Value.Trim
                        Dim sTag As String = mMatch.Groups("Tag").Value.Trim
                        Dim sVar As String = mMatch.Groups("Var").Value.Trim
                        Dim bIsConst As Boolean = mMatch.Groups("IsConst").Success

                        If (iLastInitIndex < iIndex) Then
                            iLastInitIndex = iIndex
                            iLastInitStyle = 2 'New style
                        End If

                        sLastTag = Nothing

                        If (String.IsNullOrEmpty(sTag)) Then
                            Continue For
                        End If

                        If (g_mFormMain.g_ClassSyntaxTools.IsForbiddenVariableName(sVar)) Then
                            Continue For
                        End If

                        If (Regex.IsMatch(sVar, sRegExEnumPattern)) Then
                            Continue For
                        End If

                        If (lTmpAutocompleteList.Exists(Function(j As STRUC_AUTOCOMPLETE)
                                                            If ((j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF OrElse
                                                                    (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) Then
                                                                Return False
                                                            End If

                                                            Return Regex.IsMatch(j.sFunctionName, String.Format("\b{0}\b", Regex.Escape(sVar)))
                                                        End Function)) Then
                            Continue For
                        End If

                        sLastTag = sTag

                        Dim tmpFile As String = IO.Path.GetFileName(sFile)
                        Dim autoItem As STRUC_AUTOCOMPLETE = lTmpVarAutocompleteList.Find(Function(j As STRUC_AUTOCOMPLETE) j.sFile.ToLower = tmpFile.ToLower AndAlso j.sFunctionName = sVar AndAlso (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                        If (autoItem Is Nothing) Then
                            autoItem = New STRUC_AUTOCOMPLETE
                            autoItem.sFile = tmpFile.ToLower
                            autoItem.sFullFunctionName = String.Format("{0} > {1}", sVar, If(bIsConst, "const:", "") & sTag)
                            autoItem.sFunctionName = sVar
                            autoItem.sInfo = ""
                            autoItem.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                            lTmpVarAutocompleteList.Add(autoItem)
                        Else
                            If (Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                autoItem.sFullFunctionName = String.Format("{0}|{1}", autoItem.sFullFunctionName, sTag)
                            End If
                            If (bIsConst AndAlso Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", "const"))) Then
                                autoItem.sFullFunctionName = String.Format("{0}:{1}", "const", autoItem.sFullFunctionName)
                            End If
                        End If
                    Next
                End If
            Next
        End If

    End Sub

    Private Sub ParseVariables_Post(sActiveSource As String, sActiveSourceFile As String, ByRef sRegExEnumPattern As String, ByRef lTmpVarAutocompleteList As ClassSyncList(Of STRUC_AUTOCOMPLETE), ByRef lTmpAutocompleteList As ClassSyncList(Of STRUC_AUTOCOMPLETE))
        'Parse function argument variables
        If (True) Then
            Dim lArgList As New List(Of STRUC_PARSE_ARGUMENT_ITEM)
            Dim codeBuilder As New StringBuilder

            For Each autoItem In lTmpAutocompleteList
                If ((autoItem.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) Then
                    Continue For
                End If

                If (ClassSettings.g_iSettingsVarAutocompleteCurrentSourceOnly) Then
                    If (Not String.IsNullOrEmpty(sActiveSourceFile) AndAlso IO.Path.GetFileName(sActiveSourceFile).ToLower <> autoItem.sFile.ToLower) Then
                        Continue For
                    End If
                End If

                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(autoItem.sFullFunctionName)
                codeBuilder.Length = 0

                Dim bInvalidBracketLevel As Boolean = False
                Dim bInvalidBraceLevel As Boolean = False
                Dim bInvalidParentLevel As Boolean = False
                Dim bNeedSave As Boolean = False
                'This is the easiest and yet stupiest idea to resolve multi-decls i've ever come up with...
                For i = 0 To autoItem.sFullFunctionName.Length - 1
                    If (bNeedSave OrElse i = autoItem.sFullFunctionName.Length - 1) Then
                        bNeedSave = False

                        Dim sLine As String = codeBuilder.ToString.Trim
                        sLine = Regex.Replace(sLine, "(\=)+(.*$)", "")
                        sLine = Regex.Replace(sLine, Regex.Escape("&"), "")
                        sLine = Regex.Replace(sLine, Regex.Escape("@"), "")

                        If (Not String.IsNullOrEmpty(sLine)) Then
                            lArgList.Add(New STRUC_PARSE_ARGUMENT_ITEM With {.sArgument = sLine, .sFile = autoItem.sFile})
                        End If

                        codeBuilder.Length = 0
                    End If

                    If (sourceAnalysis.InNonCode(i)) Then
                        Continue For
                    End If

                    If (sourceAnalysis.GetBracketLevel(i) > 0) Then
                        If (Not bInvalidBracketLevel) Then
                            bInvalidBracketLevel = True
                        End If
                        Continue For
                    ElseIf (bInvalidBracketLevel) Then
                        bInvalidBracketLevel = False
                        Continue For
                    End If

                    If (sourceAnalysis.GetBraceLevel(i) > 0) Then
                        If (Not bInvalidBraceLevel) Then
                            bInvalidBraceLevel = True
                        End If
                        Continue For
                    ElseIf (bInvalidBraceLevel) Then
                        bInvalidBraceLevel = False
                        Continue For
                    End If

                    If (sourceAnalysis.GetParenthesisLevel(i) > 0) Then
                        If (bInvalidParentLevel) Then
                            If (autoItem.sFullFunctionName(i) = ","c) Then
                                bNeedSave = True
                                Continue For
                            Else
                                codeBuilder.Append(autoItem.sFullFunctionName(i))
                            End If
                        End If
                        If (Not bInvalidParentLevel) Then
                            bInvalidParentLevel = True
                        End If

                        Continue For
                    ElseIf (bInvalidParentLevel) Then
                        bInvalidParentLevel = False
                        bNeedSave = True
                        Continue For
                    End If
                Next
            Next

            For Each sArg As STRUC_PARSE_ARGUMENT_ITEM In lArgList
                'Old style
                If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6) Then
                    Dim mMatch As Match = Regex.Match(sArg.sArgument, String.Format("(?<OneSevenTag>\b{0}\b\s+)*((?<Tag>\b{1}\b)\:\s*)*(?<Var>\b[a-zA-Z_][a-zA-Z0-9_]*\b)$", sRegExEnumPattern, sRegExEnumPattern))
                    Dim sTag As String = If(String.IsNullOrEmpty(mMatch.Groups("Tag").Value), "int", mMatch.Groups("Tag").Value).Trim
                    Dim sVar As String = mMatch.Groups("Var").Value.Trim
                    Dim bIsOneSeven As Boolean = mMatch.Groups("OneSevenTag").Success

                    If (Not bIsOneSeven AndAlso mMatch.Success AndAlso mMatch.Groups("Var").Success AndAlso Not g_mFormMain.g_ClassSyntaxTools.IsForbiddenVariableName(sVar)) Then
                        Dim tmpFile As String = IO.Path.GetFileName(sArg.sFile)
                        Dim autoItem As STRUC_AUTOCOMPLETE = lTmpVarAutocompleteList.Find(Function(j As STRUC_AUTOCOMPLETE) j.sFile.ToLower = tmpFile.ToLower AndAlso j.sFunctionName = sVar AndAlso (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                        If (autoItem Is Nothing) Then
                            autoItem = New STRUC_AUTOCOMPLETE
                            autoItem.sFile = tmpFile.ToLower
                            autoItem.sFullFunctionName = String.Format("{0} > {1}", sVar, sTag)
                            autoItem.sFunctionName = sVar
                            autoItem.sInfo = ""
                            autoItem.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                            lTmpVarAutocompleteList.Add(autoItem)
                        Else
                            If (Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                autoItem.sFullFunctionName = String.Format("{0}|{1}", autoItem.sFullFunctionName, sTag)
                            End If
                        End If
                    End If
                End If

                'New style
                If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7) Then
                    Dim mMatch As Match = Regex.Match(sArg.sArgument, String.Format("(?<Tag>\b{0}\b)\s+(?<Var>\b[a-zA-Z_][a-zA-Z0-9_]*\b)$", sRegExEnumPattern))
                    Dim sTag As String = If(String.IsNullOrEmpty(mMatch.Groups("Tag").Value), "int", mMatch.Groups("Tag").Value).Trim
                    Dim sVar As String = mMatch.Groups("Var").Value.Trim

                    If (mMatch.Success AndAlso mMatch.Groups("Var").Success AndAlso Not g_mFormMain.g_ClassSyntaxTools.IsForbiddenVariableName(sVar)) Then
                        Dim tmpFile As String = IO.Path.GetFileName(sArg.sFile)
                        Dim autoItem As STRUC_AUTOCOMPLETE = lTmpVarAutocompleteList.Find(Function(j As STRUC_AUTOCOMPLETE) j.sFile.ToLower = tmpFile.ToLower AndAlso j.sFunctionName = sVar AndAlso (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                        If (autoItem Is Nothing) Then
                            autoItem = New STRUC_AUTOCOMPLETE
                            autoItem.sFile = tmpFile.ToLower
                            autoItem.sFullFunctionName = String.Format("{0} > {1}", sVar, sTag)
                            autoItem.sFunctionName = sVar
                            autoItem.sInfo = ""
                            autoItem.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                            lTmpVarAutocompleteList.Add(autoItem)
                        Else
                            If (Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                autoItem.sFullFunctionName = String.Format("{0}|{1}", autoItem.sFullFunctionName, sTag)
                            End If
                        End If
                    End If
                End If
            Next
        End If

        'Make methodmaps using variables
        If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7) Then
            Dim lVarMethodmapList As New List(Of STRUC_AUTOCOMPLETE)

            For Each item In lTmpVarAutocompleteList
                If ((item.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) Then
                    Continue For
                End If

                For Each item2 In lTmpAutocompleteList
                    If ((item2.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP OrElse Not item2.sFunctionName.Contains("."c)) Then
                        Continue For
                    End If

                    Dim splitMethodmap As String() = item2.sFunctionName.Split(New String() {"."c}, 0)
                    If (splitMethodmap.Length <> 2) Then
                        Continue For
                    End If

                    Dim sMethodmapName As String = splitMethodmap(0)
                    Dim sMethodmapMethod As String = splitMethodmap(1)

                    'If (lTmpAutocompleteList.Exists(Function(j As STRUC_AUTOCOMPLETE) Regex.IsMatch(j.sFunctionName, "\b" & Regex.Escape(item.sFunctionName & "." & sMethodmapMethod) & "\b"))) Then
                    '    Continue For
                    'End If

                    If (Not Regex.IsMatch(item.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sMethodmapName)))) Then
                        Continue For
                    End If

                    Dim autoItem As New STRUC_AUTOCOMPLETE
                    autoItem.sFile = IO.Path.GetFileName(item.sFile).ToLower
                    autoItem.sFullFunctionName = String.Format("{0}.{1}", sMethodmapName, sMethodmapMethod)
                    autoItem.sFunctionName = String.Format("{0}.{1}", item.sFunctionName, sMethodmapMethod)
                    autoItem.sInfo = item2.sInfo
                    autoItem.mType = (STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE Or item2.mType) And Not STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP
                    lVarMethodmapList.Add(autoItem)
                Next
            Next

            lTmpVarAutocompleteList.AddRange(lVarMethodmapList)
        End If
    End Sub

    ''' <summary>
    ''' Gets all include files from a file
    ''' </summary>
    ''' <param name="sPath"></param>
    ''' <returns>Array if include file paths</returns>
    Public Function GetIncludeFiles(sActiveSource As String, sActiveSourceFile As String, sPath As String, Optional bFindAll As Boolean = False, Optional iMaxDirectoryDepth As Integer = 10) As String()
        Dim lList As New List(Of String)
        Dim lLoadedIncludes As New Dictionary(Of String, Boolean)

        GetIncludeFilesRecursive(sActiveSource, sActiveSourceFile, sPath, lList, lLoadedIncludes)

        If (bFindAll) Then
            While True
                If (String.IsNullOrEmpty(sActiveSourceFile) OrElse Not IO.File.Exists(sActiveSourceFile)) Then
                    g_mFormMain.PrintInformation("[ERRO]", "Could not read includes! Could not get current source file!")
                    Exit While
                End If

                'Check includes
                Dim sIncludePaths As String
                If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    If (String.IsNullOrEmpty(sActiveSourceFile) OrElse Not IO.File.Exists(sActiveSourceFile)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "Could not read includes! Could not get current source file!", False, False, 1)
                        Exit While
                    End If
                    sIncludePaths = IO.Path.Combine(IO.Path.GetDirectoryName(sActiveSourceFile), "include")
                Else
                    sIncludePaths = ClassSettings.g_sConfigOpenIncludeFolders
                End If

                'Check compiler
                Dim sCompilerPath As String
                If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    If (String.IsNullOrEmpty(sActiveSourceFile) OrElse Not IO.File.Exists(sActiveSourceFile)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "Could not read includes! Could not get current source file!", False, False, 1)
                        Exit While
                    End If
                    sCompilerPath = IO.Path.GetDirectoryName(sActiveSourceFile)
                ElseIf (Not String.IsNullOrEmpty(ClassSettings.g_sConfigCompilerPath) AndAlso IO.File.Exists(ClassSettings.g_sConfigCompilerPath)) Then
                    sCompilerPath = IO.Path.GetDirectoryName(ClassSettings.g_sConfigCompilerPath)
                Else
                    sCompilerPath = ""
                End If

                GetIncludeFilesRecursiveAll(IO.Path.GetDirectoryName(sActiveSourceFile), lList, lLoadedIncludes, iMaxDirectoryDepth)

                For Each sInclude As String In sIncludePaths.Split(";"c)
                    If (Not IO.Directory.Exists(sInclude)) Then
                        Continue For
                    End If

                    GetIncludeFilesRecursiveAll(sInclude, lList, lLoadedIncludes, iMaxDirectoryDepth)
                Next

                GetIncludeFilesRecursiveAll(sCompilerPath, lList, lLoadedIncludes, iMaxDirectoryDepth)

                Exit While
            End While
        End If

        For Each i In lLoadedIncludes
            If (i.Value) Then
                Continue For
            End If

            g_mFormMain.PrintInformation("[ERRO]", String.Format("Could not read include: {0}", i.Key), False, False, 5)
        Next

        Return lList.ToArray
    End Function

    Private Sub GetIncludeFilesRecursiveAll(sInclude As String, ByRef lList As List(Of String), lLoadedIncludes As Dictionary(Of String, Boolean), iMaxDirectoryDepth As Integer)
        Dim sFiles As String()
        Dim sDirectories As String()

        sFiles = IO.Directory.GetFiles(sInclude)
        sDirectories = IO.Directory.GetDirectories(sInclude)

        For Each i As String In sFiles
            Dim sFileName As String = IO.Path.GetFileNameWithoutExtension(i)

            If (Not IO.File.Exists(i)) Then
                Continue For
            End If

            If (lList.Contains(i.ToLower)) Then
                lLoadedIncludes(sFileName.ToLower) = True
                Continue For
            End If

            Select Case (IO.Path.GetExtension(i).ToLower)
                Case ".sp", ".sma", ".p", ".pwn", ".inc"
                    lList.Add(i.ToLower)
                    lLoadedIncludes(sFileName.ToLower) = True
            End Select
        Next

        If (iMaxDirectoryDepth < 1) Then
            g_mFormMain.PrintInformation("[ERRO]", "Max recursive directory search depth reached!", False, False, 5)
            Return
        End If

        For Each i As String In sDirectories
            GetIncludeFilesRecursiveAll(i, lList, lLoadedIncludes, iMaxDirectoryDepth - 1)
        Next
    End Sub

    Private Sub GetIncludeFilesRecursive(sActiveSource As String, sActiveSourceFile As String, sPath As String, ByRef lList As List(Of String), lLoadedIncludes As Dictionary(Of String, Boolean))
        Dim sSource As String

        Dim sFileName As String = IO.Path.GetFileNameWithoutExtension(sPath)

        If (sActiveSourceFile.ToLower = sPath.ToLower) Then
            If (lList.Contains(sPath.ToLower)) Then
                lLoadedIncludes(sFileName.ToLower) = True
                Return
            End If

            lList.Add(sPath.ToLower)
            lLoadedIncludes(sFileName.ToLower) = True

            sSource = sActiveSource
        Else
            If (Not IO.File.Exists(sPath)) Then
                If (Not lLoadedIncludes.ContainsKey(sFileName.ToLower)) Then
                    lLoadedIncludes(sFileName.ToLower) = False
                End If

                Return
            End If

            If (lList.Contains(sPath.ToLower)) Then
                lLoadedIncludes(sFileName.ToLower) = True
                Return
            End If

            lList.Add(sPath.ToLower)
            lLoadedIncludes(sFileName.ToLower) = True

            sSource = IO.File.ReadAllText(sPath)
        End If

        Dim lPathList As New List(Of String)
        Dim sCurrentDir As String = IO.Path.GetDirectoryName(sPath)

        Dim sCorrectPath As String
        Dim sMatchValue As String

        While True
            'Check includes
            Dim sIncludePaths As String
            If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                If (String.IsNullOrEmpty(sActiveSourceFile) OrElse Not IO.File.Exists(sActiveSourceFile)) Then
                    g_mFormMain.PrintInformation("[ERRO]", "Could not read includes! Could not get current source file!", False, False, 1)
                    Exit While
                End If
                sIncludePaths = IO.Path.Combine(IO.Path.GetDirectoryName(sActiveSourceFile), "include")
            Else
                sIncludePaths = ClassSettings.g_sConfigOpenIncludeFolders
            End If

            'Check compiler
            Dim sCompilerPath As String
            If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                If (String.IsNullOrEmpty(sActiveSourceFile) OrElse Not IO.File.Exists(sActiveSourceFile)) Then
                    g_mFormMain.PrintInformation("[ERRO]", "Could not read includes! Could not get current source file!", False, False, 1)
                    Exit While
                End If
                sCompilerPath = IO.Path.GetDirectoryName(sActiveSourceFile)
            ElseIf (Not String.IsNullOrEmpty(ClassSettings.g_sConfigCompilerPath) AndAlso IO.File.Exists(ClassSettings.g_sConfigCompilerPath)) Then
                sCompilerPath = IO.Path.GetDirectoryName(ClassSettings.g_sConfigCompilerPath)
            Else
                sCompilerPath = ""
            End If

            For Each sInclude As String In sIncludePaths.Split(";"c)
                If (ClassSettings.g_iSettingsAlwaysLoadDefaultIncludes) Then
                    If (sActiveSourceFile.ToLower = sPath.ToLower) Then
                        For Each sDefaultInc As String In New String() {"sourcemod"}
                            Select Case (True)
                                Case IO.File.Exists(IO.Path.Combine(sInclude, sDefaultInc))
                                    sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sInclude, sDefaultInc))
                                Case IO.File.Exists(IO.Path.Combine(sCurrentDir, sDefaultInc))
                                    sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sCurrentDir, sDefaultInc))
                                Case IO.File.Exists(IO.Path.Combine(sCompilerPath, sDefaultInc))
                                    sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sCompilerPath, sDefaultInc))

                                Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sInclude, sDefaultInc)))
                                    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sInclude, sDefaultInc)))

                                Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sCurrentDir, sDefaultInc)))
                                    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sCurrentDir, sDefaultInc)))

                                Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sCompilerPath, sDefaultInc)))
                                    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sCompilerPath, sDefaultInc)))

                                Case Else
                                    If (Not lLoadedIncludes.ContainsKey(sDefaultInc.ToLower)) Then
                                        lLoadedIncludes(sDefaultInc.ToLower) = False
                                    End If
                                    Continue For
                            End Select

                            If (Not IO.File.Exists(sCorrectPath)) Then
                                Continue For
                            End If

                            If (lPathList.Contains(sCorrectPath.ToLower)) Then
                                Continue For
                            End If

                            lPathList.Add(sCorrectPath.ToLower)
                        Next
                    End If
                End If

                Using SR As New IO.StringReader(sSource)
                    While True
                        Dim sLine As String = SR.ReadLine()
                        If (sLine Is Nothing) Then
                            Exit While
                        End If

                        If (Not sLine.Contains("#include") AndAlso Not sLine.Contains("#tryinclude")) Then
                            Continue While
                        End If

                        Dim mRegMatch As Match = Regex.Match(sLine, "^\s*#(include|tryinclude)\s+(\<(?<PathInc>.*?)\>|""(?<PathFull>.*?)"")\s*$", RegexOptions.Compiled)
                        If (Not mRegMatch.Success) Then
                            Continue While
                        End If

                        Select Case (True)
                            Case mRegMatch.Groups("PathInc").Success
                                sMatchValue = mRegMatch.Groups("PathInc").Value.Replace("/"c, "\"c)

                                Select Case (True)
                                    Case IO.File.Exists(IO.Path.Combine(sInclude, sMatchValue))
                                        sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sInclude, sMatchValue))
                                    Case IO.File.Exists(IO.Path.Combine(sCompilerPath, sMatchValue))
                                        sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sCompilerPath, sMatchValue))

                                    'Case IO.File.Exists(String.Format("{0}.sp", IO.Path.Combine(sInclude, sMatchValue)))
                                    '    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sp", IO.Path.Combine(sInclude, sMatchValue)))
                                    'Case IO.File.Exists(String.Format("{0}.sma", IO.Path.Combine(sInclude, sMatchValue)))
                                    '    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sma", IO.Path.Combine(sInclude, sMatchValue)))
                                    'Case IO.File.Exists(String.Format("{0}.p", IO.Path.Combine(sInclude, sMatchValue)))
                                    '    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.p", IO.Path.Combine(sInclude, sMatchValue)))
                                    'Case IO.File.Exists(String.Format("{0}.pwn", IO.Path.Combine(sInclude, sMatchValue)))
                                    '    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.pwn", IO.Path.Combine(sInclude, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sInclude, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sInclude, sMatchValue)))

                                    'Case IO.File.Exists(String.Format("{0}.sp", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    '    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sp", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    'Case IO.File.Exists(String.Format("{0}.sma", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    '    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sma", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    'Case IO.File.Exists(String.Format("{0}.p", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    '    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.p", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    'Case IO.File.Exists(String.Format("{0}.pwn", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    '    sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.pwn", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sCompilerPath, sMatchValue)))

                                    Case Else
                                        If (Not lLoadedIncludes.ContainsKey(sMatchValue.ToLower)) Then
                                            lLoadedIncludes(sMatchValue.ToLower) = False
                                        End If
                                        Continue While
                                End Select

                            Case mRegMatch.Groups("PathFull").Success
                                sMatchValue = mRegMatch.Groups("PathFull").Value.Replace("/"c, "\"c)

                                Select Case (True)
                                    Case sMatchValue.Length > 1 AndAlso sMatchValue(1) = ":"c AndAlso IO.File.Exists(sMatchValue)
                                        sCorrectPath = IO.Path.GetFullPath(sMatchValue)

                                    Case IO.File.Exists(IO.Path.Combine(sInclude, sMatchValue))
                                        sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sInclude, sMatchValue))
                                    Case IO.File.Exists(IO.Path.Combine(sCurrentDir, sMatchValue))
                                        sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sCurrentDir, sMatchValue))
                                    Case IO.File.Exists(IO.Path.Combine(sCompilerPath, sMatchValue))
                                        sCorrectPath = IO.Path.GetFullPath(IO.Path.Combine(sCompilerPath, sMatchValue))

                                    Case IO.File.Exists(String.Format("{0}.sp", IO.Path.Combine(sInclude, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sp", IO.Path.Combine(sInclude, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.sma", IO.Path.Combine(sInclude, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sma", IO.Path.Combine(sInclude, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.p", IO.Path.Combine(sInclude, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.p", IO.Path.Combine(sInclude, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.pwn", IO.Path.Combine(sInclude, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.pwn", IO.Path.Combine(sInclude, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sInclude, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sInclude, sMatchValue)))

                                    Case IO.File.Exists(String.Format("{0}.sp", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sp", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.sma", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sma", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.p", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.p", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.pwn", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.pwn", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sCurrentDir, sMatchValue)))

                                    Case IO.File.Exists(String.Format("{0}.sp", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sp", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.sma", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.sma", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.p", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.p", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.pwn", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.pwn", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                    Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sCompilerPath, sMatchValue)))
                                        sCorrectPath = IO.Path.GetFullPath(String.Format("{0}.inc", IO.Path.Combine(sCompilerPath, sMatchValue)))

                                    Case Else
                                        If (Not lLoadedIncludes.ContainsKey(sMatchValue.ToLower)) Then
                                            lLoadedIncludes(sMatchValue.ToLower) = False
                                        End If
                                        Continue While
                                End Select

                            Case Else
                                Continue While
                        End Select

                        If (Not IO.File.Exists(sCorrectPath)) Then
                            Continue While
                        End If

                        If (lPathList.Contains(sCorrectPath.ToLower)) Then
                            Continue While
                        End If

                        lPathList.Add(sCorrectPath.ToLower)
                    End While
                End Using
            Next

            Exit While
        End While

        For i = 0 To lPathList.Count - 1
            GetIncludeFilesRecursive(sActiveSource, sActiveSourceFile, lPathList(i), lList, lLoadedIncludes)
        Next
    End Sub
End Class
