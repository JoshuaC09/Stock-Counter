namespace WebApplication2.Interfaces
{
    public interface IConnectionStringProviderService
    {
        Task SetConnectionStringAsync(string connectionString, string remoteDatabase);
        Task<string> GetConnectionStringAsync();
        Task<string> GetRemoteDatabaseAsync();
        (string coreConnectionString, string remoteDatabase) ExtractConnectionString(string connectionString);
    }
}
