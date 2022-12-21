using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SchoolBot.DbWork
{
    public class Log
    {
        [Key] public DateTime Timestamp { get; set; }

        //public Guid id { get; set; }
        public string? UserId { get; set; }
        public string? RequestType { get; set; }
    }

    public class Menu
    {
        [Key] public string? Day { get; set; }
        public string? Breakfast { get; set; }
        public string? Lunch { get; set; }
    }

    public class Error
    {
        // public Guid id { get; set; }
        [Key] public DateTime Timestamp { get; set; }
        public string? Message { get; set; }
    }

    public class MyContext : DbContext
    {
        public DbSet<Log> Logs => Set<Log>();
        public DbSet<Menu> Menu => Set<Menu>();
        public DbSet<Error> Errors => Set<Error>();


        public MyContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Base.db");
        }
    }

    public static class DbManager
    {
        private static readonly MyContext Db = new MyContext();

        public static void AddLog(string idUser, string requestType)
        {
            var log = new Log
            {
                UserId = idUser,
                Timestamp = DateTime.Now,
                RequestType = requestType
            };
            Db.Logs.Add(log);
            Db.SaveChanges();
        }

        public static void LoggingError(string message)
        {
            var error = new Error()
            {
                Timestamp = DateTime.Now,
                Message = message
            };
            Db.Errors.Add(error);
            Db.SaveChanges();
        }

        public static string GetBreakfast(string day) =>
            Db.Menu.FirstOrDefault(menu => menu.Day != null && day == menu.Day)?.Breakfast ?? $"Меню еще нет на {day}";

        public static string GetLunch(string day) =>
            Db.Menu.FirstOrDefault(menu => menu.Day != null && day == menu.Day)?.Lunch ?? $"Меню еще нет на {day}";

        public static void AddDayMenu(string day, string breakfast, string lunch)
        {
            if (Db.Menu.Any(x => x.Day == day))
            {
                LoggingError($"Попытка добавить {day} без очистки базы");
                return;
            }

            var menu = new Menu()
            {
                Breakfast = breakfast,
                Day = day,
                Lunch = lunch
            };
            Db.Menu.Add(menu);
            Db.SaveChanges();
        }

        public static void CleanMenu()
        {
            foreach (var menu in Db.Menu)
            {
                Db.Menu.Remove(menu);
            }

            Db.SaveChanges();
        }
    }
}