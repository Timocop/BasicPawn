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

Public Class ClassDebuggerParser
    Private g_mFormMain As FormMain

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
        Dim iLenght As Integer
        Dim iTotalLenght As Integer

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
    Public Sub UpdateBreakpoints(sSource As String, bKeepIdentity As Boolean)
        Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

        If (Not bKeepIdentity) Then
            g_lBreakpointList.Clear()
        End If

        Dim iListIndex As Integer = 0
        For Each m As Match In Regex.Matches(sSource, String.Format("\b{0}\b\s*(?<Lenght>)(?<Arguments>\(){1}", g_sBreakpointName, "{0,1}"))
            Dim iIndex As Integer = m.Index
            Dim bHasArgument As Boolean = m.Groups("Arguments").Success

            If (sourceAnalysis.InNonCode(iIndex) OrElse Not bHasArgument) Then
                Continue For
            End If

            Dim iArgumentIndex As Integer = m.Groups("Arguments").Index

            Dim sGUID As String = Guid.NewGuid.ToString
            Dim iLine As Integer = sSource.Substring(0, m.Index).Split(New String() {vbLf}, 0).Length
            Dim iLineIndex As Integer = 0
            For i = iIndex - 1 To 0 Step -1
                If (sSource(i) = vbLf) Then
                    Exit For
                End If

                iLineIndex += 1
            Next

            Dim iLenght As Integer = m.Groups("Lenght").Index - m.Index
            Dim iTotalLenght As Integer = 0
            Dim sArguments As New StringBuilder
            Dim sTotalFunction As New StringBuilder

            Dim iStartLevel As Integer = sourceAnalysis.GetParenthesisLevel(iIndex)
            Dim bGetArguments As Boolean = False
            For i = iIndex To sSource.Length - 1
                iTotalLenght += 1

                sTotalFunction.Append(sSource(i))

                If (sSource(i) = ")" AndAlso iStartLevel = sourceAnalysis.GetParenthesisLevel(i)) Then
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

            If (iTotalLenght < 1) Then
                Continue For
            End If

            If (bKeepIdentity) Then
                Dim debuggerItem As New STRUC_DEBUGGER_ITEM
                debuggerItem.sGUID = g_lBreakpointList(iListIndex).sGUID
                debuggerItem.iLine = iLine
                debuggerItem.iIndex = iLineIndex
                debuggerItem.iLenght = iLenght
                debuggerItem.iTotalLenght = iTotalLenght
                debuggerItem.iOffset = iIndex
                debuggerItem.sArguments = sArguments.ToString
                debuggerItem.sTotalFunction = sTotalFunction.ToString

                g_lBreakpointList(iListIndex) = debuggerItem
            Else
                Dim debuggerItem As New STRUC_DEBUGGER_ITEM
                debuggerItem.sGUID = sGUID
                debuggerItem.iLine = iLine
                debuggerItem.iIndex = iLineIndex
                debuggerItem.iLenght = iLenght
                debuggerItem.iTotalLenght = iTotalLenght
                debuggerItem.iOffset = iIndex
                debuggerItem.sArguments = sArguments.ToString
                debuggerItem.sTotalFunction = sTotalFunction.ToString

                g_lBreakpointList.Add(debuggerItem)
            End If

            iListIndex += 1
        Next
    End Sub

    ''' <summary>
    ''' Updates the breakpoint list.
    ''' </summary>
    ''' <param name="sSource"></param>
    Public Sub UpdateWatchers(sSource As String, bKeepIdentity As Boolean)
        Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

        If (Not bKeepIdentity) Then
            g_lWatcherList.Clear()
        End If

        Dim iListIndex As Integer = 0
        For Each m As Match In Regex.Matches(sSource, String.Format("\b{0}\b\s*(?<Lenght>)(?<Arguments>\(){1}", g_sWatcherName, "{0,1}"))
            Dim iIndex As Integer = m.Index
            Dim bHasArgument As Boolean = m.Groups("Arguments").Success

            If (sourceAnalysis.InNonCode(iIndex) OrElse Not bHasArgument) Then
                Continue For
            End If

            Dim iArgumentIndex As Integer = m.Groups("Arguments").Index

            Dim sGUID As String = Guid.NewGuid.ToString
            Dim iLine As Integer = sSource.Substring(0, m.Index).Split(New String() {vbLf}, 0).Length
            Dim iLineIndex As Integer = 0
            For i = iIndex - 1 To 0 Step -1
                If (sSource(i) = vbLf) Then
                    Exit For
                End If

                iLineIndex += 1
            Next

            Dim iLenght As Integer = m.Groups("Lenght").Index - m.Index
            Dim iTotalLenght As Integer = 0
            Dim sArguments As New StringBuilder()
            Dim sTotalFunction As New StringBuilder

            Dim iStartLevel As Integer = sourceAnalysis.GetParenthesisLevel(iIndex)
            Dim bGetArguments As Boolean = False
            For i = iIndex To sSource.Length - 1
                iTotalLenght += 1

                sTotalFunction.Append(sSource(i))

                If (sSource(i) = ")" AndAlso iStartLevel = sourceAnalysis.GetParenthesisLevel(i)) Then
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

            If (iTotalLenght < 1) Then
                Continue For
            End If

            If (bKeepIdentity) Then
                Dim breakpointItem As New STRUC_DEBUGGER_ITEM
                breakpointItem.sGUID = g_lWatcherList(iListIndex).sGUID
                breakpointItem.iLine = iLine
                breakpointItem.iIndex = iLineIndex
                breakpointItem.iLenght = iLenght
                breakpointItem.iTotalLenght = iTotalLenght
                breakpointItem.iOffset = iIndex
                breakpointItem.sArguments = sArguments.ToString
                breakpointItem.sTotalFunction = sTotalFunction.ToString

                g_lWatcherList(iListIndex) = breakpointItem
            Else
                Dim breakpointItem As New STRUC_DEBUGGER_ITEM
                breakpointItem.sGUID = sGUID
                breakpointItem.iLine = iLine
                breakpointItem.iIndex = iLineIndex
                breakpointItem.iLenght = iLenght
                breakpointItem.iTotalLenght = iTotalLenght
                breakpointItem.iOffset = iIndex
                breakpointItem.sArguments = sArguments.ToString
                breakpointItem.sTotalFunction = sTotalFunction.ToString

                g_lWatcherList.Add(breakpointItem)
            End If

            iListIndex += 1
        Next
    End Sub

    ''' <summary>
    ''' Gets a list of usefull autocompletes
    ''' </summary>
    ''' <returns></returns>
    Public Function GetDebuggerAutocomplete() As STRUC_AUTOCOMPLETE()
        Dim autocompleteList As New List(Of STRUC_AUTOCOMPLETE)
        Dim autocompleteInfo As New StringBuilder

        autocompleteInfo.Length = 0
        autocompleteInfo.AppendLine("/**")
        autocompleteInfo.AppendLine("*  Pauses the plugin until manually resumed. Also shows the current position in the BasicPawn Debugger.")
        autocompleteInfo.AppendLine("*  Optionaly you can return a custom non-array value.")
        autocompleteInfo.AppendLine("*/")
        autocompleteList.Add(New STRUC_AUTOCOMPLETE With {
                             .sFile = "BasicPawn.exe",
                             .sFullFunctionName = String.Format("any:{0}(any:val=0)", g_sBreakpointName),
                             .sFunctionName = g_sBreakpointName,
                             .sInfo = autocompleteInfo.ToString,
                             .mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEBUG})

        autocompleteInfo.Length = 0
        autocompleteInfo.AppendLine("/**")
        autocompleteInfo.AppendLine("*  Prints the passed value into the BasicPawn Debugger.")
        autocompleteInfo.AppendLine("*/")
        autocompleteList.Add(New STRUC_AUTOCOMPLETE With {
                             .sFile = "BasicPawn.exe",
                             .sFullFunctionName = String.Format("any:{0}(any:val=0)", g_sWatcherName),
                             .sFunctionName = g_sWatcherName,
                             .sInfo = autocompleteInfo.ToString,
                             .mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEBUG})

        Return autocompleteList.ToArray
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
        Dim mStackTrace As STRUC_SM_EXCEPTION_STACK_TRACE()
    End Structure

    Structure STRUC_SM_FATAL_EXCEPTION
        Dim sExceptionInfo As String
        Dim sBlamingFile As String
        Dim dLogDate As Date
        Dim sMiscInformation As Object()
    End Structure

    ''' <summary>
    ''' Reads all sourcemod exceptions from a log.
    ''' </summary>
    ''' <param name="sLogLines"></param>
    ''' <returns></returns>
    Public Function ReadSourceModLogExceptions(sLogLines As String()) As STRUC_SM_EXCEPTION()
        Dim iExpectingState As Integer = 0

        Dim smException As New STRUC_SM_EXCEPTION

        Dim smExceptionsList As New List(Of STRUC_SM_EXCEPTION)
        Dim smStackTraceList As New List(Of STRUC_SM_EXCEPTION_STACK_TRACE)

        For i = 0 To sLogLines.Length - 1
            Dim mExceptionInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+ \- [0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] Exception reported\:(?<Message>.*?)$")
            If (mExceptionInfo.Success) Then
                If (iExpectingState = 3) Then
                    smException.mStackTrace = smStackTraceList.ToArray
                    smExceptionsList.Add(smException)

                    iExpectingState = 0
                End If

                smException = New STRUC_SM_EXCEPTION
                smStackTraceList = New List(Of STRUC_SM_EXCEPTION_STACK_TRACE)

                Dim sDate As String = mExceptionInfo.Groups("Date").Value
                Dim sMessage As String = mExceptionInfo.Groups("Message").Value

                Dim dDate As Date
                If (Not Date.TryParseExact(sDate.Trim, "MM/dd/yyyy - HH:mm:ss", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, dDate)) Then
                    Continue For
                End If

                smException.sExceptionInfo = sMessage.Trim
                smException.dLogDate = dDate

                iExpectingState = 1
                Continue For
            End If

            Select Case (iExpectingState)
                Case 1 'Expecting: [SM] Blaming
                    Dim mBlamingInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] Blaming\:(?<File>.*?)$")
                    If (mBlamingInfo.Success) Then
                        Dim sFile As String = mBlamingInfo.Groups("File").Value

                        smException.sBlamingFile = sFile.Trim

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

                            smStackTraceList.Add(New STRUC_SM_EXCEPTION_STACK_TRACE() With {
                                                 .iLine = iLine,
                                                 .sFileName = sFile,
                                                 .sFunctionName = sFunction,
                                                 .bNativeFault = False})

                        Case mMoreStackTraceInfo.Groups("NativeFault").Success
                            Dim sFunction As String = mMoreStackTraceInfo.Groups("Function").Value.Trim

                            smStackTraceList.Add(New STRUC_SM_EXCEPTION_STACK_TRACE() With {
                                                 .iLine = -1,
                                                 .sFileName = "",
                                                 .sFunctionName = sFunction,
                                                 .bNativeFault = True})

                        Case Else
                            smException.mStackTrace = smStackTraceList.ToArray
                            smExceptionsList.Add(smException)

                            iExpectingState = 0
                    End Select
            End Select
        Next

        Return smExceptionsList.ToArray
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
    '                                             .sFunctionName = sFunction,
    '                                             .bNativeFault = False})

    '                    Case mMoreStackTraceInfo.Groups("NativeFault").Success
    '                        Dim sFunction As String = mMoreStackTraceInfo.Groups("Function").Value.Trim

    '                        smStackTraceList.Add(New STRUC_SM_EXCEPTION_STACK_TRACE() With {
    '                                             .iLine = -1,
    '                                             .sFileName = "",
    '                                             .sFunctionName = sFunction,
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

    Public Sub CleanupDebugPlaceholder(ByRef sSource As String)
        'TODO: Add more debug placeholder
        With New ClassBreakpoints(g_mFormMain)
            .RemoveAllBreakpoints(sSource)
        End With
        With New ClassWatchers(g_mFormMain)
            .RemoveAllWatchers(sSource)
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
        Dim lNameList As New List(Of String)

        lNameList.Add(g_sBreakpointName)
        lNameList.Add(g_sWatcherName)

        Return lNameList.ToArray
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
        Public Shared g_sDebuggerRunnerCmdFileExt As String = ".cmd.bpdebug"
        Public Shared g_sDebuggerRunnerEntityFileExt As String = ".entities.bpdebug"

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
            If (g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.HasSomethingSelected AndAlso g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0) Then
                Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Offset
                Dim iLenght As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Length

                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset + iLenght, ")")
                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}(", ClassDebuggerParser.g_sBreakpointName))
                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
            Else
                Dim sCaretWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True)

                If (String.IsNullOrEmpty(sCaretWord)) Then
                    Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Caret.Offset
                    g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}();", ClassDebuggerParser.g_sBreakpointName))
                Else
                    Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Caret.Offset

                    For Each m As Match In Regex.Matches(g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.TextContent, String.Format("(?<Word>\b{0}\b)(?<Function>\s*\(){1}", Regex.Escape(sCaretWord), "{0,1}"))
                        Dim iStartOffset As Integer = m.Groups("Word").Index
                        Dim iStartLen As Integer = m.Groups("Word").Value.Length
                        Dim bIsFunction As Boolean = m.Groups("Function").Success

                        If (iOffset < iStartOffset OrElse iOffset > (iStartOffset + iStartLen)) Then
                            Continue For
                        End If

                        If (bIsFunction) Then
                            Dim sSource As String = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.TextContent
                            Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                            Dim iFullLenght As Integer = 0
                            Dim iStartLevel As Integer = sourceAnalysis.GetParenthesisLevel(iStartOffset)
                            For i = iStartOffset To sSource.Length - 1
                                iFullLenght += 1

                                If (sSource(i) = ")" AndAlso iStartLevel = sourceAnalysis.GetParenthesisLevel(i)) Then
                                    Exit For
                                End If
                            Next

                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset + iFullLenght, ")")
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sBreakpointName))
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                        Else
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset + iStartLen, ")")
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sBreakpointName))
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                        End If
                    Next
                End If
            End If

            g_mFormMain.TextEditorControl_Source.Refresh()
            g_mFormMain.PrintInformation("[INFO]", "A Breakpoint has been added!")
        End Sub

        ''' <summary>
        ''' Removes one breakpoint using the caret position in the text editor
        ''' </summary>
        Public Sub TextEditorRemoveBreakpointAtCaret()
            Dim sCaretWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True)

            If (sCaretWord <> ClassDebuggerParser.g_sBreakpointName) Then
                g_mFormMain.PrintInformation("[ERROR]", "This is not a valid breakpoint!")
                Return
            End If

            Dim iCaretOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Offset

            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

            Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
            debuggerParser.UpdateBreakpoints(g_mFormMain.TextEditorControl_Source.Document.TextContent, False)

            For i = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                Dim iIndex As Integer = debuggerParser.g_lBreakpointList(i).iOffset
                Dim iLenght As Integer = debuggerParser.g_lBreakpointList(i).iLenght
                Dim iTotalLenght As Integer = debuggerParser.g_lBreakpointList(i).iTotalLenght
                Dim iLine As Integer = debuggerParser.g_lBreakpointList(i).iLine
                Dim sFullFunction As String = debuggerParser.g_lBreakpointList(i).sArguments

                If (iIndex > iCaretOffset OrElse (iIndex + iLenght) < iCaretOffset) Then
                    Continue For
                End If

                g_mFormMain.TextEditorControl_Source.Document.Replace(iIndex, iTotalLenght, sFullFunction)
                If (g_mFormMain.TextEditorControl_Source.Document.TextLength > iIndex AndAlso g_mFormMain.TextEditorControl_Source.Document.TextContent(iIndex) = ";"c) Then
                    g_mFormMain.TextEditorControl_Source.Document.Remove(iIndex, 1)
                End If

                g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Breakpoint removed at line: {0}", iLine))

                Exit For
            Next

            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

            g_mFormMain.TextEditorControl_Source.Refresh()
        End Sub

        ''' <summary>
        ''' Removes all available breakpoints in the text editor
        ''' </summary>
        Public Sub TextEditorRemoveAllBreakpoints()
            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

            g_mFormMain.PrintInformation("[INFO]", "Removing all debugger breakpoints...")

            Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
            While True
                debuggerParser.UpdateBreakpoints(g_mFormMain.TextEditorControl_Source.Document.TextContent, False)

                For i = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = debuggerParser.g_lBreakpointList(i).iOffset
                    Dim iLenght As Integer = debuggerParser.g_lBreakpointList(i).iLenght
                    Dim iTotalLenght As Integer = debuggerParser.g_lBreakpointList(i).iTotalLenght
                    Dim iLine As Integer = debuggerParser.g_lBreakpointList(i).iLine
                    Dim sFullFunction As String = debuggerParser.g_lBreakpointList(i).sArguments

                    g_mFormMain.TextEditorControl_Source.Document.Replace(iIndex, iTotalLenght, sFullFunction)
                    If (g_mFormMain.TextEditorControl_Source.Document.TextLength > iIndex AndAlso g_mFormMain.TextEditorControl_Source.Document.TextContent(iIndex) = ";"c) Then
                        g_mFormMain.TextEditorControl_Source.Document.Remove(iIndex, 1)
                    End If

                    g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Breakpoint removed at line: {0}", iLine))

                    Dim bDoRebuild As Boolean = False
                    For j = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                        If (i = j) Then
                            Continue For
                        End If

                        Dim jIndex As Integer = debuggerParser.g_lBreakpointList(j).iOffset
                        Dim jTotalLenght As Integer = debuggerParser.g_lBreakpointList(j).iTotalLenght
                        If (iIndex < jIndex OrElse iIndex > (jIndex + jTotalLenght)) Then
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

            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

            g_mFormMain.TextEditorControl_Source.Refresh()
            g_mFormMain.PrintInformation("[INFO]", "All debugger breakpoints removed!")
        End Sub

        ''' <summary>
        ''' Removes all available breakpoints in the source
        ''' </summary>
        ''' <param name="sSource"></param>
        Public Sub RemoveAllBreakpoints(ByRef sSource As String)
            Dim SB As New StringBuilder(sSource)

            Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
            While True
                debuggerParser.UpdateBreakpoints(SB.ToString, False)

                For i = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = debuggerParser.g_lBreakpointList(i).iOffset
                    Dim iTotalLenght As Integer = debuggerParser.g_lBreakpointList(i).iTotalLenght
                    Dim sFullFunction As String = debuggerParser.g_lBreakpointList(i).sArguments

                    SB.Remove(iIndex, iTotalLenght)
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
                        Dim jTotalLenght As Integer = debuggerParser.g_lBreakpointList(j).iTotalLenght
                        If (iIndex < jIndex OrElse iIndex > (jIndex + jTotalLenght)) Then
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

        Public Sub CompilerReady(ByRef sSource As String, debuggerParser As ClassDebuggerParser)
            Dim SB As New StringBuilder(sSource)
            Dim SBModules As New StringBuilder()

            Dim bForceNewSyntax As Boolean = (g_mFormMain.g_ClassSyntaxTools.HasNewDeclsPragma(sSource) <> -1)

            For i = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                Dim iIndex As Integer = debuggerParser.g_lBreakpointList(i).iOffset
                Dim iLenght As Integer = debuggerParser.g_lBreakpointList(i).iLenght
                Dim sGUID As String = debuggerParser.g_lBreakpointList(i).sGUID
                Dim sNewName As String = g_sBreakpointName & sGUID.Replace("-", "")

                SB.Remove(iIndex, iLenght)
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
            If (g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.HasSomethingSelected AndAlso g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0) Then
                Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Offset
                Dim iLenght As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Length

                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset + iLenght, ")")
                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}(", ClassDebuggerParser.g_sWatcherName))
                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
            Else
                Dim sCaretWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True)

                If (String.IsNullOrEmpty(sCaretWord)) Then
                    Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Caret.Offset
                    g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}();", ClassDebuggerParser.g_sWatcherName))
                Else
                    Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Caret.Offset

                    For Each m As Match In Regex.Matches(g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.TextContent, String.Format("(?<Word>\b{0}\b)(?<Function>\s*\(){1}", Regex.Escape(sCaretWord), "{0,1}"))
                        Dim iStartOffset As Integer = m.Groups("Word").Index
                        Dim iStartLen As Integer = m.Groups("Word").Value.Length
                        Dim bIsFunction As Boolean = m.Groups("Function").Success

                        If (iOffset < iStartOffset OrElse iOffset > (iStartOffset + iStartLen)) Then
                            Continue For
                        End If

                        If (bIsFunction) Then
                            Dim sSource As String = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.TextContent
                            Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                            Dim iFullLenght As Integer = 0
                            Dim iStartLevel As Integer = sourceAnalysis.GetParenthesisLevel(iStartOffset)
                            For i = iStartOffset To sSource.Length - 1
                                iFullLenght += 1

                                If (sSource(i) = ")" AndAlso iStartLevel = sourceAnalysis.GetParenthesisLevel(i)) Then
                                    Exit For
                                End If
                            Next

                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset + iFullLenght, ")")
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sWatcherName))
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                        Else
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset + iStartLen, ")")
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sWatcherName))
                            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                        End If
                    Next
                End If
            End If

            g_mFormMain.TextEditorControl_Source.Refresh()
            g_mFormMain.PrintInformation("[INFO]", "A Watcher has been added!")
        End Sub

        ''' <summary>
        ''' Removes one watcher using the caret position in the text editor
        ''' </summary>
        Public Sub TextEditorRemoveWatcherAtCaret()
            Dim sCaretWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True)

            If (sCaretWord <> ClassDebuggerParser.g_sWatcherName) Then
                g_mFormMain.PrintInformation("[ERROR]", "This is not a valid watcher!")
                Return
            End If

            Dim iCaretOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Offset

            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

            Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
            debuggerParser.UpdateWatchers(g_mFormMain.TextEditorControl_Source.Document.TextContent, False)

            For i = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                Dim iIndex As Integer = debuggerParser.g_lWatcherList(i).iOffset
                Dim iLenght As Integer = debuggerParser.g_lWatcherList(i).iLenght
                Dim iTotalLenght As Integer = debuggerParser.g_lWatcherList(i).iTotalLenght
                Dim iLine As Integer = debuggerParser.g_lWatcherList(i).iLine
                Dim sFullFunction As String = debuggerParser.g_lWatcherList(i).sArguments

                If (iIndex > iCaretOffset OrElse (iIndex + iLenght) < iCaretOffset) Then
                    Continue For
                End If

                g_mFormMain.TextEditorControl_Source.Document.Replace(iIndex, iTotalLenght, sFullFunction)
                If (g_mFormMain.TextEditorControl_Source.Document.TextLength > iIndex AndAlso g_mFormMain.TextEditorControl_Source.Document.TextContent(iIndex) = ";"c) Then
                    g_mFormMain.TextEditorControl_Source.Document.Remove(iIndex, 1)
                End If

                g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Watcher removed at line: {0}", iLine))

                Exit For
            Next

            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

            g_mFormMain.TextEditorControl_Source.Refresh()
        End Sub

        ''' <summary>
        ''' Removes all available watchers in the text editor
        ''' </summary>
        Public Sub TextEditorRemoveAllWatchers()
            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()


            g_mFormMain.PrintInformation("[INFO]", "Removing all debugger watcher...")

            Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
            While True
                debuggerParser.UpdateWatchers(g_mFormMain.TextEditorControl_Source.Document.TextContent, False)

                For i = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = debuggerParser.g_lWatcherList(i).iOffset
                    Dim iLenght As Integer = debuggerParser.g_lWatcherList(i).iLenght
                    Dim iTotalLenght As Integer = debuggerParser.g_lWatcherList(i).iTotalLenght
                    Dim iLine As Integer = debuggerParser.g_lWatcherList(i).iLine
                    Dim sFullFunction As String = debuggerParser.g_lWatcherList(i).sArguments

                    g_mFormMain.TextEditorControl_Source.Document.Replace(iIndex, iTotalLenght, sFullFunction)
                    If (g_mFormMain.TextEditorControl_Source.Document.TextLength > iIndex AndAlso g_mFormMain.TextEditorControl_Source.Document.TextContent(iIndex) = ";"c) Then
                        g_mFormMain.TextEditorControl_Source.Document.Remove(iIndex, 1)
                    End If

                    g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Watcher removed at line: {0}", iLine))

                    Dim bDoRebuild As Boolean = False
                    For j = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                        If (i = j) Then
                            Continue For
                        End If

                        Dim jIndex As Integer = debuggerParser.g_lWatcherList(j).iOffset
                        Dim jTotalLenght As Integer = debuggerParser.g_lWatcherList(j).iTotalLenght
                        If (iIndex < jIndex OrElse iIndex > (jIndex + jTotalLenght)) Then
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

            g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

            g_mFormMain.TextEditorControl_Source.Refresh()
            g_mFormMain.PrintInformation("[INFO]", "All debugger watchers removed!")
        End Sub

        ''' <summary>
        ''' Removes all available watchers in the source
        ''' </summary>
        ''' <param name="sSource"></param>
        Public Sub RemoveAllWatchers(ByRef sSource As String)
            Dim SB As New StringBuilder(sSource)

            Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
            While True
                debuggerParser.UpdateWatchers(SB.ToString, False)

                For i = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = debuggerParser.g_lWatcherList(i).iOffset
                    Dim iTotalLenght As Integer = debuggerParser.g_lWatcherList(i).iTotalLenght
                    Dim sFullFunction As String = debuggerParser.g_lWatcherList(i).sArguments

                    SB.Remove(iIndex, iTotalLenght)
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
                        Dim jTotalLenght As Integer = debuggerParser.g_lWatcherList(j).iTotalLenght
                        If (iIndex < jIndex OrElse iIndex > (jIndex + jTotalLenght)) Then
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

        Public Sub CompilerReady(ByRef sSource As String, debuggerParser As ClassDebuggerParser)
            Dim SB As New StringBuilder(sSource)
            Dim SBModules As New StringBuilder()

            Dim bForceNewSyntax As Boolean = (g_mFormMain.g_ClassSyntaxTools.HasNewDeclsPragma(sSource) <> -1)

            For i = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                Dim iIndex As Integer = debuggerParser.g_lWatcherList(i).iOffset
                Dim iLenght As Integer = debuggerParser.g_lWatcherList(i).iLenght
                Dim sGUID As String = debuggerParser.g_lWatcherList(i).sGUID
                Dim sNewName As String = g_sWatcherName & sGUID.Replace("-", "")

                SB.Remove(iIndex, iLenght)
                SB.Insert(iIndex, sNewName)

                SBModules.AppendLine(GenerateModuleCode(sNewName, sGUID, bForceNewSyntax))
            Next

            SB.AppendLine()
            SB.AppendLine(SBModules.ToString)

            sSource = SB.ToString
        End Sub
    End Class
End Class
