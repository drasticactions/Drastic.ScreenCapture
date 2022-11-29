// <copyright file="IWindowEnumeration.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Drastic.ScreenCapture
{
    public interface IWindowEnumeration
    {
        IReadOnlyList<IWindow> GetWindows();

        Task<IReadOnlyList<IWindow>> GetWindowsAsync();
    }
}
