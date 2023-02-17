using Data_Processing_Project.Services;
using System.Configuration;

namespace Data_Processing_Project
{
    internal class FileManager
    {
        private readonly string pathA = ConfigurationManager.AppSettings["folderA"];
        private readonly string pathB = ConfigurationManager.AppSettings["folderB"];
        private Converter _converter;

        public FileManager()
        {
            _converter = new Converter(new TXTReader());
        }

        public async Task ManageFile()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(pathA);
            watcher.Filters.Add("*.csv");
            watcher.Filters.Add("*.txt");
            watcher.EnableRaisingEvents = true;

            await CheckAndProcessFolder();

            watcher.Created += new FileSystemEventHandler(Watcher_Created);
        }

        void Watcher_Created(object s, FileSystemEventArgs e)
        {
            var ext = e.Name.Substring(e.Name.LastIndexOf('.'));
            switch (ext)
            {
                case ".csv":
                    _converter.reader = new CSVReader();
                    _converter.ProcessFile(e, pathB);
                    break;
                case ".txt":
                    _converter.reader = new TXTReader();
                    _converter.ProcessFile(e, pathB);
                    break;
            }
        }

        private async Task CheckAndProcessFolder()
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
                            _converter.reader = new CSVReader();
                            await _converter.ProcessFile(null, pathB, x);
                            break;
                        case ".txt":
                            _converter.reader = new TXTReader();
                            await _converter.ProcessFile(null, pathB, x);
                            break;
                    }
                };
            }
        }
    }
}
