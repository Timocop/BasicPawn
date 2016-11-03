Public Class FormDebuggerStop
    Enum ENUM_DIALOG_RESULT
        DO_NOTHING
        TERMINATE_GAME
        RESTART_GAME
        UNLOAD_PLUGIN
    End Enum

    Private g_mDialogResult As ENUM_DIALOG_RESULT = ENUM_DIALOG_RESULT.DO_NOTHING

    Public ReadOnly Property m_DialogResult As ENUM_DIALOG_RESULT
        Get
            Return g_mDialogResult
        End Get
    End Property


    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        g_mDialogResult = ENUM_DIALOG_RESULT.DO_NOTHING
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        g_mDialogResult = ENUM_DIALOG_RESULT.TERMINATE_GAME
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        g_mDialogResult = ENUM_DIALOG_RESULT.RESTART_GAME
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged
        g_mDialogResult = ENUM_DIALOG_RESULT.UNLOAD_PLUGIN
    End Sub
End Class