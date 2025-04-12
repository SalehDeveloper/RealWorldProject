using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using Users.Api.Data;
using Users.Api.Models;

namespace Users.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async  Task<bool> CreateAsync(User user)
        {
           await _context.Users.AddAsync(user);
           await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is null)
                return false;

                 _context.Users.Remove(user);
          await  _context.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();

            return users;   
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var user =await _context.Users.FindAsync(id);
            return user;
        }
    }
}
