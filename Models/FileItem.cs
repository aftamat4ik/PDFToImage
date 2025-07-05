using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFToImage.Models
{
    public class FileItem : ObservableObject
    {
    [ObservableProperty]
    private string _filePath;
    [ObservableProperty]
    private string _fileName;
    public FileItem(IStorageFile file) {
            FilePath = file.TryGetLocalPath() ?? file.Path.ToString();
            FileName = file.Name;
    }
    }
}
