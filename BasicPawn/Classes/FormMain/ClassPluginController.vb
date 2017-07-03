'BasicPawn
'Copyright(C) 2017 TheTimocop

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


Public Class ClassPluginController
    Private g_mFormMain As FormMain

    Public Sub New(f As FormMain)
        g_mFormMain = f
    End Sub

    Structure STRUC_PLUGIN_ITEM
        Dim sFile As String
        Dim mPluginInformation As BasicPawnPluginInterface.IPluginInterface.STRUC_PLUGIN_INFORMATION
        Dim mPluginInterface As BasicPawnPluginInterface.IPluginInterface
    End Structure
    Private g_lPlugins As New List(Of STRUC_PLUGIN_ITEM)

    ReadOnly Property m_Plugins As STRUC_PLUGIN_ITEM()
        Get
            Return g_lPlugins.ToArray
        End Get
    End Property

    Public Sub PluginsExecute(mAction As Action(Of BasicPawnPluginInterface.IPluginInterface))
        For Each mPlugin In m_Plugins
            Try
                mAction(mPlugin.mPluginInterface)
            Catch ex As NotImplementedException
                'Ignore
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        Next
    End Sub

    Public Function LoadPlugin(sFile As String) As BasicPawnPluginInterface.IPluginInterface
        Try
            Dim mAssembly As Reflection.Assembly = Reflection.Assembly.LoadFile(sFile)

            For Each mType As Type In mAssembly.GetTypes
                Try
                    Dim mInstance As Object = mAssembly.CreateInstance(mType.FullName)
                    Dim mPlugin As BasicPawnPluginInterface.IPluginInterface = TryCast(mInstance, BasicPawnPluginInterface.IPluginInterface)
                    If (mPlugin Is Nothing) Then
                        Continue For
                    End If

                    g_lPlugins.Add(New STRUC_PLUGIN_ITEM With {
                        .mPluginInformation = mPlugin.m_PluginInformation,
                        .mPluginInterface = mPlugin,
                        .sFile = sFile
                    })

                    Return mPlugin
                Catch ex As MissingMethodException
                    'Ignore
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            Next
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try

        Return Nothing
    End Function

    Public Sub LoadPlugins(sPluginDirectory As String)
        If (Not IO.Directory.Exists(sPluginDirectory)) Then
            IO.Directory.CreateDirectory(sPluginDirectory)
        End If

        For Each sPluginFile As String In IO.Directory.GetFiles(sPluginDirectory, "*.dll")
            Try
                Dim mPlugin = LoadPlugin(sPluginFile)
                If (mPlugin Is Nothing) Then
                    Continue For
                End If

                mPlugin.OnPluginLoad(sPluginFile)
            Catch ex As NotImplementedException
                'Ignore
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        Next
    End Sub
End Class
