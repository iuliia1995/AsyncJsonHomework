using System;
using System.Threading.Tasks;
using AsyncJsonModule.Repositories;
using AsyncJsonModule.Repositories.Json;

namespace AsyncJsonHomework
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"ОСНОВНОЙ ПОТОК ПРОГРАММЫ. (Поток {System.Threading.Thread.CurrentThread.ManagedThreadId})");
            Console.WriteLine();

            IUserJsonRepository repository = new UserJsonRepository();

            Console.WriteLine("Добавление пользователей:");
            await repository.AddUserAsync("ivan@mail.ru", "ivanzolo2004", "12345678qwerty");
            await repository.AddUserAsync("roma@mail.ru", "roman_romashka", "12345678qwerty");
            Console.WriteLine();

            Console.WriteLine("Загрузка всех пользователей:");
            var allUsers = await repository.LoadUsersAsync();
            foreach (var user in allUsers)
            {
                Console.WriteLine($"{user.id} | {user.login} | {user.email}");
            }
            Console.WriteLine();

            Console.WriteLine("Всё готово.");
        }
    }
}