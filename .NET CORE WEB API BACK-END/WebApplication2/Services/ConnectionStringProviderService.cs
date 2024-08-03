using WebApplication2.Interfaces;

namespace WebApplication2.Services
{
    public class ConnectionStringProviderService : IConnectionStringProviderService
    {
        private string _connectionString = string.Empty;
        private string _remoteDatabase = string.Empty;

        public Task SetConnectionStringAsync(string connectionString, string remoteDatabase)
        {
            _connectionString = connectionString;
            _remoteDatabase = remoteDatabase;
            return Task.CompletedTask;
        }

        public Task<string> GetConnectionStringAsync()
        {
            return Task.FromResult(_connectionString);
        }

        public Task<string> GetRemoteDatabaseAsync()
        {
            return Task.FromResult(_remoteDatabase);
        }

        public (string coreConnectionString, string remoteDatabase) ExtractConnectionString(string connectionString)
        {
            string coreConnectionString = string.Empty;
            string remoteDatabase = string.Empty;
            var parametersToExtract = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Server", "Database", "User", "Password", "RemoteDatabase"
            };

            foreach (var part in connectionString.Split(';'))
            {
                var keyValue = part.Split('=', 2);
                if (keyValue.Length == 2 && parametersToExtract.Contains(keyValue[0]))
                {
                    if (keyValue[0].Equals("RemoteDatabase", StringComparison.OrdinalIgnoreCase))
                    {
                        remoteDatabase = keyValue[1];
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(coreConnectionString))
                        {
                            coreConnectionString += ";";
                        }
                        coreConnectionString += part;
                    }
                }
            }

            return (coreConnectionString, remoteDatabase);
        }
    }
}
