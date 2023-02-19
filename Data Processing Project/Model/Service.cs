namespace Data_Processing_Project.Model
{
    internal class Service
    {
        public string Name { get; set; }
        public IList<Payer> Payers { get; set; }
        public decimal Total { get; set; }
    }
}
