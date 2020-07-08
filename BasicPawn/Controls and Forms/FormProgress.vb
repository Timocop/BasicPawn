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


Public Class FormProgress
    Private g_mCloseAction As Func(Of Boolean) = Nothing

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.  
        ClassControlStyle.SetNameFlag(Panel_FooterControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)
    End Sub

    Private Sub FormProgress_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub FormProgress_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If (e.CloseReason = CloseReason.UserClosing) Then
            e.Cancel = If(g_mCloseAction Is Nothing, True, g_mCloseAction.Invoke)
        End If
    End Sub

    Property m_ProgressMax As Integer
        Get
            Return ProgressBar_Progress.Maximum
        End Get
        Set(value As Integer)
            ProgressBar_Progress.Maximum = value
        End Set
    End Property

    Property m_Progress As Integer
        Get
            Return ProgressBar_Progress.Value
        End Get
        Set(value As Integer)
            value = Math.Min(value, ProgressBar_Progress.Maximum)

            'Skip animation
            Dim tmpInt = ProgressBar_Progress.Maximum
            ProgressBar_Progress.Maximum += 1
            ProgressBar_Progress.Value = value + 1
            ProgressBar_Progress.Value = value
            ProgressBar_Progress.Maximum = tmpInt

            Me.Invalidate()
        End Set
    End Property

    ''' <summary>
    ''' Action when closing the form.
    ''' </summary>
    ''' <returns>True to cancel closing, false otherwise</returns>
    Property m_CloseAction As Func(Of Boolean)
        Get
            Return g_mCloseAction
        End Get
        Set(value As Func(Of Boolean))
            g_mCloseAction = value
        End Set
    End Property
End Class