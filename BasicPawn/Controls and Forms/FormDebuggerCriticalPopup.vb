Public Class FormDebuggerCriticalPopup
    Private g_mFormDebugger As FormDebugger

    Public Sub New(mFormDebugger As FormDebugger, sTitle As String, sHeaderTitle As String, sText As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormDebugger = mFormDebugger

        Me.Text = sTitle
        Label_Title.Text = sHeaderTitle
        TextBox_Text.Text = sText
    End Sub

    Private Sub Button_Close_Click(sender As Object, e As EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    Private Sub Button_Continue_Click(sender As Object, e As EventArgs) Handles Button_Continue.Click
        g_mFormDebugger.g_ClassDebuggerRunner.ContinueDebugging()
        Me.Close()
    End Sub
End Class