using ToDoListManagement.Entity.Models;

namespace ToDoListManagement.Repository.Interfaces;

public interface IAuthRepository
{
    Task<bool> ValidateUser(string email, string password);
    Task<string?> GetRoleNameByEmailAsync(string email);
    Task<bool> CheckEmailExistsAsync(string email);
    Task<bool> RegisterUserAsync(User user);
}
