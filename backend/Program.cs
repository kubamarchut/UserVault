using Microsoft.AspNetCore.Antiforgery;
using UserVault.Repositories;

var builder = WebApplication.CreateBuilder(args);

string connectionString = $"Server={(builder.Environment.IsDevelopment() ? "localhost": "db")};Database=UserVaultDb;User Id=sa;Password=mssqlP@ss;TrustServerCertificate=True;";

builder.Services.AddSingleton<UserRepository>(new UserRepository(connectionString));

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:9000")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .WithExposedHeaders("Content-Disposition");
        });
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserVault API v1");
        c.RoutePrefix = string.Empty;
    }
    );
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/api/csrf-token", (IAntiforgery antiforgery, HttpContext http) =>
{
    var tokens = antiforgery.GetAndStoreTokens(http);
    http.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!,
        new CookieOptions
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
    return Results.Ok(new { token = tokens.RequestToken });
});

app.Run();