﻿using System;
using System.Collections.Generic;
using FFmpeg.AutoGen.Native;

namespace FFmpeg.AutoGen
{
    public delegate IntPtr GetOrLoadLibrary(string libraryName, int version);

    public static partial class ffmpeg
    {
        public const int EAGAIN = 11;

        public const int ENOMEM = 12;

        public const int EINVAL = 22;

        private static readonly object SyncRoot = new object();

        static ffmpeg()
        {
            var loadedLibraries = new Dictionary<string, IntPtr>();

            GetOrLoadLibrary = (name, version) =>
            {
                var key = $"{name}{version}";
                IntPtr ptr;
                if (loadedLibraries.TryGetValue(key, out ptr)) return ptr;

                lock (SyncRoot)
                {
                    if (loadedLibraries.TryGetValue(key, out ptr)) return ptr;

                    ptr = LibraryLoader.LoadNativeLibrary(RootPath, name, version);
                    if (ptr == IntPtr.Zero) throw new DllNotFoundException($"Unable to load DLL '{name}.{version}': The specified module could not be found.");
                    loadedLibraries.Add(key, ptr);
                }

                return ptr;
            };
        }

        /// <summary>
        ///     Gets or sets the root path for loading libraries.
        /// </summary>
        /// <value>The root path.</value>
        public static string RootPath { get; set; } = string.Empty;

        public static GetOrLoadLibrary GetOrLoadLibrary { get; set; }

        public static T GetFunctionDelegate<T>(IntPtr libraryHandle, string functionName)
        {
            return FunctionLoader.GetFunctionDelegate<T>(libraryHandle, functionName);
        }

        public static ulong UINT64_C<T>(T a)
        {
            return Convert.ToUInt64(a);
        }

        public static int AVERROR<T1>(T1 a)
        {
            return -Convert.ToInt32(a);
        }

        public static int MKTAG<T1, T2, T3, T4>(T1 a, T2 b, T3 c, T4 d)
        {
            return (int) (Convert.ToUInt32(a) | (Convert.ToUInt32(b) << 8) | (Convert.ToUInt32(c) << 16) |
                          (Convert.ToUInt32(d) << 24));
        }

        public static int FFERRTAG<T1, T2, T3, T4>(T1 a, T2 b, T3 c, T4 d)
        {
            return -MKTAG(a, b, c, d);
        }

        public static int AV_VERSION_INT<T1, T2, T3>(T1 a, T2 b, T3 c)
        {
            return (Convert.ToInt32(a) << 16) | (Convert.ToInt32(b) << 8) | Convert.ToInt32(c);
        }

        public static string AV_VERSION_DOT<T1, T2, T3>(T1 a, T2 b, T3 c)
        {
            return $"{a}.{b}.{c}";
        }

        public static string AV_VERSION<T1, T2, T3>(T1 a, T2 b, T3 c)
        {
            return AV_VERSION_DOT(a, b, c);
        }
    }
}