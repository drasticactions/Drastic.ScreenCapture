// <copyright file="IWinCaptureSurface.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Windows.Graphics.Capture;

namespace Drastic.ScreenCapture
{
    public interface IWinCaptureSurface : ICaptureSurface
    {
        GraphicsCaptureItem? GraphicsCaptureItem { get; }
    }
}
