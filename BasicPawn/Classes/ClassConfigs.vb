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


Public Class ClassConfigs
    Private Shared g_sConfigFolder As String = IO.Path.Combine(Application.StartupPath, "configs")
    Private Shared g_sConfigFileExt As String = ".ini"
    Private Shared g_mDefaultConfig As New STRUC_CONFIG_ITEM("Default")
    Private Shared g_mActiveConfig As STRUC_CONFIG_ITEM

    Class STRUC_CONFIG_ITEM
        Private g_sName As String = ""

        'General
        Public g_iCompilingType As ClassSettings.ENUM_COMPILING_TYPE = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC
        Public g_sIncludeFolders As String = ""
        Public g_sCompilerPath As String = ""
        Public g_sOutputFolder As String = ""
        Public g_bAutoload As Boolean = False
        'Debugging
        Public g_sDebugGameFolder As String = ""
        Public g_sDebugSourceModFolder As String = ""
        'Misc
        Public g_sExecuteShell As String = ""
        Public g_sSyntaxHighlightingPath As String = ""

        Public Sub New(sName As String)
            g_sName = sName
        End Sub

        Public Sub New(sName As String,
                       iCompilingType As ClassSettings.ENUM_COMPILING_TYPE, sIncludeFolders As String, sCompilerPath As String, sOutputFolder As String, bAutoload As Boolean,
                       sDebugGameFolder As String, sDebugSourceModFolder As String,
                       sExecuteShell As String, sSyntaxHighlightingPath As String)

            g_sName = sName
            'General
            g_iCompilingType = iCompilingType
            g_sIncludeFolders = sIncludeFolders
            g_sCompilerPath = sCompilerPath
            g_sOutputFolder = sOutputFolder
            g_bAutoload = bAutoload
            'Debugging
            g_sDebugGameFolder = sDebugGameFolder
            g_sDebugSourceModFolder = sDebugSourceModFolder
            'Misc
            g_sExecuteShell = sExecuteShell
            g_sSyntaxHighlightingPath = sSyntaxHighlightingPath
        End Sub

        Public Sub SetName(sName As String)
            g_sName = sName
        End Sub

        Public Function GetName() As String
            Return g_sName
        End Function

        Public Function IsDefault() As Boolean
            Return (m_DefaultConfig Is Me)
        End Function

        Public Function IsActive() As Boolean
            Return (m_ActiveConfig Is Me)
        End Function

        Public Function GetConfigPath() As String
            If (IsDefault()) Then
                Return Nothing
            End If

            Return IO.Path.Combine(m_ConfigFolder, g_sName & m_ConfigFileExtension)
        End Function

        Public Function ConfigExist() As Boolean
            If (String.IsNullOrEmpty(g_sName)) Then
                Return False
            End If

            Dim sConfigFile As String = IO.Path.Combine(m_ConfigFolder, g_sName & m_ConfigFileExtension)

            Return IO.File.Exists(sConfigFile)
        End Function

        Public Function SaveConfig() As Boolean
            Return ClassConfigs.SaveConfig(Me)
        End Function

        Public Function RemoveConfig() As Boolean
            Return ClassConfigs.RemoveConfig(g_sName)
        End Function
    End Class

    Shared ReadOnly Property m_ConfigFolder As String
        Get
            Return g_sConfigFolder
        End Get
    End Property

    Shared ReadOnly Property m_ConfigFileExtension As String
        Get
            Return g_sConfigFileExt
        End Get
    End Property

    Shared ReadOnly Property m_DefaultConfig As STRUC_CONFIG_ITEM
        Get
            Return g_mDefaultConfig
        End Get
    End Property

    Shared Property m_ActiveConfig As STRUC_CONFIG_ITEM
        Get
            If (g_mActiveConfig IsNot Nothing AndAlso g_mActiveConfig.ConfigExist) Then
                Return g_mActiveConfig
            Else
                Return g_mDefaultConfig
            End If
        End Get
        Set(value As STRUC_CONFIG_ITEM)
            If (value Is Nothing) Then
                g_mActiveConfig = m_DefaultConfig
            Else
                g_mActiveConfig = value
            End If
        End Set
    End Property

    Shared Function SaveConfig(mConfig As STRUC_CONFIG_ITEM) As Boolean
        If (String.IsNullOrEmpty(mConfig.GetName) OrElse mConfig.GetName = m_DefaultConfig.GetName) Then
            Return False
        End If

        Dim sConfigFile As String = IO.Path.Combine(m_ConfigFolder, mConfig.GetName & m_ConfigFileExtension)

        If (Not IO.Directory.Exists(m_ConfigFolder)) Then
            IO.Directory.CreateDirectory(m_ConfigFolder)
        End If

        Dim iniFile As New ClassIniFile(sConfigFile)
        'Misc
        iniFile.WriteKeyValue("Config", "Type", If(mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.CONFIG, "1", "0"))
        iniFile.WriteKeyValue("Config", "CompilerPath", mConfig.g_sCompilerPath)
        iniFile.WriteKeyValue("Config", "IncludeDirectory", mConfig.g_sIncludeFolders)
        iniFile.WriteKeyValue("Config", "OutputDirectory", mConfig.g_sOutputFolder)
        iniFile.WriteKeyValue("Config", "Autoload", If(mConfig.g_bAutoload, "1", "0"))
        'Debugging
        iniFile.WriteKeyValue("Config", "DebugGameDirectory", mConfig.g_sDebugGameFolder)
        iniFile.WriteKeyValue("Config", "DebugSourceModDirectory", mConfig.g_sDebugSourceModFolder)
        'Misc
        iniFile.WriteKeyValue("Config", "ExecuteShell", mConfig.g_sExecuteShell)
        iniFile.WriteKeyValue("Config", "SyntaxPath", mConfig.g_sSyntaxHighlightingPath)

        Return True
    End Function

    Shared Function LoadConfig(sName As String) As STRUC_CONFIG_ITEM
        If (String.IsNullOrEmpty(sName) OrElse sName = m_DefaultConfig.GetName) Then
            Return Nothing
        End If

        Dim sConfigFile As String = IO.Path.Combine(m_ConfigFolder, sName & g_sConfigFileExt)

        If (Not IO.File.Exists(sConfigFile)) Then
            Return Nothing
        End If

        Dim iniFile As New ClassIniFile(sConfigFile)

        'General
        Dim iCompilingType As ClassSettings.ENUM_COMPILING_TYPE = If(iniFile.ReadKeyValue("Config", "Type", "0") <> "0", ClassSettings.ENUM_COMPILING_TYPE.CONFIG, ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC)
        Dim sOpenIncludeFolders As String = iniFile.ReadKeyValue("Config", "IncludeDirectory", "")
        Dim sCompilerPath As String = iniFile.ReadKeyValue("Config", "CompilerPath", "")
        Dim sOutputFolder As String = iniFile.ReadKeyValue("Config", "OutputDirectory", "")
        Dim bIsDefault As Boolean = (iniFile.ReadKeyValue("Config", "Autoload", "0") <> "0")
        'Debugging
        Dim sDebugGameFolder As String = iniFile.ReadKeyValue("Config", "DebugGameDirectory", "")
        Dim sDebugSourceModFolder As String = iniFile.ReadKeyValue("Config", "DebugSourceModDirectory", "")
        'Misc
        Dim sExecuteShell As String = iniFile.ReadKeyValue("Config", "ExecuteShell", "")
        Dim sSyntaxHighlightingPath As String = iniFile.ReadKeyValue("Config", "SyntaxPath", "")

        Return New STRUC_CONFIG_ITEM(sName, iCompilingType, sOpenIncludeFolders, sCompilerPath, sOutputFolder, bIsDefault, sDebugGameFolder, sDebugSourceModFolder, sExecuteShell, sSyntaxHighlightingPath)
    End Function

    Shared Function RemoveConfig(sName As String) As Boolean
        Dim sConfigFile As String = IO.Path.Combine(m_ConfigFolder, sName & m_ConfigFileExtension)

        If (Not IO.Directory.Exists(m_ConfigFolder)) Then
            IO.Directory.CreateDirectory(m_ConfigFolder)
        End If

        If (IO.File.Exists(sConfigFile)) Then
            IO.File.Delete(sConfigFile)
            Return True
        End If

        Return False
    End Function

    Shared Function GetConfigs(bIncludeDefault As Boolean) As STRUC_CONFIG_ITEM()
        Dim lConfigList As New List(Of STRUC_CONFIG_ITEM) From {
            m_DefaultConfig
        }

        If (IO.Directory.Exists(m_ConfigFolder)) Then
            For Each sFile As String In IO.Directory.GetFiles(m_ConfigFolder)
                If (IO.Path.GetExtension(sFile).ToLower <> m_ConfigFileExtension.ToLower) Then
                    Continue For
                End If

                Dim sName As String = IO.Path.GetFileNameWithoutExtension(sFile)

                Dim mConfig As STRUC_CONFIG_ITEM = LoadConfig(sName)
                If (mConfig Is Nothing) Then
                    Continue For
                End If

                lConfigList.Add(mConfig)
            Next
        End If

        Return lConfigList.ToArray
    End Function
End Class
