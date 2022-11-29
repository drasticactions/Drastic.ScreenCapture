// <copyright file="MonitorEnumeration.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Drastic.ScreenCapture;

namespace Drastic.ScreenCapture
{
    /// <summary>
    /// Monitor Enumeration.
    /// </summary>
    public class MonitorEnumeration : IMonitorEnumeration
    {
        private delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);

        public IReadOnlyList<IMonitor> GetMonitors()
        {
            var result = new List<MonitorInfo>();

            EnumDisplayMonitors(
               IntPtr.Zero,
               IntPtr.Zero,
               delegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
               {
                   MonitorInfoEx mi = default(MonitorInfoEx);
                   mi.Size = Marshal.SizeOf(mi);
                   bool success = GetMonitorInfo(hMonitor, ref mi);
                   if (success)
                   {
                       result.Add(new MonitorInfo(hMonitor, mi));
                   }

                   return true;
               },
               IntPtr.Zero);

            return result;
        }

        public Task<IReadOnlyList<IMonitor>> GetMonitorsAsync()
        {
            return Task.FromResult(this.GetMonitors());
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        [DllImport("user32.dll")]
        private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

        /// <summary>
        /// The internal monitor information.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct MonitorInfoEx
        {
            public int Size;
            public RECT Monitor;
            public RECT WorkArea;
            public uint Flags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
        }

        /// <summary>
        /// Creates a common struct for a monitor Rectangle.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
