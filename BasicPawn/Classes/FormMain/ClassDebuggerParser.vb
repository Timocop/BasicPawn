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


Imports System.Text
Imports System.Text.RegularExpressions

Public Class ClassDebuggerParser
    Private g_mFormMain As FormMain

    Public Event OnBreakpointsUpdate()
    Public Event OnWatchersUpdate()

    Public Shared g_sDebuggerFilesExt As String = ".bpdebug"

    Public Shared g_sBreakpointName As String = "BPDBreakpoint"
    Public Shared g_sDebuggerBreakpointIgnoreExt As String = ".ignore" & g_sDebuggerFilesExt 'If exist, the breakpoint is disabled
    Public Shared g_sDebuggerBreakpointTriggerExt As String = ".trigger" & g_sDebuggerFilesExt 'If exist, BasicPawn knows this breakpoint has been triggered
    Public Shared g_sDebuggerBreakpointContinueExt As String = ".continue" & g_sDebuggerFilesExt 'If exist, the breakpoint will continue
    Public Shared g_sDebuggerBreakpointContinueVarExt As String = ".continuev" & g_sDebuggerFilesExt 'If exist, the breakpoint will continue with its custom return value

    Public Shared g_sWatcherName As String = "BPDWatcher"
    Public Shared g_sDebuggerWatcherValueExt As String = ".value" & g_sDebuggerFilesExt

    Structure STRUC_DEBUGGER_ITEM
        Dim sGUID As String

        Dim iLine As Integer

        Dim iIndex As Integer
        Dim iLength As Integer
        Dim iTotalLength As Integer

        Dim iOffset As Integer

        Dim sArguments As String
        Dim sTotalFunction As String
    End Structure
    Public g_lBreakpointList As New List(Of STRUC_DEBUGGER_ITEM)
    Public g_lWatcherList As New List(Of STRUC_DEBUGGER_ITEM)

    Public Sub New(f As FormMain)
        g_mFormMain = f
    End Sub

    ''' <summary>
    ''' Updates the breakpoint list.
    ''' </summary>
    ''' <param name="sSource"></param>
    Public Sub UpdateBreakpoints(sSource As String, bKeepIdentity As Boolean, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
        Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, iLanguage)

        If (Not bKeepIdentity) Then
            g_lBreakpointList.Clear()
        End If

        Dim iListIndex As Integer = 0
        For Each mMatch As Match In Regex.Matches(sSource, String.Format("\b{0}\b\s*(?<Length>)(?<Arguments>\(){1}", g_sBreakpointName, "{0,1}"))
            Dim iIndex As Integer = mMatch.Index
            Dim bHasArgument As Boolean = mMatch.Groups("Arguments").Success

            If (mSourceAnalysis.m_InNonCode(iIndex) OrElse Not bHasArgument) Then
                Continue For
            End If

            Dim iArgumentIndex As Integer = mMatch.Groups("Arguments").Index

            Dim sGUID As String = Guid.NewGuid.ToString
            Dim iLine As Integer = sSource.Substring(0, mMatch.Index).Split(New String() {vbLf}, 0).Length
            Dim iLineIndex As Integer = 0
            For i = iIndex - 1 To 0 Step -1
                If (sSource(i) = vbLf) Then
                    Exit For
                End If

                iLineIndex += 1
            Next

            Dim iLength As Integer = mMatch.Groups("Length").Index - mMatch.Index
            Dim iTotalLength As Integer = 0
            Dim sArguments As New StringBuilder
            Dim sTotalFunction As New StringBuilder

            Dim iStartLevel As Integer = mSourceAnalysis.GetParenthesisLevel(iIndex, Nothing)
            Dim bGetArguments As Boolean = False
            For i = iIndex To sSource.Length - 1
                iTotalLength += 1

                sTotalFunction.Append(sSource(i))

                Dim iParentRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                If (iStartLevel + 1 = mSourceAnalysis.GetParenthesisLevel(i, iParentRange) AndAlso
                            iParentRange = ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END) Then
                    bGetArguments = False
                    Exit For
                End If

                If (bGetArguments) Then
                    sArguments.Append(sSource(i))
                End If

                If (i = iArgumentIndex) Then
                    bGetArguments = True
                End If
            Next

            If (iTotalLength < 1) Then
                Continue For
            End If

            If (bKeepIdentity) Then
                g_lBreakpointList(iListIndex) = New STRUC_DEBUGGER_ITEM With {
                    .sGUID = g_lBreakpointList(iListIndex).sGUID,
                    .iLine = iLine,
                    .iIndex = iLineIndex,
                    .iLength = iLength,
                    .iTotalLength = iTotalLength,
                    .iOffset = iIndex,
                    .sArguments = sArguments.ToString,
                    .sTotalFunction = sTotalFunction.ToString
                }
            Else
                g_lBreakpointList.Add(New STRUC_DEBUGGER_ITEM With {
                    .sGUID = sGUID,
                    .iLine = iLine,
                    .iIndex = iLineIndex,
                    .iLength = iLength,
                    .iTotalLength = iTotalLength,
                    .iOffset = iIndex,
                    .sArguments = sArguments.ToString,
                    .sTotalFunction = sTotalFunction.ToString
                })
            End If

            iListIndex += 1
        Next

        RaiseEvent OnBreakpointsUpdate()
    End Sub

    ''' <summary>
    ''' Updates the breakpoint list.
    ''' </summary>
    ''' <param name="sSource"></param>
    Public Sub UpdateWatchers(sSource As String, bKeepIdentity As Boolean, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
        Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, iLanguage)

        If (Not bKeepIdentity) Then
            g_lWatcherList.Clear()
        End If

        Dim iListIndex As Integer = 0
        For Each mMatch As Match In Regex.Matches(sSource, String.Format("\b{0}\b\s*(?<Length>)(?<Arguments>\(){1}", g_sWatcherName, "{0,1}"))
            Dim iIndex As Integer = mMatch.Index
            Dim bHasArgument As Boolean = mMatch.Groups("Arguments").Success

            If (mSourceAnalysis.m_InNonCode(iIndex) OrElse Not bHasArgument) Then
                Continue For
            End If

            Dim iArgumentIndex As Integer = mMatch.Groups("Arguments").Index

            Dim sGUID As String = Guid.NewGuid.ToString
            Dim iLine As Integer = sSource.Substring(0, mMatch.Index).Split(New String() {vbLf}, 0).Length
            Dim iLineIndex As Integer = 0
            For i = iIndex - 1 To 0 Step -1
                If (sSource(i) = vbLf) Then
                    Exit For
                End If

                iLineIndex += 1
            Next

            Dim iLength As Integer = mMatch.Groups("Length").Index - mMatch.Index
            Dim iTotalLength As Integer = 0
            Dim sArguments As New StringBuilder
            Dim sTotalFunction As New StringBuilder

            Dim iStartLevel As Integer = mSourceAnalysis.GetParenthesisLevel(iIndex, Nothing)
            Dim bGetArguments As Boolean = False
            For i = iIndex To sSource.Length - 1
                iTotalLength += 1

                sTotalFunction.Append(sSource(i))

                Dim iParentRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                If (iStartLevel + 1 = mSourceAnalysis.GetParenthesisLevel(i, iParentRange) AndAlso
                            iParentRange = ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END) Then
                    bGetArguments = False
                    Exit For
                End If

                If (bGetArguments) Then
                    sArguments.Append(sSource(i))
                End If

                If (i = iArgumentIndex) Then
                    bGetArguments = True
                End If
            Next

            If (iTotalLength < 1) Then
                Continue For
            End If

            If (bKeepIdentity) Then
                g_lWatcherList(iListIndex) = New STRUC_DEBUGGER_ITEM With {
                    .sGUID = g_lWatcherList(iListIndex).sGUID,
                    .iLine = iLine,
                    .iIndex = iLineIndex,
                    .iLength = iLength,
                    .iTotalLength = iTotalLength,
                    .iOffset = iIndex,
                    .sArguments = sArguments.ToString,
                    .sTotalFunction = sTotalFunction.ToString
                }
            Else
                g_lWatcherList.Add(New STRUC_DEBUGGER_ITEM With {
                    .sGUID = sGUID,
                    .iLine = iLine,
                    .iIndex = iLineIndex,
                    .iLength = iLength,
                    .iTotalLength = iTotalLength,
                    .iOffset = iIndex,
                    .sArguments = sArguments.ToString,
                    .sTotalFunction = sTotalFunction.ToString
                })
            End If

            iListIndex += 1
        Next

        RaiseEvent OnWatchersUpdate()
    End Sub

    ''' <summary>
    ''' Gets a list of usefull autocompletes
    ''' </summary>
    ''' <returns></returns>
    Public Function GetDebuggerAutocomplete() As ClassSyntaxTools.STRUC_AUTOCOMPLETE()
        Dim lAutocomplete As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
        Dim mInfoBuilder As New StringBuilder

        mInfoBuilder.AppendLine("/**")
        mInfoBuilder.AppendLine(" * Pauses the plugin until manually resumed. Also shows the current position in the BasicPawn Debugger.")
        mInfoBuilder.AppendLine(" * Optionaly you can return a custom non-array value.")
        mInfoBuilder.AppendLine(" *")
        mInfoBuilder.AppendLine(" * WARN: Do not use this in 'float-to-float' comparisons.")
        mInfoBuilder.AppendLine(" *       The operator will see the 'any' type as non-float and parse it incorrectly.")
        mInfoBuilder.AppendLine(" */")
        lAutocomplete.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mInfoBuilder.ToString,
                                                                  "BasicPawn.exe",
                                                                  "",
                                                                  ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEBUG,
                                                                  g_sBreakpointName,
                                                                  g_sBreakpointName,
                                                                  String.Format("any:{0}(any:val=0)", g_sBreakpointName)))

        mInfoBuilder.Length = 0
        mInfoBuilder.AppendLine("/**")
        mInfoBuilder.AppendLine(" * Prints the passed value into the BasicPawn Debugger.")
        mInfoBuilder.AppendLine(" *")
        mInfoBuilder.AppendLine(" * WARN: Do not use this in 'float-to-float' comparisons.")
        mInfoBuilder.AppendLine(" *       The operator will see the 'any' type as non-float and parse it incorrectly.")
        mInfoBuilder.AppendLine(" */")
        lAutocomplete.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mInfoBuilder.ToString,
                                                                  "BasicPawn.exe",
                                                                  "",
                                                                  ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEBUG,
                                                                  g_sWatcherName,
                                                                  g_sWatcherName,
                                                                  String.Format("any:{0}(any:val=0)", g_sWatcherName)))

        Return lAutocomplete.ToArray
    End Function

    Structure STRUC_SM_EXCEPTION_STACK_TRACE
        Dim iLine As Integer
        Dim sFileName As String
        Dim sFunctionName As String
        Dim bNativeFault As Boolean
    End Structure

    Structure STRUC_SM_EXCEPTION
        Dim sExceptionInfo As String
        Dim sBlamingFile As String
        Dim dLogDate As Date
        Dim mStackTraces As STRUC_SM_EXCEPTION_STACK_TRACE()
    End Structure

    Structure STRUC_SM_FATAL_EXCEPTION
        Dim sExceptionInfo As String
        Dim sBlamingFile As String
        Dim dLogDate As Date
        Dim mMiscInformation As Object()
    End Structure

    ''' <summary>
    ''' Reads all sourcemod exceptions from a log.
    ''' </summary>
    ''' <param name="sLogLines"></param>
    ''' <returns></returns>
    Public Function ReadSourceModLogExceptions(sLogLines As String()) As STRUC_SM_EXCEPTION()
        Dim iExpectingState As Integer = 0

        Dim mSMException As New STRUC_SM_EXCEPTION

        Dim lSMExceptions As New List(Of STRUC_SM_EXCEPTION)
        Dim lSMStackTraces As New List(Of STRUC_SM_EXCEPTION_STACK_TRACE)

        For i = 0 To sLogLines.Length - 1
            Dim mExceptionInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+ \- [0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] Exception reported\:(?<Message>.*?)$")
            If (mExceptionInfo.Success) Then
                If (iExpectingState = 3) Then
                    mSMException.mStackTraces = lSMStackTraces.ToArray
                    lSMExceptions.Add(mSMException)

                    iExpectingState = 0
                End If

                mSMException = New STRUC_SM_EXCEPTION
                lSMStackTraces = New List(Of STRUC_SM_EXCEPTION_STACK_TRACE)

                Dim sDate As String = mExceptionInfo.Groups("Date").Value
                Dim sMessage As String = mExceptionInfo.Groups("Message").Value

                Dim dDate As Date
                If (Not Date.TryParseExact(sDate.Trim, "MM/dd/yyyy - HH:mm:ss", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, dDate)) Then
                    Continue For
                End If

                mSMException.sExceptionInfo = sMessage.Trim
                mSMException.dLogDate = dDate

                iExpectingState = 1
                Continue For
            End If

            Select Case (iExpectingState)
                Case 1 'Expecting: [SM] Blaming
                    Dim mBlamingInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] Blaming\:(?<File>.*?)$")
                    If (mBlamingInfo.Success) Then
                        Dim sFile As String = mBlamingInfo.Groups("File").Value

                        mSMException.sBlamingFile = sFile.Trim

                        iExpectingState = 2
                    Else
                        iExpectingState = 0
                    End If

                Case 2 'Expecting: [SM] Call stack trace:
                    Dim mStackTraceInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] Call stack trace\:\s*$")
                    If (mStackTraceInfo.Success) Then
                        iExpectingState = 3
                    Else
                        iExpectingState = 0
                    End If

                Case 3 'Expecting: [SM]   [0] ... 
                    Dim mMoreStackTraceInfo As Match = Regex.Match(sLogLines(i),
                                                                   "(" &
                                                                   "(?<PluginFault>^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\]   \[[0-9]+\] Line (?<Line>[0-9]+), (?<File>.*?)\:\:(?<Function>.*?)$)" &
                                                                   "|" &
                                                                   "(?<NativeFault>^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\]   \[[0-9]+\] (?<Function>.*?)$)" &
                                                                   ")")

                    Select Case (True)
                        Case mMoreStackTraceInfo.Groups("PluginFault").Success
                            Dim iLine As Integer = CInt(mMoreStackTraceInfo.Groups("Line").Value.Trim)
                            Dim sFile As String = mMoreStackTraceInfo.Groups("File").Value.Trim
                            Dim sFunction As String = mMoreStackTraceInfo.Groups("Function").Value.Trim

                            lSMStackTraces.Add(New STRUC_SM_EXCEPTION_STACK_TRACE() With {
                                                 .iLine = iLine,
                                                 .sFileName = sFile,
                                                 .sFunctionName = sFunction,
                                                 .bNativeFault = False})

                        Case mMoreStackTraceInfo.Groups("NativeFault").Success
                            Dim sFunction As String = mMoreStackTraceInfo.Groups("Function").Value.Trim

                            lSMStackTraces.Add(New STRUC_SM_EXCEPTION_STACK_TRACE() With {
                                                 .iLine = -1,
                                                 .sFileName = "",
                                                 .sFunctionName = sFunction,
                                                 .bNativeFault = True})

                        Case Else
                            mSMException.mStackTraces = lSMStackTraces.ToArray
                            lSMExceptions.Add(mSMException)

                            iExpectingState = 0
                    End Select
            End Select
        Next

        Return lSMExceptions.ToArray
    End Function

    '''' <summary>
    '''' Reads all sourcemod exceptions from a fatal log.
    '''' </summary>
    '''' <param name="sLogLines"></param>
    '''' <returns></returns>
    'Public Function ReadSourceModLogMemoryLeaks(sLogLines As String()) As STRUC_SM_FATAL_EXCEPTION()
    '    Dim iExpectingState As Integer = 0

    '    Dim smException As New STRUC_SM_FATAL_EXCEPTION

    '    Dim smExceptionsList As New List(Of STRUC_SM_FATAL_EXCEPTION)
    '    Dim smStackTraceList As New List(Of Object())

    '    For i = 0 To sLogLines.Length - 1
    '        Dim mExceptionInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+ \- [0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] MEMORY LEAK DETECTED IN PLUGIN \(file ""(?<File>.*?)""\)$")
    '        If (mExceptionInfo.Success) Then
    '            If (iExpectingState = 3) Then
    '                smException.sMiscInformation = smStackTraceList.ToArray
    '                smExceptionsList.Add(smException)

    '                iExpectingState = 0
    '            End If

    '            smException = New STRUC_SM_FATAL_EXCEPTION
    '            smStackTraceList = New List(Of Object())

    '            Dim sDate As String = mExceptionInfo.Groups("Date").Value
    '            Dim sMessage As String = "Memory leak detected"
    '            Dim sFile As String = mExceptionInfo.Groups("File").Value

    '            Dim dDate As Date
    '            If (Not Date.TryParseExact(sDate.Trim, "MM/dd/yyyy - HH:mm:ss", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, dDate)) Then
    '                Continue For
    '            End If

    '            smException.sBlamingFile = sFile
    '            smException.sExceptionInfo = sMessage.Trim
    '            smException.dLogDate = dDate

    '            iExpectingState = 1
    '            Continue For
    '        End If

    '        Select Case (iExpectingState)
    '            Case 1 'Expecting: [SM] Blaming
    '                Dim mBlamingInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] Blaming\:(?<File>.*?)$")
    '                If (mBlamingInfo.Success) Then
    '                    Dim sFile As String = mBlamingInfo.Groups("File").Value

    '                    smException.sBlamingFile = sFile.Trim

    '                    iExpectingState = 2
    '                Else
    '                    iExpectingState = 0
    '                End If

    '            Case 2 'Expecting: [SM] Call stack trace:
    '                Dim mStackTraceInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] Call stack trace\:\s*$")
    '                If (mStackTraceInfo.Success) Then
    '                    iExpectingState = 3
    '                Else
    '                    iExpectingState = 0
    '                End If

    '            Case 3 'Expecting: [SM]   [0] ... 
    '                Dim mMoreStackTraceInfo As Match = Regex.Match(sLogLines(i),
    '                                                               "(" &
    '                                                               "(?<PluginFault>^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\]   \[[0-9]+\] Line (?<Line>[0-9]+), (?<File>.*?)\:\:(?<Function>.*?)$)" &
    '                                                               "|" &
    '                                                               "(?<NativeFault>^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\]   \[[0-9]+\] (?<Function>.*?)$)" &
    '                                                               ")")

    '                Select Case (True)
    '                    Case mMoreStackTraceInfo.Groups("PluginFault").Success
    '                        Dim iLine As Integer = CInt(mMoreStackTraceInfo.Groups("Line").Value.Trim)
    '                        Dim sFile As String = mMoreStackTraceInfo.Groups("File").Value.Trim
    '                        Dim sFunction As String = mMoreStackTraceInfo.Groups("Function").Value.Trim

    '                        smStackTraceList.Add(New STRUC_SM_EXCEPTION_STACK_TRACE() With {
    '                                             .iLine = iLine,
    '                                             .sFileName = sFile,
    '                                             .m_FunctionName = sFunction,
    '                                             .bNativeFault = False})

    '                    Case mMoreStackTraceInfo.Groups("NativeFault").Success
    '                        Dim sFunction As String = mMoreStackTraceInfo.Groups("Function").Value.Trim

    '                        smStackTraceList.Add(New STRUC_SM_EXCEPTION_STACK_TRACE() With {
    '                                             .iLine = -1,
    '                                             .sFileName = "",
    '                                             .m_FunctionName = sFunction,
    '                                             .bNativeFault = True})

    '                    Case Else
    '                        smException.mStackTrace = smStackTraceList.ToArray
    '                        smExceptionsList.Add(smException)

    '                        iExpectingState = 0
    '                End Select
    '        End Select
    '    Next

    '    Return smExceptionsList.ToArray
    'End Function

    Public Sub CleanupDebugPlaceholder(ByRef sSource As String, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
        'TODO: Add more debug placeholder
        With New ClassBreakpoints(g_mFormMain)
            .RemoveAllBreakpoints(sSource, iLanguage)
        End With
        With New ClassWatchers(g_mFormMain)
            .RemoveAllWatchers(sSource, iLanguage)
        End With
    End Sub

    Public Sub CleanupDebugPlaceholder(mFormMain As FormMain)
        'TODO: Add more debug placeholder
        With New ClassBreakpoints(g_mFormMain)
            .TextEditorRemoveAllBreakpoints()
        End With
        With New ClassWatchers(g_mFormMain)
            .TextEditorRemoveAllWatchers()
        End With
    End Sub

    Public Function GetDebugPlaceholderNames() As String()
        'TODO: Add more debug placeholder
        Return New List(Of String) From {
            g_sBreakpointName,
            g_sWatcherName
        }.ToArray
    End Function

    Public Function HasDebugPlaceholder(sSource As String) As Boolean
        For Each sName As String In GetDebugPlaceholderNames()
            If (Regex.IsMatch(sSource, String.Format("\b{0}\b\s*\(", Regex.Escape(sName)))) Then
                Return True
            End If
        Next

        Return False
    End Function

    ''' <summary>
    ''' SourceMod and BasicPawn communication.
    ''' </summary>
    Class ClassRunnerEngine
        Public g_sDebuggerRunnerGuid As String = Guid.NewGuid.ToString
        Public Shared g_sDebuggerRunnerCmdFileExt As String = ".cmd" & g_sDebuggerFilesExt
        Public Shared g_sDebuggerRunnerEntityFileExt As String = ".entities" & g_sDebuggerFilesExt
        Public Shared g_sDebuggerRunnerPingExt As String = ".ping" & g_sDebuggerFilesExt

        ''' <summary>
        ''' Generates a engine source which can be used to accept commands, when its running.
        ''' </summary>
        ''' <param name="bNewSyntax"></param>
        ''' <returns></returns>
        Public Function GenerateRunnerEngine(bNewSyntax As Boolean) As String
            Dim SB As New StringBuilder

            If (bNewSyntax) Then
                'TODO: Add new synrax engine
                SB.AppendLine(My.Resources.Debugger_CommandRunnerEngineOld)
            Else
                SB.AppendLine(My.Resources.Debugger_CommandRunnerEngineOld)
            End If

            SB.Replace("{IndentifierGUID}", g_sDebuggerRunnerGuid)

            Return SB.ToString
        End Function

        ''' <summary>
        ''' Sends a command to the engine plugin, when its running.
        ''' </summary>
        ''' <param name="sGamePath"></param>
        ''' <param name="sCmd"></param>
        Public Sub AcceptCommand(sGamePath As String, sCmd As String)
            Dim sFile As String = IO.Path.Combine(sGamePath, g_sDebuggerRunnerGuid & g_sDebuggerRunnerCmdFileExt)

            IO.File.AppendAllText(sFile, sCmd & Environment.NewLine)
        End Sub
    End Class

    Class ClassBreakpoints
        Private g_mFormMain As FormMain

        Public Sub New(f As FormMain)
            g_mFormMain = f
        End Sub


        ''' <summary>
        ''' Inserts one breakpoint using the caret position in the text editor
        ''' </summary>
        Public Sub TextEditorInsertBreakpointAtCaret()
            Dim mActiveTextEditor As TextEditorControlEx = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor

            If (mActiveTextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected AndAlso mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0) Then
                Dim iOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Offset
                Dim iLength As Integer = mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Length

                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset + iLength, ")")
                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}(", ClassDebuggerParser.g_sBreakpointName))
                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
            Else
                Dim sCaretWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True, False, True)

                If (String.IsNullOrEmpty(sCaretWord)) Then
                    Dim iOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.Caret.Offset
                    mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}();", ClassDebuggerParser.g_sBreakpointName))
                Else
                    Dim iOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.Caret.Offset

                    For Each m As Match In Regex.Matches(mActiveTextEditor.ActiveTextAreaControl.Document.TextContent, String.Format("(?<Word>\b{0}\b)((?<Function>\s*\()|(?<Array>\s*\[)|)", Regex.Escape(sCaretWord)))
                        Dim iStartOffset As Integer = m.Groups("Word").Index
                        Dim iStartLen As Integer = m.Groups("Word").Value.Length
                        Dim bIsFunction As Boolean = m.Groups("Function").Success
                        Dim bIsArray As Boolean = m.Groups("Array").Success

                        If (iOffset < iStartOffset OrElse iOffset > (iStartOffset + iStartLen)) Then
                            Continue For
                        End If

                        If (bIsArray) Then
                            Dim sSource As String = mActiveTextEditor.ActiveTextAreaControl.Document.TextContent
                            Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                            Dim iFullLength As Integer = 0
                            Dim iStartLevel As Integer = mSourceAnalysis.GetBracketLevel(iStartOffset, Nothing)
                            For i = iStartOffset To sSource.Length - 1
                                iFullLength += 1

                                Dim iBracketRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                                If (iStartLevel + 1 = mSourceAnalysis.GetBracketLevel(i, iBracketRange) AndAlso
                                            iBracketRange = ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END) Then
                                    Exit For
                                End If
                            Next

                            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                            mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset + iFullLength, ")")
                            mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sBreakpointName))
                            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                        ElseIf (bIsFunction) Then
                            Dim sSource As String = mActiveTextEditor.ActiveTextAreaControl.Document.TextContent
                            Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                            Dim iFullLength As Integer = 0
                            Dim iStartLevel As Integer = mSourceAnalysis.GetParenthesisLevel(iStartOffset, Nothing)
                            For i = iStartOffset To sSource.Length - 1
                                iFullLength += 1

                                Dim iParentRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                                If (iStartLevel + 1 = mSourceAnalysis.GetParenthesisLevel(i, iParentRange) AndAlso
                                            iParentRange = ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END) Then
                                    Exit For
                                End If
                            Next

                            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                            mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset + iFullLength, ")")
                            mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sBreakpointName))
                            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                        Else
                            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                            mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset + iStartLen, ")")
                            mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sBreakpointName))
                            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                        End If
                    Next
                End If
            End If

            mActiveTextEditor.Refresh()
            g_mFormMain.PrintInformation("[INFO]", "A Breakpoint has been added!")
        End Sub

        ''' <summary>
        ''' Removes one breakpoint using the caret position in the text editor
        ''' </summary>
        Public Sub TextEditorRemoveBreakpointAtCaret()
            Dim mActiveTextEditor As TextEditorControlEx = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor
            Dim sCaretWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True, False, False)

            If (sCaretWord <> ClassDebuggerParser.g_sBreakpointName) Then
                g_mFormMain.PrintInformation("[ERROR]", "This is not a valid breakpoint!")
                Return
            End If

            Dim iCaretOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset

            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

            Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
            debuggerParser.UpdateBreakpoints(mActiveTextEditor.Document.TextContent, False, g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

            Dim lRemovedBreakpoints As New List(Of Integer)

            For i = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                Dim iIndex As Integer = debuggerParser.g_lBreakpointList(i).iOffset
                Dim iLength As Integer = debuggerParser.g_lBreakpointList(i).iLength
                Dim iTotalLength As Integer = debuggerParser.g_lBreakpointList(i).iTotalLength
                Dim iLine As Integer = debuggerParser.g_lBreakpointList(i).iLine
                Dim sFullFunction As String = debuggerParser.g_lBreakpointList(i).sArguments

                If (iIndex > iCaretOffset OrElse (iIndex + iLength) < iCaretOffset) Then
                    Continue For
                End If

                mActiveTextEditor.Document.Replace(iIndex, iTotalLength, sFullFunction)
                If (mActiveTextEditor.Document.TextLength > iIndex AndAlso mActiveTextEditor.Document.TextContent(iIndex) = ";"c) Then
                    mActiveTextEditor.Document.Remove(iIndex, 1)
                End If

                lRemovedBreakpoints.Add(iLine)

                Exit For
            Next

            lRemovedBreakpoints.Reverse()
            For Each i As Integer In lRemovedBreakpoints
                g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Breakpoint removed at line: {0}", i))

            Next

            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

            mActiveTextEditor.Refresh()
        End Sub

        ''' <summary>
        ''' Removes all available breakpoints in the text editor
        ''' </summary>
        Public Sub TextEditorRemoveAllBreakpoints()
            Dim mActiveTextEditor As TextEditorControlEx = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor
            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

            g_mFormMain.PrintInformation("[INFO]", "Removing all debugger breakpoints...")

            Dim lRemovedBreakpoints As New List(Of Integer)

            Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
            While True
                debuggerParser.UpdateBreakpoints(mActiveTextEditor.Document.TextContent, False, g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                For i = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = debuggerParser.g_lBreakpointList(i).iOffset
                    Dim iLength As Integer = debuggerParser.g_lBreakpointList(i).iLength
                    Dim iTotalLength As Integer = debuggerParser.g_lBreakpointList(i).iTotalLength
                    Dim iLine As Integer = debuggerParser.g_lBreakpointList(i).iLine
                    Dim sFullFunction As String = debuggerParser.g_lBreakpointList(i).sArguments

                    mActiveTextEditor.Document.Replace(iIndex, iTotalLength, sFullFunction)
                    If (mActiveTextEditor.Document.TextLength > iIndex AndAlso mActiveTextEditor.Document.TextContent(iIndex) = ";"c) Then
                        mActiveTextEditor.Document.Remove(iIndex, 1)
                    End If

                    lRemovedBreakpoints.Add(iLine)

                    Dim bDoRebuild As Boolean = False
                    For j = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                        If (i = j) Then
                            Continue For
                        End If

                        Dim jIndex As Integer = debuggerParser.g_lBreakpointList(j).iOffset
                        Dim jTotalLength As Integer = debuggerParser.g_lBreakpointList(j).iTotalLength
                        If (iIndex < jIndex OrElse iIndex > (jIndex + jTotalLength)) Then
                            Continue For
                        End If

                        bDoRebuild = True
                        Exit For
                    Next

                    If (bDoRebuild) Then
                        Continue While
                    Else
                        Continue For
                    End If
                Next

                Exit While
            End While

            lRemovedBreakpoints.Reverse()
            For Each i As Integer In lRemovedBreakpoints
                g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Breakpoint removed at line: {0}", i))
            Next

            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

            mActiveTextEditor.Refresh()
            g_mFormMain.PrintInformation("[INFO]", "All debugger breakpoints removed!")
        End Sub

        ''' <summary>
        ''' Removes all available breakpoints in the source
        ''' </summary>
        ''' <param name="sSource"></param>
        Public Sub RemoveAllBreakpoints(ByRef sSource As String, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
            Dim SB As New StringBuilder(sSource)

            Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
            While True
                debuggerParser.UpdateBreakpoints(SB.ToString, False, iLanguage)

                For i = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = debuggerParser.g_lBreakpointList(i).iOffset
                    Dim iTotalLength As Integer = debuggerParser.g_lBreakpointList(i).iTotalLength
                    Dim sFullFunction As String = debuggerParser.g_lBreakpointList(i).sArguments

                    SB.Remove(iIndex, iTotalLength)
                    SB.Insert(iIndex, sFullFunction)
                    If (SB.Length > iIndex AndAlso SB.Chars(iIndex) = ";"c) Then
                        SB.Remove(iIndex, 1)
                    End If

                    Dim bDoRebuild As Boolean = False
                    For j = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                        If (i = j) Then
                            Continue For
                        End If

                        Dim jIndex As Integer = debuggerParser.g_lBreakpointList(j).iOffset
                        Dim jTotalLength As Integer = debuggerParser.g_lBreakpointList(j).iTotalLength
                        If (iIndex < jIndex OrElse iIndex > (jIndex + jTotalLength)) Then
                            Continue For
                        End If

                        bDoRebuild = True
                        Exit For
                    Next

                    If (bDoRebuild) Then
                        Continue While
                    Else
                        Continue For
                    End If
                Next

                Exit While
            End While

            sSource = SB.ToString
        End Sub

        Public Function GenerateModuleCode(sFunctionName As String, sIndentifierGUID As String, bNewSyntax As Boolean) As String
            Dim SB As New StringBuilder

            If (bNewSyntax) Then
                SB.AppendLine(My.Resources.Debugger_BreakpointModuleNew)
            Else
                SB.AppendLine(My.Resources.Debugger_BreakpointModuleOld)
            End If

            SB.Replace("{FunctionName}", sFunctionName)
            SB.Replace("{IndentifierGUID}", sIndentifierGUID)

            Return SB.ToString
        End Function

        Public Sub CompilerReady(ByRef sSource As String, debuggerParser As ClassDebuggerParser, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
            Dim SB As New StringBuilder(sSource)
            Dim SBModules As New StringBuilder()

            Dim bForceNewSyntax As Boolean = (g_mFormMain.g_ClassSyntaxTools.HasNewDeclsPragma(sSource, iLanguage) <> -1)

            For i = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                Dim iIndex As Integer = debuggerParser.g_lBreakpointList(i).iOffset
                Dim iLength As Integer = debuggerParser.g_lBreakpointList(i).iLength
                Dim sGUID As String = debuggerParser.g_lBreakpointList(i).sGUID
                Dim sNewName As String = g_sBreakpointName & sGUID.Replace("-", "")

                SB.Remove(iIndex, iLength)
                SB.Insert(iIndex, sNewName)

                SBModules.AppendLine(GenerateModuleCode(sNewName, sGUID, bForceNewSyntax))
            Next

            SB.AppendLine()
            SB.AppendLine(SBModules.ToString)

            sSource = SB.ToString
        End Sub
    End Class

    Class ClassWatchers
        Private g_mFormMain As FormMain

        Public Sub New(f As FormMain)
            g_mFormMain = f
        End Sub

        ''' <summary>
        ''' Inserts one watcher using the caret position in the text editor
        ''' </summary>
        Public Sub TextEditorInsertWatcherAtCaret()
            Dim mActiveTextEditor As TextEditorControlEx = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor

            If (mActiveTextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected AndAlso mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0) Then
                Dim iOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Offset
                Dim iLength As Integer = mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Length

                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset + iLength, ")")
                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}(", ClassDebuggerParser.g_sWatcherName))
                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
            Else
                Dim sCaretWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True, False, False)

                If (String.IsNullOrEmpty(sCaretWord)) Then
                    Dim iOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.Caret.Offset
                    mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}();", ClassDebuggerParser.g_sWatcherName))
                Else
                    Dim iOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.Caret.Offset

                    For Each m As Match In Regex.Matches(mActiveTextEditor.ActiveTextAreaControl.Document.TextContent, String.Format("(?<Word>\b{0}\b)((?<Function>\s*\()|(?<Array>\s*\[)|)", Regex.Escape(sCaretWord)))
                        Dim iStartOffset As Integer = m.Groups("Word").Index
                        Dim iStartLen As Integer = m.Groups("Word").Value.Length
                        Dim bIsFunction As Boolean = m.Groups("Function").Success
                        Dim bIsArray As Boolean = m.Groups("Array").Success

                        If (iOffset < iStartOffset OrElse iOffset > (iStartOffset + iStartLen)) Then
                            Continue For
                        End If

                        If (bIsArray) Then
                            Dim sSource As String = mActiveTextEditor.ActiveTextAreaControl.Document.TextContent
                            Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                            Dim iFullLength As Integer = 0
                            Dim iStartLevel As Integer = mSourceAnalysis.GetBracketLevel(iStartOffset, Nothing)
                            For i = iStartOffset To sSource.Length - 1
                                iFullLength += 1

                                Dim iBracketRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                                If (iStartLevel + 1 = mSourceAnalysis.GetBracketLevel(i, iBracketRange) AndAlso
                                            iBracketRange = ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END) Then
                                    Exit For
                                End If
                            Next

                            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                            mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset + iFullLength, ")")
                            mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sWatcherName))
                            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                        ElseIf (bIsFunction) Then
                            Dim sSource As String = mActiveTextEditor.ActiveTextAreaControl.Document.TextContent
                            Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                            Dim iFullLength As Integer = 0
                            Dim iStartLevel As Integer = mSourceAnalysis.GetParenthesisLevel(iStartOffset, Nothing)
                            For i = iStartOffset To sSource.Length - 1
                                iFullLength += 1

                                Dim iParentRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                                If (iStartLevel + 1 = mSourceAnalysis.GetParenthesisLevel(i, iParentRange) AndAlso
                                            iParentRange = ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END) Then
                                    Exit For
                                End If
                            Next

                            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                            mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset + iFullLength, ")")
                            mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sWatcherName))
                            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                        Else
                            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                            mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset + iStartLen, ")")
                            mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sWatcherName))
                            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                        End If
                    Next
                End If
            End If

            mActiveTextEditor.Refresh()
            g_mFormMain.PrintInformation("[INFO]", "A Watcher has been added!")
        End Sub

        ''' <summary>
        ''' Removes one watcher using the caret position in the text editor
        ''' </summary>
        Public Sub TextEditorRemoveWatcherAtCaret()
            Dim mActiveTextEditor As TextEditorControlEx = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor
            Dim sCaretWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True, False, False)

            If (sCaretWord <> ClassDebuggerParser.g_sWatcherName) Then
                g_mFormMain.PrintInformation("[ERROR]", "This is not a valid watcher!")
                Return
            End If

            Dim iCaretOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset

            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

            Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
            debuggerParser.UpdateWatchers(mActiveTextEditor.Document.TextContent, False, g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

            Dim lRemovedWatchers As New List(Of Integer)

            For i = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                Dim iIndex As Integer = debuggerParser.g_lWatcherList(i).iOffset
                Dim iLength As Integer = debuggerParser.g_lWatcherList(i).iLength
                Dim iTotalLength As Integer = debuggerParser.g_lWatcherList(i).iTotalLength
                Dim iLine As Integer = debuggerParser.g_lWatcherList(i).iLine
                Dim sFullFunction As String = debuggerParser.g_lWatcherList(i).sArguments

                If (iIndex > iCaretOffset OrElse (iIndex + iLength) < iCaretOffset) Then
                    Continue For
                End If

                mActiveTextEditor.Document.Replace(iIndex, iTotalLength, sFullFunction)
                If (mActiveTextEditor.Document.TextLength > iIndex AndAlso mActiveTextEditor.Document.TextContent(iIndex) = ";"c) Then
                    mActiveTextEditor.Document.Remove(iIndex, 1)
                End If

                lRemovedWatchers.Add(iLine)

                Exit For
            Next

            lRemovedWatchers.Reverse()
            For Each i As Integer In lRemovedWatchers
                g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Watcher removed at line: {0}", i))
            Next

            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

            mActiveTextEditor.Refresh()
        End Sub

        ''' <summary>
        ''' Removes all available watchers in the text editor
        ''' </summary>
        Public Sub TextEditorRemoveAllWatchers()
            Dim mActiveTextEditor As TextEditorControlEx = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor
            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

            g_mFormMain.PrintInformation("[INFO]", "Removing all debugger watcher...")

            Dim lRemovedWatchers As New List(Of Integer)

            Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
            While True
                debuggerParser.UpdateWatchers(mActiveTextEditor.Document.TextContent, False, g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                For i = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = debuggerParser.g_lWatcherList(i).iOffset
                    Dim iLength As Integer = debuggerParser.g_lWatcherList(i).iLength
                    Dim iTotalLength As Integer = debuggerParser.g_lWatcherList(i).iTotalLength
                    Dim iLine As Integer = debuggerParser.g_lWatcherList(i).iLine
                    Dim sFullFunction As String = debuggerParser.g_lWatcherList(i).sArguments

                    mActiveTextEditor.Document.Replace(iIndex, iTotalLength, sFullFunction)
                    If (mActiveTextEditor.Document.TextLength > iIndex AndAlso mActiveTextEditor.Document.TextContent(iIndex) = ";"c) Then
                        mActiveTextEditor.Document.Remove(iIndex, 1)
                    End If

                    lRemovedWatchers.Add(iLine)

                    Dim bDoRebuild As Boolean = False
                    For j = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                        If (i = j) Then
                            Continue For
                        End If

                        Dim jIndex As Integer = debuggerParser.g_lWatcherList(j).iOffset
                        Dim jTotalLength As Integer = debuggerParser.g_lWatcherList(j).iTotalLength
                        If (iIndex < jIndex OrElse iIndex > (jIndex + jTotalLength)) Then
                            Continue For
                        End If

                        bDoRebuild = True
                        Exit For
                    Next

                    If (bDoRebuild) Then
                        Continue While
                    Else
                        Continue For
                    End If
                Next

                Exit While
            End While

            lRemovedWatchers.Reverse()
            For Each i As Integer In lRemovedWatchers
                g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Watcher removed at line: {0}", i))
            Next

            mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

            mActiveTextEditor.Refresh()
            g_mFormMain.PrintInformation("[INFO]", "All debugger watchers removed!")
        End Sub

        ''' <summary>
        ''' Removes all available watchers in the source
        ''' </summary>
        ''' <param name="sSource"></param>
        Public Sub RemoveAllWatchers(ByRef sSource As String, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
            Dim SB As New StringBuilder(sSource)

            Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
            While True
                debuggerParser.UpdateWatchers(SB.ToString, False, iLanguage)

                For i = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = debuggerParser.g_lWatcherList(i).iOffset
                    Dim iTotalLength As Integer = debuggerParser.g_lWatcherList(i).iTotalLength
                    Dim sFullFunction As String = debuggerParser.g_lWatcherList(i).sArguments

                    SB.Remove(iIndex, iTotalLength)
                    SB.Insert(iIndex, sFullFunction)
                    If (SB.Length > iIndex AndAlso SB.Chars(iIndex) = ";"c) Then
                        SB.Remove(iIndex, 1)
                    End If

                    Dim bDoRebuild As Boolean = False
                    For j = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                        If (i = j) Then
                            Continue For
                        End If

                        Dim jIndex As Integer = debuggerParser.g_lWatcherList(j).iOffset
                        Dim jTotalLength As Integer = debuggerParser.g_lWatcherList(j).iTotalLength
                        If (iIndex < jIndex OrElse iIndex > (jIndex + jTotalLength)) Then
                            Continue For
                        End If

                        bDoRebuild = True
                        Exit For
                    Next

                    If (bDoRebuild) Then
                        Continue While
                    Else
                        Continue For
                    End If
                Next

                Exit While
            End While

            sSource = SB.ToString
        End Sub

        Public Function GenerateModuleCode(sFunctionName As String, sIndentifierGUID As String, bNewSyntax As Boolean) As String
            Dim SB As New StringBuilder

            If (bNewSyntax) Then
                SB.AppendLine(My.Resources.Debugger_WatcherModuleNew)
            Else
                SB.AppendLine(My.Resources.Debugger_WatcherModuleOld)
            End If

            SB.Replace("{FunctionName}", sFunctionName)
            SB.Replace("{IndentifierGUID}", sIndentifierGUID)

            Return SB.ToString
        End Function

        Public Sub CompilerReady(ByRef sSource As String, debuggerParser As ClassDebuggerParser, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
            Dim SB As New StringBuilder(sSource)
            Dim SBModules As New StringBuilder()

            Dim bForceNewSyntax As Boolean = (g_mFormMain.g_ClassSyntaxTools.HasNewDeclsPragma(sSource, iLanguage) <> -1)

            For i = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                Dim iIndex As Integer = debuggerParser.g_lWatcherList(i).iOffset
                Dim iLength As Integer = debuggerParser.g_lWatcherList(i).iLength
                Dim sGUID As String = debuggerParser.g_lWatcherList(i).sGUID
                Dim sNewName As String = g_sWatcherName & sGUID.Replace("-", "")

                SB.Remove(iIndex, iLength)
                SB.Insert(iIndex, sNewName)

                SBModules.AppendLine(GenerateModuleCode(sNewName, sGUID, bForceNewSyntax))
            Next

            SB.AppendLine()
            SB.AppendLine(SBModules.ToString)

            sSource = SB.ToString
        End Sub
    End Class
End Class
