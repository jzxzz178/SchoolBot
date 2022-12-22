namespace SchoolBot.BotAPI;

public interface IMenuManager
{
    string GetMenu(string day, string meal);
    Lazy<IBot> Bot { get; set; }
    IConstants Constants { get; set; }
}