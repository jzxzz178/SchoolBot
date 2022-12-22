using System.Diagnostics;
using System.Net;

namespace GetFilesAndExecPython;

static class UpdatingDataBase
{
    private static readonly string ScriptsDirectory =
        @$"{Environment.CurrentDirectory.Replace(@"GetFilesAndExecPython\GetFilesAndExecutePython\bin\Debug\net6.0", "")}FoodDataBase";
    static void Main(string[] args)
    {
        var days = GetCurrentDays();
        
        foreach (var day in days)
        {
            var isDownloadWasCorrect = DownloadFileFromSite(day);
            if (!isDownloadWasCorrect) continue;
            
            // путь до питона укажите, если у вас другой
            var psi = new ProcessStartInfo
            {
                FileName = @"C:\Users\user\AppData\Local\Programs\Python\Python311\python.exe"
            };

            // var currentDirectory = Directory.GetCurrentDirectory();
            // var clearByDateScript = @$"{ScriptsDirectory}\clear_database_by_date.py";
            var updateDataBaseScript = @$"{ScriptsDirectory}\update_database.py";

            var arg1 = $" {day:yyyy-MM-dd}";
            var arg2 = $" {ScriptsDirectory}";
            
            psi.UseShellExecute = false;
            
            /*psi.Arguments = clearByDateScript + arg1 + arg2;
            using Process clearProcess = Process.Start(psi);*/

            psi.Arguments = updateDataBaseScript + arg1 + arg2;
            using Process updateProcess = Process.Start(psi);
        }
    }

    private static bool DownloadFileFromSite(DateTime date) 
    { 
        var address = $"https://xn--47-6kclvec3aj7p.xn--80acgfbsl1azdqr.xn--p1ai/food/{date:yyyy-MM-dd}-sm.xls"; 
        var localFileName = $"{ScriptsDirectory}\\excels\\{date:yyyy-MM-dd}-sm.xls";
        var client = new WebClient();
        try 
        {
            client.DownloadFile(address, localFileName);
            return true;
        }
        catch (Exception e) 
        {
            Console.WriteLine(e);
            return false;
        }
    }

    private static List<DateTime> GetCurrentDays()
    {
        var currentDay= DateTime.Today;
        var dayNumber = (int) DateTime.Now.DayOfWeek;
        if (dayNumber != 0 & dayNumber != 6)
            return GetWeekdays(currentDay);
        return GetWeekdays(currentDay.AddDays(3));
    }

    private static List<DateTime> GetWeekdays(DateTime currentDayDT)
    {
        var days = new List<DateTime> { currentDayDT };
        for (var i = -1; i > -5; i--)
        {
            var prevDay = currentDayDT.AddDays(i);
            if ((int) prevDay.DayOfWeek == 0) 
                break;
            days.Insert(0, prevDay);
        }
        for (var i = 1; i < 6; i++)
        {
            var prevDay = currentDayDT.AddDays(i);
            if ((int) prevDay.DayOfWeek == 6) 
                break;
            days.Add(prevDay);
        }

        return days;
    }
}