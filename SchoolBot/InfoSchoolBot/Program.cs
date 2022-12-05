using Microsoft.Extensions.Configuration;
using Serilog;

namespace SchoolBot;

static class Program
{
    private static void Main()
    {
        Console.WriteLine();
        var builder = new ConfigurationBuilder();
        BuildConfig(builder);
        
        using var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            //.Enrich.FromLogContext()
            .WriteTo.SQLite(Environment.CurrentDirectory.Replace(@"SchoolBot\InfoSchoolBot\bin\Debug\net6.0", @"LogDataBase\Logs.db"))
            // .WriteTo.Console()
            .CreateLogger();
        
        logger.Information("Started StolovkaBot");
        
        BotClient.StartBot();
    }
    private static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}