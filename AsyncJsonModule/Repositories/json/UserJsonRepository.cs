using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Models;
using AsyncJsonModule.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace AsyncJsonModule.Repositories.Json
{
    public class UserJsonRepository : IUserJsonRepository
    {
        private static readonly string FilePath = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Data", "users.json"));

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public async Task<List<User>> LoadUsersAsync()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return new List<User>();
                }

                string json;
                using (var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read,
                    FileShare.Read, bufferSize: 4096, useAsync: true))
                using (var reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    json = await reader.ReadToEndAsync();
                }

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<User>();
                }

                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке пользователей: {ex.Message}");
                return new List<User>();
            }
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var users = await LoadUsersAsync();
            return users.FirstOrDefault(u => u.id == id) ?? new User();
        }

        public async Task AddUserAsync(string email, string login, string password)
        {
            try
            {
                var users = await LoadUsersAsync();

                var newUser = new User
                {
                    id = Guid.NewGuid(),
                    email = email,
                    login = login,
                    password = password
                };

                users.Add(newUser);
                await SaveUsersAsync(users);
                Console.WriteLine($"[УСПЕХ] Пользователь добавлен. ID: {newUser.id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ОШИБКА] Не удалось добавить пользователя: {ex.Message}");
            }
        }

        public async Task<bool> UpdateUserByIdAsync(Guid id, string email, string login, string password)
        {
            try
            {
                var users = await LoadUsersAsync();
                var user = users.FirstOrDefault(u => u.id == id);
                if (user == null) return false;

                user.email = email;
                user.login = login;
                user.password = password;

                await SaveUsersAsync(users);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении пользователя: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUserByIdAsync(Guid id)
        {
            try
            {
                var users = await LoadUsersAsync();
                var user = users.FirstOrDefault(u => u.id == id);
                if (user == null) return false;

                users.Remove(user);
                await SaveUsersAsync(users);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении пользователя: {ex.Message}");
                return false;
            }
        }



        public async Task SaveUsersAsync(List<User> users)
        {
            try
            {
                string json = JsonSerializer.Serialize(users, _jsonOptions);

                using (var fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write,
                    FileShare.None, bufferSize: 4096, useAsync: true))
                using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    await writer.WriteAsync(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении пользователей: {ex.Message}");
            }
        }
        public async Task<User?> GetByLoginAsync(string login)
        {
            var users = await LoadUsersAsync();
            return users.FirstOrDefault(u => u.login == login);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var users = await LoadUsersAsync();
            return users.FirstOrDefault(u => u.email == email);
        }
    }
}