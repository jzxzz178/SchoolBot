using SchoolBot.DbWork.Logic.DbTableClasses;
using SchoolBot.DbWork.Manager_Interfaces;

namespace SchoolBot.DbWork.Logic.DbCommunicators;

public class ErrorLogManager : IErrorLogManager
{
    private readonly AbstractDbTablesContext databaseContext;

    public ErrorLogManager(AbstractDbTablesContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }
    
    public void LoggingError(string message)
    {
        var error = new Error()
        {
            Timestamp = DateTime.Now,
            Message = message
        };
        databaseContext.Errors.Add(error);
        databaseContext.SaveChanges();
    }
}