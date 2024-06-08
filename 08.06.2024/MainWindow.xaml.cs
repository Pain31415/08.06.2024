using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace _08._06._2024
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource? cancellationTokenSource;
        private Task? copyTask;

        public MainWindow()
        {
            InitializeComponent();
            PauseButton.IsEnabled = false;
            StopButton.IsEnabled = false;
        }

        private void BrowseSourceButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                SourceFilePath.Text = openFileDialog.FileName;
            }
        }

        private void BrowseDestinationButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == Forms.DialogResult.OK)
            {
                DestinationFolderPath.Text = dialog.SelectedPath;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string sourceFilePath = SourceFilePath.Text;
            string destinationFolderPath = DestinationFolderPath.Text;

            if (string.IsNullOrEmpty(sourceFilePath) || string.IsNullOrEmpty(destinationFolderPath))
            {
                MessageBox.Show("Please select both source file and destination folder.");
                return;
            }

            if (!int.TryParse(((ComboBoxItem)ThreadCountComboBox.SelectedItem)?.Content.ToString(), out int threadCount))
            {
                MessageBox.Show("Invalid thread count.");
                return;
            }

            StartButton.IsEnabled = false;
            PauseButton.IsEnabled = true;
            StopButton.IsEnabled = true;
            CopyProgressBar.Value = 0;

            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            copyTask = Task.Run(() => CopyFile(sourceFilePath, destinationFolderPath, threadCount, token), token);
        }

        private async Task CopyFile(string sourceFilePath, string destinationFolderPath, int threadCount, CancellationToken token)
        {
            string fileName = Path.GetFileName(sourceFilePath);
            string destinationFilePath = Path.Combine(destinationFolderPath, fileName);

            long fileSize = new FileInfo(sourceFilePath).Length;
            long chunkSize = fileSize / threadCount;

            using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
            {
                for (int i = 0; i < threadCount; i++)
                {
                    long start = i * chunkSize;
                    long end = (i == threadCount - 1) ? fileSize : (i + 1) * chunkSize;

                    await Task.Run(() => CopyChunk(sourceStream, destinationStream, start, end, token));
                }
            }

            Dispatcher.Invoke(() =>
            {
                CopyProgressBar.Value = 100;
                ProgressTextBlock.Text = "Copy Complete";
                StartButton.IsEnabled = true;
                PauseButton.IsEnabled = false;
                StopButton.IsEnabled = false;
            });
        }

        private async Task CopyChunk(FileStream sourceStream, FileStream destinationStream, long start, long end, CancellationToken token)
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
            long totalBytesRead = 0;

            sourceStream.Seek(start, SeekOrigin.Begin);
            destinationStream.Seek(start, SeekOrigin.Begin);

            while (totalBytesRead < (end - start))
            {
                int bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length, token);
                await destinationStream.WriteAsync(buffer, 0, bytesRead, token);
                totalBytesRead += bytesRead;

                Dispatcher.Invoke(() =>
                {
                    CopyProgressBar.Value = (double)totalBytesRead / (end - start) * 100;
                    ProgressTextBlock.Text = $"Copied {totalBytesRead / (1024 * 1024)} MB";
                });

                if (token.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (copyTask != null && !copyTask.IsCompleted)
            {
                cancellationTokenSource?.Cancel();
                PauseButton.IsEnabled = false;
                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
                ProgressTextBlock.Text = "Copy Paused";
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (copyTask != null && !copyTask.IsCompleted)
            {
                cancellationTokenSource?.Cancel();
                copyTask = null;
                StartButton.IsEnabled = true;
                PauseButton.IsEnabled = false;
                StopButton.IsEnabled = false;
                ProgressTextBlock.Text = "Copy Stopped";
            }
        }
    }
}