using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace _08._06._2024
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<long> fibonacciNumbers;

        public ObservableCollection<long> FibonacciNumbers
        {
            get { return fibonacciNumbers; }
            set { fibonacciNumbers = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            FibonacciNumbers = new ObservableCollection<long>();
        }

        private async void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            FibonacciNumbers.Clear();

            if (!long.TryParse(LimitTextBox.Text, out long limit))
            {
                MessageBox.Show("Please enter a valid number for the limit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            await Task.Run(() => CalculateFibonacciNumbers(limit));
        }

        private void CalculateFibonacciNumbers(long limit)
        {
            long a = 0;
            long b = 1;

            FibonacciNumbers.Add(a);
            FibonacciNumbers.Add(b);

            for (int i = 2; i < limit; i++)
            {
                long temp = a + b;
                FibonacciNumbers.Add(temp);
                a = b;
                b = temp;
            }
        }
    }
}