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


Public Class FormOpenTabFromInstances
    Private g_mFormMain As FormMain

    Private g_sCallerIdentifier As String = Guid.NewGuid.ToString
    Private g_mShowDelayThread As Threading.Thread

    Structure STRUC_TABINFO_ITEM
        Dim sTabIndentifier As String
        Dim sTabIndex As Integer
        Dim sTabFile As String
        Dim iProcessID As Integer
    End Structure

    Public Sub New(f As FormMain)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Panel_FooterControl.Name &= "@FooterControl"
        Panel_FooterDarkControl.Name &= "@FooterDarkControl"

        g_mFormMain = f

        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)
    End Sub

    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Public Sub RefreshList()
        g_sCallerIdentifier = Guid.NewGuid.ToString

        ListView_Instances.Items.Clear()
        ListView_Instances.Groups.Clear()

        Dim iPID As Integer = Process.GetCurrentProcess.Id
        Dim sProcessName As String = Process.GetCurrentProcess.ProcessName

        'Send request to other BasicPawn instances
        g_mFormMain.g_ClassCrossAppCom.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_REQUEST_TABS, g_sCallerIdentifier), False)
    End Sub

    Public Sub AddListViewItem(sTabIdentifier As String, iTabIndex As Integer, sTabFile As String, sProcessName As String, iProcessID As Integer, Optional sCallerIdentifier As String = Nothing)
        If (sCallerIdentifier IsNot Nothing AndAlso sCallerIdentifier <> g_sCallerIdentifier) Then
            Return
        End If

        If (String.IsNullOrEmpty(sTabFile) OrElse Not IO.File.Exists(sTabFile)) Then
            Return
        End If

        Dim sHeaderName As String = String.Format("{0} ({1}){2}", sProcessName, iProcessID, If(iProcessID = Process.GetCurrentProcess.Id, " (Current)", ""))

        Dim mListViewItemData As New ClassListViewItemData(New String() {CStr(iTabIndex), sTabFile}, FindOrCreateGroup(sHeaderName))
        mListViewItemData.g_mData("TabInfo") = New STRUC_TABINFO_ITEM With {
            .sTabIndentifier = sTabIdentifier,
            .sTabIndex = iTabIndex,
            .sTabFile = sTabFile,
            .iProcessID = iProcessID
        }

        ListView_Instances.Items.Add(mListViewItemData)
        ClassTools.ClassControls.ClassListView.AutoResizeColumns(ListView_Instances)
    End Sub

    Public Function FindOrCreateGroup(sHeader As String) As ListViewGroup
        For Each i As ListViewGroup In ListView_Instances.Groups
            If (i.Header = sHeader) Then
                Return i
            End If
        Next

        Dim j = New ListViewGroup(sHeader)
        ListView_Instances.Groups.Add(j)
        Return j
    End Function

    Private Sub FormOpenTabFromInstances_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RefreshList()

        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub LinkLabel_Refresh_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_Refresh.LinkClicked
        RefreshList()
    End Sub

    Private Sub Button_Open_Click(sender As Object, e As EventArgs) Handles Button_Open.Click
        Try
            Dim bOpenNew As Boolean = CheckBox_NewInstance.Checked
            Dim bCloseTabs As Boolean = CheckBox_CloseTabs.Checked

            Dim lFiles As New List(Of String)

            For i = ListView_Instances.CheckedItems.Count - 1 To 0 Step -1
                Dim mListViewItemData = TryCast(ListView_Instances.CheckedItems(i), ClassListViewItemData)
                If (mListViewItemData Is Nothing) Then
                    Continue For
                End If

                Dim mTabInfo As STRUC_TABINFO_ITEM? = DirectCast(mListViewItemData.g_mData("TabInfo"), STRUC_TABINFO_ITEM?)

                If (String.IsNullOrEmpty(mTabInfo.Value.sTabFile)) Then
                    MessageBox.Show(String.Format("Invalid file from process id {0} tab index {1}", mTabInfo.Value.iProcessID, mTabInfo.Value.sTabIndex - 1), "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                If (Not IO.File.Exists(mTabInfo.Value.sTabFile)) Then
                    MessageBox.Show(String.Format("'{0}' does not exist!", mTabInfo.Value.sTabFile), "File does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Continue For
                End If

                If (bOpenNew) Then
                    lFiles.Add("""" & mTabInfo.Value.sTabFile & """")
                Else
                    Dim mTab = g_mFormMain.g_ClassTabControl.AddTab()
                    mTab.OpenFileTab(mTabInfo.Value.sTabFile)
                    mTab.SelectTab(500)
                End If

                If (bCloseTabs) Then
                    Dim iPID As Integer = mTabInfo.Value.iProcessID
                    Dim sTabIdentifier As String = mTabInfo.Value.sTabIndentifier
                    Dim sFile As String = mTabInfo.Value.sTabFile

                    g_mFormMain.g_ClassCrossAppCom.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_CLOSE_TAB, CStr(iPID), sTabIdentifier, sFile, CStr(True)), False)
                End If
            Next

            If (bOpenNew) Then
                Process.Start(Application.ExecutablePath, String.Join(" ", {"-newinstance", String.Join(" ", lFiles.ToArray)}))
            End If

            Me.Close()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ListView_Instances_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_Instances.SelectedIndexChanged
        If (ListView_Instances.SelectedItems.Count < 1) Then
            Return
        End If

        Dim mListViewItemData = TryCast(ListView_Instances.SelectedItems(0), ClassListViewItemData)
        If (mListViewItemData Is Nothing) Then
            Return
        End If

        Dim mTabInfo As STRUC_TABINFO_ITEM? = DirectCast(mListViewItemData.g_mData("TabInfo"), STRUC_TABINFO_ITEM?)

        Dim iPID As Integer = mTabInfo.Value.iProcessID
        Dim sTabIndetifier As String = mTabInfo.Value.sTabIndentifier
         
        ClassThread.Abort(g_mShowDelayThread)

        'Ping with a small delay, so Me.Activate works correctly
        g_mShowDelayThread = New Threading.Thread(Sub()
                                                      Try
                                                          Threading.Thread.Sleep(500)
                                                          g_mFormMain.g_ClassCrossAppCom.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_SHOW_PING_FLASH, CStr(iPID), sTabIndetifier), False)
                                                      Catch ex As Threading.ThreadAbortException
                                                          Throw
                                                      Catch ex As Exception
                                                      End Try
                                                  End Sub) With {
            .IsBackground = True
        }
        g_mShowDelayThread.Start()
    End Sub

    Private Sub FormOpenTabFromInstances_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub CleanUp()
        ClassThread.Abort(g_mShowDelayThread)
    End Sub

End Class