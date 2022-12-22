using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SchoolBot.BotAPI.Interfaces;

public interface ISendMessage
{
    Task SendTextMessage(ChatId chatId, string? text, IReplyMarkup? replyMarkup = null);
    Task SendExceptionMessage(ChatId chatId);
    Task EditSentMessageAndMarkup(Message message, string textMessage, InlineKeyboardMarkup markup);
}