using Data_Processing_Project.Model;

namespace Data_Processing_Project.Services.Abstract
{
    internal interface IConverter
    {
        public Task<IEnumerable<Output>> ConvertToOutput(IEnumerable<Transaction> transactions);
    }
}
