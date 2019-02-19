using System;

namespace SignalRAspCore.Hub
{
    public delegate void StatusLogHandler(object sender, MessageSenderArgs e);

    public interface IMessageSender
    {
        event StatusLogHandler Event;
        void SendServerMessage(string connectionId, string message);
        void SendServerMessageToGroup(string groupId, string message);
        void BroadcastServerMessage(string message);
    }

    public class MessageSender : IMessageSender
    {
        public event StatusLogHandler Event;
        
        public void SendServerMessage(string connectionId, string message)
        {
            Event?.Invoke(this, new MessageSenderArgs(message, MessageType.Direct, connectionId, null));
        }

        public void SendServerMessageToGroup(string groupId, string message)
        {
            Event?.Invoke(this, new MessageSenderArgs(message, MessageType.Group, null, groupId));
        }
        
        public void BroadcastServerMessage(string message)
        {
            Event?.Invoke(this, new MessageSenderArgs(message, MessageType.Broadcast, null, null));
        }
    }

    public enum MessageType
    {
        Broadcast,
        Direct,
        Group
    }

    public class MessageSenderArgs : EventArgs
    {
        public string Message { get; }
        public MessageType Type { get; }
        public string ConnectionId { get; }
        public string GroupId { get; }

        public MessageSenderArgs(string message, MessageType type, string connectionId, string groupId)
        {
            Message = message;
            Type = type;
            ConnectionId = connectionId;
            GroupId = groupId;
        }
    }
}
