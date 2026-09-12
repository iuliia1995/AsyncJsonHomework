using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AsyncJsonHomework.Models;

namespace AsyncJsonHomework
{
    public class Program
    {
        private static readonly string FilePath = Path.GetFullPath(
    Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "users.json"));
        static async Task Main(string[] args)
        {
            Console.WriteLine("Программа запущена");

            var newUser = new User
            {
                email = "test@example.com",
                login = "test_user",
                password = "qwerty123"
            };

            Console.WriteLine($"Попытка добавить пользователя с логином {newUser.login}");
            await AddUserAppendAsync(newUser);

            Console.WriteLine("Программа завершена");
        }

        public static async Task AddUserAppendAsync(User newUser)
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
                newUser.id = nextId;

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

                Console.WriteLine($"[УСПЕХ] Пользователь добавлен. Присвоен ID {nextId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ОШИБКА] Не удалось добавить пользователя {ex.Message}");
            }
        }
    }
}