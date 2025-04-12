using System.Diagnostics;
using Users.Api.Logging;
using Users.Api.Models;
using Users.Api.Repositories;

namespace Users.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ILoggerAdapter<UserService> _logger;

        public UserService(IUserRepository repository, ILoggerAdapter<UserService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(User user)
        {
            _logger.LogInformation("Creating user with id {0} and name: {1}", user.Id, user.FullName);
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await _repository.CreateAsync(user);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while creating a user");
                throw;
            }
            finally 
            {
                stopWatch.Stop();
                _logger.LogInformation("User with id {0} created in {1}ms", user.Id, stopWatch.ElapsedMilliseconds);
            }
             

        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting user with id: {0}", id);
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await _repository.DeleteByIdAsync(id);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while deleting user with id {0}", id);
                throw;

            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("User with id {0} deleted in {1}ms", id, stopWatch.ElapsedMilliseconds);
            }
            
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all users");
            var stopWatch =  Stopwatch.StartNew();
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while retrieving all users");

                  throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("All users retrieved in {0}ms", stopWatch.ElapsedMilliseconds);
            }
            
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Retrieving user with id: {0}", id);
            var stopWatch = Stopwatch.StartNew();
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while retrieving user with id {0}", id);
                throw;
            }
            finally
            {
                stopWatch.Stop();
                _logger.LogInformation("User with id {0} retrieved in {1}ms", id, stopWatch.ElapsedMilliseconds);
            }
            
        }
    }
}
