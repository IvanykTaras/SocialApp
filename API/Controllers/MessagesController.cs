using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MessagesController : BaseApiController
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMemberRepository _memberRepository;
        public MessagesController(IMessageRepository messageRepository, IMemberRepository memberRepository)
        {
            _messageRepository = messageRepository;
            _memberRepository = memberRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO createMessageDTO)
        {
            var sender = await _memberRepository.GetMemberByIdAsync(User.GetMemberId());
            var recipient = await _memberRepository.GetMemberByIdAsync(createMessageDTO.RecipientId);

            if (recipient == null || sender == null || sender.Id == createMessageDTO.RecipientId)
                return BadRequest("Failed to send message");

            var message = new Message
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                Content = createMessageDTO.Content,
            };

            _messageRepository.AddMessage(message);

            if (await _messageRepository.SaveAllAsync())
            {
                return Ok(message.ToDTO());
            }

            return BadRequest("Failed to send message");
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<MessageDTO>>> GetMessageByContainer([FromQuery] MessageParams messageParams)
        {
            messageParams.MemberId = User.GetMemberId();
            var messages = await _messageRepository.GetMessagesForMember(messageParams);

            return Ok(messages);
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<ActionResult<IReadOnlyList<MessageDTO>>> GetMessageThread(string recipientId)
        {
            return Ok(await _messageRepository.GetMessageThread(User.GetMemberId(), recipientId));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(string id)
        {
            var memberId = User.GetMemberId();

            var message = await _messageRepository.GetMessage(id);

            if (message == null) return BadRequest("Cannot delete this message");

            if (message.SenderId != memberId && message.RecipientId != memberId)
                return BadRequest("You cannot delete this message");

            if (message.SenderId == memberId) message.SenderDeleted = true;
            if (message.RecipientId == memberId) message.RecipientDeleted = true;

            if(message is {SenderDeleted: true, RecipientDeleted: true })
            {
                _messageRepository.DeleteMessage(message);
            }

            if (await _messageRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the message");
        }
    }
}
