using Calmy_Focus_App.Models;
using System.Threading.Tasks;

namespace Calmy_Focus_App.Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
    }
}