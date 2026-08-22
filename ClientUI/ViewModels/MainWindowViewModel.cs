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

        private readonly IJsonConfigurationServices _jsonConfigurationServices;

        #endregion

        #region Ctor

        public MainWindowViewModel(IDialogWindowService dialogWindow, IJsonConfigurationServices jsonConfigurationServices)
        {
            _dialogWindow = dialogWindow;
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
            _dialogWindow.DragMoveWindow();
        }

        public Command CloseWindowCommand { get; }
        private bool CanCloseWindowCommandExecute(object p) => true;
        private void OnCloseWindowCommandExecuted(object p)
        {
            _dialogWindow.CloseWindow();
        }

        public Command MinimizeWindowCommand { get; }
        private bool CanMinimizeWindowCommandExecute(object p) => true;
        private void OnMinimizeWindowCommandExecuted(object p)
        {
            _dialogWindow.MinimizeWindow();
        }

        #endregion

        #region Binding

        public ObservableCollection<EnvironmentVariablesViewModel> Items => _items;

        private readonly ObservableCollection<EnvironmentVariablesViewModel> _items;

        #endregion
    }
}
