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


Imports System.Text

Public Class ClassIniFileMem
    Structure STRUC_INI_CONTENT
        Dim sSection As String
        Dim sKey As String
        Dim sValue As String
    End Structure

    Private g_sIniContent As String = ""

    Public Property m_sIniContent As String
        Get
            Return g_sIniContent
        End Get
        Set(value As String)
            g_sIniContent = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(sContent As String)
        g_sIniContent = sContent
    End Sub

    ''' <summary>
    ''' Reads a key value.
    ''' </summary>
    ''' <param name="sFromSection">The section.</param>
    ''' <param name="sFromKey">The key.</param>
    ''' <returns>The value from the key, otherwise Nothing.</returns>
    Public Function ReadKeyValue(sFromSection As String, sFromKey As String) As String
        Try
            For Each iContent In ReadEverything()
                If (iContent.sSection = sFromSection AndAlso iContent.sKey = sFromKey) Then
                    Return iContent.sValue
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
            For Each iContent In ReadEverything()
                If (iContent.sSection = sFromSection AndAlso iContent.sKey = sFromKey) Then
                    Return iContent.sValue
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
        For Each iContent In ReadEverything()
            If (iContent.sSection = sFromSection AndAlso iContent.sKey = sFromKey) Then
                Return iContent.sValue
            End If
        Next

        Throw New ArgumentException("Can't find INI section or key")
    End Function

    Public Sub WriteKeyValue(sFromSection As String, sFromKey As String, Optional sFromValue As String = Nothing)
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
                        lIniContentList(i) = New STRUC_INI_CONTENT With {
                            .sSection = sFromSection,
                            .sKey = sFromKey,
                            .sValue = sFromValue
                        }

                        bFoundContent = True
                    End If
                End If
            Next

            Exit While
        End While

        If (Not bFoundContent AndAlso sFromValue IsNot Nothing) Then
            lIniContentList.Add(New STRUC_INI_CONTENT With {
                .sSection = sFromSection,
                .sKey = sFromKey,
                .sValue = sFromValue
            })
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
            SB.AppendLine(String.Format("[{0}]", sSectionListItem))

            For Each iContentListItem In lIniContentList
                If (iContentListItem.sSection = sSectionListItem) Then
                    SB.AppendLine(String.Format("{0}={1}", iContentListItem.sKey, iContentListItem.sValue))
                End If
            Next
        Next

        g_sIniContent = SB.ToString
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

        Using SR As New IO.StringReader(g_sIniContent)
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
