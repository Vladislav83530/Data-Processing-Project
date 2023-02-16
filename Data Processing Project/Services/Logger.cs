namespace Data_Processing_Project.Services
{
    internal class Logger
    {
        public int ParsedFiles { get; set; }
        public int ParsedLines { get; set; }
        public int FoundError { get; set; }
        public IEnumerable<string> InvalidFiles { get; set; }
    }
}