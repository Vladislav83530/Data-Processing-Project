using CsvHelper;
using Data_Processing_Project;
using Data_Processing_Project.Services;
using Data_Processing_Project.Services.Abstract;
using System;
using System.Configuration;

CSVReader reader= new CSVReader();
//var result = await reader.ReadAsync(@"");
//foreach(var item in result.Transactions)
//{
//    Console.WriteLine(item.FirstName);
//    Console.WriteLine(item.LastName);
//    Console.WriteLine(item.Address);
//    Console.WriteLine(item.Payment);
//    Console.WriteLine(item.Date.ToString("yyyy -dd-mm"));
//    Console.WriteLine(item.AccountNumber);
//    Console.WriteLine(item.Service);
//    Console.WriteLine("----------------------");
//}
//Console.WriteLine(result.InvalidRecord);
//Converter converter = new();
//var results = await converter.ConvertToOutput(result.Transactions);
//foreach(var item in results)
//{
//    Console.WriteLine(item.City);
//    foreach(var service in item.Services)
//    {
//        Console.WriteLine($"-----{service.Name}");
//        foreach(var payer in service.Payers)
//        {
//            Console.WriteLine($"---------{payer.Name}");
//            Console.WriteLine($"---------{payer.Payment}");
//            Console.WriteLine($"---------{payer.Date}");
//            Console.WriteLine($"---------{payer.AccountNumber}");
//            Console.WriteLine();
//        }
//        Console.WriteLine($"-----{service.Total}");
//        Console.WriteLine();
//    }
//    Console.WriteLine(item.Total);
//    Console.WriteLine();
//}

Converter convert;
FileManager fm = new FileManager();
fm.ManageFile();

Console.Read();


//TXTReader readerTXT = new TXTReader();
//var resultTXT = await readerTXT.ReadAsync(@"C:\Users\vladp\OneDrive\Робочий стіл\FILE.txt");
//foreach (var item in resultTXT.Transactions)
//{
//    Console.WriteLine(item.FirstName);
//    Console.WriteLine(item.LastName);
//    Console.WriteLine(item.Address);
//    Console.WriteLine(item.Payment);
//    Console.WriteLine(item.Date.ToString("yyyy-dd-mm"));
//    Console.WriteLine(item.AccountNumber);
//    Console.WriteLine(item.Service);
//    Console.WriteLine("----------------------");
//}
//Console.WriteLine(resultTXT.InvalidRecord);