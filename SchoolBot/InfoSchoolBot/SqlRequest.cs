namespace SchoolBot;

public static class SqlRequest
{
    public static string GetAnswer(RequestFormatter requestFormat)
    {
        /*if (requestFormatter.Day == null || requestFormatter.MealType == null)
            throw new AggregateException("Day or MealType is null");*/
        
        return requestFormat.Day + ' ' + requestFormat.MealType;
    }
}