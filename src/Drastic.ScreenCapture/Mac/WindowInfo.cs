// <copyright file="WindowInfo.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using ScreenCaptureKit;

namespace Drastic.ScreenCapture
{
    public class WindowInfo : IWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowInfo"/> class.
        /// </summary>
        /// <param name="window"><see cref="SCWindow"/>.</param>
        public WindowInfo(SCWindow window)
        {
            ArgumentNullException.ThrowIfNull(window, nameof(window));

            this.Title = window.Title ?? string.Empty;
            this.Window = window;
        }

        /// <inheritdoc/>
        public string Title { get; }

        /// <summary>
        /// Gets the <see cref="SCWindow"/>.
        /// </summary>
        public SCWindow Window { get; }

        /// <inheritdoc/>
        public object RawHandler => this.Window;
    }
}
