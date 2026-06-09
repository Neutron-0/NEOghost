using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NeoGhost.Controls;

/// <summary>
/// Multi-layered vector orb with an internal particle field simulation.
/// Mirrors the neoghost.vercel.app cosmic asset: purple/cyan aura, fluid core matter, glass highlight.
/// </summary>
internal sealed class GlowingOrbPanel : Panel
{
    private struct OrbParticle
    {
        public float X;
        public float Y;
        public float SpeedX;
        public float SpeedY;
        public float Alpha;
        public float Scale;
    }

    private const int ParticleCount = 30;

    private static readonly Color PurpleAura = Color.FromArgb(168, 85, 247);
    private static readonly Color CyanAura = Color.FromArgb(0, 212, 255);
    private static readonly Color CoreCenterColor = Color.FromArgb(220, 240, 255);
    private static readonly Color GlassHighlight = Color.FromArgb(102, 255, 255, 255);

    private OrbParticle[] _particles = Array.Empty<OrbParticle>();
    private System.Windows.Forms.Timer? _animationTimer;
    private float _phase;
    private readonly Random _random = new();
    private float _centerX;
    private float _centerY;
    private float _coreRadius;

    public GlowingOrbPanel()
    {
        SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.UserPaint |
            ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.ResizeRedraw |
            ControlStyles.SupportsTransparentBackColor, true);

        DoubleBuffered = true;
        BackColor = Color.Transparent;
        Size = new Size(44, 44);

