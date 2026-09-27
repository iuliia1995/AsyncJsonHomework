using System;
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
        private readonly IAuthService _authService;
        private readonly IRegisterService _registerService;

        public UserController(
            IUserService userService,
            IAuthService authService,
            IRegisterService registerService)
        {
            _userService = userService;
            _authService = authService;
            _registerService = registerService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null || user.id == Guid.Empty)
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
                return BadRequest("Некорректные данные: email/login/password не должны быть пустыми, email должен содержать @.");

            return Ok("Пользователь успешно добавлен.");
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateUser(Guid id, [FromBody] User updatedUser)
        {
            if (updatedUser == null)
                return BadRequest("Тело запроса пустое.");

            var result = await _userService.UpdateUserAsync(id, updatedUser.email, updatedUser.login, updatedUser.password);
            if (!result)
                return NotFound($"Пользователь с ID={id} не найден или данные некорректны.");
            return Ok($"Пользователь ID={id} обновлён.");
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound($"Пользователь с ID={id} не найден.");
            return Ok($"Пользователь ID={id} удалён.");
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginRequest request)
        {
            if (request == null)
                return BadRequest("Тело запроса пустое.");

            var user = await _authService.LoginAsync(request.Login, request.Password);
            if (user == null)
                return Unauthorized("Неверный логин или пароль.");

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
                return BadRequest("Тело запроса пустое.");

            var user = await _registerService.RegisterAsync(request.Login, request.Email, request.Password);
            if (user == null)
                return Conflict("Логин или email уже заняты, либо данные некорректные.");

            return Ok(user);
        }

    }
    public class LoginRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}