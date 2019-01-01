namespace Unosquare.FFME
{
    using Rendering;
    using Shared;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using Unosquare.FFME.Common.NET4.Shared;

    public partial class MediaElement
    {
        /// <summary>
        /// Provides access to various internal media renderer options.
        /// The default options are optimal to work for most media streams.
        /// This is an advanced feature and it is not recommended to change these
        /// options without careful consideration.
        /// </summary>
        public RendererOptions RendererOptions { get; } = new RendererOptions();

        /// <summary>
        /// Gets the Media's natural duration
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public Duration NaturalDuration
        {
            get
            {
                var duration = MediaCore?.State.NaturalDuration;

                return !duration.HasValue
                  ? Duration.Automatic
                  : (duration.Value == TimeSpan.MinValue
                    ? Duration.Forever
                    : (duration.Value < TimeSpan.Zero
                    ? default(Duration)
                    : new Duration(duration.Value)));
            }
        }

        /// <summary>
        /// Gets the remaining playback duration. Returns Forever for indeterminate values.
        /// </summary>
        public Duration RemainingDuration
        {
            get
            {
                if (NaturalDuration.HasTimeSpan == false || IsSeekable == false) return Duration.Forever;
                return NaturalDuration.TimeSpan.Ticks < Position.Ticks ?
                    new Duration(TimeSpan.Zero) :
                    new Duration(TimeSpan.FromTicks(NaturalDuration.TimeSpan.Ticks - Position.Ticks));
            }
        }

        /// <summary>
        /// Gets the discrete time position of the start of the current
        /// frame of the main component.
        /// </summary>
        public TimeSpan FramePosition
        {
            get { return MediaCore?.State.FramePosition ?? default(TimeSpan); }
        }

        /// <summary>
        /// Gets the stream's total bit rate as reported by the container.
        /// Returns 0 if unavailable.
        /// </summary>
        public long BitRate
        {
            get { return MediaCore?.State.BitRate ?? default(long); }
        }

        /// <summary>
        /// Gets the instantaneous, compressed bit rate of the decoders for the currently active component streams.
        /// This is provided in bits per second.
        /// </summary>
        public long DecodingBitRate
        {
            get { return MediaCore?.State.DecodingBitRate ?? default(long); }
        }

        /// <summary>
        /// Provides key-value pairs of the metadata contained in the media.
        /// Returns null when media has not been loaded.
        /// </summary>
        public ReadOnlyDictionary<string, string> Metadata
        {
            get { return MediaCore?.State.Metadata; }
        }

        /// <summary>
        /// Provides stream, chapter and program info of the underlying media.
        /// Returns null when no media is loaded.
        /// </summary>
        public MediaInfo MediaInfo
        {
            get { return MediaCore?.MediaInfo; }
        }

        /// <summary>
        /// Gets the index of the video stream.
        /// </summary>
        public int VideoStreamIndex
        {
            get { return MediaCore?.State.VideoStreamIndex ?? -1; }
        }

        /// <summary>
        /// Gets the index of the audio stream.
        /// </summary>
        public int AudioStreamIndex
        {
            get { return MediaCore?.State.AudioStreamIndex ?? -1; }
        }

        /// <summary>
        /// Gets the index of the subtitle stream.
        /// </summary>
        public int SubtitleStreamIndex
        {
            get { return MediaCore?.State.SubtitleStreamIndex ?? -1; }
        }

        /// <summary>
        /// Gets the media format. Returns null when media has not been loaded.
        /// </summary>
        public string MediaFormat
        {
            get { return MediaCore?.State.MediaFormat; }
        }

        /// <summary>
        /// Gets the size in bytes of the current stream being read.
        /// For multi-file streams, get the size of the current file only.
        /// </summary>
        public long MediaStreamSize
        {
            get { return MediaCore?.State.MediaStreamSize ?? 0; }
        }

        /// <summary>
        /// Gets the duration of a single frame step.
        /// If there is a video component with a frame rate, this property returns the length of a frame.
        /// If there is no video component it simply returns a tenth of a second.
        /// </summary>
        public TimeSpan PositionStep
        {
            get { return MediaCore?.State.PositionStep ?? TimeSpan.Zero; }
        }

        /// <summary>
        /// Returns whether the given media has audio.
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public bool HasAudio
        {
            get { return MediaCore?.State.HasAudio ?? default(bool); }
        }

        /// <summary>
        /// Returns whether the given media has video. Only valid after the
        /// MediaOpened event has fired.
        /// </summary>
        public bool HasVideo
        {
            get { return MediaCore?.State.HasVideo ?? default(bool); }
        }

        /// <summary>
        /// Returns whether the given media has subtitles. Only valid after the
        /// MediaOpened event has fired.
        /// </summary>
        public bool HasSubtitles
        {
            get { return MediaCore?.State.HasSubtitles ?? false; }
        }

        /// <summary>
        /// Gets the video codec.
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public string VideoCodec
        {
            get { return MediaCore?.State.VideoCodec; }
        }

        /// <summary>
        /// Gets the video bit rate.
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public long VideoBitRate
        {
            get { return MediaCore?.State.VideoBitRate ?? default(long); }
        }

        /// <summary>
        /// Returns the clockwise angle that needs to be applied to the video for it to be displayed
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public double VideoRotation
        {
            get { return MediaCore?.State.VideoRotation ?? default(double); }
        }

        /// <summary>
        /// Returns the natural width of the media in the video.
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public int NaturalVideoWidth
        {
            get { return MediaCore?.State.NaturalVideoWidth ?? default(int); }
        }

        /// <summary>
        /// Returns the natural height of the media in the video.
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public int NaturalVideoHeight
        {
            get { return MediaCore?.State.NaturalVideoHeight ?? default(int); }
        }

        /// <summary>
        /// Gets the video frame rate.
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public double VideoFrameRate
        {
            get { return MediaCore?.State.VideoFrameRate ?? default(double); }
        }

        /// <summary>
        /// Gets the name of the video hardware decoder in use.
        /// Enabling hardware acceleration does not guarantee decoding will be performed in hardware.
        /// When hardware decoding of frames is in use this will return the name of the HW accelerator.
        /// Otherwise it will return an empty string.
        /// </summary>
        public string VideoHardwareDecoder
        {
            get { return MediaCore?.State.VideoHardwareDecoder; }
        }

        /// <summary>
        /// Gets the audio codec.
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public string AudioCodec
        {
            get { return MediaCore?.State.AudioCodec; }
        }

        /// <summary>
        /// Gets the audio bit rate.
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public long AudioBitRate
        {
            get { return MediaCore?.State.AudioBitRate ?? default(long); }
        }

        /// <summary>
        /// Gets the audio channels count.
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public int AudioChannels
        {
            get { return MediaCore?.State.AudioChannels ?? default(int); }
        }

        /// <summary>
        /// Gets the audio sample rate.
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public int AudioSampleRate
        {
            get { return MediaCore?.State.AudioSampleRate ?? default(int); }
        }

        /// <summary>
        /// Gets the audio bits per sample.
        /// Only valid after the MediaOpened event has fired.
        /// </summary>
        public int AudioBitsPerSample
        {
            get { return MediaCore?.State.AudioBitsPerSample ?? default(int); }
        }

        /// <summary>
        /// Returns whether the currently loaded media can be paused.
        /// This is only valid after the MediaOpened event has fired.
        /// Note that this property is computed based on whether the stream is detected to be a live stream.
        /// </summary>
        public bool CanPause
        {
            get { return MediaCore?.State.CanPause ?? default(bool); }
        }

        /// <summary>
        /// Returns whether the currently loaded media is live or realtime
        /// This is only valid after the MediaOpened event has fired.
        /// </summary>
        public bool IsLiveStream
        {
            get { return MediaCore?.State.IsLiveStream ?? default(bool); }
        }

        /// <summary>
        /// Returns whether the currently loaded media is a network stream.
        /// This is only valid after the MediaOpened event has fired.
        /// </summary>
        public bool IsNetworkStream
        {
            get { return MediaCore?.State.IsNetworkStream ?? default(bool); }
        }

        /// <summary>
        /// Gets a value indicating whether the currently loaded media can be seeked.
        /// </summary>
        public bool IsSeekable
        {
            get { return MediaCore?.State.IsSeekable ?? default(bool); }
        }

        /// <summary>
        /// Gets a value indicating whether the media is playing.
        /// </summary>
        public bool IsPlaying
        {
            get { return MediaCore?.State.IsPlaying ?? default(bool); }
        }

        /// <summary>
        /// Gets a value indicating whether the media is playing.
        /// </summary>
        public bool IsPaused
        {
            get { return MediaCore?.State.IsPaused ?? default(bool); }
        }

        /// <summary>
        /// Gets a value indicating whether the media has reached its end.
        /// </summary>
        public bool HasMediaEnded
        {
            get { return MediaCore?.State.HasMediaEnded ?? default(bool); }
        }

        /// <summary>
        /// Get a value indicating whether the media is buffering.
        /// </summary>
        public bool IsBuffering
        {
            get { return MediaCore?.State.IsBuffering ?? default(bool); }
        }

        /// <summary>
        /// Gets a value indicating whether the media seeking is in progress.
        /// </summary>
        public bool IsSeeking
        {
            get { return MediaCore?.State.IsSeeking ?? default(bool); }
        }

        /// <summary>
        /// Returns the current video SMTPE time code if available.
        /// </summary>
        public string VideoSmtpeTimeCode
        {
            get { return MediaCore?.State.VideoSmtpeTimeCode; }
        }

        /// <summary>
        /// Gets the current video aspect ratio if available.
        /// </summary>
        public string VideoAspectRatio
        {
            get { return MediaCore?.State.VideoAspectRatio; }
        }

        /// <summary>
        /// Gets a value that indicates the percentage of buffering progress made.
        /// Range is from 0 to 1
        /// </summary>
        public double BufferingProgress
        {
            get { return MediaCore?.State.BufferingProgress ?? default(double); }
        }

        /// <summary>
        /// Gets a value that indicates the percentage of download progress made.
        /// Range is from 0 to 1
        /// </summary>
        public double DownloadProgress
        {
            get { return MediaCore?.State.DownloadProgress ?? default(double); }
        }

        /// <summary>
        /// Gets the amount of bytes in the packet buffer for the active stream components.
        /// </summary>
        public long PacketBufferLength
        {
            get { return MediaCore?.State.PacketBufferLength ?? default(long); }
        }

        /// <summary>
        /// Gets the number of packets buffered for all components
        /// </summary>
        public int PacketBufferCount
        {
            get { return MediaCore?.State.PacketBufferCount ?? default(int); }
        }

        /// <summary>
        /// Gets a value indicating whether the media is in the process of opening.
        /// </summary>
        public bool IsOpening
        {
            get { return MediaCore?.State.IsOpening ?? default(bool); }
        }

        /// <summary>
        /// Gets a value indicating whether the media is in the process of closing.
        /// </summary>
        public bool IsClosing
        {
            get { return MediaCore?.State.IsClosing ?? default(bool); }
        }

        /// <summary>
        /// Gets a value indicating whether the media is currently changing its components.
        /// </summary>
        public bool IsChanging
        {
            get { return MediaCore?.State.IsChanging ?? default(bool); }
        }

        /// <summary>
        /// Gets a value indicating whether this media element
        /// currently has an open media url.
        /// </summary>
        public bool IsOpen
        {
            get { return MediaCore?.State.IsOpen ?? default(bool); }
        }

        /// <summary>
        /// Gets the current playback state.
        /// </summary>
        public MediaState MediaState
        {
            get { return (MediaState) (MediaCore?.State.MediaState ?? PlaybackStatus.Close); }
        }

        /// <summary>
        /// Gets a value indicating whether the video stream contains closed captions
        /// </summary>
        public bool HasClosedCaptions
        {
            get { return MediaCore?.State.HasClosedCaptions ?? default(bool); }
        }
    }
}
