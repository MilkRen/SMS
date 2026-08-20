using Grpc.Net.Client;

namespace Sms.Test
{
    /// <summary>
    /// Логика клиента gRPC
    /// </summary>
    public class ClienGRPC
    {
        private readonly SmsTestService.SmsTestServiceClient _client; // для отправки запросов

        /// <summary>
        /// Отправка запросов
        /// </summary>
        /// <param name="grpcEndpoint">Адрес подключения</param>
        public ClienGRPC(string grpcEndpoint)
        {
            var channel = GrpcChannel.ForAddress(grpcEndpoint);
            _client = new SmsTestService.SmsTestServiceClient(channel);
        }

        /// <summary>
        /// Получить список блюд
        /// </summary>
        /// <param name="withPrice">Указать цену?</param>
        /// <returns>Лист блюд</returns>
        /// <exception cref="Exception">Сервер вернул отрицательный флаг выполнения</exception>
        public async Task<List<MenuItem>> GetMenuAsync(bool withPrice = true)
        {
            var request = new Google.Protobuf.WellKnownTypes.BoolValue
            {
                Value = withPrice
            };
            var response = await _client.GetMenuAsync(request);
            if (!response.Success)
                throw new Exception($"Ошибка: {response?.ErrorMessage ?? "Проблема с сервером"}");

            return response.MenuItems.ToList();
        }

        /// <summary>
        /// Отправить заказ
        /// </summary>
        /// <param name="order">Заказ</param>
        /// <returns>Выполнено?</returns>
        /// <exception cref="Exception">Сервер вернул отрицательный флаг выполнения</exception>
        public async Task<bool> SendOrderAsync(Order order)
        {
            var response = await _client.SendOrderAsync(order);
            if (!response.Success)
                throw new Exception($"Ошибка: {response?.ErrorMessage ?? "Проблема с сервером"}");

            return response.Success;
        }
    }
}
