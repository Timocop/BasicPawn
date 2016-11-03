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


Imports System.Text.RegularExpressions

Public Class UCAutocomplete
    Private g_mFormMain As FormMain

    Public g_ClassToolTip As New ClassToolTip(Me)

    Public Sub New(f As FormMain)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormMain = f
    End Sub

    Public Function UpdateAutocomplete(sText As String) As Integer
        If (g_mFormMain Is Nothing) Then
            Return 0
        End If

        If (sText.Length < 3 OrElse String.IsNullOrEmpty(sText) OrElse Regex.IsMatch(sText, "^[0-9]+$")) Then
            ListView1.Items.Clear()
            ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
            Return 0
        End If

        Dim sMethodMapArray As String() = sText.Split("."c)

        Dim bSelectedWord As Boolean = g_mFormMain.TextEditorControl1.ActiveTextAreaControl.TextArea.SelectionManager.HasSomethingSelected

        Dim lListViewItemsList As New List(Of ListViewItem)

        Dim sAutocompleteArray As FormMain.STRUC_AUTOCOMPLETE() = g_mFormMain.g_ClassSyntraxTools.lAutocompleteList.ToArray
        For i = 0 To sAutocompleteArray.Length - 1
            If (bSelectedWord) Then
                If (sAutocompleteArray(i).sFunctionName.Equals(sText)) Then
                    lListViewItemsList.Add(New ListViewItem(New String() {sAutocompleteArray(i).sFile,
                                                                            sAutocompleteArray(i).sType,
                                                                            sAutocompleteArray(i).sFunctionName,
                                                                            sAutocompleteArray(i).sFullFunctionname,
                                                                            sAutocompleteArray(i).sInfo}))
                End If
            Else
                If (sAutocompleteArray(i).sFile.Equals(sText, If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase)) OrElse
                            sAutocompleteArray(i).sFunctionName.IndexOf(sText, If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase)) > -1) Then
                    lListViewItemsList.Add(New ListViewItem(New String() {sAutocompleteArray(i).sFile,
                                                                            sAutocompleteArray(i).sType,
                                                                            sAutocompleteArray(i).sFunctionName,
                                                                            sAutocompleteArray(i).sFullFunctionname,
                                                                            sAutocompleteArray(i).sInfo}))
                ElseIf (sMethodMapArray.Length = 2 AndAlso sAutocompleteArray(i).sType.Contains("methodmap") AndAlso sAutocompleteArray(i).sFunctionName.Split("."c).Length = 2) Then
                    Dim sMethodMapNames As String() = sAutocompleteArray(i).sFunctionName.Split("."c)
                    If (String.IsNullOrEmpty(sMethodMapArray(1))) Then
                        If (sMethodMapArray(0).IndexOf(sMethodMapNames(0)) > -1) Then
                            lListViewItemsList.Add(New ListViewItem(New String() {sAutocompleteArray(i).sFile,
                                                                                    sAutocompleteArray(i).sType,
                                                                                    sAutocompleteArray(i).sFunctionName,
                                                                                    sAutocompleteArray(i).sFullFunctionname,
                                                                                    sAutocompleteArray(i).sInfo}))
                        End If
                    Else
                        If (sMethodMapArray(0).IndexOf(sMethodMapNames(0)) > -1 AndAlso
                            sMethodMapNames(1).IndexOf(sMethodMapArray(1), If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase)) > -1) Then
                            lListViewItemsList.Add(New ListViewItem(New String() {sAutocompleteArray(i).sFile,
                                                                                    sAutocompleteArray(i).sType,
                                                                                    sAutocompleteArray(i).sFunctionName,
                                                                                    sAutocompleteArray(i).sFullFunctionname,
                                                                                    sAutocompleteArray(i).sInfo}))
                        End If
                    End If
                End If
            End If
        Next

        ListView1.Items.Clear()
        ListView1.Items.AddRange(lListViewItemsList.ToArray)

        If (ListView1.Items.Count > 0) Then
            ListView1.Items(0).Selected = True
            ListView1.Items(0).EnsureVisible()

            ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
        Else
            ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
        End If

        Return ListView1.Items.Count
    End Function

    Public Function GetSelectedItem() As FormMain.STRUC_AUTOCOMPLETE
        If (ListView1.SelectedItems.Count < 1) Then
            Return Nothing
        End If

        Dim mSelectedItem As ListViewItem = ListView1.SelectedItems(0)

        Dim struc As FormMain.STRUC_AUTOCOMPLETE
        struc.sFile = mSelectedItem.SubItems(0).Text
        struc.sType = mSelectedItem.SubItems(1).Text
        struc.sFunctionName = mSelectedItem.SubItems(2).Text
        struc.sFullFunctionname = mSelectedItem.SubItems(3).Text
        struc.sInfo = mSelectedItem.SubItems(4).Text
        Return struc
    End Function

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
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
                Dim sAutocompleteArray As FormMain.STRUC_AUTOCOMPLETE() = g_AutocompleteUC.g_mFormMain.g_ClassSyntraxTools.lAutocompleteList.ToArray
                For i = 0 To sAutocompleteArray.Length - 1
                    If (sAutocompleteArray(i).sFunctionName.Contains(sCurrentMethodName) AndAlso Regex.IsMatch(sAutocompleteArray(i).sFunctionName, If(bIsMethodMap, "(\.)", "") & "\b" & Regex.Escape(sCurrentMethodName) & "\b")) Then
                        If (ClassSettings.g_iSettingsUseWindowsToolTip AndAlso Not bPrintedInfo) Then
                            bPrintedInfo = True
                            SB_TipText_IntelliSenseToolTip.AppendLine("IntelliSense:")
                        End If

                        Dim sName As String = Regex.Replace(sAutocompleteArray(i).sFullFunctionname.Trim, vbTab, New String(" "c, iTabSize))
                        Dim sNameToolTip As String = Regex.Replace(sAutocompleteArray(i).sFullFunctionname.Trim, vbTab, New String(" "c, iTabSize))
                        If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
                            Dim sNewlineDistance As Integer = sNameToolTip.IndexOf("("c)

                            If (sNewlineDistance > -1) Then
                                Dim iSynCR As New FormMain.ClassSyntraxTools.ClassSyntraxCharReader(sNameToolTip)
                                For ii = sNameToolTip.Length - 1 To 0 Step -1
                                    If (sNameToolTip(ii) <> ","c OrElse iSynCR.InNonCode(ii)) Then
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



            If (g_AutocompleteUC.ListView1.SelectedItems.Count > 0) Then
                Dim mSelectedItem As ListViewItem = g_AutocompleteUC.ListView1.SelectedItems(0)
                If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
                    SB_TipText_AutocompleteToolTip.AppendLine("Autocomplete:")
                End If

                Dim sName As String = Regex.Replace(mSelectedItem.SubItems(3).Text.Trim, vbTab, New String(" "c, iTabSize))
                Dim sNameToolTip As String = Regex.Replace(mSelectedItem.SubItems(3).Text.Trim, vbTab, New String(" "c, iTabSize))
                If (ClassSettings.g_iSettingsUseWindowsToolTip) Then
                    Dim sNewlineDistance As Integer = sNameToolTip.IndexOf("("c)

                    If (sNewlineDistance > -1) Then
                        Dim iSynCR As New FormMain.ClassSyntraxTools.ClassSyntraxCharReader(sNameToolTip)
                        For ii = sNameToolTip.Length - 1 To 0 Step -1
                            If (sNameToolTip(ii) <> ","c OrElse iSynCR.InNonCode(ii)) Then
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

                Dim iX As Integer = g_AutocompleteUC.g_mFormMain.TextEditorControl1.ActiveTextAreaControl.Caret.ScreenPosition.X + iXSpace
                Dim iY As Integer = g_AutocompleteUC.g_mFormMain.TextEditorControl1.ActiveTextAreaControl.Caret.ScreenPosition.Y + iYSpace
                Dim iFontH As Integer = g_AutocompleteUC.g_mFormMain.TextEditorControl1.ActiveTextAreaControl.Font.GetHeight
                'Dim iFontH As Integer = (g_AutocompleteUC.g_mFormMain.TextEditorControl1.ActiveTextAreaControl.Font.GetHeight * 2) + g_AutocompleteUC.g_mFormMain.TextEditorControl1.ActiveTextAreaControl.Font.GetHeight

                If (SB_TipText_IntelliSenseToolTip.Length + SB_TipText_AutocompleteToolTip.Length > 0) Then
                    g_AutocompleteUC.g_mFormMain.g_mUCToolTip.TextEditorControl_ToolTip.Document.TextContent = SB_TipText_IntelliSenseToolTip.ToString & SB_TipText_AutocompleteToolTip.ToString
                    g_AutocompleteUC.g_mFormMain.g_mUCToolTip.Location = New Point(iX, iY + iFontH)
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
                        g_AutocompleteUC.SplitContainer2.SplitterDistance = g_AutocompleteUC.SplitContainer2.Height / 2
                    Else
                        g_AutocompleteUC.SplitContainer2.SplitterDistance = g_AutocompleteUC.SplitContainer2.Width / 2
                    End If

                    g_AutocompleteUC.SplitContainer1.Panel2Collapsed = False
                Else
                    g_AutocompleteUC.SplitContainer1.Panel2Collapsed = True
                End If
            End If
        End Sub
    End Class
End Class
