using System;
using API.DTO;
using API.Entities;

namespace API.Extensions;

public static class AppUserExtensions
{
    public static UserDTO ToDTO(this AppUser user, string token)
    {
        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
            DisplayName = user.DisplayName,
            Token = token
        };
    }
}
