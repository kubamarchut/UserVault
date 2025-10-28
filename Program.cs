using System;
using System.Linq;
using UserVault.Model;
using UserVault.Repositories;

class Program
{
    public static void Main(string[] args)
    {
        string connectionString = "Server=localhost;Database=UserVaultDb;User Id=sa;Password=mssqlP@ss;TrustServerCertificate=True;";

        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        // Create an instance of UserRepository
        var userRepository = new UserRepository(connectionString);

        // Fetch all users from the database

        // Map the root endpoint to print user info
        app.MapGet("/", () =>
        {
            var users = userRepository.GetAllUsers();
            return string.Join("\n\n", users.Select(user =>
                $"ID: {user.Id}, Name: {user.Firstname} {user.Lastname}, BirthDate: {user.DateOfBirth.ToShortDateString()}, Sex: {user.Sex}, CustomProperties: {user.ShowCustomProperties()}"));
        });

        // Run the application
        app.Run();
    }
}
