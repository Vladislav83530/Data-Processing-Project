using Data_Processing_Project.Model;
using Data_Processing_Project.Services.Abstract;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Data_Processing_Project.Services
{
    internal class TXTReader : IFileReader
    {
        private readonly ILogger _logger;

        public TXTReader(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Reading txt file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>List of transaction and count of error lines</returns>
        public async Task<ReadingResult> ReadAsync(string path)
        {
            List<string> records;
            using (StreamReader streamReader = new(path))
            {
                string fileContent = await streamReader.ReadToEndAsync();
                records = fileContent.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            List<Transaction> transactions = new List<Transaction>();
            int invalidRecords = 0;

            Parallel.ForEach(records, async record =>
            {
                try
                {
                    transactions.Add(await ParseStringToTransaction(record));
                }
                catch
                {
                    _logger.LogCritical("Invalid line {0}", record);
                    invalidRecords++;
                }
            });

            _logger.LogInformation("File {0} have {1} invalid lines", path, invalidRecords);
            _logger.LogInformation("File {0} have {1} valid lines", path, transactions.Count());

            File.Delete(path);

            return new ReadingResult
            {
                Transactions = transactions,
                InvalidRecord = invalidRecords
            };
        }

        /// <summary>
        /// Parse string to Transaction model
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Transaction</returns>
        /// <exception cref="Exception"></exception>
        private async Task<Transaction> ParseStringToTransaction(string record)
        {
            var fields = record.Split(',');
            foreach (var field in fields)
            {
                if (string.IsNullOrEmpty(field))
                    throw new Exception("Field is null or empty");
            }

            var transaction = new Transaction();
            transaction.FirstName = fields[0].Trim();
            transaction.LastName = fields[1].Trim();
            transaction.Address = fields[2].Trim().Replace("\"", "");
            transaction.Payment = Decimal.Parse(fields[5].Trim(), CultureInfo.InvariantCulture);
            transaction.Date = DateTime.ParseExact(fields[6].Trim(), "yyyy-dd-mm", null);
            transaction.AccountNumber = Int64.Parse(fields[7].Trim(), CultureInfo.InvariantCulture);
            transaction.Service = fields[8].Trim();

            return transaction;
        }
    }
}