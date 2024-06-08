using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace _08._06._2024
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Thread numberThread = new Thread(GenerateNumbers) { IsBackground = true };
            Thread letterThread = new Thread(GenerateLetters) { IsBackground = true };
            Thread symbolThread = new Thread(GenerateSymbols) { IsBackground = true };

            numberThread.Priority = GetSelectedPriority(NumberPriority);
            letterThread.Priority = GetSelectedPriority(LetterPriority);
            symbolThread.Priority = GetSelectedPriority(SymbolPriority);

            numberThread.Start();
            letterThread.Start();
            symbolThread.Start();
        }

        private ThreadPriority GetSelectedPriority(ComboBox comboBox)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            string priority = selectedItem.Content.ToString();

            return priority switch
            {
                "Lowest" => ThreadPriority.Lowest,
                "BelowNormal" => ThreadPriority.BelowNormal,
                "Normal" => ThreadPriority.Normal,
                "AboveNormal" => ThreadPriority.AboveNormal,
                "Highest" => ThreadPriority.Highest,
                _ => ThreadPriority.Normal,
            };
        }

        private void GenerateNumbers()
        {
            Random random = new Random();
            while (true)
            {
                int number = random.Next(0, 100);
                Dispatcher.Invoke(() => OutputTextBox.AppendText($"Number: {number}\n"));
                Thread.Sleep(1000);
            }
        }

        private void GenerateLetters()
        {
            Random random = new Random();
            while (true)
            {
                char letter = (char)random.Next('A', 'Z' + 1);
                Dispatcher.Invoke(() => OutputTextBox.AppendText($"Letter: {letter}\n"));
                Thread.Sleep(1000);
            }
        }

        private void GenerateSymbols()
        {
            Random random = new Random();
            string symbols = "!@#$%^&*()";
            while (true)
            {
                char symbol = symbols[random.Next(symbols.Length)];
                Dispatcher.Invoke(() => OutputTextBox.AppendText($"Symbol: {symbol}\n"));
                Thread.Sleep(1000);
            }
        }
    }
}