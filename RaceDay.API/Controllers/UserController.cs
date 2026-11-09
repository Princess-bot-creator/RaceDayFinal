using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Services;
namespace RaceDay.API.Controllers;
[ApiController, Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db; private readonly BlobService _blob;
    public UsersController(AppDbContext db, BlobService blob) { _db = db; _blob = blob; }
    [HttpGet("{id}")] public async Task<IActionResult> Get(int id) => Ok(await _db.Users.FindAsync(id));
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] string FullName, [FromForm] string Email, IFormFile? ProfilePicture)
    {
        var u = await _db.Users.FindAsync(id); if (u == null) return NotFound();
        u.FullName = FullName; u.Email = Email;
        if (ProfilePicture != null) u.ProfilePictureUrl = await _blob.UploadAsync(ProfilePicture);
        await _db.SaveChangesAsync(); return Ok(u);
    }
}