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

Public Class UCAutocomplete
    Private g_mFormMain As FormMain

    Public g_ClassToolTip As ClassToolTip 
    Public g_sLastAutocompleteText As String = ""

    Public Sub New(f As FormMain)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormMain = f

        g_ClassToolTip = New ClassToolTip(Me)

        'Set double buffering to avoid annonying flickers when collapsing/showing SplitContainer panels
        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)

        ListView_AutocompleteList.ListViewItemSorter = New ListViewItemComparer(Me, 2)
        ListView_AutocompleteList.Sorting = SortOrder.Ascending
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
                Dim lvX As ListViewItem = DirectCast(x, ListViewItem)
                Dim lvY As ListViewItem = DirectCast(y, ListViewItem)

                Dim lvXIndex As Integer = lvX.SubItems(g_Collum).Text.IndexOf(g_mUCAutocomplete.g_sLastAutocompleteText)
                Dim lvYIndex As Integer = lvY.SubItems(g_Collum).Text.IndexOf(g_mUCAutocomplete.g_sLastAutocompleteText)

                Return lvXIndex.CompareTo(lvYIndex)
            Else
                Dim lvX As ListViewItem = DirectCast(x, ListViewItem)
                Dim lvY As ListViewItem = DirectCast(y, ListViewItem)

                Return String.Compare(lvX.SubItems(g_Collum).Text, lvY.SubItems(g_Collum).Text)
            End If

        End Function
    End Class


    Public Function ParseMethodAutocomplete(Optional bForceUpdate As Boolean = False) As Boolean
        If (bForceUpdate) Then
            Dim sTextContent As String = CStr(Me.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent))
            g_mFormMain.g_mSourceSyntaxSourceAnalysis = New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sTextContent)
        End If

        If (g_mFormMain.g_mSourceSyntaxSourceAnalysis Is Nothing) Then
            Return False
        End If

        Dim iCaretOffset As Integer = CInt(Me.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset))

        If (iCaretOffset <= 1 OrElse
                        iCaretOffset >= g_mFormMain.g_mSourceSyntaxSourceAnalysis.GetMaxLenght() OrElse
                        g_mFormMain.g_mSourceSyntaxSourceAnalysis.InMultiComment(iCaretOffset) OrElse
                        g_mFormMain.g_mSourceSyntaxSourceAnalysis.InSingleComment(iCaretOffset)) Then
            Return False
        End If

        Dim iValidOffset As Integer = -1
        Dim iCaretBrace As Integer = g_mFormMain.g_mSourceSyntaxSourceAnalysis.GetParenthesisLevel(iCaretOffset - 1)
        Dim i As Integer
        For i = iCaretOffset - 1 To 0 Step -1
            If (g_mFormMain.g_mSourceSyntaxSourceAnalysis.GetParenthesisLevel(i) < iCaretBrace) Then
                iValidOffset = i
                Exit For
            End If
        Next

        If (iValidOffset < 0) Then
            Return False
        End If

        Dim SB As New StringBuilder

        For i = iValidOffset To iValidOffset - 64 Step -1
            If (i < 0 OrElse i > CInt(Me.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Document.TextLength)) - 1) Then
                Exit For
            End If

            SB.Append(Me.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Document.GetCharAt(i)))
        Next

        Dim sFuncStart As String = StrReverse(SB.ToString)
        sFuncStart = Regex.Match(sFuncStart, "(\.){0,1}(\b[a-zA-Z0-9_]+\b)$").Value

        Me.BeginInvoke(Sub()
                           g_ClassToolTip.CurrentMethod = sFuncStart
                           g_ClassToolTip.UpdateToolTip()
                       End Sub)
        Return True
    End Function


    Public Function UpdateAutocomplete(sText As String) As Integer
        If (g_mFormMain Is Nothing) Then
            Return 0
        End If

        If (sText.Length < 3 OrElse String.IsNullOrEmpty(sText) OrElse Regex.IsMatch(sText, "^[0-9]+$")) Then
            ListView_AutocompleteList.Items.Clear()
            ListView_AutocompleteList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
            g_sLastAutocompleteText = ""
            Return 0
        End If

        Dim bSelectedWord As Boolean = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.SelectionManager.HasSomethingSelected
        Dim lListViewItemsList As New List(Of ListViewItem)

        Dim sAutocompleteArray As FormMain.STRUC_AUTOCOMPLETE() = g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.ToArray
        For i = 0 To sAutocompleteArray.Length - 1
            If (bSelectedWord) Then
                If (sAutocompleteArray(i).sFunctionName.Equals(sText)) Then
                    lListViewItemsList.Add(New ListViewItem(New String() {sAutocompleteArray(i).sFile,
                                                                            sAutocompleteArray(i).GetTypeFullNames,
                                                                            sAutocompleteArray(i).sFunctionName,
                                                                            sAutocompleteArray(i).sFullFunctionName,
                                                                            sAutocompleteArray(i).sInfo}))
                End If
            Else
                If (sAutocompleteArray(i).sFile.Equals(sText, If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase)) OrElse
                            sAutocompleteArray(i).sFunctionName.IndexOf(sText, If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase)) > -1) Then
                    lListViewItemsList.Add(New ListViewItem(New String() {sAutocompleteArray(i).sFile,
                                                                            sAutocompleteArray(i).GetTypeFullNames,
                                                                            sAutocompleteArray(i).sFunctionName,
                                                                            sAutocompleteArray(i).sFullFunctionName,
                                                                            sAutocompleteArray(i).sInfo}))
                End If
            End If
        Next

        ListView_AutocompleteList.Items.Clear()
        ListView_AutocompleteList.Items.AddRange(lListViewItemsList.ToArray)

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

    Public Function GetSelectedItem() As FormMain.STRUC_AUTOCOMPLETE
        If (ListView_AutocompleteList.SelectedItems.Count < 1) Then
            Return Nothing
        End If

        Dim mSelectedItem As ListViewItem = ListView_AutocompleteList.SelectedItems(0)

        Dim struc As New FormMain.STRUC_AUTOCOMPLETE
        struc.sFile = mSelectedItem.SubItems(0).Text
        struc.mType = struc.ParseTypeFullNames(mSelectedItem.SubItems(1).Text)
        struc.sFunctionName = mSelectedItem.SubItems(2).Text
        struc.sFullFunctionName = mSelectedItem.SubItems(3).Text
        struc.sInfo = mSelectedItem.SubItems(4).Text
        Return struc
    End Function

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_AutocompleteList.SelectedIndexChanged
        g_ClassToolTip.UpdateToolTip()
    End Sub

    Public Class ClassToolTip
        Public g_AutocompleteUC As UCAutocomplete

        Private g_sCurrentMethodName As String = ""

        Public Sub New(c As UCAutocomplete)
            g_AutocompleteUC = c
        End Sub

        Public Property CurrentMethod As String
            Get
                Return g_sCurrentMethodName
            End Get
            Set(value As String)
                g_sCurrentMethodName = value
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

            If (Not String.IsNullOrEmpty(g_sCurrentMethodName)) Then
                Dim sCurrentMethodName As String = g_sCurrentMethodName
                Dim bIsMethodMap As Boolean = sCurrentMethodName.StartsWith("."c)
                If (bIsMethodMap) Then
                    sCurrentMethodName = sCurrentMethodName.Remove(0, 1)
                End If

                Dim bPrintedInfo As Boolean = False
                Dim sAutocompleteArray As FormMain.STRUC_AUTOCOMPLETE() = g_AutocompleteUC.g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.ToArray
                For i = 0 To sAutocompleteArray.Length - 1
                    If (sAutocompleteArray(i).sFunctionName.Contains(sCurrentMethodName) AndAlso
                                (sAutocompleteArray(i).mType And FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE AndAlso
                                Regex.IsMatch(sAutocompleteArray(i).sFunctionName, String.Format("{0}\b{1}\b", If(bIsMethodMap, "(\.)", ""), Regex.Escape(sCurrentMethodName)))) Then

                        If (ClassSettings.g_iSettingsUseWindowsToolTip AndAlso Not bPrintedInfo) Then
                            bPrintedInfo = True
                            SB_TipText_IntelliSenseToolTip.AppendLine("IntelliSense:")
                        End If

                        Dim sName As String = Regex.Replace(sAutocompleteArray(i).sFullFunctionName.Trim, vbTab, New String(" "c, iTabSize))
                        Dim sNameToolTip As String = Regex.Replace(sAutocompleteArray(i).sFullFunctionName.Trim, vbTab, New String(" "c, iTabSize))
                        If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
                            Dim sNewlineDistance As Integer = sNameToolTip.IndexOf("("c)

                            If (sNewlineDistance > -1) Then
                                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sNameToolTip)
                                For ii = sNameToolTip.Length - 1 To 0 Step -1
                                    If (sNameToolTip(ii) <> ","c OrElse sourceAnalysis.InNonCode(ii)) Then
                                        Continue For
                                    End If

                                    sNameToolTip = sNameToolTip.Insert(ii + 1, Environment.NewLine & New String(" "c, sNewlineDistance))
                                Next
                            End If
                        End If

                        Dim sComment As String = Regex.Replace(sAutocompleteArray(i).sInfo.Trim, "^", New String(" "c, iTabSize), RegexOptions.Multiline)

                        SB_TipText_IntelliSense.AppendLine(sName)
                        SB_TipText_IntelliSenseToolTip.AppendLine(sNameToolTip)

                        If (ClassSettings.g_iSettingsToolTipMethodComments AndAlso Not String.IsNullOrEmpty(sComment.Trim)) Then
                            SB_TipText_IntelliSense.AppendLine(sComment)
                            SB_TipText_IntelliSenseToolTip.AppendLine(sComment)
                        End If

                        SB_TipText_IntelliSense.AppendLine()
                        SB_TipText_IntelliSenseToolTip.AppendLine()
                    End If
                Next
            End If



            If (g_AutocompleteUC.ListView_AutocompleteList.SelectedItems.Count > 0) Then
                Dim mSelectedItem As ListViewItem = g_AutocompleteUC.ListView_AutocompleteList.SelectedItems(0)
                If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
                    SB_TipText_AutocompleteToolTip.AppendLine("Autocomplete:")
                End If

                Dim sName As String = Regex.Replace(mSelectedItem.SubItems(3).Text.Trim, vbTab, New String(" "c, iTabSize))
                Dim sNameToolTip As String = Regex.Replace(mSelectedItem.SubItems(3).Text.Trim, vbTab, New String(" "c, iTabSize))
                If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
                    Dim sNewlineDistance As Integer = sNameToolTip.IndexOf("("c)

                    If (sNewlineDistance > -1) Then
                        Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sNameToolTip)
                        For ii = sNameToolTip.Length - 1 To 0 Step -1
                            If (sNameToolTip(ii) <> ","c OrElse sourceAnalysis.InNonCode(ii)) Then
                                Continue For
                            End If

                            sNameToolTip = sNameToolTip.Insert(ii + 1, Environment.NewLine & New String(" "c, sNewlineDistance))
                        Next
                    End If
                End If

                Dim sComment As String = Regex.Replace(mSelectedItem.SubItems(4).Text.Trim, "^", New String(" "c, iTabSize), RegexOptions.Multiline)

                SB_TipText_Autocomplete.AppendLine(sName)
                SB_TipText_AutocompleteToolTip.AppendLine(sNameToolTip)

                If (ClassSettings.g_iSettingsToolTipAutocompleteComments AndAlso Not String.IsNullOrEmpty(sComment.Trim)) Then
                    SB_TipText_Autocomplete.AppendLine(sComment)
                    SB_TipText_AutocompleteToolTip.AppendLine(sComment)
                End If
            End If

            If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
                'Dim iXSpace As Integer = g_AutocompleteUC.g_mFormMain.TextEditorControl1.ActiveTextAreaControl.PointToScreen(Point.Empty).X - g_AutocompleteUC.g_mFormMain.PointToScreen(Point.Empty).X
                'Dim iYSpace As Integer = g_AutocompleteUC.g_mFormMain.TextEditorControl1.ActiveTextAreaControl.PointToScreen(Point.Empty).Y - g_AutocompleteUC.g_mFormMain.PointToScreen(Point.Empty).Y
                Dim iXSpace As Integer = 0
                Dim iYSpace As Integer = 0

                Dim iX As Integer = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Caret.ScreenPosition.X + iXSpace
                Dim iY As Integer = g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Caret.ScreenPosition.Y + iYSpace
                Dim iFontH As Integer = CInt(g_AutocompleteUC.g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Font.GetHeight)

                Dim iWTabSpace As Integer = 6
                Dim iHTabSpace As Integer = g_AutocompleteUC.g_mFormMain.TabControl_SourceTabs.ItemSize.Height + 6

                'Dim iFontH As Integer = (g_AutocompleteUC.g_mFormMain.TextEditorControl1.ActiveTextAreaControl.Font.GetHeight * 2) + g_AutocompleteUC.g_mFormMain.TextEditorControl1.ActiveTextAreaControl.Font.GetHeight

                If (SB_TipText_IntelliSenseToolTip.Length + SB_TipText_AutocompleteToolTip.Length > 0) Then
                    g_AutocompleteUC.g_mFormMain.g_mUCToolTip.TextEditorControl_ToolTip.Document.TextContent = SB_TipText_IntelliSenseToolTip.ToString & SB_TipText_AutocompleteToolTip.ToString
                    g_AutocompleteUC.g_mFormMain.g_mUCToolTip.m_Location = New Point(iX + iWTabSpace, iY + iHTabSpace + iFontH)
                    g_AutocompleteUC.g_mFormMain.g_mUCToolTip.Show()
                    g_AutocompleteUC.g_mFormMain.g_mUCToolTip.TextEditorControl_ToolTip.Refresh()
                Else
                    g_AutocompleteUC.g_mFormMain.g_mUCToolTip.Hide()
                End If
            Else
                If (g_AutocompleteUC.g_mFormMain.g_mUCToolTip.Visible) Then
                    g_AutocompleteUC.g_mFormMain.g_mUCToolTip.TextEditorControl_ToolTip.Document.TextContent = ""
                    g_AutocompleteUC.g_mFormMain.g_mUCToolTip.Hide()
                    g_AutocompleteUC.g_mFormMain.g_mUCToolTip.TextEditorControl_ToolTip.Refresh()
                End If
            End If

            If (True) Then
                If (SB_TipText_IntelliSense.Length > 0) Then
                    g_AutocompleteUC.SplitContainer2.Panel1Collapsed = False
                    g_AutocompleteUC.RichTextBox_IntelliSense.Text = SB_TipText_IntelliSense.ToString
                Else
                    g_AutocompleteUC.SplitContainer2.Panel1Collapsed = True
                End If

                If (SB_TipText_Autocomplete.Length > 0) Then
                    g_AutocompleteUC.SplitContainer2.Panel2Collapsed = False
                    g_AutocompleteUC.RichTextBox_Autocomplete.Text = SB_TipText_Autocomplete.ToString
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
    End Class
End Class
