using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Contracts;
using Users.Api.Mappers;
using Users.Api.Models;
using Users.Api.Services;

namespace Users.Api.Controllers
{
    
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }



        [HttpGet("users")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            var userReponses = users.Select(u => u.ToUserResponse());
            return Ok(userReponses);
        }


        [HttpPost("users")]
        public async Task<IActionResult> Create ([FromBody] CreateUserRequest request)
        {
            var user = new User { FullName = request.FullName };


            var isCreated = await _userService.CreateAsync(user);

            if (!isCreated)
                return BadRequest();

            var userResponse = user.ToUserResponse();
            return CreatedAtAction(nameof(Create), new { id = userResponse.Id} ,userResponse);
        }


        [HttpGet("users/{id:guid}")]
        public async Task<IActionResult> GetById (Guid id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            var userResponse = user.ToUserResponse();

            return Ok(userResponse);

        }

        [HttpDelete("users/{id:guid}")]
        public async Task<IActionResult> DeleteById (Guid id)
        {
            var isDeleted = await _userService.DeleteAsync(id);

            if (!isDeleted)
                return NotFound();

            return Ok();
        }
    }
}
