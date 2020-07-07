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



Public Class FormExportWizard
    Event OnNavigationPage(iDirection As Integer, iPage As ENUM_PAGES, ByRef bHandeled As Boolean)

    Enum ENUM_PAGES
        WELCOME
        FILE
        PACK_SETTINGS
        FINALIZE
    End Enum

    Private g_iWizardPages([Enum].GetNames(GetType(ENUM_PAGES)).Length - 1) As UserControl
    Private g_iWizardPage As ENUM_PAGES = ENUM_PAGES.WELCOME

    Private g_sExportFile As String = ""
    Private g_bExportIsPacked As Boolean = False
    Private g_mExportKeyValue As FormTranslationEditor.ClassTranslationManager.ClassTranslation()

    Public Sub New(sFile As String, bIsPacked As Boolean, mKeyValue As FormTranslationEditor.ClassTranslationManager.ClassTranslation())
        g_sExportFile = sFile
        g_bExportIsPacked = bIsPacked
        g_mExportKeyValue = mKeyValue

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Panel_TopDarkControl.Name &= "@FooterDarkControl"
        Panel_FooterControl.Name &= "@FooterControl"
        Panel_FooterDarkControl.Name &= "@FooterDarkControl"

        g_iWizardPages(ENUM_PAGES.WELCOME) = New UCExportWizardWelcome(Me) With {
            .Parent = Panel_Pages,
            .Dock = DockStyle.Fill,
            .Visible = False
        }
        g_iWizardPages(ENUM_PAGES.FILE) = New UCExportWizardFile(Me) With {
            .Parent = Panel_Pages,
            .Dock = DockStyle.Fill,
            .Visible = False
        }
        g_iWizardPages(ENUM_PAGES.PACK_SETTINGS) = New UCExportWizardMethod(Me) With {
            .Parent = Panel_Pages,
            .Dock = DockStyle.Fill,
            .Visible = False
        }
        g_iWizardPages(ENUM_PAGES.FINALIZE) = New UCExportWizardFinalize(Me) With {
            .Parent = Panel_Pages,
            .Dock = DockStyle.Fill,
            .Visible = False
        }

        For i = 0 To g_iWizardPages.Length - 1
            If (g_iWizardPages Is Nothing) Then
                Throw New ArgumentException("Page can not be NULL")
            End If
        Next

        m_WizardPage = ENUM_PAGES.WELCOME
    End Sub

    Property m_ExportFile As String
        Get
            Return g_sExportFile
        End Get
        Set(value As String)
            g_sExportFile = value
        End Set
    End Property

    Property m_ExportIsPacked As Boolean
        Get
            Return g_bExportIsPacked
        End Get
        Set(value As Boolean)
            g_bExportIsPacked = value
        End Set
    End Property

    ReadOnly Property m_ExportKeyValue As FormTranslationEditor.ClassTranslationManager.ClassTranslation()
        Get
            Return g_mExportKeyValue
        End Get
    End Property

    ReadOnly Property m_WizardPages(i As ENUM_PAGES) As UserControl
        Get
            Return g_iWizardPages(i)
        End Get
    End Property

    Private Property m_WizardPage As ENUM_PAGES
        Get
            Return g_iWizardPage
        End Get
        Set(value As ENUM_PAGES)
            g_iWizardPage = value

            Try
                Me.SuspendLayout()

                For i = 0 To g_iWizardPages.Length - 1
                    If (i <> g_iWizardPage) Then
                        g_iWizardPages(i).Visible = False
                    End If
                Next

                g_iWizardPages(g_iWizardPage).Visible = True


                UpdateButtons()
            Finally
                Me.ResumeLayout()
            End Try
        End Set
    End Property

    Private Sub FormExportWizard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub Button_WizNext_Click(sender As Object, e As EventArgs) Handles Button_WizNext.Click
        If (g_iWizardPage = g_iWizardPages.Length - 1) Then
            Me.DialogResult = DialogResult.OK
        Else
            PageNext()
        End If
    End Sub

    Private Sub Button_WizBack_Click(sender As Object, e As EventArgs) Handles Button_WizBack.Click
        PageBack()
    End Sub

    Public Sub SetPageTitle(sTitle As String, sDescription As String)
        Label_WizTitle.Text = sTitle
        Label_WizDesc.Text = sDescription
    End Sub

    Public Sub PageNext()
        Dim bHandeled As Boolean = False

        RaiseEvent OnNavigationPage(1, g_iWizardPage, bHandeled)

        If (bHandeled) Then
            Return
        End If

        Dim iNewPage As Integer = (m_WizardPage + 1)
        If (iNewPage > g_iWizardPages.Length - 1) Then
            Return
        End If

        m_WizardPage = CType(iNewPage, ENUM_PAGES)
    End Sub

    Public Sub PageBack()
        Dim bHandeled As Boolean = False

        RaiseEvent OnNavigationPage(-1, g_iWizardPage, bHandeled)

        If (bHandeled) Then
            Return
        End If

        Dim iNewPage As Integer = (m_WizardPage - 1)
        If (iNewPage < 0) Then
            Return
        End If

        m_WizardPage = CType(iNewPage, ENUM_PAGES)
    End Sub

    Public Sub UpdateButtons()
        Button_WizNext.Text = If(g_iWizardPage = g_iWizardPages.Length - 1, "Finish", "Next >")

        Button_WizBack.Enabled = (g_iWizardPage > 0)
    End Sub

    ''' <summary>
    ''' Get additional translation file if unpacked.
    ''' </summary>
    ''' <returns></returns>
    Public Function GetAdditionalTranslationFiles() As String()
        If (String.IsNullOrEmpty(m_ExportFile)) Then
            Return New String() {}
        End If

        Dim sLangauges As New HashSet(Of String)
        Dim sFiles As New List(Of String)

        For Each mTranslation In m_ExportKeyValue
            For Each mItem In mTranslation.m_TranslationItems
                If (mItem.m_Language = "en") Then
                    Continue For
                End If

                sLangauges.Add(mItem.m_Language)
            Next
        Next

        Dim sFileDirectory As String = IO.Path.GetDirectoryName(m_ExportFile)
        Dim sFileName As String = IO.Path.GetFileName(m_ExportFile)

        For Each sLang As String In sLangauges
            Dim sSubDirectory As String = IO.Path.Combine(sFileDirectory, sLang)

            sFiles.Add(IO.Path.Combine(sSubDirectory, sFileName))
        Next

        Return sFiles.ToArray
    End Function
End Class