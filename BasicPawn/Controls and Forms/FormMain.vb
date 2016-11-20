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

    Public g_cDarkTextEditorBackgroundColor As Color = Color.FromArgb(255, 26, 26, 26)
    Public g_cDarkFormDetailsBackgroundColor As Color = Color.FromArgb(255, 24, 24, 24)
    Public g_cDarkFormBackgroundColor As Color = Color.FromArgb(255, 48, 48, 48)
    Public g_cDarkFormMenuBackgroundColor As Color = Color.FromArgb(255, 64, 64, 64)

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

    Private g_bCodeChanged As Boolean = False

    Public Property m_CodeChanged As Boolean
        Get
            Return g_bCodeChanged
        End Get
        Set(value As Boolean)
            g_bCodeChanged = value
            UpdateFormTitle()
        End Set
    End Property

    Public Sub UpdateFormTitle()
        If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenedSourceFile)) Then
            Me.Text = String.Format("{0} ({1}){2}", Application.ProductName, "Unnamed", If(g_bCodeChanged, "*"c, ""))
        Else
            Me.Text = String.Format("{0} ({1}){2}", Application.ProductName, IO.Path.GetFileName(ClassSettings.g_sConfigOpenedSourceFile), If(g_bCodeChanged, "*"c, ""))
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

#Region "Syntax Stuff"


    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        UpdateFormTitle()
    End Sub

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

        ClassSettings.g_sConfigOpenedSourceFile = ""
        TextEditorControl_Source.Document.TextContent = ""
        TextEditorControl_Source.Refresh()

        g_bCodeChanged = False
        UpdateFormTitle()
        g_ClassLineState.ClearStates()

        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

        PrintInformation("[INFO]", "User created a new source file")
    End Sub

    Private Sub ToolStripMenuItem_FileOpen_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileOpen.Click
        Using i As New OpenFileDialog
            i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|Pawn (Not fully supported)|*.pwn;*.p|AMX Mod X|*.sma|All files|*.*"
            i.FileName = ClassSettings.g_sConfigOpenedSourceFile
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
            i.FileName = ClassSettings.g_sConfigOpenedSourceFile

            If (i.ShowDialog = DialogResult.OK) Then
                ClassSettings.g_sConfigOpenedSourceFile = i.FileName

                g_bCodeChanged = False
                UpdateFormTitle()
                g_ClassLineState.SaveStates()

                PrintInformation("[INFO]", "User saved file to: " & ClassSettings.g_sConfigOpenedSourceFile)
                IO.File.WriteAllText(ClassSettings.g_sConfigOpenedSourceFile, TextEditorControl_Source.Document.TextContent)
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAsTemp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAsTemp.Click
        Dim sTempFile As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))

        ClassSettings.g_sConfigOpenedSourceFile = sTempFile

        g_bCodeChanged = False
        UpdateFormTitle()
        g_ClassLineState.SaveStates()

        PrintInformation("[INFO]", "User saved file to: " & ClassSettings.g_sConfigOpenedSourceFile)
        IO.File.WriteAllText(ClassSettings.g_sConfigOpenedSourceFile, TextEditorControl_Source.Document.TextContent)
    End Sub


    Private Sub ToolStripMenuItem_FileExit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileExit.Click
        Me.Close()
    End Sub
#End Region

#Region "MenuStrip_Tools"
    Private Sub ToolStripMenuItem_ToolsSettingsAndConfigs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsSettingsAndConfigs.Click
        Using i As New FormSettings(Me)
            If (i.ShowDialog() = DialogResult.OK) Then
                UpdateFormTitle()

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
        Using i As New FormSettings(Me)
            i.TabControl1.SelectTab(1)
            If (i.ShowDialog() = DialogResult.OK) Then
                UpdateFormTitle()

                g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

                TextEditorControl_Source.ActiveTextAreaControl.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
                TextEditorControl_Source.Refresh()

                g_ClassSyntaxTools.UpdateFormColors()
            End If
        End Using
    End Sub

#End Region


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

    Private Sub ToolStripMenuItem_FileOpenFolder_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileOpenFolder.Click
        Try
            If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenedSourceFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenedSourceFile)) Then
                MessageBox.Show("Can't open current folder. Source file can't be found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Process.Start("explorer.exe", "/select,""" & ClassSettings.g_sConfigOpenedSourceFile & """")
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
