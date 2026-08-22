namespace WpfApp.Services.Interfaces
{
    /// <summary>
    /// Сервис для работы с View
    /// </summary>
    public interface IDialogWindowService
    {
        void CloseWindow();

        void DragMoveWindow();

        void MinimizeWindow();

        /// <summary>
        /// Открытие главного окна
        /// </summary>
        void OpenMainWindow();
    }
}