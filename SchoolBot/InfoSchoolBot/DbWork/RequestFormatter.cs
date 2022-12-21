namespace SchoolBot.DbWork;

public class RequestFormatter
{
    public string? Day { get; private set; }
    public string? MealType { get; private set; }

    public void UpdateDay(string? day)
    {
        if (day == DaysOfWeek.Today.GetDescription())
        {
            Day = System.Globalization.CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat.DayNames[
                (int) Convert.ToDateTime(DateTime.Today).DayOfWeek];
            Day = Day.First().ToString().ToUpper() + Day.Remove(0, 1);
            return;
        }
        Day = day;
    }

    public void UpdateMealType(string? mealType) => MealType = mealType;
}