namespace WebApplication2.Interfaces
{
    public interface ISecurityService
    {
        byte[] DeriveKeyFromPassword(string ConnString, byte[] salt);
        Task<string> DecryptAsync(string cipherText);
        string GenerateWebToken(string key, string userName, int expireMinutes);
    }
}
