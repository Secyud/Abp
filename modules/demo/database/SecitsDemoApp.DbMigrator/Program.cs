using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SecitsDemoApp;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
#if DEBUG
    .MinimumLevel.Override("SecitsDemoApp", LogEventLevel.Debug)
#else
    .MinimumLevel.Override("SecitsDemoApp", LogEventLevel.Information)
#endif
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("Logs/logs.txt"))
    .WriteTo.Async(c => c.Console())
    .CreateLogger();
    
await Host.CreateDefaultBuilder(args)
    .AddAppSettingsSecretsJson()
    .ConfigureLogging((context, logging) => logging.ClearProviders())
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<DbMigratorHostedService>();
    }).RunConsoleAsync();
