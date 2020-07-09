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


Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Public Class ClassTools
    Private Shared _RandomInt As New Random

    Class ClassRandom
        ''' <summary>
        ''' Gets a random number.
        ''' </summary>
        ''' <param name="Min"></param>
        ''' <param name="Max"></param>
        ''' <returns></returns>
        Public Shared Function RandomInt(Min As Integer, Max As Integer) As Integer
            If (Max < Min) Then
                Return Max
            End If

            Return _RandomInt.Next(Min, Max)
        End Function

        ''' <summary>
        ''' Generate a random string with length and custom pattern.
        ''' </summary>
        ''' <param name="length"></param>
        ''' <param name="pattern"></param>
        ''' <returns></returns>
        Public Shared Function Generate(length As Integer, Optional pattern As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_") As String
            Dim SB As New Text.StringBuilder
            For i = 1 To length
                SB.Append(pattern.Substring(RandomInt(0, pattern.Length), 1))
            Next
            Return SB.ToString
        End Function
    End Class

    Class ClassProcess
        Public Shared Sub ExecuteProgram(sPath As String, sArguments As String, ByRef r_ExitCode As Integer, ByRef r_Output As String)
            ExecuteProgram(sPath, sArguments, IO.Path.GetDirectoryName(sPath), r_ExitCode, r_Output)
        End Sub

        Public Shared Sub ExecuteProgram(sPath As String, sArguments As String, sWorkingDirectory As String, ByRef r_ExitCode As Integer, ByRef r_Output As String)
            ExecuteProgram(sPath, sArguments, sWorkingDirectory, Nothing, r_ExitCode, r_Output)
        End Sub

        Public Shared Sub ExecuteProgram(sPath As String, sArguments As String, sWorkingDirectory As String, mEnvironmentVariables As Dictionary(Of String, String), ByRef r_ExitCode As Integer, ByRef r_Output As String)
            r_ExitCode = 0
            r_Output = ""

            Using i As New Process
                i.StartInfo.FileName = sPath
                i.StartInfo.Arguments = sArguments
                i.StartInfo.WorkingDirectory = sWorkingDirectory

                i.StartInfo.UseShellExecute = False
                i.StartInfo.CreateNoWindow = True
                i.StartInfo.RedirectStandardOutput = True

                If (mEnvironmentVariables IsNot Nothing) Then
                    For Each mVar In mEnvironmentVariables
                        i.StartInfo.EnvironmentVariables(mVar.Key) = mVar.Value
                    Next
                End If

                i.Start()

                'Read 'StandartOutput' before 'WaitForExit'. The compiler tends to "wait" forever for something sometimes.
                r_Output = i.StandardOutput.ReadToEnd

                i.WaitForExit()

                r_ExitCode = i.ExitCode
            End Using
        End Sub
    End Class

    Class ClassStrings

        ''' <summary>
        ''' Checks if the text is a word A-Z0-9.
        ''' </summary>
        ''' <param name="sText"></param>
        ''' <returns></returns>
        Public Shared Function IsWord(sText As String) As Boolean
            Return Regex.IsMatch(sText, "^[a-zA-Z0-9_]+$")
        End Function

        ''' <summary>
        ''' Counts words.
        ''' </summary>
        ''' <param name="sText"></param>
        ''' <param name="sSearch"></param>
        ''' <returns></returns>
        Public Shared Function WordCount(sText As String, sSearch As String) As Integer
            Dim i As Integer = 0
            Dim iCount As Integer = 0

            While True
                i = sText.IndexOf(sSearch, i)
                If (i < 0) Then
                    Exit While
                End If

                iCount += 1
                i += sSearch.Length
            End While

            Return iCount
        End Function


        ''' <summary>
        ''' Reads the lines at the end of the file.
        ''' Should be used on big files.
        ''' </summary>
        ''' <param name="sFile"></param>
        ''' <param name="iMaxLines"></param>
        ''' <returns></returns>
        Public Shared Function StringReadLinesEnd(sFile As String, iMaxLines As Integer) As String()
            Using SR As New IO.StreamReader(sFile)
                SR.BaseStream.Seek(0, IO.SeekOrigin.End)

                Dim iCount As Integer = 0

                While (iCount < iMaxLines AndAlso SR.BaseStream.Position > 0)
                    SR.BaseStream.Position -= 1

                    Dim iChr As Integer = SR.BaseStream.ReadByte

                    If (SR.BaseStream.Position > 0) Then
                        SR.BaseStream.Position -= 1
                    End If

                    If (iChr = AscW(vbLf)) Then
                        iCount += 1

                        If (iCount = iMaxLines) Then
                            If (SR.BaseStream.Position < SR.BaseStream.Length) Then
                                SR.BaseStream.Position += 1
                            End If

                            Exit While
                        End If
                    End If
                End While

                Return SR.ReadToEnd.Split(New String() {Environment.NewLine, vbLf}, 0)
            End Using
        End Function

        Public Shared Sub ReadStringPart(sText As String, iIndex As Integer, iBackReadLen As Integer, iForwardReadLen As Integer, ByRef sBackText As String, ByRef sForwardText As String)
            sBackText = Nothing
            sForwardText = Nothing

            If (iBackReadLen > 0) Then
                Dim iMin As Integer = iIndex - iBackReadLen
                If (iMin < 0) Then
                    sBackText = sText.Substring(0, iBackReadLen - Math.Abs(iMin))
                Else
                    sBackText = sText.Substring(iMin, iBackReadLen)
                End If
            End If

            If (iForwardReadLen > 0) Then
                Dim iMax As Integer = iIndex + iForwardReadLen
                If (iMax > sText.Length - 1) Then
                    sForwardText = sText.Substring(iIndex, (Math.Abs(iMax) - iForwardReadLen) + 2)
                Else
                    sForwardText = sText.Substring(iIndex, iForwardReadLen)
                End If
            End If
        End Sub

        Public Shared Function FormatBytes(iBytes As Double) As String
            Dim sNames As String() = New String() {"Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"}
            Dim i = 0
            While (iBytes >= 1024 AndAlso i < sNames.Length - 1)
                i += 1
                iBytes = (iBytes / 1024)
            End While

            Return iBytes.ToString("0.##") & " " & sNames(i)
        End Function

        Public Shared Function ToSafeKey(sText As String) As String
            If (sText Is Nothing) Then
                Return Nothing
            End If

            Return ClassCrypto.ClassBase.ToBase64(sText, Encoding.UTF8)
        End Function

        Public Shared Function ToUnsafeKey(sText As String) As String
            If (sText Is Nothing) Then
                Return Nothing
            End If

            Return ClassCrypto.ClassBase.FromBase64(sText, Encoding.UTF8)
        End Function
    End Class

    Class ClassControls
        Class ClassListView
            Public Shared Sub AutoResizeColumns(mListView As ListView)
                Dim iWidth As Integer = 0

                mListView.BeginUpdate()

                For Each mColumn As ColumnHeader In mListView.Columns
                    iWidth = mColumn.Width

                    mListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
                    If (iWidth > mColumn.Width) Then
                        mColumn.Width = iWidth
                    End If

                    mListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
                    If (iWidth > mColumn.Width) Then
                        mColumn.Width = iWidth
                    End If
                Next

                mListView.EndUpdate()
            End Sub
        End Class
    End Class

    Class ClassForms
        ''' <summary>
        ''' Checks if a form is opened.
        ''' </summary>
        ''' <param name="fForm"></param>
        ''' <returns></returns>
        Public Shared Function IsFormOpen(fForm As Form) As Boolean
            For Each f As Form In Application.OpenForms
                If (f Is fForm) Then
                    Return True
                End If
            Next

            Return False
        End Function


        Class NativeWinAPI
            ''' <summary>
            ''' http://pinvoke.net/default.aspx/Enums/ShowWindowCommand.html
            ''' </summary>
            Enum ShowWindowCommands As Integer
                ''' <summary>
                ''' Hides the window and activates another window.
                ''' </summary>
                Hide = 0
                ''' <summary>
                ''' Activates and displays a window. If the window is minimized or 
                ''' maximized, the system restores it to its original size and position.
                ''' An application should specify this flag when displaying the window 
                ''' for the first time.
                ''' </summary>
                Normal = 1
                ''' <summary>
                ''' Activates the window and displays it as a minimized window.
                ''' </summary>
                ShowMinimized = 2
                ''' <summary>
                ''' Maximizes the specified window.
                ''' </summary>
                Maximize = 3
                ' is this the right value?
                ''' <summary>
                ''' Activates the window and displays it as a maximized window.
                ''' </summary>       
                ShowMaximized = 3
                ''' <summary>
                ''' Displays a window in its most recent size and position. This value 
                ''' is similar to ShowWindowCommands.Normal, except 
                ''' the window is not actived.
                ''' </summary>
                ShowNoActivate = 4
                ''' <summary>
                ''' Activates the window and displays it in its current size and position. 
                ''' </summary>
                Show = 5
                ''' <summary>
                ''' Minimizes the specified window and activates the next top-level 
                ''' window in the Z order.
                ''' </summary>
                Minimize = 6
                ''' <summary>
                ''' Displays the window as a minimized window. This value is similar to
                ''' ShowWindowCommands.ShowMinimized, except the 
                ''' window is not activated.
                ''' </summary>
                ShowMinNoActive = 7
                ''' <summary>
                ''' Displays the window in its current size and position. This value is 
                ''' similar to ShowWindowCommands.Show, except the 
                ''' window is not activated.
                ''' </summary>
                ShowNA = 8
                ''' <summary>
                ''' Activates and displays the window. If the window is minimized or 
                ''' maximized, the system restores it to its original size and position. 
                ''' An application should specify this flag when restoring a minimized window.
                ''' </summary>
                Restore = 9
                ''' <summary>
                ''' Sets the show state based on the SW_* value specified in the 
                ''' STARTUPINFO structure passed to the CreateProcess function by the 
                ''' program that started the application.
                ''' </summary>
                ShowDefault = 10
                ''' <summary>
                '''  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread 
                ''' that owns the window is not responding. This flag should only be 
                ''' used when minimizing windows from a different thread.
                ''' </summary>
                ForceMinimize = 11
            End Enum


            Friend Shared ReadOnly GWL_EXSTYLE As Integer = -20
            Friend Shared ReadOnly WS_EX_COMPOSITED As Integer = &H2000000
            Friend Shared ReadOnly WM_SETREDRAW As Integer = 11

            <DllImport("user32")>
            Friend Shared Function GetWindowLong(hWnd As IntPtr, nIndex As Integer) As Integer
            End Function

            <DllImport("user32")>
            Friend Shared Function SetWindowLong(hWnd As IntPtr, nIndex As Integer, dwNewLong As Integer) As Integer
            End Function

            <DllImport("user32")>
            Friend Shared Function SendMessage(hWnd As IntPtr, wMsg As Integer, wParam As Boolean, lParam As Integer) As Integer
            End Function

            <DllImport("user32")>
            Friend Shared Function ShowWindow(hWnd As IntPtr, nCmdShow As ShowWindowCommands) As Boolean
            End Function

            <DllImport("uxtheme", CharSet:=CharSet.[Unicode], ExactSpelling:=False)>
            Friend Shared Function SetWindowTheme(hWnd As IntPtr, textSubAppName As String, textSubIdList As String) As Integer
            End Function

            <DllImport("user32")>
            Friend Shared Function FlashWindow(hwnd As IntPtr, bInvert As Boolean) As Boolean
            End Function
        End Class

        ''' <summary>
        ''' Enables/Disables double buffering using unmanaged.
        ''' Only works on Windows Vista and higher!
        ''' </summary>
        ''' <param name="c"></param>
        Public Shared Sub SetDoubleBufferingUnmanaged(c As Control, bEnable As Boolean)
            If (Environment.OSVersion.Version.Major > 5) Then
                Dim iStyle As Integer = NativeWinAPI.GetWindowLong(c.Handle, NativeWinAPI.GWL_EXSTYLE)

                If (bEnable) Then
                    iStyle = iStyle Or NativeWinAPI.WS_EX_COMPOSITED
                Else
                    iStyle = iStyle And Not NativeWinAPI.WS_EX_COMPOSITED
                End If

                NativeWinAPI.SetWindowLong(c.Handle, NativeWinAPI.GWL_EXSTYLE, iStyle)
            End If
        End Sub

        ''' <summary>
        ''' Enables/Disables double buffering using unmanaged on all control childs.
        ''' Only works on Windows Vista and higher!
        ''' </summary>
        ''' <param name="c"></param>
        Public Shared Sub SetDoubleBufferingUnmanagedAllChilds(c As Control, bEnable As Boolean)
            SetDoubleBufferingUnmanaged(c, bEnable)
            For Each c2 As Control In c.Controls
                SetDoubleBufferingUnmanagedAllChilds(c2, bEnable)
            Next
        End Sub

        ''' <summary>
        ''' Force double buffering using reflection.
        ''' </summary>
        ''' <param name="c"></param>
        ''' <param name="bEnable"></param>
        Public Shared Sub SetDoubleBuffering(c As Control, bEnable As Boolean)
            GetType(Control).InvokeMember("DoubleBuffered", Reflection.BindingFlags.SetProperty Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic, Nothing, c, New Object() {True})
        End Sub

        ''' <summary>
        ''' Force double buffering using reflection on all control childs.
        ''' </summary>
        ''' <param name="c"></param>
        ''' <param name="bEnable"></param>
        Public Shared Sub SetDoubleBufferingAllChilds(c As Control, bEnable As Boolean)
            SetDoubleBuffering(c, bEnable)
            For Each c2 As Control In c.Controls
                SetDoubleBufferingAllChilds(c2, bEnable)
            Next
        End Sub

        ''' <summary>
        ''' Suspends drawing of a control using unmanaged.
        ''' </summary>
        ''' <param name="i"></param>
        ''' <param name="c"></param>
        Public Shared Sub SuspendDrawing(ByRef i As Integer, c As Control)
            If (i = 0) Then
                NativeWinAPI.SendMessage(c.Handle, NativeWinAPI.WM_SETREDRAW, False, 0)
            End If

            i += 1
        End Sub

        ''' <summary>
        ''' Resumes drawing of a control using unmanaged.
        ''' </summary>
        ''' <param name="i"></param>
        ''' <param name="c"></param>
        Public Shared Sub ResumeDrawing(ByRef i As Integer, c As Control)
            If (i > 0) Then
                i -= 1

                If (i = 0) Then
                    NativeWinAPI.SendMessage(c.Handle, NativeWinAPI.WM_SETREDRAW, True, 0)
                    c.Refresh()
                End If
            End If
        End Sub

        ''' <summary>
        ''' Enables explorer themes on controls
        ''' </summary>
        ''' <param name="c">Control</param>
        Public Shared Sub EnableTheme(c As Control)
            NativeWinAPI.SetWindowTheme(c.Handle, "explorer", Nothing)
        End Sub

        ''' <summary>
        ''' Disables explorer themes on controls
        ''' </summary>
        ''' <param name="c">Control</param>
        Public Shared Sub DisableTheme(c As Control)
            NativeWinAPI.SetWindowTheme(c.Handle, "", "")
        End Sub

        ''' <summary>
        ''' Calculates a value with DPI
        ''' </summary>
        ''' <param name="cCtrl">The control to read the DPI from</param>
        ''' <param name="f"></param>
        ''' <returns></returns>
        Public Shared Function CalcDPI(cCtrl As Control, f As Single) As Single
            Return f / (96.0F / cCtrl.CreateGraphics().DpiX)
        End Function

        Public Shared Sub FormWindowCommand(f As Form, i As NativeWinAPI.ShowWindowCommands)
            NativeWinAPI.ShowWindow(f.Handle, i)
        End Sub

        Public Shared Sub FlashFormWindow(f As Form)
            NativeWinAPI.FlashWindow(f.Handle, True)
        End Sub
    End Class

    Public Class ClassCrypto
        Class ClassBase
            Enum ENUM_BASE
                BASE2 = 2
                BASE8 = 8
                BASE10 = 10
                BASE16 = 16
            End Enum

            Public Shared Function ToBase(sText As String, iBase As ENUM_BASE, i As Text.Encoding) As String
                Dim iBytes() As Byte = i.GetBytes(sText)

                Dim mStringBuilder As New Text.StringBuilder
                For j As Integer = 0 To iBytes.Length - 1
                    mStringBuilder.Append(Convert.ToString(iBytes(j), iBase))
                Next

                Return mStringBuilder.ToString
            End Function

            Public Shared Function ToBase64(sText As String, i As Text.Encoding) As String
                Return Convert.ToBase64String(i.GetBytes(sText))
            End Function

            Public Shared Function ToBase64Ex(iData As Byte()) As String
                Return Convert.ToBase64String(iData)
            End Function

            Public Shared Function FromBase64(sText As String, i As Text.Encoding) As String
                Return i.GetString(Convert.FromBase64String(sText))
            End Function

            Public Shared Function FromBase64Ex(sText As String) As Byte()
                Return Convert.FromBase64String(sText)
            End Function

            Public Shared Function TryFromBase64(sText As String, i As Text.Encoding, sDefault As String) As String
                Try
                    Return FromBase64(sText, i)
                Catch ex As Exception
                    Return sDefault
                End Try
            End Function

            Public Shared Function TryFromBase64(sText As String, iDefault As Byte()) As Byte()
                Try
                    Return FromBase64Ex(sText)
                Catch ex As Exception
                    Return iDefault
                End Try
            End Function

        End Class

        Class ClassHash
            Public Shared Function SHA256StringHash(sText As String) As String
                Dim iHash() As Byte
                Dim sResult As New StringBuilder
                Dim sTemp As String = ""

                Using mHash As New SHA256Managed()
                    iHash = mHash.ComputeHash(Encoding.UTF8.GetBytes(sText))

                    For ii As Integer = 0 To iHash.Length - 1
                        sTemp = Convert.ToString(iHash(ii), 16)
                        If (sTemp.Length = 1) Then
                            sTemp = "0" & sTemp
                        End If
                        sResult.Append(sTemp)
                    Next

                    mHash.Clear()
                End Using

                Return sResult.ToString
            End Function

            Public Shared Function SHA256FileHash(sFile As String) As String
                Dim iHash() As Byte
                Dim sResult As New StringBuilder
                Dim sTemp As String = ""

                Using mHash As New SHA256Managed()
                    Using mFS As New IO.FileStream(sFile, IO.FileMode.Open, IO.FileAccess.Read)
                        iHash = mHash.ComputeHash(mFS)
                    End Using

                    For ii As Integer = 0 To iHash.Length - 1
                        sTemp = Convert.ToString(iHash(ii), 16)
                        If (sTemp.Length = 1) Then
                            sTemp = "0" & sTemp
                        End If
                        sResult.Append(sTemp)
                    Next

                    mHash.Clear()
                End Using

                Return sResult.ToString
            End Function
        End Class

        Class ClassRSA
            Public Shared Sub GenerateKeys(ByRef r_sPrivateKeyXML As String, ByRef r_sPublicKeyXML As String, Optional iKeySize As Integer = 2048)
                Using mRSA As New RSACryptoServiceProvider(iKeySize)
                    Try
                        r_sPrivateKeyXML = mRSA.ToXmlString(True)
                        r_sPublicKeyXML = mRSA.ToXmlString(False)
                    Finally
                        mRSA.PersistKeyInCsp = False
                    End Try
                End Using
            End Sub

            Public Shared Function Encrypt(sText As String, sKeyXML As String) As String
                Dim iData As Byte() = New UTF8Encoding(False).GetBytes(sText)

                Using mRSA As New RSACryptoServiceProvider()
                    Try
                        mRSA.FromXmlString(sKeyXML)

                        Dim iEncryptedData As Byte()

                        If (mRSA.PublicOnly) Then
                            Dim mRSAParm = mRSA.ExportParameters(False)

                            Dim bigInteger As BigInteger = New BigInteger(iData)
                            Dim bigInteger2 As BigInteger = bigInteger.modPow(New BigInteger(mRSAParm.Exponent), New BigInteger(mRSAParm.Modulus))

                            iEncryptedData = bigInteger2.getBytes
                        Else
                            Dim mRSAParm = mRSA.ExportParameters(True)

                            Dim bigInteger As BigInteger = New BigInteger(iData)
                            Dim bigInteger2 As BigInteger = bigInteger.modPow(New BigInteger(mRSAParm.D), New BigInteger(mRSAParm.Modulus))

                            iEncryptedData = bigInteger2.getBytes
                        End If

                        Return Convert.ToBase64String(iEncryptedData)
                    Finally
                        mRSA.PersistKeyInCsp = False
                    End Try
                End Using
            End Function

            Public Shared Function Decrypt(sTextBase64 As String, sKeyXML As String) As String
                Dim iData As Byte() = Convert.FromBase64String(sTextBase64)

                Using mRSA As New RSACryptoServiceProvider()
                    Try
                        mRSA.FromXmlString(sKeyXML)

                        Dim iDecryptedData As Byte()

                        If (mRSA.PublicOnly) Then
                            Dim mRSAParm = mRSA.ExportParameters(False)

                            Dim bigInteger As BigInteger = New BigInteger(iData)
                            Dim bigInteger2 As BigInteger = bigInteger.modPow(New BigInteger(mRSAParm.Exponent), New BigInteger(mRSAParm.Modulus))

                            iDecryptedData = bigInteger2.getBytes
                        Else
                            Dim mRSAParm = mRSA.ExportParameters(True)

                            Dim bigInteger As BigInteger = New BigInteger(iData)
                            Dim bigInteger2 As BigInteger = bigInteger.modPow(New BigInteger(mRSAParm.D), New BigInteger(mRSAParm.Modulus))

                            iDecryptedData = bigInteger2.getBytes
                        End If

                        Return New UTF8Encoding(False).GetString(iDecryptedData)
                    Finally
                        mRSA.PersistKeyInCsp = False
                    End Try
                End Using
            End Function
        End Class
    End Class

    Class ClassOperatingSystem
        <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
        Public Shared Function GetModuleHandle(sModuleName As String) As IntPtr
        End Function

        <DllImport("kernel32.dll", CharSet:=CharSet.Ansi, SetLastError:=True)>
        Private Shared Function GetProcAddress(hModule As IntPtr, sProcName As String) As IntPtr
        End Function

        <DllImport("ntdll.dll", CharSet:=CharSet.Ansi, SetLastError:=True)>
        Private Shared Function wine_get_version() As String
        End Function

        ''' <summary>
        ''' Gets the Wine version if its running on Wine, otherwise it returns |Nothing|.
        ''' https://www.winehq.org/pipermail/wine-devel/2008-September/069387.html
        ''' </summary>
        ''' <returns>Wine version on success, |Nothing| otherwise.</returns>
        Public Shared Function GetWineVersion() As String
            Static sWineVersion As String = Nothing
            Static iWineInstalled As Integer = -1
            If (iWineInstalled > -1) Then
                Return sWineVersion
            End If

            Dim hModule As IntPtr = GetModuleHandle("ntdll.dll")
            If (hModule = IntPtr.Zero) Then
                iWineInstalled = 0
                Return Nothing
            End If

            Dim hAddress As IntPtr = GetProcAddress(hModule, "wine_get_version")
            If (hAddress = IntPtr.Zero) Then
                iWineInstalled = 0
                Return Nothing
            End If

            iWineInstalled = 1
            sWineVersion = wine_get_version()

            Return sWineVersion
        End Function

        ''' <summary>
        ''' Checks if the application is running using Mono.
        ''' http://www.mono-project.com/docs/gui/winforms/porting-winforms-applications/
        ''' </summary>
        ''' <returns>True if running on Mono, false otherwise</returns>
        Public Shared Function IsMonoRuntime() As Boolean
            Static iMonoRuntime As Integer = -1

            If (iMonoRuntime > -1) Then
                Return (iMonoRuntime > 0)
            End If

            If (Type.GetType("Mono.Runtime") IsNot Nothing) Then
                iMonoRuntime = 1
                Return True
            Else
                iMonoRuntime = 0
                Return False
            End If
        End Function
    End Class

    Class ClassRegistry
        Enum ENUM_SELECTION_MODEL
            [SINGLE]
            PLAYER
            DOCUMENT
        End Enum

        Public Shared Sub SetAssociation(sProgID As String, sExtension As String, sCommand As String, sIconFile As String, sDefaultIcon As String, iSelectionModel As ENUM_SELECTION_MODEL)
            If (String.IsNullOrEmpty(sProgID)) Then
                Throw New ArgumentException("ProgID invalid")
            End If

            If (String.IsNullOrEmpty(sExtension)) Then
                Throw New ArgumentException("Extension invalid")
            End If

            Using mClassesKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes", True)
                If (mClassesKey Is Nothing) Then
                    Return
                End If

                Using mExtKey = mClassesKey.CreateSubKey(sProgID)
                    Using mShellKey = mExtKey.CreateSubKey("DefaultIcon")
                        mShellKey.SetValue("", """" & sDefaultIcon & """,0")
                    End Using

                    Using mShellKey = mExtKey.CreateSubKey("Shell")
                        Using mTextKey = mShellKey.CreateSubKey("Open")
                            mTextKey.SetValue("Icon", """" & sIconFile & """")

                            Select Case (iSelectionModel)
                                Case ENUM_SELECTION_MODEL.SINGLE
                                    mTextKey.SetValue("MultiSelectModel", "Single")
                                Case ENUM_SELECTION_MODEL.PLAYER
                                    mTextKey.SetValue("MultiSelectModel", "Player")
                                Case ENUM_SELECTION_MODEL.DOCUMENT
                                    mTextKey.SetValue("MultiSelectModel", "Document")
                                Case Else
                                    Throw New ArgumentException("Unknown selection model")
                            End Select

                            Using mCommandKey = mTextKey.CreateSubKey("command")
                                mCommandKey.SetValue("", sCommand)
                            End Using
                        End Using
                    End Using
                End Using

                Using mExtKey = mClassesKey.CreateSubKey(sExtension)
                    mExtKey.SetValue("", sProgID)
                End Using
            End Using
        End Sub

        Public Shared Sub RemoveAssociation(sProgID As String)
            If (String.IsNullOrEmpty(sProgID)) Then
                Throw New ArgumentException("ProgID invalid")
            End If

            Using mClassesKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes", True)
                If (mClassesKey Is Nothing) Then
                    Return
                End If

                Using mExtKey = mClassesKey.OpenSubKey(sProgID)
                    If (mExtKey Is Nothing) Then
                        Return
                    End If
                End Using

                mClassesKey.DeleteSubKeyTree(sProgID)
            End Using

        End Sub


        Public Shared Sub SetExplorerContextMenu(sExtension As String, sContextText As String, sCommand As String, sIconFile As String)
            Using mClassesKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes", True)
                If (mClassesKey Is Nothing) Then
                    Return
                End If

                Using mExtKey = mClassesKey.CreateSubKey(sExtension)
                    Using mShellKey = mExtKey.CreateSubKey("Shell")
                        Using mTextKey = mShellKey.CreateSubKey(sContextText)
                            mTextKey.SetValue("Icon", """" & sIconFile & """")

                            Using mCommandKey = mTextKey.CreateSubKey("command")
                                mCommandKey.SetValue("", sCommand)
                            End Using
                        End Using
                    End Using
                End Using
            End Using
        End Sub

        Public Shared Sub RemoveExplorerContextMenu(sExtension As String, sContextText As String)
            Using mClassesKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Classes", True)
                If (mClassesKey Is Nothing) Then
                    Return
                End If

                Using mExtKey = mClassesKey.OpenSubKey(sExtension, True)
                    If (mExtKey Is Nothing) Then
                        Return
                    End If

                    Using mShellKey = mExtKey.OpenSubKey("Shell", True)
                        If (mShellKey Is Nothing) Then
                            Return
                        End If

                        Using mContextKey = mShellKey.OpenSubKey(sContextText, True)
                            If (mContextKey Is Nothing) Then
                                Return
                            End If
                        End Using

                        mShellKey.DeleteSubKeyTree(sContextText)
                    End Using
                End Using
            End Using
        End Sub
    End Class

    Class ClassMath
        Public Shared Function ClampInt(i As Integer, iMin As Integer, iMax As Integer) As Integer
            If (i > iMax) Then
                i = iMax
            End If

            If (i < iMin) Then
                i = iMin
            End If

            Return i
        End Function
    End Class
End Class
