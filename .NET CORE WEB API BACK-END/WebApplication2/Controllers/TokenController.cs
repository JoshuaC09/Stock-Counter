using Microsoft.AspNetCore.Mvc;
using WebApplication2.Interfaces;
using WebApplication2.Models;
using WebApplication2.Helpers;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IConnectionStringProviderService _connectionStringProvider;
        private readonly string _jwtKey;

        public TokenController(ISecurityService securityService, IConnectionStringProviderService connectionStringProvider, AppSettingSecurity appSettingSecurity)
        {
            _securityService = securityService;
            _connectionStringProvider = connectionStringProvider;
            _jwtKey = appSettingSecurity.DecryptedJwtKey;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateToken()
        {
            var connectionString = await _connectionStringProvider.GetConnectionStringAsync();

            if (string.IsNullOrEmpty(connectionString))
            {
                return BadRequest("Connection string has not been set. Please set a valid connection string first.");
            }

     
            bool connectionSuccessful = await DatabaseHelper.TestConnectionAsync(connectionString);
            if (!connectionSuccessful)
            {
                return BadRequest("Unable to establish a connection to the database. Please check your connection string.");
            }
            var tokenString = _securityService.GenerateWebToken(_jwtKey, "Stock_Counter", 300);
            return Ok(new { Token = tokenString });
        }
    }
}