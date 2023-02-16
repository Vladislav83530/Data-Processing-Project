﻿using Data_Processing_Project.Model;
using Data_Processing_Project.Services.Abstract;
using System.Globalization;

namespace Data_Processing_Project.Services
{
    internal class TXTReader : IFileReader
    {
        public async Task<ReadingResult> ReadAsync(string path)
        {
            IEnumerable<string> records;
            using (StreamReader streamReader = new(path))
            {
                string fileContent = await streamReader.ReadToEndAsync();
                records = fileContent.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            }

            List<Transaction> transactions = new List<Transaction>();
            int invalidRecords  = 0;

            foreach (var record in records)
            {
                try
                {
                    var transaction = await ParseStringToTransactionAsync(record);
                    transactions.Add(await ParseStringToTransactionAsync(record));
                }
                catch
                {
                    invalidRecords++;
                }
            }

            return new ReadingResult { 
                Transactions = transactions,
                InvalidRecord= invalidRecords
            };
        }

        private async Task<Transaction> ParseStringToTransactionAsync(string record)
        {
            var fields = record.Split(',');
            foreach(var field in fields)
            {
                if (string.IsNullOrEmpty(field))
                    throw new Exception("Field is null or empty");
            }

            var transaction = new Transaction();
            transaction.FirstName= fields[0].Trim();
            transaction.LastName= fields[1].Trim();
            transaction.Address = (fields[2]).Trim().Replace("\"", "");
            transaction.Payment =  Decimal.Parse(fields[5].Trim(), CultureInfo.InvariantCulture);
            transaction.Date = DateTime.ParseExact(fields[6].Trim(), "yyyy-dd-mm", null);
            transaction.AccountNumber = Int64.Parse(fields[7].Trim(), CultureInfo.InvariantCulture);
            transaction.Service = fields[8].Trim();

            return transaction;
        }
    }
}