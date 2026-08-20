using System.Collections.ObjectModel;
using WpfApp.ViewModels;

namespace WpfApp.Services.Interfaces
{
    /// <summary>
    /// Сервис для работы с файлами для переменных окружений
    /// </summary>
    public interface IJsonConfigurationServices
    {
        /// <summary>
        /// Первичная загрузка данных с json
        /// </summary>
        void LoadData();

        /// <summary>
        /// Сохранение значений для переменных окружений
        /// </summary>
        void SaveEnvironmentVariable(EnvironmentVariablesViewModel item);

        /// <summary>
        /// Сохранение комментариев для переменных окружений
        /// </summary>
        void SaveComments(EnvironmentVariablesViewModel item);

        /// <summary>
        /// Получить данные переменных окружений и комментариев
        /// </summary>
        ObservableCollection<EnvironmentVariablesViewModel> GetEnvironmentVariables();
    }
}