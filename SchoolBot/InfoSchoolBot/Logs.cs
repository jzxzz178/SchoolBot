using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SQLiteApp
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string? RequestType { get; set; }
    }

    public class MyContext : DbContext
    {
        public DbSet<Log> Logs => Set<Log>();
        
        public MyContext()
        {
            Database.EnsureCreated();
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Logs.db");
        }
    }

    public static class manager
    {
        public static MyContext db = new MyContext();

        public static void AddLog(string idUser, string requestType)
        {
            var lll = new Log
            {
                UserId = idUser,
                Timestamp = DateTime.Now,
                RequestType = requestType
            };
            db.Logs.Add(lll);
            db.SaveChanges();
        }
    }
}