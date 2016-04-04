using System.Threading.Tasks;

namespace Timer
{
    public interface ISendScheduledMessagesToChannel
    {
        Task Send(object message);
    }
}