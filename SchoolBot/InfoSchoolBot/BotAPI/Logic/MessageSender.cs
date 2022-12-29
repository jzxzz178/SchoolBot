using SchoolBot.BotAPI.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SchoolBot.BotAPI.Logic;

public class MessageSender : ISendMessage
{
    private readonly ITelegramBotClient bot;
    
    public MessageSender(ITelegramBotClient bot)
    {
        this.bot = bot;
    }

    public async Task EditSentMessageAndMarkup(Message message, string textMessage, InlineKeyboardMarkup markup)
    {
        await bot.EditMessageTextAsync(
            chatId: message.Chat.Id,
            messageId: message.MessageId,
            text: textMessage,
            parseMode: ParseMode.Markdown);

        await bot.EditMessageReplyMarkupAsync(
            chatId: message.Chat.Id,
            messageId: message.MessageId,
            replyMarkup: markup);
    }

    public async Task SendExceptionMessage(ChatId chatId)
    {
        await bot.SendTextMessageAsync(
            chatId: chatId,
            text: "Ой, что-то пошло не так... Повторите попытку");
    }

    public async Task SendTextMessage(ChatId chatId, string? text, IReplyMarkup? replyMarkup = null)
    {
        if (text == null)
        {
            await SendExceptionMessage(chatId);
            return;
        }

        await bot.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            parseMode: ParseMode.Markdown,
            replyMarkup: replyMarkup);
    }
}