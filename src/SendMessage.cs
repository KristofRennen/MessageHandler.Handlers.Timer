using Quartz;

namespace Timer
{
    public class SendMessage : IJob
    {
        private readonly ISendScheduledMessagesToChannel _sender;
        private readonly ILeaseLock _leaseLock;

        public SendMessage(ISendScheduledMessagesToChannel sender, ILeaseLock leaseLock)
        {
            _sender = sender;
            _leaseLock = leaseLock;
        }

        public void Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var body = dataMap.GetString("body");

            _leaseLock.ExecuteIfLeaseAcquired(async () =>
            {
                await _sender.Send(body);
            }).Wait();
        }

    }
}