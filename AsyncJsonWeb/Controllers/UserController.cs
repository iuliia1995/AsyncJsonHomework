using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncJsonModule.Interfaces;
using AsyncJsonModule.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsyncJsonWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null || user.id == 0)
                return NotFound($"Пользователь с ID={id} не найден.");
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] User newUser)
        {
            if (newUser == null)
                return BadRequest("Тело запроса пустое.");

            var result = await _userService.AddUserAsync(newUser.email, newUser.login, newUser.password);
            if (!result)
                return BadRequest("Некорректные данные. email/login/password не должны быть пустыми, email должен содержать @.");

            return Ok("Пользователь успешно добавлен.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (updatedUser == null)
                return BadRequest("Тело запроса пустое.");

            var result = await _userService.UpdateUserAsync(id, updatedUser.email, updatedUser.login, updatedUser.password);
            if (!result)
                return NotFound($"Пользователь с ID={id} не найден или данные некорректны.");
            return Ok($"Пользователь ID={id} обновлён.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound($"Пользователь с ID={id} не найден.");
            return Ok($"Пользователь ID={id} удалён.");
        }
    }
}