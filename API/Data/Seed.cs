using System;
using System.Security.Cryptography;
using System.Text.Json;
using API.DTO;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(AppDbContext context)
    {
        if(await context.Users.AnyAsync()) return;

        var membersData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var members = JsonSerializer.Deserialize<List<SeedUserDTO>>(membersData);

        if(members == null) { 
            Console.WriteLine("Failed to deserialize members data"); 
            return; 
        }


        foreach(var member in members)
        {
            using var hmac = new HMACSHA512();
            
            var user = new AppUser
            {
                Id = member.Id,
                Email = member.Email,
                DisplayName = member.DisplayName,
                ImageUrl = member.ImageUrl,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Pa$$w0rd")),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    Id = member.Id,
                    DisplayName = member.DisplayName,
                    Description = member.Description,
                    ImageUrl = member.ImageUrl,
                    DateOfBirth = member.DateOfBirth,
                    City = member.City,
                    Country = member.Country,
                    Gender = member.Gender,
                    Created = member.Created,
                    LastActive = member.LastActive
                }
            };

            user.Member.Photos.Add(new Photo
            {
                Url = member.ImageUrl!,
                MemberId = member.Id
            });

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    } 
}
