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


Public Interface IPluginVersionInterface
    Class STRUC_PLUGIN_VERSION_INFORMATION
        Public sVersionUrl As String

        Public Sub New(_VersionURL As String)
            sVersionUrl = _VersionURL
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("URL: {0}", If(sVersionUrl, "-"))
        End Function
    End Class

    ''' <summary>
    ''' All available plugin information such as name, author etc.
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property m_PluginVersionInformation As STRUC_PLUGIN_VERSION_INFORMATION
End Interface
