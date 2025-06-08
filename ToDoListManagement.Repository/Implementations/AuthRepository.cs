using Microsoft.EntityFrameworkCore;
using ToDoListManagement.Entity.Data;
using ToDoListManagement.Entity.Models;
using ToDoListManagement.Repository.Interfaces;

namespace ToDoListManagement.Repository.Implementations;

public class AuthRepository : IAuthRepository
{
    private readonly ToDoListDbContext _context;
    public AuthRepository(ToDoListDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ValidateUser(string email, string password)
    {
        string? hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        try
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email.Trim().ToLower());

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while validating user: {ex.Message}");
            return false;
        }

        return false;
    }

    public async Task<string?> GetRoleNameByEmailAsync(string email)
    {
        try
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user?.Role;
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving user role", ex);
        }
    }

    public async Task<bool> CheckEmailExistsAsync(string email)
    {
        try
        {
            return await _context.Users.AnyAsync(u => u.Email == email.Trim().ToLower());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while checking email existence: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RegisterUserAsync(User user)
    {
        try
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash))
            {
                return false;
            }
            user.Email = user.Email.Trim().ToLower();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.Role = user.Role;
            user.IsDeleted = false;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while registering user: {ex.Message}");
            return false;
        }
    }
}
