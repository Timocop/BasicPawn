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



Public Class FormDebuggerAssertSetAction
    Enum ENUM_ACTION
        IGNORE
        [ERROR]
        FAIL
    End Enum

    Private g_iAction As ENUM_ACTION = ENUM_ACTION.IGNORE

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        ClassControlStyle.SetNameFlag(Panel_FooterControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)
    End Sub

    ReadOnly Property m_Action As ENUM_ACTION
        Get
            Return g_iAction
        End Get
    End Property

    Private Sub FormDebuggerAssertSetAction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub Button_Fail_Click(sender As Object, e As EventArgs) Handles Button_Fail.Click
        g_iAction = ENUM_ACTION.FAIL

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button_Error_Click(sender As Object, e As EventArgs) Handles Button_Error.Click
        g_iAction = ENUM_ACTION.ERROR

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button_Ignore_Click(sender As Object, e As EventArgs) Handles Button_Ignore.Click
        g_iAction = ENUM_ACTION.IGNORE

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class