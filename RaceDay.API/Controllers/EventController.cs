using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using Microsoft.AspNetCore.Authorization;
using RaceDay.API.Services;

namespace RaceDay.API.Controllers;
[ApiController, Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _db; private readonly BlobService _blob;
    public EventsController(AppDbContext db, BlobService blob) { _db = db; _blob = blob; }

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _db.Events.ToListAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetOne(int id) => Ok(await _db.Events.FindAsync(id));
    [HttpGet("organiser/{id}")] public async Task<IActionResult> GetByOrganiser(int id) => Ok(await _db.Events.Where(e => e.OrganiserId == id).ToListAsync());

    [Authorize, HttpPost]
    public async Task<IActionResult> Create([FromForm] Event ev, IFormFile? BannerImage)
    {
        if (BannerImage != null) ev.BannerImageUrl = await _blob.UploadAsync(BannerImage);
        _db.Events.Add(ev); await _db.SaveChangesAsync(); return Ok(ev);
    }
    [Authorize, HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) { var e = await _db.Events.FindAsync(id); if (e != null) { _db.Events.Remove(e); await _db.SaveChangesAsync(); } return Ok(); }
}