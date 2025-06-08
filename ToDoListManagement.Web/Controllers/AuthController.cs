using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListManagement.Entity.ViewModel;
using ToDoListManagement.Service.Interfaces;

namespace ToDoListManagement.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Login()
    {
        string? email = Request.Cookies["Email"];
        if (!string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Index", "Dashboard");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            bool IsValid = await _authService.ValidateUserAsync(model);

            if (IsValid)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid Credentials!";
                return View(model);
            }
        }
        return View(model);
    }

    public async Task<IActionResult> CheckEmailExists(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Json(true);
        }

        bool exists = await _authService.CheckEmailExistsAsync(email.Trim().ToLower());
        return Json(!exists);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            bool isRegistered = await _authService.RegisterUserAsync(model);
            if (isRegistered)
            {
                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["ErrorMessage"] = "Registration failed! Please try again.";
            }
        }
        return View(model);
    }

    public IActionResult Logout()
    {
        try
        {
            _authService.LogoutUser();
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return RedirectToAction("Login", "Auth");
        }
    }
}
