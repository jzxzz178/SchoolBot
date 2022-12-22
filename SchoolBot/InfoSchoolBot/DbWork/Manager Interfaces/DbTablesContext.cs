using Microsoft.EntityFrameworkCore;

namespace SchoolBot.DbWork.Manager_Interfaces
{
    public sealed class DbTablesContext : DbContext
    {
        public DbSet<Log> Logs => Set<Log>();
        public DbSet<Menu> Menu => Set<Menu>();
        public DbSet<Error> Errors => Set<Error>();


        public DbTablesContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Base.db");
        }
    }
}