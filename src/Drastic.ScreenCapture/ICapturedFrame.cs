// <copyright file="ICapturedFrame.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.ScreenCapture
{
    public interface ICapturedFrame
    {
        int Width { get; }

        int Height { get; }

        byte[] ImageData { get; }

        object Raw { get; }
    }
}