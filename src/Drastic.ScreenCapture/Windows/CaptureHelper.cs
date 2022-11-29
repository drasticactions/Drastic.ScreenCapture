// <copyright file="CaptureHelper.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.UI.Composition;
using WinRT;

namespace Drastic.ScreenCapture
{
    internal static class CaptureHelper
    {
        internal static Guid ID3D11Texture2D = new Guid("6f15aaf2-d208-4e89-9ab4-489535d34f9c");

        private static readonly Guid GraphicsCaptureItemGuid = new Guid("79C3F95B-31F7-4EC2-A464-632EF5D30760");

        [ComImport]
        [Guid("25297D5C-3AD4-4C9C-B5CF-E36A38512330")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComVisible(true)]
        private interface ICompositorInterop
        {
            ICompositionSurface CreateCompositionSurfaceForHandle(
                IntPtr swapChain);

            ICompositionSurface CreateCompositionSurfaceForSwapChain(
                IntPtr swapChain);

            CompositionGraphicsDevice CreateGraphicsDevice(
                IntPtr renderingDevice);
        }

        [ComImport]
        [Guid("A9B3D012-3DF2-4EE3-B8D1-8695F457D3C1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComVisible(true)]
        private interface IDirect3DDxgiInterfaceAccess
        {
            IntPtr GetInterface([In] ref Guid iid);
        }

        internal static ICompositionSurface CreateCompositionSurfaceForSwapChain(this Compositor compositor, SharpDX.DXGI.SwapChain1 swapChain)
        {
            var interop = (ICompositorInterop)(object)compositor;
            return interop.CreateCompositionSurfaceForSwapChain(swapChain.NativePointer);
        }

        [DllImport("d3d11.dll", EntryPoint = "CreateDirect3D11DeviceFromDXGIDevice", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern uint CreateDirect3D11DeviceFromDXGIDevice(IntPtr dxgiDevice, out IntPtr graphicsDevice);

        [ComImport]
        [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComVisible(true)]
        private interface IInitializeWithWindow
        {
            void Initialize(
                IntPtr hwnd);
        }

        [ComImport]
        [Guid("3628E81B-3CAC-4C60-B7F4-23CE0E0C3356")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComVisible(true)]
        private interface IGraphicsCaptureItemInterop
        {
            void CreateForWindow(
                [In] IntPtr window,
                [In] ref Guid iid,
                out IntPtr result);

            void CreateForMonitor(
                [In] IntPtr monitor,
                [In] ref Guid iid,
                out IntPtr result);
        }

        public static IDirect3DDevice CreateDevice()
        {
            using var d3dDevice = new SharpDX.Direct3D11.Device(
                driverType: SharpDX.Direct3D.DriverType.Hardware,
                flags: SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport);

            // Acquire the DXGI interface for the Direct3D device.
            using var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device3>();

            // Wrap the native device using a WinRT interop object.
            var hr = CreateDirect3D11DeviceFromDXGIDevice(dxgiDevice.NativePointer, out IntPtr abi);

            if (hr != 0)
            {
                throw new InvalidProgramException($"CreateDirect3D11DeviceFromDXGIDevice failed with error code {hr}.");
            }

            return MarshalInterface<IDirect3DDevice>.FromAbi(abi);
        }

        public static GraphicsCaptureItem CreateItemForWindow(IntPtr hwnd)
        {
            var interop = GraphicsCaptureItem.As<IGraphicsCaptureItemInterop>();

            var temp = typeof(GraphicsCaptureItem);

            // For some reason typeof(GraphicsCaptureItem).GUID returns the wrong guid?
            interop.CreateForWindow(hwnd, GraphicsCaptureItemGuid, out var raw);
            var item = GraphicsCaptureItem.FromAbi(raw);
            Marshal.Release(raw);

            return item;
        }

        public static GraphicsCaptureItem CreateItemForMonitor(IntPtr hmon)
        {
            var interop = GraphicsCaptureItem.As<IGraphicsCaptureItemInterop>();

            var temp = typeof(GraphicsCaptureItem);

            // For some reason typeof(GraphicsCaptureItem).GUID returns the wrong guid?
            interop.CreateForMonitor(hmon, GraphicsCaptureItemGuid, out var raw);
            var item = GraphicsCaptureItem.FromAbi(raw);
            Marshal.Release(raw);

            return item;
        }

        internal static Windows.Media.MediaProperties.VideoEncodingQuality FromVideoEncodingQuality(this VideoEncodingQuality quality)
        {
            switch (quality)
            {
                case VideoEncodingQuality.Auto:
                    return Windows.Media.MediaProperties.VideoEncodingQuality.Auto;
                case VideoEncodingQuality.HD1080p:
                    return Windows.Media.MediaProperties.VideoEncodingQuality.HD1080p;
                case VideoEncodingQuality.HD720p:
                    return Windows.Media.MediaProperties.VideoEncodingQuality.HD720p;
                case VideoEncodingQuality.Wvga:
                    return Windows.Media.MediaProperties.VideoEncodingQuality.Wvga;
                case VideoEncodingQuality.Ntsc:
                    return Windows.Media.MediaProperties.VideoEncodingQuality.Ntsc;
                case VideoEncodingQuality.Pal:
                    return Windows.Media.MediaProperties.VideoEncodingQuality.Pal;
                case VideoEncodingQuality.Vga:
                    return Windows.Media.MediaProperties.VideoEncodingQuality.Vga;
                case VideoEncodingQuality.Qvga:
                    return Windows.Media.MediaProperties.VideoEncodingQuality.Qvga;
                case VideoEncodingQuality.Uhd2160p:
                    return Windows.Media.MediaProperties.VideoEncodingQuality.Uhd2160p;
                case VideoEncodingQuality.Uhd4320p:
                    return Windows.Media.MediaProperties.VideoEncodingQuality.Uhd4320p;
                default:
                    throw new NotImplementedException();
            }
        }

        internal static SharpDX.Direct3D11.Texture2D CreateSharpDXTexture2D(IDirect3DSurface surface)
        {
            var access = (IDirect3DDxgiInterfaceAccess)surface;
            var d3dPointer = access.GetInterface(ID3D11Texture2D);
            var d3dSurface = new SharpDX.Direct3D11.Texture2D(d3dPointer);
            return d3dSurface;
        }
    }
}
