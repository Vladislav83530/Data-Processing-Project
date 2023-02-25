using Data_Processing_Project.Model;
using System.Configuration;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Data_Processing_Project
{
    internal class MetaLogger
    {
        private readonly MetaLogInfo metaLogInfo;
        public MetaLogger(FileManager fm)
        {
            metaLogInfo = fm._converter.metaLogInfo;
        }

        /// <summary>
        /// Timer for meta.log file
        /// </summary>
        public void MetaLogTimer()
        {
            var now = DateTime.Now;
            var midnight = now.Date.AddDays(1);
            var timer = new Timer(midnight.Subtract(now).TotalMilliseconds);

            timer.Elapsed += new ElapsedEventHandler(CreateFile);

            timer.Start();
        }

        /// <summary>
        /// Create meta.log and write value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CreateFile(object sender, ElapsedEventArgs e)
        {
            var result = $"Parsed files: {metaLogInfo.ParsedFiles} \nParsed Lines: {metaLogInfo.ParsedLines} \n" +
                $"FoundError: {metaLogInfo.FoundError} \nInvalid Files: [{string.Join(", ", metaLogInfo.InvalidFiles)}]";

            var fileName = $"meta.log";
            using (StreamWriter wr = new StreamWriter(Path.Combine(ConfigurationManager.AppSettings["folderB"], $"{DateTime.Now.AddDays(-1).ToString("dd-MM-yyyy")}/{fileName}")))
            {
                wr.WriteLine(result);
            }

            metaLogInfo.ParsedLines = 0;
            metaLogInfo.ParsedFiles = 0;
            metaLogInfo.FoundError = 0;
            metaLogInfo.InvalidFiles = new List<string>();
            var now = DateTime.Now;
            var midnight = now.Date.AddDays(1);

            var timer = (Timer)sender;
            timer.Interval = midnight.Subtract(now).TotalMilliseconds;
        }
    }
}