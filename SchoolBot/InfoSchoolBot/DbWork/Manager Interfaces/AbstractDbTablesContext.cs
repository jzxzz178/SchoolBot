using Microsoft.EntityFrameworkCore;
using SchoolBot.DbWork.Logic;
using SchoolBot.DbWork.Logic.DbTableClasses;

namespace SchoolBot.DbWork.Manager_Interfaces;

public abstract class AbstractDbTablesContext : DbContext
{
    public DbSet<Log> Logs => Set<Log>();
    public DbSet<Menu> Menu => Set<Menu>();
    public DbSet<Error> Errors => Set<Error>();
}