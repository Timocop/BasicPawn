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
Imports ICSharpCode.TextEditor

Public Class UCAutocomplete
    Private g_mFormMain As FormMain

    Public g_ClassToolTip As ClassToolTip
    Public g_ClassActions As ClassActions
    Public g_sLastAutocompleteText As String = ""
    Private g_bControlLoaded As Boolean = False

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.  
        ClassControlStyle.SetNameFlag(Label_IntelliSense, ClassControlStyle.ENUM_STYLE_FLAGS.LABEL_ROYALBLUE)
        ClassControlStyle.SetNameFlag(Label_Autocomplete, ClassControlStyle.ENUM_STYLE_FLAGS.LABEL_ROYALBLUE)

        TextEditorControlEx_IntelliSense.SuspendLayout()
        TextEditorControlEx_Autocomplete.SuspendLayout()

        TextEditorControlEx_IntelliSense.IsReadOnly = True
        TextEditorControlEx_IntelliSense.TextEditorProperties.MouseWheelTextZoom = False
        TextEditorControlEx_IntelliSense.ActiveTextAreaControl.HScrollBar.Visible = False
        TextEditorControlEx_IntelliSense.ActiveTextAreaControl.VScrollBar.Visible = True
        TextEditorControlEx_Autocomplete.IsReadOnly = True
        TextEditorControlEx_Autocomplete.TextEditorProperties.MouseWheelTextZoom = False
        TextEditorControlEx_Autocomplete.ActiveTextAreaControl.HScrollBar.Visible = False
        TextEditorControlEx_Autocomplete.ActiveTextAreaControl.VScrollBar.Visible = True

        'Fix shitty disabled scrollbars side effects...
        If (True) Then
            Dim TextEditorLoc As Point
            Dim TextEditorRec As Rectangle

            TextEditorControlEx_IntelliSense.Dock = DockStyle.Fill
            TextEditorLoc = TextEditorControlEx_IntelliSense.Location
            TextEditorRec = TextEditorControlEx_IntelliSense.Bounds
            TextEditorControlEx_IntelliSense.Dock = DockStyle.None
            TextEditorControlEx_IntelliSense.Location = TextEditorLoc
            TextEditorControlEx_IntelliSense.Bounds = TextEditorRec
            TextEditorControlEx_IntelliSense.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

            TextEditorControlEx_Autocomplete.Dock = DockStyle.Fill
            TextEditorLoc = TextEditorControlEx_Autocomplete.Location
            TextEditorRec = TextEditorControlEx_Autocomplete.Bounds
            TextEditorControlEx_Autocomplete.Dock = DockStyle.None
            TextEditorControlEx_Autocomplete.Location = TextEditorLoc
            TextEditorControlEx_Autocomplete.Bounds = TextEditorRec
            TextEditorControlEx_Autocomplete.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

            TextEditorControlEx_IntelliSense.Height += SystemInformation.VerticalScrollBarWidth
            TextEditorControlEx_Autocomplete.Height += SystemInformation.VerticalScrollBarWidth
        End If

        TextEditorControlEx_IntelliSense.ResumeLayout()
        TextEditorControlEx_Autocomplete.ResumeLayout()

        g_ClassToolTip = New ClassToolTip(Me)
        g_ClassActions = New ClassActions(Me)

        'Set double buffering to avoid annonying flickers when collapsing/showing SplitContainer panels
        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)
    End Sub

    Private Sub UCAutocomplete_Load(sender As Object, e As EventArgs) Handles Me.Load
        g_bControlLoaded = True
    End Sub

    Public Function UpdateAutocomplete(sFunctionName As String) As Integer
        If (String.IsNullOrEmpty(sFunctionName) OrElse sFunctionName.Length < 3 OrElse Regex.IsMatch(sFunctionName, "^[0-9]+$")) Then
            ListBox_Autocomplete.Items.Clear()

            g_sLastAutocompleteText = ""
            Return 0
        End If

        Dim sFile As String = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File
        Dim bSelectedWord As Boolean = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.SelectionManager.HasSomethingSelected

        Dim sFunctionNames As String() = sFunctionName.Split("."c)
        Dim mSortedAutocomplete As New ClassAutocompleteListBox.ClassSortedAutocomplete(ListBox_Autocomplete)


        Dim iCurrentScopeIndex As Integer = -1

        'If 'this' keyword find out the type 
        If (sFunctionNames.Length = 2 AndAlso sFunctionNames(0) = "this") Then
            Dim sTextContent As String = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent
            Dim iCaretOffset As Integer = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset
            Dim iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language
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


        Dim mAutocompleteArray As ClassSyntaxTools.STRUC_AUTOCOMPLETE() = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_AutocompleteGroup.m_AutocompleteItems.ToArray
        For i = 0 To mAutocompleteArray.Length - 1
            If (mAutocompleteArray(i).m_Data.ContainsKey("EnumHidden")) Then
                Continue For
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

            If (bSelectedWord) Then
                If (mAutocompleteArray(i).m_FunctionString.Equals(sFunctionName)) Then
                    mSortedAutocomplete.Add(sFunctionName, mAutocompleteArray(i))
                End If
            Else
                If (mAutocompleteArray(i).m_Filename.Equals(sFunctionName, If(ClassSettings.g_bSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase)) OrElse
                            mAutocompleteArray(i).m_FunctionString.IndexOf(sFunctionName, If(ClassSettings.g_bSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase)) > -1) Then
                    mSortedAutocomplete.Add(sFunctionName, mAutocompleteArray(i))
                End If
            End If
        Next

        mSortedAutocomplete.PushToListBox()

        If (ListBox_Autocomplete.Items.Count > 0) Then
            ListBox_Autocomplete.SelectedIndex = 0
        End If

        Return ListBox_Autocomplete.Items.Count
    End Function

    Public Function GetSelectedItem() As ClassSyntaxTools.STRUC_AUTOCOMPLETE
        If (ListBox_Autocomplete.SelectedItems.Count < 1) Then
            Return Nothing
        End If

        Dim mAutocompleteItem = TryCast(ListBox_Autocomplete.SelectedItems(0), ClassAutocompleteListBox.ClassAutocompleteItem)
        If (mAutocompleteItem Is Nothing) Then
            Return Nothing
        End If

        Return mAutocompleteItem.m_Autocomplete
    End Function

    Private Sub ListBox_Autocomplete_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox_Autocomplete.SelectedIndexChanged
        g_ClassToolTip.UpdateToolTip()
    End Sub

    Private Sub ContextMenuStrip_Autocomplete_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Autocomplete.Opening
        Dim mAutocomplete = GetSelectedItem()

        ToolStripMenuItem_FindDefinition.Enabled = (mAutocomplete IsNot Nothing)
        ToolStripMenuItem_PeekDefinition.Enabled = (mAutocomplete IsNot Nothing)
    End Sub

    Private Sub ListBox_Autocomplete_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListBox_Autocomplete.MouseDoubleClick
        FindSelectedDefinition(False)
    End Sub

    Private Sub ToolStripMenuItem_FindDefinition_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FindDefinition.Click
        FindSelectedDefinition(False)
    End Sub

    Private Sub ToolStripMenuItem_PeekDefinition_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_PeekDefinition.Click
        FindSelectedDefinition(True)
    End Sub

    Private Sub FindSelectedDefinition(bForceNewTab As Boolean)
        Try
            Dim mAutocomplete = GetSelectedItem()
            If (mAutocomplete Is Nothing) Then
                Return
            End If

            Dim sWord As String = mAutocomplete.m_FunctionName

            Dim mDefinition As ClassTextEditorTools.STRUC_DEFINITION_ITEM = Nothing
            Select Case (g_mFormMain.g_ClassTextEditorTools.FindDefinition(mAutocomplete, mDefinition))
                Case ClassTextEditorTools.ENUM_DEFINITION_ERROR_CODE.INVALID_FILE
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'! Could not get current source file!", sWord), False, True, True)
                    Return
                Case ClassTextEditorTools.ENUM_DEFINITION_ERROR_CODE.INVALID_INPUT
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'! Nothing valid selected!", sWord), False, True, True)
                    Return
                Case ClassTextEditorTools.ENUM_DEFINITION_ERROR_CODE.NO_RESULT
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'!", sWord), False, True, True)
                    Return
            End Select

            If (mDefinition IsNot Nothing) Then
                'If not, check if file exist and search for tab
                If (IO.File.Exists(mDefinition.sFile)) Then
                    Dim mTab As ClassTabControl.ClassTab = Nothing

                    If (Not bForceNewTab) Then
                        mTab = g_mFormMain.g_ClassTabControl.GetTabByFile(mDefinition.sFile)
                    End If

                    'If that also fails, just open the file
                    If (mTab Is Nothing) Then
                        mTab = g_mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(mDefinition.sFile)
                        mTab.SelectTab()
                    End If

                    Dim iLineNum As Integer = ClassTools.ClassMath.ClampInt(mDefinition.iLine - 1, 0, mTab.m_TextEditor.Document.TotalNumberOfLines - 1)
                    Dim iLineLen As Integer = mTab.m_TextEditor.Document.GetLineSegment(iLineNum).Length

                    Dim mStartLoc As New TextLocation(0, iLineNum)
                    Dim mEndLoc As New TextLocation(iLineLen, iLineNum)

                    mTab.m_TextEditor.ActiveTextAreaControl.Caret.Position = mStartLoc
                    mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.ClearSelection()
                    mTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(mStartLoc, mEndLoc)
                    mTab.m_TextEditor.ActiveTextAreaControl.CenterViewOn(iLineNum, 10)

                    If (Not mTab.m_IsActive) Then
                        mTab.SelectTab()
                    End If

                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("Listing definitions of '{0}':", sWord))

                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_NONE, vbTab & String.Format("{0}({1}): {2}", mDefinition.sFile, mDefinition.iLine, mDefinition.mAutocomplete.m_FullFunctionString),
                                                          New UCInformationList.ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO(mDefinition.sFile, {mDefinition.iLine}))

                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, String.Format("{0} definition found!", 1), False, True, True)
                Else
                    g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'! Could not find file '{1}'!", sWord, mDefinition.sFile), False, True, True)
                End If
            Else
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Could not find definition of '{0}'!", sWord), False, True, True)
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub CleanUp()
        If (g_ClassActions IsNot Nothing) Then
            g_ClassActions.Dispose()
            g_ClassActions = Nothing
        End If
    End Sub

    Public Class ClassToolTip
        Public g_AutocompleteUC As UCAutocomplete

        Private g_sIntelliSenseFunction As String = ""

        Public Sub New(c As UCAutocomplete)
            g_AutocompleteUC = c
        End Sub

        Public Property m_IntelliSenseFunction As String
            Get
                Return g_sIntelliSenseFunction
            End Get
            Set(value As String)
                g_sIntelliSenseFunction = value
                ' UpdateToolTip()
            End Set
        End Property

        Public Sub UpdateToolTip(Optional sFunctionName As String = Nothing)
            If (Not ClassSettings.g_bSettingsEnableToolTip) Then
                g_AutocompleteUC.SplitContainer1.Panel2Collapsed = True
                Return
            End If

            If (sFunctionName IsNot Nothing) Then
                g_sIntelliSenseFunction = sFunctionName
            End If

            'Dim sTipTitle As String = ""
            Dim SB_TipText_IntelliSense As New Text.StringBuilder
            Dim SB_TipText_Autocomplete As New Text.StringBuilder
            Dim SB_TipText_IntelliSenseToolTip As New Text.StringBuilder
            Dim SB_TipText_AutocompleteToolTip As New Text.StringBuilder

            Dim iTabSize As Integer = 4

            'IntelliSense
            If (Not String.IsNullOrEmpty(g_sIntelliSenseFunction)) Then
                Dim sIntelliSenseFunction As String = g_sIntelliSenseFunction

                Dim sIntelliSenseFunctionNames As String() = sIntelliSenseFunction.Split("."c)
                Dim bHasAccessor As Boolean = (sIntelliSenseFunctionNames.Length > 1)

                Dim sFile As String = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File

                Dim iCurrentScopeIndex As Integer = -1

                ' TODO: Make this universal and outline it into a new function. Almost the same code can be found in FindDefinition().
                'If 'this' keyword find out the type 
                If (sIntelliSenseFunctionNames.Length = 2 AndAlso sIntelliSenseFunctionNames(0) = "this") Then
                    Dim sTextContent As String = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent
                    Dim iCaretOffset As Integer = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset
                    Dim iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language
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


                Dim lAlreadyShownList As New List(Of String)

                Dim bPrintedInfo As Boolean = False
                Dim iPrintedItems As Integer = 0
                Dim iMaxPrintedItems As Integer = 3
                Dim mAutocompleteArray As ClassSyntaxTools.STRUC_AUTOCOMPLETE() = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_AutocompleteGroup.m_AutocompleteItems.ToArray
                For i = 0 To mAutocompleteArray.Length - 1
                    If ((mAutocompleteArray(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0 AndAlso Not bHasAccessor) Then
                        Continue For
                    End If

                    If (mAutocompleteArray(i).m_Data.ContainsKey("EnumHidden")) Then
                        Continue For
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

                    If (Not mAutocompleteArray(i).m_FunctionString.Equals(sIntelliSenseFunction)) Then
                        Continue For
                    End If

                    If (ClassSettings.g_bSettingsUseWindowsToolTip AndAlso Not bPrintedInfo) Then
                        bPrintedInfo = True
                        SB_TipText_IntelliSenseToolTip.AppendLine("IntelliSense:")
                    End If

                    Dim sName As String = Regex.Replace(mAutocompleteArray(i).m_FullFunctionString.Trim, vbTab, New String(" "c, iTabSize))
                    Dim sNameToolTip As String = Regex.Replace(mAutocompleteArray(i).m_FullFunctionString.Trim, vbTab, New String(" "c, iTabSize))

                    If (lAlreadyShownList.Contains(sName)) Then
                        Continue For
                    End If

                    lAlreadyShownList.Add(sName)

                    If (ClassSettings.g_bSettingsUseWindowsToolTip AndAlso ClassSettings.g_bSettingsUseWindowsToolTipNewlineMethods) Then
                        Dim sNewlineDistance As Integer = sNameToolTip.IndexOf("("c)

                        If (sNewlineDistance > -1) Then
                            Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sNameToolTip, g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)
                            For ii = sNameToolTip.Length - 1 To 0 Step -1
                                If (sNameToolTip(ii) <> ","c OrElse mSourceAnalysis.m_InNonCode(ii)) Then
                                    Continue For
                                End If

                                sNameToolTip = sNameToolTip.Insert(ii + 1, Environment.NewLine & New String(" "c, sNewlineDistance))
                            Next
                        End If
                    End If

                    Dim sComment As String = Regex.Replace(mAutocompleteArray(i).m_Info.Trim, String.Format("(^|{0})", vbTab), New String(" "c, iTabSize), RegexOptions.Multiline)

                    SB_TipText_IntelliSense.AppendLine(sName)
                    If (ClassSettings.g_bSettingsToolTipMethodComments AndAlso Not String.IsNullOrEmpty(sComment.Trim)) Then
                        SB_TipText_IntelliSense.AppendLine(sComment)
                    End If
                    SB_TipText_IntelliSense.AppendLine()

                    If (iPrintedItems < iMaxPrintedItems) Then
                        SB_TipText_IntelliSenseToolTip.AppendLine(sNameToolTip)
                        If (ClassSettings.g_bSettingsToolTipMethodComments AndAlso Not String.IsNullOrEmpty(sComment.Trim)) Then
                            SB_TipText_IntelliSenseToolTip.AppendLine(sComment)
                        End If
                        SB_TipText_IntelliSenseToolTip.AppendLine()

                    ElseIf (iPrintedItems = iMaxPrintedItems) Then
                        SB_TipText_IntelliSenseToolTip.AppendLine("...")
                    End If

                    iPrintedItems += 1
                Next
            End If

            'Autocomplete
            If (g_AutocompleteUC.ListBox_Autocomplete.SelectedItems.Count > 0 AndAlso
                        TypeOf g_AutocompleteUC.ListBox_Autocomplete.SelectedItems(0) Is ClassAutocompleteListBox.ClassAutocompleteItem) Then
                Dim mAutocompleteItem = DirectCast(g_AutocompleteUC.ListBox_Autocomplete.SelectedItems(0), ClassAutocompleteListBox.ClassAutocompleteItem)

                If (ClassSettings.g_bSettingsUseWindowsToolTip) Then
                    SB_TipText_AutocompleteToolTip.AppendLine("Autocomplete:")
                End If

                Dim sName As String = Regex.Replace(mAutocompleteItem.m_Autocomplete.m_FullFunctionString.Trim, vbTab, New String(" "c, iTabSize))
                Dim sNameToolTip As String = Regex.Replace(mAutocompleteItem.m_Autocomplete.m_FullFunctionString.Trim, vbTab, New String(" "c, iTabSize))
                If (ClassSettings.g_bSettingsUseWindowsToolTip AndAlso ClassSettings.g_bSettingsUseWindowsToolTipNewlineMethods) Then
                    Dim sNewlineDistance As Integer = sNameToolTip.IndexOf("("c)

                    If (sNewlineDistance > -1) Then
                        Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sNameToolTip, g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)
                        For ii = sNameToolTip.Length - 1 To 0 Step -1
                            If (sNameToolTip(ii) <> ","c OrElse mSourceAnalysis.m_InNonCode(ii)) Then
                                Continue For
                            End If

                            sNameToolTip = sNameToolTip.Insert(ii + 1, Environment.NewLine & New String(" "c, sNewlineDistance))
                        Next
                    End If
                End If

                Dim sComment As String = Regex.Replace(mAutocompleteItem.m_Autocomplete.m_Info.Trim, String.Format("(^|{0})", vbTab), New String(" "c, iTabSize), RegexOptions.Multiline)

                SB_TipText_Autocomplete.AppendLine(sName)
                SB_TipText_AutocompleteToolTip.AppendLine(sNameToolTip)

                If (ClassSettings.g_bSettingsToolTipAutocompleteComments AndAlso Not String.IsNullOrEmpty(sComment.Trim)) Then
                    SB_TipText_Autocomplete.AppendLine(sComment)
                    SB_TipText_AutocompleteToolTip.AppendLine(sComment)
                End If
            End If

            'ToolTip
            If (True) Then
                'UpdateToolTipFormLocation()

                If (ClassSettings.g_bSettingsUseWindowsToolTip) Then
                    g_AutocompleteUC.g_mFormMain.g_mFormToolTip.m_Text = SB_TipText_IntelliSenseToolTip.ToString & SB_TipText_AutocompleteToolTip.ToString
                Else
                    If (g_AutocompleteUC.g_mFormMain.g_mFormToolTip.m_TextLength > 0) Then
                        g_AutocompleteUC.g_mFormMain.g_mFormToolTip.m_Text = ""
                    End If
                End If

                UpdateToolTipFormLocation()
            End If

            If (True) Then
                If (SB_TipText_IntelliSense.Length > 0) Then
                    g_AutocompleteUC.SplitContainer2.Panel1Collapsed = False
                    g_AutocompleteUC.TextEditorControlEx_IntelliSense.Text = SB_TipText_IntelliSense.ToString

                    g_AutocompleteUC.TextEditorControlEx_IntelliSense.InvalidateTextArea()
                Else
                    g_AutocompleteUC.SplitContainer2.Panel1Collapsed = True
                End If

                If (SB_TipText_Autocomplete.Length > 0) Then
                    g_AutocompleteUC.SplitContainer2.Panel2Collapsed = False
                    g_AutocompleteUC.TextEditorControlEx_Autocomplete.Text = SB_TipText_Autocomplete.ToString

                    g_AutocompleteUC.TextEditorControlEx_Autocomplete.InvalidateTextArea()
                Else
                    g_AutocompleteUC.SplitContainer2.Panel2Collapsed = True
                End If

                If (SB_TipText_IntelliSense.Length <> 0 OrElse SB_TipText_Autocomplete.Length <> 0) Then
                    If (g_AutocompleteUC.SplitContainer2.Orientation = Orientation.Horizontal) Then
                        g_AutocompleteUC.SplitContainer2.SplitterDistance = CInt(g_AutocompleteUC.SplitContainer2.Height / 2)
                    Else
                        g_AutocompleteUC.SplitContainer2.SplitterDistance = CInt(g_AutocompleteUC.SplitContainer2.Width / 2)
                    End If

                    g_AutocompleteUC.SplitContainer1.Panel2Collapsed = False
                Else
                    g_AutocompleteUC.SplitContainer1.Panel2Collapsed = True
                End If
            End If
        End Sub

        Public Sub UpdateToolTipFormLocation()
            If (ClassSettings.g_bSettingsUseWindowsToolTip) Then
                Dim iEditorX As Integer = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.PointToScreen(Point.Empty).X
                Dim iEditorY As Integer = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.PointToScreen(Point.Empty).Y
                Dim iEditorW As Integer = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Width
                Dim iEditorH As Integer = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Height

                Dim iX As Integer = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Caret.ScreenPosition.X + iEditorX
                Dim iY As Integer = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Caret.ScreenPosition.Y + iEditorY

                Dim iFontH As Integer = CInt(g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Font.GetHeight)
                Dim iWTabSpace As Integer = 0
                Dim iHTabSpace As Integer = 6

                Dim iNewLocation As Point
                Dim bOutsideEditor As Boolean = False

                If (ClassSettings.g_bSettingsUseWindowsToolTipDisplayTop) Then
                    Dim iHeight = g_AutocompleteUC.g_mFormMain.g_mFormToolTip.Height

                    iNewLocation = New Point(iX + iWTabSpace, iY - iHeight - iHTabSpace)

                    If (iNewLocation.X < iEditorX OrElse iNewLocation.X > (iEditorX + iEditorW)) Then
                        bOutsideEditor = True
                    End If
                    If (iNewLocation.Y + iHeight < iEditorY OrElse iNewLocation.Y + iHeight > (iEditorY + iEditorH)) Then
                        bOutsideEditor = True
                    End If
                Else
                    iNewLocation = New Point(iX + iWTabSpace, iY + iHTabSpace + iFontH)

                    If (iNewLocation.X < iEditorX OrElse iNewLocation.X > (iEditorX + iEditorW)) Then
                        bOutsideEditor = True
                    End If
                    If (iNewLocation.Y < iEditorY OrElse iNewLocation.Y > (iEditorY + iEditorH)) Then
                        bOutsideEditor = True
                    End If
                End If


                If (Not bOutsideEditor AndAlso g_AutocompleteUC.g_mFormMain.g_mFormToolTip.m_TextLength > 0) Then
                    g_AutocompleteUC.g_mFormMain.g_mFormToolTip.m_Location = iNewLocation

                    If (Not g_AutocompleteUC.g_mFormMain.g_mFormToolTip.Visible) Then
                        g_AutocompleteUC.g_mFormMain.g_mFormToolTip.Visible = True
                        g_AutocompleteUC.g_mFormMain.g_mFormToolTip.Invalidate()
                    End If
                Else
                    If (g_AutocompleteUC.g_mFormMain.g_mFormToolTip.Visible) Then
                        g_AutocompleteUC.g_mFormMain.g_mFormToolTip.Visible = False
                    End If
                End If
            Else
                If (g_AutocompleteUC.g_mFormMain.g_mFormToolTip.Visible) Then
                    g_AutocompleteUC.g_mFormMain.g_mFormToolTip.Visible = False
                End If
            End If
        End Sub
    End Class

    Class ClassActions
        Implements IDisposable

        Private g_mUCAutocomplete As UCAutocomplete

        Sub New(f As UCAutocomplete)
            g_mUCAutocomplete = f

            AddHandler g_mUCAutocomplete.g_mFormMain.g_ClassTabControl.OnTextEditorTabDetailsAction, AddressOf OnTextEditorTabDetailsAction
            AddHandler g_mUCAutocomplete.g_mFormMain.g_ClassTabControl.OnTextEditorTabDetailsMove, AddressOf OnTextEditorTabDetailsMove
        End Sub

        Public Sub OnTextEditorTabDetailsAction(mTab As ClassTabControl.ClassTab, iDetailsTabIndex As Integer, bIsSpecialAction As Boolean, iKeys As Keys)
            'Check if the tab is actualy selected, if not, return
            If (iDetailsTabIndex <> g_mUCAutocomplete.g_mFormMain.TabPage_Autocomplete.TabIndex) Then
                Return
            End If

            Try
                mTab.m_TextEditor.Document.UndoStack.StartUndoGroup()

                Dim iOffset As Integer = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset
                Dim iPosition As Integer = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                Dim iLineOffset As Integer = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Offset
                Dim iLineLen As Integer = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Length
                Dim iLineNum As Integer = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).LineNumber

                Dim sCaretFunctionMatchLeft As Match = Regex.Match(mTab.m_TextEditor.Document.GetText(iLineOffset, iPosition), "((?<Accessor>\b[a-zA-Z0-9_]+\b)(?<Type>\.|\:|\@){0,1}(?<Name>\b[a-zA-Z0-9_]+\b){0,1})$")
                Dim sCaretFunctionMatchRight As Match = Regex.Match(mTab.m_TextEditor.Document.GetText(iLineOffset + iPosition, iLineLen - iPosition), "^(\b[a-zA-Z0-9_]+\b)")

                Dim sCaretFunctionFullNameLeft As String = sCaretFunctionMatchLeft.Value
                Dim sCaretFunctionFullNameRight As String = sCaretFunctionMatchRight.Value
                Dim sCaretFunctionFullName As String = sCaretFunctionFullNameLeft & sCaretFunctionFullNameRight
                Dim sCaretFunctionAccessor As String = If(sCaretFunctionMatchLeft.Groups("Accessor").Success, sCaretFunctionMatchLeft.Groups("Accessor").Value, "")
                Dim sCaretFunctionType As String = If(sCaretFunctionMatchLeft.Groups("Type").Success, sCaretFunctionMatchLeft.Groups("Type").Value, "")
                Dim sCaretFunctionName As String = If(sCaretFunctionMatchLeft.Groups("Name").Success, sCaretFunctionMatchLeft.Groups("Name").Value, "") & sCaretFunctionMatchRight.Value

                Dim mAutocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE = g_mUCAutocomplete.g_mFormMain.g_mUCAutocomplete.GetSelectedItem()

                Select Case (sCaretFunctionType)
                    Case ":"
                        'Force fully autocomplete when using as accessor: functags, typedefs 
                        Dim sCallbackName As String = sCaretFunctionName

                        If (String.IsNullOrEmpty(sCallbackName)) Then
                            Exit Select
                        End If

                        'Reverse order so we get first funenum/typeset
                        For i = mTab.m_AutocompleteGroup.m_AutocompleteItems.Count - 1 To 0 Step -1
                            If (Not mTab.m_AutocompleteGroup.m_AutocompleteItems(i).m_FunctionName.Equals(sCaretFunctionAccessor, If(ClassSettings.g_bSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase))) Then
                                Continue For
                            End If

                            'TODO: Add better funcenum, typeset detection? Since they can have multiple sets of callbacks.
                            Select Case (True)
                                Case (mTab.m_AutocompleteGroup.m_AutocompleteItems(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) <> 0,
                                            (mTab.m_AutocompleteGroup.m_AutocompleteItems(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) <> 0,
                                             (mTab.m_AutocompleteGroup.m_AutocompleteItems(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) <> 0,
                                             (mTab.m_AutocompleteGroup.m_AutocompleteItems(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) <> 0
                                    'Remove caret function name from text editor
                                    mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition - sCaretFunctionFullNameLeft.Length
                                    mTab.m_TextEditor.Document.Remove(iOffset - sCaretFunctionFullNameLeft.Length, sCaretFunctionFullName.Length)

                                    'Re-read ceverything since text changed
                                    iOffset = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset
                                    iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    iLineOffset = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Offset
                                    iLineLen = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Length
                                    iLineNum = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).LineNumber

                                    mTab.m_TextEditor.Document.Insert(iOffset, sCallbackName)

                                    Dim sIndentation As String = ClassSyntaxTools.ClassSyntaxHelpers.BuildIndentation(1, ClassSyntaxTools.ClassSyntaxHelpers.ENUM_INDENTATION_TYPES.USE_SETTINGS)

                                    Dim sPublicFunction As String
                                    sPublicFunction = mTab.m_AutocompleteGroup.m_AutocompleteItems(i).m_FunctionString.Trim
                                    sPublicFunction = Regex.Replace(sPublicFunction, "\b" & Regex.Escape(sCaretFunctionAccessor) & "\b\s*\(", sCallbackName & "(", RegexOptions.IgnoreCase)

                                    Dim sNewInput As String
                                    With New Text.StringBuilder
                                        .AppendLine()
                                        .AppendLine(sPublicFunction)
                                        .AppendLine("{")
                                        .AppendLine(sIndentation)
                                        .AppendLine("}")
                                        sNewInput = .ToString
                                    End With

                                    mTab.m_TextEditor.Document.Insert(mTab.m_TextEditor.Document.TextLength, sNewInput)

                                    iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sCallbackName.Length

                                    Return
                            End Select
                        Next

                    Case "@"
                        'Execute command
                        Dim sCommandName As String = sCaretFunctionAccessor
                        Dim sCommandArg As String = sCaretFunctionName

                        If (String.IsNullOrEmpty(sCommandName)) Then
                            Exit Select
                        End If

                        For Each mItem In ClassTextEditorTools.ClassTestEditorCommands.m_Commands
                            If (mItem.m_Command.ToLower <> sCommandName.ToLower) Then
                                Continue For
                            End If

                            'Remove caret function name from text editor
                            mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition - sCaretFunctionFullNameLeft.Length
                            mTab.m_TextEditor.Document.Remove(iOffset - sCaretFunctionFullNameLeft.Length, sCaretFunctionFullName.Length)

                            mItem.Execute(sCommandArg)
                            Return
                        Next
                End Select

                If (mAutocomplete IsNot Nothing) Then
                    'Remove caret function name from text editor
                    mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition - sCaretFunctionFullNameLeft.Length
                    mTab.m_TextEditor.Document.Remove(iOffset - sCaretFunctionFullNameLeft.Length, sCaretFunctionFullName.Length)

                    'Re-read ceverything since text changed
                    iOffset = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset
                    iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                    iLineOffset = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Offset
                    iLineLen = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).Length
                    iLineNum = mTab.m_TextEditor.Document.GetLineSegmentForOffset(iOffset).LineNumber

                    Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(mTab.m_TextEditor.Document.TextContent, g_mUCAutocomplete.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language, True)

                    'Generate full function if the caret is out of scope, not inside a array/method and not in preprocessor.
                    Dim bGenerateFull As Boolean = True
                    Dim bGenerateSingle As Boolean = False
                    Dim bGenerateSingleBraceLoc As Integer = 0

                    If (iOffset - 1 > -1) Then
                        Dim iStateRange As ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE
                        Dim bInBrace As Boolean = (mSourceAnalysis.GetBraceLevel(iOffset - 1, iStateRange) > 0 AndAlso iStateRange <> ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END)
                        Dim bInBracket As Boolean = (mSourceAnalysis.GetBracketLevel(iOffset - 1, iStateRange) > 0 AndAlso iStateRange <> ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END)
                        Dim bInParenthesis As Boolean = (mSourceAnalysis.GetParenthesisLevel(iOffset - 1, iStateRange) > 0 AndAlso iStateRange <> ClassSyntaxTools.ClassSyntaxSourceAnalysis.ENUM_STATE_RANGE.END)
                        Dim bInPreprocessor As Boolean = mSourceAnalysis.m_InPreprocessor(iOffset - 1)

                        bGenerateFull = (Not bInBrace AndAlso Not bInBracket AndAlso Not bInParenthesis AndAlso Not bInPreprocessor)
                    End If

                    'Check for function starting brace. If found, only replace line if needed.
                    If (True) Then
                        Dim bBraceFound As Boolean = False

                        'Search backwards
                        For i = iLineOffset + iLineLen - 1 To iLineOffset Step -1
                            If (mSourceAnalysis.m_InNonCode(i)) Then
                                Continue For
                            End If

                            Dim iChar As Char = mTab.m_TextEditor.Document.GetCharAt(i)

                            If (Char.IsWhiteSpace(iChar)) Then
                                Continue For
                            End If

                            If (iChar = "{"c) Then
                                bBraceFound = True
                                bGenerateSingleBraceLoc = -1
                            End If

                            Exit For
                        Next

                        'Search forward
                        For i = iLineOffset + iLineLen To mTab.m_TextEditor.Document.TextLength - 1
                            If (mSourceAnalysis.m_InNonCode(i)) Then
                                Continue For
                            End If

                            Dim iChar As Char = mTab.m_TextEditor.Document.GetCharAt(i)

                            If (Char.IsWhiteSpace(iChar)) Then
                                Continue For
                            End If

                            If (iChar = "{"c) Then
                                bBraceFound = True
                                bGenerateSingleBraceLoc = 1
                            End If

                            Exit For
                        Next

                        bGenerateSingle = bBraceFound
                    End If

                    Select Case (True)
                        Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) <> 0
                            If (bGenerateSingle AndAlso Not bIsSpecialAction) Then
                                Dim iLineOffsetNum As Integer = mTab.m_TextEditor.Document.GetLineSegment(iLineNum).Offset
                                Dim iLineLenNum As Integer = mTab.m_TextEditor.Document.GetLineSegment(iLineNum).Length
                                mTab.m_TextEditor.Document.Remove(iLineOffsetNum, iLineLenNum)

                                Dim sIndentation As String = ClassSyntaxTools.ClassSyntaxHelpers.BuildIndentation(1, ClassSyntaxTools.ClassSyntaxHelpers.ENUM_INDENTATION_TYPES.USE_SETTINGS)

                                Dim sNewInput As String = "public " & mAutocomplete.m_FullFunctionString.Remove(0, "forward".Length).Trim & If(bGenerateSingleBraceLoc = -1, " {", "")

                                mTab.m_TextEditor.Document.Insert(iLineOffsetNum, sNewInput)

                                iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length

                            ElseIf (bGenerateFull AndAlso Not bIsSpecialAction) Then
                                Dim iLineOffsetNum As Integer = mTab.m_TextEditor.Document.GetLineSegment(iLineNum).Offset
                                Dim iLineLenNum As Integer = mTab.m_TextEditor.Document.GetLineSegment(iLineNum).Length
                                mTab.m_TextEditor.Document.Remove(iLineOffsetNum, iLineLenNum)

                                Dim sIndentation As String = ClassSyntaxTools.ClassSyntaxHelpers.BuildIndentation(1, ClassSyntaxTools.ClassSyntaxHelpers.ENUM_INDENTATION_TYPES.USE_SETTINGS)

                                Dim sNewInput As String
                                With New Text.StringBuilder
                                    .AppendLine("public " & mAutocomplete.m_FullFunctionString.Remove(0, "forward".Length).Trim)
                                    .AppendLine("{")
                                    .AppendLine(sIndentation)
                                    .AppendLine("}")
                                    sNewInput = .ToString
                                End With

                                mTab.m_TextEditor.Document.Insert(iLineOffsetNum, sNewInput)

                                iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Line = iLineNum + 2
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = sIndentation.Length
                            Else
                                mTab.m_TextEditor.Document.Insert(iOffset, mAutocomplete.m_FunctionName)

                                iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + mAutocomplete.m_FunctionName.Length
                            End If

                        Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) <> 0,
                                 (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) <> 0,
                                 (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) <> 0,
                                 (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) <> 0
                            If (bGenerateSingle AndAlso Not bIsSpecialAction) Then
                                Dim iLineOffsetNum As Integer = mTab.m_TextEditor.Document.GetLineSegment(iLineNum).Offset
                                Dim iLineLenNum As Integer = mTab.m_TextEditor.Document.GetLineSegment(iLineNum).Length
                                mTab.m_TextEditor.Document.Remove(iLineOffsetNum, iLineLenNum)

                                Dim sIndentation As String = ClassSyntaxTools.ClassSyntaxHelpers.BuildIndentation(1, ClassSyntaxTools.ClassSyntaxHelpers.ENUM_INDENTATION_TYPES.USE_SETTINGS)

                                Dim sNewInput As String = mAutocomplete.m_FunctionString.Trim & If(bGenerateSingleBraceLoc = -1, " {", "")

                                mTab.m_TextEditor.Document.Insert(iLineOffsetNum, sNewInput)

                                iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length

                            ElseIf (bGenerateFull AndAlso Not bIsSpecialAction) Then
                                Dim iLineOffsetNum As Integer = mTab.m_TextEditor.Document.GetLineSegment(iLineNum).Offset
                                Dim iLineLenNum As Integer = mTab.m_TextEditor.Document.GetLineSegment(iLineNum).Length
                                mTab.m_TextEditor.Document.Remove(iLineOffsetNum, iLineLenNum)

                                Dim sIndentation As String = ClassSyntaxTools.ClassSyntaxHelpers.BuildIndentation(1, ClassSyntaxTools.ClassSyntaxHelpers.ENUM_INDENTATION_TYPES.USE_SETTINGS)

                                Dim sNewInput As String
                                With New Text.StringBuilder
                                    .AppendLine(mAutocomplete.m_FunctionString.Trim)
                                    .AppendLine("{")
                                    .AppendLine(sIndentation)
                                    .AppendLine("}")
                                    sNewInput = .ToString
                                End With

                                mTab.m_TextEditor.Document.Insert(iLineOffsetNum, sNewInput)

                                iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Line = iLineNum + 2
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = sIndentation.Length
                            Else
                                mTab.m_TextEditor.Document.Insert(iOffset, mAutocomplete.m_FunctionName)

                                iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + mAutocomplete.m_FunctionName.Length
                            End If


                        Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE) <> 0,
                                    (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR) <> 0
                            mTab.m_TextEditor.Document.Insert(iOffset, mAutocomplete.m_FunctionString)

                            iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                            mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + mAutocomplete.m_FunctionString.Length

                        Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) <> 0,
                                    (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STRUCT) <> 0
                            If ((ClassSettings.g_bSettingsFullEnumAutocomplete AndAlso Not bIsSpecialAction) OrElse mAutocomplete.m_FunctionString.IndexOf("."c) < 0) Then
                                mTab.m_TextEditor.Document.Insert(iOffset, mAutocomplete.m_FunctionString.Replace("."c, ":"c))

                                iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + mAutocomplete.m_FunctionString.Length
                            Else
                                mTab.m_TextEditor.Document.Insert(iOffset, mAutocomplete.m_FunctionString.Remove(0, mAutocomplete.m_FunctionString.IndexOf("."c) + 1))

                                iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + mAutocomplete.m_FunctionString.Remove(0, mAutocomplete.m_FunctionString.IndexOf("."c) + 1).Length
                            End If

                        Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> 0,
                                (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM_STRUCT) <> 0,
                                (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0,
                                (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.OPERATOR) <> 0
                            If (mAutocomplete.m_FunctionString.IndexOf("."c) > -1 AndAlso sCaretFunctionFullName.IndexOf("."c) > -1 AndAlso Not bIsSpecialAction) Then
                                Dim sParenthesis As String = ""
                                If (mAutocomplete.m_FullFunctionString.Contains("("c) AndAlso mAutocomplete.m_FullFunctionString.Contains(")"c)) Then
                                    sParenthesis = "()"
                                End If

                                Dim sNewInput As String = String.Format("{0}.{1}{2}",
                                                                        sCaretFunctionFullName.Remove(sCaretFunctionFullName.LastIndexOf("."c), sCaretFunctionFullName.Length - sCaretFunctionFullName.LastIndexOf("."c)),
                                                                        mAutocomplete.m_FunctionString.Remove(0, mAutocomplete.m_FunctionString.IndexOf("."c) + 1),
                                                                        sParenthesis)
                                mTab.m_TextEditor.Document.Insert(iOffset, sNewInput)

                                iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length + If(sParenthesis.Length > 0, -1, 0)
                            Else
                                Dim sParenthesis As String = ""
                                If (mAutocomplete.m_FullFunctionString.Contains("("c) AndAlso mAutocomplete.m_FullFunctionString.Contains(")"c)) Then
                                    sParenthesis = "()"
                                End If

                                Dim sNewInput As String = String.Format("{0}{1}",
                                                                        mAutocomplete.m_FunctionString.Remove(0, mAutocomplete.m_FunctionString.IndexOf("."c) + 1),
                                                                        sParenthesis)

                                mTab.m_TextEditor.Document.Insert(iOffset, sNewInput)

                                iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length + If(sParenthesis.Length > 0, -1, 0)
                            End If

                        Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PREPROCESSOR) <> 0
                            Dim iLineOffsetNum As Integer = mTab.m_TextEditor.Document.GetLineSegment(iLineNum).Offset
                            Dim sNewInput As String = String.Format("#{0}", mAutocomplete.m_FunctionString)

                            mTab.m_TextEditor.Document.Remove(iLineOffsetNum, iPosition)
                            mTab.m_TextEditor.Document.Insert(iLineOffsetNum, sNewInput)

                            mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = sNewInput.Length

                        Case (mAutocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.COMMAND) <> 0
                            Dim sCommand As String = String.Format("{0}@", mAutocomplete.m_FunctionString)

                            mTab.m_TextEditor.Document.Insert(iOffset, sCommand)

                            iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                            mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sCommand.Length

                        Case Else
                            If (ClassSettings.g_bSettingsFullMethodAutocomplete AndAlso Not bIsSpecialAction) Then
                                Dim sNewInput As String = mAutocomplete.m_FullFunctionString.Remove(0, Regex.Match(mAutocomplete.m_FullFunctionString, "^(?<Useless>.*?)(\b[a-zA-Z0-9_]+\b)\s*(\()").Groups("Useless").Length)
                                mTab.m_TextEditor.Document.Insert(iOffset, sNewInput)

                                iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length
                            Else
                                Dim sNewInput As String = mAutocomplete.m_FunctionString.Remove(0, Regex.Match(mAutocomplete.m_FunctionString, "^(?<Useless>.*?)(\b[a-zA-Z0-9_]+\b)\s*(\()").Groups("Useless").Length)
                                mTab.m_TextEditor.Document.Insert(iOffset, String.Format("{0}()", sNewInput))

                                iPosition = mTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                mTab.m_TextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length + 1
                            End If

                    End Select
                End If
            Finally
                mTab.m_TextEditor.Document.UndoStack.EndUndoGroup()

                mTab.m_TextEditor.InvalidateTextArea()
            End Try
        End Sub

        Public Sub OnTextEditorTabDetailsMove(mTab As ClassTabControl.ClassTab, iDetailsTabIndex As Integer, iDirection As Integer, iKeys As Keys)
            'Check if the tab is actualy selected, if not, return
            If (iDetailsTabIndex <> g_mUCAutocomplete.g_mFormMain.TabPage_Autocomplete.TabIndex) Then
                Return
            End If

            If (iDirection = 0) Then
                Return
            End If

            If (g_mUCAutocomplete.ListBox_Autocomplete.SelectedItems.Count < 1) Then
                Return
            End If

            Dim iCount As Integer = g_mUCAutocomplete.ListBox_Autocomplete.Items.Count
            Dim iNewIndex As Integer = g_mUCAutocomplete.ListBox_Autocomplete.SelectedIndices(0) + iDirection

            If (iNewIndex > -1 AndAlso iNewIndex < iCount) Then
                g_mUCAutocomplete.ListBox_Autocomplete.SelectedIndex = iNewIndex
            End If
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    If (g_mUCAutocomplete.g_mFormMain.g_ClassTabControl IsNot Nothing) Then
                        RemoveHandler g_mUCAutocomplete.g_mFormMain.g_ClassTabControl.OnTextEditorTabDetailsAction, AddressOf OnTextEditorTabDetailsAction
                        RemoveHandler g_mUCAutocomplete.g_mFormMain.g_ClassTabControl.OnTextEditorTabDetailsMove, AddressOf OnTextEditorTabDetailsMove
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

End Class
