using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListManagement.Entity.Models;

public partial class ToDoList
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TaskId { get; set; }

    public int? ProjectId { get; set; }

    public int? CreatedBy { get; set; }

    public int? AssignedTo { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Priority { get; set; }

    [ForeignKey("ProjectId")]
    public virtual Project? Project { get; set; }
    [ForeignKey("CreatedBy")]
    public virtual User? CreatedUser { get; set; }
    [ForeignKey("AssignedTo")]
    public virtual User? AssignedUser { get; set; }

}
