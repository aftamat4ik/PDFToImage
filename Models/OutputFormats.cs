/*
PDFToImage Converter

Copyright (c) 2025 aftamat4ik

Licensed under the MIT License. 
See LICENSE.txt in the project root for license information. */

using CommunityToolkit.Mvvm.ComponentModel;
using PDFToImage.Interfaces;
using SkiaSharp;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Filters.Dct.JpegLibrary;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Rendering.Skia;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Filters.Jbig2.PdfboxJbig2;
using UglyToad.PdfPig.Filters.Jpx.OpenJpeg;

namespace PDFToImage.Models
{
    // Create your filter provider
    /*public sealed class MyFilterProvider : BaseFilterProvider
    {
        /// <summary>
        /// The single instance of this provider.
        /// </summary>
        public static readonly IFilterProvider Instance = new MyFilterProvider();

        /// <inheritdoc/>
        private MyFilterProvider() : base(GetDictionary())
        {
        }

        private static Dictionary<string, IFilter> GetDictionary()
        {
            var ascii85 = new Ascii85Filter();
            var asciiHex = new AsciiHexDecodeFilter();
            var ccitt = new CcittFaxDecodeFilter();
            var dct = new JpegLibraryDctDecodeFilter(); // new filter
            var flate = new FlateFilter();
            var jbig2 = new PdfboxJbig2DecodeFilter();
            var jpx = new OpenJpegJpxDecodeFilter();
            var runLength = new RunLengthFilter();
            var lzw = new LzwFilter();

            return new Dictionary<string, IFilter>
        {
            { NameToken.Ascii85Decode.Data, ascii85 },
            { NameToken.Ascii85DecodeAbbreviation.Data, ascii85 },
            { NameToken.AsciiHexDecode.Data, asciiHex },
            { NameToken.AsciiHexDecodeAbbreviation.Data, asciiHex },
            { NameToken.CcittfaxDecode.Data, ccitt },
            { NameToken.CcittfaxDecodeAbbreviation.Data, ccitt },
            { NameToken.DctDecode.Data, dct }, // new filter
            { NameToken.DctDecodeAbbreviation.Data, dct }, // new filter
            { NameToken.FlateDecode.Data, flate },
            { NameToken.FlateDecodeAbbreviation.Data, flate },
            { NameToken.Jbig2Decode.Data, jbig2 },
            { NameToken.JpxDecode.Data, jpx },
            { NameToken.RunLengthDecode.Data, runLength },
            { NameToken.RunLengthDecodeAbbreviation.Data, runLength },
            { NameToken.LzwDecode.Data, lzw },
            { NameToken.LzwDecodeAbbreviation.Data, lzw }
        };
        }
    }*/

    // Create your filter provider
    public sealed class MyFilterProvider : BaseFilterProvider
    {
        /// <summary>
        /// The single instance of this provider.
        /// </summary>
        public static readonly IFilterProvider Instance = new MyFilterProvider();

        /// <inheritdoc/>
        private MyFilterProvider() : base(GetDictionary())
        {
        }

        private static Dictionary<string, IFilter> GetDictionary()
        {
            // new filters
            var jbig2 = new PdfboxJbig2DecodeFilter();
            var jpx = new OpenJpegJpxDecodeFilter();
            var dct = new JpegLibraryDctDecodeFilter();

            // Default filters
            var ascii85 = new Ascii85Filter();
            var asciiHex = new AsciiHexDecodeFilter();
            var ccitt = new CcittFaxDecodeFilter();
            //var dct = new DctDecodeFilter();
            var flate = new FlateFilter();
            var runLength = new RunLengthFilter();
            var lzw = new LzwFilter();

            return new Dictionary<string, IFilter>
        {
            { NameToken.Ascii85Decode.Data, ascii85 },
            { NameToken.Ascii85DecodeAbbreviation.Data, ascii85 },
            { NameToken.AsciiHexDecode.Data, asciiHex },
            { NameToken.AsciiHexDecodeAbbreviation.Data, asciiHex },
            { NameToken.CcittfaxDecode.Data, ccitt },
            { NameToken.CcittfaxDecodeAbbreviation.Data, ccitt },
            { NameToken.DctDecode.Data, dct }, // new filter
            { NameToken.DctDecodeAbbreviation.Data, dct },
            { NameToken.FlateDecode.Data, flate },
            { NameToken.FlateDecodeAbbreviation.Data, flate },
            { NameToken.Jbig2Decode.Data, jbig2 },
            { NameToken.JpxDecode.Data, jpx },
            { NameToken.RunLengthDecode.Data, runLength },
            { NameToken.RunLengthDecodeAbbreviation.Data, runLength },
            { NameToken.LzwDecode.Data, lzw },
            { NameToken.LzwDecodeAbbreviation.Data, lzw }
        };
        }
    }

    // this literally used for one task... TODO: maybe put it into helpers or so... or let it be
    public static class BitmapResizer{
        /// <summary>
        /// just resizes given bitmap while keeping aspect ratio
        /// </summary>
        public static SKBitmap? ResizeBitmap(SKBitmap? bitmap, int targetWidth = 512, int targetHeight = 512)
        {
            if (bitmap == null)
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
                    if (bitmap.Width > MaxWidth && bitmap.Height > MaxHeight) // resize only if bigger
                    {
                        bitmap = BitmapResizer.ResizeBitmap(bitmap, MaxWidth, MaxHeight);
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
                    if (bitmap.Width > MaxWidth && bitmap.Height > MaxHeight) // resize only if bigger
                    {
                        bitmap = BitmapResizer.ResizeBitmap(bitmap, MaxWidth, MaxHeight);
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
                    if (bitmap.Width > MaxWidth && bitmap.Height > MaxHeight) // resize only if bigger
                    {
                        bitmap = BitmapResizer.ResizeBitmap(bitmap, MaxWidth, MaxHeight);
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
