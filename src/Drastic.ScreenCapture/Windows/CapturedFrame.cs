// <copyright file="CapturedFrame.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace Drastic.ScreenCapture
{
    public class CapturedFrame : ICapturedFrame
    {
        public CapturedFrame(byte[] bitmap, int width, int height)
        {
            this.ImageData = bitmap;
            this.Width = width;
            this.Height = height;
        }

        public int Width { get; }

        public int Height { get; }

        public byte[] ImageData { get; internal set; } = new byte[0];

        public object Raw => this.ImageData;
    }
}
