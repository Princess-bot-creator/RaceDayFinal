using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
namespace RaceDay.API.Controllers;
[ApiController, Route("api/results")]
public class ResultsController : ControllerBase
{
    private readonly AppDbContext _db; public ResultsController(AppDbContext db) => _db = db;
    [HttpPost] public async Task<IActionResult> Create(Result r) { _db.Results.Add(r); await _db.SaveChangesAsync(); return Ok(r); }
    [HttpGet("participant/{id}")]
    public async Task<IActionResult> ByParticipant(int id)
    {
        var list = await (from r in _db.Results join en in _db.Enrolments on r.EnrolmentId equals en.Id join ev in _db.Events on en.EventId equals ev.Id where en.ParticipantId == id select new { ev.Name, ev.Date, FinishTime = r.FinishTime, FinishingPosition = r.FinishingPosition, TotalFinishers = r.TotalFinishers, Category = "General" }).ToListAsync();
        return Ok(list);
    }
}