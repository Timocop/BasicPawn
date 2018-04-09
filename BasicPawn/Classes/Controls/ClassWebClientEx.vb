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


Imports System.Net

Public Class ClassWebClientEx
    Inherits WebClient

    Private g_iTimeout As Integer

    ''' <summary>
    ''' Connection timeout in milliseconds
    ''' </summary>
    Public Property m_Timeout As Integer
        Get
            Return g_iTimeout
        End Get
        Set(value As Integer)
            g_iTimeout = value
        End Set
    End Property

    Public Sub New()
        Me.New(60000)
    End Sub

    Public Sub New(iTimeout As Integer)
        Me.m_Timeout = iTimeout
    End Sub

    Protected Overrides Function GetWebRequest(mAddress As Uri) As WebRequest
        Dim mRequest = MyBase.GetWebRequest(mAddress)

        If mRequest IsNot Nothing Then
            mRequest.Timeout = Me.m_Timeout
        End If

        Return mRequest
    End Function
End Class
