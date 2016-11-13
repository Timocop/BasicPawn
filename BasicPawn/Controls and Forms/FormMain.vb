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


Imports System.ComponentModel
Imports System.Text
Imports System.Text.RegularExpressions
Imports ICSharpCode.TextEditor
Imports ICSharpCode.TextEditor.Document


Public Class FormMain
    Public g_ClassSyntaxUpdater As ClassSyntaxUpdater
    Public g_ClassSyntaxTools As ClassSyntaxTools
    Public g_ClassAutocompleteUpdater As ClassAutocompleteUpdater
    Public g_ClassTextEditorTools As ClassTextEditorTools
    Public g_ClassLineState As ClassTextEditorTools.ClassLineState
    Public g_ClassCustomHighlighting As ClassTextEditorTools.ClassCustomHighlighting

    Public g_mSourceSyntaxSourceAnalysis As ClassSyntaxTools.ClassSyntaxSourceAnalysis

    Public g_mUCAutocomplete As UCAutocomplete
    Public g_mUCInformationList As UCInformationList
    Public g_mUCObjectBrowser As UCObjectBrowser
    Public g_mUCToolTip As UCToolTip
    Public g_mFormDebugger As FormDebugger

    Private g_cDarkTextEditorBackgroundColor As Color = Color.FromArgb(255, 26, 26, 26)
    Private g_cDarkFormDetailsBackgroundColor As Color = Color.FromArgb(255, 24, 24, 24)
    Private g_cDarkFormBackgroundColor As Color = Color.FromArgb(255, 48, 48, 48)
    Private g_cDarkFormMenuBackgroundColor As Color = Color.FromArgb(255, 64, 64, 64)

    Public Class STRUC_AUTOCOMPLETE
        Public sInfo As String
        Public sFile As String
        Public mType As ENUM_TYPE_FLAGS
        Public sFunctionName As String
        Public sFullFunctionName As String

        Enum ENUM_TYPE_FLAGS
            NONE = 0
            DEBUG = (1 << 0)
            DEFINE = (1 << 1)
            [ENUM] = (1 << 2)
            FUNCENUM = (1 << 3)
            FUNCTAG = (1 << 4)
            STOCK = (1 << 5)
            [STATIC] = (1 << 6)
            [CONST] = (1 << 7)
            [PUBLIC] = (1 << 8)
            NATIVE = (1 << 9)
            FORWARD = (1 << 10)
            TYPESET = (1 << 11)
            METHODMAP = (1 << 12)
            TYPEDEF = (1 << 13)
            VARIABLE = (1 << 14)
            PUBLICVAR = (1 << 15)
            [PROPERTY] = (1 << 16)
            [FUNCTION] = (1 << 17)
        End Enum

        Public Function ParseTypeFullNames(sStr As String) As ENUM_TYPE_FLAGS
            Return ParseTypeNames(sStr.Split(New String() {" "}, 0))
        End Function

        Public Function ParseTypeNames(sStr As String()) As ENUM_TYPE_FLAGS
            Dim mTypes As ENUM_TYPE_FLAGS = ENUM_TYPE_FLAGS.NONE

            For i = 0 To sStr.Length - 1
                Select Case (sStr(i))
                    Case "debug" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.DEBUG)
                    Case "define" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.DEFINE)
                    Case "enum" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.ENUM)
                    Case "funcenum" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FUNCENUM)
                    Case "functag" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FUNCTAG)
                    Case "stock" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.STOCK)
                    Case "static" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.STATIC)
                    Case "const" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.CONST)
                    Case "public" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PUBLIC)
                    Case "native" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.NATIVE)
                    Case "forward" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FORWARD)
                    Case "typeset" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.TYPESET)
                    Case "methodmap" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.METHODMAP)
                    Case "typedef" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.TYPEDEF)
                    Case "variable" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.VARIABLE)
                    Case "publicvar" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PUBLICVAR)
                    Case "property" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PROPERTY)
                    Case "function" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FUNCTION)
                End Select
            Next

            Return mTypes
        End Function

        Public Function GetTypeNames() As String()
            Dim lNames As New List(Of String)

            If ((mType And ENUM_TYPE_FLAGS.DEBUG) = ENUM_TYPE_FLAGS.DEBUG) Then lNames.Add("debug")
            If ((mType And ENUM_TYPE_FLAGS.DEFINE) = ENUM_TYPE_FLAGS.DEFINE) Then lNames.Add("define")
            If ((mType And ENUM_TYPE_FLAGS.ENUM) = ENUM_TYPE_FLAGS.ENUM) Then lNames.Add("enum")
            If ((mType And ENUM_TYPE_FLAGS.FUNCENUM) = ENUM_TYPE_FLAGS.FUNCENUM) Then lNames.Add("funcenum")
            If ((mType And ENUM_TYPE_FLAGS.FUNCTAG) = ENUM_TYPE_FLAGS.FUNCTAG) Then lNames.Add("functag")
            If ((mType And ENUM_TYPE_FLAGS.STOCK) = ENUM_TYPE_FLAGS.STOCK) Then lNames.Add("stock")
            If ((mType And ENUM_TYPE_FLAGS.STATIC) = ENUM_TYPE_FLAGS.STATIC) Then lNames.Add("static")
            If ((mType And ENUM_TYPE_FLAGS.CONST) = ENUM_TYPE_FLAGS.CONST) Then lNames.Add("const")
            If ((mType And ENUM_TYPE_FLAGS.PUBLIC) = ENUM_TYPE_FLAGS.PUBLIC) Then lNames.Add("public")
            If ((mType And ENUM_TYPE_FLAGS.NATIVE) = ENUM_TYPE_FLAGS.NATIVE) Then lNames.Add("native")
            If ((mType And ENUM_TYPE_FLAGS.FORWARD) = ENUM_TYPE_FLAGS.FORWARD) Then lNames.Add("forward")
            If ((mType And ENUM_TYPE_FLAGS.TYPESET) = ENUM_TYPE_FLAGS.TYPESET) Then lNames.Add("typeset")
            If ((mType And ENUM_TYPE_FLAGS.METHODMAP) = ENUM_TYPE_FLAGS.METHODMAP) Then lNames.Add("methodmap")
            If ((mType And ENUM_TYPE_FLAGS.TYPEDEF) = ENUM_TYPE_FLAGS.TYPEDEF) Then lNames.Add("typedef")
            If ((mType And ENUM_TYPE_FLAGS.VARIABLE) = ENUM_TYPE_FLAGS.VARIABLE) Then lNames.Add("variable")
            If ((mType And ENUM_TYPE_FLAGS.PUBLICVAR) = ENUM_TYPE_FLAGS.PUBLICVAR) Then lNames.Add("publicvar")
            If ((mType And ENUM_TYPE_FLAGS.PROPERTY) = ENUM_TYPE_FLAGS.PROPERTY) Then lNames.Add("property")
            If ((mType And ENUM_TYPE_FLAGS.FUNCTION) = ENUM_TYPE_FLAGS.FUNCTION) Then lNames.Add("function")

            Return lNames.ToArray
        End Function

        Public Function GetTypeFullNames() As String
            Return String.Join(" ", GetTypeNames())
        End Function
    End Class



#Region "GUI Stuff"
    Private g_bCodeChanged As Boolean = False

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_ClassSyntaxUpdater = New ClassSyntaxUpdater(Me)
        g_ClassSyntaxTools = New ClassSyntaxTools(Me)
        g_ClassAutocompleteUpdater = New ClassAutocompleteUpdater(Me)
        g_ClassTextEditorTools = New ClassTextEditorTools(Me)
        g_ClassLineState = New ClassTextEditorTools.ClassLineState(Me)
        g_ClassCustomHighlighting = New ClassTextEditorTools.ClassCustomHighlighting(Me)

        ' Load other Forms/Controls
        g_mUCAutocomplete = New UCAutocomplete(Me)
        g_mUCAutocomplete.Parent = TabPage_Autocomplete
        g_mUCAutocomplete.Dock = DockStyle.Fill
        g_mUCAutocomplete.Show()

        g_mUCInformationList = New UCInformationList(Me)
        g_mUCInformationList.Parent = TabPage_Information
        g_mUCInformationList.Dock = DockStyle.Fill
        g_mUCInformationList.Show()

        g_mUCObjectBrowser = New UCObjectBrowser(Me)
        g_mUCObjectBrowser.Parent = TabPage_ObjectBrowser
        g_mUCObjectBrowser.Dock = DockStyle.Fill
        g_mUCObjectBrowser.Show()

        g_mUCToolTip = New UCToolTip(Me)
        g_mUCToolTip.Parent = TextEditorControl_Source
        g_mUCToolTip.BringToFront()
        g_mUCToolTip.Hide()

        SplitContainer1.SplitterDistance = SplitContainer1.Height - 175
    End Sub

    Public Sub ChangeTitle()
        If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile)) Then
            Me.Text = String.Format("{0} ({1}){2}", Application.ProductName, "Unnamed", If(g_bCodeChanged, "*"c, ""))
        Else
            Me.Text = String.Format("{0} ({1}){2}", Application.ProductName, IO.Path.GetFileName(ClassSettings.g_sConfigOpenSourceFile), If(g_bCodeChanged, "*"c, ""))
        End If

        ToolStripStatusLabel_CurrentConfig.Text = "Config: " & If(String.IsNullOrEmpty(ClassSettings.g_sConfigName), "Default", ClassSettings.g_sConfigName)
    End Sub

    Public Sub PrintInformation(sType As String, sMessage As String, Optional bClear As Boolean = False, Optional bShowInformationTab As Boolean = False)
        Me.BeginInvoke(
            Sub()
                If (g_mUCInformationList Is Nothing) Then
                    Return
                End If

                If (bClear) Then
                    g_mUCInformationList.ListBox_Information.Items.Clear()
                End If
                g_mUCInformationList.ListBox_Information.Items.Insert(0, String.Format("{0} ({1}) {2}", sType, Now.ToString, sMessage))
                ToolStripStatusLabel_LastInformation.Text = sMessage

                If (bShowInformationTab) Then
                    SplitContainer1.Panel2Collapsed = False
                    SplitContainer1.SplitterDistance = SplitContainer1.Height - 200
                    TabControl_Details.SelectTab(1)
                End If
            End Sub)
    End Sub

#End Region

