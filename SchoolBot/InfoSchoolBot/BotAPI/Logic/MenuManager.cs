using SchoolBot.BotAPI.Interfaces;
using SchoolBot.DbWork.Manager_Interfaces;

namespace SchoolBot.BotAPI.Logic;

public class MenuManager : IMenuManager
{
    // public Lazy<IBot> Bot { get; set; }
    private readonly IDatabaseManager dbManager;

    public MenuManager( /*Lazy<IBot> bot,*/ IDatabaseManager dbManager)
    {
        // Bot = bot;
        this.dbManager = dbManager;
    }

    /*public void Run()
    {
        Bot.Value.Run();
    }*/

    public string GetMenu(string? userId, string? day, string? meal)
    {
        if (day == null || meal == null) dbManager.LoggingError($"Null request to db!!! || Day: {day}; Meal: {meal}");
        return meal == "Обед" ? dbManager.GetLunch(day!) : dbManager.GetBreakfast(day!);
    }

    public void MakeLog(string userId, string requestType)
    {
        dbManager.AddLog(userId, requestType);
    }
}