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
            int invalidRecords = 0;
            List<string> records;
            using (StreamReader streamReader = new(path))
            {
                _logger.LogInformation("Start processing file: {0}", path);
                string fileContent = await streamReader.ReadToEndAsync();
                records = fileContent.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            List<Transaction> transactions = new List<Transaction>();

            foreach(var record in records) 
            {
                try
                {
                    if (!String.IsNullOrEmpty(record))
                    {
                        transactions.Add(await ParseStringToTransaction(record));
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    invalidRecords++;
                }
            };

            if(invalidRecords > 0)
                _logger.LogWarning("File {0} have {1} invalid lines", path, invalidRecords);
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
            var fields = record.Split(", ");
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