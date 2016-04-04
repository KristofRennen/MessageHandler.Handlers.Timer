﻿using MessageHandler;
using Quartz;
using Quartz.Spi;

namespace Timer
{
    public class ContainerJobFactory : IJobFactory
    {
        private readonly IContainer _container;

        public ContainerJobFactory(IContainer container)
        {
            _container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob)_container.Resolve(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}