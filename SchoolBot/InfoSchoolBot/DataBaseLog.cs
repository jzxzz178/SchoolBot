using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace ShoolBot
{
    public class DataBaseLog
    {
        static DataBaseLog()
        {
            if(File.Exists("logs.db"))
                return;
            string sqlExpression = @"CREATE TABLE Logs 
                                (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                 Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, 
                                 UserId TEXT NOT NULL,
                                 RequestType TEXT NOT NULL)";
            using (var connection = new SqliteConnection("Data Source=logs.db"))
            {
                connection.Open();
 
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
                Console.WriteLine("Таблица Files создана");
            }
        }
        public static async Task Logger(string userId,string requestType)
        {
            using (var connection = new SqliteConnection("Data Source=logs.db"))
            {
                connection.Open();
 
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Logs (UserId,RequestType)
                                        VALUES (@UserId,@RequestType)";
                command.Parameters.Add(new SqliteParameter("@UserId", userId));
                command.Parameters.Add(new SqliteParameter("@RequestType", requestType));
                int number = command.ExecuteNonQuery();
                // Console.WriteLine($"Добавлено объектов: {number}");
            }
            
        }
    }
}