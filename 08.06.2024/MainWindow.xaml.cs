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

            if (double.TryParse(NumberTextBox.Text, out double number) && int.TryParse(ExponentTextBox.Text, out int exponent))
            {
                double result = await Task.Run(() => CalculatePower(number, exponent));
                ResultTextBlock.Text = $"Result: {result}";
            }
            else
            {
                ResultTextBlock.Text = "Please enter valid number and exponent.";
            }
        }

        private double CalculatePower(double number, int exponent)
        {
            return Math.Pow(number, exponent);
        }
    }
}