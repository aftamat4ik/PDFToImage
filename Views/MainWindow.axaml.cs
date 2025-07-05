using Avalonia.Controls;
using Avalonia.Platform.Storage;
using PDFToImage.Models;
using PDFToImage.ViewModels;

namespace PDFToImage.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            animals.ItemsSource = new string[]
                {"cat", "camel", "cow", "chameleon", "mouse", "lion", "zebra" }
            .OrderBy(x => x);
            selectFilesBtn.Click += SelectFilesBtn_Click;
        }

        /// <summary>
        /// Files selection handled here
        /// </summary>
        private async void SelectFilesBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;

            var options = new FilePickerOpenOptions
            {
                Title = "Select Multiple Files",
                AllowMultiple = true,
                FileTypeFilter = new[]
                {
                    FilePickerFileTypes.TextPlain,
                    FilePickerFileTypes.ImageAll,
                    new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
                }
            };

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
            
            // here we can set vm variable
            if(DataContext is MainWindowViewModel viewModel)
            {
                viewModel.Files.Clear();
                // add files
                foreach (var file in files){
                    if (file == null) continue;
                    var path = file.TryGetLocalPath() ?? file.Path.ToString();

                    System.Diagnostics.Debug.WriteLine($"selected files = {path} name = {file.Name}");

                    var item = new FileItem(file);
                    viewModel.Files.Add(item);
                }
            }
            /*SelectedFilePaths.Clear();
            foreach (var file in files)
            {
                var path = file.TryGetLocalPath() ?? file.Path.ToString();
                SelectedFilePaths.Add(path);
            }*/
        }
    }
}