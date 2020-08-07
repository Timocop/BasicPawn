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

Public Class ClassBackgroundUpdater
    Private g_mFormMain As FormMain
    Private g_mBackgroundThread As Threading.Thread
    Private g_bResetThreadDelays As Boolean = False

    Public Event OnSyntaxUpdate(bIsFormMainFocused As Boolean, iCaretOffset As Integer, mCaretPos As Point)

    Public Sub New(f As FormMain)
        g_mFormMain = f
    End Sub

    ''' <summary>
    ''' Starts the updater thread
    ''' </summary>
    Public Sub StartThread()
        If (ClassThread.IsValid(g_mBackgroundThread)) Then
            Return
        End If

        g_mBackgroundThread = New Threading.Thread(AddressOf BackgroundThread) With {
               .Priority = Threading.ThreadPriority.Lowest,
               .IsBackground = True
           }
        g_mBackgroundThread.Start()
    End Sub

    ''' <summary>
    ''' Stops the updater thread
    ''' </summary>
    Public Sub StopThread()
        ClassThread.Abort(g_mBackgroundThread)
    End Sub

    Public Sub ResetDelays()
        g_bResetThreadDelays = True
    End Sub

    ''' <summary>
    ''' The main thread to update all kinds of stuff.
    ''' </summary>
    Private Sub BackgroundThread()
        Static mRequestSyntaxParseDelay As New TimeSpan(0, 0, 1)
        Static mFullSyntaxParseDelay As New TimeSpan(0, 0, 10)
        Static mVarSyntaxParseDelay As New TimeSpan(0, 0, 5)
        Static mObjectBrowserUpdateDelay As New TimeSpan(0, 0, 1)
        Static mFoldingUpdateDelay As New TimeSpan(0, 0, 5)
        Static mTextMinimapDelay As New TimeSpan(0, 0, 1)
        Static mTextMinimapRefreshDelay As New TimeSpan(0, 0, 10)
        Static mMarkCaretWordDelay As New TimeSpan(0, 0, 1)
        Static mScopeHighlightDelay As New TimeSpan(0, 0, 1)

        While True
            Dim dLastRequestSyntaxParseDelay As Date = (Now + mRequestSyntaxParseDelay)
            Dim dLastFullSyntaxParseDelay As Date = (Now + mFullSyntaxParseDelay)
            Dim dLastVarSyntaxParseDelay As Date = (Now + mVarSyntaxParseDelay)
            Dim dObjectBrowserUpdateDelay As Date = (Now + mObjectBrowserUpdateDelay)
            Dim dLastFoldingUpdateDelay As Date = (Now + mFoldingUpdateDelay)
            Dim dLastTextMinimapDelay As Date = (Now + mTextMinimapDelay)
            Dim dLastTextMinimapRefreshDelay As Date = (Now + mTextMinimapRefreshDelay)
            Dim dLastMarkCaretWordDelay As Date = (Now + mMarkCaretWordDelay)
            Dim dScopeHighlightDelay As Date = (Now + mScopeHighlightDelay)

            g_bResetThreadDelays = False

            While True
                If (g_bResetThreadDelays) Then
                    Exit While
                End If

                Threading.Thread.Sleep(ClassSettings.g_iSettingsThreadUpdateRate)

                Try
                    Dim bIsFormMainFocused As Boolean = (Not ClassSettings.g_bSettingsOnlyUpdateSyntaxWhenFocused OrElse ClassThread.ExecEx(Of Boolean)(g_mFormMain, Function() Form.ActiveForm IsNot Nothing))

                    Dim mActiveTab As ClassTabControl.ClassTab = ClassThread.ExecEx(Of ClassTabControl.ClassTab)(g_mFormMain, Function() g_mFormMain.g_ClassTabControl.m_ActiveTab)
                    Dim iCaretOffset As Integer = ClassThread.ExecEx(Of Integer)(g_mFormMain, Function() mActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset)
                    Dim mCaretPos As Point = ClassThread.ExecEx(Of Point)(g_mFormMain, Function() mActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.ScreenPosition)

                    'Update Autocomplete
                    If (dLastRequestSyntaxParseDelay < Now AndAlso bIsFormMainFocused AndAlso g_mFormMain.g_ClassSyntaxParser.m_UpdateRequests.Count > 0) Then
                        dLastRequestSyntaxParseDelay = (Now + mRequestSyntaxParseDelay)

                        UpdateSyntaxSchedule(mActiveTab)

                    ElseIf (dLastFullSyntaxParseDelay < Now AndAlso bIsFormMainFocused) Then
                        dLastFullSyntaxParseDelay = (Now + mFullSyntaxParseDelay)

                        ClassThread.ExecAsync(g_mFormMain, Sub()
                                                               g_mFormMain.g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL)
                                                           End Sub)
                    End If

                    'Update Object Browser
                    If (dObjectBrowserUpdateDelay < Now AndAlso bIsFormMainFocused AndAlso g_mFormMain.g_mUCObjectBrowser.m_UpdateRequests) Then
                        dObjectBrowserUpdateDelay = (Now + mObjectBrowserUpdateDelay)

                        ClassThread.ExecAsync(g_mFormMain, Sub()
                                                               g_mFormMain.g_mUCObjectBrowser.StartUpdateSchedule()
                                                           End Sub)
                    End If

                    'Update Variable Autocomplete
                    If (dLastVarSyntaxParseDelay < Now AndAlso bIsFormMainFocused) Then
                        dLastVarSyntaxParseDelay = (Now + mVarSyntaxParseDelay)

                        ClassThread.ExecAsync(g_mFormMain, Sub()
                                                               g_mFormMain.g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.VAR_PARSE)
                                                           End Sub)
                    End If

                    'Update Foldings
                    If (dLastFoldingUpdateDelay < Now AndAlso bIsFormMainFocused) Then
                        dLastFoldingUpdateDelay = (Now + mFoldingUpdateDelay)

                        ClassThread.ExecAsync(g_mFormMain, Sub()
                                                               mActiveTab.UpdateFoldings()
                                                           End Sub)
                    End If

                    'Update document minimap
                    If (dLastTextMinimapDelay < Now AndAlso bIsFormMainFocused) Then
                        dLastTextMinimapDelay = (Now + mTextMinimapDelay)

                        ClassThread.ExecAsync(g_mFormMain, Sub()
                                                               g_mFormMain.g_mUCTextMinimap.UpdateText(False)
                                                               g_mFormMain.g_mUCTextMinimap.UpdatePosition(False, True, False)
                                                           End Sub)
                    End If

                    'Update document minimap refresh
                    If (dLastTextMinimapRefreshDelay < Now AndAlso bIsFormMainFocused) Then
                        dLastTextMinimapRefreshDelay = (Now + mTextMinimapRefreshDelay)

                        ClassThread.ExecAsync(g_mFormMain, Sub()
                                                               g_mFormMain.g_mUCTextMinimap.UpdateText(True)
                                                               g_mFormMain.g_mUCTextMinimap.UpdatePosition(False, True, False)
                                                           End Sub)
                    End If

                    'Update Autocomplete
                    Static iLastAutoupdateCaretOffset As Integer = -1
                    If (iLastAutoupdateCaretOffset <> iCaretOffset) Then
                        iLastAutoupdateCaretOffset = iCaretOffset

                        UpdateAutocomplete(mActiveTab, iCaretOffset)
                    End If

                    'Update IntelliSense 
                    Static iLastIntelliSenseCaretOffset As Integer = -1
                    If (iLastIntelliSenseCaretOffset <> iCaretOffset) Then
                        iLastIntelliSenseCaretOffset = iCaretOffset

                        UpdateIntelliSense(mActiveTab, iCaretOffset)
                    End If

                    'Hide Autocomplete & IntelliSense Tooltips when scrolling 
                    Static iLastToolTipCaretPos As Point
                    If (iLastToolTipCaretPos <> mCaretPos) Then
                        iLastToolTipCaretPos = mCaretPos

                        ClassThread.ExecAsync(g_mFormMain, Sub()
                                                               g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTipFormLocation()
                                                           End Sub)
                    End If

                    'Update caret word maker
                    Static iLastAutoupdateCaretOffset3 As Integer = -1
                    If (dLastMarkCaretWordDelay < Now AndAlso iLastAutoupdateCaretOffset3 <> iCaretOffset) Then
                        iLastAutoupdateCaretOffset3 = iCaretOffset
                        dLastMarkCaretWordDelay = (Now + mMarkCaretWordDelay)

                        UpdateMarkerHighlighting(mActiveTab)
                    End If

                    'Update scope highlighter
                    Static iLastScopeHighlightCarretOffset As Integer = -1
                    If (dScopeHighlightDelay < Now AndAlso iLastScopeHighlightCarretOffset <> iCaretOffset) Then
                        iLastScopeHighlightCarretOffset = iCaretOffset
                        dScopeHighlightDelay = (Now + mScopeHighlightDelay)

                        UpdateScopeHighlighting(mActiveTab, iCaretOffset)
                    End If

                    RaiseEvent OnSyntaxUpdate(bIsFormMainFocused, iCaretOffset, mCaretPos)
                Catch ex As Threading.ThreadAbortException
                    Throw
                Catch ex As Exception
                    ClassExceptionLog.WriteToLog(ex)
                    Threading.Thread.Sleep(5000)
                End Try
            End While
        End While
    End Sub

    Private Sub UpdateSyntaxSchedule(mActiveTab As ClassTabControl.ClassTab)
        Dim iMaxUpdateCount As Integer = (ClassSettings.GetMaxParsingThreads() - g_mFormMain.g_ClassSyntaxParser.GetAliveThreadCount)
        If (iMaxUpdateCount > 0) Then
            Dim sActiveTabIdentifier As String = mActiveTab.m_Identifier
            Dim mActiveTabRequest As ClassSyntaxParser.STRUC_SYNTAX_PARSE_TAB_REQUEST = Nothing

            Dim mUpdateRequests = g_mFormMain.g_ClassSyntaxParser.m_UpdateRequests.ToArray
            Dim iRequestsLeft As Integer = iMaxUpdateCount

            For Each mRequest In mUpdateRequests
                If (mRequest.sTabIdentifier <> sActiveTabIdentifier) Then
                    Continue For
                End If

                mActiveTabRequest = mRequest
                Exit For
            Next

            'Active tabs have higher priority to update
            While (mActiveTabRequest IsNot Nothing)
                If (g_mFormMain.g_ClassSyntaxParser.IsThreadProcessing(mActiveTabRequest.sTabIdentifier)) Then
                    Exit While
                End If

                ClassThread.ExecAsync(g_mFormMain, Sub()
                                                       g_mFormMain.g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL, mActiveTabRequest.sTabIdentifier, mActiveTabRequest.iOptionFlags)
                                                   End Sub)

                iRequestsLeft -= 1

                Exit While
            End While

            For Each mRequest In mUpdateRequests
                If (iRequestsLeft < 1) Then
                    Exit For
                End If

                If (mRequest Is mActiveTabRequest) Then
                    Continue For
                End If

                If (g_mFormMain.g_ClassSyntaxParser.IsThreadProcessing(mRequest.sTabIdentifier)) Then
                    Continue For
                End If

                ClassThread.ExecAsync(g_mFormMain, Sub()
                                                       g_mFormMain.g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL, mRequest.sTabIdentifier, mRequest.iOptionFlags)
                                                   End Sub)

                iRequestsLeft -= 1
            Next
        End If
    End Sub

    Private Sub UpdateMarkerHighlighting(mActiveTab As ClassTabControl.ClassTab)
        If (Not ClassSettings.g_bSettingsAutoMark) Then
            Return
        End If

        Dim sTextContent As String = mActiveTab.m_TextEditor.Document.TextContent
        Dim sWord As String = ""

        If (Not ClassThread.ExecEx(Of Boolean)(g_mFormMain, Function() mActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)) Then
            sWord = ClassThread.ExecEx(Of String)(g_mFormMain, Function() g_mFormMain.g_ClassTextEditorTools.GetCaretWord(mActiveTab, False, False, False))
        End If

        Dim mWordLocations As New List(Of Point)
        If (Not mActiveTab.g_ClassMarkerHighlighting.FindWordLocations(sWord, sTextContent, mWordLocations)) Then
            ClassThread.ExecAsync(g_mFormMain, Sub()
                                                   mActiveTab.g_ClassMarkerHighlighting.RemoveHighlighting(ClassTabControl.ClassTab.ClassMarkerHighlighting.ENUM_MARKER_TYPE.CARET_MARKER)
                                               End Sub)
            Return
        End If

        ClassThread.ExecAsync(g_mFormMain, Sub()
                                               mActiveTab.g_ClassMarkerHighlighting.UpdateHighlighting(mWordLocations, ClassTabControl.ClassTab.ClassMarkerHighlighting.ENUM_MARKER_TYPE.CARET_MARKER)
                                           End Sub)
    End Sub

    Private Sub UpdateScopeHighlighting(mActiveTab As ClassTabControl.ClassTab, iCaretOffset As Integer)
        If (Not ClassSettings.g_bSettingsHighlightCurrentScope) Then
            Return
        End If

        Dim sTextContent As String = mActiveTab.m_TextEditor.Document.TextContent
        Dim iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = mActiveTab.m_Language
        Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sTextContent, iLanguage)

        Dim mScopeLocation As Point
        If (Not mActiveTab.g_ClassScopeHighlighting.FindScopeLocation(mSourceAnalysis, iCaretOffset, True, mScopeLocation)) Then
            ClassThread.ExecAsync(g_mFormMain, Sub()
                                                   mActiveTab.g_ClassScopeHighlighting.RemoveHighlighting()
                                               End Sub)
            Return
        End If

        ClassThread.ExecAsync(g_mFormMain, Sub()
                                               mActiveTab.g_ClassScopeHighlighting.UpdateHighlighting(mScopeLocation)
                                           End Sub)
    End Sub

    Private Sub UpdateAutocomplete(mActiveTab As ClassTabControl.ClassTab, iCaretOffset As Integer)
        Dim sTextContent As String = mActiveTab.m_TextEditor.Document.TextContent
        Dim iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = mActiveTab.m_Language
        Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sTextContent, iLanguage)

        iCaretOffset = Math.Min(iCaretOffset, sTextContent.Length - 1)

        If (iCaretOffset < 0) Then
            ClassThread.ExecAsync(g_mFormMain, Sub()
                                                   g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete("")
                                                   g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
                                               End Sub)
            Return
        End If

        If (Not mSourceAnalysis.m_InRange(iCaretOffset) OrElse
                        mSourceAnalysis.m_InString(iCaretOffset) OrElse
                        mSourceAnalysis.m_InChar(iCaretOffset) OrElse
                        mSourceAnalysis.m_InMultiComment(iCaretOffset) OrElse
                        mSourceAnalysis.m_InSingleComment(iCaretOffset)) Then
            ClassThread.ExecAsync(g_mFormMain, Sub()
                                                   g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete("")
                                                   g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
                                               End Sub)
            Return
        End If

        Dim sFunctionName As String = ClassThread.ExecEx(Of String)(g_mFormMain, Function() g_mFormMain.g_ClassTextEditorTools.GetCaretWord(mActiveTab, True, True, True))

        If (ClassThread.ExecEx(Of Integer)(g_mFormMain.g_mUCAutocomplete, Function() g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete(sFunctionName)) < 1) Then
            sFunctionName = ClassThread.ExecEx(Of String)(g_mFormMain, Function() g_mFormMain.g_ClassTextEditorTools.GetCaretWord(mActiveTab, False, False, False))

            ClassThread.ExecAsync(g_mFormMain.g_mUCAutocomplete, Sub()
                                                                     g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete(sFunctionName)
                                                                 End Sub)
        End If

        ClassThread.ExecAsync(g_mFormMain.g_mUCAutocomplete, Sub()
                                                                 g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
                                                             End Sub)
    End Sub

    Private Sub UpdateIntelliSense(mActiveTab As ClassTabControl.ClassTab, iCaretOffset As Integer)
        Dim sTextContent As String = mActiveTab.m_TextEditor.Document.TextContent
        Dim iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = mActiveTab.m_Language
        Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sTextContent, iLanguage)

        iCaretOffset = Math.Min(iCaretOffset, sTextContent.Length - 1)

        If (iCaretOffset < 0) Then
            ClassThread.ExecAsync(g_mFormMain, Sub()
                                                   g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip("")
                                               End Sub)
            Return
        End If

        If (Not mSourceAnalysis.m_InRange(iCaretOffset) OrElse
                        mSourceAnalysis.m_InMultiComment(iCaretOffset) OrElse
                        mSourceAnalysis.m_InSingleComment(iCaretOffset)) Then
            ClassThread.ExecAsync(g_mFormMain, Sub()
                                                   g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip("")
                                               End Sub)
            Return
        End If

        'Create a valid range to read the method name and for performance. 
        Dim mStringBuilder As New Text.StringBuilder
        Dim iLastParenthesisRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
        Dim iLastParenthesis As Integer = mSourceAnalysis.GetParenthesisLevel(iCaretOffset, iLastParenthesisRange)
        If (iLastParenthesisRange = ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.START) Then
            iLastParenthesis -= 1
        End If

        Dim i As Integer
        For i = iCaretOffset - 1 To 0 Step -1
            If (mSourceAnalysis.GetBraceLevel(i, Nothing) < 1 OrElse
                        mSourceAnalysis.GetParenthesisLevel(i, Nothing) < iLastParenthesis - 1) Then
                Exit For
            End If

            If (mSourceAnalysis.m_InNonCode(i)) Then
                Continue For
            End If

            If (mSourceAnalysis.GetParenthesisLevel(i, Nothing) > iLastParenthesis - 1 OrElse
                        mSourceAnalysis.GetBracketLevel(i, Nothing) > 0) Then
                Continue For
            End If

            mStringBuilder.Append(sTextContent(i))
        Next

        Dim sTmp As String = StrReverse(mStringBuilder.ToString).Trim
        Dim sMethodStart As String = Regex.Match(sTmp, "((\b[a-zA-Z0-9_]+\b)(\.){0,1}(\b[a-zA-Z0-9_]+\b){0,1})$").Value

        ClassThread.ExecAsync(g_mFormMain, Sub()
                                               g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip(sMethodStart)
                                           End Sub)
    End Sub
End Class
