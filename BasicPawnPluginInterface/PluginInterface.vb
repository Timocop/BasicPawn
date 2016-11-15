'BasicPawn
'Copyright(C) 2016 TheTimocop

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


Public Interface PluginInterface

    Class STRUC_PLUGIN_INFORMATION
        Public sName As String
        Public sAuthor As String
        Public sDescription As String
        Public sVersion As String
        Public sURL As String

        Public Sub New(_Name As String, _Author As String, _Description As String, _Version As String, _URL As String)
            sName = _Name
            sAuthor = _Author
            sDescription = _Description
            sVersion = _Version
            sURL = _URL
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("Name: {0}, Author: {1}, Description: {2}, Version: {3}, URL: {4}", If(sName, "-"), If(sAuthor, "-"), If(sDescription, "-"), If(sVersion, "-"), If(sURL, "-"))
        End Function
    End Class

    ''' <summary>
    ''' All available plugin information such as name, author etc.
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property m_PluginInformation As STRUC_PLUGIN_INFORMATION

    ''' <summary>
    ''' Fires when the plugin is loaded.
    ''' </summary>
    ''' <param name="sDLLPath">The current plugin DLL path.</param>
    Sub OnPluginLoad(sDLLPath As String)

    ''' <summary>
    ''' Fires when main form has finished loading.
    ''' </summary>
    ''' <param name="mFormMain"></param>
    Sub OnPluginStart(mFormMain As BasicPawn.FormMain)

    ''' <summary>
    ''' Fires when the main form is closing.
    ''' </summary>
    ''' <returns>False to block closing the main form, true otherwise.</returns>
    Function OnPluginEnd() As Boolean

    ''' <summary>
    ''' Fires when the main form is being disposed.
    ''' </summary>
    Sub OnPluginEndPost()

    ''' <summary>
    ''' Fires when settings have been changed.
    ''' </summary>
    Sub OnSettingsChanged()

    ''' <summary>
    ''' Fires when configs have been changed.
    ''' </summary>
    Sub OnConfigChanged()

End Interface
