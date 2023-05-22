namespace ChatApplication.Controllers.v1;

using Models.Messages.Request;
using Infrastructure.Services.Messages;
using Microsoft.AspNetCore.Mvc;
using ChatApplication.Application.Services.Messages;

[ApiVersion("1.0")]
public class MessageController : ApiController
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService) => _messageService = messageService;

    [HttpPost]
    [Route("send")]
    public async Task<IActionResult> Send([FromBody] RequestMessage request)
    {
        var username = HttpContext?.User?.Identity?.Name;

        await _messageService.SendMessage(username, request.Text);

        return Ok();
    }
}
