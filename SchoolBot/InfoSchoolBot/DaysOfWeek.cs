using System.ComponentModel;

namespace SchoolBot;

public enum DaysOfWeek
{
    [Description("Сегодня")] Today,
    [Description("Понедельник")] Monday,
    [Description("Вторник")] Tuesday,
    [Description("Среда")] Wednesday,
    [Description("Четверг")] Thursday,
    [Description("Пятница")] Friday
}

public static class DaysOfWeekExtensions
{
    private static readonly string[] Descriptions = Enum.GetValues(typeof(DaysOfWeek)).Cast<DaysOfWeek>()
        .Select(val => val.GetDescription())
        .ToArray();

    public static bool ContainsButton(string? pressedButtonData)
    {
        if (pressedButtonData == null)
            throw new ArgumentException("Callback data is null");

        return Descriptions.Contains(pressedButtonData);
    }
}