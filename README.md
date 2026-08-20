# SMS Тестовое задание

## Структура
![](https://github.com/MilkRen/SMS/blob/master/srcGitHub/project.png)

*   **Часть 1 (ConsoleApp, gRPC):** `ConsoleApp`, `DB.BAL`, `DB.Core`, `DB.DAL`, `GrpcClient`, `GrpcServer`
*   **Часть 2 (WPF):** `WpfApp`

## Запуск (Часть 1)

1.  Запустите PostgreSQL: `docker-compose up -d` (файл в корне решения)
2.  Запустите `GrpcServer`
3.  Запустите `ConsoleApp`

Результат 
![](https://github.com/MilkRen/SMS/blob/master/srcGitHub/prog1.png)

 
## Запуск (Часть 2)

1.  Запустите `WpfApp`

![](https://github.com/MilkRen/SMS/blob/master/srcGitHub/wpf.png)

### Пакеты:
1.  Aspire.Npgsql.EntityFrameworkCore.PostgreSQL
2.  Google.Protobuf
3.  Grpc.Net.Client
4.  Grpc.Tools
5.  Grpc.AspNetCore
6.  Microsoft.EntityFrameworkCore
7.  Microsoft.EntityFrameworkCore.Design
8.  Microsoft.EntityFrameworkCore.Tools
9.  Microsoft.Extensions.Configuration.Json"
10.  Microsoft.Extensions.DependencyInjection
11.  Microsoft.Extensions.Hosting
12.  Newtonsoft.Json
13.  Serilog.Extensions.Hosting
14.  Serilog.Sinks.File