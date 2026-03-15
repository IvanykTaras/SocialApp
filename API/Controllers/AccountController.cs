using System.Security.Cryptography;
using API.Data;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(AppDbContext context, ITokenService tokenService) 
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == login.Email);


            if (user == null) return Unauthorized();

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(login.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized();
            }

            return Ok(user.ToDTO(_tokenService.CreateToken(user)));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
        {
            if (await EmailExists(register.Email)) return BadRequest("Email is already taken.");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Email = register.Email,
                DisplayName = register.DisplayName,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(register.Password)),
                PasswordSalt = hmac.Key
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(user.ToDTO(_tokenService.CreateToken(user)));
        }

        private async Task<bool> EmailExists(string email) => await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());   
    }
}
