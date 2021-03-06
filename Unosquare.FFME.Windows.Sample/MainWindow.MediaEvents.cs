﻿namespace Unosquare.FFME.Windows.Sample
{
    using ClosedCaptions;
    using Events;
    using FFmpeg.AutoGen;
    using Platform;
    using Shared;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows;

    public partial class MainWindow
    {
        #region Logging Event Handlers

        /// <summary>
        /// Handles the MessageLogged event of the Media control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MediaLogMessageEventArgs" /> instance containing the event data.</param>
        private void OnMediaMessageLogged(object sender, MediaLogMessageEventArgs e)
        {
            if (e.MessageType == MediaLogMessageType.Trace)
                return;

            Debug.WriteLine(e);
        }

        /// <summary>
        /// Handles the FFmpegMessageLogged event of the MediaElement control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MediaLogMessageEventArgs"/> instance containing the event data.</param>
        private void OnMediaFFmpegMessageLogged(object sender, MediaLogMessageEventArgs e)
        {
            if (e.MessageType != MediaLogMessageType.Warning && e.MessageType != MediaLogMessageType.Error)
                return;

            if (string.IsNullOrWhiteSpace(e.Message) == false && e.Message.Contains("Using non-standard frame rate"))
                return;

            Debug.WriteLine(e);
        }

        /// <summary>
        /// Handles the MediaFailed event of the Media control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExceptionRoutedEventArgs"/> instance containing the event data.</param>
        private void OnMediaFailed(object sender, Events.ExceptionRoutedEventArgs e)
        {
            MessageBox.Show(
                App.Current.MainWindow,
                $"Media Failed: {e.ErrorException.GetType()}\r\n{e.ErrorException.Message}",
                $"{nameof(MediaElement)} Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.OK);
        }

        #endregion

        #region Media Stream Opening Event Handlers

        /// <summary>
        /// Handles the MediaInitializing event of the Media control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MediaInitializingEventArgs"/> instance containing the event data.</param>
        private void OnMediaInitializing(object sender, MediaInitializingEventArgs e)
        {
            // An example of injecting input options for http/https streams
            if (e.Url.StartsWith("http://") || e.Url.StartsWith("https://"))
            {
                e.Configuration.PrivateOptions["user_agent"] = $"{typeof(ContainerConfiguration).Namespace}/{typeof(ContainerConfiguration).Assembly.GetName().Version}";
                e.Configuration.PrivateOptions["headers"] = "Referer:https://www.unosquare.com";
                e.Configuration.PrivateOptions["multiple_requests"] = "1";
                e.Configuration.PrivateOptions["reconnect"] = "1";
                e.Configuration.PrivateOptions["reconnect_streamed"] = "1";
                e.Configuration.PrivateOptions["reconnect_delay_max"] = "10"; // in seconds

                // e.Configuration.PrivateOptions["reconnect_at_eof"] = "1"; // This prevents some HLS stream from opening properly
            }

            // Example of forcing tcp transport on rtsp feeds
            // RTSP is similar to HTTP but it only provides metadata about the underlying stream
            // Most RTSP compatible streams expose RTP data over both UDP and TCP.
            // TCP provides reliable communication while UDP does not
            if (e.Url.StartsWith("rtsp://"))
            {
                e.Configuration.PrivateOptions["rtsp_transport"] = "tcp";
                e.Configuration.GlobalOptions.FlagNoBuffer = true;
            }

            // A few WMV files I have tested don't have continuous enough audio packets to support
            // perfect synchronization between audio and video
            Media.RendererOptions.AudioDisableSync = e.Url.EndsWith(".wmv");

            // In realtime streams these settings can be used to reduce latency (see example from issue #152)
            // e.Options.GlobalOptions.FlagNoBuffer = true;
            // e.Options.GlobalOptions.ProbeSize = 8192;
            // e.Options.GlobalOptions.MaxAnalyzeDuration = System.TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Handles the MediaOpening event of the Media control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MediaOpeningEventArgs"/> instance containing the event data.</param>
        private void OnMediaOpening(object sender, MediaOpeningEventArgs e)
        {
            const string SideLoadAspect = "Client.SideLoad";

            // You can start off by adjusting subtitles delay
            // e.Options.SubtitlesDelay = TimeSpan.FromSeconds(7); // See issue #216

            // Get the local file path from the URL (if possible)
            var mediaFilePath = string.Empty;
            try
            {
                var url = new Uri(e.Info.InputUrl);
                mediaFilePath = url.IsFile || url.IsUnc ? Path.GetFullPath(url.LocalPath) : string.Empty;
            }
            catch { /* Ignore Exceptions */ }

            // Example of automatically side-loading SRT subs
            if (string.IsNullOrWhiteSpace(mediaFilePath) == false)
            {
                var srtFilePath = Path.ChangeExtension(mediaFilePath, "srt");
                if (File.Exists(srtFilePath))
                    e.Options.SubtitlesUrl = srtFilePath;
            }

            // You can force video FPS if necessary
            // see: https://github.com/unosquare/ffmediaelement/issues/212
            // e.Options.VideoForcedFps = 25;

            // An example of specifically selecting a subtitle stream
            var subtitleStreams = e.Info.Streams.Where(kvp => kvp.Value.CodecType == AVMediaType.AVMEDIA_TYPE_SUBTITLE).Select(kvp => kvp.Value);
            var englishSubtitleStream = subtitleStreams.FirstOrDefault(s => s.Language != null && s.Language.ToLowerInvariant().StartsWith("en"));
            if (englishSubtitleStream != null)
            {
                e.Options.SubtitleStream = englishSubtitleStream;
            }

            // An example of specifically selecting an audio stream
            var audioStreams = e.Info.Streams.Where(kvp => kvp.Value.CodecType == AVMediaType.AVMEDIA_TYPE_AUDIO).Select(kvp => kvp.Value);
            var englishAudioStream = audioStreams.FirstOrDefault(s => s.Language != null && s.Language.ToLowerInvariant().StartsWith("en"));
            if (englishAudioStream != null)
            {
                e.Options.AudioStream = englishAudioStream;
            }

            // Setting Advanced Video Stream Options is also possible
            // ReSharper disable once InvertIf
            var videoStream = e.Options.VideoStream;
            if (videoStream != null)
            {
                // If we have a valid seek index let's use it!
                if (string.IsNullOrWhiteSpace(mediaFilePath) == false)
                {
                    try
                    {
                        // Try to Create or Load a Seek Index
                        var durationSeconds = e.Info.Duration.TotalSeconds > 0 ? e.Info.Duration.TotalSeconds : 0;
                        var seekIndex = LoadOrCreateVideoSeekIndex(mediaFilePath, videoStream.StreamIndex, durationSeconds);

                        // Make sure the seek index belongs to the media file path
                        if (seekIndex != null &&
                            string.IsNullOrWhiteSpace(seekIndex.SourceUrl) == false &&
                            seekIndex.SourceUrl.Equals(mediaFilePath) &&
                            seekIndex.StreamIndex == videoStream.StreamIndex)
                        {
                            // Set the index on the options object.
                            e.Options.VideoSeekIndex = seekIndex;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception, and ignore it. Continue execution.
                        Media?.LogError(SideLoadAspect, "Error loading seek index data.", ex);
                    }
                }

                // Hardware device priorities
                var deviceCandidates = new[]
                {
                    AVHWDeviceType.AV_HWDEVICE_TYPE_CUDA,
                    AVHWDeviceType.AV_HWDEVICE_TYPE_D3D11VA,
                    AVHWDeviceType.AV_HWDEVICE_TYPE_DXVA2
                };

                // Hardware device selection
                if (videoStream.FPS <= 30)
                {
                    foreach (var deviceType in deviceCandidates)
                    {
                        var accelerator = videoStream.HardwareDevices.FirstOrDefault(d => d.DeviceType == deviceType);
                        if (accelerator == null) continue;
                        if (GuiContext.Current.IsInDebugMode)
                            e.Options.VideoHardwareDevice = accelerator;

                        break;
                    }
                }

                // Start building a video filter
                var videoFilter = new StringBuilder();

                // The yadif filter de-interlaces the video; we check the field order if we need
                // to de-interlace the video automatically
                if (videoStream.IsInterlaced)
                    videoFilter.Append("yadif,");

                // Scale down to maximum 1080p screen resolution.
                if (videoStream.PixelHeight > 1080)
                {
                    // e.Options.VideoHardwareDevice = null;
                    videoFilter.Append("scale=-1:1080,");
                }

                e.Options.VideoFilter = videoFilter.ToString().TrimEnd(',');

                // Since the MediaElement control belongs to a different thread
                // we have to set properties on its UI thread.
                GuiContext.Current.EnqueueInvoke(() =>
                {
                    Media.ClosedCaptionsChannel = videoStream.HasClosedCaptions ?
                        CaptionsChannel.CC1 : CaptionsChannel.CCP;
                });
            }

            // e.Options.AudioFilter = "aecho=0.8:0.9:1000:0.3";
            // e.Options.AudioFilter = "chorus=0.5:0.9:50|60|40:0.4|0.32|0.3:0.25|0.4|0.3:2|2.3|1.3";
        }

        /// <summary>
        /// Handles the MediaOpened event of the Media control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnMediaOpened(object sender, MediaOpenedRoutedEventArgs e)
        {
            // Perform some notification or status change when the media opened
        }

        /// <summary>
        /// Handles the MediaReady event of the Media control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnMediaReady(object sender, RoutedEventArgs e)
        {
            // Set a start position (see issue #66 or issue #277)
            // Media.Position = TimeSpan.FromSeconds(5);
            // await Media.Seek(TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// Handles the MediaChanging event of the MediaControl.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MediaOpeningEventArgs"/> instance containing the event data.</param>
        private void OnMediaChanging(object sender, MediaOpeningEventArgs e)
        {
            var availableStreams = e.Info.Streams
                .Where(s => s.Value.CodecType == (AVMediaType)StreamCycleMediaType)
                .Select(x => x.Value)
                .ToList();

            if (availableStreams.Count <= 0) return;

            // Allow cycling though a null stream (means removing the stream)
            // Except for video streams.
            if (StreamCycleMediaType != MediaType.Video)
                availableStreams.Add(null);

            int currentIndex;

            switch (StreamCycleMediaType)
            {
                case MediaType.Audio:
                    currentIndex = availableStreams.IndexOf(e.Options.AudioStream);
                    break;

                case MediaType.Video:
                    currentIndex = availableStreams.IndexOf(e.Options.VideoStream);
                    break;

                case MediaType.Subtitle:
                    currentIndex = availableStreams.IndexOf(e.Options.SubtitleStream);
                    break;

                default:
                    return;
            }

            currentIndex += 1;
            if (currentIndex >= availableStreams.Count)
                currentIndex = 0;

            var newStream = availableStreams[currentIndex];
            switch (StreamCycleMediaType)
            {
                case MediaType.Audio:
                    e.Options.AudioStream = newStream;
                    break;

                case MediaType.Video:
                    e.Options.VideoStream = newStream;
                    break;

                case MediaType.Subtitle:
                    e.Options.SubtitleStream = newStream;
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// Handles the media changed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MediaOpenedRoutedEventArgs"/> instance containing the event data.</param>
        private void OnMediaChanged(object sender, MediaOpenedRoutedEventArgs e)
        {
            // placeholder
        }

        /// <summary>
        /// Called when the current audio device changes.
        /// Call <see cref="MediaElement.ChangeMedia"/> so the new default audio device gets selected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void OnAudioDeviceStopped(object sender, EventArgs e)
        {
            if (Media != null) await Media?.ChangeMedia();
        }

        #endregion

        #region Other Media Event Handlers and Methods

        /// <summary>
        /// Handles the PositionChanged event of the Media control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PositionChangedRoutedEventArgs"/> instance containing the event data.</param>
        private void OnMediaPositionChanged(object sender, PositionChangedRoutedEventArgs e)
        {
            // Handle position change notifications
        }

        /// <summary>
        /// Loads the index of the or create media seek.
        /// </summary>
        /// <param name="mediaFilePath">The URL.</param>
        /// <param name="streamIndex">The associated stream index.</param>
        /// <param name="durationSeconds">The duration in seconds.</param>
        /// <returns>
        /// The seek index
        /// </returns>
        private VideoSeekIndex LoadOrCreateVideoSeekIndex(string mediaFilePath, int streamIndex, double durationSeconds)
        {
            var seekFileName = $"{Path.GetFileNameWithoutExtension(mediaFilePath)}.six";
            var seekFilePath = Path.Combine(App.Current.ViewModel.Playlist.IndexDirectory, seekFileName);
            if (string.IsNullOrWhiteSpace(seekFilePath)) return null;

            if (File.Exists(seekFilePath))
            {
                using (var stream = File.OpenRead(seekFilePath))
                    return VideoSeekIndex.Load(stream);
            }
            else
            {
                if (GuiContext.Current.IsInDebugMode == false || durationSeconds <= 0 || durationSeconds >= 60)
                    return null;

                var seekIndex = MediaEngine.CreateVideoSeekIndex(mediaFilePath, streamIndex);
                if (seekIndex.Entries.Count <= 0) return null;

                using (var stream = File.OpenWrite(seekFilePath))
                    seekIndex.Save(stream);

                return seekIndex;
            }
        }

        #endregion
    }
}