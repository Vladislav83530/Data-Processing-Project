using Data_Processing_Project.Services;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Data_Processing_Project
{
    internal class FileManager
    {
        private readonly string pathA = ConfigurationManager.AppSettings["folderA"];
        private readonly string pathB = Path.Combine(ConfigurationManager.AppSettings["folderB"], $"{DateTime.Now.ToString("dd-MM-yyyy")}/");
        private Converter _converter;
        int CountFile = 0;
        private readonly ILogger _logger;

        public FileManager(ILogger logger)
        {
            _logger = logger;
            _converter = new Converter(new TXTReader(_logger));
        }

        public async Task ManageFile()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(pathA);
            watcher.Filters.Add("*.csv");
            watcher.Filters.Add("*.txt");
            watcher.EnableRaisingEvents = true;

            if (!Directory.Exists(pathB))
                Directory.CreateDirectory(pathB);

            watcher.Created += new FileSystemEventHandler(Watcher_Created);
            await CheckAndProcessFolderAsync();
        }

        void Watcher_Created(object s, FileSystemEventArgs e)
        {
            var ext = e.Name.Substring(e.Name.LastIndexOf('.'));
            switch (ext)
            {
                case ".csv":
                    CountFile++;
                    _converter.reader = new CSVReader(_logger);
                    _converter.ProcessFile(e, pathB, CountFile);
                    break;
                case ".txt":
                    CountFile++;
                    _converter.reader = new TXTReader(_logger);
                    _converter.ProcessFile(e, pathB, CountFile);
                    break;
            }
        }

        private async Task CheckAndProcessFolderAsync()
        {
            var files = Directory.GetFiles(pathA, "*.*").Where(f => f.EndsWith(".csv") || f.EndsWith(".txt")); 
            if (files.FirstOrDefault() != null)
            {
                foreach (var x in files)
                {
                    var ext = x.Substring(x.LastIndexOf('.'));
                    switch (ext)
                    {
                        case ".csv":
                            CountFile++;
                            _converter.reader = new CSVReader(_logger);
                            await _converter.ProcessFileAsync(x, pathB, CountFile);
                            break;
                        case ".txt":
                            CountFile++;
                            _converter.reader = new TXTReader(_logger);
                            await _converter.ProcessFileAsync(x, pathB, CountFile);
                            break;
                    }
                };
            }
        }
    }
}
