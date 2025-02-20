﻿using RaceElement.Util.SystemExtensions;
using RaceElement.HUD.Overlay.Internal;
using RaceElement.HUD.Overlay.OverlayUtil;
using RaceElement.HUD.Overlay.Util;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Collections.Generic;
using System.Linq;

namespace RaceElement.HUD.ACC.Overlays.OverlayShiftIndicator
{
    [Overlay(Name = "Shift Indicator",
        Description = "A bar showing the current RPM, optionally showing when the pit limiter is enabled.",
        Version = 1.00,
        OverlayType = OverlayType.Release,
        OverlayCategory = OverlayCategory.Driving)]
    internal sealed class ShiftIndicatorOverlay : AbstractOverlay
    {
        private readonly ShiftIndicatorConfiguration _config = new ShiftIndicatorConfiguration();

        private string _lastCar = string.Empty;
        private CachedBitmap _cachedBackground;
        private CachedBitmap _cachedRpmLines;
        private CachedBitmap _cachedPitLimiterOutline;

        private Font _font;
        private float _halfRpmStringWidth = -1;

        private List<(float, Color)> _colors;
        private List<CachedBitmap> _cachedColorBars;

        public ShiftIndicatorOverlay(Rectangle rectangle) : base(rectangle, "Shift Indicator")
        {
            this.RefreshRateHz = this._config.Bar.RefreshRate;
            this.Height = _config.Bar.Height + 1;
            this.Width = _config.Bar.Width + 1;
        }

        public sealed override void BeforeStart()
        {
            if (_config.Bar.ShowRpmText || _config.Bar.ShowPitLimiter)
            {
                float height = _config.Bar.Height - 14;
                height.Clip(11, 22);
                _font = FontUtil.FontSegoeMono(height);
            }
            int cornerRadius = (int)(10 * this.Scale);

            _colors = new List<(float, Color)>
                {
                    (0.7f, Color.FromArgb(_config.Colors.NormalOpacity, _config.Colors.NormalColor)),
                    (0.94f, Color.FromArgb(_config.Colors.EarlyOpacity, _config.Colors.EarlyColor)),
                    (0.973f, Color.FromArgb(_config.Colors.UpshiftOpacity, _config.Colors.UpshiftColor))
                };

            _cachedColorBars = new List<CachedBitmap>();
            foreach (var color in _colors.Select(x => x.Item2))
            {
                _cachedColorBars.Add(new CachedBitmap((int)(_config.Bar.Width * this.Scale + 1), (int)(_config.Bar.Height * this.Scale + 1), g =>
                {
                    Rectangle rect = new Rectangle(0, 1, (int)(_config.Bar.Width * this.Scale), (int)(_config.Bar.Height * this.Scale));
                    HatchBrush hatchBrush = new HatchBrush(HatchStyle.LightDownwardDiagonal, color, Color.FromArgb(color.A - 50, color));
                    g.FillRoundedRectangle(hatchBrush, rect, cornerRadius);
                }));
            }

            _cachedBackground = new CachedBitmap((int)(_config.Bar.Width * this.Scale + 1), (int)(_config.Bar.Height * this.Scale + 1), g =>
            {
                int midHeight = (int)(_config.Bar.Height * this.Scale) / 2;
                var linerBrush = new LinearGradientBrush(new Point(0, midHeight), new Point((int)(_config.Bar.Width * this.Scale), midHeight), Color.FromArgb(160, 0, 0, 0), Color.FromArgb(230, 0, 0, 0));
                g.FillRoundedRectangle(linerBrush, new Rectangle(0, 0, (int)(_config.Bar.Width * this.Scale), (int)(_config.Bar.Height * this.Scale)), cornerRadius);
                g.DrawRoundedRectangle(new Pen(Color.Black, 1 * this.Scale), new Rectangle(0, 0, (int)(_config.Bar.Width * this.Scale), (int)(_config.Bar.Height * this.Scale)), cornerRadius);
            });

            _cachedRpmLines = new CachedBitmap((int)(_config.Bar.Width * this.Scale + 1), (int)(_config.Bar.Height * this.Scale + 1), g =>
            {
                int lineCount = (int)Math.Floor((pageStatic.MaxRpm - _config.Bar.HideRpm) / 1000d);

                int leftOver = (pageStatic.MaxRpm - _config.Bar.HideRpm) % 1000;
                if (leftOver < 70)
                    lineCount--;

                lineCount.ClipMin(0);

                Pen linePen = new Pen(new SolidBrush(Color.FromArgb(220, Color.Black)), 1.5f * this.Scale);

                double thousandPercent = (1000d / (pageStatic.MaxRpm - _config.Bar.HideRpm)) * lineCount;
                double baseX = (_config.Bar.Width * this.Scale) / lineCount * thousandPercent;
                for (int i = 1; i <= lineCount; i++)
                {
                    int x = (int)(i * baseX);
                    g.DrawLine(linePen, x, 1, x, (_config.Bar.Height * this.Scale) - 1);
                }
            });

            if (_config.Bar.ShowPitLimiter)
                _cachedPitLimiterOutline = new CachedBitmap((int)(_config.Bar.Width * this.Scale + 1), (int)(_config.Bar.Height * this.Scale + 1), g =>
                {
                    g.DrawRoundedRectangle(new Pen(Color.Yellow, 2.5f), new Rectangle(0, 0, (int)(_config.Bar.Width * this.Scale), (int)(_config.Bar.Height * this.Scale)), cornerRadius);
                });
        }

