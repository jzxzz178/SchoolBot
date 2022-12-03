using System.Diagnostics;
using System.Net;

namespace download_food
{
    class Program
    {
        static void Main(string[] args)
        {
            var days = GetCurrentDays();
            foreach (var day in days)
            {
                var isDownloadWasCorrect = DownloadFileFromSite(day);
                if (isDownloadWasCorrect)
                {
                    //путь до питона укажите, если у вас другой
                    var psi = new ProcessStartInfo
                    {
                        FileName = @"C:\Program Files\Python310\python.exe"
                    };

                    var currentDirectory = Directory.GetCurrentDirectory();
                    var clearCmd = $"{currentDirectory}\\clear_database_by_date.py";
                    var updateCmd = $"{currentDirectory}\\update_database.py";

                    var arg = $" {day:yyyy-MM-dd}";
                    
                    psi.Arguments = clearCmd + arg;
                    psi.UseShellExecute = false;
                    using Process clearProcess = Process.Start(psi);

                    psi.Arguments = updateCmd + arg;
                    using Process updateProcess = Process.Start(psi);
                }
            }
        }
        
        public static bool DownloadFileFromSite(DateTime date) 
        { 
            var address = $"https://xn--47-6kclvec3aj7p.xn--80acgfbsl1azdqr.xn--p1ai/food/{date:yyyy-MM-dd}-sm.xls"; 
            var localFileName = $"{Directory.GetCurrentDirectory()}\\excels\\{date:yyyy-MM-dd}-sm.xls";
            var client = new WebClient();
            try 
            {
                client.DownloadFile(address, localFileName);
                return true;
            }
            catch 
            {
                Console.WriteLine("мяу");
                return false;
            }
        }
        
        public static List<DateTime> GetCurrentDays()
        {
            var currentDayDT= DateTime.Today;
            var dayNumber = (int) DateTime.Now.DayOfWeek;
            if (dayNumber != 0 & dayNumber != 6)
                return GetWeekdays(currentDayDT);
            return GetWeekdays(currentDayDT.AddDays(3));
        }

        public static List<DateTime> GetWeekdays(DateTime currentDayDT)
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
}