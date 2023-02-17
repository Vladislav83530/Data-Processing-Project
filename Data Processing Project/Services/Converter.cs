﻿using Data_Processing_Project.Model;
using Data_Processing_Project.Services.Abstract;
using System.Text.Json;


namespace Data_Processing_Project.Services
{
    internal class Converter
    {
        public IFileReader reader { private get; set; }
        int countFile = 0;
        public Converter(IFileReader reader)
        {
            this.reader = reader;
        }

        public async Task ProcessFile(FileSystemEventArgs e, string pathB, string filePath = "")
        {
            countFile++;

            var readingResult = new ReadingResult();
            if (!String.IsNullOrEmpty(filePath))
                readingResult = await reader.ReadAsync(filePath);
            else
                readingResult = await  reader.ReadAsync(e.FullPath);

            var output = ConvertToOutput(readingResult.Transactions);

            string json = JsonSerializer.Serialize(output);
            using (StreamWriter writer = new StreamWriter(pathB + $"output{countFile}.json"))
            {
                writer.WriteLine(json);
            }

            if (!String.IsNullOrEmpty(filePath))
                File.Delete(filePath);
            else
                File.Delete(e.FullPath);
        }

        /// <summary>
        /// Convert list of Transaction to list of Output
        /// </summary>
        /// <param name="transactions"></param>
        /// <returns>list of Output</returns>
        private IEnumerable<Output> ConvertToOutput(IEnumerable<Transaction> transactions)
        {
            List<Output> result = new List<Output>();

            var sortedByCity = transactions.GroupBy(x => x.Address);
            foreach (var city in sortedByCity)
            {
                Output output = new Output() { Services = new List<Service>() };
                output.City = city.Key;

                decimal totalCity = 0;

                var sortedByService = city.GroupBy(x => x.Service);
                foreach (var service_ in sortedByService)
                {
                    Service service = new Service() { Payers = new List<Payer>() };
                    service.Name = service_.Key;

                    decimal totalService = 0;

                    foreach (var payer_ in service_)
                    {
                        Payer payer = new Payer()
                        {
                            Name = payer_.FirstName + " " + payer_.LastName,
                            Payment = payer_.Payment,
                            Date = payer_.Date,
                            AccountNumber = payer_.AccountNumber,
                        };
                        service.Payers.Add(payer);

                        totalService += payer_.Payment;
                    }

                    totalCity += totalService;
                    service.Total = totalService;

                    output.Total = totalCity;
                    output.Services.Add(service);
                }
                result.Add(output);
            }
            return result;
        }
    }
}