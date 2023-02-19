using CsvHelper.Configuration.Attributes;

namespace Data_Processing_Project.Model
{
    public class Transaction
    {
        [Name("first_name")]     
        public string FirstName { get; set; }
        [Name("last_name")]    
        public string LastName { get; set; }
        [Name("address")]
        public string Address { get; set; }
        [Name("payment")]
        public decimal Payment { get; set; }
        [Name("date")]
        public DateTime Date { get; set; }
        [Name("account_number")]
        public long AccountNumber { get; set; }
        [Name("service")]
        public string Service { get; set; }
    }
}
