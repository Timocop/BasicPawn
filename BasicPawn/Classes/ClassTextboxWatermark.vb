Public Class ClassTextboxWatermark
    Inherits TextBox

    Private g_sWatermarkText As String = ""
    Private g_bIsWatermarkVisible As Boolean = False

    ''' <summary>
    ''' Displays text on top of the textbox
    ''' </summary>
    ''' <returns></returns>
    Public Property m_sWatermarkText() As String
        Get
            Return g_sWatermarkText
        End Get
        Set(value As String)
            g_sWatermarkText = value

            If (Not String.IsNullOrEmpty(g_sWatermarkText)) Then
                Me.Text = g_sWatermarkText
                g_bIsWatermarkVisible = True
            End If
        End Set
    End Property

    ''' <summary>
    ''' Checks if the watermark is currently visible
    ''' </summary>
    ''' <returns>True if visible, false otherwise</returns>
    Public ReadOnly Property m_bWatermarkVisible() As Boolean
        Get
            Return g_bIsWatermarkVisible
        End Get
    End Property

    Protected Overrides Sub OnGotFocus(e As EventArgs)
        If (g_bIsWatermarkVisible) Then
            Me.Text = ""
            g_bIsWatermarkVisible = False
        End If

        MyBase.OnGotFocus(e)
    End Sub

    Public Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            If (g_sWatermarkText <> value) Then
                g_bIsWatermarkVisible = False
            End If

            MyBase.Text = value
        End Set
    End Property

    Protected Overrides Sub OnLostFocus(e As EventArgs)
        If (Not String.IsNullOrEmpty(g_sWatermarkText)) Then
            If (String.IsNullOrEmpty(Me.Text) OrElse String.IsNullOrEmpty(Me.Text.Trim)) Then
                Me.Text = g_sWatermarkText
                g_bIsWatermarkVisible = True
            End If
        End If

        MyBase.OnLostFocus(e)
    End Sub
End Class
