// <copyright file="MonitorInfo.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Drawing;
using ScreenCaptureKit;

namespace Drastic.ScreenCapture
{
    public class MonitorInfo : IMonitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorInfo"/> class.
        /// </summary>
        /// <param name="display">The Mac Display.</param>
        internal MonitorInfo(SCDisplay display)
        {
            this.ScreenSize = new SizeF(display.Width, display.Height);
            this.MonitorArea = new Rectangle(0, 0, (int)display.Width, (int)display.Height);
            this.WorkArea = new Rectangle(0, 0, (int)display.Width, (int)display.Height);
            this.DeviceName = display.DisplayId.ToString();
            this.Display = display;

            // TODO: Is that right?
            this.IsPrimary = display.DisplayId == 1;
        }

        /// <inheritdoc/>
        public bool IsPrimary { get; }

        /// <inheritdoc/>
        public SizeF ScreenSize { get; }

        /// <inheritdoc/>
        public Rectangle MonitorArea { get; }

        /// <inheritdoc/>
        public Rectangle WorkArea { get; }

        /// <inheritdoc/>
        public string DeviceName { get; }

        /// <summary>
        /// Gets the Mac Display.
        /// </summary>
        public SCDisplay Display { get; }

        /// <inheritdoc/>
        public object RawHandler => this.Display;
    }
}
