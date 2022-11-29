// <copyright file="WindowEnumeration.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using ScreenCaptureKit;

namespace Drastic.ScreenCapture
{
    public class WindowEnumeration : IWindowEnumeration
    {
        public IReadOnlyList<IWindow> GetWindows()
            => this.GetWindowsAsync().Result;

        public async Task<IReadOnlyList<IWindow>> GetWindowsAsync()
        {
            var list = new List<WindowInfo>();

            var result = await SCShareableContent.GetShareableContentAsync(false, true);
            foreach (var item in result.Windows)
            {
                list.Add(new WindowInfo(item));
            }

            return list;
        }
    }
}
