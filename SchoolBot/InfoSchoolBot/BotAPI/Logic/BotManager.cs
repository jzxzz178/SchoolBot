using SchoolBot.BotAPI.Interfaces;
using SchoolBot.DbWork.Manager_Interfaces;

namespace SchoolBot.BotAPI.Logic;

public class BotManager : IBotManager
{
    private readonly IMenuDataManager menuDataManager;
    private readonly IUserLogManger userLogManger;
    private readonly IErrorLogManager errorLogManager;

    public BotManager(IMenuDataManager menuDataManager, IUserLogManger userLogManger,
        IErrorLogManager errorLogManager)
    {
        this.menuDataManager = menuDataManager;
        this.userLogManger = userLogManger;
        this.errorLogManager = errorLogManager;
    }

    public string GetMenu(string? day, string? meal)
    {
        if (day == null || meal == null)
            errorLogManager.LoggingError($"Null request to db!!! || Day: {day}; Meal: {meal}");
        return meal == "Обед" ? menuDataManager.GetLunch(day!) : menuDataManager.GetBreakfast(day!);
    }

    public void MakeLog(string userId, string requestType)
    {
        userLogManger.AddUserLog(userId, requestType);
    }
}