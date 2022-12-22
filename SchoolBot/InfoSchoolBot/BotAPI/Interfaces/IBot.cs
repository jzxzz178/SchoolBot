namespace SchoolBot.BotAPI.Interfaces;

public interface IBot
{
    void Run();
    ISendMessage MessageSender { get; set; }
    IButtons ButtonCreate { get; set; }
}