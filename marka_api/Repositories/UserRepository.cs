using marka_api.Data;
using marka_api.Interfaces;
using marka_api.Models;
using Microsoft.EntityFrameworkCore;

namespace marka_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.id == id);
        }
        public async Task<User> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }
            // Soft delete instead of physical delete
            user.is_deleted = true;
            user.deleted_at = DateTime.UtcNow;
            user.deleted_by = 0; // Replace with actual user context
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
