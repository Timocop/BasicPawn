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


Public Class FormDatabaseInput
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        ClassControlStyle.SetNameFlag(Panel_FooterControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)
    End Sub

    Private Sub FormDatabaseInput_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    ReadOnly Property m_Name As String
        Get
            Return TextBox_Name.Text
        End Get
    End Property

    ReadOnly Property m_Username As String
        Get
            Return TextBox_Username.Text
        End Get
    End Property

    ReadOnly Property m_Password As String
        Get
            Return TextBox_Password.Text
        End Get
    End Property

    Private Sub Button_Apply_Click(sender As Object, e As EventArgs) Handles Button_Apply.Click
        If (String.IsNullOrEmpty(m_Name) OrElse m_Name.Trim.Length < 1) Then
            MessageBox.Show("Entry name is empty!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If (String.IsNullOrEmpty(m_Username) OrElse m_Username.Trim.Length < 1) Then
            MessageBox.Show("User is empty!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If (String.IsNullOrEmpty(m_Password) OrElse m_Password.Trim.Length < 1) Then
            MessageBox.Show("Password is empty!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If (ClassDatabase.IsNameUsed(m_Name)) Then
            MessageBox.Show("A Database entry with that name already exist!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Private Sub TextBox_Name_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox_Name.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            Button_Apply.PerformClick()
        End If
    End Sub

    Private Sub TextBox_Username_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox_Username.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            Button_Apply.PerformClick()
        End If
    End Sub

    Private Sub TextBox_Password_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox_Password.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            Button_Apply.PerformClick()
        End If
    End Sub
End Class