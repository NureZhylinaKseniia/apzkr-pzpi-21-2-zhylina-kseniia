using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Application.Models;
using Application.Repositories;
using System;
using Application.ViewModels;
using Microsoft.EntityFrameworkCore;
using Application.DBContext;

namespace Application.Controllers
{
    // Контролер для операцій, пов'язаних з користувачами
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        // Конструктор контролера, де відбувається ініціалізація залежностей
        public UserController(IUserRepository userService, DBContext.AppDbContext context)
        {
            _userRepository = userService;
            _context = context;
        }


        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                // Отримання всіх усиновлень та повернення їх у відповіді
                var User = _userRepository.GetAllUsers().ToList();
                return Ok(User);
            }
            catch (Exception ex)
            {
                // Повернення помилки у відповіді при виникненні виключення
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // Операція для отримання користувача за ідентифікатором
        [HttpGet("{id}")]
        [ProducesResponseType(200)] // ОК
        [ProducesResponseType(404)] // Не знайдено
        public IActionResult GetUserById(int id)
        {
            // Отримання користувача за userId та його повернення
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        // Метод для пошуку користувача за електронною поштою
        [NonAction]
        private async Task<User> FindUserByEmailAsync(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }


        // Операція для створення нового користувача
        [HttpPost]
        [ProducesResponseType(201)] // Створено
        [ProducesResponseType(400)] // Помилковий запит
        public IActionResult CreateUser([FromBody] UserView userView)
        {
            // Валідація моделі UserView
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Перевірка, чи існує вже користувач з таким же електронним листом
            var existingUser = _userRepository.GetUserByEmail(userView.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Цей електронний лист вже використовується.");
                return BadRequest(ModelState);
            }

            // Перетворення UserView в User для збереження в базі даних
            var user = new User
            {
                Fullname = userView.Fullname,
                Email = userView.Email,
                Password = userView.Password
            };

            // Логіка аутентифікації користувача після успішного створення
            _userRepository.CreateUser(user);

            // Повернення ресурсу, що був створений
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }


        // Операція для оновлення даних користувача
        [HttpPut("edit/{id}")]
        [ProducesResponseType(204)] // Немає вмісту
        [ProducesResponseType(400)] // Помилковий запит
        [ProducesResponseType(404)] // Не знайдено
        public IActionResult UpdateUser([FromRoute] int id, [FromBody] UserView viewUser)
        {
            try
            {
                // Отримання користувача за ідентифікатором
                var existingUser = _userRepository.GetUserById(id);

                if (existingUser == null)
                {
                    return NotFound();
                }

                // Оновлення даних користувача з ViewUser
                existingUser.Fullname = viewUser.Fullname;
                existingUser.Email = viewUser.Email;
                existingUser.Password = viewUser.Password;

                // Збереження оновленого користувача
                _userRepository.UpdateUser(existingUser);

                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                // Обробка помилок
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // Операція для видалення користувача
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(204)] // Немає вмісту
        [ProducesResponseType(404)] // Не знайдено
        public IActionResult DeleteUser(int id)
        {
            var existingUser = _userRepository.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            _userRepository.DeleteUser(id);
            return NoContent();
        }


        // Операція для входу користувача
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginView model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Пошук користувача за електронною поштою
            var user = await FindUserByEmailAsync(model.Email);

            if (user == null)
            {
                // Користувач з вказаною електронною поштою не знайдений
                return BadRequest("Невірна електронна пошта або пароль");
            }

            // Перевірка пароля
            var passwordVerificationResult = VerifyPassword(user, model.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                // Успішна аутентифікація
                return Ok(user);
            }

            // Невірні облікові дані
            return BadRequest("Невірна електронна пошта або пароль");
        }


        // Enum для представлення результатів перевірки пароля
        public enum PasswordVerificationResult
        {
            Success,
            Failed
        }


        // Метод для перевірки пароля
        [NonAction]
        public PasswordVerificationResult VerifyPassword(User user, string providedPassword)
        {
            try
            {
                if (user != null)
                {
                    bool passwordMatches = false;

                    if (user.Password.StartsWith("$2a$"))
                    {
                        passwordMatches = BCrypt.Net.BCrypt.Verify(providedPassword, user.Password);
                    }
                    else
                    {
                        passwordMatches = providedPassword == user.Password;
                    }

                    if (passwordMatches)
                    {
                        string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(providedPassword);
                        user.Password = newHashedPassword;
                    }

                    return passwordMatches ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
                }

                // Користувача з вказаною електронною поштою не знайдено
                return PasswordVerificationResult.Failed;
            }
            catch (Exception ex)
            {
                // Запис або обробка винятку належним чином
                return PasswordVerificationResult.Failed;
            }
        }


        // Операція для виходу користувача
        [HttpPost("logout")]
        [Authorize] // Доступ дозволено лише аутентифікованим користувачам
        public IActionResult Logout()
        {
            // Вихід користувача
            HttpContext.SignOutAsync();
            return Ok("Вихід успішний");
        }
    }
}