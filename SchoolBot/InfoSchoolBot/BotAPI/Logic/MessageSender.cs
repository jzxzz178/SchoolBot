using SchoolBot.BotAPI.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SchoolBot.BotAPI.Logic;

public class MessageSender : ISendMessage
{
    private readonly ITelegramBotClient bot;
    private readonly CancellationToken cancellationToken;

    public MessageSender(ITelegramBotClient bot, CancellationToken cancellationToken)
    {
        this.bot = bot;
        this.cancellationToken = cancellationToken;
    }

    public async Task EditSentMessageAndMarkup(Message message, string textMessage, InlineKeyboardMarkup markup)
    {
        await bot.EditMessageTextAsync(
            chatId: message.Chat.Id,
            messageId: message.MessageId,
            text: textMessage,
            parseMode: ParseMode.Markdown,
            cancellationToken: cancellationToken);

        await bot.EditMessageReplyMarkupAsync(
            chatId: message.Chat.Id,
            messageId: message.MessageId,
            cancellationToken: cancellationToken,
            replyMarkup: markup);
    }

    public async Task SendExceptionMessage(ChatId chatId)
    {
        await bot.SendTextMessageAsync(
            chatId: chatId,
            text: "Ой, что-то пошло не так... Повторите попытку",
            cancellationToken: cancellationToken);
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
            cancellationToken: cancellationToken,
            replyMarkup: replyMarkup);
    }
}