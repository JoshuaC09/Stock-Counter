using WebApplication2.Models;

namespace WebApplication2.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync(string databaseName, string pattern);
    }
}
