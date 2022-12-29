using System.Diagnostics;
using System.Net;
using SchoolBot.DbWork.Manager_Interfaces;

namespace SchoolBot.DbWork.Logic.DbUpdate;

public class DbUpdateManager : IDbUpdateManager
{
    private readonly IMenuDataManager menuDataManager;
    private readonly IErrorLogManager errorLogManager;

    private static readonly string ScriptsDirectory =
        @$"{Environment.CurrentDirectory.Replace(@"SchoolBot\InfoSchoolBot\bin\Debug\net6.0", "")}FoodDataBase";

    public DbUpdateManager(IMenuDataManager menuDataManager, IErrorLogManager errorLogManager)
    {
        this.menuDataManager = menuDataManager;
        this.errorLogManager = errorLogManager;
    }

    public void ClearAndUpdateDb()
    {
        var days = GetCurrentDays();
        menuDataManager.CleanMenu();

        foreach (var day in days)
        {
            var isDownloadWasCorrect = DownloadFileFromSite(day);
            if (!isDownloadWasCorrect) continue;

            var psi = new ProcessStartInfo
            {
                FileName = @"python.exe"
            };

            var updateDataBaseScript = @$"{ScriptsDirectory}\update_database.py";

            var arg1 = $" {day:yyyy-MM-dd}";
            var arg2 = $" {ScriptsDirectory}";

            psi.UseShellExecute = false;
            
            psi.Arguments = updateDataBaseScript + arg1 + arg2;
            using Process updateProcess = Process.Start(psi)!;
        }
    }

    private bool DownloadFileFromSite(DateTime date)
    {
        var address = $"https://xn--47-6kclvec3aj7p.xn--80acgfbsl1azdqr.xn--p1ai/food/{date:yyyy-MM-dd}-sm.xls";
        var localFileName = $"{ScriptsDirectory}\\excels\\{date:yyyy-MM-dd}-sm.xls";
        var client = new WebClient();
        try
        {
            client.DownloadFile(address, localFileName);
            return true;
        }
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.ProtocolError &&
                ex.Response != null)
            {
                var resp = (HttpWebResponse) ex.Response;
                if (resp.StatusCode == HttpStatusCode.NotFound)
                    return false;
            }

            errorLogManager.LoggingError(ex.Message);
            return false;
        }
        catch (Exception e)
        {
            errorLogManager.LoggingError(e.Message);
            return false;
        }
    }

    private static List<DateTime> GetCurrentDays()
    {
        var currentDay = DateTime.Today;
        var dayNumber = (int) DateTime.Now.DayOfWeek;
        if (dayNumber != 0 & dayNumber != 6)
            return GetWeekdays(currentDay);
        return GetWeekdays(currentDay.AddDays(3));
    }

    private static List<DateTime> GetWeekdays(DateTime currentDayDt)
    {
        var days = new List<DateTime> {currentDayDt};
        for (var i = -1; i > -5; i--)
        {
            var prevDay = currentDayDt.AddDays(i);
            if ((int) prevDay.DayOfWeek == 0)
                break;
            days.Insert(0, prevDay);
        }

        for (var i = 1; i < 6; i++)
        {
            var prevDay = currentDayDt.AddDays(i);
            if ((int) prevDay.DayOfWeek == 6)
                break;
            days.Add(prevDay);
        }

        return days;
    }
}