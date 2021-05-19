'BasicPawn
'Copyright(C) 2021 Externet

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


Imports System.Text

Partial Public Class FormMain
    Private Sub ToolStripMenuItem_HelpAbout_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_HelpAbout.Click
        With New StringBuilder
            .AppendFormat("{0} v.{1}", Application.ProductName, Application.ProductVersion).AppendLine()
            .AppendLine("Created by Externet (aka Timocop)")
            .AppendLine()
            .AppendLine("Source and Releases")
            .AppendLine("     https://github.com/Timocop/BasicPawn")
            .AppendLine()
            .AppendLine("Third-Party tools:")
            .AppendLine("     SharpDevelop - TextEditor (LGPL-2.1)")
            .AppendLine()
            .AppendLine("         Authors:")
            .AppendLine("         Daniel Grunwald and SharpDevelop Community")
            .AppendLine("         https://github.com/icsharpcode/SharpDevelop")
            .AppendLine()
            .AppendLine("     SSH.NET (MIT)")
            .AppendLine()
            .AppendLine("         Authors:")
            .AppendLine("         Gert Driesen and Community")
            .AppendLine("         https://github.com/sshnet/SSH.NET")
            .AppendLine()
            .AppendLine("     BigInteger (Copyright (c) 2002 Chew Keong TAN)")
            .AppendLine()
            .AppendLine("         Authors:")
            .AppendLine("         Chew Keong TAN")
            .AppendLine("         https://www.codeproject.com/Articles/2728/C-BigInteger-Class")
            MessageBox.Show(.ToString, "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End With
    End Sub

    Private Sub ToolStripMenuItem_HelpCheckUpdates_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_HelpCheckUpdates.Click
        With New FormUpdate
            .ShowDialog(Me)
        End With
    End Sub

    Private Sub ToolStripMenuItem_ShowTips_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ShowTips.Click
        Dim mFormTip As FormTipOfTheDay = Nothing
        For Each mForm In Application.OpenForms
            If (TypeOf mForm Is FormTipOfTheDay) Then
                mFormTip = DirectCast(mForm, FormTipOfTheDay)
                Exit For
            End If
        Next

        If (mFormTip IsNot Nothing) Then
            mFormTip.Activate()
        Else
            Dim mTipForm As New FormTipOfTheDay
            mTipForm.Show(Me)
        End If
    End Sub

    Private Sub ToolStripMenuItem_HelpGithub_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_HelpGithub.Click
        Try
            Process.Start("https://github.com/Timocop/BasicPawn")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
