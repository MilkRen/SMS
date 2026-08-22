using ClientUI;
using System.Windows;
using System.Windows.Input;
using WpfApp.Services.Interfaces;
using WpfApp.ViewModels;

namespace WpfApp.Services
{
    /// <summary>
    /// Сервис для работы с View
    /// </summary>
    public class DialogWindowService : IDialogWindowService
    {
        public void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext is MainWindowViewModel)
                    window.Close();
            }
        }

        public void DragMoveWindow()
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext is MainWindowViewModel)
                        window.DragMove();
                }
            }
        }

        public void MinimizeWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if(window.DataContext is MainWindowViewModel)
                    window.WindowState = WindowState.Minimized;
            }
        }

        /// <summary>
        /// Открытие главного окна
        /// </summary>
        public void OpenMainWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel(this, new JsonConfigurationServices());
            mainWindow.Show();
            App.Logger.Information("Открыто главное окно");
        }
    }
}
