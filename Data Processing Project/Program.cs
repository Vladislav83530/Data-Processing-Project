using Data_Processing_Project;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

class Program
{
    static void Main(string[] args)
    {
        var command = args.AsQueryable().FirstOrDefault();
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        Application app = serviceProvider.GetService<Application>();
        try
        {
            if (command == null)
            {
                Console.WriteLine("Enter command: ");
                command = Console.ReadLine()!.Trim();
                if (command == "Start")
                    app.Start();
                else if (command == "Stop")
                    app.Stop();
            }
        }
        catch (Exception ex)
        {
            app.HandleError(ex);
        }
        Console.ReadLine();
    }
    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddLogging(configure => configure.AddConsole())
        .AddTransient<Application>();
    }
}

class Application
{
    private readonly ILogger _logger;
    public Application(ILogger<Application> logger)
    {
        _logger = logger;
    }

    public async Task  Start()
    {
        _logger.LogInformation($"MyApplication Started at {DateTime.Now}");
        try
        {
            FileManager fm = new FileManager(_logger);
            await fm.ManageFile();
            Console.Read();
        }
        catch
        {
            throw;
        }
        //finally
        //{
        //    _logger.LogCritical($"Fix bag");
        //}
    }

    public void Stop()
    {
        _logger.LogInformation($"Application Stopped at {DateTime.Now}");
    }

    public void HandleError(Exception ex)
    {
        _logger.LogError($"Application Error Encountered at {DateTime.Now} & Error is: {ex.Message}");
    }
}