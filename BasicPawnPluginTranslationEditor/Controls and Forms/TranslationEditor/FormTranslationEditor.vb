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



Public Class FormTranslationEditor
    Private g_mPluginTranslationEditor As PluginTranslationEditor
    Private g_TreeViewColumns As ClassTreeViewColumns

    Public g_mPluginConfigRecent As ClassPluginController.ClassPluginConfig

    Public g_ClassTranslationManager As ClassTranslationManager
    Public g_ClassRecentTranslations As ClassRecentTranslations

    Private g_mRecentToolMenus As New List(Of ToolStripMenuItem)

    Enum ENUM_TRANSLATION_IMAGE_INDEX
        MAIN
        ENTRY_MASTER
        ENTRY_SLAVE
        MISSING
    End Enum

    Public Sub New(mPluginTranslationEditor As PluginTranslationEditor)
        g_mPluginTranslationEditor = mPluginTranslationEditor

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        ImageList_Translation.Images.Clear()
        ImageList_Translation.Images.Add(CStr(ENUM_TRANSLATION_IMAGE_INDEX.MAIN), My.Resources.accessibilitycpl_325_16x16_32)
        ImageList_Translation.Images.Add(CStr(ENUM_TRANSLATION_IMAGE_INDEX.ENTRY_MASTER), My.Resources.netcenter_7_16x16_32)
        ImageList_Translation.Images.Add(CStr(ENUM_TRANSLATION_IMAGE_INDEX.ENTRY_SLAVE), My.Resources.shell32_157_16x16_32)
        ImageList_Translation.Images.Add(CStr(ENUM_TRANSLATION_IMAGE_INDEX.MISSING), My.Resources.imageres_5337_16x16_32) 'imageres_5337_16x16_32 / shell32_261_16x16_32

        g_TreeViewColumns = New ClassTreeViewColumns
        g_TreeViewColumns.m_Columns.Add("Language", 100)
        g_TreeViewColumns.m_Columns.Add("Format", 100)
        g_TreeViewColumns.m_Columns.Add("Text", 450)
        g_TreeViewColumns.m_TreeView.ImageList = ImageList_Translation
        g_TreeViewColumns.Parent = Me
        g_TreeViewColumns.Dock = DockStyle.Fill
        g_TreeViewColumns.BringToFront()

        AddHandler g_TreeViewColumns.m_TreeView.MouseClick, AddressOf OnTreeViewClick

        g_mPluginConfigRecent = New ClassPluginController.ClassPluginConfig("PluginTranslationEditorRecent")

        g_ClassTranslationManager = New ClassTranslationManager(Me)
        g_ClassRecentTranslations = New ClassRecentTranslations(Me)
    End Sub

    Private Sub OnTreeViewClick(sender As Object, e As MouseEventArgs)
        If (e.Button <> MouseButtons.Right) Then
            Return
        End If

        ContextMenuStrip_Translation.Show(DirectCast(sender, Control), e.Location)
    End Sub

    Private Sub FormTranslationEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub FormTranslationEditor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If (g_ClassTranslationManager.m_Changed) Then
            If (MessageBox.Show("All translation changes will be disregarded! Continue?", "Translation not saved", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = DialogResult.Cancel) Then
                e.Cancel = True
                Return
            End If
        End If
    End Sub

    Private Sub FormTranslationEditor_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub CleanUp()
        If (g_TreeViewColumns IsNot Nothing AndAlso g_TreeViewColumns.m_TreeView IsNot Nothing) Then
            RemoveHandler g_TreeViewColumns.m_TreeView.MouseClick, AddressOf OnTreeViewClick
        End If

        For Each mToolItem In g_mRecentToolMenus
            If (mToolItem.IsDisposed) Then
                Continue For
            End If

            RemoveHandler mToolItem.Click, AddressOf OnRecentToolStripItem_Click

            mToolItem.Dispose()
        Next

        g_mRecentToolMenus.Clear()

        If (g_TreeViewColumns IsNot Nothing AndAlso Not g_TreeViewColumns.IsDisposed) Then
            g_TreeViewColumns.Dispose()
            g_TreeViewColumns = Nothing
        End If
    End Sub

    Class ClassTranslationManager
        Private g_mFormTranslationEditor As FormTranslationEditor

        Private g_sFile As String = ""
        Private g_bIsPacked As Boolean = False
        Private g_bChanged As Boolean = False

        Public Sub New(mFormTranslationEditor As FormTranslationEditor)
            g_mFormTranslationEditor = mFormTranslationEditor
        End Sub

        ReadOnly Property m_Translations As ClassTranslation()
            Get
                Dim mTranslations As New List(Of ClassTranslation)

                Dim mTreeView = g_mFormTranslationEditor.g_TreeViewColumns.m_TreeView

                For Each mNode As TreeNode In mTreeView.Nodes
                    Dim mNodeName = TryCast(mNode, ClassTranslationTreeNode)
                    If (mNodeName Is Nothing) Then
                        Continue For
                    End If

                    If (mNodeName.m_NodeType <> ClassTranslationTreeNode.ENUM_NODE_TYPE.NAME) Then
                        Continue For
                    End If

                    Dim mTranslation As New ClassTranslation(mNodeName.m_Name)

                    For Each mSubNode As TreeNode In mNode.Nodes
                        Dim mNodeKey = TryCast(mSubNode, ClassTranslationTreeNode)
                        If (mNodeKey Is Nothing) Then
                            Continue For
                        End If

                        If (mNodeKey.m_NodeType = ClassTranslationTreeNode.ENUM_NODE_TYPE.NAME) Then
                            Continue For
                        End If

                        If (mNodeKey.m_NodeType = ClassTranslationTreeNode.ENUM_NODE_TYPE.MISSING_ITEM) Then
                            Continue For
                        End If

                        mTranslation.m_TranslationItems.Add(New ClassTranslation.ClassTranslationItem(mNodeKey.m_Language, mNodeKey.m_Format, mNodeKey.m_Text))
                    Next

                    mTranslations.Add(mTranslation)
                Next

                Return mTranslations.ToArray
            End Get
        End Property

        Property m_File As String
            Get
                Return g_sFile
            End Get
            Set(value As String)
                If (g_sFile <> value) Then
                    g_sFile = value

                    UpdateTitle()
                End If
            End Set
        End Property

        ReadOnly Property m_IsPacked As Boolean
            Get
                Return g_bIsPacked
            End Get
        End Property

        Property m_Changed As Boolean
            Get
                Return g_bChanged
            End Get
            Set(value As Boolean)
                If (g_bChanged <> value) Then
                    g_bChanged = value

                    UpdateTitle()
                End If
            End Set
        End Property

        Public Sub UpdateTitle()
            If (String.IsNullOrEmpty(g_sFile)) Then
                g_mFormTranslationEditor.Text = String.Format("Translation Editor{0}", If(g_bChanged, "*", ""))
            Else
                g_mFormTranslationEditor.Text = String.Format("Translation Editor ({0}){1}", IO.Path.GetFileName(g_sFile), If(g_bChanged, "*", ""))
            End If
        End Sub

        Public Sub LoadTranslation(sFile As String, bLoadAdditionalFiles As Boolean)
            LoadTranslation(sFile, bLoadAdditionalFiles, True)
        End Sub

        Public Sub LoadTranslation(sFile As String, bLoadAdditionalFiles As Boolean, bOptimizations As Boolean)
            Dim mTranslations As New List(Of ClassTranslation)

            Using mKeyValue As New ClassKeyValues(sFile, IO.FileMode.OpenOrCreate)
                mTranslations.AddRange(ParseFromKeyValues(mKeyValue.Deserialize(True)))
            End Using

            Dim bIsPacked As Boolean = True

            If (bLoadAdditionalFiles) Then
                'Has additional files?
                Dim sAdditionalFiles As String() = FindAdditionalFiles(sFile)

                For Each sAdditionalFile In sAdditionalFiles
                    Using mKeyValue As New ClassKeyValues(sAdditionalFile, IO.FileMode.OpenOrCreate)
                        mTranslations.AddRange(ParseFromKeyValues(mKeyValue.Deserialize(True)))
                    End Using

                    bIsPacked = False
                Next

                'Merge duplicated translations
                Dim mMergedTranslations As New Dictionary(Of String, ClassTranslation)
                For Each mTranslation In mTranslations
                    Dim sSafeTranslationName As String = ClassTools.ClassStrings.ToSafeKey(mTranslation.m_Name)

                    If (Not mMergedTranslations.ContainsKey(sSafeTranslationName)) Then
                        mMergedTranslations(sSafeTranslationName) = New ClassTranslation(mTranslation.m_Name)
                    End If

                    For Each mItem In mTranslation.m_TranslationItems
                        Dim mTranslationItems = mMergedTranslations(sSafeTranslationName).m_TranslationItems

                        If (mTranslationItems.Exists(Function(x As ClassTranslation.ClassTranslationItem)
                                                         Return (x.m_Language = mItem.m_Language)
                                                     End Function)) Then
                            Continue For
                        End If

                        mTranslationItems.Add(mItem)
                    Next
                Next

                mTranslations = New List(Of ClassTranslation)

                For Each mItem In mMergedTranslations
                    mTranslations.Add(mItem.Value)
                Next
            End If

            If (bOptimizations) Then
                For Each mTranslation In mTranslations
                    Dim mFormats As New HashSet(Of String)

                    For Each mItem In mTranslation.m_TranslationItems
                        mFormats.Add(mItem.m_Format)
                    Next

                    'Do we have different formating rules? Abort merging!
                    If (mFormats.Count > 1) Then
                        Continue For
                    End If

                    Dim sMasterFormat As String = mFormats(0)
                    Dim bMasterLangFound As Boolean = False

                    Dim mNewItem As New List(Of ClassTranslation.ClassTranslationItem)
                    For Each mItem In mTranslation.m_TranslationItems
                        If (mItem.m_Language = "en") Then
                            bMasterLangFound = True

                            mNewItem.Add(New ClassTranslation.ClassTranslationItem(mItem.m_Language, sMasterFormat, mItem.m_Text))
                        Else
                            mNewItem.Add(New ClassTranslation.ClassTranslationItem(mItem.m_Language, "", mItem.m_Text))
                        End If
                    Next

                    If (Not bMasterLangFound) Then
                        mNewItem.Add(New ClassTranslation.ClassTranslationItem("en", sMasterFormat, ""))
                    End If

                    mTranslation.m_TranslationItems.Clear()
                    mTranslation.m_TranslationItems.AddRange(mNewItem)
                Next
            End If

            TreeViewLoadTranslations(mTranslations.ToArray)

            'UI stuff
            g_sFile = sFile
            g_bIsPacked = bIsPacked
            g_bChanged = False
            UpdateTitle()
        End Sub

        Public Sub SaveTranslationSingle(sFile As String)
            If (String.IsNullOrEmpty(sFile)) Then
                Throw New ArgumentException("File path can not be NULL")
            End If

            IO.File.WriteAllText(sFile, "")

            Using mKeyValue As New ClassKeyValues(sFile, IO.FileMode.OpenOrCreate)
                Dim mKVRoot As New ClassKeyValues.STRUC_KEYVALUES_SECTION(Nothing)
                Dim mKVPhrases As New ClassKeyValues.STRUC_KEYVALUES_SECTION("Phrases")
                mKVRoot.m_Sections.Add(mKVPhrases)

                For Each mTranslation In m_Translations.ToArray
                    Dim mKVName As New ClassKeyValues.STRUC_KEYVALUES_SECTION(mTranslation.m_Name)
                    mKVPhrases.m_Sections.Add(mKVName)

                    For Each mItem In mTranslation.m_TranslationItems
                        If (Not String.IsNullOrEmpty(mItem.m_Format)) Then
                            'Only add #format once
                            If (Not mKVName.m_Keys.Exists(Function(x As KeyValuePair(Of String, String))
                                                              Return x.Key = "#format"
                                                          End Function)) Then
                                mKVName.m_Keys.Add(New KeyValuePair(Of String, String)("#format", mItem.m_Format))
                            End If
                        End If

                        'Skip empty translations
                        If (String.IsNullOrEmpty(mItem.m_Text)) Then
                            Continue For
                        End If

                        mKVName.m_Keys.Add(New KeyValuePair(Of String, String)(mItem.m_Language, mItem.m_Text))
                    Next

                    'Sort languages and '#format' needs to be the first key!
                    mKVName.m_Keys.Sort(Function(x As KeyValuePair(Of String, String), y As KeyValuePair(Of String, String))
                                            If (y.Key = "#format") Then
                                                Return 1
                                            End If

                                            Return (x.Key.CompareTo(y.Key))
                                        End Function)
                Next

                mKeyValue.Serialize(mKVRoot, True)
            End Using

            g_sFile = sFile
            g_bIsPacked = True
            g_bChanged = False
            UpdateTitle()
        End Sub

        Public Sub SaveTranslationMulti(sFile As String)
            If (String.IsNullOrEmpty(sFile)) Then
                Throw New ArgumentException("File path can not be NULL")
            End If

            IO.File.WriteAllText(sFile, "")

            Dim mLangTranslations = TranslationSplitByLanguage(m_Translations.ToArray)
            Dim sFileDirectory As String = IO.Path.GetDirectoryName(sFile)
            Dim sFileName As String = IO.Path.GetFileName(sFile)

            For Each sLang In mLangTranslations.Keys
                'English is master file
                If (sLang = "en") Then
                    Using mKeyValue As New ClassKeyValues(sFile, IO.FileMode.OpenOrCreate)
                        Dim mKVRoot As New ClassKeyValues.STRUC_KEYVALUES_SECTION(Nothing)
                        Dim mKVPhrases As New ClassKeyValues.STRUC_KEYVALUES_SECTION("Phrases")
                        mKVRoot.m_Sections.Add(mKVPhrases)

                        For Each mTranslation In mLangTranslations(sLang).ToArray
                            Dim mKVName As New ClassKeyValues.STRUC_KEYVALUES_SECTION(mTranslation.m_Name)
                            mKVPhrases.m_Sections.Add(mKVName)

                            For Each mItem In mTranslation.m_TranslationItems
                                If (Not String.IsNullOrEmpty(mItem.m_Format)) Then
                                    mKVName.m_Keys.Add(New KeyValuePair(Of String, String)("#format", mItem.m_Format))
                                End If

                                'Skip empty translations
                                If (String.IsNullOrEmpty(mItem.m_Text)) Then
                                    Continue For
                                End If

                                mKVName.m_Keys.Add(New KeyValuePair(Of String, String)(mItem.m_Language, mItem.m_Text))
                            Next

                            'Sort languages and '#format' needs to be the first key!
                            mKVName.m_Keys.Sort(Function(x As KeyValuePair(Of String, String), y As KeyValuePair(Of String, String))
                                                    If (y.Key = "#format") Then
                                                        Return 1
                                                    End If

                                                    Return (x.Key.CompareTo(y.Key))
                                                End Function)
                        Next

                        mKeyValue.Serialize(mKVRoot, True)
                    End Using
                Else
                    Dim sNewDirectory As String = IO.Path.Combine(sFileDirectory, sLang)
                    Dim sNewFile As String = IO.Path.Combine(IO.Path.Combine(sFileDirectory, sLang), sFileName)

                    If (Not IO.Directory.Exists(sNewDirectory)) Then
                        IO.Directory.CreateDirectory(sNewDirectory)
                    End If

                    IO.File.WriteAllText(sNewFile, "")

                    Using mKeyValue As New ClassKeyValues(sNewFile, IO.FileMode.OpenOrCreate)
                        Dim mKVRoot As New ClassKeyValues.STRUC_KEYVALUES_SECTION(Nothing)
                        Dim mKVPhrases As New ClassKeyValues.STRUC_KEYVALUES_SECTION("Phrases")
                        mKVRoot.m_Sections.Add(mKVPhrases)

                        For Each mTranslation In mLangTranslations(sLang).ToArray
                            Dim mKVName As New ClassKeyValues.STRUC_KEYVALUES_SECTION(mTranslation.m_Name)
                            mKVPhrases.m_Sections.Add(mKVName)

                            For Each mItem In mTranslation.m_TranslationItems
                                If (Not String.IsNullOrEmpty(mItem.m_Format)) Then
                                    mKVName.m_Keys.Add(New KeyValuePair(Of String, String)("#format", mItem.m_Format))
                                End If

                                'Skip empty translations
                                If (String.IsNullOrEmpty(mItem.m_Text)) Then
                                    Continue For
                                End If

                                mKVName.m_Keys.Add(New KeyValuePair(Of String, String)(mItem.m_Language, mItem.m_Text))
                            Next

                            'Sort languages and '#format' needs to be the first key!
                            mKVName.m_Keys.Sort(Function(x As KeyValuePair(Of String, String), y As KeyValuePair(Of String, String))
                                                    If (y.Key = "#format") Then
                                                        Return 1
                                                    End If

                                                    Return (x.Key.CompareTo(y.Key))
                                                End Function)
                        Next

                        mKeyValue.Serialize(mKVRoot, True)
                    End Using
                End If
            Next

            g_sFile = sFile
            g_bIsPacked = False
            g_bChanged = False
            UpdateTitle()
        End Sub

        Public Sub CloseTranslation()
            TreeViewLoadTranslations({})

            g_sFile = ""
            g_bIsPacked = False
            g_bChanged = False
            UpdateTitle()
        End Sub

        Public Function TranslationSplitByLanguage(mTranslations As ClassTranslation()) As Dictionary(Of String, List(Of ClassTranslation))
            Dim mTranslationsAnon As New List(Of ClassTranslation)
            Dim mKnownLanguages As New HashSet(Of String)

            'Anonymize items
            For Each mTranslation In mTranslations
                For Each mItem In mTranslation.m_TranslationItems
                    mKnownLanguages.Add(mItem.m_Language)

                    Dim mNewTranslation As New ClassTranslation(mTranslation.m_Name)
                    mNewTranslation.m_TranslationItems.Add(New ClassTranslation.ClassTranslationItem(mItem.m_Language, mItem.m_Format, mItem.m_Text))
                    mTranslationsAnon.Add(mNewTranslation)
                Next
            Next

            Dim mTranslationsSplited As New Dictionary(Of String, List(Of ClassTranslation))
            For Each sLanguage In mKnownLanguages
                If (Not mTranslationsSplited.ContainsKey(sLanguage)) Then
                    mTranslationsSplited(sLanguage) = New List(Of ClassTranslation)
                End If

                For Each mTranslation In mTranslationsAnon
                    Dim mTransItem = mTranslation.m_TranslationItems(0)
                    If (mTransItem.m_Language <> sLanguage) Then
                        Continue For
                    End If

                    Dim mNewTranslation As New ClassTranslation(mTranslation.m_Name)
                    mNewTranslation.m_TranslationItems.Add(mTransItem)
                    mTranslationsSplited(sLanguage).Add(mNewTranslation)
                Next
            Next

            Return mTranslationsSplited
        End Function

        Public Function TranslationCollapseByLangauge(mTranslations As Dictionary(Of String, List(Of ClassTranslation))) As ClassTranslation()
            Dim mTranslationsAnon As New List(Of ClassTranslation)
            Dim mKnownNames As New HashSet(Of String)

            'Anonymize items 
            For Each sKey As String In mTranslations.Keys
                Dim mTranslationByLang = mTranslations(sKey)

                For Each mTranslation In mTranslationByLang
                    mKnownNames.Add(mTranslation.m_Name)

                    For Each mTransItem In mTranslation.m_TranslationItems
                        Dim mNewTranslation As New ClassTranslation(mTranslation.m_Name)
                        mNewTranslation.m_TranslationItems.Add(mTransItem)
                        mTranslationsAnon.Add(mNewTranslation)
                    Next
                Next
            Next

            Dim mTranslationsSorted As New Dictionary(Of String, List(Of ClassTranslation.ClassTranslationItem))
            For Each sName In mKnownNames
                Dim sSafeName As String = ClassTools.ClassStrings.ToSafeKey(sName)

                If (Not mTranslationsSorted.ContainsKey(sSafeName)) Then
                    mTranslationsSorted(sSafeName) = New List(Of ClassTranslation.ClassTranslationItem)
                End If

                For Each mItem In mTranslationsAnon
                    If (mItem.m_Name <> sName) Then
                        Continue For
                    End If

                    mTranslationsSorted(sSafeName).Add(mItem.m_TranslationItems(0))
                Next
            Next

            Dim mTranslationsMerged As New List(Of ClassTranslation)
            For Each sSafeKey As String In mTranslationsSorted.Keys
                Dim sKey As String = ClassTools.ClassStrings.ToUnsafeKey(sSafeKey)

                Dim mTranslation As New ClassTranslation(sKey)

                mTranslation.m_TranslationItems.AddRange(mTranslationsSorted(sSafeKey))

                mTranslationsMerged.Add(mTranslation)
            Next

            Return mTranslationsMerged.ToArray
        End Function

        Public Function ParseFromKeyValues(mKeyValue As ClassKeyValues.STRUC_KEYVALUES_SECTION) As ClassTranslation()
            mKeyValue = mKeyValue.m_Root

            Dim mPhrasesSection = mKeyValue.FindSection("Phrases")
            If (mPhrasesSection Is Nothing) Then
                Throw New ArgumentException("Unable to find 'Phrases'")
            End If

            Dim mTranslationItems As New List(Of ClassTranslation)

            For Each mSection In mPhrasesSection.m_Sections
                Dim mTranslation As New ClassTranslation(mSection.m_Name)

                mTranslation.m_TranslationItems.AddRange(ParseFromKeyValues(mSection.m_Name, mKeyValue))

                mTranslationItems.Add(mTranslation)
            Next

            Return mTranslationItems.ToArray
        End Function

        Public Function ParseFromKeyValues(sName As String, mKeyValue As ClassKeyValues.STRUC_KEYVALUES_SECTION) As ClassTranslation.ClassTranslationItem()
            mKeyValue = mKeyValue.m_Root

            Dim mPhrasesSection = mKeyValue.FindSection("Phrases")
            If (mPhrasesSection Is Nothing) Then
                Throw New ArgumentException("Unable to find 'Phrases'")
            End If

            Dim mTranslationSection = mPhrasesSection.FindSection(sName)
            If (mTranslationSection Is Nothing) Then
                Throw New ArgumentException("Unable to find '" & sName & "'")
            End If

            Dim mTranslationItems As New List(Of ClassTranslation.ClassTranslationItem)

            Dim sFormat As String = ""

            For Each mKey In mTranslationSection.m_Keys
                Select Case (mKey.Key)
                    Case "#format"
                        sFormat = mKey.Value
                        Exit For
                End Select
            Next

            For Each mKey In mTranslationSection.m_Keys
                Select Case (mKey.Key)
                    Case "#format"
                        'Nothing
                    Case Else
                        mTranslationItems.Add(New ClassTranslation.ClassTranslationItem(mKey.Key, sFormat, mKey.Value))
                End Select
            Next

            Return mTranslationItems.ToArray
        End Function

        Public Sub TreeViewLoadTranslations(mTranslations As ClassTranslation())
            Dim mTreeView = g_mFormTranslationEditor.g_TreeViewColumns.m_TreeView

            Try
                mTreeView.BeginUpdate()
                mTreeView.Nodes.Clear()

                Dim mKnownNodes As New List(Of KeyValuePair(Of TreeNode, ClassTranslationTreeNode))

                For Each mTranslation In mTranslations
                    Dim mNode As TreeNode
                    If (mTreeView.Nodes.ContainsKey(mTranslation.m_Name)) Then
                        mNode = mTreeView.Nodes(mTranslation.m_Name)
                    Else
                        mNode = New ClassTranslationTreeNode(mTranslation.m_Name)

                        mTreeView.Nodes.Add(mNode)
                    End If

                    For Each mTranslationItem In mTranslation.m_TranslationItems
                        'Master language
                        If (mTranslationItem.m_Language = "en") Then
                            mNode.Nodes.Add(New ClassTranslationTreeNode(mTranslation.m_Name, mTranslationItem.m_Language, mTranslationItem.m_Format, mTranslationItem.m_Text, ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER))
                        Else
                            mNode.Nodes.Add(New ClassTranslationTreeNode(mTranslation.m_Name, mTranslationItem.m_Language, mTranslationItem.m_Format, mTranslationItem.m_Text, ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE))
                        End If

                        For Each mKnownNode In mKnownNodes.ToArray
                            If (mKnownNode.Key IsNot mNode) Then
                                Continue For
                            End If

                            If (mKnownNode.Value.m_Language <> mTranslationItem.m_Language) Then
                                Continue For
                            End If

                            mNode.Nodes.Remove(mKnownNode.Value)
                        Next
                    Next
                Next
            Finally
                mTreeView.Sort()
                mTreeView.EndUpdate()
            End Try
        End Sub

        Public Sub TreeViewFillMissing(bRemoveOnly As Boolean)
            Dim mTreeView = g_mFormTranslationEditor.g_TreeViewColumns.m_TreeView

            Try
                mTreeView.BeginUpdate()

                If (bRemoveOnly) Then
                    Dim mToRemove As New List(Of ClassTranslationTreeNode)

                    For Each mRootNode As TreeNode In mTreeView.Nodes
                        Dim mTransRootNode = TryCast(mRootNode, ClassTranslationTreeNode)
                        If (mTransRootNode Is Nothing) Then
                            Return
                        End If

                        For Each mNode As TreeNode In mTransRootNode.Nodes
                            Dim mTransNode = TryCast(mNode, ClassTranslationTreeNode)
                            If (mTransNode Is Nothing) Then
                                Return
                            End If

                            If (mTransNode.m_NodeType <> ClassTranslationTreeNode.ENUM_NODE_TYPE.MISSING_ITEM) Then
                                Continue For
                            End If

                            mToRemove.Add(mTransNode)
                        Next
                    Next

                    For Each mNode In mToRemove
                        mNode.Remove()
                    Next
                Else
                    Dim mKnownLanguages = GetKnownLangauges()

                    For Each mRootNode As TreeNode In mTreeView.Nodes
                        Dim mTransRootNode = TryCast(mRootNode, ClassTranslationTreeNode)
                        If (mTransRootNode Is Nothing) Then
                            Return
                        End If

                        For Each mKnownLangauge In mKnownLanguages
                            Dim bFound As Boolean = False

                            For Each mNode As TreeNode In mTransRootNode.Nodes
                                Dim mTransNode = TryCast(mNode, ClassTranslationTreeNode)
                                If (mTransNode Is Nothing) Then
                                    Return
                                End If

                                If (mTransNode.m_Language <> mKnownLangauge.Key) Then
                                    Continue For
                                End If

                                bFound = True
                                Exit For
                            Next

                            If (Not bFound) Then
                                mTransRootNode.Nodes.Add(New ClassTranslationTreeNode(mTransRootNode.m_Name, mKnownLangauge.Key, "", "", ClassTranslationTreeNode.ENUM_NODE_TYPE.MISSING_ITEM))
                            End If
                        Next
                    Next
                End If
            Finally
                mTreeView.Sort()
                mTreeView.EndUpdate()
            End Try
        End Sub

        Public Function FindAdditionalFiles(sFile As String) As String()
            If (String.IsNullOrEmpty(sFile)) Then
                Return New String() {}
            End If

            Dim mFiles As New List(Of String)

            'Has sub directories?
            Dim sFileName As String = IO.Path.GetFileName(sFile)
            Dim sFileDirectory As String = IO.Path.GetDirectoryName(sFile)

            For Each sDirectory In IO.Directory.GetDirectories(sFileDirectory)
                Dim sLang As String = New IO.DirectoryInfo(sDirectory).Name

                Dim sAdditionalFile As String = IO.Path.Combine(sDirectory, sFileName)
                If (Not IO.File.Exists(sAdditionalFile)) Then
                    Continue For
                End If

                mFiles.Add(sAdditionalFile)
            Next

            Return mFiles.ToArray
        End Function

        Public Function GetKnownLangauges() As KeyValuePair(Of String, String)()
            Static mLanguages As Dictionary(Of String, String) = Nothing

            If (mLanguages Is Nothing) Then
                mLanguages = New Dictionary(Of String, String)
                mLanguages("en") = "English"
                mLanguages("ar") = "Arabic"
                mLanguages("pt") = "Brazilian"
                mLanguages("bg") = "Bulgarian"
                mLanguages("cze") = "Czech"
                mLanguages("da") = "Danish"
                mLanguages("nl") = "Dutch"
                mLanguages("fi") = "Finnish"
                mLanguages("fr") = "French"
                mLanguages("de") = "German"
                mLanguages("el") = "Greek"
                mLanguages("he") = "Hebrew"
                mLanguages("hu") = "Hungarian"
                mLanguages("it") = "Italian"
                mLanguages("jp") = "Japanese"
                mLanguages("ko") = "KoreanA"
                mLanguages("ko") = "Korean"
                mLanguages("lv") = "Latvian"
                mLanguages("lt") = "Lithuanian"
                mLanguages("no") = "Norwegian"
                mLanguages("pl") = "Polish"
                mLanguages("pt_p") = "Portuguese"
                mLanguages("ro") = "Romanian"
                mLanguages("ru") = "Russian"
                mLanguages("chi") = "SChinese"
                mLanguages("sk") = "Slovak"
                mLanguages("es") = "Spanish"
                mLanguages("sv") = "Swedish"
                mLanguages("zho") = "TChinese"
                mLanguages("th") = "Thai"
                mLanguages("tr") = "Turkish"
                mLanguages("ua") = "Ukrainian"
            End If

            Return mLanguages.ToArray
        End Function

        Class ClassTranslation
            Private g_sName As String

            Private g_mTranslationItems As New List(Of ClassTranslationItem)

            Public Sub New(_Name As String)
                g_sName = _Name
            End Sub

            ReadOnly Property m_Name As String
                Get
                    Return g_sName
                End Get
            End Property

            ReadOnly Property m_TranslationItems As List(Of ClassTranslationItem)
                Get
                    Return g_mTranslationItems
                End Get
            End Property

            Class ClassTranslationItem
                Private g_sLanguage As String
                Private g_sFormat As String
                Private g_sText As String

                Public Sub New(_Language As String, _Format As String, _Text As String)
                    g_sLanguage = _Language
                    g_sFormat = _Format
                    g_sText = _Text
                End Sub

                ReadOnly Property m_Language As String
                    Get
                        Return g_sLanguage
                    End Get
                End Property

                ReadOnly Property m_Format As String
                    Get
                        Return g_sFormat
                    End Get
                End Property

                ReadOnly Property m_Text As String
                    Get
                        Return g_sText
                    End Get
                End Property
            End Class
        End Class

        Class ClassTranslationTreeNode
            Inherits TreeNode

            Private g_sName As String
            Private g_sLanguage As String
            Private g_sFormat As String
            Private g_sText As String

            Enum ENUM_NODE_TYPE
                NAME
                ITEM_MASTER
                ITEM_SLAVE
                MISSING_ITEM
            End Enum

            Private g_iNodeType As ENUM_NODE_TYPE

            Public Sub New(sName As String)
                Me.New(sName, Nothing, Nothing, Nothing, ENUM_NODE_TYPE.NAME)
            End Sub

            Public Sub New(sName As String, sLangauge As String, sFormat As String, sText As String, iType As ENUM_NODE_TYPE)
                MyBase.New()

                SetInfo(sName, sLangauge, sFormat, sText, iType)
            End Sub

            ReadOnly Property m_NodeType As ENUM_NODE_TYPE
                Get
                    Return g_iNodeType
                End Get
            End Property

            ReadOnly Property m_Name As String
                Get
                    Return g_sName
                End Get
            End Property

            ReadOnly Property m_Language As String
                Get
                    Return g_sLanguage
                End Get
            End Property

            ReadOnly Property m_Format As String
                Get
                    Return g_sFormat
                End Get
            End Property

            ReadOnly Property m_Text As String
                Get
                    Return g_sText
                End Get
            End Property

            Public Sub SetInfo(sName As String, sLangauge As String, sFormat As String, sText As String, iType As ENUM_NODE_TYPE)
                g_sName = sName
                g_sLanguage = sLangauge
                g_sFormat = sFormat
                g_sText = sText

                g_iNodeType = iType

                'Normalize display string
                Dim sDisplayText = sText
                If (Not String.IsNullOrEmpty(sDisplayText)) Then
                    sDisplayText = sDisplayText.Replace(vbCrLf, " ")
                    sDisplayText = sDisplayText.Replace(vbLf, " ")
                    sDisplayText = sDisplayText.Replace(vbTab, " ")
                End If

                Me.Text = sLangauge
                Me.Name = ClassTools.ClassStrings.ToSafeKey(sLangauge)
                Me.Tag = New String() {sFormat, sDisplayText}

                Select Case (iType)
                    Case ENUM_NODE_TYPE.NAME
                        g_sLanguage = Nothing
                        g_sFormat = Nothing
                        g_sText = Nothing

                        Me.Text = sName
                        Me.Name = ClassTools.ClassStrings.ToSafeKey(sName)
                        Me.Tag = Nothing

                        Me.ImageKey = CStr(ENUM_TRANSLATION_IMAGE_INDEX.MAIN)
                        Me.SelectedImageKey = Me.ImageKey

                    Case ENUM_NODE_TYPE.ITEM_MASTER
                        Me.ImageKey = CStr(ENUM_TRANSLATION_IMAGE_INDEX.ENTRY_MASTER)
                        Me.SelectedImageKey = Me.ImageKey

                    Case ENUM_NODE_TYPE.ITEM_SLAVE
                        Me.ImageKey = CStr(ENUM_TRANSLATION_IMAGE_INDEX.ENTRY_SLAVE)
                        Me.SelectedImageKey = Me.ImageKey

                    Case Else
                        Me.ImageKey = CStr(ENUM_TRANSLATION_IMAGE_INDEX.MISSING)
                        Me.SelectedImageKey = Me.ImageKey
                End Select
            End Sub
        End Class
    End Class

    Class ClassRecentTranslations
        Private g_mFormTranslationEditor As FormTranslationEditor

        Private g_iMaxRecentFiles As Integer = 16

        Public Sub New(mFormTranslationEditor As FormTranslationEditor)
            g_mFormTranslationEditor = mFormTranslationEditor
        End Sub

        Property m_MaxRecentFiles As Integer
            Get
                Return g_iMaxRecentFiles
            End Get
            Set(value As Integer)
                g_iMaxRecentFiles = value
            End Set
        End Property

        Public Sub AddRecent(sFile As String)
            Dim mRecentSorted As New List(Of KeyValuePair(Of String, Date))

            g_mFormTranslationEditor.g_mPluginConfigRecent.LoadConfig()

            For Each mItem In g_mFormTranslationEditor.g_mPluginConfigRecent.ReadEverything
                If (mItem.sSection <> "Recent Translations") Then
                    Continue For
                End If

                Dim tmpLng As Long
                If (Long.TryParse(mItem.sValue, tmpLng)) Then
                    mRecentSorted.Add(New KeyValuePair(Of String, Date)(mItem.sKey, New Date(tmpLng)))
                End If
            Next

            mRecentSorted.RemoveAll(Function(x As KeyValuePair(Of String, Date))
                                        Return sFile.ToLower = x.Key.ToLower
                                    End Function)

            mRecentSorted.Add(New KeyValuePair(Of String, Date)(sFile.ToLower, Now))

            mRecentSorted.Sort(Function(x As KeyValuePair(Of String, Date), y As KeyValuePair(Of String, Date))
                                   Return x.Value.Ticks.CompareTo(y.Value.Ticks)
                               End Function)

            'Clean Config
            g_mFormTranslationEditor.g_mPluginConfigRecent.ParseFromString("")

            Dim mIniContent As New List(Of ClassIni.STRUC_INI_CONTENT)

            For i = 0 To mRecentSorted.Count - 1
                If (i > g_iMaxRecentFiles) Then
                    Exit For
                End If

                mIniContent.Add(New ClassIni.STRUC_INI_CONTENT("Recent Translations", mRecentSorted(i).Key, CStr(mRecentSorted(i).Value.Ticks)))
            Next

            g_mFormTranslationEditor.g_mPluginConfigRecent.WriteKeyValue(mIniContent.ToArray)

            g_mFormTranslationEditor.g_mPluginConfigRecent.SaveConfig()
        End Sub

        Public Function GetRecent() As KeyValuePair(Of String, Date)()
            Dim mRecentSorted As New List(Of KeyValuePair(Of String, Date))

            g_mFormTranslationEditor.g_mPluginConfigRecent.LoadConfig()

            For Each mItem In g_mFormTranslationEditor.g_mPluginConfigRecent.ReadEverything
                If (mItem.sSection <> "Recent Translations") Then
                    Continue For
                End If

                Dim tmpLng As Long
                If (Long.TryParse(mItem.sValue, tmpLng)) Then
                    mRecentSorted.Add(New KeyValuePair(Of String, Date)(mItem.sKey, New Date(tmpLng)))
                End If
            Next

            mRecentSorted.Sort(Function(x As KeyValuePair(Of String, Date), y As KeyValuePair(Of String, Date))
                                   Return x.Value.Ticks.CompareTo(y.Value.Ticks)
                               End Function)

            Return mRecentSorted.ToArray
        End Function
    End Class
End Class