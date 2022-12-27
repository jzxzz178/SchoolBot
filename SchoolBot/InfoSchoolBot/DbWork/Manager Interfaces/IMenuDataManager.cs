namespace SchoolBot.DbWork.Manager_Interfaces;

public interface IMenuDataManager
{
    public string GetBreakfast(string day);
    public string GetLunch(string day);
    public void CleanMenu();
}