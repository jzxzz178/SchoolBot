namespace SchoolBot;

public static class SqlRequest
{
    public static string GetAnswer(RequestFormatter requestFormat)
    {
        if (requestFormat.Day == null || requestFormat.MealType == null)
            throw new AggregateException("Day or MealType is null");
        
        return requestFormat.Day + ' ' + requestFormat.MealType;
    }
}