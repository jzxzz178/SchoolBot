namespace SchoolBot.BotAPI.Interfaces;

public interface IBotManager
{
    string GetMenu(string? day, string meal);
    public void MakeLog(string userId, string requestType);
}