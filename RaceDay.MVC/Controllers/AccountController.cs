
using RaceDay.MVC.Models;
using RaceDay.MVC.Services;
using System.Text.Json;

namespace RaceDay.MVC.Controllers;

public class AccountController : Controller
{
    private readonly ApiService _api;
    public AccountController(ApiService api) => _api = api;

    [HttpGet] public IActionResult Login() => View();
    [HttpGet] public IActionResult Register() => View();
    [HttpGet] public IActionResult AccessDenied() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        var (ok, body) = await _api.PostJsonAsync("/api/auth/login", vm);
        if (!ok) { ModelState.AddModelError("", "Invalid email or password."); return View(vm); }
        var res = JsonSerializer.Deserialize<LoginResponse>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        HttpContext.Session.SetString("JWT", res.Token);
        HttpContext.Session.SetString("UserRole", res.Role);
        HttpContext.Session.SetString("UserId", res.UserId.ToString());
        return res.Role == "Organiser" ? RedirectToAction("Dashboard", "Organiser") : RedirectToAction("Index", "Participant");
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm vm)
    {
        var (ok, _) = await _api.PostJsonAsync("/api/auth/register", vm);
        if (!ok) { ModelState.AddModelError("", "Registration failed."); return View(vm); }
        return RedirectToAction("Login");
    }

    public IActionResult Logout() { HttpContext.Session.Clear(); return RedirectToAction("Login"); }
}