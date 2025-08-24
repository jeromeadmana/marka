using marka_api.Models;

namespace marka_api.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> CreateUser(User user);
        Task<bool> DeleteUser(int id);
    }
}
