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
Imports System.Text
Imports System.Web.Script.Serialization

Public Class FormTranslator
    Private g_ClassTranslator As ClassTranslator

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ClassControlStyle.SetNameFlag(Panel_Footer, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER)
        ClassControlStyle.SetNameFlag(Panel_FooterDark, ClassControlStyle.ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)

        g_ClassTranslator = New ClassTranslator(Me)

        RemoveHandler g_ClassTranslator.OnTranslationDone, AddressOf OnTranslationDone
        AddHandler g_ClassTranslator.OnTranslationDone, AddressOf OnTranslationDone

        Try
            ComboBox_TranslateFrom.BeginUpdate()

            ComboBox_TranslateFrom.Items.Clear()
            For Each mLang In ClassLanguage.GetSupportedLanguages
                Dim iIndex As Integer = ComboBox_TranslateFrom.Items.Add(mLang)

                If (mLang.m_Lang = "en") Then
                    ComboBox_TranslateFrom.SelectedIndex = iIndex
                End If
            Next
        Finally
            ComboBox_TranslateFrom.EndUpdate()
        End Try

        Try
            ComboBox_TranslateTo.BeginUpdate()

            ComboBox_TranslateTo.Items.Clear()
            For Each mLang In ClassLanguage.GetSupportedLanguages
                Dim iIndex As Integer = ComboBox_TranslateTo.Items.Add(mLang)

                If (mLang.m_Lang = "de") Then
                    ComboBox_TranslateTo.SelectedIndex = iIndex
                End If
            Next
        Finally
            ComboBox_TranslateTo.EndUpdate()
        End Try
    End Sub

    Private Sub FormTranslator_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClassControlStyle.UpdateControls(Me)
    End Sub

    Private Sub Button_Translate_Click(sender As Object, e As EventArgs) Handles Button_Translate.Click
        Dim sText As String = TextBox_TranslateFrom.Text

        Dim mLangFrom As ClassLanguage = TryCast(ComboBox_TranslateFrom.SelectedItem, ClassLanguage)
        If (mLangFrom Is Nothing) Then
            Return
        End If

        Dim mLangTo As ClassLanguage = TryCast(ComboBox_TranslateTo.SelectedItem, ClassLanguage)
        If (mLangTo Is Nothing) Then
            Return
        End If

        If (g_ClassTranslator.IsBusy) Then
            MessageBox.Show("Translator is busy translating", "Please wait...", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        g_ClassTranslator.StartTranslate(sText, mLangFrom.m_Lang, mLangTo.m_Lang)
    End Sub

    Private Sub OnTranslationDone(sTranslatedText As String)
        ClassThread.ExecAsync(Me, Sub()
                                      TextBox_TranslateTo.Text = sTranslatedText
                                  End Sub)
    End Sub

    Class ClassLanguage
        Private g_sLang As String
        Private g_sLanguageDispaly As String

        Public Sub New(sLang As String, sLanguageDisplay As String)
            g_sLang = sLang
            g_sLanguageDispaly = sLanguageDisplay
        End Sub

        ReadOnly Property m_Lang As String
            Get
                Return g_sLang
            End Get
        End Property

        ReadOnly Property m_LangaugeDisplay As String
            Get
                Return g_sLanguageDispaly
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("{0} ({1})", g_sLanguageDispaly, g_sLang)
        End Function

        Public Shared Function GetSupportedLanguages() As ClassLanguage()
            Static mLanguages As List(Of ClassLanguage) = Nothing

            If (mLanguages Is Nothing) Then
                'https://cloud.google.com/translate/docs/languages
                Dim mPairLanguages As New List(Of KeyValuePair(Of String, String)) From {
                    New KeyValuePair(Of String, String)("Afrikaans", "af"),
                    New KeyValuePair(Of String, String)("Albanian", "sq"),
                    New KeyValuePair(Of String, String)("Amharic", "am"),
                    New KeyValuePair(Of String, String)("Arabic", "ar"),
                    New KeyValuePair(Of String, String)("Armenian", "hy"),
                    New KeyValuePair(Of String, String)("Azerbaijani", "az"),
                    New KeyValuePair(Of String, String)("Basque", "eu"),
                    New KeyValuePair(Of String, String)("Belarusian", "be"),
                    New KeyValuePair(Of String, String)("Bengali", "bn"),
                    New KeyValuePair(Of String, String)("Bosnian", "bs"),
                    New KeyValuePair(Of String, String)("Bulgarian", "bg"),
                    New KeyValuePair(Of String, String)("Catalan", "ca"),
                    New KeyValuePair(Of String, String)("Cebuano", "ceb"),
                    New KeyValuePair(Of String, String)("Chinese (Simplified)", "zh-CN"),
                    New KeyValuePair(Of String, String)("Chinese (Traditional)", "zh-TW"),
                    New KeyValuePair(Of String, String)("Corsican", "co"),
                    New KeyValuePair(Of String, String)("Croatian", "hr"),
                    New KeyValuePair(Of String, String)("Czech", "cs"),
                    New KeyValuePair(Of String, String)("Danish", "da"),
                    New KeyValuePair(Of String, String)("Dutch", "nl"),
                    New KeyValuePair(Of String, String)("English", "en"),
                    New KeyValuePair(Of String, String)("Esperanto", "eo"),
                    New KeyValuePair(Of String, String)("Estonian", "et"),
                    New KeyValuePair(Of String, String)("Finnish", "fi"),
                    New KeyValuePair(Of String, String)("French", "fr"),
                    New KeyValuePair(Of String, String)("Frisian", "fy"),
                    New KeyValuePair(Of String, String)("Galician", "gl"),
                    New KeyValuePair(Of String, String)("Georgian", "ka"),
                    New KeyValuePair(Of String, String)("German", "de"),
                    New KeyValuePair(Of String, String)("Greek", "el"),
                    New KeyValuePair(Of String, String)("Gujarati", "gu"),
                    New KeyValuePair(Of String, String)("Haitian Creole", "ht"),
                    New KeyValuePair(Of String, String)("Hausa", "ha"),
                    New KeyValuePair(Of String, String)("Hawaiian", "haw"),
                    New KeyValuePair(Of String, String)("Hebrew", "he"),
                    New KeyValuePair(Of String, String)("Hindi", "hi"),
                    New KeyValuePair(Of String, String)("Hmong", "hmn"),
                    New KeyValuePair(Of String, String)("Hungarian", "hu"),
                    New KeyValuePair(Of String, String)("Icelandic", "is"),
                    New KeyValuePair(Of String, String)("Igbo", "ig"),
                    New KeyValuePair(Of String, String)("Indonesian", "id"),
                    New KeyValuePair(Of String, String)("Irish", "ga"),
                    New KeyValuePair(Of String, String)("Italian", "it"),
                    New KeyValuePair(Of String, String)("Japanese", "ja"),
                    New KeyValuePair(Of String, String)("Javanese", "jv"),
                    New KeyValuePair(Of String, String)("Kannada", "kn"),
                    New KeyValuePair(Of String, String)("Kazakh", "kk"),
                    New KeyValuePair(Of String, String)("Khmer", "km"),
                    New KeyValuePair(Of String, String)("Kinyarwanda", "rw"),
                    New KeyValuePair(Of String, String)("Korean", "ko"),
                    New KeyValuePair(Of String, String)("Kurdish", "ku"),
                    New KeyValuePair(Of String, String)("Kyrgyz", "ky"),
                    New KeyValuePair(Of String, String)("Lao", "lo"),
                    New KeyValuePair(Of String, String)("Latin", "la"),
                    New KeyValuePair(Of String, String)("Latvian", "lv"),
                    New KeyValuePair(Of String, String)("Lithuanian", "lt"),
                    New KeyValuePair(Of String, String)("Luxembourgish", "lb"),
                    New KeyValuePair(Of String, String)("Macedonian", "mk"),
                    New KeyValuePair(Of String, String)("Malagasy", "mg"),
                    New KeyValuePair(Of String, String)("Malay", "ms"),
                    New KeyValuePair(Of String, String)("Malayalam", "ml"),
                    New KeyValuePair(Of String, String)("Maltese", "mt"),
                    New KeyValuePair(Of String, String)("Maori", "mi"),
                    New KeyValuePair(Of String, String)("Marathi", "mr"),
                    New KeyValuePair(Of String, String)("Mongolian", "mn"),
                    New KeyValuePair(Of String, String)("Myanmar (Burmese)", "my"),
                    New KeyValuePair(Of String, String)("Nepali", "ne"),
                    New KeyValuePair(Of String, String)("Norwegian", "no"),
                    New KeyValuePair(Of String, String)("Nyanja (Chichewa)", "ny"),
                    New KeyValuePair(Of String, String)("Odia (Oriya)", "or"),
                    New KeyValuePair(Of String, String)("Pashto", "ps"),
                    New KeyValuePair(Of String, String)("Persian", "fa"),
                    New KeyValuePair(Of String, String)("Polish", "pl"),
                    New KeyValuePair(Of String, String)("Portuguese (Portugal, Brazil)", "pt"),
                    New KeyValuePair(Of String, String)("Punjabi", "pa"),
                    New KeyValuePair(Of String, String)("Romanian", "ro"),
                    New KeyValuePair(Of String, String)("Russian", "ru"),
                    New KeyValuePair(Of String, String)("Samoan", "sm"),
                    New KeyValuePair(Of String, String)("Scots Gaelic", "gd"),
                    New KeyValuePair(Of String, String)("Serbian", "sr"),
                    New KeyValuePair(Of String, String)("Sesotho", "st"),
                    New KeyValuePair(Of String, String)("Shona", "sn"),
                    New KeyValuePair(Of String, String)("Sindhi", "sd"),
                    New KeyValuePair(Of String, String)("Sinhala (Sinhalese)", "si"),
                    New KeyValuePair(Of String, String)("Slovak", "sk"),
                    New KeyValuePair(Of String, String)("Slovenian", "sl"),
                    New KeyValuePair(Of String, String)("Somali", "so"),
                    New KeyValuePair(Of String, String)("Spanish", "es"),
                    New KeyValuePair(Of String, String)("Sundanese", "su"),
                    New KeyValuePair(Of String, String)("Swahili", "sw"),
                    New KeyValuePair(Of String, String)("Swedish", "sv"),
                    New KeyValuePair(Of String, String)("Tagalog (Filipino)", "tl"),
                    New KeyValuePair(Of String, String)("Tajik", "tg"),
                    New KeyValuePair(Of String, String)("Tamil", "ta"),
                    New KeyValuePair(Of String, String)("Tatar", "tt"),
                    New KeyValuePair(Of String, String)("Telugu", "te"),
                    New KeyValuePair(Of String, String)("Thai", "th"),
                    New KeyValuePair(Of String, String)("Turkish", "tr"),
                    New KeyValuePair(Of String, String)("Turkmen", "tk"),
                    New KeyValuePair(Of String, String)("Ukrainian", "uk"),
                    New KeyValuePair(Of String, String)("Urdu", "ur"),
                    New KeyValuePair(Of String, String)("Uyghur", "ug"),
                    New KeyValuePair(Of String, String)("Uzbek", "uz"),
                    New KeyValuePair(Of String, String)("Vietnamese", "vi"),
                    New KeyValuePair(Of String, String)("Welsh", "cy"),
                    New KeyValuePair(Of String, String)("Xhosa", "xh"),
                    New KeyValuePair(Of String, String)("Yiddish", "yi"),
                    New KeyValuePair(Of String, String)("Yoruba", "yo"),
                    New KeyValuePair(Of String, String)("Zulu", "zu")
                }

                mLanguages = New List(Of ClassLanguage)
                For Each mItem In mPairLanguages
                    mLanguages.Add(New ClassLanguage(mItem.Value, mItem.Key))
                Next
            End If

            Return mLanguages.ToArray
        End Function
    End Class

    Class ClassTranslator
        Implements IDisposable

        Private g_mFormTranslator As FormTranslator
        Private g_mFormProgress As FormProgress

        Event OnTranslationDone(sTranslatedText As String)

        Private g_mTranslateThread As Threading.Thread = Nothing

        Public Sub New(mFormTranslator As FormTranslator)
            g_mFormTranslator = mFormTranslator
        End Sub

        Public Sub StartTranslate(sText As String, sLangFrom As String, sLangTo As String)
            If (ClassThread.IsValid(g_mTranslateThread)) Then
                Return
            End If

            g_mTranslateThread = New Threading.Thread(Sub()
                                                          Try
                                                              Try
                                                                  ClassThread.ExecEx(Of Object)(g_mFormTranslator, Sub()
                                                                                                                       If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                                                                                                                           g_mFormProgress.Dispose()
                                                                                                                           g_mFormProgress = Nothing
                                                                                                                       End If

                                                                                                                       g_mFormProgress = New FormProgress
                                                                                                                       g_mFormProgress.Text = "Translating..."

                                                                                                                       g_mFormProgress.Show(g_mFormTranslator)
                                                                                                                   End Sub)

                                                                  Dim sTranslatedText = TranslateText(sText, sLangFrom, sLangTo)

                                                                  RaiseEvent OnTranslationDone(sTranslatedText)
                                                              Finally
                                                                  ClassThread.ExecAsync(g_mFormTranslator, Sub()
                                                                                                               If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                                                                                                                   g_mFormProgress.Dispose()
                                                                                                                   g_mFormProgress = Nothing
                                                                                                               End If
                                                                                                           End Sub)
                                                              End Try
                                                          Catch ex As Threading.ThreadAbortException
                                                              Throw
                                                          Catch ex As Exception
                                                              ClassExceptionLog.WriteToLogMessageBox(ex)
                                                          End Try
                                                      End Sub) With {
                .IsBackground = True
            }
            g_mTranslateThread.Start()
        End Sub

        Public Function IsBusy() As Boolean
            Return ClassThread.IsValid(g_mTranslateThread)
        End Function

        Public Sub EndTranslate()
            ClassThread.Abort(g_mTranslateThread)
        End Sub

        Private Function TranslateText(sText As String, sLangFrom As String, sLangTo As String) As String
            Dim sURL As String = "https://translate.googleapis.com/translate_a/single"

            Using mClient As New ClassWebClientEx()
                mClient.Encoding = Encoding.UTF8

                mClient.QueryString = New Specialized.NameValueCollection From {
                    {"client", "gtx"},
                    {"sl", Uri.EscapeDataString(sLangFrom)},
                    {"tl", Uri.EscapeDataString(sLangTo)},
                    {"dt", "t"},
                    {"q", Uri.EscapeDataString(sText)}
                }

                Dim sOutput As String = mClient.DownloadString(New Uri(sURL))
                If (String.IsNullOrEmpty(sOutput)) Then
                    Throw New ArgumentException("Translation limit reached")
                End If

                Dim mJson = (New JavaScriptSerializer).Deserialize(Of Object())(sOutput)
                If (mJson Is Nothing OrElse mJson.Count < 1) Then
                    Throw New ArgumentException("No results found")
                End If

                Dim mJson1 As Object() = TryCast(mJson(0), Object())
                If (mJson1 Is Nothing OrElse mJson1.Length < 1) Then
                    Throw New ArgumentException("Translation not found")
                End If

                Dim mJson2 As Object() = TryCast(mJson1(0), Object())
                If (mJson2 Is Nothing OrElse mJson2.Length < 1) Then
                    Throw New ArgumentException("Translation not found")
                End If

                Return CStr(mJson2(0))
            End Using
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    ClassThread.Abort(g_mTranslateThread)

                    If (g_mFormProgress IsNot Nothing AndAlso Not g_mFormProgress.IsDisposed) Then
                        g_mFormProgress.Dispose()
                        g_mFormProgress = Nothing
                    End If
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class

    Private Sub FormTranslator_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        CleanUp()
    End Sub

    Private Sub CleanUp()
        If (g_ClassTranslator IsNot Nothing) Then
            RemoveHandler g_ClassTranslator.OnTranslationDone, AddressOf OnTranslationDone

            g_ClassTranslator.Dispose()
            g_ClassTranslator = Nothing
        End If
    End Sub
End Class