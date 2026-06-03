using LearningBasics.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LearningBasics.Repository.Users
{
    public class UserRepository(IConfiguration configuration) : IUserRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnectionString");
        public async Task<int> CreateUser(User user)
        {
            int rowsAffected = 0;

            using(var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using(var cmd = new SqlCommand("sp_CreateUser", conn)) {
                    cmd.CommandType = CommandType.StoredProcedure;  
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Subjects", user.Subjects);
                    cmd.Parameters.AddWithValue("@DateofBirth", user.DateOfBirth);

                    rowsAffected = (int)await cmd.ExecuteScalarAsync();
                }
                
            }

            return rowsAffected;

        }

        public async Task<int> UpdateUser(User user)
        {
            int rowsAffected = 0;
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand("sp_UpdateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Subjects", user.Subjects);
                    cmd.Parameters.AddWithValue("@DateofBirth", user.DateOfBirth);
                    rowsAffected = (int)await cmd.ExecuteScalarAsync();
                }
            }
            return rowsAffected;
        }

        public async Task<int> DeleteUser(int id)
        {
            int rowsAffected = 0;
            using(var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using(var cmd = new SqlCommand("sp_DeleteUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", id);
                    rowsAffected = (int)await cmd.ExecuteScalarAsync();
                }
            }

            return rowsAffected;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = new List<User>();

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand("SELECT * FROM Users", conn))
                {
                    using(var reader = await cmd.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            var user = new User
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Subjects = reader.GetString(3),
                                DateOfBirth = reader.GetString(4)
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = new User();

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand("Select * from Users where Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    { 
                        if (await reader.ReadAsync())
                        {
                            user.Id = reader.GetInt32(0);
                            user.FirstName = reader.GetString(1);
                            user.LastName = reader.GetString(2);
                            user.Subjects = reader.GetString(3);
                            user.DateOfBirth = reader.GetString(4);
                        }
                    }
                }
            }
            

            return user;

        }


    }
}
