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


Public Class FormDebuggerException
    Private g_mFormDebugger As FormDebugger
    Private g_sLogFile As String = ""

    Private Const MAX_LINES_TO_LIST = 100

    Public Sub New(mFormDebugger As FormDebugger, sLogFile As String, smException As ClassDebuggerParser.STRUC_SM_EXCEPTION)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormDebugger = mFormDebugger
        g_sLogFile = sLogFile

        Label_ExceptionInfo.Text = "Exception: " & smException.sExceptionInfo
        Label_Date.Text = "Date: " & smException.dLogDate.ToString

        ListView_StackTrace.BeginUpdate()
        Dim iIndex As Integer = 0
        For Each stackTrace In smException.mStackTrace
            Dim iRealLine As Integer = -1
            Dim sFile As String = ""

            If (stackTrace.iLine > -1 AndAlso stackTrace.iLine < g_mFormDebugger.g_ClassDebuggerRunner.g_mSourceLinesInfo.Length) Then
                Dim info = g_mFormDebugger.g_ClassDebuggerRunner.g_mSourceLinesInfo(stackTrace.iLine)
                iRealLine = info.iRealLine - 1
                sFile = info.sFile
            End If

            If (stackTrace.sFileName = g_mFormDebugger.g_ClassDebuggerRunner.m_sPluginIdentity) Then
                ListView_StackTrace.Items.Add(New ListViewItem(New String() {iIndex.ToString, stackTrace.iLine.ToString, iRealLine.ToString, sFile, stackTrace.sFunctionName}))
            Else
                ListView_StackTrace.Items.Add(New ListViewItem(New String() {iIndex.ToString, "-1", stackTrace.iLine.ToString, stackTrace.sFileName, stackTrace.sFunctionName}))
            End If

            iIndex += 1
        Next
        ListView_StackTrace.EndUpdate()
    End Sub

    Private Sub ListView_StackTrace_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_StackTrace.SelectedIndexChanged
        MarkListViewItem()
    End Sub

    Private Sub ListView_StackTrace_MouseClick(sender As Object, e As MouseEventArgs) Handles ListView_StackTrace.MouseClick
        MarkListViewItem()
    End Sub

    Private Sub MarkListViewItem()
        Try
            If (ListView_StackTrace.SelectedItems.Count < 1) Then
                Return
            End If

            Dim iDebugLine As Integer = CInt(ListView_StackTrace.SelectedItems(0).SubItems(1).Text)
            iDebugLine -= 1

            If (iDebugLine < 0) Then
                Return
            End If

            Dim iLineLen As Integer = g_mFormDebugger.TextEditorControlEx_DebuggerSource.Document.GetLineSegment(iDebugLine).Length

            Dim startLocation As New ICSharpCode.TextEditor.TextLocation(0, iDebugLine)
            Dim endLocation As New ICSharpCode.TextEditor.TextLocation(iLineLen, iDebugLine)

            g_mFormDebugger.TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.Caret.Position = startLocation
            g_mFormDebugger.TextEditorControlEx_DebuggerSource.ActiveTextAreaControl.SelectionManager.SetSelection(startLocation, endLocation)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button_ViewLog_Click(sender As Object, e As EventArgs) Handles Button_ViewLog.Click
        Try
            Dim sLines As String() = ClassTools.ClassStrings.StringReadLinesEnd(g_sLogFile, MAX_LINES_TO_LIST)

            Using i As New FormDebuggerCriticalPopup(g_mFormDebugger, "SourceMod Exception Log", "Latest exceptions from the SourceMod log", String.Join(Environment.NewLine, sLines))
                i.ShowDialog()
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub Button_Close_Click(sender As Object, e As EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    Private Sub Button_Continue_Click(sender As Object, e As EventArgs) Handles Button_Continue.Click
        g_mFormDebugger.g_ClassDebuggerRunner.ContinueDebugging()
        Me.Close()
    End Sub
End Class