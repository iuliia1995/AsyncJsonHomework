using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonHomework.Models;

namespace AsyncJsonHomework.Interfaces
{
    public interface IUserJsonRepository
    {
        Task<List<User>> LoadUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<bool> UpdateUserByIdAsync(int id, string newEmail, string newLogin, string newPassword);
        Task AddUserAsync(string newEmail, string newLogin, string newPassword);
        Task SaveUsersAsync(List<User> users);
        Task<bool> DeleteUserByIdAsync(int id);
    }
}