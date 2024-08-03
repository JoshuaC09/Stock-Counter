using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface ICountSheetService
    {
        Task AddCountSheetAsync(string employeeCode, string description, DateTime date);
        Task DeleteCountSheetAsync(string countCode);
        Task EditCountSheetAsync(string countCode, string description);
        Task<IEnumerable<CountSheet>> ShowCountSheetAsync(string employeeCode);
    }
}
