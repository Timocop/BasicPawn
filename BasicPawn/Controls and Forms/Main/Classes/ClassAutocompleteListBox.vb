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


Public Class ClassAutocompleteListBox
    Inherits ListBox

    Enum ENUM_ICONS
        ICO_CLASS
        ICO_ENUMERATOR
        ICO_ENUMITEM
        ICO_EVENT
        ICO_FIELD
        ICO_INTERFACE
        ICO_KEYWORD
        ICO_METHOD
        ICO_MISC
        ICO_NAMESPACE
        ICO_PROPERTY
        ICO_STRING
        ICO_VARIABLE
    End Enum

    Private g_mIcons([Enum].GetNames(GetType(ENUM_ICONS)).Length - 1) As Image

    Public Sub New()
        MyBase.New()

        g_mIcons(ENUM_ICONS.ICO_CLASS) = My.Resources.IntelliSenseClass_32x
        g_mIcons(ENUM_ICONS.ICO_ENUMERATOR) = My.Resources.IntelliSenseEnumerator_32x
        g_mIcons(ENUM_ICONS.ICO_ENUMITEM) = My.Resources.IntelliSenseEnumItem_32x
        g_mIcons(ENUM_ICONS.ICO_EVENT) = My.Resources.IntelliSenseEvent_32x
        g_mIcons(ENUM_ICONS.ICO_FIELD) = My.Resources.IntelliSenseField_32x
        g_mIcons(ENUM_ICONS.ICO_INTERFACE) = My.Resources.IntelliSenseInterface_32x
        g_mIcons(ENUM_ICONS.ICO_KEYWORD) = My.Resources.IntelliSenseKeyword_32x
        g_mIcons(ENUM_ICONS.ICO_METHOD) = My.Resources.IntelliSenseMethod_32x
        g_mIcons(ENUM_ICONS.ICO_MISC) = My.Resources.IntelliSenseMisc_32x
        g_mIcons(ENUM_ICONS.ICO_NAMESPACE) = My.Resources.IntelliSenseNamespace_32x
        g_mIcons(ENUM_ICONS.ICO_PROPERTY) = My.Resources.IntelliSenseProperty_32x
        g_mIcons(ENUM_ICONS.ICO_STRING) = My.Resources.IntelliSenseString_32x
        g_mIcons(ENUM_ICONS.ICO_VARIABLE) = My.Resources.IntelliSenseVariable_32x

        Me.ItemHeight = 16
        Me.DrawMode = DrawMode.OwnerDrawVariable

        Me.SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        If (e.Index < 0) Then
            Return
        End If

        If (Me.Items.Count < 1 OrElse e.Index > Me.Items.Count - 1) Then
            Return
        End If

        Dim mItem = TryCast(Me.Items(e.Index), ClassAutocompleteItem)
        If (mItem Is Nothing) Then
            Return
        End If

        Const TEXT_FILE_OFFSET = 24
        Const TEXT_FUNCTION_OFFSET = 164
        Const ICON_SIZE = 16

        'e.DrawBackground()

        If (Me.SelectedIndex = e.Index) Then
            If (ClassControlStyle.m_IsInvertedColors) Then
                'Darker Color.RoyalBlue. Orginal Color.RoyalBlue: Color.FromArgb(65, 105, 150) 
                e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(36, 59, 127)), e.Bounds)
            Else
                e.Graphics.FillRectangle(New SolidBrush(Color.LightBlue), e.Bounds)
            End If
        Else
            e.Graphics.FillRectangle(New SolidBrush(Me.BackColor), e.Bounds)
        End If

        TextRenderer.DrawText(e.Graphics, mItem.m_File, Me.Font,
                              New Rectangle(e.Bounds.X + TEXT_FILE_OFFSET, e.Bounds.Y, e.Bounds.X - TEXT_FILE_OFFSET + TEXT_FUNCTION_OFFSET, e.Bounds.Height),
                              Me.ForeColor,
                              TextFormatFlags.EndEllipsis Or TextFormatFlags.Left Or TextFormatFlags.VerticalCenter)

        TextRenderer.DrawText(e.Graphics, mItem.m_Function, Me.Font,
                              New Rectangle(e.Bounds.X + TEXT_FUNCTION_OFFSET, e.Bounds.Y, e.Bounds.Width - TEXT_FUNCTION_OFFSET, e.Bounds.Height),
                              Me.ForeColor,
                              TextFormatFlags.EndEllipsis Or TextFormatFlags.Left Or TextFormatFlags.VerticalCenter)

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

        'Draw icon
        e.Graphics.DrawImage(g_mIcons(mItem.m_Icon), e.Bounds.X, e.Bounds.Y, ICON_SIZE, ICON_SIZE)

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.Default
        e.Graphics.CompositingQuality = Drawing2D.CompositingQuality.Default
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.Default

        e.DrawFocusRectangle()

        MyBase.OnDrawItem(e)
    End Sub

    Class ClassSortedAutocomplete
        Private g_ClassAutocompleteListBox As ClassAutocompleteListBox

        Private g_lSortedList As New SortedList(Of Integer, ClassSyntaxTools.STRUC_AUTOCOMPLETE)(New ClassDupKeyComparer())

        Public Sub New(_ClassAutocompleteListBox As ClassAutocompleteListBox)
            g_ClassAutocompleteListBox = _ClassAutocompleteListBox
        End Sub

        Public Sub Add(sTarget As String, mAutocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE)
