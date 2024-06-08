using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Select Folder";
            openFileDialog.Filter = "All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                DirectoryPathTextBox.Text = openFileDialog.FileName;
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string directoryPath = DirectoryPathTextBox.Text;
            string wordToSearch = WordTextBox.Text;

            if (string.IsNullOrEmpty(directoryPath) || string.IsNullOrEmpty(wordToSearch))
            {
                MessageBox.Show("Please enter both directory path and word to search.");
                return;
            }

            try
            {
                List<SearchResult> searchResults = await Task.Run(() => SearchFiles(directoryPath, wordToSearch));

                ResultsListView.ItemsSource = searchResults;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private List<SearchResult> SearchFiles(string directoryPath, string wordToSearch)
        {
            List<SearchResult> searchResults = new List<SearchResult>();

            IEnumerable<string> files = Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                int occurrencesCount = File.ReadAllText(file).Split(new char[] { ' ', '.', ',' }, StringSplitOptions.RemoveEmptyEntries).Count(word => word.Equals(wordToSearch, StringComparison.OrdinalIgnoreCase));

                if (occurrencesCount > 0)
                {
                    SearchResult result = new SearchResult
                    {
                        FileName = Path.GetFileName(file),
                        FilePath = file,
                        OccurrencesCount = occurrencesCount
                    };

                    searchResults.Add(result);
                }
            }

            return searchResults;
        }
    }

    public class SearchResult
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int OccurrencesCount { get; set; }
    }
}
