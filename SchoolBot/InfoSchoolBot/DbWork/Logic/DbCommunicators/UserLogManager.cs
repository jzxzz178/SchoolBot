using SchoolBot.DbWork.Logic.DbTableClasses;
using SchoolBot.DbWork.Manager_Interfaces;

namespace SchoolBot.DbWork.Logic.DbCommunicators;

public class UserLogManager : IUserLogManger
{
    private readonly AbstractDbTablesContext databaseContext;

    public UserLogManager(AbstractDbTablesContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }


    public void AddUserLog(string idUser, string requestType)
    {
        var log = new Log
        {
            UserId = idUser,
            Timestamp = DateTime.Now,
            RequestType = requestType
        };
        databaseContext.Logs.Add(log);
        databaseContext.SaveChanges();
    }
}