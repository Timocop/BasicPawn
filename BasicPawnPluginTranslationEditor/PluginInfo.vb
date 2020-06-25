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


Imports BasicPawnPluginInterface

Public Class PluginInfo
    Implements IPluginInfoInterface

    Public ReadOnly Property m_PluginInformation As IPluginInfoInterface.STRUC_PLUGIN_INFORMATION Implements IPluginInfoInterface.m_PluginInformation
        Get
            Return New IPluginInfoInterface.STRUC_PLUGIN_INFORMATION("Translation Editor",
                                                                     "Timocop",
                                                                     "An editor for translation files.",
                                                                     Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString,
                                                                     "https://github.com/Timocop/BasicPawn")
        End Get
    End Property
End Class
