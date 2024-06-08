using System;
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

        private async void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Text = "Analyzing...";

            string text = TextInputTextBox.Text;

            (int vowels, int consonants, int symbols) = await Task.Run(() => AnalyzeText(text));

            ResultTextBlock.Text = $"Vowels: {vowels}\nConsonants: {consonants}\nSymbols: {symbols}";
        }

        private (int vowels, int consonants, int symbols) AnalyzeText(string text)
        {
            int vowels = 0;
            int consonants = 0;
            int symbols = 0;

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    if ("AEIOUaeiou".Contains(c))
                    {
                        vowels++;
                    }
                    else
                    {
                        consonants++;
                    }
                }
                else
                {
                    symbols++;
                }
            }

            return (vowels, consonants, symbols);
        }
    }
}