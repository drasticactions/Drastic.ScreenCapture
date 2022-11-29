// <copyright file="WindowCaptureItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Diagnostics;
using Windows.Graphics.Capture;

namespace Drastic.ScreenCapture
{
    public class WindowCaptureItem : IWinCaptureSurface
    {
        public WindowCaptureItem(WindowInfo info)
        {
            this.Process = info.Process;
            var item = CaptureHelper.CreateItemForWindow(this.Process.MainWindowHandle);
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

        /// <summary>
        /// Gets the process.
        /// </summary>
        public Process Process { get; }
    }
}