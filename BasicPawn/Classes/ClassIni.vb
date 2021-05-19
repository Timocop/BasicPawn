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


Public Class ClassIni
    Implements IDisposable

    Private g_mStream As IO.Stream
    Private g_mStreamWriter As IO.StreamWriter
    Private g_mStreamReader As IO.StreamReader

    Structure STRUC_INI_CONTENT
        Dim sSection As String
        Dim sKey As String
        Dim sValue As String

        Sub New(_Section As String, _Key As String, _Value As String)
            sSection = _Section
            sKey = _Key
            sValue = _Value
        End Sub
    End Structure

    Public Sub New()
        Me.New(New IO.MemoryStream())
    End Sub

    Public Sub New(sContent As String)
        Me.New(New IO.MemoryStream())

        g_mStreamWriter.Write(sContent)
    End Sub

    Public Sub New(sFile As String, iMode As IO.FileMode)
        Me.New(New IO.FileStream(sFile, iMode, IO.FileAccess.ReadWrite))
    End Sub

    Public Sub New(mStream As IO.Stream)
        g_mStream = mStream

        If (g_mStream.CanWrite) Then
            g_mStreamWriter = New IO.StreamWriter(mStream)
            g_mStreamWriter.AutoFlush = True
        End If

        If (g_mStream.CanRead) Then
            g_mStreamReader = New IO.StreamReader(mStream)
        End If
    End Sub

    ''' <summary>
    ''' Reads a key value.
    ''' </summary>
    ''' <param name="sFromSection">The section.</param>
    ''' <param name="sFromKey">The key.</param>
    ''' <returns>The value from the key, otherwise Nothing.</returns>
    Public Function ReadKeyValue(sFromSection As String, sFromKey As String) As String
        Try
            For Each mContent In ReadEverything()
                If (mContent.sSection = sFromSection AndAlso mContent.sKey = sFromKey) Then
                    Return mContent.sValue
                End If
            Next
        Catch : End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Reads a key value.
    ''' </summary>
    ''' <param name="sFromSection">The section.</param>
    ''' <param name="sFromKey">The key.</param>
    ''' <param name="sReturnIfNothing">If fails or nothing found it will return this value.</param>
    ''' <returns>The value from the key, otherwise it will return 'sReturnIfNothing'.</returns>
    Public Function ReadKeyValue(sFromSection As String, sFromKey As String, sReturnIfNothing As String) As String
        Try
            For Each mContent In ReadEverything()
                If (mContent.sSection = sFromSection AndAlso mContent.sKey = sFromKey) Then
                    Return mContent.sValue
                End If
            Next
        Catch : End Try

        Return sReturnIfNothing
    End Function

    ''' <summary>
    ''' Reads a key value. Instead of returning Nothing, it will throw an exeption.
    ''' </summary>
    ''' <param name="sFromSection">The section.</param>
    ''' <param name="sFromKey">The key.</param>
    ''' <returns>The value from the key, otherwise Nothing.</returns>
    Public Function ReadKeyValueThrow(sFromSection As String, sFromKey As String) As String
        For Each mContent In ReadEverything()
            If (mContent.sSection = sFromSection AndAlso mContent.sKey = sFromKey) Then
                Return mContent.sValue
            End If
        Next

        Throw New ArgumentException("Can't find INI section or key")
    End Function

    Public Sub RemoveKeyValue(sFromSection As String, sFromKey As String)
        WriteKeyValue({New STRUC_INI_CONTENT(sFromSection, sFromKey, Nothing)})
    End Sub

    Public Sub WriteKeyValue(sFromSection As String, sFromKey As String, Optional sFromValue As String = Nothing)
        WriteKeyValue({New STRUC_INI_CONTENT(sFromSection, sFromKey, sFromValue)})
    End Sub

    Public Sub WriteKeyValue(mContent As STRUC_INI_CONTENT)
        WriteKeyValue({mContent})
    End Sub

    Public Sub WriteKeyValue(mContent As STRUC_INI_CONTENT())
        Dim lContents As New List(Of STRUC_INI_CONTENT)(ReadEverything())

        For j = 0 To mContent.Length - 1
            Dim bFoundContent As Boolean = False

            While True
                For i = 0 To lContents.Count - 1
                    If (lContents(i).sSection = mContent(j).sSection AndAlso lContents(i).sKey = mContent(j).sKey) Then
                        If (mContent(j).sValue Is Nothing) Then
                            lContents.RemoveAt(i)
                            Continue While
                        Else
                            lContents(i) = mContent(j)
                            bFoundContent = True
                        End If
                    End If
                Next

                Exit While
            End While

            If (Not bFoundContent AndAlso mContent(j).sValue IsNot Nothing) Then
                lContents.Add(mContent(j))
            End If
        Next

        'Process write to file
        Dim lSectionNames As New List(Of String)
        For i = 0 To lContents.Count - 1
            If (lSectionNames.Contains(lContents(i).sSection)) Then
                Continue For
            End If

            lSectionNames.Add(lContents(i).sSection)
        Next

        'Sort items and write to file
        Dim SB As New Text.StringBuilder
        For Each sSection In lSectionNames
            SB.AppendFormat("[{0}]", sSection).AppendLine()

            For Each mItem In lContents
                If (mItem.sSection = sSection) Then
                    SB.AppendLine(mItem.sKey & "=" & mItem.sValue)
                End If
            Next
        Next

        g_mStreamWriter.BaseStream.Seek(0, IO.SeekOrigin.Begin)
        g_mStreamWriter.BaseStream.SetLength(0)
        g_mStreamWriter.Write(SB.ToString)
    End Sub

    ''' <summary>
    ''' Gets all section names.
    ''' </summary>
    ''' <returns>All section names.</returns>
    Public Function GetSectionNames() As String()
        Dim mContent = ReadEverything()

        Dim lSectionNames As New List(Of String)
        For i = 0 To mContent.Length - 1
            If (lSectionNames.Contains(mContent(i).sSection)) Then
                Continue For
            End If

            lSectionNames.Add(mContent(i).sSection)
        Next

        Return lSectionNames.ToArray
    End Function

    ''' <summary>
    ''' Parses the ini file and reads everything.
    ''' </summary>
    ''' <returns></returns>
    Public Function ReadEverything() As STRUC_INI_CONTENT()
        Dim lContents As New List(Of STRUC_INI_CONTENT)

        Dim sCurrentSection As String = ""

        g_mStreamReader.BaseStream.Seek(0, IO.SeekOrigin.Begin)

        While True
            Dim sLine As String = g_mStreamReader.ReadLine
            If (sLine Is Nothing) Then
                Exit While
            End If

            'Ignore comments
            If (sLine.TrimStart.StartsWith(";")) Then
                Continue While
            End If

            Dim sSection As String = GetSectionFromLine(sLine)
            If (sSection IsNot Nothing) Then
                sCurrentSection = sSection
            End If

            Dim mContent As STRUC_INI_CONTENT = GetKeyAndValueFromLine(sLine)
            If (mContent.sSection Is Nothing) Then
                Continue While
            End If
            mContent.sSection = sCurrentSection

            lContents.Add(mContent)
        End While

        Return lContents.ToArray
    End Function

    ''' <summary>
    ''' Exports the IO.Stream content to file.
    ''' </summary>
    ''' <param name="sFile"></param>
    Public Sub ExportToFile(sFile As String)
        g_mStream.Seek(0, IO.SeekOrigin.Begin)

        Using mFile As New IO.FileStream(sFile, IO.FileMode.OpenOrCreate, IO.FileAccess.Write)
            mFile.SetLength(0)

            CopyStream(g_mStream, mFile, 1024 * 8)
        End Using
    End Sub

    ''' <summary>
    ''' Exports the IO.Stream content to string.
    ''' </summary>
    ''' <returns></returns>
    Public Function ExportToString() As String
        g_mStream.Seek(0, IO.SeekOrigin.Begin)
        Return g_mStreamReader.ReadToEnd
    End Function

    ''' <summary>
    ''' Parses the file content to IO.Stream.
    ''' </summary>
    ''' <param name="sFile"></param>
    Public Sub ParseFromFile(sFile As String)
        g_mStream.Seek(0, IO.SeekOrigin.Begin)
        g_mStream.SetLength(0)

        Using mFile As New IO.FileStream(sFile, IO.FileMode.Open, IO.FileAccess.Read)
            CopyStream(mFile, g_mStream, 1024 * 8)
        End Using
    End Sub

    ''' <summary>
    ''' Parses the string content to IO.Stream.
    ''' </summary>
    ''' <param name="sText"></param>
    Public Sub ParseFromString(sText As String)
        g_mStreamWriter.BaseStream.Seek(0, IO.SeekOrigin.Begin)
        g_mStreamWriter.BaseStream.SetLength(0)
        g_mStreamWriter.Write(sText)
    End Sub

    Private Sub CopyStream(mInput As IO.Stream, mOutput As IO.Stream, iBufferSize As Integer)
        Dim iBuffer As Byte() = New Byte(iBufferSize - 1) {}
        Dim iBytesRead As Integer = 0

        Do
            iBytesRead = mInput.Read(iBuffer, 0, iBuffer.Length)
            mOutput.Write(iBuffer, 0, iBytesRead)
        Loop While iBytesRead > 0
    End Sub

    ''' <summary>
    ''' Gets the section name from the current line. 
    ''' </summary>
    ''' <param name="sLine">The line in the ini file.</param>
    ''' <returns>Returns the section name, otherwise nothing if not found.</returns>
    Private Function GetSectionFromLine(sLine As String) As String
        If (sLine.TrimStart.StartsWith("["c) AndAlso sLine.TrimEnd.EndsWith("]"c)) Then
            Return sLine.Trim.Trim("["c, "]"c)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Parses the current line into key=value.
    ''' </summary>
    ''' <param name="sLine">The line in the ini file.</param>
    ''' <returns>Return the current line as STRUC_INI_CONTENT, otherwise Nothing.</returns>
    Private Function GetKeyAndValueFromLine(sLine As String) As STRUC_INI_CONTENT
        sLine = sLine.Trim

        Dim iAssignIndex As Integer = sLine.IndexOf("="c)
        If (iAssignIndex < 0) Then
            Return Nothing
        End If

        Dim sKey As String = sLine.Substring(0, iAssignIndex)
        Dim sValue As String = sLine.Remove(0, iAssignIndex + 1)

        Return New STRUC_INI_CONTENT("", sKey, sValue)
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                If (g_mStreamWriter IsNot Nothing) Then
                    g_mStreamWriter.Dispose()
                    g_mStreamWriter = Nothing
                End If

                If (g_mStreamReader IsNot Nothing) Then
                    g_mStreamReader.Dispose()
                    g_mStreamReader = Nothing
                End If

                If (g_mStream IsNot Nothing) Then
                    g_mStream.Dispose()
                    g_mStream = Nothing
                End If
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null. 
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        'GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class