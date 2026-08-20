namespace WpfApp.Services.Interfaces
{
    /// <summary>
    /// Сервис для работы с View
    /// </summary>
    public interface IDialogWindowService
    {
        Action CloseAction { get; set; }

        Action DragMoveAction { get; set; }

        void CloseWindow();

        void DragMoveWindow();

        /// <summary>
        /// Открытие главного окна
        /// </summary>
        void OpenMainWindow();
    }
}