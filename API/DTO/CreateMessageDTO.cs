using System;

namespace API.DTO;

public class CreateMessageDTO
{
    public required string RecipientId { get; set; }
    public required string Content { get; set; }
}
