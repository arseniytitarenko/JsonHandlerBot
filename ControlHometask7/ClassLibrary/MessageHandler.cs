using Serilog;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace ClassLibrary
{
    /// <summary>
    /// Класс обрабатывает сообщения пользователя.
    /// </summary>
    public class MessageHandler
    {
        /// <summary>
        /// Поле для отправки сообщений.
        /// </summary>
        private readonly MessageSender _messageSender;

        /// <summary>
        /// Поле для отправки меню.
        /// </summary>
        private readonly MenuSender _menuSender;

        /// <summary>
        /// Поле с данными о пользователях.
        /// </summary>
        private readonly Dictionary<long, UserData> _users;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="messageSender"></param>
        /// <param name="menuSender"></param>
        /// <param name="users"></param>
        public MessageHandler(MessageSender messageSender,
            MenuSender menuSender, Dictionary<long, UserData> users)
        {
            _messageSender = messageSender;
            _menuSender = menuSender;
            _users = users;
        }

        /// <summary>
        /// Пустой коструктор.
        /// </summary>
        public MessageHandler()
        {
            _menuSender = new MenuSender();
            _messageSender = new MessageSender();
            _users = new Dictionary<long, UserData>();
        }

        /// <summary>
        /// Метод обрабатывает сообщения и отвечает на них в зависимости от текста сообщения и уровня, на котором находится каждый пользователь.
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message) return;
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            // Работа логгера.
            if (update.Message.Text is { } text)
                Log.Information($"At {DateTime.Now} received message in chat {chatId}: {text}");
            if (update.Message.Document is { })
                Log.Information($"At {DateTime.Now} received document in chat {chatId}");

            // Добавление нового чата в словарь.
            if (!_users.ContainsKey(chatId)) { _users.Add(chatId, new UserData()); }

            // Перезапуск бота.
            if (message.Text == "/start")
            {
                await _menuSender.AskTypeFile(chatId);
                _users[chatId].LevelType = Level.Start;
                return;
            }

            switch (_users[chatId].LevelType)
            {
                case Level.Start when message.Text == "CSV" || message.Text == "JSON":
                    await _messageSender.SendWithoutMarkup("Отправьте файл в выбранном формате", chatId, messageId);
                    _users[chatId].LevelType = Level.Send;
                    _users[chatId].FileType = message.Text == "CSV" ? TypeFile.Csv : TypeFile.Json;
                    break;
                // Получение от пользователя файла.
                case Level.Send when message.Document is { } doc:
                    {
                        await using MemoryStream ms = new();
                        await bot.GetInfoAndDownloadFileAsync(
                            fileId: doc.FileId,
                            destination: ms,
                            cancellationToken: cancellationToken);
                        IProcessable processing = _users[chatId].FileType == TypeFile.Csv ? new CsvProcessing() : new JsonProcessing();
                        try
                        {
                            _users[chatId].Aeroexpresses = processing.Read(ms);
                            _users[chatId].LevelType = Level.Choice;
                            await _menuSender.AskMenu(chatId);
                        }
                        catch (ArgumentException e)
                        {
                            await _messageSender.SendReplyMessage(e.Message, chatId, messageId);
                        }
                        catch (System.Text.Json.JsonException)
                        {
                            await _messageSender.SendReplyMessage("Формат вашего файла неверен", chatId, messageId);
                        }
                        break;
                    }
                case Level.Choice when message.Text == "Загрузить файл на обработку":
                    await _menuSender.AskTypeFile(chatId);
                    _users[chatId].LevelType = Level.Start;
                    break;
                case Level.Choice when message.Text == "Произвести выборку":
                    await _menuSender.AskTypeFilter(chatId);
                    _users[chatId].LevelType = Level.Filter;
                    break;
                case Level.Choice when message.Text == "Отсортировать":
                    await _menuSender.AskTypeSorting(chatId);
                    _users[chatId].LevelType = Level.Sort;
                    break;
                case Level.Choice when message.Text == "Скачать обработанный файл":
                    await _menuSender.AskTypeFile(chatId);
                    _users[chatId].LevelType = Level.Get;
                    break;
                case Level.Sort when message.Text == "TimeStart в порядке увеличения времени":
                    _users[chatId].Aeroexpresses = HandlerAero.Sort(_users[chatId].Aeroexpresses, true);
                    await _messageSender.SendReplyMessage("Файл отсортирован по TimeStart в порядке увеличения времени", chatId, messageId);               
                    await _menuSender.AskMenu(chatId);
                    _users[chatId].LevelType = Level.Choice;
                    break;
                case Level.Sort when message.Text == "TimeEnd в порядке увеличения времени":
                    _users[chatId].Aeroexpresses = HandlerAero.Sort(_users[chatId].Aeroexpresses, true);
                    await _messageSender.SendReplyMessage("Файл отсортирован по TimeEnd в порядке увеличения времени", chatId, messageId); 
                    await _menuSender.AskMenu(chatId);
                    _users[chatId].LevelType = Level.Choice;
                    break;
                case Level.Filter when message.Text == "StationStart" || message.Text == "StationStart и StationEnd":
                    await _messageSender.SendWithoutMarkup("Введите значение поля StationStart", chatId, messageId);
                    _users[chatId].LevelType = Level.GetFirstFilter;
                    _users[chatId].FilterType = message.Text == "StationStart" ? TypeFilter.StationStart : TypeFilter.StartEnd;
                    break;
                case Level.Filter when message.Text == "StationEnd":
                    await _messageSender.SendWithoutMarkup("Введите значение поля StationEnd", chatId, messageId);
                    _users[chatId].LevelType = Level.GetSecondFilter;
                    _users[chatId].FilterType = TypeFilter.StationEnd;
                    break;
                case Level.GetFirstFilter:
                    try
                    {
                        _users[chatId].Aeroexpresses = HandlerAero.Filter(_users[chatId].Aeroexpresses, true, false, message.Text ?? "", "");
                    }
                    catch (ArgumentException e)
                    {
                        await _messageSender.SendWithoutMarkup(e.Message, chatId, messageId);
                        return;
                    }
                    await _messageSender.SendReplyMessage("Произведена фильтрация по StationStart", chatId, messageId);
                    if (_users[chatId].FilterType == TypeFilter.StationStart)
                    {
                        _users[chatId].LevelType = Level.Choice;
                        await _menuSender.AskMenu(chatId);
                    }
                    else
                    {
                        _users[chatId].LevelType = Level.GetSecondFilter;
                        await _messageSender.SendWithoutMarkup("Введите значение поля StationEnd", chatId, messageId);
                    }
                    break;
                case Level.GetSecondFilter:
                    try
                    {
                        _users[chatId].Aeroexpresses = HandlerAero.Filter(_users[chatId].Aeroexpresses, false, true, "", message.Text ?? "");
                    }
                    catch (ArgumentException e)
                    {
                        await _messageSender.SendReplyMessage(e.Message, chatId, messageId);
                        return;
                    }
                    await _messageSender.SendReplyMessage("Произведена фильтрация по StationEnd", chatId, messageId);
                    _users[chatId].LevelType = Level.Choice;
                    await _menuSender.AskMenu(chatId);
                    break;
                // Отправка файла пользователю.
                case Level.Get when message.Text == "CSV" || message.Text == "JSON":
                    {
                        IProcessable processing = message.Text == "CSV" ? new CsvProcessing() : new JsonProcessing();
                        string typeFile = message.Text == "CSV" ? "csv" : "json";
                        try
                        {
                            await using Stream stream = processing.Write(_users[chatId].Aeroexpresses);
                            _ = await bot.SendDocumentAsync(
                                chatId: chatId,
                                document: InputFile.FromStream(stream: stream, fileName: $"aeroexpress.{typeFile}"),
                                caption: typeFile.ToUpper(), cancellationToken: cancellationToken);
                            _users[chatId].LevelType = Level.Choice;
                            await _menuSender.AskMenu(chatId);
                        }
                        catch (ArgumentException e)
                        {
                            await _messageSender.SendReplyMessage(e.Message, chatId, messageId);
                        }
                        break;
                    }
                default:
                    await _messageSender.SendReplyMessage("У меня нет такой команды", chatId, messageId);
                    break;                
            }
        }

        /// <summary>
        /// Метод обрабатывающий ошибки.
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandlePollingErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
}