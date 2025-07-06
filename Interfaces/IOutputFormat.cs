using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFToImage.Interfaces
{
    public interface IOutputFormat
    {
        string Name { get; }

        int ConversionQuality { get; set; }

        Task DoConversion(string inputPath, string outputPath);
    }
}
