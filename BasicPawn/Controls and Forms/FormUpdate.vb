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


Public Class FormUpdate
    Private g_mUpdateThread As Threading.Thread
    Private g_mCheckUpdateThread As Threading.Thread

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.  
        ClassControlStyle.SetNameFlag(Panel_FooterControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)
    End Sub

    Private Sub FormUpdate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)

        g_mCheckUpdateThread = New Threading.Thread(AddressOf CheckUpdate) With {
                    .IsBackground = True
                }
        g_mCheckUpdateThread.Start()
    End Sub

    Private Sub Button_Close_Click(sender As Object, e As EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    Private Sub Button_Update_Click(sender As Object, e As EventArgs) Handles Button_Update.Click
        Select Case (MessageBox.Show("All BasicPawn instances will be closed and all your unsaved work will be lost!" & Environment.NewLine & Environment.NewLine & "Do you want to continue?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
            Case DialogResult.Cancel
                Return
        End Select

        If (ClassThread.IsValid(g_mUpdateThread)) Then
            Return
        End If

        g_mUpdateThread = New Threading.Thread(AddressOf UpdateThread) With {
            .IsBackground = True
        }
        g_mUpdateThread.Start()
    End Sub

    Private Sub CheckUpdate()
        Try
            Dim sLocationInfo As String = "Unknown"
            Dim sNextVersion As String = ""
            Dim sCurrentVersion As String = ""
            Dim bSkipCheck As Boolean = False

#If DEBUG Then
            bSkipCheck = True
#End If

            If (ClassUpdate.CheckUpdateAvailable(sLocationInfo, sNextVersion, sCurrentVersion) OrElse bSkipCheck) Then
                ClassThread.ExecEx(Of Object)(Me, Sub()
                                                      With New Text.StringBuilder
                                                          .AppendLine("A new BasicPawn update is available!")
                                                          .AppendFormat("(Server: {0})", sLocationInfo).AppendLine()
                                                          .AppendFormat("Do you want to update from version {0} to version {1} now?", sCurrentVersion, sNextVersion).AppendLine()
                                                          Label_StatusTitle.Text = .ToString
                                                      End With
                                                      Button_Update.Visible = True

                                                      ClassPictureBoxQuality_WarnIcon.Visible = True
                                                      Label_WarnText.Visible = True
                                                  End Sub)
            Else
                ClassThread.ExecEx(Of Object)(Me, Sub()
                                                      Label_StatusTitle.Text = "There are no new updates available!"
                                                  End Sub)
            End If

        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ClassExceptionLog.WriteToLog(ex)

            ClassThread.ExecAsync(Me, Sub()
                                          Label_StatusTitle.Text = "Could not check for updates!"

                                          Label_Status.Text = "Error: " & ex.Message
                                          Label_Status.ForeColor = Color.Red
                                          Label_Status.Visible = True
                                          ProgressBar_Status.Visible = False
                                      End Sub)
        End Try
    End Sub

    Private Sub UpdateThread()
        Try
            ClassThread.ExecEx(Of Object)(Me, Sub()
                                                Label_Status.Text = "Status: Downloading updates..."
                                                ClassControlStyle.UpdateControls(Label_Status)
                                                Label_Status.Visible = True
                                                ProgressBar_Status.Visible = True
                                            End Sub)

            ClassUpdate.InstallUpdate()

            'Debug only
            ClassThread.ExecEx(Of Object)(Me, Sub()
                                                Label_Status.Text = "Status: Downloaded update!"
                                                Label_Status.Visible = True
                                                ProgressBar_Status.Visible = False
                                            End Sub)
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ClassExceptionLog.WriteToLog(ex)

            ClassThread.ExecAsync(Me, Sub()
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


    Private Sub FormUpdate_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub CleanUp()
        ClassThread.Abort(g_mCheckUpdateThread)
        ClassThread.Abort(g_mUpdateThread)
    End Sub
End Class