using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Services
{
    public class UserService : IUserService
    {
        private readonly IUserJsonRepository _userRepository;

        public UserService(IUserJsonRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.LoadUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        // Логика валидации — здесь, в сервисе. Репозиторий просто пишет в файл.
        public async Task<bool> AddUserAsync(string email, string login, string password)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Email, login и password не должны быть пустыми.");
                return false;
            }

            if (!email.Contains("@"))
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Некорректный email.");
                return false;
            }

            await _userRepository.AddUserAsync(email, login, password);
            return true;
        }

        public async Task<bool> UpdateUserAsync(int id, string email, string login, string password)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("[ОШИБКА ВАЛИДАЦИИ] Email, login и password не должны быть пустыми.");
                return false;
            }

            return await _userRepository.UpdateUserByIdAsync(id, email, login, password);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteUserByIdAsync(id);
        }
    }
}