using LearningBasics.DTOs.Request;
using LearningBasics.DTOs.Response;
using LearningBasics.Models;
using LearningBasics.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningBasics.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController(IUserService userService,ILogger<HomeController> _logger) : ControllerBase
    {
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<GetUserResponse>>> GetUsers()
        {
            userService = null;
            return Ok(await userService.GetUsersAsync());
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            return Ok(await userService.GetUserByIdAsync(id));
        }

        [HttpPost("users")]
        public async Task<ActionResult<int>> CreateUser(CreateUserRequest user)
        {
            return await userService.CreateUserAsync(user);
        }

        [HttpPut("users")]
        public async Task<ActionResult<string>> UpdateUser(UpdateUserRequest user)
        {
            var rowsAffected = await userService.UpdateUserAsync(user);
            return rowsAffected > 0 ? Ok($"Successfully updated user {user.FirstName}")
                : BadRequest($"Unable to update user {user.FirstName}");
        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult<int>> DeleteUserByIdAsync(int id)
        {
            var rowsAffected = await userService.DeleteUserByIdAsync(id);
            return rowsAffected > 0 ? Ok($"Successfully deleted user with id {id}")
                : BadRequest($"Unable to delete user with id {id}");
        }

    }
}
