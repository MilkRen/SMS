using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO;
using System.Windows;
using WpfApp.Constants;
using WpfApp.Services;
using WpfApp.Services.Interfaces;

namespace ClientUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IConfiguration Configuration { get; private set; } = null!;

        public static ILogger Logger { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Configuration = new ConfigurationBuilder()
                .AddJsonFile(FileConstant.ConfigurationFile, optional: true)
                .Build();

            var logPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "test-sms-wpf-app",
                $"test-sms-wpf-app-{DateTime.Now:yyyyMMdd}.log");

            Logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Logger = Logger;

            Logger.Information("Загрузка данных переменных окружений");
            IJsonConfigurationServices configurationServices = new JsonConfigurationServices();
            configurationServices.LoadData();

            Logger.Information("Открытие главного окна");
            IDialogWindowService windowService = new DialogWindowService();
            windowService.OpenMainWindow();
        }
    }
}
