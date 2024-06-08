using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace _08._06._2024
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BrowseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathTextBox.Text;
            string wordToSearch = WordTextBox.Text;

            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(wordToSearch))
            {
                MessageBox.Show("Please enter both file path and word to search.");
                return;
            }

            try
            {
                int count = await Task.Run(() =>
                {
                    int totalCount = 0;
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        totalCount += (line.Split(new char[] { ' ', '.', ',' }, StringSplitOptions.RemoveEmptyEntries).Length);
                    }
                    return totalCount;
                });

                MessageBox.Show($"The word '{wordToSearch}' appeared {count} times in the file.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathTextBox.Text;
            if (!string.IsNullOrEmpty(filePath))
            {
                await Task.Run(() =>
                {
                    string folderPath = Path.GetDirectoryName(filePath);
                    System.Diagnostics.Process.Start("explorer.exe", folderPath);
                });
            }
            else
            {
                MessageBox.Show("Please enter the file path first.");
            }
        }

        private async void FindButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathTextBox.Text;
            string wordToSearch = WordTextBox.Text;

            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(wordToSearch))
            {
                MessageBox.Show("Please enter both file path and word to search.");
                return;
            }

            try
            {
                bool found = await Task.Run(() =>
                {
                    string[] lines = File.ReadAllLines(filePath);
                    foreach (string line in lines)
                    {
                        if (line.Contains(wordToSearch))
                        {
                            return true;
                        }
                    }
                    return false;
                });

                if (found)
                {
                    MessageBox.Show($"The word '{wordToSearch}' found in the file.");
                }
                else
                {
                    MessageBox.Show($"The word '{wordToSearch}' not found in the file.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}