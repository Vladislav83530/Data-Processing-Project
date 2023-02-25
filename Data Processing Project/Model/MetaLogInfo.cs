namespace Data_Processing_Project.Model
{
    internal class MetaLogInfo
    {
        public int ParsedFiles { get; set; }
        public int ParsedLines { get; set; }
        public int FoundError { get; set; }
        public List<string> InvalidFiles { get; set; }
    }
}
