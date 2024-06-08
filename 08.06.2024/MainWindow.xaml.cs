using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace _08._06._2024
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<ProgressBarViewModel> progressBars;

        public ObservableCollection<ProgressBarViewModel> ProgressBars
        {
            get { return progressBars; }
            set { progressBars = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void StartDancingButton_Click(object sender, RoutedEventArgs e)
        {
            int progressBarCount;
            if (!int.TryParse(ProgressBarCountTextBox.Text, out progressBarCount))
            {
                MessageBox.Show("Please enter a valid number for the count of progress bars.");
                return;
            }

            ProgressBars = new ObservableCollection<ProgressBarViewModel>();

            for (int i = 0; i < progressBarCount; i++)
            {
                ProgressBars.Add(new ProgressBarViewModel());
            }

            await Dance(progressBarCount);
        }

        private async Task Dance(int progressBarCount)
        {
            Random random = new Random();

            while (true)
            {
                await Task.Delay(500);

                foreach (var progressBar in ProgressBars)
                {
                    progressBar.Value = random.Next(101);
                    progressBar.Color = string.Format("#{0:X6}", random.Next(0x1000000));
                }
            }
        }

        public class ProgressBarViewModel : INotifyPropertyChanged
        {
            private int value;
            public int Value
            {
                get { return value; }
                set
                {
                    this.value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
                }
            }

            private string color;
            public string Color
            {
                get { return color; }
                set
                {
                    color = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color"));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}