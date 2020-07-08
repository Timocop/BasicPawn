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


Public Interface IPluginInterfaceV10

#Region "Main"
    ''' <summary>
    ''' Checks if the plugin is enabled or disabled. Controlled by the plugin.
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property m_PluginEnabled As Boolean

    ''' <summary>
    ''' Fires when the user requested to enable/disable the plugin.
    ''' </summary>
    ''' <param name="sReason">The reason why it didn't succeed</param>
    ''' <returns>True on success, false otherwise.</returns>
    Function OnPluginEnabled(ByRef sReason As String) As Boolean
    Function OnPluginDisabled(ByRef sReason As String) As Boolean

    ''' <summary>
    ''' Fires when the plugin is loaded.
    ''' </summary>
    ''' <param name="sDLLPath">The current plugin DLL path.</param>
    Sub OnPluginLoad(sDLLPath As String)

    ''' <summary>
    ''' Fires when main form has finished loading.
    ''' </summary>
    ''' <param name="mFormMain">BasicPawn.FormMain</param> 
    ''' <param name="bEnabled">True if the plugin is enabled, false otherwise. Enabling and Disabling is controlled by the plugin.</param> 
    Sub OnPluginStart(mFormMain As Object, bEnabled As Boolean)

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

    ''' <summary>
    ''' Fires when the main text editor syntax file was updated.
    ''' </summary>
    Sub OnEditorSyntaxUpdate()
    Sub OnEditorSyntaxUpdateEnd()

    ''' <summary>
    ''' Fires when the updater thread is running. Autocomplete etc.
    ''' </summary>
    ''' <param name="iType">BasicPawn.ClassSyntaxTools.ENUM_SYNTAX_UPDATE_TYPE</param>
    ''' <param name="bForceFromMemory"></param> 
    Sub OnSyntaxUpdate(iType As Integer, bForceFromMemory As Boolean)
    Sub OnSyntaxUpdateEnd(iType As Integer, bForceFromMemory As Boolean)

    ''' <summary>
    ''' Fires when the form colors are being updated.
    ''' </summary>
    Sub OnFormColorUpdate()
#End Region

#Region "Debugger"
    ''' <summary>
    ''' Fires when debugger form has finished loading.
    ''' </summary>
    ''' <param name="mFormDebugger">BasicPawn.FormDebugger</param>
    Sub OnDebuggerStart(mFormDebugger As Object)

    ''' <summary>
    ''' Fires when debugger refreshes its source.
    ''' </summary>
    ''' <param name="mFormDebugger">BasicPawn.FormDebugger</param>
    Sub OnDebuggerRefresh(mFormDebugger As Object)

    ''' <summary>
    ''' Fires when the debugger form is closing.
    ''' </summary>
    ''' <param name="mFormDebugger">BasicPawn.FormDebugger</param>
    ''' <returns>False to block closing the main form, true otherwise.</returns>
    Function OnDebuggerEnd(mFormDebugger As Object) As Boolean

    ''' <summary>
    ''' Fires when the Debugger is being disposed.
    ''' </summary>
    ''' <param name="mFormDebugger">BasicPawn.FormDebugger</param>
    Sub OnDebuggerEndPost(mFormDebugger As Object)

    ''' <summary>
    ''' The Debugger started it's debugging.
    ''' </summary>
    Sub OnDebuggerDebugStart()

    ''' <summary>
    ''' The Debugger paused it's debugging.
    ''' </summary>
    Sub OnDebuggerDebugPause()

    ''' <summary>
    ''' The Debugger stopped it's debugging.
    ''' </summary>
    Sub OnDebuggerDebugStop()
#End Region

End Interface