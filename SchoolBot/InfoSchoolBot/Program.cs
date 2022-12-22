using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchoolBot.BotAPI;
using SchoolBot.DbWork;
using SchoolBot.DbWork.Manager_Interfaces;
using Serilog;
using Log = Serilog.Log;

namespace SchoolBot;

static class Program
{
    private static void Main()
    {
        var builder = new ConfigurationBuilder();
        BuildConfig(builder);
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Build())
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IBot, Bot>();
                services.AddSingleton<IMenuManager, MenuManager>();
                services.AddSingleton<IDatabaseManager, DbManager>();
            })
            .UseSerilog()
            .Build();

        var menuManager = ActivatorUtilities.CreateInstance<MenuManager>(host.Services);
        menuManager.Bot.Value.Run();
    }

    private static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}