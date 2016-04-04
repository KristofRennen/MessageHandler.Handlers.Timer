using System;
using System.Threading.Tasks;
using MessageHandler;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Environment = MessageHandler.Environment;

namespace Timer
{
    public class LeaseLock : ILeaseLock
    {
        private readonly string _streamId;
        private readonly CloudBlockBlob _leaseBlob;
        private readonly AutoRenewLease _autoRenew;

        private readonly ITemplatingEngine _templatingEngine;

        private readonly dynamic _account;
        private readonly dynamic _channel;
        private readonly dynamic _environment;

        public LeaseLock(IConfigurationSource configurationSource, ITemplatingEngine templatingEngine, IVariablesSource variables)
        {
            _templatingEngine = templatingEngine;

            _account = variables.GetAccountVariables(Account.Current());
            _channel = variables.GetChannelVariables(Channel.Current());
            _environment = variables.GetEnvironmentVariables(Environment.Current());

            var config = configurationSource.GetConfiguration<TimerConfig>();

            var account = CloudStorageAccount.Parse(ApplyTemplate(config.ConnectionString));

            var container = account.CreateCloudBlobClient().GetContainerReference("timerlocks");
            container.CreateIfNotExists();

            _streamId = Channel.Current() ?? Guid.NewGuid().ToString(); // guid when hosted locally outside channel
            _leaseBlob = container.GetBlockBlobReference(_streamId);
            _autoRenew = new AutoRenewLease(_leaseBlob);
        }

        public async Task<bool> ExecuteIfLeaseAcquired(Func<Task> func)
        {
            if (!await _leaseBlob.ExistsAsync())
            {
                await _leaseBlob.UploadTextAsync(_streamId);
            }

            using (var lease = await _autoRenew.TryAcquire())
            {
                if (lease.HasLease)
                    await func();
                
                return lease.HasLease;
            }
        }

        private string ApplyTemplate(string template)
        {
            return _templatingEngine.Apply(template, null, _channel, _environment, _account);
        }
    }
}