using System;
using System.Threading.Tasks;
using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Repositories;

namespace AsyncJsonHomework
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"ОСНОВНОЙ ПОТОК ПРОГРАММЫ. (Поток {System.Threading.Thread.CurrentThread.ManagedThreadId})");
            Console.WriteLine();

            IUserJsonRepository repository = new UserJsonRepository();

            Console.WriteLine("Добавление пользователей ");
            await repository.AddUserAsync("ivan@mail.ru", "ivanzolo2004", "12345678qwerty");
            await repository.AddUserAsync("roma@mail.ru", "roman_romashka", "12345678qwerty");
            await repository.AddUserAsync("nikita@mail.ru", "Smolnik228", "12345678qwerty");
            Console.WriteLine();

            Console.WriteLine("Загрузка всех пользователей ");
            var allUsers = await repository.LoadUsersAsync();
            foreach (var user in allUsers)
            {
                Console.WriteLine($"{user.id} | {user.login} | {user.email} | {user.password}");
            }
            Console.WriteLine();

            Console.WriteLine("Поиск пользователя с ID=2 ");
            var foundUser = await repository.GetUserByIdAsync(2);
            Console.WriteLine($"{foundUser.id} | {foundUser.login} | {foundUser.email} | {foundUser.password}");
            Console.WriteLine();

            Console.WriteLine("Обновление пользователя с ID=1 ");
            await repository.UpdateUserByIdAsync(1, "ivan_NEW@mail.ru", "ivan_new_login", "new_password");
            Console.WriteLine();

            Console.WriteLine("Удаление пользователя с ID=3 ");
            await repository.DeleteUserByIdAsync(3);
            Console.WriteLine();

            Console.WriteLine("Итоговый список после всех операций ");
            var finalUsers = await repository.LoadUsersAsync();
            foreach (var user in finalUsers)
            {
                Console.WriteLine($"{user.id} | {user.login} | {user.email} | {user.password}");
            }

            Console.WriteLine();
            Console.WriteLine("Всё готово.");
        }
    }
}