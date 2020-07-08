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


Public Class FormTipOfTheDay
    Private g_lTipsList As New List(Of KeyValuePair(Of String, String))

    Private g_iCurrentTipIndex As Integer = 0

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        ClassControlStyle.SetNameFlag(Panel_FooterControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)
    End Sub

    Private Sub FormTipOfTheDay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)

        LoadViews()

        LoadTips()
    End Sub

    Private Sub FormTipOfTheDay_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveViews()
    End Sub

    ReadOnly Property m_DoNotShow As Boolean
        Get
            If (String.IsNullOrEmpty(Me.Name)) Then
                Return False
            End If

            Using mStream = ClassFileStreamWait.Create(ClassSettings.g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    Return (mIni.ReadKeyValue(Me.Name, "DoNotShow", "0") <> "0")
                End Using
            End Using

            Return False
        End Get
    End Property

    Public Sub SaveViews()
        If (String.IsNullOrEmpty(Me.Name)) Then
            Return
        End If

        Using mStream = ClassFileStreamWait.Create(ClassSettings.g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT) From {
                    New ClassIni.STRUC_INI_CONTENT(Me.Name, "DoNotShow", If(CheckBox_DoNotShow.Checked, "1", "0"))
                }

                mIni.WriteKeyValue(lContent.ToArray)
            End Using
        End Using
    End Sub

    Public Sub LoadViews()
        If (String.IsNullOrEmpty(Me.Name)) Then
            Return
        End If

        Using mStream = ClassFileStreamWait.Create(ClassSettings.g_sWindowInfoFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                CheckBox_DoNotShow.Checked = (mIni.ReadKeyValue(Me.Name, "DoNotShow", "0") <> "0")
            End Using
        End Using
    End Sub

    Private Sub LoadTips()
        g_lTipsList.Clear()
        g_iCurrentTipIndex = 0

        Dim iTipCounter = 0

        Using mIni As New ClassIni(My.Resources.TipOfTheDayTips)
            For Each mItem In mIni.ReadEverything()
                If (mItem.sSection <> "Tips") Then
                    Continue For
                End If

                Dim sTip As String = mItem.sValue.Replace("\n", Environment.NewLine).Trim
                If (String.IsNullOrEmpty(sTip)) Then
                    Continue For
                End If

                iTipCounter += 1
                g_lTipsList.Add(New KeyValuePair(Of String, String)(String.Format("Tip: #{0}", iTipCounter), sTip))
            Next
        End Using

        If (g_lTipsList.Count < 1) Then
            RichTextBox_Tips.Text = "I guess there are no tips."
            Return
        End If

        g_iCurrentTipIndex = ClassTools.ClassRandom.RandomInt(0, g_lTipsList.Count - 1)

        SetTip()
    End Sub

    Private Sub LinkLabel_PreviousTip_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_PreviousTip.LinkClicked
        If (g_lTipsList.Count < 1) Then
            Return
        End If

        g_iCurrentTipIndex += g_lTipsList.Count - 1

        g_iCurrentTipIndex = (g_iCurrentTipIndex Mod g_lTipsList.Count)
        g_iCurrentTipIndex = Math.Max(0, g_iCurrentTipIndex)
        g_iCurrentTipIndex = Math.Min(g_lTipsList.Count - 1, g_iCurrentTipIndex)

        SetTip()
    End Sub

    Private Sub LinkLabel_NextTip_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_NextTip.LinkClicked
        If (g_lTipsList.Count < 1) Then
            Return
        End If

        g_iCurrentTipIndex += 1

        g_iCurrentTipIndex = (g_iCurrentTipIndex Mod g_lTipsList.Count)
        g_iCurrentTipIndex = Math.Max(0, g_iCurrentTipIndex)
        g_iCurrentTipIndex = Math.Min(g_lTipsList.Count - 1, g_iCurrentTipIndex)

        SetTip()
    End Sub

    Private Sub SetTip()
        Label_TipNumber.Text = g_lTipsList(g_iCurrentTipIndex).Key

        RichTextBox_Tips.SuspendLayout()
        RichTextBox_Tips.Text = g_lTipsList(g_iCurrentTipIndex).Value
        RichTextBox_Tips.ResumeLayout()
    End Sub

    Private Sub RichTextBox_Tips_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles RichTextBox_Tips.LinkClicked
        Try
            Process.Start(e.LinkText)
        Catch ex As Exception
        End Try
    End Sub
End Class