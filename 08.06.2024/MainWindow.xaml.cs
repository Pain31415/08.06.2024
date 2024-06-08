using System.IO;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.MessageBox;

namespace _08._06._2024
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseSourceButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SourceDirectoryPath.Text = dialog.SelectedPath;
            }
        }

        private void BrowseDestinationButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                DestinationDirectoryPath.Text = dialog.SelectedPath;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string sourceDirectory = SourceDirectoryPath.Text;
            string destinationDirectory = DestinationDirectoryPath.Text;

            if (string.IsNullOrEmpty(sourceDirectory) || string.IsNullOrEmpty(destinationDirectory))
            {
                MessageBox.Show("Please select both source and destination directories.");
                return;
            }

            if (!int.TryParse(((ComboBoxItem)ThreadCountComboBox.SelectedItem)?.Content.ToString(), out int threadCount))
            {
                MessageBox.Show("Invalid thread count.");
                return;
            }

            StartButton.IsEnabled = false;
            CopyProgressBar.Value = 0;
            ProgressTextBlock.Text = "Copying...";

            Task.Run(() => CopyDirectory(sourceDirectory, destinationDirectory, threadCount));
        }

        private void CopyDirectory(string sourceDirectory, string destinationDirectory, int threadCount)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
            DirectoryInfo destDir = new DirectoryInfo(destinationDirectory);

            if (!destDir.Exists)
            {
                destDir.Create();
            }

            FileInfo[] files = sourceDir.GetFiles();
            DirectoryInfo[] directories = sourceDir.GetDirectories();

            int totalItems = files.Length + directories.Length;
            int copiedItems = 0;

            Parallel.ForEach(files, new ParallelOptions { MaxDegreeOfParallelism = threadCount }, file =>
            {
                string targetFilePath = Path.Combine(destinationDirectory, file.Name);
                file.CopyTo(targetFilePath, true);
                Interlocked.Increment(ref copiedItems);
                Dispatcher.Invoke(() =>
                {
                    CopyProgressBar.Value = (double)copiedItems / totalItems * 100;
                    ProgressTextBlock.Text = $"Copied {copiedItems} of {totalItems} items";
                });
            });

            Parallel.ForEach(directories, new ParallelOptions { MaxDegreeOfParallelism = threadCount }, directory =>
            {
                string targetDirectoryPath = Path.Combine(destinationDirectory, directory.Name);
                CopyDirectory(directory.FullName, targetDirectoryPath, threadCount);
                Interlocked.Increment(ref copiedItems);
                Dispatcher.Invoke(() =>
                {
                    CopyProgressBar.Value = (double)copiedItems / totalItems * 100;
                    ProgressTextBlock.Text = $"Copied {copiedItems} of {totalItems} items";
                });
            });

            Dispatcher.Invoke(() =>
            {
                CopyProgressBar.Value = 100;
                ProgressTextBlock.Text = "Copy Complete";
                StartButton.IsEnabled = true;
            });
        }
    }
}