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

Public Class UCInformationList
    Private g_mFormMain As FormMain
    Private g_ClassActions As ClassActions

    Public Sub New(f As FormMain)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormMain = f

        g_ClassActions = New ClassActions(Me)

        'Set double buffering to avoid annonying flickers
        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)
    End Sub

    Public Sub PrintInformation(iIcon As ClassInformationListBox.ENUM_ICONS, sText As String, Optional bClear As Boolean = False, Optional bShowInformationTab As Boolean = False, Optional bEnsureVisible As Boolean = False)
        PrintInformation(iIcon, sText, Nothing, bClear, bShowInformationTab, bEnsureVisible)
    End Sub

    Public Sub PrintInformation(iIcon As ClassInformationListBox.ENUM_ICONS, sText As String, mAction As ClassListBoxItemAction.ClassActions.IAction, Optional bClear As Boolean = False, Optional bShowInformationTab As Boolean = False, Optional bEnsureVisible As Boolean = False)
        ClassThread.ExecAsync(Me, Sub()
                                      If (bClear) Then
                                          ListBox_Information.Items.Clear()
                                      End If

                                      'Tabs are not displayed correctly with TextRenderer. Replace them with spaces
                                      sText = sText.Replace(vbTab, New String(" "c, 4))

                                      Dim mItem As New ClassListBoxItemAction(iIcon, Now, sText) With {
                                          .m_Action = mAction
                                      }

                                      Dim iIndex = ListBox_Information.Items.Add(mItem)

                                      If (bShowInformationTab AndAlso g_mFormMain.ToolStripMenuItem_ViewDetails.Checked) Then
                                          g_mFormMain.SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False

                                          Dim iSplitterHeight As Integer = g_mFormMain.SplitContainer_ToolboxSourceAndDetails.Height - g_mFormMain.g_iDefaultDetailsSplitterDistance
                                          If (iSplitterHeight > 0 AndAlso g_mFormMain.SplitContainer_ToolboxSourceAndDetails.SplitterDistance > iSplitterHeight) Then
                                              g_mFormMain.SplitContainer_ToolboxSourceAndDetails.SplitterDistance = iSplitterHeight
                                          End If

                                          If (g_mFormMain.TabControl_Details.SelectedTab IsNot g_mFormMain.TabPage_Information) Then
                                              g_mFormMain.TabControl_Details.SelectTabNoFocus(g_mFormMain.TabPage_Information)
                                          End If
                                      End If

                                      If (bEnsureVisible) Then
                                          'Scroll to item
                                          ListBox_Information.TopIndex = iIndex
                                      End If
                                  End Sub)
    End Sub

    Public Function ParseFromCompilerOutput(sFile As String, sOutputLine As String) As ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO
        Dim mMatch As Match = Regex.Match(sOutputLine, "^(?<File>.+?)\(((?<Line>[0-9]+)|(?<LineStart>[0-9]+)\s*--\s*(?<LineEnd>[0-9]+))\)\s*:", RegexOptions.IgnoreCase)
        If (mMatch.Success) Then
            Dim sOutputFile As String = mMatch.Groups("File").Value.Replace("/"c, "\"c)

            'Try to make an absolute path from source file and compiler output.
            If (Not String.IsNullOrEmpty(sFile)) Then
                If (IO.Path.GetFullPath(sOutputFile).ToLower <> sOutputFile.ToLower) Then
                    sOutputFile = IO.Path.Combine(IO.Path.GetDirectoryName(sFile), sOutputFile)
                End If
            End If

            If (mMatch.Groups("Line").Success) Then
                Dim iLines As Integer() = New Integer() {
                    CInt(mMatch.Groups("Line").Value)
                }

                Return New ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO(sOutputFile, iLines)
            ElseIf (mMatch.Groups("LineStart").Success AndAlso mMatch.Groups("LineEnd").Success) Then
                Dim iLines As Integer() = New Integer() {
                    CInt(mMatch.Groups("LineStart").Value),
                    CInt(mMatch.Groups("LineEnd").Value)
                }

                Return New ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO(sOutputFile, iLines)
            End If
        End If

        Return Nothing
    End Function

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

            Dim mSelectedItem As ClassListBoxItemAction = TryCast(ListBox_Information.SelectedItems(0), ClassListBoxItemAction)
            If (mSelectedItem Is Nothing) Then
                Exit While
            End If

            'We have no data to work with, end here
            If (mSelectedItem.m_Action Is Nothing) Then
                Exit While
            End If

            ToolStripMenuItem_OpenExplorer.Enabled = (TypeOf mSelectedItem.m_Action Is ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN)
            ToolStripMenuItem_GotoLine.Enabled = (TypeOf mSelectedItem.m_Action Is ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO)

            Exit While
        End While
    End Sub

    Private Sub ListBox_Information_KeyUp(sender As Object, e As KeyEventArgs) Handles ListBox_Information.KeyUp
        If (e.Control AndAlso e.KeyCode = Keys.C) Then
            If (e.Shift) Then
                ItemsContentAction(ENUM_COPY_ACTION.COPY, ENUM_COPY_SELECTION.SELECTED, ENUM_COPY_TYPE.FULL)
            Else
                ItemsContentAction(ENUM_COPY_ACTION.COPY, ENUM_COPY_SELECTION.SELECTED, ENUM_COPY_TYPE.MIN)
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
        Try
            If (ListBox_Information.SelectedItems.Count < 1) Then
                Return
            End If

            If (ListBox_Information.SelectedItems(0) Is Nothing) Then
                Return
            End If

            Dim mSelectedItem As ClassListBoxItemAction = TryCast(ListBox_Information.SelectedItems(0), ClassListBoxItemAction)
            If (mSelectedItem Is Nothing) Then
                Return
            End If

            'Theres no action
            If (mSelectedItem.m_Action Is Nothing) Then
                Return
            End If

            'Open path in explorer or browser
            If (iAction = ENUM_ITEM_ACTION.AUTO OrElse iAction = ENUM_ITEM_ACTION.OPEN) Then
                If (TypeOf mSelectedItem.m_Action Is ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN) Then
                    Dim mAction = DirectCast(mSelectedItem.m_Action, ClassListBoxItemAction.ClassActions.STRUC_ACTION_OPEN)

                    If (String.IsNullOrEmpty(mAction.m_Path)) Then
                        MessageBox.Show("Invalid path", "Unable to open path", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    Select Case (True)
                        Case mAction.m_Path.ToLower.StartsWith("http://") OrElse mAction.m_Path.ToLower.StartsWith("https://")
                            Try
                                Process.Start(mAction.m_Path)
                            Catch ex As Exception
                            End Try

                        Case (IO.File.Exists(mAction.m_Path))
                            Dim sFile As String = IO.Path.GetFullPath(mAction.m_Path)

                            Process.Start("explorer.exe", String.Format("/select,""{0}""", sFile))
                        Case (IO.Directory.Exists(mAction.m_Path))
                            Dim sDir As String = IO.Path.GetFullPath(mAction.m_Path)

                            Process.Start("explorer.exe", sDir)

                        Case Else
                            MessageBox.Show(String.Format("Could not find path '{0}'!", mAction.m_Path), "Unable to open path", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Select
                End If
            End If

            'Goto line number if path is open in a tab or includes
            If (iAction = ENUM_ITEM_ACTION.AUTO OrElse iAction = ENUM_ITEM_ACTION.GOTO) Then
                If (TypeOf mSelectedItem.m_Action Is ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO) Then
                    Dim mAction = DirectCast(mSelectedItem.m_Action, ClassListBoxItemAction.ClassActions.STRUC_ACTION_GOTO)

                    If (String.IsNullOrEmpty(mAction.m_Path)) Then
                        MessageBox.Show("Invalid path", "Unable to open path", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    If (mAction.m_Lines.Length < 1) Then
                        MessageBox.Show("Invalid lines", "Unable to open path", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    Dim bForceEnd As Boolean = False
                    While True
                        For i = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
                            If (g_mFormMain.g_ClassTabControl.m_Tab(i).m_IsUnsaved OrElse g_mFormMain.g_ClassTabControl.m_Tab(i).m_InvalidFile) Then
                                Continue For
                            End If

                            Dim sFile As String = g_mFormMain.g_ClassTabControl.m_Tab(i).m_File.Replace("/"c, "\"c)

                            'Try to find using absolute and relative path
                            If (sFile.ToLower.EndsWith(mAction.m_Path.ToLower)) Then
                                Dim iLineNum As Integer = ClassTools.ClassMath.ClampInt(mAction.m_Lines(0) - 1, 0, g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.TotalNumberOfLines - 1)
                                Dim iLineLen As Integer = g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.GetLineSegment(iLineNum).Length

                                Dim mStartLoc As New TextLocation(0, iLineNum)
                                Dim mEndLoc As New TextLocation(iLineLen, iLineNum)

                                g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.Caret.Position = mStartLoc
                                g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.SelectionManager.ClearSelection()
                                g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(mStartLoc, mEndLoc)
                                g_mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.CenterViewOn(iLineNum, 10)

                                If (Not g_mFormMain.g_ClassTabControl.m_Tab(i).m_IsActive) Then
                                    g_mFormMain.g_ClassTabControl.m_Tab(i).SelectTab()
                                End If

                                Return
                            End If
                        Next

                        If (bForceEnd) Then
                            Exit While
                        End If

                        If (IO.File.Exists(mAction.m_Path)) Then
                            Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
                            mTab.OpenFileTab(mAction.m_Path)
                            mTab.SelectTab()

                            bForceEnd = True
                        End If

                        If (bForceEnd) Then
                            Continue While
                        End If

                        For Each mInclude In g_mFormMain.g_ClassTabControl.m_ActiveTab.m_IncludesGroup.m_IncludeFilesFull.ToArray
                            If (String.IsNullOrEmpty(mInclude.Value) OrElse Not IO.File.Exists(mInclude.Value)) Then
                                Continue For
                            End If

                            Dim sFile As String = mInclude.Value.Replace("/"c, "\"c)

                            'Try to find using absolute and relative path
                            If (sFile.ToLower.EndsWith(mAction.m_Path.ToLower)) Then
                                Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
                                mTab.OpenFileTab(mInclude.Value)
                                mTab.SelectTab()

                                bForceEnd = True
                                Continue While
                            End If
                        Next

                        Exit While
                    End While

                    MessageBox.Show(String.Format("Could not find path '{0}'!", mAction.m_Path), "Unable to open path", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
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
                Dim mListBoxItem As ClassListBoxItemAction = TryCast(mItem, ClassListBoxItemAction)
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

    Private Sub CleanUp()
        If (g_ClassActions IsNot Nothing) Then
            g_ClassActions.Dispose()
            g_ClassActions = Nothing
        End If
    End Sub

    Class ClassListBoxItemAction
        Inherits ClassInformationListBox.ClassInformationItem

        Class ClassActions
            Interface IAction
            End Interface

            Class STRUC_ACTION_OPEN
                Implements IAction

                Property m_Path As String

                Sub New(_Path As String)
                    m_Path = _Path
                End Sub
            End Class

            Class STRUC_ACTION_GOTO
                Implements IAction

                Property m_Path As String
                Property m_Lines As Integer()

                Sub New(_Path As String, _Lines As Integer())
                    m_Path = _Path
                    m_Lines = _Lines
                End Sub
            End Class
        End Class

        Property m_Action As ClassActions.IAction = Nothing

        Public Sub New(_Icon As ClassInformationListBox.ENUM_ICONS, _Time As Date, _Text As String)
            MyBase.New(_Icon, _Time, _Text)
        End Sub

        Public Function ToStringFull() As String
            Return String.Format("[{0}] {1}", m_Time.ToString, m_Text)
        End Function

        Public Function ToStringMinimal() As String
            Return m_Text
        End Function

        Public Overrides Function GetDrawIcon() As Image
            Select Case (True)
                Case (TypeOf m_Action Is ClassActions.STRUC_ACTION_OPEN)
                    Return My.Resources.imageres_5304_16x16

                Case (TypeOf m_Action Is ClassActions.STRUC_ACTION_GOTO)
                    Return My.Resources.imageres_5302_16x16
            End Select

            Return MyBase.GetDrawIcon()
        End Function
    End Class

    Class ClassActions
        Implements IDisposable

        Private g_mUCInformationList As UCInformationList

        Sub New(f As UCInformationList)
            g_mUCInformationList = f

            AddHandler g_mUCInformationList.g_mFormMain.g_ClassTabControl.OnTextEditorTabDetailsAction, AddressOf OnTextEditorTabDetailsAction
            AddHandler g_mUCInformationList.g_mFormMain.g_ClassTabControl.OnTextEditorTabDetailsMove, AddressOf OnTextEditorTabDetailsMove
        End Sub

        Public Sub OnTextEditorTabDetailsAction(mTab As ClassTabControl.ClassTab, iDetailsTabIndex As Integer, bIsSpecialAction As Boolean, iKeys As Keys)
            If (iDetailsTabIndex <> g_mUCInformationList.g_mFormMain.TabPage_Information.TabIndex) Then
                Return
            End If

            If (bIsSpecialAction) Then
                g_mUCInformationList.ItemAction(ENUM_ITEM_ACTION.OPEN)
            Else
                g_mUCInformationList.ItemAction(ENUM_ITEM_ACTION.GOTO)
            End If
        End Sub

        Public Sub OnTextEditorTabDetailsMove(mTab As ClassTabControl.ClassTab, iDetailsTabIndex As Integer, iDirection As Integer, iKeys As Keys)
            If (iDetailsTabIndex <> g_mUCInformationList.g_mFormMain.TabPage_Information.TabIndex) Then
                Return
            End If

            If (iDirection = 0) Then
                Return
            End If

            Static iLastCount As Integer = 0
            If (g_mUCInformationList.ListBox_Information.Items.Count < 1) Then
                Return
            End If

            Dim bCountChanged = (g_mUCInformationList.ListBox_Information.Items.Count <> iLastCount)
            iLastCount = g_mUCInformationList.ListBox_Information.Items.Count

            If (bCountChanged OrElse g_mUCInformationList.ListBox_Information.SelectedItems.Count < 1) Then
                g_mUCInformationList.ListBox_Information.SelectedIndices.Clear()
                g_mUCInformationList.ListBox_Information.SelectedIndex = (g_mUCInformationList.ListBox_Information.Items.Count - 1)
            End If

            'What? Mmh...
            If (g_mUCInformationList.ListBox_Information.SelectedItems.Count < 1) Then
                Return
            End If

            Dim iCount As Integer = g_mUCInformationList.ListBox_Information.Items.Count
            Dim iNewIndex As Integer = g_mUCInformationList.ListBox_Information.SelectedIndices(0) + iDirection

            If (iNewIndex > -1 AndAlso iNewIndex < iCount) Then
                g_mUCInformationList.ListBox_Information.SelectedIndices.Clear()
                g_mUCInformationList.ListBox_Information.SelectedIndex = iNewIndex

                'Fix last line hiding behind scrollbar :(
                If (iNewIndex = iCount - 1) Then
                    g_mUCInformationList.ListBox_Information.TopIndex = iNewIndex
                End If
            End If
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    If (g_mUCInformationList.g_mFormMain.g_ClassTabControl IsNot Nothing) Then
                        RemoveHandler g_mUCInformationList.g_mFormMain.g_ClassTabControl.OnTextEditorTabDetailsAction, AddressOf OnTextEditorTabDetailsAction
                        RemoveHandler g_mUCInformationList.g_mFormMain.g_ClassTabControl.OnTextEditorTabDetailsMove, AddressOf OnTextEditorTabDetailsMove
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
