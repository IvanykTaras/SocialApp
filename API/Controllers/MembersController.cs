using System.Security.Claims;
using API.Data;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    [Authorize]
    public class MembersController : BaseApiController // {{url}}/api/members
    {
        private readonly IMemberRepository _memberRepository;
        public MembersController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AppUser>>> GetMembers()
        {
            var members = await _memberRepository.GetMembersAsync();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetMember(string id) // {{url}}/api/members/{id}
        {
            var member = await _memberRepository.GetMemberByIdAsync(id);
            if (member == null) return NotFound();
            return Ok(member);
        }

        [HttpGet("{id}/photos")]
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetPhotosByUserId(string id)
        {
            var photos = await _memberRepository.GetPhotosByUserIdAsync(id);
            return Ok(photos);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdateDTO memberUpdateDTO)
        {
            var memberId = User.GetMemberId();

            var member = await _memberRepository.GetMemberForUpdate(memberId);

            if(member == null) return BadRequest("Member not found.");

            member.DisplayName = memberUpdateDTO.DisplayName ?? member.DisplayName;
            member.City = memberUpdateDTO.City ?? member.City;
            member.Country = memberUpdateDTO.Country ?? member.Country;
            member.Description = memberUpdateDTO.Description ?? member.Description; 

            member.AppUser.DisplayName = memberUpdateDTO.DisplayName ?? member.AppUser.DisplayName;

            _memberRepository.Update(member);

            if ( await _memberRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update member.");
        }
    }
}
