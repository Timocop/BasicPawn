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


Imports System.Windows.Forms
Imports BasicPawn
Imports ICSharpCode.TextEditor

Public Class FormReportDetails
    Public g_mFormReportManager As FormReportManager
    Public g_mException As ClassDebuggerParser.STRUC_SM_EXCEPTION

    Public Sub New(mFormReportManager As FormReportManager, mException As ClassDebuggerParser.STRUC_SM_EXCEPTION)
        g_mFormReportManager = mFormReportManager
        g_mException = mException

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Panel2.Name &= "@FooterDarkControl"
        Panel4.Name &= "@FooterDarkControl"

        Me.Text = mException.sExceptionInfo

        Label_ExceptionName.Text = mException.sExceptionInfo
        Label_FileName.Text = mException.sBlamingFile
        Label_Date.Text = mException.dLogDate.ToLongDateString & " - " & mException.dLogDate.ToShortTimeString

        ListView_StackTrace.BeginUpdate()
        For i = 0 To mException.mStackTraces.Length - 1
            Dim mListViewItemData As New ClassListViewItemData(New String() {
                                                         CStr(i),
                                                         CStr(mException.mStackTraces(i).iLine),
                                                         mException.mStackTraces(i).sFileName,
                                                         mException.mStackTraces(i).sFunctionName})

            mListViewItemData.g_mData("Index") = i
            mListViewItemData.g_mData("Line") = mException.mStackTraces(i).iLine
            mListViewItemData.g_mData("File") = mException.mStackTraces(i).sFileName
            mListViewItemData.g_mData("FunctionName") = mException.mStackTraces(i).sFunctionName

            ListView_StackTrace.Items.Add(mListViewItemData)
        Next
        ListView_StackTrace.EndUpdate()

        For i = 0 To mException.mStackTraces.Length - 1
            If (IO.File.Exists(mException.mStackTraces(i).sFileName)) Then
                Dim dFileDate As Date = New IO.FileInfo(mException.mStackTraces(i).sFileName).LastWriteTime

                If (dFileDate > mException.dLogDate) Then
                    m_WarningText = "Some source files are more recent than the report. Stack traces might be inaccurate."
                End If
            Else
                m_WarningText = "Unable to find some source files."
            End If
        Next
    End Sub

    Private Sub FormReportDetails_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub Button_Close_Click(sender As Object, e As EventArgs) Handles Button_Close.Click
        Me.Close()
    End Sub

    Property m_WarningText As String
        Get
            Return Label_Warning.Text
        End Get
        Set(value As String)
            If (String.IsNullOrEmpty(value)) Then
                Label_Warning.Text = ""
                Label_Warning.Visible = False
                PictureBox_Warning.Visible = False

                Return
            End If

            Label_Warning.Text = value
            Label_Warning.Visible = True
            PictureBox_Warning.Visible = True
        End Set
    End Property

    Private Sub ListView_StackTrace_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_StackTrace.SelectedIndexChanged
        If (ListView_StackTrace.SelectedItems.Count < 1) Then
            Return
        End If

        Try
            Dim mListViewItemData = TryCast(ListView_StackTrace.SelectedItems(0), ClassListViewItemData)
            If (mListViewItemData Is Nothing) Then
                Return
            End If

            Dim sLine As String = CStr(mListViewItemData.g_mData("Line"))
            Dim sFile As String = CStr(mListViewItemData.g_mData("File"))

            If (String.IsNullOrEmpty(sFile)) Then
                Return
            End If

            Dim bGuessTab As Boolean = (Not IO.File.Exists(sFile))

            Dim mFormMain = g_mFormReportManager.g_mPluginAutoErrorReport.g_mFormMain

            Dim bForceEnd As Boolean = False
            While True
                For i = 0 To mFormMain.g_ClassTabControl.m_TabsCount - 1
                    If (mFormMain.g_ClassTabControl.m_Tab(i).m_IsUnsaved OrElse mFormMain.g_ClassTabControl.m_Tab(i).m_InvalidFile) Then
                        Continue For
                    End If

                    Dim sTabPath As String = mFormMain.g_ClassTabControl.m_Tab(i).m_File.Replace("/"c, "\"c)

                    If (sTabPath.ToLower = sFile.ToLower OrElse (bGuessTab AndAlso sTabPath.ToLower.EndsWith(sFile.ToLower))) Then
                        Dim iLineNum As Integer = CInt(sLine) - 1
                        If (iLineNum < 0 OrElse iLineNum > mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.TotalNumberOfLines - 1) Then
                            Return
                        End If

                        mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.Caret.Line = iLineNum
                        mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.Caret.Column = 0
                        mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.SelectionManager.ClearSelection()

                        Dim iLineLen As Integer = mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.GetLineSegment(iLineNum).Length

                        Dim iStart As New TextLocation(0, iLineNum)
                        Dim iEnd As New TextLocation(iLineLen, iLineNum)

                        mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(iStart, iEnd)

                        If (mFormMain.g_ClassTabControl.m_ActiveTabIndex <> i) Then
                            mFormMain.g_ClassTabControl.SelectTab(i)
                        End If
                        Return
                    End If
                Next

                If (bForceEnd) Then
                    Exit While
                End If

                For Each mInclude As DictionaryEntry In mFormMain.g_ClassTabControl.m_ActiveTab.m_IncludeFilesFull.ToArray
                    If (String.IsNullOrEmpty(CStr(mInclude.Value)) OrElse Not IO.File.Exists(CStr(mInclude.Value))) Then
                        Continue For
                    End If

                    Dim sIncludePath As String = CStr(mInclude.Value).Replace("/"c, "\"c)
                    If (sIncludePath.ToLower = sFile.ToLower OrElse (bGuessTab AndAlso sIncludePath.ToLower.EndsWith(sFile.ToLower))) Then
                        Dim mTab = mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(CStr(mInclude.Value))
                        mTab.SelectTab()

                        bForceEnd = True
                        Continue While
                    End If
                Next

                Exit While
            End While

            MessageBox.Show("Could not find file. Please open the file manualy and try again.", "Unable to find file", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class