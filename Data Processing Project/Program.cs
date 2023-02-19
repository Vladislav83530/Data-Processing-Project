using Data_Processing_Project;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();
ConfigureServices(services);
ServiceProvider serviceProvider = services.BuildServiceProvider();
Application app = serviceProvider.GetService<Application>();
try
{
    await Task.Run(async () =>
    {
        while (true)
        {
            await app.Start();
            Console.WriteLine("If you want to stop service Enter 'stop':");
            string command = Console.ReadLine()!.Trim().ToLower();
            if (command == "stop")
            {
                app.Stop();
                break;
            }
        }
    });
}
catch (Exception ex)
{
    app.HandleError(ex);
}

static void ConfigureServices(ServiceCollection services)
{
    services.AddLogging(configure => configure.AddConsole())
    .AddTransient<Application>();
}