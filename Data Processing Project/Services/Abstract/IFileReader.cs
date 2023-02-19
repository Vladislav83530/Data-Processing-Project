using Data_Processing_Project.Model;

namespace Data_Processing_Project.Services.Abstract
{
    internal interface IFileReader
    {
        Task<ReadingResult> ReadAsync(string path);
    }
}
