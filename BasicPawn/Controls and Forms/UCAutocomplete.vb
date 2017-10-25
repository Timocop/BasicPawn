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

        g_ClassToolTip = New ClassToolTip(Me)

        'Set double buffering to avoid annonying flickers when collapsing/showing SplitContainer panels
        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)

        ListView_AutocompleteList.ListViewItemSorter = New ListViewItemComparer(Me, 2)
        ListView_AutocompleteList.Sorting = SortOrder.Ascending
    End Sub

    Private Sub UCAutocomplete_Load(sender As Object, e As EventArgs) Handles Me.Load
        g_bControlLoaded = True
    End Sub

    Class ListViewItemComparer
        Implements IComparer

        Private g_mUCAutocomplete As UCAutocomplete
        Private g_Collum As Integer

        Public Sub New(c As UCAutocomplete)
            g_mUCAutocomplete = c
            g_Collum = 0
        End Sub

        Public Sub New(c As UCAutocomplete, mCollum As Integer)
            g_mUCAutocomplete = c
            g_Collum = mCollum
        End Sub

        Private Function IComparer_Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            If (g_mUCAutocomplete.g_sLastAutocompleteText.Length > 0) Then
                Dim mItemX As ListViewItem = DirectCast(x, ListViewItem)
                Dim mItemY As ListViewItem = DirectCast(y, ListViewItem)

                Dim iItemXIndex As Integer = mItemX.SubItems(g_Collum).Text.IndexOf(g_mUCAutocomplete.g_sLastAutocompleteText, If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase))
                Dim iItemYIndex As Integer = mItemY.SubItems(g_Collum).Text.IndexOf(g_mUCAutocomplete.g_sLastAutocompleteText, If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase))

                Return iItemXIndex.CompareTo(iItemYIndex)
            Else
                Dim mItemX As ListViewItem = DirectCast(x, ListViewItem)
                Dim mItemY As ListViewItem = DirectCast(y, ListViewItem)

                Return String.Compare(mItemX.SubItems(g_Collum).Text, mItemY.SubItems(g_Collum).Text, If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase))
            End If

        End Function
    End Class


    Public Function UpdateIntelliSense() As Boolean
        Dim sTextContent As String = ClassThread.ExecEx(Of String)(Me, Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent)
        Dim iCaretOffset As Integer = ClassThread.ExecEx(Of Integer)(Me, Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset)
        Dim iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = ClassThread.ExecEx(Of ClassSyntaxTools.ENUM_LANGUAGE_TYPE)(Me, Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Language)
        Dim mSourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sTextContent, iLanguage)

        If (iCaretOffset < 0 OrElse iCaretOffset > sTextContent.Length - 1) Then
            Return False
        End If

        If (Not mSourceAnalysis.m_InRange(iCaretOffset) OrElse
                        mSourceAnalysis.m_InMultiComment(iCaretOffset) OrElse
                        mSourceAnalysis.m_InSingleComment(iCaretOffset)) Then
            Return False
        End If

        'Create a valid range to read the method name and for performance. 
        Dim mStringBuilder As New StringBuilder
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

        ClassThread.ExecAsync(Me, Sub()
                                      g_ClassToolTip.m_IntelliSenseFunction = sMethodStart
                                      g_ClassToolTip.UpdateToolTip()
                                  End Sub)
        Return True
    End Function


    Public Function UpdateAutocomplete(sText As String) As Integer
        If (String.IsNullOrEmpty(sText) OrElse sText.Length < 3 OrElse Regex.IsMatch(sText, "^[0-9]+$")) Then
            ListView_AutocompleteList.Items.Clear()
            ListView_AutocompleteList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
            g_sLastAutocompleteText = ""
            Return 0
        End If

        Dim bSelectedWord As Boolean = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.SelectionManager.HasSomethingSelected
        Dim lListViewItemsList As New List(Of ClassListViewItemData)

        Dim sAutocompleteArray As ClassSyntaxTools.STRUC_AUTOCOMPLETE() = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_AutocompleteItems.ToArray
        For i = 0 To sAutocompleteArray.Length - 1
            If (bSelectedWord) Then
                If (sAutocompleteArray(i).m_FunctionName.Equals(sText)) Then
                    Dim mListViewItemData As New ClassListViewItemData(New String() {sAutocompleteArray(i).m_File,
                                                                                    sAutocompleteArray(i).GetTypeFullNames,
                                                                                    sAutocompleteArray(i).m_FunctionName,
                                                                                    sAutocompleteArray(i).m_FullFunctionName})

                    mListViewItemData.g_mData("File") = sAutocompleteArray(i).m_File
                    mListViewItemData.g_mData("TypeFullNames") = sAutocompleteArray(i).GetTypeFullNames
                    mListViewItemData.g_mData("FunctionName") = sAutocompleteArray(i).m_FunctionName
                    mListViewItemData.g_mData("FullFunctionName") = sAutocompleteArray(i).m_FullFunctionName
                    mListViewItemData.g_mData("Info") = sAutocompleteArray(i).m_Info

                    lListViewItemsList.Add(mListViewItemData)
                End If
            Else
                If (sAutocompleteArray(i).m_File.Equals(sText, If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase)) OrElse
                            sAutocompleteArray(i).m_FunctionName.IndexOf(sText, If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase)) > -1) Then
                    Dim mListViewItemData As New ClassListViewItemData(New String() {sAutocompleteArray(i).m_File,
                                                                                    sAutocompleteArray(i).GetTypeFullNames,
                                                                                    sAutocompleteArray(i).m_FunctionName,
                                                                                    sAutocompleteArray(i).m_FullFunctionName})

                    mListViewItemData.g_mData("File") = sAutocompleteArray(i).m_File
                    mListViewItemData.g_mData("TypeFullNames") = sAutocompleteArray(i).GetTypeFullNames
                    mListViewItemData.g_mData("FunctionName") = sAutocompleteArray(i).m_FunctionName
                    mListViewItemData.g_mData("FullFunctionName") = sAutocompleteArray(i).m_FullFunctionName
                    mListViewItemData.g_mData("Info") = sAutocompleteArray(i).m_Info

                    lListViewItemsList.Add(mListViewItemData)
                End If
            End If
        Next

        ListView_AutocompleteList.Items.Clear()
        ListView_AutocompleteList.Items.AddRange(lListViewItemsList.ToArray)

        If (ClassSettings.g_iSettingsSwitchTabToAutocomplete AndAlso lListViewItemsList.Count > 0 AndAlso g_mFormMain.TabControl_Details.TabPages.IndexOf(g_mFormMain.TabControl_Details.SelectedTab) <> 0) Then
            g_mFormMain.TabControl_Details.SuspendLayout()
            g_mFormMain.TabControl_Details.Enabled = False
            g_mFormMain.TabControl_Details.SelectTab(0)
            g_mFormMain.TabControl_Details.Enabled = True
            g_mFormMain.TabControl_Details.ResumeLayout()
        End If

        'Sort ascending first then match the closest one.
        g_sLastAutocompleteText = ""
        ListView_AutocompleteList.Sort()
        g_sLastAutocompleteText = sText
        ListView_AutocompleteList.Sort()


        If (ListView_AutocompleteList.Items.Count > 0) Then
            ListView_AutocompleteList.Items(0).Selected = True
            ListView_AutocompleteList.Items(0).EnsureVisible()

            ListView_AutocompleteList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
        Else
            ListView_AutocompleteList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
        End If

        Return ListView_AutocompleteList.Items.Count
    End Function

    Public Function GetSelectedItem() As ClassSyntaxTools.STRUC_AUTOCOMPLETE
        If (ListView_AutocompleteList.SelectedItems.Count < 1) Then
            Return Nothing
        End If

        If (TypeOf ListView_AutocompleteList.SelectedItems(0) IsNot ClassListViewItemData) Then
            Return Nothing
        End If

        Dim mListViewItemData = DirectCast(ListView_AutocompleteList.SelectedItems(0), ClassListViewItemData)

        Dim mAutocomplete As New ClassSyntaxTools.STRUC_AUTOCOMPLETE(CStr(mListViewItemData.g_mData("Info")),
                                                                     CStr(mListViewItemData.g_mData("File")),
                                                                     ClassSyntaxTools.STRUC_AUTOCOMPLETE.ParseTypeFullNames(CStr(mListViewItemData.g_mData("TypeFullNames"))),
                                                                     CStr(mListViewItemData.g_mData("FunctionName")),
                                                                     CStr(mListViewItemData.g_mData("FullFunctionName")))

        Return mAutocomplete
    End Function

    Private Sub ListView_AutocompleteList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_AutocompleteList.SelectedIndexChanged
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

        Public Sub UpdateToolTip()
            If (Not ClassSettings.g_iSettingsEnableToolTip) Then
                g_AutocompleteUC.SplitContainer1.Panel2Collapsed = True
                Return
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

                Dim bIsMethodMap As Boolean = sIntelliSenseFunction.Contains("."c)

                Dim lAlreadyShownList As New List(Of String)

                Dim bPrintedInfo As Boolean = False
                Dim iPrintedItems As Integer = 0
                Dim iMaxPrintedItems As Integer = 3
                Dim sAutocompleteArray As ClassSyntaxTools.STRUC_AUTOCOMPLETE() = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_AutocompleteItems.ToArray
                For i = 0 To sAutocompleteArray.Length - 1
                    If ((sAutocompleteArray(i).m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE AndAlso Not bIsMethodMap) Then
                        Continue For
                    End If

                    If (bIsMethodMap) Then
                        If (Not sAutocompleteArray(i).m_FunctionName.Equals(sIntelliSenseFunction)) Then
                            Continue For
                        End If
                    Else
                        If (Not sAutocompleteArray(i).m_FunctionName.Contains(sIntelliSenseFunction) OrElse
                                    Not Regex.IsMatch(sAutocompleteArray(i).m_FunctionName, String.Format("{0}\b{1}\b", If(bIsMethodMapEnd, "(\.)", ""), Regex.Escape(sIntelliSenseFunction)))) Then
                            Continue For
                        End If
                    End If


                    If (ClassSettings.g_iSettingsUseWindowsToolTip AndAlso Not bPrintedInfo) Then
                        bPrintedInfo = True
                        SB_TipText_IntelliSenseToolTip.AppendLine("IntelliSense:")
                    End If

                    Dim sName As String = Regex.Replace(sAutocompleteArray(i).m_FullFunctionName.Trim, vbTab, New String(" "c, iTabSize))
                    Dim sNameToolTip As String = Regex.Replace(sAutocompleteArray(i).m_FullFunctionName.Trim, vbTab, New String(" "c, iTabSize))

                    If (lAlreadyShownList.Contains(sName)) Then
                        Continue For
                    End If

                    lAlreadyShownList.Add(sName)

                    If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
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

                    Dim sComment As String = Regex.Replace(sAutocompleteArray(i).m_Info.Trim, String.Format("(^|{0})", vbTab), New String(" "c, iTabSize), RegexOptions.Multiline)

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
            If (g_AutocompleteUC.ListView_AutocompleteList.SelectedItems.Count > 0 AndAlso
                        TypeOf g_AutocompleteUC.ListView_AutocompleteList.SelectedItems(0) Is ClassListViewItemData) Then
                Dim mListViewItemData = DirectCast(g_AutocompleteUC.ListView_AutocompleteList.SelectedItems(0), ClassListViewItemData)

                If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
                    SB_TipText_AutocompleteToolTip.AppendLine("Autocomplete:")
                End If

                Dim sName As String = Regex.Replace(CStr(mListViewItemData.g_mData("FullFunctionName")).Trim, vbTab, New String(" "c, iTabSize))
                Dim sNameToolTip As String = Regex.Replace(CStr(mListViewItemData.g_mData("FullFunctionName")).Trim, vbTab, New String(" "c, iTabSize))
                If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
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

                Dim sComment As String = Regex.Replace(CStr(mListViewItemData.g_mData("Info")).Trim, String.Format("(^|{0})", vbTab), New String(" "c, iTabSize), RegexOptions.Multiline)

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
                    g_AutocompleteUC.g_mFormMain.g_mFormToolTip.TextEditorControl_ToolTip.Document.TextContent = SB_TipText_IntelliSenseToolTip.ToString & SB_TipText_AutocompleteToolTip.ToString
                Else
                    If (g_AutocompleteUC.g_mFormMain.g_mFormToolTip.TextEditorControl_ToolTip.Document.TextLength > 0) Then
                        g_AutocompleteUC.g_mFormMain.g_mFormToolTip.TextEditorControl_ToolTip.Document.TextContent = ""
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

                Dim iNewLocation As New Point(iX + iWTabSpace, iY + iHTabSpace + iFontH)
                Dim bOutsideEditor As Boolean = False
                If (iNewLocation.X < iEditorX OrElse iNewLocation.X > (iEditorX + iEditorW)) Then
                    bOutsideEditor = True
                End If
                If (iNewLocation.Y < iEditorY OrElse iNewLocation.Y > (iEditorY + iEditorH)) Then
                    bOutsideEditor = True
                End If

                If (Not bOutsideEditor AndAlso g_AutocompleteUC.g_mFormMain.g_mFormToolTip.TextEditorControl_ToolTip.Document.TextLength > 0) Then
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
