using System;
using System.Threading;
using SignalRAspCore.Hub;

namespace SignalRAspCore.WebApp
{
    public class MessagePinger
    {
        private Timer _broadcastTimer;
        private Timer _groupTimer;
        private readonly IMessageSender _messageSender;

        public MessagePinger(IMessageSender messageSender)
        {
            _messageSender = messageSender;
            StartPinging();
        }

        private void StartPinging()
        {
            _broadcastTimer = new Timer(BroadcastMessage, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            _groupTimer = new Timer(SendToGroups, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void StopPinging()
        {
            _broadcastTimer.Dispose();
            _groupTimer.Dispose();
        }

        private void BroadcastMessage(object obj)
        {
            _messageSender.BroadcastServerMessage($"General broadcast message");
        }

        private void SendToGroups(object obj)
        {
            _messageSender.SendServerMessageToGroup("1", $"Update for group 1: {Guid.NewGuid().ToString().Substring(0, 5)}");
            _messageSender.SendServerMessageToGroup("2", $"Update for group 2: {Guid.NewGuid().ToString().Substring(0, 5)}");
            _messageSender.SendServerMessageToGroup("3", $"Update for group 3: {Guid.NewGuid().ToString().Substring(0, 5)}");
        }
    }
}
