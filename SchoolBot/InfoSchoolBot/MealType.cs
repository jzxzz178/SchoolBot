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
    public static bool ContainsCallBack(string? callBackQuery)
    {
        if (callBackQuery == null) 
            throw new ArgumentException("Callback is null");
        
        var a = typeof(MealType).GetFields();
        var b = a.Where(field => field.GetCustomAttributes(true).OfType<DescriptionAttribute>().Count() != 0);
        var c = b.Select(field => field.GetDescription());
        var d = c.Contains(callBackQuery);
        return d;
    }
}