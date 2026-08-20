using ClientUI;
using System.Xml.Linq;
using WpfApp.Constants;
using WpfApp.Services.Interfaces;
using WpfApp.ViewModels;

namespace WpfApp.Services
{
    /// <summary>
    /// Сервис для работы с View
    /// </summary>
    public class DialogWindowService : IDialogWindowService
    {
        public Action CloseAction { get; set; }

        public Action DragMoveAction { get; set; }

        public void CloseWindow()
        {
            App.Logger.Information("Кнопка - закрыть приложение");
            CloseAction();
        }

        public void DragMoveWindow()
        {
            App.Logger.Information("Кнопка - Перетаскивание приложения");
            DragMoveAction();
        }

        /// <summary>
        /// Открытие главного окна
        /// </summary>
        public void OpenMainWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel(mainWindow.Close, mainWindow.DragMove, new WindowService(mainWindow), new JsonConfigurationServices());
            mainWindow.Show();
            App.Logger.Information("Открыто главное окно");
        }
    }
}
