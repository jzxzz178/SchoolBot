using SchoolBot.DbWork.Manager_Interfaces;

namespace SchoolBot.DbWork.Logic.DbCommunicators;

public class MenuDataManager : IMenuDataManager
{
    private readonly AbstractDbTablesContext databaseContext;

    public MenuDataManager(AbstractDbTablesContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public string GetBreakfast(string day) =>
        databaseContext.Menu.FirstOrDefault(menu => menu.Day != null && day == menu.Day)?.Breakfast ??
        $"Меню еще нет на {day}";

    public string GetLunch(string day) =>
        databaseContext.Menu.FirstOrDefault(menu => menu.Day != null && day == menu.Day)?.Lunch ??
        $"Меню еще нет на {day}";

    public void CleanMenu()
    {
        foreach (var menu in databaseContext.Menu)
        {
            databaseContext.Menu.Remove(menu);
        }

        databaseContext.SaveChanges();
    }
}