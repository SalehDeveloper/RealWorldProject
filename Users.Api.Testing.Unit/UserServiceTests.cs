using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Users.Api.Logging;
using Users.Api.Models;
using Users.Api.Repositories;
using Users.Api.Services;

namespace Users.Api.Testing.Unit
{  
    //Complete UserService Test
    public class UserServiceTests
    {
        private readonly UserService _sut;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly ILoggerAdapter<UserService> _logger  = Substitute.For<ILoggerAdapter<UserService>>();
        public UserServiceTests()
        {

            _sut = new UserService(_userRepository , _logger);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoUsersExist_ShouldReturnAnEmptyList()
        {
             //Arrange
            _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>());

            //Act 
            var result = await _sut.GetAllAsync();  
           
            //Assert 
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_WhenSomeUsersExist_ShouldReturnListOfUsers()
        {
            var expectedUsers = new List<User> 
            {
               new User {Id = Guid.NewGuid() , FullName = "Ahmad"},
               new User {Id = Guid.NewGuid() , FullName = "Ali"}
            };


            // arrange 

            _userRepository.GetAllAsync().Returns(expectedUsers);
            
            
            //act 

            var result = await _sut.GetAllAsync();
            
            //assert
            result.Should().BeEquivalentTo(expectedUsers);
            result.Should().HaveCount(2);


        }

        [Fact]
        public async Task GetAllAsync_WhenInvoked_ShouldLogMessages()
        {
            //Arrange 
            _userRepository.GetAllAsync().Returns(Enumerable.Empty<User>());
            //Act 
           await _sut.GetAllAsync();
            //Assert  
            _logger.Received(1).LogInformation(Arg.Is("Retrieving all users"));
            _logger.Received(1).LogInformation(Arg.Is("All users retrieved in {0}ms") ,  Arg.Any<long>());


        }

        [Fact]
        public async Task GetAllAsync_WhenExceptionIsThrown_ShouldLogMessageAndException()
        {
            //Arrange 
            var ex = new Exception("Something went wrong");
            _userRepository.GetAllAsync().Throws(ex);
            //Act 
           var requestAction = async()=>  await _sut.GetAllAsync();

            //Assert

          await  requestAction.Should().ThrowAsync<Exception>()
                .WithMessage("Something went wrong");

            _logger.Received(1).LogError(Arg.Is(ex), "Something went wrong while retrieving all users");



        }

        //-------------------------------


        [Fact]
        public async Task GetByIdAsync_WhenUserNotExists_ShouldReturnNull()
        {
            //Arrange 
            
            var userId= Guid.NewGuid();
            _userRepository.GetByIdAsync(userId).ReturnsNull();

            //Act 
            var result = await _sut.GetByIdAsync(userId);

            //Assert 
            result.Should().BeNull();

        }

        [Fact]
        public async Task GetByIdAsync_WhenUserExists_ShouldReturnUser()
        { 
            //Arrange
            
            var userId= Guid.NewGuid();
            var expectedUser= new User { Id =userId  , FullName="Ahmad"};
            _userRepository.GetByIdAsync(userId).Returns(expectedUser);
            
            //Act

            var result = await _sut.GetByIdAsync(userId);
            
            //Assert 
             
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task GetByIdAsync_WhenInvoked_ShouldLogMessages()
        {
            //Arrange

            var userId = Guid.NewGuid();
            _userRepository.GetByIdAsync(userId).ReturnsNull();

            //Act 

            await _sut.GetByIdAsync(userId);

            //Assert

            _logger.Received(1).LogInformation("Retrieving user with id: {0}", Arg.Is(userId));
            _logger.Received(1).LogInformation("User with id {0} retrieved in {1}ms" ,Arg.Is(userId) , Arg.Any<long>());
        }

        [Fact]
        public async Task GetByIdAsync_WhenExceptionIsThrown_ShouldLogMessageAndException()
        {

            //Arrange 

            var ex = new Exception("something went wrong");
            var userId = Guid.NewGuid();

            _userRepository.GetByIdAsync(userId).ThrowsAsync(ex);

            //Act 

            var requestAction = async () =>await _sut.GetByIdAsync(userId);

            //Assert 
           await requestAction.Should().ThrowAsync<Exception>().WithMessage("something went wrong");

           _logger.Received(1).LogInformation("Retrieving user with id: {0}" , Arg.Is(userId));
           _logger.Received(1).LogError(Arg.Is(ex), "Something went wrong while retrieving user with id {0}", Arg.Is(userId));
           _logger.Received(1).LogInformation("User with id {0} retrieved in {1}ms", Arg.Is(userId), Arg.Any<long>());

        }


        //-------------------------------------

        [Fact]
        public async Task DeleteAsync_WhenUserExist_ShouldReturnTrue()
        {
            //Arrange
            
            var userId = Guid.NewGuid();
            _userRepository.DeleteByIdAsync(userId).Returns(true);

            //Act 
            var result = await _sut.DeleteAsync(userId);

            //Assert 

            result.Should().BeTrue();


        }

        [Fact]
        public async Task DeleteAsync_WhenUserNotExists_ShouldReturnFalse()
        { 

            //Arrange
            var userId = Guid.NewGuid();
            _userRepository.DeleteByIdAsync(userId).Returns(false);

            //Act 
            var result = await _sut.DeleteAsync(userId);

            //Assert 

            result.Should().BeFalse();

        }

        [Fact]
        public async Task DeleteAsync_WhenCalled_ShouldLogMessages()
        {
            //Arrange

            var userId = Guid.NewGuid();
            _userRepository.DeleteByIdAsync(userId).Returns(true);

            //Act 

            await _sut.DeleteAsync(userId);

            //Assert

            _logger.Received(1).LogInformation("Deleting user with id: {0}", Arg.Is(userId));
            _logger.Received(1).LogInformation("User with id {0} deleted in {1}ms", Arg.Is(userId), Arg.Any<long>());
        }

        [Fact]
        public async Task DeleteAsync_WhenExceptionIsThrown_ShouldLogMessagesAndException()
        {
            //Arrange 

            var ex = new Exception("something went wrong");
            var userId = Guid.NewGuid();

            _userRepository.DeleteByIdAsync(userId).ThrowsAsync(ex);

            //Act 

            var requestAction = async () => await _sut.DeleteAsync(userId);

            //Assert 
            await requestAction.Should().ThrowAsync<Exception>().WithMessage("something went wrong");

            _logger.Received(1).LogInformation("Deleting user with id: {0}", Arg.Is(userId));
            _logger.Received(1).LogError(Arg.Is(ex), "Something went wrong while deleting user with id {0}", Arg.Is(userId));
            _logger.Received(1).LogInformation("User with id {0} deleted in {1}ms", Arg.Is(userId), Arg.Any<long>());
        }


        //-------------------------------


        [Fact]
        public async Task CreateAsync_WhenUserCreated_ShouldReturnTrue()
        {
            //Arrange 

            var user  = new User { Id = Guid.NewGuid(), FullName = "Ahmad Saleh" };
            _userRepository.CreateAsync(user).Returns(true);

            //Assert 

            var result  = await _sut.CreateAsync(user); 

            //Act 

            result.Should().BeTrue();



        }

        [Fact]
        public async Task CreateAsync_WhenInvoked_ShouldLogMessages()
        {
            //Arrange 

            var user = new User { Id = Guid.NewGuid(), FullName = "Ahmad Saleh" };
            _userRepository.CreateAsync(user).Returns(true);

            //Assert 

            var result = await _sut.CreateAsync(user);

            //Act 

            _logger.Received(1).LogInformation("Creating user with id {0} and name: {1}", Arg.Is(user.Id) , Arg.Is(user.FullName));
            _logger.Received(1).LogInformation("User with id {0} created in {1}ms", Arg.Is(user.Id), Arg.Any<long>());



        }

        [Fact]
        public async Task CreateAsync_WhenExceptionIsThrown_ShouldLogMessagesAndException()
        {

            //Arrange 
            var ex = new Exception("something went wrong");
            var user = new User { Id = Guid.NewGuid(), FullName = "Ahmad Saleh" };
            _userRepository.CreateAsync(user).ThrowsAsync(ex);

            //Act 

            var requestActiom =  async() => await _sut.CreateAsync(user);

            //Assert 
            await  requestActiom.Should().ThrowAsync<Exception>("something went wrong");
            _logger.Received(1).LogInformation("Creating user with id {0} and name: {1}", Arg.Is(user.Id), Arg.Is(user.FullName));
            _logger.Received(1).LogError(Arg.Is(ex), "Something went wrong while creating a user");
            _logger.Received(1).LogInformation("User with id {0} created in {1}ms", Arg.Is(user.Id), Arg.Any<long>());
        }

    }
}