using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PDFToImage.Interfaces;
using PDFToImage.Models;
using System.Collections.ObjectModel;
using System.Text;

namespace PDFToImage.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";

        [ObservableProperty]
        private ObservableCollection<IOutputFormat> _availableFormats = new();

        [ObservableProperty]
        private IOutputFormat? _selectedFormat = null;

        [ObservableProperty]
        private ObservableCollection<FileItem> _files = new();

        [ObservableProperty]
        private ObservableCollection<object> _selectedFiles = new();

        // webp allows to loseless compression therefore i have to separate it's logic
        [ObservableProperty]
        private bool _isWebpSelected = false;

        [ObservableProperty]
        private bool _isLossless = false;

        [ObservableProperty]
        private int _quality = 80;

        [ObservableProperty]
        private string _log = "";
        // used to form log itself
        private readonly Queue<string> _logBuilder = new();
        private readonly object _logLock = new(); // as log is written from async methods
        private const int MAX_LOG_SIZE = 200;

        // attribute [RelayCommand] for async commands for some reason doesn't work so i had to do it manually
        public IAsyncRelayCommand ConvertFilesCommand { get; }

        public MainWindowViewModel() {
            AvailableFormats = new ObservableCollection<IOutputFormat>
            {
                new WebpFormat(),
                new PngFormat(),
                new JpgFormat()
            };
            SelectedFormat = AvailableFormats[0];

            ConvertFilesCommand = new AsyncRelayCommand(ConvertFilesAsync, CanConvertFiles);

            PropertyChanged += (s, e) =>
            {
                //System.Diagnostics.Debug.WriteLine($"PropertyName = {e.PropertyName}");
                if (e.PropertyName == nameof(SelectedFormat) || e.PropertyName == nameof(Quality))
                {
                    UpdateWebpSelectionState();
                }
            };

            SelectedFiles.CollectionChanged += (s, e) =>
            {
                //System.Diagnostics.Debug.WriteLine($"SelectedFiles.Count = {SelectedFiles.Count}");
                RemoveSelectedCommand.NotifyCanExecuteChanged();
            };

            Files.CollectionChanged += (s, e) =>
            {
                ClearFilesCommand.NotifyCanExecuteChanged();
                ConvertFilesCommand.NotifyCanExecuteChanged();
            };


            UpdateWebpSelectionState();

            // initial log message
            AppendLog(" ------- Welcome! ^-^ -----");
        }

        [RelayCommand(CanExecute = nameof(CanRemoveSelected))] // CanExecute here enables and disables the button under the hood
        private void RemoveSelected()
        {
            if (SelectedFiles == null) return;
            AppendLog("> Removing selected items!");
            var toRemove = SelectedFiles.ToList(); // somehow i can't cast to <FileItem> here...
            foreach (var item in toRemove)
            {
                if (item is FileItem fi)
                {
                    Files.Remove(fi);
                    AppendLog($"> Item Removed {fi.FileName}");
                }
            }
            SelectedFiles?.Clear();
            RemoveSelectedCommand.NotifyCanExecuteChanged();
            ClearFilesCommand.NotifyCanExecuteChanged();
        }

        private void UpdateWebpSelectionState(){
            IsWebpSelected = SelectedFormat is WebpFormat;
            IsLossless = SelectedFormat is WebpFormat && Quality >= 100;
        }

        private bool CanRemoveSelected()
        {
            return SelectedFiles?.Count > 0;
        }

        [RelayCommand(CanExecute = nameof(CanClearFiles))]
        private void ClearFiles()
        {
            AppendLog(" ------- List of files cleared! -------");
            SelectedFiles?.Clear();
            Files?.Clear();
        }

        private bool CanClearFiles()
        {
            return Files?.Count > 0;
        }

        private string MakeOutputDirectory()
        {
            AppendLog($"> Checking Output Directory ... ");

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            if (Directory.Exists(basePath))
            {
                string outputDirectory = System.IO.Path.Combine(basePath, "Output");
                // make sure output dir exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);

                    AppendLog($"> Created output directory at {basePath}.");
                }

                AppendLog($"> Output directory is : {outputDirectory}");
                
                return outputDirectory;
            } else
                throw new InvalidOperationException($"Somehow current base directory not found in your system!");
        }

        private bool CanConvertFiles()
        {
            if (Files == null || Files.Count == 0)
            {
                return false;
            }
            if (SelectedFormat == null)
            {
                return false;
            }
            return true;
        }

        private async Task ConvertFilesAsync()
        {
            if (Files == null || Files.Count == 0){
                AppendLog("> No files: First Add files that will be converted!");
                return;
            }

            if (SelectedFormat == null) {
                AppendLog("> No format: Select format before conversion!");    
                return; 
            }

            var outputDir = MakeOutputDirectory();

            var loselessStr = "";
            if (SelectedFormat is WebpFormat swp) {
                if (IsLossless){
                    loselessStr = "(lossless)";
                    swp.IsLossless = true;
                } else {
                    swp.IsLossless = false;
                }
            }
            SelectedFormat.ConversionQuality = Quality;

            AppendLog($"> converting {Files.Count} files to {SelectedFormat.Name} {loselessStr}");

            var convertedCount = 0;

            using var semaphore = new SemaphoreSlim(3); // Limit to 3 concurrent conversions
            var tasks = SelectedFiles.OfType<FileItem>().ToList().Select(async item =>
            {
                await semaphore.WaitAsync(); // Acquire semaphore

                var pureName = Path.GetFileNameWithoutExtension(item.FileName);
                try
                {
                    var outputPath = Path.Combine(outputDir, $"{pureName}.{SelectedFormat.Name.ToLower()}");
                    await SelectedFormat.DoConversion(item.FilePath, outputPath);
                    Files?.Remove(item); // this is safe
                    AppendLog($"> Converted {pureName}");
                    Interlocked.Increment(ref convertedCount);
                }
                catch (Exception ex)
                {
                    AppendLog($"> Error: can't convert file with name {pureName} to format {SelectedFormat.Name}!\n> {ex.Message}");
                }
                finally
                {
                    semaphore.Release(); // Release semaphore
                }
            });
            await Task.WhenAll(tasks);


            foreach (var item in Files.ToList()) // .ToList() actually duplicates original collection so we can remove it's elements safely
            {
                var pureName = Path.GetFileNameWithoutExtension(item.FileName);
                try
                {
                    var outputPath = Path.Combine(outputDir, $"{pureName}.{SelectedFormat.Name.ToLower()}");
                    await SelectedFormat.DoConversion(item.FilePath, outputPath);
                    Files?.Remove(item); // this is safe
                    AppendLog($"> Converted {pureName}");
                    convertedCount++;
                }
                catch (Exception ex)
                {
                    AppendLog($"> Error: can't convert file with name {pureName} to format {SelectedFormat.Name}!\n> {ex.Message}");
                }
            }

            AppendLog($"> Converted {convertedCount} files to {SelectedFormat.Name}{loselessStr}");
            AppendLog($"> Files can be found in Output directory: {outputDir}.");
            AppendLog($"> ---------- ^-^ ----------");
        }

        /// <summary>
        /// Adds another message to the end of log
        /// </summary>
        public void AppendLog(string message)
        {
            lock (_logLock)
            {
                _logBuilder.Enqueue(message);
                Log = string.Join("\n", _logBuilder);
                if (_logBuilder.Count > MAX_LOG_SIZE)
                    _logBuilder.Dequeue();
            }
        }
    }
}
