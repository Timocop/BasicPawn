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


Imports System.Net
Imports System.Reflection
Imports System.Security.Authentication
Imports System.Text.RegularExpressions
Imports BasicPawnPluginInterface

Public Class ClassPluginController
    Private g_mFormMain As FormMain

    Private g_bPluginsLoaded As Boolean = False

    Public Sub New(f As FormMain)
        g_mFormMain = f
    End Sub

    Class STRUC_PLUGIN_ITEM
        Public sFile As String
        Public mPluginInformation As IPluginInfoInterface.STRUC_PLUGIN_INFORMATION
        Public mPluginVersionInformation As IPluginVersionInterface.STRUC_PLUGIN_VERSION_INFORMATION
        Public mPluginInterface As IPluginInterfaceV11

        Public Sub New(_File As String, _PluginInterface As IPluginInterfaceV11, _PluginInformation As IPluginInfoInterface.STRUC_PLUGIN_INFORMATION, _PluginUpdateInformation As IPluginVersionInterface.STRUC_PLUGIN_VERSION_INFORMATION)
            sFile = _File
            mPluginInterface = _PluginInterface
            mPluginInformation = _PluginInformation
            mPluginVersionInformation = _PluginUpdateInformation
        End Sub
    End Class
    Private g_lPlugins As New List(Of STRUC_PLUGIN_ITEM)

    Class STRUC_PLUGIN_FAIL_ITEM
        Public sFile As String
        Public mPluginInformation As IPluginInfoInterface.STRUC_PLUGIN_INFORMATION
        Public mPluginVersionInformation As IPluginVersionInterface.STRUC_PLUGIN_VERSION_INFORMATION
        Public mException As Exception

        Public Sub New(_File As String, _PluginInformation As IPluginInfoInterface.STRUC_PLUGIN_INFORMATION, _PluginUpdateInformation As IPluginVersionInterface.STRUC_PLUGIN_VERSION_INFORMATION, _Exception As Exception)
            sFile = _File
            mPluginInformation = _PluginInformation
            mPluginVersionInformation = _PluginUpdateInformation
            mException = _Exception
        End Sub
    End Class
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

    Public Function GetPluginInfo(mPluginInterface As IPluginInterfaceV11) As IPluginInfoInterface.STRUC_PLUGIN_INFORMATION
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

    Private Function LoadPlugin(sFile As String) As IPluginInterfaceV11
        Dim mAssembly = Assembly.LoadFile(sFile)

        If (mAssembly Is Nothing) Then
            Return Nothing
        End If

        Dim mPluginInfo As IPluginInfoInterface = Nothing
        Dim mPluginVersion As IPluginVersionInterface = Nothing

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

            'Find version info first
            For Each mType In GetValidTypes(mAssembly)
                If (Not GetType(IPluginVersionInterface).IsAssignableFrom(mType)) Then
                    Continue For
                End If

                mPluginVersion = DirectCast(mAssembly.CreateInstance(mType.FullName), IPluginVersionInterface)
                Exit For
            Next

            'Find plugin stuff
            For Each mType In GetValidTypes(mAssembly)
                If (Not GetType(IPluginInterfaceV11).IsAssignableFrom(mType)) Then
                    Continue For
                End If

                Dim mPlugin = DirectCast(mAssembly.CreateInstance(mType.FullName), IPluginInterfaceV11)

                g_lPlugins.Add(New STRUC_PLUGIN_ITEM(sFile,
                                                     mPlugin,
                                                     mPluginInfo.m_PluginInformation,
                                                     If(mPluginVersion IsNot Nothing, mPluginVersion.m_PluginVersionInformation, New IPluginVersionInterface.STRUC_PLUGIN_VERSION_INFORMATION(""))))

                Return mPlugin
            Next

            Throw New ArgumentException("Unable to load plugin. IPluginInterface not found. Probably outdated plugin.")

        Catch ex As Exception
            g_lFailPlugins.Add(New STRUC_PLUGIN_FAIL_ITEM(sFile,
                                                          If(mPluginInfo IsNot Nothing, mPluginInfo.m_PluginInformation, New IPluginInfoInterface.STRUC_PLUGIN_INFORMATION("", "", "", "", "")),
                                                          If(mPluginVersion IsNot Nothing, mPluginVersion.m_PluginVersionInformation, New IPluginVersionInterface.STRUC_PLUGIN_VERSION_INFORMATION("")),
                                                          ex))

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

    Class ClassPluginUpdate
        Public Shared Function CheckUpdateAvailable(mPlugin As STRUC_PLUGIN_ITEM) As Boolean
            Dim sNextVersion = ""
            Dim sCurrentVersion = ""
            Return CheckUpdateAvailable(mPlugin, sNextVersion, sCurrentVersion)
        End Function

        Public Shared Function CheckUpdateAvailable(mPlugin As STRUC_PLUGIN_ITEM, ByRef r_sNextVersion As String, ByRef r_sCurrentVersion As String) As Boolean
            Dim sNextVersion As String = GetNextVersion(mPlugin)
            Dim sCurrentVersion As String = GetCurrentVerison(mPlugin)

            If (String.IsNullOrEmpty(sNextVersion) OrElse String.IsNullOrEmpty(sCurrentVersion)) Then
                Return False
            End If

            sNextVersion = Regex.Match(sNextVersion, "[0-9\.]+").Value
            sCurrentVersion = Regex.Match(sCurrentVersion, "[0-9\.]+").Value

            r_sNextVersion = sNextVersion
            r_sCurrentVersion = sCurrentVersion

            Return (New Version(sNextVersion) > New Version(sCurrentVersion))
        End Function

        Public Shared Function GetCurrentVerison(mPlguin As STRUC_PLUGIN_ITEM) As String
            Try
                Return mPlguin.mPluginInformation.sVersion
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Shared Function GetNextVersion(mPlguin As STRUC_PLUGIN_ITEM) As String
            If (mPlguin.mPluginVersionInformation Is Nothing OrElse String.IsNullOrEmpty(mPlguin.mPluginVersionInformation.sVersionUrl)) Then
                Return Nothing
            End If

            Dim sFileVersion = GetCurrentVerison(mPlguin)
            If (String.IsNullOrEmpty(sFileVersion)) Then
                Return Nothing
            End If

            SetTLS12()

            Dim sNextVersion As String = Nothing
            Dim sUserAgent As String = String.Format("BasicPawnPlugin/{0} (compatible; Windows NT)", sFileVersion)

            Try
                Using mWC As New ClassWebClientEx
                    If (Not String.IsNullOrEmpty(sUserAgent)) Then
                        mWC.Headers("User-Agent") = sUserAgent
                    End If

                    Dim sVersion = mWC.DownloadString(mPlguin.mPluginVersionInformation.sVersionUrl)
                    If (String.IsNullOrEmpty(sVersion)) Then
                        Return Nothing
                    End If

                    If (Not String.IsNullOrEmpty(sNextVersion)) Then
                        If (New Version(sNextVersion) > New Version(sVersion)) Then
                            Return Nothing
                        End If
                    End If

                    sNextVersion = sVersion
                End Using
            Catch ex As Exception
            End Try

            If (String.IsNullOrEmpty(sNextVersion)) Then
                Throw New ArgumentException("Unable to find update files")
            End If

            Return sNextVersion
        End Function

        Private Shared Sub SetTLS12()
            'https://stackoverflow.com/questions/43240611/net-framework-3-5-and-tls-1-2
            Const _Tls12 As SslProtocols = DirectCast(&HC00, SslProtocols)
            Const Tls12 As SecurityProtocolType = DirectCast(_Tls12, SecurityProtocolType)

            ServicePointManager.SecurityProtocol = Tls12
        End Sub
    End Class

    Class ClassPluginConfig
        Inherits ClassIni

        Private g_sConfigDirectory As String = IO.Path.Combine(Application.StartupPath, "plugins\configs")
        Private g_sConfigName As String = ""

        Public Sub New(sConfigName As String)
            MyBase.New(New IO.MemoryStream())

            If (String.IsNullOrEmpty(sConfigName)) Then
                Throw New ArgumentException("Config name can not be NULL")
            End If

            g_sConfigName = sConfigName
        End Sub

        ReadOnly Property m_ConfigFile As String
            Get
                Return IO.Path.Combine(g_sConfigDirectory, String.Format("{0}.{1}", g_sConfigName, "ini"))
            End Get
        End Property

        Public Sub SaveConfig()
            If (Not IO.Directory.Exists(g_sConfigDirectory)) Then
                IO.Directory.CreateDirectory(g_sConfigDirectory)
            End If

            ExportToFile(m_ConfigFile)
        End Sub

        Public Sub LoadConfig()
            If (Not IO.Directory.Exists(g_sConfigDirectory)) Then
                IO.Directory.CreateDirectory(g_sConfigDirectory)
            End If

            If (Not IO.File.Exists(m_ConfigFile)) Then
                IO.File.WriteAllText(m_ConfigFile, "")
            End If

            ParseFromFile(m_ConfigFile)
        End Sub
    End Class
End Class
