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



Partial Public Class FormTranslationEditor
    Private Sub ToolStripMenuItem_New_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_New.Click
        If (g_ClassTranslationManager.m_Changed) Then
            If (MessageBox.Show("All translation changes will be disregarded! Continue?", "Translation not saved", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = DialogResult.Cancel) Then
                Return
            End If
        End If

        g_ClassTranslationManager.CloseTranslation()

        g_ClassTranslationManager.TreeViewFillMissing(Not ToolStripMenuItem_ShowMissing.Checked)
    End Sub

    Private Sub ToolStripMenuItem_Import_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Import.Click
        Try
            If (g_ClassTranslationManager.m_Changed) Then
                If (MessageBox.Show("All translation changes will be disregarded! Continue?", "Translation not saved", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = DialogResult.Cancel) Then
                    Return
                End If
            End If

            Using i As New OpenFileDialog()
                i.Filter = "Translation files|*.phrases.txt; *.txt|All files|*.*"

                If (i.ShowDialog(Me) = DialogResult.OK) Then
                    Using j As New FormImportWizard(i.FileName, g_ClassTranslationManager.FindAdditionalFiles(i.FileName))
                        If (j.ShowDialog(Me) = DialogResult.OK) Then
                            g_ClassTranslationManager.LoadTranslation(i.FileName, (Not j.m_IsPacked))
                            g_ClassRecentTranslations.AddRecent(i.FileName)

                            g_ClassTranslationManager.TreeViewFillMissing(Not ToolStripMenuItem_ShowMissing.Checked)
                        End If
                    End Using
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_File_DropDownOpening(sender As Object, e As EventArgs) Handles ToolStripMenuItem_File.DropDownOpening
        Try
            For Each mToolItem In g_mRecentToolMenus
                If (mToolItem.IsDisposed) Then
                    Continue For
                End If

                RemoveHandler mToolItem.Click, AddressOf OnRecentToolStripItem_Click

                mToolItem.Dispose()
            Next

            g_mRecentToolMenus.Clear()

            Dim mRecentFiles = g_ClassRecentTranslations.GetRecent

            For i = mRecentFiles.Length - 1 To 0 Step -1
                Dim mToolItem As New ToolStripMenuItem(mRecentFiles(i).Key)

                g_mRecentToolMenus.Add(mToolItem)
                ToolStripMenuItem_ImportRecent.DropDownItems.Add(mToolItem)

                RemoveHandler mToolItem.Click, AddressOf OnRecentToolStripItem_Click
                AddHandler mToolItem.Click, AddressOf OnRecentToolStripItem_Click
            Next

            ClassControlStyle.UpdateControls(ToolStripMenuItem_File)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub OnRecentToolStripItem_Click(sender As Object, e As EventArgs)
        Try
            Dim mToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
            If (mToolStripMenuItem Is Nothing) Then
                Return
            End If

            Dim sFile As String = mToolStripMenuItem.Text
            If (String.IsNullOrEmpty(sFile) OrElse Not IO.File.Exists(sFile)) Then
                Throw New IO.FileNotFoundException("File not found")
            End If

            If (g_ClassTranslationManager.m_Changed) Then
                If (MessageBox.Show("All translation changes will be disregarded! Continue?", "Translation not saved", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = DialogResult.Cancel) Then
                    Return
                End If
            End If

            Using j As New FormImportWizard(sFile, g_ClassTranslationManager.FindAdditionalFiles(sFile))
                If (j.ShowDialog(Me) = DialogResult.OK) Then
                    g_ClassTranslationManager.LoadTranslation(sFile, (Not j.m_IsPacked))
                    g_ClassRecentTranslations.AddRecent(sFile)

                    g_ClassTranslationManager.TreeViewFillMissing(Not ToolStripMenuItem_ShowMissing.Checked)
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_Export_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Export.Click
        Try
            Using i As New FormExportWizard(g_ClassTranslationManager.m_File, g_ClassTranslationManager.m_IsPacked, g_ClassTranslationManager.m_Translations.ToArray)
                If (i.ShowDialog(Me) = DialogResult.OK) Then
                    For Each sRemoveFile In i.m_ExportFilesRemoval
                        Try
                            IO.File.Delete(sRemoveFile)
                        Catch ex As Exception
                            ClassExceptionLog.WriteToLogMessageBox(ex)
                        End Try
                    Next

                    If (i.m_ExportIsPacked) Then
                        g_ClassTranslationManager.SaveTranslationSingle(i.m_ExportFile)
                    Else
                        g_ClassTranslationManager.SaveTranslationMulti(i.m_ExportFile)
                    End If

                    g_ClassRecentTranslations.AddRecent(i.m_ExportFile)
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
