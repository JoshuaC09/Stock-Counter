using WebApplication1.DatabaseContext;
using WebApplication2;
using WebApplication2.Interfaces;
using WebApplication2.Security;
using WebApplication2.Services;
using WebApplication2.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<MyDbContextFactory>();
builder.Services.AddSingleton<IConnectionStringProviderService, ConnectionStringProviderService>();

var encryptionSettings = builder.Configuration.GetSection("EncryptionSettings");
var encryptionKey = AppSettingSecurity.EncryptionKey;
var saltString = AppSettingSecurity.EncryptionSalt;

if (encryptionKey == null)
{
    throw new ArgumentNullException(nameof(encryptionKey), "Encryption key must not be null.");
}

if (saltString == null)
{
    throw new ArgumentNullException(nameof(saltString), "Salt must not be null.");
}

var salt = Encoding.UTF8.GetBytes(saltString);
builder.Services.AddSingleton<ISecurityService>(new SecurityService(encryptionKey, salt));

builder.Services.AddScoped<ICountSheetService, CountSheetService>();
builder.Services.AddScoped<IItemCount, ItemCountService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddScoped<MyDbContext>(provider =>
{
    var connectionStringProvider = provider.GetRequiredService<IConnectionStringProviderService>();
    var dbContextFactory = provider.GetRequiredService<MyDbContextFactory>();
    var connectionStringTask = connectionStringProvider.GetConnectionStringAsync();
    connectionStringTask.Wait();
    var connectionString = connectionStringTask.Result;

    return dbContextFactory.CreateDbContext(connectionString);
});

var serviceProvider = builder.Services.BuildServiceProvider();
var securityService = serviceProvider.GetRequiredService<ISecurityService>();

var encryptedJwtKey = builder.Configuration["JwtSecretKey:Key"];
if (string.IsNullOrEmpty(encryptedJwtKey))
{
    throw new ArgumentNullException(nameof(encryptedJwtKey), "JWT key must not be null.");
}

string decryptedJwtKey;
try
{
    decryptedJwtKey = await securityService.DecryptAsync(encryptedJwtKey);
}
catch (Exception ex)
{
    throw new InvalidOperationException("Failed to decrypt the JWT key.", ex);
}

var appSettingSecurity = new AppSettingSecurity { DecryptedJwtKey = decryptedJwtKey };
builder.Services.AddSingleton(appSettingSecurity);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettingSecurity.DecryptedJwtKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiUser", policy => policy.RequireAuthenticatedUser());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
