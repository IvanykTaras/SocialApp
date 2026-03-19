using System;

namespace API.DTO;

public class SeedUserDTO
{
    public string Id {get; set;} = null!;
    public DateOnly DateOfBirth {get; set;}
    public string? ImageUrl {get; set;}
    public required string DisplayName { get; set; }
    public required string Email { get; set; }

    public DateTime Created { get; set; } 
    public DateTime LastActive { get; set; } 
    public required string Gender { get; set; }
    public string? Description { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
}
