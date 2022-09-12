using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using UrlGenerator.ExtensionMethods;
using UrlGenerator.Jobs;

namespace UrlGenerator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionScopedJobFactory();

                        // Register the job, loading the schedule from configuration
                        q.AddHelloWorldJobAndTrigger();
                        q.AddRequestUrlGeneratingJobAndTrigger();
                    });

                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
                });
        }
    }
}
