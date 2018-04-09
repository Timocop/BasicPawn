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


Public Class ClassDatabaseViewerItem
    Private g_sName As String
    Private g_sUsername As String

    Public Sub New(sName As String, sUsername As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_Name = sName
        m_Username = sUsername
    End Sub

    Property m_Name As String
        Get
            Return g_sName
        End Get
        Set(value As String)
            Label_Name.Text = value
            g_sName = value
        End Set
    End Property

    Property m_Username As String
        Get
            Return g_sUsername
        End Get
        Set(value As String)
            Label_Username.Text = value
            g_sUsername = value
        End Set
    End Property

    Private Sub Button_Remove_Click(sender As Object, e As EventArgs) Handles Button_Remove.Click
        For Each iItem In ClassDatabase.GetDatabaseItems
            If (iItem.m_Name <> m_Name OrElse Not iItem.HasUserAccess) Then
                Continue For
            End If

            iItem.Remove()
        Next

        Me.Dispose()
    End Sub

    Private Sub ClassDatabaseViewerItem_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub
End Class
