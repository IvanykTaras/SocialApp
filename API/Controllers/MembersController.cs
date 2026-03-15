using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class MembersController : BaseApiController // {{url}}/api/members
    {
        private readonly AppDbContext _context;
        public MembersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await _context.Users.ToListAsync();
            return Ok(members);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetMember(string id) // {{url}}/api/members/{id}
        {
            var member = await _context.Users.FindAsync(id);
            if (member == null) return NotFound();
            return Ok(member);
        }
    }
}
