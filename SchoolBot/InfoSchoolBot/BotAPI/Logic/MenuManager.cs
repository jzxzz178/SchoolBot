using SchoolBot.BotAPI.Interfaces;
using SchoolBot.DbWork.Manager_Interfaces;

namespace SchoolBot.BotAPI.Logic;

public class MenuManager : IMenuManager
{
    private readonly IDatabaseManager dbManager;

    public MenuManager(IDatabaseManager dbManager)
    {
        this.dbManager = dbManager;
    }


    public string GetMenu(string? day, string? meal)
    {
        if (day == null || meal == null) dbManager.LoggingError($"Null request to db!!! || Day: {day}; Meal: {meal}");
        return meal == "Обед" ? dbManager.GetLunch(day!) : dbManager.GetBreakfast(day!);
    }

    public void MakeLog(string userId, string requestType)
    {
        dbManager.AddLog(userId, requestType);
    }
}