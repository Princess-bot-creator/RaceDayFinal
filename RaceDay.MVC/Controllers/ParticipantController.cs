
using RaceDay.MVC.Filters;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers;

[SessionAuthorize(Roles = "Participant")]
public class ParticipantController : Controller
{
    private readonly ApiService _api;
    public ParticipantController(ApiService api) => _api = api;
    private int UserId => int.Parse(HttpContext.Session.GetString("UserId")!);

    public async Task<IActionResult> Index(string? search, string? type)
    {
        var events = await _api.GetAsync<List<EventVm>>("/api/events") ?? new();
        events = events.Where(e => e.Date >= DateTime.Today).ToList();
        if (!string.IsNullOrEmpty(search)) events = events.Where(e => e.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!string.IsNullOrEmpty(type)) events = events.Where(e => e.EventType == type).ToList();
        return View(events);
    }

    public async Task<IActionResult> Details(int id) => View(await _api.GetAsync<EventVm>($"/api/events/{id}"));

    [HttpPost]
    public async Task<IActionResult> Enrol(int eventId, int categoryId)
    {
        await _api.PostJsonAsync("/api/enrolments", new { EventId = eventId, CategoryId = categoryId, ParticipantId = UserId });
        return RedirectToAction("MyEvents");
    }

    public async Task<IActionResult> MyEvents() => View(await _api.GetAsync<List<EnrolmentVm>>($"/api/enrolments/participant/{UserId}") ?? new());

    public async Task<IActionResult> MyResults() => View(await _api.GetAsync<List<ResultVm>>($"/api/results/participant/{UserId}") ?? new());

    [HttpGet] public async Task<IActionResult> Profile() => View(await _api.GetAsync<ProfileVm>($"/api/users/{UserId}"));
    [HttpPost]
    public async Task<IActionResult> Profile(ProfileVm vm, IFormFile? ProfilePicture)
    {
        var form = new MultipartFormDataContent();
        form.Add(new StringContent(vm.FullName), "FullName");
        form.Add(new StringContent(vm.Email), "Email");
        if (ProfilePicture != null) form.Add(new StreamContent(ProfilePicture.OpenReadStream()), "ProfilePicture", ProfilePicture.FileName);
        await _api.PutFormAsync($"/api/users/{UserId}", form);
        return RedirectToAction("Profile");
    }
}