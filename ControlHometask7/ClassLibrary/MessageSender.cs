using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ClassLibrary
{   
    /// <summary>
    /// Класс отправляет пользователю различные виды текстовых сообщений.
    /// </summary>
    public class MessageSender
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
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="cancellationToken"></param>
        public MessageSender(ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            _botClient = botClient;
            _cancellationToken = cancellationToken;
        }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public MessageSender()
        {
            _botClient = new TelegramBotClient("7139961450:AAHSSQQtDB-kdffLYy4uxyxvJpR_u_BqR88");
        }
        
        /// <summary>
        /// Метод отправляет сообщение в чат chatId с текстом text, в ответ на сообщение messageId.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="chatId"></param>
        /// <param name="messageId"></param>
        public async Task SendReplyMessage(string text, long chatId, int messageId)
        {
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyToMessageId: messageId,
                cancellationToken: _cancellationToken);
        }
        
        /// <summary>
        /// Метод отправляет сообщение в чат chatId с текстом text, в ответ на сообщение messageId без меню.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="chatId"></param>
        /// <param name="messageId"></param>
        public async Task SendWithoutMarkup(string text, long chatId, int messageId)
        {
            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyToMessageId: messageId,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: _cancellationToken);
        }
    }
}