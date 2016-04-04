using MessageHandler;
using Quartz;
using Quartz.Impl;

namespace Timer
{
    public class ScheduleMessages : IScheduleMessages
    {
        private IScheduler scheduler;
        private ITrigger trigger;

        private readonly ITemplatingEngine _templatingEngine;

        private readonly dynamic _account;
        private readonly dynamic _channel;
        private readonly dynamic _environment;

        public ScheduleMessages(IContainer container, IConfigurationSource configurationSource, ITemplatingEngine templatingEngine, IVariablesSource variables)
        {
            _templatingEngine = templatingEngine;

            _account = variables.GetAccountVariables(Account.Current());
            _channel = variables.GetChannelVariables(Channel.Current());
            _environment = variables.GetEnvironmentVariables(Environment.Current());

            var config = configurationSource.GetConfiguration<TimerConfig>();

            var factory = new StdSchedulerFactory();

            // get a scheduler
            scheduler = factory.GetScheduler();
            scheduler.JobFactory = new ContainerJobFactory(container);

            // Trigger the job to run now, and then every 30 seconds
            trigger = TriggerBuilder.Create()
                .WithIdentity("Repeat")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(int.Parse(ApplyTemplate(config.Interval))) //Config?
                    .RepeatForever())
                .Build();
        }

        public void Schedule(string body)
        {

            var job = JobBuilder.Create<SendMessage>()
                .WithIdentity("sendMessage")
                .UsingJobData("body", body)
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public void Start()
        {
            scheduler.Start();
        }

        public void Stop()
        {
            scheduler.Standby();
        }

        private string ApplyTemplate(string template)
        {
            return _templatingEngine.Apply(template, null, _channel, _environment, _account);
        }
    }
}