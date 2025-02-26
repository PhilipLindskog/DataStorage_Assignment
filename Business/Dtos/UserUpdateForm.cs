﻿using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class UserUpdateForm
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is reqired.")]
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters long.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is reqired.")]
    [MinLength(2, ErrorMessage = "Last name must be at least 2 characters long.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "First name is reqired.")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email must be in a valid format like user@mail.com")]
    public string Email { get; set; } = null!;
}
