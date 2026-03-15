using System;

namespace API.DTO;

public class LoginDTO
{
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
}
