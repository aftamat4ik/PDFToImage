using Avalonia.Media.Imaging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFToImage
{
    public static class Helpers
    {
        public const int DEFAULT_QUALITY = 80;
        public const int MAX_LOG_SIZE = 200;

        public static string GetTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        }
    }
}
