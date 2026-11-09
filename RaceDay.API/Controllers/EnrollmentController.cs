using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
namespace RaceDay.API.Controllers;
[ApiController, Route("api/enrolments")]
public class EnrolmentsController : ControllerBase
{
    private readonly AppDbContext _db; public EnrolmentsController(AppDbContext db) => _db = db;
    [HttpPost] public async Task<IActionResult> Create(Enrolment e) { _db.Enrolments.Add(e); await _db.SaveChangesAsync(); return Ok(e); }
    [HttpGet("participant/{id}")] public async Task<IActionResult> ByParticipant(int id) => Ok(await _db.Enrolments.Include(e => e.Event).Include(e => e.Participant).Where(e => e.ParticipantId == id).ToListAsync());
}