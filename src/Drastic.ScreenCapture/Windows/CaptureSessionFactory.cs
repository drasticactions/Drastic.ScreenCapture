// <copyright file="CaptureSessionFactory.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.ScreenCapture
{
    public class CaptureSessionFactory : ICaptureSessionFactory
    {
        public ICaptureSession CreateCaptureSessionForMonitor(IMonitor monitor)
        {
            if (monitor is MonitorInfo info)
            {
                var item = new MonitorCaptureItem(info);
                return new CaptureSession(item);
            }

            throw new ArgumentNullException(nameof(monitor));
        }

        public ICaptureSession CreateCaptureSessionForWindow(IWindow window)
        {
            if (window is WindowInfo info)
            {
                var item = new WindowCaptureItem(info);
                return new CaptureSession(item);
            }

            throw new ArgumentNullException(nameof(window));
        }
    }
}
