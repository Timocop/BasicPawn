Imports System.ComponentModel
Imports System.Text.RegularExpressions

Public Class ToolTipForm
    Public g_mMainForm As MainForm
    Public g_bIsResizeWindow As Boolean = False
    Public g_bBlockHideOnDeactivate As Boolean = False
    Public g_bDontFocus As Boolean = False

    Private g_sCurrentMethodName As String = ""

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
        If (Not SettingsClass.g_iSettingsEnableToolTip) Then
            Me.Visible = False
            Return
        End If

        g_bBlockHideOnDeactivate = True

        'Dim sTipTitle As String = ""
        Dim SB_TipText_IntelliSense As New Text.StringBuilder
        Dim SB_TipText_Autocomplete As New Text.StringBuilder

        If (Not String.IsNullOrEmpty(g_sCurrentMethodName)) Then
            Dim sCurrentMethodName As String = g_sCurrentMethodName
            Dim bIsMethodMap As Boolean = sCurrentMethodName.StartsWith("."c)
            If (bIsMethodMap) Then
                sCurrentMethodName = sCurrentMethodName.Remove(0, 1)
            End If

            Dim bPrintedInfo As Boolean = False
            Dim sAutocompleteArray As MainForm.STRUC_AUTOCOMPLETE() = g_mMainForm.lAutocompleteList.ToArray
            For i = 0 To sAutocompleteArray.Length - 1
                If (sAutocompleteArray(i).sFunctionName.Contains(sCurrentMethodName) AndAlso Regex.IsMatch(sAutocompleteArray(i).sFunctionName, If(bIsMethodMap, "(\.)", "") & "\b" & Regex.Escape(sCurrentMethodName) & "\b")) Then
                    If (SettingsClass.g_iSettingsUseWindowsToolTipInstead AndAlso Not bPrintedInfo) Then
                        bPrintedInfo = True
                        SB_TipText_IntelliSense.AppendLine("IntelliSense:")
                    End If

                    Dim sName As String = Regex.Replace(sAutocompleteArray(i).sFullFunctionname.Trim, vbTab, New String(" "c, 8))
                    If (SettingsClass.g_iSettingsUseWindowsToolTipInstead) Then
                        Dim sCommaDistance As Integer = sName.IndexOf(","c)
                        If (sCommaDistance > -1) Then
                            Dim sTabCount As Integer = Tools.WordCount(sName.Substring(0, sCommaDistance), " ")
                            Dim iSynCR As New MainForm.SyntraxCharReader(sName)
                            For ii = sName.Length - 1 To 0 Step -1
                                If (sName(ii) <> ","c OrElse iSynCR.InNonCode(ii)) Then
                                    Continue For
                                End If

                                sName = sName.Insert(ii + 1, Environment.NewLine & New String(" "c, sTabCount + 1) & New String(" "c, sCommaDistance))
                            Next
                        End If
                    End If

                    Dim sComment As String = Regex.Replace(sAutocompleteArray(i).sInfo.Trim, "^", New String(" "c, 8), RegexOptions.Multiline)

                    SB_TipText_IntelliSense.AppendLine(sName)
                    If (SettingsClass.g_iSettingsToolTipMethodComments AndAlso Not String.IsNullOrEmpty(sComment.Trim)) Then
                        SB_TipText_IntelliSense.AppendLine(sComment)
                    End If
                    SB_TipText_IntelliSense.AppendLine()
                End If
            Next
        End If



        If (g_mMainForm.g_mAutocompleteUC.ListView1.SelectedItems.Count > 0) Then
            Dim mSelectedItem As ListViewItem = g_mMainForm.g_mAutocompleteUC.ListView1.SelectedItems(0)
            If (SettingsClass.g_iSettingsUseWindowsToolTipInstead) Then
                SB_TipText_Autocomplete.AppendLine("Autocomplete:")
            End If

            Dim sName As String = Regex.Replace(mSelectedItem.SubItems(3).Text.Trim, vbTab, New String(" "c, 8))
            If (SettingsClass.g_iSettingsUseWindowsToolTipInstead) Then
                Dim sCommaDistance As Integer = sName.IndexOf(","c)
                If (sCommaDistance > -1) Then
                    Dim sTabCount As Integer = Tools.WordCount(sName.Substring(0, sCommaDistance), " ")
                    Dim iSynCR As New MainForm.SyntraxCharReader(sName)
                    For ii = sName.Length - 1 To 0 Step -1
                        If (sName(ii) <> ","c OrElse iSynCR.InNonCode(ii)) Then
                            Continue For
                        End If

                        sName = sName.Insert(ii + 1, Environment.NewLine & New String(" "c, sTabCount) & New String(" "c, sCommaDistance))
                    Next
                End If
            End If

            Dim sComment As String = Regex.Replace(mSelectedItem.SubItems(4).Text.Trim, "^", New String(" "c, 8), RegexOptions.Multiline)

            SB_TipText_Autocomplete.AppendLine(sName)

            If (SettingsClass.g_iSettingsToolTipAutocompleteComments AndAlso Not String.IsNullOrEmpty(sComment.Trim)) Then
                SB_TipText_Autocomplete.AppendLine(sComment)
            End If
        End If

        Dim iX As Integer = g_mMainForm.TextEditorControl1.ActiveTextAreaControl.Caret.ScreenPosition.X + g_mMainForm.Location.X
        Dim iY As Integer = g_mMainForm.TextEditorControl1.ActiveTextAreaControl.Caret.ScreenPosition.Y + g_mMainForm.Location.Y
        Dim iFontH As Integer = (g_mMainForm.TextEditorControl1.ActiveTextAreaControl.Font.GetHeight * 2) + g_mMainForm.TextEditorControl1.ActiveTextAreaControl.Font.GetHeight

        If (SettingsClass.g_iSettingsUseWindowsToolTipInstead) Then
            If (SB_TipText_IntelliSense.Length + SB_TipText_Autocomplete.Length > 0) Then
                Dim lTitleList As New List(Of String)
                If (SB_TipText_IntelliSense.Length > 0) Then
                    lTitleList.Add("IntelliSense")
                End If
                If (SB_TipText_Autocomplete.Length > 0) Then
                    lTitleList.Add("Autocomplete")
                End If

                g_mMainForm.ToolTip1.ToolTipTitle = String.Join(" & ", lTitleList.ToArray)
                g_mMainForm.ToolTip1.Show(SB_TipText_IntelliSense.ToString & SB_TipText_Autocomplete.ToString.ToString, g_mMainForm.TextEditorControl1, iX, iY + iFontH, Int32.MaxValue)
            Else
                g_mMainForm.ToolTip1.Hide(g_mMainForm.TextEditorControl1)
            End If
        Else
            If (SB_TipText_IntelliSense.Length > 0) Then
                SplitContainer1.Panel1Collapsed = False
                RichTextBox1.Text = SB_TipText_IntelliSense.ToString
            Else
                SplitContainer1.Panel1Collapsed = True
            End If

            If (SB_TipText_Autocomplete.Length > 0) Then
                SplitContainer1.Panel2Collapsed = False
                RichTextBox2.Text = SB_TipText_Autocomplete.ToString
            Else
                SplitContainer1.Panel2Collapsed = True
            End If

            If (SB_TipText_IntelliSense.Length = 0 AndAlso SB_TipText_Autocomplete.Length = 0) Then
                Me.Visible = False
            Else
                If (SplitContainer1.Orientation = Orientation.Horizontal) Then
                    SplitContainer1.SplitterDistance = SplitContainer1.Height / 2
                Else
                    SplitContainer1.SplitterDistance = SplitContainer1.Width / 2
                End If

                Dim iTitlebarHeight = Me.Height - Me.ClientSize.Height 'RectangleToScreen(Me.ClientRectangle).Top - Me.Top

                Me.Location = New Point(iX, iY + iFontH + iTitlebarHeight)
                Me.Visible = True

                If (Not g_bDontFocus) Then
                    g_mMainForm.Focus()
                End If
            End If

        End If

        g_bBlockHideOnDeactivate = False
    End Sub

    Private Sub ToolTipForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.Visible = False
        e.Cancel = True
    End Sub

    Private Sub ToolTipForm_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin
        g_bIsResizeWindow = True
        Me.Visible = True
    End Sub

    Private Sub ToolTipForm_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        g_bIsResizeWindow = False
        UpdateToolTip()
    End Sub
End Class