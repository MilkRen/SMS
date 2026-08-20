using ClientUI;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.IO;
using WpfApp.Constants;
using WpfApp.Services.Interfaces;
using WpfApp.ViewModels;

namespace WpfApp.Services
{
    /// <summary>
    /// Сервис для работы с файлами для переменных окружений
    /// </summary>
    public class JsonConfigurationServices : IJsonConfigurationServices
    {
        #region Fields

        /// <summary>
        /// Флаг загрузки данных
        /// </summary>
        private bool LoadDataFlag = false;

        #endregion


        /// <summary>
        /// Первичная загрузка данных с json
        /// </summary>
        public void LoadData()
        {
            App.Logger.Information("Загрузка данных с json");

            var variableNames = App.Configuration.GetSection("EnvironmentVariables").Get<string[]>() ?? [];
            var commentsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileConstant.CommentsFile);

            // Загружаем существующие комментарии (если файл есть)
            var comments = new Dictionary<string, string>();
            if (File.Exists(commentsPath))
            {
                try
                {
                    var json = File.ReadAllText(commentsPath);
                    comments = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
                }
                catch (Exception ex)
                {
                    App.Logger.Error(ex, "Не удалось прочитать {File}, создаём заново", FileConstant.CommentsFile);
                    comments = new();
                }
            }

            // Добавляем отсутствующие переменные с пустым комментарием
            var needsSave = false;
            foreach (var name in variableNames)
            {
                if (!comments.ContainsKey(name))
                {
                    comments[name] = string.Empty;
                    needsSave = true;
                    App.Logger.Warning("Добавлен новый ключ '{Name}' в {File}", name, FileConstant.CommentsFile);
                }
            }

            // Сохраняем, если были изменения
            if (needsSave)
            {
                try
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(comments, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(commentsPath, json);
                    App.Logger.Information("{File} обновлён: добавлено {Count} новых ключей", FileConstant.CommentsFile, comments.Count - variableNames.Length + variableNames.Length);
                }
                catch (Exception ex)
                {
                    App.Logger.Error(ex, "Не удалось обновить {File}", FileConstant.CommentsFile);
                    throw;
                }
            }
        }

        /// <summary>
        /// Сохранение значений для переменных окружений
        /// </summary>
        public void SaveEnvironmentVariable(EnvironmentVariablesViewModel item)
        {
            if (LoadDataFlag)
                return;

            Environment.SetEnvironmentVariable(item.Name, item.Value, EnvironmentVariableTarget.User);
            App.Logger.Warning("Обновлены данные: {Name} - {Value} - {User}", item.Name, item.Value, EnvironmentVariableTarget.User);
        }

        /// <summary>
        /// Сохранение комментариев для переменных окружений
        /// </summary>
        public void SaveComments(EnvironmentVariablesViewModel item)
        {
            if (LoadDataFlag)
                return;

            try
            {
                var commentsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "comments.json");

                var comments = File.Exists(commentsPath)
                    ? System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(commentsPath)) ?? new()
                    : new Dictionary<string, string>();

                comments[item.Name] = item.Comment;
                try
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(comments, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(commentsPath, json);
                    App.Logger.Warning("Обновлены данные: {Name} - {Comment}", item.Name, item.Comment);
                }
                catch (Exception ex)
                {

                    App.Logger.Error(ex, "Не удалось обновить {File}", FileConstant.CommentsFile);
                    throw;
                }
            }
            catch (Exception ex)
            {
                App.Logger.Warning(ex, "Ошибка при сохранении {File}: {error}", FileConstant.CommentsFile);
                throw;
            }
        }

        /// <summary>
        /// Получить данные переменных окружений и комментариев
        /// </summary>
        public ObservableCollection<EnvironmentVariablesViewModel> GetEnvironmentVariables()
        {
            var variables = new List<EnvironmentVariablesViewModel>();
            var comments = new Dictionary<string, string>();

            // Читаем список переменных
            var variableNames = App.Configuration.GetSection("EnvironmentVariables").Get<string[]>() ?? [];

            // Читаем комментарии из comments.json
            var commentsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileConstant.CommentsFile);
            if (File.Exists(commentsPath))
            {
                var json = File.ReadAllText(commentsPath);
                foreach (var item in (System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json))?.ToList() ?? new())
                    comments.Add(item.Key, item.Value);
            }

            LoadDataFlag = true;
            // Заполняем список
            foreach (var name in variableNames)
            {
                var value = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Environment", name, "") as string ?? string.Empty;
                var comment = comments.GetValueOrDefault(name, string.Empty);
                variables.Add(new EnvironmentVariablesViewModel(this)
                {
                    Name = name,
                    Value = value,
                    Comment = comment
                });
            }
            LoadDataFlag = false;

            return new ObservableCollection<EnvironmentVariablesViewModel>(variables);
        }
    }
}
