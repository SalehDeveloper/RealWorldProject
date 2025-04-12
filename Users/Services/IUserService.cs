using Users.Api.Models;

namespace Users.Api.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetAllAsync();

        public Task<User?> GetByIdAsync(Guid id);

        public Task<bool> CreateAsync(User user);

        public Task<bool> DeleteAsync(Guid id);

    }
}
