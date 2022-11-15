using System.ComponentModel;

namespace SchoolBot;

public enum MealType
{
    [Description("Завтрак")] Breakfast,
    [Description("Обед")] Lunch,
    [Description("Буфет")] Buffet
}

public static class MealTypeExtensions
{
    private static string[] descriptions = Enum.GetValues(typeof(MealType)).Cast<MealType>()
        .Select(val => val.GetDescription())
        .ToArray();

    public static bool ContainsCallBack(string? callBackQuery)
    {
        if (callBackQuery == null)
            throw new ArgumentException("Callback data is null");

        return descriptions.Contains(callBackQuery);
    }
}