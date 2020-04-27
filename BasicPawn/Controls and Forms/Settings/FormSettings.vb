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


Public Class FormSettings
    Private g_mFormMain As FormMain
    Private g_iConfigType As ENUM_CONFIG_TYPE = ENUM_CONFIG_TYPE.ACTIVE

    Private g_lRestoreConfigs As New List(Of ClassConfigs.STRUC_CONFIG_ITEM)
    Private g_bRestoreConfigs As Boolean = False
    Private g_bIgnoreChange As Boolean = False
    Private g_bConfigSettingsChanged As Boolean = False
    Private g_bComboBoxIgnoreEvent As Boolean = False

    Private g_mListBoxConfigSelectedItem As Object = Nothing

    Enum ENUM_CONFIG_TYPE
        ALL
        ACTIVE
    End Enum

    Enum ENUM_PLUGIN_IMAGE_STATE
        ENABLED
        DISABLED
    End Enum

    Public Sub New(f As FormMain, iConfigType As ENUM_CONFIG_TYPE)
        g_mFormMain = f
        g_iConfigType = iConfigType
        g_bIgnoreChange = True

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        Me.Size = Me.MinimumSize

        NumericUpDown_MaxParseThreads.Minimum = 1
        NumericUpDown_MaxParseThreads.Maximum = Environment.ProcessorCount

        Init_Configs()
        Init_Plugins()

        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)

        g_bIgnoreChange = False
        m_ConfigSettingsChanged = False
    End Sub

    Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'List all configs
        UpdateConfigListBox()

        'Update log button text
        UpdateErrorLogSize()

        'Get all settings
        ClassSettings.LoadSettings()
        ClassConfigs.ClassKnownConfigs.LoadKnownConfigs()

        Load_General()
        Load_TextEditor()
        Load_SyntaxHighlighting()
        Load_AutocompleteIntelliSense()

        Load_Configs()

        'List plugins
        UpdatePluginsListView()

        'Fill DatabaseViewer
        DatabaseListBox_Database.FillFromDatabase()

        ClassControlStyle.UpdateControls(Me)

        'Load last window info
        ClassSettings.LoadWindowInfo(Me)
    End Sub

    Private Sub Button_Apply_Click(sender As Object, e As EventArgs) Handles Button_Apply.Click
        Apply_Configs()

        'General
        Apply_General()
        Apply_TextEditor()
        Apply_SyntaxHighlighting()
        Apply_AutocompleteIntelliSense()

        ClassSettings.SaveSettings()
        ClassConfigs.ClassKnownConfigs.SaveKnownConfigs()

        g_mFormMain.g_ClassPluginController.PluginsExecute(Sub(j As ClassPluginController.STRUC_PLUGIN_ITEM) j.mPluginInterface.OnSettingsChanged())

        Me.Close()
    End Sub

    Private Sub Button_Cancel_Click(sender As Object, e As EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Private Sub FormSettings_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        FormClosing_Configs()
        FormClosing_SyntaxHighlighting()

        'Save window info
        ClassSettings.SaveWindowInfo(Me)
    End Sub

    Public Sub ApplySettings()
        g_mFormMain.UpdateFormConfigText()
        g_mFormMain.g_ClassSyntaxTools.UpdateFormColors()

        g_mFormMain.g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL, "", ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS.FORCE_UPDATE)
        For j = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
            g_mFormMain.g_ClassSyntaxParser.StartUpdateSchedule(ClassSyntaxParser.ENUM_PARSE_TYPE_FLAGS.ALL, g_mFormMain.g_ClassTabControl.m_Tab(j), ClassSyntaxParser.ENUM_PARSE_OPTIONS_FLAGS.FORCE_UPDATE)
        Next

        For j = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
            g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
            g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.TextEditorProperties.IndentationSize = If(ClassSettings.g_iSettingsTabsToSpaces > 0, ClassSettings.g_iSettingsTabsToSpaces, 4)
            g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.TextEditorProperties.ConvertTabsToSpaces = (ClassSettings.g_iSettingsTabsToSpaces > 0)
            g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.m_CustomIconBarVisible = ClassSettings.g_bSettingsIconBar

            g_mFormMain.g_ClassTabControl.m_Tab(j).m_TextEditor.InvalidateTextArea()
        Next

        For j = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
            g_mFormMain.g_ClassTabControl.m_Tab(j).g_ClassLineState.UpdateStates()
            g_mFormMain.g_ClassTabControl.m_Tab(j).g_ClassLineState.LimitStates()
        Next

        For j = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
            g_mFormMain.g_ClassTabControl.m_Tab(j).g_ClassScopeHighlighting.RemoveHighlighting()
            g_mFormMain.g_ClassTabControl.m_Tab(j).g_ClassScopeHighlighting.UpdateHighlighting()
        Next

        For j = 0 To g_mFormMain.g_ClassTabControl.m_TabsCount - 1
            g_mFormMain.g_ClassTabControl.m_Tab(j).g_ClassMarkerHighlighting.RemoveHighlighting(ClassTabControl.ClassTab.ClassMarkerHighlighting.ENUM_MARKER_TYPE.STATIC_MARKER)
            g_mFormMain.g_ClassTabControl.m_Tab(j).g_ClassMarkerHighlighting.RemoveHighlighting(ClassTabControl.ClassTab.ClassMarkerHighlighting.ENUM_MARKER_TYPE.CARET_MARKER)
        Next

    End Sub
End Class