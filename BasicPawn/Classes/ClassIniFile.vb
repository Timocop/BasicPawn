Imports System.Runtime.InteropServices
Imports System.Text

Public Class ClassIniFile
    Structure STRUC_INI_CONTENT
        Dim sSection As String
        Dim sKey As String
        Dim sValue As String
    End Structure

    Private g_sIniPath As String = ""

    ''' <summary>
    ''' Open a ini file
    ''' </summary>
    ''' <param name="sFile">The ini file path</param>
    Public Sub New(sFile As String)
        g_sIniPath = sFile
    End Sub

    ''' <summary>
    ''' Reads a key value.
    ''' </summary>
    ''' <param name="sFromSection">The section.</param>
    ''' <param name="sFromKey">The key.</param>
    ''' <returns>The value from the key, otherwise Nothing.</returns>
    Public Function ReadKeyValue(sFromSection As String, sFromKey As String) As String
        Try
            If (IO.File.Exists(g_sIniPath)) Then
                For Each iContent In ReadEverything()
                    If (iContent.sSection = sFromSection AndAlso iContent.sKey = sFromKey) Then
                        Return iContent.sValue
                    End If
                Next
            End If
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
            If (IO.File.Exists(g_sIniPath)) Then
                For Each iContent In ReadEverything()
                    If (iContent.sSection = sFromSection AndAlso iContent.sKey = sFromKey) Then
                        Return iContent.sValue
                    End If
                Next
            End If
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
        For Each iContent In ReadEverything()
            If (iContent.sSection = sFromSection AndAlso iContent.sKey = sFromKey) Then
                Return iContent.sValue
            End If
        Next

        Throw New ArgumentException("Can't find INI section or key")
    End Function

    Public Sub WriteKeyValue(sFromSection As String, sFromKey As String, Optional sFromValue As String = Nothing)
        If (Not IO.File.Exists(g_sIniPath)) Then
            IO.File.WriteAllText(g_sIniPath, "")
        End If

        Dim lIniContentList As New List(Of STRUC_INI_CONTENT)
        lIniContentList.AddRange(ReadEverything())

        Dim bFoundContent As Boolean = False

        While True
            For i = 0 To lIniContentList.Count - 1
                If (lIniContentList(i).sSection = sFromSection AndAlso lIniContentList(i).sKey = sFromKey) Then
                    If (sFromValue Is Nothing) Then
                        lIniContentList.RemoveAt(i)
                        Continue While
                    Else
                        Dim iNewContent As New STRUC_INI_CONTENT
                        iNewContent.sSection = sFromSection
                        iNewContent.sKey = sFromKey
                        iNewContent.sValue = sFromValue
                        lIniContentList(i) = iNewContent

                        bFoundContent = True
                    End If
                End If
            Next

            Exit While
        End While

        If (Not bFoundContent AndAlso sFromValue IsNot Nothing) Then
            Dim iNewContent As New STRUC_INI_CONTENT
            iNewContent.sSection = sFromSection
            iNewContent.sKey = sFromKey
            iNewContent.sValue = sFromValue
            lIniContentList.Add(iNewContent)
        End If

        ' Process write to file
        Dim lSectionsList As New List(Of String)
        For i = 0 To lIniContentList.Count - 1
            If (lSectionsList.Contains(lIniContentList(i).sSection)) Then
                Continue For
            End If

            lSectionsList.Add(lIniContentList(i).sSection)
        Next

        ' Sort items and write to file
        Dim SB As New StringBuilder
        For Each sSectionListItem In lSectionsList
            SB.AppendLine("[" & sSectionListItem & "]")

            For Each iContentListItem In lIniContentList
                If (iContentListItem.sSection = sSectionListItem) Then
                    SB.AppendLine(iContentListItem.sKey & "=" & iContentListItem.sValue)
                End If
            Next
        Next

        IO.File.WriteAllText(g_sIniPath, SB.ToString)
    End Sub

    ''' <summary>
    ''' Gets all section names.
    ''' </summary>
    ''' <returns>All section names.</returns>
    Public Function GetSectionNames() As String()
        Dim iIniContent = ReadEverything()

        Dim lSectionsList As New List(Of String)
        For i = 0 To iIniContent.Length - 1
            If (lSectionsList.Contains(iIniContent(i).sSection)) Then
                Continue For
            End If

            lSectionsList.Add(iIniContent(i).sSection)
        Next

        Return lSectionsList.ToArray
    End Function

    ''' <summary>
    ''' Parses the ini file and reads everything.
    ''' </summary>
    ''' <returns></returns>
    Public Function ReadEverything() As STRUC_INI_CONTENT()
        Dim lIniContentList As New List(Of STRUC_INI_CONTENT)

        Dim sCurrentSection As String = ""

        Using SR As New IO.StreamReader(g_sIniPath)
            While True
                Dim sLine As String = SR.ReadLine
                If (sLine Is Nothing) Then
                    Exit While
                End If

                Dim sSection As String = GetSectionFromLine(sLine)
                If (sSection IsNot Nothing) Then
                    sCurrentSection = sSection
                End If

                Dim sContent As STRUC_INI_CONTENT = GetKeyAndValueFromLine(sLine)
                If (sContent.sSection Is Nothing) Then
                    Continue While
                End If
                sContent.sSection = sCurrentSection

                lIniContentList.Add(sContent)
            End While
        End Using

        Return lIniContentList.ToArray
    End Function

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

        Dim iIniContent As New STRUC_INI_CONTENT

        Dim iAssignIndex As Integer = sLine.IndexOf("="c)
        If (iAssignIndex < 0) Then
            Return Nothing
        End If

        Dim sKey As String = sLine.Substring(0, iAssignIndex)
        Dim sValue As String = sLine.Remove(0, iAssignIndex + 1)

        iIniContent.sSection = ""
        iIniContent.sKey = sKey
        iIniContent.sValue = sValue

        Return iIniContent
    End Function
End Class
