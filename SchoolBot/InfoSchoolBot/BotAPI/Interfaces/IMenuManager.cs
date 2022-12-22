namespace SchoolBot.BotAPI.Interfaces;

public interface IMenuManager
{
    string GetMenu(string? userId, string? day, string meal);
    Lazy<IBot> Bot { get; set; }
    void Run();
}