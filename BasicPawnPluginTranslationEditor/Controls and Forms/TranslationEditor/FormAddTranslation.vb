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

Public Class FormAddTranslation
    Private g_iDialogType As ENUM_DIALOG_TYPE
    Private g_sLanguages As KeyValuePair(Of String, String)()
    Private g_mCustomLang As ClassLanguage = Nothing

    Private g_mFormOnlineTranslator As FormOnlineTranslator = Nothing

    Enum ENUM_DIALOG_TYPE
        ADD
        EDIT
    End Enum

    Public Sub New(sName As String, sLanguage As String, sFormat As String, sText As String, iDialogType As ENUM_DIALOG_TYPE, sLanguages As KeyValuePair(Of String, String)())
        g_iDialogType = iDialogType
        g_sLanguages = sLanguages

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.  
        ClassControlStyle.SetNameFlag(Panel_FooterControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER)
        ClassControlStyle.SetNameFlag(Panel_FooterDarkControl, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)

        ComboBox_Language.Items.Clear()
        For Each mItem In sLanguages
            ComboBox_Language.Items.Add(New ClassLanguage(mItem.Key, mItem.Value))
        Next

        m_Name = sName
        m_Langauge = sLanguage
        m_Format = sFormat
        m_Text = ClassKeyValues.EscapeString(sText)

        Select Case (iDialogType)
            Case ENUM_DIALOG_TYPE.ADD
                ComboBox_Language.Enabled = True
                LinkLabel_LangCustom.Enabled = True

                Me.Text = "Add Translation"

            Case Else
                ComboBox_Language.Enabled = False
                LinkLabel_LangCustom.Enabled = False

                Me.Text = "Edit Translation"
        End Select

        If (m_Langauge = "en") Then
            CheckBox_FormatInherit.Enabled = False
            CheckBox_FormatInherit.Checked = False
        Else
            CheckBox_FormatInherit.Enabled = True
            CheckBox_FormatInherit.Checked = String.IsNullOrEmpty(m_Format.Trim)
        End If
    End Sub

    Property m_Name As String
        Get
            Return TextBox_TranslationName.Text
        End Get
        Set(value As String)
            TextBox_TranslationName.Text = value
        End Set
    End Property

    Property m_Langauge As String
        Get
            Dim mLang As ClassLanguage = TryCast(ComboBox_Language.SelectedItem, ClassLanguage)
            If (mLang Is Nothing) Then
                Return Nothing
            End If

            Return mLang.m_Language
        End Get
        Set(value As String)
            If (Not String.IsNullOrEmpty(value)) Then
                Dim iIndex As Integer = -1
                For i = 0 To ComboBox_Language.Items.Count - 1
                    Dim mLang As ClassLanguage = TryCast(ComboBox_Language.Items(i), ClassLanguage)
                    If (mLang Is Nothing) Then
                        Continue For
                    End If

                    If (mLang.m_Language = value) Then
                        iIndex = i
                        Exit For
                    End If
                Next

                If (iIndex = -1) Then
                    iIndex = ComboBox_Language.Items.Add(New ClassLanguage(value, "Custom"))
                End If

                ComboBox_Language.SelectedIndex = iIndex
            Else
                If (ComboBox_Language.Items.Count > 0) Then
                    ComboBox_Language.SelectedIndex = 0
                End If
            End If
        End Set
    End Property

    Property m_Format As String
        Get
            Return TextBox_Format.Text
        End Get
        Set(value As String)
            TextBox_Format.Text = value
        End Set
    End Property

    Property m_Text As String
        Get
            Return TextBox_Text.Text
        End Get
        Set(value As String)
            TextBox_Text.Text = value
        End Set
    End Property

    Private Sub FormAddTranslation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub CheckBox_FormatInherit_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_FormatInherit.CheckedChanged
        TextBox_Format.Enabled = (Not CheckBox_FormatInherit.Checked)
    End Sub

    Class ClassLanguage
        Private g_sLanguage As String
        Private g_sLongLanguage As String

        Public Sub New(sLanguage As String, sLongLanguage As String)
            g_sLanguage = sLanguage
            g_sLongLanguage = sLongLanguage
        End Sub

        ReadOnly Property m_Language As String
            Get
                Return g_sLanguage
            End Get
        End Property

        ReadOnly Property m_LongLanguage As String
            Get
                Return g_sLongLanguage
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("{0} ({1})", g_sLanguage, g_sLongLanguage)
        End Function
    End Class

    Private Sub LinkLabel_LangCustom_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_LangCustom.LinkClicked
        Dim sNewLang As String = InputBox("", "Enter new language (ISO code)")
        If (String.IsNullOrEmpty(sNewLang)) Then
            Return
        End If

        If (g_mCustomLang IsNot Nothing) Then
            ComboBox_Language.Items.Remove(g_mCustomLang)
        End If

        g_mCustomLang = New ClassLanguage(sNewLang, "Custom")

        Dim iIndex As Integer = ComboBox_Language.Items.Add(g_mCustomLang)
        If (iIndex > -1) Then
            ComboBox_Language.SelectedIndex = iIndex
        End If
    End Sub

    Private Sub Button_Apply_Click(sender As Object, e As EventArgs) Handles Button_Apply.Click
        If (CheckBox_FormatInherit.Checked) Then
            m_Format = ""
        End If

        If (Not IsFormatValid()) Then
            MessageBox.Show("Format formating is incorrect! Please follow the formating rules.", "Incorrect formating", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        'Use Unix newline not windows
        m_Text = m_Text.Replace(vbCrLf, vbLf)
        m_Text = ClassKeyValues.UnescapeString(m_Text)

        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub LinkLabel_OnlineTranslator_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_OnlineTranslator.LinkClicked
        If (g_mFormOnlineTranslator IsNot Nothing AndAlso Not g_mFormOnlineTranslator.IsDisposed) Then
            Return
        End If

        g_mFormOnlineTranslator = New FormOnlineTranslator
        g_mFormOnlineTranslator.Show(Me)
    End Sub

    Private Function IsFormatValid() As Boolean
        If (String.IsNullOrEmpty(m_Format)) Then
            Return True
        End If

        If (Regex.IsMatch(m_Format, "^({\d+\:\w}|,{\d+\:\w})+$")) Then
            Return True
        End If

        Return False
    End Function

    Private Sub FormAddTranslation_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub CleanUp()
        If (g_mFormOnlineTranslator IsNot Nothing AndAlso Not g_mFormOnlineTranslator.IsDisposed) Then
            g_mFormOnlineTranslator.Dispose()
            g_mFormOnlineTranslator = Nothing
        End If
    End Sub
End Class