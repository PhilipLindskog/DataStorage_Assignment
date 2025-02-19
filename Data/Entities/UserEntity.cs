﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class UserEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "NVARCHAR(50)")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Column(TypeName = "NVARCHAR(50)")]
    public string LastName { get; set; } = null!;

    [Required]
    [Column(TypeName = "VARCHAR(150)")]
    public string Email { get; set; } = null!;

    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
