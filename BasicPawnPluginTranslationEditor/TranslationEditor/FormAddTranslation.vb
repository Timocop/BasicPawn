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


Imports BasicPawn

Public Class FormAddTranslation
    Private g_iDialogType As ENUM_DIALOG_TYPE
    Private g_sLanguages As KeyValuePair(Of String, String)()

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
        ComboBox_Language.Items.Clear()
        For Each mItem In sLanguages
            ComboBox_Language.Items.Add(New ClassLanguage(mItem.Key, mItem.Value))
        Next

        If (Not String.IsNullOrEmpty(sLanguage)) Then
            Dim iIndex As Integer = -1
            For i = 0 To ComboBox_Language.Items.Count - 1
                Dim mLang As ClassLanguage = TryCast(ComboBox_Language.Items(i), ClassLanguage)
                If (mLang Is Nothing) Then
                    Continue For
                End If

                If (mLang.m_Language = sLanguage) Then
                    iIndex = i
                    Exit For
                End If
            Next

            If (iIndex = -1) Then
                iIndex = ComboBox_Language.Items.Add(New ClassLanguage(sLanguage, "Custom"))
            End If

            ComboBox_Language.SelectedIndex = iIndex
        Else
            If (ComboBox_Language.Items.Count > 0) Then
                ComboBox_Language.SelectedIndex = 0
            End If
        End If

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

        TextBox_TranslationName.Text = sName
        TextBox_Format.Text = sFormat
        TextBox_Text.Text = sText
    End Sub

    ReadOnly Property m_Name As String
        Get
            Return TextBox_TranslationName.Text
        End Get
    End Property

    ReadOnly Property m_Langauge As String
        Get
            Return ComboBox_Language.SelectedText
        End Get
    End Property

    ReadOnly Property m_Format As String
        Get
            Return TextBox_Format.Text
        End Get
    End Property

    ReadOnly Property m_Text As String
        Get
            Return TextBox_Text.Text
        End Get
    End Property

    Private Sub FormAddTranslation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)
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
End Class