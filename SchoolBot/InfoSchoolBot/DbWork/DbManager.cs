namespace SchoolBot.DbWork;

public static class DbManager
{
    private static readonly DbTablesContext Db = new DbTablesContext();

    public static void AddLog(string idUser, string requestType)
    {
        var log = new Log
        {
            UserId = idUser,
            Timestamp = DateTime.Now,
            RequestType = requestType
        };
        Db.Logs.Add(log);
        Db.SaveChanges();
    }

    private static void LoggingError(string message)
    {
        var error = new Error()
        {
            Timestamp = DateTime.Now,
            Message = message
        };
        Db.Errors.Add(error);
        Db.SaveChanges();
    }

    public static string GetBreakfast(string day) =>
        Db.Menu.FirstOrDefault(menu => menu.Day != null && day == menu.Day)?.Breakfast ?? $"Меню еще нет на {day}";

    public static string GetLunch(string day) =>
        Db.Menu.FirstOrDefault(menu => menu.Day != null && day == menu.Day)?.Lunch ?? $"Меню еще нет на {day}";

    public static void AddDayMenu(string day, string breakfast, string lunch)
    {
        if (Db.Menu.Any(x => x.Day == day))
        {
            LoggingError($"Попытка добавить {day} без очистки базы");
            return;
        }

        var menu = new Menu()
        {
            Breakfast = breakfast,
            Day = day,
            Lunch = lunch
        };
        Db.Menu.Add(menu);
        Db.SaveChanges();
    }

    public static void CleanMenu()
    {
        foreach (var menu in Db.Menu)
        {
            Db.Menu.Remove(menu);
        }

        Db.SaveChanges();
    }
}