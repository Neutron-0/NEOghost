#region header

// MouseJiggler - MainForm.cs
// 
// Maintained by Neutron.
// Original source has been rebranded and cleaned for this repository.

#endregion

#region using

using NeoGhost.Controls;
using NeoGhost.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;

#endregion

namespace NeoGhost;

public partial class MainForm : Form
{
  private const int MaxNotifyIconTextLength = 63;
  private const int ToggleJigglingHotKeyId = 1;
  private const int HotKeyMessage = 0x0312;
  private const HOT_KEY_MODIFIERS ToggleJigglingHotKeyModifiers = HOT_KEY_MODIFIERS.MOD_CONTROL | HOT_KEY_MODIFIERS.MOD_SHIFT;
  private const VIRTUAL_KEY ToggleJigglingHotKeyKey = VIRTUAL_KEY.VK_J;
  private const string ToggleJigglingHotKeyText = "Ctrl+Shift+J";

  private const int CardCornerDiameter = 16;
  private const float BorderBeamTailFraction = 0.22f;

  private static readonly Color CardSurface = Color.FromArgb(28, 28, 30);
  private static readonly Color CardBorderMetallic = Color.FromArgb(26, 255, 255, 255);
  private static readonly Color ZincCharcoalBaseline = Color.FromArgb(39, 39, 42);
  private static readonly Color BeamPurpleTail = Color.FromArgb(190, 255, 255, 255);
  private static readonly Color BeamCyan = Color.White;

  private bool _hotKeyRegistered;

  // ── Border Beam Animation ───────────────────────────────────────
  private float _borderBeamProgress;
  private System.Windows.Forms.Timer? _borderBeamTimer;
  private IntPtr _formIconHandle;
  private IntPtr _trayIconHandle;

  [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
  private static extern bool DestroyIcon(IntPtr handle);

  /// <summary>
  ///     Constructor for use by the form designer.
  /// </summary>
  public MainForm ()
      : this (false, false, JiggleMode.Normal, false, 1, 1, false)
  { }

  public MainForm (bool jiggleOnStartup, bool minimizeOnStartup, JiggleMode jiggleMode, bool randomTimer, int jigglePeriod, int jiggleDistance, bool showSettings)
  {
    try
    {
      this.InitializeComponent ();

      this.DoubleBuffered = true;

    this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
    this.UpdateStyles();

    this.EnableControlDoubleBuffer(this.flpLayout);
    this.EnableControlDoubleBuffer(this.panelHeader);
    this.EnableControlDoubleBuffer(this.panelBase);
    this.EnableControlDoubleBuffer(this.panelActions);
    this.EnableControlDoubleBuffer(this.panelSettings);
    this.EnableControlDoubleBuffer(this.pnlIndicator);
    this.EnableControlDoubleBuffer(this.lblLogo);

    this.panelSettings.Visible = true;

    // Initialize JiggleMode combo box with enum values
    this.cmbJiggleMode.Items.Clear ();
    foreach (JiggleMode mode in Enum.GetValues<JiggleMode> ())
    {
      _ = this.cmbJiggleMode.Items.Add (mode);
    }

    // Jiggling on startup?
    this.JiggleOnStartup = jiggleOnStartup;

    // Set settings properties
    // We do this by setting the controls, and letting them set the properties.

    this.cbMinimize.Checked = minimizeOnStartup;
    this.cmbJiggleMode.SelectedItem = jiggleMode;
    this.cbRandom.Checked = randomTimer;
    this.RespectLockedState = Settings.Default.RespectLockedState;

    // Validate jigglePeriod before setting it
    if (jigglePeriod >= this.nudPeriod.Minimum && jigglePeriod <= this.nudPeriod.Maximum)
      this.nudPeriod.Value = jigglePeriod;
    else
      // Handle invalid jigglePeriod value, e.g., set to default or raise an error
      this.nudPeriod.Value = this.nudPeriod.Minimum; // or any default value within the range
    this.JigglePeriod = (int)this.nudPeriod.Value;

    // Validate jiggleDistance before setting it
    if (jiggleDistance >= this.nudDistance.Minimum && jiggleDistance <= this.nudDistance.Maximum)
      this.nudDistance.Value = jiggleDistance;
    else
      // Handle invalid jiggleDistance value, e.g., set to default or raise an error
      this.nudDistance.Value = this.nudDistance.Minimum; // or any default value within the range
    this.JiggleDistance = (int)this.nudDistance.Value;

    // Component initial setting
    this.tsmiStartJiggling.Visible = !this.cbJiggling.Checked;
    this.tsmiStopJiggling.Visible  = this.cbJiggling.Checked;
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"[NEOghost] MainForm constructor failed: {ex}");
      try
      {
        // Keep app bootable even if styling/controls fail.
        this.BackColor = Color.Black;
      }
      catch
      {
        // ignore
      }
    }
  }


