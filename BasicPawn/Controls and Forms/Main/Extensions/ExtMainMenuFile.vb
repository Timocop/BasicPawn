'BasicPawn
'Copyright(C) 2018 TheTimocop

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


Partial Public Class FormMain
    Private Sub ToolStripMenuItem_FileNew_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileNew.Click
        g_ClassTabControl.AddTab(True, False, False, True)

        g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User created a new source file")
    End Sub

    Private Sub ToolStripMenuItem_FileNewWizard_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileNewWizard.Click
        g_ClassTabControl.AddTab(True, True, False, True)

        g_mUCInformationList.PrintInformation(ClassInformationListBox.ENUM_ICONS.ICO_INFO, "User created a new source file")
    End Sub

    Private Sub ToolStripMenuItem_FileProjectSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileProjectSave.Click
        Try
            Using i As New SaveFileDialog
                i.Filter = String.Format("BasicPawn Project|*{0}", UCProjectBrowser.ClassProjectControl.g_sProjectExtension)

                i.InitialDirectory = If(String.IsNullOrEmpty(g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile), "", IO.Path.GetDirectoryName(g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile))
                i.FileName = IO.Path.GetFileName(g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile)

                If (i.ShowDialog = DialogResult.OK) Then
                    g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile = i.FileName
                Else
                    Return
                End If
            End Using

            g_mUCProjectBrowser.g_ClassProjectControl.SaveProject()

            g_mUCStartPage.g_mClassRecentItems.AddRecent(g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileProjectLoad_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileProjectLoad.Click
        Try
            Using i As New OpenFileDialog
                i.Filter = String.Format("BasicPawn Project|*{0}", UCProjectBrowser.ClassProjectControl.g_sProjectExtension)
                i.Multiselect = True

                i.InitialDirectory = If(String.IsNullOrEmpty(g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile), "", IO.Path.GetDirectoryName(g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile))
                i.FileName = IO.Path.GetFileName(g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile)

                If (i.ShowDialog = DialogResult.OK) Then
                    Dim bAppendFiles As Boolean = False

                    g_ClassTabControl.RemoveAllTabs()

                    For Each sProjectFile As String In i.FileNames
                        g_mUCProjectBrowser.g_ClassProjectControl.m_ProjectFile = sProjectFile
                        g_mUCProjectBrowser.g_ClassProjectControl.LoadProject(bAppendFiles, ClassSettings.g_iSettingsAutoOpenProjectFiles)
                        bAppendFiles = True
                    Next
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileProjectClose_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileProjectClose.Click
        Try
            g_mUCProjectBrowser.g_ClassProjectControl.CloseProject()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileClose_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileClose.Click
        Try
            g_ClassTabControl.RemoveTab(g_ClassTabControl.m_ActiveTabIndex, True)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileCloseAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileCloseAll.Click
        Try
            g_ClassTabControl.RemoveAllTabs()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileOpen_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileOpen.Click
        Try
            Using i As New OpenFileDialog
                i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|AMX Mod X|*.sma|Pawn (Not fully supported)|*.pwn;*.p|All files|*.*"
                i.Multiselect = True

                i.InitialDirectory = If(String.IsNullOrEmpty(g_ClassTabControl.m_ActiveTab.m_File), "", IO.Path.GetDirectoryName(g_ClassTabControl.m_ActiveTab.m_File))
                i.FileName = IO.Path.GetFileName(g_ClassTabControl.m_ActiveTab.m_File)

                If (i.ShowDialog = DialogResult.OK) Then
                    Try
                        g_ClassTabControl.BeginUpdate()

                        For Each sFile As String In i.FileNames
                            Try
                                Dim mTab = g_ClassTabControl.AddTab()
                                mTab.OpenFileTab(sFile)
                                mTab.SelectTab(ClassTabControl.DEFAULT_SELECT_TAB_DELAY)
                            Catch ex As Exception
                                ClassExceptionLog.WriteToLogMessageBox(ex)
                            End Try
                        Next
                    Finally
                        g_ClassTabControl.EndUpdate()
                    End Try
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSave.Click
        Try
            g_ClassTabControl.SaveFileTab(g_ClassTabControl.m_ActiveTabIndex)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAll.Click
        For i = g_ClassTabControl.m_TabsCount - 1 To 0 Step -1
            Try
                g_ClassTabControl.SaveFileTab(i)
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
            End Try
        Next
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAs.Click
        Try
            g_ClassTabControl.SaveFileTab(g_ClassTabControl.m_ActiveTabIndex, True)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAsTemp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAsTemp.Click
        Try
            Dim sTempFile As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
            IO.File.WriteAllText(sTempFile, "")

            g_ClassTabControl.m_ActiveTab.m_File = sTempFile
            g_ClassTabControl.SaveFileTab(g_ClassTabControl.m_ActiveTabIndex)

            g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileSavePacked_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSavePacked.Click
        Try
            Dim sSource As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent

            If (ClassDebuggerTools.ClassDebuggerHelpers.HasDebugPlaceholder(sSource)) Then
                ClassDebuggerTools.ClassDebuggerHelpers.CleanupDebugPlaceholder(sSource, g_ClassTabControl.m_ActiveTab.m_Language)
            End If

            Dim sSourceFile As String = Nothing
            If (Not g_ClassTabControl.m_ActiveTab.m_IsUnsaved AndAlso Not g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                sSourceFile = g_ClassTabControl.m_ActiveTab.m_File
            End If

            Dim sTempFile As String = ""
            Dim sPreSource As String = g_ClassTextEditorTools.GetCompilerPreProcessCode(Nothing, Nothing, sSource, True, True, sTempFile, Nothing, If(sSourceFile Is Nothing, Nothing, IO.Path.GetDirectoryName(sSourceFile)), Nothing, Nothing, sSourceFile)
            If (String.IsNullOrEmpty(sPreSource)) Then
                MessageBox.Show("Could not export packed source. See information tab for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            If (String.IsNullOrEmpty(sTempFile)) Then
                Throw New ArgumentException("Last Pre-Process source invalid")
            End If

            With New ClassDebuggerRunner.ClassPreProcess(Nothing)
                .FixPreProcessFiles(sPreSource)
            End With

            Using i As New SaveFileDialog
                i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|AMX Mod X|*.sma|Pawn (Not fully supported)|*.pwn;*.p|All files|*.*"

                Dim sDialogPath As String = IO.Path.Combine(IO.Path.GetDirectoryName(g_ClassTabControl.m_ActiveTab.m_File), String.Format("{0}.packed{1}", IO.Path.GetFileNameWithoutExtension(g_ClassTabControl.m_ActiveTab.m_File), IO.Path.GetExtension(g_ClassTabControl.m_ActiveTab.m_File)))

                i.InitialDirectory = If(String.IsNullOrEmpty(sDialogPath), "", IO.Path.GetDirectoryName(sDialogPath))
                i.FileName = IO.Path.GetFileName(sDialogPath)

                If (i.ShowDialog = DialogResult.OK) Then
                    IO.File.WriteAllText(i.FileName, sPreSource)
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileLoadTabs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileLoadTabs.Click
        If (g_mFormInstanceManager IsNot Nothing AndAlso Not g_mFormInstanceManager.IsDisposed) Then
            Return
        End If

        g_mFormInstanceManager = New FormInstanceManager(Me)
        g_mFormInstanceManager.Show(Me)
    End Sub

    Private Sub ToolStripMenuItem_FileStartPage_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileStartPage.Click
        'Disable IntelliSense tooltip when StartPage is showing.
        g_mUCAutocomplete.UpdateAutocomplete("")
        g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()

        g_mUCStartPage.Show()
    End Sub

    Private Sub ToolStripMenuItem_FileOpenFolder_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileOpenFolder.Click
        Try
            If (g_ClassTabControl.m_ActiveTab.m_IsUnsaved OrElse g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                MessageBox.Show("Could not open current folder. Source file does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Process.Start("explorer.exe", String.Format("/select,""{0}""", g_ClassTabControl.m_ActiveTab.m_File))
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileExit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileExit.Click
        Me.Close()
    End Sub
End Class
