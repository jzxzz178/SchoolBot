using System.ComponentModel;
using System.Reflection;

namespace SchoolBot;

public static class EnumHelper
{
    public static string GetDescription<T>(this T enumValue)
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException($"{typeof(T)} is not Enum");

        var fieldInfo = enumValue?.GetType()
            .GetField(enumValue.ToString() ?? throw new ArgumentException("Incorrect name of Enum"));

        var attribute = fieldInfo?
            .GetCustomAttributes(true)
            .OfType<DescriptionAttribute>()
            .First();

        if (attribute == null)
            throw new ArgumentException("Еhe attribute is not set above the Enum");
        return attribute.Description;
    }
}

// в идеале, надо перенести MealTypeExtensions.ContainsCallBack в этот класс,
// но как это сделать, я понятия не имею

/*public static class EnumExtensions
{
    public static bool ContainsCallBackQuery(this Enum enumName, string? callBackQuery) 
    {
        if (!enumName.GetType().IsEnum)
            throw new ArgumentException($"{enumName} is not Enum");

        if (callBackQuery == null)
            throw new ArgumentException("Callback is null");
        

        var descriptions = Enum.GetValues(enumName.GetType()).Cast<MealType>()
            .Select(val => val.GetDescription())
            .ToArray();
        
        return descriptions.Contains(callBackQuery);
    }
}*/