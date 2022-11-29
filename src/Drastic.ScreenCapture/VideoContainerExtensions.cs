// <copyright file="VideoContainerExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.ScreenCapture
{
    public static class VideoContainerExtensions
    {
        public static string GenerateExtension(this VideoContainer container)
        {
            switch (container)
            {
                case VideoContainer.Unknown:
                    throw new NotImplementedException();
                case VideoContainer.Avi:
                    return ".avi";
                case VideoContainer.Mp4:
                    return ".mp4";
                case VideoContainer.Wmv:
                    return ".wmv";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
