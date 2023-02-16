using Data_Processing_Project.Services;

CSVReader reader= new CSVReader();
var result = await reader.ReadAsync("");
foreach(var item in result.Transactions)
{
    Console.WriteLine(item.FirstName);
    Console.WriteLine(item.LastName);
    Console.WriteLine(item.Address);
    Console.WriteLine(item.Payment);
    Console.WriteLine(item.Date.ToString("yyyy-dd-mm"));
    Console.WriteLine(item.AccountNumber);
    Console.WriteLine(item.Service);
    Console.WriteLine("----------------------");
}
Console.WriteLine(result.InvalidRecord);


TXTReader readerTXT = new TXTReader();
var resultTXT = await readerTXT.ReadAsync("");
foreach (var item in resultTXT.Transactions)
{
    Console.WriteLine(item.FirstName);
    Console.WriteLine(item.LastName);
    Console.WriteLine(item.Address);
    Console.WriteLine(item.Payment);
    Console.WriteLine(item.Date.ToString("yyyy-dd-mm"));
    Console.WriteLine(item.AccountNumber);
    Console.WriteLine(item.Service);
    Console.WriteLine("----------------------");
}
Console.WriteLine(resultTXT.InvalidRecord);