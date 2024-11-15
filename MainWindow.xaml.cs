using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfTask.LoaderFiles;

namespace WpfTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileSystemWatcher _watcher;
        private Timer _timer;
        private readonly Dictionary<string, IFileLoader> _loaders;
        public MainWindow()
        {
            InitializeComponent();
            _loaders = new Dictionary<string, IFileLoader>
            {
                { ".csv", new CSVLoader() },
                { ".xml", new XMLLoader() },
                { ".txt", new TXTLoader() }
            };
            StartMonitoring();
        }

        private void StartMonitoring()
        {
            
            string directoryPath = ConfigurationManager.AppSettings["InputDirectory"];
            int interval = int.Parse(ConfigurationManager.AppSettings["MonitoringInterval"]);

            directoryPath = System.IO.Path.GetFullPath(directoryPath);

            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                MessageBox.Show("Invalid directory path or directory does not exist.");
                return;
            }


            _watcher = new FileSystemWatcher(directoryPath)
            {
                Filter = "*.*", 
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };
            _watcher.Created += OnFileCreated;

        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            string fileExtension = System.IO.Path.GetExtension(e.FullPath).ToLower();
            Console.WriteLine($"Detected file: {e.FullPath} with extension: {fileExtension}");

            if (_loaders.ContainsKey(fileExtension))
            {
                var loader = _loaders[fileExtension];

                
                Task.Run(() =>
                {
                    var data = loader.LoadFile(e.FullPath);  

                   
                    Dispatcher.Invoke(() =>
                    {
                        DisplayData(data);  
                    });
                });
            }
            //LoadFile(e.FullPath);
        }

        private void DisplayData(List<TradeData> data)
        {
            if (data == null || !data.Any())
            {
                MessageBox.Show("No data loaded.");
                return;
            }

            Dispatcher.Invoke(() =>
            {
                DataGrid.ItemsSource = null; 
                DataGrid.ItemsSource = data; 
            });
        }

        private void AddNewFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                // Faylın yüklənməsi
                string fileExtension = System.IO.Path.GetExtension(selectedFilePath).ToLower();
                if (_loaders.ContainsKey(fileExtension))
                {
                    var loader = _loaders[fileExtension];
                    var data = loader.LoadFile(selectedFilePath);
                    DisplayData(data);
                }
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void LoadFile(string filePath)
        {

            List<string> data = new List<string> { $"Dosya yüklendi: {filePath}" };
            Dispatcher.Invoke(() => DataGrid.ItemsSource = data);
        }
    }
}
