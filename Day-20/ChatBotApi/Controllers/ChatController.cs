using ChatBotApi.Interfaces;
using ChatBotApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatBotApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<ActionResult<ChatResponseDto>> Post(ChatRequestDto request)
        {
            var result = await _chatService.GetChatResponseAsync(request);
            return Ok(result);
        }
    }
}
