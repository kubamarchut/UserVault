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

        public void AddUser(User user)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();
                var userId = db.ExecuteScalar<int>(
                    "INSERT INTO Users (FirstName, LastName, DateOfBirth, Sex)" +
                    "VALUES (@FirstName, @LastName, @DateOfBirth, @Sex);" +
                    "SELECT CAST(SCOPE_IDENTITY() AS int);",
                    new
                    {
                        user.FirstName,
                        user.LastName,
                        user.DateOfBirth,
                        Sex = user.Sex.ToString()
                    });
                user.Id = userId;

                foreach (var prop in user.GetCustomProperties())
                {
                    var propId = db.ExecuteScalar<int>(
                    "INSERT INTO CustomProperties (UserId, Name, Value)" +
                    "VALUES (@UserId, @Name, @Value);" +
                    "SELECT CAST(SCOPE_IDENTITY() AS int);",
                    new
                    {
                        UserId = user.Id,
                        prop.Name,
                        prop.Value
                    });

                    prop.Id = propId;
                }

            }
        }

        public void RemoveUser(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();
                db.Execute("DELETE FROM CustomProperties WHERE UserId = @UserId", new { UserId = id });
                db.Execute("DELETE FROM Users WHERE Id = @Id", new { Id = id });
            }
        }

        public void UpdateUser(User user)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();
                db.Execute(
                    "UPDATE Users SET Firstname = @Firstname, LastName = @LastName, DateOfBirth = @DateOfBirth, Sex = @Sex WHERE Id = @Id",
                    new
                    {
                        user.FirstName,
                        user.LastName,
                        user.DateOfBirth,
                        Sex = user.Sex.ToString(),
                        user.Id
                    }
                );
                var incomingProps = user.GetCustomProperties();

                var existingProps = db.Query<CustomProperty>(
                    "SELECT * FROM CustomProperties WHERE UserId = @UserId",
                    new { UserId = user.Id }
                ).ToList();

                var incomingIds = incomingProps.Where(p => p.Id != 0).Select(p => p.Id).ToHashSet();
                var toDelete = existingProps.Where(p => !incomingIds.Contains(p.Id)).ToList();

                if (toDelete.Any())
                {
                    db.Execute(
                        "DELETE FROM CustomProperties WHERE Id IN @Ids",
                        new { Ids = toDelete.Select(p => p.Id).ToList() }
                    );
                }

                if (toDelete.Any())
                {
                    db.Execute(
                        "DELETE FROM CustomProperties WHERE Id IN @Ids",
                        new { Ids = toDelete.Select(p => p.Id).ToList() }
                    );
                }

                var toUpdate = incomingProps.Where(p => p.Id != 0).ToList();
                foreach (var prop in toUpdate)
                {
                    db.Execute(
                        "UPDATE CustomProperties SET Name = @Name, Value = @Value WHERE Id = @Id",
                        new { prop.Name, prop.Value, prop.Id }
                    );
                }

                var toInsert = incomingProps.Where(p => p.Id == 0).ToList();
                foreach (var prop in toInsert)
                {
                    var propId = db.ExecuteScalar<int>(
                        @"INSERT INTO CustomProperties (UserId, Name, Value) 
                        VALUES (@UserId, @Name, @Value); 
                        SELECT CAST(SCOPE_IDENTITY() AS int);",
                        new { UserId = user.Id, prop.Name, prop.Value }
                    );
                    prop.Id = propId;
                }
            }
        }
    }
}
