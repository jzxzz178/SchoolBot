using SchoolBot.DbWork.Manager_Interfaces;

namespace SchoolBot.BotAPI;

public class MenuManager : IMenuManager
{
    public Lazy<IBot> Bot { get; set; }
    public IConstants Constants { get; set; }
    private readonly IDatabaseManager dbManager;

    public MenuManager(Lazy<IBot> bot, IConstants constants, IDatabaseManager dbManager)
    {
        Bot = bot;
        Constants = constants;
        this.dbManager = dbManager;
    }

    public string GetMenu(string day, string meal)
    {
        return meal == "Обед" ? dbManager.GetLunch(day) : dbManager.GetBreakfast(day);
    }
}