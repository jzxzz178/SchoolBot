using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SchoolBot.DbWork;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static SchoolBot.DaysOfWeek;
using static SchoolBot.MealType;


namespace SchoolBot;

public interface IBot
{
    void Run();
    /*void HandleUpdate();
    void HandleError();*/
}

public class BotClient : IBot
{
    private readonly ILogger<IBot> log;
    private readonly IConfiguration config; 
    
    private const string BackButton = "Назад   ◀️";

    private static readonly ITelegramBotClient Bot =
        new TelegramBotClient("5628215183:AAFeTuAoxldhloxF8yFzgUaC3GK04w28hOk");

    // Key: UserID, Value: selected Day
    private static readonly Dictionary<string, string?> DaySelectedByUser = new Dictionary<string, string?>();

    public BotClient(ILogger<IBot> log, IConfiguration config)
    {
        this.log = log;
        this.config = config;
    }

    public void Run()
    {
        var botName = Bot.GetMeAsync().Result.FirstName;
        log.LogInformation("Запущен бот " + botName);

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


    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        // Console.WriteLine();
        var request = JsonConvert.SerializeObject(update.Type);


        switch (update.Type)
        {
            case UpdateType.CallbackQuery:
            {
                var requestFormatter = new RequestFormatter();
                var userId = JsonConvert.SerializeObject(update.CallbackQuery?.From.Username);
                var pressedButtonData = update.CallbackQuery?.Data;

                log.LogInformation(
                    $"UserID: {userId}; Nickname: " +
                    $"{JsonConvert.SerializeObject(update.CallbackQuery?.From.FirstName)} " +
                    $"{JsonConvert.SerializeObject(update.CallbackQuery?.From.LastName)}");


                log.LogInformation($"Pressed button = {pressedButtonData}");

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

                    // Console.WriteLine(SqlRequest.GetAnswer(requestFormatter));
                    await SendTextMessage(update.CallbackQuery?.Message?.Chat.Id!,
                        SqlRequest.GetAnswer(requestFormatter));

                    // Эта строка нужна для того, чтобы после нажатия на кнопку исчезала
                    // анимация подгрузки в виде часов на этой кнопке
                    await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery!.Id,
                        cancellationToken: cancellationToken);
                }

                break;
            }
            case UpdateType.Message:
            {
                var message = update.Message;
                var userId = JsonConvert.SerializeObject(update.Message?.From?.Username);

                DataBaseLog.Logger(userId, request);
                // manager.AddLog(userId, request);
                log.LogInformation(userId, request);

                userId = userId.Remove(0, 1).Remove(userId.Length - 2, 1);
                log.LogInformation($"UserID: {userId}; Nickname: " +
                                   $"{JsonConvert.SerializeObject(update.Message?.From?.FirstName)} " +
                                   $"{JsonConvert.SerializeObject(update.Message?.From?.LastName)}");
                log.LogInformation($"Message: {message?.Text}");

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
                parseMode: ParseMode.Markdown,
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

        async Task SendTextMessage(ChatId chatId, string? text, IReplyMarkup? replyMarkup = null)
        {
            if (text == null)
            {
                await SendExceptionMessage(chatId);
                return;
            }

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                parseMode: ParseMode.Markdown,
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