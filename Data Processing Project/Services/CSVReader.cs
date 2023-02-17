using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Data_Processing_Project.Model;
using Data_Processing_Project.Services.Abstract;

namespace Data_Processing_Project.Services
{
    internal class CSVReader : IFileReader
    {
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
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {                  
                    ReadingExceptionOccurred = args =>
                    {
                        if (args.Exception is TypeConverterException || args.Exception is FieldValidationException || args.Exception is ReaderException)
                        {
                            invalidRecord++;
                            isRecordInvalid = true;
                            return false;
                        }   
                        return true;
                    }
                };

                using (var csv = new CsvReader(reader, config))
                {
                    var validRecord = new List<Transaction>();
                    csv.Context.RegisterClassMap<TransactionClassMap>();
                    while (await csv.ReadAsync())
                    {                                   
                        var record = csv.GetRecord<Transaction>();
                        
                        if (!isRecordInvalid)
                            validRecord.Add(record);

                        isRecordInvalid = false;
                    }

                    return new ReadingResult {
                        Transactions = validRecord,
                        InvalidRecord = invalidRecord
                    };
                }               
            }            
        }
    }
}