Imports ICSharpCode.TextEditor

Public Class TextEditorControlEx
    Inherits TextEditorControl

    Public Event g_eProcessCmdKey(ByRef bBlock As Boolean, ByRef iMsg As Message, iKeys As Keys)

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, e As Keys) As Boolean
        Dim bBLock As Boolean = False

        RaiseEvent g_eProcessCmdKey(bBLock, msg, e)

        If (bBLock) Then
            Return True
        End If

        Return MyBase.ProcessCmdKey(msg, e)
    End Function
End Class
