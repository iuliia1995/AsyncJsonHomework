using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Repositories
{
    public interface IUserJsonRepository
    {
        Task<List<User>> LoadUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task AddUserAsync(string email, string login, string password);
        Task<bool> UpdateUserByIdAsync(Guid id, string email, string login, string password);
        Task<bool> DeleteUserByIdAsync(Guid id);
        Task SaveUsersAsync(List<User> users);
    }
}