using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static SchoolBot.DaysOfWeek;
using static SchoolBot.MealType;


namespace SchoolBot;

public static class BotClient
{
    private const string BackButton = "Назад   ◀️";

    private static readonly ITelegramBotClient Bot =
        new TelegramBotClient("5735097045:AAGyp2lMa72zKg2PTkLke-bFI7DS7zpu7xI");

    // Key: UserID, Value: selected Day
    private static readonly Dictionary<string, string?> DaySelectedByUser = new Dictionary<string, string?>();


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
                    $"UserID: {JsonConvert.SerializeObject(update.CallbackQuery?.From.Username)}; Nickname: " +
                    $"{JsonConvert.SerializeObject(update.CallbackQuery?.From.FirstName)} " +
                    $"{JsonConvert.SerializeObject(update.CallbackQuery?.From.LastName)}");

                var requestFormatter = new RequestFormatter();
                var userId = JsonConvert.SerializeObject(update.CallbackQuery?.From.Username);
                var pressedButtonData =
                    update.CallbackQuery?.Data ?? throw new ArgumentException("Nobody pressed the button");

                Console.WriteLine($"Pressed button = {pressedButtonData}");

                if (pressedButtonData == BackButton)
                {
                    // забыть день, выбранный до этого момента
                    DaySelectedByUser.Remove(userId);

                    await EditSentMessageAndMarkup(update.CallbackQuery?.Message!, "Выберите день",
                        GetInlineKeyboardWithDays());
                    break;
                }

                if (DaysOfWeekExtensions.ContainsButton(pressedButtonData))
                {
                    // Здесь надо запомнить pressedButtonData - какой день выбрал пользователь
                    DaySelectedByUser[userId] = pressedButtonData;

                    await EditSentMessageAndMarkup(update.CallbackQuery?.Message!, "Выберите время",
                        GetInlineKeyboardWithTimeOfMeal());
                    break;
                }

                if (MealTypeExtensions.ContainsButton(pressedButtonData))
                {
                    // Запомнить MealType в словарь
                    if (DaySelectedByUser.ContainsKey(userId))
                    {
                        requestFormatter.UpdateDay(DaySelectedByUser[userId]);
                        requestFormatter.UpdateMealType(pressedButtonData);
                    }
                    else
                    {
                        await SendExceptionMessage(update.CallbackQuery?.Message?.Chat.Id!);
                        break;
                    }

                    await SendTextMessage(update.CallbackQuery?.Message?.Chat.Id!,
                        SqlRequest.GetAnswer(requestFormatter));
                }

                break;
            }
            case UpdateType.Message:
            {
                var message = update.Message;
                Console.WriteLine($"UserID: {JsonConvert.SerializeObject(update.Message?.From?.Username)}; Nickname: " +
                                  $"{JsonConvert.SerializeObject(update.Message?.From?.FirstName)} " +
                                  $"{JsonConvert.SerializeObject(update.Message?.From?.LastName)}");
                Console.WriteLine($"Message: {message?.Text}");

                switch (message?.Text?.ToLower())
                {
                    case "/start":
                        await SendTextMessage(update.Message!.Chat.Id,
                            "Нажмите на кнопку или отправьте в чат 'Узнать меню'", GetReplyKeyboardWithSchedule());
                        break;
                    
                    case "узнать меню":
                        await SendTextMessage(update.Message!.Chat.Id,
                            "Выберите день", GetInlineKeyboardWithDays());
                        break;
                    
                    default:
                        await SendTextMessage(update.Message!.Chat.Id,
                            "Нажмите на кнопку или отправьте в чат 'Узнать меню'", GetReplyKeyboardWithSchedule());
                        break;
                }

                break;
            }
        }

        async Task EditSentMessageAndMarkup(Message message, string textMessage, InlineKeyboardMarkup markup)
        {
            await botClient.EditMessageTextAsync(
                chatId: message.Chat.Id,
                messageId: message.MessageId,
                text: textMessage,
                cancellationToken: cancellationToken);

            await botClient.EditMessageReplyMarkupAsync(
                chatId: message.Chat.Id,
                messageId: message.MessageId,
                cancellationToken: cancellationToken,
                replyMarkup: markup);
        }

        async Task SendExceptionMessage(ChatId chatId)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Ой, что-то пошло не так... Повторите попытку",
                cancellationToken: cancellationToken);
        }

        async Task SendTextMessage(ChatId chatId, string text, IReplyMarkup? replyMarkup = null)
        {
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                cancellationToken: cancellationToken,
                replyMarkup: replyMarkup);
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
        })
        {
            ResizeKeyboard = true
        };
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