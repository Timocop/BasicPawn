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


Public Class ClassTextboxWatermark
    Inherits TextBox

    Private g_sWatermarkText As String = ""
    Private g_bIsWatermarkVisible As Boolean = False

    ''' <summary>
    ''' Displays text on top of the textbox.
    ''' </summary>
    ''' <returns></returns>
    Public Property m_WatermarkText() As String
        Get
            Return g_sWatermarkText
        End Get
        Set(value As String)
            g_sWatermarkText = value

            ShowWatermark()
        End Set
    End Property

    ''' <summary>
    ''' Checks if the watermark is currently visible.
    ''' </summary>
    ''' <returns>True if visible, false otherwise</returns>
    Public ReadOnly Property m_WatermarkVisible() As Boolean
        Get
            Return g_bIsWatermarkVisible
        End Get
    End Property

    ''' <summary>
    ''' Gets the current text, will return an empty string when the watermark is visible.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property m_NoWatermarkText As String
        Get
            If (g_bIsWatermarkVisible) Then
                Return ""
            Else
                Return Me.Text
            End If
        End Get
    End Property

    Public Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            MyBase.Text = value

            If (Me.DesignMode) Then
                Return
            End If

            If (g_sWatermarkText <> value) Then
                Me.Font = New Font(Me.Font, FontStyle.Regular)
                g_bIsWatermarkVisible = False
            End If
        End Set
    End Property

    Protected Overrides Sub OnGotFocus(e As EventArgs)
        HideWatermark()

        MyBase.OnGotFocus(e)
    End Sub

    Protected Overrides Sub OnLostFocus(e As EventArgs)
        ShowWatermark()

        MyBase.OnLostFocus(e)
    End Sub

    Public Sub ShowWatermark()
        If (Me.DesignMode) Then
            Return
        End If

        If (g_bIsWatermarkVisible) Then
            Return
        End If

        If (String.IsNullOrEmpty(g_sWatermarkText)) Then
            Return
        End If

        If (Not String.IsNullOrEmpty(Me.Text) AndAlso Not String.IsNullOrEmpty(Me.Text.Trim)) Then
            Return
        End If

        g_bIsWatermarkVisible = True
        Me.Text = g_sWatermarkText
        Me.Font = New Font(Me.Font, FontStyle.Italic)
    End Sub

    Public Sub HideWatermark()
        If (Me.DesignMode) Then
            Return
        End If

        If (Not g_bIsWatermarkVisible) Then
            Return
        End If

        Me.Font = New Font(Me.Font, FontStyle.Regular)
        Me.Text = ""
        g_bIsWatermarkVisible = False
    End Sub
End Class
