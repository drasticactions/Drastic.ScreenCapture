// <copyright file="MonitorEnumeration.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using ScreenCaptureKit;

namespace Drastic.ScreenCapture
{
    public class MonitorEnumeration : IMonitorEnumeration
    {
        public IReadOnlyList<IMonitor> GetMonitors() => this.GetMonitorsAsync().Result;

        public async Task<IReadOnlyList<IMonitor>> GetMonitorsAsync()
        {
            var list = new List<MonitorInfo>();
            var result = await SCShareableContent.GetShareableContentAsync(true, false);
            foreach (var item in result.Displays)
            {
                list.Add(new MonitorInfo(item));
            }

            return list;
        }
    }
}
