// <copyright file="ScreenRecorder.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Runtime.CompilerServices;
using CoreFoundation;
using CoreMedia;
using ScreenCaptureKit;

namespace Drastic.ScreenCapture
{
    /// <summary>
    /// Screen Recorder.
    /// </summary>
    public class ScreenRecorder : NSObject, ISCStreamDelegate, ISCStreamOutput, INotifyPropertyChanged
    {
        public EventHandler<CapturedFrameEventArgs>? CapturedFrame;

        private CapturedFrame? frame;
        private bool isRecording;

        private DispatchQueue frameOutputQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenRecorder"/> class.
        /// </summary>
        /// <param name="display"><see cref="SCDisplay"/>.</param>
        public ScreenRecorder(SCDisplay display)
        {
            this.frameOutputQueue = new DispatchQueue("frame-handling");

            this.Filter = new SCContentFilter(display, new SCWindow[0], SCContentFilterOption.Exclude);

            // Set the capture size to twice the display size to support retina displays.
            this.Config = new SCStreamConfiguration();
            this.Config.Width = (nuint)display.Width * 2;
            this.Config.Height = (nuint)display.Height * 2;
            this.Config.MinimumFrameInterval = new CoreMedia.CMTime(1, 60);
            this.Config.QueueDepth = 5;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenRecorder"/> class.
        /// </summary>
        /// <param name="window"><see cref="SCWindow"/>.</param>
        public ScreenRecorder(SCWindow window)
        {
            this.frameOutputQueue = new DispatchQueue("frame-handling");

            this.Filter = new SCContentFilter(window);
            this.Config = new SCStreamConfiguration();
            this.Config.MinimumFrameInterval = new CoreMedia.CMTime(1, 60);
            this.Config.QueueDepth = 5;
            this.Stream = new SCStream(this.Filter, this.Config, this);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the screen recorder is recording.
        /// </summary>
        public bool IsRecording
        {
            get => this.isRecording;
            set => this.SetProperty(ref this.isRecording, value);
        }

        /// <summary>
        /// Gets the Window.
        /// </summary>
        public SCWindow? Window { get; }

        /// <summary>
        /// Gets the display.
        /// </summary>
        public SCDisplay? Display { get; }

        /// <summary>
        /// Gets the <see cref="SCStream"/>.
        /// </summary>
        public SCStream? Stream { get; private set; }

        /// <summary>
        /// Gets the <see cref="SCStreamConfiguration"/>.
        /// </summary>
        public SCStreamConfiguration Config { get; }

        /// <summary>
        /// Gets the <see cref="SCStreamConfiguration"/>.
        /// </summary>
        public SCContentFilter Filter { get; }

        public void StartCapture()
        {
            this.Stream = new SCStream(this.Filter, this.Config, this);

            this.Stream.AddStreamOutput(this, SCStreamOutputType.Screen, this.frameOutputQueue, out NSError? error);

            this.Stream.StartCapture((NSError error) =>
            {
                // TODO: Handle possible errors from ScreenCaptureKit.
            });

            this.IsRecording = true;
        }

        public void StopCapture()
        {
            this.Stream?.StopCapture((NSError error) =>
            {
                // TODO: Handle possible errors from ScreenCaptureKit.
            });

            this.IsRecording = false;
        }

#pragma warning disable SA1600 // Elements should be documented
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action? onChanged = null)
#pragma warning restore SA1600 // Elements should be documented
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// On Property Changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.InvokeOnMainThread(() =>
            {
                var changed = this.PropertyChanged;
                if (changed == null)
                {
                    return;
                }

                changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }

        [Export("stream:didStopWithError:")]
        private void DidStopWithErrors(SCStream stream, NSError error)
        {
            // TODO: Handle possible errors from ScreenCaptureKit.
        }

        [Export("stream:didOutputSampleBuffer:ofType:")]
        private void DidOutputSampleBuffer(SCStream stream, CMSampleBuffer sampleBuffer, SCStreamOutputType type)
        {
            if (!sampleBuffer.IsValid)
            {
                return;
            }

            DispatchQueue.MainQueue.DispatchAsync(async () =>
            {
                this.CapturedFrame?.Invoke(this, new CapturedFrameEventArgs(new CapturedFrame(sampleBuffer)));
            });
        }
    }
}
