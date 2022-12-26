namespace SchoolBot.BotAPI.Interfaces;

public interface IMenuManager
{
    string GetMenu(string? day, string meal);
    public void MakeLog(string userId, string requestType);
}