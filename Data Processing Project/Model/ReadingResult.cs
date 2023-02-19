namespace Data_Processing_Project.Model
{
    internal class ReadingResult
    {
        public IEnumerable<Transaction> Transactions { get; set; }
        public int InvalidRecord { get; set; }
    }
}