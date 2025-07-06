using CommunityToolkit.Mvvm.ComponentModel;
using PDFToImage.Interfaces;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFToImage.Models
{
    public partial class WebpFormat : ObservableObject, IOutputFormat
    {
        [ObservableProperty]
        private string _name = "webp";

        /// <summary>
        /// Webp format support two types of conversion - lossy and loseless. I can't put this into interface itself, so it will be class-specific
        /// </summary>
        [ObservableProperty]
        private bool _isLossless = false;

        [ObservableProperty]
        private int _conversionQuality = 100;

        public async Task DoConversion(string inputPath, string outputPath)
        {
            await Task.Run(() =>
            {
                // NOTE: 'using' in c# works as defer so all this will be closet at end of the scope
                using var inputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
                using var skInputStream = new SkiaSharp.SKManagedStream(inputStream);
                using var bitmap = SKBitmap.Decode(skInputStream);

                if (bitmap == null) 
                    throw new InvalidOperationException($"Failed to decode image from {inputPath}.");

                // Set WebP encoder options
                var compressionType = IsLossless ? SKWebpEncoderCompression.Lossless : SKWebpEncoderCompression.Lossy;
                var webpOptions = new SKWebpEncoderOptions(compressionType, ConversionQuality);

                using var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
                using var skOutputStream = new SkiaSharp.SKManagedWStream(outputStream, false);
                using var pixmap = bitmap.PeekPixels();

                if (!pixmap.Encode(skOutputStream, webpOptions))
                    // this all goes to the log...
                    throw new InvalidOperationException($"Failed to encode and write image to {outputPath}.");
            });
        }
    }

    public partial class PngFormat : ObservableObject, IOutputFormat
    {
        [ObservableProperty]
        private string _name = "png";

        [ObservableProperty]
        private int _conversionQuality = 100;

        public async Task DoConversion(string inputPath, string outputPath)
        {
            await Task.Run(() =>
            {
                using var inputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
                using var skInputStream = new SkiaSharp.SKManagedStream(inputStream);
                using var bitmap = SKBitmap.Decode(skInputStream);

                if (bitmap == null)
                    throw new InvalidOperationException($"Failed to decode image from {inputPath}.");

                using var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
                if(!bitmap.Encode(outputStream, SKEncodedImageFormat.Png, ConversionQuality))
                    throw new InvalidOperationException($"Failed to encode and write image to {outputPath}.");

            });
        }
    }

    public partial class JpgFormat : ObservableObject, IOutputFormat
    {
        [ObservableProperty]
        private string _name = "jpg";

        [ObservableProperty]
        private int _conversionQuality = 100;

        public async Task DoConversion(string inputPath, string outputPath)
        {
            await Task.Run(() =>
            {
                using var inputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read);
                using var skInputStream = new SkiaSharp.SKManagedStream(inputStream);
                using var bitmap = SKBitmap.Decode(skInputStream);
                if (bitmap == null)
                    throw new InvalidOperationException($"Failed to decode image from {inputPath}.");

                using var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
                
                if (!bitmap.Encode(outputStream, SKEncodedImageFormat.Jpeg, ConversionQuality))
                    throw new InvalidOperationException($"Failed to encode and write image to {outputPath}.");
            });
        }
    }
}
