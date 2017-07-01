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


Public Class FormUpdate
    Private g_mUpdateThread As Threading.Thread

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        Panel_FooterControl.Name &= "@FooterControl"
        Panel_FooterDarkControl.Name &= "@FooterDarkControl"
    End Sub

    Private Sub FormUpdate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub Button_Close_Click(sender As Object, e As EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    Private Sub Button_Update_Click(sender As Object, e As EventArgs) Handles Button_Update.Click
        If (g_mUpdateThread IsNot Nothing AndAlso g_mUpdateThread.IsAlive) Then
            Return
        End If

        g_mUpdateThread = New Threading.Thread(AddressOf UpdateThread) With {
            .IsBackground = True
        }
        g_mUpdateThread.Start()
    End Sub

    Private Sub UpdateThread()
        Try
            Me.Invoke(Sub()
                          Label_Status.Text = "Status: Downloading updates..."
                          ClassControlStyle.UpdateControls(Label_Status)
                          Label_Status.Visible = True
                          ProgressBar_Status.Visible = True
                      End Sub)

            ClassUpdate.InstallUpdate()

            'Should not reach... ever...
            Me.Invoke(Sub()
                          Label_Status.Text = "Status: Could not install update for unknown reasons!"
                          Label_Status.ForeColor = Color.Red
                          Label_Status.Visible = True
                          ProgressBar_Status.Visible = False
                      End Sub)
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ClassExceptionLog.WriteToLog(ex)

            Me.BeginInvoke(Sub()
                               Label_Status.Text = "Error: " & ex.Message
                               Label_Status.ForeColor = Color.Red
                               Label_Status.Visible = True
                               ProgressBar_Status.Visible = False
                           End Sub)
        End Try
    End Sub

    Private Sub LinkLabel_ManualUpdate_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_ManualUpdate.LinkClicked
        Try
            Process.Start("https://github.com/Timocop/BasicPawn/releases")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class