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


Public Class FormDebuggerCriticalPopup
    Private g_mFormDebugger As FormDebugger

    Public Sub New(mFormDebugger As FormDebugger, sTitle As String, sHeaderTitle As String, sText As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ClassControlStyle.SetNameFlag(Panel_FooterControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)
        ClassControlStyle.SetNameFlag(Label_Title, ClassControlStyle.ENUM_STYLE_FLAGS.LABEL_BLACK)

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

    Private Sub FormDebuggerCriticalPopup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub
End Class