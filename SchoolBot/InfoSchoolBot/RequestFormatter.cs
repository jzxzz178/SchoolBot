namespace SchoolBot;

public class RequestFormatter
{
    public string? Day { get; private set; }
    public string? MealType { get; private set; }

    public void UpdateDay(string? day) => Day = day;

    public void ClearDay() => Day = null;

    public void UpdateMealType(string? mealType) => MealType = mealType;
    
    public void ClearMealType() => MealType = null;
}