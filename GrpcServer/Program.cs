using GrpcServer.Services;

namespace GrpcServer
{
    /// <summary>
    /// Заглушка для клиента (якобы ответы от сервака)
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddGrpc();

            var app = builder.Build();

            app.MapGrpcService<GreeterService>();

            app.Run();
        }
    }
}