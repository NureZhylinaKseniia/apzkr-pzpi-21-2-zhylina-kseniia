using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Application.Models;
using Application.Repositories;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Application.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

// Контролер для адміністраторів
[ApiController]
[Route("api/manager")]
public class ManagerController : ControllerBase
{
    private readonly ILogger<ManagerController> _logger;
    private readonly IManagerRepository _managerRepository;
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;

    // Конструктор контролера
    public ManagerController(ILogger<ManagerController> logger, IManagerRepository managerRepository, IUserRepository userRepository, AppDbContext context)
    {
        _logger = logger;
        _managerRepository = managerRepository;
        _userRepository = userRepository;
        _context = context;
    }


    [HttpGet]
    public IActionResult GetAllManagers()
    {
        try
        {
            // Отримання всіх усиновлень та повернення їх у відповіді
            var Manager = _managerRepository.GetAllManagers().ToList();
            return Ok(Manager);
        }
        catch (Exception ex)
        {
            // Повернення помилки у відповіді при виникненні виключення
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    // Дія для отримання адміністратора за ідентифікатором
    [HttpGet("{id}")]
    public IActionResult GetManagerById(int id)
    {
        try
        {
            var manager = _managerRepository.GetManagerById(id);

            if (manager == null)
            {
                return NotFound("Адміністратора не знайдено");
            }

            return Ok(manager);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при отриманні адміністратора за ідентифікатором");
            return StatusCode(500, "Помилка сервера");
        }
    }


    // Внутрішній метод для пошуку адміністратора за електронною поштою
    [NonAction]
    private async Task<Manager> FindManagerByEmailAsync(string email)
    {
        var manager = await _context.Manager.FirstOrDefaultAsync(u => u.Email == email);
        return manager;
    }


    // Дія для створення нового адміністратора
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public IActionResult CreateManager([FromBody] ManagerView managerView)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Перевірка, чи не існує адміністратора з такою ж електронною поштою
        var existingUser = _managerRepository.GetManagerByEmail(managerView.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("Email", "Цей електронний лист вже використовується.");
            return BadRequest(ModelState);
        }

        // Створення нового адміністратора
        var manager = new Manager
        {
            Fullname = managerView.Fullname,
            Email = managerView.Email,
            Password = managerView.Password,
        };

        _managerRepository.CreateManager(manager);

        return CreatedAtAction(nameof(GetManagerById), new { id = manager.ManagerId }, manager);
    }
    

    // Дія для оновлення інформації про адміністратора
    [HttpPut("edit/{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateManager([FromRoute] int id, [FromBody] ManagerView viewManager)
    {
        try
        {
            var existingManager = _managerRepository.GetManagerById(id);

            if (existingManager == null)
            {
                return NotFound();
            }

            existingManager.Fullname = viewManager.Fullname;
            existingManager.Email = viewManager.Email;
            existingManager.Password = viewManager.Password;

            _managerRepository.UpdateManager(existingManager);

            return Ok(existingManager);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }


    // Дія для видалення адміністратора
    [HttpDelete("delete/{id}")]
    public IActionResult DeleteManager(int id)
    {
        try
        {
            _managerRepository.DeleteManager(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при видаленні адміністратора");
            return StatusCode(500, "Помилка сервера");
        }
    }


    // Дія для видалення профілю користувача
    [HttpDelete("deleteUser/{id}")]
    public IActionResult DeleteUserProfile(int id)
    {
        try
        {
            _userRepository.DeleteUser(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Помилка при видаленні профілю користувача");
            return StatusCode(500, "Помилка сервера");
        }
    }


    // Дія для входу адміністратора в систему
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginView model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var manager = await FindManagerByEmailAsync(model.Email);

        if (manager == null)
        {
            return BadRequest("Невірна електронна пошта або пароль");
        }

        var passwordVerificationResult = VerifyPassword(manager, model.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Success)
        {
            return Ok("Вхід успішний");
        }

        return BadRequest("Невірна електронна пошта або пароль");
    }


    // Перелік можливих результатів перевірки паролю
    public enum PasswordVerificationResult
    {
        Success,
        Failed
    }


    // Внутрішній метод для перевірки паролю адміністратора
    [NonAction]
    public PasswordVerificationResult VerifyPassword(Manager manager, string providedPassword)
    {
        try
        {
            if (manager != null)
            {
                bool passwordMatches = false;

                if (manager.Password.StartsWith("$2a$"))
                {
                    passwordMatches = BCrypt.Net.BCrypt.Verify(providedPassword, manager.Password);
                }
                else
                {
                    passwordMatches = providedPassword == manager.Password;
                }

                if (passwordMatches)
                {
                    string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(providedPassword);
                    manager.Password = newHashedPassword;
                }

                return passwordMatches ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
            }

            return PasswordVerificationResult.Failed;
        }
        catch (Exception ex)
        {
            return PasswordVerificationResult.Failed;
        }
    }


    // Дія для виходу адміністратора з системи
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return Ok("Вихід успішний");
    }
}
