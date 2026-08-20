using Grpc.Core;
using Sms.Test;

namespace GrpcServer.Services
{
    /// <summary>
    /// Заглушка для клиента (якобы ответы от сервака)
    /// </summary>
    public class GreeterService : SmsTestService.SmsTestServiceBase
    {
        public override Task<GetMenuResponse> GetMenu(Google.Protobuf.WellKnownTypes.BoolValue request, ServerCallContext context)
        {
            var response = new GetMenuResponse
            {
                Success = true,
                ErrorMessage = ""
            };

            var menuItem1 = new MenuItem
            {
                Id = "5979224",
                Article = "A1004292",
                Name = "Каша гречневая",
                Price = 50,
                IsWeighted = false,
                FullPath = "ПРОИЗВОДСТВО\\Гарниры",
            };
            menuItem1.Barcodes.Add("57890975627974236429");

            var menuItem2 = new MenuItem
            {
                Id = "9084246",
                Article = "A1004293",
                Name = "Конфеты Коровка",
                Price = 300,
                IsWeighted = true,
                FullPath = "ДЕСЕРТЫ\\Развес"
            };

            response.MenuItems.Add(menuItem1);
            response.MenuItems.Add(menuItem2);

            return Task.FromResult(response);
        }

        public override Task<SendOrderResponse> SendOrder(Order request, ServerCallContext context)
        {
            return Task.FromResult(new SendOrderResponse
            {
                Success = true,
                ErrorMessage = ""
            });
        }
    }
}
