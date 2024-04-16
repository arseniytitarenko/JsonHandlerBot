using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ClassLibrary
{
    /// <summary>
    /// Класс отправляет пользователю различные виды меню.
    /// </summary>
    public class MenuSender 
    { 
        /// <summary>
        /// Клиент бота.
        /// </summary>
        private readonly ITelegramBotClient _botClient;

        /// <summary>
        /// Токен отмены.
        /// </summary>
        private readonly CancellationToken _cancellationToken;

        /// <summary>
        /// Конструктор с парметрами.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="cancellationToken"></param>
        public MenuSender(ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            _botClient = botClient;
            _cancellationToken = cancellationToken;
        }
        
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public MenuSender()
        {
            _botClient = new TelegramBotClient("7139961450:AAHSSQQtDB-kdffLYy4uxyxvJpR_u_BqR88");
        }
    
        /// <summary>
        /// Метод отправляет в чат chatId меню с выбором типа файла.
        /// </summary>
        /// <param name="chatId"></param>
        public async Task AskTypeFile(long chatId)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
                            {
                    new KeyboardButton[] { "JSON", "CSV" },
                })
            {
                ResizeKeyboard = true
            };
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите формат файла",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: _cancellationToken);
        }
        
        /// <summary>
        /// Метод отправляет в чат chatId меню с выбором типа сортировки.
        /// </summary>
        /// <param name="chatId"></param>
        public async Task AskTypeSorting(long chatId)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "TimeStart в порядке увеличения времени" },
                new KeyboardButton[] { "TimeEnd в порядке увеличения времени" }
            })
            {
                ResizeKeyboard = true
            };
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите тип фильтрации",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: _cancellationToken);
        }
        
        /// <summary>
        /// Метод отправляет в чат chatId меню с выбором типа фильтра.
        /// </summary>
        /// <param name="chatId"></param>
        public async Task AskTypeFilter(long chatId)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "StationStart", "StationEnd" },
                new KeyboardButton[] { "StationStart и StationEnd" }
            })
            {
                ResizeKeyboard = true
            };
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите тип сортировки",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: _cancellationToken);
        }
        
        /// <summary>
        /// Метод отправляет в чат chatId меню с выбором основного меню.
        /// </summary>
        /// <param name="chatId"></param>
        public async Task AskMenu(long chatId)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "Произвести выборку", "Отсортировать" },
                new KeyboardButton[] { "Загрузить файл на обработку" },
                new KeyboardButton[] { "Скачать обработанный файл" }
            })
            {
                ResizeKeyboard = true
            };
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите пункт меню",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: _cancellationToken);
        }
    }
}