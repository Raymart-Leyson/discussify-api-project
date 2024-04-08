using Microsoft.AspNetCore.SignalR;

namespace DiscussifyApi.Hubs 
{
    public class MessageHub : Hub <IMessageHub>
    {
        public async Task SendMessage(int roomId, object message)
        {
            // Handle the incoming message, process it, and broadcast it to other clients
            // You can access the message, room ID, and perform any necessary business logic here
            // await Clients.All.ReceiveMessage(roomId, message);
            await Clients.Group($"room_{roomId}").ReceiveMessage(roomId, message);
        }

        public override async Task OnConnectedAsync()
        {
            // Retrieve the room ID from the connection query string
            string roomId = Context.GetHttpContext()!.Request.Path.Value!.Split('/')[3];
            if (!string.IsNullOrEmpty(roomId))
            {
                // Add the client to the corresponding group
                await Groups.AddToGroupAsync(Context.ConnectionId, $"room_{roomId}");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string roomId = Context.GetHttpContext()!.Request.Path.Value!.Split('/')[3];
            if (!string.IsNullOrEmpty(roomId))
            {
                // Remove the client from the corresponding group
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"room_{roomId}");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}