using Microsoft.Extensions.Configuration;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using UrlGenerator.Jobs;

namespace UrlGenerator.ExtensionMethods
{
    public static class ServiceCollectionQuartzConfiguratorExtensions
    {
        public static void AddHelloWorldJobAndTrigger(
            this IServiceCollectionQuartzConfigurator quartz)
        {
            string jobName = "HelloWorldJob";
            string cronSchedule = "0/5 * * * * ?";

            // register the job as before
            var jobKey = new JobKey(jobName);
            quartz.AddJob<HelloWorldJob>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithCronSchedule(cronSchedule)); // use the schedule from configuration
        }

        public static void AddRequestUrlGeneratingJobAndTrigger(
            this IServiceCollectionQuartzConfigurator quartz)
        {
            string jobName = "RequestUrlGeneratingJob";
            string cronSchedule = "0/5 * * * * ?";

            // register the job as before
            var jobKey = new JobKey(jobName);
            quartz.AddJob<RequestUrlGeneratingJob>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithCronSchedule(cronSchedule)); // use the schedule from configuration
        }
    }
}
