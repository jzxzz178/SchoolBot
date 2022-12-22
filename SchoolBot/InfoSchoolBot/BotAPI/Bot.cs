using Microsoft.Extensions.Logging;
using SchoolBot.BotAPI.Buttons;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SchoolBot.DbWork;
using SchoolBot.DbWork.Manager_Interfaces;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static SchoolBot.DaysOfWeek;
using static SchoolBot.MealType;

namespace SchoolBot.BotAPI;

public class Bot : IBot
{
    private readonly ITelegramBotClient bot;
    private readonly MenuManager menuManager;
    public ISendMessage MessageSender { get; set; }
    public IButtons ButtonCreate { get; set; }

    // Key: UserID, Value: selected Day
    private static readonly Dictionary<string, string?> DaySelectedByUser = new Dictionary<string, string?>();

    public Bot(ISendMessage messageSender, IButtons buttonCreate, MenuManager menuManager)
    {
        MessageSender = messageSender;
        ButtonCreate = buttonCreate;
        this.menuManager = menuManager;
        bot = new TelegramBotClient("5628215183:AAFeTuAoxldhloxF8yFzgUaC3GK04w28hOk");
    }

    public void Run()
    {
        // var botName = bot.GetMeAsync().Result.FirstName;
        // log.LogInformation("Запущен бот " + botName);

        var cancellationToken = new CancellationTokenSource().Token;
        var receiverOptions = new ReceiverOptions();

        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
    }

    private Task HandleUpdateAsync(ITelegramBotClient arg1, Update arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }

    private Task HandleErrorAsync(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }
}