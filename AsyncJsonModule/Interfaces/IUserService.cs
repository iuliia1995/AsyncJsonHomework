using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<bool> AddUserAsync(string email, string login, string password);
        Task<bool> UpdateUserAsync(Guid id, string email, string login, string password);
        Task<bool> DeleteUserAsync(Guid id);
    }
}