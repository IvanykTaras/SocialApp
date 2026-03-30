using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly ILikesRepository _likesRepository;

        public LikesController(ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
        }

        [HttpPost("{targetMemberId}")]
        public async Task<ActionResult> ToggleLike(string targetMemberId)
        {
            var sourceMemberId = User.GetMemberId();

            if(sourceMemberId == targetMemberId) return BadRequest("You cannot like yourself.");

            var existingLike = await _likesRepository.GetMemberLike(sourceMemberId, targetMemberId);

            if(existingLike == null){
                var like = new MemberLike
                {
                    SourceMemberId = sourceMemberId,
                    TargetMemberId = targetMemberId
                };

                _likesRepository.AddLike(like);
            }
            else
            {
                _likesRepository.DeleteLike(existingLike);
            }

            if(await _likesRepository.SaveAllChanges()) return Ok();

            return BadRequest("Failed to toggle like.");

        }

        [HttpGet("list")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetCurrentMemberLikes()
        {
            var memberId = User.GetMemberId();
            var members = await _likesRepository.GetCurrentMemberLikeIds(memberId);
            return Ok(members);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMemberLikes(string predicate)
        {
            var members = await _likesRepository.GetMemberLikes(predicate, User.GetMemberId());
            return Ok(members);
        }

    }
}
