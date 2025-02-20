﻿using System;

namespace RaceElement.HUD.Overlay.Configuration
{
    public class ToolTipAttribute : Attribute
    {
        public string ToolTip { get; private set; } = string.Empty;
        public ToolTipAttribute(string toolTip)
        {
            this.ToolTip = toolTip;
        }
    }
}
