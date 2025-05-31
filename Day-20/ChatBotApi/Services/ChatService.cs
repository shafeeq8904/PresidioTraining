using ChatBotApi.Interfaces;
using ChatBotApi.Models;

namespace ChatBotApi.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<ChatResponseDto> GetChatResponseAsync(ChatRequestDto request)
        {
            var messages = new List<ChatMessage>
            {
                new ChatMessage { Role = "user", Content = request.Message }
            };

            var response = await _chatRepository.SendMessageAsync(messages);

            return new ChatResponseDto { Response = response };
        }
    }
}
