namespace Data_Processing_Project.Model
{
    internal class MetaLogInfo : IDisposable
    {
        public int ParsedFiles { get; set; }
        public int ParsedLines { get; set; }
        public int FoundError { get; set; }
        public List<string> InvalidFiles { get; set; }

        public void Dispose()
        {
            ParsedLines = 0;
            ParsedFiles = 0;
            FoundError = 0;
            InvalidFiles = new List<string>();
        }
    }
}
