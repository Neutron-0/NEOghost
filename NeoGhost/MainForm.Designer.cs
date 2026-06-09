namespace NeoGhost
{
  partial class MainForm
  {
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose (bool disposing)
    {
      if (disposing && (this.components != null))
        this.components.Dispose ();
      base.Dispose (disposing);
    }

    private void InitializeComponent ()
    {
      this.components = new System.ComponentModel.Container ();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MainForm));

      this.jiggleTimer = new System.Windows.Forms.Timer (this.components);
      this.niTray      = new System.Windows.Forms.NotifyIcon (this.components);
      this.trayMenu    = new System.Windows.Forms.ContextMenuStrip (this.components);
      this.tsmiOpen          = new System.Windows.Forms.ToolStripMenuItem ();
      this.tsmiStartJiggling = new System.Windows.Forms.ToolStripMenuItem ();
      this.tsmiStopJiggling  = new System.Windows.Forms.ToolStripMenuItem ();
      this.tsmiExit          = new System.Windows.Forms.ToolStripMenuItem ();

      this.flpLayout = new System.Windows.Forms.FlowLayoutPanel ();

      // Header
      this.panelHeader   = new System.Windows.Forms.Panel ();
      this.lblLogo       = new NeoGhost.Controls.GlowingOrbPanel ();
      this.lblAppName    = new System.Windows.Forms.Label ();
      this.lblSubtitle   = new System.Windows.Forms.Label ();
      this.pnlIndicator  = new System.Windows.Forms.Panel ();
      this.lblStatusText = new System.Windows.Forms.Label ();

      // Jiggle toggle row
      this.panelBase      = new System.Windows.Forms.Panel ();
      this.lblJiggleTitle = new System.Windows.Forms.Label ();
      this.cbJiggling     = new NeoGhost.Controls.ToggleSwitch ();

      // Action buttons
      this.panelActions = new System.Windows.Forms.Panel ();
      this.btnAbout     = new System.Windows.Forms.Button ();
      this.btnTray      = new System.Windows.Forms.Button ();

      this.btnClose     = new System.Windows.Forms.Button();
      this.btnMinimize  = new System.Windows.Forms.Button();

      // Settings panel
      this.panelSettings    = new System.Windows.Forms.Panel ();
      this.pnlSeparator     = new System.Windows.Forms.Panel ();
      this.lblModeTitle     = new System.Windows.Forms.Label ();
      this.cmbJiggleMode    = new System.Windows.Forms.ComboBox ();
      this.lblRandomTitle   = new System.Windows.Forms.Label ();
      this.cbRandom         = new NeoGhost.Controls.ToggleSwitch ();
      this.lblPeriodLabel   = new System.Windows.Forms.Label ();
      this.nudPeriod        = new System.Windows.Forms.NumericUpDown ();
      this.lbPeriod         = new System.Windows.Forms.Label ();
      this.lblDistanceLabel = new System.Windows.Forms.Label ();
      this.nudDistance      = new System.Windows.Forms.NumericUpDown ();
      this.lblMinimizeTitle = new System.Windows.Forms.Label ();
      this.cbMinimize       = new NeoGhost.Controls.ToggleSwitch ();

      this.flpLayout.SuspendLayout ();
      this.panelHeader.SuspendLayout ();
      this.panelBase.SuspendLayout ();
      this.panelActions.SuspendLayout ();
      this.panelSettings.SuspendLayout ();
      ((System.ComponentModel.ISupportInitialize) this.nudPeriod).BeginInit ();
      ((System.ComponentModel.ISupportInitialize) this.nudDistance).BeginInit ();
      this.trayMenu.SuspendLayout ();
      this.SuspendLayout ();

      // -- jiggleTimer --------------------------------------------------------
      this.jiggleTimer.Interval = 1000;
      this.jiggleTimer.Tick    += this.jiggleTimer_Tick;

      // -- trayMenu -----------------------------------------------------------
      this.trayMenu.BackColor = System.Drawing.Color.FromArgb (18, 19, 26);
      this.trayMenu.ForeColor = System.Drawing.Color.FromArgb (226, 225, 235);
      this.trayMenu.Items.AddRange (new System.Windows.Forms.ToolStripItem[] {
        this.tsmiOpen, this.tsmiStartJiggling, this.tsmiStopJiggling, this.tsmiExit });
      this.trayMenu.Name = "trayMenu";
      this.trayMenu.Size = new System.Drawing.Size (143, 92);

      this.tsmiOpen.Name   = "tsmiOpen";
      this.tsmiOpen.Text   = "Open";
      this.tsmiOpen.Click += this.niTray_DoubleClick;

      this.tsmiStartJiggling.Name   = "tsmiStartJiggling";
      this.tsmiStartJiggling.Text   = "Start Jiggling";
      this.tsmiStartJiggling.Click += this.trayMenu_ClickStartJiggling;

      this.tsmiStopJiggling.Name   = "tsmiStopJiggling";
      this.tsmiStopJiggling.Text   = "Stop Jiggling";
      this.tsmiStopJiggling.Click += this.trayMenu_ClickStopJiggling;

      this.tsmiExit.Name   = "tsmiExit";
      this.tsmiExit.Text   = "Exit";
      this.tsmiExit.Click += this.trayMenu_ClickExit;

      // -- niTray -------------------------------------------------------------
      this.niTray.ContextMenuStrip = this.trayMenu;
      this.niTray.Text             = "NEOghost";
      this.niTray.DoubleClick     += this.niTray_DoubleClick;

      // -- flpLayout ----------------------------------------------------------
      // Reference tokens: --spacing-gutter 24px, --color-pure-black #000000
      this.flpLayout.BackColor     = System.Drawing.Color.Transparent;
      this.flpLayout.Controls.Add (this.panelHeader);
      this.flpLayout.Controls.Add (this.panelBase);
      this.flpLayout.Controls.Add (this.panelActions);
      this.flpLayout.Controls.Add (this.panelSettings);
      this.flpLayout.Dock          = System.Windows.Forms.DockStyle.Fill;
      this.flpLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
      this.flpLayout.Location      = new System.Drawing.Point (0, 0);
      this.flpLayout.Margin        = new System.Windows.Forms.Padding (0);
      this.flpLayout.Name          = "flpLayout";
      this.flpLayout.Padding       = new System.Windows.Forms.Padding (24, 24, 24, 24);
      this.flpLayout.Size          = new System.Drawing.Size (460, 540);
      this.flpLayout.TabIndex      = 0;
      this.flpLayout.WrapContents  = false;

      // -- panelHeader --------------------------------------------------------
      // glass-panel surface: rgba(28,28,30,0.5) on pure black
      this.panelHeader.BackColor = System.Drawing.Color.Transparent;
      this.panelHeader.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.panelHeader.Controls.Add (this.lblLogo);
      this.panelHeader.Controls.Add (this.lblAppName);
      this.panelHeader.Controls.Add (this.lblSubtitle);
      this.panelHeader.Controls.Add (this.pnlIndicator);
      this.panelHeader.Controls.Add (this.lblStatusText);
      this.panelHeader.Controls.Add (this.btnClose);
      this.panelHeader.Controls.Add (this.btnMinimize);
      this.panelHeader.Margin    = new System.Windows.Forms.Padding (0, 0, 0, 16);
      this.panelHeader.Name      = "panelHeader";
      this.panelHeader.Size      = new System.Drawing.Size (412, 72);
      this.panelHeader.TabIndex  = 0;
      this.panelHeader.Paint    += this.CustomPanel_Paint;
      this.panelHeader.MouseDown += this.Form_MouseDown;

      this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
      this.btnMinimize.FlatAppearance.BorderSize = 0;
      this.btnMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(15, 255, 255, 255);
      this.btnMinimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(25, 255, 255, 255);
      this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnMinimize.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
      this.btnMinimize.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
      this.btnMinimize.Location = new System.Drawing.Point(346, 8);
      this.btnMinimize.Name = "btnMinimize";
      this.btnMinimize.Size = new System.Drawing.Size(24, 24);
      this.btnMinimize.TabIndex = 4;
      this.btnMinimize.Text = "-";
      this.btnMinimize.UseVisualStyleBackColor = false;
      this.btnMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnMinimize.Click += this.btnMinimize_Click;

      this.btnClose.BackColor = System.Drawing.Color.Transparent;
      this.btnClose.FlatAppearance.BorderSize = 0;
      this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(15, 255, 255, 255);
      this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(25, 255, 255, 255);
      this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.btnClose.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
      this.btnClose.Location = new System.Drawing.Point(376, 8);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(24, 24);
      this.btnClose.TabIndex = 5;
      this.btnClose.Text = "X";
      this.btnClose.UseVisualStyleBackColor = false;
      this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnClose.Click += this.btnClose_Click;

      this.lblLogo.BackColor = System.Drawing.Color.Transparent;
      this.lblLogo.Location  = new System.Drawing.Point (16, 14);
      this.lblLogo.Name      = "lblLogo";
      this.lblLogo.Size      = new System.Drawing.Size (44, 44);

      this.lblAppName.AutoSize  = false;
      this.lblAppName.BackColor = System.Drawing.Color.Transparent;
      this.lblAppName.Font      = new System.Drawing.Font ("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
      this.lblAppName.ForeColor = System.Drawing.Color.FromArgb (255, 255, 255);
      this.lblAppName.Location  = new System.Drawing.Point (68, 10);
      this.lblAppName.Name      = "lblAppName";
      this.lblAppName.Size      = new System.Drawing.Size (240, 28);
      this.lblAppName.Text      = "NEOghost";
      this.lblAppName.MouseDown += this.Form_MouseDown;

      this.lblSubtitle.AutoSize  = false;
      this.lblSubtitle.BackColor = System.Drawing.Color.Transparent;
      this.lblSubtitle.Font      = new System.Drawing.Font ("Segoe UI", 9F);
      this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb (161, 161, 170);
      this.lblSubtitle.Location  = new System.Drawing.Point (70, 40);
      this.lblSubtitle.Name      = "lblSubtitle";
      this.lblSubtitle.Size      = new System.Drawing.Size (200, 18);
      this.lblSubtitle.Text      = "prevent screen sleep";
      this.lblSubtitle.MouseDown += this.Form_MouseDown;

      // status indicator dot (20 x 20, custom-painted)
      this.pnlIndicator.BackColor = System.Drawing.Color.Transparent;
      this.pnlIndicator.Location  = new System.Drawing.Point (358, 22);
      this.pnlIndicator.Name      = "pnlIndicator";
      this.pnlIndicator.Size      = new System.Drawing.Size (20, 20);
      this.pnlIndicator.TabStop   = false;
      this.pnlIndicator.Paint    += this.pnlIndicator_Paint;

      this.lblStatusText.AutoSize  = false;
      this.lblStatusText.BackColor = System.Drawing.Color.Transparent;
      this.lblStatusText.Font      = new System.Drawing.Font ("Segoe UI", 7.5F);
      this.lblStatusText.ForeColor = System.Drawing.Color.FromArgb (161, 161, 170);
      this.lblStatusText.Location  = new System.Drawing.Point (328, 46);
      this.lblStatusText.Name      = "lblStatusText";
      this.lblStatusText.Size      = new System.Drawing.Size (60, 16);
      this.lblStatusText.Text      = "IDLE";
      this.lblStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

      // -- panelBase -- jiggle toggle row -------------------------------------
      this.panelBase.BackColor = System.Drawing.Color.Transparent;
      this.panelBase.Controls.Add (this.lblJiggleTitle);
      this.panelBase.Controls.Add (this.cbJiggling);
      this.panelBase.Margin    = new System.Windows.Forms.Padding (0, 0, 0, 16);
      this.panelBase.Name      = "panelBase";
      this.panelBase.Size      = new System.Drawing.Size (412, 88);
      this.panelBase.TabIndex  = 1;
      this.panelBase.Paint    += this.CustomPanel_Paint;

      this.lblJiggleTitle.AutoSize  = false;
      this.lblJiggleTitle.BackColor = System.Drawing.Color.Transparent;
      this.lblJiggleTitle.Font      = new System.Drawing.Font ("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
      this.lblJiggleTitle.ForeColor = System.Drawing.Color.FromArgb (196, 199, 200);
      this.lblJiggleTitle.Location  = new System.Drawing.Point (24, 30);
      this.lblJiggleTitle.Name      = "lblJiggleTitle";
      this.lblJiggleTitle.Size      = new System.Drawing.Size (200, 24);
      this.lblJiggleTitle.Text      = "JIGGLING";

      this.cbJiggling.Location       = new System.Drawing.Point (332, 28);
      this.cbJiggling.Name           = "cbJiggling";
      this.cbJiggling.Size           = new System.Drawing.Size (56, 28);
      this.cbJiggling.TabIndex       = 0;
      this.cbJiggling.CheckedChanged += this.cbJiggling_CheckedChanged;

      // -- panelActions -------------------------------------------------------
      this.panelActions.BackColor = System.Drawing.Color.Transparent;
      this.panelActions.Controls.Add (this.btnAbout);
      this.panelActions.Controls.Add (this.btnTray);
      this.panelActions.Margin    = new System.Windows.Forms.Padding (0, 0, 0, 16);
      this.panelActions.Name      = "panelActions";
      this.panelActions.Size      = new System.Drawing.Size (412, 50);
      this.panelActions.TabIndex  = 2;
      this.panelActions.Paint    += this.CustomPanel_Paint;

      this.btnAbout.BackColor                         = System.Drawing.Color.White;
      this.btnAbout.FlatAppearance.BorderColor        = System.Drawing.Color.FromArgb (26, 255, 255, 255);
      this.btnAbout.FlatAppearance.BorderSize         = 1;
      this.btnAbout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb (226, 226, 226);
      this.btnAbout.FlatAppearance.MouseDownBackColor  = System.Drawing.Color.FromArgb (198, 198, 198);
      this.btnAbout.FlatStyle                         = System.Windows.Forms.FlatStyle.Flat;
      this.btnAbout.Font                              = new System.Drawing.Font ("Consolas", 8.5F, System.Drawing.FontStyle.Bold);
      this.btnAbout.ForeColor                         = System.Drawing.Color.Black;
      this.btnAbout.Location                          = new System.Drawing.Point (6, 5);
      this.btnAbout.Name                              = "btnAbout";
      this.btnAbout.Size                              = new System.Drawing.Size (197, 40);
      this.btnAbout.TabIndex                          = 1;
      this.btnAbout.Text                              = "About";
      this.btnAbout.UseVisualStyleBackColor           = false;
      this.btnAbout.Click                            += new System.EventHandler(this.btnAbout_Click);

      this.btnTray.BackColor                         = System.Drawing.Color.FromArgb (28, 28, 30);
      this.btnTray.FlatAppearance.BorderColor        = System.Drawing.Color.FromArgb (26, 255, 255, 255);
      this.btnTray.FlatAppearance.BorderSize         = 1;
      this.btnTray.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb (15, 255, 255, 255);
      this.btnTray.FlatAppearance.MouseDownBackColor  = System.Drawing.Color.FromArgb (25, 255, 255, 255);
      this.btnTray.FlatStyle                         = System.Windows.Forms.FlatStyle.Flat;
      this.btnTray.Font                              = new System.Drawing.Font ("Consolas", 8.5F, System.Drawing.FontStyle.Bold);
      this.btnTray.ForeColor                         = System.Drawing.Color.White;
      this.btnTray.Location                          = new System.Drawing.Point (209, 5);
      this.btnTray.Name                              = "btnTray";
      this.btnTray.Size                              = new System.Drawing.Size (197, 40);
      this.btnTray.TabIndex                          = 2;
      this.btnTray.Text                              = "Minimize";
      this.btnTray.UseVisualStyleBackColor           = false;
      this.btnTray.Click                            += new System.EventHandler(this.btnTray_Click);

      // -- panelSettings ------------------------------------------------------
      this.panelSettings.BackColor = System.Drawing.Color.Transparent;
      this.panelSettings.Controls.Add (this.lblModeTitle);
      this.panelSettings.Controls.Add (this.cmbJiggleMode);
      this.panelSettings.Controls.Add (this.lblRandomTitle);
      this.panelSettings.Controls.Add (this.cbRandom);
      this.panelSettings.Controls.Add (this.lblPeriodLabel);
      this.panelSettings.Controls.Add (this.nudPeriod);
      this.panelSettings.Controls.Add (this.lbPeriod);
      this.panelSettings.Controls.Add (this.lblDistanceLabel);
      this.panelSettings.Controls.Add (this.nudDistance);
      this.panelSettings.Controls.Add (this.lblMinimizeTitle);
      this.panelSettings.Controls.Add (this.cbMinimize);
      this.panelSettings.Margin    = new System.Windows.Forms.Padding (0);
      this.panelSettings.Name      = "panelSettings";
      this.panelSettings.Size      = new System.Drawing.Size (412, 228);
      this.panelSettings.TabIndex  = 3;
      this.panelSettings.Visible   = true;
      this.panelSettings.Paint    += this.CustomPanel_Paint;

      // row 1 -- jiggle mode (y = 16)
      this.lblModeTitle.AutoSize  = false;
      this.lblModeTitle.BackColor = System.Drawing.Color.Transparent;
      this.lblModeTitle.Font      = new System.Drawing.Font ("Segoe UI", 10F);
      this.lblModeTitle.ForeColor = System.Drawing.Color.FromArgb (196, 199, 200);
      this.lblModeTitle.Location  = new System.Drawing.Point (24, 16);
      this.lblModeTitle.Name      = "lblModeTitle";
      this.lblModeTitle.Size      = new System.Drawing.Size (160, 22);
      this.lblModeTitle.Text      = "JIGGLE MODE";

      this.cmbJiggleMode.BackColor         = System.Drawing.Color.FromArgb (26, 26, 29);
      this.cmbJiggleMode.DrawMode          = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.cmbJiggleMode.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbJiggleMode.FlatStyle         = System.Windows.Forms.FlatStyle.Flat;
      this.cmbJiggleMode.ForeColor         = System.Drawing.Color.White;
      this.cmbJiggleMode.FormattingEnabled = true;
      this.cmbJiggleMode.Location          = new System.Drawing.Point (242, 13);
      this.cmbJiggleMode.Name              = "cmbJiggleMode";
      this.cmbJiggleMode.Size              = new System.Drawing.Size (146, 26);
      this.cmbJiggleMode.TabIndex          = 0;
      this.cmbJiggleMode.SelectedIndexChanged += this.cmbJiggleMode_SelectedIndexChanged;
      this.cmbJiggleMode.DrawItem          += this.cmbJiggleMode_DrawItem;

      // row 2 -- random timer (y = 60)
      this.lblRandomTitle.AutoSize  = false;
      this.lblRandomTitle.BackColor = System.Drawing.Color.Transparent;
      this.lblRandomTitle.Font      = new System.Drawing.Font ("Segoe UI", 10F);
      this.lblRandomTitle.ForeColor = System.Drawing.Color.FromArgb (196, 199, 200);
      this.lblRandomTitle.Location  = new System.Drawing.Point (24, 58);
      this.lblRandomTitle.Name      = "lblRandomTitle";
      this.lblRandomTitle.Size      = new System.Drawing.Size (200, 22);
      this.lblRandomTitle.Text      = "RANDOM TIMER";

      this.cbRandom.Location       = new System.Drawing.Point (332, 56);
      this.cbRandom.Name           = "cbRandom";
      this.cbRandom.Size           = new System.Drawing.Size (56, 28);
      this.cbRandom.TabIndex       = 1;
      this.cbRandom.CheckedChanged += this.cbRandom_CheckedChanged;

      // row 3 -- interval (y = 104)
      this.lblPeriodLabel.AutoSize  = false;
      this.lblPeriodLabel.BackColor = System.Drawing.Color.Transparent;
      this.lblPeriodLabel.Font      = new System.Drawing.Font ("Segoe UI", 10F);
      this.lblPeriodLabel.ForeColor = System.Drawing.Color.FromArgb (196, 199, 200);
      this.lblPeriodLabel.Location  = new System.Drawing.Point (24, 102);
      this.lblPeriodLabel.Name      = "lblPeriodLabel";
      this.lblPeriodLabel.Size      = new System.Drawing.Size (160, 22);
      this.lblPeriodLabel.Text      = "INTERVAL (s)";

      this.nudPeriod.BackColor   = System.Drawing.Color.FromArgb (26, 26, 29);
      this.nudPeriod.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.nudPeriod.ForeColor   = System.Drawing.Color.White;
      this.nudPeriod.Location    = new System.Drawing.Point (242, 99);
      this.nudPeriod.Maximum     = new decimal (new int[] { 10800, 0, 0, 0 });
      this.nudPeriod.Minimum     = new decimal (new int[] { 1, 0, 0, 0 });
      this.nudPeriod.Name        = "nudPeriod";
      this.nudPeriod.Size        = new System.Drawing.Size (100, 21);
      this.nudPeriod.TabIndex    = 4;
      this.nudPeriod.TextAlign   = System.Windows.Forms.HorizontalAlignment.Right;
      this.nudPeriod.Value       = new decimal (new int[] { 1, 0, 0, 0 });
      this.nudPeriod.ValueChanged += this.nudPeriod_ValueChanged;

      // lbPeriod -- shows current effective interval (updated by random mode)
      this.lbPeriod.AutoSize  = false;
      this.lbPeriod.BackColor = System.Drawing.Color.Transparent;
      this.lbPeriod.Font      = new System.Drawing.Font ("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
      this.lbPeriod.ForeColor = System.Drawing.Color.White;
      this.lbPeriod.Location  = new System.Drawing.Point (348, 103);
      this.lbPeriod.Name      = "lbPeriod";
      this.lbPeriod.Size      = new System.Drawing.Size (48, 18);
      this.lbPeriod.Text      = "1 s";
      this.lbPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

      // row 4 -- distance (y = 148)
      this.lblDistanceLabel.AutoSize  = false;
      this.lblDistanceLabel.BackColor = System.Drawing.Color.Transparent;
      this.lblDistanceLabel.Font      = new System.Drawing.Font ("Segoe UI", 10F);
      this.lblDistanceLabel.ForeColor = System.Drawing.Color.FromArgb (196, 199, 200);
      this.lblDistanceLabel.Location  = new System.Drawing.Point (24, 146);
      this.lblDistanceLabel.Name      = "lblDistanceLabel";
      this.lblDistanceLabel.Size      = new System.Drawing.Size (200, 22);
      this.lblDistanceLabel.Text      = "MOVE DISTANCE";

      this.nudDistance.BackColor   = System.Drawing.Color.FromArgb (26, 26, 29);
      this.nudDistance.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.nudDistance.ForeColor   = System.Drawing.Color.White;
      this.nudDistance.Location    = new System.Drawing.Point (242, 143);
      this.nudDistance.Maximum     = new decimal (new int[] { 120, 0, 0, 0 });
      this.nudDistance.Minimum     = new decimal (new int[] { 1, 0, 0, 0 });
      this.nudDistance.Name        = "nudDistance";
      this.nudDistance.Size        = new System.Drawing.Size (100, 21);
      this.nudDistance.TabIndex    = 6;
      this.nudDistance.TextAlign   = System.Windows.Forms.HorizontalAlignment.Right;
      this.nudDistance.Value       = new decimal (new int[] { 1, 0, 0, 0 });
      this.nudDistance.ValueChanged += this.nudDistance_ValueChanged;

      // row 5 -- minimize on start (y = 196)
      this.lblMinimizeTitle.AutoSize  = false;
      this.lblMinimizeTitle.BackColor = System.Drawing.Color.Transparent;
      this.lblMinimizeTitle.Font      = new System.Drawing.Font ("Segoe UI", 10F);
      this.lblMinimizeTitle.ForeColor = System.Drawing.Color.FromArgb (196, 199, 200);
      this.lblMinimizeTitle.Location  = new System.Drawing.Point (24, 190);
      this.lblMinimizeTitle.Name      = "lblMinimizeTitle";
      this.lblMinimizeTitle.Size      = new System.Drawing.Size (240, 22);
      this.lblMinimizeTitle.Text      = "MINIMIZE ON START";

      this.cbMinimize.Location       = new System.Drawing.Point (332, 188);
      this.cbMinimize.Name           = "cbMinimize";
      this.cbMinimize.Size           = new System.Drawing.Size (56, 28);
      this.cbMinimize.TabIndex       = 7;
      this.cbMinimize.CheckedChanged += this.cbMinimize_CheckedChanged;

      // -- MainForm -----------------------------------------------------------
      this.AutoScaleDimensions = new System.Drawing.SizeF (7F, 15F);
      this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor           = System.Drawing.Color.Black;
      this.ClientSize          = new System.Drawing.Size (460, 540);
      this.Controls.Add (this.flpLayout);
      this.Font                = new System.Drawing.Font ("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular);
      this.FormBorderStyle     = System.Windows.Forms.FormBorderStyle.None;
      this.Margin              = new System.Windows.Forms.Padding (0);
      this.MaximizeBox         = false;
      this.MinimizeBox         = false;
      this.Name                = "MainForm";
      this.Text                = "NEOghost";
      this.Load               += this.MainForm_Load;
      this.Shown              += this.MainForm_Shown;

      this.flpLayout.ResumeLayout (false);
      this.flpLayout.PerformLayout ();
      this.panelHeader.ResumeLayout (false);
      this.panelBase.ResumeLayout (false);
      this.panelActions.ResumeLayout (false);
      this.panelSettings.ResumeLayout (false);
      this.panelSettings.PerformLayout ();
      ((System.ComponentModel.ISupportInitialize) this.nudPeriod).EndInit ();
      ((System.ComponentModel.ISupportInitialize) this.nudDistance).EndInit ();
      this.trayMenu.ResumeLayout (false);
      this.ResumeLayout (false);
      this.PerformLayout ();
    }

    // -- Field declarations ---------------------------------------------------
    private System.Windows.Forms.Timer                        jiggleTimer;
    private System.Windows.Forms.FlowLayoutPanel             flpLayout;

    private System.Windows.Forms.Panel                       panelHeader;
    private NeoGhost.Controls.GlowingOrbPanel                    lblLogo;
    private System.Windows.Forms.Label                       lblAppName;
    private System.Windows.Forms.Label                       lblSubtitle;
    private System.Windows.Forms.Button                      btnClose;
    private System.Windows.Forms.Button                      btnMinimize;
    private System.Windows.Forms.Panel                       pnlIndicator;
    private System.Windows.Forms.Label                       lblStatusText;

    private System.Windows.Forms.Panel                       panelBase;
    private System.Windows.Forms.Label                       lblJiggleTitle;
    private NeoGhost.Controls.ToggleSwitch cbJiggling;

    private System.Windows.Forms.Panel                       panelActions;
    private System.Windows.Forms.Button                      btnAbout;
    private System.Windows.Forms.Button                      btnTray;

    private System.Windows.Forms.Panel                       panelSettings;
    private System.Windows.Forms.Panel                       pnlSeparator;
    private System.Windows.Forms.Label                       lblModeTitle;
    private System.Windows.Forms.ComboBox                    cmbJiggleMode;
    private System.Windows.Forms.Label                       lblRandomTitle;
    private NeoGhost.Controls.ToggleSwitch cbRandom;
    private System.Windows.Forms.Label                       lblPeriodLabel;
    private System.Windows.Forms.NumericUpDown               nudPeriod;
    private System.Windows.Forms.Label                       lbPeriod;
    private System.Windows.Forms.Label                       lblDistanceLabel;
    private System.Windows.Forms.NumericUpDown               nudDistance;
    private System.Windows.Forms.Label                       lblMinimizeTitle;
    private NeoGhost.Controls.ToggleSwitch cbMinimize;

    private System.Windows.Forms.NotifyIcon                  niTray;
    private System.Windows.Forms.ContextMenuStrip            trayMenu;
    private System.Windows.Forms.ToolStripMenuItem           tsmiOpen;
    private System.Windows.Forms.ToolStripMenuItem           tsmiStartJiggling;
    private System.Windows.Forms.ToolStripMenuItem           tsmiStopJiggling;
    private System.Windows.Forms.ToolStripMenuItem           tsmiExit;
  }
}
