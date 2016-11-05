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
Imports ICSharpCode.TextEditor

Public Class UCInformationList
    Private g_mFormMain As FormMain

    Public Sub New(f As FormMain)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormMain = f
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox_Information.SelectedIndexChanged
        If (ListBox_Information.SelectedItems.Count < 1) Then
            Return
        End If

        If (String.IsNullOrEmpty(ClassSettings.g_sConfigOpenSourcePawnFile) OrElse Not IO.File.Exists(ClassSettings.g_sConfigOpenSourcePawnFile)) Then
            Return
        End If

        Dim reMatch As Match = Regex.Match(ListBox_Information.SelectedItems(0), String.Format("\b{0}\b\((?<Line>[0-9]+)\)\s\:", Regex.Escape(ClassSettings.g_sConfigOpenSourcePawnFile)), RegexOptions.IgnoreCase)
        If (reMatch.Success) Then
            Dim iLineNum As Integer = CInt(reMatch.Groups("Line").Value) - 1
            If (iLineNum < 0 OrElse iLineNum > g_mFormMain.TextEditorControl1.Document.TotalNumberOfLines - 1) Then
                Return
            End If

            g_mFormMain.TextEditorControl1.ActiveTextAreaControl.Caret.Line = iLineNum
            g_mFormMain.TextEditorControl1.ActiveTextAreaControl.SelectionManager.ClearSelection()

            Dim iLineLen As Integer = g_mFormMain.TextEditorControl1.Document.GetLineSegment(iLineNum).Length

            Dim iStart As New TextLocation(0, iLineNum)
            Dim iEnd As New TextLocation(iLineLen, iLineNum)

            g_mFormMain.TextEditorControl1.ActiveTextAreaControl.SelectionManager.SetSelection(iStart, iEnd)
        End If
    End Sub
End Class
