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

    Property m_ExportIsPacked As Boolean
        Get
            Return g_mFormExportWizard.m_ExportIsPacked
        End Get
        Set(value As Boolean)
            g_mFormExportWizard.m_ExportIsPacked = value

            UpdateControls()
        End Set
    End Property

    Private Sub RadioButton_StoreSingleFile_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_StoreSingleFile.CheckedChanged
        If (g_bIgnoreEvent) Then
            Return
        End If

        If (Not RadioButton_StoreSingleFile.Checked) Then
            Return
        End If

        m_ExportIsPacked = True
    End Sub

    Private Sub RadioButton_StoreMultiFiles_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton_StoreMultiFiles.CheckedChanged
        If (g_bIgnoreEvent) Then
            Return
        End If

        If (Not RadioButton_StoreMultiFiles.Checked) Then
            Return
        End If

        m_ExportIsPacked = False
    End Sub

    Private Sub UCExportWizardFileMode_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If (Me.Visible = False) Then
            Return
        End If

        g_mFormExportWizard.SetPageTitle("Translations export method", "Specify the method how translations are exported")
        UpdateControls()
    End Sub

    Private Sub OnNavigationPage(iDirection As Integer, iPage As FormExportWizard.ENUM_PAGES, ByRef bHandeled As Boolean)
        If (iPage <> FormExportWizard.ENUM_PAGES.PACK_SETTINGS) Then
            Return
        End If

        If (iDirection <> 1) Then
            Return
        End If

        If (m_ExportIsPacked) Then
            If (Not CheckFormatOverwrites()) Then
                bHandeled = True
                Return
            End If

            If (Not CheckObsoleteFiles()) Then
                bHandeled = True
                Return
            End If
        Else
            If (Not CheckFileOverwrites()) Then
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
                sMessage.AppendFormat("Translation group '{0}' has multiple formating rules! Single file translation files only support one formating rule.", mTranslation.m_Name).AppendLine()
                sMessage.AppendLine("If you need multiple formating rules, export your translations into multiple files.")
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

        For Each sFile As String In g_mFormExportWizard.GetAdditionalTranslationFiles
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
            Using mOverwriteMessageBox As New FormFilesMessageBox("The following translation files already exist:", "Do you want to overwrite these existing translation files?", "Overwrite files", "Overwrite", mOverwrittenFiles.ToArray)
                Return (mOverwriteMessageBox.ShowDialog(Me) = DialogResult.OK)
            End Using
        End If

        Return True
    End Function

    Private Function CheckObsoleteFiles() As Boolean
        Dim mUnusedFiles As New HashSet(Of String)

        For Each sFile As String In g_mFormExportWizard.GetAdditionalTranslationFiles
            Try
                If (Not IO.File.Exists(sFile)) Then
                    Continue For
                End If

                mUnusedFiles.Add(sFile)
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        Next

        If (mUnusedFiles.Count > 0) Then
            Using mOverwriteMessageBox As New FormFilesMessageBox("The following translation files are obsolete and should be removed:", "Do you want to remove these obsolete translation files?", "Obsolete files", "Remove", mUnusedFiles.ToArray)
                If (mOverwriteMessageBox.ShowDialog(Me) = DialogResult.OK) Then
                    Try
                        For Each sFile In mUnusedFiles
                            If (Not IO.File.Exists(sFile)) Then
                                Continue For
                            End If

                            IO.File.Delete(sFile)
                        Next
                    Catch ex As Exception
                        ClassExceptionLog.WriteToLogMessageBox(ex)
                    End Try

                    Return True
                Else
                    Return False
                End If
            End Using
        End If

        Return True
    End Function

    Private Sub UpdateControls()
        If (m_ExportIsPacked) Then
            RadioButton_StoreSingleFile.Checked = True
            RadioButton_StoreMultiFiles.Checked = False
        Else
            RadioButton_StoreSingleFile.Checked = False
            RadioButton_StoreMultiFiles.Checked = True
        End If

        If (m_ExportIsPacked) Then
            Label_AdditionalFiles.Visible = False
            ListBox_AdditionalFiles.Visible = False
        Else
            Label_AdditionalFiles.Visible = True
            ListBox_AdditionalFiles.Visible = True

            Try
                ListBox_AdditionalFiles.BeginUpdate()
                ListBox_AdditionalFiles.Items.Clear()

                Try
                    For Each sFile In g_mFormExportWizard.GetAdditionalTranslationFiles
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

    Private Sub CleanUp()
        If (g_mFormExportWizard IsNot Nothing) Then
            RemoveHandler g_mFormExportWizard.OnNavigationPage, AddressOf OnNavigationPage
        End If
    End Sub
End Class
