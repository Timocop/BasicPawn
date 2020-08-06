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



Public Class ClassTextMinimap
    Private g_mFormMain As FormMain
    Private g_mPanel As Panel

    Structure STRUC_FOLDMARKER_INFO
        Dim iOffset As Integer
        Dim iLength As Integer
        Dim bIsFolded As Boolean

        Sub New(_Offset As Integer, _Length As Integer, _IsFolded As Boolean)
            iOffset = _Offset
            iLength = _Length
            bIsFolded = _IsFolded
        End Sub
    End Structure

    Private g_sLastText As String = ""
    Private g_lLastFoldings As New List(Of STRUC_FOLDMARKER_INFO)

    Private g_bRequestUpdate As Boolean = False

    Const VIEW_FONT_SIZE = 1
    Const VIEW_WIDTH_OFFSET = 2

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.   
        g_mPanel = New Panel With {
            .Parent = RichTextBoxEx_Minimap,
            .BorderStyle = BorderStyle.None,
            .BackColor = Color.CornflowerBlue,
            .Location = New Point(0, 0),
            .Size = New Size(0, 0)
        }
        g_mPanel.Show()
        g_mPanel.BringToFront()

        ClassControlStyle.SetNameFlag(g_mPanel, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_KEEP_COLOR)

        AddHandler g_mPanel.MouseClick, AddressOf Panel_MouseClick
        AddHandler g_mPanel.MouseMove, AddressOf Panel_MouseMove

        RichTextBoxEx_Minimap.SuspendLayout()
        RichTextBoxEx_Minimap.Dock = DockStyle.None
        RichTextBoxEx_Minimap.Location = New Point(VIEW_WIDTH_OFFSET, 0)
        RichTextBoxEx_Minimap.Font = New Font(RichTextBoxEx_Minimap.Font.FontFamily, VIEW_FONT_SIZE, FontStyle.Bold)
        RichTextBoxEx_Minimap.ResumeLayout()
    End Sub

    Public Sub UpdateText(bRefresh As Boolean)
        If (Not Me.Visible) Then
            Return
        End If

        Dim mActiveTab = g_mFormMain.g_ClassTabControl.m_ActiveTab
        Dim mFoldingManager = mActiveTab.m_TextEditor.Document.FoldingManager

        Dim bChanged As Boolean = False
        While True
            'RichTextBox.Text != Document.TextContent?!
            'Workaround: Cache last TextConent in |g_sLastTexteditorText| instead.
            If (mActiveTab.m_TextEditor.Document.TextContent <> g_sLastText) Then
                bChanged = True
                Exit While
            End If

            If (True) Then
                If (g_lLastFoldings.Count <> mFoldingManager.FoldMarker.Count) Then
                    bChanged = True
                    Exit While
                End If

                For i = 0 To g_lLastFoldings.Count - 1
                    If (g_lLastFoldings(i).iOffset = mFoldingManager.FoldMarker(i).Offset AndAlso
                            g_lLastFoldings(i).iLength = mFoldingManager.FoldMarker(i).Length AndAlso
                            g_lLastFoldings(i).bIsFolded = mFoldingManager.FoldMarker(i).IsFolded) Then
                        Continue For
                    End If

                    bChanged = True
                    Exit While
                Next
            End If

            Exit While
        End While

        If (bRefresh OrElse g_bRequestUpdate OrElse bChanged) Then
            g_bRequestUpdate = False
            g_sLastText = mActiveTab.m_TextEditor.Document.TextContent

            g_lLastFoldings.Clear()
            For Each mFold In mFoldingManager.FoldMarker
                g_lLastFoldings.Add(New STRUC_FOLDMARKER_INFO(mFold.Offset, mFold.Length, mFold.IsFolded))
            Next


            Dim bFoldedIndexes As Boolean() = New Boolean(g_sLastText.Length - 1) {}
            For Each mFold In mFoldingManager.FoldMarker
                If (Not mFold.IsFolded) Then
                    Continue For
                End If

                For i = mFold.Offset To Math.Min(mFold.Offset + mFold.Length - 1, bFoldedIndexes.Length - 1)
                    bFoldedIndexes(i) = True
                Next
            Next

            RichTextBoxEx_Minimap.SuspendLayout()

            Dim mMapBuilder As New Text.StringBuilder(g_sLastText.Length)
            For i = 0 To g_sLastText.Length - 1
                If (bFoldedIndexes(i)) Then
                    Continue For
                End If

                If (AscW(g_sLastText(i)) < 0 OrElse AscW(g_sLastText(i)) > 127) Then
                    mMapBuilder.Append("?"c)
                    Continue For
                End If

                If (g_sLastText(i) = vbTab(0)) Then
                    mMapBuilder.Append(New String(" "c, 4))
                    Continue For
                End If

                mMapBuilder.Append(g_sLastText(i))
            Next

            'Replace unprintable - unicode - characters. For some reason it makes the RichTextBox font bold.
            RichTextBoxEx_Minimap.Text = mMapBuilder.ToString

            RichTextBoxEx_Minimap.ResumeLayout()
        End If
    End Sub

    Public Sub RequestUpdate()
        g_bRequestUpdate = True
    End Sub

    Public Sub UpdatePosition(bUpdateScrolling As Boolean, bUpdateView As Boolean, bAutoScrollToView As Boolean)
        If (Not Me.Visible) Then
            Return
        End If

        Dim mActiveTab = g_mFormMain.g_ClassTabControl.m_ActiveTab

        Dim iTotalLines = mActiveTab.m_TextEditor.Document.TotalNumberOfLines
        If (iTotalLines < 1) Then
            Return
        End If

        Dim bFoldedLines As Boolean() = New Boolean(iTotalLines - 1) {}
        For Each mFold In mActiveTab.m_TextEditor.Document.FoldingManager.FoldMarker
            If (Not mFold.IsFolded) Then
                Continue For
            End If

            For i = mFold.StartLine To mFold.EndLine - 1
                bFoldedLines(i) = True
            Next
        Next

        Dim iVisibleLines As Integer = 1
        For i = 1 To iTotalLines
            If (bFoldedLines(i - 1)) Then
                Continue For
            End If

            iVisibleLines += 1
        Next

        Dim iPointLabelY As Integer = RichTextBoxEx_Minimap.PointToScreen(Point.Empty).Y
        Dim iPointLabelSizeY As Integer = RichTextBoxEx_Minimap.PointToScreen(New Point(RichTextBoxEx_Minimap.Size.Width, RichTextBoxEx_Minimap.Size.Height)).Y
        Dim iPointLabelLineSize As Integer = CInt((iPointLabelSizeY - iPointLabelY) / iVisibleLines)
        If (iPointLabelLineSize < 1) Then
            Return
        End If

        Dim iPointLabelLocalY As Integer = (Cursor.Position.Y - RichTextBoxEx_Minimap.PointToScreen(Point.Empty).Y)
        Dim iSelectedLine As Integer = Math.Min(CInt(iPointLabelLocalY / iPointLabelLineSize), iVisibleLines)
        If (iSelectedLine < 0) Then
            Return
        End If

        If (bUpdateScrolling) Then
            'Update Scroll
            For i = 1 To iTotalLines
                If (i > iSelectedLine) Then
                    Exit For
                End If

                If (Not bFoldedLines(i - 1)) Then
                    Continue For
                End If

                iSelectedLine += 1
            Next

            mActiveTab.m_TextEditor.ActiveTextAreaControl.CenterViewOn(iSelectedLine, 0)
        End If

        If (bUpdateView) Then
            Dim mTextView = mActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.TextView

            'Update Alpha View
            g_mPanel.SuspendLayout()
            g_mPanel.Location = New Point(0, mTextView.FirstPhysicalLine * iPointLabelLineSize)
            g_mPanel.Size = New Size(VIEW_WIDTH_OFFSET, mTextView.VisibleLineCount * iPointLabelLineSize)
            g_mPanel.ResumeLayout()

            If (bAutoScrollToView) Then
                Me.ScrollControlIntoView(g_mPanel)
            End If

            RichTextBoxEx_Minimap.Update()
        End If
    End Sub

    Private Sub Panel_MouseClick(sender As Object, e As MouseEventArgs)
        If (e.Button <> MouseButtons.Left) Then
            Return
        End If

        UpdatePosition(True, True, False)
    End Sub

    Private Sub Panel_MouseMove(sender As Object, e As MouseEventArgs)
        If (e.Button <> MouseButtons.Left) Then
            Return
        End If

        UpdatePosition(True, True, False)
    End Sub

    Private Sub RichTextBoxEx_Minimap_MouseMove(sender As Object, e As MouseEventArgs) Handles RichTextBoxEx_Minimap.MouseMove
        If (e.Button <> MouseButtons.Left) Then
            Return
        End If

        UpdatePosition(True, True, False)
    End Sub

    Private Sub RichTextBoxEx_Minimap_MouseClick(sender As Object, e As MouseEventArgs) Handles RichTextBoxEx_Minimap.MouseClick
        If (e.Button <> MouseButtons.Left) Then
            Return
        End If

        UpdatePosition(True, True, False)
    End Sub

    Private Sub ClassTextMinimap_Scroll(sender As Object, e As ScrollEventArgs) Handles Me.Scroll
        g_mPanel.Update()
    End Sub

    Private Sub ClassTextMinimap_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel
        g_mPanel.Update()
    End Sub

    Private Sub RichTextBoxEx_Minimap_ContentsResized(sender As Object, e As ContentsResizedEventArgs) Handles RichTextBoxEx_Minimap.ContentsResized
        RichTextBoxEx_Minimap.SuspendLayout()

        RichTextBoxEx_Minimap.Width = e.NewRectangle.Width
        RichTextBoxEx_Minimap.Height = e.NewRectangle.Height

        RichTextBoxEx_Minimap.ResumeLayout()
    End Sub

    Private Sub CleanUp()
        'Remove Handlers
        If (g_mPanel IsNot Nothing) Then
            RemoveHandler g_mPanel.MouseClick, AddressOf Panel_MouseClick
            RemoveHandler g_mPanel.MouseMove, AddressOf Panel_MouseMove
        End If
    End Sub
End Class
