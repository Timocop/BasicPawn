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


Public Class ClassTabControlFix
    Inherits TabControl

    ''' <summary>
    ''' Updates the line overflow.
    ''' Should be used after removing tab pages.
    ''' </summary>
    Public Sub UpdateLineOverflow()
        If (Me.Multiline) Then
            Return
        End If

        Me.SuspendLayout()
        Me.Multiline = True
        Me.Multiline = False
        Me.ResumeLayout()
    End Sub
End Class
