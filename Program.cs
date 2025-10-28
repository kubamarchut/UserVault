using UserVault.Repositories;

var builder = WebApplication.CreateBuilder(args);

string connectionString = "Server=localhost;Database=UserVaultDb;User Id=sa;Password=mssqlP@ss;TrustServerCertificate=True;";

builder.Services.AddSingleton<UserRepository>(new UserRepository(connectionString));

builder.Services.AddControllers();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var swagger = builder.Build();

if (swagger.Environment.IsDevelopment())
{
    swagger.UseSwagger();
    swagger.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserVault API v1");
        c.RoutePrefix = string.Empty;
    }
    );
}

swagger.UseHttpsRedirection();
swagger.UseAuthorization();
swagger.MapControllers();

swagger.Run();