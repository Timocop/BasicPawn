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


Public Class FormNewWizard
#Region "Form"
    Private g_sTemplateFolder As String = IO.Path.Combine(Application.StartupPath, "templates")

    Private g_sOrginalTemplate As String = ""
    Private g_sPreviewTemplate As String = ""

    Private g_ClassProperties As New ClassProperties(Me)
    Private g_bIgnoreComboBoxSetPropertyEvents As Boolean = False

    Private g_mSelectedPropertyItem As ClassListViewItemData

    Enum ENUM_TREEVIEW_ICONS
        FILE
        FOLDER
    End Enum

    Enum ENUM_TEMPLATE_TYPES
        ROOT
        SOURCEMOD_OLD
        SOURCEMOD_NEW
        AMXMODX
    End Enum

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ImageList_TreeView.Images.Clear()
        ImageList_TreeView.Images.Add(CStr(ENUM_TREEVIEW_ICONS.FILE), My.Resources.Ico_Rtf)
        ImageList_TreeView.Images.Add(CStr(ENUM_TREEVIEW_ICONS.FOLDER), My.Resources.Ico_Folder)
    End Sub

    ReadOnly Property m_OrginalTemplateSource As String
        Get
            Return g_sOrginalTemplate
        End Get
    End Property

    ReadOnly Property m_PreviewTemplateSource As String
        Get
            Return g_sPreviewTemplate
        End Get
    End Property

    Private Sub FormNewWizard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Write all default templates to disk
        If (Not IO.Directory.Exists(g_sTemplateFolder)) Then
            CreateTemplates()
        End If

        g_ClassProperties.Clear()
        FillTemplateExplorer()
        FillListViewPropeties()
        UpdatePreview()

        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub CreateTemplates()
        Dim mPathDic As New Dictionary(Of ENUM_TEMPLATE_TYPES, String)
        mPathDic(ENUM_TEMPLATE_TYPES.ROOT) = g_sTemplateFolder
        mPathDic(ENUM_TEMPLATE_TYPES.SOURCEMOD_OLD) = IO.Path.Combine(g_sTemplateFolder, "SourceMod")
        mPathDic(ENUM_TEMPLATE_TYPES.SOURCEMOD_NEW) = IO.Path.Combine(g_sTemplateFolder, "SourceMod (Transitional)")
        mPathDic(ENUM_TEMPLATE_TYPES.AMXMODX) = IO.Path.Combine(g_sTemplateFolder, "AMX Mod X")

        For Each mItem In mPathDic
            If (Not IO.Directory.Exists(mItem.Value)) Then
                IO.Directory.CreateDirectory(mItem.Value)
            End If

            Select Case (mItem.Key)
                Case (ENUM_TEMPLATE_TYPES.ROOT)
                    IO.File.WriteAllText(IO.Path.Combine(mItem.Value, "Include.txt"), My.Resources.Template_Include)

                Case (ENUM_TEMPLATE_TYPES.SOURCEMOD_OLD)
                    IO.File.WriteAllText(IO.Path.Combine(mItem.Value, "Plugin.txt"), My.Resources.Template_SourcePawnOldPlugin)
                    IO.File.WriteAllText(IO.Path.Combine(mItem.Value, "Shared Plugin Library.txt"), My.Resources.Template_SourcePawnOldSharedPluginInclude)
                    IO.File.WriteAllText(IO.Path.Combine(mItem.Value, "Extension Library.txt"), My.Resources.Template_SourcePawnOldExtensionInclude)

                Case (ENUM_TEMPLATE_TYPES.SOURCEMOD_NEW)
                    IO.File.WriteAllText(IO.Path.Combine(mItem.Value, "Plugin.txt"), My.Resources.Template_SourcePawnNewPlugin)
                    IO.File.WriteAllText(IO.Path.Combine(mItem.Value, "Shared Plugin Library.txt"), My.Resources.Template_SourcePawnNewSharedPluginInclude)
                    IO.File.WriteAllText(IO.Path.Combine(mItem.Value, "Extension Library.txt"), My.Resources.Template_SourcePawnNewExtensionInclude)

                Case (ENUM_TEMPLATE_TYPES.AMXMODX)
                    IO.File.WriteAllText(IO.Path.Combine(mItem.Value, "Plugin.txt"), My.Resources.Template_AMXModXPlugin)
                    IO.File.WriteAllText(IO.Path.Combine(mItem.Value, "Include.txt"), My.Resources.Template_AMXModXInclude)
                    IO.File.WriteAllText(IO.Path.Combine(mItem.Value, "Library.txt"), My.Resources.Template_AMXModXLibraryInclude)
                    IO.File.WriteAllText(IO.Path.Combine(mItem.Value, "Module Library.txt"), My.Resources.Template_AMXModXModuleInclude)
            End Select
        Next
    End Sub

    Private Sub LinkLabel_CreateDefault_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_CreateDefault.LinkClicked
        Try
            CreateTemplates()

            g_ClassProperties.Clear()
            FillTemplateExplorer()
            FillListViewPropeties()
            UpdatePreview()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Class ClassProperties
        Private g_mFormNewWizard As FormNewWizard

        Private g_lPropertiesTypeDefault As New Dictionary(Of String, STRUC_DEFAULT_REPLACE)
        Private g_lPropertiesTypeBoolean As New Dictionary(Of String, STRUC_BOOLEAN_REPLACE)
        Private g_lPropertiesTypeList As New Dictionary(Of String, STRUC_LIST_REPLACE)
        Private g_lSource As New List(Of String)

        Structure STRUC_DEFAULT_REPLACE
            Dim sDescription As String
            Dim sReplace As String
        End Structure

        Structure STRUC_BOOLEAN_REPLACE
            Dim sDescription As String
            Dim sReplaceTrue As String
            Dim sReplaceFalse As String

            Dim bValue As Boolean
        End Structure

        Structure STRUC_LIST_REPLACE
            Dim sDescription As String
            Dim mReplace As STRUC_LIST_REPLACE_ITEM()

            Dim iIndex As Integer

            Structure STRUC_LIST_REPLACE_ITEM
                Dim sItemDescription As String
                Dim sReplace As String
            End Structure
        End Structure

        Public Sub New(f As FormNewWizard)
            g_mFormNewWizard = f
        End Sub

        ReadOnly Property m_Source As String
            Get
                Dim mSourceBuilder As New Text.StringBuilder()

                For Each sLine As String In g_lSource.ToArray
                    ResolveNewlines(sLine)
                    ResolveTabs(sLine)
                    ResolveTerminator(sLine)

                    mSourceBuilder.AppendLine(sLine)
                Next

                Return mSourceBuilder.ToString
            End Get
        End Property

        ReadOnly Property m_PropertiesTypeDefault As Dictionary(Of String, STRUC_DEFAULT_REPLACE)
            Get
                Return g_lPropertiesTypeDefault
            End Get
        End Property

        ReadOnly Property m_PropertiesTypeBoolean As Dictionary(Of String, STRUC_BOOLEAN_REPLACE)
            Get
                Return g_lPropertiesTypeBoolean
            End Get
        End Property

        ReadOnly Property m_PropertiesTypeList As Dictionary(Of String, STRUC_LIST_REPLACE)
            Get
                Return g_lPropertiesTypeList
            End Get
        End Property

        Public Sub ParseFromFile(sFile As String)
            Parse(IO.File.ReadAllText(sFile))
        End Sub

        Public Sub Parse(sSource As String)
            Clear()

            Using mIniMem As New ClassIni(sSource)
                For Each mItem In mIniMem.ReadEverything
                    Select Case (mItem.sSection)
                        Case "Source"
                            g_lSource.Add(mItem.sValue)

                        Case "Properties"
                            Select Case (True)
                                Case mItem.sKey.StartsWith("$")
                                    Dim sProperty As String = mItem.sKey.TrimStart("$"c)
                                    Dim sSplit As String() = sProperty.Split(","c)
                                    If (sSplit.Length <> 2) Then
                                        Continue For
                                    End If

                                    Dim sName As String = sSplit(0)
                                    Dim sDescription As String = sSplit(1)

                                    If (String.IsNullOrEmpty(sName) OrElse sName.Trim.Length < 1) Then
                                        Continue For
                                    End If

                                    If (g_lPropertiesTypeDefault.ContainsKey(sName)) Then
                                        Continue For
                                    End If

                                    Dim mItemDefault As New STRUC_DEFAULT_REPLACE With {
                                        .sDescription = sDescription.Trim,
                                        .sReplace = mItem.sValue.Trim
                                    }

                                    g_lPropertiesTypeDefault(sName) = mItemDefault

                                Case mItem.sKey.StartsWith("?")
                                    Dim sProperty As String = mItem.sKey.TrimStart("?"c)
                                    Dim sSplit As String() = sProperty.Split(","c)
                                    If (sSplit.Length <> 2) Then
                                        Continue For
                                    End If

                                    Dim sName As String = sSplit(0)
                                    Dim sDescription As String = sSplit(1)

                                    If (String.IsNullOrEmpty(sName) OrElse sName.Trim.Length < 1) Then
                                        Continue For
                                    End If

                                    If (g_lPropertiesTypeBoolean.ContainsKey(sName)) Then
                                        'If we hit the second key, its probably FALSE, we already hit TRUE.
                                        If (g_lPropertiesTypeBoolean(sName).sReplaceTrue Is Nothing) Then
                                            Throw New ArgumentException(String.Format("Boolean property '{0}' has no TRUE replacement", sName))

                                        ElseIf (g_lPropertiesTypeBoolean(sName).sReplaceFalse Is Nothing) Then
                                            Dim mItemBoolean = g_lPropertiesTypeBoolean(sName)

                                            mItemBoolean.sReplaceFalse = mItem.sValue.Trim

                                            g_lPropertiesTypeBoolean(sName) = mItemBoolean
                                        Else
                                            Dim mItemBoolean = g_lPropertiesTypeBoolean(sName)

                                            mItemBoolean.bValue = (mItem.sValue = "1")

                                            g_lPropertiesTypeBoolean(sName) = mItemBoolean
                                        End If
                                    Else
                                        Dim mItemBoolean As New STRUC_BOOLEAN_REPLACE With {
                                            .sDescription = sDescription.Trim,
                                            .sReplaceTrue = mItem.sValue.Trim,
                                            .sReplaceFalse = Nothing,
                                            .bValue = True
                                        }

                                        g_lPropertiesTypeBoolean(sName) = mItemBoolean
                                    End If

                                Case mItem.sKey.StartsWith("#")
                                    Dim sProperty As String = mItem.sKey.TrimStart("#"c)
                                    Dim sSplit As String() = sProperty.Split(","c)
                                    If (sSplit.Length <> 3) Then
                                        Continue For
                                    End If

                                    Dim sName As String = sSplit(0)
                                    Dim sDescription As String = sSplit(1)
                                    Dim sItemDescription As String = sSplit(2)

                                    If (String.IsNullOrEmpty(sName) OrElse sName.Trim.Length < 1) Then
                                        Continue For
                                    End If

                                    If (String.IsNullOrEmpty(sItemDescription) OrElse sItemDescription.Trim.Length < 1) Then
                                        Continue For
                                    End If

                                    If (g_lPropertiesTypeList.ContainsKey(sName)) Then
                                        Dim mItemList = g_lPropertiesTypeList(sName)

                                        With New List(Of STRUC_LIST_REPLACE.STRUC_LIST_REPLACE_ITEM)
                                            .AddRange(mItemList.mReplace)

                                            Dim mItemItemList As New STRUC_LIST_REPLACE.STRUC_LIST_REPLACE_ITEM With {
                                                .sItemDescription = sItemDescription,
                                                .sReplace = mItem.sValue.Trim
                                            }
                                            .Add(mItemItemList)

                                            mItemList.mReplace = .ToArray
                                        End With

                                        g_lPropertiesTypeList(sName) = mItemList
                                    Else
                                        Dim mItemItemList As New STRUC_LIST_REPLACE.STRUC_LIST_REPLACE_ITEM With {
                                            .sItemDescription = sItemDescription,
                                            .sReplace = mItem.sValue.Trim
                                        }

                                        Dim mItemList As New STRUC_LIST_REPLACE With {
                                            .sDescription = sDescription.Trim,
                                            .mReplace = {mItemItemList},
                                            .iIndex = 0
                                        }

                                        g_lPropertiesTypeList(sName) = mItemList
                                    End If

                            End Select
                    End Select
                Next
            End Using

            'Check for unfinished properties 
            For Each mItem In g_lPropertiesTypeBoolean
                If (mItem.Value.sReplaceTrue Is Nothing) Then
                    Throw New ArgumentException(String.Format("Boolean property '{0}' has no TRUE replacement", mItem.Key))
                End If

                If (mItem.Value.sReplaceFalse Is Nothing) Then
                    Throw New ArgumentException(String.Format("Boolean property '{0}' has no FALSE replacement", mItem.Key))
                End If
            Next

            For Each mItem In g_lPropertiesTypeList
                If (mItem.Value.mReplace Is Nothing OrElse mItem.Value.mReplace.Length < 1) Then
                    Throw New ArgumentException(String.Format("List property '{0}' has no items", mItem.Key))
                End If
            Next
        End Sub

        Public Sub ResolveNewlines(ByRef sSource As String)
            sSource = sSource.Replace("%n%", Environment.NewLine)
        End Sub

        Public Sub ResolveTabs(ByRef sSource As String)
            sSource = sSource.Replace("%t%", ClassSettings.BuildIndentation(1, ClassSettings.ENUM_INDENTATION_TYPES.USE_SETTINGS))
        End Sub

        Public Sub ResolveTerminator(ByRef sSource As String)
            Dim iIndex As Integer = sSource.IndexOf("%0%")
            If (iIndex < 0) Then
                Return
            End If

            sSource = sSource.Substring(0, iIndex)
        End Sub

        Public Sub Clear()
            g_lPropertiesTypeDefault.Clear()
            g_lPropertiesTypeBoolean.Clear()
            g_lPropertiesTypeList.Clear()
            g_lSource.Clear()
        End Sub

        Public Function GenerateSource() As String
            Dim mSourceBuilder As New Text.StringBuilder

            For Each sLine As String In g_lSource
                For Each mItem In g_lPropertiesTypeDefault
                    sLine = sLine.Replace("{" & String.Format("${0}", mItem.Key) & "}", mItem.Value.sReplace)
                Next

                For Each mItem In g_lPropertiesTypeBoolean
                    If (mItem.Value.bValue) Then
                        sLine = sLine.Replace("{" & String.Format("?{0}", mItem.Key) & "}", mItem.Value.sReplaceTrue)
                    Else
                        sLine = sLine.Replace("{" & String.Format("?{0}", mItem.Key) & "}", mItem.Value.sReplaceFalse)
                    End If
                Next

                For Each mItem In g_lPropertiesTypeList
                    If (mItem.Value.iIndex < 0 OrElse mItem.Value.iIndex > mItem.Value.mReplace.Length) Then
                        Continue For
                    End If

                    sLine = sLine.Replace("{" & String.Format("#{0}", mItem.Key) & "}", mItem.Value.mReplace(mItem.Value.iIndex).sReplace)
                Next

                ResolveNewlines(sLine)
                ResolveTabs(sLine)
                ResolveTerminator(sLine)

                mSourceBuilder.AppendLine(sLine)
            Next

            Return mSourceBuilder.ToString
        End Function
    End Class
