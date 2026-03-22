using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class RegisterDTO
{
    [Required]
    public required string DisplayName { get; set; } = "";
    [Required]
    [EmailAddress]
    public required string Email { get; set; } = "";
    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public required string Password { get; set; } = "";

    [Required]
    public string Gender { get; set; } = "";
    public string City { get; set; } = "";
    public string Country { get; set; } = "";
    public DateOnly DateOfBirth { get; set; }
}
