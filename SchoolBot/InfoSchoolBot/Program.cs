using System.Configuration;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace SchoolBot;

static class Program
{
    private static string dataBasePath = Environment.CurrentDirectory.Replace(@"SchoolBot\InfoSchoolBot\bin\Debug\net6.0",@"LogDataBase\Logs.db");
    private static string appsettingsPath = Environment.CurrentDirectory.Replace(@"bin\Debug\net6.0",@"appsettings.json");

    private static void Main()
    {
        /*var builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsPath)
            .AddEnvironmentVariables()
            .Build();*/
        
        /*Log.Logger = new LoggerConfiguration()
            //.ReadFrom.Configuration(builder)
            //.ReadFrom.AppSettings(filePath: appsettingsPath)
            .WriteTo.SQLite(ConfigurationManager.AppSettings["SQLite"])
            .WriteTo.Console()
            .CreateLogger();*/

        BotClient.StartBot();
    }
    
    
}