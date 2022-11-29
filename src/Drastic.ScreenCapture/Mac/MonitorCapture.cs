// <copyright file="MonitorCapture.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.ScreenCapture
{
    public class MonitorCapture : IMonitorCaptureSurface
    {
        public MonitorCapture(MonitorInfo info)
        {
            this.Monitor = info;
            this.Surface = new MacCaptureItem(info);
        }

        public IMonitor Monitor { get; }

        public ICaptureSurface Surface { get; }
    }
}