#If Not USE_SIMPLE_COMPUTE Then
            g_lSortedList.Add(ComputeLevenshtein(mAutocomplete.m_FunctionString, sTarget), mAutocomplete)
#Else
            g_lSortedList.Add(ComputeSimple(mItem.m_FunctionString, sTarget), mItem)
#End If
        End Sub

        ''' <summary>
        ''' Computes Levenshtein distance
        ''' https://www.dotnetperls.com/levenshtein
        ''' </summary>
        ''' <param name="s"></param>
        ''' <param name="t"></param>
        ''' <returns></returns>
        Private Function ComputeLevenshtein(ByVal s As String, ByVal t As String) As Integer
            Dim n As Integer = s.Length
            Dim m As Integer = t.Length
            Dim d As Integer(,) = New Integer(n + 1 - 1, m + 1 - 1) {}

            Dim i As Integer = 0
            Dim j As Integer = 0

            If n = 0 Then
                Return m
            End If

            If m = 0 Then
                Return n
            End If

            While i <= n
                d(i, 0) = Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
            End While

            While j <= m
                d(0, j) = Math.Min(System.Threading.Interlocked.Increment(j), j - 1)
            End While

            For i = 1 To n
                For j = 1 To m
                    d(i, j) = Math.Min(Math.Min(d(i - 1, j) + 1, d(i, j - 1) + 1), d(i - 1, j - 1) + If((t(j - 1) = s(i - 1)), 0, 1))
                Next
            Next

            Return d(n, m)
        End Function

        Private Function ComputeSimple(x As String, y As String) As Integer
            Dim i = x.IndexOf(y, If(ClassSettings.g_iSettingsAutocompleteCaseSensitive, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase))
            If (i = -1) Then
                Return Integer.MaxValue
            End If

            Return i
        End Function

        Public Sub PushToListBox()
            g_ClassAutocompleteListBox.BeginUpdate()
            g_ClassAutocompleteListBox.Items.Clear()

            For Each mItem In g_lSortedList
                g_ClassAutocompleteListBox.Items.Add(New ClassAutocompleteItem(mItem.Value))
            Next

            g_ClassAutocompleteListBox.EndUpdate()
        End Sub

        Class ClassDupKeyComparer
            Implements IComparer(Of Integer)

            Public Function Compare(x As Integer, y As Integer) As Integer Implements IComparer(Of Integer).Compare
                Dim i = x.CompareTo(y)

                If (i = 0) Then
                    Return -1
                End If

                Return i
            End Function
        End Class
    End Class

    Class ClassAutocompleteItem
        Property m_Icon As ENUM_ICONS
        Property m_File As String
        Property m_Function As String
        Property m_Autocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE

        Public Sub New(_Autocomplete As ClassSyntaxTools.STRUC_AUTOCOMPLETE)
            m_File = _Autocomplete.m_Filename
            m_Function = _Autocomplete.m_FunctionString
            m_Autocomplete = _Autocomplete

            While True
                If (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PROPERTY) <> 0 Then
                    m_Icon = ENUM_ICONS.ICO_PROPERTY
                    Exit While
                End If

                If (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHODMAP) <> 0 Then
                    If (Not _Autocomplete.m_FunctionString.Contains("."c)) Then
                        m_Icon = ENUM_ICONS.ICO_CLASS
                        Exit While
                    End If
                End If

                If (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.ENUM) <> 0 Then
                    If (_Autocomplete.m_FunctionString.Contains("."c)) Then
                        m_Icon = ENUM_ICONS.ICO_ENUMITEM
                    Else
                        m_Icon = ENUM_ICONS.ICO_ENUMERATOR
                    End If
                    Exit While
                End If

                If (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FORWARD) <> 0 OrElse
                        (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCENUM) <> 0 OrElse
                        (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.FUNCTAG) <> 0 OrElse
                        (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPEDEF) <> 0 OrElse
                        (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.TYPESET) <> 0 Then
                    m_Icon = ENUM_ICONS.ICO_EVENT
                    Exit While
                End If

                If (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.VARIABLE) <> 0 Then
                    If (Not _Autocomplete.m_FunctionString.Contains("."c)) Then
                        m_Icon = ENUM_ICONS.ICO_VARIABLE
                        Exit While
                    End If
                End If

                If (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLICVAR) <> 0 OrElse
                        (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STRUCT) <> 0 Then
                    m_Icon = ENUM_ICONS.ICO_INTERFACE
                    Exit While
                End If

                If (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PREPROCESSOR) <> 0 OrElse
                        (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEFINE) <> 0 OrElse
                        (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.DEBUG) <> 0 Then
                    m_Icon = ENUM_ICONS.ICO_KEYWORD
                    Exit While
                End If

                If (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.METHOD) <> 0 OrElse
                        (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.PUBLIC) <> 0 OrElse
                        (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.NATIVE) <> 0 OrElse
                        (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STOCK) <> 0 OrElse
                        (_Autocomplete.m_Type And ClassSyntaxTools.STRUC_AUTOCOMPLETE.ENUM_TYPE_FLAGS.STATIC) <> 0 Then
                    m_Icon = ENUM_ICONS.ICO_METHOD
                    Exit While
                End If


                m_Icon = ENUM_ICONS.ICO_STRING

                Exit While
            End While
        End Sub
    End Class
End Class
