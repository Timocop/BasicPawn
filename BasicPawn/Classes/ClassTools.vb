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


Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions

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
        ''' Generate a random string with lenght and custom pattern.
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
        ''' <summary>
        ''' Executes a program and receives exit code and output.
        ''' </summary>
        ''' <param name="sPath"></param>
        ''' <param name="sArguments"></param>
        ''' <param name="r_ExitCode"></param>
        ''' <param name="r_Output"></param>
        Public Shared Sub ExecuteProgram(sPath As String, sArguments As String, ByRef r_ExitCode As Integer, ByRef r_Output As String)
            Using i As New Process
                i.StartInfo.CreateNoWindow = True
                i.StartInfo.RedirectStandardOutput = True
                i.StartInfo.UseShellExecute = False
                i.StartInfo.FileName = sPath
                i.StartInfo.Arguments = sArguments
                i.StartInfo.WorkingDirectory = IO.Path.GetDirectoryName(sPath)
                i.Start()
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
            Return Regex.Matches(sText, Regex.Escape(sSearch)).Count
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
        End Class

        ''' <summary>
        ''' Enables/Disables double buffering using unmanaged.
        ''' Only works on Windows Vista and higher!
        ''' </summary>
        ''' <param name="cControl"></param>
        Public Shared Sub SetDoubleBufferingUnmanaged(cControl As Control, bEnable As Boolean)
            If (Environment.OSVersion.Version.Major > 5) Then
                Dim style As Integer = NativeWinAPI.GetWindowLong(cControl.Handle, NativeWinAPI.GWL_EXSTYLE)

                If (bEnable) Then
                    style = style Or NativeWinAPI.WS_EX_COMPOSITED
                Else
                    style = style And Not NativeWinAPI.WS_EX_COMPOSITED
                End If

                NativeWinAPI.SetWindowLong(cControl.Handle, NativeWinAPI.GWL_EXSTYLE, style)
            End If
        End Sub

        ''' <summary>
        ''' Enables/Disables double buffering using unmanaged on all control childs.
        ''' Only works on Windows Vista and higher!
        ''' </summary>
        ''' <param name="cControl"></param>
        Public Shared Sub SetDoubleBufferingUnmanagedAllChilds(cControl As Control, bEnable As Boolean)
            SetDoubleBufferingUnmanaged(cControl, bEnable)
            For Each c As Control In cControl.Controls
                SetDoubleBufferingUnmanagedAllChilds(c, bEnable)
            Next
        End Sub

        ''' <summary>
        ''' Force double buffering using reflection.
        ''' </summary>
        ''' <param name="cControl"></param>
        ''' <param name="bEnable"></param>
        Public Shared Sub SetDoubleBuffering(cControl As Control, bEnable As Boolean)
            Dim controlProperty As Reflection.PropertyInfo = GetType(Control).GetProperty("DoubleBuffered", Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
            controlProperty.SetValue(cControl, bEnable, Nothing)
        End Sub

        ''' <summary>
        ''' Force double buffering using reflection on all control childs.
        ''' </summary>
        ''' <param name="cControl"></param>
        ''' <param name="bEnable"></param>
        Public Shared Sub SetDoubleBufferingAllChilds(cControl As Control, bEnable As Boolean)
            SetDoubleBuffering(cControl, bEnable)
            For Each c As Control In cControl.Controls
                SetDoubleBufferingAllChilds(c, bEnable)
            Next
        End Sub

        ''' <summary>
        ''' Suspends drawing of a control using unmanaged.
        ''' </summary>
        ''' <param name="cControl"></param>
        Public Shared Sub SuspendDrawing(cControl As Control)
            NativeWinAPI.SendMessage(cControl.Handle, NativeWinAPI.WM_SETREDRAW, False, 0)
        End Sub

        ''' <summary>
        ''' Resumes drawing of a control using unmanaged.
        ''' </summary>
        ''' <param name="cControl"></param>
        Public Shared Sub ResumeDrawing(cControl As Control)
            NativeWinAPI.SendMessage(cControl.Handle, NativeWinAPI.WM_SETREDRAW, True, 0)
            cControl.Refresh()
        End Sub
    End Class

    Public Class ClassCrypto
        Public Class Base
            Enum ENUM_BASE
                BASE2 = 2
                BASE8 = 8
                BASE10 = 10
                BASE16 = 16
            End Enum

            Public Shared Function ToBase(sText As String, iBase As ENUM_BASE) As String
                Dim iBytes() As Byte = Text.Encoding.Default.GetBytes(sText)

                Dim mStringBuilder As New Text.StringBuilder
                For i As Integer = 0 To iBytes.Length - 1
                    mStringBuilder.Append(Convert.ToString(iBytes(i), iBase))
                Next

                Return mStringBuilder.ToString
            End Function

            Public Shared Function ToBase64(sText As String) As String
                Return Convert.ToBase64String(Text.Encoding.Default.GetBytes(sText))
            End Function

            Public Shared Function FromBase64(sText As String) As String
                Return System.Text.Encoding.Default.GetString(Convert.FromBase64String(sText))
            End Function
        End Class
    End Class
End Class
