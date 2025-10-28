using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using UserVault.Model;

namespace UserVault.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<User> GetAllUsers()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();

                var users = db.Query<User>("SELECT * FROM Users").ToList();

                foreach (var user in users)
                {
                    var customProperties = db.Query<CustomProperty>(
                        "SELECT * FROM CustomProperties WHERE UserId = @UserId",
                        new { UserId = user.Id }
                    ).ToList();
                    foreach (var prop in customProperties)
                    {
                        user.AddCustomProperty(prop);
                    }
                }

                return users;
            }
        }

        public User? GetUserById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();
                var user = db.QueryFirstOrDefault<User>(
                    "SELECT * FROM Users WHERE Id = @Id",
                    new { Id = id }
                );

                if (user != null)
                {
                    var customProperties = db.Query<CustomProperty>(
                            "SELECT * FROM CustomProperties WHERE UserId = @UserId",
                            new { UserId = user.Id }
                        ).ToList();
                    foreach (var prop in customProperties)
                    {
                        user.AddCustomProperty(prop);
                    }
                }

                return user;
            }
        }
    }
}
