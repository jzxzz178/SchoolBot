namespace SchoolBot.DbWork.Manager_Interfaces;

public interface IDatabaseManager
{
    string GetBreakfast(string day);
    string GetLunch(string day);
    void AddDayMenu(string day, string breakfast, string lunch);
    void AddLog(string idUser, string requestType);
    void LoggingError(string message);
}