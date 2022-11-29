// <copyright file="ICaptureSurface.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.ScreenCapture
{
    public interface ICaptureSurface
    {
        object RawSurface { get; }

        string Title { get; }
    }
}
