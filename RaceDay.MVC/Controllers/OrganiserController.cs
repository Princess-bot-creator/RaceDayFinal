using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Filters;
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers;

[SessionAuthorize(Roles = "Organiser")]
public class OrganiserController : Controller
{
    private readonly ApiService _api;
    public OrganiserController(ApiService api) => _api = api;
    private int UserId => int.Parse(HttpContext.Session.GetString("UserId")!);

    public async Task<IActionResult> Dashboard()
    {
        var events = await _api.GetAsync<List<EventVm>>($"/api/events/organiser/{UserId}") ?? new();
        ViewBag.TotalEvents = events.Count;
        ViewBag.TotalEnrolments = events.Sum(e => e.EnrolmentCount);
        ViewBag.NextEvent = events.Where(e => e.Date >= DateTime.Today).OrderBy(e => e.Date).FirstOrDefault();
        return View(events);
    }

    public async Task<IActionResult> Events() => View(await _api.GetAsync<List<EventVm>>($"/api/events/organiser/{UserId}") ?? new());

    [HttpGet] public IActionResult CreateEvent() => View(new EventVm { Date = DateTime.Today });
    [HttpPost]
    public async Task<IActionResult> CreateEvent(EventVm vm, IFormFile? BannerImage)
    {
        var form = new MultipartFormDataContent();
        form.Add(new StringContent(vm.Name), "Name");
        form.Add(new StringContent(vm.Description), "Description");
        form.Add(new StringContent(vm.Date.ToString("yyyy-MM-dd")), "Date");
        form.Add(new StringContent(vm.Location), "Location");
        form.Add(new StringContent(vm.Distance.ToString()), "Distance");
        form.Add(new StringContent(vm.EventType), "EventType");
        form.Add(new StringContent(UserId.ToString()), "OrganiserId");
        if (BannerImage != null) form.Add(new StreamContent(BannerImage.OpenReadStream()), "BannerImage", BannerImage.FileName);
        if (await _api.PostFormAsync("/api/events", form)) return RedirectToAction("Events");
        return View(vm);
    }

    public async Task<IActionResult> Enrolments(int eventId) => View(await _api.GetAsync<List<EnrolmentVm>>($"/api/events/{eventId}/enrolments") ?? new());

    [HttpPost]
    public async Task<IActionResult> SaveResult(int enrolmentId, string finishTime, int position, int totalFinishers)
    {
        await _api.PostJsonAsync("/api/results", new { EnrolmentId = enrolmentId, FinishTime = finishTime, FinishingPosition = position, TotalFinishers = totalFinishers });
        return RedirectToAction("Dashboard");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        await _api.DeleteAsync($"/api/events/{id}");
        return RedirectToAction("Events");
    }
}