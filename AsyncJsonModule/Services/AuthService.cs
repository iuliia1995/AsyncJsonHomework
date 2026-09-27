using System;
using System.Threading.Tasks;
using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Models;
using AsyncJsonModule.Repositories;

namespace AsyncJsonModule.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserJsonRepository _userRepository;

        public AuthService(IUserJsonRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> LoginAsync(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("[AUTH] Логин или пароль пустые.");
                return null;
            }

            var user = await _userRepository.GetByLoginAsync(login);

            if (user == null)
            {
                Console.WriteLine($"[AUTH] Пользователь с логином '{login}' не найден.");
                return null;
            }

            if (user.password != password)
            {
                Console.WriteLine($"[AUTH] Неверный пароль для логина '{login}'.");
                return null;
            }

            Console.WriteLine($"[AUTH] Успешный вход: {login}");
            return user;
        }
    }
}