using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;
using AsyncJsonModule.Repositories;
using Dapper;
using Npgsql;

namespace AsyncJsonModule.Repositories.PostgreSQL
{
    public class UserPostgresRepository : IUserJsonRepository
    {
        private readonly string _connectionString;

        public UserPostgresRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<User>> LoadUsersAsync()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"SELECT id, login, email, password
                               FROM ""Users""";

                var users = await connection.QueryAsync<User>(sql);
                return users.AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка загрузки пользователей: {ex.Message}");
                return new List<User>();
            }
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"SELECT id, login, email, password
                               FROM ""Users""
                               WHERE id = @Id";

                var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
                return user ?? new User();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка получения пользователя {id}: {ex.Message}");
                return new User();
            }
        }

        public async Task AddUserAsync(string email, string login, string password)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"INSERT INTO ""Users"" (id, login, email, password, created_at)
                               VALUES (@Id, @Login, @Email, @Password, @CreatedAt)";

                await connection.ExecuteAsync(sql, new
                {
                    Id = Guid.NewGuid(),
                    Login = login,
                    Email = email,
                    Password = password,
                    CreatedAt = DateTime.UtcNow
                });

                Console.WriteLine($"[POSTGRES] Пользователь {login} добавлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка добавления пользователя: {ex.Message}");
            }
        }

        public async Task<bool> UpdateUserByIdAsync(Guid id, string email, string login, string password)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"UPDATE ""Users""
                               SET login = @Login,
                                   email = @Email,
                                   password = @Password
                               WHERE id = @Id";

                int rows = await connection.ExecuteAsync(sql, new
                {
                    Id = id,
                    Login = login,
                    Email = email,
                    Password = password
                });

                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка обновления пользователя: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUserByIdAsync(Guid id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                string sql = @"DELETE FROM ""Users"" WHERE id = @Id";

                int rows = await connection.ExecuteAsync(sql, new { Id = id });
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POSTGRES] Ошибка удаления пользователя: {ex.Message}");
                return false;
            }
        }

        public Task SaveUsersAsync(List<User> users)
        {
            return Task.CompletedTask;
        }
    }
}