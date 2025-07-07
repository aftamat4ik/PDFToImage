using CommunityToolkit.Mvvm.ComponentModel;
using PDFToImage.Interfaces;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Rendering.Skia;

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

        [ObservableProperty]
        private float _scale = 1.0f;

        [ObservableProperty]
        private int _maxWidth = 1000;

        [ObservableProperty]
        private int _maxHeight = 1000;

        public async Task DoConversion(PdfDocument? document, string outputPath)
        {
            await Task.Run(() =>
            {
                if (document == null)
                {
                    throw new InvalidOperationException($"Error: PDF document not valid");
                }

                document.AddSkiaPageFactory(); // this allows rendering documents into images

                // for all pages
                for (int i = 1; i <= document.NumberOfPages; i++) // NOTE: index starts from 1 because GetPageAsSKBitmap requires this
                {
                    var bitmap = document.GetPageAsSKBitmap(i, Scale, RGBColor.White);
                    if (bitmap.Width > MaxWidth || bitmap.Height > MaxHeight) // resize only if bigger
                    {
                        bitmap = Helpers.ResizeBitmap(bitmap, MaxWidth, MaxHeight);
                    }

                    if (bitmap == null)
                        throw new InvalidOperationException($"Failed to render page {i}.");

                    using (bitmap) // defer bitmap e.g. free it at end of the block
                    {
                        // Set WebP encoder options
                        var compressionType = IsLossless ? SKWebpEncoderCompression.Lossless : SKWebpEncoderCompression.Lossy;
                        var webpOptions = new SKWebpEncoderOptions(compressionType, ConversionQuality);

                        var relativePath = Path.Combine(outputPath, $"page_{i}.{Name.ToLower()}");

                        using var outputStream = new FileStream(relativePath, FileMode.Create, FileAccess.Write);
                        using var skOutputStream = new SkiaSharp.SKManagedWStream(outputStream, false);
                        using var pixmap = bitmap.PeekPixels();
                        if (!pixmap.Encode(skOutputStream, webpOptions))
                            // this all goes to the log...
                            throw new InvalidOperationException($"Failed to encode and write image to {relativePath}.");
                    }
                };
            });
        }
    }

    public partial class PngFormat : ObservableObject, IOutputFormat
    {
        [ObservableProperty]
        private string _name = "png";

        [ObservableProperty]
        private int _conversionQuality = 100;

        [ObservableProperty]
        private float _scale = 1.0f;

        [ObservableProperty]
        private int _maxWidth = 1000;

        [ObservableProperty]
        private int _maxHeight = 1000;

        public async Task DoConversion(PdfDocument? document, string outputPath)
        {
            await Task.Run(() =>
            {
                if (document == null)
                {
                    throw new InvalidOperationException($"Error: PDF document not valid");
                }

                document.AddSkiaPageFactory(); // this allows rendering documents into images

                // for all pages
                for (int i = 1; i <= document.NumberOfPages; i++)
                {
                    var bitmap = document.GetPageAsSKBitmap(i, Scale, RGBColor.White);
                    if (bitmap.Width > MaxWidth || bitmap.Height > MaxHeight) // resize only if bigger
                    {
                        bitmap = Helpers.ResizeBitmap(bitmap, MaxWidth, MaxHeight);
                    }

                    if (bitmap == null)
                        throw new InvalidOperationException($"Failed to render page {i}.");

                    using (bitmap)
                    {
                        string? relativePath = Path.Combine(outputPath, $"page_{i}.{Name.ToLower()}");
                        using var outputStream = new FileStream(relativePath, FileMode.Create, FileAccess.Write);
                        if (!bitmap.Encode(outputStream, SKEncodedImageFormat.Png, ConversionQuality))
                            throw new InvalidOperationException($"Failed to encode and write image to {relativePath}.");
                    }
                }
            });
        }
    }

    public partial class JpgFormat : ObservableObject, IOutputFormat
    {
        [ObservableProperty]
        private string _name = "jpg";

        [ObservableProperty]
        private int _conversionQuality = 100;

        [ObservableProperty]
        private float _scale = 1.0f;

        [ObservableProperty]
        private int _maxWidth = 1000;

        [ObservableProperty]
        private int _maxHeight = 1000;

        public async Task DoConversion(PdfDocument? document, string outputPath)
        {
            await Task.Run(() =>
            {
                if (document == null)
                {
                    throw new InvalidOperationException($"Error: PDF document not valid");
                }

                document.AddSkiaPageFactory(); // this allows rendering documents into images

                // for all pages
                for (int i = 1; i <= document.NumberOfPages; i++)
                {
                    var bitmap = document.GetPageAsSKBitmap(i, Scale, RGBColor.White);
                    if (bitmap.Width > MaxWidth || bitmap.Height > MaxHeight) // resize only if bigger
                    {
                        bitmap = Helpers.ResizeBitmap(bitmap, MaxWidth, MaxHeight);
                    }

                    if (bitmap == null)
                        throw new InvalidOperationException($"Failed to render page {i}.");
                    using (bitmap)
                    {
                        string? relativePath = Path.Combine(outputPath, $"page_{i}.{Name.ToLower()}");
                        using var outputStream = new FileStream(relativePath, FileMode.Create, FileAccess.Write);
                        if (!bitmap.Encode(outputStream, SKEncodedImageFormat.Jpeg, ConversionQuality))
                            throw new InvalidOperationException($"Failed to encode and write image to {relativePath}.");
                    }
                }
            });
        }
    }
}
