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



Public Class UCExportWizardMethod
    Private g_mFormExportWizard As FormExportWizard
    Private g_bIgnoreEvent As Boolean = True

    Public Sub New(mFormExportWizard As FormExportWizard)
        g_mFormExportWizard = mFormExportWizard

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler g_mFormExportWizard.OnNavigationPage, AddressOf OnNavigationPage

        UpdateControls()

        g_bIgnoreEvent = False
    End Sub

    Private Sub RadioButton_StoreSingleFile_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_StoreSingleFile.CheckedChanged
        If (g_bIgnoreEvent) Then
            Return
        End If

        If (Not RadioButton_StoreSingleFile.Checked) Then
            Return
        End If

        g_mFormExportWizard.m_ExportIsPacked = True
        UpdateControls()
    End Sub

    Private Sub RadioButton_StoreMultiFiles_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_StoreMultiFiles.CheckedChanged
        If (g_bIgnoreEvent) Then
            Return
        End If

        If (Not RadioButton_StoreMultiFiles.Checked) Then
            Return
        End If

        g_mFormExportWizard.m_ExportIsPacked = False
        UpdateControls()
    End Sub

    Private Sub UCExportWizardFileMode_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If (Me.Visible = False) Then
            Return
        End If

        g_mFormExportWizard.SetPageTitle("Phrases Export Method", "Specify the method how phrases are exported")
        UpdateControls()
    End Sub

    Private Sub OnNavigationPage(iDirection As Integer, iPage As FormExportWizard.ENUM_PAGES, ByRef bHandeled As Boolean)
        If (iPage <> FormExportWizard.ENUM_PAGES.PACK_SETTINGS) Then
            Return
        End If

        If (iDirection <> 1) Then
            Return
        End If

        g_mFormExportWizard.m_ExportFilesRemoval.Clear()
        g_mFormExportWizard.m_ExportFilesAdditional.Clear()

        If (g_mFormExportWizard.m_ExportIsPacked) Then
            If (Not CheckFormatOverwrites()) Then
                bHandeled = True
                Return
            End If

            If (Not CheckPackedObsoleteFiles()) Then
                bHandeled = True
                Return
            End If
        Else
            For Each sAdditionFile In GetAdditionalTranslationFiles()
                g_mFormExportWizard.m_ExportFilesAdditional.Add(sAdditionFile.ToLower)
            Next

            If (Not CheckFileOverwrites()) Then
                bHandeled = True
                Return
            End If

            If (Not CheckUnpackedObsoleteFiles()) Then
                bHandeled = True
                Return
            End If
        End If
    End Sub

    Private Function CheckFormatOverwrites() As Boolean
        For Each mTranslation In g_mFormExportWizard.m_ExportKeyValue
            Dim iFormatCount As Integer = 0

            For Each mItem In mTranslation.m_TranslationItems
                If (String.IsNullOrEmpty(mItem.m_Format)) Then
                    Continue For
                End If

                iFormatCount += 1
            Next

            If (iFormatCount > 1) Then
                Dim sMessage As New Text.StringBuilder
                sMessage.AppendFormat("Phrase group '{0}' has multiple formating rules! Single file translation files only support one formating rule.", mTranslation.m_Name).AppendLine()
                sMessage.AppendLine("If you need multiple formating rules, export your phrases into multiple files.")
                sMessage.AppendLine()
                sMessage.AppendLine("Click OK to ignore this warning and merge formating rules (will result in unexpected behaviours).")
                sMessage.AppendLine("Click CANCEL to cancel.")

                Select Case (MessageBox.Show(sMessage.ToString, "Multiple formating rules", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                    Case DialogResult.Cancel
                        Return False
                End Select
            End If
        Next

        Return True
    End Function

    Private Function CheckFileOverwrites() As Boolean
        Dim mOverwrittenFiles As New HashSet(Of String)

        For Each sFile As String In GetAdditionalTranslationFiles()
            Try
                If (Not IO.File.Exists(sFile)) Then
                    Continue For
                End If

                mOverwrittenFiles.Add(sFile)
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        Next

        If (mOverwrittenFiles.Count > 0) Then
            Using mOverwriteMessageBox As New FormFilesMessageBox("The following translation files already exist:", "Do you want to overwrite these existing translation files?", "Overwrite files", "Continue", mOverwrittenFiles.ToArray)
                Return (mOverwriteMessageBox.ShowDialog(Me) = DialogResult.OK)
            End Using
        End If

        Return True
    End Function

    Private Function CheckPackedObsoleteFiles() As Boolean
        Dim mUnusedFiles As New HashSet(Of String)

        For Each sFile As String In FindAdditionalTranslationFiles(g_mFormExportWizard.m_ExportFile)
            Try
                If (Not IO.File.Exists(sFile)) Then
                    Continue For
                End If

                mUnusedFiles.Add(sFile.ToLower)
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        Next

        If (mUnusedFiles.Count > 0) Then
            Using mOverwriteMessageBox As New FormFilesMessageBox("The following translation files are obsolete and should be removed:", "Do you want to remove these obsolete translation files?", "Obsolete files", "Continue", mUnusedFiles.ToArray)
                If (mOverwriteMessageBox.ShowDialog(Me) = DialogResult.OK) Then
                    For Each sRemoveFile In mUnusedFiles
                        g_mFormExportWizard.m_ExportFilesRemoval.Add(sRemoveFile.ToLower)
                    Next

                    Return True
                Else
                    Return False
                End If
            End Using
        End If

        Return True
    End Function

    Private Function CheckUnpackedObsoleteFiles() As Boolean
        Dim mUnusedFiles As New HashSet(Of String)

        For Each sFile As String In FindUnusedAdditionalTranslationFiles(g_mFormExportWizard.m_ExportFile)
            Try
                If (Not IO.File.Exists(sFile)) Then
                    Continue For
                End If

                mUnusedFiles.Add(sFile.ToLower)
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        Next

        If (mUnusedFiles.Count > 0) Then
            Using mOverwriteMessageBox As New FormFilesMessageBox("The following translation files are obsolete and should be removed:", "Do you want to remove these obsolete translation files?", "Obsolete files", "Continue", mUnusedFiles.ToArray)
                If (mOverwriteMessageBox.ShowDialog(Me) = DialogResult.OK) Then
                    For Each sRemoveFile In mUnusedFiles
                        g_mFormExportWizard.m_ExportFilesRemoval.Add(sRemoveFile.ToLower)
                    Next

                    Return True
                Else
                    Return False
                End If
            End Using
        End If

        Return True
    End Function

    Private Sub UpdateControls()
        If (g_mFormExportWizard.m_ExportIsPacked) Then
            RadioButton_StoreSingleFile.Checked = True
            RadioButton_StoreMultiFiles.Checked = False
        Else
            RadioButton_StoreSingleFile.Checked = False
            RadioButton_StoreMultiFiles.Checked = True
        End If

        If (g_mFormExportWizard.m_ExportIsPacked) Then
            Label_AdditionalFiles.Visible = False
            ListBox_AdditionalFiles.Visible = False
        Else
            Label_AdditionalFiles.Visible = True
            ListBox_AdditionalFiles.Visible = True

            Try
                ListBox_AdditionalFiles.BeginUpdate()
                ListBox_AdditionalFiles.Items.Clear()

                Try
                    For Each sFile In GetAdditionalTranslationFiles()
                        ListBox_AdditionalFiles.Items.Add(sFile)
                    Next
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)
                End Try
            Finally
                ListBox_AdditionalFiles.EndUpdate()
            End Try
        End If
    End Sub
    ''' <summary>
    ''' Find unused additional translation files.
    ''' </summary>
    ''' <param name="sFile"></param>
    ''' <returns></returns>
    Public Function FindUnusedAdditionalTranslationFiles(sFile As String) As String()
        Dim mFilesFS As New List(Of String)
        For Each sFileFS As String In FindAdditionalTranslationFiles(sFile)
            mFilesFS.Add(sFileFS.ToLower)
        Next

        Dim mFilesLNG As New List(Of String)
        For Each sFileLNG As String In GetAdditionalTranslationFiles()
            mFilesLNG.Add(sFileLNG.ToLower)
        Next

        Dim mFiles As New HashSet(Of String)

        For Each sFileFS As String In mFilesFS
            If (mFilesLNG.Contains(sFileFS)) Then
                Continue For
            End If

            mFiles.Add(sFileFS)
        Next

        Return mFiles.ToArray
    End Function

    ''' <summary>
    ''' Get additional translation files by filesystem.
    ''' </summary>
    ''' <param name="sFile"></param>
    ''' <returns></returns>
    Public Function FindAdditionalTranslationFiles(sFile As String) As String()
        If (String.IsNullOrEmpty(sFile)) Then
            Return New String() {}
        End If

        Dim mFiles As New List(Of String)

        'Has sub directories?
        Dim sFileName As String = IO.Path.GetFileName(sFile)
        Dim sFileDirectory As String = IO.Path.GetDirectoryName(sFile)

        For Each sDirectory In IO.Directory.GetDirectories(sFileDirectory)
            Dim sLang As String = New IO.DirectoryInfo(sDirectory).Name

            Dim sAdditionalFile As String = IO.Path.Combine(sDirectory, sFileName)
            If (Not IO.File.Exists(sAdditionalFile)) Then
                Continue For
            End If

            mFiles.Add(sAdditionalFile)
        Next

        Return mFiles.ToArray
    End Function

    ''' <summary>
    ''' Get additional translation files by language
    ''' </summary>
    ''' <returns></returns>
    Public Function GetAdditionalTranslationFiles() As String()
        If (String.IsNullOrEmpty(g_mFormExportWizard.m_ExportFile)) Then
            Return New String() {}
        End If

        Dim sLangauges As New HashSet(Of String)
        Dim sFiles As New List(Of String)

        For Each mTranslation In g_mFormExportWizard.m_ExportKeyValue
            For Each mItem In mTranslation.m_TranslationItems
                If (mItem.m_Language = "en") Then
                    Continue For
                End If

                sLangauges.Add(mItem.m_Language)
            Next
        Next

        Dim sFileDirectory As String = IO.Path.GetDirectoryName(g_mFormExportWizard.m_ExportFile)
        Dim sFileName As String = IO.Path.GetFileName(g_mFormExportWizard.m_ExportFile)

        For Each sLang As String In sLangauges
            Dim sSubDirectory As String = IO.Path.Combine(sFileDirectory, sLang)

            sFiles.Add(IO.Path.Combine(sSubDirectory, sFileName))
        Next

        Return sFiles.ToArray
    End Function

    Private Sub CleanUp()
        If (g_mFormExportWizard IsNot Nothing) Then
            RemoveHandler g_mFormExportWizard.OnNavigationPage, AddressOf OnNavigationPage
        End If
    End Sub
End Class
