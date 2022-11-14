using System.Diagnostics;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static SchoolBot.DaysOfWeek;
using static SchoolBot.MealType;
using static SchoolBot.MealTypeExtensions;
using static SchoolBot.EnumHelper;


namespace SchoolBot;

public class OrderInfo
{
    private DaysOfWeek Day { get; }
    private MealType MealType { get; }

    public OrderInfo(DayOfWeek dayOfWeek, MealType mealType)
    {
    }
}

public class BotClient
{
    private static readonly ITelegramBotClient Bot =
        new TelegramBotClient("5735097045:AAGyp2lMa72zKg2PTkLke-bFI7DS7zpu7xI");

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        Console.WriteLine();
        Console.WriteLine(JsonConvert.SerializeObject(update.Type));
        switch (update.Type)
        {
            case UpdateType.CallbackQuery:
            {
                var pressedButtonData = update.CallbackQuery?.Data;
                Console.WriteLine($"Pressed button = {pressedButtonData}");
                // if (typeof(MealType).ContainsCallBackQuery(pressedButtonData))
                if (ContainsCallBack(pressedButtonData))
                {
                    await botClient.SendTextMessageAsync(
                        chatId: update.CallbackQuery?.Message?.Chat.Id ?? 
                                throw new InvalidOperationException("Chat.Id was null"),
                        pressedButtonData,
                        cancellationToken: cancellationToken);
                    break;
                }

                // тут я думал создавать new OrderInfo()

                if (update.CallbackQuery?.Message != null)
                    await botClient.EditMessageReplyMarkupAsync(
                        chatId: update.CallbackQuery?.Message?.Chat.Id ??
                                throw new InvalidOperationException("Chat.Id was null"),
                        messageId: update.CallbackQuery.Message.MessageId,
                        cancellationToken: cancellationToken,
                        replyMarkup: GetInlineKeyboardWithTimeOfMeal());
                break;
            }
            case UpdateType.Message:
            {
                var message = update.Message;
                Console.WriteLine(message?.Text);

                if (message?.Text == "/start")
                {
                    Debug.Assert(update.Message != null, "update.Message != null");
                    await botClient.SendTextMessageAsync(update.Message.Chat,
                        "На какой день показать расписание обэда?",
                        ParseMode.MarkdownV2,
                        cancellationToken: cancellationToken,
                        replyMarkup: GetInlineKeyboardWithDays());
                }
                else
                {
                    Debug.Assert(message?.Chat != null, "message?.Chat != null");
                    await botClient.SendTextMessageAsync(message.Chat,
                        "Отправьте /start, чтобы узнать меню на неделю",
                        ParseMode.MarkdownV2,
                        cancellationToken: cancellationToken);
                }

                break;
            }
        }
    }

    private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(JsonConvert.SerializeObject(exception));
    }

    public static void StartBot()
    {
        Console.WriteLine("Запущен бот " + Bot.GetMeAsync().Result.FirstName);

        var cancellationToken = new CancellationTokenSource().Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };
        Bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
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
                InlineKeyboardButton.WithCallbackData(Buffet.GetDescription()),
            },
        });

        return inlineKeyboard;
    }
}