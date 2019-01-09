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


Public Class FormToolTip
    Private g_mFormMain As FormMain

    Private g_mLocation As Point
    Private g_mMoveLocation As Point
    Private g_iMoveStep As Integer = 16
    Private g_iSizeSpace As Integer = 8
    Private g_iMoveSpeed As Integer = 10
    Private g_iIdleSpeed As Integer = 100

    Public Sub New(f As FormMain)
        g_mFormMain = f

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 
        TextEditorControl_ToolTip.IsReadOnly = True
        TextEditorControl_ToolTip.TextEditorProperties.MouseWheelTextZoom = False
        TextEditorControl_ToolTip.TextEditorProperties.IndentationSize = 0
        TextEditorControl_ToolTip.ActiveTextAreaControl.AutoScroll = False
        TextEditorControl_ToolTip.ActiveTextAreaControl.AutoSize = False
        TextEditorControl_ToolTip.ActiveTextAreaControl.HScrollBar.Visible = False
        TextEditorControl_ToolTip.ActiveTextAreaControl.VScrollBar.Visible = False
        TextEditorControl_ToolTip.ActiveTextAreaControl.DoHandleMousewheel = False

        'Fix shitty disabled scrollbars side effects...
        If (True) Then
            Dim TextEditorLoc As Point
            Dim TextEditorRec As Rectangle

            TextEditorControl_ToolTip.Dock = DockStyle.Fill
            TextEditorLoc = TextEditorControl_ToolTip.Location
            TextEditorRec = TextEditorControl_ToolTip.Bounds
            TextEditorControl_ToolTip.Dock = DockStyle.None
            TextEditorControl_ToolTip.Location = TextEditorLoc
            TextEditorControl_ToolTip.Bounds = TextEditorRec
            TextEditorControl_ToolTip.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top

            TextEditorControl_ToolTip.Width += SystemInformation.VerticalScrollBarWidth
            TextEditorControl_ToolTip.Height += SystemInformation.HorizontalScrollBarHeight
        End If

        ClassTools.ClassForms.SetDoubleBufferingAllChilds(Me, True)
        ClassTools.ClassForms.SetDoubleBufferingUnmanagedAllChilds(Me, True)

        ClassControlStyle.UpdateControls(Me)
    End Sub

    Property m_Location As Point
        Get
            Return g_mLocation
        End Get
        Set(value As Point)
            Me.SuspendLayout()

            g_mMoveLocation = New Point()
            g_mLocation = value
            MoveWindow(value, True)

            Me.ResumeLayout()
        End Set
    End Property

    Private Sub Timer_Move_Tick(sender As Object, e As EventArgs) Handles Timer_Move.Tick
        If (Form.ActiveForm Is g_mFormMain AndAlso MoveWindow(Me.Location, Not ClassSettings.g_iSettingsUseWindowsToolTipAnimations)) Then
            Timer_Move.Interval = g_iMoveSpeed
        Else
            Timer_Move.Interval = g_iIdleSpeed
        End If
    End Sub

    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)

        Timer_Move.Enabled = Me.Visible
    End Sub

    Private Function MoveWindow(mStartLocation As Point, bInstant As Boolean) As Boolean
        Dim bMoved As Boolean = False

        Dim mCursorPoint As Point = Cursor.Position
        Dim mLocation As Point = mStartLocation
        If (mLocation = Point.Empty) Then
            mLocation = Me.Location
        End If

        While True
            If (ClassSettings.g_iSettingsUseWindowsToolTipDisplayTop) Then
                Const MAX_MOVE = -500
                Const MIN_MOVE = 0

                If (g_mMoveLocation.Y > MAX_MOVE AndAlso
                        (mCursorPoint.X + g_iSizeSpace > mLocation.X AndAlso mCursorPoint.Y + g_iSizeSpace > mLocation.Y AndAlso
                            mCursorPoint.X < mLocation.X + Me.Width + g_iSizeSpace AndAlso mCursorPoint.Y < mLocation.Y + Me.Height + g_iSizeSpace)) Then
                    g_mMoveLocation.Y -= g_iMoveStep
                    g_mMoveLocation.Y = Math.Max(g_mMoveLocation.Y, MAX_MOVE)

                    mLocation = New Point(g_mLocation.X, g_mLocation.Y + g_mMoveLocation.Y)

                    bMoved = True

                    If (bInstant) Then
                        Continue While
                    End If

                ElseIf (g_mMoveLocation.Y < MIN_MOVE AndAlso
                            Not (mCursorPoint.X + g_iSizeSpace + g_iMoveStep > mLocation.X AndAlso mCursorPoint.Y + g_iSizeSpace + g_iMoveStep > mLocation.Y AndAlso
                                    mCursorPoint.X < mLocation.X + Me.Width + g_iSizeSpace + g_iMoveStep AndAlso mCursorPoint.Y < mLocation.Y + Me.Height + g_iSizeSpace + g_iMoveStep)) Then
                    g_mMoveLocation.Y += g_iMoveStep
                    g_mMoveLocation.Y = Math.Min(g_mMoveLocation.Y, MIN_MOVE)

                    mLocation = New Point(g_mLocation.X, g_mLocation.Y + g_mMoveLocation.Y)

                    bMoved = True

                    If (bInstant) Then
                        Continue While
                    End If
                End If
            Else
                Const MAX_MOVE = 500
                Const MIN_MOVE = 0

                If (g_mMoveLocation.Y < MAX_MOVE AndAlso
                        (mCursorPoint.X + g_iSizeSpace > mLocation.X AndAlso mCursorPoint.Y + g_iSizeSpace > mLocation.Y AndAlso
                            mCursorPoint.X < mLocation.X + Me.Width + g_iSizeSpace AndAlso mCursorPoint.Y < mLocation.Y + Me.Height + g_iSizeSpace)) Then
                    g_mMoveLocation.Y += g_iMoveStep
                    g_mMoveLocation.Y = Math.Min(g_mMoveLocation.Y, MAX_MOVE)

                    mLocation = New Point(g_mLocation.X, g_mLocation.Y + g_mMoveLocation.Y)

                    bMoved = True

                    If (bInstant) Then
                        Continue While
                    End If

                ElseIf (g_mMoveLocation.Y > MIN_MOVE AndAlso
                            Not (mCursorPoint.X + g_iSizeSpace + g_iMoveStep > mLocation.X AndAlso mCursorPoint.Y + g_iSizeSpace + g_iMoveStep > mLocation.Y AndAlso
                                    mCursorPoint.X < mLocation.X + Me.Width + g_iSizeSpace + g_iMoveStep AndAlso mCursorPoint.Y < mLocation.Y + Me.Height + g_iSizeSpace + g_iMoveStep)) Then
                    g_mMoveLocation.Y -= g_iMoveStep
                    g_mMoveLocation.Y = Math.Max(g_mMoveLocation.Y, MIN_MOVE)

                    mLocation = New Point(g_mLocation.X, g_mLocation.Y + g_mMoveLocation.Y)

                    bMoved = True

                    If (bInstant) Then
                        Continue While
                    End If
                End If
            End If

            Exit While
        End While

        Me.Location = mLocation

        Return bMoved
    End Function

    Private Sub TextEditorControl_ToolTip_TextChanged(sender As Object, e As EventArgs) Handles TextEditorControl_ToolTip.TextChanged
        'TODO: Better DPI, Border detection, or size in general
        Me.SuspendLayout()

        Dim textSize = TextRenderer.MeasureText(TextEditorControl_ToolTip.Document.TextContent, New Font(TextEditorControl_ToolTip.ActiveTextAreaControl.Font, FontStyle.Bold))
        Me.Size = New Size(CInt(textSize.Width * 1.1), CInt(textSize.Height * 1.1))

        Me.ResumeLayout()
        Me.Refresh()
    End Sub

#Region "Form focus"
    Protected Overrides ReadOnly Property ShowWithoutActivation As Boolean
        Get
            Return True
        End Get
    End Property

    Const WS_EX_NOACTIVATE As Integer = &H8000000
    Const WS_EX_TOOLWINDOW As Integer = &H80

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim baseParams As CreateParams = MyBase.CreateParams
            baseParams.ExStyle = baseParams.ExStyle Or (WS_EX_NOACTIVATE Or WS_EX_TOOLWINDOW)
            Return baseParams
        End Get
    End Property

    Const WM_MOUSEACTIVATE As Integer = &H21
    Const MA_NOACTIVATEANDEAT As Integer = &H4

    Const WM_SETFOCUS As Integer = &H7
    Const WM_KILLFOCUS As Integer = &H8

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case (m.Msg)
            Case WM_SETFOCUS
                m.Msg = WM_KILLFOCUS
                MyBase.WndProc(m)

            Case WM_MOUSEACTIVATE
                m.Result = New IntPtr(MA_NOACTIVATEANDEAT)
                Return

            Case Else
                MyBase.WndProc(m)
        End Select
    End Sub
#End Region
End Class