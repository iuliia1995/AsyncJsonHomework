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
        private readonly IUserJsonRepository _userRepository;

        public UserController(IUserJsonRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userRepository.LoadUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null || user.id == 0)
            {
                return NotFound($"Пользователь с ID={id} не найден.");
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser([FromBody] User newUser)
        {
            if (newUser == null)
            {
                return BadRequest("Тело запроса пустое.");
            }

            await _userRepository.AddUserAsync(newUser.email, newUser.login, newUser.password);
            return Ok("Пользователь успешно добавлен.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            if (updatedUser == null)
            {
                return BadRequest("Тело запроса пустое.");
            }

            var result = await _userRepository.UpdateUserByIdAsync(
                id, updatedUser.email, updatedUser.login, updatedUser.password);
            if (!result)
            {
                return NotFound($"Пользователь с ID={id} не найден.");
            }
            return Ok($"Пользователь ID={id} обновлён.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.DeleteUserByIdAsync(id);
            if (!result)
            {
                return NotFound($"Пользователь с ID={id} не найден.");
            }
            return Ok($"Пользователь ID={id} удалён.");
        }
    }
}