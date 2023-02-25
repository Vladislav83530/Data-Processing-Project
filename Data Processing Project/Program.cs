using Data_Processing_Project;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


var services = new ServiceCollection();
ConfigureServices(services);
ServiceProvider serviceProvider = services.BuildServiceProvider();
Application app = serviceProvider.GetService<Application>();
try
{
    app.Start();
}
catch (Exception ex)
{
    app.HandleError(ex);
}
Console.ReadLine();
static void ConfigureServices(ServiceCollection services)
{
    services.AddLogging(configure => configure.AddConsole())
    .AddTransient<Application>();
}

class Application
{
    private readonly ILogger _logger;
    public Application(ILogger<Application> logger)
    {
        _logger = logger;
    }

    public async Task Start()
    {
        _logger.LogInformation($"MyApplication Started at {DateTime.Now}");
        try
        {
            var work = true;
            while (work)
            {
                Console.WriteLine("Add file to folder_a or Enter command: reset/stop");             
                FileManager fm = new FileManager(_logger);
                await fm.ManageFile();
                MetaLogger ml = new MetaLogger(fm);
                ml.MetaLogTimer();
                string command = Console.ReadLine()!.Trim().ToLower();
                if (command == "reset")
                {
                    Reset();
                }
                else if (command == "stop")
                {
                    Stop();
                    work = false;
                }
            }
        }
        catch
        {
            throw;
        }
    }

    public void Stop()
    {
        _logger.LogInformation($"Application Stopped at {DateTime.Now}. Press some button to end");
    }

    public void HandleError(Exception ex)
    {
        _logger.LogError($"Application Error Encountered at {DateTime.Now} & Error is: {ex.Message}");
    }

    public void Reset()
    {
        _logger.LogInformation($"Application Reset at {DateTime.Now}.");
    }
}