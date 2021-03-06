﻿namespace Unosquare.FFME.Platform
{
    using Rendering;
    using Shared;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Root for platform-specific implementations
    /// </summary>
    /// <seealso cref="IPlatform" />
    internal class WindowsPlatform : IPlatform
    {
        /// <summary>
        /// Initializes static members of the <see cref="WindowsPlatform"/> class.
        /// </summary>
        static WindowsPlatform()
        {
            Instance = new WindowsPlatform();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="WindowsPlatform"/> class from being created.
        /// </summary>
        /// <exception cref="InvalidOperationException">Unable to get a valid GUI context.</exception>
        private WindowsPlatform()
        {
            NativeMethods = WindowsNativeMethods.Instance;
            IsInDesignTime = GuiContext.Current.IsInDesignTime;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static WindowsPlatform Instance { get; }

        /// <inheritdoc />
        public INativeMethods NativeMethods { get; }

        /// <inheritdoc />
        public bool IsInDebugMode { get; } = Debugger.IsAttached;

        /// <inheritdoc />
        public bool IsInDesignTime { get; }

        /// <inheritdoc />
        public IMediaRenderer CreateRenderer(MediaType mediaType, MediaEngine mediaCore)
        {
            switch (mediaType)
            {
                case MediaType.Audio:
                    return new AudioRenderer(mediaCore);
                case MediaType.Video:
                    return new VideoRenderer(mediaCore);
                case MediaType.Subtitle:
                    return new SubtitleRenderer(mediaCore);
                default:
                    throw new NotSupportedException($"No suitable renderer for Media Type '{mediaType}'");
            }
        }

        /// <inheritdoc />
        public void HandleFFmpegLogMessage(MediaLogMessage message)
        {
            MediaElement.RaiseFFmpegMessageLogged(typeof(MediaElement), message);
        }
    }
}
