using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace TaskManagementAPI.Hubs
{
    public class TaskHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task SendTaskUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveTaskUpdate", message);
        }

        public async Task NotifyTaskCreated(object taskDto)
        {
            await Clients.All.SendAsync("TaskCreated", taskDto);
        }

        public async Task NotifyTaskUpdated(object taskDto)
        {
            await Clients.All.SendAsync("TaskUpdated", taskDto);
        }
    }
}
