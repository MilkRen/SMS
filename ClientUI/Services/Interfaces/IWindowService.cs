using System.Windows;

namespace WpfApp.Services.Interfaces
{
    /// <summary>
    /// Сервис для работы с окном
    /// </summary>
    public interface IWindowService
    {
        Window Window { get; }
    }
}