/*
PDFToImage Converter

Copyright (c) 2025 aftamat4ik

Licensed under the MIT License. 
See LICENSE.txt in the project root for license information. */

using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFToImage.Models
{
    // this selected files list single file item
    public partial class FileItem : ObservableObject
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
