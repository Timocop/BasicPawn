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


Imports System.Text.RegularExpressions

Public Class ClassTextMinimap
    Private g_mFormMain As FormMain
    Private g_sLastText As String = ""
    Private g_mPanel As Panel

    Const VIEW_FONT_SIZE = 1
    Const VIEW_WIDTH_OFFSET = 2

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.   
        g_mPanel = New Panel With {
            .Name = "@KeepForeBackColor",
            .Parent = RichTextBoxEx_Minimap,
            .BorderStyle = BorderStyle.None,
            .BackColor = Color.CornflowerBlue,
            .Location = New Point(0, 0),
            .Size = New Size(0, 0)
        }
        g_mPanel.Show()
        g_mPanel.BringToFront()

        AddHandler g_mPanel.MouseClick, AddressOf Panel_MouseClick
        AddHandler g_mPanel.MouseMove, AddressOf Panel_MouseMove

        RichTextBoxEx_Minimap.SuspendLayout()
        RichTextBoxEx_Minimap.Dock = DockStyle.None
        RichTextBoxEx_Minimap.Location = New Point(VIEW_WIDTH_OFFSET, 0)
        RichTextBoxEx_Minimap.Font = New Font(RichTextBoxEx_Minimap.Font.FontFamily, VIEW_FONT_SIZE, FontStyle.Bold)
        RichTextBoxEx_Minimap.ResumeLayout()
    End Sub

    Public Sub UpdateText()
        If (Not Me.Visible) Then
            Return
        End If

        'RichTextBox.Text != Document.TextContent?!
        'Workaround: Cache last TextConent in |g_sLastTexteditorText| instead.
        If (g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent <> g_sLastText) Then
            g_sLastText = g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent

            RichTextBoxEx_Minimap.SuspendLayout()

            'Replace unprintable - unicode - characters. For some reason it makes the RichTextBox font bold.
            RichTextBoxEx_Minimap.Text = Regex.Replace(g_sLastText, "[^\u0000-\u007F]+", "?").Replace(vbTab(0), New String(" "c, 4))

            RichTextBoxEx_Minimap.ResumeLayout()
        End If
    End Sub

    Public Sub UpdatePosition(bUpdateScrolling As Boolean, bUpdateView As Boolean, bAutoScrollToView As Boolean)
        If (Not Me.Visible) Then
            Return
        End If

        If (g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TotalNumberOfLines < 1) Then
            Return
        End If

        Dim iPointLabelY As Integer = RichTextBoxEx_Minimap.PointToScreen(Point.Empty).Y
        Dim iPointLabelSizeY As Integer = RichTextBoxEx_Minimap.PointToScreen(New Point(RichTextBoxEx_Minimap.Size.Width, RichTextBoxEx_Minimap.Size.Height)).Y
        Dim miointLabelLineSize As Integer = CInt((iPointLabelSizeY - iPointLabelY) / g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TotalNumberOfLines)
        If (miointLabelLineSize < 1) Then
            Return
        End If

        Dim iPointLabelLocalY As Integer = (Cursor.Position.Y - RichTextBoxEx_Minimap.PointToScreen(Point.Empty).Y)
        Dim iSelectedLine As Integer = CInt(iPointLabelLocalY / miointLabelLineSize)
        If (iSelectedLine < 0) Then
            Return
        End If

        If (bUpdateScrolling) Then
            'Update Scroll
            g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.CenterViewOn(iSelectedLine, 0)
        End If

        If (bUpdateView) Then
            'Update Alpha View
            g_mPanel.SuspendLayout()
            g_mPanel.Location = New Point(0, CInt(g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.TextView.FirstPhysicalLine * miointLabelLineSize))
            g_mPanel.Size = New Size(VIEW_WIDTH_OFFSET, CInt(g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.TextView.VisibleLineCount * miointLabelLineSize))
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
        If (g_mPanel IsNot Nothing AndAlso Not g_mPanel.IsDisposed) Then
            RemoveHandler g_mPanel.MouseClick, AddressOf Panel_MouseClick
            RemoveHandler g_mPanel.MouseMove, AddressOf Panel_MouseMove
        End If
    End Sub
End Class
