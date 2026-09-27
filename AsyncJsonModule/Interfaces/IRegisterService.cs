using System.Threading.Tasks;
using AsyncJsonModule.Models;

namespace AsyncJsonModule.Interfaces
{
    public interface IRegisterService
    {
        Task<User?> RegisterAsync(string login, string email, string password);
    }
}