        InitializeParticles();
        StartAnimationTimer();
    }

    private void StartAnimationTimer()
    {
        _animationTimer = new System.Windows.Forms.Timer { Interval = 16 };
        _animationTimer.Tick += (_, _) =>
        {
            UpdateParticles();
            Invalidate(false);
        };
        _animationTimer.Start();
    }

    private void InitializeParticles()
    {
        _centerX = Width / 2f;
        _centerY = Height / 2f;
        _coreRadius = Math.Min(Width, Height) / 2f * 0.42f;

        if (_particles.Length != ParticleCount)
            _particles = new OrbParticle[ParticleCount];

        for (int i = 0; i < ParticleCount; i++)
        {
            float angle = (float)(_random.NextDouble() * Math.PI * 2.0);
            float dist = (float)(_random.NextDouble() * _coreRadius * 0.82f);
            _particles[i] = new OrbParticle
            {
                X = _centerX + MathF.Cos(angle) * dist,
                Y = _centerY + MathF.Sin(angle) * dist,
                SpeedX = (float)((_random.NextDouble() - 0.5) * 0.55),
                SpeedY = (float)((_random.NextDouble() - 0.5) * 0.55),
                Alpha = 0.35f + (float)_random.NextDouble() * 0.45f,
                Scale = 0.55f + (float)_random.NextDouble() * 0.85f
            };
        }
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        InitializeParticles();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        RenderOrb(e.Graphics, Width, Height, _particles, _phase, _centerX, _centerY, _coreRadius);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && _animationTimer != null)
        {
            _animationTimer.Stop();
            _animationTimer.Dispose();
            _animationTimer = null;
        }

        base.Dispose(disposing);
    }

    private void UpdateParticles()
    {
        _phase += 0.045f;
        float boundRadius = _coreRadius * 0.9f;

        for (int i = 0; i < _particles.Length; i++)
        {
            ref OrbParticle p = ref _particles[i];
            p.X += p.SpeedX;
            p.Y += p.SpeedY;

            float dx = p.X - _centerX;
            float dy = p.Y - _centerY;
            float distSq = dx * dx + dy * dy;
            float boundSq = boundRadius * boundRadius;

            if (distSq > boundSq)
            {
                float dist = MathF.Sqrt(distSq);
                float nx = dx / dist;
                float ny = dy / dist;
                p.X = _centerX - nx * boundRadius * 0.72f;
                p.Y = _centerY - ny * boundRadius * 0.72f;
                p.SpeedX = -p.SpeedX * 0.92f + (float)((_random.NextDouble() - 0.5) * 0.08);
                p.SpeedY = -p.SpeedY * 0.92f + (float)((_random.NextDouble() - 0.5) * 0.08);
            }

            p.Alpha = 0.28f + 0.42f * (0.5f + 0.5f * MathF.Sin(_phase + i * 0.63f));
        }
    }

    /// <summary>
    /// Renders a deterministic orb snapshot for tray/form icon bitmaps.
    /// </summary>
    public static void DrawOrbToGraphics(Graphics g, int width, int height)
    {
        var particles = new OrbParticle[ParticleCount];
        var rng = new Random(0x4E474F);
        float cx = width / 2f;
        float cy = height / 2f;
        float coreRadius = Math.Min(width, height) / 2f * 0.42f;

        for (int i = 0; i < ParticleCount; i++)
        {
            float angle = (float)(rng.NextDouble() * Math.PI * 2.0);
            float dist = (float)(rng.NextDouble() * coreRadius * 0.82f);
            particles[i] = new OrbParticle
            {
                X = cx + MathF.Cos(angle) * dist,
                Y = cy + MathF.Sin(angle) * dist,
                SpeedX = 0f,
                SpeedY = 0f,
                Alpha = 0.35f + (float)rng.NextDouble() * 0.4f,
                Scale = 0.55f + (float)rng.NextDouble() * 0.85f
            };
        }

        RenderOrb(g, width, height, particles, 1.7f, cx, cy, coreRadius);
    }

    private static void RenderOrb(
        Graphics g, int width, int height,
        ReadOnlySpan<OrbParticle> particles, float phase,
        float cx, float cy, float coreRadius)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        g.CompositingQuality = CompositingQuality.HighQuality;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        float outerRadius = Math.Min(width, height) / 2f;

        // ── Layer 1: Cosmic Background Aura ────────────────────────────────
        DrawAuraLayer(g, cx, cy, outerRadius * 1.65f, Color.FromArgb(48, PurpleAura), Color.Transparent);
        DrawAuraLayer(g, cx, cy, outerRadius * 1.35f, Color.FromArgb(38, PurpleAura), Color.Transparent);
        DrawAuraLayer(g, cx, cy, outerRadius * 1.12f, Color.FromArgb(77, CyanAura), Color.Transparent);
        DrawAuraLayer(g, cx, cy, outerRadius * 0.95f, Color.FromArgb(32, CyanAura), Color.Transparent);

        // ── Layer 2: Fluid Core Matter ─────────────────────────────────────
        using (var coreClip = new GraphicsPath())
        {
            coreClip.AddEllipse(cx - coreRadius, cy - coreRadius, coreRadius * 2f, coreRadius * 2f);
            g.SetClip(coreClip);

            using (var baseCorePath = new GraphicsPath())
            {
                baseCorePath.AddEllipse(cx - coreRadius, cy - coreRadius, coreRadius * 2f, coreRadius * 2f);
                using var baseCoreBrush = new PathGradientBrush(baseCorePath)
                {
                    CenterColor = Color.FromArgb(200, CoreCenterColor),
                    SurroundColors = new[] { Color.FromArgb(90, CyanAura) },
                    CenterPoint = new PointF(cx - coreRadius * 0.1f, cy - coreRadius * 0.12f),
                    FocusScales = new PointF(0.42f, 0.42f)
                };
                g.FillPath(baseCoreBrush, baseCorePath);
            }

            for (int i = 0; i < particles.Length; i++)
            {
                OrbParticle p = particles[i];
                float radius = coreRadius * 0.11f * p.Scale;
                float pulse = 0.85f + 0.15f * MathF.Sin(phase + i * 0.81f);
                int alpha = (int)(255f * p.Alpha * pulse);
                if (alpha < 8)
                    continue;

                using var particlePath = new GraphicsPath();
                particlePath.AddEllipse(p.X - radius, p.Y - radius, radius * 2f, radius * 2f);

                using var particleBrush = new PathGradientBrush(particlePath)
                {
                    CenterColor = Color.FromArgb(alpha, CoreCenterColor),
                    SurroundColors = new[] { Color.FromArgb(alpha / 4, CyanAura) },
                    CenterPoint = new PointF(p.X, p.Y),
                    FocusScales = new PointF(0.55f, 0.55f)
                };
                g.FillPath(particleBrush, particlePath);
            }

            g.ResetClip();
        }

        // ── Layer 3: Optical Glass Layer ─────────────────────────────────────
        float specW = coreRadius * 0.82f;
        float specH = coreRadius * 0.5f;
        float specX = cx - coreRadius * 0.42f;
        float specY = cy - coreRadius * 0.62f;

        using (var glassPath = new GraphicsPath())
        {
            glassPath.AddEllipse(specX, specY, specW, specH);
            using var glassBrush = new PathGradientBrush(glassPath)
            {
                CenterColor = GlassHighlight,
                SurroundColors = new[] { Color.FromArgb(18, 255, 255, 255), Color.Transparent },
                CenterPoint = new PointF(specX + specW * 0.38f, specY + specH * 0.26f),
                FocusScales = new PointF(0.48f, 0.38f)
            };
            g.FillPath(glassBrush, glassPath);
        }

        using (var rimPath = new GraphicsPath())
        {
            float rimW = specW * 0.55f;
            float rimH = specH * 0.32f;
            rimPath.AddEllipse(specX + specW * 0.08f, specY + specH * 0.12f, rimW, rimH);
            using var rimBrush = new PathGradientBrush(rimPath)
            {
                CenterColor = Color.FromArgb(64, 255, 255, 255),
                SurroundColors = new[] { Color.Transparent },
                CenterPoint = new PointF(specX + specW * 0.35f, specY + specH * 0.22f),
                FocusScales = new PointF(0.6f, 0.5f)
            };
            g.FillPath(rimBrush, rimPath);
        }
    }

    private static void DrawAuraLayer(Graphics g, float cx, float cy, float radius, Color centerColor, Color surroundColor)
    {
        using var path = new GraphicsPath();
        path.AddEllipse(cx - radius, cy - radius, radius * 2f, radius * 2f);

        using var brush = new PathGradientBrush(path)
        {
            CenterColor = centerColor,
            SurroundColors = new[] { surroundColor },
            CenterPoint = new PointF(cx, cy)
        };
        g.FillPath(brush, path);
    }
}
