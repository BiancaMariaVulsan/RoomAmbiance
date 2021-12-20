using RoomProject.ViewModels;
using System.Windows;
namespace RoomProject.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private async void OnStartClicked(object sender, RoutedEventArgs e)
        {
            await _viewModel.StartMeasureAsync();
        }
    }
}
