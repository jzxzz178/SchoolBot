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