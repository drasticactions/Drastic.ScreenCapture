// <copyright file="CaptureSession.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Graphics.Imaging;

namespace Drastic.ScreenCapture
{
    public class CaptureSession : ICaptureSession
    {
        private IWinCaptureSurface captureSurface;
        private GraphicsCaptureSession? session;
        private GraphicsCaptureItem item;

        private Direct3D11CaptureFramePool? framePool;
        private IDirect3DDevice device;

        public CaptureSession(ICaptureSurface captureSurface)
        {
            this.captureSurface = captureSurface as IWinCaptureSurface ?? throw new ArgumentNullException(nameof(captureSurface));
            this.item = this.captureSurface.GraphicsCaptureItem ?? throw new ArgumentNullException(nameof(captureSurface));
            this.device = CaptureHelper.CreateDevice();
            this.Title = captureSurface.Title;
        }

        public event EventHandler<CapturedFrameEventArgs>? OnCapturedFrame;

        public string Title { get; }

        public void Start()
        {
            this.framePool = Direct3D11CaptureFramePool.CreateFreeThreaded(this.device, DirectXPixelFormat.B8G8R8A8UIntNormalized, 1, this.item.Size);
            this.framePool.FrameArrived += this.FramePool_FrameArrived;
            this.session = this.framePool.CreateCaptureSession(this.item);
            this.session.StartCapture();
        }

        public void Stop()
        {
            if (this.framePool is not null)
            {
                this.framePool.FrameArrived -= this.FramePool_FrameArrived;
            }

            this.framePool?.Dispose();
            this.framePool = null;
            this.session?.Dispose();
            this.session = null;
        }

        private async void FramePool_FrameArrived(Direct3D11CaptureFramePool sender, object args)
        {
            using (var frame = sender.TryGetNextFrame())
            {
                using var sbmp = await this.CreateSoftwareBitmapFromSurface(frame.Surface);
                var array = await this.EncodeImageAsync(sbmp);
                var cf = new CapturedFrame(array, sbmp.PixelWidth, sbmp.PixelHeight);
                this.OnCapturedFrame?.Invoke(this, new CapturedFrameEventArgs(cf));
            }
        }

        private async Task<SoftwareBitmap> CreateSoftwareBitmapFromSurface(IDirect3DSurface surface)
        {
            return await SoftwareBitmap.CreateCopyFromSurfaceAsync(surface);
        }

        private async Task<byte[]> EncodeImageAsync(SoftwareBitmap bitmap)
        {
            using MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, stream.AsRandomAccessStream());
            encoder.SetSoftwareBitmap(bitmap);
            await encoder.FlushAsync();
            return stream.ToArray();
        }
    }
}
