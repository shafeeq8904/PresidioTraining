using ChatBotApi.Models;
using System.Threading.Tasks;

namespace ChatBotApi.Interfaces
{
    public interface IChatService
    {
        Task<ChatResponseDto> GetChatResponseAsync(ChatRequestDto request);
    }
}
