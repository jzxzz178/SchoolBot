using SchoolBot.BotAPI.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;
using static SchoolBot.BotAPI.Buttons.DaysOfWeek;
using static SchoolBot.BotAPI.Buttons.MealType;
using static SchoolBot.BotAPI.Interfaces.IConstants;

namespace SchoolBot.BotAPI.Buttons;

public class Buttons : IButtons
{
    public static ReplyKeyboardMarkup GetReplyKeyboardWithSchedule()
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

    public static InlineKeyboardMarkup GetInlineKeyboardWithDays()
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

    public static InlineKeyboardMarkup GetInlineKeyboardWithTimeOfMeal()
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