﻿using SchoolBot.DbWork.Logic.DbTableClasses;
using SchoolBot.DbWork.Manager_Interfaces;

namespace SchoolBot.DbWork.Logic.DbCommunicators;

public class DbManager : IDatabaseManager
{
    private AbstractDbTablesContext DatabaseContext { get; }

    public DbManager(AbstractDbTablesContext tablesContext)
    {
        DatabaseContext = tablesContext;
    }

    public void AddLog(string idUser, string requestType)
    {
        var log = new Log
        {
            UserId = idUser,
            Timestamp = DateTime.Now,
            RequestType = requestType
        };
        DatabaseContext.Logs.Add(log);
        DatabaseContext.SaveChanges();
    }

    public void LoggingError(string message)
    {
        var error = new Error()
        {
            Timestamp = DateTime.Now,
            Message = message
        };
        DatabaseContext.Errors.Add(error);
        DatabaseContext.SaveChanges();
    }

    public string GetBreakfast(string day) =>
        DatabaseContext.Menu.FirstOrDefault(menu => menu.Day != null && day == menu.Day)?.Breakfast ??
        $"Меню еще нет на {day}";

    public string GetLunch(string day) =>
        DatabaseContext.Menu.FirstOrDefault(menu => menu.Day != null && day == menu.Day)?.Lunch ??
        $"Меню еще нет на {day}";

    public void AddDayMenu(string day, string breakfast, string lunch)
    {
        if (DatabaseContext.Menu.Any(x => x.Day == day))
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
        DatabaseContext.Menu.Add(menu);
        DatabaseContext.SaveChanges();
    }

    public void CleanMenu()
    {
        foreach (var menu in DatabaseContext.Menu)
        {
            DatabaseContext.Menu.Remove(menu);
        }

        DatabaseContext.SaveChanges();
    }
}