#End Region

#Region "Template Explorer"
    Private Sub FillTemplateExplorer()
        TreeView_Explorer.Nodes.Clear()

        If (Not IO.Directory.Exists(g_sTemplateFolder)) Then
            Return
        End If

        TreeView_Explorer.Nodes.Add(CreateFileNodes(g_sTemplateFolder))
        TreeView_Explorer.ExpandAll()
    End Sub

    Private Function CreateFileNodes(sDirectory As String) As TreeNode
        Dim mDirInfo As New IO.DirectoryInfo(sDirectory)

        Dim mDirectoyNode As New TreeNode(mDirInfo.Name, ENUM_TREEVIEW_ICONS.FOLDER, ENUM_TREEVIEW_ICONS.FOLDER)

        For Each mNextDirInfo In mDirInfo.GetDirectories()
            mDirectoyNode.Nodes.Add(CreateFileNodes(mNextDirInfo.FullName))
        Next

        For Each mFileInfo In mDirInfo.GetFiles("*.txt", IO.SearchOption.TopDirectoryOnly)
            mDirectoyNode.Nodes.Add(New TreeNode(mFileInfo.Name, ENUM_TREEVIEW_ICONS.FILE, ENUM_TREEVIEW_ICONS.FILE))
        Next

        Return mDirectoyNode
    End Function

    Private Sub TreeView_Explorer_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeView_Explorer.NodeMouseClick
        Try
            'Fixes TreeView glichty focus with ContextMenuStrips
            Select Case (e.Button)
                Case MouseButtons.Right
                    TreeView_Explorer.SelectedNode = e.Node
                    ContextMenuStrip_TreeView.Show(TreeView_Explorer, e.Location)

            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub TreeView_Explorer_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView_Explorer.AfterSelect
        Try
            Dim sTemplatePath As String = IO.Path.Combine(Application.StartupPath, e.Node.FullPath)
            If (IO.Directory.Exists(sTemplatePath)) Then
                g_ClassProperties.Clear()
                FillListViewPropeties()
                UpdatePreview()
                Return
            End If

            If (Not IO.File.Exists(sTemplatePath)) Then
                g_ClassProperties.Clear()
                FillListViewPropeties()
                UpdatePreview()
                Throw New ArgumentException("File does not exist")
            End If

            g_ClassProperties.ParseFromFile(sTemplatePath)

            If (String.IsNullOrEmpty(g_ClassProperties.m_Source)) Then
                Throw New ArgumentException("Invalid template")
            End If

            g_sOrginalTemplate = g_ClassProperties.m_Source
            g_sPreviewTemplate = g_ClassProperties.GenerateSource()

            FillListViewPropeties()
            UpdatePreview()
        Catch ex As Exception
            g_ClassProperties.Clear()
            g_sOrginalTemplate = ""
            g_sPreviewTemplate = ""

            FillListViewPropeties()
            UpdatePreview()

            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ContextMenuStrip_TreeView_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_TreeView.Opening
        ClassControlStyle.UpdateControls(ContextMenuStrip_TreeView)

        ToolStripMenuItem_OpenDir.Enabled = (TreeView_Explorer.SelectedNode IsNot Nothing AndAlso
                                                        (IO.Directory.Exists(TreeView_Explorer.SelectedNode.FullPath) OrElse
                                                                IO.File.Exists(TreeView_Explorer.SelectedNode.FullPath)))
        ToolStripMenuItem_DelTemplate.Enabled = (TreeView_Explorer.SelectedNode IsNot Nothing AndAlso
                                                        (IO.Directory.Exists(TreeView_Explorer.SelectedNode.FullPath) OrElse
                                                                IO.File.Exists(TreeView_Explorer.SelectedNode.FullPath)))
    End Sub
