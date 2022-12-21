﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchoolBot.DbWork;
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
            .ConfigureServices((context, services) => { services.AddTransient<IBot, BotClient>(); })
            .UseSerilog()
            .Build();

        DbManager.AddDayMenu("Понедельник", "кашка", "супик");
        Console.WriteLine(DbManager.GetBreakfast("Понедельник"));
        DbManager.AddDayMenu("Понедельник","картоха","мяса");
        var bot = ActivatorUtilities.CreateInstance<BotClient>(host.Services);
        bot.Run();
    }

    private static void BuildConfig(IConfigurationBuilder builder)
    {
        builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}