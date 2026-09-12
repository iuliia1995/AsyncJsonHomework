using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AsyncJsonHomework.Interfaces;
using AsyncJsonHomework.Models;

namespace AsyncJsonHomework.Repositories
{
    public class UserJsonRepository : IUserJsonRepository
    {
        private static readonly string FilePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "users.json"));

        public async Task<List<User>> LoadUsersAsync()
        {
            Console.WriteLine($"Загрузка началась. (Поток {System.Threading.Thread.CurrentThread.ManagedThreadId})");

            try
            {
                if (!File.Exists(FilePath))
                {
                    return new List<User>();
                }

                string originalJson;
                using (var fileStream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Read,
                    FileShare.Read, bufferSize: 4096, useAsync: true))
                using (var reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    originalJson = await reader.ReadToEndAsync();
                }

                if (string.IsNullOrWhiteSpace(originalJson))
                {
                    originalJson = "[]";
                }

                var users = JsonSerializer.Deserialize<List<User>>(originalJson) ?? new List<User>();
                Console.WriteLine($"Загрузка завершена. (Поток {System.Threading.Thread.CurrentThread.ManagedThreadId})");
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных {ex.Message}");
                return new List<User>();
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                var users = await LoadUsersAsync();
                var user = users.FirstOrDefault(u => u.id == id);

                if (user == null)
                {
                    Console.WriteLine($"Пользователь с ID={id} не найден.");
                    return new User();
                }
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске пользователя {ex.Message}");
                return new User();
            }
        }

        public async Task<bool> UpdateUserByIdAsync(int id, string newEmail, string newLogin, string newPassword)
        {
            try
            {
                var users = await LoadUsersAsync();
                var user = users.FirstOrDefault(u => u.id == id);

                if (user == null)
                {
                    Console.WriteLine($"Пользователь с ID={id} не найден.");
                    return false;
                }

                user.email = newEmail;
                user.login = newLogin;
                user.password = newPassword;

                await SaveUsersAsync(users); // Полная перезапись, т.к. меняются данные в середине
                Console.WriteLine($"Данные пользователя ID={id} успешно изменены.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось изменить пользователя ID={id}: {ex.Message}");
                return false;
            }
        }

        public async Task AddUserAsync(string newEmail, string newLogin, string newPassword)
        {
            try
            {
                string content = "";
                if (File.Exists(FilePath))
                {
                    content = (await File.ReadAllTextAsync(FilePath)).Trim();
                }
                int nextId = 1;
                if (!string.IsNullOrEmpty(content) && content != "[]")
                {
                    var existingUsers = JsonSerializer.Deserialize<List<User>>(content) ?? new List<User>();
                    if (existingUsers.Any())
                    {
                        nextId = existingUsers.Max(u => u.id) + 1;
                    }
                }

                var newUser = new User
                {
                    id = nextId,
                    email = newEmail,
                    login = newLogin,
                    password = newPassword
                };

                string newUserJson = JsonSerializer.Serialize(newUser);

                using (var fileStream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                {
                    if (fileStream.Length == 0 || string.IsNullOrEmpty(content) || content == "[]")
                    {
                        fileStream.SetLength(0);
                        byte[] bytes = Encoding.UTF8.GetBytes("[\n  " + newUserJson + "\n]");
                        await fileStream.WriteAsync(bytes, 0, bytes.Length);
                    }
                    else
                    {
                        int lastBracketIndex = content.LastIndexOf(']');
                        if (lastBracketIndex == -1)
                        {
                            fileStream.SetLength(0);
                            byte[] bytes = Encoding.UTF8.GetBytes("[\n  " + newUserJson + "\n]");
                            await fileStream.WriteAsync(bytes, 0, bytes.Length);
                        }
                        else
                        {
                            fileStream.SetLength(lastBracketIndex);
                            fileStream.Seek(lastBracketIndex, SeekOrigin.Begin);
                            byte[] bytes = Encoding.UTF8.GetBytes(",\n  " + newUserJson + "\n]");
                            await fileStream.WriteAsync(bytes, 0, bytes.Length);
                        }
                    }
                }

                Console.WriteLine($"[УСПЕХ] Пользователь добавлен (дописан в конец файла). Присвоен ID {nextId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ОШИБКА] Не удалось добавить пользователя {ex.Message}");
            }
        }
        public async Task SaveUsersAsync(List<User> users)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(users, options);

                using (var fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write,
                    FileShare.None, bufferSize: 4096, useAsync: true))
                using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    await writer.WriteAsync(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных {ex.Message}");
            }
        }

        public async Task<bool> DeleteUserByIdAsync(int id)
        {
            try
            {
                var users = await LoadUsersAsync();
                var userToDelete = users.FirstOrDefault(u => u.id == id);

                if (userToDelete == null)
                {
                    Console.WriteLine($"Пользователь с ID={id} не найден — удаление не выполнено.");
                    return false;
                }

                users.Remove(userToDelete);

                await SaveUsersAsync(users);
                Console.WriteLine($"[УСПЕХ] Пользователь с ID={id} удалён.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении {ex.Message}");
                return false;
            }
        }
    }
}