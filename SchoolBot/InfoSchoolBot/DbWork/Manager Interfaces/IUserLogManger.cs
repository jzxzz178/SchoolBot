namespace SchoolBot.DbWork.Manager_Interfaces;

public interface IUserLogManger
{
    public void AddUserLog(string idUser, string requestType);
}