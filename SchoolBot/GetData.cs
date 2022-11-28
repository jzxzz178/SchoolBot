using Microsoft.Data.Sqlite;

namespace ShoolBot
{
    public class GetData
    {
        public static string GetRequestAnswer(RequestFormatter request)
        {
            if (request == null || request.Day == null || request.MealType == null)
                return "У Феди ошибка";

            using var connection = new SqliteConnection(@"Data Source = D:\ооп\Project\SchoolBot\all project\SchoolBot\SchoolBot\food_info.db");
            connection.Open();
            var sqlExpression = "SELECT * FROM "+request.Day;
            try
            {
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return "Меню еще нет на этот день :(";
                    while (reader.Read()) 
                    {
                        var meal = reader.GetValue(0).ToString();
                        if(meal==request.MealType)
                            return reader.GetValue(1).ToString();
                    }

                    return $"Нет приема пищи {request.MealType} !!!";
                }
            }
            catch
            {
                return $"Нет дня недели {request.Day}!!!";
            }
        }
    }
}