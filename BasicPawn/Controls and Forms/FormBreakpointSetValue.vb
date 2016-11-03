Public Class FormBreakpointSetValue
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'Set the maximum and minimum possible value that SourceMod supports.
        NumericUpDown_BreakpointValue.Maximum = Integer.MaxValue
        NumericUpDown_BreakpointValue.Minimum = Integer.MinValue
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_TypeInteger.CheckedChanged
        'Make Int
        NumericUpDown_BreakpointValue.DecimalPlaces = 0
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_TypeFloatingPoint.CheckedChanged
        'Make Float
        NumericUpDown_BreakpointValue.DecimalPlaces = 5
    End Sub
End Class