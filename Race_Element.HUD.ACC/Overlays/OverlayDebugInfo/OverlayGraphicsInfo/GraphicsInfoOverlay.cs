﻿using RaceElement.HUD.ACC.Overlays.OverlayDebugInfo;
using RaceElement.HUD.Overlay.Internal;
using RaceElement.HUD.Overlay.OverlayUtil;
using RaceElement.Util;
using System;
using System.Drawing;
using System.Reflection;
using static RaceElement.HUD.ACC.Overlays.OverlayDebugInfo.DebugInfoHelper;

namespace RaceElement.HUD.ACC.Overlays.OverlayGraphicsInfo
{
    [Overlay(Name = "Graphics Info", Version = 1.00,
        Description = "Shared Memory Graphics Page", OverlayType = OverlayType.Debug)]
    internal sealed class GraphicsInfoOverlay : AbstractOverlay
    {
        private readonly DebugConfig _config = new DebugConfig();
        private readonly InfoTable _table;

        public GraphicsInfoOverlay(Rectangle rectangle) : base(rectangle, "Graphics Info")
        {
            this.AllowReposition = false;
            this.RefreshRateHz = 5;
            this.Width = 275;
            this.Height = 1105;

            _table = new InfoTable(9, new int[] { 250 });
        }

        private void Instance_WidthChanged(object sender, bool e)
        {
            if (e)
                this.X = DebugInfoHelper.Instance.GetX(this);
        }

        public sealed override void BeforeStart()
        {
            if (this._config.Dock.Undock)
                this.AllowReposition = true;
            else
            {
                DebugInfoHelper.Instance.WidthChanged += Instance_WidthChanged;
                DebugInfoHelper.Instance.AddOverlay(this);
                this.X = DebugInfoHelper.Instance.GetX(this);
                this.Y = 0;
            }
        }

        public sealed override void BeforeStop()
        {
            if (!this._config.Dock.Undock)
            {
                DebugInfoHelper.Instance.RemoveOverlay(this);
                DebugInfoHelper.Instance.WidthChanged -= Instance_WidthChanged;
            }
        }

        public sealed override void Render(Graphics g)
        {
            FieldInfo[] members = pageGraphics.GetType().GetFields();
            foreach (FieldInfo member in members)
            {
                var value = member.GetValue(pageGraphics);
                bool isObsolete = false;
                foreach (CustomAttributeData cad in member.CustomAttributes)
                {
                    if (cad.AttributeType == typeof(ObsoleteAttribute)) { isObsolete = true; break; }
                }

                if (!isObsolete && !member.Name.Equals("Buffer") && !member.Name.Equals("Size"))
                {
                    value = ReflectionUtil.FieldTypeValue(member, value);
                    _table.AddRow($"{member.Name}", new string[] { value.ToString() });
                }
            }

            _table.Draw(g);
        }

        public sealed override bool ShouldRender() => true;
    }
}
