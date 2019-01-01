namespace Unosquare.FFME
{
    using Shared;
    using System;
    using System.Runtime.CompilerServices;

    public partial class MediaEngine
    {
        /// <summary>
        /// Raises the MessageLogged event
        /// </summary>
        /// <param name="message">The <see cref="MediaLogMessage" /> instance containing the message.</param>
        [MethodImpl(256)]
        internal void SendOnMessageLogged(MediaLogMessage message)
        {
            Connector?.OnMessageLogged(this, message);
        }

        /// <summary>
        /// Raises the media failed event.
        /// </summary>
        /// <param name="ex">The ex.</param>
        [MethodImpl(256)]
        internal void SendOnMediaFailed(Exception ex)
        {
            this.LogError(Aspects.Connector, "Media Failure", ex);
            Connector?.OnMediaFailed(this, ex);
        }

        /// <summary>
        /// Raises the media closed event.
        /// </summary>
        [MethodImpl(256)]
        internal void SendOnMediaClosed()
        {
            Connector?.OnMediaClosed(this);
        }

        /// <summary>
        /// Raises the media opened event.
        /// </summary>
        [MethodImpl(256)]
        internal void SendOnMediaOpened()
        {
            Connector?.OnMediaOpened(this, Container?.MediaInfo);
        }

        /// <summary>
        /// Raises the media initializing event.
        /// </summary>
        /// <param name="config">The container configuration options.</param>
        /// <param name="url">The URL.</param>
        [MethodImpl(256)]
        internal void SendOnMediaInitializing(ContainerConfiguration config, string url)
        {
            Connector?.OnMediaInitializing(this, config, url);
        }

        /// <summary>
        /// Raises the media opening event.
        /// </summary>
        [MethodImpl(256)]
        internal void SendOnMediaOpening()
        {
            Connector?.OnMediaOpening(this, Container.MediaOptions, Container.MediaInfo);
        }

        /// <summary>
        /// Raises the media changing event.
        /// </summary>
        [MethodImpl(256)]
        internal void SendOnMediaChanging()
        {
            Connector?.OnMediaChanging(this, Container.MediaOptions, Container.MediaInfo);
        }

        /// <summary>
        /// Raises the media changed event.
        /// </summary>
        [MethodImpl(256)]
        internal void SendOnMediaChanged()
        {
            Connector?.OnMediaChanged(this, Container.MediaInfo);
        }

        /// <summary>
        /// Raises the buffering started event.
        /// </summary>
        [MethodImpl(256)]
        internal void SendOnBufferingStarted()
        {
            Connector?.OnBufferingStarted(this);
        }

        /// <summary>
        /// Raises the buffering ended event.
        /// </summary>
        [MethodImpl(256)]
        internal void SendOnBufferingEnded()
        {
            Connector?.OnBufferingEnded(this);
        }

        /// <summary>
        /// Raises the Seeking started event.
        /// </summary>
        [MethodImpl(256)]
        internal void SendOnSeekingStarted()
        {
            Connector?.OnSeekingStarted(this);
        }

        /// <summary>
        /// Raises the Seeking ended event.
        /// </summary>
        [MethodImpl(256)]
        internal void SendOnSeekingEnded()
        {
            Connector?.OnSeekingEnded(this);
        }

        /// <summary>
        /// Raises the media ended event.
        /// </summary>
        [MethodImpl(256)]
        internal void SendOnMediaEnded()
        {
            Connector?.OnMediaEnded(this);
        }

        /// <summary>
        /// Sends the on position changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        [MethodImpl(256)]
        internal void SendOnPositionChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            Connector?.OnPositionChanged(this, oldValue, newValue);
        }

        /// <summary>
        /// Sends the on media state changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        [MethodImpl(256)]
        internal void SendOnMediaStateChanged(PlaybackStatus oldValue, PlaybackStatus newValue)
        {
            Connector?.OnMediaStateChanged(this, oldValue, newValue);
        }
    }
}