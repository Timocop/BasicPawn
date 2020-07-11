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


Imports ICSharpCode.TextEditor

Public Class FormReportDetails
    Public g_mFormReportManager As FormReportManager
    Public g_mException As ClassDebuggerTools.STRUC_SM_EXCEPTION

    Public Sub New(mFormReportManager As FormReportManager, mException As ClassDebuggerTools.STRUC_SM_EXCEPTION)
        g_mFormReportManager = mFormReportManager
        g_mException = mException

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        ClassControlStyle.SetNameFlag(Panel2, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)
        ClassControlStyle.SetNameFlag(Panel4, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)

        m_WarningText = ""

        Me.Text = mException.sExceptionInfo

        Label_ExceptionName.Text = mException.sExceptionInfo
        Label_FileName.Text = mException.sBlamingFile
        Label_Date.Text = mException.dLogDate.ToLongDateString & " - " & mException.dLogDate.ToShortTimeString

        Try
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
        Finally
            ListView_StackTrace.EndUpdate()
        End Try

        Const FLAG_NONE = (0 << 1)
        Const FLAG_FILEDATE = (1 << 1)
        Const FLAG_FILEMISSING = (2 << 1)
        Dim iWarningFlag = FLAG_NONE

        For i = 0 To mException.mStackTraces.Length - 1
            If (String.IsNullOrEmpty(mException.mStackTraces(i).sFileName)) Then
                Continue For
            End If

            If (IO.File.Exists(mException.mStackTraces(i).sFileName)) Then
                Dim dFileDate As Date = New IO.FileInfo(mException.mStackTraces(i).sFileName).LastWriteTime

                If (dFileDate > mException.dLogDate) Then
                    If ((iWarningFlag And FLAG_FILEDATE) = 0) Then
                        iWarningFlag = iWarningFlag Or FLAG_FILEDATE
                        m_WarningText &= If(m_WarningText.Length > 0, Environment.NewLine, "") & "Some source files are newer. Stack traces might be inaccurate."
                    End If
                End If
            Else
                If ((iWarningFlag And FLAG_FILEMISSING) = 0) Then
                    iWarningFlag = iWarningFlag Or FLAG_FILEMISSING
                    m_WarningText &= If(m_WarningText.Length > 0, Environment.NewLine, "") & "Unable to find some source files."
                End If
                Exit For
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

    Private Sub ListView_StackTrace_Click(sender As Object, e As EventArgs) Handles ListView_StackTrace.Click
        If (ListView_StackTrace.SelectedItems.Count < 1) Then
            Return
        End If

        Try
            Dim mListViewItemData = TryCast(ListView_StackTrace.SelectedItems(0), ClassListViewItemData)
            If (mListViewItemData Is Nothing) Then
                Return
            End If

            Dim sLine As String = CStr(mListViewItemData.g_mData("Line"))
            Dim sFile As String = CStr(mListViewItemData.g_mData("File")).Replace("/"c, "\"c)

            If (String.IsNullOrEmpty(sFile)) Then
                Return
            End If

            Dim bGuessTab As Boolean = False
            If (Not IO.File.Exists(sFile)) Then
                bGuessTab = True
                sFile = IO.Path.GetFileName(sFile)
            End If

            Dim mFormMain = g_mFormReportManager.g_mPluginAutoErrorReport.g_mFormMain

            Dim bForceEnd As Boolean = False
            While True
                For i = 0 To mFormMain.g_ClassTabControl.m_TabsCount - 1
                    If (mFormMain.g_ClassTabControl.m_Tab(i).m_IsUnsaved OrElse mFormMain.g_ClassTabControl.m_Tab(i).m_InvalidFile) Then
                        Continue For
                    End If

                    Dim sTabPath As String = mFormMain.g_ClassTabControl.m_Tab(i).m_File.Replace("/"c, "\"c)

                    If (sTabPath.ToLower = sFile.ToLower OrElse (bGuessTab AndAlso sTabPath.ToLower.EndsWith(sFile.ToLower))) Then
                        Dim iLineNum As Integer = ClassTools.ClassMath.ClampInt(CInt(sLine) - 1, 0, mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.TotalNumberOfLines - 1)
                        Dim iLineLen As Integer = mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.Document.GetLineSegment(iLineNum).Length

                        Dim mStartLoc As New TextLocation(0, iLineNum)
                        Dim mEndLoc As New TextLocation(iLineLen, iLineNum)

                        mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.Caret.Position = mStartLoc
                        mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.SelectionManager.ClearSelection()
                        mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.SelectionManager.SetSelection(mStartLoc, mEndLoc)
                        mFormMain.g_ClassTabControl.m_Tab(i).m_TextEditor.ActiveTextAreaControl.CenterViewOn(iLineNum, 10)

                        If (Not mFormMain.g_ClassTabControl.m_Tab(i).m_IsActive) Then
                            mFormMain.g_ClassTabControl.m_Tab(i).SelectTab()
                        End If

                        Me.TopMost = True
                        mFormMain.Activate()
                        Me.TopMost = False
                        Exit While
                    End If
                Next

                If (bForceEnd) Then
                    Exit While
                End If

                If (IO.File.Exists(sFile)) Then
                    Dim mTab = mFormMain.g_ClassTabControl.AddTab()
                    mTab.OpenFileTab(sFile)
                    mTab.SelectTab()

                    bForceEnd = True
                End If

                If (bForceEnd) Then
                    Continue While
                End If

                For Each mInclude In mFormMain.g_ClassTabControl.m_ActiveTab.m_IncludesGroup.m_IncludeFilesFull.ToArray
                    If (String.IsNullOrEmpty(mInclude.Value) OrElse Not IO.File.Exists(mInclude.Value)) Then
                        Continue For
                    End If

                    Dim sIncludePath As String = mInclude.Value.Replace("/"c, "\"c)
                    If (sIncludePath.ToLower = sFile.ToLower OrElse (bGuessTab AndAlso sIncludePath.ToLower.EndsWith(sFile.ToLower))) Then
                        Dim mTab = mFormMain.g_ClassTabControl.AddTab()
                        mTab.OpenFileTab(mInclude.Value)
                        mTab.SelectTab()

                        bForceEnd = True
                    End If
                Next

                If (bForceEnd) Then
                    Continue While
                End If

                Select Case (MessageBox.Show(String.Format("Could not find file. Do you want to use known files from the configs to find the file?{0}{0}Otherwise open the file manualy and try again.", Environment.NewLine), "Unable to find file", MessageBoxButtons.YesNo, MessageBoxIcon.Error))
                    Case DialogResult.Yes
                        Dim bFoundFile As Boolean = False

                        For Each mItem In ClassConfigs.ClassKnownConfigs.GetKnownConfigs
                            Dim sKnownFile As String = mItem.sFile.Replace("/"c, "\"c)

                            If (Not IO.File.Exists(sKnownFile)) Then
                                Continue For
                            End If

                            If (sKnownFile.ToLower = sFile.ToLower OrElse (bGuessTab AndAlso sKnownFile.ToLower.EndsWith(sFile.ToLower))) Then
                                Dim mTab = mFormMain.g_ClassTabControl.AddTab()
                                mTab.OpenFileTab(sKnownFile)
                                mTab.SelectTab()

                                bForceEnd = True
                                bFoundFile = True
                            End If
                        Next

                        If (Not bFoundFile) Then
                            MessageBox.Show("Could not find file. Please open the file manualy and try again.", "Unable to find file", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                End Select

                If (bForceEnd) Then
                    Continue While
                End If

                Exit While
            End While

        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
End Class