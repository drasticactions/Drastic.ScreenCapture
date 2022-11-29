// <copyright file="MonitorCaptureItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Windows.Graphics.Capture;

namespace Drastic.ScreenCapture
{
    public class MonitorCaptureItem : IWinCaptureSurface
    {
        public MonitorCaptureItem(MonitorInfo info)
        {
            var item = CaptureHelper.CreateItemForMonitor(info.Hmon);
            if (item is null)
            {
                throw new NullReferenceException(nameof(item));
            }

            this.RawSurface = item;
            this.Title = item.DisplayName;
        }

        public string Title { get; }

        public object RawSurface { get; }

        public GraphicsCaptureItem? GraphicsCaptureItem => this.RawSurface as GraphicsCaptureItem;
    }
}
