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
Imports ICSharpCode.TextEditor

Public Class UCInformationList
    Private g_mFormMain As FormMain

    Public ReadOnly INFO_DATA_OPEN_PATH As String = "OpenPath" 'Open absolute path
    Public ReadOnly INFO_DATA_FIND_PATH As String = "FindPath" 'Find absolute/relative path in open tabs/includes
    Public ReadOnly INFO_DATA_FIND_LINES As String = "FindLines" 'Find lines. {Line} or {LineStart, LineEnd}.

    Public Sub New(f As FormMain)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormMain = f
    End Sub

    Public Sub PrintInformation(sType As String, sMessage As String, Optional bClear As Boolean = False, Optional bShowInformationTab As Boolean = False, Optional bEnsureVisible As Boolean = False)
        PrintInformation(sType, sMessage, Nothing, bClear, bShowInformationTab, bEnsureVisible)
    End Sub

    Public Sub PrintInformation(sType As String, sMessage As String, mData As Dictionary(Of String, Object), Optional bClear As Boolean = False, Optional bShowInformationTab As Boolean = False, Optional bEnsureVisible As Boolean = False)
        ClassThread.ExecAsync(Me, Sub()
                                      If (bClear) Then
                                          ListBox_Information.Items.Clear()
                                      End If

                                      Dim mItem As New ClassListBoxItemData(String.Format("{0} ({1})", sType, Now.ToString), sMessage) With {
                                          .m_Data = mData
                                      }

                                      Dim iIndex = ListBox_Information.Items.Add(mItem)

                                      g_mFormMain.ToolStripStatusLabel_LastInformation.Text = sMessage

                                      If (bShowInformationTab) Then
                                          g_mFormMain.SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False

                                          If (g_mFormMain.SplitContainer_ToolboxSourceAndDetails.SplitterDistance > (g_mFormMain.SplitContainer_ToolboxSourceAndDetails.Height - g_mFormMain.g_iDefaultDetailsSplitterDistance)) Then
                                              g_mFormMain.SplitContainer_ToolboxSourceAndDetails.SplitterDistance = (g_mFormMain.SplitContainer_ToolboxSourceAndDetails.Height - g_mFormMain.g_iDefaultDetailsSplitterDistance)
                                          End If

                                          g_mFormMain.TabControl_Details.SelectTab(g_mFormMain.TabPage_Information)
                                      End If

                                      If (bEnsureVisible) Then
                                          'Scroll to item
                                          ListBox_Information.TopIndex = iIndex
                                      End If
                                  End Sub)
    End Sub

    Public Sub ParseFromCompilerOutput(sOutputLine As String, mData As Dictionary(Of String, Object))
        Dim mMatch As Match = Regex.Match(sOutputLine, "^(?<File>.+?)\((?<Line>([0-9]+)|(?<LineStart>[0-9]+)\s*--\s*(?<LineEnd>[0-9]+))\)\s*:", RegexOptions.IgnoreCase)
        If (mMatch.Success) Then
            mData(INFO_DATA_FIND_PATH) = mMatch.Groups("File").Value.Replace("/"c, "\"c)

            If (mMatch.Groups("Line").Success) Then
                mData(INFO_DATA_FIND_LINES) = New Integer() {
                    CInt(mMatch.Groups("Line").Value)
                }
            ElseIf (mMatch.Groups("LineStart").Success AndAlso mMatch.Groups("LineEnd").Success) Then
                mData(INFO_DATA_FIND_LINES) = New Integer() {
                    CInt(mMatch.Groups("LineStart").Value),
                    CInt(mMatch.Groups("LineEnd").Value)
                }
            End If
        End If
    End Sub

    Private Sub ListBox_Information_DoubleClick(sender As Object, e As EventArgs) Handles ListBox_Information.DoubleClick
        ItemAction(ENUM_ITEM_ACTION.AUTO)
    End Sub


    Private Sub ToolStripMenuItem_OpenExplorer_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OpenExplorer.Click
        ItemAction(ENUM_ITEM_ACTION.OPEN)
    End Sub

    Private Sub ToolStripMenuItem_GotoLine_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_GotoLine.Click
        ItemAction(ENUM_ITEM_ACTION.GOTO)
    End Sub


    Private Sub ToolStripMenuItem_OpenNotepadAllFull_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OpenNotepadAllFull.Click
        ItemsContentAction(ENUM_COPY_ACTION.NOTEPAD, ENUM_COPY_SELECTION.ALL, ENUM_COPY_TYPE.FULL)
    End Sub

    Private Sub ToolStripMenuItem_OpenNotepadAllMin_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OpenNotepadAllMin.Click
        ItemsContentAction(ENUM_COPY_ACTION.NOTEPAD, ENUM_COPY_SELECTION.ALL, ENUM_COPY_TYPE.MIN)
    End Sub

    Private Sub ToolStripMenuItem_OpenNotepadSelectedFull_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OpenNotepadSelectedFull.Click
        ItemsContentAction(ENUM_COPY_ACTION.NOTEPAD, ENUM_COPY_SELECTION.SELECTED, ENUM_COPY_TYPE.FULL)
    End Sub

    Private Sub ToolStripMenuItem_OpenNotepadSelectedMin_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OpenNotepadSelectedMin.Click
        ItemsContentAction(ENUM_COPY_ACTION.NOTEPAD, ENUM_COPY_SELECTION.SELECTED, ENUM_COPY_TYPE.MIN)
    End Sub

    Private Sub ToolStripMenuItem_CopyAllFull_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CopyAllFull.Click
        ItemsContentAction(ENUM_COPY_ACTION.COPY, ENUM_COPY_SELECTION.ALL, ENUM_COPY_TYPE.FULL)
    End Sub

    Private Sub ToolStripMenuItem_CopyAllMin_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CopyAllMin.Click
        ItemsContentAction(ENUM_COPY_ACTION.COPY, ENUM_COPY_SELECTION.ALL, ENUM_COPY_TYPE.MIN)
    End Sub

    Private Sub ToolStripMenuItem_CopySelectedFull_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CopySelectedFull.Click
        ItemsContentAction(ENUM_COPY_ACTION.COPY, ENUM_COPY_SELECTION.SELECTED, ENUM_COPY_TYPE.FULL)
    End Sub

    Private Sub ToolStripMenuItem_CopySelectedMin_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CopySelectedMin.Click
        ItemsContentAction(ENUM_COPY_ACTION.COPY, ENUM_COPY_SELECTION.SELECTED, ENUM_COPY_TYPE.MIN)
    End Sub

    Enum ENUM_ITEM_ACTION
        AUTO
        OPEN
        [GOTO]
    End Enum

    Private Sub ItemAction(iAction As ENUM_ITEM_ACTION)
        If (ListBox_Information.SelectedItems.Count < 1) Then
            Return
        End If

        If (ListBox_Information.SelectedItems(0) Is Nothing) Then
            Return
        End If

        Dim mSelectedItem As ClassListBoxItemData = TryCast(ListBox_Information.SelectedItems(0), ClassListBoxItemData)
        If (mSelectedItem Is Nothing) Then
            Return
        End If

        'We have no data to work with, end here
        If (mSelectedItem.m_Data Is Nothing) Then
            Return
        End If

        'Open path in explorer
        If (iAction = ENUM_ITEM_ACTION.AUTO OrElse iAction = ENUM_ITEM_ACTION.OPEN) Then
            If (mSelectedItem.m_Data.ContainsKey(INFO_DATA_OPEN_PATH)) Then
                Dim sPath As String = CStr(mSelectedItem.m_Data(INFO_DATA_OPEN_PATH))
                If (String.IsNullOrEmpty(sPath)) Then
                    MessageBox.Show("Invalid path", "Unable to open path", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                Select Case (True)
                    Case (IO.File.Exists(sPath))
                        Dim sFile As String = IO.Path.GetFullPath(sPath)

                        Process.Start("explorer.exe", String.Format("/select,""{0}""", sFile))
                    Case (IO.Directory.Exists(sPath))
                        Dim sDir As String = IO.Path.GetFullPath(sPath)

                        Process.Start("explorer.exe", sDir)

                    Case Else
                        MessageBox.Show(String.Format("Could not find path '{0}'!", sPath), "Unable to open path", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Select
            End If
        End If

        'Goto line number if path is open in a tab or includes
        If (iAction = ENUM_ITEM_ACTION.AUTO OrElse iAction = ENUM_ITEM_ACTION.GOTO) Then
            If (mSelectedItem.m_Data.ContainsKey(INFO_DATA_FIND_PATH) AndAlso mSelectedItem.m_Data.ContainsKey(INFO_DATA_FIND_LINES)) Then
                Dim sPath As String = CStr(mSelectedItem.m_Data(INFO_DATA_FIND_PATH))
                Dim iLines As Integer() = CType(mSelectedItem.m_Data(INFO_DATA_FIND_LINES), Integer())

                Dim bForceEnd As Boolean = False
                While True
                    For i = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                        If (g_mFormMain.g_ClassTabControl.m_Tab(i).m_IsUnsaved OrElse g_mFormMain.g_ClassTabControl.m_Tab(i).m_InvalidFile) Then
                            Continue For
                        End If

                        Dim sFile As String = g_mFormMain.g_ClassTabControl.m_Tab(i).m_File.Replace("/"c, "\"c)

                        If (sFile.ToLower.EndsWith(sPath.ToLower)) Then
                            Dim iLineNum As Integer = ClassTools.ClassMath.ClampInt(0, g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.TotalNumberOfLines - 1, iLines(0) - 1)

                            g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.Caret.Line = iLineNum
                            g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.Caret.Column = 0
                            g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.SelectionManager.ClearSelection()

                            Dim iLineLen As Integer = g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.GetLineSegment(iLineNum).Length

                            Dim iStart As New TextLocation(0, iLineNum)
                            Dim iEnd As New TextLocation(iLineLen, iLineNum)

                            g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(iStart, iEnd)

                            If (g_mFormMain.g_ClassTabControl.m_ActiveTabIndex <> i) Then
                                g_mFormMain.g_ClassTabControl.SelectTab(i)
                            End If

                            Return
                        End If
                    Next

                    If (bForceEnd) Then
                        Exit While
                    End If

                    For Each mInclude As DictionaryEntry In g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IncludeFilesFull.ToArray
                        If (String.IsNullOrEmpty(CStr(mInclude.Value)) OrElse Not IO.File.Exists(CStr(mInclude.Value))) Then
                            Continue For
                        End If

                        Dim sFile As String = CStr(mInclude.Value).Replace("/"c, "\"c)

                        If (sFile.ToLower.EndsWith(sPath.ToLower)) Then
                            Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
                            mTab.OpenFileTab(CStr(mInclude.Value))
                            mTab.SelectTab()

                            bForceEnd = True
                            Continue While
                        End If
                    Next

                    Exit While
                End While

                MessageBox.Show(String.Format("Could not find path '{0}'!", sPath), "Unable to open path", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub


    Enum ENUM_COPY_ACTION
        NOTEPAD
        COPY
    End Enum

    Enum ENUM_COPY_SELECTION
        ALL
        SELECTED
    End Enum

    Enum ENUM_COPY_TYPE
        FULL
        MIN
    End Enum

    Private Sub ItemsContentAction(iAction As ENUM_COPY_ACTION, iSelection As ENUM_COPY_SELECTION, iType As ENUM_COPY_TYPE)
        Try
            Dim mContent As New Text.StringBuilder

            Dim lItems As New List(Of Object)

            Select Case (iSelection)
                Case ENUM_COPY_SELECTION.ALL
                    For Each mItem As Object In ListBox_Information.Items
                        lItems.Add(mItem)
                    Next

                Case ENUM_COPY_SELECTION.SELECTED
                    For Each mItem As Object In ListBox_Information.SelectedItems
                        lItems.Add(mItem)
                    Next
            End Select

            For Each mItem As Object In lItems.ToArray
                Dim mListBoxItem As ClassListBoxItemData = TryCast(mItem, ClassListBoxItemData)
                If (mListBoxItem Is Nothing) Then
                    Continue For
                End If

                Select Case (iType)
                    Case ENUM_COPY_TYPE.FULL
                        mContent.AppendLine(mListBoxItem.ToStringFull)

                    Case ENUM_COPY_TYPE.MIN
                        mContent.AppendLine(mListBoxItem.ToStringMinimal)

                End Select
            Next

            Select Case (iAction)
                Case ENUM_COPY_ACTION.NOTEPAD
                    Dim sTempFile As String = IO.Path.GetTempFileName
                    IO.File.WriteAllText(sTempFile, mContent.ToString)

                    Process.Start("notepad.exe", sTempFile)

                Case ENUM_COPY_ACTION.COPY
                    My.Computer.Clipboard.SetText(mContent.ToString, TextDataFormat.Text)

            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub


    Class ClassListBoxItemData
        Property m_Info As String
        Property m_Message As String
        Property m_Text As String
        Property m_Data As Dictionary(Of String, Object)

        Sub New(_Info As String, _Message As String)
            Me.m_Info = _Info
            Me.m_Message = _Message
            Me.m_Text = String.Format("{0} {1}", _Info, _Message)
        End Sub

        Public Function ToStringFull() As String
            Return m_Text
        End Function

        Public Function ToStringMinimal() As String
            Return m_Message
        End Function

        Public Overrides Function ToString() As String
            If (m_Data IsNot Nothing AndAlso m_Data.Count > 0) Then
                'Add somekind of 'link' symbol: Ꝉ
                Return m_Text & " " & ChrW(&HA748)
            Else
                Return m_Text
            End If
        End Function
    End Class

    Private Sub ContextMenuStrip_Information_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Information.Opening
        ToolStripMenuItem_OpenExplorer.Enabled = False
        ToolStripMenuItem_GotoLine.Enabled = False

        While True
            If (ListBox_Information.SelectedItems.Count <> 1) Then
                Exit While
            End If

            If (ListBox_Information.SelectedItems(0) Is Nothing) Then
                Exit While
            End If

            Dim mSelectedItem As ClassListBoxItemData = TryCast(ListBox_Information.SelectedItems(0), ClassListBoxItemData)
            If (mSelectedItem Is Nothing) Then
                Exit While
            End If

            'We have no data to work with, end here
            If (mSelectedItem.m_Data Is Nothing) Then
                Exit While
            End If

            ToolStripMenuItem_OpenExplorer.Enabled = (mSelectedItem.m_Data.ContainsKey(INFO_DATA_OPEN_PATH))
            ToolStripMenuItem_GotoLine.Enabled = (mSelectedItem.m_Data.ContainsKey(INFO_DATA_FIND_PATH) AndAlso mSelectedItem.m_Data.ContainsKey(INFO_DATA_FIND_LINES))

            Exit While
        End While
    End Sub
End Class
