using System.ComponentModel;

namespace SchoolBot.BotAPI.Buttons;

public enum MealType
{
    [Description("Завтрак")] Breakfast,
    [Description("Обед")] Lunch,
    // [Description("Буфет")] Buffet
}

public static class MealTypeExtensions
{
    private static readonly string[] Descriptions = Enum.GetValues(typeof(MealType)).Cast<MealType>()
        .Select(val => val.GetDescription())
        .ToArray();

    public static bool ContainsButton(string? pressedButtonData)
    {
        if (pressedButtonData == null)
            throw new ArgumentException("Callback data is null");

        return Descriptions.Contains(pressedButtonData);
    }
}