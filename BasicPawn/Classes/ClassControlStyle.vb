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


Public Class ClassControlStyle
    Enum ENUM_STYLE_FLAGS
        CONTROL_FOOTER
        CONTROL_FOOTER_DARK
        CONTROL_KEEP_BACKCOLOR
        CONTROL_KEEP_COLOR
        LABEL_BLACK
        LABEL_ROYALBLUE
        MENU_SYSTEMRENDER
    End Enum

    Private Shared ReadOnly g_mStyleFlagNames([Enum].GetNames(GetType(ENUM_STYLE_FLAGS)).Length - 1) As String

    Shared Sub New()
        g_mStyleFlagNames(ENUM_STYLE_FLAGS.CONTROL_FOOTER) = "{$ControlFooter}"
        g_mStyleFlagNames(ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK) = "{$ControlFooterDark}"
        g_mStyleFlagNames(ENUM_STYLE_FLAGS.CONTROL_KEEP_BACKCOLOR) = "{$ControlKeepBackColor}"
        g_mStyleFlagNames(ENUM_STYLE_FLAGS.CONTROL_KEEP_COLOR) = "{$ControlKeepColor}"
        g_mStyleFlagNames(ENUM_STYLE_FLAGS.LABEL_BLACK) = "{$LabelBlack}"
        g_mStyleFlagNames(ENUM_STYLE_FLAGS.LABEL_ROYALBLUE) = "{$LabelRoyalBlue}"
        g_mStyleFlagNames(ENUM_STYLE_FLAGS.MENU_SYSTEMRENDER) = "{$MenuSystemRender}"

        For i = 0 To g_mStyleFlagNames.Length - 1
            If (String.IsNullOrEmpty(g_mStyleFlagNames(i))) Then
                Throw New ArgumentException("StyleFlagNames item is NULL")
            End If
        Next
    End Sub

    Public Shared ReadOnly Property m_IsInvertedColors As Boolean
        Get
            Return ClassSettings.g_bSettingsInvertColors
        End Get
    End Property

    Public Class STRUC_CONTROL_COLORS
        Public mLightForegound As Color
        Public mLightBackground As Color

        Public mDarkForeground As Color
        Public mDarkBackground As Color

        Public Sub New(_LightForegound As Color, _LightBackground As Color, _DarkForeground As Color, _DarkBackground As Color)
            mLightForegound = _LightForegound
            mLightBackground = _LightBackground
            mDarkForeground = _DarkForeground
            mDarkBackground = _DarkBackground
        End Sub
    End Class

    Public Shared ReadOnly g_cDarkTextEditorColor As New STRUC_CONTROL_COLORS(Color.Black, Color.White, Color.LightGray, Color.FromArgb(255, 32, 32, 32))
    Public Shared ReadOnly g_cDarkControlColor As New STRUC_CONTROL_COLORS(Color.Black, Color.White, Color.LightGray, Color.FromArgb(255, 26, 26, 26))
    Public Shared ReadOnly g_cDarkPanelColor As New STRUC_CONTROL_COLORS(Color.Black, Color.White, Color.LightGray, Color.FromArgb(255, 34, 34, 34))
    Public Shared ReadOnly g_cDarkFormColor As New STRUC_CONTROL_COLORS(Color.Black, Color.White, Color.LightGray, Color.FromArgb(255, 38, 38, 38))
    Public Shared ReadOnly g_cDarkMenuColor As New STRUC_CONTROL_COLORS(Color.Black, Color.White, Color.LightGray, Color.FromArgb(255, 64, 64, 64))

    Public Shared Sub SetNameFlag(c As Control, iFlag As ENUM_STYLE_FLAGS)
        RemoveNameFlag(c, iFlag)

        c.Name &= g_mStyleFlagNames(iFlag)
    End Sub

    Public Shared Sub SetNameFlag(c As ToolStripItem, iFlag As ENUM_STYLE_FLAGS)
        RemoveNameFlag(c, iFlag)

        c.Name &= g_mStyleFlagNames(iFlag)
    End Sub

    Public Shared Sub RemoveNameFlag(c As Control, iFlag As ENUM_STYLE_FLAGS)
        c.Name = c.Name.Replace(g_mStyleFlagNames(iFlag), "")
    End Sub

    Public Shared Sub RemoveNameFlag(c As ToolStripItem, iFlag As ENUM_STYLE_FLAGS)
        c.Name = c.Name.Replace(g_mStyleFlagNames(iFlag), "")
    End Sub

    Public Shared Function HasNameFlag(c As Control, iFlag As ENUM_STYLE_FLAGS) As Boolean
        Return c.Name.Contains(g_mStyleFlagNames(iFlag))
    End Function

    Public Shared Function HasNameFlag(c As ToolStripItem, iFlag As ENUM_STYLE_FLAGS) As Boolean
        Return c.Name.Contains(g_mStyleFlagNames(iFlag))
    End Function

    ''' <summary>
    ''' Updates the controls colors
    ''' </summary>
    ''' <param name="c"></param>
    Public Shared Sub UpdateControls(c As Control)
        c.SuspendLayout()

        SetColor(c)

        For Each i As Control In c.Controls
            UpdateControls(i)
        Next

        c.ResumeLayout()
    End Sub

    Public Shared Sub UpdateControls(c As Object)
        SetColor(c)
    End Sub

    ''' <summary>
    ''' Invert the color
    ''' </summary>
    ''' <param name="cColor"></param>
    ''' <returns></returns>
    Public Shared Function InvertColor(cColor As Color) As Color
        Dim cNewColor As Color = Color.FromArgb(cColor.ToArgb Xor -1) '&HFFFFFF 
        Return Color.FromArgb(cColor.A, cNewColor.R, cNewColor.G, cNewColor.B)
    End Function

    Private Shared Sub SetColor(o As Object)
        Select Case True
            Case TypeOf o Is Form
                Dim i As Form = DirectCast(o, Form)
                If (m_IsInvertedColors) Then
                    i.BackColor = g_cDarkFormColor.mDarkBackground
                    i.ForeColor = g_cDarkFormColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkFormColor.mLightBackground
                    i.ForeColor = g_cDarkFormColor.mLightForegound
                End If

            Case TypeOf o Is TabControl
                Dim i As TabControl = DirectCast(o, TabControl)
                If (m_IsInvertedColors) Then
                    i.BackColor = g_cDarkPanelColor.mDarkBackground
                    i.ForeColor = g_cDarkPanelColor.mDarkForeground

                    If (TypeOf i Is ClassTabControlColor) Then
                        i.DrawMode = TabDrawMode.OwnerDrawFixed
                    End If
                Else
                    i.BackColor = g_cDarkPanelColor.mLightBackground
                    i.ForeColor = g_cDarkPanelColor.mLightForegound

                    If (TypeOf i Is ClassTabControlColor) Then
                        i.DrawMode = TabDrawMode.Normal
                    End If
                End If

            Case TypeOf o Is TabPage
                Dim i As TabPage = DirectCast(o, TabPage)
                If (m_IsInvertedColors) Then
                    i.BackColor = g_cDarkPanelColor.mDarkBackground
                    i.ForeColor = g_cDarkPanelColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkPanelColor.mLightBackground
                    i.ForeColor = g_cDarkPanelColor.mLightForegound
                End If

            Case TypeOf o Is PictureBox
                Dim i As PictureBox = DirectCast(o, PictureBox)
                If (m_IsInvertedColors) Then
                    i.BackColor = Color.Transparent
                    i.ForeColor = g_cDarkPanelColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkPanelColor.mLightBackground
                    i.ForeColor = g_cDarkPanelColor.mLightForegound
                End If

            Case TypeOf o Is Panel
                Dim i As Panel = DirectCast(o, Panel)
                Select Case (True)
                    Case HasNameFlag(i, ENUM_STYLE_FLAGS.CONTROL_FOOTER)
                        If (m_IsInvertedColors) Then
                            i.BackColor = g_cDarkPanelColor.mDarkBackground
                            i.ForeColor = g_cDarkPanelColor.mDarkForeground
                        Else
                            i.BackColor = Color.FromKnownColor(KnownColor.Control)
                            i.ForeColor = g_cDarkPanelColor.mLightForegound
                        End If

                    Case HasNameFlag(i, ENUM_STYLE_FLAGS.CONTROL_FOOTER_DARK)
                        If (m_IsInvertedColors) Then
                            i.BackColor = Color.Gray
                            i.ForeColor = g_cDarkPanelColor.mDarkForeground
                        Else
                            i.BackColor = Color.FromKnownColor(KnownColor.ControlDark)
                            i.ForeColor = g_cDarkPanelColor.mLightForegound
                        End If

                    Case HasNameFlag(i, ENUM_STYLE_FLAGS.CONTROL_KEEP_COLOR)
                        'Ignore

                    Case Else
                        If (m_IsInvertedColors) Then
                            i.BackColor = g_cDarkPanelColor.mDarkBackground
                            i.ForeColor = g_cDarkPanelColor.mDarkForeground
                        Else
                            i.BackColor = g_cDarkPanelColor.mLightBackground
                            i.ForeColor = g_cDarkPanelColor.mLightForegound
                        End If
                End Select

            Case TypeOf o Is Button
                Dim i As Button = DirectCast(o, Button)
                If (m_IsInvertedColors) Then
                    i.UseVisualStyleBackColor = False
                    i.FlatStyle = FlatStyle.Flat
                    i.FlatAppearance.BorderSize = 1
                    i.FlatAppearance.BorderColor = Color.Gray
                    i.BackColor = g_cDarkControlColor.mDarkBackground
                    i.ForeColor = g_cDarkControlColor.mDarkForeground
                Else
                    i.UseVisualStyleBackColor = True
                    i.FlatStyle = FlatStyle.System
                    i.BackColor = g_cDarkControlColor.mLightBackground
                    i.ForeColor = g_cDarkControlColor.mLightForegound
                End If

            Case TypeOf o Is CheckBox
                Dim i As CheckBox = DirectCast(o, CheckBox)
                If (m_IsInvertedColors) Then
                    i.UseVisualStyleBackColor = False
                    i.FlatStyle = FlatStyle.Standard
                    i.FlatAppearance.BorderSize = 0
                    i.BackColor = Color.Transparent
                    i.ForeColor = g_cDarkControlColor.mDarkForeground
                Else
                    i.UseVisualStyleBackColor = True
                    i.FlatStyle = FlatStyle.System
                    i.FlatAppearance.BorderSize = 0
                    i.BackColor = g_cDarkControlColor.mLightBackground
                    i.ForeColor = g_cDarkControlColor.mLightForegound
                End If

            Case TypeOf o Is RadioButton
                Dim i As RadioButton = DirectCast(o, RadioButton)
                If (m_IsInvertedColors) Then
                    i.UseVisualStyleBackColor = False
                    i.FlatStyle = FlatStyle.Standard
                    i.FlatAppearance.BorderSize = 0
                    i.BackColor = Color.Transparent
                    i.ForeColor = g_cDarkControlColor.mDarkForeground
                Else
                    i.UseVisualStyleBackColor = True
                    i.FlatStyle = FlatStyle.System
                    i.FlatAppearance.BorderSize = 0
                    i.BackColor = g_cDarkControlColor.mLightBackground
                    i.ForeColor = g_cDarkControlColor.mLightForegound
                End If

            Case TypeOf o Is ButtonBase
                Dim i As ButtonBase = DirectCast(o, ButtonBase)
                If (m_IsInvertedColors) Then
                    i.UseVisualStyleBackColor = False
                    i.FlatStyle = FlatStyle.Flat
                    i.FlatAppearance.BorderSize = 0
                    i.BackColor = Color.Transparent
                    i.ForeColor = g_cDarkControlColor.mDarkForeground
                Else
                    i.UseVisualStyleBackColor = True
                    i.FlatStyle = FlatStyle.System
                    i.FlatAppearance.BorderSize = 0
                    i.BackColor = g_cDarkControlColor.mLightBackground
                    i.ForeColor = g_cDarkControlColor.mLightForegound
                End If

            Case TypeOf o Is LinkLabel
                Dim i As LinkLabel = DirectCast(o, LinkLabel)
                If (m_IsInvertedColors) Then
                    i.LinkColor = Color.DodgerBlue
                    i.BackColor = Color.Transparent
                    i.ForeColor = g_cDarkControlColor.mDarkForeground
                Else
                    i.LinkColor = Color.RoyalBlue
                    i.BackColor = Color.Transparent
                    i.ForeColor = g_cDarkControlColor.mLightForegound
                End If

            Case TypeOf o Is Label
                Dim i As Label = DirectCast(o, Label)
                Select Case (True)
                    Case HasNameFlag(i, ENUM_STYLE_FLAGS.LABEL_BLACK)
                        If (m_IsInvertedColors) Then
                            i.BackColor = Color.Transparent
                            i.ForeColor = Color.Black
                        Else
                            i.BackColor = Color.Transparent
                            i.ForeColor = Color.Black
                        End If

                    Case HasNameFlag(i, ENUM_STYLE_FLAGS.LABEL_ROYALBLUE)
                        If (m_IsInvertedColors) Then
                            i.BackColor = Color.Transparent
                            i.ForeColor = Color.DodgerBlue
                        Else
                            i.BackColor = g_cDarkFormColor.mLightBackground
                            i.ForeColor = Color.RoyalBlue
                        End If

                    Case Else
                        If (m_IsInvertedColors) Then
                            i.BackColor = Color.Transparent
                            i.ForeColor = g_cDarkFormColor.mDarkForeground
                        Else
                            i.BackColor = g_cDarkFormColor.mLightBackground
                            i.ForeColor = g_cDarkFormColor.mLightForegound
                        End If
                End Select

            Case TypeOf o Is TextBox
                Dim i As TextBox = DirectCast(o, TextBox)
                If (m_IsInvertedColors) Then
                    i.BackColor = g_cDarkControlColor.mDarkBackground
                    i.ForeColor = g_cDarkControlColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkControlColor.mLightBackground
                    i.ForeColor = g_cDarkControlColor.mLightForegound
                End If

            Case TypeOf o Is RichTextBox
                Dim i As RichTextBox = DirectCast(o, RichTextBox)
                If (m_IsInvertedColors) Then
                    i.BackColor = g_cDarkControlColor.mDarkBackground
                    i.ForeColor = g_cDarkControlColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkControlColor.mLightBackground
                    i.ForeColor = g_cDarkControlColor.mLightForegound
                End If

                'Fix RichTextBox drawing issues
                i.BackColor = i.BackColor
                i.ForeColor = i.ForeColor

            Case TypeOf o Is ToolStripLabel
                Dim i As ToolStripLabel = DirectCast(o, ToolStripLabel)
                Select Case (True)
                    Case HasNameFlag(i, ENUM_STYLE_FLAGS.CONTROL_KEEP_BACKCOLOR)
                        If (m_IsInvertedColors) Then
                            i.ForeColor = Color.Black
                        Else
                            i.ForeColor = g_cDarkFormColor.mLightForegound
                        End If

                    Case Else
                        If (m_IsInvertedColors) Then
                            If (i.IsLink) Then
                                i.LinkColor = Color.DodgerBlue
                                i.BackColor = Color.Transparent
                                i.ForeColor = g_cDarkFormColor.mDarkForeground
                            Else
                                i.BackColor = Color.Transparent
                                i.ForeColor = g_cDarkFormColor.mDarkForeground
                            End If
                        Else
                            If (i.IsLink) Then
                                i.LinkColor = Color.RoyalBlue
                                i.BackColor = Color.Transparent
                                i.ForeColor = g_cDarkFormColor.mLightForegound
                            Else
                                i.BackColor = g_cDarkFormColor.mLightBackground
                                i.ForeColor = g_cDarkFormColor.mLightForegound
                            End If
                        End If
                End Select

            Case TypeOf o Is StatusStrip
                Dim i As StatusStrip = DirectCast(o, StatusStrip)
                If (m_IsInvertedColors) Then
                    i.RenderMode = ToolStripRenderMode.ManagerRenderMode
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.RenderMode = ToolStripRenderMode.System
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

                Select Case (True)
                    Case HasNameFlag(i, ENUM_STYLE_FLAGS.MENU_SYSTEMRENDER)
                        i.Renderer = New ToolStripSystemRenderer()

                    Case Else
                        If (m_IsInvertedColors) Then
                            i.Renderer = New ClassToolStripCustomRenderer(g_cDarkMenuColor.mDarkBackground, g_cDarkMenuColor.mDarkBackground, g_cDarkMenuColor.mDarkForeground, g_cDarkControlColor.mDarkBackground, g_cDarkControlColor.mDarkBackground)
                        Else
                            i.Renderer = New ToolStripSystemRenderer()
                        End If
                End Select

            Case TypeOf o Is ContextMenuStrip
                Dim i As ContextMenuStrip = DirectCast(o, ContextMenuStrip)
                If (m_IsInvertedColors) Then
                    i.RenderMode = ToolStripRenderMode.ManagerRenderMode
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.RenderMode = ToolStripRenderMode.System
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

                Select Case (True)
                    Case HasNameFlag(i, ENUM_STYLE_FLAGS.MENU_SYSTEMRENDER)
                        i.Renderer = New ToolStripSystemRenderer()

                    Case Else
                        If (m_IsInvertedColors) Then
                            i.Renderer = New ClassToolStripCustomRenderer(g_cDarkMenuColor.mDarkBackground, g_cDarkMenuColor.mDarkBackground, g_cDarkMenuColor.mDarkForeground, g_cDarkControlColor.mDarkBackground, g_cDarkControlColor.mDarkBackground)
                        Else
                            i.Renderer = New ToolStripSystemRenderer()
                        End If
                End Select

            Case TypeOf o Is ToolStripDropDownMenu
                Dim i As ToolStripDropDownMenu = DirectCast(o, ToolStripDropDownMenu)
                If (m_IsInvertedColors) Then
                    i.RenderMode = ToolStripRenderMode.ManagerRenderMode
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.RenderMode = ToolStripRenderMode.System
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

                Select Case (True)
                    Case HasNameFlag(i, ENUM_STYLE_FLAGS.MENU_SYSTEMRENDER)
                        i.Renderer = New ToolStripSystemRenderer()

                    Case Else
                        If (m_IsInvertedColors) Then
                            i.Renderer = New ClassToolStripCustomRenderer(g_cDarkMenuColor.mDarkBackground, g_cDarkMenuColor.mDarkBackground, g_cDarkMenuColor.mDarkForeground, g_cDarkControlColor.mDarkBackground, g_cDarkControlColor.mDarkBackground)
                        Else
                            i.Renderer = New ToolStripSystemRenderer()
                        End If
                End Select

            Case TypeOf o Is ToolStripDropDown
                Dim i As ToolStripDropDown = DirectCast(o, ToolStripDropDown)
                If (m_IsInvertedColors) Then
                    i.RenderMode = ToolStripRenderMode.ManagerRenderMode
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.RenderMode = ToolStripRenderMode.System
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

                Select Case (True)
                    Case HasNameFlag(i, ENUM_STYLE_FLAGS.MENU_SYSTEMRENDER)
                        i.Renderer = New ToolStripSystemRenderer()

                    Case Else
                        If (m_IsInvertedColors) Then
                            i.Renderer = New ClassToolStripCustomRenderer(g_cDarkMenuColor.mDarkBackground, g_cDarkMenuColor.mDarkBackground, g_cDarkMenuColor.mDarkForeground, g_cDarkControlColor.mDarkBackground, g_cDarkControlColor.mDarkBackground)
                        Else
                            i.Renderer = New ToolStripSystemRenderer()
                        End If
                End Select

            Case TypeOf o Is ToolStripDropDownItem
                Dim i As ToolStripDropDownItem = DirectCast(o, ToolStripDropDownItem)
                If (m_IsInvertedColors) Then
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

            Case TypeOf o Is ToolStripMenuItem
                Dim i As ToolStripMenuItem = DirectCast(o, ToolStripMenuItem)
                If (m_IsInvertedColors) Then
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

            Case TypeOf o Is ToolStripItem
                Dim i As ToolStripItem = DirectCast(o, ToolStripItem)
                Select Case (True)
                    Case HasNameFlag(i, ENUM_STYLE_FLAGS.CONTROL_KEEP_BACKCOLOR)
                        If (m_IsInvertedColors) Then
                            i.ForeColor = Color.Black
                        Else
                            i.ForeColor = g_cDarkMenuColor.mLightForegound
                        End If

                    Case Else
                        If (m_IsInvertedColors) Then
                            i.BackColor = g_cDarkMenuColor.mDarkBackground
                            i.ForeColor = g_cDarkMenuColor.mDarkForeground
                        Else
                            i.BackColor = g_cDarkMenuColor.mLightBackground
                            i.ForeColor = g_cDarkMenuColor.mLightForegound
                        End If
                End Select

            Case TypeOf o Is ToolStrip
                Dim i As ToolStrip = DirectCast(o, ToolStrip)
                If (m_IsInvertedColors) Then
                    i.RenderMode = ToolStripRenderMode.ManagerRenderMode
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.RenderMode = ToolStripRenderMode.System
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

                Select Case (True)
                    Case HasNameFlag(i, ENUM_STYLE_FLAGS.MENU_SYSTEMRENDER)
                        i.Renderer = New ToolStripSystemRenderer()

                    Case Else
                        If (m_IsInvertedColors) Then
                            i.Renderer = New ClassToolStripCustomRenderer(g_cDarkMenuColor.mDarkBackground, g_cDarkMenuColor.mDarkBackground, g_cDarkMenuColor.mDarkForeground, g_cDarkControlColor.mDarkBackground, g_cDarkControlColor.mDarkBackground)
                        Else
                            i.Renderer = New ToolStripSystemRenderer()
                        End If
                End Select

            Case TypeOf o Is Control
                Dim i As Control = DirectCast(o, Control)
                If (m_IsInvertedColors) Then
                    i.BackColor = g_cDarkControlColor.mDarkBackground
                    i.ForeColor = g_cDarkControlColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkControlColor.mLightBackground
                    i.ForeColor = g_cDarkControlColor.mLightForegound
                End If

        End Select

        If (TypeOf o Is Control) Then
            Dim i As Control = DirectCast(o, Control)
            SetColor(i.ContextMenuStrip)
        End If

        If (TypeOf o Is TabControl) Then
            Dim i As TabControl = DirectCast(o, TabControl)
            For Each j As Object In i.TabPages
                SetColor(j)
            Next
        End If

        If (TypeOf o Is ToolStrip) Then
            Dim i As ToolStrip = DirectCast(o, ToolStrip)
            For Each j As Object In i.Items
                SetColor(j)
            Next
        End If

        If (TypeOf o Is Menu) Then
            Dim i As Menu = DirectCast(o, Menu)
            For Each j As Object In i.MenuItems
                SetColor(j)
            Next
        End If

        If (TypeOf o Is ToolStripDropDownItem) Then
            Dim i As ToolStripDropDownItem = DirectCast(o, ToolStripDropDownItem)
            For Each j As Object In i.DropDownItems
                SetColor(j)
            Next
        End If
    End Sub

    Private Class ClassToolStripCustomRenderer
        Inherits ToolStripProfessionalRenderer

        Public Sub New(iToolStripBackgroundColor As Color, iImageMarginColor As Color, iMenuItemBorderColor As Color, iMenuItemPressedColor As Color, iMenuItemSelected As Color)
            MyBase.New(New CustomProfessionalColorTable(iToolStripBackgroundColor, iImageMarginColor, iMenuItemBorderColor, iMenuItemPressedColor, iMenuItemSelected))
        End Sub

        Private Class CustomProfessionalColorTable
            Inherits ProfessionalColorTable

            Private g_iToolStripBackgroundColor As Color
            Private g_iImageMarginColor As Color
            Private g_iMenuItemBorderColor As Color
            Private g_iMenuItemPressedColor As Color
            Private g_iMenuItemSelected As Color

            Public Sub New(iToolStripBackgroundColor As Color, iImageMarginColor As Color, iMenuItemBorderColor As Color, iMenuItemPressedColor As Color, iMenuItemSelected As Color)
                g_iToolStripBackgroundColor = iToolStripBackgroundColor
                g_iImageMarginColor = iImageMarginColor
                g_iMenuItemBorderColor = iMenuItemBorderColor
                g_iMenuItemPressedColor = iMenuItemPressedColor
                g_iMenuItemSelected = iMenuItemSelected
            End Sub



            'Menus
            Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
                Get
                    Return g_iToolStripBackgroundColor
                End Get
            End Property

            Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
                Get
                    Return g_iImageMarginColor
                End Get
            End Property
            Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
                Get
                    Return g_iImageMarginColor
                End Get
            End Property
            Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
                Get
                    Return g_iImageMarginColor
                End Get
            End Property

            Public Overrides ReadOnly Property MenuItemBorder As Color
                Get
                    Return g_iMenuItemBorderColor
                End Get
            End Property
            Public Overrides ReadOnly Property MenuItemPressedGradientBegin As Color
                Get
                    Return g_iMenuItemPressedColor
                End Get
            End Property
            Public Overrides ReadOnly Property MenuItemPressedGradientEnd As Color
                Get
                    Return g_iMenuItemPressedColor
                End Get
            End Property
            Public Overrides ReadOnly Property MenuItemPressedGradientMiddle As Color
                Get
                    Return g_iMenuItemPressedColor
                End Get
            End Property
            Public Overrides ReadOnly Property MenuItemSelected() As Color
                Get
                    Return g_iMenuItemSelected
                End Get
            End Property
            Public Overrides ReadOnly Property MenuItemSelectedGradientBegin() As Color
                Get
                    Return g_iMenuItemSelected
                End Get
            End Property
            Public Overrides ReadOnly Property MenuItemSelectedGradientEnd() As Color
                Get
                    Return g_iMenuItemSelected
                End Get
            End Property
        End Class
    End Class
End Class
