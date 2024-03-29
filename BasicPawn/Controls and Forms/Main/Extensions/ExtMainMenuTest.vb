﻿'BasicPawn
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


Imports System.Text.RegularExpressions

Partial Public Class FormMain
    Private Sub ToolStripMenuItem_TestCurrent_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TestCurrent.Click
        Try
            Using mProgress As New FormProgress
                mProgress.Text = "Testing..."
                mProgress.Show(Me)
                mProgress.m_Progress = 0

                Dim sSource As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent

                If (ClassDebuggerTools.ClassDebuggerHelpers.HasDebugPlaceholder(sSource)) Then
                    ClassDebuggerTools.ClassDebuggerHelpers.CleanupDebugPlaceholder(sSource, g_ClassTabControl.m_ActiveTab.m_Language)
                End If

                Dim sSourceFile As String = Nothing
                If (Not g_ClassTabControl.m_ActiveTab.m_IsUnsaved AndAlso Not g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                    sSourceFile = g_ClassTabControl.m_ActiveTab.m_File
                End If

                Dim sOutputFile As String = ""
                g_ClassTextEditorTools.CompileSource(Nothing,
                                                     Nothing,
                                                     sSource,
                                                     True,
                                                     True,
                                                     sOutputFile,
                                                     Nothing,
                                                     If(sSourceFile Is Nothing, Nothing, IO.Path.GetDirectoryName(sSourceFile)),
                                                     Nothing,
                                                     Nothing,
                                                     Nothing,
                                                     Nothing,
                                                     sSourceFile)

                mProgress.m_Progress = 100
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_TestAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TestAll.Click
        Try
            Using mProgress As New FormProgress
                mProgress.Text = "Testing..."
                mProgress.Show(Me)
                mProgress.m_ProgressMax = g_ClassTabControl.m_TabsCount
                mProgress.m_Progress = 0

                For Each mTab In g_ClassTabControl.GetAllTabs
                    Dim sSource As String = mTab.m_TextEditor.Document.TextContent

                    If (ClassDebuggerTools.ClassDebuggerHelpers.HasDebugPlaceholder(sSource)) Then
                        ClassDebuggerTools.ClassDebuggerHelpers.CleanupDebugPlaceholder(sSource, mTab.m_Language)
                    End If

                    Dim sSourceFile As String = Nothing
                    If (Not mTab.m_IsUnsaved AndAlso Not mTab.m_InvalidFile) Then
                        sSourceFile = mTab.m_File
                    End If

                    Dim sOutputFile As String = ""
                    Dim sCompilerOutput As String = ""
                    Dim bSuccess = g_ClassTextEditorTools.CompileSource(mTab,
                                                                        Nothing,
                                                                        sSource,
                                                                        True,
                                                                        True,
                                                                        sOutputFile,
                                                                        mTab.m_ActiveConfig,
                                                                        If(sSourceFile Is Nothing, Nothing, IO.Path.GetDirectoryName(sSourceFile)),
                                                                        Nothing,
                                                                        Nothing,
                                                                        Nothing,
                                                                        Nothing,
                                                                        sSourceFile,
                                                                        True,
                                                                        sCompilerOutput)

                    Dim bWarning As Boolean = Regex.Match(sCompilerOutput, "^\s*[0-9]+\s+\b(Warning|Warnings)\b\.\s*$", RegexOptions.Multiline).Success

                    If (Not bSuccess) Then
                        With New Text.StringBuilder
                            .AppendFormat("'{0}' failed to compile!", If(sSourceFile, "Unnamed")).AppendLine()
                            .AppendLine("See information tab for more information.")
                            .AppendLine()
                            .AppendLine("Do you want to switch to the tab now?")

                            Select Case (MessageBox.Show(Me, .ToString, "Compiler failure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error))
                                Case DialogResult.Yes
                                    mTab.SelectTab()

                                Case DialogResult.Cancel
                                    Exit For
                            End Select
                        End With

                    ElseIf (bWarning) Then
                        With New Text.StringBuilder
                            .AppendFormat("'{0}' has compiler warnings!", If(sSourceFile, "Unnamed")).AppendLine()
                            .AppendLine("See information tab for more information.")
                            .AppendLine()
                            .AppendLine("Do you want to switch to the tab now?")

                            Select Case (MessageBox.Show(Me, .ToString, "Compiler warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
                                Case DialogResult.Yes
                                    mTab.SelectTab()

                                Case DialogResult.Cancel
                                    Exit For
                            End Select
                        End With
                    End If

                    mProgress.m_Progress += 1
                Next

                mProgress.m_Progress = g_ClassTabControl.m_TabsCount
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_TestWith_DropDownOpening(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TestWith.DropDownOpening
        For i = ToolStripMenuItem_TestWith.DropDownItems.Count - 1 To 0 Step -1
            ToolStripMenuItem_TestWith.DropDownItems(i).Dispose()
        Next

        For Each mConfig In ClassConfigs.GetConfigs
            If (mConfig.IsDefault) Then
                Continue For
            End If

            ToolStripMenuItem_TestWith.DropDownItems.Add(mConfig.GetName, Nothing, AddressOf ToolStripMenuItem_TestWithSubs_Click)
        Next

        ClassControlStyle.UpdateControls(ToolStripMenuItem_TestWith)
    End Sub

    Private Sub ToolStripMenuItem_TestWithSubs_Click(sender As Object, e As EventArgs)
        Dim mToolStripItem As ToolStripItem = DirectCast(sender, ToolStripItem)

        Try
            Using mProgress As New FormProgress
                mProgress.Text = "Testing..."
                mProgress.Show(Me)
                mProgress.m_Progress = 0

                Dim mConfig = ClassConfigs.FindConfig(mToolStripItem.Text)
                If (mConfig Is Nothing) Then
                    Throw New ArgumentException("Unable to find config '" & mToolStripItem.Text & "'")
                End If

                Dim sSource As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent

                If (ClassDebuggerTools.ClassDebuggerHelpers.HasDebugPlaceholder(sSource)) Then
                    ClassDebuggerTools.ClassDebuggerHelpers.CleanupDebugPlaceholder(sSource, g_ClassTabControl.m_ActiveTab.m_Language)
                End If

                Dim sSourceFile As String = Nothing
                If (Not g_ClassTabControl.m_ActiveTab.m_IsUnsaved AndAlso Not g_ClassTabControl.m_ActiveTab.m_InvalidFile) Then
                    sSourceFile = g_ClassTabControl.m_ActiveTab.m_File
                End If

                Dim sOutputFile As String = ""
                g_ClassTextEditorTools.CompileSource(Nothing,
                                                     Nothing,
                                                     sSource,
                                                     True,
                                                     True,
                                                     sOutputFile,
                                                     mConfig,
                                                     If(sSourceFile Is Nothing, Nothing, IO.Path.GetDirectoryName(sSourceFile)),
                                                     Nothing,
                                                     Nothing,
                                                     Nothing,
                                                     Nothing,
                                                     sSourceFile)

                mProgress.m_Progress = 100
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_TestDebug_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TestDebug.Click
        Try
            If (g_mFormDebugger Is Nothing OrElse g_mFormDebugger.IsDisposed) Then
                Try
                    g_mFormDebugger = New FormDebugger(Me, g_ClassTabControl.m_ActiveTab)
                    g_mFormDebugger.Show()
                Catch ex As Exception
                    ClassExceptionLog.WriteToLogMessageBox(ex)

                    If (g_mFormDebugger IsNot Nothing AndAlso Not g_mFormDebugger.IsDisposed) Then
                        g_mFormDebugger.Dispose()
                        g_mFormDebugger = Nothing
                    End If
                End Try
            Else
                If (g_mFormDebugger.WindowState = FormWindowState.Minimized) Then
                    ClassTools.ClassForms.FormWindowCommand(g_mFormDebugger, ClassTools.ClassForms.NativeWinAPI.ShowWindowCommands.Restore)
                End If

                g_mFormDebugger.Activate()
            End If
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class
