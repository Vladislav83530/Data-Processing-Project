namespace Data_Processing_Project.Model
{
    internal class Output
    {
        public string City { get; set; }
        public IList<Service> Services { get; set; }
        public decimal Total { get; set; }
    }
}