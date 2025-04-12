using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Users.Api.Contracts;
using Users.Api.Controllers;
using Users.Api.Mappers;
using Users.Api.Models;
using Users.Api.Services;

namespace Users.Api.Testing.Unit
{
    public class UserControllerTests
    {
        private readonly UserController _sut;
        private readonly IUserService _userService = Substitute.For<IUserService>();
        public UserControllerTests()
        {
            _sut = new UserController(_userService);
        }




        [Fact]
        public async Task GetAll_WhenUsersExist_ShouldReturnUsersResponse()
        {
            //Arrange 

            var users = new[] 
            {
                new User {Id = Guid.NewGuid(),FullName = "Ahmad" }
            };
            _userService.GetAllAsync().Returns(users);

            var usersResponse = users.Select(x => x.ToUserResponse());
            //Act 

           var result =  (OkObjectResult)await _sut.GetAll();

            //Assert 
            result.Value.As<IEnumerable<UserResponse>>().Should().BeEquivalentTo(usersResponse);
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAll_WhenUsersNotExist_ShouldReturnEmptyList()
        {
            //arrange 
            _userService.GetAllAsync().Returns(Enumerable.Empty<User>());
            //act 
            var result = (OkObjectResult)await _sut.GetAll();
            //assert 
            result.Value.As<IEnumerable<UserResponse>>().Should().BeEmpty();
            result.StatusCode.Should().Be(200);


        }

        [Fact]
        public async Task GetById_WhenUserExists_ShouldReturnOkAndObject()
        {
            //arrange 
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, FullName = "Ahmad" };
           _userService.GetByIdAsync(userId).Returns(user);
            var userReponse = user.ToUserResponse();

            //act 
            var result = (OkObjectResult) await _sut.GetById(userId);
           
            //assert 
            result.Value.As<UserResponse>().Should().BeEquivalentTo(userReponse);
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetById_WhenUserNotExists_ShouldReturnNotFound()
        {
            //arrange 

            _userService.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();
            //act 

            var result = (NotFoundResult) await _sut.GetById(Guid.NewGuid());

            //assert 
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task DeleteById_WhenUserWasDeleted_ShouldReturnOk()
        {
            //arrange 
             
            _userService.DeleteAsync(Arg.Any<Guid>()).Returns(true);

            //act 

            var result  = (OkResult)  await _sut.DeleteById(Guid.NewGuid());

            //assert 

            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task DeleteById_WhenUserWasNotDeleted_ShouldReturnNotFound()
        {
            //arrange 

            _userService.DeleteAsync(Arg.Any<Guid>()).Returns(false);
            
            //act 
           
            var result = (NotFoundResult) await _sut.DeleteById(Guid.NewGuid());

            //assert

            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Create_WhenCreateRequestIsValid_ShouldCraeteUser()
        {
            //arrange
             var user = new User {  FullName = "Ahmad" };
            _userService.CreateAsync(Arg.Do<User>(x => user=x)).Returns(true);
            

            //act 

           var result =  (CreatedAtActionResult) await _sut.Create(new CreateUserRequest { FullName = user.FullName });


            //aserrt 
            var userResponse = user.ToUserResponse();

            result.Value.As<UserResponse>().Should().BeEquivalentTo(userResponse);
            result.StatusCode.Should().Be(201);
           
        }
        
        [Fact]
        public async Task Create_WhenCreateUserRequestIsInvalid_ShouldReturnBadRequest()
        {
            //arrange 

            _userService.CreateAsync(Arg.Any<User>()).Returns(false);


            //act 

            var result = (BadRequestResult)await _sut.Create(new CreateUserRequest());


            //assert 

            result.StatusCode.Should().Be(400);


        }
    }
}
