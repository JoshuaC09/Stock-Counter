using MySqlConnector;
using System.Data;
using WebApplication2.Interfaces;

namespace WebApplication2.Helpers
{
    public static class DatabaseHelper
    {
        public static async Task<MySqlConnection> GetOpenConnectionAsync(IConnectionStringProviderService connectionStringProvider)
        {
            var connectionString = await connectionStringProvider.GetConnectionStringAsync();
            var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public static MySqlCommand CreateCommand(MySqlConnection connection, string storedProcedure, params (string, object)[] parameters)
        {
            var command = new MySqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
            }
            return command;
        }

        public static async Task<bool> TestConnectionAsync(string connectionString)
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch (Exception ex)
            {
              
                return false;
            }
        }
    }
}
