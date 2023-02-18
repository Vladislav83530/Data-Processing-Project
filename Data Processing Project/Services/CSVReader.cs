using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Data_Processing_Project.Model;
using Data_Processing_Project.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace Data_Processing_Project.Services
{
    internal class CSVReader : IFileReader
    {
        private readonly ILogger _logger;

        public CSVReader(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Reading scv file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>List of transaction and count of error lines</returns>
        public async Task<ReadingResult> ReadAsync(string path)
        {         
            int invalidRecord = 0;
            bool isRecordInvalid = false;

            using (var reader = new StreamReader(path))
            {
                _logger.LogInformation("Start processing file: {0}", path);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {                  
                    ReadingExceptionOccurred = args =>
                    {
                        if (args.Exception is TypeConverterException || args.Exception is FieldValidationException || args.Exception is ReaderException )
                        {
                            invalidRecord++;
                            _logger.LogCritical("Invalid line: {0}", args.Exception.Message.Trim().Split(":").TakeLast(1));
                            isRecordInvalid = true;
                            return false;
                        }   
                        return true;
                    }
                };
                var validRecord = new List<Transaction>();
                using (var csv = new CsvReader(reader, config))
                {
                    try
                    {
                        csv.Context.RegisterClassMap<TransactionClassMap>();
                        while (await csv.ReadAsync())
                        {
                            var record = csv.GetRecord<Transaction>();

                            if (!isRecordInvalid)
                                validRecord.Add(record);

                            isRecordInvalid = false;
                        }
                    }
                    catch(HeaderValidationException ex)
                    {
                        _logger.LogCritical("Invalid header in file {0}", path);
                    }
                }
                _logger.LogInformation("File {0} have {1} invalid lines", path, invalidRecord);
                _logger.LogInformation("File {0} have {1} valid lines", path, validRecord.Count());

                File.Delete(path);
                return new ReadingResult
                {
                    Transactions = validRecord,
                    InvalidRecord = invalidRecord
                };
            }
        }
    }
}