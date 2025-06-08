using Microsoft.EntityFrameworkCore;
using ToDoListManagement.Entity.Models;

namespace ToDoListManagement.Entity.Data;

public partial class ToDoListDbContext : DbContext
{
    public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options)
        : base(options)
    {

    }

    public DbSet<Project> Projects { get; set; }

    public DbSet<ProjectUser> ProjectUsers { get; set; }

    public DbSet<TaskAttachment> TaskAttachments { get; set; }

    public DbSet<ToDoList> ToDoLists { get; set; }

    public DbSet<User> Users { get; set; }

}
