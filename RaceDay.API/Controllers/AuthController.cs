using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers;
[ApiController, Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    public AuthController(AppDbContext db) => _db = db;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email)) return BadRequest("Email exists");
        var user = new User { FullName = dto.FullName, Email = dto.Email, Password = dto.Password, Role = dto.Role };
        _db.Users.Add(user); await _db.SaveChangesAsync();
        return Ok(new { user.Id, user.FullName, user.Email, user.Role });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);
        if (user == null) return Unauthorized();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ChangeThisToAStrongSecretAtLeast32Chars"));
        var token = new JwtSecurityTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Role, user.Role) }),
            Expires = DateTime.UtcNow.AddHours(5),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        });
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), role = user.Role, userId = user.Id, fullName = user.FullName });
    }
}