using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DbIns;
using DbIns.Models;
using Ins.ReqModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly DbInsContext _insContext;

    public AuthController(DbInsContext insContext, UserManager<User> userManager, SignInManager<User> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _insContext = insContext;
    }

    [Authorize]
    [HttpGet("getClientByToken")] // Новый маршрут для получения клиента по токену
    public async Task<IActionResult> GetClientByToken()
    {
        // Получаем текущего пользователя из контекста
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Извлекаем Id пользователя
        if (userId == null)
        {
            return Unauthorized();
        }

        // Находим клиента по userId
        var client = await _insContext.Clients
            .Include(c => c.Address) // Если вы хотите включить информацию о адресе
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (client == null)
        {
            return NotFound();
        }

        var clientResponse = new ReqClient
        {
            Name = client.FIO,
            IdClient = client.Id
            // Другие свойства, которые вы хотите вернуть
            // Например:
            // Birthday = client.Birthday,
            // Passport = client.Passport,
            // Phone = client.Phone
        };

        return Ok(clientResponse);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // Проверка входных данных
        if (request == null)
        {
            return BadRequest(new { message = "Данные для регистрации не получены." });
        }

        // Добавление нового адреса
        var address = await AddNewAddress(request.Address);
        if (address == null) // Проверка на случай, если адрес не был добавлен
        {
            return BadRequest(new { message = "Ошибка при добавлении адреса." });
        }

        // Создание нового пользователя
        var user = new User { UserName = request.Register.Email, Email = request.Register.Email };
        var result = await _userManager.CreateAsync(user, request.Register.Password);

        if (!result.Succeeded)
        {
            // Если регистрация пользователя не удалась, возвращаем ошибки
            return BadRequest(new { message = "Ошибка при создании пользователя", errors = result.Errors });
        }

        // Создаем нового клиента
        var client = new Client
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            FIO = request.Client.FIO,
            Birthday = request.Client.Birthday,
            Passport = request.Client.Passport,
            Phone = request.Client.Phone,
            IdAddress = address.Id,
            Address = address
        };

        // Добавляем клиента в контекст и сохраняем изменения
        await _insContext.Clients.AddAsync(client);
        var saveResult = await _insContext.SaveChangesAsync();

        if (saveResult < 1) // Проверяем, что запись была сохранена
        {
            return StatusCode(500, new { message = "Ошибка при сохранении данных клиента." });
        }

        // Возвращаем успех
        return Ok(new { message = "Регистрация прошла успешно." });
    }


    private async Task<Address> AddNewAddress(ReqAddress address)
    {
        var findCity = await _insContext.Cities
            .FirstOrDefaultAsync(n => n.Name == address.NameCity && n.Region == address.Region);

        if (findCity == null)
        {
            var newCity = new City
            {
                Id = Guid.NewGuid(),
                Name = address.NameCity,
                Region = address.Region
            };
            await _insContext.Cities.AddAsync(newCity);
            await _insContext.SaveChangesAsync();
            findCity = newCity; // Присвоить созданный город в findCity
        }

        var newAddress = new Address()
        {
            Id = Guid.NewGuid(),
            NameStreet = address.NameStreet,
            NumberHome = address.NumberHome,
            Apartment = address.Apartment,
            Entrance = address.Entrance,
            IdCity = findCity.Id,
            City = findCity
        };

        await _insContext.Addresses.AddAsync(newAddress);
        await _insContext.SaveChangesAsync();
        return newAddress;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        var token = GenerateJwtToken(user);

        return Ok(new { token });
    }
    

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(270),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}