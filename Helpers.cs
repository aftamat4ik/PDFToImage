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

        /// <summary>
        /// just resizes given bitmap while keeping aspect ratio
        /// </summary>
        public static SKBitmap? ResizeBitmap(SKBitmap? bitmap, int targetWidth = 512, int targetHeight = 512)
        {
            if(bitmap == null)
                return null;

            float widthRatio = (float)targetWidth / bitmap.Width;
            float heightRatio = (float)targetHeight / bitmap.Height;
            float scale = Math.Min(widthRatio, heightRatio); // ratio itself

            int newWidth = (int)(bitmap.Width * scale);
            int newHeight = (int)(bitmap.Height * scale);

            // Center offsets
            int offsetX = (targetWidth - newWidth) / 2;
            int offsetY = (targetHeight - newHeight) / 2;

            // Create new bitmap
            var resizedBitmap = new SKBitmap(targetWidth, targetHeight, bitmap.ColorType, bitmap.AlphaType);
            using var canvas = new SKCanvas(resizedBitmap);
            canvas.Clear(SKColors.Transparent);

            // Draw scaled bitmap centered
            canvas.DrawBitmap(bitmap,
            new SKRect(0, 0, bitmap.Width, bitmap.Height), // source rect
                new SKRect(offsetX, offsetY, offsetX + newWidth, offsetY + newHeight) // destination rect
            );

            return resizedBitmap;
        }
    }
}
