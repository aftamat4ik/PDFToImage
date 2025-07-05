using CommunityToolkit.Mvvm.ComponentModel;
using PDFToImage.Models;
using System.Collections.ObjectModel;

namespace PDFToImage.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";

        [ObservableProperty]
        private ObservableCollection<FileItem> _files = new();

        MainWindowViewModel() { }
    }
}
