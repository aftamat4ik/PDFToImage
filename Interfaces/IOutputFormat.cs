using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace PDFToImage.Interfaces
{
    public interface IOutputFormat
    {
        string Name { get; }

        int ConversionQuality { get; set; }

        float Scale { get; set; }

        int MaxWidth { get; set; }

        int MaxHeight { get; set; }

        Task DoConversion(PdfDocument? document, string outputPath);
    }
}
