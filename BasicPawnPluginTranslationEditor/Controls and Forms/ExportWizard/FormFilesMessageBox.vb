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


Public Class FormFilesMessageBox
    Public Sub New(sMessage As String, sQuestion As String, sTitle As String, sButton As String, sFiles As String())
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        ClassControlStyle.SetNameFlag(Panel_FooterControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)

        Label_Message.Text = sMessage
        Label_Question.Text = sQuestion

        Me.Text = sTitle

        Button_Apply.Text = sButton

        Try
            ListBox_Files.BeginUpdate()
            ListBox_Files.Items.Clear()

            For Each sFile In sFiles
                ListBox_Files.Items.Add(sFile)
            Next
        Finally
            ListBox_Files.EndUpdate()
        End Try

    End Sub

    Private Sub FormOverwriteMessageBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub
End Class