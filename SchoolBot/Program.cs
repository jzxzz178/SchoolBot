using System;
namespace ShoolBot
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var request = new RequestFormatter();
            request.UpdateDay("Четверг");
            request.UpdateMealType("Завтрак");
            Console.WriteLine(GetData.GetRequestAnswer(request));
        }
    }
}