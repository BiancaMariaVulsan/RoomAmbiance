using RoomProject.ViewModels;
using RoomProject.Views;
using System.Windows;

namespace RoomProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var viewModel = new MainWindowViewModel();
            var mainWindow = new MainWindow(viewModel);
            mainWindow.Show();
        }
    }
}
