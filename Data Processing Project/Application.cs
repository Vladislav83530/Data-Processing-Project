using Data_Processing_Project.FileManager_;
using Microsoft.Extensions.Logging;
using System.Configuration;

namespace Data_Processing_Project
{
    internal class Application
    {
        private readonly ILogger _logger;

        public Application(ILogger<Application> logger)
        {
            _logger = logger;
        }

        public async Task Start()
        {
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["folderA"]) || String.IsNullOrEmpty(ConfigurationManager.AppSettings["folderB"]))
            {
                _logger.LogCritical("Specify the paths to the folder_a and folder_b in the file app.config. And try again");
                Stop();
            }

            _logger.LogInformation($"MyApplication Started at {DateTime.Now}");
            FileManager fm = new FileManager(_logger);
            MetaLogger ml = new MetaLogger(fm);
            try
            {
                await fm.ManageFile();
                ml.MetaLogTimer();
            }
            catch
            {
                throw;
            }
        }

        public void Stop()
        {
            _logger.LogInformation($"Application Stopped at {DateTime.Now}");
            Environment.Exit(0);
        }

        public void HandleError(Exception ex)
        {
            _logger.LogError($"Application Error Encountered at {DateTime.Now} & Error is: {ex.Message}");
        }
    }
}