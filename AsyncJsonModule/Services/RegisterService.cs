using System;
using System.Threading.Tasks;
using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Models;
using AsyncJsonModule.Repositories;

namespace AsyncJsonModule.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserJsonRepository _userRepository;

        public RegisterService(IUserJsonRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> RegisterAsync(string login, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("[REGISTER] Логин, email и пароль не должны быть пустыми.");
                return null;
            }

            if (!email.Contains("@"))
            {
                Console.WriteLine("[REGISTER] Некорректный email.");
                return null;
            }

            var existingByLogin = await _userRepository.GetByLoginAsync(login);
            if (existingByLogin != null)
            {
                Console.WriteLine($"[REGISTER] Логин '{login}' уже занят.");
                return null;
            }

            var existingByEmail = await _userRepository.GetByEmailAsync(email);
            if (existingByEmail != null)
            {
                Console.WriteLine($"[REGISTER] Email '{email}' уже занят.");
                return null;
            }

            await _userRepository.AddUserAsync(email, login, password);

            var newUser = await _userRepository.GetByLoginAsync(login);
            Console.WriteLine($"[REGISTER] Пользователь '{login}' зарегистрирован.");
            return newUser;
        }
    }
}