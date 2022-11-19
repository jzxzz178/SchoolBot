using System.Diagnostics;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static SchoolBot.DaysOfWeek;
using static SchoolBot.MealType;


namespace SchoolBot;

public class BotClient
{
    private const string BackButton = "Назад   ◀️";

    private static readonly ITelegramBotClient Bot =
        new TelegramBotClient("5735097045:AAGyp2lMa72zKg2PTkLke-bFI7DS7zpu7xI");


    public static void StartBot()
    {
        Console.WriteLine("Запущен бот " + Bot.GetMeAsync().Result.FirstName);

        var cancellationToken = new CancellationTokenSource().Token;
        var receiverOptions = new ReceiverOptions();

        Bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
    }


    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        Console.WriteLine();
        Console.WriteLine($"Type of request: {JsonConvert.SerializeObject(update.Type)}");

        switch (update.Type)
        {
            case UpdateType.CallbackQuery:
            {
                Console.WriteLine(
                    $"User: {JsonConvert.SerializeObject(update.CallbackQuery?.From.Username)}; Nickname: " +
                    $"{JsonConvert.SerializeObject(update.CallbackQuery?.From.FirstName)} " +
                    $"{JsonConvert.SerializeObject(update.CallbackQuery?.From.LastName)}");

                var pressedButtonData =
                    update.CallbackQuery?.Data ?? throw new ArgumentException("Nobody pressed the button");

                Console.WriteLine($"Pressed button = {pressedButtonData}");

                if (pressedButtonData == BackButton)
                {
                    // забыть день, выбранный до этого момента

                    await EditSentMessageAndMarkup("Выберите день", GetInlineKeyboardWithDays());
                    break;
                }

                if (DaysOfWeekExtensions.ContainsButton(pressedButtonData))
                {
                    // Здесь надо запомнить pressedButtonData - какой день выбрал пользователь

                    await EditSentMessageAndMarkup("Выберите время", GetInlineKeyboardWithTimeOfMeal());
                    break;
                }

                if (MealTypeExtensions.ContainsButton(pressedButtonData))
                {
                    await botClient.SendTextMessageAsync(
                        chatId: update.CallbackQuery?.Message?.Chat.Id ??
                                throw new InvalidOperationException("Chat.Id was null"),
                        text: pressedButtonData,
                        cancellationToken: cancellationToken);
                }
                
                break;
            }
            case UpdateType.Message:
            {
                var message = update.Message;
                Console.WriteLine($"User: {JsonConvert.SerializeObject(update.Message?.Chat)}, ");
                Console.WriteLine($"Message: {message?.Text}");

                if (message?.Text?.ToLower() == "/start")
                {
                    Debug.Assert(update.Message != null, "update.Message == null");
                    await botClient.SendTextMessageAsync(update.Message.Chat,
                        "Нажмите на кнопку или отправьте в чат 'Узнать расписание'",
                        ParseMode.MarkdownV2,
                        cancellationToken: cancellationToken,
                        replyMarkup: GetReplyKeyboardWithSchedule());
                }

                else if (message?.Text?.ToLower() == "узнать меню")
                {
                    Debug.Assert(update.Message != null, "update.Message == null");

                    await botClient.SendTextMessageAsync(update.Message.Chat,
                        "Выберите день",
                        ParseMode.MarkdownV2,
                        cancellationToken: cancellationToken,
                        replyMarkup: GetInlineKeyboardWithDays());
                }

                else
                {
                    Debug.Assert(update.Message != null, "update.Message == null");

                    await botClient.SendTextMessageAsync(update.Message.Chat,
                        "Нажмите на кнопку или отправьте в чат 'Узнать меню'",
                        ParseMode.MarkdownV2,
                        cancellationToken: cancellationToken,
                        replyMarkup: GetReplyKeyboardWithSchedule());
                }

                break;
            }
        }

        async Task EditSentMessageAndMarkup(string textMessage, InlineKeyboardMarkup markup)
        {
            await botClient.EditMessageTextAsync(
                chatId: update.CallbackQuery?.Message?.Chat.Id
                        ?? throw new InvalidOperationException("Chat.Id was null"),
                messageId: update.CallbackQuery.Message.MessageId,
                text: textMessage,
                cancellationToken: cancellationToken);

            await botClient.EditMessageReplyMarkupAsync(
                chatId: update.CallbackQuery?.Message?.Chat.Id ??
                        throw new InvalidOperationException("Chat.Id was null"),
                messageId: update.CallbackQuery.Message.MessageId,
                cancellationToken: cancellationToken,
                replyMarkup: markup);
        }
    }

    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(JsonConvert.SerializeObject(exception));
        return Task.CompletedTask;
    }


    private static ReplyKeyboardMarkup GetReplyKeyboardWithSchedule()
    {
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[]
        {
            new[]
            {
                new KeyboardButton("Узнать меню"),
            }
        });
        replyKeyboardMarkup.ResizeKeyboard = true;
        return replyKeyboardMarkup;
    }

    private static InlineKeyboardMarkup GetInlineKeyboardWithDays()
    {
        var inlineKeyboardDays = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Today.GetDescription())
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Monday.GetDescription()),
                InlineKeyboardButton.WithCallbackData(Tuesday.GetDescription()),
                InlineKeyboardButton.WithCallbackData(Wednesday.GetDescription()),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Thursday.GetDescription()),
                InlineKeyboardButton.WithCallbackData(Friday.GetDescription()),
            },
        });

        return inlineKeyboardDays;
    }

    private static InlineKeyboardMarkup GetInlineKeyboardWithTimeOfMeal()
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Breakfast.GetDescription()),
                InlineKeyboardButton.WithCallbackData(Lunch.GetDescription())
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(BackButton),
            },
        });

        return inlineKeyboard;
    }
}