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


Imports System.Globalization
Imports System.Windows.Forms
Imports BasicPawn

Public Class FormTranslationEditor
    Private g_mPluginTranslationEditor As PluginTranslationEditor
    Private g_TreeViewColumns As ClassTreeViewColumns

    Public g_mSettingsSecureStorage As ClassSecureStorage
    Public g_mRecentSecureStorage As ClassSecureStorage

    Private g_ClassTranslationManager As ClassTranslationManager
    Private g_ClassRecentTranslations As ClassRecentTranslations

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
        ImageList_Translation.Images.Add(CStr(ENUM_TRANSLATION_IMAGE_INDEX.ENTRY_MASTER), My.Resources.shell32_157_16x16_32)
        ImageList_Translation.Images.Add(CStr(ENUM_TRANSLATION_IMAGE_INDEX.ENTRY_SLAVE), My.Resources.fontext_410_16x16_32)
        ImageList_Translation.Images.Add(CStr(ENUM_TRANSLATION_IMAGE_INDEX.MISSING), My.Resources.shell32_261_16x16_32)

        g_TreeViewColumns = New ClassTreeViewColumns
        g_TreeViewColumns.m_Columns.Add("Language", 100)
        g_TreeViewColumns.m_Columns.Add("Format", 100)
        g_TreeViewColumns.m_Columns.Add("Text", 450)
        g_TreeViewColumns.m_TreeView.ImageList = ImageList_Translation
        g_TreeViewColumns.Parent = Me
        g_TreeViewColumns.Dock = DockStyle.Fill
        g_TreeViewColumns.BringToFront()

        AddHandler g_TreeViewColumns.m_TreeView.MouseClick, AddressOf OnTreeViewClick

        g_mSettingsSecureStorage = New ClassSecureStorage("PluginTranslationEditorSettings")
        g_mRecentSecureStorage = New ClassSecureStorage("PluginTranslationEditorRecent")

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

    Private Sub ToolStripMenuItem_New_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_New.Click
        g_ClassTranslationManager.CloseTranslation()

        g_ClassTranslationManager.RefreshTreeView()
    End Sub

    Private Sub ToolStripMenuItem_Open_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Open.Click
        Try
            Using i As New OpenFileDialog()
                If (i.ShowDialog(Me) = DialogResult.OK) Then
                    g_ClassTranslationManager.LoadTranslation(i.FileName, True)
                    g_ClassRecentTranslations.AddRecent(i.FileName)

                    g_ClassTranslationManager.RefreshTreeView()
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

            For Each mItem In mRecentFiles
                Dim mToolItem As New ToolStripMenuItem(mItem.Key)

                g_mRecentToolMenus.Add(mToolItem)
                ToolStripMenuItem_Recent.DropDownItems.Add(mToolItem)

                RemoveHandler mToolItem.Click, AddressOf OnRecentToolStripItem_Click
                AddHandler mToolItem.Click, AddressOf OnRecentToolStripItem_Click
            Next

            ClassControlStyle.UpdateControls(Me)
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
            If (Not IO.File.Exists(sFile)) Then
                Throw New IO.FileNotFoundException("File not found")
            End If

            g_ClassTranslationManager.LoadTranslation(sFile, True)
            g_ClassTranslationManager.RefreshTreeView()
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ContextMenuStrip_Translation_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip_Translation.Opening
        Dim mSelectedNode = TryCast(g_TreeViewColumns.m_TreeView.SelectedNode, ClassTranslationManager.ClassTranslationTreeNode)
        If (mSelectedNode Is Nothing) Then
            Return
        End If

        ToolStripMenuItem_TransEdit.Enabled = (mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER OrElse
                                                    mSelectedNode.m_NodeType = ClassTranslationManager.ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE)
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

        Public g_mTranslation As ClassTranslation
        Public g_mSubTranslation As New List(Of ClassTranslation)

        Public Sub New(mFormTranslationEditor As FormTranslationEditor)
            g_mFormTranslationEditor = mFormTranslationEditor
        End Sub

        ReadOnly Property m_Translation As ClassTranslation
            Get
                Return g_mTranslation
            End Get
        End Property

        ReadOnly Property m_SubTranslation As ClassTranslation()
            Get
                Return g_mSubTranslation.ToArray
            End Get
        End Property

        ReadOnly Property m_TranslationAll As ClassTranslation()
            Get
                Dim mTranslations As New List(Of ClassTranslation)

                If (g_mTranslation IsNot Nothing) Then
                    mTranslations.Add(g_mTranslation)
                End If

                mTranslations.AddRange(g_mSubTranslation)

                Return mTranslations.ToArray
            End Get
        End Property

        ReadOnly Property m_HasSubTranslations As Boolean
            Get
                Return g_mSubTranslation.Count > 0
            End Get
        End Property

        Public Sub LoadTranslation(sFile As String, bCheckSubFolders As Boolean)
            g_mTranslation = New ClassTranslation(sFile, False)

            g_mSubTranslation.Clear()

            If (Not bCheckSubFolders) Then
                Return
            End If

            'Has sub directories?
            Dim sFileName As String = IO.Path.GetFileName(sFile)
            Dim sFileDirectory As String = IO.Path.GetDirectoryName(sFile)

            For Each sDirectory In IO.Directory.GetDirectories(sFileDirectory)
                Dim sLang As String = New IO.DirectoryInfo(sDirectory).Name

                Dim sSubFile As String = IO.Path.Combine(sDirectory, sFileName)
                If (Not IO.File.Exists(sSubFile)) Then
                    Continue For
                End If

                g_mSubTranslation.Add(New ClassTranslation(sSubFile, True))
            Next
        End Sub

        Public Sub SaveTranslation()
            g_mTranslation.Save()

            For Each mTranslation In g_mSubTranslation
                mTranslation.Save()
            Next
        End Sub

        Public Sub CloseTranslation()
            g_mTranslation = Nothing
            g_mSubTranslation.Clear()
        End Sub

        Public Sub RefreshTreeView()
            Dim mTreeView = g_mFormTranslationEditor.g_TreeViewColumns.m_TreeView

            Try
                mTreeView.BeginUpdate()
                mTreeView.Nodes.Clear()

                Dim mKnownNodes As New List(Of KeyValuePair(Of TreeNode, ClassTranslationTreeNode))

                For Each mTranslation In m_TranslationAll
                    Dim mKeyValues = mTranslation.GetTranslationKeys()
                    If (mKeyValues Is Nothing) Then
                        Throw New ArgumentException("Invalid translation")
                    End If

                    For Each mKeyValue In mKeyValues
                        Dim mNode As TreeNode
                        If (mTreeView.Nodes.ContainsKey(mKeyValue.m_Name)) Then
                            mNode = mTreeView.Nodes(mKeyValue.m_Name)
                        Else
                            mNode = New ClassTranslationTreeNode(mKeyValue.m_Name)

                            mTreeView.Nodes.Add(mNode)

                            'Pre-fill nodes for removal later
                            For Each mLang In GetKnownLangauges()
                                Dim mSubNode As New ClassTranslationTreeNode(mKeyValue.m_Name, mLang.Key, "", "", ClassTranslationTreeNode.ENUM_NODE_TYPE.MISSING_ITEM)

                                mKnownNodes.Add(New KeyValuePair(Of TreeNode, ClassTranslationTreeNode)(mNode, mSubNode))

                                mNode.Nodes.Add(mSubNode)
                            Next
                        End If

                        For Each mKeyTranslation In mKeyValue.m_Translations
                            If (Not mTranslation.m_IsSubTranslation) Then
                                mNode.Nodes.Add(New ClassTranslationTreeNode(mKeyValue.m_Name, mKeyTranslation.sLang, mKeyValue.m_Format, mKeyTranslation.sText, ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_MASTER))
                            Else
                                mNode.Nodes.Add(New ClassTranslationTreeNode(mKeyValue.m_Name, mKeyTranslation.sLang, mKeyValue.m_Format, mKeyTranslation.sText, ClassTranslationTreeNode.ENUM_NODE_TYPE.ITEM_SLAVE))
                            End If

                            For Each mKnownNode In mKnownNodes.ToArray
                                If (mKnownNode.Key IsNot mNode) Then
                                    Continue For
                                End If

                                If (mKnownNode.Value.m_Language <> mKeyTranslation.sLang) Then
                                    Continue For
                                End If

                                mNode.Nodes.Remove(mKnownNode.Value)
                            Next
                        Next
                    Next
                Next
            Finally
                mTreeView.Sort()
                mTreeView.EndUpdate()
            End Try
        End Sub

        ''' <summary>
        ''' Gets a known short langes names. Used from SourceMod 'langauges.cfg'.
        ''' </summary>
        ''' <returns></returns>
        Public Function GetKnownLangauges() As KeyValuePair(Of String, String)()
            'Static mLanguagesOS As List(Of KeyValuePair(Of String, String)) = Nothing

            'If (mLanguagesOS Is Nothing) Then
            '    mLanguagesOS = New List(Of KeyValuePair(Of String, String))

            '    For Each mInfo In CultureInfo.GetCultures(CultureTypes.AllCultures)
            '        If (Not mInfo.IsNeutralCulture) Then
            '            Continue For
            '        End If

            '        mLanguagesOS.Add(New KeyValuePair(Of String, String)(mInfo.Name, mInfo.DisplayName))
            '    Next
            'End If

            'Return mLanguagesOS.ToArray

            Static mLanguages As List(Of KeyValuePair(Of String, String)) = Nothing

            If (mLanguages Is Nothing) Then
                mLanguages = New List(Of KeyValuePair(Of String, String)) From {
                            New KeyValuePair(Of String, String)("en", "English"),
                            New KeyValuePair(Of String, String)("ar", "Arabic"),
                            New KeyValuePair(Of String, String)("pt", "Brazilian"),
                            New KeyValuePair(Of String, String)("bg", "Bulgarian"),
                            New KeyValuePair(Of String, String)("cze", "Czech"),
                            New KeyValuePair(Of String, String)("da", "Danish"),
                            New KeyValuePair(Of String, String)("nl", "Dutch"),
                            New KeyValuePair(Of String, String)("fi", "Finnish"),
                            New KeyValuePair(Of String, String)("fr", "French"),
                            New KeyValuePair(Of String, String)("de", "German"),
                            New KeyValuePair(Of String, String)("el", "Greek"),
                            New KeyValuePair(Of String, String)("he", "Hebrew"),
                            New KeyValuePair(Of String, String)("hu", "Hungarian"),
                            New KeyValuePair(Of String, String)("it", "Italian"),
                            New KeyValuePair(Of String, String)("jp", "Japanese"),
                            New KeyValuePair(Of String, String)("ko", "KoreanA"),
                            New KeyValuePair(Of String, String)("ko", "Korean"),
                            New KeyValuePair(Of String, String)("lv", "Latvian"),
                            New KeyValuePair(Of String, String)("lt", "Lithuanian"),
                            New KeyValuePair(Of String, String)("no", "Norwegian"),
                            New KeyValuePair(Of String, String)("pl", "Polish"),
                            New KeyValuePair(Of String, String)("pt_p", "Portuguese"),
                            New KeyValuePair(Of String, String)("ro", "Romanian"),
                            New KeyValuePair(Of String, String)("ru", "Russian"),
                            New KeyValuePair(Of String, String)("chi", "SChinese"),
                            New KeyValuePair(Of String, String)("sk", "Slovak"),
                            New KeyValuePair(Of String, String)("es", "Spanish"),
                            New KeyValuePair(Of String, String)("sv", "Swedish"),
                            New KeyValuePair(Of String, String)("zho", "TChinese"),
                            New KeyValuePair(Of String, String)("th", "Thai"),
                            New KeyValuePair(Of String, String)("tr", "Turkish"),
                            New KeyValuePair(Of String, String)("ua", "Ukrainian")
                        }
            End If

            Return mLanguages.ToArray
        End Function


        Class ClassTranslation
            Private g_sFile As String = ""
            Private g_bIsSubTranslation As Boolean = False
            Private g_mKeyValues As ClassKeyValues.STRUC_KEYVALUES_SECTION

            Public Sub New(sFile As String, bIsSubTranslation As Boolean)
                g_sFile = sFile
                g_bIsSubTranslation = bIsSubTranslation

                Using mKeyVal As New ClassKeyValues(g_sFile, IO.FileMode.OpenOrCreate)
                    g_mKeyValues = mKeyVal.Deserialize()
                End Using
            End Sub

            ReadOnly Property m_File As String
                Get
                    Return g_sFile
                End Get
            End Property

            ReadOnly Property m_KeyValues As ClassKeyValues.STRUC_KEYVALUES_SECTION
                Get
                    Return g_mKeyValues
                End Get
            End Property

            ReadOnly Property m_IsSubTranslation As Boolean
                Get
                    Return g_bIsSubTranslation
                End Get
            End Property

            Public Sub Save()
                Using mKeyVal As New ClassKeyValues(g_sFile, IO.FileMode.OpenOrCreate)
                    mKeyVal.Serialize(g_mKeyValues)
                End Using
            End Sub

            Public Function GetTranslationKeys() As ClassTranslationItem()
                'Is not root? 
                If (g_mKeyValues.m_Parent IsNot Nothing) Then
                    Return Nothing
                End If

                Dim mPhrasesSection = g_mKeyValues.FindSection("Phrases")
                If (mPhrasesSection Is Nothing) Then
                    Return Nothing
                End If

                Dim mTranslations As New List(Of ClassTranslationItem)

                For Each mSections In mPhrasesSection.m_Sections
                    mTranslations.Add(New ClassTranslationItem(mSections))
                Next

                Return mTranslations.ToArray
            End Function

            Public Sub SetTextByLanguage(sName As String, sLang As String, sText As String)
                Dim bSuccess As Boolean = False

                For Each mSection In GetTranslationKeys()
                    If (mSection.m_Name <> sName) Then
                        Continue For
                    End If

                    For Each mTranslation In mSection.m_Translations
                        If (mTranslation.sLang <> sLang) Then
                            Continue For
                        End If

                        mTranslation.sText = sText
                    Next
                Next
            End Sub

            Public Function GetTextByLanguage(sName As String, sLang As String) As String
                For Each mSection In GetTranslationKeys()
                    If (mSection.m_Name <> sName) Then
                        Continue For
                    End If

                    For Each mTranslation In mSection.m_Translations
                        If (mTranslation.sLang <> sLang) Then
                            Continue For
                        End If

                        Return mTranslation.sText
                    Next
                Next

                Return Nothing
            End Function

            Class ClassTranslationItem
                Private g_sName As String
                Private g_mTranslations As New List(Of STRUC_TRANSLATED_TEXT)
                Private g_sFormat As String

                Private g_mKeyValue As ClassKeyValues.STRUC_KEYVALUES_SECTION

                Structure STRUC_TRANSLATED_TEXT
                    Public sLang As String
                    Public sText As String

                    Public Sub New(_Lang As String, _Text As String)
                        sLang = _Lang
                        sText = _Text
                    End Sub
                End Structure

                Public Sub New(mKeyValue As ClassKeyValues.STRUC_KEYVALUES_SECTION)
                    g_mKeyValue = mKeyValue

                    Refresh()
                End Sub

                ReadOnly Property m_Name As String
                    Get
                        Return g_sName
                    End Get
                End Property

                ReadOnly Property m_Translations As STRUC_TRANSLATED_TEXT()
                    Get
                        Return g_mTranslations.ToArray
                    End Get
                End Property

                ReadOnly Property m_Format As String
                    Get
                        Return g_sFormat
                    End Get
                End Property

                Public Sub Refresh()
                    g_sName = g_mKeyValue.m_Name

                    g_mTranslations.Clear()

                    For Each mKey In g_mKeyValue.m_Keys
                        Select Case (mKey.Key)
                            Case "#format"
                                g_sFormat = mKey.Value

                            Case Else
                                g_mTranslations.Add(New STRUC_TRANSLATED_TEXT(mKey.Key, mKey.Value))
                        End Select
                    Next
                End Sub

                Public Sub SetTranslation(mTranslation As STRUC_TRANSLATED_TEXT)
                    Dim iIndex As Integer = g_mKeyValue.m_Keys.FindIndex(Function(x As KeyValuePair(Of String, String))
                                                                             Select Case (x.Key)
                                                                                 Case "#format"
                                                                                     Return False

                                                                                 Case Else
                                                                                     Return (x.Key = mTranslation.sLang)
                                                                             End Select
                                                                         End Function)

                    If (iIndex > -1) Then
                        g_mKeyValue.m_Keys(iIndex) = New KeyValuePair(Of String, String)(mTranslation.sLang, mTranslation.sText)
                    Else
                        g_mKeyValue.m_Keys.Add(New KeyValuePair(Of String, String)(mTranslation.sLang, mTranslation.sText))
                    End If

                    Refresh()
                End Sub
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

                Me.Text = sLangauge
                Me.Name = sLangauge
                Me.Tag = New String() {sFormat, sText}

                Select Case (iType)
                    Case ENUM_NODE_TYPE.NAME
                        g_sLanguage = Nothing
                        g_sFormat = Nothing
                        g_sText = Nothing

                        Me.Text = sName
                        Me.Name = sName
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

            g_mFormTranslationEditor.g_mRecentSecureStorage.Open()

            Using mIni As New ClassIni(g_mFormTranslationEditor.g_mRecentSecureStorage.m_String(System.Text.Encoding.Default))
                For Each mItem In mIni.ReadEverything
                    If (mItem.sSection <> "Recent Translations") Then
                        Continue For
                    End If

                    Dim tmpLng As Long
                    If (Long.TryParse(mItem.sValue, tmpLng)) Then
                        mRecentSorted.Add(New KeyValuePair(Of String, Date)(mItem.sKey, New Date(tmpLng)))
                    End If
                Next
            End Using

            mRecentSorted.RemoveAll(Function(x As KeyValuePair(Of String, Date))
                                        Return sFile.ToLower = x.Key.ToLower
                                    End Function)

            mRecentSorted.Add(New KeyValuePair(Of String, Date)(sFile.ToLower, Now))

            mRecentSorted.Sort(Function(x As KeyValuePair(Of String, Date), y As KeyValuePair(Of String, Date))
                                   Return x.Value.Ticks.CompareTo(y.Value.Ticks)
                               End Function)

            Using mIni As New ClassIni()
                Dim mIniContent As New List(Of ClassIni.STRUC_INI_CONTENT)

                For i = 0 To mRecentSorted.Count - 1
                    If (i > g_iMaxRecentFiles) Then
                        Exit For
                    End If

                    mIniContent.Add(New ClassIni.STRUC_INI_CONTENT("Recent Translations", mRecentSorted(i).Key, CStr(mRecentSorted(i).Value.Ticks)))
                Next

                mIni.WriteKeyValue(mIniContent.ToArray)

                g_mFormTranslationEditor.g_mRecentSecureStorage.m_String(System.Text.Encoding.Default) = mIni.ExportToString()
            End Using

            g_mFormTranslationEditor.g_mRecentSecureStorage.Close()
        End Sub

        Public Function GetRecent() As KeyValuePair(Of String, Date)()
            Dim mRecentSorted As New List(Of KeyValuePair(Of String, Date))

            g_mFormTranslationEditor.g_mRecentSecureStorage.Open()

            Using mIni As New ClassIni(g_mFormTranslationEditor.g_mRecentSecureStorage.m_String(System.Text.Encoding.Default))
                For Each mItem In mIni.ReadEverything
                    If (mItem.sSection <> "Recent Translations") Then
                        Continue For
                    End If

                    Dim tmpLng As Long
                    If (Long.TryParse(mItem.sValue, tmpLng)) Then
                        mRecentSorted.Add(New KeyValuePair(Of String, Date)(mItem.sKey, New Date(tmpLng)))
                    End If
                Next
            End Using

            mRecentSorted.Sort(Function(x As KeyValuePair(Of String, Date), y As KeyValuePair(Of String, Date))
                                   Return x.Value.Ticks.CompareTo(y.Value.Ticks)
                               End Function)

            Return mRecentSorted.ToArray
        End Function
    End Class

End Class