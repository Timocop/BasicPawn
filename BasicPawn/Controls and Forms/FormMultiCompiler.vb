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


Imports System.Text.RegularExpressions

Public Class FormMultiCompiler
    Private g_mMainForm As FormMain

    Private g_sSourceFiles As String()
    Private g_bTestingOnly As Boolean = False
    Private g_bCleanDebuggerPlaceholder As Boolean = False

    Private g_mCompilerThread As Threading.Thread
    Private g_lCompiledFiles As New List(Of String)

    Public Sub New(f As FormMain, sSourceFiles As String(), bTestingOnly As Boolean, bCleanDebuggerPlaceholder As Boolean, Optional ByRef lCompiledFiles As List(Of String) = Nothing)
        g_mMainForm = f
        g_sSourceFiles = sSourceFiles
        g_bTestingOnly = bTestingOnly
        g_bCleanDebuggerPlaceholder = bCleanDebuggerPlaceholder
        lCompiledFiles = g_lCompiledFiles

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.  
        ClassControlStyle.SetNameFlag(Panel_FooterControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)
    End Sub

    Private Sub FormMultiCompiler_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)

        StartCompile()
    End Sub

    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub


    Public Sub StartCompile()
        If (ClassThread.IsValid(g_mCompilerThread)) Then
            Return
        End If

        g_lCompiledFiles.Clear()

        g_mCompilerThread = New Threading.Thread(AddressOf CompilerThread) With {
            .IsBackground = True
        }
        g_mCompilerThread.Start()
    End Sub

    Private Sub CompilerThread()
        Try
            ClassThread.ExecEx(Of Object)(ProgressBar_Compiled, Sub()
                                                                  ProgressBar_Compiled.Maximum = g_sSourceFiles.Length
                                                                  ProgressBar_Compiled.Value = 0
                                                              End Sub)

            For i = 0 To g_sSourceFiles.Length - 1
                Dim sSourceFile As String = g_sSourceFiles(i)
                Dim sSource As String = IO.File.ReadAllText(g_sSourceFiles(i))
                Dim sCompilerOutput As String = ""
                Dim iLanguage As ClassSyntaxTools.ENUM_LANGUAGE_TYPE = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN

                Dim mConfig = ClassConfigs.FindOptimalConfigForFile(sSourceFile, False, Nothing)
                If (mConfig Is Nothing) Then
                    Throw New ArgumentException(String.Format("Could not find config for file '{0}'.{1}Make sure BasicPawn can find a config for this file using 'Known files' or 'Default config paths' in configs.", sSourceFile, Environment.NewLine))
                End If

                'We have no other choices here. Get mod type by extension...
                If (mConfig.g_iLanguage = ClassConfigs.STRUC_CONFIG_ITEM.ENUM_LANGUAGE_DETECT_TYPE.AUTO_DETECT) Then
                    Select Case (IO.Path.GetExtension(sSourceFile).ToLower)
                        Case ".sp"
                            iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN

                        Case ".sma"
                            iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.AMXMODX

                        Case ".p", ".pwn"
                            iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.PAWN

                        Case Else
                            Select Case (MessageBox.Show(String.Format("Could not detect language from file '{0}'", sSourceFile), "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                                Case DialogResult.Cancel
                                    Throw New ArgumentException("Compiling canceled")
                            End Select
                    End Select
                Else
                    Select Case (mConfig.g_iLanguage)
                        Case ClassConfigs.STRUC_CONFIG_ITEM.ENUM_LANGUAGE_DETECT_TYPE.SOURCEPAWN
                            iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.SOURCEPAWN

                        Case ClassConfigs.STRUC_CONFIG_ITEM.ENUM_LANGUAGE_DETECT_TYPE.AMXMODX
                            iLanguage = ClassSyntaxTools.ENUM_LANGUAGE_TYPE.AMXMODX

                        Case Else
                            Throw New ArgumentException("Unknown mod type")
                    End Select
                End If


                Dim sOutputFile As String = IO.Path.Combine(mConfig.g_sOutputFolder, String.Format("{0}.unk", IO.Path.GetFileNameWithoutExtension(sSourceFile)))
                Dim bSuccess As Boolean = ClassThread.ExecEx(Of Boolean)(Me, Function()
                                                                                 If (g_bCleanDebuggerPlaceholder) Then
                                                                                     If (ClassDebuggerTools.ClassDebuggerHelpers.HasDebugPlaceholder(sSource)) Then
                                                                                         ClassDebuggerTools.ClassDebuggerHelpers.CleanupDebugPlaceholder(sSource, iLanguage)
                                                                                     End If
                                                                                 End If

                                                                                 Return g_mMainForm.g_ClassTextEditorTools.CompileSource(Nothing,
                                                                                                                                           sSourceFile,
                                                                                                                                           sSource,
                                                                                                                                           g_bTestingOnly,
                                                                                                                                           True,
                                                                                                                                           sOutputFile,
                                                                                                                                           mConfig,
                                                                                                                                           IO.Path.GetDirectoryName(sSourceFile),
                                                                                                                                           Nothing,
                                                                                                                                           Nothing,
                                                                                                                                           Nothing,
                                                                                                                                           Nothing,
                                                                                                                                           sSourceFile,
                                                                                                                                           True,
                                                                                                                                           sCompilerOutput)
                                                                             End Function)

                If (bSuccess AndAlso IO.File.Exists(sOutputFile)) Then
                    g_lCompiledFiles.Add(sOutputFile)
                End If

                Dim bWarning As Boolean = Regex.Match(sCompilerOutput, "^\s*[0-9]+\s+\b(Warning|Warnings)\b\.\s*$", RegexOptions.Multiline).Success

                Dim bCancel As Boolean = False

                If (Not bSuccess) Then
                    ClassThread.ExecEx(Of Object)(Me, Sub()
                                                          Try
                                                              With New Text.StringBuilder
                                                                  .AppendFormat("'{0}' failed to compile!", sSourceFile).AppendLine()
                                                                  .AppendLine("See information tab for more information.")
                                                                  .AppendLine()
                                                                  .AppendLine("Do you want to open the file now?")

                                                                  Select Case (MessageBox.Show(Me, .ToString, "Compiler failure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error))
                                                                      Case DialogResult.Yes
                                                                          Dim mTab = g_mMainForm.g_ClassTabControl.AddTab()
                                                                          mTab.OpenFileTab(sSourceFile)
                                                                          mTab.SelectTab()

                                                                      Case DialogResult.Cancel
                                                                          bCancel = True
                                                                  End Select
                                                              End With
                                                          Catch ex As Exception
                                                              ClassExceptionLog.WriteToLogMessageBox(ex)
                                                          End Try
                                                      End Sub)

                ElseIf (bWarning) Then
                    ClassThread.ExecEx(Of Object)(Me, Sub()
                                                          Try
                                                              With New Text.StringBuilder
                                                                  .AppendFormat("'{0}' has compiler warnings!", sSourceFile).AppendLine()
                                                                  .AppendLine("See information tab for more information.")
                                                                  .AppendLine()
                                                                  .AppendLine("Do you want to open the file now?")

                                                                  Select Case (MessageBox.Show(Me, .ToString, "Compiler warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
                                                                      Case DialogResult.Yes
                                                                          Dim mTab = g_mMainForm.g_ClassTabControl.AddTab()
                                                                          mTab.OpenFileTab(sSourceFile)
                                                                          mTab.SelectTab()

                                                                      Case DialogResult.Cancel
                                                                          bCancel = True
                                                                  End Select
                                                              End With
                                                          Catch ex As Exception
                                                              ClassExceptionLog.WriteToLogMessageBox(ex)
                                                          End Try
                                                      End Sub)
                End If

                If (bCancel) Then
                    Exit For
                End If

                ClassThread.ExecEx(Of Object)(ProgressBar_Compiled, Sub() ProgressBar_Compiled.Increment(1))
                ClassThread.ExecEx(Of Object)(Me, Sub() Me.Invalidate())
            Next

            ClassThread.ExecAsync(Me, Sub() Me.Close())
        Catch ex As Threading.ThreadAbortException
            Throw
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)

            ClassThread.ExecAsync(Me, Sub() Me.Close())
        End Try
    End Sub

    Private Sub FormMultiCompiler_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub CleanUp()
        ClassThread.Abort(g_mCompilerThread)
    End Sub
End Class