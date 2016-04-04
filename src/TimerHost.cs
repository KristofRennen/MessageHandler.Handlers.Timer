using MessageHandler;

namespace Timer
{
    public class TimerHost : IStream
    {
        private readonly IConfigurationSource _configurationSource;
        private readonly IScheduleMessages _scheduler;
        private readonly ITemplatingEngine _templatingEngine;

        private readonly dynamic _account;
        private readonly dynamic _channel;
        private readonly dynamic _environment;

        public TimerHost(IConfigurationSource configurationSource, IScheduleMessages scheduler, ITemplatingEngine templatingEngine, IVariablesSource variables)
        {
            _configurationSource = configurationSource;
            _scheduler = scheduler;
            _templatingEngine = templatingEngine;

            _account = variables.GetAccountVariables(Account.Current());
            _channel = variables.GetChannelVariables(Channel.Current());
            _environment = variables.GetEnvironmentVariables(Environment.Current());
        }

        public void Start()
        {
            var config = _configurationSource.GetConfiguration<TimerConfig>();

            _scheduler.Schedule(ApplyTemplate(config.Body));

            _scheduler.Start();
        }

        public void Stop()
        {
            _scheduler.Stop();
        }

        private string ApplyTemplate(string template)
        {
            return _templatingEngine.Apply(template, null, _channel, _environment, _account);
        }
    }
}