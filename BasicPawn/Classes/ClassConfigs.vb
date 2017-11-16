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


Imports System.Text.RegularExpressions

Public Class ClassConfigs
    Private Shared ReadOnly g_sConfigFolder As String = IO.Path.Combine(Application.StartupPath, "configs")
    Private Shared ReadOnly g_sConfigFileExt As String = ".ini"
    Private Shared ReadOnly g_mDefaultConfig As New STRUC_CONFIG_ITEM("Default")

    Class STRUC_CONFIG_ITEM
        Private g_sName As String = ""

        Enum ENUM_LANGUAGE_DETECT_TYPE
            AUTO_DETECT
            SOURCEPAWN
            AMXMODX
        End Enum

        Class CompilerOptions
            Interface ICompilerOptions

                Function BuildCommandline() As String

                Sub SaveToIni(ByRef lContent As List(Of ClassIni.STRUC_INI_CONTENT))

                Sub LoadFromIni(ByRef mIni As ClassIni)

            End Interface

            Class STRUC_SP_COMPILER_OPTIONS
                Implements ICompilerOptions

                'Options:
                '         -a       output assembler code
                '         -c<name> codepage name Or number; e.g. 1252 for Windows Latin-1
                '         -Dpath   active directory path
                '         -e<name> set name of error file (quiet compile)
                '         -H<hwnd> window handle to send a notification message on finish
                '         -h       show included file paths
                '         -i<name> path for include files
                '         -l       create list file (preprocess only)
                '         -o<name> set base name of (P-code) output file
                '         -O<num>  optimization level (default=-O2)
                '             0    no optimization
                '             2    full optimizations
                '         -p<name> set name of "prefix" file
                '         -s<num>  skip lines from the input file
                '         -t<num>  TAB indent size (in character positions, default=8)
                '         -v<num>  verbosity level; 0=quiet, 1=normal, 2=verbose (default=1)
                '         -w<num>  disable a specific warning by its number
                '         -E       treat warnings as errors
                '         -\       use '\' for escape characters
                '         -^       use '^' for escape characters
                '         -;<+/->  require a semicolon to end each statement (default=-)
                '         sym=val  define constant "sym" with value "val"
                '         sym=     define constant "sym" with value 0

                Private g_sSectionName As String = "CompilerOptionsSP"

                Public g_iOptimizationLevel As Integer = -1
                Public g_iVerbosityLevel As Integer = -1
                Public g_iTreatWarningsAsErrors As Integer = -1
                Public g_lIgnoredWarnings As New List(Of String)
                Public g_mDefineConstants As New Dictionary(Of String, String)

                Public Sub New()
                End Sub

                Public Sub New(iOptimizationLevel As Integer, iVerbosityLevel As Integer, iTreatWarningsAsErrors As Integer, lIgnoredWarnings As List(Of String), mDefineConstants As Dictionary(Of String, String))
                    g_iOptimizationLevel = iOptimizationLevel
                    g_iVerbosityLevel = iVerbosityLevel
                    g_iTreatWarningsAsErrors = iTreatWarningsAsErrors
                    g_lIgnoredWarnings = New List(Of String)(lIgnoredWarnings)
                    g_mDefineConstants = New Dictionary(Of String, String)(mDefineConstants)
                End Sub

                Public Function BuildCommandline() As String Implements ICompilerOptions.BuildCommandline
                    Dim lCmds As New List(Of String)

                    'g_iOptimizationLevel
                    Select Case (g_iOptimizationLevel)
                        Case 0, 2
                            lCmds.Add(String.Format("-O{0}", g_iOptimizationLevel))
                    End Select

                    'g_iVerbosityLevel
                    Select Case (g_iVerbosityLevel)
                        Case 0, 1, 2
                            lCmds.Add(String.Format("-v{0}", g_iVerbosityLevel))
                    End Select

                    'g_bTreatWarningsAsErrors
                    Select Case (g_iTreatWarningsAsErrors)
                        Case 1
                            lCmds.Add("-E")
                    End Select

                    'g_lIgnoredWarnings
                    For Each sWarnNum In g_lIgnoredWarnings
                        If (Not IsValidIgnoredWarning(sWarnNum)) Then
                            Continue For
                        End If

                        lCmds.Add(String.Format("-w{0}", sWarnNum))
                    Next

                    'g_mDefineConstants
                    For Each mDefine In g_mDefineConstants
                        If (Not IsValidDefineConstant(mDefine.Key, mDefine.Value)) Then
                            Continue For
                        End If

                        lCmds.Add(String.Format("{0}={1}", mDefine.Key, mDefine.Value))
                    Next

                    Return String.Join(" "c, lCmds.ToArray)
                End Function

                Public Sub SaveToIni(ByRef lContent As List(Of ClassIni.STRUC_INI_CONTENT)) Implements ICompilerOptions.SaveToIni
                    Dim tmpStr As String

                    lContent.Add(New ClassIni.STRUC_INI_CONTENT(g_sSectionName, "OptimizationLevel", If(g_iOptimizationLevel < 0, Nothing, CStr(g_iOptimizationLevel))))
                    lContent.Add(New ClassIni.STRUC_INI_CONTENT(g_sSectionName, "VerbosityLevel", If(g_iVerbosityLevel < 0, Nothing, CStr(g_iVerbosityLevel))))
                    lContent.Add(New ClassIni.STRUC_INI_CONTENT(g_sSectionName, "TreatWarningsAsErrors", If(g_iTreatWarningsAsErrors < 0, Nothing, CStr(g_iTreatWarningsAsErrors))))

                    tmpStr = IgnoredWarningsToString(g_lIgnoredWarnings)
                    lContent.Add(New ClassIni.STRUC_INI_CONTENT(g_sSectionName, "IgnoreWarnings", If(String.IsNullOrEmpty(tmpStr), Nothing, tmpStr)))

                    tmpStr = DefineConstantsToString(g_mDefineConstants)
                    lContent.Add(New ClassIni.STRUC_INI_CONTENT(g_sSectionName, "DefineConstants", If(String.IsNullOrEmpty(tmpStr), Nothing, tmpStr)))
                End Sub

                Public Sub LoadFromIni(ByRef mIni As ClassIni) Implements ICompilerOptions.LoadFromIni
                    Dim tmpStr As String

                    tmpStr = mIni.ReadKeyValue(g_sSectionName, "OptimizationLevel", Nothing)
                    g_iOptimizationLevel = If(String.IsNullOrEmpty(tmpStr), -1, CInt(tmpStr))

                    tmpStr = mIni.ReadKeyValue(g_sSectionName, "VerbosityLevel", Nothing)
                    g_iVerbosityLevel = If(String.IsNullOrEmpty(tmpStr), -1, CInt(tmpStr))

                    tmpStr = mIni.ReadKeyValue(g_sSectionName, "TreatWarningsAsErrors", Nothing)
                    g_iTreatWarningsAsErrors = If(String.IsNullOrEmpty(tmpStr), -1, CInt(tmpStr))

                    tmpStr = mIni.ReadKeyValue(g_sSectionName, "IgnoreWarnings", "")
                    g_lIgnoredWarnings.Clear()
                    ParseIgnoredWarnings(tmpStr, g_lIgnoredWarnings)

                    tmpStr = mIni.ReadKeyValue(g_sSectionName, "DefineConstants", "")
                    g_mDefineConstants.Clear()
                    ParseDefineConstants(tmpStr, g_mDefineConstants)
                End Sub
            End Class

            Class STRUC_AMXX_COMPILER_OPTIONS
                Implements ICompilerOptions

                'Options:
                '         -A<num>  alignment in bytes of the data segment And the stack
                '         -a       output assembler code
                '         -C[+/-]  compact encoding for output file (default=-)
                '         -c<name> codepage name Or number; e.g. 1252 for Windows Latin-1
                '         -Dpath   active directory path
                '         -d0      no symbolic information, no run-time checks
                '         -d1      [default] run-time checks, no symbolic information
                '         -d2      full debug information And dynamic checking
                '         -d3      full debug information, dynamic checking, no optimization
                '         -e<name> set name of error file (quiet compile)
                '         -H<hwnd> window handle to send a notification message on finish
                '         -i<name> path for include files
                '         -l       create list file (preprocess only)
                '         -o<name> set base name of (P-code) output file
                '         -p<name> set name of "prefix" file
                '         -r[name] write cross reference report to console Or to specified file
                '         -S<num>  stack/heap size in cells (default=4096)
                '         -s<num>  skip lines from the input file
                '         -t<num>  TAB indent size (in character positions, default=8)
                '         -v<num>  verbosity level; 0=quiet, 1=normal, 2=verbose (default=1)
                '         -w<num>  disable a specific warning by its number
                '         -E       treat warnings as errors
                '         -X<num>  abstract machine size limit in bytes
                '         -\       use '\' for escape characters
                '         -^       use '^' for escape characters
                '         -;[+/-]  require a semicolon to end each statement (default=-)
                '         -([+/-]  require parantheses for function invocation (default=-)
                '         sym=val  define constant "sym" with value "val"
                '         sym=     define constant "sym" with value 0

                Private g_sSectionName As String = "CompilerOptionsAMXX"

                Public g_iVerbosityLevel As Integer = -1
                Public g_iTreatWarningsAsErrors As Integer = -1
                Public g_iSymbolicInformation As Integer = -1
                Public g_lIgnoredWarnings As New List(Of String)
                Public g_mDefineConstants As New Dictionary(Of String, String)

                Public Sub New()
                End Sub

                Public Sub New(iVerbosityLevel As Integer, iTreatWarningsAsErrors As Integer, iSymbolicInformation As Integer, lIgnoredWarnings As List(Of String), mDefineConstants As Dictionary(Of String, String))
                    g_iVerbosityLevel = iVerbosityLevel
                    g_iTreatWarningsAsErrors = iTreatWarningsAsErrors
                    g_lIgnoredWarnings = New List(Of String)(lIgnoredWarnings)
                    g_mDefineConstants = New Dictionary(Of String, String)(mDefineConstants)
                    g_iSymbolicInformation = iSymbolicInformation
                End Sub

                Public Function BuildCommandline() As String Implements ICompilerOptions.BuildCommandline
                    Dim lCmds As New List(Of String)

                    'g_iVerbosityLevel
                    Select Case (g_iVerbosityLevel)
                        Case 0, 1, 2
                            lCmds.Add(String.Format("-v{0}", g_iVerbosityLevel))
                    End Select

                    'g_bTreatWarningsAsErrors
                    Select Case (g_iTreatWarningsAsErrors)
                        Case 1
                            lCmds.Add("-E")
                    End Select

                    'g_iSymbolicInformation
                    Select Case (g_iSymbolicInformation)
                        Case 0, 1, 2, 3
                            lCmds.Add(String.Format("-d{0}", g_iSymbolicInformation))
                    End Select

                    'g_lIgnoredWarnings
                    For Each sWarnNum In g_lIgnoredWarnings
                        If (Not IsValidIgnoredWarning(sWarnNum)) Then
                            Continue For
                        End If

                        lCmds.Add(String.Format("-w{0}", sWarnNum))
                    Next

                    'g_mDefineConstants
                    For Each mDefine In g_mDefineConstants
                        If (Not IsValidDefineConstant(mDefine.Key, mDefine.Value)) Then
                            Continue For
                        End If

                        lCmds.Add(String.Format("{0}={1}", mDefine.Key, mDefine.Value))
                    Next

                    Return String.Join(" "c, lCmds.ToArray)
                End Function

                Public Sub SaveToIni(ByRef lContent As List(Of ClassIni.STRUC_INI_CONTENT)) Implements ICompilerOptions.SaveToIni
                    Dim tmpStr As String

                    lContent.Add(New ClassIni.STRUC_INI_CONTENT(g_sSectionName, "VerbosityLevel", If(g_iVerbosityLevel < 0, Nothing, CStr(g_iVerbosityLevel))))
                    lContent.Add(New ClassIni.STRUC_INI_CONTENT(g_sSectionName, "TreatWarningsAsErrors", If(g_iTreatWarningsAsErrors < 0, Nothing, CStr(g_iTreatWarningsAsErrors))))
                    lContent.Add(New ClassIni.STRUC_INI_CONTENT(g_sSectionName, "SymbolicInformation", If(g_iSymbolicInformation < 0, Nothing, CStr(g_iVerbosityLevel))))

                    tmpStr = IgnoredWarningsToString(g_lIgnoredWarnings)
                    lContent.Add(New ClassIni.STRUC_INI_CONTENT(g_sSectionName, "IgnoreWarnings", If(String.IsNullOrEmpty(tmpStr), Nothing, tmpStr)))

                    tmpStr = DefineConstantsToString(g_mDefineConstants)
                    lContent.Add(New ClassIni.STRUC_INI_CONTENT(g_sSectionName, "DefineConstants", If(String.IsNullOrEmpty(tmpStr), Nothing, tmpStr)))
                End Sub

                Public Sub LoadFromIni(ByRef mIni As ClassIni) Implements ICompilerOptions.LoadFromIni
                    Dim tmpStr As String

                    tmpStr = mIni.ReadKeyValue(g_sSectionName, "VerbosityLevel", Nothing)
                    g_iVerbosityLevel = If(String.IsNullOrEmpty(tmpStr), -1, CInt(tmpStr))

                    tmpStr = mIni.ReadKeyValue(g_sSectionName, "TreatWarningsAsErrors", Nothing)
                    g_iTreatWarningsAsErrors = If(String.IsNullOrEmpty(tmpStr), -1, CInt(tmpStr))

                    tmpStr = mIni.ReadKeyValue(g_sSectionName, "SymbolicInformation", Nothing)
                    g_iSymbolicInformation = If(String.IsNullOrEmpty(tmpStr), -1, CInt(tmpStr))

                    tmpStr = mIni.ReadKeyValue(g_sSectionName, "IgnoreWarnings", "")
                    g_lIgnoredWarnings.Clear()
                    ParseIgnoredWarnings(tmpStr, g_lIgnoredWarnings)

                    tmpStr = mIni.ReadKeyValue(g_sSectionName, "DefineConstants", "")
                    g_mDefineConstants.Clear()
                    ParseDefineConstants(tmpStr, g_mDefineConstants)
                End Sub
            End Class

            Public Shared Sub ParseIgnoredWarnings(sInput As String, ByRef lIgnoredWarnings As List(Of String))
                For Each sWarnNum As String In sInput.Split(";"c)
                    sWarnNum = sWarnNum.Trim

                    If (Not IsValidIgnoredWarning(sWarnNum)) Then
                        Continue For
                    End If

                    lIgnoredWarnings.Add(sWarnNum)
                Next
            End Sub

            Public Shared Function IgnoredWarningsToString(ByRef lIgnoredWarnings As List(Of String)) As String
                Dim tmpList As New List(Of String)

                For Each sWarnNum In lIgnoredWarnings
                    If (Not IsValidIgnoredWarning(sWarnNum)) Then
                        Continue For
                    End If

                    tmpList.Add(sWarnNum)
                Next

                Return String.Join(";"c, tmpList.ToArray)
            End Function

            Public Shared Sub ParseDefineConstants(sInput As String, ByRef mDefineConstants As Dictionary(Of String, String))
                For Each sDefine As String In sInput.Split(";"c)
                    Dim sDefineArray As String() = sDefine.Split("="c)
                    If (sDefineArray.Length <> 2) Then
                        Continue For
                    End If

                    sDefineArray(0) = sDefineArray(0).Trim
                    sDefineArray(1) = sDefineArray(1).Trim

                    If (Not IsValidDefineConstant(sDefineArray(0), sDefineArray(1))) Then
                        Continue For
                    End If

                    mDefineConstants(sDefineArray(0)) = sDefineArray(1)
                Next
            End Sub

            Public Shared Function DefineConstantsToString(ByRef mDefineConstants As Dictionary(Of String, String)) As String
                Dim tmpList As New List(Of String)

                For Each mDefine In mDefineConstants
                    If (Not IsValidDefineConstant(mDefine.Key, mDefine.Value)) Then
                        Continue For
                    End If

                    tmpList.Add(String.Format("{0}={1}", mDefine.Key, mDefine.Value))
                Next

                Return String.Join(";"c, tmpList.ToArray)
            End Function

            Public Shared Function IsValidIgnoredWarning(sWarnNum As String) As Boolean
                Return Regex.IsMatch(sWarnNum, "^[0-9]+$")
            End Function

            Public Shared Function IsValidDefineConstant(sKey As String, sValue As String) As Boolean
                Return (Regex.IsMatch(sKey, "^[a-zA-Z0-9_]+$") AndAlso Regex.IsMatch(sValue, "^[0-9]+$"))
            End Function
        End Class

        'General
        Public g_iCompilingType As ClassSettings.ENUM_COMPILING_TYPE = ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC
        Public g_sIncludeFolders As String = ""
        Public g_sCompilerPath As String = ""
        Public g_sOutputFolder As String = ""
        Public g_bAutoload As Boolean = False
        Public g_iLanguage As ENUM_LANGUAGE_DETECT_TYPE = ENUM_LANGUAGE_DETECT_TYPE.AUTO_DETECT
        'Compiler Options
        Public g_mCompilerOptionsSP As New CompilerOptions.STRUC_SP_COMPILER_OPTIONS
        Public g_mCompilerOptionsAMXX As New CompilerOptions.STRUC_AMXX_COMPILER_OPTIONS
        'Debugging
        Public g_sDebugGameFolder As String = ""
        Public g_sDebugSourceModFolder As String = ""
        'Misc
        Public g_sExecuteShell As String = ""

        Public Sub New(sName As String)
            g_sName = sName
        End Sub

        Public Sub New(sName As String,
                       iCompilingType As ClassSettings.ENUM_COMPILING_TYPE, sIncludeFolders As String, sCompilerPath As String, sOutputFolder As String, bAutoload As Boolean, iLanguage As ENUM_LANGUAGE_DETECT_TYPE,
                       mCompilerOptionsSP As CompilerOptions.STRUC_SP_COMPILER_OPTIONS, mCompilerOptionsAMXX As CompilerOptions.STRUC_AMXX_COMPILER_OPTIONS,
                       sDebugGameFolder As String, sDebugSourceModFolder As String,
                       sExecuteShell As String)

            g_sName = sName
            'General
            g_iCompilingType = iCompilingType
            g_sIncludeFolders = sIncludeFolders
            g_sCompilerPath = sCompilerPath
            g_sOutputFolder = sOutputFolder
            g_bAutoload = bAutoload
            g_iLanguage = iLanguage
            'Compiler Options
            g_mCompilerOptionsSP = mCompilerOptionsSP
            g_mCompilerOptionsAMXX = mCompilerOptionsAMXX
            'Debugging
            g_sDebugGameFolder = sDebugGameFolder
            g_sDebugSourceModFolder = sDebugSourceModFolder
            'Misc
            g_sExecuteShell = sExecuteShell
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

    Shared Function SaveConfig(mConfig As STRUC_CONFIG_ITEM) As Boolean
        If (String.IsNullOrEmpty(mConfig.GetName) OrElse mConfig.GetName = m_DefaultConfig.GetName) Then
            Return False
        End If

        Dim sConfigFile As String = IO.Path.Combine(m_ConfigFolder, mConfig.GetName & m_ConfigFileExtension)

        If (Not IO.Directory.Exists(m_ConfigFolder)) Then
            IO.Directory.CreateDirectory(m_ConfigFolder)
        End If

        Using mStream = ClassFileStreamWait.Create(sConfigFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT)

                'Misc
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Config", "Type", If(mConfig.g_iCompilingType = ClassSettings.ENUM_COMPILING_TYPE.CONFIG, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Config", "CompilerPath", mConfig.g_sCompilerPath))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Config", "IncludeDirectory", mConfig.g_sIncludeFolders))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Config", "OutputDirectory", mConfig.g_sOutputFolder))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Config", "Autoload", If(mConfig.g_bAutoload, "1", "0")))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Config", "ModType", CStr(mConfig.g_iLanguage)))

                'Compiler Options
                mConfig.g_mCompilerOptionsSP.SaveToIni(lContent)
                mConfig.g_mCompilerOptionsAMXX.SaveToIni(lContent)

                'Debugging
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Config", "DebugGameDirectory", mConfig.g_sDebugGameFolder))
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Config", "DebugSourceModDirectory", mConfig.g_sDebugSourceModFolder))

                'Misc
                lContent.Add(New ClassIni.STRUC_INI_CONTENT("Config", "ExecuteShell", mConfig.g_sExecuteShell))

                mIni.WriteKeyValue(lContent.ToArray)
            End Using
        End Using

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

        Using mStream = ClassFileStreamWait.Create(sConfigFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
            Using mIni As New ClassIni(mStream)
                'General
                Dim iCompilingType As ClassSettings.ENUM_COMPILING_TYPE = If(mIni.ReadKeyValue("Config", "Type", "0") <> "0", ClassSettings.ENUM_COMPILING_TYPE.CONFIG, ClassSettings.ENUM_COMPILING_TYPE.AUTOMATIC)
                Dim sOpenIncludeFolders As String = mIni.ReadKeyValue("Config", "IncludeDirectory", "")
                Dim sCompilerPath As String = mIni.ReadKeyValue("Config", "CompilerPath", "")
                Dim sOutputFolder As String = mIni.ReadKeyValue("Config", "OutputDirectory", "")
                Dim bIsDefault As Boolean = (mIni.ReadKeyValue("Config", "Autoload", "0") <> "0")
                Dim sLanguage As String = mIni.ReadKeyValue("Config", "ModType", CStr(STRUC_CONFIG_ITEM.ENUM_LANGUAGE_DETECT_TYPE.AUTO_DETECT))
                Dim iLanguage As Integer
                If (Integer.TryParse(sLanguage, iLanguage)) Then
                    iLanguage = ClassTools.ClassMath.ClampInt(iLanguage, 0, [Enum].GetNames(GetType(STRUC_CONFIG_ITEM.ENUM_LANGUAGE_DETECT_TYPE)).Length - 1)
                Else
                    iLanguage = STRUC_CONFIG_ITEM.ENUM_LANGUAGE_DETECT_TYPE.AUTO_DETECT
                End If

                'Compiler Options
                Dim mCompilerOptionsSourcePawn As New STRUC_CONFIG_ITEM.CompilerOptions.STRUC_SP_COMPILER_OPTIONS
                Dim mCompilerOptionsAMXModX As New STRUC_CONFIG_ITEM.CompilerOptions.STRUC_AMXX_COMPILER_OPTIONS
                mCompilerOptionsSourcePawn.LoadFromIni(mIni)
                mCompilerOptionsAMXModX.LoadFromIni(mIni)

                'Debugging
                Dim sDebugGameFolder As String = mIni.ReadKeyValue("Config", "DebugGameDirectory", "")
                Dim sDebugSourceModFolder As String = mIni.ReadKeyValue("Config", "DebugSourceModDirectory", "")

                'Misc
                Dim sExecuteShell As String = mIni.ReadKeyValue("Config", "ExecuteShell", "")

                Return New STRUC_CONFIG_ITEM(sName, iCompilingType, sOpenIncludeFolders, sCompilerPath, sOutputFolder, bIsDefault, CType(iLanguage, STRUC_CONFIG_ITEM.ENUM_LANGUAGE_DETECT_TYPE),
                                         mCompilerOptionsSourcePawn, mCompilerOptionsAMXModX,
                                         sDebugGameFolder, sDebugSourceModFolder,
                                         sExecuteShell)
            End Using
        End Using
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

    Class ClassKnownConfigs
        Private Shared ReadOnly g_sLastConfigFile As String = IO.Path.Combine(Application.StartupPath, "knownconfigs.ini")
        Private Shared g_mKnownConfigsDic As New Dictionary(Of String, String)

        Class STRUC_KNOWN_CONFIG_ITEM
            Public sFile As String
            Public sConfigName As String

            Sub New(_File As String, _ConfigName As String)
                sFile = _File
                sConfigName = _ConfigName
            End Sub

            Function FindConfig() As STRUC_CONFIG_ITEM
                For Each mConfig In GetConfigs(False)
                    If (mConfig.GetName = sConfigName) Then
                        Return mConfig
                    End If
                Next

                Return Nothing
            End Function
        End Class

        Public Shared Sub LoadKnownConfigs()
            g_mKnownConfigsDic.Clear()

            Using mStream = ClassFileStreamWait.Create(g_sLastConfigFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    For Each mItem In mIni.ReadEverything
                        If (mItem.sKey <> "ConfigName") Then
                            Continue For
                        End If

                        Dim sFile As String = mItem.sSection
                        Dim sConfigName As String = mItem.sValue

                        g_mKnownConfigsDic(sFile.ToLower) = sConfigName
                    Next
                End Using
            End Using
        End Sub

        Public Shared Sub SaveKnownConfigs()
            Using mStream = ClassFileStreamWait.Create(g_sLastConfigFile, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)
                Using mIni As New ClassIni(mStream)
                    Dim lContent As New List(Of ClassIni.STRUC_INI_CONTENT)

                    For Each mItem In g_mKnownConfigsDic
                        lContent.Add(New ClassIni.STRUC_INI_CONTENT(mItem.Key, "ConfigName", mItem.Value))
                    Next

                    mIni.WriteKeyValue(lContent.ToArray)
                End Using
            End Using
        End Sub

        Public Shared Property m_KnownConfigByFile(sFile As String) As STRUC_CONFIG_ITEM
            Get
                If (Not g_mKnownConfigsDic.ContainsKey(sFile.ToLower)) Then
                    Return Nothing
                End If

                Dim sConfigName As String = g_mKnownConfigsDic(sFile.ToLower)

                Return LoadConfig(sConfigName)
            End Get
            Set(value As STRUC_CONFIG_ITEM)
                If (value Is Nothing OrElse value.IsDefault) Then
                    g_mKnownConfigsDic(sFile.ToLower) = Nothing
                Else
                    g_mKnownConfigsDic(sFile.ToLower) = value.GetName
                End If
            End Set
        End Property

        Public Shared Function GetKnownConfigs() As STRUC_KNOWN_CONFIG_ITEM()
            Dim lKnownConfigs As New List(Of STRUC_KNOWN_CONFIG_ITEM)

            For Each mItem In g_mKnownConfigsDic
                lKnownConfigs.Add(New STRUC_KNOWN_CONFIG_ITEM(mItem.Key, mItem.Value))
            Next

            Return lKnownConfigs.ToArray
        End Function
    End Class
End Class
