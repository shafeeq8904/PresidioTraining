using ChatBotApi.Models;
using System.Threading.Tasks;

namespace ChatBotApi.Interfaces
{
    public interface IChatRepository
    {
        Task<string> SendMessageAsync(List<ChatMessage> messages);
    }
}
