﻿using Microsoft.Data.Sqlite;

namespace SchoolBot.DbWork;

public static class SqlRequest
{
    private static readonly string DataBase =
        @$"Data Source = {Environment.CurrentDirectory.Replace(@"SchoolBot\InfoSchoolBot\bin\Debug\net6.0", "")}FoodDataBase\food_info.db";

    public static string? GetAnswer(RequestFormatter request)
    {
        if (request.Day == null || request.MealType == null)
            return "У Феди ошибка";

        using var connection =
            new SqliteConnection(DataBase);
        connection.Open();
        var sqlExpression = "SELECT * FROM " + request.Day; 
        try
        {
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows) return "Меню еще нет на этот день :(";
                while (reader.Read())
                {
                    var meal = reader.GetValue(0).ToString();
                    if (meal == request.MealType)
                        return reader.GetValue(1).ToString();
                }

                return $"Пока нет меню на {request.MealType} !!!";
            }
        }
        catch
        {
            return $"Пока нет меню на {request.Day} !!!";
        }
    }
}