#Region "TextEditor Folding Code"

    ''' <summary>
    ''' The class to generate the foldings, it implements ICSharpCode.TextEditor.Document.IFoldingStrategy
    ''' </summary>
    Public Class VariXFolding
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

#Region "Syntax Stuff"


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ToolStripStatusLabel_AppVersion.Text = String.Format("v.{0}", Application.ProductVersion)

        'Some control init
        ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndex = 0

        'Load Settings 
        ClassSettings.LoadSettings()

        'Load Syntax 
        g_ClassSyntaxTools.UpdateTextEditorSyntax()

        'Add Events
        AddHandler TextEditorControl_Source.g_eProcessCmdKey, AddressOf TextEditorControl_Source_ProcessCmdKey

        AddHandler TextEditorControl_Source.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_DoubleClickMarkWord

        AddHandler TextEditorControl_Source.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.TextArea.MouseClick, AddressOf TextEditorControl_Source_UpdateAutocomplete

        AddHandler TextEditorControl_Source.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.TextArea.MouseUp, AddressOf TextEditorControl_Source_UpdateAutocomplete

        AddHandler TextEditorControl_Source.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.TextArea.MouseDoubleClick, AddressOf TextEditorControl_Source_UpdateAutocomplete

        AddHandler TextEditorControl_Source.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.TextArea.KeyUp, AddressOf TextEditorControl_Source_UpdateAutocomplete

        AddHandler TextEditorControl_Source.KeyDown, AddressOf TextEditorControl_Source_KeyDown
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.KeyDown, AddressOf TextEditorControl_Source_KeyDown
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.TextArea.KeyDown, AddressOf TextEditorControl_Source_KeyDown

        AddHandler TextEditorControl_Source.TextChanged, AddressOf TextEditorControl_Source_TextChanged
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.TextChanged, AddressOf TextEditorControl_Source_TextChanged
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.TextArea.TextChanged, AddressOf TextEditorControl_Source_TextChanged

        AddHandler TextEditorControl_Source.ActiveTextAreaControl.TextArea.SelectionManager.SelectionChanged, AddressOf TextEditorControl_Source_UpdateInfo
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.PositionChanged, AddressOf TextEditorControl_Source_UpdateInfo

        AddHandler TextEditorControl_Source.ActiveTextAreaControl.TextArea.Document.LineLengthChanged, AddressOf TextEditorControl_DetectLineLenghtChange
        AddHandler TextEditorControl_Source.ActiveTextAreaControl.TextArea.Document.LineCountChanged, AddressOf TextEditorControl_DetectLineCountChange

        'Load source files via Arguments
        Dim sArgs As String() = Environment.GetCommandLineArgs
        For i = 1 To sArgs.Length - 1
            If (g_ClassTextEditorTools.OpenFile(sArgs(i), True)) Then
                Exit For
            End If
        Next

        'Update Autocomplete
        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

        'Update Folding
        TextEditorControl_Source.Document.FoldingManager.FoldingStrategy = New VariXFolding()
        TextEditorControl_Source.Document.FoldingManager.UpdateFoldings(Nothing, Nothing)

        'UpdateTextEditorControl1Colors()
        g_ClassSyntaxTools.UpdateFormColors()

        'Update Text Editor Settings
        TextEditorControl_Source.ActiveTextAreaControl.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
        TextEditorControl_Source.Refresh()

        g_ClassSyntaxUpdater.StartThread()
    End Sub

#End Region

#Region "TextEditor Controls"
    Private Sub TextEditorControl_Source_ProcessCmdKey(ByRef bBlock As Boolean, ByRef iMsg As Message, iKeys As Keys)
        Select Case (iKeys)

            'Duplicate Line/Word
            Case Keys.Control Or Keys.D
                bBlock = True

                If (TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                    Dim sText As String = TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectedText
                    Dim iCaretOffset As Integer = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Offset

                    TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iCaretOffset, sText)
                Else
                    Dim iCaretOffset As Integer = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Offset
                    Dim iLineOffset As Integer = TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iCaretOffset).Offset
                    Dim iLineLen As Integer = TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iCaretOffset).Length

                    TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iLineOffset, TextEditorControl_Source.ActiveTextAreaControl.Document.GetText(iLineOffset, iLineLen) & Environment.NewLine)
                End If

                TextEditorControl_Source.Refresh()

            'Paste Autocomplete
            Case Keys.Control Or Keys.Enter
                bBlock = True

                TextEditorControl_Source.Document.UndoStack.StartUndoGroup()

                Dim iOffset As Integer = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Offset
                Dim iPosition As Integer = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                Dim iLineOffset As Integer = TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iOffset).Offset
                Dim iLineLen As Integer = TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iOffset).Length
                Dim iLineNum As Integer = TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iOffset).LineNumber

                Dim sFunctionName As String = Regex.Match(TextEditorControl_Source.ActiveTextAreaControl.Document.GetText(iLineOffset, iPosition), "(\b[a-zA-Z0-9_]+\b(\.|\:){0,1}(\b[a-zA-Z0-9_]+\b){0,1})$").Value

                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = iPosition - sFunctionName.Length
                TextEditorControl_Source.ActiveTextAreaControl.Document.Remove(iOffset - sFunctionName.Length, sFunctionName.Length)

                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column

                Dim bIsEmpty = Regex.IsMatch(TextEditorControl_Source.ActiveTextAreaControl.Document.GetText(iLineOffset, iLineLen - sFunctionName.Length), "^\s*$")

                Dim struc As STRUC_AUTOCOMPLETE = g_mUCAutocomplete.GetSelectedItem()

                If (struc IsNot Nothing) Then
                    Select Case (True)
                        Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD
                            If (bIsEmpty) Then
                                Dim iLineOffsetNum As Integer = TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Offset
                                Dim iLineLenNum As Integer = TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Length
                                TextEditorControl_Source.ActiveTextAreaControl.Document.Remove(iLineOffsetNum, iLineLenNum)

                                Dim sNewInputFirst As String = "public" & struc.sFullFunctionName.Remove(0, "forward".Length) & Environment.NewLine &
                                                                   "{" & Environment.NewLine &
                                                                   vbTab
                                Dim sNewInputLast As String = Environment.NewLine &
                                                                   "}"

                                TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iLineOffsetNum, sNewInputFirst & sNewInputLast)

                                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Line = iLineNum + 2
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = 1
                            Else
                                TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName)

                                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Length
                            End If

                        Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG
                            If (bIsEmpty) Then
                                Dim iLineOffsetNum As Integer = TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Offset
                                Dim iLineLenNum As Integer = TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Length
                                TextEditorControl_Source.ActiveTextAreaControl.Document.Remove(iLineOffsetNum, iLineLenNum)

                                Dim sNewInputFirst As String = "public" & struc.sFullFunctionName.Remove(0, "functag".Length) & Environment.NewLine &
                                                                   "{" & Environment.NewLine &
                                                                   vbTab
                                Dim sNewInputLast As String = Environment.NewLine &
                                                                   "}"

                                TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iLineOffsetNum, sNewInputFirst & sNewInputLast)

                                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Line = iLineNum + 2
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = 1
                            Else
                                TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName)

                                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Length
                            End If

                        Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM,
                                     (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET,
                                     (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF
                            If (bIsEmpty) Then
                                Dim iLineOffsetNum As Integer = TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Offset
                                Dim iLineLenNum As Integer = TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegment(iLineNum).Length
                                TextEditorControl_Source.ActiveTextAreaControl.Document.Remove(iLineOffsetNum, iLineLenNum)

                                Dim sNewInputFirst As String = struc.sFunctionName & Environment.NewLine &
                                                                   "{" & Environment.NewLine &
                                                                   vbTab
                                Dim sNewInputLast As String = Environment.NewLine &
                                                                   "}"

                                TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iLineOffsetNum, sNewInputFirst & sNewInputLast)

                                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Line = iLineNum + 2
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = 1
                            Else
                                TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName)

                                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Length
                            End If


                        Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE,
                                   (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR
                            TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName)

                            iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                            TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Length

                        Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM
                            If (ClassSettings.g_iSettingsFullEnumAutocomplete OrElse struc.sFunctionName.IndexOf("."c) < 0) Then
                                TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName.Replace("."c, ":"c))

                                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Length
                            Else
                                TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName.Remove(0, struc.sFunctionName.IndexOf("."c) + 1))

                                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Remove(0, struc.sFunctionName.IndexOf("."c) + 1).Length
                            End If

                        Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP,
                                   (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                            If (struc.sFunctionName.IndexOf("."c) > -1 AndAlso sFunctionName.IndexOf("."c) > -1 AndAlso Not sFunctionName.StartsWith(struc.sFunctionName)) Then
                                Dim sNewInput As String = String.Format("{0}.{1}",
                                                                        sFunctionName.Remove(sFunctionName.LastIndexOf("."c), sFunctionName.Length - sFunctionName.LastIndexOf("."c)),
                                                                        struc.sFunctionName.Remove(0, struc.sFunctionName.IndexOf("."c) + 1))
                                TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, sNewInput)

                                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length
                            Else
                                TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, struc.sFunctionName.Remove(0, struc.sFunctionName.IndexOf("."c) + 1))

                                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = iPosition + struc.sFunctionName.Remove(0, struc.sFunctionName.IndexOf("."c) + 1).Length
                            End If

                        Case Else
                            If (ClassSettings.g_iSettingsFullMethodAutocomplete) Then
                                Dim sNewInput As String = struc.sFullFunctionName.Remove(0, Regex.Match(struc.sFullFunctionName, "^(?<Useless>.*?)\b[a-zA-Z0-9_]+\b\s*\(").Groups("Useless").Length)
                                TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, sNewInput)

                                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length
                            Else
                                Dim sNewInput As String = struc.sFunctionName.Remove(0, Regex.Match(struc.sFunctionName, "^(?<Useless>.*?)\b[a-zA-Z0-9_]+\b\s*\(").Groups("Useless").Length)
                                TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset - sFunctionName.Length, String.Format("{0}()", sNewInput))

                                iPosition = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
                                TextEditorControl_Source.ActiveTextAreaControl.Caret.Column = iPosition + sNewInput.Length + 1
                            End If

                    End Select
                End If

                TextEditorControl_Source.Document.UndoStack.EndUndoGroup()
                TextEditorControl_Source.Refresh()

            'Autocomplete up
            Case Keys.Control Or Keys.Up
                If (g_mUCAutocomplete.ListView_AutocompleteList.SelectedItems.Count < 1) Then
                    Return
                End If

                Dim iListViewCount As Integer = g_mUCAutocomplete.ListView_AutocompleteList.Items.Count

                Dim iNewIndex As Integer = g_mUCAutocomplete.ListView_AutocompleteList.SelectedItems(0).Index - 1

                If (iNewIndex > -1 AndAlso iNewIndex < iListViewCount) Then
                    g_mUCAutocomplete.ListView_AutocompleteList.Items(iNewIndex).Selected = True
                    g_mUCAutocomplete.ListView_AutocompleteList.Items(iNewIndex).EnsureVisible()
                End If

                bBlock = True

            'Autocomplete Down
            Case Keys.Control Or Keys.Down
                If (g_mUCAutocomplete.ListView_AutocompleteList.SelectedItems.Count < 1) Then
                    Return
                End If

                Dim iListViewCount As Integer = g_mUCAutocomplete.ListView_AutocompleteList.Items.Count

                Dim iNewIndex As Integer = g_mUCAutocomplete.ListView_AutocompleteList.SelectedItems(0).Index + 1

                If (iNewIndex > -1 AndAlso iNewIndex < iListViewCount) Then
                    g_mUCAutocomplete.ListView_AutocompleteList.Items(iNewIndex).Selected = True
                    g_mUCAutocomplete.ListView_AutocompleteList.Items(iNewIndex).EnsureVisible()
                End If

                bBlock = True

            'Update Autocomplete
            Case Keys.F5
                g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

                bBlock = True
        End Select
    End Sub

    Private Sub TextEditorControl_Source_UpdateInfo(sender As Object, e As EventArgs)
        ToolStripStatusLabel_EditorLine.Text = "L: " & TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Line + 1
        ToolStripStatusLabel_EditorCollum.Text = "C: " & TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Column
        ToolStripStatusLabel_EditorSelectedCount.Text = "S: " & TextEditorControl_Source.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText.Length
    End Sub

    Private Sub TextEditorControl_DetectLineLenghtChange(sender As Object, e As LineLengthChangeEventArgs)
        Dim iTotalLines As Integer = TextEditorControl_Source.Document.TotalNumberOfLines

        If (e.LineSegment.IsDeleted OrElse e.LineSegment.Length < 0) Then
            Return
        End If

        If (e.LineSegment.LineNumber > iTotalLines) Then
            Return
        End If

        g_ClassLineState.m_LineState(e.LineSegment.LineNumber) = ClassTextEditorTools.ClassLineState.LineStateBookmark.ENUM_BOOKMARK_TYPE.CHANGED
    End Sub

    Private Sub TextEditorControl_DetectLineCountChange(sender As Object, e As LineCountChangeEventArgs)
        Dim iTotalLines As Integer = TextEditorControl_Source.Document.TotalNumberOfLines

        If (e.LinesMoved > -1) Then
            For i = 0 To e.LinesMoved
                If (e.LineStart + i > iTotalLines) Then
                    Return
                End If

                g_ClassLineState.m_LineState(e.LineStart + i) = ClassTextEditorTools.ClassLineState.LineStateBookmark.ENUM_BOOKMARK_TYPE.CHANGED
            Next
        End If
    End Sub

    Private Sub TextEditorControl_Source_UpdateAutocomplete(sender As Object, e As Object)
        Static iOldCaretPos As Integer = 0

        Dim iOffset As Integer = TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Offset

        If (iOldCaretPos = iOffset) Then
            Return
        End If

        iOldCaretPos = iOffset

        Dim sFunctionName As String = g_ClassTextEditorTools.GetCaretWord(True)

        If (g_mUCAutocomplete.UpdateAutocomplete(sFunctionName) < 1) Then
            sFunctionName = g_ClassTextEditorTools.GetCaretWord(False)

            g_mUCAutocomplete.UpdateAutocomplete(sFunctionName)
        Else
            g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
        End If
    End Sub

    Private Sub TextEditorControl_Source_TextChanged(sender As Object, e As EventArgs)
        g_bCodeChanged = True
        ChangeTitle()
    End Sub

    Private Function ParseMethodAutocomplete(Optional bForceUpdate As Boolean = False) As Boolean
        If (bForceUpdate) Then
            Dim sTextContent As String = CStr(Me.Invoke(Function() TextEditorControl_Source.Document.TextContent))
            g_mSourceSyntaxSourceAnalysis = New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sTextContent)
        End If

        If (g_mSourceSyntaxSourceAnalysis IsNot Nothing) Then
            Dim iCaretOffset As Integer = CInt(Me.Invoke(Function() TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Offset))
            If (iCaretOffset > 1 AndAlso g_mSourceSyntaxSourceAnalysis.GetMaxLenght() > iCaretOffset AndAlso Not g_mSourceSyntaxSourceAnalysis.InMultiComment(iCaretOffset) AndAlso Not g_mSourceSyntaxSourceAnalysis.InSingleComment(iCaretOffset)) Then
                Dim iValidOffset As Integer = -1
                Dim iCaretBrace As Integer = g_mSourceSyntaxSourceAnalysis.GetParenthesisLevel(iCaretOffset - 1)
                For i = iCaretOffset - 1 To 0 Step -1
                    If (g_mSourceSyntaxSourceAnalysis.GetParenthesisLevel(i) < iCaretBrace) Then
                        iValidOffset = i
                        Exit For
                    End If
                Next

                If (iValidOffset > -1) Then
                    Dim SB As New StringBuilder

                    Dim i As Integer
                    For i = iValidOffset To iValidOffset - 64 Step -1
                        If (i < 0) Then
                            Exit For
                        End If

                        SB.Append(Me.Invoke(Function() TextEditorControl_Source.ActiveTextAreaControl.Document.GetCharAt(i)))
                    Next

                    Dim sFuncStart As String = StrReverse(SB.ToString)
                    sFuncStart = Regex.Match(sFuncStart, "(\.){0,1}(\b[a-zA-Z0-9_]+\b)$").Value

                    Me.BeginInvoke(Sub()
                                       g_mUCAutocomplete.g_ClassToolTip.CurrentMethod = sFuncStart
                                       g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
                                   End Sub)
                    Return True
                End If
            End If
        End If

        Return False
    End Function

    Private Sub TextEditorControl_Source_DoubleClickMarkWord(sender As Object, e As MouseEventArgs)
        If (Not ClassSettings.g_iSettingsDoubleClickMark) Then
            Return
        End If

        g_ClassTextEditorTools.MarkSelectedWord()
    End Sub

#End Region

#Region "Open/Save/Dialog"

    Private Sub TextEditorControl_Source_KeyDown(sender As Object, e As KeyEventArgs)
        If (e.KeyCode = Keys.S AndAlso e.Modifiers = Keys.Control) Then
            e.Handled = True
            g_ClassTextEditorTools.SaveFile()
        End If
        If (e.KeyCode = Keys.F AndAlso e.Modifiers = Keys.Control) Then
            e.Handled = True


            If (TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                g_ClassTextEditorTools.ShowSearchAndReplace(TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectedText)
            Else
                g_ClassTextEditorTools.ShowSearchAndReplace("")
            End If
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If (g_ClassTextEditorTools.PromptSave()) Then
            e.Cancel = True
        End If
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip_RightClick.Opening
        g_mUCAutocomplete.UpdateAutocomplete("")
        g_mUCAutocomplete.g_ClassToolTip.CurrentMethod = ""
        g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
    End Sub
#End Region

#Region "ContextMenuStrip"
    Private Sub ToolStripMenuItem_Mark_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Mark.Click
        g_ClassTextEditorTools.MarkSelectedWord()
    End Sub

    Private Sub ToolStripMenuItem_Cut_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Cut.Click
        TextEditorControl_Source.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_Copy_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Copy.Click
        TextEditorControl_Source.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_Paste_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Paste.Click
        TextEditorControl_Source.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e)
    End Sub
#End Region

#Region "MenuStrip"

#Region "MenuStrip_File"
    Private Sub ToolStripMenuItem_FileNew_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileNew.Click
        If (g_ClassTextEditorTools.PromptSave()) Then
            Return
        End If

        ClassSettings.g_sConfigOpenSourceFile = ""
        TextEditorControl_Source.Document.TextContent = ""
        TextEditorControl_Source.Refresh()

        g_bCodeChanged = False
        ChangeTitle()
        g_ClassLineState.ClearStates()

        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

        PrintInformation("[INFO]", "User created a new source file")
    End Sub

    Private Sub ToolStripMenuItem_FileOpen_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileOpen.Click
        Using i As New OpenFileDialog
            i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|Pawn (Not fully supported)|*.pwn;*.p|AMX Mod X|*.sma|All files|*.*"
            i.FileName = ClassSettings.g_sConfigOpenSourceFile
            If (i.ShowDialog = DialogResult.OK) Then
                g_ClassTextEditorTools.OpenFile(i.FileName)
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_FileSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSave.Click
        g_ClassTextEditorTools.PromptSave(False, True)
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAs.Click
        Using i As New SaveFileDialog
            i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|Pawn (Not fully supported)|*.pwn;*.p|AMX Mod X|*.sma|All files|*.*"
            i.FileName = ClassSettings.g_sConfigOpenSourceFile

            If (i.ShowDialog = DialogResult.OK) Then
                ClassSettings.g_sConfigOpenSourceFile = i.FileName

                g_bCodeChanged = False
                ChangeTitle()
                g_ClassLineState.SaveStates()

                PrintInformation("[INFO]", "User saved file to: " & ClassSettings.g_sConfigOpenSourceFile)
                IO.File.WriteAllText(ClassSettings.g_sConfigOpenSourceFile, TextEditorControl_Source.Document.TextContent)
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAsTemp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAsTemp.Click
        Dim sTempFile As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))

        ClassSettings.g_sConfigOpenSourceFile = sTempFile

        g_bCodeChanged = False
        ChangeTitle()
        g_ClassLineState.SaveStates()

        PrintInformation("[INFO]", "User saved file to: " & ClassSettings.g_sConfigOpenSourceFile)
        IO.File.WriteAllText(ClassSettings.g_sConfigOpenSourceFile, TextEditorControl_Source.Document.TextContent)
    End Sub


    Private Sub ToolStripMenuItem_FileExit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileExit.Click
        Me.Close()
    End Sub
#End Region

#Region "MenuStrip_Tools"
    Private Sub ToolStripMenuItem_ToolsSettingsAndConfigs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsSettingsAndConfigs.Click
        Using i As New FormSettings
            i.g_fFormMain = Me
            If (i.ShowDialog() = DialogResult.OK) Then
                ChangeTitle()

                g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

                TextEditorControl_Source.ActiveTextAreaControl.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
                TextEditorControl_Source.Refresh()

                g_ClassSyntaxTools.UpdateFormColors()
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_ToolsFormatCode_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsFormatCode.Click
        Try
            Dim sSource As String = TextEditorControl_Source.Document.TextContent

            sSource = g_ClassSyntaxTools.FormatCode(sSource)

            TextEditorControl_Source.Document.UndoStack.StartUndoGroup()
            TextEditorControl_Source.Document.Remove(0, TextEditorControl_Source.Document.TextLength)
            TextEditorControl_Source.Document.Insert(0, sSource)
            TextEditorControl_Source.Document.UndoStack.EndUndoGroup()
            TextEditorControl_Source.Refresh()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ToolsSearchReplace_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsSearchReplace.Click
        g_ClassTextEditorTools.ShowSearchAndReplace("")
    End Sub

    Private Sub ToolStripMenuItem_ToolsShowInformation_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsShowInformation.Click
        SplitContainer1.Panel2Collapsed = False
        SplitContainer1.SplitterDistance = SplitContainer1.Height - 200
        TabControl_Details.SelectTab(1)
    End Sub

    Private Sub ToolStripMenuItem_ToolsClearInformationLog_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsClearInformationLog.Click
        PrintInformation("[INFO]", "Information log cleaned!", True)
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocompleteUpdate_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocompleteUpdate.Click
        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)
    End Sub

    Private Sub ToolStripComboBox_ToolsAutocompleteSyntax_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndexChanged
        Select Case (ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndex)
            Case 0
                ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX
            Case 1
                ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6
            Case 2
                ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7
        End Select

        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocompleteShowAutocomplete_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocompleteShowAutocomplete.Click
        SplitContainer1.Panel2Collapsed = False
        SplitContainer1.SplitterDistance = SplitContainer1.Height - 200
        TabControl_Details.SelectTab(0)
    End Sub

    Private Sub ToolStripMenuItem_ListReferences_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ListReferences.Click
        g_ClassTextEditorTools.ListReferences()
    End Sub
#End Region

#Region "MenuStrip_Build"
    Private Sub ToolStripMenuItem_Build_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Build.Click
        With New ClassDebuggerParser(Me)
            If (.HasDebugPlaceholder(TextEditorControl_Source.Document.TextContent)) Then
                Select Case (MessageBox.Show("All BasicPawn Debugger placeholders need to be removed before compiling the source. Remove all placeholder?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
                    Case DialogResult.OK
                        .CleanupDebugPlaceholder(Me)
                    Case Else
                        Return
                End Select
            End If
        End With

        g_ClassTextEditorTools.CompileSource(False)
    End Sub
#End Region

#Region "MenuStrip_Test"
    Private Sub ToolStripMenuItem_Test_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Test.Click
        With New ClassDebuggerParser(Me)
            If (.HasDebugPlaceholder(TextEditorControl_Source.Document.TextContent)) Then
                Select Case (MessageBox.Show("All BasicPawn Debugger placeholders need to be removed before compiling the source. Remove all placeholder?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
                    Case DialogResult.OK
                        .CleanupDebugPlaceholder(Me)
                    Case Else
                        Return
                End Select
            End If
        End With

        g_ClassTextEditorTools.CompileSource(True)
    End Sub
#End Region

#Region "MenuStrip_Debug"
    Private Sub ToolStripMenuItem_Debug_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Debug.Click
        If (g_mFormDebugger Is Nothing OrElse g_mFormDebugger.IsDisposed) Then
            g_mFormDebugger = New FormDebugger(Me)
            g_mFormDebugger.Show()
        End If
    End Sub
#End Region

#Region "MenuStrip_Shell"
    Private Sub ToolStripMenuItem_Shell_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Shell.Click
        Try
            Dim sShell As String = ClassSettings.g_sConfigExecuteShell

            For Each shellModule In ClassSettings.GetShellArguments
                sShell = sShell.Replace(shellModule.g_sMarker, shellModule.g_sArgument)
            Next

            Try
                If (String.IsNullOrEmpty(sShell)) Then
                    Throw New ArgumentException("Shell is empty")
                End If

                Shell(sShell, AppWinStyle.NormalFocus)
            Catch ex As Exception
                MessageBox.Show(ex.Message & Environment.NewLine & Environment.NewLine & sShell, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
#End Region

#Region "MenuStrip_Help"
    Private Sub ToolStripMenuItem_HelpAbout_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_HelpAbout.Click
        Dim SB As New StringBuilder
        SB.AppendLine(String.Format("{0} v.{1}", Application.ProductName, Application.ProductVersion))
        SB.AppendLine("Created by Externet (aka Timocop)")
        SB.AppendLine()
        SB.AppendLine("Source and Releases")
        SB.AppendLine("     https://github.com/Timocop/BasicPawn")
        SB.AppendLine()
        SB.AppendLine("Third-Party tools:")
        SB.AppendLine("     SharpDevelop - TextEditor (LGPL-2.1)")
        SB.AppendLine()
        SB.AppendLine("     Authors:")
        SB.AppendLine("     Daniel Grunwald and SharpDevelop Community")
        SB.AppendLine("     https://github.com/icsharpcode/SharpDevelop")
        MessageBox.Show(SB.ToString, "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

#Region "MenuStrip_Undo"
    Private Sub ToolStripMenuItem_Undo_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Undo.Click
        TextEditorControl_Source.Undo()
    End Sub
#End Region

#Region "MenuStrip_Redo"
    Private Sub ToolStripMenuItem_Redo_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Redo.Click
        TextEditorControl_Source.Redo()
    End Sub
#End Region


    Private Sub ToolStripStatusLabel_CurrentConfig_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel_CurrentConfig.Click
        Using i As New FormSettings
            i.g_fFormMain = Me
            i.TabControl1.SelectTab(1)
            If (i.ShowDialog() = DialogResult.OK) Then
                ChangeTitle()

                g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

                TextEditorControl_Source.ActiveTextAreaControl.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
                TextEditorControl_Source.Refresh()

                g_ClassSyntaxTools.UpdateFormColors()
            End If
        End Using
    End Sub

#End Region

    ''' <summary>
    ''' Usefull tools for the TextEditor
    ''' </summary>
    Public Class ClassTextEditorTools
        Private g_mFormMain As FormMain

        Public Sub New(f As FormMain)
            g_mFormMain = f
        End Sub

        ''' <summary>
        ''' Marks a selected word in the text editor
        ''' </summary>
        Public Sub MarkSelectedWord()
            Dim sLastWord As String = g_mFormMain.g_ClassSyntaxTools.g_sHighlightWord
            g_mFormMain.g_ClassSyntaxTools.g_sHighlightWord = ""

            If (g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                Dim m_CurrentSelection As ISelection = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection(0)

                If (Regex.IsMatch(m_CurrentSelection.SelectedText, "^[a-zA-Z0-9_]+$")) Then
                    g_mFormMain.g_ClassSyntaxTools.g_sHighlightWord = m_CurrentSelection.SelectedText
                End If
            End If

            If (g_mFormMain.g_ClassSyntaxTools.g_sHighlightWord = sLastWord) Then
                Return
            End If

            g_mFormMain.g_ClassSyntaxTools.UpdateSyntaxFile(ClassSyntaxTools.ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD)
            g_mFormMain.g_ClassSyntaxTools.UpdateTextEditorSyntax()
        End Sub

        ''' <summary>
        ''' Marks a selected word in the text editor
        ''' </summary>
        Public Sub MarkCaretWord()
            Dim sLastWord As String = g_mFormMain.g_ClassSyntaxTools.g_sCaretWord
            g_mFormMain.g_ClassSyntaxTools.g_sCaretWord = ""

            If (Not g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                Dim sWord As String = GetCaretWord(False)

                If (Not String.IsNullOrEmpty(sWord)) Then
                    g_mFormMain.g_ClassSyntaxTools.g_sCaretWord = sWord
                End If
            End If

            If (g_mFormMain.g_ClassSyntaxTools.g_sCaretWord = sLastWord) Then
                Return
            End If

            If (Not String.IsNullOrEmpty(g_mFormMain.g_ClassSyntaxTools.g_sCaretWord) AndAlso
                        Regex.Matches(g_mFormMain.TextEditorControl_Source.Document.TextContent, String.Format("\b{0}\b", Regex.Escape(g_mFormMain.g_ClassSyntaxTools.g_sCaretWord)), RegexOptions.Multiline).Count < 2) Then
                Return
            End If

            g_mFormMain.g_ClassSyntaxTools.UpdateSyntaxFile(ClassSyntaxTools.ENUM_SYNTAX_UPDATE_TYPE.CARET_WORD)
            g_mFormMain.g_ClassSyntaxTools.UpdateTextEditorSyntax()
        End Sub

        ''' <summary>
        ''' Lists all references of a word from current opeened source and all include files.
        ''' </summary>
        ''' <param name="sText">Word to search, otherwise it will get the word under the caret</param>
        Public Sub ListReferences(Optional sText As String = Nothing)
            Dim sWord As String

            If (Not String.IsNullOrEmpty(sText)) Then
                sWord = sText
            Else
                sWord = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(False)
            End If

            If (String.IsNullOrEmpty(sWord)) Then
                g_mFormMain.PrintInformation("[ERRO]", "Can't check references! Nothing valid selected!", False, True)
                Return
            End If

            If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                g_mFormMain.PrintInformation("[ERRO]", "Can't check references! Could not get current source file!", False, True)
                Return
            End If

            g_mFormMain.PrintInformation("[INFO]", String.Format("Listing references of: {0}", sWord), False, True)

            Dim sIncludeFiles As String() = g_mFormMain.g_ClassAutocompleteUpdater.GetIncludeFiles(ClassSettings.g_sConfigOpenSourceFile)

            Dim lRefList As New List(Of String)

            For Each sFile As String In sIncludeFiles
                If (Not IO.File.Exists(sFile)) Then
                    Continue For
                End If

                If (sFile.ToLower = ClassSettings.g_sConfigOpenSourceFile.ToLower) Then
                    Dim iLineCount As Integer = 0
                    Using SR As New IO.StringReader(g_mFormMain.TextEditorControl_Source.Document.TextContent)
                        While True
                            Dim sLine As String = SR.ReadLine
                            If (sLine Is Nothing) Then
                                Exit While
                            End If

                            iLineCount += 1

                            If (sLine.Contains(sWord) AndAlso Regex.IsMatch(sLine, String.Format("\b{0}\b", Regex.Escape(sWord)))) Then
                                lRefList.Add(vbTab & String.Format("Reference found: {0}({1}) : {2}", sFile, iLineCount, sLine.Trim))
                            End If
                        End While
                    End Using
                Else
                    Dim iLineCount As Integer = 0
                    Using SR As New IO.StreamReader(sFile)
                        While True
                            Dim sLine As String = SR.ReadLine
                            If (sLine Is Nothing) Then
                                Exit While
                            End If

                            iLineCount += 1

                            If (sLine.Contains(sWord) AndAlso Regex.IsMatch(sLine, String.Format("\b{0}\b", Regex.Escape(sWord)))) Then
                                lRefList.Add(vbTab & String.Format("Reference found: {0}({1}) : {2}", sFile, iLineCount, sLine.Trim))
                            End If
                        End While
                    End Using
                End If
            Next

            lRefList.Reverse()

            For Each sRef As String In lRefList
                g_mFormMain.PrintInformation("[INFO]", sRef, False, True)
            Next


            g_mFormMain.PrintInformation("[INFO]", "All references listed!", False, True)
        End Sub

        ''' <summary>
        ''' Opens a 'Search and Replace' form
        ''' </summary>
        ''' <param name="sSearchText"></param>
        Public Sub ShowSearchAndReplace(sSearchText As String)
            Dim iFormCount As Integer = 0
            For Each c As Form In Application.OpenForms
                If (TryCast(c, FormSearch) IsNot Nothing) Then
                    iFormCount += 1
                End If
            Next

            If (iFormCount > 100) Then
                MessageBox.Show("Too many 'Search & Replace' windows open!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                Dim i As New FormSearch(g_mFormMain, sSearchText)
                i.Show()
            End If
        End Sub

        ''' <summary>
        ''' Gets the caret word in the text editor
        ''' </summary>
        ''' <param name="bIncludeDot">If true, includes dots (e.g ThisWord.ThatWord)</param>
        ''' <returns></returns>
        Public Function GetCaretWord(bIncludeDot As Boolean) As String
            Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Offset
            Dim iPosition As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column
            Dim iLineOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iOffset).Offset
            Dim iLineLen As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iOffset).Length

            Dim sFuncStart As String
            Dim sFuncEnd As String
            Dim sFunctionName As String

            If (bIncludeDot) Then
                sFuncStart = Regex.Match(g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.GetText(iLineOffset, iPosition), "((\b[a-zA-Z0-9_]+\b)(\.){0,1}(\b[a-zA-Z0-9_]+\b){0,1})$").Value
                sFuncEnd = Regex.Match(g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.GetText(iLineOffset + iPosition, iLineLen - iPosition), "^((\b[a-zA-Z0-9_]+\b){0,1}(\.){0,1}(\b[a-zA-Z0-9_]+\b))").Value
            Else
                sFuncStart = Regex.Match(g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.GetText(iLineOffset, iPosition), "(\b[a-zA-Z0-9_]+\b)$").Value
                sFuncEnd = Regex.Match(g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.GetText(iLineOffset + iPosition, iLineLen - iPosition), "^(\b[a-zA-Z0-9_]+\b)").Value
            End If

            sFunctionName = (sFuncStart & sFuncEnd)

            Return sFunctionName
        End Function

        ''' <summary>
        ''' Opens a new source file
        ''' </summary>
        ''' <param name="sPath"></param>
        ''' <param name="bIgnoreSavePrompt">If true, the new file will be opened without prompting to save the changed source</param>
        ''' <returns></returns>
        Public Function OpenFile(sPath As String, Optional bIgnoreSavePrompt As Boolean = False) As Boolean
            If (Not bIgnoreSavePrompt AndAlso PromptSave()) Then
                Return False
            End If

            If (String.IsNullOrEmpty(sPath) OrElse Not IO.File.Exists(sPath)) Then
                ClassSettings.g_sConfigOpenSourceFile = ""
                g_mFormMain.TextEditorControl_Source.Document.TextContent = ""
                g_mFormMain.TextEditorControl_Source.Refresh()

                g_mFormMain.g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

                g_mFormMain.PrintInformation("[INFO]", "User created a new source file")
                Return False
            End If

            g_mFormMain.g_ClassLineState.m_IgnoreUpdates = True

            ClassSettings.g_sConfigOpenSourceFile = sPath
            g_mFormMain.TextEditorControl_Source.Document.TextContent = IO.File.ReadAllText(sPath)
            g_mFormMain.TextEditorControl_Source.Refresh()

            g_mFormMain.g_bCodeChanged = False
            g_mFormMain.ChangeTitle()
            g_mFormMain.g_ClassLineState.ClearStates()
            g_mFormMain.g_ClassLineState.m_IgnoreUpdates = False

            g_mFormMain.g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

            g_mFormMain.PrintInformation("[INFO]", "User opened a new file: " & sPath)
            Return True
        End Function

        ''' <summary>
        ''' Saves a source file
        ''' </summary>
        ''' <param name="bSaveAs">Force to use a new file using SaveFileDialog</param>
        Public Sub SaveFile(Optional bSaveAs As Boolean = False)
            If (bSaveAs OrElse String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                Using i As New SaveFileDialog
                    i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|Pawn (Not fully supported)|*.pwn;*.p|AMX Mod X|*.sma|All files|*.*"
                    i.FileName = ClassSettings.g_sConfigOpenSourceFile
                    If (i.ShowDialog = DialogResult.OK) Then
                        ClassSettings.g_sConfigOpenSourceFile = i.FileName

                        g_mFormMain.g_bCodeChanged = False
                        g_mFormMain.ChangeTitle()
                        g_mFormMain.g_ClassLineState.SaveStates()

                        g_mFormMain.PrintInformation("[INFO]", "User saved file to: " & ClassSettings.g_sConfigOpenSourceFile)
                        IO.File.WriteAllText(ClassSettings.g_sConfigOpenSourceFile, g_mFormMain.TextEditorControl_Source.Document.TextContent)
                    End If
                End Using
            Else
                g_mFormMain.g_bCodeChanged = False
                g_mFormMain.ChangeTitle()
                g_mFormMain.g_ClassLineState.SaveStates()

                g_mFormMain.PrintInformation("[INFO]", "User saved file to: " & ClassSettings.g_sConfigOpenSourceFile)
                IO.File.WriteAllText(ClassSettings.g_sConfigOpenSourceFile, g_mFormMain.TextEditorControl_Source.Document.TextContent)
            End If
        End Sub

        ''' <summary>
        ''' If the code has been changed it will prompt the user and saves the source. The user can abort the saving.
        ''' </summary>
        ''' <param name="bAlwaysPrompt">If true, always show MessageBox even if the code didnt change</param>
        ''' <param name="bAlwaysYes">If true, ignores MessageBox prompt</param>
        ''' <returns></returns>
        Public Function PromptSave(Optional bAlwaysPrompt As Boolean = False, Optional bAlwaysYes As Boolean = False) As Boolean
            If (Not bAlwaysPrompt AndAlso Not g_mFormMain.g_bCodeChanged) Then
                Return False
            End If

            Select Case (If(bAlwaysYes, DialogResult.Yes, MessageBox.Show("Do you want to save your work?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)))
                Case DialogResult.Yes
                    If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                        Using i As New SaveFileDialog
                            i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|Pawn (Not fully supported)|*.pwn;*.p|AMX Mod X|*.sma|All files|*.*"
                            i.FileName = ClassSettings.g_sConfigOpenSourceFile
                            If (i.ShowDialog = DialogResult.OK) Then
                                ClassSettings.g_sConfigOpenSourceFile = i.FileName

                                g_mFormMain.g_bCodeChanged = False
                                g_mFormMain.ChangeTitle()
                                g_mFormMain.g_ClassLineState.SaveStates()

                                g_mFormMain.PrintInformation("[INFO]", "User saved file to: " & ClassSettings.g_sConfigOpenSourceFile)
                                IO.File.WriteAllText(ClassSettings.g_sConfigOpenSourceFile, g_mFormMain.TextEditorControl_Source.Document.TextContent)

                                Return False
                            Else
                                Return True
                            End If
                        End Using
                    Else
                        g_mFormMain.g_bCodeChanged = False
                        g_mFormMain.ChangeTitle()
                        g_mFormMain.g_ClassLineState.SaveStates()

                        g_mFormMain.PrintInformation("[INFO]", "User saved file to: " & ClassSettings.g_sConfigOpenSourceFile)
                        IO.File.WriteAllText(ClassSettings.g_sConfigOpenSourceFile, g_mFormMain.TextEditorControl_Source.Document.TextContent)

                        Return False
                    End If
                Case DialogResult.No
                    Return False
                Case Else
                    Return True
            End Select
        End Function

        Enum ENUM_COMPILER_TYPE
            UNKNOWN
            SOURCEPAWN
            AMXX
            AMX
        End Enum

        ''' <summary>
        ''' Compiles the source in the text editor
        ''' </summary>
        ''' <param name="bTesting">If true, compiles the source and removes the compiled binary file again</param>
        Public Sub CompileSource(bTesting As Boolean)
            Try
                If (PromptSave(False, True)) Then
                    Return
                End If

                If (g_mFormMain.SplitContainer1.Panel2Collapsed) Then
                    g_mFormMain.SplitContainer1.Panel2Collapsed = False
                    g_mFormMain.SplitContainer1.SplitterDistance = g_mFormMain.SplitContainer1.Height - 200
                End If
                g_mFormMain.TabControl_Details.SelectTab(1)

                g_mFormMain.PrintInformation("[INFO]", "Compiling source started!")

                Dim sCompilerPath As String = ""
                Dim sIncludePath As String = ""
                Dim sOutputFile As String = ""

                Dim iExitCode As Integer = 0
                Dim sOutput As String = ""

                If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                    g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Could not get current source file!")
                    Return
                End If

                'Check compiler
                If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    While True
                        'SourcePawn
                        sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "spcomp.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'AMX Mod X
                        sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "amxxpc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'Small
                        sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "sc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'Pawn
                        sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "pawncc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Compiler can not be found!")
                        Return
                    End While
                Else
                    sCompilerPath = ClassSettings.g_sConfigCompilerPath
                    If (Not IO.File.Exists(sCompilerPath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Compiler can not be found!")
                        Return
                    End If
                End If

                'Check include path
                If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    sIncludePath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "include")

                    If (Not IO.Directory.Exists(sIncludePath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Include path can not be found!")
                        Return
                    End If
                Else
                    sIncludePath = ClassSettings.g_sConfigOpenIncludeFolder
                    If (Not IO.Directory.Exists(sIncludePath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Include path can not be found!")
                        Return
                    End If
                End If

                'Set output path
                If (bTesting) Then
                    sOutputFile = String.Format("{0}.unk", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
                Else
                    If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                        sOutputFile = String.Format("{0}\compiled\{1}.unk", IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile).TrimEnd("\"c), IO.Path.GetFileNameWithoutExtension(ClassSettings.g_sConfigOpenSourceFile))
                    Else
                        If (Not IO.Directory.Exists(ClassSettings.g_sConfigPluginOutputFolder)) Then
                            g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Invalid output directory!")
                            Return
                        End If
                        sOutputFile = String.Format("{0}\{1}.unk", ClassSettings.g_sConfigPluginOutputFolder.TrimEnd("\"c), IO.Path.GetFileNameWithoutExtension(ClassSettings.g_sConfigOpenSourceFile))
                    End If
                End If

                IO.File.Delete(sOutputFile)

                Dim sArguments As String = String.Format("""{0}"" -i""{1}"" -o""{2}""", ClassSettings.g_sConfigOpenSourceFile, sIncludePath, sOutputFile)
                ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, sArguments, iExitCode, sOutput)

                Dim compilerType As ENUM_COMPILER_TYPE = ENUM_COMPILER_TYPE.UNKNOWN

                Dim sLines As String() = sOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
                For i = sLines.Length - 1 To 0 Step -1
                    If (i = 0) Then
                        Select Case (True)
                            Case Regex.IsMatch(sLines(i), "\b(SourcePawn)\b \b(Compiler)\b", RegexOptions.IgnoreCase)
                                compilerType = ENUM_COMPILER_TYPE.SOURCEPAWN

                            Case Regex.IsMatch(sLines(i), "\b(AMX)\b \b(Mod)\b \b(X)\b", RegexOptions.IgnoreCase)
                                compilerType = ENUM_COMPILER_TYPE.AMXX

                            'Old AMX Mod still uses Small compiler
                            Case Regex.IsMatch(sLines(i), "\b(Pawn)\b \b(compiler)\b", RegexOptions.IgnoreCase), Regex.IsMatch(sLines(i), "\b(Small)\b \b(compiler)\b", RegexOptions.IgnoreCase)
                                compilerType = ENUM_COMPILER_TYPE.AMX

                        End Select
                    End If

                    g_mFormMain.PrintInformation("[INFO]", vbTab & sLines(i))
                Next

                If (IO.File.Exists(sOutputFile)) Then
                    Select Case (compilerType)
                        Case ENUM_COMPILER_TYPE.SOURCEPAWN
                            IO.File.Move(sOutputFile, IO.Path.ChangeExtension(sOutputFile, ".smx"))
                            sOutputFile = IO.Path.ChangeExtension(sOutputFile, ".smx")

                        Case ENUM_COMPILER_TYPE.AMXX
                            IO.File.Move(sOutputFile, IO.Path.ChangeExtension(sOutputFile, ".amxx"))
                            sOutputFile = IO.Path.ChangeExtension(sOutputFile, ".amxx")

                        Case ENUM_COMPILER_TYPE.AMX
                            IO.File.Move(sOutputFile, IO.Path.ChangeExtension(sOutputFile, ".amx"))
                            sOutputFile = IO.Path.ChangeExtension(sOutputFile, ".amx")

                    End Select
                End If

                If (bTesting) Then
                    IO.File.Delete(sOutputFile)
                ElseIf (Not IO.File.Exists(sOutputFile)) Then
                    g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Source output can not be found!")
                    Return
                End If

                If (Not bTesting) Then
                    g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Saved compiled source: {0}", sOutputFile))
                End If
                g_mFormMain.PrintInformation("[INFO]", "Compiling source finished!")
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Compiles the source in the text editor
        ''' </summary>
        ''' <param name="bTesting">If true, compiles the source and removes the compiled binary file again</param>
        Public Function CompileSource(bTesting As Boolean, sSource As String, sOutputFile As String, Optional sCompilerPath As String = Nothing, Optional sIncludePath As String = Nothing) As Boolean
            Try
                If (PromptSave(False, True)) Then
                    Return False
                End If

                If (g_mFormMain.SplitContainer1.Panel2Collapsed) Then
                    g_mFormMain.SplitContainer1.Panel2Collapsed = False
                    g_mFormMain.SplitContainer1.SplitterDistance = g_mFormMain.SplitContainer1.Height - 200
                End If
                g_mFormMain.TabControl_Details.SelectTab(1)

                g_mFormMain.PrintInformation("[INFO]", "Compiling source started!")

                Dim iExitCode As Integer = 0
                Dim sOutput As String = ""

                'Check compiler
                If (sCompilerPath Is Nothing) Then
                    If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                        If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                            g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Could not get current source file!")
                            Return False
                        End If

                        While True
                            'SourcePawn
                            sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "spcomp.exe")
                            If (IO.File.Exists(sCompilerPath)) Then
                                Exit While
                            End If

                            'AMX Mod X
                            sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "amxxpc.exe")
                            If (IO.File.Exists(sCompilerPath)) Then
                                Exit While
                            End If

                            'Small
                            sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "sc.exe")
                            If (IO.File.Exists(sCompilerPath)) Then
                                Exit While
                            End If

                            'Pawn
                            sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "pawncc.exe")
                            If (IO.File.Exists(sCompilerPath)) Then
                                Exit While
                            End If

                            g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Compiler can not be found!")
                            Return False
                        End While
                    Else
                        sCompilerPath = ClassSettings.g_sConfigCompilerPath
                        If (Not IO.File.Exists(sCompilerPath)) Then
                            g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Compiler can not be found!")
                            Return False
                        End If
                    End If
                Else
                    If (Not IO.File.Exists(sCompilerPath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Compiler can not be found!")
                        Return False
                    End If
                End If

                'Check include path
                If (sIncludePath Is Nothing) Then
                    If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                        If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                            g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Could not get current source file!")
                            Return False
                        End If

                        sIncludePath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "include")
                        If (Not IO.Directory.Exists(sIncludePath)) Then
                            g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Include path can not be found!")
                            Return False
                        End If
                    Else
                        sIncludePath = ClassSettings.g_sConfigOpenIncludeFolder
                        If (Not IO.Directory.Exists(sIncludePath)) Then
                            g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Include path can not be found!")
                            Return False
                        End If
                    End If
                Else
                    If (Not IO.Directory.Exists(sIncludePath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Include path can not be found!")
                        Return False
                    End If
                End If

                'Set output path
                If (bTesting) Then
                    sOutputFile = String.Format("{0}.unk", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
                End If

                IO.File.Delete(sOutputFile)

                Dim TmpSourceFile As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
                IO.File.WriteAllText(TmpSourceFile, sSource)

                Dim sArguments As String = String.Format("""{0}"" -i""{1}"" -o""{2}""", TmpSourceFile, sIncludePath, sOutputFile)
                ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, sArguments, iExitCode, sOutput)

                IO.File.Delete(TmpSourceFile)

                Dim compilerType As ENUM_COMPILER_TYPE = ENUM_COMPILER_TYPE.UNKNOWN

                Dim sLines As String() = sOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
                For i = sLines.Length - 1 To 0 Step -1
                    If (i = 0) Then
                        Select Case (True)
                            Case Regex.IsMatch(sLines(i), "\b(SourcePawn)\b \b(Compiler)\b", RegexOptions.IgnoreCase)
                                compilerType = ENUM_COMPILER_TYPE.SOURCEPAWN

                            Case Regex.IsMatch(sLines(i), "\b(AMX)\b \b(Mod)\b \b(X)\b", RegexOptions.IgnoreCase)
                                compilerType = ENUM_COMPILER_TYPE.AMXX

                            'Old AMX Mod still uses Small compiler
                            Case Regex.IsMatch(sLines(i), "\b(Pawn)\b \b(compiler)\b", RegexOptions.IgnoreCase), Regex.IsMatch(sLines(i), "\b(Small)\b \b(compiler)\b", RegexOptions.IgnoreCase)
                                compilerType = ENUM_COMPILER_TYPE.AMX

                        End Select
                    End If

                    g_mFormMain.PrintInformation("[INFO]", vbTab & sLines(i))
                Next

                If (IO.File.Exists(sOutputFile)) Then
                    Select Case (compilerType)
                        Case ENUM_COMPILER_TYPE.SOURCEPAWN
                            IO.File.Move(sOutputFile, IO.Path.ChangeExtension(sOutputFile, ".smx"))
                            sOutputFile = IO.Path.ChangeExtension(sOutputFile, ".smx")

                        Case ENUM_COMPILER_TYPE.AMXX
                            IO.File.Move(sOutputFile, IO.Path.ChangeExtension(sOutputFile, ".amxx"))
                            sOutputFile = IO.Path.ChangeExtension(sOutputFile, ".amxx")

                        Case ENUM_COMPILER_TYPE.AMX
                            IO.File.Move(sOutputFile, IO.Path.ChangeExtension(sOutputFile, ".amx"))
                            sOutputFile = IO.Path.ChangeExtension(sOutputFile, ".amx")

                    End Select
                End If

                If (bTesting) Then
                    IO.File.Delete(sOutputFile)
                ElseIf (Not IO.File.Exists(sOutputFile)) Then
                    g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Source output can not be found!")
                    Return False
                End If

                If (Not bTesting) Then
                    g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Saved compiled source: {0}", sOutputFile))
                End If
                g_mFormMain.PrintInformation("[INFO]", "Compiling source finished!")

                Return True
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try

            Return False
        End Function

        ''' <summary>
        ''' Gets the pre-process source from the compiler. Its cleaned up, defines resolved etc.
        ''' </summary>
        ''' <param name="bCleanUpSourcemodDuplicate">Removed duplicated sourcemod includes, includes from the compiler</param>
        ''' <param name="bCleanupForCompile"></param>
        ''' <returns></returns>
        Public Function GetCompilerPreProcessCode(bCleanUpSourcemodDuplicate As Boolean, bCleanupForCompile As Boolean, ByRef sTempOutputFile As String) As String
            Try
                If (PromptSave(False, True)) Then
                    Return Nothing
                End If

                If (g_mFormMain.SplitContainer1.Panel2Collapsed) Then
                    g_mFormMain.SplitContainer1.Panel2Collapsed = False
                    g_mFormMain.SplitContainer1.SplitterDistance = g_mFormMain.SplitContainer1.Height - 200
                End If
                g_mFormMain.TabControl_Details.SelectTab(1)

                g_mFormMain.PrintInformation("[INFO]", "Pre-Processing source started!")

                Dim sMarkStart As String = Guid.NewGuid.ToString
                Dim sMarkEnd As String = Guid.NewGuid.ToString

                Dim sCompilerPath As String = ""
                Dim sIncludePath As String = ""
                Dim sOutputFile As String = ""

                Dim iExitCode As Integer = 0
                Dim sOutput As String = ""

                If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                    g_mFormMain.PrintInformation("[ERRO]", "Pre-Processing failed! Could not get current source file!")
                    Return Nothing
                End If

                'Check compiler
                If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    While True
                        'SourcePawn
                        sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "spcomp.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'AMX Mod X
                        sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "amxxpc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'Small
                        sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "sc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'Pawn
                        sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "pawncc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        g_mFormMain.PrintInformation("[ERRO]", "Pre-Processing failed! Compiler can not be found!")
                        Return Nothing
                    End While
                Else
                    sCompilerPath = ClassSettings.g_sConfigCompilerPath
                    If (Not IO.File.Exists(sCompilerPath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "Pre-Processing failed! Compiler can not be found!")
                        Return Nothing
                    End If
                End If

                'Check include path
                If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    sIncludePath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "include")
                    If (Not IO.Directory.Exists(sIncludePath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Include path can not be found!")
                        Return Nothing
                    End If
                Else
                    sIncludePath = ClassSettings.g_sConfigOpenIncludeFolder
                    If (Not IO.Directory.Exists(sIncludePath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "Compiling failed! Include path can not be found!")
                        Return Nothing
                    End If
                End If


                Dim sTmpSource As String = IO.File.ReadAllText(ClassSettings.g_sConfigOpenSourceFile)
                Dim sTmpSourcePath As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))

                sTempOutputFile = sTmpSourcePath

                If (bCleanUpSourcemodDuplicate) Then
                    '#file pushes the lines +1 in the main source, add #line 0 to make them even again
                    Dim SB As New StringBuilder
                    SB.AppendLine("#file " & sMarkStart)
                    SB.AppendLine("#line 0")
                    SB.AppendLine(sTmpSource)
                    SB.AppendLine("#file " & sMarkEnd)
                    sTmpSource = SB.ToString
                End If

                IO.File.WriteAllText(sTmpSourcePath, sTmpSource)

                sOutputFile = String.Format("{0}.lst", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))

                Dim sArguments As String = String.Format("""{0}"" -l -i""{1}"" -o""{2}""", sTmpSourcePath, sIncludePath, sOutputFile)
                ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, sArguments, iExitCode, sOutput)

                IO.File.Delete(sTmpSourcePath)

                Dim sLines As String() = sOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
                For i = sLines.Length - 1 To 0 Step -1
                    g_mFormMain.PrintInformation("[INFO]", vbTab & sLines(i))
                Next

                If (String.IsNullOrEmpty(sOutputFile) OrElse Not IO.File.Exists(sOutputFile)) Then
                    g_mFormMain.PrintInformation("[ERRO]", "Pre-Processing failed! Could not get Pre-Processed source file!")
                    Return Nothing
                End If

                Dim sOutputSource As String = IO.File.ReadAllText(sOutputFile)

                IO.File.Delete(sOutputFile)


                Dim sList As New List(Of String)
                Dim bRecord As Boolean = False

                Dim sLine As String
                Using SR As New IO.StringReader(sOutputSource)
                    While True
                        sLine = SR.ReadLine()
                        If (sLine Is Nothing) Then
                            Exit While
                        End If

                        If (bCleanUpSourcemodDuplicate) Then
                            If (Not bRecord) Then
                                If (sLine.Contains("#file " & sMarkStart)) Then
                                    bRecord = True
                                    Continue While
                                Else
                                    If (sList.Count > 0) Then
                                        sList.Clear()
                                    End If
                                    Continue While
                                End If
                            Else
                                'Remove invalid lines
                                If (sLine.Contains("#line 0")) Then
                                    Continue While
                                End If

                                If (sLine.Contains("#file " & sMarkEnd)) Then
                                    Exit While
                                End If
                            End If
                        End If

                        'Dont trim empty lines, breaks the line numbers
                        'If (String.IsNullOrEmpty(sLine.Trim)) Then
                        '    Continue While
                        'End If

                        sList.Add(sLine)
                    End While
                End Using

                Dim sNewSource = String.Join(Environment.NewLine, sList.ToArray)

                If (bCleanupForCompile) Then
                    sNewSource = Regex.Replace(sNewSource, "^\s*#\b(endinput)\b", "", RegexOptions.Multiline)
                End If


                g_mFormMain.PrintInformation("[INFO]", "Pre-Processing source finished!")

                Return sNewSource
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try

            Return Nothing
        End Function

        ''' <summary>
        ''' Gets the assembly from the code. Throws exceptions on compile error.
        ''' </summary> 
        ''' <returns></returns>
        Public Function GetCompilerAssemblyCode() As String
            Try
                If (PromptSave(False, True)) Then
                    Return Nothing
                End If

                If (g_mFormMain.SplitContainer1.Panel2Collapsed) Then
                    g_mFormMain.SplitContainer1.Panel2Collapsed = False
                    g_mFormMain.SplitContainer1.SplitterDistance = g_mFormMain.SplitContainer1.Height - 200
                End If
                g_mFormMain.TabControl_Details.SelectTab(1)

                g_mFormMain.PrintInformation("[INFO]", "DIASM source started!")

                Dim sCompilerPath As String = ""
                Dim sIncludePath As String = ""
                Dim sOutputFile As String = ""

                Dim iExitCode As Integer = 0
                Dim sOutput As String = ""

                If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                    g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Could not get current source file!")
                    Return Nothing
                End If

                'Check compiler
                If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    While True
                        'SourcePawn
                        sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "spcomp.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'AMX Mod X
                        sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "amxxpc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'Small
                        sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "sc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        'Pawn
                        sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "pawncc.exe")
                        If (IO.File.Exists(sCompilerPath)) Then
                            Exit While
                        End If

                        g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Compiler can not be found!")
                        Return Nothing
                    End While
                Else
                    sCompilerPath = ClassSettings.g_sConfigCompilerPath
                    If (Not IO.File.Exists(sCompilerPath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Compiler can not be found!")
                        Return Nothing
                    End If
                End If

                'Check include path
                If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                    sIncludePath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "include")
                    If (Not IO.Directory.Exists(sIncludePath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Include path can not be found!")
                        Return Nothing
                    End If
                Else
                    sIncludePath = ClassSettings.g_sConfigOpenIncludeFolder
                    If (Not IO.Directory.Exists(sIncludePath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Include path can not be found!")
                        Return Nothing
                    End If
                End If


                Dim sTmpSource As String = IO.File.ReadAllText(ClassSettings.g_sConfigOpenSourceFile)
                Dim sTmpSourcePath As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))

                IO.File.WriteAllText(sTmpSourcePath, sTmpSource)

                sOutputFile = String.Format("{0}.asm", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))

                Dim sArguments As String = String.Format("""{0}"" -a -i""{1}"" -o""{2}""", sTmpSourcePath, sIncludePath, sOutputFile)
                ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, sArguments, iExitCode, sOutput)

                IO.File.Delete(sTmpSourcePath)

                Dim sLines As String() = sOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
                For i = sLines.Length - 1 To 0 Step -1
                    g_mFormMain.PrintInformation("[INFO]", vbTab & sLines(i))
                Next

                If (String.IsNullOrEmpty(sOutputFile) OrElse Not IO.File.Exists(sOutputFile)) Then
                    g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Could not get Pre-Processed source file!")
                    Return Nothing
                End If

                Dim sOutputSource As String = IO.File.ReadAllText(sOutputFile)

                IO.File.Delete(sOutputFile)


                g_mFormMain.PrintInformation("[INFO]", "DIASM source finished!")

                Return sOutputSource
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try

            Return Nothing
        End Function

        ''' <summary>
        ''' Gets the assembly from the code. Throws exceptions on compile error.
        ''' </summary> 
        ''' <returns></returns>
        Public Function GetCompilerAssemblyCode(bTesting As Boolean, sSource As String, sOutputFile As String, Optional sCompilerPath As String = Nothing, Optional sIncludePath As String = Nothing) As String
            Try
                If (PromptSave(False, True)) Then
                    Return Nothing
                End If

                If (g_mFormMain.SplitContainer1.Panel2Collapsed) Then
                    g_mFormMain.SplitContainer1.Panel2Collapsed = False
                    g_mFormMain.SplitContainer1.SplitterDistance = g_mFormMain.SplitContainer1.Height - 200
                End If
                g_mFormMain.TabControl_Details.SelectTab(1)

                g_mFormMain.PrintInformation("[INFO]", "DIASM source started!")

                Dim iExitCode As Integer = 0
                Dim sOutput As String = ""

                'Check compiler
                If (sCompilerPath Is Nothing) Then
                    If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                        If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                            g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Could not get current source file!")
                            Return Nothing
                        End If

                        While True
                            'SourcePawn
                            sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "spcomp.exe")
                            If (IO.File.Exists(sCompilerPath)) Then
                                Exit While
                            End If

                            'AMX Mod X
                            sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "amxxpc.exe")
                            If (IO.File.Exists(sCompilerPath)) Then
                                Exit While
                            End If

                            'Small
                            sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "sc.exe")
                            If (IO.File.Exists(sCompilerPath)) Then
                                Exit While
                            End If

                            'Pawn
                            sCompilerPath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "pawncc.exe")
                            If (IO.File.Exists(sCompilerPath)) Then
                                Exit While
                            End If

                            g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Compiler can not be found!")
                            Return Nothing
                        End While
                    Else
                        sCompilerPath = ClassSettings.g_sConfigCompilerPath
                        If (Not IO.File.Exists(sCompilerPath)) Then
                            g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Compiler can not be found!")
                            Return Nothing
                        End If
                    End If
                Else
                    If (Not IO.File.Exists(sCompilerPath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Compiler can not be found!")
                        Return Nothing
                    End If
                End If

                'Check include path
                If (sIncludePath Is Nothing) Then
                    If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                        If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                            g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Could not get current source file!")
                            Return Nothing
                        End If

                        sIncludePath = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "include")
                        If (Not IO.Directory.Exists(sIncludePath)) Then
                            g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Include path can not be found!")
                            Return Nothing
                        End If
                    Else
                        sIncludePath = ClassSettings.g_sConfigOpenIncludeFolder
                        If (Not IO.Directory.Exists(sIncludePath)) Then
                            g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Include path can not be found!")
                            Return Nothing
                        End If
                    End If
                Else
                    If (Not IO.Directory.Exists(sIncludePath)) Then
                        g_mFormMain.PrintInformation("[ERRO]", "DIASM failed! Include path can not be found!")
                        Return Nothing
                    End If
                End If

                'Set output path
                If (bTesting) Then
                    sOutputFile = String.Format("{0}.asm", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
                End If

                Dim TmpSourceFile As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
                IO.File.WriteAllText(TmpSourceFile, sSource)

                Dim sArguments As String = String.Format("""{0}"" -a -i""{1}"" -o""{2}""", TmpSourceFile, sIncludePath, sOutputFile)
                ClassTools.ClassProcess.ExecuteProgram(sCompilerPath, sArguments, iExitCode, sOutput)

                Dim sLines As String() = sOutput.Split(New String() {Environment.NewLine, vbLf}, 0)
                For i = sLines.Length - 1 To 0 Step -1
                    g_mFormMain.PrintInformation("[INFO]", vbTab & sLines(i))
                Next

                Dim sAssemblySource As String = IO.File.ReadAllText(sOutputFile)

                IO.File.Delete(TmpSourceFile)

                If (bTesting) Then
                    IO.File.Delete(sOutputFile)
                End If

                If (Not bTesting) Then
                    g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Saved DIASM source: {0}", sOutputFile))
                End If
                g_mFormMain.PrintInformation("[INFO]", "DIASM source finished!")

                Return sAssemblySource
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try

            Return Nothing
        End Function

        ''' <summary>
        ''' Class for custom color highlighting.
        ''' </summary>
        Public Class ClassCustomHighlighting
            Implements IDisposable

            Private g_mFormMain As FormMain
            Private Const g_sColorTextName As String = "Highlighting"

            Class STRUC_HIGHLIGHT_ITEM
                Public sIdentifier As String
                Public sWord As String
                Public mToolStripItem As ToolStripMenuItem
            End Class

            Private g_lHighlightItemList As New List(Of STRUC_HIGHLIGHT_ITEM)

            ReadOnly Property m_HightlightItems As STRUC_HIGHLIGHT_ITEM()
                Get
                    Return g_lHighlightItemList.ToArray
                End Get
            End Property

            Public Sub New(f As FormMain)
                g_mFormMain = f
            End Sub

            ''' <summary>
            ''' Adds a highlight item. If exist, it will be ignored.
            ''' The highlight list will automatically allocated.
            ''' </summary>
            ''' <param name="iIndex"></param>
            Public Sub Add(iIndex As Integer)
                AllocateList(iIndex)

                If (g_lHighlightItemList(iIndex) IsNot Nothing) Then
                    Return
                End If

                Dim sIdentifier = Guid.NewGuid.ToString

                Dim dropItem As ToolStripMenuItem = CType(g_mFormMain.Invoke(Function() g_mFormMain.ToolStripMenuItem_HightlightCustom.DropDownItems.Add(String.Format("{0} {1}", g_sColorTextName, iIndex))), ToolStripMenuItem)
                dropItem.Name = sIdentifier
                dropItem.BackColor = Color.White

                RemoveHandler dropItem.Click, AddressOf OnClick
                AddHandler dropItem.Click, AddressOf OnClick

                Dim highlightItem As New STRUC_HIGHLIGHT_ITEM
                highlightItem.sIdentifier = sIdentifier
                highlightItem.sWord = ""
                highlightItem.mToolStripItem = dropItem

                g_lHighlightItemList(iIndex) = highlightItem
            End Sub

            ''' <summary>
            ''' Cleans the highlight list, events and disposes all ToolStrip controls.
            ''' </summary>
            Public Sub Clear()
                For i = 0 To g_lHighlightItemList.Count - 1
                    If (g_lHighlightItemList(i) Is Nothing) Then
                        Continue For
                    End If

                    RemoveHandler g_lHighlightItemList(i).mToolStripItem.Click, AddressOf OnClick

                    g_lHighlightItemList(i).mToolStripItem.Dispose()
                    g_lHighlightItemList(i).mToolStripItem = Nothing
                Next

                g_lHighlightItemList.Clear()
            End Sub

            Public Function AllocateList(iSize As Integer) As Boolean
                Dim bAllocated As Boolean = False

                While (iSize > g_lHighlightItemList.Count - 1)
                    bAllocated = True
                    g_lHighlightItemList.Add(Nothing)
                End While

                Return bAllocated
            End Function

            Private Sub OnClick(sender As Object, e As EventArgs)
                Try
                    Dim toolStripItem As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
                    If (toolStripItem Is Nothing) Then
                        Throw New ArgumentException("Invalid ToolStripMenuItem")
                    End If

                    Dim sIdentifier As String = toolStripItem.Name
                    If (String.IsNullOrEmpty(sIdentifier)) Then
                        Throw New ArgumentException("Invalid ToolStripMenuItem name")
                    End If

                    Dim iIndex As Integer = -1
                    Dim highlightItem As STRUC_HIGHLIGHT_ITEM = Nothing

                    For i = 0 To g_lHighlightItemList.Count - 1
                        If (g_lHighlightItemList(i) Is Nothing) Then
                            Continue For
                        End If

                        If (g_lHighlightItemList(i).sIdentifier <> sIdentifier) Then
                            Continue For
                        End If

                        iIndex = i
                        highlightItem = g_lHighlightItemList(i)
                    Next

                    If (highlightItem Is Nothing OrElse iIndex < 0) Then
                        Throw New ArgumentException("No matching ToolStripMenuItem found")
                    End If

                    highlightItem.sWord = ""

                    If (g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                        Dim m_CurrentSelection As ISelection = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection(0)

                        If (Regex.IsMatch(m_CurrentSelection.SelectedText, "^[a-zA-Z0-9_]+$")) Then
                            highlightItem.sWord = m_CurrentSelection.SelectedText
                        End If
                    Else
                        Dim sWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(False)

                        If (Not String.IsNullOrEmpty(sWord)) Then
                            highlightItem.sWord = sWord
                        End If
                    End If

                    If (String.IsNullOrEmpty(highlightItem.sWord)) Then
                        toolStripItem.Text = String.Format("{0} {1}", g_sColorTextName, iIndex)
                    Else
                        toolStripItem.Text = String.Format("{0} {1} {2}", g_sColorTextName, iIndex, "(Visible)")
                    End If

                    g_mFormMain.g_ClassSyntaxTools.UpdateSyntaxFile(ClassSyntaxTools.ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD_CUSTOM)
                    g_mFormMain.g_ClassSyntaxTools.UpdateTextEditorSyntax()
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            End Sub

#Region "IDisposable Support"
            Private disposedValue As Boolean ' To detect redundant calls

            ' IDisposable
            Protected Overridable Sub Dispose(disposing As Boolean)
                If Not Me.disposedValue Then
                    If disposing Then
                        ' TODO: dispose managed state (managed objects).
                        Clear()
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

        ''' <summary>
        ''' Class for custom text editor iconbar icons if a line has been changed/saved.
        ''' </summary>
        Class ClassLineState
            Private g_mFormMain As FormMain

            Public Sub New(f As FormMain)
                g_mFormMain = f
            End Sub

            Property m_IgnoreUpdates As Boolean

            Property m_LineState(iIndex As Integer) As LineStateBookmark.ENUM_BOOKMARK_TYPE
                Get
                    For Each item In g_mFormMain.TextEditorControl_Source.Document.BookmarkManager.Marks
                        Dim lineStateBookmark As LineStateBookmark = TryCast(item, LineStateBookmark)
                        If (lineStateBookmark Is Nothing) Then
                            Continue For
                        End If

                        If (lineStateBookmark.LineNumber <> iIndex) Then
                            Continue For
                        End If

                        Return lineStateBookmark.m_iType
                    Next

                    Return LineStateBookmark.ENUM_BOOKMARK_TYPE.NONE
                End Get
                Set(value As LineStateBookmark.ENUM_BOOKMARK_TYPE)
                    If (m_IgnoreUpdates) Then
                        g_mFormMain.TextEditorControl_Source.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, iIndex))
                        g_mFormMain.TextEditorControl_Source.Document.CommitUpdate()
                        Return
                    End If

                    For Each item In g_mFormMain.TextEditorControl_Source.Document.BookmarkManager.Marks
                        Dim lineStateBookmark As LineStateBookmark = TryCast(item, LineStateBookmark)
                        If (lineStateBookmark Is Nothing) Then
                            Continue For
                        End If

                        If (lineStateBookmark.LineNumber <> iIndex) Then
                            Continue For
                        End If

                        lineStateBookmark.m_iType = value
                        Return
                    Next

                    Dim mBookmark As New LineStateBookmark(g_mFormMain.TextEditorControl_Source.Document, New TextLocation(0, iIndex), True, LineStateBookmark.ENUM_BOOKMARK_TYPE.CHANGED)
                    g_mFormMain.TextEditorControl_Source.Document.BookmarkManager.AddMark(mBookmark)

                    g_mFormMain.TextEditorControl_Source.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, iIndex))
                    g_mFormMain.TextEditorControl_Source.Document.CommitUpdate()
                End Set
            End Property

            ''' <summary>
            ''' Change all 'changed' states to 'saved'.
            ''' </summary>
            Public Sub SaveStates()
                For Each item In g_mFormMain.TextEditorControl_Source.Document.BookmarkManager.Marks
                    Dim lineStateBookmark As LineStateBookmark = TryCast(item, LineStateBookmark)
                    If (lineStateBookmark Is Nothing) Then
                        Continue For
                    End If

                    If (lineStateBookmark.m_iType = LineStateBookmark.ENUM_BOOKMARK_TYPE.CHANGED) Then
                        lineStateBookmark.m_iType = LineStateBookmark.ENUM_BOOKMARK_TYPE.SAVED

                        g_mFormMain.TextEditorControl_Source.Document.RequestUpdate(New TextAreaUpdate(TextAreaUpdateType.SingleLine, lineStateBookmark.LineNumber))
                    End If
                Next

                g_mFormMain.TextEditorControl_Source.Document.CommitUpdate()
            End Sub

            ''' <summary>
            ''' Clear all line states. Like on loading or new source.
            ''' </summary>
            Public Sub ClearStates()
                g_mFormMain.TextEditorControl_Source.Document.BookmarkManager.RemoveMarks(Function(mBookmark As Bookmark) TryCast(mBookmark, LineStateBookmark) IsNot Nothing)
            End Sub

            ''' <summary>
            ''' Custom bookmark to display custom icons in the iconbar.
            ''' </summary>
            Public Class LineStateBookmark
                Inherits Bookmark

                Enum ENUM_BOOKMARK_TYPE
                    NONE
                    CHANGED
                    SAVED
                End Enum

                Private g_sIdentifier As String = Guid.NewGuid.ToString

                Property m_iType As ENUM_BOOKMARK_TYPE

                Public Sub New(iDocument As IDocument, textLocation As TextLocation, bEnabled As Boolean, iType As ENUM_BOOKMARK_TYPE)
                    MyBase.New(iDocument, textLocation, bEnabled)
                    m_iType = iType
                End Sub

                Public Overrides ReadOnly Property CanToggle As Boolean
                    Get
                        Return False
                        'Return MyBase.CanToggle
                    End Get
                End Property

                Protected Overrides Sub OnIsEnabledChanged(e As EventArgs)
                    IsEnabled = True
                    'MyBase.OnIsEnabledChanged(e)
                End Sub

                ''' <summary>
                ''' Lets override the bookmarks drawing, so we can show our custom coloring!
                ''' The bookmark is the only real icon we can change.
                ''' </summary>
                ''' <param name="iconBarMargin"></param>
                ''' <param name="iGraphics"></param>
                ''' <param name="iPoint"></param>
                Public Overrides Sub Draw(iconBarMargin As IconBarMargin, iGraphics As Graphics, iPoint As Point)
                    'Dim num As Integer = margin.TextArea.TextView.FontHeight / 8
                    'Dim r As Rectangle = New Rectangle(1, p.Y + num, margin.DrawingPosition.Width - 4, margin.TextArea.TextView.FontHeight - num * 2)
                    Dim calcFontHeight As Integer = CInt(iconBarMargin.TextArea.TextView.FontHeight / 32)
                    Dim r As New Rectangle(CInt(iconBarMargin.DrawingPosition.Width / 1.25),
                                            iPoint.Y + calcFontHeight,
                                            iconBarMargin.DrawingPosition.Width - 4,
                                            iconBarMargin.TextArea.TextView.FontHeight - calcFontHeight * 2)

                    Dim iColor As Color = If(m_iType = ENUM_BOOKMARK_TYPE.CHANGED, Color.Orange, Color.Green)

                    Using mBrush As New Drawing2D.LinearGradientBrush(New Point(r.Left, r.Top), New Point(r.Right, r.Bottom), iColor, iColor)
                        iGraphics.FillRectangle(mBrush, r)
                    End Using
                End Sub
            End Class
        End Class
    End Class

    Public Class ClassSyntaxUpdater
        Private g_mFormMain As FormMain
        Private g_mSourceSyntaxUpdaterThread As Threading.Thread

        Public Sub New(f As FormMain)
            g_mFormMain = f
        End Sub

        ''' <summary>
        ''' Starts the updater thread
        ''' </summary>
        Public Sub StartThread()
            If (g_mSourceSyntaxUpdaterThread Is Nothing OrElse Not g_mSourceSyntaxUpdaterThread.IsAlive) Then
                g_mSourceSyntaxUpdaterThread = New Threading.Thread(AddressOf SourceSyntaxUpdater_Thread)
                g_mSourceSyntaxUpdaterThread.IsBackground = True
                g_mSourceSyntaxUpdaterThread.Start()
            End If
        End Sub

        ''' <summary>
        ''' Stops the updater thread
        ''' </summary>
        Public Sub StopThread()
            If (g_mSourceSyntaxUpdaterThread IsNot Nothing AndAlso g_mSourceSyntaxUpdaterThread.IsAlive) Then
                g_mSourceSyntaxUpdaterThread.Abort()
                g_mSourceSyntaxUpdaterThread.Join()
                g_mSourceSyntaxUpdaterThread = Nothing
            End If
        End Sub

        ''' <summary>
        ''' The main thread to update all kinds of stuff.
        ''' </summary>
        Private Sub SourceSyntaxUpdater_Thread()
            Static g_mLastFullAutocompleteUpdate As Date = Now
            Static g_mLastVarAutocompleteUpdate As Date = Now
            Static g_mLastMethodAutocompleteUpdate As Date = Now
            Static g_mLastFoldingUpdate As Date = Now

            While True
                Threading.Thread.Sleep(500)

                Try
                    'Update Autocomplete
                    If (g_mLastFullAutocompleteUpdate < Now OrElse g_mFormMain.g_ClassAutocompleteUpdater.g_bForceFullAutocompleteUpdate) Then
                        g_mLastFullAutocompleteUpdate = (Now + New TimeSpan(0, 0, 1, 0, 0))

                        g_mFormMain.BeginInvoke(Sub() g_mFormMain.g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL))
                    End If

                    'Update Autocomplete
                    If (g_mLastVarAutocompleteUpdate < Now) Then
                        g_mLastVarAutocompleteUpdate = (Now + New TimeSpan(0, 0, 0, 10, 0))

                        g_mFormMain.BeginInvoke(Sub() g_mFormMain.g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.VARIABLES_AUTOCOMPLETE))
                    End If

                    'Update method Autocomplete
                    If (g_mLastMethodAutocompleteUpdate < Now) Then
                        g_mLastMethodAutocompleteUpdate = (Now + New TimeSpan(0, 0, 0, 10, 0))

                        Dim sTextContent As String = CStr(g_mFormMain.Invoke(Function() g_mFormMain.TextEditorControl_Source.Document.TextContent))
                        g_mFormMain.g_mSourceSyntaxSourceAnalysis = New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sTextContent)
                    End If

                    'Update Foldings
                    If (g_mLastFoldingUpdate < Now) Then
                        g_mLastFoldingUpdate = Now + New TimeSpan(0, 0, 5)

                        'If ((Tools.WordCount(Me.Invoke(Function() TextEditorControl1.Document.TextContent), "{") + Tools.WordCount(Me.Invoke(Function() TextEditorControl1.Document.TextContent), "}")) Mod 2 = 0) Then
                        g_mFormMain.BeginInvoke(Sub() g_mFormMain.TextEditorControl_Source.Document.FoldingManager.UpdateFoldings(Nothing, Nothing))
                        'End If
                    End If


                    Dim iCaretOffset As Integer = CInt(g_mFormMain.Invoke(Function() g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Offset))
                    Dim iCaretPos As Integer = CInt(g_mFormMain.Invoke(Function() g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.ScreenPosition.X + g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.ScreenPosition.Y))

                    'Update Method Autoupdate 
                    Static iLastMethodAutoupdateCaretOffset As Integer = -1
                    If (iLastMethodAutoupdateCaretOffset <> iCaretOffset) Then
                        iLastMethodAutoupdateCaretOffset = iCaretOffset

                        If (Not g_mFormMain.ParseMethodAutocomplete(True)) Then
                            g_mFormMain.BeginInvoke(Sub()
                                                        g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.CurrentMethod = ""
                                                        g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
                                                    End Sub)
                        End If
                    End If

                    'Update Autocomplete
                    Static iLastAutoupdateCaretOffset As Integer = -1
                    Static iLastAutoupdateCaretPos As Integer = -1
                    If (iLastAutoupdateCaretPos <> iCaretPos) Then
                        iLastAutoupdateCaretPos = iCaretPos

                        If (iLastAutoupdateCaretOffset = iCaretOffset) Then
                            g_mFormMain.BeginInvoke(Sub()
                                                        g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete("")
                                                        g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.CurrentMethod = ""
                                                        g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
                                                    End Sub)
                        End If

                        iLastAutoupdateCaretOffset = iCaretOffset
                    End If

                    'Update Autocomplete
                    Static iLastAutoupdateCaretOffset2 As Integer = -1
                    If (iLastAutoupdateCaretOffset2 <> iCaretOffset) Then
                        iLastAutoupdateCaretOffset2 = iCaretOffset


                        If (iCaretOffset > -1 AndAlso iCaretOffset < g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.TextLength) Then
                            Dim iPosition As Integer = CInt(g_mFormMain.Invoke(Function() g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Position.Column))
                            Dim iLineOffset As Integer = CInt(g_mFormMain.Invoke(Function() g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iCaretOffset).Offset))
                            Dim iLineLen As Integer = CInt(g_mFormMain.Invoke(Function() g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iCaretOffset).Length))

                            If ((iLineLen - iPosition) > 0) Then
                                Dim sFunctionName As String = CType(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True)), String)

                                If (CInt(g_mFormMain.Invoke(Function() g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete(sFunctionName))) < 1) Then
                                    sFunctionName = CStr(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTextEditorTools.GetCaretWord(False)))

                                    g_mFormMain.Invoke(Sub() g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete(sFunctionName))
                                    g_mFormMain.Invoke(Sub() g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip())
                                Else
                                    g_mFormMain.Invoke(Sub() g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip())
                                End If
                            Else
                                g_mFormMain.Invoke(Sub() g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip())
                            End If
                        Else
                            g_mFormMain.Invoke(Sub() g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip())
                        End If
                    End If

                    'Update caret word maker
                    Static iLastAutoupdateCaretOffset3 As Integer = -1
                    If (iLastAutoupdateCaretOffset3 <> iCaretOffset) Then
                        iLastAutoupdateCaretOffset3 = iCaretOffset

                        If (ClassSettings.g_iSettingsAutoMark) Then
                            g_mFormMain.BeginInvoke(Sub() g_mFormMain.g_ClassTextEditorTools.MarkCaretWord())
                        End If
                    End If

                Catch ex As Threading.ThreadAbortException
                    Throw
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                    Threading.Thread.Sleep(5000)
                End Try
            End While
        End Sub

    End Class

    Public Class ClassSyntaxTools
        Private g_mFormMain As FormMain

        Public g_sStatementsArray As String() = {"if", "else", "for", "while", "do", "switch"}

        Public g_sHighlightWord As String = ""
        Public g_sCaretWord As String = ""

        Enum ENUM_SYNTAX_FILES
            MAIN_TEXTEDITOR
            DEBUGGER_TEXTEDITOR
        End Enum
        Structure STRUC_SYNTAX_FILES_ITEM
            Dim sFile As String
            Dim sFolder As String
            Dim sDefinition As String
        End Structure
        Public g_SyntaxFiles([Enum].GetNames(GetType(ENUM_SYNTAX_FILES)).Length - 1) As STRUC_SYNTAX_FILES_ITEM

        Public g_SyntaxXML As String = My.Resources.SourcePawn_Syntax

        Public sSyntax_HighlightCaretMarker As String = "<!-- [DO NOT EDIT | HIGHLIGHT CARET MARKER] -->"
        Public sSyntax_HighlightWordMarker As String = "<!-- [DO NOT EDIT | HIGHLIGHT WORD MARKER] -->"
        Public sSyntax_HighlightWordCustomMarker As String = "<!-- [DO NOT EDIT | HIGHLIGHT WORD CUSTOM MARKER] -->"
        Public sSyntax_HighlightDefineMarker As String = "<!-- [DO NOT EDIT | DEFINE MARKER] -->"
        Public sSyntax_HighlightEnumMarker As String = "<!-- [DO NOT EDIT | ENUM MARKER] -->"
        Public sSyntax_HighlightEnum2Marker As String = "<!-- [DO NOT EDIT | ENUM2 MARKER] -->"
        Public sSyntax_SourcePawnMarker As String = "SourcePawn-04e3632f-5472-42c5-929a-c3e0c2b35324"

        Public lAutocompleteList As New ClassSyncList(Of STRUC_AUTOCOMPLETE)

        Private _lock As Object = New Object()

        Public Enum ENUM_SYNTAX_UPDATE_TYPE
            NONE
            CARET_WORD
            HIGHLIGHT_WORD
            HIGHLIGHT_WORD_CUSTOM
            AUTOCOMPLETE
        End Enum

        Public Sub New(f As FormMain)
            g_mFormMain = f

            'Add syntax Files for TextEditor
            Dim sSyntaxWorkingDir As String = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.Temp, Guid.NewGuid.ToString)

            g_SyntaxFiles(ENUM_SYNTAX_FILES.MAIN_TEXTEDITOR) = New STRUC_SYNTAX_FILES_ITEM() With {
                                                                        .sFile = String.Format("{0}.xshd", IO.Path.Combine(sSyntaxWorkingDir, Guid.NewGuid.ToString)),
                                                                        .sFolder = sSyntaxWorkingDir,
                                                                        .sDefinition = "SourcePawn-MainTextEditor-" & Guid.NewGuid.ToString}

            g_SyntaxFiles(ENUM_SYNTAX_FILES.DEBUGGER_TEXTEDITOR) = New STRUC_SYNTAX_FILES_ITEM() With {
                                                                        .sFile = String.Format("{0}.xshd", IO.Path.Combine(sSyntaxWorkingDir, Guid.NewGuid.ToString)),
                                                                        .sFolder = sSyntaxWorkingDir,
                                                                        .sDefinition = "SourcePawn-DebugTextEditor-" & Guid.NewGuid.ToString}

            'Add all syntax files to the provider, only once
            For i = 0 To g_SyntaxFiles.Length - 1
                CreateSyntaxFile(CType(i, ENUM_SYNTAX_FILES))

                Dim synraxProvider As New FileSyntaxModeProvider(g_SyntaxFiles(i).sFolder)
                HighlightingManager.Manager.AddSyntaxModeFileProvider(synraxProvider)
            Next
        End Sub


        Public Class STRUC_FORM_COLORS_ITEM
            Public g_cControl As Control
            Public g_cBackColorOrg As Color
            Public g_cBackColorInv As Color
            Public g_cForeColorOrg As Color
            Public g_cForeColorInv As Color

            Public Sub New(cControl As Control, cBackColorOrg As Color, cBackColorInv As Color, cForeColorOrg As Color, cForeColorInv As Color)
                g_cControl = cControl
                g_cBackColorOrg = cBackColorOrg
                g_cBackColorInv = cBackColorInv
                g_cForeColorOrg = cForeColorOrg
                g_cForeColorInv = cForeColorInv
            End Sub
        End Class

        ''' <summary>
        ''' Updates the form colors and syntax.
        ''' </summary>
        Public Sub UpdateFormColors()
            Dim lControlList As New List(Of STRUC_FORM_COLORS_ITEM)
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain, Color.White, g_mFormMain.g_cDarkFormBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.TextEditorControl_Source, Color.White, g_mFormMain.g_cDarkFormBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.MenuStrip_BasicPawn, Color.White, g_mFormMain.g_cDarkFormMenuBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))

            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCAutocomplete, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCAutocomplete.ListView_AutocompleteList, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCAutocomplete.Panel_Autocomplete, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCAutocomplete.Panel_IntelliSense, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCAutocomplete.Label_Autocomplete, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, Color.RoyalBlue, InvertColor(Color.RoyalBlue)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCAutocomplete.Label_IntelliSense, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, Color.RoyalBlue, InvertColor(Color.RoyalBlue)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCAutocomplete.RichTextBox_Autocomplete, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCAutocomplete.RichTextBox_IntelliSense, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCAutocomplete.SplitContainer1.Panel1, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCAutocomplete.SplitContainer1.Panel2, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCAutocomplete.SplitContainer2.Panel1, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCAutocomplete.SplitContainer2.Panel2, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))

            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCInformationList, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCInformationList.ListBox_Information, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))

            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCObjectBrowser, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCObjectBrowser.TreeView_ObjectBrowser, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCObjectBrowser.TextboxWatermark_Search, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))

            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.TabControl_Details, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.TabControl_Toolbox, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))

            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.TabPage_ObjectBrowser, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.TabPage_Autocomplete, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.TabPage_Information, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))

            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCToolTip, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))
            lControlList.Add(New STRUC_FORM_COLORS_ITEM(g_mFormMain.g_mUCToolTip.TextEditorControl_ToolTip, Color.White, g_mFormMain.g_cDarkFormDetailsBackgroundColor, InvertColor(Color.White), InvertColor(Color.Black)))

            For Each iItem In lControlList
                iItem.g_cControl.BackColor = If(ClassSettings.g_iSettingsInvertColors, iItem.g_cBackColorInv, iItem.g_cBackColorOrg)
                iItem.g_cControl.ForeColor = If(ClassSettings.g_iSettingsInvertColors, iItem.g_cForeColorInv, iItem.g_cForeColorOrg)
            Next

            UpdateSyntaxFile(ENUM_SYNTAX_UPDATE_TYPE.NONE, True) 'Just generate new files once, we dont need to create new files every type.
            UpdateSyntaxFile(ENUM_SYNTAX_UPDATE_TYPE.AUTOCOMPLETE)
            UpdateSyntaxFile(ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD)
            UpdateSyntaxFile(ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD_CUSTOM)
            UpdateSyntaxFile(ENUM_SYNTAX_UPDATE_TYPE.CARET_WORD)
            UpdateTextEditorSyntax()
        End Sub

        ''' <summary>
        ''' Invert the color
        ''' </summary>
        ''' <param name="cColor"></param>
        ''' <returns></returns>
        Public Function InvertColor(cColor As Color) As Color
            Dim cNewColor As Color = Color.FromArgb(cColor.ToArgb Xor -1) '&HFFFFFF 
            Return Color.FromArgb(cColor.A, cNewColor.R, cNewColor.G, cNewColor.B)
        End Function

        ''' <summary>
        ''' Checks if the syntax files exist. If not, they will be created.
        ''' </summary>
        Public Sub CreateSyntaxFile(i As ENUM_SYNTAX_FILES)
            Try
                SyncLock _lock
                    IO.Directory.CreateDirectory(g_SyntaxFiles(i).sFolder)

                    Dim sModSyntaxXML As String
                    If (Not String.IsNullOrEmpty(ClassSettings.g_sConfigSyntaxHighlightingPath) AndAlso
                            IO.File.Exists(ClassSettings.g_sConfigSyntaxHighlightingPath) AndAlso
                            IO.Path.GetExtension(ClassSettings.g_sConfigSyntaxHighlightingPath).ToLower = ".xml") Then
                        Dim sFileText As String = IO.File.ReadAllText(ClassSettings.g_sConfigSyntaxHighlightingPath)
                        sModSyntaxXML = sFileText.Replace(sSyntax_SourcePawnMarker, g_SyntaxFiles(i).sDefinition)
                    Else
                        sModSyntaxXML = g_SyntaxXML.Replace(sSyntax_SourcePawnMarker, g_SyntaxFiles(i).sDefinition)
                    End If

                    IO.File.WriteAllText(g_SyntaxFiles(i).sFile, sModSyntaxXML)
                End SyncLock
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Updates the syntax file with new information (e.g highlights, defines, enums etc. from the includes and source).
        ''' This only changes the syntax file.
        ''' </summary>
        ''' <param name="iType"></param>
        ''' <param name="bForceFromMemory">If true, overwrites the syntax file from memory cache (factory new)</param>
        Public Sub UpdateSyntaxFile(iType As ENUM_SYNTAX_UPDATE_TYPE, Optional bForceFromMemory As Boolean = False)
            Try
                SyncLock _lock
                    For i = 0 To g_SyntaxFiles.Length - 1
                        If (Not IO.File.Exists(g_SyntaxFiles(i).sFile) OrElse bForceFromMemory) Then
                            CreateSyntaxFile(CType(i, ENUM_SYNTAX_FILES))
                        End If

                        Select Case (i)
                            Case ENUM_SYNTAX_FILES.MAIN_TEXTEDITOR,
                                    ENUM_SYNTAX_FILES.DEBUGGER_TEXTEDITOR
                                Dim SB As New StringBuilder

                                Dim iHighlightCustomCount As Integer = 0

                                Using SR As New IO.StreamReader(g_SyntaxFiles(i).sFile)
                                    Dim sLine As String

                                    While True
                                        sLine = SR.ReadLine
                                        If (sLine Is Nothing) Then
                                            Exit While
                                        End If

                                        Select Case (iType)
                                            Case ENUM_SYNTAX_UPDATE_TYPE.CARET_WORD
                                                If (i = ENUM_SYNTAX_FILES.MAIN_TEXTEDITOR AndAlso
                                                        sLine.Contains(sSyntax_HighlightCaretMarker)) Then

                                                    SB.Append(sSyntax_HighlightCaretMarker)

                                                    If (Not String.IsNullOrEmpty(g_sCaretWord) AndAlso ClassSettings.g_iSettingsAutoMark) Then
                                                        SB.Append(String.Format("<Key word=""{0}""/>", g_sCaretWord))
                                                    End If

                                                    SB.AppendLine()
                                                    Continue While
                                                End If

                                            Case ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD
                                                If (i = ENUM_SYNTAX_FILES.MAIN_TEXTEDITOR AndAlso
                                                        sLine.Contains(sSyntax_HighlightWordMarker)) Then

                                                    SB.Append(sSyntax_HighlightWordMarker)

                                                    If (Not String.IsNullOrEmpty(g_sHighlightWord)) Then
                                                        SB.Append(String.Format("<Key word=""{0}""/>", g_sHighlightWord))
                                                    End If

                                                    SB.AppendLine()
                                                    Continue While
                                                End If

                                            Case ENUM_SYNTAX_UPDATE_TYPE.HIGHLIGHT_WORD_CUSTOM
                                                If (i = ENUM_SYNTAX_FILES.MAIN_TEXTEDITOR AndAlso
                                                        sLine.Contains(sSyntax_HighlightWordCustomMarker)) Then

                                                    SB.Append(sSyntax_HighlightWordCustomMarker)

                                                    g_mFormMain.g_ClassCustomHighlighting.Add(iHighlightCustomCount)
                                                    Dim menuItem = g_mFormMain.g_ClassCustomHighlighting.m_HightlightItems()

                                                    If (menuItem(iHighlightCustomCount) IsNot Nothing AndAlso Not String.IsNullOrEmpty(menuItem(iHighlightCustomCount).sWord)) Then
                                                        SB.Append(String.Format("<Key word=""{0}""/>", menuItem(iHighlightCustomCount).sWord))
                                                    End If

                                                    SB.AppendLine()
                                                    iHighlightCustomCount += 1
                                                    Continue While
                                                End If

                                            Case ENUM_SYNTAX_UPDATE_TYPE.AUTOCOMPLETE
                                                If (sLine.Contains(sSyntax_HighlightDefineMarker)) Then
                                                    SB.Append(sSyntax_HighlightDefineMarker)

                                                    For Each struc In lAutocompleteList
                                                        Select Case (True)
                                                            Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE,
                                                                        (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR
                                                                SB.Append(String.Format("<Key word=""{0}""/>", struc.sFunctionName))

                                                        End Select
                                                    Next

                                                    SB.AppendLine()
                                                    Continue While
                                                End If

                                                If (sLine.Contains(sSyntax_HighlightEnumMarker)) Then
                                                    SB.Append(sSyntax_HighlightEnumMarker)

                                                    Dim lExistList As New List(Of String)

                                                    For Each struc In lAutocompleteList
                                                        Select Case (True)
                                                            Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM
                                                                Dim sEnumName As String() = struc.sFunctionName.Split("."c)
                                                                If (sEnumName.Length = 2) Then
                                                                    If (Not lExistList.Contains(sEnumName(0))) Then
                                                                        lExistList.Add(sEnumName(0))
                                                                    End If

                                                                    SB.Append(String.Format("<Key word=""{0}""/>", sEnumName(1)))
                                                                End If

                                                            Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP
                                                                If (Not lExistList.Contains(struc.sFunctionName)) Then
                                                                    lExistList.Add(struc.sFunctionName)
                                                                End If
                                                        End Select
                                                    Next

                                                    SB.AppendLine()
                                                    Continue While
                                                End If

                                                If (sLine.Contains(sSyntax_HighlightEnum2Marker)) Then
                                                    SB.Append(sSyntax_HighlightEnum2Marker)

                                                    Dim lExistList As New List(Of String)

                                                    For Each struc In lAutocompleteList
                                                        Select Case (True)
                                                            Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM
                                                                Dim sEnumName As String() = struc.sFunctionName.Split("."c)
                                                                If (sEnumName.Length = 2) Then
                                                                    If (Not lExistList.Contains(sEnumName(0))) Then
                                                                        lExistList.Add(sEnumName(0))
                                                                    End If
                                                                End If

                                                            Case (struc.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP
                                                                If (Not lExistList.Contains(struc.sFunctionName)) Then
                                                                    lExistList.Add(struc.sFunctionName)
                                                                End If
                                                        End Select
                                                    Next

                                                    For Each s In lExistList
                                                        SB.Append(String.Format("<Key word=""{0}""/>", s))
                                                    Next

                                                    SB.AppendLine()
                                                    Continue While
                                                End If
                                        End Select

                                        If (Not String.IsNullOrEmpty(sLine.Trim)) Then
                                            SB.AppendLine(sLine.Trim)
                                        End If
                                    End While
                                End Using

                                Dim sFormatedString As String = SB.ToString

                                'Invert colors of the syntax file. But only for the default syntax file.
                                While (ClassSettings.g_iSettingsInvertColors)
                                    If (Not String.IsNullOrEmpty(ClassSettings.g_sConfigSyntaxHighlightingPath) AndAlso
                                            IO.File.Exists(ClassSettings.g_sConfigSyntaxHighlightingPath) AndAlso
                                            IO.Path.GetExtension(ClassSettings.g_sConfigSyntaxHighlightingPath).ToLower = ".xml") Then
                                        Exit While
                                    End If

                                    Dim mMatchColl As MatchCollection = Regex.Matches(sFormatedString, "\b(color|bgcolor)\b\s*=\s*""(?<Color>[a-zA-Z]+)""")
                                    For j = mMatchColl.Count - 1 To 0 Step -1
                                        Dim mMatch As Match = mMatchColl(j)
                                        If (Not mMatch.Success) Then
                                            Continue For
                                        End If

                                        Try
                                            Dim sColorName As String = mMatch.Groups("Color").Value
                                            Dim iColorNameIndex As Integer = mMatch.Groups("Color").Index
                                            Dim cConv As Color = ColorTranslator.FromHtml(sColorName)

                                            Dim cInvColor As Color = InvertColor(cConv)

                                            If (cInvColor.R = 0 AndAlso cInvColor.G = 0 AndAlso cInvColor.B = 0) Then
                                                cInvColor = g_mFormMain.g_cDarkTextEditorBackgroundColor
                                            End If

                                            Dim sInvColor As String = ColorTranslator.ToHtml(cInvColor)

                                            sFormatedString = sFormatedString.Remove(iColorNameIndex, sColorName.Length)
                                            sFormatedString = sFormatedString.Insert(iColorNameIndex, sInvColor)
                                        Catch : End Try
                                    Next

                                    Exit While
                                End While

                                IO.File.WriteAllText(g_SyntaxFiles(i).sFile, sFormatedString)
                        End Select
                    Next
                End SyncLock
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Updates the text editor syntax. Should be used after changing the syntax file.
        ''' </summary>
        Public Sub UpdateTextEditorSyntax()
            Try
                For i = 0 To g_SyntaxFiles.Length - 1
                    If (Not IO.File.Exists(g_SyntaxFiles(i).sFile)) Then
                        CreateSyntaxFile(CType(i, ENUM_SYNTAX_FILES))
                    End If
                Next

                HighlightingManager.Manager.ReloadSyntaxModes()

                For i = 0 To g_SyntaxFiles.Length - 1
                    Select Case (i)
                        Case ENUM_SYNTAX_FILES.MAIN_TEXTEDITOR
                            g_mFormMain.TextEditorControl_Source.SetHighlighting(g_SyntaxFiles(i).sDefinition)

                            g_mFormMain.g_mUCToolTip.TextEditorControl_ToolTip.SetHighlighting(g_SyntaxFiles(i).sDefinition)
                            g_mFormMain.g_mUCToolTip.TextEditorControl_ToolTip.Font = New Font(g_mFormMain.TextEditorControl_Source.Font.FontFamily, 8, FontStyle.Regular)

                        Case ENUM_SYNTAX_FILES.DEBUGGER_TEXTEDITOR
                            If (g_mFormMain.g_mFormDebugger IsNot Nothing AndAlso Not g_mFormMain.g_mFormDebugger.IsDisposed) Then
                                g_mFormMain.g_mFormDebugger.TextEditorControlEx_DebuggerSource.SetHighlighting(g_SyntaxFiles(i).sDefinition)
                                g_mFormMain.g_mFormDebugger.TextEditorControlEx_DebuggerDiasm.SetHighlighting(g_SyntaxFiles(i).sDefinition)
                            End If
                    End Select
                Next

            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Gets the expression between characters e.g if "()": MyFunc(MyArgs) => MyArgs
        ''' </summary>
        ''' <param name="sExpression"></param>
        ''' <param name="sCharOpen"></param>
        ''' <param name="sCharClose"></param>
        ''' <param name="iTargetEndScopeLevel"></param>
        ''' <param name="bInvalidCodeCheck">If true, it will ignore all Non-Code stuff like strings, comments etc.</param>
        ''' <returns></returns>
        Public Function GetExpressionBetweenCharacters(sExpression As String, sCharOpen As Char, sCharClose As Char, iTargetEndScopeLevel As Integer, Optional bInvalidCodeCheck As Boolean = False) As Integer()()
            Dim iCurrentLevel As Integer = 0
            Dim sExpressionsList As New List(Of Integer())
            Dim bWasOpen As Boolean = False

            Dim iStartLoc As Integer = 0

            Dim sourceAnalysis As ClassSyntaxSourceAnalysis = Nothing
            If (bInvalidCodeCheck) Then
                sourceAnalysis = New ClassSyntaxSourceAnalysis(sExpression)
            End If

            For i = 0 To sExpression.Length - 1
                If (sourceAnalysis IsNot Nothing AndAlso bInvalidCodeCheck) Then
                    If (sourceAnalysis.InNonCode(i)) Then
                        Continue For
                    End If
                End If

                If (sExpression(i) = sCharOpen) Then
                    iCurrentLevel += 1
                End If

                If (Not bWasOpen AndAlso iCurrentLevel >= iTargetEndScopeLevel) Then
                    iStartLoc = i
                    bWasOpen = True
                End If

                If (sExpression(i) = sCharClose) Then
                    iCurrentLevel -= 1
                    If (iCurrentLevel <= 0) Then
                        iCurrentLevel = 0
                    End If
                End If

                If (bWasOpen AndAlso iCurrentLevel < iTargetEndScopeLevel) Then
                    sExpressionsList.Add({iStartLoc, i})
                    bWasOpen = False
                End If
            Next

            Return sExpressionsList.ToArray
        End Function

        ''' <summary>
        ''' Automatic formats tabs in the code.
        ''' </summary>
        ''' <param name="sSource"></param>
        ''' <returns></returns>
        Public Function FormatCode(sSource As String) As String
            Dim SB As New StringBuilder
            Using SR As New IO.StringReader(sSource)
                While True
                    Dim sLine As String = SR.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    SB.AppendLine(sLine.Trim)
                End While
            End Using
            sSource = SB.ToString

            Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

            Dim iBraceCount As Integer = 0
            Dim iBracedCount As Integer = 0

            For i = sSource.Length - 1 - 1 To 0 Step -1
                Try
                    Select Case (sSource(i))
                        Case "("c
                            If (Not sourceAnalysis.InNonCode(i)) Then
                                iBracedCount -= 1
                            End If

                        Case ")"c
                            If (Not sourceAnalysis.InNonCode(i)) Then
                                iBracedCount += 1
                            End If

                        Case "{"c
                            If (Not sourceAnalysis.InNonCode(i)) Then
                                iBraceCount -= 1
                            End If

                        Case "}"c
                            If (Not sourceAnalysis.InNonCode(i)) Then
                                iBraceCount += 1
                            End If

                        Case vbLf(0)
                            'If (Not sourceAnalysis.InNonCode(i)) Then
                            sSource = sSource.Insert(i + 1, New String(vbTab(0), sourceAnalysis.GetBraceLevel(i + 1 + iBraceCount) + If(iBracedCount > 0, iBracedCount + 1, 0)))
                            'End If
                            iBraceCount = 0

                    End Select
                Catch ex As Exception
                    ' Ignore random errors
                End Try
            Next

            Return sSource
        End Function

        ''' <summary>
        ''' Checks if the source requires new decls and returns the offset.
        ''' NOTE: High false positive rate.
        ''' Returns -1 if not found.
        ''' </summary>
        ''' <param name="sSource"></param>
        ''' <param name="bIgnoreChecks"></param>
        ''' <returns>The offset in the source, -1 if not found.</returns>
        Public Function HasNewDeclsPragma(sSource As String, Optional bIgnoreChecks As Boolean = True) As Integer
            'TODO: Add better check
            Dim sRegexPattern As String = "\#\b(pragma)\b(\s+|\s*(\\*)\s*)\b(newdecls)\b(\s+|\s*(\\*)\s*)\b(required)\b"

            If (bIgnoreChecks) Then
                For Each match As Match In Regex.Matches(sSource, sRegexPattern, RegexOptions.Multiline)
                    Return match.Index
                Next
            Else
                Dim sourceAnalysis As New ClassSyntaxSourceAnalysis(sSource)
                For Each match As Match In Regex.Matches(sSource, sRegexPattern, RegexOptions.Multiline)
                    If (sourceAnalysis.InNonCode(match.Index)) Then
                        Continue For
                    End If

                    Return match.Index
                Next
            End If

            Return -1
        End Function

        ''' <summary>
        ''' Checks if the name is a forbidden name.
        ''' </summary>
        ''' <param name="sName"></param>
        ''' <returns></returns>
        Public Function IsForbiddenVariableName(sName As String) As Boolean
            Static sRegexBadName As String = Nothing
            If (sRegexBadName = Nothing) Then
                Dim lBadList As New List(Of String)
                lBadList.Add("const")
                lBadList.Add("new")
                lBadList.Add("decl")
                lBadList.Add("static")
                lBadList.Add("stock")
                lBadList.Add("public")
                lBadList.Add("private")
                lBadList.Add("if")
                lBadList.Add("for")
                lBadList.Add("else")
                lBadList.Add("case")
                lBadList.Add("switch")
                lBadList.Add("while")
                lBadList.Add("do")
                lBadList.Add("enum")
                sRegexBadName = String.Format("^({0})$", String.Join("|", lBadList.ToArray))
            End If

            Return Regex.IsMatch(sName, sRegexBadName)
        End Function

        Public Class ClassSyntaxSourceAnalysis
            Enum ENUM_STATE_TYPES
                PARENTHESIS_LEVEL
                BRACKET_LEVEL
                BRACE_LEVEL
                IN_SINGLE_COMMENT
                IN_MULTI_COMMENT
                IN_STRING
                IN_CHAR
                IN_PREPROCESSOR
            End Enum

            Private iStateArray As Integer(,)
            Private iMaxLenght As Integer = 0
            Private sCacheText As String = ""

            ''' <summary>
            ''' Gets the max lenght
            ''' </summary>
            ''' <returns></returns>
            Public ReadOnly Property GetMaxLenght() As Integer
                Get
                    Return iMaxLenght
                End Get
            End Property

            ''' <summary>
            ''' If char index is in single-comment
            ''' </summary>
            ''' <param name="i">The char index</param>
            ''' <returns></returns>
            Public ReadOnly Property InSingleComment(i As Integer) As Boolean
                Get
                    Return iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) > 0
                End Get
            End Property

            ''' <summary>
            ''' If char index is in multi-comment
            ''' </summary>
            ''' <param name="i">The char index</param>
            ''' <returns></returns>
            Public ReadOnly Property InMultiComment(i As Integer) As Boolean
                Get
                    Return iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) > 0
                End Get
            End Property

            ''' <summary>
            ''' If char index is in string
            ''' </summary>
            ''' <param name="i">The char index</param>
            ''' <returns></returns>
            Public ReadOnly Property InString(i As Integer) As Boolean
                Get
                    Return iStateArray(i, ENUM_STATE_TYPES.IN_STRING) > 0
                End Get
            End Property

            ''' <summary>
            ''' If char index is in char
            ''' </summary>
            ''' <param name="i">The char index</param>
            ''' <returns></returns>
            Public ReadOnly Property InChar(i As Integer) As Boolean
                Get
                    Return iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) > 0
                End Get
            End Property

            ''' <summary>
            ''' If char index is in Preprocessor (#define, #pragma etc.)
            ''' </summary>
            ''' <param name="i">The char index</param>
            ''' <returns></returns>
            Public ReadOnly Property InPreprocessor(i As Integer) As Boolean
                Get
                    Return iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) > 0
                End Get
            End Property

            ''' <summary>
            ''' Get current parenthesis "(" or ")" level
            ''' </summary>
            ''' <param name="i">The char index</param>
            ''' <returns></returns>
            Public ReadOnly Property GetParenthesisLevel(i As Integer) As Integer
                Get
                    Return iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL)
                End Get
            End Property

            ''' <summary>
            ''' Get current parenthesis "[" or "]" level
            ''' </summary>
            ''' <param name="i">The char index</param>
            ''' <returns></returns>
            Public ReadOnly Property GetBracketLevel(i As Integer) As Integer
                Get
                    Return iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL)
                End Get
            End Property

            ''' <summary>
            ''' Get current parenthesis "{" or "}" level
            ''' </summary>
            ''' <param name="i">The char index</param>
            ''' <returns></returns>
            Public ReadOnly Property GetBraceLevel(i As Integer) As Integer
                Get
                    Return iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL)
                End Get
            End Property

            ''' <summary>
            ''' It will return true if the char index is in a string, char, single- or multi-comment.
            ''' </summary>
            ''' <param name="i">The char index</param>
            ''' <returns></returns>
            Public ReadOnly Property InNonCode(i As Integer) As Boolean
                Get
                    Return (iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) > 0 OrElse
                                iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) > 0 OrElse
                                iStateArray(i, ENUM_STATE_TYPES.IN_STRING) > 0 OrElse
                                iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) > 0)
                End Get
            End Property

            Public Function GetIndexFromLine(iLine As Integer) As Integer
                If (iLine = 0) Then
                    Return 0
                End If

                Dim iLineCount As Integer = 0
                For i = 0 To sCacheText.Length - 1
                    Select Case (sCacheText(i))
                        Case vbLf(0)
                            iLineCount += 1

                            If (iLineCount >= iLine) Then
                                Return If(i + 1 > sCacheText.Length - 1, -1, i + 1)
                            End If
                    End Select
                Next

                Return -1
            End Function


            Public Sub New(ByRef sText As String, Optional bIgnorePreprocessor As Boolean = True)
                sCacheText = sText
                iMaxLenght = sText.Length

                iStateArray = New Integer(sText.Length - 1, [Enum].GetNames(GetType(ENUM_STATE_TYPES)).Length - 1) {}

                Dim iParenthesisLevel As Integer = 0 '()
                Dim iBracketLevel As Integer = 0 '[]
                Dim iBraceLevel As Integer = 0 '{}
                Dim bInSingleComment As Integer = 0
                Dim bInMultiComment As Integer = 0
                Dim bInString As Integer = 0
                Dim bInChar As Integer = 0
                Dim bInPreprocessor As Integer = 0

                For i = 0 To sText.Length - 1
                    iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) = iParenthesisLevel '0
                    iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel '1
                    iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel '2
                    iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment '3
                    iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = bInMultiComment '4
                    iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = bInString '5
                    iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) = bInChar '6
                    iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = bInPreprocessor '7

                    Select Case (sText(i))
                        Case "#"c
                            If (iParenthesisLevel > 0 OrElse iBracketLevel > 0 OrElse iBraceLevel > 0 OrElse bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                                Continue For
                            End If

                            '/*
                            bInPreprocessor = 1
                            iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = 1
                        Case "*"c
                            If (bInSingleComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                                Continue For
                            End If

                            '/*
                            If (i > 0) Then
                                If (sText(i - 1) = "/"c AndAlso bInMultiComment < 1) Then
                                    bInMultiComment = 1
                                    iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = 1
                                    iStateArray(i - 1, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = 1
                                    Continue For
                                End If
                            End If

                            If (i + 1 < sText.Length - 1) Then
                                If (sText(i + 1) = "/"c AndAlso bInMultiComment > 0) Then
                                    bInMultiComment = 0
                                    iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = 1
                                    iStateArray(i + 1, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = 1

                                    i += 1
                                    Continue For
                                End If
                            End If
                        Case "/"c
                            If (bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                                Continue For
                            End If

                            '//
                            If (i + 1 < sText.Length - 1) Then
                                If (sText(i + 1) = "/"c AndAlso bInSingleComment < 1) Then
                                    bInSingleComment = 1
                                    iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                                End If
                            End If
                        Case "("c
                            If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                                Continue For
                            End If

                            If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                                Continue For
                            End If

                            iParenthesisLevel += 1
                            iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) = iParenthesisLevel
                        Case ")"c
                            If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                                Continue For
                            End If

                            If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                                Continue For
                            End If

                            iParenthesisLevel -= 1
                            iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) = iParenthesisLevel
                        Case "["c
                            If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                                Continue For
                            End If

                            If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                                Continue For
                            End If

                            iBracketLevel += 1
                            iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel
                        Case "]"c
                            If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                                Continue For
                            End If

                            If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                                Continue For
                            End If

                            iBracketLevel -= 1
                            iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel
                        Case "{"c
                            If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                                Continue For
                            End If

                            If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                                Continue For
                            End If

                            iBraceLevel += 1
                            iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel
                        Case "}"c
                            If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                                Continue For
                            End If

                            If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0 OrElse bInChar > 0) Then
                                Continue For
                            End If

                            iBraceLevel -= 1
                            iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel
                        Case "'"c
                            If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                                Continue For
                            End If

                            If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInString > 0) Then
                                Continue For
                            End If

                            'ignore \'
                            If (i > 1 AndAlso sText(i - 1) <> "\"c) Then
                                bInChar = If(bInChar > 0, 0, 1)
                                iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) = 1
                            ElseIf (i > 2 AndAlso sText(i - 1) = "\"c AndAlso sText(i - 2) = "\"c) Then
                                bInChar = If(bInChar > 0, 0, 1)
                                iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) = 1
                            End If
                        Case """"c
                            If (Not bIgnorePreprocessor AndAlso bInPreprocessor > 0) Then
                                Continue For
                            End If

                            If (bInSingleComment > 0 OrElse bInMultiComment > 0 OrElse bInChar > 0) Then
                                Continue For
                            End If

                            'ignore \"
                            If (i > 1 AndAlso sText(i - 1) <> "\"c) Then
                                bInString = If(bInString > 0, 0, 1)
                                iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = 1
                            ElseIf (i > 2 AndAlso sText(i - 1) = "\"c AndAlso sText(i - 2) = "\"c) Then
                                bInString = If(bInString > 0, 0, 1)
                                iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = 1
                            End If
                        Case vbLf(0)
                            If (bInSingleComment > 0) Then
                                bInSingleComment = 0
                                iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                            End If

                            If (bInPreprocessor > 0) Then
                                bInPreprocessor = 0
                                iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = bInPreprocessor
                            End If
                        Case Else
                            iStateArray(i, ENUM_STATE_TYPES.PARENTHESIS_LEVEL) = iParenthesisLevel
                            iStateArray(i, ENUM_STATE_TYPES.BRACKET_LEVEL) = iBracketLevel
                            iStateArray(i, ENUM_STATE_TYPES.BRACE_LEVEL) = iBraceLevel
                            iStateArray(i, ENUM_STATE_TYPES.IN_SINGLE_COMMENT) = bInSingleComment
                            iStateArray(i, ENUM_STATE_TYPES.IN_MULTI_COMMENT) = bInMultiComment
                            iStateArray(i, ENUM_STATE_TYPES.IN_STRING) = bInString
                            iStateArray(i, ENUM_STATE_TYPES.IN_CHAR) = bInChar
                            iStateArray(i, ENUM_STATE_TYPES.IN_PREPROCESSOR) = bInPreprocessor
                    End Select
                Next
            End Sub
        End Class

    End Class

    Public Class ClassAutocompleteUpdater
        Private g_mFormMain As FormMain
        Private g_mAutocompleteUpdaterThread As Threading.Thread

        Public g_bForceFullAutocompleteUpdate As Boolean = False
        Public _lock As New Object

        Public Sub New(f As FormMain)
            g_mFormMain = f
        End Sub

        Enum ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS
            ALL = 0
            FULL_AUTOCOMPLETE = (1 << 0)
            VARIABLES_AUTOCOMPLETE = (1 << 1)
        End Enum

        ''' <summary>
        ''' Starts the autocomplete update thread
        ''' </summary>
        Public Function StartUpdate(iUpdateType As ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS) As Boolean
            If (g_mAutocompleteUpdaterThread Is Nothing OrElse Not g_mAutocompleteUpdaterThread.IsAlive) Then
                g_mAutocompleteUpdaterThread = New Threading.Thread(Sub()
                                                                        SyncLock _lock
                                                                            If (iUpdateType = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL OrElse
                                                                                        (iUpdateType And ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.FULL_AUTOCOMPLETE) = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.FULL_AUTOCOMPLETE) Then
                                                                                FullAutocompleteUpdate_Thread()
                                                                            End If

                                                                            If (iUpdateType = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL OrElse
                                                                                        (iUpdateType And ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.VARIABLES_AUTOCOMPLETE) = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.VARIABLES_AUTOCOMPLETE) Then
                                                                                VariableAutocompleteUpdate_Thread()
                                                                            End If
                                                                        End SyncLock
                                                                    End Sub)
                g_mAutocompleteUpdaterThread.IsBackground = True
                g_mAutocompleteUpdaterThread.Start()

                If (iUpdateType = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL OrElse
                            (iUpdateType And ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.FULL_AUTOCOMPLETE) = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.FULL_AUTOCOMPLETE) Then
                    g_bForceFullAutocompleteUpdate = False
                End If

                Return True
            Else
                If (Not g_bForceFullAutocompleteUpdate) Then
                    g_mFormMain.PrintInformation("[INFO]", "Could not start autocomplete update thread, it's already running!")
                End If

                If (iUpdateType = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL OrElse
                            (iUpdateType And ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.FULL_AUTOCOMPLETE) = ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.FULL_AUTOCOMPLETE) Then
                    g_bForceFullAutocompleteUpdate = True
                End If

                Return False
            End If
        End Function

        ''' <summary>
        ''' Stops the autocomplete update thread
        ''' </summary>
        Public Sub StopUpdate()
            If (g_mAutocompleteUpdaterThread IsNot Nothing AndAlso g_mAutocompleteUpdaterThread.IsAlive) Then
                'g_mFormMain.PrintInformation("[WARN]", "Autocomplete update canceled!")

                g_mAutocompleteUpdaterThread.Abort()
                g_mAutocompleteUpdaterThread.Join()
                g_mAutocompleteUpdaterThread = Nothing
            End If
        End Sub

        Private Sub FullAutocompleteUpdate_Thread()
            Try
                'g_mFormMain.PrintInformation("[INFO]", "Autocomplete update started...")

                If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                    g_mFormMain.PrintInformation("[ERRO]", "Autocomplete update failed! Could not get current source file!")
                    Return
                End If

                Dim lTmpAutocompleteList As New ClassSyncList(Of STRUC_AUTOCOMPLETE)

                'Add debugger placeholder variables and methods
                lTmpAutocompleteList.AddRange((New ClassDebuggerParser(g_mFormMain)).GetDebuggerAutocomplete)


                'Parse everything. Methods etc.
                If (True) Then
                    Dim sSourceList As New ClassSyncList(Of String())
                    Dim sFiles As String() = GetIncludeFiles(ClassSettings.g_sConfigOpenSourceFile)

                    For i = 0 To sFiles.Length - 1
                        ParseAutocomplete_Pre(sFiles(i), sSourceList, lTmpAutocompleteList)
                    Next

                    Dim sRegExEnum As String = String.Format("(\b{0}\b)", String.Join("\b|\b", GetEnumNames(lTmpAutocompleteList)))
                    For i = 0 To sSourceList.Count - 1
                        ParseAutocomplete_Post(sSourceList(i)(0), sRegExEnum, sSourceList(i)(1), lTmpAutocompleteList)
                    Next
                End If

                'Parse Methodmaps
                If (True) Then
                    ParseAutocompleteMethodmap(lTmpAutocompleteList)
                End If

                'Save everything and update syntax
                g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.RemoveAll(Function(j As STRUC_AUTOCOMPLETE) (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.AddRange(lTmpAutocompleteList)

                g_mFormMain.g_ClassSyntaxTools.UpdateSyntaxFile(ClassSyntaxTools.ENUM_SYNTAX_UPDATE_TYPE.AUTOCOMPLETE)
                g_mFormMain.BeginInvoke(Sub() g_mFormMain.g_ClassSyntaxTools.UpdateTextEditorSyntax())
                g_mFormMain.BeginInvoke(Sub() g_mFormMain.g_mUCObjectBrowser.UpdateTreeView())

                'g_mFormMain.PrintInformation("[INFO]", "Autocomplete update finished!")
            Catch ex As Threading.ThreadAbortException
                Throw
            Catch ex As Exception
                g_mFormMain.PrintInformation("[ERRO]", "Autocomplete update failed! " & ex.Message)
                ClassExceptionLog.WriteToLog(ex)
            End Try
        End Sub

        Private Sub ParseAutocompleteMethodmap(lTmpAutoList As ClassSyncList(Of STRUC_AUTOCOMPLETE))
            Dim lAlreadyDidList As New List(Of String)

            Dim lTmpAutoAddList As New List(Of STRUC_AUTOCOMPLETE)

            For i = lTmpAutoList.Count - 1 To 0 Step -1
                If ((lTmpAutoList(i).mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP OrElse Not lTmpAutoList(i).sFullFunctionName.Contains("<"c) OrElse Not lTmpAutoList(i).sFunctionName.Contains("."c)) Then
                    Continue For
                End If

                Dim mMatch As Match = Regex.Match(lTmpAutoList(i).sFullFunctionName, "(?<Name>\b[a-zA-Z0-9_]+\b)\s+\<\s+(?<Parent>\b[a-zA-Z0-9_]+\b)")
                If (Not mMatch.Success) Then
                    Continue For
                End If

                Dim sName As String = mMatch.Groups("Name").Value
                Dim sParent As String = mMatch.Groups("Parent").Value

                If (lAlreadyDidList.Contains(sName)) Then
                    Continue For
                End If

                lAlreadyDidList.Add(sName)

                Dim iOldNextParent As String = sParent

                While Not String.IsNullOrEmpty(iOldNextParent)
                    Dim sNextParent As String = ""

                    For ii = 0 To lTmpAutoList.Count - 1
                        If ((lTmpAutoList(ii).mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP OrElse Not lTmpAutoList(ii).sFunctionName.Contains("."c)) Then
                            Continue For
                        End If

                        Dim mParentMatch As Match = Regex.Match(lTmpAutoList(ii).sFullFunctionName, "(\b[a-zA-Z0-9_]+\b)\s+\<\s+(?<Parent>\b[a-zA-Z0-9_]+\b)")
                        Dim mParentMatch2 As Match = Regex.Match(lTmpAutoList(ii).sFunctionName, "(?<Name>^\b[a-zA-Z0-9_]+\b)\.")


                        Dim sParentName As String = mParentMatch2.Groups("Name").Value
                        Dim sParentParent As String = mParentMatch.Groups("Parent").Value

                        If (String.IsNullOrEmpty(sParentParent) OrElse iOldNextParent <> sParentName) Then
                            Continue For
                        End If

                        sNextParent = sParentParent

                        Dim struc As New STRUC_AUTOCOMPLETE
                        struc.sFile = lTmpAutoList(ii).sFile
                        struc.sFullFunctionName = lTmpAutoList(ii).sFullFunctionName
                        struc.sFunctionName = Regex.Replace(lTmpAutoList(ii).sFunctionName, "^\b[a-zA-Z0-9_]+\b\.", String.Format("{0}.", sName))
                        struc.sInfo = lTmpAutoList(ii).sInfo
                        struc.mType = lTmpAutoList(ii).mType

                        lTmpAutoAddList.Add(struc)
                    Next

                    iOldNextParent = sNextParent
                End While
            Next

            lTmpAutoList.AddRange(lTmpAutoAddList.ToArray)
        End Sub

        Private Function GetEnumNames(lTmpAutoList As ClassSyncList(Of STRUC_AUTOCOMPLETE)) As String()
            Dim lList As New List(Of String)

            'For >1.7
            lList.Add("void")
            lList.Add("int")
            lList.Add("bool")
            lList.Add("float")
            lList.Add("char")
            lList.Add("function")
            lList.Add("object")
            lList.Add("null_t")
            lList.Add("nullfunc_t")
            lList.Add("__nullable__")

            'For <1.6
            lList.Add("_")
            lList.Add("any")
            lList.Add("bool")
            lList.Add("Float")
            lList.Add("String")
            lList.Add("Function")

            'Special
            'lList.Add("EOS")
            'lList.Add("INVALID_FUNCTION")
            'lList.Add("cellbits")
            'lList.Add("cellmax")
            'lList.Add("cellmin")
            'lList.Add("charbits")
            'lList.Add("charmin")
            'lList.Add("charmax")
            'lList.Add("ucharmax")
            'lList.Add("__Pawn")
            'lList.Add("debug")

            lTmpAutoList.ForEach(Sub(j As STRUC_AUTOCOMPLETE)
                                     If ((j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) Then
                                         Return
                                     End If

                                     If (j.sFunctionName.Contains("."c)) Then
                                         Return
                                     End If

                                     If (lList.Contains(j.sFunctionName)) Then
                                         Return
                                     End If

                                     lList.Add(j.sFunctionName)
                                 End Sub)

            Return lList.ToArray
        End Function

        Public Sub CleanUpNewLinesSource(ByRef sSource As String)
            'Remove new lines e.g: MyStuff \
            '                      MyStuff2
            sSource = Regex.Replace(sSource, "\\\s*\n\s*", "")

            If (True) Then
                'From:
                '   public
                '       static
                '           bla()
                'To:
                '   public static bla()
                Dim sTypes As String() = {"enum", "funcenum", "functag", "stock", "static", "const", "public", "native", "forward", "typeset", "methodmap", "typedef"}
                Dim mRegMatchCol As MatchCollection = Regex.Matches(sSource, String.Format("^\s*(\b{0}\b)(?<Space>\s*\n\s*)", String.Join("\b|\b", sTypes)), RegexOptions.Multiline)
                For i = mRegMatchCol.Count - 1 To 0 Step -1
                    Dim mRegMatch As Match = mRegMatchCol(i)
                    If (Not mRegMatch.Groups("Space").Success) Then
                        Continue For
                    End If
                    sSource = sSource.Remove(mRegMatch.Groups("Space").Index, mRegMatch.Groups("Space").Length)
                    sSource = sSource.Insert(mRegMatch.Groups("Space").Index, " "c)
                Next
            End If

            If (True) Then
                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                'Filter new lines in statements with parenthesis e.g: MyStuff(MyArg1,
                '                                                               MyArg2)
                For i = 0 To sSource.Length - 1
                    Select Case (sSource(i))
                        Case vbLf(0)
                            If (sourceAnalysis.InNonCode(i)) Then
                                Exit Select
                            End If

                            If (sourceAnalysis.GetParenthesisLevel(i) > 0) Then
                                Select Case (True)
                                    Case i > 1 AndAlso sSource(i - 1) = vbCr
                                        sSource = sSource.Remove(i - 1, 2)
                                        sSource = sSource.Insert(i - 1, New String(" "c, 2))
                                    Case Else
                                        sSource = sSource.Remove(i, 1)
                                        sSource = sSource.Insert(i, New String(" "c, 1))
                                End Select
                            End If
                    End Select
                Next
            End If
        End Sub

        Private Sub ParseAutocomplete_Pre(sFile As String, ByRef sSourceList As ClassSyncList(Of String()), ByRef lTmpAutocompleteList As ClassSyncList(Of STRUC_AUTOCOMPLETE))
            Dim sSource As String
            If (ClassSettings.g_sConfigOpenSourceFile.ToLower = sFile.ToLower) Then
                sSource = CStr(g_mFormMain.Invoke(Function() g_mFormMain.TextEditorControl_Source.Document.TextContent))
            Else
                sSource = IO.File.ReadAllText(sFile)
            End If

            CleanUpNewLinesSource(sSource)

            Dim lStringLocList As New List(Of Integer())
            Dim lBraceLocList As New List(Of Integer())

            If (True) Then
                'Filter new spaces 
                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                Dim iBraceLevel As Integer = 0
                Dim iStringStart As Integer = 0
                Dim iBraceStart As Integer = 0
                Dim bInString As Boolean = False
                For i = 0 To sSource.Length - 1
                    Select Case (sSource(i))
                        Case "("c
                            If (sourceAnalysis.InNonCode(i)) Then
                                Continue For
                            End If

                            If (iBraceLevel = 0) Then
                                iBraceStart = i
                            End If
                            iBraceLevel += 1
                        Case ")"c
                            If (sourceAnalysis.InNonCode(i)) Then
                                Continue For
                            End If

                            iBraceLevel -= 1
                            If (iBraceLevel = 0) Then
                                Dim iBraceLen = i - iBraceStart + 1
                                lBraceLocList.Add(New Integer() {iBraceStart, iBraceLen})
                            End If
                        Case """"c
                            If (i > 1 AndAlso sSource(i - 1) <> "\"c) Then
                                If (Not bInString AndAlso sSource(i - 1) = "'"c) Then
                                    Continue For
                                End If

                                Select Case (bInString)
                                    Case True
                                        Dim iStringLen = i - iStringStart + 1
                                        lStringLocList.Add(New Integer() {iStringStart, iStringLen})
                                    Case Else
                                        iStringStart = i
                                End Select

                                bInString = Not bInString
                            End If
                    End Select
                Next
            End If

            'Fix function spaces
            If (True) Then
                For i = lBraceLocList.Count - 1 To 0 Step -1
                    Dim iBraceStart As Integer = lBraceLocList(i)(0)
                    Dim iBraceLen As Integer = lBraceLocList(i)(1)

                    Dim sBraceString As String = sSource.Substring(iBraceStart, iBraceLen)
                    Dim mRegMatchCol As MatchCollection = Regex.Matches(sBraceString, "\s+")

                    For ii = mRegMatchCol.Count - 1 To 0 Step -1
                        Dim mRegMatch As Match = mRegMatchCol(ii)
                        If (Not mRegMatch.Success) Then
                            Continue For
                        End If

                        Dim iOffset As Integer = iBraceStart + mRegMatch.Index
                        Dim bContinue As Boolean = False
                        For iii = 0 To lStringLocList.Count - 1
                            If (iOffset > lStringLocList(iii)(0) AndAlso iOffset < (lStringLocList(iii)(0) + lStringLocList(iii)(1))) Then
                                bContinue = True
                                Exit For
                            End If
                        Next

                        If (bContinue) Then
                            Continue For
                        End If

                        sBraceString = sBraceString.Remove(mRegMatch.Index, mRegMatch.Length)
                        sBraceString = sBraceString.Insert(mRegMatch.Index, " "c)
                    Next

                    sSource = sSource.Remove(iBraceStart, iBraceLen)
                    sSource = sSource.Insert(iBraceStart, sBraceString)
                Next
            End If


            'Get methodmap enums
            If ((ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("methodmap")) Then
                Dim SB_Source As New StringBuilder(sSource.Length)

                If (True) Then
                    Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                    For i = 0 To sSource.Length - 1
                        SB_Source.Append(If(sourceAnalysis.InNonCode(i), " "c, sSource(i)))
                    Next
                End If

                Dim mPossibleEnumMatches As MatchCollection = Regex.Matches(SB_Source.ToString, "^\s*\b(methodmap)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)", RegexOptions.Multiline) '^\s*\b(enum)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)

                Dim mRegMatch As Match

                For i = 0 To mPossibleEnumMatches.Count - 1
                    mRegMatch = mPossibleEnumMatches(i)

                    If (Not mRegMatch.Success) Then
                        Continue For
                    End If

                    Dim sEnumName As String = mRegMatch.Groups("Name").Value

                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = IO.Path.GetFileName(sFile)
                    struc.sFullFunctionName = "enum " & sEnumName
                    struc.sFunctionName = sEnumName
                    struc.sInfo = ""
                    struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM

                    If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                        lTmpAutocompleteList.Add(struc)
                    End If
                Next
            End If


            'Get typeset enums
            If ((ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("typeset")) Then
                Dim SB_Source As New StringBuilder(sSource.Length)

                If (True) Then
                    Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                    For i = 0 To sSource.Length - 1
                        SB_Source.Append(If(sourceAnalysis.InNonCode(i), " "c, sSource(i)))
                    Next
                End If

                Dim mPossibleEnumMatches As MatchCollection = Regex.Matches(SB_Source.ToString, "^\s*\b(typeset)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)", RegexOptions.Multiline) '^\s*\b(enum)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)

                Dim mRegMatch As Match

                For i = 0 To mPossibleEnumMatches.Count - 1
                    mRegMatch = mPossibleEnumMatches(i)

                    If (Not mRegMatch.Success) Then
                        Continue For
                    End If

                    Dim sEnumName As String = mRegMatch.Groups("Name").Value

                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = IO.Path.GetFileName(sFile)
                    struc.sFullFunctionName = "enum " & sEnumName
                    struc.sFunctionName = sEnumName
                    struc.sInfo = ""
                    struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM

                    If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                        lTmpAutocompleteList.Add(struc)
                    End If
                Next
            End If

            'Get typedef enums
            'If ((SettingsClass.g_CurrentAutocompleteSyntax = SettingsClass.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7 OrElse SettingsClass.g_CurrentAutocompleteSyntax = SettingsClass.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("typedef")) Then
            '    Dim SB_Source As New StringBuilder(sSource.Length)

            '    If (True) Then
            '        Dim iSynCR_Source As New ClassSyntaxTools.ClassSyntaxCharReader(sSource)

            '        For i = 0 To sSource.Length - 1
            '            SB_Source.Append(If(iSynCR_Source.InNonCode(i), " "c, sSource(i)))
            '        Next
            '    End If

            '    Dim mPossibleEnumMatches As MatchCollection = Regex.Matches(SB_Source.ToString, "^\s*\b(typedef)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)", RegexOptions.Multiline) '^\s*\b(enum)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)

            '    Dim mRegMatch As Match

            '    For i = 0 To mPossibleEnumMatches.Count - 1
            '        mRegMatch = mPossibleEnumMatches(i)

            '        If (Not mRegMatch.Success) Then
            '            Continue For
            '        End If

            '        Dim sEnumName As String = mRegMatch.Groups("Name").Value

            '        Dim struc As STRUC_AUTOCOMPLETE
            '        struc.sFile = IO.Path.GetFileName(sFile)
            '        struc.sFullFunctionname = "enum " & sEnumName
            '        struc.sFunctionName = sEnumName
            '        struc.sInfo = ""
            '        struc.sType = "enum"

            '        If (Not lTmpAutoList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.sType = struc.sType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
            '            lTmpAutoList.Add(struc)
            '        End If
            '    Next
            'End If

            'Get enums
            If (sSource.Contains("enum")) Then
                Dim SB_Source As New StringBuilder(sSource.Length)

                If (True) Then
                    Dim sourceAnalysis2 As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                    For i = 0 To sSource.Length - 1
                        SB_Source.Append(If(sourceAnalysis2.InNonCode(i), " "c, sSource(i)))
                    Next
                End If

                Dim mPossibleEnumMatches As MatchCollection = Regex.Matches(SB_Source.ToString, "^\s*\b(enum)\b\s+((?<Name>\b[a-zA-Z0-9_]+\b)|\(.*?\)|)(\:){0,1}\s*(?<BraceStart>\{)", RegexOptions.Multiline) '^\s*\b(enum)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)
                Dim iBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(SB_Source.ToString, "{"c, "}"c, 1, True)


                Dim mRegMatch As Match

                Dim sEnumName As String

                Dim bIsValid As Boolean
                Dim sEnumSource As String
                Dim iBraceIndex As Integer

                Dim sourceAnalysis As ClassSyntaxTools.ClassSyntaxSourceAnalysis

                Dim SB As StringBuilder
                Dim lEnumSplitList As List(Of String)

                Dim sLine As String

                Dim iTargetEnumSplitListIndex As Integer

                Dim regMatch As Match

                For i = 0 To mPossibleEnumMatches.Count - 1
                    mRegMatch = mPossibleEnumMatches(i)
                    If (Not mRegMatch.Success) Then
                        Continue For
                    End If

                    sEnumName = mRegMatch.Groups("Name").Value
                    If (String.IsNullOrEmpty(sEnumName.Trim)) Then
                        g_mFormMain.PrintInformation("[ERRO]", String.Format("Failed to read name from enum because it has no name: Renamed to 'Enum' ({0})", IO.Path.GetFileName(sFile)))
                        sEnumName = "Enum"
                    End If

                    bIsValid = False
                    sEnumSource = ""
                    iBraceIndex = mRegMatch.Groups("BraceStart").Index
                    For ii = 0 To iBraceList.Length - 1
                        If (iBraceIndex = iBraceList(ii)(0)) Then
                            sEnumSource = SB_Source.ToString.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                            bIsValid = True
                            Exit For
                        End If
                    Next
                    If (Not bIsValid) Then
                        Continue For
                    End If

                    sourceAnalysis = New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sEnumSource)

                    SB = New StringBuilder
                    lEnumSplitList = New List(Of String)

                    For ii = 0 To sEnumSource.Length - 1
                        Select Case (sEnumSource(ii))
                            Case ","c
                                If (sourceAnalysis.GetParenthesisLevel(ii) > 0 OrElse sourceAnalysis.GetBracketLevel(ii) > 0 OrElse sourceAnalysis.GetBraceLevel(ii) > 0 OrElse sourceAnalysis.InNonCode(ii)) Then
                                    Exit Select
                                End If

                                If (SB.Length < 1) Then
                                    Exit Select
                                End If

                                sLine = SB.ToString
                                sLine = Regex.Replace(sLine.Trim, "\s+", " ")

                                lEnumSplitList.Add(sLine)
                                SB.Length = 0
                            Case Else
                                If (sourceAnalysis.InNonCode(ii)) Then
                                    Exit Select
                                End If

                                SB.Append(sEnumSource(ii))
                        End Select
                    Next

                    If (SB.Length > 0) Then
                        sLine = SB.ToString
                        sLine = Regex.Replace(sLine.Trim, "\s+", " ")

                        If (Not String.IsNullOrEmpty(sLine)) Then
                            lEnumSplitList.Add(sLine)
                            SB.Length = 0
                        End If
                    End If


                    Dim lEnumCommentArray As String() = New String(lEnumSplitList.Count - 1) {}

                    Dim sEnumSourceLines As String() = sEnumSource.Split(New String() {vbNewLine, vbLf}, 0)
                    For ii = 0 To sEnumSourceLines.Length - 1
                        iTargetEnumSplitListIndex = -1
                        For iii = 0 To lEnumSplitList.Count - 1
                            If (sEnumSourceLines(ii).Contains(lEnumSplitList(iii)) AndAlso Regex.IsMatch(sEnumSourceLines(ii), String.Format("\b{0}\b", Regex.Escape(lEnumSplitList(iii))))) Then
                                iTargetEnumSplitListIndex = iii
                                Exit For
                            End If
                        Next
                        If (iTargetEnumSplitListIndex < 0) Then
                            Continue For
                        End If

                        While True
                            regMatch = Regex.Match(sEnumSourceLines(ii), "/\*(.*?)\*/\s*$")
                            If (regMatch.Success) Then
                                lEnumCommentArray(iTargetEnumSplitListIndex) = regMatch.Value
                                Exit While
                            End If
                            If (ii > 1) Then
                                regMatch = Regex.Match(sEnumSourceLines(ii - 1), "^\s*(?<Comment>//(.*?)$)")
                                If (regMatch.Success) Then
                                    lEnumCommentArray(iTargetEnumSplitListIndex) = regMatch.Groups("Comment").Value
                                    Exit While
                                End If
                            End If

                            lEnumCommentArray(iTargetEnumSplitListIndex) = ""
                            Exit While
                        End While
                    Next

                    If (True) Then
                        Dim struc As New STRUC_AUTOCOMPLETE
                        struc.sFile = IO.Path.GetFileName(sFile)
                        struc.sFullFunctionName = "enum " & sEnumName
                        struc.sFunctionName = sEnumName
                        struc.sInfo = ""
                        struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM

                        If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                            lTmpAutocompleteList.Add(struc)
                        End If
                    End If

                    For ii = 0 To lEnumSplitList.Count - 1
                        Dim sEnumFull As String = lEnumSplitList(ii)
                        Dim sEnumComment As String = lEnumCommentArray(ii)

                        regMatch = Regex.Match(sEnumFull, "^\s*(?<Tag>\b[a-zA-Z0-9_]+\b:)*(?<Name>\b[a-zA-Z0-9_]+\b)")
                        If (Not regMatch.Groups("Name").Success) Then
                            g_mFormMain.PrintInformation("[ERRO]", String.Format("Failed to resolve type 'Enum': enum {0} {1}", sEnumName, sEnumFull))
                            Continue For
                        End If

                        Dim sEnumVarName As String = regMatch.Groups("Name").Value

                        Dim struc As New STRUC_AUTOCOMPLETE
                        struc.sFile = IO.Path.GetFileName(sFile)
                        struc.sFullFunctionName = String.Format("enum {0} {1}", sEnumName, sEnumFull)
                        struc.sFunctionName = String.Format("{0}.{1}", sEnumName, sEnumVarName)
                        struc.sInfo = sEnumComment
                        struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM

                        If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                            lTmpAutocompleteList.Add(struc)
                        End If
                    Next
                Next
            End If

            sSourceList.Add(New String() {sFile, sSource})
        End Sub

        Private Sub ParseAutocomplete_Post(ByRef sFile As String, ByRef sRegExEnum As String, ByRef sSource As String, ByRef lTmpAutocompleteList As ClassSyncList(Of STRUC_AUTOCOMPLETE))
            'Get Defines
            If (sSource.Contains("#define")) Then
                Dim sLine As String

                Dim mRegMatch As Match

                Dim sFullName As String
                Dim sName As String

                Dim iBraceList As Integer()()

                Dim sBraceText As String
                Dim sFullDefine As String

                Using SR As New IO.StringReader(sSource)
                    While True
                        sLine = SR.ReadLine
                        If (sLine Is Nothing) Then
                            Exit While
                        End If

                        If (Not sLine.Contains("#define")) Then
                            Continue While
                        End If

                        mRegMatch = Regex.Match(sLine, "^\s*#define\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<Arguments>\()*")
                        If (Not mRegMatch.Success) Then
                            Continue While
                        End If

                        sFullName = ""
                        sName = mRegMatch.Groups("Name").Value

                        If (mRegMatch.Groups("Arguments").Success) Then
                            iBraceList = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sLine, "("c, ")"c, 1, True)
                            If (iBraceList.Length < 1) Then
                                Continue While
                            End If

                            sBraceText = sLine.Substring(iBraceList(0)(0) + 1, iBraceList(0)(1) - iBraceList(0)(0))

                            sFullDefine = Regex.Match(sLine, String.Format("{0}{1}(.*?)$", Regex.Escape(mRegMatch.Value), Regex.Escape(sBraceText))).Value
                            sFullName = sFullDefine
                        Else
                            sFullDefine = Regex.Match(sLine, String.Format("{0}(.*?)$", Regex.Escape(mRegMatch.Value))).Value
                            sFullName = sFullDefine
                        End If

                        sFullName = sFullName.Replace(vbTab, " ")

                        Dim struc As New STRUC_AUTOCOMPLETE
                        struc.sFile = IO.Path.GetFileName(sFile)
                        struc.sFullFunctionName = sFullName
                        struc.sFunctionName = sName
                        struc.sInfo = ""
                        struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE

                        If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                            lTmpAutocompleteList.Add(struc)
                        End If
                    End While
                End Using
            End If

            'Get public global variables
            If (sSource.Contains("public")) Then
                'Dim SB_Source As New StringBuilder(sSource.Length)

                'If (True) Then
                '    Dim iSynCR_Source As New ClassSyntaxTools.ClassSyntaxCharReader(sSource)

                '    For i = 0 To sSource.Length - 1
                '        SB_Source.Append(If(iSynCR_Source.InNonCode(i), " "c, sSource(i)))
                '    Next
                'End If


                Dim mRegMatch As Match

                Dim sFullName As String
                Dim sName As String
                Dim sComment As String

                Dim sLines As String() = sSource.Split(New String() {vbNewLine, vbLf}, 0)
                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)
                For i = 0 To sLines.Length - 1
                    If (Not sLines(i).Contains("public") OrElse Not sLines(i).Contains(";"c)) Then
                        Continue For
                    End If

                    Dim iIndex = sourceAnalysis.GetIndexFromLine(i)
                    If (iIndex < 0 OrElse sourceAnalysis.GetBraceLevel(iIndex) > 0 OrElse sourceAnalysis.InNonCode(iIndex)) Then
                        Continue For
                    End If

                    'SP 1.7 +Tags
                    mRegMatch = Regex.Match(sLines(i), String.Format("^\s*(?<Types>public(\b[a-zA-Z0-9_ ]+\b)*)\s+(?<Tag>\b{0}\b(\[\s*\])*\s)\s*(?<Name>\b[a-zA-Z0-9_]+\b)(?<Other>(.*?))$", sRegExEnum))
                    If (Not mRegMatch.Success) Then
                        If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7) Then
                            Continue For
                        End If

                        'SP 1.6 +Tags
                        mRegMatch = Regex.Match(sLines(i), String.Format("^\s*(?<Types>public(\b[a-zA-Z0-9_ ]+\b)*)\s+(?<Tag>\b{0}\b\:)(?<Name>\b[a-zA-Z0-9_]+\b)(?<Other>(.*?))$", sRegExEnum))
                        If (Not mRegMatch.Success) Then
                            'SP 1.6 -Tags
                            mRegMatch = Regex.Match(sLines(i), String.Format("^\s*(?<Types>public(\b[a-zA-Z0-9_ ]+\b)*)\s+(?<!<Tag>\b{0}\b\:)(?<Name>\b[a-zA-Z0-9_]+\b)(?<Other>(.*?))$", sRegExEnum))
                            If (Not mRegMatch.Success) Then
                                Continue For
                            End If
                        End If
                    Else
                        If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6) Then
                            Continue For
                        End If
                    End If

                    sFullName = ""
                    sName = mRegMatch.Groups("Name").Value
                    sComment = Regex.Match(mRegMatch.Groups("Other").Value, "/\*(.*?)\*/\s*$").Value

                    sFullName = String.Format("{0} {1}{2}{3}", mRegMatch.Groups("Types").Value, mRegMatch.Groups("Tag").Value, mRegMatch.Groups("Name").Value, mRegMatch.Groups("Other").Value)
                    sFullName = sFullName.Replace(vbTab, " "c)
                    If (Not String.IsNullOrEmpty(sComment)) Then
                        sFullName = sFullName.Replace(sComment, "")
                    End If

                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = IO.Path.GetFileName(sFile)
                    struc.sFullFunctionName = sFullName
                    struc.sFunctionName = sName
                    struc.sInfo = sComment
                    struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR

                    If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                        lTmpAutocompleteList.Add(struc)
                    End If
                Next
            End If

            'Get Functions
            If (True) Then
                Dim iBraceList As Integer()()
                Dim sBraceText As String
                Dim mRegMatch As Match

                Dim sTypes As String()
                Dim sTag As String
                Dim sName As String
                Dim sFull As String
                Dim sComment As String

                Dim mFuncTagMatch As Match

                Dim bCommentStart As Boolean
                Dim mRegMatch2 As Match

                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)
                Dim sLines As String() = sSource.Split(New String() {vbNewLine, vbLf}, 0)

                If (sourceAnalysis.GetMaxLenght - 1 > 0) Then
                    Dim iLastBraceLevel As Integer = sourceAnalysis.GetBraceLevel(sourceAnalysis.GetMaxLenght - 1)
                    If (iLastBraceLevel > 0) Then
                        g_mFormMain.PrintInformation("[ERRO]", String.Format("Uneven brace level! May lead to syntax parser failures! [LV:{0}] ({1})", iLastBraceLevel, IO.Path.GetFileName(sFile)))
                    End If
                End If

                For i = 0 To sLines.Length - 1
                    If ((ClassTools.ClassStrings.WordCount(sLines(i), "("c) + ClassTools.ClassStrings.WordCount(sLines(i), ")"c)) Mod 2 <> 0) Then
                        Continue For
                    End If

                    Dim iIndex = sourceAnalysis.GetIndexFromLine(i)
                    If (iIndex < 0 OrElse sourceAnalysis.GetBraceLevel(iIndex) > 0 OrElse sourceAnalysis.InNonCode(iIndex)) Then
                        Continue For
                    End If

                    iBraceList = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sLines(i), "("c, ")"c, 1, True)
                    If (iBraceList.Length < 1) Then
                        Continue For
                    End If

                    sBraceText = sLines(i).Substring(iBraceList(0)(0), iBraceList(0)(1) - iBraceList(0)(0) + 1)

                    'SP 1.7 +Tags
                    mRegMatch = Regex.Match(sLines(i), String.Format("^\s*(?<Types>[a-zA-Z0-9_ ]*)(?<Tag>\b{0}\b\s)\s*(?<Name>\b[a-zA-Z0-9_]+\b)\s*{1}(?<IsFunc>\s*;){2}", sRegExEnum, Regex.Escape(sBraceText), "{0,1}"))
                    If (Not mRegMatch.Success) Then
                        If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7) Then
                            Continue For
                        End If

                        'SP 1.6 +Tags
                        mRegMatch = Regex.Match(sLines(i), String.Format("^\s*(?<Types>[a-zA-Z0-9_ ]*)(?<Tag>\b{0}\b\:)(?<Name>\b[a-zA-Z0-9_]+\b)\s*{1}(?<IsFunc>\s*;){2}", sRegExEnum, Regex.Escape(sBraceText), "{0,1}"))
                        If (Not mRegMatch.Success) Then
                            'SP 1.6 -Tags
                            mRegMatch = Regex.Match(sLines(i), String.Format("^\s*(?<Types>[a-zA-Z0-9_ ]*)(?<!<Tag>\b{0}\b\:)(?<Name>\b[a-zA-Z0-9_]+\b)\s*{1}(?<IsFunc>\s*;){2}", sRegExEnum, Regex.Escape(sBraceText), "{0,1}"))
                            If (Not mRegMatch.Success) Then
                                Continue For
                            End If
                        End If
                    Else
                        If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6) Then
                            Continue For
                        End If
                    End If

                    sTypes = mRegMatch.Groups("Types").Value.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
                    sTag = mRegMatch.Groups("Tag").Value
                    sName = mRegMatch.Groups("Name").Value
                    sFull = mRegMatch.Groups("Types").Value & sTag & sName & sBraceText
                    sComment = ""

                    If (Regex.IsMatch(sName, String.Format("(\b{0}\b)", String.Join("\b|\b", g_mFormMain.g_ClassSyntaxTools.g_sStatementsArray)))) Then
                        Continue For
                    End If

                    If (sTypes.Length < 1 AndAlso mRegMatch.Groups("IsFunc").Success) Then
                        Continue For
                    End If

                    If (Array.IndexOf(sTypes, "functag") > -1) Then
                        While True
                            mFuncTagMatch = Regex.Match(sFull, "(?<Name>\b[a-zA-Z0-9_]+\b)\s+(?<Tag>\b[a-zA-Z0-9_]+\:\b)*\b(public)\b\s*\(")
                            If (mFuncTagMatch.Success) Then
                                sTypes = New String() {"functag"}
                                sTag = mFuncTagMatch.Groups("Tag").Value
                                sName = mFuncTagMatch.Groups("Name").Value
                                sFull = "functag " & sTag & sName & sBraceText
                                Exit While
                            End If
                            mFuncTagMatch = Regex.Match(sFull, "\b(public)\b\s+(?<Tag>\b[a-zA-Z0-9_]+\:\b)*(?<Name>\b[a-zA-Z0-9_]+\b)\s*\(")
                            If (mFuncTagMatch.Success) Then
                                sTypes = New String() {"functag"}
                                sTag = mFuncTagMatch.Groups("Tag").Value
                                sName = mFuncTagMatch.Groups("Name").Value
                                sFull = "functag " & sTag & sName & sBraceText
                                Exit While
                            End If

                            Exit While
                        End While
                    End If

                    bCommentStart = False
                    For ii = i - 1 To 0 Step -1
                        mRegMatch2 = Regex.Match(sLines(ii), "(?<Start>(/\*+))|(?<End>\*+/|(?<Pragma>)^\s*#pragma)")

                        If (Not bCommentStart AndAlso Not mRegMatch2.Groups("End").Success) Then
                            Exit For
                        End If

                        If (Not mRegMatch2.Groups("Pragma").Success) Then
                            bCommentStart = True
                        End If

                        Select Case (True)
                            Case mRegMatch2.Groups("End").Success AndAlso Not mRegMatch2.Groups("Pragma").Success
                                sComment = sLines(ii).Substring(0, mRegMatch2.Groups("End").Index + 2) & Environment.NewLine & sComment
                            Case mRegMatch2.Groups("Start").Success
                                sComment = sLines(ii).Substring(mRegMatch2.Groups("Start").Index, sLines(ii).Length - mRegMatch2.Groups("Start").Index) & Environment.NewLine & sComment
                            Case Else
                                sComment = sLines(ii) & Environment.NewLine & sComment
                        End Select

                        If (bCommentStart AndAlso mRegMatch2.Groups("Start").Success) Then
                            Exit For
                        End If
                    Next

                    sComment = New Regex("^\s+", RegexOptions.Multiline).Replace(sComment, "")

                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = IO.Path.GetFileName(sFile)
                    struc.sFullFunctionName = sFull
                    struc.sFunctionName = sName
                    struc.sInfo = sComment
                    struc.mType = struc.ParseTypeNames(sTypes)

                    If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                        lTmpAutocompleteList.Add(struc)
                    End If
                Next
            End If

            'Get funcenums
            If ((ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6 OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("funcenum")) Then
                Dim mPossibleEnumMatches As MatchCollection = Regex.Matches(sSource, "^\s*\b(funcenum)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)", RegexOptions.Multiline)
                Dim iBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sSource, "{"c, "}"c, 1, True)

                Dim mRegMatch As Match

                Dim sEnumName As String

                Dim bIsValid As Boolean
                Dim sEnumSource As String
                Dim iBraceIndex As Integer

                Dim SB As StringBuilder
                Dim lEnumSplitList As List(Of String)

                Dim sourceAnalysis As ClassSyntaxTools.ClassSyntaxSourceAnalysis

                Dim sLine As String
                Dim iInvalidLen As Integer

                Dim sComment As String
                Dim lEnumCommentArray As String()
                Dim sEnumSourceLines As String()

                Dim iLine As Integer
                Dim bCommentStart As Boolean
                Dim mRegMatch2 As Match

                For i = 0 To mPossibleEnumMatches.Count - 1
                    mRegMatch = mPossibleEnumMatches(i)
                    If (Not mRegMatch.Success) Then
                        Continue For
                    End If

                    sEnumName = mRegMatch.Groups("Name").Value

                    bIsValid = False
                    sEnumSource = ""
                    iBraceIndex = mRegMatch.Groups("BraceStart").Index
                    For ii = 0 To iBraceList.Length - 1
                        If (iBraceIndex = iBraceList(ii)(0)) Then
                            sEnumSource = sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                            bIsValid = True
                            Exit For
                        End If
                    Next
                    If (Not bIsValid) Then
                        Continue For
                    End If

                    SB = New StringBuilder
                    lEnumSplitList = New List(Of String)

                    sourceAnalysis = New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sEnumSource)

                    For ii = 0 To sEnumSource.Length - 1
                        Select Case (sEnumSource(ii))
                            Case ","c
                                If (sourceAnalysis.GetParenthesisLevel(ii) > 0 OrElse sourceAnalysis.GetBracketLevel(ii) > 0 OrElse sourceAnalysis.GetBraceLevel(ii) > 0 OrElse sourceAnalysis.InNonCode(ii)) Then
                                    Exit Select
                                End If

                                If (SB.Length < 1) Then
                                    Exit Select
                                End If

                                sLine = SB.ToString
                                sLine = Regex.Replace(sLine.Trim, "\s+", " ")

                                iInvalidLen = Regex.Match(sLine, "^(.*?)(?<End>\b[a-zA-Z0-9_]+\b)").Groups("End").Index
                                If (iInvalidLen > 0) Then
                                    sLine = sLine.Remove(0, iInvalidLen)
                                End If

                                lEnumSplitList.Add(sLine)
                                SB.Length = 0
                        End Select

                        If (Not sourceAnalysis.InSingleComment(ii) AndAlso Not sourceAnalysis.InMultiComment(ii)) Then
                            SB.Append(sEnumSource(ii))
                        End If
                    Next

                    If (SB.Length > 0) Then
                        sLine = SB.ToString
                        sLine = Regex.Replace(sLine.Trim, "\s+", " ")

                        iInvalidLen = Regex.Match(sLine, "^(.*?)(?<End>\b[a-zA-Z0-9_]+\b)").Groups("End").Index
                        If (iInvalidLen > 0) Then
                            sLine = sLine.Remove(0, iInvalidLen)

                            If (Not String.IsNullOrEmpty(sLine)) Then
                                lEnumSplitList.Add(sLine)
                                SB.Length = 0
                            End If
                        End If
                    End If

                    sComment = ""
                    lEnumCommentArray = New String(lEnumSplitList.Count - 1) {}
                    sEnumSourceLines = sEnumSource.Split(New String() {vbNewLine, vbLf}, 0)

                    For ii = 0 To lEnumSplitList.Count - 1
                        iLine = 0
                        For iii = 0 To sEnumSourceLines.Length - 1
                            If (sEnumSourceLines(iii).Contains(lEnumSplitList(ii))) Then
                                iLine = iii
                                Exit For
                            End If
                        Next

                        bCommentStart = False
                        sComment = ""
                        For iii = iLine - 1 To 0 Step -1
                            mRegMatch2 = Regex.Match(sEnumSourceLines(iii), "^\s*(?<Start>//(.*?))$")
                            If (Not bCommentStart AndAlso mRegMatch2.Success) Then
                                sComment = mRegMatch2.Groups("Start").Value & Environment.NewLine & sComment
                                Continue For
                            End If

                            mRegMatch2 = Regex.Match(sEnumSourceLines(iii), "(?<Start>(/\*+))|(?<End>\*+/|(?<Pragma>)^\s*#pragma)")

                            If (Not bCommentStart AndAlso Not mRegMatch2.Groups("End").Success) Then
                                Exit For
                            End If

                            If (Not mRegMatch2.Groups("Pragma").Success) Then
                                bCommentStart = True
                            End If

                            Select Case (True)
                                Case mRegMatch2.Groups("End").Success AndAlso Not mRegMatch2.Groups("Pragma").Success
                                    sComment = sEnumSourceLines(iii).Substring(0, mRegMatch2.Groups("End").Index + 2) & Environment.NewLine & sComment
                                Case mRegMatch2.Groups("Start").Success
                                    sComment = sEnumSourceLines(iii).Substring(mRegMatch2.Groups("Start").Index, sEnumSourceLines(iii).Length - mRegMatch2.Groups("Start").Index) & Environment.NewLine & sComment
                                Case Else
                                    sComment = sEnumSourceLines(iii) & Environment.NewLine & sComment
                            End Select

                            If (bCommentStart AndAlso mRegMatch2.Groups("Start").Success) Then
                                Exit For
                            End If
                        Next

                        lEnumCommentArray(ii) = sComment
                    Next

                    For ii = 0 To lEnumSplitList.Count - 1
                        Dim sEnumFull As String = lEnumSplitList(ii)
                        Dim sEnumComment As String = lEnumCommentArray(ii)

                        sEnumComment = New Regex("^\s+", RegexOptions.Multiline).Replace(sEnumComment, "")

                        Dim regMatch As Match = Regex.Match(sEnumFull, "^\s*(?<Tag>\b[a-zA-Z0-9_]+\b:)*(?<Name>\b[a-zA-Z0-9_]+\b)")
                        If (Not regMatch.Groups("Name").Success) Then
                            g_mFormMain.PrintInformation("[ERRO]", String.Format("Failed to resolve type 'Enum': enum {0} {1}", sEnumName, sEnumFull))
                            Continue For
                        End If

                        Dim sEnumVarName As String = regMatch.Groups("Name").Value

                        Dim struc As New STRUC_AUTOCOMPLETE
                        struc.sFile = IO.Path.GetFileName(sFile)
                        struc.sFullFunctionName = String.Format("funcenum {0} {1}", sEnumName, sEnumFull)
                        struc.sFunctionName = "public " & (New Regex("\b(public)\b").Replace(sEnumFull, sEnumName, 1)) 'sEnumFull.Replace("public(", sEnumName & "(" )
                        struc.sInfo = sEnumComment
                        struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM

                        If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                            lTmpAutocompleteList.Add(struc)
                        End If
                    Next
                Next
            End If

            'Get methodmaps
            If ((ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("methodmap")) Then
                Dim mPossibleMethodmapMatches As MatchCollection = Regex.Matches(sSource, "^\s*\b(methodmap)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)(?<ParentingName>\s+\b[a-zA-Z0-9_]+\b){0,1}(?<FullParent>\s*\<\s*(?<Parent>\b[a-zA-Z0-9_]+\b)){0,1}\s*(?<BraceStart>\{)", RegexOptions.Multiline)
                Dim iBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sSource, "{"c, "}"c, 1, True)

                Dim mRegMatch As Match

                Dim bIsValid As Boolean
                Dim sMethodmapSource As String
                Dim iBraceIndex As Integer

                Dim sMethodMapName As String
                Dim sMethodMapHasParent As Boolean
                Dim sMethodMapParentName As String
                Dim sMethodMapFullParentName As String
                Dim sMethodMapParentingName As String

                For i = 0 To mPossibleMethodmapMatches.Count - 1
                    mRegMatch = mPossibleMethodmapMatches(i)
                    If (Not mRegMatch.Success) Then
                        Continue For
                    End If

                    bIsValid = False
                    sMethodmapSource = ""
                    iBraceIndex = mRegMatch.Groups("BraceStart").Index
                    For ii = 0 To iBraceList.Length - 1
                        If (iBraceIndex = iBraceList(ii)(0)) Then
                            sMethodmapSource = sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                            bIsValid = True
                            Exit For
                        End If
                    Next
                    If (Not bIsValid) Then
                        Continue For
                    End If

                    sMethodMapName = mRegMatch.Groups("Name").Value
                    sMethodMapHasParent = mRegMatch.Groups("Parent").Success
                    sMethodMapParentName = mRegMatch.Groups("Parent").Value
                    sMethodMapFullParentName = mRegMatch.Groups("FullParent").Value
                    sMethodMapParentingName = mRegMatch.Groups("ParentingName").Value

                    If (True) Then
                        Dim struc As New STRUC_AUTOCOMPLETE
                        struc.sFile = IO.Path.GetFileName(sFile)
                        struc.sFullFunctionName = "methodmap " & sMethodMapName & sMethodMapFullParentName
                        struc.sFunctionName = sMethodMapName
                        struc.sInfo = ""
                        struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP

                        If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                            lTmpAutocompleteList.Add(struc)
                        End If
                    End If

                    Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sMethodmapSource)
                    Dim iMethodmapBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sMethodmapSource, "("c, ")"c, 1, True)

                    Dim mMethodMatches As MatchCollection = Regex.Matches(sMethodmapSource,
                                                                          String.Format("^\s*(?<Type>\b(property|public\s+(static\s+){2}native|public)\b)\s+((?<Tag>\b({0})\b\s)\s*(?<Name>\b[a-zA-Z0-9_]+\b)|(?<Constructor>\b{1}\b)|(?<Name>\b[a-zA-Z0-9_]+\b))\s*(?<BraceStart>\(){3}", sRegExEnum, sMethodMapName, "{0,1}", "{0,1}"),
                                                                          RegexOptions.Multiline)

                    Dim SB As StringBuilder

                    For ii = 0 To mMethodMatches.Count - 1
                        If (sourceAnalysis.InNonCode(mMethodMatches(ii).Groups("Type").Index)) Then
                            Continue For
                        End If

                        SB = New StringBuilder
                        For iii = mMethodMatches(ii).Groups("Type").Index - 1 To 0 Step -1
                            Select Case (sMethodmapSource(iii))
                                Case " "c, vbTab(0), vbLf(0), vbCr(0)
                                    SB.Append(sMethodmapSource(iii))
                                Case Else
                                    If (Not sourceAnalysis.InMultiComment(iii) AndAlso Not sourceAnalysis.InSingleComment(iii) AndAlso Not sourceAnalysis.InPreprocessor(iii)) Then
                                        Exit For
                                    End If

                                    SB.Append(sMethodmapSource(iii))
                            End Select
                        Next

                        Dim sComment As String = StrReverse(SB.ToString)
                        sComment = Regex.Replace(sComment, "^\s+", "", RegexOptions.Multiline)
                        sComment = Regex.Replace(sComment, "\s+$", "", RegexOptions.Multiline)
                        Dim sType As String = mMethodMatches(ii).Groups("Type").Value
                        Dim sTag As String = mMethodMatches(ii).Groups("Tag").Value.Trim
                        Dim sName As String = mMethodMatches(ii).Groups("Name").Value

                        If (sType = "property") Then
                            Dim struc As New STRUC_AUTOCOMPLETE
                            struc.sFile = IO.Path.GetFileName(sFile)
                            struc.sFullFunctionName = String.Format("methodmap {0}{1}{2} {3} {4}", sMethodMapName, sMethodMapParentingName, If(sMethodMapHasParent, " < " & sMethodMapParentName, ""), sTag, sMethodMapName)
                            struc.sFunctionName = String.Format("{0}.{1}", sMethodMapName, sName)
                            struc.sInfo = sComment
                            struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP Or struc.ParseTypeFullNames(sType)

                            If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                                lTmpAutocompleteList.Add(struc)
                            End If
                        Else
                            Dim bIsConstructor As Boolean = mMethodMatches(ii).Groups("Constructor").Success
                            Dim iBraceStart As Integer = mMethodMatches(ii).Groups("BraceStart").Index
                            Dim sBraceString As String = Nothing

                            For iii = 0 To iMethodmapBraceList.Length - 1
                                If (iBraceStart = iMethodmapBraceList(iii)(0)) Then
                                    sBraceString = sMethodmapSource.Substring(iMethodmapBraceList(iii)(0), iMethodmapBraceList(iii)(1) - iMethodmapBraceList(iii)(0) + 1)
                                    Exit For
                                End If
                            Next

                            If (String.IsNullOrEmpty(sBraceString)) Then
                                Continue For
                            End If

                            If (bIsConstructor) Then
                                Dim struc As New STRUC_AUTOCOMPLETE
                                struc.sFile = IO.Path.GetFileName(sFile)
                                struc.sFullFunctionName = String.Format("methodmap {0}{1} {2}{3}", sMethodMapName, If(sMethodMapHasParent, " < " & sMethodMapParentName, ""), sMethodMapName, sBraceString)
                                struc.sFunctionName = String.Format("{0}.{1}", sMethodMapName, sMethodMapName)
                                struc.sInfo = sComment
                                struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP Or struc.ParseTypeFullNames(sType)

                                If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                                    lTmpAutocompleteList.Add(struc)
                                End If
                            Else
                                Dim struc As New STRUC_AUTOCOMPLETE
                                struc.sFile = IO.Path.GetFileName(sFile)
                                struc.sFullFunctionName = String.Format("methodmap {0} {1} {2}{3} {4}{5}", sType, sTag, sMethodMapName, If(sMethodMapHasParent, " < " & sMethodMapParentName, ""), sName, sBraceString)
                                struc.sFunctionName = String.Format("{0}.{1}", sMethodMapName, sName)
                                struc.sInfo = sComment
                                struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP Or struc.ParseTypeFullNames(sType)

                                If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                                    lTmpAutocompleteList.Add(struc)
                                End If
                            End If
                        End If
                    Next
                Next
            End If

            'Get typesets
            If ((ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("typeset")) Then
                Dim mPossibleMethodmapMatches As MatchCollection = Regex.Matches(sSource, "^\s*\b(typeset)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s*(?<BraceStart>\{)", RegexOptions.Multiline)
                Dim iBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sSource, "{"c, "}"c, 1, True)

                Dim mRegMatch As Match

                Dim bIsValid As Boolean
                Dim sMethodmapSource As String
                Dim iBraceIndex As Integer

                Dim sMethodMapName As String

                For i = 0 To mPossibleMethodmapMatches.Count - 1
                    mRegMatch = mPossibleMethodmapMatches(i)
                    If (Not mRegMatch.Success) Then
                        Continue For
                    End If

                    bIsValid = False
                    sMethodmapSource = ""
                    iBraceIndex = mRegMatch.Groups("BraceStart").Index
                    For ii = 0 To iBraceList.Length - 1
                        If (iBraceIndex = iBraceList(ii)(0)) Then
                            sMethodmapSource = sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                            bIsValid = True
                            Exit For
                        End If
                    Next
                    If (Not bIsValid) Then
                        Continue For
                    End If

                    sMethodMapName = mRegMatch.Groups("Name").Value

                    If (True) Then
                        Dim struc As New STRUC_AUTOCOMPLETE
                        struc.sFile = IO.Path.GetFileName(sFile)
                        struc.sFullFunctionName = "enum " & sMethodMapName
                        struc.sFunctionName = sMethodMapName
                        struc.sInfo = ""
                        struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM

                        If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                            lTmpAutocompleteList.Add(struc)
                        End If
                    End If

                    Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sMethodmapSource)
                    Dim iMethodmapBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sMethodmapSource, "("c, ")"c, 1, True)

                    Dim mMethodMatches As MatchCollection = Regex.Matches(sMethodmapSource, String.Format("^\s*(?<Type>\b(function)\b)\s+(?<Tag>\b({0})\b)\s*(?<BraceStart>\()", sRegExEnum), RegexOptions.Multiline)

                    Dim SB As StringBuilder

                    For ii = 0 To mMethodMatches.Count - 1
                        If (sourceAnalysis.InNonCode(mMethodMatches(ii).Groups("Type").Index)) Then
                            Continue For
                        End If

                        SB = New StringBuilder
                        For iii = mMethodMatches(ii).Groups("Type").Index - 1 To 0 Step -1
                            Select Case (sMethodmapSource(iii))
                                Case " "c, vbTab(0), vbLf(0), vbCr(0)
                                    SB.Append(sMethodmapSource(iii))
                                Case Else
                                    If (Not sourceAnalysis.InMultiComment(iii) AndAlso Not sourceAnalysis.InSingleComment(iii) AndAlso Not sourceAnalysis.InPreprocessor(iii)) Then
                                        Exit For
                                    End If

                                    SB.Append(sMethodmapSource(iii))
                            End Select
                        Next

                        Dim sComment As String = StrReverse(SB.ToString)
                        sComment = Regex.Replace(sComment, "^\s+", "", RegexOptions.Multiline)
                        sComment = Regex.Replace(sComment, "\s+$", "", RegexOptions.Multiline)
                        Dim sType As String = mMethodMatches(ii).Groups("Type").Value
                        Dim sTag As String = mMethodMatches(ii).Groups("Tag").Value.Trim
                        Dim iBraceStart As Integer = mMethodMatches(ii).Groups("BraceStart").Index
                        Dim sBraceString As String = Nothing

                        For iii = 0 To iMethodmapBraceList.Length - 1
                            If (iBraceStart = iMethodmapBraceList(iii)(0)) Then
                                sBraceString = sMethodmapSource.Substring(iMethodmapBraceList(iii)(0), iMethodmapBraceList(iii)(1) - iMethodmapBraceList(iii)(0) + 1)
                                Exit For
                            End If
                        Next

                        If (String.IsNullOrEmpty(sBraceString)) Then
                            Continue For
                        End If

                        Dim struc As New STRUC_AUTOCOMPLETE
                        struc.sFile = IO.Path.GetFileName(sFile)
                        struc.sFullFunctionName = String.Format("typeset {0} {1} {2}{3}", sMethodMapName, sType, sTag, sBraceString)
                        struc.sFunctionName = String.Format("public {0} {1}{2}", sTag, sMethodMapName, sBraceString)
                        struc.sInfo = sComment
                        struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET Or struc.ParseTypeFullNames(sType)

                        If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                            lTmpAutocompleteList.Add(struc)
                        End If
                    Next
                Next
            End If

            If ((ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7 OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX) AndAlso sSource.Contains("typedef")) Then
                Dim mPossibleMethodmapMatches As MatchCollection = Regex.Matches(sSource, String.Format("^\s*\b(typedef)\b\s+(?<Name>\b[a-zA-Z0-9_]+\b)\s+=\s+\b(function)\b\s+(?<Tag>\b({0})\b)\s*(?<BraceStart>\()", sRegExEnum), RegexOptions.Multiline)
                Dim iBraceList As Integer()() = g_mFormMain.g_ClassSyntaxTools.GetExpressionBetweenCharacters(sSource, "("c, ")"c, 1, True)

                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                For i = 0 To mPossibleMethodmapMatches.Count - 1
                    Dim mRegMatch As Match = mPossibleMethodmapMatches(i)
                    If (Not mRegMatch.Success) Then
                        Continue For
                    End If

                    If (sourceAnalysis.InNonCode(mRegMatch.Index)) Then
                        Continue For
                    End If

                    Dim bIsValid As Boolean = False
                    Dim sBraceString As String = ""
                    Dim iBraceIndex As Integer = mRegMatch.Groups("BraceStart").Index
                    For ii = 0 To iBraceList.Length - 1
                        If (iBraceIndex = iBraceList(ii)(0)) Then
                            sBraceString = sSource.Substring(iBraceList(ii)(0) + 1, iBraceList(ii)(1) - iBraceList(ii)(0) - 1)
                            bIsValid = True
                            Exit For
                        End If
                    Next
                    If (Not bIsValid) Then
                        Continue For
                    End If

                    If (String.IsNullOrEmpty(sBraceString)) Then
                        Continue For
                    End If

                    sBraceString = sBraceString.Trim

                    Dim sName As String = mRegMatch.Groups("Name").Value
                    Dim sTag As String = mRegMatch.Groups("Tag").Value

                    Dim SB As New StringBuilder
                    For iii = mRegMatch.Index - 1 To 0 Step -1
                        Select Case (sSource(iii))
                            Case " "c, vbTab(0), vbLf(0), vbCr(0)
                                SB.Append(sSource(iii))
                            Case Else
                                If (Not sourceAnalysis.InMultiComment(iii) AndAlso Not sourceAnalysis.InSingleComment(iii) AndAlso Not sourceAnalysis.InPreprocessor(iii)) Then
                                    Exit For
                                End If

                                SB.Append(sSource(iii))
                        End Select
                    Next

                    Dim sComment As String = StrReverse(SB.ToString)
                    sComment = Regex.Replace(sComment, "^\s+", "", RegexOptions.Multiline)
                    sComment = Regex.Replace(sComment, "\s+$", "", RegexOptions.Multiline)


                    Dim struc As New STRUC_AUTOCOMPLETE
                    struc.sFile = IO.Path.GetFileName(sFile)
                    struc.sFullFunctionName = String.Format("typedef {0} = function {1} ({2})", sName, sTag, sBraceString)
                    struc.sFunctionName = String.Format("public {0} {1}({2})", sTag, sName, sBraceString)
                    struc.sInfo = sComment
                    struc.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF

                    If (Not lTmpAutocompleteList.Exists(Function(struc2 As STRUC_AUTOCOMPLETE) struc2.mType = struc.mType AndAlso struc2.sFunctionName = struc.sFunctionName)) Then 'Not lTmpAutoList.Contains(struc)
                        lTmpAutocompleteList.Add(struc)
                    End If
                Next
            End If
        End Sub

        Private Sub VariableAutocompleteUpdate_Thread()
            Try
                'g_mFormMain.PrintInformation("[INFO]", "Variable autocomplete update started...")
                If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                    'g_mFormMain.PrintInformation("[ERRO]", "Variable autocomplete update failed! Could not get current source file!")
                    Return
                End If

                'No autocomplete entries?
                If (g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.Count < 1) Then
                    Return
                End If


                Dim lTmpVarAutocompleteList As New ClassSyncList(Of STRUC_AUTOCOMPLETE)

                'Parse variables and create methodmaps for variables
                If (True) Then
                    Dim sRegExEnumPattern As String = String.Format("(\b{0}\b)", String.Join("\b|\b", GetEnumNames(g_mFormMain.g_ClassSyntaxTools.lAutocompleteList)))

                    If (ClassSettings.g_iSettingsVarAutocompleteCurrentSourceOnly) Then
                        ParseVariables_Pre(ClassSettings.g_sConfigOpenSourceFile, sRegExEnumPattern, lTmpVarAutocompleteList, g_mFormMain.g_ClassSyntaxTools.lAutocompleteList)
                    Else
                        Dim sFiles As String() = GetIncludeFiles(ClassSettings.g_sConfigOpenSourceFile)
                        For i = 0 To sFiles.Length - 1
                            ParseVariables_Pre(sFiles(i), sRegExEnumPattern, lTmpVarAutocompleteList, g_mFormMain.g_ClassSyntaxTools.lAutocompleteList)
                        Next
                    End If

                    ParseVariables_Post(sRegExEnumPattern, lTmpVarAutocompleteList, g_mFormMain.g_ClassSyntaxTools.lAutocompleteList)
                End If

                g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.RemoveAll(Function(j As STRUC_AUTOCOMPLETE) (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                g_mFormMain.g_ClassSyntaxTools.lAutocompleteList.AddRange(lTmpVarAutocompleteList.ToArray)

                'g_mFormMain.PrintInformation("[INFO]", "Variable autocomplete update finished!")
            Catch ex As Threading.ThreadAbortException
                Throw
            Catch ex As Exception
                g_mFormMain.PrintInformation("[ERRO]", "Variable autocomplete update failed! " & ex.Message)
                ClassExceptionLog.WriteToLog(ex)
            End Try
        End Sub

        Private Structure STRUC_PARSE_ARGUMENT_ITEM
            Dim sArgument As String
            Dim sFile As String
        End Structure

        Private Sub ParseVariables_Pre(ByRef sFile As String, ByRef sRegExEnumPattern As String, ByRef lTmpVarAutocompleteList As ClassSyncList(Of STRUC_AUTOCOMPLETE), ByRef lTmpAutocompleteList As ClassSyncList(Of STRUC_AUTOCOMPLETE))
            Dim sSource As String
            If (ClassSettings.g_sConfigOpenSourceFile.ToLower = sFile.ToLower) Then
                sSource = CStr(g_mFormMain.Invoke(Function() g_mFormMain.TextEditorControl_Source.Document.TextContent))
            Else
                sSource = IO.File.ReadAllText(sFile)
            End If

            CleanUpNewLinesSource(sSource)

            'Parse variables
            If (True) Then
                Dim sInitTypesPattern As String = "\b(new|decl|static|const)\b" '"public" is already taken care off
                Dim sOldStyleVarPattern As String = String.Format("(?<Init>{0})\s+((?<Tag>{1})\:\s*(?<Var>\b[a-zA-Z0-9_]+\b)|(?<Var>\b[a-zA-Z0-9_]+\b))($|\W)", sInitTypesPattern, sRegExEnumPattern)
                Dim sNewStyleVarPattern As String = String.Format("(?<Tag>{0})\s+(?<Var>\b[a-zA-Z0-9_]+\b)($|\W)", sRegExEnumPattern)

                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)
                Dim lCommaLinesList As New List(Of String)
                Dim codeBuilder As New StringBuilder

                Dim bInvalidBracketLevel As Boolean = False
                Dim bInvalidParentLevel As Boolean = False
                'This is the easiest and yet stupiest idea to resolve multi-decls i've ever come up with...
                For i = 0 To sSource.Length - 1
                    If (i <> sSource.Length - 1) Then
                        If (sourceAnalysis.InNonCode(i)) Then
                            Continue For
                        End If

                        If (sourceAnalysis.GetBracketLevel(i) > 0) Then
                            If (Not bInvalidBracketLevel) Then
                                bInvalidBracketLevel = True
                            End If
                            Continue For
                        ElseIf (bInvalidBracketLevel) Then
                            bInvalidBracketLevel = False
                            Continue For
                        End If

                        If (sourceAnalysis.GetParenthesisLevel(i) > 0) Then
                            If (Not bInvalidParentLevel) Then
                                bInvalidParentLevel = True
                                codeBuilder.Append("(")
                            End If
                            Continue For
                        ElseIf (bInvalidParentLevel) Then
                            bInvalidParentLevel = False
                            codeBuilder.Append(")")
                            Continue For
                        End If
                    End If

                    If (sSource(i) = ","c OrElse sSource(i) = "="c OrElse i = sSource.Length - 1) Then
                        Dim sLine As String = codeBuilder.ToString.Trim

                        If (Not String.IsNullOrEmpty(sLine)) Then
                            lCommaLinesList.Add(sLine)
                        End If

                        codeBuilder.Length = 0
                        Continue For
                    End If

                    codeBuilder.Append(sSource(i))
                Next

                '0: Not yet found
                '1: Old style
                '2: New style
                Dim iLastInitStyle As Byte = 0
                Dim iLastInitIndex As Integer = 0

                For Each sLine As String In lCommaLinesList
                    Select Case (iLastInitStyle)
                        Case 1 'Old Style
                            If (ClassSettings.g_iSettingsAutocompleteSyntax <> ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX AndAlso ClassSettings.g_iSettingsAutocompleteSyntax <> ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6) Then
                                Exit Select
                            End If

                            Dim mMatch As Match = Regex.Match(sLine, String.Format("^((?<Tag>{0})\:\s*)*(?<Var>\b[a-zA-Z_][a-zA-Z0-9_]*\b)((?<End>$)|(?<Func>\()|(?<More>\W))", sRegExEnumPattern))
                            Dim sTag As String = If(String.IsNullOrEmpty(mMatch.Groups("Tag").Value), "int", mMatch.Groups("Tag").Value).Trim
                            Dim sVar As String = mMatch.Groups("Var").Value.Trim

                            If (mMatch.Groups("Func").Success OrElse Not mMatch.Groups("Var").Success) Then
                                Exit Select
                            End If

                            If (g_mFormMain.g_ClassSyntaxTools.IsForbiddenVariableName(sVar)) Then
                                Exit Select
                            End If

                            If (lTmpAutocompleteList.Exists(Function(j As STRUC_AUTOCOMPLETE) (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE AndAlso Regex.IsMatch(j.sFunctionName, String.Format("\b{0}\b", Regex.Escape(sVar))))) Then
                                Exit Select
                            End If

                            Dim tmpFile As String = IO.Path.GetFileName(sFile)
                            Dim autoItem As STRUC_AUTOCOMPLETE = lTmpVarAutocompleteList.Find(Function(j As STRUC_AUTOCOMPLETE) j.sFile.ToLower = tmpFile.ToLower AndAlso j.sFunctionName = sVar AndAlso (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                            If (autoItem Is Nothing) Then
                                autoItem = New STRUC_AUTOCOMPLETE
                                autoItem.sFile = tmpFile.ToLower
                                autoItem.sFullFunctionName = String.Format("{0} > {1}", sVar, sTag)
                                autoItem.sFunctionName = sVar
                                autoItem.sInfo = ""
                                autoItem.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                                lTmpVarAutocompleteList.Add(autoItem)
                            Else
                                If (Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                    autoItem.sFullFunctionName = String.Format("{0}|{1}", autoItem.sFullFunctionName, sTag)
                                End If
                            End If

                        Case 2 'New Style
                            If (ClassSettings.g_iSettingsAutocompleteSyntax <> ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX AndAlso ClassSettings.g_iSettingsAutocompleteSyntax <> ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7) Then
                                Exit Select
                            End If

                            Dim mMatch As Match = Regex.Match(sLine, String.Format("^(?<Tag>{0}\s+)*(?<Var>\b[a-zA-Z_][a-zA-Z0-9_]*\b)((?<End>$)|(?<Func>\()|(?<More>\W))", sRegExEnumPattern))
                            Dim sTag As String = If(String.IsNullOrEmpty(mMatch.Groups("Tag").Value), "int", mMatch.Groups("Tag").Value).Trim
                            Dim sVar As String = mMatch.Groups("Var").Value.Trim

                            If (mMatch.Groups("Func").Success OrElse Not mMatch.Groups("Var").Success) Then
                                Exit Select
                            End If

                            If (g_mFormMain.g_ClassSyntaxTools.IsForbiddenVariableName(sVar)) Then
                                Exit Select
                            End If

                            If (lTmpAutocompleteList.Exists(Function(j As STRUC_AUTOCOMPLETE) (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE AndAlso Regex.IsMatch(j.sFunctionName, String.Format("\b{0}\b", Regex.Escape(sVar))))) Then
                                Exit Select
                            End If

                            Dim tmpFile As String = IO.Path.GetFileName(sFile)
                            Dim autoItem As STRUC_AUTOCOMPLETE = lTmpVarAutocompleteList.Find(Function(j As STRUC_AUTOCOMPLETE) j.sFile.ToLower = tmpFile.ToLower AndAlso j.sFunctionName = sVar AndAlso (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                            If (autoItem Is Nothing) Then
                                autoItem = New STRUC_AUTOCOMPLETE
                                autoItem.sFile = tmpFile.ToLower
                                autoItem.sFullFunctionName = String.Format("{0} > {1}", sVar, sTag)
                                autoItem.sFunctionName = sVar
                                autoItem.sInfo = ""
                                autoItem.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                                lTmpVarAutocompleteList.Add(autoItem)
                            Else
                                If (Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                    autoItem.sFullFunctionName = String.Format("{0}|{1}", autoItem.sFullFunctionName, sTag)
                                End If
                            End If
                    End Select

                    If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsAutocompleteSyntax <> ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6) Then
                        For Each mMatch As Match In Regex.Matches(sLine, sOldStyleVarPattern)
                            Dim iIndex As Integer = mMatch.Groups("Var").Index
                            Dim sInit As String = mMatch.Groups("Init").Value.Trim
                            Dim sTag As String = If(String.IsNullOrEmpty(mMatch.Groups("Tag").Value), "int", mMatch.Groups("Tag").Value).Trim
                            Dim sVar As String = mMatch.Groups("Var").Value.Trim

                            If (iLastInitIndex < iIndex) Then
                                iLastInitIndex = iIndex
                                iLastInitStyle = 1 'Old style
                            End If

                            If (g_mFormMain.g_ClassSyntaxTools.IsForbiddenVariableName(sVar)) Then
                                Continue For
                            End If

                            If (lTmpAutocompleteList.Exists(Function(j As STRUC_AUTOCOMPLETE) (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE AndAlso Regex.IsMatch(j.sFunctionName, String.Format("\b{0}\b", Regex.Escape(sVar))))) Then
                                Continue For
                            End If

                            Dim tmpFile As String = IO.Path.GetFileName(sFile)
                            Dim autoItem As STRUC_AUTOCOMPLETE = lTmpVarAutocompleteList.Find(Function(j As STRUC_AUTOCOMPLETE) j.sFile.ToLower = tmpFile.ToLower AndAlso j.sFunctionName = sVar AndAlso (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                            If (autoItem Is Nothing) Then
                                autoItem = New STRUC_AUTOCOMPLETE
                                autoItem.sFile = tmpFile.ToLower
                                autoItem.sFullFunctionName = String.Format("{0} > {1}", sVar, sTag)
                                autoItem.sFunctionName = sVar
                                autoItem.sInfo = ""
                                autoItem.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                                lTmpVarAutocompleteList.Add(autoItem)
                            Else
                                If (Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                    autoItem.sFullFunctionName = String.Format("{0}|{1}", autoItem.sFullFunctionName, sTag)
                                End If
                            End If
                        Next
                    End If

                    If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsAutocompleteSyntax <> ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7) Then
                        For Each mMatch As Match In Regex.Matches(sLine, sNewStyleVarPattern)
                            Dim iIndex As Integer = mMatch.Groups("Var").Index
                            Dim sInit As String = mMatch.Groups("Init").Value.Trim
                            Dim sTag As String = If(String.IsNullOrEmpty(mMatch.Groups("Tag").Value), "_NONE_", mMatch.Groups("Tag").Value).Trim
                            Dim sVar As String = mMatch.Groups("Var").Value.Trim

                            If (iLastInitIndex < iIndex) Then
                                iLastInitIndex = iIndex
                                iLastInitStyle = 2 'New style
                            End If

                            If (g_mFormMain.g_ClassSyntaxTools.IsForbiddenVariableName(sVar)) Then
                                Continue For
                            End If

                            If (lTmpAutocompleteList.Exists(Function(j As STRUC_AUTOCOMPLETE) (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE AndAlso Regex.IsMatch(j.sFunctionName, String.Format("\b{0}\b", Regex.Escape(sVar))))) Then
                                Continue For
                            End If

                            Dim tmpFile As String = IO.Path.GetFileName(sFile)
                            Dim autoItem As STRUC_AUTOCOMPLETE = lTmpVarAutocompleteList.Find(Function(j As STRUC_AUTOCOMPLETE) j.sFile.ToLower = tmpFile.ToLower AndAlso j.sFunctionName = sVar AndAlso (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                            If (autoItem Is Nothing) Then
                                autoItem = New STRUC_AUTOCOMPLETE
                                autoItem.sFile = tmpFile.ToLower
                                autoItem.sFullFunctionName = String.Format("{0} > {1}", sVar, sTag)
                                autoItem.sFunctionName = sVar
                                autoItem.sInfo = ""
                                autoItem.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                                lTmpVarAutocompleteList.Add(autoItem)
                            Else
                                If (Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                    autoItem.sFullFunctionName = String.Format("{0}|{1}", autoItem.sFullFunctionName, sTag)
                                End If
                            End If
                        Next
                    End If
                Next
            End If

        End Sub

        Private Sub ParseVariables_Post(ByRef sRegExEnumPattern As String, ByRef lTmpVarAutocompleteList As ClassSyncList(Of STRUC_AUTOCOMPLETE), ByRef lTmpAutocompleteList As ClassSyncList(Of STRUC_AUTOCOMPLETE))
            'Parse function argument variables
            If (True) Then
                Dim lArgList As New List(Of STRUC_PARSE_ARGUMENT_ITEM)
                Dim codeBuilder As New StringBuilder

                For Each autoItem In lTmpAutocompleteList
                    If (ClassSettings.g_iSettingsVarAutocompleteCurrentSourceOnly) Then
                        If (Not String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) AndAlso IO.Path.GetFileName(ClassSettings.g_sConfigOpenSourceFile).ToLower <> autoItem.sFile.ToLower) Then
                            Continue For
                        End If
                    End If

                    Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(autoItem.sFullFunctionName)
                    codeBuilder.Length = 0

                    Dim bInvalidBracketLevel As Boolean = False
                    Dim bInvalidBraceLevel As Boolean = False
                    Dim bInvalidParentLevel As Boolean = False
                    Dim bNeedSave As Boolean = False
                    'This is the easiest and yet stupiest idea to resolve multi-decls i've ever come up with...
                    For i = 0 To autoItem.sFullFunctionName.Length - 1
                        If (bNeedSave OrElse i = autoItem.sFullFunctionName.Length - 1) Then
                            bNeedSave = False

                            Dim sLine As String = codeBuilder.ToString.Trim
                            sLine = Regex.Replace(sLine, "(\=)+(.*$)", "")
                            sLine = Regex.Replace(sLine, Regex.Escape("&"), "")
                            sLine = Regex.Replace(sLine, Regex.Escape("@"), "")

                            If (Not String.IsNullOrEmpty(sLine)) Then
                                lArgList.Add(New STRUC_PARSE_ARGUMENT_ITEM With {.sArgument = sLine, .sFile = autoItem.sFile})
                            End If

                            codeBuilder.Length = 0
                        End If

                        If (sourceAnalysis.InNonCode(i)) Then
                            Continue For
                        End If

                        If (sourceAnalysis.GetBracketLevel(i) > 0) Then
                            If (Not bInvalidBracketLevel) Then
                                bInvalidBracketLevel = True
                            End If
                            Continue For
                        ElseIf (bInvalidBracketLevel) Then
                            bInvalidBracketLevel = False
                            Continue For
                        End If

                        If (sourceAnalysis.GetBraceLevel(i) > 0) Then
                            If (Not bInvalidBraceLevel) Then
                                bInvalidBraceLevel = True
                            End If
                            Continue For
                        ElseIf (bInvalidBraceLevel) Then
                            bInvalidBraceLevel = False
                            Continue For
                        End If

                        If (sourceAnalysis.GetParenthesisLevel(i) > 0) Then
                            If (bInvalidParentLevel) Then
                                If (autoItem.sFullFunctionName(i) = ","c) Then
                                    bNeedSave = True
                                    Continue For
                                Else
                                    codeBuilder.Append(autoItem.sFullFunctionName(i))
                                End If
                            End If
                            If (Not bInvalidParentLevel) Then
                                bInvalidParentLevel = True
                            End If

                            Continue For
                        ElseIf (bInvalidParentLevel) Then
                            bInvalidParentLevel = False
                            bNeedSave = True
                            Continue For
                        End If
                    Next
                Next

                For Each sArg As STRUC_PARSE_ARGUMENT_ITEM In lArgList
                    'Old style
                    If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6) Then
                        Dim mMatch As Match = Regex.Match(sArg.sArgument, String.Format("((?<Tag>\b{0}\b)\:\s*)*(?<Var>\b[a-zA-Z_][a-zA-Z0-9_]*\b)$", sRegExEnumPattern))
                        Dim sTag As String = If(String.IsNullOrEmpty(mMatch.Groups("Tag").Value), "int", mMatch.Groups("Tag").Value).Trim
                        Dim sVar As String = mMatch.Groups("Var").Value.Trim

                        If (mMatch.Success AndAlso mMatch.Groups("Var").Success AndAlso Not g_mFormMain.g_ClassSyntaxTools.IsForbiddenVariableName(sVar)) Then
                            Dim tmpFile As String = IO.Path.GetFileName(sArg.sFile)
                            Dim autoItem As STRUC_AUTOCOMPLETE = lTmpVarAutocompleteList.Find(Function(j As STRUC_AUTOCOMPLETE) j.sFile.ToLower = tmpFile.ToLower AndAlso j.sFunctionName = sVar AndAlso (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                            If (autoItem Is Nothing) Then
                                autoItem = New STRUC_AUTOCOMPLETE
                                autoItem.sFile = tmpFile.ToLower
                                autoItem.sFullFunctionName = String.Format("{0} > {1}", sVar, sTag)
                                autoItem.sFunctionName = sVar
                                autoItem.sInfo = ""
                                autoItem.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                                lTmpVarAutocompleteList.Add(autoItem)
                            Else
                                If (Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                    autoItem.sFullFunctionName = String.Format("{0}|{1}", autoItem.sFullFunctionName, sTag)
                                End If
                            End If
                        End If
                    End If

                    'New style
                    If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7) Then
                        Dim mMatch As Match = Regex.Match(sArg.sArgument, String.Format("(?<Tag>\b{0}\b)\s+(?<Var>\b[a-zA-Z_][a-zA-Z0-9_]*\b)$", sRegExEnumPattern))
                        Dim sTag As String = If(String.IsNullOrEmpty(mMatch.Groups("Tag").Value), "int", mMatch.Groups("Tag").Value).Trim
                        Dim sVar As String = mMatch.Groups("Var").Value.Trim

                        If (mMatch.Success AndAlso mMatch.Groups("Var").Success AndAlso Not g_mFormMain.g_ClassSyntaxTools.IsForbiddenVariableName(sVar)) Then
                            Dim tmpFile As String = IO.Path.GetFileName(sArg.sFile)
                            Dim autoItem As STRUC_AUTOCOMPLETE = lTmpVarAutocompleteList.Find(Function(j As STRUC_AUTOCOMPLETE) j.sFile.ToLower = tmpFile.ToLower AndAlso j.sFunctionName = sVar AndAlso (j.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE)
                            If (autoItem Is Nothing) Then
                                autoItem = New STRUC_AUTOCOMPLETE
                                autoItem.sFile = tmpFile.ToLower
                                autoItem.sFullFunctionName = String.Format("{0} > {1}", sVar, sTag)
                                autoItem.sFunctionName = sVar
                                autoItem.sInfo = ""
                                autoItem.mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE
                                lTmpVarAutocompleteList.Add(autoItem)
                            Else
                                If (Not Regex.IsMatch(autoItem.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sTag)))) Then
                                    autoItem.sFullFunctionName = String.Format("{0}|{1}", autoItem.sFullFunctionName, sTag)
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            'Make methodmaps using variables
            If (ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX OrElse ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7) Then
                Dim lVarMethodmapList As New List(Of STRUC_AUTOCOMPLETE)

                For Each item In lTmpVarAutocompleteList
                    If ((item.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) Then
                        Continue For
                    End If

                    For Each item2 In lTmpAutocompleteList
                        If ((item2.mType And STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP OrElse Not item2.sFunctionName.Contains("."c)) Then
                            Continue For
                        End If

                        Dim splitMethodmap As String() = item2.sFunctionName.Split(New String() {"."c}, 0)
                        If (splitMethodmap.Length <> 2) Then
                            Continue For
                        End If

                        Dim sMethodmapName As String = splitMethodmap(0)
                        Dim sMethodmapMethod As String = splitMethodmap(1)

                        'If (lTmpAutocompleteList.Exists(Function(j As STRUC_AUTOCOMPLETE) Regex.IsMatch(j.sFunctionName, "\b" & Regex.Escape(item.sFunctionName & "." & sMethodmapMethod) & "\b"))) Then
                        '    Continue For
                        'End If

                        If (Not Regex.IsMatch(item.sFullFunctionName, String.Format("\b{0}\b", Regex.Escape(sMethodmapName)))) Then
                            Continue For
                        End If

                        Dim autoItem As New STRUC_AUTOCOMPLETE
                        autoItem.sFile = IO.Path.GetFileName(item.sFile).ToLower
                        autoItem.sFullFunctionName = String.Format("{0}.{1}", sMethodmapName, sMethodmapMethod)
                        autoItem.sFunctionName = String.Format("{0}.{1}", item.sFunctionName, sMethodmapMethod)
                        autoItem.sInfo = item2.sInfo
                        autoItem.mType = (STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE Or item2.mType) And Not STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP
                        lVarMethodmapList.Add(autoItem)
                    Next
                Next

                lTmpVarAutocompleteList.AddRange(lVarMethodmapList)
            End If
        End Sub

        ''' <summary>
        ''' Gets all include files from a file
        ''' </summary>
        ''' <param name="sPath"></param>
        ''' <returns>Array if include file paths</returns>
        Public Function GetIncludeFiles(sPath As String) As String()
            Dim lList As New List(Of String)
            GetIncludeFilesRecursive(sPath, lList)

            Return lList.ToArray
        End Function

        Private Sub GetIncludeFilesRecursive(sPath As String, ByRef lList As List(Of String))
            Dim sSource As String
            If (ClassSettings.g_sConfigOpenSourceFile.ToLower = sPath.ToLower) Then
                If (lList.Contains(sPath)) Then
                    Return
                End If

                lList.Add(sPath)

                sSource = CStr(g_mFormMain.Invoke(Function() g_mFormMain.TextEditorControl_Source.Document.TextContent))
            Else
                If (Not IO.File.Exists(sPath)) Then
                    g_mFormMain.PrintInformation("[ERRO]", String.Format("Could not read include: {0}", IO.Path.GetFileName(sPath)))
                    Return
                End If

                If (lList.Contains(sPath)) Then
                    Return
                End If

                lList.Add(sPath)

                sSource = IO.File.ReadAllText(sPath)
            End If

            Dim lPathList As New List(Of String)
            Dim sCurrentDir As String = IO.Path.GetDirectoryName(sPath)

            Using SR As New IO.StringReader(sSource)
                While True
                    Dim sLine As String = SR.ReadLine()
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    If (Not sLine.Contains("#include") AndAlso Not sLine.Contains("#tryinclude")) Then
                        Continue While
                    End If

                    Dim mRegMatch As Match = Regex.Match(sLine, "^\s*#(include|tryinclude)\s+(\<(?<PathInc>.*?)\>|""(?<PathFull>.*?)"")\s*$", RegexOptions.Compiled)
                    If (Not mRegMatch.Success) Then
                        Continue While
                    End If

                    Dim sCorrectPath As String
                    Dim sMatchValue As String

                    Dim sIncludeDir As String = ClassSettings.g_sConfigOpenIncludeFolder
                    If (ClassSettings.g_iConfigCompilingType = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC) Then
                        If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourceFile)) Then
                            g_mFormMain.PrintInformation("[ERRO]", "Could not read includes! Could not get current source file!")
                            Exit While
                        End If
                        sIncludeDir = IO.Path.Combine(IO.Path.GetDirectoryName(ClassSettings.g_sConfigOpenSourceFile), "include")
                    End If

                    Select Case (True)
                        Case mRegMatch.Groups("PathInc").Success
                            sMatchValue = mRegMatch.Groups("PathInc").Value
                            sMatchValue = sMatchValue.Replace("/"c, "\"c)
                            Select Case (True)
                                Case IO.File.Exists(IO.Path.Combine(sIncludeDir, sMatchValue))
                                    sCorrectPath = IO.Path.Combine(sIncludeDir, sMatchValue)
                                Case IO.File.Exists(String.Format("{0}.sp", IO.Path.Combine(sIncludeDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.sp", IO.Path.Combine(sIncludeDir, sMatchValue))
                                Case IO.File.Exists(String.Format("{0}.sma", IO.Path.Combine(sIncludeDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.sma", IO.Path.Combine(sIncludeDir, sMatchValue))
                                Case IO.File.Exists(String.Format("{0}.p", IO.Path.Combine(sIncludeDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.p", IO.Path.Combine(sIncludeDir, sMatchValue))
                                Case IO.File.Exists(String.Format("{0}.pwn", IO.Path.Combine(sIncludeDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.pwn", IO.Path.Combine(sIncludeDir, sMatchValue))
                                Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sIncludeDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.inc", IO.Path.Combine(sIncludeDir, sMatchValue))
                                Case Else
                                    g_mFormMain.PrintInformation("[ERRO]", String.Format("Could not read include: {0}", sMatchValue))
                                    Continue While
                            End Select

                        Case mRegMatch.Groups("PathFull").Success
                            sMatchValue = mRegMatch.Groups("PathFull").Value
                            sMatchValue = sMatchValue.Replace("/"c, "\"c)
                            Select Case (True)
                                Case sMatchValue(1) = ":"c AndAlso IO.File.Exists(sMatchValue)
                                    sCorrectPath = sMatchValue
                                Case IO.File.Exists(IO.Path.Combine(sCurrentDir, sMatchValue))
                                    sCorrectPath = IO.Path.Combine(sCurrentDir, sMatchValue)
                                Case IO.File.Exists(String.Format("{0}.sp", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.sp", IO.Path.Combine(sCurrentDir, sMatchValue))
                                Case IO.File.Exists(String.Format("{0}.sma", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.sma", IO.Path.Combine(sCurrentDir, sMatchValue))
                                Case IO.File.Exists(String.Format("{0}.p", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.p", IO.Path.Combine(sCurrentDir, sMatchValue))
                                Case IO.File.Exists(String.Format("{0}.pwn", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.pwn", IO.Path.Combine(sCurrentDir, sMatchValue))
                                Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sCurrentDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.inc", IO.Path.Combine(sCurrentDir, sMatchValue))

                                Case IO.File.Exists(IO.Path.Combine(sIncludeDir, sMatchValue))
                                    sCorrectPath = IO.Path.Combine(sIncludeDir, sMatchValue)
                                Case IO.File.Exists(String.Format("{0}.sp", IO.Path.Combine(sIncludeDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.sp", IO.Path.Combine(sIncludeDir, sMatchValue))
                                Case IO.File.Exists(String.Format("{0}.sma", IO.Path.Combine(sIncludeDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.sma", IO.Path.Combine(sIncludeDir, sMatchValue))
                                Case IO.File.Exists(String.Format("{0}.p", IO.Path.Combine(sIncludeDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.p", IO.Path.Combine(sIncludeDir, sMatchValue))
                                Case IO.File.Exists(String.Format("{0}.pwn", IO.Path.Combine(sIncludeDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.pwn", IO.Path.Combine(sIncludeDir, sMatchValue))
                                Case IO.File.Exists(String.Format("{0}.inc", IO.Path.Combine(sIncludeDir, sMatchValue)))
                                    sCorrectPath = String.Format("{0}.inc", IO.Path.Combine(sIncludeDir, sMatchValue))
                                Case Else
                                    g_mFormMain.PrintInformation("[ERRO]", String.Format("Could not read include: {0}", sMatchValue))
                                    Continue While
                            End Select

                        Case Else
                            Continue While
                    End Select

                    If (Not IO.File.Exists(sCorrectPath)) Then
                        Continue While
                    End If

                    If (lPathList.Contains(sCorrectPath)) Then
                        Continue While
                    End If

                    lPathList.Add(sCorrectPath)
                End While
            End Using

            For i = 0 To lPathList.Count - 1
                GetIncludeFilesRecursive(lPathList(i), lList)
            Next
        End Sub
    End Class

    Public Class ClassDebuggerParser
        Private g_mFormMain As FormMain

        Public Shared g_sDebuggerFilesExt As String = ".bpdebug"

        Public Shared g_sBreakpointName As String = "BPDBreakpoint"
        Public Shared g_sDebuggerBreakpointIgnoreExt As String = ".ignore" & g_sDebuggerFilesExt 'If exist, the breakpoint is disabled
        Public Shared g_sDebuggerBreakpointTriggerExt As String = ".trigger" & g_sDebuggerFilesExt 'If exist, BasicPawn knows this breakpoint has been triggered
        Public Shared g_sDebuggerBreakpointContinueExt As String = ".continue" & g_sDebuggerFilesExt 'If exist, the breakpoint will continue
        Public Shared g_sDebuggerBreakpointContinueVarExt As String = ".continuev" & g_sDebuggerFilesExt 'If exist, the breakpoint will continue with its custom return value

        Public Shared g_sWatcherName As String = "BPDWatcher"
        Public Shared g_sDebuggerWatcherValueExt As String = ".value" & g_sDebuggerFilesExt

        Structure STRUC_DEBUGGER_ITEM
            Dim sGUID As String

            Dim iLine As Integer

            Dim iIndex As Integer
            Dim iLenght As Integer
            Dim iTotalLenght As Integer

            Dim iOffset As Integer

            Dim sArguments As String
            Dim sTotalFunction As String
        End Structure
        Public g_lBreakpointList As New List(Of STRUC_DEBUGGER_ITEM)
        Public g_lWatcherList As New List(Of STRUC_DEBUGGER_ITEM)

        Public Sub New(f As FormMain)
            g_mFormMain = f
        End Sub

        ''' <summary>
        ''' Updates the breakpoint list.
        ''' </summary>
        ''' <param name="sSource"></param>
        Public Sub UpdateBreakpoints(sSource As String, bKeepIdentity As Boolean)
            Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

            If (Not bKeepIdentity) Then
                g_lBreakpointList.Clear()
            End If

            Dim iListIndex As Integer = 0
            For Each m As Match In Regex.Matches(sSource, String.Format("\b{0}\b\s*(?<Lenght>)(?<Arguments>\(){1}", g_sBreakpointName, "{0,1}"))
                Dim iIndex As Integer = m.Index
                Dim bHasArgument As Boolean = m.Groups("Arguments").Success

                If (sourceAnalysis.InNonCode(iIndex) OrElse Not bHasArgument) Then
                    Continue For
                End If

                Dim iArgumentIndex As Integer = m.Groups("Arguments").Index

                Dim sGUID As String = Guid.NewGuid.ToString
                Dim iLine As Integer = sSource.Substring(0, m.Index).Split(New String() {vbLf}, 0).Length
                Dim iLineIndex As Integer = 0
                For i = iIndex - 1 To 0 Step -1
                    If (sSource(i) = vbLf) Then
                        Exit For
                    End If

                    iLineIndex += 1
                Next

                Dim iLenght As Integer = m.Groups("Lenght").Index - m.Index
                Dim iTotalLenght As Integer = 0
                Dim sArguments As New StringBuilder()
                Dim sTotalFunction As New StringBuilder

                Dim iStartLevel As Integer = sourceAnalysis.GetParenthesisLevel(iIndex)
                Dim bGetArguments As Boolean = False
                For i = iIndex To sSource.Length - 1
                    iTotalLenght += 1

                    sTotalFunction.Append(sSource(i))

                    If (sSource(i) = ")" AndAlso iStartLevel = sourceAnalysis.GetParenthesisLevel(i)) Then
                        bGetArguments = False
                        Exit For
                    End If

                    If (bGetArguments) Then
                        sArguments.Append(sSource(i))
                    End If

                    If (i = iArgumentIndex) Then
                        bGetArguments = True
                    End If
                Next

                If (iTotalLenght < 1) Then
                    Continue For
                End If

                If (bKeepIdentity) Then
                    Dim debuggerItem As New STRUC_DEBUGGER_ITEM
                    debuggerItem.sGUID = g_lBreakpointList(iListIndex).sGUID
                    debuggerItem.iLine = iLine
                    debuggerItem.iIndex = iLineIndex
                    debuggerItem.iLenght = iLenght
                    debuggerItem.iTotalLenght = iTotalLenght
                    debuggerItem.iOffset = iIndex
                    debuggerItem.sArguments = sArguments.ToString
                    debuggerItem.sTotalFunction = sTotalFunction.ToString

                    g_lBreakpointList(iListIndex) = debuggerItem
                Else
                    Dim debuggerItem As New STRUC_DEBUGGER_ITEM
                    debuggerItem.sGUID = sGUID
                    debuggerItem.iLine = iLine
                    debuggerItem.iIndex = iLineIndex
                    debuggerItem.iLenght = iLenght
                    debuggerItem.iTotalLenght = iTotalLenght
                    debuggerItem.iOffset = iIndex
                    debuggerItem.sArguments = sArguments.ToString
                    debuggerItem.sTotalFunction = sTotalFunction.ToString

                    g_lBreakpointList.Add(debuggerItem)
                End If

                iListIndex += 1
            Next
        End Sub

        ''' <summary>
        ''' Updates the breakpoint list.
        ''' </summary>
        ''' <param name="sSource"></param>
        Public Sub UpdateWatchers(sSource As String, bKeepIdentity As Boolean)
            Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

            If (Not bKeepIdentity) Then
                g_lWatcherList.Clear()
            End If

            Dim iListIndex As Integer = 0
            For Each m As Match In Regex.Matches(sSource, String.Format("\b{0}\b\s*(?<Lenght>)(?<Arguments>\(){1}", g_sWatcherName, "{0,1}"))
                Dim iIndex As Integer = m.Index
                Dim bHasArgument As Boolean = m.Groups("Arguments").Success

                If (sourceAnalysis.InNonCode(iIndex) OrElse Not bHasArgument) Then
                    Continue For
                End If

                Dim iArgumentIndex As Integer = m.Groups("Arguments").Index

                Dim sGUID As String = Guid.NewGuid.ToString
                Dim iLine As Integer = sSource.Substring(0, m.Index).Split(New String() {vbLf}, 0).Length
                Dim iLineIndex As Integer = 0
                For i = iIndex - 1 To 0 Step -1
                    If (sSource(i) = vbLf) Then
                        Exit For
                    End If

                    iLineIndex += 1
                Next

                Dim iLenght As Integer = m.Groups("Lenght").Index - m.Index
                Dim iTotalLenght As Integer = 0
                Dim sArguments As New StringBuilder()
                Dim sTotalFunction As New StringBuilder

                Dim iStartLevel As Integer = sourceAnalysis.GetParenthesisLevel(iIndex)
                Dim bGetArguments As Boolean = False
                For i = iIndex To sSource.Length - 1
                    iTotalLenght += 1

                    sTotalFunction.Append(sSource(i))

                    If (sSource(i) = ")" AndAlso iStartLevel = sourceAnalysis.GetParenthesisLevel(i)) Then
                        bGetArguments = False
                        Exit For
                    End If

                    If (bGetArguments) Then
                        sArguments.Append(sSource(i))
                    End If

                    If (i = iArgumentIndex) Then
                        bGetArguments = True
                    End If
                Next

                If (iTotalLenght < 1) Then
                    Continue For
                End If

                If (bKeepIdentity) Then
                    Dim breakpointItem As New STRUC_DEBUGGER_ITEM
                    breakpointItem.sGUID = g_lWatcherList(iListIndex).sGUID
                    breakpointItem.iLine = iLine
                    breakpointItem.iIndex = iLineIndex
                    breakpointItem.iLenght = iLenght
                    breakpointItem.iTotalLenght = iTotalLenght
                    breakpointItem.iOffset = iIndex
                    breakpointItem.sArguments = sArguments.ToString
                    breakpointItem.sTotalFunction = sTotalFunction.ToString

                    g_lWatcherList(iListIndex) = breakpointItem
                Else
                    Dim breakpointItem As New STRUC_DEBUGGER_ITEM
                    breakpointItem.sGUID = sGUID
                    breakpointItem.iLine = iLine
                    breakpointItem.iIndex = iLineIndex
                    breakpointItem.iLenght = iLenght
                    breakpointItem.iTotalLenght = iTotalLenght
                    breakpointItem.iOffset = iIndex
                    breakpointItem.sArguments = sArguments.ToString
                    breakpointItem.sTotalFunction = sTotalFunction.ToString

                    g_lWatcherList.Add(breakpointItem)
                End If

                iListIndex += 1
            Next
        End Sub

        ''' <summary>
        ''' Gets a list of usefull autocompletes
        ''' </summary>
        ''' <returns></returns>
        Public Function GetDebuggerAutocomplete() As STRUC_AUTOCOMPLETE()
            Dim autocompleteList As New List(Of STRUC_AUTOCOMPLETE)
            Dim autocompleteInfo As New StringBuilder

            autocompleteInfo.Length = 0
            autocompleteInfo.AppendLine("/**")
            autocompleteInfo.AppendLine("*  Pauses the plugin until manually resumed. Also shows the current position in the BasicPawn Debugger.")
            autocompleteInfo.AppendLine("*  Optionaly you can return a custom non-array value.")
            autocompleteInfo.AppendLine("*/")
            autocompleteList.Add(New STRUC_AUTOCOMPLETE With {
                                 .sFile = "BasicPawn.exe",
                                 .sFullFunctionName = String.Format("any:{0}(any:val=0)", g_sBreakpointName),
                                 .sFunctionName = g_sBreakpointName,
                                 .sInfo = autocompleteInfo.ToString,
                                 .mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEBUG})

            autocompleteInfo.Length = 0
            autocompleteInfo.AppendLine("/**")
            autocompleteInfo.AppendLine("*  Prints the passed value into the BasicPawn Debugger.")
            autocompleteInfo.AppendLine("*/")
            autocompleteList.Add(New STRUC_AUTOCOMPLETE With {
                                 .sFile = "BasicPawn.exe",
                                 .sFullFunctionName = String.Format("any:{0}(any:val=0)", g_sWatcherName),
                                 .sFunctionName = g_sWatcherName,
                                 .sInfo = autocompleteInfo.ToString,
                                 .mType = STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEBUG})

            Return autocompleteList.ToArray
        End Function

        Structure STRUC_SM_EXCEPTION_STACK_TRACE
            Dim iLine As Integer
            Dim sFileName As String
            Dim sFunctionName As String
            Dim bNativeFault As Boolean
        End Structure

        Structure STRUC_SM_EXCEPTION
            Dim sExceptionInfo As String
            Dim sBlamingFile As String
            Dim dLogDate As Date
            Dim mStackTrace As STRUC_SM_EXCEPTION_STACK_TRACE()
        End Structure

        Structure STRUC_SM_FATAL_EXCEPTION
            Dim sExceptionInfo As String
            Dim sBlamingFile As String
            Dim dLogDate As Date
            Dim sMiscInformation As Object()
        End Structure

        ''' <summary>
        ''' Reads all sourcemod exceptions from a log.
        ''' </summary>
        ''' <param name="sLogLines"></param>
        ''' <returns></returns>
        Public Function ReadSourceModLogExceptions(sLogLines As String()) As STRUC_SM_EXCEPTION()
            Dim iExpectingState As Integer = 0

            Dim smException As New STRUC_SM_EXCEPTION

            Dim smExceptionsList As New List(Of STRUC_SM_EXCEPTION)
            Dim smStackTraceList As New List(Of STRUC_SM_EXCEPTION_STACK_TRACE)

            For i = 0 To sLogLines.Length - 1
                Dim mExceptionInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+ \- [0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] Exception reported\:(?<Message>.*?)$")
                If (mExceptionInfo.Success) Then
                    If (iExpectingState = 3) Then
                        smException.mStackTrace = smStackTraceList.ToArray
                        smExceptionsList.Add(smException)

                        iExpectingState = 0
                    End If

                    smException = New STRUC_SM_EXCEPTION
                    smStackTraceList = New List(Of STRUC_SM_EXCEPTION_STACK_TRACE)

                    Dim sDate As String = mExceptionInfo.Groups("Date").Value
                    Dim sMessage As String = mExceptionInfo.Groups("Message").Value

                    Dim dDate As Date
                    If (Not Date.TryParseExact(sDate.Trim, "MM/dd/yyyy - HH:mm:ss", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, dDate)) Then
                        Continue For
                    End If

                    smException.sExceptionInfo = sMessage.Trim
                    smException.dLogDate = dDate

                    iExpectingState = 1
                    Continue For
                End If

                Select Case (iExpectingState)
                    Case 1 'Expecting: [SM] Blaming
                        Dim mBlamingInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] Blaming\:(?<File>.*?)$")
                        If (mBlamingInfo.Success) Then
                            Dim sFile As String = mBlamingInfo.Groups("File").Value

                            smException.sBlamingFile = sFile.Trim

                            iExpectingState = 2
                        Else
                            iExpectingState = 0
                        End If

                    Case 2 'Expecting: [SM] Call stack trace:
                        Dim mStackTraceInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] Call stack trace\:\s*$")
                        If (mStackTraceInfo.Success) Then
                            iExpectingState = 3
                        Else
                            iExpectingState = 0
                        End If

                    Case 3 'Expecting: [SM]   [0] ... 
                        Dim mMoreStackTraceInfo As Match = Regex.Match(sLogLines(i),
                                                                       "(" &
                                                                       "(?<PluginFault>^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\]   \[[0-9]+\] Line (?<Line>[0-9]+), (?<File>.*?)\:\:(?<Function>.*?)$)" &
                                                                       "|" &
                                                                       "(?<NativeFault>^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\]   \[[0-9]+\] (?<Function>.*?)$)" &
                                                                       ")")

                        Select Case (True)
                            Case mMoreStackTraceInfo.Groups("PluginFault").Success
                                Dim iLine As Integer = CInt(mMoreStackTraceInfo.Groups("Line").Value.Trim)
                                Dim sFile As String = mMoreStackTraceInfo.Groups("File").Value.Trim
                                Dim sFunction As String = mMoreStackTraceInfo.Groups("Function").Value.Trim

                                smStackTraceList.Add(New STRUC_SM_EXCEPTION_STACK_TRACE() With {
                                                     .iLine = iLine,
                                                     .sFileName = sFile,
                                                     .sFunctionName = sFunction,
                                                     .bNativeFault = False})

                            Case mMoreStackTraceInfo.Groups("NativeFault").Success
                                Dim sFunction As String = mMoreStackTraceInfo.Groups("Function").Value.Trim

                                smStackTraceList.Add(New STRUC_SM_EXCEPTION_STACK_TRACE() With {
                                                     .iLine = -1,
                                                     .sFileName = "",
                                                     .sFunctionName = sFunction,
                                                     .bNativeFault = True})

                            Case Else
                                smException.mStackTrace = smStackTraceList.ToArray
                                smExceptionsList.Add(smException)

                                iExpectingState = 0
                        End Select
                End Select
            Next

            Return smExceptionsList.ToArray
        End Function

        '''' <summary>
        '''' Reads all sourcemod exceptions from a fatal log.
        '''' </summary>
        '''' <param name="sLogLines"></param>
        '''' <returns></returns>
        'Public Function ReadSourceModLogMemoryLeaks(sLogLines As String()) As STRUC_SM_FATAL_EXCEPTION()
        '    Dim iExpectingState As Integer = 0

        '    Dim smException As New STRUC_SM_FATAL_EXCEPTION

        '    Dim smExceptionsList As New List(Of STRUC_SM_FATAL_EXCEPTION)
        '    Dim smStackTraceList As New List(Of Object())

        '    For i = 0 To sLogLines.Length - 1
        '        Dim mExceptionInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+ \- [0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] MEMORY LEAK DETECTED IN PLUGIN \(file ""(?<File>.*?)""\)$")
        '        If (mExceptionInfo.Success) Then
        '            If (iExpectingState = 3) Then
        '                smException.sMiscInformation = smStackTraceList.ToArray
        '                smExceptionsList.Add(smException)

        '                iExpectingState = 0
        '            End If

        '            smException = New STRUC_SM_FATAL_EXCEPTION
        '            smStackTraceList = New List(Of Object())

        '            Dim sDate As String = mExceptionInfo.Groups("Date").Value
        '            Dim sMessage As String = "Memory leak detected"
        '            Dim sFile As String = mExceptionInfo.Groups("File").Value

        '            Dim dDate As Date
        '            If (Not Date.TryParseExact(sDate.Trim, "MM/dd/yyyy - HH:mm:ss", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, dDate)) Then
        '                Continue For
        '            End If

        '            smException.sBlamingFile = sFile
        '            smException.sExceptionInfo = sMessage.Trim
        '            smException.dLogDate = dDate

        '            iExpectingState = 1
        '            Continue For
        '        End If

        '        Select Case (iExpectingState)
        '            Case 1 'Expecting: [SM] Blaming
        '                Dim mBlamingInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] Blaming\:(?<File>.*?)$")
        '                If (mBlamingInfo.Success) Then
        '                    Dim sFile As String = mBlamingInfo.Groups("File").Value

        '                    smException.sBlamingFile = sFile.Trim

        '                    iExpectingState = 2
        '                Else
        '                    iExpectingState = 0
        '                End If

        '            Case 2 'Expecting: [SM] Call stack trace:
        '                Dim mStackTraceInfo As Match = Regex.Match(sLogLines(i), "^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\] Call stack trace\:\s*$")
        '                If (mStackTraceInfo.Success) Then
        '                    iExpectingState = 3
        '                Else
        '                    iExpectingState = 0
        '                End If

        '            Case 3 'Expecting: [SM]   [0] ... 
        '                Dim mMoreStackTraceInfo As Match = Regex.Match(sLogLines(i),
        '                                                               "(" &
        '                                                               "(?<PluginFault>^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\]   \[[0-9]+\] Line (?<Line>[0-9]+), (?<File>.*?)\:\:(?<Function>.*?)$)" &
        '                                                               "|" &
        '                                                               "(?<NativeFault>^L (?<Date>[0-9]+\/[0-9]+\/[0-9]+) \- (?<Time>[0-9]+\:[0-9]+\:[0-9]+)\: \[SM\]   \[[0-9]+\] (?<Function>.*?)$)" &
        '                                                               ")")

        '                Select Case (True)
        '                    Case mMoreStackTraceInfo.Groups("PluginFault").Success
        '                        Dim iLine As Integer = CInt(mMoreStackTraceInfo.Groups("Line").Value.Trim)
        '                        Dim sFile As String = mMoreStackTraceInfo.Groups("File").Value.Trim
        '                        Dim sFunction As String = mMoreStackTraceInfo.Groups("Function").Value.Trim

        '                        smStackTraceList.Add(New STRUC_SM_EXCEPTION_STACK_TRACE() With {
        '                                             .iLine = iLine,
        '                                             .sFileName = sFile,
        '                                             .sFunctionName = sFunction,
        '                                             .bNativeFault = False})

        '                    Case mMoreStackTraceInfo.Groups("NativeFault").Success
        '                        Dim sFunction As String = mMoreStackTraceInfo.Groups("Function").Value.Trim

        '                        smStackTraceList.Add(New STRUC_SM_EXCEPTION_STACK_TRACE() With {
        '                                             .iLine = -1,
        '                                             .sFileName = "",
        '                                             .sFunctionName = sFunction,
        '                                             .bNativeFault = True})

        '                    Case Else
        '                        smException.mStackTrace = smStackTraceList.ToArray
        '                        smExceptionsList.Add(smException)

        '                        iExpectingState = 0
        '                End Select
        '        End Select
        '    Next

        '    Return smExceptionsList.ToArray
        'End Function

        Public Sub CleanupDebugPlaceholder(ByRef sSource As String)
            'TODO: Add more debug placeholder
            With New ClassBreakpoints(g_mFormMain)
                .RemoveAllBreakpoints(sSource)
            End With
            With New ClassWatchers(g_mFormMain)
                .RemoveAllWatchers(sSource)
            End With
        End Sub

        Public Sub CleanupDebugPlaceholder(mFormMain As FormMain)
            'TODO: Add more debug placeholder
            With New ClassBreakpoints(g_mFormMain)
                .TextEditorRemoveAllBreakpoints()
            End With
            With New ClassWatchers(g_mFormMain)
                .TextEditorRemoveAllWatchers()
            End With
        End Sub

        Public Function GetDebugPlaceholderNames() As String()
            'TODO: Add more debug placeholder
            Dim lNameList As New List(Of String)

            lNameList.Add(g_sBreakpointName)
            lNameList.Add(g_sWatcherName)

            Return lNameList.ToArray
        End Function

        Public Function HasDebugPlaceholder(sSource As String) As Boolean
            For Each sName As String In GetDebugPlaceholderNames()
                If (Regex.IsMatch(sSource, String.Format("\b{0}\b\s*\(", Regex.Escape(sName)))) Then
                    Return True
                End If
            Next

            Return False
        End Function

        ''' <summary>
        ''' SourceMod and BasicPawn communication.
        ''' </summary>
        Class ClassRunnerEngine
            Public g_sDebuggerRunnerGuid As String = Guid.NewGuid.ToString
            Public Shared g_sDebuggerRunnerCmdFileExt As String = ".cmd.bpdebug"
            Public Shared g_sDebuggerRunnerEntityFileExt As String = ".entities.bpdebug"

            ''' <summary>
            ''' Generates a engine source which can be used to accept commands, when its running.
            ''' </summary>
            ''' <param name="bNewSyntax"></param>
            ''' <returns></returns>
            Public Function GenerateRunnerEngine(bNewSyntax As Boolean) As String
                Dim SB As New StringBuilder

                If (bNewSyntax) Then
                    'TODO: Add new synrax engine
                    SB.AppendLine(My.Resources.Debugger_CommandRunnerEngineOld)
                Else
                    SB.AppendLine(My.Resources.Debugger_CommandRunnerEngineOld)
                End If

                SB.Replace("{IndentifierGUID}", g_sDebuggerRunnerGuid)

                Return SB.ToString
            End Function

            ''' <summary>
            ''' Sends a command to the engine plugin, when its running.
            ''' </summary>
            ''' <param name="sGamePath"></param>
            ''' <param name="sCmd"></param>
            Public Sub AcceptCommand(sGamePath As String, sCmd As String)
                Dim sFile As String = IO.Path.Combine(sGamePath, g_sDebuggerRunnerGuid & g_sDebuggerRunnerCmdFileExt)

                IO.File.AppendAllText(sFile, sCmd & Environment.NewLine)
            End Sub
        End Class

        Class ClassBreakpoints
            Private g_mFormMain As FormMain

            Public Sub New(f As FormMain)
                g_mFormMain = f
            End Sub


            ''' <summary>
            ''' Inserts one breakpoint using the caret position in the text editor
            ''' </summary>
            Public Sub TextEditorInsertBreakpointAtCaret()
                If (g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.HasSomethingSelected AndAlso g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0) Then
                    Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Offset
                    Dim iLenght As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Length

                    g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                    g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset + iLenght, ")")
                    g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}(", ClassDebuggerParser.g_sBreakpointName))
                    g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                Else
                    Dim sCaretWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True)

                    If (String.IsNullOrEmpty(sCaretWord)) Then
                        Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Caret.Offset
                        g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}();", ClassDebuggerParser.g_sBreakpointName))
                    Else
                        Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Caret.Offset

                        For Each m As Match In Regex.Matches(g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.TextContent, String.Format("(?<Word>\b{0}\b)(?<Function>\s*\(){1}", Regex.Escape(sCaretWord), "{0,1}"))
                            Dim iStartOffset As Integer = m.Groups("Word").Index
                            Dim iStartLen As Integer = m.Groups("Word").Value.Length
                            Dim bIsFunction As Boolean = m.Groups("Function").Success

                            If (iOffset < iStartOffset OrElse iOffset > (iStartOffset + iStartLen)) Then
                                Continue For
                            End If

                            If (bIsFunction) Then
                                Dim sSource As String = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.TextContent
                                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                                Dim iFullLenght As Integer = 0
                                Dim iStartLevel As Integer = sourceAnalysis.GetParenthesisLevel(iStartOffset)
                                For i = iStartOffset To sSource.Length - 1
                                    iFullLenght += 1

                                    If (sSource(i) = ")" AndAlso iStartLevel = sourceAnalysis.GetParenthesisLevel(i)) Then
                                        Exit For
                                    End If
                                Next

                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset + iFullLenght, ")")
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sBreakpointName))
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                            Else
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset + iStartLen, ")")
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sBreakpointName))
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                            End If
                        Next
                    End If
                End If

                g_mFormMain.TextEditorControl_Source.Refresh()
                g_mFormMain.PrintInformation("[INFO]", "A Breakpoint has been added!")
            End Sub

            ''' <summary>
            ''' Removes one breakpoint using the caret position in the text editor
            ''' </summary>
            Public Sub TextEditorRemoveBreakpointAtCaret()
                Dim sCaretWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True)

                If (sCaretWord <> ClassDebuggerParser.g_sBreakpointName) Then
                    g_mFormMain.PrintInformation("[ERROR]", "This is not a valid breakpoint!")
                    Return
                End If

                Dim iCaretOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Offset

                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

                Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
                debuggerParser.UpdateBreakpoints(g_mFormMain.TextEditorControl_Source.Document.TextContent, False)

                For i = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = debuggerParser.g_lBreakpointList(i).iOffset
                    Dim iLenght As Integer = debuggerParser.g_lBreakpointList(i).iLenght
                    Dim iTotalLenght As Integer = debuggerParser.g_lBreakpointList(i).iTotalLenght
                    Dim iLine As Integer = debuggerParser.g_lBreakpointList(i).iLine
                    Dim sFullFunction As String = debuggerParser.g_lBreakpointList(i).sArguments

                    If (iIndex > iCaretOffset OrElse (iIndex + iLenght) < iCaretOffset) Then
                        Continue For
                    End If

                    g_mFormMain.TextEditorControl_Source.Document.Replace(iIndex, iTotalLenght, sFullFunction)
                    If (g_mFormMain.TextEditorControl_Source.Document.TextLength > iIndex AndAlso g_mFormMain.TextEditorControl_Source.Document.TextContent(iIndex) = ";"c) Then
                        g_mFormMain.TextEditorControl_Source.Document.Remove(iIndex, 1)
                    End If

                    g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Breakpoint removed at line: {0}", iLine))

                    Exit For
                Next

                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                g_mFormMain.TextEditorControl_Source.Refresh()
            End Sub

            ''' <summary>
            ''' Removes all available breakpoints in the text editor
            ''' </summary>
            Public Sub TextEditorRemoveAllBreakpoints()
                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

                g_mFormMain.PrintInformation("[INFO]", "Removing all debugger breakpoints...")

                Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
                While True
                    debuggerParser.UpdateBreakpoints(g_mFormMain.TextEditorControl_Source.Document.TextContent, False)

                    For i = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                        Dim iIndex As Integer = debuggerParser.g_lBreakpointList(i).iOffset
                        Dim iLenght As Integer = debuggerParser.g_lBreakpointList(i).iLenght
                        Dim iTotalLenght As Integer = debuggerParser.g_lBreakpointList(i).iTotalLenght
                        Dim iLine As Integer = debuggerParser.g_lBreakpointList(i).iLine
                        Dim sFullFunction As String = debuggerParser.g_lBreakpointList(i).sArguments

                        g_mFormMain.TextEditorControl_Source.Document.Replace(iIndex, iTotalLenght, sFullFunction)
                        If (g_mFormMain.TextEditorControl_Source.Document.TextLength > iIndex AndAlso g_mFormMain.TextEditorControl_Source.Document.TextContent(iIndex) = ";"c) Then
                            g_mFormMain.TextEditorControl_Source.Document.Remove(iIndex, 1)
                        End If

                        g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Breakpoint removed at line: {0}", iLine))

                        Dim bDoRebuild As Boolean = False
                        For j = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                            If (i = j) Then
                                Continue For
                            End If

                            Dim jIndex As Integer = debuggerParser.g_lBreakpointList(j).iOffset
                            Dim jTotalLenght As Integer = debuggerParser.g_lBreakpointList(j).iTotalLenght
                            If (iIndex < jIndex OrElse iIndex > (jIndex + jTotalLenght)) Then
                                Continue For
                            End If

                            bDoRebuild = True
                            Exit For
                        Next

                        If (bDoRebuild) Then
                            Continue While
                        Else
                            Continue For
                        End If
                    Next

                    Exit While
                End While

                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                g_mFormMain.TextEditorControl_Source.Refresh()
                g_mFormMain.PrintInformation("[INFO]", "All debugger breakpoints removed!")
            End Sub

            ''' <summary>
            ''' Removes all available breakpoints in the source
            ''' </summary>
            ''' <param name="sSource"></param>
            Public Sub RemoveAllBreakpoints(ByRef sSource As String)
                Dim SB As New StringBuilder(sSource)

                Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
                While True
                    debuggerParser.UpdateBreakpoints(SB.ToString, False)

                    For i = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                        Dim iIndex As Integer = debuggerParser.g_lBreakpointList(i).iOffset
                        Dim iTotalLenght As Integer = debuggerParser.g_lBreakpointList(i).iTotalLenght
                        Dim sFullFunction As String = debuggerParser.g_lBreakpointList(i).sArguments

                        SB.Remove(iIndex, iTotalLenght)
                        SB.Insert(iIndex, sFullFunction)
                        If (SB.Length > iIndex AndAlso SB.Chars(iIndex) = ";"c) Then
                            SB.Remove(iIndex, 1)
                        End If

                        Dim bDoRebuild As Boolean = False
                        For j = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                            If (i = j) Then
                                Continue For
                            End If

                            Dim jIndex As Integer = debuggerParser.g_lBreakpointList(j).iOffset
                            Dim jTotalLenght As Integer = debuggerParser.g_lBreakpointList(j).iTotalLenght
                            If (iIndex < jIndex OrElse iIndex > (jIndex + jTotalLenght)) Then
                                Continue For
                            End If

                            bDoRebuild = True
                            Exit For
                        Next

                        If (bDoRebuild) Then
                            Continue While
                        Else
                            Continue For
                        End If
                    Next

                    Exit While
                End While

                sSource = SB.ToString
            End Sub

            Public Function GenerateModuleCode(sFunctionName As String, sIndentifierGUID As String, bNewSyntax As Boolean) As String
                Dim SB As New StringBuilder

                If (bNewSyntax) Then
                    SB.AppendLine(My.Resources.Debugger_BreakpointModuleNew)
                Else
                    SB.AppendLine(My.Resources.Debugger_BreakpointModuleOld)
                End If

                SB.Replace("{FunctionName}", sFunctionName)
                SB.Replace("{IndentifierGUID}", sIndentifierGUID)

                Return SB.ToString
            End Function

            Public Sub CompilerReady(ByRef sSource As String, debuggerParser As ClassDebuggerParser)
                Dim SB As New StringBuilder(sSource)
                Dim SBModules As New StringBuilder()

                Dim bForceNewSyntax As Boolean = (g_mFormMain.g_ClassSyntaxTools.HasNewDeclsPragma(sSource) <> -1)

                For i = debuggerParser.g_lBreakpointList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = debuggerParser.g_lBreakpointList(i).iOffset
                    Dim iLenght As Integer = debuggerParser.g_lBreakpointList(i).iLenght
                    Dim sGUID As String = debuggerParser.g_lBreakpointList(i).sGUID
                    Dim sNewName As String = g_sBreakpointName & sGUID.Replace("-", "")

                    SB.Remove(iIndex, iLenght)
                    SB.Insert(iIndex, sNewName)

                    SBModules.AppendLine(GenerateModuleCode(sNewName, sGUID, bForceNewSyntax))
                Next

                SB.AppendLine()
                SB.AppendLine(SBModules.ToString)

                sSource = SB.ToString
            End Sub
        End Class

        Class ClassWatchers
            Private g_mFormMain As FormMain

            Public Sub New(f As FormMain)
                g_mFormMain = f
            End Sub

            ''' <summary>
            ''' Inserts one watcher using the caret position in the text editor
            ''' </summary>
            Public Sub TextEditorInsertWatcherAtCaret()
                If (g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.HasSomethingSelected AndAlso g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count > 0) Then
                    Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Offset
                    Dim iLenght As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.SelectionManager.SelectionCollection(0).Length

                    g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                    g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset + iLenght, ")")
                    g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}(", ClassDebuggerParser.g_sWatcherName))
                    g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                Else
                    Dim sCaretWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True)

                    If (String.IsNullOrEmpty(sCaretWord)) Then
                        Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Caret.Offset
                        g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iOffset, String.Format("{0}();", ClassDebuggerParser.g_sWatcherName))
                    Else
                        Dim iOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Caret.Offset

                        For Each m As Match In Regex.Matches(g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.TextContent, String.Format("(?<Word>\b{0}\b)(?<Function>\s*\(){1}", Regex.Escape(sCaretWord), "{0,1}"))
                            Dim iStartOffset As Integer = m.Groups("Word").Index
                            Dim iStartLen As Integer = m.Groups("Word").Value.Length
                            Dim bIsFunction As Boolean = m.Groups("Function").Success

                            If (iOffset < iStartOffset OrElse iOffset > (iStartOffset + iStartLen)) Then
                                Continue For
                            End If

                            If (bIsFunction) Then
                                Dim sSource As String = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.TextContent
                                Dim sourceAnalysis As New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sSource)

                                Dim iFullLenght As Integer = 0
                                Dim iStartLevel As Integer = sourceAnalysis.GetParenthesisLevel(iStartOffset)
                                For i = iStartOffset To sSource.Length - 1
                                    iFullLenght += 1

                                    If (sSource(i) = ")" AndAlso iStartLevel = sourceAnalysis.GetParenthesisLevel(i)) Then
                                        Exit For
                                    End If
                                Next

                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset + iFullLenght, ")")
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sWatcherName))
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                            Else
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset + iStartLen, ")")
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.Insert(iStartOffset, String.Format("{0}(", ClassDebuggerParser.g_sWatcherName))
                                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()
                            End If
                        Next
                    End If
                End If

                g_mFormMain.TextEditorControl_Source.Refresh()
                g_mFormMain.PrintInformation("[INFO]", "A Watcher has been added!")
            End Sub

            ''' <summary>
            ''' Removes one watcher using the caret position in the text editor
            ''' </summary>
            Public Sub TextEditorRemoveWatcherAtCaret()
                Dim sCaretWord As String = g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True)

                If (sCaretWord <> ClassDebuggerParser.g_sWatcherName) Then
                    g_mFormMain.PrintInformation("[ERROR]", "This is not a valid watcher!")
                    Return
                End If

                Dim iCaretOffset As Integer = g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.TextArea.Caret.Offset

                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()

                Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
                debuggerParser.UpdateWatchers(g_mFormMain.TextEditorControl_Source.Document.TextContent, False)

                For i = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = debuggerParser.g_lWatcherList(i).iOffset
                    Dim iLenght As Integer = debuggerParser.g_lWatcherList(i).iLenght
                    Dim iTotalLenght As Integer = debuggerParser.g_lWatcherList(i).iTotalLenght
                    Dim iLine As Integer = debuggerParser.g_lWatcherList(i).iLine
                    Dim sFullFunction As String = debuggerParser.g_lWatcherList(i).sArguments

                    If (iIndex > iCaretOffset OrElse (iIndex + iLenght) < iCaretOffset) Then
                        Continue For
                    End If

                    g_mFormMain.TextEditorControl_Source.Document.Replace(iIndex, iTotalLenght, sFullFunction)
                    If (g_mFormMain.TextEditorControl_Source.Document.TextLength > iIndex AndAlso g_mFormMain.TextEditorControl_Source.Document.TextContent(iIndex) = ";"c) Then
                        g_mFormMain.TextEditorControl_Source.Document.Remove(iIndex, 1)
                    End If

                    g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Watcher removed at line: {0}", iLine))

                    Exit For
                Next

                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                g_mFormMain.TextEditorControl_Source.Refresh()
            End Sub

            ''' <summary>
            ''' Removes all available watchers in the text editor
            ''' </summary>
            Public Sub TextEditorRemoveAllWatchers()
                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.StartUndoGroup()


                g_mFormMain.PrintInformation("[INFO]", "Removing all debugger watcher...")

                Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
                While True
                    debuggerParser.UpdateWatchers(g_mFormMain.TextEditorControl_Source.Document.TextContent, False)

                    For i = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                        Dim iIndex As Integer = debuggerParser.g_lWatcherList(i).iOffset
                        Dim iLenght As Integer = debuggerParser.g_lWatcherList(i).iLenght
                        Dim iTotalLenght As Integer = debuggerParser.g_lWatcherList(i).iTotalLenght
                        Dim iLine As Integer = debuggerParser.g_lWatcherList(i).iLine
                        Dim sFullFunction As String = debuggerParser.g_lWatcherList(i).sArguments

                        g_mFormMain.TextEditorControl_Source.Document.Replace(iIndex, iTotalLenght, sFullFunction)
                        If (g_mFormMain.TextEditorControl_Source.Document.TextLength > iIndex AndAlso g_mFormMain.TextEditorControl_Source.Document.TextContent(iIndex) = ";"c) Then
                            g_mFormMain.TextEditorControl_Source.Document.Remove(iIndex, 1)
                        End If

                        g_mFormMain.PrintInformation("[INFO]", vbTab & String.Format("Watcher removed at line: {0}", iLine))

                        Dim bDoRebuild As Boolean = False
                        For j = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                            If (i = j) Then
                                Continue For
                            End If

                            Dim jIndex As Integer = debuggerParser.g_lWatcherList(j).iOffset
                            Dim jTotalLenght As Integer = debuggerParser.g_lWatcherList(j).iTotalLenght
                            If (iIndex < jIndex OrElse iIndex > (jIndex + jTotalLenght)) Then
                                Continue For
                            End If

                            bDoRebuild = True
                            Exit For
                        Next

                        If (bDoRebuild) Then
                            Continue While
                        Else
                            Continue For
                        End If
                    Next

                    Exit While
                End While

                g_mFormMain.TextEditorControl_Source.ActiveTextAreaControl.Document.UndoStack.EndUndoGroup()

                g_mFormMain.TextEditorControl_Source.Refresh()
                g_mFormMain.PrintInformation("[INFO]", "All debugger watchers removed!")
            End Sub

            ''' <summary>
            ''' Removes all available watchers in the source
            ''' </summary>
            ''' <param name="sSource"></param>
            Public Sub RemoveAllWatchers(ByRef sSource As String)
                Dim SB As New StringBuilder(sSource)

                Dim debuggerParser As New ClassDebuggerParser(g_mFormMain)
                While True
                    debuggerParser.UpdateWatchers(SB.ToString, False)

                    For i = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                        Dim iIndex As Integer = debuggerParser.g_lWatcherList(i).iOffset
                        Dim iTotalLenght As Integer = debuggerParser.g_lWatcherList(i).iTotalLenght
                        Dim sFullFunction As String = debuggerParser.g_lWatcherList(i).sArguments

                        SB.Remove(iIndex, iTotalLenght)
                        SB.Insert(iIndex, sFullFunction)
                        If (SB.Length > iIndex AndAlso SB.Chars(iIndex) = ";"c) Then
                            SB.Remove(iIndex, 1)
                        End If

                        Dim bDoRebuild As Boolean = False
                        For j = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                            If (i = j) Then
                                Continue For
                            End If

                            Dim jIndex As Integer = debuggerParser.g_lWatcherList(j).iOffset
                            Dim jTotalLenght As Integer = debuggerParser.g_lWatcherList(j).iTotalLenght
                            If (iIndex < jIndex OrElse iIndex > (jIndex + jTotalLenght)) Then
                                Continue For
                            End If

                            bDoRebuild = True
                            Exit For
                        Next

                        If (bDoRebuild) Then
                            Continue While
                        Else
                            Continue For
                        End If
                    Next

                    Exit While
                End While

                sSource = SB.ToString
            End Sub

            Public Function GenerateModuleCode(sFunctionName As String, sIndentifierGUID As String, bNewSyntax As Boolean) As String
                Dim SB As New StringBuilder

                If (bNewSyntax) Then
                    SB.AppendLine(My.Resources.Debugger_WatcherModuleNew)
                Else
                    SB.AppendLine(My.Resources.Debugger_WatcherModuleOld)
                End If

                SB.Replace("{FunctionName}", sFunctionName)
                SB.Replace("{IndentifierGUID}", sIndentifierGUID)

                Return SB.ToString
            End Function

            Public Sub CompilerReady(ByRef sSource As String, debuggerParser As ClassDebuggerParser)
                Dim SB As New StringBuilder(sSource)
                Dim SBModules As New StringBuilder()

                Dim bForceNewSyntax As Boolean = (g_mFormMain.g_ClassSyntaxTools.HasNewDeclsPragma(sSource) <> -1)

                For i = debuggerParser.g_lWatcherList.Count - 1 To 0 Step -1
                    Dim iIndex As Integer = debuggerParser.g_lWatcherList(i).iOffset
                    Dim iLenght As Integer = debuggerParser.g_lWatcherList(i).iLenght
                    Dim sGUID As String = debuggerParser.g_lWatcherList(i).sGUID
                    Dim sNewName As String = g_sWatcherName & sGUID.Replace("-", "")

                    SB.Remove(iIndex, iLenght)
                    SB.Insert(iIndex, sNewName)

                    SBModules.AppendLine(GenerateModuleCode(sNewName, sGUID, bForceNewSyntax))
                Next

                SB.AppendLine()
                SB.AppendLine(SBModules.ToString)

                sSource = SB.ToString
            End Sub
        End Class
    End Class



    Private Sub ToolStripMenuItem_DebuggerBreakpointInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointInsert.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorInsertBreakpointAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointRemove.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorRemoveBreakpointAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointRemoveAll.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorRemoveAllBreakpoints()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherInsert.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorInsertWatcherAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherRemove.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorRemoveWatcherAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherRemoveAll.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorRemoveAllWatchers()
        End With
    End Sub


    Private Sub FormMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If (g_mFormDebugger IsNot Nothing AndAlso Not g_mFormDebugger.IsDisposed) Then
            MessageBox.Show("You can't close BasicPawn while debugging!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            e.Cancel = True
        End If
    End Sub

    Private Sub ToolStripMenuItem_CheckUpdate_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CheckUpdate.Click
        Try
            Process.Start("https://github.com/Timocop/BasicPawn/releases")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
