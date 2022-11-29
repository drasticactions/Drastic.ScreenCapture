// <copyright file="IWindowCaptureSurface.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.ScreenCapture
{
    public interface IWindowCaptureSurface
    {
        IWindow Window { get; }

        ICaptureSurface Surface { get; }
    }
}
