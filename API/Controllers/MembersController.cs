using System.Security.Claims;
using API.Data;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using CloudinaryDotNet.Actions;
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
        private readonly IPhotoService _photoService;
        public MembersController(IMemberRepository memberRepository, IPhotoService photoService)
        {
            _memberRepository = memberRepository;
            _photoService = photoService;
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

            if (member == null) return BadRequest("Member not found.");

            member.DisplayName = memberUpdateDTO.DisplayName ?? member.DisplayName;
            member.City = memberUpdateDTO.City ?? member.City;
            member.Country = memberUpdateDTO.Country ?? member.Country;
            member.Description = memberUpdateDTO.Description ?? member.Description;

            member.AppUser.DisplayName = memberUpdateDTO.DisplayName ?? member.AppUser.DisplayName;

            _memberRepository.Update(member);

            if (await _memberRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update member.");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto([FromForm] IFormFile file)
        {
            var member = await _memberRepository.GetMemberByIdAsync(User.GetMemberId());

            if (member == null) return BadRequest("Member not found.");

            var result = await _photoService.UploadPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                MemberId = member.Id
            };

            if (member.ImageUrl == null)
            {
                member.ImageUrl = photo.Url;
                member.AppUser.ImageUrl = photo.Url;
            }

            member.Photos.Add(photo);

            if (await _memberRepository.SaveAllAsync()) return photo;
            return BadRequest("Failed to add photo.");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var member = await _memberRepository.GetMemberForUpdate(User.GetMemberId());

            if (member == null) return BadRequest("Cannot get member from token");

            var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);

            if (member.ImageUrl == photo?.Url || photo == null)
            {
                return BadRequest("Cannot set this as main image");
            }

            member.ImageUrl = photo.Url;
            member.AppUser.ImageUrl = photo.Url;

            if (await _memberRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var member = await _memberRepository.GetMemberForUpdate(User.GetMemberId());

            if (member == null) return BadRequest("Cannot get member from token");

            var photo = member.Photos.SingleOrDefault(x => x.Id == photoId);

            if (photo == null || photo.Url == member.ImageUrl)
            {
                return BadRequest("This photo cannot be deleted");
            }

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            }

            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            member.Photos.Remove(photo);

            if (await _memberRepository.SaveAllAsync()) return Ok();
            return BadRequest("Problem deleting photo");
        }

    }
}
