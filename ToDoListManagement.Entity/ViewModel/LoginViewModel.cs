using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ToDoListManagement.Entity.ViewModel;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    // [Remote("CheckEmailExists", "Login", ErrorMessage = "Email already exists, Please try some other email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }    
}
