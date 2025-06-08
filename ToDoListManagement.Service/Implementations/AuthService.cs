using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToDoListManagement.Entity.Models;
using ToDoListManagement.Entity.ViewModel;
using ToDoListManagement.Repository.Interfaces;
using ToDoListManagement.Service.Interfaces;

namespace ToDoListManagement.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthService(IAuthRepository authRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _authRepository = authRepository;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> ValidateUserAsync(LoginViewModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            return false;
        }

        bool IsValid = await _authRepository.ValidateUser(model.Email, model.Password);

        if (IsValid)
        {
            if (model.RememberMe)
            {
                CookieOptions? Email = new()
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(30)
                };
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("Email", model.Email.ToLower(), Email);
            }

            string? token = await GenerateJwtToken(model.Email);

            CookieOptions? Token = new()
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(30)
            };
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("Token", token, Token);

            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> CheckEmailExistsAsync(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        return await _authRepository.CheckEmailExistsAsync(email.Trim().ToLower());
    }

    public async Task<string> GenerateJwtToken(string email)
    {
        string? Rolename = await _authRepository.GetRoleNameByEmailAsync(email);

        Claim[]? claims =
        [
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, Rolename)
        ];

        SymmetricSecurityKey? key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        SigningCredentials? creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken? token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(360),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> RegisterUserAsync(RegisterViewModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            return false;
        }
        User user = new()
        {
            Name = model.Name?.Trim(),
            Email = model.Email.Trim().ToLower(),
            PasswordHash = model.Password.Trim(),
            Role = "Member"
        };

        bool isRegistered = await _authRepository.RegisterUserAsync(user);

        return isRegistered;
    }

    public void LogoutUser()
    {
        HttpContext? context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            foreach (string? cookie in context.Request.Cookies.Keys)
            {
                context.Response.Cookies.Delete(cookie);
            }
        }
    }   
}
