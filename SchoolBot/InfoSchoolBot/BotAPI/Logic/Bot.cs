using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SchoolBot.BotAPI.Buttons;
using SchoolBot.BotAPI.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static SchoolBot.BotAPI.Buttons.Buttons;
using static SchoolBot.BotAPI.Interfaces.IConstants;

namespace SchoolBot.BotAPI.Logic;

public class Bot : IBot
{
    private readonly ITelegramBotClient bot;
    private readonly IMenuManager menuManager;
    private readonly CancellationToken cancellationToken;
    public ISendMessage MessageSender { get; set; }
    public IButtons ButtonCreate { get; set; }

    // Key: UserID, Value: selected Day
    private static readonly Dictionary<string, string?> DaySelectedByUser = new Dictionary<string, string?>();
    private readonly ILogger logger;

    public Bot(IButtons buttonCreate, IMenuManager menuManager, ILogger<Bot> logger, IConfiguration configuration)
    {
        ButtonCreate = buttonCreate;
        this.menuManager = menuManager;
        this.logger = logger;
        cancellationToken = new CancellationTokenSource().Token;
        bot = new TelegramBotClient(configuration.GetValue<string>("BOT_API_TOKEN")!);
        MessageSender = new MessageSender(bot, cancellationToken);
    }

    public void Run()
    {
        var botName = bot.GetMeAsync(cancellationToken: cancellationToken).Result.FirstName;
        logger.LogInformation("Запущен бот {Name}", botName);

        var receiverOptions = new ReceiverOptions();

        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancelToken)
    {
        var request = JsonConvert.SerializeObject(update.Type);
        switch (update.Type)
        {
            case UpdateType.Message:
            {
                var message = update.Message;
                var userId = JsonConvert.SerializeObject(update.Message?.From?.Username);
                userId = userId.Remove(0, 1).Remove(userId.Length - 2, 1);
                logger.LogInformation("{Id}: {Request}", userId, request);
                switch (message?.Text?.ToLower())
                {
                    case "/start":
                        await MessageSender.SendTextMessage(update.Message!.Chat.Id,
                            "Нажмите на кнопку или отправьте в чат 'Узнать меню'", GetReplyKeyboardWithSchedule());
                        break;

                    case "узнать меню":
                        await MessageSender.SendTextMessage(update.Message!.Chat.Id,
                            "Выберите день", GetInlineKeyboardWithDays());
                        break;

                    default:
                        await MessageSender.SendTextMessage(update.Message!.Chat.Id,
                            "Нажмите на кнопку или отправьте в чат 'Узнать меню'", GetReplyKeyboardWithSchedule());
                        break;
                }

                break;
            }
            case UpdateType.CallbackQuery:
            {
                var userId = JsonConvert.SerializeObject(update.CallbackQuery?.From.Username);
                userId = userId.Remove(0, 1).Remove(userId.Length - 2, 1);
                var pressedButtonData = update.CallbackQuery?.Data;
                logger.LogInformation("{Id}: {Request}", userId, request);
                var requestFormatter = new RequestFormatter();

                if (pressedButtonData == BackButton)
                {
                    // забыть день, выбранный до этого момента
                    DaySelectedByUser.Remove(userId);

                    await MessageSender.EditSentMessageAndMarkup(update.CallbackQuery?.Message!, "Выберите день",
                        GetInlineKeyboardWithDays());
                    break;
                }

                if (DaysOfWeekExtensions.ContainsButton(pressedButtonData))
                {
                    // Здесь надо запомнить pressedButtonData - какой день выбрал пользователь
                    DaySelectedByUser[userId] = pressedButtonData;

                    await MessageSender.EditSentMessageAndMarkup(update.CallbackQuery?.Message!, "Выберите время",
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
                        await MessageSender.SendExceptionMessage(update.CallbackQuery?.Message?.Chat.Id!);
                        break;
                    }

                    menuManager.MakeLog(userId, request);
                    await MessageSender.SendTextMessage(update.CallbackQuery?.Message?.Chat.Id!,
                        menuManager.GetMenu(requestFormatter.Day, requestFormatter.MealType!));

                    // Эта строка нужна для того, чтобы после нажатия на кнопку исчезала
                    // анимация подгрузки в виде часов на этой кнопке
                    await botClient.AnswerCallbackQueryAsync(callbackQueryId: update.CallbackQuery!.Id,
                        cancellationToken: cancelToken);
                }

                break;
            }
        }
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancelToken)
    {
        throw new NotImplementedException();
    }
}