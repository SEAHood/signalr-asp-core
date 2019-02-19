using Microsoft.AspNetCore.SignalR;

namespace SignalRAspCore.Hub
{
    public class MessageSenderHook
    {
        public MessageSenderHook(IHubContext<MessageHub> hubContext, IMessageSender eventSender)
        {
            eventSender.Event += async (sender, args) => await SendHub.SendServerMessage(hubContext.Clients, args);
        }
    }
}
