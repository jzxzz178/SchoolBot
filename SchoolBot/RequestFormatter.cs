namespace ShoolBot
{
    public class RequestFormatter
    {
        public string Day { get; set; }
        public string MealType { get; set; }

        public void UpdateDay(string day)
        {
            Day = day;
        }

        public void UpdateMealType(string mealType)
        {
            MealType = mealType;
        }
    }
}
