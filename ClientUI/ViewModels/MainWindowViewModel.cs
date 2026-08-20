using ClientUI.Infrastructure.Commads;
using ClientUI.Infrastructure.Commads.Base;
using ClientUI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApp.Services;
using WpfApp.Services.Interfaces;

namespace WpfApp.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        #region Services
        private readonly IDialogWindowService _dialogWindow;

        private readonly IWindowService _windowService;

        private readonly IJsonConfigurationServices _jsonConfigurationServices;

        #endregion

        #region Fields

        private Action _dragMoveAction;

        private Action _closeAction;

        #endregion

        #region Ctor

        public MainWindowViewModel(Action closeAction, Action dragMove, IWindowService windowService, IJsonConfigurationServices jsonConfigurationServices)
        {
            _dragMoveAction = dragMove;
            _closeAction = closeAction;
            _dialogWindow = new DialogWindowService();
            _windowService = windowService;
            _jsonConfigurationServices = jsonConfigurationServices;

            MoveWindowCommand = new LambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
            CloseWindowCommand = new LambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
            MinimizeWindowCommand = new LambdaCommand(OnMinimizeWindowCommandExecuted, CanMinimizeWindowCommandExecute);

            _items = _jsonConfigurationServices.GetEnvironmentVariables();
        }

        #endregion

        #region Commands

        public Command MoveWindowCommand { get; }
        private bool CanMoveWindowCommandExecute(object p) => true;
        private void OnMoveWindowCommandExecuted(object p)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                _dialogWindow.DragMoveAction = _dragMoveAction;
                _dialogWindow.DragMoveWindow();
            }
        }

        public Command CloseWindowCommand { get; }
        private bool CanCloseWindowCommandExecute(object p) => true;
        private void OnCloseWindowCommandExecuted(object p)
        {
            _dialogWindow.CloseAction = _closeAction;
            _dialogWindow.CloseAction();
        }

        public Command MinimizeWindowCommand { get; }
        private bool CanMinimizeWindowCommandExecute(object p) => true;
        private void OnMinimizeWindowCommandExecuted(object p)
        {
            _windowService.Window.WindowState = System.Windows.WindowState.Minimized;
        }

        #endregion

        #region Binding

        public ObservableCollection<EnvironmentVariablesViewModel> Items => _items;

        private readonly ObservableCollection<EnvironmentVariablesViewModel> _items;

        #endregion
    }
}
