using WebApplication2.Helpers;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class CountSheetService : ICountSheetService
    {
        private readonly IConnectionStringProviderService _connectionStringProvider;

        public CountSheetService(IConnectionStringProviderService connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public async Task AddCountSheetAsync(string employeeCode, string description, DateTime date)
        {
            using (var connection = await DatabaseHelper.GetOpenConnectionAsync(_connectionStringProvider))
            using (var command = DatabaseHelper.CreateCommand(connection, "add_count_sheet", ("_emp", employeeCode), ("_desc", description), ("_date", date)))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteCountSheetAsync(string countCode)
        {
            using (var connection = await DatabaseHelper.GetOpenConnectionAsync(_connectionStringProvider))
            using (var command = DatabaseHelper.CreateCommand(connection, "del_count_sheet", ("_cntcode", countCode)))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task EditCountSheetAsync(string countCode, string description)
        {
            using (var connection = await DatabaseHelper.GetOpenConnectionAsync(_connectionStringProvider))
            using (var command = DatabaseHelper.CreateCommand(connection, "edit_count_sheet", ("_cntcode", countCode), ("_desc", description)))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<CountSheet>> ShowCountSheetAsync(string employeeCode)
        {
            var countSheets = new List<CountSheet>();
            using (var connection = await DatabaseHelper.GetOpenConnectionAsync(_connectionStringProvider))
            using (var command = DatabaseHelper.CreateCommand(connection, "show_count_sheet", ("_emp", employeeCode)))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    countSheets.Add(new CountSheet
                    {
                        CountCode = reader.GetString("cnt_code"),
                        CountDescription = reader.GetString("cnt_desc"),
                        CountDate = reader.GetDateTime("cnt_date"),
                        CountSheetEmployee = employeeCode
                    });
                }
            }
            return countSheets;
        }
    }
}