  public bool JiggleOnStartup { get; }

  private void MainForm_Load (object sender, EventArgs e)
  {
    SystemEvents.SessionSwitch += this.SystemEvents_SessionSwitch;

    // ── Programmatic Icon Assignment ────────────────────────────
    this.AssignDynamicIcons ();

    // ── Start Border Beam Animation Timer ───────────────────────
    this._borderBeamTimer = new System.Windows.Forms.Timer { Interval = 16 };
    this._borderBeamTimer.Tick += this.BorderBeamTimer_Tick;
    this._borderBeamTimer.Start ();

    if (this.JiggleOnStartup)
      this.cbJiggling.Checked = true;
  }

  protected override void OnFormClosing (FormClosingEventArgs e)
  {
    SystemEvents.SessionSwitch -= this.SystemEvents_SessionSwitch;

    // ── Dispose border beam timer ───────────────────────────────
    if (this._borderBeamTimer != null)
    {
      try
      {
        this._borderBeamTimer.Stop ();
        this._borderBeamTimer.Tick -= this.BorderBeamTimer_Tick;
        this._borderBeamTimer.Dispose ();
      }
      catch (Exception ex)
      {
        Debug.WriteLine($"[NEOghost] BorderBeamTimer dispose failed: {ex}");
      }
      this._borderBeamTimer = null;
    }

    // ── Destroy programmatic icon handles ────────────────────────
    try
    {
      if (this._formIconHandle != IntPtr.Zero)
      {
        DestroyIcon (this._formIconHandle);
        this._formIconHandle = IntPtr.Zero;
      }
      if (this._trayIconHandle != IntPtr.Zero)
      {
        DestroyIcon (this._trayIconHandle);
        this._trayIconHandle = IntPtr.Zero;
      }
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"[NEOghost] DestroyIcon failed: {ex}");
    }

