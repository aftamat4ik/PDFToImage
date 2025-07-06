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
            addFilesBtn.Click += AddFilesBtn_Click;
            logTB.TextChanged += LogTB_TextChanged;

        }

        /// <summary>
        /// Log always should be scrolled to the end
        /// </summary>
        private void LogTB_TextChanged(object? sender, TextChangedEventArgs e)
        {
            var TextSize = logTB.Text?.Length?? 0;
            if (TextSize == 0) 
                return;
            logTB.CaretIndex = TextSize - 1; // we just move caet index and this forces TextBox to scroll
        }

        /// <summary>
        /// Here we select files and add them to the list
        /// </summary>
        private async void AddFilesBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) 
                return;

            var options = new FilePickerOpenOptions
            {
                Title = "Select Multiple Files",
                AllowMultiple = true,
                FileTypeFilter = new[]
                {
                    //FilePickerFileTypes.Pdf,
                    FilePickerFileTypes.ImageAll,
                    new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
                }
            };

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);

            // this way we can set ViewModel variable values
            if (DataContext is MainWindowViewModel viewModel)
            {
                try
                {
                    //viewModel.Files.Clear(); // DEPRECATED: there is 'clear' button for this

                    viewModel.AppendLog($"> Adding files: {files.Count}");

                    // remove duplicates, NOTE that hames compared in IgnoreCase
                    var uniqueFiles = files
                        .Where(f => !viewModel.Files.Any(existing =>
                            string.Equals(existing.FilePath, f.TryGetLocalPath() ?? f.Path.ToString(), StringComparison.OrdinalIgnoreCase)))
                        .ToList();

                    // add files
                    foreach (var file in uniqueFiles)
                    {
                        var path = file.TryGetLocalPath() ?? file.Path.ToString();

                        //System.Diagnostics.Debug.WriteLine($"added file = {path} name = {file.Name}");
                        //viewModel.AppendLog($" > file = {path} name = {file.Name}");

                        var item = new FileItem(file);
                        viewModel.Files.Add(item);
                    }

                    viewModel.AppendLog($"> Total files: {viewModel.Files.Count}");
                }
                catch (Exception ex)
                {
                    viewModel.AppendLog($"> Error : {ex.Message}");
                }
            }
        }
    }
}