        public sealed override void BeforeStop()
        {
            _cachedBackground?.Dispose();
            _cachedRpmLines?.Dispose();

            _cachedPitLimiterOutline?.Dispose();
            if (_cachedColorBars != null)
                foreach (CachedBitmap cachedBitmap in _cachedColorBars)
                    cachedBitmap?.Dispose();
        }

        public sealed override void Render(Graphics g)
        {
            _cachedBackground?.Draw(g, _config.Bar.Width, _config.Bar.Height);

            if (_config.Bar.ShowPitLimiter && pagePhysics.PitLimiterOn)
                DrawPitLimiterBar(g);

            DrawRpmBar(g);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.TextContrast = 1;

            if (_config.Bar.ShowRpmText)
                DrawRpmText(g);
        }

        private int _limiterColorSwitch = 0;
        private void DrawPitLimiterBar(Graphics g)
        {
            if (_limiterColorSwitch > this.RefreshRateHz / 5)
                _cachedPitLimiterOutline?.Draw(g, 0, 0, _config.Bar.Width, _config.Bar.Height);

            if (_limiterColorSwitch > this.RefreshRateHz / 2)
                _limiterColorSwitch = 0;

            _limiterColorSwitch++;
        }

        private void DrawRpmText(Graphics g)
        {
            string currentRpm = $"{pagePhysics.Rpms}".FillStart(4, '0');

            if (_halfRpmStringWidth < 0)
                _halfRpmStringWidth = g.MeasureString("9999", _font).Width / 2;

            int x = (int)((_halfRpmStringWidth + 8));
            int y = (int)(_config.Bar.Height / 2 - _font.Height / 2.05);
            DrawTextWithOutline(g, Color.White, currentRpm, x, y);
        }

        private void DrawTextWithOutline(Graphics g, Color textColor, string text, int x, int y)
        {
            Rectangle backgroundDimension = new Rectangle((int)(x - _halfRpmStringWidth), y, (int)(_halfRpmStringWidth * 2), _font.Height);
            g.FillRoundedRectangle(new SolidBrush(Color.FromArgb(185, 0, 0, 0)), backgroundDimension, 2);
            g.DrawStringWithShadow(text, _font, textColor, new PointF(x - _halfRpmStringWidth, y + _font.Height / 14f), 1.5f * this.Scale);
        }

        private void DrawRpmBar(Graphics g)
        {
            double maxRpm = pageStatic.MaxRpm;
            double currentRpm = pagePhysics.Rpms;
            double percent = 0;

            if (maxRpm > 0 && currentRpm > 0)
                percent = currentRpm / maxRpm;

            if (percent > 0)
            {
                int index = 0;
                foreach ((float, Color) colorRange in _colors)
                    if (percent > colorRange.Item1)
                        index = _colors.IndexOf(colorRange);

                double adjustedPercent = (currentRpm - _config.Bar.HideRpm) / (maxRpm - _config.Bar.HideRpm);
                var barDrawWidth = (int)(_config.Bar.Width * adjustedPercent);

                g.SetClip(new Rectangle(0, 0, barDrawWidth, _config.Bar.Height));
                _cachedColorBars[index].Draw(g, 1, 0, _config.Bar.Width - 1, _config.Bar.Height - 1);

                g.SetClip(new Rectangle(0, 0, _config.Bar.Width, _config.Bar.Height));
            }

            DrawRpmBar1kLines(g);
        }

        private void DrawRpmBar1kLines(Graphics g)
        {
            if (_lastCar != pageStatic.CarModel)
            {
                _cachedRpmLines.Render();
                _lastCar = pageStatic.CarModel;
            }

            _cachedRpmLines.Draw(g, _config.Bar.Width, _config.Bar.Height);
        }
    }
}