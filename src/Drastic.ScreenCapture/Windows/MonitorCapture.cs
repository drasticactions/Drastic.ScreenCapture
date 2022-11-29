// <copyright file="MonitorCapture.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drastic.ScreenCapture
{
    public class MonitorCapture : IMonitorCaptureSurface
    {
        public MonitorCapture(MonitorInfo info)
        {
            this.Monitor = info;
            this.Surface = new MonitorCaptureItem(info);
        }

        public IMonitor Monitor { get; }

        public ICaptureSurface Surface { get; }
    }
}
