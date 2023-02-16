using CsvHelper.Configuration;
using System.Globalization;

namespace Data_Processing_Project.Model
{
    internal class TransactionClassMap : ClassMap<Transaction>
    {
        public TransactionClassMap() {
            Map(m => m.FirstName).Name("first_name")
                .Validate(field => !string.IsNullOrEmpty(field.Field));
            Map(m => m.LastName).Name("last_name")
                .Validate(field => !string.IsNullOrEmpty(field.Field));
            Map(m => m.Address).Name("address")
                .Validate(x => !string.IsNullOrEmpty(x.Field))
                .Convert(x=>x.Row.GetField("address").Split(',')[0]
                    .Trim()
                    .Replace("\"", ""));

            Map(m => m.Payment).Name("payment")
                .Validate(field => !string.IsNullOrEmpty(field.Field));
            Map(m => m.Date).Name("date")
                .Validate(field => !string.IsNullOrEmpty(field.Field));
            Map(x => x.Date).Convert(x =>
            {
                return DateTime.ParseExact(
                    x.Row.GetField("date"),
                    "yyyy-dd-mm",
                    CultureInfo.InvariantCulture);
            });
            Map(m => m.AccountNumber).Name("account_number")
                .Validate(field => !string.IsNullOrEmpty(field.Field));
            Map(m => m.Service).Name("service")
                .Validate(field => !string.IsNullOrEmpty(field.Field));
        }
    }
}
