// <copyright file="ICaptureSessionFactory.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.ScreenCapture
{
    public interface ICaptureSessionFactory
    {
        public ICaptureSession CreateCaptureSessionForMonitor(IMonitor monitor);

        public ICaptureSession CreateCaptureSessionForWindow(IWindow window);
    }
}
