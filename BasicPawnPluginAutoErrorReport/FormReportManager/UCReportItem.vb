'BasicPawn
'Copyright(C) 2018 TheTimocop

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


Imports System.Drawing
Imports BasicPawn

Public Class UCReportItem
    Public Event OnButtonClick(sender As Object, e As EventArgs)

    Public g_mFormReportManager As FormReportManager

    Public Sub New(mFormReportManager As FormReportManager)
        Me.New(mFormReportManager, "", "", "", Nothing)
    End Sub

    Public Sub New(mFormReportManager As FormReportManager, sTitle As String, sText As String, sDate As String, mImage As Image)
        g_mFormReportManager = mFormReportManager

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Label_Title.Text = sTitle
        Label_File.Text = sText
        Label_Date.Text = sDate
        PictureBox1.Image = mImage
    End Sub

    Private Sub UCReportItem_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub Form_Click(sender As Object, e As EventArgs) Handles Me.Click, Label_Title.Click, Label_File.Click, Label_Date.Click, PictureBox1.Click
        RaiseEvent OnButtonClick(sender, e)
    End Sub
End Class
