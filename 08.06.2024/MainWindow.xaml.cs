using System;
using System.Numerics;
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

        private async void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Text = "Calculating...";
            if (int.TryParse(NumberTextBox.Text, out int number) && number >= 0)
            {
                BigInteger result = await Task.Run(() => CalculateFactorial(number));
                ResultTextBlock.Text = $"Factorial: {result}";
            }
            else
            {
                ResultTextBlock.Text = "Please enter a valid non-negative integer.";
            }
        }

        private BigInteger CalculateFactorial(int number)
        {
            if (number == 0) return 1;
            BigInteger result = 1;
            for (int i = 1; i <= number; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}