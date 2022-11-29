// <copyright file="MonitorInfo.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Drawing;
using static Drastic.ScreenCapture.MonitorEnumeration;

namespace Drastic.ScreenCapture
{
    /// <summary>
    /// Windows Monitor Info.
    /// </summary>
    public class MonitorInfo : IMonitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorInfo"/> class.
        /// </summary>
        /// <param name="hMonitor">The IntPtr pointing to the monitor.</param>
        /// <param name="mi">The MonitorInfoEx. Must be already set.</param>
        internal MonitorInfo(IntPtr hMonitor, MonitorInfoEx mi)
        {
            this.ScreenSize = new SizeF(mi.Monitor.Right - mi.Monitor.Left, mi.Monitor.Bottom - mi.Monitor.Top);
            this.MonitorArea = new Rectangle(mi.Monitor.Left, mi.Monitor.Top, mi.Monitor.Right - mi.Monitor.Left, mi.Monitor.Bottom - mi.Monitor.Top);
            this.WorkArea = new Rectangle(mi.WorkArea.Left, mi.WorkArea.Top, mi.WorkArea.Right - mi.WorkArea.Left, mi.WorkArea.Bottom - mi.WorkArea.Top);
            this.IsPrimary = mi.Flags > 0;
            this.Hmon = hMonitor;
            this.DeviceName = mi.DeviceName;
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
        /// Gets the pointer to the monitors Hardware Monitor.
        /// </summary>
        public IntPtr Hmon { get; }

        /// <inheritdoc/>
        public object RawHandler => this.Hmon;
    }
}
