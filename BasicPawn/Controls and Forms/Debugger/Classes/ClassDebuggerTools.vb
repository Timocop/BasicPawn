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

Public Class ClassDebuggerTools
    Private g_mFormDebugger As FormDebugger

    Public Shared ReadOnly g_sDebuggerFilesExt As String = ".bpdebug"
    Public Shared ReadOnly g_sDebuggerIdentifierExt As String = ".running" & g_sDebuggerFilesExt

    Public Shared ReadOnly g_sBreakpointName As String = "BPDBreakpoint"
    Public Shared ReadOnly g_sDebuggerBreakpointIgnoreExt As String = ".ignore" & g_sDebuggerFilesExt 'If exist, the breakpoint is disabled
    Public Shared ReadOnly g_sDebuggerBreakpointTriggerExt As String = ".trigger" & g_sDebuggerFilesExt 'If exist, BasicPawn knows this breakpoint has been triggered
    Public Shared ReadOnly g_sDebuggerBreakpointContinueExt As String = ".continue" & g_sDebuggerFilesExt 'If exist, the breakpoint will continue
    Public Shared ReadOnly g_sDebuggerBreakpointContinueVarExt As String = ".continuev" & g_sDebuggerFilesExt 'If exist, the breakpoint will continue with its custom return value

    Public Shared ReadOnly g_sWatcherName As String = "BPDWatcher"
    Public Shared ReadOnly g_sDebuggerWatcherValueExt As String = ".value" & g_sDebuggerFilesExt

    Public Shared ReadOnly g_sAssertName As String = "BPDAssert"
    Public Shared ReadOnly g_sDebuggerAssertTriggerExt As String = ".trigger" & g_sDebuggerFilesExt 'If exist, BasicPawn knows this assert has been triggered
    Public Shared ReadOnly g_sDebuggerAssertContinueExt As String = ".continue" & g_sDebuggerFilesExt 'If exist, the assert will continue
    Public Shared ReadOnly g_sDebuggerAssertContinueErrorExt As String = ".continuee" & g_sDebuggerFilesExt 'If exist, the assert will throw an error
    Public Shared ReadOnly g_sDebuggerAssertContinueFailExt As String = ".continuef" & g_sDebuggerFilesExt 'If exist, the assert will fail and stop the plugin


    Public Sub New(f As FormDebugger)
        g_mFormDebugger = f
    End Sub

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
    ''' SourceMod and BasicPawn communication.
    ''' </summary>
    Class ClassRunnerEngine
        Public g_sDebuggerRunnerGuid As String = Guid.NewGuid.ToString

        Public Shared ReadOnly g_sDebuggerRunnerCmdFileExt As String = ".cmd" & g_sDebuggerFilesExt
        Public Shared ReadOnly g_sDebuggerRunnerEntityFileExt As String = ".entities" & g_sDebuggerFilesExt
        Public Shared ReadOnly g_sDebuggerRunnerPingExt As String = ".ping" & g_sDebuggerFilesExt

        ''' <summary>
        ''' Generates a engine source which can be used to accept commands, when its running.
        ''' </summary>
        ''' <param name="bNewSyntax"></param>
        ''' <returns></returns>
        Public Function GenerateRunnerEngine(sDebuggerIdentifier As String, bNewSyntax As Boolean) As String
            Dim SB As New StringBuilder

            If (bNewSyntax) Then
                'TODO: Add new synrax engine
                SB.AppendLine(My.Resources.Debugger_CommandRunnerEngineOld)
            Else
                SB.AppendLine(My.Resources.Debugger_CommandRunnerEngineOld)
            End If

            SB.Replace("{DebuggerIdentifier}", sDebuggerIdentifier)
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

    Class ClassDebuggerHelpers
        ''' <summary>
        ''' Cleanup debugger placeholders.
        ''' </summary>
        ''' <param name="sSource"></param>
        ''' <param name="iLanguage"></param>
        Public Shared Sub CleanupDebugPlaceholder(ByRef sSource As String, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
            'TODO: Add more debug placeholder 
            Call (New ClassDebuggerEntries.ClassBreakpointEntry).RemoveAll(sSource, iLanguage)
            Call (New ClassDebuggerEntries.ClassWatcherEntry).RemoveAll(sSource, iLanguage)
            Call (New ClassDebuggerEntries.ClassAssetEntry).RemoveAll(sSource, iLanguage)
        End Sub

        ''' <summary>
        ''' Cleanup debugger placeholders.
        ''' </summary>
        ''' <param name="mFormMain"></param>
        Public Shared Sub CleanupDebugPlaceholder(mFormMain As FormMain)
            'TODO: Add more debug placeholder
            Call (New ClassDebuggerEntries.ClassBreakpointEntry).TextEditorRemoveAll(mFormMain)
            Call (New ClassDebuggerEntries.ClassWatcherEntry).TextEditorRemoveAll(mFormMain)
            Call (New ClassDebuggerEntries.ClassAssetEntry).TextEditorRemoveAll(mFormMain)
        End Sub

        ''' <summary>
        ''' Gets all available debugger placeholders.
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetDebugPlaceholderNames() As String()
            'TODO: Add more debug placeholder
            Return {
                g_sBreakpointName,
                g_sWatcherName,
                g_sAssertName
            }
        End Function

        ''' <summary>
        ''' Checks if the source has any debugger placeholders such as breakpoints.
        ''' </summary>
        ''' <param name="sSource"></param>
        ''' <returns></returns>
        Public Shared Function HasDebugPlaceholder(sSource As String) As Boolean
            For Each sName As String In GetDebugPlaceholderNames()
                If (Regex.IsMatch(sSource, String.Format("\b{0}\b\s*\(", Regex.Escape(sName)))) Then
                    Return True
                End If
            Next

            Return False
        End Function

        ''' <summary>
        ''' Gets a list of usefull debugger functions.
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetDebuggerAutocomplete() As ClassSyntaxTools.STRUC_AUTOCOMPLETE()
            Dim lAutocomplete As New List(Of ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            Dim mInfoBuilder As New StringBuilder

            mInfoBuilder.AppendLine("/**")
            mInfoBuilder.AppendLine(" * Pauses the plugin until manually resumed and shows the current position in the BasicPawn Debugger.")
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
                                                                  String.Format("any {0}(any val = 0)", g_sBreakpointName)))

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
                                                                  String.Format("any {0}(any val = 0)", g_sWatcherName)))

            mInfoBuilder.Length = 0
            mInfoBuilder.AppendLine("/**")
            mInfoBuilder.AppendLine(" * Checks for a condition; if the condition is false, it pauses the plugin until manually resumed and shows the current position in the BasicPawn Debugger.")
            mInfoBuilder.AppendLine(" *")
            mInfoBuilder.AppendLine(" * WARN: Do not use this in 'float-to-float' comparisons.")
            mInfoBuilder.AppendLine(" *       The operator will see the 'any' type as non-float and parse it incorrectly.")
            mInfoBuilder.AppendLine(" */")
            lAutocomplete.Add(New ClassSyntaxTools.STRUC_AUTOCOMPLETE(mInfoBuilder.ToString,
                                                                  "BasicPawn.exe",
                                                                  "",
                                                                  ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEBUG,
                                                                  g_sAssertName,
                                                                  g_sAssertName,
                                                                  String.Format("{0}(any val = 0, char ...)", g_sAssertName)))

            Return lAutocomplete.ToArray
        End Function

        ''' <summary>
        ''' Reads all sourcemod exceptions from a log.
        ''' </summary>
        ''' <param name="sLog"></param>
        ''' <returns></returns>
        Public Shared Function ReadSourceModLogExceptions(sLog As String) As STRUC_SM_EXCEPTION()
            Return ReadSourceModLogExceptions(sLog.Split(New String() {Environment.NewLine, vbLf}, 0))
        End Function

        Public Shared Function ReadSourceModLogExceptions(sLogLines As String()) As STRUC_SM_EXCEPTION()
            Dim iExpectingState As Integer = 0

            Dim mSMException As New STRUC_SM_EXCEPTION

            Dim lSMExceptions As New List(Of STRUC_SM_EXCEPTION)
            Dim lSMStackTraces As New List(Of STRUC_SM_EXCEPTION_STACK_TRACE)

            For i = 0 To sLogLines.Length - 1
                'Ignore empty lines
                If (sLogLines(i).Trim.Length < 1) Then
                    Continue For
                End If

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

            If (iExpectingState = 3) Then
                mSMException.mStackTraces = lSMStackTraces.ToArray
                lSMExceptions.Add(mSMException)

                iExpectingState = 0
            End If

            Return lSMExceptions.ToArray
        End Function

    End Class

    Class ClassDebuggerEntries
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
        Public g_lAssertList As New List(Of STRUC_DEBUGGER_ITEM)

        Interface IDebuggerEntry
            Sub TextEditorInsertAtCaret(mFormMain As FormMain)
            Sub TextEditorRemoveAtCaret(mFormMain As FormMain)
            Sub TextEditorRemoveAll(mFormMain As FormMain)

            Sub RemoveAll(ByRef sSource As String, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)

            Sub CompilerReady(ByRef sSource As String, sDebuggerIdentifier As String, mDebuggerEntries As ClassDebuggerTools.ClassDebuggerEntries, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)

            Function GenerateModuleCode(sDebuggerIdentifier As String, sFunctionName As String, sIndentifierGUID As String, bNewSyntax As Boolean) As String
        End Interface

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
        End Sub

        ''' <summary>
        ''' Updates the breakpoint list.
        ''' </summary>
        ''' <param name="sSource"></param>
        Public Sub UpdateAsserts(sSource As String, bKeepIdentity As Boolean, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE)
            Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, iLanguage)

            If (Not bKeepIdentity) Then
                g_lAssertList.Clear()
            End If

            Dim iListIndex As Integer = 0
            For Each mMatch As Match In Regex.Matches(sSource, String.Format("\b{0}\b\s*(?<Length>)(?<Arguments>\(){1}", g_sAssertName, "{0,1}"))
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
                    g_lAssertList(iListIndex) = New STRUC_DEBUGGER_ITEM With {
                    .sGUID = g_lAssertList(iListIndex).sGUID,
                    .iLine = iLine,
                    .iIndex = iLineIndex,
                    .iLength = iLength,
                    .iTotalLength = iTotalLength,
                    .iOffset = iIndex,
                    .sArguments = sArguments.ToString,
                    .sTotalFunction = sTotalFunction.ToString
                }
                Else
                    g_lAssertList.Add(New STRUC_DEBUGGER_ITEM With {
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
        End Sub


        Class ClassBreakpointEntry
            Implements IDebuggerEntry

            Public Sub TextEditorInsertAtCaret(mFormMain As FormMain) Implements IDebuggerEntry.TextEditorInsertAtCaret
                Dim mActiveTextEditor As TextEditorControlEx = mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor

                If (mActiveTextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected AndAlso mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0) Then
                    Dim iOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Offset
                    Dim iLength As Integer = mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Length

                    mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                    mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset + iLength, ")")
                    mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}(", ClassDebuggerTools.g_sBreakpointName))
                    mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                Else
                    Dim sCaretWord As String = mFormMain.g_ClassTextEditorTools.GetCaretWord(True, False, True)

                    If (String.IsNullOrEmpty(sCaretWord)) Then
                        Dim iOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.Caret.Offset
                        mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}();", ClassDebuggerTools.g_sBreakpointName))
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
                                Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

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
                                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerTools.g_sBreakpointName))
                                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                            ElseIf (bIsFunction) Then
                                Dim sSource As String = mActiveTextEditor.ActiveTextAreaControl.Document.TextContent
                                Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

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
                                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerTools.g_sBreakpointName))
                                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                            Else
                                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset + iStartLen, ")")
                                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerTools.g_sBreakpointName))
                                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                            End If
                        Next
                    End If
                End If

                mActiveTextEditor.Refresh()
                mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "A Breakpoint has been added!")
            End Sub

            Public Sub TextEditorRemoveAtCaret(mFormMain As FormMain) Implements IDebuggerEntry.TextEditorRemoveAtCaret
                Dim mActiveTextEditor As TextEditorControlEx = mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor
                Dim sCaretWord As String = mFormMain.g_ClassTextEditorTools.GetCaretWord(True, False, False)

                If (sCaretWord <> ClassDebuggerTools.g_sBreakpointName) Then
                    mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "This is not a valid breakpoint!")
                    Return
                End If

                Dim iCaretOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset

                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

                Dim mDebuggerEntries As New ClassDebuggerTools.ClassDebuggerEntries
                mDebuggerEntries.UpdateBreakpoints(mActiveTextEditor.Document.TextContent, False, mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                Dim lRemovedBreakpoints As New List(Of Integer)

                For i = mDebuggerEntries.g_lBreakpointList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = mDebuggerEntries.g_lBreakpointList(i).iOffset
                    Dim iLength As Integer = mDebuggerEntries.g_lBreakpointList(i).iLength
                    Dim iTotalLength As Integer = mDebuggerEntries.g_lBreakpointList(i).iTotalLength
                    Dim iLine As Integer = mDebuggerEntries.g_lBreakpointList(i).iLine
                    Dim sFullFunction As String = mDebuggerEntries.g_lBreakpointList(i).sArguments

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
                    mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("Breakpoint removed at line: {0}", i))
                Next

                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                mActiveTextEditor.Refresh()
            End Sub

            Public Sub TextEditorRemoveAll(mFormMain As FormMain) Implements IDebuggerEntry.TextEditorRemoveAll
                Dim mActiveTextEditor As TextEditorControlEx = mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor
                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

                mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Removing all debugger breakpoints...")

                Dim lRemovedBreakpoints As New List(Of Integer)

                Dim mDebuggerEntries As New ClassDebuggerTools.ClassDebuggerEntries
                While True
                    mDebuggerEntries.UpdateBreakpoints(mActiveTextEditor.Document.TextContent, False, mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                    For i = mDebuggerEntries.g_lBreakpointList.Count - 1 To 0 Step -1
                        Dim iIndex As Integer = mDebuggerEntries.g_lBreakpointList(i).iOffset
                        Dim iLength As Integer = mDebuggerEntries.g_lBreakpointList(i).iLength
                        Dim iTotalLength As Integer = mDebuggerEntries.g_lBreakpointList(i).iTotalLength
                        Dim iLine As Integer = mDebuggerEntries.g_lBreakpointList(i).iLine
                        Dim sFullFunction As String = mDebuggerEntries.g_lBreakpointList(i).sArguments

                        mActiveTextEditor.Document.Replace(iIndex, iTotalLength, sFullFunction)
                        If (mActiveTextEditor.Document.TextLength > iIndex AndAlso mActiveTextEditor.Document.TextContent(iIndex) = ";"c) Then
                            mActiveTextEditor.Document.Remove(iIndex, 1)
                        End If

                        lRemovedBreakpoints.Add(iLine)

                        Dim bDoRebuild As Boolean = False
                        For j = mDebuggerEntries.g_lBreakpointList.Count - 1 To 0 Step -1
                            If (i = j) Then
                                Continue For
                            End If

                            Dim jIndex As Integer = mDebuggerEntries.g_lBreakpointList(j).iOffset
                            Dim jTotalLength As Integer = mDebuggerEntries.g_lBreakpointList(j).iTotalLength
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
                    mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("Breakpoint removed at line: {0}", i))
                Next

                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                mActiveTextEditor.Refresh()
                mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "All debugger breakpoints removed!")
            End Sub

            Public Sub RemoveAll(ByRef sSource As String, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE) Implements IDebuggerEntry.RemoveAll
                Dim SB As New StringBuilder(sSource)

                Dim mDebuggerEntries As New ClassDebuggerTools.ClassDebuggerEntries
                While True
                    mDebuggerEntries.UpdateBreakpoints(SB.ToString, False, iLanguage)

                    For i = mDebuggerEntries.g_lBreakpointList.Count - 1 To 0 Step -1
                        Dim iIndex As Integer = mDebuggerEntries.g_lBreakpointList(i).iOffset
                        Dim iTotalLength As Integer = mDebuggerEntries.g_lBreakpointList(i).iTotalLength
                        Dim sFullFunction As String = mDebuggerEntries.g_lBreakpointList(i).sArguments

                        SB.Remove(iIndex, iTotalLength)
                        SB.Insert(iIndex, sFullFunction)
                        If (SB.Length > iIndex AndAlso SB.Chars(iIndex) = ";"c) Then
                            SB.Remove(iIndex, 1)
                        End If

                        Dim bDoRebuild As Boolean = False
                        For j = mDebuggerEntries.g_lBreakpointList.Count - 1 To 0 Step -1
                            If (i = j) Then
                                Continue For
                            End If

                            Dim jIndex As Integer = mDebuggerEntries.g_lBreakpointList(j).iOffset
                            Dim jTotalLength As Integer = mDebuggerEntries.g_lBreakpointList(j).iTotalLength
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

            Public Sub CompilerReady(ByRef sSource As String, sDebuggerIdentifier As String, mDebuggerEntries As ClassDebuggerTools.ClassDebuggerEntries, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE) Implements IDebuggerEntry.CompilerReady
                Dim SB As New StringBuilder(sSource)
                Dim SBModules As New StringBuilder()

                Dim bForceNewSyntax As Boolean = (ClassSyntaxTools.ClassSyntaxHelpers.HasNewDeclsPragma(sSource, iLanguage) <> -1)

                For i = mDebuggerEntries.g_lBreakpointList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = mDebuggerEntries.g_lBreakpointList(i).iOffset
                    Dim iLength As Integer = mDebuggerEntries.g_lBreakpointList(i).iLength
                    Dim sGUID As String = mDebuggerEntries.g_lBreakpointList(i).sGUID
                    Dim sNewName As String = g_sBreakpointName & sGUID.Replace("-", "")

                    SB.Remove(iIndex, iLength)
                    SB.Insert(iIndex, sNewName)

                    SBModules.AppendLine(GenerateModuleCode(sDebuggerIdentifier, sNewName, sGUID, bForceNewSyntax))
                Next

                SB.AppendLine()
                SB.AppendLine(SBModules.ToString)

                sSource = SB.ToString
            End Sub

            Public Function GenerateModuleCode(sDebuggerIdentifier As String, sFunctionName As String, sIndentifierGUID As String, bNewSyntax As Boolean) As String Implements IDebuggerEntry.GenerateModuleCode
                Dim SB As New StringBuilder

                If (bNewSyntax) Then
                    SB.AppendLine(My.Resources.Debugger_BreakpointModuleNew)
                Else
                    SB.AppendLine(My.Resources.Debugger_BreakpointModuleOld)
                End If

                SB.Replace("{DebuggerIdentifier}", sDebuggerIdentifier)
                SB.Replace("{FunctionName}", sFunctionName)
                SB.Replace("{IndentifierGUID}", sIndentifierGUID)

                Return SB.ToString
            End Function
        End Class

        Class ClassWatcherEntry
            Implements IDebuggerEntry

            Public Sub TextEditorInsertAtCaret(mFormMain As FormMain) Implements IDebuggerEntry.TextEditorInsertAtCaret
                Dim mActiveTextEditor As TextEditorControlEx = mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor

                If (mActiveTextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected AndAlso mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0) Then
                    Dim iOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Offset
                    Dim iLength As Integer = mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Length

                    mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                    mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset + iLength, ")")
                    mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}(", ClassDebuggerTools.g_sWatcherName))
                    mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                Else
                    Dim sCaretWord As String = mFormMain.g_ClassTextEditorTools.GetCaretWord(True, False, False)

                    If (String.IsNullOrEmpty(sCaretWord)) Then
                        Dim iOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.Caret.Offset
                        mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}();", ClassDebuggerTools.g_sWatcherName))
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
                                Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

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
                                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerTools.g_sWatcherName))
                                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                            ElseIf (bIsFunction) Then
                                Dim sSource As String = mActiveTextEditor.ActiveTextAreaControl.Document.TextContent
                                Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

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
                                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerTools.g_sWatcherName))
                                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                            Else
                                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset + iStartLen, ")")
                                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerTools.g_sWatcherName))
                                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                            End If
                        Next
                    End If
                End If

                mActiveTextEditor.Refresh()
                mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "A Watcher has been added!")
            End Sub

            Public Sub TextEditorRemoveAtCaret(mFormMain As FormMain) Implements IDebuggerEntry.TextEditorRemoveAtCaret
                Dim mActiveTextEditor As TextEditorControlEx = mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor
                Dim sCaretWord As String = mFormMain.g_ClassTextEditorTools.GetCaretWord(True, False, False)

                If (sCaretWord <> ClassDebuggerTools.g_sWatcherName) Then
                    mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "This is not a valid watcher!")
                    Return
                End If

                Dim iCaretOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset

                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

                Dim mDebuggerEntries As New ClassDebuggerTools.ClassDebuggerEntries
                mDebuggerEntries.UpdateWatchers(mActiveTextEditor.Document.TextContent, False, mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                Dim lRemovedWatchers As New List(Of Integer)

                For i = mDebuggerEntries.g_lWatcherList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = mDebuggerEntries.g_lWatcherList(i).iOffset
                    Dim iLength As Integer = mDebuggerEntries.g_lWatcherList(i).iLength
                    Dim iTotalLength As Integer = mDebuggerEntries.g_lWatcherList(i).iTotalLength
                    Dim iLine As Integer = mDebuggerEntries.g_lWatcherList(i).iLine
                    Dim sFullFunction As String = mDebuggerEntries.g_lWatcherList(i).sArguments

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
                    mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("Watcher removed at line: {0}", i))
                Next

                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                mActiveTextEditor.Refresh()
            End Sub

            Public Sub TextEditorRemoveAll(mFormMain As FormMain) Implements IDebuggerEntry.TextEditorRemoveAll
                Dim mActiveTextEditor As TextEditorControlEx = mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor
                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

                mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Removing all debugger watcher...")

                Dim lRemovedWatchers As New List(Of Integer)

                Dim mDebuggerEntries As New ClassDebuggerTools.ClassDebuggerEntries
                While True
                    mDebuggerEntries.UpdateWatchers(mActiveTextEditor.Document.TextContent, False, mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                    For i = mDebuggerEntries.g_lWatcherList.Count - 1 To 0 Step -1
                        Dim iIndex As Integer = mDebuggerEntries.g_lWatcherList(i).iOffset
                        Dim iLength As Integer = mDebuggerEntries.g_lWatcherList(i).iLength
                        Dim iTotalLength As Integer = mDebuggerEntries.g_lWatcherList(i).iTotalLength
                        Dim iLine As Integer = mDebuggerEntries.g_lWatcherList(i).iLine
                        Dim sFullFunction As String = mDebuggerEntries.g_lWatcherList(i).sArguments

                        mActiveTextEditor.Document.Replace(iIndex, iTotalLength, sFullFunction)
                        If (mActiveTextEditor.Document.TextLength > iIndex AndAlso mActiveTextEditor.Document.TextContent(iIndex) = ";"c) Then
                            mActiveTextEditor.Document.Remove(iIndex, 1)
                        End If

                        lRemovedWatchers.Add(iLine)

                        Dim bDoRebuild As Boolean = False
                        For j = mDebuggerEntries.g_lWatcherList.Count - 1 To 0 Step -1
                            If (i = j) Then
                                Continue For
                            End If

                            Dim jIndex As Integer = mDebuggerEntries.g_lWatcherList(j).iOffset
                            Dim jTotalLength As Integer = mDebuggerEntries.g_lWatcherList(j).iTotalLength
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
                    mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("Watcher removed at line: {0}", i))
                Next

                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                mActiveTextEditor.Refresh()
                mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "All debugger watchers removed!")
            End Sub

            Public Sub RemoveAll(ByRef sSource As String, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE) Implements IDebuggerEntry.RemoveAll
                Dim SB As New StringBuilder(sSource)

                Dim mDebuggerEntries As New ClassDebuggerTools.ClassDebuggerEntries
                While True
                    mDebuggerEntries.UpdateWatchers(SB.ToString, False, iLanguage)

                    For i = mDebuggerEntries.g_lWatcherList.Count - 1 To 0 Step -1
                        Dim iIndex As Integer = mDebuggerEntries.g_lWatcherList(i).iOffset
                        Dim iTotalLength As Integer = mDebuggerEntries.g_lWatcherList(i).iTotalLength
                        Dim sFullFunction As String = mDebuggerEntries.g_lWatcherList(i).sArguments

                        SB.Remove(iIndex, iTotalLength)
                        SB.Insert(iIndex, sFullFunction)
                        If (SB.Length > iIndex AndAlso SB.Chars(iIndex) = ";"c) Then
                            SB.Remove(iIndex, 1)
                        End If

                        Dim bDoRebuild As Boolean = False
                        For j = mDebuggerEntries.g_lWatcherList.Count - 1 To 0 Step -1
                            If (i = j) Then
                                Continue For
                            End If

                            Dim jIndex As Integer = mDebuggerEntries.g_lWatcherList(j).iOffset
                            Dim jTotalLength As Integer = mDebuggerEntries.g_lWatcherList(j).iTotalLength
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

            Public Sub CompilerReady(ByRef sSource As String, sDebuggerIdentifier As String, mDebuggerEntries As ClassDebuggerTools.ClassDebuggerEntries, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE) Implements IDebuggerEntry.CompilerReady
                Dim SB As New StringBuilder(sSource)
                Dim SBModules As New StringBuilder()

                Dim bForceNewSyntax As Boolean = (ClassSyntaxTools.ClassSyntaxHelpers.HasNewDeclsPragma(sSource, iLanguage) <> -1)

                For i = mDebuggerEntries.g_lWatcherList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = mDebuggerEntries.g_lWatcherList(i).iOffset
                    Dim iLength As Integer = mDebuggerEntries.g_lWatcherList(i).iLength
                    Dim sGUID As String = mDebuggerEntries.g_lWatcherList(i).sGUID
                    Dim sNewName As String = g_sWatcherName & sGUID.Replace("-", "")

                    SB.Remove(iIndex, iLength)
                    SB.Insert(iIndex, sNewName)

                    SBModules.AppendLine(GenerateModuleCode(sDebuggerIdentifier, sNewName, sGUID, bForceNewSyntax))
                Next

                SB.AppendLine()
                SB.AppendLine(SBModules.ToString)

                sSource = SB.ToString
            End Sub

            Public Function GenerateModuleCode(sDebuggerIdentifier As String, sFunctionName As String, sIndentifierGUID As String, bNewSyntax As Boolean) As String Implements IDebuggerEntry.GenerateModuleCode
                Dim SB As New StringBuilder

                If (bNewSyntax) Then
                    SB.AppendLine(My.Resources.Debugger_WatcherModuleNew)
                Else
                    SB.AppendLine(My.Resources.Debugger_WatcherModuleOld)
                End If

                SB.Replace("{DebuggerIdentifier}", sDebuggerIdentifier)
                SB.Replace("{FunctionName}", sFunctionName)
                SB.Replace("{IndentifierGUID}", sIndentifierGUID)

                Return SB.ToString
            End Function
        End Class

        Class ClassAssetEntry
            Implements IDebuggerEntry

            Public Sub TextEditorInsertAtCaret(mFormMain As FormMain) Implements IDebuggerEntry.TextEditorInsertAtCaret
                Dim mActiveTextEditor As TextEditorControlEx = mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor

                If (mActiveTextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected AndAlso mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0) Then
                    Dim iOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Offset
                    Dim iLength As Integer = mActiveTextEditor.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Length

                    mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                    mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset + iLength, ")")
                    mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}(", ClassDebuggerTools.g_sAssertName))
                    mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                Else
                    Dim sCaretWord As String = mFormMain.g_ClassTextEditorTools.GetCaretWord(True, False, False)

                    If (String.IsNullOrEmpty(sCaretWord)) Then
                        Dim iOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.Caret.Offset
                        mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}();", ClassDebuggerTools.g_sAssertName))
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
                                Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

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
                                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerTools.g_sAssertName))
                                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                            ElseIf (bIsFunction) Then
                                Dim sSource As String = mActiveTextEditor.ActiveTextAreaControl.Document.TextContent
                                Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource, mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

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
                                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerTools.g_sAssertName))
                                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                            Else
                                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset + iStartLen, ")")
                                mActiveTextEditor.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerTools.g_sAssertName))
                                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                            End If
                        Next
                    End If
                End If

                mActiveTextEditor.Refresh()
                mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "A Assert has been added!")
            End Sub

            Public Sub TextEditorRemoveAtCaret(mFormMain As FormMain) Implements IDebuggerEntry.TextEditorRemoveAtCaret
                Dim mActiveTextEditor As TextEditorControlEx = mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor
                Dim sCaretWord As String = mFormMain.g_ClassTextEditorTools.GetCaretWord(True, False, False)

                If (sCaretWord <> ClassDebuggerTools.g_sAssertName) Then
                    mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, "This is not a valid assert!")
                    Return
                End If

                Dim iCaretOffset As Integer = mActiveTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset

                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

                Dim mDebuggerEntries As New ClassDebuggerTools.ClassDebuggerEntries
                mDebuggerEntries.UpdateAsserts(mActiveTextEditor.Document.TextContent, False, mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                Dim lRemovedAsserts As New List(Of Integer)

                For i = mDebuggerEntries.g_lAssertList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = mDebuggerEntries.g_lAssertList(i).iOffset
                    Dim iLength As Integer = mDebuggerEntries.g_lAssertList(i).iLength
                    Dim iTotalLength As Integer = mDebuggerEntries.g_lAssertList(i).iTotalLength
                    Dim iLine As Integer = mDebuggerEntries.g_lAssertList(i).iLine
                    Dim sFullFunction As String = mDebuggerEntries.g_lAssertList(i).sArguments

                    If (iIndex > iCaretOffset OrElse (iIndex + iLength) < iCaretOffset) Then
                        Continue For
                    End If

                    mActiveTextEditor.Document.Replace(iIndex, iTotalLength, sFullFunction)
                    If (mActiveTextEditor.Document.TextLength > iIndex AndAlso mActiveTextEditor.Document.TextContent(iIndex) = ";"c) Then
                        mActiveTextEditor.Document.Remove(iIndex, 1)
                    End If

                    lRemovedAsserts.Add(iLine)

                    Exit For
                Next

                lRemovedAsserts.Reverse()
                For Each i As Integer In lRemovedAsserts
                    mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("Assert removed at line: {0}", i))
                Next

                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                mActiveTextEditor.Refresh()
            End Sub

            Public Sub TextEditorRemoveAll(mFormMain As FormMain) Implements IDebuggerEntry.TextEditorRemoveAll
                Dim mActiveTextEditor As TextEditorControlEx = mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor
                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

                mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "Removing all debugger asserts...")

                Dim lRemovedAsserts As New List(Of Integer)

                Dim mDebuggerEntries As New ClassDebuggerTools.ClassDebuggerEntries
                While True
                    mDebuggerEntries.UpdateAsserts(mActiveTextEditor.Document.TextContent, False, mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)

                    For i = mDebuggerEntries.g_lAssertList.Count - 1 To 0 Step -1
                        Dim iIndex As Integer = mDebuggerEntries.g_lAssertList(i).iOffset
                        Dim iLength As Integer = mDebuggerEntries.g_lAssertList(i).iLength
                        Dim iTotalLength As Integer = mDebuggerEntries.g_lAssertList(i).iTotalLength
                        Dim iLine As Integer = mDebuggerEntries.g_lAssertList(i).iLine
                        Dim sFullFunction As String = mDebuggerEntries.g_lAssertList(i).sArguments

                        mActiveTextEditor.Document.Replace(iIndex, iTotalLength, sFullFunction)
                        If (mActiveTextEditor.Document.TextLength > iIndex AndAlso mActiveTextEditor.Document.TextContent(iIndex) = ";"c) Then
                            mActiveTextEditor.Document.Remove(iIndex, 1)
                        End If

                        lRemovedAsserts.Add(iLine)

                        Dim bDoRebuild As Boolean = False
                        For j = mDebuggerEntries.g_lAssertList.Count - 1 To 0 Step -1
                            If (i = j) Then
                                Continue For
                            End If

                            Dim jIndex As Integer = mDebuggerEntries.g_lAssertList(j).iOffset
                            Dim jTotalLength As Integer = mDebuggerEntries.g_lAssertList(j).iTotalLength
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

                lRemovedAsserts.Reverse()
                For Each i As Integer In lRemovedAsserts
                    mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("Assert removed at line: {0}", i))
                Next

                mActiveTextEditor.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                mActiveTextEditor.Refresh()
                mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "All debugger asserts removed!")
            End Sub

            Public Sub RemoveAll(ByRef sSource As String, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE) Implements IDebuggerEntry.RemoveAll
                Dim SB As New StringBuilder(sSource)

                Dim mDebuggerEntries As New ClassDebuggerTools.ClassDebuggerEntries
                While True
                    mDebuggerEntries.UpdateAsserts(SB.ToString, False, iLanguage)

                    For i = mDebuggerEntries.g_lAssertList.Count - 1 To 0 Step -1
                        Dim iIndex As Integer = mDebuggerEntries.g_lAssertList(i).iOffset
                        Dim iTotalLength As Integer = mDebuggerEntries.g_lAssertList(i).iTotalLength
                        Dim sFullFunction As String = mDebuggerEntries.g_lAssertList(i).sArguments

                        SB.Remove(iIndex, iTotalLength)
                        SB.Insert(iIndex, sFullFunction)
                        If (SB.Length > iIndex AndAlso SB.Chars(iIndex) = ";"c) Then
                            SB.Remove(iIndex, 1)
                        End If

                        Dim bDoRebuild As Boolean = False
                        For j = mDebuggerEntries.g_lAssertList.Count - 1 To 0 Step -1
                            If (i = j) Then
                                Continue For
                            End If

                            Dim jIndex As Integer = mDebuggerEntries.g_lAssertList(j).iOffset
                            Dim jTotalLength As Integer = mDebuggerEntries.g_lAssertList(j).iTotalLength
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

            Public Sub CompilerReady(ByRef sSource As String, sDebuggerIdentifier As String, mDebuggerEntries As ClassDebuggerTools.ClassDebuggerEntries, iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE) Implements IDebuggerEntry.CompilerReady
                Dim SB As New StringBuilder(sSource)
                Dim SBModules As New StringBuilder()

                Dim bForceNewSyntax As Boolean = (ClassSyntaxTools.ClassSyntaxHelpers.HasNewDeclsPragma(sSource, iLanguage) <> -1)

                For i = mDebuggerEntries.g_lAssertList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = mDebuggerEntries.g_lAssertList(i).iOffset
                    Dim iLength As Integer = mDebuggerEntries.g_lAssertList(i).iLength
                    Dim sGUID As String = mDebuggerEntries.g_lAssertList(i).sGUID
                    Dim sNewName As String = g_sAssertName & sGUID.Replace("-", "")

                    SB.Remove(iIndex, iLength)
                    SB.Insert(iIndex, sNewName)

                    SBModules.AppendLine(GenerateModuleCode(sDebuggerIdentifier, sNewName, sGUID, bForceNewSyntax))
                Next

                SB.AppendLine()
                SB.AppendLine(SBModules.ToString)

                sSource = SB.ToString
            End Sub

            Public Function GenerateModuleCode(sDebuggerIdentifier As String, sFunctionName As String, sIndentifierGUID As String, bNewSyntax As Boolean) As String Implements IDebuggerEntry.GenerateModuleCode
                Dim SB As New StringBuilder

                If (bNewSyntax) Then
                    SB.AppendLine(My.Resources.Debugger_AssertModuleNew)
                Else
                    SB.AppendLine(My.Resources.Debugger_AssertModuleOld)
                End If

                SB.Replace("{DebuggerIdentifier}", sDebuggerIdentifier)
                SB.Replace("{FunctionName}", sFunctionName)
                SB.Replace("{IndentifierGUID}", sIndentifierGUID)

                Return SB.ToString
            End Function
        End Class
    End Class
End Class
