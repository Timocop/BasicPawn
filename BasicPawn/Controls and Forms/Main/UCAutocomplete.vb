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


Imports System.Text.RegularExpressions

Public Class UCAutocomplete
    Private g_mFormMain As FormMain

    Public g_ClassToolTip As ClassToolTip
    Public g_sLastAutocompleteText As String = ""
    Private g_bControlLoaded As Boolean = False

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        Label_IntelliSense.Name &= "@SetForeColorRoyalBlue"
        Label_Autocomplete.Name &= "@SetForeColorRoyalBlue"

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
            Dim iBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sTextContent, "{"c, "}"c, 1, iLanguage, True)

            'Get current scope index
            For i = 0 To iBraceList.Length - 1
                If (iCaretOffset < iBraceList(i)(0) OrElse iCaretOffset > iBraceList(i)(1)) Then
                    Continue For
                End If

                iCurrentScopeIndex = i
                Exit For
            Next
        End If


        Dim mAutocompleteArray As ClassSyntaxTools.STRUC_AUTOCOMPLETE() = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_AutocompleteItems.ToArray
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
                If (mAutocompleteArray(i).m_Filename.Equals(sFunctionName, If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase)) OrElse
                            mAutocompleteArray(i).m_FunctionString.IndexOf(sFunctionName, If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase)) > -1) Then
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
            If (Not ClassSettings.g_iSettingsEnableToolTip) Then
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

                Dim bIsMethodMapEnd As Boolean = sIntelliSenseFunction.StartsWith("."c)
                If (bIsMethodMapEnd) Then
                    sIntelliSenseFunction = sIntelliSenseFunction.Remove(0, 1)
                End If

                Dim sIntelliSenseFunctionNames As String() = sIntelliSenseFunction.Split("."c)
                Dim bIsMethodMap As Boolean = sIntelliSenseFunction.Contains("."c)

                Dim sFile As String = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_File

                Dim iCurrentScopeIndex As Integer = -1

                'If 'this' keyword find out the type 
                If (sIntelliSenseFunctionNames.Length = 2 AndAlso sIntelliSenseFunctionNames(0) = "this") Then
                    Dim sTextContent As String = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent
                    Dim iCaretOffset As Integer = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset
                    Dim iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language
                    Dim iBraceList As Integer()() = g_AutocompleteUC.g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sTextContent, "{"c, "}"c, 1, iLanguage, True)

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
                Dim mAutocompleteArray As ClassSyntaxTools.STRUC_AUTOCOMPLETE() = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_AutocompleteItems.ToArray
                For i = 0 To mAutocompleteArray.Length - 1
                    If ((mAutocompleteArray(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0 AndAlso Not bIsMethodMap) Then
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

                    If (bIsMethodMap) Then
                        If (Not mAutocompleteArray(i).m_FunctionString.Equals(sIntelliSenseFunction)) Then
                            Continue For
                        End If
                    Else
                        If (Not mAutocompleteArray(i).m_FunctionString.Contains(sIntelliSenseFunction) OrElse
                                    Not Regex.IsMatch(mAutocompleteArray(i).m_FunctionString, String.Format("{0}\b{1}\b", If(bIsMethodMapEnd, "(\.)", ""), Regex.Escape(sIntelliSenseFunction)))) Then
                            Continue For
                        End If
                    End If


                    If (ClassSettings.g_iSettingsUseWindowsToolTip AndAlso Not bPrintedInfo) Then
                        bPrintedInfo = True
                        SB_TipText_IntelliSenseToolTip.AppendLine("IntelliSense:")
                    End If

                    Dim sName As String = Regex.Replace(mAutocompleteArray(i).m_FullFunctionString.Trim, vbTab, New String(" "c, iTabSize))
                    Dim sNameToolTip As String = Regex.Replace(mAutocompleteArray(i).m_FullFunctionString.Trim, vbTab, New String(" "c, iTabSize))

                    If (lAlreadyShownList.Contains(sName)) Then
                        Continue For
                    End If

                    lAlreadyShownList.Add(sName)

                    If (ClassSettings.g_iSettingsUseWindowsToolTip AndAlso ClassSettings.g_iSettingsUseWindowsToolTipNewlineMethods) Then
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
                    If (ClassSettings.g_iSettingsToolTipMethodComments AndAlso Not String.IsNullOrEmpty(sComment.Trim)) Then
                        SB_TipText_IntelliSense.AppendLine(sComment)
                    End If
                    SB_TipText_IntelliSense.AppendLine()

                    If (iPrintedItems < iMaxPrintedItems) Then
                        SB_TipText_IntelliSenseToolTip.AppendLine(sNameToolTip)
                        If (ClassSettings.g_iSettingsToolTipMethodComments AndAlso Not String.IsNullOrEmpty(sComment.Trim)) Then
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

                If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
                    SB_TipText_AutocompleteToolTip.AppendLine("Autocomplete:")
                End If

                Dim sName As String = Regex.Replace(mAutocompleteItem.m_Autocomplete.m_FullFunctionString.Trim, vbTab, New String(" "c, iTabSize))
                Dim sNameToolTip As String = Regex.Replace(mAutocompleteItem.m_Autocomplete.m_FullFunctionString.Trim, vbTab, New String(" "c, iTabSize))
                If (ClassSettings.g_iSettingsUseWindowsToolTip AndAlso ClassSettings.g_iSettingsUseWindowsToolTipNewlineMethods) Then
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

                If (ClassSettings.g_iSettingsToolTipAutocompleteComments AndAlso Not String.IsNullOrEmpty(sComment.Trim)) Then
                    SB_TipText_Autocomplete.AppendLine(sComment)
                    SB_TipText_AutocompleteToolTip.AppendLine(sComment)
                End If
            End If

            'ToolTip
            If (True) Then
                'UpdateToolTipFormLocation()

                If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
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
                    g_AutocompleteUC.TextEditorControlEx_IntelliSense.Refresh()
                Else
                    g_AutocompleteUC.SplitContainer2.Panel1Collapsed = True
                End If

                If (SB_TipText_Autocomplete.Length > 0) Then
                    g_AutocompleteUC.SplitContainer2.Panel2Collapsed = False
                    g_AutocompleteUC.TextEditorControlEx_Autocomplete.Text = SB_TipText_Autocomplete.ToString
                    g_AutocompleteUC.TextEditorControlEx_Autocomplete.Refresh()
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
            If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
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

                If (ClassSettings.g_iSettingsUseWindowsToolTipDisplayTop) Then
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
                        g_AutocompleteUC.g_mFormMain.g_mFormToolTip.Refresh()
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
End Class
