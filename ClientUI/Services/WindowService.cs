using System.Windows;
using WpfApp.Services.Interfaces;

namespace WpfApp.Services
{
    /// <summary>
    /// Сервис для работы с окном
    /// </summary>
    public class WindowService : IWindowService
    {
        public Window Window { get; }

        public WindowService(Window window)
        {
            Window = window;
        }
    }
}
