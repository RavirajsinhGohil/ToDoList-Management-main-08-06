using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListManagement.Entity.Models;

public partial class TaskAttachment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AttachmentId { get; set; }

    public int? TaskId { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public bool? IsImage { get; set; }

    public DateTime? UploadedOn { get; set; }

    [ForeignKey("TaskId")]
    public virtual ToDoList? ToDoList { get; set; }

}
