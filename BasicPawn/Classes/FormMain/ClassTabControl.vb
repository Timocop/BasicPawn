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
Imports ICSharpCode.TextEditor.Document

Public Class ClassTabControl
    Implements IDisposable

    Private g_mFormMain As FormMain
    Private g_bIgnoreOnTabSelected As Boolean = False
    Private g_iOldActiveIndex As Integer = -1
    Private g_bIsLoadingEntries As Boolean = False

    Private g_iFreezePaintCounter As Integer = 0


    Public Sub New(f As FormMain)
        g_mFormMain = f

        AddHandler g_mFormMain.TabControl_SourceTabs.SelectedIndexChanged, AddressOf OnTabSelected
    End Sub

    Public Sub Init()
        g_mFormMain.TabControl_SourceTabs.TabPages.Clear()
        AddTab(True, True, False)
    End Sub

    ReadOnly Property m_IsLoadingEntries As Boolean
        Get
            Return g_bIsLoadingEntries
        End Get
    End Property


    ReadOnly Property m_ActiveTab As SourceTabPage
        Get
            Return DirectCast(g_mFormMain.TabControl_SourceTabs.SelectedTab, SourceTabPage)
        End Get
    End Property

    ReadOnly Property m_ActiveTabIndex As Integer
        Get
            Return g_mFormMain.TabControl_SourceTabs.SelectedIndex
        End Get
    End Property

    ReadOnly Property m_Tab(iIndex As Integer) As SourceTabPage
        Get
            Return DirectCast(g_mFormMain.TabControl_SourceTabs.TabPages(iIndex), SourceTabPage)
        End Get
    End Property

    ReadOnly Property m_TabsCount As Integer
        Get
            Return g_mFormMain.TabControl_SourceTabs.TabCount
        End Get
    End Property

    Public Sub AddTab(Optional bSelect As Boolean = False, Optional bIncludeTemplate As Boolean = False, Optional bChanged As Boolean = False)
        Try
            FreezePaint(g_mFormMain.SplitContainer_ToolboxAndEditor)

            Dim mTabPage As New SourceTabPage(g_mFormMain)
            mTabPage.m_Changed = bChanged

            If (bIncludeTemplate) Then
                mTabPage.m_TextEditor.Document.TextContent = My.Resources.SourcePawnOldTemplate
            End If

            g_mFormMain.TabControl_SourceTabs.TabPages.Add(mTabPage)
            g_mFormMain.g_ClassSyntaxTools.UpdateTextEditorSyntax()

            If (bSelect OrElse m_TabsCount < 2) Then
                SelectTab(m_TabsCount - 1)
            End If
        Finally
            UnfreezePaint(g_mFormMain.SplitContainer_ToolboxAndEditor)
        End Try
    End Sub

    Public Function RemoveTab(iIndex As Integer, bPrompSave As Boolean) As Boolean
        Try
            FreezePaint(g_mFormMain.SplitContainer_ToolboxAndEditor)

            If (bPrompSave AndAlso PromptSaveTab(iIndex)) Then
                Return False
            End If

            g_iOldActiveIndex = -1

            Dim mTabPage = m_Tab(iIndex)
            mTabPage.Dispose()
            mTabPage = Nothing

            If (m_TabsCount < 1) Then
                AddTab(False)
            End If

            If (iIndex > m_TabsCount - 1) Then
                SelectTab(m_TabsCount - 1)
            Else
                SelectTab(iIndex)
            End If

            Return True
        Finally
            UnfreezePaint(g_mFormMain.SplitContainer_ToolboxAndEditor)
        End Try
    End Function

    Public Sub SwapTabs(iFromIndex As Integer, iToIndex As Integer)
        Try
            FreezePaint(g_mFormMain.SplitContainer_ToolboxAndEditor)

            g_bIgnoreOnTabSelected = True

            SaveLoadTabEntries(iFromIndex, ENUM_TAB_CONFIG.SAVE)
            SaveLoadTabEntries(iToIndex, ENUM_TAB_CONFIG.SAVE)

            Dim mFrom As SourceTabPage = m_Tab(iFromIndex)
            g_mFormMain.TabControl_SourceTabs.TabPages.Remove(mFrom)
            g_mFormMain.TabControl_SourceTabs.TabPages.Insert(iToIndex, mFrom)

            g_bIgnoreOnTabSelected = False

            SelectTab(iToIndex)
        Finally
            UnfreezePaint(g_mFormMain.SplitContainer_ToolboxAndEditor)
        End Try
    End Sub

    Public Sub SelectTab(iIndex As Integer)
        Try
            FreezePaint(g_mFormMain.SplitContainer_ToolboxAndEditor)

            If (g_iOldActiveIndex > -1) Then
                SaveLoadTabEntries(g_iOldActiveIndex, ENUM_TAB_CONFIG.SAVE)
                m_Tab(g_iOldActiveIndex).m_HandlersEnabled = False
            End If

            g_iOldActiveIndex = iIndex

            If (iIndex > -1) Then
                SaveLoadTabEntries(iIndex, ENUM_TAB_CONFIG.LOAD)
                m_Tab(iIndex).m_HandlersEnabled = True

                g_mFormMain.TabControl_SourceTabs.SelectTab(iIndex)
                g_mFormMain.g_ClassSyntaxTools.UpdateTextEditorSyntax()
            End If
        Finally
            UnfreezePaint(g_mFormMain.SplitContainer_ToolboxAndEditor)
        End Try
    End Sub


    ''' <summary>
    ''' Opens a new source file
    ''' </summary>
    ''' <param name="iIndex"></param>
    ''' <param name="sPath"></param>
    ''' <param name="bIgnoreSavePrompt">If true, the new file will be opened without prompting to save the changed source</param>
    ''' <returns></returns>
    Public Function OpenFileTab(iIndex As Integer, sPath As String, Optional bIgnoreSavePrompt As Boolean = False) As Boolean
        If (Not bIgnoreSavePrompt AndAlso PromptSaveTab(iIndex)) Then
            Return False
        End If

        If (String.IsNullOrEmpty(sPath) OrElse Not IO.File.Exists(sPath)) Then
            m_Tab(iIndex).m_ClassLineState.m_IgnoreUpdates = True
            m_Tab(iIndex).m_File = ""
            m_Tab(iIndex).m_TextEditor.Document.TextContent = ""

            m_Tab(iIndex).m_Changed = False
            m_Tab(iIndex).m_AutocompleteItems = Nothing

            SaveLoadTabEntries(iIndex, ENUM_TAB_CONFIG.LOAD)

            m_Tab(iIndex).m_ClassLineState.m_IgnoreUpdates = False

            g_mFormMain.PrintInformation("[INFO]", "User created a new source file")
            Return False
        End If


        m_Tab(iIndex).m_ClassLineState.m_IgnoreUpdates = True

        m_Tab(iIndex).m_File = sPath
        m_Tab(iIndex).m_TextEditor.Document.TextContent = IO.File.ReadAllText(sPath)

        m_Tab(iIndex).m_Changed = False
        m_Tab(iIndex).m_AutocompleteItems = Nothing

        SaveLoadTabEntries(iIndex, ENUM_TAB_CONFIG.LOAD)

        m_Tab(iIndex).m_ClassLineState.m_IgnoreUpdates = False

        g_mFormMain.PrintInformation("[INFO]", "User opened a new file: " & sPath)
        Return True
    End Function

    ''' <summary>
    ''' Saves a source file
    ''' </summary>
    ''' <param name="iIndex"></param>
    ''' <param name="bSaveAs">Force to use a new file using SaveFileDialog</param>
    Public Sub SaveFileTab(iIndex As Integer, Optional bSaveAs As Boolean = False)
        SaveLoadTabEntries(iIndex, ENUM_TAB_CONFIG.SAVE)

        If (bSaveAs OrElse String.IsNullOrEmpty(m_Tab(iIndex).m_File) OrElse Not IO.File.Exists(m_Tab(iIndex).m_File)) Then
            Using i As New SaveFileDialog
                i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|Pawn (Not fully supported)|*.pwn;*.p|AMX Mod X|*.sma|All files|*.*"
                i.FileName = m_Tab(iIndex).m_File

                If (i.ShowDialog = DialogResult.OK) Then
                    m_Tab(iIndex).m_File = i.FileName

                    m_Tab(iIndex).m_Changed = False
                    m_Tab(iIndex).m_ClassLineState.SaveStates(m_Tab(iIndex).m_TextEditor)
                    m_Tab(iIndex).m_TextEditor.Refresh()

                    g_mFormMain.PrintInformation("[INFO]", "User saved file to: " & m_Tab(iIndex).m_File)
                    IO.File.WriteAllText(m_Tab(iIndex).m_File, m_Tab(iIndex).m_TextEditor.Document.TextContent)

                    g_mFormMain.ShowPingFlash()
                End If
            End Using
        Else
            m_Tab(iIndex).m_Changed = False
            m_Tab(iIndex).m_ClassLineState.SaveStates(m_Tab(iIndex).m_TextEditor)
            m_Tab(iIndex).m_TextEditor.Refresh()

            g_mFormMain.PrintInformation("[INFO]", "User saved file to: " & m_Tab(iIndex).m_File)
            IO.File.WriteAllText(m_Tab(iIndex).m_File, m_Tab(iIndex).m_TextEditor.Document.TextContent)

            g_mFormMain.ShowPingFlash()
        End If
    End Sub

    ''' <summary>
    ''' If the code has been changed it will prompt the user and saves the source. The user can abort the saving.
    ''' </summary>
    ''' <param name="iIndex"></param>
    ''' <param name="bAlwaysPrompt">If true, always show MessageBox even if the code didnt change</param>
    ''' <param name="bAlwaysYes">If true, ignores MessageBox prompt</param>
    ''' <returns>False if saved, otherwise canceled.</returns>
    Public Function PromptSaveTab(iIndex As Integer, Optional bAlwaysPrompt As Boolean = False, Optional bAlwaysYes As Boolean = False) As Boolean
        SaveLoadTabEntries(iIndex, ENUM_TAB_CONFIG.SAVE)

        If (Not bAlwaysPrompt AndAlso Not m_Tab(iIndex).m_Changed) Then
            Return False
        End If

        Dim sFilename As String = ""
        If (Not String.IsNullOrEmpty(m_Tab(iIndex).m_File) AndAlso IO.File.Exists(m_Tab(iIndex).m_File)) Then
            sFilename = String.Format(" ({0})", IO.Path.GetFileName(m_Tab(iIndex).m_File))
        End If

        Select Case (If(bAlwaysYes, DialogResult.Yes, MessageBox.Show(String.Format("Do you want to save your work?{0}", sFilename), "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)))
            Case DialogResult.Yes
                If (String.IsNullOrEmpty(m_Tab(iIndex).m_File) OrElse Not IO.File.Exists(m_Tab(iIndex).m_File)) Then
                    Using i As New SaveFileDialog
                        i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|Pawn (Not fully supported)|*.pwn;*.p|AMX Mod X|*.sma|All files|*.*"
                        i.FileName = m_Tab(iIndex).m_File

                        If (i.ShowDialog = DialogResult.OK) Then
                            m_Tab(iIndex).m_File = i.FileName

                            m_Tab(iIndex).m_Changed = False
                            m_Tab(iIndex).m_ClassLineState.SaveStates(m_Tab(iIndex).m_TextEditor)
                            m_Tab(iIndex).m_TextEditor.Refresh()

                            g_mFormMain.PrintInformation("[INFO]", "User saved file to: " & m_Tab(iIndex).m_File)
                            IO.File.WriteAllText(m_Tab(iIndex).m_File, m_Tab(iIndex).m_TextEditor.Document.TextContent)

                            Return False
                        Else
                            Return True
                        End If
                    End Using
                Else
                    m_Tab(iIndex).m_Changed = False
                    m_Tab(iIndex).m_ClassLineState.SaveStates(m_Tab(iIndex).m_TextEditor)
                    m_Tab(iIndex).m_TextEditor.Refresh()

                    g_mFormMain.PrintInformation("[INFO]", "User saved file to: " & m_Tab(iIndex).m_File)
                    IO.File.WriteAllText(m_Tab(iIndex).m_File, m_Tab(iIndex).m_TextEditor.Document.TextContent)

                    Return False
                End If

            Case DialogResult.No
                Return False

            Case Else
                Return True

        End Select
    End Function



    Enum ENUM_TAB_CONFIG
        SAVE
        LOAD
    End Enum

    Private Sub SaveLoadTabEntries(iIndex As Integer, i As ENUM_TAB_CONFIG)
        Select Case (i)
            Case ENUM_TAB_CONFIG.SAVE
                m_Tab(iIndex).m_AutocompleteItems = g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.ToArray

            Case Else
                Try
                    g_bIsLoadingEntries = True

                    'Stop all threads
                    'TODO: For some reason it locks the *.xshd file, need fix! 
                    'FIX: Using 'UpdateSyntaxFile' in the UI thread seems to solve this problem... but why, im using |SyncLock|, |Using| etc.?!
                    g_mFormMain.g_ClassSyntaxUpdater.StopThread()
                    g_mFormMain.g_ClassAutocompleteUpdater.StopUpdate()

                    'Autocomplete 
                    g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.DoSync(
                            Sub()
                                g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.Clear()
                                If (m_Tab(iIndex).m_AutocompleteItems IsNot Nothing) Then
                                    g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.AddRange(m_Tab(iIndex).m_AutocompleteItems)
                                End If
                            End Sub)

                    g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete("")
                    g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.CurrentMethod = ""
                    g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
                    g_mFormMain.g_mUCObjectBrowser.StartUpdate()

                    g_mFormMain.g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)
                    g_mFormMain.g_ClassSyntaxUpdater.StartThread()
                Finally
                    g_bIsLoadingEntries = False
                End Try
        End Select
    End Sub

    Private Sub OnTabSelected(sender As Object, e As EventArgs)
        If (g_bIgnoreOnTabSelected) Then
            Return
        End If

        g_bIgnoreOnTabSelected = True
        SelectTab(m_ActiveTabIndex)
        g_bIgnoreOnTabSelected = False
    End Sub



    Private Sub FreezePaint(c As Control)
        g_iFreezePaintCounter += 1
        ClassTools.ClassForms.SuspendDrawing(c)
    End Sub

    Private Sub UnfreezePaint(c As Control)
        If (g_iFreezePaintCounter > 0) Then
            g_iFreezePaintCounter -= 1

            If (g_iFreezePaintCounter = 0) Then
                ClassTools.ClassForms.ResumeDrawing(c)
            End If
        End If
    End Sub

    Public Class SourceTabPage
        Inherits TabPage

        Private g_mFormMain As FormMain

        Private g_sText As String = "Unnamed"
        Private g_bTextChanged As Boolean = False

        Private g_sFile As String = ""
        Private g_mAutocompleteItems As FormMain.STRUC_AUTOCOMPLETE()
        Private g_mSourceTextEditor As TextEditorControlEx
        Private g_bEnabled As Boolean = True

        Public Sub New(f As FormMain)
            g_mFormMain = f
            CreateTextEditor()

            ClassControlStyle.UpdateControls(Me)
        End Sub

        Public Sub New(f As FormMain, sText As String)
            g_mFormMain = f
            m_Text = sText

            CreateTextEditor()

            ClassControlStyle.UpdateControls(Me)
        End Sub

        Public Sub New(f As FormMain, sText As String, bChanged As Boolean)
            g_mFormMain = f
            m_Text = sText
            m_Changed = bChanged

            CreateTextEditor()

            ClassControlStyle.UpdateControls(Me)
        End Sub

        Private Sub AddHandlers()
            RemoveHandlers()

            AddHandler g_mSourceTextEditor.g_eProcessCmdKey, AddressOf TextEditorControl_Source_ProcessCmdKey

            AddHandler g_mSourceTextEditor.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord

            AddHandler g_mSourceTextEditor.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete

            AddHandler g_mSourceTextEditor.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete

            AddHandler g_mSourceTextEditor.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete

            AddHandler g_mSourceTextEditor.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete

            AddHandler g_mSourceTextEditor.TextChanged, AddressOf TextEditorControl_Source_TextChanged
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextChanged, AddressOf TextEditorControl_Source_TextChanged
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.TextChanged, AddressOf TextEditorControl_Source_TextChanged

            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectionChanged, AddressOf TextEditorControl_Source_UpdateInfo
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged, AddressOf TextEditorControl_Source_UpdateInfo

            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Document.LineLengthChanged, AddressOf TextEditorControl_DetectLineLenghtChange
            AddHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Document.LineCountChanged, AddressOf TextEditorControl_DetectLineCountChange
        End Sub

        Private Sub RemoveHandlers()
            RemoveHandler g_mSourceTextEditor.g_eProcessCmdKey, AddressOf TextEditorControl_Source_ProcessCmdKey

            RemoveHandler g_mSourceTextEditor.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord

            RemoveHandler g_mSourceTextEditor.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete

            RemoveHandler g_mSourceTextEditor.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete

            RemoveHandler g_mSourceTextEditor.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete

            RemoveHandler g_mSourceTextEditor.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete

            RemoveHandler g_mSourceTextEditor.TextChanged, AddressOf TextEditorControl_Source_TextChanged
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextChanged, AddressOf TextEditorControl_Source_TextChanged
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.TextChanged, AddressOf TextEditorControl_Source_TextChanged

            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectionChanged, AddressOf TextEditorControl_Source_UpdateInfo
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged, AddressOf TextEditorControl_Source_UpdateInfo

            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Document.LineLengthChanged, AddressOf TextEditorControl_DetectLineLenghtChange
            RemoveHandler g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Document.LineCountChanged, AddressOf TextEditorControl_DetectLineCountChange
        End Sub

        Private Sub CreateTextEditor()
            g_mSourceTextEditor = New TextEditorControlEx
            g_mSourceTextEditor.ContextMenuStrip = g_mFormMain.ContextMenuStrip_RightClick
            g_mSourceTextEditor.IsIconBarVisible = True
            g_mSourceTextEditor.ShowTabs = True
            g_mSourceTextEditor.ShowVRuler = False
            g_mSourceTextEditor.HideMouseCursor = True

            g_mSourceTextEditor.Margin = New Padding(0)
            g_mSourceTextEditor.Padding = New Padding(0)

            g_mSourceTextEditor.ActiveTextAreaControl.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont

            g_mSourceTextEditor.Parent = Me
            g_mSourceTextEditor.Dock = DockStyle.Fill

            g_mSourceTextEditor.Document.FoldingManager.FoldingStrategy = New VariXFolding()
            g_mSourceTextEditor.Document.FoldingManager.UpdateFoldings(Nothing, Nothing)

            g_mSourceTextEditor.Visible = True
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            Try
                If (disposing) Then
                    RemoveHandlers()

                    If (g_mSourceTextEditor IsNot Nothing) Then
                        g_mSourceTextEditor.Dispose()
                        g_mSourceTextEditor = Nothing
                    End If
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Public Property m_HandlersEnabled As Boolean
            Get
                Return g_bEnabled
            End Get
            Set(value As Boolean)
                g_bEnabled = value

                If (g_bEnabled) Then
                    AddHandlers()
                Else
                    RemoveHandlers()
                End If
            End Set
        End Property

        Public Property m_File As String
            Get
                Return g_sFile
            End Get
            Set(value As String)
                g_sFile = value

                m_Text = IO.Path.GetFileName(g_sFile)
                Text = IO.Path.GetFileName(g_sFile)
            End Set
        End Property

        Public ReadOnly Property m_TextEditor As TextEditorControlEx
            Get
                Return g_mSourceTextEditor
            End Get
        End Property

        Public Property m_AutocompleteItems As FormMain.STRUC_AUTOCOMPLETE()
            Get
                Return g_mAutocompleteItems
            End Get
            Set(value As FormMain.STRUC_AUTOCOMPLETE())
                g_mAutocompleteItems = value
            End Set
        End Property

        Public ReadOnly Property m_ClassLineState As ClassTextEditorTools.ClassLineState
            Get
                Return g_mFormMain.g_ClassLineState
            End Get
        End Property



        Public Property m_Changed As Boolean
            Get
                Return g_bTextChanged
            End Get
            Set(value As Boolean)
                g_bTextChanged = value
                Text = g_sText
            End Set
        End Property

        Public Property m_Text As String
            Get
                Return g_sText
            End Get
            Set(value As String)
                g_sText = value
                Text = g_sText
            End Set
        End Property



        Public Overrides Property Text As String
            Get
                Return g_sText & If(g_bTextChanged, "*"c, "")
            End Get
            Set(value As String)
                MyBase.Text = value
            End Set
        End Property


#Region "TextEditor Controls"
        Private Sub TextEditorControl_Source_ProcessCmdKey(ByRef bBlock As Boolean, ByRef iMsg As Message, iKeys As Keys)
            Select Case (iKeys)

                'Duplicate Line/Word
                Case Keys.Control Or Keys.D
                    bBlock = True

                    If (g_mSourceTextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                        Dim sText As String = g_mSourceTextEditor.ActiveTextAreaControl.SelectionManager.SelectedText
                        Dim iCaretOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset

                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iCaretOffset, sText)
                    Else
                        Dim iCaretOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset
                        Dim iLineOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iCaretOffset).Offset
                        Dim iLineLen As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iCaretOffset).Length

                        g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iLineOffset, g_mSourceTextEditor.ActiveTextAreaControl.Document.GetText(iLineOffset, iLineLen) & Environment.NewLine)
                    End If

                    g_mSourceTextEditor.Refresh()

                'Paste Autocomplete
                Case Keys.Control Or Keys.Enter
                    bBlock = True

                    g_mSourceTextEditor.Document.UndoStack.StartUndoGroup()

                    Dim iOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset
                    Dim iPosition As Integer = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                    Dim iLineOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iOffset).Offset
                    Dim iLineLen As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iOffset).Length
                    Dim iLineNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iOffset).LineNumber

                    Dim sFunctionName As String = Regex.Match(g_mSourceTextEditor.ActiveTextAreaControl.Document.GetText(iLineOffset, iPosition), "(\b[a-zA-Z0-9_]+\b(\.|\:){0,1}(\b[a-zA-Z0-9_]+\b){0,1})$").Value

                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition - sFunctionName.Length
                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Remove(iOffset - sFunctionName.Length, sFunctionName.Length)

                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column

                    Dim bIsEmpty = Regex.IsMatch(g_mSourceTextEditor.ActiveTextAreaControl.Document.GetText(iLineOffset, iLineLen - sFunctionName.Length), "^\s*$")

                    Dim struc As FormMain.STRUC_AUTOCOMPLETE = g_mFormMain.g_mUCAutocomplete.GetSelectedItem()

                    If (struc IsNot Nothing) Then
                        Select Case (True)
                            Case (struc.mType And FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) = FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD
                                If (bIsEmpty) Then
                                    Dim iLineOffsetNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Offset
                                    Dim iLineLenNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Length
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Remove(iLineOffsetNum, iLineLenNum)

                                    Dim sNewInputFirst As String = "public" & struc.sFullFunctionName.Remove(0, "forward".Length) & Environment.NewLine &
                                                                   "{" & Environment.NewLine &
                                                                   vbTab
                                    Dim sNewInputLast As String = Environment.NewLine &
                                                                   "}"

                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iLineOffsetNum, sNewInputFirst & sNewInputLast)

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Line = iLineNum + 2
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = 1
                                Else
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName)

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Length
                                End If

                            Case (struc.mType And FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) = FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG
                                If (bIsEmpty) Then
                                    Dim iLineOffsetNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Offset
                                    Dim iLineLenNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Length
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Remove(iLineOffsetNum, iLineLenNum)

                                    Dim sNewInputFirst As String = "public" & struc.sFullFunctionName.Remove(0, "functag".Length) & Environment.NewLine &
                                                                   "{" & Environment.NewLine &
                                                                   vbTab
                                    Dim sNewInputLast As String = Environment.NewLine &
                                                                   "}"

                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iLineOffsetNum, sNewInputFirst & sNewInputLast)

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Line = iLineNum + 2
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = 1
                                Else
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName)

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Length
                                End If

                            Case (struc.mType And FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) = FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM,
                                     (struc.mType And FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) = FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET,
                                     (struc.mType And FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) = FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF
                                If (bIsEmpty) Then
                                    Dim iLineOffsetNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Offset
                                    Dim iLineLenNum As Integer = g_mSourceTextEditor.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Length
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Remove(iLineOffsetNum, iLineLenNum)

                                    Dim sNewInputFirst As String = struc.sFunctionName & Environment.NewLine &
                                                                   "{" & Environment.NewLine &
                                                                   vbTab
                                    Dim sNewInputLast As String = Environment.NewLine &
                                                                   "}"

                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iLineOffsetNum, sNewInputFirst & sNewInputLast)

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Line = iLineNum + 2
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = 1
                                Else
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName)

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Length
                                End If


                            Case (struc.mType And FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE) = FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE,
                                   (struc.mType And FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR) = FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR
                                g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName)

                                iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Length

                            Case (struc.mType And FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) = FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM
                                If (ClassSettings.g_iSettingsFullEnumAutocomplete OrElse struc.sFunctionName.IndexOf("."c) < 0) Then
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName.Replace("."c, ":"c))

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Length
                                Else
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName.Remove(0, struc.sFunctionName.IndexOf("."c) + 1))

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Remove(0, struc.sFunctionName.IndexOf("."c) + 1).Length
                                End If

                            Case (struc.mType And FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP,
                                   (struc.mType And FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = FormMain.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                                If (struc.sFunctionName.IndexOf("."c) > -1 AndAlso sFunctionName.IndexOf("."c) > -1 AndAlso Not sFunctionName.StartsWith(struc.sFunctionName)) Then
                                    Dim sNewInput As String = String.Format("{0}.{1}",
                                                                        sFunctionName.Remove(sFunctionName.LastIndexOf("."c), sFunctionName.Length - sFunctionName.LastIndexOf("."c)),
                                                                        struc.sFunctionName.Remove(0, struc.sFunctionName.IndexOf("."c) + 1))
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, sNewInput)

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length
                                Else
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName.Remove(0, struc.sFunctionName.IndexOf("."c) + 1))

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Remove(0, struc.sFunctionName.IndexOf("."c) + 1).Length
                                End If

                            Case Else
                                If (ClassSettings.g_iSettingsFullMethodAutocomplete) Then
                                    Dim sNewInput As String = struc.sFullFunctionName.Remove(0, Regex.Match(struc.sFullFunctionName, "^(?<Useless>.*?)\b[a-zA-Z0-9_]+\b\s*\(").Groups("Useless").Length)
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, sNewInput)

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length
                                Else
                                    Dim sNewInput As String = struc.sFunctionName.Remove(0, Regex.Match(struc.sFunctionName, "^(?<Useless>.*?)\b[a-zA-Z0-9_]+\b\s*\(").Groups("Useless").Length)
                                    g_mSourceTextEditor.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, String.Format("{0}()", sNewInput))

                                    iPosition = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                    g_mSourceTextEditor.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length + 1
                                End If

                        End Select
                    End If

                    g_mSourceTextEditor.Document.UndoStack.EndUndoGroup()
                    g_mSourceTextEditor.Refresh()

                'Autocomplete up
                Case Keys.Control Or Keys.Up
                    If (g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.SelectedItems.Count < 1) Then
                        Return
                    End If

                    Dim iListViewCount As Integer = g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items.Count

                    Dim iNewIndex As Integer = g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.SelectedItems(0).Index - 1

                    If (iNewIndex > -1 AndAlso iNewIndex < iListViewCount) Then
                        g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items(iNewIndex).Selected = True
                        g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items(iNewIndex).EnsureVisible()
                    End If

                    bBlock = True

                'Autocomplete Down
                Case Keys.Control Or Keys.Down
                    If (g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.SelectedItems.Count < 1) Then
                        Return
                    End If

                    Dim iListViewCount As Integer = g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items.Count

                    Dim iNewIndex As Integer = g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.SelectedItems(0).Index + 1

                    If (iNewIndex > -1 AndAlso iNewIndex < iListViewCount) Then
                        g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items(iNewIndex).Selected = True
                        g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList.Items(iNewIndex).EnsureVisible()
                    End If

                    bBlock = True
            End Select
        End Sub

        Private Sub TextEditorControl_Source_UpdateInfo(sender As Object, e As EventArgs)
            g_mFormMain.ToolStripStatusLabel_EditorLine.Text = "L: " & g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Line + 1
            g_mFormMain.ToolStripStatusLabel_EditorCollum.Text = "C: " & g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Column
            g_mFormMain.ToolStripStatusLabel_EditorSelectedCount.Text = "S: " & g_mSourceTextEditor.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText.Length
        End Sub

        Private Sub TextEditorControl_DetectLineLenghtChange(sender As Object, e As LineLengthChangeEventArgs)
            Dim iTotalLines As Integer = g_mSourceTextEditor.Document.TotalNumberOfLines

            If (e.LineSegment.IsDeleted OrElse e.LineSegment.Length < 0) Then
                Return
            End If

            If (e.LineSegment.LineNumber > iTotalLines) Then
                Return
            End If

            m_ClassLineState.m_LineState(g_mSourceTextEditor, e.LineSegment.LineNumber) = ClassTextEditorTools.ClassLineState.LineStateBookmark.ENUM_BOOKMARK_TYPE.CHANGED
        End Sub

        Private Sub TextEditorControl_DetectLineCountChange(sender As Object, e As LineCountChangeEventArgs)
            Dim iTotalLines As Integer = g_mSourceTextEditor.Document.TotalNumberOfLines

            If (e.LinesMoved > -1) Then
                For i = 0 To e.LinesMoved
                    If (e.LineStart + i > iTotalLines) Then
                        Return
                    End If

                    m_ClassLineState.m_LineState(g_mSourceTextEditor, e.LineStart + i) = ClassTextEditorTools.ClassLineState.LineStateBookmark.ENUM_BOOKMARK_TYPE.CHANGED
                Next
            End If
        End Sub

        Private Sub TextEditorControl_Source_UpdateAutocomplete(sender As Object, e As Object)
            Static iOldCaretPos As Integer = 0

            Dim iOffset As Integer = g_mSourceTextEditor.ActiveTextAreaControl.TextArea.Caret.Offset

            If (iOldCaretPos = iOffset) Then
                Return
            End If

            iOldCaretPos = iOffset

            Dim sFunctionName As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True)

            If (g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete(sFunctionName) < 1) Then
                sFunctionName = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(False)

                g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete(sFunctionName)
            Else
                g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
            End If
        End Sub

        Private Sub TextEditorControl_Source_TextChanged(sender As Object, e As EventArgs)
            If (Not g_mFormMain.g_ClassTabControl.m_IsLoadingEntries) Then
                g_mFormMain.g_ClassTabControl.m_ActiveTab.m_Changed = True
            End If
        End Sub

        Private Sub TextEditorControl_Source_DoubleClickMarkWord(sender As Object, e As MouseEventArgs)
            If (Not ClassSettings.g_iSettingsDoubleClickMark) Then
                Return
            End If

            g_mFormMain.g_ClassTextEditorTools.MarkSelectedWord()
        End Sub
#End Region



#Region "TextEditor Folding Code"

        ''' <summary>
        ''' The class to generate the foldings, it implements ICSharpCode.TextEditor.Document.IFoldingStrategy
        ''' </summary>
        Private Class VariXFolding
            Implements IFoldingStrategy
            ''' <summary>
            ''' Generates the foldings for our document.
            ''' </summary>
            ''' <param name="document">The current document.</param>
            ''' <param name="fileName">The filename of the document.</param>
            ''' <param name="parseInformation">Extra parse information, not used in this sample.</param>
            ''' <returns>A list of FoldMarkers.</returns>
            Public Function GenerateFoldMarkers(document As IDocument, fileName As String, parseInformation As Object) As List(Of FoldMarker) Implements IFoldingStrategy.GenerateFoldMarkers
                Dim list As New List(Of FoldMarker)()

                'If ((Tools.WordCount(document.TextContent, "{") + Tools.WordCount(document.TextContent, "}")) Mod 2 <> 0) Then
                '    Return list
                'End If

                'Dim sourceAnalysis As New SyntaxCharReader(document.TextContent)

                Dim iMaxLevels As Integer = 0
                Dim i As Integer = 0
                While True
                    i = document.TextContent.IndexOf("{"c, i)
                    If (i < 0) Then
                        Exit While
                    End If
                    i += 1
                    iMaxLevels += 1
                End While

                If (iMaxLevels < 1) Then
                    Return list
                End If

                Dim iLevels As Integer() = New Integer(iMaxLevels) {}
                Dim iCurrentLevel As Integer = 0

                For i = 0 To document.TextContent.Length - 1
                    'If (sourceAnalysis.InNonCode(i)) Then
                    '    Continue For
                    'End If

                    'Dim iCurrentLevel As Integer = sourceAnalysis.GetBraceLevel(i)

                    Select Case (document.TextContent(i))
                        Case "{"c
                            iCurrentLevel += 1
                            If ((iCurrentLevel - 1) < 0) Then
                                Continue For
                            End If

                            iLevels(iCurrentLevel - 1) = If(i > 0, i - 1, i)
                        Case "}"c
                            iCurrentLevel -= 1
                            If (iCurrentLevel < 0) Then
                                Continue For
                            End If

                            'Debug.WriteLine(document.TextContent.Substring(iLevels(iCurrentLevel), i - iLevels(iCurrentLevel)))

                            Dim iLineStart = document.GetLineNumberForOffset(iLevels(iCurrentLevel))
                            Dim iColumStart = document.GetLineSegment(iLineStart).Length
                            Dim iLineEnd = document.GetLineNumberForOffset(i)
                            Dim iColumEnd = document.GetLineSegment(iLineEnd).Length

                            If (iLineStart = iLineEnd) Then
                                Continue For
                            End If

                            list.Add(New FoldMarker(document, iLineStart, iColumStart, iLineEnd, iColumEnd))
                    End Select
                Next

                '' Create foldmarkers for the whole document, enumerate through every line.
                'For i As Integer = 0 To document.TotalNumberOfLines - 1
                '    ' Get the text of current line.
                '    Dim text As String = document.GetText(document.GetLineSegment(i))

                '    If text.StartsWith("def") Then
                '        ' Look for method starts
                '        start = i
                '    End If
                '    If text.StartsWith("enddef;") Then
                '        ' Look for method endings
                '        ' Add a new FoldMarker to the list.
                '        ' document = the current document
                '        ' start = the start line for the FoldMarker
                '        ' document.GetLineSegment(start).Length = the ending of the current line = the start column of our foldmarker.
                '        ' i = The current line = end line of the FoldMarker.
                '        ' 7 = The end column
                '        list.Add(New FoldMarker(document, start, document.GetLineSegment(start).Length, i, 7))
                '    End If
                'Next

                Return list
            End Function
        End Class
#End Region

    End Class

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                RemoveHandler g_mFormMain.TabControl_SourceTabs.SelectedIndexChanged, AddressOf OnTabSelected
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
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