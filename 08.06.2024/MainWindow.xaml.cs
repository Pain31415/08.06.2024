using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace _08._06._2024
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<HorseViewModel> horses;
        private CancellationTokenSource cancellationTokenSource;

        public ObservableCollection<HorseViewModel> Horses
        {
            get { return horses; }
            set { horses = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitializeHorses();
        }

        private void InitializeHorses()
        {
            Horses = new ObservableCollection<HorseViewModel>();

            for (int i = 1; i <= 5; i++)
            {
                Horses.Add(new HorseViewModel { Name = "Horse " + i, Distance = 0 });
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await Task.Run(() => StartRace(cancellationTokenSource.Token));
            }
            catch (TaskCanceledException)
            {
                // Race cancelled
            }
        }

        private async Task StartRace(CancellationToken cancellationToken)
        {
            Random random = new Random();

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500);

                foreach (var horse in Horses)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    horse.Distance += random.Next(1, 11);
                }

                foreach (var horse in Horses)
                {
                    if (horse.Distance >= 1000)
                    {
                        MessageBox.Show($"{horse.Name} wins the race!", "Race Over", MessageBoxButton.OK, MessageBoxImage.Information);
                        cancellationTokenSource.Cancel();
                        return;
                    }
                }
            }
        }
    }

    public class HorseViewModel : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        private int distance;
        public int Distance
        {
            get { return distance; }
            set
            {
                distance = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Distance"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}