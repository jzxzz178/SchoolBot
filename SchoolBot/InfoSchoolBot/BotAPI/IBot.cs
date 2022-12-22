using SchoolBot.BotAPI.Buttons;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SchoolBot.BotAPI;

public interface IBot
{
    void Run();
    ISendMessage MessageSender { get; set; }
    IButtons ButtonCreate { get; set; }
}