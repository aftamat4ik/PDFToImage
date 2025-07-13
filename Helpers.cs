using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SkiaSharp;

namespace PDFToImage
{
    public static class Helpers
    {
        public const int DEFAULT_QUALITY = 80;
        public const int MAX_LOG_SIZE = 200;
        public const string LOCALE_SETTING = "locale";

        /// <summary>
        /// returns UTC timestamp from current time
        /// </summary>
        public static string GetTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        }

        /// <summary>
        /// returns directory that contains this application binary
        /// </summary>
        public static string GetBaseDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// same as 'System.IO.Path.Combine' but also creates all folders
        /// </summary>
        /// <param name="parts">Path Parts</param>
        /// <returns>resulting directory</returns>
        public static string MakeDirectories(params string[] parts)
        {
            string combinedPath = Path.Combine(parts);

            if (!Directory.Exists(combinedPath))
            {
                Directory.CreateDirectory(combinedPath);
            }

            return combinedPath;
        }

        /// <summary>
        /// returns path to the 'Output' directory
        /// </summary>
        public static string GetOutputDirectory()
        {
            var res = Helpers.MakeDirectories(GetBaseDirectory(), "Output");
            if (res == null || !Directory.Exists(res))
                throw new InvalidOperationException($"Somehow current base directory not found in your system!");
            return res;
        }

        /// <summary>
        /// Opens systen Explorer with given folder opened in it
        /// </summary>
        /// <param name="folderPath">folder that will be opened</param>
        /// <exception cref="NotSupportedException">happens when current os explorer is not supported by this function</exception>
        public static void OpenFolder(string folderPath)
        {
            var fullPath = Path.GetFullPath(folderPath);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start("explorer", fullPath);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", fullPath);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", fullPath);
            }
            else
            {
                throw new NotSupportedException("Platform not supported");
            }
        }
    }
}
