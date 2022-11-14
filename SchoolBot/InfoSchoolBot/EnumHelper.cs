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

    public static bool ContainsCallBackQuery<T>(this T enumName, string? callBackQuery)
        where T : class
    {
        /*if (!typeof(T).IsEnum)
            throw new ArgumentException($"{typeof(T)} is not Enum");*/
         if (callBackQuery == null)
             throw new ArgumentException("Callback is null");

        var a = enumName.GetType();
        var b = a.GetFields();
        var c = b.Where(field => field.GetCustomAttributes(true).Contains(typeof(DescriptionAttribute)));
        var d = c.Select(field => field.GetDescription());
        var e = d.Contains(callBackQuery);
        return e;
    }
}