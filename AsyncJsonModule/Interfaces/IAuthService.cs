using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Interfaces
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string login, string password);
    }
}