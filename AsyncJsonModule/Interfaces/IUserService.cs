using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<bool> AddUserAsync(string email, string login, string password);
        Task<bool> UpdateUserAsync(int id, string email, string login, string password);
        Task<bool> DeleteUserAsync(int id);
    }
}