#End Region

#Region "Properties"
    Private Sub FillListViewPropeties()
        Dim mListViewItems As New List(Of ClassListViewItemData)

        For Each mItem In g_ClassProperties.m_PropertiesTypeDefault
            Dim mListViewItem As New ClassListViewItemData(New String() {mItem.Value.sDescription, mItem.Value.sReplace})
            mListViewItem.g_mData("Key") = mItem.Key
            mListViewItem.g_mData("Property") = mItem.Value
            mListViewItems.Add(mListViewItem)
        Next

        For Each mItem In g_ClassProperties.m_PropertiesTypeBoolean
            Dim mListViewItem As New ClassListViewItemData(New String() {mItem.Value.sDescription, If(mItem.Value.bValue, "True", "False")})
            mListViewItem.g_mData("Key") = mItem.Key
            mListViewItem.g_mData("Property") = mItem.Value
            mListViewItems.Add(mListViewItem)
        Next

        For Each mItem In g_ClassProperties.m_PropertiesTypeList
            Dim mListViewItem As New ClassListViewItemData(New String() {mItem.Value.sDescription, mItem.Value.mReplace(mItem.Value.iIndex).sItemDescription})
            mListViewItem.g_mData("Key") = mItem.Key
            mListViewItem.g_mData("Property") = mItem.Value
            mListViewItems.Add(mListViewItem)
        Next

        ListView_Properties.Items.Clear()
        ListView_Properties.Items.AddRange(mListViewItems.ToArray)
        ClassTools.ClassControls.ClassListView.AutoResizeColumns(ListView_Properties)
    End Sub

    Private Sub UpdatePreview()
        g_sPreviewTemplate = g_ClassProperties.GenerateSource()

        RichTextBox_Preview.Text = g_sPreviewTemplate
    End Sub

    Private Sub ListView_Properties_Click(sender As Object, e As EventArgs) Handles ListView_Properties.Click
        Try
            ContextMenuStrip_Properties.Close()
            g_mSelectedPropertyItem = Nothing

            If (ListView_Properties.SelectedItems.Count < 1) Then
                Return
            End If

            If (TypeOf ListView_Properties.SelectedItems(0) IsNot ClassListViewItemData) Then
                Return
            End If

            g_mSelectedPropertyItem = DirectCast(ListView_Properties.SelectedItems(0), ClassListViewItemData)

            ContextMenuStrip_Properties.Show(ListView_Properties, New Point(g_mSelectedPropertyItem.Bounds.Location.X, g_mSelectedPropertyItem.Bounds.Location.Y + g_mSelectedPropertyItem.Bounds.Height))
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ContextMenuStrip_Properties_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Properties.Opening
        Try
            ClassControlStyle.UpdateControls(ContextMenuStrip_Properties)

            If (g_mSelectedPropertyItem Is Nothing OrElse
                        TypeOf g_mSelectedPropertyItem IsNot ClassListViewItemData) Then
                e.Cancel = True
                Return
            End If

            Dim mUnkProperty As Object = g_mSelectedPropertyItem.g_mData("Property")

            Select Case (True)
                Case (TypeOf mUnkProperty Is ClassProperties.STRUC_DEFAULT_REPLACE)
                    Dim mProperty = DirectCast(mUnkProperty, ClassProperties.STRUC_DEFAULT_REPLACE)

                    g_bIgnoreComboBoxSetPropertyEvents = True
                    ToolStripComboBox_SetProperty.Items.Clear()
                    ToolStripComboBox_SetProperty.DropDownStyle = ComboBoxStyle.DropDown
                    ToolStripComboBox_SetProperty.Text = mProperty.sReplace
                    g_bIgnoreComboBoxSetPropertyEvents = False

                Case (TypeOf mUnkProperty Is ClassProperties.STRUC_BOOLEAN_REPLACE)
                    Dim mProperty = DirectCast(mUnkProperty, ClassProperties.STRUC_BOOLEAN_REPLACE)

                    g_bIgnoreComboBoxSetPropertyEvents = True
                    ToolStripComboBox_SetProperty.Items.Clear()
                    ToolStripComboBox_SetProperty.DropDownStyle = ComboBoxStyle.DropDownList
                    ToolStripComboBox_SetProperty.Items.Add("True")
                    ToolStripComboBox_SetProperty.Items.Add("False")
                    ToolStripComboBox_SetProperty.SelectedIndex = If(mProperty.bValue, 0, 1)
                    g_bIgnoreComboBoxSetPropertyEvents = False

                Case (TypeOf mUnkProperty Is ClassProperties.STRUC_LIST_REPLACE)
                    Dim mProperty = DirectCast(mUnkProperty, ClassProperties.STRUC_LIST_REPLACE)

                    g_bIgnoreComboBoxSetPropertyEvents = True
                    ToolStripComboBox_SetProperty.Items.Clear()
                    ToolStripComboBox_SetProperty.DropDownStyle = ComboBoxStyle.DropDownList

                    For Each mItem In mProperty.mReplace
                        ToolStripComboBox_SetProperty.Items.Add(mItem.sItemDescription)
                    Next

                    ToolStripComboBox_SetProperty.SelectedIndex = mProperty.iIndex
                    g_bIgnoreComboBoxSetPropertyEvents = False

                Case Else
                    e.Cancel = True
                    Return
            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub SetPropertiesByComboBox()
        Try
            If (g_mSelectedPropertyItem Is Nothing OrElse
                        TypeOf g_mSelectedPropertyItem IsNot ClassListViewItemData) Then
                Return
            End If

            Dim mUnkProperty As Object = g_mSelectedPropertyItem.g_mData("Property")
            Dim sKey As String = CStr(g_mSelectedPropertyItem.g_mData("Key"))

            Select Case (True)
                Case (TypeOf mUnkProperty Is ClassProperties.STRUC_DEFAULT_REPLACE AndAlso
                            g_ClassProperties.m_PropertiesTypeDefault.ContainsKey(sKey))
                    Dim mProperty = g_ClassProperties.m_PropertiesTypeDefault(sKey)

                    mProperty.sReplace = ToolStripComboBox_SetProperty.Text

                    g_ClassProperties.m_PropertiesTypeDefault(sKey) = mProperty

                Case (TypeOf mUnkProperty Is ClassProperties.STRUC_BOOLEAN_REPLACE AndAlso
                            g_ClassProperties.m_PropertiesTypeBoolean.ContainsKey(sKey))
                    Dim mProperty = g_ClassProperties.m_PropertiesTypeBoolean(sKey)

                    mProperty.bValue = (ToolStripComboBox_SetProperty.SelectedIndex = 0)

                    g_ClassProperties.m_PropertiesTypeBoolean(sKey) = mProperty

                Case (TypeOf mUnkProperty Is ClassProperties.STRUC_LIST_REPLACE AndAlso
                            g_ClassProperties.m_PropertiesTypeList.ContainsKey(sKey))
                    Dim mProperty = g_ClassProperties.m_PropertiesTypeList(sKey)

                    mProperty.iIndex = ToolStripComboBox_SetProperty.SelectedIndex

                    g_ClassProperties.m_PropertiesTypeList(sKey) = mProperty
            End Select

            FillListViewPropeties()
            UpdatePreview()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripComboBox_SetProperty_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox_SetProperty.SelectedIndexChanged
        If (g_bIgnoreComboBoxSetPropertyEvents) Then
            Return
        End If

        If (ToolStripComboBox_SetProperty.DropDownStyle <> ComboBoxStyle.DropDownList) Then
            Return
        End If

        SetPropertiesByComboBox()
        ContextMenuStrip_Properties.Close()
    End Sub

    Private Sub ToolStripComboBox_SetProperty_KeyDown(sender As Object, e As KeyEventArgs) Handles ToolStripComboBox_SetProperty.KeyDown
        If (g_bIgnoreComboBoxSetPropertyEvents) Then
            Return
        End If

        If (ToolStripComboBox_SetProperty.DropDownStyle <> ComboBoxStyle.DropDown) Then
            Return
        End If

        Select Case (e.KeyCode)
            Case Keys.Enter
                e.Handled = True
                e.SuppressKeyPress = True

                SetPropertiesByComboBox()
                ContextMenuStrip_Properties.Close()

        End Select
    End Sub

    Private Sub ToolStripMenuItem_OpenDir_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_OpenDir.Click
        Try
            If (TreeView_Explorer.SelectedNode Is Nothing) Then
                Return
            End If

            Dim sPath As String = TreeView_Explorer.SelectedNode.FullPath

            If (IO.File.Exists(sPath)) Then
                sPath = IO.Path.GetDirectoryName(sPath)
            End If

            If (Not IO.Directory.Exists(sPath)) Then
                Return
            End If

            Process.Start("explorer.exe", sPath)
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_DelTemplate_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DelTemplate.Click
        Try
            If (TreeView_Explorer.SelectedNode Is Nothing) Then
                Return
            End If

            If (IO.Directory.Exists(TreeView_Explorer.SelectedNode.FullPath)) Then
                My.Computer.FileSystem.DeleteDirectory(TreeView_Explorer.SelectedNode.FullPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.ThrowException)

            ElseIf (IO.File.Exists(TreeView_Explorer.SelectedNode.FullPath)) Then
                My.Computer.FileSystem.DeleteFile(TreeView_Explorer.SelectedNode.FullPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin, FileIO.UICancelOption.ThrowException)
            End If

            FillTemplateExplorer()
            FillListViewPropeties()
            UpdatePreview()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

#End Region
End Class