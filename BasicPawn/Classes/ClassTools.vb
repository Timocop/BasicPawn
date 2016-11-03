Imports System.Text.RegularExpressions

Public Class ClassTools
    Private Shared _RandomInt As New Random

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
            i.Start()
            r_Output = i.StandardOutput.ReadToEnd
            i.WaitForExit()
            r_ExitCode = i.ExitCode
        End Using
    End Sub

    ''' <summary>
    ''' Checks if the text is a word A-Z0-9.
    ''' </summary>
    ''' <param name="sText"></param>
    ''' <returns></returns>
    Public Shared Function IsWord(sText As String) As Boolean
        Return Regex.IsMatch(sText, " ^[a-zA-Z0-9_]+$")
    End Function

    Public Shared Function WordCount(sText As String, sSearch As String) As Integer
#If Not USE_REGEX Then
        Dim iCount As Integer = 0
        Dim i As Integer = 0
        While True
            i = sText.IndexOf(sSearch, i)
            If (i < 0) Then
                Exit While
            End If

            i += 1
            iCount += 1
        End While

        Return iCount 
#Else
        Return Regex.Matches(sText, Regex.Escape(sSearch)).Count
#End If
    End Function

    ''' <summary>
    ''' Checks if a form is opened.
    ''' </summary>
    ''' <param name="fForm"></param>
    ''' <returns></returns>
    Public Shared Function IsFormOpen(fForm As Form)
        For Each f As Form In Application.OpenForms
            If (f Is fForm) Then
                Return True
            End If
        Next

        Return False
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

End Class
