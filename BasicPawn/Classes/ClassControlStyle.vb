'BasicPawn
'Copyright(C) 2016 TheTimocop

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

    Public Shared ReadOnly Property m_InvertColor As Boolean
        Get
            Return ClassSettings.g_iSettingsInvertColors
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

    Public Shared g_cDarkTextEditorColor As New STRUC_CONTROL_COLORS(Color.Black, Color.White, Color.LightGray, Color.FromArgb(255, 26, 26, 26))
    Public Shared g_cDarkControlColor As New STRUC_CONTROL_COLORS(Color.Black, Color.White, Color.LightGray, Color.FromArgb(255, 24, 24, 24))
    Public Shared g_cDarkPanelColor As New STRUC_CONTROL_COLORS(Color.Black, Color.White, Color.LightGray, Color.FromArgb(255, 32, 32, 32))
    Public Shared g_cDarkFormColor As New STRUC_CONTROL_COLORS(Color.Black, Color.White, Color.LightGray, Color.FromArgb(255, 48, 48, 48))
    Public Shared g_cDarkMenuColor As New STRUC_CONTROL_COLORS(Color.Black, Color.White, Color.LightGray, Color.FromArgb(255, 64, 64, 64))

    Private Shared Sub SetColor(o As Object)
        If (TypeOf o Is Control) Then
            Dim i As Control = DirectCast(o, Control)
            SetColor(i.ContextMenuStrip)
        End If

        Select Case True
            Case TypeOf o Is Form
                Dim i As Form = DirectCast(o, Form)
                If (m_InvertColor) Then
                    i.BackColor = g_cDarkFormColor.mDarkBackground
                    i.ForeColor = g_cDarkFormColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkFormColor.mLightBackground
                    i.ForeColor = g_cDarkFormColor.mLightForegound
                End If

            Case TypeOf o Is TabControl
                Dim i As TabControl = DirectCast(o, TabControl)
                If (m_InvertColor) Then
                    i.BackColor = g_cDarkPanelColor.mDarkBackground
                    i.ForeColor = g_cDarkPanelColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkPanelColor.mLightBackground
                    i.ForeColor = g_cDarkPanelColor.mLightForegound
                End If

                For Each j As Object In i.TabPages
                    SetColor(j)
                Next

            Case TypeOf o Is TabPage
                Dim i As TabPage = DirectCast(o, TabPage)
                If (m_InvertColor) Then
                    i.BackColor = g_cDarkPanelColor.mDarkBackground
                    i.ForeColor = g_cDarkPanelColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkPanelColor.mLightBackground
                    i.ForeColor = g_cDarkPanelColor.mLightForegound
                End If

            Case TypeOf o Is PictureBox
                Dim i As PictureBox = DirectCast(o, PictureBox)
                If (m_InvertColor) Then
                    i.BackColor = Color.Transparent
                    i.ForeColor = g_cDarkPanelColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkPanelColor.mLightBackground
                    i.ForeColor = g_cDarkPanelColor.mLightForegound
                End If

            Case TypeOf o Is Panel
                Dim i As Panel = DirectCast(o, Panel)
                Select Case (i.Name)
                    Case "Panel_FooterControl"
                        If (m_InvertColor) Then
                            i.BackColor = g_cDarkPanelColor.mDarkBackground
                            i.ForeColor = g_cDarkPanelColor.mDarkForeground
                        Else
                            i.BackColor = Color.FromKnownColor(KnownColor.Control)
                            i.ForeColor = g_cDarkPanelColor.mLightForegound
                        End If

                    Case "Panel_FooterDarkControl"
                        If (m_InvertColor) Then
                            i.BackColor = Color.Gray
                            i.ForeColor = g_cDarkPanelColor.mDarkForeground
                        Else
                            i.BackColor = Color.FromKnownColor(KnownColor.ControlDark)
                            i.ForeColor = g_cDarkPanelColor.mLightForegound
                        End If

                    Case "#Ignore"
                        'Ignore

                    Case Else
                        If (m_InvertColor) Then
                            i.BackColor = g_cDarkPanelColor.mDarkBackground
                            i.ForeColor = g_cDarkPanelColor.mDarkForeground
                        Else
                            i.BackColor = g_cDarkPanelColor.mLightBackground
                            i.ForeColor = g_cDarkPanelColor.mLightForegound
                        End If
                End Select

            Case TypeOf o Is Button
                Dim i As Button = DirectCast(o, Button)
                If (m_InvertColor) Then
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
                If (m_InvertColor) Then
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
                If (m_InvertColor) Then
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
                If (m_InvertColor) Then
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
                If (m_InvertColor) Then
                    i.LinkColor = Color.RoyalBlue
                    i.BackColor = Color.Transparent
                    i.ForeColor = g_cDarkControlColor.mDarkForeground
                Else
                    i.LinkColor = Color.Blue
                    i.BackColor = g_cDarkControlColor.mLightBackground
                    i.ForeColor = g_cDarkControlColor.mLightForegound
                End If

            Case TypeOf o Is Label
                Dim i As Label = DirectCast(o, Label)
                Select Case (i.Name)
                    Case "Label_IntelliSense", "Label_Autocomplete"
                        If (m_InvertColor) Then
                            i.BackColor = Color.Transparent
                            i.ForeColor = InvertColor(Color.RoyalBlue)
                        Else
                            i.BackColor = g_cDarkFormColor.mLightBackground
                            i.ForeColor = Color.RoyalBlue
                        End If

                    Case "Label_Title"
                        If (m_InvertColor) Then
                            i.BackColor = Color.Transparent
                            i.ForeColor = Color.Black
                        Else
                            i.BackColor = Color.Transparent
                            i.ForeColor = Color.Black
                        End If

                    Case Else
                        If (m_InvertColor) Then
                            i.BackColor = Color.Transparent
                            i.ForeColor = g_cDarkFormColor.mDarkForeground
                        Else
                            i.BackColor = g_cDarkFormColor.mLightBackground
                            i.ForeColor = g_cDarkFormColor.mLightForegound
                        End If
                End Select

            Case TypeOf o Is ToolStripLabel
                Dim i As ToolStripLabel = DirectCast(o, ToolStripLabel)
                Select Case (i.Name)
                    Case "ToolStripStatusLabel_DebugState"
                        If (m_InvertColor) Then
                            i.ForeColor = Color.Black
                        Else
                            i.ForeColor = g_cDarkFormColor.mLightForegound
                        End If

                    Case Else
                        If (m_InvertColor) Then
                            i.BackColor = Color.Transparent
                            i.ForeColor = g_cDarkFormColor.mDarkForeground
                        Else
                            i.BackColor = g_cDarkFormColor.mLightBackground
                            i.ForeColor = g_cDarkFormColor.mLightForegound
                        End If
                End Select

            Case TypeOf o Is StatusStrip
                Dim i As StatusStrip = DirectCast(o, StatusStrip)
                If (m_InvertColor) Then
                    i.RenderMode = ToolStripRenderMode.System
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.RenderMode = ToolStripRenderMode.System
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

                For Each j As Object In i.Items
                    SetColor(j)
                Next

            Case TypeOf o Is ContextMenu
                Dim i As ContextMenu = DirectCast(o, ContextMenu)
                For Each j As Object In i.MenuItems
                    SetColor(j)
                Next

            Case TypeOf o Is MenuItem
                Dim i As MenuItem = DirectCast(o, MenuItem)
                For Each j As Object In i.MenuItems
                    SetColor(j)
                Next

            Case TypeOf o Is Menu
                Dim i As Menu = DirectCast(o, Menu)
                For Each j As Object In i.MenuItems
                    SetColor(j)
                Next

            Case TypeOf o Is ContextMenuStrip
                Dim i As ContextMenuStrip = DirectCast(o, ContextMenuStrip)
                If (m_InvertColor) Then
                    i.RenderMode = ToolStripRenderMode.ManagerRenderMode
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.RenderMode = ToolStripRenderMode.System
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

                For Each j As Object In i.Items
                    SetColor(j)
                Next

            Case TypeOf o Is ToolStripDropDownMenu
                Dim i As ToolStripDropDownMenu = DirectCast(o, ToolStripDropDownMenu)
                If (m_InvertColor) Then
                    i.RenderMode = ToolStripRenderMode.ManagerRenderMode
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.RenderMode = ToolStripRenderMode.System
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

                For Each j As Object In i.Items
                    SetColor(j)
                Next

            Case TypeOf o Is ToolStripDropDown
                Dim i As ToolStripDropDown = DirectCast(o, ToolStripDropDown)
                If (m_InvertColor) Then
                    i.RenderMode = ToolStripRenderMode.ManagerRenderMode
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.RenderMode = ToolStripRenderMode.System
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

                For Each j As Object In i.Items
                    SetColor(j)
                Next

            Case TypeOf o Is ToolStripDropDownItem
                Dim i As ToolStripDropDownItem = DirectCast(o, ToolStripDropDownItem)
                If (m_InvertColor) Then
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

                For Each j As Object In i.DropDownItems
                    SetColor(j)
                Next

            Case TypeOf o Is ToolStripMenuItem
                Dim i As ToolStripMenuItem = DirectCast(o, ToolStripMenuItem)
                If (m_InvertColor) Then
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

                For Each j As Object In i.DropDownItems
                    SetColor(j)
                Next

            Case TypeOf o Is ToolStripItem
                Dim i As ToolStripItem = DirectCast(o, ToolStripItem)
                If (m_InvertColor) Then
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

            Case TypeOf o Is ToolStrip
                Dim i As ToolStrip = DirectCast(o, ToolStrip)
                If (m_InvertColor) Then
                    i.RenderMode = ToolStripRenderMode.ManagerRenderMode
                    i.BackColor = g_cDarkMenuColor.mDarkBackground
                    i.ForeColor = g_cDarkMenuColor.mDarkForeground
                Else
                    i.RenderMode = ToolStripRenderMode.System
                    i.BackColor = g_cDarkMenuColor.mLightBackground
                    i.ForeColor = g_cDarkMenuColor.mLightForegound
                End If

                For Each j As Object In i.Items
                    SetColor(j)
                Next

            Case TypeOf o Is Control
                Dim i As Control = DirectCast(o, Control)
                If (m_InvertColor) Then
                    i.BackColor = g_cDarkControlColor.mDarkBackground
                    i.ForeColor = g_cDarkControlColor.mDarkForeground
                Else
                    i.BackColor = g_cDarkControlColor.mLightBackground
                    i.ForeColor = g_cDarkControlColor.mLightForegound
                End If

        End Select

    End Sub


    Public Shared Sub UpdateControls(c As Control)
        SetColor(c)

        For Each i As Control In c.Controls
            UpdateControls(i)
        Next
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
End Class
