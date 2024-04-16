using ClassLibrary;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Serilog;

namespace TgBot
{
    static class Program
    {
        static async Task Main()
        {
            // Создание папки для логов.
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "var");
            Directory.CreateDirectory(logDirectory);
            // Создание логгера.
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(logDirectory, "messages.log"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
            
            var botClient = new TelegramBotClient("");
            using CancellationTokenSource cts = new();
            
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() 
            };

            // Пользователи бота.
            Dictionary<long, UserData> users = new();
            var messageSender = new MessageSender(botClient, cts.Token);
            var menuSender = new MenuSender(botClient, cts.Token);
            MessageHandler handler = new(messageSender, menuSender, users);

            botClient.StartReceiving(
                updateHandler: handler.HandleUpdateAsync,
                pollingErrorHandler: handler.HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync(cancellationToken: cts.Token);
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            cts.Cancel();
        }        
    }
}