// <copyright file="CapturedFrameEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.ScreenCapture
{
    public class CapturedFrameEventArgs : EventArgs
    {
        public CapturedFrameEventArgs(ICapturedFrame frame)
        {
            this.Frame = frame;
        }

        public ICapturedFrame Frame { get; }
    }
}