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


Public Class FormOpenTabFromInstances
    Private g_mFormMain As FormMain

    Private g_sItemsIdentifier As String = Guid.NewGuid.ToString

    Structure STRUC_TABINFO_ITEM
        Dim iIndex As Integer
        Dim sFile As String
        Dim iProcessID As Integer
    End Structure

    Public Sub New(f As FormMain)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        g_mFormMain = f

        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)
    End Sub

    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Public Sub RefreshList()
        g_sItemsIdentifier = Guid.NewGuid.ToString

        ListView_Instances.Items.Clear()
        ListView_Instances.Groups.Clear()

        Dim iPID As Integer = Process.GetCurrentProcess.Id
        Dim sProcessName As String = Process.GetCurrentProcess.ProcessName

        ''List current items
        'For i = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
        '    Dim j = g_mFormMain.g_ClassTabControl.m_Tab(i)

        '    AddListViewItem(j.TabIndex, sProcessName, iPID, j.m_File)
        'Next

        'Send request to other BasicPawn instances
        g_mFormMain.g_ClassCrossAppComunication.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_REQUEST_TABS, g_sItemsIdentifier), False)
    End Sub

    Public Sub AddListViewItem(iTabIndex As Integer, sProcessName As String, iProcessID As Integer, sFile As String, Optional sIdentifier As String = Nothing)
        If (sIdentifier IsNot Nothing AndAlso sIdentifier <> g_sItemsIdentifier) Then
            Return
        End If

        If (String.IsNullOrEmpty(sFile) OrElse Not IO.File.Exists(sFile)) Then
            Return
        End If

        Dim sHeaderName As String = String.Format("{0} ({1})", sProcessName, iProcessID)

        ListView_Instances.Items.Add(New ClassListViewItem(New String() {CStr(iTabIndex), sFile}, FindOrCreateGroup(sHeaderName)) With {
            .mTabInfo = New STRUC_TABINFO_ITEM With {
                .iIndex = iTabIndex,
                .iProcessID = iProcessID,
                .sFile = sFile
            }
        })
        ListView_Instances.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
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

    Class ClassListViewItem
        Inherits ListViewItem

        Public mTabInfo As STRUC_TABINFO_ITEM?

        Public Sub New(items() As String, group As ListViewGroup)
            MyBase.New(items, group)
        End Sub
    End Class

    Private Sub FormOpenTabFromInstances_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RefreshList()

        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub LinkLabel_Refresh_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_Refresh.LinkClicked
        RefreshList()
    End Sub

    Private Sub Button_Open_Click(sender As Object, e As EventArgs) Handles Button_Open.Click
        Dim bOpenNew As Boolean = CheckBox_NewInstance.Checked
        Dim bCloseTabs As Boolean = CheckBox_CloseTabs.Checked

        Dim lFiles As New List(Of String)

        For i = ListView_Instances.CheckedItems.Count - 1 To 0 Step -1
            If (TypeOf ListView_Instances.CheckedItems(i) IsNot ClassListViewItem) Then
                Continue For
            End If

            Dim iItem = DirectCast(ListView_Instances.CheckedItems(i), ClassListViewItem)

            If (String.IsNullOrEmpty(iItem.mTabInfo.Value.sFile)) Then
                MessageBox.Show(String.Format("Invalid file from process id {0} tab {1}", iItem.mTabInfo.Value.iProcessID, iItem.mTabInfo.Value.iIndex), "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Continue For
            End If

            If (Not IO.File.Exists(iItem.mTabInfo.Value.sFile)) Then
                MessageBox.Show(String.Format("'{0}' does not exist!", iItem.mTabInfo.Value.sFile), "File does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Continue For
            End If

            If (bOpenNew) Then
                lFiles.Add("""" & iItem.mTabInfo.Value.sFile & """")
            Else
                g_mFormMain.g_ClassTabControl.AddTab(True)
                g_mFormMain.g_ClassTabControl.OpenFileTab(g_mFormMain.g_ClassTabControl.m_TabsCount - 1, iItem.mTabInfo.Value.sFile)
            End If

            If (bCloseTabs) Then
                Dim iTabIndex As Integer = iItem.mTabInfo.Value.iIndex
                Dim iPID As Integer = iItem.mTabInfo.Value.iProcessID
                Dim sFile As String = iItem.mTabInfo.Value.sFile

                g_mFormMain.g_ClassCrossAppComunication.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_CLOSE_TAB, CStr(iTabIndex), CStr(iPID), sFile), False)
            End If
        Next

        If (bOpenNew) Then
            Process.Start(Application.ExecutablePath, String.Join(" ", {"-newinstance", String.Join(" ", lFiles.ToArray)}))
        End If

        Me.Close()
    End Sub

    Private Sub ListView_Instances_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView_Instances.SelectedIndexChanged
        If (ListView_Instances.SelectedItems.Count < 1) Then
            Return
        End If

        Dim iItem = DirectCast(ListView_Instances.SelectedItems(0), ClassListViewItem)

        Dim iPID As Integer = iItem.mTabInfo.Value.iProcessID

        g_mFormMain.g_ClassCrossAppComunication.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_SHOW_PING_FLASH, CStr(iPID)), False)
    End Sub

    Private Sub ListView_Instances_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListView_Instances.ItemChecked
        If (Not e.Item.Checked) Then
            Return
        End If

        Dim iItem = DirectCast(e.Item, ClassListViewItem)

        Dim iPID As Integer = iItem.mTabInfo.Value.iProcessID

        g_mFormMain.g_ClassCrossAppComunication.SendMessage(New ClassCrossAppComunication.ClassMessage(FormMain.COMARG_SHOW_PING_FLASH, CStr(iPID)), False)
    End Sub
End Class