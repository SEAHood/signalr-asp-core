using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRAspCore.Hub
{
    public static class SendHub
    {
        public static async Task SendServerMessage(IHubClients clients, MessageSenderArgs args)
        {
            await SendMessage("ServerMessage", clients, args);
        }

        private static async Task SendMessage(string methodName, IHubClients clients, MessageSenderArgs args)
        {
            switch (args.Type)
            {
                case MessageType.Broadcast:
                    await clients.All.SendAsync(methodName, args.Message);
                    break;
                case MessageType.Direct:
                    if (args.ConnectionId == null)
                    {
                        Console.WriteLine("ConnectionId doesn't exist for the given message, unable to deliver.");
                        return;
                    }
                    await clients.Client(args.ConnectionId).SendAsync(methodName, args.Message);
                    break;
                case MessageType.Group:
                    if (args.GroupId == null)
                    {
                        Console.WriteLine("GroupId doesn't exist for the given message, unable to deliver.");
                        return;
                    }
                    await clients.Groups(args.GroupId).SendAsync(methodName, args.Message);
                    break;
            }
        }
    }

    public class MessageHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IMessageSender _sender;

        public MessageHub(IMessageSender sender)
        {
            _sender = sender;
        }

        public override async Task OnConnectedAsync()
        {
            _sender.SendServerMessage(Context.ConnectionId, "Connected to SignalR hub");
            await base.OnConnectedAsync();
        }

        public void ClientMessage(string message)
        {
            _sender.SendServerMessage(Context.ConnectionId, $"Server received message: {message}");
        }

        public async Task SubscribeToGroup(string groupId)
        {
            if (groupId == null) return;
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            _sender.SendServerMessage(Context.ConnectionId, $"Subscribed to group {groupId}");
        }

        public async Task UnsubscribeFromGroup(string groupId)
        {
            if (groupId == null) return;
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
            _sender.SendServerMessage(Context.ConnectionId, $"Unsubscribed from group {groupId}");
        }
    }
}
