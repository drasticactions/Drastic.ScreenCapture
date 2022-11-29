// <copyright file="CapturedFrame.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using CoreImage;
using CoreMedia;
using CoreVideo;

namespace Drastic.ScreenCapture
{
    /// <summary>
    /// Captured Frame.
    /// </summary>
    public class CapturedFrame : ICapturedFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CapturedFrame"/> class.
        /// </summary>
        /// <param name="sample"><see cref="CMSampleBuffer"/>.</param>
        public CapturedFrame(CMSampleBuffer sample)
        {
            this.SampleBuffer = sample;
            var attachments = sample.GetAttachments(CMAttachmentMode.ShouldPropagate);

            using var imageBuffer = this.SampleBuffer.GetImageBuffer() as CVPixelBuffer;
            if (imageBuffer is not null)
            {
                this.Width = (int)imageBuffer.Width;
                this.Height = (int)imageBuffer.Height;
                this.Surface = imageBuffer.GetIOSurface();
                this.CIImage = new CIImage(imageBuffer);
            }
        }

        public object Raw => this.CIImage ?? throw new NullReferenceException();

        public CMSampleBuffer SampleBuffer { get; }

        public IOSurface.IOSurface? Surface { get; }

        public CIImage? CIImage { get; }

        public CGRect? ContentRect { get; }

        public double ContentScale { get; }

        public double ScaleFactor { get; }

        public int Width { get; }

        public int Height { get; }

        public byte[] ImageData => throw new NotImplementedException();
    }
}
