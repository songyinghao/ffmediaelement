namespace Unosquare.FFME
{
    /// <summary>
    /// Provides constants for logging aspect identifiers
    /// </summary>
    internal static class Aspects
    {
        public static string None
        {
            get { return "Log.Text"; }
        }

        public static string FFmpegLog
        {
            get { return "FFmpeg.Log"; }
        }

        public static string EngineCommand
        {
            get { return "Engine.Commands"; }
        }

        public static string DecodingWorker
        {
            get { return "Engine.Decoding"; }
        }

        public static string RenderingWorker
        {
            get { return "Engine.Rendering"; }
        }

        public static string Connector
        {
            get { return "Engine.Connector"; }
        }

        public static string Container
        {
            get { return "Container"; }
        }

        public static string Component
        {
            get { return "Container.Component"; }
        }

        public static string ReferenceCounter
        {
            get { return "ReferenceCounter"; }
        }
    }
}
