using System;
using System.Threading.Tasks;
using MessageHandler;

namespace Timer
{
    public class SendScheduledMessagesToChannel: ISendScheduledMessagesToChannel
    {
        private readonly IChannel _channel;
        private readonly ILeaseLock _leaseLock;

        public SendScheduledMessagesToChannel(IChannel channel, ILeaseLock leaseLock)
        {
            _channel = channel;
            _leaseLock = leaseLock;
        }

        public async Task Send(object message)
        {
            await _leaseLock.ExecuteIfLeaseAcquired(() => _channel.Push(message));
        }
    }
}