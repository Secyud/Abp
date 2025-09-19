using SecitsDemoApp;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("Logs/logs.txt"))
#if DEBUG
    .WriteTo.Async(c => c.Console())
#endif
    .CreateLogger();
try
{
    Log.Information("Starting web host.");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host
        .UseAutofac()
        .UseSerilog();

    await builder.AddApplicationAsync<SecitsDemoAppBlazorModule>();

    var app = builder.Build();

    await app.InitializeApplicationAsync();

    app.Run();
    
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!");
    
    return 1;
}
finally
{
    Log.CloseAndFlush();
}