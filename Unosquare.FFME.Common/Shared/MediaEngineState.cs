﻿namespace Unosquare.FFME.Shared
{
    using Decoding;
    using Primitives;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.CompilerServices;
    using Unosquare.FFME.Common.NET4.Shared;

    /// <summary>
    /// Contains all the status properties of the stream being handled by the media engine.
    /// </summary>
    public sealed class MediaEngineState : IMediaEngineState
    {
        #region Property Backing and Private State

        private static readonly ReadOnlyDictionary<string, string> EmptyDictionary
            = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());

        private readonly MediaEngine MediaCore;
        private readonly AtomicInteger m_MediaState = new AtomicInteger((int)PlaybackStatus.Close);
        private readonly AtomicBoolean m_HasMediaEnded = new AtomicBoolean(default(bool));

        private readonly AtomicBoolean m_IsBuffering = new AtomicBoolean(default(bool));
        private readonly AtomicLong m_DecodingBitRate = new AtomicLong(default(long));
        private readonly AtomicDouble m_BufferingProgress = new AtomicDouble(default(double));
        private readonly AtomicDouble m_DownloadProgress = new AtomicDouble(default(double));
        private readonly AtomicLong m_PacketBufferLength = new AtomicLong(default(long));
        private readonly AtomicInteger m_PacketBufferCount = new AtomicInteger(default(int));

        private readonly AtomicTimeSpan m_Position = new AtomicTimeSpan(default(TimeSpan));
        private readonly AtomicTimeSpan m_FramePosition = new AtomicTimeSpan(default(TimeSpan));
        private readonly AtomicDouble m_SpeedRatio = new AtomicDouble(Constants.Controller.DefaultSpeedRatio);
        private readonly AtomicDouble m_Volume = new AtomicDouble(Constants.Controller.DefaultVolume);
        private readonly AtomicDouble m_Balance = new AtomicDouble(Constants.Controller.DefaultBalance);
        private readonly AtomicBoolean m_IsMuted = new AtomicBoolean(false);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaEngineState" /> class.
        /// </summary>
        /// <param name="mediaCore">The associated media core.</param>
        internal MediaEngineState(MediaEngine mediaCore)
        {
            MediaCore = mediaCore;
            ResetAll();
        }

        #endregion

        #region Controller Properties

        /// <inheritdoc />
        public Uri Source { get; private set; }

        /// <inheritdoc />
        public double SpeedRatio
        {
            get { return m_SpeedRatio.Value; }
            set { m_SpeedRatio.Value = value; }
        }

        /// <inheritdoc />
        public double Volume
        {
            get { return m_Volume.Value; }
            set { m_Volume.Value = value; }
        }

        /// <inheritdoc />
        public double Balance
        {
            get { return m_Balance.Value; }
            set { m_Balance.Value = value; }
        }

        /// <inheritdoc />
        public bool IsMuted
        {
            get { return m_IsMuted.Value; }
            set { m_IsMuted.Value = value; }
        }

        #endregion

        #region Renderer Update Driven Properties

        /// <inheritdoc />
        public PlaybackStatus MediaState
        {
            get { return (PlaybackStatus) m_MediaState.Value; }
            private set { m_MediaState.Value = (int) value; }
        }

        /// <inheritdoc />
        public TimeSpan Position
        {
            get { return m_Position.Value; }
            private set { m_Position.Value = value; }
        }

        /// <inheritdoc />
        public TimeSpan FramePosition
        {
            get { return m_FramePosition.Value; }
            private set { m_FramePosition.Value = value; }
        }

        /// <inheritdoc />
        public bool HasMediaEnded
        {
            get { return m_HasMediaEnded.Value; }
            private set { m_HasMediaEnded.Value = value; }
        }

        /// <inheritdoc />
        public string VideoSmtpeTimeCode { get; private set; } = string.Empty;

        /// <inheritdoc />
        public string VideoHardwareDecoder { get; private set; } = string.Empty;

        /// <inheritdoc />
        public bool HasClosedCaptions { get; private set; }

        #endregion

        #region Self-Updating Properties

        /// <inheritdoc />
        public bool IsPlaying
        {
            get { return IsOpen && MediaCore.Clock.IsRunning; }
        }

        /// <inheritdoc />
        public bool IsPaused
        {
            get { return IsOpen && !MediaCore.Clock.IsRunning; }
        }

        /// <inheritdoc />
        public bool IsSeeking
        {
            get { return MediaCore.Commands?.IsSeeking ?? false; }
        }

        /// <inheritdoc />
        public bool IsClosing
        {
            get { return MediaCore.Commands?.IsClosing ?? false; }
        }

        /// <inheritdoc />
        public bool IsOpening
        {
            get { return MediaCore.Commands?.IsOpening ?? false; }
        }

        /// <inheritdoc />
        public bool IsChanging
        {
            get { return MediaCore.Commands?.IsChanging ?? false; }
        }

        #endregion

        #region Container Fixed, One-Time Properties

        /// <inheritdoc />
        public bool IsOpen { get; private set; }

        /// <inheritdoc />
        public TimeSpan PositionStep { get; private set; }

        /// <inheritdoc />
        public long BitRate { get; private set; }

        /// <inheritdoc />
        public ReadOnlyDictionary<string, string> Metadata { get; private set; }

        /// <inheritdoc />
        public bool CanPause { get; private set; }

        /// <inheritdoc />
        public string MediaFormat { get; private set; }

        /// <inheritdoc />
        public long MediaStreamSize { get; private set; }

        /// <inheritdoc />
        public int VideoStreamIndex { get; private set; }

        /// <inheritdoc />
        public int AudioStreamIndex { get; private set; }

        /// <inheritdoc />
        public int SubtitleStreamIndex { get; private set; }

        /// <inheritdoc />
        public bool HasAudio { get; private set; }

        /// <inheritdoc />
        public bool HasVideo { get; private set; }

        /// <inheritdoc />
        public bool HasSubtitles { get; private set; }

        /// <inheritdoc />
        public string VideoCodec { get; private set; }

        /// <inheritdoc />
        public long VideoBitRate { get; private set; }

        /// <inheritdoc />
        public double VideoRotation { get; private set; }

        /// <inheritdoc />
        public int NaturalVideoWidth { get; private set; }

        /// <inheritdoc />
        public int NaturalVideoHeight { get; private set; }

        /// <inheritdoc />
        public string VideoAspectRatio { get; private set; }

        /// <inheritdoc />
        public double VideoFrameRate { get; private set; }

        /// <inheritdoc />
        public string AudioCodec { get; private set; }

        /// <inheritdoc />
        public long AudioBitRate { get; private set; }

        /// <inheritdoc />
        public int AudioChannels { get; private set; }

        /// <inheritdoc />
        public int AudioSampleRate { get; private set; }

        /// <inheritdoc />
        public int AudioBitsPerSample { get; private set; }

        /// <inheritdoc />
        public TimeSpan? NaturalDuration { get; private set; }

        /// <inheritdoc />
        public bool IsLiveStream { get; private set; }

        /// <inheritdoc />
        public bool IsNetworkStream { get; private set; }

        /// <inheritdoc />
        public bool IsSeekable { get; private set; }

        #endregion

        #region State Method Managed Media Properties

        /// <inheritdoc />
        public bool IsBuffering
        {
            get { return m_IsBuffering.Value; }
            private set { m_IsBuffering.Value = value; }
        }

        /// <inheritdoc />
        public long DecodingBitRate
        {
            get { return m_DecodingBitRate.Value; }
            private set { m_DecodingBitRate.Value = value; }
        }

        /// <inheritdoc />
        public double BufferingProgress
        {
            get { return m_BufferingProgress.Value; }
            private set { m_BufferingProgress.Value = value; }
        }

        /// <inheritdoc />
        public double DownloadProgress
        {
            get { return m_DownloadProgress.Value; }
            private set { m_DownloadProgress.Value = value; }
        }

        /// <inheritdoc />
        public long PacketBufferLength
        {
            get { return m_PacketBufferLength.Value; }
            private set { m_PacketBufferLength.Value = value; }
        }

        /// <inheritdoc />
        public int PacketBufferCount
        {
            get { return m_PacketBufferCount.Value; }
            private set { m_PacketBufferCount.Value = value; }
        }

        #endregion

        #region State Management Methods

        /// <summary>
        /// Updates the <see cref="Source"/> property.
        /// </summary>
        /// <param name="newSource">The new source.</param>
        [MethodImpl(256)]
        internal void UpdateSource(Uri newSource)
        {
            Source = newSource;
        }

        /// <summary>
        /// Updates the fixed container properties.
        /// </summary>
        [MethodImpl(256)]
        internal void UpdateFixedContainerProperties()
        {
            BitRate = MediaCore.Container?.MediaBitRate ?? default(long);
            IsOpen = !IsOpening && (MediaCore.Container?.IsOpen ?? default(bool));
            Metadata = MediaCore.Container?.Metadata ?? EmptyDictionary;
            MediaFormat = MediaCore.Container?.MediaFormatName;
            MediaStreamSize = MediaCore.Container?.MediaStreamSize ?? default(long);
            VideoStreamIndex = MediaCore.Container?.Components.Video?.StreamIndex ?? -1;
            AudioStreamIndex = MediaCore.Container?.Components.Audio?.StreamIndex ?? -1;
            SubtitleStreamIndex = MediaCore.Container?.Components.Subtitles?.StreamIndex ?? -1;
            HasAudio = MediaCore.Container?.Components.HasAudio ?? default(bool);
            HasVideo = MediaCore.Container?.Components.HasVideo ?? default(bool);
            HasClosedCaptions = MediaCore.Container?.Components.Video?.StreamInfo?.HasClosedCaptions ?? default(bool);
            HasSubtitles = (MediaCore.PreloadedSubtitles?.Count ?? 0) > 0
                || (MediaCore.Container?.Components.HasSubtitles ?? false);
            VideoCodec = MediaCore.Container?.Components.Video?.CodecName;
            VideoBitRate = MediaCore.Container?.Components.Video?.BitRate ?? default(long);
            VideoRotation = MediaCore.Container?.Components.Video?.DisplayRotation ?? default(double);
            NaturalVideoWidth = MediaCore.Container?.Components.Video?.FrameWidth ?? default(int);
            NaturalVideoHeight = MediaCore.Container?.Components.Video?.FrameHeight ?? default(int);
            VideoFrameRate = MediaCore.Container?.Components.Video?.AverageFrameRate ?? default(double);
            AudioCodec = MediaCore.Container?.Components.Audio?.CodecName;
            AudioBitRate = MediaCore.Container?.Components.Audio?.BitRate ?? default(long);
            AudioChannels = MediaCore.Container?.Components.Audio?.Channels ?? default(int);
            AudioSampleRate = MediaCore.Container?.Components.Audio?.SampleRate ?? default(int);
            AudioBitsPerSample = MediaCore.Container?.Components.Audio?.BitsPerSample ?? default(int);
            NaturalDuration = MediaCore.Container?.MediaDuration;
            IsLiveStream = MediaCore.Container?.IsLiveStream ?? default(bool);
            IsNetworkStream = MediaCore.Container?.IsNetworkStream ?? default(bool);
            IsSeekable = MediaCore.Container?.IsStreamSeekable ?? default(bool);
            CanPause = IsOpen && !IsLiveStream;

            var videoAspectWidth = MediaCore.Container?.Components.Video?.DisplayAspectWidth ?? default(int);
            var videoAspectHeight = MediaCore.Container?.Components.Video?.DisplayAspectHeight ?? default(int);
            VideoAspectRatio = videoAspectWidth != default(int) && videoAspectHeight != default(int) ?
                $"{videoAspectWidth}:{videoAspectHeight}" : string.Empty;

            var mediaType = MediaCore.Container?.Components.MainMediaType ?? MediaType.None;
            var main = MediaCore.Container?.Components.Main;

            switch (mediaType)
            {
                case MediaType.Audio:
                    PositionStep = TimeSpan.FromTicks(Convert.ToInt64(
                        TimeSpan.TicksPerMillisecond * AudioSampleRate / 1000d));
                    break;

                case MediaType.Video:
                    var baseFrameRate = (main as VideoComponent)?.BaseFrameRate ?? 1d;
                    PositionStep = TimeSpan.FromTicks(Convert.ToInt64(
                        TimeSpan.TicksPerMillisecond * 1000d / baseFrameRate));
                    break;

                default:
                    PositionStep = default(TimeSpan);
                    break;
            }
        }

        /// <summary>
        /// Updates state properties coming from a new media block.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="buffer">The buffer.</param>
        [MethodImpl(256)]
        internal void UpdateDynamicBlockProperties(MediaBlock block, MediaBlockBuffer buffer)
        {
            if (block == null) return;

            // Update the discrete frame position upon rendering
            if (block.MediaType == (MediaCore.Container?.Components.MainMediaType ?? MediaType.None))
                FramePosition = block.StartTime;

            // Update video block properties
            if (block is VideoBlock == false) return;

            // Capture the video block
            var videoBlock = (VideoBlock)block;

            // I don't know of any codecs changing the width and the height dynamically
            // but we update the properties just to be safe.
            NaturalVideoWidth = videoBlock.PixelWidth;
            NaturalVideoHeight = videoBlock.PixelHeight;

            // Update the has closed captions state as it might come in later
            // as frames are decoded
            if (HasClosedCaptions == false && videoBlock.ClosedCaptions.Count > 0)
                HasClosedCaptions = true;

            VideoSmtpeTimeCode = videoBlock.SmtpeTimeCode;
            VideoHardwareDecoder = videoBlock.IsHardwareFrame ?
                videoBlock.HardwareAcceleratorName : string.Empty;
        }

        /// <summary>
        /// Updates the media ended state and notifies the parent if there is a change from false to true.
        /// </summary>
        /// <param name="hasEnded">if set to <c>true</c> [has ended].</param>
        /// <param name="endTime">The time span to update the <see cref="NaturalDuration"/> with.</param>
        [MethodImpl(256)]
        internal void UpdateMediaEnded(bool hasEnded, TimeSpan endTime)
        {
            if (HasMediaEnded == false && hasEnded)
            {
                if (IsSeekable) NaturalDuration = endTime;
                HasMediaEnded = true;
                MediaCore.SendOnMediaEnded();
                return;
            }

            HasMediaEnded = hasEnded;
        }

        /// <summary>
        /// Updates the position related properties.
        /// </summary>
        [MethodImpl(256)]
        internal void UpdatePosition()
        {
            UpdatePosition(MediaCore.WallClock);
        }

        /// <summary>
        /// Updates the position related properties.
        /// </summary>
        /// <param name="newPosition">The new position.</param>
        [MethodImpl(256)]
        internal void UpdatePosition(TimeSpan newPosition)
        {
            var oldSpeedRatio = MediaCore.Clock.SpeedRatio;
            var newSpeedRatio = SpeedRatio;

            if (Math.Abs(oldSpeedRatio - newSpeedRatio) > double.Epsilon)
                MediaCore.Clock.SpeedRatio = SpeedRatio;

            var oldPosition = Position;
            if (oldPosition.Ticks == newPosition.Ticks)
                return;

            Position = newPosition;
            MediaCore.SendOnPositionChanged(oldPosition, newPosition);
        }

        /// <summary>
        /// Resets all media state properties
        /// </summary>
        [MethodImpl(256)]
        internal void ResetAll()
        {
            ResetMediaProperties();
            UpdateFixedContainerProperties();
            InitializeBufferingStatistics();
        }

        /// <summary>
        /// Updates the MediaState property.
        /// </summary>
        /// <param name="mediaState">State of the media.</param>
        [MethodImpl(256)]
        internal void UpdateMediaState(PlaybackStatus mediaState)
        {
            var oldValue = MediaState;
            if (oldValue == mediaState)
                return;

            MediaState = mediaState;
            MediaCore.SendOnMediaStateChanged(oldValue, mediaState);
        }

        /// <summary>
        /// Resets the controller properties.
        /// </summary>
        [MethodImpl(256)]
        internal void ResetMediaProperties()
        {
            var oldMediaState = MediaState;
            var newMediaState = PlaybackStatus.Close;

            // Reset Method-controlled properties
            MediaState = newMediaState;
            Position = default(TimeSpan);
            FramePosition = default(TimeSpan);
            HasMediaEnded = default(bool);

            // Reset decoder and buffering
            ResetBufferingStatistics();

            VideoSmtpeTimeCode = string.Empty;
            VideoHardwareDecoder = string.Empty;

            // Reset controller properties
            SpeedRatio = Constants.Controller.DefaultSpeedRatio;

            if (oldMediaState != newMediaState)
                MediaCore.SendOnMediaStateChanged(oldMediaState, newMediaState);
        }

        /// <summary>
        /// Resets all the buffering properties to their defaults.
        /// </summary>
        [MethodImpl(256)]
        internal void InitializeBufferingStatistics()
        {
            const long MinimumValidFileSize = 1024 * 1024; // 1 MB

            // Start with default values
            ResetBufferingStatistics();

            // Reset the properties if the is no associated container
            if (MediaCore.Container == null)
            {
                MediaStreamSize = default(long);
                return;
            }

            // Try to get a valid stream size
            MediaStreamSize = MediaCore.Container.MediaStreamSize;
            var durationSeconds = NaturalDuration?.TotalSeconds ?? 0d;

            // Compute the bit rate and buffering properties based on media byte size
            if (MediaStreamSize >= MinimumValidFileSize && IsSeekable && durationSeconds > 0)
            {
                // The bit rate is simply the media size over the total duration
                BitRate = Convert.ToInt64(8d * MediaStreamSize / durationSeconds);
            }
        }

        /// <summary>
        /// Updates the decoding bit rate.
        /// </summary>
        /// <param name="bitRate">The bit rate.</param>
        [MethodImpl(256)]
        internal void UpdateDecodingBitRate(long bitRate)
        {
            DecodingBitRate = bitRate;
        }

        /// <summary>
        /// Updates the buffering properties: <see cref="PacketBufferCount" />, <see cref="PacketBufferLength" />,
        /// <see cref="IsBuffering" />, <see cref="BufferingProgress" />, <see cref="DownloadProgress" />.
        /// If a change is detected on the <see cref="IsBuffering" /> property then a notification is sent.
        /// </summary>
        /// <param name="bufferLength">Length of the packet buffer.</param>
        /// <param name="bufferCount">The packet buffer count.</param>
        /// <param name="bufferCountMax">The packet buffer count maximum for all components</param>
        [MethodImpl(256)]
        internal void UpdateBufferingStats(long bufferLength, int bufferCount, int bufferCountMax)
        {
            PacketBufferCount = bufferCount;
            PacketBufferLength = bufferLength;
            BufferingProgress = bufferCountMax <= 0 ? 0 : Math.Min(1d, (double)bufferCount / bufferCountMax);
            DownloadProgress = Math.Min(1d, (double)bufferLength / MediaCore.BufferLengthMax);

            // Check if we are currently buffering
            var isCurrentlyBuffering = MediaCore.ShouldReadMorePackets
                && (MediaCore.IsSyncBuffering || BufferingProgress < 1d);

            // Detect and notify a change in buffering state
            if (isCurrentlyBuffering == IsBuffering)
                return;

            IsBuffering = isCurrentlyBuffering;
            if (isCurrentlyBuffering)
                MediaCore.SendOnBufferingStarted();
            else
                MediaCore.SendOnBufferingEnded();
        }

        /// <summary>
        /// Resets the buffering statistics.
        /// </summary>
        [MethodImpl(256)]
        private void ResetBufferingStatistics()
        {
            IsBuffering = default(bool);
            DecodingBitRate = default(long);
            BufferingProgress = default(double);
            DownloadProgress = default(double);
            PacketBufferLength = default(long);
            PacketBufferCount = default(int);
        }

        #endregion
    }
}
