
using MessageHandler;

namespace Timer
{
    public class AddToContainer : IInitialization
    {
        public void Init(IContainer container)
        {
            container.Register<ScheduleMessages>(Lifecycle.Singleton);
            container.Register<SendMessage>(Lifecycle.InstancePerCall);
            container.Register<SendScheduledMessagesToChannel>(Lifecycle.InstancePerCall);
            container.Register<LeaseLock>(Lifecycle.Singleton);
        }
    }
}