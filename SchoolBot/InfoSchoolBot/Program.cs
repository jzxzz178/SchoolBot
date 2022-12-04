using Serilog;

namespace SchoolBot;

static class Program
{
    private static void Main()
    {
        Console.WriteLine();
        using var logger = new LoggerConfiguration()
            .WriteTo.SQLite(Environment.CurrentDirectory.Replace(@"SchoolBot\InfoSchoolBot\bin\Debug\net6.0", ""))
            .WriteTo.Console()
            .CreateLogger();
        logger.Information("Started StolovkaBot");
        BotClient.StartBot();
    }
}