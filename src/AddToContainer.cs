
using MessageHandler;

namespace Timer
{
	// testing buildhook
	
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