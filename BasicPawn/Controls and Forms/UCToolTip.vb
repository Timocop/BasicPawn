Public Class UCToolTip
    Private g_mFormMain As FormMain

    Public Sub New(f As FormMain)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormMain = f

        TextEditorControl_ToolTip.IsReadOnly = True
    End Sub
End Class
