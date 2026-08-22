using ConsoleApp.Constants;
using DB.BAL.Services;
using DB.BAL.Services.Interfaces;
using DB.Core.DTO;
using DB.Core.Enums;
using DB.DAL;
using DB.DAL.Repositories;
using DB.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Server.Helpers;
using Sms.Test;

namespace Server
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var logPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "test-sms-console-app",
                $"test-sms-console-app-{DateTime.Now:yyyyMMdd}.log");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath)
                .CreateLogger();

            var builder = Host.CreateApplicationBuilder(args);

            // логирование
            builder.Logging.AddSerilog(Log.Logger);

            builder.Configuration.AddJsonFile(FileConstant.ConfigurationFile, optional: false); // падаем с ошибкой, если файла нету

            #region БД

            builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Error); // чуть ограничим логи, чтобы без спама в консоле

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(ApplicationDbContext))));

            builder.Services.AddScoped<IMenuItemService, MenuItemService>();
            builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();

            #endregion

            // dll
            builder.Services.AddScoped<ClienGRPC>(provider =>
            {
                var grpcEndpoint = "https://localhost:7170";
                return new ClienGRPC(grpcEndpoint);
            });

            var host = builder.Build();

            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var clientGRPC = scope.ServiceProvider.GetRequiredService<ClienGRPC>();
            var menuItemService = scope.ServiceProvider.GetRequiredService<IMenuItemService>();

            try
            {
                await dbContext.Database.EnsureCreatedAsync(); // прогрев БД

                ConsolePlus.WriteLine($"Добро пожаловать!", ConsoleColor.Black, ConsoleColor.DarkGreen);
                ConsolePlus.WriteLine("БД инициализирована!", ConsoleColor.DarkGreen, ConsoleColor.Black);
                ConsolePlus.Write($"Введите команду или напишите команду '{CommandConstant.Help}': ");
                while (true)
                {
                    var readline = Convert.ToString(Console.ReadLine()).Trim().ToLower();
                    Log.Information($"Пользователь ввёл данные: {readline}");
                    switch (readline)
                    {
                        case CommandConstant.GetMenu:
                            await GetMenuAsync(clientGRPC, menuItemService);
                            break;
                        case CommandConstant.SendOrder:
                            await SendOrderAsync(clientGRPC, menuItemService);
                            break;
                        case CommandConstant.Clear:
                            Console.Clear();
                            break;
                        case CommandConstant.Help:
                            ConsolePlus.WriteLine($"Список команд:", ConsoleColor.Black, ConsoleColor.DarkYellow);
                            ConsolePlus.WriteLine($"'{CommandConstant.GetMenu}' - Получить список блюд и записать в БД", ConsoleColor.DarkYellow, ConsoleColor.Black);
                            ConsolePlus.WriteLine($"'{CommandConstant.SendOrder}' - Собрать и отправить заказ", ConsoleColor.DarkYellow, ConsoleColor.Black);
                            ConsolePlus.WriteLine($"'{CommandConstant.Clear}' - Очистить консоль", ConsoleColor.DarkYellow, ConsoleColor.Black);
                            ConsolePlus.WriteLine($"'{CommandConstant.Help}' - Справка с командами", ConsoleColor.DarkYellow, ConsoleColor.Black);
                            break;
                        default:
                            ConsolePlus.WriteLine($"Команда не найдена! ('{CommandConstant.Help}')", ConsoleColor.DarkRed);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ConsolePlus.WriteLine(ex.Message,ConsoleColor.DarkRed, ConsoleColor.Black, isLog: false);
                Log.Fatal(ex, "Ошибка при выполнении!");
            }
        }

        /// <summary>
        /// Получить список меню с сервера и записать в БД
        /// </summary>
        private static async Task GetMenuAsync(ClienGRPC clientGRPC, IMenuItemService menuItemService)
        {
            ConsolePlus.WriteLine("Отправляем команду на сервер GRPC");
            var menu = await clientGRPC.GetMenuAsync();
            if (!menu.Any())
                throw new ArgumentOutOfRangeException(nameof(menu), "Пустая коллекция");

            ConsolePlus.WriteLine("Получили список блюд и записываем в БД");
            var result = (ErrorDb)await menuItemService.CreateManyMenuAsync(menu.ConvertAll(x => new MenuItemDTO(x.Article, x.Name, x.Price, x.IsWeighted, x.FullPath, x.Barcodes.ToList())));
            if (result == ErrorDb.Fatal || result == ErrorDb.Undefined)
                throw new Exception("Не получилось создать запись в БД");

            var menuItem = await menuItemService.GetMenuAsync();
            await MenuWrite(menuItem);

            ConsolePlus.WriteLine("Вышли из команды!", ConsoleColor.DarkYellow, isLog: false);
        }

        /// <summary>
        /// Вывести список меню
        /// </summary>
        private static async Task MenuWrite(List<MenuItemDTO> menuItem)
        {
            ConsolePlus.WriteLine("Вывод списка блюд из БД");
            ConsolePlus.WriteLine("Название – Код (артикул) – Цена за единицу");
            foreach (var item in menuItem)
                ConsolePlus.WriteLine($"{item.Name} – {item.Article} – {item.Price}");
        }

        /// <summary>
        /// Отправить заказ с блюдами
        /// </summary>
        private static async Task SendOrderAsync(ClienGRPC clientGRPC, IMenuItemService menuItemService)
        {
            var menuItem = await menuItemService.GetMenuAsync();
            await MenuWrite(menuItem);

            while (true)
            {
                var isValid = true;
                ConsolePlus.WriteLine("Введите данные для меню в формате \"Код1:Количество1;Код2:Количество2;Код3:Количество3;...\"");
                var menuLine = Convert.ToString(Console.ReadLine()).Trim();
                if (string.IsNullOrEmpty(menuLine))
                {
                    ConsolePlus.WriteLine("Вышли из команды!", ConsoleColor.DarkRed);
                    break;
                }

                var menuElement = menuLine.Split(";", StringSplitOptions.RemoveEmptyEntries);
                if (menuElement is null)
                {
                    ConsolePlus.WriteLine("Некорректный формат", ConsoleColor.DarkRed);
                    break;
                }

                var menuElementList = new List<(string code, double quantity)>();
                for (var i = 0; i < menuElement.Length; i++)
                {
                    var element = menuElement[i].Split(":", StringSplitOptions.RemoveEmptyEntries);
                    if (element.Length != 2)
                    {
                        var textError = $"Некорректный формат: '{string.Join(":", element)}'";
                        ConsolePlus.WriteLine(textError, ConsoleColor.DarkRed, isLog: false);
                        Log.Error(textError);
                        isValid = false;
                        break;
                    }

                    var code = element[0].Trim();
                    if (!double.TryParse(element[1].Trim(), out var quantity) || quantity <= 0)
                    {
                        var textError = $"Некорректное количество: '{element[1]}' для кода '{code}'. Должно быть > 0";
                        ConsolePlus.WriteLine(textError, ConsoleColor.DarkRed, isLog: false);
                        Log.Error(textError);
                        isValid = false;
                    }

                    if (!menuItem.Any(x => x.Article == code))
                    {
                        var textError = $"Блюдо с кодом '{code}' не найдено.";
                        ConsolePlus.WriteLine(textError, ConsoleColor.DarkRed, isLog: false);
                        Log.Error(textError);
                        isValid = false;
                    }

                    if (isValid)
                        menuElementList.Add(new(code, quantity));
                }

                if (!isValid)
                {
                    ConsolePlus.WriteLine("Повторите ввод!", ConsoleColor.DarkYellow);
                    continue;
                }

                var newOrder = new Order();
                foreach (var item in menuElementList)
                {
                    var newOrderItem = new OrderItem()
                    {
                        Id = item.code,
                        Quantity = item.quantity,
                    };
                    newOrder.OrderItems.Add(newOrderItem);
                }

                if (await clientGRPC.SendOrderAsync(newOrder))
                    ConsolePlus.WriteLine("УСПЕХ", ConsoleColor.DarkGreen);

                ConsolePlus.WriteLine("Вышли из команды!", ConsoleColor.DarkYellow, isLog: false);
                break;
            }
        }
    }
}
