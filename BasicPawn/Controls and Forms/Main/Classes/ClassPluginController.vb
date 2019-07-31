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


Imports System.Reflection
Imports BasicPawnPluginInterface

Public Class ClassPluginController
    Private g_mFormMain As FormMain

    Private g_bPluginsLoaded As Boolean = False

    Public Sub New(f As FormMain)
        g_mFormMain = f
    End Sub

    Structure STRUC_PLUGIN_ITEM
        Dim sFile As String
        Dim mPluginInformation As IPluginInfoInterface.STRUC_PLUGIN_INFORMATION
        Dim mPluginInterface As IPluginInterfaceV7
    End Structure
    Private g_lPlugins As New List(Of STRUC_PLUGIN_ITEM)

    Structure STRUC_PLUGIN_FAIL_ITEM
        Dim sFile As String
        Dim mPluginInformation As IPluginInfoInterface.STRUC_PLUGIN_INFORMATION
        Dim mException As Exception
    End Structure
    Private g_lFailPlugins As New List(Of STRUC_PLUGIN_FAIL_ITEM)

    ReadOnly Property m_Plugins As STRUC_PLUGIN_ITEM()
        Get
            Return g_lPlugins.ToArray
        End Get
    End Property

    ReadOnly Property m_FailPlugins As STRUC_PLUGIN_FAIL_ITEM()
        Get
            Return g_lFailPlugins.ToArray
        End Get
    End Property

    Public Function GetPluginInfo(mPluginInterface As IPluginInterfaceV7) As IPluginInfoInterface.STRUC_PLUGIN_INFORMATION
        For Each mPlugin In m_Plugins
            If (mPluginInterface IsNot mPlugin.mPluginInterface) Then
                Continue For
            End If

            Return mPlugin.mPluginInformation
        Next

        Return Nothing
    End Function

    Public Property m_PluginEnabledByConfig(mPlugin As STRUC_PLUGIN_ITEM) As Boolean
        Get
            Dim sFilename As String = IO.Path.GetFileName(mPlugin.sFile)
            Dim sConfigFile As String = IO.Path.Combine(Application.StartupPath, "plugins\PluginConfig.ini")
            Dim bEnabled As Boolean = True

            If (Not IO.File.Exists(sConfigFile)) Then
                IO.File.WriteAllText(sConfigFile, "")
            End If

            Using mStream = ClassFileStreamWait.Create(sConfigFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    For Each mItem In mIni.ReadEverything
                        If (mItem.sSection <> "Plugins") Then
                            Continue For
                        End If

                        If (mItem.sValue.ToLower <> sFilename.ToLower) Then
                            Continue For
                        End If

                        Dim sGuid As String = mItem.sKey

                        bEnabled = (mIni.ReadKeyValue("States", sGuid, "1") = "1")
                        Exit For
                    Next
                End Using
            End Using

            Return bEnabled
        End Get
        Set(value As Boolean)
            Dim sFilename As String = IO.Path.GetFileName(mPlugin.sFile)
            Dim sConfigFile As String = IO.Path.Combine(Application.StartupPath, "plugins\PluginConfig.ini")
            Dim sGuid As String = Guid.NewGuid.ToString

            Using mStream = ClassFileStreamWait.Create(sConfigFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    For Each mItem In mIni.ReadEverything
                        If (mItem.sSection <> "Plugins") Then
                            Continue For
                        End If

                        If (mItem.sValue.ToLower <> sFilename.ToLower) Then
                            Continue For
                        End If

                        sGuid = mItem.sKey
                        Exit For
                    Next

                    mIni.WriteKeyValue("States", sGuid, If(value, "1", "0"))
                    mIni.WriteKeyValue("Plugins", sGuid, sFilename)
                End Using
            End Using
        End Set
    End Property

    Public Sub PluginsExecute(mAction As Action(Of STRUC_PLUGIN_ITEM))
        For Each mPlugin In m_Plugins
            Try
                mAction(mPlugin)
            Catch ex As NotImplementedException
                'Ignore
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        Next
    End Sub

    Public Sub LoadPlugins(sPluginDirectory As String)
        If (g_bPluginsLoaded) Then
            Throw New ArgumentException("Plugins can only be loaded once")
        End If

        g_bPluginsLoaded = True

        If (Not IO.Directory.Exists(sPluginDirectory)) Then
            IO.Directory.CreateDirectory(sPluginDirectory)
        End If

        For Each sPluginFile As String In IO.Directory.GetFiles(sPluginDirectory, "*.dll")
            Try
                Dim mPlugin = LoadPlugin(sPluginFile)
                If (mPlugin Is Nothing) Then
                    Continue For
                End If

                Try
                    mPlugin.OnPluginLoad(sPluginFile)
                Catch ex As NotImplementedException
                    'Ignore
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            Catch ex As Exception
                g_mFormMain.g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_ERROR, String.Format("Plugin '{0}' could not be loaded! Exception: {1}", IO.Path.GetFileName(sPluginFile), ex.Message))
            End Try
        Next
    End Sub

    Private Function LoadPlugin(sFile As String) As IPluginInterfaceV7
        Dim mAssembly = Assembly.LoadFile(sFile)

        If (mAssembly Is Nothing) Then
            Return Nothing
        End If

        Dim mPluginInfo As IPluginInfoInterface = Nothing

        Try
            'Find info first
            For Each mType In GetValidTypes(mAssembly)
                If (Not GetType(IPluginInfoInterface).IsAssignableFrom(mType)) Then
                    Continue For
                End If

                mPluginInfo = DirectCast(mAssembly.CreateInstance(mType.FullName), IPluginInfoInterface)
                Exit For
            Next

            If (mPluginInfo Is Nothing) Then
                Throw New ArgumentException("Unable to load plugin. IPluginInfoInterface not found.")
            End If

            'Find plugin stuff
            For Each mType In GetValidTypes(mAssembly)
                If (Not GetType(IPluginInterfaceV7).IsAssignableFrom(mType)) Then
                    Continue For
                End If

                Dim mPlugin = DirectCast(mAssembly.CreateInstance(mType.FullName), IPluginInterfaceV7)

                g_lPlugins.Add(New STRUC_PLUGIN_ITEM With {
                    .mPluginInformation = mPluginInfo.m_PluginInformation,
                    .mPluginInterface = mPlugin,
                    .sFile = sFile
                })

                Return mPlugin
            Next

            Throw New ArgumentException("Unable to load plugin. IPluginInterface not found. Probably outdated plugin.")

        Catch ex As Exception
            g_lFailPlugins.Add(New STRUC_PLUGIN_FAIL_ITEM() With {
                .sFile = sFile,
                .mPluginInformation = If(mPluginInfo IsNot Nothing, mPluginInfo.m_PluginInformation, New IPluginInfoInterface.STRUC_PLUGIN_INFORMATION("", "", "", "", "")),
                .mException = ex
            })

            Throw
        End Try

        Return Nothing
    End Function

    Private Function GetValidTypes(mAssembly As Assembly) As Type()
        Try
            Return mAssembly.GetTypes()
        Catch e As ReflectionTypeLoadException
            Dim lTypes As New List(Of Type)

            For Each mType In e.Types
                If (mType Is Nothing) Then
                    Continue For
                End If

                lTypes.Add(mType)
            Next

            Return lTypes.ToArray
        End Try
    End Function
End Class