    base.OnFormClosing (e);
  }


  private void UpdateNotificationAreaText ()
  {
    if (!this.cbJiggling.Checked)
    {
      this.niTray.Text = @"Not jiggling the mouse.";
    }
    else
    {
      var mode = this.JiggleMode.ToString ();
      var rnd = this.RandomTimer ? $@" with random variation," : string.Empty;
      var text = $@"Jiggling mouse every {this.JigglePeriod} s,{rnd} mode: {mode} (Δ {this.JiggleDistance}).";
      this.niTray.Text = text.Length > MaxNotifyIconTextLength ? text[..(MaxNotifyIconTextLength - 3)] + "..." : text;
    }
  }

  private void btnAbout_Click (object sender, EventArgs e) => new AboutBox ().ShowDialog (this);

  private void trayMenu_ClickOpen (object sender, EventArgs e) => this.niTray_DoubleClick (sender, e);

  private void trayMenu_ClickExit (object sender, EventArgs e) => Application.Exit ();

  private void trayMenu_ClickStartJiggling (object sender, EventArgs e)
  {
    this.cbJiggling.Checked = true;
    this.UpdateNotificationAreaText ();
  }

  private void trayMenu_ClickStopJiggling (object sender, EventArgs e)
  {
    this.cbJiggling.Checked = false;
    this.UpdateNotificationAreaText ();
  }

  protected override void OnHandleCreated (EventArgs e)
  {
    base.OnHandleCreated (e);
    this.RegisterToggleJigglingHotKey ();
  }

  private void btnClose_Click(object sender, EventArgs e) => Application.Exit();

  private void btnMinimize_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;

  // Interop for custom drag
  public const int WM_NCLBUTTONDOWN = 0xA1;
  public const int HT_CAPTION = 0x2;

  [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
  public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
  [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
  public static extern bool ReleaseCapture();

  internal void Form_MouseDown(object sender, MouseEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
    {
      ReleaseCapture();
      SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
    }
  }

  protected override CreateParams CreateParams
  {
    get
    {
      CreateParams cp = base.CreateParams;
      cp.ClassStyle |= 0x00020000; // CS_DROPSHADOW
      return cp;
    }
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);
    e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
    e.Graphics.Clear(Color.Black);
    this.DrawReferenceBackdrop(e.Graphics);
    using (Pen borderPen = new Pen(Color.FromArgb(24, 24, 27), 1))
    {
      e.Graphics.DrawRectangle(borderPen, 0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
    }
  }

  protected override void OnHandleDestroyed (EventArgs e)
  {
    this.UnregisterToggleJigglingHotKey ();
    base.OnHandleDestroyed (e);
  }

  protected override void WndProc (ref Message m)
  {
    if (m.Msg == HotKeyMessage && m.WParam == (IntPtr)ToggleJigglingHotKeyId)
      this.cbJiggling.Checked = !this.cbJiggling.Checked;

    base.WndProc (ref m);
  }

  private void RegisterToggleJigglingHotKey ()
  {
    if (this._hotKeyRegistered)
      return;

    this._hotKeyRegistered = PInvoke.RegisterHotKey (new HWND(this.Handle),
        ToggleJigglingHotKeyId,
        ToggleJigglingHotKeyModifiers,
        (uint)ToggleJigglingHotKeyKey);

    if (!this._hotKeyRegistered)
    {
      Debugger.Log (1, nameof (MainForm), $"failed to register {ToggleJigglingHotKeyText} hotkey.\n");
      _ = MessageBox.Show (this,
          $@"Could not register global hotkey ({ToggleJigglingHotKeyText}). It may already be in use by another application.",
          @"NEOghost",
          MessageBoxButtons.OK,
          MessageBoxIcon.Warning);
    }
  }

  private void UnregisterToggleJigglingHotKey ()
  {
    if (!this._hotKeyRegistered)
      return;

    _ = PInvoke.UnregisterHotKey (new HWND(this.Handle), ToggleJigglingHotKeyId);
    this._hotKeyRegistered = false;
  }

  #region Property synchronization

  private void cbMinimize_CheckedChanged (object sender, EventArgs e) => this.MinimizeOnStartup = this.cbMinimize.Checked;

  private void cmbJiggleMode_DrawItem(object sender, DrawItemEventArgs e)
  {
    if (e.Index < 0) return;

    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

    if (sender is not ComboBox combo) return;
    var rect = e.Bounds;

    // Draw flat dark surface-container to suppress the standard Windows bevel.
    using var brush = new SolidBrush(Color.FromArgb(28, 28, 30));
    e.Graphics.FillRectangle(brush, rect);

    // Draw border
    using var pen = new Pen(CardBorderMetallic, 1);
    e.Graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);

    // Draw centered white text with null-safe font fallback
    var text = combo.Items[e.Index]?.ToString() ?? string.Empty;
    using var textBrush = new SolidBrush(Color.White);
    Font drawFont = e.Font ?? this.Font ?? SystemFonts.DefaultFont;
    var stringFormat = new StringFormat
    {
        Alignment = StringAlignment.Center,
        LineAlignment = StringAlignment.Center
    };
    e.Graphics.DrawString(text, drawFont, textBrush, rect, stringFormat);
  }

  private void cmbJiggleMode_SelectedIndexChanged (object sender, EventArgs e)
  {
    if (this.cmbJiggleMode.SelectedItem is JiggleMode mode)
    {
      this.JiggleMode = mode;
      this.Pattern = mode switch
      {
        JiggleMode.Normal => JigglePatterns.Normal,
        JiggleMode.Zen => JigglePatterns.Zen,
        JiggleMode.Circle => JigglePatterns.Circle,
        JiggleMode.Linear => JigglePatterns.Linear,
        _ => throw new ArgumentOutOfRangeException (null, mode, "No pattern exists for specified mode.")
      };
    }
  }

  private void cbRandom_CheckedChanged (object sender, EventArgs e) => this.RandomTimer = this.cbRandom.Checked;

  private void nudPeriod_ValueChanged (object sender, EventArgs e) => this.JigglePeriod = (int)this.nudPeriod.Value;

  private void nudDistance_ValueChanged (object sender, EventArgs e) => this.JiggleDistance = (int)this.nudDistance.Value;

  #endregion Property synchronization

  #region Do the Jiggle!

  protected (int deltax, int deltay)[] Pattern = null!;
  protected int Step = 0;

  private void cbJiggling_CheckedChanged (object sender, EventArgs e)
  {
    if (this.cbJiggling.Checked)
      Helpers.StayAwake ();
    else
      Helpers.AllowSleep ();

    this.Step = 0;
    this.jiggleTimer.Enabled = this.cbJiggling.Checked;
    this.UpdateStatusIndicator ();
    this.UpdateTrayMenu ();
    this.UpdateNotificationAreaText ();
  }

  private void UpdateTrayMenu ()
  {
    this.tsmiStartJiggling.Visible = !this.cbJiggling.Checked;
    this.tsmiStopJiggling.Visible  = this.cbJiggling.Checked;
  }

  private void jiggleTimer_Tick (object sender, EventArgs e)
  {
    // Don't jiggle if the user has moved the mouse since the last jiggle, to avoid interfering with user input.
    if (Helpers.HasMouseMoved ())
    {
      return;
    }

    var (deltax, deltay) = this.Pattern[this.Step];
    this.Step++;

    if (this.Step >= this.Pattern.Length)
      this.Step = 0;

    Helpers.Jiggle (deltax, deltay);

    Helpers.UpdateMousePosition ();

    if (this.RandomTimer)
    {
      var newInterval = Random.Shared.Next(1, this.JigglePeriod + 1) * 1000;
      this.lbPeriod.Text = $@"{newInterval / 1000} s";
      this.jiggleTimer.Interval = newInterval;
    }
    else
      this.jiggleTimer.Interval = this.JigglePeriod * 1000;
  }

  #endregion Do the Jiggle!

  #region Visual status

  private void UpdateStatusIndicator ()
  {
    bool active = this.cbJiggling.Checked;
    this.pnlIndicator.Invalidate ();
    this.lblStatusText.Text      = active ? "ACTIVE" : "IDLE";
    this.lblStatusText.ForeColor = active
      ? System.Drawing.Color.FromArgb (16, 185, 129)
      : System.Drawing.Color.FromArgb (161, 161, 170);
    this.lblJiggleTitle.ForeColor = active
      ? System.Drawing.Color.White
      : System.Drawing.Color.FromArgb (196, 199, 200);
  }

  private void pnlIndicator_Paint (object sender, System.Windows.Forms.PaintEventArgs e)
  {
    var g = e.Graphics;
    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
    if (this.cbJiggling.Checked)
    {
      using var outerGlow = new System.Drawing.SolidBrush (System.Drawing.Color.FromArgb (25, 16, 185, 129));
      g.FillEllipse (outerGlow, 0, 0, 20, 20);
      using var midGlow = new System.Drawing.SolidBrush (System.Drawing.Color.FromArgb (55, 16, 185, 129));
      g.FillEllipse (midGlow, 3, 3, 14, 14);
      using var core = new System.Drawing.SolidBrush (System.Drawing.Color.FromArgb (16, 185, 129));
      g.FillEllipse (core, 6, 6, 8, 8);
    }
    else
    {
      using var core = new System.Drawing.SolidBrush (System.Drawing.Color.FromArgb (71, 85, 105));
      g.FillEllipse (core, 6, 6, 8, 8);
    }
  }

  private void DrawReferenceBackdrop (Graphics g)
  {
    g.SmoothingMode = SmoothingMode.AntiAlias;
    g.CompositingQuality = CompositingQuality.HighQuality;

    using (var glowPath = new GraphicsPath())
    {
      var radius = Math.Max(this.ClientSize.Width, this.ClientSize.Height) * 0.72f;
      glowPath.AddEllipse(
        this.ClientSize.Width / 2f - radius,
        this.ClientSize.Height / 2f - radius,
        radius * 2f,
        radius * 2f);

      using var brush = new PathGradientBrush(glowPath)
      {
        CenterColor = Color.FromArgb(24, 24, 24, 28),
        SurroundColors = new[] { Color.Black },
        CenterPoint = new PointF(this.ClientSize.Width / 2f, this.ClientSize.Height / 2f)
      };
      g.FillRectangle(brush, this.ClientRectangle);
    }

    var time = this._borderBeamProgress * MathF.PI * 2f;
    var center = new PointF(this.ClientSize.Width / 2f, this.ClientSize.Height * 0.48f);
    for (int i = 0; i < 58; i++)
    {
      var seed = i * 19.137f;
      var angle = Fract(MathF.Sin(seed) * 43758.545f) * MathF.PI * 2f + time * 0.18f;
      var orbit = 80f + Fract(MathF.Sin(seed + 7.7f) * 12231.44f) * 190f;
      var wobble = MathF.Sin(time + i * 0.41f) * 18f;
      var x = center.X + MathF.Cos(angle) * orbit;
      var y = center.Y + MathF.Sin(angle) * (orbit * 0.48f) + wobble;
      var alpha = (int)(18 + Fract(MathF.Sin(seed + 31.9f) * 9182.2f) * 42);

      using var particle = new SolidBrush(Color.FromArgb(alpha, 255, 255, 255));
      g.FillEllipse(particle, x, y, 1.2f, 1.2f);
    }
  }

  private static float Fract (float value) => value - MathF.Floor(value);

  private void CustomPanel_Paint (object sender, System.Windows.Forms.PaintEventArgs e)
  {
    var g = e.Graphics;
    g.SmoothingMode = SmoothingMode.AntiAlias;
    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
    g.CompositingQuality = CompositingQuality.HighQuality;
    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

    if (sender is not Panel panel)
      return;

    var rect = new RectangleF(0.5f, 0.5f, panel.ClientSize.Width - 1f, panel.ClientSize.Height - 1f);
    using var path = CreateRoundedRectPath(rect, CardCornerDiameter);

    using (var fillBrush = new SolidBrush(CardSurface))
      g.FillPath(fillBrush, path);

    using (var borderPen = new Pen(ZincCharcoalBaseline, 1f))
      g.DrawPath(borderPen, path);

    using (var metallicPen = new Pen(CardBorderMetallic, 1f))
      g.DrawPath(metallicPen, path);

    DrawBorderBeam(g, path, rect, CardCornerDiameter, this._borderBeamProgress);
  }

  private static GraphicsPath CreateRoundedRectPath (RectangleF rect, int cornerDiameter)
  {
    var path = new GraphicsPath();
    float r = Math.Min(cornerDiameter, Math.Min(rect.Width, rect.Height));
    float d = r;
    path.AddArc(rect.X, rect.Y, d, d, 180, 90);
    path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
    path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
    path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
    path.CloseFigure();
    return path;
  }

  private static float GetRoundedRectPerimeter (RectangleF rect, float cornerDiameter)
  {
    float r = Math.Min(cornerDiameter / 2f, Math.Min(rect.Width, rect.Height) / 2f);
    float straight = 2f * (rect.Width + rect.Height - cornerDiameter * 2f);
    float arcs = 2f * MathF.PI * r;
    return straight + arcs;
  }

  private static (PointF point, float tangentAngle) GetPointOnRoundedRectPerimeter (RectangleF rect, float cornerDiameter, float progress)
  {
    float r = Math.Min(cornerDiameter / 2f, Math.Min(rect.Width, rect.Height) / 2f);
    float top = rect.Width - cornerDiameter;
    float right = rect.Height - cornerDiameter;
    float bottom = rect.Width - cornerDiameter;
    float left = rect.Height - cornerDiameter;
    float arcSegment = MathF.PI * r / 2f;
    float total = top + right + bottom + left + (4f * arcSegment);
    float d = (progress - MathF.Truncate(progress)) * total;

    float x = rect.X + r;
    float y = rect.Y;

    if (d <= top)
      return (new PointF(x + d, y), 0f);
    d -= top;

    if (d <= arcSegment)
    {
      float angle = -MathF.PI / 2f + (d / arcSegment) * (MathF.PI / 2f);
      float cx = rect.Right - r;
      float cy = rect.Y + r;
      return (new PointF(cx + r * MathF.Cos(angle), cy + r * MathF.Sin(angle)), angle + MathF.PI / 2f);
    }
    d -= arcSegment;

    if (d <= right)
      return (new PointF(rect.Right, rect.Y + r + d), MathF.PI / 2f);
    d -= right;

    if (d <= arcSegment)
    {
      float angle = (d / arcSegment) * (MathF.PI / 2f);
      float cx = rect.Right - r;
      float cy = rect.Bottom - r;
      return (new PointF(cx + r * MathF.Cos(angle), cy + r * MathF.Sin(angle)), angle + MathF.PI / 2f);
    }
    d -= arcSegment;

    if (d <= bottom)
      return (new PointF(rect.Right - r - d, rect.Bottom), MathF.PI);
    d -= bottom;

    if (d <= arcSegment)
    {
      float angle = MathF.PI / 2f + (d / arcSegment) * (MathF.PI / 2f);
      float cx = rect.X + r;
      float cy = rect.Bottom - r;
      return (new PointF(cx + r * MathF.Cos(angle), cy + r * MathF.Sin(angle)), angle + MathF.PI / 2f);
    }
    d -= arcSegment;

    if (d <= left)
      return (new PointF(rect.X, rect.Bottom - r - d), -MathF.PI / 2f);
    d -= left;

    float finalAngle = MathF.PI + (d / arcSegment) * (MathF.PI / 2f);
    float finalCx = rect.X + r;
    float finalCy = rect.Y + r;
    return (new PointF(finalCx + r * MathF.Cos(finalAngle), finalCy + r * MathF.Sin(finalAngle)), finalAngle + MathF.PI / 2f);
  }

  private static void DrawBorderBeam (Graphics g, GraphicsPath shapePath, RectangleF bounds, float cornerDiameter, float progress)
  {
    float normalizedProgress = progress - MathF.Truncate(progress);
    float tailProgress = normalizedProgress - BorderBeamTailFraction;
    if (tailProgress < 0f)
      tailProgress += 1f;

    var (head, tangentAngle) = GetPointOnRoundedRectPerimeter(bounds, cornerDiameter, normalizedProgress);
    var (tail, _) = GetPointOnRoundedRectPerimeter(bounds, cornerDiameter, tailProgress);

    float perimeter = GetRoundedRectPerimeter(bounds, cornerDiameter);
    float beamSpan = perimeter * BorderBeamTailFraction;
    float cos = MathF.Cos(tangentAngle);
    float sin = MathF.Sin(tangentAngle);

    var gradientTail = new PointF(head.X - cos * beamSpan, head.Y - sin * beamSpan);
    var gradientHead = new PointF(head.X + cos * (beamSpan * 0.15f), head.Y + sin * (beamSpan * 0.15f));

    using var tailBrush = new LinearGradientBrush(gradientTail, gradientHead, Color.Transparent, Color.Transparent);
    tailBrush.InterpolationColors = new ColorBlend
    {
      Colors = new[]
      {
        Color.Transparent,
        Color.FromArgb(0, BeamPurpleTail),
        Color.FromArgb(48, BeamPurpleTail),
        Color.FromArgb(72, BeamCyan),
        Color.FromArgb(140, BeamCyan),
        Color.FromArgb(220, 255, 255, 255),
        Color.FromArgb(255, 255, 255, 255)
      },
      Positions = new[] { 0f, 0.12f, 0.28f, 0.48f, 0.68f, 0.88f, 1f }
    };

    using (var bloomPen = new Pen(tailBrush, 3.5f)
    {
      LineJoin = LineJoin.Round,
      StartCap = LineCap.Round,
      EndCap = LineCap.Round
    })
      g.DrawPath(bloomPen, shapePath);

    using var leadingBrush = new LinearGradientBrush(
      new PointF(tail.X, tail.Y),
      new PointF(head.X, head.Y),
      Color.FromArgb(160, BeamCyan),
      Color.FromArgb(255, 255, 255, 255));

    using (var leadingPen = new Pen(leadingBrush, 2f)
    {
      LineJoin = LineJoin.Round,
      StartCap = LineCap.Round,
      EndCap = LineCap.Round
    })
      g.DrawPath(leadingPen, shapePath);
  }

  private void EnableControlDoubleBuffer (Control control)
  {
    typeof(Control).InvokeMember(
      "DoubleBuffered",
      BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
      null,
      control,
      new object[] { true });
  }

  private void BorderBeamTimer_Tick (object? sender, EventArgs e)
  {
    this._borderBeamProgress = (this._borderBeamProgress + 0.008f) % 1.0f;
    this.panelHeader.Invalidate(false);
    this.panelBase.Invalidate(false);
    this.panelActions.Invalidate(false);
    this.panelSettings.Invalidate(false);
  }

  // ── Dynamic Icon Factory ──────────────────────────────────────────
  private void AssignDynamicIcons ()
  {
    var formIcon = CreateGlowingOrbIcon (32);
    this._formIconHandle = formIcon.Handle;
    this.Icon = formIcon;

    var trayIcon = CreateGlowingOrbIcon (16);
    this._trayIconHandle = trayIcon.Handle;
    this.niTray.Icon = trayIcon;
  }

  private static Icon CreateGlowingOrbIcon (int size)
  {
    using var bmp = new Bitmap(size, size, PixelFormat.Format32bppArgb);
    using (var g = Graphics.FromImage(bmp))
    {
      g.Clear(Color.Transparent);
      GlowingOrbPanel.DrawOrbToGraphics(g, size, size);
    }
    IntPtr hIcon = bmp.GetHicon();
    return Icon.FromHandle(hIcon);
  }


  #endregion Visual status

  private void SystemEvents_SessionSwitch (object sender, SessionSwitchEventArgs e)
  {
    if (this.InvokeRequired)
    {
      this.BeginInvoke (() => this.SystemEvents_SessionSwitch (sender, e));
      return;
    }

    if (!this.RespectLockedState)
      return;

    if (e.Reason == SessionSwitchReason.SessionLock && this.cbJiggling.Checked)
    {
      this._resumeJigglingAfterUnlock = true;
      this.cbJiggling.Checked = false;
    }
    else if (e.Reason == SessionSwitchReason.SessionUnlock && this._resumeJigglingAfterUnlock)
    {
      this._resumeJigglingAfterUnlock = false;
      this.cbJiggling.Checked = true;
    }
  }

  #region Minimize and restore

  private void btnTray_Click (object sender, EventArgs e) => this.MinimizeToTray ();

  private void niTray_DoubleClick (object sender, EventArgs e) => this.RestoreFromTray ();

  private void MinimizeToTray ()
  {
    this.Visible = false;
    this.ShowInTaskbar = false;
    this.niTray.Visible = true;

    this.UpdateNotificationAreaText ();
  }

  private void RestoreFromTray ()
  {
    this.Visible = true;
    this.ShowInTaskbar = true;
    this.niTray.Visible = false;
  }

  #endregion Minimize and restore

  #region Settings property backing fields

  private int _jigglePeriod;

  private bool _minimizeOnStartup;

  private bool _randomTimer;

  private JiggleMode _jiggleMode;

  private int _jiggleDistance;

  private bool _respectLockedState;

  private bool _resumeJigglingAfterUnlock;

  #endregion Settings property backing fields

  #region Settings properties

  [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]

  public bool MinimizeOnStartup
  {
    get => this._minimizeOnStartup;
    set
    {
      this._minimizeOnStartup = value;
      Settings.Default.MinimizeOnStartup = value;
      Settings.Default.Save ();
      this.OnPropertyChanged (nameof (this.MinimizeOnStartup));
    }
  }

  [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]

  public bool RandomTimer
  {
    get => this._randomTimer;
    set
    {
      this._randomTimer = value;
      Settings.Default.RandomTimer = value;
      Settings.Default.Save ();
      this.OnPropertyChanged (nameof (this.RandomTimer));
    }
  }

  [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]

  public JiggleMode JiggleMode
  {
    get => this._jiggleMode;
    set
    {
      // Validate that the value is a defined enum value
      if (!Enum.IsDefined (value))
        throw new ArgumentOutOfRangeException (nameof (value), value, "Invalid JiggleMode value");

      this._jiggleMode = value;
      Settings.Default.JiggleMode = value.ToString ();
      Settings.Default.Save ();
      this.OnPropertyChanged (nameof (this.JiggleMode));
    }
  }

  [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]

  public int JigglePeriod
  {
    get => this._jigglePeriod;
    set
    {
      this._jigglePeriod = value;
      Settings.Default.JigglePeriod = value;
      Settings.Default.Save ();

      this.jiggleTimer.Interval = value * 1000;
      this.lbPeriod.Text = $@"{value} s";

      this.OnPropertyChanged (nameof (this.JigglePeriod));
    }
  }

  [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]

  public int JiggleDistance
  {
    get => this._jiggleDistance;
    set
    {
      this._jiggleDistance = value;
      Settings.Default.JiggleDistance = value;
      Settings.Default.Save ();

      JigglePatterns.UpdatePatterns (value);

      this.Pattern = this.JiggleMode switch
      {
        JiggleMode.Normal => JigglePatterns.Normal,
        JiggleMode.Zen => JigglePatterns.Zen,
        JiggleMode.Circle => JigglePatterns.Circle,
        JiggleMode.Linear => JigglePatterns.Linear,
        _ => throw new ArgumentOutOfRangeException (null, this.JiggleMode, "No pattern exists for specified mode.")
      };

      this.OnPropertyChanged (nameof (this.JiggleDistance));
    }
  }

  [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]

  public bool RespectLockedState
  {
    get => this._respectLockedState;
    set
    {
      this._respectLockedState = value;
      Settings.Default.RespectLockedState = value;
      Settings.Default.Save ();
      this.OnPropertyChanged (nameof (this.RespectLockedState));
    }
  }

  private void OnPropertyChanged (string propertyName) => PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (propertyName));

  public event PropertyChangedEventHandler? PropertyChanged;

  #endregion Settings properties

  #region Minimize on start

  private bool _firstShown = true;

  private void MainForm_Shown (object sender, EventArgs e)
  {
    if (this._firstShown && this.MinimizeOnStartup)
      this.MinimizeToTray ();

    this._firstShown = false;
  }

  #endregion
}

