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