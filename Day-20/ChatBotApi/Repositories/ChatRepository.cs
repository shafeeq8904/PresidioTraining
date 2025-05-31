using ChatBotApi.Interfaces;
using ChatBotApi.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ChatBotApi.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly string _modelId;

        public ChatRepository(HttpClient client, IConfiguration config)
        {
            _client = client;
            _apiKey = config["HuggingFace:ApiKey"];
            _apiUrl = "https://router.huggingface.co/nebius/v1/chat/completions";
            _modelId = "deepseek-ai/DeepSeek-R1-0528";
        }

        public async Task<string> SendMessageAsync(List<ChatMessage> messages)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var payload = new Dictionary<string, object>
        {
            { "model", _modelId },
            { "messages", messages.Select(m => new { role = m.Role, content = m.Content }).ToList() }
        };


            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_apiUrl, content);

            if (!response.IsSuccessStatusCode)
                return $"Error: {response.StatusCode}";

            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);
            var chatContent = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            return chatContent?.Trim() ?? "No response";
        }
    }
}
